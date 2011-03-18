using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Windows.Forms;
using SIL.PublishingSolution;

namespace SIL.Tool
{
    public interface IPreExportProcess
    {
        string ProcessedXhtml { get; }
        string ProcessedCss { get; }
        string GetCreatedTempFolderPath { get; }
        /// <summary>
        /// To swap the headword and reversal-form when main.xhtml and FlexRev.xhtml included
        /// </summary>
        void SwapHeadWordAndReversalForm();

        /// <summary>
        /// This function is created for the task TD-872(Kabwa Scripture formatting in XP). Flex exporting css as "REVERSE_SOLIDUS"
        /// when "\" comes with the classname. So this function replace the classname which contains "\" into "REVERSE_SOLIDUS" in the XHTML files
        /// like class="\Citation_Line1" to class="REVERSE_SOLIDUSCitation_line1"
        /// </summary>
        void ReplaceSlashToREVERSE_SOLIDUS();

        /// <summary>
        /// FileOpen used for Preprocessing temp File
        /// </summary>
        string ImagePreprocess();

        void GetTempFolderPath();

        /// <summary>
        /// To replace the symbol string if the symbol matches with the text
        /// </summary>
        void ReplaceStringInCss(string cssFile);

        void SetDropCapInCSS(string cssFileName);
    }

    public class PreExportProcess : IPreExportProcess
    {
        private string _xhtmlFileNameWithPath;
        private string _cssFileNameWithPath;
        private string _baseXhtmlFileNameWithPath;
        private PublicationInformation _projInfo;
        private StringBuilder _fileContent = new StringBuilder();
        bool isNodeFound = false;
        XmlNode returnNode = null;

        public PreExportProcess()
        {

        }
        public PreExportProcess(PublicationInformation projInfo)
        {
            _xhtmlFileNameWithPath = projInfo.DefaultXhtmlFileWithPath;
            _baseXhtmlFileNameWithPath = projInfo.DefaultXhtmlFileWithPath;
            _cssFileNameWithPath = projInfo.DefaultCssFileWithPath;
            _projInfo = projInfo;
        }
        public string ProcessedXhtml
        {
            get { return _xhtmlFileNameWithPath; }
        }
        public string ProcessedCss
        {
            get { return _cssFileNameWithPath; }
        }

        string tempFolder = Common.PathCombine(Path.GetTempPath(), "Preprocess12");
        public string GetCreatedTempFolderPath
        {
            get { return tempFolder; }
        }

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
            string searchText = "class=\"\\\\";
            string replaceText = "class=\"REVERSESOLIDUS";
            Common.StreamReplaceInFile(_xhtmlFileNameWithPath, searchText, replaceText);

            searchText = "class=\"([\\\\]*\\w+)[*]";
            replaceText = "class = \"$1ASTERISK";
            Common.ReplaceInFile(_xhtmlFileNameWithPath, searchText, replaceText);
        }

        /// <summary>
        /// FileOpen used for Preprocessing temp File
        /// </summary>
        public string ImagePreprocess()
        {
            //if (string.IsNullOrEmpty(sourceFile) || !File.Exists(sourceFile)) return string.Empty;

            //Temp folder and file copy
            string sourcePicturePath = Path.GetDirectoryName(_baseXhtmlFileNameWithPath);
            //GetTempFolderPath();
            string tempFile = ProcessedXhtml;

            string metaname = Common.GetBaseValue(tempFile);
            if (metaname.Length == 0)
            {
                metaname = Common.GetMetaValue(tempFile);
            }

            // Removal of html tag namespace and other formats.
            Common.ReplaceInFile(tempFile, @"<html\b[^>]*>", "<html>");
            if (!File.Exists(tempFile)) return string.Empty;
            var xmldoc = new XmlDocument();
            // xml image copy
            try
            {
                xmldoc = new XmlDocument { XmlResolver = null, PreserveWhitespace = true };
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
                                string toFileName = Common.PathCombine(tempFolder, counter + ext);
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
            ParagraphVerserSetUp(xmldoc); // TODO - Seperate it from this method.
            xmldoc.Save(tempFile);
            }
            catch
            {
            } 
            return tempFile;
        }

        public void GetTempFolderPath()
        {
            if (Directory.Exists(tempFolder))
            {
                try
                {
                    Directory.Delete(tempFolder, true);
                }
                catch
                {
                    tempFolder = Common.PathCombine(Path.GetTempPath(), "SilPathWay" + Path.GetFileNameWithoutExtension(Path.GetTempFileName()));
                }
            }
            Directory.CreateDirectory(tempFolder);

            //Note - copies the xhtml and css files to temp folder
            string tempFile = Common.PathCombine(tempFolder, Path.GetFileName(_xhtmlFileNameWithPath));
            File.Copy(Common.DirectoryPathReplace(_xhtmlFileNameWithPath), tempFile, true);
            _xhtmlFileNameWithPath = tempFile;

            tempFile = Common.PathCombine(tempFolder, Path.GetFileName(_cssFileNameWithPath));
            if (File.Exists(_cssFileNameWithPath))
                File.Copy(Common.DirectoryPathReplace(_cssFileNameWithPath), tempFile, true);
            _cssFileNameWithPath = tempFile;
            // add a timestamp to the .css for troubleshooting purposes
            AddProductVersionToCSS();

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
            //string pattern = "_AllComplexFormEntryBackRefs|LexEntryRef_PrimaryLexemes";
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
            _xhtmlFileNameWithPath = OutputFile;
            return _xhtmlFileNameWithPath;
        }

        /// <summary>
        /// AnchorTage Processing for <a> Tag to point the href</a>
        /// </summary>
        /// <param name="sourceFile">The Xhtml File</param>
        /// <param name="anchor">Arraylist value</param>
        public void AnchorTagProcessing(string sourceFile, ref ArrayList anchor)
        {
            try
            {
                const string tag = "a";
                var xDoc = new XmlDocument { XmlResolver = null };
                xDoc.Load(sourceFile);
                XmlNodeList nodeList = xDoc.GetElementsByTagName(tag);
                if (nodeList.Count > 0)
                {
                    //FileOpen(sourceFile);
                    nodeList = xDoc.GetElementsByTagName(tag);
                    string fileContent = xDoc.OuterXml.ToLower();
                    if (nodeList.Count > 0)
                    {
                        foreach (XmlNode item in nodeList)
                        {
                            var name = item.Attributes.GetNamedItem("href");
                            if (name != null)
                            {
                                if (name.Value.IndexOf('#') >= 0)
                                {
                                    var href = name.Value.Replace("#", "");
                                    if (href.Length > 0)
                                    {
                                        href = href.ToLower();
                                        string hrefQuot = "\"" + href + "\"";
                                        if (fileContent.IndexOf(hrefQuot) < 0)
                                        {
                                            name.Value = "";
                                        }
                                        else
                                        {
                                            anchor.Add(href.ToLower());
                                        }
                                    }
                                }
                            }
                        }
                    }
                    xDoc.Save(sourceFile);
                }
            }
            catch
            {
            }
        }

        private string OpenFile()
        {
            //string tempFolder = Common.PathCombine(Path.GetTempPath(), "IDPreprocess");
            //if (Directory.Exists(tempFolder))
            //{
            //    Directory.Delete(tempFolder, true);
            //}
            //Directory.CreateDirectory(tempFolder);

            //string OutputFile = Common.PathCombine(tempFolder, Path.GetFileName(_xhtmlFileNameWithPath));
            ////string OutputFile = _xhtmlFileNameWithPath;
            //File.Copy(Common.DirectoryPathReplace(_xhtmlFileNameWithPath), OutputFile, true);

            string OutputFile = _xhtmlFileNameWithPath;
            var reader = new StreamReader(OutputFile, Encoding.UTF8);
            _fileContent.Remove(0, _fileContent.Length);
            _fileContent.Append(reader.ReadToEnd());
            reader.Close();
            return OutputFile;
        }

        /// <summary>
        /// Modify <body> tag as <body xml:space="preserve">
        /// </summary>
        public string PreserveSpace()
        {
            FileStream fs = new FileStream(_xhtmlFileNameWithPath, FileMode.Open);
            StreamReader stream = new StreamReader(fs);

            string fileDir = Path.GetDirectoryName(_xhtmlFileNameWithPath);
            string fileName = "Preserve" + Path.GetFileName(_xhtmlFileNameWithPath);
            string Newfile = Path.Combine(fileDir, fileName);

            var fs2 = new FileStream(Newfile, FileMode.Create, FileAccess.Write);
            var sw2 = new StreamWriter(fs2);
            string line;

            bool replace = true;
            while ((line = stream.ReadLine()) != null)
            {
                if (replace)
                {
                    if (line.IndexOf("<body") >= 0)
                    {
                        //line = line.Replace(">", @" xml:space=""preserve"">");
                        line = line.Replace("<body", @" <body xml:space=""preserve""  ");
                        replace = false;
                    }
                }
                sw2.WriteLine(line);
            }
            sw2.Close();
            fs.Close();
            fs2.Close();

            _xhtmlFileNameWithPath = Newfile;
            return _xhtmlFileNameWithPath;
        }

        public string InsertHiddenChapterNumber()
        {
            var xDoc = new XmlDocument { XmlResolver = null };
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

        public string InsertHiddenVerseNumber()
        {
            var xDoc = new XmlDocument { XmlResolver = null };
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

        public string GetDefinitionLanguage()
        {
            var xDoc = new XmlDocument { XmlResolver = null };
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
                var xDoc = new XmlDocument { XmlResolver = null };
                xDoc.Load(_xhtmlFileNameWithPath);
                XmlNodeList nodeList = xDoc.GetElementsByTagName("figure");
                if (nodeList.Count > 0)
                {
                    //object keyValue;
                    //var gotKey = RegistryHelperLite.RegEntryExists(RegistryHelperLite.ParatextKey, "Settings_Directory", "", out keyValue);
                    //string paratextDir = "";
                    //if (gotKey)
                    //    paratextDir = (string) keyValue;
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
                        //<div class="pictureRight" id="a2"  >
                        //   <img id ="b2" src="figures\WA03904b.tif" alt="alternative"/>
                        //   <div class="caption">
                        //     Mbitebí ámʋ fɛ́ɛ́ bɔkʋsʋ́ nywɛ́ amʋ́ nkandɩ́ɛ. (Mateo 25:7)
                        //   </div>
                        // </div>
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
                            if (chapterNode.Attributes == null) return;
                            var chapter = chapterNode.Attributes.GetNamedItem("class");
                            if (chapter != null && chapter.Value.ToLower() == "chapter_number")
                            {
                                XmlNode verseNode = chapterNode.NextSibling;
                                if (verseNode == null) continue;
                                var verse = verseNode.Attributes.GetNamedItem("class");
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

            string titleTag = "title";
            XmlNodeList titleNodeList = xmldoc.GetElementsByTagName(titleTag);
            if (titleNodeList.Count > 0)
            {
                XmlNode item = titleNodeList[0];
                if (item.Value == null)
                {
                    XmlNode parent = item.ParentNode;
                    XmlNode title = xmldoc.CreateNode("element", "title", null);
                    title.InnerText = Path.GetFileNameWithoutExtension(xmldoc.BaseURI);
                    parent.AppendChild(title);
                    item.ParentNode.RemoveChild(item);
                }
            }
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

        public void SetDropCapInCSS(string cssFileName)
        {
            TextWriter tw = new StreamWriter(cssFileName, true);
            tw.WriteLine(".Chapter_Number {");
            tw.WriteLine("font-size: 199%;");
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

        /// <summary>
        /// Appends a timestamp to the .css file for field troubleshooting
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

        #endregion
    }
}
