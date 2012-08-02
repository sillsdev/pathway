// --------------------------------------------------------------------------------------------
// <copyright file="ExportProcess.cs" from='2009' to='2009' company='SIL International'>
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
// Export process used to Export the ODT and Prince PDF output
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using SIL.Tool;
using Test;

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

            PreExportProcess preProcessor = new PreExportProcess(projInfo);
            if (Common.IsUnixOS())
            {
                Common.RemoveDTDForLinuxProcess(projInfo.DefaultXhtmlFileWithPath);
            }
            // preProcessor.GetTempFolderPath();
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
            //_tableOfContent = Param.GetMetadataValue(Param.TableOfContents, organization) ?? ""; // empty string if null / not found
            _coverImage = (Param.GetMetadataValue(Param.CoverPage, organization) == null) ? false : Boolean.Parse(Param.GetMetadataValue(Param.CoverPage, organization));
            _coverPageImagePath = Param.GetMetadataValue(Param.CoverPageFilename, organization);


            _titleInCoverPage = (Param.GetMetadataValue(Param.TitlePage, organization) == null) ? false : Boolean.Parse(Param.GetMetadataValue(Param.TitlePage, organization));


            _copyrightInformation = (Param.GetMetadataValue(Param.CopyrightPage, organization) == null) ? false : Boolean.Parse(Param.GetMetadataValue(Param.CopyrightPage, organization));
            _copyrightInformationPagePath = Param.GetMetadataValue(Param.CopyrightPageFilename, organization);

            _includeBookTitleintheImage = (Param.GetMetadataValue(Param.CoverPageTitle, organization) == null) ? false : Boolean.Parse(Param.GetMetadataValue(Param.CoverPageTitle, organization));

            _tableOfContent = (Param.GetMetadataValue(Param.TableOfContents, organization) == null) ? false : Boolean.Parse(Param.GetMetadataValue(Param.TableOfContents, organization));


            BuildLanguagesList(projInfo.DefaultXhtmlFileWithPath);

            string fileName = Path.GetFileNameWithoutExtension(projInfo.DefaultXhtmlFileWithPath);
            //projInfo.DefaultXhtmlFileWithPath = preProcessor.ProcessedXhtml;
            projInfo.DefaultCssFileWithPath = preProcessor.ProcessedCss;
            projInfo.ProjectPath = Path.GetDirectoryName(preProcessor.ProcessedXhtml);
            projInfo.DefaultXhtmlFileWithPath = preProcessor.PreserveSpace();

            Dictionary<string, Dictionary<string, string>> cssClass = new Dictionary<string, Dictionary<string, string>>();
            CssTree cssTree = new CssTree();
            cssTree.OutputType = Common.OutputType.XELATEX;
            cssClass = cssTree.CreateCssProperty(projInfo.DefaultCssFileWithPath, true);

            string xeLatexFullFile = Path.Combine(projInfo.ProjectPath, fileName + ".tex");
            StreamWriter xeLatexFile = new StreamWriter(xeLatexFullFile);

            Dictionary<string, List<string>> classInlineStyle = new Dictionary<string, List<string>>();
            Dictionary<string, Dictionary<string, string>> xeTexAllClass = new Dictionary<string, Dictionary<string, string>>();
            XeLaTexStyles xeLaTexStyles = new XeLaTexStyles();
            classInlineStyle = xeLaTexStyles.CreateXeTexStyles(projInfo.ProjectPath, xeLatexFile, cssClass);

            XeLaTexContent xeLaTexContent = new XeLaTexContent();
            Dictionary<string, List<string>> classInlineText = xeLaTexStyles._classInlineText;
            xeLaTexContent.TocEndingPage = preProcessor.GetDictionaryLetterCount();
            Dictionary<string, Dictionary<string, string>> newProperty = xeLaTexContent.CreateContent(projInfo, cssClass, xeLatexFile, classInlineStyle, cssTree.SpecificityClass, cssTree.CssClassOrder, classInlineText);

            if (projInfo.IsReversalExist)
            {
                var revFile = Path.Combine(Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath), "FlexRev.xhtml");
                string fileNameXhtml = Path.GetFileNameWithoutExtension(revFile);
                string xeLatexCopyrightFile = fileNameXhtml + ".tex";

                CloseDocument(xeLatexFile, true, xeLatexCopyrightFile);
            }
            else
            {
                CloseDocument(xeLatexFile, false, string.Empty);
            }

            string include = xeLaTexStyles.PageStyle.ToString();
            ModifyXeLaTexStyles modifyXeLaTexStyles = new ModifyXeLaTexStyles();
            modifyXeLaTexStyles.ProjectType = _inputType;
            modifyXeLaTexStyles.TocChecked = _tableOfContent.ToString();

            modifyXeLaTexStyles.CoverImage = _coverImage.ToString();
            modifyXeLaTexStyles.TitleInCoverPage = _titleInCoverPage.ToString();
            modifyXeLaTexStyles.CopyrightInformation = _copyrightInformation.ToString();
            modifyXeLaTexStyles.IncludeBookTitleintheImage = _includeBookTitleintheImage.ToString();
            modifyXeLaTexStyles.CopyrightInformationPagePath = _copyrightInformationPagePath;
            modifyXeLaTexStyles.CoverPageImagePath = _coverPageImagePath;

            if (ExportCopyright(projInfo))
            {
                _copyrightTexCreated = true;
                modifyXeLaTexStyles.CopyrightTexCreated = true;
                modifyXeLaTexStyles.CopyrightTexFilename = Path.GetFileName(_copyrightTexFileName);
            }

            if (ExportReversalIndex(projInfo))
            {
                _reversalIndexTexCreated = true;
                var revFile = Path.Combine(Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath), "FlexRev.xhtml");
                //var revCSSFile = Path.Combine(Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath), "FlexRev.css");
                string fileNameXhtml = Path.GetFileNameWithoutExtension(revFile);
                string xeLatexCopyrightFile = Path.Combine(projInfo.ProjectPath, fileNameXhtml + ".tex");

                modifyXeLaTexStyles.ReversalIndexExist = true;
                modifyXeLaTexStyles.ReversalIndexTexFilename = Path.GetFileName(xeLatexCopyrightFile);
            }

            modifyXeLaTexStyles.XelatexDocumentOpenClosedRequired = false;
            _xelatexDocumentOpenClosedRequired = false;
            modifyXeLaTexStyles.ModifyStylesXML(projInfo.ProjectPath, xeLatexFile, newProperty, cssClass, xeLatexFullFile, include);

            //CallXeTex(Path.GetFileName(xeLatexFullFile));
            Dictionary<string, string> imgPath = new Dictionary<string, string>();
            if (newProperty.ContainsKey("ImagePath"))
            {
                imgPath = newProperty["ImagePath"];
            }
            UpdateXeLaTexFontCacheIfNecessary();
            CallXeLaTex(xeLatexFullFile, true, imgPath);
            return true;
        }

        public bool ExportCopyright(PublicationInformation projInfo)
        {
            if (_copyrightInformation)
            {
                string copyRightFilePath = Param.GetMetadataValue(Param.CopyrightPageFilename);

                // **    string fileName = Path.GetFileNameWithoutExtension(projInfo.DefaultXhtmlFileWithPath);
                if (copyRightFilePath.Trim().Length <= 0 && !File.Exists(copyRightFilePath))
                {
                    return false;
                }
                projInfo.DefaultXhtmlFileWithPath = copyRightFilePath;
                string filepath = Path.GetFullPath(copyRightFilePath);

                Dictionary<string, Dictionary<string, string>> cssClass =
                    new Dictionary<string, Dictionary<string, string>>();
                CssTree cssTree = new CssTree();
                cssTree.OutputType = Common.OutputType.XELATEX;
                cssClass = cssTree.CreateCssProperty(Path.Combine(filepath, "copy.css"), true);
                string fileNameXhtml = Path.GetFileNameWithoutExtension(copyRightFilePath);
                string xeLatexCopyrightFile = Path.Combine(projInfo.ProjectPath, fileNameXhtml + ".tex");
                _copyrightTexFileName = xeLatexCopyrightFile;


                StreamWriter xeLatexFile = new StreamWriter(xeLatexCopyrightFile);
                Dictionary<string, List<string>> classInlineStyle = new Dictionary<string, List<string>>();
                Dictionary<string, Dictionary<string, string>> xeTexAllClass =
                    new Dictionary<string, Dictionary<string, string>>();
                XeLaTexStyles xeLaTexStyles = new XeLaTexStyles();
                classInlineStyle = xeLaTexStyles.CreateXeTexStyles(projInfo.ProjectPath, xeLatexFile, cssClass);

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
                                                                                                          classInlineText);

                _xelatexDocumentOpenClosedRequired = true; //Don't change the place.
                CloseDocument(xeLatexFile, false, string.Empty);
                string include = xeLaTexStyles.PageStyle.ToString();
                ModifyXeLaTexStyles modifyXeLaTexStyles = new ModifyXeLaTexStyles();
                modifyXeLaTexStyles.XelatexDocumentOpenClosedRequired = true;
                modifyXeLaTexStyles.ModifyStylesXML(projInfo.ProjectPath, xeLatexFile, newProperty, cssClass,
                                                    xeLatexCopyrightFile, include);

                return true;
            }
            return false;
        }

        public bool ExportReversalIndex(PublicationInformation projInfo)
        {

            if (projInfo.IsReversalExist)
            {
                var revFile = Path.Combine(projInfo.ProjectPath, "FlexRev.xhtml");
                if (!File.Exists(revFile))
                {
                    return false;
                }

                projInfo.DefaultXhtmlFileWithPath = revFile;
                Dictionary<string, Dictionary<string, string>> cssClass = new Dictionary<string, Dictionary<string, string>>();
                CssTree cssTree = new CssTree();
                cssTree.OutputType = Common.OutputType.XELATEX;
                cssClass = cssTree.CreateCssProperty(projInfo.DefaultCssFileWithPath, true);
                string fileNameXhtml = Path.GetFileNameWithoutExtension(revFile);
                string xeLatexRevesalIndexFile = Path.Combine(projInfo.ProjectPath, fileNameXhtml + ".tex");
                _reversalIndexTexFileName = xeLatexRevesalIndexFile;
                StreamWriter xeLatexFile = new StreamWriter(xeLatexRevesalIndexFile);
                Dictionary<string, List<string>> classInlineStyle = new Dictionary<string, List<string>>();
                Dictionary<string, Dictionary<string, string>> xeTexAllClass = new Dictionary<string, Dictionary<string, string>>();
                XeLaTexStyles xeLaTexStyles = new XeLaTexStyles();
                classInlineStyle = xeLaTexStyles.CreateXeTexStyles(projInfo.ProjectPath, xeLatexFile, cssClass);

                XeLaTexContent xeLaTexContent = new XeLaTexContent();
                Dictionary<string, List<string>> classInlineText = xeLaTexStyles._classInlineText;
                Dictionary<string, Dictionary<string, string>> newProperty = xeLaTexContent.CreateContent(projInfo, cssClass, xeLatexFile, classInlineStyle, cssTree.SpecificityClass, cssTree.CssClassOrder, classInlineText);

                _xelatexDocumentOpenClosedRequired = true;          //Don't change the place.
                CloseDocument(xeLatexFile, false, string.Empty);
                string include = xeLaTexStyles.PageStyle.ToString();
                ModifyXeLaTexStyles modifyXeLaTexStyles = new ModifyXeLaTexStyles();
                modifyXeLaTexStyles.XelatexDocumentOpenClosedRequired = true;
                modifyXeLaTexStyles.ModifyStylesXML(projInfo.ProjectPath, xeLatexFile, newProperty, cssClass, xeLatexRevesalIndexFile, include);
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
                        xelatexPath = Path.Combine(xelatexPath, "bin");
                        xelatexPath = Path.Combine(xelatexPath, "win32");
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

            string xeLaTexInstallationPath = XeLaTexInstallation.GetXeLaTexDir();
            string name = "xelatex.exe";
            string arguments = "-interaction=batchmode \"" + Path.GetFileName(xeLatexFullFile) + "\"";
            if (Common.IsUnixOS())
            {
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
                    //p1Output = p1.StandardOutput.ReadToEnd();
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
                    //p1Output = p1.StandardOutput.ReadToEnd();
                    p1Error = p1.StandardError.ReadToEnd();
                }

            }
            string pdfFullName = string.Empty;
            string texNameOnly = Path.GetFileNameWithoutExtension(xeLatexFullFile);
            string userFolder = Path.GetDirectoryName(xeLatexFullFile);

            if (Common.IsUnixOS())
            {
                if (userFolder != null) 
                    pdfFullName = Path.Combine(userFolder, texNameOnly + ".pdf");

                if (File.Exists(pdfFullName))
                {
                    try
                    {
                        Common.OpenOutput(pdfFullName);
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
                        Common.OpenOutput(pdfFullName);
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
            string tmpLogFullName = Path.Combine(instPath, logName);
            string logFullName = Path.Combine(userFolder, logName);
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
                ReversalIndexContent += "\\thispagestyle{empty} \r\n";
                ReversalIndexContent += "\\newpage \r\n";
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

        #endregion

        #endregion
    }
}
