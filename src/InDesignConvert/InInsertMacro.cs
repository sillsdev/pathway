// --------------------------------------------------------------------------------------------
// <copyright file="InInsertMacro.cs" from='2009' to='2014' company='SIL International'>
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
// File contains functions to insert in macro file.
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using Microsoft.Win32;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class InInsertMacro
    {
        #region private Variable
        readonly CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
        readonly TextInfo textInfo;
        private static string strMacroPath;
        private string supportFileFolder = string.Empty;
        #endregion

        #region public Variable
        public IDictionary<string, Dictionary<string, string>> _cssClass;
        #endregion

        #region Constructor
        public InInsertMacro()
        {
            textInfo = cultureInfo.TextInfo;
        }

        #endregion

        public void InsertMacroVariable(PublicationInformation projInfo, IDictionary<string, Dictionary<string, string>> cssClass)
        {
            var textContent = new StringBuilder();
            ArrayList insertVariable = new ArrayList();
            if (projInfo.ProjectInputType == null)
                projInfo.ProjectInputType = "Dictionary"; //TODO
            _cssClass = cssClass;

            //Create and copy file to temp folder
            supportFileFolder = Common.PathCombine(Common.GetPSApplicationPath(), "InDesignFiles" + Path.DirectorySeparatorChar + projInfo.ProjectInputType);

			if (!Directory.Exists(supportFileFolder))
			{
				supportFileFolder = Common.PathCombine(Path.GetDirectoryName(Common.AssemblyPath), "InDesignFiles" + Path.DirectorySeparatorChar + projInfo.ProjectInputType);
			}

            projInfo.TempOutputFolder = Common.PathCombine(Path.GetTempPath(), "InDesignFiles" + Path.DirectorySeparatorChar + projInfo.ProjectInputType);
            CopyFolderWithFiles(supportFileFolder, projInfo.TempOutputFolder);

            //Same macro is used for dictionary and scripture
            var scriptsFolder = Common.PathCombine(Common.GetPSApplicationPath(), "InDesignFiles/Dictionary/Scripts");
			if (!Directory.Exists(scriptsFolder))
			{
				scriptsFolder = Common.PathCombine(Path.GetDirectoryName(Common.AssemblyPath), "InDesignFiles/Dictionary/Scripts");
			}
            CopyFolderWithFiles(scriptsFolder, Common.PathCombine(projInfo.TempOutputFolder, "Scripts"));

            strMacroPath = Common.PathCombine(projInfo.TempOutputFolder, "Scripts/Startup Scripts/PlaceFrames.jsx");
            ReadFile(false, textContent);

            insertVariable.Add(GetColumnRule());
            insertVariable.Add(GetBorderRule());
            insertVariable.Add(GetMarginValue());
            insertVariable.Add(GetCropMarks());
            insertVariable.Add(GetIndexTab(projInfo.ProjectInputType));

            WriteFile(textContent, insertVariable);

            CopySupportFolder(projInfo);

            //Copy pictures to IndesignFiles folder (inside Stories folder)
            var pictureFolder = Common.PathCombine(projInfo.DictionaryPath, "Pictures");
            if (File.Exists(pictureFolder))
                CopyFolderWithFiles(pictureFolder, Common.PathCombine(projInfo.TempOutputFolder, "Pictures"));
        }

        private static void WriteFile(StringBuilder textContent, ArrayList insertVariables)
        {
            var write = new StreamWriter(strMacroPath, false);

            foreach (string variable in insertVariables)
            {
                if (!string.IsNullOrEmpty(variable))
                {
                    write.WriteLine(variable);
                }
            }
            write.WriteLine(textContent.ToString());
            write.Flush();
            write.Close();
        }

        private static void ReadFile(bool isCommentStart, StringBuilder textContent)
        {
            var reader = new StreamReader(strMacroPath);
            string currLine;
            while ((currLine = reader.ReadLine()) != null)
            {
                if (currLine.IndexOf("// --") == 0 && !isCommentStart)
                {
                    isCommentStart = true;
                }
                if (isCommentStart)
                {
                    textContent.AppendLine(currLine);
                }
            }
            reader.Close();
        }

        public string GetMarginValue()
        {
            string margin = "var margin=new Array(\"";
            var list = new ArrayList();
            var val = new Dictionary<string, ArrayList>();

            foreach (var className in _cssClass)
            {
                bool isvalidProperty = false;

                list.Clear();
                GetMarginValue(className.Key, list, "top");
                GetMarginValue(className.Key, list, "right");
                GetMarginValue(className.Key, list, "bottom");
                GetMarginValue(className.Key, list, "left");
                foreach (string value in list)
                {
                    if (value != "0")
                    {
                        isvalidProperty = true;
                    }
                }
                if (isvalidProperty)
                {
                    var copyList = new ArrayList();
                    copyList.AddRange(list);
                    val[className.Key] = copyList;
                }
            }

            foreach (KeyValuePair<string, ArrayList> pair in val)
            {
                if (pair.Key.ToLower().IndexOf("lethead") >= 0)
                {
                    float topValue = float.Parse(Common.UnitConverter(pair.Value[0].ToString() + "pt", "pc"), CultureInfo.GetCultureInfo("en-US"));
                    float rightValue = float.Parse(Common.UnitConverter(pair.Value[1].ToString() + "pt", "pc"), CultureInfo.GetCultureInfo("en-US"));
                    float bottomValue = float.Parse(Common.UnitConverter(pair.Value[2].ToString() + "pt", "pc"), CultureInfo.GetCultureInfo("en-US"));
                    float leftValue = float.Parse(Common.UnitConverter(pair.Value[3].ToString() + "pt", "pc"), CultureInfo.GetCultureInfo("en-US"));

                    margin = margin + pair.Key + "\", \"" + topValue + "\", \"" + rightValue + "\", \"" +
                    bottomValue + "\", \"" + leftValue + "\", \"";
                    break;
                }
            }
            if (margin.LastIndexOf("\", \"") > 0)
            {
                margin = margin.Remove(margin.LastIndexOf("\", \""), 4);
            }
            margin = margin + "\");";
            return margin;
        }

        public string GetBorderRule()
        {
            string borderRule = "var borderRule=new Array(\"";
            var list = new ArrayList();
            var val = new Dictionary<string, ArrayList>();

            foreach (var className in _cssClass)
            {
                bool isvalidProperty = false;
                list.Clear();
                GetDimensionValue(className.Key, list, "top");
                GetDimensionValue(className.Key, list, "right");
                GetDimensionValue(className.Key, list, "bottom");
                GetDimensionValue(className.Key, list, "left");
                foreach (string value in list)
                {
                    if (value != "none")
                    {
                        isvalidProperty = true;
                    }
                }
                if (isvalidProperty)
                {
                    var copyList = new ArrayList();
                    copyList.AddRange(list);
                    val[className.Key] = copyList;
                }
            }

            foreach (KeyValuePair<string, ArrayList> pair in val)
            {
                borderRule = borderRule + pair.Key + "\", \"" + pair.Value[0] + "\", \"" + pair.Value[1] + "\", \"" + pair.Value[2] + "\", \"" + pair.Value[3] + "\", \"";
            }
            if (borderRule.LastIndexOf("\", \"") > 0)
            {
                borderRule = borderRule.Remove(borderRule.LastIndexOf("\", \""), 4);
            }
            borderRule = borderRule + "\");";
            return borderRule;
        }

        public string GetCropMarks()
        {
            string IsCropMarkChecked = "false";
            const string cropMarks = "var cropMarks = ";
            var pages = new ArrayList();
            pages.Add("@page");
            pages.Add("@page:left");
            pages.Add("@page:right");
            foreach (string page in pages)
            {
                if (_cssClass.ContainsKey(page) && _cssClass[page].ContainsKey("marks"))
                {
                    IsCropMarkChecked = (_cssClass["@page"]["marks"] == "crop").ToString();
                    break;
                }
            }
            return cropMarks + IsCropMarkChecked.ToLower() + ";";
        }

        public string GetIndexTab(string inputType)
        {

            string isLocatorExists = "false";
            const string indexTab = "var indexTab = ";
            if (inputType.ToLower() == "dictionary" && _cssClass.ContainsKey("locator"))
            {
                isLocatorExists = "true";
            }
            return indexTab + isLocatorExists.ToLower() + ";";
        }

        public string GetColumnRule()
        {
            string colWidth, colStyle, colColor;
            string columnRule = "var columnRule=new Array(\"";
            string classWithColumnRule = string.Empty;
            foreach (var className in _cssClass)
            {
                if (_cssClass[className.Key].ContainsKey("column-rule-style"))
                {
                    colWidth = _cssClass[className.Key].ContainsKey("column-rule-width")
                                   ? _cssClass[className.Key]["column-rule-width"]
                                   : "x";
                    colStyle = _cssClass[className.Key].ContainsKey("column-rule-style")
                                   ? textInfo.ToTitleCase(_cssClass[className.Key]["column-rule-style"])
                                   : "x";
                    colColor = _cssClass[className.Key].ContainsKey("column-rule-color")
                                   ? _cssClass[className.Key]["column-rule-color"]
                                   : "x";

                    classWithColumnRule = classWithColumnRule + "\",\"";

                    if (colWidth != "x" || colStyle != "x" || colColor != "x")
                    {
                        classWithColumnRule = classWithColumnRule + className.Key + " " + colWidth + " " + colStyle + " " + colColor;
                    }
                }
            }

            if (classWithColumnRule.Length > 2)
            {
                columnRule = columnRule + classWithColumnRule.Substring(3) + "\");";
            }
            else if (classWithColumnRule.Length == 0)
            {
                columnRule = columnRule + "\");";
            }
            return columnRule;
        }

        private void GetDimensionValue(string className, IList list, string dimension)
        {
            string styleName = "border-" + dimension + "-style";
            string colorName = "border-" + dimension + "-color";
            string widthName = "border-" + dimension + "-width";
            string borderStyle = _cssClass[className].ContainsKey(styleName) ? _cssClass[className][styleName] : "none";
            string borderColor = _cssClass[className].ContainsKey(colorName) ? _cssClass[className][colorName] : "none";
            string borderWidth = _cssClass[className].ContainsKey(widthName) ? _cssClass[className][widthName] : "none";

            if (borderStyle != "none" && borderColor != "none" && borderWidth != "none")
            {
                list.Add(borderWidth + " " + borderStyle + " " + borderColor);
            }
            else if (borderStyle != "none")
            {
                if (borderColor == "none")
                {
                    borderColor = "#000000"; // set default to black color.
                }
                if (borderWidth == "none")
                {
                    borderWidth = ".5"; // set default to .5pt
                }

                list.Add(borderWidth + " " + borderStyle + " " + borderColor);
            }
            else
            {
                list.Add("none");
            }
        }

        private void GetMarginValue(string className, IList list, string dimension)
        {
            string styleName = "class-margin-" + dimension;
            string margin = _cssClass[className].ContainsKey(styleName) ? _cssClass[className][styleName] : "0";

            if (margin != "0")
            {
                list.Add(margin);
            }
            else
            {
                list.Add("0");
            }
        }
        public void CopySupportFolder(PublicationInformation projInfo)
        {
            Common.SupportFolder = "";
            const string postPath = @"/en_US/Scripts";
            string scriptsFolderWithPath = Common.PathCombine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Adobe/InDesign/");
            string versionName = GetVersionFolderName(scriptsFolderWithPath);
            scriptsFolderWithPath = scriptsFolderWithPath + versionName + postPath;
            CopyFolderWithFiles(Common.PathCombine(projInfo.TempOutputFolder, "Scripts"), scriptsFolderWithPath);
        }

        private static string GetVersionFolderName(string ScriptsFolderWithPath)
        {
            var dirInfo = new DirectoryInfo(ScriptsFolderWithPath);
            if (!dirInfo.Exists)
                dirInfo.Create();   // InDesign is not installed.
            DirectoryInfo[] subDirInfo = dirInfo.GetDirectories();
            string version = "Version 6.0";
            foreach (var info in subDirInfo)
            {
                if (info.Name.Contains("Version"))
                {
                    version = info.Name;
                }
            }
            return version;
        }

        /// <summary>
        /// To copy Temporary files to Environmental Temp Folder instead of keeping changes in Application Itself.
        /// </summary>
        /// <param name="sourceFolder"></param>
        /// <param name="destFolder"></param>
        private static void CopyFolderWithFiles(string sourceFolder, string destFolder)
        {
            if (Directory.Exists(destFolder))
            {
                DirectoryInfo di = new DirectoryInfo(destFolder);
                Common.CleanDirectory(di);
            }
            Directory.CreateDirectory(destFolder);
            string[] files = Directory.GetFiles(sourceFolder);
            try
            {
                foreach (string file in files)
                {
                    string name = Path.GetFileName(file);
                    string dest = Common.PathCombine(destFolder, name);
                    File.Copy(file, dest);
                }

                string[] folders = Directory.GetDirectories(sourceFolder);
                foreach (string folder in folders)
                {
                    string name = Path.GetFileName(folder);
                    string dest = Common.PathCombine(destFolder, name);
                    if (name != ".svn")
                    {
                        CopyFolderWithFiles(folder, dest);
                    }
                }
            }
            catch
            {
                return;
            }
        }
    }
}
