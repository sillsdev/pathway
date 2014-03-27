// --------------------------------------------------------------------------------------------
// <copyright file="XeLaTexContent.cs" from='2009' to='2014' company='SIL International'>
//      Copyright ( c ) 2014, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
//
// </remarks>
// --------------------------------------------------------------------------------------------

#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;
using System.IO;
using SIL.Tool;

#endregion Using

namespace SIL.PublishingSolution
{
    public class XeLaTexContent : XHTMLProcess
    {
        #region Private Variable

        private int _storyNo = 0;
        private int _hyperLinkNo = 0;
        private bool isFileEmpty = true;
        private bool isFileCreated;
        private bool _isDropCaps;
        private bool _nextContent;
        private int _incrementDropCap = 0;

        private bool _hasImgCloseTag;
        private bool isImageAvailable;
        private bool isHomographNumber = false;
        private string columnCount = string.Empty;
        private string imageClass = string.Empty;
        private string _inputPath;
        private ArrayList _textFrameClass = new ArrayList();
        private ArrayList _textVariables = new ArrayList();
        private ArrayList _columnClass = new ArrayList();
        private ArrayList _psuedoBefore = new ArrayList();
        private Dictionary<string, ClassInfo> _psuedoAfter = new Dictionary<string, ClassInfo>();
        private Dictionary<string, ArrayList> _styleName = new Dictionary<string, ArrayList>();
        private ArrayList _crossRef = new ArrayList();
        private int _crossRefCounter = 1;
        private bool _isWhiteSpace = true;
        private bool _imageInserted;
        private List<string> _usedStyleName = new List<string>();
        private List<string> _mergedInlineStyle;
        private bool _IsHeadword = false;
        private bool _isDropCap = false;
        private bool _xetexNewLine;
        private string _dropCapStyle = string.Empty;
        private string _currentStoryName = string.Empty;
        Dictionary<string, List<string>> _classInlineStyle = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> _classInlineInnerStyle = new Dictionary<string, List<string>>();
        private string xhtmlFile;
        private Dictionary<string, string> _endParagraphStringDic = new Dictionary<string, string>();
        private int _pageWidth;

        protected Stack<string> _braceClass = new Stack<string>();
        protected Stack<string> _braceInlineClass = new Stack<string>();
        protected Dictionary<string, int> _braceInlineClassCount = new Dictionary<string, int>();
        protected Stack<string> _mathStyleClass = new Stack<string>();
        private List<string> _mathStyle = new List<string>();
        private int _inlineCount;
        private string _headerContent = string.Empty;
        private bool _dictionaryStarting = false;
        public bool _dictionaryEnding = false;
        private string _tocStartingPage;
        private int _tocPageStock = 0;
        private string _tocStyleName;
        private Dictionary<string, string> _toc = new Dictionary<string, string>();
        private bool _isHeadwordInsertedAfterImage = false;
        private string _bookName = string.Empty;
        public string _dicMainReversal = string.Empty;
        private string _chapterStyleforHeader = string.Empty;
        private int _bookCount = 0;
        private bool _bookPageBreak;
        private bool _hideFirstVerseNo;
        private bool _removeSpaceAfterVerse;
		private bool _directionEnd = false;
        #endregion

        #region Private Variables

        public string TocStartingPage
        {
            get { return _tocStartingPage; }
            set { _tocStartingPage = value; }
        }

        public string TocStyleName
        {
            get { return _tocStyleName; }
            set { _tocStyleName = value; }
        }

        public int TocPageStock
        {
            get { return _tocPageStock; }
            set { _tocPageStock = value; }
        }

        #endregion

        public Dictionary<string, Dictionary<string, string>> CreateContent(PublicationInformation projInfo, Dictionary<string, Dictionary<string, string>> cssClass, StreamWriter xetexFile, Dictionary<string, List<string>> classInlineStyle, Dictionary<string, ArrayList> classFamily, ArrayList cssClassOrder, Dictionary<string, List<string>> classInlineText, int pageWidth)
        {
            _projInfo = projInfo;
            _xetexFile = xetexFile;
            _classInlineStyle = classInlineStyle;
            _classInlineInnerStyle = classInlineText;
            _pageWidth = pageWidth;
            _inputPath = projInfo.ProjectPath;

            _dicMainReversal = Path.GetFileNameWithoutExtension(projInfo.DefaultXhtmlFileWithPath);

            xhtmlFile = projInfo.DefaultXhtmlFileWithPath;
            Dictionary<string, Dictionary<string, string>> idAllClass = new Dictionary<string, Dictionary<string, string>>();
            InitializeData(projInfo.ProjectPath, cssClass, classFamily, cssClassOrder);
            InitializeMathStyle();
            ProcessProperty();
            ProcessCounterProperty();
            OpenXhtmlFile(projInfo.DefaultXhtmlFileWithPath);
            ProcessXHTML(projInfo.DefaultXhtmlFileWithPath);

            CloseFile();
            GetTableofContent();
            return _newProperty;
        }

        private void GetTableofContent()
        {

            if (_projInfo.ProjectInputType.ToLower() == "scripture")
            {
                _newProperty.Add("TableofContent", _toc);
            }
            else
            {
                _newProperty.Add("TableofContent", _toc);
            }
        }

        private void ProcessProperty()
        {
            Common.ColumnWidth = 0.0;

            foreach (string className in IdAllClass.Keys)
            {
                string searchKey = "column-count";
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
                            i++;
                            i++;
                            string value = values[i].Replace("\"", "");
                            value = Common.ReplaceSymbolToText(value);
                            _replaceSymbolToText[key] = Common.UnicodeConversion(value);
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
            }

            if (Common.ColumnWidth == 0.0)
            {
                Common.ColumnWidth = Common.GetPictureWidth(IdAllClass, _projInfo.ProjectInputType);
            }
        }

        private void ProcessCounterProperty()
        {
            _FootNote.Clear();
            foreach (string className in IdAllClass.Keys)
            {
                string searchKey = "counter-increment";
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

                //// Footnote process 
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
                string searchKey1 = "..footnote-call";
                if (className.IndexOf(searchKey1) >= 0)
                {
                    if (!_footnoteCallContent.Contains(className))
                        _footnoteCallContent.Add(className);
                }
            }
        }

        private void ProcessXHTML(string xhtmlFileWithPath)
        {
            try
            {
                _reader = Common.DeclareXmlTextReader(xhtmlFileWithPath, true);
                bool headXML = true;
                while (_reader.Read())
                {
                    if (IsEmptyNode())
                    {
                        continue;
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

                    if (_reader.Name == "img")
                    {
                        _hasImgCloseTag = true;
                    }
                    if (_reader.IsEmptyElement)
                    {
                        if (_reader.Name == "img")
                        {
                            _hasImgCloseTag = false;
                        }
                        if (_reader.Name == "br")
                        {
                            _xetexFile.Write(" \\\\ ");
                            continue;
                        }
                    }

                    switch (_reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            StartElement();
                            break;
                        case XmlNodeType.Text:
                            Write();
                            break;
                        case XmlNodeType.EndElement:
                            EndElement();
                            break;
                        case XmlNodeType.SignificantWhitespace:
                            InsertWhiteSpace();
                            break;
                    }
                }
            }
            catch (XmlException e)
            {
                var msg = new[] { e.Message, xhtmlFileWithPath };
            }
        }

        private void Write()
        {

            if (_isNewParagraph)
            {
                if (_paragraphName == null)
                {
                    _paragraphName = StackPeek(_allParagraph); // _allParagraph.Pop();
                }

                ClosePara(false);

                DoNotInheritClassStart();

                AddUsedStyleName(_paragraphName);

                _previousParagraphName = _paragraphName;
                _paragraphName = null;
                _isNewParagraph = false;
                _isParagraphClosed = false;
                _xetexNewLine = true;
            }

            if (_previousParagraphName.ToLower().IndexOf("scrbook") == 0)
            {
                if (_bookName.Trim().Length == 0)
                    _bookName = _reader.Value;
            }
            WriteText();
            isFileEmpty = false;
        }

        private void DoNotInheritClassStart()
        {
            if (_doNotInheritClass.Length == 0) return;

            string property = "SpaceBefore";
            if (IdAllClass.ContainsKey(_doNotInheritClass) == false)
            {
                return;
            }

            if (IdAllClass[_doNotInheritClass].ContainsKey(property))
            {
                string value = IdAllClass[_doNotInheritClass][property];
                _newProperty[_paragraphName][property] = value;
            }
            _doNotInheritProperty.Push(_doNotInheritOriginalClass);
            _doNotInheritOriginalClass = string.Empty;
            _doNotInheritClass = string.Empty;
        }

        private void DoNotInheritClassEnd(string closeChildName)
        {
            if (_doNotInheritProperty.Count == 0) return;

            string className = _doNotInheritProperty.Peek();
            string property = "SpaceAfter";

            if (closeChildName == className)
            {
                if (IdAllClass[className].ContainsKey(property))
                {
                    string value = IdAllClass[className][property];
                    _newProperty[_previousParagraphName][property] = value;
                }
                _doNotInheritProperty.Pop();
            }
        }

        private void WriteText()
        {
            string content = _reader.Value;
            content = ReplaceString(content);

            if (CollectFootNoteChapterVerse(content, Common.OutputType.XELATEX.ToString())) return;

            // Psuedo Before
            foreach (ClassInfo psuedoBefore in _psuedoBefore)
            {
                if (psuedoBefore.Content == "2023")
                    psuedoBefore.Content = @"\char""2665";
                WriteCharacterStyle(psuedoBefore.Content, psuedoBefore.StyleName);
            }

            // Text Write
            if (_characterName == null)
            {
                _characterName = StackPeekCharStyle(_allCharacter);
            }

            if (_psuedoContainsStyle != null)
            {
                if (content.IndexOf(_psuedoContainsStyle.Contains) > -1)
                {
                    content = _psuedoContainsStyle.Content;
                    _characterName = _psuedoContainsStyle.StyleName;
                }
            }

            if (!_bookPageBreak && (_previousParagraphName.IndexOf("TitleMain") == 0 || _previousParagraphName.IndexOf("TitleSecondary") == 0))
            {
                _bookPageBreak = true;
                if (_bookCount != 0)
                {
                    content = "\\newpage \r\n" + content;
                }
                _bookCount++;
            }
            string modifiedContent = ModifiedContent(content, _previousParagraphName, _characterName);
            WriteCharacterStyle(modifiedContent, _characterName);

            _psuedoBefore.Clear();
        }

        /// <summary>
        /// To replace the symbol string if the symbol matches with the text
        /// </summary>
        /// <param name="data">XML Content</param>
        private string ReplaceString(string data)
        {
            if (!_replaceSymbolToText.ContainsKey(" // "))
            {
                _replaceSymbolToText.Add(" // ", " \\linebreak  ");
            }

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

        private string whiteSpacePre(string content)
        {
            string whiteSpacePre = GetPropertyValue(_classNameWithLang, "white-space", string.Empty);
            if (whiteSpacePre != "pre")
            {

                content = content.Replace("\r\n", " ");
                content = content.Replace("\t", " ");
                Char[] charac = content.ToCharArray();
                StringBuilder builder = new StringBuilder();
                foreach (char var in charac)
                {
                    if (var == ' ' || var == '\b')
                    {
                        if (_isWhiteSpace)
                        {
                            continue;
                        }
                        _isWhiteSpace = true;
                    }
                    else
                    {
                        _isWhiteSpace = false;
                    }
                    builder.Append(var);
                }
                content = builder.ToString();
            }
            // No space after versenumber
            if (_removeSpaceAfterVerse && _classNameWithLang.IndexOf("VerseNumber") == 0)
            {
                _isWhiteSpace = true;
            }
            return content;
        }

        private void WriteCharacterStyle(string content, string characterStyle)
        {
            SetHomographNumber(false);

            

            string footerClassName = string.Empty;
            if (_isDropCaps)
            {
                _xetexFile.Write("\\lettrine{");
                _nextContent = true;
                _inlineCount++;
            }

            footerClassName = WritePara(characterStyle, content);

            AnchorBookMark();

            if (isFootnote)
            {
                footerClassName = Common.LeftString(characterStyle, "_");
                WriteFootNoteMarker(footerClassName, content);
            }
            else
            {
                content = WriteCounter(content);
                content = whiteSpacePre(content);
                LanguageFontCheck(content, _childName);
                _childName = Common.ReplaceSeperators(_childName);
                content = Common.ReplaceSymbolToXelatexText(content);
                List<string> value = CreateInlineInnerStyle(characterStyle);



                if (_childName.IndexOf("scrBookName") == 0 && content != null)
                {
                    _tocStartingPage = content;
                    _tocStartingPage = _tocStartingPage.Replace("~", "\\textasciitilde{~}");
                    TocPageStock++;
                    _toc.Add("PageStock_" + _dicMainReversal + TocPageStock.ToString(), "\\" + _childName + "{" + _tocStartingPage + "}");
                    _xetexFile.Write("\r\n \\label{PageStock_" + _dicMainReversal + TocPageStock.ToString() + "} ");
                }


                content = content.Replace("~", "\\textasciitilde{~}");

                if(_hideFirstVerseNo && _childName.ToLower().IndexOf("verse") == 0)
                {
                    content = string.Empty;
                }
                if (_characterName != null && IdAllClass.ContainsKey(_characterName) && IdAllClass[_characterName].ContainsKey("direction"))
                {
                    if (IdAllClass[_characterName]["direction"] == "rtl")
                    {
                        _xetexFile.Write("\\RL{");
                        _directionEnd = true;
                    }
                }
                _xetexFile.Write(content);
                if (_directionEnd)
                {
                    string directionEnd = "}";
                    _xetexFile.Write(directionEnd);
                    _directionEnd = false;
                }
                CloseInlineInnerStyle(value);
                for (int i = 1; i <= _inlineCount; i++) // close braces for inline style
                {
                    _xetexFile.Write("}");

                }
                _inlineCount = 0;

                _xetexFile.Write("}");

                
                if (_incrementDropCap != 0)
                {
                    _xetexFile.Write("}");
                    _incrementDropCap = 0;
                    _xetexFile.Write(_headerContent);
                    _headerContent = string.Empty;
                }

                if (_nextContent && _isDropCaps)
                {
                    _xetexFile.Write("}{");
                    _isDropCaps = false;
                    _incrementDropCap++;
                }
                string classNameWOLang = _classNameWithLang;
                if (classNameWOLang.IndexOf("_.") > 0)
                    classNameWOLang = Common.RightString(classNameWOLang, ".");
                string letterletHeadStyle = "letter";
                if (classNameWOLang != null || classNameWOLang != string.Empty)
                {
                    letterletHeadStyle = letterletHeadStyle + classNameWOLang;
                }
                letterletHeadStyle = letterletHeadStyle.Replace("-", "") + "letHead";
                if (_childName.IndexOf(letterletHeadStyle) == 0 && content != null)
                {
                    _tocStartingPage = content;
                    _tocStartingPage = _tocStartingPage.Replace("~", "\\textasciitilde{~}");
                    TocPageStock++;
                    _toc.Add("PageStock_" + _dicMainReversal + TocPageStock.ToString(), "\\" + _childName + "{" + _tocStartingPage + "}");
                    _xetexFile.Write("\r\n \\label{PageStock_" + _dicMainReversal + TocPageStock.ToString() + "} ");
                }
            }
            AnchorBookMark();
        }

        private void CloseInlineInnerStyle(List<string> value)
        {
            if (value.Count > 0)
            {
                for (int i = 0; i < value.Count; i++)
                {
                    if (value[i].IndexOf("$") == -1)
                        _xetexFile.Write("}");
                    else
                        _xetexFile.Write("}$");
                }
            }
        }

        private List<string> CreateInlineInnerStyle(string characterStyle)
        {
            if (characterStyle == "$ID/[No character style]")
            {
                characterStyle = StackPeek(_allStyle);
            }
            List<string> value = new List<string>();
            if (characterStyle.IndexOf("_") > 0)
            {
                string className = characterStyle.Substring(0, characterStyle.IndexOf("_"));
                if (_classInlineInnerStyle.ContainsKey(className))
                {
                    value = _classInlineInnerStyle[className];
                    for (int i = 0; i < value.Count; i++)
                    {
                        _xetexFile.Write(value[i]);
                        if (value[i].IndexOf("$") == -1)
                            _xetexFile.Write("{");
                    }
                }
            }
            return value;
        }

        private string WritePara(string characterStyle, string content)
        {
            string footerClassName = string.Empty;
            if (isFootnote)
            {
                footerClassName = Common.LeftString(characterStyle, "_");
            }
            else // regular style
            {

                //Note - Xetex paragraph new line
                if (_xetexNewLine)
                {
                    _xetexFile.Write("\r\n");
                    _xetexNewLine = false;
                }

                string paraStyle;

                if (characterStyle.IndexOf("No character style") > 0)
                {
                    paraStyle = _previousParagraphName;
                }
                else
                {
                    paraStyle = characterStyle;
                }
                string mergedParaStyle = MergeInlineStyle(paraStyle);

                string getStyleName = StackPeek(_allStyle);
                if (_classInlineStyle.ContainsKey(mergedParaStyle))
                {
                    List<string> inlineStyle = _classInlineStyle[mergedParaStyle];
                    int paraStyleCount = 0;
                    foreach (string property in inlineStyle)
                    {
                        string propName = Common.LeftString(property, " ");
                        if (_paragraphPropertyList.Contains(propName))
                        {
                            paraStyleCount++;
                            continue;
                        }
                        if (_mathStyle.Contains(property))
                        {
                            _xetexFile.Write("$");
                            _mathStyleClass.Push(getStyleName);
                        }
                        _xetexFile.Write(property);
                        _xetexFile.Write("{");
                    }
                    _inlineCount = inlineStyle.Count - paraStyleCount;
                    mergedParaStyle = Common.ReplaceSeperators(mergedParaStyle);
                    if (_projInfo.ProjectInputType.ToLower() == "scripture")
                    {
                        string headerStyle = Common.ReplaceSeperators(_previousParagraphName);
                        string referenceFormat = _projInfo.HeaderReferenceFormat;
                        if (mergedParaStyle.ToLower().IndexOf("chapter") == 0)
                        {
                            _chapterStyleforHeader = mergedParaStyle;
                        }
                        if (mergedParaStyle.ToLower().IndexOf("chapter") == 0 && (referenceFormat == "Genesis 1" || referenceFormat == "Gen 1"))
                        {
                            if (_headerContent.Trim().Length == 0)
                            {
                                if (referenceFormat == "Genesis 1")
                                {
                                    _headerContent = _bookName + " " + _chapterNo;
                                }
                                else if (referenceFormat == "Gen 1")
                                {
                                    _headerContent = _bookName.Substring(0, 3) + " " + _chapterNo;
                                }
                            }
                            _tocStyleName = mergedParaStyle;
                            string headerFormat = "\\markboth{ \\" + headerStyle + " " + _headerContent + "}{ \\" + headerStyle + " " + _headerContent + "}";
                            headerFormat = headerFormat.Replace("~", "\\textasciitilde{~}");
                            _headerContent = headerFormat;
                        }
                        else if (mergedParaStyle.ToLower().IndexOf("verse") == 0 && (referenceFormat == "Genesis 1:1" || referenceFormat == "Gen 1:1" || referenceFormat == "Genesis 1:1-2:1"))
                        {
                            if (_headerContent.Trim().Length == 0)
                            {
                                if (referenceFormat == "Genesis 1:1")
                                {
                                    _headerContent = _bookName + " " + _chapterNo + ":" + _verseNo;
                                }
                                else if (referenceFormat == "Gen 1:1")
                                {
                                    _headerContent = _bookName.Substring(0, 3) + " " + _chapterNo + ":" + _verseNo;
                                }
                                else if (referenceFormat == "Genesis 1:1-2:1")
                                {
                                    _headerContent = _bookName + " " + _chapterNo + ":" + _verseNo + "-" + _chapterNo + ":" + _verseNo;
                                }

                            }
                            _tocStyleName = mergedParaStyle;
                            string headerFormat = "\\markboth{ \\" + headerStyle + " " + _headerContent + "}{ \\" + headerStyle + " " + _headerContent + "}";
                            headerFormat = headerFormat.Replace("~", "\\textasciitilde{~}");
                            _xetexFile.Write(headerFormat);
                            _headerContent = string.Empty;
                        }
                    }
                    else
                    {
                        string styleFullName = string.Empty;
                        string[] rearrangeStyleName = characterStyle.Split('_');
                        foreach (string currString in rearrangeStyleName)
                        {
                            if (!currString.Contains("."))
                            {
                                styleFullName += currString;
                            }
                        }

                        if (mergedParaStyle.IndexOf("headword") == 0 && content != null)
                        {
                            _headerContent = content;

                            _tocStyleName = mergedParaStyle;
                            string headerFormat = "\\markboth{ \\" + mergedParaStyle + " " + _headerContent + "}{ \\" + mergedParaStyle + " " + _headerContent + "}";
                            headerFormat = headerFormat.Replace("~", "\\textasciitilde{~}");
                            if (_isHeadwordInsertedAfterImage)
                            {
                                string hangingLength = string.Empty;
                                foreach (string property in _classInlineStyle["entry"])
                                {
                                    if (property.Contains("text-indent"))
                                    {
                                        hangingLength = property.Replace("text-indent", "");
                                        hangingLength = "\r\n\\hangindent=" + hangingLength + "\r\n\\hangafter=1";
                                    }
                                }
                                headerFormat = hangingLength + headerFormat;
                                _isHeadwordInsertedAfterImage = false;
                            }
                            _xetexFile.Write(headerFormat);
                            _headerContent = content;
                        }
                        if (styleFullName.Contains("spanreversalform") && styleFullName.Contains("entryletDatadicBody") && content != null)
                        {
                            _headerContent = content;

                            _tocStyleName = mergedParaStyle;
                            string headerFormat = "\\markboth{ \\" + mergedParaStyle + " " + _headerContent + "}{ \\" + mergedParaStyle + " " + _headerContent + "}";
                            headerFormat = headerFormat.Replace("~", "\\textasciitilde{~}");
                            _xetexFile.Write(headerFormat);
                            _headerContent = content;
                        }
                    }

                    _xetexFile.Write("\\" + mergedParaStyle + "{");
                }
                AddUsedStyleName(characterStyle);
            }
            return footerClassName;
        }

        private string MergeInlineStyle(string paraStyle)
        {
            paraStyle = paraStyle.Replace("_body", "");
            string parentClass = paraStyle;
            string mergedClass = paraStyle.Replace("_", "");
            string[] parent = paraStyle.Split('_');
            if (parent.Length > 0)
            {
                string childClass = Common.LeftString(paraStyle, "_");
                parentClass = paraStyle.Replace(childClass + "_", "");
                parentClass = parentClass.Replace("_", "");
                if (_classInlineStyle.ContainsKey(mergedClass))
                {
                    return mergedClass;
                }
                _mergedInlineStyle = new List<string>();
                //Copy
                if (_classInlineStyle.ContainsKey(childClass))
                {
                    foreach (string sty in _classInlineStyle[childClass])
                    {
                        _mergedInlineStyle.Add(sty);
                    }
                }
                else if (parent.Length > 1 && _classInlineStyle.ContainsKey(parent[1]))
                {
                    foreach (string sty in _classInlineStyle[parent[1]])
                    {
                        if (!_mergedInlineStyle.Contains(sty))
                            _mergedInlineStyle.Add(sty);
                    }
                }

                //Merge
                if (_classInlineStyle.ContainsKey(parentClass))
                {
                    foreach (string sty in _classInlineStyle[parentClass])
                    {
                        string parentProp = Common.LeftString(sty, " ");
                        if (_paragraphPropertyList.Contains(parentProp)) continue; // skip parent paragraph property 
                        bool IsContains = false;
                        foreach (string styl in _mergedInlineStyle)
                        {
                            string ChildProp = Common.LeftString(styl, " ");
                            if (parentProp == ChildProp)
                            {
                                IsContains = true;
                                break;
                            }
                        }
                        if (!IsContains)
                            _mergedInlineStyle.Add(sty);
                    }
                }
                _classInlineStyle[mergedClass] = _mergedInlineStyle;
            }
            return mergedClass;
        }

        private void WriteFootNoteMarker(string footerClassName, string content)
        {
            string markerClassName = footerClassName + "..footnote-call";
            if (_footnoteMarkerClass.StyleName == markerClassName)
            {
                string styleName = _childName;
                styleName = Common.ReplaceSeperators(styleName);
                if (_removeSpaceAfterVerse)
                {
                    _xetexFile.Write(" ");
                }
                _xetexFile.Write("\\footnote {\\" + styleName + "{" + content + "}}");
            }
        }

        private void AnchorBookMark()
        {
            if (_anchorBookMarkName != string.Empty)
            {
                string status = _anchorBookMarkName.Substring(0, 4);
                if (status == "href")
                {
                    string bookMark = _anchorBookMarkName.Replace("href#", "");
                    _anchorBookMarkName = "endbookmark";
                }
                else if (status == "name")
                {
                    _crossRefCounter++;
                    string bookMark = _anchorBookMarkName.Replace("name", "");
                    if (!_crossRef.Contains(bookMark))
                        _crossRef.Add(bookMark);
                    _anchorBookMarkName = string.Empty;
                }
                else if (status == "endb")
                {
                    _anchorBookMarkName = string.Empty;
                }
            }
        }

        public bool InsertImage()
        {
            int defaultWidht, defaultHeight;
            defaultWidht = defaultHeight = 72;
            bool inserted = false;
            if (_imageInsert)
            {
                //1 inch = 72 PostScript points
                string alignment = "Center";
                string wrapSide = "none";
                string rectHeight = "0";
                string rectWidth = "0";
                string srcFile;
                string wrapMode = "BoundingBoxTextWrap";
                string HoriAlignment = "LeftAlign";
                const string HoriRefPoint = "ColumnEdge";
                string VertAlignment = "CenterAlign";
                string VertRefPoint = "LineBaseline";
                string AnchorPoint = "TopLeftAnchor";

                isImageAvailable = true;
                inserted = true;

                string[] cc = _allStyle.ToArray();
                imageClass = cc[0];

                srcFile = _imageSource.ToLower();

                string fromPath = Common.GetPictureFromPath(srcFile, "", _inputPath);
                string clsName = _allStyle.Peek();
                if (IdAllClass.ContainsKey(cc[0]))
                {
                    rectHeight = GetPropertyValue(srcFile, "height", rectHeight);
                    rectWidth = GetPropertyValue(srcFile, "width", rectWidth);

                    if (rectHeight == "0" && rectWidth == "0")
                    {
                        clsName = _childName;
                        rectHeight = GetPropertyValue(clsName, "height", rectHeight);
                        rectWidth = GetPropertyValue(clsName, "width", rectWidth);
                    }

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

                    rectWidth = Convert.ToString(Common.ColumnWidth * .9);
                    rectHeight = Common.CalcDimension(fromPath, ref rectWidth, 'H');
                }
                else
                {
                    //Default value is 72 
                    rectHeight = defaultHeight.ToString(); // fixed the width as 1 in = 72pt;
                    rectWidth = Common.CalcDimension(fromPath, ref rectHeight, 'W');
                }
                if (rectWidth == "0")
                {
                    rectWidth = defaultWidht.ToString();
                }
                if (rectHeight == "0")
                {
                    rectHeight = defaultHeight.ToString();
                }

                if (rectWidth.IndexOf("%") != -1)
                {
                    rectWidth = defaultWidht.ToString();
                }

                if (rectHeight.IndexOf("%") != -1)
                {
                    rectHeight = defaultHeight.ToString();
                }

                double x = double.Parse(rectWidth, CultureInfo.GetCultureInfo("en-US")) / 3;
                double y = double.Parse(rectHeight, CultureInfo.GetCultureInfo("en-US")) / 3;

                string xPlus = x.ToString();
                string xMinus = "-" + xPlus;

                string yPlus = y.ToString();
                string yMinus = "-" + yPlus;

                int height = Convert.ToInt32(y); //1 in
                int width = Convert.ToInt32(x); // 1 in

                string picFile = string.Empty;
                //TODO Make it function 
                //To get Image details
                if (File.Exists(fromPath))
                {
                    picFile = Path.GetFileName(fromPath);
                    string toPath = Common.PathCombine(_inputPath, picFile);
                    string installedDirectory = XeLaTexInstallation.GetXeLaTexDir();
                    string destination = string.Empty;

                    if (Common.IsUnixOS())
                    {
                        toPath = _inputPath;
                        if (picFile.IndexOf(".tif") >= 0)
                        {
                            if (picFile != null)
                                picFile = picFile.Replace(".tif", ".jpg");
                        }
                        destination = Common.PathCombine(_inputPath, Path.GetFileName(picFile));
                        installedDirectory = _inputPath;
                    }
                    else
                    {
                        installedDirectory = Common.PathCombine(installedDirectory, "bin");
                        installedDirectory = Common.PathCombine(installedDirectory, "win32");
                        toPath = installedDirectory;
                        destination = Common.PathCombine(installedDirectory, Path.GetFileName(picFile));
                    }

                    if (!File.Exists(destination))
                    {
                        if (fromPath.IndexOf(".tif") >= 0)
                        {
                            toPath = Common.ConvertTifftoImage(fromPath, "jpg");
                            if (picFile != null) picFile = picFile.Replace(".tif", ".jpg");
                            if (destination != null) destination = destination.Replace(".tif", ".jpg");
                            fromPath = toPath;
                            toPath = installedDirectory;
                        }
                        if (!string.IsNullOrEmpty(toPath))
                        {
                            if (fromPath != Common.PathCombine(toPath, Path.GetFileName(fromPath)))
                                File.Copy(fromPath, Common.PathCombine(toPath, Path.GetFileName(fromPath)), true);

                        }
                    }
                    else
                    {
                        if (picFile.IndexOf(".tif") >= 0)
                        {
                            if (picFile != null) picFile = picFile.Replace(".tif", ".jpg");
                        }
                    }
                    Dictionary<string, string> prop = new Dictionary<string, string>();
                    prop.Add("filePath", toPath);
                    _newProperty["ImagePath"] = prop;
                }

                WriteImage(picFile, height, width);

                _imageInsert = false;
                _imageSource = string.Empty;
                _isNewParagraph = false;
                _isParagraphClosed = true;
            }
            return inserted;
        }

        private void WriteImage(string picFile, int height, int width)
        {
            if (!string.IsNullOrEmpty(picFile))
            {
                isImageAvailable = true;

                _xetexFile.WriteLine("\r\n");

                string p1 = @"\begin{wrapfigure}";
                string p2 = @"\begin{center}";
                string p3 = @"\includegraphics[angle=0,width=" + width + "mm,height=" + height + "mm]{" + picFile + "} ";
                _xetexFile.WriteLine(p1);
                _xetexFile.WriteLine(p2);
                _xetexFile.WriteLine(p3);
                string p4 = @"\caption{}";
                string p5 = @"\end{center}";
                string p6 = @"\end{wrapfigure}";
                _xetexFile.WriteLine(p4);
                _xetexFile.WriteLine(p5);
                _xetexFile.WriteLine(p6);
                _xetexFile.WriteLine("\r\n");
            }
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

        private string GetPropertyValue(string clsName, string property, string defaultValue)
        {
            string valueOfProperty = defaultValue;
            string excludeParent = Common.LeftString(clsName, "_");
            if (IdAllClass.ContainsKey(excludeParent) && IdAllClass[excludeParent].ContainsKey(property))
            {
                valueOfProperty = IdAllClass[excludeParent][property];
            }
            else if (IdAllClass.ContainsKey(clsName) && IdAllClass[clsName].ContainsKey(property))
            {
                valueOfProperty = IdAllClass[clsName][property];
            }
            return valueOfProperty;
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
            if (_allParagraph.Count > 0)
            {
                string[] splitedClassName = _allParagraph.ToArray(); ; // _allStyle.ToArray();

                if (splitedClassName.Length > 0)
                {
                    for (int i = 0; i < splitedClassName.Length; i++) // // From Recent to Begining Class
                    {
                        string clsName = splitedClassName[i];
                        string pos = GetPropertyValue(clsName, "float", alignment);
                        switch (pos.ToLower())
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
                            case "center":
                                AnchorPoint = "TopCenterAnchor";
                                HoriAlignment = "CenterAlign";
                                VertAlignment = "CenterAlign";
                                VertRefPoint = "LineBaseline";
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
                        wrapSide = GetPropertyValue(clsName, "clear", wrapSide);
                        if (pos != "left" && wrapSide != "none")
                        {
                            break;
                        }
                        return;
                    }
                }
            }
        }

        private void StartElement()
        {
            SetHeadwordTrue();
            StartElementBase(_IsHeadword);
            _imageInserted = InsertImage();
            ListBegin();
            SetClassCounter();
            Psuedo();
            DropCaps();
            SetHomographNumber(true);
            FooterSetup(Common.OutputType.XELATEX.ToString());

            WriteParagraphInline();

            if (IdAllClass.ContainsKey(_classNameWithLang))
            {
                bool isPageBreak = false;
                if (IdAllClass[_classNameWithLang].ContainsKey("PageBreakBefore"))
                {
                    isPageBreak = IdAllClass[_classNameWithLang]["PageBreakBefore"] == "always";
                }
                if (IdAllClass[_classNameWithLang].ContainsKey("column-count"))
                {
                    columnCount = IdAllClass[_classNameWithLang]["column-count"];
                }
                if (isPageBreak || columnCount != string.Empty)
                {
                    _textFrameClass.Add(_childName);
                }
            }
        }

        private void ListBegin()
        {
            if (_tagType == "ol")
            {
                _xetexFile.WriteLine();
                _xetexFile.WriteLine("\\begin{enumerate}");
            }
            else if (_tagType == "ul")
            {
                _xetexFile.WriteLine();
                _xetexFile.WriteLine("\\begin{itemize}");
            }
            else if (_tagType == "li")
            {
                _xetexFile.Write("\\item");
            }
        }

        private void WriteParagraphInline()
        {
            string getStyleName = StackPeek(_allStyle);

            string paraStyle = _childName.Replace("_body", ""); ;
            string childClass = Common.LeftString(paraStyle, "_");
            string directionStart = string.Empty;
            string directionEnd = string.Empty;
            if (_divType.Contains(_tagType) && _classInlineStyle.ContainsKey(childClass))
            {
                string endParagraphString = string.Empty;

                string mdFrameStart = string.Empty;
                string txtAlignStart = string.Empty;
                string txtAlignEnd = string.Empty;
                string txtLineSpaceStart = string.Empty;
                string txtLineSpaceEnd = string.Empty;
                string columnStart = string.Empty;
                string paddingLeft = string.Empty;
                string paddingRight = string.Empty;
                string paddingTop = string.Empty;
                string paddingBottom = string.Empty;
                string displayNoneStart = string.Empty;
                string displayNoneEnd = string.Empty;
                string widows = string.Empty;
                string orphans = string.Empty;
                string textIndent = string.Empty;

                foreach (string property in _classInlineStyle[childClass])
                {
                    string propName = Common.LeftString(property, " ");
                    if (_paragraphPropertyList.Contains(propName))
                    {
                        if (propName == "column-count" && property != "column-count 1")
                        {
                            if (IdAllClass[childClass].ContainsKey("column-gap") &&
                                IdAllClass[childClass]["column-gap"] != null)
                            {
                                string propertyValue = Common.PercentageToEm(IdAllClass[childClass]["column-gap"]);
                                columnStart = "\\setlength{\\columnsep}{" + propertyValue + "} \r\n";
                                columnStart = columnStart + "\\setlength\\columnseprule{" + "0.4pt" + "} \r\n";
                            }
                            columnStart = columnStart + "\\begin{multicols}{" + Common.RightString(property, " ") + "}";
                        }
                        else if (propName == "margin")
                        {
                            mdFrameStart += ", " + Common.RightString(property, " ");
                        }
                        else if (propName == "line-height")
                        {
                            string prop = Common.RightString(property, " ");
                            if (prop.Trim() != "0")
                            {
                                txtLineSpaceStart = "\\begin{spacing}{" + prop + "}";
                                txtLineSpaceEnd = "\\end{spacing}";
                            }

                        }
                        else if (propName == "text-align")
                        {

                            if (property.IndexOf("center") > 0)
                            {
                                txtAlignStart = "\\begin{" + Common.RightString(property, " ") + "}";
                                txtAlignEnd = "\\end{" + Common.RightString(property, " ") + "}";

                                if (isImageAvailable)
                                {
                                    isImageAvailable = false;
                                    txtAlignEnd = txtAlignEnd + " ";
                                }
                            }
                            else
                            {
                                txtAlignStart = "{\\" + Common.RightString(property, " ") + "} ";
                                txtAlignEnd = " ";
                            }
                        }
                        else if (propName.Contains("text-indent"))
                        {
                            if (property.Contains("text-indent"))
                            {
                                string hangingLength = property.Replace("text-indent", "");
                                txtAlignStart = "\r\n\\hangindent=" + hangingLength + "\r\n\\hangafter=1";
                            }
                        }
                        else if (propName == "display-none")
                        {
                            displayNoneStart = "\\begin{comment}";
                            displayNoneEnd = "\\end{comment}\r\n";
                        }
                        else if (propName == "RTL")
                        {
                            directionStart = "\\begin{RTLitems} \\item";
                            directionEnd = "\\end{RTLitems}";
                        }
                        else if (propName == "padding-left")
                        {
                            paddingLeft = Common.RightString(property, " ");
                        }
                        else if (propName == "padding-right")
                        {
                            paddingRight = Common.RightString(property, " ");
                        }
                        else if (propName == "padding-top")
                        {
                            paddingTop = Common.RightString(property, " ");
                        }
                        else if (propName == "padding-bottom")
                        {
                            paddingBottom = Common.RightString(property, " ");
                        }
                        else if (propName == "widows")
                        {
                            widows = "\\widowpenalty=300";
                        }
                        else if (propName == "orphans")
                        {
                            orphans = "\\clubpenalty=300";
                        }

                    }
                }


                // column -> mdframe -> text-align
                if (columnStart != string.Empty)
                {
                    _xetexFile.Write(columnStart);
                    endParagraphString = "\\end{multicols}";
                }

                if ((childClass.ToLower().IndexOf("line") == -1) && (paddingLeft.Length > 0 || paddingRight.Length > 0 || paddingTop.Length > 0 || paddingBottom.Length > 0))
                {
                    if (paddingLeft.Length == 0)
                    {
                        paddingLeft = "0pt";
                    }
                    if (paddingRight.Length == 0)
                    {
                        paddingRight = "0pt";
                    }
                    if (paddingTop.Length == 0)
                    {
                        paddingTop = "0pt";
                    }
                    if (paddingBottom.Length == 0)
                    {
                        paddingBottom = "0pt";
                    }
                }

                if (mdFrameStart != string.Empty)
                {
                    string prop = "{\\begin{mdframed}[linecolor=white";

                    _xetexFile.Write(prop);
                    _xetexFile.Write(mdFrameStart);
                    _xetexFile.Write("]");
                    endParagraphString = "\\end{mdframed}}" + endParagraphString;
                }


                if (txtLineSpaceStart != string.Empty)
                {
                    _xetexFile.Write(txtLineSpaceStart);
                    endParagraphString = txtLineSpaceEnd + endParagraphString;
                }

                if (txtAlignStart != string.Empty)
                {
                    _xetexFile.Write(txtAlignStart);
                    endParagraphString = txtAlignEnd + endParagraphString;
                }
                if (displayNoneStart != string.Empty)
                {
                    _xetexFile.WriteLine(displayNoneStart);
                    endParagraphString = displayNoneEnd + " " + endParagraphString;
                }
                if (directionStart != string.Empty)
                {
                    _xetexFile.WriteLine(directionStart);
                    endParagraphString = directionEnd + " " + endParagraphString;
                }
                if (endParagraphString != string.Empty)
                {
                    _braceInlineClassCount[getStyleName] = _classInlineStyle[childClass].Count;
                    _braceInlineClass.Push(getStyleName);

                    _endParagraphStringDic[getStyleName] = endParagraphString;
                }
                if (widows != string.Empty)
                {
                    _xetexFile.WriteLine(widows);
                }
                if (orphans != string.Empty)
                {
                    _xetexFile.WriteLine(orphans);
                }
                if (textIndent != string.Empty)
                {
                    _xetexFile.Write(textIndent);
                }
            }
            else if ((_tagType == "span") && _classInlineStyle.ContainsKey(childClass))
            {
                string endParagraphString = string.Empty;
                string displayNoneStart = string.Empty;
                string displayNoneEnd = string.Empty;

                foreach (string property in _classInlineStyle[childClass])
                {
                    string propName = Common.LeftString(property, " ");
                    if (_paragraphPropertyList.Contains(propName))
                    {
                        if (propName == "display-none")
                        {
                            displayNoneStart = "\\begin{comment}";
                            displayNoneEnd = "\\end{comment}\r\n";
                        }
                        else if (propName == "RTL")
                        {
                            directionStart = "\\begin{RTLitems} \\item";
                            directionEnd = "\\end{RTLitems}";
                        }
                    }
                }

                if (displayNoneStart != string.Empty)
                {

                    _xetexFile.WriteLine(displayNoneStart);
                    endParagraphString = displayNoneEnd + " " + endParagraphString;
                }
                if (directionStart != string.Empty)
                {
                    _xetexFile.WriteLine(directionStart);
                    endParagraphString = directionEnd + " " + endParagraphString;
                }
                if (endParagraphString != string.Empty)
                {
                    _braceInlineClassCount[getStyleName] = _classInlineStyle[childClass].Count;
                    _braceInlineClass.Push(getStyleName);
                    _endParagraphStringDic[getStyleName] = endParagraphString;
                }
            }
        }

        private void SetHeadwordTrue()
        {
            if (_reader.GetAttribute("class") != null && _reader.GetAttribute("class").ToLower() == "headword")
            {
                _IsHeadword = true;
                _headwordStyles = true;
            }
        }

        private void SetHomographNumber(bool defValue)
        {
            if (_classNameWithLang.IndexOf("homographnumber") >= 0)
            {
                isHomographNumber = defValue;
            }
        }


        private void DropCaps()
        {
            string classNameWOLang = _classNameWithLang;
            if (classNameWOLang.IndexOf("_.") > 0)
                classNameWOLang = Common.LeftString(classNameWOLang, "_.");

            if (IdAllClass.ContainsKey(classNameWOLang) && IdAllClass[classNameWOLang].ContainsKey("float") && IdAllClass[classNameWOLang].ContainsKey("vertical-align"))
            {
                _isDropCaps = true;
            }
        }

        private void DropCapsOLD()
        {
            string classNameWOLang = _classNameWithLang;
            if (classNameWOLang.IndexOf("_.") > 0)
                classNameWOLang = Common.LeftString(classNameWOLang, "_.");
            string inner = string.Empty;

            if (classNameWOLang == "ChapterNumber")
            {
                _chapterNo = _reader.ReadString();
            }
            if (IdAllClass.ContainsKey(classNameWOLang) && IdAllClass[classNameWOLang].ContainsKey("float") && IdAllClass[classNameWOLang].ContainsKey("vertical-align"))
            {
                Dictionary<string, string> mystyle = new Dictionary<string, string>();
                _isDropCap = true;
                string lines = "2";
                _allStyle.Pop();
                CollectFootNoteChapterVerse(_chapterNo, Common.OutputType.XELATEX.ToString());

                try
                {
                    if (IdAllClass[classNameWOLang].ContainsKey("PointSize") && IdAllClass[classNameWOLang]["PointSize"].IndexOf('%') > 0)
                    {
                        lines = (int.Parse(IdAllClass[classNameWOLang]["PointSize"].Replace("%", "")) / 100).ToString();
                    }
                }
                catch
                {
                }
                mystyle["DropCapCharacters"] = _chapterNo.Length.ToString();
                mystyle["DropCapLines"] = lines; // No of Lines.
                _paragraphName = classNameWOLang + _chapterNo.Length.ToString();
                _newProperty[_paragraphName] = mystyle;
                _dropCapStyle = _paragraphName;
                Write();

            }
        }

        private void EndElement()
        {
            _characterName = null;
            if (_hasImgCloseTag && _imageInserted)
            {
                _hasImgCloseTag = false;
            }
            else
            {
                _closeChildName = StackPop(_allStyle);
            }

            CloseBrace(_closeChildName);
            DoNotInheritClassEnd(_closeChildName);
            SetHeadwordFalse();
            ClosefooterNote();
            EndElementForImage();

            if (_closeChildName == string.Empty) return;
            if (_psuedoAfter.Count > 0)
            {
                if (_psuedoAfter.ContainsKey(_closeChildName))
                {
                    ClassInfo classInfo = _psuedoAfter[_closeChildName];
                    WriteCharacterStyle(classInfo.Content, classInfo.StyleName);
                    _psuedoAfter.Remove(_closeChildName);
                }
            }

            if (_reader.Name == "ul")
            {
                _xetexFile.WriteLine("\\end{itemize}");
            }
            else if (_reader.Name == "ol")
            {
                _xetexFile.WriteLine("\\end{enumerate}");
            }

            EndElementBase(false);

            if (_closeChildName.IndexOf("scrBookName") == 0)
            {
                _xetexFile.Write("\r\n \\label{PageStock_" + _dicMainReversal + TocPageStock.ToString() + "} ");
                _bookName = string.Empty;
                _bookPageBreak = false;
            }
            _classNameWithLang = StackPeek(_allStyle);
            _classNameWithLang = Common.LeftString(_classNameWithLang, "_");
        }

        private void CloseBrace(string closeChildName)
        {
            string closeStyle = StackPeek(_braceInlineClass);
            if (closeChildName == closeStyle && _endParagraphStringDic.ContainsKey(closeStyle))
            {
                _xetexFile.Write(_endParagraphStringDic[closeStyle]);
                _endParagraphStringDic[closeStyle] = string.Empty;
                StackPop(_braceInlineClass);
            }

            string dollar = StackPeek(_mathStyleClass);
            if (dollar.Length != 0 && closeChildName == dollar)
            {
                _xetexFile.Write("$");
                StackPop(_mathStyleClass);
            }
        }

        private void SetHeadwordFalse()
        {
            if (_closeChildName.ToLower() == "headword")
            {
                _IsHeadword = false;
                _headwordStyles = false;
            }
        }

        /// <summary>
        /// Closes the opened footnote Content, Chapter No and VerseNo
        /// </summary>
        private void ClosefooterNote()
        {
            if (_closeChildName.Length > 0 && _closeChildName == footnoteClass)
            {
                WriteCharacterStyle(footnoteContent.ToString(), footnoteClass);
                isFootnote = false;
                footnoteContent.Remove(0, footnoteContent.Length);
            }
        }

        /// <summary>
        /// Closing all the tages for Image
        /// </summary>
        private void EndElementForImage()
        {
            if (!_imageInsert && _imageInserted)
            {
                if (_closeChildName == _imageClass) // Without Caption
                {
                    _allCharacter.Push(_imageClass); // temporarily storing to get width and position
                    _allCharacter.Pop();    // retrieving it again.
                    isImageAvailable = false;
                    imageClass = "";
                    _isParagraphClosed = true;

                }
            }
            else // With Caption
            {
                if (imageClass.Length > 0 && _closeChildName == imageClass)
                {
                    isImageAvailable = false;
                    _isHeadwordInsertedAfterImage = true;
                    imageClass = "";
                    _isParagraphClosed = false;
                }
            }
        }

        private void WriteEmptyHomographStyle()
        {
            if (isHomographNumber)
            {
                isHomographNumber = false;
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
        /// Store used Paragraph adn Character style name. This data used in ModifyIDStyle.cs
        /// </summary>
        /// <param name="styleName">a_b</param>
        private void AddUsedStyleName(string styleName)
        {
            if (!_usedStyleName.Contains(styleName))
                _usedStyleName.Add(styleName);
        }

        private void InitializeMathStyle()
        {
            //_mathStyle.Add("\\underline");
        }

        private void InsertWhiteSpace()
        {
            //if (!_isWhiteSpace && !_pseudoSingleSpace)
            if (!_isWhiteSpace)
            {
                _xetexFile.Write(" ");
                _isWhiteSpace = true;
            }
        }

        #region Private Methods

        private void InitializeData(string projectPath, Dictionary<string, Dictionary<string, string>> idAllClass, Dictionary<string, ArrayList> classFamily, ArrayList cssClassOrder)
        {
            _outputType = Common.OutputType.XELATEX;
            _allStyle = new Stack<string>();
            _allParagraph = new Stack<string>();
            _allCharacter = new Stack<string>();
            _doNotInheritProperty = new Stack<string>();

            _childStyle = new Dictionary<string, string>();
            IdAllClass = new Dictionary<string, Dictionary<string, string>>();
            _newProperty = new Dictionary<string, Dictionary<string, string>>();
            ParentClass = new Dictionary<string, string>();
            _displayBlock = new Dictionary<string, string>();
            _cssClassOrder = cssClassOrder;

            IdAllClass = idAllClass;
            _projectPath = projectPath;
            _classFamily = classFamily;

            _isNewParagraph = false;
            _characterName = "$ID/[No character style]";// "[No character style]"; 

            _paragraphPropertyList = new List<string>();
            ////Padding
            _paragraphPropertyList.Add("padding");
            _paragraphPropertyList.Add("padding-left");
            _paragraphPropertyList.Add("padding-right");
            _paragraphPropertyList.Add("padding-top");
            _paragraphPropertyList.Add("padding-bottom");

            //Text-Indent
            _paragraphPropertyList.Add("text-indent");

            //Display-None
            _paragraphPropertyList.Add("display-none");

            _paragraphPropertyList.Add("line-height");

            //TextAlign
            _paragraphPropertyList.Add("text-align");
            _paragraphPropertyList.Add("column-count");

            //Widows and Orphans
            _paragraphPropertyList.Add("widows");
            _paragraphPropertyList.Add("orphans");

            //Direction right to left
            _paragraphPropertyList.Add("RTL");

            if(IdAllClass.ContainsKey("@page"))
            {
                if(IdAllClass["@page"].ContainsKey("-ps-hide-versenumber-one"))
                {
                    _hideFirstVerseNo = bool.Parse(IdAllClass["@page"]["-ps-hide-versenumber-one"]);
                }
            }
        }

        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Close the Xhtml and CSS files.
        /// 
        /// <list> 
        /// </list>
        /// </summary>
        /// <returns> </returns>
        /// -------------------------------------------------------------------------------------------
        private void CloseFile()
        {
            if (_reader != null)
                _reader.Close();
        }
        #endregion
    }
}
