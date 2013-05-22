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

        private bool IsEmptyElement = false;
        private string _allDiv = string.Empty;
        //bool _isWhiteSpace;
        private bool _isNewLine = true;
        private readonly string _tempFile = Common.PathCombine(Path.GetTempPath(), "tempXHTMLFile.xhtml"); //TD-351

        private XmlDocument _xmldoc;
        private ArrayList _psuedoBefore = new ArrayList();
        private Dictionary<string, ClassInfo> _psuedoAfter = new Dictionary<string, ClassInfo>();
        
        private bool _IsHeadword = false;
        private bool _significant;
        private bool _anchorWrite;
        
        //private Stack<string> _referenceCloseStyleStack = new Stack<string>();
        private bool _isPictureDisplayNone = false;

        private bool _imageParaForCaption = false;
        private bool isFileEmpty = true;
        string _outputExtension = string.Empty;
        private string _sourcePicturePath;

        public ArrayList _psuedoClassName = new ArrayList();

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
                if (_reader.IsEmptyElement)
                {
                    IsEmptyElement = true;
                }
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
                    if (psuedoBefore.Content != null && !_psuedoClassName.Contains(psuedoBefore.StyleName))
                    {
                        _psuedoClassName.Add(psuedoBefore.StyleName);
                    }
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
            if (!IsEmptyElement)
            {
                StartElementBase(_IsHeadword);
                Psuedo();
            }
            IsEmptyElement = false;
            //StartElementBase(_IsHeadword);
            //Psuedo();
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
                        if (classInfo.Content != null && !_psuedoClassName.Contains(classInfo.StyleName))
                        {
                            _psuedoClassName.Add(classInfo.StyleName);
                        }
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
