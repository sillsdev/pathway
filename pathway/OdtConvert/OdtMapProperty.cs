using System.Collections.Generic;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class OOMapProperty
    {
        private Dictionary<string, string> _IDProperty = new Dictionary<string, string>();
        private Dictionary<string, string> _cssProperty = new Dictionary<string, string>();
        //TextInfo _titleCase = CultureInfo.CurrentCulture.TextInfo;
        public Dictionary<string, string> IDProperty(Dictionary<string, string> cssProperty)
        {
            _IDProperty.Clear();
            _cssProperty = cssProperty;
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
                        TextDecoration(property.Value);
                        break;
                    case "font-variant":
                        FontVariant(property.Value);
                        break;
                    case "text-indent":
                        TextIndent(property.Value);
                        break;
                    case "margin-left":
                        MarginLeft(property.Value);
                        break;
                    case "margin-right":
                        MarginRight(property.Value);
                        break;
                    case "margin-top":
                        MarginTop(property.Value);
                        break;
                    case "margin-bottom":
                        MarginBottom(property.Value);
                        break;
                    case "font-family":
                        FontFamily(property.Value);
                        break;
                    case "width":
                        PageWidth(property.Value);
                        break;
                    case "height":
                        PageHeight(property.Value);
                        break;
                    case "mirror":
                        Mirror(property.Value);
                        break;
                    case "padding":
                    case "margin":
                        //Margin(styleAttributeInfo);
                        break;
                    case "color":
                    //case "background-color":
                        Color(property.Value);
                        break;
                    case "visibility":
                        Visibility(property.Value);
                        break;
                    case "size":
                        //Size(styleAttributeInfo);
                        break;
                    case "language":
                        //Language(styleAttributeInfo);
                        break;
                    case "border":
                        //Border(styleAttributeInfo);
                        break;
                    case "column-count":
                        ColumnCount(property.Value);
                        break;
                    case "column-gap":
                        ColumnGap(property.Value);
                        break;
                    default:
                        //SimpleProperty(styleAttributeInfo);
                        break;
                }
            }
            return _IDProperty;
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
            _IDProperty["Height"] = propertyValue;
        }

        public void PageWidth(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["Width"] = propertyValue;
        }

        public void MarginLeft(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["LeftIndent"] = propertyValue;
        }

        public void MarginRight(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["RightIndent"] = propertyValue;
        }

        public void MarginTop(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["Top"] = propertyValue;
        }

        public void MarginBottom(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["Bottom"] = propertyValue;
        }

        public void FontFamily(string propertyValue)
        {
            _IDProperty["AppliedFont"] = propertyValue;
        }
        public void TextIndent(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["FirstLineIndent"] = propertyValue;
        }

        public void Color(string propertyValue)
        {
            if (propertyValue == string.Empty || _cssProperty.ContainsValue("hidden"))
            {
                return;
            }
            _IDProperty["FillColor"] = "Color/" + propertyValue;
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

        public void Visibility(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }

            if (propertyValue == "hidden")
            {
                _IDProperty["FillColor"] = "Color/Paper";
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
                propertyValue = "Normal";
            }
            else if (propertyValue == "small-caps")
            {
                propertyValue = "SmallCaps";
            }
            _IDProperty["Capitalization"] = propertyValue;
        }

        public void TextDecoration(string propertyValue)
        {
            if (propertyValue == string.Empty || propertyValue == "inherit")
            {
                return;
            }

            if (propertyValue == "none")
            {
                propertyValue = "false";
            }
            else if (propertyValue == "underline")
            {
                propertyValue = "true";
            }
            _IDProperty["Underline"] = propertyValue;
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

            if (strValue == "normalnormal" || strValue == "normal")
            {
                propertyValue = "Regular";
            }
            else if (strValue == "boldnormal" || strValue == "bold")
            {
                propertyValue = "Bold";
            }
            else if (strValue == "normalitalic" || strValue == "italic")
            {
                propertyValue = "Italic";
            }
            else if (strValue == "bolditalic")
            {
                propertyValue = "Bold Italic";
            }

            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["FontStyle"] = propertyValue;
        }

        public void TextAlign(string propertyValue)
        {
            if (propertyValue == string.Empty || propertyValue == "inherit")
            {
                return;
            }

            if (propertyValue == "justify")
            {
                propertyValue = "FullyJustified";
            }
            else if (propertyValue == "center")
            {
                propertyValue = "CenterAlign";
            }
            else if (propertyValue == "left")
            {
                propertyValue = "LeftAlign";
            }
            else if (propertyValue == "right")
            {
                propertyValue = "RightAlign";
            }
            _IDProperty["Justification"] = propertyValue;
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

        
    }
}