#region // Copyright (C) 2014, SIL International. All Rights Reserved.
// --------------------------------------------------------------------------------------------
// <copyright file="EpubManifest.cs" from='2009' to='2014' company='SIL International'>
//      Copyright (C) 2014, SIL International. All Rights Reserved.
//
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed:
//
// <remarks>
// </remarks>
// --------------------------------------------------------------------------------------------
#endregion

#region using

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using SIL.PublishingSolution;
using SIL.Tool;

#endregion using

namespace epubConvert
{
    public class EpubManifest
    {
        #region Setup & Class data
        private readonly Exportepub _parent;
        private readonly EpubFont _epubFont;

        public string Title { get; private set; }
        private string Creator { get; set; }
        private string Description { get; set; }
        private string Publisher { get; set; }
        private string Coverage { get; set; }
        private string Rights { get; set; }
        private string Format { get; set; }
        private string Source { get; set; }

        public EpubManifest(Exportepub exportepub, EpubFont epubFont)
        {
            _parent = exportepub;
            _epubFont = epubFont;
        }
        #endregion Setup & Class data

        #region void CreateOpf(PublicationInformation projInfo, string contentFolder, Guid bookId)
        /// <summary>
        /// Generates the manifest and metadata information file used by the .epub reader
        /// (content.opf). For more information, refer to <see cref="http://www.idpf.org/doc_library/epub/OPF_2.0.1_draft.htm#Section2.0"/>
        /// </summary>
        /// <param name="projInfo">Project information</param>
        /// <param name="contentFolder">Content folder (.../OEBPS)</param>
        /// <param name="bookId">Unique identifier for the book we're generating.</param>
        public void CreateOpf(PublicationInformation projInfo, string contentFolder, Guid bookId)
        {
            XmlWriter opf = XmlWriter.Create(Common.PathCombine(contentFolder, "content.opf"));
            opf.WriteStartDocument();
            // package name
            opf.WriteStartElement("package", "http://www.idpf.org/2007/opf");

            opf.WriteAttributeString("version", "2.0");
            opf.WriteAttributeString("unique-identifier", "BookId");

            // metadata - items defined by the Dublin Core Metadata Initiative:
            Metadata(projInfo, bookId, opf);
            StartManifest(opf);
            if (_parent.EmbedFonts)
            {
                ManifestFontEmbed(opf);
            }
            string[] files = Directory.GetFiles(contentFolder);
            ManifestContent(opf, files, "epub2");
			opf.WriteEndElement(); // manifest
			Spine(opf, files);
			Guide(projInfo, opf, files, "epub2");
            opf.WriteEndElement(); // package
            opf.WriteEndDocument();
            opf.Close();
        }

        /// <summary>
        /// Generates the manifest and metadata information file used by the .epub reader
        /// (content.opf). For more information, refer to <see cref="http://www.idpf.org/doc_library/epub/OPF_2.0.1_draft.htm#Section2.0"/>
        /// </summary>
        /// <param name="projInfo">Project information</param>
        /// <param name="contentFolder">Content folder (.../OEBPS)</param>
        /// <param name="bookId">Unique identifier for the book we're generating.</param>
        public void CreateOpfV3(PublicationInformation projInfo, string contentFolder, Guid bookId)
        {
            LoadPropertiesFromSettings();
            XmlWriter opf = XmlWriter.Create(Common.PathCombine(contentFolder, "content.opf"));
            opf.WriteStartDocument();
            // package name
            opf.WriteStartElement("package", "http://www.idpf.org/2007/opf");
            opf.WriteAttributeString("version", "3.0");
            opf.WriteAttributeString("xml", "lang", null, "en");
            opf.WriteAttributeString("unique-identifier", "pub-id");
            opf.WriteAttributeString("prefix", "rendition: http://www.idpf.org/vocab/rendition/#");

            // metadata - items defined by the Dublin Core Metadata Initiative:
            MetadataV3(projInfo, bookId, opf);
            StartManifestV3(opf);
            if (_parent.EmbedFonts)
            {
                ManifestFontEmbed(opf);
            }
            string[] files = Directory.GetFiles(contentFolder);
            ManifestContent(opf, files, "epub3");
	        var avFolder = Path.Combine(contentFolder, "AudioVisual");
	        if (Directory.Exists(avFolder))
	        {
		        var avFiles = Directory.GetFiles(avFolder);
				ManifestAvContent(opf, avFiles);
			}
			opf.WriteEndElement(); // manifest
			SpineV3(opf, files);
            Guide(projInfo, opf, files, "epub3");
            opf.WriteEndElement(); // package
            opf.WriteEndDocument();
            opf.Close();
        }

        private void Metadata(PublicationInformation projInfo, Guid bookId, XmlWriter opf)
        {
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

            opf.WriteElementString("dc", "subject", null, _parent.InputType.ToLower() == "dictionary" ? "Reference" : "Religion & Spirituality");

            if (Description.Length > 0)
                opf.WriteElementString("dc", "description", null, Description);

            if (Publisher.Length > 0)
                opf.WriteElementString("dc", "publisher", null, Publisher);

            opf.WriteStartElement("dc", "contributor", null); // authoring program as a "contributor", e.g.:
            opf.WriteAttributeString("opf", "role", null, "bkp");

            opf.WriteValue(Common.GetProductName());
            opf.WriteEndElement();
            opf.WriteElementString("dc", "date", null, DateTime.Today.ToString("yyyy-MM-dd"));
            // .epub standard date format (http://www.idpf.org/2007/opf/OPF_2.0_final_spec.html#Section2.2.7)
            opf.WriteElementString("dc", "type", null, "Text"); //
            if (Format.Length > 0)
                opf.WriteElementString("dc", "format", null, Format);
            if (Source.Length > 0)
                opf.WriteElementString("dc", "source", null, Source);

            if (_epubFont.LanguageCount == 0)
            {
                opf.WriteElementString("dc", "language", null, "en");
            }

            foreach (var lang in _epubFont.LanguageCodes())
            {
                opf.WriteElementString("dc", "language", null, lang);
            }


            if (Coverage.Length > 0)
                opf.WriteElementString("dc", "coverage", null, Coverage);
            if (Rights.Length > 0)
                opf.WriteElementString("dc", "rights", null, Rights);
            opf.WriteStartElement("dc", "identifier", null); // <dc:identifier id="BookId">[guid]</dc:identifier>
            opf.WriteAttributeString("id", "BookId");
            opf.WriteValue("http://pathway.sil.org/"); // bookId.ToString()
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
        }

        private void MetadataV3(PublicationInformation projInfo, Guid bookId, XmlWriter opf)
        {
            // (http://dublincore.org/documents/2004/12/20/dces/)
            opf.WriteStartElement("metadata");
            opf.WriteAttributeString("xmlns", "dc", null, "http://purl.org/dc/elements/1.1/");
            opf.WriteStartElement("dc", "title", null);
            opf.WriteAttributeString("id", "title");

            if (!string.IsNullOrEmpty(Title))
            {
                opf.WriteValue(Title);
            }
            else
            {
                opf.WriteValue(Common.databaseName + " " + projInfo.ProjectName);
            }
            opf.WriteEndElement();

            opf.WriteStartElement("dc", "creator", null); //<dc:creator opf:role="aut">[author]</dc:creator>
            opf.WriteAttributeString("id", "creator");
            if (!string.IsNullOrEmpty(Creator.Trim()))
            {
                opf.WriteValue(Creator);
            }
            else
            {
                opf.WriteValue(Environment.UserName);
            }

            opf.WriteEndElement();

            if (_epubFont != null && _epubFont.LanguageCount == 0)
            {
                opf.WriteElementString("dc", "language", null, "en");
            }

            if (_epubFont != null)
            {
                foreach (var lang in _epubFont.LanguageCodes())
                {
                    opf.WriteElementString("dc", "language", null, lang);
                }
            }

            opf.WriteStartElement("dc", "identifier", null);
            opf.WriteAttributeString("id", "pub-id");
            opf.WriteValue("http://pathway.sil.org/");
            //opf.WriteValue(bookId.ToString());
            opf.WriteEndElement();

            if (!string.IsNullOrEmpty(Source.Trim()))
                opf.WriteElementString("dc", "source", null, Source);
            else
                opf.WriteElementString("dc", "source", null, "Epub3 Source");

            opf.WriteStartElement("meta");
            opf.WriteAttributeString("property", "dcterms:modified");
            opf.WriteValue(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"));
            opf.WriteEndElement();

            opf.WriteElementString("dc", "date", null, DateTime.Now.Year.ToString(CultureInfo.InvariantCulture));

            if (!string.IsNullOrEmpty(Publisher.Trim()))
                opf.WriteElementString("dc", "publisher", null, Publisher);
            else
                opf.WriteElementString("dc", "publisher", null, "Epub3");

            opf.WriteStartElement("dc", "contributor", null); // authoring program as a "contributor", e.g.:
            opf.WriteAttributeString("id", "contributor-id");
            opf.WriteValue(Common.GetProductName());
            opf.WriteEndElement();

            if (!string.IsNullOrEmpty(Rights.Trim()))
                opf.WriteElementString("dc", "rights", null, Rights);
            else
                opf.WriteElementString("dc", "rights", null, "Epub3");

            if (!string.IsNullOrEmpty(Description.Trim()))
                opf.WriteElementString("dc", "description", null, Description);
            else
                opf.WriteElementString("dc", "description", null, "Epub3 Export");

            if (!string.IsNullOrEmpty(Coverage.Trim()))
                opf.WriteElementString("dc", "coverage", null, Coverage);
            else
                opf.WriteElementString("dc", "coverage", null, "Epub3");


            opf.WriteElementString("dc", "subject", null, _parent.InputType.ToLower() == "dictionary" ? "Reference" : "Religion & Spirituality");


            opf.WriteElementString("dc", "type", null, "Text");


            // cover image (optional)
            if (Param.GetMetadataValue(Param.CoverPage).ToLower().Equals("true"))
            {
                opf.WriteStartElement("meta");
                opf.WriteAttributeString("name", "cover");
                opf.WriteAttributeString("content", "cover-image");
                opf.WriteEndElement(); // meta
            }

            opf.WriteStartElement("meta");
            opf.WriteAttributeString("property", "rendition:layout");
            opf.WriteValue("reflowable");
            opf.WriteEndElement(); // meta

            opf.WriteStartElement("meta");
            opf.WriteAttributeString("property", "rendition:orientation");
            opf.WriteValue("auto");
            opf.WriteEndElement(); // meta

            opf.WriteStartElement("meta");
            opf.WriteAttributeString("property", "rendition:spread");
            opf.WriteValue("auto");
            opf.WriteEndElement(); // meta

            opf.WriteEndElement(); // metadata
        }

        private static void StartManifest(XmlWriter opf)
        {
            // manifest
            opf.WriteStartElement("manifest");
            // (individual "item" elements in the manifest)
            opf.WriteStartElement("item");
            opf.WriteAttributeString("id", "ncx");
            opf.WriteAttributeString("href", "toc.ncx");
            opf.WriteAttributeString("media-type", "application/x-dtbncx+xml");
            opf.WriteEndElement(); // item
        }

        private static void StartManifestV3(XmlWriter opf)
        {
            // manifest <item id="toc" properties="nav" href="TOC.xhtml" media-type="application/xhtml+xml"/>
            opf.WriteStartElement("manifest");
            // (individual "item" elements in the manifest)
            opf.WriteStartElement("item");
            opf.WriteAttributeString("href", "toc.xhtml");
            opf.WriteAttributeString("id", "nav");
            opf.WriteAttributeString("properties", "nav");
            opf.WriteAttributeString("media-type", "application/xhtml+xml");

            opf.WriteEndElement(); // item
        }

        private void ManifestFontEmbed(XmlWriter opf)
        {
            int fontNum = 1;
            foreach (var embeddedFont in _epubFont.EmbeddedFonts())
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
                if (_parent.IncludeFontVariants)
                {
                    // italic
                    if (embeddedFont.HasItalic && String.Compare(embeddedFont.Filename, embeddedFont.ItalicFilename, StringComparison.Ordinal) != 0)
                    {
                        if (embeddedFont.ItalicFilename != string.Empty)
                        {
                            opf.WriteStartElement("item"); // item (charis embedded font)
                            opf.WriteAttributeString("id", "epub.embedded.font_i_" + fontNum);

                            opf.WriteAttributeString("href", Path.GetFileName(embeddedFont.ItalicFilename));

                            opf.WriteAttributeString("media-type", "font/opentype/");
                            opf.WriteEndElement(); // item
                            fontNum++;
                        }
                    }
                    // bold
                    if (embeddedFont.HasBold && String.Compare(embeddedFont.Filename, embeddedFont.BoldFilename, StringComparison.Ordinal) != 0)
                    {
                        if (embeddedFont.BoldFilename != string.Empty)
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
        }

        private void ManifestContent(XmlWriter opf, IEnumerable<string> files, string epubVersion)
        {
            var listIdRef = new List<string>();
            int counterSet = 1;
            foreach (string file in files)
            {
                // iterate through the file set and add <item> elements for each xhtml file
                string name = Path.GetFileName(file);
                Debug.Assert(name != null);
                string nameNoExt = Path.GetFileNameWithoutExtension(file);

                if (name.EndsWith(".xhtml") || name.EndsWith(".html"))
                {
                    // is this the cover page?
                    if (name.StartsWith(PreExportProcess.CoverPageFilename.Substring(0, 8)))
                    {
                        // yup - write it out and go to the next item
                        opf.WriteStartElement("item");
                        opf.WriteAttributeString("id", "cover");
                        //if (epubVersion == "epub3")
                        //{
                        // opf.WriteAttributeString("properties", "scripted");
                        //}
                        opf.WriteAttributeString("href", name);
                        opf.WriteAttributeString("media-type", "application/xhtml+xml");
                        opf.WriteEndElement(); // item
                        continue;
                    }

                    if (nameNoExt != null && nameNoExt.ToLower() == "toc")
                    {
                        continue;
                    }

                    // if we can, write out the "user friendly" book name in the TOC
                    string fileId = _parent.GetBookId(file);

                    string idRefValue;
                    if (listIdRef.Contains(fileId))
                    {
                        listIdRef.Add(fileId + counterSet.ToString(CultureInfo.InvariantCulture));
                        idRefValue = fileId + counterSet.ToString(CultureInfo.InvariantCulture);
                        counterSet++;
                    }
                    else
                    {
                        listIdRef.Add(fileId);
                        idRefValue = fileId;
                    }

                    opf.WriteStartElement("item");
                    // the book ID can be wacky (and non-unique) for dictionaries. Just use the filename.
                    var itemId = _parent.InputType.ToLower() == "dictionary" ? nameNoExt : idRefValue;
                    opf.WriteAttributeString("id", itemId);
                    opf.WriteAttributeString("href", name);
                    opf.WriteAttributeString("media-type", "application/xhtml+xml");
					if (epubVersion == "epub3" && isScripted(file))
					{
						opf.WriteAttributeString("properties", "scripted");
					}
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
                    if (nameNoExt != null && epubVersion == "epub3" && nameNoExt.ToLower() == "cover")
                    {
                        opf.WriteAttributeString("id", nameNoExt + "image");
                        opf.WriteAttributeString("properties", "cover-image");
                    }
                    else
                    {
                        opf.WriteAttributeString("id", "image" + nameNoExt);
                    }

                    opf.WriteAttributeString("href", name);
                    if (nameNoExt != null && nameNoExt.Contains("sil-bw-logo"))
                    {
                        opf.WriteAttributeString("media-type", "image/png");
                    }
                    else
                    {
                        opf.WriteAttributeString("media-type", "image/jpeg");
                    }

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
                    if (nameNoExt != null && epubVersion == "epub3" && nameNoExt.ToLower() == "cover")
                    {
                        opf.WriteAttributeString("properties", "cover-image");
                    }
                    opf.WriteAttributeString("media-type", "image/png");
                    opf.WriteEndElement(); // item
                }
                else if (nameNoExt != null && name.ToLower() == "toc.ncx")
                {
                    opf.WriteStartElement("item");
                    opf.WriteAttributeString("href", name);
                    opf.WriteAttributeString("id", "ncx");

                    opf.WriteAttributeString("media-type", "application/x-dtbncx+xml");
                    opf.WriteEndElement(); // item
                }
            }
        }

		private static Dictionary<string, string> _audioMime = new Dictionary<string, string>
		{ {".wav", "audio/vnd.wav"}, {".mp3", "audio/mpeg3"}, {".ogg", "audio/ogg"}, {".mp4", "video/mp4"}, {".avi", "video/avi"}, {".3gp", "video/3gp"} } ;
		/// <summary>
		/// Adds audio files to the opf xml file
		/// </summary>
		protected void ManifestAvContent(XmlWriter opf, IEnumerable<string> files)
		{
			int counterSet = 1;

			foreach (string file in files)
			{
				// iterate through the file set and add <item> elements for each xhtml file
				string name = Path.GetFileName(file);
				Debug.Assert(name != null);
				var ext = Path.GetExtension(file).ToLower();

				if (_audioMime.ContainsKey(ext))
				{
					if (_audioMime[ext].StartsWith("video"))
					{
						File.Delete(file);
						continue;
					}
					opf.WriteStartElement("item"); // item (image)
					opf.WriteAttributeString("id", string.Format("av{0}", counterSet));
					opf.WriteAttributeString("href", Path.Combine("AudioVisual",name).Replace("\\","/"));
					opf.WriteAttributeString("media-type", _audioMime[ext]);
					opf.WriteEndElement(); // item
					counterSet += 1;
				}
			}
		}

		protected bool isScripted(string file)
		{
			XmlDocument xmlDocument = Common.DeclareXMLDocument(false);
			var namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
			namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
			var xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, DtdProcessing = DtdProcessing.Parse };
			var xmlReader = XmlReader.Create(file, xmlReaderSettings);
			xmlDocument.Load(xmlReader);
			xmlReader.Close();
			var scriptAttrNode = xmlDocument.SelectSingleNode("//@onclick");
			if (scriptAttrNode == null) return false;
			var audioNodes = xmlDocument.SelectNodes("//xhtml:audio|//xhtml:video", namespaceManager);
			Debug.Assert(audioNodes != null);
			foreach (XmlElement node in audioNodes)
			{
				if (!string.IsNullOrEmpty(node.InnerText.Trim())) continue;
				var fc = node.FirstChild;
				if (fc?.NodeType != XmlNodeType.Element) continue;
				var fce = fc as XmlElement;
				Debug.Assert(fce != null);
				if (!fce.HasAttributes) continue;
				if (fce.Attributes["src"] == null) continue;
				if (fce.Attributes["src"].Value.Contains("\\"))
				{
					fce.Attributes["src"].Value = string.Join("/", fce.Attributes["src"].Value.Split('\\'));
				}
				if (!string.IsNullOrEmpty(node.InnerText)) continue;
				var fallBack = xmlDocument.CreateTextNode("Missing " + Path.GetFileName(node.FirstChild.Attributes["src"].Value));
				node.AppendChild(fallBack);
			}
			var xws= new XmlWriterSettings {Indent = false, Encoding = Encoding.UTF8};
			var xw = XmlWriter.Create(file, xws);
			xmlDocument.Save(xw);
			xw.Close();
			return true;
		}

		private void Spine(XmlWriter opf, IEnumerable<string> files)
        {
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

            var listIdRef = new List<string>();
            int counterSet = 1;
            foreach (string file in files)
            {
                // is this the cover page?
                var fileName = Path.GetFileName(file);
                Debug.Assert(fileName != null);
                if (fileName.StartsWith(PreExportProcess.CoverPageFilename.Substring(0, 8)))
                {
                    continue;
                }
                // add an <itemref> for each xhtml file in the set
                if (fileName.EndsWith(".xhtml"))
                {
                    string fileId = _parent.GetBookId(file);
                    string idRefValue;
                    if (listIdRef.Contains(fileId))
                    {
                        var counter = counterSet.ToString(CultureInfo.InvariantCulture);
                        listIdRef.Add(fileId + counter);
                        idRefValue = fileId + counter;
                        counterSet++;
                    }
                    else
                    {
                        listIdRef.Add(fileId);
                        idRefValue = fileId;
                    }
                    //if ((fileId.IndexOf("PartFile") == -1 && _parent.InputType == "dictionary") || _parent.InputType == "scripture")
                    //if (fileId.IndexOf("PartFile") == -1 || _parent.InputType.ToLower() == "scripture")
                    //{
                        opf.WriteStartElement("itemref"); // item (stylesheet)
                        // the book ID can be wacky (and non-unique) for dictionaries. Just use the filename.
                        var idRef = _parent.InputType.ToLower() == "dictionary"
                                        ? Path.GetFileNameWithoutExtension(file)
                                        : idRefValue;
                        opf.WriteAttributeString("idref", idRef);
                        opf.WriteEndElement(); // itemref
                    //}
                }
            }
            opf.WriteEndElement(); // spine
        }

        private void SpineV3(XmlWriter opf, IEnumerable<string> files)
        {
            // spine
            opf.WriteStartElement("spine");
            opf.WriteAttributeString("toc", "ncx");
            opf.WriteAttributeString("page-progression-direction", "ltr");
            // a couple items for the cover image
            if (Param.GetMetadataValue(Param.CoverPage).ToLower().Equals("true"))
            {
                //opf.WriteStartElement("itemref");
                //opf.WriteAttributeString("idref", "cover");
                //opf.WriteAttributeString("linear", "yes");
                //opf.WriteEndElement(); // itemref
            }

            var listIdRef = new List<string>();
            int counterSet = 1;
            foreach (string file in files)
            {
                // is this the cover page?
                var fileName = Path.GetFileName(file);
                Debug.Assert(fileName != null);
                //if (fileName.StartsWith(PreExportProcess.CoverPageFilename.Substring(0, 8)))
                //{
                //    continue;
                //}

                //if (fileName.ToLower().Contains("toc.html"))
                //{
                //    continue;
                //}

                // add an <itemref> for each xhtml file in the set
                if (fileName.EndsWith(".xhtml") || fileName.EndsWith(".html"))
                {
                    string fileId = _parent.GetBookId(file);
                    string idRefValue;
                    if (listIdRef.Contains(fileId))
                    {
                        var counter = counterSet.ToString(CultureInfo.InvariantCulture);
                        listIdRef.Add(fileId + counter);
                        idRefValue = fileId + counter;
                        counterSet++;
                    }
                    else
                    {
                        listIdRef.Add(fileId);
                        idRefValue = fileId;
                    }

                    //if (fileId.IndexOf("PartFile") == -1 || _parent.InputType.ToLower() == "scripture")
                    //{
                        opf.WriteStartElement("itemref"); // item (stylesheet)
                        // the book ID can be wacky (and non-unique) for dictionaries. Just use the filename.
                        var idRef = _parent.InputType.ToLower() == "dictionary"
                                        ? Path.GetFileNameWithoutExtension(file)
                                        : idRefValue;

                        if (idRef != null && idRef.Contains("File0Cvr00000"))
                        {
                            idRef = "cover";
                        }

                        opf.WriteAttributeString("idref", idRef);
                        opf.WriteAttributeString("linear", "yes");
						opf.WriteAttributeString("properties", "rendition:layout-reflowable");
                        opf.WriteEndElement(); // itemref
                    //}
                }
            }
            opf.WriteEndElement(); // spine
        }

        private static void Guide(PublicationInformation projInfo, XmlWriter opf, string[] files, string epubVersion)
        {
            // guide
            opf.WriteStartElement("guide");
            // cover image
            if (Param.GetMetadataValue(Param.CoverPage).Trim().Equals("True"))
            {
                opf.WriteStartElement("reference");
                if (epubVersion == "epub3")
                {
                    opf.WriteAttributeString("href", "File0Cvr00000_.html");
                }
                else
                {
                    opf.WriteAttributeString("href", "File0Cvr00000_.xhtml");
                }
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
                if (files[index].EndsWith(".xhtml") || files[index].EndsWith(".html"))
                {
                    break;
                }
                index++;
            }
            if (index == files.Length) index--; // edge case
            opf.WriteAttributeString("href", Path.GetFileName(files[index]));
            opf.WriteEndElement(); // reference
            opf.WriteEndElement(); // guide
        }
        #endregion void CreateOpf(PublicationInformation projInfo, string contentFolder, Guid bookId)

        #region Property persistence
        /// <summary>
        /// Loads the settings file and pulls out the values we look at.
        /// </summary>
        public void LoadPropertiesFromSettings()
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
            Rights = Common.UpdateCopyrightYear(Rights);
        }
        #endregion

    }
}
