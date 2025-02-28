using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using Location = KyoshinMonitorLib.Location;

namespace KyoshinEewViewer.Core.Models;

public class KyoshinEewViewerConfiguration : ReactiveObject
{
	private bool _showWizard = true;
	public bool ShowWizard
	{
		get => _showWizard;
		set => this.RaiseAndSetIfChanged(ref _showWizard, value);
	}

	private double _windowScale = 1;
	public double WindowScale
	{
		get => _windowScale;
		set => this.RaiseAndSetIfChanged(ref _windowScale, value);
	}

	private WindowState _windowState = WindowState.Normal;
	public WindowState WindowState
	{
		get => _windowState;
		set => this.RaiseAndSetIfChanged(ref _windowState, value);
	}

	private Point2D? _windowSize;
	public Point2D? WindowSize
	{
		get => _windowSize;
		set => this.RaiseAndSetIfChanged(ref _windowSize, value);
	}

	private Point2D? _windowLocation;
	public Point2D? WindowLocation
	{
		get => _windowLocation;
		set => this.RaiseAndSetIfChanged(ref _windowLocation, value);
	}

	private string? _selectedTabName;
	public string? SelectedTabName
	{
		get => _selectedTabName;
		set => this.RaiseAndSetIfChanged(ref _selectedTabName, value);
	}

	private bool _autoProcessPriority = false;
	public bool AutoProcessPriority
	{
		get => _autoProcessPriority;
		set => this.RaiseAndSetIfChanged(ref _autoProcessPriority, value);
	}

	public record Point2D(double X, double Y);

	private Version? _savedVersion;
	public Version? SavedVersion
	{
		get => _savedVersion;
		set => this.RaiseAndSetIfChanged(ref _savedVersion, value);
	}

	private Dictionary<string, bool> _series = [];
	public Dictionary<string, bool> SeriesEnable
	{
		get => _series;
		set => this.RaiseAndSetIfChanged(ref _series, value);
	}

	private TimerConfig _timer = new();
	public TimerConfig Timer
	{
		get => _timer;
		set => this.RaiseAndSetIfChanged(ref _timer, value);
	}
	public class TimerConfig : ReactiveObject
	{
		private int _offset = 1100;
		public int Offset
		{
			get => _offset;
			set => this.RaiseAndSetIfChanged(ref _offset, value);
		}

		private bool _autoOffsetIncrement = true;
		public bool AutoOffsetIncrement
		{
			get => _autoOffsetIncrement;
			set => this.RaiseAndSetIfChanged(ref _autoOffsetIncrement, value);
		}
	}

	private KyoshinMonitorConfig _kyoshinMonitor = new();
	public KyoshinMonitorConfig KyoshinMonitor
	{
		get => _kyoshinMonitor;
		set => this.RaiseAndSetIfChanged(ref _kyoshinMonitor, value);
	}
	public class KyoshinMonitorConfig : ReactiveObject
	{
		private bool _useExperimentalShakeDetect = false;
		public bool UseExperimentalShakeDetect
		{
			get => _useExperimentalShakeDetect;
			set => this.RaiseAndSetIfChanged(ref _useExperimentalShakeDetect, value);
		}

		private KyoshinEventLevel _eventNotificationLevel = KyoshinEventLevel.Medium;
		public KyoshinEventLevel EventNotificationLevel
		{
			get => _eventNotificationLevel;
			set => this.RaiseAndSetIfChanged(ref _eventNotificationLevel, value);
		}

		private int _fetchFrequency = 1;
		public int FetchFrequency
		{
			get => _fetchFrequency;
			set => this.RaiseAndSetIfChanged(ref _fetchFrequency, value);
		}

		private bool _forcefetchOnEew;
		public bool ForcefetchOnEew
		{
			get => _forcefetchOnEew;
			set => this.RaiseAndSetIfChanged(ref _forcefetchOnEew, value);
		}

		private bool _switchAtShakeDetect;
		public bool SwitchAtShakeDetect
		{
			get => _switchAtShakeDetect;
			set => this.RaiseAndSetIfChanged(ref _switchAtShakeDetect, value);
		}

		private bool _showColorSample = true;
		public bool ShowColorSample
		{
			get => _showColorSample;
			set => this.RaiseAndSetIfChanged(ref _showColorSample, value);
		}

		private bool _keepReceiveDuringReplay = true;
		public bool KeepReceiveDuringReplay
		{
			get => _keepReceiveDuringReplay;
			set => this.RaiseAndSetIfChanged(ref _keepReceiveDuringReplay, value);
		}

		private bool _returnToRealtimeAtShakeDetected = true;
		public bool ReturnToRealtimeAtShakeDetected
		{
			get => _returnToRealtimeAtShakeDetected;
			set => this.RaiseAndSetIfChanged(ref _returnToRealtimeAtShakeDetected, value);
		}

		private bool _returnToRealtimeAtEewReceived = true;
		public bool ReturnToRealtimeAtEewReceived
		{
			get => _returnToRealtimeAtEewReceived;
			set => this.RaiseAndSetIfChanged(ref _returnToRealtimeAtEewReceived, value);
		}
	}

	private EewConfig _eew = new();
	public EewConfig Eew
	{
		get => _eew;
		set => this.RaiseAndSetIfChanged(ref _eew, value);
	}
	public class EewConfig : ReactiveObject
	{
		private bool _enableSignalNowProfessional;
		public bool EnableSignalNowProfessional
		{
			get => _enableSignalNowProfessional;
			set => this.RaiseAndSetIfChanged(ref _enableSignalNowProfessional, value);
		}
		private bool _enableSignalNowProfessionalLocation;
		public bool EnableSignalNowProfessionalLocation
		{
			get => _enableSignalNowProfessionalLocation;
			set => this.RaiseAndSetIfChanged(ref _enableSignalNowProfessionalLocation, value);
		}

		private bool _showDetails;
		public bool ShowDetails
		{
			get => _showDetails;
			set => this.RaiseAndSetIfChanged(ref _showDetails, value);
		}

		private bool _syncKyoshinMonitorPsWave;
		public bool SyncKyoshinMonitorPsWave
		{
			get => _syncKyoshinMonitorPsWave;
			set => this.RaiseAndSetIfChanged(ref _syncKyoshinMonitorPsWave, value);
		}

		private bool _fillWarningArea;
		public bool FillWarningArea
		{
			get => _fillWarningArea;
			set => this.RaiseAndSetIfChanged(ref _fillWarningArea, value);
		}

		private bool _fillForecastIntensity;
		public bool FillForecastIntensity
		{
			get => _fillForecastIntensity;
			set => this.RaiseAndSetIfChanged(ref _fillForecastIntensity, value);
		}

		private bool _switchAtAnnounce;
		public bool SwitchAtAnnounce
		{
			get => _switchAtAnnounce;
			set => this.RaiseAndSetIfChanged(ref _switchAtAnnounce, value);
		}

		private bool _disableAnimation;
		public bool DisableAnimation
		{
			get => _disableAnimation;
			set => this.RaiseAndSetIfChanged(ref _disableAnimation, value);
		}
	}

	private ThemeConfig _theme = new();
	public ThemeConfig Theme
	{
		get => _theme;
		set => this.RaiseAndSetIfChanged(ref _theme, value);
	}
	public class ThemeConfig : ReactiveObject
	{
		private string _windowThemeName = "Light";
		public string WindowThemeName
		{
			get => _windowThemeName;
			set => this.RaiseAndSetIfChanged(ref _windowThemeName, value);
		}
		private string _intensityThemeName = "Standard";
		public string IntensityThemeName
		{
			get => _intensityThemeName;
			set => this.RaiseAndSetIfChanged(ref _intensityThemeName, value);
		}
	}

	private NetworkTimeConfig _networkTime = new();
	public NetworkTimeConfig NetworkTime
	{
		get => _networkTime;
		set => this.RaiseAndSetIfChanged(ref _networkTime, value);
	}
	public class NetworkTimeConfig : ReactiveObject
	{
		private bool _enable = true;
		public bool Enable
		{
			get => _enable;
			set => this.RaiseAndSetIfChanged(ref _enable, value);
		}
		private string _address = "time.google.com";
		public string Address
		{
			get => _address;
			set => this.RaiseAndSetIfChanged(ref _address, value);
		}
		private bool _enableFallbackHttp = true;
		public bool EnableFallbackHttp
		{
			get => _enableFallbackHttp;
			set => this.RaiseAndSetIfChanged(ref _enableFallbackHttp, value);
		}
	}

	private LoggingConfig _logging = new();
	public LoggingConfig Logging
	{
		get => _logging;
		set => this.RaiseAndSetIfChanged(ref _logging, value);
	}
	public class LoggingConfig : ReactiveObject
	{
		private bool _enable = false;
		public bool Enable
		{
			get => _enable;
			set => this.RaiseAndSetIfChanged(ref _enable, value);
		}
		private string _directory = "Logs";
		public string Directory
		{
			get => _directory;
			set => this.RaiseAndSetIfChanged(ref _directory, value);
		}
	}

	private UpdateConfig _update = new();
	public UpdateConfig Update
	{
		get => _update;
		set => this.RaiseAndSetIfChanged(ref _update, value);
	}
	public class UpdateConfig : ReactiveObject
	{
		private bool _enable = true;
		public bool Enable
		{
			get => _enable;
			set => this.RaiseAndSetIfChanged(ref _enable, value);
		}

		private bool _usePreReleaseBuild = false;
		public bool UsePreReleaseBuild
		{
			get => _usePreReleaseBuild;
			set => this.RaiseAndSetIfChanged(ref _usePreReleaseBuild, value);
		}

		private bool _useUnstableBuild;
		public bool UseUnstableBuild
		{
			get => _useUnstableBuild;
			set => this.RaiseAndSetIfChanged(ref _useUnstableBuild, value);
		}

		private bool _sendCrashReport = true;
		public bool SendCrashReport
		{
			get => _sendCrashReport;
			set => this.RaiseAndSetIfChanged(ref _sendCrashReport, value);
		}
	}

	private NotificationConfig _notification = new();
	public NotificationConfig Notification
	{
		get => _notification;
		set => this.RaiseAndSetIfChanged(ref _notification, value);
	}
	public class NotificationConfig : ReactiveObject
	{
		private bool _trayIconEnable = true;
		public bool TrayIconEnable
		{
			get => _trayIconEnable;
			set => this.RaiseAndSetIfChanged(ref _trayIconEnable, value);
		}
		private bool _hideWhenMinimizeWindow = true;
		public bool HideWhenMinimizeWindow
		{
			get => _hideWhenMinimizeWindow;
			set => this.RaiseAndSetIfChanged(ref _hideWhenMinimizeWindow, value);
		}
		private bool _hideWhenClosingWindow;
		public bool HideWhenClosingWindow
		{
			get => _hideWhenClosingWindow;
			set => this.RaiseAndSetIfChanged(ref _hideWhenClosingWindow, value);
		}

		private bool _minimizeWindowOnStartup;
		public bool MinimizeWindowOnStartup
		{
			get => _minimizeWindowOnStartup;
			set => this.RaiseAndSetIfChanged(ref _minimizeWindowOnStartup, value);
		}

		private bool _enable = true;
		public bool Enable
		{
			get => _enable;
			set => this.RaiseAndSetIfChanged(ref _enable, value);
		}
		private bool _switchEqSource = true;
		public bool SwitchEqSource
		{
			get => _switchEqSource;
			set => this.RaiseAndSetIfChanged(ref _switchEqSource, value);
		}
		private bool _gotEq = true;
		public bool GotEq
		{
			get => _gotEq;
			set => this.RaiseAndSetIfChanged(ref _gotEq, value);
		}
		private bool _eewReceived = true;
		public bool EewReceived
		{
			get => _eewReceived;
			set => this.RaiseAndSetIfChanged(ref _eewReceived, value);
		}
		private bool _tsunami = true;
		public bool Tsunami
		{
			get => _tsunami;
			set => this.RaiseAndSetIfChanged(ref _tsunami, value);
		}
	}

	private MapConfig _map = new();
	public MapConfig Map
	{
		get => _map;
		set => this.RaiseAndSetIfChanged(ref _map, value);
	}
	public class MapConfig : ReactiveObject
	{
		private bool _disableManualMapControl;
		public bool DisableManualMapControl
		{
			get => _disableManualMapControl;
			set => this.RaiseAndSetIfChanged(ref _disableManualMapControl, value);
		}
		private bool _keepRegion;
		public bool KeepRegion
		{
			get => _keepRegion;
			set => this.RaiseAndSetIfChanged(ref _keepRegion, value);
		}
		private bool _autoFocus = true;
		public bool AutoFocus
		{
			get => _autoFocus;
			set => this.RaiseAndSetIfChanged(ref _autoFocus, value);
		}
		private double _maxNavigateZoom = 8.5;
		public double MaxNavigateZoom
		{
			get => _maxNavigateZoom;
			set => this.RaiseAndSetIfChanged(ref _maxNavigateZoom, value);
		}
		private bool _showGrid = false;
		public bool ShowGrid
		{
			get => _showGrid;
			set => this.RaiseAndSetIfChanged(ref _showGrid, value);
		}

		private Location _location1 = new(45.619358f, 145.77399f);
		public Location Location1
		{
			get => _location1;
			set => this.RaiseAndSetIfChanged(ref _location1, value);
		}
		private Location _location2 = new(29.997368f, 128.22534f);
		public Location Location2
		{
			get => _location2;
			set => this.RaiseAndSetIfChanged(ref _location2, value);
		}

		private bool _autoFocusAnimation = true;
		public bool AutoFocusAnimation
		{
			get => _autoFocusAnimation;
			set => this.RaiseAndSetIfChanged(ref _autoFocusAnimation, value);
		}

		private bool _useMiniMap = true;
		public bool UseMiniMap
		{
			get => _useMiniMap;
			set => this.RaiseAndSetIfChanged(ref _useMiniMap, value);
		}
	}

	private DmdataConfig _dmdata = new();
	public DmdataConfig Dmdata
	{
		get => _dmdata;
		set => this.RaiseAndSetIfChanged(ref _dmdata, value);
	}
	public class DmdataConfig : ReactiveObject
	{
		public const string DefaultOAuthClientId = "CId._xg46xWbfdrOqxN7WtwNfBUL3fhKLH9roksSfV8RV3Nj";
		private string _oAuthClientId = DefaultOAuthClientId;
		public string OAuthClientId
		{
			get => _oAuthClientId;
			set => this.RaiseAndSetIfChanged(ref _oAuthClientId, value);
		}
		private string? _oAuthClientSecret;
		public string? OAuthClientSecret
		{
			get => _oAuthClientSecret;
			set => this.RaiseAndSetIfChanged(ref _oAuthClientSecret, value);
		}
		private string? _refreshToken;
		public string? RefreshToken
		{
			get => _refreshToken;
			set => this.RaiseAndSetIfChanged(ref _refreshToken, value);
		}
		private bool _receiveTraining;
		public bool ReceiveTraining
		{
			get => _receiveTraining;
			set => this.RaiseAndSetIfChanged(ref _receiveTraining, value);
		}
		private bool _useWebSocket = true;
		public bool UseWebSocket
		{
			get => _useWebSocket;
			set => this.RaiseAndSetIfChanged(ref _useWebSocket, value);
		}
		private float _pullMultiply = 1;
		public float PullMultiply
		{
			get => _pullMultiply;
			set => this.RaiseAndSetIfChanged(ref _pullMultiply, value);
		}
	}

	private EarthquakeConfig _earthquake = new();
	public EarthquakeConfig Earthquake
	{
		get => _earthquake;
		set => this.RaiseAndSetIfChanged(ref _earthquake, value);
	}
	public class EarthquakeConfig : ReactiveObject
	{
		private bool _fillSokuhou = true;
		public bool FillSokuhou
		{
			get => _fillSokuhou;
			set => this.RaiseAndSetIfChanged(ref _fillSokuhou, value);
		}
		private bool _fillDetail = false;
		public bool FillDetail
		{
			get => _fillDetail;
			set => this.RaiseAndSetIfChanged(ref _fillDetail, value);
		}

		private bool _showHistory = true;
		public bool ShowHistory
		{
			get => _showHistory;
			set => this.RaiseAndSetIfChanged(ref _showHistory, value);
		}

		private bool _switchAtUpdate;
		public bool SwitchAtUpdate
		{
			get => _switchAtUpdate;
			set => this.RaiseAndSetIfChanged(ref _switchAtUpdate, value);
		}
	}

	private TsunamiConfig _tsunami = new();
	public TsunamiConfig Tsunami
	{
		get => _tsunami;
		set => this.RaiseAndSetIfChanged(ref _tsunami, value);
	}
	public class TsunamiConfig : ReactiveObject
	{
		private bool _switchAtUpdate;
		public bool SwitchAtUpdate
		{
			get => _switchAtUpdate;
			set => this.RaiseAndSetIfChanged(ref _switchAtUpdate, value);
		}
	}

	private RadarConfig _radar = new();
	public RadarConfig Radar
	{
		get => _radar;
		set => this.RaiseAndSetIfChanged(ref _radar, value);
	}
	public class RadarConfig : ReactiveObject
	{
		private bool _autoUpdate = true;
		public bool AutoUpdate
		{
			get => _autoUpdate;
			set => this.RaiseAndSetIfChanged(ref _autoUpdate, value);
		}
	}

	private RawIntensityObjectConfig _rawIntensityObject = new();
	public RawIntensityObjectConfig RawIntensityObject
	{
		get => _rawIntensityObject;
		set => this.RaiseAndSetIfChanged(ref _rawIntensityObject, value);
	}
	public class RawIntensityObjectConfig : ReactiveObject
	{
		private double _showNameZoomLevel = 9;
		public double ShowNameZoomLevel
		{
			get => _showNameZoomLevel;
			set => this.RaiseAndSetIfChanged(ref _showNameZoomLevel, value);
		}

		private double _minShownIntensity = -3;
		public double MinShownIntensity
		{
			get => _minShownIntensity;
			set => this.RaiseAndSetIfChanged(ref _minShownIntensity, value);
		}

		private double _minShownDetailIntensity = -3;
		public double MinShownDetailIntensity
		{
			get => _minShownDetailIntensity;
			set => this.RaiseAndSetIfChanged(ref _minShownDetailIntensity, value);
		}

		private bool _showInvalidateIcon = true;
		public bool ShowInvalidateIcon
		{
			get => _showInvalidateIcon;
			set => this.RaiseAndSetIfChanged(ref _showInvalidateIcon, value);
		}
	}

	private AudioConfig _audio = new();
	public AudioConfig Audio
	{
		get => _audio;
		set => this.RaiseAndSetIfChanged(ref _audio, value);
	}
	public class AudioConfig : ReactiveObject
	{
		private double _globalVolume = 1;
		public double GlobalVolume
		{
			get => _globalVolume;
			set => this.RaiseAndSetIfChanged(ref _globalVolume, value);
		}
	}

	private Dictionary<string, Dictionary<string, SoundConfig>> _sounds = [];
	public Dictionary<string, Dictionary<string, SoundConfig>> Sounds
	{
		get => _sounds;
		set => this.RaiseAndSetIfChanged(ref _sounds, value);
	}
	public class SoundConfig : ReactiveObject
	{
		private bool _enabled = false;
		public bool Enabled
		{
			get => _enabled;
			set => this.RaiseAndSetIfChanged(ref _enabled, value);
		}
		private string _filePath = "";
		public string FilePath
		{
			get => _filePath;
			set => this.RaiseAndSetIfChanged(ref _filePath, value);
		}
		private double _volume = 1;
		public double Volume
		{
			get => _volume;
			set => this.RaiseAndSetIfChanged(ref _volume, value);
		}
		private bool _allowMultiPlay = false;
		public bool AllowMultiPlay
		{
			get => _allowMultiPlay;
			set => this.RaiseAndSetIfChanged(ref _allowMultiPlay, value);
		}
	}

	private QzssConfig _qzss = new();
	public QzssConfig Qzss
	{
		get => _qzss;
		set => this.RaiseAndSetIfChanged(ref _qzss, value);
	}
	public class QzssConfig : ReactiveObject
	{
		private bool _connect = false;
		public bool Connect
		{
			get => _connect;
			set => this.RaiseAndSetIfChanged(ref _connect, value);
		}

		private string _serialPort = "";
		public string SerialPort
		{
			get => _serialPort;
			set => this.RaiseAndSetIfChanged(ref _serialPort, value);
		}

		private int _baudRate = 115200;
		public int BaudRate
		{
			get => _baudRate;
			set => this.RaiseAndSetIfChanged(ref _baudRate, value);
		}

		private bool _showCurrentPositionInMap = false;
		public bool ShowCurrentPositionInMap
		{
			get => _showCurrentPositionInMap;
			set => this.RaiseAndSetIfChanged(ref _showCurrentPositionInMap, value);
		}

		private bool _hidePositionNumber = true;
		public bool HidePositionNumber
		{
			get => _hidePositionNumber;
			set => this.RaiseAndSetIfChanged(ref _hidePositionNumber, value);
		}

		private bool _ignoreOtherOrganizationReport = true;
		public bool IgnoreOtherOrganizationReport
		{
			get => _ignoreOtherOrganizationReport;
			set => this.RaiseAndSetIfChanged(ref _ignoreOtherOrganizationReport, value);
		}

		private bool _ignoreTrainingOrTestReport = true;
		public bool IgnoreTrainingOrTestReport
		{
			get => _ignoreTrainingOrTestReport;
			set => this.RaiseAndSetIfChanged(ref _ignoreTrainingOrTestReport, value);
		}
	}

	private VoicevoxConfig _voicevox = new();
	public VoicevoxConfig Voicevox
	{
		get => _voicevox;
		set => this.RaiseAndSetIfChanged(ref _voicevox, value);
	}
	public class VoicevoxConfig : ReactiveObject
	{
		private bool _enabled = false;
		public bool Enabled
		{
			get => _enabled;
			set => this.RaiseAndSetIfChanged(ref _enabled, value);
		}

		private string _address = "http://localhost:50021/";
		public string Address
		{
			get => _address;
			set => this.RaiseAndSetIfChanged(ref _address, value);
		}

		private int _speakerId = 2;
		public int SpeakerId
		{
			get => _speakerId;
			set => this.RaiseAndSetIfChanged(ref _speakerId, value);
		}

		private float _speedScale = 1;
		public float SpeedScale
		{
			get => _speedScale;
			set => this.RaiseAndSetIfChanged(ref _speedScale, value);
		}

		private float _pitchScale = 0;
		public float PitchScale
		{
			get => _pitchScale;
			set => this.RaiseAndSetIfChanged(ref _pitchScale, value);
		}

		private float _intonationScale = 1;
		public float IntonationScale
		{
			get => _intonationScale;
			set => this.RaiseAndSetIfChanged(ref _intonationScale, value);
		}

		private float _volumeScale = 1;
		public float VolumeScale
		{
			get => _volumeScale;
			set => this.RaiseAndSetIfChanged(ref _volumeScale, value);
		}
	}
}
