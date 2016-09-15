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
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using CssSimpler;
using NUnit.Framework;
using SIL.PublishingSolution;
using FileData = BuildStep.FileData;

// ReSharper disable once CheckNamespace
namespace Test.CssSimplerTest
{
    [TestFixture]
    public class CssSimplerTest : Program
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
        ///A test removing errors from CSS
        ///</summary>
        [Test]
        public void ParseCssRemovingErrorsTest1()
        {
            const string testName = "CssErrors";
            var cssName = testName + ".css";
            _testFiles.Copy(cssName);
            var parser = new CssTreeParser();
            ParseCssRemovingErrors(parser, _testFiles.Output(cssName));
            TextFileAssert.AreEqual(_testFiles.Expected(cssName), _testFiles.Output(cssName), "Css errors not removed properly");
        }

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
            var outFile = WriteSimpleXhtml(xhtmlFullName);
			var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore, XmlResolver = new NullResolver() };
			var xhtml = new XmlDocument();
			xhtml.Load(XmlReader.Create(outFile, settings));
			var checkClass = xhtml.SelectSingleNode("//*[local-name()='body'][@class='dicBody']");
			Assert.IsNotNull(checkClass);
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
        public void AddSubTreeTest3()
        {
            const string testName = "AddSubTree3";
            var fileName = testName + ".css";
            var ctp = new CssTreeParser();
            ctp.Parse(_testFiles.Input(fileName));
            var root = ctp.Root;
            try
            {
                AddSubTree(null, root, ctp); //when an empty css file is given root == null
                Assert.AreEqual("<error: >", root.ToString(), "unexpected empty parse result" );
            }
            catch (NullReferenceException)
            {
                // Expected with some versions of Antlr
            }
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
            UniqueClasses = new List<string> {"entry"};
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
            UniqueClasses = new List<string> { "entry", "pronunciations", "pronunciation", "form" };
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
            var outFile = WriteSimpleXhtml(xhtmlFullName);
            var settings = new XmlReaderSettings {DtdProcessing = DtdProcessing.Ignore, XmlResolver = new NullResolver()};
            var xhtml = new XmlDocument();
            xhtml.Load(XmlReader.Create(outFile, settings));
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
            var outFile = WriteSimpleXhtml(xhtmlFullName);
            var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore, XmlResolver = new NullResolver() };
            var xhtml = new XmlDocument();
            xhtml.Load(XmlReader.Create(outFile, settings));
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
            var outFile = WriteSimpleXhtml(xhtmlFullName);
            var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore, XmlResolver = new NullResolver() };
            var xhtml = new XmlDocument();
            xhtml.Load(XmlReader.Create(outFile, settings));
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
            UniqueClasses = new List<string> {"entry"};
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
            UniqueClasses = new List<string> { "entry", "pictures", "picture" };
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
            UniqueClasses = new List<string> { "entry", "senses", "sense", "examples", "example", "translations", "translation" };
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

        [Test]
        public void AnyTest()
        {
            const string testName = "Any";
            var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore, XmlResolver = new NullResolver() };
            var xml = new XmlDocument();
            var inFullName = _testFiles.Input(testName + ".xml");
            xml.Load(XmlReader.Create(inFullName, settings));
            var outFullName = _testFiles.Output(testName + ".css");
            File.Copy(inFullName, outFullName);
            WriteSimpleCss(outFullName, xml);
            TextFileAssert.AreEqual(_testFiles.Expected(testName + ".css"), outFullName);
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
            UniqueClasses = new List<string> { "entry", "senses", "sense", "sensecontent", "sensenumber", "sensetype"};
            AddSubTree(xml.DocumentElement, root, ctp);
            _testFiles.Copy(fileName);
            var resultFile = _testFiles.Output(fileName);
            WriteSimpleCss(resultFile, xml);
            WriteCssXml(_testFiles.Output(testName + ".xml"), xml);
            Assert.IsNotNull(xml.SelectSingleNode("//RULE[1]/*[2][local-name()='PARENTOF']"));
            Assert.IsNotNull(xml.SelectSingleNode("//RULE[1]/*[3]/name[text()='senses']"));
            Assert.IsNotNull(xml.SelectSingleNode("//RULE[1]/*[6]/name[text()='senses']"));
            Assert.IsNotNull(xml.SelectSingleNode("//RULE[1]/*[7][local-name()='PARENTOF']"));
            Assert.IsNotNull(xml.SelectSingleNode("//RULE[1]/*[8]/name[text()='span']"));
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
            UniqueClasses = new List<string> { "entry", "subentries", "subentry" };
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
        ///A test for multiple class names
        ///</summary>
        [Test]
        public void MultiClassTest()
        {
            const string testName = "MultiClass";
            var fileName = testName + ".xhtml";
            var result = new LoadClasses(_testFiles.Input(fileName));
            Assert.AreEqual(47, result.UniqueClasses.Count);
            // subentry is not a class on its own but is listed with another class and the two names are separated with a space
            Assert.True(result.UniqueClasses.Contains("subentry"), "subentry class missing");
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
            UniqueClasses = new List<string> { "entry", "senses", "sensecontent" };
            AddSubTree(xml.DocumentElement, root, ctp);
            _testFiles.Copy(fileName);
            var resultFile = _testFiles.Output(fileName);
            WriteSimpleCss(resultFile, xml);
            WriteCssXml(_testFiles.Output(testName + ".xml"), xml);
            Assert.IsNotNull(xml.SelectSingleNode("//RULE[1]/*[3]/name[text()='senses']"), "expeted senses retained");
            Assert.IsNotNull(xml.SelectSingleNode("//RULE[1]/*[5]/name[text()='sensecontent']"), "expected sensecontent (w/o span tag)");
        }

        /// <summary>
        /// A test of moving inline styles to css
        /// </summary>
        [Test]
        [Category("SkipOnPrecise")]
        public void InlineStyleTest()
        {
            const string testName = "InlineStyle";
            var xhtmlFile = testName + ".xhtml";
            var xhtmlFullName = _testFiles.Output(xhtmlFile);
            var cssFile = testName + ".css";
            _testFiles.Copy(cssFile);
            var ilst = new MoveInlineStyles(_testFiles.Input(xhtmlFile), xhtmlFullName, _testFiles.Output(cssFile));
            Assert.IsNotNull(ilst);
			var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore, XmlResolver = new NullResolver() };
			var xr = XmlReader.Create(xhtmlFullName, settings);
            var xhtmlDoc = new XmlDocument();
            xhtmlDoc.Load(xr);
            xr.Close();
			var node1 = xhtmlDoc.SelectSingleNode("//*[contains(@class,'stxfin')]");
            Assert.IsNotNull(node1, "style node missing");
            Assert.IsNotNull(node1.Attributes, "attributes missing");
            Assert.IsNotNull(node1.Attributes["class"], "class missing");
			Assert.AreEqual("stxfintranslation", node1.Attributes["class"].InnerText);

            var styleSheet = _testFiles.Output(cssFile);
            var lc = new LoadClasses(xhtmlFullName);
            var parser = new CssTreeParser();
            var xml = new XmlDocument();
            UniqueClasses = lc.UniqueClasses;
            OutputXml = true;
            LoadCssXml(parser, styleSheet, xml);
			var node2 = xml.SelectSingleNode("//name[.='translationstxfin']/ATTRIB");
            Assert.IsNull(node2, "Missing attribute on inline translation style");
        }

        /// <summary>
        /// A test of moving inline styles to css
        /// </summary>
        [Test]
        [Category("SkipOnPrecise")]
        public void InlineStyle2Test()
        {
            const string testName = "InlineStyle2";
            var xhtmlFile = testName + ".xhtml";
            var cssFile = testName + ".css";
            _testFiles.Copy(cssFile);
            var ilst = new MoveInlineStyles(_testFiles.Input(xhtmlFile), _testFiles.Output(xhtmlFile),
                _testFiles.Output(cssFile));
            Assert.IsNotNull(ilst);
			var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore, XmlResolver = new NullResolver() };
			var xr = XmlReader.Create(_testFiles.Output(xhtmlFile), settings);
            var xhtmlDoc = new XmlDocument();
            xhtmlDoc.Load(xr);
            xr.Close();
			var node1 = xhtmlDoc.SelectSingleNode("//*[contains(@class,'stxfin')]");
            Assert.IsNotNull(node1, "style node missing");
            Assert.IsNotNull(node1.Attributes, "attributes missing");
            Assert.IsNotNull(node1.Attributes["class"], "class missing");
			var nodes = xhtmlDoc.SelectNodes("//*[contains(@class,'stxfin')]");
            Debug.Assert(nodes != null, "nodes != null");
            Assert.AreEqual(26, nodes.Count);
            var unique = new SortedSet<string>();
            foreach (XmlElement node in nodes)
            {
                unique.Add(node.Attributes["class"].InnerText);
            }
            Assert.AreEqual(2, unique.Count);
        }

        /// <summary>
        /// A test to use the css to insert pseudo content into xhtml
        /// </summary>
        [Test]
        public void BulletsTest()
        {
            const string testName = "bullets";
            _testFiles.Copy(testName + ".xhtml");
            var xhtmlFullName = _testFiles.Output(testName + ".xhtml");
            _testFiles.Copy(testName + ".css");
            var styleSheet = _testFiles.Output(testName + ".css");
            var lc = new LoadClasses(xhtmlFullName);
            var parser = new CssTreeParser();
            var xml = new XmlDocument();
            UniqueClasses = lc.UniqueClasses;
            OutputXml = true;
            LoadCssXml(parser, styleSheet, xml);
            WriteSimpleCss(styleSheet, xml); //reloads xml with simplified version
            var tmpXhtmlFullName = WriteSimpleXhtml(xhtmlFullName);
            var tmp2Out = Path.GetTempFileName();
            // ReSharper disable once UnusedVariable
            var inlineStyle = new MoveInlineStyles(tmpXhtmlFullName, tmp2Out, styleSheet);
            xml.RemoveAll();
            UniqueClasses = null;
            LoadCssXml(parser, styleSheet, xml);
            // ReSharper disable once UnusedVariable
            var ps = new ProcessPseudo(tmp2Out, xhtmlFullName, xml, NeedHigher);
            RemoveCssPseudo(styleSheet, xml);
            try
            {
                File.Delete(tmpXhtmlFullName);
                File.Delete(tmp2Out);
            }
            catch
            {
                // ignored
            }
            RemoveCssPseudo(_testFiles.Output(testName + ".css"), xml);
            NodeTest(xhtmlFullName, 3, "//*[local-name()='a']/preceding-sibling::*", "Wrong number of bullets in output");
            NodeTest(xhtmlFullName, 14, "//*[@xml:space]", "Wrong number of list punctuation");
        }

        /// <summary>
        /// A test to use the css to insert pseudo content into xhtml
        /// </summary>
        [Test]
        public void PseudoComplexSubentryTest()
        {
            const string testName = "PseudoComplexSubentry";
            _testFiles.Copy(testName + ".xhtml");
            var xhtmlFullName = _testFiles.Output(testName + ".xhtml");
            _testFiles.Copy(testName + ".css");
            var styleSheet = _testFiles.Output(testName + ".css");
            var lc = new LoadClasses(xhtmlFullName);
            var parser = new CssTreeParser();
            var xml = new XmlDocument();
            UniqueClasses = lc.UniqueClasses;
            OutputXml = true;
            LoadCssXml(parser, styleSheet, xml);
            //WriteSimpleCss(styleSheet, xml); //reloads xml with simplified version
            var tmpXhtmlFullName = WriteSimpleXhtml(xhtmlFullName);
            var tmp2Out = Path.GetTempFileName();
            // ReSharper disable once UnusedVariable
            var inlineStyle = new MoveInlineStyles(tmpXhtmlFullName, tmp2Out, styleSheet);
            xml.RemoveAll();
            UniqueClasses = null;
            LoadCssXml(parser, styleSheet, xml);
            // ReSharper disable once UnusedVariable
            var ps = new ProcessPseudo(tmp2Out, xhtmlFullName, xml, NeedHigher);
            RemoveCssPseudo(styleSheet, xml);
            try
            {
                File.Delete(tmpXhtmlFullName);
                File.Delete(tmp2Out);
            }
            catch
            {
                // ignored
            }
            RemoveCssPseudo(_testFiles.Output(testName + ".css"), xml);
            NodeInspect(xhtmlFullName, new Dictionary<string, string> { { "(//*[@class='custentry'])[1]/*[1]", "G{" }, { "(//*[@class='custentry'])[2]/*[1]", "I{" }, { "(//*[@class='custentry'])[3]/*[1]", "J{" } });
        }

        /// <summary>
        /// A test to use the css to insert pseudo content into xhtml
        /// </summary>
        [Test]
        public void PseudoHeadwordRefsTest()
        {
            const string testName = "PseudoHeadwordRefs";
            _testFiles.Copy(testName + ".xhtml");
            var xhtmlFullName = _testFiles.Output(testName + ".xhtml");
            _testFiles.Copy(testName + ".css");
            var styleSheet = _testFiles.Output(testName + ".css");
            var lc = new LoadClasses(xhtmlFullName);
            var parser = new CssTreeParser();
            var xml = new XmlDocument();
            UniqueClasses = lc.UniqueClasses;
            OutputXml = true;
            LoadCssXml(parser, styleSheet, xml);
            //WriteSimpleCss(styleSheet, xml); //reloads xml with simplified version
            var tmpXhtmlFullName = WriteSimpleXhtml(xhtmlFullName);
            var tmp2Out = Path.GetTempFileName();
            // ReSharper disable once UnusedVariable
            var inlineStyle = new MoveInlineStyles(tmpXhtmlFullName, tmp2Out, styleSheet);
            xml.RemoveAll();
            UniqueClasses = null;
            LoadCssXml(parser, styleSheet, xml);
            // ReSharper disable once UnusedVariable
            var ps = new ProcessPseudo(tmp2Out, xhtmlFullName, xml, NeedHigher);
            RemoveCssPseudo(styleSheet, xml);
            try
            {
                File.Delete(tmpXhtmlFullName);
                File.Delete(tmp2Out);
            }
            catch
            {
                // ignored
            }
            RemoveCssPseudo(_testFiles.Output(testName + ".css"), xml);
            NodeInspect(xhtmlFullName, new Dictionary<string, string> { { "(//*[@class='custentry'])[7]/*[1]", "F(" }, { "(//*[@class='custentry'])[8]/*[1]", "C(" } });
        }

        /// <summary>
        /// A test for bulleted paragraphs that use the :not() css property
        /// </summary>
        [Test]
        public void BulletParagraphsTest()
        {
            const string testName = "BulletParagraphs";
            _testFiles.Copy(testName + ".xhtml");
            var xhtmlFullName = _testFiles.Output(testName + ".xhtml");
            _testFiles.Copy(testName + ".css");
            var styleSheet = _testFiles.Output(testName + ".css");
            var lc = new LoadClasses(xhtmlFullName);
            var parser = new CssTreeParser();
            var xml = new XmlDocument();
            UniqueClasses = lc.UniqueClasses;
            OutputXml = true;
            LoadCssXml(parser, styleSheet, xml);
            WriteSimpleCss(styleSheet, xml); //reloads xml with simplified version
            var tmpXhtmlFullName = WriteSimpleXhtml(xhtmlFullName);
            var tmp2Out = Path.GetTempFileName();
            // ReSharper disable once UnusedVariable
            var inlineStyle = new MoveInlineStyles(tmpXhtmlFullName, tmp2Out, styleSheet);
            xml.RemoveAll();
            UniqueClasses = null;
            LoadCssXml(parser, styleSheet, xml);
            // ReSharper disable once UnusedVariable
            var ps = new ProcessPseudo(tmp2Out, xhtmlFullName, xml, NeedHigher);
            RemoveCssPseudo(styleSheet, xml);
            try
            {
                File.Delete(tmpXhtmlFullName);
                File.Delete(tmp2Out);
            }
            catch
            {
                // ignored
            }
            RemoveCssPseudo(_testFiles.Output(testName + ".css"), xml);
            //NodeInspect(xhtmlFullName, new Dictionary<string, string> { { "(//*[@class='custentry-ps'])[19]", "F(" }, { "(//*[@class='custentry-ps'])[22]", "C(" } });
        }

        /// <summary>
        /// A test to use the css to insert pseudo content into xhtml
        /// </summary>
        [Test]
        public void Bke828Test()
        {
            const string testName = "bke828";
            var cssFullName = _testFiles.Input(testName + ".css");
            var xhtmlFullName = _testFiles.Input(testName + ".xhtml");
            var outFullName = _testFiles.Output(testName + ".xhtml");
            var ctp = new CssTreeParser();
            ctp.Parse(cssFullName);
            var root = ctp.Root;
            Assert.True(root != null);
            var xml = new XmlDocument();
            xml.LoadXml("<root/>");
            var lc = new LoadClasses(xhtmlFullName);
            UniqueClasses = lc.UniqueClasses;
            AddSubTree(xml.DocumentElement, root, ctp);
            _testFiles.Copy(testName + ".css");
            WriteSimpleCss(_testFiles.Output(testName + ".css"), xml);
            WriteCssXml(_testFiles.Output(testName + ".xml"), xml);
            // ReSharper disable once UnusedVariable
            var ps = new ProcessPseudo(xhtmlFullName, outFullName, xml, NeedHigher);
            RemoveCssPseudo(_testFiles.Output(testName + ".css"), xml);
            NodeTest(outFullName, 3023, "//*[@xml:space]", "Nodes with pseudo content changed for Fw 8.2.8");
        }

        /// <summary>
        /// A test to use the css to insert pseudo content into xhtml
        /// </summary>
        [Test]
        public void PseudoProcessTest()
        {
            const string testName = "PseudoProcess";
            var cssFullName = _testFiles.Input(testName + ".css");
            var xhtmlFullName = _testFiles.Input(testName + ".xhtml");
            var outFullName = _testFiles.Output(testName + ".xhtml");
            var ctp = new CssTreeParser();
            ctp.Parse(cssFullName);
            var root = ctp.Root;
            Assert.True(root != null);
            var xml = new XmlDocument();
            xml.LoadXml("<root/>");
            var lc = new LoadClasses(xhtmlFullName);
            UniqueClasses = lc.UniqueClasses;
            AddSubTree(xml.DocumentElement, root, ctp);
            _testFiles.Copy(testName + ".css");
            WriteSimpleCss(_testFiles.Output(testName + ".css"), xml);
            WriteCssXml(_testFiles.Output(testName + ".xml"), xml);
            // ReSharper disable once UnusedVariable
            var ps = new ProcessPseudo(xhtmlFullName, outFullName, xml, NeedHigher);
            RemoveCssPseudo(_testFiles.Output(testName + ".css"), xml);
        }

        /// <summary>
        /// A test to see if lexsensereferences > span + span:before works correctly
        /// </summary>
        [Test]
        public void PseudoMultiRelTest()
        {
            const string testName = "PseudoMultiRel";
            var cssFullName = _testFiles.Input(testName + ".css");
            var xhtmlFullName = _testFiles.Input(testName + ".xhtml");
            var outFullName = _testFiles.Output(testName + ".xhtml");
            var ctp = new CssTreeParser();
            ctp.Parse(cssFullName);
            var root = ctp.Root;
            Assert.True(root != null);
            var xml = new XmlDocument();
            xml.LoadXml("<root/>");
            var lc = new LoadClasses(xhtmlFullName);
            UniqueClasses = lc.UniqueClasses;
            AddSubTree(xml.DocumentElement, root, ctp);
            _testFiles.Copy(testName + ".css");
            //WriteSimpleCss(_testFiles.Output(testName + ".css"), xml);
            WriteCssXml(_testFiles.Output(testName + ".xml"), xml);
            // ReSharper disable once UnusedVariable
            var ps = new ProcessPseudo(xhtmlFullName, outFullName, xml, NeedHigher);
            RemoveCssPseudo(_testFiles.Output(testName + ".css"), xml);
            NodeTest(outFullName, 1, "//*[@class='ownertype_abbreviation']/preceding-sibling::*", "node with ; not inserted between lexical relations");
        }

        /// <summary>
        /// A test for punctuation after sense numbers
        /// </summary>
        [Test]
        public void PseudoSenseNumberTest()
        {
            const string testName = "PseudoSenseNumber";
            var cssFullName = _testFiles.Input(testName + ".css");
            var xhtmlFullName = _testFiles.Input(testName + ".xhtml");
            var outFullName = _testFiles.Output(testName + ".xhtml");
            var ctp = new CssTreeParser();
            ctp.Parse(cssFullName);
            var root = ctp.Root;
            Assert.True(root != null);
            var xml = new XmlDocument();
            xml.LoadXml("<root/>");
            var lc = new LoadClasses(xhtmlFullName);
            UniqueClasses = lc.UniqueClasses;
            AddSubTree(xml.DocumentElement, root, ctp);
            _testFiles.Copy(testName + ".css");
            WriteSimpleCss(_testFiles.Output(testName + ".css"), xml);
            WriteCssXml(_testFiles.Output(testName + ".xml"), xml);
            // ReSharper disable once UnusedVariable
            var ps = new ProcessPseudo(xhtmlFullName, outFullName, xml, NeedHigher);
            RemoveCssPseudo(_testFiles.Output(testName + ".css"), xml);
            NodeTest(outFullName, 2, "//*[@class='sensenumber']/*", "Wrong amount of sense number punctuation.");
        }

        /// <summary>
        /// A test for punctuation in between list elements
        /// </summary>
        [Test]
        public void PseudoBetweenTest()
        {
            const string testName = "PseudoBetween";
            var cssFullName = _testFiles.Input(testName + ".css");
            var xhtmlFullName = _testFiles.Input(testName + ".xhtml");
            var outFullName = _testFiles.Output(testName + ".xhtml");
            var ctp = new CssTreeParser();
            ctp.Parse(cssFullName);
            var root = ctp.Root;
            Assert.True(root != null);
            var xml = new XmlDocument();
            xml.LoadXml("<root/>");
            var lc = new LoadClasses(xhtmlFullName);
            UniqueClasses = lc.UniqueClasses;
            AddSubTree(xml.DocumentElement, root, ctp);
            _testFiles.Copy(testName + ".css");
            //WriteSimpleCss(_testFiles.Output(testName + ".css"), xml);
            WriteCssXml(_testFiles.Output(testName + ".xml"), xml);
            // ReSharper disable once UnusedVariable
            var ps = new ProcessPseudo(xhtmlFullName, outFullName, xml, NeedHigher);
            RemoveCssPseudo(_testFiles.Output(testName + ".css"), xml);
            TextFileAssert.AreEqual(_testFiles.Expected(testName + ".css"), _testFiles.Output(testName + ".css"));
            NodeTest(outFullName, 9, "//*[@class='headword']/preceding-sibling::*", "missing commas between lexical relation headwords");
        }

        /// <summary>
        /// A test for punctuation in between list elements
        /// </summary>
        [Test]
        public void PseudoSubentryTest()
        {
            const string testName = "PseudoSubentry";
            var cssFullName = _testFiles.Input(testName + ".css");
            var xhtmlFullName = _testFiles.Input(testName + ".xhtml");
            var outFullName = _testFiles.Output(testName + ".xhtml");
            var ctp = new CssTreeParser();
            ctp.Parse(cssFullName);
            var root = ctp.Root;
            Assert.True(root != null);
            var xml = new XmlDocument();
            xml.LoadXml("<root/>");
            var lc = new LoadClasses(xhtmlFullName);
            UniqueClasses = lc.UniqueClasses;
            AddSubTree(xml.DocumentElement, root, ctp);
            _testFiles.Copy(testName + ".css");
            //WriteSimpleCss(_testFiles.Output(testName + ".css"), xml);
            WriteCssXml(_testFiles.Output(testName + ".xml"), xml);
            // ReSharper disable once UnusedVariable
            var ps = new ProcessPseudo(xhtmlFullName, outFullName, xml, NeedHigher);
            RemoveCssPseudo(_testFiles.Output(testName + ".css"), xml);
            TextFileAssert.AreEqual(_testFiles.Expected(testName + ".css"), _testFiles.Output(testName + ".css"));
            NodeTest(outFullName, 81, "//*[@xml:space]", "subentry punctuation");
        }

        /// <summary>
        /// Cleans up bad rule in CSS export.
        /// </summary>
        [Test]
        public void PseudoBuangKde2Test()
        {
            const string testName = "BuangKde2";
            _testFiles.Copy(testName + ".css");
            var cssFullName = _testFiles.Output(testName + ".css");
            var xhtmlFullName = _testFiles.Input(testName + ".xhtml");
            var outFullName = _testFiles.Output(testName + ".xhtml");
            var ctp = new CssTreeParser();
            ctp.Parse(cssFullName);
            var root = ctp.Root;
            Assert.True(root != null);
            var xml = new XmlDocument();
            xml.LoadXml("<root/>");
            var lc = new LoadClasses(xhtmlFullName);
            UniqueClasses = lc.UniqueClasses;
            AddSubTree(xml.DocumentElement, root, ctp);
            _testFiles.Copy(testName + ".css");
            //WriteSimpleCss(_testFiles.Output(testName + ".css"), xml);
            WriteCssXml(_testFiles.Output(testName + ".xml"), xml);
            // ReSharper disable once UnusedVariable
            var ps = new ProcessPseudo(xhtmlFullName, outFullName, xml, NeedHigher);
            RemoveCssPseudo(_testFiles.Output(testName + ".css"), xml);
            TextFileAssert.AreEqual(_testFiles.Expected(testName + ".css"), _testFiles.Output(testName + ".css"));
            NodeTest(outFullName, 14, "//*[@class='semanticdomains']/*[@xml:space]", "semantic domain punctuation");
        }

        /// <summary>
        /// Remove extra parenthesis in Semantic domaain before abbreviation
        /// </summary>
        [Test]
        public void PseudoExtraSemDomParenTest()
        {
            const string testName = "PseudoExtraSemDomParen";
            _testFiles.Copy(testName + ".css");
            var cssFullName = _testFiles.Output(testName + ".css");
            var xhtmlFullName = _testFiles.Input(testName + ".xhtml");
            var outFullName = _testFiles.Output(testName + ".xhtml");
            var ctp = new CssTreeParser();
            ctp.Parse(cssFullName);
            var root = ctp.Root;
            Assert.True(root != null);
            var xml = new XmlDocument();
            xml.LoadXml("<root/>");
            var lc = new LoadClasses(xhtmlFullName);
            UniqueClasses = lc.UniqueClasses;
            AddSubTree(xml.DocumentElement, root, ctp);
            _testFiles.Copy(testName + ".css");
            WriteSimpleCss(_testFiles.Output(testName + ".css"), xml);
            WriteCssXml(_testFiles.Output(testName + ".xml"), xml);
            // ReSharper disable once UnusedVariable
            var ps = new ProcessPseudo(xhtmlFullName, outFullName, xml, NeedHigher);
            RemoveCssPseudo(_testFiles.Output(testName + ".css"), xml);
            TextFileAssert.AreEqual(_testFiles.Expected(testName + ".css"), _testFiles.Output(testName + ".css"));
            XmlAssert.AreEqual(_testFiles.Expected(testName + ".xhtml"), _testFiles.Output(testName + ".xhtml"), "Xhtml file not converted as expected");
        }

        /// <summary>
        /// Remove extra parenthesis in Semantic domaain before abbreviation
        /// </summary>
        [Test]
        public void PseudoRev1Test()
        {
            const string testName = "PseudoRev1";
            _testFiles.Copy(testName + ".css");
            var cssFullName = _testFiles.Output(testName + ".css");
            var xhtmlFullName = _testFiles.Input(testName + ".xhtml");
            var outFullName = _testFiles.Output(testName + ".xhtml");
            var ctp = new CssTreeParser();
            ctp.Parse(cssFullName);
            var root = ctp.Root;
            Assert.True(root != null);
            var xml = new XmlDocument();
            xml.LoadXml("<root/>");
            var lc = new LoadClasses(xhtmlFullName);
            UniqueClasses = lc.UniqueClasses;
            AddSubTree(xml.DocumentElement, root, ctp);
            _testFiles.Copy(testName + ".css");
            //WriteSimpleCss(_testFiles.Output(testName + ".css"), xml);
            WriteCssXml(_testFiles.Output(testName + ".xml"), xml);
            // ReSharper disable once UnusedVariable
            var ps = new ProcessPseudo(xhtmlFullName, outFullName, xml, NeedHigher);
            RemoveCssPseudo(_testFiles.Output(testName + ".css"), xml);
            TextFileAssert.AreEqual(_testFiles.Expected(testName + ".css"), _testFiles.Output(testName + ".css"));
            NodeTest(outFullName, 7, "//*[@class='semanticdomain']/*[@class='abbreviation']/preceding-sibling::*", "multiple semantic domain punctuation");
        }

        /// <summary>
        /// Remove extra parenthesis in Semantic domaain before abbreviation
        /// </summary>
        [Test]
        public void WritingSystemAbbrTest()
        {
            const string testName = "WritingSystemAbbr";
            _testFiles.Copy(testName + ".css");
            var cssFullName = _testFiles.Output(testName + ".css");
            var xhtmlFullName = _testFiles.Input(testName + ".xhtml");
            var outFullName = _testFiles.Output(testName + ".xhtml");
            var ctp = new CssTreeParser();
            var xml = new XmlDocument();
            var lc = new LoadClasses(xhtmlFullName);
            UniqueClasses = lc.UniqueClasses;
            ctp.Parse(cssFullName);
            _testFiles.Copy(testName + ".css");
            LoadCssXml(ctp, cssFullName, xml);
            //WriteSimpleCss(_testFiles.Output(testName + ".css"), xml);
            WriteCssXml(_testFiles.Output(testName + ".xml"), xml);
            // ReSharper disable once UnusedVariable
            var ps = new ProcessPseudo(xhtmlFullName, outFullName, xml, NeedHigher);
            RemoveCssPseudo(_testFiles.Output(testName + ".css"), xml);
            TextFileAssert.AreEqual(_testFiles.Expected(testName + ".css"), _testFiles.Output(testName + ".css"));
            NodeTest(outFullName, 129, "//*[@xml:space]", "semantic domain punctuation");
        }

        /// <summary>
        /// Remove extra parenthesis in Semantic domaain before abbreviation
        /// </summary>
        [Test]
        public void MultiIndentTest()
        {
            const string testName = "MultiIndent";
            _testFiles.Copy(testName + ".css");
            var cssFullName = _testFiles.Output(testName + ".css");
            var xhtmlFullName = _testFiles.Input(testName + ".xhtml");
            var outFullName = _testFiles.Output(testName + ".xhtml");
            var ctp = new CssTreeParser();
            var xml = new XmlDocument();
            var lc = new LoadClasses(xhtmlFullName);
            UniqueClasses = lc.UniqueClasses;
            ctp.Parse(cssFullName);
            _testFiles.Copy(testName + ".css");
            LoadCssXml(ctp, cssFullName, xml);
            WriteCssXml(_testFiles.Output(testName + ".xml"), xml);
            // ReSharper disable once UnusedVariable
            var ps = new ProcessPseudo(xhtmlFullName, outFullName, xml, NeedHigher);
            RemoveCssPseudo(_testFiles.Output(testName + ".css"), xml);
            WriteSimpleCss(_testFiles.Output(testName + ".css"), xml);
            TextFileAssert.AreEqual(_testFiles.Expected(testName + ".css"), _testFiles.Output(testName + ".css"));
        }

        /// <summary>
        /// Remove extra parenthesis in Semantic domaain before abbreviation
        /// </summary>
        [Test]
        public void SpanClassTest()
        {
            const string testName = "SpanClass";
            _testFiles.Copy(testName + ".css");
            var cssFullName = _testFiles.Output(testName + ".css");
            var xhtmlFullName = _testFiles.Input(testName + ".xhtml");
            var outFullName = _testFiles.Output(testName + ".xhtml");
            var ctp = new CssTreeParser();
            var xml = new XmlDocument();
            var lc = new LoadClasses(xhtmlFullName);
            UniqueClasses = lc.UniqueClasses;
            ctp.Parse(cssFullName);
            _testFiles.Copy(testName + ".css");
            LoadCssXml(ctp, cssFullName, xml);
            WriteCssXml(_testFiles.Output(testName + ".xml"), xml);
            // ReSharper disable once UnusedVariable
            var ps = new ProcessPseudo(xhtmlFullName, outFullName, xml, NeedHigher);
            RemoveCssPseudo(_testFiles.Output(testName + ".css"), xml);
            WriteSimpleCss(_testFiles.Output(testName + ".css"), xml);
            NodeTest(outFullName, 1, "//*[@class='sensecontent']/*[@xml:space]", "comma between sense content");
        }

        /// <summary>
        /// test for multi selector expansion
        /// </summary>
        [Test]
        public void ElaborateMultiSelectorRulesTest()
        {
            const string testName = "MultiSelector";
            _testFiles.Copy(testName + ".css");
            var cssFullName = _testFiles.Output(testName + ".css");
            var xhtmlFullName = _testFiles.Input(testName + ".xhtml");
            var ctp = new CssTreeParser();
            var xml = new XmlDocument();
            var lc = new LoadClasses(xhtmlFullName);
            UniqueClasses = lc.UniqueClasses;
            ctp.Parse(cssFullName);
            var r = ctp.Root;
            xml.LoadXml("<ROOT/>");
            AddSubTree(xml.DocumentElement, r, ctp);
            WriteCssXml(cssFullName, xml);
            var xmlFullName = _testFiles.Output(testName + ".xml");
            File.Copy(xmlFullName, _testFiles.Output(testName + "Input.xml"));
            ElaborateMultiSelectorRules(xml);
            WriteCssXml(cssFullName, xml);
            var cssFile = new FileStream(cssFullName, FileMode.Create);
            var cssWriter = XmlWriter.Create(cssFile, XmlCss.OutputSettings);
            XmlCss.Transform(xml, null, cssWriter);
            cssFile.Close();
            TextFileAssert.AreEqual(_testFiles.Expected(testName + ".css"), cssFullName);
        }

        /// <summary>
        /// test for multi selector expansion
        /// </summary>
        [Test]
        public void ElaborateMultiSelectorRules2Test()
        {
            const string testName = "MultiSelector2";
            _testFiles.Copy(testName + ".css");
            var cssFullName = _testFiles.Output(testName + ".css");
            var xhtmlFullName = _testFiles.Input(testName + ".xhtml");
            var ctp = new CssTreeParser();
            var xml = new XmlDocument();
            var lc = new LoadClasses(xhtmlFullName);
            UniqueClasses = lc.UniqueClasses;
            ctp.Parse(cssFullName);
            var r = ctp.Root;
            xml.LoadXml("<ROOT/>");
            AddSubTree(xml.DocumentElement, r, ctp);
            WriteCssXml(cssFullName, xml);
            var xmlFullName = _testFiles.Output(testName + ".xml");
            File.Copy(xmlFullName, _testFiles.Output(testName + "Input.xml"));
            ElaborateMultiSelectorRules(xml);
            WriteCssXml(cssFullName, xml);
            var cssFile = new FileStream(cssFullName, FileMode.Create);
            var cssWriter = XmlWriter.Create(cssFile, XmlCss.OutputSettings);
            XmlCss.Transform(xml, null, cssWriter);
            cssFile.Close();
            TextFileAssert.AreEqual(_testFiles.Expected(testName + ".css"), cssFullName);
        }

        /// <summary>
        /// Remove extra parenthesis in Semantic domaain before abbreviation
        /// </summary>
        [Test]
        public void AudioTest()
        {
            const string testName = "Audio";
            _testFiles.Copy(testName + ".css");
            var cssFullName = _testFiles.Output(testName + ".css");
            var xhtmlFullName = _testFiles.Input(testName + ".xhtml");
            var outFullName = _testFiles.Output(testName + ".xhtml");
            var ctp = new CssTreeParser();
            ctp.Parse(cssFullName);
            var root = ctp.Root;
            Assert.True(root != null);
            var xml = new XmlDocument();
            xml.LoadXml("<root/>");
            var lc = new LoadClasses(xhtmlFullName);
            UniqueClasses = lc.UniqueClasses;
            AddSubTree(xml.DocumentElement, root, ctp);
            _testFiles.Copy(testName + ".css");
            WriteSimpleCss(_testFiles.Output(testName + ".css"), xml);
            WriteCssXml(_testFiles.Output(testName + ".xml"), xml);
            // ReSharper disable once UnusedVariable
            var ps = new ProcessPseudo(xhtmlFullName, outFullName, xml, NeedHigher);
            RemoveCssPseudo(_testFiles.Output(testName + ".css"), xml);
            NodeTest(outFullName, 1, "//*[@class='fr-Zxxx-x-audio']/*[string-length(.)=2]", "audio icon");
        }

        /// <summary>
        /// Remove extra parenthesis in Semantic domaain before abbreviation
        /// </summary>
        [Test]
        public void Audio2Test()
        {
            const string testName = "Audio2";
            _testFiles.Copy(testName + ".css");
            var cssFullName = _testFiles.Output(testName + ".css");
            var xhtmlFullName = _testFiles.Input(testName + ".xhtml");
            var outFullName = _testFiles.Output(testName + ".xhtml");
            var ctp = new CssTreeParser();
            ctp.Parse(cssFullName);
            var root = ctp.Root;
            Assert.True(root != null);
            var xml = new XmlDocument();
            xml.LoadXml("<root/>");
            var lc = new LoadClasses(xhtmlFullName);
            UniqueClasses = lc.UniqueClasses;
            AddSubTree(xml.DocumentElement, root, ctp);
            _testFiles.Copy(testName + ".css");
            WriteSimpleCss(_testFiles.Output(testName + ".css"), xml);
            WriteCssXml(_testFiles.Output(testName + ".xml"), xml);
            // ReSharper disable once UnusedVariable
            var ps = new ProcessPseudo(xhtmlFullName, outFullName, xml, NeedHigher);
            RemoveCssPseudo(_testFiles.Output(testName + ".css"), xml);
            NodeTest(outFullName, 1, "//*[@class='seh-Zxxx-x-audio']/*[string-length(.)=2]", "audio icon");
        }

        /// <summary>
        /// Remove extra parenthesis in Semantic domaain before abbreviation
        /// </summary>
        [Test]
        public void SubentryBulletTest()
        {
            const string testName = "SubentryBullet";
            _testFiles.Copy(testName + ".css");
            var cssFullName = _testFiles.Output(testName + ".css");
            var xhtmlFullName = _testFiles.Input(testName + ".xhtml");
            var outFullName = _testFiles.Output(testName + ".xhtml");
            var ctp = new CssTreeParser();
            ctp.Parse(cssFullName);
            var root = ctp.Root;
            Assert.True(root != null);
            var xml = new XmlDocument();
            xml.LoadXml("<root/>");
            var lc = new LoadClasses(xhtmlFullName);
            UniqueClasses = lc.UniqueClasses;
            AddSubTree(xml.DocumentElement, root, ctp);
            _testFiles.Copy(testName + ".css");
            WriteSimpleCss(_testFiles.Output(testName + ".css"), xml);
            WriteCssXml(_testFiles.Output(testName + ".xml"), xml);
            // ReSharper disable once UnusedVariable
            var ps = new ProcessPseudo(xhtmlFullName, outFullName, xml, NeedHigher);
            RemoveCssPseudo(_testFiles.Output(testName + ".css"), xml);
            NodeTest(outFullName, 1, "//*[contains(@class,'subentry ')]/*[1][.='\x29EB']", "subentry bullet");
        }

        /// <summary>
        /// Remove extra parenthesis in Semantic domaain before abbreviation
        /// </summary>
        [Test]
        public void BulletsAndIndentsTest()
        {
            const string testName = "TD4596";
            _testFiles.Copy(testName + ".css");
            var cssFullName = _testFiles.Output(testName + ".css");
            var xhtmlFullName = _testFiles.Input(testName + ".xhtml");
            var outFullName = _testFiles.Output(testName + ".xhtml");
            var ctp = new CssTreeParser();
            ctp.Parse(cssFullName);
            var root = ctp.Root;
            Assert.True(root != null);
            var xml = new XmlDocument();
            xml.LoadXml("<root/>");
            var lc = new LoadClasses(xhtmlFullName);
            UniqueClasses = lc.UniqueClasses;
            AddSubTree(xml.DocumentElement, root, ctp);
            _testFiles.Copy(testName + ".css");
            //WriteSimpleCss(_testFiles.Output(testName + ".css"), xml);
            WriteCssXml(_testFiles.Output(testName + ".xml"), xml);
            // ReSharper disable once UnusedVariable
            var ps = new ProcessPseudo(xhtmlFullName, outFullName, xml, NeedHigher);
            RemoveCssPseudo(_testFiles.Output(testName + ".css"), xml);
            TextFileAssert.AreEqual(_testFiles.Expected(testName + ".css"), _testFiles.Output(testName + ".css"));
            NodeTest(outFullName, 2, "//*[@class='examplescontent-ps']", "examplebullet");
        }

        [Test]
        public void WriteValue1Test()
        {
            const string testName = "Value1" + ".xml";
            // ReSharper disable once UnusedVariable
            var value = new ValueTest(_testFiles.Input(testName), _testFiles.Output(testName), @"\2022");
            TextFileAssert.AreEqual(_testFiles.Expected(testName), _testFiles.Output(testName), "write bullet test");
        }

        [Test]
        public void WriteValue2Test()
        {
            const string testName = "Value2" + ".xml";
            // ReSharper disable once UnusedVariable
            var value = new ValueTest(_testFiles.Input(testName), _testFiles.Output(testName), @"Try: \2022 and \34");
            TextFileAssert.AreEqual(_testFiles.Expected(testName), _testFiles.Output(testName), "write multiple bullet test");
        }

        [Test]
        public void WriteValue3Test()
        {
            const string testName = "Value3" + ".xml";
            // ReSharper disable once UnusedVariable
            var value = new ValueTest(_testFiles.Input(testName), _testFiles.Output(testName), @"Try: \2022 and \34.");
            TextFileAssert.AreEqual(_testFiles.Expected(testName), _testFiles.Output(testName), "write multiple Unicode with text after");
        }

        /// <summary>
        /// Method to load an XML (XHTML) file and search for the inserted nodes and complain if count isn't right.
        /// </summary>
        /// <param name="outFullName">File to search</param>
        /// <param name="count">Expected number of hits</param>
        /// <param name="xpath">node to search for</param>
        /// <param name="msg">message to display on mismatch</param>
        private static void NodeTest(string outFullName, int count, string xpath, string msg)
        {
            var xr = XmlReader.Create(outFullName,
                new XmlReaderSettings { XmlResolver = null, DtdProcessing = DtdProcessing.Ignore });
            var xDoc = new XmlDocument();
            xDoc.Load(xr);
            xr.Close();
            var ns = new XmlNamespaceManager(xDoc.NameTable);
            ns.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            ns.AddNamespace("xml", "http://www.w3.org/XML/1998/namespace");
            // ReSharper disable once PossibleNullReferenceException
            Assert.AreEqual(count, xDoc.SelectNodes(xpath, ns).Count, msg);
        }

        /// <summary>
        /// Method to load an XML (XHTML) check various xpaths for their values.
        /// </summary>
        private static void NodeInspect(string outFullName, Dictionary<string, string> nodeValues )
        {
            var xr = XmlReader.Create(outFullName,
                new XmlReaderSettings { XmlResolver = null, DtdProcessing = DtdProcessing.Ignore });
            var xDoc = new XmlDocument();
            xDoc.Load(xr);
            xr.Close();
            foreach (var xpath in nodeValues.Keys)
            {
                var node = xDoc.SelectSingleNode(xpath);
                Assert.IsNotNull(node, "Missing " + xpath);
                Assert.AreEqual(nodeValues[xpath], node.InnerText);
            }
        }

        /// <summary>
        ///A test for ValidateXhtml
        /// https://support.microsoft.com/en-us/kb/307379
        /// https://msdn.microsoft.com/en-us/library/system.xml.xmlurlresolver%28v=vs.110%29.aspx
        /// http://stackoverflow.com/questions/470313/net-how-to-validate-xml-file-with-dtd-without-doctype-declaration
        ///</summary>
        [Test]
        public void ValidateTest()
        {
            const string testName = "Validate";
            var fileName = testName + ".xhtml";
            _testFiles.Copy(fileName);
            string xhtmlFullName = _testFiles.Output(fileName);
            var outFile = WriteSimpleXhtml(xhtmlFullName);
            var resolver = new MyUrlResolver();
            var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Parse, ValidationType = ValidationType.DTD, XmlResolver = resolver};
            settings.ValidationEventHandler += delegate(object sender, ValidationEventArgs args)
            {
                throw new XmlSchemaValidationException(args.Message);
            };
            var reader = XmlReader.Create(outFile, settings);
            while (reader.Read()) { }
        }

        /// <summary>
        ///A test for ValidateXhtml
        ///</summary>
        [Test]
        public void ValidateFailTest()
        {
            const string testName = "ValidateFail";
            var fileName = testName + ".xhtml";
            _testFiles.Copy(fileName);
            string xhtmlFullName = _testFiles.Output(fileName);
            //WriteSimpleXhtml(xhtmlFullName);
            var resolver = new MyUrlResolver();
            var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Parse, ValidationType = ValidationType.DTD, XmlResolver = resolver };
            var validErrorCount = 0;
            settings.ValidationEventHandler += delegate
            {
                validErrorCount += 1;
            };
            var reader = XmlReader.Create(xhtmlFullName, settings);
			try
			{
				while (reader.Read()) { }
				// Windows will count errors and report the results.
				Assert.AreEqual(33, validErrorCount, "The number of DTD errors reported by C# has changed");
			}
			catch (XmlSchemaException e) // Linux will throw an error on the first validation issue rather than counting.
			{
				Assert.AreEqual (1, e.LineNumber);
			}
        }

        #endregion Tests
    }

    public class MyUrlResolver : XmlUrlResolver
    {
        public override Uri ResolveUri(Uri baseUri, string relativeUri)
        {
            if (relativeUri.StartsWith("-//"))
            {
                return new Uri("file://" + relativeUri);
            }
            return base.ResolveUri(baseUri, relativeUri);
        }

        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
			const string xhtml11Path = "http://www.w3.org/";
			var lookup = absoluteUri.OriginalString;
			if (lookup.Contains (xhtml11Path)) {
				lookup = absoluteUri.Segments [absoluteUri.Segments.Length - 1];
			}
			switch (lookup)
            {
				case "xhtml11.dtd":
                case "file://-//W3C//DTD XHTML 1.1//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml11.dtd");
				case "xhtml-inlstyle-1.mod":
                case "file://-//W3C//ELEMENTS XHTML Inline Style 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-inlstyle-1.mod");
                case "xhtml11-model-1.mod":
				case "file://-//W3C//ENTITIES XHTML 1.1 Document Model 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml11-model-1.mod");
                case "xhtml-datatypes-1.mod":
				case "file://-//W3C//ENTITIES XHTML Datatypes 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-datatypes-1.mod");
                case "xhtml-framework-1.mod": 
				case "file://-//W3C//ENTITIES XHTML Modular Framework 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-framework-1.mod");
                case "xhtml-qname-1.mod":
				case "file://-//W3C//ENTITIES XHTML Qualified Names 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-qname-1.mod");
                case "xhtml-events-1.mod":
				case "file://-//W3C//ENTITIES XHTML Intrinsic Events 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-events-1.mod");
                case "xhtml-attribs-1.mod":
				case "file://-//W3C//ENTITIES XHTML Common Attributes 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-attribs-1.mod");
                case "xhtml-charent-1.mod":
				case "file://-//W3C//ENTITIES XHTML Character Entities 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-charent-1.mod");
                case "xhtml-lat1.ent":
				case "file://-//W3C//ENTITIES Latin 1 for XHTML//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-lat1.ent");
                case "xhtml-symbol.ent":
				case "file://-//W3C//ENTITIES Symbols for XHTML//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-symbol.ent");
                case "xhtml-special.ent":
				case "file://-//W3C//ENTITIES Special for XHTML//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-special.ent");
                case "xhtml-text-1.mod":
				case "file://-//W3C//ELEMENTS XHTML Text 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-text-1.mod");
                case "xhtml-inlstruct-1.mod":
				case "file://-//W3C//ELEMENTS XHTML Inline Structural 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-inlstruct-1.mod");
                case "xhtml-inlphras-1.mod":
				case "file://-//W3C//ELEMENTS XHTML Inline Phrasal 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-inlphras-1.mod");
                case "xhtml-blkstruct-1.mod":
				case "file://-//W3C//ELEMENTS XHTML Block Structural 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-blkstruct-1.mod");
                case "xhtml-blkphras-1.mod":
				case "file://-//W3C//ELEMENTS XHTML Block Phrasal 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-blkphras-1.mod");
                case "xhtml-hypertext-1.mod":
				case "file://-//W3C//ELEMENTS XHTML Hypertext 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-hypertext-1.mod");
                case "xhtml-list-1.mod":
				case "file://-//W3C//ELEMENTS XHTML Lists 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-list-1.mod");
                case "xhtml-edit-1.mod":
				case "file://-//W3C//ELEMENTS XHTML Editing Elements 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-edit-1.mod");
                case "xhtml-bdo-1.mod":
				case "file://-//W3C//ELEMENTS XHTML BIDI Override Element 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-bdo-1.mod");
                case "xhtml-ruby-1.mod":
				case "file://-//W3C//ELEMENTS XHTML Ruby 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-ruby-1.mod");
                case "xhtml-pres-1.mod":
				case "file://-//W3C//ELEMENTS XHTML Presentation 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-pres-1.mod");
                case "xhtml-inlpres-1.mod":
				case "file://-//W3C//ELEMENTS XHTML Inline Presentation 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-inlpres-1.mod");
                case "xhtml-blkpres-1.mod":
				case "file://-//W3C//ELEMENTS XHTML Block Presentation 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-blkpres-1.mod");
                case "xhtml-link-1.mod":
				case "file://-//W3C//ELEMENTS XHTML Link Element 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-link-1.mod");
                case "xhtml-meta-1.mod":
				case "file://-//W3C//ELEMENTS XHTML Metainformation 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-meta-1.mod");
                case "xhtml-base-1.mod":
				case "file://-//W3C//ELEMENTS XHTML Base Element 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-base-1.mod");
                case "xhtml-script-1.mod":
				case "file://-//W3C//ELEMENTS XHTML Scripting 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-script-1.mod");
                case "xhtml-style-1.mod":
				case "file://-//W3C//ELEMENTS XHTML Style Sheets 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-style-1.mod");
                case "xhtml-image-1.mod":
				case "file://-//W3C//ELEMENTS XHTML Images 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-image-1.mod");
                case "xhtml-csismap-1.mod":
				case "file://-//W3C//ELEMENTS XHTML Client-side Image Maps 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-csismap-1.mod");
                case "xhtml-ssismap-1.mod":
				case "file://-//W3C//ELEMENTS XHTML Server-side Image Maps 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-ssismap-1.mod");
                case "xhtml-param-1.mod":
				case "file://-//W3C//ELEMENTS XHTML Param Element 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-param-1.mod");
                case "xhtml-object-1.mod":
				case "file://-//W3C//ELEMENTS XHTML Embedded Object 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-object-1.mod");
                case "xhtml-table-1.mod":
				case "file://-//W3C//ELEMENTS XHTML Tables 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-table-1.mod");
                case "xhtml-form-1.mod":
				case "file://-//W3C//ELEMENTS XHTML Forms 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-form-1.mod");
                case "xhtml-legacy-1.mod":
				case "file://-//W3C//ELEMENTS XHTML Legacy Markup 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-legacy-1.mod");
                case "xhtml-struct-1.mod":
				case "file://-//W3C//ELEMENTS XHTML Document Structure 1.0//EN": return Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.CssSimpler.xhtml-struct-1.mod");
                default: return base.GetEntity(absoluteUri, role, ofObjectToReturn);
            }
        }
    }

    public class ValueTest : XmlCopy
    {
        private readonly string _testValue;

        public ValueTest(string inpTest, string outTest, string testValue) : base(inpTest, outTest)
        {
            _testValue = testValue;
            DeclareBeforeEnd(XmlNodeType.EndElement, WriteValue);
            Parse();
        }

        private void WriteValue(int dept, string name)
        {
            WriteContent(_testValue, "Test");
        }
    }
}