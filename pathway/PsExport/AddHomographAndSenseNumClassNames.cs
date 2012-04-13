#region // Copyright (c) 2012, SIL International. All Rights Reserved.
// <copyright from='2011' to='2012' company='SIL International'>
//		Copyright (c) 2011, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: AddHomographAndSenseNumClassNames.cs
// Responsibility: Greg Trihus
// ---------------------------------------------------------------------------------------------
using System.Text.RegularExpressions;
using System.Xml;

namespace RevHomographNum
{
    public static class AddHomographAndSenseNumClassNames
    {
        #region InsertHomographClass
        private static bool _insertHomographClass = true;
        public static bool InsertHomographClass
        {
            get { return _insertHomographClass; }
            set { _insertHomographClass = value; }
        }
        #endregion InsertHomographClass

        #region InsertSenseNumClass
        private static bool _insertSenseNumClass = true;
        public static bool InsertSenseNumClass
        {
            get { return _insertSenseNumClass; }
            set { _insertSenseNumClass = value; }
        }
        #endregion InsertSenseNumClass

        public static void Execute(string inputFile, string output)
        {
            if (!_insertHomographClass && !_insertSenseNumClass)
                return;
            var xmlDoc = new XmlDocument { XmlResolver = null };
            var nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            const string xhtmlns = "http://www.w3.org/1999/xhtml";
            nsmgr.AddNamespace("x", xhtmlns);
            xmlDoc.Load(inputFile);
            var changed = false;

            if (_insertSenseNumClass)
            {
                var senseNums = xmlDoc.SelectNodes("//x:span[@class='headref']/x:span", nsmgr);
                foreach (XmlElement senseNum in senseNums)
                {
                    if (!senseNum.HasAttribute("class"))
                    {
                        senseNum.SetAttribute("class", "revsensenumber");
                        changed = true;
                    }
                }
            }

            if (_insertHomographClass)
            {
                var nodes = xmlDoc.SelectNodes("//x:span[@class='headref']/text()", nsmgr);
                foreach (XmlNode node in nodes)
                {
                    var match = Regex.Match(node.InnerText, "([^0-9]*)([0-9]*)");
                    if (match.Groups[2].Length > 0)
                    {
                        var homographNode = xmlDoc.CreateElement("span", xhtmlns);
                        homographNode.SetAttribute("class", "revhomographnumber");
                        homographNode.InnerText = match.Groups[2].Value;
                        node.InnerText = match.Groups[1].Value;
                        node.ParentNode.InsertAfter(homographNode, node);
                        changed = true;
                    }
                }
            }
            if (changed)
            {
                var xmlWriter = XmlWriter.Create(output);
                xmlDoc.WriteTo(xmlWriter);
                xmlWriter.Close();
            }
            xmlDoc.RemoveAll();
        }
    }
}
