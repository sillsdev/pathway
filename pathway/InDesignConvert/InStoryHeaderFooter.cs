// --------------------------------------------------------------------------------------------
// <copyright file="InStoryHeaderFooter.cs" from='2009' to='2009' company='SIL International'>
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
// Creates the Story file for Header and Footer
// </remarks>
// --------------------------------------------------------------------------------------------

#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Xml;
using System.Drawing;
using System.IO;
using SIL.Tool;

#endregion Using

namespace SIL.PublishingSolution
{
    public class InStoryHeaderFooter
    {
        public XmlTextWriter _writer;
        private string _contentType;
        private enum ReferencePosition
        {
            first,last
        }

        public string CreateStoryforHeaderFooter(string projectPath, string storyName, string contentType, string className, ArrayList headwordStyle)
        {
            string fileName = "Story_" + storyName + ".xml";
            _contentType = contentType;
            string textAlign = GetAligment(storyName);
            string storyXMLWithPath = Common.PathCombine(projectPath, fileName);
            _writer = new XmlTextWriter(storyXMLWithPath, null)
                          {
                              Formatting = Formatting.Indented
                          };
            _writer.WriteStartDocument();
            _writer.WriteStartElement("idPkg:Story");
            _writer.WriteAttributeString("xmlns:idPkg", "http://ns.adobe.com/AdobeInDesign/idml/1.0/packaging");
            _writer.WriteAttributeString("DOMVersion", "6.0");
            _writer.WriteStartElement("Story");
            _writer.WriteAttributeString("Self", storyName);
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
            _writer.WriteStartElement("ParagraphStyleRange");
            // Note: Paragraph Start Element
            _writer.WriteAttributeString("AppliedParagraphStyle", "ParagraphStyle/" + className);
            _writer.WriteAttributeString("Justification", textAlign);

            string stylename = string.Empty;

            stylename = (projectPath.ToLower().IndexOf("dictionary") > 0) ? "xhomographnumber_headword_entry_letData_dicBody" : "scrBookName_1";

            if (_contentType == "PageCount")
            {
                WritePageCount(className);
            }
            else if (_contentType == "GF" || _contentType == "GL")
            {
                //WriteGuidewordVariable(className);
                WriteGuidewordVariable(headwordStyle, className);
            }
            else if (_contentType == "BL CL:VL" || _contentType == "BF CF:VF")
            {
                //string stylename = "xhomographnumber_headword_entry_letData_dicBody";
                
                string position = _contentType.IndexOf("F") > 0 ? ReferencePosition.first.ToString() : ReferencePosition.last.ToString();
                WriteBooknameVariable("CharacterStyle/" + stylename, position);
                WriteSpecialCharacter(" ");
                WriteChapterVariable("CharacterStyle/" + stylename, position);
                WriteSpecialCharacter(":");
                WriteVerseVariable("CharacterStyle/" + stylename, position);
            }
            else if (_contentType == "BF CF" || _contentType == "BL CL")
            {
                //string stylename = "xhomographnumber_headword_entry_letData_dicBody";
                string position = _contentType.IndexOf("F") > 0 ? "first" : "last";
                WriteBooknameVariable("CharacterStyle/" + stylename, position);
                WriteSpecialCharacter(" ");
                WriteChapterVariable("CharacterStyle/" + stylename, position);
            }
            else if (_contentType == "BF CF:VF-CL:VL")
            {
                //string stylename = "xhomographnumber_headword_entry_letData_dicBody";
                WriteBooknameVariable("CharacterStyle/" + stylename, ReferencePosition.first.ToString());
                WriteSpecialCharacter(" ");
                WriteChapterVariable("CharacterStyle/" + stylename, ReferencePosition.first.ToString());
                WriteSpecialCharacter(":");
                WriteVerseVariable("CharacterStyle/" + stylename, ReferencePosition.first.ToString());
                WriteSpecialCharacter(" - ");
                WriteChapterVariable("CharacterStyle/" + stylename, ReferencePosition.last.ToString());
                WriteSpecialCharacter(":");
                WriteVerseVariable("CharacterStyle/" + stylename, ReferencePosition.last.ToString());
            }
            else if (_contentType == "BF CF-CL")
            {
                //string stylename = "xhomographnumber_headword_entry_letData_dicBody";
                WriteBooknameVariable("CharacterStyle/" + stylename, ReferencePosition.first.ToString());
                WriteSpecialCharacter(" ");
                WriteChapterVariable("CharacterStyle/" + stylename, ReferencePosition.first.ToString());
                WriteSpecialCharacter(" - ");
                WriteChapterVariable("CharacterStyle/" + stylename, ReferencePosition.last.ToString());
            }
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.Flush();
            _writer.Close();
            return fileName;
        }

        private void WriteVerseVariable(string styleName, string position)
        {
            // Note: Verse Character Start Element
            _writer.WriteStartElement("CharacterStyleRange");
            _writer.WriteAttributeString("AppliedCharacterStyle", styleName);
            _writer.WriteAttributeString("PageNumberType", "TextVariable");
            if (position == ReferencePosition.first.ToString())
            {
                _writer.WriteStartElement("TextVariableInstance");
                _writer.WriteAttributeString("Self", "u786");
                _writer.WriteAttributeString("Name", "FirstVerseNumber");
                _writer.WriteAttributeString("ResultText", "&lt;VF&gt;");
                _writer.WriteAttributeString("AssociatedTextVariable", "dTextVariablenFirstVerseNumber");
                _writer.WriteEndElement();
            }
            else
            {
                _writer.WriteStartElement("TextVariableInstance");
                _writer.WriteAttributeString("Self", "u786");
                _writer.WriteAttributeString("Name", "LastVerseNumber");
                _writer.WriteAttributeString("ResultText", "&lt;VL&gt;");
                _writer.WriteAttributeString("AssociatedTextVariable", "dTextVariablenLastVerseNumber");
                _writer.WriteEndElement();
            }
            _writer.WriteEndElement();
        }

        private void WriteSpecialCharacter(string insertChar)
        {
            _writer.WriteStartElement("CharacterStyleRange");
            // Note: Character Start Element
            _writer.WriteAttributeString("AppliedCharacterStyle", "CharacterStyle/$ID/[No character style]");
            if (_contentType.IndexOf(insertChar.Trim()) > 0 || insertChar.Length == 1)
            {
                _writer.WriteStartElement("Content");
                _writer.WriteString(insertChar);
                _writer.WriteEndElement();
            }
            _writer.WriteEndElement();
        }

        private void WriteBooknameVariable(string styleName, string position)
        {
            _writer.WriteStartElement("CharacterStyleRange");
            _writer.WriteAttributeString("AppliedCharacterStyle", styleName);
            _writer.WriteAttributeString("PageNumberType", "TextVariable");
            if (position == ReferencePosition.first.ToString())
            {
                _writer.WriteStartElement("TextVariableInstance");
                _writer.WriteAttributeString("Self", "u678");
                _writer.WriteAttributeString("Name", "FirstBookName");
                _writer.WriteAttributeString("ResultText", "&lt;BF&gt;");
                _writer.WriteAttributeString("AssociatedTextVariable", "dTextVariablenFirstBookName");
                _writer.WriteEndElement();
            }
            else
            {
                _writer.WriteStartElement("TextVariableInstance");
                _writer.WriteAttributeString("Self", "u678");
                _writer.WriteAttributeString("Name", "LastBookName");
                _writer.WriteAttributeString("ResultText", "&lt;BL&gt;");
                _writer.WriteAttributeString("AssociatedTextVariable", "dTextVariablenLastBookName");
                _writer.WriteEndElement();
            }
            _writer.WriteEndElement();
        }


        private void WritePageCount(string className)
        {
            _writer.WriteStartElement("CharacterStyleRange");
            _writer.WriteAttributeString("AppliedCharacterStyle", "CharacterStyle/headword");// + className
            if (_contentType == "PageCount")
            {
                _writer.WriteStartElement("Content");
                _writer.WriteRaw("<?ACE 18?>");
                _writer.WriteEndElement();
            }
            _writer.WriteEndElement();
        }


        //private void WriteGuidewordVariable(string styleName)
        private void WriteGuidewordVariable(ArrayList GuideWordStyle, string PageStyleName)
        {
            int i = 1;
            bool isHomographExist = FindHomographNumber(GuideWordStyle);
            foreach (string sName in GuideWordStyle)
            {
                //if (GuideWordStyle.Count > 2 && (sName.IndexOf("headword") >= 0 || sName.ToLower() == "reversal-form")) continue;
                if (sName.IndexOf("Guideword") == 0)
                {
                    string styleName = Common.RightString(sName, "_");
                    styleName = "CharacterStyle/" + styleName;
                    // Note: GuideWord Character Start Element
                    _writer.WriteStartElement("CharacterStyleRange");
                    _writer.WriteAttributeString("AppliedCharacterStyle", styleName);
                    //_writer.WriteAttributeString("AppliedCharacterStyle", PageStyleName);
                    if (_contentType == "GF")
                    {
                        _writer.WriteAttributeString("PageNumberType", "TextVariable");
                        _writer.WriteStartElement("TextVariableInstance");
                        _writer.WriteAttributeString("Self", "uf1");
                        _writer.WriteAttributeString("Name", "FG_" + i);
                        _writer.WriteAttributeString("ResultText", "&lt;First&gt;");
                        _writer.WriteAttributeString("AssociatedTextVariable", "dTextVariablenFirst" + i);
                        _writer.WriteEndElement();
                    }
                    if (_contentType == "GL")
                    {
                        _writer.WriteAttributeString("PageNumberType", "TextVariable");
                        _writer.WriteStartElement("TextVariableInstance");
                        _writer.WriteAttributeString("Self", "ul1");
                        _writer.WriteAttributeString("Name", "LG_" + i);
                        _writer.WriteAttributeString("ResultText", "&lt;Last&gt;");
                        _writer.WriteAttributeString("AssociatedTextVariable", "dTextVariablenLast" + i);
                        _writer.WriteEndElement();
                    }
                    _writer.WriteEndElement();
                    // Note: GuideWord Character End Element
                    // Note: HomoGraphNumber Character Start Element
                    if (isHomographExist)
                    {
                        _writer.WriteStartElement("CharacterStyleRange");
                        _writer.WriteAttributeString("AppliedCharacterStyle",
                                                     "CharacterStyle/xhomographnumber_headword_entry_letData_dicBody");
                        //_writer.WriteAttributeString("AppliedCharacterStyle",
                        //                            homoStyleName);
                        if (_contentType == "GF")
                        {
                            _writer.WriteAttributeString("PageNumberType", "TextVariable");
                            _writer.WriteStartElement("TextVariableInstance");
                            _writer.WriteAttributeString("Self", "u678");
                            _writer.WriteAttributeString("Name", "HGF");
                            _writer.WriteAttributeString("ResultText", "&lt;HomoGraph&gt;");
                            _writer.WriteAttributeString("AssociatedTextVariable", "dTextVariablenHomoGraphF");
                            _writer.WriteEndElement();
                        }
                        if (_contentType == "GL")
                        {
                            _writer.WriteAttributeString("PageNumberType", "TextVariable");
                            _writer.WriteStartElement("TextVariableInstance");
                            _writer.WriteAttributeString("Self", "u678");
                            _writer.WriteAttributeString("Name", "HGL");
                            _writer.WriteAttributeString("ResultText", "&lt;HomoGraph&gt;");
                            _writer.WriteAttributeString("AssociatedTextVariable", "dTextVariablenHomoGraphL");
                            _writer.WriteEndElement();
                        }
                        _writer.WriteEndElement();
                    }
                    // Note: HomoGraphNumber Character End Element
                }
                else if (sName.IndexOf("RevGuideword") == 0)
                //else
                {
                    string styleName = Common.RightString(sName, "_");
                    styleName = "CharacterStyle/" + styleName;
                    // Note: GuideWord Character Start Element
                    _writer.WriteStartElement("CharacterStyleRange");
                    _writer.WriteAttributeString("AppliedCharacterStyle", styleName);
                    //_writer.WriteAttributeString("AppliedCharacterStyle", PageStyleName);
                    if (_contentType == "GF")
                    {
                        _writer.WriteAttributeString("PageNumberType", "TextVariable");
                        _writer.WriteStartElement("TextVariableInstance");
                        _writer.WriteAttributeString("Self", "uf1");
                        _writer.WriteAttributeString("Name", "RFG_" + i);
                        _writer.WriteAttributeString("ResultText", "&lt;First&gt;");
                        _writer.WriteAttributeString("AssociatedTextVariable", "dTextVariablenFirst" + i);
                        _writer.WriteEndElement();
                    }
                    if (_contentType == "GL")
                    {
                        _writer.WriteAttributeString("PageNumberType", "TextVariable");
                        _writer.WriteStartElement("TextVariableInstance");
                        _writer.WriteAttributeString("Self", "ul1");
                        _writer.WriteAttributeString("Name", "RLG_" + i);
                        _writer.WriteAttributeString("ResultText", "&lt;Last&gt;");
                        _writer.WriteAttributeString("AssociatedTextVariable", "dTextVariablenLast" + i);
                        _writer.WriteEndElement();
                    }
                    _writer.WriteEndElement();
                }
                i = i + 1;
            }
        }

        private static bool FindHomographNumber(ArrayList styles)
        {
            bool result = false;
            foreach (string styleName in styles)
            {
                if(styleName.IndexOf("HomoGraphNumber_") == 0)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private void WriteChapterVariable(string styleName, string position)
        {
            // Note: Chapter Character Start Element
            _writer.WriteStartElement("CharacterStyleRange");
            _writer.WriteAttributeString("AppliedCharacterStyle", styleName);
            _writer.WriteAttributeString("PageNumberType", "TextVariable");
            if (position == ReferencePosition.first.ToString())
            {
                _writer.WriteStartElement("TextVariableInstance");
                _writer.WriteAttributeString("Self", "u344");
                _writer.WriteAttributeString("Name", "FirstChapterNumber");
                _writer.WriteAttributeString("ResultText", "&lt;CF&gt;");
                _writer.WriteAttributeString("AssociatedTextVariable", "dTextVariablenFirstChapterNumber");
                _writer.WriteEndElement();
            }
            else
            {
                _writer.WriteStartElement("TextVariableInstance");
                _writer.WriteAttributeString("Self", "u344");
                _writer.WriteAttributeString("Name", "LastChapterNumber");
                _writer.WriteAttributeString("ResultText", "&lt;CL&gt;");
                _writer.WriteAttributeString("AssociatedTextVariable", "dTextVariablenLastChapterNumber");
                _writer.WriteEndElement();
            }
            _writer.WriteEndElement();
        }

        private string GetAligment(string storyName)
        {
            string align = "LeftAlign";
            string pos = storyName.Substring(1, 2);
            if (pos == "tl" || pos == "bl")
            {
                align = "LeftAlign";
            }
            if (pos == "tr" || pos == "br")
            {
                align = "RightAlign";
            }
            if (pos == "tc" || pos == "bc")
            {
                align = "CenterAlign";
            }
            return align;
        }
    }
}
