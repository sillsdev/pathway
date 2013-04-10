// --------------------------------------------------------------------------------------
// <copyright file="Exportepub.cs" from='2009' to='2010' company='SIL International'>
//      Copyright © 2010, SIL International. All Rights Reserved.
//
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
// <author>Erik Brommers</author>
// <email>erik_brommers@sil.org</email>
// Last reviewed:
// 
// <remarks>
// epub export
//
// .epub files are zipped archives with the following file structure:
// |-mimetype
// |-META-INF
// | `-container.xml
// |-OEBPS
//   |-content.opf
//   |-toc.ncx
//   |-<any fonts and other files embedded into the archive>
//   |-<list of files in book – xhtml format + .css for styling>
//   '-<any images referenced in book files>
//
// See also http://www.openebook.org/2007/ops/OPS_2.0_final_spec.html
// </remarks>
// --------------------------------------------------------------------------------------

// uncomment this to write out performance timings
//#define TIME_IT 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using epubConvert;
using epubConvert.Properties;
using epubValidator;
using SIL.Tool;


namespace SIL.PublishingSolution
{
    /// <summary>
    /// Possible values for the MissingFont and NonSilFont properties.
    /// </summary>
    public enum FontHandling
    {
        EmbedFont,
        SubstituteDefaultFont,
        PromptUser,
        CancelExport
    }

    public class Exportepub : IExportProcess
    {

        protected string processFolder;
        protected string restructuredFullName;
        protected string outputPathBase;
        protected string outputNameBase;
        protected static ProgressBar _pb;
        private Dictionary<string, EmbeddedFont> _embeddedFonts;  // font information for this export
        private Dictionary<string, string> _langFontDictionary; // languages and font names in use for this export
        private readonly XslCompiledTransform _fixPlayOrder = new XslCompiledTransform();
        private readonly XslCompiledTransform _addRevId = new XslCompiledTransform();
        private readonly XslCompiledTransform _noXmlSpace = new XslCompiledTransform();
        private readonly XslCompiledTransform _addDicTocHeads = new XslCompiledTransform();
        private readonly XslCompiledTransform _fixEpub = new XslCompiledTransform();
        private readonly XslCompiledTransform _fixEpubToc = new XslCompiledTransform();

        private string currentChapterNumber = string.Empty;
        private bool isIncludeImage = true;
        private bool isNoteTargetReferenceExists = false;
        private bool _isUnixOS = false;
        private bool _isPictureDisplayNone = false;

        //        protected static PostscriptLanguage _postscriptLanguage = new PostscriptLanguage();
        protected string _inputType = "dictionary";

        public const string ReferencesFilename = "zzReferences.xhtml";


        // property implementations
        public string Title { get; set; }
        public string Creator { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public string Relation { get; set; }
        public string Coverage { get; set; }
        public string Rights { get; set; }
        public string Format { get; set; }
        public string Source { get; set; }
        public bool EmbedFonts { get; set; }
        public bool IncludeFontVariants { get; set; }
        public string TocLevel { get; set; }
        public int MaxImageWidth { get; set; }

        public int BaseFontSize { get; set; }
        public int DefaultLineHeight { get; set; }
        /// <summary>
        /// Fallback font (if the embedded font is missing or non-SIL)
        /// </summary>
        public string DefaultFont { get; set; }
        public string DefaultAlignment { get; set; }
        public string ChapterNumbers { get; set; }
        public string References { get; set; }
        public FontHandling MissingFont { get; set; } // note that this doesn't use all the enum values
        public FontHandling NonSilFont { get; set; }

        // interface methods
        public string ExportType
        {
            get
            {
                return "E-Book (.epub)";
            }
        }

        /// <summary>
        /// Returns what input data types this export process handles. The epub exporter
        /// currently handles scripture and dictionary data types.
        /// </summary>
        /// <param name="inputDataType">input data type to test</param>
        /// <returns>true if this export process handles the specified data type</returns>
        public bool Handle(string inputDataType)
        {
            var returnValue = inputDataType.ToLower() == "dictionary" || inputDataType.ToLower() == "scripture";
            return returnValue;
        }

        /// <summary>
        /// Entry point for epub converter
        /// </summary>
        /// <param name="projInfo">values passed including xhtml and css names</param>
        /// <returns>true if succeeds</returns>
        public bool Export(PublicationInformation projInfo)
        {
            //projInfo.IsReversalExist = true;
            var fixPlayorderStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("epubConvert.fixPlayorder.xsl");
            Debug.Assert(fixPlayorderStream != null);
            _fixPlayOrder.Load(XmlReader.Create(fixPlayorderStream));
            var addRevIdStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("epubConvert.addRevId.xsl");
            Debug.Assert(addRevIdStream != null);
            _addRevId.Load(XmlReader.Create(addRevIdStream));
            var noXmlSpaceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("epubConvert.noXmlSpace.xsl");
            Debug.Assert(noXmlSpaceStream != null);
            _noXmlSpace.Load(XmlReader.Create(noXmlSpaceStream));
            _addDicTocHeads.Load(XmlReader.Create(UsersXsl("addDicTocHeads.xsl")));

            _fixEpub.Load(XmlReader.Create(UsersXsl("FixEpub.xsl")));

            _fixEpubToc.Load(XmlReader.Create(UsersXsl("FixEpubToc.xsl")));


            _isUnixOS = Common.UnixVersionCheck();
            var myCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            var curdir = Environment.CurrentDirectory;
            bool success = true;
            if (projInfo == null)
            {
                // missing some vital information - error out
                success = false;
                Cursor.Current = myCursor;
            }
            else
            {

#if (TIME_IT)
                DateTime dt1 = DateTime.Now;    // time this thing
#endif
                // basic setup
                PreExportProcess preProcessor = new PreExportProcess(projInfo);
                if (_isUnixOS)
                {
                    Common.RemoveDTDForLinuxProcess(projInfo.DefaultXhtmlFileWithPath);
                }

                isIncludeImage = GetIncludeImageStatus(projInfo.SelectedTemplateStyle);
                isNoteTargetReferenceExists = Common.NodeExists(projInfo.DefaultXhtmlFileWithPath, "");

                //if (projInfo.ProjectInputType.ToLower() == "dictionary")
                InsertBeforeAfterInXHTML(projInfo);

                _langFontDictionary = new Dictionary<string, string>();
                _embeddedFonts = new Dictionary<string, EmbeddedFont>();

                var inProcess = new InProcess(0, 9); // create a progress bar with 7 steps (we'll add more below)
                inProcess.Text = Resources.Exportepub_Export_Exporting__epub_file;
                inProcess.Show();
                inProcess.PerformStep();
                inProcess.ShowStatus = true;
                inProcess.SetStatus("Preprocessing content");
                var sb = new StringBuilder();
                Guid bookId = Guid.NewGuid(); // NOTE: this creates a new ID each time Pathway is run. 
                string outputFolder = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath); // finished .epub goes here

                Environment.CurrentDirectory = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath);
                Common.SetProgressBarValue(projInfo.ProgressBar, projInfo.DefaultXhtmlFileWithPath);
                inProcess.PerformStep();
                //_postscriptLanguage.SaveCache();
                // XHTML preprocessing
                Common.StreamReplaceInFile(preProcessor.ProcessedXhtml, "&nbsp;", Common.NonBreakingSpace);
                preProcessor.GetTempFolderPath();
                preProcessor.ImagePreprocess();
                preProcessor.ReplaceSlashToREVERSE_SOLIDUS();
                if (projInfo.SwapHeadword)
                {
                    preProcessor.SwapHeadWordAndReversalForm();
                }

                BuildLanguagesList(projInfo.DefaultXhtmlFileWithPath);
                var langArray = new string[_langFontDictionary.Keys.Count];
                _langFontDictionary.Keys.CopyTo(langArray, 0);

                // CSS preprocessing
                inProcess.SetStatus("Preprocessing stylesheet");
                string tempFolder = Path.GetDirectoryName(preProcessor.ProcessedXhtml);
                string tempFolderName = Path.GetFileName(tempFolder);
                string cssFolder = Path.GetDirectoryName(projInfo.DefaultCssFileWithPath);


                string cssFullPath = Common.PathCombine(cssFolder, "epub.css");
                if (!File.Exists(cssFullPath))
                {
                    cssFullPath = projInfo.DefaultCssFileWithPath;
                }

                var mc = new MergeCss { OutputLocation = tempFolderName };
                string mergedCSS = mc.Make(cssFullPath, "book.css");
                preProcessor.ReplaceStringInCss(mergedCSS);
                preProcessor.SetDropCapInCSS(mergedCSS);
                preProcessor.InsertCoverPageImageStyleInCSS(mergedCSS);
                preProcessor.InsertSectionHeadID();
                string defaultCSS = Path.GetFileName(mergedCSS);
                // rename the CSS file to something readable
                string niceNameCSS = Path.Combine(tempFolder, "book.css");
                projInfo.DefaultCssFileWithPath = niceNameCSS;
                if (niceNameCSS != mergedCSS)
                {
                    if (File.Exists(niceNameCSS))
                    {
                        File.Delete(niceNameCSS);
                    }
                    File.Copy(mergedCSS, niceNameCSS);
                    mergedCSS = niceNameCSS;
                }
                defaultCSS = Path.GetFileName(niceNameCSS);
                Common.SetDefaultCSS(projInfo.DefaultXhtmlFileWithPath, defaultCSS);
                Common.SetDefaultCSS(preProcessor.ProcessedXhtml, defaultCSS);

                // pull in the style settings
                LoadPropertiesFromSettings();

                // Make a local copy of the epub xslt. 
                string xsltFullName = GetXsltFile();

                // EDB 10/22/2010
                // HACK: we need the preprocessed image file names (preprocessor.imageprocess()), but
                // it's missing the xml namespace that makes it a valid xhtml file. We'll add it here.
                // (The unprocessed html works fine, but doesn't have the updated links to the image files in it, 
                // so we can't use it.)
                // TODO: remove this line when TE provides valid XHTML output.

                if (langArray.Length > 0)
                {
                    //Common.StreamReplaceInFile(preProcessor.ProcessedXhtml, "<html",
                    //                           string.Format(
                    //                               "<html xmlns='http://www.w3.org/1999/xhtml' xml:lang='{0}' dir='{1}'",
                    //                               langArray[0], Common.GetTextDirection(langArray[0])));
                    Common.StreamReplaceInFile(preProcessor.ProcessedXhtml, "<html>",
                                               string.Format(
                                                   "<html xmlns='http://www.w3.org/1999/xhtml' xml:lang='{0}' dir='{1}'>",
                                                   langArray[0], Common.GetTextDirection(langArray[0])));
                    // The TE export outputs both xml:lang and lang parameters
                    if (projInfo.ProjectInputType.ToLower() == "scripture")
                        Common.StreamReplaceInFile(preProcessor.ProcessedXhtml, "xml:lang=\"utf-8\" lang=\"utf-8\"", "xml:lang=\"utf-8\"");
                    Common.StreamReplaceInFile(preProcessor.ProcessedXhtml, " lang=\"", " xml:lang=\"");
                }

                ApplyXslt(preProcessor.ProcessedXhtml, _noXmlSpace);
                ApplyXslt(preProcessor.ProcessedXhtml, _fixEpub);
                // end EDB 10/22/2010
                inProcess.PerformStep();

                preProcessor.PrepareBookNameAndChapterCount();

                // split the .XHTML into multiple files, as specified by the user
                List<string> htmlFiles = new List<string>();
                List<string> splitFiles = new List<string>();
                Common.XsltProgressBar = inProcess.Bar();

                // insert the front matter items as separate files in the output folder
                inProcess.SetStatus("Adding content");
                preProcessor.SkipChapterInformation = TocLevel;
                splitFiles.AddRange(preProcessor.InsertFrontMatter(tempFolder, false));
                foreach (var file in splitFiles)
                {
                    Common.SetDefaultCSS(file, defaultCSS);
                }

                if ((_inputType.ToLower().Equals("dictionary") && projInfo.IsLexiconSectionExist) ||
                    (_inputType.ToLower().Equals("scripture")))
                {
                    if (projInfo.FileToProduce.ToLower() != "one")
                    {
                        splitFiles.AddRange(SplitFile(preProcessor.ProcessedXhtml, projInfo));
                    }
                    else
                    {
                        splitFiles.Add(preProcessor.ProcessedXhtml);
                    }
                }

                foreach (string file in splitFiles)
                {
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                    if (fileNameWithoutExtension != null && fileNameWithoutExtension.IndexOf(@"PartFile") == 0)
                    {
                        RemoveNodeInXhtmlFile(file);
                    }
                }

                // If we are working with a dictionary and have a reversal index, process it now)
                if (projInfo.IsReversalExist)
                {
                    var revFile = Path.Combine(Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath), "FlexRev.xhtml");
                    // EDB 10/20/2010 - TD-1629 - remove when merged CSS passes validation
                    // (note that the rev file uses a "FlexRev.css", not "main.css"

                    if (_isUnixOS)
                    {
                        Common.RemoveDTDForLinuxProcess(revFile);
                    }

                    Common.SetDefaultCSS(revFile, defaultCSS);
                    // EDB 10/29/2010 FWR-2697 - remove when fixed in FLEx
                    Common.StreamReplaceInFile(revFile, "<ReversalIndexEntry_Self", "<span class='ReversalIndexEntry_Self'");
                    Common.StreamReplaceInFile(revFile, "</ReversalIndexEntry_Self", "</span");
                    if (langArray.Length > 0)
                    {
                        Common.StreamReplaceInFile(revFile,
                                                   "<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"utf-8\" lang=\"utf-8\"",
                                                   string.Format(
                                                       "<html  xmlns='http://www.w3.org/1999/xhtml' xml:lang='{0}' dir='{1}'",
                                                       langArray[0], Common.GetTextDirection(langArray[0])));
                        Common.StreamReplaceInFile(revFile,
                                                   "<html>",
                                                   string.Format(
                                                       "<html  xmlns='http://www.w3.org/1999/xhtml' xml:lang='{0}' dir='{1}'>",
                                                       langArray[0], Common.GetTextDirection(langArray[0])));
                        Common.StreamReplaceInFile(revFile, " lang=\"", " xml:lang=\"");
                    }
                    //MessageBox.Show("applyxslt 1 ");
                    ApplyXslt(revFile, _addRevId);      // also removes xml:space="preserve" attributes
                    // now split out the html as needed
                    List<string> fileNameWithPath = new List<string>();
                    fileNameWithPath = Common.SplitXhtmlFile(revFile, "letHead", "RevIndex", true);
                    splitFiles.AddRange(fileNameWithPath);
                }
                // add the total file count (so far) to the progress bar, so it's a little more accurate
                //inProcess.AddToMaximum(splitFiles.Count);
                inProcess.SetStatus("Processing stylesheet information");
                // get rid of styles that don't work with .epub
                //RemovePagedStylesFromCss(niceNameCSS);
                // customize the CSS file based on the settings
                CustomizeCSS(mergedCSS);

                string getPsApplicationPath = Common.GetPSApplicationPath();
                string xsltProcessExe = Path.Combine(getPsApplicationPath, "XslProcess.exe");
                inProcess.SetStatus("Apply Xslt Process in html file");
                if (File.Exists(xsltProcessExe))
                {
                    foreach (string file in splitFiles)
                    {
                        if (_isUnixOS)
                        {
                            if (file.Contains("File2Cpy"))
                            {
                                string copyRightpage = file.Replace(".xhtml", "_.xhtml");
                                File.Copy(file, copyRightpage);
                                htmlFiles.Add(Path.Combine(Path.GetDirectoryName(file), (Path.GetFileNameWithoutExtension(file) + "_.xhtml")));
                                File.Delete(file);
                            }
                            else
                            {
                                Common.XsltProcess(file, xsltFullName, "_.xhtml");
                                htmlFiles.Add(Path.Combine(Path.GetDirectoryName(file),
                                                                                (Path.GetFileNameWithoutExtension(file) + "_.xhtml")));
                                File.Delete(file);
                            }
                        }
                        else
                        {

                            if (File.Exists(file))
                            {
                                Common.RunCommand(xsltProcessExe,
                                                  string.Format("{0} {1} {2} {3}", file, xsltFullName, "_.xhtml",
                                                                getPsApplicationPath), 1);

                                //Common.XsltProcess(file, xsltFullName, "_.xhtml");
                                // add this file to the html files list
                                htmlFiles.Add(Path.Combine(Path.GetDirectoryName(file),
                                                           (Path.GetFileNameWithoutExtension(file) + "_.xhtml")));
                                // clean up the un-transformed file
                                File.Delete(file);
                            }
                        }
                    }
                }
                inProcess.PerformStep();
                // create the "epub" directory structure and copy over the boilerplate files
                inProcess.SetStatus("Creating .epub structure");
                sb.Append(tempFolder);
                sb.Append(Path.DirectorySeparatorChar);
                sb.Append("epub");
                string strFromOfficeFolder = Common.PathCombine(Common.GetPSApplicationPath(), "epub");
                projInfo.TempOutputFolder = sb.ToString();
                CopyFolder(strFromOfficeFolder, projInfo.TempOutputFolder);
                // set the folder where our epub content goes
                sb.Append(Path.DirectorySeparatorChar);
                sb.Append("OEBPS");
                string contentFolder = sb.ToString();
                if (!Directory.Exists(contentFolder))
                {
                    Directory.CreateDirectory(contentFolder);
                }
                inProcess.PerformStep();

                // extract references file if specified
                if (References.Contains("End") && _inputType.ToLower().Equals("scripture"))
                {
                    inProcess.SetStatus("Creating endnote references file");
                    CreateReferencesFile(contentFolder, preProcessor.ProcessedXhtml);
                    splitFiles.Add(Path.Combine(contentFolder, ReferencesFilename));
                }

                // -- Font handling --
                // First, get the list of fonts used in this project
                inProcess.SetStatus("Processing fonts");
                BuildFontsList();
                // Embed fonts if needed
                if (EmbedFonts)
                {
                    if (!EmbedAllFonts(langArray, contentFolder))
                    {
                        // user cancelled the epub conversion - clean up and exit
                        Environment.CurrentDirectory = curdir;
                        Cursor.Current = myCursor;
                        inProcess.Close();
                        return false;
                    }
                }
                // update the CSS file to reference any fonts used by the writing systems
                // (if they aren't embedded in the .epub, we'll still link to them here)
                ReferenceFonts(mergedCSS);
                inProcess.PerformStep();

                // copy over the XHTML and CSS files
                string cssPath = Common.PathCombine(contentFolder, defaultCSS);
                File.Copy(mergedCSS, cssPath);
                string tocFiletoUpdate = string.Empty;
                // copy the xhtml files into the content directory
                foreach (string file in htmlFiles)
                {
                    string name = Path.GetFileNameWithoutExtension(file).Substring(0, 8);
                    string substring = Path.GetFileNameWithoutExtension(file).Substring(8);
                    string dest = Common.PathCombine(contentFolder, name + substring.PadLeft(6, '0') + ".xhtml");

                    if (_isUnixOS)
                    {
                        Common.RemoveDTDForLinuxProcess(file);
                    }

                    File.Move(file, dest);
                    // split the file into smaller pieces if needed
                    List<string> files = SplitBook(dest);

                    if (dest.Contains("File3TOC"))
                    {
                        tocFiletoUpdate = dest;
                    }

                    if (files.Count > 1)
                    {
                        Common.StreamReplaceInFile(tocFiletoUpdate, Path.GetFileName(dest), Path.GetFileName(files[0]));
                        try
                        {
                            File.Delete(tocFiletoUpdate + ".tmp");
                        }
                        catch { }
                        // file was split out - delete "dest" (it's been replaced)
                        File.Delete(dest);
                    }
                }
                // fix any relative hyperlinks that might have broken when we split the .xhtml up
#if (TIME_IT)
                DateTime dtRefStart = DateTime.Now;
#endif
                InsertChapterLinkBelowBookName(contentFolder);

                inProcess.SetStatus("Processing hyperlinks");
                if (_inputType == "scripture" && References.Contains("End"))
                {
                    UpdateReferenceHyperlinks(contentFolder, inProcess);
                    UpdateReferenceSourcelinks(contentFolder, inProcess);
                }
                FixRelativeHyperlinks(contentFolder, inProcess);

#if (TIME_IT)
                TimeSpan tsRefTotal = DateTime.Now - dtRefStart;
                Debug.WriteLine("Exportepub: time spent fixing reference hyperlinks: " + tsRefTotal);
#endif
                inProcess.PerformStep();

                // process and copy over the image files
                inProcess.SetStatus("Processing images");
                ProcessImages(tempFolder, contentFolder);
                inProcess.PerformStep();

                // generate the toc / manifest files
                inProcess.SetStatus("Generating .epub TOC and manifest");
                CreateOpf(projInfo, contentFolder, bookId);
                CreateNcx(projInfo, contentFolder, bookId);
                inProcess.PerformStep();

                // Done adding content - now zip the whole thing up and name it
                inProcess.SetStatus("Cleaning up");
                string fileName = Path.GetFileNameWithoutExtension(projInfo.DefaultXhtmlFileWithPath);
                Compress(projInfo.TempOutputFolder, Common.PathCombine(outputFolder, fileName));
#if (TIME_IT)
                TimeSpan tsTotal = DateTime.Now - dt1;
                Debug.WriteLine("Exportepub: time spent in .epub conversion: " + tsTotal);
#endif
                inProcess.PerformStep();
                inProcess.Close();

                // clean up
                var outputPathWithFileName = Common.PathCombine(outputFolder, fileName) + ".epub";
                Common.CleanupOutputDirectory(outputFolder, outputPathWithFileName);
                Environment.CurrentDirectory = curdir;
                Cursor.Current = myCursor;


                // Postscript - validate the file using our epubcheck wrapper
                if (Common.Testing)
                {
                    // Running the unit test - just run the validator and return the result
                    var validationResults = epubValidator.Program.ValidateFile(outputPathWithFileName);
                    Debug.WriteLine("Exportepub: validation results: " + validationResults);
                    // we've succeeded if epubcheck returns no errors
                    //success = (validationResults.Contains("No errors or warnings detected"));
                }
                else
                {
                    //MessageBox.Show(Resources.ExportCallingEpubValidator, Resources.ExportComplete,
                    //                MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (MessageBox.Show(Resources.ExportCallingEpubValidator + "\r\nDo you want to Validate ePub file", Resources.ExportComplete, MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Information) ==
                        DialogResult.Yes)
                    {

                        var validationDialog = new ValidationDialog();
                        validationDialog.FileName = outputPathWithFileName;
                        validationDialog.ShowDialog();
                    }

                    if (File.Exists(outputPathWithFileName))
                    {
                        if (_isUnixOS)
                        {
                            string epubFileName = fileName.Replace(" ", "") + ".epub";
                            string replaceEmptyCharacterinFileName = Path.GetDirectoryName(outputPathWithFileName);
                            replaceEmptyCharacterinFileName = Path.Combine(outputFolder, epubFileName);
                            if (outputPathWithFileName != replaceEmptyCharacterinFileName && File.Exists(outputPathWithFileName))
                            {
                                File.Copy(outputPathWithFileName, replaceEmptyCharacterinFileName, true);
                            }

                            SubProcess.Run(outputFolder, "ebook-viewer", epubFileName, false);
                        }
                        else
                        {
                            Process.Start(outputPathWithFileName);
                        }
                    }
                }
            }
            return success;
        }

        private bool GetIncludeImageStatus(string cssFileName)
        {
            try
            {
                if (cssFileName.Trim().Length == 0) { return true; }
                Param.LoadSettings();
                XmlDocument xDoc = Common.DeclareXMLDocument(false);
                string path = Param.SettingOutputPath;
                xDoc.Load(path);
                string xPath = "//stylePick/styles/others/style[@name='" + cssFileName + "']/styleProperty[@name='IncludeImage']/@value";
                //string xPath = "//stylePick/styles/others/style[@name='" + "Copy of EBook (epub)" + "']/styleProperty[@name='IncludeImage']/@value";
                XmlNode includeImageNode = xDoc.SelectSingleNode(xPath);
                if (includeImageNode != null && includeImageNode.InnerText == "No")
                    isIncludeImage = false;
            }
            catch { }
            return isIncludeImage;
        }

        private void RemoveNodeInXhtmlFile(string fileName)
        {
            //Removed NoteTargetReference tag from XHTML file
            XmlDocument xDoc = Common.DeclareXMLDocument(false);
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xDoc.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            xDoc.Load(fileName);
            XmlElement elmRoot = xDoc.DocumentElement;
            //string xPath = "//xhtml:span[@class='Note_Target_Reference']";
            //if (elmRoot != null)
            //{
            //    XmlNodeList referenceNode = elmRoot.SelectNodes(xPath, namespaceManager);
            //    if (referenceNode != null && referenceNode.Count > 0)
            //    {
            //        for (int i = 0; i < referenceNode.Count; i++)
            //        {
            //            //referenceNode[i].RemoveChild(referenceNode[i].FirstChild);
            //            var parentNode = referenceNode[i].ParentNode;
            //            if (parentNode != null)
            //                parentNode.RemoveChild(referenceNode[i]);
            //        }
            //    }
            //}

            //If includeImage is false, removes the img -> parent tag
            if (isIncludeImage == false)
            {
                string[] pictureClass = { "pictureCaption", "pictureColumn", "picturePage" };
                foreach (string clsName in pictureClass)
                {
                    string xPath = "//xhtml:div[@class='" + clsName + "']";
                    if (elmRoot != null)
                    {
                        XmlNodeList pictCaptionNode = elmRoot.SelectNodes(xPath, namespaceManager);
                        if (pictCaptionNode != null && pictCaptionNode.Count > 0)
                        {
                            for (int i = 0; i < pictCaptionNode.Count; i++)
                            {
                                //referenceNode[i].RemoveChild(referenceNode[i].FirstChild);
                                var parentNode = pictCaptionNode[i].ParentNode;
                                if (parentNode != null)
                                    parentNode.RemoveChild(pictCaptionNode[i]);
                            }
                        }
                    }
                }

                if (elmRoot != null && isIncludeImage == false)
                {
                    XmlNodeList imgNodes = elmRoot.GetElementsByTagName("img");
                    if (imgNodes.Count > 0)
                    {
                        for (int i = 0; i < imgNodes.Count; i++)
                        {
                            //referenceNode[i].RemoveChild(referenceNode[i].FirstChild);
                            var parentNode = imgNodes[i].ParentNode;
                            if (parentNode != null)
                                parentNode.RemoveChild(imgNodes[i]);
                        }
                    }
                }
            }
            xDoc.Save(fileName);
        }

        private string UsersXsl(string xslName)
        {
            var myPath = Path.Combine(Common.GetAllUserPath(), xslName);
            if (File.Exists(myPath))
                return myPath;
            return Common.FromRegistry(xslName);
        }

        #region Private Functions
        #region Handle After Before
        /// <summary>
        /// Inserting After & Before content to XHTML file
        /// </summary>
        private void InsertBeforeAfterInXHTML(PublicationInformation projInfo)
        {
            if (projInfo == null) return;
            if (projInfo.DefaultXhtmlFileWithPath == null || projInfo.DefaultCssFileWithPath == null) return;
            if (projInfo.DefaultXhtmlFileWithPath.Trim().Length == 0 || projInfo.DefaultCssFileWithPath.Trim().Length == 0) return;

            Dictionary<string, Dictionary<string, string>> cssClass = new Dictionary<string, Dictionary<string, string>>();
            CssTree cssTree = new CssTree();
            cssClass = cssTree.CreateCssProperty(projInfo.DefaultCssFileWithPath, true);

            AfterBeforeProcessEpub afterBeforeProcess = new AfterBeforeProcessEpub();
            afterBeforeProcess.RemoveAfterBefore(projInfo, cssClass, cssTree.SpecificityClass, cssTree.CssClassOrder);

            if (projInfo.IsReversalExist && projInfo.ProjectInputType.ToLower() == "dictionary")
            {
                cssClass = cssTree.CreateCssProperty(projInfo.DefaultRevCssFileWithPath, true);
                string originalDefaultXhtmlFileName = projInfo.DefaultXhtmlFileWithPath;
                projInfo.DefaultXhtmlFileWithPath = Path.Combine(Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath), "FlexRev.xhtml");
                AfterBeforeProcessEpub afterBeforeProcessReversal = new AfterBeforeProcessEpub();
                afterBeforeProcessReversal.RemoveAfterBefore(projInfo, cssClass, cssTree.SpecificityClass, cssTree.CssClassOrder);
                Common.StreamReplaceInFile(projInfo.DefaultXhtmlFileWithPath, "&nbsp;", Common.NonBreakingSpace);
                projInfo.DefaultXhtmlFileWithPath = originalDefaultXhtmlFileName;
            }
        }
        #endregion

        #region Property persistence
        /// <summary>
        /// Loads the settings file and pulls out the values we look at.
        /// </summary>
        private void LoadPropertiesFromSettings()
        {
            // Load User Interface Collection Parameters
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
            string layout = Param.GetItem("//settings/property[@name='LayoutSelected']/@value").Value;
            Dictionary<string, string> othersfeature = Param.GetItemsAsDictionary("//stylePick/styles/others/style[@name='" + layout + "']/styleProperty");
            // Title (book title in Configuration Tool UI / dc:title in metadata)
            Title = Param.GetMetadataValue(Param.Title, organization) ?? ""; // empty string if null / not found
            // Creator (dc:creator))
            Creator = Param.GetMetadataValue(Param.Creator, organization) ?? ""; // empty string if null / not found
            // information
            Description = Param.GetMetadataValue(Param.Description, organization) ?? ""; // empty string if null / not found
            // Source
            Source = Param.GetMetadataValue(Param.Source, organization) ?? ""; // empty string if null / not found
            // Format
            Format = Param.GetMetadataValue(Param.Format, organization) ?? ""; // empty string if null / not found
            // Publisher
            Publisher = Param.GetMetadataValue(Param.Publisher, organization) ?? ""; // empty string if null / not found
            // Coverage
            Coverage = Param.GetMetadataValue(Param.Coverage, organization) ?? ""; // empty string if null / not found
            // Rights (dc:rights)
            Rights = Param.GetMetadataValue(Param.CopyrightHolder, organization) ?? ""; // empty string if null / not found
            // embed fonts
            if (othersfeature.ContainsKey("EmbedFonts"))
            {
                EmbedFonts = (othersfeature["EmbedFonts"].Trim().Equals("Yes")) ? true : false;
            }
            else
            {
                // default - we're more concerned about accurate font rendering than size
                EmbedFonts = true;
            }
            if (othersfeature.ContainsKey("IncludeFontVariants"))
            {
                IncludeFontVariants = (othersfeature["IncludeFontVariants"].Trim().Equals("Yes")) ? true : false;
            }
            else
            {
                IncludeFontVariants = true;
            }
            if (othersfeature.ContainsKey("MaxImageWidth"))
            {
                try
                {
                    MaxImageWidth = int.Parse(othersfeature["MaxImageWidth"].Trim());
                }
                catch (Exception)
                {
                    MaxImageWidth = 600;
                }
            }
            else
            {
                MaxImageWidth = 600;
            }
            // TOC Level
            if (othersfeature.ContainsKey("TOCLevel"))
            {
                TocLevel = othersfeature["TOCLevel"].Trim();
            }
            else
            {
                TocLevel = "";
            }
            // Default Font
            if (othersfeature.ContainsKey("DefaultFont"))
            {
                DefaultFont = othersfeature["DefaultFont"].Trim();
            }
            else
            {
                DefaultFont = "Charis SIL";
            }
            // Default Alignment
            if (othersfeature.ContainsKey("DefaultAlignment"))
            {
                DefaultAlignment = othersfeature["DefaultAlignment"].Trim();
            }
            else
            {
                DefaultAlignment = "Justified";
            }
            // Chapter Numbers
            if (othersfeature.ContainsKey("ChapterNumbers"))
            {
                ChapterNumbers = othersfeature["ChapterNumbers"].Trim();
            }
            else
            {
                ChapterNumbers = "Drop Cap"; // default
            }

            // Chapter Numbers
            if (othersfeature.ContainsKey("References"))
            {
                References = othersfeature["References"].Trim();
            }
            else
            {
                References = "After Each Section"; // default
            }

            // base font size
            if (othersfeature.ContainsKey("BaseFontSize"))
            {
                try
                {
                    BaseFontSize = int.Parse(othersfeature["BaseFontSize"].Trim());
                }
                catch (Exception)
                {
                    BaseFontSize = 13;
                }
            }
            else
            {
                BaseFontSize = 13;
            }
            // default line height
            if (othersfeature.ContainsKey("DefaultLineHeight"))
            {
                try
                {
                    DefaultLineHeight = int.Parse(othersfeature["DefaultLineHeight"].Trim());
                }
                catch (Exception)
                {
                    DefaultLineHeight = 125;
                }
            }
            else
            {
                DefaultLineHeight = 125;
            }
            // Missing Font
            // Note that the Embed Font enum value doesn't apply here (if it were to appear, we'd fall to the Default
            // "Prompt user" case
            if (othersfeature.ContainsKey("MissingFont"))
            {
                switch (othersfeature["MissingFont"].Trim())
                {
                    case "Use Fallback Font":
                        MissingFont = FontHandling.SubstituteDefaultFont;
                        break;
                    case "Cancel Export":
                        MissingFont = FontHandling.CancelExport;
                        break;
                    default: // "Prompt User" case goes here
                        MissingFont = FontHandling.PromptUser;
                        break;
                }
            }
            else
            {
                MissingFont = FontHandling.PromptUser;
            }
            // Non SIL Font
            if (othersfeature.ContainsKey("NonSILFont"))
            {
                switch (othersfeature["NonSILFont"].Trim())
                {
                    case "Embed Font Anyway":
                        NonSilFont = FontHandling.EmbedFont;
                        break;
                    case "Use Fallback Font":
                        NonSilFont = FontHandling.SubstituteDefaultFont;
                        break;
                    case "Cancel Export":
                        NonSilFont = FontHandling.CancelExport;
                        break;
                    default: // "Prompt User" case goes here
                        NonSilFont = FontHandling.PromptUser;
                        break;
                }
            }
            else
            {
                NonSilFont = FontHandling.PromptUser;
            }
        }
        #endregion

        #region xslt processing
        /// <summary>
        /// Helper method that copies the epub xslt file into the temp directory, optionally inserts some
        /// processing commands, then returns the path to the modified file.
        /// </summary>
        /// <returns>Full path / filename of the xslt file</returns>
        private string GetXsltFile()
        {
            string xsltFullName = Common.FromRegistry("TE_XHTML-to-epub_XHTML.xslt");
            var tempXslt = Path.Combine(Path.GetTempPath(), Path.GetFileName(xsltFullName));
            File.Copy(xsltFullName, tempXslt, true);
            xsltFullName = tempXslt;

            // Modify the local XSLT for the following conditions:
            // - Scriptures with a inline footnotes (References == "After Each Section"):
            //   adds the 

            if (_inputType.ToLower().Equals("scripture") && References.Contains("Section"))
            {
                // add references inline, after each section (first change)
                const string searchText = "<!-- Section div reference processing -->";
                var sbRef = new StringBuilder();
                sbRef.AppendLine(searchText);
                sbRef.AppendLine("<xsl:if test=\"@class = 'scrSection'\">");
                sbRef.Append("<xsl:if test=\"(count(descendant::xhtml:span[@class='Note_General_Paragraph']) +");
                sbRef.AppendLine(" count(descendant::xhtml:span[@class='Note_CrossHYPHENReference_Paragraph'])) > 0\">");
                sbRef.AppendLine("<xsl:element name=\"ul\">");
                sbRef.AppendLine("<xsl:attribute name=\"class\"><xsl:text>footnotes</xsl:text></xsl:attribute>");
                sbRef.AppendLine("<!-- general notes - use the note title for the list bullet -->");
                sbRef.AppendLine("<xsl:for-each select=\"descendant::xhtml:span[@class='Note_General_Paragraph']\">");
                sbRef.AppendLine("<xsl:element name=\"li\">");
                sbRef.AppendLine("<xsl:attribute name=\"id\"><xsl:text>FN_</xsl:text><xsl:value-of select=\"@id\"/></xsl:attribute>");
                sbRef.AppendLine("<xsl:element name=\"a\">");
                sbRef.AppendLine("<xsl:attribute name=\"href\"><xsl:text>#</xsl:text><xsl:value-of select=\"@id\"/></xsl:attribute>");
                sbRef.AppendLine("<xsl:text>[</xsl:text><xsl:value-of select=\"@title\"/><xsl:text>]</xsl:text>");
                sbRef.AppendLine("</xsl:element><xsl:text> </xsl:text><xsl:value-of select=\".\"/></xsl:element>");
                sbRef.AppendLine("</xsl:for-each>");
                sbRef.AppendLine("<!-- cross-references - use) the verse number for the list bullet -->");
                sbRef.AppendLine("<xsl:for-each select=\"descendant::xhtml:span[@class='Note_CrossHYPHENReference_Paragraph']\">");
                sbRef.AppendLine("<xsl:element name=\"li\">");
                sbRef.AppendLine("<xsl:attribute name=\"id\"><xsl:text>FN_</xsl:text><xsl:value-of select=\"@id\"/></xsl:attribute>");
                sbRef.AppendLine("<xsl:element name=\"a\">");
                sbRef.AppendLine("<xsl:attribute name=\"href\"><xsl:text>#</xsl:text><xsl:value-of select=\"@id\"/></xsl:attribute>");
                if (isNoteTargetReferenceExists)
                {
                    sbRef.AppendLine("<xsl:value-of select=\"xhtml:span[@class='Note_Target_Reference']\"/>");
                    sbRef.AppendLine("</xsl:element><xsl:text> </xsl:text>");
                    sbRef.AppendLine("<xsl:for-each select=\"xhtml:span[not(@class ='Note_Target_Reference')]\"><xsl:value-of select=\".\"/></xsl:for-each>");
                    sbRef.AppendLine("</xsl:element></xsl:for-each>");
                }
                else
                {
                    sbRef.AppendLine("<xsl:value-of select=\"preceding::xhtml:span[@class='Chapter_Number'][1]\"/><xsl:text>:</xsl:text>");
                    sbRef.AppendLine("<xsl:value-of select=\"preceding::xhtml:span[@class='Verse_Number'][1]\"/>");
                    sbRef.AppendLine("</xsl:element><xsl:text> </xsl:text><xsl:value-of select=\".\"/></xsl:element></xsl:for-each>");
                }

                sbRef.AppendLine("</xsl:element></xsl:if></xsl:if>");
                Common.StreamReplaceInFile(xsltFullName, searchText, sbRef.ToString());
                // add references inline (second change)
                sbRef.Length = 0;
                const string searchText2 = "<!-- secondary Section div reference processing -->";
                sbRef.AppendLine(searchText2);
                sbRef.Append("<xsl:if test=\"(count(descendant::xhtml:span[@class='Note_General_Paragraph']) + ");
                sbRef.AppendLine("count(descendant::xhtml:span[@class='Note_CrossHYPHENReference_Paragraph'])) > 0\">");
                sbRef.AppendLine("<xsl:element name=\"ul\">");
                sbRef.AppendLine("<xsl:attribute name=\"class\"><xsl:text>footnotes</xsl:text></xsl:attribute>");
                sbRef.AppendLine("<!-- general) notes - use the note title for the list bullet -->");
                sbRef.AppendLine("<xsl:for-each select=\"descendant::xhtml:span[@class='Note_General_Paragraph']\">");
                sbRef.AppendLine("<xsl:element name=\"li\">");
                sbRef.AppendLine("<xsl:attribute name=\"id\"><xsl:text>FN_</xsl:text><xsl:value-of select=\"@id\"/></xsl:attribute>");
                sbRef.AppendLine("<xsl:element name=\"a\">");
                sbRef.AppendLine("<xsl:attribute name=\"href\"><xsl:text>#</xsl:text><xsl:value-of select=\"@id\"/></xsl:attribute>");
                sbRef.AppendLine("<xsl:text>[</xsl:text><xsl:value-of select=\"@title\"/><xsl:text>]</xsl:text>");
                sbRef.AppendLine("</xsl:element><xsl:text> </xsl:text><xsl:value-of select=\".\"/></xsl:element></xsl:for-each>");
                sbRef.AppendLine("<!-- cross-references - use the verse number for the list bullet -->");
                sbRef.AppendLine("<xsl:for-each select=\"descendant::xhtml:span[@class='Note_CrossHYPHENReference_Paragraph']\">");
                sbRef.AppendLine("<xsl:element name=\"li\">");
                sbRef.AppendLine("<xsl:attribute name=\"id\"><xsl:text>FN_</xsl:text><xsl:value-of select=\"@id\"/></xsl:attribute>");
                sbRef.AppendLine("<xsl:element name=\"a\">");
                sbRef.AppendLine("<xsl:attribute name=\"href\"><xsl:text>#</xsl:text><xsl:value-of select=\"@id\"/></xsl:attribute>");
                if (isNoteTargetReferenceExists)
                {
                    sbRef.AppendLine("<xsl:value-of select=\"xhtml:span[@class='Note_Target_Reference']\"/>");
                    sbRef.AppendLine("</xsl:element><xsl:text> </xsl:text>");
                    sbRef.AppendLine("<xsl:for-each select=\"xhtml:span[not(@class ='Note_Target_Reference')]\"><xsl:value-of select=\".\"/></xsl:for-each>");
                    sbRef.AppendLine("</xsl:element>");
                }
                else
                {
                    sbRef.AppendLine("<xsl:value-of select=\"preceding::xhtml:span[@class='Chapter_Number'][1]\"/><xsl:text>:</xsl:text>");
                    sbRef.AppendLine("<xsl:value-of select=\"preceding::xhtml:span[@class='Verse_Number'][1]\"/>");
                    sbRef.AppendLine("</xsl:element><xsl:text> </xsl:text><xsl:value-of select=\".\"/></xsl:element>");
                }

                sbRef.AppendLine("</xsl:for-each></xsl:element></xsl:if>");
                Common.StreamReplaceInFile(xsltFullName, searchText2, sbRef.ToString());
            }
            return xsltFullName;
        }
        #endregion

        #region CSS processing
        /// <summary>
        /// Modifies the CSS based on the parameters from the Configuration Tool:
        /// - BaseFontSize
        /// - DefaultLineHeight
        /// - DefaultAlignment
        /// - ChapterNumbers
        /// </summary>
        /// <param name="cssFile"></param>
        private void CustomizeCSS(string cssFile)
        {
            if (!File.Exists(cssFile)) return;
            // BaseFontSize and DefaultLineHeight - body element only
            var sb = new StringBuilder();
            sb.AppendLine("body {");
            sb.Append("font-size: ");
            sb.Append(BaseFontSize);
            sb.AppendLine("pt;");
            sb.Append("line-height: ");
            sb.Append(DefaultLineHeight);
            sb.AppendLine("%;");
            Common.StreamReplaceInFile(cssFile, "body {", sb.ToString());
            // ChapterNumbers - scripture only
            if (_inputType == "scripture")
            {
                // ChapterNumbers (drop cap or in margin) - .Chapter_Number and .Paragraph1 class elements
                sb.Length = 0;  // reset the stringbuilder
                sb.AppendLine(".Chapter_Number {");
                sb.Append("font-size: ");
                if (ChapterNumbers == "Drop Cap")
                {
                    sb.AppendLine("250%;");
                    // vertical alignment of Cap specified by setting the padding-top to (defaultlineheight / 2)
                    sb.Append("padding-top: ");
                    sb.Append(BaseFontSize / 2);
                    sb.AppendLine("pt;");
                }
                else
                {
                    sb.AppendLine("24pt;");
                }
                Common.StreamReplaceInFile(cssFile, ".Chapter_Number {", sb.ToString());
            }
            // DefaultAlignment - several spots in the css file
            sb.Length = 0; // reset the stringbuilder
            sb.Append("text-align: ");
            sb.Append(DefaultAlignment.ToLower());
            sb.AppendLine(";");
            Common.StreamReplaceInFile(cssFile, "text-align:left;", sb.ToString());
        }
        /// <summary>
        /// This method addresses an ugly problem in e-book readers, where the :before and :after pseudo-items in the CSS aren't
        /// recognized (causing some bad spacing / punctuation). We work around the issue by moving the properties into an XSLT
        /// file and running it on the given list of xhtmlFiles -- moving the characters into the XHTML so it will render
        /// in all readers.
        /// </summary>
        /// <param name="cssFile">CSS file containing content properties that need to be moved into the XHTML</param>
        /// <param name="xhtmlFiles">List of XHTML files that will get the content characters inserted via our generated XSLT.</param>
        private void ContentCssToXhtml(string cssFile, List<string> xhtmlFiles, InProcess inProcess)
        {
            if (!File.Exists(cssFile)) { return; }
            string xsltFullName = Common.FromRegistry("punct_XHTML.xslt");
            if (!File.Exists(xsltFullName)) { return; }
            var tempXslt = Path.Combine(Path.GetTempPath(), Path.GetFileName(xsltFullName));
            File.Copy(xsltFullName, tempXslt, true);

            // copy the CSS rules into a Dictionary and iterate through it
            var sbBefore = new StringBuilder();
            var sbAfter = new StringBuilder();
            var sbBetweenXItem = new StringBuilder();
            var sbBetweenSense = new StringBuilder();
            Dictionary<string, Dictionary<string, string>> cssClass = new Dictionary<string, Dictionary<string, string>>();
            CssTree cssTree = new CssTree();
            cssTree.OutputType = Common.OutputType.EPUB;
            cssClass = cssTree.CreateCssProperty(cssFile, true);
            int startIndex = 0;
            // iterate through the Dictionary. We're looking for instances of :before, :after and .xitem + xitem:before
            // that have "content" properties -- when we find one, we'll convert the CSS into an XSLT rule that will insert
            // the content text into XHTML.
            foreach (string key in cssClass.Keys)
            {
                if (key.Contains("..after"))
                {
                    if (cssClass[key].ContainsKey("content"))
                    {
                        // found an item - is it the first?
                        if (sbAfter.Length == 0)
                        {
                            // first item - write out the comment
                            sbAfter.AppendLine("<!-- CLOSING lexical punctuation-->");
                        }
                        // write out the rule - if you come across the class, add the content text to the xhtml
                        sbAfter.Append("<xsl:if test = \"@class = '");
                        sbAfter.Append(key.Substring(0, key.Length - 7));
                        sbAfter.Append("'\"><xsl:text>");
                        sbAfter.Append(cssClass[key]["content"]);
                        sbAfter.AppendLine("</xsl:text></xsl:if>");
                    }
                }
                else if (key.Contains("..before"))
                {
                    // this could be either a :before or <class> + <class>:before (i.e., between items) -
                    // figure out which it is
                    if (key.Contains("xitem-xitem"))
                    {
                        if (cssClass[key].ContainsKey("content"))
                        {
                            // found an item - is it the first?
                            if (sbBetweenXItem.Length == 0)
                            {
                                // first item - write out the opening rule:
                                // for the <class> + <class>:before case, we're using an <xsl:choose> statement
                                sbBetweenXItem.AppendLine("<xsl:if test=\"following-sibling::xhtml:span[@class='xitem'] and @class='xitem' \">");
                                sbBetweenXItem.AppendLine("<!-- These are for the most part separators between items -->");
                                sbBetweenXItem.AppendLine("<xsl:choose>");
                            }
                            // write out the rule - if you come across the class, add the content text to the xhtml
                            sbBetweenXItem.Append("<xsl:when test = \"parent::xhtml:span[@class = '");
                            startIndex = key.IndexOf("_") + 1;
                            sbBetweenXItem.Append(key.Substring(startIndex, key.Length - 8 - startIndex));
                            sbBetweenXItem.Append("']\"><xsl:text>");
                            sbBetweenXItem.Append(cssClass[key]["content"]);
                            sbBetweenXItem.AppendLine("</xsl:text></xsl:when>");
                        }
                    }
                    else if (key.Contains("sense-sense"))
                    {
                        if (cssClass[key].ContainsKey("content"))
                        {
                            // found an item - is it the first?
                            if (sbBetweenSense.Length == 0)
                            {
                                // first item - write out the opening rule:
                                // for the <class> + <class>:before case, we're using an <xsl:choose> statement
                                sbBetweenSense.AppendLine("<xsl:if test=\"following-sibling::xhtml:span[@class='xitem'] and @class='xitem' \">");
                                sbBetweenSense.AppendLine("<!-- These are for the most part separators between items -->");
                                sbBetweenSense.AppendLine("<xsl:choose>");
                            }
                            // write out the rule - if you come across the class, add the content text to the xhtml
                            sbBetweenSense.Append("<xsl:when test = \"parent::xhtml:span[@class = '");
                            startIndex = key.IndexOf("_") + 1;
                            sbBetweenSense.Append(key.Substring(startIndex, key.Length - 8 - startIndex));
                            sbBetweenSense.Append("']\"><xsl:text>");
                            sbBetweenSense.Append(cssClass[key]["content"]);
                            sbBetweenSense.AppendLine("</xsl:text></xsl:when>");
                        }
                    }
                    else
                    {
                        if (cssClass[key].ContainsKey("content"))
                        {
                            // found an item - is it the first?
                            if (sbBefore.Length == 0)
                            {
                                // first item - write out the comment
                                sbBefore.AppendLine("<!-- OPENING lexical punctuation-->");
                            }
                            // write out the rule - if you come across the class, add the content text to the xhtml
                            sbBefore.Append("<xsl:if test = \"@class = '");
                            if (key.Contains("span."))
                            {
                                sbBefore.Append(key.Substring(5, key.Length - 13));
                            }
                            else
                            {
                                sbBefore.Append(key.Substring(0, key.Length - 8));
                            }
                            sbBefore.Append("'\"><xsl:text>");
                            sbBefore.Append(cssClass[key]["content"]);
                            sbBefore.AppendLine("</xsl:text></xsl:if>");
                        }
                    }
                }
            }
            // close out the StringBuilders
            if (sbBetweenXItem.Length > 0)
            {
                sbBetweenXItem.AppendLine("</xsl:choose></xsl:if><!-- between xitems -->");
            }
            if (sbBetweenSense.Length > 0)
            {
                sbBetweenSense.AppendLine("</xsl:choose></xsl:if><!-- between senses -->");
            }
            // append the two "between" cases to the after case
            sbAfter.AppendLine(sbBetweenSense.ToString());
            sbAfter.AppendLine(sbBetweenXItem.ToString());

            // replace the before and after blocks in the XSLT
            Common.StreamReplaceInFile(tempXslt, "<!-- OPENING lexical punctuation -->", sbBefore.ToString());
            Common.StreamReplaceInFile(tempXslt, "<!-- CLOSING lexical punctuation -->", sbAfter.ToString());

            // final step - run the updated xslt file
            foreach (var xhtmlFile in xhtmlFiles)
            {
                if (cssFile.Contains("FlexRev"))
                {
                    // running against reversal index files - skip the main ones
                    if (xhtmlFile.Contains("PartFile"))
                        continue;
                }
                else
                {
                    // running against the main files - skip the reversal index ones
                    if (xhtmlFile.Contains("RevIndex"))
                        continue;
                }
                inProcess.SetStatus("Folding punctuation into XHTML: " + xhtmlFile);
                Common.XsltProcess(xhtmlFile, tempXslt, Path.GetExtension(xhtmlFile) + ".tmp");
                // replace the original file
                if (File.Exists(xhtmlFile))
                {
                    File.Delete(xhtmlFile);
                }
                File.Move(xhtmlFile + ".tmp", xhtmlFile);
            }
        }


        /// <summary>
        /// Removes stylings that don't work with e-book readers from the specified .css file.
        /// </summary>
        /// <param name="cssFile"></param>
        private void RemovePagedStylesFromCss(string cssFile)
        {
            if (!File.Exists(cssFile)) { return; }
            // open the file
            var reader = new StreamReader(cssFile);
            var writer = new StreamWriter(cssFile + ".tmp");
            bool done = false;
            string oneLine = null;
            while (!done)
            {
                oneLine = reader.ReadLine();
                if (oneLine == null)
                {
                    done = true;
                    continue;
                }
                if (oneLine.Contains("/** imported"))
                {
                    writer.WriteLine(oneLine);
                    done = true;
                    continue;
                }
                // epub readers vary in their support for :before and :after (which FLEx uses to insert content: punctuation) -
                // we've already transformed the text to include these items in ContentCssToXhtml(); remove them from the css now.
                if (oneLine.Contains("content:"))
                {
                    if (!oneLine.Contains("counter(sense, disc)"))
                        continue;
                }
                // font family and size are specified elsewhere in the css - remove from the merged part
                if (oneLine.Contains("font-family") || oneLine.Contains("font-size"))
                {
                    continue;
                }
                // epub doesn't work with footnote, prince-footnote, columns, string-sets or counters
                if (oneLine.Contains("display: footnote") || oneLine.Contains("display: prince-footnote") ||
                    oneLine.Contains("position: footnote") || oneLine.Contains("column-count") || oneLine.Contains("column-gap") ||
                    oneLine.Contains("string-set") || oneLine.Contains("column-fill") || oneLine.Contains("counter-reset"))
                {
                    continue;
                }
                // These are blocks that we need to remove completely:
                // - The @page and ::<something> pseudo-elements are also not supported, at least in the way they are
                //   generated by the xhtml export (i.e., for paged media).
                // - The .picture class style from the merged Scripture CSS is too small (we resize it as needed for .epub)
                if (oneLine.Contains("@page") || oneLine.Contains("::") || oneLine.Contains(".picture "))
                {
                    // match the bracket count until we get back to 0 -- this will mark the end of the css block
                    int bracketCount = 1;
                    while (bracketCount != 0 && !reader.EndOfStream)
                    {
                        var nextChar = (char)reader.Read();
                        switch (nextChar)
                        {
                            case '{':
                                // found a sub-element - make sure we have a matching pair
                                bracketCount++;
                                break;
                            case '}':
                                // closed out an element (or sub-element) - decrement the bracket count
                                bracketCount--;
                                break;
                            default:
                                break;
                        }
                    }
                    // the entire CSS should now be dropped -- continue on to the next line of data
                    continue;
                }
                // if we got here, the line is good - write it out
                writer.WriteLine(oneLine);
            }
            // there's nothing more we're interested in - read the rest of the file out
            if (oneLine != null)
            {
                writer.Write(reader.ReadToEnd());
            }
            writer.Close();
            reader.Close();
            // now copy over our changes
            if (File.Exists(cssFile))
            {
                // should always be the case
                File.Delete(cssFile);
            }
            File.Move(cssFile + ".tmp", cssFile);
        }
        #endregion

        #region Font Handling
        /// <summary>
        /// Handles font embedding for the .epub file. The fonts are verified before they are copied over, to
        /// make sure they (1) exist on the system and (2) are SIL produced. For the latter, the user is able
        /// to embed them anyway if they click that they have the appropriate rights (it's an honor system approach).
        /// </summary>
        /// <param name="langArray"></param>
        /// <param name="contentFolder"></param>
        /// <returns></returns>
        private bool EmbedAllFonts(string[] langArray, string contentFolder)
        {
            var nonSILFonts = new Dictionary<EmbeddedFont, string>();
            string langs;
            // Build the list of non-SIL fonts in use
            foreach (var embeddedFont in _embeddedFonts)
            {
                if (!embeddedFont.Value.CanRedistribute)
                {
                    foreach (var language in _langFontDictionary.Keys)
                    {
                        if (_langFontDictionary[language].Equals(embeddedFont.Key))
                        {
                            // add this language to the list of langs that use this font
                            if (nonSILFonts.TryGetValue(embeddedFont.Value, out langs))
                            {
                                // existing entry - add this language to the list of langs that use this font
                                var sbName = new StringBuilder();
                                sbName.Append(langs);
                                sbName.Append(", ");
                                sbName.Append(language);
                                // set the value
                                nonSILFonts[embeddedFont.Value] = sbName.ToString();
                            }
                            else
                            {
                                // new entry
                                nonSILFonts.Add(embeddedFont.Value, language);
                            }
                        }
                    }
                }
            }
            // If there are any non-SIL fonts in use, show the Font Warning Dialog
            // (possibly multiple times) and replace our embedded font items if needed
            // (if we're running a test, skip the dialog and just embed the font)
            if (nonSILFonts.Count > 0 && !Common.Testing)
            {
                FontWarningDlg dlg = new FontWarningDlg();
                dlg.RepeatAction = false;
                dlg.RemainingIssues = nonSILFonts.Count - 1;
                // Handle the cases where the user wants to automatically process non-SIL / missing fonts
                if (NonSilFont == FontHandling.CancelExport)
                {
                    // TODO: implement message box
                    // Give the user a message indicating there's a non-SIL font in their writing system, and
                    // to go fix the problem. Don't let them continue with the export.
                    return false;
                }
                if (NonSilFont != FontHandling.PromptUser)
                {
                    dlg.RepeatAction = true; // the handling picks up below...
                    dlg.SelectedFont = DefaultFont;
                }
                bool isMissing = false;
                bool isManualProcess = false;
                foreach (var nonSilFont in nonSILFonts)
                {
                    dlg.MyEmbeddedFont = nonSilFont.Key.Name;
                    dlg.Languages = nonSilFont.Value;
                    isMissing = (nonSilFont.Key.Filename == null);
                    isManualProcess = ((isMissing == false && NonSilFont == FontHandling.PromptUser) || (isMissing == true && MissingFont == FontHandling.PromptUser));
                    if (dlg.RepeatAction)
                    {
                        // user wants to repeat the last action - if the last action
                        // was to change the font, change this one as well
                        // (this is also where the automatic FontHandling takes place)
                        if ((!dlg.UseFontAnyway() && !nonSilFont.Key.Name.Equals(dlg.SelectedFont) && isManualProcess) || // manual "repeat this action" for non-SIL AND missing fonts
                            (isMissing == false && NonSilFont == FontHandling.SubstituteDefaultFont && !nonSilFont.Key.Name.Equals(DefaultFont)) || // automatic for non-SIL fonts
                            (isMissing == true && MissingFont == FontHandling.SubstituteDefaultFont && !nonSilFont.Key.Name.Equals(DefaultFont))) // automatic for missing fonts
                        {
                            // the user has chosen a different (SIL) font - 
                            // create a new EmbeddedFont and add it to the list
                            _embeddedFonts.Remove(nonSilFont.Key.Name);
                            var newFont = new EmbeddedFont(dlg.SelectedFont);
                            _embeddedFonts[dlg.SelectedFont] = newFont; // set index value adds if it doesn't exist
                            // also update the references in _langFontDictionary
                            foreach (var lang in langArray)
                            {
                                if (_langFontDictionary[lang] == nonSilFont.Key.Name)
                                {
                                    _langFontDictionary[lang] = dlg.SelectedFont;
                                }
                            }
                        }
                        // the UseFontAnyway checkbox (and FontHandling.EmbedFont) cases fall through here -
                        // The current non-SIL font is ignored and embedded below
                        continue;
                    }
                    // sanity check - are there any SIL fonts installed?
                    int count = dlg.BuildSILFontList();
                    if (count == 0)
                    {
                        // No SIL fonts found (returns a DialogResult.Abort):
                        // tell the user there are no SIL fonts installed, and allow them to Cancel
                        // and install the fonts now
                        if (MessageBox.Show(Resources.NoSILFontsMessage, Resources.NoSILFontsTitle,
                                             MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
                            == DialogResult.Cancel)
                        {
                            // user cancelled the operation - Cancel out of the whole .epub export
                            return false;
                        }
                        // user clicked OK - leave the embedded font list alone and continue the export
                        // (presumably the user has the proper rights to this font, even though it isn't
                        // an SIL font)
                        break;
                    }
                    // show the dialog
                    DialogResult result = dlg.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        if (!dlg.UseFontAnyway() && !nonSilFont.Key.Name.Equals(dlg.SelectedFont))
                        {
                            // the user has chosen a different (SIL) font - 
                            // create a new EmbeddedFont and add it to the list
                            _embeddedFonts.Remove(nonSilFont.Key.Name);
                            var newFont = new EmbeddedFont(dlg.SelectedFont);
                            _embeddedFonts[dlg.SelectedFont] = newFont; // set index value adds if it doesn't exist
                            // also update the references in _langFontDictionary
                            foreach (var lang in langArray)
                            {
                                if (_langFontDictionary[lang] == nonSilFont.Key.Name)
                                {
                                    _langFontDictionary[lang] = dlg.SelectedFont;
                                }
                            }
                        }
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        // User cancelled - Cancel out of the whole .epub export
                        return false;
                    }
                    // decrement the remaining issues for the next dialog display
                    dlg.RemainingIssues--;
                }
            }
            // copy all the fonts over
            foreach (var embeddedFont in _embeddedFonts.Values)
            {
                if (embeddedFont.Filename == null)
                {
                    Debug.WriteLine("ERROR: embedded font " + embeddedFont.Name + " is not installed - skipping");
                    continue;
                }
                string dest = Common.PathCombine(contentFolder, Path.GetFileName(embeddedFont.Filename));
                if (embeddedFont.Filename.ToString() != string.Empty && File.Exists(embeddedFont.Filename))
                {
                    File.Copy(embeddedFont.Filename, dest, true);

                    if (IncludeFontVariants)
                    {
                        // italic
                        if (embeddedFont.HasItalic && embeddedFont.ItalicFilename.Trim().Length > 0 &&
                            embeddedFont.ItalicFilename != embeddedFont.Filename)
                        {
                            dest = Common.PathCombine(contentFolder, Path.GetFileName(embeddedFont.ItalicFilename));
                            if (!File.Exists(dest))
                            {
                                File.Copy(embeddedFont.ItalicFilename, dest, true);
                            }
                        }
                        // bold
                        if (embeddedFont.HasBold && embeddedFont.BoldFilename.Trim().Length > 0 &&
                            embeddedFont.BoldFilename != embeddedFont.Filename)
                        {
                            dest = Common.PathCombine(contentFolder, Path.GetFileName(embeddedFont.BoldFilename));
                            if (!File.Exists(dest))
                            {
                                File.Copy(embeddedFont.BoldFilename,
                                          dest, true);
                            }
                        }
                    }
                }
            }
            // clean up
            if (nonSILFonts.Count > 0)
            {
                nonSILFonts.Clear();
            }
            return true;
        }

        /// <summary>
        /// Inserts links in the CSS file to the fonts used by the writing systems:
        /// - If the fonts are embedded, adds a @font-face declaration referencing the .ttf file 
        ///   that's found in the archive
        /// - Sets the font-family for the body:lang selector to the referenced font
        /// </summary>
        /// <param name="cssFile"></param>
        private void ReferenceFonts(string cssFile)
        {
            if (!File.Exists(cssFile)) return;
            // read in the CSS file
            string mainTextDirection = "ltr";
            var reader = new StreamReader(cssFile);
            string content = reader.ReadToEnd();
            reader.Close();
            var sb = new StringBuilder();
            // write a timestamp for field troubleshooting
            sb.Append("/* font info - added by ");
            sb.Append(Application.ProductName);
            sb.Append(" (");
            sb.Append(Assembly.GetCallingAssembly().FullName);
            sb.AppendLine(") */");
            // If we're embedding the fonts, build the @font-face elements))))
            if (EmbedFonts)
            {
                foreach (var embeddedFont in _embeddedFonts.Values)
                {
                    if (embeddedFont.Filename == null)
                    {
                        sb.Append("/* missing embedded font: ");
                        sb.Append(embeddedFont.Name);
                        sb.AppendLine(" */");
                        continue;
                    }
                    sb.AppendLine("@font-face {");
                    sb.Append(" font-family : ");
                    sb.Append(embeddedFont.Name);
                    sb.AppendLine(";");
                    sb.AppendLine(" font-weight : normal;");
                    sb.AppendLine(" font-style : normal;");
                    sb.AppendLine(" font-variant : normal;");
                    sb.AppendLine(" font-size : all;");
                    sb.Append(" src : url('");
                    sb.Append(Path.GetFileName(embeddedFont.Filename));
                    sb.AppendLine("');");
                    sb.AppendLine("}");
                    // if we're also embedding the font variants (bold, italic), reference them now
                    if (IncludeFontVariants)
                    {
                        // Italic version
                        if (embeddedFont.HasItalic)
                        {
                            sb.AppendLine("@font-face {");
                            sb.Append(" font-family : i_");
                            sb.Append(embeddedFont.Name);
                            sb.AppendLine(";");
                            sb.AppendLine(" font-weight : normal;");
                            sb.AppendLine(" font-style : italic;");
                            sb.AppendLine(" font-variant : normal;");
                            sb.AppendLine(" font-size : all;");
                            sb.Append(" src : url('");
                            sb.Append(Path.GetFileName(embeddedFont.ItalicFilename));
                            sb.AppendLine("');");
                            sb.AppendLine("}");
                        }
                        // Bold version
                        if (embeddedFont.HasBold)
                        {
                            sb.AppendLine("@font-face {");
                            sb.Append(" font-family : b_");
                            sb.Append(embeddedFont.Name);
                            sb.AppendLine(";");
                            sb.AppendLine(" font-weight : bold;");
                            sb.AppendLine(" font-style : normal;");
                            sb.AppendLine(" font-variant : normal;");
                            sb.AppendLine(" font-size : all;");
                            sb.Append(" src : url('");
                            sb.Append(Path.GetFileName(embeddedFont.BoldFilename));
                            sb.AppendLine("');");
                            sb.AppendLine("}");
                        }
                    }
                }
            }
            // add :lang pseudo-elements for each language and set them to the proper font
            bool firstLang = true;
            foreach (var language in _langFontDictionary)
            {
                EmbeddedFont embeddedFont;
                // If this is the first language in the loop (i.e., the main language),
                // set the font for the body element
                if (firstLang)
                {
                    sb.AppendLine("/* default language font info */");
                    sb.AppendLine("body {");
                    sb.Append("font-family: '");
                    sb.Append(language.Value);
                    sb.Append("', ");
                    if (_embeddedFonts.TryGetValue(language.Value, out embeddedFont))
                    {
                        sb.AppendLine((embeddedFont.Serif) ? "Times, serif;" : "Arial, sans-serif;");
                    }
                    else
                    {
                        // fall back on a serif font if we can't find it (shouldn't happen)
                        sb.AppendLine("Times, serif;");
                    }
                    // also insert the text direction for this language
                    sb.Append("direction: ");
                    mainTextDirection = Common.GetTextDirection(language.Key);
                    sb.Append(Common.GetTextDirection(language.Key));
                    sb.AppendLine(";");
                    sb.AppendLine("}");
                    if (IncludeFontVariants)
                    {
                        // Italic version
                        if (embeddedFont != null)
                            if (embeddedFont.HasItalic)
                            {
                                sb.Append(".partofspeech, .example, .grammatical-info, .lexref-type, ");
                                sb.Append(".parallel_passage_reference, .Parallel_Passage_Reference, ");
                                sb.AppendLine(".Emphasis, .pictureCaption, .Section_Range_Paragraph {");
                                if (language.Value.ToLower() == "charis sil")
                                {
                                    sb.Append("font-family: '" + language.Value.Trim());
                                    sb.Append("-i");
                                }
                                else
                                {
                                    sb.Append("font-family: 'i_");
                                    sb.Append(language.Value);
                                }
                                sb.Append("', ");
                                if (_embeddedFonts.TryGetValue(language.Value, out embeddedFont))
                                {
                                    sb.AppendLine((embeddedFont.Serif) ? "Times, serif;" : "Arial, sans-serif;");
                                }
                                else
                                {
                                    // fall back on a serif font if we can't find it (shouldn't happen)
                                    sb.AppendLine("Times, serif;");
                                }
                                sb.AppendLine("}");
                            }
                        // Bold version
                        if (embeddedFont != null)
                            if (embeddedFont.HasBold)
                            {
                                sb.Append(
                                    ".headword, .headword-minor, .LexSense-publishStemMinorPrimaryTarget-OwnerOutlinePub, ");
                                sb.Append(".LexEntry-publishStemMinorPrimaryTarget-MLHeadWordPub, .xsensenumber");
                                sb.Append(
                                    ".complexform-form, .crossref, .LexEntry-publishStemComponentTarget-MLHeadWordPub, ");
                                sb.Append(
                                    ".LexEntry-publishStemMinorPrimaryTarget-MLHeadWordPub, .LexSense-publishStemComponentTarget-OwnerOutlinePub, ");
                                sb.Append(".LexSense-publishStemMinorPrimaryTarget-OwnerOutlinePub, .sense-crossref, ");
                                sb.Append(".crossref-headword, .reversal-form, ");
                                //sb.Append(".Alternate_Reading, .Section_Head, .Section_Head_Minor, ");
                                sb.Append(".Alternate_Reading, .Section_Head_Minor, ");
                                sb.AppendLine(".Inscription, .Intro_Section_Head, .Section_Head_Major, .iot {");
                                if (language.Value.ToLower() == "charis sil")
                                {
                                    sb.Append("font-family: '" + language.Value.Trim());
                                    sb.Append("-b");
                                }
                                else
                                {
                                    sb.Append("font-family: 'b_");
                                    sb.Append(language.Value);
                                }
                                sb.Append("', ");
                                if (_embeddedFonts.TryGetValue(language.Value, out embeddedFont))
                                {
                                    sb.AppendLine((embeddedFont.Serif) ? "Times, serif;" : "Arial, sans-serif;");
                                }
                                else
                                {
                                    // fall back on a serif font if we can't find it (shouldn't happen)
                                    sb.AppendLine("Times, serif;");
                                }
                                sb.AppendLine("}");
                            }
                    }
                    // finished processing - clear the flag
                    firstLang = false;
                }

                // set the font for the *:lang(xxx) pseudo-element
                sb.Append("*:lang(");
                sb.Append(language.Key);
                sb.AppendLine(") {");
                sb.Append("font-family: '");
                sb.Append(language.Value);
                sb.Append("', ");
                if (_embeddedFonts.TryGetValue(language.Value, out embeddedFont))
                {
                    sb.AppendLine((embeddedFont.Serif) ? "Times, serif;" : "Arial, sans-serif;");
                }
                else
                {
                    // fall back on a serif font if we can't find it (shouldn't happen)
                    sb.AppendLine("Times, serif;");
                }
                // also insert the text direction for this language
                sb.Append("direction: ");
                sb.Append(Common.GetTextDirection(language.Key));
                sb.AppendLine(";");
                sb.AppendLine("}");

                if (IncludeFontVariants)
                {
                    // italic version
                    if (embeddedFont != null)
                        if (embeddedFont.HasItalic)
                        {
                            // dictionary
                            sb.Append(".partofspeech:lang(");
                            sb.Append(language.Key);
                            sb.Append("), .example:lang(");
                            sb.Append(language.Key);
                            sb.Append("), .grammatical-info:lang(");
                            sb.Append(language.Key);
                            sb.Append("), .lexref-type:lang(");
                            sb.Append(language.Key);
                            // scripture
                            sb.Append("), .parallel_passage_reference:lang(");
                            sb.Append(language.Key);
                            sb.Append("), .Parallel_Passage_Reference:lang(");
                            sb.Append(language.Key);
                            sb.Append("), .Emphasis:lang(");
                            sb.Append(language.Key);
                            sb.Append("), .pictureCaption:lang(");
                            sb.Append(language.Key);
                            sb.Append("), .Section_Range_Paragraph:lang(");
                            sb.Append(language.Key);
                            sb.AppendLine(") {");
                            sb.Append("font-family: 'i_");
                            sb.Append(language.Value);
                            sb.Append("', ");
                            if (_embeddedFonts.TryGetValue(language.Value, out embeddedFont))
                            {
                                sb.AppendLine((embeddedFont.Serif) ? "Times, serif;" : "Arial, sans-serif;");
                            }
                            else
                            {
                                // fall back on a serif font if we can't find it (shouldn't happen)
                                sb.AppendLine("Times, serif;");
                            }
                            sb.AppendLine("}");
                        }
                    // bold version
                    if (embeddedFont != null)
                        if (embeddedFont.HasBold)
                        {
                            // dictionary
                            sb.Append(".headword:lang(");
                            sb.Append(language.Key);
                            sb.Append("), .headword-minor:lang(");
                            sb.Append(language.Key);
                            sb.Append("), .LexSense-publishStemMinorPrimaryTarget-OwnerOutlinePub:lang(");
                            sb.Append(language.Key);
                            sb.Append("), .LexEntry-publishStemMinorPrimaryTarget-MLHeadWordPub:lang(");
                            sb.Append(language.Key);
                            sb.Append("), .xsensenumber:lang(");
                            sb.Append(language.Key);
                            sb.Append("), .complexform-form:lang(");
                            sb.Append(language.Key);
                            sb.Append("), .crossref:lang(");
                            sb.Append(language.Key);
                            sb.Append("), .LexEntry-publishStemComponentTarget-MLHeadWordPub:lang(");
                            sb.Append(language.Key);
                            sb.Append("), .LexEntry-publishStemMinorPrimaryTarget-MLHeadWordPub:lang(");
                            sb.Append(language.Key);
                            sb.Append("), .LexSense-publishStemComponentTarget-OwnerOutlinePub:lang(");
                            sb.Append(language.Key);
                            sb.Append("), .LexSense-publishStemMinorPrimaryTarget-OwnerOutlinePub:lang(");
                            sb.Append(language.Key);
                            sb.Append("), .sense-crossref:lang(");
                            sb.Append(language.Key);
                            sb.Append("), .crossref-headword:lang(");
                            sb.Append(language.Key);
                            sb.Append("), .reversal-form:lang(");
                            sb.Append(language.Key);
                            sb.Append("), .Alternate_Reading:lang(");
                            // scripture
                            sb.Append(language.Key);
                            sb.Append("), .Section_Head:lang(");
                            sb.Append(language.Key);
                            sb.Append("), .Section_Head_Minor:lang(");
                            sb.Append(language.Key);
                            sb.Append("), .Inscription:lang(");
                            sb.Append(language.Key);
                            sb.Append("), .Intro_Section_Head:lang(");
                            sb.Append(language.Key);
                            sb.Append("), .Section_Head_Major:lang(");
                            sb.Append(language.Key);
                            sb.Append("), .iot:lang(");
                            sb.Append(language.Key);
                            sb.AppendLine(") {");
                            sb.Append("font-family: 'b_");
                            sb.Append(language.Value);
                            sb.Append("', ");
                            if (_embeddedFonts.TryGetValue(language.Value, out embeddedFont))
                            {
                                sb.AppendLine((embeddedFont.Serif) ? "Times, serif;" : "Arial, sans-serif;");
                            }
                            else
                            {
                                // fall back on a serif font if we can't find it (shouldn't happen)
                                sb.AppendLine("Times, serif;");
                            }
                            sb.AppendLine("}");
                        }
                }
            }
            sb.AppendLine("/* end auto-generated font info */");
            sb.AppendLine();
            // nuke the @import statement (we're going off one CSS file here)
            //string contentNoImport = content.Substring(content.IndexOf(';') + 1);
            //sb.Append(contentNoImport);
            // remove the @import statement IF it exists in the css file
            sb.Append(content.StartsWith("@import") ? content.Substring(content.IndexOf(';') + 1) : content);
            // write out the updated CSS file
            var writer = new StreamWriter(cssFile);
            writer.Write(sb.ToString());
            writer.Close();
            // Now that we know the text direction, we can add some padding info for the chapter numbers
            // (Scripture only)
            if (_inputType == "scripture")
            {
                sb.Length = 0; // reset the stringbuilder
                sb.AppendLine(".Chapter_Number {");
                sb.Append("float: ");
                sb.Append(mainTextDirection.ToLower().Equals("ltr") ? "left" : "right");
                sb.AppendLine(";");
                if (mainTextDirection.ToLower().Equals("ltr"))
                {
                    sb.Append("padding-right: 5pt; padding-left: ");
                }
                else
                {
                    sb.Append("padding-left: 5pt; padding-right: ");
                }
                sb.Append((ChapterNumbers == "Drop Cap") ? "4%;" : "5pt;");
                Common.StreamReplaceInFile(cssFile, ".Chapter_Number {", sb.ToString());
            }
        }

        /// <summary>
        /// Returns the font families for the languages in _langFontDictionary.
        /// </summary>
        private void BuildFontsList()
        {
            // modifying the _langFontDictionary dictionary - let's make an array copy for the iteration
            int numLangs = _langFontDictionary.Keys.Count;
            var langs = new string[numLangs];
            _langFontDictionary.Keys.CopyTo(langs, 0);
            foreach (var language in langs)
            {
                string[] langCoun = language.Split('-');

                try
                {
                    string wsPath;
                    if (langCoun.Length < 2)
                    {
                        // try the language (no country code) (e.g, "en" for "en-US")
                        wsPath = Common.PathCombine(Common.GetLDMLPath(), langCoun[0] + ".ldml");
                    }
                    else
                    {
                        // try the whole language expression (e.g., "ggo-Telu-IN")
                        wsPath = Common.PathCombine(Common.GetLDMLPath(), language + ".ldml");
                    }
                    if (File.Exists(wsPath))
                    {
                        var ldml = Common.DeclareXMLDocument(false);
                        ldml.Load(wsPath);
                        var nsmgr = new XmlNamespaceManager(ldml.NameTable);
                        nsmgr.AddNamespace("palaso", "urn://palaso.org/ldmlExtensions/v1");
                        var node = ldml.SelectSingleNode("//palaso:defaultFontFamily/@value", nsmgr);
                        if (node != null)
                        {
                            // build the font information and return
                            _langFontDictionary[language] = node.Value; // set the font used by this language
                            _embeddedFonts[node.Value] = new EmbeddedFont(node.Value);
                        }
                    }
                    else if (AppDomain.CurrentDomain.FriendlyName.ToLower() == "paratext.exe") // is paratext
                    {
                        SettingsHelper settingsHelper = new SettingsHelper(Param.DatabaseName);
                        string fileName = settingsHelper.GetSettingsFilename();
                        string xPath = "//ScriptureText/DefaultFont";
                        XmlNode xmlFont = Common.GetXmlNode(fileName, xPath);
                        if (xmlFont != null)
                        {
                            // get the text direction specified by the .ssf file
                            _langFontDictionary[language] = xmlFont.InnerText; // set the font used by this language
                            _embeddedFonts[xmlFont.InnerText] = new EmbeddedFont(xmlFont.InnerText);
                        }
                    }
                    else
                    {
                        // Paratext case (no .ldml file) - fall back on Charis
                        _langFontDictionary[language] = "Charis SIL"; // set the font used by this language
                        _embeddedFonts["Charis SIL"] = new EmbeddedFont("Charis SIL");

                    }
                }
                catch
                {
                }
            }
        }

        #endregion

        #region Language Handling
        /// <summary>
        /// Parses the specified file and sets the internal languages list to all the languages found in the file.
        /// </summary>
        /// <param name="xhtmlFileName">File name to parse</param>
        private void BuildLanguagesList(string xhtmlFileName)
        {
            XmlDocument xmlDocument = Common.DeclareXMLDocument(false);
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, ProhibitDtd = false }; //Common.DeclareXmlReaderSettings(false);
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

            if (nodes == null || nodes.Count == 0)
            {
                nodes = xmlDocument.SelectNodes("//span[@class='headword']", namespaceManager);
            }
            //nodes = xmlDocument.SelectNodes("//span[@class='headword']", namespaceManager);
            if (nodes != null && nodes.Count == 0)
            {
                // not in this file - this might be scripture?
                nodes = xmlDocument.SelectNodes("//xhtml:span[@class='scrBookName']", namespaceManager);
                if (nodes == null || nodes.Count == 0)
                {
                    nodes = xmlDocument.SelectNodes("//span[@class='scrBookName']", namespaceManager);
                }
                //nodes = xmlDocument.SelectNodes("//span[@class='scrBookName']", namespaceManager);
                if (nodes != null && nodes.Count > 0)
                    _inputType = "scripture";
            }
            else
            {
                _inputType = "dictionary";
            }
        }

        #endregion

        #region Book ID and Name
        /// <summary>
        /// Returns a book ID to be used in the .opf file. This is similar to the GetBookName call, but here
        /// we're wanting something that (1) doesn't start with a numeric value and (2) is unique.
        /// </summary>
        /// <param name="xhtmlFileName"></param>
        /// <returns></returns>
        private string GetBookID(string xhtmlFileName)
        {
            try
            {
                XmlDocument xmlDocument = Common.DeclareXMLDocument(false);
                XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
                namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
                XmlReaderSettings xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, ProhibitDtd = false }; //Common.DeclareXmlReaderSettings(false);
                XmlReader xmlReader = XmlReader.Create(xhtmlFileName, xmlReaderSettings);
                xmlDocument.Load(xmlReader);
                xmlReader.Close();
                // should only be one of these after splitting out the chapters.
                XmlNodeList nodes;
                if (_inputType.Equals("dictionary"))
                {
                    nodes = xmlDocument.SelectNodes("//xhtml:div[@class='letter']", namespaceManager);
                }
                else
                {
                    // no scrBookName - use Title_Main
                    nodes = xmlDocument.SelectNodes("//xhtml:div[@class='Title_Main']", namespaceManager);
                    if (nodes == null || nodes.Count == 0)
                    {
                        // start out with the book code (e.g., 2CH for 2 Chronicles)
                        nodes = xmlDocument.SelectNodes("//xhtml:span[@class='scrBookCode']", namespaceManager);
                    }
                    if (nodes == null || nodes.Count == 0)
                    {
                        // no book code - use scrBookName
                        nodes = xmlDocument.SelectNodes("//xhtml:span[@class='scrBookName']", namespaceManager);
                    }
                }
                if (nodes != null && nodes.Count > 0)
                {
                    var sb = new StringBuilder();
                    // just in case the name starts with a number, prepend "id"
                    sb.Append("id");
                    // remove any whitespace in the node text (the ID can't have it)
                    //sb.Append(new Regex(@"\s*").Replace(nodes[0].InnerText, string.Empty));
                    sb.Append("boooknode");
                    return (sb.ToString());
                }
                // fall back on just the file name
                return Path.GetFileName(xhtmlFileName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                if (ex.StackTrace != null)
                {
                    Debug.WriteLine(ex.StackTrace);
                }
                return Path.GetFileName(xhtmlFileName);
            }
        }

        /// <summary>
        /// Returns the user-friendly book name inside this file.
        /// </summary>
        /// <param name="xhtmlFileName">Split xhtml filename in the form PartFile[#]_.xhtml</param>
        /// <returns>User-friendly book name (value of the scrBookName or letter element in the xhtml file).</returns>
        private string GetBookName(string xhtmlFileName)
        {
            var fileNoPath = Path.GetFileName(xhtmlFileName);
            if (fileNoPath != null && fileNoPath.StartsWith(PreExportProcess.CoverPageFilename.Substring(0, 8)))
            {
                return ("Cover Page");
            }
            if (fileNoPath != null && fileNoPath.StartsWith(PreExportProcess.TitlePageFilename.Substring(0, 8)))
            {
                return ("Title Page");
            }
            if (fileNoPath != null && fileNoPath.StartsWith(PreExportProcess.TableOfContentsFilename.Substring(0, 8)))
            {
                return ("Table of Content");
            }
            if (fileNoPath != null && fileNoPath.StartsWith(PreExportProcess.CopyrightPageFilename.Substring(0, 8)))
            {
                return ("Copyright Information");
            }
            XmlDocument xmlDocument = Common.DeclareXMLDocument(false);
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, ProhibitDtd = false }; //Common.DeclareXmlReaderSettings(false);
            XmlReader xmlReader = XmlReader.Create(xhtmlFileName, xmlReaderSettings);
            xmlDocument.Load(xmlReader);
            xmlReader.Close();
            // should only be one of these after splitting out the chapters.
            XmlNodeList nodes;
            if (_inputType.Equals("dictionary"))
            {
                nodes = xmlDocument.SelectNodes("//xhtml:div[@class='letter']", namespaceManager);
                if (nodes == null || nodes.Count == 0)
                {
                    nodes = xmlDocument.SelectNodes("//div[@class='letter']", namespaceManager);
                }
            }
            else
            {
                nodes = xmlDocument.SelectNodes("//xhtml:span[@class='scrBookName']", namespaceManager);
                if (nodes == null || nodes.Count == 0)
                {
                    nodes = xmlDocument.SelectNodes("//span[@class='scrBookName']", namespaceManager);
                }
                //nodes = xmlDocument.SelectNodes("//xhtml:div[@class='Title_Main']", namespaceManager);
                if (nodes == null || nodes.Count == 0)
                {
                    // nothing there - check on the Title_Main span
                    nodes = xmlDocument.SelectNodes("//xhtml:div[@class='Title_Main']", namespaceManager);
                    // nothing there - check on the scrBookName span
                    //nodes = xmlDocument.SelectNodes("//xhtml:span[@class='scrBookName']", namespaceManager);
                    if (nodes == null || nodes.Count == 0)
                    {
                        nodes = xmlDocument.SelectNodes("//div[@class='Title_Main']", namespaceManager);
                    }
                }
                if (nodes == null || nodes.Count == 0)
                {
                    // we're really scraping the bottom - check on the scrBookCode span
                    nodes = xmlDocument.SelectNodes("//xhtml:span[@class='scrBookCode']", namespaceManager);
                    if (nodes == null || nodes.Count == 0)
                    {
                        nodes = xmlDocument.SelectNodes("//span[@class='scrBookCode']", namespaceManager);
                    }
                }
            }
            if (nodes != null && nodes.Count > 0)
            {
                var sb = new StringBuilder();
                sb.Append(nodes[0].InnerText);
                return (sb.ToString());
            }
            // fall back on just the file name
            return Path.GetFileName(xhtmlFileName);
        }
        #endregion

        #region Relative Hyperlink processing
        /// <summary>
        /// Returns the list of "broken" relative hyperlink hrefs in the given file (i.e.,
        /// relative hyperlinks that don't have a target within the file). This can happen when
        /// the xhtml file gets split out into multiple pieces, and the target for an href ends up
        /// in a different file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private List<string> FindBrokenRelativeHrefIds(string filePath)
        {
            if (!File.Exists(filePath)) return null;
            const string searchText = "a href=\"#"; // denotes a relative href
            var brokenRelativeHrefIds = new List<string>();
            var reader = new StreamReader(filePath);
            var content = reader.ReadToEnd();
            reader.Close();
            int start = content.IndexOf(searchText, 0);
            if (start != -1)
            {
                start += searchText.Length;
            }
            int stop = 0;
            while (start != -1)
            {
                // next instance of a relative hyperlink ref - read until the closing quote
                stop = (content.IndexOf("\"", start) - start);
                if (stop == -1) { break; }
                var hrefID = content.Substring(start, (stop));
                // not found -- this link is broken
                if (!brokenRelativeHrefIds.Contains(hrefID))
                {
                    brokenRelativeHrefIds.Add(hrefID);
                }
                start = content.IndexOf(searchText, (start + stop));
                if (start != -1)
                {
                    start += searchText.Length;
                }
            }
            return brokenRelativeHrefIds;
        }

        private void FixRelativeHyperlinks(string contentFolder, InProcess inProcess)
        {
            string[] files = Directory.GetFiles(contentFolder, "PartFile*.xhtml");
            string[] revFiles = Directory.GetFiles(contentFolder, "RevIndex*.xhtml");
            //inProcess.AddToMaximum(files.Length);
            PreExportProcess preExport = new PreExportProcess();
            var dictHyperlinks = new Dictionary<string, string>();
            List<string> sourceList = new List<string>();
            List<string> targetList = new List<string>();
            List<string> targettempList = new List<string>();
            Dictionary<string, string> fileDict = new Dictionary<string, string>();

            foreach (string targetFile in files)
            {
                preExport.GetReferenceList(targetFile, sourceList, targettempList);

                targetList.AddRange(targettempList);
                foreach (string target in targettempList)
                {
                    fileDict[target] = Path.GetFileName(targetFile);
                }
                targettempList.Clear();
            }

            foreach (string target in targetList)
            {
                if (sourceList.Contains(target) && !dictHyperlinks.ContainsKey(target))
                {
                    dictHyperlinks.Add(target, fileDict[target] + "#" + target);
                }
            }

            if (dictHyperlinks.Count > 0)
            {
                foreach (string targetFile in files)
                {
                    RemoveSpanVerseNumberNodeInXhtmlFile(targetFile);
                    ReplaceAllBrokenHrefs(targetFile, dictHyperlinks);
                }
                foreach (string targetFile in revFiles)
                {
                    RemoveSpanVerseNumberNodeInXhtmlFile(targetFile);
                    ReplaceAllBrokenHrefs(targetFile, dictHyperlinks);
                }
            }
            else
            {
                if (files.Length > 0)
                {
                    foreach (string targetFile in files)
                    {
                        RemoveSpanVerseNumberNodeInXhtmlFile(targetFile);
                    }
                }
                if (revFiles.Length > 0)
                {
                    foreach (string targetFile in revFiles)
                    {
                        RemoveSpanVerseNumberNodeInXhtmlFile(targetFile);
                    }
                }
            }
        }

        /// <summary>
        /// When the preprocessed xhtml gets split out into chunks the epub reader can understand, the
        /// targets for the relative links within the xhtml file might get moved into a separate file. This method
        /// locates each relative link within the content folder and replaces it with an absolute one (i.e., one containing
        /// the filename and id anchor).
        /// </summary>
        /// <param name="contentFolder">Folder containing the xhtml to be parsed</param>
        /// <param name="inProcess">progress bar (this is a potentially long-running process)</param>
        private void FixRelativeHyperlinks_OLD(string contentFolder, InProcess inProcess)
        {
            string[] files = Directory.GetFiles(contentFolder, "PartFile*.xhtml");
            inProcess.AddToMaximum(files.Length);
            var sbID = new StringBuilder();
            var books = new StringBuilder();
#if (TIME_IT)
            var startTime = DateTime.Now;
#endif
            bool bFound = false;
            int item = 0;
            var dictHyperlinks = new Dictionary<string, string>();
            foreach (string sourceFile in files)
            {

                RemoveSpanVerseNumberNodeInXhtmlFile(sourceFile);

                var relativeIDs = FindBrokenRelativeHrefIds(sourceFile);
                item = 0;
                dictHyperlinks.Clear();
                foreach (var relativeID in relativeIDs)
                {
                    item++;
                    inProcess.SetStatus(sourceFile + ": updating hyperlinks (" + item + " of " + relativeIDs.Count + ")");
                    // Try 1: Footnotes and cross-references -
                    // Footnotes and cross-references are the most common relative hyperlinks. Before going through the 
                    // trouble of searching through all the files, see if the target ended up in our references file.
                    if (References.Contains("End") && _inputType == "scripture")
                    {
                        if (IsStringInFile(Path.Combine(contentFolder, ReferencesFilename), relativeID))
                        {
                            dictHyperlinks.Add(relativeID, ReferencesFilename + "#" + relativeID);
                            continue;
                        }
                    }
                    //    find [id="<n>] in the xhtml file list
                    sbID.Length = 0; // reset the stringbuilder
                    sbID.Append("id=\"");
                    sbID.Append(relativeID);
                    sbID.Append("\"");
                    bFound = false;
                    // Try 2: localize the search to the split books (e.g., PartFile0002_01, _02, _03 ...)
                    int split = Path.GetFileName(sourceFile).IndexOf("_");
                    books.Length = 0;
                    if (split > 0)
                    {
                        books.Append(Path.GetFileName(sourceFile).Substring(0, Path.GetFileName(sourceFile).IndexOf("_")));
                        books.Append("*.xhtml");
                    }
                    string[] bookFiles = Directory.GetFiles(contentFolder, books.ToString());
                    foreach (var targetBookFile in bookFiles)
                    {
                        if (IsStringInFile(targetBookFile, sbID.ToString()))
                        {
                            dictHyperlinks.Add(relativeID, Path.GetFileName(targetBookFile) + "#" + relativeID);
                            bFound = true;
                            break;
                        }
                    }
                    if (!bFound)
                    {
                        // Try 3: the entire content folder
                        // Not found in the local (split) book. Try casting a wider net -- look at all the xhtml files in the directory
                        foreach (var targetFile in files)
                        {
                            if (IsStringInFile(targetFile, sbID.ToString()))
                            {
                                dictHyperlinks.Add(relativeID, Path.GetFileName(targetFile) + "#" + relativeID);
                                bFound = true;
                                break;
                            }
                        }
                    }
                    if (bFound == false)
                    {
                        // Still not found -- give up
                        Debug.WriteLine(">> Target ID for " + relativeID + " not found - this link is broken!");
                    }
                }
                // We've found the targets for all the relative hyperlinks that aren't in the current file
                // (and possibly some broken ones as well). Now update the targets for the file.
                if (dictHyperlinks.Count > 0)
                {
                    ReplaceAllBrokenHrefs(sourceFile, dictHyperlinks);
                }

                //Find & replcae the filename of the target chapter number



                // show our progress
                inProcess.PerformStep();
            }
#if (TIME_IT)
            TimeSpan tsTotal = DateTime.Now - startTime;
            Debug.WriteLine("Exportepub: time spent in FixRelativeHyperlinks: " + tsTotal);
#endif
        }

        private void RemoveSpanVerseNumberNodeInXhtmlFile(string fileName)
        {
            //Removed NoteTargetReference tag from XHTML file
            XmlDocument xDoc = Common.DeclareXMLDocument(true);
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xDoc.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            xDoc.Load(fileName);
            XmlElement elmRoot = xDoc.DocumentElement;

            //If includeImage is false, removes the img -> parent tag

            string xPath = "//xhtml:div[@class='scrBook']/xhtml:span[@class='Verse_Number']";
            if (elmRoot != null)
            {
                XmlNodeList divNode = elmRoot.SelectNodes(xPath, namespaceManager);
                if (divNode.Count > 0)
                {
                    for (int i = 0; i < divNode.Count; i++)
                    {
                        //referenceNode[i].RemoveChild(referenceNode[i].FirstChild);
                        var parentNode = divNode[i].ParentNode;
                        if (parentNode != null)
                            parentNode.RemoveChild(divNode[i]);
                    }
                }
            }

            xDoc.Save(fileName);
        }

        private void ReplaceAllBrokenHrefs(string filePath, Dictionary<string, string> dictHyperlinks)
        {
            if (!File.Exists(filePath)) return;
            var reader = new StreamReader(filePath);
            string content = reader.ReadToEnd();
            reader.Close();
            var contentWriter = new StringBuilder();
            string searchText = "a href=\"#";
            string newValue;
            int startIndex = 0;
            int stopIndex = 0;
            int nextIndex = 0;
            bool done = false;
            while (!done)
            {
                nextIndex = content.IndexOf(searchText, startIndex);
                if (nextIndex >= 0)
                {
                    // find the href target
                    stopIndex = content.IndexOf("\"", nextIndex + searchText.Length);
                    var target = content.Substring(nextIndex + searchText.Length, stopIndex - (nextIndex + searchText.Length));
                    // is it in our dictionary?
                    if (dictHyperlinks.TryGetValue(target, out newValue))
                    {
                        // yes - write the corrected text to the output file
                        contentWriter.Append(content.Substring(startIndex, nextIndex - startIndex));
                        contentWriter.Append("a href=\"");
                        contentWriter.Append(newValue);
                        //contentWriter.Append("\"");
                    }
                    else
                    {
                        // no - write out the existing text to the output file
                        contentWriter.Append(content.Substring(startIndex, stopIndex - startIndex));
                    }
                    // update startIndex
                    startIndex = stopIndex;
                }
                else
                {
                    // no more relative hyperlinks
                    contentWriter.Append(content.Substring(startIndex, content.Length - startIndex));
                    done = true;
                }
            }
            var writer = new StreamWriter(filePath);
            writer.Write(contentWriter);
            writer.Close();

            /*
            if (!File.Exists(filePath)) return false;
            string searchText = "a href=\"#";
            bool foundString = false;
            var reader = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4080, false);
            var writer = new FileStream(filePath + ".tmp", FileMode.Create);
            int next;
            while ((next = reader.ReadByte()) != -1)
            {
                byte b = (byte)next;
                if (b == searchText[0]) // first char in search text?
                {
                    // yes - searchText.Length chars into a buffer and compare them
                    int len = searchText.Length;
                    long pos = reader.Position;
                    byte[] buf = new byte[len];
                    buf[0] = b;
                    if (reader.Read(buf, 1, (len - 1)) == -1)
                    {
                        // reached the end of file - write out what we hit and jump out of the while loop
                        //                        writer.Write(new string(buf));
                        continue;
                    }
                    string data = Encoding.UTF8.GetString(buf);
                    if (String.Compare(searchText, data, true) == 0)
                    {
                        // found an instance of our search text - now run to the end of the href target quote
                        foundString = true;
                        Byte[] bytes = Encoding.UTF8.GetBytes(replaceText);
                        writer.Write(bytes, 0, bytes.Length);
                    }
                    else
                    {
                        // not what we're looking for - just write it out
                        reader.Position = pos;
                        writer.WriteByte(b);
                    }
                    data = null;
                }
                else // not what we're looking for - just write it out
                {
                    writer.WriteByte(b);
                }
            }
            reader.Close();
            reader.Dispose();
            writer.Close();
            writer.Dispose();
            // replace the original file with the new one
            if (foundString)
            {
                // at least one instance of the string was found - replace
                File.Copy(filePath + ".tmp", filePath, true);
            }
            // delete the temp file
            File.Delete(filePath + ".tmp");
             */
        }
        #endregion

        #region Image processing
        /// <summary>
        /// This method handles the images for the .epub file. Each image is resized and renamed (to .png) if necessary, then
        /// copied to the .epub folder. Any references to the image files from the .xhtml are also updated if needed.
        /// </summary>
        /// <param name="tempFolder"></param>
        /// <param name="contentFolder"></param>
        private void ProcessImages(string tempFolder, string contentFolder)
        {
            string[] imageFiles = Directory.GetFiles(tempFolder);
            bool renamedImages = false;
            Image image;
            foreach (string file in imageFiles)
            {
                switch (Path.GetExtension(file) == null ? "" : Path.GetExtension(file).ToLower())
                {
                    case ".jpg":
                    case ".jpeg":
                    case ".gif":
                    case ".png":
                        // .epub supports this image format - just copy the thing over
                        string name = Path.GetFileName(file);
                        string dest = Common.PathCombine(contentFolder, name);
                        // sanity check - if the image is gigantic, scale it
                        image = Image.FromFile(file);
                        if (image.Width > MaxImageWidth)
                        {
                            // need to scale image
                            var img = ResizeImage(image);
                            var extension = Path.GetExtension(file);
                            if (extension != null)
                                switch (extension.ToLower())
                                {
                                    case ".jpg":
                                    case ".jpeg":
                                        img.Save(dest, System.Drawing.Imaging.ImageFormat.Jpeg);
                                        break;
                                    case ".gif":
                                        img.Save(dest, System.Drawing.Imaging.ImageFormat.Gif);
                                        break;
                                    default:
                                        img.Save(dest, System.Drawing.Imaging.ImageFormat.Png);
                                        break;
                                }
                        }
                        else
                        {
                            File.Copy(file, dest);
                        }
                        break;
                    case ".bmp":
                    case ".tif":
                    case ".tiff":
                    case ".ico":
                    case ".wmf":
                    case ".pcx":
                    case ".cgm":
                        // TE (and others?) support these file types, but .epub doesn't -
                        // convert them to .png if we can
                        var imageName = Path.GetFileNameWithoutExtension(file) + ".png";
                        using (var fileStream = new FileStream(Common.PathCombine(contentFolder, imageName), FileMode.CreateNew))
                        {
                            image = Image.FromFile(file);
                            if (image.Width > MaxImageWidth)
                            {
                                var img = ResizeImage(image);
                                img.Save(fileStream, System.Drawing.Imaging.ImageFormat.Png);
                            }
                            else
                            {
                                image.Save(fileStream, System.Drawing.Imaging.ImageFormat.Png);
                            }
                        }
                        renamedImages = true;
                        break;
                    default:
                        // not an image file (or not one we recognize) - skip
                        break;
                }
            }
            // be sure to clean up any hyperlink references to the old file types
            if (renamedImages)
            {
                CleanupImageReferences(contentFolder);
            }
        }

        /// <summary>
        /// Resizes the given image down to MaxImageWidth pixels and returns the result.
        /// </summary>
        /// <param name="image">File to resize</param>
        private Image ResizeImage(Image image)
        {
            float nPercent = ((float)MaxImageWidth / (float)image.Width);
            int destW = (int)(image.Width * nPercent);
            int destH = (int)(image.Height * nPercent);
            var b = new Bitmap(destW, destH);
            var g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(image, 0, 0, destW, destH);
            g.Dispose();
            return (Image)b;
        }

        /// <summary>
        /// The .epub format doesn't support all image file types; when we copied the image files over, we had
        /// to convert the unsupported file types to .png. Here we'll do a search/replace for all references to
        /// the old versions.
        /// </summary>
        /// <param name="contentFolder">OEBPS folder containing all the xhtml files we need to clean up</param>
        private void CleanupImageReferences(string contentFolder)
        {
            string[] files = Directory.GetFiles(contentFolder, "*.xhtml");
            foreach (string file in files)
            {
                // using a streaming approach to reduce the memory footprint of this method
                // (we had Regex.Replace before, but it was using >100MB of data on larger dictionaries)
                var reader = new StreamReader(file);
                var writer = new StreamWriter(file + ".tmp");
                Int32 next;
                while ((next = reader.Read()) != -1)
                {
                    char b = (char)next;
                    if (b == '.') // found a period - is it a filename extension that we need to change?
                    {
                        // copy the period and the next 3 characters into a string
                        int len = 4;
                        char[] buf = new char[len];
                        buf[0] = b;
                        reader.Read(buf, 1, 3);
                        string data = new string(buf);
                        // is this an unsupported filename extension?
                        switch (data)
                        {
                            case ".bmp":
                            case ".ico":
                            case ".wmf":
                            case ".pcx":
                            case ".cgm":
                                // yes - replace with ".png"
                                writer.Write(".png");
                                break;
                            case ".tif":
                                // yes, but this could be either ".tif" or ".tiff" -
                                // find out which one by peeking at the next character
                                int nextchar = reader.Peek();
                                if (((char)nextchar) == 'f')
                                {
                                    // ".tiff" case
                                    reader.Read(); // move the reader up one position (consume the "f")
                                    // replace with ".png"
                                    writer.Write(".png");
                                }
                                else
                                {
                                    // ".tif" case - replace it with ".png"
                                    writer.Write(".png");
                                }
                                break;
                            default:
                                // not an unsupported extension - just write the data we collected
                                writer.Write(data);
                                break;
                        }
                    }
                    else // not a "."
                    {
                        writer.Write((char)next);
                    }
                }
                reader.Close();
                writer.Close();
                // replace the original file with the new one
                File.Delete(file);
                File.Move((file + ".tmp"), file);
            }
        }
        #endregion

        #region File Processing Methods
        /// <summary>
        /// Returns true if the specified search text string is found in the given file.
        /// Modified to use the Knuth-Morris-Pratt search algorithm 
        /// (http://en.wikipedia.org/wiki/Knuth%E2%80%93Morris%E2%80%93Pratt_algorithm)
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        private bool IsStringInFile(string filePath, string searchText)
        {

            try
            {
                //XmlTextReader _reader = new XmlTextReader(filePath)
                //{
                //    XmlResolver = null,
                //    WhitespaceHandling = WhitespaceHandling.Significant
                //};
                XmlTextReader _reader = Common.DeclareXmlTextReader(filePath, true);
                while (_reader.Read())
                {
                    if (_reader.NodeType == XmlNodeType.Element)
                    {
                        // bool found = Regex.IsMatch(_reader.ToString(), "<a\\shref.*?>");
                        string idString = searchText.Replace("id=\"", "").Replace("\"", "");
                        if (_reader.Name == "div" || _reader.Name == "span")
                        {

                            string id = _reader.GetAttribute("id");
                            if (id == idString)
                            {
                                _reader.Close();
                                return true;
                            }
                        }
                    }
                }
                _reader.Close();
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            //try
            //{

            //if (!File.Exists(filePath)) return false;
            //var reader = new FileStream(filePath, FileMode.Open);
            //byte[] buffer = new byte[reader.Length];
            //int[] T = new int[searchText.Length];
            //reader.Read(buffer, 0, (int) reader.Length);
            //reader.Close();
            //int j=0, i=2, sub=0;
            //// populate the T table
            //T[0] = -1;
            //T[1] = 0;
            //while (i < searchText.Length)
            //{
            //    if (searchText[i-1] == searchText[sub])
            //    {
            //        // continued substring
            //        sub++;
            //        T[i] = sub;
            //        i++;
            //    }
            //    else if (sub > 0)
            //    {
            //        // substring stops, fall back to the start
            //        sub = T[sub];
            //    }
            //    else
            //    {
            //        // not in a substring; use the default value
            //        T[i] = 0;
            //        i++;
            //    }
            //}

            //// the actual search
            //i = 0;
            //while (j+i < buffer.Length)
            //{
            //    if (searchText[i] == buffer[j+i])
            //    {
            //        if (i == searchText.Length - 1)
            //        {
            //            // match found
            //            return true;
            //        }
            //        i++;
            //    }
            //    else
            //    {
            //        // no match - move to next search pattern
            //        j = j + i - T[i];
            //        if (T[i] > -1)
            //        {
            //            i = T[i];
            //        }
            //        else
            //        {
            //            i = 0;
            //        }
            //    }
            //}

            //}
            //catch
            //{ }

            // return false;
            //////////////////
            /*
            int next;
            while ((next = reader.ReadByte()) != -1)
            {
                byte b = (byte)next;
                if (b == searchText[0]) // first char in search text?
                {
                    // yes - searchText.Length chars into a buffer and compare them
                    int len = searchText.Length;
                    long pos = reader.Position;
                    byte[] buf = new byte[len];
                    buf[0] = b;
                    if (reader.Read(buf, 1, (len - 1)) == -1)
                    {
                        // reached the end of file - write out what we hit and jump out of the while loop
                        //                        writer.Write(new string(buf));
                        continue;
                    }
                    string data = Encoding.UTF8.GetString(buf);
                    if (String.Compare(searchText, data, true) == 0)
                    {
                        // found an instance of our search text
                        reader.Close();
                        return true;
                    }
                    else
                    {
                        // not what we're looking for
                        reader.Position = pos;
                    }
                }
            }
            reader.Close();
            return false;
             */
        }

        /// <summary>
        /// Helper method to change the relative hyperlinks in the references file to absolute ones. 
        /// This is done after the scripture files are split out into individual books of 100K or less in size.
        /// </summary>
        /// <param name="contentFolder"></param>
        /// <param name="inProcess"></param>
        private void UpdateReferenceHyperlinks(string contentFolder, InProcess inProcess)
        {
            var outFilename = Path.Combine(contentFolder, ReferencesFilename);
            var hrefs = FindBrokenRelativeHrefIds(outFilename);
            //inProcess.AddToMaximum(hrefs.Count + 1);
            var reader = new StreamReader(outFilename);
            var content = new StringBuilder();
            content.Append(reader.ReadToEnd());
            reader.Close();
            string[] files = Directory.GetFiles(contentFolder, "PartFile*.xhtml");
            int index = 0;
            bool looped = false;
            foreach (var href in hrefs)
            {
                // find where the target is for this reference -
                // since the lists are sequential in the references file, we're using an index instead
                // of a foreach loop (so the search continues in the same file the last href left off on).
                while (true)
                {
                    // search the current file in the list
                    if (IsStringInFile(files[index], href))
                    {
                        content.Replace(("a href=\"#" + href + "\""),
                                        ("a href=\"" + Path.GetFileName(files[index]) + "#" + href + "\""));
                        break;
                    }
                    // update the index and try again
                    index++;
                    if (index == files.Length)
                    {
                        if (looped) break; // already searched through the list -- this item isn't found, get out
                        index = 0;
                        looped = true;
                    }
                }
                inProcess.PerformStep();
                looped = false;
            }
            var writer = new StreamWriter(outFilename);
            writer.Write(content);
            writer.Close();
            inProcess.PerformStep();
        }

        /// <summary>
        /// Helper method to change the relative hyperlinks in the references file to absolute ones. 
        /// This is done after the scripture files are split out into individual books of 100K or less in size.
        /// </summary>
        /// <param name="contentFolder"></param>
        /// <param name="inProcess"></param>
        private void UpdateReferenceSourcelinks(string contentFolder, InProcess inProcess)
        {
            //var hrefs = FindBrokenRelativeHrefIds(Path.Combine(contentFolder, "zzReferences.xhtml"));
            string[] files = Directory.GetFiles(contentFolder, "PartFile*.xhtml");
            foreach (var file in files)
            {
                XmlDocument xmlDocument = Common.DeclareXMLDocument(false);
                XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
                namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
                XmlReaderSettings xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, ProhibitDtd = false };
                XmlReader xmlReader = XmlReader.Create(file, xmlReaderSettings);
                xmlDocument.Load(xmlReader);
                xmlReader.Close();
                //var crossRefNodes = xmlDocument.SelectNodes("//xhtml:span[@class='Note_CrossHYPHENReference_Paragraph']", namespaceManager);
                var footnoteNodes = xmlDocument.SelectNodes("//xhtml:span[@class='Note_General_Paragraph']/xhtml:a", namespaceManager);
                if (footnoteNodes == null)
                {
                    return;
                }
                foreach (XmlNode footnoteNode in footnoteNodes)
                {
                    if (footnoteNode.Attributes != null)
                        footnoteNode.Attributes["href"].Value = "zzReferences.xhtml" + footnoteNode.Attributes["href"].Value;
                }

                footnoteNodes = xmlDocument.SelectNodes("//xhtml:span[@class='Note_CrossHYPHENReference_Paragraph']/xhtml:a", namespaceManager);
                if (footnoteNodes == null)
                {
                    return;
                }
                foreach (XmlNode footnoteNode in footnoteNodes)
                {
                    if (footnoteNode.Attributes != null)
                        footnoteNode.Attributes["href"].Value = "zzReferences.xhtml" + footnoteNode.Attributes["href"].Value;
                }

                xmlDocument.Save(file);
                inProcess.PerformStep();
            }
        }

        /// <summary>
        /// Creates a separate references file at the end of the xhtml files in scripture content, for both footnotes and cross-references.
        /// Each reference links back relatively to the source xhtml, so that the links can be updated when the content is split into
        /// smaller chunks.
        /// </summary>
        /// <param name="outputFolder"></param>
        /// <param name="xhtmlFileName"></param>
        private void CreateReferencesFile(string outputFolder, string xhtmlFileName)
        {
            // sanity check - return if the references are to be left in the text
            if (References.Contains("Section")) { return; }
            // collect all cross-references and footnotes in the content file
            XmlDocument xmlDocument = Common.DeclareXMLDocument(false);
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, ProhibitDtd = false }; //Common.DeclareXmlReaderSettings(false);
            XmlReader xmlReader = XmlReader.Create(xhtmlFileName, xmlReaderSettings);
            xmlDocument.Load(xmlReader);
            xmlReader.Close();
            // pick your nodes
            var crossRefNodes = xmlDocument.SelectNodes("//xhtml:span[@class='Note_CrossHYPHENReference_Paragraph']", namespaceManager);
            var footnoteNodes = xmlDocument.SelectNodes("//xhtml:span[@class='Note_General_Paragraph']", namespaceManager);
            if (crossRefNodes == null && footnoteNodes == null)
            {
                // nothing to pull out -- just exit
                return;
            }
            // file preamble
            var sbPreamble = new StringBuilder();
            sbPreamble.Append("<?xml version='1.0' encoding='utf-8'?><!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Strict//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd'[]>");
            sbPreamble.Append("<html xmlns='http://www.w3.org/1999/xhtml'><head><title>");
            //sbPreamble.Append(projInfo.ProjectName);
            sbPreamble.AppendLine("</title><link rel='stylesheet' href='book.css' type='text/css' /></head>");
            sbPreamble.Append("<body class='scrBody'><div class='Front_Matter'>");
            var outFilename = Path.Combine(outputFolder, ReferencesFilename);
            var outFile = new StreamWriter(outFilename);
            outFile.WriteLine(sbPreamble.ToString());
            // iterate through the files and pull out each reference hyperlink
            if (footnoteNodes != null && footnoteNodes.Count > 0)
            {
                outFile.WriteLine("<h1>Endnotes</h1>");
                outFile.WriteLine("<ul>");
                foreach (XmlNode footnoteNode in footnoteNodes)
                {
                    outFile.Write("<li id=\"FN_");
                    try
                    {
                        outFile.Write(footnoteNode.Attributes["id"].Value);
                    }
                    catch (NullReferenceException e)
                    {
                        Debug.WriteLine(e);
                    }
                    outFile.Write("\"><a href=\"");
                    outFile.Write("#");
                    try
                    {
                        outFile.Write(footnoteNode.Attributes["id"].Value);
                    }
                    catch (NullReferenceException e)
                    {
                        Debug.WriteLine(e);
                    }
                    outFile.Write("\">[");
                    try
                    {
                        outFile.Write(footnoteNode.Attributes["title"].Value);
                    }
                    catch (NullReferenceException e)
                    {
                        Debug.WriteLine(e);
                    }
                    outFile.Write("] ");
                    outFile.Write("</a> ");
                    //XmlNode bookNode = footnoteNode.SelectSingleNode("preceding::xhtml:div[@class='Title_Main'][1]", namespaceManager);
                    XmlNode bookNode = footnoteNode.SelectSingleNode("preceding::xhtml:span[@class='scrBookName'][1]", namespaceManager);
                    if (bookNode != null)
                    {
                        outFile.Write("<span class=\"BookName\"><b>" + bookNode.InnerText + "</b></span>");
                    }
                    outFile.Write(" ");
                    //XmlNode chapterNode = footnoteNode.SelectSingleNode("preceding::xhtml:span[@class='Chapter_Number'][1]", namespaceManager);
                    //if (chapterNode != null)
                    //{
                    //    outFile.Write(chapterNode.InnerText);
                    //}
                    //outFile.Write(":");
                    //XmlNode verseNode = footnoteNode.SelectSingleNode("preceding::xhtml:span[@class='Verse_Number'][1]", namespaceManager);
                    //if (verseNode != null)
                    //{
                    //    outFile.Write(verseNode.InnerText);
                    //}
                    //outFile.Write("</a> ");
                    outFile.Write(CleanupSpans(footnoteNode.InnerXml));
                    outFile.WriteLine("</li>");
                }
                outFile.WriteLine("</ul>");
            }
            if (crossRefNodes != null && crossRefNodes.Count > 0)
            {
                outFile.WriteLine("<h1>References</h1>");
                outFile.WriteLine("<ul>");
                foreach (XmlNode crossRefNode in crossRefNodes)
                {
                    outFile.Write("<li id=\"FN_");
                    try
                    {
                        outFile.Write(crossRefNode.Attributes["id"].Value);
                    }
                    catch (NullReferenceException e)
                    {
                        Debug.WriteLine(e);
                    }
                    outFile.Write("\"><a href=\"");
                    outFile.Write("#");
                    try
                    {
                        outFile.Write(crossRefNode.Attributes["id"].Value);
                    }
                    catch (NullReferenceException e)
                    {
                        Debug.WriteLine(e);
                    }
                    outFile.Write("\">");
                    XmlNode bookNode = crossRefNode.SelectSingleNode("preceding::xhtml:span[@class='Title_Main'][1]", namespaceManager);
                    if (bookNode != null)
                    {
                        outFile.Write(bookNode.InnerText);
                    }
                    outFile.Write(" ");
                    XmlNode chapterNode = crossRefNode.SelectSingleNode("preceding::xhtml:span[@class='Chapter_Number'][1]", namespaceManager);
                    if (chapterNode != null)
                    {
                        outFile.Write(chapterNode.InnerText);
                    }
                    outFile.Write(":");
                    XmlNode verseNode = crossRefNode.SelectSingleNode("preceding::xhtml:span[@class='Verse_Number'][1]", namespaceManager);
                    if (verseNode != null)
                    {
                        outFile.Write(verseNode.InnerText);
                    }
                    outFile.Write("</a> ");
                    outFile.Write(CleanupSpans(crossRefNode.InnerXml));
                    outFile.WriteLine("</li>");
                }
                outFile.WriteLine("</ul>");
            }

            outFile.WriteLine("</div></body></html>");
            outFile.Flush();
            outFile.Close();
        }

        private string CleanupSpans(string text)
        {
            var sb = new StringBuilder(text);
            sb.Replace(" lang", " xml:lang");
            sb.Replace("xmlns=\"http://www.w3.org/1999/xhtml\"", "");
            return sb.ToString();
        }

        /// <summary>
        /// Splits the specified xhtml file out into multiple files, either based on letter (dictionary) or book (scripture). 
        /// This method was adapted from ExportOpenOffice.cs.
        /// </summary>
        /// <param name="temporaryCvFullName"></param>
        /// <param name="pubInfo"></param>
        /// <returns></returns>
        private List<string> SplitFile(string temporaryCvFullName, PublicationInformation pubInfo)
        {
            List<string> fileNameWithPath = new List<string>();
            if (_inputType.Equals("dictionary"))
            {
                fileNameWithPath = Common.SplitXhtmlFile(temporaryCvFullName, "letHead", true);
            }
            else
            {
                fileNameWithPath = Common.SplitXhtmlFile(temporaryCvFullName, "scrBook", false);
            }
            return fileNameWithPath;
        }

        /// <summary>
        /// Splits a book file into smaller files, based on file size.
        /// </summary>
        /// <param name="xhtmlFilename">file to split into smaller pieces</param>
        /// <returns></returns>
        private List<string> SplitBook(string xhtmlFilename)
        {
            const long maxSize = 204800; // 200KB
            // sanity check - make sure the file exists
            if (!File.Exists(xhtmlFilename))
            {
                return null;
            }
            List<string> fileNames = new List<string>();
            // is it worth splitting this file?
            FileInfo fi = new FileInfo(xhtmlFilename);
            if (fi.Length <= maxSize)
            {
                // not worth splitting this file - just return it
                fileNames.Add(xhtmlFilename);
                return fileNames;
            }

            // If we got here, it's worth our time to split the file out.
            StreamWriter writer;
            var reader = new StreamReader(xhtmlFilename);
            string content = reader.ReadToEnd();
            reader.Close();

            string bookcode = "<span class=\"scrBookCode\">" + GetBookID(xhtmlFilename) + "</span>";
            string head = content.Substring(0, content.IndexOf("<body"));
            bool done = false;
            int startIndex = 0;
            int fileIndex = 1;
            int softMax = 0, realMax = 0;
            var sb = new StringBuilder();
            while (!done)
            {
                // look for a good breaking point after our soft maximum size
                string outFile = Path.Combine(Path.GetDirectoryName(xhtmlFilename), (Path.GetFileNameWithoutExtension(xhtmlFilename) + fileIndex.ToString().PadLeft(2, '0') + ".xhtml"));
                softMax = startIndex + (int)(maxSize / 2); // UTF-16
                if (softMax > content.Length)
                {
                    realMax = -1;
                }
                else
                {
                    if (_inputType == "scripture")
                    {
                        // scripture - find the next section head; this will be our break point
                        realMax = content.IndexOf("<div class=\"Section_Head", softMax);
                    }
                    else
                    {
                        // dictionary - find the next entry; this will be our break point
                        realMax = content.IndexOf("<div class=\"entry", softMax);
                    }
                }
                if (realMax == -1)
                {
                    if (startIndex == 0)
                    {
                        // can't split this file (no section breaks after the soft limit) - just return it
                        fileNames.Add(xhtmlFilename);
                        return fileNames;
                    }
                    // no more section heads - just pull in the rest of the content
                    // write out head + substring(startIndex to the end)
                    sb.Append(head);
                    if (_inputType == "scripture")
                    {
                        sb.Append("<body class=\"scrBody\"><div class=\"scrBook\">");
                        sb.Append(bookcode);
                    }
                    else
                    {
                        sb.Append("<body class=\"dicBody\"><div class=\"letData\">");
                    }

                    sb.AppendLine(content.Substring(startIndex));
                    writer = new StreamWriter(outFile);
                    writer.Write(sb.ToString());
                    writer.Close();
                    // add this file to fileNames)))
                    fileNames.Add(outFile);
                    break;
                }
                // build the content
                if (startIndex == 0)
                {
                    // for the first section, we go from the start of the file to realMax
                    sb.Append(content.Substring(0, (realMax - startIndex)));
                    sb.AppendLine("</div></body></html>"); // close out the xhtml
                }
                else
                {
                    // for the subsequent sections, we need the head + the substring (startIndex to realMax)
                    sb.Append(head);
                    if (_inputType == "scripture")
                    {
                        sb.Append("<body class=\"scrBody\"><div class=\"scrBook\">");
                    }
                    else
                    {
                        sb.Append("<body class=\"dicBody\"><div class=\"letData\">");
                    }
                    sb.Append(content.Substring(startIndex, (realMax - startIndex)));
                    sb.AppendLine("</div></body></html>"); // close out the xhtml
                }
                // write the string buffer content out to file
                writer = new StreamWriter(outFile);
                writer.Write(sb.ToString());
                writer.Close();
                // add this file to fileNames
                fileNames.Add(outFile);
                // move the indices up for the next file chunk
                startIndex = realMax;
                // reset the stringbuilder
                sb.Length = 0;
                fileIndex++;
            }
            // return the result
            return fileNames;
        }

        /// <summary>
        /// Copies the selected source folder and its subdirectories to the destination folder path. 
        /// This is a recursive method.
        /// </summary>
        /// <param name="sourceFolder"></param>
        /// <param name="destFolder"></param>
        private void CopyFolder(string sourceFolder, string destFolder)
        {
            if (Directory.Exists(destFolder))
            {
                Directory.Delete(destFolder, true);
            }
            Directory.CreateDirectory(destFolder);
            string[] files = Directory.GetFiles(sourceFolder);
            try
            {
                foreach (string file in files)
                {
                    string name = Path.GetFileName(file);
                    string dest = Common.PathCombine(destFolder, name);
                    // Special processing for the mimetype file - don't copy it now; copy it after
                    // compressing the rest of the archive (in Compress() below) as a stored / not compressed
                    // file in the archive. This is keeping in line with the .epub OEBPS Container Format (OCF)
                    // recommendations: http://www.idpf.org/ocf/ocf1.0/download/ocf10.htm.
                    if (name != "mimetype")
                    {
                        File.Copy(file, dest);
                    }
                }

                string[] folders = Directory.GetDirectories(sourceFolder);
                foreach (string folder in folders)
                {
                    string name = Path.GetFileName(folder);
                    string dest = Common.PathCombine(destFolder, name);
                    if (name != ".svn")
                    {
                        CopyFolder(folder, dest);
                    }
                }
            }
            catch
            {
                return;
            }
        }

        /// <summary>
        /// Compresses the selected folder's contents and saves the archive in the specified outputPath
        /// with the extension .epub.
        /// </summary>
        /// <param name="sourceFolder">Folder to compress</param>
        /// <param name="outputPath">Output path and filename (without extension)</param>
        private void Compress(string sourceFolder, string outputPath)
        {
            var mODT = new ZipFolder();
            string outputPathWithFileName = outputPath + ".epub";

            // add the content to the existing epub.zip file
            string zipFile = Path.Combine(sourceFolder, "epub.zip");
            string contentFolder = Path.Combine(sourceFolder, "OEBPS");
            string[] files = Directory.GetFiles(contentFolder);
            mODT.AddToZip(files, zipFile);
            var sb = new StringBuilder();
            sb.Append(sourceFolder);
            sb.Append(Path.DirectorySeparatorChar);
            sb.Append("META-INF");
            sb.Append(Path.DirectorySeparatorChar);
            sb.Append("container.xml");
            var containerFile = new string[1] { sb.ToString() };
            mODT.AddToZip(containerFile, zipFile);
            // copy the results to the output directory
            File.Copy(zipFile, outputPathWithFileName, true);
        }

        #endregion

        #region EPUB metadata handlers

        /// <summary>
        /// Generates the manifest and metadata information file used by the .epub reader
        /// (content.opf). For more information, refer to <see cref="http://www.idpf.org/doc_library/epub/OPF_2.0.1_draft.htm#Section2.0"/> 
        /// </summary>
        /// <param name="projInfo">Project information</param>
        /// <param name="contentFolder">Content folder (.../OEBPS)</param>
        /// <param name="bookId">Unique identifier for the book we're generating.</param>
        private void CreateOpf(PublicationInformation projInfo, string contentFolder, Guid bookId)
        {
            XmlWriter opf = XmlWriter.Create(Common.PathCombine(contentFolder, "content.opf"));
            opf.WriteStartDocument();
            // package name
            opf.WriteStartElement("package", "http://www.idpf.org/2007/opf");
            opf.WriteAttributeString("version", "2.0");
            opf.WriteAttributeString("unique-identifier", "BookId");
            // metadata - items defined by the Dublin Core Metadata Initiative:
            // (http://dublincore.org/documents/2004/12/20/dces/)
            opf.WriteStartElement("metadata");
            opf.WriteAttributeString("xmlns", "dc", null, "http://purl.org/dc/elements/1.1/");
            opf.WriteAttributeString("xmlns", "opf", null, "http://www.idpf.org/2007/opf");
            opf.WriteElementString("dc", "title", null,
                                   (Title == "") ? (Common.databaseName + " " + projInfo.ProjectName) : Title);
            opf.WriteStartElement("dc", "creator", null); //<dc:creator opf:role="aut">[author]</dc:creator>
            opf.WriteAttributeString("opf", "role", null, "aut");
            opf.WriteValue((Creator == "") ? Environment.UserName : Creator);
            opf.WriteEndElement();
            if (_inputType == "dictionary")
            {
                opf.WriteElementString("dc", "subject", null, "Reference");
            }
            else
            {
                opf.WriteElementString("dc", "subject", null, "Religion & Spirituality");
            }
            if (Description.Length > 0)
                opf.WriteElementString("dc", "description", null, Description);
            if (Publisher.Length > 0)
                opf.WriteElementString("dc", "publisher", null, Publisher);
            opf.WriteStartElement("dc", "contributor", null); // authoring program as a "contributor", e.g.:
            opf.WriteAttributeString("opf", "role", null, "bkp");
            // <dc:contributor opf:role="bkp">FieldWorks 7</dc:contributor>
            opf.WriteValue(Common.GetProductName());
            opf.WriteEndElement();
            opf.WriteElementString("dc", "date", null, DateTime.Today.ToString("yyyy-MM-dd"));
            // .epub standard date format (http://www.idpf.org/2007/opf/OPF_2.0_final_spec.html#Section2.2.7)
            opf.WriteElementString("dc", "type", null, "Text"); // 
            if (Format.Length > 0)
                opf.WriteElementString("dc", "format", null, Format);
            if (Source.Length > 0)
                opf.WriteElementString("dc", "source", null, Source);

            if (_langFontDictionary.Count == 0)
            {
                opf.WriteElementString("dc", "language", null, "en");
            }

            foreach (var lang in _langFontDictionary.Keys)
            {
                opf.WriteElementString("dc", "language", null, lang);
            }


            if (Coverage.Length > 0)
                opf.WriteElementString("dc", "coverage", null, Coverage);
            if (Rights.Length > 0)
                opf.WriteElementString("dc", "rights", null, Rights);
            opf.WriteStartElement("dc", "identifier", null); // <dc:identifier id="BookId">[guid]</dc:identifier>
            opf.WriteAttributeString("id", "BookId");
            opf.WriteValue(bookId.ToString());
            opf.WriteEndElement();
            // cover image (optional)
            if (Param.GetMetadataValue(Param.CoverPage).ToLower().Equals("true"))
            {
                opf.WriteStartElement("meta");
                opf.WriteAttributeString("name", "cover");
                opf.WriteAttributeString("content", "cover-image");
                opf.WriteEndElement(); // meta
            }
            opf.WriteEndElement(); // metadata
            // manifest
            opf.WriteStartElement("manifest");
            // (individual "item" elements in the manifest)
            opf.WriteStartElement("item");
            opf.WriteAttributeString("id", "ncx");
            opf.WriteAttributeString("href", "toc.ncx");
            opf.WriteAttributeString("media-type", "application/x-dtbncx+xml");
            opf.WriteEndElement(); // item

            if (EmbedFonts)
            {
                int fontNum = 1;
                foreach (var embeddedFont in _embeddedFonts.Values)
                {
                    if (embeddedFont.Filename == null)
                    {
                        // already written out that this font doesn't exist in the CSS file; just skip it here
                        continue;
                    }
                    opf.WriteStartElement("item"); // item (charis embedded font)
                    opf.WriteAttributeString("id", "epub.embedded.font" + fontNum);
                    opf.WriteAttributeString("href", Path.GetFileName(embeddedFont.Filename));
                    opf.WriteAttributeString("media-type", "font/opentype/");
                    opf.WriteEndElement(); // item
                    fontNum++;
                    if (IncludeFontVariants)
                    {
                        // italic
                        if (embeddedFont.HasItalic && embeddedFont.Filename.CompareTo(embeddedFont.ItalicFilename) != 0)
                        {
                            opf.WriteStartElement("item"); // item (charis embedded font)
                            opf.WriteAttributeString("id", "epub.embedded.font_i_" + fontNum);
                            opf.WriteAttributeString("href", Path.GetFileName(embeddedFont.ItalicFilename));
                            opf.WriteAttributeString("media-type", "font/opentype/");
                            opf.WriteEndElement(); // item
                            fontNum++;
                        }
                        // bold
                        if (embeddedFont.HasBold && embeddedFont.Filename.CompareTo(embeddedFont.BoldFilename) != 0)
                        {
                            opf.WriteStartElement("item"); // item (charis embedded font)
                            opf.WriteAttributeString("id", "epub.embedded.font_b_" + fontNum);
                            opf.WriteAttributeString("href", Path.GetFileName(embeddedFont.BoldFilename));
                            opf.WriteAttributeString("media-type", "font/opentype/");
                            opf.WriteEndElement(); // item
                            fontNum++;
                        }
                    }
                }
            }
            List<string> _listIdRef = new List<string>();
            int counterSet = 1;
            string idRefValue = string.Empty;
            // now add the xhtml files to the manifest
            string[] files = Directory.GetFiles(contentFolder);
            foreach (string file in files)
            {
                // iterate through the file set and add <item> elements for each xhtml file
                string name = Path.GetFileName(file);
                string nameNoExt = Path.GetFileNameWithoutExtension(file);

                if (name.EndsWith(".xhtml"))
                {
                    // is this the cover page?
                    if (name.StartsWith(PreExportProcess.CoverPageFilename.Substring(0, 8)))
                    {
                        // yup - write it out and go to the next item
                        opf.WriteStartElement("item");
                        opf.WriteAttributeString("id", "cover");
                        opf.WriteAttributeString("href", name);
                        opf.WriteAttributeString("media-type", "application/xhtml+xml");
                        opf.WriteEndElement(); // item
                        continue;
                    }

                    // if we can, write out the "user friendly" book name in the TOC
                    string fileId = GetBookID(file);

                    if (_listIdRef.Contains(fileId))
                    {
                        _listIdRef.Add(fileId + counterSet.ToString());
                        idRefValue = fileId + counterSet.ToString();
                        counterSet++;
                    }
                    else
                    {
                        _listIdRef.Add(fileId);
                        idRefValue = fileId;
                    }

                    opf.WriteStartElement("item");
                    if (_inputType == "dictionary")
                    {
                        // the book ID can be wacky (and non-unique) for dictionaries. Just use the filename.
                        opf.WriteAttributeString("id", nameNoExt);
                    }
                    else
                    {
                        // scripture - use the book ID
                        opf.WriteAttributeString("id", idRefValue);
                    }
                    opf.WriteAttributeString("href", name);
                    opf.WriteAttributeString("media-type", "application/xhtml+xml");
                    opf.WriteEndElement(); // item
                }
                else if (name.EndsWith(".css"))
                {
                    opf.WriteStartElement("item"); // item (stylesheet)
                    opf.WriteAttributeString("id", "stylesheet");
                    opf.WriteAttributeString("href", name);
                    opf.WriteAttributeString("media-type", "text/css");
                    opf.WriteEndElement(); // item
                }
                else if (name.ToLower().EndsWith(".jpg") || name.ToLower().EndsWith(".jpeg"))
                {
                    opf.WriteStartElement("item"); // item (image)
                    opf.WriteAttributeString("id", "image" + nameNoExt);
                    opf.WriteAttributeString("href", name);
                    opf.WriteAttributeString("media-type", "image/jpeg");
                    opf.WriteEndElement(); // item
                }
                else if (name.ToLower().EndsWith(".gif"))
                {
                    opf.WriteStartElement("item"); // item (image)
                    opf.WriteAttributeString("id", "image" + nameNoExt);
                    opf.WriteAttributeString("href", name);
                    opf.WriteAttributeString("media-type", "image/gif");
                    opf.WriteEndElement(); // item
                }
                else if (name.ToLower().EndsWith(".png"))
                {
                    opf.WriteStartElement("item"); // item (image)
                    opf.WriteAttributeString("id", "image" + nameNoExt);
                    opf.WriteAttributeString("href", name);
                    opf.WriteAttributeString("media-type", "image/png");
                    opf.WriteEndElement(); // item
                }
            }
            opf.WriteEndElement(); // manifest
            // spine
            opf.WriteStartElement("spine");
            opf.WriteAttributeString("toc", "ncx");
            // a couple items for the cover image
            if (Param.GetMetadataValue(Param.CoverPage).ToLower().Equals("true"))
            {
                opf.WriteStartElement("itemref");
                opf.WriteAttributeString("idref", "cover");
                opf.WriteAttributeString("linear", "yes");
                opf.WriteEndElement(); // itemref
            }

            _listIdRef = new List<string>();
            counterSet = 1;
            idRefValue = string.Empty;
            foreach (string file in files)
            {
                // is this the cover page?
                if (Path.GetFileName(file).StartsWith(PreExportProcess.CoverPageFilename.Substring(0, 8)))
                {
                    continue;
                }
                // add an <itemref> for each xhtml file in the set
                string name = Path.GetFileName(file);
                if (name.EndsWith(".xhtml"))
                {
                    string fileId = GetBookID(file);
                    if (_listIdRef.Contains(fileId))
                    {
                        _listIdRef.Add(fileId + counterSet.ToString());
                        idRefValue = fileId + counterSet.ToString();
                        counterSet++;
                    }
                    else
                    {
                        _listIdRef.Add(fileId);
                        idRefValue = fileId;
                    }


                    opf.WriteStartElement("itemref"); // item (stylesheet)
                    if (_inputType == "dictionary")
                    {
                        // the book ID can be wacky (and non-unique) for dictionaries. Just use the filename.
                        opf.WriteAttributeString("idref", Path.GetFileNameWithoutExtension(file));
                    }
                    else
                    {
                        // scripture - use the book ID
                        opf.WriteAttributeString("idref", idRefValue);
                    }
                    opf.WriteEndElement(); // itemref
                }
            }
            opf.WriteEndElement(); // spine
            // guide
            opf.WriteStartElement("guide");
            // cover image
            if (Param.GetMetadataValue(Param.CoverPage).Trim().Equals("True"))
            {
                opf.WriteStartElement("reference");
                opf.WriteAttributeString("href", "File0Cvr00000_.xhtml");
                opf.WriteAttributeString("type", "cover");
                opf.WriteAttributeString("title", "Cover");
                opf.WriteEndElement(); // reference
            }
            // first xhtml filename
            opf.WriteStartElement("reference");
            opf.WriteAttributeString("type", "text");
            opf.WriteAttributeString("title", Common.databaseName + " " + projInfo.ProjectName);
            int index = 0;
            while (index < files.Length)
            {
                if (files[index].EndsWith(".xhtml"))
                {
                    break;
                }
                index++;
            }
            if (index == files.Length) index--; // edge case
            opf.WriteAttributeString("href", Path.GetFileName(files[index]));
            opf.WriteEndElement(); // reference
            opf.WriteEndElement(); // guide
            opf.WriteEndElement(); // package
            opf.WriteEndDocument();
            opf.Close();
        }

        /// <summary>
        /// Creates the table of contents file used by .epub readers (toc.ncx).
        /// </summary>
        /// <param name="projInfo">project information</param>
        /// <param name="contentFolder">the content folder (../OEBPS)</param>
        /// <param name="bookId">Unique identifier for the book we're creating</param>
        private void CreateNcx(PublicationInformation projInfo, string contentFolder, Guid bookId)
        {
            // toc.ncx
            string tocFullPath = Common.PathCombine(contentFolder, "toc.ncx");
            XmlWriter ncx = XmlWriter.Create(tocFullPath);
            ncx.WriteStartDocument();
            ncx.WriteStartElement("ncx", "http://www.daisy.org/z3986/2005/ncx/");
            ncx.WriteAttributeString("version", "2005-1");
            ncx.WriteStartElement("head");
            ncx.WriteStartElement("meta");
            ncx.WriteAttributeString("name", "dtb:uid");
            ncx.WriteAttributeString("content", bookId.ToString());
            ncx.WriteEndElement(); // meta
            ncx.WriteStartElement("meta");
            ncx.WriteAttributeString("name", "epub-creator");
            ncx.WriteAttributeString("content", Common.GetProductName());
            ncx.WriteEndElement(); // meta
            ncx.WriteStartElement("meta");
            ncx.WriteAttributeString("name", "dtb:depth");
            ncx.WriteAttributeString("content", "1");
            ncx.WriteEndElement(); // meta
            ncx.WriteStartElement("meta");
            ncx.WriteAttributeString("name", "dtb:totalPageCount");
            ncx.WriteAttributeString("content", "0"); // TODO: (is this possible?)
            ncx.WriteEndElement(); // meta
            ncx.WriteStartElement("meta");
            ncx.WriteAttributeString("name", "dtb:maxPageNumber");
            ncx.WriteAttributeString("content", "0"); // TODO: is this info available?
            ncx.WriteEndElement(); // meta
            ncx.WriteEndElement(); // head
            ncx.WriteStartElement("docTitle");
            ncx.WriteElementString("text", projInfo.ProjectName);
            ncx.WriteEndElement(); // docTitle
            ncx.WriteStartElement("navMap");
            // individual navpoint elements (one for each xhtml)
            string[] files = Directory.GetFiles(contentFolder, "*.xhtml");
            bool RevIndex = false;
            bool isMainOpen = false;
            bool isMainSubOpen = false;
            bool isRevOpen = false;
            bool isRevSubOpen = false;
            bool isScriptureSubOpen = false;
            int index = 1;
            int chapNum = 1;
            bool needsEnd = false;
            bool skipChapterInfo = TocLevel.StartsWith("1");
            string scripFileName = string.Empty;
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                string bookName = GetBookName(file);
                if (name.IndexOf("File") == 0 && name.IndexOf("TOC") == -1)
                {
                    WriteNavPoint(ncx, index.ToString(), bookName, name);
                    index++;
                    // chapters within the books (nested as a subhead)
                    chapNum = 1;
                    if (!skipChapterInfo)
                    {
                        WriteChapterLinks(file, ref index, ncx, ref chapNum);
                    }
                    // end the book's navPoint element
                    ncx.WriteEndElement(); // navPoint
                }
                else
                {
                    if (name.IndexOf("TOC") != -1)
                    {
                        WriteNavPoint(ncx, index.ToString(), bookName, name);
                        index++;
                    }
                    if (_inputType.ToLower() == "dictionary")
                    {
                        if (name.Contains("PartFile"))
                        {
                            if (!isMainOpen)
                            {
                                //WriteNavPoint(ncx, index.ToString(), "Main", name);
                                //index++;
                                isMainOpen = true;
                            }
                            if (Path.GetFileNameWithoutExtension(file).EndsWith("_") ||
                                Path.GetFileNameWithoutExtension(file).EndsWith("_01"))
                            {
                                if (isMainSubOpen)
                                {
                                    ncx.WriteEndElement(); // navPoint
                                }
                                WriteNavPoint(ncx, index.ToString(), bookName, name);
                                index++;
                                chapNum = 1;
                                isMainSubOpen = true;
                            }
                            if (!skipChapterInfo)
                            {
                                WriteChapterLinks(file, ref index, ncx, ref chapNum);
                            }
                            //ncx.WriteEndElement(); // navPoint
                        }
                        else if (name.Contains("RevIndex"))
                        {
                            if (isMainOpen)
                            {
                                //ncx.WriteEndElement(); // navPoint Main value
                                ncx.WriteEndElement(); // navPoint Main
                                isMainSubOpen = false;
                                isMainOpen = false;
                            }
                            if (Path.GetFileNameWithoutExtension(file).EndsWith("_") ||
                                Path.GetFileNameWithoutExtension(file).EndsWith("_01"))
                            {
                                if (isRevSubOpen)
                                {
                                    ncx.WriteEndElement(); // navPoint
                                }
                                if (!isRevOpen)
                                {
                                    //WriteNavPoint(ncx, index.ToString(), "Reversal Index", name);
                                    //index++;
                                    isRevOpen = true;
                                }
                                WriteNavPoint(ncx, index.ToString(), bookName, name);
                                index++;
                                chapNum = 1;
                                isRevSubOpen = true;
                            }
                            if (!skipChapterInfo)
                            {
                                WriteChapterLinks(file, ref index, ncx, ref chapNum);
                            }
                            //ncx.WriteEndElement(); // navPoint
                        }
                    }
                    else
                    {
                        if (name.IndexOf("TOC") == -1 &&
                            (Path.GetFileNameWithoutExtension(file).EndsWith("_") ||
                             Path.GetFileNameWithoutExtension(file).EndsWith("_01")))
                        {
                            if (isScriptureSubOpen)
                            {
                                ncx.WriteEndElement(); // navPoint
                            }
                            bookName = GetBookName(file);
                            ncx.WriteStartElement("navPoint");
                            ncx.WriteAttributeString("id", "dtb:uid");
                            ncx.WriteAttributeString("playOrder", index.ToString());
                            ncx.WriteStartElement("navLabel");
                            ncx.WriteElementString("text", bookName);
                            ncx.WriteEndElement(); // navlabel
                            ncx.WriteStartElement("content");
                            ncx.WriteAttributeString("src", name);
                            ncx.WriteEndElement(); // meta
                            index++;
                            // chapters within the books (nested as a subhead)
                            chapNum = 1;
                            if (!skipChapterInfo)
                            {
                                WriteChapterLinks(file, ref index, ncx, ref chapNum);
                            }
                            isScriptureSubOpen = true;
                        }
                        else if (name.IndexOf("zzReference") == 0)
                        {
                            if (isScriptureSubOpen)
                            {
                                ncx.WriteEndElement(); // navPoint
                            }
                            ncx.WriteStartElement("navPoint");
                            ncx.WriteAttributeString("id", "dtb:uid");
                            ncx.WriteAttributeString("playOrder", index.ToString());
                            ncx.WriteStartElement("navLabel");
                            ncx.WriteElementString("text", "End Notes");
                            ncx.WriteEndElement(); // navlabel
                            ncx.WriteStartElement("content");
                            ncx.WriteAttributeString("src", name);
                            ncx.WriteEndElement(); // meta
                            index++;
                            // chapters within the books (nested as a subhead)
                            chapNum = 1;
                            if (!skipChapterInfo)
                            {
                                WriteEndNoteLinks(file, ref index, ncx, ref chapNum);
                                //WriteChapterLinks(file, ref index, ncx, ref chapNum);
                            }
                            //isScriptureSubOpen = true;
                        }
                        else
                        {
                            if (!skipChapterInfo)
                            {
                                WriteChapterLinks(file, ref index, ncx, ref chapNum);
                            }
                        }
                    }

                }

            }
            if (isRevOpen && _inputType.ToLower() == "dictionary")
            {
                // end the book's navPoint element
                //ncx.WriteEndElement(); // navPoint Rev value
                ncx.WriteEndElement(); // navPoint TOC
                isRevOpen = false;
            }
            if (isScriptureSubOpen)
            {
                ncx.WriteEndElement(); // navPoint
            }
            ncx.WriteEndElement(); // navPoint TOC
            ncx.WriteEndElement(); // navmap
            //ncx.WriteEndElement(); // ncx
            ncx.WriteEndDocument();
            ncx.Close();
            FixPlayOrder(tocFullPath);
            if (_inputType.ToLower() == "dictionary")
            {
                ApplyXslt(tocFullPath, _addDicTocHeads);
            }
            if (!_isUnixOS)
            {
                ApplyXslt(tocFullPath, _fixEpubToc);
            }
        }

        private void FixPlayOrder(string tocFullPath)
        {
            // Renumber all PlayOrder attributes in order with no gaps.
            XmlTextReader reader = Common.DeclareXmlTextReader(tocFullPath, true);
            var tocDoc = new XmlDocument();
            tocDoc.Load(reader);
            reader.Close();
            var nodes = tocDoc.SelectNodes("//@playOrder");
            Debug.Assert(nodes != null);
            int n = 1;
            foreach (XmlAttribute node in nodes)
            {
                node.InnerText = n.ToString();
                n += 1;
            }
            FileStream xmlFile = new FileStream(tocFullPath, FileMode.Create);
            XmlWriter writer = XmlWriter.Create(xmlFile);
            tocDoc.Save(writer);
            xmlFile.Close();
        }

        private void ApplyXslt(string fileFullPath, XslCompiledTransform xslt)
        {
            var folder = Path.GetDirectoryName(fileFullPath);
            var name = Path.GetFileNameWithoutExtension(fileFullPath);
            var tempFullName = Path.Combine(folder, name) + "-1.xml";
            File.Copy(fileFullPath, tempFullName);

            XmlTextReader reader = Common.DeclareXmlTextReader(tempFullName, true);
            FileStream xmlFile = new FileStream(fileFullPath, FileMode.Create);
            XmlWriter writer = XmlWriter.Create(xmlFile, xslt.OutputSettings);
            xslt.Transform(reader, null, writer, null);
            xmlFile.Close();
            reader.Close();

            File.Delete(tempFullName);
        }

        private void WriteNavPoint(XmlWriter ncx, string index, string text, string name)
        {
            ncx.WriteStartElement("navPoint");
            ncx.WriteAttributeString("id", "dtb:uid");
            ncx.WriteAttributeString("playOrder", index);
            ncx.WriteStartElement("navLabel");
            ncx.WriteElementString("text", text);
            ncx.WriteEndElement(); // navlabel
            ncx.WriteStartElement("content");
            ncx.WriteAttributeString("src", name);
            ncx.WriteEndElement(); // meta
        }

        /// <summary>
        /// Creates the table of contents file used by .epub readers (toc.ncx).
        /// </summary>
        /// <param name="projInfo">project information</param>
        /// <param name="contentFolder">the content folder (../OEBPS)</param>
        /// <param name="bookId">Unique identifier for the book we're creating</param>
        private void CreateNcx_old(PublicationInformation projInfo, string contentFolder, Guid bookId)
        {
            // toc.ncx
            XmlWriter ncx = XmlWriter.Create(Common.PathCombine(contentFolder, "toc.ncx"));
            ncx.WriteStartDocument();
            ncx.WriteStartElement("ncx", "http://www.daisy.org/z3986/2005/ncx/");
            ncx.WriteAttributeString("version", "2005-1");
            ncx.WriteStartElement("head");
            ncx.WriteStartElement("meta");
            ncx.WriteAttributeString("name", "dtb:uid");
            ncx.WriteAttributeString("content", bookId.ToString());
            ncx.WriteEndElement(); // meta
            ncx.WriteStartElement("meta");
            ncx.WriteAttributeString("name", "epub-creator");
            ncx.WriteAttributeString("content", Common.GetProductName());
            ncx.WriteEndElement(); // meta
            ncx.WriteStartElement("meta");
            ncx.WriteAttributeString("name", "dtb:depth");
            ncx.WriteAttributeString("content", "1");
            ncx.WriteEndElement(); // meta
            ncx.WriteStartElement("meta");
            ncx.WriteAttributeString("name", "dtb:totalPageCount");
            ncx.WriteAttributeString("content", "0"); // TODO: (is this possible?)
            ncx.WriteEndElement(); // meta
            ncx.WriteStartElement("meta");
            ncx.WriteAttributeString("name", "dtb:maxPageNumber");
            ncx.WriteAttributeString("content", "0"); // TODO: is this info available?
            ncx.WriteEndElement(); // meta
            ncx.WriteEndElement(); // head
            ncx.WriteStartElement("docTitle");
            ncx.WriteElementString("text", projInfo.ProjectName);
            ncx.WriteEndElement(); // docTitle
            ncx.WriteStartElement("navMap");
            // individual navpoint elements (one for each xhtml)
            string[] files = Directory.GetFiles(contentFolder, "*.xhtml");
            bool RevIndex = false;
            int index = 1;
            int chapNum = 1;
            bool needsEnd = false;
            bool skipChapterInfo = TocLevel.StartsWith("1");
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                // nest the reversal index entries
                if (name.Contains("RevIndex") && RevIndex == false)
                {
                    ncx.WriteStartElement("navPoint");
                    ncx.WriteAttributeString("id", "dtb:uid");
                    ncx.WriteAttributeString("playOrder", index.ToString());
                    ncx.WriteStartElement("navLabel");
                    ncx.WriteElementString("text", "Reversal Index");
                    ncx.WriteEndElement(); // navlabel
                    ncx.WriteStartElement("content");
                    ncx.WriteAttributeString("src", name + "#body");
                    ncx.WriteEndElement(); // meta
                    index++;
                    RevIndex = true;
                }
                if (name.Contains(ReferencesFilename))
                {
                    ncx.WriteStartElement("navPoint");
                    ncx.WriteAttributeString("id", "dtb:uid");
                    ncx.WriteAttributeString("playOrder", index.ToString());
                    ncx.WriteStartElement("navLabel");
                    ncx.WriteElementString("text", "References");
                    ncx.WriteEndElement(); // navlabel
                    ncx.WriteStartElement("content");
                    ncx.WriteAttributeString("src", name);
                    ncx.WriteEndElement(); // meta
                    index++;
                    RevIndex = true;
                }
                if (!Path.GetFileNameWithoutExtension(file).EndsWith("_"))
                {
                    // this is a split file - is it the first one?
                    if (Path.GetFileNameWithoutExtension(file).EndsWith("_01"))
                    {
                        // first chunk of a split file
                        if (needsEnd)
                        {
                            // end the last book's navPoint element
                            ncx.WriteEndElement(); // navPoint
                            needsEnd = false;
                        }
                        // start a new book entry, but don't end it
                        string bookName = GetBookName(file);
                        ncx.WriteStartElement("navPoint");
                        ncx.WriteAttributeString("id", "dtb:uid");
                        ncx.WriteAttributeString("playOrder", index.ToString());
                        ncx.WriteStartElement("navLabel");
                        ncx.WriteElementString("text", bookName);
                        ncx.WriteEndElement(); // navlabel
                        ncx.WriteStartElement("content");
                        ncx.WriteAttributeString("src", name);
                        ncx.WriteEndElement(); // meta
                        index++;
                        // chapters within the books (nested as a subhead)
                        chapNum = 1;
                        if (!skipChapterInfo)//If Book and Section selected
                        {
                            WriteChapterLinks(file, ref index, ncx, ref chapNum);
                        }
                        needsEnd = true;
                    }
                    else if (!skipChapterInfo)
                    {
                        // somewhere in the middle of a split file - just write out the chapter entries
                        WriteChapterLinks(file, ref index, ncx, ref chapNum);
                    }
                }
                else
                {
                    // no split in this file - write out the book and chapter stuff
                    if (needsEnd)
                    {
                        // end the book's navPoint element
                        ncx.WriteEndElement(); // navPoint
                        needsEnd = false;
                    }
                    string bookName = GetBookName(file);
                    ncx.WriteStartElement("navPoint");
                    ncx.WriteAttributeString("id", "dtb:uid");
                    ncx.WriteAttributeString("playOrder", index.ToString());
                    ncx.WriteStartElement("navLabel");
                    ncx.WriteElementString("text", bookName);
                    ncx.WriteEndElement(); // navlabel
                    ncx.WriteStartElement("content");
                    ncx.WriteAttributeString("src", name);
                    ncx.WriteEndElement(); // meta
                    index++;
                    // chapters within the books (nested as a subhead)
                    chapNum = 1;
                    if (!skipChapterInfo)
                    {
                        WriteChapterLinks(file, ref index, ncx, ref chapNum);
                    }
                    // end the book's navPoint element
                    ncx.WriteEndElement(); // navPoint
                }
            }
            // close out the reversal index entry if needed
            if (RevIndex)
            {
                ncx.WriteEndElement(); // navPoint
            }
            ncx.WriteEndElement(); // navmap
            ncx.WriteEndElement(); // ncx
            ncx.WriteEndDocument();
            ncx.Close();
        }

        /// <summary>
        /// Writes the chapter links out to the specified XmlWriter (the .ncx file).
        /// </summary>
        /// <returns>List of url strings</returns>
        private void WriteChapterLinks(string xhtmlFileName, ref int playOrder, XmlWriter ncx, ref int chapnum)
        {
            XmlDocument xmlDocument = Common.DeclareXMLDocument(true);
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, ProhibitDtd = false }; //Common.DeclareXmlReaderSettings(false);
            XmlReader xmlReader = XmlReader.Create(xhtmlFileName, xmlReaderSettings);
            xmlDocument.Load(xmlReader);
            xmlReader.Close();
            XmlNodeList nodes;
            bool isSectionHead = false, isChapterNumber = false, isVerseNumber = false;
            string sectionHeadRef = string.Empty;
            string sectionHead = string.Empty, fromChapterNumber = string.Empty, firstVerseNumber = string.Empty, lastVerseNumber = string.Empty;
            string formatString = string.Empty;
            if (_inputType.Equals("dictionary"))
            {
                nodes = xmlDocument.SelectNodes("//xhtml:div[@class='entry']", namespaceManager);
            }
            else
            {
                nodes = xmlDocument.SelectNodes("//xhtml:div[@class='scrBook']", namespaceManager);
            }

            if (nodes != null && nodes.Count > 0)
            {
                var sb = new StringBuilder();
                string name = Path.GetFileName(xhtmlFileName);
                foreach (XmlNode node in nodes)
                {
                    string textString = string.Empty;
                    if (_inputType.Equals("dictionary"))
                    {
                        sb.Append(name);
                        sb.Append("#");
                        XmlNode val = null;
                        if (node.Attributes != null && node.Attributes["id"] != null)
                        {
                            val = node.Attributes["id"];
                        }
                        //XmlNode val = node.Attributes["id"];
                        if (val != null)
                            sb.Append(val.Value);
                        //sb.Append(node.Attributes["id"].Value);

                        // for a dictionary, the headword / headword-minor is the label
                        if (!node.HasChildNodes)
                        {
                            // reset the stringbuilder
                            sb.Length = 0;
                            // This entry doesn't have any information - skip it
                            continue;
                        }
                        const string headwordXPath = ".//xhtml:span[@class='headword']";
                        XmlNode headwordNode = node.SelectSingleNode(headwordXPath, namespaceManager);
                        if (headwordNode != null)
                        {
                            textString = headwordNode.InnerText;
                        }
                        else
                        {
                            textString = node.FirstChild.InnerText;
                        }

                        if (textString.Trim().Length > 0)
                        {
                            // write out the node
                            ncx.WriteStartElement("navPoint");
                            ncx.WriteAttributeString("id", "dtb:uid");
                            ncx.WriteAttributeString("playOrder", playOrder.ToString());
                            ncx.WriteStartElement("navLabel");
                            ncx.WriteElementString("text", textString);
                            ncx.WriteEndElement(); // navlabel
                            ncx.WriteStartElement("content");
                            ncx.WriteAttributeString("src", sb.ToString());
                            ncx.WriteEndElement(); // meta
                            //ncx.WriteEndElement(); // meta
                            playOrder++;
                        }

                        // If this is a dictionary with TOC level 3, gather the senses for this entry
                        if (_inputType.Equals("dictionary") && TocLevel.StartsWith("3"))
                        {
                            // see if there are any senses to add to this entry
                            XmlNodeList childNodes = node.SelectNodes(".//xhtml:span[@class='sense']", namespaceManager);
                            if (childNodes != null)
                            {
                                sb.Length = 0;
                                foreach (XmlNode childNode in childNodes)
                                {
                                    // for a dictionary, the grammatical-info//partofspeech//span is the label
                                    if (!childNode.HasChildNodes)
                                    {
                                        // reset the stringbuilder
                                        sb.Length = 0;
                                        // This entry doesn't have any information - skip it
                                        continue;
                                    }

                                    if (childNode.HasChildNodes && childNode.FirstChild != null && childNode.FirstChild.FirstChild != null)
                                        textString = childNode.FirstChild.FirstChild.InnerText;
                                    sb.Append(name);
                                    sb.Append("#");
                                    if (childNode.Attributes != null && childNode.Attributes["id"] != null)
                                    {
                                        sb.Append(childNode.Attributes["id"].Value);
                                    }
                                    // write out the node
                                    ncx.WriteStartElement("navPoint");
                                    ncx.WriteAttributeString("id", "dtb:uid");
                                    ncx.WriteAttributeString("playOrder", playOrder.ToString());
                                    ncx.WriteStartElement("navLabel");
                                    ncx.WriteElementString("text", textString);
                                    ncx.WriteEndElement(); // navlabel
                                    ncx.WriteStartElement("content");
                                    ncx.WriteAttributeString("src", sb.ToString());
                                    ncx.WriteEndElement(); // meta
                                    ncx.WriteEndElement(); // navPoint
                                    // reset the stringbuilder
                                    sb.Length = 0;
                                    playOrder++;
                                }
                            }
                        }
                        if (textString.Trim().Length > 0)
                        {
                            ncx.WriteEndElement(); // navPoint
                        }
                    }
                    else // Scripture
                    {
                        using (XmlReader reader = XmlReader.Create(new StringReader(node.OuterXml)))
                        {
                            // Parse the file and display each of the nodes.
                            while (reader.Read())
                            {
                                switch (reader.NodeType)
                                {
                                    case XmlNodeType.Element:
                                        string className = reader.GetAttribute("class");
                                        if (className == "Section_Head")
                                        {
                                            if (fromChapterNumber == currentChapterNumber)
                                            {
                                                textString = textString + "-" + lastVerseNumber + ")";
                                            }
                                            else
                                            {
                                                textString = textString + "-" + currentChapterNumber + ":" +
                                                               lastVerseNumber + ")";
                                            }
                                            if (textString.Trim().Length > 4)
                                            {
                                                // write out the node
                                                ncx.WriteStartElement("navPoint");
                                                ncx.WriteAttributeString("id", "dtb:uid");
                                                ncx.WriteAttributeString("playOrder", playOrder.ToString());
                                                ncx.WriteStartElement("navLabel");
                                                ncx.WriteElementString("text", textString);
                                                ncx.WriteEndElement(); // navlabel
                                                ncx.WriteStartElement("content");
                                                ncx.WriteAttributeString("src", sb.ToString());
                                                ncx.WriteEndElement(); // meta
                                                //ncx.WriteEndElement(); // meta
                                                //ncx.WriteEndElement(); // navPoint
                                                playOrder++;
                                                sb.Length = 0;
                                            }
                                            if (reader.GetAttribute("id") != null)
                                            {
                                                sb.Append(name);
                                                sb.Append("#");
                                                sb.Append(reader.GetAttribute("id"));
                                            }
                                            if (textString.Trim().Length > 4)
                                            {
                                                ncx.WriteEndElement(); // navPoint
                                            }
                                            textString = string.Empty;
                                            firstVerseNumber = string.Empty;
                                            isSectionHead = true;
                                        }
                                        else if (className == "Chapter_Number")
                                        {
                                            isChapterNumber = true;
                                        }
                                        else if (className != null && className.IndexOf("Verse_Number") == 0)
                                        {
                                            isVerseNumber = true;
                                        }
                                        break;
                                    case XmlNodeType.Text:
                                        if (isSectionHead)
                                        {
                                            sectionHead = reader.Value;
                                            isSectionHead = false;
                                        }
                                        if (isChapterNumber)
                                        {
                                            currentChapterNumber = reader.Value;
                                            isChapterNumber = false;
                                        }
                                        if (isVerseNumber)
                                        {
                                            if (firstVerseNumber.Trim().Length == 0 && sectionHead.Length > 0)
                                            {
                                                firstVerseNumber = reader.Value;
                                                fromChapterNumber = currentChapterNumber;
                                                textString = sectionHead + "(" + currentChapterNumber + ":" + firstVerseNumber;
                                            }
                                            lastVerseNumber = reader.Value;
                                            isVerseNumber = false;
                                        }
                                        break;
                                    case XmlNodeType.XmlDeclaration:
                                    case XmlNodeType.ProcessingInstruction:
                                        break;
                                    case XmlNodeType.Comment:
                                        break;
                                    case XmlNodeType.EndElement:
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                    // reset the stringbuilder
                    sb.Length = 0;
                }
            }
        }


        protected void WriteEndNoteLinks(string xhtmlFileName, ref int playOrder, XmlWriter ncx, ref int chapnum)
        {
            XmlDocument xmlDocument = Common.DeclareXMLDocument(true);
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, ProhibitDtd = false }; //Common.DeclareXmlReaderSettings(false);
            XmlReader xmlReader = XmlReader.Create(xhtmlFileName, xmlReaderSettings);
            xmlDocument.Load(xmlReader);
            xmlReader.Close();
            XmlNodeList nodes;
            bool isanchor = false, isBookName = false, isNoteTargetReference = false, isList = false;
            string sectionHeadRef = string.Empty;
            string anchorValue = string.Empty, bookNameValue = string.Empty, noteTargetReferenceValue = string.Empty, ListValue = string.Empty;
            string formatString = string.Empty;
            nodes = xmlDocument.SelectNodes("//xhtml:li", namespaceManager);

            if (nodes != null && nodes.Count > 0)
            {
                var sb = new StringBuilder();
                string name = Path.GetFileName(xhtmlFileName);
                foreach (XmlNode node in nodes)
                {
                    string textString = string.Empty;
                    using (XmlReader reader = XmlReader.Create(new StringReader(node.OuterXml)))
                    {
                        // Parse the file and display each of the nodes.
                        while (reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    string className = reader.GetAttribute("class");
                                    if (reader.Name == "a")
                                    {
                                        isanchor = true;
                                    }
                                    else if (reader.Name == "li")
                                    {
                                        if (reader.GetAttribute("id") != null)
                                        {
                                            sb.Append(name);
                                            sb.Append("#");
                                            sb.Append(reader.GetAttribute("id"));
                                        }
                                        isList = true;
                                    }
                                    else if (className == "BookName")
                                    {
                                        isBookName = true;
                                    }
                                    else if (className == "Note_Target_Reference")
                                    {
                                        isNoteTargetReference = true;
                                    }
                                    break;
                                case XmlNodeType.Text:
                                    if (isanchor)
                                    {
                                        anchorValue = reader.Value;
                                        isanchor = false;
                                    }
                                    else if (isList)
                                    {
                                        //ListValue = reader.GetAttribute("id");
                                    }
                                    if (isBookName)
                                    {
                                        bookNameValue = reader.Value;
                                        isBookName = false;
                                    }
                                    if (isNoteTargetReference)
                                    {
                                        if (anchorValue.Trim().Length > 0 && bookNameValue.Trim().Length > 0)
                                        {
                                            textString = anchorValue + " " + bookNameValue + " " + reader.Value;
                                        }
                                        if (textString.Trim().Length > 0)
                                        {
                                            // write out the node
                                            ncx.WriteStartElement("navPoint");
                                            ncx.WriteAttributeString("id", "dtb:uid");
                                            ncx.WriteAttributeString("playOrder", playOrder.ToString());
                                            ncx.WriteStartElement("navLabel");
                                            ncx.WriteElementString("text", textString);
                                            ncx.WriteEndElement(); // navlabel
                                            ncx.WriteStartElement("content");
                                            ncx.WriteAttributeString("src", sb.ToString());
                                            ncx.WriteEndElement(); // meta
                                            //ncx.WriteEndElement(); // meta
                                            //ncx.WriteEndElement(); // navPoint
                                            playOrder++;
                                            sb.Length = 0;
                                        }

                                        if (textString.Trim().Length > 4)
                                        {
                                            ncx.WriteEndElement(); // navPoint
                                        }
                                        //noteTargetReferenceValue = reader.Value;
                                        isNoteTargetReference = false;
                                    }
                                    break;
                                case XmlNodeType.XmlDeclaration:
                                case XmlNodeType.ProcessingInstruction:
                                    break;
                                case XmlNodeType.Comment:
                                    break;
                                case XmlNodeType.EndElement:
                                    break;
                                default:
                                    break;
                            }
                        }

                    }
                    // reset the stringbuilder
                    sb.Length = 0;
                }
            }
        }


        protected void InsertChapterLinkBelowBookName(string contentFolder)
        {
            string[] files = Directory.GetFiles(contentFolder, "PartFile*.xhtml");
            List<string> chapterIdList = new List<string>();
            string fileName = string.Empty;
            XmlDocument xmlDocument = Common.DeclareXMLDocument(true);
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, ProhibitDtd = false };//Common.DeclareXmlReaderSettings(false);
            foreach (string sourceFile in files)
            {
                if (!File.Exists(sourceFile)) return;
                fileName = Path.GetFileName(sourceFile);
                XmlReader xmlReader = XmlReader.Create(sourceFile, xmlReaderSettings);
                xmlDocument.Load(xmlReader);
                xmlReader.Close();
                if (_inputType.Equals("scripture"))
                {
                    XmlNodeList nodes = xmlDocument.SelectNodes(".//xhtml:div[@class='Chapter_Number']", namespaceManager);
                    foreach (XmlNode chapterNode in nodes)
                    {
                        if (chapterNode.Attributes.Count > 0 && chapterNode.Attributes["id"] != null)
                        {
                            string value = fileName + "#" + chapterNode.Attributes["id"].Value;
                            if (!chapterIdList.Contains(value))
                                chapterIdList.Add(value);
                        }
                    }
                }
            }
            foreach (string sourceFile in files)
            {
                fileName = Path.GetFileNameWithoutExtension(sourceFile);
                if (fileName.LastIndexOf("_01") == fileName.Length - 3 || fileName.LastIndexOf("_") == fileName.Length - 1)
                {
                    string[] valueList1 = Path.GetFileNameWithoutExtension(sourceFile).Split('_');
                    if (OnlyOneChapter(chapterIdList, valueList1[0]))
                        continue;
                    XmlReader xmlReader = XmlReader.Create(sourceFile, xmlReaderSettings);
                    xmlDocument.Load(xmlReader);
                    xmlReader.Close();
                    const string xPath = ".//xhtml:div[@class='Title_Main']";
                    XmlNodeList nodes = xmlDocument.SelectNodes(xPath, namespaceManager);

                    if (nodes.Count > 0)
                    {
                        var next = nodes[0].NextSibling;
                        while (next.Attributes.GetNamedItem("class").InnerText.ToLower().Contains("title"))
                            next = next.NextSibling;
                        foreach (string VARIABLE in chapterIdList)
                        {
                            string[] valueList = VARIABLE.Split('_');
                            if (valueList[0] != valueList1[0])
                                continue;

                            XmlNode nodeContent = xmlDocument.CreateElement("a",
                                                                            xmlDocument.DocumentElement.NamespaceURI);
                            XmlAttribute attribute = xmlDocument.CreateAttribute("href");
                            attribute.Value = VARIABLE;
                            nodeContent.Attributes.Append(attribute);
                            nodeContent.InnerText = GetChapterNumber(VARIABLE);
                            next.ParentNode.InsertBefore(nodeContent, next);
                            XmlNode spaceNode = xmlDocument.CreateElement("span",
                                                                            xmlDocument.DocumentElement.NamespaceURI);
                            spaceNode.InnerText = " ";
                            next.ParentNode.InsertBefore(spaceNode, next);
                        }
                    }

                    xmlDocument.Save(sourceFile);
                }
            }
        }

        private bool OnlyOneChapter(List<string> chapterIdList, string s)
        {
            int count = 0;
            foreach (string s1 in chapterIdList)
            {
                var values = s1.Split('_');
                if (values[0] == s)
                    count += 1;
            }
            return count == 1;
        }

        private string GetChapterNumber(string value)
        {
            string[] valueList = value.Split('_');
            return valueList[valueList.Length - 1].ToLower().Replace("chapter", "");
        }

        #endregion

        #endregion
    }
}
