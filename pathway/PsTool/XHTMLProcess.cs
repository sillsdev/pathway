// --------------------------------------------------------------------------------------------
// <copyright file="XHTMLProcess.cs" from='2009' to='2009' company='SIL International'>
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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using SIL.PublishingSolution;
using SIL.Tool;

#endregion Using

namespace SIL.PublishingSolution
{
    public class XHTMLProcess
    {
        #region Public Variable

        protected XmlTextWriter _writer;
        protected XmlTextReader _reader;

        protected Stack<string> _allStyle;
        protected Stack<string> _allParagraph;
        protected Stack<string> _allCharacter;
        protected Stack<ClassInfo> _allStyleInfo = new Stack<ClassInfo>();
        protected ClassAttrib _precedeClassAttrib = new ClassAttrib();
        protected ClassAttrib _xhtmlClassAttrib = new ClassAttrib();
        protected ClassAttrib _parentClassAttrib = new ClassAttrib();
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
        protected Dictionary<string, string> _displayBlock;

        protected Dictionary<string, Dictionary<string, string>> IdAllClass;
        protected Dictionary<string, Dictionary<string, string>> _newProperty;
        protected Dictionary<string, string> ParentClass;
        protected Dictionary<string, string> _listTypeDictionary = new Dictionary<string, string>();
        protected Dictionary<string, ArrayList> _classFamily;
        //protected ClassInfo _classInfo = new ClassInfo();
        protected string _className;
        protected ArrayList _attribute;
        private string _isTagClass;
        private string _listType;
        protected string _anchorBookMarkName = string.Empty;
        protected string _anchorIdValue = string.Empty;
        protected bool _anchorStart;

        protected string _childName = string.Empty;
        protected string _closeChildName = string.Empty;
        protected bool _isNewParagraph;
        protected bool _isParagraphClosed = true;

        protected string _paragraphName;
        protected string _previousParagraphName;
        protected string _characterName;
        protected string _lang = string.Empty;
        protected Dictionary<string, string> _languageStyleName = new Dictionary<string, string>();
        ArrayList _xhtmlAttribute = new ArrayList();
        protected ArrayList _cssClassOrder = new ArrayList();
        protected string _tagType;
        protected bool _isclassNameExist;
        protected string _projectPath;
        protected string _classNameWithLang;
        protected bool _imageInsert;
        protected string _imageSource;
        protected string _imageClass;
        protected string _imageSrcClass;
        protected bool _isAutoWidthforCaption;
        protected Common.OutputType _outputType;
        protected string _listName = string.Empty;

        protected ArrayList _headwordStyleName = new ArrayList();
        protected bool _headwordStyles = false;
        private string _parentStyleName;
        protected ArrayList _visibilityClassName = new ArrayList();
        protected Dictionary<string, string> _replaceSymbolToText = new Dictionary<string, string>();
        protected List<string> _dropCap = new List<string>();
        protected bool _isDropCap;

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
            _reader = new XmlTextReader(xhtmlFileWithPath)
                          {
                              XmlResolver = null,
                              WhitespaceHandling = WhitespaceHandling.Significant
                          };
        }

        protected bool IsEmptyNode()
        {
            bool result = false;
            if (_reader.IsEmptyElement)
            {
                if (_reader.Name != "img")
                {
                    result = true;
                }
            }
            return result;
        }

        protected void StartElementBase(bool isHeadword)
        {

            ClassInfo classInfo = new ClassInfo();
            _classNameWithLang = GetTagInfo();
            _xhtmlClassAttrib.SetClassAttrib(_className, _xhtmlAttribute);
            classInfo.CoreClass = _xhtmlClassAttrib;
            classInfo.Precede = _precedeClassAttrib;
            _childName = FindStyleName();
            GetHeadwordStyles(isHeadword);
            _allStyleInfo.Push(classInfo);

            BlockInline();
            string[] divTypeList = new[] { "div", "ol", "ul", "li", "p", "body", "h1", "h2", "h3", "h4", "h5", "h6" };
            List<string> divType = new List<string>(divTypeList);
            if (divType.Contains(_tagType))
            {
                _paragraphName = _childName;
                _allParagraph.Push(_paragraphName);
                _isNewParagraph = true;

                CreateSectionClass(_paragraphName);

                DivTypeLi_Odt();


            }
            else if (_tagType == "span" || _tagType == "em") 
            {
                _characterName = _childName;
                _allCharacter.Push(_characterName);
            }
            else if (_tagType == "img")
            {
                _imageSource = _reader.GetAttribute("src").ToLower();
                //_characterName = _childName;
                //_allCharacter.Push(_characterName);
                _imageInsert = true;
                _imageClass = StackPeek(_allParagraph);
                _imageSrcClass = _className;
            }
            else if (_tagType == "a")
            {
                _anchorBookMarkName = Common.RightString(_classNameWithLang, Common.SepTag);
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
                    return;
                }
            }
            if (_tagType != "img")
                _allStyle.Push(_childName);
        }

        private void DivTypeLi_Odt()
        {
            if (_outputType == Common.OutputType.ODT)
            {
                if (_tagType == "ol" || _tagType == "ul")
                {
                    _listName = _listTypeDictionary.ContainsKey(_paragraphName)
                                    ? _listTypeDictionary[_paragraphName]
                                    : _tagType;
                }
                else if (_tagType == "li")
                {
                    if (_isNewParagraph)
                    {
                        _writer.WriteEndElement();
                        _isNewParagraph = true;
                        _isParagraphClosed = true;
                    }

                    if (_listName.Length != 0)
                    {
                        _writer.WriteStartElement("text:list");
                        _writer.WriteAttributeString("text:style-name", _listName);
                        _listName = string.Empty;
                       
                    }
                    _writer.WriteStartElement("text:list-item");
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
                string headwordStyle = _childName;
                if (headwordStyle.IndexOf("xhomographnumber") >= 0)
                {
                    styleName = "HomoGraphNumber_" + headwordStyle;
                    if (!_headwordStyleName.Contains(styleName))
                        _headwordStyleName.Add(styleName);
                }
                else
                {
                    styleName = "Guideword_" + headwordStyle;
                    if (!_headwordStyleName.Contains(styleName))
                        _headwordStyleName.Add(styleName);
                }
            }
        }

        private void BlockInline()
        {
            string value = GetPropertyValue(string.Empty, "display");
            if (value == "block")
            {
                _tagType = "div";
            }
            else if (value == "inline")
            {
                _tagType = "span";
            }
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
            _anchorStart = false;
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
                        _className = Common.SortMutiClass(_className);
                    }
                    else if (_reader.Name == "lang")
                    {
                        _lang = _reader.Value;
                        //classNameWithLang = classNameWithLang + Common.SepAttrib + _lang;
                        _xhtmlAttribute.Add(_lang);
                        AddEntryLanguage();
                    }
                    else if (!(_reader.Name == "id" || _reader.Name == "xml:space"))
                    {
                        _xhtmlAttribute.Add(_reader.Name + _reader.Value);
                        if (_reader.Name == "id") 
                            _anchorIdValue = _reader.Value;
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
                    //DivTypeLi_Odt();
                    string tagType = string.Empty;
                    if (_tagType == "ol" || _tagType == "ul")
                    {
                        //classNameWithLang = _tagType + Common.SepTag + _className;
                        //_className = _tagType;
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
                        }
                        else if (_outputType == Common.OutputType.ODT)
                        {
                            tagType = _listType;
                        }
                        classNameWithLang = tagType + Common.SepTag + _className;
                    }
                
                else if (_tagType == "p")
                {
                    //classNameWithLang = _tagType + Common.SepTag + _className;
                    //_className = _tagType;
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

        protected void EndElementBase()
        {
            //_closeChildName = StackPop(_allStyle);
            _precedeClassAttrib = GetPreced();
            //if (_closeChildName == string.Empty) return;

            if (StackPeek(_allParagraph).CompareTo(_closeChildName) == 0)
            {
                StackPop(_allParagraph);
                ClosePara();
            }

            if (StackPeek(_allCharacter).CompareTo(_closeChildName) == 0)
            {
                StackPop(_allCharacter);
            }
        }

        protected void ClosePara()
        {
            if (_outputType == Common.OutputType.ODT && (_reader.Name == "ul" || _reader.Name == "ol"))
            {
                _writer.WriteEndElement();
            }

            if (_allParagraph.Count > 0 && !_isParagraphClosed) // Is Para Exist
            {
                if(_outputType == Common.OutputType.IDML)
                {
                    _writer.WriteRaw("<Br/>");
                }

                _writer.WriteEndElement();
                _isNewParagraph = true;
                _isParagraphClosed = true;
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
                if (IdAllClass[styleName].ContainsKey("display") && IdAllClass[styleName]["display"] == "none")
                {
                    modifiedContent = string.Empty;
                }
                if (IdAllClass[styleName].ContainsKey("TextTransform"))
                {
                    modifiedContent = TextTransform(content, styleName, modifiedContent);
                }
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

        #region Private Methods


        private string FindStyleName()
        {
            if (_allStyle.Count == 0) return _classNameWithLang;

            //string styleName = _precedeClassAttrib.ClassName + _classNameWithLang + Common.SepParent + StackPeek(_allStyle);
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
                if(_lang.Length > 0)
                    _languageStyleName[newStyleName] = _lang;
            }
            //if (SkipEmptyIDChapterNumber())
            //{
            //    newStyleName = "hide" + newStyleName;
            //}
            return newStyleName;
        }

        private string CreateStyle()
        {
            _parentClassAttrib = GetParent();
            _parentPrecedeClassAttrib = GetParentPrecede();
            ArrayList multiClassList = MultiClassCombination(_className);

            float ancestorFontSize = FindAncestorFontSize();
            _parentStyleName = StackPeek(_allStyle);

            string styleName = MatchCssStyle(ancestorFontSize, "null", multiClassList);
            if (styleName == string.Empty) // missing style in CSS
            {
                styleName = _className;
            }
            //string newStyleName = styleName + Common.SepParent + parentStyle;
            string newStyleName = GetStyleNumber(styleName);

            _newProperty[newStyleName] = _tempStyle;
            IdAllClass[newStyleName] = _tempStyle;
            ParentClass[newStyleName] = _parentStyleName + "|" + _tagType;

            _psuedoBeforeStyle = _psuedoAfterStyle = _psuedoContainsStyle = null;
            string styleBefore = MatchCssStyle(ancestorFontSize, "before", multiClassList);
            if (styleBefore != string.Empty)
            {
                //string newStyleBefore = styleBefore + Common.SepParent + parentStyle;
                string newStyleBefore = GetStyleNumber(styleBefore);
                _newProperty[newStyleBefore] = _tempStyle;
                IdAllClass[newStyleBefore] = _tempStyle;
                _psuedoBeforeStyle.StyleName = newStyleBefore;
                ParentClass[newStyleBefore] = _parentStyleName + "|" + _tagType;

            }

            string styleAfter = MatchCssStyle(ancestorFontSize, "after", multiClassList);
            if (styleAfter != string.Empty)
            {
                //string newStyleAfter = styleAfter + Common.SepParent + parentStyle;
                string newStyleAfter = GetStyleNumber(styleAfter);
                _newProperty[newStyleAfter] = _tempStyle;
                IdAllClass[newStyleAfter] = _tempStyle;
                _psuedoAfterStyle.StyleName = newStyleAfter;
                ParentClass[newStyleAfter] = _parentStyleName + "|" + _tagType;
            }

            string styleContains = MatchCssStyle(ancestorFontSize, "contains", multiClassList);
            if (styleContains != string.Empty)
            {
                //string newStyleContains = styleContains + Common.SepParent + parentStyle;
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
            if (_outputType == Common.OutputType.ODT)
            {
                if(_parentStyleName == "body") return styleName;

                string newStyleName = styleName + Common.SepParent + _parentStyleName;
                return newStyleName;
            }

            if(styleName == "headword") return styleName;
            if (styleName == "xhomographnumber") return styleName;
            if (_headwordStyles) return styleName;
            int suffix = 1;
            string newname;
            while(true)
            {
                newname = styleName + Common.SepParent + suffix++;
                if (!_newProperty.ContainsKey(newname))
                {
                    break; 
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
                    if (cssClassInfo.Pseudo != psuedo) continue;

                    resultCoreClass = CompareCoreClass(cssClassInfo.CoreClass, _xhtmlClassAttrib, multiClass);
                    resultTagClass = true;
                    //if (_isTagClass != string.Empty && cssClassInfo.Tag.ClassName != string.Empty)
                    if (cssClassInfo.TagName != string.Empty)
                    {
                        //string xhtmlTag = _tagType;
                        //if(_lang.Length > 0)
                        //    xhtmlTag =  _tagType + Common.SepAttrib + _lang;
                        //if (cssClassInfo.TagName != xhtmlTag)
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
                                //_psuedoBeforeStyle = cssClassInfo;
                                _psuedoBeforeStyle = SetClassInfo(cssClassInfo.CoreClass.ClassName, cssClassInfo);
                            }
                            else if (psuedo == "after")
                            {
                                //_psuedoAfterStyle = cssClassInfo;
                                _psuedoAfterStyle = SetClassInfo(cssClassInfo.CoreClass.ClassName, cssClassInfo);
                            }
                            else if (psuedo == "contains")
                            {
                                //_psuedoContainsStyle = cssClassInfo;
                                _psuedoContainsStyle = SetClassInfo(cssClassInfo.CoreClass.ClassName, cssClassInfo);
                            }
                        }
                    }
                }
            }
            if (_outputType == Common.OutputType.IDML)
            {
                AppendParentProperty(); 
            }
            //else if (_outputType == Common.OutputType.ODT)
            //{
            //    ParentClass[_matchedCssStyleName] = _parentStyleName + "||" + _tagType;
            //}
            return _matchedCssStyleName;
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
            //cssClassInfoTag.CoreClass.ClassName = multiClass;

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
                foreach (KeyValuePair<string, string> property in IdAllClass[parentStyle])
                {
                    if (_tempStyle.ContainsKey(property.Key) || property.Key == "TextColumnCount") continue;
                    _tempStyle[property.Key] = property.Value;
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
                if (property.Value.IndexOf("%") > 0)
                {
                    float value = float.Parse(property.Value.Replace("%", ""));
                    _tempStyle[property.Key] = (ancestorFontSize * value / 100).ToString();
                }
                else
                {
                    _tempStyle[property.Key] = property.Value;
                }
            }
            //if (IdAllClass[cssStyleName].ContainsKey("TextColumnGutter"))
            //{
            //    IdAllClass[cssStyleName]["TextColumnGutter"] = _tempStyle["TextColumnGutter"];
            //}
            WordCharSpace(ancestorFontSize);
        }

        private bool CompareCoreClass(ClassAttrib classAttrib, ClassAttrib xhtmlClassInfo, string cssTagFamily)
        {
            if (_isclassNameExist == false)
            {
                if(classAttrib.ClassName == string.Empty) // Tag class
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

            ArrayList superList = new ArrayList();
            superList.AddRange(super);

            for (int i = 0; i < sub.Length - 1; i++)
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


        private float FindAncestorFontSize()
        {
            // TODO find from the current style ex: font-size:20pt; line-height: 1em;
            string[] ancestorStyleName = _allStyle.ToArray();
            float fontSize = 12F;

            // Search in ancestor class
            foreach (string ancestor in ancestorStyleName)
            {
                if (IdAllClass.ContainsKey(ancestor))
                {
                    if (IdAllClass[ancestor].ContainsKey("PointSize"))
                    {
                        fontSize = float.Parse(IdAllClass[ancestor]["PointSize"]);
                        break;
                    }
                }
            }

            // Search in current class
            if (IdAllClass.ContainsKey(_classNameWithLang) && IdAllClass[_classNameWithLang].ContainsKey("PointSize"))
            {
                string currentFontSize = IdAllClass[_classNameWithLang]["PointSize"];
                if (currentFontSize.IndexOf("%") > 0)
                {
                    float value = float.Parse(currentFontSize.Replace("%", ""));
                    fontSize = fontSize * value / 100;
                    IdAllClass[_classNameWithLang]["PointSize"] = fontSize.ToString();
                }
                else if (currentFontSize == "larger" || currentFontSize == "smaller")
                {
                    fontSize = Common.GetLargerSmaller(fontSize, currentFontSize);
                    IdAllClass[_classNameWithLang]["PointSize"] = fontSize.ToString();
                }
                else
                {
                    fontSize = float.Parse(currentFontSize);
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

        protected ClassInfo StackPop(Stack<ClassInfo> stack)
        {
            ClassInfo result = new ClassInfo();
            result = null;
            if (stack != null && stack.Count > 0)
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

        protected ClassAttrib GetParent()
        {
            ClassAttrib result = new ClassAttrib();
            if (_allStyleInfo != null && _allStyleInfo.Count > 0)
            {
                ClassInfo classInfo = new ClassInfo();
                classInfo = _allStyleInfo.Peek();
                result.SetClassAttrib(classInfo.CoreClass.ClassName, classInfo.CoreClass.Attribute);
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
