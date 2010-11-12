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
//   |-page-template.xpgt
//   |-<any fonts and other files embedded into the archive>
//   |-<list of files in book – xhtml format + .css for styling>
//   |-toc.ncx
//   |-toc.xhtml
//   `-images
//     `-<any images referenced in book files>
//
// </remarks>
// --------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using epubConvert;
using SIL.Tool;


namespace SIL.PublishingSolution
{
    public class Exportepub : IExportProcess 
    {

        protected string processFolder;
        protected string restructuredFullName;
        protected string outputPathBase;
        protected string outputNameBase;
        protected static ProgressBar _pb;
        private Dictionary<string, EmbeddedFont> _embeddedFonts;  // font information for this export
        private Dictionary<string, string> _langFontDictionary; // languages and font names in use for this export

//        protected static PostscriptLanguage _postscriptLanguage = new PostscriptLanguage();
        protected string _inputType;

        // property implementations
        public string Description { get; set; }
        public string Publisher { get; set; }
        public string Relation { get; set; }
        public string Coverage { get; set; }
        public string Rights { get; set; }
        public string Format { get; set; }
        public string Source { get; set; }
        public bool EmbedFonts { get; set; }


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
            bool returnValue = false;
            if (inputDataType.ToLower() == "dictionary" || inputDataType.ToLower() == "scripture")
            {
                returnValue = true;
            }
            return returnValue;
        }

        /// <summary>
        /// Entry point for epub converter
        /// </summary>
        /// <param name="projInfo">values passed including xhtml and css names</param>
        /// <returns>true if succeeds</returns>
        public bool Export(PublicationInformation projInfo)
        {
            bool success = true;
            _langFontDictionary = new Dictionary<string, string>();
            _embeddedFonts = new Dictionary<string, EmbeddedFont>();
            var myCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            var curdir = Environment.CurrentDirectory;
            if (projInfo == null)
            {
                // missing some vital information - error out
                success = false;
                Cursor.Current = myCursor;
            }
            else
            {
                // basic setup
                DateTime dt1 = DateTime.Now;    // time this thing
                var sb = new StringBuilder();
                Guid bookId = Guid.NewGuid(); // NOTE: this creates a new ID each time Pathway is run. 
                string outputFolder = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath); // finished .epub goes here
                PreExportProcess preProcessor = new PreExportProcess(projInfo);
                Environment.CurrentDirectory = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath);
                //_postscriptLanguage.SaveCache();
                // XHTML preprocessing
                 preProcessor.GetTempFolderPath();
                preProcessor.ImagePreprocess();
                preProcessor.ReplaceSlashToREVERSE_SOLIDUS();
                if (projInfo.SwapHeadword)
                {
                    preProcessor.SwapHeadWordAndReversalForm();
                }


                BuildLanguagesList(projInfo.DefaultXhtmlFileWithPath);
                // CSS preprocessing
                string tempFolder = Path.GetDirectoryName(preProcessor.ProcessedXhtml);

                // EDB 10/20/2010 - TD-1629
                // HACK: Currently the merged CSS file fails validation; this is causing Adobe's epub reader
                // to toss ALL formatting. I'm working around the issue for now by relying solely on the
                // epub.css file.
                // TODO: replace this with the merged CSS file block (below) when our merging process passes validation.
                string mergedCSS;
                // EDB - try not messing with the CSS file
                if (File.Exists(Path.Combine(outputFolder, "epub.css")))
                {
                    mergedCSS = Path.Combine(outputFolder, "epub.css");
                }
                else {
                    mergedCSS = projInfo.DefaultCssFileWithPath;
                }
//                string tempFolderName = Path.GetFileName(tempFolder);
//                var mc = new MergeCss { OutputLocation = tempFolderName };
//                string mergedCSS = mc.Make(projInfo.DefaultCssFileWithPath);
//                preProcessor.ReplaceStringInCss(mergedCSS);
//                preProcessor.SetDropCapInCSS(mergedCSS);
//                string defaultCSS = Path.GetFileName(mergedCSS);
                // rename the CSS file to something readable
//                string niceNameCSS = Path.Combine(Path.GetDirectoryName(mergedCSS), "book.css");
                // end EDB 10/20/2010

                string niceNameCSS = Path.Combine(tempFolder, "book.css");
                projInfo.DefaultCssFileWithPath = niceNameCSS;
                string defaultCSS = Path.GetFileName(niceNameCSS);
                if (File.Exists(niceNameCSS))
                {
                    File.Delete(niceNameCSS);
                }
                File.Move(mergedCSS, niceNameCSS);
                mergedCSS = niceNameCSS;
                Common.SetDefaultCSS(projInfo.DefaultXhtmlFileWithPath, defaultCSS);
                Common.SetDefaultCSS(preProcessor.ProcessedXhtml, defaultCSS);
                // pull in the style settings
                LoadPropertiesFromSettings(); 
                // transform the XHTML content with our XSLT. Currently this does the following:
                // - strips out "lang" tags from <span> elements (.epub doesn't like them there)
                // - strips out <meta> tags (.epub chokes on the filename IIRC.  TODO: verify the problem here)
                // - adds an "id" in each Chapter_Number span, so we can link to it from the TOC
                string cvFileName = Path.GetFileNameWithoutExtension(preProcessor.ProcessedXhtml) + "_cv";
                string xsltFullName = Common.FromRegistry("TE_XHTML-to-epub_XHTML.xslt");
//                string temporaryCvFullName = Common.PathCombine(tempFolder, cvFileName + ".xhtml");
                restructuredFullName = Path.Combine(outputFolder, cvFileName + ".xhtml");

                // EDB 10/22/2010
                // HACK: we need the preprocessed image file names (preprocessor.imageprocess()), but
                // it's missing the xml namespace that makes it a valid xhtml file. We'll add it here.
                // (The unprocessed html works fine, but doesn't have the updated links to the image files in it, 
                // so we can't use it.)
                // TODO: remove this line when TE provides valid XHTML output.
                //
                // EDB 10/29/2010 FWR-2697 - remove when fixed in FLEx
                Common.ReplaceInFile(preProcessor.ProcessedXhtml, "<LexSense_VariantFormEntryBackRefs", "<span class='LexSense_VariantFormEntryBackRefs'");
                Common.ReplaceInFile(preProcessor.ProcessedXhtml, "<LexSense_RefsFrom_LexReference_Targets", "<span class='LexSense_RefsFrom_LexReference_Targets'");
                Common.ReplaceInFile(preProcessor.ProcessedXhtml, "</LexSense_VariantFormEntryBackRefs", "</span");
                Common.ReplaceInFile(preProcessor.ProcessedXhtml, "</LexSense_RefsFrom_LexReference_Targets", "</span");
                // end EDB 10/29/2010
                Common.ReplaceInFile(preProcessor.ProcessedXhtml, "<html", "<html xmlns='http://www.w3.org/1999/xhtml'");
                // end EDB 10/22/2010

                // split the .XHTML into multiple files, as specified by the user
                List<string> htmlFiles = new List<string>();
                List<string> splitFiles = new List<string>();
                if (projInfo.FileToProduce.ToLower() != "one")
                {
                    splitFiles = SplitFile(preProcessor.ProcessedXhtml, projInfo);
                }
                else
                {
                    splitFiles.Add(preProcessor.ProcessedXhtml);
                }

                foreach (string file in splitFiles)
                {
                    if (File.Exists(file))
                    {
                        Common.XsltProcess(file, xsltFullName, "_cv.xhtml");
                        // add this file to the html files list
                        htmlFiles.Add(Path.Combine(Path.GetDirectoryName(file), (Path.GetFileNameWithoutExtension(file) + "_cv.xhtml")));
                        // clean up the un-transformed file
                        File.Delete(file);
                    }
                }
                //// split the .XHTML into multiple files, as specified by the user
                //List<string> htmlFiles = new List<string>();
                //if (projInfo.FileToProduce.ToLower() != "one")
                //{
                //    htmlFiles = SplitFile(temporaryCvFullName, projInfo);
                //}
                //else
                //{
                //    htmlFiles.Add(temporaryCvFullName);
                //}
                // create the "epub" directory structure and copy over the boilerplate files
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

                // Font handling
                // First, get the list of fonts used in this project
                BuildFontsList();
                // If we're embedding the fonts, handle the non-SIL ones now
                if (EmbedFonts)
                {
                    var nonSILFonts = new Dictionary<EmbeddedFont, string>();
                    string langs;
                    // Build the list of non-SIL fonts in use
                    foreach (var embeddedFont in _embeddedFonts)
                    {
                        if (!embeddedFont.Value.SILFont)
                        {
                            // append or add this entry to nonSILFonts
                            if (nonSILFonts.TryGetValue(embeddedFont.Value, out langs))
                            {
                                // existing entry - add this language to the list of langs that use this font
                                var sbName = new StringBuilder();
                                sbName.Append(langs);
                                sbName.Append(", ");
                                sbName.Append(embeddedFont.Key);
                                // set the value
                                nonSILFonts[embeddedFont.Value] = sbName.ToString();
                            }
                            else
                            {
                                // new entry
                                nonSILFonts.Add(embeddedFont.Value, embeddedFont.Key);
                            }
                        }
                    }
                    // If there are any non-SIL fonts in use, show the Font Warning Dialog
                    // (possibly multiple times) and replace our embedded font items if needed
                    if (nonSILFonts.Count > 0)
                    {
                        FontWarningDlg dlg = new FontWarningDlg();
                        dlg.RepeatAction = false;
                        dlg.RemainingIssues = nonSILFonts.Count - 1;
                        var langArray = new string[_langFontDictionary.Keys.Count];
                        _langFontDictionary.Keys.CopyTo(langArray,0);
                        foreach (var nonSilFont in nonSILFonts)
                        {
                            dlg.MyEmbeddedFont = nonSilFont.Key.Name;
                            dlg.Languages = nonSilFont.Value;
                            if (dlg.RepeatAction)
                            {
                                // user wants to repeat the last action - if the last action
                                // was to change the font, change this one as well
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
                                continue;
                            }
                            // show the dialog
                            if (dlg.ShowDialog() == DialogResult.OK)
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
                            else
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
                        string dest = Common.PathCombine(contentFolder, embeddedFont.Filename);
                        File.Copy(Path.Combine(EmbeddedFont.GetFontFolderPath(), embeddedFont.Filename), dest);
                        
                    }
                    // clean up
                    if (nonSILFonts.Count > 0)
                    {
                        nonSILFonts.Clear();
                    }
                }
                // update the CSS file to reference any fonts used by the writing systems
                // (if they aren't embedded in the .epub, we'll still link to them here)
                ReferenceFonts(mergedCSS);

                // copy over the XHTML and CSS files
                string cssPath = Common.PathCombine(contentFolder, defaultCSS);
                File.Copy(mergedCSS, cssPath);
                // copy the xhtml files into the content directory; if we can, give them scripture book names
                foreach (string file in htmlFiles)
                {
                    //string name = GetBookName(file);
                    string name = Path.GetFileName(file);
                    string substring = Path.GetFileNameWithoutExtension(file).Substring(8);
                    string dest = Common.PathCombine(contentFolder, "PartFile" + substring.PadLeft(6, '0') + ".xhtml");
                    File.Move(file, dest);
                }

                // copy over the image files
                string[] imageFiles = Directory.GetFiles(tempFolder);
                foreach (string file in imageFiles)
                {
                    switch (Path.GetExtension(file))
                    {
                        case ".jpg":
                        case ".jpeg":
                        case ".gif":
                        case ".png":
                            // .epub supports this image format - just copy the thing over
                            string name = Path.GetFileName(file);
                            string dest = Common.PathCombine(contentFolder, name);
                            File.Copy(file, dest);
                            continue;
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
                                var image = Image.FromFile(file);
                                image.Save(fileStream, System.Drawing.Imaging.ImageFormat.Png);
                            }
                            continue;
                        default:
                            // not an image file (or not one we recognize) - skip
                            continue;
                    }
                }

                // generate the toc / manifest files
                createOpf(projInfo, contentFolder, bookId);
                createNCX(projInfo, contentFolder, bookId);

                // Done adding content - now zip the whole thing up and name it
                string fileName = Path.GetFileNameWithoutExtension(projInfo.DefaultXhtmlFileWithPath);
                Compress(projInfo.TempOutputFolder, Common.PathCombine(outputFolder, fileName));
                TimeSpan tsTotal = DateTime.Now - dt1;
                Debug.WriteLine("Exportepub: time spent in .epub conversion: " + tsTotal);
            }
                
            // clean up and return
            Environment.CurrentDirectory = curdir;
            Cursor.Current = myCursor;
            return success;
        }

        #region Private Functions
        /// <summary>
        /// Inserts links in the CSS file to the fonts used by the writing systems:
        /// - If the fonts are embedded, adds a @font-face declaration referencing the .ttf file 
        ///   that's found in the archive
        /// - Sets the font-family for the body:lang selector to the referenced font
        /// </summary>
        /// <param name="cssFile"></param>
        private void ReferenceFonts (string cssFile)
        {
            if (!File.Exists(cssFile)) return;
            // read in the CSS file
            var reader = new StreamReader(cssFile);
            string content = reader.ReadToEnd();
            reader.Close();
            var sb = new StringBuilder();
            // If we're embedding the fonts, build the @font-face elements
            if (EmbedFonts)
            {
                foreach (var embeddedFont in _embeddedFonts.Values)
                {
                    sb.AppendLine("/* font-face info - added by SIL Pathway */");
                    sb.AppendLine("@font-face {");
                    sb.Append(" font-family : ");
                    sb.Append(embeddedFont.Name);
                    sb.AppendLine(";");
                    sb.Append(" font-weight : ");
                    sb.Append(embeddedFont.Weight);
                    sb.AppendLine(";");
                    sb.Append(" font-style : ");
                    sb.Append(embeddedFont.Style);
                    sb.AppendLine(";");
                    sb.Append(" src : url(");
                    sb.Append(Path.GetFileName(embeddedFont.Filename));
                    sb.AppendLine(");");
                    sb.AppendLine("}");
                }
            }
            // add :lang pseudo-elements for each language and set them to the proper font
            foreach( var language in _langFontDictionary)
            {
                sb.Append("body:lang(");
                sb.Append(language.Key);
                sb.AppendLine(") {");
                sb.Append("font-family: '");
                sb.Append(language.Value);
                sb.Append("', ");
                EmbeddedFont embeddedFont;
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
                sb.Append(getTextDirection(language.Key));
                sb.AppendLine(";");
                sb.AppendLine("}");
            }
            // nuke the @import statement (we're going off one CSS file here)
            string contentNoImport = content.Substring(content.IndexOf(';') + 1);
            sb.Append(contentNoImport);
            // write out the updated CSS file
            var writer = new StreamWriter(cssFile);
            writer.Write(sb.ToString());
            writer.Close();
        }

        /// <summary>
        /// Returns the text direction specified by the writing system, or "ltr" if not found
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        private string getTextDirection(string language)
        {
            string[] langCoun = language.Split('-');
            string direction;

            try
            {
                string wsPath;
                if (langCoun.Length < 2)
                {
                    // try the language (no country code) (e.g, "en" for "en-US")
                    wsPath = Common.PathCombine(Common.GetAllUserAppPath(), "SIL/WritingSystemStore/" + langCoun[0] + ".ldml");
                }
                else
                {
                    // try the whole language expression (e.g., "ggo-Telu-IN")
                    wsPath = Common.PathCombine(Common.GetAllUserAppPath(), "SIL/WritingSystemStore/" + language + ".ldml");
                }
                if (File.Exists(wsPath))
                {
                    var ldml = new XmlDocument { XmlResolver = null };
                    ldml.Load(wsPath);
                    var nsmgr = new XmlNamespaceManager(ldml.NameTable);
                    nsmgr.AddNamespace("palaso", "urn://palaso.org/ldmlExtensions/v1");
                    var node = ldml.SelectSingleNode("//orientation/@characters", nsmgr);
                    if (node != null)
                    {
                        // get the text direction specified by the .ldml file
                        direction = (node.Value.ToLower().Equals("right-to-left")) ? "rtl" : "ltr"; 
                    }
                    else
                    {
                        direction = "ltr";
                    }
                }
                else
                {
                    direction = "ltr";
                }
            }
            catch
            {
                direction = "ltr";
            }

            return direction;
        }

        /// <summary>
        /// Returns the font families for the languages in _langFontDictionary.
        /// </summary>
        private void BuildFontsList()
        {
            // modifying the _langFontDictionary dictionary - let's make an array copy for the iteration
            int numLangs = _langFontDictionary.Keys.Count;
            var langs = new string[numLangs];
            _langFontDictionary.Keys.CopyTo(langs,0);
            foreach (var language in langs)
            {
                string[] langCoun = language.Split('-');

                try
                {
                    string wsPath;
                    if (langCoun.Length < 2)
                    {
                        // try the language (no country code) (e.g, "en" for "en-US")
                        wsPath = Common.PathCombine(Common.GetAllUserAppPath(), "SIL/WritingSystemStore/" + langCoun[0] + ".ldml");
                    }
                    else
                    {
                        // try the whole language expression (e.g., "ggo-Telu-IN")
                        wsPath = Common.PathCombine(Common.GetAllUserAppPath(), "SIL/WritingSystemStore/" + language + ".ldml");
                    }
                    if (File.Exists(wsPath))
                    {
                        var ldml = new XmlDocument { XmlResolver = null };
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
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Returns the user-friendly book name inside this file.
        /// </summary>
        /// <param name="xhtmlFileName">Split xhtml filename in the form PartFile[#]_cv.xhtml</param>
        /// <returns>User-friendly book name (value of the scrBookName or letter element in the xhtml file).</returns>
        private string GetBookName(string xhtmlFileName)
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
            if (_inputType.Equals("dictionary"))
            {
                nodes = xmlDocument.SelectNodes("//xhtml:div[@class='letter']", namespaceManager);
            }
            else
            {
                nodes = xmlDocument.SelectNodes("//xhtml:div[@class='Title_Main']/span", namespaceManager);
                if (nodes == null || nodes.Count == 0)
                {
                    // nothing there - check on the scrBookName span
                    nodes = xmlDocument.SelectNodes("//xhtml:span[@class='scrBookName']", namespaceManager);
                }
                if (nodes == null || nodes.Count == 0)
                {
                    // we're really scraping the bottom - check on the scrBookCode span
                    nodes = xmlDocument.SelectNodes("//xhtml:span[@class='scrBookCode']", namespaceManager);
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

        /// <summary>
        /// Writes the chapter links out to the specified XmlWriter (the .ncx file).
        /// </summary>
        /// <returns>List of url strings</returns>
        private void WriteChapterLinks(string xhtmlFileName, int playOrder, XmlWriter ncx)
        {
            XmlDocument xmlDocument = new XmlDocument { XmlResolver = null };
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, ProhibitDtd = false };
            XmlReader xmlReader = XmlReader.Create(xhtmlFileName, xmlReaderSettings);
            xmlDocument.Load(xmlReader);
            xmlReader.Close();
            XmlNodeList nodes;
            if (_inputType.Equals("dictionary"))
            {
                nodes = xmlDocument.SelectNodes("//xhtml:div[@class='entry']", namespaceManager);
            }
            else
            {
                nodes = xmlDocument.SelectNodes("//xhtml:span[@class='Chapter_Number']", namespaceManager);
            }
            if (nodes != null && nodes.Count > 0)
            {
                var sb = new StringBuilder();
                string name = Path.GetFileName(xhtmlFileName);
                int chapnum = 1;
                foreach(XmlNode node in nodes)
                {
                    string textString;
                    sb.Append(name);
                    sb.Append("#");
                    sb.Append(node.Attributes["id"].Value);
                    if (_inputType.Equals("dictionary"))
                    {
                        // for a dictionary, the headword / headword-minor is the label
                        if (!node.HasChildNodes)
                        {
                            // This entry doesn't have any information - skip it
                            continue;
                        }
                        textString = node.FirstChild.InnerText;
                    }
                    else
                    {
                        // for scriptures, we'll keep a running chapter number count for the label
                        textString = chapnum.ToString();
                        chapnum++;
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
        /// Loads the settings file and pulls out the values we look at.
        /// </summary>
        private void LoadPropertiesFromSettings()
        {
            // Load User Interface Collection Parameters
            Param.LoadSettings();
            // TODO: how to pull in the current style?
            Dictionary<string, string> mobilefeature = Param.GetItemsAsDictionary("//styles/others/style[@name='EBook (epub)']/styleProperty");
            // information
            if (mobilefeature.ContainsKey("Information"))
            {
                Description = mobilefeature["Information"].Trim();
            }
            else
            {
                Description = "";
            }
            // copyright
            if (mobilefeature.ContainsKey("Copyright"))
            {
                Rights = mobilefeature["Copyright"].Trim();
            }
            else
            {
                Rights = "";
            }
            // Source
            if (mobilefeature.ContainsKey("Source"))
            {
                Source = mobilefeature["Source"].Trim();
            }
            else
            {
                Source = "";
            }
            // Format
            if (mobilefeature.ContainsKey("Format"))
            {
                Format = mobilefeature["Format"].Trim();
            }
            else
            {
                Format = "";
            }
            // Publisher
            if (mobilefeature.ContainsKey("Publisher"))
            {
                Publisher = mobilefeature["Publisher"].Trim();
            }
            else
            {
                Publisher = "";
            }
            // Coverage
            if (mobilefeature.ContainsKey("Coverage"))
            {
                Coverage = mobilefeature["Coverage"].Trim();
            }
            else
            {
                Coverage = "";
            }
            // Rights
            if (mobilefeature.ContainsKey("Rights"))
            {
                Rights = mobilefeature["Rights"].Trim();
            }
            else
            {
                Rights = "";
            }
            // embed fonts
            if (mobilefeature.ContainsKey("EmbedFonts"))
            {
                EmbedFonts = (mobilefeature["EmbedFonts"].Trim().Equals("Yes")) ? true : false;
            }
            else
            {
                // default - we're more concerned about accurate font rendering
                EmbedFonts = true;
            }
        }

        /// <summary>
        /// Generates the manifest and metadata information file used by the .epub reader
        /// (content.opf). For more information, refer to <see cref="http://www.idpf.org/doc_library/epub/OPF_2.0.1_draft.htm#Section2.0"/> 
        /// </summary>
        /// <param name="projInfo">Project information</param>
        /// <param name="contentFolder">Content folder (.../OEBPS)</param>
        /// <param name="bookId">Unique identifier for the book we're generating.</param>
        private void createOpf(PublicationInformation projInfo, string contentFolder, Guid bookId)
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
            opf.WriteElementString("dc", "title", null, projInfo.ProjectName);
            opf.WriteStartElement("dc", "creator", null);       //<dc:creator opf:role="aut">[author]</dc:creator>
            opf.WriteAttributeString("opf", "role", null, "aut");
            opf.WriteValue(Environment.UserName);
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
            opf.WriteStartElement("dc", "contributor", null);       // authoring program as a "contributor", e.g.:
            opf.WriteAttributeString("opf", "role", null, "bkp");   // <dc:contributor opf:role="bkp">FieldWorks 7</dc:contributor>
            opf.WriteValue(Common.GetProductName());
            opf.WriteEndElement();
            opf.WriteElementString("dc", "date", null, DateTime.Today.ToString("yyyy-MM-dd")); // .epub standard date format (http://www.idpf.org/2007/opf/OPF_2.0_final_spec.html#Section2.2.7)
            opf.WriteElementString("dc", "type", null, "Text"); // 
            if (Format.Length > 0)
                opf.WriteElementString("dc", "format", null, Format);
            if (Source.Length > 0)
                opf.WriteElementString("dc", "source", null, Source);
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
                foreach (var embeddedFont in _embeddedFonts.Values)
                {
                    opf.WriteStartElement("item"); // item (charis embedded font)
                    opf.WriteAttributeString("id", "epub.embedded.font" + embeddedFont.Name);
                    opf.WriteAttributeString("href", embeddedFont.Filename);
                    opf.WriteAttributeString("media-type", "font/opentype/"); 
                    opf.WriteEndElement(); // item
                }
            }
            // now add the xhtml files to the manifest
            string[] files = Directory.GetFiles(contentFolder);
            foreach (string file in files) 
            {
                // iterate through the file set and add <item> elements for each xhtml file
                string name = Path.GetFileName(file);
                string nameNoExt = Path.GetFileNameWithoutExtension(file);
                if (name.EndsWith(".xhtml"))
                {
                    // if we can, write out the "user friendly" book name in the TOC
                    string bookName = GetBookName(file); 
//                    string bookName = (nameNoExt.IndexOf("_") > -1) ?
//                        (nameNoExt.Substring(name.IndexOf("_") + 1)) : (nameNoExt);
                    opf.WriteStartElement("item");
                    opf.WriteAttributeString("id", bookName);
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
                else if (name.EndsWith(".jpg") || name.EndsWith(".jpeg"))
                {
                    opf.WriteStartElement("item"); // item (image)
                    opf.WriteAttributeString("id", "image" + nameNoExt);
                    opf.WriteAttributeString("href", name);
                    opf.WriteAttributeString("media-type", "image/jpg");
                    opf.WriteEndElement(); // item
                }
                else if (name.EndsWith(".gif"))
                {
                    opf.WriteStartElement("item"); // item (image)
                    opf.WriteAttributeString("id", "image" + nameNoExt);
                    opf.WriteAttributeString("href", name);
                    opf.WriteAttributeString("media-type", "image/gif");
                    opf.WriteEndElement(); // item
                }
                else if (name.EndsWith(".png"))
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
            foreach (string file in files)
            {
                // add an <itemref> for each xhtml file in the set
                string name = Path.GetFileName(file);
                if (name.EndsWith(".xhtml"))
                {
                    string nameNoExt = Path.GetFileNameWithoutExtension(file);
                    string bookName = GetBookName(file); 
//                    string bookName = (nameNoExt.IndexOf("_") > -1) ? 
//                        (nameNoExt.Substring(name.IndexOf("_") + 1)) : (nameNoExt);
                    opf.WriteStartElement("itemref"); // item (stylesheet)
                    opf.WriteAttributeString("idref", bookName);
                    opf.WriteEndElement(); // itemref
                }
            }
            opf.WriteEndElement(); // spine
            // guide
            opf.WriteStartElement("guide");
            // The <guide> element is optional; I'm not sure that it buys us anything -
            // for now, just add a single element for the first filename in the list.
            opf.WriteStartElement("reference");
            opf.WriteAttributeString("type", "text");
            opf.WriteAttributeString("title", projInfo.ProjectName);
            opf.WriteAttributeString("href", Path.GetFileName(files[0]));
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
        private void createNCX(PublicationInformation projInfo, string contentFolder, Guid bookId)
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
            int index = 1;
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                if (name.EndsWith(".xhtml"))
                {
                    string nameNoExt = Path.GetFileNameWithoutExtension(file);
                    string bookName = GetBookName(file);
//                    string bookName = (nameNoExt.IndexOf("_") > -1) ?
//                        (nameNoExt.Substring(name.IndexOf("_") + 1)) : (nameNoExt);
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
                    WriteChapterLinks(file, index, ncx);
                    // end the book's navPoint element
                    ncx.WriteEndElement(); // navPoint
                }
            }
            ncx.WriteEndElement(); // navmap
            ncx.WriteEndElement(); // ncx
            ncx.WriteEndDocument();
            ncx.Close();
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

            mODT.CreateZip(sourceFolder, outputPathWithFileName, 0);

            // add the mimetype file to this archive, uncompressed
            string strFromOfficeFolder = Common.PathCombine(Common.GetPSApplicationPath(), "epub");
            string mimetypeFile = Common.PathCombine(strFromOfficeFolder, "mimetype");
            string copiedMimetypePath = Path.Combine(sourceFolder, Path.GetFileName(mimetypeFile));
            File.Copy(mimetypeFile, copiedMimetypePath);
            mODT.AddToZip(copiedMimetypePath, outputPathWithFileName, false);

            try
            {
                Common.OpenOutput(outputPathWithFileName);
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                if (ex.NativeErrorCode == 1155)
                {

                }
            }
        }

        #endregion

    }
}
