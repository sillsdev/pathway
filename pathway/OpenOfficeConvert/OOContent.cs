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
using System.Diagnostics;
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
    public class LOContent : XHTMLProcess
    {
        #region Private Variable
        public OldStyles _oldStyles = new OldStyles();
        string _strBook = string.Empty;
        string _strBook2ndBook = string.Empty;
        private bool _is1stBookFound = false;
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

        string _outputExtension = string.Empty;
        int _autoFootNoteCount;
        private string _sourcePicturePath;

        //Table 
        List<string> _table = new List<string>();
        private int _tableCount = 0;
        private int _tableColumnCount = 0;
        Dictionary<string, int> _tableColumnModify = new Dictionary<string, int>();
        private bool _isTableOpen;
        private bool _hasImgCloseTag;

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
        private bool _pseudoSingleSpace = false;
        //bool _isWhiteSpace;
        bool _isNewLine = true;
        string _prevLang = string.Empty;
        string _parentClass = string.Empty;
        string _parentLang = string.Empty;
        string _projectType = string.Empty;
        readonly Dictionary<string, string> _counterVolantryReset = new Dictionary<string, string>();
        readonly string _tempFile = Common.PathCombine(Path.GetTempPath(), "tempXHTMLFile.xhtml"); //TD-351
        readonly string _hardSpace = Common.ConvertUnicodeToString("\u00A0");
        readonly string _fixedSpace = Common.ConvertUnicodeToString("\u2002");
        readonly string _thinSpace = Common.ConvertUnicodeToString("\u2009");

        ArrayList _anchor = new ArrayList();
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
        private bool _imageParaForCaption = false;
        private List<string> _unUsedParagraphStyle = new List<string>();
        string footCallSymb = string.Empty;
        List<string> LanguageFontStyleName = new List<string>();
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
        private Dictionary<string, ClassInfo> _psuedoAfter = new Dictionary<string, ClassInfo>();
        //private Dictionary<string, ArrayList> _styleName = new Dictionary<string, ArrayList>();
        private ArrayList _crossRef = new ArrayList();
        private int _crossRefCounter = 1;
        private bool _isWhiteSpace = false;
        private bool _isForcedWhiteSpace = false;
        private bool _isVerseNumberContent = false;
        private StringBuilder _verseContent = new StringBuilder();

        private bool _IsHeadword = false;
        private bool _significant;
        private bool _footnoteSpace;
        private bool _isListBegin;
        private Dictionary<string, string> ListType;
        //private string _anchorText = string.Empty;
        private bool _anchorWrite;
        private List<string> _sourceList = new List<string>();
        private List<string> _targetList = new List<string>();

        //private Stack<string> _referenceCloseStyleStack = new Stack<string>();
        private bool _isPictureDisplayNone = false;

        private bool _isEmptyTitleExist;
        private int _titleCounter = 1;
        private int _pageWidth;

        Dictionary<string, string> _pageSize = new Dictionary<string, string>();
        private bool _isFromExe = false;

        List<string> _headwordVariable = new List<string>();
        private int _headwordIndex = 0;
        private bool _isFootnoteCallerStared;
        private string _customFootnoteSymbol = string.Empty;
        private string _customXRefSymbol = string.Empty;
        private string firstRevHeadWord = string.Empty;
        Dictionary<string, string> FirstDataOnEntry = new Dictionary<string, string>();
        #endregion

        #region Public Variable
        public bool _multiLanguageHeader = false;
        public string RefFormat = "Genesis 1";
        public bool IsMirrorPage;
        public string _ChapterNo = "1";
        public bool IsFirstEntry;
        private bool isPageBreak;

        public LOContent()
        {
            _outputType = Common.OutputType.ODT;
        }
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

        public Dictionary<string, ArrayList> CreateStory(PublicationInformation projInfo, Dictionary<string, Dictionary<string, string>> idAllClass,
            Dictionary<string, ArrayList> classFamily, ArrayList cssClassOrder, int pageWidth, Dictionary<string, string> pageSize)
        {
            OldStyles styleInfo = new OldStyles();
            _pageWidth = pageWidth;
            _structStyles = styleInfo;
            _structStyles.IsMacroEnable = true;
            _pageSize = pageSize;
            _isFootnoteCallerStared = true;
            _isFromExe = Common.CheckExecutionPath();
            _customFootnoteSymbol = projInfo.IncludeFootnoteSymbol;
            _customXRefSymbol = projInfo.IncludeXRefSymbol;
            string _inputPath = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath);
            InitializeData(projInfo, idAllClass, classFamily, cssClassOrder);
            ProcessProperty();
            Preprocess();
            OpenXhtmlFile(projInfo.DefaultXhtmlFileWithPath); //reader
            CreateFile(projInfo.TempOutputFolder); //writer
            CreateSection();
            //Preprocess();
            //PreprocessAnchor(projInfo.DefaultXhtmlFileWithPath); // todo in linux
            CreateBody();
            ProcessXHTML(projInfo.ProgressBar, projInfo.DefaultXhtmlFileWithPath, projInfo.TempOutputFolder);
            UpdateRelativeInStylesXML();
            CloseFile(projInfo.TempOutputFolder);
            AlterFrameWithoutCaption(projInfo.TempOutputFolder);
            ModifyContentXML(projInfo.TempOutputFolder);

            return new Dictionary<string, ArrayList>();
        }

        private void Preprocess()
        {
            PreExportProcess preProcessor = new PreExportProcess(_projInfo);
            //preProcessor.ReplaceInvalidTagtoSpan("CmPicture-publishStemPile-ThumbnailPub", "div");
            preProcessor.GetReferenceList(_projInfo.DefaultXhtmlFileWithPath, _sourceList, _targetList);
            //TD-2912
            if (_projInfo.ProjectInputType.ToLower() == "dictionary")
            {
                _headwordVariable = preProcessor.PrepareCurrentNextHeadwordPair();
            }
        }

        private void PreprocessAnchor(string xhtmlFile)
        {
            PreExportProcess preProcessor = new PreExportProcess(_projInfo);
            preProcessor.AnchorTagProcessing(xhtmlFile, ref _anchor);

        }

        private void ProcessProperty()
        {
            Common.ColumnWidth = 0.0;

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

                if (className.IndexOf("Sect_") >= 0)
                {
                    _dictColumnGapEm[className] = IdAllClass[className];
                }
                string colWidth = string.Empty;
                if (className.IndexOf("SectColumnWidth_") >= 0)
                {
                    colWidth = IdAllClass[className]["ColumnWidth"];
                    Common.ColumnWidth = double.Parse(colWidth, CultureInfo.GetCultureInfo("en-US"));
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
                    if (values.Length <= 1)
                        values = IdAllClass[className][searchKey].Split('\'');
                    for (int i = 0; i < values.Length; i++)
                    {
                        if (values[i].Length > 0)
                        {
                            string key = values[i].Replace("\"", "");
                            //key = Common.ReplaceSymbolToText(key);
                            i++;
                            i++;
                            string value = values[i].Replace("\"", "");
                            value = Common.ReplaceSymbolToText(value);
                            _replaceSymbolToText[key] = Common.ConvertUnicodeToString(value);
                        }
                    }
                }

                // avoid white background color for pdf thru libreoffice - TD-1573
                searchKey = "background-color";
                if (IdAllClass[className].ContainsKey(searchKey) && IdAllClass[className][searchKey].ToLower() == "#ffffff")
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
                    if (dropCap)
                    {
                        _dropCap.Add(className);
                    }
                }
                // Drop caps ends


                searchKey = "counter-increment";
                if (IdAllClass[className].ContainsKey(searchKey))
                {
                    var key = new Dictionary<string, string>();
                    string value = IdAllClass[className][searchKey];
                    if (value.IndexOf(',') > 0)
                    {
                        string[] splitcomma = value.Split(',');
                        if (splitcomma.Length > 1)
                        {
                            key[splitcomma[0]] = splitcomma[1];
                            contentCounterIncrement[className] = key;
                            ContentCounter[splitcomma[0]] = 0;
                        }
                    }
                    else
                    {
                        key[value] = "1";
                        contentCounterIncrement[className] = key;
                        ContentCounter[value] = 0;
                    }

                }

                searchKey = "counter-reset";
                if (IdAllClass[className].ContainsKey(searchKey))
                {
                    ContentCounterReset[className] = IdAllClass[className][searchKey];
                }

                // Footnote process 
                searchKey = "display";
                if (IdAllClass[className].ContainsKey(searchKey) && className.IndexOf("..footnote") > 0)
                {
                    string footnoteClsName = Common.LeftString(className, "..");
                    if (IdAllClass[footnoteClsName][searchKey] == "footnote" || IdAllClass[footnoteClsName][searchKey] == "prince-footnote")
                    {
                        if (!_FootNote.Contains(footnoteClsName))
                            _FootNote.Add(footnoteClsName);
                    }
                }
                searchKey = "..footnote-call";
                if (className.IndexOf(searchKey) >= 0)
                {
                    if (!_footnoteCallContent.Contains(className))
                        _footnoteCallContent.Add(className);
                }
                searchKey = "..footnote-marker";
                if (className.IndexOf(searchKey) >= 0)
                {
                    if (!_footnoteMarkerContent.Contains(className))
                    {
                        _footnoteMarkerContent.Add(className);
                        if (IdAllClass[className].ContainsKey("content"))
                        {
                            _footNoteMarker[className] = IdAllClass[className]["content"];
                        }
                    }

                }

                //searchKey = "list-style-type";
                //if (IdAllClass[className].ContainsKey(searchKey))
                //{
                //    ListType[className] = IdAllClass[className][searchKey];
                //}


            }

            if (Common.ColumnWidth == 0.0)
            {
                Common.ColumnWidth = _pageWidth;
            }
        }

        private void InitializeData(PublicationInformation projInfo, Dictionary<string, Dictionary<string, string>> idAllClass, Dictionary<string, ArrayList> classFamily, ArrayList cssClassOrder)
        {
            _outputExtension = projInfo.OutputExtension;
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
        /// <param name="data">XML Content</param>
        private string ReplaceString(string data)
        {
            //data = Common.ReplaceSymbolToText(data);
            if (_replaceSymbolToText.Count > 0)
            {
                if (data.IndexOf("<<<") >= 0)
                {
                    string s1 = Common.ConvertUnicodeToString("\\201C") + Common.ConvertUnicodeToString("\\2018");
                    data = data.Replace("<<<", s1);
                }
                if (data.IndexOf(">>>") >= 0)
                {
                    string s2 = Common.ConvertUnicodeToString("\\2019") + Common.ConvertUnicodeToString("\\201D");
                    data = data.Replace(">>>", s2);
                }
                foreach (string srchKey in _replaceSymbolToText.Keys)
                {
                    if (data.IndexOf(srchKey) >= 0)
                    {
                        data = data.Replace(srchKey, _replaceSymbolToText[srchKey]);
                    }
                }
            }
            return data;
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
                _xmldoc = Common.DeclareXMLDocument(false);
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
            if (_outputExtension == "odm") return;

            if (pb != null && pb.Maximum == 0)
            {
                progressBarError = true;
            }

            _styleFilePath = targetPath + "styles.xml";
            _columnWidth = _structStyles.ColumnWidth;
            //DateTime startTime = DateTime.Now;
            try
            {
                _reader = Common.DeclareXmlTextReader(Sourcefile, true);
                //CreateBody();
                bool headXML = true;
                while (_reader.Read())
                {
                    if (headXML) // skip previous parts of <body> tag
                    {
                        if (_reader.Name == "body")
                        {
                            headXML = false;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    if (_reader.Name == "img")
                    {
                        _hasImgCloseTag = true;
                    }
                    // _hasImgCloseTag = _imageInsert; 
                    if (_reader.IsEmptyElement)
                    {
                        if (_reader.Name == "img")
                        {
                            _hasImgCloseTag = false;
                        }
                        else if (_reader.Name == "br")
                        {
                            _writer.WriteRaw(@"<text:line-break/>");
                            continue;
                        }
                        else
                        {
                            if (_reader.Name == "a")
                            {
                                continue;
                            }
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
                            InsertEmptySpanForPicture();
                            StartElement(targetPath);
                            break;
                        case XmlNodeType.EndElement:
                            EndElement();
                            break;
                        case XmlNodeType.Text: // Text.Write
                            Write();
                            break;
                        case XmlNodeType.SignificantWhitespace:
                            InsertWhiteSpace();
                            break;
                        case XmlNodeType.EntityReference:
                            IncludeWhiteSpace();
                            break;

                    }
                }

                InsertFlexRevFirstGuidewordOnMainForOdm();

                //TimeSpan totalTime = DateTime.Now - startTime;
                //System.Windows.Forms.MessageBox.Show(totalTime.ToString());
            }
            catch (XmlException e)
            {
                var msg = new[] { e.Message, Sourcefile };
                if (LocDB.DB != null)
                {
                    LocDB.Message("errProcessXHTML", Sourcefile + " is Not Valid. " + "\n" + e.Message, msg,
                                  LocDB.MessageTypes.Info, LocDB.MessageDefault.First);
                }
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

        /// <summary>
        /// Insert Empty Span if Picture comes first for a LetHead in Dictionary
        /// </summary>
        private void InsertEmptySpanForPicture()
        {
            if (_childName.ToLower().IndexOf("pictureright_entry") == 0 && _previousParagraphName == "letter_letHead_dicBody")
            {
                _writer.WriteStartElement("text:p");
                _writer.WriteAttributeString("text:style-name", "entry_letData_dicBody");
                _writer.WriteStartElement("text:span");
                _writer.WriteAttributeString("text:style-name", "span_entry_letData_dicBody");
                _writer.WriteString(" ");
                _writer.WriteEndElement();
                _writer.WriteEndElement();
            }
        }

        /// <summary>
        /// Insert Reversal first guideword on end of the Main document for TD-3626
        /// </summary>
        private void InsertFlexRevFirstGuidewordOnMainForOdm()
        {
            if (_projInfo.DefaultXhtmlFileWithPath.ToLower().IndexOf("flexrev") <= 0 && _projInfo.IsODM)
            {
                //MessageBox.Show(_projInfo.DefaultXhtmlFileWithPath);
                firstRevHeadWord = ReadXHTMLFirstData(_projInfo.DefaultXhtmlFileWithPath.Replace("Preservemain", "FlexRev"));
                if (firstRevHeadWord.Trim().Length > 0)
                {
                    _writer.WriteStartElement("text:p");
                    _writer.WriteAttributeString("text:style-name", "hideDiv_dicBody");
                    _writer.WriteStartElement("text:variable-set");
                    _writer.WriteAttributeString("text:name", "Left_Guideword_L");
                    _writer.WriteAttributeString("text:display", "none");
                    _writer.WriteAttributeString("text:formula", "ooow: " + firstRevHeadWord);
                    _writer.WriteAttributeString("office:value-type", "string");
                    _writer.WriteAttributeString("office:string-value", firstRevHeadWord);
                    _writer.WriteEndElement();
                    _writer.WriteEndElement();
                }
            }
        }

        private void IncludeWhiteSpace()
        {
            if (_reader.Value != "")
            {
                return;
            }

            bool whiteSpaceExist = _significant;
            string data = SignificantSpace(_reader.Value);
            //_writer.WriteString(data);
            if (!whiteSpaceExist && !_pseudoSingleSpace)
            {
                //_writer.WriteStartElement("text:s");
                //_writer.WriteAttributeString("text:c", "1");
                //_writer.WriteString(" ");
                //_writer.WriteEndElement();
                _significant = true;
                _writer.WriteString(" ");
            }
        }

        private void InsertWhiteSpace()
        {
            bool whiteSpaceExist = _significant;
            string data = SignificantSpace(_reader.Value);
            //_writer.WriteString(data);
            if (!whiteSpaceExist && !_pseudoSingleSpace)
            {
                //if (_isNewParagraph)
                //{
                //    _footnoteSpace = false;  &&_significant == false
                //}
                if (_footnoteSpace == false && _projInfo.ProjectInputType.ToLower() == "dictionary")
                {
                    _writer.WriteStartElement("text:s");
                    _writer.WriteAttributeString("text:c", "1");
                    _writer.WriteString(" ");
                    _writer.WriteEndElement();
                    _significant = true;
                }
                //_writer.WriteString(" ");
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
            //if (charac.Length == 1)
            //{
            //    return content;
            //}
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
            //if (_classNameWithLang.IndexOf("scrFootnoteMarker") == 0 || _classNameWithLang.IndexOf("VerseNumber") == 0)
            if (_classNameWithLang.IndexOf("scrFootnoteMarker") == 0)
            {
                _significant = true;
            }
            else if (_classNameWithLang.IndexOf("VerseNumber") == 0)
            {
                _significant = true;
                _footnoteSpace = true;
            }


            return content;
            //_writer.WriteString(content);
        }
        private void Write()
        {

            //if (_reader.Value == "Filemón")
            //{
            //    //string content = _strBook + " 1";
            //    string content = "Filemón 1";
            //    _writer.WriteStartElement("text:p");
            //    _writer.WriteStartElement("text:variable-set");
            //    _writer.WriteAttributeString("text:name", "Left_Guideword_L");
            //    _writer.WriteAttributeString("text:display", "none");
            //    _writer.WriteAttributeString("text:formula", "ooow:" + content);
            //    _writer.WriteAttributeString("office:value-type", "string");
            //    _writer.WriteAttributeString("office:string-value", content);
            //    _writer.WriteEndElement();
            //    _writer.WriteEndElement();
            //}

            if (_projInfo.DefaultXhtmlFileWithPath.ToLower().IndexOf("flexrev") > 0 && !_projInfo.IsODM && _childName.ToLower() == "hidediv_dicbody" && !_projInfo.IsFrontMatterEnabled)
            {
                return;
            }

            //if (!_isWhiteSpace && (_characterName != null) && _characterName.IndexOf("xhomographnumber") != 0)
            //{
            //    InsertWhiteSpace();
            //}
            if (_childName.ToLower().Contains("tableofcontents"))
            {
                CallTOC();
                //_writer.WriteStartElement("text:p");
                //_writer.WriteAttributeString("text:style-name", "P4");
                //_writer.WriteEndElement();
                return;
            }

            if (_isDisplayNone)
            {
                CollectFootNoteChapterVerse(ReplaceString(_reader.Value), Common.OutputType.ODT.ToString());
                return; // skip the node
            }

            if (_isNewParagraph)
            {
                _footnoteSpace = false;
                if (_paragraphName == null)
                {
                    _paragraphName = StackPeek(_allParagraph); // _allParagraph.Pop();
                }

                ClosePara(false);
                WriteTable();

                if (_isDropCap)
                {
                    DropCapsParagraph();
                }
                else
                {
                    //I comment this feature, we can use this feature, when we need again.(TD-3187, 3226)
                    ////Insert Fixed Height Hidden Paragraph for TD-2912
                    //if (_projInfo.ProjectInputType.ToLower() == "dictionary")
                    //{
                    //    if (_previousParagraphName != null && _previousParagraphName.IndexOf("entry") == 0 &&
                    //        (_childName.IndexOf("letHead") == -1 && _childName.IndexOf("pictureCaption") == -1))
                    //    {
                    //        //<text:p text:style-name="block_5f_p">
                    //        //    <text:soft-page-break/>
                    //        //    <text:span text:style-name="headword_5f_entry_5f_letData_5f_dicBody"
                    //        //        >abaá</text:span>
                    //        //</text:p> 
                    //        //Insert Fixed Height Hidden Paragraph for TD-2912
                    //        //if (_childName.IndexOf("letHead") == -1)
                    //        //{

                    //        if (_headwordIndex < _headwordVariable.Count)
                    //        {
                    //            _writer.WriteStartElement("text:p");
                    //            _writer.WriteAttributeString("text:style-name", "block_5f_p");
                    //            _writer.WriteStartElement("text:soft-page-break");
                    //            _writer.WriteEndElement();
                    //            _writer.WriteStartElement("text:span");
                    //            _writer.WriteAttributeString("text:style-name", "headword_5f_entry_5f_letData_5f_dicBody");

                    //            _writer.WriteValue(_headwordVariable[_headwordIndex]); // + 1
                    //            _writer.WriteEndElement();

                    //            _writer.WriteEndElement();
                    //        }
                    //        // }
                    //    }
                    //}


                    // Note: Paragraph Start Element
                   


                        _writer.WriteStartElement("text:p");
                        _writer.WriteAttributeString("text:style-name", _paragraphName); //_divClass
                        isPageBreak = false;
                        if (_paragraphName.IndexOf("bookPageBreak_") == 0)
                        {
                            isPageBreak = true;
                        }

                    //                <text:variable-set text:name="Left_Guideword_L"
                    //text:display="none" text:formula="ooow:Filemón 1" office:value-type="string"
                    //office:string-value="Filemón 1"/>

                    if (_imageInserted)
                        _imageParaForCaption = true;
                }
                AddUsedStyleName(_paragraphName);
                _previousParagraphName = _paragraphName;
                _paragraphName = null;
                _isNewParagraph = false;
                _isParagraphClosed = false;
                _textWritten = false;
            }
            WriteText();
            isFileEmpty = false;
        }

        private void DropCapsParagraph()
        {
            string currentParentStyle = _paragraphName;
            _writer.WriteStartElement("text:p");
            int noOfChar = _reader.Value.Length;
            string currentStyle = _className + noOfChar;
            ModifyLOStyles oom = new ModifyLOStyles();
            oom.CreateDropCapStyle(_styleFilePath, _className, currentStyle, currentParentStyle, noOfChar);
            _writer.WriteAttributeString("text:style-name", currentStyle);
        }

        private void WriteText()
        {
            string content = _reader.Value;

            content = ReplaceString(content);
            if (CollectFootNoteChapterVerse(content, Common.OutputType.ODT.ToString())) return;
            if (_isPictureDisplayNone)
            {
                return;
            }
            content = InsertSpaceInTextforMacro(content); //TD-2034
            InsertBookNameBeforeBookIntroduction(content);

            //TD-2448
            //if (_projInfo.ProjectInputType.ToLower() == "dictionary")
            //{
            //WriteGuidewordValueToVariable(content);
            //}

            // Psuedo Before
            //foreach (ClassInfo psuedoBefore in _psuedoBefore)
            //{
            //    WriteCharacterStyle(psuedoBefore.Content, psuedoBefore.StyleName, true);
            //}

            // Text Write
            if (_characterName == null)
            {
                _characterName = StackPeekCharStyle(_allCharacter);
            }
            //content = whiteSpacePre(content);
            //bool contains = false;
            //if (_psuedoContainsStyle != null)
            //{
            //    if (content.IndexOf(_psuedoContainsStyle.Contains) > -1)
            //    {
            //        content = _psuedoContainsStyle.Content;
            //        _characterName = _psuedoContainsStyle.StyleName;
            //        contains = true;
            //    }
            //}

            // Ignore display : "none"; 
            if (IdAllClass.ContainsKey(_characterName) && IdAllClass[_characterName].ContainsKey("display"))
            {
                if (IdAllClass[_characterName]["display"] == "none")
                {
                    if (!_significant)
                    {
                        //_writer.WriteStartElement("text:s");
                        //_writer.WriteAttributeString("text:c", "1");
                        //_writer.WriteString(" ");
                        //_writer.WriteEndElement();
                        _writer.WriteString(" ");
                        _significant = true;
                    }
                }
            }

            string modifiedContent = ModifiedContent(content, _previousParagraphName, _characterName);
            //WriteCharacterStyle(modifiedContent, _characterName, contains);
            WriteCharacterStyle(modifiedContent, _characterName);
            if (_isDropCap) // until the next paragraph
            {
                _isDropCap = false;
            }
            _psuedoBefore.Clear();

            WriteGuidewordValueToVariable(content);
        }



        /// <summary>
        /// For TD-2034
        /// Macro need the space between two different cases
        /// 1. BookName and BookCode
        /// 2. ChapterNumber and VerseNumber
        /// So, Method to insert the space before the text, if already exists removed and add again.
        /// </summary>
        /// <param name="content">Input text to be need to add space before</param>
        /// <returns>Text with space based on condition</returns>
        private string InsertSpaceInTextforMacro(string content)
        {
            if (_allCharacter.Count > 0)
            {
                //if (_allCharacter.Peek().IndexOf("scrBookName") == 0 || _allCharacter.Peek().IndexOf("ChapterNumber") == 0)
                //{
                //    content = content.TrimEnd() + " ";
                //}
                if ((_allCharacter.Peek().IndexOf("scrBookCode") == 0 && RefFormat.ToLower().IndexOf("gen 1") == 0) || (_allCharacter.Peek().IndexOf("scrBookName") == 0 && RefFormat.ToLower().IndexOf("genesis 1") == 0))
                //if ((_allCharacter.Peek().IndexOf("scrBookCode") == 0 && RefFormat.ToLower().IndexOf("gen") == 0) || (_allCharacter.Peek().IndexOf("scrBookName") == 0 && RefFormat.ToLower().IndexOf("gen") == 0))
                {
                    //_strBook = content;
                    content = content.TrimEnd() + " ";
                    if (_strBook.Length > 0)
                    {
                        _strBook2ndBook = content;
                    }
                    _strBook = content;
                    //else
                    //{
                    //    _strBook = content;
                    //}
                }
                else if (_allCharacter.Peek().IndexOf("span_TitleMain") == 0 && RefFormat.ToLower().IndexOf("genesis 1") == 0 && _strBook.Trim().Length == 0)
                {
                    //_strBook = content;
                    content = content.TrimEnd() + " ";
                    if (_strBook.Length > 0 && _is1stBookFound)
                    {
                        _strBook2ndBook = content;
                        _is1stBookFound = false;
                    }
                    else
                    {
                        _strBook = content;
                        _is1stBookFound = true;
                    }
                }
                else if (_allCharacter.Peek().ToLower().IndexOf("versenumber") == 0 || _allCharacter.Peek().ToLower().IndexOf("versenumber1") == 0)
                {
                    content = " " + content.TrimStart();
                }
            }
            return content;
        }


        protected override string StackPeekCharStyle(Stack<string> stack)
        {
            string result = "none";
            if (stack.Count > 0)
            {
                result = stack.Peek();
            }
            return result;
        }

        private void whiteSpacePre(string content)
        {
            //string whiteSpacePre = GetPropertyValue(_classNameWithLang, "white-space", string.Empty);
            string whiteSpacePre = GetPropertyValue(_classNameWithLang, "white-space", content);
            if (whiteSpacePre == "pre")
            {
                WhiteSpace(content, _classNameWithLang);
            }
            else
            {
                if (_isVerseNumberContent == false && _verseContent.Length > 0)
                {
                    _writer.WriteRaw(_verseContent.ToString());
                    _verseContent.Remove(0, _verseContent.Length);
                }

                if (_classNameWithLang.IndexOf("reference") == 0)
                {
                    if (content.IndexOf(" ") >= 0)
                    {
                        //_writer.WriteStartElement("text:s");
                        //_writer.WriteAttributeString("text:c", "1");
                        //_writer.WriteString(" ");
                        //_writer.WriteEndElement();
                        _writer.WriteString(" ");
                        content.Replace(" ", "");
                    }
                }

                content = SignificantSpace(content);
                if (_imageClass.Length > 0)
                {
                    //if (!_imageParaForCaption)
                    //{
                    //    _writer.WriteStartElement("text:p");
                    //    _writer.WriteAttributeString("text:style-name", "paraStyle");
                    //}
                    _writer.WriteString(content);
                    //if (!_imageParaForCaption)
                    //{
                    //    _writer.WriteEndElement();
                    //}
                }
                //else if (pseudo)
                //{
                //    if (content == " " && _pseudoSingleSpace == false && _isWhiteSpace == false)
                //    {
                //        _writer.WriteStartElement("text:s");
                //        _writer.WriteAttributeString("text:c", "1");
                //        _writer.WriteEndElement();
                //        _pseudoSingleSpace = true;
                //        _isWhiteSpace = true;
                //    }
                //    else if (content.Trim().Length > 0)
                //    {
                //        _writer.WriteRaw(content);
                //    }
                //}
                else if (_isVerseNumberContent)
                {
                    _verseContent.Append(content);
                }
                else if (!VisibleHidden())
                {
                    _pseudoSingleSpace = false;
                    _isWhiteSpace = false;

                    content = content.Replace(" // ", @"text:line-break/");
                    if (content.IndexOf(@"text:line-break/") >= 0)
                    {
                        _writer.WriteRaw(content.Replace(@"text:line-break/", @"<text:line-break/>"));
                    }
                    else
                    {
                        content = WriteTitleLogo(content);
                        _writer.WriteString(content);
                        WriteLeftGuidewordForLetter(content);
                    }

                    if (content.LastIndexOf(" ") == content.Length - 1)
                    {
                        _pseudoSingleSpace = true;
                    }
                }
            }
        }

        private string WriteTitleLogo(string content)
        {
            if (_classNameWithLang.IndexOf("logo") == 0)
            {

                Param.LoadSettings();
                string organization;
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
                string logoName = String.Empty;
                if (organization.StartsWith("SIL"))
                {
                    logoName = _projInfo.ProjectInputType.ToLower() == "dictionary"
                                   ? "sil-bw-logo.jpg"
                                   : "WBT_H_RGB_red.png";
                }
                else if (organization.StartsWith("Wycliffe"))
                {
                    logoName = "WBT_H_RGB_red.png";
                }

                string height = "19.575pt";
                string width = "67.5pt";
                if (logoName.ToLower().StartsWith("sil"))
                {
                    height = "50pt";
                    width = "50pt";
                }

                string logoFromPath = Param.GetMetadataValue(Param.CopyrightPageFilename, organization);
                logoFromPath = Common.PathCombine(Path.GetDirectoryName(logoFromPath), logoName);
                string normalTargetFile = _projInfo.TempOutputFolder;
                string basePath = normalTargetFile.Substring(0,
                                                             normalTargetFile.LastIndexOf(Path.DirectorySeparatorChar));
                String logoToPath = Common.DirectoryPathReplace(basePath + "/Pictures/" + logoName);
                if (File.Exists(logoFromPath))
                {
                    File.Copy(logoFromPath, logoToPath, true);
                }

                //_writer.WriteStartElement("text:p");
                //_writer.WriteAttributeString("text:style-name", "logo");
                _writer.WriteStartElement("draw:frame");
                _writer.WriteAttributeString("draw:style-name", "GraphicsI1");
                _writer.WriteAttributeString("draw:name", "Graphics1");
                _writer.WriteAttributeString("text:anchor-type", "paragraph");
                _writer.WriteAttributeString("draw:z-index", "1");
                _writer.WriteAttributeString("svg:width", "2.3063in");
                _writer.WriteStartElement("draw:text-box");
                _writer.WriteAttributeString("fo:min-height", "1in");
                _writer.WriteStartElement("text:p");
                _writer.WriteAttributeString("text:style-name", "publisher");
                _writer.WriteString(Param.GetMetadataCurrentValue(Param.Publisher));
                _writer.WriteEndElement();
                _writer.WriteStartElement("text:p");
                _writer.WriteAttributeString("text:style-name", "Illustration");
                _writer.WriteStartElement("draw:frame");
                _writer.WriteAttributeString("draw:style-name", "GraphicsI2");
                _writer.WriteAttributeString("draw:name", "Graphics1");
                _writer.WriteAttributeString("text:anchor-type", "paragraph");
                _writer.WriteAttributeString("svg:height", height);
                _writer.WriteAttributeString("svg:width", width);
                _writer.WriteStartElement("draw:image");
                _writer.WriteAttributeString("xlink:type", "simple");
                _writer.WriteAttributeString("xlink:show", "embed");
                _writer.WriteAttributeString("xlink:actuate", "onLoad");
                _writer.WriteAttributeString("xlink:href", "Pictures/" + logoName);
                _writer.WriteEndElement();
                _writer.WriteStartElement("svg:title");
                _writer.WriteString(logoName);
                _writer.WriteEndElement();
                _writer.WriteEndElement();
                _writer.WriteEndElement();
                _writer.WriteEndElement();
                _writer.WriteEndElement();
                //_writer.WriteEndElement();
                content = string.Empty;
            }
            return content;
        }

        /// <summary>
        /// Insert the left guideword on each letter
        /// </summary>
        /// <param name="content"></param>
        private void WriteLeftGuidewordForLetter(string content)
        {
            if (_classNameWithLang.IndexOf("letter") == 0)
            {
                if (FirstDataOnEntry.ContainsKey(content))
                {
                    _writer.WriteStartElement("text:variable-set");
                    _writer.WriteAttributeString("text:name", "Left_Guideword_L");
                    _writer.WriteAttributeString("text:display", "none");
                    _writer.WriteAttributeString("text:formula", "ooow: " + FirstDataOnEntry[content]);
                    _writer.WriteAttributeString("office:value-type", "string");
                    _writer.WriteAttributeString("office:string-value", FirstDataOnEntry[content]);
                    _writer.WriteEndElement();
                }
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

        //private void WriteCharacterStyle(string content, string characterStyle, bool pseudo)
        private void WriteCharacterStyle(string content, string characterStyle)
        {
            _isVerseNumberContent = characterStyle.ToLower().IndexOf("versenumber") == 0;

            //_imageInserted = InsertImage();
            if (_imageClass.Length > 0)
            {
                _imageTextAvailable = true;
            }
            //if ((_tagType == "span" || _tagType == "a" || pseudo) && characterStyle != "none" || (_tagType == "img" && _imageInserted)) //span start
            if ((_tagType == "span" || _tagType == "a") && characterStyle != "none" || (_tagType == "img" && _imageInserted)) //span start
            {
                if (isFootnote)
                {
                    string footerClassName = Common.LeftString(characterStyle, "_");
                    StringBuilder footnoteStyle = MakeFootnoteStyle(characterStyle);
                    WriteFootNoteMarker(footerClassName, footnoteStyle.ToString(), "");
                    return;
                }

                if (_imageClass.Length > 0 && _imageClass.IndexOf("cover") != 0)
                {
                    ////if (!_isNewParagraph && !_forcedPara)
                    //bool a = _isNewParagraph;
                    //bool b = _isParagraphClosed;
                    //if (_isParagraphClosed && !_forcedPara)
                    //{
                    //    _writer.WriteStartElement("text:p");
                    //    _writer.WriteAttributeString("text:style-name", "ForcedDiv");
                    //    _util.CreateStyleHyphenate(_styleFilePath, "ForcedDiv");
                    //    _forcedPara = true;
                    //    _imageParaForCaption = true;
                    //}
                }
                //Todo force para if span comes without para
                //else if (!_isNewParagraph)
                //{
                //    //_isNewParagraph = false;
                //    if (_paragraphName == null)
                //    {
                //        _paragraphName = StackPeek(_allParagraph); // _allParagraph.Pop();
                //    }
                //    _writer.WriteStartElement("text:p");
                //    _writer.WriteAttributeString("text:style-name", _paragraphName); //_divClass
                //}
                if (_isVerseNumberContent == false)
                {
                    _writer.WriteStartElement("text:span");
                    _writer.WriteAttributeString("text:style-name", characterStyle); //_util.ChildName
                }
                else
                {
                    _verseContent.Append(" <text:span text:style-name=\"" + characterStyle + "\">");
                    if (_projInfo.HideSpaceVerseNumber.ToLower() == "false")
                    {
                        content = content.Replace("-", "‑") + _fixedSpace;
                    }
                    else
                    {
                        content = content.Replace("-", "‑") + _thinSpace;
                    }
                }
            }

            AddUsedStyleName(characterStyle);

            //if (!AnchorBookMark())
            //{
            //    content = WriteCounter(content);
            //    whiteSpacePre(content, pseudo); // TODO -2000 - SignificantSpace() - IN OO convert
            //}

            //if (_anchorStart)
            //    _anchorText = content;

            //bool isAnchorTagOpen = AnchorBookMark(pseudo);
            bool isAnchorTagOpen = AnchorBookMark();
            content = WriteCounter(content);
            whiteSpacePre(content); // TODO -2000 - SignificantSpace() - IN OO convert
            LanguageFontCheck(content, _childName);

            if (isAnchorTagOpen)
            {
                _writer.WriteEndElement();
            }


            //if ((_tagType == "span" || _tagType == "a" || pseudo) && characterStyle != "none" || (_tagType == "img" && _imageInserted))  // span end
            if ((_tagType == "span" || _tagType == "a") && characterStyle != "none" || (_tagType == "img" && _imageInserted))  // span end
            {
                if (_isVerseNumberContent == false)
                {
                    _writer.WriteEndElement();
                }
                else
                {
                    _verseContent.Append("</text:span>");
                    _isVerseNumberContent = false;
                }

            }
            if (_imageClass.Length <= 0)
                _textWritten = true;

        }

        //private bool AnchorBookMark(bool pseudo)
        private bool AnchorBookMark()
        {
            //if (pseudo) return false;

            bool isAnchorTagOpen = false;

            if (_anchorWrite)
            {
                string status = _anchorBookMarkName.Substring(0, 4);

                if (status == "href")
                {
                    //string hrefValueWOHash = Common.RightString(_anchorBookMarkName, "#"); // _anchor[_anchor.Count - 1].ToString();
                    string anchorName = _anchorBookMarkName.Replace("href", "");
                    _writer.WriteStartElement("text:a");
                    _writer.WriteAttributeString("xlink:type", "simple");
                    _writer.WriteAttributeString("xlink:href", anchorName.ToLower());
                    //<text:a xlink:type="simple" xlink:href="#hvo9749">
                    //_writer.WriteString(data);
                    //_writer.WriteEndElement(); // for Anchor Ends
                    //StackPop(_referenceCloseStyleStack);
                    isAnchorTagOpen = true;
                }
                else if (status == "name")
                {
                    string anchorName = _anchorBookMarkName.Replace("name", "");

                    _writer.WriteStartElement("text:bookmark-start");
                    _writer.WriteAttributeString("text:name", anchorName.ToLower());
                    _writer.WriteEndElement();
                    _writer.WriteStartElement("text:bookmark-end");
                    _writer.WriteAttributeString("text:name", anchorName.ToLower());
                    _writer.WriteEndElement();
                    //<text:bookmark-end text:name="hvo9749"/>
                    //<text:bookmark-start text:name="hvo9749"/>
                    //StackPop(_referenceCloseStyleStack);
                    //_referenceCloseStyle = string.Empty;
                }

                _anchorBookMarkName = string.Empty;
                _anchorIdValue = string.Empty;
                _anchor.Clear();
                _anchorWrite = false;
            }
            else if (_anchorIdValue.Length > 0 && _sourceList.Contains(_anchorIdValue.Replace("#", "").ToLower()) && _targetList.Contains(_anchorIdValue.Replace("#", "").ToLower())) //search in source for writing target
            {
                _writer.WriteStartElement("text:bookmark-start");
                _writer.WriteAttributeString("text:name", _anchorIdValue.ToLower());
                _writer.WriteEndElement();
                _writer.WriteStartElement("text:bookmark-end");
                _writer.WriteAttributeString("text:name", _anchorIdValue.ToLower());
                _writer.WriteEndElement();
                _anchorIdValue = string.Empty;
                //StackPop(_referenceCloseStyleStack);
                //_referenceCloseStyle = string.Empty;
            }



            return isAnchorTagOpen;
        }

        private bool AnchorBookMark_old(bool pseudo)
        {
            if (pseudo) return false;

            bool isAnchorTagOpen = false;

            if (_anchorWrite)
            {
                string status = _anchorBookMarkName.Substring(0, 4);

                if (status == "href")
                {
                    //string hrefValueWOHash = Common.RightString(_anchorBookMarkName, "#"); // _anchor[_anchor.Count - 1].ToString();
                    string anchorName = _anchorBookMarkName.Replace("href#", "");
                    _writer.WriteStartElement("text:reference-ref");
                    _writer.WriteAttributeString("text:reference-format", "text");
                    _writer.WriteAttributeString("text:ref-name", anchorName.ToLower());
                    //_writer.WriteString(data);
                    //_writer.WriteEndElement(); // for Anchor Ends
                    //StackPop(_referenceCloseStyleStack);
                    isAnchorTagOpen = true;
                }
                else if (status == "name")
                {
                    string anchorName = _anchorBookMarkName.Replace("name", "");
                    _writer.WriteStartElement("text:reference-mark");
                    _writer.WriteAttributeString("text:name", anchorName.ToLower());
                    _writer.WriteEndElement();
                    //StackPop(_referenceCloseStyleStack);
                    //_referenceCloseStyle = string.Empty;
                }

                _anchorBookMarkName = string.Empty;
                _anchorIdValue = string.Empty;
                _anchor.Clear();
                _anchorWrite = false;
            }
            else if (_anchorIdValue.Length > 0 && _sourceList.Contains(_anchorIdValue.Replace("#", "").ToLower()) &&
                _targetList.Contains(_anchorIdValue.Replace("#", "").ToLower())) //search in source for writing target
            {
                _writer.WriteStartElement("text:reference-mark");
                _writer.WriteAttributeString("text:name", _anchorIdValue.ToLower());
                _writer.WriteEndElement();
                _anchorIdValue = string.Empty;
                //StackPop(_referenceCloseStyleStack);
                //_referenceCloseStyle = string.Empty;
            }



            return isAnchorTagOpen;
        }


        private StringBuilder MakeFootnoteStyle(string characterStyle)
        {
            StringBuilder footnoteStyle = new StringBuilder();
            //footnoteStyle.Append("<text:span ");
            //footnoteStyle.Append("text:style-name=\"" + characterStyle + "\">" + footnoteContent);
            //footnoteStyle.Append("</text:span>");
            footnoteStyle.Append(footnoteContent);
            return footnoteStyle;
        }

        private void WriteFootNoteMarker(string footerClassName, string content, string marker)
        {
            if (footCallSymb.Length == 0)
            {
                //footCallSymb = _titleCounter++.ToString();
                //footCallSymb = " ";
                _isEmptyTitleExist = true;
            }
            //footCallSymb = footCallSymb.Trim() + " ";
            //footCallSymb = footCallSymb.Trim();
            string footerCall = footerClassName + "..footnote-call";
            string footerMarker = footerClassName + "..footnote-marker";
            if (IdAllClass.ContainsKey(footerCall) && (footerClassName.IndexOf("NoteCross") != 0 && String.IsNullOrEmpty(footCallSymb)))
                footCallSymb = string.Empty;

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
            _writer.WriteAttributeString("text:style-name", footerClassName);
            if (marker != string.Empty && marker != footCallSymb)
            {

                _writer.WriteStartElement("text:span");
                _writer.WriteAttributeString("text:style-name", footerMarker);
                _writer.WriteString(marker);
                _writer.WriteEndElement();
            }
            //TD-3343
            content = NoteCrossHyphenReferenceContentNodeMissing(content);

            _writer.WriteRaw(content);
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            //isFootnote = false;

            if (footerClassName.IndexOf("NoteCross") != 0)
            {
                _writer.WriteStartElement("text:s"); // Insert space after the footnote call
                _writer.WriteAttributeString("text:c", "1");
                _writer.WriteEndElement();
            }
        }

        private static string NoteCrossHyphenReferenceContentNodeMissing(string content)
        {
            string pattern = "<text:span text:style-name=\"NoteCrossHYPHENReferenceParagraph..footnote-marker\">\\s?<|<text:span text:style-name=\"NoteGeneralParagraph..footnote-marker\">\\s?<";
            MatchCollection matches = Regex.Matches(content, pattern);
            if (matches.Count == 1)
            {
                content = content + "</text:span>";
            }
            return content;
        }

        private bool VisibleHidden()
        {
            bool hidden = false;
            if (isHiddenText)
            {
                int noOfChar = _reader.Value.Trim().Replace("\r\n", "").Length;
                noOfChar = noOfChar + (noOfChar * 20 / 100);
                //_writer.WriteStartElement("text:s");
                //_writer.WriteAttributeString("text:c", noOfChar.ToString());
                //_writer.WriteString(" ");
                //_writer.WriteEndElement();
                _writer.WriteString(" ");
                isHiddenText = false;
                hidden = true;
            }
            return hidden;
        }

        private void IsAnchorBookMark()
        {
            if (_anchorStart)
            {
                string val = _anchorBookMarkName.Replace("href#", "").Replace("name", "");
                //if (!_referenceIDList.Contains(_anchorIdValue.Replace("#", "")))
                if ((_sourceList.Contains(val.ToLower()) && _targetList.Contains(val.ToLower())))
                {
                    _anchorWrite = true;
                }
                _anchorStart = false;
            }
            //else
            //{
            //    if (_anchorIdValue.Length > 0)
            //    {
            //        string anchorName = _anchorBookMarkName.Replace("name", "");
            //        _writer.WriteStartElement("text:reference-mark");
            //        _writer.WriteAttributeString("text:name", _anchorIdValue.ToLower());
            //        _writer.WriteEndElement();
            //        _anchorIdValue = string.Empty;
            //    }
            //}
            //return isAnchorTagOpen;
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
                if (_reader.Name == "div" && _projInfo.DefaultXhtmlFileWithPath.ToLower().IndexOf("flexrev") < 0)// 
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
            //if (_isDisplayNone) return; // skip the node

            StartElementBase(_IsHeadword);
            _imageInserted = InsertImage();
            ListBegin();
            SetClassCounter();
            Psuedo();
            VisibilityCheck();
            DropCaps();
            SetFootnote();
            FooterSetup(Common.OutputType.ODT.ToString());
            ResetTitleCounter();
            IsAnchorBookMark();
            Table();
        }

        private void ResetTitleCounter()
        {
            if (_isEmptyTitleExist && _classNameWithLang == "scrBook")
            {
                _titleCounter = 1;
            }
        }

        private void ListBegin()
        {
            if (_tagType == "ol" || _tagType == "ul")
            {
                ClosePara(false);
                string listClassName = Common.LeftString(_paragraphName, "_");
                if (listClassName.IndexOf(_tagType) == -1 || !IdAllClass.ContainsKey(listClassName))
                {
                    listClassName = _tagType;
                }
                _writer.WriteStartElement("text:list");
                _writer.WriteAttributeString("text:style-name", listClassName);
            }
            else if (_tagType == "li")
            {
                _writer.WriteStartElement("text:list-item");
            }
        }

        private void SetFootnote()
        {
            string footerCall = _className + "..footnote-call";
            if (IdAllClass.ContainsKey(footerCall) && IdAllClass[footerCall].ContainsKey("content"))
            {
                footCallSymb = IdAllClass[footerCall]["content"];
                if (footCallSymb.IndexOf('(') >= 0)
                {
                    string attrName = footCallSymb.Substring(footCallSymb.IndexOf('(') + 1, footCallSymb.Length - footCallSymb.IndexOf('(') - 2);
                    try
                    {
                        footCallSymb = _reader.GetAttribute(attrName);
                        if (!string.IsNullOrEmpty(_customFootnoteSymbol) && _customFootnoteSymbol.ToLower() != "default" && footerCall.IndexOf("NoteGeneral") == 0)
                        {
                            footCallSymb = _customFootnoteSymbol;
                        }
                        else if (!string.IsNullOrEmpty(_customXRefSymbol) && _customXRefSymbol.ToLower() != "default" && footerCall.IndexOf("NoteCross") == 0)
                        {
                            footCallSymb = _customXRefSymbol;
                        }
                        else if (footCallSymb != null && footCallSymb.Trim().Length == 0)
                        {
                            if (footerCall.IndexOf("NoteCross") == 0)
                            {
                                footCallSymb = "\u2009";
                            }
                            else
                            {
                                footCallSymb = "*";
                            }
                        }
                    }
                    catch (NullReferenceException)
                    {
                        //footCallSymb = "\u2006";
                        footCallSymb = "*";
                    }
                }
            }
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
            if (_visibilityClassName.Contains(_className))
            {
                isHiddenText = true;
            }
        }

        public override void CreateSectionClass(string readerValue)
        {
            ClosePara(false);
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
                IsFirstEntry = true;
            }
        }

        private void Psuedo()
        {
            if (_isDisplayNone) return; // skip the node

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

        private void EndElement()
        {
            if (_closeChildName.IndexOf("scrBookName") == 0)
            {
                if (isPageBreak) return;
            }
            //if (isPageBreak) return;
            if (_reader.Name == "a" && _anchorWrite)
            {
                _anchorWrite = false;
            }

            _characterName = null;
            if (_hasImgCloseTag && _imageInserted)
            {
                _hasImgCloseTag = false;
            }
            else
            {
                _closeChildName = StackPop(_allStyle);
            }
            if (_closeChildName == string.Empty) return;
            string closeChild = Common.LeftString(_closeChildName, "_");

            //if (_closeChildName.IndexOf("scrBookName") == 0)
            //{
            //    _strBook = "";
            //    _strBook2ndBook = "";
            //}

            ReferenceClose(_closeChildName);
            CheckDisplayNone(closeChild);
            if (_outputType == Common.OutputType.ODT && (_reader.Name == "ul" || _reader.Name == "ol"))
            {
                _writer.WriteEndElement();
            }
            if (_outputType == Common.OutputType.ODT && (_reader.Name == "li"))
            {
                _writer.WriteEndElement();
            }

            TableClose();
            SectionClose(closeChild);
            //SetHeadwordFalse();  Note: verify &todo in OO td2000 
            ClosefooterNote();
            bool isImageEnd = EndElementForImage();
            //PseudoAfter();
            EndElementBase(isImageEnd); //Note: base class
            ColumnClass();

            _classNameWithLang = StackPeek(_allStyle);
            _classNameWithLang = Common.LeftString(_classNameWithLang, "_");
        }

        private void TableClose()
        {

            // end of table,td,tr,th
            if (_reader.Name == "table" || _reader.Name == "th" || _reader.Name == "tr" || _reader.Name == "td")
            {
                if (_isTableOpen == false)
                {
                    _table.Clear();
                    _paragraphName = null;
                    return;
                }
                _writer.WriteEndElement();
                if (_reader.Name == "table")
                {
                    _tableColumnModify["table" + _tableCount] = _tableColumnCount;
                    _isTableOpen = false;
                }
            }
            //if (_reader.Name == "table" || _reader.Name == "th" || _reader.Name == "tr" || _reader.Name == "td") // end of table,td,tr,th
            //{
            //    _writer.WriteEndElement();
            //}
            //else if (_isTableOpen) // After closed the entire table 
            //{
            //    _tableColumnModify["table" + _tableCount] = _tableColumnCount;
            //    _isTableOpen = false;
            //}
        }

        //private void PseudoAfter()
        //{
        //    if (_psuedoAfter.Count > 0)
        //    {
        //        if (_psuedoAfter.ContainsKey(_closeChildName))
        //        {
        //            ClassInfo classInfo = _psuedoAfter[_closeChildName];
        //            WriteCharacterStyle(classInfo.Content, classInfo.StyleName, true);
        //            _psuedoAfter.Remove(_closeChildName);
        //        }
        //    }
        //}

        private void ColumnClass()
        {
            if (_columnClass.Count > 0)
            {
                if (_closeChildName == _columnClass[_columnClass.Count - 1].ToString())
                {
                    _columnClass.RemoveAt(_columnClass.Count - 1);
                    //////////////////////////CloseFile(); ???
                }
            }
        }

        private void SectionClose(string closeChild)
        {
            if (_sectionName.Contains(closeChild)) // section close
            {
                _writer.WriteEndElement();
            }
        }

        private void CheckDisplayNone(string closeChild)
        {
            if (closeChild.Length > 0 && _displayNoneStyle == closeChild)
                _isDisplayNone = false;
        }

        private void ReferenceClose(string closeChild)
        {
            //string type1 = _tagType;
            //if (closeChild.Length > 0 && StackPeek(_referenceCloseStyleStack) == closeChild)
            //{
            //    _writer.WriteEndElement();
            //    StackPop(_referenceCloseStyleStack);
            //    string type = _tagType;
            //}

        }

        /// <summary>
        /// Collects <Table> information
        /// </summary>
        private void Table()
        {
            if (_tagType == "table")
            {
                _table.Add("table:table|table" + ++_tableCount);
                _table.Add("table:table-column|" + _childName);

            }
            else if (_tagType == "tr")
            {
                _table.Add("table:table-row|" + _childName);
                _tableColumnCount = 0;
            }
            else if (_tagType == "td" || _tagType == "th")
            {
                _table.Add("table:table-cell|" + _childName);
                _tableColumnCount++;
                _isNewParagraph = true;
            }
        }


        /// <summary>
        /// Writes <Table> information
        /// </summary>
        private void WriteTable()
        {
            if (_table.Count > 0)
            {
                foreach (string st in _table)
                {
                    string[] tag_styleName = st.Split('|');
                    _writer.WriteStartElement(tag_styleName[0]);
                    _writer.WriteAttributeString("table:style-name", tag_styleName[1]);


                    if (tag_styleName[0] == "table:table")
                    {
                        _writer.WriteAttributeString("table:name", tag_styleName[1]);
                        _isTableOpen = true;
                    }
                    else if (tag_styleName[0] == "table:table-column")
                    {
                        _writer.WriteAttributeString("table:number-columns-repeated", "1");
                        _writer.WriteEndElement();
                    }
                }
                _table.Clear();
            }
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
                        //_writer.WriteStartElement("text:s");
                        //_writer.WriteAttributeString("text:c", j.ToString());
                        //_writer.WriteString(" ");
                        //_writer.WriteEndElement();
                        _writer.WriteString(" ");

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
                        //_writer.WriteStartElement("text:s");
                        //_writer.WriteAttributeString("text:c", j.ToString());
                        //_writer.WriteString(" ");
                        //_writer.WriteEndElement();
                        _writer.WriteString(" ");
                        j = 0;
                    }
                    _writer.WriteString(var.ToString());
                }
            }
        }

        private void CreateFile(string targetPath)
        {
            string targetContentXML = targetPath + "content.xml";
            _writer = new XmlTextWriter(targetContentXML, null);
            //{ Formatting = Formatting.Indented };
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
            _writer.WriteAttributeString("xmlns:formx", "urn:openoffice:names:experimental:ooxml-odf-interop:xmlns:form:1.0");
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
                //_writer.WriteStartElement("script:event-listener");
                //_writer.WriteAttributeString("script:language", "ooo:script");
                //_writer.WriteAttributeString("script:event-name", "office:print");
                //if (string.Compare(_projectType, "Scripture") == 0)
                //{
                //    _writer.WriteAttributeString("xlink:href", "vnd.sun.star.script:Standard.Module1.IsReferenceCorrected?language=Basic&location=document");
                //}
                //else
                //{
                //    _writer.WriteAttributeString("xlink:href", "vnd.sun.star.script:Standard.Module1.IsGuidewordsCorrected?language=Basic&location=document");
                //}
                //_writer.WriteEndElement();
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

            _writer.WriteStartElement("style:font-face");
            _writer.WriteAttributeString("style:name", "Latha");
            _writer.WriteAttributeString("svg:font-family", "'Latha'");
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

            if (_isFromExe)
            {
                _writer.WriteStartElement("style:style");
                _writer.WriteAttributeString("style:name", "fr11");
                _writer.WriteAttributeString("style:family", "graphic");
                _writer.WriteAttributeString("style:parent-style-name", "Graphics");
                _writer.WriteStartElement("style:graphic-properties");
                _writer.WriteAttributeString("style:run-through", "background");
                _writer.WriteAttributeString("style:wrap", "run-through");
                _writer.WriteAttributeString("style:number-wrapped-paragraphs", "no-limit");
                _writer.WriteAttributeString("style:horizontal-pos", "center");
                _writer.WriteAttributeString("style:horizontal-rel", "paragraph");
                _writer.WriteAttributeString("fo:clip", "rect(0pt, 0pt, 0pt, 0pt)");
                _writer.WriteAttributeString("draw:luminance", "0%");
                _writer.WriteAttributeString("draw:contrast", "0%");
                _writer.WriteAttributeString("draw:red", "0%");
                _writer.WriteAttributeString("draw:green", "0%");
                _writer.WriteAttributeString("draw:blue", "0%");
                _writer.WriteAttributeString("draw:gamma", "100%");
                _writer.WriteAttributeString("draw:color-inversion", "false");
                _writer.WriteAttributeString("draw:image-opacity", "100%");
                _writer.WriteAttributeString("draw:color-mode", "standard");
                _writer.WriteEndElement();
                _writer.WriteEndElement();
            }
            //TD-2448
            //if (_projInfo.ProjectInputType.ToLower() == "dictionary")
            //{
            _writer.WriteStartElement("style:style");
            _writer.WriteAttributeString("style:name", "P2");
            _writer.WriteAttributeString("style:family", "paragraph");
            _writer.WriteAttributeString("style:parent-style-name", "Frame_20_contents");
            _writer.WriteStartElement("style:paragraph-properties");
            _writer.WriteAttributeString("fo:text-align", "end");
            _writer.WriteAttributeString("style:justify-single-word", "false");
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            _writer.WriteStartElement("style:style");
            _writer.WriteAttributeString("style:name", "fr1");
            _writer.WriteAttributeString("style:family", "graphic");
            _writer.WriteAttributeString("style:parent-style-name", "Frame");
            _writer.WriteStartElement("style:graphic-properties");
            _writer.WriteAttributeString("style:vertical-pos", "from-top");
            _writer.WriteAttributeString("style:vertical-rel", "page");
            _writer.WriteAttributeString("style:horizontal-pos", "right");
            _writer.WriteAttributeString("style:horizontal-rel", "paragraph");
            _writer.WriteAttributeString("fo:padding", "0pt");
            _writer.WriteAttributeString("fo:border", "none");
            _writer.WriteAttributeString("style:flow-with-text", "false");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            //}

            if (_outputExtension == "odm")
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
            if (_allStyle.Count > 0)
            {
                string stackClass = _allStyle.Peek();
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
                    wrapSide = "right";
                    break;
                case "right":
                    wrapSide = "left";
                    break;
                case "both":
                    wrapSide = "both";
                    wrapMode = "JumpObjectTextWrap";
                    break;
                case "none":
                    wrapSide = "both";
                    break;
            }
        }

        private void GetAlignment(ref string align, string className)
        {
            string clsName = className;
            //TextInfo _titleCase = CultureInfo.CurrentCulture.TextInfo;
            string floatValue = GetPropertyValue(clsName, "float");
            if (floatValue.Length > 0)
            {
                align = floatValue;
            }
        }

        private void GetAlignment(ref string wrapSide, ref string HoriAlignment)
        {
            string[] splitedClassName = _allStyle.ToArray();
            //if (_allStyle.Count > 0)
            //    splitedClassName = _allStyle.ToArray();

            if (splitedClassName.Length > 0)
            {
                //for (int i = splitedClassName.Length -1; i >= 0; i--)  // From Begining to Recent Class
                for (int i = 0; i < splitedClassName.Length; i++) // // From Recent to Begining Class
                {
                    string clsName = splitedClassName[i];
                    string pos = GetPropertyValue(clsName, "float", "left");
                    switch (pos)
                    {
                        case "left":
                            HoriAlignment = pos;
                            break;
                        case "right":
                            HoriAlignment = pos;
                            break;
                        case "top":
                        case "prince-column-top":
                        case "-ps-column-top":
                        case "top-left":
                            HoriAlignment = "top";
                            break;
                        case "top-right":
                            HoriAlignment = "top";
                            break;
                        case "bottom":
                        case "prince-column-bottom":
                        case "-ps-column-bottom":
                        case "bottom-left":
                            HoriAlignment = "bottom";
                            break;
                        case "bottom-right":
                            HoriAlignment = "right";
                            break;
                    }
                    wrapSide = GetPropertyValue(clsName, "clear", wrapSide);
                    if (HoriAlignment != "left")
                        break;
                }
            }
        }

        public bool InsertImage()
        {

            string classPicture = _reader.GetAttribute("class") ?? "img";
            string altText = _imageAltText;
            //if (_isDisplayNone) // Checking all parent classes
            //    return;

            //if (_structStyles.DisplayNone.Contains(classPicture)) // Checking current class
            //    return;
            _overWriteParagraph = false;
            bool inserted = false;
            if (_imageInsert)
            {
                if (_allStyle.Peek().IndexOf("coverImage") == -1)
                {
                    GetPictureDisplay();
                    if (_isPictureDisplayNone)
                    {
                        string ccc = _imageClass;
                        _imageInsert = false;
                        _imageSource = string.Empty;
                        _imageParaForCaption = true;
                        return false;
                    }
                    //1 inch = 72 PostScript points
                    //string alignment = "left";
                    string wrapSide = string.Empty;
                    string rectHeight = "0";
                    string rectWidth = "0";
                    string srcFile;
                    string wrapMode = "BoundingBoxTextWrap";
                    string HoriAlignment = string.Empty;
                    string VertAlignment = "center";
                    //string VertRefPoint = "LineBaseline";
                    //string AnchorPoint = "TopLeftAnchor";

                    isImage = true;
                    inserted = true;
                    string[] cc = _allStyle.ToArray(); //_allParagraph.ToArray();
                    imageClass = cc[0]; //cc[1];
                    srcFile = _imageSource;
                    string srcFilrLongDesc = _imageLongDesc;

                    //string srcFilrLongDesc = _imageSrcClass;

                    //string fileName = "file:" + Common.GetPictureFromPath(srcFile, "", _sourcePicturePath);
                    string executablePath = Path.GetDirectoryName(Application.ExecutablePath);
                    string currentPicturePath = _sourcePicturePath;
                    if (_allStyle.Peek().IndexOf("logo") == 0)
                    {
                        currentPicturePath = Common.FromRegistry("Copyrights");
                    }
                    string fromPath = Common.GetPictureFromPath(srcFile, _metaValue, currentPicturePath);
                    string fileName = Path.GetFileName(srcFile);

                    string normalTargetFile = _projInfo.TempOutputFolder;
                    string basePath = normalTargetFile.Substring(0,
                                                                 normalTargetFile.LastIndexOf(
                                                                     Path.DirectorySeparatorChar));
                    String toPath = Common.DirectoryPathReplace(basePath + "/Pictures/" + fileName);
                    if (File.Exists(fromPath))
                    {
                        File.Copy(fromPath, toPath, true);
                    }

                    string clsName = _allStyle.Peek();
                    GetAlignment(ref wrapSide, ref HoriAlignment);
                    if (srcFilrLongDesc.Length > 0 && IdAllClass.ContainsKey(clsName) &&
                        (HoriAlignment == "left" || HoriAlignment == "right"))
                    {
                        //img[src='Thomsons-gazelle1.jpg'] 
                        rectHeight = GetPropertyValue(srcFilrLongDesc, "height", rectHeight);
                        rectWidth = GetPropertyValue(srcFilrLongDesc, "width", rectWidth);
                        if (rectHeight == "0" && rectWidth == "0")
                        {
                            clsName = _childName;
                            rectHeight = GetPropertyValue(clsName, "height", rectHeight);
                            rectWidth = GetPropertyValue(clsName, "width", rectWidth);
                        }
                        GetAlignment(ref HoriAlignment, srcFilrLongDesc);
                    }
                    else
                    {
                        GetHeightandWidth(ref rectHeight, ref rectWidth);
                        //GetAlignment(alignment, ref wrapSide, ref HoriAlignment, ref AnchorPoint, ref VertRefPoint, ref VertAlignment, ref wrapMode);
                        //GetAlignment(ref wrapSide, ref HoriAlignment);
                        GetWrapSide(ref wrapSide, ref wrapMode);
                    }

                    // Setting the Height and Width according css value or the image size
                    if (rectHeight != "0")
                    {
                        if (rectWidth == "0") //H=72 W=0
                        {
                            rectWidth = Common.CalcDimension(fromPath, ref rectHeight, 'W');
                        }

                    }
                    else if (rectWidth != "0" && rectWidth != "72") //H=0; W != 0,72 
                    {
                        rectHeight = Common.CalcDimension(fromPath, ref rectWidth, 'H');
                    }
                    else if (rectWidth == "0" && rectHeight == "0") //H=0; W = 0, 
                    {
                        double value = .9;
                        if (_allStyle.Peek().IndexOf("logo") == 0)
                        {
                            if (_projInfo.ProjectInputType.ToLower() == "scripture")
                            {
                                value = .45;
                            }
                            else if (_projInfo.ProjectInputType.ToLower() == "dictionary")
                            {
                                value = .25;
                            }
                        }
                        rectWidth = Convert.ToString(Common.ColumnWidth * value);
                        rectHeight = Common.CalcDimension(fromPath, ref rectWidth, 'H');
                    }
                    else
                    {
                        //Default value is 72 
                        rectHeight = "72"; // fixed the width as 1 in = 72pt;
                        rectWidth = Common.CalcDimension(fromPath, ref rectHeight, 'W');
                    }
                    if (rectWidth == "0")
                    {
                        rectWidth = "72";
                    }
                    if (rectHeight == "0")
                    {
                        rectHeight = "72";
                    }

                    if (imageClass.ToLower().IndexOf("picturecenter") == 0)
                        HoriAlignment = "center";

                    string strFrameCount = "Graphics" + _frameCount;
                    _imageGraphicsName = strFrameCount;
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
                    if (_isParagraphClosed) // Forcing a Paragraph Style, if it is not exist
                    {
                        int counter = _allParagraph.Count;
                        string divTagName = string.Empty;
                        if (counter > 0)
                        {
                            var tempStyle = new string[counter];
                            _allParagraph.CopyTo(tempStyle, 0);
                            divTagName = counter > 1 ? tempStyle[1] : tempStyle[0];
                        }
                        _writer.WriteStartElement("text:p");
                        //_writer.WriteAttributeString("text:style-name", _util.ParentName);
                        _writer.WriteAttributeString("text:style-name", divTagName);

                        _isParagraphClosed = false;
                        _isNewParagraph = false;
                    }

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
                        if (_allStyle.Peek().IndexOf("logo") != 0 && (HoriAlignment == "top" || HoriAlignment == "bottom"))
                            anchorType = "page";
                        _writer.WriteAttributeString("text:anchor-type", anchorType);
                        _writer.WriteAttributeString("draw:z-index", "1");
                    }
                    else
                    {
                        anchorType = "as-char";
                        _writer.WriteAttributeString("text:anchor-type", anchorType);
                        _writer.WriteAttributeString("draw:z-index", "0");
                    }

                    if (HoriAlignment == "left" || HoriAlignment == "right")
                    {
                        // = (Column width - column-gap)/2 - Left Margin

                        //string pageWidth = Common.ConvertToInch(_pageLayoutProperty["fo:page-width"]);
                        //string spacing = Common.ConvertToInch(columnGap) / 2;
                        //string relWidth = (pageWidth - (spacing * (columnCount * 2))) / columnCount;
                        //double halfWidth = double.Parse(rectWidth) / 2;
                        ////double halfWidth = double.Parse(Common.ColumnWidth.ToString()) / 2;
                        //double halfHeight = double.Parse(rectHeight) / 2;
                        //rectWidth = halfWidth.ToString();
                        //rectHeight = halfHeight.ToString();
                    }

                    string width = rectWidth;
                    if (rectWidth.IndexOf("%") == -1)
                        width = rectWidth + imgWUnit;

                    string height = rectHeight;
                    if (rectHeight.IndexOf("%") == -1)
                        height = rectHeight + imgWUnit;

                    if (_allStyle.Peek().IndexOf("logo") == 0)
                    {
                        _writer.WriteAttributeString("svg:width", "2.3063in");
                        //_writer.WriteAttributeString("svg:height", height);
                    }
                    else
                    {
                        if (width != "100%")
                            _writer.WriteAttributeString("svg:width", width);
                        _writer.WriteAttributeString("svg:height", height);
                    }

                    //TD-349(width:auto)
                    if (_isAutoWidthforCaption)
                    {
                        _writer.WriteAttributeString("fo:min-width", rectWidth + imgWUnit);
                    }

                    //1st textbox
                    _writer.WriteStartElement("draw:text-box");
                    _writer.WriteAttributeString("fo:min-height", _allStyle.Peek().IndexOf("logo") == 0 ? "1in" : "0in");


                    _frameCount++;

                    if (_allStyle.Peek().ToLower().IndexOf("picturenone") == 0)
                    {
                        HoriAlignment = "center";
                        wrapSide = "none";
                    }

                    ModifyLOStyles modifyIDStyles = new ModifyLOStyles();
                    modifyIDStyles.CreateGraphicsStyle(_styleFilePath, strFrameCount, _util.ParentName, HoriAlignment,
                                                       wrapSide);

                    _writer.WriteStartElement("draw:frame");
                    _writer.WriteAttributeString("draw:style-name", strFrameCount);
                    _writer.WriteAttributeString("draw:name", strFrameCount);
                    _writer.WriteAttributeString("text:anchor-type", "paragraph");
                    // _writer.WriteAttributeString("text:anchor-type", anchorType);

                    if (rectWidth.IndexOf("%") == -1)
                        width = rectWidth + imgWUnit;
                    else
                        width = "100%";

                    if (rectHeight.IndexOf("%") == -1)
                        height = rectHeight + imgWUnit;
                    else
                        height = "100%";

                    if (width != "100%")
                        _writer.WriteAttributeString("svg:width", width);
                    if (height != "100%")
                        _writer.WriteAttributeString("svg:height", height);

                    //_writer.WriteAttributeString("style:rel-height", "scale");
                    //_writer.WriteAttributeString("style:rel-width", "100%");

                    _writer.WriteStartElement("draw:image");
                    _writer.WriteAttributeString("xlink:type", "simple");
                    _writer.WriteAttributeString("xlink:show", "embed");
                    _writer.WriteAttributeString("xlink:actuate", "onLoad");
                    _writer.WriteAttributeString("xlink:href", "Pictures/" + fileName);
                    _writer.WriteEndElement();
                    //_writer.WriteStartElement("svg:desc");
                    //_writer.WriteString(altText);
                    _writer.WriteStartElement("svg:title");
                    _writer.WriteString(fileName);
                    _writer.WriteEndElement();
                    _writer.WriteEndElement();


                    _imageInsert = false;
                    _imageSource = string.Empty;
                    _isNewParagraph = false;
                    _isParagraphClosed = true;
                }
                else
                {
                    GetPictureDisplay();
                    string rectHeight = "0";
                    string rectWidth = "0";
                    string srcFile;
                    string[] cc = _allStyle.ToArray(); //_allParagraph.ToArray();
                    imageClass = cc[0]; //cc[1];
                    srcFile = _imageSource;
                    string srcFilrLongDesc = _imageLongDesc;
                    string fromPath = Common.GetPictureFromPath(srcFile, _metaValue, _sourcePicturePath);
                    string fileName = Path.GetFileName(srcFile);
                    string normalTargetFile = _projInfo.TempOutputFolder;
                    string basePath = normalTargetFile.Substring(0, normalTargetFile.LastIndexOf(Path.DirectorySeparatorChar));
                    String toPath = Common.DirectoryPathReplace(basePath + "/Pictures/" + fileName);
                    if (File.Exists(fromPath))
                    {
                        File.Copy(fromPath, toPath, true);

                    }

                    string clsName = _allStyle.Peek();
                    if (srcFilrLongDesc.Length > 0 && IdAllClass.ContainsKey(clsName))
                    {
                        //img[src='Thomsons-gazelle1.jpg'] 
                        rectHeight = GetPropertyValue(srcFilrLongDesc, "height", rectHeight);
                        rectWidth = GetPropertyValue(srcFilrLongDesc, "width", rectWidth);
                    }

                    //Calculate Cover Imagesize
                    if (clsName.IndexOf("coverImage") == 0 && rectHeight == "0" && rectWidth == "0")
                    {
                        // Get Page property
                        clsName = _childName;
                        rectHeight = GetPropertyValue(clsName, "height", _pageSize["height"]);
                        rectWidth = GetPropertyValue(clsName, "width", _pageSize["width"]);

                        // Get Picture property
                        double picheight = 0.0;
                        double picwidth = 0.0;
                        if (File.Exists(fromPath))
                        {
                            Image fullimage = Image.FromFile(fromPath);
                            picheight = fullimage.Height;
                            picwidth = fullimage.Width;
                            fullimage.Dispose();
                        }

                        try
                        {
                            //Find aspect Ratio
                            if (picheight > picwidth) // Find width
                            {
                                rectWidth = (picwidth / picheight * double.Parse(rectHeight, CultureInfo.GetCultureInfo("en-US"))).ToString();
                            }
                            else  // Find height
                            {
                                rectHeight = (picheight / picwidth * double.Parse(rectWidth, CultureInfo.GetCultureInfo("en-US"))).ToString();
                            }
                        }
                        catch { }

                    }
                    //END Calculate Cover Imagesize

                    string strFrameCount = "Graphics" + _frameCount;
                    _imageGraphicsName = strFrameCount;

                    _imageZindexCounter++;
                    string imgWUnit = "pt";
                    string imgHUnit = "pt";
                    string width = rectWidth;
                    if (rectWidth.IndexOf("%") == -1)
                        width = rectWidth + imgWUnit;

                    string height = rectHeight;
                    if (rectHeight.IndexOf("%") == -1)
                        height = rectHeight + imgWUnit;
                    _frameCount++;

                    _writer.WriteStartElement("text:p");
                    _writer.WriteAttributeString("text:style-name", "Standard");
                    _writer.WriteStartElement("draw:frame");
                    _writer.WriteAttributeString("draw:style-name", "fr11");
                    _writer.WriteAttributeString("draw:name", strFrameCount);
                    if (_projInfo.DefaultRevCssFileWithPath != null && _projInfo.DefaultRevCssFileWithPath.Trim().Length > 0)
                    {
                        _writer.WriteAttributeString("text:anchor-type", "paragraph");
                    }
                    else
                    {
                        _writer.WriteAttributeString("text:anchor-type", "page");
                    }
                    if (width != "100%")
                        _writer.WriteAttributeString("svg:width", width);
                    _writer.WriteAttributeString("svg:height", height);

                    _writer.WriteStartElement("draw:image");
                    _writer.WriteAttributeString("xlink:type", "simple");
                    _writer.WriteAttributeString("xlink:show", "embed");
                    _writer.WriteAttributeString("xlink:actuate", "onLoad");
                    _writer.WriteAttributeString("xlink:href", "Pictures/" + fileName);
                    _writer.WriteEndElement();
                    _writer.WriteEndElement();
                    _imageInsert = false;
                    _imageSource = string.Empty;
                    _isNewParagraph = false;
                    _isParagraphClosed = true;
                }
            }
            return inserted;
        }


        private void GetPictureDisplay()
        {
            string className = Common.RightString(_childName, "_");
            if (className.ToLower().IndexOf("picture") == 0)
                className = Common.LeftString(className, "_");

            if (IdAllClass.ContainsKey(className) && IdAllClass[className].ContainsKey("display"))
            {
                if (IdAllClass[className]["display"] == "none")
                {
                    _isPictureDisplayNone = true;
                }
            }
        }

        /// <summary>
        /// Closing all the tages for Image
        /// </summary>
        private bool EndElementForImage()
        {
            bool isImageEnd = false;
            if (_isPictureDisplayNone)
            {
                if (_imageClass.Length > 0 && _closeChildName == _imageClass)
                {
                    _isPictureDisplayNone = false;
                    isImage = false;
                    _imageClass = string.Empty;
                    _isParagraphClosed = true;
                    isImageEnd = true;
                    _paragraphName = null;
                    return false;
                }
            }

            if (_imageInsert && !_imageInserted)
            {
                if (_closeChildName == _imageClass) // Without Caption
                {
                    _allCharacter.Push(_imageClass); // temporarily storing to get width and position
                    //_imageInserted = InsertImage();
                    _writer.WriteEndElement(); // for ParagraphStyle
                    _writer.WriteEndElement(); // for Textframe
                    _allCharacter.Pop();    // retrieving it again.
                    isImage = false;
                    _imageClass = string.Empty;
                    _isParagraphClosed = true;
                    isImageEnd = true;
                }
            }
            else // With Caption
            {
                if (_imageClass.Length > 0 && _closeChildName == _imageClass)
                {
                    if (_forcedPara)
                    {
                        _writer.WriteEndElement(); // for ParagraphStyle}
                        _forcedPara = false;
                    }
                    _imageParaForCaption = false;

                    if (_closeChildName.IndexOf("coverImage") != 0)
                    {
                        _writer.WriteEndElement();// for ParagraphStyle
                        _writer.WriteEndElement(); // for Textframe
                    }

                    isImage = false;
                    _imageClass = "";
                    _isNewParagraph = false;
                    _isParagraphClosed = false;
                    //bool a = _isNewParagraph;
                    //bool b = _isParagraphClosed;

                    if (!_imageTextAvailable)
                        _imageCaptionEmpty.Add(_imageGraphicsName);

                    _imageTextAvailable = false;
                    isImageEnd = true;
                }
            }
            return isImageEnd;
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

            //TD-2448
            //if (_projInfo.ProjectInputType.ToLower() == "dictionary")
            //{
            //2377
            CreateVariable();
            //}

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

            //Get the list of headwords to insert guideword after the letdata
            ReadAllFirstEntryData(_projInfo.DefaultXhtmlFileWithPath, FirstDataOnEntry);

            //To be insert left guideword on flexrev file
            WriteLeftGuidewordOnFlexRev();

            //Fornt Matter added here//
            //WriteFrontMatter();
        }


        /// <summary>
        /// Front matter for libre office with cover page, title page, copyright page and TOC page
        /// </summary>
        private void WriteFrontMatter()
        {
            if (_isFromExe)
            {
                Param.LoadSettings();
                string organization;
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

                bool coverImage = (Param.GetMetadataValue(Param.CoverPage, organization) == null)
                                      ? false
                                      : Boolean.Parse(Param.GetMetadataValue(Param.CoverPage, organization));
                bool includeTitlePage = (Param.GetMetadataValue(Param.TitlePage, organization) == null)
                                            ? false
                                            : Boolean.Parse(Param.GetMetadataValue(Param.TitlePage, organization));
                bool copyrightInformation = (Param.GetMetadataValue(Param.CopyrightPage, organization) == null)
                                                ? false
                                                : Boolean.Parse(Param.GetMetadataValue(Param.CopyrightPage, organization));
                bool includeTitleinCoverImage = (Param.GetMetadataValue(Param.CoverPageTitle, organization) == null)
                                                    ? false
                                                    : Boolean.Parse(Param.GetMetadataValue(Param.CoverPageTitle,
                                                                                           organization));
                bool includeTOCPage = (Param.GetMetadataValue(Param.TableOfContents, organization) == null)
                                          ? false
                                          : Boolean.Parse(Param.GetMetadataValue(Param.TableOfContents, organization));

                string titleName = Param.GetMetadataCurrentValue(Param.Title);
                string publisherName = Param.GetMetadataCurrentValue(Param.Publisher);
                string copyRightFilePath = Param.GetMetadataCurrentValue(Param.CopyrightPageFilename);
                string logoName = string.Empty;
                if (Param.GetOrganization().StartsWith("SIL"))
                {
                    logoName = _projInfo.ProjectInputType.ToLower() == "dictionary"
                                   ? "sil-bw-logo.jpg"
                                   : "WBT_H_RGB_red.png";
                }
                else if (Param.GetOrganization().StartsWith("Wycliffe"))
                {
                    logoName = "WBT_H_RGB_red.png";
                }

                string logoFromPath = Param.GetMetadataValue(Param.CopyrightPageFilename, organization);
                logoFromPath = Common.PathCombine(Path.GetDirectoryName(logoFromPath), logoName);
                string normalTargetFile = _projInfo.TempOutputFolder;
                string basePath = normalTargetFile.Substring(0,
                                                             normalTargetFile.LastIndexOf(Path.DirectorySeparatorChar));
                String logoToPath = Common.DirectoryPathReplace(basePath + "/Pictures/" + logoName);
                if (File.Exists(logoFromPath))
                {
                    File.Copy(logoFromPath, logoToPath, true);
                }

                //COVER IMAGE
                if (coverImage)
                {
                    //InsertCoverImage();
                }

                //TITLE IN COVER IMAGE
                if (includeTitleinCoverImage)
                {
                    _writer.WriteStartElement("text:p");
                    _writer.WriteAttributeString("text:style-name", "cover");
                    _writer.WriteString(titleName);
                    _writer.WriteEndElement();
                }

                _writer.WriteStartElement("text:p");
                _writer.WriteAttributeString("text:style-name", "dummypage");
                _writer.WriteEndElement();

                //TITLE PAGE
                if (includeTitlePage)
                {
                    _writer.WriteStartElement("text:p");
                    _writer.WriteAttributeString("text:style-name", "title");
                    _writer.WriteString(titleName);
                    _writer.WriteEndElement();

                    _writer.WriteStartElement("text:p");
                    _writer.WriteAttributeString("text:style-name", "logo");
                    _writer.WriteStartElement("draw:frame");
                    _writer.WriteAttributeString("draw:style-name", "GraphicsI1");
                    _writer.WriteAttributeString("draw:name", "Graphics1");
                    _writer.WriteAttributeString("text:anchor-type", "paragraph");
                    _writer.WriteAttributeString("draw:z-index", "1");
                    _writer.WriteAttributeString("svg:width", "2.3063in");
                    _writer.WriteStartElement("draw:text-box");
                    _writer.WriteAttributeString("fo:min-height", "1in");
                    _writer.WriteStartElement("text:p");
                    _writer.WriteAttributeString("text:style-name", "publisher");
                    _writer.WriteString(publisherName);
                    _writer.WriteEndElement();
                    _writer.WriteStartElement("text:p");
                    _writer.WriteAttributeString("text:style-name", "Illustration");
                    _writer.WriteStartElement("draw:frame");
                    _writer.WriteAttributeString("draw:style-name", "GraphicsI2");
                    _writer.WriteAttributeString("draw:name", "Graphics1");
                    _writer.WriteAttributeString("text:anchor-type", "paragraph");
                    _writer.WriteAttributeString("svg:height", "19.575pt");
                    _writer.WriteAttributeString("svg:width", "67.5pt");
                    _writer.WriteStartElement("draw:image");
                    _writer.WriteAttributeString("xlink:type", "simple");
                    _writer.WriteAttributeString("xlink:show", "embed");
                    _writer.WriteAttributeString("xlink:actuate", "onLoad");
                    _writer.WriteAttributeString("xlink:href", "Pictures/" + logoName);
                    _writer.WriteEndElement();
                    _writer.WriteStartElement("svg:title");
                    _writer.WriteString(logoName);
                    _writer.WriteEndElement();
                    _writer.WriteEndElement();
                    _writer.WriteEndElement();
                    _writer.WriteEndElement();
                    _writer.WriteEndElement();
                    _writer.WriteEndElement();

                    _writer.WriteStartElement("text:p");
                    _writer.WriteAttributeString("text:style-name", "dummypage");
                    _writer.WriteEndElement();
                }

                //COPYRIGHT PAGE
                if (copyrightInformation)
                {
                    Dictionary<string, string> outData = new Dictionary<string, string>();
                    ReadXHTMLData(copyRightFilePath, outData);

                    foreach (var message in outData)
                    {
                        _writer.WriteStartElement("text:p");
                        _writer.WriteAttributeString("text:style-name", message.Key);
                        _writer.WriteRaw(message.Value);
                        _writer.WriteEndElement();
                    }

                    _writer.WriteStartElement("text:p");
                    _writer.WriteAttributeString("text:style-name", "dummypage");
                    _writer.WriteEndElement();
                }

                //TABLE OF CONTENTS PAGE
                if (includeTOCPage)
                {
                    TableOfContent toc = new TableOfContent();
                    toc.CreateTOC(_writer, _projInfo.ProjectInputType);

                    _writer.WriteStartElement("text:p");
                    _writer.WriteAttributeString("text:style-name", "dummypage");
                    _writer.WriteEndElement();
                }

                InsertLoFrontMatterCss(_projInfo.DefaultCssFileWithPath);
                //End Front Matter//
            }
        }

        /// <summary>
        /// Insert the first headword in Reversal as first line
        /// </summary>
        private void WriteLeftGuidewordOnFlexRev()
        {
            //MessageBox.Show(_projInfo.IsODM.ToString());
            //Param myParam = new Param();
            //MessageBox.Show(_projInfo.DefaultXhtmlFileWithPath.ToLower());
            //return;
            if (_projInfo.DefaultXhtmlFileWithPath.ToLower().IndexOf("flexrev") > 0 && !_projInfo.IsODM)
            {
                _writer.WriteStartElement("text:p");
                _writer.WriteAttributeString("text:style-name", "P4");
                _writer.WriteEndElement();

                //firstRevHeadWord = ReadXHTMLFirstData(_projInfo.DefaultXhtmlFileWithPath);
                //if (firstRevHeadWord.Trim().Length > 0)
                //{
                //    _writer.WriteStartElement("text:p");
                //    if (_projInfo.IsODM)
                //    {
                //        //MessageBox.Show("ODM " + _projInfo.DefaultXhtmlFileWithPath.ToLower());
                //        //_writer.WriteAttributeString("text:style-name", "hideDiv_dicBody");
                //        //_writer.WriteStartElement("text:variable-set");
                //        //_writer.WriteAttributeString("text:name", "Left_Guideword_L");
                //        //_writer.WriteAttributeString("text:display", "none");
                //        //_writer.WriteAttributeString("text:formula", "ooow: " + firstRevHeadWord);
                //        //_writer.WriteAttributeString("office:value-type", "string");
                //        //_writer.WriteAttributeString("office:string-value", firstRevHeadWord);
                //        //_writer.WriteEndElement();
                //    }
                //    else
                //    {
                //        //_writer.WriteAttributeString("text:style-name", "hideDiv_dicBody");
                //        //MessageBox.Show("Odt " + _projInfo.DefaultXhtmlFileWithPath.ToLower());
                //        _writer.WriteAttributeString("text:style-name", "P4");
                //    }
                //    //_writer.WriteStartElement("text:variable-set");
                //    //_writer.WriteAttributeString("text:name", "Left_Guideword_L");
                //    //_writer.WriteAttributeString("text:display", "none");
                //    //_writer.WriteAttributeString("text:formula", "ooow: " + firstRevHeadWord);
                //    //_writer.WriteAttributeString("office:value-type", "string");
                //    //_writer.WriteAttributeString("office:string-value", firstRevHeadWord);
                //    //_writer.WriteEndElement();

                //    _writer.WriteEndElement();
                //    firstRevHeadWord = string.Empty;
                //}
            }
        }

        private void CallTOC()
        {
            Param.LoadSettings();
            string organization;
            try
            {
                organization = "SIL International";
                // get the organization
                if (Param.Value.ContainsKey("Organization"))
                {
                    organization = Param.Value["Organization"];
                }
            }
            catch (Exception)
            {
                // shouldn't happen (ExportThroughPathway dialog forces the user to select an organization), 
                // but just in case, specify a default org.
                organization = "SIL International";
            }
            string tableOfContent = Param.GetMetadataValue(Param.TableOfContents, organization) ?? "";
            // empty string if null / not found

            if (tableOfContent.ToLower() == "true")
            {
                TableOfContent toc = new TableOfContent();
                toc.CreateTOC(_writer, _projInfo.ProjectInputType);
            }
        }

        private void CreateVariable()
        {
            _writer.WriteStartElement("text:variable-decls");
            _writer.WriteStartElement("text:variable-decl");
            _writer.WriteAttributeString("office:value-type", "string");
            _writer.WriteAttributeString("text:name", "Left_Guideword_L");
            _writer.WriteEndElement();

            //TD-2575 to avoid Mirror page Variables for Normal Page
            if (IsMirrorPage)
            {
                _writer.WriteStartElement("text:variable-decl");
                _writer.WriteAttributeString("office:value-type", "string");
                _writer.WriteAttributeString("text:name", "Right_Guideword_L");
                _writer.WriteEndElement();
                _writer.WriteStartElement("text:variable-decl");
                _writer.WriteAttributeString("office:value-type", "string");
                _writer.WriteAttributeString("text:name", "Left_Guideword_R");
                _writer.WriteEndElement();
            }

            _writer.WriteStartElement("text:variable-decl");
            _writer.WriteAttributeString("office:value-type", "string");
            _writer.WriteAttributeString("text:name", "Right_Guideword_R");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }


        private void UpdateRelativeInStylesXML()
        {
            ModifyLOStyles modifyIDStyles = new ModifyLOStyles();
            _textVariables = modifyIDStyles.ModifyStylesXML(_projectPath, _newProperty, _usedStyleName, _languageStyleName, "", _IsHeadword, ParentClass, _projInfo.HeaderFontName);
        }



        /// <summary>
        /// Closes the opened footnote Content, Chapter No and VerseNo
        /// </summary>
        private void ClosefooterNote()
        {
            //footnoteClass = "footnote_p.first_section_div.scriptureText_scrBody";
            if (_closeChildName.Length > 0 && _closeChildName == footnoteClass)
            {
                //WriteCharacterStyle(footnoteContent.ToString(), footnoteClass, false);
                WriteCharacterStyle(footnoteContent.ToString(), footnoteClass);
                isFootnote = false;
                footnoteContent.Remove(0, footnoteContent.Length);
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
            try
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

                    ModifyLOStyles modifyIDStyles = new ModifyLOStyles();
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
            catch
            {
            }
        }

        /// <summary>
        /// Closes the opened footnote Content, Chapter No and VerseNo
        /// </summary>
        private void ModifyContentXML(string targetPath)
        {
            string targetFile = targetPath + "content.xml";
            ModifyLOContent modifyContentXML = new ModifyLOContent();
            modifyContentXML.SetTableColumnCount(targetFile, _tableColumnModify);
        }


        private void WriteGuidewordValueToVariable2(string content)
        {
            //TD-2580
            string bookname = _strBook;
            //if(_classNameWithLang.IndexOf("headword") == 0 || _classNameWithLang.IndexOf("reversalform") == 0 && _previousParagraphName.IndexOf("entry_") == 0 || _previousParagraphName.IndexOf("div_pictureCaption") == 0)
            // _classNameWithLang.ToLower().IndexOf("chapternumber") == 0) && (_previousParagraphName.IndexOf("entry_") == 0           )

            //if ((_classNameWithLang.IndexOf("headword") == 0 || _classNameWithLang.IndexOf("reversalform") == 0
            //     || _classNameWithLang.ToLower().IndexOf("chapternumber") == 0) && (_previousParagraphName.ToLower().IndexOf("paragraph") == 00))
            if (((_classNameWithLang.IndexOf("headword") == 0 || _classNameWithLang.IndexOf("reversalform") == 0) && (_previousParagraphName.IndexOf("entry_") == 0 || _previousParagraphName.IndexOf("div_pictureCaption") == 0)) ||
             (_classNameWithLang.ToLower().IndexOf("chapternumber") == 0 && (_previousParagraphName.ToLower().IndexOf("paragraph") == 0)))
            {

                string chapterNo = content;

                if (_strBook.Length > 0)
                    content = _strBook + chapterNo;

                _writer.WriteStartElement("text:span");
                _writer.WriteAttributeString("text:style-name", _classNameWithLang);
                _writer.WriteStartElement("text:variable-set");
                _writer.WriteAttributeString("text:name", "Left_Guideword_L");
                _writer.WriteAttributeString("text:display", "none");
                _writer.WriteAttributeString("text:formula", "ooow: " + content);
                _writer.WriteAttributeString("office:value-type", "string");
                //_writer.WriteAttributeString("office:string-value", " " + content);//TD-2688
                _writer.WriteAttributeString("office:string-value", content);
                _writer.WriteEndElement();
                _writer.WriteEndElement();

                _writer.WriteStartElement("text:span");
                _writer.WriteAttributeString("text:style-name", _classNameWithLang);
                _writer.WriteStartElement("text:variable-set");
                _writer.WriteAttributeString("text:name", "Right_Guideword_R");
                _writer.WriteAttributeString("text:display", "none");
                _writer.WriteAttributeString("text:formula", "ooow: " + content);
                _writer.WriteAttributeString("office:value-type", "string");
                //_writer.WriteAttributeString("office:string-value", " " + content);
                _writer.WriteAttributeString("office:string-value", content);
                _writer.WriteEndElement();
                _writer.WriteEndElement();

                if (_multiLanguageHeader)
                {
                    if (_strBook2ndBook.Length > 0)
                        content = _strBook2ndBook + chapterNo;

                    _writer.WriteStartElement("text:span");
                    _writer.WriteAttributeString("text:style-name", _classNameWithLang);
                    _writer.WriteStartElement("text:variable-set");
                    _writer.WriteAttributeString("text:name", "Left_Guideword_R");
                    _writer.WriteAttributeString("text:display", "none");
                    _writer.WriteAttributeString("text:formula", "ooow: " + content);
                    _writer.WriteAttributeString("office:value-type", "string");
                    //_writer.WriteAttributeString("office:string-value", " " + content);
                    _writer.WriteAttributeString("office:string-value", content);
                    _writer.WriteEndElement();
                    _writer.WriteEndElement();

                    _writer.WriteStartElement("text:span");
                    _writer.WriteAttributeString("text:style-name", _classNameWithLang);
                    _writer.WriteStartElement("text:variable-set");
                    _writer.WriteAttributeString("text:name", "Right_Guideword_L");
                    _writer.WriteAttributeString("text:display", "none");
                    _writer.WriteAttributeString("text:formula", "ooow: " + content);
                    _writer.WriteAttributeString("office:value-type", "string");
                    //_writer.WriteAttributeString("office:string-value", " " + content);
                    _writer.WriteAttributeString("office:string-value", content);
                    _writer.WriteEndElement();
                    _writer.WriteEndElement();
                }

                _writer.WriteStartElement("text:span");
                _writer.WriteEndElement();
                //_writer.WriteRaw(" ");
                LanguageFontCheck(content, "headerFontStyleName");
            }
        }

        private void InsertBookNameBeforeBookIntroduction(string content)
        {
            if (_classNameWithLang.IndexOf("scrBookName") == 0)// == "scrBook_scrBody"
            {
                content += "1";
                _writer.WriteStartElement("text:span");
                _writer.WriteAttributeString("text:style-name", _classNameWithLang);
                _writer.WriteStartElement("text:variable-set");
                _writer.WriteAttributeString("text:name", "Left_Guideword_L");
                _writer.WriteAttributeString("text:display", "none");
                _writer.WriteAttributeString("text:formula", "ooow:" + content);
                _writer.WriteAttributeString("office:value-type", "string");
                _writer.WriteAttributeString("office:string-value", content);
                _writer.WriteEndElement();
                _writer.WriteEndElement();
            }
        }

        private void WriteGuidewordValueToVariable(string content)
        {
            bool fillHeadword = false;

            if (_projInfo.ProjectInputType.ToLower() == "dictionary")
            {
                //if (content == "inde")
                //    fillHeadword = false;
                if (_previousParagraphName == null) _previousParagraphName = string.Empty;
                if ((_classNameWithLang.IndexOf("headwordminor") == 0 || _classNameWithLang.IndexOf("headword") == 0 || (_classNameWithLang.IndexOf("reversalform") == 0 || _childName.Replace(_classNameWithLang + "_", "").IndexOf("reversalform") == 0 || _childName.Replace("span_", "").IndexOf("reversalform") == 0))
                    && (_previousParagraphName.IndexOf("minorentries_") == 0 || _previousParagraphName.IndexOf("entry_") == 0 || _previousParagraphName.IndexOf("div_pictureCaption") == 0 || _previousParagraphName.IndexOf("picture") >= 0))
                {
                    fillHeadword = true;
                }
            }
            else if (_projInfo.ProjectInputType.ToLower() == "scripture")//scripture
            {
                if (_classNameWithLang.ToLower().IndexOf("chapternumber") == 0 && (_previousParagraphName.ToLower().IndexOf("paragraph") == 0))
                {
                    fillHeadword = true;
                }
            }


            //if (((_classNameWithLang.IndexOf("headword_") == 0 || _classNameWithLang.IndexOf("reversalform") == 0) && (_previousParagraphName.IndexOf("entry_") == 0 || _previousParagraphName.IndexOf("div_pictureCaption") == 0)) ||
            // (_classNameWithLang.ToLower().IndexOf("chapternumber") == 0 && (_previousParagraphName.ToLower().IndexOf("paragraph") == 0)))
            if (fillHeadword)
            {
                //Insert leftGuideword for TD-2912
                string leftHeadword = content;

                if (_classNameWithLang.IndexOf("headword") >= 0)
                {
                    if (_headwordVariable.Count - 1 > _headwordIndex + 1)
                    {
                        if (IsFirstEntry)
                        {
                            leftHeadword = _headwordVariable[_headwordIndex];
                            ++_headwordIndex;
                            IsFirstEntry = false;
                        }
                        else
                        {
                            ++_headwordIndex;
                            leftHeadword = content;// _headwordVariable[++_headwordIndex];
                        }
                    }

                }

                string chapterNo = content;

                if (_strBook.Length > 0)
                {
                    content = _strBook + chapterNo;
                    leftHeadword = content;
                }

                _writer.WriteStartElement("text:span");
                _writer.WriteAttributeString("text:style-name", _classNameWithLang);
                _writer.WriteStartElement("text:variable-set");
                _writer.WriteAttributeString("text:name", "Left_Guideword_L");
                _writer.WriteAttributeString("text:display", "none");
                _writer.WriteAttributeString("text:formula", "ooow: " + leftHeadword);//leftHeadword
                _writer.WriteAttributeString("office:value-type", "string");
                //_writer.WriteAttributeString("office:string-value", " " + content);//TD-2688
                _writer.WriteAttributeString("office:string-value", leftHeadword);//leftHeadword
                _writer.WriteEndElement();
                _writer.WriteEndElement();

                _writer.WriteStartElement("text:span");
                _writer.WriteAttributeString("text:style-name", _classNameWithLang);
                _writer.WriteStartElement("text:variable-set");
                _writer.WriteAttributeString("text:name", "Right_Guideword_R");
                _writer.WriteAttributeString("text:display", "none");
                _writer.WriteAttributeString("text:formula", "ooow: " + content);
                _writer.WriteAttributeString("office:value-type", "string");
                //_writer.WriteAttributeString("office:string-value", " " + content);
                _writer.WriteAttributeString("office:string-value", content);
                _writer.WriteEndElement();
                _writer.WriteEndElement();

                if (_multiLanguageHeader)
                {
                    if (_strBook2ndBook.Length > 0)
                    {
                        content = _strBook2ndBook + chapterNo;
                        leftHeadword = content;
                    }
                    _writer.WriteStartElement("text:span");
                    _writer.WriteAttributeString("text:style-name", _classNameWithLang);
                    _writer.WriteStartElement("text:variable-set");
                    _writer.WriteAttributeString("text:name", "Left_Guideword_R");
                    _writer.WriteAttributeString("text:display", "none");
                    _writer.WriteAttributeString("text:formula", "ooow: " + leftHeadword);
                    _writer.WriteAttributeString("office:value-type", "string");
                    //_writer.WriteAttributeString("office:string-value", " " + content);
                    _writer.WriteAttributeString("office:string-value", leftHeadword);
                    _writer.WriteEndElement();
                    _writer.WriteEndElement();

                    _writer.WriteStartElement("text:span");
                    _writer.WriteAttributeString("text:style-name", _classNameWithLang);
                    _writer.WriteStartElement("text:variable-set");
                    _writer.WriteAttributeString("text:name", "Right_Guideword_L");
                    _writer.WriteAttributeString("text:display", "none");
                    _writer.WriteAttributeString("text:formula", "ooow: " + content);
                    _writer.WriteAttributeString("office:value-type", "string");
                    //_writer.WriteAttributeString("office:string-value", " " + content);
                    _writer.WriteAttributeString("office:string-value", content);
                    _writer.WriteEndElement();
                    _writer.WriteEndElement();
                }

                _writer.WriteStartElement("text:span");
                _writer.WriteEndElement();
                //_writer.WriteRaw(" ");
                LanguageFontCheck(content, "headerFontStyleName");
            }
        }

        /// <summary>
        /// Read XHTML content
        /// </summary>
        /// <param name="filePath">File path of the XHTML file</param>
        /// <param name="XHTMLData">Return value</param>
        public static void ReadXHTMLData(string filePath, Dictionary<string, string> XHTMLData)
        {
            XmlTextReader reader = Common.DeclareXmlTextReader(filePath, true);

            string className = "div";
            string content;
            bool headXML = true;
            while (reader.Read())
            {
                if (headXML) // skip previous parts of <body> tag
                {
                    if (reader.Name == "body")
                    {
                        headXML = false;
                    }
                    else
                    {
                        continue;
                    }
                }

                if (reader.IsEmptyElement)
                {
                    if (reader.Name == "br")
                    {
                        //_writer.WriteRaw(@"<text:line-break/>");
                        continue;
                    }
                    else
                    {
                        if (reader.Name == "a")
                        {
                            continue;
                        }
                        continue;
                    }
                }
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        string clName = StartElement(reader);
                        if (clName != String.Empty)
                        {
                            if (XHTMLData.ContainsKey(clName) || clName == "para")
                            {
                                if (XHTMLData[className].Trim() != String.Empty)
                                {

                                    XHTMLData[className] = XHTMLData[className] + @"<text:line-break/>";
                                }
                            }
                            else
                            {
                                className = clName;
                                XHTMLData[className] = String.Empty;
                            }
                        }
                        break;
                    case XmlNodeType.Text: // Text.Write
                        XHTMLData[className] = XHTMLData[className] + reader.Value;
                        break;
                }
            }
        }

        private static string StartElement(XmlReader reader)
        {
            if (reader.Name == "p") return "para";
            if (reader.Name != "div") return String.Empty;
            string className = reader.Name;
            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                {
                    if (reader.Name == "class")
                    {
                        className = reader.Value;
                        className = className.Replace("_", "");
                        className = className.Replace("-", "");
                        if (Common._outputType == Common.OutputType.XELATEX)
                        {
                            className = Common.ReplaceCSSClassName(className);
                        }
                    }
                }
            }
            return className;
        }

        /// <summary>
        /// Read XHTML content
        /// </summary>
        /// <param name="filePath">File path of the XHTML file</param>
        /// <param name="searchText">Text to be search</param>
        public static string ReadXHTMLFirstData(string filePath)
        {
            XmlTextReader reader = Common.DeclareXmlTextReader(filePath, true);

            string className = "div";
            bool isReadData = false;
            string content = String.Empty;
            bool headXML = true;
            while (reader.Read())
            {
                if (headXML) // skip previous parts of <body> tag
                {
                    if (reader.Name == "body")
                    {
                        headXML = false;
                    }
                    else
                    {
                        continue;
                    }
                }

                if (reader.IsEmptyElement)
                {
                    if (reader.Name == "br")
                    {
                        //_writer.WriteRaw(@"<text:line-break/>");
                        continue;
                    }
                    else
                    {
                        if (reader.Name == "a")
                        {
                            continue;
                        }
                        continue;
                    }
                }
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        className = StartElement(reader);
                        if (className != String.Empty)
                        {
                            if (className == "entry")
                            {
                                isReadData = true;
                            }
                        }
                        break;
                    case XmlNodeType.Text: // Text.Write
                        if (isReadData)
                        {
                            content = reader.Value;

                        }
                        break;
                }

                if (content.Trim() != String.Empty)
                {
                    break;
                }
            }

            return content;
        }

        public void InsertLoFrontMatterCss(string cssFilename)
        {
            string frontMatterCSSStyle = string.Empty;
            try
            {
                frontMatterCSSStyle = frontMatterCSSStyle + ".cover{margin-top: 112pt; text-align: center; font-size:18pt; font-weight:bold;page-break-after: always;} ";
                frontMatterCSSStyle = frontMatterCSSStyle + ".title{margin-top: 112pt; text-align: center; font-weight:bold;font-size:18pt;} .publisher{text-align: center;font-size:14pt;} .logo{page-break-after: always; text-align:center; clear:both;float:bottom;}";
                frontMatterCSSStyle = frontMatterCSSStyle + ".copyright{text-align: left; font-size:1pt;visibility:hidden;}.LHeading{font-size:18pt;font-weight:bold;line-height:14pt;margin-bottom:.25in;}.LText{font-size:12pt;font-style:italic}.LText:before{content: \"\\2028\"}.dummyTOC{text-align: left; font-size:1pt;visibility:hidden;page-break-after: always;} ";
                frontMatterCSSStyle = frontMatterCSSStyle + ".TableOfContentLO{visibility:hidden;}";
                if (frontMatterCSSStyle.Trim().Length > 0)
                    frontMatterCSSStyle = frontMatterCSSStyle + ".dummypage{page-break-after: always;} ";
                InsertLoFrontMatterCssFile(cssFilename, frontMatterCSSStyle);
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

        /// <summary>
        /// Read XHTML content
        /// </summary>
        /// <param name="filePath">File path of the XHTML file</param>
        /// <param name="searchText">Text to be search</param>
        public static void ReadAllFirstEntryData(string filePath, Dictionary<string, string> XHTMLData)
        {
            XmlTextReader reader = Common.DeclareXmlTextReader(filePath, true);

            string className = "div";
            bool isLetter = false;
            bool isEntry = false;
            bool isReadData = false;
            string letter = string.Empty;
            string entry = string.Empty;
            bool headXML = true;
            while (reader.Read())
            {
                if (headXML) // skip previous parts of <body> tag
                {
                    if (reader.Name == "body")
                    {
                        headXML = false;
                    }
                    else
                    {
                        continue;
                    }
                }

                if (reader.IsEmptyElement)
                {
                    if (reader.Name == "br")
                    {
                        continue;
                    }
                    else
                    {
                        if (reader.Name == "a")
                        {
                            continue;
                        }
                        continue;
                    }
                }
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        className = StartElement(reader);
                        if (className != string.Empty)
                        {
                            if (className == "letter")
                            {
                                isLetter = true;
                                isReadData = true;
                            }
                            else if (isReadData && className == "entry")
                            {
                                isEntry = true;
                            }
                        }
                        break;
                    case XmlNodeType.Text: // Text.Write
                        if (isLetter)
                        {
                            if (reader.Value.Trim() != string.Empty)
                            {
                                letter = reader.Value;
                                isLetter = false;
                            }
                        }
                        else if (isEntry && isReadData)
                        {
                            if (reader.Value.Trim() != string.Empty)
                            {
                                entry = reader.Value;
                                isEntry = false;
                                isReadData = false;
                                XHTMLData[letter] = entry;
                            }
                        }
                        break;
                }
            }
        }



        #endregion
    }
}
