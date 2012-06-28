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
	public class StyToXML
	{
	    public string StyFullPath { get; set; }
        private string _cssFullPath;
        private Dictionary<string, Dictionary<string, string>> _styleInfo = new Dictionary<string, Dictionary<string, string>>();
		private Dictionary<string, string> _cssProp;
        protected XmlTextWriter _writer;

		/// ------------------------------------------------------------
		/// <summary>
		/// Convert a Paratext sty file to a CSS.
		/// </summary>
		/// <param name="database">The settings for the Paratext database.
		/// </param>
		/// <param name="cssFullPath">The XML full path.</param>
        /// ------------------------------------------------------------
		public void ConvertStyToXML(string database, string cssFullPath)
        {

            //StyFullPath = styFullPath;
            _cssFullPath = cssFullPath;
            FindStyFile(database);
            CreateFile();
            ParseFile();
            WriteXML();
        }

        /// <summary>
        /// Override to convert the sty file to XML, assuming the
        /// sty file is already set for this class.
        /// </summary>
        /// <param name="cssFullPath"></param>
        public void ConvertStyToXML(string cssFullPath)
        {
            _cssFullPath = cssFullPath;
            CreateFile();
            ParseFile();
            WriteXML();
        }

		/// ------------------------------------------------------------
		/// <summary>
		/// Finds the sty file for a Paratext project.
		/// </summary>
		/// <param name="database">The settings for the Paratext database.
		/// </param>
		/// ------------------------------------------------------------
		private void FindStyFile(string database)
        {
            string ssfFile = database + ".ssf";
            string ssfFullPath = string.Empty;

            if (Common.GetOsName() == "Windows7")
            {
                ssfFullPath = Common.GetValueFromRegistry("SOFTWARE\\Wow6432Node\\ScrChecks\\1.0\\Settings_Directory", "");
            }
            else if (Common.GetOsName() == "Windows XP")
            {
                ssfFullPath = Common.GetValueFromRegistry("SOFTWARE\\ScrChecks\\1.0\\Settings_Directory", "");
            }
            else if (Common.GetOsName() == "Unix") // todo: add linux code
            {
                ssfFullPath = Common.GetValueFromRegistry("SOFTWARE\\ScrChecks\\1.0\\Settings_Directory", "");
            }

            ssfFullPath = ssfFullPath + ssfFile;
           
            bool isStylesheet = false;

            if(!File.Exists(ssfFullPath))
            {
                Debug.WriteLine(ssfFile + " does not exist.");
                return;
            }

            var reader = new XmlTextReader(ssfFullPath) { XmlResolver = null };
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "StyleSheet") // Is class name null
                {
                    isStylesheet = true;
                }
                else if (reader.NodeType == XmlNodeType.Text && isStylesheet)
                {
                    ssfFile = reader.Value;
                    break;
                }
            }
            reader.Close();
            StyFullPath = Common.PathCombine(Path.GetDirectoryName(ssfFullPath), ssfFile);
        }

		/// ------------------------------------------------------------------------
		/// <summary>
		/// Parses all the lines in an sty file converting the settings to 
		/// properties in a CSS.
		/// </summary>
	    /// ------------------------------------------------------------------------
		private void ParseFile()
        {
            _styleInfo.Clear();
            if(!File.Exists(StyFullPath))
            {
                Debug.WriteLine(StyFullPath + " does not exist.");
                return;
            }
            StreamReader file = new StreamReader(StyFullPath);
            string line;
            while ((line = file.ReadLine()) != null)
            {
                ParseLine(line);
            }
            file.Close();
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
		    int start;
            string word = Common.LeftString(line, " ");

            switch (word)
            {
                case "\\Marker":
                    CreateClass(line);
                    break;
                case "\\Name":
                    start = line.IndexOf(" ") + 1;
                    _cssProp["Name"] = line.Substring(start);
                    break;
                case "\\Description":
                    start = line.IndexOf(" ") + 1;
                    _cssProp["Description"] = line.Substring(start);
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
                case "\\TextProperties":
                    TextProperties(line);
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

            _cssProp = new Dictionary<string, string>();

		    _styleInfo[mapClassName] = _cssProp;
        }

	    private void CreateFile()
	    {
	        _writer = new XmlTextWriter(_cssFullPath, null);
	        _writer.Formatting = Formatting.Indented;
	        _writer.WriteStartDocument();
	        _writer.WriteStartElement("stylesheet");

            string fontName = Common.ParaTextFontName(string.Empty);
            string fontSize = Common.ParaTextFontSize();
            
	        _writer.WriteStartElement("property");
	        _writer.WriteAttributeString("name", "font-family");
            _writer.WriteString(fontName);
	        _writer.WriteEndElement();

	        _writer.WriteStartElement("property");
	        _writer.WriteAttributeString("name", "font-size");
	        _writer.WriteAttributeString("unit", "pt");
            _writer.WriteString(fontSize);
	        _writer.WriteEndElement();
	    }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        private void TextProperties(string line)
        {
            string verseText = "false";
            if (line.IndexOf(" vernacular") >= 0)
            {
                verseText = "true";
            }

            string publishable = "true";
            if (line.IndexOf("nonpublishable") >= 0 )
            {
                publishable = "false";
            }
            _cssProp["versetext"] = verseText;
            _cssProp["publishable"] = publishable;
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
		public void WriteXML()
		{
            foreach (KeyValuePair<string, Dictionary<string, string>> cssClass in _styleInfo)
            {
                _writer.WriteStartElement("style");
                _writer.WriteAttributeString("id", cssClass.Key);
                SetTextProperties(cssClass);

                foreach (KeyValuePair<string, string> property in cssClass.Value)
                {
                    if (cssClass.Key == "publishable" || cssClass.Key == "versetext")
                    {
                        continue;
                    }
                    else if (cssClass.Key == "\\Name" || cssClass.Key == "\\Description")
                    {
                        _writer.WriteStartElement(cssClass.Key);
                        _writer.WriteString(property.Value);
                        _writer.WriteEndElement();
                    }
                    else
                    {
                        _writer.WriteStartElement("property");
                        _writer.WriteAttributeString("name", property.Key);
                        _writer.WriteString(property.Value);
                        _writer.WriteEndElement();
                    }
                }
                _writer.WriteEndElement();

            }
            _writer.WriteEndElement();
		    _writer.WriteEndDocument();
            _writer.Flush();
		    _writer.Close();
		}

	    private void SetTextProperties(KeyValuePair<string, Dictionary<string, string>> cssClass)
	    {
	        if (cssClass.Value.ContainsKey("publishable"))
	        {
	            _writer.WriteAttributeString("publishable", cssClass.Value["publishable"]);
	        }
	        else
	        {
	            _writer.WriteAttributeString("publishable", "true");
	        }

	        if (cssClass.Value.ContainsKey("versetext"))
	        {
	            _writer.WriteAttributeString("versetext", cssClass.Value["versetext"]);
	        }
	        else
	        {
	            _writer.WriteAttributeString("versetext", "false");
	        }
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
            else if(value == "1")
            {
                result = "150%";
            }
            else if (value == "2")
            {
                result = "200%";
            }
		    return result;
        }

       
	}
}
