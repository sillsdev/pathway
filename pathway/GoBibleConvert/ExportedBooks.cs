// --------------------------------------------------------------------------------------
// <copyright file="ExportedBooks.cs" from='2012' to='2012' company='SIL International'>
//      Copyright © 2012, SIL International. All Rights Reserved.
//
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed:
// 
// <remarks>
// </remarks>
// --------------------------------------------------------------------------------------
using System.Diagnostics;
using System.IO;
using System.Xml;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    class ExportedBooks
    {
        private XmlDocument _xmlDocument;
        private XmlNodeList _bookNodes;
        private PublicationInformation _projInfo;

        public ExportedBooks(PublicationInformation projInfo)
        {
            _projInfo = projInfo;
            var xhtmlFullName = projInfo.DefaultXhtmlFileWithPath;
            _xmlDocument = new XmlDocument { XmlResolver = null };
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, ProhibitDtd = false };
            XmlReader xmlReader = XmlReader.Create(xhtmlFullName, xmlReaderSettings);
            _xmlDocument.Load(xmlReader);
            xmlReader.Close();
            //XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(_xmlDocument.NameTable);
            //xmlNamespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            _bookNodes = _xmlDocument.SelectNodes("//*[@class='scrBook']");
        }

        public int Length {
            get { return _bookNodes.Count; } 
        }

        internal void MakeBook(int i)
        {
            var code = GetCode(_bookNodes[i]);
            var fileName = MakeFileName(code);
            File.Copy(_projInfo.DefaultXhtmlFileWithPath, fileName);
            var xDoc = new XmlDocument {XmlResolver = null};
            var xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, ProhibitDtd = false };
            var xmlReader = XmlReader.Create(fileName, xmlReaderSettings);
            xDoc.Load(xmlReader);
            xmlReader.Close();
            var xBooks = xDoc.SelectNodes("//*[@class='scrBook']");
            if (xBooks != null)
                for (int j = xBooks.Count; --j >= 0; )
                    if (j != i)
                    {
                        Debug.Assert(xBooks[j].ParentNode != null);
                        xBooks[j].ParentNode.RemoveChild(xBooks[j]);
                    }
            xDoc.Save(fileName);
        }

        private string MakeFileName(string code)
        {
            var baseFileName = Path.GetFileNameWithoutExtension(_projInfo.DefaultXhtmlFileWithPath);
            var folder = Path.GetDirectoryName(_projInfo.DefaultXhtmlFileWithPath);
            Debug.Assert(folder != null);
            var fileName = Common.PathCombine(folder, string.Format("{0}_{1}.xhtml", baseFileName, code));
            return fileName;
        }

        private string MakeCvFileName(string code)
        {
            var baseFileName = Path.GetFileNameWithoutExtension(_projInfo.DefaultXhtmlFileWithPath);
            var folder = Path.GetDirectoryName(_projInfo.DefaultXhtmlFileWithPath);
            Debug.Assert(folder != null);
            if (code != string.Empty)
                code += "_";
            var fileName = Common.PathCombine(folder, string.Format("{0}_{1}cv.xhtml", baseFileName, code));
            return fileName;
        }

        private static string GetCode(XmlNode bookNode)
        {
            var code = bookNode.SelectSingleNode(".//*[@class='scrBookCode']");
            if (code == null)
                return null;
            return code.InnerText;
        }

        public PublicationInformation BookPublicationInformation;

        internal PublicationInformation ProjInfo(int i)
        {
            BookPublicationInformation = _projInfo;
            var code = GetCode(_bookNodes[i]);
            var fileName = MakeFileName(code);
            BookPublicationInformation.DefaultXhtmlFileWithPath = fileName;
            return BookPublicationInformation;
        }

        internal string Combine()
        {
            var code = GetCode(_bookNodes[0]);
            var firstCvName = MakeCvFileName(code);
            var xDoc = new XmlDocument { XmlResolver = null };
            var xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, ProhibitDtd = false };
            var xmlReader = XmlReader.Create(firstCvName, xmlReaderSettings);
            xDoc.Load(xmlReader);
            xmlReader.Close();
            var bookNode = xDoc.SelectSingleNode("//*[@class='scrBook']");
            Debug.Assert(bookNode != null);
            var testamentNode = bookNode.ParentNode;
            Debug.Assert(testamentNode != null);
            for (int i = 1; i < _bookNodes.Count; i++)
            {
                code = GetCode(_bookNodes[i]);
                var nextReader = XmlReader.Create(MakeCvFileName(code), xmlReaderSettings);
                var nextDoc = new XmlDocument {XmlResolver = null};
                nextDoc.Load(nextReader);
                nextReader.Close();
                var nextBookNode = nextDoc.SelectSingleNode("//*[@class='scrBook']");
                Debug.Assert(nextBookNode != null && nextBookNode.ParentNode != null);
                var importedNode = nextBookNode.ParentNode.RemoveChild(nextBookNode);
                Debug.Assert(testamentNode.OwnerDocument != null);
                importedNode = testamentNode.OwnerDocument.ImportNode(importedNode, true);
                testamentNode.AppendChild(importedNode);
            }
            var outName = MakeCvFileName("");
            xDoc.Save(outName);
            return outName;
        }
    }
}
