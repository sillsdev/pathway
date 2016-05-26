// --------------------------------------------------------------------------------------------
// <copyright file="XeLaTexMapProperty.cs" from='2009' to='2014' company='SIL International'>
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
using System.Drawing;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class XeLaTexMapProperty
    {
        private readonly Dictionary<string, string> _idProperty = new Dictionary<string, string>();
        private bool _isKeepLineWrittern = false;
        private bool _isLinux = false;
        private string _className;
        private string _fontName;
        private readonly List<string> _fontOption = new List<string>();
        private readonly List<string> _fontStyle = new List<string>();
        private string _fontSize;
        private List<string> _inlineStyle;
        private List<string> _includePackageList;
        private List<string> _inlineInnerStyle;
        private Dictionary<string, string> _langFontDictionary = new Dictionary<string, string>();

        //TextInfo _titleCase = CultureInfo.CurrentCulture.TextInfo;
        public string XeLaTexProperty(Dictionary<string, string> cssProperty, string className, List<string> inlineStyle, List<string> includePackageList, List<string> inlineInnerStyle, Dictionary<string, string> langFontDictionary)
        {
            _langFontDictionary = langFontDictionary;
            _isLinux = Common.IsUnixOS();
            Initialize(className, cssProperty, inlineStyle, includePackageList, inlineInnerStyle);
            foreach (KeyValuePair<string, string> property in cssProperty)
            {
                string propertyValue = PercentageToEm(property.Value);

                switch (property.Key.ToLower())
                {
                    case "font-weight":
                    case "font-style":
                        FontWeight(cssProperty);
                        break;
                    case "text-align":
                        TextAlign(propertyValue);
                        break;
                    case "font-size":
                        FontSize(propertyValue);
                        break;
                    case "text-decoration":
					case "border-bottom":
					case "border-bottom-style":
                    case "class-text-decoration":
                        TextDecoration(propertyValue);
                        break;
                    case "font-variant":
                        FontVariant(propertyValue);
                        break;
                    case "text-indent":
                        TextIndent(propertyValue, className, cssProperty);
                        break;
                    case "margin-left":
                    case "class-margin-left":
                        MarginLeft(propertyValue);
                        break;
                    case "margin-right":
                    case "class-margin-right":
                        MarginRight(propertyValue);
                        break;
                    case "margin-top":
                    case "class-margin-top":
                        MarginTop(propertyValue);
                        break;
                    case "margin-bottom":
                    case "class-margin-bottom":
                        MarginBottom(propertyValue);
                        break;
                    case "font-family":
                        FontFamily(propertyValue);
                        break;
                    case "page-width":
                        PageWidth(propertyValue);
                        break;
                    case "page-height":
                        PageHeight(propertyValue);
                        break;
                    case "padding-left":
                        PaddingLeft(propertyValue);
                        break;
                    case "padding-right":
                        PaddingRight(propertyValue);
                        break;
                    case "padding-top":
                        PaddingTop(propertyValue);
                        break;
                    case "padding-bottom":
                        PaddingBottom(propertyValue);
                        break;
                    case "color":
                        Color(propertyValue);
                        break;
                    case "background-color":
                        BGColor(propertyValue);
                        break;
                    case "column-count":
                        ColumnCount(propertyValue);
                        break;
                    case "display":
                        Display(propertyValue);
                        break;
                    case "text-transform":
                        TextTransform(propertyValue);
                        break;
                    case "vertical-align":
                        VerticalAlign(propertyValue);
                        break;
                    case "line-height":
                        LineHeight(propertyValue);
                        break;
                    case "letter-spacing":
                        LetterSpacing(propertyValue);
                        break;
                    case "word-spacing":
                        WordSpacing(propertyValue);
                        break;
                    case "visibility":
                        Visibility(propertyValue);
                        break;
                    case "orphans":
                        Orphans(propertyValue);
                        break;
                    case "widows":
                        Widows(propertyValue);
                        break;
                    case "marks":
                        Marks(propertyValue);
                        break;
                    case "direction":
                        Direction(propertyValue);
                        break;
                }
            }
            string style = ComposeStyle();
            return style;
        }

        private void Visibility(string propertyValue)
        {
            if (propertyValue == string.Empty || propertyValue == "visible")
            {
                return;
            }

            if (propertyValue.ToLower() == "hidden")
            {
                propertyValue = "\\censor";
                if (!_includePackageList.Contains("\\usepackage{censor}"))
                    _includePackageList.Add("\\usepackage{censor}");
            }

            if (propertyValue.Trim().Length > 0)
                _inlineInnerStyle.Add(propertyValue);

        }

        private void Initialize(string className, Dictionary<string, string> cssProperty, List<string> inlineStyle, List<string> includePackageList, List<string> inlineText)
        {
            _className = className;
            _inlineStyle = inlineStyle;
            _includePackageList = includePackageList;
            _inlineInnerStyle = inlineText;
            if (_langFontDictionary.Count > 0)
            {
                foreach (var selectFontName in _langFontDictionary)
                {
                    if (selectFontName.Key.ToLower() == "fontname" || selectFontName.Key.ToLower() == "en")
                    {
                        _fontName = selectFontName.Value;
                        break;
                    }
                }
            }
            else
            {
                _fontName = string.Empty;
            }

            if (_isLinux)
            {
                SetFontFamily();
            }
            else
            {
                if (string.IsNullOrEmpty(_fontName))
                    _fontName = "Times New Roman";
            }

            foreach (KeyValuePair<string, string> property in cssProperty)
            {
                if (property.Key.ToLower() == "font-family")
                {
                    _fontName = FontFamily(property.Value);
                    break;
                }
            }

            _fontOption.Clear();
            _fontStyle.Clear();
            _fontSize = " at 12pt";
        }

        private void SetFontFamily()
        {
            string fontName;
            fontName = "Charis SIL";
            if (_langFontDictionary.Count != 0)
            {
                string[] splitClassNameMeta = _className.Split('.');
                if (splitClassNameMeta.Length > 1)
                {
                    splitClassNameMeta[1] = "." + splitClassNameMeta[1];
                    foreach (string langCode in _langFontDictionary.Keys)
                    {
                        if (splitClassNameMeta[1].Contains("." + langCode))
                        {
                            fontName = _langFontDictionary[langCode];
                            _fontName = fontName;
                            break;
                        }
                    }
                }
            }

            if (_fontName != null)
            {
                FontFamily[] systemFontList = System.Drawing.FontFamily.Families;
                foreach (FontFamily systemFont in systemFontList)
                {
                    if (_fontName.ToLower() == systemFont.Name.ToLower())
                    {
                        fontName = _fontName;
                        break;
                    }
                }
            }
            _fontName = fontName;
        }

        private string ComposeStyle()
        {
            string style = string.Empty;
            _className = Common.ReplaceSeperators(_className);
            if (_className == "@page")
            {
                //cmyk 0.1 0.9 0.5 0
                if (_idProperty.ContainsKey("backgroundColor"))
                    style += @"\special{background cmyk " + _idProperty["backgroundColor"] + "}";
            }
            else
            {
                style = @"\font\" + _className + "=\"" + _fontName;
                foreach (string sty in _fontOption)
                {
                    style += sty;
                }
                style += "\"";

                foreach (string sty in _fontStyle)
                {
                    style += sty;
                }


                if (_fontSize.Length > 0)
                {
                    style += _fontSize;
                }
            }
            return style;
        }

        private void Widows(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            propertyValue = Common.SetPropertyValue("widows", propertyValue);
            _inlineStyle.Add(propertyValue);
            _idProperty["KeepLastLines"] = propertyValue;
            AddKeepLinesTogetherProperty();
        }

        private void Orphans(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            propertyValue = Common.SetPropertyValue("orphans", propertyValue);
            _inlineStyle.Add(propertyValue);
            _idProperty["KeepFirstLines"] = propertyValue;
            AddKeepLinesTogetherProperty();
        }

        private void AddKeepLinesTogetherProperty()
        {
            if (_isKeepLineWrittern == false)
            {
                _idProperty["KeepLinesTogether"] = "true";
            }
            _isKeepLineWrittern = true;
        }

        public void WordSpacing(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            propertyValue = Common.SetPropertyValue("\\spaceskip", propertyValue);
            _inlineStyle.Add(propertyValue);
            _idProperty["WordSpacing"] = propertyValue;
        }

        public void LetterSpacing(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }

            string space = ":letterspace=" + propertyValue;
            _fontStyle.Add(space);

        }

        public void Marks(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }

            if (propertyValue == "crop")
            {
                if (!_includePackageList.Contains("\\usepackage[croplength=10mm,cropgap=3mm, cropmarks]{zwpagelayout}"))
                    _includePackageList.Add("\\usepackage[croplength=10mm,cropgap=3mm, cropmarks]{zwpagelayout}");
            }
        }

        public void LineHeight(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }

            if (propertyValue.IndexOf("em") > 0)
            {
                propertyValue = propertyValue.Replace("em", "");
                double decimalValue = Convert.ToDouble(propertyValue) + 0.5;
                propertyValue = Convert.ToDouble(decimalValue).ToString();
            }
            else
            {
                propertyValue = propertyValue.Replace("pt", "");
                propertyValue = Convert.ToString(Convert.ToDouble(propertyValue) / 24);
            }
            propertyValue = Common.SetPropertyValue("line-height", propertyValue);
            propertyValue = propertyValue.Replace("pt", "");


            _idProperty["line-height"] = propertyValue;
            _inlineStyle.Add(propertyValue);

            propertyValue = "\\usepackage{setspace}";
            if (!_includePackageList.Contains(propertyValue))
                _includePackageList.Add(propertyValue);
        }
        public void VerticalAlign(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }

            if (propertyValue.ToLower() == "super")
            {
                propertyValue = "$^{";
            }
            else if (propertyValue.ToLower() == "sub")
            {
                propertyValue = "$_{";
            }
            else if (propertyValue.ToLower() == "baseline")
            {
                propertyValue = "";
            }
            else
            {
                propertyValue = "";
            }
            if (propertyValue.Trim().Length > 0)
                _inlineInnerStyle.Add(propertyValue);

        }
        public void TextTransform(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            if (propertyValue.ToLower() == "uppercase")
            {
                propertyValue = "\\uppercase";
            }
            else if (propertyValue.ToLower() == "lowercase")
            {
                propertyValue = "\\lowercase";
            }
            else if (propertyValue.ToLower() == "capitalize")
            {
                propertyValue = "\\textsc";
            }
            _inlineStyle.Add(propertyValue);
        }
        public void PaddingLeft(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }

            _idProperty["LeftIndent"] = propertyValue;
            propertyValue = Common.SetPropertyValue("padding-left", propertyValue);
            _idProperty["padding-left"] = propertyValue;
            _inlineStyle.Add(propertyValue);

            propertyValue = "\\usepackage{changepage}";
            if (!_includePackageList.Contains(propertyValue))
                _includePackageList.Add(propertyValue);
        }
        public void PaddingRight(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }

            propertyValue = Common.SetPropertyValue("padding-right", propertyValue);
            _idProperty["padding-right"] = propertyValue;
            _inlineStyle.Add(propertyValue);

            propertyValue = "\\usepackage{changepage}";
            if (!_includePackageList.Contains(propertyValue))
                _includePackageList.Add(propertyValue);
        }
        public void PaddingTop(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }

            _idProperty["SpaceBefore"] = propertyValue;
            propertyValue = Common.SetPropertyValue("padding-top", propertyValue);
            _idProperty["padding-top"] = propertyValue;
            _inlineStyle.Add(propertyValue);

            propertyValue = "\\usepackage{changepage}";
            if (!_includePackageList.Contains(propertyValue))
                _includePackageList.Add(propertyValue);
        }
        public void PaddingBottom(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }

            _idProperty["SpaceAfter"] = propertyValue;
            propertyValue = Common.SetPropertyValue("padding-bottom", propertyValue);
            _idProperty["padding-bottom"] = propertyValue;
            _inlineStyle.Add(propertyValue);

            propertyValue = "\\usepackage{changepage}";
            if (!_includePackageList.Contains(propertyValue))
                _includePackageList.Add(propertyValue);
        }
        public void PageHeight(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _idProperty["Page-Height"] = propertyValue;
        }
        public void PageWidth(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _idProperty["Page-Width"] = propertyValue;
        }
        public void MarginLeft(string propertyValue)
        {
            //if (propertyValue == string.Empty)
            //{
            //    return;
            //}
            //_IDProperty["Margin-Left"] = propertyValue;

            //propertyValue = Common.SetPropertyValue("\\leftmargin", propertyValue);

            ////propertyValue = Common.SetPropertyValue("margin leftmargin=", propertyValue);
            //_IDProperty["margin-left"] = propertyValue;
            //_inlineStyle.Add(propertyValue);

            //propertyValue = "\\usepackage{changepage}";
            //if (!_includePackageList.Contains(propertyValue))
            //    _includePackageList.Add(propertyValue);
        }
        public void MarginRight(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }

            _idProperty["Margin-Right"] = propertyValue;

            propertyValue = Common.SetPropertyValue("\\rightmargin", propertyValue);

            _idProperty["margin-right"] = propertyValue;
            _inlineStyle.Add(propertyValue);

            propertyValue = "\\usepackage{changepage}";
            if (!_includePackageList.Contains(propertyValue))
                _includePackageList.Add(propertyValue);

        }
        public void MarginTop(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _idProperty["Margin-Top"] = propertyValue;

            propertyValue = Common.SetPropertyValue("\\topskip", propertyValue);

            _idProperty["margin-top"] = propertyValue;
            _inlineStyle.Add(propertyValue);

            propertyValue = "\\usepackage{changepage}";
            if (!_includePackageList.Contains(propertyValue))
                _includePackageList.Add(propertyValue);
        }
        public void MarginBottom(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _idProperty["Margin-Bottom"] = propertyValue;
            //propertyValue = Common.SetPropertyValue("\\needspace\\baselineskip","");//propertyValue
            //\section*{\needspace {3\baselineskip}{\topskip 18pt{\letterhiletHeaddicBody{र sam}}}}
            propertyValue = "\\section*{\\needspace {8\\baselineskip}";
            _idProperty["margin-bottom"] = propertyValue;
            _inlineStyle.Add(propertyValue);

            propertyValue = "\\usepackage{needspace}";
            if (!_includePackageList.Contains(propertyValue))
                _includePackageList.Add(propertyValue);
        }
        public string FontFamily(string propertyValue)
        {
            string fontName = "Times New Roman";

            if (_isLinux)
                fontName = "Charis SIL";

            if (_langFontDictionary.Count != 0)
            {
                string[] splitClassNameMeta = _className.Split('.');
                if (splitClassNameMeta.Length > 1)
                {
                    splitClassNameMeta[1] = "." + splitClassNameMeta[1];
                    foreach (string langCode in _langFontDictionary.Keys)
                    {
                        if (splitClassNameMeta[1].Contains("." + langCode))
                        {
                            fontName = _langFontDictionary[langCode];
                            propertyValue = fontName;
                            break;
                        }
                    }
                }
            }

            FontFamily[] systemFontList = System.Drawing.FontFamily.Families;
            foreach (FontFamily systemFont in systemFontList)
            {
				if (propertyValue.ToLower() == systemFont.Name.ToLower())
                {
					fontName = systemFont.Name;
                    break;
                }
            }

            _fontName = fontName;
            return _fontName;
        }

        public void TextIndent(string propertyValue, string className, Dictionary<string, string> cssProperty)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }

            if (propertyValue != "0" && (className == "entry" || className.Contains("IntroList") || (className.IndexOf("Line") == 0)))
            {
                propertyValue = propertyValue.Replace("-", "");

                if (cssProperty.ContainsKey("class-margin-left"))
                {
                    propertyValue = cssProperty["class-margin-left"] + "pt";
                }

                propertyValue = "text-indent " + propertyValue;
            }

            if (propertyValue == "0")
            {
                return;
            }

            if (propertyValue.IndexOf("text-indent") == 0)
            {
                _inlineStyle.Add(propertyValue);
            }
        }
        public void Color(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            string color = ":color=" + propertyValue.Replace("#", "");
            _fontStyle.Add(color);
        }
        public void BGColor(string propertyValue)
        {
            if (propertyValue == "transparent" || propertyValue.Contains("inherit"))
                return;

            if (propertyValue == string.Empty)
            {
                return;
            }

            string cVal = propertyValue.Replace("#", "");
            int red = int.Parse(cVal.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            int green = int.Parse(cVal.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            int blue = int.Parse(cVal.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);


            _idProperty["backgroundColor"] = Rgb2Cmyk(red, green, blue);

        }
        public void Display(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }

            if (propertyValue.ToLower() == "none")
            {
                propertyValue = "display-none %comment";
                _inlineStyle.Add(propertyValue);
            }
            else
            {
                return;
            }
            _idProperty["display"] = propertyValue;
            propertyValue = "\\usepackage{verbatim} ";
            if (!_includePackageList.Contains(propertyValue))
                _includePackageList.Add(propertyValue);
        }

        private void Direction(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }

            if (propertyValue.ToLower() == "rtl")
            {
                propertyValue = "RTL";
                _inlineStyle.Add(propertyValue);
            }
            _idProperty["direction"] = propertyValue;
            propertyValue = "\\usepackage{bidi} ";
            if (!_includePackageList.Contains(propertyValue))
                _includePackageList.Add(propertyValue);
        }

        public void ColumnCount(string propertyValue)
        {
            if (propertyValue == string.Empty || Common.ValidateAlphabets(propertyValue)
                || propertyValue.IndexOf('-') > -1)
            {
                _idProperty["TextColumnCount"] = "1";
            }
            else
            {
                _idProperty["TextColumnCount"] = propertyValue;
            }

            _idProperty["column-count"] = propertyValue;
            propertyValue = "column-count " + propertyValue;
            _idProperty["column-count"] = propertyValue;
            _inlineStyle.Add(propertyValue);

            propertyValue = "\\usepackage{multicol}";
            if (!_includePackageList.Contains(propertyValue))
                _includePackageList.Add(propertyValue);

        }

        public void FontVariant(string propertyValue)
        {
            if (propertyValue == string.Empty || propertyValue == "inherit")
            {
                return;
            }

            if (propertyValue == "normal")
            {
                propertyValue = "";
            }
            else if (propertyValue.ToLower() == "small-caps")
            {
                propertyValue = "\\textsc";
            }
            _idProperty["Capitalization"] = propertyValue;
            if (propertyValue.Trim().Length > 0)
                _inlineInnerStyle.Add(propertyValue);
        }
        public void TextDecoration(string propertyValue)
        {
			//another package we can use for dotted http://xpt.sourceforge.net/techdocs/language/latex/latex24-SpecialEffectsUnderlining/single/
            if (propertyValue == string.Empty || propertyValue == "inherit")
            {
                return;
            }
			if (propertyValue == "underline" || propertyValue == "double" || propertyValue == "solid")
            {
				propertyValue = "\\underline";
            }
			if (propertyValue == "underlineunderline")
			{
				propertyValue = "\\underline";
				if (propertyValue.Trim().Length > 0)
					_inlineInnerStyle.Add(propertyValue);
			}
            else if (propertyValue == "none")
            {
                propertyValue = "";
            }
            if (propertyValue.Trim().Length > 0)
				_inlineInnerStyle.Add(propertyValue);
        }

        public void FontWeight(Dictionary<string, string> cssProperty)
        {
            if (_idProperty.ContainsKey("FontStyle")) return;

            string propertyWeight = "";
            string propertyStyle = "";
            string propertyValue = "";
            if (cssProperty.ContainsKey("font-weight"))
            {
                propertyWeight = cssProperty["font-weight"];
            }

            if (cssProperty.ContainsKey("font-style"))
            {
                propertyStyle = cssProperty["font-style"];
            }
            string strValue = propertyWeight + propertyStyle;

            if (strValue == "boldnormal" || strValue == "bold" || strValue == "700")
            {
                propertyValue = "/B";
            }
            else if (strValue == "normalitalic" || strValue == "italic")
            {
                propertyValue = "/I";
            }
            else if (strValue == "bolditalic" || strValue == "700italic")
            {
                propertyValue = "/BI";
            }

            if (propertyValue == string.Empty)
            {
                return;
            }
            _idProperty["FontStyleBold"] = propertyValue;
            if (propertyValue.Trim().Length > 0 && !_fontOption.Contains(propertyValue))
                _fontOption.Add(propertyValue);
        }
        public void TextAlign(string propertyValue)
        {
            if (propertyValue == string.Empty || propertyValue == "inherit"
                || (_className.ToLower() == "entry" || _className.ToLower() == "subentry" || _className.ToLower() == "reventry"))
            {
                return;
            }

            if (propertyValue == "center")
            {
                propertyValue = "text-align center";
            }
            else if (propertyValue == "left")
            {
                propertyValue = "text-align raggedright";
            }
            else if (propertyValue == "right")
            {
                propertyValue = "text-align raggedleft";
            }
            if (propertyValue != "justify")
            {
                _idProperty["Justification"] = propertyValue;
                _inlineStyle.Add(propertyValue);
            }
        }

        public void FontSize(string propertyValue)
        {
            if (propertyValue == "larger" || propertyValue == "smaller")
            {
                _idProperty["PointSize"] = propertyValue;
            }
            else if (propertyValue == string.Empty || Common.ValidateAlphabets(propertyValue)
                || propertyValue.IndexOf('-') > -1)
            {
                return;
            }
            _idProperty["PointSize"] = propertyValue;

            if (propertyValue == "larger")
                _fontSize = " at 20pt";
            else if (propertyValue == "smaller")
                _fontSize = " at 10pt";
            else
                _fontSize = " at " + Common.SetPropertyValue(string.Empty, propertyValue);
        }

        private string Rgb2Cmyk(int r, int g, int b)
        {
            float computedC = 0;
            float computedM = 0;
            float computedY = 0;
            float computedK = 0;


            // BLACK
            if (r == 0 && g == 0 && b == 0)
            {
                return "0 0 0 1";
            }

            computedC = 1 - (r / 255);
            computedM = 1 - (g / 255);
            computedY = 1 - (b / 255);

            var minCMY = Math.Min(computedC, Math.Min(computedM, computedY));

            computedC = (computedC - minCMY) / (1 - minCMY);
            computedM = (computedM - minCMY) / (1 - minCMY);
            computedY = (computedY - minCMY) / (1 - minCMY);
            computedK = minCMY;
            string cmyk = computedC + " " + computedM + " " + computedY + " " + computedK;
            return cmyk;
        }
        private string PercentageToEm(string propertyValue)
        {
            if (propertyValue.IndexOf("%") > 0)
            {
                propertyValue = propertyValue.Replace("%", "");
                double numericValue = Convert.ToDouble(propertyValue);
                numericValue = numericValue / 100;
                propertyValue = numericValue + "em";
            }
            return propertyValue;
        }
    }
}