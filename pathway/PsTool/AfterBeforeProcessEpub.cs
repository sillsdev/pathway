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
    public class AfterBeforeProcessEpub : AfterBeforeXHTMLProcess
    {
        #region Private Variable

        private string _strBook = string.Empty;
        private string _strBook2ndBook = string.Empty;
        private bool _is1stBookFound = false;
        private readonly Stack _styleStack = new Stack();
        private readonly Stack _allSpanStack = new Stack();
        private readonly Stack _allDivStack = new Stack();
        private readonly Stack _usedSpanStack = new Stack();

        private readonly Stack _tagTypeStack = new Stack();
                               // P - Para, T - Text, I - Image, L - <ol>/<ul> tag , S - <li> tag,  O - Others

        private readonly Dictionary<string, string> _makeAttribute = new Dictionary<string, string>();
        private ArrayList _sectionName = new ArrayList();
        //Dictionary<string, string> dictColumnGap = new Dictionary<string, string>();

        private bool isHiddenText = false;
        private string _familyType = string.Empty;
        private string _styleName = string.Empty;
        // Image
        private int _frameCount; //For Picture Frame count
        private string _beforePseudoValue = string.Empty;
        private string _beforePseudoParentValue = string.Empty;
        //readonly StringBuilder _imageBuilder = new StringBuilder();
        private readonly Stack _prevLangStack = new Stack(); // For handle previous Language
        private readonly StringBuilder _pseudoBuilder = new StringBuilder();
        private bool _contentReplace;
        private readonly StringBuilder _classContentBuilder = new StringBuilder();
        private int _styleCounter;
        private string _metaValue = string.Empty; // TD-278 <meta name />
        private ArrayList _odtFiles;
        //ArrayList _odtEndFiles;

        private string _outputExtension = string.Empty;
        private int _autoFootNoteCount;
        private string _sourcePicturePath;

        //Table 
        private List<string> _table = new List<string>();
        private int _tableCount = 0;
        private int _tableColumnCount = 0;
        private Dictionary<string, int> _tableColumnModify = new Dictionary<string, int>();
        private bool _isTableOpen;

        //readonly Library _lib = new Library();
        private int _pbCounter;
        private string _readerValue = string.Empty;
        private string _footnoteValue = string.Empty;
        private string _classAfter = string.Empty;
        private char _closeTagType;
        private string _parentStyleName;
        private readonly Dictionary<string, string> _existingStyleName = new Dictionary<string, string>();

        private string _styleFilePath;
        private float _columnWidth;
        private string _temp;
        private string _divClass = string.Empty;
        private string _allDiv = string.Empty;
        private string _class = string.Empty;
        private bool _pseudoSingleSpace = false;
        //bool _isWhiteSpace;
        private bool _isNewLine = true;
        private string _prevLang = string.Empty;
        private string _parentClass = string.Empty;
        private string _parentLang = string.Empty;
        private string _projectType = string.Empty;
        private readonly Dictionary<string, string> _counterVolantryReset = new Dictionary<string, string>();
        private readonly string _tempFile = Common.PathCombine(Path.GetTempPath(), "tempXHTMLFile.xhtml"); //TD-351
        private readonly string _hardSpace = Common.ConvertUnicodeToString("\u00A0");

        private ArrayList _anchor = new ArrayList();
        private XmlDocument _xmldoc;
        private bool _imageStart;
        private string _imageParent;
        private bool _imageDiv;
        private bool progressBarError;
        private string _imageGraphicsName = string.Empty;
        private bool _imageTextAvailable;
        private ArrayList _imageCaptionEmpty = new ArrayList();
        private int _imageZindexCounter;
        private bool _imagePreviousFinished = false;
        private bool _imageParaForCaption = false;
        private List<string> _unUsedParagraphStyle = new List<string>();
        private string footCallSymb = string.Empty;
        private List<string> LanguageFontStyleName = new List<string>();
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

        private string _originalXHTML = string.Empty;

        private Dictionary<string, string> _pageSize = new Dictionary<string, string>();
        private bool _isFromExe = false;

        #endregion

        #region Public Variable

        public bool _multiLanguageHeader = false;
        public string RefFormat = "Genesis 1";
        public bool IsMirrorPage;
        public string _ChapterNo = "1";

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

        public void RemoveAfterBefore(PublicationInformation projInfo,
                                      Dictionary<string, Dictionary<string, string>> idAllClass,
                                      Dictionary<string, ArrayList> classFamily, ArrayList cssClassOrder)
        {
            InitializeData(projInfo, idAllClass, classFamily, cssClassOrder);
            string sourceFile = SourceTargetFile(projInfo);
            OpenXhtmlFile(sourceFile); //reader
            CreateFile(projInfo); //writer
            InsertBeforeAfter();
        }

        private string SourceTargetFile(PublicationInformation projInfo)
        {
            string sourceFile = "";
            //if (projInfo.FinalOutput == "epub")
            //{
            sourceFile = projInfo.DefaultXhtmlFileWithPath.Replace(".xhtml", "_1.xhtml");
            File.Copy(projInfo.DefaultXhtmlFileWithPath, sourceFile, true);
            //}
            //else if (projInfo.FinalOutput == "odt" || projInfo.FinalOutput == "idml")
            //{
            //    sourceFile = projInfo.DefaultXhtmlFileWithPath;

            //    //string tempFolder = Common.PathCombine(Path.GetTempPath(), "Preprocess");
            //    string tempFolder = Path.GetTempPath();
            //    string targetFile = Path.GetFileName(projInfo.DefaultXhtmlFileWithPath);
            //    string targetFileWithPath = Common.PathCombine(tempFolder, targetFile);

            //    projInfo.DefaultXhtmlFileWithPath = targetFileWithPath;
            //}
            return sourceFile;
        }

        #endregion

        private void InitializeData(PublicationInformation projInfo,
                                    Dictionary<string, Dictionary<string, string>> idAllClass,
                                    Dictionary<string, ArrayList> classFamily, ArrayList cssClassOrder)
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

            _sourcePicturePath = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath);
            _projectPath = projInfo.TempOutputFolder;

            IdAllClass = idAllClass;
            _classFamily = classFamily;

            _isNewParagraph = false;
            _characterName = "None";
        }

        ///// <summary>
        ///// Cleanup Process
        ///// </summary>
        //public void CleanUp()
        //{
        //    if (File.Exists(_tempFile))
        //    {
        //        File.Delete(_tempFile);
        //    }
        //}

        /// <summary>
        /// To replace the symbol string if the symbol matches with the text
        /// </summary>
        /// <param name="data">XML Content</param>
        private string ReplaceString(string data)
        {
            //data = Common.ReplaceSymbolToText(data);
            if (_replaceSymbolToText.Count > 0)
            {
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


        public void InsertBeforeAfter()
        {

            bool headXML = true;
            while (_reader.Read())
            {

                switch (_reader.NodeType)
                {

                    case XmlNodeType.Element:
                        if (_reader.Name == "html")
                        {
                            _writer.WriteStartElement("html");
                            break;
                        }
                        _writer.WriteStartElement(_reader.LocalName);
                        _writer.WriteAttributes(_reader, true);
                        if (_reader.IsEmptyElement)
                        {
                            _writer.WriteEndElement();
                        }


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
                        StartElement();
                        break;

                    case XmlNodeType.Text:
                        Write(); // Code here ************
                        break;

                    case XmlNodeType.Whitespace:
                    case XmlNodeType.SignificantWhitespace:
                        //_writer.WriteWhitespace(_reader.Value);
                        InsertWhiteSpace();
                        break;

                    case XmlNodeType.CDATA:

                        _writer.WriteCData(_reader.Value);

                        break;

                    case XmlNodeType.EntityReference:

                        //_writer.WriteEntityRef(_reader.Name);
                        IncludeWhiteSpace();

                        break;

                    case XmlNodeType.XmlDeclaration:

                    case XmlNodeType.ProcessingInstruction:

                        _writer.WriteProcessingInstruction(_reader.Name, _reader.Value);

                        break;

                    case XmlNodeType.DocumentType:

                        _writer.WriteDocType(_reader.Name, _reader.GetAttribute("PUBLIC"),
                                             _reader.GetAttribute("SYSTEM"), _reader.Value);

                        break;

                    case XmlNodeType.Comment:

                        _writer.WriteComment(_reader.Value);

                        break;

                    case XmlNodeType.EndElement:
                        EndElement(); // Code here ************
                        _writer.WriteFullEndElement();

                        break;

                }
            }
            _writer.Close();
            _reader.Close();
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
            if (!whiteSpaceExist)
            {
                _writer.WriteStartElement("span");
                _writer.WriteRaw("&nbsp;");
                _writer.WriteEndElement();
                _significant = true;
                //_writer.WriteString(" ");
            }
        }

        private void InsertWhiteSpace()
        {
            bool whiteSpaceExist = _significant;
            string data = SignificantSpace(_reader.Value);
            //_writer.WriteString(data);
            if (!whiteSpaceExist)
            {
                _writer.WriteStartElement("span");
                _writer.WriteRaw("&nbsp;");
                _writer.WriteEndElement();
                _significant = true;
                //_writer.WriteString(" ");
            }
        }

        private void Write()
        {

            if (_isDisplayNone)
            {
                return; // skip the node
            }

            if (_isNewParagraph)
            {
                if (_paragraphName == null)
                {
                    _paragraphName = StackPeek(_allParagraph); // _allParagraph.Pop();
                }

                ClosePara(false);

                {

                    if (_imageInserted)
                        _imageParaForCaption = true;
                }
                _previousParagraphName = _paragraphName;
                _paragraphName = null;
                _isNewParagraph = false;
                _isParagraphClosed = false;
                _textWritten = false;
            }
            WriteText();
            isFileEmpty = false;
        }

        private void WriteText()
        {
            string content = _reader.Value;
            content = ReplaceString(content);
            if (_isPictureDisplayNone)
            {
                return;
            }

            // Psuedo Before
            foreach (ClassInfo psuedoBefore in _psuedoBefore)
            {
                //bool whiteSpaceExist = _significant;
                //string content1 = SignificantSpace(psuedoBefore.Content);
                if (psuedoBefore.Content != null && psuedoBefore.Content.Trim().Length == 0)
                {
                    if (!_significant)
                    {
                        _writer.WriteStartElement("span");
                        _writer.WriteRaw("&nbsp;");
                        _writer.WriteEndElement();
                        _significant = true;
                    }
                }
                else
                {
                    _writer.WriteString(psuedoBefore.Content);
                }
            }

            //// Psuedo Before
            //foreach (ClassInfo psuedoBefore in _psuedoBefore)
            //{
            //    //WriteCharacterStyle(psuedoBefore.Content, psuedoBefore.StyleName, true);
            //    if (psuedoBefore.Content.Trim().Length ==0)
            //    {
            //        if (!_isWhiteSpace)
            //        {
            //            _writer.WriteString(psuedoBefore.Content);
            //            _isWhiteSpace = true;
            //        }
            //    }
            //    else
            //    {
            //        _writer.WriteString(psuedoBefore.Content);
            //        _isWhiteSpace = false;
            //    }
            //}

            // Text Write
            if (_characterName == null)
            {
                _characterName = StackPeekCharStyle(_allCharacter);
            }
            //content = whiteSpacePre(content);
            bool contains = false;
            if (_psuedoContainsStyle != null)
            {
                if (content.IndexOf(_psuedoContainsStyle.Contains) > -1)
                {
                    content = _psuedoContainsStyle.Content;
                    _characterName = _psuedoContainsStyle.StyleName;
                }
            }
            content = SignificantSpace(content);
            _writer.WriteString(content);
            //string modifiedContent = ModifiedContent(content, _previousParagraphName, _characterName);
            //WriteCharacterStyle(modifiedContent, _characterName, contains);
            //if (_isDropCap) // until the next paragraph
            //{
            //    _isDropCap = false;
            //}
            _psuedoBefore.Clear();

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
            return content;
            //_writer.WriteString(content);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetPath"></param>
        private void StartElement()
        {
            StartElementBase(_IsHeadword);
            Psuedo();
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
            if (_reader.Name == "a" && _anchorWrite)
            {
                _anchorWrite = false;
            }

            _characterName = null;
            _closeChildName = StackPop(_allStyle);
            if (_closeChildName == string.Empty) return;
            string closeChild = Common.LeftString(_closeChildName, "_");

            // Psuedo After
            PseudoAfter();
            EndElementBase(false);

            _classNameWithLang = StackPeek(_allStyle);
            _classNameWithLang = Common.LeftString(_classNameWithLang, "_");
        }


        private void PseudoAfter()
        {
            if (_psuedoAfter.Count > 0)
            {
                if (_psuedoAfter.ContainsKey(_closeChildName))
                {
                    ClassInfo classInfo = _psuedoAfter[_closeChildName];
                    if (classInfo.Content != null && classInfo.Content.Trim().Length == 0)
                    {
                        if (!_significant)
                        {
                            _writer.WriteStartElement("span");
                            _writer.WriteRaw("&nbsp;");
                            _writer.WriteEndElement();
                            _significant = true;
                        }
                    }
                    else
                    {
                        _writer.WriteString(classInfo.Content);
                    }
                    _psuedoAfter.Remove(_closeChildName);
                }
            }
        }

        private void CreateFile(PublicationInformation projInfo)
        {

            _writer = new XmlTextWriter(projInfo.DefaultXhtmlFileWithPath, null);
        }

    }

}
