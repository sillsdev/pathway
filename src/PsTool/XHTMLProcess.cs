// --------------------------------------------------------------------------------------------
// <copyright file="XHTMLProcess.cs" from='2009' to='2014' company='SIL International'>
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
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
#endregion Using

namespace SIL.Tool
{
    public class XHTMLProcess
    {
        #region Public Variable

        protected XmlTextWriter _writer;
        protected XmlTextReader _reader;
        protected StreamWriter _xetexFile;
        protected PublicationInformation _projInfo;

        protected Stack<string> _allStyle;
        protected Stack<string> _allParagraph;
        protected Stack<string> _allCharacter;
        protected Stack<string> _doNotInheritProperty;
        protected string _doNotInheritOriginalClass = string.Empty;
        protected string _doNotInheritClass = string.Empty;
        protected Stack<ClassInfo> _allStyleInfo = new Stack<ClassInfo>();
        protected ClassAttrib _precedeClassAttrib = new ClassAttrib();
        protected ClassAttrib _xhtmlClassAttrib = new ClassAttrib();
        protected ClassAttrib _parentPrecedeClassAttrib = new ClassAttrib();

        protected Dictionary<string, string> _childStyle;
        protected Dictionary<string, ClassInfo> _existingPsuedoBeforeStyle = new Dictionary<string, ClassInfo>();
        protected ClassInfo _psuedoBeforeStyle = new ClassInfo();
        protected Dictionary<string, ClassInfo> _existingPsuedoAfterStyle = new Dictionary<string, ClassInfo>();
        protected ClassInfo _psuedoAfterStyle = new ClassInfo();
        protected Dictionary<string, ClassInfo> _existingPsuedoContainsStyle = new Dictionary<string, ClassInfo>();
        protected ClassInfo _psuedoContainsStyle = new ClassInfo();
        private string _matchedCssStyleName;
        protected Dictionary<string, string> _tempStyle;
        protected string footerMarkerClassName = string.Empty;

        protected Dictionary<string, Dictionary<string, string>> IdAllClass;
        protected Dictionary<string, Dictionary<string, string>> _newProperty;
        protected Dictionary<string, string> ParentClass;
        protected Dictionary<string, string> _listTypeDictionary = new Dictionary<string, string>();
        protected Dictionary<string, ArrayList> _classFamily;
        //protected ClassInfo _classInfo = new ClassInfo();
        protected Dictionary<string, Dictionary<string, string>> _dictColumnGapEm = new Dictionary<string, Dictionary<string, string>>();
        protected string _className;
        private string _isTagClass;
        private string _listType;

        #region Anchor
        protected string _anchorBookMarkName = string.Empty;
        protected string _anchorIdValue = string.Empty;
        protected bool _anchorStart;
        protected string _anchorTitleValue = string.Empty;
        #endregion

        protected string _childName = string.Empty;
        protected string _closeChildName = string.Empty;
        protected bool _isNewParagraph;
        protected bool _isParagraphClosed = true;
        protected List<string> _divType;
        protected bool _isFirstPictureWritten;
	    protected List<string> _parentPrecendeSpace = new List<string>();

        #region Footnote
        protected string _chapterNo = string.Empty;
        protected string _verseNo;
        protected bool isFootnote = false;
        protected string footnoteClass = string.Empty;
        protected StringBuilder footnoteContent = new StringBuilder();
        protected ArrayList _FootNote = new ArrayList();
        protected ArrayList _footnoteCallContent = new ArrayList();
        protected ArrayList _footnoteMarkerContent = new ArrayList();
        protected ClassInfo _footnoteMarkerClass = new ClassInfo();

        protected Dictionary<string, string> _footNoteMarker = new Dictionary<string, string>();
        #endregion
        #region counter
        public Dictionary<string, Dictionary<string, string>> contentCounterIncrement = new Dictionary<string, Dictionary<string, string>>();
        public Dictionary<string, string> ContentCounterReset = new Dictionary<string, string>();
        public Dictionary<string, int> ContentCounter = new Dictionary<string, int>();
        #endregion
        #region Image
        protected bool _imageInsert;
        protected string _imageSource;
        protected string _imageLongDesc;
        protected string _imageClass = string.Empty;
        protected string _imageSrcClass;
        protected string _imageAltText;
        protected bool _isAutoWidthforCaption;
        protected bool _forcedPara;
        #endregion

        protected string _paragraphName;
        protected string _previousParagraphName;
        protected string _previousChildName = string.Empty;
        protected string _characterName;
        public string _characterNameAlways = string.Empty;
        protected string _lang = string.Empty;
        protected Dictionary<string, string> _languageStyleName = new Dictionary<string, string>();
        ArrayList _xhtmlAttribute = new ArrayList();
        protected ArrayList _cssClassOrder = new ArrayList();
        protected string _tagType;
        protected bool _isclassNameExist;
        protected string _projectPath;
        protected string _classNameWithLang;

        protected Common.OutputType _outputType;

        protected ArrayList _headwordStyleName = new ArrayList();
        protected bool _headwordStyles = false;
        private string _parentStyleName;
        protected ArrayList _visibilityClassName = new ArrayList();
        protected Dictionary<string, string> _replaceSymbolToText = new Dictionary<string, string>();

        protected List<string> _dropCap = new List<string>();
        protected bool _isDropCap;

        protected bool _isDisplayNone = false;
        protected string _displayNoneStyle = string.Empty;

        protected List<string> _usedStyleName = new List<string>();
        protected bool _textWritten = false;
        protected bool _imageInserted;
        protected bool _overWriteParagraph;

        protected List<string> LanguageFontStyleName = new List<string>();

        //Xetex
        protected List<string> _paragraphPropertyList;
        #endregion

        #region Constructor
        public XHTMLProcess()
        {
            //_cssClassOrder.Reverse();
        }
        #endregion

        protected void OpenXhtmlFile(string xhtmlFileWithPath)
        {
            _psuedoBeforeStyle = _psuedoAfterStyle = _psuedoContainsStyle = null;
        }

        protected bool IsEmptyNode()
        {
            bool result = false;
            if (_reader.IsEmptyElement)
            {
                if (_reader.Name == "br")
                {
                    return false;
                }
                else if (_reader.Name != "img")
                {
                    result = true;
                }
            }
            return result;
        }

        protected void StartElementBase(bool isHeadword)
        {
            string[] divTypeList = new[] { "div", "ol", "ul", "li", "p", "body", "h1", "h2", "h3", "h4", "h5", "h6", "table", "tr", "td", "th" };
            _divType = new List<string>(divTypeList);

            ClassInfo classInfo = new ClassInfo();
            _classNameWithLang = GetTagInfo();
            if (_outputType == Common.OutputType.XELATEX)
            {
                _classNameWithLang = Common.ReplaceCSSClassName(_classNameWithLang);
            }
            _xhtmlClassAttrib.SetClassAttrib(_className, _xhtmlAttribute);
            classInfo.CoreClass = _xhtmlClassAttrib;
            classInfo.Precede = _precedeClassAttrib;

            BlockInline();

            _childName = FindStyleName();
            GetHeadwordStyles(isHeadword);
            _allStyleInfo.Push(classInfo);

            if (_divType.Contains(_tagType))
            {
                _paragraphName = _childName;
                _allParagraph.Push(_paragraphName);
                _isNewParagraph = true;

                if (_tagType == "ol" || _tagType == "ul")
                {
                }
                else if (_tagType == "li")
                {
                }
                CreateSectionClass(_paragraphName);
            }
            else if (_tagType == "span" || _tagType == "em")
            {
                _characterName = _childName;
                _allCharacter.Push(_characterName);
            }
            else if (_tagType == "img")
            {
                _imageSource = _reader.GetAttribute("src") ?? string.Empty;
                _imageSource = _imageSource.ToLower();

                _imageLongDesc = _reader.GetAttribute("longdesc") ?? string.Empty;
                _imageLongDesc = _imageLongDesc.ToLower();

                _imageAltText = _reader.GetAttribute("alt") ?? string.Empty;
                _imageInsert = true;
                _imageClass = StackPeek(_allStyle);
                _imageSrcClass = _className;
            }
            else if (_tagType == "a")
            {
                _anchorBookMarkName = Common.RightString(_classNameWithLang, Common.SepTag);
                _anchorTitleValue = _reader.GetAttribute("title") ?? string.Empty;
                _anchorStart = true;
            }
            else
            {
                if (_reader.Name == "title" || _reader.Name == "style") // skip the node
                {
                    _reader.Read();
                }
                else
                {
                    _tagType = "O";
                }
            }
            if (_tagType != "img")
                _allStyle.Push(_childName);
        }

        public void LanguageFontCheck(string content, string styleName)
        {
            if (_newProperty.ContainsKey(styleName) && _newProperty[styleName].ContainsKey("font-family"))
            {
                return;
            }
            if (LanguageFontStyleName.Contains(styleName) == false)
            {
                string font = Common.GetLanguageUnicode(content);
                if (font != string.Empty) // Is telugu/assamese?
                {
                    if (_newProperty.ContainsKey(styleName) == false)
                    {
                        Dictionary<string, string> newStyle = new Dictionary<string, string>();
                        _newProperty[styleName] = newStyle;
                        AddUsedStyleName(styleName);
                        ParentClass[styleName] = "Standard|div";
                    }
                    _newProperty[styleName]["font-family"] = font;
                    _newProperty[styleName]["font-family-complex"] = font;
                }
            }
        }

        public virtual void CreateSectionClass(string name)
        {

        }

        /// <summary>
        /// To get the headwords styles for header
        /// </summary>
        /// <param name="isHeadword">true/false to get only headword styles</param>
        private void GetHeadwordStyles(bool isHeadword)
        {
            if (isHeadword)
            {
                string styleName;
                string headwordStyle = string.Empty;//_childName
                if (_childName.IndexOf("headword") == 0 || _childName.IndexOf("reversalform") == 0)
                {
                    headwordStyle = _childName;
                }
                if (headwordStyle.Length == 0) return;
                if (headwordStyle.IndexOf("xhomographnumber") >= 0)
                {
                    styleName = "HomoGraphNumber_" + headwordStyle;
                    if (!_headwordStyleName.Contains(styleName))
                        _headwordStyleName.Add(styleName);
                }
                else
                {
                    styleName = "Guideword_" + headwordStyle;
                    if (headwordStyle.IndexOf("reversalform") == 0)
                    {
                        styleName = "Rev" + styleName;
                    }
                    if (!_headwordStyleName.Contains(styleName))
                    {
                        _headwordStyleName.Add(styleName);
                    }

                }
            }
        }

        private void BlockInline()
        {
            string imgTag = Common.LeftString(_className, Common.SepParent);
            if (imgTag == "img")
                return;

			var multiClass = new List<string>();
			if (_className.IndexOf(' ') > 0)
			{
				multiClass.AddRange(_className.Split(' '));
			}
			else
			{
				multiClass.Add(_className);
			}

			foreach (var clsName in multiClass)
	        {
				string value = GetDisplayBlock(clsName, "display");
		        if (value == "block")
		        {
			        if (_tagType == "span")
			        {
				        _tagType = "div";
			        }
		        }
		        else if (value == "inline")
		        {
			        _tagType = "span";
		        }
		        else if (value.ToLower() == "none")
		        {
			        if (_outputType != Common.OutputType.ODT)
				        _isDisplayNone = true;
			        _displayNoneStyle = Common.LeftString(_classNameWithLang, Common.SepParent);
		        }
	        }
        }

        private string GetDisplayBlock(string multiClass, string propertyName)
        {
            string returnValue = string.Empty;
            ArrayList cssClassDetail1 = new ArrayList();
            if (_classFamily.ContainsKey(multiClass))
            {
                cssClassDetail1 = _classFamily[multiClass];
            }
            if (cssClassDetail1 == null) return _matchedCssStyleName;

            foreach (ClassInfo cssClassInfo in cssClassDetail1)
            {
                string className = cssClassInfo.StyleName;
                if (IdAllClass.ContainsKey(className) && IdAllClass[className].ContainsKey(propertyName))
                {
                    returnValue = IdAllClass[className][propertyName];
                    break;
                }
            }
            return returnValue;

        }

        protected string GetPropertyValue(string className, string propertyName)
        {
            string value = string.Empty;
            string classToCheck = className.Length > 0 ? className : _classNameWithLang;
            if (IdAllClass.ContainsKey(classToCheck) && IdAllClass[classToCheck].ContainsKey(propertyName))
            {
                value = IdAllClass[classToCheck][propertyName];
            }
            return value;
        }

        protected string GetTagInfo()
        {
            _xhtmlAttribute.Clear();
            _isclassNameExist = false;
            _lang = string.Empty;
            _className = _tagType = _reader.Name;
            if (_reader.HasAttributes)
            {
                while (_reader.MoveToNextAttribute())
                {
                    if (_reader.Name == "class")
                    {
                        _isclassNameExist = true;
                        _className = _reader.Value;
                        _className = _className.Replace("_", "");
                        _className = _className.Replace("-", "");
                        FindSecondPicture();
                        _className = Common.SortMutiClass(_className);
                        if (_outputType == Common.OutputType.XELATEX)
                        {
                            _className = Common.ReplaceCSSClassName(_className);
                        }
                    }
                    else if (_reader.Name == "lang")
                    {
                        _lang = _reader.Value;
                        _xhtmlAttribute.Add(_lang);
                        AddEntryLanguage();
                    }
                    else if (!(_reader.Name == "id" || _reader.Name == "xml:space"))
                    {
                        _xhtmlAttribute.Add(_reader.Name + _reader.Value);
                        if (_reader.Name == "href")
                            _anchorBookMarkName = _reader.Value;
                    }
                    else if (_reader.Name == "id")
                    {
                        _anchorIdValue = _reader.Value;
                    }
                }
            }

            string classNameWithLang = _className;
            _isTagClass = Common.IsTagClass(_tagType);
            if (_isTagClass != string.Empty)
            {
                string tagType = string.Empty;
                if (_tagType == "ol" || _tagType == "ul")
                {
                    _listType = _tagType;
                }
                else if (_tagType == "li")
                {
                    _className = _tagType;
                    if (_outputType == Common.OutputType.IDML)
                    {
                        if (_listType == "ol" || _listType == "ul")
                        {
                            tagType = _listType + "First";
                            _listType = _listType + "4";
                        }
                        else
                        {
                            tagType = _listType + "Next";
                        }
                        classNameWithLang = tagType + Common.SepTag + _className;
                    }
                    else if (_outputType == Common.OutputType.ODT)
                    {
                        tagType = _listType;
                        classNameWithLang = _className + Common.SepTag + tagType;
                    }
                }

                else if (_tagType == "p")
                {

                }
                _className = classNameWithLang;
            }

            _xhtmlAttribute.Sort();
            foreach (string attribute in _xhtmlAttribute)
            {
                classNameWithLang = classNameWithLang + Common.SepAttrib + attribute;
            }

            return classNameWithLang;
        }

        private void AddEntryLanguage()
        {
            if (_className == "entry")
            {
                if (IdAllClass.ContainsKey("entry") && !IdAllClass["entry"].ContainsKey("lang"))
                    IdAllClass["entry"].Add("lang", _lang);
            }
        }

        protected void EndElementBase(bool isImage)
        {
            _precedeClassAttrib = GetPreced();
            if (StackPeek(_allParagraph).CompareTo(_closeChildName) == 0)
            {
                StackPop(_allParagraph);
                ClosePara(isImage);
            }

            if (StackPeek(_allCharacter).CompareTo(_closeChildName) == 0)
            {
                StackPop(_allCharacter);
            }
        }

        protected void ClosePara(bool isImage)
        {
            if (_allParagraph.Count > 0 && !_isParagraphClosed) // Is Para Exist
            {
                _isNewParagraph = true;
                _isParagraphClosed = true;

                if (_outputType == Common.OutputType.XETEX || _outputType == Common.OutputType.XELATEX)
                {
                    _xetexFile.WriteLine();
                }
                else if (_outputType == Common.OutputType.IDML)
                {
                    _writer.WriteRaw("<Br/>");
                    _writer.WriteEndElement();
                }
                else if (_outputType == Common.OutputType.ODT)
                {
                    if (isImage)
                    {
                        _isNewParagraph = false;
                        _isParagraphClosed = false;
                    }
                    else
                    {
                        _writer.WriteEndElement();
                    }
                }
                else
                {
                    _writer.WriteEndElement();
                }
            }
            if (_forcedPara)
            {
                _writer.WriteEndElement();
                _isNewParagraph = true;
                _isParagraphClosed = true;
                _forcedPara = false;
            }
        }

        protected string ModifiedContent(string content, string paragraphName, string characterName)
        {
            string styleName = characterName;
            if (characterName == string.Empty || characterName == "$ID/[No character style]")
            {
                styleName = paragraphName;
            }
            string modifiedContent = content;
            if (styleName != null && IdAllClass.ContainsKey(styleName))
            {
                if (IdAllClass[styleName].ContainsKey("TextTransform"))
                {
                    modifiedContent = TextTransform(content, styleName, modifiedContent);
                }
            }
            if (styleName != null && styleName == "LText_FrontMatter_div_div_scrBody")
            {
                modifiedContent = modifiedContent.Replace("\\u0026", "&amp;");
            }
            return modifiedContent;
        }

        private string TextTransform(string content, string styleName, string modifiedContent)
        {
            string transformType = IdAllClass[styleName]["TextTransform"];
            switch (transformType.ToLower())
            {
                case "lowercase":
                    modifiedContent = content.ToLower();
                    break;
                case "uppercase":
                    modifiedContent = content.ToUpper();
                    break;
                case "capitalize":
                    modifiedContent = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(content);
                    break;
                default:
                    break;
            }
            return modifiedContent;
        }

        protected void SetClassCounter()
        {
            string classNameNoLang = _classNameWithLang;
            bool isIncrClassmatch = false;
            bool isResetClassmatch = false;
            if (classNameNoLang.IndexOf("_.") > 0)
                classNameNoLang = Common.LeftString(classNameNoLang, "_.");

            string[] splitClass = classNameNoLang.Split(' ');
            foreach (string clName in splitClass)
            {
                if (ContentCounterReset.ContainsKey(clName))
                {
                    classNameNoLang = clName;
                    isResetClassmatch = true;
                }
                if (contentCounterIncrement.ContainsKey(clName))
                {
                    classNameNoLang = clName;
                    isIncrClassmatch = true;
                }
            }
            if (isResetClassmatch)
            {
                string key = ContentCounterReset[classNameNoLang];
                ContentCounter[key] = 0;
            }
            if (isIncrClassmatch)
            {
                string key = "";
                string keyValue = "";
                foreach (KeyValuePair<string, string> kvp in contentCounterIncrement[classNameNoLang])
                {
                    key = kvp.Key;
                    keyValue = kvp.Value;
                }
                ContentCounter[key] = ContentCounter[key] + int.Parse(keyValue);
            }
        }

        protected string WriteCounter(string content)
        {
            try
            {
                string ConcatContent = string.Empty;
                string origContent = content;
                if (origContent != null)
                {
                    if (origContent.IndexOf("counter") >= 0
                        && origContent.IndexOf("|") >= 0)
                    {
                        string[] y = origContent.Split('|');

                        foreach (string var in y)
                        {
                            if (var.IndexOf("counter") >= 0)
                            {
                                int srtPos = var.IndexOf('(');
                                int endPos = var.IndexOf(')');
                                string var1 = var.Substring(srtPos + 1, endPos - srtPos - 1);
                                string var2 = var1.Replace("decimal", "");
                                if (ContentCounter.ContainsKey(var1))
                                {
                                    ConcatContent = ConcatContent + ContentCounter[var1];
                                }
                                else if (var1 == "sensedisc")
                                {
                                    ConcatContent = ConcatContent + Common.ConvertUnicodeToString("\\2022");
                                }
                                else if (ContentCounter.ContainsKey(var2))
                                {
                                    ConcatContent = ConcatContent + ContentCounter[var2];
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
                        content = ConcatContent;
                    }
                }
                else
                {
                    content = string.Empty;
                }
                return content;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Collects the contents of footnotes, ChapterNo and verseno.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="outputType">ODT/IDML</param>
        /// <returns></returns>
        protected bool CollectFootNoteChapterVerse(string content, string outputType)
        {
            if ((_outputType == Common.OutputType.ODT || _outputType == Common.OutputType.XELATEX) && _className.ToLower() == "chapternumber")
            {
                _chapterNo = content;
            }
            if (_className.ToLower() == "versenumber" || _className.ToLower() == "versenumber1")
            {
                _verseNo = content;
            }
            if (isFootnote)
            {
                if (string.IsNullOrEmpty(_characterName)) return false;
                LanguageFontCheck(content, _characterName);
                AddUsedStyleName(_characterName);
                StringBuilder footnoteFormat = new StringBuilder();
                if (outputType == Common.OutputType.ODT.ToString())
                {
                    if (_characterName.IndexOf("NoteTargetReference") == 0 && footnoteContent.Length > 0)
                    {
                        footnoteFormat.Append(Common.ReplaceSymbolToText(content) + "</text:span>");
                    }
                    else
                    {
                        footnoteFormat.Append("<text:span text:style-name=\"" + _characterName + "\">" + Common.ReplaceSymbolToText(content) + "</text:span>");
                    }
                }
                else if (outputType == Common.OutputType.XETEX.ToString())
                {
                    footnoteFormat.Append(Common.ReplaceSymbolToText(content));
                }
                else if (outputType == Common.OutputType.XELATEX.ToString())
                {
                    string xelatexContent = Common.ReplaceSymbolToXelatexText(content);
                    footnoteFormat.Append(Common.ReplaceSymbolToText(xelatexContent));
                }
                else
                {
                    footnoteFormat.Append("<CharacterStyleRange AppliedCharacterStyle=\"" + "CharacterStyle/" + _characterName + "\"><Content>" + Common.ReplaceSymbolToText(content) + "</Content></CharacterStyleRange>");
                }
                footnoteContent.Append(footnoteFormat);
            }
            return isFootnote;
        }

        /// <summary>
        /// Store used Paragraph adn Character style name. This data used in ModifyIDStyle.cs
        /// </summary>
        /// <param name="styleName">a_b</param>
        protected void AddUsedStyleName(string styleName)
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
        protected void AddUsedParentStyleName(string styleName)
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


        protected virtual string StackPeekCharStyle(Stack<string> stack)
        {
            string result = "$ID/[No character style]";
            if (stack.Count > 0)
            {
                result = stack.Peek();
            }
            return result;
        }
        /// <summary>
        /// Sets the chapterno, verseno and class names for footer
        /// </summary>
        protected void FooterSetup(string outputType)
        {
            string characterStyle = StackPeekCharStyle(_allCharacter);
            if (_FootNote.Contains(_className))
            {
                footnoteClass = characterStyle;
                isFootnote = true;

                //Note : if needed call this below code for - "footer call"
                footerMarkerClassName = _className + "..footnote-marker";
                if (IdAllClass.ContainsKey(footerMarkerClassName))
                {
                    string footnoteText = string.Empty;
                    if (!IdAllClass[footerMarkerClassName].ContainsKey("content")) return;
                    string content = IdAllClass[footerMarkerClassName]["content"];
                    if (content.IndexOf("string(chapter)") >= 0)
                        footnoteText = content.ToLower().Replace("string(chapter)", _chapterNo);
                    if (content.IndexOf("string(verse)") >= 0)
                    {
                        footnoteText = footnoteText.Replace("string(verse)", _verseNo);
                    }
                    if (outputType == Common.OutputType.ODT.ToString())
                    {
                        if (!IdAllClass.ContainsKey("NoteTargetReference"))
                        {
                            footnoteContent.Append("<text:span text:style-name=\"" + footerMarkerClassName + "\">" + footnoteText + "</text:span>");
                        }
                        else
                        {
                            //footnoteContent.Append("<text:span text:style-name=\"" + footerMarkerClassName + "\">");
                        }

                    }
                    else
                    {
                        footnoteContent.Append(footnoteText);
                    }
                }
            }
            else if (_className.IndexOf("NoteTargetReference") == 0 && outputType == Common.OutputType.ODT.ToString())
            {
                footnoteContent.Append("<text:span text:style-name=\"" + footerMarkerClassName + "\">");
            }
        }
        #region Private Methods


        private string FindStyleName()
        {
            string styleName = _precedeClassAttrib.ClassName + _tagType + _classNameWithLang + Common.SepParent + StackPeek(_allStyle);
            string newStyleName;
            if (_childStyle.ContainsKey(styleName)) // note: Child Style already created
            {
                newStyleName = _childStyle[styleName];
                _psuedoBeforeStyle = _existingPsuedoBeforeStyle[styleName];
                _psuedoAfterStyle = _existingPsuedoAfterStyle[styleName];
                _psuedoContainsStyle = _existingPsuedoContainsStyle[styleName];
            }
            else
            {
                newStyleName = CreateStyle();
                _childStyle[styleName] = newStyleName;
                _existingPsuedoBeforeStyle[styleName] = _psuedoBeforeStyle;
                _existingPsuedoAfterStyle[styleName] = _psuedoAfterStyle;
                _existingPsuedoContainsStyle[styleName] = _psuedoContainsStyle;
                if (_lang.Length > 0)
                    _languageStyleName[newStyleName] = _lang;
            }
            return newStyleName;
        }

        private string CreateStyle()
        {
            _parentPrecedeClassAttrib = GetParentPrecede();
            ArrayList multiClassList = MultiClassCombination(_className);

            float ancestorFontSize = FindAncestorFontSize();
            _parentStyleName = StackPeek(_allStyle);

            string styleName = MatchCssStyle(ancestorFontSize, "null", multiClassList);
			if (styleName == string.Empty) // missing style in CSS
            {
                styleName = _className;
                if (_outputType == Common.OutputType.ODT || _outputType == Common.OutputType.XELATEX)
                {
                    if (_lang.Length > 0)
                        styleName = _className + Common.SepAttrib + _lang;
                }

            }
			else if (styleName == "span")
			{
				styleName = _className;
			}
			else
			{
				if (_outputType == Common.OutputType.XELATEX)
				{
					if (_lang.Length > 0)
						styleName = _className + Common.SepAttrib + _lang;
				}
			}
	        string newStyleName = GetStyleNumber(styleName);

            if (_newProperty.ContainsKey(newStyleName) == false)
            {
                _newProperty[newStyleName] = _tempStyle;
            }

            IdAllClass[newStyleName] = _tempStyle;
            string tagType = _tagType;
            if (_divType.Contains(_tagType)) tagType = "div";
            ParentClass[newStyleName] = _parentStyleName + "|" + tagType;

            _psuedoBeforeStyle = _psuedoAfterStyle = _psuedoContainsStyle = null;
            string styleBefore = MatchCssStyle(ancestorFontSize, "before", multiClassList);
            if (styleBefore != string.Empty)
            {
                string tag = "span"; //  always character styles
                string newStyleBefore = GetStyleNumber(styleBefore);
                _newProperty[newStyleBefore] = _tempStyle;
                IdAllClass[newStyleBefore] = _tempStyle;
                _psuedoBeforeStyle.StyleName = newStyleBefore;
                ParentClass[newStyleBefore] = _parentStyleName + "|" + tag;

            }

            string styleAfter = MatchCssStyle(ancestorFontSize, "after", multiClassList);
            if (styleAfter != string.Empty)
            {
                string tag = "span"; //  always character styles
                string newStyleAfter = GetStyleNumber(styleAfter);
                _newProperty[newStyleAfter] = _tempStyle;
                IdAllClass[newStyleAfter] = _tempStyle;
                _psuedoAfterStyle.StyleName = newStyleAfter;
                ParentClass[newStyleAfter] = _parentStyleName + "|" + tag;
            }

            string styleContains = MatchCssStyle(ancestorFontSize, "contains", multiClassList);
            if (styleContains != string.Empty)
            {
                string newStyleContains = GetStyleNumber(styleContains);
                _newProperty[newStyleContains] = _tempStyle;
                IdAllClass[newStyleContains] = _tempStyle;
                _psuedoContainsStyle.StyleName = newStyleContains;
                ParentClass[newStyleContains] = _parentStyleName + "|" + _tagType;
            }

            return newStyleName;
        }

        private string GetStyleNumber(string styleName)
        {
            if (_outputType != Common.OutputType.IDML)
            {
                if (_parentStyleName.Trim().Length == 0) return styleName;

                return styleName + Common.SepParent + _parentStyleName;
            }

            if (styleName == "headword") return styleName;
            if (styleName == "xhomographnumber") return styleName;
            if (styleName == "hideChapterNumber") return styleName;
            if (styleName == "hideVerseNumber") return styleName;
            if (styleName == "hideVerseNumber1") return styleName;
            if (_headwordStyles) return styleName;
            while (styleName.StartsWith("span") || styleName.StartsWith("xitem"))
            {
                if (string.IsNullOrEmpty(_parentStyleName))
                {
                    break;
                }
                styleName = _parentStyleName;
                if (styleName.Contains("_"))
                {
                    styleName = styleName.Substring(0, styleName.IndexOf("_"));
                }
                _parentStyleName = StackPeek(_allStyle);
            }
            int suffix = 1;
            string newname;
            while (true)
            {
                newname = styleName + Common.SepParent + suffix++;
                if (!_newProperty.ContainsKey(newname))
                {
                    break;
                }
                var props = _newProperty[newname];
                if (props.Count == _tempStyle.Count)
                {
                    var allPopsEqual = true;
                    foreach (string prop in props.Keys)
                    {
                        if (!_tempStyle.ContainsKey(prop) || props[prop] != _tempStyle[prop])
                        {
                            allPopsEqual = false;
                            break;
                        }
                    }
                    if (allPopsEqual)
                    {
                        break;
                    }
                }
            }
            return newname;
        }

        private string MatchCssStyle(float ancestorFontSize, string psuedo, ArrayList multiClassList)
        {
            bool resultTagClass, resultCoreClass, resultAncestor, resultParent, resultParentPrecede, resultPrecede;
            _tempStyle = new Dictionary<string, string>();
            _matchedCssStyleName = string.Empty;
            foreach (string multiClass in multiClassList)
            {
                ArrayList cssClassDetail = new ArrayList();
				if (_classFamily.ContainsKey(multiClass))
				{
					cssClassDetail = _classFamily[multiClass];
				}
                AddTagProperty(cssClassDetail, multiClass);
                if (cssClassDetail == null) return _matchedCssStyleName;

                foreach (ClassInfo cssClassInfo in cssClassDetail)
                {
                    if (cssClassInfo.Pseudo == "footnote-call")
                        _footnoteMarkerClass = cssClassInfo;

                    if (cssClassInfo.Pseudo != psuedo) continue;

					if (cssClassInfo.StyleName.IndexOf(".-", System.StringComparison.Ordinal) > 0 || cssClassInfo.StyleName.IndexOf("_", System.StringComparison.Ordinal) > 0)
	                {
						var xhtmlClassInfo = _allStyleInfo.ToArray();

						var parentStyle = cssClassInfo.StyleName.Replace("span_", "").Replace("span.-", "");
						parentStyle = parentStyle.Replace(".-", "_");
						var clsNameList = parentStyle.Split('_').ToList();

						if (clsNameList[0] != cssClassInfo.CoreClass.ClassName)
						{
							clsNameList.Remove(cssClassInfo.CoreClass.ClassName);
						}

						GetParentPrecedeClass(cssClassInfo);

		                var currClsNameList = new ArrayList();

						foreach (ClassInfo clsName in xhtmlClassInfo)
						{
							if (clsName.CoreClass.ClassName.IndexOf(' ') > 0)
							{
								string[] mulltiClass = clsName.CoreClass.ClassName.Split(' ');
								currClsNameList.AddRange(mulltiClass);
							}
							else
							{
								currClsNameList.Add(clsName.CoreClass.ClassName);
							}
						}

						//Consider only the list when is having greater than 2 classes
		                if (cssClassInfo.StyleName.IndexOf("span") == 0 && clsNameList.Count > 1 && clsNameList.Any(t => !currClsNameList.Contains(t)))
		                {
			                currClsNameList = new ArrayList();
		                }

						if (currClsNameList.Count == 0) continue;
	                }

	                resultCoreClass = CompareCoreClass(cssClassInfo.CoreClass, _xhtmlClassAttrib, multiClass);
                    resultTagClass = true;
                    if (cssClassInfo.TagName != string.Empty)
                    {
                        if (cssClassInfo.TagName != _tagType)
                        {
                            resultTagClass = false;
                        }
                    }

                    resultAncestor = CompareClass(cssClassInfo.Ancestor);
                    resultParent = CompareParentClass(cssClassInfo.parent);
                    resultParentPrecede = CompareClass(cssClassInfo.ParentPrecede, _parentPrecedeClassAttrib);
                    resultPrecede = CompareClass(cssClassInfo.Precede, _precedeClassAttrib);

                    if (resultCoreClass && resultTagClass && resultAncestor && resultParent && resultParentPrecede &&
                        resultPrecede)
                    {
	                    AssignProperty(cssClassInfo.StyleName, ancestorFontSize);

						if (_matchedCssStyleName == string.Empty)
                        {
                            _matchedCssStyleName = cssClassInfo.StyleName;
                            if (psuedo == "before")
                            {
                                _psuedoBeforeStyle = SetClassInfo(cssClassInfo.CoreClass.ClassName, cssClassInfo);
                            }
                            else if (psuedo == "after")
                            {
                                _psuedoAfterStyle = SetClassInfo(cssClassInfo.CoreClass.ClassName, cssClassInfo);
                            }
                            else if (psuedo == "contains")
                            {
                                _psuedoContainsStyle = SetClassInfo(cssClassInfo.CoreClass.ClassName, cssClassInfo);
                            }
                        }
                    }
                }
            }
            if (_outputType != Common.OutputType.ODT)
            {
                AppendParentProperty();
            }
            if (_outputType != Common.OutputType.XETEX || _outputType != Common.OutputType.XELATEX)
            {
                _matchedCssStyleName.Replace(Common.sepPrecede, "");
                _matchedCssStyleName.Replace(Common.SepParent, "");
                _matchedCssStyleName.Replace(Common.SepPseudo, "");
                _matchedCssStyleName.Replace(Common.SepTag, "");
                _matchedCssStyleName.Replace(Common.SepAttrib, "");
                _matchedCssStyleName.Replace(Common.Space, "");

            }
            return _matchedCssStyleName;
        }

		/// <summary>
		///Method to get the classname which are like ".semanticdomain + .semanticdomain;
		/// Based on this, Code will avoid default space after the first "semanticdomain"		/// </summary>
		/// <param name="cssClassInfo"></param>
	    private void GetParentPrecedeClass(ClassInfo cssClassInfo)
	    {
		    string currStyleName = cssClassInfo.StyleName.Replace(".-", "-");
		    var clsNameList = currStyleName.Split('-').ToList();
		    if (cssClassInfo.CoreClass.ClassName == clsNameList[0] && clsNameList[0] == clsNameList[1])
		    {
			    if (!_parentPrecendeSpace.Contains(cssClassInfo.CoreClass.ClassName))
				    _parentPrecendeSpace.Add(cssClassInfo.CoreClass.ClassName);
		    }
	    }

	    /// <summary>
        /// xhtml - h1 , Get h1,h1[lang='en'],h1[level='1'] from css.
        /// </summary>
        /// <param name="cssClassDetail"></param>
        /// <param name="multiClass"></param>
        private void AddTagProperty(ArrayList cssClassDetail, string multiClass)
        {
            if (_classFamily.ContainsKey(_tagType))
            {
                if (multiClass != _tagType)
                {
                    ClassInfo cssClassInfoTag;
                    foreach (ClassInfo cssClassInfo in _classFamily[_tagType])
                    {
                        cssClassInfoTag = SetClassInfo(multiClass, cssClassInfo);
                        cssClassDetail.Add(cssClassInfoTag);
                    }
                }
            }
        }

        private ClassInfo SetClassInfo(string multiClass, ClassInfo cssClassInfo)
        {
            ClassInfo cssClassInfoTag;
            cssClassInfoTag = new ClassInfo();

            cssClassInfoTag.StyleName = cssClassInfo.StyleName;
            cssClassInfoTag.CoreClass.SetClassAttrib(multiClass, cssClassInfo.CoreClass.Attribute);
            cssClassInfoTag.Ancestor.SetClassAttrib(cssClassInfo.Ancestor.ClassName, cssClassInfo.Ancestor.Attribute);
            cssClassInfoTag.ParentPrecede.SetClassAttrib(cssClassInfo.ParentPrecede.ClassName, cssClassInfo.ParentPrecede.Attribute);
            cssClassInfoTag.Precede.SetClassAttrib(cssClassInfo.Precede.ClassName, cssClassInfo.Precede.Attribute);

            cssClassInfoTag.StyleName = cssClassInfo.StyleName;
            cssClassInfoTag.TagName = cssClassInfo.TagName;
            cssClassInfoTag.Content = cssClassInfo.Content;
            cssClassInfoTag.Contains = cssClassInfo.Contains;
            cssClassInfoTag.SpecificityWeightage = cssClassInfo.SpecificityWeightage;
            cssClassInfoTag.Pseudo = cssClassInfo.Pseudo;
            return cssClassInfoTag;
        }

        private void AppendParentProperty()
        {
            string parentStyle = StackPeek(_allStyle);
            if (IdAllClass.ContainsKey(parentStyle))
            {
                DoNotInherit(parentStyle);

                foreach (KeyValuePair<string, string> property in IdAllClass[parentStyle])
                {
                    if (_outputType == Common.OutputType.IDML)
                    {
                        if (_tempStyle.ContainsKey(property.Key) || property.Key == "TextColumnCount")
                        {
                            continue;
                        }
                        _tempStyle[property.Key] = property.Value;
                    }
                    else if (_outputType == Common.OutputType.XETEX || _outputType == Common.OutputType.XELATEX)
                    {
                        if (_paragraphPropertyList != null)
                        {
                            if (_paragraphPropertyList.Contains(property.Key))
                                continue;
                        }
                        if (_tempStyle.ContainsKey(property.Key) == false)
                        {
                            _tempStyle[property.Key] = property.Value;
                        }
                    }
                }
            }
        }

        private void DoNotInherit(string parentStyle)
        {
            bool removed = false;
            if (IdAllClass[parentStyle].ContainsKey("SpaceBefore"))
            {
                IdAllClass[parentStyle].Remove("SpaceBefore");
                removed = true;
            }
            if (IdAllClass[parentStyle].ContainsKey("SpaceAfter"))
            {
                IdAllClass[parentStyle].Remove("SpaceAfter");
                removed = true;
            }
            if (removed)
            {
                string originalParentStyle = Common.LeftString(parentStyle, "_");
                if (_doNotInheritProperty.Contains(originalParentStyle) == false)
                {
                    _doNotInheritOriginalClass = parentStyle;
                    _doNotInheritClass = originalParentStyle;
                }
            }
        }

        private void FindSecondPicture()
        {
            if (_outputType == Common.OutputType.ODT || _outputType == Common.OutputType.ODM)
            {
                if (_isFirstPictureWritten)
                {
                    if (_className == "entry")
                    {
                        _isFirstPictureWritten = false;
                    }
                    else if (_className == "pictureRight")
                    {
                        _className = "pictureNone";
                    }
                }
                else if (_className == "pictureRight")
                {
                    _isFirstPictureWritten = true;
                }
            }
        }

        private void AssignProperty(string cssStyleName, float ancestorFontSize)
        {
            if (!IdAllClass.ContainsKey(cssStyleName))
            {
                return;
            }
            foreach (KeyValuePair<string, string> property in IdAllClass[cssStyleName])
            {
                if (_tempStyle.ContainsKey(property.Key)) continue;

                if (_outputType == Common.OutputType.ODT && property.Key == "line-spacing")
                {
                    string without_pt = property.Value.Replace("pt", "");
                    float value = float.Parse(without_pt.Replace("%", ""));
                    float lineHeight = 0;
                    if (property.Value.IndexOf("%") > 0)
                    {
                        lineHeight = (value - 100) * ancestorFontSize / 100;
                        lineHeight = lineHeight / 2;
                    }
                    else if (property.Value.IndexOf("pt") > 0)
                    {
                        lineHeight = (value - ancestorFontSize) / 2;
                    }
                    if (lineHeight > 0)
                    {
                        _tempStyle[property.Key] = lineHeight + "pt";
                    }
                }
                else if (property.Value.IndexOf("%") > 0 && property.Key != "text-position")
                {
                    float value = float.Parse(property.Value.Replace("%", ""));
                    _tempStyle[property.Key] = (ancestorFontSize * value / 100).ToString();
                    const string point = "pt";
                    if (_outputType == Common.OutputType.ODT)
                    {
                        if (property.Key == "column-gap") // For column-gap: 2em; change the value as (-ve)
                        {
                            string secClass = "Sect_" + _className.Trim();
                            if (_dictColumnGapEm.ContainsKey(secClass) && _dictColumnGapEm[secClass].ContainsKey("columnGap"))
                            {
                                _dictColumnGapEm[secClass]["columnGap"] = _tempStyle[property.Key] + "pt";
                                _tempStyle[property.Key] = (-ancestorFontSize * value / 100).ToString();

                                if (true)
                                {
                                    int counter;
                                    string colGapValue = _dictColumnGapEm[secClass]["columnGap"];
                                    string columnGap = Common.GetNumericChar(colGapValue, out counter);
                                    float columnGapInch = Common.ConvertToInch(columnGap);
                                    float expColumnGap = Common.ConvertToInch(_dictColumnGapEm[secClass]["pageWidth"]) - columnGapInch - Common.ConvertToInch(_dictColumnGapEm[secClass]["marginLeft"])
                                                 - Common.ConvertToInch(_dictColumnGapEm[secClass]["marginLeft"]) / 2.0F;
                                    Common.ColumnWidth = float.Parse(Common.UnitConverter(expColumnGap + "in", "pt"));
                                }
                            }
                        }
                        else
                        {
                            float size = ancestorFontSize * value / 100;
                            _tempStyle[property.Key] = size + point;
                        }
                    }
                }
                else
                {
                    if (property.Key != "prince-text-replace")
                        _tempStyle[property.Key] = property.Value;
                }
            }
            WordCharSpace(ancestorFontSize);
        }

        private bool CompareCoreClass(ClassAttrib classAttrib, ClassAttrib xhtmlClassInfo, string cssTagFamily)
        {
            if (_isclassNameExist == false)
            {
                if (classAttrib.ClassName == string.Empty) // Tag class
                {
                    if (_tagType != cssTagFamily)
                    {
                        return false;
                    }
                    classAttrib.ClassName = cssTagFamily;
                }
            }

            bool match = CompareClass(classAttrib, xhtmlClassInfo);
            return match;
        }

        private bool CompareParentClass(ArrayList parent)
        {
            parent.Reverse();
            bool match = true;
            ClassInfo[] xhtmlClassInfo = _allStyleInfo.ToArray();
            int i = 0;
            foreach (ClassAttrib cssParentClass in parent)
            {
	            match = CompareClass(cssParentClass, xhtmlClassInfo[i++].CoreClass);
				if (match == false)
				{
					foreach (var it in xhtmlClassInfo.Select((x, j) => new { Value = x, Index = j}))
					{
						if(it.Index < i) continue;
						match = CompareClass(cssParentClass, it.Value.CoreClass);
						if (match) break;
					}
					break;
				}
            }
            parent.Reverse();
            return match;
        }

        private bool CompareClass(ClassAttrib cssClassInfo)
        {
            bool match = true;

            if (cssClassInfo.ClassName == string.Empty)
            {
                return match;
            }
            if (_allStyleInfo.Count == 0)
            {
                return false;
            }

            foreach (ClassInfo xhtmlClassInfo in _allStyleInfo)
            {
                match = CompareClass(cssClassInfo, xhtmlClassInfo.CoreClass);
                if (match)
                {
                    break;
                }
            }

            return match;
        }

        private bool CompareClass(ClassAttrib cssClassInfo, ClassAttrib xhtmlClassInfo)
        {
            bool match = true;

            if (cssClassInfo.ClassName == string.Empty)
            {
                return match;
            }
            match = IsSubClass(xhtmlClassInfo.ClassName, cssClassInfo.ClassName);
            if (match)
            {
                foreach (string attrib in cssClassInfo.Attribute)
                {
                    if (!xhtmlClassInfo.Attribute.Contains(attrib))
                    {
                        match = false;
                        break;
                    }
                }
            }

            return match;
        }



        private bool IsSubClass(string superSet, string subSet)
        {
            bool result = true;

            string[] super = superSet.Split(' ');
            string[] sub = subSet.Split(' ');
            int superLen = super.Length;

            if (superLen == 1)
            {
                if (superSet != subSet)
                {
                    result = false;
                }
                return result;
            }

            var superList = new ArrayList();
            superList.AddRange(super);

            for (int i = 0; i < sub.Length; i++)
            {
                if (!superList.Contains(sub[i]))
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        private ArrayList MultiClassCombination(string multiClass)
        {
            multiClass = Common.SortMutiClass(multiClass);
            string[] className = multiClass.Split(' ');
            ArrayList result = new ArrayList();
            ArrayList concate = new ArrayList();
            ArrayList resultCssClassOrder = new ArrayList();
            int len = className.Length;

            if (len == 1)
            {
                resultCssClassOrder.AddRange(className);
                return resultCssClassOrder;
            }
            result.AddRange(className);
            int searchFrom;
            int searchTo = 0;
            for (int i = 0; i < len - 1; i++)
            {
                searchFrom = searchTo + 1;
                searchTo = result.Count - 1;
                concate = MultiClassConcate(className, result, len, searchFrom, searchTo);
                result.AddRange(concate);
                resultCssClassOrder.InsertRange(0, OrderByCss(concate));
            }
            concate.Clear();
            concate.AddRange(className);
            resultCssClassOrder.AddRange(OrderByCss(concate));

            return resultCssClassOrder;
        }

        private ArrayList MultiClassConcate(string[] clsList, IList arr, int sourceUpper, int searchFrom, int searchTo)
        {
            ArrayList result = new ArrayList();
            for (int i = 0; i < sourceUpper; i++)
            {
                string first = clsList[i];
                for (int j = searchFrom + i; j <= searchTo; j++)
                {
                    string[] element = arr[j].ToString().Split(' ');
                    string secondStart = element[0];
                    if (first.CompareTo(secondStart) >= 0) continue;
                    string combine = first + " " + arr[j];
                    result.Add(combine);
                }
            }
            return result;
        }


        private ArrayList OrderByCss(ArrayList result)
        {
            ArrayList resultCssClassOrder = new ArrayList();
            foreach (string className in _cssClassOrder)
            {
                if (result.Contains(className))
                {
                    resultCssClassOrder.Insert(0, className);
                }
            }
            return resultCssClassOrder;
        }


        private void WordCharSpace(float ancestorFontSize)
        {
            if (_outputType == Common.OutputType.IDML)
            {
                if (_tempStyle.ContainsKey("DesiredLetterSpacing"))
                {
                    float value = float.Parse(_tempStyle["DesiredLetterSpacing"]);
                    float percentage = value / ancestorFontSize * 100 * 3.6F;
                    _tempStyle["DesiredLetterSpacing"] = percentage.ToString();
                    _tempStyle["MaximumLetterSpacing"] = percentage.ToString();
                }
                if (_tempStyle.ContainsKey("DesiredWordSpacing"))
                {
                    float value = float.Parse(_tempStyle["DesiredWordSpacing"]);
                    float percentage = value / ancestorFontSize * 100 * 3.6F;
                    _tempStyle["DesiredWordSpacing"] = percentage.ToString();
                    _tempStyle["MaximumWordSpacing"] = percentage.ToString();
                }
            }
        }


        private float FindAncestorFontSize()
        {
            // TODO find from the current style ex: font-size:20pt; line-height: 1em;
            string[] ancestorStyleName = _allStyle.ToArray();
            float fontSize = _projInfo.DefaultFontSize;

            string fontPointSize = "font-size";
            if (_outputType == Common.OutputType.IDML)
                fontPointSize = "PointSize";

            // Search in ancestor class
            foreach (string ancestor in ancestorStyleName)
            {
                if (IdAllClass.ContainsKey(ancestor))
                {
                    if (IdAllClass[ancestor].ContainsKey(fontPointSize))
                    {
                        try
                        {
                            fontSize = float.Parse(IdAllClass[ancestor][fontPointSize].Replace("pt", ""), CultureInfo.GetCultureInfo("en-US"));
                        }
                        catch (Exception)
                        {
                            fontSize = 10;
                        }
                        break;
                    }
                }
            }

            // Search in current class
            if (IdAllClass.ContainsKey(_classNameWithLang) && IdAllClass[_classNameWithLang].ContainsKey(fontPointSize))
            {

                string currentFontSize = IdAllClass[_classNameWithLang][fontPointSize];
                if (currentFontSize.IndexOf("%") > 0)
                {
                    float value = float.Parse(currentFontSize.Replace("%", ""));
                    fontSize = fontSize * value / 100;
                    IdAllClass[_classNameWithLang][fontPointSize] = fontSize.ToString();
                }
                else if (currentFontSize == "larger" || currentFontSize == "smaller")
                {
                    fontSize = Common.GetLargerSmaller(fontSize, currentFontSize);
                    IdAllClass[_classNameWithLang][fontPointSize] = fontSize.ToString();
                }
                else
                {
                    try
                    {
                        fontSize = float.Parse(currentFontSize.Replace("pt", ""), CultureInfo.GetCultureInfo("en-US"));
                    }
                    catch (Exception)
                    {
                        fontSize = 10;
                    }
                }
            }

            return fontSize;
        }

        protected string StackPeek(Stack<string> stack)
        {
            string result = string.Empty;
            if (stack.Count > 0)
            {
                result = stack.Peek();
            }
            return result;
        }

        protected string StackPop(Stack<string> stack)
        {
            string result = string.Empty;
            if (stack.Count > 0)
            {
                result = stack.Pop();
            }
            return result;
        }

        protected ClassAttrib GetPreced()
        {
            ClassAttrib result = new ClassAttrib();
            if (_allStyleInfo != null && _allStyleInfo.Count > 0)
            {
                ClassInfo result1 = new ClassInfo();
                result1 = _allStyleInfo.Pop();
                result.SetClassAttrib(result1.CoreClass.ClassName, result1.CoreClass.Attribute);
            }
            return result;
        }

        protected ClassAttrib GetParentPrecede()
        {
            ClassAttrib result = new ClassAttrib();
            if (_allStyleInfo != null && _allStyleInfo.Count > 0)
            {
                ClassInfo classInfo = new ClassInfo();
                classInfo = _allStyleInfo.Peek();
                result.SetClassAttrib(classInfo.Precede.ClassName, classInfo.Precede.Attribute);
            }
            return result;
        }


        #endregion
    }
}
