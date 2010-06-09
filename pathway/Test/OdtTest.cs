// --------------------------------------------------------------------------------------------
// <copyright file="OdtTest.cs" from='2009' to='2009' company='SIL International'>
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
// ODT Test Support
// </remarks>
// --------------------------------------------------------------------------------------------
using System.IO;
using System.Security.Cryptography.Xml;
using System.Xml;
using ICSharpCode.SharpZipLib.Zip;
using NUnit.Framework;
using SIL.Tool;

namespace Test
{
    public static class OdtTest
    {
        /// <summary>
        /// Compares all odt's in outputPath to make sure the content.xml and styles.xml are the same
        /// </summary>
        /// <param name="expectPath">expected output path</param>
        /// <param name="outputPath">output path</param>
        /// <param name="msg">message to display if mismatch</param>
        public static void AreEqual(string expectPath, string outputPath, string msg)
        {
            var outDi = new DirectoryInfo(outputPath);
            var expDi = new DirectoryInfo(expectPath);
            FileInfo[] outFi = outDi.GetFiles("*.od*");
            FileInfo[] expFi = expDi.GetFiles("*.od*");
            Assert.AreEqual(outFi.Length, expFi.Length, string.Format("{0} {1} odt found {2} expected", msg, outFi.Length, expFi.Length));
            foreach (FileInfo fi in outFi)
            {
                var outFl = new ZipFile(fi.FullName);
                var expFl = new ZipFile(Common.PathCombine(expectPath, fi.Name));
                foreach (string name in "content.xml,styles.xml".Split(','))
                {
                    string outputEntry = new StreamReader(outFl.GetInputStream(outFl.GetEntry(name).ZipFileIndex)).ReadToEnd();
                    string expectEntry = new StreamReader(expFl.GetInputStream(expFl.GetEntry(name).ZipFileIndex)).ReadToEnd();
                    XmlDocument outputDocument = new XmlDocument();
                    outputDocument.XmlResolver = null;
                    outputDocument.LoadXml(outputEntry);
                    XmlDocument expectDocument = new XmlDocument();
                    expectDocument.XmlResolver = null;
                    expectDocument.LoadXml(expectEntry);
                    XmlDsigC14NTransform outputCanon = new XmlDsigC14NTransform();
                    outputCanon.Resolver = null;
                    outputCanon.LoadInput(outputDocument);
                    XmlDsigC14NTransform expectCanon = new XmlDsigC14NTransform();
                    expectCanon.Resolver = null;
                    expectCanon.LoadInput(expectDocument);
                    Stream outputStream = (Stream)outputCanon.GetOutput(typeof(Stream));
                    Stream expectStream = (Stream)expectCanon.GetOutput(typeof(Stream));
                    string errMessage = string.Format("{0}: {1} doesn't match", msg, name);
                    Assert.AreEqual(expectStream.Length, outputStream.Length, errMessage);
                    FileAssert.AreEqual(expectStream, outputStream, errMessage);
                }
            }
        }

        /// <summary>
        /// Load Xml data from part of ODT file (usually content.xml or styles.xml)
        /// </summary>
        /// <param name="odtPath">full path to odt</param>
        /// <param name="sectionName">section name (usually content.xml or styles.xml)</param>
        /// <returns>xmlDocument with <paramref name="sectionName">sectionName</paramref> loaded</returns>
        public static XmlDocument LoadXml(string odtPath, string sectionName)
        {
            var odtFile = new ZipFile(odtPath);
            var reader = new StreamReader(odtFile.GetInputStream(odtFile.GetEntry(sectionName).ZipFileIndex));
            var text = reader.ReadToEnd();
            reader.Close();
            odtFile.Close();
            var xmlDocument = new XmlDocument();
            xmlDocument.XmlResolver = null;
            xmlDocument.LoadXml(text);
            return xmlDocument;
        }

        public static XmlNamespaceManager NamespaceManager(XmlDocument xmlDocument)
        {
            var root = xmlDocument.DocumentElement;
            Assert.IsNotNull(root, "Missing xml document");
            var nsManager = new XmlNamespaceManager(xmlDocument.NameTable);
            foreach (XmlAttribute attribute in root.Attributes)
            {
                var namePart = attribute.Name.Split(':');
                if (namePart[0] == "xmlns")
                    nsManager.AddNamespace(namePart[1], attribute.Value);
            }
            return nsManager;
        }
    }
}
