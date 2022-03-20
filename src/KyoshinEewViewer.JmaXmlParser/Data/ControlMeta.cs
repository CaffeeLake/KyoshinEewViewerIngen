using System;
using U8Xml;

namespace KyoshinEewViewer.JmaXmlParser.Data;

public class ControlMeta
{
	private XmlNode Node { get; set; }

	internal ControlMeta(XmlNode node)
	{
		Node = node;
	}

	private string? title;
	/// <summary>
	/// 包括的に電文の種別を示すための情報名称
	/// <para>種別が同一であれば常に同じ情報名称が記述される。<br/>
	/// 電文の処理系、及び配信系を制御するためのキーとして用いることを想定している</para>
	/// </summary>
	public string Title => title ??= (Node.TryFindStringNode(Literals.Title(), out var n) ? n : throw new JmaXmlParseException("Title ノードが存在しません"));

	private DateTimeOffset? datetime;
	/// <summary>
	/// 電文を作成、発信した実時刻
	/// <para>順序や同一性を検証するためのキーとして用いることを想定している</para>
	/// </summary>
	public DateTimeOffset DateTime => datetime ??= (Node.TryFindDateTimeNode(Literals.DateTime(), out var n) ? n : throw new JmaXmlParseException("DateTime ノードが存在しません"));

	private string? status;
	/// <summary>
	/// 電文の運用上の種別<br/>
	/// 原則として 2形態の表記法により表現する
	/// <list type="number">
	///		<item>
	///			「通常」「訓練」「試験」等の日本語形式
	///			<list type="bullet">
	///				<item>「通常」については、処理系・配信系の運用</item>
	///				<item>「訓練」については、業務訓練を想定した処理系の運用</item>
	///				<item>「試験」については、処理系の動作試験のための運用</item>
	///			</list>
	///		</item>
	///		<item>
	///			「CCA」等の英字形式<br/>
	///			現状の WMO の GTS 配信に則った運用
	///		</item>
	/// </list>
	/// どちらの形式により表現するかは、情報名称により一意に定まる
	/// </summary>
	public string Status => status ??= (Node.TryFindStringNode(Literals.Status(), out var n) ? n : throw new JmaXmlParseException("Status ノードが存在しません"));

	private string? editorialOffice;
	/// <summary>
	/// 電文を作成した機関(発信処理に関わった機関名称)<br/>
	/// 配信系で制御のキーとして用いることを想定している
	/// </summary>
	public string EditorialOffice => editorialOffice ??= (Node.TryFindStringNode(Literals.EditorialOffice(), out var n) ? n : throw new JmaXmlParseException("EditorialOffice ノードが存在しません"));

	private string? publishingOffice;
	/// <summary>
	/// 業務的に電文の作成に責任を持っている機関
	/// 配信系で制御のキーとして用いる際は <see cref="EditorialOffice"/> を使用する
	/// </summary>
	public string PublishingOffice => publishingOffice ??= (Node.TryFindStringNode(Literals.PublishingOffice(), out var n) ? n : throw new JmaXmlParseException("PublishingOffice ノードが存在しません"));
}
