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
using System.Globalization;
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
    public class OOContent : XHTMLProcess
    {
        #region Private Variable

        readonly Stack _styleStack = new Stack();
        readonly Stack _allSpanStack = new Stack();
        readonly Stack _allDivStack = new Stack();
        readonly Stack _usedSpanStack = new Stack();
        readonly Stack _tagTypeStack = new Stack(); // P - Para, T - Text, I - Image, L - <ol>/<ul> tag , S - <li> tag,  O - Others
        readonly Dictionary<string, string> _makeAttribute = new Dictionary<string, string>();
        private ArrayList _sectionName = new ArrayList();
        //Dictionary<string, string> dictColumnGap = new Dictionary<string, string>();

        bool isHiddenText = false;
        readonly OOUtility _util = new OOUtility();
        readonly StyleAttribute _attributeInfo = new StyleAttribute();
        readonly StyleAttribute _fontsizeAttributeInfo = new StyleAttribute();
        string _familyType = string.Empty;
        string _styleName = string.Empty;
        // Image
        int _frameCount; //For Picture Frame count
        string _beforePseudoValue = string.Empty;
        string _beforePseudoParentValue = string.Empty;
        //readonly StringBuilder _imageBuilder = new StringBuilder();
        readonly Stack _prevLangStack = new Stack(); // For handle previous Language
        readonly StringBuilder _pseudoBuilder = new StringBuilder();
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
        OldStyles _structStyles;
        //readonly Library _lib = new Library();
        int _pbCounter;
        string _readerValue = string.Empty;
        string _footnoteValue = string.Empty;
        string _classAfter = string.Empty;
        char _closeTagType;
        string _parentStyleName;
        readonly Dictionary<string, string> _existingStyleName = new Dictionary<string, string>();

        string _styleFilePath;
        float _columnWidth;
        string _temp;
        string _divClass = string.Empty;
        string _allDiv = string.Empty;
        string _class = string.Empty;
        bool _divOpen;
        //bool _isWhiteSpace;
        bool _isNewLine = true;
        string _prevLang = string.Empty;
        string _parentClass = string.Empty;
        string _parentLang = string.Empty;
        string _projectType = string.Empty;
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

        private List<string> _unUsedParagraphStyle = new List<string>();

        #endregion
        #region Private Variable
        private int _storyNo = 0;
        private int _hyperLinkNo = 0;
        private bool isFileEmpty = true;
        private bool isFileCreated;
        private bool isImage;
        private bool isHomographNumber = false;
        private string imageClass = string.Empty;
        private string _inputPath;
        private ArrayList _textFrameClass = new ArrayList();
        private ArrayList _textVariables = new ArrayList();
        private ArrayList _columnClass = new ArrayList();
        private ArrayList _psuedoBefore = new ArrayList();
        public Dictionary<string, Dictionary<string, string>> contentCounterIncrement = new Dictionary<string, Dictionary<string, string>>();
        public Dictionary<string, string> ContentCounterReset = new Dictionary<string, string>();
        public Dictionary<string, int> ContentCounter = new Dictionary<string, int>();
        private Dictionary<string, ClassInfo> _psuedoAfter = new Dictionary<string, ClassInfo>();
        //private Dictionary<string, ArrayList> _styleName = new Dictionary<string, ArrayList>();
        private ArrayList _crossRef = new ArrayList();
        private int _crossRefCounter = 1;
        private bool _isWhiteSpace = true;
        private bool _imageInserted;
        private bool _footnoteStart = false;
        bool isFootnote = false;
        private string footnoteClass = string.Empty;
        private StringBuilder footnoteContent = new StringBuilder();
        //private InDesignStyles _inDesignStyles;
        private ArrayList _FootNote = new ArrayList();
        private ArrayList _footnoteCallContent = new ArrayList();
        private ArrayList _footnoteMarkerContent = new ArrayList();
        private List<string> _usedStyleName = new List<string>();
        private bool _chapterNoStart;
        private bool _verserNoStart;
        private string _chapterNo;
        private string _verseNo;
        private PublicationInformation _projInfo;
        private bool _IsHeadword = false;
        private bool _significant;
        private Dictionary<string, Dictionary<string, string>> _dictColumnGapEm = new Dictionary<string, Dictionary<string, string>>();

        #endregion

        public OOContent()
        {
            _outputType = Common.OutputType.ODT;
        }
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

        public void InitializeObject(OldStyles styleInfo, string fileType)
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
        //public void CreateContent(PublicationInformation projInfo, Styles structStyles)
        //{
        //    //TODO: When the Input has been preprocessed, GetProjectType returns the wrong type!
        //    //_projectType = Common.GetProjectType(projInfo.DefaultXhtmlFileWithPath);
        //    _projectType = projInfo.ProjectInputType;
        //    if (_projectType == "Scripture")
        //    {
        //        structStyles.IsMacroEnable = true;
        //    }
        //    InitializeObject(structStyles, projInfo.OutputExtension); // Creates new Objects
        //    CreateFile(projInfo.TempOutputFolder);
        //    //CreateSection(structStyles.SectionName);
        //    //projInfo.DefaultXhtmlFileWithPath = PreProcess(projInfo.DefaultXhtmlFileWithPath, structStyles);
        //    Common.SetProgressBarValue(projInfo.ProgressBar, projInfo.DefaultXhtmlFileWithPath);
        //    CreateBody();
        //    Console.WriteLine(Common.OdType.ToString());
        //    if (Common.OdType != Common.OdtType.OdtMaster)
        //        ProcessXHTML(projInfo.ProgressBar, projInfo.DefaultXhtmlFileWithPath, projInfo.TempOutputFolder);
            
        //    CloseFile(projInfo.TempOutputFolder, structStyles.ColumnGapEm);
        //    AlterFrameWithoutCaption(projInfo.TempOutputFolder);
        //    CleanUp();
        //}

        public Dictionary<string, ArrayList> CreateStory(PublicationInformation projInfo,  Dictionary<string, Dictionary<string, string>> idAllClass, Dictionary<string, ArrayList> classFamily, ArrayList cssClassOrder)
        {
            OldStyles styleInfo = new OldStyles(); 

            _structStyles = styleInfo;
            string _inputPath = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath);
            InitializeData(projInfo, idAllClass, classFamily, cssClassOrder);
            ProcessProperty();
            OpenXhtmlFile(projInfo.DefaultXhtmlFileWithPath); //reader
            CreateFile(projInfo.TempOutputFolder); //writer
            CreateSection();
            Preprocess(projInfo.DefaultXhtmlFileWithPath);
            CreateBody();
            ProcessXHTML(projInfo.ProgressBar, projInfo.DefaultXhtmlFileWithPath, projInfo.TempOutputFolder);
            UpdateRelativeInStylesXML();
            CloseFile(projInfo.TempOutputFolder);
            return new Dictionary<string, ArrayList>();
        }

        private void Preprocess(string xhtmlFile)
        {
            AnchorTagProcessing(xhtmlFile);
            ReplaceString(xhtmlFile);

        }


        private void ProcessProperty()
        {
            foreach (string className in IdAllClass.Keys)
            {
                string searchKey = "column-count";
                if (IdAllClass[className].ContainsKey(searchKey))
                {
                    if (!_sectionName.Contains(className))
                    {
                        _sectionName.Add(className);
                    }

                }

                if(className.IndexOf("Sect_") >= 0)
                {
                    _dictColumnGapEm[className] = IdAllClass[className];
                }



                searchKey = "visibility";
                if (IdAllClass[className].ContainsKey(searchKey) && IdAllClass[className][searchKey] == "hidden")
                {
                    if (!_visibilityClassName.Contains(className))
                    {
                        _visibilityClassName.Add(className);
                    }

                }

                searchKey = "prince-text-replace";
                if (IdAllClass[className].ContainsKey(searchKey))
                {
                    _replaceSymbolToText.Clear();
                    string[] values = IdAllClass[className][searchKey].Split('\"');
                                for (int i = 0; i < values.Length; i++)
                                {
                                    if (values[i].Length > 0)
                                    {
                                        string key = values[i].Replace("\"", "");
                                        key = Common.ReplaceSymbolToText(key);
                                        i++;
                                        i++;
                                        string value = values[i].Replace("\"", "");
                                        value = Common.ReplaceSymbolToText(value);
                                        CssParser cssParser = new CssParser();
                                        _replaceSymbolToText[key] = cssParser.UnicodeConversion(value);
                                    }
                                }
                }

                // avoid white background color for pdf thru openoffice - TD-1573
                searchKey = "background-color";
                if (IdAllClass[className].ContainsKey(searchKey) && IdAllClass[className][searchKey] == "#ffffff")
                {
                    IdAllClass[className].Remove(searchKey);
                }

                searchKey = "list-style-type";
                if (IdAllClass[className].ContainsKey(searchKey))
                {
                    _listTypeDictionary[className] = IdAllClass[className][searchKey];
                }

                // Drop caps starts
                bool dropCap = false;
                searchKey = "float";
                if (IdAllClass[className].ContainsKey(searchKey))
                {
                    dropCap = true;
                }
                searchKey = "vertical-align";
                if (IdAllClass[className].ContainsKey(searchKey))
                {
                    if(dropCap)
                    {
                        _dropCap.Add(className);
                    }
                }
                // Drop caps ends


                //searchKey = "counter-reset";
                //if (IdAllClass[className].ContainsKey(searchKey))
                //{
                //    ContentCounterReset[className] = IdAllClass[className][searchKey];
                //}

            //    // Footnote process 
            //    searchKey = "display";
            //    if (IdAllClass[className].ContainsKey(searchKey) && className.IndexOf("..") == -1)
            //    {
            //        if (IdAllClass[className][searchKey] == "footnote" || IdAllClass[className][searchKey] == "prince-footnote")
            //        {
            //            if (!_FootNote.Contains(className))
            //                _FootNote.Add(className);
            //        }
            //    }
            //    string searchKey1 = "..footnote-call";
            //    if (className.IndexOf(searchKey1) >= 0)
            //    {
            //        if (!_footnoteCallContent.Contains(className))
            //            _footnoteCallContent.Add(className);
            //    }
            //    string searchKey2 = "..footnote-marker";
            //    if (className.IndexOf(searchKey2) >= 0)
            //    {
            //        if (!_footnoteMarkerContent.Contains(className))
            //            _footnoteMarkerContent.Add(className);
            //    }
            }
        }

        private void InitializeData(PublicationInformation projInfo, Dictionary<string, Dictionary<string, string>> idAllClass, Dictionary<string, ArrayList> classFamily, ArrayList cssClassOrder)
        {
            _allStyle = new Stack<string>();
            _allParagraph = new Stack<string>();
            _allCharacter = new Stack<string>();
            
            _childStyle = new Dictionary<string, string>();
            IdAllClass = new Dictionary<string, Dictionary<string, string>>();
            ParentClass = new Dictionary<string, string>();
            _newProperty = new Dictionary<string, Dictionary<string, string>>();
            _displayBlock = new Dictionary<string, string>();
            _cssClassOrder = cssClassOrder;
            //_classFamily = new Dictionary<string, ArrayList>();
            _projInfo = projInfo;

            _sourcePicturePath = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath);
            _projectPath = projInfo.TempOutputFolder;

            IdAllClass = idAllClass;
            _classFamily = classFamily;

            _isNewParagraph = false;
            _characterName = "None"; 
        }

        private void AlterFrameWithoutCaption(string contentFilePath)
        {
            OOUtility utility = new OOUtility();
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
        private string PreProcess(string sourceFile, OldStyles structStyles)
        {
            _sourcePicturePath = Path.GetDirectoryName(sourceFile);
            //string newSourceFile = sourceFile;
            ClassContainsText(sourceFile, structStyles);
            //AnchorTagProcessing(sourceFile);
            sourceFile = CheckSourceChanged(sourceFile);
            //ReplaceString(sourceFile, structStyles);
            return sourceFile;
        }

        /// <summary>
        /// To replace the symbol string if the symbol matches with the text
        /// </summary>
        /// <param name="sourceFile">XHTML file path</param>
 private void ReplaceString(string sourceFile)
        {
            if (_replaceSymbolToText.Count > 0)
            {
                var sr = new StreamReader(sourceFile);
                string ss = sr.ReadToEnd();
                sr.Close();
                foreach (string srchKey in _replaceSymbolToText.Keys)
                {
                    if (ss.IndexOf(srchKey) >= 0)
                    {
                        ss = ss.Replace(srchKey, _replaceSymbolToText[srchKey]);
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
                                        _anchor.Add(href.ToLower());
                                    }
                                }
                            }
                        }
                    }
                }
                xDoc.Save(sourceFile);
            }
        }

        /// <summary>
        /// Class Contains method modifies the style Name of the container
        /// </summary>
        /// <param name="sourceFile">The Xhtml File</param>
        /// <param name="structStyles">The Structure of Styles objects</param>
        private void ClassContainsText(string sourceFile, OldStyles structStyles)
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
        private void ProcessXHTML_id(string xhtmlFileWithPath)
        {
            try
            {
                while (_reader.Read())
                {
                    if (IsEmptyNode())
                    {
                        continue;
                    }

                    switch (_reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            //StartElement();
                            break;
                        case XmlNodeType.Text:
                            //Write();
                            break;
                        case XmlNodeType.EndElement:
                            //EndElement();
                            break;
                        case XmlNodeType.SignificantWhitespace:
                            if (_reader.Value.Replace(" ", "") == "")
                            {
                                //Write();
                            }
                            break;
                    }
                }
            }
            catch (XmlException e)
            {
                var msg = new[] { e.Message, xhtmlFileWithPath };
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
                    switch (_reader.NodeType)
                    {

                        case XmlNodeType.Element:
                            StartElement(targetPath);
                            break;
                        case XmlNodeType.EndElement:
                            EndElement();
                            //EndElement(pb);

                            break;
                        case XmlNodeType.Text: // Text.Write
                            //WriteText(_footnoteValue);
                            Write();
                            break;
                        case XmlNodeType.SignificantWhitespace:
                            string data = SignificantSpace(_reader.Value);
                            _writer.WriteString(data);
                            break;

                    }
                    //if (_contentReplace) // TD-204(unable to put tol/pisin)
                    //{
                    //    WriteText(_footnoteValue);
                    //}
                }
                //TimeSpan totalTime = DateTime.Now - startTime;
                //System.Windows.Forms.MessageBox.Show(totalTime.ToString());
            }
            catch (XmlException e)
            {
                var msg = new[] { e.Message, Sourcefile };
                LocDB.Message("errProcessXHTML", Sourcefile + " is Not Valid. " + "\n" + e.Message, msg, LocDB.MessageTypes.Info, LocDB.MessageDefault.First);
                CloseFile(targetPath);
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

        private string SignificantSpace(string content)
        {
            if (content == null) return "";
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
        private void Write()
        {
            //if (isFileCreated == false)
            //{
            //    //CreateFile();
            //    _textFrameClass.Add(_childName);
            //}

            if (_isNewParagraph)
            {
                if (_paragraphName == null)
                {
                    _paragraphName = StackPeek(_allParagraph); // _allParagraph.Pop();
                }

                ClosePara();

//todo extract drop caps
                if (_isDropCap) // forcing new paragraph for drop caps
                {
                    string currentParentStyle = _paragraphName;
                    _writer.WriteStartElement("text:p");
                    int noOfChar = _reader.Value.Length;
                    string currentStyle = _className + noOfChar;
                    ModifyOOStyles oom = new ModifyOOStyles();
                    oom.CreateDropCapStyle(_styleFilePath, _className, currentStyle, currentParentStyle, noOfChar);
                    _writer.WriteAttributeString("text:style-name", currentStyle);
                }
                else
                {
                    // Note: Paragraph Start Element
                    _writer.WriteStartElement("text:p");
                    _writer.WriteAttributeString("text:style-name", _paragraphName); //_divClass
                }
                AddUsedStyleName(_paragraphName);
                _previousParagraphName = _paragraphName;
                _paragraphName = null;
                _isNewParagraph = false;
                _isParagraphClosed = false;
            }
            WriteText();
            isFileEmpty = false;
        }

        private void WriteText()
        {
            string content = _reader.Value;

            //if (CollectFootNoteChapterVerse(content)) return;

            // Psuedo Before
            foreach (ClassInfo psuedoBefore in _psuedoBefore)
            {
                WriteCharacterStyle(psuedoBefore.Content, psuedoBefore.StyleName, true);
            }

            // Text Write
            if (_characterName == null)
            {
                _characterName = StackPeekCharStyle(_allCharacter);
            }
            //content = whiteSpacePre(content);

            if (_psuedoContainsStyle != null)
            {
                if (content.IndexOf(_psuedoContainsStyle.Contains) > -1)
                {
                    content = _psuedoContainsStyle.Content;
                    _characterName = _psuedoContainsStyle.StyleName;
                }
            }
            string modifiedContent = ModifiedContent(content, _previousParagraphName, _characterName);
            WriteCharacterStyle(modifiedContent, _characterName, false);

            _psuedoBefore.Clear();
        }

        private string StackPeekCharStyle(Stack<string> stack)
        {
            string result = "none";
            if (stack.Count > 0)
            {
                result = stack.Peek();
            }
            return result;
        }

        private void whiteSpacePre(string content, bool pseudo)
        {
            string whiteSpacePre = GetPropertyValue(_classNameWithLang, "white-space", string.Empty);
            if (whiteSpacePre == "pre")
            {
                 WhiteSpace(content, _classNameWithLang);
            }
            else
            {
                content = SignificantSpace(content);
                if (pseudo)
                    _writer.WriteRaw(content);
                else if(!VisibleHidden())
                    _writer.WriteString(content);
            }
        }

        private string GetPropertyValue(string clsName, string property, string defaultValue)
        {
            string valueOfProperty = defaultValue;
            if (IdAllClass.ContainsKey(clsName) && IdAllClass[clsName].ContainsKey(property))
            {
                valueOfProperty = IdAllClass[clsName][property];
            }
            return valueOfProperty;
        }

        private void WriteCharacterStyle(string content, string characterStyle, bool pseudo)
        {
            _imageInserted = InsertImage();

            if ((_tagType == "span" || _tagType == "a") && characterStyle != "none") //span start
            {
                _writer.WriteStartElement("text:span");
                _writer.WriteAttributeString("text:style-name", characterStyle); //_util.ChildName
            }
            AddUsedStyleName(characterStyle);

            if (!AnchorBookMark())
            {

                whiteSpacePre(content, pseudo); // TODO -2000 - SignificantSpace() - IN OO convert

            }
            if ((_tagType == "span" || _tagType == "a") && characterStyle != "none")  // span end
            {
                _writer.WriteEndElement();
            }

        }

        private bool VisibleHidden()
        {
            bool hidden = false;
            if (isHiddenText)
            {
                int noOfChar = _reader.Value.Trim().Replace("\r\n", "").Length;
                noOfChar = noOfChar + (noOfChar * 20 / 100);
                _writer.WriteStartElement("text:s");
                _writer.WriteAttributeString("text:c", noOfChar.ToString());
                _writer.WriteEndElement();
                isHiddenText = false;
                hidden = true;
            }
            return hidden;
        }

        private bool AnchorBookMark()
        {
            bool dataWritten = false;
            if (_anchorStart && _anchorIdValue.Length > 0)
            {
                if (_anchorIdValue != null)
                {
                    string anchorIdValue = _anchorIdValue.ToLower();
                    if (_anchor.Contains(anchorIdValue))
                    {
                        _anchorIdValue = anchorIdValue;
                        _anchor.Remove(anchorIdValue);
                    }
                }

                _writer.WriteStartElement("text:reference-mark");
                _writer.WriteAttributeString("text:name", _anchorIdValue);
                _writer.WriteEndElement();
                _anchorIdValue = string.Empty;
            }
            else if (_anchorStart && _anchorBookMarkName != string.Empty)
            {
                string status = _anchorBookMarkName.Substring(0, 4);
                if (status == "href")
                {
                    //_anchorBookMarkName = "endbookmark";
                    if (_anchor.Count > 0)
                    {
                        string data = HardSpace(_classNameWithLang, _reader.Value);
                        string hrefValueWOHash = Common.RightString(_anchorBookMarkName, "#"); // _anchor[_anchor.Count - 1].ToString();
                        _writer.WriteStartElement("text:reference-ref");
                        _writer.WriteAttributeString("text:reference-format", "text");
                        _writer.WriteAttributeString("text:ref-name", hrefValueWOHash.ToLower());
                        _writer.WriteString(data);
                        _writer.WriteEndElement(); // for Anchor Ends
                        _anchorStart = false;
                        _anchorBookMarkName = string.Empty;
                        dataWritten = true;
                    }
 
                }
            }
            return dataWritten;
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

            //if (_structStyles.AllCSSName.Contains(tempClassName))
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
            StartElementBase(_IsHeadword);
            Psuedo();
            VisibilityCheck();
            DropCaps();
        }

        private void DropCaps()
        {
            if (_dropCap.Contains(_className))  // Matches the drop cap class
            {
                _isDropCap = true;
            }
        }

        private void VisibilityCheck()
        {
            //string
            if (_visibilityClassName.Contains(_className))
            {
                isHiddenText = true;
            }
        }

        public override void CreateSectionClass(string readerValue)
        {
            string sectionName = string.Empty;
            readerValue = Common.LeftString(readerValue, "_");
            if (_sectionName.Contains(readerValue))
            {
                sectionName = readerValue;
            }
            else if (_sectionName.Contains(readerValue + "." + _tagType))
            {
                sectionName = readerValue + "." + _tagType;
            }
            if (sectionName.Length > 0)
            {
                sectionName = "Sect_" + sectionName;
                _writer.WriteStartElement("text:section");
                _writer.WriteAttributeString("text:style-name", sectionName);
                _writer.WriteAttributeString("text:name", sectionName);
            }
        }

        private void Psuedo()
        {
            // Psuedo Before
            if (_psuedoBeforeStyle != null)
            {
                _psuedoBefore.Add(_psuedoBeforeStyle);
            }

            // Psuedo After
            if (_psuedoAfterStyle != null)
            {
                _psuedoAfter[_childName] = _psuedoAfterStyle;
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

        private void EndElement()
        {
            _characterName = null;
            _closeChildName = StackPop(_allStyle);

            string sectionName = Common.LeftString(_closeChildName, "_");
            if (_sectionName.Contains(sectionName)) // section close
            {
                _writer.WriteEndElement();
            }
            //Note: verify &todo in OO td2000 
            /*
            SetHeadwordFalse();
            ClosefooterNote();
            EndElementForImage();
            */
            EndElementForImage();

            if (_closeChildName == string.Empty) return;
            if (_psuedoAfter.Count > 0)
            {
                if (_psuedoAfter.ContainsKey(_closeChildName))
                {
                    ClassInfo classInfo = _psuedoAfter[_closeChildName];
                    WriteCharacterStyle(classInfo.Content, classInfo.StyleName, true);
                    _psuedoAfter.Remove(_closeChildName);
                }
            }

            EndElementBase();
            if (_columnClass.Count > 0)
            {
                if (_closeChildName == _columnClass[_columnClass.Count - 1].ToString())
                {
                    _columnClass.RemoveAt(_columnClass.Count - 1);
                    //////////////////////////CloseFile(); ???
                }
            }
            _classNameWithLang = StackPeek(_allStyle);
            _classNameWithLang = Common.LeftString(_classNameWithLang, "_");
        }
       #endregion

        #region Private Methods

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
                    _writer.WriteAttributeString("text:style-name", divClass + j.ToString());

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

 

        /// <summary>
        /// Generate Section block in Content.xml.
        /// </summary>
        private void CreateSection()
        {

            if (_projInfo.ProjectInputType == "Scripture")
                RemoveScrSectionClass(_sectionName);

            foreach (string section in _sectionName)
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

        private void GetHeightandWidth(ref string height, ref string width)
        {
            if (_allCharacter.Count > 0)
            {
                string stackClass = _allCharacter.Peek();
                string[] splitedClassName = stackClass.Split('_');

                string[] allClasses = new string[splitedClassName.Length + 1];
                splitedClassName.CopyTo(allClasses, 1);
                allClasses[0] = _imageSrcClass; // including current Class
                if (allClasses.Length > 0)
                {
                    //for (int i = splitedClassName.Length -1; i >= 0; i--)  // From Begining to Recent Class
                    for (int i = 0; i < allClasses.Length; i++) // // From Recent to Begining Class
                    {
                        string clsName = allClasses[i];
                        string ht = string.Empty;
                        string wd = string.Empty;
                        ht = GetPropertyValue(clsName, "height");
                        wd = GetPropertyValue(clsName, "width");
                        if (ht.Length > 0 || wd.Length > 0)
                        {
                            if (ht.Length > 0)
                                height = ht;

                            if (wd.Length > 0)
                                width = wd;

                            break;
                        }
                    }
                }
            }
        }



        private void GetWrapSide(ref string wrapSide, ref string wrapMode)
        {
            string clearValue = GetPropertyValue(_imageClass, "clear");
            if (clearValue.Length > 0)
            {
                wrapSide = clearValue;
            }
            switch (wrapSide)
            {
                case "left":
                    wrapSide = "RightSide";
                    break;
                case "right":
                    wrapSide = "LeftSide";
                    break;
                case "both":
                    wrapSide = "RightSide";
                    wrapMode = "JumpObjectTextWrap";
                    break;
                case "none":
                    wrapSide = "BothSides";
                    break;
            }
        }

        private void GetAlignment(string alignment, ref string align, ref string alignPosition, string className)
        {
            string clsName = className;
            TextInfo _titleCase = CultureInfo.CurrentCulture.TextInfo;
            string floatValue = GetPropertyValue(clsName, "float");
            if (floatValue.Length > 0)
            {
                alignment = floatValue;
            }
            if (alignment.Length > 0)
            {
                alignment = _titleCase.ToTitleCase(alignment);
                align = align.Replace("Left", alignment);
                alignPosition = alignPosition.Replace("Left", alignment);
            }
        }

        private void GetAlignment(string alignment, ref string wrapSide, ref string HoriAlignment, ref string AnchorPoint, ref string VertRefPoint, ref string VertAlignment, ref string wrapMode)
        {
            if (_allCharacter.Count > 0)
            {
                string stackClass2 = _allStyle.Peek();
                string stackClass1 = _allParagraph.Peek();
                string stackClass = _allCharacter.Peek();
                //string[] splitedClassName = stackClass.Split('_');

                string[] splitedClassName = _allStyle.ToArray();

                if (splitedClassName.Length > 0)
                {
                    //for (int i = splitedClassName.Length -1; i >= 0; i--)  // From Begining to Recent Class
                    for (int i = 0; i < splitedClassName.Length; i++) // // From Recent to Begining Class
                    {
                        string clsName = splitedClassName[i];
                        string pos = GetPropertyValue(clsName, "float", alignment);
                        switch (pos)
                        {
                            case "left":
                                HoriAlignment = "LeftAlign";
                                AnchorPoint = "TopLeftAnchor";
                                VertAlignment = "CenterAlign";
                                VertRefPoint = "LineBaseline";
                                break;
                            case "right":
                                AnchorPoint = "TopRightAnchor";
                                HoriAlignment = "RightAlign";
                                VertAlignment = "CenterAlign";
                                VertRefPoint = "LineBaseline";
                                break;
                            case "top":
                            case "prince-column-top":
                            case "-ps-column-top":
                            case "top-left":
                                AnchorPoint = "TopLeftAnchor";
                                HoriAlignment = "LeftAlign";
                                VertAlignment = "TopAlign";
                                VertRefPoint = "PageMargins";
                                wrapMode = "JumpObjectTextWrap";
                                break;
                            case "top-right":
                                AnchorPoint = "TopRightAnchor";
                                VertAlignment = "TopAlign";
                                HoriAlignment = "RightAlign";
                                VertRefPoint = "PageMargins";
                                wrapMode = "JumpObjectTextWrap";
                                break;
                            case "bottom":
                            case "prince-column-bottom":
                            case "-ps-column-bottom":
                            case "bottom-left":
                                AnchorPoint = "BottomLeftAnchor";
                                VertAlignment = "BottomAlign";
                                HoriAlignment = "LeftAlign";
                                VertRefPoint = "PageMargins";
                                wrapMode = "JumpObjectTextWrap";
                                break;
                            case "bottom-right":
                                AnchorPoint = "BotomRightAnchor";
                                VertAlignment = "BottomAlign";
                                HoriAlignment = "RightAlign";
                                VertRefPoint = "PageMargins";
                                wrapMode = "JumpObjectTextWrap";
                                break;
                        }
                        wrapSide = GetPropertyValue("clear", clsName, wrapSide);
                    }
                }
            }
        }
        public bool InsertImage()
        {
            bool inserted = false;
            if (_imageInsert)
            {
                //1 inch = 72 PostScript points
                string alignment = "left";
                string wrapSide = string.Empty;
                string rectHeight = "0";
                string rectWidth = "0";
                string srcFile;
                string wrapMode = "BoundingBoxTextWrap";
                string HoriAlignment = string.Empty;
                string VertAlignment = "center";
                string VertRefPoint = "LineBaseline";
                string AnchorPoint = "TopLeftAnchor";

                isImage = true;
                inserted = true;
                string[] cc = _allParagraph.ToArray();
                imageClass = cc[1];
                srcFile = _imageSource.ToLower();
                //string fileName = "file:" + Common.GetPictureFromPath(srcFile, "", _sourcePicturePath);
                string fromPath = Common.GetPictureFromPath(srcFile, _metaValue, _sourcePicturePath);
                string fileName = Path.GetFileName(srcFile);

            string normalTargetFile = _projInfo.TempOutputFolder;
            string basePath = normalTargetFile.Substring(0, normalTargetFile.LastIndexOf(Path.DirectorySeparatorChar));
            String toPath = Common.DirectoryPathReplace(basePath + "/Pictures/" + fileName);
            if (File.Exists(fromPath))
            {
                File.Copy(fromPath, toPath, true);

            }
                if (IdAllClass.ContainsKey(srcFile))
                {
                    rectHeight = GetPropertyValue(srcFile, "height", rectHeight);
                    rectWidth = GetPropertyValue(srcFile, "width", rectWidth);
                    GetAlignment(alignment, ref HoriAlignment, ref AnchorPoint, srcFile);
                }
                else
                {
                    GetHeightandWidth(ref rectHeight, ref rectWidth);
                    GetAlignment(alignment, ref wrapSide, ref HoriAlignment, ref AnchorPoint, ref VertRefPoint, ref VertAlignment, ref wrapMode);
                    GetWrapSide(ref wrapSide, ref wrapMode);
                }

                // Setting the Height and Width according css value or the image size
                if (rectHeight != "0")
                {
                    if (rectWidth == "0")
                    {
                        rectWidth = Common.CalcDimension(fromPath, rectHeight, 'W');
                    }
                }
                else if (rectWidth != "0" && rectWidth != "72") // 72 = auto width
                {
                    rectHeight = Common.CalcDimension(fromPath, rectWidth, 'H');
                }
                else
                {
                    //Default value is 72 However the line draws 36pt in X-axis and 36pt in y-axis.
                    rectHeight = "72"; // fixed the width as 1 in;
                    rectWidth = Common.CalcDimension(fromPath, rectHeight, 'W');
                    if (rectHeight == "0")
                    {
                        rectWidth = "72";
                    }
                }

                string strFrameCount = "Graphics" + _frameCount;
                ////TODO Make it function 
                ////To get Image details
                //if (File.Exists(fileName1))
                //{
                //    Image fullimage = Image.FromFile(fileName1);
                //    height = fullimage.Height;
                //    width = fullimage.Width;
                //}

                //if (!_divOpen)  // Forcing a Paragraph Style, if it is not exist
                //{
                //    int counter = _styleStack.Count;
                //    string divTagName = string.Empty;
                //    if (counter > 0)
                //    {
                //        var tempStyle = new string[counter];
                //        _styleStack.CopyTo(tempStyle, 0);
                //        divTagName = counter > 1 ? tempStyle[1] : tempStyle[0];
                //    }
                //    _writer.WriteStartElement("text:p");
                //    //_writer.WriteAttributeString("text:style-name", _util.ParentName);
                //    _writer.WriteAttributeString("text:style-name", cc[0];);

                //    //_divOpen = true;
                //}

                // 1st frame
                _writer.WriteStartElement("draw:frame");
                _writer.WriteAttributeString("draw:style-name", strFrameCount);
                _writer.WriteAttributeString("draw:name", strFrameCount);

                _imageZindexCounter++;
                string imgWUnit = "pt";
                string imgHUnit = "pt";
                string anchorType = string.Empty;
                //if (_imageZindexCounter == 1 || _imageZindexCounter == 4)
                if (_imagePreviousFinished)
                {
                    anchorType = "char";
                    _writer.WriteAttributeString("text:anchor-type", anchorType);
                    _writer.WriteAttributeString("draw:z-index", "2");
                    wrapSide = "both";
                    HoriAlignment = "center";
                }
                else if (HoriAlignment.Length > 0)
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

                _writer.WriteAttributeString("svg:width", rectWidth + imgWUnit);
                _writer.WriteAttributeString("svg:height", rectHeight + imgHUnit);
                //TD-349(width:auto)
                if (_isAutoWidthforCaption)
                {
                    _writer.WriteAttributeString("fo:min-width", rectWidth + imgWUnit);
                }

                //1st textbox
                _writer.WriteStartElement("draw:text-box");
                _writer.WriteAttributeString("fo:min-height", "0in");

                _frameCount++;

                ModifyOOStyles modifyIDStyles = new ModifyOOStyles();
                modifyIDStyles.CreateGraphicsStyle(_styleFilePath, strFrameCount, _util.ParentName, HoriAlignment, wrapSide);

                _writer.WriteStartElement("draw:frame");
                _writer.WriteAttributeString("draw:style-name", strFrameCount);
                _writer.WriteAttributeString("draw:name", strFrameCount);
                _writer.WriteAttributeString("text:anchor-type", "paragraph");
                // _writer.WriteAttributeString("text:anchor-type", anchorType);
                _writer.WriteAttributeString("svg:width", rectWidth + imgWUnit);
                _writer.WriteAttributeString("svg:height", rectHeight + imgHUnit);

                //_writer.WriteAttributeString("style:rel-height", "scale");
                //_writer.WriteAttributeString("style:rel-width", "100%");

                _writer.WriteStartElement("draw:image");
                _writer.WriteAttributeString("xlink:type", "simple");
                _writer.WriteAttributeString("xlink:show", "embed");
                _writer.WriteAttributeString("xlink:actuatet", "onLoad");
                _writer.WriteAttributeString("xlink:href", "Pictures/" + fileName);
                _writer.WriteEndElement();
                string altText = "";
                _writer.WriteStartElement("svg:desc");
                _writer.WriteString(altText);

                _writer.WriteEndElement();
                _writer.WriteEndElement();


                _imageInsert = false;
                _imageSource = string.Empty;
                _isNewParagraph = false;
            }
            return inserted;
        }

        /// <summary>
        /// Closing all the tages for Image
        /// </summary>
        private void EndElementForImage()
        {
            if (_imageInsert && !_imageInserted)
            {
                if (_closeChildName == _imageClass) // Without Caption
                {
                    _allCharacter.Push(_imageClass); // temporarily storing to get width and position
                    _imageInserted = InsertImage();
                    _writer.WriteEndElement(); // for ParagraphStyle
                    _writer.WriteEndElement(); // for Textframe
                    _allCharacter.Pop();    // retrieving it again.
                    isImage = false;
                    imageClass = "";
                    _isParagraphClosed = true;

                }
            }
            else // With Caption
            {
                if (imageClass.Length > 0 && _closeChildName == imageClass)
                {

                    _writer.WriteEndElement();// for ParagraphStyle
                    _writer.WriteEndElement(); // for Textframe

                    isImage = false;
                    imageClass = "";
                    _isParagraphClosed = true;
                }
            }
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


        private void UpdateRelativeInStylesXML()
        {
            ModifyOOStyles modifyIDStyles = new ModifyOOStyles();
            _textVariables = modifyIDStyles.ModifyStylesXML(_projectPath, _newProperty, _usedStyleName, _languageStyleName, "", _IsHeadword, ParentClass);
        }

        /// <summary>
        /// Store used Paragraph adn Character style name. This data used in ModifyIDStyle.cs
        /// </summary>
        /// <param name="styleName">a_b</param>
        private void AddUsedStyleName(string styleName)
        {
            if (!_usedStyleName.Contains(styleName))
            {
                _usedStyleName.Add(styleName);
                AddUsedParentStyleName(styleName);
            }
        }

        /// <summary>
        /// Recursive Parents styles should be added
        /// </summary>
        /// <param name="styleName">current style Name</param>
        private void AddUsedParentStyleName(string styleName)
        {
            if (ParentClass.ContainsKey(styleName))
            {
                string parent = ParentClass[styleName];
                parent = Common.LeftString(parent, "|");
                if (_usedStyleName.Contains(parent))
                {
                    return;
                }
                _usedStyleName.Add(parent);
                AddUsedParentStyleName(parent);
            }
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
        private void CloseFile(string targetPath)
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

            if (_dictColumnGapEm != null && _dictColumnGapEm.Count > 0)
            {
                var xmlDoc = new XmlDocument { PreserveWhitespace = true };
                var nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                nsmgr.AddNamespace("st", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
                nsmgr.AddNamespace("fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");
                xmlDoc.Load(targetFile);
                XmlElement root = xmlDoc.DocumentElement;

                ModifyOOStyles modifyIDStyles = new ModifyOOStyles();
                Dictionary<string, XmlNode> ColumnGap = modifyIDStyles.SetColumnGap(targetFile, _dictColumnGapEm);
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
