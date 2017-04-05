// --------------------------------------------------------------------------------------------
// <copyright file="MakeProperty.cs" from='2009' to='2014' company='SIL International'>
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
// Maps all property of Css
// </remarks>
// --------------------------------------------------------------------------------------------

#region Using
using System;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
using System.Windows.Forms;
using System.Drawing;
using SIL.Tool;

#endregion Using

namespace SIL.PublishingSolution
{
    public class MakeProperty
    {
        #region Private Variable
        readonly Dictionary<string, string> _dicColorInfo = new Dictionary<string, string>();
        readonly Dictionary<string, string> _dictFontSize = new Dictionary<string, string>();
        readonly ArrayList _unit = new ArrayList();
        private Dictionary<string, string> _cssProperty = new Dictionary<string, string>();
        #endregion

        #region Private Variable
        public string PsSupportPath = Common.GetPSApplicationPath();
        public ArrayList CssBorderColor = new ArrayList();
        #endregion

        #region Constructor Function
        public MakeProperty()
        {
            _unit.Add("em");
            _unit.Add("ex");
            _unit.Add("%");

            _unit.Add("px");
            _unit.Add("in");
            _unit.Add("cm");
            _unit.Add("mm");
            _unit.Add("pt");
            _unit.Add("pc");


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

            _dictFontSize.Add("xx-small", "6.6");
            _dictFontSize.Add("x-small", "7.5");
            _dictFontSize.Add("small", "10");
            _dictFontSize.Add("medium", "12");
            _dictFontSize.Add("large", "14");
            _dictFontSize.Add("x-large", "18");
            _dictFontSize.Add("xx-large", "24");
            _dictFontSize.Add("larger", "larger");
            _dictFontSize.Add("smaller", "smaller");
        }


        public Dictionary<string, string> GetProperty
        {
            get { return _cssProperty; }
        }

        public void ClearProperty()
        {
            _cssProperty.Clear();
        }

        #endregion

        #region Public Functions

        /// -------------------------------------------------------------------------------------------
        /// Map _attributeInfo to MapPropertys.cs
        /// -------------------------------------------------------------------------------------------

        public Dictionary<string, string> CreateProperty(StyleAttribute styleAttributeInfo)
        {
            _cssProperty.Clear();
            styleAttributeInfo.Name = styleAttributeInfo.Name.ToLower();
            styleAttributeInfo.StringValueLower = styleAttributeInfo.StringValue.ToLower();
	        switch (styleAttributeInfo.Name)
	        {
		        case "padding":
		        case "margin":
		        case "class-margin":
			        Margin(styleAttributeInfo);
			        break;
		        case "color":
		        case "background-color":
				case "text-decoration-color":
			        Color(styleAttributeInfo);
			        break;
		        case "size":
                    Size(styleAttributeInfo);
                    break;
                case "language":
                    Language(styleAttributeInfo);
                    break;
                case "border":
                case "border-top":
                case "border-bottom":
                case "border-left":
                case "border-right":
                case "column-rule":
                    BorderMethod1(styleAttributeInfo);
                    break;
                case "border-style":
                case "border-top-style":
                case "border-right-style":
                case "border-bottom-style":
                case "border-left-style":
                case "border-color":
                case "border-top-color":
                case "border-right-color":
                case "border-bottom-color":
                case "border-left-color":
                case "border-width":
                case "border-top-width":
                case "border-right-width":
                case "border-bottom-width":
                case "border-left-width":
                    BorderMethod2(styleAttributeInfo);
                    break;
                case "font-family":
                    FontFamily(styleAttributeInfo);
                    break;
				case "font-feature-settings":
					FontFeatureSettings(styleAttributeInfo);
                    break;
                case "font":
                    Font(styleAttributeInfo);
                    break;
                case "text-align":
                    TextAlign(styleAttributeInfo);
                    break;
                case "font-variant":
                    FontVariant(styleAttributeInfo);
                    break;
                case "text-decoration":
                    TextDecoration(styleAttributeInfo);
                    break;
                case "font-style":
                    FontStyle(styleAttributeInfo);
                    break;
                case "font-weight":
                    FontWeight(styleAttributeInfo);
                    break;
                case "font-size":
                    FontSize(styleAttributeInfo);
                    break;
                case "column-gap":
                    ColumnGap(styleAttributeInfo);
                    break;
                case "line-height":
                    LineHeight(styleAttributeInfo);
                    break;
				case "flex-line-height":
				case "-ps-fixed-line-height":
                    FixedLineHeight(styleAttributeInfo);
                    break;
                case "-ps-disable-widow-orphan":
                    DisableWidowandOrphan(styleAttributeInfo);
                    break;
                case "counter-increment":
                    CounterIncrement(styleAttributeInfo);
                    break;
                case "orphans":
                    Orphans(styleAttributeInfo);
                    break;
                case "widows":
                    Widows(styleAttributeInfo);
                    break;
                case "marks":
                    Marks(styleAttributeInfo);
                    break;
                case "-ps-outline-level":
                    CommomProperty(styleAttributeInfo);
                    break;
                default:
                    SimpleProperty(styleAttributeInfo);
                    break;
            }
            return _cssProperty;
        }

        private void CommomProperty(StyleAttribute styleAttributeInfo)
        {
            _cssProperty[styleAttributeInfo.Name] = styleAttributeInfo.StringValue;
        }

        private void Marks(StyleAttribute styleAttributeInfo)
        {
            _cssProperty[styleAttributeInfo.Name] = styleAttributeInfo.StringValue;
        }

        private void Widows(StyleAttribute styleAttributeInfo)
        {
            _cssProperty[styleAttributeInfo.Name] = styleAttributeInfo.StringValue;
        }

        private void Orphans(StyleAttribute styleAttributeInfo)
        {
            _cssProperty[styleAttributeInfo.Name] = styleAttributeInfo.StringValue;
        }

        private void CounterIncrement(StyleAttribute styleAttributeInfo)
        {
            _cssProperty[styleAttributeInfo.Name] = styleAttributeInfo.StringValue;
        }

        private void LineHeight(StyleAttribute styleAttributeInfo)
        {
            ValidateLineHeight(styleAttributeInfo);
        }

        private void FixedLineHeight(StyleAttribute styleAttributeInfo)
        {
            ValidateFixedLineHeight(styleAttributeInfo);
        }

        private void DisableWidowandOrphan(StyleAttribute styleAttributeInfo)
        {
            _cssProperty[styleAttributeInfo.Name] = styleAttributeInfo.StringValue;
        }

        private void BorderMethod2(StyleAttribute styleAttributeInfo)
        {
            string propertyName = styleAttributeInfo.Name;
            string propertyValue = styleAttributeInfo.StringValue;
            var borderSide = new[] { "border-top-", "border-right-", "border-bottom-", "border-left-" };
            switch (styleAttributeInfo.Name)
            {
                case "border-style":
                    propertyName = "style";
                    break;
                case "border-color":
                    propertyName = "color";
                    GetBorderColorList(styleAttributeInfo.StringValue);
                    break;
                case "border-width":
                    const string top = "border-top-width";
                    const string right = "border-right-width";
                    const string bottom = "border-bottom-width";
                    const string left = "border-left-width";
                    SetPropertyDimension(styleAttributeInfo, top, right, bottom, left);
                    propertyName = "";
                    break;
                default:
                    if (styleAttributeInfo.Name.IndexOf("width") > 0)
                    {
                        propertyValue = propertyValue.Replace(",", "");
                    }
                    _cssProperty[propertyName] = propertyValue;
                    propertyName = "";
                    break;
            }

            if (propertyName.Length > 0)
                for (int i = 0; i < borderSide.Length; i++)
                {
                    _cssProperty[borderSide[i] + propertyName] = propertyValue;
                }
        }

        private void GetBorderColorList(string propertyValue)
        {
            if (propertyValue.IndexOf("#") == 0)
            {
                if (!CssBorderColor.Contains(propertyValue))
                {
                    CssBorderColor.Add(propertyValue);
                }
            }
        }

        private void FontSize(StyleAttribute styleAttributeInfo)
        {
            Numeric(styleAttributeInfo);
        }

        private void ColumnGap(StyleAttribute styleAttributeInfo)
        {
            Numeric(styleAttributeInfo);
        }

        private void Numeric(StyleAttribute styleAttributeInfo)
        {
            if (styleAttributeInfo.StringValue == "inherit") return;
            string attrValue = DeleteSeperator(styleAttributeInfo.StringValue);

            if (_dictFontSize.ContainsKey(attrValue))
            {
                attrValue = _dictFontSize[attrValue];
            }
            else
            {
                attrValue = Common.UnitConverter(attrValue);
            }
            _cssProperty[styleAttributeInfo.Name] = attrValue;
        }

        private void ValidateLineHeight(StyleAttribute styleAttributeInfo)
        {
            if (styleAttributeInfo.StringValue.ToLower() == "inherit")
                return;
            string attrValue = DeleteSeperator(styleAttributeInfo.StringValue);
            if (styleAttributeInfo.StringValue.ToLower() == "none" || styleAttributeInfo.StringValue.ToLower() == "normal")
            {
                attrValue = "100%";
            }
            else if (styleAttributeInfo.StringValue.IndexOf(",") < 0)
            {
                int value = int.Parse(styleAttributeInfo.StringValue) * 100;
                attrValue = value.ToString() + "%";
            }

            if (_dictFontSize.ContainsKey(attrValue))
            {
                attrValue = _dictFontSize[attrValue];
            }
            else
            {
                attrValue = Common.UnitConverter(attrValue);
            }
            _cssProperty[styleAttributeInfo.Name] = attrValue;
        }

        private void ValidateFixedLineHeight(StyleAttribute styleAttributeInfo)
        {
            string attrValue = DeleteSeperator(styleAttributeInfo.StringValue);
            _cssProperty[styleAttributeInfo.Name] = attrValue;
        }

        private void TextAlign(StyleAttribute styleAttributeInfo)
        {
            string attrValue = styleAttributeInfo.StringValue;

            if (attrValue == "left" || attrValue == "right" || attrValue == "center" || attrValue == "justify" || attrValue == "inherit")
            {
                _cssProperty["text-align"] = attrValue;
            }
        }

        private void FontVariant(StyleAttribute styleAttributeInfo)
        {
            string attrValue = styleAttributeInfo.StringValue;

            if (attrValue == "normal" || attrValue == "small-caps" || attrValue == "inherit")
            {
                _cssProperty["font-variant"] = attrValue;
            }
        }

        private void TextDecoration(StyleAttribute styleAttributeInfo)
        {
            string attrValue = styleAttributeInfo.StringValue;

            if (attrValue == "none" || attrValue == "underline" || attrValue == "inherit" || attrValue == "line-through")
            {
                _cssProperty["text-decoration"] = attrValue;
            }

			if(attrValue.ToLower() == "underline,double")
			{
				_cssProperty["text-decoration"] = "underlineunderline";
			}
        }

        private void FontWeight(StyleAttribute styleAttributeInfo)
        {
            string attrValue = styleAttributeInfo.StringValue;

            if (attrValue == "normal" || attrValue == "100" || attrValue == "200"
                || attrValue == "300" || attrValue == "400" || attrValue == "500"
                || attrValue == "lighter")
            {
                _cssProperty["font-weight"] = "normal";
            }
            else if (attrValue == "bold" || attrValue == "600" || attrValue == "700"
                || attrValue == "800" || attrValue == "900" || attrValue == "bolder")
            {
                _cssProperty["font-weight"] = "bold";
            }
        }

        private void FontStyle(StyleAttribute styleAttributeInfo)
        {
            string attrValue = styleAttributeInfo.StringValue;

            if (attrValue == "normal" || attrValue == "italic")
            {
                _cssProperty["font-style"] = attrValue;
            }
        }

        private void Language(StyleAttribute styleAttributeInfo)
        {
            string[] languageCountrySplit = styleAttributeInfo.StringValue.Split('-');
            string language = languageCountrySplit[0];
            _cssProperty["language"] = language;

            string country;
            if (languageCountrySplit.Length > 1)
            {
                country = languageCountrySplit[1];
                _cssProperty["country"] = country;
            }
            else // find country for english language
            {
                if (language.ToLower() == "en")
                {
                    string[] langCountry = Application.CurrentCulture.IetfLanguageTag.Split('-');
                    country = langCountry[1];
                    _cssProperty["country"] = country;
                }
            }
        }

        private void Size(StyleAttribute styleAttributeInfo)
        {
            string[] parameters = styleAttributeInfo.StringValue.Split(',');
            _cssProperty["page-width"] = Math.Round(double.Parse(Common.UnitConverter(parameters[0] + parameters[1]), CultureInfo.GetCultureInfo("en-US")), 9).ToString(CultureInfo.GetCultureInfo("en-US"));
            _cssProperty["page-height"] = Math.Round(double.Parse(Common.UnitConverter(parameters[2] + parameters[3]), CultureInfo.GetCultureInfo("en-US")), 9).ToString(CultureInfo.GetCultureInfo("en-US"));
        }

        /// <summary>
        /// Returns the Color Value in # (Hash Format)
        /// </summary>
        /// <param name="styleAttributeInfo">StyleAttribute</param>
        private void Color(StyleAttribute styleAttributeInfo)
        {
			_cssProperty[styleAttributeInfo.Name] = Color2Hash(styleAttributeInfo);
        }

        public string Color2Hash(StyleAttribute styleAttributeInfo)
        {
            if (styleAttributeInfo.StringValue.IndexOf("rgb") >= 0)
            {
                styleAttributeInfo.StringValue = ColorRGB(styleAttributeInfo.StringValue);
            }
            else if (styleAttributeInfo.StringValue.IndexOf("#") >= 0)
            {
                styleAttributeInfo.StringValue = ColorHash(styleAttributeInfo.StringValue);
            }
            else if (styleAttributeInfo.StringValue.IndexOf("cmyk") >= 0)
            {
                styleAttributeInfo.StringValue = ColorCMYK(styleAttributeInfo.StringValue);
                styleAttributeInfo.StringValue = ColorRGB(styleAttributeInfo.StringValue);
            }
            else if (_dicColorInfo.ContainsKey(styleAttributeInfo.StringValue.ToLower()))
            {
                styleAttributeInfo.StringValue = _dicColorInfo[styleAttributeInfo.StringValue.ToLower()];
            }
            return styleAttributeInfo.StringValue;
        }

        public void SimpleProperty(StyleAttribute styleAttributeInfo)
        {
            string value = DeleteSeperator(styleAttributeInfo.StringValue);
			switch (styleAttributeInfo.Name.ToLower())
			{
				case "column-fill":
				case "columns":
				case "page-break-before":
				case "page-break-after":
				case "page-break-inside":
				case "visibility":
				case "display":
				case "vertical-align":
				case "column-count":
				case "column-gap":
				case "text-transform":
				case "white-space":
				case "list-style-position":
				case "list-style-type":
				case "list-style":
				case "direction":
				case "float":
				case "clear":
				case "hyphens":
				case "hyphenate-before":
				case "hyphenate-after":
				case "hyphenate-lines":
				case "counter-reset":
				case "content":
				case "position":
				case "-ps-vertical-justification":
				case "-ps-fileproduce":
				case "prince-text-replace":
				case "-ps-referenceformat":
				case "-ps-positionchapternumbers-string":
				case "-ps-includeversenumber-string":
				case "-ps-includeverseinheaderreferences-string":
				case "ps-nonconsecutivereferenceseparator-string":
				case "-ps-nonconsecutivereferenceseparator-string":
				case "-ps-custom-footnote-caller":
				case "-ps-custom-xref-caller":
				case "-ps-hide-versenumber-one":
				case "-ps-hide-space-versenumber":
				case "prince-hyphenate-patterns":
				case "guideword-length":
				case "-ps-split-file-by-letter":
				case "-ps-center-title-header":
				case "-ps-header-font-size":
				case "top":
					_cssProperty[styleAttributeInfo.Name] = value;
					break;

				// convert to pt
				case "text-indent":
				case "margin-top":
				case "margin-right":
				case "margin-bottom":
				case "margin-left":
				case "class-margin-top":
				case "class-margin-right":
				case "class-margin-bottom":
				case "class-margin-left":
				case "padding-top":
				case "padding-bottom":
				case "padding-right":
				case "padding-left":
				case "line-height":
				case "letter-spacing":
				case "word-spacing":
				case "left":
				case "right":
				case "border-radius":
					_cssProperty[styleAttributeInfo.Name] = Common.UnitConverter(value);
					break;
				case "height":
				case "width":
				case "max-height":
					value = styleAttributeInfo.StringValue.ToLower() == "auto" ? "72" : Common.UnitConverter(value);
					_cssProperty[styleAttributeInfo.Name] = value;
					break;
				case "-ps-fixed-line-height":
				case "-ps-disable-widow-orphan":
				case "string-set":
				case "unicode-bidi":
				case "pathway":
				case "-webkit-column-count":
				case "overflow-wrap":
				case "-webkit-font-feature-settings":
				case "-moz-font-feature-settings":
				case "-ms-font-feature-settings":
					break;
				default:
					throw new Exception("Not a valid CSS Command: " + styleAttributeInfo.Name);
			}
        }

        #endregion Public Functions

        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Margin and Padding Values
        ///
        /// <list>
        /// </list>
        /// </summary>
        /// <param name="styleAttributeInfo">margin: 1cm 2cm 3cm 4cm</param>
        /// <returns>margin-top:1cm , margin-right:2cm, margin-bottom:3cm, margin-left:4cm </returns>
        /// -------------------------------------------------------------------------------------------

        private void Margin(StyleAttribute styleAttributeInfo)
        {

            string keyWord = styleAttributeInfo.Name;
            string top = keyWord + "-top";
            string right = keyWord + "-right";
            string bottom = keyWord + "-bottom";
            string left = keyWord + "-left";

            SetPropertyDimension(styleAttributeInfo, top, right, bottom, left);
        }

        private void SetPropertyDimension(StyleAttribute styleAttributeInfo, string top, string right, string bottom, string left)
        {
            try
            {
                ArrayList propertyValues = GetPropertyValue(styleAttributeInfo);

                if (propertyValues.Count == 1)
                {
                    _cssProperty[top] = propertyValues[0].ToString();
                    _cssProperty[right] = propertyValues[0].ToString();
                    _cssProperty[bottom] = propertyValues[0].ToString();
                    _cssProperty[left] = propertyValues[0].ToString();
                }
                else if (propertyValues.Count == 2)
                {
                    _cssProperty[top] = propertyValues[0].ToString();
                    _cssProperty[right] = propertyValues[1].ToString();
                    _cssProperty[bottom] = propertyValues[0].ToString();
                    _cssProperty[left] = propertyValues[1].ToString();
                }
                else if (propertyValues.Count == 3)
                {
                    _cssProperty[top] = propertyValues[0].ToString();
                    _cssProperty[right] = propertyValues[1].ToString();
                    _cssProperty[bottom] = propertyValues[2].ToString();
                    _cssProperty[left] = propertyValues[1].ToString();
                }
                else if (propertyValues.Count == 4)
                {
                    _cssProperty[top] = propertyValues[0].ToString();
                    _cssProperty[right] = propertyValues[1].ToString();
                    _cssProperty[bottom] = propertyValues[2].ToString();
                    _cssProperty[left] = propertyValues[3].ToString();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                _cssProperty[right] = "0";
                _cssProperty[bottom] = "0";
                _cssProperty[left] = "0";
                _cssProperty[top] = "0";
                throw new Exception(ex.Message);
            }
        }

        public ArrayList GetPropertyValue(StyleAttribute styleAttributeInfo)
        {
            var ParamList = new ArrayList();
            string[] parameters = styleAttributeInfo.StringValue.Split(',');
            int length = parameters.Length;
            for (int i = 0; i < length; i++)
            {
                string value = parameters[i];
                if (!Common.ValidateNumber(value))
                    continue;
                string unit = "pt";
                if (i < length - 1)
                {
                    string nextVal = parameters[i + 1];
                    if (_unit.Contains(nextVal))
                    {
                        unit = nextVal;
                        i++;
                    }
                }
                ParamList.Add(Common.UnitConverter(value + unit));

            }
            return ParamList;
        }

        public void Font(StyleAttribute styleAttributeInfo)
        {
            try
            {
                string[] parameters = styleAttributeInfo.StringValue.Split(',');
                // font-size: xx-small | x-small | small | medium | large | x-large | xx-large / pt / em / %
                // line-height:
                string param;
                int fontsizeBegin = -1;
                int fontsizeEnd = -1;

                for (int counter = 0; counter < parameters.Length; counter++)
                {
                    param = parameters[counter];
                    if (_unit.Contains(param))
                    {
                        if (counter - 1 >= 0)
                        {
                            string val = parameters[counter - 1];
                            string unit = parameters[counter];
                            string fontSize = Common.UnitConverter(val + unit);

                            if (!_cssProperty.ContainsKey("font-size"))
                            {
                                _cssProperty["font-size"] = fontSize;
                                fontsizeBegin = counter - 2;
                            }
                            else
                            {
                                _cssProperty["line-height"] = fontSize;
                            }
                            fontsizeEnd = counter; // for font-family
                        }
                    }
                    else if (_dictFontSize.ContainsKey(param))
                    {
                        _cssProperty["font-size"] = param;
                        if (fontsizeBegin == -1)
                        {
                            fontsizeBegin = counter - 1;
                        }
                        fontsizeEnd = counter; // for font-family
                    }
                }

                // font-family "New Century Schoolbook", serif  (or) sans-serif
                string fontFaceClean = string.Empty;
                for (int counter = fontsizeEnd + 1; counter < parameters.Length; counter++)
                {
                    param = parameters[counter];
                    fontFaceClean = fontFaceClean + "," + param;
                }

                fontFaceClean = fontFaceClean.Substring(1);

                StyleAttribute fontFamily = new StyleAttribute();
                fontFamily.Name = "font-family";
                fontFamily.StringValue = fontFaceClean;
                FontFamily(fontFamily);

                // font-style: italic,  font-variant: small-caps,
                // font-weight: normal | bold | bolder | lighter | 100 | 200 | 300 | 400 | 500 | 600 | 700 | 800 | 900
                for (int counter = 0; counter <= fontsizeBegin; counter++)
                {
                    param = parameters[counter];
                    if (param == "italic" || param == "oblique")
                    {
                        _cssProperty["font-style"] = param;
                    }
                    else if (param == "small-caps")
                    {
                        _cssProperty["font-variant"] = param;
                    }
                    else if (param == "bold" || param == "bolder" || param == "lighter")
                    {
                        _cssProperty["font-weight"] = param;
                    }
                    else if (Common.ValidateNumber(param))
                    {
                        _cssProperty["font-weight"] = param;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);

            }
        }

        private void FontFamily(StyleAttribute styleAttributeInfo)
        {
            string[] font = styleAttributeInfo.StringValue.Split(',');
            int fontLength = font.Length;
            if (fontLength == 0 || styleAttributeInfo.StringValueLower == "inherit")
            {
                return;
            }

            string fontName; // Gentium
            FontFamily[] systemFontList = System.Drawing.FontFamily.Families;
            for (int counter = 0; counter < fontLength; counter++)
            {
                fontName = font[counter].Replace("\"", "").Trim();
	            fontName = fontName.Replace("'", "");
                foreach (FontFamily systemFont in systemFontList)
                {
                    if (fontName.ToLower() == systemFont.Name.ToLower())
                    {
                        _cssProperty[styleAttributeInfo.Name] = fontName;
                        return;
                    }
                }
            }
            ArrayList genericFamilyList = new ArrayList(new[] { "serif", "sans-serif", "cursive", "fantasy", "monospace" });
            fontName = font[0];
            for (int i = 0; i < fontLength; i++)
            {
                fontName = font[i].Replace("<", "");
                fontName = fontName.Replace(">", "");
                fontName = fontName.Replace("default", "").Trim();

                if (genericFamilyList.Contains(fontName.ToLower()))
                {
                    string xmlFileNameWithPath = Common.PathCombine(PsSupportPath, "GenericFont.xml");

                    string xPath = "//font-preference/generic-family [@name = \"" + fontName.ToLower() + "\"]";
                    ArrayList fontList = new ArrayList();
                    fontList = Common.GetXmlNodeList(xmlFileNameWithPath, xPath);
                    if (fontList.Count > 0)
                    {
                        fontName = fontList[0].ToString();
                        break;
                    }
                }
            }

            _cssProperty["font-family"] = fontName.Trim();

        }

		private void FontFeatureSettings(StyleAttribute styleAttributeInfo)
		{
			string fontFeatureProperyValue = styleAttributeInfo.StringValue;
			int fontLength = fontFeatureProperyValue.Length;
			if (fontLength == 0 || styleAttributeInfo.StringValueLower == "inherit")
			{
				return;
			}

			_cssProperty[styleAttributeInfo.Name] = fontFeatureProperyValue;
			return;
		}

        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Delete seperators
        ///
        /// <list>
        /// </list>
        /// </summary>
        /// <param name="AttributeStringValue">attribute value like rgb(47,96,255)</param>
        /// <returns>"#ff0000" format data</returns>
        /// -------------------------------------------------------------------------------------------
        public string DeleteSeperator(string AttributeStringValue)
        {
            try
            {
                string value = AttributeStringValue.Replace(",", "");
                return (value);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return AttributeStringValue;
            }
        }

        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// For include space between seperators - BorderMethod1 bottom
        ///
        /// <list>
        /// </list>
        /// </summary>
        /// <param name="attribute">attribute value like border-bottom: .5pt solid #a00</param>
        /// <returns>".5pt solid #a00 </returns>
        /// -------------------------------------------------------------------------------------------
        private void BorderMethod1(StyleAttribute attribute)
        {
            string borderStyle = null;
            string borderWidth = null;
            string borderColor = null;
            ArrayList borderStyleList = CreateBorderStyleList();
            try
            {
                string[] value = attribute.StringValue.Split(',');

                for (int i = 0; i < value.Length; i++)
                {
                    if (borderStyleList.Contains(value[i]))
                    {
                        borderStyle = value[i];
                    }
                    else if (_unit.Contains(value[i]))
                    {
                        if (i - 1 >= 0)
                        {
                            string val = value[i - 1];
                            string unit = value[i];
                            borderWidth = Common.UnitConverter(val + unit);
                        }
                    }
                    else if (value[i].IndexOf('#') >= 0)  // #f00 conversion to #ff0000
                    {
                        borderColor = ColorHash(value[i]);
                        GetBorderColorList(borderColor);
                    }
                    else if (_dicColorInfo.ContainsKey(value[i])) // red conversion to #ff0000
                    {
                        borderColor = _dicColorInfo[value[i]];
                    }

                    else if (value[i].ToLower().IndexOf("rgb") >= 0)  // rgb,(,255,0,0,) conversion to #ff0000
                    {
                        string rgbConcat = GetRgbConcat(i, value);
                        borderColor = ColorRGB(rgbConcat);
                    }
                }

                if (attribute.Name == "border")
                {
                    BorderAssign(attribute.Name + "-top", borderStyle, borderWidth, borderColor);
                    BorderAssign(attribute.Name + "-right", borderStyle, borderWidth, borderColor);
                    BorderAssign(attribute.Name + "-bottom", borderStyle, borderWidth, borderColor);
                    BorderAssign(attribute.Name + "-left", borderStyle, borderWidth, borderColor);
                }
                else
                {
                    BorderAssign(attribute.Name, borderStyle, borderWidth, borderColor);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private ArrayList CreateBorderStyleList()
        {
            ArrayList borderStyleList = new ArrayList();
            borderStyleList.Add("none");
            borderStyleList.Add("hidden");
            borderStyleList.Add("dotted");
            borderStyleList.Add("dashed");
            borderStyleList.Add("solid");
            borderStyleList.Add("double");
            borderStyleList.Add("groove");
            borderStyleList.Add("ridge");
            borderStyleList.Add("inset");
            borderStyleList.Add("outset");
            return borderStyleList;
        }

        private void BorderAssign(string attributeName, string borderStyle, string borderWidth, string borderColor)
        {
            if (borderStyle != null)
            {
                _cssProperty[attributeName + "-style"] = borderStyle;
            }
            else
            {
                return; // No property should be added.
            }

            if (borderWidth == null)
            {
                borderWidth = ".5"; // set default to .5pt
            }
            _cssProperty[attributeName + "-width"] = borderWidth;

            if (borderColor == null)
            {
                borderColor = "#000000"; // set default to black color.
            }
            _cssProperty[attributeName + "-color"] = borderColor;
            GetBorderColorList(borderColor);
        }

        private string GetRgbConcat(int i, string[] value)
        {
            string rgbConcat = "";
            int counter = 0;
            for (int j = i; j < i + 6; j++)
            {
                if (counter != 5)
                {
                    rgbConcat = rgbConcat + value[j] + ",";
                    value[j] = string.Empty;
                }
                else
                {
                    rgbConcat = rgbConcat + value[j];
                    value[j] = string.Empty;
                }

                counter++;
            }
            return rgbConcat;
        }

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
        public string ColorRGB(string attributeStringValue)
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

        public string ColorHash(string attributeStringValue)
        {
            try
            {
                string retValue = attributeStringValue;
                int colorLen = attributeStringValue.Length;
                if (colorLen == 4)
                {
                    retValue = "#" + attributeStringValue[1] + attributeStringValue[1] + attributeStringValue[2] + attributeStringValue[2] + attributeStringValue[3] + attributeStringValue[3];
                }
                if (colorLen == 3)
                {
                    retValue = "#" + attributeStringValue[1] + attributeStringValue[2] + "0000";
                }
                if (colorLen == 5)
                {
                    retValue = "#" + attributeStringValue[1] + attributeStringValue[2] + attributeStringValue[3] + attributeStringValue[4] + "00";
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
        public string ColorCMYK(string attributeStringValue)
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
				Console.WriteLine(ex.Message);
                throw new Exception("Parameter Length - Not Valid");
            }
        }

    }
}
