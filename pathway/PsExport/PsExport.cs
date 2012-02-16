// --------------------------------------------------------------------------------------------
// <copyright file="PsExport.cs" from='2009' to='2009' company='SIL International'>
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
// Implements Fieldworks Utility Interface for DictionaryExpress
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using JWTools;
using RevHomographNum;
using SIL.Tool;
using SIL.Tool.Localization;

namespace SIL.PublishingSolution
{
    /// <summary>
    /// Implements Publishing Solutins for Translation Editor
    /// </summary>
    public class PsExport: IExporter
    {
        private const bool _Wait = true;
        public bool _fromNUnit = false;

        #region Properties
        /// <summary>Gets or sets Output format (ODT, PDF, INX, TeX, HTM, PDB, etc.)</summary>
        public string Destination
        {
            get
            {
                return Param.Value[Param.PrintVia];
            }
            set
            {
                Param.SetValue(Param.PrintVia, value);
            } 
        }

        /// <summary>Gets or sets data type (Scripture, Dictionary)</summary>
        public string DataType { get; set; }

        /// <summary>UI progress bar</summary>
        public ProgressBar ProgressBar { get; set; }
        #endregion Properties

        private string _projectFile;

        #region Export
        /// <summary>
        /// Have the utility do what it does.
        /// </summary>
        public void Export(string outFullName)
        {
            Debug.Assert(DataType == "Scripture" || DataType == "Dictionary", "DataType must be Scripture or Dictionary");
            Debug.Assert(outFullName.IndexOf(Path.DirectorySeparatorChar) >= 0, "full path for output must be given");
            try

            {
                //get xsltFile from ExportThroughPathway.cs
                if (DataType == "Dictionary")
                    XsltPreProcess(outFullName);

                string supportPath = GetSupportPath();
				Backend.Load(Common.ProgInstall);
                LoadProgramSettings(supportPath);
                LocalizationSetup();
                LoadDataTypeSettings();
                var outDir = Path.GetDirectoryName(outFullName);
                DefaultProjectFileSetup(outDir);
                SubProcess.BeforeProcess(outFullName);

                var mainXhtml = Path.GetFileNameWithoutExtension(outFullName) + ".xhtml";
                //var mainXhtml = Path.GetFileNameWithoutExtension(outFullName) + "_cv.xhtml";
                var mainFullName = Common.PathCombine(outDir, mainXhtml);
                Debug.Assert(mainFullName.IndexOf(Path.DirectorySeparatorChar) >= 0, "Path for input file missing");
                if (string.IsNullOrEmpty(mainFullName) || !File.Exists(mainFullName))
                {
                    var msg = new[] { "Input File(main.xhtml) is not Found" };
                    LocDB.Message("errFnFound", "Input File(main.xhtml) is not Found.", msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                    return;
                }
                string cssFullName = GetCssFullName(outDir, mainFullName);
                if (cssFullName == null) return;
                string fluffedCssFullName;
                string revFileName = string.Empty;
                if (Path.GetFileNameWithoutExtension(outFullName) == "FlexRev")
                {
                    fluffedCssFullName = GetFluffedCssFullName(GetRevFullName(outFullName), outDir, cssFullName);
                }
                else
                {
                    fluffedCssFullName = GetFluffedCssFullName(outFullName, outDir, cssFullName);
                    revFileName = GetRevFullName(outDir);
                }
                string fluffedRevCssFullName = string.Empty;
                if (revFileName.Length > 0)
                {
                    fluffedRevCssFullName = GetFluffedCssFullName(revFileName, outDir, cssFullName);
                }
                DestinationSetup();
                SetDefaultLanguageFont(fluffedCssFullName, mainFullName, fluffedRevCssFullName);
                if (DataType == "Scripture")
                {
                    SeExport(mainXhtml, Path.GetFileName(fluffedCssFullName), outDir);
                }
                else if (DataType == "Dictionary")
                {
                    
                    string revFullName = GetRevFullName(outDir);
                    string gramFullName = MakeXhtml(outDir, "sketch.xml", "XLingPap.xsl", supportPath);
                    DeExport(outFullName, fluffedCssFullName, revFullName, fluffedRevCssFullName, gramFullName);
                }
            }
            catch (InvalidStyleSettingsException err)
            {
                if (_fromNUnit)
                {
                    Console.WriteLine(string.Format(err.ToString(), err.FullFilePath), "Pathway Export");
                }
                else
                {
                    MessageBox.Show(string.Format(err.ToString(), err.FullFilePath), "Pathway Export",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //var msg = new[] { err.FullFilePath };
                //LocDB.Message("errNotValidXml", err.ToString(), msg, LocDB.MessageTypes.Warning, LocDB.MessageDefault.First);
                return;
            }
            catch (UnauthorizedAccessException err)
            {
                if (_fromNUnit)
                {
                    Console.WriteLine(string.Format(err.ToString(), "Sorry! You might not have permission to use this resource."));
                }
                else
                {
                    MessageBox.Show(string.Format(err.ToString(), "Sorry! You might not have permission to use this resource."), @"Pathway Export", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //var msg = new[] { "Sorry! You might not have permission to use this resource." };
                //LocDB.Message("errUnauthorized", err.ToString(), msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                return;
            }
            catch (Exception ex)
            {
                if (_fromNUnit)
                {
                    Console.WriteLine(ex.ToString());
                }
                else
                {
                    MessageBox.Show(ex.ToString(), @"Pathway Export", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //var msg = new[] { ex.ToString() };
                //LocDB.Message("defErrMsg", ex.ToString(), msg, LocDB.MessageTypes.Warning, LocDB.MessageDefault.First);
                return;
            }
        }

        /// <summary>
        /// Preprocess the xhtml file using xsl file.
        /// </summary>
        /// <param name="outFullName">input xhtml file</param>
        private void XsltPreProcess(string outFullName)
        {
            List<string> xsltFile = new List<string>();
            if (Param.Value.ContainsKey(Param.RemoveEmptyDiv) && Param.Value[Param.RemoveEmptyDiv] == "True")
            {
                xsltFile.Add("Remove Hyperlinks.xsl");
            }
            if (Param.Value.ContainsKey(Param.RemoveHyperlink) && Param.Value[Param.RemoveHyperlink] == "True")
            {
                xsltFile.Add("Remove Empty Divs.xsl");
            }

            for (int i = 0; i < xsltFile.Count; i++)
            {
                var mainCvXhtml = Common.PathCombine(Path.GetDirectoryName(outFullName),
                                                     Path.GetFileNameWithoutExtension(outFullName) +
                                                     "_cv.xhtml");
                string xsltFullName = Common.PathCombine(Common.GetApplicationPath(), "Preprocessing\\" + xsltFile[i]);
                Common.XsltProcess(outFullName, xsltFullName, "_cv.xhtml");
                if (i == 0)
                {
                    var mainCpyFile = Common.PathCombine(Path.GetDirectoryName(outFullName),
                                                         Path.GetFileNameWithoutExtension(outFullName) +
                                                         "0.xhtml");
                    File.Copy(outFullName, mainCpyFile, true);
                }
                File.Copy(mainCvXhtml, outFullName, true);

                File.Delete(mainCvXhtml);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fluffedCssFullName"></param>
        /// <param name="mainFullName"></param>
        /// <param name="fluffedCssReversal"></param>
        private void SetDefaultLanguageFont(string fluffedCssFullName, string mainFullName, string fluffedCssReversal)
        {
            string fileName = Path.GetFileName(mainFullName);
            if (AppDomain.CurrentDomain.FriendlyName.ToLower() == "paratext.exe" || (DataType == "Dictionary" && fileName == "main.xhtml"))
            {
                Common.LanguageSettings(mainFullName, fluffedCssFullName, DataType == "Dictionary", fluffedCssReversal);
            }
            else
            {
                Common.TeLanguageSettings(fluffedCssFullName);
            }

            //string[] args = Environment.GetCommandLineArgs();
            //for (int i = 0; i < args.Length; i++)
            //{MessageBox.Show(args[i].ToString());}
        }


        /// <summary>
        /// Returns reversal name with path (empty string if file doesn't exist)
        /// </summary>
        /// <param name="outDir">Folder which contains reversal file if it exists</param>
        /// <returns>Reversal name with path</returns>
        protected static string GetRevFullName(string outDir)
        {
            //string revFullName = Common.PathCombine(Path.GetDirectoryName(outDir), "FlexRev.xhtml");
            if(File.Exists(outDir))
            {
                outDir = Path.GetDirectoryName(outDir);
            }
            string revFullName = Common.PathCombine(outDir, "FlexRev.xhtml");
            if (!File.Exists(revFullName))
                revFullName = "";
            else
            {
                Common.StreamReplaceInFile(revFullName, "<ReversalIndexEntry_Self>", "");
                Common.StreamReplaceInFile(revFullName, "</ReversalIndexEntry_Self>", "");
                Common.StreamReplaceInFile(revFullName, "class=\"headword\"", "class=\"headref\"");
                string revCssFullName = revFullName.Substring(0, revFullName.Length - 6) + ".css";
                Common.StreamReplaceInFile(revCssFullName, ".headword", ".headref");
                AddHomographAndSenseNumClassNames.Execute(revFullName, revFullName);
            }
            return revFullName;


            //string tmpOutDir = outDir;
            //string fileName = Path.GetFileName(tmpOutDir);
            //if (fileName.Length > 1)
            //    tmpOutDir = Path.GetDirectoryName(outDir);
            //string revFullName = Common.PathCombine(tmpOutDir, "FlexRev.xhtml");
            //if (!File.Exists(revFullName))
            //    revFullName = "";
            //else
            //{
            //    Common.StreamReplaceInFile(revFullName, "<ReversalIndexEntry_Self>", "");
            //    Common.StreamReplaceInFile(revFullName, "</ReversalIndexEntry_Self>", "");
            //    Common.StreamReplaceInFile(revFullName, "class=\"headword\"", "class=\"headref\"");
            //    string revCssFullName = revFullName.Substring(0, revFullName.Length - 6) + ".css";
            //    Common.StreamReplaceInFile(revCssFullName, ".headword", ".headref");
            //    AddHomographAndSenseNumClassNames.Execute(revFullName, revFullName);
            //}
            //return revFullName;
        }

        /// <summary>
        /// Combines all css files into one and adds expected css to beginning
        /// </summary>
        /// <param name="outputFullName">Name used to calculate default css name</param>
        /// <param name="outDir">where results will be stored</param>
        /// <param name="cssFullName">name and path of css file</param>
        /// <returns></returns>
        public string GetFluffedCssFullName(string outputFullName, string outDir, string cssFullName)
        {
            var mc = new MergeCss();
            string fluffedCssFullName;

            try
            {
                if (!File.Exists(cssFullName))
                {
                    var layout = Param.Value[Param.LayoutSelected];
                    cssFullName = Param.StylePath(Param.StyleFile[layout]);
                }
                string myCss = Common.PathCombine(outDir, Path.GetFileName(cssFullName));
                if (cssFullName != myCss)
                    File.Copy(cssFullName, myCss, true);
                var expCss = Path.GetFileNameWithoutExtension(outputFullName) + ".css";
                string expCssLine = "@import \"" + expCss + "\";";
                Common.FileInsertText(myCss, expCssLine);
                string outputCSSFileName = "merged" + expCss;
                var tmpCss = mc.Make(myCss, outputCSSFileName);
                fluffedCssFullName = Common.PathCombine(outDir, Path.GetFileName(tmpCss));
                File.Copy(tmpCss, fluffedCssFullName, true);
                File.Delete(tmpCss);
                Common.StreamReplaceInFile(fluffedCssFullName, "string(verse) ' = '", "string(verse) ' '"); //TD-1945
            }
            catch (Exception)
            {
                fluffedCssFullName = string.Empty;
            }
            return fluffedCssFullName;
        }

        /// <summary>
        /// Return full css name
        /// </summary>
        /// <param name="outDir">where to find css name</param>
        /// <param name="mainFullName">export name used to calculate css name</param>
        /// <returns>name and path of css</returns>
        protected string GetCssFullName(string outDir, string mainFullName)
        {
            var cssFullName = Param.StylePath(Param.Value[Param.LayoutSelected]);
            if (string.IsNullOrEmpty(cssFullName))
            {
                var stylePick = new PublicationTask { InputPath = outDir, CurrentInput = mainFullName, InputType = DataType };
                if (!Common.Testing)
                    stylePick.ShowDialog();
                else
                {
                    stylePick.DoLoad();
                    stylePick.DoAccept();
                }
                cssFullName = stylePick.cssFile;
            }
            return cssFullName;
        }

        /// <summary>
        /// Determines destination back end.
        /// </summary>
        protected void DestinationSetup()
        {
            if (string.IsNullOrEmpty(Destination))
            {
                var edlg = new ExportDlg { ExportType = DataType };
                edlg.ShowDialog();
                Destination = edlg.ExportType;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outDir"></param>
        protected void DefaultProjectFileSetup(string outDir)
        {
            _projectFile = Common.PathCombine(outDir, DataType + ".de");
            if (!File.Exists(_projectFile))
                File.Copy(Common.FromRegistry(Common.PathCombine(Param.Value[Param.SamplePath], Path.GetFileName(_projectFile))), _projectFile);
        }

        protected void LoadDataTypeSettings()
        {
            Param.Value[Param.InputType] = DataType;
            Param.LoadSettings();
        }

        protected static void LocalizationSetup()
        {
            JW_Registry.RootKey = @"SOFTWARE\The Seed Company\Dictionary Express!";
            LocDB.SetAppTitle();
            LocDB.BaseName = "PsLocalization.xml";
            var folderPath = Param.Value[Param.OutputPath];
            var localizationPath = Common.PathCombine(folderPath, "Loc");
            if (!Directory.Exists(localizationPath))
            {
                Directory.CreateDirectory(localizationPath);
				File.Copy(Common.FromRegistry(@"Loc/" + LocDB.BaseName), Common.PathCombine(localizationPath, LocDB.BaseName));
            }
            LocDB.Initialize(folderPath);
        }

        protected static void LoadProgramSettings(string supportPath)
        {
			Common.ProgBase = supportPath;
            Param.LoadSettings();
        }

        protected static string GetSupportPath()
        {
            return Common.GetPSApplicationPath();
        }

        #endregion Export

        #region Protected Function
        #region MakeXhtml
        /// <summary>
        /// If XML file exists, transform it to XHTML
        /// </summary>
        /// <param name="outPath">path to XML</param>
        /// <param name="xmlName">XML file name</param>
        /// <param name="transform">Name of transform</param>
        /// <param name="dicPath">If dtds required for Xsl, it is a path, otherwise null</param>
        /// <returns>xhtmlFile full name or empty string if no XML file available</returns>
        protected static string MakeXhtml(string outPath, string xmlName, string transform, string dicPath)
        {
            string xhtmlFile = "";
            string xmlPath = Common.PathCombine(outPath, xmlName);
            if (File.Exists(xmlPath))
            {
                if (dicPath != null)
                    CopyDtds(dicPath, outPath);
                xhtmlFile = Common.XsltProcess(xmlPath, transform, ".xhtml");
                if (!Path.IsPathRooted(xhtmlFile))
                {
                    var msg = new[] { xhtmlFile };
                    LocDB.Message("defErrMsg", xhtmlFile, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                }
                if (dicPath != null)
                    RemoveDtds(dicPath, outPath);
            }
            return xhtmlFile;
        }

        #region CopyDtds
        /// <summary>
        /// Copy all DTDs from DictionaryExpress folder to Temp folder
        /// </summary>
        /// <param name="dicPath">DictionaryExpress Folder name</param>
        /// <param name="outPath">Output path</param>
        protected static void CopyDtds(string dicPath, string outPath)
        {
            var dir = new DirectoryInfo(dicPath);
            FileInfo[] dtdFiles = dir.GetFiles("*.dtd");
            foreach (FileInfo fl in dtdFiles)
            {
                string outFile = Common.PathCombine(outPath, fl.Name);
                if (!File.Exists(outFile))
                    File.Copy(Common.PathCombine(dicPath, fl.Name), outFile);
            }
        }
        #endregion CopyDtds

        #region RemoveDtds
        /// <summary>
        /// Remove all DTDs from Temp folder that exist in DictionaryExpress folder
        /// </summary>
        /// <param name="dicPath">DictionaryExpress Folder name</param>
        /// <param name="outPath">Output path</param>
        protected static void RemoveDtds(string dicPath, string outPath)
        {
            var dir = new DirectoryInfo(dicPath);
            FileInfo[] dtdFiles = dir.GetFiles("*.dtd");
            foreach (FileInfo fl in dtdFiles)
            {
                string outFile = Common.PathCombine(outPath, fl.Name);
                if (File.Exists(outFile))
                    File.Delete(outFile);
            }
        }
        #endregion RemoveDtds
        #endregion MakeXhtml

        #region DeExport
        /// <summary>
        /// Exports the input files to the chosen destination
        /// </summary>
        /// <param name="lexiconFull">main dictionary content</param>
        /// /// <param name="lexiconCSS">main css file with style info</param>
        /// <param name="revFull">reversal content</param>
        /// <param name="revCSS">rev CSS file with style info</param>
        /// <param name="gramFull">grammar content</param>
        public void DeExport(string lexiconFull, string lexiconCSS, string revFull, string revCSS, string gramFull)
        {
            var projInfo = new PublicationInformation();

            if (ProgressBar == null)
                ProgressBar = new ProgressBar();
            projInfo.ProgressBar = ProgressBar;
            projInfo.ProjectFileWithPath = _projectFile;
            projInfo.IsLexiconSectionExist = File.Exists(lexiconFull);
            //projInfo.IsReversalExist = File.Exists(revFull);
            //projInfo.IsReversalExist = Param.Value[Param.ReversalIndex] == "True";
            SetReverseExistValue(projInfo);
            projInfo.SwapHeadword = false;
            projInfo.FromPlugin = true;
            projInfo.DefaultCssFileWithPath = lexiconCSS;
            projInfo.DefaultRevCssFileWithPath = revCSS;
            projInfo.DefaultXhtmlFileWithPath = lexiconFull;
            projInfo.ProjectInputType = "Dictionary";
            projInfo.DictionaryPath = Path.GetDirectoryName(lexiconFull);
            projInfo.ProjectName = Path.GetFileNameWithoutExtension(lexiconFull);
            //if (lexiconFull == revFull || lexiconFull == gramFull)
            //    projInfo.IsLexiconSectionExist = false;

            string lexiconFileName = Path.GetFileName(lexiconFull);
            string revFileName = Path.GetFileName(revFull);
            string gramFileName = Path.GetFileName(gramFull);
            if (lexiconFileName == revFileName || lexiconFileName == gramFileName)
                projInfo.IsLexiconSectionExist = false;
            if (projInfo.IsLexiconSectionExist && !projInfo.IsReversalExist)
            {
                projInfo.ProjectName = Path.GetFileNameWithoutExtension(lexiconFull);
            }
            else if (!projInfo.IsLexiconSectionExist && projInfo.IsReversalExist)
            {
                projInfo.ProjectName = Path.GetFileNameWithoutExtension(revFull);
            }
            SetExtraProcessingValue(projInfo);

            Backend.Launch(Destination, projInfo);
        }

        private void SetReverseExistValue(PublicationInformation projInfo)
        {
            if (_fromNUnit)
            {
                projInfo.IsReversalExist = !_fromNUnit;
            }
            else
            {
                projInfo.IsReversalExist = Param.Value[Param.ReversalIndex] == "True";
            }
        }

        private void SetExtraProcessingValue(PublicationInformation projInfo)
        {
            if (_fromNUnit)
            {
                projInfo.IsExtraProcessing = _fromNUnit;
            }
            else
            {
                projInfo.IsExtraProcessing = Param.Value[Param.ExtraProcessing] == "True";
            }
        }

        #endregion DeExport

        #region SeExport
        /// <summary>
        /// Exports the input files to the chosen destination
        /// </summary>
        /// <param name="mainXhtml">main dictionary content</param>
        /// <param name="jobFileName">css file with style info</param>
        /// <param name="outPath">destination path</param>
        public void SeExport(string mainXhtml, string jobFileName, string outPath)
        {
            Debug.Assert(mainXhtml.IndexOf(Path.DirectorySeparatorChar) < 0, mainXhtml + " should be just name");
            Debug.Assert(jobFileName.IndexOf(Path.DirectorySeparatorChar) < 0, jobFileName + " should be just name");
            var pb = new ProgressBar();
            Common.ShowMessage = !Common.Testing;

            var projInfo = new PublicationInformation();
            var mainSection = File.Exists(Common.PathCombine(outPath, mainXhtml));
            projInfo.DefaultCssFileWithPath = Common.PathCombine(outPath, jobFileName);
            projInfo.ProjectInputType = "Scripture";
            projInfo.FromPlugin = true;
            projInfo.DictionaryPath = outPath;
            if (mainSection)
            {
                projInfo.DefaultXhtmlFileWithPath = Common.PathCombine(outPath, mainXhtml);
                string DictionaryName = Common.PathCombine(Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath), Path.GetFileNameWithoutExtension(projInfo.DefaultXhtmlFileWithPath));
                projInfo.DictionaryOutputName = DictionaryName;
                projInfo.ProgressBar = pb;
                projInfo.IsOpenOutput = !Common.Testing;
                projInfo.ProjectName = Path.GetFileNameWithoutExtension(mainXhtml);
                SetExtraProcessingValue(projInfo);
                Backend.Launch(Destination, projInfo);
            }
        }
        #endregion SeExport
        #endregion Protected Function

    }
}