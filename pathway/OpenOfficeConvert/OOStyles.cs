// --------------------------------------------------------------------------------------------
// <copyright file="OOStyles.cs" from='2009' to='2014' company='SIL International'>
//      Copyright (c) 2014, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Creates the OOStyle file 
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class LOStyles : LOStyleBase
    {
        public Dictionary<string, Dictionary<string, string>> _cssProperty = new Dictionary<string, Dictionary<string, string>>();
        Dictionary<string, Dictionary<string, string>> _LOAllClass = new Dictionary<string, Dictionary<string, string>>();
        Dictionary<string, string> refDict = new Dictionary<string, string>();
        Dictionary<string, string> _LOClass = new Dictionary<string, string>();
        private Dictionary<string, string> _LOProperty = new Dictionary<string, string>();
        OOMapProperty mapProperty = new OOMapProperty();
        public bool IsMacroEnable =  false;
        protected bool isFixedLengthEnabled = false;
        
        //Marks_crop - Declaration
        private const double _lineLength = 0.8;
        private const double _gapToMargin = .2;

        float _PageWidth = 0.00F;
        float _leftMargin = 0.00F;
        float _rightMargin = 0.00F;
        float _rightPosition = 0.00F;
        float _PageHeight = 0.00F;
        float _topMargin = 0.00F;
        float _bottomMargin = 0.00F;
        float _bottomPosition = 0.00F;
        bool _isFirstpageDimensionChanged = false; // TD-190(marks:crop)
        public bool _multiLanguageHeader = false;
        private string _projectType = string.Empty;
        private string _enableKerning = "true";

        public string _customFootnoteCaller;
        public string _customXRefCaller;
        public string _hideVerseNumberOne;
        public string _hideSpaceVerseNumber = "False";

        public string _splitFileByLetter = "False";


        private bool _isFromExe = false;
        private bool _isCenterTabStopNeeded = true;
        private int _guidewordLength;

        public Dictionary<string, Dictionary<string, string>> CreateStyles(PublicationInformation projInfo, Dictionary<string, Dictionary<string, string>> cssProperty, string outputFileName)
        {
            
            try
            {
                string outputFile = Path.GetFileNameWithoutExtension(projInfo.DefaultXhtmlFileWithPath);
                string strStylePath = Common.PathCombine(projInfo.TempOutputFolder, outputFileName);
                _isFromExe = Common.CheckExecutionPath();
                _projectType = projInfo.ProjectInputType;
                _projInfo = projInfo;
                _cssProperty = cssProperty;
                GetGuidewordLength(cssProperty);
                InitializeObject(outputFile); // Creates new Objects
                EnableKerning();
                LoadAllProperty();  // Loads all properties
                LoadSpellCheck();
                FixedLineHeightStatus();
                CreateODTStyles(strStylePath,_cssProperty); 
                CreateCssStyle(); 
                CreatePageStyle();
                AddTagStyle(); // Add missing tags in styles.xml (h1,h2,..)
                CloseODTStyles();  // Close Styles.xml for odt
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return _LOAllClass;
        }

        /// <summary>
        /// Get Guideword Length
        /// </summary>
        private void GetGuidewordLength(Dictionary<string, Dictionary<string, string>> IdAllClass)
        {
            if (_projInfo.ProjectInputType.ToLower() == "dictionary")
            {
                if (IdAllClass.ContainsKey("guidewordLength") && IdAllClass["guidewordLength"].ContainsKey("guideword-length"))
                {
                    int a;
                    if (int.TryParse(IdAllClass["guidewordLength"]["guideword-length"], out a))
                    {
                        _guidewordLength = Convert.ToInt16(IdAllClass["guidewordLength"]["guideword-length"]);
                    }
                }
                _guidewordLength = _guidewordLength > 0 ? _guidewordLength : 99;
            }
        }

        private void EnableKerning()
        {
            if(Common.IsUnixOS())
            {
                _enableKerning = "false";
            }
        }


        private string GetLanguageForReversalNumber()
        {
            string language = "en";
            Dictionary<string, string> AllStyles = new Dictionary<string, string>();

            XmlDocument xdoc = Common.DeclareXMLDocument(true);
            xdoc.Load(_projInfo.DefaultXhtmlFileWithPath);
            string xPath = "//span[@class='revsensenumber']";
            XmlNodeList nodes = xdoc.SelectNodes(xPath);
            if(nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    var xmlAttributeCollection = nodes[i].Attributes;
                    if (xmlAttributeCollection != null)
                    if(xmlAttributeCollection["lang"] != null)
                    {
                        if (xmlAttributeCollection["lang"].Value != language)
                        {
                            language = xmlAttributeCollection["lang"].Value;
                            break;
                        }
                    }
                }
            }
            return language;
        }

        private void FixedLineHeightStatus()
        {
            if (_cssProperty.ContainsKey("@page") && _cssProperty["@page"].ContainsKey("-ps-fixed-line-height"))
            {
                isFixedLengthEnabled = Convert.ToBoolean(_cssProperty["@page"]["-ps-fixed-line-height"]);
            }
        }

        #region CreateCssStyle
        /// <summary>
        //  <style:style style:name="a" style:family="paragraph" style:parent-style-name="none"> *
        //  <style:paragraph-properties fo:text-align="left" /> 
        //  <style:text-properties fo:font-size="24pt" style:font-size-complex="24pt" fo:color="#ff0000" />
        //  </style:style>
        /// </summary>
        private void CreateCssStyle()
        {
            Dictionary<string, string> beforeCounter = new Dictionary<string, string>();
            var isDirectionChange = _cssProperty.ContainsKey("scrBody") &&
                                    _cssProperty["scrBody"].ContainsKey("direction") &&
                                    _cssProperty["scrBody"]["direction"] == "rtl";
            foreach (KeyValuePair<string, Dictionary<string, string>> cssClass in _cssProperty)
            {
                bool isFootnoteStyle = false;
                if (cssClass.Key.StartsWith("@page") || cssClass.Key.EndsWith("..after") || cssClass.Key.EndsWith("..before"))
                {
                    if (cssClass.Key.EndsWith("..before")) //TD-3051 
                    {
                        if (cssClass.Value.ContainsKey("counter-increment"))
                        {
                            string clsName = cssClass.Key.Replace("..before", "");
                            beforeCounter[clsName] = cssClass.Value["counter-increment"];
                        }
                    }

                    continue;
                }
                string className = cssClass.Key;
                if (className.Trim().Length == 0) continue;
                Dictionary<string, string> propValue = new Dictionary<string, string>();
                propValue = PropertyReplace(cssClass.Key, cssClass.Value);

                string familyType = "paragraph";

                if (cssClass.Key.Contains("..footnote-marker") || cssClass.Key.Contains("..footnote-call"))
                {
                    isFootnoteStyle = true;
                    familyType = "text";
                }

                _writer.WriteStartElement("style:style");
                _writer.WriteAttributeString("style:name", className);
                _writer.WriteAttributeString("style:family", familyType); // "paragraph" will override by ContentXML.cs
                _writer.WriteAttributeString("style:parent-style-name", "none");

                _paragraphProperty.Clear();
                _textProperty.Clear();
                _columnProperty.Clear();

                _LOClass = new Dictionary<string, string>();

                if (familyType == "paragraph" && isDirectionChange && !propValue.ContainsKey("direction"))
                {
                    propValue.Add("direction", "rtl");
                }

                _LOProperty = mapProperty.IDProperty(propValue, isFixedLengthEnabled);
                if (cssClass.Key == "revsensenumber")
                {
                    if (!_LOProperty.ContainsKey("font-family"))
                    {
                        string languageFont = "span_." + GetLanguageForReversalNumber();
                        if (_cssProperty.ContainsKey(languageFont) && _cssProperty[languageFont].ContainsKey("font-family"))
                        {
                            _LOProperty.Add("font-family", _cssProperty[languageFont]["font-family"]);
                        }
                    }
                }
                foreach (KeyValuePair<string, string> property in _LOProperty)
                {
                    _LOClass[property.Key] = property.Value;

                    string propName = property.Key;
                    if (_allParagraphProperty.ContainsKey(propName))
                    {
                        if (!_paragraphProperty.ContainsKey(property.Key))
                        {
                            string prefix = _allParagraphProperty[property.Key].ToString();
                            _paragraphProperty[prefix + property.Key] = property.Value;
                        }
                    }
                    else if (_allTextProperty.ContainsKey(propName))
                    {
                        if (!_textProperty.ContainsKey(property.Key))
                        {
                            string prefix = _allTextProperty[property.Key].ToString();
                            _textProperty[prefix + property.Key] = property.Value;

                        }
                    }
                    else if (_allColumnProperty.ContainsKey(propName)) 
                    {
                        MapColumnProperty(property, propName);
                    }

                }
                _LOAllClass[cssClass.Key] = _LOClass;

                if (_paragraphProperty.Count > 0 && !isFootnoteStyle)
                {
                    _writer.WriteStartElement("style:paragraph-properties");
                    DropCap();
                    foreach (KeyValuePair<string, string> para in _paragraphProperty)
                    {
                        _writer.WriteAttributeString(para.Key, para.Value);
                    }
                    _writer.WriteEndElement();
                }

                if (_textProperty.Count > 0)
                {
                    _writer.WriteStartElement("style:text-properties");
                    foreach (KeyValuePair<string, string> text in _textProperty)
                    {
                        _writer.WriteAttributeString(text.Key, text.Value);
                    }
                    if (_paragraphProperty.ContainsKey("fo:background-color"))
                    {
                        _writer.WriteAttributeString("fo:background-color", _paragraphProperty["fo:background-color"]);
                    }
                    _writer.WriteEndElement();
                }

                if (_columnProperty.Count > 0) // create a value XML file for content.xml with column property.
                {
                    CreateColumnXMLFile(className);
                }
                _writer.WriteEndElement();
                
            }

            foreach (KeyValuePair<string, string> property in beforeCounter)
            {
                if(_LOAllClass.ContainsKey(property.Key))
                {
                    _LOAllClass[property.Key]["counter-increment"] = property.Value;
                }
            }
        }

        private Dictionary<string, string> PropertyReplace(string className, Dictionary<string, string> OOProperty)
        {
            ListReplace(className,OOProperty);
            PositionReplace(OOProperty);
            BorderReplace(OOProperty);
            return OOProperty;
        }

        private void BorderReplace(Dictionary<string, string> OOProperty)
        {
            string[] positions = new[] { "top", "bottom", "left", "right" };
            foreach (string position in positions)
            {
                BorderPropertyMerge(OOProperty, position);
            }
        }

        private void BorderPropertyMerge(Dictionary<string, string> OOProperty, string pos)
        {
            string key1 = "border-" + pos + "-style";
            if (OOProperty.ContainsKey(key1))
            {

                string property = OOProperty[key1] + " ";
                OOProperty.Remove(key1);
                key1 = "border-" + pos + "-width";
                if (OOProperty.ContainsKey(key1))
                {
                    string point = string.Empty;
                    if (OOProperty[key1].IndexOf("pt") == -1)
                        point = "pt";
                    property += OOProperty[key1] + point + " ";
                    OOProperty.Remove(key1);
                }

                key1 = "border-" + pos + "-color";
                if (OOProperty.ContainsKey(key1))
                {
                    property += OOProperty[key1] + " ";
                    OOProperty.Remove(key1);
                }

                OOProperty["border-" + pos] = property;
            }
        }

        private void ListReplace(string className, Dictionary<string, string> OOProperty)
        {
            string key = "list-style-type";
            if (OOProperty.ContainsKey(key))  //List type - li,ol,ul
                CreateListType(className,OOProperty[key]);
        }

        private void PositionReplace(Dictionary<string, string> OOProperty)
        {
            string key;
            key = "position";
            if (OOProperty.ContainsKey(key) && (OOProperty.ContainsKey("left") || OOProperty.ContainsKey("right")))
            {
                OOProperty.Remove(key);
                if (OOProperty.ContainsKey("left"))
                {
                    OOProperty["margin-left"] = OOProperty["left"];
                    OOProperty.Remove("left");
                }
                else if (OOProperty.ContainsKey("right"))
                {

                    if (OOProperty["right"].IndexOf("-") >= 0)
                    {
                        OOProperty["margin-left"] = OOProperty["right"];
                    }
                    else
                    {
                        OOProperty["margin-left"] = "-" + OOProperty["right"];
                    }
                    OOProperty.Remove("right");
                }        
            }
        }

        private void DropCap()
        {
            if (_paragraphProperty.ContainsKey("fo:float") && _paragraphProperty.ContainsKey("style:vertical-align"))
            {
                string lines = "2";
                foreach (KeyValuePair<string, string> para in _paragraphProperty)
                {
                    if (para.Key == "style:line-spacing")
                    {
                        string result = AdjustLineHeight(para.Value);
                        if (result != string.Empty )
                        {
                            _writer.WriteAttributeString(para.Key, result); 
                        }
                    }
                    else
                    {
                        _writer.WriteAttributeString(para.Key, para.Value);    
                    }
                }
                _writer.WriteStartElement("style:drop-cap");
                if (_textProperty.ContainsKey("fo:font-size"))
                {
                    if (_textProperty["fo:font-size"].IndexOf('%') > 0)
                    {
                        lines = (int.Parse(_textProperty["fo:font-size"].Replace("%", "")) / 100).ToString();
                    }
                    _textProperty.Remove("fo:font-size");
                }
                _writer.WriteAttributeString("style:lines", lines);
                _writer.WriteAttributeString("style:distance", "0.20cm");
                _writer.WriteAttributeString("style:length", "1"); // No of Character
                _writer.WriteEndElement();
                _paragraphProperty.Clear();
            }
        }

        private string AdjustLineHeight(string val)
        {
            string result = string.Empty;
            try
            {
                string without_pt = val.Replace("pt", "");
                float value = float.Parse(without_pt.Replace("%", ""));
                float lineHeight = 0;
                int ancestorFontSize = 11;
                if (val.IndexOf("%") > 0)
                {
                    lineHeight = (value - 100) * ancestorFontSize / 100;
                    lineHeight = lineHeight / 2;
                }
                else if (val.IndexOf("pt") > 0)
                {
                    lineHeight = (value - ancestorFontSize) / 2;
                }
                if (lineHeight > 0)
                {
                    result = lineHeight + "pt";
                }
            }
            catch (Exception)
            {
                result = string.Empty;
            }

            return result;
        }

        /// <summary>
        /// Create custom List
        /// list-style-type: none;
        /// </summary>
        /// <param name="listType">Type Name</param>
        private void CreateListType(string className, string listType)
        {
            string listName = "Listdecimal";
            string numFormat = "1";
            string numSuffix = ".";
            if (listType == "none")
            {
                listName = "Listnone";
                numFormat = string.Empty;
                numSuffix = string.Empty;
            }
            else if (listType == "disc")
            {
                listName = "Listdisc";
                numFormat = "\u2022"; // solid bullet
            }
            else if (listType == "circle")
            {
                listName = "Listcircle";
                numFormat = "\u25e6"; // open bullet
            }
            else if (listType == "square")
            {
                listName = "Listsquare";
                numFormat = "\u25aa"; // square bullet
            }
            else if (listType == "decimal")
            {
                listName = "Listdecimal";
                numFormat = "1";
            }
            else if (listType == "lower-roman")
            {
                listName = "Listlowerroman";
                numFormat = "i";
            }
            else if (listType == "upper-roman")
            {
                listName = "Listupperroman";
                numFormat = "I";
            }
            else if (listType == "lower-alpha")
            {
                listName = "Listloweralpha";
                numFormat = "a";
            }
            else if (listType == "upper-alpha")
            {
                listName = "Listupperalpha";
                numFormat = "A";
            }

            switch (listType)
            {

                case "disc":
                case "circle":
                case "square":
                    {
                        _writer.WriteStartElement("text:list-style");
                        _writer.WriteAttributeString("style:name", className);
                        _writer.WriteStartElement("text:list-level-style-bullet");
                        _writer.WriteAttributeString("text:level", "1");
                        _writer.WriteAttributeString("text:style-name", "Bullet_20_Symbols");
                        _writer.WriteAttributeString("style:num-suffix", numSuffix);
                        _writer.WriteAttributeString("text:bullet-char", numFormat);
                        break;
                    }
                case "none":
                case "decimal":
                case "lower-roman":
                case "upper-roman":
                case "lower-alpha":
                case "upper-alpha":
                    {
                        _writer.WriteStartElement("text:list-style");
                        _writer.WriteAttributeString("style:name", className);
                        _writer.WriteStartElement("text:list-level-style-number");
                        _writer.WriteAttributeString("text:level", "1");
                        _writer.WriteAttributeString("text:style-name", "Numbering_20_Symbols");
                        _writer.WriteAttributeString("style:num-suffix", numSuffix);
                        _writer.WriteAttributeString("style:num-format", numFormat);
                        break;
                    }
            }

            _writer.WriteStartElement("style:list-level-properties");
            _writer.WriteAttributeString("text:list-level-position-and-space-mode", "label-alignment");
            _writer.WriteStartElement("style:list-level-label-alignment");
            _writer.WriteAttributeString("text:label-followed-by", "listtab");
            _writer.WriteAttributeString("text:list-tab-stop-position", "0.5in");
            _writer.WriteAttributeString("fo:text-indent", "-0.25in");
            _writer.WriteAttributeString("fo:margin-left", "0.5in");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        private void MapColumnProperty(KeyValuePair<string, string> property, string propName)
        {
            string prefix = _allColumnProperty[propName];
            if (propName == "column-rule-width" || propName == "column-rule-style" || propName == "column-rule-color")
            {
                if (propName == "column-rule-width")
                    propName = "width";
                if (propName == "column-rule-color")
                    propName = "color";

                if (propName != "column-rule-style")  // still not handled
                    _columnSep[prefix + propName] = property.Value; // Column rule Dictionary
            }
            else
            {
                _columnProperty[prefix + propName] = property.Value;
            }
        }

        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Create value XML file for storing section and column info
        /// 
        /// <list> 
        /// </list>
        /// </summary>
        /// <returns>class Name</returns>
        /// -------------------------------------------------------------------------------------------        
        /// 
        private void CreateColumnXMLFile(string className)
        {
            string path = Common.PathCombine(Path.GetTempPath(), "_" + className.Trim() + ".xml");

            XmlTextWriter writerCol = null;
            try
            {
                writerCol = new XmlTextWriter(path, null);
                writerCol.Formatting = Formatting.Indented;
                writerCol.WriteStartDocument();
                writerCol.WriteStartElement("office:document-content");
                writerCol.WriteAttributeString("xmlns:office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
                writerCol.WriteAttributeString("xmlns:style", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
                writerCol.WriteAttributeString("xmlns:fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");
                writerCol.WriteAttributeString("xmlns:text", "urn:oasis:names:tc:opendocument:xmlns:text:1.0");
                writerCol.WriteAttributeString("style:parent-style-name", "none");

                writerCol.WriteStartElement("style:style");
                writerCol.WriteAttributeString("style:name", "Sect_" + className.Trim());
                writerCol.WriteAttributeString("style:family", "section");

                writerCol.WriteStartElement("style:section-properties");
                if (_columnProperty.ContainsKey("text:dont-balance-text-columns"))
                {
                    writerCol.WriteAttributeString("text:dont-balance-text-columns", _columnProperty["text:dont-balance-text-columns"]);
                    _columnProperty.Remove("text:dont-balance-text-columns");
                }
                else
                {
                writerCol.WriteAttributeString("text:dont-balance-text-columns", "false");
                }

                if (_sectionProperty != null && _sectionProperty.ContainsKey("style:writing-mode"))
                {
                    writerCol.WriteAttributeString("style:writing-mode", _sectionProperty["style:writing-mode"]);
                }

                writerCol.WriteAttributeString("style:editable", "false");

                writerCol.WriteStartElement("style:columns");


                string columnGap = "0pt";
                byte columnCount = 0;
                foreach (KeyValuePair<string, string> text in _columnProperty)
                {
                    if (text.Key == "fo:column-gap")
                    {
                        columnGap = text.Value;
                    }
                    else if (text.Key == "fo:column-count")
                    {
                        columnCount = (byte)Common.ConvertToInch(text.Value);
                    }
                }

                writerCol.WriteAttributeString("fo:column-count", columnCount.ToString());
                float pageWidth = 0;
                float relWidth = 0;
                float spacing = 0;
                float colWidth = 0;
                float marginLeft = 0;
                float marginRight = 0;
                if (columnCount > 1)
                {

                    Dictionary<string, string> pageStyle = _cssProperty["@page"];

                    pageWidth = Common.ConvertToInch(pageStyle["page-width"] + "pt");
                    marginLeft = Common.ConvertToInch(pageStyle["margin-left"] + "pt");
                    marginRight = Common.ConvertToInch(pageStyle["margin-right"] + "pt");

                    spacing = Common.ConvertToInch(columnGap) / 2;
                    relWidth = (pageWidth - (spacing * (columnCount * 2))) / columnCount;

                    if (columnGap.IndexOf("em") > 0 || columnGap.IndexOf("%") > 0) // Column Gap will be calculte in content.xml
                    {
                        colWidth = (pageWidth - marginLeft - marginRight) / 2.0F;
                    }
                    else
                    {
                        colWidth = (pageWidth - spacing - marginLeft - marginRight) / 2.0F;

                        var width = Common.UnitConverter(String.Format(CultureInfo.GetCultureInfo("en-US"), "{0}{1}",colWidth,"in"), "pt");
                        colWidth = float.Parse(width, CultureInfo.GetCultureInfo("en-US"));
                        Dictionary<string, string> columnWidth = new Dictionary<string, string>();
                        columnWidth["ColumnWidth"] = colWidth.ToString();
                        _LOAllClass["SectColumnWidth_" + className.Trim()] = columnWidth;

                    }

                    _styleName.ColumnWidth = colWidth; // for picture size calculation

                    string relWidthStr = relWidth.ToString() + "*";
                    for (int i = 1; i <= columnCount; i++)
                    {
                        writerCol.WriteStartElement("style:column");
                        if (i == 1)
                        {
                            writerCol.WriteAttributeString("style:rel-width", relWidthStr);
                            writerCol.WriteAttributeString("fo:start-indent", 0 + "in");
                            writerCol.WriteAttributeString("fo:end-indent", spacing + "in");
                        }
                        else if (i == columnCount)
                        {
                            writerCol.WriteAttributeString("style:rel-width", relWidthStr);
                            writerCol.WriteAttributeString("fo:start-indent", spacing + "in");
                            writerCol.WriteAttributeString("fo:end-indent", 0 + "in");
                        }
                        else
                        {
                            writerCol.WriteAttributeString("style:rel-width", relWidthStr);
                            writerCol.WriteAttributeString("fo:start-indent", spacing + "in");
                            writerCol.WriteAttributeString("fo:end-indent", spacing + "in");
                        }
                        writerCol.WriteEndElement();
                    }

                    if (columnGap.IndexOf("em") > 0 || columnGap.IndexOf("%") > 0)
                    {
                        Dictionary<string, string> pageProperties = new Dictionary<string, string>();
                        pageProperties["pageWidth"] = pageWidth.ToString();
                        pageProperties["columnCount"] = columnCount.ToString();
                        pageProperties["marginLeft"] = marginLeft.ToString();
                        if (columnGap.IndexOf("em") > 0)
                            pageProperties["columnGap"] = columnGap.ToString();
                        else if (columnGap.IndexOf("%") > 0)
                        {
                            try
                            {
                                int convertToEm = int.Parse(columnGap.Replace("%", "")) / 100;
                                pageProperties["columnGap"] = columnGap;

                            }
                            catch (Exception)
                            {
                                pageProperties["columnGap"] = "1em";

                            }
                        }
                        _LOAllClass["Sect_" + className.Trim()] = pageProperties;
                    }
                }

                if (_columnSep.Count > 0)
                {
                    writerCol.WriteStartElement("style:column-sep");
                    foreach (KeyValuePair<string, string> text in _columnSep)
                    {
                        writerCol.WriteAttributeString(text.Key, text.Value);
                    }
                    writerCol.WriteEndElement();
                }

                writerCol.WriteEndElement();
                writerCol.WriteEndElement();
                writerCol.WriteEndElement();

                writerCol.WriteEndElement();
                writerCol.WriteEndDocument();
                writerCol.Flush();
                writerCol.Close();
            }
            catch (Exception ex)
            {
                // close out the file if we opened it
                if (writerCol != null)
                {
                    writerCol.Flush();
                    writerCol.Close();
                }
                Console.Write(ex.Message);
            }
        }

        #endregion

        #region Page Layouts and Master Page - Header/Footer

        private void CreatePageStyle()   // PageHeaderFooter() in odt conversion 
        {
            CreateHeaderFooter();
            CreatePageFirstPage();
        }

        private void CreatePageFirstPage()
        {
            string pageName = "@page";
            _pageLayoutProperty = ProcessPageProperty(pageName);

            pageName = "@page:first";
            _firstPageLayoutProperty = ProcessPageProperty(pageName);

            pageName = "@page:left";
            _leftPageLayoutProperty = ProcessPageProperty(pageName);

            pageName = "@page:right";
            _rightPageLayoutProperty = ProcessPageProperty(pageName);

            pageName = "@page-footnotes";
            _styleName.FootNoteSeperator = ProcessPageProperty(pageName);

            if (_leftPageLayoutProperty.Count > 0 || _rightPageLayoutProperty.Count > 0)
            {
                isMirrored = true;
                if (_rightPageLayoutProperty.Count == 0)
                {
                    _rightPageLayoutProperty = _pageLayoutProperty;
                }
                else if (_leftPageLayoutProperty.Count == 0)
                {
                    _leftPageLayoutProperty = _pageLayoutProperty;
                }
            }
        }

        private Dictionary<string, string> ProcessPageProperty(string pageName)
        {
            Dictionary<string, string> pageLayoutProperty = new Dictionary<string, string>();
            if (_cssProperty.ContainsKey(pageName))
            {
                Dictionary<string, string> cssClass1 = _cssProperty[pageName];
                _LOProperty = mapProperty.IDProperty(cssClass1, isFixedLengthEnabled);
                foreach (KeyValuePair<string, string> para in _LOProperty)
                    if (para.Key.ToLower() == "-ps-referenceformat-string")
                    {
                        _styleName.ReferenceFormat = para.Value.Replace("\"", "");
                    }
                    else if (para.Key.ToLower() == "-ps-custom-footnote-caller")
                    {
                        _customFootnoteCaller = para.Value.Replace("\"", "");
                    }
                    else if (para.Key.ToLower() == "-ps-custom-xref-caller")
                    {
                        _customXRefCaller = para.Value.Replace("\"", "");
                    }
                    else if (para.Key.ToLower() == "-ps-hide-versenumber-one")
                    {
                        _hideVerseNumberOne = para.Value.Replace("\"", "");
                    }
                    else if (para.Key.ToLower() == "-ps-split-file-by-letter")
                    {
                        _splitFileByLetter = para.Value.Replace("\"", "");
                    }
                    else if (para.Key.ToLower() == "-ps-hide-space-versenumber")
                    {
                        _hideSpaceVerseNumber = para.Value.Replace("\"", "");
                    }
                    else if (para.Key == "border-top"
                             || para.Key == "border-top-style"
                             || para.Key == "border-top-width"
                             || para.Key == "border-top-color"
                             || para.Key == "border-bottom"
                             || para.Key == "border-left"
                             || para.Key == "border-right"
                             || para.Key == "margin-top"
                             || para.Key == "margin-bottom"
                             || para.Key == "margin-left"
                             || para.Key == "margin-right"
                             || para.Key == "padding-top"
                             || para.Key == "padding-bottom"
                             || para.Key == "padding-left"
                             || para.Key == "padding-right"
                             || para.Key == "visibility")
                    {
                        pageLayoutProperty[_allPageLayoutProperty[para.Key].ToString() + para.Key] =

                            para.Value;
                    }
                    else if (para.Key == "size" || para.Key == "page-width" || para.Key == "page-height")
                    {
                        if (para.Value.ToLower() == "landscape" || para.Value.ToLower() == "portrait" ||
                            para.Value.ToLower() == "auto")
                        {
                            pageLayoutProperty["style:print-orientation"] = para.Value.ToLower();
                        }
                        else
                        {
                            pageLayoutProperty[_allPageLayoutProperty[para.Key].ToString() + para.Key] = para.Value;
                        }
                    }
                    else if (para.Key == "color" || para.Key == "background-color")
                    {
                        pageLayoutProperty[_allPageLayoutProperty[para.Key].ToString() + para.Key] = para.Value;
                    }
                    else if (para.Key == "marks" && para.Value == "crop")
                    {
                    }
                    else if (para.Key.ToLower() == "dictionary")
                    {
                        if (para.Value == "true")
                        {
                        }
                    }
                    else
                    {
                    }
            }
            return pageLayoutProperty;
        }

        private void CreateHeaderFooter()
        {
            string pageName = "@page";
            SetPageHeaderFooter(pageName);

            pageName = "@page:first";
            SetPageHeaderFooter(pageName);

            pageName = "@page:left";
            SetPageHeaderFooter(pageName);

            pageName = "@page:right";
            SetPageHeaderFooter(pageName);
        }

        private void SetPageHeaderFooter(string pageName)
        {
            int headerFooterIndex = 0;

            if (pageName == "@page")
            {
                headerFooterIndex = 6;
            }
            else if (pageName.IndexOf("left") > 0)
            {
                headerFooterIndex = 12;
            }
            else if (pageName.IndexOf("right") > 0)
            {
                headerFooterIndex = 18;
            }
            string[] searchKey = { "top-left", "top-center", "top-right", "bottom-left", "bottom-center", "bottom-right" };
            for (int i = 0; i < 6; i++)
            {
                string currentPagePosition = pageName + "-" + searchKey[i];   // Ex: page:first-topleft
                if (_cssProperty.ContainsKey(currentPagePosition))
                {
                    if (true)
                    {
                        Dictionary<string, string> cssProp = _cssProperty[currentPagePosition];

                        _LOProperty = mapProperty.IDProperty(cssProp, isFixedLengthEnabled);
                        foreach (KeyValuePair<string, string> para in _LOProperty)
                        {
                            if (para.Key == "content")
                            {
                                string writingString = para.Value.Replace("\"", "");
                                writingString = para.Value.Replace("'", "");
                                if (writingString.ToLower() == "normal" || writingString.ToLower() == "none")
                                {
                                    if (pageName != "@page")
                                    {
                                        _firstPageContentNone.Add(i); // avoiding first page content:normal or none.
                                    }
                                    continue;
                                }
                                _pageHeaderFooter[i + headerFooterIndex][para.Key] = writingString;
                            }
                            else if (para.Key == "-ps-referenceformat")
                            {
                                if (pageName.IndexOf("first") == -1 && _styleName.ReferenceFormat.Length == 0)
                                {
                                    SetReferenceFormat(pageName, para.Value.Replace("\"", ""));
                                }
                            }
                            else
                            {
                                string prefix = string.Empty;

                                if (_allParagraphProperty.ContainsKey(para.Key))
                                    prefix = _allParagraphProperty[para.Key];
                                else if (_allTextProperty.ContainsKey(para.Key))
                                    prefix = _allTextProperty[para.Key];
                                else
                                {
                                    prefix = "";
                                }
                                string check = prefix + para.Key;
                                if (!para.Key.StartsWith("-")) // user defined functions not allowed now
                                    _pageHeaderFooter[i + headerFooterIndex][prefix + para.Key] = para.Value.Replace(
                                        "'", "");
                            }
                        }
                    }

                }
            }
        }

        private void SetReferenceFormat(string pageName, string value)
        {
            if (value != string.Empty)
            {
                if (!refDict.ContainsKey(pageName))
                    refDict.Add(pageName, value);
                _LOAllClass["ReferenceFormat"] = refDict;
            }
        }

        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Generate last block of Styles.xml
        /// 
        /// <list> 
        /// </list>
        /// </summary>
        /// <returns> </returns>
        /// -------------------------------------------------------------------------------------------
        private void CloseODTStyles() // todo rename this as Create Page styles or setup
        {
            try
            {
                InitializePageHeadFootDictionary();

                CreateDefaultStyles();

                CreateOfficeStylesAttribue();

                ODTPageFooter(); // Creating Footer Information for OpenOffice Document.

                MasterPageCreation();

                CloseDocument();

                CloseVerboseWriter();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                _writer.Flush();
                _writer.Close();
            }
        }

        private void CloseVerboseWriter()
        {
            try
            {
                if (_verboseWriter.ShowError && _verboseWriter.ErrorWritten)  // error file closing
                {
                    _verboseWriter.WriteError("</table>");
                    _verboseWriter.WriteError("</body>");
                    _verboseWriter.WriteError("</html>");
                    _verboseWriter.Close();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private void CloseDocument()
        {
            _writer.WriteEndDocument();
            _writer.Flush();
            _writer.Close();
        }

        private void CreateOfficeStylesAttribue()
        {
            //office:styles Attributes.
            //// "Standard"
            _writer.WriteStartElement("style:style");
            _writer.WriteAttributeString("style:name", "Standard");
            _writer.WriteAttributeString("style:family", "paragraph");
            _writer.WriteAttributeString("style:class", "text");
            _writer.WriteEndElement();

            //// "HyperLink"
            _writer.WriteStartElement("style:style");
            _writer.WriteAttributeString("style:name", "Internet_20_link");
            _writer.WriteAttributeString("style:display-name", "Internet link");
            _writer.WriteAttributeString("style:family", "text");
            _writer.WriteStartElement("style:text-properties");
            _writer.WriteAttributeString("fo:color", "#000000");
            _writer.WriteAttributeString("style:text-line-through-style", "none");
            _writer.WriteAttributeString("style:text-underline-style", "none");
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            _writer.WriteStartElement("style:style");
            _writer.WriteAttributeString("style:name", "Visited_20_Internet_20_Link");
            _writer.WriteAttributeString("style:display-name", "Visited Internet Link");
            _writer.WriteAttributeString("style:family", "text");
            _writer.WriteStartElement("style:text-properties");
            _writer.WriteAttributeString("fo:color", "#000000");
            _writer.WriteAttributeString("style:text-line-through-style", "none");
            _writer.WriteAttributeString("style:text-underline-style", "none");
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            //Insert Fixed Height Hidden Paragraph for TD-2912
            _writer.WriteStartElement("style:style");
            _writer.WriteAttributeString("style:name", "block_5f_p");
            _writer.WriteAttributeString("style:display-name", "block_p");
            _writer.WriteAttributeString("style:family", "paragraph");
            _writer.WriteStartElement("style:paragraph-properties");
            _writer.WriteAttributeString("fo:margin-left", "0.1665in");
            _writer.WriteAttributeString("fo:margin-right", "0in");
            _writer.WriteAttributeString("fo:line-height", "0.2in");
            _writer.WriteAttributeString("fo:text-align", "start");
            _writer.WriteAttributeString("style:justify-single-word", "false");
            _writer.WriteAttributeString("fo:orphans", "2");
            _writer.WriteAttributeString("fo:text-indent", "-0.1665in");
            _writer.WriteAttributeString("style:auto-text-indent", "false");
            _writer.WriteAttributeString("style:page-number", "auto");
            _writer.WriteAttributeString("fo:background-color", "transparent");
            _writer.WriteAttributeString("fo:padding-left", "0in");
            _writer.WriteAttributeString("fo:padding-right", "0in");
            _writer.WriteAttributeString("fo:padding-top", "0.0138in");
            _writer.WriteAttributeString("fo:padding-bottom", "0.028in");
            _writer.WriteAttributeString("fo:border", "none");
            _writer.WriteAttributeString("fo:keep-with-next", "always");
            _writer.WriteEndElement();
            _writer.WriteStartElement("style:text-properties");
            _writer.WriteAttributeString("text:display", "none");
            _writer.WriteEndElement();
            _writer.WriteEndElement();


            //office:styles Attributes.
            //// "Header"
            _writer.WriteStartElement("style:style");
            _writer.WriteAttributeString("style:name", "Header");
            _writer.WriteAttributeString("style:family", "paragraph");
            _writer.WriteAttributeString("style:parent-style-name", "headerFontStyleName");
            _writer.WriteAttributeString("style:next-style-name", "Text_20_body");
            _writer.WriteAttributeString("style:class", "text");

            _writer.WriteStartElement("style:paragraph-properties");//style:paragraph-properties
            _writer.WriteAttributeString("fo:margin-top", "0.0005in");
            _writer.WriteAttributeString("fo:margin-bottom", "0.0835in");
            _writer.WriteAttributeString("fo:padding-bottom", "0.0499in");
            _writer.WriteAttributeString("fo:keep-with-next", "always");

            InsertHeaderRule(); //Note Header ruler

            _writer.WriteAttributeString("text:number-lines", "false");
            _writer.WriteAttributeString("text:line-number", "0");

            _writer.WriteStartElement("style:tab-stops");//style:tab-stops

            if(_isCenterTabStopNeeded)
            {
                _writer.WriteStartElement("style:tab-stop");//style:tab-stop
            }

            float borderLeft;
            if (_pageLayoutProperty.ContainsKey("fo:border-left"))
            {
                string[] parameters = _pageLayoutProperty["fo:border-left"].Split(' ');
                string pageBorderLeft = "0pt";
                foreach (string param in parameters)
                {
                    if (Common.ValidateNumber(param[0].ToString() + "1"))
                    {
                        pageBorderLeft = param;
                        break;
                    }
                }
                borderLeft = Common.ConvertToInch(pageBorderLeft);
            }
            else
            {
                borderLeft = 0F;
            }

            float width = Common.ConvertToInch(_pageLayoutProperty["fo:page-width"]);
            float left = Common.ConvertToInch(_pageLayoutProperty["fo:margin-left"]);
            float right = Common.ConvertToInch(_pageLayoutProperty["fo:margin-left"]);
            float mid = width / 2F - left - borderLeft;
            float rightGuide = (width - left - right);

            if (_isCenterTabStopNeeded)
            {
                _writer.WriteAttributeString("style:position", mid.ToString() + "in");
                _writer.WriteAttributeString("style:type", "center");
                _writer.WriteEndElement();
            }

            _writer.WriteStartElement("style:tab-stop");//style:tab-stop
            _writer.WriteAttributeString("style:position", rightGuide.ToString() + "in");
            _writer.WriteAttributeString("style:type", "right");
            _writer.WriteEndElement();

            _writer.WriteEndElement();//style:tab-stops
            _writer.WriteEndElement();//style:paragraph-properties

            //TD-2682
            _writer.WriteStartElement("style:text-properties");//style:text-properties
            string headerFontName = Common.GetHeaderFontName(_cssProperty, _projInfo.DefaultCssFileWithPath);
            //TD-3304
            _writer.WriteAttributeString("style:font-name", headerFontName);
            //TD-2815
            string headerFontWeight = Common.GetHeaderFontWeight(_cssProperty);
            _writer.WriteAttributeString("fo:font-weight", headerFontWeight);

            //TD-2819
            string headerFontSize = Common.GetHeaderFontSize(_cssProperty);
            _writer.WriteAttributeString("fo:font-size", headerFontSize);
            
            _writer.WriteEndElement();
            _writer.WriteEndElement();


            //// "Footer"
            _writer.WriteStartElement("style:style");
            _writer.WriteAttributeString("style:name", "Footer");
            _writer.WriteAttributeString("style:family", "paragraph");
            _writer.WriteAttributeString("style:parent-style-name", "Standard");
            _writer.WriteAttributeString("style:next-style-name", "Text_20_body");
            _writer.WriteAttributeString("style:class", "text");

            _writer.WriteStartElement("style:paragraph-properties");//style:paragraph-properties
            _writer.WriteAttributeString("fo:margin-top", "0.0005in");
            _writer.WriteAttributeString("fo:margin-bottom", "0.0835in");
            _writer.WriteAttributeString("fo:keep-with-next", "always");

            _writer.WriteAttributeString("text:number-lines", "false");
            _writer.WriteAttributeString("text:line-number", "0");

            _writer.WriteStartElement("style:tab-stops");//style:tab-stops


            if (_isCenterTabStopNeeded)
            {
                _writer.WriteStartElement("style:tab-stop");//style:tab-stop
                _writer.WriteAttributeString("style:position", mid.ToString() + "in");
                _writer.WriteAttributeString("style:type", "center");
                _writer.WriteEndElement();
            }


            _writer.WriteStartElement("style:tab-stop");//style:tab-stop
            _writer.WriteAttributeString("style:position", rightGuide.ToString() + "in");
            _writer.WriteAttributeString("style:type", "right");
            _writer.WriteEndElement();

            _writer.WriteEndElement();//style:tab-stops
            _writer.WriteEndElement();//style:paragraph-properties
            _writer.WriteEndElement();//Footer style

            //office:styles Attributes.
            //// "Text_20_body"
            _writer.WriteStartElement("style:style");
            _writer.WriteAttributeString("style:name", "Text_20_body");
            _writer.WriteAttributeString("style:display-name", "Text body");
            _writer.WriteAttributeString("style:family", "paragraph");
            _writer.WriteAttributeString("style:parent-style-name", "Standard");
            _writer.WriteAttributeString("style:class", "text");
            _writer.WriteStartElement("style:paragraph-properties");
            _writer.WriteAttributeString("fo:margin-top", "0in");
            _writer.WriteAttributeString("fo:margin-bottom", "0.0835in");
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            //office:styles Attributes.
            //// "List"
            _writer.WriteStartElement("style:style");
            _writer.WriteAttributeString("style:name", "List");
            _writer.WriteAttributeString("style:family", "paragraph");
            _writer.WriteAttributeString("style:parent-style-name", "Text_20_body");
            _writer.WriteAttributeString("style:class", "list");
            _writer.WriteStartElement("style:text-properties");
            _writer.WriteAttributeString("style:font-name-asian", "Yi plus Phonetics");
            _writer.WriteAttributeString("style:font-name-complex", "Tahoma1");
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            //office:styles Attributes.
            //// "Caption"
            _writer.WriteStartElement("style:style");
            _writer.WriteAttributeString("style:name", "Caption");
            _writer.WriteAttributeString("style:family", "paragraph");
            _writer.WriteAttributeString("style:parent-style-name", "Standard");
            _writer.WriteAttributeString("style:class", "extra");
            _writer.WriteStartElement("style:paragraph-properties");
            _writer.WriteAttributeString("fo:margin-top", "0.0835in");
            _writer.WriteAttributeString("fo:margin-bottom", "0.0835in");
            _writer.WriteAttributeString("text:number-lines", "false");
            _writer.WriteAttributeString("text:line-number", "0");
            _writer.WriteEndElement();
            _writer.WriteStartElement("style:text-properties");
            _writer.WriteAttributeString("fo:font-size", "12pt");
            _writer.WriteAttributeString("fo:font-style", "italic");
            _writer.WriteAttributeString("style:font-name-asian", "Yi plus Phonetics");
            _writer.WriteAttributeString("style:font-size-asian", "12pt");
            _writer.WriteAttributeString("style:font-style-asian", "italic");
            _writer.WriteAttributeString("style:font-name-complex", "Tahoma1");
            _writer.WriteAttributeString("style:font-size-complex", "12pt");
            _writer.WriteAttributeString("style:font-style-complex", "italic");
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            //office:styles Attributes.
            //// "index"
            _writer.WriteStartElement("style:style");
            _writer.WriteAttributeString("style:name", "Index");
            _writer.WriteAttributeString("style:family", "paragraph");
            _writer.WriteAttributeString("style:parent-style-name", "Standard");
            _writer.WriteAttributeString("style:class", "index");
            _writer.WriteStartElement("style:paragraph-properties");
            _writer.WriteAttributeString("text:number-lines", "false");
            _writer.WriteAttributeString("text:line-number", "0");
            _writer.WriteEndElement();
            _writer.WriteStartElement("style:text-properties");
            _writer.WriteAttributeString("style:font-name-asian", "Yi plus Phonetics");
            _writer.WriteAttributeString("style:font-name-complex", "Tahoma1");
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            _writer.WriteStartElement("style:style");
            _writer.WriteAttributeString("style:name", "Frame_20_contents");
            _writer.WriteAttributeString("style:display-name", "Frame contents");
            _writer.WriteAttributeString("style:family", "paragraph");
            _writer.WriteAttributeString("style:parent-style-name", "headerFontStyleName");
            _writer.WriteAttributeString("style:class", "extra");
            _writer.WriteStartElement("style:paragraph-properties");//style:text-properties
            _writer.WriteAttributeString("fo:text-align", "center");
            _writer.WriteAttributeString("style:justify-single-word", "false");
            _writer.WriteEndElement();
            
            //TD-2682
            _writer.WriteStartElement("style:text-properties");//style:text-properties
            headerFontName = Common.GetHeaderFontName(_cssProperty,_projInfo.DefaultCssFileWithPath);
            _writer.WriteAttributeString("style:font-name", headerFontName);

            //TD-2815
            headerFontWeight = Common.GetHeaderFontWeight(_cssProperty);
            _writer.WriteAttributeString("fo:font-weight", headerFontWeight);

            //TD-2819
            headerFontSize = Common.GetHeaderFontSize(_cssProperty);
            _writer.WriteAttributeString("fo:font-size", headerFontSize);

            _writer.WriteEndElement();
            _writer.WriteEndElement();

            _writer.WriteStartElement("style:style");
            _writer.WriteAttributeString("style:name", "Contents_20_Heading");
            _writer.WriteAttributeString("style:display-name", "Contents Heading");
            _writer.WriteAttributeString("style:family", "paragraph");
            _writer.WriteAttributeString("style:parent-style-name", "Heading");
            _writer.WriteAttributeString("style:class", "index");
            _writer.WriteStartElement("style:paragraph-properties");//style:text-properties
            _writer.WriteAttributeString("fo:margin", "100%");
            _writer.WriteAttributeString("fo:margin-left", "0in");
            _writer.WriteAttributeString("fo:text-indent", "0in");
            _writer.WriteAttributeString("fo:margin-right", "0in");
            _writer.WriteAttributeString("style:auto-text-indent", "false");
            _writer.WriteAttributeString("text:number-lines", "false");
            _writer.WriteAttributeString("text:line-number", "0");
            _writer.WriteEndElement();
            _writer.WriteStartElement("style:text-properties");//style:text-properties
            _writer.WriteAttributeString("fo:font-size", "16pt");
            _writer.WriteAttributeString("fo:font-weight", "bold");
            _writer.WriteAttributeString("fo:font-size-asian", "16pt");
            _writer.WriteAttributeString("fo:font-weight-asian", "bold");
            _writer.WriteAttributeString("fo:font-size-complex", "16pt");
            _writer.WriteAttributeString("fo:font-weight-complex", "bold");
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            _writer.WriteStartElement("style:style");
            _writer.WriteAttributeString("style:name", "Contents_20_1");
            _writer.WriteAttributeString(" style:display-name", "Contents 1");
            _writer.WriteAttributeString("style:family", "paragraph");
            _writer.WriteAttributeString("style:class", "index");
            _writer.WriteStartElement("style:paragraph-properties");//style:text-properties
            _writer.WriteAttributeString("fo:margin", "100%");
            _writer.WriteAttributeString("fo:margin-left", "0in");
            _writer.WriteAttributeString("fo:text-indent", "0in");
            _writer.WriteAttributeString("fo:margin-right", "0in");
            _writer.WriteAttributeString("style:auto-text-indent", "false");
            _writer.WriteStartElement("style:tab-stops");
            _writer.WriteStartElement("style:tab-stop");
            _writer.WriteAttributeString("style:position", "4.8028in");
            _writer.WriteAttributeString("style:type", "right");
            _writer.WriteAttributeString("style:leader-style", "dotted");
            _writer.WriteAttributeString("style:leader-text", ".");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("style:text-properties"); //style:text-properties
            _writer.WriteAttributeString("style:font-name", _projInfo.HeaderFontName);
            _writer.WriteAttributeString("fo:language", "zxx");
            _writer.WriteAttributeString("fo:country", "none");
            _writer.WriteAttributeString("style:font-name-complex", _projInfo.HeaderFontName);
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            _writer.WriteStartElement("style:style");
            _writer.WriteAttributeString("style:name", "Graphics");
            _writer.WriteAttributeString("style:family", "graphic");
            _writer.WriteStartElement("style:graphic-properties");
            _writer.WriteAttributeString("text:anchor-type", "paragraph");
            _writer.WriteAttributeString("svg:x", "0in");
            _writer.WriteAttributeString("svg:y", "0in");
            if (!isMirrored)
            {
                _writer.WriteAttributeString("style:mirror", "none");
            }
            _writer.WriteAttributeString("fo:clip", "rect(0in 0in 0in 0in)");
            _writer.WriteAttributeString("draw:luminance", "0%");
            _writer.WriteAttributeString("draw:contrast", "0%");
            _writer.WriteAttributeString("draw:red", "0%");
            _writer.WriteAttributeString("draw:green", "0%");
            _writer.WriteAttributeString("draw:blue", "0%");
            _writer.WriteAttributeString("draw:gamma", "100%");
            _writer.WriteAttributeString("draw:color-inversion", "false");
            _writer.WriteAttributeString("draw:image-opacity", "100%");
            _writer.WriteAttributeString("draw:color-mode", "standard");
            _writer.WriteAttributeString("style:wrap", "none");
            _writer.WriteAttributeString("style:vertical-pos", "top");
            _writer.WriteAttributeString("style:vertical-rel", "paragraph");
            _writer.WriteAttributeString("style:horizontal-pos", "center");
            _writer.WriteAttributeString("style:horizontal-rel", "paragraph");
            _writer.WriteEndElement();
            _writer.WriteEndElement();


            _writer.WriteStartElement("style:style");
            _writer.WriteAttributeString("style:name", "GraphicsI1");
            _writer.WriteAttributeString("style:family", "graphic");
            _writer.WriteAttributeString("style:parent-style-name", "Graphics1");
            _writer.WriteStartElement("style:graphic-properties");
            _writer.WriteAttributeString("style:vertical-pos", "bottom");
            _writer.WriteAttributeString("style:vertical-rel", "page-content");
            _writer.WriteAttributeString("style:horizontal-pos", "center");
            _writer.WriteAttributeString("style:horizontal-rel", "page");
             _writer.WriteEndElement();
            _writer.WriteEndElement();

            _writer.WriteStartElement("style:style");
            _writer.WriteAttributeString("style:name", "GraphicsI2");
            _writer.WriteAttributeString("style:family", "graphic");
            _writer.WriteAttributeString("style:parent-style-name", "Graphics1");
            _writer.WriteStartElement("style:graphic-properties");
            _writer.WriteAttributeString("style:vertical-pos", "from-top");
            _writer.WriteAttributeString("style:vertical-rel", "paragraph");
            _writer.WriteAttributeString("style:horizontal-pos", "center");
            _writer.WriteAttributeString("style:horizontal-rel", "paragraph");
            _writer.WriteAttributeString("style:mirror", "none");
            _writer.WriteAttributeString("fo:clip", "rect(0in 0in 0in 0in)");
            _writer.WriteAttributeString("draw:luminance", "0%");
            _writer.WriteAttributeString("draw:contrast", "0%");
            _writer.WriteAttributeString("draw:red", "0%");
            _writer.WriteAttributeString("draw:green", "0%");
            _writer.WriteAttributeString("draw:blue", "0%");
            _writer.WriteAttributeString("draw:gamma", "100%");
            _writer.WriteAttributeString("draw:color-inversion", "false");
            _writer.WriteAttributeString("draw:image-opacity", "100%");
            _writer.WriteAttributeString("draw:color-mode", "standard");
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            _writer.WriteStartElement("style:style");
            _writer.WriteAttributeString("style:name", "Frame");
            _writer.WriteAttributeString("style:family", "graphic");
            _writer.WriteAttributeString("style:parent-style-name", "Graphics");
            _writer.WriteStartElement("style:graphic-properties");
            _writer.WriteAttributeString("text:anchor-type", "paragraph");
            _writer.WriteAttributeString("style:wrap", "parallel");
            _writer.WriteAttributeString("style:number-wrapped-paragraphs", "no-limit");
            _writer.WriteAttributeString("style:wrap-contour", "false");
            _writer.WriteAttributeString("style:vertical-pos", "top");
            _writer.WriteAttributeString("style:vertical-rel", "paragraph-content");
            _writer.WriteAttributeString("style:horizontal-pos", "center");
            _writer.WriteAttributeString("style:horizontal-rel", "paragraph-content");
            _writer.WriteAttributeString("style:flow-with-text", "false");
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            //office:styles Attributes.
            //// fullString:outline-style
            _writer.WriteStartElement("text:outline-style");
            _writer.WriteStartElement("text:outline-level-style");
            _writer.WriteAttributeString("text:level", "1");
            _writer.WriteAttributeString("style:num-format", "");
            _writer.WriteStartElement("style:list-level-properties");
            _writer.WriteAttributeString("text:min-label-distance", "0.15in");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("text:outline-level-style");
            _writer.WriteAttributeString("text:level", "2");
            _writer.WriteAttributeString("style:num-format", "");
            _writer.WriteStartElement("style:list-level-properties");
            _writer.WriteAttributeString("text:min-label-distance", "0.15in");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("text:outline-level-style");
            _writer.WriteAttributeString("text:level", "3");
            _writer.WriteAttributeString("style:num-format", "");
            _writer.WriteStartElement("style:list-level-properties");
            _writer.WriteAttributeString("text:min-label-distance", "0.15in");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("text:outline-level-style");
            _writer.WriteAttributeString("text:level", "4");
            _writer.WriteAttributeString("style:num-format", "");
            _writer.WriteStartElement("style:list-level-properties");
            _writer.WriteAttributeString("text:min-label-distance", "0.15in");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("text:outline-level-style");
            _writer.WriteAttributeString("text:level", "5");
            _writer.WriteAttributeString("style:num-format", "");
            _writer.WriteStartElement("style:list-level-properties");
            _writer.WriteAttributeString("text:min-label-distance", "0.15in");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("text:outline-level-style");
            _writer.WriteAttributeString("text:level", "6");
            _writer.WriteAttributeString("style:num-format", "");
            _writer.WriteStartElement("style:list-level-properties");
            _writer.WriteAttributeString("text:min-label-distance", "0.15in");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("text:outline-level-style");
            _writer.WriteAttributeString("text:level", "7");
            _writer.WriteAttributeString("style:num-format", "");
            _writer.WriteStartElement("style:list-level-properties");
            _writer.WriteAttributeString("text:min-label-distance", "0.15in");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("text:outline-level-style");
            _writer.WriteAttributeString("text:level", "8");
            _writer.WriteAttributeString("style:num-format", "");
            _writer.WriteStartElement("style:list-level-properties");
            _writer.WriteAttributeString("text:min-label-distance", "0.15in");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("text:outline-level-style");
            _writer.WriteAttributeString("text:level", "9");
            _writer.WriteAttributeString("style:num-format", "");
            _writer.WriteStartElement("style:list-level-properties");
            _writer.WriteAttributeString("text:min-label-distance", "0.15in");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("text:outline-level-style");
            _writer.WriteAttributeString("text:level", "10");
            _writer.WriteAttributeString("style:num-format", "");
            _writer.WriteStartElement("style:list-level-properties");
            _writer.WriteAttributeString("text:min-label-distance", "0.15in");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            foreach (string keyValue in _cssProperty.Keys)
            {
                if(keyValue.IndexOf("..footnote-marker") > 0)
                {
                    string className = Common.LeftString(keyValue, "..");
                    string callclassName = Common.LeftString(keyValue, "..") + "..footnote-call";
                    string markerclassName = Common.LeftString(keyValue, "..")+ "..footnote-marker";
                    _writer.WriteStartElement("text:notes-configuration");
                    _writer.WriteAttributeString("text:note-class", className);
                    _writer.WriteAttributeString("text:citation-style-name", callclassName);
                    _writer.WriteAttributeString("text:citation-body-style-name", callclassName);
                    _writer.WriteAttributeString("style:num-format", "1");
                    _writer.WriteAttributeString("text:start-value", "0");
                    _writer.WriteAttributeString("text:footnotes-position", "page");
                    _writer.WriteAttributeString("text:start-numbering-at", "document");
                    _writer.WriteEndElement();
                }
            }

            //office:styles Attributes.
            //// fullString:notes-configuration endnote
            _writer.WriteStartElement("text:notes-configuration");
            _writer.WriteAttributeString("text:note-class", "endnote");
            _writer.WriteAttributeString("style:num-format", "i");
            _writer.WriteAttributeString("text:start-value", "0");
            _writer.WriteEndElement();

            //office:styles Attributes.
            //// fullString:linenumbering-configuration
            _writer.WriteStartElement("text:linenumbering-configuration");
            _writer.WriteAttributeString("text:number-lines", "false");
            _writer.WriteAttributeString("text:offset", "0.1965in");
            _writer.WriteAttributeString("style:num-format", "1");
            _writer.WriteAttributeString("text:number-position", "left");
            _writer.WriteAttributeString("text:increment", "5");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        private void CreateDefaultStyles()
        {
            // "graphic"
            _writer.WriteStartElement("style:default-style");
            _writer.WriteAttributeString("style:family", "graphic");
            _writer.WriteStartElement("style:graphic-properties");
            _writer.WriteAttributeString("draw:shadow-offset-x", "0.1181in");
            _writer.WriteAttributeString("draw:shadow-offset-y", "0.1181in");
            _writer.WriteAttributeString("draw:start-line-spacing-horizontal", "0.1114in");
            _writer.WriteAttributeString("draw:start-line-spacing-vertical", "0.1114in");
            _writer.WriteAttributeString("draw:end-line-spacing-horizontal", "0.1114in");
            _writer.WriteAttributeString("draw:end-line-spacing-vertical", "0.1114in");
            _writer.WriteAttributeString("style:flow-with-text", "true");
            _writer.WriteEndElement();

            _writer.WriteStartElement("style:paragraph-properties");
            _writer.WriteAttributeString("style:text-autospace", "ideograph-alpha");
            _writer.WriteAttributeString("style:line-break", "strict");
            _writer.WriteAttributeString("style:writing-mode", "lr-tb");
            _writer.WriteAttributeString("style:font-independent-line-spacing", "false");

            _writer.WriteStartElement("style:tab-stops");
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            _writer.WriteStartElement("style:text-properties");
            _writer.WriteAttributeString("style:use-window-font-color", "true");
            _writer.WriteAttributeString("fo:font-size", "12pt");
            _writer.WriteAttributeString("fo:language", "none");
            _writer.WriteAttributeString("fo:country", "none");
            _writer.WriteAttributeString("style:letter-kerning", _enableKerning);
            _writer.WriteAttributeString("style:font-size-asian", "12pt");
            _writer.WriteAttributeString("style:language-asian", "zxx");
            _writer.WriteAttributeString("style:country-asian", "none");
            _writer.WriteAttributeString("style:font-size-complex", "12pt");
            _writer.WriteAttributeString("style:language-complex", "zxx");
            _writer.WriteAttributeString("style:country-complex", "none");
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            //office:styles Attributes.
            //// "paragraph"
            _writer.WriteStartElement("style:default-style");
            _writer.WriteAttributeString("style:family", "paragraph");
            _writer.WriteStartElement("style:paragraph-properties");
            _writer.WriteAttributeString("fo:hyphenation-ladder-count", "no-limit");
            _writer.WriteAttributeString("style:text-autospace", "ideograph-alpha");
            _writer.WriteAttributeString("style:punctuation-wrap", "hanging");
            _writer.WriteAttributeString("style:line-break", "strict");
            _writer.WriteAttributeString("style:tab-stop-distance", "0.4925in");
            _writer.WriteAttributeString("style:writing-mode", "page");
            _writer.WriteEndElement();

            _writer.WriteStartElement("style:text-properties");
            _writer.WriteAttributeString("style:use-window-font-color", "true");
            _writer.WriteAttributeString("style:font-name", _projInfo.DefaultFontName);
            setFooterDefaultFont();//for TD-2889

            _writer.WriteAttributeString("fo:language", "none");
            _writer.WriteAttributeString("fo:country", "none");
            _writer.WriteAttributeString("style:letter-kerning", _enableKerning);
            _writer.WriteAttributeString("style:font-name-asian", "Yi plus Phonetics");
            _writer.WriteAttributeString("style:font-size-asian", "12pt");
            _writer.WriteAttributeString("style:language-asian", "none");
            _writer.WriteAttributeString("style:country-asian", "none");
            _writer.WriteAttributeString("style:font-name-complex", "Tahoma");
            _writer.WriteAttributeString("style:font-size-complex", "12pt");
            _writer.WriteAttributeString("style:language-complex", "zxx");
            _writer.WriteAttributeString("style:country-complex", "none");
            _writer.WriteAttributeString("fo:hyphenate", "false");
            _writer.WriteAttributeString("fo:hyphenation-remain-char-count", "2");
            _writer.WriteAttributeString("fo:hyphenation-push-char-count", "2");
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            //office:styles Attributes.
            //// "table"
            _writer.WriteStartElement("style:default-style");
            _writer.WriteAttributeString("style:family", "table");
            _writer.WriteStartElement("style:table-properties");
            _writer.WriteAttributeString("table:border-model", "collapsing");
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            //office:styles Attributes.
            //// "table-row"
            _writer.WriteStartElement("style:default-style");
            _writer.WriteAttributeString("style:family", "table-row");
            _writer.WriteStartElement("style:table-row-properties");
            _writer.WriteAttributeString("fo:keep-together", "auto");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        //This method set Default font for Footer (TD-2889)
        private void setFooterDefaultFont()
        {
            string _defaultFontSize = _projInfo.DefaultFontSize + "pt";
            try
            {
                if (_projInfo.ProjectInputType == "Dictionary")
                {
                    if (_cssProperty.ContainsKey("entry") && _cssProperty["entry"].ContainsKey("font-size"))
                    {
                        _defaultFontSize = _cssProperty["entry"]["font-size"] + "pt";
                    }
                }
                else
                {
                    if (_cssProperty.ContainsKey("Paragraph") && _cssProperty["Paragraph"].ContainsKey("font-size"))
                    {
                        _defaultFontSize = _cssProperty["Paragraph"]["font-size"] + "pt";
                    }
                }
                _writer.WriteAttributeString("fo:font-size", _defaultFontSize);
            }
            catch
            {
                _writer.WriteAttributeString("fo:font-size", _defaultFontSize);
            }
        }

        private void InitializePageHeadFootDictionary()
        {
            //All PageProperty information is assinged to First PageProperty, when First PageProperty is not found
            for (int i = 0; i <= 5; i++)
            {
                if (_firstPageContentNone.Contains(i))
                {
                    continue; // no need of copy. for content : normal or none;
                }

                if (_pageHeaderFooter[i].Count == 0) // Only copy @page values when equivalent element is empty in @PageProperty:first
                {
                    _pageHeaderFooter[i] = _pageHeaderFooter[i + 6];
                }
            }

            _firstPageLayoutProperty = MergePageProperty(_firstPageLayoutProperty, true, "first");
            _firstPageLayoutProperty = MergePageProperty(_firstPageLayoutProperty, false, "first");
            _leftPageLayoutProperty = MergePageProperty(_leftPageLayoutProperty, false, "left");
            _rightPageLayoutProperty = MergePageProperty(_rightPageLayoutProperty, false, "right");

        }

        private Dictionary<string, string> MergePageProperty(Dictionary<string, string> layoutProperty, bool fromRight, string pageName)
        {
            int headerFooterIndex = 0;
            if (pageName.IndexOf("left") == 0)
            {
                headerFooterIndex = 12;
            }
            else if (pageName.IndexOf("right") == 0)
            {
                headerFooterIndex = 18;
            }

            if (fromRight)
            {
                if (layoutProperty.Count == 0)
                {
                    layoutProperty = _rightPageLayoutProperty;
                }
                else
                {
                    foreach (KeyValuePair<string, string> property in _rightPageLayoutProperty)
                    {
                        if (!layoutProperty.ContainsKey(property.Key))
                        {
                            layoutProperty[property.Key] = property.Value;
                        }
                    }
                }
                for (int i = 0; i <= 5; i++)
                {
                    if (_pageHeaderFooter[i].Count == 0)
                    {
                        _pageHeaderFooter[i] = _pageHeaderFooter[i + 18];
                    }
                    else
                    {
                        foreach (KeyValuePair<string, string> property in _pageHeaderFooter[i + 18])
                        {
                            if (!_pageHeaderFooter[i].ContainsKey(property.Key))
                            {
                                _pageHeaderFooter[i][property.Key] = property.Value;
                            }
                        }
                    }

                }

            }
            else
            {
                if (layoutProperty.Count == 0)
                {
                    layoutProperty = _pageLayoutProperty;
                }
                else
                {
                    foreach (KeyValuePair<string, string> property in _pageLayoutProperty)
                    {
                        if (!layoutProperty.ContainsKey(property.Key))
                        {
                            layoutProperty[property.Key] = property.Value;
                        }
                    }
                }




                for (int i = 0; i <= 5; i++)
                {
                    if (_pageHeaderFooter[i + headerFooterIndex].Count == 0)
                    {
                        _pageHeaderFooter[i + headerFooterIndex] = _pageHeaderFooter[i + 6];
                    }
                    else
                    {
                        foreach (KeyValuePair<string, string> property in _pageHeaderFooter[i + 6])
                        {
                            if (!_pageHeaderFooter[i + headerFooterIndex].ContainsKey(property.Key))
                            {
                                _pageHeaderFooter[i + headerFooterIndex][property.Key] = property.Value;
                            }
                        }
                    }

                }


            }
            return layoutProperty;
        }
        private void CreateHeaderFooterVariables(int PageIndex)
        {
            bool isHeaderCreated = false;
            bool isFooterCreated = false;
            bool isHeaderClosed = false;
            bool isFooterClosed = false;
            bool isPageNumber = false;
            bool isRightGuideword = false;
            for (int i = PageIndex; i <= PageIndex + 5; i++)
            {
                if(_pageHeaderFooter[i].ContainsKey("content"))
                {
                    if (_pageHeaderFooter[i]["content"].IndexOf("first)") > 0) 
                    {
                        if (!isHeaderCreated)
                        {
                            CreateHeaderFooter(i, "first", ref isHeaderCreated, ref isFooterCreated, ref isHeaderClosed, ref isFooterClosed);
                        }
                        CreateLeftGuidewordPageNumber(i,"LeftGuideword");
                    }
                    else if (_pageHeaderFooter[i]["content"].IndexOf("last)") > 0)
                    {
                        if (!isFooterCreated)
                        {
                            CreateHeaderFooter(i, "last", ref isHeaderCreated, ref isFooterCreated, ref isHeaderClosed, ref isFooterClosed);
                        }
                        CreateRightGuideword(i);
                        isRightGuideword = true;
                    }
                    else if (_pageHeaderFooter[i]["content"].IndexOf("counter") > 0)
                    {
                        CreateHeaderFooter(i, "PageNumber", ref isHeaderCreated, ref isFooterCreated, ref isHeaderClosed, ref isFooterClosed);
                        CreateLeftGuidewordPageNumber(i, "PageNumber");
                        if (i < 21) //PageNumber Created on RightPage Header
                        {
                            isPageNumber = true;
                        }
                    }
                }
            }
            //Close Header/Footer
            if (isHeaderCreated && isHeaderClosed == false)
                HeaderFooterEndElement();
            if (isFooterCreated && isFooterClosed == false)
                HeaderFooterEndElement();

            if(!isPageNumber && isRightGuideword && isMirrored)
            {
                CreateEmptyHeader(true);
            }
        }

        private void HeaderFooterEndElement()
        {
            _writer.WriteEndElement();//text:p
            _writer.WriteEndElement();//style:
        }
        private void CreateHeaderFooter(int i,string variableType,ref bool isHeaderCreated,
                ref bool isFooterCreated, ref bool isHeaderClosed, ref bool isFooterClosed)
        {
            string type;
            if(variableType == "first")
            {
                type = "Header";
                isHeaderCreated = true;
            }   
            else if(variableType == "last")
            {
                type = "Footer";
                isFooterCreated = true;
            }
            else
            {
                switch (i)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 6:
                    case 7:
                    case 8:
                    case 12:
                    case 13:
                    case 14:
                    case 18:
                    case 19:
                    case 20:
                        if (isHeaderCreated) return;
                        type = "Header";
                        isHeaderCreated = true;
                        break;
                    default:
                        if (isFooterCreated) return;
                        type = "Footer";
                        isFooterCreated = true;
                        break; 
                }
            }

            if (type == "Footer" && isHeaderCreated)
            {
                HeaderFooterEndElement();
                isHeaderClosed = true;
            }

            if (type == "Header" && isFooterCreated)
            {
                HeaderFooterEndElement();
                isFooterClosed = true;
            }
            _writer.WriteStartElement("style:" + type.ToLower());
            _writer.WriteStartElement("text:p");
            _writer.WriteAttributeString("text:style-name", type);
        }

        
        private void SetTab()
        {
            _writer.WriteStartElement("text:tab");
            _writer.WriteEndElement();
        }

        private void SetPageNumber()
        {
            _writer.WriteStartElement("text:page-number");
            _writer.WriteAttributeString("text:select-page", "current");
            _writer.WriteString("4");
            _writer.WriteEndElement();
        }

        private void CreateRightGuideword(int i)
        {
            string mainFrameName = "Mfr1";
            
            _writer.WriteStartElement("draw:frame");
            _writer.WriteAttributeString("draw:style-name", mainFrameName);
            _writer.WriteAttributeString("draw:name", "Frame1");
            _writer.WriteAttributeString("text:anchor-type", "paragraph");


            string value = "0pt";
            if (!String.IsNullOrEmpty(_pageLayoutProperty["fo:margin-top"]))
            {
                value = _pageLayoutProperty["fo:margin-top"];
                Array arValue = value.Split('p');
                value = Convert.ToDouble(arValue.GetValue(0)) - 0.75 + "pt";
            }
            _writer.WriteAttributeString("svg:y", value);   

            _writer.WriteAttributeString("fo:min-width", "145pt");
            _writer.WriteAttributeString("draw:z-index", "1");
            _writer.WriteStartElement("draw:text-box");
            _writer.WriteAttributeString("fo:min-height", "14.14pt");//added for TD-2579
            _writer.WriteStartElement("text:p");
            _writer.WriteAttributeString("text:style-name", "MP1");

            //TD-2566
            _writer.WriteStartElement("text:span");
            _writer.WriteAttributeString("text:style-name", "MT1");

            _writer.WriteStartElement("text:variable-get");
            _writer.WriteAttributeString("text:name", "Right_Guideword_R");
            _writer.WriteAttributeString("office:value-type", "string");

            _writer.WriteEndElement();

            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        private void CreateEmptyHeader(bool isLeftGuidewordNeeded)
        {
            string hf = "Header";
            _writer.WriteStartElement("style:" + hf.ToLower());
            _writer.WriteStartElement("text:p");
            _writer.WriteAttributeString("text:style-name", hf);

            _writer.WriteStartElement("text:span");
            _writer.WriteAttributeString("text:style-name", "MT1");
            //Left should come
            if (isLeftGuidewordNeeded)
            {
                CreateLeftGuideword(20);
            }
            _writer.WriteEndElement();//text:span

            _writer.WriteEndElement();//text:p
            _writer.WriteEndElement();//style:
        }

        private void CreateLeftGuideword(int i)
        {
            switch (i)
            {
                case 1:
                case 7:
                case 13:
                case 19:
                    SetTab();
                    break;
                case 2:
                case 8:
                case 14:
                case 20:
                    SetTab();
                    if (_isCenterTabStopNeeded)
                    {
                        SetTab();
                    }
                    break;
            }
            _writer.WriteStartElement("text:span");
            _writer.WriteAttributeString("text:style-name", "MT1");
            _writer.WriteStartElement("text:variable-get");
            _writer.WriteAttributeString("text:name", "Left_Guideword_L");
            _writer.WriteAttributeString("office:value-type", "string");
            _writer.WriteEndElement(); //text:variable-get
            _writer.WriteEndElement(); //text:span
        }

        private void CreateLeftGuidewordPageNumber(int i, string type)
        {
            if (type == "LeftGuideword")
            {
                CreateLeftGuideword(i);
            }
            else
            {
                switch (i)
                {
                    case 1:
                    case 4:
                    case 7:
                    case 10:
                    case 13:
                    case 16:
                    case 19:
                    case 22:
                        SetTab();
                        break;
                    case 2:
                    case 5:
                    case 8:
                      case 11:
                    case 14:
                    case 17:
                    case 20:
                    case 23:
                        if (_guidewordLength > 0 && _guidewordLength < 99 && i == 14) break;
                        SetTab();
                        if (_isCenterTabStopNeeded)
                        {
                            SetTab();
                        }
                        break;
                }
                if (i == 21 || i == 22 || i == 23)
                    CreateEmptyHeader(false);
                if (_guidewordLength > 0 && _guidewordLength < 99 && i == 14)
                    SetPageNumberInFrame();
                else
                    SetPageNumber();
                //TD-2825 This will work for right page alone
                if (i == 18)                    //if page no is topLeft, leftguideword will be created topRight with 2 Tabs
                    CreateLeftGuideword(20);
                else if (i == 19)               //if page no is topCenter, leftguideword will be created topRight with 1 Tab
                    CreateLeftGuideword(19);
            }
        }

        private void SetPageNumberInFrame()
        {




            string mainFrameName = "Mfr1";

            _writer.WriteStartElement("draw:frame");
            _writer.WriteAttributeString("draw:style-name", mainFrameName);
            _writer.WriteAttributeString("draw:name", "Frame1");
            _writer.WriteAttributeString("text:anchor-type", "paragraph");


            string value = "0pt";
            if (!String.IsNullOrEmpty(_pageLayoutProperty["fo:margin-top"]))
            {
                value = _pageLayoutProperty["fo:margin-top"];
                Array arValue = value.Split('p');
                value = Convert.ToDouble(arValue.GetValue(0)) - 0.75 + "pt";
            }
            _writer.WriteAttributeString("svg:y", value);

            _writer.WriteAttributeString("fo:min-width", "35pt");
            _writer.WriteAttributeString("draw:z-index", "1");
            _writer.WriteStartElement("draw:text-box");
            _writer.WriteAttributeString("fo:min-height", "14.14pt");//added for TD-2579
            _writer.WriteStartElement("text:p");
            _writer.WriteAttributeString("text:style-name", "MP1");

            //TD-2566
            _writer.WriteStartElement("text:span");
            _writer.WriteAttributeString("text:style-name", "MT1");
            _writer.WriteStartElement("text:page-number");
            _writer.WriteAttributeString("text:select-page", "current");
            _writer.WriteString("4");
            _writer.WriteEndElement();

            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        private void CreateHeaderFooterStyle(string hf, int index)
        {
            if (ValidPageHeaderFooter(index) == false) return;
            //TD-2448
            _writer.WriteStartElement("style:" + hf.ToLower());
            _writer.WriteStartElement("text:p");
            _writer.WriteAttributeString("text:style-name", hf);
            if (hf.ToLower() == "header")
            {
                    int maxCount = index + 3;
                    for (int i = index; i < maxCount; i++)
                    {
                        if (_pageHeaderFooter[i].Count > 0 && _pageHeaderFooter[i]["content"].IndexOf("first)") > 0)
                        {
                            //Left Side - Left Guideword
                            //TD-2566
                            _writer.WriteStartElement("text:span");
                            _writer.WriteAttributeString("text:style-name", "MT1");

                            _writer.WriteStartElement("text:variable-get");
                            _writer.WriteAttributeString("text:name", "Left_Guideword_L");
                            _writer.WriteAttributeString("office:value-type", "string");
                            _writer.WriteEndElement();

                            _writer.WriteEndElement();


                            if (_multiLanguageHeader)
                            {
                                //Left Side - Right Guideword
                                _writer.WriteStartElement("draw:frame");
                                _writer.WriteAttributeString("draw:style-name", "Mfr1");
                                _writer.WriteAttributeString("draw:name", "Frame1");
                                _writer.WriteAttributeString("text:anchor-type", "paragraph");
                                string value = "56.89pt";
                                if (isMirrored && !String.IsNullOrEmpty(_leftPageLayoutProperty["fo:margin-top"]))
                                {
                                    value = _leftPageLayoutProperty["fo:margin-top"];
                                }
                                else if (!String.IsNullOrEmpty(_pageLayoutProperty["fo:margin-top"]))
                                {
                                    value = _pageLayoutProperty["fo:margin-top"];
                                }
                                _writer.WriteAttributeString("svg:y", value);
                                _writer.WriteAttributeString("fo:min-width", "35pt");
                                _writer.WriteAttributeString("svg:height", "14.14pt");
                                _writer.WriteAttributeString("draw:z-index", "1");
                                _writer.WriteStartElement("draw:text-box");
                                _writer.WriteStartElement("text:p");
                                _writer.WriteAttributeString("text:style-name", "MP1");
                                _writer.WriteStartElement("text:variable-get");
                                _writer.WriteAttributeString("text:name", "Left_Guideword_R");
                                _writer.WriteAttributeString("office:value-type", "string");
                                _writer.WriteEndElement();
                                _writer.WriteEndElement();
                                _writer.WriteEndElement();
                                _writer.WriteEndElement();
                            }
                        }
                        else if (_pageHeaderFooter[i].Count > 0 && _pageHeaderFooter[i]["content"].IndexOf("last)") > 0)
                        {
                            //Right Side - Right Guideword
                            string mainFrameName = "Mfr1";
                            string value = "56.89pt";
                            _writer.WriteStartElement("draw:frame");
                            _writer.WriteAttributeString("draw:style-name", mainFrameName);
                            _writer.WriteAttributeString("draw:name", "Frame1");
                            _writer.WriteAttributeString("text:anchor-type", "paragraph");
                            if (i == 18 || i == 19)
                            {
                                _writer.WriteAttributeString("svg:x", "0pt");
                            }

                            //TD-2631 for adjust the Frame Top
                            if (value.IndexOf("pt") > 0)
                            {
                                value = value.Replace("pt", "");
                                double top = Convert.ToDouble(value, CultureInfo.GetCultureInfo("en-US")) - 0.70;
                                value = top.ToString() + "pt";
                            }

                            _writer.WriteAttributeString("svg:y", value);
                            _writer.WriteAttributeString("fo:min-width", "135pt");
                            _writer.WriteAttributeString("draw:z-index", "1");
                            _writer.WriteStartElement("draw:text-box");
                            _writer.WriteAttributeString("fo:min-height", "14.14pt");//added for TD-2579
                            _writer.WriteStartElement("text:p");
                            _writer.WriteAttributeString("text:style-name", "MP1");

                            //TD-2566
                            _writer.WriteStartElement("text:span");
                            _writer.WriteAttributeString("text:style-name", "MT1");

                            _writer.WriteStartElement("text:variable-get");
                            _writer.WriteAttributeString("text:name", "Right_Guideword_R");
                            _writer.WriteAttributeString("office:value-type", "string");

                            _writer.WriteEndElement();

                            _writer.WriteEndElement();
                            _writer.WriteEndElement();
                            _writer.WriteEndElement();
                        }
                        else if (_pageHeaderFooter[i].Count > 0 && (_pageHeaderFooter[i]["content"].IndexOf("page") > 0 &&
                                  _pageHeaderFooter[i]["content"].IndexOf("counter") >= 0))
                        {
                            _writer.WriteStartElement("text:page-number");
                            _writer.WriteAttributeString("text:select-page", "current");
                            _writer.WriteString("4");
                            _writer.WriteEndElement();
                        }
                        if (i != maxCount-1)
                        {
                            _writer.WriteStartElement("text:tab");
                            _writer.WriteEndElement();
                        }
                    }
            }//Footer
            else
            {
                int maxCount = index + 3;
                bool pageNumberWritten = false;
                int footerFieldCount = _pageHeaderFooter[3].Count + _pageHeaderFooter[4].Count +
                                       _pageHeaderFooter[5].Count + _pageHeaderFooter[9].Count +
                                       _pageHeaderFooter[10].Count + _pageHeaderFooter[11].Count +
                                       _pageHeaderFooter[15].Count + _pageHeaderFooter[16].Count +
                                       _pageHeaderFooter[17].Count + _pageHeaderFooter[21].Count +
                                       _pageHeaderFooter[22].Count + _pageHeaderFooter[23].Count;
                for (int i = index; i < maxCount; i++)
                
                {
                    if (i != maxCount - 1)
                    {
                    }
                    if (_pageHeaderFooter[i].Count > 0 && _pageHeaderFooter[i].ContainsKey("content") &&
                        _pageHeaderFooter[i]["content"].IndexOf("last)") > 0)
                    {
                        
                        _writer.WriteStartElement("text:tab");
                        _writer.WriteEndElement();

                        string mainFrameName = "Mfr1";
                        string value = "56.89pt";
                        if (_multiLanguageHeader || (i == 18))
                        {
                            mainFrameName = "Mfr2";
                            value = "0pt";
                        }
                        else if(i == 19)
                        {
                            mainFrameName = "Mfr3";
                            value = "0pt";
                        }
                        if (isMirrored && !String.IsNullOrEmpty(_rightPageLayoutProperty["fo:margin-top"]))
                        {
                            value = _rightPageLayoutProperty["fo:margin-top"];
                        }
                        else if (!String.IsNullOrEmpty(_pageLayoutProperty["fo:margin-top"]))
                        {
                            value = _pageLayoutProperty["fo:margin-top"];
                        }
                        _writer.WriteStartElement("draw:frame");
                        _writer.WriteAttributeString("draw:style-name", mainFrameName);
                        _writer.WriteAttributeString("draw:name", "Frame1");
                        _writer.WriteAttributeString("text:anchor-type", "paragraph");
                        if(i == 18 || i == 19)
                        {
                            _writer.WriteAttributeString("svg:x", "0pt");
                        }

                        //TD-2631 for adjust the Frame Top
                        if(value.IndexOf("pt") > 0)
                        {
                            value = value.Replace("pt", "");
                            double top = Convert.ToDouble(value, CultureInfo.GetCultureInfo("en-US")) - 0.70;
                            value = top.ToString() + "pt";
                        }

                        _writer.WriteAttributeString("svg:y", value);
                        _writer.WriteAttributeString("fo:min-width", "135pt");
                        _writer.WriteAttributeString("draw:z-index", "1");
                        _writer.WriteStartElement("draw:text-box");
                        _writer.WriteAttributeString("fo:min-height", "14.14pt");//added for TD-2579
                        _writer.WriteStartElement("text:p");
                        _writer.WriteAttributeString("text:style-name", "MP1");

                        //TD-2566
                        _writer.WriteStartElement("text:span");
                        _writer.WriteAttributeString("text:style-name", "MT1");

                        _writer.WriteStartElement("text:variable-get");
                        _writer.WriteAttributeString("text:name", "Right_Guideword_R");
                        _writer.WriteAttributeString("office:value-type", "string");

                        _writer.WriteEndElement();

                        _writer.WriteEndElement();
                        _writer.WriteEndElement();
                        _writer.WriteEndElement();


                    }
                    else if (footerFieldCount > 0)
                    {
                        if (!pageNumberWritten && _pageHeaderFooter[i].Count > 0 &&
                            _pageHeaderFooter[i].ContainsKey("content") &&
                            (_pageHeaderFooter[i]["content"].IndexOf("page") > 0 &&
                             _pageHeaderFooter[i]["content"].IndexOf("counter") >= 0))
                        {   
                            //left 
                            _writer.WriteStartElement("text:page-number");
                            _writer.WriteAttributeString("text:select-page", "current");
                            _writer.WriteString("4");
                            _writer.WriteEndElement();
                            pageNumberWritten = true;
                        }
                        else if (!pageNumberWritten && _pageHeaderFooter[i + 1].Count > 0 &&
                                 _pageHeaderFooter[i + 1].ContainsKey("content") &&
                                 (_pageHeaderFooter[i + 1]["content"].IndexOf("page") > 0 &&
                                  _pageHeaderFooter[i + 1]["content"].IndexOf("counter") >= 0))
                        {   
                            //center
                            _writer.WriteStartElement("text:tab");
                            _writer.WriteEndElement();

                            _writer.WriteStartElement("text:page-number");
                            _writer.WriteAttributeString("text:select-page", "current");
                            _writer.WriteString("4");
                            _writer.WriteEndElement();
                            pageNumberWritten = true;
                        }
                        else if (!pageNumberWritten && _pageHeaderFooter[i + 2].Count > 0 &&
                                 _pageHeaderFooter[i + 2].ContainsKey("content") &&
                                 (_pageHeaderFooter[i + 2]["content"].IndexOf("page") > 0 &&
                                  _pageHeaderFooter[i + 2]["content"].IndexOf("counter") >= 0))
                        {   
                            //right
                            _writer.WriteStartElement("text:tab");
                            _writer.WriteEndElement();
                            _writer.WriteStartElement("text:tab");
                            _writer.WriteEndElement();

                            _writer.WriteStartElement("text:page-number");
                            _writer.WriteAttributeString("text:select-page", "current");
                            _writer.WriteString("4");
                            _writer.WriteEndElement();
                            pageNumberWritten = true;
                        }
                    }
                }
            }
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        private bool ValidPageHeaderFooter(int currIndex)
        {
            return (_pageHeaderFooter[currIndex].ContainsKey("content") && _pageHeaderFooter[currIndex]["content"].Trim().Length > 0)
                   || (_pageHeaderFooter[currIndex + 1].ContainsKey("content") && _pageHeaderFooter[currIndex + 1]["content"].Trim().Length > 0)
                   || (_pageHeaderFooter[currIndex + 2].ContainsKey("content") && _pageHeaderFooter[currIndex + 2]["content"].Trim().Length > 0);
        }

        private void CreateHeaderFooterStyle_OLD(string hf, int index)
        {
            //TD-2448
            _writer.WriteStartElement("style:" + hf.ToLower());
            _writer.WriteStartElement("text:p");
            _writer.WriteAttributeString("text:style-name", hf);
            if (hf.ToLower() == "header")
            {
                if (_pageHeaderFooter[index].ContainsKey("content"))
                {
                    if (_pageHeaderFooter[index]["content"].IndexOf("first)") > 0)
                    {
                        //Left Side - Left Guideword
                        _writer.WriteStartElement("text:variable-get");
                        _writer.WriteAttributeString("text:name", "Left_Guideword_L");
                        _writer.WriteAttributeString("office:value-type", "string");
                        _writer.WriteEndElement();

                        if (_multiLanguageHeader)
                        {
                            //Left Side - Right Guideword
                            _writer.WriteStartElement("draw:frame");
                            _writer.WriteAttributeString("draw:style-name", "Mfr1");
                            _writer.WriteAttributeString("draw:name", "Frame1");
                            _writer.WriteAttributeString("text:anchor-type", "paragraph");
                            string value = "56.89pt";
                            if (!String.IsNullOrEmpty(_pageLayoutProperty["fo:margin-top"]))
                            {
                                value = _pageLayoutProperty["fo:margin-top"];
                            }
                            _writer.WriteAttributeString("svg:y", value);
                            _writer.WriteAttributeString("fo:min-width", "35pt");
                            _writer.WriteAttributeString("svg:height", "14.14pt");
                            _writer.WriteAttributeString("draw:z-index", "1");
                            _writer.WriteStartElement("draw:text-box");
                            _writer.WriteStartElement("text:p");
                            _writer.WriteAttributeString("text:style-name", "MP1");

                            _writer.WriteStartElement("text:variable-get");
                            _writer.WriteAttributeString("text:name", "Left_Guideword_R");
                            _writer.WriteAttributeString("office:value-type", "string");
                            _writer.WriteEndElement();
                            _writer.WriteEndElement();
                            _writer.WriteEndElement();
                            _writer.WriteEndElement();
                        }
                    }
                }
            }
            else
            {
                if (_pageHeaderFooter[index - 1].ContainsKey("content"))
                {
                    if (_pageHeaderFooter[index - 1]["content"].IndexOf("last)") > 0)
                    {
                        if (_multiLanguageHeader)
                        {
                            //Right Side - Left guideword
                            _writer.WriteStartElement("draw:frame");
                            _writer.WriteAttributeString("draw:style-name", "Mfr2");
                            _writer.WriteAttributeString("draw:name", "Frame1");
                            _writer.WriteAttributeString("text:anchor-type", "paragraph");
                            string value = "0pt";
                            if (!String.IsNullOrEmpty(_pageLayoutProperty["fo:margin-top"]))
                            {
                                value = _pageLayoutProperty["fo:margin-top"];
                            }
                            _writer.WriteAttributeString("svg:x", "0pt");
                            _writer.WriteAttributeString("svg:y", value);
                            _writer.WriteAttributeString("fo:min-width", "35pt");
                            _writer.WriteAttributeString("svg:height", "14.14pt");
                            _writer.WriteAttributeString("draw:z-index", "1");
                            _writer.WriteStartElement("draw:text-box");
                            _writer.WriteStartElement("text:p");
                            _writer.WriteAttributeString("text:style-name", "MP1");
                            _writer.WriteStartElement("text:variable-get");
                            _writer.WriteAttributeString("text:name", "Right_Guideword_L");
                            _writer.WriteAttributeString("office:value-type", "string");
                            _writer.WriteEndElement();
                            _writer.WriteEndElement();
                            _writer.WriteEndElement();
                            _writer.WriteEndElement();

                        }
                        //Right Side - Right Guideword
                        _writer.WriteStartElement("draw:frame");
                        _writer.WriteAttributeString("draw:style-name", "Mfr1");
                        _writer.WriteAttributeString("draw:name", "Frame1");
                        _writer.WriteAttributeString("text:anchor-type", "paragraph");
                        string value1 = "56.89pt";
                        if (!String.IsNullOrEmpty(_pageLayoutProperty["fo:margin-top"]))
                        {
                            value1 = _pageLayoutProperty["fo:margin-top"];
                        }
                        _writer.WriteAttributeString("svg:y", value1);
                        _writer.WriteAttributeString("fo:min-width", "35pt");
                        _writer.WriteAttributeString("svg:height", "14.14pt");
                        _writer.WriteAttributeString("draw:z-index", "1");
                        _writer.WriteStartElement("draw:text-box");
                        _writer.WriteStartElement("text:p");
                        _writer.WriteAttributeString("text:style-name", "MP1");

                        //TD-2566
                        _writer.WriteStartElement("text:span");
                        _writer.WriteAttributeString("text:style-name", "MT1");

                        _writer.WriteStartElement("text:variable-get");
                        _writer.WriteAttributeString("text:name", "Right_Guideword_R");
                        _writer.WriteAttributeString("office:value-type", "string");

                        _writer.WriteEndElement();

                        _writer.WriteEndElement();
                        _writer.WriteEndElement();
                        _writer.WriteEndElement();
                        _writer.WriteEndElement();
                    }
                }
            }
            _writer.WriteStartElement("text:tab");
            _writer.WriteEndElement();
            if (_pageHeaderFooter[++index].ContainsKey("content"))
            {
                if (_pageHeaderFooter[index]["content"].IndexOf("page") > 0 &&
                    _pageHeaderFooter[index]["content"].IndexOf("counter") >= 0)
                {
                    _writer.WriteStartElement("text:page-number");
                    _writer.WriteAttributeString("text:select-page", "current");
                    _writer.WriteString("4");
                    _writer.WriteEndElement();
                }
            }
            _writer.WriteStartElement("text:tab");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }


        private void MasterPageCreation()
        {
            _writer.WriteStartElement("office:master-styles"); // pm1

            #region STANDARD CODE PART

            ///STANDARD CODE PART
            _writer.WriteStartElement("style:master-page");
            _writer.WriteAttributeString("style:name", "Standard");
            _writer.WriteAttributeString("style:page-layout-name", "pm1");
            _writer.WriteEndElement();
            ///STANDARD CODE PART ENDS 

            #endregion

            #region TITLE CODE PART

            if (_isFromExe)
            {
                //COVER CODE PART
                _writer.WriteStartElement("style:master-page");
                _writer.WriteAttributeString("style:name", "Cover_20_Page");
                _writer.WriteAttributeString("style:display-name", "Cover Page");
                _writer.WriteAttributeString("style:next-style-name", "First_20_Page");
                _writer.WriteAttributeString("style:page-layout-name", "pm1");
                _writer.WriteEndElement(); // Close of Master Page
                //COVER CODE PART ENDS

                //DUMMYPAGE CODE PART
                _writer.WriteStartElement("style:master-page");
                _writer.WriteAttributeString("style:name", "Dummy_20_Page");
                _writer.WriteAttributeString("style:display-name", "Dummy Page");
                _writer.WriteAttributeString("style:next-style-name", "First_20_Page");
                _writer.WriteAttributeString("style:page-layout-name", "pm12");
                _writer.WriteEndElement(); // Close of Master Page
                //DUMMYPAGE CODE PART ENDS


                //TITLE CODE PART
                _writer.WriteStartElement("style:master-page");
                _writer.WriteAttributeString("style:name", "Title_20_Page");
                _writer.WriteAttributeString("style:display-name", "Title Page");
                _writer.WriteAttributeString("style:next-style-name", "First_20_Page");
                _writer.WriteAttributeString("style:page-layout-name", "pm7");
                _writer.WriteEndElement(); // Close of Master Page
                //TITLE CODE PART ENDS

                //COPYRIGHT CODE PART
                _writer.WriteStartElement("style:master-page");
                _writer.WriteAttributeString("style:name", "CopyRight_20_Page");
                _writer.WriteAttributeString("style:display-name", "CopyRight Page");
                _writer.WriteAttributeString("style:next-style-name", "First_20_Page");
                _writer.WriteAttributeString("style:page-layout-name", "pm7");
                _writer.WriteEndElement(); // Close of Master Page

                //DUMMYPAGE CODE PART
                _writer.WriteStartElement("style:master-page");
                _writer.WriteAttributeString("style:name", "Dummy_20_Page");
                _writer.WriteAttributeString("style:display-name", "Dummy Page");
                _writer.WriteAttributeString("style:next-style-name", "First_20_Page");
                _writer.WriteAttributeString("style:page-layout-name", "pm12");
                _writer.WriteEndElement(); // Close of Master Page
                //DUMMYPAGE CODE PART ENDS

                //TABLEOFCONTENTS CODE PART
                _writer.WriteStartElement("style:master-page");
                _writer.WriteAttributeString("style:name", "CopyRight_20_Page");
                _writer.WriteAttributeString("style:display-name", "TableofContents Page");
                _writer.WriteAttributeString("style:next-style-name", "First_20_Page");
                _writer.WriteAttributeString("style:page-layout-name", "pm7");
                _writer.WriteStartElement("style:footer");
                _writer.WriteStartElement("text:p");
                _writer.WriteAttributeString("text:style-name", "Footer");
                _writer.WriteStartElement("text:tab");
                _writer.WriteEndElement();
                _writer.WriteStartElement("text:page-number");
                _writer.WriteAttributeString("style:num-format", "i");
                _writer.WriteAttributeString("text:select-page", "current");
                _writer.WriteString("xv");
                _writer.WriteEndElement(); //Page-number
                _writer.WriteEndElement(); //p
                _writer.WriteEndElement(); //Footer
                _writer.WriteEndElement(); // Close of Master Page
                //COPYRIGHT CODE PART ENDS

                //DUMMYPAGE CODE PART
                _writer.WriteStartElement("style:master-page");
                _writer.WriteAttributeString("style:name", "Dummy_20_Page");
                _writer.WriteAttributeString("style:display-name", "Dummy Page");
                _writer.WriteAttributeString("style:next-style-name", "First_20_Page");
                _writer.WriteAttributeString("style:page-layout-name", "pm12");
                _writer.WriteEndElement(); // Close of Master Page
                //DUMMYPAGE CODE PART ENDS
            }

            #endregion

            #region First CODE PART

            _writer.WriteStartElement("style:master-page");
            _writer.WriteAttributeString("style:name", "First_20_Page");
            _writer.WriteAttributeString("style:display-name", "First Page"); // First PageProperty
            string nextStyle = "XHTML";
            if (isMirrored)
            {
                nextStyle = "Left_20_Page";
            }
            _writer.WriteAttributeString("style:next-style-name", nextStyle);
            _writer.WriteAttributeString("style:page-layout-name", "pm3");
            CreateHeaderFooterVariables(0);
            _writer.WriteEndElement(); // Close of Master Page

            #endregion

            if (isMirrored)
            {

                #region LEFT CODE PART

                _writer.WriteStartElement("style:master-page");
                _writer.WriteAttributeString("style:name", "Left_20_Page"); // Left PageProperty
                _writer.WriteAttributeString("style:display-name", "Left Page"); // Left PageProperty
                _writer.WriteAttributeString("style:page-layout-name", "pm4");
                _writer.WriteAttributeString("style:next-style-name", "Right_20_Page");
                CreateHeaderFooterVariables(12);
                _writer.WriteEndElement(); // Close of Master Page

                #endregion

                #region RIGHT CODE PART

                _writer.WriteStartElement("style:master-page");
                _writer.WriteAttributeString("style:name", "Right_20_Page"); // Right PageProperty
                _writer.WriteAttributeString("style:display-name", "Right Page"); // Right PageProperty
                _writer.WriteAttributeString("style:page-layout-name", "pm5");
                _writer.WriteAttributeString("style:next-style-name", "Left_20_Page");
                
                CreateHeaderFooterVariables(18);
                _writer.WriteEndElement(); // Close of Master Page

                #endregion
            }
            else
            {
                #region XHTML CODE PART

                ////XHTML CODE PART START
                _writer.WriteStartElement("style:master-page");
                _writer.WriteAttributeString("style:name", "XHTML"); // All PageProperty
                _writer.WriteAttributeString("style:page-layout-name", "pm2");
                _writer.WriteAttributeString("style:next-style-name", "XHTML");
                CreateHeaderFooterVariables(6);
                _writer.WriteEndElement(); // Close of Master Page
                ////XHTML CODE PART ENDS 

                #endregion
            }
        }

        private void InsertHeaderRule()
        {
            if (!string.IsNullOrEmpty(HeaderRule))
            {
                _writer.WriteAttributeString("fo:border-bottom", HeaderRule);
            }
            else
            {
                _writer.WriteAttributeString("fo:border-bottom", "1pt solid #000000");
            }
            HeaderRule = string.Empty;
        }

        /// <summary>
        /// Function to load content of Allpage.
        /// </summary>
        private void SetAllPageHeader()
        {
            if (_pageHeaderFooter[6].Count > 0 || _pageHeaderFooter[7].Count > 0 || _pageHeaderFooter[8].Count > 0 || IsCropMarkChecked == true)
            {
                if (_pageHeaderFooter[6].ContainsKey("content"))
                {
                    _writer.WriteStartElement("text:span");
                    _writer.WriteAttributeString("text:style-name", "AllHeaderPageLeft");
                    FillHeaderFooter(_pageHeaderFooter[6]["content"], 6);
                    _writer.WriteEndElement();
                }
                if (IsCropMarkChecked)
                {
                    AddHeaderCropMarks(_pageLayoutProperty);
                }
                _writer.WriteStartElement("text:tab");
                _writer.WriteEndElement();

                if (_pageHeaderFooter[7].ContainsKey("content"))
                {
                    _writer.WriteStartElement("text:span");
                    _writer.WriteAttributeString("text:style-name", "AllHeaderPageNumber");
                    FillHeaderFooter(_pageHeaderFooter[7]["content"], 7);
                    _writer.WriteEndElement();
                }

                _writer.WriteStartElement("text:tab");
                _writer.WriteEndElement();

                if (_pageHeaderFooter[8].ContainsKey("content"))
                {
                    _writer.WriteStartElement("text:span");
                    _writer.WriteAttributeString("text:style-name", "AllHeaderPageRight");
                    FillHeaderFooter(_pageHeaderFooter[8]["content"], 8);
                    _writer.WriteEndElement();
                }
            }
        }

        /// <summary>
        /// Function to load content of Allpage.
        /// </summary>
        private void SetAllPageFooter()
        {
            if (_pageHeaderFooter[9].Count > 0 || _pageHeaderFooter[10].Count > 0 || _pageHeaderFooter[11].Count > 0 || IsCropMarkChecked == true)
            {
                if (_pageHeaderFooter[9].ContainsKey("content"))
                {
                    _writer.WriteStartElement("text:span");
                    _writer.WriteAttributeString("text:style-name", "AllFooterPageLeft");
                    FillHeaderFooter(_pageHeaderFooter[9]["content"], 9);
                    _writer.WriteEndElement();
                }
                if (IsCropMarkChecked)
                {
                    AddHeaderCropMarks(_pageLayoutProperty);
                }
                _writer.WriteStartElement("text:tab");
                _writer.WriteEndElement();

                if (_pageHeaderFooter[10].ContainsKey("content"))
                {
                    _writer.WriteStartElement("text:span");
                    _writer.WriteAttributeString("text:style-name", "AllFooterPageNumber");
                    FillHeaderFooter(_pageHeaderFooter[10]["content"], 10);
                    _writer.WriteEndElement();
                }

                _writer.WriteStartElement("text:tab");
                _writer.WriteEndElement();

                if (_pageHeaderFooter[11].ContainsKey("content"))
                {
                    _writer.WriteStartElement("text:span");
                    _writer.WriteAttributeString("text:style-name", "AllFooterPageRight");
                    FillHeaderFooter(_pageHeaderFooter[11]["content"], 11);
                    _writer.WriteEndElement();
                }
            }
        }

        private void ODTPageFooter()
        {
            //office:styles Attributes.
            _writer.WriteStartElement("office:automatic-styles");
            //For First page
            for (int i = 0; i <= 5; i++)
            {
                FillPageHeaderFooter(i);
            }
            //For Left and Right page
            for (int i = 12; i <= 23; i++)
            {
                FillPageHeaderFooter(i);
            }
            //End Footer AllPage Style
            CreateHeaderFrame();
            PageLayout();

            // office:automatic-styles - Ends
            _writer.WriteEndElement();
        }

        private void FrameAlignment(string source)
        {
            if (_pageHeaderFooter[8].ContainsKey("content") && _pageHeaderFooter[8]["content"].IndexOf("last)") > 0 ||
                _pageHeaderFooter[14].ContainsKey("content") && _pageHeaderFooter[14]["content"].IndexOf("last)") > 0 ||
                _pageHeaderFooter[20].ContainsKey("content") && _pageHeaderFooter[20]["content"].IndexOf("last)") > 0)
            {
                if (source == "Variable")
                {
                    _writer.WriteAttributeString("fo:text-align", "end");
                }
                else
                {
                    _writer.WriteAttributeString("style:horizontal-pos", "right");
                }
            }
            else if (_pageHeaderFooter[7].ContainsKey("content") && _pageHeaderFooter[7]["content"].IndexOf("last)") > 0 ||
                 _pageHeaderFooter[13].ContainsKey("content") && _pageHeaderFooter[13]["content"].IndexOf("last)") > 0 ||
                _pageHeaderFooter[19].ContainsKey("content") && _pageHeaderFooter[19]["content"].IndexOf("last)") > 0)
            {
                if (source == "Variable")
                {
                    _writer.WriteAttributeString("fo:text-align", "center");
                }
                else
                {
                    _writer.WriteAttributeString("style:horizontal-pos", "center");
                }
            }
            else
            {
                if (source == "Variable")
                {
                    _writer.WriteAttributeString("fo:text-align", "start");
                }
                else
                {
                    _writer.WriteAttributeString("style:horizontal-pos", "left");
                }
            }
        }

        private void CreateHeaderFrame()
        {
            _writer.WriteStartElement("style:style");
            _writer.WriteAttributeString("style:name", "MP1");
            _writer.WriteAttributeString("style:family", "paragraph");
            _writer.WriteAttributeString("style:parent-style-name", "Frame_20_contents");
            _writer.WriteStartElement("style:paragraph-properties");

            //TD-2640 Right guideword alignment in Frame
            FrameAlignment("Variable");//TD-2802
            
            _writer.WriteAttributeString("style:justify-single-word", "false");
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            //TD-2566
            _writer.WriteStartElement("style:style");
            _writer.WriteAttributeString("style:name", "MT1");
            _writer.WriteAttributeString("style:family", "text");
            _writer.WriteStartElement("style:text-properties");
            _writer.WriteAttributeString("style:font-name-complex", "GenericFont");
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            _writer.WriteStartElement("style:style");
            _writer.WriteAttributeString("style:name", "Mfr1");
            _writer.WriteAttributeString("style:family", "graphic");
            _writer.WriteAttributeString("style:parent-style-name", "Frame");
            _writer.WriteStartElement("style:graphic-properties");
            _writer.WriteAttributeString("style:vertical-pos", "from-top");
            _writer.WriteAttributeString("style:vertical-rel", "page");
            FrameAlignment("Frame");//TD-2802
            _writer.WriteAttributeString("style:horizontal-rel", "paragraph");
            _writer.WriteAttributeString("fo:background-color", "transparent");
            _writer.WriteAttributeString("style:background-transparency", "100%");
            _writer.WriteAttributeString("fo:padding", "0in");
            _writer.WriteAttributeString("fo:border", "none");
            _writer.WriteAttributeString("style:shadow", "none");
            _writer.WriteAttributeString("style:flow-with-text", "false");
            _writer.WriteStartElement("style:background-image");
            _writer.WriteEndElement();

            _writer.WriteStartElement("style:columns");
            _writer.WriteAttributeString("fo:column-count", "1");
            _writer.WriteAttributeString("fo:column-gap", "0in");
            _writer.WriteEndElement();

            _writer.WriteEndElement();
            _writer.WriteEndElement();

            if (_multiLanguageHeader)
            {

                _writer.WriteStartElement("style:style");
                _writer.WriteAttributeString("style:name", "Mfr2");
                _writer.WriteAttributeString("style:family", "graphic");
                _writer.WriteAttributeString("style:parent-style-name", "Frame");
                _writer.WriteStartElement("style:graphic-properties");
                _writer.WriteAttributeString("style:vertical-pos", "from-top");
                _writer.WriteAttributeString("style:vertical-rel", "page");
                _writer.WriteAttributeString("style:horizontal-pos", "from-left");
                _writer.WriteAttributeString("style:horizontal-rel", "paragraph");
                _writer.WriteAttributeString("fo:background-color", "transparent");
                _writer.WriteAttributeString("style:background-transparency", "100%");
                _writer.WriteAttributeString("fo:padding", "0in");
                _writer.WriteAttributeString("fo:border", "none");
                _writer.WriteAttributeString("style:shadow", "none");
                _writer.WriteAttributeString("style:flow-with-text", "false");

                _writer.WriteStartElement("style:background-image");
                _writer.WriteEndElement();

                _writer.WriteStartElement("style:columns");
                _writer.WriteAttributeString("fo:column-count", "1");
                _writer.WriteAttributeString("fo:column-gap", "0in");
                _writer.WriteEndElement();

                _writer.WriteEndElement();
                _writer.WriteEndElement();
            }
            _writer.WriteStartElement("style:style");
            _writer.WriteAttributeString("style:name", "Mfr3");
            _writer.WriteAttributeString("style:family", "graphic");
            _writer.WriteAttributeString("style:parent-style-name", "Frame");
            _writer.WriteStartElement("style:graphic-properties");
            _writer.WriteAttributeString("style:vertical-pos", "from-top");
            _writer.WriteAttributeString("style:vertical-rel", "page");
            _writer.WriteAttributeString("style:horizontal-pos", "center");
            _writer.WriteAttributeString("style:horizontal-rel", "paragraph");
            _writer.WriteAttributeString("fo:background-color", "transparent");
            _writer.WriteAttributeString("style:background-transparency", "100%");
            _writer.WriteAttributeString("fo:padding", "0in");
            _writer.WriteAttributeString("fo:border", "none");
            _writer.WriteAttributeString("style:shadow", "none");
            _writer.WriteAttributeString("style:flow-with-text", "false");

            _writer.WriteStartElement("style:background-image");
            _writer.WriteEndElement();

            _writer.WriteStartElement("style:columns");
            _writer.WriteAttributeString("fo:column-count", "1");
            _writer.WriteAttributeString("fo:column-gap", "0in");
            _writer.WriteEndElement();

            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        private void FillPageHeaderFooter(int index)
        {
            _writer.WriteStartElement("style:style");
            _writer.WriteAttributeString("style:name", "PageHeaderFooter" + index);
            _writer.WriteAttributeString("style:family", "text");
            _writer.WriteStartElement("style:text-properties");
            foreach (KeyValuePair<string, string> attProperty in _pageHeaderFooter[index])
            {

                _writer.WriteAttributeString(attProperty.Key, attProperty.Value);
                if(attProperty.Key == "fo:font-family")
                    _writer.WriteAttributeString("style:font-name-complex", attProperty.Value);
            }
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        private bool FirstPageHeaderRuler(int index)
        {
            bool drawRuler = false;
             for(int i=index;i<index+3;i++)
            {
                 if(_pageHeaderFooter[i].ContainsKey("content") && _pageHeaderFooter[i]["content"] != string.Empty)
                 {
                     drawRuler = true;
                     break;
                 }
            }
            return drawRuler;
        }

        private void PageLayout()
        {
            bool isDirChange = GetPageDirection();

            _writer.WriteStartElement("style:page-layout");
            _writer.WriteAttributeString("style:name", "pm1");
            if (isMirrored)
            {
                _writer.WriteAttributeString("style:page-usage", "mirrored"); // If mirrored Page TD-410
                _writer.WriteStartElement("style:page-layout-properties");
                foreach (KeyValuePair<string, string> para in _pageLayoutProperty)
                {
                    _writer.WriteAttributeString(para.Key, para.Value);
                }
            }
            else
            {
                _writer.WriteStartElement("style:page-layout-properties");
                foreach (KeyValuePair<string, string> para in _pageLayoutProperty)
                {
                    _writer.WriteAttributeString(para.Key, para.Value);
                }
            }

            // START FootNote Seperator
            FootnoteSeperator();
            // END FootNote Seperator
            _writer.WriteEndElement();
            _writer.WriteStartElement("style:header-style");
            _writer.WriteEndElement();
            _writer.WriteStartElement("style:footer-style");
            _writer.WriteEndElement();
            _writer.WriteEndElement();


            /* pm2 starts */
            _writer.WriteStartElement("style:page-layout"); // pm2
            _writer.WriteAttributeString("style:name", "pm2"); // All Page
            if (isMirrored)
            {
                _writer.WriteAttributeString("style:page-usage", "mirrored"); // If mirrored Page TD 410
            }
            _writer.WriteStartElement("style:page-layout-properties");
            foreach (KeyValuePair<string, string> para in _pageLayoutProperty)
            {
                _writer.WriteAttributeString(para.Key, para.Value);
            }

            _writer.WriteStartElement("style:background-image");

            _writer.WriteEndElement();
            _writer.WriteEndElement(); // end of style:page-layout-properties
            //Header & Footer styles for pm2
            _writer.WriteStartElement("style:header-style");
            LoadHeaderSettings();
            _writer.WriteEndElement();
            _writer.WriteStartElement("style:footer-style");
            LoadFooterSettings(9);
            _writer.WriteEndElement();
            //End Header & Footer styles for pm2
            _writer.WriteEndElement();
            /* pm2 Ends*/

            /* pm3 starts */
            _writer.WriteStartElement("style:page-layout"); // pm3
            _writer.WriteAttributeString("style:name", "pm3"); // First Page
            _writer.WriteStartElement("style:page-layout-properties");
            foreach (KeyValuePair<string, string> para in _firstPageLayoutProperty)
            {
                _writer.WriteAttributeString(para.Key, para.Value);
            }

            _writer.WriteStartElement("style:background-image");
            _writer.WriteEndElement();
            // START FootNote Seperator
            FootnoteSeperator();
            // END FootNote Seperator
            _writer.WriteEndElement(); // end of style:page-layout-properties
            //Header & Footer styles for pm3
            _writer.WriteStartElement("style:header-style");
            LoadHeaderSettings();
            _writer.WriteEndElement();
            _writer.WriteStartElement("style:footer-style");
            LoadFooterSettings(3);
            _writer.WriteEndElement();
            //End Header & Footer styles for pm3
            _writer.WriteEndElement();
            /* pm3 ends*/

            if (_isFromExe)
            {
                /* pm6 starts */
                _writer.WriteStartElement("style:page-layout"); // pm6
                _writer.WriteAttributeString("style:name", "pm6"); // Index Page
                _writer.WriteStartElement("style:page-layout-properties");
                foreach (KeyValuePair<string, string> para in _pageLayoutProperty)
                {
                    _writer.WriteAttributeString(para.Key, para.Value);
                }
                _writer.WriteStartElement("style:background-image");
                _writer.WriteEndElement();
                // START FootNote Seperator
                FootnoteSeperator();
                // END FootNote Seperator
                _writer.WriteEndElement(); // end of style:page-layout-properties
                //Header & Footer styles for pm6
                _writer.WriteStartElement("style:header-style");
                _writer.WriteEndElement();
                _writer.WriteStartElement("style:footer-style");
                LoadFooterSettings(9);
                _writer.WriteEndElement();
                //End Header & Footer styles for pm6
                _writer.WriteEndElement();
                /* pm6 ends*/
            }

            if (isMirrored)
            {
                /* pm4 starts */
                _writer.WriteStartElement("style:page-layout"); // pm4
                _writer.WriteAttributeString("style:name", "pm4"); // Left Page
                _writer.WriteAttributeString("style:page-usage", "mirrored");
                _writer.WriteStartElement("style:page-layout-properties");
                foreach (KeyValuePair<string, string> para in _leftPageLayoutProperty)
                {
                    _writer.WriteAttributeString(para.Key, para.Value);
                }
                _writer.WriteStartElement("style:background-image");
                _writer.WriteEndElement();
                // START FootNote Seperator
                FootnoteSeperator();
                // END FootNote Seperator
                _writer.WriteEndElement(); // end of style:page-layout-properties
                //Header & Footer styles for pm4
                _writer.WriteStartElement("style:header-style");
                LoadHeaderSettings();
                _writer.WriteEndElement();
                _writer.WriteStartElement("style:footer-style");
                LoadFooterSettings(15);
                _writer.WriteEndElement();
                //End Header & Footer styles for pm4
                _writer.WriteEndElement();
                /* pm4 ends*/

                /* pm5 starts */
                _writer.WriteStartElement("style:page-layout"); // pm5
                _writer.WriteAttributeString("style:name", "pm5"); // Right Page
                _writer.WriteAttributeString("style:page-usage", "mirrored");
                _writer.WriteStartElement("style:page-layout-properties");
                foreach (KeyValuePair<string, string> para in _rightPageLayoutProperty)
                {
                    _writer.WriteAttributeString(para.Key, para.Value);
                }
                _writer.WriteStartElement("style:background-image");
                _writer.WriteEndElement();
                // START FootNote Seperator
                FootnoteSeperator();
                // END FootNote Seperator
                _writer.WriteEndElement(); // end of style:page-layout-properties
                //Header & Footer styles for pm5
                _writer.WriteStartElement("style:header-style");
                LoadHeaderSettings();
                _writer.WriteEndElement();
                _writer.WriteStartElement("style:footer-style");
                LoadFooterSettings(21);
                _writer.WriteEndElement();
                //End Header & Footer styles for pm5
                _writer.WriteEndElement();
                /* pm5 ends*/
            }

            _writer.WriteStartElement("style:page-layout");
            _writer.WriteAttributeString("style:name", "pm12");
            _writer.WriteStartElement("style:page-layout-properties");
            foreach (KeyValuePair<string, string> para in _pageLayoutProperty)
            {
                _writer.WriteAttributeString(para.Key, para.Value);
            }
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            _writer.WriteStartElement("style:page-layout");
            _writer.WriteAttributeString("style:name", "pm13");
            _writer.WriteStartElement("style:page-layout-properties");
            foreach (KeyValuePair<string, string> para in _pageLayoutProperty)
            {
                _writer.WriteAttributeString(para.Key, para.Value);
            }
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            /* pm7 starts - Non Footer settings */
            _writer.WriteStartElement("style:page-layout"); // pm7
            _writer.WriteAttributeString("style:name", "pm7"); // First Page
            _writer.WriteStartElement("style:page-layout-properties");
            foreach (KeyValuePair<string, string> para in _firstPageLayoutProperty)
            {
                _writer.WriteAttributeString(para.Key, para.Value);
            }
            if (isDirChange)
                _writer.WriteAttributeString("style:writing-mode", "rl-tb");
            _writer.WriteStartElement("style:background-image");
            _writer.WriteEndElement();
            // START FootNote Seperator
            FootnoteSeperator();
            // END FootNote Seperator
            _writer.WriteEndElement(); // end of style:page-layout-properties
            _writer.WriteEndElement();
            /* pm7 ends*/



        }

        /// <summary>
        /// To change the page direction based on the direction property from the CSS file.
        /// </summary>
        /// <returns>true / false</returns>
        public bool GetPageDirection()
        {
            if (_cssProperty.ContainsKey("body") && _cssProperty["body"].ContainsKey("direction"))
            {
                return (_cssProperty["body"]["direction"] == "rtl");
            }
            if (_cssProperty.ContainsKey("scrBody") && _cssProperty["scrBody"].ContainsKey("direction"))
            {
                return (_cssProperty["scrBody"]["direction"] == "rtl");
            }
            return false;
        }

        private void LoadFooterSettings(int index)
        {
            _writer.WriteStartElement("style:header-footer-properties");
            string cspace = "0.0pt";
            string space = "0.0pt";
            string height = "0.0pt";

            const string defaultUnit = "pt";
            if (_pageLayoutProperty.ContainsKey("fo:padding-top"))
            {
                float marginTop = float.Parse(Common.UnitConverterOO(_pageLayoutProperty["fo:margin-top"], defaultUnit).ToString().Replace(defaultUnit, ""));
                float paddingTop = float.Parse(Common.UnitConverterOO(_pageLayoutProperty["fo:padding-top"], defaultUnit).ToString().Replace(defaultUnit, ""));
                const float defaultfontSize = 12F;
                float calcSpace = (1 * marginTop / 2) + (1 * defaultfontSize / 2) + paddingTop;
                cspace = calcSpace + defaultUnit;
            }

            string ht = "svg:height";
            if (_pageHeaderFooter[index].ContainsKey("content") && _pageHeaderFooter[index]["content"].Length > 0
                || _pageHeaderFooter[index + 1].ContainsKey("content") && _pageHeaderFooter[index + 1]["content"].Length > 0
                || _pageHeaderFooter[index + 2].ContainsKey("content") && _pageHeaderFooter[index + 2]["content"].Length > 0)
            {
                space = cspace;
                height = "14.21" + defaultUnit;
            }
            else
            {
                ht = "fo:min-height";
            }
            _writer.WriteAttributeString("fo:margin-bottom", space); //Spacing
            _writer.WriteAttributeString(ht, height); //Height
            _writer.WriteAttributeString("fo:margin-left", "0pt");
            _writer.WriteAttributeString("fo:margin-right", "0pt");
            _writer.WriteAttributeString("style:dynamic-spacing", "false");
            _writer.WriteEndElement();
        }

        private void LoadHeaderSettings()
        {
            _writer.WriteStartElement("style:header-footer-properties");
            string height = "28.42pt";
            string space = "14.21pt";
            const string defaultUnit = "pt";
            if (_pageLayoutProperty.ContainsKey("fo:padding-top"))
            {
                float marginTop = float.Parse(Common.UnitConverterOO(_pageLayoutProperty["fo:margin-top"], defaultUnit).ToString().Replace(defaultUnit, ""));
                float paddingTop = float.Parse(Common.UnitConverterOO(_pageLayoutProperty["fo:padding-top"], defaultUnit).ToString().Replace(defaultUnit, ""));
                const float defaultfontSize = 12F;
                float calcSpace = (1 * marginTop / 2) + (1 * defaultfontSize / 2) + paddingTop;
                space = calcSpace + defaultUnit;
                height = "14.21" + defaultUnit;
            }
            _writer.WriteAttributeString("fo:margin-bottom", space); //Spacing
            _writer.WriteAttributeString("fo:min-height", height); //Height
            _writer.WriteAttributeString("fo:margin-left", "0pt");
            _writer.WriteAttributeString("fo:margin-right", "0pt");
            _writer.WriteAttributeString("style:dynamic-spacing", "false");
            _writer.WriteEndElement();
        }

        private void LoadHeaderFooterSettings()
        {
            _writer.WriteStartElement("style:header-footer-properties");
            string height = "28.42pt";
            string space = "14.21pt";
            const string defaultUnit = "pt";
            if (_pageLayoutProperty.ContainsKey("fo:padding-top"))
            {
                float marginTop = float.Parse(Common.UnitConverterOO(_pageLayoutProperty["fo:margin-top"], defaultUnit).ToString().Replace(defaultUnit, ""));
                float paddingTop = float.Parse(Common.UnitConverterOO(_pageLayoutProperty["fo:padding-top"], defaultUnit).ToString().Replace(defaultUnit, ""));
                const float defaultfontSize = 12F;
                float calcSpace = (1 * marginTop / 2) + (1 * defaultfontSize / 2) + paddingTop;
                space = calcSpace + defaultUnit;
                height = "14.21" + defaultUnit;
            }
            _writer.WriteAttributeString("fo:margin-bottom", space); //Spacing
            _writer.WriteAttributeString("fo:min-height", height); //Height
            _writer.WriteAttributeString("fo:margin-left", "0pt");
            _writer.WriteAttributeString("fo:margin-right", "0pt");
            _writer.WriteAttributeString("style:dynamic-spacing", "false");
            _writer.WriteEndElement();
        }

        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// To fill the haeder and footer values based on CSS input.
        /// 
        /// <list> 
        /// </list>
        /// </summary>
        /// <param name="contentValue">Value of the content from dictionary</param>
        /// <param name="index">Dictionary Index</param>
        /// <returns> </returns>
        /// -------------------------------------------------------------------------------------------
        private void FillHeaderFooter(string contentValue, int index)
        {
            try
            {
                if (_pageHeaderFooter[index].Count > 0)
                {
                    if (contentValue.IndexOf("first)") > 0 && contentValue.IndexOf("last)") > 0)
                    {
                    }
                    else if (contentValue.IndexOf("first)") > 0)
                    {
                        _writer.WriteStartElement("text:variable-get");
                        _writer.WriteAttributeString("text:name", "Left_Guideword_L");
                        _writer.WriteAttributeString("office:value-type", "string");
                        _writer.WriteEndElement();

                        if (_multiLanguageHeader)
                        {
                            _writer.WriteStartElement("draw:frame");
                            _writer.WriteAttributeString("draw:style-name", "Mfr2");
                            _writer.WriteAttributeString("draw:name", "Frame1");
                            _writer.WriteAttributeString("text:anchor-type", "paragraph");
                            string value = "0pt";
                            if (!String.IsNullOrEmpty(_pageLayoutProperty["fo:margin-top"]))
                            {
                                value = _pageLayoutProperty["fo:margin-top"];
                            }
                            _writer.WriteAttributeString("svg:x", "0pt");
                            _writer.WriteAttributeString("svg:y", value);
                            _writer.WriteAttributeString("fo:min-width", "35pt");
                            _writer.WriteAttributeString("svg:height", "14.14pt");
                            _writer.WriteAttributeString("draw:z-index", "1");
                            _writer.WriteStartElement("draw:text-box");
                            _writer.WriteStartElement("text:p");
                            _writer.WriteAttributeString("text:style-name", "MP1");
                            _writer.WriteStartElement("text:variable-get");
                            _writer.WriteAttributeString("text:name", "Left_Guideword_L");
                            _writer.WriteAttributeString("office:value-type", "string");
                            _writer.WriteEndElement();
                            _writer.WriteEndElement();
                            _writer.WriteEndElement();
                            _writer.WriteEndElement();
                        }
                    }
                    else if (contentValue.IndexOf("last)") > 0)
                    {
                        string value = "0pt";
                        if (_multiLanguageHeader)
                        {
                            _writer.WriteStartElement("draw:frame");
                            _writer.WriteAttributeString("draw:style-name", "Mfr2");
                            _writer.WriteAttributeString("draw:name", "Frame1");
                            _writer.WriteAttributeString("text:anchor-type", "paragraph");
                            if (!String.IsNullOrEmpty(_pageLayoutProperty["fo:margin-top"]))
                            {
                                value = _pageLayoutProperty["fo:margin-top"];
                            }
                            _writer.WriteAttributeString("svg:x", "0pt");
                            _writer.WriteAttributeString("svg:y", value);
                            _writer.WriteAttributeString("fo:min-width", "35pt");
                            _writer.WriteAttributeString("svg:height", "14.14pt");
                            _writer.WriteAttributeString("draw:z-index", "1");
                            _writer.WriteStartElement("draw:text-box");
                            _writer.WriteStartElement("text:p");
                            _writer.WriteAttributeString("text:style-name", "MP1");
                            _writer.WriteStartElement("text:variable-get");
                            _writer.WriteAttributeString("text:name", "Right_Guideword_L");
                            _writer.WriteAttributeString("office:value-type", "string");
                            _writer.WriteEndElement();
                            _writer.WriteEndElement();
                            _writer.WriteEndElement();
                            _writer.WriteEndElement();
                        }

                        _writer.WriteStartElement("draw:frame");
                        _writer.WriteAttributeString("draw:style-name", "Mfr1");
                        _writer.WriteAttributeString("draw:name", "Frame1");
                        _writer.WriteAttributeString("text:anchor-type", "paragraph");
                        value = "56.89pt";
                        if (!String.IsNullOrEmpty(_pageLayoutProperty["fo:margin-top"]))
                        {
                            value = _pageLayoutProperty["fo:margin-top"];
                        }
                        _writer.WriteAttributeString("svg:y", value);
                        _writer.WriteAttributeString("fo:min-width", "35pt");
                        _writer.WriteAttributeString("svg:height", "14.14pt");
                        _writer.WriteAttributeString("draw:z-index", "1");
                        _writer.WriteStartElement("draw:text-box");
                        _writer.WriteStartElement("text:p");
                        _writer.WriteAttributeString("text:style-name", "MP1");
                        _writer.WriteStartElement("text:variable-get");
                        _writer.WriteAttributeString("text:name", "Right_Guideword_R");
                        _writer.WriteAttributeString("office:value-type", "string");
                        _writer.WriteEndElement();
                        _writer.WriteEndElement();
                        _writer.WriteEndElement();
                        _writer.WriteEndElement();
                    }
                    else if (contentValue.IndexOf("page") > 0 && contentValue.IndexOf("counter") >= 0)
                    {
                        _writer.WriteStartElement("text:page-number");
                        _writer.WriteAttributeString("text:select-page", "current");
                        _writer.WriteString("4");
                        _writer.WriteEndElement();
                    }
                    else
                    {
                        _writer.WriteString(_pageHeaderFooter[index]["content"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        private bool IsContentAvailable(byte startIndex)
        {
            try
            {
                for (int i = startIndex; i < startIndex + 3; i++)
                {
                    if (_pageHeaderFooter[i].ContainsKey("content"))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Dimension calculation for Crop_Marks by Placing horizontal and Vertical line at each corners.
        /// </summary>
        /// 
        private void AddHeaderCropMarks(Dictionary<string, string> _pageLayoutProperty)
        {
            _pageLayoutProperty = _pageLayoutProperty;
            if (_firstPageLayoutProperty.Count > 0)
            {
                double pageMargin = double.Parse(Common.UnitConverterOO(_pageLayoutProperty["fo:margin-left"], "cm").ToString().Replace("cm", ""));
                double pageFirstMargin = double.Parse(Common.UnitConverterOO(_firstPageLayoutProperty["fo:margin-left"], "cm").ToString().Replace("cm", ""));
                if (pageMargin < pageFirstMargin)
                {
                    _pageLayoutProperty = _firstPageLayoutProperty;
                }
            }

            // Marks-Crop Calculation 
            _PageWidth = float.Parse(_pageLayoutProperty["fo:page-width"].Replace("cm", ""));
            _leftMargin = float.Parse(Common.UnitConverterOO(_pageLayoutProperty["fo:margin-left"], "cm").ToString().Replace("cm", ""));
            _rightMargin = float.Parse(Common.UnitConverterOO(_pageLayoutProperty["fo:margin-right"], "cm").ToString().Replace("cm", ""));
            _rightPosition = _PageWidth - (_leftMargin + _rightMargin); // position

            _PageHeight = float.Parse(_pageLayoutProperty["fo:page-height"].Replace("cm", ""));
            _topMargin = float.Parse(Common.UnitConverterOO(_pageLayoutProperty["fo:margin-top"], "cm").ToString().Replace("cm", ""));
            _bottomMargin = float.Parse(Common.UnitConverterOO(_pageLayoutProperty["fo:margin-bottom"], "cm").ToString().Replace("cm", ""));
            _bottomPosition = _PageHeight - (_topMargin + _bottomMargin); // position

            //top - left - horizontal
            _writer.WriteStartElement("draw:line");
            _writer.WriteAttributeString("text:anchor-type", "paragraph");
            _writer.WriteAttributeString("draw:style-name", "Mgr1");
            _writer.WriteAttributeString("draw:z-index", "0");
            _writer.WriteAttributeString("svg:x1", (-_gapToMargin - _leftMargin + 1.27) + "cm");
            _writer.WriteAttributeString("svg:y1", -_topMargin + 1.27 + "cm");
            _writer.WriteAttributeString("svg:x2", (-_gapToMargin - _leftMargin - _lineLength + 1.27) + "cm");
            _writer.WriteAttributeString("svg:y2", -_topMargin + 1.27 + "cm");
            _writer.WriteEndElement();

            // top - left - vertical
            _writer.WriteStartElement("draw:line");
            _writer.WriteAttributeString("text:anchor-type", "paragraph");
            _writer.WriteAttributeString("draw:style-name", "Mgr1");
            _writer.WriteAttributeString("draw:z-index", "1");
            _writer.WriteAttributeString("svg:x1", -_leftMargin + 1.27 + "cm");
            _writer.WriteAttributeString("svg:y1", (-_gapToMargin - _topMargin + 1.27) + "cm");
            _writer.WriteAttributeString("svg:x2", -_leftMargin + 1.27 + "cm");
            _writer.WriteAttributeString("svg:y2", (-_gapToMargin - _topMargin - _lineLength + 1.27) + "cm");
            _writer.WriteEndElement();

            //top - right - horizontal
            _writer.WriteStartElement("draw:line");
            _writer.WriteAttributeString("text:anchor-type", "paragraph");
            _writer.WriteAttributeString("draw:style-name", "Mgr1");
            _writer.WriteAttributeString("draw:z-index", "2");
            _writer.WriteAttributeString("svg:x1", (_rightPosition + _rightMargin + _gapToMargin - 1.27) + "cm");
            _writer.WriteAttributeString("svg:y1", -_topMargin + 1.27 + "cm");
            _writer.WriteAttributeString("svg:x2", (_rightPosition + _rightMargin + _gapToMargin + _lineLength - 1.27) + "cm");
            _writer.WriteAttributeString("svg:y2", -_topMargin + 1.27 + "cm");
            _writer.WriteEndElement();

            // top -right - vertical
            _writer.WriteStartElement("draw:line");
            _writer.WriteAttributeString("text:anchor-type", "paragraph");
            _writer.WriteAttributeString("draw:style-name", "Mgr1");
            _writer.WriteAttributeString("draw:z-index", "3");
            _writer.WriteAttributeString("svg:x1", (_rightPosition + _rightMargin - 1.27) + "cm");
            _writer.WriteAttributeString("svg:y1", (-_gapToMargin - _topMargin - _lineLength + 1.27) + "cm");
            _writer.WriteAttributeString("svg:x2", (_rightPosition + _rightMargin - 1.27) + "cm");
            _writer.WriteAttributeString("svg:y2", (-_gapToMargin - _topMargin + 1.27) + "cm");
            _writer.WriteEndElement();

            //bot - left - horizontal
            _writer.WriteStartElement("draw:line");
            _writer.WriteAttributeString("text:anchor-type", "paragraph");
            _writer.WriteAttributeString("draw:style-name", "Mgr1");
            _writer.WriteAttributeString("draw:z-index", "4");
            _writer.WriteAttributeString("svg:x1", (-_gapToMargin - _leftMargin - _lineLength + 1.27) + "cm");
            _writer.WriteAttributeString("svg:y1", (_bottomPosition + _bottomMargin - 1.27) + "cm");
            _writer.WriteAttributeString("svg:x2", (-_gapToMargin - _leftMargin + 1.27) + "cm");
            _writer.WriteAttributeString("svg:y2", (_bottomPosition + _bottomMargin - 1.27) + "cm");
            _writer.WriteEndElement();

            // bot - left - vertical
            _writer.WriteStartElement("draw:line");
            _writer.WriteAttributeString("text:anchor-type", "paragraph");
            _writer.WriteAttributeString("draw:style-name", "Mgr1");
            _writer.WriteAttributeString("draw:z-index", "5");
            _writer.WriteAttributeString("svg:x1", -_leftMargin + 1.27 + "cm");
            _writer.WriteAttributeString("svg:y1", (_bottomPosition + _gapToMargin + _bottomMargin + _lineLength - 1.27) + "cm");
            _writer.WriteAttributeString("svg:x2", -_leftMargin + 1.27 + "cm");
            _writer.WriteAttributeString("svg:y2", (_bottomPosition + _gapToMargin + _bottomMargin - 1.27) + "cm");
            _writer.WriteEndElement();

            //bot - right - horizontal
            _writer.WriteStartElement("draw:line");
            _writer.WriteAttributeString("text:anchor-type", "paragraph");
            _writer.WriteAttributeString("draw:style-name", "Mgr1");
            _writer.WriteAttributeString("draw:z-index", "6");
            _writer.WriteAttributeString("svg:x1", (_rightPosition + _bottomMargin + _gapToMargin - 1.27) + "cm");
            _writer.WriteAttributeString("svg:y1", (_bottomPosition + _rightMargin - 1.27) + "cm");
            _writer.WriteAttributeString("svg:x2", (_rightPosition + _bottomMargin + _gapToMargin + _lineLength - 1.27) + "cm");
            _writer.WriteAttributeString("svg:y2", (_bottomPosition + _rightMargin - 1.27) + "cm");
            _writer.WriteEndElement();

            // bot -right - vertical
            _writer.WriteStartElement("draw:line");
            _writer.WriteAttributeString("text:anchor-type", "paragraph");
            _writer.WriteAttributeString("draw:style-name", "Mgr1");
            _writer.WriteAttributeString("draw:z-index", "7");
            _writer.WriteAttributeString("svg:x1", (_rightPosition + _rightMargin - 1.27) + "cm");
            _writer.WriteAttributeString("svg:y1", (_bottomPosition + _gapToMargin + _rightMargin + _lineLength - 1.27) + "cm");
            _writer.WriteAttributeString("svg:x2", _rightPosition + _rightMargin - 1.27 + "cm");
            _writer.WriteAttributeString("svg:y2", (_bottomPosition + _gapToMargin + _rightMargin - 1.27) + "cm");
            _writer.WriteEndElement();
        }

        /// <summary>
        /// To verify default value for page and increase the page values when the mark: crop is ON
        /// </summary>
        /// <param name="keyName">Page property</param>
        /// <param name="isPageFirst">bool - is first page?</param>
        private void ChangePageProperty(string keyName, bool isPageFirst)
        {
            if (isPageFirst)
            {
                if (_firstPageLayoutProperty.Count > 0)
                {
                    if (keyName == "fo:page-width" || keyName == "fo:page-height")
                    {
                        _firstPageLayoutProperty[keyName] = (float.Parse(Common.UnitConverterOO(_firstPageLayoutProperty[keyName], "cm").ToString().Replace("cm", "")) + 2.54) + "cm";
                    }
                    else
                    {
                        _firstPageLayoutProperty[keyName] = (float.Parse(Common.UnitConverterOO(_firstPageLayoutProperty[keyName], "cm").ToString().Replace("cm", "")) + 1.27) + "cm";
                        IsMarginChanged = true;
                    }
                    _isFirstpageDimensionChanged = true;
                }
            }
            else
            {
                if ((keyName == "fo:page-width") || (keyName == "fo:page-height"))
                {
                    _pageLayoutProperty[keyName] = (float.Parse(Common.UnitConverterOO(_pageLayoutProperty[keyName], "cm").ToString().Replace("cm", "")) + 2.54) + "cm";
                }
                else
                {
                    _pageLayoutProperty[keyName] = (float.Parse(Common.UnitConverterOO(_pageLayoutProperty[keyName], "cm").ToString().Replace("cm", "")) + 1.27) + "cm";
                    IsMarginChanged = true;
                }
            }
        }

        #region FootnoteSeperator

        private void FootnoteSeperator()
        {
            string fNoteColor = "#000000";
            string fNotePadTop = "0.0398in";
            string fNotePadBot = "0.0398in";
            _writer.WriteStartElement("style:footnote-sep");
            if (_styleName.FootNoteSeperator.ContainsKey("fo:border-top-style"))
            {
                string border = _styleName.FootNoteSeperator["fo:border-top-style"];

                if (border.ToLower() == "thin")
                {
                    _writer.WriteAttributeString("style:width", "0.5pt");
                }
                else if (border.ToLower() == "medium")
                {
                    _writer.WriteAttributeString("style:width", "1.0pt");
                }
                else if (border.ToLower() == "thick")
                {
                    _writer.WriteAttributeString("style:width", "1.5pt");
                }
                else if (border.ToLower() == "solid")
                {
                    _writer.WriteAttributeString("style:width", "0.0071in");
                    _writer.WriteAttributeString("style:line-style", border);
                }
                else
                {
                    _writer.WriteAttributeString("style:width", "0.0071in");
                }
            }

            if (_styleName.FootNoteSeperator.ContainsKey("fo:border-top-color"))
            {
                fNoteColor = _styleName.FootNoteSeperator["fo:border-top-color"];
            }

            if (_styleName.FootNoteSeperator.ContainsKey("fo:padding-top"))
            {
                fNotePadTop = _styleName.FootNoteSeperator["fo:padding-top"];
            }

            if (_styleName.FootNoteSeperator.ContainsKey("fo:padding-bottom"))
            {
                fNotePadBot = _styleName.FootNoteSeperator["fo:padding-bottom"];
            }

            _writer.WriteAttributeString("style:distance-before-sep", fNotePadTop);
            _writer.WriteAttributeString("style:distance-after-sep", fNotePadBot);
            _writer.WriteAttributeString("style:color", fNoteColor);
            _writer.WriteAttributeString("style:adjustment", "centre");
            _writer.WriteAttributeString("style:rel-width", "100%");

            _writer.WriteEndElement();
        }
        #endregion

        #endregion

        #region MergeTage
        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Merge Standard tag property(h1) to css tag property(h1.entry)
        /// <list> 
        /// </list>
        /// </summary>
        /// <returns>Dictionary with TAG Properties.</returns>
        /// -------------------------------------------------------------------------------------------
        protected void MergeTag()
        {
            Dictionary<string, string> tagProperty = new Dictionary<string, string>();
            OOUtility util = new OOUtility();
            XmlDocument doc = new XmlDocument();
            doc.Load(_styleFilePath);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("st", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
            nsmgr.AddNamespace("fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");
            XmlElement root = doc.DocumentElement;
            try
            {
                string[] _aeName = new string[2];
                bool isPropertyExist;
                foreach (string tag in _aeName) // for each tag like h1, h2 etc.
                {
                    string style = "//st:style[@st:name='" + tag + "']"; // Find class
                    XmlNode node = root.SelectSingleNode(style, nsmgr);

                    XmlNode paragraphNode = null;
                    XmlNode textNode = null;
                    for (int i = 0; i < node.ChildNodes.Count; i++) // find paragraph & Text node
                    {
                        if (node.ChildNodes[i].Name == "style:paragraph-properties")
                        {
                            paragraphNode = node.ChildNodes[i];
                        }
                        else if (node.ChildNodes[i].Name == "style:text-properties")
                        {
                            textNode = node.ChildNodes[i];
                        }
                    }

                    string[] tagClass = tag.Split('.'); // tagClass[0] - className & tagClass[1] - TagName
                    string tagClassN = string.Empty;
                    if (tagClass.Length == 1)
                    {
                        tagClassN = tagClass[0];
                    }
                    else if (tagClass.Length == 2)
                    {
                        tagClassN = tagClass[1];
                    }
                    tagProperty = _tagProperty[tagClassN];

                    foreach (KeyValuePair<string, string> prop in tagProperty) // font-size: etc.
                    {
                        isPropertyExist = false;

                        for (int i = 0; i < node.ChildNodes.Count; i++) // open paragraph & Text node
                        {
                            XmlNode child = node.ChildNodes[i];

                            foreach (XmlAttribute attribute in child.Attributes) // open it'line attributes
                            {
                                if (prop.Key == attribute.Name)
                                {
                                    isPropertyExist = true;
                                    i = 10; // exit two loops
                                    break;
                                }
                            }
                        }
                        if (isPropertyExist == false)
                        {
                            string propertyName = prop.Key.Substring(prop.Key.IndexOf(':') + 1);
                            if (_allParagraphProperty.ContainsKey(propertyName)) // add property in Paragraph node
                            {
                                if (paragraphNode == null)
                                {
                                    paragraphNode = node.InsertBefore(doc.CreateElement("style:paragraph-properties", nsmgr.LookupNamespace("st").ToString()), node.FirstChild);
                                }
                                paragraphNode.Attributes.Append(doc.CreateAttribute(prop.Key, nsmgr.LookupNamespace("st").ToString())).InnerText = prop.Value;
                            }
                            else if (_allTextProperty.ContainsKey(propertyName)) // add fullString in Paragraph node
                            {
                                if (textNode == null)
                                {
                                    textNode = node.AppendChild(doc.CreateElement("style:text-properties", nsmgr.LookupNamespace("st").ToString()));
                                }
                                textNode.Attributes.Append(doc.CreateAttribute(prop.Key, nsmgr.LookupNamespace("fo").ToString())).InnerText = prop.Value;
                            }
                        }
                    }

                    if (tagClassN == "div")
                    {
                        XmlDocumentFragment styleNode = doc.CreateDocumentFragment();
                        styleNode.InnerXml = node.OuterXml;
                        node.ParentNode.InsertAfter(styleNode, node);

                        XmlElement nameElement = (XmlElement)node;
                        nameElement.SetAttribute("style:name", tagClass[0]);
                    }
                }
                doc.Save(_styleFilePath);
            }
            catch
            {
            }
        }
        #endregion
    }
}