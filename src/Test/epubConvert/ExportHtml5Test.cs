// --------------------------------------------------------------------------------------------
#region Copyright ( c ) 2016, SIL International. All Rights Reserved.
// <copyright file="ExportHtml5Test.cs" from='2016' to='2016' company='SIL International'>
//      Copyright ( c ) 2016, SIL International. All Rights Reserved.
//
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed:
//
// <remarks>
//
// </remarks>
// --------------------------------------------------------------------------------------------

using System.Diagnostics;
using System.IO;
using System.Xml;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.epubConvert
{
    /// <summary>
    ///</summary>
    [TestFixture]
    public class ExportHtml5Test
    {
        #region Private Variables

        private TestFiles _testFiles;
        private readonly XmlReaderSettings _readerSettings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore, XmlResolver = new NullResolver() };

        #endregion

        #region Setup
        [TestFixtureSetUp]
        protected void SetUp()
        {
            _testFiles = new TestFiles("epubConvert");
        }
        #endregion Setup

        #region Tests
        [Test]
        public void XmlOneToManySplit1Test()
        {
            const string testName = "XmlOneToManySplit1";
            var inputFullPath = _testFiles.SubInput("html5",testName + ".xhtml");
            var outputFullPath = _testFiles.Output(testName + ".xhtml");
            var html5Split = new SplitXml(inputFullPath, "letHead");
            using (var sw = new StreamWriter(outputFullPath))
            {
                sw.Write(html5Split.Header);
                sw.Close();
            }
            XmlAssert.AreEqual(_testFiles.Expected(testName + ".xhtml"), outputFullPath, "incorrect header");
        }

        [Test]
        public void XmlOneToManySplit2Test()
        {
            const string testName = "XmlOneToManySplit2";
            var inputFullPath = _testFiles.SubInput("html5",testName + ".xhtml");
            var outputFullPath = _testFiles.Output(testName + ".xhtml");
            var html5Split = new SplitXml(inputFullPath, "letHead");
            html5Split.Folder = testName;
            var output = html5Split.CreateOutputFolderAt(Path.GetDirectoryName(outputFullPath));
            while (!html5Split.EndOfFile())
            {
                html5Split.Parse();
            }
            var di = new DirectoryInfo(output);
            Assert.AreEqual(4, di.GetFiles().Length, "Wrong number of files.");
            var doc4 = new XmlDocument();
            doc4.Load(XmlReader.Create(Path.Combine(output, "s4.html"), _readerSettings));
            var nodes = doc4.SelectNodes("//*");
            Debug.Assert(nodes != null, "nodes != null");
            Assert.AreEqual(282, nodes.Count, "Wrong number of nodes in 4th document");
        }

        [Test]
        public void LargeBuangFileTest()
        {
            const string testName = "LargeBuangFile";
            var inputFullPath = _testFiles.SubInput("html5",testName + ".xhtml");
            var outputFullPath = _testFiles.Output(testName + ".xhtml");
            var html5Split = new SplitXml(inputFullPath, "letHead");
            html5Split.Folder = testName;
            var output = html5Split.CreateOutputFolderAt(Path.GetDirectoryName(outputFullPath));
            while (!html5Split.EndOfFile())
            {
                html5Split.Parse();
            }
            var di = new DirectoryInfo(output);
            Assert.AreEqual(157, di.GetFiles("*.html").Length, "Wrong number of files.");
            var doc4 = new XmlDocument();
            doc4.Load(XmlReader.Create(Path.Combine(output, "s4.html"), _readerSettings));
            var nodes = doc4.SelectNodes("//*");
            Debug.Assert(nodes != null, "nodes != null");
            Assert.AreEqual(1549, nodes.Count, "Wrong number of nodes in 4th document");
        }

        [Test]
        public void LargeWithColsTest()
        {
            const string testName = "LargeWithCols";
            var inputFullPath = _testFiles.SubInput("html5",testName + ".xhtml");
            var outputFullPath = _testFiles.Output(testName + ".xhtml");
            var html5Split = new SplitXml(inputFullPath, "letHead");
            html5Split.Folder = testName;
            var output = html5Split.CreateOutputFolderAt(Path.GetDirectoryName(outputFullPath));
            while (!html5Split.EndOfFile())
            {
                html5Split.Parse();
            }
            var di = new DirectoryInfo(output);
            Assert.AreEqual(198, di.GetFiles("*.html").Length, "Wrong number of files.");
            var doc4 = new XmlDocument();
            doc4.Load(XmlReader.Create(Path.Combine(output, "s4.html"), _readerSettings));
            var nodes = doc4.SelectNodes("//*");
            Debug.Assert(nodes != null, "nodes != null");
            Assert.AreEqual(6031, nodes.Count, "Wrong number of nodes in 4th document");
        }

        [Test]
        public void AddCssTest()
        {
            const string testName = "AddCss";
            var inputFullPath = _testFiles.SubInput("html5",testName + ".xhtml");
            var outputFullPath = _testFiles.Output(testName + ".xhtml");
            var html5Split = new SplitXml(inputFullPath, "letHead");
            html5Split.Folder = testName;
            var output = html5Split.CreateOutputFolderAt(Path.GetDirectoryName(outputFullPath));
            var inpCss = _testFiles.SubInput("html5",testName + ".css");
            var outCss = html5Split.AddCss(inpCss);
            var di = new DirectoryInfo(Path.Combine(output, "css"));
            Assert.AreEqual(1, di.GetFiles("*.css").Length, "Wrong number of files.");
            TextFileAssert.AreEqual(inpCss, outCss);
        }

        [Test]
        public void ChangeCssLinkTest()
        {
            const string testName = "ChangeCssLink";
            var inputFullPath = _testFiles.SubInput("html5",testName + ".xhtml");
            var outputFullPath = _testFiles.Output(testName + ".xhtml");
            var html5Split = new SplitXml(inputFullPath, "letHead");
            html5Split.Folder = testName;
            var output = html5Split.CreateOutputFolderAt(Path.GetDirectoryName(outputFullPath));
            html5Split.AddCss(_testFiles.SubInput("html5",testName + ".css"));
            while (!html5Split.EndOfFile())
            {
                html5Split.Parse();
            }
            var doc1 = new XmlDocument();
            doc1.Load(XmlReader.Create(Path.Combine(output, "s1.html"), _readerSettings));
            // ReSharper disable once PossibleNullReferenceException
            Assert.AreEqual("css/" + testName + ".css", doc1.SelectSingleNode("//*[@rel='stylesheet']/@href").InnerText);
        }

        [Test]
        public void FileListTest()
        {
            const string testName = "FileList";
            var inputFullPath = _testFiles.SubInput("html5",testName + ".xhtml");
            var outputFullPath = _testFiles.Output(testName + ".xhtml");
            var html5Split = new SplitXml(inputFullPath, "letHead");
            html5Split.Folder = testName;
            html5Split.CreateOutputFolderAt(Path.GetDirectoryName(outputFullPath));
            while (!html5Split.EndOfFile())
            {
                html5Split.Parse();
            }
            Assert.AreEqual(17, html5Split.FileList.Count);
        }

        [Test]
        public void LetterButtonsTest()
        {
            const string testName = "LetterButtons";
            var inputFullPath = _testFiles.SubInput("html5",testName + ".xhtml");
            var outputFullPath = _testFiles.Output(testName + ".xhtml");
            var html5Split = new SplitXml(inputFullPath, "letHead");
            html5Split.Folder = testName;
            html5Split.CreateOutputFolderAt(Path.GetDirectoryName(outputFullPath));
            while (!html5Split.EndOfFile())
            {
                html5Split.Parse();
            }
            Assert.AreEqual(5, html5Split.LetterLabels.Count);
            Assert.AreEqual("Ngw ngw", html5Split.LetterLabels[3]);
            Assert.AreEqual("s14.html", html5Split.LetterLinks[3]);
        }

        [Test]
        public void ReplaceLinksTest()
        {
            const string testName = "ReplaceLinks";
            var inputFullPath = _testFiles.SubInput("html5",testName + ".xhtml");
            var outputFullPath = _testFiles.Output(testName + ".xhtml");
            var html5Split = new SplitXml(inputFullPath, "letHead");
            html5Split.Folder = testName;
            var output = html5Split.CreateOutputFolderAt(Path.GetDirectoryName(outputFullPath));
            while (!html5Split.EndOfFile())
            {
                html5Split.Parse();
            }
            var doc1 = new XmlDocument();
            doc1.Load(XmlReader.Create(Path.Combine(output, "s1.html"), _readerSettings));
            // ReSharper disable once PossibleNullReferenceException
            Assert.AreEqual("gae831c2b", doc1.SelectSingleNode("//@id").InnerText);
            // ReSharper disable once PossibleNullReferenceException
            Assert.AreEqual("gae831c2b", doc1.SelectSingleNode("//@entryguid").InnerText);
        }

        [Test]
        public void IdLinksTest()
        {
            const string testName = "IdLinks";
            var inputFullPath = _testFiles.SubInput("html5",testName + ".xhtml");
            var outputFullPath = _testFiles.Output(testName + ".xhtml");
            var html5Split = new SplitXml(inputFullPath, "letHead");
            html5Split.Folder = testName;
            html5Split.CreateOutputFolderAt(Path.GetDirectoryName(outputFullPath));
            while (!html5Split.EndOfFile())
            {
                html5Split.Parse();
            }
            Assert.AreEqual(436, html5Split.IdLinks.Count);
            Assert.AreEqual("s6.html", html5Split.IdLinks["gf921a938"]);
        }

        #endregion Tests

    }
}
