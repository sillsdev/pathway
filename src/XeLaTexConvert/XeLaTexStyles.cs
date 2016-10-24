// --------------------------------------------------------------------------------------------
// <copyright file="XeLaTexStyles.cs" from='2009' to='2014' company='SIL International'>
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class XeLaTexStyles : XeLaTexStyleBase
    {
        #region Private Variables

        Dictionary<string, Dictionary<string, string>> _cssProperty = new Dictionary<string, Dictionary<string, string>>();
        XeLaTexMapProperty mapProperty = new XeLaTexMapProperty();
        private List<string> _inlineStyle;
        private List<string> _inlineText;
        private List<string> _includePackageList;
        Dictionary<string, List<string>> _classInlineStyle = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>> _classInlineText = new Dictionary<string, List<string>>();
        protected bool IsMirrored = false;
        public StringBuilder PageStyle = new StringBuilder();
        readonly Dictionary<string, string> _pageStyleFormat = new Dictionary<string, string>();
        private Dictionary<string, string> _langFontDictionary = new Dictionary<string, string>();
        private bool _isHeaderRule = false, _isFooterRule = false;

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
                _cssProperty = cssProperty;
                _projInfo = projInfo;
                LoadPageStyleFormat();
                CreatePageStyle();
                CreateStyle();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
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
                if (_cssProperty.ContainsKey("@page-bottom-center") && _cssProperty["@page-bottom-center"]["content"] == "|counter(page)|")
                {
                    _pageStyleFormat.Add("@page-bottom-center", "\\cfoot");
                }
                _pageStyleFormat.Add("@page-bottom-right", "\\rfoot");
                _pageStyleFormat.Add("@page:first-top-left", "\\lhead");
                _pageStyleFormat.Add("@page:first-top-center", "\\chead");
                _pageStyleFormat.Add("@page:first-top-right", "\\rhead");
                _pageStyleFormat.Add("@page:first-bottom-left", "\\lfoot");
                //_pageStyleFormat.Add("@page:first-bottom-center", "\\cfoot");
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
            //PageStyle.AppendLine("\\renewcommand{\\headrulewidth}{0.4pt} \r\n \\renewcommand{\\footrulewidth}{0.4pt} \r\n");
            PageStyle.AppendLine("\\renewcommand{\\thefootnote}{\\alphalph{\\value{footnote}}} \r\n");
            
        }

        private void SetPageHeaderFooter(string pageName)
        {
            string[] searchKey = { "top-left", "top-center", "top-right", "bottom-left", "bottom-center", "bottom-right" };
            for (int i = 0; i < 6; i++)
            {
                string currentPagePosition = pageName + "-" + searchKey[i];   // Ex: page:first-topleft
                if (_cssProperty.ContainsKey(currentPagePosition))
                {
                    if (!_isHeaderRule && i <= 2)
                    {
                        PageStyle.AppendLine("\\renewcommand{\\headrulewidth}{0.4pt} \r\n");
                        _isHeaderRule = true;
                    }
                    else if (!_isFooterRule && i >= 3 && pageName != "@page:first" && _cssProperty[currentPagePosition]["content"].Trim().Length > 2)
                    {
                        PageStyle.AppendLine("\\renewcommand{\\footrulewidth}{0.4pt} \r\n");
                        _isFooterRule = true;
                    }
                    Dictionary<string, string> cssProp = _cssProperty[currentPagePosition];
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
                                if (!_cssProperty.ContainsKey("@page:none-none"))
                                {
                                    PageStyle.AppendLine(_pageStyleFormat[currentPagePosition] + "{\\thepage}");
                                }
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
                            _projInfo.HeaderReferenceFormat = para.Value;
                        }
                    }
                }
            }
        }

        private void CreatePageFirstPage()
        {
            string pageName = "@page";
            ProcessPageProperty(pageName);

            pageName = "@page:first";
            ProcessPageProperty(pageName);

            pageName = "@page:left";
            LeftPageLayoutProperty = ProcessPageProperty(pageName);

            pageName = "@page:right";
            RightPageLayoutProperty = ProcessPageProperty(pageName);

            pageName = "@page-footnotes";
            ProcessPageProperty(pageName);
        }

        private Dictionary<string, string> ProcessPageProperty(string pageName)
        {
            Dictionary<string, string> pageLayoutProperty = new Dictionary<string, string>();
            if (_cssProperty.ContainsKey(pageName))
            {
                Dictionary<string, string> cssClassProperty = _cssProperty[pageName];

                if (cssClassProperty.ContainsKey("-ps-custom-footnote-caller"))
                {
                    foreach (KeyValuePair<string, string> para in cssClassProperty)
                    {
                        if (para.Key == "-ps-custom-footnote-caller" && para.Value != string.Empty)
                        {
                            PageStyle.AppendLine("\\renewcommand{\\thefootnote}{\\textit{\\"+ para.Value +"{}\\value{footnote}}} \r\n");
                        }
                    }
                }
            }
            return pageLayoutProperty;
        }

        private void CreateStyle()
        {
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
                mapProperty.XeLaTexProperty(cssClass.Value, replaceNumberInStyle, _inlineStyle, _includePackageList, _inlineText, LangFontDictionary);
                
                _classInlineStyle[replaceNumberInStyle] = _inlineStyle;
                if (_inlineText.Count > 0)
                    _classInlineText[replaceNumberInStyle] = _inlineText;
            }
        }
    }
}