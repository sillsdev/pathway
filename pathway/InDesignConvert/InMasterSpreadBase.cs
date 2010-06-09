// --------------------------------------------------------------------------------------------
// <copyright file="InMasterPage.cs" from='2009' to='2010' company='SIL International'>
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
// Creates the InMasterPage file 
// </remarks>
// --------------------------------------------------------------------------------------------

using System.IO;
using System.Xml;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class InMasterSpreadBase
    {
        #region Private Variables
        public XmlTextWriter _writer;
        #endregion

        public void CreateFile(string projectPath, string masterpagename)
        {
            string styleXMLWithPath = Common.PathCombine(projectPath, masterpagename + ".xml");
            _writer = new XmlTextWriter(styleXMLWithPath, null) { Formatting = Formatting.Indented };
            _writer.WriteStartDocument();
            _writer.WriteStartElement("idPkg:MasterSpread");
            _writer.WriteAttributeString("xmlns:idPkg", "http://ns.adobe.com/AdobeInDesign/idml/1.0/packaging");
            _writer.WriteAttributeString("DOMVersion", "6.0");
        }

        public void CloseFile()
        {
            _writer.WriteEndElement();
            _writer.WriteEndDocument();
            _writer.Flush();
            _writer.Close();
        }

        public void CreateMainFrameStaticMethod2()
        {
            _writer.WriteStartElement("TextWrapPreference");
            _writer.WriteAttributeString("Inverse", "false");
            _writer.WriteAttributeString("ApplyToMasterPageOnly", "false");
            _writer.WriteAttributeString("TextWrapSide", "BothSides");
            _writer.WriteAttributeString("TextWrapMode", "None");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("TextWrapOffset");
            _writer.WriteAttributeString("Top", "36");
            _writer.WriteAttributeString("Left", "72");
            _writer.WriteAttributeString("Bottom", "36");
            _writer.WriteAttributeString("Right", "72");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }
    }
}
