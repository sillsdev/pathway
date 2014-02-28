// --------------------------------------------------------------------------------------------
// <copyright file="ExportXeLaTexConvert.cs" from='2009' to='2014' company='SIL International'>
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
//
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class ExportXeLaTex : IExportProcess
    {

        private Dictionary<string, string> _langFontDictionary; // languages and font names in use for this export
        protected string _inputType = "dictionary";
        private bool _tableOfContent;
        private bool _coverImage;
        private bool _titleInCoverPage;
        private bool _copyrightInformation;
        private bool _includeBookTitleintheImage;
        private string _copyrightInformationPagePath;
        private string _coverPageImagePath;
        private bool _xelatexDocumentOpenClosedRequired = false;
        private bool _copyrightTexCreated = false;
        private string _copyrightTexFileName = string.Empty;
        private string _reversalIndexTexFileName = string.Empty;
        private bool _reversalIndexTexCreated = false;
        private bool _isInputTypeFound = false;
        private bool _isFileFontCodeandFontNameFound = false;
        private Dictionary<string, string> _tocPropertyList = new Dictionary<string, string>();
        private Dictionary<string, string> _langFontCodeandName;
        private Dictionary<string, string> _fontLangMap = new Dictionary<string, string>();
        #region Public Functions
        public string ExportType
        {
            get
            {
                return "XeLaTex";
            }
        }

        public bool Handle(string inputDataType)
        {
            bool returnValue = false;
            string dataType = inputDataType.ToLower();
            if (dataType == "dictionary" || dataType == "scripture")
            {
                returnValue = true;
            }
            if (string.IsNullOrEmpty(XeLaTexInstallation.GetXeLaTexDir()))
            {
                returnValue = false;
            }
            return returnValue;
        }

        /// <summary>
        /// Convert XHTML to ODT
        /// </summary>
        public bool Export(PublicationInformation projInfo)
        {
            _langFontDictionary = new Dictionary<string, string>();
            _langFontCodeandName = new Dictionary<string, string>();
            string mainXhtmlFileWithPath = projInfo.DefaultXhtmlFileWithPath;
            projInfo.OutputExtension = "pdf";
            PreExportProcess preProcessor = new PreExportProcess(projInfo);
            if (Common.IsUnixOS())
            {
                Common.RemoveDTDForLinuxProcess(projInfo.DefaultXhtmlFileWithPath);
            }
            preProcessor.SetLangforLetter(projInfo.DefaultXhtmlFileWithPath);
            preProcessor.XelatexImagePreprocess();
            Param.LoadSettings();
            string organization;
            try
            {
                // get the organization
                organization = Param.Value["Organization"];
            }
            catch (Exception)
            {
                // shouldn't happen (ExportThroughPathway dialog forces the user to select an organization), 
                // but just in case, specify a default org.
                organization = "SIL International";
            }
            _coverImage = (Param.GetMetadataValue(Param.CoverPage, organization) == null) ? false : Boolean.Parse(Param.GetMetadataValue(Param.CoverPage, organization));
            _coverPageImagePath = Param.GetMetadataValue(Param.CoverPageFilename, organization);


            _titleInCoverPage = (Param.GetMetadataValue(Param.TitlePage, organization) == null) ? false : Boolean.Parse(Param.GetMetadataValue(Param.TitlePage, organization));


            _copyrightInformation = (Param.GetMetadataValue(Param.CopyrightPage, organization) == null) ? false : Boolean.Parse(Param.GetMetadataValue(Param.CopyrightPage, organization));
            _copyrightInformationPagePath = Param.GetMetadataValue(Param.CopyrightPageFilename, organization);

            _includeBookTitleintheImage = (Param.GetMetadataValue(Param.CoverPageTitle, organization) == null) ? false : Boolean.Parse(Param.GetMetadataValue(Param.CoverPageTitle, organization));

            _tableOfContent = (Param.GetMetadataValue(Param.TableOfContents, organization) == null) ? false : Boolean.Parse(Param.GetMetadataValue(Param.TableOfContents, organization));

            BuildLanguagesList(projInfo.DefaultXhtmlFileWithPath);
            string fileName = Path.GetFileNameWithoutExtension(projInfo.DefaultXhtmlFileWithPath);
            
            if (projInfo.DefaultXhtmlFileWithPath.Contains("FlexRev.xhtml"))
            {
                projInfo.IsReversalExist = false;
            }

            projInfo.DefaultCssFileWithPath = preProcessor.ProcessedCss;
            projInfo.ProjectPath = Path.GetDirectoryName(preProcessor.ProcessedXhtml);
            projInfo.DefaultXhtmlFileWithPath = preProcessor.PreserveSpace();
            preProcessor.InsertPropertyForXelatexCss(projInfo.DefaultCssFileWithPath);
            projInfo.DefaultCssFileWithPath = preProcessor.RemoveTextIndent(projInfo.DefaultCssFileWithPath);
            ModifyXeLaTexStyles modifyXeLaTexStyles = new ModifyXeLaTexStyles();
            modifyXeLaTexStyles.LangFontDictionary = _langFontCodeandName;

            Dictionary<string, Dictionary<string, string>> cssClass = new Dictionary<string, Dictionary<string, string>>();
            CssTree cssTree = new CssTree();
            cssTree.OutputType = Common.OutputType.XELATEX;
            cssClass = cssTree.CreateCssProperty(projInfo.DefaultCssFileWithPath, true);
            int pageWidth = Common.GetPictureWidth(cssClass, projInfo.ProjectInputType);

            string xeLatexFullFile = Common.PathCombine(projInfo.ProjectPath, fileName + ".tex");
            StreamWriter xeLatexFile = new StreamWriter(xeLatexFullFile);

            Dictionary<string, List<string>> classInlineStyle = new Dictionary<string, List<string>>();
            Dictionary<string, Dictionary<string, string>> xeTexAllClass = new Dictionary<string, Dictionary<string, string>>();
            XeLaTexStyles xeLaTexStyles = new XeLaTexStyles();
            xeLaTexStyles.LangFontDictionary = _langFontCodeandName;
            classInlineStyle = xeLaTexStyles.CreateXeTexStyles(projInfo, xeLatexFile, cssClass);

            XeLaTexContent xeLaTexContent = new XeLaTexContent();
            Dictionary<string, List<string>> classInlineText = xeLaTexStyles._classInlineText;
            Dictionary<string, Dictionary<string, string>> newProperty = xeLaTexContent.CreateContent(projInfo, cssClass, xeLatexFile, classInlineStyle,
                cssTree.SpecificityClass, cssTree.CssClassOrder, classInlineText, pageWidth);

            if (projInfo.IsReversalExist)
            {
                var revFile = Common.PathCombine(Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath), "FlexRev.xhtml");
                string fileNameXhtml = Path.GetFileNameWithoutExtension(revFile);
                string reversalFileName = fileNameXhtml + ".tex";

                CloseDocument(xeLatexFile, true, reversalFileName);
            }
            else
            {
                CloseDocument(xeLatexFile, false, string.Empty);
            }

            string include = xeLaTexStyles.PageStyle.ToString();

            modifyXeLaTexStyles.ProjectType = _inputType;
            modifyXeLaTexStyles.TocChecked = _tableOfContent.ToString();

            modifyXeLaTexStyles.CoverImage = _coverImage.ToString();
            modifyXeLaTexStyles.TitleInCoverPage = _titleInCoverPage.ToString();
            modifyXeLaTexStyles.CopyrightInformation = _copyrightInformation.ToString();
            modifyXeLaTexStyles.IncludeBookTitleintheImage = _includeBookTitleintheImage.ToString();
            modifyXeLaTexStyles.CopyrightInformationPagePath = _copyrightInformationPagePath;
            modifyXeLaTexStyles.CoverPageImagePath = _coverPageImagePath;

            if (ExportCopyright(projInfo, mainXhtmlFileWithPath))
            {
                _copyrightTexCreated = true;
                modifyXeLaTexStyles.CopyrightTexCreated = true;
                modifyXeLaTexStyles.CopyrightTexFilename = Path.GetFileName(_copyrightTexFileName);
            }

            if (projInfo.IsReversalExist)
            {
                if (ExportReversalIndex(projInfo))
                {
                    _reversalIndexTexCreated = true;
                    var revFile = Common.PathCombine(Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath),
                                                     "FlexRev.xhtml");
                    string fileNameXhtml = Path.GetFileNameWithoutExtension(revFile);
                    string xeLatexReversalFile = Common.PathCombine(projInfo.ProjectPath, fileNameXhtml + ".tex");

                    modifyXeLaTexStyles.ReversalIndexExist = true;
                    modifyXeLaTexStyles.ReversalIndexTexFilename = Path.GetFileName(xeLatexReversalFile);
                }
            }

            modifyXeLaTexStyles.XelatexDocumentOpenClosedRequired = false;
            _xelatexDocumentOpenClosedRequired = false;
            modifyXeLaTexStyles.ProjectType = projInfo.ProjectInputType;

            if (newProperty.ContainsKey("TableofContent") && newProperty["TableofContent"].Count > 0)
            {
                foreach (var tocSection in _tocPropertyList)
                {
                    if (tocSection.Key.Contains("PageStock"))
                    {
                        newProperty["TableofContent"].Add(tocSection.Key, tocSection.Value);
                    }
                }
            }

            modifyXeLaTexStyles.ModifyStylesXML(projInfo.ProjectPath, xeLatexFile, newProperty, cssClass, xeLatexFullFile, include, _langFontCodeandName);
            Dictionary<string, string> imgPath = new Dictionary<string, string>();
            if (newProperty.ContainsKey("ImagePath"))
            {
                imgPath = newProperty["ImagePath"];
            }
            UpdateXeLaTexFontCacheIfNecessary();
            CallXeLaTex(xeLatexFullFile, true, imgPath);
            ProcessRampFile(projInfo, xeLatexFullFile, organization);
            return true;
        }

        private void ProcessRampFile(PublicationInformation projInfo, string xeLatexFullFile, string organization)
        {
            string pdfFullName = string.Empty;
            string texNameOnly = Path.GetFileNameWithoutExtension(xeLatexFullFile);
            string userFolder = Path.GetDirectoryName(xeLatexFullFile);

            string exportTitle = string.Empty;
            exportTitle = Param.GetMetadataValue(Param.Title, organization);

            if (exportTitle == string.Empty)
            {
                exportTitle = texNameOnly;
            }

            if (userFolder != null)
                pdfFullName = Common.PathCombine(userFolder, exportTitle + ".pdf");

            if (File.Exists(pdfFullName))
            {
                Common.CleanupExportFolder(xeLatexFullFile, ".tmp,.de,.jpg,.tif,.tex,.log,.exe,.xml,.jar",
                                           "layout,mergedmain1,preserve", string.Empty);
                CreateRAMP(projInfo);
            }
        }

        private void CreateRAMP(PublicationInformation projInfo)
        {
            Ramp ramp = new Ramp();
            ramp.Create(projInfo.DefaultXhtmlFileWithPath, ".pdf", projInfo.ProjectInputType);
        }

        public bool ExportCopyright(PublicationInformation projInfo, string mainXhtmlFileWithPath)
        {
            if (_copyrightInformation)
            {
                var preProcess = new PreExportProcess(projInfo);
                var processFolder = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath);
                preProcess.CopyCopyrightPage(processFolder);
                string copyRightFilePath = Common.PathCombine(processFolder, "File2Cpy.xhtml");

                if (copyRightFilePath.Trim().Length <= 0 && !File.Exists(copyRightFilePath))
                {
                    return false;
                }
                if (File.Exists(copyRightFilePath))
                {
                    if (Common.UnixVersionCheck())
                    {
                        string draftTempFileName = Path.GetFileName(copyRightFilePath);
                        draftTempFileName = Common.PathCombine(Path.GetTempPath(), draftTempFileName);
                        if (!File.Exists(draftTempFileName))
                        {
                            File.Copy(copyRightFilePath, draftTempFileName, true);
                            Common.RemoveDTDForLinuxProcess(draftTempFileName);
                        }
                        projInfo.DefaultXhtmlFileWithPath = draftTempFileName;
                        copyRightFilePath = draftTempFileName;
                    }
                    else
                    {
                        projInfo.DefaultXhtmlFileWithPath = copyRightFilePath;
                    }
                }
                else
                {
                    return false;
                }

                string filepath = Path.GetFullPath(copyRightFilePath);

                Dictionary<string, Dictionary<string, string>> cssClass =
                    new Dictionary<string, Dictionary<string, string>>();
                CssTree cssTree = new CssTree();
                cssTree.OutputType = Common.OutputType.XELATEX;
                cssClass = cssTree.CreateCssProperty(Common.PathCombine(filepath, "copy.css"), true);
                string fileNameXhtml = Path.GetFileNameWithoutExtension(copyRightFilePath);
                string xeLatexCopyrightFile = Common.PathCombine(projInfo.ProjectPath, fileNameXhtml + ".tex");
                _copyrightTexFileName = xeLatexCopyrightFile;
                int pageWidth = Common.GetPictureWidth(cssClass, projInfo.ProjectInputType);

                StreamWriter xeLatexFile = new StreamWriter(xeLatexCopyrightFile);
                Dictionary<string, List<string>> classInlineStyle = new Dictionary<string, List<string>>();
                Dictionary<string, Dictionary<string, string>> xeTexAllClass =
                    new Dictionary<string, Dictionary<string, string>>();
                XeLaTexStyles xeLaTexStyles = new XeLaTexStyles();
                xeLaTexStyles.LangFontDictionary = _langFontCodeandName;
                classInlineStyle = xeLaTexStyles.CreateXeTexStyles(projInfo, xeLatexFile, cssClass);

                XeLaTexContent xeLaTexContent = new XeLaTexContent();
                Dictionary<string, List<string>> classInlineText = xeLaTexStyles._classInlineText;
                Dictionary<string, Dictionary<string, string>> newProperty = xeLaTexContent.CreateContent(projInfo,
                                                                                                          cssClass,
                                                                                                          xeLatexFile,
                                                                                                          classInlineStyle,
                                                                                                          cssTree.
                                                                                                              SpecificityClass,
                                                                                                          cssTree.
                                                                                                              CssClassOrder,
                                                                                                          classInlineText, pageWidth);

                _xelatexDocumentOpenClosedRequired = true; //Don't change the place.
                CloseDocument(xeLatexFile, false, string.Empty);
                string include = xeLaTexStyles.PageStyle.ToString();
                ModifyXeLaTexStyles modifyXeLaTexStyles = new ModifyXeLaTexStyles();
                modifyXeLaTexStyles.XelatexDocumentOpenClosedRequired = true;
                modifyXeLaTexStyles.ProjectType = projInfo.ProjectInputType;
                modifyXeLaTexStyles.ModifyStylesXML(projInfo.ProjectPath, xeLatexFile, newProperty, cssClass,
                                                    xeLatexCopyrightFile, include, _langFontCodeandName);

                string copyright = GetLanguageInfo(mainXhtmlFileWithPath, projInfo);
                InsertInFile(xeLatexCopyrightFile, "copyright information", copyright);

                return true;
            }
            return false;
        }
        static public void InsertInFile(string filePath, string searchText, string insertText)
        {
            if (!File.Exists(filePath)) return;
            string tempFile = Common.PathCombine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath) + "1.tex");
            File.Copy(filePath, tempFile, true);
            var reader = new StreamReader(tempFile);
            string contentWriter;
            var writer = new StreamWriter(filePath);
            while ((contentWriter = reader.ReadLine()) != null)
            {
                if (contentWriter.ToLower().IndexOf(searchText) >= 0)
                {
                    writer.WriteLine(insertText);
                    writer.WriteLine(contentWriter);
                    string st = string.Empty;
                    string contributors = Param.GetMetadataValue(Param.Contributor);
                    if (contributors.Trim().Length > 0)
                    {
                        st = "(" + contributors + "), ";
                    }
                    string rights = Common.UpdateCopyrightYear(Param.GetMetadataValue(Param.CopyrightHolder));
                    if (rights.Trim().Length > 0)
                    {
                        st = st + rights;
                    }

                    writer.WriteLine(@"\OtherCopyrights{" + st + @"}\end{adjustwidth}");
                    writer.WriteLine("\\mbox{}");

                }
                else
                {
                    writer.WriteLine(contentWriter);
                }
            }
            reader.Close();
            writer.Close();
            File.Delete(tempFile);
        }

        private string GetLanguageInfo(string mainXhtmlFileWithPath, PublicationInformation projInfo)
        {
            var sb = new StringBuilder();
            // append what we know about this language, including a hyperlink to the ethnologue.
            string languageCode = Common.GetLanguageCode(mainXhtmlFileWithPath, projInfo.ProjectInputType);
            if (languageCode.Length > 0)
            {
                var languageName = Common.GetLanguageName(languageCode);

                sb.AppendLine(@"\empFrontMatterdiv{ABOUT THIS DOCUMENT}\end{adjustwidth}");

                string txt = "This document contains data written in ";
                if (languageName.Length > 0)
                {
                    txt = txt + languageName;
                }
                txt = txt + "[" + languageCode + "].";
                sb.AppendLine(@"\empFrontMatterdiv{" + txt + @"}\end{adjustwidth}");

                txt = " For more information about this language, visit http://www.ethnologue.com/show_language.asp?code=";
                var codeLen = languageCode.Length > 3 ? 3 : languageCode.Length;
                txt = txt + languageCode.Substring(0, codeLen);
                sb.Append(@"\empFrontMatterdiv{" + txt + @" \newline \newline}\end{adjustwidth}");
            }
            return sb.ToString();
        }

        public bool ExportReversalIndex(PublicationInformation projInfo)
        {

            if (projInfo.IsReversalExist)
            {
                var revFile = Common.PathCombine(projInfo.ProjectPath, "FlexRev.xhtml");
                if (!File.Exists(revFile))
                {
                    return false;
                }

                if (File.Exists(revFile))
                {
                    if (Common.UnixVersionCheck())
                    {
                        Common.RemoveDTDForLinuxProcess(revFile);
                    }
                }

                projInfo.DefaultXhtmlFileWithPath = revFile;
                PreExportProcess preProcessor = new PreExportProcess(projInfo);
                preProcessor.SetLangforLetter(projInfo.DefaultXhtmlFileWithPath);
                Dictionary<string, Dictionary<string, string>> cssClass =
                    new Dictionary<string, Dictionary<string, string>>();
                CssTree cssTree = new CssTree();
                cssTree.OutputType = Common.OutputType.XELATEX;
                cssClass = cssTree.CreateCssProperty(projInfo.DefaultRevCssFileWithPath, true);
                string fileNameXhtml = Path.GetFileNameWithoutExtension(revFile);
                string xeLatexRevesalIndexFile = Common.PathCombine(projInfo.ProjectPath, fileNameXhtml + ".tex");
                _reversalIndexTexFileName = xeLatexRevesalIndexFile;
                StreamWriter xeLatexFile = new StreamWriter(xeLatexRevesalIndexFile);
                Dictionary<string, List<string>> classInlineStyle = new Dictionary<string, List<string>>();
                XeLaTexStyles xeLaTexStyles = new XeLaTexStyles();
                xeLaTexStyles.LangFontDictionary = _langFontCodeandName;
                classInlineStyle = xeLaTexStyles.CreateXeTexStyles(projInfo, xeLatexFile, cssClass);
                int pageWidth = Common.GetPictureWidth(cssClass, projInfo.ProjectInputType);

                XeLaTexContent xeLaTexContent = new XeLaTexContent();
                Dictionary<string, List<string>> classInlineText = xeLaTexStyles._classInlineText;
                Dictionary<string, Dictionary<string, string>> newProperty = xeLaTexContent.CreateContent(projInfo,
                                                                                                          cssClass,
                                                                                                          xeLatexFile,
                                                                                                          classInlineStyle,
                                                                                                          cssTree
                                                                                                              .SpecificityClass,
                                                                                                          cssTree
                                                                                                              .CssClassOrder,
                                                                                                          classInlineText,
                                                                                                          pageWidth);

                _xelatexDocumentOpenClosedRequired = true; //Don't change the place.
                CloseDocument(xeLatexFile, false, string.Empty);
                string include = xeLaTexStyles.PageStyle.ToString();
                ModifyXeLaTexStyles modifyXeLaTexStyles = new ModifyXeLaTexStyles();
                modifyXeLaTexStyles.XelatexDocumentOpenClosedRequired = true;
                modifyXeLaTexStyles.ProjectType = projInfo.ProjectInputType;
                modifyXeLaTexStyles.ModifyStylesXML(projInfo.ProjectPath, xeLatexFile, newProperty, cssClass,
                                                    xeLatexRevesalIndexFile, include, _langFontCodeandName);


                if (newProperty.ContainsKey("TableofContent") && newProperty["TableofContent"].Count > 0)
                {
                    foreach (var tocSection in newProperty["TableofContent"])
                    {
                        if (tocSection.Key.Contains("PageStock"))
                        {
                            _tocPropertyList.Add(tocSection.Key, tocSection.Value);
                        }
                    }
                }
                return true;
            }
            return false;
        }

        protected void UpdateXeLaTexFontCacheIfNecessary()
        {
            Debug.Assert(XeLaTexInstallation.GetXeLaTexDir() != "");
            var systemFontList = FontFamily.Families;
            if (systemFontList.Length != XeLaTexInstallation.GetXeLaTexFontCount())
            {
                using (var p2 = new Process())
                {
                    var xelatexPath = XeLaTexInstallation.GetXeLaTexDir();
                    if (!Common.IsUnixOS())
                    {
                        xelatexPath = Common.PathCombine(xelatexPath, "bin");
                        xelatexPath = Common.PathCombine(xelatexPath, "win32");
                    }
                    p2.StartInfo.WorkingDirectory = xelatexPath;
                    p2.StartInfo.FileName = "fc-cache";
                    p2.StartInfo.Arguments = "-v -r";
                    p2.Start();
                    p2.WaitForExit();
                }
                XeLaTexInstallation.SetXeLaTexFontCount(systemFontList.Length);
            }
        }

        public void CallXeLaTex(string xeLatexFullFile, bool openFile, Dictionary<string, string> ImageFilePath)
        {

            string[] pdfFiles = Directory.GetFiles(Path.GetDirectoryName(xeLatexFullFile), "*.pdf");
            foreach (string pdfFile in pdfFiles)
            {
                try
                {
                    File.Delete(pdfFile);
                }
                catch { }
            }


            bool isUnixOs = Common.IsUnixOS();
            string xeLaTexInstallationPath = XeLaTexInstallation.GetXeLaTexDir();

            if (!Directory.Exists(xeLaTexInstallationPath))
            {
                MessageBox.Show("Please install the Xelatex application.");
                return;
            }

            string name = "xelatex.exe";
            string arguments = "-interaction=batchmode \"" + Path.GetFileName(xeLatexFullFile) + "\"";
            if (isUnixOs)
            {
                string path = Environment.GetEnvironmentVariable("PATH");
                Debug.Assert(path != null);
                if (!path.Contains(xeLaTexInstallationPath))
                {
                    Environment.SetEnvironmentVariable("PATH", string.Format("{0}:{1}", xeLaTexInstallationPath, path));
                }
                xeLaTexInstallationPath = Path.GetDirectoryName(xeLatexFullFile);
                name = "xelatex";
                arguments = "-interaction=batchmode \"" + xeLatexFullFile + "\"";
            }
            else
            {
                xeLaTexInstallationPath = Common.PathCombine(xeLaTexInstallationPath, "bin");
                xeLaTexInstallationPath = Common.PathCombine(xeLaTexInstallationPath, "win32");
            }


            string originalDirectory = Directory.GetCurrentDirectory();
            string dest = Common.PathCombine(xeLaTexInstallationPath, Path.GetFileName(xeLatexFullFile));

            if (xeLatexFullFile != dest)
                File.Copy(xeLatexFullFile, dest, true);

            if (_copyrightTexCreated)
            {
                string copyrightDest = Common.PathCombine(xeLaTexInstallationPath, Path.GetFileName(_copyrightTexFileName));
                if (_copyrightTexFileName != copyrightDest)
                    File.Copy(_copyrightTexFileName, copyrightDest, true);
            }

            if (_reversalIndexTexCreated)
            {
                string copyrightDest = Common.PathCombine(xeLaTexInstallationPath, Path.GetFileName(_reversalIndexTexFileName));
                if (_reversalIndexTexFileName != copyrightDest)
                    File.Copy(_reversalIndexTexFileName, copyrightDest, true);
            }

            Directory.SetCurrentDirectory(xeLaTexInstallationPath);

            string p1Error = string.Empty;
            using (Process p1 = new Process())
            {
                p1.StartInfo.FileName = name;
                if (xeLatexFullFile != null)
                    p1.StartInfo.Arguments = arguments;
                p1.StartInfo.RedirectStandardOutput = true;
                p1.StartInfo.RedirectStandardError = p1.StartInfo.RedirectStandardOutput;
                p1.StartInfo.UseShellExecute = !p1.StartInfo.RedirectStandardOutput;
                p1.Start();
                p1.WaitForExit();
                p1Error = p1.StandardError.ReadToEnd();
            }

            if (Convert.ToBoolean(_tableOfContent))
            {
                using (Process p1 = new Process())
                {
                    p1.StartInfo.FileName = name;
                    if (xeLatexFullFile != null)
                        p1.StartInfo.Arguments = arguments;
                    p1.StartInfo.RedirectStandardOutput = true;
                    p1.StartInfo.RedirectStandardError = p1.StartInfo.RedirectStandardOutput;
                    p1.StartInfo.UseShellExecute = !p1.StartInfo.RedirectStandardOutput;
                    p1.Start();
                    p1.WaitForExit();
                    p1Error = p1.StandardError.ReadToEnd();
                }

                using (Process p1 = new Process())
                {
                    p1.StartInfo.FileName = name;
                    if (xeLatexFullFile != null)
                        p1.StartInfo.Arguments = arguments;
                    p1.StartInfo.RedirectStandardOutput = true;
                    p1.StartInfo.RedirectStandardError = p1.StartInfo.RedirectStandardOutput;
                    p1.StartInfo.UseShellExecute = !p1.StartInfo.RedirectStandardOutput;
                    p1.Start();
                    p1.WaitForExit();
                    p1Error = p1.StandardError.ReadToEnd();
                }

            }
            string pdfFullName = string.Empty;
            string texNameOnly = Path.GetFileNameWithoutExtension(xeLatexFullFile);
            string userFolder = Path.GetDirectoryName(xeLatexFullFile);

            if (isUnixOs)
            {
                if (userFolder != null)
                    pdfFullName = Common.PathCombine(userFolder, texNameOnly + ".pdf");

                if (File.Exists(pdfFullName))
                {
                    try
                    {
                        if (File.Exists(pdfFullName))
                        {

                            pdfFullName = Common.InsertCopyrightInPdf(pdfFullName, "XeLaTex", _inputType);
                        }
                    }
                    catch { }
                }
            }
            else
            {
                Directory.SetCurrentDirectory(originalDirectory);
                pdfFullName = CopyProcessResult(xeLaTexInstallationPath, texNameOnly, ".pdf", userFolder);
                string logFullName = CopyProcessResult(xeLaTexInstallationPath, texNameOnly, ".log", userFolder);

                if (openFile && File.Exists(pdfFullName))
                {
                    try
                    {
                        if (File.Exists(pdfFullName))
                        {
                            pdfFullName = Common.InsertCopyrightInPdf(pdfFullName, "XeLaTex", _inputType);
                        }
                    }
                    catch (System.ComponentModel.Win32Exception ex)
                    {
                        if (ex.NativeErrorCode == 1155)
                        {
                            if (File.Exists(pdfFullName))
                            {
                                string installedLocation = pdfFullName;
                                MessageBox.Show("The output has been save in " + installedLocation +
                                                ".\n Please install the Xelatex application.");
                            }
                        }
                    }

                }
                try
                {
                    File.Delete(Common.PathCombine(xeLaTexInstallationPath, texNameOnly + ".log"));
                    File.Delete(Common.PathCombine(xeLaTexInstallationPath, texNameOnly + ".pdf"));
                    File.Delete(Common.PathCombine(xeLaTexInstallationPath, texNameOnly + ".aux"));

                    string[] picList = Directory.GetFiles(xeLaTexInstallationPath, "*.jpg");
                    foreach (string picturefile in picList)
                    {
                        File.Delete(picturefile);
                    }

                    File.Delete(dest);
                }
                catch
                {
                }
            }
        }

        protected static string CopyProcessResult(string instPath, string texNameOnly, string ext, string userFolder)
        {
            const bool overwrite = true;
            string logName = texNameOnly + ext;
            string tmpLogFullName = Common.PathCombine(instPath, logName);
            string logFullName = Common.PathCombine(userFolder, logName);
            if (File.Exists(tmpLogFullName))
                File.Copy(tmpLogFullName, logFullName, overwrite);
            return logFullName;
        }

        private void CloseDocument(StreamWriter xeLatexFile, bool insertReversalIndexFile, string reversalFileName)
        {
            String ReversalIndexContent = string.Empty;
            if (insertReversalIndexFile)
            {

                xeLatexFile.WriteLine();
                ReversalIndexContent += "\\input{" + reversalFileName + "} \r\n";
                xeLatexFile.WriteLine(ReversalIndexContent);
            }

            if (!_xelatexDocumentOpenClosedRequired)
            {
                xeLatexFile.WriteLine();
                xeLatexFile.WriteLine(@"\end{document}");
            }

            xeLatexFile.Flush();
            xeLatexFile.Close();
        }

        #region Language Handling
        /// <summary>
        /// Parses the specified file and sets the internal languages list to all the languages found in the file.
        /// </summary>
        /// <param name="xhtmlFileName">File name to parse</param>
        private void BuildLanguagesList(string xhtmlFileName)
        {
            bool isInputTypeFound = false;
            XmlTextReader _reader = Common.DeclareXmlTextReader(xhtmlFileName, true);
            while (_reader.Read())
            {
                if (_reader.NodeType == XmlNodeType.Element)
                {
                    GetXhtmlFileFontCodeandFontName(_reader);
                    if (_reader.Name == "div" || _reader.Name == "span")
                    {
                        FindInputType(_reader);
                        string name = _reader.GetAttribute("lang");
                        if (name != null)
                        {
                            if (_langFontDictionary.ContainsKey(name) == false && name.ToLower() != "utf-8")
                            {
                                _langFontDictionary.Add(name, "*");
                            }
                        }
                    }
                }
            }
            _reader.Close();
            CreateFontLanguageMap();

        }

        private void CreateFontLanguageMap()
        {
            Common.FillMappedFonts(_fontLangMap);

            foreach (var fontName in _fontLangMap)
            {
                if (_langFontCodeandName.ContainsKey(fontName.Key))
                {
                    if (fontName.Key.ToLower() != "en")
                    {
                        _langFontCodeandName.Remove(fontName.Key);
                        _langFontCodeandName.Add(fontName.Key, fontName.Value);
                    }
                }

                if (_langFontDictionary.ContainsKey(fontName.Key))
                {
                    if (fontName.Key.ToLower() != "en")
                    {
                        _langFontDictionary.Remove(fontName.Key);
                        _langFontDictionary.Add(fontName.Key, fontName.Value);
                    }
                }
            }
        }

        #endregion

        private void GetXhtmlFileFontCodeandFontName(XmlTextReader _reader)
        {

            if (_isFileFontCodeandFontNameFound)
            {
                return;
            }

            if (_reader.Name == "meta")
            {
                string name = _reader.GetAttribute("name");
                string content = _reader.GetAttribute("content");
                if (name != null && content != null)
                {
                    FontFamily[] systemFontList = System.Drawing.FontFamily.Families;
                    foreach (FontFamily systemFont in systemFontList)
                    {
                        if (content.ToLower() == systemFont.Name.ToLower())
                        {
                            _langFontCodeandName.Add(name, content);
                            break;
                        }
                    }
                }
            }
            else if (_reader.Name == "body")
            {
                _isFileFontCodeandFontNameFound = true;
            }

        }

        #region Find InputType
        /// <summary>
        /// now go check to see if we're working on scripture or dictionary data
        /// </summary>
        /// <param name="xhtmlFileName">File name to parse</param>
        private void FindInputType(XmlTextReader _reader)
        {
            if (_isInputTypeFound)
            {
                return;
            }

            string name = _reader.GetAttribute("class");
            if (name != null)
            {
                if (name == "headword")
                {
                    _inputType = "scripture";
                    _isInputTypeFound = true;
                }
                else if (name == "scrBookName")
                {
                    _inputType = "dictionary";
                    _isInputTypeFound = true;
                }
            }

        }
        #endregion

        /// <summary>
        /// Parses the specified file and sets the internal languages list to all the languages found in the file.
        /// </summary>
        /// <param name="xhtmlFileName">File name to parse</param>
        private void BuildLanguagesListOLD(string xhtmlFileName)
        {
            XmlDocument xmlDocument = new XmlDocument { XmlResolver = null };
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, ProhibitDtd = false };
            XmlReader xmlReader = XmlReader.Create(xhtmlFileName, xmlReaderSettings);
            xmlDocument.Load(xmlReader);
            xmlReader.Close();
            // should only be one of these after splitting out the chapters.
            XmlNodeList nodes;
            nodes = xmlDocument.SelectNodes("//@lang", namespaceManager);
            if (nodes.Count > 0)
            {
                foreach (XmlNode node in nodes)
                {
                    string value;
                    if (_langFontDictionary.TryGetValue(node.Value, out value))
                    {
                        // already have this item in our list - continue
                        continue;
                    }
                    if (node.Value.ToLower() == "utf-8")
                    {
                        // TE-9078 "utf-8" showing up as language in html tag - remove when fixed
                        continue;
                    }
                    // add an entry for this language in the list (the * gets overwritten in BuildFontsList())
                    _langFontDictionary.Add(node.Value, "*");
                }
            }



            // now go check to see if we're working on scripture or dictionary data
            nodes = xmlDocument.SelectNodes("//xhtml:span[@class='headword']", namespaceManager);
            if (nodes.Count == 0)
            {
                // not in this file - this might be scripture?
                nodes = xmlDocument.SelectNodes("//xhtml:span[@class='scrBookName']", namespaceManager);
                if (nodes.Count > 0)
                    _inputType = "scripture";
            }
            else
            {
                _inputType = "dictionary";
            }
        }

        private void GetXhtmlFileFontCodeandFontNameOLD(string xhtmlFileName)
        {
            if (!File.Exists(xhtmlFileName)) return;
            XmlDocument xdoc = new XmlDocument { XmlResolver = null };
            xdoc.Load(xhtmlFileName);
            XmlNodeList metaNodes = xdoc.GetElementsByTagName("meta");
            if (metaNodes != null && metaNodes.Count > 0)
            {
                try
                {
                    foreach (XmlNode metaNode in metaNodes)
                    {
                        FontFamily[] systemFontList = System.Drawing.FontFamily.Families;
                        foreach (FontFamily systemFont in systemFontList)
                        {
                            if (metaNode.Attributes["content"].Value.ToLower() == systemFont.Name.ToLower())
                            {
                                _langFontCodeandName.Add(metaNode.Attributes["name"].Value, metaNode.Attributes["content"].Value);
                                break;
                            }
                        }
                    }
                }
                catch { }
            }
        }

        #endregion
    }
}
