// --------------------------------------------------------------------------------------------
// <copyright file="InSpreadBase.cs" from='2009' to='2014' company='SIL International'>
//      Copyright © 2014, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Creates the InDesign Spread base file 
// </remarks>
// --------------------------------------------------------------------------------------------

using System.Xml;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class InSpreadBase
    {
        #region public Variables
        public XmlTextWriter _writer;
        #endregion

        public void CreateFlattenerPreference()
        {
            _writer.WriteStartElement("FlattenerPreference");
            _writer.WriteAttributeString("LineArtAndTextResolution", "300");
            _writer.WriteAttributeString("GradientAndMeshResolution", "150");
            _writer.WriteAttributeString("ClipComplexRegions", "false");
            _writer.WriteAttributeString("ConvertAllStrokesToOutlines", "false");
            _writer.WriteAttributeString("ConvertAllTextToOutlines", "false");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("RasterVectorBalance");
            _writer.WriteAttributeString("type", "double");
            _writer.WriteString("50");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        public void CloseFile()
        {
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndDocument();
            _writer.Flush();
            _writer.Close();
        }

        public void CreateaFile(string projectPath, int spread)
        {
            string spreadXMLWithPath = Common.PathCombine(projectPath, "Spread_" + spread + ".xml");
            _writer = new XmlTextWriter(spreadXMLWithPath, null) { Formatting = Formatting.Indented };
            _writer.WriteStartDocument();
            _writer.WriteStartElement("idPkg:Spread");
            _writer.WriteAttributeString("xmlns:idPkg", "http://ns.adobe.com/AdobeInDesign/idml/1.0/packaging");
            _writer.WriteAttributeString("DOMVersion", "6.0");
        }

    }
}
