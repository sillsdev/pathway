// --------------------------------------------------------------------------------------------
// <copyright file="UsxToOSIS.cs" from='2009' to='2014' company='SIL International'>
//      Copyright ( c ) 2014, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Converts USX to OSIS
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;
using SIL.Tool;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Xml.Xsl;

namespace SIL.PublishingSolution
{
	public class UsxToOSIS
	{
		#region Private Variable

		private Stack<string> _allStyle = new Stack<string>();
		private Stack<string> _alltagName = new Stack<string>();

		private string _usxFullPath, _osisFullPath;

		private XmlTextReader _reader;
		private XmlTextWriter _writer;

		private Dictionary<string, string> _bookCode = new Dictionary<string, string>();

		private string _bookCodeName;

		private Dictionary<string, string> _mapClassName = new Dictionary<string, string>();

		private string _tagName, _style, _number, _caller, _content;
		private string _parentTagName = string.Empty, _parentStyleName = string.Empty;
		private string _verseNumber, _chapterNumber;
		private string _paraStyle = string.Empty;
		private bool _skipTag = false;

		private string _desc, _file, _size, _loc, _copy, _ref;
		private bool _significant, _isEmptyNode, _isParaWritten;
		private const string Space = " ";
		private const string Bar = "|";

		private List<string> _xhtmlAttribute = new List<string>();
		private bool _listItemOpen = false;
		private int _openDivCount = 0;
		private bool _isFirstScope = true;
		private Dictionary<string, string> _mappingScopeChapterandVerse = new Dictionary<string, string>();
		private string _scopeKey = string.Empty;
		private List<string> _scopeChapterandVerse = new List<string>();
		#endregion

		/// <summary>
		/// Entry method to convert USX to OSIS
		/// </summary>
		public void ConvertUsxToOSIS(string usxFullPath, string OSISFullPath, string xhtmlLang)
		{
			_usxFullPath = usxFullPath;
			_osisFullPath = OSISFullPath;
			SetBookCode();
			OpenFile();
			CreateHead(xhtmlLang);
			ProcessUsx();
			CloseFile();
			UpdateScopeProcess();
		}

		public void ConvertUsxToOSIS(string usxFullPath, string OSISFullPath)
		{
			
			_usxFullPath = usxFullPath;
			_osisFullPath = OSISFullPath;
			CorrectTheBookCodeProcess();
			ProcessUsxtoOsis(usxFullPath, OSISFullPath);
			UpdateScopeProcess();
		}

		/// ------------------------------------------------------------------------
		/// <summary>
		/// Parses all the lines in the usx file.
		/// </summary>
		/// ------------------------------------------------------------------------
		private void ProcessUsx()
		{
			try
			{
				bool headXML = true;
				while (_reader.Read())
				{
					if (headXML) // skip previous parts of <body> tag
					{
						if (_reader.Name == "para")
						{
							headXML = false;
						}
						else
						{
							if (_reader.Name == "book")
							{
								Book();
							}
							continue;
						}
					}

					if (_reader.IsEmptyElement)
					{
						_isEmptyNode = true;
					}
					switch (_reader.NodeType)
					{
						case XmlNodeType.Element:
							StartElement();
							break;
						case XmlNodeType.Text:
							WriteElement();
							break;
						case XmlNodeType.EndElement:
							EndElement();
							break;
					}
				}
			}
			catch (XmlException e)
			{
				Console.WriteLine(e.Message);
			}

			_reader.Close();
		}

		/// <summary>
		/// Write content
		/// </summary>
		private void WriteElement()
		{
			if (_skipTag)
			{
				return;
			}

			_content = SignificantSpace(_reader.Value);

			if (_tagName == "para" && _style == "h")
			{
				_writer.WriteAttributeString("short", _content);
				_style = string.Empty;
			}
			else if (_tagName == "char" && _style == "bk")
			{
				_writer.WriteAttributeString("type", "x-bookName");
				_writer.WriteString(_content);
				_style = string.Empty;
			}
			else if (_tagName == "char" && _style == "nd")
			{
				_writer.WriteString(_content);
				_writer.WriteEndElement();
				_style = string.Empty;
			}
			else if (_tagName == "para" && _style == "r")
			{
				_writer.WriteString(_content);
				_writer.WriteEndElement();
				_style = string.Empty;
			}
			else if (_tagName == "para" && _style == "rem")
			{
				_writer.WriteComment("\\" + _style + " " + _content);
				_style = string.Empty;
			}
			else if (_tagName == "para" && (_style == "mr" || _style == "imt" || _style == "mt" || _style == "mt1"))
			{
				_writer.WriteString(_content);
				_writer.WriteEndElement();
				_style = string.Empty;
			}
			else
			{
				_writer.WriteString(_content);
			}

		}

		/// <summary>
		/// input: <book code="RUT" style="id">TestName</book>
		/// output: \id RUT TestName
		/// </summary>
		private void Book()
		{
			if (_reader.HasAttributes)
			{
				while (_reader.MoveToNextAttribute())
				{
					if (_reader.Name == "code" || _reader.Name == "id")
					{
						if (_bookCode.ContainsKey(_reader.Value.ToLower()))
						{
							_bookCodeName = _bookCode[_reader.Value.ToLower()];
						}
						else
						{
							_bookCodeName = _reader.Value;
						}
						_writer.WriteAttributeString("osisID", _bookCodeName);
						break;
					}
				}
			}
		}


		/// <summary>
		/// input:  <figure style="fig" desc="Map" file="Ruth.pdf" size="col" loc="" copy="" ref="RUT 1.0">Israel</figure>
		/// output: \imt Kata-Kata Partama
		/// </summary>
		private void Content()
		{
			string line = _content + Space;
			_writer.WriteString(line);
		}


		/// <summary>
		/// input:  <figure style="fig" desc="Map" file="Ruth.pdf" size="col" loc="" copy="" ref="RUT 1.0">Israel</figure>
		/// output: \imt Kata-Kata Partama
		/// </summary>
		private void Para()
		{
			string prefix = string.Empty;
			if (_style != string.Empty)
			{
				prefix = "\\" + _style + Space;
			}
			string line = prefix + _content + EndText();

			string bookName = string.Empty;
			if (_style == "h")
			{
				bookName = _content;
				Common.BookNameTag = "h";
				if (Common.BookNameCollection.Contains(bookName) == false && bookName != string.Empty)
				{
					//Common.BookNameCollection.Remove(_code);
					//Common.BookNameCollection.Add(bookName);
				}
			}

			_writer.WriteString(line);
		}

		/// <summary>
		/// input:  <verse number="1" style="v">abc </verse>
		/// output: \v 1 abc
		/// </summary>
		private void Verse()
		{
			string line;
			if (_isParaWritten == false)
			{
				line = "\\" + _parentStyleName;
				_writer.WriteString(line);
				_isParaWritten = true;
				_verseNumber = string.Empty;
			}


			bool isWritten = HandleBridgeVerseNumbers(_number);

			if (!isWritten)
			{
				line = "\\" + _style + Space + _number + Space + _content.Trim() + EndText();
				_writer.WriteString(line);
				_verseNumber = string.Empty;
			}

		}

		private bool HandleBridgeVerseNumbers(string verseNumber)
		{
			bool isWritten = false;
			StringBuilder sb = new StringBuilder();
			string[] bridgeVerses = verseNumber.Split('-');

			bool isNumeric = true;
			foreach (string bridgeVerseNo in bridgeVerses)
			{
				if (!Common.ValidateNumber(bridgeVerseNo))
				{
					isNumeric = false;
				}
				break;
			}


			if (bridgeVerses.Length == 2 && isNumeric)
			{
				int verseFrom = int.Parse(bridgeVerses[0]);

				if (Common.IsAlphanumeric(bridgeVerses[1]))
				{
					if (bridgeVerses[1].IndexOf("a") > 0)
					{
						int verseAlpha = Common.GetNumberFromAlphaNumeric(bridgeVerses[1]);
						for (int cntVal = verseFrom; cntVal < verseAlpha; cntVal++)
						{
							sb.AppendLine("\\" + _style + Space + cntVal + Space + "..." + EndText());
						}
						sb.AppendLine("\\" + _style + Space + verseAlpha + Space + _content.Trim() + EndText());
						_writer.WriteString(sb.ToString());
					}
				}
				else
				{
					int verseAlpha = int.Parse(bridgeVerses[1]);
					for (int cntVal = verseFrom; cntVal < verseAlpha; cntVal++)
					{
						sb.AppendLine("\\" + _style + Space + cntVal + Space + "..." + EndText());
					}
					sb.AppendLine("\\" + _style + Space + verseAlpha + Space + _content.Trim() + EndText());
					_writer.WriteString(sb.ToString());
				}
				isWritten = true;
			}
			else if (bridgeVerses.Length == 1 && Common.IsAlphanumeric(bridgeVerses[0]))
			{
				if (bridgeVerses[0].IndexOf("a") > 0)
				{
					int verseAlpha = Common.GetNumberFromAlphaNumeric(bridgeVerses[0]);
					sb.AppendLine("\\" + _style + Space + verseAlpha + Space + _content.Trim() + EndText());
				}
				else
				{
					sb.AppendLine(_content.Trim() + EndText());
				}
				_writer.WriteString(sb.ToString());
				isWritten = true;
			}
			else if (isNumeric == false)
			{
				sb.AppendLine("\\" + _style + Space + verseNumber + Space + _content.Trim() + EndText());
				_writer.WriteString(sb.ToString());
				isWritten = true;
			}
			return isWritten;
		}

		/// <summary>
		/// </summary>
		private void Chapter()
		{
			string line = "\\" + _style + Space + _number;
			_writer.WriteString(line);
		}

		/// <summary>
		/// input:  <figure style="fig" desc="Map" file="Ruth.pdf" size="col" loc="" copy="" ref="RUT 1.0">Israel</figure>
		/// output: \fig Map|Ruth.pdf|col|||Israel|RUT 1.0\fig*
		/// </summary>
		private void Figure()
		{
			string line = "\\" + _style + Space + _desc + Bar + _file + Bar + _size + Bar + _loc + Bar + _copy + Bar + _content + Bar + _ref + EndText();
			_writer.WriteString(line);
		}


		/// <summary>
		/// input:  
		/// output: 
		/// </summary>
		private void Char()
		{
			string prefix = string.Empty;
			if (_style != string.Empty)
			{
				prefix = Space + "\\" + _style + Space;
			}

			if (_parentTagName == "note")
			{
				string line = prefix + _content;
				_writer.WriteString(line);
			}
			else
			{
				string line = prefix + _content + EndText();
				_writer.WriteString(line);
			}
		}

		private string EndText()
		{
			string endText = string.Empty;
			if (_tagName == "char" || _tagName == "figure" || _tagName == "note")
			{
				endText = "\\" + StackPeek(_allStyle) + "*";
			}

			return endText;
		}

		/// <summary>
		/// Clear Stack entry
		/// </summary>
		private void EndElement()
		{
			StackPop(_allStyle);
			StackPop(_alltagName);
			if (_skipTag)
			{
				_skipTag = false;
				return;
			}

			if (_tagName == "char" && _style == "xt")
			{
				_style = string.Empty;
				return;
			}

			if (_tagName == "verse")
			{
				string booksID = _bookCodeName + "." + _chapterNumber + "." + _verseNumber;
				_writer.WriteStartElement("verse");
				_writer.WriteAttributeString("eID", booksID);
				_writer.WriteEndElement();
				_tagName = string.Empty;
			}
			if (_paraStyle != "rem")
			{
				_writer.WriteEndElement();
			}
		}

		/// <summary>
		/// Collect Tag Information
		/// </summary>
		private void StartElement()
		{
			_xhtmlAttribute.Clear();
			_number = string.Empty;

			_parentStyleName = StackPeek(_allStyle);
			_parentTagName = StackPeek(_alltagName);

			_style = _tagName = _reader.Name;

			//Read Attributes
			if (_reader.HasAttributes)
			{
				WriteStyle();
			}

			if (_listItemOpen && _style != "io1")
			{
				_writer.WriteEndElement();
				_listItemOpen = false;
			}
			//Write Start Element
			if (_tagName == "para")
			{
				WriteParagraphStyle();
			}
			else if (_tagName == "chapter")
			{
				for (int i = 1; i <= _openDivCount; i++)
				{
					_writer.WriteEndElement();
				}
				_openDivCount = 0;

				_writer.WriteStartElement("chapter");
				_writer.WriteAttributeString("n", _chapterNumber);
				_writer.WriteAttributeString("osisID", _bookCodeName + "." + _chapterNumber);
			}
			else if (_tagName == "verse")
			{
				WriteVerse();
			}
			else if (_tagName == "note")
			{
				_writer.WriteStartElement("note");
				_writer.WriteAttributeString("type", "crossReference");
				_writer.WriteAttributeString("n", _caller);
			}
			else if (_tagName == "char")
			{
				if (_style == "bk")
				{
					_writer.WriteStartElement("reference");
				}
				else if (_style == "xo")
				{
					_writer.WriteStartElement("reference");
					_writer.WriteAttributeString("type", "source");
				}
				else if (_style == "xt")
				{
					return;
				}
				else if (_style == "nd")
				{
					_writer.WriteStartElement("seg");
					_writer.WriteStartElement("divineName");
				}
				else
				{
					_writer.WriteStartElement("char");
				}
			}
			else
			{
				_writer.WriteStartElement(_tagName);
			}
			//Write Attributes

			if (_isEmptyNode)
			{
				_writer.WriteEndElement();
				_isEmptyNode = false;
			}
			else
			{
				_allStyle.Push(_style);
				_alltagName.Push(_tagName);
			}
		}

		private void WriteStyle()
		{
			while (_reader.MoveToNextAttribute())
			{
				if (_reader.Name == "style")
				{
					_style = _reader.Value;
				}
				else if (_reader.Name == "number")
				{
					_number = _reader.Value;
					if (_tagName == "verse")
					{
						_verseNumber = _number;
					}
					else
					{
						_chapterNumber = _number;
					}
				}
				else if (_reader.Name == "code")
				{
					//_writer.WriteAttributeString(_reader.Name, _reader.Value);
				}
				else if (_reader.Name == "id")
				{
					//_writer.WriteAttributeString("osisID", _reader.Value);
				}
				else if (_reader.Name == "caller")
				{
					_caller = _reader.Value;
					//_writer.WriteAttributeString(_reader.Name, _reader.Value);
				}
			}
		}

		private void WriteVerse()
		{
			string booksID = _bookCodeName + "." + _chapterNumber + "." + _verseNumber;
			_scopeChapterandVerse.Add(booksID);
			_writer.WriteStartElement("verse");

			if (_verseNumber == "1")
			{
				_writer.WriteAttributeString("subType", "x-first");
			}
			else
			{
				_writer.WriteAttributeString("subType", "x-embedded");
			}
			_writer.WriteAttributeString("sID", booksID);
			_writer.WriteAttributeString("n", _verseNumber);
			_writer.WriteAttributeString("osisID", booksID);
		}

		private void WriteParagraphStyle()
		{
			_paraStyle = _style;

			if (_style == "s")
			{
				string booksID = _bookCodeName + "." + _chapterNumber + "." + _verseNumber;

				if (!_isFirstScope)
				{
					_writer.WriteEndElement();
					if (_scopeKey != string.Empty && !_mappingScopeChapterandVerse.ContainsKey(_scopeKey) && _scopeChapterandVerse.Count > 0)
					{
						_mappingScopeChapterandVerse.Add(_scopeKey, _scopeChapterandVerse[0].ToString() + "-" + _scopeChapterandVerse[_scopeChapterandVerse.Count - 1].ToString());
					}
					_scopeKey = string.Empty;
					_scopeChapterandVerse.Clear();
				}
				_scopeKey = booksID;
				_writer.WriteStartElement("div");
				_writer.WriteAttributeString("scope", booksID);
				_writer.WriteAttributeString("type", "section");

				_writer.WriteStartElement("title");
				_writer.WriteAttributeString("level", "1");
				_isFirstScope = false;
			}
			else if (_style == "h" || _style == "is" || _style == "title")
			{
				_writer.WriteStartElement("title");
			}
			else if (_style == "iot")
			{
				_writer.WriteStartElement("div");
				_writer.WriteAttributeString("type", "outline");
				_openDivCount++;

				_writer.WriteStartElement("title");
			}
			else if (_style == "toc1" || _style == "toc2" || _style == "toc3")
			{
				_skipTag = true;
			}
			else if (_style == "imt")
			{
				_writer.WriteStartElement("div");
				_writer.WriteAttributeString("type", "introduction");
				_writer.WriteAttributeString("canonical", "false");
				_openDivCount++;

				_writer.WriteStartElement("title");
				_writer.WriteAttributeString("type", "main");
				_writer.WriteStartElement("title");
				_writer.WriteAttributeString("level", "1");
			}
			else if (_style == "mt" || _style == "mt1")
			{
				_writer.WriteStartElement("title");
				_writer.WriteAttributeString("type", "main");

				_writer.WriteStartElement("title");
				_writer.WriteAttributeString("level", "1");
			}
			else if (_style == "mt2")
			{
				_writer.WriteStartElement("title");
				_writer.WriteAttributeString("level", "2");
			}
			else if (_style == "im")
			{
				_writer.WriteStartElement("p");
				_writer.WriteAttributeString("type", "x-continued");
			}
			else if (_style == "io1")
			{
				if (_listItemOpen == false)
				{
					_writer.WriteStartElement("list");
					_listItemOpen = true;
				}
				_writer.WriteStartElement("item");
			}
			else if (_style == "p")
			{
				_writer.WriteStartElement("p");
			}
			else if (_style == "rem")
			{
				//_writer.WriteStartElement("p");
			}
			else if (_style == "ms")
			{
				_writer.WriteStartElement("title");
			}
			else if (_style == "mr")
			{
				_writer.WriteStartElement("title");
				_writer.WriteAttributeString("type", "scope");
				_writer.WriteStartElement("reference");
			}
			else if (_style == "s1")
			{
				_writer.WriteStartElement("hi");
				_writer.WriteAttributeString("type", "bold");
			}
			else if (_style == "r")
			{
				_writer.WriteStartElement("title");
				_writer.WriteAttributeString("level", "2");
				_writer.WriteStartElement("hi");
				_writer.WriteAttributeString("type", "italic");
			}
			else
			{
				_writer.WriteStartElement("p");
			}
		}

		private void WriteStyle(string style)
		{
			string prefix = string.Empty;
			if (style != string.Empty)
			{
				prefix = "\\" + style;
				if (_number != string.Empty)
				{
					prefix += Space + _number;
				}
			}
			_writer.WriteString(prefix);
		}

		/// <summary>
		/// Collect Figure Tag Information
		/// <figure style="fig" desc="Map of Israel and Moab during the time of Naomi and Ruth"
		/// file="Ruth-CS4_BW_ver2.pdf" size="col" loc="" copy="" ref="RUT 1.0"/>
		/// </summary>
		private void GetFigure()
		{

			_desc = string.Empty;
			_file = string.Empty;
			_size = string.Empty;
			_loc = string.Empty;
			_copy = string.Empty;
			_ref = string.Empty;
			if (_reader.HasAttributes)
			{
				while (_reader.MoveToNextAttribute())
				{
					if (_reader.Name == "style")
					{
						_style = _reader.Value;
					}
					else if (_reader.Name == "desc")
					{
						_desc = _reader.Value;
					}
					else if (_reader.Name == "file")
					{
						_file = _reader.Value;
					}
					else if (_reader.Name == "size")
					{
						_size = _reader.Value;
					}
					else if (_reader.Name == "loc")
					{
						_loc = _reader.Value;
					}
					else if (_reader.Name == "copy")
					{
						_copy = _reader.Value;
					}
					else if (_reader.Name == "ref")
					{
						_ref = _reader.Value;
					}
				}
			}
		}

		/// <summary>
		/// input: 
		/// <note caller="+" style="f">
		/// <char style="fr" closed="false">1.1-2 </char>
		/// <char style="ft" closed="false">hakim.</char>
		/// </note>
		/// output: 
		/// \f + 
		/// \fr 1.1-2 
		/// \ft hakim.
		/// \f*
		/// </summary>
		private void Note()
		{
			if (_tagName == "note")
			{
				string line;
				if (_verseNumber != string.Empty)
				{
					line = "\\v " + _verseNumber + Space;
					_writer.WriteString(line);
				}

				line = "\\" + _style + Space + _caller + Space;
				_writer.WriteString(line);
			}
		}

		private void MapClassName()
		{
			_mapClassName["toc1"] = "scrBookCode";
			_mapClassName["toc2"] = "scrBookName";
			_mapClassName["mt1"] = "Title_Main";
			_mapClassName["mt2"] = "Title_Secondary";
			_mapClassName["w"] = "See_In_Glossary";
			_mapClassName["v"] = "Verse_Number";
			_mapClassName["fr"] = "Note_Target_Reference";
			_mapClassName["fq"] = "Alternate_Reading";
			_mapClassName["f"] = "Note_General_Paragraph";
			_mapClassName["p"] = "Paragraph";
			_mapClassName["s"] = "Section_Head";
			_mapClassName["r"] = "Parallel_Passage_Reference";
			_mapClassName["c"] = "Chapter_Number";
			_mapClassName["rem"] = "rem";
			_mapClassName["fig"] = "fig";
			_mapClassName["q1"] = "Line1";
			_mapClassName["q2"] = "Line2";
			_mapClassName["m"] = "Paragraph_Continuation";
			_mapClassName["fqa"] = "fqa";
			_mapClassName["sc"] = "Inscription";
		}

		private string SignificantSpace(string content)
		{
			if (content == null) return "";
			content = content.Replace("\r\n", "");
			content = content.Replace("\n", "");
			content = content.Replace("\t", "");
			Char[] charac = content.ToCharArray();
			StringBuilder builder = new StringBuilder();
			foreach (char var in charac)
			{
				if (var == ' ' || var == '\b')
				{
					if (_significant)
					{
						continue;
					}
					_significant = true;
				}
				else
				{
					_significant = false;
				}
				builder.Append(var);
			}
			content = builder.ToString();
			return content;
		}

		private string StackPop(Stack<string> stack)
		{
			string result = string.Empty;
			if (stack.Count > 0)
			{
				result = stack.Pop();
			}
			return result;
		}

		private string StackPeek(Stack<string> stack)
		{
			string result = string.Empty;
			if (stack.Count > 0)
			{
				result = stack.Peek();
			}
			return result;
		}

		/// <summary>
		/// Open usx and OSIS file
		/// </summary>
		private void OpenFile()
		{
			_reader = Common.DeclareXmlTextReader(_usxFullPath, true);

			_writer = new XmlTextWriter(_osisFullPath, null);
			_writer.Formatting = Formatting.Indented;
			_writer.WriteStartDocument();

		}

		private void CreateHead(string xhtmlLang)
		{
			_writer.WriteStartElement("osis");
			_writer.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
			_writer.WriteAttributeString("xmlns", "http://www.bibletechnologies.net/2003/OSIS/namespace");
			_writer.WriteAttributeString("xsi:schemaLocation", "http://www.bibletechnologies.net/2003/OSIS/namespace file:../osisCore.2.0_UBS_SIL_BestPractice.xsd");
			_writer.WriteStartElement("osisText");
			_writer.WriteAttributeString("osisIDWork", "thisWork");
			_writer.WriteAttributeString("osisRefWork", "bible");
			_writer.WriteAttributeString("xml:lang", xhtmlLang);
			_writer.WriteStartElement("header");
			_writer.WriteStartElement("work");
			_writer.WriteAttributeString("osisWork", "thisWork");
			_writer.WriteEndElement();
			_writer.WriteEndElement();
			_writer.WriteStartElement("div");
			_writer.WriteAttributeString("type", "book");
		}


		private void SetBookCode()
		{
			_bookCode.Clear();
			_bookCode.Add("gen", "Gen");
			_bookCode.Add("exo", "Exod");
			_bookCode.Add("lev", "Lev");
			_bookCode.Add("num", "Num");
			_bookCode.Add("deu", "Deut");
			_bookCode.Add("jos", "Josh");
			_bookCode.Add("jdg", "Judg");
			_bookCode.Add("rut", "Ruth");
			_bookCode.Add("1sa", "1Sam");
			_bookCode.Add("2sa", "2Sam");
			_bookCode.Add("1ki", "1Kgs");
			_bookCode.Add("2ki", "2Kgs");
			_bookCode.Add("1ch", "1Chr");
			_bookCode.Add("2ch", "2Chr");
			_bookCode.Add("ezr", "Ezra");
			_bookCode.Add("neh", "Neh");
			_bookCode.Add("est", "Esth");
			_bookCode.Add("job", "Job");
			_bookCode.Add("psa", "Ps");
			_bookCode.Add("pro", "Prov");
			_bookCode.Add("ecc", "Eccl");
			_bookCode.Add("sng", "Song");
			_bookCode.Add("isa", "Isa");
			_bookCode.Add("jer", "Jer");
			_bookCode.Add("lam", "Lam");
			_bookCode.Add("dan", "Dan");
			_bookCode.Add("ezk", "Ezek");
			_bookCode.Add("hos", "Hos");
			_bookCode.Add("jol", "Joel");
			_bookCode.Add("amo", "Amos");
			_bookCode.Add("oba", "Obad");
			_bookCode.Add("jon", "Jonah");
			_bookCode.Add("mic", "Mic");
			_bookCode.Add("nah", "Nah");
			_bookCode.Add("hab", "Hab");
			_bookCode.Add("zep", "Zeph");
			_bookCode.Add("hag", "Hag");
			_bookCode.Add("zec", "Zech");
			_bookCode.Add("mal", "Mal");
			_bookCode.Add("jdt", "Jdt");
			_bookCode.Add("wis", "Wis");
			_bookCode.Add("tob", "Tob");
			_bookCode.Add("sir", "Sir");
			_bookCode.Add("bar", "Bar");
			_bookCode.Add("1mac", "1Macc");
			_bookCode.Add("2mac", "2Macc");
			_bookCode.Add("aes", "AddEsth");
			_bookCode.Add("sus", "Sus");
			_bookCode.Add("bel", "Bel");
			_bookCode.Add("paz", "PrAzar");
			_bookCode.Add("pma", "PrMan");
			_bookCode.Add("mat", "Matt");
			_bookCode.Add("mrk", "Mark");
			_bookCode.Add("luk", "Luke");
			_bookCode.Add("jhn", "John");
			_bookCode.Add("act", "Acts");
			_bookCode.Add("rom", "Rom");
			_bookCode.Add("1co", "1Cor");
			_bookCode.Add("2co", "2Cor");
			_bookCode.Add("gal", "Gal");
			_bookCode.Add("eph", "Eph");
			_bookCode.Add("php", "Phil");
			_bookCode.Add("col", "Col");
			_bookCode.Add("1th", "1Thess");
			_bookCode.Add("2th", "2Thess");
			_bookCode.Add("1ti", "1Tim");
			_bookCode.Add("2ti", "2Tim");
			_bookCode.Add("tit", "Titus");
			_bookCode.Add("phm", "Phlm");
			_bookCode.Add("heb", "Heb");
			_bookCode.Add("jas", "Jas");
			_bookCode.Add("1pe", "1Pet");
			_bookCode.Add("2pe", "2Pet");
			_bookCode.Add("1jn", "1John");
			_bookCode.Add("2jn", "2John");
			_bookCode.Add("3jn", "3John");
			_bookCode.Add("jud", "Jude");
			_bookCode.Add("rev", "Rev");
		}

		private void CloseFile()
		{
			_writer.WriteEndElement();
			_writer.WriteEndDocument();
			_writer.Flush();
			_writer.Close();
			ClearScope();
		}

		private void ClearScope()
		{
			if (_scopeKey != string.Empty && !_mappingScopeChapterandVerse.ContainsKey(_scopeKey) && _scopeChapterandVerse.Count > 0)
			{
				_mappingScopeChapterandVerse.Add(_scopeKey, _scopeChapterandVerse[0].ToString() + "-" + _scopeChapterandVerse[_scopeChapterandVerse.Count - 1].ToString());
			}
			_scopeKey = string.Empty;
			_scopeChapterandVerse.Clear();
		}

		private void UpdateScopeProcess()
		{
			SetBookCode();

			string scope;
			XmlDocument xmlDocument = new XmlDocument();			
			if(!File.Exists(_osisFullPath))
			{
				string usxToOsisFile = _usxFullPath.Replace(".usx",".xml");
				if(File.Exists(usxToOsisFile))
				{
					File.Copy(usxToOsisFile, _osisFullPath, true);
				}
			}
			xmlDocument.Load(_osisFullPath);
			XmlNodeList entryNodes = xmlDocument.GetElementsByTagName("div");
			foreach (XmlNode node in entryNodes)
			{
				if (node.Attributes != null && node.Attributes["scope"] != null)
				{
					string scopeData = node.Attributes["scope"].Value;
					if (_mappingScopeChapterandVerse.ContainsKey(scopeData))
					{
						scope = _mappingScopeChapterandVerse[scopeData];
						node.Attributes["scope"].Value = scope;
					}
				}
			}

            XmlNodeList symbolNodes = xmlDocument.GetElementsByTagName("p");
            foreach (XmlNode node in symbolNodes)
            {
                if (node != null)
                {
                    string sText = node.InnerXml;
                    if (sText.IndexOf("&lt;", StringComparison.Ordinal) >= 0 || sText.IndexOf("&gt;", StringComparison.Ordinal) >= 0)
                    {
                        sText = sText.Replace("&lt;", "‘").Replace("&gt;", "’");
                        node.InnerXml = sText;
                    }

                }
            }
			xmlDocument.Save(_osisFullPath);
		}

		private void CorrectTheBookCodeProcess()
		{
			SetBookCode();
			XmlDocument xmlDocument = new XmlDocument();
			try		
			{
				if (File.Exists(_usxFullPath))
				{					
					xmlDocument.Load(_usxFullPath);
					XmlNodeList entryNodes = xmlDocument.GetElementsByTagName("book");
					string bookCodeValue;
					foreach (XmlNode node in entryNodes)
					{
						if (node.Attributes != null && node.Attributes["code"] != null)
						{
							if (node.Attributes != null && node.Attributes["code"] != null)
							{
								string bookCode = node.Attributes["code"].Value;
								if (_bookCode.ContainsKey(bookCode.ToLower()))
								{
									bookCodeValue = _bookCode[bookCode.ToLower()];
									node.Attributes["code"].Value = bookCodeValue;
								}
							}
						}
					}
					xmlDocument.Save(_usxFullPath);
				}
			}
			catch
			{
				return;
			}
		}

		private void ProcessUsxtoOsis(string USXFile, string OSISFile)
		{
				if (File.Exists(OSISFile))
				{
					File.Delete(OSISFile);  // clean up the un-transformed file
				}
				XslCompiledTransform xsltProcess = new XslCompiledTransform();
				xsltProcess.Load(XmlReader.Create(Common.UsersXsl("ConvertUSXtoOSIS.xsl")));
				ApplyUsxtoOsisXsl(USXFile, xsltProcess, OSISFile);
		}


		public static void ApplyUsxtoOsisXsl(string fileFullPath, XslCompiledTransform xslt, string OSISFile)
		{
		
			XmlTextReader reader = Common.DeclareXmlTextReader(fileFullPath, true);
			FileStream xmlFile = new FileStream(OSISFile, FileMode.Create);
			XmlWriter writer = XmlWriter.Create(xmlFile, xslt.OutputSettings);
			xslt.Transform(reader, null, writer, null);
			xmlFile.Close();
			reader.Close();
		
		}
	}
}