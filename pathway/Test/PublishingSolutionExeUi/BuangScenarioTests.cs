// --------------------------------------------------------------------------------------------
// <copyright file="PsUiScenarioTests.cs" from='2009' to='2009' company='SIL International'>
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
// For DE scripts.
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using GuiTestDriver;
using NUnit.Framework;
using SIL.Tool;

namespace Test.PublishingSolutionExeUi
{
    /// <summary>
    /// DE script for Buang Scenario
    /// </summary>
    [TestFixture]
    [Category("Scenario")]
    [Category("LongTest")]
    [Ignore]
    public class BuangScenarioTests
    {
#if ScenarioTests
        #region Setup
        /// <summary>test root</summary>
        RunTest _runTest;

        /// <summary>content of odt if not null</summary>
        private XmlDocument _content = null;

        private XmlNamespaceManager _contentNamespace;

        public XmlNode Content
        {
            get
            {
                if (_content != null)
                    return _content.DocumentElement;
                _content = OdtTest.LoadXml(TestSourcePath + @"TestFiles/Output/Buang/Buang.odt", "content.xml");
                _contentNamespace = OdtTest.NamespaceManager(_content);
                return _content.DocumentElement;
            }
        }

        /// <summary>styles of odt if not null</summary>
        private XmlDocument _styles = null;

        private XmlNamespaceManager _stylesNamespace;

        public XmlNode Styles
        {
            get
            {
                if (_styles != null)
                    return _styles.DocumentElement;
                _styles = OdtTest.LoadXml(Common.DirectoryPathReplace(TestSourcePath + "TestFiles/Output/Buang/Buang.odt"), "styles.xml");
                _stylesNamespace = OdtTest.NamespaceManager(_styles);
                return _styles.DocumentElement;
            }
        }

        /// <summary>Base path so user can install these files anywhere on their hard drive</summary>
        public string TestSourcePath
        {
            get
            {
                return Common.DirectoryPathReplace(Environment.CurrentDirectory + "/../../PublishingSolutionExeUi/");
            }
        }

        /// <summary>
        /// setup test root
        /// </summary>
        [TestFixtureSetUp]
        protected void SetUp()
        {
            var format = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><include><var id=\"testPath\" set=\"{0}TestFiles\"/></include>";
            var sw = new StreamWriter(Common.DirectoryPathReplace(TestSourcePath + "Scripts/DE/testPath.xml"));
            sw.Write(string.Format(format, TestSourcePath));
            sw.Close();
            var outInfo = new DirectoryInfo(Common.DirectoryPathReplace(TestSourcePath + "TestFiles/Output"));
            var fileList = outInfo.GetDirectories();
            foreach (var folderName in fileList)
            {
                if (folderName.Name.Substring(0, 1) != ".")
                    Directory.Delete(folderName.FullName, true);
            }
            _runTest = new RunTest("DE");
            _runTest.fromFile("BuangSample.xml");
        }
        #endregion Setup

        #region TearDown
        [TearDown]
        protected void TearDown()
        {
            _content = null;
            _contentNamespace = null;
            _styles = null;
            _stylesNamespace = null;
        }
        #endregion TearDown

        [Test]
        public void PageWidth()
        {
            var actualValues = Styles.SelectNodes("/office:document-styles/office:automatic-styles/style:page-layout/style:page-layout-properties/@fo:page-width", _stylesNamespace);
            foreach (XmlAttribute actual in actualValues)
                Assert.AreEqual("6.378in", actual.Value);
        }

        [Test]
        public void PageHeight()
        {
            var actualValues = Styles.SelectNodes("/office:document-styles/office:automatic-styles/style:page-layout/style:page-layout-properties/@fo:page-height", _stylesNamespace);
            foreach (XmlAttribute actual in actualValues)
                Assert.AreEqual("9.0161in", actual.Value);
        }

        [Test]
        public void ColumnCount()
        {
            var actualValues = Content.SelectNodes("/office:document-content/office:automatic-styles/style:style/style:section-properties/style:columns/@fo:column-count", _contentNamespace);
            Dictionary<string, int> totals = new Dictionary<string, int>();
            foreach (XmlAttribute actual in actualValues)
                if (totals.ContainsKey(actual.Value))
                    totals[actual.Value] += 1;
                else
                    totals[actual.Value] = 1;
            Assert.AreEqual(2, totals.Count);
        }

        [Test]
        public void BlockSense()
        {
            var actualValues = Content.SelectNodes("/office:document-content/office:body/office:text/text:section/text:p[starts-with(@text:style-name, \"sense\")]", _contentNamespace);
            Assert.GreaterOrEqual(actualValues.Count,2);
        }

        [Test]
        public void NoSpanSense()
        {
            var actualValues = Content.SelectNodes("/office:document-content/office:body/office:text/text:section/text:p/text:span[starts-with(@text:style-name, \"sense_\")]", _contentNamespace);
            Assert.AreEqual(0, actualValues.Count);
        }
#endif
    }
}