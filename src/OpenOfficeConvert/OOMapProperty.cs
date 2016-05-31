// --------------------------------------------------------------------------------------------
// <copyright file="OOMapProperty.cs" from='2009' to='2014' company='SIL International'>
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
// Mapping with Libre Office property
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class OOMapProperty
    {
        private readonly Dictionary<string, string> _idProperty = new Dictionary<string, string>();
        private Dictionary<string, string> _cssProperty = new Dictionary<string, string>();
        readonly Dictionary<string, string> _dicColorInfo = new Dictionary<string, string>();
        readonly Dictionary<string, string> _dictFontSize = new Dictionary<string, string>();
        readonly ArrayList _arrUnits = new ArrayList();
        private string _propertyKey = string.Empty;
        private bool _isFixedLineHeightEnable = false;
        private string _baseFontSize = string.Empty;
        public OOMapProperty()
        {
            CreateColor();
            CreateSize();
            CreateUnit();
        }

        public Dictionary<string, string> IDProperty(Dictionary<string, string> cssProperty, bool isFixedLineHeightEnable, string baseFontSize)
        {
            _idProperty.Clear();
            _cssProperty = cssProperty;
            _isFixedLineHeightEnable = isFixedLineHeightEnable;
            _baseFontSize = baseFontSize;
            foreach (KeyValuePair<string, string> property in cssProperty)
            {
                // Null or Empty or inherited property - skip the property
                if (string.IsNullOrEmpty(property.Value) || property.Value == "inherit")
                {
                    continue;
                }
                _propertyKey = property.Key.ToLower();
                bool writtenProperty = MappingFontProperty(property);
                if (!writtenProperty)
                {
                    writtenProperty = MappingPageProperty(cssProperty, property);
                }
                if (!writtenProperty)
                {
                    MappingOtherProperty(property);
                }
            }

            return _idProperty;
        }

        private void MappingOtherProperty(KeyValuePair<string, string> property)
        {
           
            switch (_propertyKey)
            {
                case "hyphens":
                    Hyphens(property.Value);
                    break;
                case "hyphenate-before":
                    HyphenateBefore(property.Value);
                    break;
                case "hyphenate-after":
                    HyphenateAfter(property.Value);
                    break;
                case "hyphenate-lines":
                    HyphenateLines(property.Value);
                    break;
                case "letter-spacing":
                    LetterSpacing(property.Value);
                    break;
                case "word-spacing":
                    WordSpacing(property.Value);
                    break;
                case "orphans":
                    Orphans(property.Value);
                    break;
                case "widows":
                    Widows(property.Value);
                    break;
                case "direction":
                    Direction(property.Value);
                    break;
                case "-ps-vertical-justification":
                    VerticalJustification(property.Value);
                    break;
                case "column-fill":
                    ColumnFill(property.Value);
                    break;
                case "column-rule-style":
                    ColumnRule(property.Value);
                    break;
                case "column-rule-color":
                    ColumnColor(property.Value);
                    break;
                case "column-rule-width":
                    ColumnWidth(property.Value);
                    break;
                case "-ps-custom-footnote-caller":
                    CustomFootnoteCaller(property.Value);
                    break;
                case "-ps-custom-XRef-caller":
                    CustomXrefCaller(property.Value);
                    break;
                case "-ps-hide-space-versenumber":
                    HideSpaceVerseNumber(property.Value);
                    break;
                case "pathway":
                    Pathway(property.Value);
                    break;
                default:
                    SimpleProperty(property);
                    break;
            }
        }

        private bool MappingPageProperty(Dictionary<string, string> cssProperty, KeyValuePair<string, string> property)
        {
            bool isPropertyWritten = false;
            switch (_propertyKey)
            {
                case "padding-left":
                case "class-margin-left":
                    PaddingLeft(property.Value);
                    isPropertyWritten = true;
                    break;
                case "padding-right":
                case "class-margin-right":
                    PaddingRight(property.Value);
                    isPropertyWritten = true;
                    break;
                case "padding-top":
                case "class-margin-top":
                    PaddingTop(property.Value);
                    isPropertyWritten = true;
                    break;
                case "padding-bottom":
                case "class-margin-bottom":
                    PaddingBottom(property.Value);
                    isPropertyWritten = true;
                    break;
                case "padding":
                case "margin":
                    //Margin(styleAttributeInfo);
                    isPropertyWritten = true;
                    break;
                case "color":
                    Color(property.Value);
                    isPropertyWritten = true;
                    break;
                case "background-color":
                    BGColor(property.Value);
                    isPropertyWritten = true;
                    break;
                case "size":
                    //Size(styleAttributeInfo);
                    isPropertyWritten = true;
                    break;
                case "language":
                    //Language(styleAttributeInfo);
                    isPropertyWritten = true;
                    break;
                case "border-top":
                case "border-bottom":
                case "border-left":
                case "border-right":
                    Border(property.Value);
                    isPropertyWritten = true;
                    break;
                case "column-count":
                case "columns":
                    ColumnCount(property.Value);
                    isPropertyWritten = true;
                    break;
                case "column-gap":
                    ColumnGap(property.Value);
                    isPropertyWritten = true;
                    break;
                case "display":
                    Display(property.Value);
                    isPropertyWritten = true;
                    break;
                case "page-break-before":
                    PageBreakBefore(property.Value);
                    isPropertyWritten = true;
                    break;
                case "page-break-after":
                    PageBreakAfter(property.Value);
                    isPropertyWritten = true;
                    break;
                case "page-break-inside":
                    PageBreakInside(property.Value);
                    isPropertyWritten = true;
                    break;
                case "text-transform":
                    TextTransform(property.Value);
                    isPropertyWritten = true;
                    break;
                case "vertical-align":
                    VerticalAlign(property.Value);
                    isPropertyWritten = true;
                    break;
                case "line-height":
                    LineHeight(property.Value, cssProperty);
                    isPropertyWritten = true;
                    break;
				case "position":
					PositionTop(property.Value);
					isPropertyWritten = true;
					break;
            }
            return isPropertyWritten;
        }

        private bool MappingFontProperty(KeyValuePair<string, string> property)
        {
            bool isPropertyWritten = false;
            switch (_propertyKey)
            {
                case "font-weight":
                    FontWeight(property.Value);
                    isPropertyWritten = true;
                    break;
                case "font-style":
                    FontStyle(property.Value);
                    isPropertyWritten = true;
                    break;
                case "text-align":
                    TextAlign(property.Value);
                    isPropertyWritten = true;
                    break;
                case "font-size":
                    FontSize(property.Value);
                    isPropertyWritten = true;
                    break;
                case "text-decoration":
                    TextDecoration(property.Value);
                    isPropertyWritten = true;
                    break;
                case "font-variant":
                    FontVariant(property.Value);
                    isPropertyWritten = true;
                    break;
                case "text-indent":
                    TextIndent(property.Value);
                    isPropertyWritten = true;
                    break;
                case "margin-left":
                    MarginLeft(property.Value);
                    isPropertyWritten = true;
                    break;
                case "margin-right":
                    MarginRight(property.Value);
                    isPropertyWritten = true;
                    break;
                case "margin-top":
                    MarginTop(property.Value);
                    isPropertyWritten = true;
                    break;
                case "margin-bottom":
                    MarginBottom(property.Value);
                    isPropertyWritten = true;
                    break;
                case "font-family":
                    FontFamily(property.Value);
                    isPropertyWritten = true;
                    break;
                case "page-width":
                    PageWidth(property.Value);
                    isPropertyWritten = true;
                    break;
                case "page-height":
                    PageHeight(property.Value);
                    isPropertyWritten = true;
                    break;
                case "mirror":
                    Mirror(property.Value);
                    isPropertyWritten = true;
                    break;
            }
            return isPropertyWritten;
        }

        private void ColumnRule(string propertyValue)
        {
            _idProperty[_propertyKey] = propertyValue;
        }

        private void ColumnColor(string propertyValue)
        {
            propertyValue = ColorConversion(propertyValue);
            _idProperty[_propertyKey] = propertyValue;
        }

        private void ColumnWidth(string propertyValue)
        {
            _idProperty[_propertyKey] = Add_pt(propertyValue);
        }

        private void CustomFootnoteCaller(string propertyValue)
        {
            _idProperty[_propertyKey] = propertyValue;
        }

        private void CustomXrefCaller(string propertyValue)
        {
            _idProperty[_propertyKey] = propertyValue;
        }

        private void HideSpaceVerseNumber(string propertyValue)
        {
            _idProperty[_propertyKey] = propertyValue;
        }

        public void VerticalJustification(string propertyValue)
        {
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
            _idProperty["VerticalJustification"] = value;
        }

        public void Direction(string propertyValue)
        {
            if (propertyValue == "rtl")
            {
                _idProperty["writing-mode"] = "rl-tb";
                if (!_idProperty.ContainsKey("text-align") || _idProperty["text-align"] != "center")
                {
                    _idProperty["text-align"] = "end";
                }
            }
            else if (propertyValue == "ltr")
            {
                _idProperty["writing-mode"] = "lr-tb";
            }
        }

        private void Widows(string propertyValue)
        {
            _idProperty[_propertyKey] = propertyValue;
        }

        private void Orphans(string propertyValue)
        {
            _idProperty[_propertyKey] = propertyValue;
        }

        public void WordSpacing(string propertyValue)
        {
            _idProperty["MinimumWordSpacing"] = "0";
            _idProperty["DesiredWordSpacing"] = propertyValue;
            _idProperty["MaximumWordSpacing"] = propertyValue;
        }
        public void LetterSpacing(string propertyValue)
        {
            _idProperty[_propertyKey] = Add_pt(propertyValue);
        }

        public void HyphenateLines(string propertyValue)
        {
            _idProperty["hyphenation-ladder-count"] = propertyValue;
        }
        public void HyphenateAfter(string propertyValue)
        {
            _idProperty["hyphenation-remain-char-count"] = propertyValue;
        }
        public void HyphenateBefore(string propertyValue)
        {
            _idProperty["hyphenation-push-char-count"] = propertyValue;
        }
        public void Hyphens(string propertyValue)
        {
            string value = propertyValue == "none" ? "false" : "true";
            _idProperty["hyphenate"] = value;
        }
        public void SimpleProperty(KeyValuePair<string, string> property)
        {
            _idProperty[property.Key.ToLower()] = property.Value;
        }

        public void LineHeight(string propertyValue, Dictionary<string, string> cssProperty)
        {
            if (_isFixedLineHeightEnable)
            {
                _propertyKey = "line-height";
            }
            else
            {
                _propertyKey = "line-height-at-least";
            }


            if (cssProperty.ContainsKey("position") && cssProperty["position"] == "footnote")
                _propertyKey = "line-height-at-least";

            if (propertyValue.IndexOf("%") > 0)
            {
                _idProperty[_propertyKey] = propertyValue; // refer xhtmlprocess.AssignProperty()
            }
            else
            {
                _idProperty[_propertyKey] = Add_pt(propertyValue);
            }
        }
        public void VerticalAlign(string propertyValue)
        {
            try
            {
                if (propertyValue == "super" || propertyValue == "sub")
                {
                    _propertyKey = "text-position";
                    SuperSub();
                    propertyValue += " 55%";
                }
                else if (propertyValue == "text-top")
                {
                    propertyValue = "top";
                }
                else if (propertyValue == "text-bottom")
                {
                    propertyValue = "bottom";
                }
                else if (propertyValue == "middle"
                         || propertyValue == "top" || propertyValue == "bottom"
                         || propertyValue == "baseline")
                {
                }
                else
                {
                    throw new Exception("Input not valid");
                }
                _idProperty[_propertyKey] = propertyValue;
            }
            catch
            {
            }
        }

		public void PositionTop(string propertyValue)
		{
			try
			{
				if (propertyValue == "relative")
				{
					_propertyKey = "text-position";
					if (_cssProperty.ContainsKey("top"))
					{
						//Default value size for top poperty in percentage
						const string topValue = "-12%";
						propertyValue = topValue;
					}
					if (_cssProperty.ContainsKey("font-size"))
					{
						if (_cssProperty["font-size"].IndexOf("%", StringComparison.Ordinal) > 0)
						{
							//Apply font-size for the text
							propertyValue += " " + _cssProperty["font-size"];

							//Change basefont as 100%
							var value = int.Parse(_cssProperty["font-size"].Replace("%", ""));
							if (value <= 100)
							{
								value = 100;
							}
							_idProperty["font-size"] = value + "%";
						}
					}
					_idProperty[_propertyKey] = propertyValue;
				}
			}
			catch
			{
			}
		}

        /// <summary>
        /// Superscript and Subscript
        /// make 6pt to 9pt (1.5 times) if greater than 10pt
        /// make 100% to 150% (1.5 times) if less than or equal to 100%
        /// </summary>
        public void SuperSub()
        {
            if (_cssProperty.ContainsKey("font-size"))
            {
                string propertyValue = _cssProperty["font-size"];

                if (propertyValue.IndexOf("%") > 0)
                {
                    int value = int.Parse(propertyValue.Replace("%", ""));
                    if (value <= 100)
                    {
                        value = 100;
                    }
                    _idProperty["font-size"] = value + "%";
                }
                else if (propertyValue == "smaller")
                {
                    _idProperty["font-size"] = "75%";
                }
                else if (propertyValue == "larger")
                {
                    _idProperty["font-size"] = "100%";
                }
                else
                {
                    int value = int.Parse(propertyValue.Replace("pt", ""));
                    int defaultSize = int.Parse(_baseFontSize.Replace("pt", ""));
                    if (value < defaultSize)
                    {
                        value = defaultSize;
                    }
                    _idProperty["font-size"] = value + "pt";
                }
            }
            else
            {
                _idProperty["font-size"] = "75%";
            }
        }

        public void TextTransform(string propertyValue)
        {
            _idProperty[_propertyKey] = propertyValue;
        }

        public void PageBreakBefore(string propertyValue)
        {
            _propertyKey = "break-before";
            if (propertyValue == "always")
            {
                propertyValue = "page";
            }
            else if (propertyValue == "auto" || propertyValue == "avoid")
            {
            }
            else
            {
                _propertyKey = "page-break-before";
                throw new Exception("Not a Valid input");
            }
            _idProperty[_propertyKey] = propertyValue;
        }

        private void RemoveClassHyphen()
        {
            if (_propertyKey.IndexOf("class-") >= 0)
            {
                _propertyKey = _propertyKey.Replace("class-", "");
            }
        }

        public void PaddingLeft(string propertyValue)
        {
            RemoveClassHyphen();
            _idProperty[_propertyKey] = Add_pt(propertyValue);
            IncludeBorder(_propertyKey);
        }

        public void PaddingRight(string propertyValue)
        {
            RemoveClassHyphen();
            _idProperty[_propertyKey] = Add_pt(propertyValue);
            IncludeBorder(_propertyKey);
        }
        public void PaddingTop(string propertyValue)
        {
            RemoveClassHyphen();
            _idProperty[_propertyKey] = Add_pt(propertyValue);
            IncludeBorder(_propertyKey);
        }
        public void PaddingBottom(string propertyValue)
        {
            RemoveClassHyphen();
            _idProperty[_propertyKey] = Add_pt(propertyValue);
            IncludeBorder(_propertyKey);
        }

        public void IncludeBorder(string propertyKey)
        {
            if (propertyKey.IndexOf("padding") == -1) return;
            string borderPos = "border-bottom";
            AppendBorderToPadding(borderPos);
            borderPos = "border-top";
            AppendBorderToPadding(borderPos);
            borderPos = "border-left";
            AppendBorderToPadding(borderPos);
            borderPos = "border-right";
            AppendBorderToPadding(borderPos);
        }

        private void AppendBorderToPadding(string borderPos)
        {
            if (!_idProperty.ContainsKey(borderPos))
            {
                _idProperty[borderPos] = "0.5pt solid #ffffff";
            }
        }

        public void Mirror(string propertyValue)
        {
            _idProperty["FacingPages"] = propertyValue;
        }
        public void PageHeight(string propertyValue)
        {
            _idProperty[_propertyKey] = Add_pt(propertyValue);
        }
        public void PageWidth(string propertyValue)
        {
            _idProperty[_propertyKey] = Add_pt(propertyValue);
        }
        public void MarginLeft(string propertyValue)
        {
            RemoveClassHyphen();
            _idProperty[_propertyKey] = Add_pt(propertyValue);
        }
        public void MarginRight(string propertyValue)
        {
            RemoveClassHyphen();
            _idProperty[_propertyKey] = Add_pt(propertyValue);
        }
        public void MarginTop(string propertyValue)
        {
            RemoveClassHyphen();
            _idProperty[_propertyKey] = Add_pt(propertyValue);
        }
        public void MarginBottom(string propertyValue)
        {
            RemoveClassHyphen();
            _idProperty[_propertyKey] = Add_pt(propertyValue);
        }

        private string Add_pt(string propertyValue)
        {
            if (propertyValue.IndexOf('%') < 0)
            {
                propertyValue = propertyValue + "pt";
            }
            return propertyValue;
        }

        public void FontFamily(string propertyValue)
        {
            string[] font = propertyValue.Split(',');
            int fontLength = font.Length;
            if (fontLength == 0)
            {
                return;
            }

            string fontName = string.Empty;
            System.Drawing.FontFamily[] systemFontList = System.Drawing.FontFamily.Families;
            for (int counter = 0; counter < fontLength; counter++)
            {
                fontName = font[counter].Replace("\"", "").Trim();
                foreach (System.Drawing.FontFamily systemFont in systemFontList)
                {
                    if (fontName.ToLower() == systemFont.Name.ToLower())
                    {
                        propertyValue = systemFont.Name;
                        _idProperty["font-family"] = propertyValue;
                        return;
                    }
                }
            }

            string genericFamily = font[fontLength - 1];
            genericFamily = genericFamily.Replace("\"", "").Trim().ToLower();
            ArrayList genericFamilyList = new ArrayList(new[] { "serif", "sans-serif", "cursive", "fantasy", "monospace" });

            fontName = font[0];
            if (genericFamilyList.Contains(genericFamily))
            {
                string xmlFileNameWithPath = Common.CopyXmlFileToTempDirectory("GenericFont.xml");
                string xPath = "//font-preference/generic-family [@name = \"" + genericFamily + "\"]";
                ArrayList fontList = new ArrayList();
                fontList = Common.GetXmlNodeList(xmlFileNameWithPath, xPath);
                if (fontList.Count > 0)
                {
                    fontName = fontList[0].ToString();
                }
            }
            else
            {
                fontName = font[0];
            }

            propertyValue = fontName.Trim();
            _idProperty[_propertyKey] = propertyValue;
        }

        public void TextIndent(string propertyValue)
        {
            _idProperty[_propertyKey] = Add_pt(propertyValue);
        }

        public void Color(string propertyValue)
        {
            propertyValue = ColorConversion(propertyValue);
            _idProperty[_propertyKey] = propertyValue;
        }

        private string ColorConversion(string propertyValue)
        {
            if (propertyValue.IndexOf("rgb") >= 0)
            {
                propertyValue = ColorRGB(propertyValue);
            }
            else if (propertyValue.IndexOf("#") >= 0)
            {
                propertyValue = ColorHash(propertyValue);
            }
            else if (propertyValue.IndexOf("cmyk") >= 0)
            {
                propertyValue = ColorCMYK(propertyValue);
                propertyValue = ColorRGB(propertyValue);
            }
            else if (_dicColorInfo.ContainsKey(propertyValue.ToLower()))
            {
                propertyValue = _dicColorInfo[propertyValue.ToLower()];
            }
            return propertyValue;
        }

        public void BGColor(string propertyValue)
        {
            Color(propertyValue);
        }

        public void ColumnCount(string propertyValue)
        {
            if (_propertyKey == "columns")
            {
                _propertyKey = "column-count";
            }
            _idProperty[_propertyKey] = propertyValue;
        }

        public void ColumnGap(string propertyValue)
        {
            if (propertyValue == string.Empty || Common.ValidateAlphabets(propertyValue)
                || propertyValue.IndexOf('-') > -1)
            {
                _idProperty[_propertyKey] = "12pt";
            }
            else
            {
                _idProperty[_propertyKey] = Add_pt(propertyValue);
            }
        }

        public void FontVariant(string propertyValue)
        {
            _idProperty[_propertyKey] = propertyValue;
        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="propertyValue"></param>
        public void TextDecoration(string propertyValue)
        {
            string propertyName = string.Empty;
            if (propertyValue == "underline")
            {
                propertyName = "text-underline-style";
                propertyValue = "solid";
            }
            else if (propertyValue == "line-through")
            {
                propertyName = "text-line-through-style";
                propertyValue = "solid";
            }
            else if (propertyValue == "none")
            {
                propertyName = "text-underline-style";
                propertyValue = "none";
            }
            else if (propertyValue == "overline" || propertyValue == "blink")
            {
            }
            else
            {
                return;
            }
            _idProperty[propertyName] = propertyValue;
        }

        public void FontWeight(string propertyValue)
        {
            if (propertyValue == "bold")
            {
                propertyValue = "700";
            }
            else if (propertyValue == "normal")
            {
                propertyValue = "400";
            }
            _idProperty[_propertyKey] = propertyValue;
        }

        public void FontStyle(string propertyValue)
        {
            _idProperty[_propertyKey] = propertyValue;
        }

        public void TextAlign(string propertyValue)
        {
            _idProperty["text-align"] = propertyValue;
        }

        public void FontSize(string propertyValue)
        {
            if (_idProperty.ContainsKey("text-position"))
            {
                return;
            }

            if (_dictFontSize.ContainsKey(propertyValue))
            {
                propertyValue = _dictFontSize[propertyValue];
            }
            else if (propertyValue == "normal")
            {
                return;
            }
            else if (propertyValue == "smaller" || propertyValue == "larger")
            {
                _idProperty[_propertyKey] = propertyValue;
                return;
            }
            _idProperty[_propertyKey] = Add_pt(propertyValue);
        }

        private void Border(string propertyValue)
        {
            _idProperty[_propertyKey] = propertyValue;
        }

        private void ColumnFill(string propertyValue)
        {
            _propertyKey = "dont-balance-text-columns";
            if (propertyValue == "auto")
            {
                propertyValue = "true";
            }
            else if (propertyValue == "balance")
            {
                propertyValue = "false";
            }
            else
            {
                //throw new Exception("Not a Valid input");
            }
            _idProperty[_propertyKey] = propertyValue;
        }

        public void PageBreakAfter(string propertyValue)
        {
            if (propertyValue == "always")
            {
                _propertyKey = "break-after";
                propertyValue = "page";
            }
            else if (propertyValue == "avoid")
            {
                _propertyKey = "keep-with-next";
                propertyValue = "always";
            }
            else if (propertyValue == "auto")
            {
            }
            else
            {
                _propertyKey = "page-break-after";
                throw new Exception("Not a Valid input");
            }
            _idProperty[_propertyKey] = propertyValue;
        }

        public void PageBreakInside(string propertyValue)
        {
            if (propertyValue == "avoid")
            {
                _propertyKey = "keep-with-next";
                propertyValue = "always";
            }
            else if (propertyValue == "auto")
            {
            }
            else
            {
                _propertyKey = "page-break-inside";
                throw new Exception("Not a Valid input");
            }
            _idProperty[_propertyKey] = propertyValue;
        }

        public void Display(string propertyValue)
        {
            _idProperty[_propertyKey] = propertyValue;
        }

        public void Pathway(string propertyValue)
        {
            _idProperty[_propertyKey] = propertyValue;
        }

        #region private methods
        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Converts rgb(47,96,255) to "#ff0000" format
        /// 
        /// <list>
        /// </list>
        /// </summary>
        /// <param name="attributeStringValue">attribute value like rgb(47,96,255)</param>
        /// <returns>"#ff0000" format data</returns>
        /// -------------------------------------------------------------------------------------------
        private static string ColorRGB(string attributeStringValue)
        {
            try
            {

                int startIndex = attributeStringValue.IndexOf('(') + 2;
                int endIndex = attributeStringValue.IndexOf(')') - 1;
                string colorStr = attributeStringValue.Substring(startIndex, endIndex - startIndex);
                string[] rgbValues = colorStr.Split(',');
                string retValue = "#";
                foreach (string color in rgbValues)
                {
                    int colValue = Convert.ToInt32(color);
                    retValue = retValue + String.Format("{0:X2}", colValue);
                }

                if (retValue.Length != 7)
                {
                    throw new Exception("Parameter Length - Not Valid");
                }

                return (retValue.Trim());
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        private static string ColorHash(string attributeStringValue)
        {
            try
            {
                string retValue = attributeStringValue;
                int colorLen = attributeStringValue.Length;
                if (colorLen == 4)
                {
                    retValue = "#" + attributeStringValue[1] + attributeStringValue[1] + attributeStringValue[2] + attributeStringValue[2] + attributeStringValue[3] + attributeStringValue[3];
                }
                if (retValue.Length != 7)
                {
                    throw new Exception("Parameter Length - Not Valid");
                }
                return retValue;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Converts cmyk to "#ff0000" format
        /// 
        /// <list>
        /// </list>
        /// </summary>
        /// <param name="attributeStringValue">attribute value like cmyk(0.5,0.1,0.0,0.2) </param>
        /// <returns>"#ff0000" format data</returns>
        /// -------------------------------------------------------------------------------------------
        private static string ColorCMYK(string attributeStringValue)
        {
            try
            {
                int startIndex = attributeStringValue.IndexOf('(') + 2;
                int endIndex = attributeStringValue.IndexOf(')') - 1;
                string colorStr = attributeStringValue.Substring(startIndex, endIndex - startIndex);
                string[] rgbValues = colorStr.Split(',');

                double C = double.Parse(rgbValues[0], CultureInfo.GetCultureInfo("en-US"));
                double M = double.Parse(rgbValues[1], CultureInfo.GetCultureInfo("en-US"));
                double Y = double.Parse(rgbValues[2], CultureInfo.GetCultureInfo("en-US"));
                double K = double.Parse(rgbValues[3], CultureInfo.GetCultureInfo("en-US"));

                double R = C * (1.0 - K) + K;
                double G = M * (1.0 - K) + K;
                double B = Y * (1.0 - K) + K;

                R = (1.0 - R) * 255.0;// +0.5;
                G = (1.0 - G) * 255.0;// +0.5;
                B = (1.0 - B) * 255.0;// +0.5;

                int r = Convert.ToInt32(R);
                int g = Convert.ToInt32(G);
                int b = Convert.ToInt32(B);


                string rgb = "rgb" + ",(," + r + "," + g + "," + b + ",)";
                return rgb;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Parameter Length - Not Valid");
            }
        }

        private void CreateColor()
        {
            _dicColorInfo.Add("aliceblue", "#f0f8ff");
            _dicColorInfo.Add("antiquewhite", "#faebd7");
            _dicColorInfo.Add("aqua", "#00ffff");
            _dicColorInfo.Add("aquamarine", "#7fffd4");
            _dicColorInfo.Add("azure", "#f0ffff");
            _dicColorInfo.Add("beige", "#f5f5dc");
            _dicColorInfo.Add("bisque", "#ffe4c4");
            _dicColorInfo.Add("black", "#000000");
            _dicColorInfo.Add("blanchedalmond", "#ffebcd");
            _dicColorInfo.Add("blue", "#0000ff");
            _dicColorInfo.Add("blueviolet", "#8a2be2");
            _dicColorInfo.Add("brown", "#a52a2a");
            _dicColorInfo.Add("burlywood", "#deb887");
            _dicColorInfo.Add("cadetblue", "#5f9ea0");
            _dicColorInfo.Add("chartreuse", "#7fff00");
            _dicColorInfo.Add("chocolate", "#d2691e");
            _dicColorInfo.Add("coral", "#ff7f50");
            _dicColorInfo.Add("cornflowerblue", "#6495ed");
            _dicColorInfo.Add("cornsilk", "#fff8dc");
            _dicColorInfo.Add("crimson", "#dc143c");
            _dicColorInfo.Add("cyan", "#00ffff");
            _dicColorInfo.Add("darkblue", "#00008b");
            _dicColorInfo.Add("darkcyan", "#008b8b");
            _dicColorInfo.Add("darkgoldenrod", "#b8860b");
            _dicColorInfo.Add("darkgray", "#a9a9a9");
            _dicColorInfo.Add("darkgreen", "#006400");
            _dicColorInfo.Add("darkkhaki", "#bdb76b");
            _dicColorInfo.Add("darkmagenta", "#8b008b");
            _dicColorInfo.Add("darkolivegreen", "#556b2f");
            _dicColorInfo.Add("darkorange", "#ff8c00");
            _dicColorInfo.Add("darkorchid", "#9932cc");
            _dicColorInfo.Add("darkred", "#8b0000");
            _dicColorInfo.Add("darksalmon", "#e9967a");
            _dicColorInfo.Add("darkseagreen", "#8fbc8f");
            _dicColorInfo.Add("darkslateblue", "#483d8b");
            _dicColorInfo.Add("darkslategray", "#2f4f4f");
            _dicColorInfo.Add("darkturquoise", "#00ced1");
            _dicColorInfo.Add("darkviolet", "#9400d3");
            _dicColorInfo.Add("deeppink", "#ff1493");
            _dicColorInfo.Add("deepskyblue", "#00bfff");
            _dicColorInfo.Add("dimgray", "#696969");
            _dicColorInfo.Add("dodgerblue", "#1e90ff");
            _dicColorInfo.Add("firebrick", "#b22222");
            _dicColorInfo.Add("floralwhite", "#fffaf0");
            _dicColorInfo.Add("forestgreen", "#228b22");
            _dicColorInfo.Add("fuchsia", "#ff00ff");
            _dicColorInfo.Add("gainsboro", "#dcdcdc");
            _dicColorInfo.Add("ghostwhite", "#f8f8ff");
            _dicColorInfo.Add("gold", "#ffd700");
            _dicColorInfo.Add("goldenrod", "#daa520");
            _dicColorInfo.Add("gray", "#808080");
            _dicColorInfo.Add("green", "#008000");
            _dicColorInfo.Add("greenyellow", "#adff2f");
            _dicColorInfo.Add("honeydew", "#f0fff0");
            _dicColorInfo.Add("hotpink", "#ff69b4");
            _dicColorInfo.Add("indianred", "#cd5c5c");
            _dicColorInfo.Add("indigo", "#4b0082");
            _dicColorInfo.Add("ivory", "#fffff0");
            _dicColorInfo.Add("khaki", "#f0e68c");
            _dicColorInfo.Add("lavender", "#e6e6fa");
            _dicColorInfo.Add("lavenderblush", "#fff0f5");
            _dicColorInfo.Add("lawngreen", "#7cfc00");
            _dicColorInfo.Add("lemonchiffon", "#fffacd");
            _dicColorInfo.Add("lightblue", "#add8e6");
            _dicColorInfo.Add("lightcoral", "#f08080");
            _dicColorInfo.Add("lightcyan", "#e0ffff");
            _dicColorInfo.Add("lightgoldenrodyellow", "#fafad2");
            _dicColorInfo.Add("lightgrey", "#d3d3d3");
            _dicColorInfo.Add("lightgreen", "#90ee90");
            _dicColorInfo.Add("lightpink", "#ffb6c1");
            _dicColorInfo.Add("lightsalmon", "#ffa07a");
            _dicColorInfo.Add("lightseagreen", "#20b2aa");
            _dicColorInfo.Add("lightskyblue", "#87cefa");
            _dicColorInfo.Add("lightslategray", "#778899");
            _dicColorInfo.Add("lightsteelblue", "#b0c4de");
            _dicColorInfo.Add("lightyellow", "#ffffe0");
            _dicColorInfo.Add("lime", "#00ff00");
            _dicColorInfo.Add("limegreen", "#32cd32");
            _dicColorInfo.Add("linen", "#faf0e6");
            _dicColorInfo.Add("magenta", "#ff00ff");
            _dicColorInfo.Add("maroon", "#800000");
            _dicColorInfo.Add("mediumaquamarine", "#66cdaa");
            _dicColorInfo.Add("mediumblue", "#0000cd");
            _dicColorInfo.Add("mediumorchid", "#ba55d3");
            _dicColorInfo.Add("mediumpurple", "#9370d8");
            _dicColorInfo.Add("mediumseagreen", "#3cb371");
            _dicColorInfo.Add("mediumslateblue", "#7b68ee");
            _dicColorInfo.Add("mediumspringgreen", "#00fa9a");
            _dicColorInfo.Add("mediumturquoise", "#48d1cc");
            _dicColorInfo.Add("mediumvioletred", "#c71585");
            _dicColorInfo.Add("midnightblue", "#191970");
            _dicColorInfo.Add("mintcream", "#f5fffa");
            _dicColorInfo.Add("mistyrose", "#ffe4e1");
            _dicColorInfo.Add("moccasin", "#ffe4b5");
            _dicColorInfo.Add("navajowhite", "#ffdead");
            _dicColorInfo.Add("navy", "#000080");
            _dicColorInfo.Add("oldlace", "#fdf5e6");
            _dicColorInfo.Add("olive", "#808000");
            _dicColorInfo.Add("olivedrab", "#6b8e23");
            _dicColorInfo.Add("orange", "#ffa500");
            _dicColorInfo.Add("orangered", "#ff4500");
            _dicColorInfo.Add("orchid", "#da70d6");
            _dicColorInfo.Add("palegoldenrod", "#eee8aa");
            _dicColorInfo.Add("palegreen", "#98fb98");
            _dicColorInfo.Add("paleturquoise", "#afeeee");
            _dicColorInfo.Add("palevioletred", "#d87093");
            _dicColorInfo.Add("papayawhip", "#ffefd5");
            _dicColorInfo.Add("peachpuff", "#ffdab9");
            _dicColorInfo.Add("peru", "#cd853f");
            _dicColorInfo.Add("pink", "#ffc0cb");
            _dicColorInfo.Add("plum", "#dda0dd");
            _dicColorInfo.Add("powderblue", "#b0e0e6");
            _dicColorInfo.Add("purple", "#800080");
            _dicColorInfo.Add("red", "#ff0000");
            _dicColorInfo.Add("rosybrown", "#bc8f8f");
            _dicColorInfo.Add("royalblue", "#4169e1");
            _dicColorInfo.Add("saddlebrown", "#8b4513");
            _dicColorInfo.Add("salmon", "#fa8072");
            _dicColorInfo.Add("sandybrown", "#f4a460");
            _dicColorInfo.Add("seagreen", "#2e8b57");
            _dicColorInfo.Add("seashell", "#fff5ee");
            _dicColorInfo.Add("sienna", "#a0522d");
            _dicColorInfo.Add("silver", "#c0c0c0");
            _dicColorInfo.Add("skyblue", "#87ceeb");
            _dicColorInfo.Add("slateblue", "#6a5acd");
            _dicColorInfo.Add("slategray", "#708090");
            _dicColorInfo.Add("snow", "#fffafa");
            _dicColorInfo.Add("springgreen", "#00ff7f");
            _dicColorInfo.Add("steelblue", "#4682b4");
            _dicColorInfo.Add("tan", "#d2b48c");
            _dicColorInfo.Add("teal", "#008080");
            _dicColorInfo.Add("thistle", "#d8bfd8");
            _dicColorInfo.Add("tomato", "#ff6347");
            _dicColorInfo.Add("turquoise", "#40e0d0");
            _dicColorInfo.Add("violet", "#ee82ee");
            _dicColorInfo.Add("wheat", "#f5deb3");
            _dicColorInfo.Add("white", "#ffffff");
            _dicColorInfo.Add("whitesmoke", "#f5f5f5");
            _dicColorInfo.Add("yellow", "#ffff00");
            _dicColorInfo.Add("yellowgreen", "#9acd32");
        }
        private void CreateSize()
        {
            _dictFontSize.Add("xx-small", "6.6pt");
            _dictFontSize.Add("x-small", "7.5pt");
            _dictFontSize.Add("small", "10pt");
            _dictFontSize.Add("medium", "12pt");
            _dictFontSize.Add("large", "14pt");
            _dictFontSize.Add("x-large", "18pt");
            _dictFontSize.Add("xx-large", "24pt");
        }

        private void CreateUnit()
        {
            _arrUnits.Add("em");
            _arrUnits.Add("ex");
            _arrUnits.Add("px");
            _arrUnits.Add("in");
            _arrUnits.Add("cm");
            _arrUnits.Add("mm");
            _arrUnits.Add("pt");
            _arrUnits.Add("pc");
        }
        #endregion
    }
}