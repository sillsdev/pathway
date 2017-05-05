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
using System.Xml.Xsl;
using L10NSharp;
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
        private readonly XslCompiledTransform _xhtmlXelatexXslProcess = new XslCompiledTransform();
        private List<string> _xeLaTexPropertyFullFontStyleList = new List<string>();
        private bool _isUnixOs = false;
        private string paraTextEnvVariable = string.Empty;
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
            string xeLatexFullFile;
            StreamWriter xeLatexFile;
            XeLaTexStyles xeLaTexStyles;
            _xhtmlXelatexXslProcess.Load(XmlReader.Create(Common.UsersXsl("AddBidi.xsl")));
            _isUnixOs = Common.IsUnixOS();
            _langFontDictionary = new Dictionary<string, string>();
            _langFontCodeandName = new Dictionary<string, string>();
            string mainXhtmlFileWithPath = projInfo.DefaultXhtmlFileWithPath;
            projInfo.OutputExtension = "pdf";
			ModifyCssStyle(projInfo);
            var preProcessor = new PreExportProcess(projInfo);
            ExportPreprocessForXelatex(projInfo, preProcessor);
            var organization = SettingFrontmatter();
            BuildLanguagesList(projInfo.DefaultXhtmlFileWithPath);

            string fileName = Path.GetFileNameWithoutExtension(projInfo.DefaultXhtmlFileWithPath);

            AssignExportFile(projInfo, preProcessor);
            ModifyXeLaTexStyles modifyXeLaTexStyles = new ModifyXeLaTexStyles();
            modifyXeLaTexStyles.LangFontDictionary = _langFontCodeandName;
	        modifyXeLaTexStyles.ProjectType = projInfo.ProjectInputType;
            Dictionary<string, Dictionary<string, string>> newProperty;
            var cssClass = WrittingTexFile(projInfo, fileName, out xeLatexFullFile, out xeLatexFile, out xeLaTexStyles, out newProperty);
            string include = xeLaTexStyles.PageStyle.ToString();

            InitilizeXelatexStyle(modifyXeLaTexStyles);

            if (ExportCopyright(projInfo, mainXhtmlFileWithPath))
            {
                _copyrightTexCreated = true;
                modifyXeLaTexStyles.CopyrightTexCreated = true;
                modifyXeLaTexStyles.CopyrightTexFilename = Path.GetFileName(_copyrightTexFileName);
            }

            ExportReversalProcess(projInfo, modifyXeLaTexStyles);
            ProcessWrittingStyles(projInfo, modifyXeLaTexStyles, newProperty, xeLatexFile, cssClass, xeLatexFullFile, include);
            Dictionary<string, string> imgPath = new Dictionary<string, string>();
            if (newProperty.ContainsKey("ImagePath"))
            {
                imgPath = newProperty["ImagePath"];
            }
            UpdateXeLaTexFontCacheIfNecessary();

	        if (Common.Testing)
	        {
		        return true;
	        }

	        CallXeLaTex(projInfo, xeLatexFullFile, true, imgPath);
	        if (!Common.Testing)
	        {
		        ProcessRampFile(projInfo, xeLatexFullFile, organization);
	        }
	        return true;
        }

		public void ModifyCssStyle(PublicationInformation projInfo)
		{
			if (projInfo.ProjectInputType.ToLower() == "scripture")
			{
				TextWriter tw = new StreamWriter(projInfo.DefaultCssFileWithPath, true);
				tw.WriteLine(".Intro_Paras{");
				tw.WriteLine("text-indent: 20pt;");
				tw.WriteLine("}");
				tw.Close();
			}
		}

        private Dictionary<string, Dictionary<string, string>> WrittingTexFile(PublicationInformation projInfo, string fileName, out string xeLatexFullFile,
                                           out StreamWriter xeLatexFile, out XeLaTexStyles xeLaTexStyles,
                                           out Dictionary<string, Dictionary<string, string>> newProperty)
        {
            CheckFontFamilyAvailable(projInfo.DefaultCssFileWithPath);

            Dictionary<string, Dictionary<string, string>> cssClass = new Dictionary<string, Dictionary<string, string>>();
            CssTree cssTree = new CssTree();
            cssTree.OutputType = Common.OutputType.XELATEX;
            cssClass = cssTree.CreateCssProperty(projInfo.DefaultCssFileWithPath, true);
            int pageWidth = Common.GetPictureWidth(cssClass, projInfo.ProjectInputType);

            xeLatexFullFile = Common.PathCombine(projInfo.ProjectPath, fileName + ".tex");
            xeLatexFile = new StreamWriter(xeLatexFullFile);

            Dictionary<string, List<string>> classInlineStyle = new Dictionary<string, List<string>>();
            xeLaTexStyles = new XeLaTexStyles();
            xeLaTexStyles.LangFontDictionary = _langFontCodeandName;
            classInlineStyle = xeLaTexStyles.CreateXeTexStyles(projInfo, xeLatexFile, cssClass);

            XeLaTexContent xeLaTexContent = new XeLaTexContent();
            xeLaTexContent.IsUnix = _isUnixOs;
            Dictionary<string, List<string>> classInlineText = xeLaTexStyles._classInlineText;
            newProperty = xeLaTexContent.CreateContent(projInfo, cssClass, xeLatexFile, classInlineStyle,
                                                       cssTree.SpecificityClass, cssTree.CssClassOrder, classInlineText,
                                                       pageWidth);

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
            return cssClass;
        }

        private void ProcessWrittingStyles(PublicationInformation projInfo, ModifyXeLaTexStyles modifyXeLaTexStyles, Dictionary<string, Dictionary<string, string>> newProperty,
                              StreamWriter xeLatexFile, Dictionary<string, Dictionary<string, string>> cssClass, string xeLatexFullFile, string include)
        {
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

            modifyXeLaTexStyles.XeLaTexPropertyFontStyleList = _xeLaTexPropertyFullFontStyleList;
            modifyXeLaTexStyles.ModifyStylesXML(projInfo.ProjectPath, xeLatexFile, newProperty, cssClass, xeLatexFullFile,
                                                include, _langFontCodeandName, true);
        }

        private void InitilizeXelatexStyle(ModifyXeLaTexStyles modifyXeLaTexStyles)
        {
            modifyXeLaTexStyles.ProjectType = _inputType;
            modifyXeLaTexStyles.TocChecked = _tableOfContent.ToString();

            modifyXeLaTexStyles.CoverImage = _coverImage.ToString();
            modifyXeLaTexStyles.TitleInCoverPage = _titleInCoverPage.ToString();
            modifyXeLaTexStyles.CopyrightInformation = _copyrightInformation.ToString();
            modifyXeLaTexStyles.IncludeBookTitleintheImage = _includeBookTitleintheImage.ToString();
            modifyXeLaTexStyles.CopyrightInformationPagePath = _copyrightInformationPagePath;
            modifyXeLaTexStyles.CoverPageImagePath = _coverPageImagePath;
        }

        private void ExportReversalProcess(PublicationInformation projInfo, ModifyXeLaTexStyles modifyXeLaTexStyles)
        {
            if (projInfo.IsReversalExist)
            {
                if (_inputType.ToLower() == "dictionary")
                {
                    Common.ApplyXslt(projInfo.DefaultXhtmlFileWithPath, _xhtmlXelatexXslProcess);
                }
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
        }

        private static void AssignExportFile(PublicationInformation projInfo, PreExportProcess preProcessor)
        {
            if (projInfo.DefaultXhtmlFileWithPath.Contains("FlexRev.xhtml"))
            {
                projInfo.IsReversalExist = false;
            }

            projInfo.DefaultCssFileWithPath = preProcessor.ProcessedCss;
            projInfo.ProjectPath = Path.GetDirectoryName(preProcessor.ProcessedXhtml);
            projInfo.DefaultXhtmlFileWithPath = preProcessor.PreserveSpace();
            preProcessor.InsertPropertyForXelatexCss(projInfo.DefaultCssFileWithPath);
            projInfo.DefaultCssFileWithPath = preProcessor.RemoveTextIndent(projInfo.DefaultCssFileWithPath);
        }

        private string SettingFrontmatter()
        {
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
            _coverImage = (Param.GetMetadataValue(Param.CoverPage, organization) == null)
                              ? false
                              : Boolean.Parse(Param.GetMetadataValue(Param.CoverPage, organization));
            _coverPageImagePath = Param.GetMetadataValue(Param.CoverPageFilename, organization);


            _titleInCoverPage = (Param.GetMetadataValue(Param.TitlePage, organization) == null)
                                    ? false
                                    : Boolean.Parse(Param.GetMetadataValue(Param.TitlePage, organization));


            _copyrightInformation = (Param.GetMetadataValue(Param.CopyrightPage, organization) == null)
                                        ? false
                                        : Boolean.Parse(Param.GetMetadataValue(Param.CopyrightPage, organization));
            _copyrightInformationPagePath = Param.GetMetadataValue(Param.CopyrightPageFilename, organization);

            _includeBookTitleintheImage = (Param.GetMetadataValue(Param.CoverPageTitle, organization) == null)
                                              ? false
                                              : Boolean.Parse(Param.GetMetadataValue(Param.CoverPageTitle, organization));

            _tableOfContent = (Param.GetMetadataValue(Param.TableOfContents, organization) == null)
                                  ? false
                                  : Boolean.Parse(Param.GetMetadataValue(Param.TableOfContents, organization));
            return organization;
        }

        private void ExportPreprocessForXelatex(PublicationInformation projInfo, PreExportProcess preProcessor)
        {
            if (_isUnixOs)
            {
                Common.RemoveDTDForLinuxProcess(projInfo.DefaultXhtmlFileWithPath, "xelatex");
            }
            preProcessor.SetLangforLetter(projInfo.DefaultXhtmlFileWithPath);
            preProcessor.XelatexImagePreprocess();

            if (_inputType.ToLower() == "dictionary" && projInfo.ProjectInputType.ToLower() == "dictionary")
            {
                Common.ApplyXslt(projInfo.DefaultXhtmlFileWithPath, _xhtmlXelatexXslProcess);
            }
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
                Common.CleanupExportFolder(xeLatexFullFile, ".tmp,.de,.jpg,.tif,.log,.exe,.xml,.jar",
                                           "layout.css,mergedmain1,preserve", string.Empty);
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
                            Common.RemoveDTDForLinuxProcess(draftTempFileName, "xelatex");
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
                XeLaTexStyles xeLaTexStyles = new XeLaTexStyles();
                xeLaTexStyles.LangFontDictionary = _langFontCodeandName;
                classInlineStyle = xeLaTexStyles.CreateXeTexStyles(projInfo, xeLatexFile, cssClass);

                XeLaTexContent xeLaTexContent = new XeLaTexContent();
                xeLaTexContent.IsUnix = _isUnixOs;
                Dictionary<string, List<string>> classInlineText = xeLaTexStyles._classInlineText;
                xeLaTexContent.CreateContent(projInfo, cssClass, xeLatexFile, classInlineStyle, cssTree.SpecificityClass, cssTree.CssClassOrder, classInlineText, pageWidth);

                _xelatexDocumentOpenClosedRequired = true; //Don't change the place.
                CloseDocument(xeLatexFile, false, string.Empty);
                ModifyXeLaTexStyles modifyXeLaTexStyles = new ModifyXeLaTexStyles();
                modifyXeLaTexStyles.XelatexDocumentOpenClosedRequired = true;
                modifyXeLaTexStyles.ProjectType = projInfo.ProjectInputType;

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
                    writer.WriteLine(contentWriter.Replace("&", @"\&"));
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
                        Common.RemoveDTDForLinuxProcess(revFile, "xelatex");
                    }
                }

                projInfo.DefaultXhtmlFileWithPath = revFile;
                PreExportProcess preProcessor = new PreExportProcess(projInfo);
                preProcessor.SetLangforLetter(projInfo.DefaultXhtmlFileWithPath);

                CheckFontFamilyAvailable(projInfo.DefaultRevCssFileWithPath);

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
                xeLaTexContent.IsUnix = _isUnixOs;
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
	            bool createPageNumber = false;
	            if (projInfo.DefaultXhtmlFileWithPath.IndexOf("PreserveFlexRev.xhtml") > 0)
		            createPageNumber = true;

                modifyXeLaTexStyles.ModifyStylesXML(projInfo.ProjectPath, xeLatexFile, newProperty, cssClass,
													xeLatexRevesalIndexFile, include, _langFontCodeandName, createPageNumber);
                _xeLaTexPropertyFullFontStyleList = modifyXeLaTexStyles.XeLaTexPropertyFontStyleList;

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

        private static void CheckFontFamilyAvailable(string cssFileName)
        {
            var installedFontList = XelatexFontMapping.InstalledFontList();
            if (!installedFontList.ContainsKey("Scheherazade Graphite Alpha"))
            {
                if (installedFontList.ContainsKey("Scheherazade"))
                {
                    Common.StreamReplaceInFile(cssFileName, "Scheherazade Graphite Alpha", "Scheherazade");
                }
            }
        }

        protected void UpdateXeLaTexFontCacheIfNecessary()
        {
            Debug.Assert(XeLaTexInstallation.GetXeLaTexDir() != "");
            var systemFontList = FontFamily.Families;
            if (systemFontList.Length != XeLaTexInstallation.GetXeLaTexFontCount())
            {
				var xelatexPath = XeLaTexInstallation.GetXeLaTexDir();
	            if (!Directory.Exists(xelatexPath))
		            return;

                using (var p2 = new Process())
                {
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

        public void CallXeLaTex(PublicationInformation projInfo, string xeLatexFullFile, bool openFile, Dictionary<string, string> ImageFilePath)
        {
            string originalDirectory = Directory.GetCurrentDirectory();
            string[] pdfFiles = Directory.GetFiles(Path.GetDirectoryName(xeLatexFullFile), "*.pdf");
            foreach (string pdfFile in pdfFiles)
            {
                try
                {
                    File.Delete(pdfFile);
                }
                catch { }
            }
            string exportDirectory = Path.GetDirectoryName(xeLatexFullFile);
            string xeLaTexInstallationPath = XeLaTexInstallation.GetXeLaTexDir();
            if (!Directory.Exists(xeLaTexInstallationPath))
            {
				var msg = LocalizationManager.GetString ("ExportXelatex.CallXelatex.Message", "Please install the Xelatex application.", "");
				throw new FileNotFoundException (msg);
            }
            string name = "xelatex.exe";
            string arguments = "-interaction=batchmode \"" + Path.GetFileName(xeLatexFullFile) + "\"";
            if (_isUnixOs)
            {
                string path = Environment.GetEnvironmentVariable("PATH");
                Debug.Assert(path != null);
                if (!path.Contains(xeLaTexInstallationPath))
                {
                    Environment.SetEnvironmentVariable("PATH", string.Format("{0}:{1}", xeLaTexInstallationPath, path));
                }
                name = Common.PathCombine(xeLaTexInstallationPath, "xelatex");
                arguments = " -interaction=batchmode \"" + xeLatexFullFile + "\"";

                if (_inputType.ToLower() == "scripture" || projInfo.ProjectInputType == "scripture")
                {
                    paraTextEnvVariable = Environment.GetEnvironmentVariable("TEXINPUTS");
                    Environment.SetEnvironmentVariable("TEXINPUTS", "");
                }
                if (exportDirectory != null)
                    Directory.SetCurrentDirectory(exportDirectory);
            }
            else
            {
                xeLaTexInstallationPath = Common.PathCombine(xeLaTexInstallationPath, "bin");
                xeLaTexInstallationPath = Common.PathCombine(xeLaTexInstallationPath, "win32");

                string dest = Common.PathCombine(xeLaTexInstallationPath, Path.GetFileName(xeLatexFullFile));
	            if (xeLatexFullFile != dest)
	            {
		            File.Copy(xeLatexFullFile, dest, true);

					string[] picList = Directory.GetFiles(Path.GetDirectoryName(xeLatexFullFile), "*.jpg");
					foreach (string picturefile in picList)
					{
						string pictureCopyTo = Common.PathCombine(xeLaTexInstallationPath, Path.GetFileName(picturefile));

						File.Copy(picturefile, pictureCopyTo, true);
					}

	            }

	            if (_copyrightTexCreated)
                {
                    string copyrightDest = Common.PathCombine(xeLaTexInstallationPath, Path.GetFileName(_copyrightTexFileName));
                    if (_copyrightTexFileName != copyrightDest)
                        File.Copy(_copyrightTexFileName, copyrightDest, true);
                }

                if (_reversalIndexTexCreated)
                {
                    string copyrightDest = Common.PathCombine(xeLaTexInstallationPath,
                                                              Path.GetFileName(_reversalIndexTexFileName));
                    if (_reversalIndexTexFileName != copyrightDest)
                        File.Copy(_reversalIndexTexFileName, copyrightDest, true);
                }
                Directory.SetCurrentDirectory(xeLaTexInstallationPath);
            }

            ExecuteXelatexProcess(xeLatexFullFile, name, arguments);
            OpenXelatexOutput(xeLatexFullFile, openFile, originalDirectory, xeLaTexInstallationPath);
        }

        private void ExecuteXelatexProcess(string xeLatexFullFile, string name, string arguments)
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
                }
            }

            if (paraTextEnvVariable != null && paraTextEnvVariable != string.Empty)
            {
                Environment.SetEnvironmentVariable("TEXINPUTS", paraTextEnvVariable);
                paraTextEnvVariable = string.Empty;
            }

        }

        private void OpenXelatexOutput(string xeLatexFullFile, bool openFile, string originalDirectory,
                                       string xeLaTexInstallationPath)
        {
            string pdfFullName = string.Empty;
            string texNameOnly = Path.GetFileNameWithoutExtension(xeLatexFullFile);
            string userFolder = Path.GetDirectoryName(xeLatexFullFile);

            if (_isUnixOs)
            {
                if (userFolder != null)
                    pdfFullName = Common.PathCombine(userFolder, texNameOnly + ".pdf");

                if (File.Exists(pdfFullName))
                {
                    try
                    {
						if (File.Exists(pdfFullName) && !Common.Testing)
                        {
                            Common.InsertCopyrightInPdf(pdfFullName, "XeLaTex", _inputType);
                        }
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                Directory.SetCurrentDirectory(originalDirectory);
                pdfFullName = CopyProcessResult(xeLaTexInstallationPath, texNameOnly, ".pdf", userFolder);
                string logFullName = CopyProcessResult(xeLaTexInstallationPath, texNameOnly, ".log", userFolder);
                Console.WriteLine(logFullName);
                if (openFile && File.Exists(pdfFullName))
                {
                    try
                    {
						if (File.Exists(pdfFullName) && !Common.Testing)
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
                                var msg = LocalizationManager.GetString("ExportXelatex.OpenXelatexOutput.Message", "The output has been saved in {0}.\n Please install the Xelatex application.", "");
                                msg = string.Format(msg, installedLocation);
                                MessageBox.Show(msg);
                            }
                        }
                    }
                }
                try
                {
					if (Common.Testing)
					{
						return;
					}

                    File.Delete(Common.PathCombine(xeLaTexInstallationPath, texNameOnly + ".log"));
                    File.Delete(Common.PathCombine(xeLaTexInstallationPath, texNameOnly + ".pdf"));
                    File.Delete(Common.PathCombine(xeLaTexInstallationPath, texNameOnly + ".aux"));

					string[] deleteList = Directory.GetFiles(xeLaTexInstallationPath, "*.jpg");
					foreach (string deletefile in deleteList)
	                {
						File.Delete(deletefile);
	                }

					deleteList = Directory.GetFiles(xeLaTexInstallationPath, "*.tex");
					foreach (string deletefile in deleteList)
					{
						File.Delete(deletefile);
					}

					deleteList = Directory.GetFiles(xeLaTexInstallationPath, "*.png");
					foreach (string deletefile in deleteList)
					{
						File.Delete(deletefile);
					}
                }
                catch
                {
                }
            }
        }

        private static void WriteConfig(StreamWriter sw2, string content)
        {
            sw2.WriteLine(content);
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

            if (_isUnixOs)
            {
                var unknownMeta = new List<string>();

                foreach (var fontCode in _langFontCodeandName)
                {
                    if (!_langFontDictionary.ContainsKey(fontCode.Key))
                    {
                        unknownMeta.Add(fontCode.Key);
                    }
                }

                foreach (var fontName in unknownMeta)
                {
                    _langFontCodeandName.Remove(fontName);
                }
                var fontMapping = new XelatexFontMapping();

                var installedFontList = XelatexFontMapping.InstalledFontList();
                //Using Generic Font List Mapping the Font set
                foreach (var fontName in _fontLangMap)
                {
                    if (installedFontList.ContainsKey(fontName.Value))
                    {
                        if (_langFontCodeandName.ContainsKey(fontName.Key))
                        {
                            _langFontCodeandName.Remove(fontName.Key);
                            _langFontCodeandName.Add(fontName.Key, fontName.Value);
                        }

                        if (_langFontDictionary.ContainsKey(fontName.Key))
                        {
                            _langFontDictionary.Remove(fontName.Key);
                            _langFontDictionary.Add(fontName.Key, fontName.Value);
                        }
                    }
                }

                if (_langFontDictionary.Count > 0)
                    _langFontDictionary = fontMapping.GetFontList(_langFontDictionary);

                if (_langFontCodeandName.Count > 0)
                    _langFontCodeandName = fontMapping.GetFontList(_langFontCodeandName);

                if (_langFontCodeandName.Count == 0)
                {
                    _langFontCodeandName = _langFontDictionary;
                }
            }
            else
            {
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
                            if (_langFontCodeandName.ContainsKey(name))
                            {
                                _langFontCodeandName.Remove(name);
                                _langFontCodeandName.Add(name, content);
                                break;
                            }
                            _langFontCodeandName.Add(name, content);
                            break;
                        }
                        if (_isUnixOs)
                        {
                            if (!_langFontCodeandName.ContainsKey(name))
                            {
                                _langFontCodeandName.Add(name, content);
                            }
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
        private void FindInputType(XmlTextReader _reader)
        {
            if (_isInputTypeFound)
            {
                return;
            }
            string name = _reader.GetAttribute("class");
            if (name != null)
            {
                if (name.ToLower() == "headword")
                {
                    _inputType = "dictionary";
                    _isInputTypeFound = true;
                }
                else if (name.ToLower() == "scrbookname")
                {
                    _inputType = "scripture";
                    _isInputTypeFound = true;
                }
            }

        }
        #endregion

        #endregion
    }
}
