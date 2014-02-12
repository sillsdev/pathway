// --------------------------------------------------------------------------------------------
// <copyright file="InMetaData.cs" from='2009' to='2009' company='SIL International'>
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
// Export process used to Export the ODT and Prince PDF output
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using SIL.Tool;
using SIL.Tool.Localization;

namespace SIL.PublishingSolution
{
    public class InMetaData
    {
        private XmlDocument _contentXMLdoc;
        public void SetDateTimeinMetaDataXML(string contentFilePath)
        {
            string getDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");
            string metaDataXMLPath = Common.PathCombine(contentFilePath, "META-INF");
            metaDataXMLPath = Common.PathCombine(metaDataXMLPath, "metadata.xml");

            string _xPath;
            _contentXMLdoc = new XmlDocument();
            _contentXMLdoc.Load(metaDataXMLPath);
            XmlElement _root = _contentXMLdoc.DocumentElement;
            var nsmgr = new XmlNamespaceManager(_contentXMLdoc.NameTable);
            nsmgr.AddNamespace("xmp", "http://ns.adobe.com/xap/1.0/");
            nsmgr.AddNamespace("xmpGImg", "http://ns.adobe.com/xap/1.0/g/img/");
            
            _xPath = "//xmp:CreateDate";
            XmlNode node = _root.SelectSingleNode(_xPath, nsmgr);
            if (node != null)
            {
                node.InnerText = getDate;
            }

            _xPath = "//xmp:MetadataDate";
            node = _root.SelectSingleNode(_xPath, nsmgr);
            if (node != null)
            {
                node.InnerText = getDate;
            }

            _xPath = "//xmp:ModifyDate";
            node = _root.SelectSingleNode(_xPath, nsmgr);
            if (node != null)
            {
                node.InnerText = getDate;
            }
            var nsmgr2 = new XmlNamespaceManager(_contentXMLdoc.NameTable);

            nsmgr2.AddNamespace("xmpMM", "http://ns.adobe.com/xap/1.0/g/img/");
            nsmgr2.AddNamespace("stEvt", "http://ns.adobe.com/xap/1.0/sType/ResourceEvent#");

            XmlNodeList nodeList = _root.SelectNodes("//stEvt:when", nsmgr2);

            foreach (XmlNode nodeX in nodeList)
            {
                nodeX.InnerText = getDate;
            }
            _contentXMLdoc.Save(metaDataXMLPath);
        }

    }
}
