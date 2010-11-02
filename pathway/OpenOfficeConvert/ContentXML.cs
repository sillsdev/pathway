// --------------------------------------------------------------------------------------------
// <copyright file="ContentXML.cs" from='2009' to='2009' company='SIL International'>
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
// Creates the Contentxml in ODT Export
// </remarks>
// --------------------------------------------------------------------------------------------

#region Using
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Collections;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;
using SIL.PublishingSolution;
using SIL.Tool;
using SIL.Tool.Localization;

#endregion Using
namespace SIL.PublishingSolution
{
    public class ContentXML
    {
        #region Private Variable

        XmlTextWriter _writer;
        XmlTextReader _reader;
        readonly Stack _styleStack = new Stack();
        readonly Stack _allSpanStack = new Stack();
        readonly Stack _allDivStack = new Stack();
        readonly Stack _usedSpanStack = new Stack();
        readonly Stack _tagTypeStack = new Stack(); // P - Para, T - Text, I - Image, L - <ol>/<ul> tag , S - <li> tag,  O - Others
        readonly Dictionary<string, string> _makeAttribute = new Dictionary<string, string>();
        //Dictionary<string, string> dictColumnGap = new Dictionary<string, string>();

        bool isHiddenText = false;
        bool _isDisplayNone = false;
        readonly Utility _util = new Utility();
        readonly StyleAttribute _attributeInfo = new StyleAttribute();
        readonly StyleAttribute _fontsizeAttributeInfo = new StyleAttribute();
        string _familyType = string.Empty;
        string _styleName = string.Empty;
        int _sectionCount;
        // Image
        int _frameCount; //For Picture Frame count
        string _beforePseudoValue = string.Empty;
        string _beforePseudoParentValue = string.Empty;
        //readonly StringBuilder _imageBuilder = new StringBuilder();
        readonly Stack _prevLangStack = new Stack(); // For handle previous Language
        readonly StringBuilder _pseudoBuilder = new StringBuilder();
        readonly StringBuilder _pseudoBuilderCurrentNode = new StringBuilder();
        bool _contentReplace;
        readonly StringBuilder _classContentBuilder = new StringBuilder();
        int _styleCounter;
        string _metaValue = string.Empty;   // TD-278 <meta name />
        ArrayList _odtFiles;
        //ArrayList _odtEndFiles;

        string _fileType = string.Empty;
        int _autoFootNoteCount;
        private string _sourcePicturePath;

        // from local
        Styles _structStyles;
        //readonly Library _lib = new Library();
        int _pbCounter;
        string _readerValue = string.Empty;
        string _footnoteValue = string.Empty;
        string _lang = string.Empty;
        string _classAfter = string.Empty;
        char _closeTagType;
        string _parentStyleName;
        readonly Dictionary<string, string> _existingStyleName = new Dictionary<string, string>();

        string _styleFilePath;
        float _columnWidth;
        string _temp;
        string _tagType;
        string _divClass = string.Empty;
        string _allDiv = string.Empty;
        string _class = string.Empty;
        bool _divOpen;
        bool _isWhiteSpace;
        bool _isNewLine = true;
        string _prevLang = string.Empty;
        string _parentClass = string.Empty;
        string _parentLang = string.Empty;
        string _listName = string.Empty;
        string _projectType = string.Empty;
        bool _isDropCap;
        private bool _forcedPara;
        private string _hapterNumber;
        private string _verseNumber;
        private string _footCal;
        private string _footNoteStyleName;
        private string _formatFootnote;

        readonly Dictionary<string, string> _counterVolantryReset = new Dictionary<string, string>();
        readonly string _tempFile = Common.PathCombine(Path.GetTempPath(), "tempXHTMLFile.xhtml"); //TD-351 
        readonly string _hardSpace = Common.ConvertUnicodeToString("\u00A0");

        readonly ArrayList _anchor = new ArrayList();
        string _anchorIdValue = string.Empty;
        bool _anchorStart;
        private XmlDocument _xmldoc;
        private bool _imageStart;
        private string _imageParent;
        bool _imageDiv;
        private bool progressBarError;
        private string _imageGraphicsName = string.Empty;
        private bool _imageTextAvailable;
        private ArrayList _imageCaptionEmpty = new ArrayList();
        private int _imageZindexCounter;
        private bool _imagePreviousFinished = false;

        private bool _isStyleExistInCss = false;
        readonly List<string> _isStyleExist = new List<string>();
        private XmlNodeType _currentNodeType;
        private XmlNodeType _previousNodeType;

        private List<string> _unUsedParagraphStyle = new List<string>();
		private bool _significant = false;
        #endregion

        #region Public Methods

        public bool IsNewLine
        {
            get { return _isNewLine; }
        }

        public string AllDiv
        {
            get { return _allDiv; }
        }

        public static long CountLinesInString(string text)
        {
            var reg = new Regex("</", RegexOptions.Multiline);
            MatchCollection mat = reg.Matches(text);
            return mat.Count;
        }

        public void InitializeObject(Styles styleInfo, string fileType)
        {
            _structStyles = styleInfo;
            _fileType = fileType;
            _odtFiles = _structStyles.MasterDocument;
        }

        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Generate Content.xml body from .xhtml
        /// <list> 
        /// </list>
        /// </summary>
        /// <param name="projInfo">Project Information</param>
        /// <param name="structStyles">"style name" collection, which has relative values</param>
        /// <returns> </returns>
        /// -------------------------------------------------------------------------------------------
        //public void CreateContent(ProgressBar pb, string Sourcefile, string targetPath, Styles structStyles, string fileType)
        public void CreateContent(PublicationInformation projInfo, Styles structStyles)
        {
            //TODO: When the Input has been preprocessed, GetProjectType returns the wrong type!
            //_projectType = Common.GetProjectType(projInfo.DefaultXhtmlFileWithPath);
            _projectType = projInfo.ProjectInputType;
            if (_projectType == "Scripture")
            {
                structStyles.IsMacroEnable = true;
            }
            InitializeObject(structStyles, projInfo.OutputExtension); // Creates new Objects
            CreateFile(projInfo.TempOutputFolder);
            CreateSection(structStyles.SectionName);
            projInfo.DefaultXhtmlFileWithPath = PreProcess(projInfo.DefaultXhtmlFileWithPath, structStyles);
            Common.SetProgressBarValue(projInfo.ProgressBar, projInfo.DefaultXhtmlFileWithPath);
            CreateBody();
            Console.WriteLine(Common.OdType.ToString());
            if (Common.OdType != Common.OdtType.OdtMaster)
                ProcessXHTML(projInfo.ProgressBar, projInfo.DefaultXhtmlFileWithPath, projInfo.TempOutputFolder);

            CloseFile(projInfo.TempOutputFolder, structStyles.ColumnGapEm);
            AlterFrameWithoutCaption(projInfo.TempOutputFolder);
            CleanUp();
        }

        private void AlterFrameWithoutCaption(string contentFilePath)
        {
            Utility utility = new Utility();
            utility.GraphicContentChange(contentFilePath, _imageCaptionEmpty);
        }

        /// <summary>
        /// Cleanup Process
        /// </summary>
        public void CleanUp()
        {
            if (File.Exists(_tempFile))
            {
                File.Delete(_tempFile);
            }

            _util.RemoveUnUsedStyle(_styleFilePath, _unUsedParagraphStyle);
        }

        /// <summary>
        /// To change the class name when the classname have contains selector
        /// </summary>
        /// <param name="sourceFile">Source XHTML file</param>
        /// <param name="structStyles">Styles Structure</param>
        /// <returns>return Source filename </returns>  
        private string PreProcess(string sourceFile, Styles structStyles)
        {
            _sourcePicturePath = Path.GetDirectoryName(sourceFile);
            //string newSourceFile = sourceFile;
            ClassContainsText(sourceFile, structStyles);
            AnchorTagProcessing(sourceFile);
            sourceFile = CheckSourceChanged(sourceFile);
            ReplaceString(sourceFile, structStyles);
            return sourceFile;
        }

        /// <summary>
        /// To replace the symbol string if the symbol matches with the text
        /// </summary>
        /// <param name="sourceFile">XHTML file path</param>
        /// <param name="structStyles">The Structure of Styles objects</param>
        private void ReplaceString(string sourceFile, Styles structStyles)
        {
            if (structStyles.ReplaceSymbolToText.Count > 0)
            {
                var sr = new StreamReader(sourceFile);
                string ss = sr.ReadToEnd();
                sr.Close();
                foreach (string srchKey in structStyles.ReplaceSymbolToText.Keys)
                {
                    if (ss.IndexOf(srchKey) >= 0)
                    {
                        ss = ss.Replace(srchKey, structStyles.ReplaceSymbolToText[srchKey]);
                    }
                }
                var sw = new StreamWriter(sourceFile);
                sw.Write(ss);
                sw.Close();
            }
        }
        /// <summary>
        /// Used for preprocess work to point the tempfile copied from source xhtml file.
        /// </summary>
        /// <param name="sourceFile">xhtml file</param>
        /// <returns>Changed file name</returns>
        private string CheckSourceChanged(string sourceFile)
        {
            if (_xmldoc != null)
            {
                sourceFile = _tempFile;
            }
            return sourceFile;
        }

        /// <summary>
        /// AnchorTage Processing for <a> Tag to point the href</a>
        /// </summary>
        /// <param name="sourceFile">The Xhtml File</param>
        private void AnchorTagProcessing(string sourceFile)
        {
            const string tag = "a";
            var xDoc = new XmlDocument { XmlResolver = null };
            xDoc.Load(sourceFile);
            XmlNodeList nodeList = xDoc.GetElementsByTagName(tag);
            if (nodeList.Count > 0)
            {
                FileOpen(sourceFile);
                nodeList = _xmldoc.GetElementsByTagName(tag);
                string fileContent = _xmldoc.OuterXml.ToLower();
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
                                        _anchor.Add(href.ToLower());
                                    }
                                }
                            }
                        }
                    }
                }
                _xmldoc.Save(_tempFile);
            }
        }

        /// <summary>
        /// Class Contains method modifies the style Name of the container
        /// </summary>
        /// <param name="sourceFile">The Xhtml File</param>
        /// <param name="structStyles">The Structure of Styles objects</param>
        private void ClassContainsText(string sourceFile, Styles structStyles)
        {
            if (structStyles.ClassContainsSelector.Count > 0)
            {
                FileOpen(sourceFile);

                //<h2 class = "section">Sample</h2>

                foreach (string classKey in structStyles.ClassContainsSelector.Keys)
                {
                    XmlNodeList nodeList;
                    string mValue = structStyles.ClassContainsSelector[classKey];
                    string mAttrib = Common.LeftString(classKey, "-");
                    if (classKey.IndexOf('.') > 0)
                    {
                        int lastPos = classKey.LastIndexOf('.') + 1;
                        string mClass = classKey.Substring(lastPos);
                        nodeList = _xmldoc.GetElementsByTagName(mClass);
                        ChangeClassName(mAttrib, mValue, nodeList);
                    }
                    else
                    {
                        string[] tags = { "div", "span" };
                        for (int i = 0; i < tags.Length; i++)
                        {
                            nodeList = _xmldoc.GetElementsByTagName(tags[i]);
                            if (nodeList.Count > 0)
                            {
                                ChangeClassName(mAttrib, mValue, nodeList);
                            }
                        }
                    }
                }
                _xmldoc.Save(_tempFile);
            }
        }

        /// <summary>
        /// FileOpen used for Preprocessing temp File
        /// </summary>
        /// <param name="sourceFile">The input Xhtml File</param>
        private void FileOpen(string sourceFile)
        {
            if (_xmldoc == null)
            {
                File.Copy(sourceFile, _tempFile, true);
                _xmldoc = new XmlDocument { XmlResolver = null };
                _xmldoc.Load(_tempFile);
            }
        }


        /// <summary>
        /// To change the class name if the value matches with mValue.
        /// </summary>
        /// <param name="mAttrib">Classname to match</param>
        /// <param name="mValue">Value to match</param>
        /// <param name="nodeList">List of matched XMLNode</param>
        private static void ChangeClassName(string mAttrib, string mValue, XmlNodeList nodeList)
        {
            foreach (XmlNode item in nodeList)
            {
                if (item.InnerText.Contains(mValue))
                {

                    XmlAttributeCollection attrColl = item.Attributes;
                    for (int i = 0; i < attrColl.Count; i++)
                    {
                        if (string.Compare(attrColl.Item(i).Value, mAttrib) == 0
                            && string.Compare(attrColl.Item(i).Name, "class") == 0)
                        {
                            attrColl.RemoveNamedItem(mAttrib); // To remove the old class attribute
                            var newElement = (XmlElement)item; // To add New class attribute
                            newElement.SetAttribute("class", mAttrib + mValue.Replace(" ", ""));
                            break;
                        }
                    }
                }
            }
        }

        private void ProcessXHTML(ProgressBar pb, string Sourcefile, string targetPath)
        {
            if (pb != null && pb.Maximum == 0)
            {
                progressBarError = true;
            }

            _styleFilePath = targetPath + "styles.xml";
            _columnWidth = _structStyles.ColumnWidth;
            //DateTime startTime = DateTime.Now;
            try
            {
                _reader = new XmlTextReader(Sourcefile)
                {
                    XmlResolver = null,
                    WhitespaceHandling = WhitespaceHandling.Significant
                    //WhitespaceHandling = WhitespaceHandling.None
                };
                //CreateBody();

                while (_reader.Read())
                {
                    if (_reader.IsEmptyElement)
                    {
                        if (_reader.Name != "img")
                        {
                            if (_metaValue.Length <= 0)
                            {
                                CheckMetaRootDirectory();
                            }
                            AllowEmptyTag();
                            continue;
                        }
                    }
                    _currentNodeType = _reader.NodeType;
                    switch (_currentNodeType)
                    {

                        case XmlNodeType.Element:
                            StartElement(targetPath);
                            break;
                        case XmlNodeType.EndElement:

                            EndElement(pb);

                            break;
                        case XmlNodeType.Text: // Text.Write
                            WriteText(_footnoteValue);
                            break;
                        case XmlNodeType.SignificantWhitespace:
                            string data = SignificantSpace(_reader.Value);
                            _writer.WriteString(data);
                            break;
                    }
                    _previousNodeType = _currentNodeType;
                    if (_contentReplace) // TD-204(unable to put tol/pisin)
                    {
                        WriteText(_footnoteValue);
                    }
                }
                //TimeSpan totalTime = DateTime.Now - startTime;
                //System.Windows.Forms.MessageBox.Show(totalTime.ToString());
            }
            catch (XmlException e)
            {
                var msg = new[] { e.Message, Sourcefile };
                LocDB.Message("errProcessXHTML", Sourcefile + " is Not Valid. " + "\n" + e.Message, msg, LocDB.MessageTypes.Info, LocDB.MessageDefault.First);
                CloseFile(targetPath, _structStyles.ColumnGapEm);
            }
            finally
            {
                if (pb != null)
                {
                    pb.Value = pb.Maximum;
                    pb.Visible = false;
                }
            }
        }

        /// <summary>
        /// Allow Empty Tag if the class name is given in CSS to apply
        /// </summary>
        private void AllowEmptyTag()
        {
            string tempClassName = string.Empty;
            if (_reader.AttributeCount > 0)
            {
                tempClassName = _reader.GetAttribute("class");
            }
            if (string.IsNullOrEmpty(tempClassName))
            {
                tempClassName = _reader.Name;
            }

            if (_structStyles.AllCSSName.Contains(tempClassName))
            {
                //string paraSpan = _reader.Name == "div" ? "text:p" : "text:span";
                // Currently Empty Div Tag is allowed. If Span is allowed remove the comment
                if (_reader.Name == "div")
                {
                    const string paraSpan = "text:p";
                    _writer.WriteStartElement(paraSpan);
                    _writer.WriteAttributeString("text:style-name", tempClassName);
                    _writer.WriteEndElement();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetPath"></param>
        private void StartElement(string targetPath)
        {
            bool IsStyleExist = false;

            if (_contentReplace)
            {
                _styleCounter++;
            }
            _tagType = _reader.Name;
            _readerValue = _reader.GetAttribute("class");

            if (_readerValue == null) // Is class name null
            {
                _readerValue = _tagType;
            }
            else
            {
                string NewStyleName = _readerValue + "." + _tagType;
                IsStyleExist = _structStyles.AllCSSName.Contains(NewStyleName);
                if (IsStyleExist)
                {
                    _readerValue = _readerValue + "." + _tagType;
                }
            }

            string anchorIdValue = _reader.GetAttribute("id");
            if (anchorIdValue != null)
            {
                anchorIdValue = anchorIdValue.ToLower();
                if (_anchor.Contains(anchorIdValue))
                {
                    _anchorIdValue = anchorIdValue;
                    _anchor.Remove(anchorIdValue);
                }
            }

            anchorIdValue = _reader.GetAttribute("name");
            if (anchorIdValue != null)
            {
                anchorIdValue = anchorIdValue.ToLower();
                if (_anchor.Contains(anchorIdValue))
                {
                    _anchorIdValue = anchorIdValue;
                    _anchor.Remove(anchorIdValue);
                }
            }



            if (_tagType == "p" ||
                _tagType == "h1" ||
                _tagType == "h2" ||
                _tagType == "h3" ||
                _tagType == "h4" ||
                _tagType == "h5" ||
                _tagType == "h6")
            {
                if (!IsStyleExist)
                {
                    _readerValue = _tagType;
                }

                //if (_readerValue == null)
                //{
                //    _readerValue = _tagType;
                //}
                //else
                //{
                //    string NewStyleName = _readerValue + "." + _tagType;
                //    bool IsStyleExist = _util.IsStyleExist(_styleFilePath, NewStyleName);
                //    if (IsStyleExist)
                //    {
                //        _readerValue = _readerValue + "." + _tagType;
                //    }
                //    else
                //    {
                //        _readerValue = _tagType;
                //    }
                //}
                _tagType = "div";
            }
            else if (_tagType == "li")
            {
                if (_divOpen)
                {
                    _writer.WriteEndElement();
                    _divOpen = false;
                }

                if (_listName.Length != 0)
                {
                    _writer.WriteStartElement("text:list");
                    _writer.WriteAttributeString("text:style-name", _listName);
                    _listName = string.Empty;
                }

                _writer.WriteStartElement("text:list-item");
                //if (_readerValue == null)
                //{
                //    _readerValue = _tagType;
                //}
                //else
                //{
                //    _readerValue = _readerValue + "." + _tagType;
                //}
                if (!IsStyleExist)
                {
                    _readerValue = _tagType;
                }
                _familyType = "paragraph";
            }
            else if (_tagType == "ol" ||
                _tagType == "ul")
            {

                //if (_divOpen)
                //{
                //    _writer.WriteEndElement();
                //    _divOpen = false;
                //}
                //NewLine();

                //if (_readerValue == null)
                //{
                //    _readerValue = _tagType;
                //}
                //else
                //{
                //    _readerValue = _readerValue + "." + _tagType;
                //}

                // Apply <ol> / <ul> styles
                _listName = _structStyles.ListType.ContainsKey(_readerValue) ? _structStyles.ListType[_readerValue] : _tagType;
            }
            else if (_tagType == "em")
            {
                //if (_readerValue == null)
                //{
                //    _readerValue = _tagType;
                //}
                //else
                //{
                //    _readerValue = _readerValue + "." + _tagType;
                //}
                _tagType = "span";
            }
            else if (_tagType == "a")
            {
                _tagType = "span";
                string hrefValue = _reader.GetAttribute("href");
                if (!string.IsNullOrEmpty(hrefValue) && hrefValue.StartsWith("#"))
                {
                    string hrefValueWOHash = hrefValue.Replace("#", "");
                    _anchor.Add(hrefValueWOHash.ToLower());
                    _anchorStart = true;
                }
            }

            _readerValue = _readerValue.Replace("_", ""); // replacing underscore. In parser Underscore is removed
            _readerValue = _readerValue.Replace("-", ""); // replacing Hyphen. In parser Underscore is removed
            _footnoteValue = _readerValue;


            string previousClass = string.Empty;  // = _classAfter
            if (_classAfter.IndexOf('_') >= 0)
            {
                previousClass = _classAfter.Substring(0, _classAfter.IndexOf('_'));
                if (previousClass.IndexOf(' ') >= 0)
                {
                    previousClass = previousClass.Substring(0, previousClass.IndexOf(' '));
                }
            }

            // sense + sense - display : block;
            if (_structStyles.DisplayBlock.Contains(previousClass + "_" + _readerValue))
            {
                _tagType = "div";
            }
            // sense + sense - display : inline;
            else if (_structStyles.DisplayInline.Contains(previousClass + "_" + _readerValue))
            {
                _tagType = "span";
            }
            //sense - display : block;
            else if (_structStyles.DisplayBlock.Contains(_readerValue))
            {
                _tagType = "div";
            }
            //sense - display : inline;
            else if (_structStyles.DisplayInline.Contains(_readerValue))
            {
                _tagType = "span";
            }
            //if (structStyles.FloatAlign.ContainsKey(m_readerValue))
            //{
            //    if (structStyles.FloatAlign[m_readerValue].ToString() != "none")
            //    {
            //        _pos = structStyles.FloatAlign[m_readerValue];
            //        string _temp1 = styleStack.Peek().ToString();
            //        string[] _arrreadervalue = _temp1.Split('_');
            //        if (structStyles.ClearProperty.ContainsKey(m_arrreadervalue[0]))
            //        {
            //            _side = structStyles.ClearProperty[_arrreadervalue[0]];
            //            return;
            //        }
            //        else
            //        {
            //            _side = "NoClear";
            //        }
            //    }
            //}

            if (_tagType == "div")
            {
                _writer.Formatting = Formatting.Indented;
                if (_divOpen)
                {
                    _writer.WriteEndElement();
                    _divOpen = false;
                    if (_structStyles.SectionName.Contains(_classAfter))
                    {
                        _writer.WriteEndElement();
                    }
                }
                if (_reader.Name == "p")
                {
                    //NewLine();
                }

                _styleName = _readerValue;
                BeforeDivSpan(_structStyles.SectionName, _readerValue);
                _familyType = "paragraph";
            }
            else if (_tagType == "span")
            {
                _writer.Formatting = Formatting.None;
                _styleName = _readerValue;
                _familyType = "text";
            }
            else if (_reader.Name == "img")
            {
                _readerValue = _reader.GetAttribute("src").ToLower();
                InsertImageCaption(_styleFilePath, targetPath);
                return;
            }
            else if (_reader.Name == "ol" || _reader.Name == "ul" || _reader.Name == "li")
            {
                _styleName = _readerValue;
                _familyType = "paragraph";
            }
            else
            {
                if (_reader.Name == "title") // skip the node
                {
                    _reader.Read();
                }
                else if (_reader.Name == "style")
                {
                    _reader.Read();
                }
                _tagTypeStack.Push('O'); // Others html head body
                return;
            }

            _class = _readerValue;
            if (_structStyles.FootNoteCall.ContainsKey(_class))
            {
                if (_structStyles.FootNoteCall[_class].IndexOf('(') >= 0)
                {
                    string attrName = _structStyles.FootNoteCall[_class].Substring(
                        _structStyles.FootNoteCall[_class].IndexOf('(') + 1,
                        _structStyles.FootNoteCall[_class].Length -
                        _structStyles.FootNoteCall[_class].IndexOf('(') - 2);
                    _footCal = _reader.GetAttribute(attrName);
                    if (_footCal == "")
                        _footCal = " ";
                }
                else
                {
                    _footCal = _structStyles.FootNoteCall[_class];
                }
            }
            //string previousClass = string.Empty;  // = _classAfter
            //if (_classAfter.IndexOf('_') >= 0)
            //{
            //    previousClass = _classAfter.Substring(0, _classAfter.IndexOf('_'));
            //    if (previousClass.IndexOf(' ') >= 0)
            //    {
            //        previousClass = previousClass.Substring(0, previousClass.IndexOf(' '));
            //    }
            //}

            if (_structStyles.WhiteSpace.Contains(_class))
            {
                _isWhiteSpace = true;
            }
            if (_structStyles.DropCap.Contains(_class))  // Matches the drop cap class
            {
                _isDropCap = true;
            }
            if (_structStyles.DisplayNone.Contains(_class))
            {
                _isDisplayNone = true;
            }
            string spaceSplitClass = _class;
            if (_class.IndexOf(' ') >= 0)
            {
                spaceSplitClass = _class.Substring(0, _class.IndexOf(' '));
            }

            if (_structStyles.VisibilityClassName.ContainsKey(spaceSplitClass))
            {
                isHiddenText = true;
            }

            if (_structStyles.ContentCounterReset.ContainsKey(spaceSplitClass))
            {
                string key = _structStyles.ContentCounterReset[spaceSplitClass];
                _structStyles.ContentCounter[key] = 0;
            }

            if (_counterVolantryReset.ContainsKey(spaceSplitClass))
            {
                string key = _counterVolantryReset[spaceSplitClass];
                _structStyles.ContentCounter[key] = 0;
            }
            if (_styleStack.Count > 0)
            {
                _parentClass = _styleStack.Peek().ToString();
                if (_prevLangStack.Count > 0)
                {
                    if (_prevLangStack.Peek() != null)
                        _parentLang = _prevLangStack.Peek().ToString();
                }
                if (_parentClass.IndexOf('_') >= 0)
                {
                    _parentClass = _parentClass.Substring(0, _parentClass.IndexOf('_'));
                }
            }
            if (_structStyles.CounterParent.ContainsKey(spaceSplitClass))
            {
                string key = "";
                string keyValue = "";
                foreach (KeyValuePair<string, string> kvp in _structStyles.CounterParent[spaceSplitClass])
                {
                    key = kvp.Key;
                    keyValue = kvp.Value;
                }
                _structStyles.ContentCounter[key] = _structStyles.ContentCounter[key] + int.Parse(keyValue);
            }
            if (_structStyles.TagAttrib.ContainsKey(_readerValue))
            {
                string attrib = _reader.GetAttribute(_structStyles.TagAttrib[_readerValue]);
                if (attrib != null)
                {
                    _readerValue = _structStyles.TagAttrib[_readerValue] + attrib + "_." + _readerValue;
                }
            }

            _lang = _reader.GetAttribute("lang");
            if (_lang != null)
            {
                _readerValue = _readerValue + "_." + _lang;
            }
            if (_structStyles.AllCSSName.Contains(_readerValue))
            {
                _isStyleExistInCss = true;
            }
            _prevLangStack.Push(_lang);
            bool hasPseudoWritten = false;
            if (_structStyles.PseudoClass.Contains(previousClass + "+" + spaceSplitClass))
            {
                _beforePseudoParentValue = spaceSplitClass;
                hasPseudoWritten = InsertPseudoParentBefore(_beforePseudoParentValue, _classAfter, _lang, _class, _structStyles, _prevLang);
            }
            if ((_structStyles.PseudoClassBefore.ContainsKey(spaceSplitClass) || _structStyles.PseudoClassBefore.ContainsKey(_readerValue))
                && hasPseudoWritten == false)
            {
                _beforePseudoValue = spaceSplitClass; //Pseudo
                InsertPseudoBefore(_beforePseudoValue, _lang, _classAfter, _class, _structStyles);
            }
            if (_contentReplace == false)
            {
                InsertClassContent(spaceSplitClass, _lang, previousClass, _prevLang, _parentClass, _parentLang, _structStyles);
            }
            _makeAttribute.Clear();

            if (_styleStack.Count > 0)
            {
                // parentStyleName = classAfter + readerValue;
                _parentStyleName = _styleStack.Peek() + _readerValue;
            }
            else
            {
                _parentStyleName = "root_" + _readerValue;
            }

            if (!_existingStyleName.ContainsKey(_parentStyleName))
            {
                bool isRelative = false;
                if (_structStyles.CssClassName.ContainsKey(_readerValue)) // Relative Values
                {
                    isRelative = RelativeValue(_styleFilePath, _styleStack, _structStyles, _readerValue);
                }
                else if (_structStyles.CssClassName.ContainsKey(_class)) // Relative Values
                {
                    isRelative = RelativeValue(_styleFilePath, _styleStack, _structStyles, _class);
                }

                if (_readerValue.IndexOf(' ') >= 0)
                {
                    _readerValue = _readerValue.Substring(0, _readerValue.IndexOf(' '));
                }

                if (_structStyles.AttribAncestor.ContainsKey(spaceSplitClass))
                {
                    _readerValue = GetAncestorNode(_structStyles.AttribAncestor, spaceSplitClass, _class);
                }

                if (isRelative == false)
                {
                    _readerValue = _util.GetNewChildName(_styleFilePath, _styleStack, _readerValue, false);
                    _styleName = _util.ChildName;
                    if (_util.MissingLang)
                    {
                        string language, country;
                        var makeAttribute = new Dictionary<string, string>();
                        //var lib = new Library();
                        Common.GetCountryCode(out language, out country, _lang, _structStyles.SpellCheck);
                        //if (language == null)
                        //{
                        //    makeAttribute["fo:language"] = "zxx";
                        //    makeAttribute["fo:country"] = "none";
                        //}
                        //else
                        //{
                        //    makeAttribute["fo:language"] = language;
                        //    makeAttribute["fo:country"] = country;
                        //}
                        makeAttribute["fo:language"] = language;
                        makeAttribute["fo:country"] = country;
                        string sourceClass = Common.LeftString(_readerValue, "_.");
                        _util.CreateStyleWithNewValue(_styleFilePath, sourceClass, _util.ChildName, makeAttribute, _util.ParentName, _familyType, _structStyles.BackgroundColor);
                        _util.MissingLang = false;
                        _isStyleExistInCss = true;
                    }
                    else if (_util.ParentName != string.Empty)
                    {
                        _util.CreateStyle(_styleFilePath, _readerValue, _util.ChildName, _util.ParentName, _familyType, _structStyles.BackgroundColor, false);
                    }
                    //_unUsedParagraphStyle.Add(_util.ChildName);
                }
                //if (FootCal != null && FootNoteStyleName == null)
                _existingStyleName.Add(_parentStyleName, _util.ChildName);
            }
            else
            {
                _util.ChildName = _existingStyleName[_parentStyleName];
            }

            //Note : After Ancestor and Parent pseudo got the name , checking it again 
            if (_structStyles.AllCSSName.Contains(Common.LeftString(_util.ChildName,Common.SepParent)))
            {
                _isStyleExistInCss = true;
            }

            if (_isStyleExistInCss)
            {
                _isStyleExist.Add(_util.ChildName);
                _isStyleExistInCss = false;
            }

            if (_footCal != null)
                if (_footCal.Length > 0 && _footNoteStyleName == null)
                {
                    _footNoteStyleName = _util.ChildName;
                }
            _styleStack.Push(_util.ChildName);

            if (_tagType == "ol" ||
                _tagType == "ul")
            {
                _tagTypeStack.Push('L'); // ol, ul
            }
            else if (_reader.Name == "li")
            {
                _tagTypeStack.Push('S');
            }

            if (_familyType == "paragraph")
            {
                _divClass = _util.ChildName;
                _allDivStack.Push(_util.ChildName);
                _tagTypeStack.Push('P'); // paragraph
            }
            else if (_familyType == "text")
            {
                _allSpanStack.Push(_util.ChildName);
                _tagTypeStack.Push('T'); // fullString;
            }
            else
            {
                _tagTypeStack.Push('O'); // Others
            }
        }

        /// <summary>
        /// To insert the Footnote symbol and the Text
        /// </summary>
        /// <param name="footCallSymb">FootNote call symbol</param>
        /// <param name="clsName">Class name for the FootNote</param>
        /// <param name="text">Text of FootNote</param>
        /// <param name="marker">Marker Format symbol added to boottom</param>
        private void InsertFootCall(string footCallSymb, string clsName, string text, string marker)
        {
            if (_structStyles.ContentCounterReset.ContainsKey(clsName))
                _autoFootNoteCount = 0;
            _autoFootNoteCount++;
            _writer.WriteStartElement("text:note");
            _writer.WriteAttributeString("text:id", "ftn" + (_autoFootNoteCount));
            _writer.WriteAttributeString("text:note-class", "footnote");
            _writer.WriteStartElement("text:note-citation");
            _writer.WriteAttributeString("text:label", footCallSymb);
            _writer.WriteString(footCallSymb);
            _writer.WriteEndElement();
            _writer.WriteStartElement("text:note-body");
            _writer.WriteStartElement("text:p");
            _writer.WriteAttributeString("text:style-name", clsName);
            if (marker != string.Empty)
            {
                _writer.WriteStartElement("text:span");
                _writer.WriteAttributeString("text:style-name", "Footnote Characters");
                _writer.WriteString(marker);
                _writer.WriteEndElement();
            }
            _writer.WriteString(text);
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }


        private void EndElement(ProgressBar pb)
        {
            // if anchor does not have test to write , close the anchor tag.
            if (_anchorStart && _reader.Name == "a")
            {
                _anchorStart = false;
            }
            if (_styleStack.Count > 0 && (_styleStack.Peek().ToString() == _footNoteStyleName))
            {
                string currClass = _footNoteStyleName.Substring(0, _footNoteStyleName.IndexOf('_'));
                string FormatMarker = string.Empty;
                if (_structStyles.FootNoteMarker.ContainsKey(currClass)
                    && (_structStyles.FootNoteMarker[currClass].IndexOf("#ChapterNumber") >= 0 ||
                    _structStyles.FootNoteMarker[currClass].IndexOf("#VerseNumber") >= 0))
                {
                    FormatMarker = _structStyles.FootNoteMarker[currClass].Replace("#ChapterNumber", _hapterNumber).Replace("#VerseNumber", _verseNumber) + " ";
                }
                InsertFootCall(_footCal, currClass, _formatFootnote, FormatMarker);
                _footCal = null;
                _formatFootnote = null;
                _footNoteStyleName = null;
            }


            _closeTagType = (char)_tagTypeStack.Pop();

            if (_contentReplace)
            {
                _styleCounter--;
                if (_styleCounter == 0)
                {
                    _contentReplace = false;
                }
            }
            _footnoteValue = string.Empty;
            _pbCounter++;

            if (progressBarError)
            {
                pb.Maximum = _pbCounter;
            }

            //pb.Value = _pbCounter++; // used to set the value for Progress Bar.
            if (pb != null)
                pb.Value = _pbCounter; // used to set the value for Progress Bar.
            if (_prevLangStack.Count > 0)
            {
                if (_prevLangStack.Peek() != null)
                {
                    _prevLang = _prevLangStack.Pop().ToString();
                }
                else
                {
                    _prevLangStack.Pop();
                }
            }
            //tagType = (char)tagTypeStack.Pop();
            if (_closeTagType == 'P')
            {
                if (_styleStack.Count > 0)
                {
                    _classAfter = _styleStack.Pop().ToString();
                    _allDiv = _allDivStack.Pop().ToString();
                    InsertPseudoAfter(_classAfter, _structStyles.PseudoClassAfter, _structStyles.PseudoWithoutStyles);
                }
                if (_divOpen) // Avoid consecutive </fullString:p>
                {
                    _writer.WriteEndElement();
                    _divOpen = false;
                }
                string[] currentArray = _classAfter.Split('_');
                string currentClass = string.Empty;
                if (currentArray.Length > 0)
                {
                    currentClass = currentArray[0];
                }
                if (_structStyles.SectionName.Contains(currentClass))
                {
                    _writer.WriteEndElement();
                }
                if ((char)_tagTypeStack.Peek() == 'S') // <li>
                {
                    _tagTypeStack.Pop();
                    _writer.WriteEndElement();
                }
                else if ((char)_tagTypeStack.Peek() == 'L') // Is <ol> or <ul>
                {
                    _tagTypeStack.Pop();
                    _writer.WriteEndElement();
                    //NewLine();
                }
                else if (_reader.Name == "p") // <p>
                {
                    //NewLine();
                }

            }
            else if (_closeTagType == 'T') // Handling nested span
            {
                _classAfter = _styleStack.Pop().ToString();
                InsertPseudoAfter(_classAfter, _structStyles.PseudoClassAfter, _structStyles.PseudoWithoutStyles);
                _temp = _allSpanStack.Pop().ToString();
                _familyType = "";
                if (_usedSpanStack.Count > 0)
                {
                    if (_temp == _usedSpanStack.Peek().ToString())
                    {
                        _temp = _usedSpanStack.Pop().ToString();
                        _writer.WriteEndElement();
                      //  _writer.WriteString(" "); //TD-1513
                    }
                }
            }
            //else if (_tagType == 'L') // <ol> & <ul>
            //{
            //    _writer.WriteEndElement();
            //    _classAfter = _styleStack.Pop().ToString();
            //    NewLine();
            //}
            //string stylePeek = string.Empty;
            //if (_styleStack.Count > 0)
            //{
            //    stylePeek = _styleStack.Peek().ToString();
            //}

            ImageEndElement();

        }

        private void ImageEndElement()
        {
            if (_imageStart)
            {
                if (_imageParent == _classAfter)
                {
                    if (_forcedPara)
                    {
                        //_writer.WriteRaw("</text:p>");
                        _writer.WriteEndElement();
                        //_forcedPara = false;
                    }
                    _writer.WriteEndElement();
                    _writer.WriteEndElement();
                    //_writer.WriteRaw("</draw:text-box></draw:frame>");
                    if (!_imageTextAvailable)
                        _imageCaptionEmpty.Add(_imageGraphicsName);
                    _imageStart = false;
                    _imagePreviousFinished = true;
                    _imageDiv = false;
                    _forcedPara = false;
                    _divOpen = true;  // make it true to continuous writing of, coming paragraph also if 

                }
            }

        }

        private void WriteText(string currClass)
        {

            if (_imagePreviousFinished)
                _imagePreviousFinished = false;

            if (_divOpen == false)
            {
                _divClass = _allDivStack.Count > 0 ? _allDivStack.Peek().ToString() : "none";
                if (_divClass.IndexOf('_') < 0 && _divClass.IndexOf(' ') >= 0)
                {
                    _divClass = _divClass.Substring(0, _divClass.IndexOf(' '));
                }

                if (_isDropCap) // forcing new paragraph for drop caps
                {
                    string currentParentStyle = _allDivStack.Peek().ToString();
                    _writer.WriteStartElement("text:p");
                    int noOfChar = _reader.Value.Length;
                    string currentStyle = currClass + noOfChar;
                    _util.CreateDropCapStyle(_styleFilePath, currClass, currentStyle, currentParentStyle, noOfChar);
                    _writer.WriteAttributeString("text:style-name", currentStyle);
                }
                else
                {
                    _writer.WriteStartElement("text:p");
                    _writer.WriteAttributeString("text:style-name", _divClass);
                }
                if (_imageStart)
                {
                    _imageDiv = true;
                    _util.CreateStyleHyphenate(_styleFilePath, _divClass);
                }

                _divOpen = true;
                _divClass = string.Empty;
            }
            else
            {
                if (_isDropCap) // until the next paragraph
                {
                    _isDropCap = false;
                }
            }
            if (_imageStart)
            {
                _imageTextAvailable = true;
            }

            if (!_isDisplayNone && _pseudoBuilder.Length > 0) // Pseudo writer
            {
                _writer.WriteRaw(_pseudoBuilder.ToString());
                _pseudoBuilder.Remove(0, _pseudoBuilder.Length);
            }
            else
            {
                _isDisplayNone = false;
            }

            if (_familyType == "text")
            {
                if (_imageStart && !_forcedPara && !_imageDiv) // images
                {
                    _writer.WriteStartElement("text:p");
                    _writer.WriteAttributeString("text:style-name", "ForcedDiv");
                    _util.CreateStyleHyphenate(_styleFilePath, "ForcedDiv");
                    _forcedPara = true;
                }
                _usedSpanStack.Push(_util.ChildName);
                MissingStyleInCss(_util.ChildName);
                //_unUsedParagraphStyle.Remove(_util.ChildName); // NOte remove all predicate check
                //_unUsedParagraphStyle.Add(_util.ChildName);
            }
            else if (_usedSpanStack.Count == 0)
            {
                if (_allSpanStack.Count > 0)
                {
                    _styleName = _allSpanStack.Peek().ToString();
                    if (_styleName.IndexOf('_') < 0 && _styleName.IndexOf(' ') >= 0)
                    {
                        _styleName = _styleName.Substring(0, _styleName.IndexOf(' '));
                    }
                    _usedSpanStack.Push(_styleName);
                    //_writer.WriteStartElement("text:span");

                    //_writer.WriteStartElement("text:span");
                    //if (_isStyleExist.Contains(_styleName))
                    //{
                    //    _writer.WriteAttributeString("text:style-name", _styleName);
                    //    _isStyleExist.Remove(_styleName);
                    //}
                   MissingStyleInCss(_styleName);
                }
            }

            if (isHiddenText)
            {
                int noOfChar = _reader.Value.Trim().Replace("\r\n", "").Length;
                noOfChar = noOfChar + (noOfChar * 20 / 100);
                _writer.WriteStartElement("text:s");
                _writer.WriteAttributeString("text:c", noOfChar.ToString());
                _writer.WriteEndElement();
                isHiddenText = false;
                goto last;
            }

            if (_anchorIdValue.Length > 0)
            {
                _writer.WriteStartElement("text:reference-mark");
                _writer.WriteAttributeString("text:name", _anchorIdValue);
                _writer.WriteEndElement();
                _anchorIdValue = string.Empty;
            }

            if (_contentReplace)
            {
                if (_classContentBuilder.Length > 0)
                {
                    _writer.WriteRaw(_classContentBuilder.ToString());
                    _classContentBuilder.Remove(0, _classContentBuilder.Length);
                }
            }
            else if (_isWhiteSpace)
            {
                WhiteSpace(_reader.Value, _divClass);
                _isWhiteSpace = false;
            }
            else
            {
                //if (!_isDisplayNone && _pseudoBuilder.Length > 0)
                //{
                //    _writer.WriteRaw(_pseudoBuilder.ToString());
                //    _pseudoBuilder.Remove(0, _pseudoBuilder.Length);
                //}
                //else
                //{
                //    _isDisplayNone = false;
                //}

                if (!_isDisplayNone &&  _pseudoBuilderCurrentNode.Length > 0)
                {
                    _writer.WriteRaw(_pseudoBuilderCurrentNode.ToString());
                    _pseudoBuilderCurrentNode.Remove(0, _pseudoBuilderCurrentNode.Length);
                }

                if (!string.IsNullOrEmpty(_footCal))
                {
                    _formatFootnote += _reader.Value;
                }
                else if ((_structStyles.DisplayFootNote.Count > 0 && _structStyles.DisplayFootNote.Contains(currClass)))
                {
                    _autoFootNoteCount++;
                    _writer.WriteStartElement("text:note");
                    _writer.WriteAttributeString("text:id", "ftn" + (_autoFootNoteCount));
                    _writer.WriteAttributeString("text:note-class", currClass);
                    _writer.WriteStartElement("text:note-citation");
                    _writer.WriteString(_autoFootNoteCount.ToString());
                    _writer.WriteEndElement();
                    _writer.WriteStartElement("text:note-body");
                    _writer.WriteStartElement("text:p");
                    _writer.WriteAttributeString("text:style-name", currClass);
                    _writer.WriteString(". " + _reader.Value);
                    _writer.WriteEndElement();
                    _writer.WriteEndElement();
                    _writer.WriteEndElement();
                }
                else if (_anchorStart)
                {
                    if (_anchor.Count > 0)
                    {
                        string data = HardSpace(currClass, _reader.Value);
                        string hrefValueWOHash = _anchor[_anchor.Count - 1].ToString();
                        _writer.WriteStartElement("text:reference-ref");
                        _writer.WriteAttributeString("text:reference-format", "text");
                        _writer.WriteAttributeString("text:ref-name", hrefValueWOHash);
                        _writer.WriteString(data);
                        _writer.WriteEndElement(); // for Anchor Ends
                        _anchorStart = false;
                    }
                }
                else
                {
                    //if (string.Compare(currClass, "ChapterNumber") == 0)
                    //{
                    //    ChapterNumber = _reader.Value;
                    //}
                    //else if (string.Compare(currClass, "VerseNumber") == 0)
                    //{
                    //    VerseNumber = _reader.Value;
                    //}
                    
                    string data = HardSpace(currClass, _reader.Value);
                    data = SignificantSpace(data);
                    bool isWritten = InsertHardLineBreak(data);
                    if (!isWritten)
                        _writer.WriteString(data);
                }
                if (string.Compare(currClass, "ChapterNumber") == 0)
                {
                    _hapterNumber = _reader.Value;
                }
                else if (string.Compare(currClass, "VerseNumber") == 0)
                {
                    _verseNumber = _reader.Value;
                }
            }
        last:
            if (_familyType == "text")
            {
                _unUsedParagraphStyle.Add(_util.ChildName);
            }
            _familyType = "";
            _isNewLine = false;

        }

        private bool InsertHardLineBreak(string content)
        {
            bool isWritten = false;

            if (content.IndexOf((char)8232) >= 0)
            {
                string[] value = content.Split(new []{(char)8232 });
                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i].Length != 0)
                    {
                        _writer.WriteRaw("<text:line-break/>");
                        _writer.WriteString(value[i]);
                    }
                }
                isWritten = true;
            }
            return isWritten;
        }

        private void MissingStyleInCss(string styleName)
        {

            _writer.WriteStartElement("text:span");
            if (_isStyleExist.Contains(styleName))
            {
                _writer.WriteAttributeString("text:style-name", styleName);
                _isStyleExist.Remove(styleName);
            }
            else
            {
                string divClass = _allDivStack.Peek().ToString();
                if (divClass.IndexOf("TitleMain_") > -1)
                {
                    _writer.WriteAttributeString("text:style-name", styleName);
                }
            }

            ////bool isStyle = _isStyleExist.Peek();
            //_writer.WriteStartElement("text:span");
            //if (_isStyleExistInCss)
            //{
            //    _writer.WriteAttributeString("text:style-name", styleName);
            //    //_isStyleExist.Add(styleName);
            //    //_isStyleExistInCss = false;
            //}
        }

        #endregion

        #region Private Methods

        private bool InsertPseudoParentBefore(string classN, string classAfter, string lang,
            string oldClass, Styles dictContents, string prevLang)
        {
            string writingContent = "\u0b84"; // Changed to Unicode. Because string empty also allowed to write in Content : Normal,None.
            string ancestorClass = string.Empty;
            int first = 0;
            int second = 0;
            int third = 0;

            string currentClass = classN.IndexOf('_') >= 0 ? classN.Substring(0, classN.IndexOf('_')) : classN;

            if (currentClass.IndexOf(' ') >= 0)
            {
                currentClass = currentClass.Substring(0, currentClass.IndexOf(' '));
            }
            string parentClass = classAfter.IndexOf('_') >= 0 ? classAfter.Substring(0, classAfter.IndexOf('_')) : classN;

            if (classAfter.IndexOf(' ') >= 0)
            {
                parentClass = classAfter.Substring(0, classAfter.IndexOf(' '));
            }
            if (classAfter.IndexOf('_') >= 0)
            {
                string[] split = classAfter.Split('_');
                //string getClass = classAfter.Substring(classAfter.IndexOf("_") + 1, (classAfter.Length - classAfter.IndexOf("_")) - 1);
                ancestorClass = split[1];
                if (split[1].IndexOf('.') >= 0)
                {
                    //getClass = classAfter.Substring(classAfter.IndexOf("_.") + 2, (classAfter.Length - classAfter.IndexOf("_")) - 2);
                    //getClass = getClass.Substring(getClass.IndexOf("_") + 1, (getClass.Length - getClass.IndexOf("_")) - 1);
                    ancestorClass = split[2];
                }
                //styleName = classN + "_" + getClass;
            }
            if (dictContents.PseudoPosition.Contains(oldClass + "_." + lang))
            {
                first = dictContents.PseudoPosition.IndexOf(oldClass + "_." + lang);
            }

            if (dictContents.PseudoPosition.Contains(currentClass + "_." + lang + "_" + parentClass))
            {
                second = dictContents.PseudoPosition.IndexOf(currentClass + "_." + lang + "_" + parentClass);
            }

            if (dictContents.PseudoPosition.Contains(currentClass + "_" + parentClass + "_." + prevLang))
            {
                third = dictContents.PseudoPosition.IndexOf(currentClass + "_" + parentClass + "_." + prevLang);
            }

            if (dictContents.PseudoAttrib.ContainsKey(currentClass + "_." + lang + "_" + parentClass + "_." + prevLang))
            {
                writingContent = dictContents.PseudoAttrib[currentClass + "_." + lang + "_" + parentClass + "_." + prevLang];
                currentClass = currentClass + "_." + lang + "_" + parentClass + "_." + prevLang;
            }
            else if (dictContents.PseudoAncestorBefore.ContainsKey(currentClass + "_." + lang + "_" + parentClass + "." + ancestorClass))
            {
                writingContent = dictContents.PseudoAncestorBefore[currentClass + "_." + lang + "_" + parentClass + "." + ancestorClass];
                currentClass = currentClass + "_." + lang + "_" + parentClass + "." + ancestorClass;
            }
            else if (dictContents.PseudoAncestorBefore.ContainsKey(currentClass + "_" + parentClass + "." + ancestorClass))
            {
                writingContent = dictContents.PseudoAncestorBefore[currentClass + "_" + parentClass + "." + ancestorClass];
                currentClass = currentClass + "_" + parentClass + "." + ancestorClass;
            }
            //else if (dictContents.PseudoClassBefore.ContainsKey(m_OldClass + "_." + lang))
            //{
            //    writingContent = dictContents.PseudoClassBefore[m_OldClass + "_." + lang];
            //    currentClass = OldClass + "_." + lang;
            //    beforePseudoValue = string.Empty;
            //}
            //else if (dictContents.PseudoAttrib.ContainsKey(currentClass + "_." + lang + "_" + parentClass))
            //{
            //    writingContent = dictContents.PseudoAttrib[currentClass + "_." + lang + "_" + parentClass];
            //    currentClass = currentClass + "_." + lang + "_" + parentClass;
            //}
            else if (dictContents.PseudoClassBefore.ContainsKey(oldClass + "_." + lang))
            {
                if (first >= second)
                {
                    writingContent = dictContents.PseudoClassBefore[oldClass + "_." + lang];
                    currentClass = oldClass + "_." + lang;
                    _beforePseudoValue = string.Empty;
                }
                else if (dictContents.PseudoAttrib.ContainsKey(currentClass + "_." + lang + "_" + parentClass))
                {
                    writingContent = dictContents.PseudoAttrib[currentClass + "_." + lang + "_" + parentClass];
                    currentClass = currentClass + "_." + lang + "_" + parentClass;
                }
            }
            else if (dictContents.PseudoAttrib.ContainsKey(currentClass + "_." + lang + "_" + parentClass) || dictContents.PseudoAttrib.ContainsKey(currentClass + "_" + parentClass + "_." + prevLang))
            {
                if (third > second)
                {
                    writingContent = dictContents.PseudoAttrib[currentClass + "_" + parentClass + "_." + prevLang];
                    currentClass = currentClass + "_" + parentClass + "_." + prevLang;
                }
                else if (second >= first)
                {
                    writingContent = dictContents.PseudoAttrib[currentClass + "_." + lang + "_" + parentClass];
                    currentClass = currentClass + "_." + lang + "_" + parentClass;
                }
                else if (dictContents.PseudoClassBefore.ContainsKey(oldClass + "_." + lang))
                {
                    writingContent = dictContents.PseudoClassBefore[oldClass + "_." + lang];
                    currentClass = oldClass + "_." + lang;
                    _beforePseudoValue = string.Empty;
                }
            }
            //else if (dictContents.PseudoClassBefore.ContainsKey(oldClass))
            //{
            //    writingContent = dictContents.PseudoClassBefore[oldClass];
            //    currentClass = oldClass;
            //    _beforePseudoValue = string.Empty;
            //}
            else if (dictContents.PseudoClassBefore.ContainsKey(currentClass + "_." + lang))
            {
                writingContent = dictContents.PseudoClassBefore[currentClass + "_." + lang];
                currentClass = currentClass + "_." + lang;
                _beforePseudoValue = string.Empty;
            }
            else if (dictContents.PseudoAttrib.ContainsKey(currentClass + "_." + lang))
            {
                writingContent = dictContents.PseudoAttrib[currentClass + "_." + lang];
                currentClass = currentClass + "_." + lang;
                _beforePseudoValue = string.Empty;
            }
            else if (dictContents.PseudoAttrib.ContainsKey(currentClass + "_" + parentClass))
            {
                writingContent = dictContents.PseudoAttrib[currentClass + "_" + parentClass];
                currentClass = currentClass + "_" + parentClass;
            }
            else if (dictContents.PseudoClassBefore.ContainsKey(currentClass))
            {
                writingContent = dictContents.PseudoClassBefore[currentClass];
                _beforePseudoValue = string.Empty;
            }
            _beforePseudoParentValue = string.Empty;

            if (writingContent != "\u0b84")
            {

                if (dictContents.PseudoClassBefore.ContainsKey(classN))
                {
                    string ConcatContent = string.Empty;
                    string x = dictContents.PseudoClassBefore[classN];

                    if (x.IndexOf("counter") >= 0)
                    {
                        string[] y = x.Split('\u0b83');
                        foreach (string var in y)
                        {
                            if (var.IndexOf("counter") >= 0)
                            {
                                int srtPos = var.IndexOf('(');
                                int endPos = var.IndexOf(')');
                                string value = var.Substring(srtPos + 1, endPos - srtPos - 1);
                                if (dictContents.ContentCounter.ContainsKey(value))
                                {
                                    ConcatContent = ConcatContent + dictContents.ContentCounter[value];
                                }
                                else
                                {
                                    ConcatContent = ConcatContent + "0";
                                }
                            }
                            else
                            {
                                ConcatContent = ConcatContent + var.Replace('\'', ' ');
                            }
                        }
                        writingContent = ConcatContent;
                    }
                }
                //if (writingContent != "") // No need to write empty spaces
                //{
                //    string styleName = currentClass + "-" + "before";
                //    // The stylename without styles will be ignored for Pseudo.
                //    if (dictContents.PseudoWithoutStyles.Contains(styleName))
                //    {
                //        _pseudoBuilder.Append(writingContent);
                //    }
                //    else
                //    {
                //        _pseudoBuilder.Append("<text:span ");
                //        _pseudoBuilder.Append("text:style-name=\"" + styleName + "\">" + writingContent);
                //        _pseudoBuilder.Append("</text:span>");
                //    }
                //}
                if (writingContent.Length > 0) // No need to write empty spaces
                {
                    string styleName = currentClass + "-" + "before";
                    // The stylename without styles will be ignored for Pseudo.
                    if (dictContents.PseudoWithoutStyles.Contains(styleName))
                    {
                        //_pseudoBuilder.Append(writingContent);
                        _pseudoBuilderCurrentNode.Append(writingContent);
                    }
                    else
                    {
                        //_pseudoBuilder.Append("<text:span ");
                        //_pseudoBuilder.Append("text:style-name=\"" + _styleName + "\">" + writingContent);
                        //_pseudoBuilder.Append("</text:span>");

                        _pseudoBuilderCurrentNode.Append("<text:span ");
                        _pseudoBuilderCurrentNode.Append("text:style-name=\"" + styleName + "\">" + writingContent);
                        _pseudoBuilderCurrentNode.Append("</text:span>");
                    }
                }
                return true;
            }
            return false;
        }

        /// --------------------------------------------------------------------------------------------------
        /// <summary>
        /// To insert the replace the fullString with content for rules
        /// like .definition > .xitem + .yitem {content: "Test"}
        /// </summary>
        /// <param name="classN"></param>
        /// <param name="lang">Current language</param>
        /// <param name="prevClass">Previous classname like xitem</param>
        /// <param name="prevLang">Previous language</param>
        /// <param name="parent">Parent classname</param>
        /// <param name="parentLang">parent language like definition</param>
        /// <param name="dictContents">List of dictionary used</param>
        /// <returns></returns>
        /// ---------------------------------------------------------------------------------------------------
        private void InsertClassContent(string classN, string lang, string prevClass, string prevLang, string parent, string parentLang, Styles dictContents)
        {
            string contentValue = string.Empty;
            string currentClass = classN.IndexOf('_') >= 0 ? classN.Substring(0, classN.IndexOf('_')) : classN;
            if (currentClass.IndexOf(' ') >= 0)
            {
                currentClass = currentClass.Substring(0, currentClass.IndexOf(' '));
            }
            currentClass = currentClass + "_." + lang;
            string precedesClass = prevClass + "_." + prevLang;
            string parentClass = parent + "_." + parentLang;
            if (dictContents.ClassContent.ContainsKey(currentClass + "." + parentClass + "_" + precedesClass))
            {
                contentValue = dictContents.ClassContent[currentClass + "." + parentClass + "_" + precedesClass];
            }
            else if (dictContents.ClassContent.ContainsKey(classN + "." + parentClass + "_" + precedesClass))
            {
                contentValue = dictContents.ClassContent[classN + "." + parentClass + "_" + precedesClass];
            }
            else if (dictContents.ClassContent.ContainsKey(classN + "." + parent + "_" + prevClass))
            {
                contentValue = dictContents.ClassContent[classN + "." + parent + "_" + prevClass];
            }
            else if (dictContents.ClassContent.ContainsKey(currentClass + "_" + precedesClass + "." + parentClass))
            {
                contentValue = dictContents.ClassContent[currentClass + "_" + precedesClass + "." + parentClass];
            }
            else if (dictContents.ClassContent.ContainsKey(classN + "_" + prevClass + "." + parent))
            {
                contentValue = dictContents.ClassContent[classN + "_" + prevClass + "." + parent];
            }
            // .XITEM + .XITEM
            else if (dictContents.ClassContent.ContainsKey(currentClass + "_" + precedesClass))
            {
                contentValue = dictContents.ClassContent[currentClass + "_" + precedesClass];
            }
            else if (dictContents.ClassContent.ContainsKey(currentClass + "_" + prevClass))
            {
                contentValue = dictContents.ClassContent[currentClass + "_" + prevClass];
            }
            else if (dictContents.ClassContent.ContainsKey(currentClass + "_" + prevClass))
            {
                contentValue = dictContents.ClassContent[currentClass + "_" + prevClass];
            }
            else if (dictContents.ClassContent.ContainsKey(classN + "_" + prevClass))
            {
                contentValue = dictContents.ClassContent[classN + "_" + prevClass];
            }
            else if (dictContents.ClassContent.ContainsKey(parentClass + "_" + precedesClass + "." + currentClass))
            {
                contentValue = dictContents.ClassContent[parentClass + "_" + precedesClass + "." + currentClass];
            }
            else if (dictContents.ClassContent.ContainsKey(currentClass))
            {
                contentValue = dictContents.ClassContent[currentClass];
            }
            else if (dictContents.ClassContent.ContainsKey(classN))
            {
                contentValue = dictContents.ClassContent[classN];
            }
            //if (isDIVOpen)
            //{
            //    _writer.WriteEndElement();
            //}
            if (contentValue != string.Empty)
            {
                if (_pseudoBuilder.Length > 0)
                {
                    _classContentBuilder.Append(_pseudoBuilder.ToString());
                    _pseudoBuilder.Remove(0, _pseudoBuilder.Length);
                }
                _classContentBuilder.Append(contentValue);
                _contentReplace = true;
                _styleCounter++;
            }
        }

        private void InsertPseudoAfter(string classAfter, IDictionary<string, string> dictPseudoAfter, List<string> pseudoWithOutStyle)
        {
            string writingContent = string.Empty;
            string firstName = classAfter;
            if (classAfter.IndexOf('_') >= 0)
            {
                firstName = classAfter.Substring(0, classAfter.IndexOf('_'));
            }
            if (firstName.IndexOf(' ') >= 0)
            {
                firstName = firstName.Substring(0, firstName.IndexOf(' '));
            }

            if (dictPseudoAfter.ContainsKey(firstName))
            {
                writingContent = dictPseudoAfter[firstName];
            }

            if (writingContent != "")
            {
                _styleName = firstName + "-" + "after";
                // The stylename without styles will be ignored for Pseudo.
                if (pseudoWithOutStyle.Contains(_styleName))
                {
                    _writer.WriteString(writingContent);
                }
                else
                {
                    _writer.WriteStartElement("text:span");
                    _writer.WriteAttributeString("text:style-name", _styleName);
                    _writer.WriteRaw(writingContent);
                    _writer.WriteEndElement();
                }
            }
        }

        /// <summary>
        /// Replace trailing "softspace" to "hardspace"
        /// </summary>
        /// <param name="className">Current ClassName</param>
        /// <param name="data">text</param>
        /// <returns></returns>
        private string HardSpace(string className, string data)
        {
            if (className == "xsensenumber" || className == "xhomographnumber" || className == "xglossnumber")
            {
                ////data = data.Replace(" ", hardSpace);
                int trimLen = data.Length;
                data = data.TrimEnd(' ');
                if (trimLen != 0)
                {
                    data = data.PadRight(trimLen, char.Parse(_hardSpace));
                }
            }
            return data;
        }

        private void WhiteSpace(string readerValue, string divClass)
        {
            Char[] charac = readerValue.ToCharArray();
            int j = 0;
            string concatString = string.Empty;

            foreach (char var in charac)
            {
                if (var == ' ')
                {
                    if (concatString != "")
                    {
                        _writer.WriteString(concatString);
                        concatString = "";
                    }
                    j++;
                }
                else if (var == '\n')
                {
                    if (j > 0)
                    {
                        _writer.WriteStartElement("text:s");
                        _writer.WriteAttributeString("text:c", j.ToString());
                        _writer.WriteEndElement();
                        j = 0;
                    }
                    _writer.WriteEndElement();
                    _writer.WriteStartElement("text:p");
                    _writer.WriteAttributeString("text:style-name", divClass);
                }
                else
                {
                    if (j > 0)
                    {
                        _writer.WriteStartElement("text:s");
                        _writer.WriteAttributeString("text:c", j.ToString());
                        _writer.WriteEndElement();
                        j = 0;
                    }
                    _writer.WriteString(var.ToString());
                }
            }
        }

        private string GetAncestorNode(IDictionary<string, ArrayList> dicAncestor, string utilClassName, string className)
        {
            if (_styleStack.Count == 0)
            {
                return className;
            }
            string readerValue = utilClassName;
            string getStyleName = utilClassName;
            int maxIndex = -1;
            int currIndex;
            if (className.IndexOf(" ") > 0)
            {
                if (dicAncestor.ContainsKey(utilClassName))
                {
                    ArrayList arrHasValue = dicAncestor[utilClassName];
                    string[] arrValue = className.Split(' ');
                    foreach (string value in arrValue)
                    {
                        currIndex = arrHasValue.IndexOf(value);
                        if (currIndex > maxIndex)
                        {
                            maxIndex = currIndex;
                            readerValue = utilClassName + " " + value;
                        }
                    }
                }
            }

            currIndex = 0;
            string ancestorClass = _styleStack.Peek().ToString();
            if (dicAncestor.ContainsKey(utilClassName))
            {
                ArrayList arrAncestor = dicAncestor[utilClassName];
                string[] arrValue = ancestorClass.Split('_');
                for (int i = 0; i < arrValue.Length; i++)
                {
                    foreach (string classname in arrAncestor)
                    {

                        if (arrValue[i].Equals(classname))
                        {
                            getStyleName = utilClassName + "." + classname;
                            break;
                        }
                        currIndex++;
                    }
                }
            }
            if (currIndex > maxIndex && maxIndex <= 0)
            {
                return getStyleName;
            }
            return currIndex < maxIndex && maxIndex >= 1 ? readerValue : utilClassName;
        }

        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Generate first block of Content.xml
        /// 
        /// <list> 
        /// </list>
        /// </summary>
        /// <param name="ClassName"></param>
        /// <param name="lang"></param>
        /// <param name="classAfter"></param>
        /// <param name="oldClass"></param>
        /// <param name="dictContents"></param>
        /// -------------------------------------------------------------------------------------------

        private void InsertPseudoBefore(string ClassName, string lang, string classAfter, string oldClass,
           Styles dictContents)
        {

            if(_pseudoBuilderCurrentNode.Length > 0)
            {
                _pseudoBuilder.Append(_pseudoBuilderCurrentNode);
                _pseudoBuilderCurrentNode.Remove(0, _pseudoBuilderCurrentNode.Length);
            }

            string writingContent = string.Empty;
            string parentClass = classAfter.IndexOf(' ') >= 0 ? classAfter.Substring(0, classAfter.IndexOf(' ')) : classAfter;
            if (parentClass.IndexOf('_') >= 0)
            {
                parentClass.Substring(0, parentClass.IndexOf('_'));
            }

            if (_beforePseudoParentValue == ClassName)
            {
                _beforePseudoParentValue = string.Empty;
            }

            if (dictContents.PseudoClassBefore.ContainsKey(oldClass + "_." + lang))  //.pronunciation[lang=en_US][class_=first-of-type]::before
            {
                writingContent = dictContents.PseudoClassBefore[oldClass + "_." + lang]; //.pronounciation first-of-type_.en_US
                ClassName = oldClass + "_." + lang;
            }
            else if (dictContents.PseudoClassBefore.ContainsKey(oldClass)) //.pronunciation[class_=first-of-type]::before
            {
                writingContent = dictContents.PseudoClassBefore[oldClass]; //pronounciation first-of-type
                ClassName = oldClass;
            }
            else if (dictContents.PseudoClassBefore.ContainsKey(ClassName + "_." + lang)) //.pronunciation[lang=en_US]::before
            {
                writingContent = dictContents.PseudoClassBefore[ClassName + "_." + lang]; //Pronunciation_.en_US
                ClassName = ClassName + "_." + lang;
            }
            else if (dictContents.PseudoClassBefore.ContainsKey(ClassName)) //.pronunciation:before
            {
                writingContent = dictContents.PseudoClassBefore[ClassName]; //Pronunciation
            }
            _beforePseudoValue = string.Empty;


            if (writingContent != "")
            {
                if (dictContents.PseudoClassBefore.ContainsKey(ClassName))
                {
                    string ConcatContent = string.Empty;
                    string x = dictContents.PseudoClassBefore[ClassName];
                    string[] y = x.Split('\u0b83');
                    if (x.IndexOf("counter") >= 0)
                    {
                        foreach (string var in y)
                        {
                            if (var.IndexOf("counter") >= 0)
                            {
                                int srtPos = var.IndexOf('(');
                                int endPos = var.IndexOf(')');
                                string var1 = var.Substring(srtPos + 1, endPos - srtPos - 1);
                                if (dictContents.ContentCounter.ContainsKey(var1))
                                {
                                    ConcatContent = ConcatContent + dictContents.ContentCounter[var1];
                                }
                                else
                                {
                                    ConcatContent = ConcatContent + "0";
                                }
                            }
                            else
                            {
                                ConcatContent = ConcatContent + var.Replace('\'', ' ');
                            }
                        }
                        writingContent = ConcatContent;
                    }
                }

                //_styleName = ClassName + "-" + "before";
                //    _pseudoBuilder.Append("<text:span ");
                //    _pseudoBuilder.Append("text:style-name=\"" + _styleName + "\">" + writingContent);
                //    _pseudoBuilder.Append("</text:span>");

                //// The stylename without styles will be ignored for Pseudo.
                //if (dictContents.PseudoWithoutStyles.Contains(_styleName))
                //{
                //    _pseudoBuilder.Append(writingContent);
                //}
                //else
                //{
                //    _pseudoBuilder.Append("<text:span ");
                //    _pseudoBuilder.Append("text:style-name=\"" + _styleName + "\">" + writingContent);
                //    _pseudoBuilder.Append("</text:span>");
                //}
                if (writingContent.Length > 0) // No need to write empty spaces
                {
                    _styleName = ClassName + "-" + "before";
                    // The stylename without styles will be ignored for Pseudo.
                    //_pseudoBuilder.Append("<text:span");
                if (dictContents.PseudoWithoutStyles.Contains(_styleName))
                {
                    //_pseudoBuilder.Append(writingContent);
                    _pseudoBuilderCurrentNode.Append(writingContent);
                }
                else
                {
                    //_pseudoBuilder.Append("<text:span ");
                    //_pseudoBuilder.Append("text:style-name=\"" + _styleName + "\">" + writingContent);
                    //_pseudoBuilder.Append("</text:span>");

                    _pseudoBuilderCurrentNode.Append("<text:span ");
                    _pseudoBuilderCurrentNode.Append("text:style-name=\"" + _styleName + "\">" + writingContent);
                    _pseudoBuilderCurrentNode.Append("</text:span>");

                }
                }
            }
        }

        private void CreateFile(string targetPath)
        {
            string targetContentXML = targetPath + "content.xml";
            _writer = new XmlTextWriter(targetContentXML, null) { Formatting = Formatting.Indented };
            _writer.WriteStartDocument();

            //office:document-content Attributes.
            _writer.WriteStartElement("office:document-content");
            _writer.WriteAttributeString("xmlns:office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
            _writer.WriteAttributeString("xmlns:style", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
            _writer.WriteAttributeString("xmlns:text", "urn:oasis:names:tc:opendocument:xmlns:text:1.0");
            _writer.WriteAttributeString("xmlns:table", "urn:oasis:names:tc:opendocument:xmlns:table:1.0");
            _writer.WriteAttributeString("xmlns:draw", "urn:oasis:names:tc:opendocument:xmlns:drawing:1.0");
            _writer.WriteAttributeString("xmlns:fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");
            _writer.WriteAttributeString("xmlns:xlink", "http://www.w3.org/1999/xlink");
            _writer.WriteAttributeString("xmlns:dc", "http://purl.org/dc/elements/1.1/");
            _writer.WriteAttributeString("xmlns:meta", "urn:oasis:names:tc:opendocument:xmlns:meta:1.0");
            _writer.WriteAttributeString("xmlns:number", "urn:oasis:names:tc:opendocument:xmlns:datastyle:1.0");
            _writer.WriteAttributeString("xmlns:svg", "urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0");
            _writer.WriteAttributeString("xmlns:chart", "urn:oasis:names:tc:opendocument:xmlns:chart:1.0");
            _writer.WriteAttributeString("xmlns:dr3d", "urn:oasis:names:tc:opendocument:xmlns:dr3d:1.0");
            _writer.WriteAttributeString("xmlns:math", "http://www.w3.org/1998/Math/MathML");
            _writer.WriteAttributeString("xmlns:form", "urn:oasis:names:tc:opendocument:xmlns:form:1.0");
            _writer.WriteAttributeString("xmlns:script", "urn:oasis:names:tc:opendocument:xmlns:script:1.0");
            _writer.WriteAttributeString("xmlns:ooo", "http://openoffice.org/2004/office");
            _writer.WriteAttributeString("xmlns:ooow", "http://openoffice.org/2004/writer");
            _writer.WriteAttributeString("xmlns:oooc", "http://openoffice.org/2004/calc");
            _writer.WriteAttributeString("xmlns:dom", "http://www.w3.org/2001/xml-events");
            _writer.WriteAttributeString("xmlns:xforms", "http://www.w3.org/2002/xforms");
            _writer.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
            _writer.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            _writer.WriteAttributeString("xmlns:rpt", "http://openoffice.org/2005/report");
            _writer.WriteAttributeString("xmlns:of", "urn:oasis:names:tc:opendocument:xmlns:of:1.2");
            _writer.WriteAttributeString("xmlns:xhtml", "http://www.w3.org/1999/xhtml");
            _writer.WriteAttributeString("xmlns:grddl", "http://www.w3.org/2003/g/data-view#");
            _writer.WriteAttributeString("xmlns:field", "urn:openoffice:names:experimental:ooo-ms-interop:xmlns:field:1.0");
            _writer.WriteAttributeString("grddl:transformation", "http://docs.oasis-open.org/office/1.2/xslt/odf2rdf.xsl");
            _writer.WriteStartElement("office:scripts");
            if (_structStyles.IsMacroEnable)
            {
                _writer.WriteStartElement("office:event-listeners");
                _writer.WriteStartElement("script:event-listener");
                _writer.WriteAttributeString("script:language", "ooo:script");
                _writer.WriteAttributeString("script:event-name", "dom:load");
                _writer.WriteAttributeString("xlink:href", "vnd.sun.star.script:Standard.Module1.StartDontForget?language=Basic&location=document");
                _writer.WriteEndElement();
                _writer.WriteStartElement("script:event-listener");
                _writer.WriteAttributeString("script:language", "ooo:script");
                _writer.WriteAttributeString("script:event-name", "office:print");
                if (string.Compare(_projectType, "Scripture") == 0)
                {
                    _writer.WriteAttributeString("xlink:href", "vnd.sun.star.script:Standard.Module1.IsReferenceCorrected?language=Basic&location=document");
                }
                else
                {
                    _writer.WriteAttributeString("xlink:href", "vnd.sun.star.script:Standard.Module1.IsGuidewordsCorrected?language=Basic&location=document");
                }
                _writer.WriteEndElement();
                _writer.WriteEndElement();
            }
            _writer.WriteEndElement();

            //office:font-face-decls Attributes.
            _writer.WriteStartElement("office:font-face-decls");
            _writer.WriteStartElement("style:font-face");
            _writer.WriteAttributeString("style:name", "Times New Roman");
            _writer.WriteAttributeString("svg:font-family", "'Times New Roman'");
            _writer.WriteAttributeString("style:font-family-generic", "roman");
            _writer.WriteAttributeString("style:font-pitch", "variable");
            _writer.WriteEndElement();
            _writer.WriteStartElement("style:font-face");
            _writer.WriteAttributeString("style:name", "Yi plus Phonetics");
            _writer.WriteAttributeString("svg:font-family", "'Yi plus Phonetics'");
            _writer.WriteEndElement();
            _writer.WriteStartElement("style:font-face");
            _writer.WriteAttributeString("style:name", "Arial");
            _writer.WriteAttributeString("svg:font-family", "'Arial'");
            _writer.WriteAttributeString("style:font-family-generic", "swiss");
            _writer.WriteAttributeString("style:font-pitch", "variable");
            _writer.WriteEndElement();
            _writer.WriteStartElement("style:font-face");
            _writer.WriteAttributeString("style:name", "Lucida Sans Unicode");
            _writer.WriteAttributeString("svg:font-family", "'Lucida Sans Unicode'");
            _writer.WriteAttributeString("style:font-family-generic", "system");
            _writer.WriteAttributeString("style:font-pitch", "variable");
            _writer.WriteEndElement();
            _writer.WriteStartElement("style:font-face");
            _writer.WriteAttributeString("style:name", "MS Mincho");
            _writer.WriteAttributeString("svg:font-family", "'MS Mincho'");
            _writer.WriteAttributeString("style:font-family-generic", "system");
            _writer.WriteAttributeString("style:font-pitch", "variable");
            _writer.WriteEndElement();
            _writer.WriteStartElement("style:font-face");
            _writer.WriteAttributeString("style:name", "Tahoma");
            _writer.WriteAttributeString("svg:font-family", "'Tahoma'");
            _writer.WriteAttributeString("style:font-family-generic", "system");
            _writer.WriteAttributeString("style:font-pitch", "variable");
            _writer.WriteEndElement();
            _writer.WriteStartElement("style:font-face");
            _writer.WriteAttributeString("style:name", "Scheherazade Graphite Alpha");
            _writer.WriteAttributeString("svg:font-family", "'Scheherazade Graphite Alpha'");
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            //office:automatic-styles - Sections and Columns area
            _writer.WriteStartElement("office:automatic-styles");
            _writer.WriteStartElement("style:style");
            _writer.WriteAttributeString("style:name", "P4");
            _writer.WriteAttributeString("style:family", "paragraph");
            _writer.WriteAttributeString("style:parent-style-name", "hide");
            _writer.WriteAttributeString("style:master-page-name", "First_20_Page");
            _writer.WriteStartElement("style:paragraph-properties");
            _writer.WriteAttributeString("style:page-number", "auto");
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            if (_fileType == "odm")
            {
                _writer.WriteStartElement("style:style");
                _writer.WriteAttributeString("style:name", "SectODM");
                _writer.WriteAttributeString("style:family", "section");

                _writer.WriteStartElement("style:section-properties");
                _writer.WriteAttributeString("style:editable", "false");

                _writer.WriteStartElement("style:columns");
                _writer.WriteAttributeString("fo:column-count", "1");
                _writer.WriteAttributeString("fo:column-gap", "0in");

                _writer.WriteEndElement();
                _writer.WriteEndElement();
                _writer.WriteEndElement();
            }

        }

        ///// -------------------------------------------------------------------------------------------
        ///// <summary>
        ///// To find the width of the image.
        ///// 
        ///// <list> 
        ///// </list>
        ///// </summary>
        ///// <param> </param>
        ///// <returns> </returns>
        ///// -------------------------------------------------------------------------------------------
        //private static string CalcDimension(string fromPath, string imgDimension, char Type)
        //{
        //    Image fullimage = Image.FromFile(fromPath);
        //    double height = fullimage.Height;
        //    double width = fullimage.Width;
        //    double retValue = 0.0;
        //    fullimage.Dispose();
        //    if (Type == 'W')
        //    {
        //        retValue = width / height * double.Parse(imgDimension);
        //    }
        //    else if (Type == 'H')
        //    {
        //        retValue = height / width * double.Parse(imgDimension);
        //    }
        //    return retValue.ToString();
        //}

        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Generate execute before Span and Div Tag.
        /// 
        /// <list> 
        /// </list>
        /// </summary>
        /// <param> </param>
        /// <returns> </returns>
        /// -------------------------------------------------------------------------------------------
        private void BeforeDivSpan(IList sectionList, string readerValue)
        {

            RemoveScrSectionClass(sectionList);
            string sectionName = string.Empty;
            if (sectionList.Contains(readerValue))
            {
                sectionName = readerValue;
            }
            else if (sectionList.Contains(readerValue + "." + _tagType))
            {
                sectionName = readerValue + "." + _tagType;
            }
            if (sectionName.Length > 0)
            {
                sectionName = "Sect_" + sectionName;
                _writer.WriteStartElement("text:section");
                _writer.WriteAttributeString("text:style-name", sectionName);
                _writer.WriteAttributeString("text:name", sectionName);
                _sectionCount++;
            }
        }

        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Handle Relative Values
        /// 
        /// <list> 
        /// </list>
        /// </summary>
        /// <param> </param>
        /// <returns>true, has Relative values</returns>
        /// -------------------------------------------------------------------------------------------
        private bool RelativeValue(string styleFilePath, Stack styleStack, Styles structStyles, string readerValue)
        {
            bool isRelative = false;
            _fontsizeAttributeInfo.NumericValue = 100;
            if (structStyles.CssClassName[readerValue].ContainsKey("fo:font-size")) // find fo:font-size value for substitute to em
            {
                _fontsizeAttributeInfo.SetAttribute(readerValue, "fo:font-size", structStyles.CssClassName[readerValue]["fo:font-size"]);
                if (_fontsizeAttributeInfo.Unit == "em")
                {
                    _fontsizeAttributeInfo.NumericValue = _fontsizeAttributeInfo.NumericValue * 100;
                }
            }

            _fontsizeAttributeInfo.Name = "fo:font-size";
            _fontsizeAttributeInfo.Unit = "%";
            string parentFontsize = _util.GetFontsize(styleFilePath, styleStack, _fontsizeAttributeInfo);
            var parentFontsizeAttribute = new StyleAttribute();
            parentFontsizeAttribute.SetAttribute(parentFontsize);

            string absVal;
            foreach (KeyValuePair<string, string> kvp in structStyles.CssClassName[readerValue])
            {
                _attributeInfo.SetAttribute(readerValue, kvp.Key, kvp.Value);
                if (_attributeInfo.Name == "fo:font-size")
                {
                    if (_attributeInfo.Unit.ToLower() == "larger" || _attributeInfo.Unit.ToLower() == "smaller")
                    {
                        int value = Common.GetLargerSmaller(parentFontsizeAttribute.NumericValue, _attributeInfo.StringValue.ToLower());
                        absVal = value.ToString() + "pt";
                    }
                    else if (_attributeInfo.Unit.ToLower() == "largerx2" || _attributeInfo.Unit.ToLower() == "smallerx2")
                    {
                        // for superscript and subscript
                        string stringValue = string.Empty;
                        parentFontsizeAttribute.NumericValue = parentFontsizeAttribute.NumericValue * 1.4F;
                        if (_attributeInfo.Unit.ToLower() == "largerx2")
                        {
                            stringValue = "larger";
                        }
                        else if (_attributeInfo.Unit.ToLower() == "smallerx2")
                        {
                            stringValue = "smaller";
                        }

                        int value = Common.GetLargerSmaller(parentFontsizeAttribute.NumericValue, stringValue);
                        absVal = value.ToString() + "pt";
                    }
                    else
                    {
                        absVal = parentFontsize; // Use calculated value
                    }
                }
                else if (_attributeInfo.Unit == "em")
                {
                    if (_attributeInfo.Name == "fo:letter-spacing") // emTest2 and emTest3
                    {
                        float emResult = parentFontsizeAttribute.NumericValue * _attributeInfo.NumericValue;
                        absVal = emResult + "pt";
                    }
                    else if (_attributeInfo.Name == "fo:line-height")
                    {
                        absVal = _util.GetFontsizeInsideSpan(styleFilePath, parentFontsizeAttribute.NumericValue, _attributeInfo);
                        float parentFontSize = float.Parse(absVal.Replace("pt", ""));
                        if (parentFontSize == 0)
                        {
                            //parentFontSize = parentFontsizeAttribute.NumericValue * 1; 
                            absVal = "100%"; //"parentFontSize + "pt";
                        }
                        else
                        {
                            absVal = parentFontSize + "pt";
                        }
                    }
                    else
                    {
                        absVal = _util.GetFontsizeInsideSpan(styleFilePath, parentFontsizeAttribute.NumericValue, _attributeInfo);
                        float parentFontSize = float.Parse(absVal.Replace("pt", ""));
                        absVal = parentFontSize + "pt";
                    }
                }
                else if (_attributeInfo.StringValue == "bolder" || _attributeInfo.StringValue == "lighter")
                {
                    absVal = _util.GetFontweight(styleFilePath, styleStack, _attributeInfo);
                }
                else if (_attributeInfo.Unit == "-em") // TD-143  Open Office: line-height: 110pt;  
                {
                    float lineHeightpt = _attributeInfo.NumericValue;
                    _attributeInfo.NumericValue = 1;
                    _attributeInfo.Unit = "em";
                    absVal = _util.GetFontsizeInsideSpan(styleFilePath, parentFontsizeAttribute.NumericValue, _attributeInfo);
                    //Common.ConvertToInch(absVal);
                    float parentFontSize = float.Parse(absVal.Replace("pt", ""));
                    lineHeightpt = (lineHeightpt - parentFontSize) / 2;

                    absVal = lineHeightpt + "pt";
                }
                else
                {
                    absVal = _util.GetFontsize(styleFilePath, styleStack, _attributeInfo);
                }
                if (_attributeInfo.Name == "fo:column-gap")
                {
                    if (structStyles.ColumnGapEm.ContainsKey("Sect_" + readerValue))
                    {
                        structStyles.ColumnGapEm["Sect_" + readerValue]["columnGap"] = absVal;
                        _columnWidth = structStyles.ColumnWidth - Common.ConvertToInch(absVal); // for picture size calculation
                    }
                    continue;
                }
                _makeAttribute.Add(kvp.Key, absVal);
            }
            if (_makeAttribute.Count > 0)
            {
                isRelative = true;
                _util.GetNewChildName(styleFilePath, styleStack, readerValue, true);
                _styleName = _util.ChildName;
                //if (!ExistingStyleName.Contains(util.ChildName))
                {
                    _util.CreateStyleWithNewValue(styleFilePath, readerValue, _util.ChildName, _makeAttribute, _util.ParentName, _familyType, _structStyles.BackgroundColor);
                    //ExistingStyleName.Add(util.ChildName);
                }
            }
            return isRelative;
        }

        /// <summary>
        /// Generate Section block in Content.xml.
        /// </summary>
        /// <param name="sectionName">SectionName List created in styles.xml</param>
        private void CreateSection(ArrayList sectionName)
        {

            RemoveScrSectionClass(sectionName);
            foreach (string section in sectionName)
            {
                //string path = Common.PathCombine(Common.GetTempFolderPath(), section.Trim() + ".xml");
                string secName = "_" + section.Trim() + ".xml";
                string path = Common.PathCombine(Path.GetTempPath(), secName);
                if (!File.Exists(path))
                {
                    return;
                }
                var xmlReader = new XmlTextReader(path);

                xmlReader.Read();
                xmlReader.Read();
                xmlReader.Read();
                string xml = xmlReader.ReadInnerXml();
                if (xml != "")
                {
                    _writer.WriteRaw(xml);
                }
                xmlReader.Close();
                // Clean UP
                File.Delete(path);
            }
        }

        /// <summary>
        /// If two classes "columns" and "scrSection" exist in CSS. This method to remove the scrSection when "columns" exists.
        /// for the Task TD-1239
        /// </summary>
        /// <param name="classList"></param>
        private static void RemoveScrSectionClass(IList classList)
        {
            bool isClassnameExits = false;
            if (classList.Contains("columns") && classList.Contains("scrSection"))
            {
                isClassnameExits = true;
            }
            if (isClassnameExits)
                classList.RemoveAt(classList.IndexOf("scrSection"));
        }

        /// <summary>
        /// Inserts the Image with Caption
        /// </summary>
        /// 
        /// <param name="styleFilePath">Styles.xml file path</param>
        /// <param name="Targetfile">To path of Target File</param>
        /// 
        private void InsertImageCaption(string styleFilePath, string targetfile)
        {
            string pos = string.Empty;
            string side = string.Empty;
            //int divCount = 1;
            if (_styleStack.Count > 0)
            {
                string stackClass = _styleStack.Peek().ToString();
                string[] splitedClassName = stackClass.Split('_');
                if (splitedClassName.Length > 0)
                {
                    //for (int i = splitedClassName.Length -1; i >= 0; i--)  // From Begining to Recent Class
                    for (int i = 0; i < splitedClassName.Length; i++) // // From Recent to Begining Class
                    {
                        string clsName = splitedClassName[i];
                        if (pos == string.Empty && _structStyles.FloatAlign.ContainsKey(clsName))
                        {
                            pos = _structStyles.FloatAlign[clsName];
                        }

                        if (side == string.Empty && _structStyles.ClearProperty.ContainsKey(clsName))
                        {
                            side = _structStyles.ClearProperty[clsName];
                        }

                        if (pos != string.Empty && side != string.Empty)
                        {
                            break;
                        }
                    }
                }
            }

            string classPicture = _reader.GetAttribute("class") ?? "img";
            string src = _reader.GetAttribute("src").ToLower();
            string longdesc = _reader.GetAttribute("longdesc");
            longdesc = longdesc != null ? longdesc.ToLower() : "";
            string altText = _reader.GetAttribute("alt");

            if (_structStyles.ImageSource.ContainsKey(longdesc))
            {
                Dictionary<string, string> tempDict = _structStyles.ImageSource[longdesc];
                if (tempDict.ContainsKey("float"))
                {
                    pos = tempDict["float"];
                }
            }
            _imageTextAvailable = false;
            ImageBuild(styleFilePath, targetfile, src, altText, classPicture, pos, side, longdesc);
            _imageStart = true;
            _imageParent = _util.ChildName;

            /*
            while (_reader.Read())
            {
                if (_reader.IsEmptyElement)
                {
                    continue;
                }
                switch (_reader.NodeType)
                {
                    case XmlNodeType.Element:
                        divCount++;
                        break;
                    case XmlNodeType.EndElement:
                        divCount--;
                        if (divCount == 0)
                        {

                            ImageBuild(styleFilePath, Targetfile, src, altText, classPicture, caption, pos, side, readerValue);
                            _imageBuilder.Remove(0, _imageBuilder.Length);

                            _styleStack.Pop().ToString();
                            string tagStyle = _tagTypeStack.Pop().ToString();
                            if (tagStyle == "P")
                            {
                                _allDivStack.Pop();
                            }
                            else if (tagStyle == "T")
                            {
                                _allSpanStack.Pop();
                            }
                            return;
                        }
                        break;
                    case XmlNodeType.Text:
                        caption += _reader.Value;
                        break;
                }
            }
           */
            //return;
        }

        private void ImageBuild(string styleFilePath, string targetFile, string src, string altText, string classPicture, string pos, string side, string readerValue)
        {
            Common.ColumnWidth = _structStyles.ColumnWidth;
            _writer.Formatting = Formatting.None;
            string imgHeight = "0";
            string imgWidth = "0";
            string imgHUnit = "in";
            string imgWUnit = "in";

            string fromPath = Common.GetPictureFromPath(src, _metaValue, _sourcePicturePath);
            string fileName = Path.GetFileName(src);

            string normalTargetFile = targetFile;
            string basePath = normalTargetFile.Substring(0, normalTargetFile.LastIndexOf(Path.DirectorySeparatorChar));
            String toPath = Common.DirectoryPathReplace(basePath + "/Pictures/" + fileName);
            if (File.Exists(fromPath))
            {
                File.Copy(fromPath, toPath, true);
                if (_structStyles.ImageSize.ContainsKey(classPicture))
                {
                    var imageDetails = new ArrayList();
                    if (_structStyles.ImageSource.ContainsKey(readerValue.ToLower()))
                    {
                        Dictionary<string, string> tempDict = _structStyles.ImageSource[readerValue];
                        imageDetails.Add(tempDict["height"]);
                        imageDetails.Add(tempDict["width"]);
                    }
                    else
                    {
                        imageDetails = _structStyles.ImageSize[classPicture];
                    }

                    foreach (string attb in imageDetails)
                    {
                        string[] splitParam = attb.Split(',');
                        string imgDimenstion = splitParam[0];
                        try
                        {
                            if (imgDimenstion == "height")
                            {
                                imgHeight = splitParam[1].Trim();
                                imgHUnit = splitParam[2].Trim();

                            }
                            else if (imgDimenstion == "width")
                            {
                                imgWidth = splitParam[1].Trim();
                                imgWUnit = splitParam[2].Trim();
                                if (imgWUnit == "%")
                                {
                                    float iWidth = _columnWidth * float.Parse(imgWidth) / 100F;
                                    imgWidth = iWidth.ToString();
                                    imgWUnit = "in";
                                }
                            }
                        }
                        catch
                        {
                            imgHeight = "1";
                            imgWidth = "1";
                        }
                    }
                    if (imgHeight != "0")
                    {
                        if (imgWidth == "0")
                        {
                            imgWUnit = imgHUnit;
                            imgWidth = Common.CalcDimension(fromPath, imgHeight, 'W');
                        }
                    }
                    else if (imgWidth != "0")
                    {
                        //imgHUnit = imgWUnit;
                        imgHeight = Common.CalcDimension(fromPath, imgWidth, 'H');
                    }
                }
                else
                {
                    imgHeight = "1";
                    imgWidth = Common.CalcDimension(fromPath, imgHeight, 'W');
                }
            }
            else // default value
            {
                imgHeight = "1";
                imgWidth = "1";
            }

            string strFrameCount = "Graphics" + _frameCount;
            _imageGraphicsName = strFrameCount;
            if (!_divOpen)  // Forcing a Paragraph Style, if it is not exist
            {
                int counter = _styleStack.Count;
                string divTagName = string.Empty;
                if (counter > 0)
                {
                    var tempStyle = new string[counter];
                    _styleStack.CopyTo(tempStyle, 0);
                    divTagName = counter > 1 ? tempStyle[1] : tempStyle[0];
                }
                _writer.WriteStartElement("text:p");
                //_writer.WriteAttributeString("text:style-name", _util.ParentName);
                _writer.WriteAttributeString("text:style-name", divTagName);

                //_divOpen = true;
            }


            // 1st frame
            _writer.WriteStartElement("draw:frame");
            _writer.WriteAttributeString("draw:style-name", strFrameCount);
            _writer.WriteAttributeString("draw:name", strFrameCount);

            _imageZindexCounter++;
            string anchorType = string.Empty;
            //if (_imageZindexCounter == 1 || _imageZindexCounter == 4)
            if (_imagePreviousFinished)
            {
                anchorType = "char";
                _writer.WriteAttributeString("text:anchor-type", anchorType);
                _writer.WriteAttributeString("draw:z-index", "2");
                side = "both";
                pos = "center";
            }
            else if (pos.Length > 0)
            {
                anchorType = "paragraph";
                _writer.WriteAttributeString("text:anchor-type", anchorType);
                _writer.WriteAttributeString("draw:z-index", "1");
            }
            else
            {
                anchorType = "as-char";
                _writer.WriteAttributeString("text:anchor-type", anchorType);
                _writer.WriteAttributeString("draw:z-index", "0");
            }

            _writer.WriteAttributeString("svg:width", imgWidth + imgWUnit);
            _writer.WriteAttributeString("svg:height", imgHeight + imgWUnit);
            //TD-349(width:auto)
            if (_structStyles.IsAutoWidthforCaption)
            {
                _writer.WriteAttributeString("fo:min-width", imgWidth + imgWUnit);
            }

            //1st textbox
            _writer.WriteStartElement("draw:text-box");
            _writer.WriteAttributeString("fo:min-height", "0in");

            _frameCount++;

            _util.CreateGraphicsStyle(styleFilePath, strFrameCount, _util.ParentName, pos, side);

            _writer.WriteStartElement("draw:frame");
            _writer.WriteAttributeString("draw:style-name", strFrameCount);
            _writer.WriteAttributeString("draw:name", strFrameCount);
            _writer.WriteAttributeString("text:anchor-type", "paragraph");
            // _writer.WriteAttributeString("text:anchor-type", anchorType);
            _writer.WriteAttributeString("svg:width", imgWidth + imgWUnit);
            _writer.WriteAttributeString("svg:height", imgHeight + imgWUnit);

            //_writer.WriteAttributeString("style:rel-height", "scale");
            //_writer.WriteAttributeString("style:rel-width", "100%");

            _writer.WriteStartElement("draw:image");
            _writer.WriteAttributeString("xlink:type", "simple");
            _writer.WriteAttributeString("xlink:show", "embed");
            _writer.WriteAttributeString("xlink:actuatet", "onLoad");
            _writer.WriteAttributeString("xlink:href", "Pictures/" + fileName);
            _writer.WriteEndElement();

            _writer.WriteStartElement("svg:desc");
            _writer.WriteString(altText);

            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }


        private void CheckMetaRootDirectory()
        {
            if (_reader.Name == "meta")
            {
                if (_reader.GetAttribute("name") != null)
                {
                    string metaName = _reader.GetAttribute("name");
                    if (string.Compare(metaName, "extLinkRootDir", true) >= 0)
                    {
                        if (_reader.GetAttribute("content") != null)
                        {
                            _metaValue = _reader.GetAttribute("content");
                        }
                    }

                }
            }
        }
        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Generate Second block of Content.xml, this block will called after inserting the column
        /// 
        /// <list> 
        /// </list>
        /// </summary>
        /// <param> </param>
        /// <returns> </returns>
        /// -------------------------------------------------------------------------------------------
        private void CreateBody()
        {
            _writer.WriteEndElement();
            //office:body Attributes.
            _writer.WriteStartElement("office:body");
            _writer.WriteStartElement("office:text");

            //_writer.WriteStartElement("office:forms");
            //_writer.WriteAttributeString("form:automatic-focus", "false");
            //_writer.WriteAttributeString("form:apply-design-mode", "false");
            //_writer.WriteEndElement();

            _writer.WriteStartElement("text:sequence-decls");
            _writer.WriteStartElement("text:sequence-decl");
            _writer.WriteAttributeString("text:display-outline-level", "0");
            _writer.WriteAttributeString("text:name", "Illustration");
            _writer.WriteEndElement();
            _writer.WriteStartElement("text:sequence-decl");
            _writer.WriteAttributeString("text:display-outline-level", "0");
            _writer.WriteAttributeString("text:name", "Table");
            _writer.WriteEndElement();
            _writer.WriteStartElement("text:sequence-decl");
            _writer.WriteAttributeString("text:display-outline-level", "0");
            _writer.WriteAttributeString("text:name", "Text");
            _writer.WriteEndElement();
            _writer.WriteStartElement("text:sequence-decl");
            _writer.WriteAttributeString("text:display-outline-level", "0");
            _writer.WriteAttributeString("text:name", "Drawing");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("text:p");
            _writer.WriteAttributeString("text:style-name", "P4");
            _writer.WriteEndElement();

            //if (_fileType == "odm" && _odtFiles != null)  // ODM - ODT files
            //{
            //    int MainPosition = 0;
            //    if (_odtFiles.Contains("Main"))
            //    {
            //        MainPosition = _odtFiles.IndexOf("Main");
            //    }
            //    _odtEndFiles = new ArrayList();
            //    for (int i = MainPosition + 1; i < _odtFiles.Count; i++)
            //    {
            //        _odtEndFiles.Add(_odtFiles[i]);
            //    }

            //    for (int i = 0; i < MainPosition; i++)
            //    {
            //        string outputFile = _odtFiles[i].ToString().Replace("xhtml", "odt");
            //        _writer.WriteStartElement("text:section");
            //        _writer.WriteAttributeString("text:style-name", "SectODM");
            //        _writer.WriteAttributeString("text:name", outputFile);
            //        //_writer.WriteAttributeString("text:protected", "true"); // read only
            //        _writer.WriteAttributeString("text:protected", "false"); // Enabled for macro purpose - headword

            //        _writer.WriteStartElement("text:section-source");
            //        _writer.WriteAttributeString("xlink:href", "../" + outputFile);
            //        _writer.WriteAttributeString("text:filter-name", "writer8");
            //        _writer.WriteEndElement();
            //        _writer.WriteEndElement();
            //    }
            //}
        }

        private string SignificantSpace(string content)
        {
            //string content = _reader.Value;
                content = content.Replace("\r\n", "");
                content = content.Replace("\t", "");
                Char[] charac = content.ToCharArray();
                StringBuilder builder = new StringBuilder();
                foreach (char var in charac)
                {
                    if (var == ' ' || var == '\b')
                    {
                        if (_significant)
                        {
                            continue;
                        }
                        _significant = true;
                    }
                    else
                    {
                        _significant = false;
                    }
                    builder.Append(var);
                }
                content = builder.ToString();
            return content;
            //_writer.WriteString(content);
        }
        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Generate last block of Content.xml
        /// 
        /// <list> 
        /// </list>
        /// </summary>
        /// <returns> </returns>
        /// -------------------------------------------------------------------------------------------
        private void CloseFile(string targetPath, Dictionary<string, Dictionary<string, string>> dictColumnGapEm)
        {
            string targetFile = targetPath + "content.xml";
            //if (_odtEndFiles != null)
            //{
            //    for (int i = 0; i < _odtEndFiles.Count; i++)  // ODM - ODT 
            //    {
            //        string outputFile = _odtEndFiles[i].ToString().Replace("xhtml", "odt");
            //        _writer.WriteStartElement("text:section");
            //        _writer.WriteAttributeString("text:style-name", "SectODM");
            //        _writer.WriteAttributeString("text:name", outputFile);
            //        //_writer.WriteAttributeString("text:protected", "true"); // read only
            //        _writer.WriteAttributeString("text:protected", "false"); // Enabled for macro purpose - headword

            //        _writer.WriteStartElement("text:section-source");
            //        _writer.WriteAttributeString("xlink:href", "../" + outputFile);
            //        _writer.WriteAttributeString("text:filter-name", "writer8");
            //        _writer.WriteEndElement();
            //        _writer.WriteEndElement();
            //    }
            //}

            _writer.WriteEndElement();
            _writer.WriteEndDocument();
            _writer.Flush();
            _writer.Close();

            if (_reader != null)
                _reader.Close();

            if (dictColumnGapEm.Count > 0)
            {
                var xmlDoc = new XmlDocument { PreserveWhitespace = true };
                var nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                nsmgr.AddNamespace("st", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
                nsmgr.AddNamespace("fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");
                xmlDoc.Load(targetFile);
                XmlElement root = xmlDoc.DocumentElement;
                Dictionary<string, XmlNode> ColumnGap = _util.SetColumnGap(targetFile, dictColumnGapEm);
                foreach (KeyValuePair<string, XmlNode> secName in ColumnGap)
                {
                    string style = "//st:style[@st:name='" + secName.Key + "']//st:columns";
                    if (root != null)
                    {
                        XmlNode ele = root.SelectSingleNode(style, nsmgr);
                        if (ele != null)
                        {
                            for (int i = ele.ChildNodes.Count - 1; i >= 0; i--)
                            {
                                if (ele.ChildNodes[i].Name == "style:column" || ele.ChildNodes[i].Name == "#whitespace")
                                {
                                    ele.RemoveChild(ele.ChildNodes[i]);
                                }
                            }
                            XmlNode colNode = ColumnGap[secName.Key];
                            XmlDocumentFragment styleNode = xmlDoc.CreateDocumentFragment();
                            styleNode.InnerXml = colNode.InnerXml;
                            ele.AppendChild(styleNode);
                        }
                    }
                }
                xmlDoc.Save(targetFile);
            }
        }
        #endregion
    }
}
