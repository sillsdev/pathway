using System;
using System.Collections.Generic;
using System.Drawing;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class XeLaTexMapProperty
    {
        private Dictionary<string, string> _IDProperty = new Dictionary<string, string>();
        private string _property; 
        private Dictionary<string, string> _cssProperty = new Dictionary<string, string>();
        private bool _IsKeepLineWrittern = false;

        private string _className;

        private string _fontName;
        private List<string> _fontOption = new List<string>();
        private List<string> _fontStyle = new List<string>();
        private string _fontSize;
        private List<string> _inlineStyle;
        private List<string> _includePackageList;


        //TextInfo _titleCase = CultureInfo.CurrentCulture.TextInfo;
        public string XeLaTexProperty(Dictionary<string, string> cssProperty, string className, List<string> inlineStyle, List<string> includePackageList)
        {
            Initialize(className, cssProperty, inlineStyle, includePackageList);
            foreach (KeyValuePair<string, string> property in cssProperty)
            {
                switch (property.Key.ToLower())
                {
                    case "font-weight":
                    case "font-style":
                        FontWeight(cssProperty);
                        break;
                    case "text-align":
                        TextAlign(property.Value);
                        break;
                    case "font-size":
                        FontSize(property.Value);
                        break;
                    case "text-decoration":
                    case "class-text-decoration":
                        TextDecoration(property.Value);
                        break;
                    case "font-variant":
                        FontVariant(property.Value);
                        break;
                    case "text-indent":
                        TextIndent(property.Value);
                        break;
                    case "margin-left":
                    case "class-margin-left":
                        MarginLeft(property.Value);
                        break;
                    case "margin-right":
                    case "class-margin-right":
                        MarginRight(property.Value);
                        break;
                    case "margin-top":
                    case "class-margin-top":
                        MarginTop(property.Value);
                        break;
                    //case "margin-bottom":
                    //    MarginBottom(property.Value);
                    //    break;
                    case "font-family":
                        FontFamily(property.Value);
                        break;
                        //\special{papersize=5in,8in}
                    case "page-width":
                        PageWidth(property.Value);
                        break;
                    case "page-height":
                        PageHeight(property.Value);
                        break;
                    //case "mirror":
                    //    Mirror(property.Value);
                    //    break;
                    //case "padding-left":
                    //case "class-margin-left":
                    //    PaddingLeft(property.Value);
                    //    break;
                    //case "padding-right":
                    //case "class-margin-right":
                    //    PaddingRight(property.Value);
                    //    break;
                    //case "padding-top":
                    //case "class-margin-top":
                    //    PaddingTop(property.Value);
                    //    break;
                    //case "padding-bottom":
                    //case "class-margin-bottom":
                    //    PaddingBottom(property.Value);
                    //    break;
                    //case "padding":
                    //case "margin":
                    //    //Margin(styleAttributeInfo);
                    //    break;
                    case "color":
                        Color(property.Value);
                        break;
                    case "background-color":
                        BGColor(property.Value);
                        break;
                    //case "size":
                    //    //Size(styleAttributeInfo);
                    //    break;
                    //case "language":
                    //    //Language(styleAttributeInfo);
                    //    break;
                    //case "border":
                    //    //Border(styleAttributeInfo);
                    //    break;
                    //case "column-count":
                    //    ColumnCount(property.Value);
                    //    break;
                    //case "column-gap":
                    //    ColumnGap(property.Value);
                    //    break;
                    //case "display":
                    //    Display(property.Value);
                    //    break;
                    //case "page-break-before":
                    //    PageBreakBefore(property.Value);
                    //    break;
                    case "text-transform":
                        TextTransform(property.Value);
                        break;
                    case "vertical-align":
                        VerticalAlign(property.Value);
                        break;
                    case "line-height":
                        LineHeight(property.Value);
                        break;
                    //case "hyphens":
                    //    Hyphens(property.Value);
                    //    break;
                    //case "hyphenate-before":
                    //    HyphenateBefore(property.Value);
                    //    break;
                    //case "hyphenate-after":
                    //    HyphenateAfter(property.Value);
                    //    break;
                    //case "hyphenate-lines":
                    //    HyphenateLines(property.Value);
                    //    break;
                    case "letter-spacing":
                        LetterSpacing(property.Value);
                        break;
                    //case "word-spacing":
                    //    WordSpacing(property.Value);
                    //    break;
                    //case "orphans":
                    //    Orphans(property.Value);
                    //    break;
                    //case "widows":
                    //    Widows(property.Value);
                    //    break;
                    //case "direction":
                    //    Direction(property.Value);
                    //    break;
                    //case "-ps-vertical-justification":
                    //    VerticalJustification(property.Value);
                    //    break;
                    case "marks":
                        Marks(property.Value);
                        break;
                    //default:
                    //    SimpleProperty(property);
                    //    break;
                }
            }
            string  style = ComposeStyle();
            return style;
        }

        public string XeLaTexPageProperty(Dictionary<string, string> cssProperty, string className, List<string> inlineStyle, List<string> includePackageList)
        {
            Initialize(className, cssProperty, inlineStyle, includePackageList);
            foreach (KeyValuePair<string, string> property in cssProperty)
            {
                switch (property.Key.ToLower())
                {
                    case "marks":
                        Marks(property.Value);
                        break;
                    //default:
                    //    SimpleProperty(property);
                    //    break;
                }
            }
            string style = ComposeStyle();
            return style;
        }

        private void Initialize(string className, Dictionary<string, string> cssProperty, List<string> inlineStyle, List<string> includePackageList)
        {
            _className = className;
            _property = string.Empty;
            _cssProperty = cssProperty;
            _inlineStyle = inlineStyle;
            _includePackageList = includePackageList;
            
            if (className.IndexOf("scrBookName") == -1)
                _fontName = "Times New Roman";
            
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

        private string ComposeStyle()
        {
            //string style = @"\font\" + _className + "=\"" + propertyValue + "\"";
            string style = string.Empty;
            _className = Common.ReplaceSeperators(_className);
            if (_className == "@page")
            {
                //\special{papersize=5in,8in}
                //style = @"\special{papersize=" + _IDProperty["Page-Height"] + "pt ," + _IDProperty["Page-Width"] + "pt} \\r\\n";
                //cmyk 0.1 0.9 0.5 0
                if(_IDProperty.ContainsKey("backgroundColor"))
                    style += @"\special{background cmyk " + _IDProperty["backgroundColor"] + "}";
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

        public void VerticalJustification(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            string value = propertyValue;
            if (propertyValue == "Top")
            {
                value = "TopAlign";
            }
            else if (propertyValue == "Center")
            {
                value = "CenterAlign";
            }
            else if (propertyValue == "Bottom")
            {
                value = "BottomAlign";
            }
            _IDProperty["VerticalJustification"] = value;
        }

        public void Direction(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            if (propertyValue == "rtl")
            {
                _IDProperty["Composer"] = "HL Composer Optyca";
                _IDProperty["DigitsType"] = "ArabicDigits";
                _IDProperty["CharacterDirection"] = "RightToLeftDirection";
                _IDProperty["ParagraphDirection"] = "RightToLeftDirection";
                _IDProperty["ParagraphJustification"] = "ArabicJustification";
                _IDProperty["Justification"] = "RightAlign";
            }
            //_IDProperty["Composer"] = "HL Composer Optyca";
            //_IDProperty["DigitsType"] = "DefaultDigits";
            //_IDProperty["CharacterDirection"] = "LeftToRightDirection";
            //_IDProperty["ParagraphDirection"] = "LeftToRightDirection";
            //_IDProperty["ParagraphJustification"] = "DefaultJustification";
            //_IDProperty["Justification"] = "LeftAlign";
        }

        private void Widows(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["KeepLastLines"] = propertyValue;
            AddKeepLinesTogetherProperty();
        }

        private void Orphans(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["KeepFirstLines"] = propertyValue;
            AddKeepLinesTogetherProperty();
        }

        private void AddKeepLinesTogetherProperty()
        {
            if (_IsKeepLineWrittern == false)
            {
                _IDProperty["KeepLinesTogether"] = "true";
            }
            _IsKeepLineWrittern = true;
        }

        public void WordSpacing(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["MinimumWordSpacing"] = "0";
            _IDProperty["DesiredWordSpacing"] = propertyValue;
            _IDProperty["MaximumWordSpacing"] = propertyValue;
        }
        public void LetterSpacing(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            //_IDProperty["MinimumLetterSpacing"] = "0";
            //_IDProperty["DesiredLetterSpacing"] = propertyValue;
            //_IDProperty["MaximumLetterSpacing"] = propertyValue;

            string space = ":letterspace=" + propertyValue;
            _fontStyle.Add(space);

        }


        public void HyphenateLines(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["HyphenateLadderLimit"] = propertyValue;
        }
        public void HyphenateAfter(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["HyphenateAfterFirst"] = propertyValue;
        }
        public void HyphenateBefore(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["HyphenateBeforeLast"] = propertyValue;
        }
        public void Hyphens(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            string value = propertyValue == "none" ? "false" : "true";
            _IDProperty["Hyphenation"] = value;
        }
        public void SimpleProperty(KeyValuePair<string, string> property)
        {
            string value = property.Value;
            switch (property.Key.ToLower())
            {
                case "float":
                case "clear":
                case "white-space":
                case "counter-increment":
                case "counter-reset":
                case "content":
                case "position":
                case "left":
                case "right":
                case "width":
                case "height":
                case "visibility":
                case "prince-text-replace":
                    _IDProperty[property.Key] = value;
                    break;
                default:
                    //throw new Exception("Not a valid CSS Command");
                    break;
            }
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

            if (propertyValue == "100%")
            {
                propertyValue = "\\baselineskip=0pt";
            }
            else
            {
                propertyValue = "\\baselineskip=" + propertyValue + "pt";
            }
            
            _inlineStyle.Add(propertyValue);
            _IDProperty["Leading"] = propertyValue;
        }
        public void VerticalAlign(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }

            if (propertyValue.ToLower() == "super")
            {
                propertyValue = "^";
            }
            else if (propertyValue.ToLower() == "sub")
            {
                propertyValue = "_";
            }
            else if (propertyValue.ToLower() == "baseline")
            {
                propertyValue = "";
            }

            if (propertyValue.Trim().Length > 0)
                _inlineStyle.Add(propertyValue);

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
        public void PageBreakBefore(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["PageBreakBefore"] = propertyValue;
        }
        public void PaddingLeft(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["LeftIndent"] = propertyValue;
        }
        public void PaddingRight(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["RightIndent"] = propertyValue;
        }
        public void PaddingTop(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["SpaceBefore"] = propertyValue;
        }
        public void PaddingBottom(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["SpaceAfter"] = propertyValue;
        }
        public void Mirror(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["FacingPages"] = propertyValue;
        }
        public void PageHeight(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["Page-Height"] = propertyValue;
        }
        public void PageWidth(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["Page-Width"] = propertyValue;
        }
        public void MarginLeft(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            propertyValue = "\\leftskip " + propertyValue + "pt";
            _IDProperty["Margin-Left"] = propertyValue;
            _inlineStyle.Add(propertyValue);
        }
        public void MarginRight(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            propertyValue = "\\rightskip=" + propertyValue + "pt";
            _IDProperty["Margin-Right"] = propertyValue;
            _inlineStyle.Add(propertyValue);
        }
        public void MarginTop(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            propertyValue = "\\topskip=" + propertyValue + "pt";
            _inlineStyle.Add(propertyValue);
            _IDProperty["Margin-Top"] = propertyValue;
        }
        public void MarginBottom(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["Margin-Bottom"] = propertyValue;
        }
        public string FontFamily(string propertyValue)
        {
            string fontName = "Times New Roman";
            FontFamily[] systemFontList = System.Drawing.FontFamily.Families;
            foreach (FontFamily systemFont in systemFontList)
            {
                if (propertyValue.ToLower() == systemFont.Name.ToLower())
                {
                    fontName = propertyValue;
                    break;
                }
            }

            _fontName = fontName;

            return _fontName;
            //_property = @"\font\" + _className + "=\"" + propertyValue + "\"";
            //_property += GetFontSize();
            // _IDProperty["AppliedFont"] = propertyValue;
        }

        public string GetFontSize()
        {
            string fontSize = string.Empty;  
            if (_cssProperty.ContainsKey("font-size"))
            {
                fontSize = " at " + _cssProperty["font-size"] + "pt";
            }
            return fontSize;
        }

        public void TextIndent(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            if (propertyValue == "0")
            {
                propertyValue = "\\noindent";
            }
            else
            {
                propertyValue = "\\parindent=" + propertyValue + "pt";
            }
            //_IDProperty["FirstLineIndent"] = propertyValue;
            _inlineStyle.Add(propertyValue);
        }
        public void Color(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return; 
            }
            //string color = "textcolor[RGB]" + propertyValue.Replace("#", "");   //:color=880000
            string color = ":color=" + propertyValue.Replace("#", "");   //:color=880000
            _fontStyle.Add(color);
            //_IDProperty["FillColor"] = "Color/" + propertyValue;
        }
        public void BGColor(string propertyValue)
        {
            if (propertyValue == "transparent")
                return;

            if (propertyValue == string.Empty)
            {
                return;
            }
            string cVal = propertyValue.Replace("#", "");
            //decValue += " " + int.Parse(concatChar, System.Globalization.NumberStyles.HexNumber);

            int red = int.Parse(cVal.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            int green = int.Parse(cVal.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            int blue = int.Parse(cVal.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);


            _IDProperty["backgroundColor"] = rgb2cmyk(red,green,blue);

        }
        public void Display(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
          //  _fontStyle.Add(propertyValue);
            _IDProperty["display"] = propertyValue;
        }     
        public void ColumnCount(string propertyValue)
        {
            if (propertyValue == string.Empty || Common.ValidateAlphabets(propertyValue)
                || propertyValue.IndexOf('-') > -1)
            {
                _IDProperty["TextColumnCount"] = "1";
            }
            else
            {
                _IDProperty["TextColumnCount"] = propertyValue;    
            }
        }
        public void ColumnGap(string propertyValue)
        {
            if (propertyValue == string.Empty || Common.ValidateAlphabets(propertyValue)
                || propertyValue.IndexOf('-') > -1)
            {
                _IDProperty["TextColumnGutter"] = "12";
            }
            else
            {
                _IDProperty["TextColumnGutter"] = propertyValue;
            }
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
            else if (propertyValue == "small-caps")
            {
                propertyValue = ":+c2sc,+smcp";
            }
            _IDProperty["Capitalization"] = propertyValue;

            if (propertyValue.Trim().Length > 0)
                _fontOption.Add(propertyValue);
        }
        public void TextDecoration(string propertyValue)
        {
            if (propertyValue == string.Empty || propertyValue == "inherit")
            {
                return;
            }
            if (propertyValue == "underline")
            {
                propertyValue = "\\underbar";
            }
            else if (propertyValue == "none")
            {
                propertyValue = "";
            }
            //propertyValue = "$\\underline{" + propertyValue + "}$";
            //_IDProperty["Underline"] = propertyValue;
            if (propertyValue.Trim().Length > 0)
                _inlineStyle.Add(propertyValue);
        }

        public void FontWeight(Dictionary<string, string> cssProperty)
        {
            if (_IDProperty.ContainsKey("FontStyle")) return;

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

            //if (strValue == "normalnormal" || strValue == "normal")
            //{
            //    propertyValue = "Regular";
            //}
            //else
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
            _IDProperty["FontStyleBold"] = propertyValue;
            if (propertyValue.Trim().Length > 0)
                _fontOption.Add(propertyValue);
        }
        public void TextAlign(string propertyValue)
        {
            if (propertyValue == string.Empty || propertyValue == "inherit")
            {
                return;
            }

            //if (propertyValue == "justify")
            //{
            //    propertyValue = "FullyJustified";
            //}
            //else 
            if (propertyValue == "center")
            {
                propertyValue = "\\centerline";
            }
            else if (propertyValue == "left")
            {
                propertyValue = "\\leftline";
            }
            else if (propertyValue == "right")
            {
                propertyValue = "\\rightline";
            }
            else if (propertyValue == "justify")
            {
                propertyValue = "\\line";
            }
            _IDProperty["Justification"] = propertyValue;
            _inlineStyle.Add(propertyValue);
        }

        public void FontSize(string propertyValue)
        {
            if (propertyValue == "larger" || propertyValue == "smaller")
            {
                _IDProperty["PointSize"] = propertyValue;
            }
            else if (propertyValue == string.Empty || Common.ValidateAlphabets(propertyValue)
                || propertyValue.IndexOf('-') > -1)
            {
                return;
            }
            _IDProperty["PointSize"] = propertyValue;

            if (propertyValue == "larger")
                _fontSize = " at 20pt";
            else if(propertyValue == "smaller")
                _fontSize = " at 10pt";
            else
                _fontSize = " at " + propertyValue + "pt";

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hexValue">"#ff0000"</param>
        /// <returns>"255 0 0"</returns>
        public string ConvertHexToDec(string hexValue)
        {
            string concatChar = string.Empty;
            string decValue = string.Empty;
            try
            {
                if (hexValue.IndexOf("#") == -1) return "00 00 00";
                string hexFormat = hexValue.Replace("#", "");
                char[] RGB = hexFormat.ToCharArray();
                if (RGB.Length < 6)
                    return "00 00 00";

                for (int i = 0; i < RGB.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        concatChar = RGB[i].ToString();
                        continue;
                    }
                    concatChar += RGB[i].ToString();
                    decValue += " " + int.Parse(concatChar, System.Globalization.NumberStyles.HexNumber);
                    concatChar = string.Empty;
                }
            }
            catch
            {
                decValue = "00 00 00";
            }
            return decValue.Trim();
        }


        private string rgb2cmyk(int r, int g, int b)
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

            computedC = 1 - (r/255);
            computedM = 1 - (g/255);
            computedY = 1 - (b/255);

            var minCMY = Math.Min(computedC, Math.Min(computedM, computedY));

            computedC = (computedC - minCMY)/(1 - minCMY);
            computedM = (computedM - minCMY)/(1 - minCMY);
            computedY = (computedY - minCMY)/(1 - minCMY);
            computedK = minCMY;
            string cmyk = computedC + " " + computedM + " " + computedY + " " + computedK;
            return cmyk;
        }

           
    }
}