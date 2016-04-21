// --------------------------------------------------------------------------------------------
// <copyright file="CssSimplerTest.cs" from='2016' to='2016' company='SIL International'>
//      Copyright (C) 2016, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Tests for CssSimpler
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using BuildStep;
using NUnit.Framework;
using SIL.PublishingSolution;

// ReSharper disable once CheckNamespace
namespace Test.CssSimplerTest
{
    [TestFixture]
    public class CssSimplerTest : CssSimpler.Program
    {
        #region Private Variables

        private TestFiles _testFiles;

        #endregion

        #region Setup
        [TestFixtureSetUp]
        protected void SetUp()
        {
            _testFiles = new TestFiles("CssSimpler");
            // ReSharper disable AssignNullToNotNullAttribute
            var xhtmlSimplifyName = _testFiles.Input(@"..\..\..\..\CssSimpler\XhtmlSimplify.xsl");
            SimplifyXhtml.Load(XmlReader.Create(new StreamReader(xhtmlSimplifyName)));
            var xmlCssName = _testFiles.Input(@"..\..\..\..\CssSimpler\XmlCss.xsl");
            XmlCss.Load(XmlReader.Create(new StreamReader(xmlCssName)));
            var xmlCssSimplifyName = _testFiles.Input(@"..\..\..\..\CssSimpler\XmlCssSimplify.xsl");
            SimplifyXmlCss.Load(XmlReader.Create(new StreamReader(xmlCssSimplifyName)));
            // ReSharper restore AssignNullToNotNullAttribute
        }
        #endregion Setup

        #region Tests

        /// <summary>
        ///A test for WriteSimpleXhtml
        ///</summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void WriteSimpleXhtmlTest1()
        {
            WriteSimpleXhtml(null);
        }

        /// <summary>
        ///A test for WriteSimpleXhtml
        ///</summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void WriteSimpleXhtmlTest2()
        {
            const string testName = "WriteSimpleXhtmlTest2";
            string xhtmlFullName = testName + ".xhtml";
            WriteSimpleXhtml(xhtmlFullName);
        }

        /// <summary>
        ///A test for WriteSimpleXhtml
        ///</summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void WriteSimpleXhtmlTest3()
        {
            const string testName = "WriteSimpleXhtmlTest3";
            string xhtmlFullName = _testFiles.Output(testName + ".xhtml");
            WriteSimpleXhtml(xhtmlFullName);
        }

        /// <summary>
        ///A test for WriteSimpleXhtml
        ///</summary>
        [Test]
        public void WriteSimpleXhtmlTest4()
        {
            const string testName = "WriteSimpleXhtmlTest4";
            var fileName = testName + ".xhtml";
            _testFiles.Copy(fileName);
            string xhtmlFullName = _testFiles.Output(fileName);
            WriteSimpleXhtml(xhtmlFullName);
        }

		/// <summary>
		///A test for WriteSimpleXhtml
		/// Checks whether the body tag has the class dicBody
		///</summary>
		[Test]
		public void IsDicBodyExistsTest()
		{
			const string testName = "IsDicBodyExistsTest";
			var fileName = testName + ".xhtml";
			_testFiles.Copy(fileName);
			string xhtmlFullName = _testFiles.Output(fileName);
			WriteSimpleXhtml(xhtmlFullName);
			var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore };
			var xhtml = new XmlDocument();
			xhtml.Load(XmlReader.Create(xhtmlFullName, settings));
			var checkClass = xhtml.SelectSingleNode("//body[@class='dicBody']");
			Assert.IsNull(checkClass);
		}

        /// <summary>
        ///A test for WriteSimpleCss
        ///</summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteSimpleCssTest1()
        {
            WriteSimpleCss(null, null);
        }

        /// <summary>
        ///A test for WriteSimpleCss
        ///</summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteSimpleCssTest2()
        {
            var stylesheet = string.Empty;
            var xml = new XmlDocument();
            WriteSimpleCss(stylesheet, xml);
        }

        /// <summary>
        ///A test for WriteSimpleCss
        ///</summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteSimpleCssTest3()
        {
            const string testName = "WriteSimpleCssTest3";
            var stylesheet = testName + ".css";
            var xml = new XmlDocument();
            WriteSimpleCss(stylesheet, xml);
        }

        /// <summary>
        ///A test for WriteSimpleCss
        ///</summary>
        [Test]
        [ExpectedException(typeof(XmlException))]
        public void WriteSimpleCssTest4()
        {
            const string testName = "WriteSimpleCssTest4";
            var fileName = testName + ".css";
            _testFiles.Copy(fileName);
            var stylesheet = _testFiles.Output(fileName);
            var xml = new XmlDocument();
            WriteSimpleCss(stylesheet, xml);
        }

        /// <summary>
        ///A test for WriteSimpleCss
        ///</summary>
        [Test]
        public void WriteSimpleCssTest5()
        {
            const string testName = "WriteSimpleCssTest5";
            var fileName = testName + ".css";
            _testFiles.Copy(fileName);
            var stylesheet = _testFiles.Output(fileName);
            var xml = new XmlDocument();
            xml.LoadXml("<root/>");
            WriteSimpleCss(stylesheet, xml);
        }

        /// <summary>
        ///A test for AddSubTree
        ///</summary>
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void AddSubTreeTest1()
        {
            AddSubTree(null, null, null);
        }

        /// <summary>
        ///A test for AddSubTree
        ///</summary>
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void AddSubTreeTest2()
        {
            var ctp = new CssTreeParser();
            AddSubTree(null, null, ctp);
        }

        /// <summary>
        ///A test for AddSubTree
        ///</summary>
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void AddSubTreeTest3()
        {
            const string testName = "AddSubTree3";
            var fileName = testName + ".css";
            var ctp = new CssTreeParser();
            ctp.Parse(_testFiles.Input(fileName));
            var root = ctp.Root;
            AddSubTree(null, root, ctp); //when an empty css file is given root == null
        }

        /// <summary>
        ///A test for AddSubTree
        ///</summary>
        [Test]
        [ExpectedException(typeof(NullReferenceException))] // since the first argument is null
        public void AddSubTreeTest4()
        {
            const string testName = "AddSubTree4";
            var fileName = testName + ".css";
            var ctp = new CssTreeParser();
            ctp.Parse(_testFiles.Input(fileName));
            var root = ctp.Root;
            Assert.True(root != null);
            AddSubTree(null, root, ctp);
        }

        /// <summary>
        ///A test for AddSubTree
        ///</summary>
        [Test]
        public void AddSubTreeTest5()
        {
            const string testName = "AddSubTree5";
            var fileName = testName + ".css";
            var ctp = new CssTreeParser();
            ctp.Parse(_testFiles.Input(fileName));
            var root = ctp.Root;
            Assert.True(root != null);
            var xml = new XmlDocument();
            xml.LoadXml("<root/>");
            _uniqueClasses = new List<string> {"entry"};
            AddSubTree(xml.DocumentElement, root, ctp);
        }

        /// <summary>
        ///A test for AddSubTree
        /// Covers all code linse in AddSubTree
        ///</summary>
        [Test]
        public void AddSubTreeTest6()
        {
            const string testName = "AddSubTree6";
            var fileName = testName + ".css";
            var ctp = new CssTreeParser();
            ctp.Parse(_testFiles.Input(fileName));
            var root = ctp.Root;
            Assert.True(root != null);
            var xml = new XmlDocument();
            xml.LoadXml("<root/>");
            _uniqueClasses = new List<string> { "entry", "pronunciations", "pronunciation", "form" };
            AddSubTree(xml.DocumentElement, root, ctp);
            WriteCssXml(_testFiles.Output(testName + ".xml"), xml);
            Debug.Assert(xml.DocumentElement != null, "xml.DocumentElement != null");
            Assert.AreEqual(3, xml.DocumentElement.ChildNodes.Count,"wrong number of rules retained");
            var termnode1 = xml.SelectSingleNode("//RULE[1]/@term");
            Assert.NotNull(termnode1, "Missing term attribute");
            Assert.AreEqual("1", termnode1.InnerText, "First rule should have only one selector");
            var marginPropertyName = xml.SelectSingleNode("//RULE[1]/PROPERTY/name");
            Debug.Assert(marginPropertyName != null, "margin-left propert name missing");
            Assert.AreEqual("margin-left", marginPropertyName.InnerText, "margin-left property name invalid");
            var marginPropertyValue = xml.SelectSingleNode("//RULE[1]/PROPERTY/value");
            Debug.Assert(marginPropertyValue != null, "margin-left propert value missing");
            Assert.AreEqual(".5", marginPropertyValue.InnerText, "margin-left property value invalid");
            var marginPropertyUnits = xml.SelectSingleNode("//RULE[1]/PROPERTY/unit");
            Debug.Assert(marginPropertyUnits != null, "margin-left propert units missing");
            Assert.AreEqual("in", marginPropertyUnits.InnerText, "margin-left property units invalid");
            var termnode3 = xml.SelectSingleNode("//RULE[3]/@term");
            Assert.NotNull(termnode3, "Missing term attribute on third rule");
            Assert.AreEqual("9", termnode3.InnerText, "Third rule should have nine selector terms");
            var lastClass = xml.SelectSingleNode("//RULE[3]/@lastClass");
            Debug.Assert(lastClass != null, "lastClass != null");
            Assert.AreEqual("pronunciation", lastClass.InnerText, "Last class name on the third rule should be pronunciation");
            var target = xml.SelectSingleNode("//RULE[3]/@target");
            Debug.Assert(target != null, "target != null");
            Assert.AreEqual("span", target.InnerText, "The target css selector for the third rule should be span");
        }

        /// <summary>
        ///A test for translation simplification
        ///</summary>
        [Test]
        public void TranslationTest()
        {
            const string testName = "translation";
            var fileName = testName + ".xhtml";
            _testFiles.Copy(fileName);
            string xhtmlFullName = _testFiles.Output(fileName);
            WriteSimpleXhtml(xhtmlFullName);
            var settings = new XmlReaderSettings {DtdProcessing = DtdProcessing.Ignore};
            var xhtml = new XmlDocument();
            xhtml.Load(XmlReader.Create(xhtmlFullName, settings));
            var translationNodes = xhtml.SelectSingleNode("//translation/translation");
            Assert.IsNull(translationNodes);
        }

        /// <summary>
        ///A test for change to complete example
        ///</summary>
        [Test]
        public void CompleteExampleTest()
        {
            const string testName = "completeExample";
            var fileName = testName + ".xhtml";
            _testFiles.Copy(fileName);
            string xhtmlFullName = _testFiles.Output(fileName);
            WriteSimpleXhtml(xhtmlFullName);
            var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore };
            var xhtml = new XmlDocument();
            xhtml.Load(XmlReader.Create(xhtmlFullName, settings));
            var exampeNodes = xhtml.SelectNodes("//*[@class='complete']/*[@class='example']");
            Debug.Assert(exampeNodes != null, "Missing comlete example nodes results!");
            Assert.AreEqual(2, exampeNodes.Count);
        }

        /// <summary>
        ///A test for remove level with span of type pictures
        ///</summary>
        [Test]
        public void PicturesXhtmlTest()
        {
            const string testName = "pictures";
            var fileName = testName + ".xhtml";
            _testFiles.Copy(fileName);
            string xhtmlFullName = _testFiles.Output(fileName);
            WriteSimpleXhtml(xhtmlFullName);
            var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore };
            var xhtml = new XmlDocument();
            xhtml.Load(XmlReader.Create(xhtmlFullName, settings));
            var picturesNodes = xhtml.SelectSingleNode("//*[local-name()='span'][@class='pictures']");
            Assert.IsNull(picturesNodes);
            var pictureNode = xhtml.SelectNodes("//*[@class='picture']");
            Debug.Assert(pictureNode != null, "missing picture node results!");
            Assert.AreEqual(1, pictureNode.Count);
        }

        /// <summary>
        ///A test for removal of rules beginning with div at root.
        ///</summary>
        [Test]
        public void DivEntryTest()
        {
            const string testName = "divEntry";
            var fileName = testName + ".css";
            var ctp = new CssTreeParser();
            ctp.Parse(_testFiles.Input(fileName));
            var root = ctp.Root;
            Assert.True(root != null);
            var xml = new XmlDocument();
            xml.LoadXml("<root/>");
            _uniqueClasses = new List<string> {"entry"};
            AddSubTree(xml.DocumentElement, root, ctp);
            WriteCssXml(_testFiles.Output(testName + ".xml"), xml);
            _testFiles.Copy(fileName);
            var resultFile = _testFiles.Output(fileName);
            WriteSimpleCss(resultFile, xml);
            var result = FileData.Get(resultFile);
            Assert.True(result.StartsWith(".entry"));
        }

        /// <summary>
        ///A test for removal of pictures from rules
        ///</summary>
        [Test]
        public void PicturesCssTest()
        {
            const string testName = "pictures";
            var fileName = testName + ".css";
            var ctp = new CssTreeParser();
            ctp.Parse(_testFiles.Input(fileName));
            var root = ctp.Root;
            Assert.True(root != null);
            var xml = new XmlDocument();
            xml.LoadXml("<root/>");
            _uniqueClasses = new List<string> { "entry", "pictures", "picture" };
            AddSubTree(xml.DocumentElement, root, ctp);
            _testFiles.Copy(fileName);
            var resultFile = _testFiles.Output(fileName);
            WriteSimpleCss(resultFile, xml);
            WriteCssXml(_testFiles.Output(testName + ".xml"), xml);
            Assert.IsNull(xml.SelectSingleNode("//RULE[1]//name[text()='pictures']"));
            Assert.IsNull(xml.SelectSingleNode("//RULE[2]//name[text()='pictures']"));
            Assert.IsNotNull(xml.SelectSingleNode("//RULE[3]//name[text()='pictures']"));
            Assert.IsNotNull(xml.SelectSingleNode("//RULE[4]//name[text()='pictures']"));
        }

        /// <summary>
        ///A test for removal of pictures from rules
        ///</summary>
        [Test]
        public void ExampleCssTest()
        {
            const string testName = "exampleCss";
            var fileName = testName + ".css";
            var ctp = new CssTreeParser();
            ctp.Parse(_testFiles.Input(fileName));
            var root = ctp.Root;
            Assert.True(root != null);
            var xml = new XmlDocument();
            xml.LoadXml("<root/>");
            _uniqueClasses = new List<string> { "entry", "senses", "sense", "examples", "example", "translations", "translation" };
            AddSubTree(xml.DocumentElement, root, ctp);
            _testFiles.Copy(fileName);
            var resultFile = _testFiles.Output(fileName);
            WriteSimpleCss(resultFile, xml);
            WriteCssXml(_testFiles.Output(testName + ".xml"), xml);
            Assert.IsNull(xml.SelectSingleNode("//RULE[1]//name[text()='example']"));
            Assert.IsNotNull(xml.SelectSingleNode("//RULE[1]//name[text()='complete']"));
            Assert.IsNull(xml.SelectSingleNode("//RULE[2]//name[text()='example']"));
            Assert.IsNull(xml.SelectSingleNode("//RULE[2]//name[text()='complete']"));
            Assert.IsNotNull(xml.SelectSingleNode("//RULE[3]//name[text()='example']"));
            Assert.IsNull(xml.SelectSingleNode("//RULE[3]//name[text()='complete']"));
            Assert.IsNull(xml.SelectSingleNode("//RULE[6]//name[text()='example']"));
            Assert.IsNull(xml.SelectSingleNode("//RULE[6]//name[text()='complete']"));
        }

        /// <summary>
        ///A test for senses within senses
        ///</summary>
        [Test]
        public void SenseSenseCssTest()
        {
            const string testName = "senseSenseCss";
            var fileName = testName + ".css";
            var ctp = new CssTreeParser();
            ctp.Parse(_testFiles.Input(fileName));
            var root = ctp.Root;
            Assert.True(root != null);
            var xml = new XmlDocument();
            xml.LoadXml("<root/>");
            _uniqueClasses = new List<string> { "entry", "senses", "sense", "sensecontent", "sensenumber", "sensetype"};
            AddSubTree(xml.DocumentElement, root, ctp);
            _testFiles.Copy(fileName);
            var resultFile = _testFiles.Output(fileName);
            WriteSimpleCss(resultFile, xml);
            WriteCssXml(_testFiles.Output(testName + ".xml"), xml);
            Assert.IsNotNull(xml.SelectSingleNode("//RULE[1]/*[2]/name[text()='senses']"));
            Assert.IsNotNull(xml.SelectSingleNode("//RULE[1]/*[3]/name[text()='senses']"));
            Assert.IsNotNull(xml.SelectSingleNode("//RULE[1]/*[4][local-name()='PARENTOF']"));
            Assert.IsNotNull(xml.SelectSingleNode("//RULE[1]/*[5]/name[text()='span']"));
        }

        /// <summary>
        ///A test for subentry within subentry
        ///</summary>
        [Test]
        public void SubentryCssTest()
        {
            const string testName = "subentryCss";
            var fileName = testName + ".css";
            var ctp = new CssTreeParser();
            ctp.Parse(_testFiles.Input(fileName));
            var root = ctp.Root;
            Assert.True(root != null);
            var xml = new XmlDocument();
            xml.LoadXml("<root/>");
            _uniqueClasses = new List<string> { "entry", "subentries", "subentry" };
            AddSubTree(xml.DocumentElement, root, ctp);
            _testFiles.Copy(fileName);
            var resultFile = _testFiles.Output(fileName);
            WriteSimpleCss(resultFile, xml);
            WriteCssXml(_testFiles.Output(testName + ".xml"), xml);
            Assert.IsNotNull(xml.SelectSingleNode("//RULE[1]/*[2]/name[text()='subentries']"), "first subentries level missing");
            Assert.IsNotNull(xml.SelectSingleNode("//RULE[1]/*[3]/name[text()='subentries']"), "second subentries level missing");
            Assert.IsNotNull(xml.SelectSingleNode("//RULE[1]/*[4]/name[text()='subentry']"), "subentry under subentries missing");
            Assert.IsNotNull(xml.SelectSingleNode("//RULE[2]/*[2]/name[text()='subentries']"), "subentries context in second rule missing");
            Assert.IsNotNull(xml.SelectSingleNode("//RULE[2]/*[3]/name[text()='subentry']"), "subentry not where it would normally be in the second rule");
        }

        /// <summary>
        ///A test for subentry within subentry
        ///</summary>
        [Test]
        public void TagClassTest()
        {
            const string testName = "tagClass";
            var fileName = testName + ".css";
            var ctp = new CssTreeParser();
            ctp.Parse(_testFiles.Input(fileName));
            var root = ctp.Root;
            Assert.True(root != null);
            var xml = new XmlDocument();
            xml.LoadXml("<root/>");
            _uniqueClasses = new List<string> { "entry", "senses", "sensecontent" };
            AddSubTree(xml.DocumentElement, root, ctp);
            _testFiles.Copy(fileName);
            var resultFile = _testFiles.Output(fileName);
            WriteSimpleCss(resultFile, xml);
            WriteCssXml(_testFiles.Output(testName + ".xml"), xml);
            Assert.IsNotNull(xml.SelectSingleNode("//RULE[1]/*[2]/name[text()='senses']"), "expeted senses retained");
            Assert.IsNotNull(xml.SelectSingleNode("//RULE[1]/*[3]/name[text()='sensecontent']"), "expected sensecontent (w/o span tag)");
        }

        #endregion Tests
    }
}