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
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using SIL.Tool;
#endregion

namespace epubConvert
{
    public class EpubToc
    {
        #region setup & data
        private string InputType { get; set; }
        private string TocLevel { get; set; }
        private readonly XslCompiledTransform _addDicTocHeads = new XslCompiledTransform();
        private readonly XslCompiledTransform _fixEpubToc = new XslCompiledTransform();
        private string _currentChapterNumber = string.Empty;
        private readonly bool _isUnixOs = Common.UnixVersionCheck();

        public EpubToc(string inputType, string tocLevel)
        {
            InputType = inputType;
            TocLevel = tocLevel;
            _addDicTocHeads.Load(XmlReader.Create(Common.UsersXsl("addDicTocHeads.xsl")));
            _fixEpubToc.Load(XmlReader.Create(Common.UsersXsl("FixEpubToc.xsl")));
        }
        #endregion setup and data

        /// <summary>
        /// Creates the table of contents file used by .epub readers (toc.ncx).
        /// </summary>
        /// <param name="projInfo">project information</param>
        /// <param name="contentFolder">the content folder (../OEBPS)</param>
        /// <param name="bookId">Unique identifier for the book we're creating</param>
        public void CreateNcx(PublicationInformation projInfo, string contentFolder, Guid bookId)
        {
            // toc.ncx
            string tocFullPath = Common.PathCombine(contentFolder, "toc.ncx");
            XmlWriter ncx = StartNcx(projInfo, bookId, tocFullPath);
            ncx.WriteStartElement("navMap");
            // individual navpoint elements (one for each xhtml)
            string[] files = Directory.GetFiles(contentFolder, "*.xhtml");
            bool isMainOpen = false;
            bool isMainSubOpen = false;
            bool isRevOpen = false;
            bool isRevSubOpen = false;
            bool isScriptureSubOpen = false;
            int index = 1;
            bool skipChapterInfo = TocLevel.StartsWith("1");
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                Debug.Assert(name != null);
                string bookName = GetBookName(file);
                if (name.IndexOf("File", StringComparison.Ordinal) == 0 && name.IndexOf("TOC", StringComparison.Ordinal) == -1)
                {
                    WriteNavPoint(ncx, index.ToString(CultureInfo.InvariantCulture), bookName, name);
                    index++;
                    // chapters within the books (nested as a subhead)
                    if (!skipChapterInfo)
                    {
                        WriteChapterLinks(file, ref index, ncx);
                    }
                    // end the book's navPoint element
                    ncx.WriteEndElement(); // navPoint
                }
                else
                {
                    if (name.IndexOf("TOC", StringComparison.Ordinal) != -1)
                    {
                        WriteNavPoint(ncx, index.ToString(CultureInfo.InvariantCulture), bookName, name);
                        index++;
                    }
                    if (InputType.ToLower() == "dictionary")
                    {
                        DictionaryNcx(ncx, ref isMainOpen, ref isMainSubOpen, ref isRevOpen, ref isRevSubOpen, ref index, skipChapterInfo, file, name, bookName);
                    }
                    else
                    {
                        ScriptureNcx(ncx, ref isScriptureSubOpen, ref index, skipChapterInfo, file, name, ref bookName);
                    }
                }

            }
            if (isRevOpen && InputType.ToLower() == "dictionary")
            {
                // end the book's navPoint element
                ncx.WriteEndElement(); // navPoint TOC
            }
            if (isScriptureSubOpen)
            {
                ncx.WriteEndElement(); // navPoint
            }
            ncx.WriteEndElement(); // navPoint TOC
            ncx.WriteEndElement(); // navmap
            ncx.WriteEndDocument();
            ncx.Close();
            FixPlayOrder(tocFullPath);
            if (InputType.ToLower() == "dictionary")
            {
                Common.ApplyXslt(tocFullPath, _addDicTocHeads);
            }
            Common.ApplyXslt(tocFullPath, _fixEpubToc);
            FixPlayOrder(tocFullPath);
        }

        private void ScriptureNcx(XmlWriter ncx, ref bool isScriptureSubOpen, ref int index, bool skipChapterInfo, string file, string name, ref string bookName)
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
            if (fileNameWithoutExtension != null && (name.IndexOf("TOC", StringComparison.Ordinal) == -1 &&
                                                                   (fileNameWithoutExtension.EndsWith("_") ||
                                                                    fileNameWithoutExtension.EndsWith("_01"))))
            {
                if (isScriptureSubOpen)
                {
                    ncx.WriteEndElement(); // navPoint
                }
                bookName = GetBookName(file);
                ncx.WriteStartElement("navPoint");
                ncx.WriteAttributeString("id", "dtb:uid");
                ncx.WriteAttributeString("playOrder", index.ToString(CultureInfo.InvariantCulture));
                ncx.WriteStartElement("navLabel");
                ncx.WriteElementString("text", bookName);
                ncx.WriteEndElement(); // navlabel
                ncx.WriteStartElement("content");
                ncx.WriteAttributeString("src", name);
                ncx.WriteEndElement(); // meta
                index++;
                // chapters within the books (nested as a subhead)
                if (!skipChapterInfo)
                {
                    WriteChapterLinks(file, ref index, ncx);
                }
                isScriptureSubOpen = true;
            }
            else if (name.IndexOf("zzReference", StringComparison.Ordinal) == 0)
            {
                if (isScriptureSubOpen)
                {
                    ncx.WriteEndElement(); // navPoint
                }
                ncx.WriteStartElement("navPoint");
                ncx.WriteAttributeString("id", "dtb:uid");
                ncx.WriteAttributeString("playOrder", index.ToString(CultureInfo.InvariantCulture));
                ncx.WriteStartElement("navLabel");
                ncx.WriteElementString("text", "Endnotes");
                ncx.WriteEndElement(); // navlabel
                ncx.WriteStartElement("content");
                ncx.WriteAttributeString("src", name);
                ncx.WriteEndElement(); // meta
                index++;
                //// chapters within the books (nested as a subhead)
                //if (!skipChapterInfo)
                //{
                //    WriteEndNoteLinks(file, ref index, ncx);
                //}
            }
            else
            {
                if (!skipChapterInfo)
                {
                    WriteChapterLinks(file, ref index, ncx);
                }
            }
        }

        private void DictionaryNcx(XmlWriter ncx, ref bool isMainOpen, ref bool isMainSubOpen, ref bool isRevOpen, ref bool isRevSubOpen, ref int index, bool skipChapterInfo, string file, string name, string bookName)
        {
            if (name.Contains("PartFile"))
            {
                if (!isMainOpen)
                {
                    isMainOpen = true;
                }
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                if (fileNameWithoutExtension != null && (fileNameWithoutExtension.EndsWith("_") ||
                                                                       fileNameWithoutExtension.EndsWith("_01")))
                {
                    if (isMainSubOpen)
                    {
                        ncx.WriteEndElement(); // navPoint
                    }
                    WriteNavPoint(ncx, index.ToString(CultureInfo.InvariantCulture), bookName, name);
                    index++;
                    isMainSubOpen = true;
                }
                if (!skipChapterInfo)
                {
                    WriteChapterLinks(file, ref index, ncx);
                }
            }
            else if (name.Contains("RevIndex"))
            {
                if (isMainOpen)
                {
                    ncx.WriteEndElement(); // navPoint Main
                    isMainSubOpen = false;
                    isMainOpen = false;
                }
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                if (fileNameWithoutExtension != null && (fileNameWithoutExtension.EndsWith("_") ||
                                                                       fileNameWithoutExtension.EndsWith("_01")))
                {
                    if (isRevSubOpen)
                    {
                        ncx.WriteEndElement(); // navPoint
                    }
                    if (!isRevOpen)
                    {
                        isRevOpen = true;
                    }
                    WriteNavPoint(ncx, index.ToString(CultureInfo.InvariantCulture), bookName, name);
                    index++;
                    isRevSubOpen = true;
                }
                if (!skipChapterInfo)
                {
                    WriteChapterLinks(file, ref index, ncx);
                }
            }
        }

        private static XmlWriter StartNcx(PublicationInformation projInfo, Guid bookId, string tocFullPath)
        {
            XmlWriter ncx = XmlWriter.Create(tocFullPath);
            ncx.WriteStartDocument();
            ncx.WriteStartElement("ncx", "http://www.daisy.org/z3986/2005/ncx/");
            ncx.WriteAttributeString("version", "2005-1");
            ncx.WriteStartElement("head");
            ncx.WriteStartElement("meta");
            ncx.WriteAttributeString("name", "dtb:uid");
            ncx.WriteAttributeString("content", "http://pathway.sil.org/"); //  bookId.ToString()
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
            return ncx;
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
                node.InnerText = n.ToString(CultureInfo.InvariantCulture);
                n += 1;
            }

            if (_isUnixOs)
            {
                nodes = tocDoc.SelectNodes("//@id");
                Debug.Assert(nodes != null);
                n = 1;
                foreach (XmlAttribute node in nodes)
                {
                    node.InnerText = node.InnerText + n.ToString(CultureInfo.InvariantCulture);
                    n += 1;
                }
            }

            var xmlFile = new FileStream(tocFullPath, FileMode.Create);
            XmlWriter writer = XmlWriter.Create(xmlFile);
            tocDoc.Save(writer);
            xmlFile.Close();
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
        /// Writes the chapter links out to the specified XmlWriter (the .ncx file).
        /// </summary>
        /// <returns>List of url strings</returns>
        private void WriteChapterLinks(string xhtmlFileName, ref int playOrder, XmlWriter ncx)
        {
            XmlDocument xmlDocument = Common.DeclareXMLDocument(true);
            var namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            var xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, ProhibitDtd = false }; //Common.DeclareXmlReaderSettings(false);
            var xmlReader = XmlReader.Create(xhtmlFileName, xmlReaderSettings);
            xmlDocument.Load(xmlReader);
            xmlReader.Close();
            bool isSectionHead = false, isChapterNumber = false, isVerseNumber = false;
            string sectionHead = string.Empty, fromChapterNumber = string.Empty, firstVerseNumber = string.Empty, lastVerseNumber = string.Empty;
            var xPath = string.Empty;

	        if (InputType.ToLower() == "dictionary")
	        {
				xPath = string.Format("//xhtml:div[@class='{0}']|//xhtml:div[@class='{1}']|//xhtml:div[@class='{2}']|//xhtml:div[@class='{3}']", "entry", "minorentryvariant", "minorentrycomplex", "reversalindexentry");
	        }
	        else
	        {
				xPath = string.Format("//xhtml:div[@class='{0}']", "scrBook");
	        }

            XmlNodeList nodes = xmlDocument.SelectNodes(xPath, namespaceManager);
            if (nodes != null && nodes.Count > 0)
            {
                var sb = new StringBuilder();
                string name = Path.GetFileName(xhtmlFileName);
                foreach (XmlNode node in nodes)
                {
                    string textString = string.Empty;
                    if (InputType.ToLower().Equals("dictionary"))
                    {
                        if (!WriteDictionaryChapters(ref playOrder, ncx, namespaceManager, sb, name, node,
                                                     ref textString))
                        {
                            continue;
                        }
                    }
                    else // Scripture
                    {
                        WriteScriptureChapters(ref playOrder, ncx, ref isSectionHead, ref isChapterNumber, ref isVerseNumber, ref sectionHead, ref fromChapterNumber, ref firstVerseNumber, ref lastVerseNumber, sb, name, node, ref textString);
                    }
                    // reset the stringbuilder
                    sb.Length = 0;
                }
            }
        }

        private void WriteScriptureChapters(ref int playOrder, XmlWriter ncx, ref bool isSectionHead, ref bool isChapterNumber, ref bool isVerseNumber, ref string sectionHead, ref string fromChapterNumber, ref string firstVerseNumber, ref string lastVerseNumber, StringBuilder sb, string name, XmlNode node, ref string textString)
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
                                if (fromChapterNumber == _currentChapterNumber)
                                {
                                    textString = textString + "-" + lastVerseNumber + ")";
                                }
                                else
                                {
                                    textString = textString + "-" + _currentChapterNumber + ":" +
                                                   lastVerseNumber + ")";
                                }
                                if (textString.Trim().Length >= 4)
                                {
                                    // write out the node
                                    ncx.WriteStartElement("navPoint");
                                    ncx.WriteAttributeString("id", "dtb:uid");
                                    ncx.WriteAttributeString("playOrder", playOrder.ToString(CultureInfo.InvariantCulture));
                                    ncx.WriteStartElement("navLabel");
                                    ncx.WriteElementString("text", textString);
                                    ncx.WriteEndElement(); // navlabel
                                    ncx.WriteStartElement("content");
                                    ncx.WriteAttributeString("src", sb.ToString());
                                    ncx.WriteEndElement(); // meta
                                    playOrder++;
                                    sb.Length = 0;
                                }
                                if (reader.GetAttribute("id") != null)
                                {
                                    sb.Append(name);
                                    sb.Append("#");
                                    sb.Append(reader.GetAttribute("id"));
                                }
                                if (textString.Trim().Length >= 4)
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
                            else if (className != null && className.IndexOf("Verse_Number", StringComparison.Ordinal) == 0)
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
                                _currentChapterNumber = reader.Value;
                                isChapterNumber = false;
                            }
                            if (isVerseNumber)
                            {
                                if (firstVerseNumber.Trim().Length == 0 && sectionHead.Length > 0)
                                {
                                    firstVerseNumber = reader.Value;
                                    fromChapterNumber = _currentChapterNumber;
                                    textString = sectionHead + "(" + _currentChapterNumber + ":" + firstVerseNumber;
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
                    }
                }
            }
        }

        private bool WriteDictionaryChapters(ref int playOrder, XmlWriter ncx, XmlNamespaceManager namespaceManager, StringBuilder sb, string name, XmlNode node, ref string textString)
        {
            sb.Append(name);
            sb.Append("#");
            XmlNode val = null;
            if (node.Attributes != null && node.Attributes["id"] != null)
            {
                val = node.Attributes["id"];
            }
            if (val != null)
                sb.Append(val.Value);

            // for a dictionary, the headword / headword-minor is the label
            if (!node.HasChildNodes)
            {
                // reset the stringbuilder
                sb.Length = 0;
                // This entry doesn't have any information - skip it
                return false;
            }
            string headwordXPath = ".//xhtml:span[@class='mainheadword']";
            XmlNode headwordNode = node.SelectSingleNode(headwordXPath, namespaceManager);

			if (headwordNode == null)
			{
				headwordXPath = ".//xhtml:span[@class='headword']/*[not(ancestor::span[@class='referencedentries']) and not(ancestor::span[@class='configtargets']) and not(ancestor::span[@class='subentries'])]";
				headwordNode = node.SelectSingleNode(headwordXPath, namespaceManager);

				if (headwordNode == null)
				{
					headwordXPath = ".//xhtml:span[@class='headword']";
					headwordNode = node.SelectSingleNode(headwordXPath, namespaceManager);
				}
			}

			if (headwordNode != null)
				textString = headwordNode.InnerText;
			else
				textString = node.FirstChild.InnerText;

	        if (textString.Trim().Length > 0)
            {
                // write out the node
                ncx.WriteStartElement("navPoint");
                ncx.WriteAttributeString("id", "dtb:uid");
                ncx.WriteAttributeString("playOrder", playOrder.ToString(CultureInfo.InvariantCulture));
                ncx.WriteStartElement("navLabel");
                ncx.WriteElementString("text", textString);
                ncx.WriteEndElement(); // navlabel
                ncx.WriteStartElement("content");
                ncx.WriteAttributeString("src", sb.ToString());
                ncx.WriteEndElement(); // meta
                playOrder++;
            }

            // If this is a dictionary with TOC level 3, gather the senses for this entry
            if (InputType.ToLower().Equals("dictionary") && TocLevel.StartsWith("3"))
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

                        textString = string.Empty;
                        XmlNodeList childNodes1 = childNode.SelectNodes(".//xhtml:span[starts-with(@class,'definition')]", namespaceManager);
                        if (childNodes1 == null) continue;
                        foreach (XmlNode childNodeb in childNodes1)
                        {
                            textString = textString.Trim() + " " + childNodeb.InnerText;
                            string[] textLen = textString.Trim().Split(' ');
                            if (textLen.Length > 3)
                            {
                                textString = textLen[0] + " " + textLen[1] + " " + textLen[2] + " ...";
                                break;
                            }
                        }
                        sb.Append(name);
                        sb.Append("#");
                        if (childNode.Attributes != null && childNode.Attributes["id"] != null)
                        {
                            sb.Append(childNode.Attributes["id"].Value);
                        }
                        if (textString.Trim().Length > 0)
                        {
                            // write out the node
                            ncx.WriteStartElement("navPoint");
                            ncx.WriteAttributeString("id", "dtb:uid");
                            ncx.WriteAttributeString("playOrder", playOrder.ToString(CultureInfo.InvariantCulture));
                            ncx.WriteStartElement("navLabel");
                            ncx.WriteElementString("text", textString.Trim());
                            ncx.WriteEndElement(); // navlabel
                            ncx.WriteStartElement("content");
                            ncx.WriteAttributeString("src", sb.ToString());
                            ncx.WriteEndElement(); // meta
                            ncx.WriteEndElement(); // navPoint
                        }
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
            return true;
        }

        private void WriteEndNoteLinks(string xhtmlFileName, ref int playOrder, XmlWriter ncx)
        {
            XmlDocument xmlDocument = Common.DeclareXMLDocument(true);
            var namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            var xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, ProhibitDtd = false }; //Common.DeclareXmlReaderSettings(false);
            XmlReader xmlReader = XmlReader.Create(xhtmlFileName, xmlReaderSettings);
            xmlDocument.Load(xmlReader);
            xmlReader.Close();
            bool isanchor = false, isBookName = false, isNoteTargetReference = false, isList = false;
            string anchorValue = string.Empty, bookNameValue = string.Empty;
            XmlNodeList nodes = xmlDocument.SelectNodes("//xhtml:li", namespaceManager);

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
                                            ncx.WriteAttributeString("playOrder", playOrder.ToString(CultureInfo.InvariantCulture));
                                            ncx.WriteStartElement("navLabel");
                                            ncx.WriteElementString("text", textString);
                                            ncx.WriteEndElement(); // navlabel
                                            ncx.WriteStartElement("content");
                                            ncx.WriteAttributeString("src", sb.ToString());
                                            ncx.WriteEndElement(); // meta
                                            playOrder++;
                                            sb.Length = 0;
                                        }

                                        if (textString.Trim().Length > 4)
                                        {
                                            ncx.WriteEndElement(); // navPoint
                                        }
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
                            }
                        }

                    }
                    // reset the stringbuilder
                    sb.Length = 0;
                }
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
            var namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            var xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, ProhibitDtd = false }; //Common.DeclareXmlReaderSettings(false);
            var xmlReader = XmlReader.Create(xhtmlFileName, xmlReaderSettings);
            xmlDocument.Load(xmlReader);
            xmlReader.Close();
            // should only be one of these after splitting out the chapters.
            XmlNodeList nodes;
            if (InputType.ToLower().Equals("dictionary"))
            {
                nodes = xmlDocument.SelectNodes("//xhtml:div[@class='letter']", namespaceManager);
	            if (nodes == null || nodes.Count == 0)
	            {
		            nodes = xmlDocument.SelectNodes("//div[@class='letter']", namespaceManager);
	            }
	            if (nodes == null || nodes.Count == 0)
	            {
		            nodes = xmlDocument.SelectNodes("//xhtml:span[@class='letter']", namespaceManager);
	            }
	            if (nodes == null || nodes.Count == 0)
	            {
		            nodes = xmlDocument.SelectNodes("//span[@class='letter']", namespaceManager);
	            }
            }
            else
            {
                nodes = xmlDocument.SelectNodes("//xhtml:span[@class='scrBookName']", namespaceManager);
                if (nodes == null || nodes.Count == 0)
                {
                    nodes = xmlDocument.SelectNodes("//span[@class='scrBookName']", namespaceManager);
                }
                if (nodes == null || nodes.Count == 0)
                {
                    // nothing there - check on the Title_Main span
                    nodes = xmlDocument.SelectNodes("//xhtml:div[@class='Title_Main']", namespaceManager);
                    // nothing there - check on the scrBookName span
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

    }
}
