using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using SIL.Tool;

namespace SIL.PublishingSolution
{
	public class StyToCSS
	{
	    private string _styFullPath, _cssFullPath;
        Dictionary<string, Dictionary<string, string>> _styleInfo = new Dictionary<string, Dictionary<string, string>>();
        Dictionary<string, string> _cssProp;
        Dictionary<string, string> _mapClassName = new Dictionary<string, string>();

        public void ConvertStyToCSS(string database, string cssFullPath)
        {

            //_styFullPath = styFullPath;
            _cssFullPath = cssFullPath;
            FindStyFile(database);
            MapClassName();
            ParseFile();
            WriteCSS();
        }

        private void FindStyFile(string database)
        {
            string ssfFile = database + ".ssf";
            string ssfFullPath;
            if (File.Exists("c:\\My Paratext Projects\\" + ssfFile))
            {
                ssfFullPath = "c:\\My Paratext Projects\\" + ssfFile;
            }
            else if (File.Exists("d:\\My Paratext Projects\\" + ssfFile))
            {
                ssfFullPath = "d:\\My Paratext Projects\\" + ssfFile;
            }
            else
            {
                Debug.WriteLine(ssfFile + " is not exist.");
                return;
            }
            //string ssfFile;

            bool isStylesheet=false;
            var reader = new XmlTextReader(ssfFullPath) {XmlResolver = null};
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == "StyleSheet") // Is class name null
                    {
                        isStylesheet = true;
                    }
                }
                else if (reader.NodeType == XmlNodeType.Text)
                {
                    if (isStylesheet) // Is class name null
                    {
                        ssfFile = reader.Value;
                        break;
                    }
                }
            }
            reader.Close();
            _styFullPath = Common.PathCombine(Path.GetDirectoryName(ssfFullPath), ssfFile);
        }

	    private void ParseFile()
        {
            _styleInfo.Clear();
            if(!File.Exists(_styFullPath))
            {
                Debug.WriteLine(_styFullPath + " is not exist.");
                return;
            }
            StreamReader file = new StreamReader(_styFullPath);
            string line;
            while ((line = file.ReadLine()) != null)
            {
                ParseLine(line);
            }
            file.Close();
        }

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
            }
        }

        private void CreateClass(string line)
        {
            int start = line.IndexOf(" ") + 1;
            int end = line.IndexOf(" ", start);
            string className = line.Substring(start, end - start);

            if (className.IndexOf("...") > 0)
            {
                className = Common.LeftString(className, "...");
            }

            string mapClassName = className;
            if (_mapClassName.ContainsKey(className))
            {
                mapClassName = _mapClassName[className];
            }
            _cssProp = new Dictionary<string, string>();
            _styleInfo[mapClassName] = _cssProp;

        }

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

        private void WriteCSS()
        {
            TextWriter cssFile = new StreamWriter(_cssFullPath);

            foreach (KeyValuePair<string, Dictionary<string, string>> cssClass in _styleInfo)
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
            cssFile.Close();
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
