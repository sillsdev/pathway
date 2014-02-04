using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.Win32;
using SIL.Tool;

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
        
        private Dictionary<string, Dictionary<string, string>> _styleInfo =
            new Dictionary<string, Dictionary<string, string>>();

        private Dictionary<string, string> _cssProp;
        private Dictionary<string, string> _mapClassName = new Dictionary<string, string>();

        private string _tagName, _style, _number, _code, _caller, _content;
        private string _parentTagName = string.Empty, _parentStyleName = string.Empty;
        private string _verseNumber = string.Empty;
        private string _paraStyle = string.Empty;


        private string _desc, _file, _size, _loc, _copy, _ref;
        private bool _significant, _isEmptyNode, _isParaWritten;

        private const string Space = " ";
        private const string Bar = "|";

        private bool _isclassNameExist;
        private List<string> _xhtmlAttribute = new List<string>();

        #endregion

        /// <summary>
        /// Entry method to convert USX to OSIS
        /// </summary>
        public void ConvertUsxToOSIS(string usxFullPath, string OSISFullPath, string xhtmlLang)
        {
            _usxFullPath = usxFullPath;
            _osisFullPath = OSISFullPath;
            OpenFile();
            CreateHead(xhtmlLang);
            ProcessUsx();
            CloseFile();
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
                        if (_reader.Name == "book")
                        {
                            headXML = false;
                        }
                        else
                        {
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
            }

            _reader.Close();
        }

        /// <summary>
        /// Write content
        /// </summary>
        private void WriteElement()
        {
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
            if (_tagName == "book")
            {
                string line = "\\" + _style + Space + _code + Space + _content;
                _writer.WriteString(line);
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
        /// input:  <verse number="1" style="v" />abc </verse>
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
            string style = StackPop(_allStyle);
            string tag = StackPop(_alltagName);
            _writer.WriteEndElement();
            //if (tag == "para")
            //{
            //    //_writer.WriteString();
            //    _verseNumber = string.Empty;
            //}
            //else if (tag == "note")
            //{
            //    string line = "\\" + style + "*";
            //    _writer.WriteString(line);
            //}
            //else if (tag != "verse")
            //{
            //    _style = string.Empty;
            //    _tagName = StackPeek(_alltagName);
            //}
        }

        /// <summary>
        /// Collect Tag Information
        /// </summary>
        private void StartElement()
        {
            _xhtmlAttribute.Clear();
            _isclassNameExist = false;
            _number = string.Empty;

            _parentStyleName = StackPeek(_allStyle);
            _parentTagName = StackPeek(_alltagName);

            _style = _tagName = _reader.Name;

            //Read Attributes
            if (_reader.HasAttributes)
            {
                while (_reader.MoveToNextAttribute())
                {

                    if (_reader.Name == "style")
                    {
                        _isclassNameExist = true;
                        _style = _reader.Value;
                        //_writer.WriteAttributeString(_reader.Name, _reader.Value);
                    }
                    else if (_reader.Name == "number")
                    {
                        _number = _reader.Value;
                        if (_tagName == "verse")
                        {
                            _verseNumber = _number;
                            //_writer.WriteAttributeString("osisID", _reader.Value);
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
                        //_writer.WriteAttributeString(_reader.Name, _reader.Value);
                    }
                }
            }


            //Write Start Element
            if (_tagName == "para")
            {
                _paraStyle = _style;
                if (_style == "h" || _style == "is" || _style == "iot" || _style == "title" || _style == "s")
                {
                    _writer.WriteStartElement("title");
                }
                else if (_style == "mt" || _style == "mt1" || _style == "imt")
                {
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
                    _writer.WriteStartElement("item");
                } 
                else if (_style == "r")
                {
                    _writer.WriteStartElement("reference");
                }
                else if (_style == "p")
                {
                    _writer.WriteStartElement("p");
                }
                else 
                {
                    _writer.WriteStartElement("p");
                }
            }
            else if (_tagName == "chapter")
            {
                _writer.WriteStartElement("chapter");
                _writer.WriteAttributeString("n", _number);
                _writer.WriteAttributeString("osisID", "Matt." + _verseNumber);
            }
            else if (_tagName == "verse")
            {
                string verseNo = "Matt." + _verseNumber + "." + _number;
                _writer.WriteStartElement("verse");
                _writer.WriteAttributeString("subType", "x-embedded");
                _writer.WriteAttributeString("sID", verseNo);
                _writer.WriteAttributeString("n", _number);
                _writer.WriteAttributeString("osisID", verseNo);
            }
            else if (_tagName == "note")
            {
                _writer.WriteStartElement("note");
                _writer.WriteAttributeString("type", "crossReference");
                _writer.WriteAttributeString("n", _caller);
            }
            else if (_tagName == "char")
            {
                if (_style == "bk" )
                {
                    _writer.WriteStartElement("reference");
                }
                else
                {
                    _writer.WriteStartElement("char");    
                }
                
                //_writer.WriteAttributeString("type", "source");
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



            //if (_isEmptyNode)
            //{
            //    if (_tagName != "verse")
            //    {
            //        WriteStyle(_style);
            //        if (_tagName == "char")
            //        {
            //            _writer.WriteString(Space);
            //        }
            //    }
            //    _isEmptyNode = false;
            //}
            //else
            //{
            //    if (_tagName == "chapter")
            //    {
            //        WriteStyle(_style);
            //        //_writer.WriteString();
            //        _isEmptyNode = false;
            //    }
            //    else if (_tagName == "para")
            //    {
            //        _isParaWritten = false;
            //    }

            //    _allStyle.Push(_style);
            //    _alltagName.Push(_tagName);
            //}
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
        /// file="Ruth-CS4_BW_ver2.pdf" size="col" loc="" copy="" ref="RUT 1.0">
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
                        _isclassNameExist = true;
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
        /// <char style="ft" closed="false">‘hakim’.</char>
        /// </note>
        /// output: 
        /// \f + 
        /// \fr 1.1-2 
        /// \ft ‘hakim’.
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


        ///// <summary>
        ///// Write Para Tag Information
        ///// </summary>
        //private void Para()
        //{
        //    string line = "\\" + _style + Space;
        //    _writer.WriteString(line);
        //    _tagName = "others";
        //}

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
            //string content = _reader.Value;
            content = content.Replace("\r\n", "");
            content = content.Replace("\n", "");
            content = content.Replace("\t", "");
            Char[] charac = content.ToCharArray();
            StringBuilder builder = new StringBuilder();
            //if (charac.Length == 1)
            //{
            //    return content;
            //}
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
            //_writer.WriteString(content);
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
            _writer.WriteAttributeString("xmlns:osis", "http://www.bibletechnologies.net/2003/OSIS/namespace");
            _writer.WriteAttributeString("xsi:schemaLocation",
                                         "http://www.bibletechnologies.net/2003/OSIS/namespace http://www.bibletechnologies.net/osisCore.2.1.1.xsd");
            _writer.WriteStartElement("osisText");
            _writer.WriteAttributeString("osisIDWork", "thisWork");
            _writer.WriteAttributeString("osisRefWork", "bible");
            _writer.WriteAttributeString("xml:lang", xhtmlLang);
            //_writer.WriteAttributeString("canonical", "true");
            _writer.WriteStartElement("header");
            _writer.WriteStartElement("work");
            _writer.WriteAttributeString("osisWork","thisWork");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("div");
            _writer.WriteAttributeString("type", "bookGroup");

        }


        private void CloseFile()
        {
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            //_writer.WriteEndElement();

            _writer.WriteEndDocument();
            _writer.Flush();
            _writer.Close();
        }
    }
}