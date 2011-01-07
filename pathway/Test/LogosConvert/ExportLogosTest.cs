// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2010, SIL International. All Rights Reserved.
// <copyright from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: LogosConvertTest.cs
// Responsibility: Trihus
// ---------------------------------------------------------------------------------------------
using System;
using System.IO;
using System.Security.Cryptography.Xml;
using System.Xml;
using ICSharpCode.SharpZipLib.Zip;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.LogosConvert
{
    /// ----------------------------------------------------------------------------------------
    /// <summary>
    /// Test functions of Logos Convert
    /// </summary>
    /// ----------------------------------------------------------------------------------------
    [TestFixture]
    public class ExportLogosTest : ExportLogos
    {
        #region setup
        private TestFiles _TestFiles;

        [TestFixtureSetUp]
        public void Setup()
        {
            Common.ProgInstall = PathPart.Bin(Environment.CurrentDirectory, @"/../PsSupport");
            Common.SupportFolder = "";
            Common.ProgBase = Common.ProgInstall;
            _TestFiles = new TestFiles("LogosConvert");
        }
        #endregion setup

        [Test]
        public void ExportTypeTest()
        {
            var target = new ExportLogos();
            var actual = target.ExportType;
            Assert.AreEqual("Logos Alpha", actual);
        }

        [Test]
        public void HandleDictionaryTest()
        {
            var target = new ExportLogos();
            var actual = target.Handle("Dictionary");
            Assert.IsFalse(actual);
        }

        [Test]
        public void HandleScriptureTest()
        {
            var target = new ExportLogos();
            var actual = target.Handle("Scripture");
            Assert.IsTrue(actual);
        }

        /// <summary>
        ///A test for Export
        ///</summary>
        [Test]
        public void ExportNullTest()
        {
            var target = new ExportLogos();
            PublicationInformation projInfo = null;
            Assert.Throws<ArgumentNullException>(
                delegate
                    {
                        target.Export(projInfo);
                    }
                );
        }

        /// <summary>
        ///A test for Export
        ///</summary>
        [Test]
        public void ExportPassTest()
        {
            const string XhtmlName = "Bughotu-gospels.xhtml";
            const string CssName = "Bughotu-gospels.css";
            PublicationInformation projInfo = GetProjInfo(XhtmlName, CssName);
            //projInfo.IsOpenOutput = true;
            _TestFiles.Copy("FileGuids.xml");
            var target = new ExportLogos();
            var actual = target.Export(projInfo);
            Assert.IsTrue(actual);
            const string dataZip = "Bughotu-gospels.zip";
            ZipAreEqual(_TestFiles.Expected(dataZip), _TestFiles.Output(dataZip));
        }

        /// <summary>
        /// Test function used to prepare email message
        /// </summary>
        [Test]
        public void SanitizeText()
        {
            Assert.AreEqual("C%3A%5C5.3%2F8%22.zip", Sanitize("C:\\5.3/8\".zip"));
        }
        #region Private Functions
        /// <summary>
        /// Create a simple PublicationInformation instance
        /// </summary>
        private PublicationInformation GetProjInfo(string XhtmlName, string BlankName)
        {
            PublicationInformation projInfo = new PublicationInformation();
            projInfo.DefaultXhtmlFileWithPath = _TestFiles.Copy(XhtmlName);
            projInfo.DefaultCssFileWithPath = _TestFiles.Copy(BlankName);
            projInfo.IsOpenOutput = false;
            return projInfo;
        }

        private void ZipAreEqual(string expected, string actual)
        {
            var expectedZipFile = new ZipFile(expected);
            var actualZipFile = new ZipFile(actual);
            foreach (ZipEntry entry in expectedZipFile)
            {
                var expectedStream = expectedZipFile.GetInputStream(entry.ZipFileIndex);
                var actualEntry = actualZipFile.GetEntry(entry.Name);
                var actualStream = actualZipFile.GetInputStream(actualEntry.ZipFileIndex);
                XmlDocument outputDocument = new XmlDocument { XmlResolver = null };
                outputDocument.Load(actualStream);
                XmlDocument expectDocument = new XmlDocument { XmlResolver = null };
                expectDocument.Load(expectedStream);
                XmlDsigC14NTransform outputCanon = new XmlDsigC14NTransform();
                outputCanon.Resolver = null;
                outputCanon.LoadInput(outputDocument);
                XmlDsigC14NTransform expectCanon = new XmlDsigC14NTransform();
                expectCanon.Resolver = null;
                expectCanon.LoadInput(expectDocument);
                Stream outputStream = (Stream)outputCanon.GetOutput(typeof(Stream));
                Stream expectStream = (Stream)expectCanon.GetOutput(typeof(Stream));
                FileAssert.AreEqual(expectStream, outputStream);
            }
        }
        #endregion PrivateFunctions
    }
}
