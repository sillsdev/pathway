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
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
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

//        protected static PostscriptLanguage _postscriptLanguage = new PostscriptLanguage();
        protected string _inputType = "dictionary";

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
                var inProcess = new InProcess(0, 9); // create a progress bar with 7 steps (we'll add more below)
                inProcess.Text = Resources.Exportepub_Export_Exporting__epub_file;
                inProcess.Show();
                inProcess.PerformStep();
                var sb = new StringBuilder();
                Guid bookId = Guid.NewGuid(); // NOTE: this creates a new ID each time Pathway is run. 
                string outputFolder = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath); // finished .epub goes here
                PreExportProcess preProcessor = new PreExportProcess(projInfo);
                Environment.CurrentDirectory = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath);
                Common.SetProgressBarValue(projInfo.ProgressBar, projInfo.DefaultXhtmlFileWithPath);
                inProcess.PerformStep();
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
                string tempFolderName = Path.GetFileName(tempFolder);
                var mc = new MergeCss { OutputLocation = tempFolderName };
                string mergedCSS = mc.Make(projInfo.DefaultCssFileWithPath, "book.css");
                preProcessor.ReplaceStringInCss(mergedCSS);
                preProcessor.SetDropCapInCSS(mergedCSS);
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
                Common.StreamReplaceInFile(preProcessor.ProcessedXhtml, "<html", string.Format("<html xmlns='http://www.w3.org/1999/xhtml' xml:lang='{0}' dir='{1}'", langArray[0], Common.GetTextDirection(langArray[0])));
                // end EDB 10/22/2010
                inProcess.PerformStep();

                // split the .XHTML into multiple files, as specified by the user
                List<string> htmlFiles = new List<string>();
                List<string> splitFiles = new List<string>();
                Common.XsltProgressBar = inProcess.Bar();

                // insert the front matter items as separate files in the output folder
                splitFiles.AddRange(preProcessor.InsertFrontMatter(tempFolder, false));
                foreach(var file in splitFiles)
                {
                    Common.SetDefaultCSS(file, defaultCSS);
                }
                if (projInfo.FileToProduce.ToLower() != "one")
                {
                    splitFiles.AddRange(SplitFile(preProcessor.ProcessedXhtml, projInfo));
                }
                else
                {
                    splitFiles.Add(preProcessor.ProcessedXhtml);
                }

                // If we are working with a dictionary and have a reversal index, process it now)
                if (projInfo.IsReversalExist)
                {
                    var revFile = Path.Combine(Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath), "FlexRev.xhtml");
                    // EDB 10/20/2010 - TD-1629 - remove when merged CSS passes validation
                    // (note that the rev file uses a "FlexRev.css", not "main.css"
                    Common.SetDefaultCSS(revFile, defaultCSS);
                    // EDB 10/29/2010 FWR-2697 - remove when fixed in FLEx
                    Common.StreamReplaceInFile(revFile, "<ReversalIndexEntry_Self", "<span class='ReversalIndexEntry_Self'");
                    Common.StreamReplaceInFile(revFile, "</ReversalIndexEntry_Self", "</span");
                    Common.StreamReplaceInFile(revFile, "<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"utf-8\" lang=\"utf-8\"", string.Format("<html  xmlns='http://www.w3.org/1999/xhtml' xml:lang='{0}' dir='{1}'", langArray[0], Common.GetTextDirection(langArray[0])));
                    // now split out the html as needed
                    List<string> fileNameWithPath = new List<string>();
                    fileNameWithPath = Common.SplitXhtmlFile(revFile, "letHead", "RevIndex", true);
                    splitFiles.AddRange(fileNameWithPath);
                }
                // add the total file count (so far) to the progress bar, so it's a little more accurate
                inProcess.AddToMaximum(splitFiles.Count);

                if (_inputType.Equals("dictionary"))
                {
                    ContentCssToXhtml(niceNameCSS, splitFiles);
                }
                // get rid of styles that don't work with .epub
                RemovePagedStylesFromCss(niceNameCSS);
                // customize the CSS file based on the settings
                CustomizeCSS(mergedCSS);

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
                    inProcess.PerformStep();
                }

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
                inProcess.PerformStep();

                // -- Font handling --
                // First, get the list of fonts used in this project
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
                // fix any relative hyperlinks that might have broken when we split the .xhtml up
                FixRelativeHyperlinks(contentFolder, inProcess);
                inProcess.PerformStep();

                // copy over the image files
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
                                switch (Path.GetExtension(file).ToLower())
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
                inProcess.PerformStep();

                // generate the toc / manifest files
                CreateOpf(projInfo, contentFolder, bookId);
                CreateNcx(projInfo, contentFolder, bookId);
                inProcess.PerformStep();

                // Done adding content - now zip the whole thing up and name it
                string fileName = Path.GetFileNameWithoutExtension(projInfo.DefaultXhtmlFileWithPath);
                Compress(projInfo.TempOutputFolder, Common.PathCombine(outputFolder, fileName));
                TimeSpan tsTotal = DateTime.Now - dt1;
                Debug.WriteLine("Exportepub: time spent in .epub conversion: " + tsTotal);
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
                    success = (validationResults.Contains("No errors or warnings detected"));
                }
                else
                {
                    MessageBox.Show(Resources.ExportCallingEpubValidator, Resources.ExportComplete, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    var validationDialog = new ValidationDialog();
                    validationDialog.FileName = outputPathWithFileName;
                    validationDialog.ShowDialog();
                }
            }
                
            return success;
        }

        #region Private Functions

        #region MergedCSSHandling
        /// <summary>
        /// This method addresses an ugly problem in e-book readers, where the :before and :after pseudo-items in the CSS aren't
        /// recognized (causing some bad spacing / punctuation). We work around the issue by moving the properties into an XSLT
        /// file and running it on the given list of xhtmlFiles -- moving the characters into the XHTML so it will render
        /// in all readers.
        /// </summary>
        /// <param name="cssFile">CSS file containing content properties that need to be moved into the XHTML</param>
        /// <param name="xhtmlFiles">List of XHTML files that will get the content characters inserted via our generated XSLT.</param>
        private void ContentCssToXhtml (string cssFile, List<string> xhtmlFiles)
        {
            if (!File.Exists(cssFile)) {return;}
            string xsltFullName = Common.FromRegistry("punct_XHTML.xslt");
            if (!File.Exists(xsltFullName)) {return;}
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
            sbBetweenXItem.AppendLine("</xsl:choose></xsl:if>");
            sbBetweenSense.AppendLine("</xsl:choose></xsl:if>");
            // append the two "between" cases to the after case
            sbAfter.AppendLine(sbBetweenSense.ToString());
            sbAfter.AppendLine(sbBetweenXItem.ToString());

            // replace the before and after blocks in the XSLT
            Common.StreamReplaceInFile(tempXslt, "<!-- OPENING lexical punctuation -->", sbBefore.ToString());
            Common.StreamReplaceInFile(tempXslt, "<!-- CLOSING lexical punctuation -->", sbAfter.ToString());

            // final step - run the updated xslt file
            foreach (var xhtmlFile in xhtmlFiles)
            {
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
        private void RemovePagedStylesFromCss (string cssFile)
        {
            if (!File.Exists(cssFile)) {return;}
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
                    while (bracketCount != 0)
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
                string dest = Common.PathCombine(contentFolder, embeddedFont.Filename);
                File.Copy(Path.Combine(FontInternals.GetFontFolderPath(), embeddedFont.Filename), dest);
                if (IncludeFontVariants)
                {
                    // italic
                    if (embeddedFont.HasItalic && embeddedFont.ItalicFilename != embeddedFont.Filename)
                    {
                        dest = Common.PathCombine(contentFolder, embeddedFont.ItalicFilename);
                        if (!File.Exists(dest))
                        {
                            File.Copy(Path.Combine(FontInternals.GetFontFolderPath(), embeddedFont.ItalicFilename),
                                      dest);
                        }
                    }
                    // bold
                    if (embeddedFont.HasBold && embeddedFont.BoldFilename != embeddedFont.Filename)
                    {
                        dest = Common.PathCombine(contentFolder, embeddedFont.BoldFilename);
                        if (!File.Exists(dest))
                        {
                            File.Copy(Path.Combine(FontInternals.GetFontFolderPath(), embeddedFont.BoldFilename),
                                      dest);
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
        private void ReferenceFonts (string cssFile)
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
                    sb.Append(" src : url(");
                    sb.Append(Path.GetFileName(embeddedFont.Filename));
                    sb.AppendLine(");");
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
                            sb.Append(" src : url(");
                            sb.Append(Path.GetFileName(embeddedFont.ItalicFilename));
                            sb.AppendLine(");");
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
                            sb.Append(" src : url(");
                            sb.Append(Path.GetFileName(embeddedFont.BoldFilename));
                            sb.AppendLine(");");
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
                        if (embeddedFont.HasItalic)
                        {
                            sb.Append(".partofspeech, .example, .grammatical-info, .lexref-type, ");
                            sb.Append(".parallel_passage_reference, .Parallel_Passage_Reference, ");
                            sb.AppendLine(".Emphasis, .pictureCaption, .Section_Range_Paragraph {");
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
                        // Bold version
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
                            sb.Append(".Alternate_Reading, .Section_Head, .Section_Head_Minor, ");
                            sb.AppendLine(".Inscription, .Intro_Section_Head, .Section_Head_Major, .iot {");
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
                sb.Append((ChapterNumbers == "Drop Cap") ? "48pt;" : "5pt;");
                Common.ReplaceInFile(cssFile, ".Chapter_Number {", sb.ToString());
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

        /// <summary>
        /// Returns a book ID to be used in the .opf file. This is similar to the GetBookName call, but here
        /// we're wanting something that (1) doesn't start with a numeric value and (2) is unique.
        /// </summary>
        /// <param name="xhtmlFileName"></param>
        /// <returns></returns>
        private string GetBookID (string xhtmlFileName)
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
            var fileNoPath = Path.GetFileName(xhtmlFileName);
            if (fileNoPath.StartsWith(PreExportProcess.CoverPageFilename.Substring(0, 8)))
            {
                return ("Cover Page");
            }
            if (fileNoPath.StartsWith(PreExportProcess.TitlePageFilename.Substring(0, 8)))
            {
                return ("Title Page");
            }
            if (fileNoPath.StartsWith(PreExportProcess.CopyrightPageFilename.Substring(0, 8)))
            {
                return ("Copyright Information");
            }
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
        /// Resizes the given image down to MaxImageWidth pixels and returns the result.
        /// </summary>
        /// <param name="image">File to resize</param>
        private Image ResizeImage(Image image)
        {
            float nPercent = ((float)MaxImageWidth / (float)image.Width);
            int destW = (int) (image.Width * nPercent);
            int destH = (int) (image.Height*nPercent);
            var b = new Bitmap(destW, destH);
            var g = Graphics.FromImage((Image) b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(image, 0, 0, destW, destH);
            g.Dispose();
            return (Image)b;
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
                    XmlNode val = node.Attributes["id"];
                    if(val != null)
                        sb.Append(val.Value);
                    //sb.Append(node.Attributes["id"].Value);
                        
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
                                playOrder++;
                                textString = childNode.FirstChild.FirstChild.InnerText;

                                sb.Append(name);
                                sb.Append("#");
                                if (childNode.Attributes != null)
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
                            }
                        }
                    }
                    ncx.WriteEndElement(); // navPoint
                    // reset the stringbuilder
                    sb.Length = 0;
                    playOrder++;
                }
            }
        }

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
                // We now have the href target id. Does the target exist in this file?
                if (content.IndexOf("id=\"" + hrefID) == -1)
                {
                    // not found -- this link is broken
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

        private bool IsStringInFile(string filePath, string searchText)
        {
            if (!File.Exists(filePath)) return false;
            var reader = new FileStream(filePath, FileMode.Open);
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
        }

        /// <summary>
        /// When the preprocessed xhtml gets split out into chunks the epub reader can understand, the
        /// targets for the relative links within the xhtml file might get moved into a separate file. This method
        /// locates each relative link within the content folder and replaces it with an absolute one (i.e., one containing
        /// the filename and id anchor).
        /// </summary>
        /// <param name="contentFolder">Folder containing the xhtml to be parsed</param>
        /// <param name="inProcess">progress bar (this is a potentially long-running process)</param>
        private void FixRelativeHyperlinks (string contentFolder, InProcess inProcess)
        {
            string[] files = Directory.GetFiles(contentFolder, "*.xhtml");
            inProcess.AddToMaximum(files.Length);
            var sbID = new StringBuilder();
            var startTime = DateTime.Now;
            bool bFound = false;
            foreach (string sourceFile in files)
            {
                var relativeIDs = FindBrokenRelativeHrefIds(sourceFile);
                Debug.WriteLine(sourceFile + ": " + relativeIDs.Count + " broken hrefs:");
                foreach (var relativeID in relativeIDs)
                {
                    //    find [id="<n>] in the xhtml file list
                    sbID.Length = 0; // reset the stringbuilder
                    sbID.Append("id=\"");
                    sbID.Append(relativeID);
                    bFound = false;
                    foreach (var targetFile in files)
                    {
                        if (IsStringInFile(targetFile, sbID.ToString()))
                        {
                            //    replace [a href="#<n>] with [a href="<filename>#<n>]
                            Debug.WriteLine("- href id:" + relativeID + " found in " + targetFile);
                            Common.StreamReplaceInFile(sourceFile, ("a href=\"#" + relativeID), ("a href=\"" + Path.GetFileName(targetFile) + "#" + relativeID));
                            bFound = true;
                        }
                    }
                    if (bFound == false)
                    {
                        Debug.WriteLine(">> Target ID for " + relativeID + " not found - this link is broken!");
                    }
                }
                // show our progress
                inProcess.PerformStep();
            }
            TimeSpan tsTotal = DateTime.Now - startTime;
            Debug.WriteLine("Exportepub: time spent in FixRelativeHyperlinks: " + tsTotal);
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
                // using a streaming approach to reduce the memory footprint of this method
                // (we had Regex.Replace before, but it was using >100MB of data on larger dictionaries)
                var reader = new StreamReader(file);
                var writer = new StreamWriter(file + ".tmp");
                Int32 next;
                while ((next = reader.Read()) != -1)
                {
                    char b = (char) next;
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
                                if (((char) nextchar) == 'f')
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
                    case  "Cancel Export":
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
                switch(othersfeature["NonSILFont"].Trim())
                {
                    case "Embed Font Anyway":
                        NonSilFont = FontHandling.EmbedFont;
                        break;
                    case "Use Fallback Font":
                        NonSilFont = FontHandling.SubstituteDefaultFont;
                        break;
                    case  "Cancel Export":
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

        #region File Processing Methods
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
                softMax = startIndex + (int)(maxSize / 2); // UTF-16
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
                    opf.WriteAttributeString("href", embeddedFont.Filename);
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
                            opf.WriteAttributeString("href", embeddedFont.ItalicFilename);
                            opf.WriteAttributeString("media-type", "font/opentype/");
                            opf.WriteEndElement(); // item
                            fontNum++;
                        }
                        // bold
                        if (embeddedFont.HasBold && embeddedFont.Filename.CompareTo(embeddedFont.BoldFilename) != 0)
                        {
                            opf.WriteStartElement("item"); // item (charis embedded font)
                            opf.WriteAttributeString("id", "epub.embedded.font_b_" + fontNum);
                            opf.WriteAttributeString("href", embeddedFont.BoldFilename);
                            opf.WriteAttributeString("media-type", "font/opentype/");
                            opf.WriteEndElement(); // item
                            fontNum++;
                        }
                    }
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
                        if (!skipChapterInfo)
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

        #endregion

        #endregion
    }
}
