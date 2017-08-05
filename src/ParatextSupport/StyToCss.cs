// --------------------------------------------------------------------------------------------
// <copyright file="StyToCss.cs" from='2009' to='2014' company='SIL International'>
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
// Convert Sty to Css format
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class StyToCss
    {
        public string StyFullPath { get; set; }
        private string _cssFullPath;
		private string _projectDirectory;
        private Dictionary<string, Dictionary<string, string>> _styleInfo = new Dictionary<string, Dictionary<string, string>>();
        private Dictionary<string, string> _cssProp;
        private Dictionary<string, string> _mapClassName = new Dictionary<string, string>();
        private string _styFolder, _cssFolder;

        /// ------------------------------------------------------------
        /// <summary>
        /// Convert a Paratext sty file to a CSS.
        /// </summary>
        /// <param name="database">The settings for the Paratext database.
        /// </param>
        /// <param name="cssFullPath">The CSS full path.</param>
        /// ------------------------------------------------------------
        public void ConvertStyToCss(string database, string cssFullPath, string ssfFullPath)
        {
            _cssFullPath = cssFullPath;
			_projectDirectory = ssfFullPath;
            bool fileExists = FindStyFile(database, ssfFullPath);

	        if (!fileExists)
	        {
		        return;
	        }

	        MapClassName();
            ParseFile();
            SetFontAndDirection();
            WriteCSS();

            // Create custom.css from custom.sty
            SetCustomPath(database);
			if (!File.Exists(StyFullPath))
			{
				return;
			}
            ParseFile();
            SetFontAndDirection();
            WriteCSS();
			MergeCustomCss(cssFullPath, _cssFullPath);
        }

	    private void MergeCustomCss(string mainCssFileName, string customCssFileName)
	    {
		    if (File.Exists(customCssFileName))
		    {
			    FileStream fs1 = null;
			    FileStream fs2 = null;
			    try
			    {
				    fs1 = File.Open(mainCssFileName, FileMode.Append);
				    fs2 = File.Open(customCssFileName, FileMode.Open);
				    byte[] fs2Content = new byte[fs2.Length];
				    fs2.Read(fs2Content, 0, (int) fs2.Length);
				    fs1.Write(fs2Content, 0, (int) fs2.Length);
			    }
			    catch (Exception ex)
			    {
				    Console.WriteLine(ex.Message + " : " + ex.StackTrace);
			    }
			    finally
			    {
				    fs1.Close();
				    fs2.Close();
			    }
		    }
	    }

        /// <summary>
        /// Override to convert the sty file to CSS, assuming the
        /// sty file is already set for this class. (Used by PathwayB.
        /// Loaded by reflection.)
        /// </summary>
        /// <param name="cssFullPath"></param>
        // ReSharper disable UnusedMember.Global
        public void ConvertStyToCss(string cssFullPath)
        // ReSharper restore UnusedMember.Global
        {
            _cssFullPath = cssFullPath;
            MapClassName();
            ParseFile();
            SetFontAndDirection();
            WriteCSS();
        }

        private void SetCustomPath(string database)
        {
			_styFolder = Path.GetDirectoryName(_projectDirectory);
            _cssFolder = Path.GetDirectoryName(_cssFullPath);
            _styFolder = Common.PathCombine(_styFolder, database);
            StyFullPath = Common.PathCombine(_styFolder, "custom.sty");
            _cssFullPath = Common.PathCombine(_cssFolder, "custom.css");
        }

        /// <summary>
        /// Add Properties for font and direction
        /// </summary>
        private void SetFontAndDirection()
        {
            CreateClass("\\Marker scrBody");
            _cssProp.Add("direction", Common.GetTextDirection(""));
            var settingsHelper = new SettingsHelper(Param.DatabaseName);
            var fontNode = Common.GetXmlNode(settingsHelper.GetSettingsFilename(), "//DefaultFont");
            if (fontNode != null)
            {
                _cssProp.Add("font-family", "\"" + fontNode.InnerText + "\"");
            }
        }

        /// ------------------------------------------------------------
        /// <summary>
        /// Finds the sty file for a Paratext project.
        /// </summary>
        /// <param name="database">The settings for the Paratext database.
        /// </param>
        /// ------------------------------------------------------------
        private bool FindStyFile(string database, string ssfFullPath)
        {
			string ssfFile = string.Empty;
			if(string.IsNullOrEmpty(ssfFullPath))
			{
				ssfFile = SettingsHelper.GetSettingFilePathForParatext(database);
				_projectDirectory = ssfFile;
			}
			else
			{
				ssfFile = ssfFullPath;
				//string ssfFileInputPath = FileInput(TestName);
				ssfFile = Common.PathCombine(ssfFile, "gather");
				ssfFile = Common.PathCombine(ssfFile, database + ".ssf");
			}

            bool isStylesheet = false;
			if (!File.Exists(ssfFile))
            {
                Debug.WriteLine(ssfFile + " does not exist.");
                return false;
            }

			string styFile = string.Empty;
			//ssf files don't have DTD or namespaces so we shouldn't need the null resolver.
	        var reader = new XmlTextReader(ssfFile); //{ XmlResolver = new NullResolver() };
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "StyleSheet") // Is class name null
                {
                    isStylesheet = true;
                }
                else if (reader.NodeType == XmlNodeType.Text && isStylesheet)
                {
					styFile = reader.Value;
                    break;
                }
            }
            reader.Close();
			StyFullPath = Common.PathCombine(Path.GetDirectoryName(ssfFile), styFile);
	        return true;
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
			if (!File.Exists(StyFullPath))
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

            switch (word.ToLower())
            {
                case "\\marker":
                    CreateClass(line);
                    break;
                case "\\fontsize":
                    value = PropertyValue(line);
                    _cssProp["font-size"] = value + "pt";
                    break;
                case "\\color":
                    value = PropertyValue(line);
                    string strHex = String.Format("{0:x2}", Convert.ToUInt32(value));
                    if (strHex.Length < 6)
                    {
                        strHex = strHex.PadRight(6, '0');
                    }
                    _cssProp["color"] = "#" + strHex;
                    break;
                case "\\justification":
                case "\\justificationtype": // ??
                    value = PropertyValue(line);
                    _cssProp["text-align"] = value;
                    break;
                case "\\spacebefore":
                    value = PropertyValue(line);
                    _cssProp["padding-top"] = value + "pt";
                    break;
                case "\\spaceafter":
                    value = PropertyValue(line);
                    _cssProp["padding-bottom"] = value + "pt";
                    break;
                case "\\leftmargin":
                    value = PropertyValue(line);
                    _cssProp["margin-left"] = value + "pt";
                    break;
                case "\\rightmargin":
                    value = PropertyValue(line);
                    _cssProp["margin-right"] = value + "pt";
                    break;
                case "\\firstlineindent":
                    value = PropertyValue(line);
                    value = Common.LeftString(value, "#").Trim();
                    _cssProp["text-indent"] = value + "pt";
                    break;
                case "\\fontname":
                    value = PropertyValue(line);
                    _cssProp["font-family"] = value;
                    break;
                case "\\italic":
                    _cssProp["font-style"] = "italic";
                    break;
                case "\\bold":
                    _cssProp["font-weight"] = "bold";
                    break;
                case "\\superscript":
                    _cssProp["vertical-align"] = "super";
                    break;
                case "\\subscript":
                    _cssProp["vertical-align"] = "sub";
                    break;
                case "\\underline":
                    _cssProp["text-decoration"] = "underline";
                    break;
                case "\\linespacing":
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

            className = RemoveMultiClass(className);

            if (className.ToLower() == "nd")
            {
                className = "NameOfGod";
            }

            string mapClassName = className;
            if (_mapClassName.ContainsKey(className))
                mapClassName = _mapClassName[className];

            _cssProp = new Dictionary<string, string>();
            _styleInfo[mapClassName] = _cssProp;
            if (mapClassName.ToLower() == "scrbookname" || mapClassName.ToLower() == "scrbookcode")
                _cssProp["display"] = "none";
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
            return className.Replace("(", "");
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
                propertyVal = Common.RightRemove(propertyVal, "#");
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

            WriteLanguageFontDirection(cssFile);

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

        protected static string WriterSettingsFile = null;
        protected static void WriteLanguageFontDirection(TextWriter cssFile)
        {
            if (WriterSettingsFile == null)
            {
                var settingsHelper = new SettingsHelper(Param.DatabaseName);
                WriterSettingsFile = settingsHelper.GetSettingsFilename();
            }
            var languageCodeNode = Common.GetXmlNode(WriterSettingsFile, "//EthnologueCode");
            var languageCode = "";
            var languageDirection = "ltr";
            var textAlign = "left";
			Common.FindParatextProject();
			Common.GetFontFeatures();
            if (languageCodeNode != null)
            {
                languageCode = languageCodeNode.InnerText;
                languageDirection = Common.GetTextDirection(languageCode);
                if (languageCode.Contains("-"))
                {
                    languageCode = languageCode.Split(new[] { '-' })[0];
                }
                if (languageDirection == "rtl")
                {
                    textAlign = "right";
                }
            }
            var fontNode = Common.GetXmlNode(WriterSettingsFile, "//DefaultFont");
            var fontFamily = "";
            if (fontNode != null)
            {
                fontFamily = fontNode.InnerText;
            }
            var fontSizeNode = Common.GetXmlNode(WriterSettingsFile, "//DefaultFontSize");
            var fontSize = "10";
            if (fontSizeNode != null)
            {
                fontSize = fontSizeNode.InnerText;
            }
            if (languageCode != "" && (fontFamily != "" || languageDirection != "ltr"))
            {
                cssFile.Write("div[lang='{0}']", languageCode);
                cssFile.Write("{");
                if (fontFamily != "")
                {
                    cssFile.Write(" font-family: \"{0}\";", fontFamily);
                }
                cssFile.Write(" font-size: {0}pt;", fontSize);
				if (Common.FontFeaturesSettingsString != string.Empty)
				{
					cssFile.Write(Common.FontFeaturesSettingsString);
				}
                if (languageDirection != "ltr")
                {
                    cssFile.Write(" direction: {0}; text-align: {1};", languageDirection, textAlign);
                }
                cssFile.WriteLine("}");
                cssFile.Write("span[lang='{0}']", languageCode);
                cssFile.Write("{");
                if (fontFamily != "")
                {
                    cssFile.Write(" font-family: \"{0}\";", fontFamily);
                }
				if (Common.FontFeaturesSettingsString != string.Empty)
				{
					cssFile.Write(Common.FontFeaturesSettingsString);
				}
                cssFile.WriteLine("}");
                cssFile.WriteLine();
            }
            WriterSettingsFile = null;
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
            else if (value == "1")
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
			_mapClassName["mt3"] = "Title_Tertiary";
            _mapClassName["w"] = "See_In_Glossary";
            _mapClassName["v"] = "Verse_Number";
            _mapClassName["fr"] = "Note_Target_Reference";
            _mapClassName["fq"] = "footnote_query";
            _mapClassName["f"] = "Note_General_Paragraph";
            _mapClassName["p"] = "Paragraph";
            _mapClassName["s"] = "Section_Head";
            _mapClassName["r"] = "Parallel_Passage_Reference";
            _mapClassName["c"] = "Chapter_Number";
            _mapClassName["rem"] = "rem";
			_mapClassName["fig"] = "pictureCaption";
            _mapClassName["q1"] = "Line1";
            _mapClassName["q2"] = "Line2";
            _mapClassName["m"] = "Paragraph_Continuation";
			_mapClassName["fqa"] = "footnote_querya";
            _mapClassName["sc"] = "Inscription";
			_mapClassName["pn"] = "pn";
			_mapClassName["rq"] = "rq";
			_mapClassName["ip"] = "Intro_Paras";
			_mapClassName["ft"] = "ft";
			_mapClassName["imt"] = "Intro_Title_Main";
			_mapClassName["im2"] = "Intro_Title_Secondary";
			_mapClassName["im3"] = "Intro_Title_Tertiary";
			_mapClassName["is"] = "Intro_Section_Head";
			_mapClassName["iq"] = "Intro_Citation_Line";
			_mapClassName["iq1"] = "Intro_Citation_Line1";
			_mapClassName["iq2"] = "Intro_Citation_Line2";
			_mapClassName["imq"] = "Intro_Citation_Paragraph";
			_mapClassName["periph"] = "Intro_Heading";
        }
    }
}
