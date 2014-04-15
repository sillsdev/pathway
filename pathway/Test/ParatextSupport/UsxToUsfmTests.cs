// --------------------------------------------------------------------------------------------
// <copyright file="UsxToUsfmTests.cs" from='2009' to='2014' company='SIL International'>
//      Copyright ( c ) 2014, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// 
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using NUnit.Framework;
using SIL.PublishingSolution;

namespace Test.ParatextSupport
{
    /// <summary>
    /// UsxToUsfmTests tests the UsxToUsfm transform. This transform converts much of the markup from USX to XHTML.
    /// NOTE: More tests are needed.
    /// </summary>
    [TestFixture]
    public class UsxToUsfmTests
    {
        #region Constants
        private const string usxOpen = "<?xml version=\"1.0\" encoding=\"utf-16\"?><usx>";
        private const string usxBookOpen = "<book code=\"JDG\" style=\"id\">";
        private const string usxBookClose = "</book></usx>";
        private const string usxTitle = "<para style=\"h\">Judges</para><para style=\"mt2\">OT: Narrative</para><para style=\"mt\">Judges</para>";
        private const string usxIntroSection = "<para style=\"is\">Intro Section</para><para style=\"ip\">Intro Para</para>";
        private const string usxIntroSectionContent = "<para style=\"ip\">Intro Para</para>";
        private const string usxChapter = "<chapter number=\"1\" style=\"c\" />";
        private const string usxSectionHead = "<para style=\"s\">Section</para>";
        private const string usxParaVerse = "<para style=\"p\"><verse number=\"1\" style=\"v\" />Para 1</para>";
        private const string usxPara = "<para style=\"p\">Para</para><para style=\"p\">Para</para>";
        private const string usxTable = "<table><row style=\"tr\"><cell style=\"tc1\" align=\"start\">cell 1</cell><cell style=\"tc2\" align=\"start\">cell 2</cell>" +
            "<cell style=\"tc3\" align=\"start\">cell 3</cell></row><row style=\"tr\"><cell style=\"tc1\" align=\"start\">cell 1</cell>" +
            "<cell style=\"tc2\" align=\"start\">cell 2</cell><cell style=\"tc3\" align=\"start\">cell 3</cell></row></table>";

        private const string introSectionHead = "<h1 class=\"Intro_Section_Head\"><span lang=\"zxx\">Intro Section</span></h1>";
        private const string introSectionContent = "<p class=\"Intro_Paragraph\"><span lang=\"zxx\">Intro Para</span></p>";
        private const string table = "<table><tr style=\"tr\"><td style=\"tc1\">cell 1</td><td style=\"tc2\">cell 2</td><td style=\"tc3\">cell 3</td></tr>" +
            "<tr style=\"tr\"><td style=\"tc1\">cell 1</td><td style=\"tc2\">cell 2</td><td style=\"tc3\">cell 3</td></tr></table>";
        private const string chapterVersePara = "<p class=\"Paragraph\"><span class=\"Chapter_Number\" lang=\"zxx\">1</span>" +
            "<span class=\"Verse_Number\" lang=\"zxx\">1</span><span lang=\"zxx\">Para 1</span></p>";
        private const string chapterPara = "<p class=\"Paragraph\"><span class=\"Chapter_Number\" lang=\"zxx\">1</span><span lang=\"zxx\">Para</span></p>";
        private const string scrSectionHead = "<h1 class=\"Section_Head\"><span lang=\"zxx\">Section</span></h1>";
        private const string scrSectionContent = "<p class=\"Paragraph\"><span lang=\"zxx\">Para</span></p><p class=\"Paragraph\"><span lang=\"zxx\">Para</span></p>";

        private const string htmlOpen = "<?xml version=\"1.0\" encoding=\"utf-16\"?><html xml:lang=\"utf-8\" xmlns=\"http://www.w3.org/1999/xhtml\">";
        private const string htmlHeader = "<head><title /><link rel=\"stylesheet\" href=\"TestProj.css\" type=\"text/css\" />" +
            "<link rel=\"schema.DCTERMS\" href=\"http://purl.org/dc/terms/\" />" +
            "<link rel=\"schema.DC\" href=\"http://purl.org/dc/elements/1.1/\" />" +
            "<meta name=\"description\" content=\"TestProj exported by Tester on 2013-08-27T00:00:00\" />" +
            "<meta name=\"filename\" content=\"TestProj.xhtml\" />" +
            "<meta name=\"stylesheet\" content=\"usfm\" />" + 
            "<meta name=\"fontName\" content=\"Times\" />" +
            "<meta name=\"fontSize\" content=\"12\" />" +
            "<meta name=\"dc.language\" content=\"\" scheme=\"DCTERMS.RFC5646\" /></head>";
        private const string bookOpen = "<body class=\"scrBody\"><div class=\"scrBook\"><span class=\"scrBookName\" lang=\"zxx\">Judges</span>" +
            "<span class=\"scrBookCode\" lang=\"zxx\">JDG</span>";
        private const string title = "<div class=\"Title_Main\"><span class=\"Title_Secondary\" lang=\"zxx\">OT: Narrative</span><span lang=\"zxx\">Judges</span></div>";
        private const string bookClose = "</div></body></html>";
        private const string expectedIntroSectionHead = "<div class=\"Intro_Section_Head\"><span lang=\"zxx\">intro section</span></div>";
        private const string expectedCreatedIntroSectionHead = "<div class=\"Intro_Section_Head\"><span lang=\"zxx\"></span></div>";
        private const string expectedIntroSectionContent = "<div class=\"Intro_Paragraph\"><span lang=\"zxx\">intro para</span></div>";
        private const string expectedChapterVersePara = "<div class=\"Paragraph\"><span class=\"Chapter_Number\" lang=\"zxx\">1</span><span class=\"Verse_Number\" lang=\"zxx\">1</span></div>";
        private const string expectedScrSectionHead = "<div class=\"Section_Head\"><span lang=\"zxx\">Section</span></div>";
        private const string expectedCreatedScrSectionHead = "<div class=\"Section_Head\"><span lang=\"zxx\"></span></div>";
        private const string expectedScrSectionContent = "<div class=\"Paragraph\"><span lang=\"zxx\">para content</span><span lang=\"zxx\">Para</span></div>";
        #endregion

        private XslCompiledTransform usxToXhtmlXslt;
        Dictionary<string, object> xslParams;

        #region Fixture setup
        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            xslParams = new Dictionary<string, object>();
            DateTime dateTime = new DateTime(2013, 8, 27);
            xslParams.Add("dateTime", String.Format("{0:s}", dateTime));
            xslParams.Add("user", "Tester");
            xslParams.Add("projName", "TestProj");
            xslParams.Add("stylesheet", "usfm");
            xslParams.Add("ws", "en");
            xslParams.Add("fontName", "Times");
            xslParams.Add("fontSize", "12");

            ParatextPathwayLink converter = new ParatextPathwayLink("testDb", xslParams);
            usxToXhtmlXslt = ParatextSupportExtensions.UsxToUsfmXslt(converter);
        }
        #endregion

        #region Tests
        /// <summary>
        /// All paragraphs in introduction and Scripture have explicit section heads.
        /// </summary>
        [Test]
        public void Simple()
        {
            StringBuilder inputBldr = new StringBuilder();
            inputBldr.Append(usxOpen)
                .Append(usxBookOpen)
                .Append(usxTitle)
                .Append(usxIntroSection)
                .Append(usxChapter)
                .Append(usxSectionHead)
                .Append(usxParaVerse)
                .Append(usxPara)
                .Append(usxBookClose);

            StringBuilder expectedBldr = new StringBuilder();
            expectedBldr.Append(htmlOpen)
                .Append(htmlHeader)
                .Append(bookOpen)
                .Append(title)
                .Append(introSectionHead)
                .Append(introSectionContent)
                .Append(scrSectionHead)
                .Append(chapterVersePara)
                .Append(scrSectionContent)
                .Append(bookClose);

            Assert.AreEqual(expectedBldr.ToString(), ApplyTransform(inputBldr.ToString()));
        }

        /// <summary>
        /// No section heads before paragraphs in introduction and Scripture.
        /// </summary>
        [Test]
        public void NoSectionHeads()
        {
            StringBuilder inputBldr = new StringBuilder();
            inputBldr.Append(usxOpen)
                .Append(usxBookOpen)
                .Append(usxTitle)
                .Append(usxIntroSectionContent)
                .Append(usxChapter)
                .Append(usxParaVerse)
                .Append(usxPara)
                .Append(usxBookClose);

            StringBuilder expectedBldr = new StringBuilder();
            expectedBldr.Append(htmlOpen)
                .Append(htmlHeader)
                .Append(bookOpen)
                .Append(title)
                .Append(introSectionContent)
                .Append(chapterVersePara)
                .Append(scrSectionContent)
                .Append(bookClose);

            Assert.AreEqual(expectedBldr.ToString(), ApplyTransform(inputBldr.ToString()));
        }
        #endregion

        #region Private helper methods
        private string ApplyTransform(string input)
        {
            // Create argument list
            XsltArgumentList args = new XsltArgumentList();
            foreach (string paramName in xslParams.Keys)
                args.AddParam(paramName, "", xslParams[paramName]);

            StringBuilder strBldr = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(strBldr); // NOTE: not using output settings from XSLT
            usxToXhtmlXslt.Transform(XmlReader.Create(new StringReader(input)), args, writer, null);

            return strBldr.ToString();
        }
        #endregion
    }
}

