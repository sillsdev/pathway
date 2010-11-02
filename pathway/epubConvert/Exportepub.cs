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
        private EmbeddedFont _defaultFont;  // default font used by this writing system

//        protected static PostscriptLanguage _postscriptLanguage = new PostscriptLanguage();
        protected string _inputType;

        // property implementations
        public string FileProduced { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public string Contributor { get; set; }
        public string Relation { get; set; }
        public string Coverage { get; set; }
        public string Rights { get; set; }
        public string Subject { get; set; }
        public string Type { get; set; }
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

                projInfo.LanguageFilterKey = GetLanguage(projInfo.DefaultXhtmlFileWithPath);
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
                string temporaryCvFullName = Common.PathCombine(tempFolder, cvFileName + ".xhtml");
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

                Common.XsltProcess(preProcessor.ProcessedXhtml, xsltFullName, "_cv.xhtml");
                // split the .XHTML into multiple files, as specified by the user
                List<string> htmlFiles = new List<string>();
                if (projInfo.FileToProduce.ToLower() != "one")
                {
                    htmlFiles = SplitFile(temporaryCvFullName, projInfo);
                }
                else
                {
                    htmlFiles.Add(temporaryCvFullName);
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

                // copy the embedded font if needed
                if (EmbedFonts)
                {
                    // First, get the default font for this language from the project info
                    GetDefaultFontFamily(projInfo.LanguageFilterKey);
                    if (!_defaultFont.SILFont)
                    {
                        FontWarningDlg dlg = new FontWarningDlg();
                        dlg.MyEmbeddedFont = _defaultFont.Name;
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            if (!_defaultFont.Name.Equals(dlg.SelectedFont))
                            {
                                // the user has chosen a different (SIL) font - create a new EmbeddedFont
                                _defaultFont = new EmbeddedFont(dlg.SelectedFont);
                            }
                        }
                        else
                        {
                            // User cancelled - error out
                            return false;
                        }
                    }
                    // Copy the font file and update the CSS to reference it
                    string dest = Common.PathCombine(contentFolder, _defaultFont.Filename);
                    File.Copy(Path.Combine(EmbeddedFont.GetFontFolderPath(), _defaultFont.Filename), dest);
                    ReferenceEmbeddedFont(mergedCSS);
                }

                // copy over the XHTML and CSS files
                string cssPath = Common.PathCombine(contentFolder, defaultCSS);
                File.Copy(mergedCSS, cssPath);
                // copy the xhtml files into the content directory; if we can, give them scripture book names
                foreach (string file in htmlFiles)
                {
                    //string name = GetBookName(file);
                    string name = Path.GetFileName(file);
                    string substring = Path.GetFileNameWithoutExtension(file).Substring(8);
                    string dest = Common.PathCombine(contentFolder, "PartFile" + substring.PadLeft(3, '0') + ".xhtml");
                    File.Copy(file, dest);
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
            }
                
            // clean up and return
            Environment.CurrentDirectory = curdir;
            Cursor.Current = myCursor;
            return success;
        }

        #region Private Functions
        /// <summary>
        /// Inserts links to the default embedded font in the CSS file:
        /// - Adds a @font-face declaration referencing the .ttf file in the archive
        /// - Sets the font-family for the body element to the referenced font
        /// </summary>
        /// <param name="cssFile"></param>
        private void ReferenceEmbeddedFont (string cssFile)
        {
            if (!File.Exists(cssFile)) return;
            // read in the CSS file
            var reader = new StreamReader(cssFile);
            string content = reader.ReadToEnd();
            reader.Close();
            // build the @font-face elements
            var sb = new StringBuilder();
            sb.AppendLine("/* font-face info - added by SIL Pathway */");
            sb.AppendLine("@font-face {");
            sb.Append(" font-family : ");
            sb.Append(_defaultFont.Name);
            sb.AppendLine(";");
            sb.Append(" font-weight : ");
            sb.Append(_defaultFont.Weight);
            sb.AppendLine(";");
            sb.Append(" font-style : ");
            sb.Append(_defaultFont.Style);
            sb.AppendLine(";");
            sb.Append(" src : url(");
            sb.Append(Path.GetFileName(_defaultFont.Filename));
            sb.AppendLine(");");
            sb.AppendLine("}");
            sb.AppendLine("body {");
            sb.Append("font-family: '");
            sb.Append(_defaultFont.Name);
            sb.Append("', ");
            sb.AppendLine((_defaultFont.Serif) ? "serif;" : "sans-serif;");
            sb.AppendLine("}");
            sb.Append(content);
            // write out the updated CSS file
            var writer = new StreamWriter(cssFile);
            writer.Write(sb.ToString());
            writer.Close();
        }

        /// <summary>
        /// Returns the default font family for the writing system. If this information isn't retrievable,
        /// defaults to "Charis SIL"
        /// </summary>
        /// <param name="langCountry">ISO639-1 country code to look up</param>
        private void GetDefaultFontFamily(string langCountry)
        {
            string[] langCoun = langCountry.Split('-');

            try
            {
                if (langCoun.Length < 2)
                {
                    var wsPath = Common.PathCombine(Common.GetAllUserAppPath(), "SIL/WritingSystemStore/" + langCoun[0] + ".ldml");
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
                            _defaultFont = new EmbeddedFont(node.Value);
                            return;
                        }
                    }
                }
            }
            catch
            {
            }
            // unable to retrieve font family - give them "Charis SIL" as a backup
            _defaultFont = new EmbeddedFont("Charis SIL");

            return;
        }

        /// <summary>
        /// Returns the user-friendly book name inside this file.
        /// </summary>
        /// <param name="xhtmlFileName">Split xhtml filename in the form PartFile[#].xhtml</param>
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
                nodes = xmlDocument.SelectNodes("//xhtml:span[@class='scrBookName']", namespaceManager);
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
        /// Returns the primary language used inside this file.
        /// </summary>
        /// <param name="xhtmlFileName">File name to parse</param>
        /// <returns>Scripture book name (value of the scrBookName or a headword element in the xhtml file).</returns>
        private string GetLanguage(string xhtmlFileName)
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
            if (nodes != null && nodes.Count > 0)
            {
                var sb = new StringBuilder();
                sb.Append(nodes[0].Attributes["lang"].Value);
                return (sb.ToString());
            }
            // fall back on English (really a flag that something's wrong).
            return "en";
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
            Dictionary<string, string> mobilefeature = Param.GetItemsAsDictionary("//mobileProperty/mobilefeature");
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
            // # of files produced
            if (mobilefeature.ContainsKey("FileProduced"))
            {
                FileProduced = mobilefeature["FileProduced"].Trim();
            }
            else
            {
                FileProduced = "One";
            }
            // Subject
            if (mobilefeature.ContainsKey("Subject"))
            {
                Subject = mobilefeature["Subject"].Trim();
            }
            else
            {
                Subject = "Reference";
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
            opf.WriteElementString("dc", "subject", null, "Religion & Spirituality");
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
            if (projInfo.LanguageFilterKey.Length > 0)
                opf.WriteElementString("dc", "language", null, projInfo.LanguageFilterKey);
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
                opf.WriteStartElement("item"); // item (charis embedded font)
                opf.WriteAttributeString("id", "epub.embedded.font" + _defaultFont.Name);
                opf.WriteAttributeString("href", _defaultFont.Filename);
                opf.WriteAttributeString("media-type", "font/opentype/"); // TODO: works for SIL; what about others?
                opf.WriteEndElement(); // item
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
