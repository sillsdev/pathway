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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using epubConvert;
using epubConvert.Properties;
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
                var langArray = new string[_langFontDictionary.Keys.Count];
                _langFontDictionary.Keys.CopyTo(langArray, 0);
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
                string cvFileName = Path.GetFileNameWithoutExtension(preProcessor.ProcessedXhtml) + "_";
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
                Common.ReplaceInFile(preProcessor.ProcessedXhtml, "<html", string.Format("<html xmlns='http://www.w3.org/1999/xhtml' xml:lang='{0}' dir='{1}'", langArray[0], getTextDirection(langArray[0])));
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

                // If we are working with a dictionary and have a reversal index, process it now
                if (projInfo.IsReversalExist)
                {
                    var revFile = Path.Combine(Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath), "FlexRev.xhtml");
                    // EDB 10/20/2010 - TD-1629 - remove when merged CSS passes validation
                    // (note that the rev file uses a "FlexRev.css", not "main.css"
                    Common.SetDefaultCSS(revFile, defaultCSS);
                    // EDB 10/29/2010 FWR-2697 - remove when fixed in FLEx
                    Common.ReplaceInFile(revFile, "<ReversalIndexEntry_Self", "<span class='ReversalIndexEntry_Self'");
                    Common.ReplaceInFile(revFile, "</ReversalIndexEntry_Self", "</span");
                    // now split out the html as needed
                    List<string> fileNameWithPath = new List<string>();
                    fileNameWithPath = Common.SplitXhtmlFile(revFile, "letHead", "RevIndex", true);
                    splitFiles.AddRange(fileNameWithPath);
                }

                foreach (string file in splitFiles)
                {
                    if (File.Exists(file))
                    {
                        Common.XsltProcess(file, xsltFullName, "_.xhtml");
                        // add this file to the html files list
                        htmlFiles.Add(Path.Combine(Path.GetDirectoryName(file), (Path.GetFileNameWithoutExtension(file) + "_.xhtml")));
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
                    if (nonSILFonts.Count > 0)
                    {
                        FontWarningDlg dlg = new FontWarningDlg();
                        dlg.RepeatAction = false;
                        dlg.RemainingIssues = nonSILFonts.Count - 1;
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
                            // sanity check - are there any SIL fonts installed?
                            int count = dlg.BuildSILFontList();
                            if (count == 0)
                            {
                                // No SIL fonts found (returns a DialogResult.Abort):
                                // tell the user there are no SIL fonts installed, and allow them to Cancel
                                // and install the fonts now
                                if (MessageBox.Show (Resources.NoSILFontsMessage, Resources.NoSILFontsTitle, 
                                    MessageBoxButtons.OKCancel,MessageBoxIcon.Warning) 
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
                        string dest = Common.PathCombine(contentFolder, embeddedFont.Filename);
                        File.Copy(Path.Combine(FontInternals.GetFontFolderPath(), embeddedFont.Filename), dest);
                        
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
                // copy the xhtml files into the content directory
                foreach (string file in htmlFiles)
                {
                    string name = Path.GetFileNameWithoutExtension(file).Substring(0, 8);
                    string substring = Path.GetFileNameWithoutExtension(file).Substring(8);
                    string dest = Common.PathCombine(contentFolder, name + substring.PadLeft(6, '0') + ".xhtml");
                    File.Move(file, dest);
                    // split the file into smaller pieces if needed (scriptures only for now)
                    if (_inputType == "scripture")
                    {
                        List<string> files = SplitBook(dest);
                        if (files.Count > 1)
                        {
                            // file was split out - delete "dest" (it's been replaced)
                            File.Delete(dest);
                        }
                    }
                }

                // copy over the image files
                string[] imageFiles = Directory.GetFiles(tempFolder);
                bool renamedImages = false;
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
                            renamedImages = true;
                            continue;
                        default:
                            // not an image file (or not one we recognize) - skip
                            continue;
                    }
                }
                // be sure to clean up any hyperlink references to the old file types
                if (renamedImages)
                {
                    CleanupImageReferences(contentFolder);
                }

                // generate the toc / manifest files
                CreateOpf(projInfo, contentFolder, bookId);
                CreateNcx(projInfo, contentFolder, bookId);
                CreateCoverImage(contentFolder, projInfo);

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
                    sb.Append(" font-weight : ");
                    sb.Append(embeddedFont.Weight);
                    sb.AppendLine(";");
                    sb.Append(" font-style : ");
                    sb.Append(embeddedFont.Style);
                    sb.AppendLine(";");
                    sb.AppendLine(" font-variant : normal;");
                    sb.AppendLine(" font-size : all;");
                    sb.Append(" src : url(");
                    sb.Append(Path.GetFileName(embeddedFont.Filename));
                    sb.AppendLine(");");
                    sb.AppendLine("}");
                }
            }
            // add :lang pseudo-elements for each language and set them to the proper font
            foreach( var language in _langFontDictionary)
            {
                sb.Append("*:lang(");
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
        /// Returns a book ID to be used in the .opf file. This is similar to the GetBookName call, but here
        /// we're wanting something that (1) doesn't start with a numeric value and (2) is unique.
        /// </summary>
        /// <param name="xhtmlFileName"></param>
        /// <returns></returns>
        private string GetBookID (string xhtmlFileName)
        {
            xhtmlFileName.GetHashCode();
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
                // start out with the book code (e.g., 2CH for 2 Chronicles)
                nodes = xmlDocument.SelectNodes("//xhtml:span[@class='scrBookCode']", namespaceManager);
                if (nodes == null || nodes.Count == 0)
                {
                    // no book code - use scrBookName
                    nodes = xmlDocument.SelectNodes("//xhtml:span[@class='scrBookName']", namespaceManager);
                }
                if (nodes == null || nodes.Count == 0)
                {
                    // no scrBookName - use Title_Main
                    nodes = xmlDocument.SelectNodes("//xhtml:div[@class='Title_Main']/span", namespaceManager);
                }
            }
            if (nodes != null && nodes.Count > 0)
            {
                var sb = new StringBuilder();
                // just in case the name starts with a number, prepend "id"
                sb.Append("id");
                // remove any whitespace in the node text (the ID can't have it)
                sb.Append(new Regex(@"\s*").Replace(nodes[0].InnerText, string.Empty));
                return (sb.ToString());
            }
            // fall back on just the file name
            return Path.GetFileName(xhtmlFileName);            
        }

        /// <summary>
        /// Returns the user-friendly book name inside this file.
        /// </summary>
        /// <param name="xhtmlFileName">Split xhtml filename in the form PartFile[#]_.xhtml</param>
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
        /// Creates a cover image based on the language code and type of project (dictionary or scripture). This
        /// is saved as "cover.png" in the content (OEBPS) folder.
        /// </summary>
        /// <param name="contentFolder">Content folder the resulting file is saved to.</param>
        /// <param name="projInfo"></param>
        private void CreateCoverImage(string contentFolder, PublicationInformation projInfo)
        {
            // open up the appropriate image for processing
            string strGraphicsFolder = Common.PathCombine(Common.GetPSApplicationPath(), "Graphic");
            string strImageFile = Path.Combine(strGraphicsFolder, "cover.png");
            if (!File.Exists(strImageFile)) return;
            var bmp = new Bitmap(strImageFile);
            Graphics g = Graphics.FromImage(bmp);
            // We're going to be "badging" the book image with a title - this consists of the database name
            // and project name (split into multiple lines if from Paratext)
            var enumerator = _langFontDictionary.GetEnumerator();
            enumerator.MoveNext();
            var sb = new StringBuilder();
            sb.AppendLine(Common.databaseName);
            if (projInfo.ProjectName.Contains("EBook (epub)"))
            {
                // Paratext has a _really long_ project name - split out into multiple lines
                string[] parts = projInfo.ProjectName.Split('_');
                sb.AppendLine(parts[0]); // "EBook (epub)"
                sb.Append(parts[1]); // date of publication
            }
            else
            {
                sb.Append(projInfo.ProjectName);
            }
            //var langCode = enumerator.Current.Key;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            var strFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            // figure out the dimensions of our rect based on the font info
            Font badgeFont = new Font("Times New Roman", 48);
            SizeF size = g.MeasureString(sb.ToString(), badgeFont, 640);
            int width = (int) Math.Ceiling(size.Width);
            int height = (int) Math.Ceiling(size.Height);
            Rectangle rect = new Rectangle((225 - (width / 2)), 100, width, height);
            // draw the badge (rect and string)
            g.FillRectangle(Brushes.Brown, rect);
            g.DrawRectangle(Pens.Gold, rect);
            g.DrawString(sb.ToString(), badgeFont, Brushes.Gold, new RectangleF(new PointF((225f - (size.Width / 2)), 100f),size), strFormat);
            // save this puppy
            string strCoverImageFile = Path.Combine(contentFolder, "cover.png");
            bmp.Save(strCoverImageFile);
        }

        /// <summary>
        /// Writes the chapter links out to the specified XmlWriter (the .ncx file).
        /// </summary>
        /// <returns>List of url strings</returns>
        private void WriteChapterLinks(string xhtmlFileName, ref int playOrder, XmlWriter ncx, ref int chapnum)
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
                if (xhtmlFileName.Contains("RevIndex"))
                {
                    nodes = xmlDocument.SelectNodes("//xhtml:span[@class='ReversalIndexEntry_Self']", namespaceManager);
                }
                else
                {
                    nodes = xmlDocument.SelectNodes("//xhtml:div[@class='entry']", namespaceManager);
                }
            }
            else
            {
                nodes = xmlDocument.SelectNodes("//xhtml:span[@class='Chapter_Number']", namespaceManager);
            }
            if (nodes != null && nodes.Count > 0)
            {
                var sb = new StringBuilder();
                string name = Path.GetFileName(xhtmlFileName);
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
                            // reset the stringbuilder
                            sb.Length = 0;
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
        /// The .epub format doesn't support all image file types; when we copied the image files over, we had
        /// to convert the unsupported file types to .png. Here we'll do a search/replace for all references to
        /// the old versions.
        /// </summary>
        /// <param name="contentFolder">OEBPS folder containing all the xhtml files we need to clean up</param>
        private void CleanupImageReferences (string contentFolder)
        {
            string[] files = Directory.GetFiles(contentFolder, "*.xhtml");
            foreach (string file in files)
            {
                var reader = new StreamReader(file);
                string content = reader.ReadToEnd();
                reader.Close();
                content = Regex.Replace(content, ".bmp", ".png");
                content = Regex.Replace(content, ".tiff", ".png");
                content = Regex.Replace(content, ".tif", ".png");
                content = Regex.Replace(content, ".ico", ".png");
                content = Regex.Replace(content, ".wmf", ".png");
                content = Regex.Replace(content, ".pcx", ".png");
                content = Regex.Replace(content, ".cgm", ".png");
                var writer = new StreamWriter(file);
                writer.Write(content);
                writer.Close();
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
                // look for the next <div class="Section_Head"> after our soft maximum size
                string outFile = Path.Combine(Path.GetDirectoryName(xhtmlFilename), (Path.GetFileNameWithoutExtension(xhtmlFilename) + fileIndex.ToString().PadLeft(2, '0') + ".xhtml"));
                softMax = startIndex + (int) (maxSize/2); // UTF-16
                if (softMax > content.Length)
                {
                    realMax = -1;
                }
                else
                {
                    realMax = content.IndexOf("<div class=\"Section_Head", softMax);
                }
                if (realMax == -1)
                {
                    // no more section heads - just pull in the rest of the content
                    // write out head + substring(startIndex to the end)
                    sb.Append(head);
                    sb.Append("<body class=\"scrBody\"><div class=\"scrBook\">");
                    sb.Append(bookcode);
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
                    sb.Append("<body class=\"scrBody\"><div class=\"scrBook\">");
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
            opf.WriteElementString("dc", "title", null, Common.databaseName + " " + projInfo.ProjectName);
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
            // meta elements
            opf.WriteStartElement("meta");
            opf.WriteAttributeString("name", "cover");
            opf.WriteAttributeString("content", "cover-image");
            opf.WriteEndElement(); // meta
            opf.WriteEndElement(); // metadata
            // manifest
            opf.WriteStartElement("manifest");
            // (individual "item" elements in the manifest)
            opf.WriteStartElement("item");
            opf.WriteAttributeString("id", "ncx");
            opf.WriteAttributeString("href", "toc.ncx");
            opf.WriteAttributeString("media-type", "application/x-dtbncx+xml");
            opf.WriteEndElement(); // item
            opf.WriteStartElement("item");
            opf.WriteAttributeString("id", "cover");
            opf.WriteAttributeString("href", "cover.html");
            opf.WriteAttributeString("media-type", "application/xhtml+xml");
            opf.WriteEndElement(); // item
            opf.WriteStartElement("item");
            opf.WriteAttributeString("id", "cover-image");
            opf.WriteAttributeString("href", "cover.png");
            opf.WriteAttributeString("media-type", "image/png");
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
                    opf.WriteAttributeString("href", embeddedFont.Filename);
                    opf.WriteAttributeString("media-type", "font/opentype/"); 
                    opf.WriteEndElement(); // item
                    fontNum++;
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
                    string fileId = GetBookID(file); 
                    opf.WriteStartElement("item");
                    if (_inputType == "dictionary")
                    {
                        // the book ID can be wacky (and non-unique) for dictionaries. Just use the filename.
                        opf.WriteAttributeString("id", nameNoExt);
                    }
                    else
                    {
                        // scripture - use the book ID
                        opf.WriteAttributeString("id", fileId);
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
                else if (name.EndsWith(".jpg") || name.EndsWith(".jpeg"))
                {
                    opf.WriteStartElement("item"); // item (image)
                    opf.WriteAttributeString("id", "image" + nameNoExt);
                    opf.WriteAttributeString("href", name);
                    opf.WriteAttributeString("media-type", "image/jpeg");
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
            // a couple items for the cover image
            opf.WriteStartElement("itemref"); 
            opf.WriteAttributeString("idref", "cover");
            opf.WriteAttributeString("linear", "yes");
            opf.WriteEndElement(); // itemref
            foreach (string file in files)
            {
                // add an <itemref> for each xhtml file in the set
                string name = Path.GetFileName(file);
                if (name.EndsWith(".xhtml"))
                {
                    string fileId = GetBookID(file); 
                    opf.WriteStartElement("itemref"); // item (stylesheet)
                    if (_inputType == "dictionary")
                    {
                        // the book ID can be wacky (and non-unique) for dictionaries. Just use the filename.
                        opf.WriteAttributeString("idref", Path.GetFileNameWithoutExtension(file));
                    }
                    else
                    {
                        // scripture - use the book ID
                        opf.WriteAttributeString("idref", fileId);
                    }
                    opf.WriteEndElement(); // itemref
                }
            }
            opf.WriteEndElement(); // spine
            // guide
            opf.WriteStartElement("guide");
            // cover image
            opf.WriteStartElement("reference");
            opf.WriteAttributeString("href", "cover.html");
            opf.WriteAttributeString("type", "cover");
            opf.WriteAttributeString("title", "Cover");
            opf.WriteEndElement(); // reference
            // first xhtml filename
            opf.WriteStartElement("reference");
            opf.WriteAttributeString("type", "text");
            opf.WriteAttributeString("title", Common.databaseName + " " + projInfo.ProjectName);
            int index = 0;
            while (!files[index].EndsWith(".xhtml") && index < files.Length)
            {
                index++;
            }
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
                    ncx.WriteAttributeString("src", name);
                    ncx.WriteEndElement(); // meta
                    index++;
                    RevIndex = true;
                }
//              string nameNoExt = Path.GetFileNameWithoutExtension(file);
                if (!Path.GetFileNameWithoutExtension(file).EndsWith("_"))
                {
                    // this is a split file - is it the first one?
                    if (Path.GetFileNameWithoutExtension(file).EndsWith("1"))
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
                        WriteChapterLinks(file, ref index, ncx, ref chapNum);
                        needsEnd = true;
                    }
                    else
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
                    WriteChapterLinks(file, ref index, ncx, ref chapNum);
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
            var containerFile = new string[1] {sb.ToString()};
            mODT.AddToZip(containerFile, zipFile);
            // copy the results to the output directory
            File.Copy(zipFile, outputPathWithFileName, true);

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
