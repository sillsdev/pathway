// --------------------------------------------------------------------------------------------
// <copyright file="AfterBeforeProcessEpub.cs" from='2009' to='2014' company='SIL International'>
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
// 
// </remarks>
// --------------------------------------------------------------------------------------------

#region Using
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections;
using System.IO;

#endregion Using

namespace SIL.Tool
{
	public class AfterBeforeProcessEpub : AfterBeforeXHTMLProcess
	{
		#region Private Variable

		private bool IsEmptyElement = false;
		private ArrayList _psuedoBefore = new ArrayList();
		private Dictionary<string, ClassInfo> _psuedoAfter = new Dictionary<string, ClassInfo>();
		private bool _IsHeadword = false;
		private bool _significant;
		private bool _anchorWrite;
		private bool _isPictureDisplayNone = false;
		private int _counter = 0;
		public ArrayList _psuedoClassName = new ArrayList();

		#endregion

		#region Public Methods

		public void RemoveAfterBefore(PublicationInformation projInfo,
									  Dictionary<string, Dictionary<string, string>> idAllClass,
									  Dictionary<string, ArrayList> classFamily, ArrayList cssClassOrder)
		{
			InitializeData(projInfo, idAllClass, classFamily, cssClassOrder);
			string sourceFile = SourceTargetFile(projInfo);
			OpenXhtmlFile(sourceFile); //reader
			CreateFile(projInfo); //writer
			InsertBeforeAfter();
		}

		private string SourceTargetFile(PublicationInformation projInfo)
		{
			string sourceFile = "";
			sourceFile = projInfo.DefaultXhtmlFileWithPath.Replace(".xhtml", "_1.xhtml");
			File.Copy(projInfo.DefaultXhtmlFileWithPath, sourceFile, true);
			return sourceFile;
		}

		#endregion

		private void InitializeData(PublicationInformation projInfo,
									Dictionary<string, Dictionary<string, string>> idAllClass,
									Dictionary<string, ArrayList> classFamily, ArrayList cssClassOrder)
		{
			_allStyle = new Stack<string>();
			_allParagraph = new Stack<string>();
			_allCharacter = new Stack<string>();

			_childStyle = new Dictionary<string, string>();
			IdAllClass = new Dictionary<string, Dictionary<string, string>>();
			ParentClass = new Dictionary<string, string>();
			_newProperty = new Dictionary<string, Dictionary<string, string>>();
			_displayBlock = new Dictionary<string, string>();
			_cssClassOrder = cssClassOrder;

			_projectPath = projInfo.TempOutputFolder;

			IdAllClass = idAllClass;
			_classFamily = classFamily;

			_isNewParagraph = false;
			_characterName = "None";
		}

		/// <summary>
		/// To replace the symbol string if the symbol matches with the text
		/// </summary>
		/// <param name="data">XML Content</param>
		private string ReplaceString(string data)
		{
			if (_replaceSymbolToText.Count > 0)
			{
				foreach (string srchKey in _replaceSymbolToText.Keys)
				{
					if (data.IndexOf(srchKey) >= 0)
					{
						data = data.Replace(srchKey, _replaceSymbolToText[srchKey]);
					}
				}
			}
			return data;
		}

		public void InsertBeforeAfter()
		{
			bool headXML = true;
			while (_reader.Read())
			{
				if (_reader.IsEmptyElement)
				{
					IsEmptyElement = true;
				}
				switch (_reader.NodeType)
				{
					case XmlNodeType.Element:
						if (_reader.Name == "html")
						{
							_writer.WriteStartElement("html");
							break;
						}
						_writer.WriteStartElement(_reader.LocalName);
						_writer.WriteAttributes(_reader, true);
						if (_reader.IsEmptyElement)
						{
							_writer.WriteEndElement();
						}


						if (headXML) // skip previous parts of <body> tag
						{
							if (_reader.Name == "body")
							{
								headXML = false;
							}
							else
							{
								continue;
							}
						}
						StartElement();
						break;
					case XmlNodeType.Text:
						Write();
						break;
					case XmlNodeType.Whitespace:
					case XmlNodeType.SignificantWhitespace:
						InsertWhiteSpace();
						break;
					case XmlNodeType.CDATA:
						_writer.WriteCData(_reader.Value);
						break;
					case XmlNodeType.EntityReference:
						IncludeWhiteSpace();
						break;
					case XmlNodeType.XmlDeclaration:
					case XmlNodeType.ProcessingInstruction:
						_writer.WriteProcessingInstruction(_reader.Name, _reader.Value);
						break;
					case XmlNodeType.DocumentType:
						_writer.WriteDocType(_reader.Name, _reader.GetAttribute("PUBLIC"),
											 _reader.GetAttribute("SYSTEM"), _reader.Value);
						break;
					case XmlNodeType.Comment:
						_writer.WriteComment(_reader.Value);
						break;
					case XmlNodeType.EndElement:
						EndElement();
						_writer.WriteFullEndElement();

						break;

				}
			}
			_writer.Close();
			_reader.Close();
		}

		private void IncludeWhiteSpace()
		{
			if (_reader.Value != "")
			{
				return;
			}

			bool whiteSpaceExist = _significant;
			SignificantSpace(_reader.Value);
			if (!whiteSpaceExist)
			{
				_writer.WriteStartElement("span");
				_writer.WriteRaw("&nbsp;");
				_writer.WriteEndElement();
				_significant = true;
			}
		}

		private void InsertWhiteSpace()
		{
			bool whiteSpaceExist = _significant;
			SignificantSpace(_reader.Value);
			if (!whiteSpaceExist)
			{
				_writer.WriteStartElement("span");
				_writer.WriteRaw("&nbsp;");
				_writer.WriteEndElement();
				_significant = true;
			}
		}

		private void Write()
		{

			if (_isDisplayNone)
			{
				return; // skip the node
			}

			if (_isNewParagraph)
			{
				if (_paragraphName == null)
				{
					_paragraphName = StackPeek(_allParagraph); // _allParagraph.Pop();
				}
				ClosePara(false);
				_previousParagraphName = _paragraphName;
				_paragraphName = null;
				_isNewParagraph = false;
				_isParagraphClosed = false;
				_textWritten = false;
			}
			WriteText();
		}

		private void WriteText()
		{
			string content = _reader.Value;
			content = ReplaceString(content);

			if (_previousParagraphName == "entry_letData")
				_counter = 0;


			if (_isPictureDisplayNone)
			{
				return;
			}

			List<string> psuedoContent = new List<string>();
			// Psuedo Before
			foreach (ClassInfo psuedoBefore in _psuedoBefore)
			{
				if (psuedoBefore.Content != null && psuedoBefore.Content.Trim().Length == 0)
				{
					if (!_significant)
					{
						_writer.WriteStartElement("span");
						_writer.WriteRaw("&nbsp;");
						_writer.WriteEndElement();
						_significant = true;
					}
				}
				else if (!string.IsNullOrEmpty(psuedoBefore.Content))
				{
					if (!psuedoContent.Contains(psuedoBefore.Content))
					{
						if (psuedoBefore.Content.Contains("counter("))
						{
							_counter = _counter + 1;
							psuedoContent.Add(psuedoBefore.Content);
							psuedoBefore.Content = _counter.ToString() + " ";
							_writer.WriteString(psuedoBefore.Content);
							if (psuedoBefore.Content != null && !_psuedoClassName.Contains(psuedoBefore.StyleName))
							{
								_psuedoClassName.Add(psuedoBefore.StyleName);
							}
							break;
						}
					}
				}
			}

			// Text Write
			if (_characterName == null)
			{
				_characterName = StackPeekCharStyle(_allCharacter);
			}
			if (_psuedoContainsStyle != null)
			{
				if (content.IndexOf(_psuedoContainsStyle.Contains) > -1)
				{
					content = _psuedoContainsStyle.Content;
					_characterName = _psuedoContainsStyle.StyleName;
				}
			}
			content = SignificantSpace(content);
			_writer.WriteString(content);
			_psuedoBefore.Clear();

		}

		protected override string StackPeekCharStyle(Stack<string> stack)
		{
			string result = "none";
			if (stack.Count > 0)
			{
				result = stack.Peek();
			}
			return result;
		}

		private string SignificantSpace(string content)
		{
			if (content == null) return "";
			content = content.Replace("\r\n", "");
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

		private void StartElement()
		{
			if (!IsEmptyElement)
			{
				StartElementBase(_IsHeadword);
				Psuedo();
			}
			IsEmptyElement = false;
		}

		private void Psuedo()
		{
			if (_isDisplayNone) return; // skip the node

			// Psuedo Before
			if (_psuedoBeforeStyle != null)
			{
				_psuedoBefore.Add(_psuedoBeforeStyle);
			}

			// Psuedo After
			if (_psuedoAfterStyle != null)
			{
				_psuedoAfter[_childName] = _psuedoAfterStyle;
			}
		}

		private void EndElement()
		{
			if (_reader.Name == "a" && _anchorWrite)
			{
				_anchorWrite = false;
			}

			_characterName = null;
			_closeChildName = StackPop(_allStyle);
			if (_closeChildName == string.Empty) return;
			
			// Psuedo After
			PseudoAfter();
			EndElementBase(false);

			_classNameWithLang = StackPeek(_allStyle);
			_classNameWithLang = Common.LeftString(_classNameWithLang, "_");
		}


		private void PseudoAfter()
		{
			if (_psuedoAfter.Count > 0)
			{
				if (_psuedoAfter.ContainsKey(_closeChildName))
				{
					ClassInfo classInfo = _psuedoAfter[_closeChildName];
					if (classInfo.Content != null && classInfo.Content.Trim().Length == 0)
					{
						if (!_significant)
						{
							_writer.WriteStartElement("span");
							_writer.WriteRaw("&nbsp;");
							_writer.WriteEndElement();
							_significant = true;
						}
					}
					else
					{
						if (_closeChildName.Contains("Glossary"))
						{
							_writer.WriteString(" ");
						}
					}
					_psuedoAfter.Remove(_closeChildName);
				}
			}
		}

		private void CreateFile(PublicationInformation projInfo)
		{

			_writer = new XmlTextWriter(projInfo.DefaultXhtmlFileWithPath, null);
		}

	}

}
