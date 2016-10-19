// --------------------------------------------------------------------------------------------
// <copyright file="UsxToSFM.cs" from='2009' to='2014' company='SIL International'>
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
// Convert USX to SFM format
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class UsxToSFM
    {
        #region Private Variable

        private Stack<string> _allStyle = new Stack<string>();
        private Stack<string> _alltagName = new Stack<string>();

        private string _usxFullPath, _sfmFullPath;

        private XmlTextReader _reader;
        private StreamWriter _sfmFile;

        private string _tagName, _style, _number, _code, _caller, _content;
        private string _parentTagName = string.Empty, _parentStyleName = string.Empty;
        private string _verseNumber = string.Empty;

        private string _desc, _file, _size, _loc, _copy, _ref;
        private bool _significant, _isEmptyNode, _isParaWritten;

        private const string Space = " ";
        private const string Bar = "|";

        private List<string> _xhtmlAttribute = new List<string>();

        #endregion

        /// <summary>
        /// Entry method to convert USX to SFM
        /// sty file is already set for this class.
        /// </summary>
        public void ConvertUsxToSFM(string usxFullPath, string sfmFullPath)
        {
            _usxFullPath = usxFullPath;
            _sfmFullPath = sfmFullPath;
            OpenFile();
            ProcessUsx();
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
                while (_reader.Read())
                {
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
            _sfmFile.Close();
        }

        /// <summary>
        /// Write content
        /// </summary>
        private void WriteElement()
        {
            _content = SignificantSpace(_reader.Value);
            if (_tagName == "para")
            {
                Para();
                _isParaWritten = true;
            }
            else if (_tagName == "char")
            {
                Char();
            }
            else if (_tagName == "chapter")
            {
                Chapter();
            }
            else if (_tagName == "verse")
            {
                Verse();
            }
            else if (_tagName == "figure")
            {
                Figure();
            }
            else if (_style == string.Empty)
            {
                Content();
            }
        }

        /// <summary>
        /// input: <book code="RUT" style="id">TestName</book>
        /// output: \id RUT TestName
        /// </summary>
        private void Book()
        {
            if (_tagName == "book")
            {
                string line = "\\" + _style + Space + _code + Space + _content;
                _sfmFile.WriteLine(line);
                if (_style == "id")
                {
                    Common.BookNameTag = "id";
                    if (Common.BookNameCollection.Contains(_code) == false && _code != null)
                    {
                        Common.BookNameCollection.Add(_code);
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
            _sfmFile.Write(line);
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
                    Common.BookNameCollection.Remove(_code);
                    Common.BookNameCollection.Add(bookName);
                }
            }

            _sfmFile.Write(line);
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
                _sfmFile.WriteLine(line);
                _isParaWritten = true;
                _verseNumber = string.Empty;
            }
            else
            {
                _sfmFile.WriteLine();
            }

            bool isWritten = HandleBridgeVerseNumbers(_number);

            if (!isWritten)
            {
                line = "\\" + _style + Space + _number + Space + _content.Trim() + EndText();
                _sfmFile.Write(line);
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
                        _sfmFile.Write(sb.ToString());
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
                    _sfmFile.Write(sb.ToString());
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
                _sfmFile.Write(sb.ToString());
                isWritten = true;
            }
            else if (isNumeric == false)
            {
                sb.AppendLine("\\" + _style + Space + verseNumber + Space + _content.Trim() + EndText());
                _sfmFile.Write(sb.ToString());
                isWritten = true;
            }
            return isWritten;
        }

        /// <summary>
        /// </summary>
        private void Chapter()
        {
            string line = "\\" + _style + Space + _number;
            _sfmFile.Write(line);
        }

        /// <summary>
        /// input:  <figure style="fig" desc="Map" file="Ruth.pdf" size="col" loc="" copy="" ref="RUT 1.0">Israel</figure>
        /// output: \fig Map|Ruth.pdf|col|||Israel|RUT 1.0\fig*
        /// </summary>
        private void Figure()
        {
            string line = "\\" + _style + Space + _desc + Bar + _file + Bar + _size + Bar + _loc + Bar + _copy + Bar + _content + Bar + _ref + EndText();
            _sfmFile.Write(line);
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
                if (_style == "bdit" || _style == "wj")
                {                    
					string line = "\\v " + _verseNumber + Space + _content;
					if (_verseNumber == null || _verseNumber == string.Empty)
					{
						line = Space + _content;
					}
					else
					{
						_sfmFile.WriteLine();
					}					
                    _sfmFile.Write(line);
                    return;
                }   
                prefix = Space + "\\" + _style + Space;             
            }

            if (_parentTagName == "note")
            {
                string line = prefix + _content;
                _sfmFile.Write(line);
            }
            else
            {
                string line = prefix + _content + EndText();
                _sfmFile.Write(line);
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
            string style = StackPop(_allStyle);
            string tag = StackPop(_alltagName);

            if (tag == "para")
            {
                _sfmFile.WriteLine();
                _verseNumber = string.Empty;
            }
            else if (tag == "note")
            {
                string line = "\\" + style + "*";
                _sfmFile.Write(line);
            }
            else if (tag != "verse")
            {
                _style = string.Empty;
                _tagName = StackPeek(_alltagName);
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

            if (_tagName == "figure")
            {
                WriteStyle(_parentStyleName);
                _sfmFile.Write(Space);
                GetFigure();
            }
            else // other nodes
            {
                if (_reader.HasAttributes)
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
                        }
                        else if (_reader.Name == "code")
                        {
                            _code = _reader.Value;
                        }
                        else if (_reader.Name == "id")
                        {
                            _code = _reader.Value;
                        }
                        else if (_reader.Name == "caller")
                        {
                            _caller = _reader.Value;
                        }
                    }
                }
            }

            Note();
            Book();

            if (_isEmptyNode)
            {
                if (_tagName != "verse")
                {
                    WriteStyle(_style);
                    if (_tagName == "char")
                    {
                        _sfmFile.Write(Space);
                    }
                    else
                    {
                        _sfmFile.WriteLine();
                    }
                }                
                _isEmptyNode = false;
            }
            else
            {
                if (_tagName == "chapter")
                {
                    WriteStyle(_style);
                    _sfmFile.WriteLine();
                    _isEmptyNode = false;
                }
                else if (_tagName == "para")
                {
                    _isParaWritten = false;
                }

                _allStyle.Push(_style);
                _alltagName.Push(_tagName);
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
            _sfmFile.Write(prefix);
        }

        /// <summary>
        /// Collect Figure Tag Information
        /// figure style="fig" desc="Map of Israel and Moab during the time of Naomi and Ruth"
        /// file="Ruth-CS4_BW_ver2.pdf" size="col" loc="" copy="" ref="RUT 1.0"
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
                    _sfmFile.Write(line);
                }

                line = "\\" + _style + Space + _caller + Space;
                _sfmFile.Write(line);
            }
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
        /// Open usx and sfm file
        /// </summary>
        private void OpenFile()
        {
            _reader = Common.DeclareXmlTextReader(_usxFullPath, true);
            _sfmFile = new StreamWriter(_sfmFullPath);
        }
    }
}