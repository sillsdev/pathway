using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    class ModifyLOContent
    {
        private XmlDocument _contentXMLdoc;
        public void SetTableColumnCount(string contentFilePath, Dictionary<string, int> tableColumnModify)
        {
            if(tableColumnModify == null || tableColumnModify.Count == 0)
                return;

            string _xPath;
            _contentXMLdoc = new XmlDocument();
            _contentXMLdoc.PreserveWhitespace = true;
            _contentXMLdoc.Load(contentFilePath);
            XmlElement _root = _contentXMLdoc.DocumentElement;
            var nsmgr = new XmlNamespaceManager(_contentXMLdoc.NameTable);
            nsmgr.AddNamespace("style", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
            nsmgr.AddNamespace("table", "urn:oasis:names:tc:opendocument:xmlns:table:1.0");
            foreach (KeyValuePair<string, int> kvp in tableColumnModify)
            {
                //table:table[@table:name="Table1"]/table:table-column
                _xPath = "//table:table[@table:name=\"" + kvp.Key + "\"]/table:table-column";
                XmlNode node = _root.SelectSingleNode(_xPath, nsmgr);
                if (node == null) return;

                node.Attributes[1].Value = kvp.Value.ToString();
            }
            _contentXMLdoc.Save(contentFilePath);
        }
   }
}