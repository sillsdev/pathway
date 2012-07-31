using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class UsxToSFM
    {
        protected Stack<string> _allStyle = new Stack<string>();

        private string _usxFullPath, _sfmFullPath;

        protected XmlTextReader _reader;
        private TextWriter _sfmFile;

        private Dictionary<string, Dictionary<string, string>> _styleInfo =
            new Dictionary<string, Dictionary<string, string>>();

        private Dictionary<string, string> _cssProp;
        private Dictionary<string, string> _mapClassName = new Dictionary<string, string>();

        private string _tagName, _style, _number, _code;

        private const string Space = " ";

        private bool _isclassNameExist;
        private List<string> _xhtmlAttribute = new List<string>();

        /// <summary>
        /// Entry method to convert USX to SFM
        /// sty file is already set for this class.
        /// </summary>
        public void ConvertUsxToSFM(string usxFullPath, string sfmFullPath)
        {
            _usxFullPath = usxFullPath;
            _sfmFullPath = sfmFullPath;
            OpenFile();
            MapClassName();
            ProcessUsx();
            //WriteCSS();
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
                        continue;
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
            _sfmFile.Close();
        }

        /// <summary>
        /// Write content
        /// </summary>
        private void WriteElement()
        {
            if (_tagName == "book")
            {
                Book();
            }
            else if (_tagName == "para" || _tagName == "para")
            {
                Para();
            }

        }

        /// <summary>
        /// input: <book code="RUT" style="id">TestName</book>
        /// output: \id RUT TestName
        /// </summary>
        private void Book()
        {
            string content = _reader.Value;
            string line = "\\" + _style + Space + _code + Space + content;
            _sfmFile.WriteLine(line);

        }

        /// <summary>
        /// input: <para style="imt">Kata-Kata Partama</para>
        /// output: \imt Kata-Kata Partama
        /// </summary>
        private void Para()
        {
            string content = _reader.Value;
            string line = "\\" + _style + Space + content;
            _sfmFile.WriteLine(line);

        }

        /// <summary>
        /// Write content
        /// </summary>
        private void EndElement()
        {
            StackPop(_allStyle);
            //if nested write with *
            //todo
        }

        /// <summary>
        /// Collect Tag Information
        /// </summary>
        protected void StartElement()
        {
            _xhtmlAttribute.Clear();
            _isclassNameExist = false;
            _style = _tagName = _reader.Name;

            if (_reader.HasAttributes)
            {
                while (_reader.MoveToNextAttribute())
                {
                    if (_reader.Name == "style")
                    {
                        _isclassNameExist = true;
                        _style = _reader.Value;
                    }
                    else if (_reader.Name == "number")
                    {
                        _number = _reader.Value;
                    }
                    else if (_reader.Name == "code")
                    {
                        _code = _reader.Value;
                    }
                }
            }

            if (_tagName == "figure")
            {
                //todo
            }
            else if (_tagName == "note")
            {
                //todo
            }

            _allStyle.Push(_style);
        }

        protected void CreateBook()
        {

        }

        /// ------------------------------------------------------------
        /// <summary>
        /// Parses a line in an Paratext sty file.
        /// </summary>
        /// <param name="line">The line in a Paratext sty file.</param>
        /// ------------------------------------------------------------
        private void ParseLine(string line)
        {
            string value;
            string word = Common.LeftString(line, " ");

            switch (word)
            {
                case "\\Name":
                    CreateClass(line);
                    break;
                case "\\FontSize":
                    value = PropertyValue(line);
                    _cssProp["font-size"] = value + "pt";
                    break;
                case "\\Color":
                    value = PropertyValue(line);
                    string strHex = String.Format("{0:x2}", Convert.ToUInt32(value));
                    _cssProp["color"] = "#" + strHex;
                    break;
                case "\\Justification":
                case "\\JustificationType": // ??
                    value = PropertyValue(line);
                    _cssProp["text-align"] = value;
                    break;
                case "\\SpaceBefore":
                    value = PropertyValue(line);
                    _cssProp["padding-top"] = value + "pt";
                    break;
                case "\\SpaceAfter":
                    value = PropertyValue(line);
                    _cssProp["padding-bottom"] = value + "pt";
                    break;
                case "\\LeftMargin":
                    value = PropertyValue(line);
                    _cssProp["margin-left"] = value + "pt";
                    break;
                case "\\RightMargin":
                    value = PropertyValue(line);
                    _cssProp["margin-right"] = value + "pt";
                    break;
                case "\\FirstLineIndent":
                    value = PropertyValue(line);
                    value = Common.LeftString(value, "#").Trim();
                    _cssProp["text-indent"] = value + "pt";
                    break;
                case "\\Fontname":
                    value = PropertyValue(line);
                    _cssProp["font-name"] = value;
                    break;
                case "\\Italic":
                    _cssProp["font-style"] = "italic";
                    break;
                case "\\Bold":
                    _cssProp["font-weight"] = "bold";
                    break;
                case "\\Superscript":
                    _cssProp["vertical-align"] = "super";
                    break;
                case "\\Subscript":
                    _cssProp["vertical-align"] = "sub";
                    break;
                case "\\Underline":
                    _cssProp["text-decoration"] = "underline";
                    break;
                case "\\LineSpacing":
                    value = PropertyValue(line);
                    string val = LineSpace(value);
                    _cssProp["line-height"] = val;
                    break;
            }
        }

        /// ------------------------------------------------------------
        /// <summary>
        /// Creates CSS style from the \Marker line.
        /// </summary>
        /// <param name="line">A line from the sty file which should
        /// contain the name of the style.</param>
        /// ------------------------------------------------------------
        private void CreateClass(string line)
        {
            int start = line.IndexOf(" ") + 1;
            int iSecondSpace = line.IndexOf(" ", start);
            int end = (iSecondSpace > start) ? iSecondSpace : line.Length;
            string className = line.Substring(start, end - start);

            className = RemoveMultiClass(className);

            string mapClassName = className;
            if (_mapClassName.ContainsKey(className))
                mapClassName = _mapClassName[className];

            _cssProp = new Dictionary<string, string>();
            _styleInfo[mapClassName] = _cssProp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        private string RemoveMultiClass(string className)
        {
            int pos = className.IndexOf("...", 1);
            if (pos > 0)
            {
                className = className.Substring(0, pos);
            }
            return className;
        }

        /// ------------------------------------------------------------
        /// <summary>
        /// Gets the value from a line in the sty file.
        /// </summary>
        /// <param name="line">A line from the sty file which should
        /// contain property values for a style.</param>
        /// <returns></returns>
        /// ------------------------------------------------------------
        private string PropertyValue(string line)
        {
            string propertyVal;
            try
            {
                propertyVal = Common.RightString(line, " ").ToLower();
            }
            catch (Exception)
            {
                propertyVal = string.Empty;
            }

            return propertyVal;
        }

        /// ------------------------------------------------------------
        /// <summary>
        /// Writes the Cascading Style Sheet given properties determined
        /// from the Paratext sty file.
        /// </summary>
        /// ------------------------------------------------------------
        public void WriteCSS()
        {
            TextWriter cssFile = new StreamWriter(_usxFullPath);

            foreach (KeyValuePair<string, Dictionary<string, string>> cssClass in _styleInfo)
            {
                if (cssClass.Key != "\\Name")
                {
                    cssFile.WriteLine("." + cssClass.Key);
                    cssFile.WriteLine("{");
                    foreach (KeyValuePair<string, string> property in cssClass.Value)
                    {
                        cssFile.WriteLine(property.Key + ": " + property.Value + ";");
                    }
                    cssFile.WriteLine("}");
                    cssFile.WriteLine();
                }
            }
            cssFile.Close();
        }

        /// <summary>
        /// Convert paratext unit to css unit
        /// </summary>
        /// <param name="value">0/1/2</param>
        /// <returns>100%/150%/200%</returns>
        private string LineSpace(string value)
        {
            string result = string.Empty;
            if (value == "0")
            {
                result = "100%";
            }
            else if (value == "1")
            {
                result = "150%";
            }
            else if (value == "2")
            {
                result = "200%";
            }
            return result;
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

        private string StackPop(Stack<string> stack)
        {
            string result = string.Empty;
            if (stack.Count > 0)
            {
                result = stack.Pop();
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