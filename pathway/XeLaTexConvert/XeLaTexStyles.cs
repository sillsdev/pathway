// --------------------------------------------------------------------------------------------
// <copyright file="InStyles.cs" from='2009' to='2010' company='SIL International'>
//      Copyright © 2009, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Creates the InDesign Styles file 
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class XeLaTexStyles : XeLaTexStyleBase
    {
        #region Private Variables

        //XmlTextWriter _writer;

        Dictionary<string, Dictionary<string, string>> _cssProperty = new Dictionary<string, Dictionary<string, string>>();
        Dictionary<string, Dictionary<string, string>> _IDAllClass = new Dictionary<string, Dictionary<string, string>>();
        private Dictionary<string, string> _IDProperty = new Dictionary<string, string>();
        private Dictionary<string, string> _IDClass = new Dictionary<string, string>();
        //CSSTree _cssTree = new CSSTree();
        XeLaTexMapProperty mapProperty = new XeLaTexMapProperty();
        private StreamWriter _xetexFile;
        private List<string> _inlineStyle;
        private List<string> _inlineText;
        private List<string> _includePackageList;
        Dictionary<string, List<string>> _classInlineStyle = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>> _classInlineText = new Dictionary<string, List<string>>();
        //public InDesignStyles InDesignStyles;
        //public ArrayList _FootNote;
        protected bool IsMirrored = false;
        public StringBuilder PageStyle = new StringBuilder();
        readonly Dictionary<string, string> _pageStyleFormat = new Dictionary<string, string>();
        public string referenceFormat = string.Empty;
        private Dictionary<string, string> _langFontDictionary = new Dictionary<string, string>();

        #endregion

        #region Public Variables

        public Dictionary<string, string> LangFontDictionary
        {
            get { return _langFontDictionary; }
            set { _langFontDictionary = value; }
        }

        #endregion


        public Dictionary<string, List<string>> CreateXeTexStyles(PublicationInformation projInfo, StreamWriter xetexFile, Dictionary<string, Dictionary<string, string>> cssProperty)
        {
            try
            {
                _includePackageList = new List<string>();
                _inlineText = new List<string>();
                _xetexFile = xetexFile;
                _cssProperty = cssProperty;
                _projInfo = projInfo;
                LoadPageStyleFormat();
                CreatePageStyle();
                CreateStyle();  // CODE HERE
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            //return _IDAllClass;
            return _classInlineStyle;
        }

        private void LoadPageStyleFormat()
        {
            if (_cssProperty.ContainsKey("@page:left") || _cssProperty.ContainsKey("@page:right"))
            {
                IsMirrored = true;
            }

            if (IsMirrored)
            {
                _pageStyleFormat.Add("@page:left-top-left", "\\fancyhead[LO]");
                _pageStyleFormat.Add("@page:left-top-center", "\\fancyhead[CO]");
                _pageStyleFormat.Add("@page:left-top-right", "\\fancyhead[RO]");
                _pageStyleFormat.Add("@page:left-bottom-left", "\\fancyfoot[LO]");
                _pageStyleFormat.Add("@page:left-bottom-center", "\\fancyfoot[CO]");
                _pageStyleFormat.Add("@page:left-bottom-right", "\\fancyfoot[RO]");
                _pageStyleFormat.Add("@page:right-top-left", "\\fancyhead[LE]");
                _pageStyleFormat.Add("@page:right-top-center", "\\fancyhead[CE]");
                _pageStyleFormat.Add("@page:right-top-right", "\\fancyhead[RE]");
                _pageStyleFormat.Add("@page:right-bottom-left", "\\fancyfoot[LE]");
                _pageStyleFormat.Add("@page:right-bottom-center", "\\fancyfoot[CE]");
                _pageStyleFormat.Add("@page:right-bottom-right", "\\fancyfoot[RE]");
            }
            else
            {
                _pageStyleFormat.Add("@page-top-left", "\\lhead");
                _pageStyleFormat.Add("@page-top-center", "\\chead");
                _pageStyleFormat.Add("@page-top-right", "\\rhead");
                _pageStyleFormat.Add("@page-bottom-left", "\\lfoot");
                _pageStyleFormat.Add("@page-bottom-center", "\\cfoot");
                _pageStyleFormat.Add("@page-bottom-right", "\\rfoot");
                _pageStyleFormat.Add("@page:first-top-left", "\\lhead");
                _pageStyleFormat.Add("@page:first-top-center", "\\chead");
                _pageStyleFormat.Add("@page:first-top-right", "\\rhead");
                _pageStyleFormat.Add("@page:first-bottom-left", "\\lfoot");
                _pageStyleFormat.Add("@page:first-bottom-center", "\\cfoot");
                _pageStyleFormat.Add("@page:first-bottom-right", "\\rfoot");
            }
            
        }

        private void CreatePageStyle()   // PageHeaderFooter() in odt conversion 
        {
            CreateHeaderFooter();
            CreatePageFirstPage();
        }

        private void CreateHeaderFooter()
        {
            PageStyle.Append("\\pagestyle{plain}");
            PageStyle.AppendLine("\\pagestyle{fancy}");
			PageStyle.AppendLine("\\fancyhead{}");
            PageStyle.AppendLine("\\fancyfoot{}");
            if (_cssProperty.ContainsKey("@page:left") || _cssProperty.ContainsKey("@page:right"))
            {
                IsMirrored = true;
            }

            string pageName = "@page";
            SetPageHeaderFooter(pageName);

            pageName = "@page:first";
            SetPageHeaderFooter(pageName);

            if (IsMirrored)
            {
                pageName = "@page:left";
                SetPageHeaderFooter(pageName);

                pageName = "@page:right";
                SetPageHeaderFooter(pageName);
            }
            PageStyle.AppendLine("\\renewcommand{\\headrulewidth}{0.4pt} \\renewcommand{\\footrulewidth}{0.4pt}");
            //PageStyle.AppendLine("\\fancyfoot{}");
        }

        private void SetPageHeaderFooter(string pageName)
        {
            string[] searchKey = { "top-left", "top-center", "top-right", "bottom-left", "bottom-center", "bottom-right" };
            for (int i = 0; i < 6; i++)
            {
                string currentPagePosition = pageName + "-" + searchKey[i];   // Ex: page:first-topleft
                if (_cssProperty.ContainsKey(currentPagePosition))
                {
                    Dictionary<string, string> cssProp = _cssProperty[currentPagePosition];
                    

                    //_LOProperty = mapProperty.IDProperty(cssProp););
                    foreach (KeyValuePair<string, string> para in cssProp)
                    {
                        if (para.Key == "content" && _pageStyleFormat.ContainsKey(currentPagePosition))
                        {
                            string writingString = para.Value.Replace("\"", "").Replace("'", "");
                            if (writingString.IndexOf("guidewordfirst") >= 0)
                            {
                                PageStyle.AppendLine(_pageStyleFormat[currentPagePosition] + "{\\rightmark}");
                            }
                            else if (writingString.IndexOf("guidewordlast") >= 0)
                            {
                                PageStyle.AppendLine(_pageStyleFormat[currentPagePosition] + "{\\leftmark}");
                            }
                            else if (writingString.IndexOf("counter(page)") >= 0)
                            {
                                PageStyle.AppendLine(_pageStyleFormat[currentPagePosition] + "{\\thepage}");
                            }
                            if (writingString.IndexOf("booknamefirst") >= 0)
                            {
                                PageStyle.AppendLine(_pageStyleFormat[currentPagePosition] + "{\\rightmark}");
                            }
                            else if (writingString.IndexOf("booknamelast") >= 0)
                            {
                                PageStyle.AppendLine(_pageStyleFormat[currentPagePosition] + "{\\leftmark}");
                            }

                            if (!_includePackageList.Contains("fancyhdr"))
                                _includePackageList.Add("fancyhdr");
                        }
                        else if (para.Key.ToLower() == "-ps-referenceformat")
                        {
                            //if (_projInfo.HeaderReferenceFormat.Trim().Length == 0)
                                _projInfo.HeaderReferenceFormat = para.Value;
                        }
                    }


                }
            }
           // PageStyle.AppendLine("\\renewcommand{\\headrulewidth}{0.4pt} \\renewcommand{\\footrulewidth}{0.4pt}");
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
            Dictionary<string, string> FootNoteSeperator = ProcessPageProperty(pageName);

            if (_leftPageLayoutProperty.Count > 0 || _rightPageLayoutProperty.Count > 0)
            {
                isMirrored = true;
            }
        }

        private Dictionary<string, string> ProcessPageProperty(string pageName)
        {
            Dictionary<string, string> pageLayoutProperty = new Dictionary<string, string>();
            if (_cssProperty.ContainsKey(pageName))
            {
                Dictionary<string, string> cssClass1 = _cssProperty[pageName];
                //_LOProperty = mapProperty.IDProperty(cssClass1);
                //foreach (KeyValuePair<string, string> para in _LOProperty)
                //    if (para.Key.ToLower() == "-ps-referenceformat-string")
                //    {
                //        _styleName.ReferenceFormat = para.Value.Replace("\"", "");
                //    }
                //    else if (para.Key == "border-top"
                //             || para.Key == "border-top-style"
                //             || para.Key == "border-top-width"
                //             || para.Key == "border-top-color"
                //             || para.Key == "border-bottom"
                //             || para.Key == "border-left"
                //             || para.Key == "border-right"
                //             || para.Key == "margin-top"
                //             || para.Key == "margin-bottom"
                //             || para.Key == "margin-left"
                //             || para.Key == "margin-right"
                //             || para.Key == "padding-top"
                //             || para.Key == "padding-bottom"
                //             || para.Key == "padding-left"
                //             || para.Key == "padding-right"
                //             || para.Key == "visibility")
                //    {
                //        pageLayoutProperty[_allPageLayoutProperty[para.Key].ToString() + para.Key] =

                //            para.Value;
                //    }
                //    else if (para.Key == "size" || para.Key == "page-width" || para.Key == "page-height")
                //    {
                //        if (para.Value.ToLower() == "landscape" || para.Value.ToLower() == "portrait" ||
                //            para.Value.ToLower() == "auto")
                //        {
                //            pageLayoutProperty["style:print-orientation"] = para.Value.ToLower();
                //        }
                //        else
                //        {
                //            pageLayoutProperty[_allPageLayoutProperty[para.Key].ToString() + para.Key] = para.Value;
                //        }
                //    }
                //    else if (para.Key == "color" || para.Key == "background-color")
                //    {
                //        pageLayoutProperty[_allPageLayoutProperty[para.Key].ToString() + para.Key] = para.Value;
                //    }
                //    else if (para.Key == "marks" && para.Value == "crop")
                //    {
                //        //StylesXML.IsCropMarkChecked = true;
                //    }
                //    else if (para.Key.ToLower() == "dictionary")
                //    {
                //        if (para.Value == "true")
                //        {
                //            //_isDictionary = true;
                //        }
                //    }
                //    else
                //    {
                //        //styleAttributeInfo = _mapProperty.MapSingleValue(styleAttributeInfo);
                //    }
            }
            return pageLayoutProperty;
        }

        private void CreateStyle()
        {
            //foreach (KeyValuePair<string, Dictionary<string, string>> cssClass in _cssProperty)
            //{
            //    if (cssClass.Key.IndexOf("@page") >= 0 && (cssClass.Key.IndexOf("@page-") == -1 && cssClass.Key.IndexOf("@page:") == -1))
            //    {
            //        _inlineStyle = new List<string>();
            //        _includePackageList = new List<string>();
            //        string xeLaTexProperty = mapProperty.XeLaTexPageProperty(cssClass.Value, cssClass.Key, _inlineStyle, _includePackageList);
            //        _classInlineStyle[cssClass.Key] = _includePackageList;
            //    }

            //}
            //\font\hoefler="Hoefler Text/B:Letter Case=Small Caps" at 12pt
            foreach (KeyValuePair<string, Dictionary<string, string>> cssClass in _cssProperty)
            {
                if (cssClass.Key.IndexOf("@page") >= 0 || cssClass.Key.IndexOf("h1") >= 0 || 
                    cssClass.Key.IndexOf("h2") >= 0 || cssClass.Key.IndexOf("h3") >= 0 || 
                    cssClass.Key.IndexOf("h4") >= 0 || cssClass.Key.IndexOf("h5") >= 0 || 
                    cssClass.Key.IndexOf("h6") >= 0) continue;

                _inlineStyle = new List<string>();
                _inlineText = new List<string>();
                string replaceNumberInStyle = Common.ReplaceCSSClassName(cssClass.Key);
                string xeLaTexProperty = mapProperty.XeLaTexProperty(cssClass.Value, replaceNumberInStyle, _inlineStyle, _includePackageList, _inlineText, LangFontDictionary);

                //if (_inlineStyle.Count > 0)
                {
                    _classInlineStyle[replaceNumberInStyle] = _inlineStyle;
                    if (_inlineText.Count > 0)
                        _classInlineText[replaceNumberInStyle] = _inlineText;
                }
                if (xeLaTexProperty.Trim().Length > 0 && Common.Testing)
                {
                    //_xetexFile.WriteLine(xeTexProperty);
                }
                //_IDClass = new Dictionary<string, string>(); // note: ToDo seperate the process
                //_IDAllClass[cssClass.Key] = _IDClass;

                //_xetexFile.WriteLine("nopagenumbers");

                //foreach (KeyValuePair<string, string> property in _IDProperty)
                //{
                //    if (property.Key == "AppliedFont")
                //    {
                //        _IDClass[property.Key] = property.Value;
                //        continue;
                //    }
                //    if (property.Key == "StrokeColor")
                //    {
                //        _IDClass[property.Key] = property.Value;
                //        InsertBackgroundColor(property.Value);
                //    }
                //    else
                //    {
                //        _IDClass[property.Key] = property.Value;
                //        _writer.WriteAttributeString(property.Key, property.Value);
                //    }
                //}

            }
            //_writer.WriteEndElement(); //End RootParagraphStyleGroup
        }

        private void DeleteRelativeInFootnote(KeyValuePair<string, Dictionary<string, string>> cssClass)
        {
            if (cssClass.Key.IndexOf("..footnote-cal") > 0 || cssClass.Key.IndexOf("..footnote-marker") > 0)
            {
                ArrayList removeProperty = new ArrayList();
                foreach (KeyValuePair<string, string> property in _IDProperty)
                {
                    if (property.Value.IndexOf("%") > 0)
                    {
                        removeProperty.Add(property.Key);
                    }
                }
                foreach (string property in removeProperty)
                {
                    _IDProperty.Remove(property);
                }
            }
        }

        private void PositionProperty()
        {
            //Note: Paragraph Margins are not Completed.
            //Note: Currently "left" and "right" are added to padding because of Indesign 
            //Note: which does not have property for Paragraph Margin.
            //Note: Also It does not support the Negative Values in Margin. Everything restricted within the frames.

                if (_IDProperty.ContainsKey("position") && (_IDProperty.ContainsKey("left") || _IDProperty.ContainsKey("right")) )
                {
                    _IDProperty.Remove("position");

                    if (_IDProperty.ContainsKey("left"))
                    {
                        if (_IDProperty.ContainsKey("LeftIndent"))
                        {
                            _IDProperty["LeftIndent"] = (int.Parse(_IDProperty["LeftIndent"]) 
                                                        + int.Parse(_IDProperty["left"])).ToString();
                        }
                        else
                        {
                            _IDProperty["LeftIndent"] = _IDProperty["left"];
                        }
                    }
                    else if (_IDProperty.ContainsKey("right"))
                    {
                        if (_IDProperty.ContainsKey("RightIndent"))
                        {
                            _IDProperty["RightIndent"] = (int.Parse(_IDProperty["RightIndent"]) 
                                                         + int.Parse(_IDProperty["right"])).ToString();
                        }
                        else
                        {
                            _IDProperty["RightIndent"] = _IDProperty["right"];
                        }
                    }
                }

                if (_IDProperty.ContainsKey("position"))
                {
                    _IDProperty.Remove("position");
                }
                if (_IDProperty.ContainsKey("left"))
                {
                    _IDProperty.Remove("left");
                }
                if (_IDProperty.ContainsKey("right"))
                {
                    _IDProperty.Remove("right");
                }
        }

        private void CreateParagraphProperty(string propertyName, string propertyType)
        {
            if (_IDProperty.ContainsKey(propertyName))
            {
                _writer.WriteStartElement(propertyName);
                _writer.WriteAttributeString("type", propertyType);
                _writer.WriteString(_IDProperty[propertyName]);
                _writer.WriteEndElement();
            }
        }

        private void InsertBackgroundColor(string propertyValue)
        {
            _IDClass["StrokeWeight"] = "1";
            _IDClass["StrokeColor"] = propertyValue;
            _IDClass["EndJoin"] = "BevelEndJoin";

            _writer.WriteAttributeString("StrokeWeight", "1");
            _writer.WriteAttributeString("StrokeColor", propertyValue);
            _writer.WriteAttributeString("EndJoin", "BevelEndJoin");
        }

        /// <summary>
        /// Increase font-size 250% for Subscript and Superscript
        /// </summary>
        /// <param name="isIncrease">to increase font-size even the property is not super/sub script</param>
        private void SuperscriptSubscriptIncreaseFontSize(bool isIncrease)
        {
            bool isSuperSub = false;
            if (_IDProperty.ContainsKey("Position") && (_IDProperty["Position"] == "Subscript" || _IDProperty["Position"] == "Superscript"))
            {
                isSuperSub = true;
            }

            if (isSuperSub || isIncrease) // increase font-size for superscipt & subscript
            {
                string newValue = "100%";
                if (_IDProperty.ContainsKey("PointSize"))
                {
                    string fontValue = _IDProperty["PointSize"];
                    int counter;
                    string retValue = Common.GetNumericChar(fontValue, out counter);
                    if (retValue.Length > 0)
                    {
                        float value = float.Parse(retValue) * 1.0F;
                        string unit = fontValue.Substring(counter);
                        newValue = value + unit;
                    }
                    else
                    {
                        if (fontValue == "larger" || fontValue == "smaller")
                        {
                            newValue = fontValue;
                        }
                    }
                }
                _IDProperty["PointSize"] = newValue;
            }
        }

    }
}