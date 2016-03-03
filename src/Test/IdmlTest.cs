// --------------------------------------------------------------------------------------------
// <copyright file="IdmlTest.cs" from='2011' to='2011' company='SIL International'>
//      Copyright ( c ) 2011, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// IDML Test Support
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
    public static class IdmlTest
    {
        /// <summary>
        /// Compares all idml's in outputPath to make sure the content.xml and styles.xml are the same
        /// </summary>
        public static void AreEqual(string expectFullName, string outputFullName, string msg)
        {
            using (var expFl = new ZipFile(expectFullName))
            {
                var outFl = new ZipFile(outputFullName);                
                foreach (ZipEntry zipEntry in expFl)
                {
                    //TODO: designmap.xml should be tested but \\MetadataPacketPreference should be ignored as it contains the creation date.
                    if (!CheckFile(zipEntry.Name,"Stories,Spreads,Resources,MasterSpreads"))
                        continue;
                    if (Path.GetExtension(zipEntry.Name) != ".xml")
                        continue;
                    string outputEntry = new StreamReader(outFl.GetInputStream(outFl.GetEntry(zipEntry.Name).ZipFileIndex)).ReadToEnd();
                    string expectEntry = new StreamReader(expFl.GetInputStream(expFl.GetEntry(zipEntry.Name).ZipFileIndex)).ReadToEnd();
                    XmlDocument outputDocument = new XmlDocument();
                    outputDocument.XmlResolver = FileStreamXmlResolver.GetNullResolver();
                    outputDocument.LoadXml(outputEntry);
                    XmlDocument expectDocument = new XmlDocument();
                    outputDocument.XmlResolver = FileStreamXmlResolver.GetNullResolver();
                    expectDocument.LoadXml(expectEntry);
                    XmlDsigC14NTransform outputCanon = new XmlDsigC14NTransform();
                    outputCanon.Resolver = new XmlUrlResolver();
                    outputCanon.LoadInput(outputDocument);
                    XmlDsigC14NTransform expectCanon = new XmlDsigC14NTransform();
                    expectCanon.Resolver = new XmlUrlResolver();
                    expectCanon.LoadInput(expectDocument);
                    Stream outputStream = (Stream)outputCanon.GetOutput(typeof(Stream));
                    Stream expectStream = (Stream)expectCanon.GetOutput(typeof(Stream));
                    string errMessage = string.Format("{0}: {1} doesn't match", msg, zipEntry.Name);
                    Assert.AreEqual(expectStream.Length, outputStream.Length, errMessage);
                    FileAssert.AreEqual(expectStream, outputStream, errMessage);
                }
            }
        }

        private static bool CheckFile(string name, string possibleBeginnings)
        {
            foreach (string begining in possibleBeginnings.Split(new [] {','}))
                if (name.StartsWith(begining))
                    return true;
            return false;
        }
    }
}
