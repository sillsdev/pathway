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

using System.Collections.Generic;
using System.Xml;
using System.IO;
using SIL.Tool;

#endregion Using

namespace SIL.PublishingSolution
{
    public class OdtContent : XHTMLProcess 
    {
        #region Private Variable
        private string _projectType;
        private string _fileType;
        #endregion


        public void CreateContent(string projectPath, string xhtmlFileWithPath, Dictionary<string, Dictionary<string, string>> cssProperty, string fileType)
        {
            _fileType = fileType;
            _projectType = Common.GetProjectType(xhtmlFileWithPath);
            InitializeData(projectPath, cssProperty);
            CreateFile(projectPath);
            OpenXhtmlFile(xhtmlFileWithPath);
            ProcessXHTML(xhtmlFileWithPath);
            UpdateRelativeInStylesXML();
            CloseFile();
        }

        private void ProcessXHTML(string xhtmlFileWithPath)
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
                            StartElementBase();
                            break;
                        case XmlNodeType.Text:
                            WriteText();
                            break;
                        case XmlNodeType.EndElement:
                            EndElement();
                            break;
                    }
                }
            }
            catch (XmlException e)
            {
                var msg = new[] { e.Message, xhtmlFileWithPath };
            }
        }

        private void EndElement()
        {
            string styleName = StackPop(_allStyle);
            EndElementBase();
        }

        private void WriteText()
        {
            //push used tags
            if (_isNewParagraph)
            {
                if (_paragraphName == null) 
                {
                    _paragraphName = StackPeek(_allParagraph); // _allParagraph.Pop();
                }

                if (_allParagraph.Count > 0 && !_isParagraphClosed) // Is Para Exist
                {
                    _writer.WriteEndElement();
                }

                _writer.WriteStartElement("ParagraphStyleRange");
                _writer.WriteAttributeString("AppliedParagraphStyle", "ParagraphStyle/" + _paragraphName);
                _paragraphName = null;
                _isNewParagraph = false;
                _isParagraphClosed = false;
            }
            _writer.WriteStartElement("CharacterStyleRange");
            if (_characterName == null)
            {
                _characterName = StackPeekCharStyle(_allCharacter); // _allParagraph.Pop();
            }
            _writer.WriteAttributeString("AppliedCharacterStyle", "CharacterStyle/" + _characterName);
            _characterName = null;
            _writer.WriteStartElement("Content");
            string text = _reader.Value;
            _writer.WriteString(text);
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        #region Private Methods

        private string StackPeekCharStyle(Stack<string> stack)
        {
            string result = "[No character style]";
            if (stack.Count > 0)
            {
                result = stack.Peek();
            }
            return result;
        }

        private void InitializeData(string projectPath, Dictionary<string, Dictionary<string, string>> cssProperty)
        {
            _allStyle = new Stack<string>();
            _allParagraph = new Stack<string>();
            _allCharacter = new Stack<string>();

            _childStyle = new Dictionary<string, string>();
            IdAllClass = new Dictionary<string, Dictionary<string, string>>();
            _newProperty = new Dictionary<string, Dictionary<string, string>>();

            IdAllClass = cssProperty;
            _projectPath = projectPath;

            _projectPath = projectPath;
            _isNewParagraph = false;
            //_characterName = "[No character style]";
        }

        private void CreateFile(string projectPath)
        {
            string targetContentXML = projectPath + "content.xml";
            _writer = new XmlTextWriter(targetContentXML, null) {Formatting = Formatting.Indented};
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
            _writer.WriteAttributeString("xmlns:rdfa", "http://docs.oasis-open.org/opendocument/meta/rdfa#");
            _writer.WriteAttributeString("office:version", "1.2");
            _writer.WriteStartElement("office:scripts");
            //if (_structStyles.IsMacroEnable)
            {
                _writer.WriteStartElement("office:event-listeners");
                _writer.WriteStartElement("script:event-listener");
                _writer.WriteAttributeString("script:language", "ooo:script");
                _writer.WriteAttributeString("script:event-name", "dom:load");
                _writer.WriteAttributeString("xlink:href",
                                             "vnd.sun.star.script:Standard.Module1.StartDontForget?language=Basic&location=document");
                _writer.WriteEndElement();
                _writer.WriteStartElement("script:event-listener");
                _writer.WriteAttributeString("script:language", "ooo:script");
                _writer.WriteAttributeString("script:event-name", "office:print");
                if (string.Compare(_projectType, "Scripture") == 0)
                {
                    _writer.WriteAttributeString("xlink:href",
                                                 "vnd.sun.star.script:Standard.Module1.IsReferenceCorrected?language=Basic&location=document");
                }
                    // todo handle the following line
                    //else if (string.Compare(StylesXML.projectType, "Dictionary") == 0)
                else
                {
                    _writer.WriteAttributeString("xlink:href",
                                                 "vnd.sun.star.script:Standard.Module1.IsGuidewordsCorrected?language=Basic&location=document");
                }
                _writer.WriteEndElement();
                _writer.WriteEndElement();
            }
            _writer.WriteEndElement();

            //office:font-face-decls Attributes.
            _writer.WriteStartElement("office:font-face-decls");

            _writer.WriteStartElement("style:font-face");
            _writer.WriteAttributeString("style:name", "Tahoma1");
            _writer.WriteAttributeString("svg:font-family", "Tahoma");
            _writer.WriteEndElement();
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
            _writer.WriteAttributeString("svg:font-family", "Arial");
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
            _writer.WriteAttributeString("svg:font-family", "Tahoma");
            _writer.WriteAttributeString("style:font-family-generic", "system");
            _writer.WriteAttributeString("style:font-pitch", "variable");
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


        private void UpdateRelativeInStylesXML()
        {
            ModifyOdtStyles modifyIDStyles = new ModifyOdtStyles();
            modifyIDStyles.ModifyStylesXML(_projectPath, _newProperty);
        }

        private void CloseFile()
        {
            _writer.WriteEndElement();
            _writer.WriteEndDocument();
            _writer.Flush();
            _writer.Close();
        }
        #endregion
    }
}
