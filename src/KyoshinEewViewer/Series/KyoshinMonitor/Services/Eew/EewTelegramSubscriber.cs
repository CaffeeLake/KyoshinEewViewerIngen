using Avalonia.Controls;
using KyoshinEewViewer.Core;
using KyoshinEewViewer.JmaXmlParser;
using KyoshinEewViewer.Series.KyoshinMonitor.Models;
using KyoshinEewViewer.Services;
using KyoshinMonitorLib;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace KyoshinEewViewer.Series.KyoshinMonitor.Services.Eew;
public class EewTelegramSubscriber : ReactiveObject
{
	private ILogger Logger { get; }
	private EewController EewController { get; }
	private TimerService Timer { get; }

	private bool _enabled;
	public bool Enabled
	{
		get => _enabled;
		set => this.RaiseAndSetIfChanged(ref _enabled, value);
	}

	private bool _warningOnlyEnabled;
	public bool WarningOnlyEnabled
	{
		get => _warningOnlyEnabled;
		set => this.RaiseAndSetIfChanged(ref _warningOnlyEnabled, value);
	}

	private bool _disconnected = true;
	public bool IsDisconnected
	{
		get => _disconnected;
		set => this.RaiseAndSetIfChanged(ref _disconnected, value);
	}

	public EewTelegramSubscriber(ILogManager logManager, EewController eewControlService, TelegramProvideService telegramProvider, TimerService timer)
	{
		Logger = logManager.GetLogger<EewTelegramSubscriber>();
		EewController = eewControlService;
		Timer = timer;

		telegramProvider.Subscribe(
			InformationCategory.EewForecast,
			(s, t) =>
			{
				// 有効になった
				Enabled = true;
				IsDisconnected = false;
				return Task.CompletedTask;
			},
			async t =>
			{
				var sw = Stopwatch.StartNew();
				// 受信した
				try
				{
					await using var stream = await t.GetBodyAsync();
					using var report = new JmaXmlDocument(stream);

					// サポート外であれば見なかったことにする
					if (report.Control.Title == "緊急地震速報配信テスト")
					{
						Logger.LogInfo($"dmdataから緊急地震速報のテスト電文を受信しました: {report.Head.EventId} / {report.Control.EditorialOffice}");
						return;
					}

					// 訓練･試験は今のところ非対応
					if (report.Control.Status != "通常")
						return;

					// 今のところ予報電文のみ対応
					if (report.Control.Title != "緊急地震速報（地震動予報）")
					{
						if (report.Control.Title != "緊急地震速報（予報）")
							Logger.LogWarning($"dmdataからEEW予報以外の電文を受信しました: {report.Control.Title}");
						return;
					}

					// 取消報
					if (report.Head.InfoType == "取消")
					{
						Logger.LogInfo($"dmdataからEEW取消報を受信しました: {report.Head.EventId}");
						EewController.Update(
							new TelegramForecastEew(report.Head.EventId, $"DM-D.S.S({report.Control.EditorialOffice})", true, t.ArrivalTime)
							{
								Count = int.TryParse(report.Head.Serial, out var c2) ? c2 : -1,
								UpdatedTime = Timer.CurrentTime,
							},
							t.ArrivalTime);
						return;
					}
					Logger.LogInfo($"dmdataからEEWを受信しました: {report.Head.EventId}");

					var earthquake = report.EarthquakeBody.Earthquake ?? throw new Exception("Earthquake 要素が見つかりません");
					var eew = new TelegramForecastEew(report.Head.EventId, $"DM-D.S.S({report.Control.EditorialOffice})", false, t.ArrivalTime)
					{
						Count = int.TryParse(report.Head.Serial, out var c) ? c : -1,
						IsTemporaryEpicenter = earthquake.Condition == "仮定震源要素",
						OccurrenceTime = earthquake.OriginTime?.DateTime ?? report.EarthquakeBody.Earthquake?.ArrivalTime?.DateTime ?? throw new Exception("OccurrenceTime が取得できません"),
						Place = earthquake.Hypocenter.Area.Name,
						Location = CoordinateConverter.GetLocation(earthquake.Hypocenter.Area.Coordinate.Value),
						Depth = CoordinateConverter.GetDepth(earthquake.Hypocenter.Area.Coordinate.Value) ?? -1,
						LocationAccuracy = earthquake.Hypocenter.Accuracy.EpicenterRank,
						DepthAccuracy = earthquake.Hypocenter.Accuracy.DepthRank,
						MagnitudeAccuracy = earthquake.Hypocenter.Accuracy.MagnitudeCalculationRank,
						Magnitude = earthquake.Magnitude.TryGetFloatValue(out var m) ? (float.IsNaN(m) ? null : m) : null,
						Intensity = report.EarthquakeBody.Intensity?.Forecast?.ForecastIntFrom.ToJmaIntensity() ?? JmaIntensity.Unknown,
						IsIntensityOver = report.EarthquakeBody.Intensity?.Forecast?.ForecastIntTo == "over",
						IsAccuracyFound = true,
						IsLocked = earthquake.Hypocenter.Accuracy.EpicenterRank2 == 9,
						IsFinal = report.EarthquakeBody.NextAdvisory == "この情報をもって、緊急地震速報：最終報とします。",
						IsWarning = report.EarthquakeBody.Comments?.WarningCommentCode?.Contains("0201") ?? false,
						UpdatedTime = Timer.CurrentTime,
					};
					try
					{
						eew.ForecastIntensityMap = report.EarthquakeBody.Intensity?.Forecast?.Prefs
							.SelectMany(p => p.Areas.Select(a => (a.Code, a.ForecastIntTo == "over" ? a.ForecastIntFrom.ToJmaIntensity() : a.ForecastIntTo.ToJmaIntensity())))
							.Where(a => a.Item2 != JmaIntensity.Unknown)
							.ToDictionary(k => k.Code, v => v.Item2);
						var warningAreas = report.EarthquakeBody.Intensity?.Forecast?.Prefs.SelectMany(p => p.Areas.Where(a => a.Category?.Kind.Code is "10" or "11" or "19"));
						if (warningAreas?.Any() ?? false)
						{
							eew.WarningAreaCodes = warningAreas?.Select(a => a.Code).ToArray();
							eew.WarningAreaNames = warningAreas?.Select(a => a.Name).ToArray();
						}
					}
					catch (Exception ex)
					{
						Logger.LogError(ex, "EEW電文予想震度処理中に例外が発生しました");
					}

					EewController.Update(eew, t.ArrivalTime);
				}
				catch (Exception ex)
				{
					Logger.LogError(ex, "EEW電文処理中に例外が発生しました");
				}
				finally
				{
					Logger.LogDebug($"dmdataEEW 処理時間: {sw.Elapsed.TotalMilliseconds:0.000}ms");
				}
			},
			s =>
			{
				// 死んだ
				Enabled = !s.isAllFailed;
				IsDisconnected = s.isRestorable;
			});
		telegramProvider.Subscribe(
			InformationCategory.EewWarning,
			(s, t) =>
			{
				WarningOnlyEnabled = !Enabled;
				IsDisconnected = false;
				return Task.CompletedTask;
			},
			async t =>
			{
				var sw = Stopwatch.StartNew();
				// 受信した
				try
				{
					await using var stream = await t.GetBodyAsync();
					using var report = new JmaXmlDocument(stream);

					// 訓練･試験は今のところ非対応
					if (report.Control.Status != "通常")
						return;

					// 今のところ予報電文のみ対応
					if (report.Control.Title != "緊急地震速報（警報）")
					{
						Logger.LogWarning($"dmdataからEEW警報以外の電文を受信しました: {report.Control.Title}");
						return;
					}

					// 取消報
					if (report.Head.InfoType == "取消")
					{
						Logger.LogInfo($"dmdataからEEW警報の取消報を受信しました: {report.Head.EventId}");
						EewController.Update(
							new TelegramForecastEew(report.Head.EventId, report.Control.EditorialOffice, true, t.ArrivalTime)
							{
								Count = int.TryParse(report.Head.Serial, out var c2) ? c2 : -1,
								UpdatedTime = Timer.CurrentTime,
							},
							t.ArrivalTime);
						return;
					}
					Logger.LogInfo($"dmdataからEEW警報を受信しました: {report.Head.EventId}");

					// 予報が有効な場合処理しない
					if (Enabled)
						return;

					var earthquake = report.EarthquakeBody.Earthquake ?? throw new Exception("Earthquake 要素が見つかりません");
					var warningAreas = report.EarthquakeBody.Intensity?.Forecast?.Prefs.SelectMany(p => p.Areas.Where(a => a.Category?.Kind.Code == "19"));
					var eew = new TelegramForecastEew(report.Head.EventId, $"DM-D.S.S({report.Control.EditorialOffice})", false, t.ArrivalTime)
					{
						Count = int.TryParse(report.Head.Serial, out var c) ? c : -1,
						OccurrenceTime = earthquake.OriginTime?.DateTime ?? report.EarthquakeBody.Earthquake?.ArrivalTime?.DateTime ?? throw new Exception("OccurrenceTime が取得できません"),
						Place = earthquake.Hypocenter.Area.Name,
						Location = CoordinateConverter.GetLocation(earthquake.Hypocenter.Area.Coordinate.Value),
						Intensity = report.EarthquakeBody.Intensity?.Forecast?.ForecastIntFrom.ToJmaIntensity() ?? JmaIntensity.Unknown,
						IsIntensityOver = report.EarthquakeBody.Intensity?.Forecast?.ForecastIntTo == "over",
						IsAccuracyFound = false,
						IsWarning = true,
						WarningAreaCodes = warningAreas?.Select(a => a.Code).ToArray(),
						WarningAreaNames = warningAreas?.Select(a => a.Name).ToArray(),
						UpdatedTime = Timer.CurrentTime,
					};

					EewController.UpdateWarningAreas(eew, t.ArrivalTime);
				}
				catch (Exception ex)
				{
					Logger.LogError(ex, "EEW電文処理中に例外が発生しました");
				}
				finally
				{
					Logger.LogDebug($"dmdataEEW 処理時間: {sw.Elapsed.TotalMilliseconds:0.000}ms");
				}
			},
			s =>
			{
				// 死んだ
				WarningOnlyEnabled = !s.isAllFailed && !Enabled;
				IsDisconnected = s.isRestorable;
			});

		if (Design.IsDesignMode)
			Enabled = true;
	}

	public class TelegramForecastEew(string id, string sourceDisplay, bool isCancelled, DateTime receiveTime) : IEew
	{
		public string Id { get; } = id;

		public string SourceDisplay { get; } = sourceDisplay;

		public bool IsCancelled { get; } = isCancelled;

		public bool IsTrueCancelled => IsCancelled;

		public DateTime ReceiveTime { get; } = receiveTime;

		public JmaIntensity Intensity { get; init; } = JmaIntensity.Unknown;

		public bool IsIntensityOver { get; init; }

		public DateTime OccurrenceTime { get; init; }

		public string? Place { get; init; }

		public KyoshinMonitorLib.Location? Location { get; init; }

		public float? Magnitude { get; init; }

		public int Depth { get; init; }

		public int Count { get; init; }

		public bool IsWarning { get; init; }

		public bool IsFinal { get; init; }

		public bool IsAccuracyFound { get; init; }

		public int? LocationAccuracy { get; set; }
		public int? DepthAccuracy { get; set; }
		public int? MagnitudeAccuracy { get; set; }

		public bool IsTemporaryEpicenter { get; init; }

		public bool? IsLocked { get; init; }

		/// <summary>
		/// 予想震度一覧
		/// </summary>
		public Dictionary<int, JmaIntensity>? ForecastIntensityMap { get; set; }

		/// <summary>
		/// 警報地域コード一覧
		/// </summary>
		public int[]? WarningAreaCodes { get; set; }

		/// <summary>
		/// 警報地域名一覧
		/// </summary>
		public string[]? WarningAreaNames { get; set; }

		public int Priority => 1;

		public DateTime UpdatedTime { get; set; }
		public bool IsVisible { get; set; } = true;
	}
}
