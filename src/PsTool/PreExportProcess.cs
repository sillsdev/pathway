using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Windows.Forms;

namespace SIL.Tool
{
    public class PreExportProcess
    {
        private string _xhtmlFileNameWithPath;
        private string _cssFileNameWithPath;
        private string _cssRevFileNameWithPath;
        private string _xhtmlRevFileNameWithPath;
        private string _baseXhtmlFileNameWithPath;
        private PublicationInformation _projInfo;
        private StringBuilder _fileContent = new StringBuilder();
        bool isNodeFound = false;
        XmlNode returnNode = null;
        private string _skipChapterInformation = null;
	    public int pictureCount = 0;
        public const string CoverPageFilename = "File0Cvr.xhtml";
        public const string TitlePageFilename = "File1Ttl.xhtml";
        public const string CopyrightPageFilename = "File2Cpy.xhtml";
        public const string TableOfContentsFilename = "File3TOC.xhtml";

        public PreExportProcess()
        {

        }
        public PreExportProcess(PublicationInformation projInfo)
        {
            _xhtmlFileNameWithPath = projInfo.DefaultXhtmlFileWithPath;
            _baseXhtmlFileNameWithPath = projInfo.DefaultXhtmlFileWithPath;
            _cssFileNameWithPath = projInfo.DefaultCssFileWithPath;
            _xhtmlRevFileNameWithPath = string.Empty;
            _projInfo = projInfo;
            if (Param.Value.Count > 0 && Common.Testing == false)
                _projInfo.ProjectInputType = Param.Value[Param.InputType];

            // EDB 11/29/2011: removed method to fix invalid xhtml:
            // - There was only 1 block of code in there to fix a bug that had been fixed in 7.0.4
            // - This code gets called A LOT from different exports; the PSExport.Export class is probably
            //   a better place to put this kind of code cleanup, as it only gets called once, before the
            //   backends are launched.
            //FixInvalidXhtml();

            if (AppDomain.CurrentDomain.FriendlyName.ToLower() == "paratext.exe")
            {
                HandleTildeSymbolReplace(projInfo.DefaultXhtmlFileWithPath);
            }

        }
        public string ProcessedXhtml
        {
            get { return _xhtmlFileNameWithPath; }
        }
        public string ProcessedCss
        {
            get { return _cssFileNameWithPath; }
        }

        string tempFolder = Common.PathCombine(Path.GetTempPath(), "Preprocess");
        public string GetCreatedTempFolderPath
        {
            get { return tempFolder; }
        }

        public string SkipChapterInformation
        {
            get { return _skipChapterInformation; }
            set { _skipChapterInformation = value; }
        }

        #region Front Matter
        /// <summary>
        /// Call to insert the front matter, with the option of either creating separate files in the outputFolder or merging the xhtml
        /// into the preprocessed xhtml.
        /// </summary>
        /// <param name="outputFolder">temporary output folder</param>
        /// <param name="mergeFiles">Specifies whether to merge the xhtml into the preprocessed xhtml file</param>
        public List<string> InsertFrontMatter(string outputFolder, bool mergeFiles)
        {
            var files = new List<string>();
            try
            {
                // add the front matter pages to the temp folder as needed
                if ((Param.GetMetadataValue(Param.CopyrightPage) != null && Param.GetMetadataValue(Param.CopyrightPage).ToLower().Equals("true")) ||
                    (Param.GetMetadataValue(Param.CoverPage) != null && Param.GetMetadataValue(Param.CoverPage).ToLower().Equals("true")) ||
                    (Param.GetMetadataValue(Param.TitlePage) != null && Param.GetMetadataValue(Param.TitlePage).ToLower().Equals("true")) ||
                    (Param.GetMetadataValue(Param.TableOfContents) != null && Param.GetMetadataValue(Param.TableOfContents).ToLower().Equals("true")))
                {
                    // at least one front matter item selected
                    var sbPreamble = new StringBuilder();

                    if (Common.UnixVersionCheck())
                    {
                        sbPreamble.Append("<?xml version='1.0' encoding='utf-8'?><!DOCTYPE html[]>");
                    }
                    else
                    {
                        sbPreamble.Append("<?xml version='1.0' encoding='utf-8'?><!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Strict//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd'[]>");
                    }

                    sbPreamble.Append("<html xmlns='http://www.w3.org/1999/xhtml'><head><title>");
                    sbPreamble.Append(_projInfo.ProjectName);
                    sbPreamble.Append("</title><link rel='stylesheet' href='");
                    sbPreamble.Append(Path.GetFileName(_cssFileNameWithPath));
                    sbPreamble.AppendLine("' type='text/css' /></head>");
                    sbPreamble.Append("<body class='scrBody'>");
                    // copy over image resources if needed
                    if (Param.GetMetadataValue(Param.TitlePage).ToLower().Equals("true") ||
                        Param.GetMetadataValue(Param.CopyrightPage).ToLower().Equals("true"))
                    {
                        CopyCCResources(outputFolder);
                    }
                    var sb = new StringBuilder();
                    if (Param.GetMetadataValue(Param.CoverPage).ToLower().Equals("true"))
                    {
                        // cover image and file
                        CreateCoverImage(outputFolder);
                        if (mergeFiles)
                        {
                            // add to stringbuilder (for merging)
                            sb.Append(CoverImagePage());
                        }
                        else
                        {
                            // write to a separate file
                            var outFilename = Common.PathCombine(outputFolder, CoverPageFilename);
                            var outFile = new StreamWriter(outFilename);
                            outFile.Write(sbPreamble.ToString());
                            outFile.Write(CoverImagePage());
                            outFile.WriteLine("</body></html>");
                            outFile.Flush();
                            outFile.Close();
                            files.Add(outFilename);
                        }
                    }
                    if (Param.GetMetadataValue(Param.TitlePage).ToLower().Equals("true"))
                    {
                        // title page
                        if (mergeFiles)
                        {
                            sb.Append(TitlePage());
                        }
                        else
                        {
                            // write to a separate file
                            var outFilename = Common.PathCombine(outputFolder, TitlePageFilename);
                            var outFile = new StreamWriter(outFilename);
                            outFile.Write(sbPreamble.ToString());
                            outFile.Write(TitlePage());
                            outFile.WriteLine("</body></html>");
                            outFile.Flush();
                            outFile.Close();
                            files.Add(outFilename);
                        }
                    }
                    if (Param.GetMetadataValue(Param.CopyrightPage).ToLower().Equals("true"))
                    {
                        // title page
                        if (mergeFiles)
                        {
                            sb.Append(EmbedCopyrightPage(outputFolder));
                        }
                        else
                        {
                            // write to a separate file
                            var outFilename = Common.PathCombine(outputFolder, CopyrightPageFilename);
                            CopyCopyrightPage(outputFolder); // copyright page is a full xhtml file -- this method copies it over
                            files.Add(outFilename);
                        }
                    }
                    if (Param.TableOfContents.ToLower() == "table of contents")
                    {
                        // title page
                        if (mergeFiles)
                        {
                            sb.Append(TableOfContents());
                        }
                        else
                        {
                            // write to a separate file
                            var outFilename = Common.PathCombine(outputFolder, TableOfContentsFilename);
                            var outFile = new StreamWriter(outFilename);
                            outFile.Write(sbPreamble.ToString());
                            outFile.Write(TableOfContents());
                            outFile.WriteLine("</body></html>");
                            outFile.Flush();
                            outFile.Close();
                            files.Add(outFilename);
                        }
                    }

                    // if we're not merging, all our work is done -- just return
                    if (!mergeFiles)
                    {
                        return files;
                    }

                    // Merging files...
                    // open the xhtml file and create a temporary copy for writing)
                    if (!File.Exists(_xhtmlFileNameWithPath)) return files; // can't insert into the content
                    var sr = new StreamReader(_xhtmlFileNameWithPath);
                    var sw = new StreamWriter(_xhtmlFileNameWithPath + ".tmp");
                    var inData = sr.ReadToEnd();
                    var outData = new StringBuilder();
                    // search for the <body> element - we'll insert the front matter right after that
                    int start = inData.IndexOf("<body"); // start of <body>
                    int endIndex = inData.IndexOf(">", start); // end of <body>
                    outData.Append(inData.Substring(0, endIndex + 1));
                    outData.Append(sb.ToString());
                    outData.Append(inData.Substring(endIndex + 1));
                    sw.Write(outData.ToString());
                    sr.Close();
                    sw.Close();
                    // replace the original file with the new one
                    File.Delete(_xhtmlFileNameWithPath);
                    File.Move((_xhtmlFileNameWithPath + ".tmp"), _xhtmlFileNameWithPath);
                    files.Add(_xhtmlFileNameWithPath);
                }

                return files;

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return files;
            }
        }

        /// <summary>
        /// Returns a cover page xhtml snippet that will be inserted into the scripture or dictionary xhtml document
        /// </summary>
        /// <returns>XHTML snippet used to create a front image cover.</returns>
        public string CoverImagePage()
        {
            if (Param.GetMetadataValue(Param.CoverPage).ToLower().Equals("false")) { return string.Empty; }
            var sb = new StringBuilder();
            sb.AppendLine("<div id='CoverPage' class='Cover'><img src='cover.png' style='height: 100%; width: 100%;' alt='Cover image'/></div>");
            sb.AppendLine("<div class='Blank'></div>");
            return sb.ToString();
        }

        /// <summary>
        /// Task to replace tilde symbol to no-break space when the output produced from Paratext
        /// </summary>
        /// <param name="xhtmlFileName">Input XHTML file</param>
        private void HandleTildeSymbolReplace(string xhtmlFileName)
        {
            if (!File.Exists(xhtmlFileName))
            {
                return;
            }
            const string searchText = "~";
            const string replaceText = " "; // space for no-breaking word
            Common.StreamReplaceInFile(_xhtmlFileNameWithPath, searchText, replaceText);
        }

        /// <summary>
        /// Creates a cover image that is optionally "badged" with a title string. The resulting file
        /// is saved as "cover.png" in the specified output folder.
        /// </summary>
        /// <param name="outputFolder">Content folder the resulting file is saved to.</param>
        private void CreateCoverImage(string outputFolder)
        {

            bool isUnixOS = Common.UnixVersionCheck();

            if (Param.GetMetadataValue(Param.CoverPage).ToLower().Equals("false")) { return; }
            // open up the appropriate image for processing
            string strImageFile = Param.GetMetadataValue(Param.CoverPageFilename).Trim();
            if (strImageFile.Length < 1)
            {
                // no image file specified -- use the default image in the Graphic directory
                string strImageFolder = Common.PathCombine(Common.GetPSApplicationPath(), "Graphic");

				if (!Directory.Exists(strImageFolder))
				{
					strImageFolder = Common.PathCombine(Path.GetDirectoryName(Common.AssemblyPath), "Graphic");
				}
                strImageFile = Common.PathCombine(strImageFolder, "cover.png");
            }
            if (!File.Exists(strImageFile))
            {
                return;
            }
            // copy the image file to the destination folder as "cover.png"
            string dest = Common.PathCombine(outputFolder, "cover.png");
            var img = new Bitmap(strImageFile);
            img.Save(dest, System.Drawing.Imaging.ImageFormat.Png);
            // if we don't want a title, we're done
            if (Param.GetMetadataValue(Param.CoverPageTitle).ToLower().Equals("false") ||
                (Param.GetMetadataValue(Param.Title) != null && Param.GetMetadataValue(Param.Title).Trim().Length < 1))
            {
                // nothing else to do -- return
                return;
            }

            // if we got this far, we want to add a title "badge" to the cover image
            var bmp = new Bitmap(strImageFile);
            Graphics g = Graphics.FromImage(bmp);
            // We're going to be "badging" the book image with the Title metadata parameter
            var sb = new StringBuilder();
            sb.Append(Param.GetMetadataValue(Param.Title));
            var strTitle = sb.ToString();
            //var langCode = enumerator.Current.Key;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            var strFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            // figure out the dimensions of our rect based on the font info
            Font badgeFont = new Font("Times New Roman", 48);
			if(bmp.Width <= 599)
				badgeFont = new Font("Times New Roman", 42);
			if(bmp.Width <= 499)
				badgeFont = new Font("Times New Roman", 38);
			if (bmp.Width <= 399)
				badgeFont = new Font("Times New Roman", 32);
			if (bmp.Width <= 299)
				badgeFont = new Font("Times New Roman", 22);
			if (bmp.Width <= 199)
				badgeFont = new Font("Times New Roman", 14);

            SizeF size = g.MeasureString(strTitle, badgeFont, (bmp.Width - 20));
            int width = (int)Math.Ceiling(size.Width);
            int height = (int)Math.Ceiling(size.Height);

            if (isUnixOS)
            {
                width += 80;
                height += 50;
                badgeFont = new Font("Times New Roman", 36);
                size = g.MeasureString(strTitle, badgeFont, (bmp.Width - 20));
                size.Height += 20;
            }

            // create the bounding rect, centered horizontally on the image
            //Rectangle rect = new Rectangle(((bmp.Size.Width / 2) - (width / 2)), 100, width, height);

	        if (bmp.Height >= 600)
	        {
				SetImageRectangleText(bmp, width, height, g, strTitle, badgeFont, size, strFormat, 100, 100f, outputFolder);
	        }
	        else if (bmp.Height >= 500)
	        {
				SetImageRectangleText(bmp, width, height, g, strTitle, badgeFont, size, strFormat, 80, 80f, outputFolder);
	        }
	        else if (bmp.Height >= 400)
	        {
				SetImageRectangleText(bmp, width, height, g, strTitle, badgeFont, size, strFormat, 60, 60f, outputFolder);
	        }
	        else if (bmp.Height >= 300)
	        {
				SetImageRectangleText(bmp, width, height, g, strTitle, badgeFont, size, strFormat, 40, 40f, outputFolder);
	        }
	        else if (bmp.Height >= 200)
	        {
				SetImageRectangleText(bmp, width, height, g, strTitle, badgeFont, size, strFormat, 30, 30f, outputFolder);
	        }
			else if (bmp.Height >= 50)
			{
				SetImageRectangleText(bmp, width, height, g, strTitle, badgeFont, size, strFormat, 20, 20f, outputFolder);
			}

        }

		private static Rectangle SetImageRectangleText(Bitmap bmp, int width, int height, Graphics g, string strTitle,
			Font badgeFont, SizeF size, StringFormat strFormat, int lineHeight, float heightSize, string outputFolder)
	    {
		    Rectangle rect;
			rect = new Rectangle(((bmp.Size.Width / 2) - (width / 2)), lineHeight, width, height);
		    // draw the badge (rect and string)
		    g.FillRectangle(Brushes.Brown, rect);
		    g.DrawRectangle(Pens.Gold, rect);
		    g.DrawString(strTitle, badgeFont, Brushes.Gold,
				new RectangleF(new PointF(((bmp.Size.Width / 2) - (size.Width / 2)), heightSize), size), strFormat);

			string strCoverImageFile = Common.PathCombine(outputFolder, "cover.png");
			bmp.Save(strCoverImageFile, System.Drawing.Imaging.ImageFormat.Png);

			return rect;
	    }

	    public string TitlePage()
        {
            if (Param.GetMetadataValue(Param.TitlePage).ToLower().Equals("false")) { return string.Empty; }
            var sb = new StringBuilder();
            sb.Append("<div id='TitlePage' class='Title'><h1>");

            if (Param.GetMetadataValue(Param.Title) != null)
                sb.Append(Common.ReplaceSymbolToText(Param.GetMetadataValue(Param.Title)));

            sb.AppendLine("<br /><br /><br /><br /><br /><br /><br />");

            sb.AppendLine("</h1>");
            sb.Append("<p class='Publisher'>");

            if (Param.GetMetadataValue(Param.Publisher) != null)
                sb.Append(Common.ReplaceSymbolToText(Param.GetMetadataValue(Param.Publisher)));

            sb.AppendLine("</p>");

            // logo stuff
            sb.Append("<p class='logo'>");
            if (Param.GetOrganization().StartsWith("SIL"))
            {
                if (_projInfo.ProjectInputType.ToLower() == "dictionary")
                {
                    // dictionary - SIL logo
                    sb.Append("<img src='2014_sil_logo.png' alt='SIL International Logo'/>");
                }
                else
                {
                    // Scripture - WBT logo
                    sb.Append("<img src='WBT_H_RGB_red.png' alt='Wycliffe Logo'/>");
                }
            }
            else if (Param.GetOrganization().StartsWith("Wycliffe"))
            {
                sb.Append("<img src='WBT_H_RGB_red.png' alt='Wycliffe Logo'/>");
            }
            sb.AppendLine("</p>");
            sb.AppendLine("</div>");
            return sb.ToString();
        }

        public XmlNode LoTitlePage(XmlDocument xmldoc)
        {
            XmlNode mainNode = xmldoc.CreateElement("div");
            XmlNode tNode = xmldoc.CreateElement("div");
            XmlAttribute xmlAttribute = xmldoc.CreateAttribute("class");
            xmlAttribute.Value = "title";
            tNode.Attributes.Append(xmlAttribute);
            tNode.InnerText = Param.GetMetadataCurrentValue(Param.Title);
            mainNode.AppendChild(tNode);
            XmlNode t4Node = xmldoc.CreateElement("div");
            XmlAttribute xmlAttribute2 = xmldoc.CreateAttribute("class");
            xmlAttribute2.Value = "logo";
            t4Node.Attributes.Append(xmlAttribute2);
            t4Node.InnerText = Common.ReplaceSymbolToText(Param.GetMetadataCurrentValue(Param.Publisher));
            mainNode.AppendChild(t4Node);
            return mainNode;
        }

        /// <summary>
        /// Copies the Creative Commons images, etc. from the Copyrights folder into the temp output folder
        /// </summary>
        /// <param name="outputFolder"></param>
        private void CopyCCResources(string outputFolder)
        {
            string strCopyrightFolder = Common.PathCombine(Common.GetPSApplicationPath(), "Copyrights");
			if (!Directory.Exists(strCopyrightFolder))
			{
				strCopyrightFolder = Common.PathCombine(Path.GetDirectoryName(Common.AssemblyPath), "Copyrights");
			}
            // copy over supporting files from the Copyright folder
            // rights logos - copyright page only
            if (Param.GetMetadataValue(Param.CopyrightPage).ToLower().Equals("true"))
            {
                File.Copy(Common.PathCombine(strCopyrightFolder, "by.png"), Common.PathCombine(outputFolder, "by.png"), true);
                File.Copy(Common.PathCombine(strCopyrightFolder, "sa.png"), Common.PathCombine(outputFolder, "sa.png"), true);
                File.Copy(Common.PathCombine(strCopyrightFolder, "nc.png"), Common.PathCombine(outputFolder, "nc.png"), true);
                File.Copy(Common.PathCombine(strCopyrightFolder, "nd.png"), Common.PathCombine(outputFolder, "nd.png"), true);
            }
            // logo - both Title and Copyright page
            if (Param.GetOrganization().StartsWith("SIL"))
            {
                if (_projInfo.ProjectInputType.ToLower() == "dictionary")
                {
                    File.Copy(Common.PathCombine(strCopyrightFolder, "2014_sil_logo.png"), Common.PathCombine(outputFolder, "2014_sil_logo.png"), true);
                }
                else
                {
                    File.Copy(Common.PathCombine(strCopyrightFolder, "WBT_H_RGB_red.png"), Common.PathCombine(outputFolder, "WBT_H_RGB_red.png"), true);
                }
            }
            else if (Param.GetOrganization().StartsWith("Wycliffe"))
            {
                File.Copy(Common.PathCombine(strCopyrightFolder, "WBT_H_RGB_red.png"), Common.PathCombine(outputFolder, "WBT_H_RGB_red.png"), true);
            }
            File.Copy(Common.PathCombine(strCopyrightFolder, "Copy.css"), Common.PathCombine(outputFolder, "Copy.css"), true);
        }

        public void CopyCopyrightPage(string outputFolder)
        {
            if (Param.GetMetadataValue(Param.CopyrightPage).ToLower().Equals("false") ||
                Param.GetMetadataValue(Param.CopyrightPageFilename).Length < 1)
            {
                return;
            }
            string strCopyrightFolder = Common.PathCombine(Common.GetPSApplicationPath(), "Copyrights");
			if (!Directory.Exists(strCopyrightFolder))
			{
				strCopyrightFolder = Common.PathCombine(Path.GetDirectoryName(Common.AssemblyPath), "Copyrights");
			}
            // open up the copyright / license file
            string strFilename = Param.GetMetadataValue(Param.CopyrightPageFilename);
            if (strCopyrightFolder == null || strFilename == null)
            {
                return;
            }
            string strCopyrightFile = Common.PathCombine(strCopyrightFolder, strFilename);
            if (!File.Exists(strCopyrightFile))
            {
                return; // something went wrong -- get out
            }
            string destFile = Common.PathCombine(outputFolder, "File2Cpy.xhtml");
            if (File.Exists(destFile))
            {
                File.Delete(destFile);
            }

            File.Copy(strCopyrightFile, destFile, true);
            if (Common.UnixVersionCheck())
            {
                Common.RemoveDTDForLinuxProcess(destFile, "epub");
            }
            InsertCopyrightImageFiles(destFile, strCopyrightFile);
            var languageCode = JustLanguageCode();
            Common.StreamReplaceInFile(destFile, "<span class='LanguageName'></span>", Common.GetLanguageName(languageCode));
            Common.StreamReplaceInFile(destFile, "<span class='LanguageCode'></span>", languageCode);
            Common.StreamReplaceInFile(destFile, "<span class='LanguageUrl'></span>", GetLanguageUrl(languageCode));
            Common.StreamReplaceInFile(destFile, "div id='OtherCopyrights' class='Front_Matter' dir='ltr'>", GetCopyrightInfo());
            if (_projInfo.ProjectInputType.ToLower() != "dictionary")
            {
                Common.StreamReplaceInFile(destFile, "src='2014_sil_logo.png' alt='SIL International logo'",
                    "src='WBT_H_RGB_red.png' alt='Wycliffe logo'  ");
            }
            Common.SetDefaultCSS(destFile, Path.GetFileName(_cssFileNameWithPath));
        }

        private string JustLanguageCode()
        {
            string languageCode = Common.GetLanguageCode(_xhtmlFileNameWithPath, _projInfo.ProjectInputType, true);
            if (languageCode.Contains(":"))
            {
                languageCode = languageCode.Substring(0, languageCode.IndexOf(':'));
            }
            return languageCode;
        }

        private void InsertCopyrightImageFiles(string copyrighthtmlfile, string copyFromLocation)
        {
            if (!File.Exists(copyrighthtmlfile)) return;
            try
            {
                XmlTextReader _reader = Common.DeclareXmlTextReader(copyrighthtmlfile, true);
                while (_reader.Read())
                {
                    if (_reader.NodeType == XmlNodeType.Element)
                    {
                        if (_reader.Name == "img")
                        {
                            string id = _reader.GetAttribute("src");
                            var srcValue = id;
                            if (srcValue != null)
                            {
                                var sourceFile = srcValue;
                                string pictureDirectory = Path.GetDirectoryName(copyFromLocation);
                                sourceFile = Common.PathCombine(pictureDirectory, sourceFile);
                                if (sourceFile.Length > 0)
                                {
                                    if (File.Exists(sourceFile))
                                    {
                                        string destinationFile = Path.GetDirectoryName(copyrighthtmlfile);
                                        destinationFile = Common.PathCombine(destinationFile, srcValue);
                                        File.Copy(sourceFile, destinationFile, true);
                                    }
                                }
                            }
                        }
                    }
                }
                _reader.Close();
            }
            catch
            {
            }
        }

        public List<string> GetMultiPictureEntryId(string fileName)
        {
            string entryId = null;
            bool isfirstImage = false;
            List<string> entryIdList = new List<string>();
            if (!File.Exists(fileName)) return null;
            try
            {
                XmlTextReader _reader = Common.DeclareXmlTextReader(fileName, true);
                while (_reader.Read())
                {
                    if (_reader.NodeType == XmlNodeType.Element)
                    {
                        if (_reader.Name == "div")
                        {
                            string clsName = _reader.GetAttribute("class");
                            string id = _reader.GetAttribute("id");
                            if (clsName != null)
                            {
                                if (clsName == "entry")
                                {
                                    isfirstImage = false;
                                    entryId = id;
                                }
                            }
                        }
                        else if (_reader.Name == "img")
                        {
                            if (!isfirstImage)
                            {
                                isfirstImage = true;
                            }
                            else
                            {
                                if (entryId != null && !entryIdList.Contains(entryId))
                                {
                                    entryIdList.Add(entryId);
                                }
                            }
                        }
                    }
                }
                _reader.Close();
            }
            catch
            {
            }
            return entryIdList;
        }

        /// <summary>
        /// Returns the string contents of the copyright / license xhtml for inserting into the dictionary / scripture data.
        /// </summary>
        public string EmbedCopyrightPage(string outputFolder)
        {
            if (Param.GetMetadataValue(Param.CopyrightPage).ToLower().Equals("false") ||
                Param.GetMetadataValue(Param.CopyrightPageFilename).Length < 1) { return string.Empty; }
            string strCopyrightFolder = Common.PathCombine(Common.GetPSApplicationPath(), "Copyrights");
			if (!Directory.Exists(strCopyrightFolder))
			{
				strCopyrightFolder = Common.PathCombine(Path.GetDirectoryName(Common.AssemblyPath), "Copyrights");
			}
            // open up the copyright / license file
            string strFilename = Param.GetMetadataValue(Param.CopyrightPageFilename);
            if (strCopyrightFolder == null || strFilename == null)
            {
                return string.Empty;
            }
            string strCopyrightFile = Common.PathCombine(strCopyrightFolder, strFilename);
            if (!File.Exists(strCopyrightFile))
            {
                return string.Empty; // something went wrong -- get out
            }

            var sr = new StreamReader(strCopyrightFile);
            var inData = sr.ReadToEnd();
            var outData = new StringBuilder();
            // search for the <body> element - we're just interested in what's inside the element
            int start = inData.IndexOf("<body"); // start of <body>
            int startIndex = inData.IndexOf(">", start); // end of <body>
            int endIndex = inData.IndexOf("</body>");
            outData.Append("<div id='CopyrightPage' class='Copyright'>");
            outData.Append(inData.Substring(startIndex + 1, (endIndex - (startIndex + 1))));
            outData.Append("</div>");
            sr.Close();
            // add the language and copyright info and return the result as a string
            string s0;
            if (_projInfo.ProjectInputType.ToLower() != "dictionary")
            {
                s0 = Regex.Replace(outData.ToString(), "src='2014_sil_logo.png' alt='SIL International logo'",
                    "src='WBT_H_RGB_red.png' alt='Wycliffe logo'  ");
            }
            else
            {
                s0 = outData.ToString();
            }
            string languageCode = JustLanguageCode();
            var s1 = Regex.Replace(s0, "<span class='LanguageName'></span>", Common.GetLanguageName(languageCode));
            var s2 = Regex.Replace(s1, "<span class='LanguageCode'></span>", languageCode);
            var s3 = Regex.Replace(s2, "<span class='LanguageUrl'></span>", GetLanguageUrl(languageCode));
            return (Regex.Replace(s3, "div id='OtherCopyrights' class='Front_Matter'>", GetCopyrightInfo()));
        }

        private string GetLanguageUrl(string languageCode)
        {
            var sb = new StringBuilder();
            sb.Append("<a href='http://www.ethnologue.com/language/");
            var codeLen = languageCode.Length > 3 ? 3 : languageCode.Length;
            sb.Append(languageCode.Substring(0, codeLen));
            sb.Append("'>http://www.ethnologue.com/language/");
            sb.Append(languageCode.Substring(0, codeLen));
            sb.Append("</a>");
            return sb.ToString();
        }

        // Returns the copyright information for this document as an XHTML snippet (not well formed). This is meant to be used
        // in conjunction with the StreamReplaceInFile() call in CreateCopyrightPage().
        private string GetCopyrightInfo()
        {
            var sb = new StringBuilder();
            sb.Append("div id='OtherCopyrights' class='Front_Matter' dir='ltr'><p>");
            // append any other copyright information to the list
            string contributors = Param.GetMetadataValue(Param.Contributor);
            if (contributors.Trim().Length > 0)
            {
                sb.Append("(");
                sb.Append(contributors);
                sb.Append("), ");
            }
            string rights = Param.GetMetadataValue(Param.CopyrightHolder);
            //http://stackoverflow.com/questions/501671/superscript-in-css-only
            rights = rights.Replace("\u00ae", "<span style='position: relative; top: -0.5em; font-size: 80%;'>\u00ae</span>");
            if (rights.Trim().Length > 0)
            {
                sb.Append(Common.UpdateCopyrightYear(rights.Replace("&", "&amp;")));
                sb.Append("</p> ");
            }
            return sb.ToString();
        }

        // Returns the copyright information for this document as an XHTML snippet (not well formed). This is meant to be used
        // in conjunction with the StreamReplaceInFile() call in CreateCopyrightPage().
        private string GetCopyrightInfoForLO()
        {
            var sb = new StringBuilder();
            sb.Append("div id='OtherCopyrights' class='Front_Matter' dir='ltr'><span class='LText'>");
            // append any other copyright information to the list
            string contributors = Param.GetMetadataValue(Param.Contributor);
            if (contributors.Trim().Length > 0)
            {
                sb.Append("(");
                sb.Append(contributors);
                sb.Append("), ");
            }
            string rights = Param.GetMetadataValue(Param.CopyrightHolder);
            if (rights.Trim().Length > 0)
            {
                sb.Append(Common.UpdateCopyrightYear(rights.Replace("&", @"\u0026")));
                sb.Append("</span> ");
            }
            return sb.ToString();
        }

        private string TableOfContents()
        {
            //Reversal XHTML file
            if (_xhtmlFileNameWithPath != null)
                _xhtmlRevFileNameWithPath = Common.PathCombine(Path.GetDirectoryName(_xhtmlFileNameWithPath), "FlexRev.xhtml");

            // sanity checks
            if (Param.GetMetadataValue(Param.TableOfContents).ToLower().Equals("false")) { return string.Empty; }
            if (!File.Exists(_xhtmlFileNameWithPath)) return string.Empty; // can't obtain list of books / letters
            // load the xhtml file we're working with

            XmlDocument xmlDocument = Common.DeclareXMLDocument(true);
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, DtdProcessing = DtdProcessing.Parse};
            XmlReader xmlReader = XmlReader.Create(_xhtmlFileNameWithPath, xmlReaderSettings);
            xmlDocument.Load(xmlReader);
            xmlReader.Close();
            var sb = new StringBuilder();
            sb.AppendLine("<!-- Contents page -->");
            sb.AppendLine("<div id='TOCPage' class='Contents'>");

            if (_xhtmlFileNameWithPath.ToLower().Contains("main") || _projInfo.ProjectInputType.ToLower() == "scripture") //TocError
            {
                if (_projInfo.ProjectInputType.ToLower() == "dictionary")
                {
                    sb.AppendLine("<h1>Table of Contents</h1><h2>Main</h2>");
                }
                else
                {
                    sb.AppendLine("<h1>Table of Contents</h1>");
                }
                // collect book names

                XmlNodeList bookIDs, bookNames;
                if (_projInfo.ProjectInputType.ToLower() == "dictionary")
                {
                    // for dictionaries, the letter is used both for the ID and name
                    bookIDs = xmlDocument.SelectNodes("//xhtml:div[@class='letter']", namespaceManager);
                    bookNames = bookIDs;
                }
                else
                {
                    // for scripture, scrBookCode contains the preferred ID while the Title_Main contains the preferred / localized name
                    // 1. Book ID: start out with the book code (e.g., 2CH for 2 Chronicles)
                    bookIDs = xmlDocument.SelectNodes("//xhtml:span[@class='scrBookCode']", namespaceManager);
                    if (bookIDs == null || bookIDs.Count == 0)
                    {
                        // no book code - try scrBookName
                        bookIDs = xmlDocument.SelectNodes("//xhtml:span[@class='scrBookName']", namespaceManager);
                    }
                    if (bookIDs == null || bookIDs.Count == 0)
                    {
                        // no scrBookName - try Title_Main
                        bookIDs = xmlDocument.SelectNodes("//xhtml:div[@class='Title_Main']/span", namespaceManager);
                    }
                    // 2. Book Name: start out with Title_Main
                    bookNames = xmlDocument.SelectNodes("//xhtml:div[@class='Title_Main']/span", namespaceManager);
                    if (bookNames == null || bookNames.Count == 0)
                    {
                        // nothing there - check on the scrBookName span
                        bookNames = xmlDocument.SelectNodes("//xhtml:span[@class='scrBookName']", namespaceManager);
                    }
                    if (bookNames == null || bookNames.Count == 0)
                    {
                        // nothing there - check on the scrBookCode span
                        bookNames = xmlDocument.SelectNodes("//xhtml:span[@class='scrBookCode']", namespaceManager);
                    }
                }
                if (bookIDs != null && bookIDs.Count > 0)
                {
                    int index = 0;
                    sb.AppendLine("<ul>");
                    foreach (XmlNode bookId in bookIDs)
                    {
                        // each entry should look something like this:
                        //    <li><a href="#idMRK">Das Evangelium nach Markus</a></li>
                        sb.Append("<li><a href='");
                        // remove whitespace
                        string indexValue = String.Format("{0:00000}", index + 1);
                        string fileNameIndex = "PartFile" + indexValue + "_.xhtml";

                        sb.Append(new Regex(@"\s*").Replace(fileNameIndex, string.Empty));
                        sb.Append("'>");
                        sb.Append(bookNames[index].InnerText);
                        sb.AppendLine("</a>");

                        if (SkipChapterInformation != null)
                        {
                            bool skipChapter = SkipChapterInformation.StartsWith("1");

                            if (!skipChapter)
                            {
                                if (bookId.ParentNode == null) break;
                                XmlNodeList chapterSectionIDs;
                                if (_projInfo.ProjectInputType.ToLower() == "dictionary")
                                {
                                    // for dictionaries, the letter is used both for the ID and name
                                    chapterSectionIDs = bookId.ParentNode.SelectNodes(".//xhtml:div[@class='Section_Head']",
                                                                                namespaceManager);
                                }
                                else
                                {
                                    // for scripture, scrBookCode contains the preferred ID while the Title_Main contains the preferred / localized name
                                    // 1. Book ID: start out with the book code (e.g., 2CH for 2 Chronicles)
                                    chapterSectionIDs = bookId.ParentNode.SelectNodes(".//xhtml:span[@class='Section_Head']",
                                                                                namespaceManager);
                                    if (chapterSectionIDs == null || chapterSectionIDs.Count == 0)
                                    {
                                        // no book code - try scrBookName
                                        chapterSectionIDs = bookId.ParentNode.SelectNodes(".//xhtml:div[@class='Section_Head']",
                                                                                    namespaceManager);
                                    }
                                }

                                if (chapterSectionIDs != null && chapterSectionIDs.Count > 0)
                                {
                                    sb.AppendLine("<ul>");
                                    foreach (XmlNode chapterSectionID in chapterSectionIDs)
                                    {
                                        sb.Append("<li><a href='");
                                        // remove whitespace
                                        string indexValues = String.Format("{0:00000}", index + 1);
                                        string fileNameIndexs = "PartFile" + indexValues + "_.xhtml#" +
                                                                chapterSectionID.Attributes["id"].Value;
                                        sb.Append(new Regex(@"\s*").Replace(fileNameIndexs, string.Empty));
                                        sb.Append("'>");
                                        sb.Append(chapterSectionID.InnerText);
                                        sb.AppendLine("</a>");
                                        sb.AppendLine("</li>");
                                    }
                                    sb.AppendLine("</ul>");
                                }
                            }
                        }
                        sb.AppendLine("</li>");
                        index++;
                    }
                    sb.AppendLine("</ul>");
                }

            }
            else
            {
                if (_projInfo.ProjectInputType.ToLower() == "dictionary")
                {
                    sb.AppendLine("<h1>Table of Contents</h1>");
                }
            }

            if (File.Exists(_xhtmlRevFileNameWithPath))
            {
                //Reversal Code here starts
                xmlReader = XmlReader.Create(_xhtmlRevFileNameWithPath, xmlReaderSettings);
                xmlDocument.Load(xmlReader);
                xmlReader.Close();
                XmlNodeList revBookIDs = null, revBookNames = null;
                if (_projInfo.ProjectInputType.ToLower() == "dictionary")
                {
                    // for dictionaries, the letter is used both for the ID and name
                    revBookIDs = xmlDocument.SelectNodes("//xhtml:div[@class='letter']", namespaceManager);
                    revBookNames = revBookIDs;
                }
                if (revBookIDs != null && revBookIDs.Count > 0)
                {
                    int index = 0;
                    sb.AppendLine("<h2>Reversal Index</h2>");
                    sb.AppendLine("<ul>");
                    foreach (XmlNode revBookId in revBookIDs)
                    {
                        // each entry should look something like this:
                        //    <li><a href="#idMRK">Das Evangelium nach Markus</a></li>
                        sb.Append("<li><a href='");
                        // remove whitespace
                        string indexValue = String.Format("{0:00000}", index + 1);
                        string fileNameIndex = "RevIndex" + indexValue + "_.xhtml";

                        sb.Append(new Regex(@"\s*").Replace(fileNameIndex, string.Empty));
                        sb.Append("'>");
                        sb.Append(revBookNames[index].InnerText);
                        sb.AppendLine("</a>");
                        sb.AppendLine("</li>");
                        index++;
                    }
                    sb.AppendLine("</ul>");
                }
                //Reversal Code here ends
            }

            // close out
            //sb.AppendLine("</ul>");
            sb.AppendLine("</div>");

            return sb.ToString();
        }

        #endregion

        #region LibreOfficeFrontMatter
        public void InsertLoFrontMatterContent(string inputXhtmlFilePath, bool isMainAlone)
        {
            Param.LoadSettings();
            string organization;
            string frontMatterXHTMLContent = string.Empty;
            string frontMatterCSSStyle = string.Empty;
            try
            {
                organization = Param.Value.ContainsKey("Organization")
                                   ? Param.Value["Organization"]
                                   : "SIL International";
            }
            catch (Exception)
            {
                organization = "SIL International";
            }

            bool _coverImage = (Param.GetMetadataValue(Param.CoverPage, organization) == null) ? false : Boolean.Parse(Param.GetMetadataValue(Param.CoverPage, organization));
            string _coverPageImagePath = Param.GetMetadataValue(Param.CoverPageFilename, organization);
            bool _includeTitlePage = (Param.GetMetadataValue(Param.TitlePage, organization) == null) ? false : Boolean.Parse(Param.GetMetadataValue(Param.TitlePage, organization));
            bool _copyrightInformation = (Param.GetMetadataValue(Param.CopyrightPage, organization) == null) ? false : Boolean.Parse(Param.GetMetadataValue(Param.CopyrightPage, organization));
            string copyRightFilePath = Param.GetMetadataValue(Param.CopyrightPageFilename, organization);
            bool _includeTitleinCoverImage = (Param.GetMetadataValue(Param.CoverPageTitle, organization) == null) ? false : Boolean.Parse(Param.GetMetadataValue(Param.CoverPageTitle, organization));
            bool _includeTOCPage = (Param.GetMetadataValue(Param.TableOfContents, organization) == null) ? false : Boolean.Parse(Param.GetMetadataValue(Param.TableOfContents, organization));
            if (!File.Exists(inputXhtmlFilePath)) return;
            const string tag = "body";

            try
            {
                XmlDocument xmldoc = Common.DeclareXMLDocument(true);
                XmlNode coverImageNode = null;
                XmlNode coverTitleNode = null;
                FileStream fs = File.OpenRead(inputXhtmlFilePath);
                xmldoc.Load(fs);
                fs.Close();
                XmlNodeList mainXhtmlFile = xmldoc.GetElementsByTagName(tag);


                XmlNode dummyNode = null;
                dummyNode = xmldoc.CreateElement("div");
                XmlAttribute xmlDAttribute = xmldoc.CreateAttribute("class");
                xmlDAttribute.Value = "dummypage";
                dummyNode.Attributes.Append(xmlDAttribute);

                XmlNode pNode = null;
                pNode = xmldoc.CreateElement("div");
                xmlDAttribute = xmldoc.CreateAttribute("class");
                xmlDAttribute.Value = "P4";
                pNode.Attributes.Append(xmlDAttribute);


                //COVER IMAGE
                if (_coverImage && File.Exists(_coverPageImagePath))
                {
                    coverImageNode = xmldoc.CreateElement("div");
                    XmlAttribute xmlAttribute = xmldoc.CreateAttribute("class");
                    xmlAttribute.Value = "coverImage";
                    coverImageNode.Attributes.Append(xmlAttribute);

                    XmlNode newNodeImg = xmldoc.CreateElement("img");
                    xmlAttribute = xmldoc.CreateAttribute("src");
                    xmlAttribute.Value = _coverPageImagePath;
                    newNodeImg.Attributes.Append(xmlAttribute);

                    xmlAttribute = xmldoc.CreateAttribute("alt");
                    xmlAttribute.Value = _coverPageImagePath;
                    newNodeImg.Attributes.Append(xmlAttribute);
                    coverImageNode.AppendChild(newNodeImg);

                    coverTitleNode = xmldoc.CreateElement("div");
                    xmlAttribute = xmldoc.CreateAttribute("class");
                    xmlAttribute.Value = "cover";
                    coverTitleNode.Attributes.Append(xmlAttribute);
                    coverTitleNode.InnerText = " ";

                    if (_includeTitleinCoverImage)
                    {
                        coverTitleNode.InnerText = Param.GetMetadataCurrentValue(Param.Title);
                    }
                }
                //COVER TITLE
                if (coverTitleNode != null)
                {
                    frontMatterXHTMLContent = coverTitleNode.OuterXml;
                    _projInfo.IsFrontMatterEnabled = true;
                    frontMatterCSSStyle = frontMatterCSSStyle + ".cover{margin-top: 112pt; text-align: center; font-size:18pt; font-weight:bold;page-break-after: always;font-family: 'Times New Roman', serif; } ";
                }
                //END OF COVER TITLE
                if (coverImageNode != null)
                {
                    frontMatterXHTMLContent = coverImageNode.OuterXml + frontMatterXHTMLContent + dummyNode.OuterXml;
                    _projInfo.IsFrontMatterEnabled = true;
                }
                //END OF COVER IMAGE
                //TITLE
                XmlNode titleNode = null;
                if (_includeTitlePage)
                {
                    _projInfo.IsTitlePageEnabled = true;
                    titleNode = LoTitlePage(xmldoc);
                }

                if (titleNode != null)
                {
                    frontMatterXHTMLContent = frontMatterXHTMLContent + titleNode.OuterXml;
                    //if (_includeTOCPage)
                    //{
                    //    //frontMatterXHTMLContent = frontMatterXHTMLContent + dummyNode.OuterXml;
                    //    frontMatterXHTMLContent = frontMatterXHTMLContent;
                    //}
                    _projInfo.IsFrontMatterEnabled = true;
                    frontMatterCSSStyle = frontMatterCSSStyle + ".title{margin-top: 112pt; text-align: center; font-family: 'Times New Roman', serif; font-weight:bold;font-size:18pt;} .publisher{text-align: center;font-size:14pt;font-family: 'Times New Roman', serif; } .logo{page-break-after: always; text-align:center; clear:both;float:bottom;}";
                }
                //END OF TITLE
                //COPYRIGHT
                XmlNode copyRightContentNode = null;
                if (File.Exists(copyRightFilePath))
                {
                    if (_copyrightInformation)
                    {
                        try
                        {
                            string draftTempFileName = Common.PathCombine(Path.GetTempPath(), Path.GetFileName(copyRightFilePath));
                            File.Copy(copyRightFilePath, draftTempFileName, true);
                            var languageCode = JustLanguageCode();
                            Common.StreamReplaceInFile(draftTempFileName, "<span class='LanguageName'></span>", Common.GetLanguageName(languageCode));
                            Common.StreamReplaceInFile(draftTempFileName, "<span class='LanguageCode'></span>", languageCode);
                            Common.StreamReplaceInFile(draftTempFileName, "<span class='LanguageUrl'></span>", GetLanguageUrl(languageCode));
                            Common.StreamReplaceInFile(draftTempFileName,
                                                       "div id='OtherCopyrights' class='Front_Matter' dir='ltr'>",
                                                       GetCopyrightInfoForLO());
                            Common.StreamReplaceInFile(draftTempFileName, "<h1>", "<span class='LHeading'>");
                            Common.StreamReplaceInFile(draftTempFileName, "</h1>", "</span>");
                            Common.StreamReplaceInFile(draftTempFileName, "<p>", "<span class='LText'>");
                            Common.StreamReplaceInFile(draftTempFileName, "</p>", "</span>");
                            Common.StreamReplaceInFile(draftTempFileName, "<em>", "<span>");
                            Common.StreamReplaceInFile(draftTempFileName, "</em>", "</span>");

                            XmlDocument xDoc = Common.DeclareXMLDocument(true);
                            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmldoc.NameTable);
                            namespaceManager.AddNamespace("html", "http://www.w3.org/1999/xhtml");
                            xDoc.Load(draftTempFileName);
                            XmlNode bodyNode = xDoc.SelectSingleNode("//html:body", namespaceManager);

                            copyRightContentNode = xmldoc.CreateElement("div");

                            XmlNode importNode = copyRightContentNode.OwnerDocument.ImportNode(bodyNode, true);

                            copyRightContentNode.InnerXml = importNode.InnerXml;
                        }
                        catch
                        {
                            copyRightContentNode = xmldoc.CreateElement("div");
                            copyRightContentNode.InnerXml = " ";
                        }
                    }

                    if (copyRightContentNode != null && _copyrightInformation)
                    {
                        frontMatterXHTMLContent = frontMatterXHTMLContent + "<div id='copyright' class='copyright' dir='ltr'>.</div>" + copyRightContentNode.OuterXml + "<div id='dummyTOC' class='dummyTOC' dir='ltr'>.</div>";
                        _projInfo.IsFrontMatterEnabled = true;

                        frontMatterCSSStyle = frontMatterCSSStyle + ".copyright{text-align: left; font-size:1pt;visibility:hidden;font-family: 'Times New Roman', serif;}.LHeading{font-size:18pt;font-weight:bold;line-height:14pt;margin-bottom:.25in;font-family: 'Times New Roman', serif;}.LText{font-size:12pt;font-style:italic;font-family: 'Times New Roman', serif;}.LText:before{content: \"\\2028\"}.dummyTOC{font-size:1pt; page-break-after: always;} ";
                    }
                }
                //END OF COPYRIGHT
                //TABLE OF CONTENTS
                XmlNode tocNode = null;
                if (_includeTOCPage)
                {
                    tocNode = xmldoc.CreateElement("div");
                    XmlAttribute xmlAttribute = xmldoc.CreateAttribute("class");
                    xmlAttribute.Value = "tableofcontents";
                    tocNode.Attributes.Append(xmlAttribute);
                    tocNode.InnerText = Param.GetMetadataValue(Param.TableOfContents);
                }

                if (tocNode != null)
                {
                    frontMatterCSSStyle = frontMatterCSSStyle +
                                          ".TableOfContentLO{visibility:hidden;}";

                    frontMatterXHTMLContent = frontMatterXHTMLContent + tocNode.OuterXml + pNode.OuterXml;
                    _projInfo.IsFrontMatterEnabled = true;
                }
                //END OF TABLE OF CONTENTS
                if (frontMatterXHTMLContent.Trim().Length > 0)
                {
                    if (isMainAlone)
                    {
                        frontMatterXHTMLContent = frontMatterXHTMLContent + pNode.OuterXml;
                    }
                    mainXhtmlFile[0].InnerXml = frontMatterXHTMLContent + mainXhtmlFile[0].InnerXml;
                }

                xmldoc.Save(inputXhtmlFilePath);

                if (frontMatterCSSStyle.Trim().Length > 0)
                    frontMatterCSSStyle = frontMatterCSSStyle + ".dummypage{page-break-after: always;} ";

                InsertLoFrontMatterCssFile(_projInfo.DefaultCssFileWithPath, frontMatterCSSStyle);
            }
            catch
            {

            }
        }

        public void InsertLoFrontMatterCssFile(string inputCssFilePath, string frontMatterCSSStyle)
        {
            Param.LoadSettings();
            if (!File.Exists(inputCssFilePath)) return;
            Common.FileInsertText(inputCssFilePath, frontMatterCSSStyle);
        }
        #endregion

        #region XHTML PreProcessor
        /// <summary>
        /// To swap the headword and reversal-form when main.xhtml and FlexRev.xhtml included
        /// </summary>
        public void SwapHeadWordAndReversalForm()
        {
            const string tempString = "\"T_em.pz726h\"";
            const string reversalString = "\"reversal-form\"";
            const string headwordString = "\"headword\"";
            Common.StreamReplaceInFile(_xhtmlFileNameWithPath, reversalString, tempString);
            Common.StreamReplaceInFile(_xhtmlFileNameWithPath, headwordString, reversalString);
            Common.StreamReplaceInFile(_xhtmlFileNameWithPath, tempString, headwordString);
        }

        private bool ClassPos(string ss, int findPos)
        {
            int cnt = findPos + ".Paragraph".Length;
            bool result = false;
            if (ss != null)
            {
                if (ss[cnt] == '{' || ss[cnt] == '\r' || ss[cnt] == ' ' || ss[cnt] == '\t')
                {
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// This function is created for the task TD-872(Kabwa Scripture formatting in XP). Flex exporting css as "REVERSE_SOLIDUS"
        /// when "\" comes with the classname. So this function replace the classname which contains "\" into "REVERSE_SOLIDUS" in the XHTML files
        /// like class="\Citation_Line1" to class="REVERSE_SOLIDUSCitation_line1"
        /// </summary>
        public void ReplaceSlashToREVERSE_SOLIDUS()
        {
            return;
        }

        public string SetLangforLetter(string xhtmlFileName)
        {
            string letterLang = "en";
            XmlDocument xDoc = Common.DeclareXMLDocument(true);
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xDoc.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            xDoc.Load(xhtmlFileName);

            XmlNodeList nodeList = xDoc.SelectNodes("//xhtml:div[@class='letHead']/xhtml:div[@class='letter']", namespaceManager);
            if (nodeList.Count > 0)
            {
                for (int i = 0; i < nodeList.Count; i++)
                {
                    XmlNode letterNode = nodeList[i];
                    XmlNode parentLetterNode = letterNode.ParentNode;
                    if (parentLetterNode != null)
                    {
                        XmlNode letDataNode = parentLetterNode.NextSibling;
                        string xPath = ".//xhtml:div[@class='entry'][1]/xhtml:span[@lang]";
                        if (letDataNode != null)
                        {
                            XmlNode spanNode = letDataNode.SelectSingleNode(xPath, namespaceManager);
                            if (spanNode != null)
                            {
                                letterLang = spanNode.Attributes["lang"].Value;
                            }
                            else
                            {
                                xPath = "//xhtml:div[@class='entry'][1]/xhtml:span[@lang]";
                                XmlNodeList spanNodeList = letDataNode.SelectNodes(xPath, namespaceManager);
                                if (spanNodeList.Count > 0)
                                {
                                    var xmlAttributeCollection = spanNodeList[0].Attributes;
                                    if (xmlAttributeCollection != null)
                                        letterLang = xmlAttributeCollection["lang"].Value;
                                }
                            }
                        }
                    }
                    XmlAttribute attribute = xDoc.CreateAttribute("lang");
                    attribute.Value = letterLang;
                    if (letterNode.Attributes != null) letterNode.Attributes.Append(attribute);
                }
            }
            xDoc.Save(xhtmlFileName);

            return xhtmlFileName;
        }

        /// <summary>
        /// FileOpen used for Preprocessing temp File For Xelatex
        /// </summary>
        public string XelatexImagePreprocess()
        {

            //Temp folder and file copy
            string sourcePicturePath = Path.GetDirectoryName(_baseXhtmlFileNameWithPath);
            string tempFile = _baseXhtmlFileNameWithPath;
            string metaname = Common.GetBaseValue(tempFile);
            if (metaname.Length == 0)
            {
                metaname = Common.GetMetaValue(tempFile);
            }
            string installedDirectory = XeLaTexInstallation.GetXeLaTexDir();

            if (Common.IsUnixOS())
            {
                installedDirectory = sourcePicturePath;
            }
            else
            {
                installedDirectory = Common.PathCombine(installedDirectory, "bin");
                installedDirectory = Common.PathCombine(installedDirectory, "win32");
            }

            if (!File.Exists(tempFile)) return string.Empty;
            var xmldoc = new XmlDocument();
            // xml image copy
            try
            {
                xmldoc = Common.DeclareXMLDocument(true);
                xmldoc.Load(tempFile);

                const string tag = "img";
                XmlNodeList nodeList = xmldoc.GetElementsByTagName(tag);
                if (nodeList.Count > 0)
                {
                    var counter = 1;
                    foreach (XmlNode item in nodeList)
                    {
                        var name = item.Attributes.GetNamedItem("src");
                        if (name != null)
                        {
                            var src = name.Value;
                            if (src.Length > 0)
                            {
                                string fromFileName = Common.GetPictureFromPath(src, metaname, sourcePicturePath);

                                if (File.Exists(fromFileName))
                                {
                                    string ext = Path.GetExtension(fromFileName);
                                    string toFileName = Common.PathCombine(sourcePicturePath, counter + ext);
                                    File.Delete(Common.PathCombine(installedDirectory, counter.ToString() + ".jpg"));
                                    File.Copy(fromFileName, toFileName, true);

                                    XmlAttribute xa = xmldoc.CreateAttribute("longdesc");
                                    xa.Value = name.Value;
                                    item.Attributes.Append(xa);

                                    name.Value = counter + ext;

                                }
                            }
                        }
                        counter++;
                    }
                }
                try
                {
                    //ParagraphVerserSetUp(xmldoc); // TODO - Seperate it from this method.
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
                xmldoc.Save(tempFile);
            }
            catch
            {
            }
            return tempFile;
        }

        /// <summary>
        /// FileOpen used for Preprocessing temp File
        /// </summary>
        /// <param name="isInDesign"> </param>
        public string ImagePreprocess(bool isInDesign)
        {
            return ImagePreprocess(isInDesign, delegate(string s, string to) { File.Copy(s, to, true); });
        }

        public delegate void CopyDelegate(string from, string to);

        public string ImagePreprocess(bool isInDesign, CopyDelegate copyDelegate)

        {
            //if (string.IsNullOrEmpty(sourceFile) || !File.Exists(sourceFile)) return string.Empty;
            //Temp folder and file copy
            string sourcePicturePath = Path.GetDirectoryName(_baseXhtmlFileNameWithPath);
            string tempFile = ProcessedXhtml;
            string metaname = Common.GetBaseValue(tempFile);
            if (metaname.Length == 0)
            {
                metaname = Common.GetMetaValue(tempFile);
            }

            if (_projInfo.ProjectInputType != null && _projInfo.ProjectInputType.ToLower() == "scripture")
            {
                string paraTextprojectPath;
                paraTextprojectPath = Common.GetParatextProjectPath();
                if (!File.Exists(tempFile)) return string.Empty;
                var xmldoc = new XmlDocument();
                // xml image copy
                try
                {
                    xmldoc = Common.DeclareXMLDocument(true);
                    xmldoc.Load(tempFile);

                    const string tag = "img";
                    XmlNodeList nodeList = xmldoc.GetElementsByTagName(tag);
                    if (nodeList.Count > 0)
                    {
						pictureCount = 1;
                        foreach (XmlNode item in nodeList)
                        {
                            var name = item.Attributes.GetNamedItem("src");
                            if (name != null)
                            {
                                var src = name.Value;
                                src = Common.PathCombine(paraTextprojectPath, src);
                                if (src.Length > 0)
                                {
                                    string fromFileName = Common.GetPictureFromPath(src, metaname, sourcePicturePath);
                                    if (File.Exists(fromFileName))
                                    {
                                        string ext = Path.GetExtension(fromFileName);
										string toFileName = Common.PathCombine(tempFolder, pictureCount + ext);
                                        copyDelegate(fromFileName, toFileName);

                                        XmlAttribute xa = xmldoc.CreateAttribute("longdesc");
                                        xa.Value = name.Value;
                                        item.Attributes.Append(xa);
										name.Value = pictureCount + ext;
                                    }
                                }
                            }
							pictureCount++;
                        }
                    }
                    try
                    {
                        ParagraphVerserSetUp(xmldoc); // TODO - Seperate it from this method.
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                    xmldoc.Save(tempFile);
                }
                catch
                {
                }
            }
            else
            {
                // Removal of html tag namespace and other formats.
                //Common.ReplaceInCssFile(tempFile, @"<html\b[^>]*>", "<html>");

                //Common.ReplaceInXhtmlFile(tempFile);
                if (!File.Exists(tempFile)) return string.Empty;
                var xmldoc = new XmlDocument();
                // xml image copy
                try
                {
                    xmldoc = Common.DeclareXMLDocument(true);
                    xmldoc.Load(tempFile);

                    const string tag = "img";
                    XmlNodeList nodeList = xmldoc.GetElementsByTagName(tag);
                    if (nodeList.Count > 0)
                    {
						pictureCount = 1;
                        int imgCount = nodeList.Count;
                        for (int i = 0; i < imgCount; i++)
                        {
							XmlNode item = nodeList[pictureCount - 1];
                            var name = item.Attributes.GetNamedItem("src");
                            if (name != null)
                            {
                                var src = name.Value;
                                if (src.Length > 0)
                                {
                                    string fromFileName = Common.GetPictureFromPath(src, metaname, sourcePicturePath);
                                    if (File.Exists(fromFileName))
                                    {
                                        string ext = Path.GetExtension(fromFileName);

										string toFileName = Common.PathCombine(tempFolder, pictureCount + ext);
                                        copyDelegate(fromFileName, toFileName);
                                        if (isInDesign)
                                        {
                                            string pictureFolderPath = Common.PathCombine(sourcePicturePath, "Pictures");
                                            if (!Directory.Exists(pictureFolderPath))
                                            {
                                                Directory.CreateDirectory(pictureFolderPath);
                                            }
											toFileName = Common.PathCombine(pictureFolderPath, pictureCount + ext);
                                            copyDelegate(fromFileName, toFileName);
                                        }
                                        XmlAttribute xa = xmldoc.CreateAttribute("longdesc");
                                        xa.Value = name.Value;
                                        item.Attributes.Append(xa);
										name.Value = pictureCount + ext;
										pictureCount++;
                                    }
                                    else
                                    {
										var parentNode = nodeList[pictureCount - 1].ParentNode;
                                        if (parentNode != null)
                                        {
											parentNode.RemoveChild(nodeList[pictureCount - 1]);
                                        }
                                        if (parentNode != null)
                                        {
                                            parentNode.RemoveAll();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    try
                    {
                        ParagraphVerserSetUp(xmldoc); // TODO - Seperate it from this method.
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                    xmldoc.Save(tempFile);
                }
                catch
                {
                }
            }

            return tempFile;
        }

        /// <summary>
        /// For dictionary data, returns the language code for the definitions
        /// </summary>
        /// <returns></returns>
        public void PrepareBookNameAndChapterCount()
        {
            XmlDocument xdoc = Common.DeclareXMLDocument(true);
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xdoc.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            xdoc.Load(ProcessedXhtml);
            string xPath = "//xhtml:div[@class='scrBook']";
            string bookName = string.Empty;
            XmlNodeList scrBookList = xdoc.SelectNodes(xPath, namespaceManager);
            if (scrBookList == null) return;
            foreach (XmlNode scrBookNode in scrBookList)
            {
                xPath = ".//xhtml:span[@class='scrBookName']";
                XmlNode bookNode = scrBookNode.SelectSingleNode(xPath, namespaceManager);

                xPath = ".//xhtml:span[@class='Chapter_Number']";
                XmlNodeList chapterNodeList = scrBookNode.SelectNodes(xPath, namespaceManager);

                if (bookNode != null)
                {
                    bookName = bookNode.InnerText;
                    Regex regex = new Regex(@"[^a-zA-Z0-9\s]", (RegexOptions)0);
                    bookName = regex.Replace(bookName, "");
                }
                bookName = bookName.Replace(" ", "_");

                foreach (XmlNode chapterNode in chapterNodeList)
                {
                    string chapterNumber = chapterNode.InnerText;
                    XmlAttribute attribute = xdoc.CreateAttribute("id");
                    if (bookNode != null)
                    {
                        attribute.Value = "id_" + bookName + "_" + "Chapter" + chapterNumber;
                    }
                    if (chapterNode.Attributes != null) chapterNode.Attributes.Append(attribute);
                }
            }
            xdoc.Save(ProcessedXhtml);
        }

		/// <summary>
		/// Remove xml:space in XHTML for Pdf using Prince
		/// </summary>
		public void HandleNewFieldworksChangeInXhtml(string xhtmlFileName)
	    {
			var xdoc = Common.DeclareXMLDocument(true);
			var namespaceManager = new XmlNamespaceManager(xdoc.NameTable);
			namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
			xdoc.Load(xhtmlFileName);
			// Remove attribute "xml:space" in the XHTML file.
			const string xPath = "//xhtml:span[@xml:space='preserve']";
			XmlNodeList spaceNodeList = xdoc.SelectNodes(xPath, namespaceManager);
			if (spaceNodeList == null) return;
			foreach (XmlNode spaceNode in spaceNodeList)
			{
				if (spaceNode.Attributes != null && (spaceNode.Attributes["xml:space"] != null))
					spaceNode.Attributes.Remove(spaceNode.Attributes["xml:space"]);
			}
			xdoc.Save(xhtmlFileName);
	    }

		/// <summary>
		/// Insert property for pictures to avoid overlapping
		/// </summary>
		/// <param name="cssFileName"></param>
		public void HandleNewFieldworksChangeInCss(string cssFileName)
	    {
			TextWriter tw = new StreamWriter(cssFileName, true);
			//Picture Property
			tw.WriteLine(".pictureRight {");
			tw.WriteLine("display: inline;");
			tw.WriteLine("}");

			tw.WriteLine(".pictureLeft {");
			tw.WriteLine("display: inline;");
			tw.WriteLine("}");

			tw.WriteLine(".pictureCenter {");
			tw.WriteLine("display: inline;");
			tw.WriteLine("}");

			tw.WriteLine(".picturePage {");
			tw.WriteLine("display: inline;");
			tw.WriteLine("}");

            tw.Close();
	    }


	    public void GetTempFolderPath()
        {
            if (Directory.Exists(tempFolder))
            {
                try
                {
                    DirectoryInfo di = new DirectoryInfo(tempFolder);
                    Common.CleanDirectory(di);
                }
                catch
                {
                    tempFolder = Common.PathCombine(Path.GetTempPath(), "SilPathWay" + Path.GetFileNameWithoutExtension(Path.GetTempFileName()));
                }
            }
            Directory.CreateDirectory(tempFolder);

            if (_xhtmlFileNameWithPath != null)
                _xhtmlRevFileNameWithPath = Common.PathCombine(Path.GetDirectoryName(_xhtmlFileNameWithPath), "FlexRev.xhtml");

            string tempRevFile = Common.PathCombine(tempFolder, Path.GetFileName(_xhtmlRevFileNameWithPath));

            //Note - copies the xhtml and css files to temp folder
            string tempFile = Common.PathCombine(tempFolder, Path.GetFileName(_xhtmlFileNameWithPath));

            if (Directory.Exists(tempFolder))
            {
                File.Copy(Common.DirectoryPathReplace(_xhtmlFileNameWithPath), tempFile, true);
            }
            else
            {
                Directory.CreateDirectory(tempFolder);
                File.Copy(Common.DirectoryPathReplace(_xhtmlFileNameWithPath), tempFile, true);
            }
            _xhtmlFileNameWithPath = tempFile;

            if (File.Exists(_xhtmlRevFileNameWithPath))
                File.Copy(_xhtmlRevFileNameWithPath, tempRevFile, true);

            if (_cssFileNameWithPath != null)
                _cssRevFileNameWithPath = Common.PathCombine(Path.GetDirectoryName(_cssFileNameWithPath), "FlexRev.css");

            string tempCssFile = Common.PathCombine(tempFolder, Path.GetFileName(_cssRevFileNameWithPath));

            tempFile = Common.PathCombine(tempFolder, Path.GetFileName(_cssFileNameWithPath));
            if (File.Exists(_cssFileNameWithPath))
                File.Copy(Common.DirectoryPathReplace(_cssFileNameWithPath), tempFile, true);
            _cssFileNameWithPath = tempFile;

            if (File.Exists(_cssRevFileNameWithPath))
                File.Copy(_cssRevFileNameWithPath, tempCssFile, true);
            // add a timestamp to the .css for troubleshooting purposes
            AddProductVersionToCSS();
        }

        public List<string> PrepareCurrentNextHeadwordPair()
        {
            bool isHeadword = false;
            List<string> headerVariable = new List<string>();
            XmlTextReader _reader = Common.DeclareXmlTextReader(_xhtmlFileNameWithPath, true);
            while (_reader.Read())
            {
                if (_reader.NodeType == XmlNodeType.Element)
                {
                    if (_reader.Name == "div" || _reader.Name == "span")
                    {
                        string name = _reader.GetAttribute("class");
                        if (name != null && ( name.ToLower() == "headword" || name.ToLower() == "mainheadword"))
                        {
                            isHeadword = true;
                        }
                    }
                }
                else if (isHeadword && _reader.NodeType == XmlNodeType.Text)
                {
                    headerVariable.Add(_reader.Value);
                    isHeadword = false;
                }

            }
            _reader.Close();
            return headerVariable;
        }

        public string InsertEmptyXHomographNumber(IDictionary<string, Dictionary<string, string>> cssClass)
        {
            if (cssClass.ContainsKey("xhomographnumber"))
            {
                string OutputFile = OpenFile();
                string pattern = "<span class=\"headword\" ([^>]+)>[^<]*</[^>]+>";
                MatchCollection matchs = Regex.Matches(_fileContent.ToString(), pattern);
                if (matchs.Count == 0)
                {
                    pattern = "{<span class=\"headword\" ([^>]+)><span class=\"xitem\" ([^>]+)>[^<]*</[^>]+><span class=\"xitem\" ([^>]+)>[^<]*</[^>]+><span class=\"xitem\" ([^>]+)>[^<]*</[^>]+>[^>]+>}|{<span class=\"headword\" ([^>]+)>[^<].+[^>]+>[^<].+[^>]+>[^<].+[^>]+>}";
                    matchs = Regex.Matches(_fileContent.ToString(), pattern);
                    if (matchs.Count > 0)
                    {
                        foreach (Match match in matchs)
                        {
                            if (match.Value.IndexOf("xhomographnumber") != -1)
                                continue;
                            pattern = "<span class=\"xitem\" ([^>]+)>[^<]*</[^>]+>";
                            MatchCollection matchs1 = Regex.Matches(match.Value, pattern);
                            if (matchs1.Count > 0)
                            {
                                foreach (Match match1 in matchs1)
                                {
                                    string matchText = match1.Value;
                                    const string replace = "<span class=\"xhomographnumber\"> </span>";
                                    string replaceText = match1.Value.Replace("</span>", "") + replace + "</span>";
                                    _fileContent.Replace(matchText, replaceText);
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (Match match in matchs)
                    {
                        string matchText = match.Value;
                        const string replace = "<span class=\"xhomographnumber\"> </span>";
                        string replaceText = match.Value.Replace("</span>", "") + replace + "</span>";
                        _fileContent.Replace(matchText, replaceText);
                    }
                }
                var writer = new StreamWriter(OutputFile);
                writer.Write(_fileContent);
                writer.Close();
                _xhtmlFileNameWithPath = OutputFile;
            }
            return _xhtmlFileNameWithPath;
        }

        public string ReplaceInvalidTagtoSpan(string pattern, string tagType)
        {
            string OutputFile = OpenFile();
            try
            {
                MatchCollection matchs = Regex.Matches(_fileContent.ToString(), pattern);
                foreach (Match match in matchs)
                {
                    string matchText = match.Value;
                    string replaceText = tagType;
                    _fileContent.Replace(matchText, replaceText);

                    var writer = new StreamWriter(OutputFile);
                    writer.Write(_fileContent);
                    writer.Close();
                }
            }
            catch
            {
            } _xhtmlFileNameWithPath = OutputFile;
            return _xhtmlFileNameWithPath;
        }

        private string OpenFile()
        {
            string OutputFile = _xhtmlFileNameWithPath;
            var reader = new StreamReader(OutputFile, Encoding.UTF8);
            _fileContent.Remove(0, _fileContent.Length);
            _fileContent.Append(reader.ReadToEnd());
            reader.Close();
            return OutputFile;
        }

        /// <summary>
        /// 1. Modify html  xmlns="http://www.w3.org/1999/xhtml" tag as html
        /// 2. Modify body tag as body xml:space="preserve"
        /// </summary>
        public string PreserveSpace()
        {
            FileStream fs = new FileStream(_xhtmlFileNameWithPath, FileMode.Open);
            StreamReader stream = new StreamReader(fs);

            string fileDir = Path.GetDirectoryName(_xhtmlFileNameWithPath);
            string fileName = "Preserve" + Path.GetFileName(_xhtmlFileNameWithPath);
            string Newfile = Common.PathCombine(fileDir, fileName);

            var fs2 = new FileStream(Newfile, FileMode.Create, FileAccess.Write);
            var sw2 = new StreamWriter(fs2);
            string line;

            bool replace = true;
            bool body = true;
            while ((line = stream.ReadLine()) != null)
            {
                if (replace)
                {
                    int htmlNodeStart = line.IndexOf("<html");
                    if (htmlNodeStart >= 0)
                    {
                        int htmlNodeEnd = line.IndexOf(">", htmlNodeStart);
                        string line1 = "<?xml version=\"1.0\" encoding=\"utf-8\"?> ";
                        line = line1 + "<html" + line.Substring(htmlNodeEnd);
                        sw2.WriteLine(line);
                        replace = false;
                    }
                }
                else
                {
                    if (body && line.IndexOf("<body") >= 0)
                    {
                        line = line.Replace("<body", @" <body xml:space=""preserve""  ");
                        body = false;
                    }
                    sw2.WriteLine(line);
                }
            }
            sw2.Close();
            fs.Close();
            fs2.Close();

            _xhtmlFileNameWithPath = Newfile;
            return _xhtmlFileNameWithPath;
        }

        public string GoBibleRearrangeVerseNumbers(string fileName)
        {
            XmlDocument xDoc = Common.DeclareXMLDocument(true);
            xDoc.Load(fileName);
            XmlNodeList nodeList = xDoc.GetElementsByTagName("div");
            XmlNode nodeContent = xDoc.CreateElement("div");
            bool isNewNode = false;
            if (nodeList.Count > 0)
            {
                int counter = nodeList.Count + 1;
                string alpha = string.Empty;
                for (int i = 0; i < counter; i++)
                {
                    XmlNode o = nodeList[i];
                    if (o == null || o.Attributes["class"] == null)
                        continue;
                    string a = o.Attributes["class"].Value;
                    bool b = o.Attributes["title"] != null;
                    if (a.ToLower().IndexOf("verse") >= 0 && b)
                    {
                        string[] num = new string[] { };
                        string currVerse = string.Empty;

                        if (o.Attributes["title"].Value.Contains("-"))
                        {
                            num = o.Attributes["title"].Value.Split('-');
                            o.Attributes["title"].Value = num[1];
                        }
                        else
                        {
                            currVerse = o.Attributes["title"].Value;
                            if (currVerse.Length > 0)
                            {
                                if (Common.IsAlpha(currVerse.Substring(currVerse.Length - 1, 1)))
                                {
                                    alpha = o.Attributes["title"].Value.Substring(0, currVerse.Length - 1);
                                    nodeContent.AppendChild(o.FirstChild.CloneNode(true));
                                    o.InnerText = string.Empty;
                                    o.ParentNode.RemoveChild(o);
                                    isNewNode = true;
                                    i--;
                                    counter--;
                                }
                                else if (isNewNode)
                                {
                                    XmlNode alphaNode = xDoc.CreateElement("div");
                                    XmlAttribute xmlAttribute = xDoc.CreateAttribute("class");
                                    xmlAttribute.Value = "verse";
                                    alphaNode.Attributes.Append(xmlAttribute);

                                    xmlAttribute = xDoc.CreateAttribute("title");
                                    xmlAttribute.Value = alpha;
                                    alphaNode.Attributes.Append(xmlAttribute);

                                    alphaNode.InnerXml = nodeContent.InnerXml;
                                    o.ParentNode.InsertBefore(alphaNode, o);

                                    isNewNode = false;
                                }
                            }
                        }


                        if (num.Length > 1)
                        {
                            int titleValue = 0;

                            titleValue = Convert.ToInt32(num[1]) - Convert.ToInt32(num[0]);
                            int startNumValue = Convert.ToInt32(num[0]);
                            for (int j = 1; j <= titleValue; j++)
                            {
                                XmlNode emptyNode = xDoc.CreateElement("div");
                                XmlAttribute xmlAttribute = xDoc.CreateAttribute("class");
                                xmlAttribute.Value = "verse";
                                emptyNode.Attributes.Append(xmlAttribute);

                                xmlAttribute = xDoc.CreateAttribute("title");
                                xmlAttribute.Value = startNumValue.ToString();
                                emptyNode.Attributes.Append(xmlAttribute);

                                emptyNode.InnerXml = "<span xml:lang='zxx'><br />" + Common.UnicodeConversion(" ") +
                                                     "<br /></span>";
                                o.ParentNode.InsertBefore(emptyNode, o);

                                startNumValue++;
                            }
                        }
                    }
                }
            }

            xDoc.Save(fileName);
            return fileName;
        }


        public string InsertSectionHeadID()
        {
            XmlDocument xDoc = Common.DeclareXMLDocument(true);
            xDoc.Load(_xhtmlFileNameWithPath);
            XmlNodeList nodeList = xDoc.GetElementsByTagName("span");
            if (nodeList.Count > 0)
            {
                int counter = nodeList.Count + 1;
                for (int i = 0; i < counter; i++)
                {
                    XmlNode o = nodeList[i];
                    if (o == null || o.Attributes["class"] == null)
                        continue;
                    string a = o.Attributes["class"].Value;
                    if (a.ToLower().IndexOf("section_head") >= 0)
                    {
                        XmlAttribute attribute = xDoc.CreateAttribute("id");
                        attribute.Value = "id_" + o.Attributes["class"].Value + i.ToString();
                        o.Attributes.Append(attribute);
                    }
                }
            }

            nodeList = xDoc.GetElementsByTagName("div");

            if (nodeList.Count > 0)
            {
                int counter = nodeList.Count + 1;
                for (int i = 0; i < counter; i++)
                {
                    XmlNode o = nodeList[i];
                    if (o == null || o.Attributes["class"] == null)
                        continue;

	                if (string.IsNullOrEmpty(o.InnerText) && string.IsNullOrEmpty(o.InnerXml))
		                o.Attributes["class"].Value = "EmptyTag";

                    string a = o.Attributes["class"].Value;
                    if (a.ToLower().IndexOf("section_head") >= 0)
                    {
                        XmlAttribute attribute = xDoc.CreateAttribute("id");
                        attribute.Value = "id_" + o.Attributes["class"].Value + i.ToString();
                        o.Attributes.Append(attribute);
                    }
                }
            }

            xDoc.Save(_xhtmlFileNameWithPath);
            return _xhtmlFileNameWithPath;
        }

        public void InsertPseudoContentProperty(string cssFileName, ArrayList pseudoClass)
        {
            TextWriter tw = new StreamWriter(cssFileName, true);
            for (int i = 0; i < pseudoClass.Count; i++)
            {
                string[] value = pseudoClass[i].ToString().Split('_');
                try
                {
                    if (value.Length > 1)
                    {
                        if (value[0].Contains(".."))
                        {
                            string className = value[0].Substring(0, value[0].LastIndexOf("..", StringComparison.Ordinal));
                            string pseudoName = value[0].Substring(value[0].LastIndexOf("..", StringComparison.Ordinal) + 2);
                            if (className.ToLower().IndexOf("span", StringComparison.Ordinal) != 0)
                            {
                                className = "." + className;
                            }
                            tw.WriteLine(className + ":" + pseudoName + " {");
                            tw.WriteLine("content: '';");
                            tw.WriteLine("}");
                        }
                    }
                }
                catch { }
            }
            tw.Close();

        }

        public string InsertHiddenChapterNumber()
        {
            XmlDocument xDoc = Common.DeclareXMLDocument(true);
            xDoc.Load(_xhtmlFileNameWithPath);
            XmlNodeList nodeList = xDoc.GetElementsByTagName("span");
            if (nodeList.Count > 0)
            {
                XmlNode newNode = null;
                int counter = nodeList.Count + 1;
                for (int i = 0; i < counter; i++)
                {
                    XmlNode o = nodeList[i];
                    if (o == null || o.Attributes["class"] == null)
                        continue;
                    string a = o.Attributes["class"].Value;
                    if (a.ToLower().IndexOf("chapter_number") >= 0)
                    {
                        newNode = o.Clone();
                        XmlAttribute attribute = xDoc.CreateAttribute("id");
                        attribute.Value = "empty";
                        newNode.Attributes["class"].Value = "hide_" + o.Attributes["class"].Value;
                        newNode.Attributes.Append(attribute);
                    }
                    if (a.ToLower().IndexOf("verse_number") >= 0 && newNode != null)
                    {
                        XmlNode iNode = newNode.Clone();
                        o.ParentNode.InsertBefore(iNode, o);
                        i = i + 1;
                        counter++;
                    }
                }
            }
            xDoc.Save(_xhtmlFileNameWithPath);
            SetHideChapterNumberInCSS();
            return _xhtmlFileNameWithPath;
        }

        public void InsertEmptyHeadwordForReversal(string fileName)
        {
            string flexRevFileName = Common.PathCombine(Path.GetDirectoryName(fileName), "FlexRev.xhtml");
            if (!File.Exists(flexRevFileName)) return;
            XmlDocument xDoc = Common.DeclareXMLDocument(true);
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xDoc.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            xDoc.Load(flexRevFileName);
            string xPath = "//xhtml:span[@class='reversal-form']";
            XmlNodeList RevFormNodes = xDoc.SelectNodes(xPath, namespaceManager);

            for (int i = 0; i < RevFormNodes.Count; i++)
            {
                XmlDocumentFragment docFrag = InsertEmptyHeadword(xDoc);
                RevFormNodes[i].AppendChild(docFrag);
            }
            xDoc.Save(flexRevFileName);
        }

        private static XmlDocumentFragment InsertEmptyHeadword(XmlDocument xdoc)
        {
            const string toInsert = "<span class=\"headword\"> </span>";
            XmlDocumentFragment docFrag = xdoc.CreateDocumentFragment();
            docFrag.InnerXml = toInsert;
            return docFrag;
        }

		/// <summary>
		/// Method to change the Verse_number style which contains "1", it happens when we choose "Hide Verse No. 1" in Pathway Configuration Tool
		/// </summary>
		/// <inputType>Dictionary/Scripture</inputType>
		/// <xhtmlFilePath>XHTML File Path</xhtmlFilePath>
		/// <returns>Modified XHTML file path</returns>
	    public string ChangeVerseNumberOneStyleWhenHide(string inputType, string xhtmlFilePath)
	    {
			if (inputType.ToLower() == "dictionary") return xhtmlFilePath;
			//Change Verse_Number to Verse_Number1 in XHTML File
			XmlDocument xDoc = Common.DeclareXMLDocument(true);
			xDoc.Load(xhtmlFilePath);
		    const string xPath = "//div[@class='Paragraph']/span[@class='Verse_Number'][text()='1']";
			XmlNodeList nodeList = xDoc.SelectNodes(xPath);
		    if (nodeList != null && nodeList.Count > 0)
		    {
				foreach (XmlNode verseNode in nodeList)
				{
					if (verseNode.Attributes != null) verseNode.Attributes["class"].Value = verseNode.Attributes["class"].Value + "1";
				}
		    }
			xDoc.Save(xhtmlFilePath);

			//Add CSS for to Hide verse number one
			if (File.Exists(_cssFileNameWithPath))
			{
				TextWriter tw = new StreamWriter(_cssFileNameWithPath, true);
				tw.WriteLine(".Verse_Number1 {");
				tw.WriteLine("font-size: 0.1pt;");
				tw.WriteLine("visibility: hidden;");
				tw.WriteLine("}");
				tw.Close();
			}

			return xhtmlFilePath;
	    }

	    public string InsertHiddenVerseNumber()
        {
            XmlDocument xDoc = Common.DeclareXMLDocument(true);
            xDoc.Load(_xhtmlFileNameWithPath);
            XmlNodeList nodeList = xDoc.GetElementsByTagName("span");
            if (nodeList.Count > 0)
            {
                XmlNode newNode = null;
                int counter = nodeList.Count + 1;
                for (int i = 0; i < counter; i++)
                {
                    XmlNode o = nodeList[i];
                    if (o == null || o.Attributes["class"] == null)
                        continue;
                    string a = o.Attributes["class"].Value;


                    if (a.ToLower().IndexOf("verse_number") >= 0)
                    {
                        newNode = o.Clone();
                        newNode.Attributes["class"].Value = "hide_" + o.Attributes["class"].Value;
                    }
                    if (a.ToLower().IndexOf("verse_number") >= 0 && newNode != null)
                    {
                        XmlNode iNode = newNode.Clone();
                        o.ParentNode.InsertBefore(iNode, o);
                        i = i + 1;
                        counter++;
                    }
                    if (o.InnerText == "1" && (o.Attributes["class"].Value.ToLower().IndexOf("verse_number") >= 0))
                    {
                        o.Attributes.GetNamedItem("class").Value = "PWHide";
                    }
                }
            }
            xDoc.Save(_xhtmlFileNameWithPath);
            SetHideVerseNumberInCSS();
            return _xhtmlFileNameWithPath;
        }

        /// <summary>
        /// For dictionary data, returns the language code for the definitions
        /// </summary>
        /// <returns></returns>
        public bool GetMultiLanguageHeader()
        {
            if (_projInfo.ProjectInputType.ToLower() == "dictionary") return false;
            bool isFound = false;
            var xDoc = Common.DeclareXMLDocument(true);
            FileStream fs = File.OpenRead(_xhtmlFileNameWithPath);
            xDoc.Load(fs);
            fs.Close();
            XmlNodeList nodeList = xDoc.GetElementsByTagName("div");
            if (nodeList.Count > 0)
            {
                foreach (XmlNode node in nodeList)
                {
                    if (node == null || node.Attributes["class"] == null)
                        continue;
                    string className = node.Attributes["class"].Value;
                    if (className.ToLower() == "scrbook")
                    {
                        string pattern = "<span class=\"scrBookName\" ([^>]+)>[^<]*</[^>]+>";
                        MatchCollection matchs = Regex.Matches(node.InnerXml, pattern);
                        if (matchs.Count > 1)
                        {
                            isFound = true;
                            break;
                        }
                    }
                }
            }
            return isFound;
        }

        /// <summary>
        /// For scripture data, move the Paratext FRT before all content.
        /// TD-3786
        /// </summary>
        /// <returns></returns>
        public void MoveBookcodeFRTtoFront(string ProcessedXhtml)
        {
            if (_projInfo.ProjectInputType.ToLower() == "dictionary") { return; }

            if (!File.Exists(ProcessedXhtml)) return;
            XmlDocument xDoc = Common.DeclareXMLDocument(true);
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xDoc.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            xDoc.Load(ProcessedXhtml);
            string xPath = "//div[@class='scrBook']//span[@class='scrBookCode']";
            XmlNodeList bookLists = xDoc.SelectNodes(xPath, namespaceManager);
            if (bookLists.Count > 0)
            {
                for (int i = 0; i < bookLists.Count; i++)
                {
                    string x1 = bookLists[i].InnerText;
                    if (x1.ToLower() == "frt")
                    {
                        XmlNode frtNode = bookLists[i].ParentNode;
                        string bookName = string.Empty;
                        bookName = Regex.Replace(frtNode.InnerXml, "<div class=\"columns\">", "<div  class=\"frtColumns\">");
                        //bookName = Regex.Replace(bookName, "<div class=\"pictureColumn\">", "<div>");
                        frtNode.InnerXml = bookName;

                        xPath = "//div[@class='scrBook'][1]";
                        XmlNode firstBookNode = xDoc.SelectSingleNode(xPath, namespaceManager);
                        if (frtNode != null) firstBookNode.ParentNode.InsertBefore(frtNode.CloneNode(true), firstBookNode);
                        firstBookNode.ParentNode.RemoveChild(frtNode);
                    }
                }
            }
            xDoc.Save(ProcessedXhtml);
        }

        /// <summary>
        /// For dictionary data, reversal title is empty, So it set as "Reversal"
        /// </summary>
        /// <returns></returns>
        public void SetTitleValueOnReversal(string xhtmlFile)
        {
            if (_projInfo.ProjectInputType.ToLower() == "scripture") { return; }
            string reversalFile = Path.Combine(Path.GetDirectoryName(xhtmlFile), "FlexRev.xhtml");
            if (!File.Exists(reversalFile)) return;
            XmlDocument xDoc = Common.DeclareXMLDocument(true);
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xDoc.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            xDoc.Load(reversalFile);
            XmlNode titleNode = xDoc.SelectSingleNode("//head/title", namespaceManager);
            if (titleNode != null)
            {
                titleNode.InnerText = "Reversal";
            }
            xDoc.Save(reversalFile);
        }

        /// <summary>
        /// For dictionary data, returns the language code for the definitions
        /// </summary>
        /// <returns></returns>
        public string GetDefinitionLanguage()
        {
            XmlDocument xDoc = Common.DeclareXMLDocument(true);
            xDoc.Load(_xhtmlFileNameWithPath);
            XmlNodeList nodeList = xDoc.GetElementsByTagName("div");
            if (nodeList.Count > 0)
            {
                XmlNode newNode = null;
                int counter = nodeList.Count + 1;
                for (int i = 0; i < counter; i++)
                {
                    isNodeFound = false;
                    XmlNode node1 = nodeList[i];
                    if (node1 == null || node1.Attributes["class"] == null)
                        continue;
                    string className = node1.Attributes["class"].Value;
                    string definitionLang = string.Empty;

                    if (className.ToLower().IndexOf("entry") >= 0)
                    {
                        XmlNode entryLang = node1.Attributes["lang"];
                        if (entryLang == null)
                        {
                            newNode = GetNode(node1, "definition");
                            if (newNode != null)
                                if (newNode.Attributes != null && newNode.Attributes["lang"] != null)
                                {
                                    definitionLang = newNode.Attributes["lang"].Value;
                                }
                        }
                        if (definitionLang.Length > 0)
                        {
                            XmlAttribute xa = xDoc.CreateAttribute("lang");
                            xa.Value = definitionLang;
                            node1.Attributes.Append(xa);
                        }
                    }

                }
            }
            xDoc.Save(_xhtmlFileNameWithPath);
            return _xhtmlFileNameWithPath;
        }

        public string GetfigureNode()
        {
            try
            {
                XmlDocument xDoc = Common.DeclareXMLDocument(true);
                xDoc.Load(_xhtmlFileNameWithPath);
                XmlNodeList nodeList = xDoc.GetElementsByTagName("figure");
                if (nodeList.Count > 0)
                {
                    int counter = nodeList.Count;
                    for (int i = 0; i < counter; i++)
                    {
                        XmlNode oldChild = nodeList[0];
                        if (oldChild == null) break;
                        string src = oldChild.Attributes["file"].Value;
                        string cap = oldChild.InnerText;
                        string refer = oldChild.Attributes["ref"].Value;
                        string pos = oldChild.Attributes["size"].Value;
                        string alt = oldChild.Attributes["desc"].Value;
                        if (pos == "span")
                            pos = "pictureCenter";
                        else
                            pos = "pictureRight";
                        XmlNode newNode = xDoc.CreateElement("div");
                        XmlAttribute xmlAttribute = xDoc.CreateAttribute("class");
                        newNode.Attributes.Append(xmlAttribute);

                        xmlAttribute = xDoc.CreateAttribute("id");
                        xmlAttribute.Value = "i1";
                        newNode.Attributes.Append(xmlAttribute);

                        XmlNode newNodeImg = xDoc.CreateElement("img");
                        xmlAttribute = xDoc.CreateAttribute("src");
                        xmlAttribute.Value = "figures\\" + src;
                        newNodeImg.Attributes.Append(xmlAttribute);

                        xmlAttribute = xDoc.CreateAttribute("id");
                        xmlAttribute.Value = "i2";
                        newNodeImg.Attributes.Append(xmlAttribute);

                        xmlAttribute = xDoc.CreateAttribute("alt");
                        xmlAttribute.Value = alt;
                        newNodeImg.Attributes.Append(xmlAttribute);

                        newNode.AppendChild(newNodeImg);

                        XmlNode newNodeDiv = xDoc.CreateElement("div");
                        xmlAttribute = xDoc.CreateAttribute("class");
                        xmlAttribute.Value = pos;  // postition class
                        newNodeDiv.Attributes.Append(xmlAttribute);
                        newNodeDiv.InnerText = cap + "(" + refer + ")";

                        newNode.AppendChild(newNodeDiv);

                        oldChild.ParentNode.ReplaceChild(newNode, oldChild);
                    }
                    xDoc.Save(_xhtmlFileNameWithPath);
                }
                xDoc = null;
            }
            catch
            {
            }
            return _xhtmlFileNameWithPath;
        }

        private XmlNode GetNode(XmlNode node, string className)
        {
            if (isNodeFound) return returnNode;

            foreach (XmlNode ChildNode in node)
            {
                if (ChildNode.HasChildNodes)
                {
                    GetNode(ChildNode, className);
                    if (isNodeFound) return returnNode;
                }
                if (ChildNode.Attributes != null && ChildNode.Attributes["class"] != null)
                {
                    string className1 = ChildNode.Attributes["class"].Value;
                    if (className1 == className)
                    {
                        returnNode = ChildNode.Clone();
                        isNodeFound = true;
                        break;
                    }
                }
            }
            return returnNode;
        }

        /// <summary>
        /// For dictionary data, returns the language code for the definitions
        /// </summary>
        /// <returns></returns>
        public void GetDefaultLanguage(PublicationInformation projInfo)
        {
            try
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(_xhtmlFileNameWithPath);
                if (fileNameWithoutExtension != null)
                {
                    string fileName = fileNameWithoutExtension.ToLower();
                    if (fileName != "main" && fileName != "main1")
                        return;
                }

                var xDoc = Common.DeclareXMLDocument(true);
                xDoc.Load(_xhtmlFileNameWithPath);
                XmlNodeList nodeList = xDoc.GetElementsByTagName("meta");
                if (nodeList.Count > 0)
                {
                    if (projInfo.ProjectInputType.ToLower() == "dictionary")
                    {
                        for (int i = 0; i < nodeList.Count; i++)
                        {
                            XmlNode node = nodeList[i];
                            if (node.Attributes != null)
                            {
                                if (node.Attributes["scheme"] == null) continue;
                                string className = node.Attributes["scheme"].Value;
                                if (className == "language to font")
                                {
                                    projInfo.DefaultFontName = node.Attributes["content"].Value;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < nodeList.Count; i++)
                        {

                            isNodeFound = false;
                            XmlNode node = nodeList[i];
                            if (node.Attributes != null)
                            {
                                if (node.Attributes["name"] == null) continue;
                                string className = node.Attributes["name"].Value;
                                if (className == "fontName")
                                {
                                    projInfo.DefaultFontName = node.Attributes["content"].Value;
                                }
                                else if (className == "fontSize")
                                {
                                    if (float.Parse(node.Attributes["content"].Value) < projInfo.DefaultFontSize)
                                        projInfo.DefaultFontSize = float.Parse(node.Attributes["content"].Value);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void GetReferenceList(string xhtmlFileNameWithPath, List<string> sourceList, List<string> targetList)
        {
            XmlTextReader _reader = Common.DeclareXmlTextReader(xhtmlFileNameWithPath, true);
            try
            {
                while (_reader.Read())
                {
                    if (_reader.NodeType == XmlNodeType.Element)
                    {
                        string st;
                        if (_reader.Name == "a")
                        {
                            string href = _reader.GetAttribute("href");
                            if (href != null)
                            {
                                st = href.Replace("#", "").ToLower();
                                if (sourceList.Contains(st)) continue;
                                sourceList.Add(st);
                                string id = _reader.GetAttribute("id");
                                if (id != null)
                                {
                                    if (targetList.Contains(id.ToLower())) continue;
                                    targetList.Add(id.ToLower());
                                    continue;
                                }
                                continue;
                            }
                        }
                        if (_reader.Name == "div" || _reader.Name == "span" || _reader.Name == "a")
                        {

                            string id = _reader.GetAttribute("id");
                            if (id != null)
                            {
                                if (targetList.Contains(id.ToLower())) continue;
                                targetList.Add(id.ToLower());
                                continue;
                            }

                            string name = _reader.GetAttribute("name");
                            if (name != null)
                            {
                                if (targetList.Contains(name.ToLower())) continue;
                                targetList.Add(name.ToLower());
                                continue;
                            }
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("GetReferenceList");
            }
            _reader.Close();
        }

        public void GetGlossaryList(string xhtmlFileNameWithPath, Dictionary<string, string> glossaryList)
        {
            XmlTextReader _reader = Common.DeclareXmlTextReader(xhtmlFileNameWithPath, true);
            try
            {
                while (_reader.Read())
                {
                    if (_reader.NodeType == XmlNodeType.Element)
                    {
                        if (_reader.Name == "a")
                        {
                            string href = _reader.GetAttribute("href");
                            string id = _reader.GetAttribute("id");
                            if (href != null && id != null)
                            {
                                if (href != "#" && !glossaryList.ContainsKey(href))
                                {
                                    glossaryList[href] = id;
                                }
                            }

                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("GetGlossaryList");
            }
            _reader.Close();
        }

        /// <summary>
        /// TD-3482 & TD-4763
        /// </summary>
        /// <param name="fileName"></param>
        public void MoveCallerToPrevText(string fileName)
        {
            if (!File.Exists(fileName)) return;
            XmlDocument xDoc = Common.DeclareXMLDocument(true);
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xDoc.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            xDoc.Load(fileName);
            const string xPath = "//xhtml:span[@class='scrFootnoteMarker']";
            XmlNodeList markerNodeList = xDoc.SelectNodes(xPath, namespaceManager);
            if (markerNodeList == null) return;
            for (int i = 0; i < markerNodeList.Count; i++)
            {
                XmlNode markerPrevNode = markerNodeList[i].PreviousSibling;
                XmlNode markerNextNode = markerNodeList[i].NextSibling;
                if (markerPrevNode != null && markerNextNode != null)
                {
                    string nextNodeContent = markerNextNode.OuterXml;
                    if (nextNodeContent.Contains("Note_Target_Reference"))
                    {
                        try
                        {
                            markerPrevNode.InnerXml = markerPrevNode.InnerXml + markerNextNode.OuterXml;
                        }
                        catch (InvalidOperationException) //If the previous node is text or whitespace
                        {
                            return;
                        }
                    }
                }
                if (markerNextNode != null && markerNextNode.ParentNode != null)
                    markerNextNode.ParentNode.RemoveChild(markerNextNode);
            }
            xDoc.Save(fileName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileName"></param>
        public void MovePictureAsLastChild(string fileName)
        {
            if (!File.Exists(fileName)) return;
            XmlDocument xDoc = Common.DeclareXMLDocument(true);
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xDoc.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            xDoc.Load(fileName);

			List<string> lstPictureXpath = new List<string>();
			lstPictureXpath.Add("//div[@class='entry']/div[@class= 'pictureRight']");
			lstPictureXpath.Add("//div[@class='entry']/div[@class= 'picture']");
			lstPictureXpath.Add("//div[@class='entry']/span[@class= 'picture']");

	        foreach (var xPath in lstPictureXpath)
	        {
		        XmlNodeList entryNodeList = xDoc.SelectNodes(xPath, namespaceManager);
		        if (entryNodeList != null && entryNodeList.Count > 0)
		        {
			        for (int i = 0; i < entryNodeList.Count; i++)
			        {
				        XmlNode entryNode = entryNodeList[i].ParentNode;
				        if (entryNode != null) entryNode.ParentNode.InsertAfter(entryNodeList[i], entryNode);

			        }
		        }
	        }
	        xDoc.Save(fileName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileName"></param>
        public void SetNonBreakInVerseNumber(string fileName)
        {
            if (!File.Exists(fileName)) return;
            XmlDocument xDoc = Common.DeclareXMLDocument(true);
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xDoc.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            xDoc.Load(fileName);
            string xPath = "//xhtml:span[@class='Verse_Number']";
            XmlNodeList verseNodeList = xDoc.SelectNodes(xPath, namespaceManager);
            if (verseNodeList == null) return;
            for (int i = 0; i < verseNodeList.Count; i++)
            {
                XmlNode nextNode = verseNodeList[i].NextSibling;
                if (nextNode == null) continue;
                if (nextNode.OuterXml.IndexOf("span") == -1)
                {
                    nextNode = nextNode.NextSibling;
                }
                verseNodeList[i].InnerText = verseNodeList[i].InnerText.Trim() + " ";
                if (nextNode != null)
                {
                    nextNode.InnerXml = verseNodeList[i].OuterXml + nextNode.InnerXml;
                    nextNode.ParentNode.RemoveChild(verseNodeList[i]);
                }
            }
            xDoc.Save(fileName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileName"></param>
        public void ReplaceDoubleSlashToLineBreak(string fileName)
        {
            if (!File.Exists(fileName)) return;
            XmlDocument xDoc = Common.DeclareXMLDocument(true);
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xDoc.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            xDoc.Load(fileName);
            string xPath = "//xhtml:div[@class='scrSection']";
            XmlNodeList SectionNodeList = xDoc.SelectNodes(xPath, namespaceManager);
            if (SectionNodeList == null) return;
            for (int i = 0; i < SectionNodeList.Count; i++)
            {
                if (SectionNodeList[i].InnerText.IndexOf(" // ") > 0)
                {
                    SectionNodeList[i].InnerXml = SectionNodeList[i].InnerXml.Replace(" // ", " <br/>");
                }

            }
            xDoc.Save(fileName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="mergedCSS"> </param>
        public void RemoveVerseNumberOne(string fileName, string mergedCSS)
        {
            if (!File.Exists(fileName)) return;
            XmlDocument xDoc = Common.DeclareXMLDocument(true);
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xDoc.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            xDoc.Load(fileName);
            string xPath = "//xhtml:span[@class='Chapter_Number']";
            XmlNodeList SectionNodeList = xDoc.SelectNodes(xPath, namespaceManager);
            if (SectionNodeList == null) return;
            for (int i = 0; i < SectionNodeList.Count; i++)
            {
                XmlNode paraNode = SectionNodeList[i].ParentNode;
                xPath = ".//xhtml:span[@class='Verse_Number']";
                XmlNodeList verseNodeList = paraNode.SelectNodes(xPath, namespaceManager);
                if (verseNodeList == null) return;
                if (verseNodeList.Count > 0)
                {
                    verseNodeList[0].InnerText = "";
                }
            }
            xDoc.Save(fileName);
        }

        public string RemoveTextIndent(string fileName)
        {
            string fileNameExtension = Path.GetExtension(fileName);
            string newFileName = fileName.Replace(fileNameExtension, "1" + fileNameExtension);
            string line;
            StreamReader read = new StreamReader(fileName);
            StreamWriter write = new StreamWriter(newFileName);

            bool isPicture = false;
            while ((line = read.ReadLine()) != null)
            {
                if (isPicture || line.Contains(".picture"))
                {
                    isPicture = true;

                    if (!line.Contains("text-indent"))
                    {
                        write.WriteLine(line);
                        if (line.Contains("}"))
                        {
                            isPicture = false;
                        }
                    }
                }
                else
                {
                    write.WriteLine(line);
                }
            }

            read.Close();
            write.Close();
            return newFileName;
        }

        /// <summary>
        /// Include hyphenation words into XHTML input file using replace method.
        /// </summary>
        /// <param name="xhtmlFile">input XHTML file</param>
        public void IncludeHyphenWordsOnXhtml(string xhtmlFile)
        {
            if(Common.Testing == false)
                if (_projInfo.ProjectInputType.ToLower() == "dictionary") return;

            if(!Param.HyphenEnable) return;

            var hyphenWords = GetHyphenationWords();

            if (!File.Exists(xhtmlFile)) return;

            // load the xhtml file we're working with
            XmlDocument xmlDocument = Common.DeclareXMLDocument(true);
            var namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            var xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, DtdProcessing = DtdProcessing.Parse};
            XmlReader xmlReader = XmlReader.Create(xhtmlFile, xmlReaderSettings);
            xmlDocument.Load(xmlReader);
            xmlReader.Close();

            const string xPath = "//xhtml:div[@class='Paragraph']";
            XmlNodeList paragraphNodeList = xmlDocument.SelectNodes(xPath, namespaceManager);
            if (paragraphNodeList != null && paragraphNodeList.Count > 0)
            {
                for (int i = 0; i < paragraphNodeList.Count; i++)
                {
                    foreach (string hyphenWord in hyphenWords.Keys)
                    {
                        if (paragraphNodeList[i].InnerText.Contains(" " + hyphenWord + " ") ||
                            paragraphNodeList[i].InnerText.Contains(" " + hyphenWord + ",") ||
                            paragraphNodeList[i].InnerText.Contains(hyphenWord + ","))
                        {
                            paragraphNodeList[i].InnerText = paragraphNodeList[i].InnerText.Replace(hyphenWord, hyphenWords[hyphenWord]);
                        }
                    }
                }
            }
            xmlDocument.Save(xhtmlFile);
        }

        /// <summary>
        /// Get the hyphenation words from the file "hyphenatedWords.txt" for scripture
        /// </summary>
        public Dictionary<string, string> GetHyphenationWords()
        {
            var hyphWords = new Dictionary<string, string>();
            string hyphFilePath = Param.HyphenFilepath;

            if (Common.Testing == true)
            {
                const string fileName = "hyphenatedWords.txt";
                string folderPath = Common.LeftString(Environment.CurrentDirectory, "bin");
                String inputFolder = Common.PathCombine(folderPath, "PsTool/TestFiles/InputFiles");
                hyphFilePath = Common.PathCombine(inputFolder, fileName);
            }

            if (File.Exists(hyphFilePath))
            {
                string line = string.Empty;
                var fs = new FileStream(hyphFilePath, FileMode.Open);
                var stream = new StreamReader(fs);
                while ((line = stream.ReadLine()) != null)
                {
                    if (line.Trim().IndexOf(' ') == -1 && line.Trim().IndexOf('=') > 0)
                    {
                        string actText = line.Trim().Replace("=", "");
                        actText = actText.Replace("*", "");
                        string chgText = line.Trim().Replace("=", "\u00AD");
                        chgText = chgText.Replace("*", "");
                        hyphWords[actText] = chgText;
                    }
                }
                fs.Close();
            }
            return hyphWords;
        }
        #endregion

        #region XML PreProcessor

        /// <summary>
        /// Changes the Class Name from Paragraph to Paragraph1 and Verse_numnber to Verse_numnber1
        /// if the para has ChapterNumber.
        /// </summary>
        /// <param name="xmldoc">The xml Document</param>
        public void ParagraphVerserSetUp(XmlDocument xmldoc)
        {
            string divTag = "div";
            XmlNodeList divNodeList = xmldoc.GetElementsByTagName(divTag);
            if (divNodeList.Count > 0)
            {
                foreach (XmlNode item in divNodeList)
                {
                    var para = item.Attributes.GetNamedItem("class");
                    if (para != null && para.Value.ToLower() == "paragraph" && para.HasChildNodes)
                    {
                        foreach (XmlNode chapterNode in item)
                        {
                            if (chapterNode.Attributes == null)
                            {
                                continue;
                            }
                            var chapter = chapterNode.Attributes.GetNamedItem("class");
                            if (chapter != null && chapter.Value.ToLower() == "chapter_number")
                            {
                                XmlNode verseNode = chapterNode.NextSibling;
                                if (verseNode == null) continue;
                                XmlNode verse = null;
                                if (verseNode.Attributes == null)
                                {
                                    continue;
                                }
                                verse = verseNode.Attributes.GetNamedItem("class");
                                if (verse != null && verse.Value.ToLower() == "verse_number")
                                {
                                    para.Value = "Paragraph1";
                                    verse.Value = "Verse_Number1";
                                    break;
                                }
                            }
                        }
                    }
                }

                // This is SrcBook - Border
                for (int i = 0; i < divNodeList.Count; i++)
                {
                    XmlNode item = divNodeList[i];
                    bool scrIntroSection = false;
                    var para = item.Attributes.GetNamedItem("class");
                    if (para != null && para.Value.ToLower() == "scrbook" && para.HasChildNodes)
                    {
                        foreach (XmlNode chapterNode in item)
                        {
                            if (chapterNode.NodeType == XmlNodeType.Whitespace) continue;
                            if (chapterNode.Attributes == null)
                            {
                                continue;
                            }
                            var chapter = chapterNode.Attributes.GetNamedItem("class");
                            if (chapter != null && chapter.Value.ToLower() == "scrintrosection")
                            {
                                scrIntroSection = true;
                            }
                            else if (scrIntroSection && chapter != null)
                            {
                                if (chapter.Value.ToLower() != "border")
                                {
                                    XmlNode borderNode = xmldoc.CreateNode("element", "div", null);
                                    XmlAttribute xmlAttrib = xmldoc.CreateAttribute("class");
                                    xmlAttrib.Value = "border";
                                    borderNode.Attributes.Append(xmlAttrib);
                                    borderNode.InnerText = "";
                                    chapterNode.ParentNode.InsertBefore(borderNode, chapterNode);
                                }
                                else
                                {
                                    scrIntroSection = false;
                                }
                            }

                        }
                    }
                }
            }

            // Fill in empty <title> elements if needed
            string titleTag = "title";
            XmlNodeList titleNodeList = xmldoc.GetElementsByTagName(titleTag);
            if (titleNodeList.Count > 0)
            {
                XmlNode item = titleNodeList[0];
                if (item.InnerText.Length == 0)
                {
                    XmlNode parent = item.ParentNode;
                    XmlNode title = xmldoc.CreateNode("element", "title", null);
                    title.InnerText = Path.GetFileNameWithoutExtension(xmldoc.BaseURI);
                    parent.AppendChild(title);
                    item.ParentNode.RemoveChild(item);
                }
            }
        }

        public void InsertFolderNameForAudioFilesinXhtml()
        {
            try
            {
                XmlDocument xdoc = Common.DeclareXMLDocument(true);
                XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xdoc.NameTable);
                namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
                xdoc.Load(_xhtmlFileNameWithPath);
                string xPath = "//xhtml:span[@lang='fr-Zxxx-x-audio']";
                XmlNodeList chapterSectionIDs = xdoc.SelectNodes(xPath, namespaceManager);
                if (chapterSectionIDs == null) return;
                for (int i = 0; i < chapterSectionIDs.Count; i++)
                {
                    chapterSectionIDs[i].InnerText = "[audio src = \"" + "AudioVisual/" + chapterSectionIDs[i].InnerText.Trim().Replace(" ", "%20") + "\" options=\"controls\"]";
                }
                xdoc.Save(_xhtmlFileNameWithPath);
            }
            catch { }

        }

        #endregion

        #region CSS PreProcessor
        /// <summary>
        /// To replace the symbol string if the symbol matches with the text
        /// </summary>
        public void ReplaceStringInCss(string cssFile)
        {
            var sr = new StreamReader(cssFile);
            string fileContent = sr.ReadToEnd();
            sr.Close();
            int searchPos = fileContent.Length;
            while (true)
            {
                int findFrom = fileContent.LastIndexOf(".paragraph", searchPos, StringComparison.OrdinalIgnoreCase);
                if (findFrom == -1)
                {
                    break;
                }
                else if (ClassPos(fileContent, findFrom))
                {
                    int closingbracePos = fileContent.IndexOf("}", findFrom);
                    fileContent = fileContent.Insert(closingbracePos - 1, "text-indent:0pt;");
                    var sw = new StreamWriter(cssFile);
                    sw.Write(fileContent);
                    sw.Close();
                    break;
                }
                searchPos = findFrom - 1;
            }
        }

        /// <summary>
        /// To replace the symbol string if the symbol matches with the text
        /// </summary>
        public void ReplaceStringInFile(string replaceinFileName, string existingContent, string replacingContent)
        {
            if (File.Exists(replaceinFileName))
            {
                var sr = new StreamReader(replaceinFileName);
                string fileContent = sr.ReadToEnd();
                sr.Close();

                if (fileContent.Contains(existingContent))
                {
                    fileContent = fileContent.Replace(existingContent, replacingContent);
                }
                var sw = new StreamWriter(replaceinFileName);
                sw.Write(fileContent);
                sw.Close();
            }
        }

        public void RemoveStringInCss(string cssFileName, string match)
        {
            if (File.Exists(cssFileName))
            {
	            string fileContent = string.Empty;
	            using (var sr = new StreamReader(cssFileName))
	            {
		            fileContent = sr.ReadToEnd();
		            sr.Close();
		            int searchPos = fileContent.Length;
		            while (true)
		            {
			            int findFrom = fileContent.LastIndexOf(match, searchPos, StringComparison.OrdinalIgnoreCase);
			            if (findFrom == -1)
			            {
				            break;
			            }
			            int closingbracePos = fileContent.IndexOf(";", findFrom) + 1;
			            fileContent = fileContent.Substring(0, findFrom) + fileContent.Substring(closingbracePos);
			            searchPos = findFrom - 1;
		            }
	            }
	            using (var sw = new StreamWriter(cssFileName))
	            {
		            sw.Write(fileContent);
		            sw.Close();
	            }
            }
        }

        public void RemoveDeclaration(string cssFileName, string match)
        {
	        if (!File.Exists(cssFileName))
		        return;

            var sr = new StreamReader(cssFileName);
            string fileContent = sr.ReadToEnd();
            sr.Close();
            int searchPos = fileContent.Length;
            while (true)
            {
                int findTop = fileContent.LastIndexOf(match, searchPos, StringComparison.OrdinalIgnoreCase);
                if (findTop == -1)
                {
                    break;
                }
                int closingbracePos = fileContent.IndexOf("}", findTop) + 1;
                fileContent = fileContent.Substring(0, findTop) + fileContent.Substring(closingbracePos);
                searchPos = findTop - 1;
            }
            var sw = new StreamWriter(cssFileName);
            sw.Write(fileContent);
            sw.Close();
        }

        public void SetDropCapInCSS(string cssFileName)
        {
            TextWriter tw = new StreamWriter(cssFileName, true);
            tw.WriteLine(".Chapter_Number {");
            tw.WriteLine("font-size: 199%;");
            tw.WriteLine("}");

            tw.WriteLine(".Title_Secondary {");
            tw.WriteLine("text-align: center;");
            tw.WriteLine("}");

            tw.WriteLine(".Line1 {");
            tw.WriteLine("text-indent: 30pt;");
            tw.WriteLine("}");

            tw.WriteLine(".Line2 {");
            tw.WriteLine("text-indent: 60pt;");
            tw.WriteLine("}");

            tw.Close();
        }

        public void InsertPropertyInCSS(string cssFileName)
        {
            //DropCap Property
            TextWriter tw = new StreamWriter(cssFileName, true);
            tw.WriteLine(".Chapter_Number {");
            tw.WriteLine("font-size: 199%;");
            tw.WriteLine("}");

            //Footnote marker Property
            tw.WriteLine(".Note_CrossHYPHENReference_Paragraph::footnote-marker {");
            tw.WriteLine("content: attr(title);");
            tw.WriteLine("}");

            tw.WriteLine(".Note_General_Paragraph::footnote-marker {");
            tw.WriteLine("content: attr(title);");
            tw.WriteLine("}");

            tw.WriteLine(".scrBookName { display: block; font-size: 0pt; string-set: bookname content();}");

            //Picture Property
            tw.WriteLine(".pictureRight {");
            tw.WriteLine("padding: 10pt;");
            tw.WriteLine("}");

            tw.WriteLine(".pictureLeft {");
            tw.WriteLine("padding: 10pt;");
            tw.WriteLine("}");

            tw.WriteLine(".pictureCenter {");
            tw.WriteLine("padding: 10pt;");
            tw.WriteLine("}");

            tw.WriteLine(".picturePage {");
            tw.WriteLine("padding: 10pt;");
            tw.WriteLine("}");

            //Space adjustment between letHead and LetData
            tw.WriteLine(".letData {");
            tw.WriteLine("padding: 20pt;");
            tw.WriteLine("}");

            //Avoid letHead as lastline of the page
            tw.WriteLine(".letHead {");
            tw.WriteLine("page-break-after: avoid;");
            tw.WriteLine("}");

            tw.WriteLine(".picture {");
            tw.WriteLine("height: 1.0in;");
            tw.WriteLine("}");

            tw.Close();
        }

        public void InsertPropertyForXelatexCss(string cssFileName)
        {
            TextWriter tw = new StreamWriter(cssFileName, true);
            tw.WriteLine(".Intro_Paragraph {");
            tw.WriteLine("line-height: 0pt;");
            tw.WriteLine("-ps-fixed-line-height: 0pt;");
            tw.WriteLine("}");


            tw.WriteLine(".Intro_Section_Head {");
            tw.WriteLine("padding-top: 8pt;");
            tw.WriteLine("padding-bottom: 4pt;");
            tw.WriteLine("}");

            tw.Close();
        }

        public void InsertCoverPageImageStyleInCSS(string cssFileName)
        {
            TextWriter tw = new StreamWriter(cssFileName, true);
            tw.WriteLine(".Cover {");
            tw.WriteLine("vertical-align: middle;");
            tw.WriteLine("text-align: center;");
            tw.WriteLine("}");
            tw.WriteLine("");
            tw.WriteLine(".Cover img{");
            tw.WriteLine("height: 100%;");
            tw.WriteLine("width: 100%;");
            tw.WriteLine("}");
            tw.Close();
        }

        public void SetHideChapterNumberInCSS()
        {
            TextWriter tw = new StreamWriter(_cssFileNameWithPath, true);
            tw.WriteLine(".hide_Chapter_Number {");
            tw.WriteLine("font-size: 0.1pt;");
            tw.WriteLine("text-indent: 0pt;");
            tw.WriteLine("line-height: 0.1pt;");
            tw.WriteLine("margin-left: 0pt;");
            tw.WriteLine("float: left;");
            tw.WriteLine("}");
            tw.Close();
        }

        public void SetHideVerseNumberInCSS()
        {
            TextWriter tw = new StreamWriter(_cssFileNameWithPath, true);
            tw.WriteLine(".hide_Verse_Number1 {");
            tw.WriteLine("font-size: 0.1pt;");
            tw.WriteLine("visibility: hidden;");
            tw.WriteLine("}");

            tw.WriteLine(".hide_Verse_Number {");
            tw.WriteLine("font-size: 0.1pt;");
            tw.WriteLine("visibility: hidden;");
            tw.WriteLine("}");

            tw.WriteLine(".PWHide {");
            tw.WriteLine("font-size: 0.1pt;");
            tw.WriteLine("visibility: hidden;");
            tw.WriteLine("}");
            tw.Close();
        }

        public void InsertKeepWithNextOnStyles(string _cssFileNameWithPath)
        {
            TextWriter tw = new StreamWriter(_cssFileNameWithPath, true);
            tw.WriteLine(".Front_Matter {");
            tw.WriteLine("margin-left : 12pt;margin-right: 12pt;direction: ltr;text-align: left;");
            tw.WriteLine("}");

            tw.WriteLine(".hideDiv {");
            tw.WriteLine("page-break-before:always;");
            tw.WriteLine("display: none;");
            tw.WriteLine("}");



            if (_projInfo.ProjectInputType.ToLower() == "scripture")
            {
				tw.WriteLine(".Section_Head {");
				tw.WriteLine("page-break-after:avoid;");
				tw.WriteLine("orphans:2;");
				tw.WriteLine("}");

				tw.WriteLine(".iot {");
				tw.WriteLine("page-break-after:avoid;");
				tw.WriteLine("widows:2;");
				tw.WriteLine("}");

                tw.WriteLine(".scrBookName {");
                tw.WriteLine("display: inline;");
                tw.WriteLine("font-size: 0pt;");
                tw.WriteLine("color: #ffffff;");
                tw.WriteLine("}");

                tw.WriteLine(".pictureColumn {");
                tw.WriteLine("width: 99%;");
                tw.WriteLine("}");

                tw.WriteLine(".frtColumns {");
                tw.WriteLine("column-count: 1;");
                tw.WriteLine("column-gap: 12pt;");
                tw.WriteLine("column-fill: balance;");
                tw.WriteLine("}");

                tw.WriteLine(".h2 {");
                tw.WriteLine("display: none;");
                tw.WriteLine("}");

                tw.WriteLine(".h3 {");
                tw.WriteLine("display: none;");
                tw.WriteLine("}");
            }
            tw.Close();
        }

		public void ArrangeImages(string inputType, string xhtmlFilePath)
        {
			if (inputType.ToLower() != "dictionary") return;

			if (!File.Exists(xhtmlFilePath)) return;
            if (_projInfo !=null && _projInfo.SplitFileByLetter != null && _projInfo.SplitFileByLetter.ToLower() == "true") return;
            XmlDocument xDoc = Common.DeclareXMLDocument(true);
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xDoc.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
			xDoc.Load(xhtmlFilePath);
            string xPath = "//div[@class='entry']/div[2]/img"; //Find the second image entry
            XmlNodeList entryLists = xDoc.SelectNodes(xPath, namespaceManager);
            if (entryLists.Count > 0)
            {
                for (int i = 0; i < entryLists.Count; i++)
                {
                    XmlNode pictureNode = entryLists[i].ParentNode;
                    XmlNode entryXmlNode = pictureNode.ParentNode;
                    xPath = ".//div/img"; //Find the second image entry
                    XmlNodeList imageLists = entryXmlNode.SelectNodes(xPath, namespaceManager);
                    if (imageLists.Count > 0)
                    {
                        for (int j = 0; j < imageLists.Count; j++)
                        {
                            var parentNode = imageLists[j].ParentNode;
                            if (parentNode != null)
                            {
                                if (parentNode.Attributes != null)
                                    parentNode.Attributes["class"].Value = "pictureCenter";
                                entryXmlNode.InsertAfter(parentNode.Clone(), entryXmlNode.LastChild);
                                if (parentNode.ParentNode != null) parentNode.ParentNode.RemoveChild(parentNode);
                            }
                        }
                    }
                }
            }
	        xPath = "//div[@class='entry']/*/img";
			entryLists = xDoc.SelectNodes(xPath);
			if (entryLists.Count > 0)
			{
				for (int i = 0; i < entryLists.Count; i++)
				{
					XmlNode pictureNode = entryLists[i].ParentNode;
					XmlNode entryXmlNode = pictureNode.ParentNode;
					XmlNode nextXmlNode = pictureNode.NextSibling;
					if ((nextXmlNode != null && nextXmlNode.Attributes != null) && (nextXmlNode.Attributes["class"].Value == "subentries"))
					{
						if (pictureNode.Attributes != null)
							pictureNode.Attributes["class"].Value = "pictureCenter";
						entryXmlNode.InsertAfter(pictureNode.Clone(), entryXmlNode.LastChild);
						if (pictureNode.ParentNode != null) pictureNode.ParentNode.RemoveChild(pictureNode);
					}
				}
			}
			xDoc.Save(xhtmlFilePath);
        }

		public void InsertBookPageBreak(string _refFormat)
        {
            if (_projInfo.ProjectInputType.ToLower() != "scripture")
            { return; }

			var strBookType = string.Empty;
			if (_refFormat.ToLower().IndexOf("gen 1") == 0)
				strBookType = "scrBookCode";
			else
				strBookType = "scrBookName";

            if (!File.Exists(_projInfo.DefaultXhtmlFileWithPath)) return;
            XmlDocument xDoc = Common.DeclareXMLDocument(true);
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xDoc.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            xDoc.Load(_projInfo.DefaultXhtmlFileWithPath);
            string bookName = string.Empty;
            const string xPath = "//div[@class='scrBook']";
            XmlNodeList bookLists = xDoc.SelectNodes(xPath, namespaceManager);
            if (bookLists != null && bookLists.Count > 0)
            {
                for (int i = 0; i < bookLists.Count; i++)
                {
                    XmlNode divNode = xDoc.CreateElement("div");
                    XmlAttribute xmlAttribute = xDoc.CreateAttribute("class");
                    xmlAttribute.Value = "hideDiv";
                    if (divNode.Attributes != null) divNode.Attributes.Append(xmlAttribute);
                    divNode.InnerText = " ";
                    bookLists[i].InsertBefore(divNode, bookLists[i].FirstChild);

                    XmlNode getBookName = bookLists[i].ParentNode;
                    XmlNodeList bookNodeList = getBookName.SelectNodes("//span[@class='" + strBookType + "']", namespaceManager);
	                if (bookNodeList != null && bookNodeList.Count > 0)
	                {
		                bookName = bookNodeList.Item(i).InnerText;
	                }
	                XmlNode divReferenceNode = xDoc.CreateElement("div");
                    XmlAttribute xmlReferenceAttribute = xDoc.CreateAttribute("class");
                    xmlReferenceAttribute.Value = "BookReferenceDiv";
                    if (divReferenceNode.Attributes != null) divReferenceNode.Attributes.Append(xmlReferenceAttribute);
                    divReferenceNode.InnerText = bookName;
                    if (i == 0)
                    {
                        bookLists[i].InsertBefore(divReferenceNode, bookLists[i].FirstChild);
                    }
                    else
                    {
                        //bookLists[i-1].InsertAfter(divReferenceNode, bookLists[i-1].LastChild);
                        XmlNodeList columnList = bookLists[i - 1].SelectNodes(".//div[@class='columns']", namespaceManager);
                        if (columnList != null && columnList.Count > 0)
                        {
                            columnList[columnList.Count - 1].InsertAfter(divReferenceNode, columnList[columnList.Count-1].LastChild);
                        }
                    }

                }
            }
            xDoc.Save(_projInfo.DefaultXhtmlFileWithPath);
        }

        public void InsertEmptyDiv(string fileName)
        {
            string flexRevFileName = Common.PathCombine(Path.GetDirectoryName(fileName), "FlexRev.xhtml");
            if (!File.Exists(flexRevFileName)) return;
            XmlDocument xDoc = Common.DeclareXMLDocument(true);
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xDoc.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            xDoc.Load(flexRevFileName);
            string xPath = "//xhtml:body";//body
            XmlNodeList RevFormNodes = xDoc.SelectNodes(xPath, namespaceManager);

            if (RevFormNodes.Count > 0)
            {
                XmlNode divNode = xDoc.CreateElement("div");
                XmlAttribute xmlAttribute = xDoc.CreateAttribute("class");
                xmlAttribute.Value = "hideDiv";
                if (divNode.Attributes != null) divNode.Attributes.Append(xmlAttribute);
                divNode.InnerText = " ";
                RevFormNodes[0].InsertBefore(divNode, RevFormNodes[0].FirstChild);
            }
            xDoc.Save(flexRevFileName);

        }

        public void InsertSpanAfterLetter(string fileName)
        {
            string flexRevFileName = Common.PathCombine(Path.GetDirectoryName(fileName), "FlexRev.xhtml");
            string letterLang = "en";
            if (!File.Exists(flexRevFileName)) return;
            XmlDocument xDoc = Common.DeclareXMLDocument(true);
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xDoc.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            xDoc.Load(flexRevFileName);
            string entryPath = "//xhtml:div[@class='entry'][1]";
            XmlNodeList RevFormNodes = xDoc.SelectNodes(entryPath, namespaceManager);
            if (RevFormNodes.Count > 0)
            {
                string xPath = "//xhtml:span";
                XmlNodeList spanList = xDoc.SelectNodes(xPath, namespaceManager);
                if (spanList.Count > 0)
                {
                    foreach (XmlNode item in spanList)
                    {
                        if(item.Attributes["class"] != null && item.Attributes["lang"] != null)
                        {
                            if(item.Attributes["class"].Value.ToLower().IndexOf("reversal-form") == 0)
                            {
                                letterLang = item.Attributes["lang"].Value;
                                break;
                            }
                        }
                    }
                }
            }

            string letterPath = "//xhtml:div[@class=\"letter\"]";
            XmlNodeList letterList = xDoc.SelectNodes(letterPath, namespaceManager);
            if (letterList.Count > 0)
            {
                foreach (XmlNode letter in letterList)
                {
                    letter.InnerXml = "<span lang=\"" + letterLang + "\">" + letter.InnerText + "</span>";
                }
            }
            xDoc.Save(flexRevFileName);
        }

        /// <summary>
        /// Appends the product / assembly version to the .css file for field troubleshooting
        /// (this allows us to see what versions of the software the user has installed).
        /// </summary>
        private void AddProductVersionToCSS()
        {
            TextWriter tw = new StreamWriter(_cssFileNameWithPath, true);
            var sb = new StringBuilder();
            sb.Append("/* Preprocessed CSS - called by ");
            sb.Append(Application.ProductName);
            sb.Append(" v.");
            sb.Append(Application.ProductVersion);
            sb.Append(" (pathway module: ");
            sb.Append(Assembly.GetCallingAssembly().FullName);
            sb.AppendLine(") */");
            tw.WriteLine(sb.ToString());
            tw.Close();
        }

        public void RemoveBrokenImage()
        {
            string sourcePicturePath = Path.GetDirectoryName(_baseXhtmlFileNameWithPath);
            string metaname = Common.GetBaseValue(_baseXhtmlFileNameWithPath);
            if (metaname.Length == 0)
            {
                metaname = Common.GetMetaValue(_baseXhtmlFileNameWithPath);
            }
            try
            {
                XmlDocument xDoc = Common.DeclareXMLDocument(false);
                XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xDoc.NameTable);
                namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
                xDoc.Load(_baseXhtmlFileNameWithPath);
                XmlElement elmRoot = xDoc.DocumentElement;
                string[] pictureClass = {
                                        "pictureColumn", "picturePage", "pictureRight ", "pictureLeft",
                                        "pictureCenter"
                                    };

                foreach (string clsName in pictureClass)
                {
                    string xPath = "//xhtml:div[@class='" + clsName + "']";
                    if (elmRoot != null)
                    {
                        XmlNodeList pictureNode = elmRoot.SelectNodes(xPath, namespaceManager);
                        if (pictureNode != null && pictureNode.Count > 0)
                        {
                            for (int i = 0; i < pictureNode.Count; i++)
                            {
                                xPath = ".//xhtml:img";
                                XmlNode pNode = pictureNode[i].SelectSingleNode(xPath, namespaceManager);
                                if (pNode != null && pNode.Attributes["src"] != null)
                                {
                                    string pictureFile = pNode.Attributes["src"].Value;
                                    if (_projInfo.ProjectInputType.ToLower() == "scripture")
                                    {
										var paraTextprojectPath = Common.GetParatextProjectPath();
                                        pictureFile = Common.PathCombine(paraTextprojectPath, pictureFile);
                                    }

                                    string fromFileName = Common.GetPictureFromPath(pictureFile, metaname, sourcePicturePath);
                                    if (!File.Exists(fromFileName))
                                    {
                                        if (pictureNode[i].ParentNode != null)
                                            pictureNode[i].ParentNode.RemoveChild(pictureNode[i]);
                                    }
                                }
                            }
                        }
                    }
                }
                xDoc.Save(_baseXhtmlFileNameWithPath);
            }
            catch
            {

            }
        }

        #endregion
    }
}
