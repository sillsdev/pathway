// --------------------------------------------------------------------------------------------
// <copyright file="ModifyOOContent.cs" from='2009' to='2014' company='SIL International'>
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
// Modify the OOContent file
// </remarks>
// --------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Xml;

namespace SIL.PublishingSolution
{
    class ModifyLOContent
    {
        private XmlDocument _contentXmLdoc;
        public void SetTableColumnCount(string contentFilePath, Dictionary<string, int> tableColumnModify)
        {
            if(tableColumnModify == null || tableColumnModify.Count == 0)
                return;

            _contentXmLdoc = new XmlDocument();
            _contentXmLdoc.PreserveWhitespace = true;
            _contentXmLdoc.Load(contentFilePath);
            XmlElement root = _contentXmLdoc.DocumentElement;
            var nsmgr = new XmlNamespaceManager(_contentXmLdoc.NameTable);
            nsmgr.AddNamespace("style", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
            nsmgr.AddNamespace("table", "urn:oasis:names:tc:opendocument:xmlns:table:1.0");
            foreach (KeyValuePair<string, int> kvp in tableColumnModify)
            {
                string xPath = "//table:table[@table:name=\"" + kvp.Key + "\"]/table:table-column";
                XmlNode node = root.SelectSingleNode(xPath, nsmgr);
                if (node == null) return;

                node.Attributes[1].Value = kvp.Value.ToString();
            }
            _contentXmLdoc.Save(contentFilePath);
        }
   }
}