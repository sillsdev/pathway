// --------------------------------------------------------------------------------------------
// <copyright file="InStory.cs" from='2009' to='2014' company='SIL International'>
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
// Creates the InDesign InStory file
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
    public class InStory : XHTMLProcess
    {
        #region Private Variable
        private int _storyNo = 0;
        private bool isFileEmpty = true;
        private bool isFileCreated;
        private string imageClass = string.Empty;
        private ArrayList _textFrameClass = new ArrayList();
        private ArrayList _textVariables = new ArrayList();
        private ArrayList _columnClass = new ArrayList();
        private ArrayList _psuedoBefore = new ArrayList();
        private Dictionary<string, ClassInfo> _psuedoAfter = new Dictionary<string, ClassInfo>();
        private Dictionary<string, ArrayList> _styleName = new Dictionary<string, ArrayList>();
        private ArrayList _crossRef = new ArrayList();
        private int _crossRefCounter = 1;
        private bool _isWhiteSpace = true;
        private bool _IdImageInserted;
        private List<string> _IdUsedStyleName = new List<string>();
        private bool _IsHeadword = false;
        private bool _isIdDropCap = false;
        #endregion

        public Dictionary<string, ArrayList> CreateStory(PublicationInformation projInfo, Dictionary<string, Dictionary<string, string>> idAllClass, Dictionary<string, ArrayList> classFamily, ArrayList cssClassOrder)
        {
            _projInfo = projInfo;
            Common.PathCombine(projInfo.DictionaryPath, "Pictures");
            InitializeData(Common.PathCombine(projInfo.TempOutputFolder, "Stories"), idAllClass, classFamily, cssClassOrder);
            ProcessCounterProperty();
            OpenXhtmlFile(projInfo.DefaultXhtmlFileWithPath);
            ProcessXHTML(projInfo.DefaultXhtmlFileWithPath);
            UpdateRelativeInStylesXML();
            CloseFile();
            SetColumnValue();
            return _styleName;
        }

        private void SetColumnValue()
        {
            _styleName["ColumnClass"] = _textFrameClass;
            _styleName["TextVariables"] = _textVariables;
            _styleName["CrossRef"] = _crossRef;
            if (_headwordStyleName.Count > 0)
                _styleName["TextVariables"].AddRange(_headwordStyleName);
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
                string searchKey1 = "..footnote-call";
                if (className.IndexOf(searchKey1) >= 0)
                {
                    if (!_footnoteCallContent.Contains(className))
                        _footnoteCallContent.Add(className);
                }
                string searchKey2 = "..footnote-marker";
                if (className.IndexOf(searchKey2) >= 0)
                {
                    if (!_footnoteMarkerContent.Contains(className))
                        _footnoteMarkerContent.Add(className);
                }
            }
        }

        private void ProcessXHTML(string xhtmlFileWithPath)
        {
            try
            {
                using (_reader = Common.DeclareXmlTextReader(xhtmlFileWithPath, true))
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
                                StartElement();
                                break;
                            case XmlNodeType.Text:
                                Write();
                                break;
                            case XmlNodeType.EndElement:
                                EndElement();
                                break;
                            case XmlNodeType.SignificantWhitespace:
                                if (_reader.Value.Replace(" ", "") == "")
                                {
                                    Write();
                                }
                                break;
                        }
                    }
                }
                _reader.Close();
            }
            catch (XmlException e)
            {
                var msg = new[] { e.Message, xhtmlFileWithPath };
            }
        }

        private void Write()
        {
            if (isFileCreated == false)
            {
                CreateFile();
                _textFrameClass.Add(_childName);
            }

            if (_isNewParagraph)
            {
                if (_paragraphName == null)
                {
                    _paragraphName = StackPeek(_allParagraph); 
                }

                ClosePara(false);

                DoNotInheritClassStart();

                _writer.WriteStartElement("ParagraphStyleRange");
                // Note: Paragraph Start Element
                _writer.WriteAttributeString("AppliedParagraphStyle", "ParagraphStyle/" + _paragraphName);
                AddIdUsedStyleName(_paragraphName);
                _previousParagraphName = _paragraphName;
                _paragraphName = null;
                _isNewParagraph = false;
                _isParagraphClosed = false;
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

            if (CollectFootNoteChapterVerse(content, Common.OutputType.IDML.ToString())) return;

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
            _IdImageInserted = InsertImage();
            SetHomographNumber(false);
            _writer.WriteStartElement("CharacterStyleRange");

            string footerClassName = GetFooterClassName(characterStyle);

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
                if (_isIdDropCap)
                {
                    content = _chapterNo;
                    _isIdDropCap = false;
                }

                _writer.WriteString(content);
                _writer.WriteEndElement();

            }
            AnchorBookMark();
            _writer.WriteEndElement();
        }

        private string GetFooterClassName(string characterStyle)
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
                AddIdUsedStyleName(characterStyle);
            }
            return footerClassName;
        }

        private void WriteFootNoteMarker(string footerClassName, string content)
        {
            _writer.WriteAttributeString("Position", "Superscript");
            _writer.WriteStartElement("Footnote");
            _writer.WriteStartElement("ParagraphStyleRange");
            _writer.WriteAttributeString("AppliedParagraphStyle", "ParagraphStyle/" + footerClassName + "..footnote-marker");
            _writer.WriteStartElement("CharacterStyleRange");
            _writer.WriteAttributeString("AppliedCharacterStyle", "CharacterStyle/$ID/[No character style]");
            _writer.WriteStartElement("Content");
            _writer.WriteRaw("<?ACE 4?> ");
            string replace = content.Replace("\r\n", " ");
            string leftPart = Common.LeftString(replace, "<");
            _writer.WriteString(leftPart);
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            string replaceLeft = content;
            if (leftPart.Length > 0)
                replaceLeft = content.Replace(leftPart, "");
            replace = replaceLeft.Replace("\r\n", " ");
            _writer.WriteRaw(replace);
            _writer.WriteEndElement();
            _writer.WriteEndElement();
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
	            string VertAlignment = "CenterAlign";
                string VertRefPoint = "LineBaseline";
                string AnchorPoint = "TopLeftAnchor";
                int height = 72; //1 in
                int width = 72; // 1 in


                inserted = true;
                string[] cc = _allParagraph.ToArray();
                imageClass = cc[1];
                srcFile = _imageSource.ToLower();
                if(srcFile.ToLower().IndexOf("pictures") == -1)
                {
                    srcFile = "Pictures/" + srcFile;
                }
                string fileName = "file:" + Common.GetPictureFromPath(srcFile, "", _projInfo.DictionaryPath);
                string fileName1 = Common.GetPictureFromPath(srcFile, "", _projInfo.DictionaryPath);
                //To get Image details
                if (File.Exists(fileName1))
                {
                    Image fullimage = Image.FromFile(fileName1);
                    height = fullimage.Height;
                    width = fullimage.Width;
                }
                string imgCls = Common.LeftString(imageClass, "_");
                if (IdAllClass.ContainsKey(imgCls))
                {
                    rectWidth = GetPropertyValue(imgCls, "width", rectWidth);
                    rectHeight = GetPropertyValue(imgCls, "height", rectHeight);
                    GetAlignment(alignment, ref HoriAlignment, ref AnchorPoint, imgCls);
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
                        rectWidth = Common.CalcDimension(fileName1, ref rectHeight, Common.CalcType.Width);
                    }
                }
                else if (rectWidth != "0")
                {
                    rectHeight = Common.CalcDimension(fileName1, ref rectWidth, Common.CalcType.Height);
                }
                else
                {
                    //Default value is 72 However the line draws 36pt in X-axis and 36pt in y-axis.
                    rectWidth = "36"; // fixed the width as 1 in;
                    rectHeight = Common.CalcDimension(fileName1, ref rectWidth, Common.CalcType.Height);
                    if (rectHeight == "0")
                    {
                        rectHeight = "36";
                    }
                }
                rectWidth = rectWidth.Replace("%", "");
                double x = double.Parse(rectWidth, CultureInfo.GetCultureInfo("en-US")) / 2;
                double y = double.Parse(rectHeight, CultureInfo.GetCultureInfo("en-US")) / 2;

                string xPlus = x.ToString();
                string xMinus = "-" + xPlus;

                string yPlus = y.ToString();
                string yMinus = "-" + yPlus;

                //To get Image details
                int xx = GCD(width, height);
                string xxw = string.Format("{0}:{1}", width/xx, height/xx);

                string imageFloatLeftRightOrCenter = string.Empty;
                string anchorPoint = string.Empty;
                string horizontalAlignment = string.Empty;
                imageFloatLeftRightOrCenter = GetPropertyValue(_imageSrcClass + "Right", "float");
                
                if (imageFloatLeftRightOrCenter == string.Empty)
                {
                    imageFloatLeftRightOrCenter = GetPropertyValue(_imageSrcClass + "Left", "float");
                }
                else if (imageFloatLeftRightOrCenter == string.Empty)
                {
                    imageFloatLeftRightOrCenter = GetPropertyValue(_imageSrcClass + "Center", "float");
                }
                else if (imageFloatLeftRightOrCenter == string.Empty)
                {
                    imageFloatLeftRightOrCenter = GetPropertyValue(_imageSrcClass, "float");
                }
                
                
                if (imageFloatLeftRightOrCenter.ToLower() == "right")
                {
                    anchorPoint = "TopRightAnchor";
                    horizontalAlignment = "RightAlign";
                }
                else if (imageFloatLeftRightOrCenter.ToLower() == "center")
                {
                    anchorPoint = "TopCenterAnchor";
                    horizontalAlignment = "CenterAlign";
                }
                else if (imageFloatLeftRightOrCenter.ToLower() == "left")
                {
                    anchorPoint = "TopLeftAnchor";
                    horizontalAlignment = "LeftAlign";
                }
                else
                {
                    anchorPoint = "TopCenterAnchor";
                    horizontalAlignment = "CenterAlign";
                }

                // Writing the Images
                _writer.WriteStartElement("Rectangle");
                _writer.WriteAttributeString("Self", "u1fa");
                _writer.WriteAttributeString("StoryTitle", "$ID/");
                _writer.WriteAttributeString("ContentType", "GraphicType");
                _writer.WriteAttributeString("StrokeWeight", "0");
                _writer.WriteAttributeString("StrokeColor", "Swatch/None");
                _writer.WriteAttributeString("GradientFillStart", "0 0");
                _writer.WriteAttributeString("GradientFillLength", "0");
                _writer.WriteAttributeString("GradientFillAngle", "0");
                _writer.WriteAttributeString("GradientStrokeStart", "0 0");
                _writer.WriteAttributeString("GradientStrokeLength", "0");
                _writer.WriteAttributeString("GradientStrokeAngle", "0");
                _writer.WriteAttributeString("Locked", "false");
                _writer.WriteAttributeString("LocalDisplaySetting", "Default");
                _writer.WriteAttributeString("GradientFillHiliteLength", "0");
                _writer.WriteAttributeString("GradientFillHiliteAngle", "0");
                _writer.WriteAttributeString("GradientStrokeHiliteLength", "0");
                _writer.WriteAttributeString("GradientStrokeHiliteAngle", "0");
                _writer.WriteAttributeString("AppliedObjectStyle", "ObjectStyle/$ID/[Normal Graphics Frame]");
                _writer.WriteAttributeString("ItemTransform", "1 0 0 1 " + xPlus + " " + yMinus); // 36 -36 
                _writer.WriteStartElement("Properties");
                _writer.WriteStartElement("PathGeometry");
                _writer.WriteStartElement("GeometryPathType");
                _writer.WriteAttributeString("PathOpen", "false");
                _writer.WriteStartElement("PathPointArray");
                _writer.WriteStartElement("PathPointType");
                _writer.WriteAttributeString("Anchor", xMinus + " " + yMinus);
                _writer.WriteAttributeString("LeftDirection", xMinus + " " + yMinus);
                _writer.WriteAttributeString("RightDirection", xMinus + " " + yMinus);
                _writer.WriteEndElement();
                _writer.WriteStartElement("PathPointType");
                _writer.WriteAttributeString("Anchor", xMinus + " " + yPlus);
                _writer.WriteAttributeString("LeftDirection", xMinus + " " + yPlus);
                _writer.WriteAttributeString("RightDirection", xMinus + " " + yPlus);
                _writer.WriteEndElement();
                _writer.WriteStartElement("PathPointType");
                _writer.WriteAttributeString("Anchor", xPlus + " " + yPlus);
                _writer.WriteAttributeString("LeftDirection", xPlus + " " + yPlus);
                _writer.WriteAttributeString("RightDirection", xPlus + " " + yPlus);
                _writer.WriteEndElement();
                _writer.WriteStartElement("PathPointType");
                _writer.WriteAttributeString("Anchor", xPlus + " " + yMinus);
                _writer.WriteAttributeString("LeftDirection", xPlus + " " + yMinus);
                _writer.WriteAttributeString("RightDirection", xPlus + " " + yMinus);
                _writer.WriteEndElement();
                _writer.WriteEndElement();
                _writer.WriteEndElement();
                _writer.WriteEndElement();
                _writer.WriteEndElement();

                _writer.WriteStartElement("AnchoredObjectSetting");
                _writer.WriteAttributeString("AnchoredPosition", "Anchored");
                _writer.WriteAttributeString("SpineRelative", "false");
                _writer.WriteAttributeString("LockPosition", "false");
                _writer.WriteAttributeString("PinPosition", "false");
                _writer.WriteAttributeString("AnchorPoint", anchorPoint);
                //CenterAlign, RightAlign , LeftAlign
                _writer.WriteAttributeString("HorizontalAlignment", horizontalAlignment);
                _writer.WriteAttributeString("HorizontalReferencePoint", "ColumnEdge");
                _writer.WriteAttributeString("VerticalAlignment", "CenterAlign");
                _writer.WriteAttributeString("VerticalReferencePoint", "LineBaseline");
                //InLIne  //AboveLine
                _writer.WriteAttributeString("AnchorXoffset", "0");
                _writer.WriteAttributeString("AnchorYoffset", "0");
                _writer.WriteAttributeString("AnchorSpaceAbove", "4");
                _writer.WriteEndElement();

                _writer.WriteStartElement("TextWrapPreference");
                _writer.WriteAttributeString("Inverse", "false");
                _writer.WriteAttributeString("ApplyToMasterPageOnly", "false");
                _writer.WriteAttributeString("TextWrapSide", wrapSide);
                _writer.WriteAttributeString("TextWrapMode", wrapMode);
                _writer.WriteStartElement("Properties");
                _writer.WriteStartElement("TextWrapOffset");
                _writer.WriteAttributeString("Top", "0");
                _writer.WriteAttributeString("Left", "0");
                _writer.WriteAttributeString("Bottom", "0");
                _writer.WriteAttributeString("Right", "0");
                _writer.WriteEndElement();
                _writer.WriteEndElement();

                _writer.WriteStartElement("ContourOption");
                _writer.WriteAttributeString("ContourType", "SameAsClipping");
                _writer.WriteAttributeString("IncludeInsideEdges", "false");
                _writer.WriteAttributeString("ContourPathName", "$ID/");
                _writer.WriteEndElement();
                _writer.WriteEndElement();
                _writer.WriteStartElement("InCopyExportOption");
                _writer.WriteAttributeString("IncludeGraphicProxies", "true");
                _writer.WriteAttributeString("IncludeAllResources", "false");
                _writer.WriteEndElement();
                _writer.WriteStartElement("FrameFittingOption");
                _writer.WriteAttributeString("LeftCrop", "0");
                _writer.WriteAttributeString("TopCrop", "0");
                _writer.WriteAttributeString("RightCrop", "0");
                _writer.WriteAttributeString("BottomCrop", "0");
                _writer.WriteAttributeString("FittingOnEmptyFrame", "ContentToFrame");
                _writer.WriteAttributeString("FittingAlignment", "CenterAnchor");

                _writer.WriteEndElement();
                _writer.WriteStartElement("Image");
                _writer.WriteAttributeString("Self", "u222");
                _writer.WriteAttributeString("Space", "$ID/#Links_RGB");
                _writer.WriteAttributeString("ActualPpi", (x * 2) + " " + (y * 2));
                _writer.WriteAttributeString("EffectivePpi", width + " " + height);
                _writer.WriteAttributeString("ImageRenderingIntent", "UseColorSettings");
                _writer.WriteAttributeString("LocalDisplaySetting", "Default");
                _writer.WriteAttributeString("ImageTypeName", "$ID/JPEG");
                _writer.WriteAttributeString("AppliedObjectStyle", "ObjectStyle/$ID/[None]");
                //TODO Make it function 
                double ImageX = (x * 2) / width;
                double ImageY = (y * 2) / height;
                _writer.WriteAttributeString("ItemTransform",
                                             ImageX + " 0 0 " + ImageY + " " + xMinus + " " + yMinus);
                _writer.WriteStartElement("Properties");
                _writer.WriteStartElement("Profile");
                _writer.WriteAttributeString("type", "string");
                _writer.WriteString("$ID/None");
                _writer.WriteEndElement();
                _writer.WriteStartElement("GraphicBounds");
                _writer.WriteAttributeString("Left", "0");
                _writer.WriteAttributeString("Top", "0");
                _writer.WriteAttributeString("Right", width.ToString());
                _writer.WriteAttributeString("Bottom", height.ToString());
                _writer.WriteEndElement();
                _writer.WriteEndElement();
                _writer.WriteStartElement("TextWrapPreference");
                _writer.WriteAttributeString("Inverse", "false");
                _writer.WriteAttributeString("ApplyToMasterPageOnly", "false");
                _writer.WriteAttributeString("TextWrapSide", "BothSides");
                _writer.WriteAttributeString("TextWrapMode", "None");
                _writer.WriteStartElement("Properties");
                _writer.WriteStartElement("TextWrapOffset");
                _writer.WriteAttributeString("Top", "0");
                _writer.WriteAttributeString("Left", "0");
                _writer.WriteAttributeString("Bottom", "0");
                _writer.WriteAttributeString("Right", "0");
                _writer.WriteEndElement();
                _writer.WriteEndElement();
                _writer.WriteStartElement("ContourOption");
                _writer.WriteAttributeString("ContourType", "SameAsClipping");
                _writer.WriteAttributeString("IncludeInsideEdges", "false");
                _writer.WriteAttributeString("ContourPathName", "$ID/");
                _writer.WriteEndElement();
                _writer.WriteEndElement();

                _writer.WriteStartElement("Link");
                _writer.WriteAttributeString("Self", "u225");
                _writer.WriteAttributeString("AssetURL", "$ID/");
                _writer.WriteAttributeString("AssetID", "$ID/");
                _writer.WriteAttributeString("LinkResourceURI", fileName);
                _writer.WriteAttributeString("StoredState", "Normal");
                _writer.WriteAttributeString("LinkClassID", "35906");
                _writer.WriteAttributeString("LinkClientID", "257");
                _writer.WriteAttributeString("LinkResourceModified", "false");
                _writer.WriteAttributeString("LinkObjectModified", "false");
                _writer.WriteAttributeString("ShowInUI", "true");
                _writer.WriteAttributeString("CanEmbed", "true");
                _writer.WriteAttributeString("CanUnembed", "true");
                _writer.WriteAttributeString("CanPackage", "true");
                _writer.WriteAttributeString("ImportPolicy", "NoAutoImport");
                _writer.WriteAttributeString("ExportPolicy", "NoAutoExport");
                _writer.WriteAttributeString("LinkImportStamp", "file 128811558721632778 187452");
                _writer.WriteAttributeString("LinkImportModificationTime", "2009-03-10T16:21:12");
                _writer.WriteAttributeString("LinkImportTime", "2009-12-17T16:28:20");
                _writer.WriteEndElement();
                _writer.WriteStartElement("ClippingPathSettings");
                _writer.WriteAttributeString("ClippingType", "None");
                _writer.WriteAttributeString("InvertPath", "false");
                _writer.WriteAttributeString("IncludeInsideEdges", "false");
                _writer.WriteAttributeString("RestrictToFrame", "false");
                _writer.WriteAttributeString("UseHighResolutionImage", "true");
                _writer.WriteAttributeString("Threshold", "25");
                _writer.WriteAttributeString("Tolerance", "2");
                _writer.WriteAttributeString("InsetFrame", "0");
                _writer.WriteAttributeString("AppliedPathName", "$ID/");
                _writer.WriteAttributeString("Index", "-1");
                _writer.WriteEndElement();
                _writer.WriteStartElement("ImageIOPreference");
                _writer.WriteAttributeString("ApplyPhotoshopClippingPath", "true");
                _writer.WriteAttributeString("AllowAutoEmbedding", "true");
                _writer.WriteAttributeString("AlphaChannelName", "$ID/");
                _writer.WriteEndElement();
                _writer.WriteEndElement();

                _writer.WriteStartElement("TextFrame");
                _writer.WriteAttributeString("Self", "u" + Path.GetFileNameWithoutExtension(fileName1));
                _writer.WriteStartElement("ParagraphStyleRange");
                
                Stack<string> _dupParagraph;
                string pictureCaption = string.Empty;
                _dupParagraph = _allParagraph;

                if (_dupParagraph.Count > 0)
                {
                    _dupParagraph.Pop();
                    if (_dupParagraph.Count > 0)
                    {
                        pictureCaption = _dupParagraph.Peek();
                        int countCharacter = 0;
                        countCharacter = pictureCaption.Length;
                        pictureCaption = pictureCaption.Substring(0, countCharacter - 2);
                    }
                }

                if (pictureCaption.ToLower() == "picturecaption")
                {
                    _writer.WriteAttributeString("AppliedParagraphStyle", "ParagraphStyle/" + "headword");
                    _writer.WriteAttributeString("FontStyle", "Regular");
                }
                else
                {
                    _writer.WriteAttributeString("AppliedParagraphStyle", "ParagraphStyle/" + cc[0]); 
                }

                _imageInsert = false;
                _imageSource = string.Empty;
                _isNewParagraph = false;
            }
            return inserted;
        }

        private static int GCD(int width, int height)
        {
            try
            {
                while (width != 0 && height != 0)
                {
                    if (width > height)
                        width %= height;
                    else
                        height %= width;
                }
                if (width == 0)
                    return height;
                else
                    return width;
            }
            catch
            {
                return 0;
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
                string[] splitedClassName = _allParagraph.ToArray();

                if (splitedClassName.Length > 0)
                {
                    bool result = false;
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
                                result = true;
                                break;
                            case "right":
                                AnchorPoint = "TopRightAnchor";
                                HoriAlignment = "RightAlign";
                                VertAlignment = "CenterAlign";
                                VertRefPoint = "LineBaseline";
                                result = true;
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
                                result = true;
                                break;
                            case "center":
                                AnchorPoint = "TopCenterAnchor";
                                HoriAlignment = "CenterAlign";
                                VertAlignment = "CenterAlign";
                                VertRefPoint = "LineBaseline";
                                result = true;
                                break;
                            case "top-right":
                                AnchorPoint = "TopRightAnchor";
                                VertAlignment = "TopAlign";
                                HoriAlignment = "RightAlign";
                                VertRefPoint = "PageMargins";
                                wrapMode = "JumpObjectTextWrap";
                                result = true;
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
                                result = true;
                                break;
                            case "bottom-right":
                                AnchorPoint = "BotomRightAnchor";
                                VertAlignment = "BottomAlign";
                                HoriAlignment = "RightAlign";
                                VertRefPoint = "PageMargins";
                                wrapMode = "JumpObjectTextWrap";
                                result = true;
                                break;
                        }
                        wrapSide = GetPropertyValue(clsName, "clear", wrapSide);
                        //if (pos != "left" && wrapSide != "none")
                        //{
                        //    break;
                        //}
                        //return;
                        if (result)
                        {
                            break;
                        }
                    }
                }
            }
        }

        private void StartElement()
        {
            SetHeadwordTrue();
            StartElementBase(_IsHeadword);
            SetClassCounter();
            Psuedo();
            DropCaps();
            SetHomographNumber(true);
            FooterSetup(Common.OutputType.IDML.ToString());

            if (IdAllClass.ContainsKey(_classNameWithLang))
            {
                bool isPageBreak = false;
                bool isColumnCount = false;
                if (IdAllClass[_classNameWithLang].ContainsKey("PageBreakBefore"))
                {
                    isPageBreak = IdAllClass[_classNameWithLang]["PageBreakBefore"] == "always";
                }
                else if (IdAllClass[_classNameWithLang].ContainsKey("PageBreakAfter"))
                {
                    isPageBreak = IdAllClass[_classNameWithLang]["PageBreakAfter"] == "always";
                }
                if (IdAllClass[_classNameWithLang].ContainsKey("TextColumnCount"))
                {
                    isColumnCount = int.Parse(IdAllClass[_classNameWithLang]["TextColumnCount"]) > 1;
                }
                if (isPageBreak || isColumnCount || _classNameWithLang.ToLower() == "cover" || _classNameWithLang.ToLower() == "title" || _classNameWithLang.ToLower() == "copyright")
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
            else if (_classNameWithLang.ToLower() == "cover" || _classNameWithLang.ToLower() == "title" || _classNameWithLang.ToLower() == "copyright")
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

        private void SetHeadwordTrue()
        {
            if (_reader.GetAttribute("class") != null && (_reader.GetAttribute("class").ToLower() == "headword" || _reader.GetAttribute("class").ToLower() == "reversal-form"))
            {
                _IsHeadword = true;
                _headwordStyles = true;
            }
        }



        private void SetHomographNumber(bool defValue)
        {
            if (_classNameWithLang.IndexOf("homographnumber") >= 0)
            {
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

            Dictionary<string, string> mystyle = new Dictionary<string, string>();
            if (IdAllClass.ContainsKey(classNameWOLang) && IdAllClass[classNameWOLang].ContainsKey("float") && IdAllClass[classNameWOLang].ContainsKey("BaselineShift"))
            {
                _isIdDropCap = true;
                string lines = "2";
                _allStyle.Pop();
                CollectFootNoteChapterVerse(_chapterNo, Common.OutputType.IDML.ToString());

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
                if (IdAllClass[classNameWOLang].ContainsKey("PointSize"))
                {
                    if (IdAllClass.ContainsKey("Paragraph") && IdAllClass["Paragraph"].ContainsKey("PointSize"))
                    {
                        mystyle["BaselineShift"] = IdAllClass["Paragraph"]["PointSize"];
                    }
                    else
                    {
                        mystyle["BaselineShift"] = IdAllClass[classNameWOLang]["PointSize"];
                    }
                }
                _paragraphName = classNameWOLang + _chapterNo.Length.ToString();
                _newProperty[_paragraphName] = mystyle;
                Write();
            }
            else if (classNameWOLang == "ChapterNumber")
            {
                CollectFootNoteChapterVerse(_chapterNo, Common.OutputType.IDML.ToString());
                _paragraphName = classNameWOLang + _chapterNo.Length.ToString();
                _newProperty[_paragraphName] = mystyle;
                _isIdDropCap = true;
                Write();
            }
        }

        private void EndElement()
        {
            _characterName = null;
            _closeChildName = StackPop(_allStyle);
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

            EndElementBase(false);
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

        private void SetHeadwordFalse()
        {
            if (_closeChildName.ToLower() == "headword" || _closeChildName.ToLower() == "reversalform")
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
            if (_imageInsert && !_IdImageInserted)
            {
                if (_closeChildName == _imageClass) // Without Caption
                {
                    _allCharacter.Push(_imageClass); // temporarily storing to get width and position
                    _IdImageInserted = InsertImage();
                    _writer.WriteEndElement(); // for ParagraphStyle
                    _writer.WriteEndElement(); // for Textframe
                    _writer.WriteEndElement(); // for rectangle
                    _allCharacter.Pop();    // retrieving it again.
                    imageClass = "";
                }
            }
            else // With Caption
            {
                if (imageClass.Length > 0 && _closeChildName == imageClass)
                {

                    _writer.WriteEndElement();// for ParagraphStyle
                    _writer.WriteEndElement(); // for Textframe
                    _writer.WriteEndElement(); // for rectangle

                    imageClass = "";
                }
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
        private void AddIdUsedStyleName(string styleName)
        {
            if (!_IdUsedStyleName.Contains(styleName))
                _IdUsedStyleName.Add(styleName);
        }

        #region Private Methods



        private void InitializeData(string projectPath, Dictionary<string, Dictionary<string, string>> idAllClass, Dictionary<string, ArrayList> classFamily, ArrayList cssClassOrder)
        {
            _outputType = Common.OutputType.IDML;
            _allStyle = new Stack<string>();
            _allParagraph = new Stack<string>();
            _allCharacter = new Stack<string>();
            _doNotInheritProperty = new Stack<string>();

            _childStyle = new Dictionary<string, string>();
            IdAllClass = new Dictionary<string, Dictionary<string, string>>();
            _newProperty = new Dictionary<string, Dictionary<string, string>>();
            ParentClass = new Dictionary<string, string>();
            _cssClassOrder = cssClassOrder;

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

        private void UpdateRelativeInStylesXML()
        {
            ModifyIDStyles modifyIDStyles = new ModifyIDStyles();
            _textVariables = modifyIDStyles.ModifyStylesXML(_projectPath, _newProperty, _IdUsedStyleName, _languageStyleName, "", _IsHeadword);
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
