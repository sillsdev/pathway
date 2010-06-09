// --------------------------------------------------------------------------------------------
// <copyright file="InDesignMap.cs" from='2009' to='2010' company='SIL International'>
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
// Creates the InDesign DesignMap file 
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.IO;
using System.Xml;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class InDesignMap : InDesignMapBase
    {

        #region Private Variables
        private int _noOfTextFrames;
        private ArrayList _masterPageList;
        private ArrayList _crossRef = new ArrayList();
        #endregion

        public bool CreateIDDesignMap(string projectPath, int noOfTextFrames, ArrayList masterPageList, ArrayList textVariable, ArrayList crossRef)
        {
            try
            {
                _crossRef = crossRef;
                _noOfTextFrames = noOfTextFrames;
                _masterPageList = masterPageList;
                StartIDDesignMap(projectPath);
                StaticMethod1(projectPath, textVariable);
                CreateSpread();
                CreateSection();
                createCrossReferenceFormat(); // Static code
                CreateStory();
                CrossRef();
                StaticMethod2();
                return true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return false;
        }

        private void CrossRef()
        {
 
            int i = 1;
            foreach (string crossref in _crossRef)
            {
                _writer.WriteStartElement("Hyperlink");
                _writer.WriteAttributeString("Self", crossref);
                _writer.WriteAttributeString("Name", crossref);
                    _writer.WriteAttributeString("Source", "Hyperlink_" + crossref);
                    _writer.WriteAttributeString("Visible", "true");
                    _writer.WriteAttributeString("Highlight", "Outline");
                    _writer.WriteAttributeString("Width", "Thin");
                    _writer.WriteAttributeString("BorderStyle", "Dashed");
                    _writer.WriteAttributeString("Hidden", "false");
                    _writer.WriteAttributeString("DestinationUniqueKey", i.ToString());

                    _writer.WriteStartElement("Properties");
                        //Border
                        _writer.WriteStartElement("BorderColor");
                        _writer.WriteAttributeString("type", "enumeration");
                        _writer.WriteString("LightBlue");
                        _writer.WriteEndElement();
                        //Destination
                        _writer.WriteStartElement("Destination");
                        _writer.WriteAttributeString("type", "object");
                        _writer.WriteString("Name_" + crossref);
                        _writer.WriteEndElement();

                   _writer.WriteEndElement(); // Properties
                _writer.WriteEndElement();
                i++;
            }

        }

        private void CreateSpread()
        {
            if (_masterPageList.Count > 0)
            {
                for (int i = 0; i < _masterPageList.Count; i++)
                {
                    if (_masterPageList[i].ToString().IndexOf("MasterSpread") >= 0)
                    {
                        _writer.WriteStartElement("idPkg:MasterSpread");
                        _writer.WriteAttributeString("src", "MasterSpreads/" + _masterPageList[i] + ".xml");
                        _writer.WriteEndElement();
                    }
                }
            }
            else
            {
                _writer.WriteStartElement("idPkg:MasterSpread");
                _writer.WriteAttributeString("src", "MasterSpreads/MasterSpread_uc0.xml");
                _writer.WriteEndElement();
            }
            
            for (int i = 1; i <= 3; i++)
            {
                _writer.WriteStartElement("idPkg:Spread");
                _writer.WriteAttributeString("src", "Spreads/Spread_" + i + ".xml");
                _writer.WriteEndElement();
            }
        }

        private void CreateStory()
        {
            _writer.WriteStartElement("idPkg:BackingStory");
            _writer.WriteAttributeString("src", "XML/BackingStory.xml");
            _writer.WriteEndElement();
            if (_masterPageList.Count > 0)
            {
                for (int i = 0; i < _masterPageList.Count; i++)
                {
                    if (_masterPageList[i].ToString().IndexOf("Story") >= 0)
                    {
                        _writer.WriteStartElement("idPkg:Story");
                        _writer.WriteAttributeString("src", "Stories/" + _masterPageList[i]);
                        _writer.WriteEndElement();
                    }
                }
            }
            for (int i = 1; i <= _noOfTextFrames; i++)
            {
                _writer.WriteStartElement("idPkg:Story");
                _writer.WriteAttributeString("src", "Stories/Story_" + i + ".xml");
                _writer.WriteEndElement();
            }
        }

        private void CreateSection()
        {
            _writer.WriteStartElement("Section");
            _writer.WriteAttributeString("Self", "ubf");
            _writer.WriteAttributeString("Length", "3");
            _writer.WriteAttributeString("Name", "");
            _writer.WriteAttributeString("PageNumberStyle", "Arabic");
            _writer.WriteAttributeString("ContinueNumbering", "true");
            _writer.WriteAttributeString("IncludeSectionPrefix", "false");
            _writer.WriteAttributeString("Marker", "");
            _writer.WriteAttributeString("PageStart", "page1");
            _writer.WriteAttributeString("SectionPrefix", "");
            _writer.WriteEndElement();
            _writer.WriteStartElement("DocumentUser");
            _writer.WriteAttributeString("Self", "dDocumentUser0");
            _writer.WriteAttributeString("UserName", "$ID/Unknown User Name");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("UserColor");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("Gold");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        private void StartIDDesignMap(string projectPath)
        {
            string styleXMLWithPath = Common.PathCombine(projectPath, "designmap.xml");
            _writer = new XmlTextWriter(styleXMLWithPath, null) { Formatting = Formatting.Indented };
            _writer.WriteStartDocument();
            _writer.WriteRaw("<?aid style=\"50\" type=\"document\" readerVersion=\"6.0\" featureSet=\"257\" product=\"6.0(352)\" ?>");
            _writer.WriteStartElement("Document");
            _writer.WriteAttributeString("xmlns:idPkg", "http://ns.adobe.com/AdobeInDesign/idml/1.0/packaging");
            _writer.WriteAttributeString("DOMVersion", "6.0");
            _writer.WriteAttributeString("Self", "d");
            string storyList = string.Empty;
            for (int i = 1; i <= _noOfTextFrames; i++)
            {
                storyList = storyList + i + " ";
            }
            _writer.WriteAttributeString("StoryList", storyList);
            _writer.WriteAttributeString("ZeroPoint", "0 0");
            _writer.WriteAttributeString("ActiveLayer", "ub6");
            _writer.WriteAttributeString("CMYKProfile", "U.S. Web Coated (SWOP) v2");
            _writer.WriteAttributeString("RGBProfile", "sRGB IEC61966-2.1");
            _writer.WriteAttributeString("SolidColorIntent", "UseColorSettings");
            _writer.WriteAttributeString("AfterBlendingIntent", "UseColorSettings");
            _writer.WriteAttributeString("DefaultImageIntent", "UseColorSettings");
            _writer.WriteAttributeString("RGBPolicy", "PreserveEmbeddedProfiles");
            _writer.WriteAttributeString("CMYKPolicy", "CombinationOfPreserveAndSafeCmyk");
            _writer.WriteAttributeString("AccurateLABSpots", "false");
        }

    }
}
