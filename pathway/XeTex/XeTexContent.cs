// --------------------------------------------------------------------------------------------
// <copyright file="InStory.cs" from='2009' to='2009' company='SIL International'>
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
using System.Text;
using System.Xml;
using System.Drawing;
using System.IO;
using SIL.Tool;

#endregion Using

namespace SIL.PublishingSolution
{
    public class XeTexContent : XHTMLProcess
    {
        #region Private Variable
        private int _storyNo = 0;
        private int _hyperLinkNo = 0;
        private bool isFileEmpty = true;
        private bool isFileCreated;
        private bool isImage;
        private bool isHomographNumber =  false;
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
        private string _dropCapStyle = string.Empty;
        private string _currentStoryName = string.Empty;
        Dictionary<string, List<string>> _classInlineStyle = new Dictionary<string, List<string>>();
        private string xhtmlFile;
        
        protected Stack<string> _braceClass = new Stack<string>();
        protected Stack<string> _braceInlineClass = new Stack<string>();
        protected Dictionary<string, int> _braceInlineClassCount = new Dictionary<string, int>();
        protected Stack<string> _mathStyleClass = new Stack<string>();
        private List<string> _mathStyle = new List<string>();
        
        #endregion

        public Dictionary<string, Dictionary<string, string>> CreateContent(string projectPath, Dictionary<string, Dictionary<string, string>> cssClass, StreamWriter xetexFile, string xhtmlFileWithPath, Dictionary<string, List<string>> classInlineStyle, Dictionary<string, ArrayList> classFamily, ArrayList cssClassOrder)
        {
            _xetexFile = xetexFile;
            _classInlineStyle = classInlineStyle;
            _inputPath = Path.GetDirectoryName(xhtmlFileWithPath);
            xhtmlFile = xhtmlFileWithPath;
            Dictionary<string, Dictionary<string, string>> idAllClass = new Dictionary<string, Dictionary<string, string>>();
            InitializeData(projectPath, cssClass, classFamily, cssClassOrder);
            InitializeMathStyle();
            ProcessCounterProperty();
            OpenXhtmlFile(xhtmlFileWithPath);
            ProcessXHTML(xhtmlFileWithPath);
            //UpdateRelativeInStylesXML();
            CloseFile();
            return _newProperty;
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

                //searchKey = "prince-text-replace";
                //if (IdAllClass[className].ContainsKey(searchKey))
                //{
                //    _replaceSymbolToText.Clear();
                //    string[] values = IdAllClass[className][searchKey].Split('\"');
                //    if (values.Length <= 1)
                //        values = IdAllClass[className][searchKey].Split('\'');
                //    for (int i = 0; i < values.Length; i++)
                //    {
                //        if (values[i].Length > 0)
                //        {
                //            string key = values[i].Replace("\"", "");
                //            //key = Common.ReplaceSymbolToText(key);
                //            i++;
                //            i++;
                //            string value = values[i].Replace("\"", "");
                //            value = Common.ReplaceSymbolToText(value);
                //            CssParser cssParser = new CssParser();
                //            _replaceSymbolToText[key] = cssParser.UnicodeConversion(value);
                //        }
                //    }
                //}

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
        }


        private void ProcessXHTML(string xhtmlFileWithPath)
        {
            try
            {
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
                            if (_reader.Value.Replace(" ","") == "")
                            {
                                Write();
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

        private void Write()
        {
            //_imageInserted = InsertImage(); // TODO

            if (isFileCreated == false)
            {
                CreateFile();
                _textFrameClass.Add(_childName);
            }

            if (_isNewParagraph)
            {
                if (_paragraphName == null)
                {
                    _paragraphName = StackPeek(_allParagraph); // _allParagraph.Pop();
                }

                ClosePara();

                DoNotInheritClassStart();

                _writer.WriteStartElement("ParagraphStyleRange");
                // Note: Paragraph Start Element
                _writer.WriteAttributeString("AppliedParagraphStyle", "ParagraphStyle/" + _paragraphName);
                AddUsedStyleName(_paragraphName);
                _previousParagraphName = _paragraphName;
                _paragraphName = null;
                _isNewParagraph = false;
                _isParagraphClosed = false;
            }
            ////Note - for Xetex Added for Extra Line for Paragraph
            //if (_tagType == "div")
            //{
            //    _xetexFile.Write("\r\n");
            //}

            WriteText();
            isFileEmpty = false;
        }

        private void DoNotInheritClassStart()
        {
            if (_doNotInheritClass.Length == 0) return;

            string property = "SpaceBefore";
            if (IdAllClass.ContainsKey(_doNotInheritClass)==false)
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

            if (CollectFootNoteChapterVerse(content, Common.OutputType.XETEX.ToString())) return;

            // Psuedo Before
            foreach (ClassInfo psuedoBefore in _psuedoBefore)
            {
                WriteCharacterStyle(psuedoBefore.Content, psuedoBefore.StyleName);
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
            WriteCharacterStyle(modifiedContent, _characterName);

            _psuedoBefore.Clear();
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
            return content;
        }

        private void WriteCharacterStyle(string content, string characterStyle)
        {
            //_imageInserted = InsertImage();
            SetHomographNumber(false);
            _writer.WriteStartElement("CharacterStyleRange");

            string footerClassName = WritePara(characterStyle);

            AnchorBookMark();

            if (isFootnote)
            {
                WriteFootNoteMarker(footerClassName, content);
            }
            else
            {
                _writer.WriteStartElement("Content");
                content = WriteCounter(content);
                content = whiteSpacePre(content);
                if (_isDropCap)
                {
                    content = _chapterNo;
                    _isDropCap = false;
                }
                
                _xetexFile.Write(content);
                _xetexFile.Write("}");
                if(_tagType == "div")
                    _xetexFile.Write("\r\n");
                _writer.WriteEndElement();
            }
            AnchorBookMark();
            _writer.WriteEndElement();
            //if (_tagType == "li")
            //    _writer.WriteRaw("<Br/>");
        }

        private string WritePara(string characterStyle)
        {
            string footerClassName = string.Empty;
            if (isFootnote)
            {
                footerClassName = Common.LeftString(characterStyle, "_");
                _writer.WriteAttributeString("AppliedCharacterStyle", "CharacterStyle/" + footerClassName + "..footnote-call");
            }
            else // regular style
            {
                _writer.WriteAttributeString("AppliedCharacterStyle", "CharacterStyle/" + characterStyle);
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
                    foreach (string property in inlineStyle)
                    {
                        if (_mathStyle.Contains(property))
                        {
                            _xetexFile.Write("$");
                            _mathStyleClass.Push(getStyleName);
                        }
                        _xetexFile.Write(property);
                        _xetexFile.Write("{");
                    }

                    if (inlineStyle.Count > 0)
                    {
                        _braceInlineClassCount[_childName] = inlineStyle.Count;
                        _braceInlineClass.Push(getStyleName);
                    }

                    if (mergedParaStyle.IndexOf(Common.SepPseudo) > 0)
                        mergedParaStyle = mergedParaStyle.Replace(Common.SepPseudo, "");

                    _xetexFile.Write("\\" + mergedParaStyle + "{");
                    //_braceClass.Push(getStyleName);
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
                //Merge
                if (_classInlineStyle.ContainsKey(parentClass))
                {
                    foreach (string sty in _classInlineStyle[parentClass])
                    {
                        if (!_mergedInlineStyle.Contains(sty))
                            _mergedInlineStyle.Add(sty);
                    }
                }
                _classInlineStyle[mergedClass] = _mergedInlineStyle;
            }
            return mergedClass;
        }

        private void WriteFootNoteMarker(string footerClassName, string content)
        {
            //Todo - Unicode marker
            string markerClassName = footerClassName + "..footnote-marker";
            string marker = _footNoteMarker[markerClassName];
            _xetexFile.Write("  \\footnote {" + marker +  "} ");
            _xetexFile.Write("{");
            _xetexFile.Write(content);
            _xetexFile.Write("}");
        }

        private void AnchorBookMark()
        {
            if (_anchorBookMarkName != string.Empty)
            {
                string status = _anchorBookMarkName.Substring(0, 4);
                if (status == "href")
                {
                    string bookMark = _anchorBookMarkName.Replace("href#", "");
                    _writer.WriteStartElement("HyperlinkTextSource");
                    _writer.WriteAttributeString("Self", "Hyperlink_" + bookMark);
                    _writer.WriteAttributeString("Name", "Hyperlink " + bookMark);
                    _writer.WriteAttributeString("Hidden", "false");
                    _writer.WriteAttributeString("AppliedCharacterStyle", "n");
                    _anchorBookMarkName = "endbookmark";
                }
                else if (status == "name")
                {
                    _writer.WriteStartElement("HyperlinkTextDestination");
                    _writer.WriteAttributeString("Self", "Name_" + _anchorBookMarkName);
                    _writer.WriteAttributeString("Name", "Name_" + _anchorBookMarkName);
                    _writer.WriteAttributeString("Hidden", "false");

                    _writer.WriteAttributeString("DestinationUniqueKey", _crossRefCounter.ToString());
                    _crossRefCounter++;
                    _writer.WriteEndElement();
                    string bookMark = _anchorBookMarkName.Replace("name", "");
                    if (!_crossRef.Contains(bookMark))
                        _crossRef.Add(bookMark);
                    _anchorBookMarkName = string.Empty;
                }
                else if (status == "endb")
                {
                    _writer.WriteEndElement();
                    _anchorBookMarkName = string.Empty;
                }
            }
        }

        public bool InsertImage()
        {
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

                isImage = true;
                inserted = true;
                string[] cc = _allParagraph.ToArray();
                imageClass = cc[1];
                srcFile = _imageSource.ToLower();
                string fileName = "file:" + Common.GetPictureFromPath(srcFile, "", _inputPath);
                string fileName1 = Common.GetPictureFromPath(srcFile, "", _inputPath);
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
                        rectWidth = Common.CalcDimension(fileName1, rectHeight, 'W');
                    }
                }
                else if (rectWidth != "0")
                {
                    rectHeight = Common.CalcDimension(fileName1, rectWidth, 'H');
                }
                else
                {
                    //Default value is 72 However the line draws 36pt in X-axis and 36pt in y-axis.
                    //rectHeight = "36";
                    rectWidth = "36"; // fixed the width as 1 in;
                    rectHeight = Common.CalcDimension(fileName1, rectWidth, 'H');
                    if (rectHeight == "0")
                    {
                        rectHeight = "36";
                    }
                }

                double x = double.Parse(rectWidth) / 2;
                double y = double.Parse(rectHeight) / 2;

                string xPlus = x.ToString();
                string xMinus = "-" + xPlus;

                string yPlus = y.ToString();
                string yMinus = "-" + yPlus;

                int height = 72; //1 in
                int width = 72; // 1 in

                string picFile = string.Empty;
                //TODO Make it function 
                //To get Image details
                if (File.Exists(fileName1))
                {
                    picFile = Path.GetFileName(fileName1);
                    string toPath = Path.Combine(_inputPath,picFile );
                    File.Copy(fileName1,toPath,true);
                    //Dictionary<string, Dictionary<string, string>>() 
                    Dictionary<string, string> prop = new Dictionary<string, string>();
                    prop.Add("filePath",fileName1);
                    _newProperty["ImagePath"] = prop;

                    Image fullimage = Image.FromFile(fileName1);
                    height = fullimage.Height;
                    width = fullimage.Width;
                    
                }

                _xetexFile.WriteLine("");
                _xetexFile.WriteLine("");

                string p1 = @"\def\leftpicpar#1{\setbox0=\hbox{\XeTeXpicfile #1}";
                string p2 = @"\dimen0=\wd0 \advance\dimen0 by 3pt";
                string p3 = @"\count255=\ht0 \advance\count255 by \baselineskip";
                string p4 = @"\divide\count255 by \baselineskip";
                string p5 = @"\hangindent\dimen0 \hangafter-\count255";
                string p6 = @"\noindent\llap{\vbox to 0pt{\kern-0.7\baselineskip\box0\vss}\kern3pt}";
                string p7 = @"\indent\ignorespaces}";

                _xetexFile.Write(p1 + p2 + p3 + p4 + p5 + p6 + p7);
                _xetexFile.WriteLine("");
                string wp = "\\leftpicpar{\"" + picFile + "\" scaled 400}";
                _xetexFile.Write(wp);
                _xetexFile.WriteLine("");
                _xetexFile.WriteLine("");
                _imageInsert = false;
                _imageSource = string.Empty;
                _isNewParagraph = false;
            }
            return inserted;
        }

        private void GetHeightandWidth(ref string height, ref string width)
        {
            if (_allCharacter.Count > 0)
            {
                string stackClass = _allCharacter.Peek();
                string[] splitedClassName = stackClass.Split('_');
                
                string[] allClasses = new string[splitedClassName.Length + 1];
                splitedClassName.CopyTo(allClasses,1);
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
                        if(ht.Length > 0 || wd.Length > 0)
                        {
                            if(ht.Length > 0)
                            height = ht;

                            if(wd.Length>0)
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
            //if (IdAllClass.ContainsKey(clsName) && IdAllClass[clsName].ContainsKey(property))
            //{
            //    valueOfProperty = IdAllClass[clsName][property];
            //}
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
                //string stackClass2 = _allStyle.Peek();
                //string stackClass1 = _allParagraph.Peek();
                //string stackClass = _allCharacter.Peek();
                //string[] splitedClassName = stackClass.Split('_');

                string[] splitedClassName = _allParagraph.ToArray(); ; // _allStyle.ToArray();

                if (splitedClassName.Length > 0)
                {
                    //for (int i = splitedClassName.Length -1; i >= 0; i--)  // From Begining to Recent Class
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
            SetClassCounter();
            Psuedo();
            DropCaps();
            SetHomographNumber(true);
            FooterSetup();

            if (IdAllClass.ContainsKey(_classNameWithLang))
            {
                bool isPageBreak = false;
                bool isColumnCount = false;
                if (IdAllClass[_classNameWithLang].ContainsKey("PageBreakBefore"))
                {
                    isPageBreak = IdAllClass[_classNameWithLang]["PageBreakBefore"] == "always";
                }
                if (IdAllClass[_classNameWithLang].ContainsKey("TextColumnCount"))
                {
                    isColumnCount = int.Parse(IdAllClass[_classNameWithLang]["TextColumnCount"]) > 1;
                }
                if (isPageBreak || isColumnCount)
                {
                    if (!isFileEmpty)
                    {
                        CloseFile();
                    }
                    CreateFile();
                    _textFrameClass.Add(_childName);
                    _columnClass.Add(_childName);
                }
            }
        }

        private void SetHeadwordTrue()
        {
            if(_reader.GetAttribute("class") != null &&_reader.GetAttribute("class").ToLower() == "headword")
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
            string inner = string.Empty; //_reader.ReadString();

            if (classNameWOLang == "ChapterNumber")
                _chapterNo = _reader.ReadString();

            if (IdAllClass.ContainsKey(classNameWOLang) && IdAllClass[classNameWOLang].ContainsKey("float") && IdAllClass[classNameWOLang].ContainsKey("BaselineShift"))
            {
                Dictionary<string, string> mystyle = new Dictionary<string, string>();
                _isDropCap = true;
                string lines = "2";
                _allStyle.Pop();
                CollectFootNoteChapterVerse(_chapterNo, Common.OutputType.XETEX.ToString());
                
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
            //WriteEmptyHomographStyle();
            _closeChildName = StackPop(_allStyle);
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

            EndElementBase();
            if (_columnClass.Count > 0)
            {
                if (_closeChildName == _columnClass[_columnClass.Count - 1].ToString())
                {
                    _columnClass.RemoveAt(_columnClass.Count - 1);
                    CloseFile();
                }
            }
            _classNameWithLang = StackPeek(_allStyle);
            _classNameWithLang = Common.LeftString(_classNameWithLang, "_");
        }

        private void CloseBrace(string closeChildName)
        {
            string brace = StackPeek(_braceInlineClass);
            if (brace.Length !=0 && closeChildName == brace)
            {
                int count = _braceInlineClassCount[brace];
                for (int i = 0; i < count; i++)
                {
                    _xetexFile.Write("}");
                }
                StackPop(_braceInlineClass);
            }

            //string clsbrace = StackPeek(_braceClass);
            //if (clsbrace.Length != 0 && closeChildName == clsbrace)
            //{
            //    _xetexFile.Write("}");
            //    StackPop(_braceClass);
            //}

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
            if (_imageInsert && !_imageInserted)
            {
                if (_closeChildName == _imageClass) // Without Caption
                {
                    _allCharacter.Push(_imageClass); // temporarily storing to get width and position
                    //_imageInserted = InsertImage();
                    //_writer.WriteEndElement(); // for ParagraphStyle
                    //_writer.WriteEndElement(); // for Textframe
                    //_writer.WriteEndElement(); // for rectangle
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

                    //_writer.WriteEndElement();// for ParagraphStyle
                    //_writer.WriteEndElement(); // for Textframe
                    //_writer.WriteEndElement(); // for rectangle

                    isImage = false;
                    imageClass = "";
                    _isParagraphClosed = true;
                }
            }
        }

        private void WriteEmptyHomographStyle()
        {
            if (isHomographNumber)
            {
                _writer.WriteStartElement("CharacterStyleRange");
                _writer.WriteAttributeString("AppliedCharacterStyle", "CharacterStyle/xhomographnumber_headword_entry_letData_dicBody");
                _writer.WriteStartElement("Content");
                _writer.WriteString(" ");
                _writer.WriteEndElement();
                _writer.WriteEndElement();
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

        #region Private Methods



        private void InitializeData(string projectPath, Dictionary<string, Dictionary<string, string>> idAllClass, Dictionary<string, ArrayList> classFamily, ArrayList cssClassOrder)
        {
            _outputType = Common.OutputType.XETEX;
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
            //_classFamily = new Dictionary<string, ArrayList>();

            IdAllClass = idAllClass;
            _projectPath = projectPath;
            _classFamily = classFamily;

            _isNewParagraph = false;
            _characterName = "$ID/[No character style]";// "[No character style]"; 
        }

        private void CreateFile()
        {
            string fileName = "Story_" + ++_storyNo + ".xml";
            string storyXMLWithPath = Common.PathCombine(_projectPath, fileName);
            _writer = new XmlTextWriter(storyXMLWithPath, null)
                          {
                              Formatting = Formatting.Indented
                          };
            _writer.WriteStartDocument();

            _writer.WriteStartElement("idPkg:Story");
            _writer.WriteAttributeString("xmlns:idPkg", "http://ns.adobe.com/AdobeInDesign/idml/1.0/packaging");
            _writer.WriteAttributeString("DOMVersion", "6.0");
            _writer.WriteStartElement("Story");
            _writer.WriteAttributeString("Self", _storyNo.ToString());
            _writer.WriteAttributeString("AppliedTOCStyle", "n");
            _writer.WriteAttributeString("TrackChanges", "false");
            _writer.WriteAttributeString("StoryTitle", "$ID/");
            _writer.WriteAttributeString("AppliedNamedGrid", "n");
            _writer.WriteStartElement("StoryPreference");
            _writer.WriteAttributeString("OpticalMarginAlignment", "false");
            _writer.WriteAttributeString("OpticalMarginSize", "12");
            _writer.WriteAttributeString("FrameType", "TextFrameType");
            _writer.WriteAttributeString("StoryOrientation", "Horizontal");
            _writer.WriteAttributeString("StoryDirection", "LeftToRightDirection");
            _writer.WriteEndElement();
            _writer.WriteStartElement("InCopyExportOption");
            _writer.WriteAttributeString("IncludeGraphicProxies", "true");
            _writer.WriteAttributeString("IncludeAllResources", "false");
            _writer.WriteEndElement();
            isFileCreated = true;
            isFileEmpty = true;
        }

        private void CloseFile()
        {
            if (isFileCreated)
            {
                _writer.Flush();
                _writer.Close();
            }
            isFileCreated = false;
            isFileEmpty = true;
            _isParagraphClosed = true;
            _isNewParagraph = true;
        }
        #endregion
    }
}
