// --------------------------------------------------------------------------------------------
// <copyright file="MapProperty.cs" from='2009' to='2009' company='SIL International'>
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
// Maps all property of Css 
// </remarks>
// --------------------------------------------------------------------------------------------

#region Using
using System;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using SIL.PublishingSolution;
using SIL.Tool;

#endregion Using
namespace SIL.PublishingSolution
{
    #region Class MapProperty
    public class MapProperty
    {
        #region Private Variable
        Dictionary<string, string> _dicAttributeInfo;
        readonly Dictionary<string, string> _dicColorInfo = new Dictionary<string, string>();
        readonly Dictionary<string, string> _dictFontSize = new Dictionary<string, string>();
        readonly ArrayList _arrUnits = new ArrayList();
        private readonly VerboseClass _verboseWriter = VerboseClass.GetInstance();
        #endregion

        #region Constructor Function
        public MapProperty()
        {
            _arrUnits.Add("em");
            _arrUnits.Add("ex");
            _arrUnits.Add("px");
            _arrUnits.Add("in");
            _arrUnits.Add("cm");
            _arrUnits.Add("mm");
            _arrUnits.Add("pt");
            _arrUnits.Add("pc");

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

            _dictFontSize.Add("xx-small", "6.6pt");
            _dictFontSize.Add("x-small", "7.5pt");
            _dictFontSize.Add("small", "10pt");
            _dictFontSize.Add("medium", "12pt");
            _dictFontSize.Add("large", "14pt");
            _dictFontSize.Add("x-large", "18pt");
            _dictFontSize.Add("xx-large", "24pt");
        }
        #endregion

        #region Public Functions
        //StyleAttribute x = new StyleAttribute();
        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Generate Open Office compatible CLASS attributes
        /// 
        /// <list>
        /// </list>
        /// </summary>
        /// <param name="attributeInfo">attribute name and attribute value</param>
        /// <returns>attributeInfo, as referenece object</returns>
        /// -------------------------------------------------------------------------------------------
        public StyleAttribute MapSingleValue(StyleAttribute attributeInfo)
        {
            try
            {
                string attribValue = attributeInfo.StringValue;
                string value = DeleteSeperator(attributeInfo.StringValue);

                if (attributeInfo.StringValue == "inherit")
                {
                    attributeInfo.Name = "";
                    attributeInfo.StringValue = "";
                    return attributeInfo;
                }

                switch (attributeInfo.Name.ToLower())
                {
                    case "text-decoration":
                        {
                            if (attributeInfo.StringValue == "underline")
                            {
                                attributeInfo.Name = "text-underline-style";
                                attributeInfo.StringValue = "solid";
                            }
                            else if (attributeInfo.StringValue == "line-through")
                            {
                                attributeInfo.Name = "text-line-through-style";
                                attributeInfo.StringValue = "solid";
                            }
                            else if (attributeInfo.StringValue == "none")
                            {
                                attributeInfo.Name = "text-underline-style";
                                attributeInfo.StringValue = "none";
                            }
                            else if (attributeInfo.StringValue == "overline" || attributeInfo.StringValue == "blink")
                            {
                            }
                            else
                            {
                                throw new Exception("Input is not valid");
                            }

                            break;
                        }
                    case "color":
                    case "background-color":
                        {
                            if (attributeInfo.StringValue.IndexOf("rgb") >= 0)
                            {
                                attributeInfo.StringValue = ColorRGB(attributeInfo.StringValue);
                            }
                            else if (attributeInfo.StringValue.IndexOf("#") >= 0)
                            {
                                attributeInfo.StringValue = ColorHash(attributeInfo.StringValue);
                            }
                            else if (attributeInfo.StringValue.IndexOf("cmyk") >= 0)
                            {
                                attributeInfo.StringValue = ColorCMYK(attributeInfo.StringValue);
                                attributeInfo.StringValue = ColorRGB(attributeInfo.StringValue);
                            }
                            else if (_dicColorInfo.ContainsKey(attributeInfo.StringValue.ToLower()))
                            {
                                attributeInfo.StringValue = _dicColorInfo[attributeInfo.StringValue.ToLower()];
                            }
                            break;
                        }
                    case "font-family":
                        {
                            string PsSupportPath = Common.GetPSApplicationPath();
                            string[] font = attributeInfo.StringValue.Split(',');
                            string familyName = string.Empty;
                            int fontLength = font.Length;
                            if (fontLength == 0 || attributeInfo.StringValueLower == "inherit")
                            {
                                attributeInfo.Name = "";
                                break;
                            }

                            //var familyName = new[] { "serif", "sans-serif", "cursive", "fantasy", "monospace" };
                            string fontName = string.Empty;
                            FontFamily[] systemFontList = System.Drawing.FontFamily.Families;
                            for (int counter = 0; counter < fontLength; counter++)
                            {
                                fontName = font[counter].Replace("\"", "").Trim();
                                foreach (FontFamily systemFont in systemFontList)
                                {
                                    if (fontName.ToLower() == systemFont.Name.ToLower())
                                    {
                                        attributeInfo.StringValue = systemFont.Name;
                                        return attributeInfo;
                                    }
                                }
                            }

                            if (font[0].Length > 0)
                                _verboseWriter.WriteError(attributeInfo.ClassName, attributeInfo.Name, "Missing Font",
                                                          font[0].Replace("\"", "").Replace("'", ""));

                            

                            string genericFamily = font[fontLength - 1];
                            genericFamily = genericFamily.Replace("\"", "").Trim().ToLower();
                            ArrayList genericFamilyList = new ArrayList(new[] { "serif", "sans-serif", "cursive", "fantasy", "monospace" });

                            fontName = font[0];
                            if (genericFamilyList.Contains(genericFamily))
                            {
                                //string xmlFileNameWithPath = Common.PathCombine(Common.GetPSApplicationPath(), "GenericFont.xml");
                                string xmlFileNameWithPath = Common.PathCombine(PsSupportPath, "GenericFont.xml");
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

                            attributeInfo.StringValue = fontName.Trim();
                            break;
                        }

                    case "margin-top":
                    case "margin-bottom":
                    case "margin-right":
                    case "margin-left":
                    case "padding-top":
                    case "padding-bottom":
                    case "padding-right":
                    case "padding-left":
                        {

                            //string[] splitValues = attribValue.Split(',');
                            //StyleAttribute attributeInfo = UnitConversion(m_splitValues[0].ToString(), splitValues[1].ToString(), attributeInfo.Name);
                            //attributeInfo.StringValue = attributeInfo.StringValue + attributeInfo.Name;
                            attributeInfo.StringValue = value;
                            break;
                        }
                    case "font-size":
                        {
                            if (_dictFontSize.ContainsKey(attributeInfo.StringValue))
                            {
                                attributeInfo.StringValue = _dictFontSize[attributeInfo.StringValue];
                            }
                            else if(attributeInfo.StringValue == "normal")
                            {
                                attributeInfo.Name = "";
                            }
                            else if (attributeInfo.StringValue == "smaller" || attributeInfo.StringValue == "larger")
                            {
                                attributeInfo.StringValue = value;
                            }
                            else
                            {
                                attributeInfo.StringValue = value;
                                attributeInfo.Name = CheckInput(attribValue, attributeInfo.Name);
                            }
                            break;
                        }
                    case "text-indent":
                        {
                            attributeInfo.Name = "text-indent";
                            attributeInfo.StringValue = SplitSeperator(attributeInfo.StringValue);
                            attributeInfo.Name = CheckInput(attribValue, attributeInfo.Name);
                            break;
                        }
                    case "border-top":
                    case "border-bottom":
                    case "border-left":
                    case "border-right":
                        {
                            //attributeInfo.Name = "border-bottom";
                            attributeInfo.StringValue = SplitSeperator(attributeInfo.StringValue);
                            break;
                        }
                    case "column-fill":
                        {
                            attributeInfo.Name = "dont-balance-text-columns";
                            if (attributeInfo.StringValue == "auto")
                            {
                                attributeInfo.StringValue = "true";
                            }
                            else if (attributeInfo.StringValue == "balance")
                            {
                                attributeInfo.StringValue = "false";
                            }
                            else
                            {
                                attributeInfo.Name = "column-fill";
                                throw new Exception("Not a Valid input");
                            }
                            break;
                        }
                    case "columns":
                        {
                            attributeInfo.Name = "column-count";
                            break;
                        }
                    case "page-break-before":
                        {
                            attributeInfo.Name = "break-before";
                            if (attributeInfo.StringValue == "always")
                            {
                                attributeInfo.StringValue = "page";
                            }
                            else if (attributeInfo.StringValue == "auto" || attributeInfo.StringValue == "avoid")
                            {
                            }
                            else
                            {
                                attributeInfo.Name = "page-break-before";
                                throw new Exception("Not a Valid input");
                            }
                            break;
                        }
                    case "page-break-after":
                        {
                            if (attributeInfo.StringValue == "always")
                            {
                                attributeInfo.Name = "break-after";
                                attributeInfo.StringValue = "page";
                            }
                            else if (attributeInfo.StringValue == "avoid")
                            {
                                attributeInfo.Name = "keep-with-next";
                                attributeInfo.StringValue = "always";
                            }
                            else if (attributeInfo.StringValue == "auto")
                            {
                            }
                            else
                            {
                                attributeInfo.Name = "page-break-after";
                                throw new Exception("Not a Valid input");
                            }
                            break;
                        }
                    case "page-break-inside":
                        {
                            if (attributeInfo.StringValue == "avoid")
                            {
                                attributeInfo.Name = "keep-together";
                                attributeInfo.StringValue = "always";
                            }
                            else if (attributeInfo.StringValue == "auto")
                            {
                            }
                            else
                            {
                                attributeInfo.Name = "page-break-inside";
                                throw new Exception("Not a Valid input");
                            }
                            break;
                        }
                    case "font-weight":
                        {
                            attributeInfo.Name = "font-weight";
                            if (attributeInfo.StringValue == "bold")
                            {
                                attributeInfo.StringValue = "700";
                            }
                            else if (attributeInfo.StringValue == "normal")
                            {
                                attributeInfo.StringValue = "400";
                            }
                            break;
                        }
                    //case "line-height":
                    //    {
                    //        attributeInfo.Name = "line-spacing";
                    //        bool lineHeight = true;
                    //        attributeInfo.StringValue = FontHeight(attributeInfo.StringValue, lineHeight);
                    //        if (attributeInfo.StringValue == null)
                    //        {
                    //            attributeInfo.Name = "";
                    //        }
                    //        break;
                    //    }
                    case "-ps-fixed-line-height":
                    case "line-height":
                        {
                            attributeInfo.Name = "line-height";
                            const bool lineHeight = false;
                            attributeInfo.StringValue = FontHeight(attributeInfo.StringValue, lineHeight);
                            if (attributeInfo.StringValue == null)
                            {
                                attributeInfo.Name = "";
                            }
                            break;
                        }
                    //case "visibility":
                    //    {
                    //        attributeInfo.Name = "color";
                    //        if (attributeInfo.StringValue == "hidden")
                            //{
                            //    attributeInfo.StringValue = "#000000";
                            //}
                            //else
                            //{
                            //    attributeInfo.Name = "visibility";
                            //    throw new Exception("Not a Valid input");
                            //}
                    case "display":
                        {
                            attributeInfo.Name = "display";
                            attributeInfo.StringValue = attributeInfo.StringValue == "none" ? "true" : "false";
                            break;
                        }
                    case "pathway":
                        {
                            attributeInfo.Name = "pathway";
                            attributeInfo.StringValue = attributeInfo.StringValue;
                            break;
                        }
                    case "vertical-align":
                        {
                            if (attributeInfo.StringValue == "super" || attributeInfo.StringValue == "sub")
                            {
                                attributeInfo.Name = "text-position";
                            }
                            else if (attributeInfo.StringValue == "text-top")
                            {
                                attributeInfo.StringValue = "top";
                            }
                            else if (attributeInfo.StringValue == "text-bottom")
                            {
                                attributeInfo.StringValue = "bottom";
                            }
                            else if (attributeInfo.StringValue == "middle"
                                || attributeInfo.StringValue == "top" || attributeInfo.StringValue == "bottom"
                                || attributeInfo.StringValue == "baseline")
                            {
                            }
                            else
                            {
                                throw new Exception("Input not valid");
                            }
                            break;
                        }

                    default:
                        {
							attributeInfo.StringValue = value;
                            switch (attributeInfo.Name)
                            {
                                case "text-align":
                                    {
                                        switch (attributeInfo.StringValue)
                                        {
                                            case "left":
                                            case "right":
                                            case "center":
                                            case "justify":
                                                break;

                                            default:
                                                throw new Exception("Not a valid command");
                                        }
                                    }
                                    break;

                                case "column-count":
                                    if (!Common.ValidateNumber(value))
                                    {
                                        throw new Exception("Numeric Value is expected");
                                    }
                                    break;


                                case "column-gap":
                                    {
                                        attributeInfo.Name = CheckInput(attribValue, attributeInfo.Name);
                                    }
                                    break;
                                case "text-transform":
                                    {
                                        switch (attributeInfo.StringValue)
                                        {
                                            case "capitalize":
                                            case "uppercase":
                                            case "lowercase":
                                            case "none":
                                                break;

                                            default:
                                                throw new Exception("Not a valid command");
                                        }

                                    }
                                    break;
                                case "direction":
                                    {
                                        if (attributeInfo.StringValue != "ltr" && attributeInfo.StringValue != "rtl")
                                        {
                                            throw new Exception("Input not valid");
                                        }
                                    }
                                    break;
                                case "white-space":
                                    {
                                        switch (attributeInfo.StringValue)
                                        {
                                            case "normal":
                                            case "pre":
                                            case "nowrap":
                                            case "pre-wrap":
                                            case "pre-line":
                                                break;

                                            default:
                                                throw new Exception("Not a valid command");
                                        }
                                    }
                                    break;

                                case "font-style":
                                    {
                                        switch (attributeInfo.StringValue.ToLower())
                                        {
                                            case "normal":
                                            case "italic":
                                            case "oblique":
                                                break;
                                            default:
                                                throw new Exception("Not a valid command");
                                        }
                                    }
                                    break;

                                case "font-variant":
                                    {
                                        switch (attributeInfo.StringValue)
                                        {
                                            case "normal":
                                            case "small-caps":
                                                break;
                                            default:
                                                throw new Exception("Not a valid command");
                                        }
                                    }
                                    break;

                                //TODO - for error handling
                               case "counter-increment":

                                case "letter-spacing":
                                case "word-spacing":
                                case "content":
                                case "string-set":
                                case "flow":
                                case "hyphens":
                                case "hyphenate-before":
                                case "hyphenate-after":
                                case "hyphenate-lines":
                                case "marks":
                                case "height":
                                case "width":
                                case "-ps-basicsFirstBook-string":
                                case "-ps-basicsLastBook-string":
                                case "-ps-IncludeVerseInHeaderReferences-string":
                                case "-ps-positionchapternumbers-string":
                                case "-ps-IncludeVersenumber-string":
                                case "-ps-BasicsVerticalRuleinGutter":
                                case "-ps-NonConsecutiveReferenceSeparator-string":
                                case "ps-NonConsecutiveReferenceSeparator-string":
                                case "prince-hyphenate-patterns":
                                case "showerror":
                                case "-ps-paper-size":
                                case "-ps-vertical-justification":
                                case "visibility":
                                    break;

                                default:
                                    throw new Exception("Not a valid CSS Command");
                            }
                            break;
                        }
                }
                return attributeInfo;
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message.IndexOf("outside the bounds") > 0 ? "Parameter Length - Not Valid" : ex.Message;
                //StylesXML.WriteError(attributeInfo.ClassName, attributeInfo.Name, errorMessage);
                _verboseWriter.WriteError(attributeInfo.ClassName, attributeInfo.Name, errorMessage, attributeInfo.StringValue);
                //Console.Write(ex.Message);
                return attributeInfo;
            }
        }
        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Generate Open Office compatible NON - CLASS attributes
        /// 
        /// <list>
        /// </list>
        /// </summary>
        /// <param name="attributeInfo">attribute name and attribute value</param>
        /// <returns>attributeInfo, as referenece object</returns>
        /// -------------------------------------------------------------------------------------------
        public Dictionary<string, string> MapMultipleValue(StyleAttribute attributeInfo)
        {
            _dicAttributeInfo = new Dictionary<string, string>();
            try
            {
                string attribValue = attributeInfo.StringValue;
                switch (attributeInfo.Name)
                {
                    case "margin":
                    case "padding":
                        {
                            MarginAndPaddingValue(attributeInfo.StringValue, attributeInfo.Name);
                            break;
                        }
                    case "border":
                        {
                            attributeInfo.StringValue = SplitSeperator(attributeInfo.StringValue);
                            _dicAttributeInfo.Add("border-top", attributeInfo.StringValue);
                            _dicAttributeInfo.Add("border-bottom", attributeInfo.StringValue);
                            _dicAttributeInfo.Add("border-left", attributeInfo.StringValue);
                            _dicAttributeInfo.Add("border-right", attributeInfo.StringValue);
                            break;
                        }
                    case "column-rule":
                        {
                            attributeInfo.Name = "column-sep";
                            attributeInfo.StringValue = SplitSeperator(attributeInfo.StringValue);
                            ColumnRuleValue(attributeInfo.StringValue);
                            StylesXML.HeaderRule = attributeInfo.StringValue;
                            break;
                        }
                    case "font":
                        {
                            attributeInfo.Name = "font";
                            bool valid = Font(attribValue);
                            if (!valid)
                            {
                                attributeInfo.Name = "";
                            }
                            break;
                        }
                    case "size":
                        {
                            string[] parameters = attributeInfo.StringValue.Split(',');
                            _dicAttributeInfo.Add("page-width", parameters[0] + parameters[1]);
                            _dicAttributeInfo.Add("page-height", parameters[2] + parameters[3]);
                            break;
                        }
                    case "language":
                        {
                            string[] languageCountrySplit = attributeInfo.StringValue.Split('-');
                            string language = languageCountrySplit[0];
                            _dicAttributeInfo.Add("language", language);

                            string country;
                            if (languageCountrySplit.Length > 1)
                            {
                                country = languageCountrySplit[1];
                                _dicAttributeInfo.Add("country", country);
                            }
                            else // find country for english language
                            {
                                if (language.ToLower() == "en")
                                {
                                    string[] langCountry = Application.CurrentCulture.IetfLanguageTag.Split('-');
                                    country = langCountry[1];
                                    _dicAttributeInfo.Add("country", country);
                                }
                            }
                            break;
                        }
                    case "direction":
                        {
                            if (attributeInfo.StringValue == "rtl")
                            {
                                _dicAttributeInfo.Add("writing-mode", "rl-tb");
                                _dicAttributeInfo.Add("text-align", "end");
                            }
                            else if (attributeInfo.StringValue == "ltr")
                            {
                                _dicAttributeInfo.Add("writing-mode", "lr-tb");                               
                            }
                            break;
                        }
                    case "list-style-position":
                        {
                            if (attributeInfo.StringValue == "inside")
                            {
                                _dicAttributeInfo.Add("margin-left", "0.25in");
                                _dicAttributeInfo.Add("margin-right", "0in");
                                _dicAttributeInfo.Add("text-indent", "-0.25in");
                                _dicAttributeInfo.Add("auto-text-indent", "false");
                            }
                            break;
                        }
                    default:
                        {
                            //attributeInfo.StringValue = value;
                            break;
                        }

                }
                return _dicAttributeInfo;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                //StylesXML.WriteError(attributeInfo.ClassName, attributeInfo.Name, ex.Message);
                _verboseWriter.WriteError(attributeInfo.ClassName, attributeInfo.Name, ex.Message, attributeInfo.StringValue);
                return _dicAttributeInfo;
            }
        }

        #endregion Public Functions

        #region Private Functions
        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Margin and Padding Values 
        /// 
        /// <list>
        /// </list>
        /// </summary>
        /// <param name="AttributeStringValue">margin: 1cm 2cm 3cm 4cm</param>
        /// <param name="m_keyWord">Keyword</param>
        /// <returns>margin-top:1cm , margin-right:2cm, margin-bottom:3cm, margin-left:4cm </returns>
        /// -------------------------------------------------------------------------------------------

        private void MarginAndPaddingValue(string AttributeStringValue, string keyWord)
        {
            int count = 0;
            string attribVal = "";
            string top = keyWord + "-top";
            string right = keyWord + "-right";
            string bottom = keyWord + "-bottom";
            string left = keyWord + "-left";
            string[] attName = { top, right, bottom, left };
            int j = 0;
            try
            {
                int attribCount = 0;
                var ParamList = new ArrayList();
                string[] parameters = AttributeStringValue.Split(',');
                for (int i = 0; i < parameters.Length; i++)
                {
                    if (i == parameters.Length - 1 || !_arrUnits.Contains(parameters[i + 1]))
                    {
                        ParamList.Add("0");
                        ParamList.Add("pt");
                    }
                    else
                    {
                        ParamList.Add(parameters[i]);
                        ParamList.Add(parameters[i + 1]);
                        i++;
                    }
                    j++;
                }

                foreach (string param in ParamList)
                {
                    bool validate;
                    if (count%2 == 0)
                    {
                        attribVal = param;
                        validate = Common.ValidateNumber(param);
                    }
                    else
                    {
                        if (!_arrUnits.Contains(param))
                        {
                            throw new Exception("Unit is not valid");
                        }
                        if (StylesXML.IsCropMarkChecked && !StylesXML.IsMarginChanged)
                        {
                            double value = 0.0;
                            if (param == "in")
                            {
                                value = double.Parse(attribVal) + double.Parse("0.50");
                            }
                            else if (param == "cm")
                            {
                                value = double.Parse(attribVal) + double.Parse("1.27");
                            }
                            else if (param == "pt")
                            {
                                value = double.Parse(attribVal);
                            }
                            attribVal = value.ToString();

                        }
                        _dicAttributeInfo.Add(attName[attribCount], attribVal + param);
                        validate = Common.ValidateAlphabets(param);
                        attribCount++;
                    }
                    count++;
                    //To do Greg clarification
                    if (validate == false)
                    {
                        throw new Exception("Not a valid numeric value");
                    }
                }
                if (attribCount == 1)
                {
                    _dicAttributeInfo.Add(right, _dicAttributeInfo[top]);
                    _dicAttributeInfo.Add(bottom, _dicAttributeInfo[top]);
                    _dicAttributeInfo.Add(left, _dicAttributeInfo[top]);
                }
                else if (attribCount == 2)
                {
                    _dicAttributeInfo.Add(bottom, _dicAttributeInfo[top]);
                    _dicAttributeInfo.Add(left, _dicAttributeInfo[right]);
                }
                else if (attribCount == 3)
                {
                    _dicAttributeInfo.Add(left, _dicAttributeInfo[right]);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                _dicAttributeInfo[right] = "0pt";
                _dicAttributeInfo[bottom] = "0pt";
                _dicAttributeInfo[left] = "0pt";
                _dicAttributeInfo[top] = "0pt";
                throw new Exception(ex.Message);

            }
        }

        private bool Font(string AttributeStringValue)
        {
            try
            {
                string[] parameters = AttributeStringValue.Split(',');
                var fontsizeList = new ArrayList {"xx-small", "small", "medium", "large", "x-large", "xx-large"};

                // font-size: xx-small | x-small | small | medium | large | x-large | xx-large / pt / em / %
                // line-height:
                string prevValue = "12", param;
                string attribName = "font-size";

                int fontsizeBegin = -1;
                int fontsizeEnd = -1;

                for (int counter = 0; counter < parameters.Length; counter++)
                {
                    param = parameters[counter];
                    if (param == "pt" || param == "em" || param == "%")
                    {
                        _dicAttributeInfo.Add(attribName, prevValue + param);
                        attribName = "line-height";
                        if (fontsizeBegin == -1)
                        {
                            fontsizeBegin = counter - 2;
                        }
                        fontsizeEnd = counter; // for font-family
                    }
                    else if (fontsizeList.Contains(param))
                    {
                        _dicAttributeInfo.Add(attribName, param);
                        attribName = "line-height";
                        if (fontsizeBegin == -1)
                        {
                            fontsizeBegin = counter - 1;
                        }
                        fontsizeEnd = counter; // for font-family
                    }
                    prevValue = param;
                }

                if (fontsizeEnd == -1)
                {
                    return false;
                }

                // font-family "New Century Schoolbook", serif  (or) sans-serif
                string fontFaceClean = string.Empty;
                for (int counter = fontsizeEnd + 1; counter < parameters.Length; counter++)
                {
                    param = parameters[counter];
                    fontFaceClean = fontFaceClean + "," + param;
                }

                if (fontFaceClean == string.Empty)
                {
                    return false;
                }
                fontFaceClean = fontFaceClean.Substring(1);

                _dicAttributeInfo.Add("font-family", fontFaceClean);

                // font-style: italic,  font-variant: small-caps,  
                // font-weight: normal | bold | bolder | lighter | 100 | 200 | 300 | 400 | 500 | 600 | 700 | 800 | 900 
                for (int counter = 0; counter <= fontsizeBegin; counter++)
                {
                    param = parameters[counter];
                    if (param == "italic")
                    {
                        _dicAttributeInfo.Add("font-style", param);
                    }
                    else if (param == "small-caps")
                    {
                        _dicAttributeInfo.Add("font-variant", param);
                    }
                    else if (param == "bold" || param == "bolder" || param == "lighter")
                    {
                        _dicAttributeInfo.Add("font-weight", param);
                    }
                    else if (Common.ValidateNumber(param))
                    {
                        _dicAttributeInfo.Add("font-weight", param);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return false;
            }
        }
        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Handle ColumnRule
        /// 
        /// <list>
        /// </list>
        /// </summary>
        /// <param name="AttributeStringValue">attribute value like rgb(47,96,255)</param>
        /// <returns>"#ff0000" format data</returns>
        /// -------------------------------------------------------------------------------------------
        private void ColumnRuleValue(string AttributeStringValue)
        {
            try
            {
                string[] attValues = AttributeStringValue.Split(' ');
                foreach (string value in attValues)
                {
                    if (value.IndexOf("pt") >= 0)
                    {
                        _dicAttributeInfo.Add("style:width", value);
                    }
                    else if (value.IndexOf("#") >= 0)
                    {
                        _dicAttributeInfo.Add("style:color", value);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
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
        private static string DeleteSeperator(string AttributeStringValue)
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
        /// For include space between seperators - Border bottom
        /// 
        /// <list>
        /// </list>
        /// </summary>
        /// <param name="AttributeStringValue">attribute value like border-bottom: .5pt solid #a00</param>
        /// <returns>".5pt solid #a00 </returns>
        /// -------------------------------------------------------------------------------------------
        private string SplitSeperator(string attributeStringValue)
        {
            string output = "";
            try
            {
                string[] value = attributeStringValue.Split(',');
                for (int i = 0; i < value.Length; i++)
                {
                    //if (m_value[i].ToLower() == "pc")  // picas conversion to pt
                    //{
                    //    StyleAttribute getValue = UnitConversion(m_value[i - 1], value[i], AttributeStringName);
                    //    value[i - 1] = getValue.StringValue;
                    //    value[i] = getValue.Name;
                    //}
                    //else 
                    if (value[i].IndexOf('#') >= 0)  // #f00 conversion to #ff0000
                    {
                        value[i] = ColorHash(value[i]);
                    }
                    else if (_dicColorInfo.ContainsKey(value[i])) // red conversion to #ff0000
                    {
                        value[i] = _dicColorInfo[value[i]];
                    }
                    else if (value[i].ToLower().IndexOf("rgb") >= 0)  // rgb,(,255,0,0,) conversion to #ff0000
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
                        value[i] = ColorRGB(rgbConcat);
                    }
                }

                for (int i = 0; i < value.Length; i++)
                {
                    if (_arrUnits.Contains(value[i]) || i == 0)
                    {
                        output += value[i];
                    }
                    else
                    {
                        output += " " + value[i];
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.Write(ex.Message);
                throw new Exception(ex.Message);
            }
            return (output);
        }

        private static string FontHeight(string attributeStringValue, bool lineHeight)
        {
            string strVal;
            try
            {
                string[] attValues = attributeStringValue.Split(',');
                string unit;

                float newValue = float.Parse(attValues[0]);
                if (attValues.Length > 1)
                {
                    unit = attValues[1];
                    if (unit == "em")
                    {
                        newValue -= 1;
                    }
                    else if (unit == "%")
                    {
                        newValue /= 100;
                        newValue -= 1;
                        unit = "em";
                        //m_strVal = newValue.ToString() + "-em";
                        //return (m_strVal);
                    }
                    else if (lineHeight && unit == "pt")
                    {
                        strVal = newValue + "-em";
                        return (strVal);
                    }
                }
                else
                {
                    unit = "em";
                    newValue = newValue - 1;
                }
                if (lineHeight)
                {
                    newValue = newValue / 2F;
                }
                strVal = newValue.ToString();
                strVal = strVal + unit;
            }
            catch (Exception ex)
            {
                strVal = null;
                Console.Write(ex.Message);
            }

            return (strVal);
        }

        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Converts rgb(47,96,255) to "#ff0000" format
        /// 
        /// <list>
        /// </list>
        /// </summary>
        /// <param name="AttributeStringValue">attribute value like rgb(47,96,255)</param>
        /// <returns>"#ff0000" format data</returns>
        /// -------------------------------------------------------------------------------------------
        private static string ColorRGB(string attributeStringValue)
        {
            //string StringValue = "rgb(125,255,255)";
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
        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Converts #f00 to "#ff0000" format
        /// 
        /// <list>
        /// </list>
        /// </summary>
        /// <param name="AttributeStringValue">attribute value like #f00</param>
        /// <returns>"#ff0000" format data</returns>
        /// -------------------------------------------------------------------------------------------
        //private string ColorHash(string StringValue)
        //{
        //    string retValue;
        //    int colorLen = StringValue.Length;
        //    if (colorLen == 4)
        //    {
        //        retValue = "#" + StringValue[1] + StringValue[1] + StringValue[2] + StringValue[2] + StringValue[3] + StringValue[3] ;
        //        return retValue;
        //    }
        //    else
        //    {
        //        return StringValue;
        //    }
        //}
        ///// -------------------------------------------------------------------------------------------

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

                double C = double.Parse(rgbValues[0]);
                double M = double.Parse(rgbValues[1]);
                double Y = double.Parse(rgbValues[2]);
                double K = double.Parse(rgbValues[3]);

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
                //return "rgb" + ",(," + "0" + "," + "0" + "," + "0" + ",)"; // black color
                throw new Exception("Parameter Length - Not Valid");
            }
        }
        /// <summary>
        /// Checks the input and returns Error if exist; ex: 12pt
        /// 
        /// <list>
        /// </list>
        /// </summary>
        /// <param name="m_attribValue">attribute value like 12pt </param>
        /// <param name="AttributeName">Attribute Name</param>
        /// <returns>"12pt"</returns>
        /// -------------------------------------------------------------------------------------------
        private string CheckInput(string attribValue, string attributeName)
        {
            if (attribValue.IndexOf(',') > 0)
            {
                string[] tempValues = attribValue.Split(',');
                if (!Common.ValidateNumber(tempValues[0]))
                {
                    throw new Exception("Numeric Value is expected");
                }

                if (tempValues[1] != "em" && tempValues[1] != "%")
                {
                    if (!_arrUnits.Contains(tempValues[1]))
                    {
                        throw new Exception("Unit is not valid");
                    }
                }
            }
            else
            {
                throw new Exception("Input is not valid");
            }
            return attributeName;
        }
        #endregion Private Functions
    }
    #endregion
}
