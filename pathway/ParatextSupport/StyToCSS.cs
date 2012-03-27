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
	public class StyToCSS
	{
	    public string StyFullPath { get; set; }
        private string _cssFullPath;
        private Dictionary<string, Dictionary<string, string>> _styleInfo = new Dictionary<string, Dictionary<string, string>>();
		private Dictionary<string, string> _cssProp;
		private Dictionary<string, string> _mapClassName = new Dictionary<string, string>();

		/// ------------------------------------------------------------
		/// <summary>
		/// Convert a Paratext sty file to a CSS.
		/// </summary>
		/// <param name="database">The settings for the Paratext database.
		/// </param>
		/// <param name="cssFullPath">The CSS full path.</param>
        /// ------------------------------------------------------------
		public void ConvertStyToCSS(string database, string cssFullPath)
        {

            //StyFullPath = styFullPath;
            _cssFullPath = cssFullPath;
            FindStyFile(database);
            MapClassName();
            ParseFile();
            WriteCSS();
        }

        /// <summary>
        /// Override to convert the sty file to CSS, assuming the
        /// sty file is already set for this class.
        /// </summary>
        /// <param name="cssFullPath"></param>
        public void ConvertStyToCSS(string cssFullPath)
        {
            _cssFullPath = cssFullPath;
            MapClassName();
            ParseFile();
            WriteCSS();
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
            string word = Common.LeftString(line, " ");

            switch (word)
            {
                case "\\Name":
                    CreateClass(line);
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

            string mapClassName = className;
            if (_mapClassName.ContainsKey(className))
                mapClassName = _mapClassName[className];

            _cssProp = new Dictionary<string, string>();
            _styleInfo[mapClassName] = _cssProp;
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
		public void WriteCSS()
        {
            TextWriter cssFile = new StreamWriter(_cssFullPath);

            foreach (KeyValuePair<string, Dictionary<string, string>> cssClass in _styleInfo)
            {
                if (cssClass.Key != "\\Name")
                {
                    cssFile.WriteLine("." + cssClass.Key);
                    cssFile.WriteLine("{");
                    foreach (KeyValuePair<string, string> property in cssClass.Value)
                    {
                        cssFile.WriteLine(property.Key + ": " + property.Value + ";");
                    }
                    cssFile.WriteLine("}");
                    cssFile.WriteLine();
                }
            }
            cssFile.Close();
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

        private void MapClassName()
        {
            _mapClassName["toc1"] = "scrBookCode";
            _mapClassName["toc2"] = "scrBookName";
            _mapClassName["mt1"] = "Title_Main";
            _mapClassName["mt2"] = "Title_Secondary";
            _mapClassName["w"] = "See_In_Glossary";
            _mapClassName["v"] = "Verse_Number";
            _mapClassName["fr"] = "Note_Target_Reference";
            _mapClassName["fq"] = "Alternate_Reading";
            _mapClassName["f"] = "Note_General_Paragraph";
            _mapClassName["p"] = "Paragraph";
            _mapClassName["s"] = "Section_Head";
            _mapClassName["r"] = "Parallel_Passage_Reference";
            _mapClassName["c"] = "Chapter_Number";
            _mapClassName["rem"] = "rem";
            _mapClassName["fig"] = "fig";
            _mapClassName["q1"] = "Line1";
            _mapClassName["q2"] = "Line2";
            _mapClassName["m"] = "Paragraph_Continuation";
            _mapClassName["fqa"] = "fqa";
            _mapClassName["sc"] = "Inscription";
        }
	}
}
