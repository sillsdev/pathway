// --------------------------------------------------------------------------------------------
// <copyright file="EncloseParasInSectionsTests.cs" from='2009' to='2014' company='SIL International'>
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
using SIL.Tool;

namespace Test.ParatextSupport
{
	/// <summary>
	/// EncloseParasInSectionsTests tests the EncloseParasInSections transform. This transform adds section structure to a Pathway file
	/// from a Paratext USX file.
	/// </summary>
	[TestFixture]
	public class EncloseParasInSectionsTests
	{
		#region Constants

		private const string divider = "------------------------------------------------------------";

		private const string htmlOpen =
			"<?xml version=\"1.0\" encoding=\"utf-16\"?><html xml:lang=\"utf-8\" xmlns=\"http://www.w3.org/1999/xhtml\">";

		private string _htmlHeader = "<head><title /></head>";

		private const string bookOpen =
			"<body class=\"scrBody\"><div class=\"scrBook\"><span class=\"scrBookName\" lang=\"zxx\">Genesis</span>" +
			"<span class=\"scrBookCode\" lang=\"zxx\">GEN</span>";

		private const string bookClose = "</div></body></html>";

		private const string title =
			"<div class=\"Title_Main\"><span class=\"Title_Secondary\" lang=\"zxx\">OT: The Law</span><span lang=\"zxx\">Genesis</span></div>";

		private const string introSectionHead =
			"<h1 class=\"Intro_Section_Head\"><span lang=\"zxx\">intro section</span></h1>";

		private const string introSectionContent = "<p class=\"Intro_Paragraph\"><span lang=\"zxx\">intro para</span></p>";

		private const string table =
			"<table><tr style=\"tr\"><td style=\"tc1\">cell 1</td><td style=\"tc2\">cell 2</td><td style=\"tc3\">cell 3</td></tr>" +
			"<tr style=\"tr\"><td style=\"tc1\">cell 1</td><td style=\"tc2\">cell 2</td><td style=\"tc3\">cell 3</td></tr></table>";

		private const string chapterVersePara =
			"<p class=\"Paragraph\"><span class=\"Chapter_Number\" lang=\"zxx\">1</span><span class=\"Verse_Number\" lang=\"zxx\">1</span></p>";

		private const string chapterPara = "<p class=\"Paragraph\"><span class=\"Chapter_Number\" lang=\"zxx\">1</span></p>";
		private const string scrSectionHead = "<h1 class=\"Section_Head\"><span lang=\"zxx\">Section</span></h1>";

		private const string scrSectionContent =
			"<p class=\"Paragraph\"><span lang=\"zxx\">para content</span><span lang=\"zxx\">Para</span></p>";

		private const string expectedIntroSectionHead =
			"<div class=\"Intro_Section_Head\"><span lang=\"zxx\">intro section</span></div>";

		private const string expectedIntroSectionContent =
			"<div class=\"Intro_Paragraph\"><span lang=\"zxx\">intro para</span></div>";

		private const string expectedChapterVersePara =
			"<div class=\"Paragraph\"><span class=\"Chapter_Number\" lang=\"zxx\">1</span>" +
			"<span class=\"Verse_Number\" lang=\"zxx\">1</span></div>";

		private const string expectedChapterPara =
			"<div class=\"Paragraph\"><span class=\"Chapter_Number\" lang=\"zxx\">1</span></div>";

		private const string expectedScrSectionHead = "<div class=\"Section_Head\"><span lang=\"zxx\">Section</span></div>";

		private const string expectedScrSectionContent =
			"<div class=\"Paragraph\"><span lang=\"zxx\">para content</span><span lang=\"zxx\">Para</span></div>";

		#endregion

		#region Member variables

		private XslCompiledTransform encloseParasInSections;
		Dictionary<string, object> xslParams;
		private TestFiles _tf = new TestFiles("ParatextSupport");

		#endregion

		#region Fixture setup

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			xslParams = new Dictionary<string, object>();
			DateTime dateTime = new DateTime(2013, 8, 27);
			xslParams.Add("dateTime", dateTime.Date);
			xslParams.Add("user", "Tester");
			xslParams.Add("projName", "TestProj");
			xslParams.Add("stylesheet", "usfm");
			xslParams.Add("ws", "en");
			xslParams.Add("fontName", "Times");
			xslParams.Add("fontSize", "12");
			Param.LoadSettings();
			if (Common.CallerSetting != null)
			{
				try
				{
					Common.CallerSetting.Dispose();
				}
				catch (Exception)
				{
					// If there is an error disposing the old one, we just create the new one.
				}
	        }
	        Common.CallerSetting = new CallerSetting {SettingsFullPath = _tf.Input("TestDb.ssf")};
            ParatextPathwayLink converter = new ParatextPathwayLink("testDb", xslParams);
            encloseParasInSections = ParatextSupportExtensions.EncloseParasInSectionsXslt(converter);
        }
        [TestFixtureTearDown]
        public void FixtureTearDown(){
            Param.DatabaseName = "DatabaseName";
			Common.CallerSetting.Dispose();
	        Common.CallerSetting = null;
        }
        #endregion

        #region Paragraph-handling tests
        /// <summary>
        /// All paragraphs in introduction and Scripture have explicit section heads.
        /// </summary>
        [Test]
        public void Simple()
        {
            StringBuilder inputBldr = new StringBuilder();
            inputBldr.Append(htmlOpen)
                .Append(_htmlHeader)
                .Append(bookOpen)
                .Append(title)
                .Append(introSectionHead)
                .Append(introSectionContent)
                .Append(scrSectionHead)
                .Append(chapterVersePara)
                .Append(bookClose);

            StringBuilder expectedBldr = new StringBuilder();
            expectedBldr.Append(htmlOpen)
                .Append(_htmlHeader)
                .Append(bookOpen)
                .Append(title)
                .Append("<div class=\"scrIntroSection\">")
                .Append(expectedIntroSectionHead)
                .Append(expectedIntroSectionContent)
                .Append("</div><div class=\"columns\"><div class=\"scrSection\">")
                .Append(expectedScrSectionHead)
                .Append(expectedChapterVersePara)
                .Append("</div></div>")
                .Append(bookClose);

            ValidateTransform("Simple", inputBldr.ToString(), expectedBldr.ToString());
        }

        /// <summary>
        /// No section heads for all paragraphs in introduction and Scripture.
        /// </summary>
        [Test]
        public void ParasWithNoSectionHeads()
        {
            StringBuilder inputBldr = new StringBuilder();
            inputBldr.Append(htmlOpen)
                .Append(_htmlHeader)
                .Append(bookOpen)
                .Append(title)
                .Append(introSectionContent)
                .Append(chapterVersePara)
                .Append(bookClose);

            StringBuilder expectedBldr = new StringBuilder();
            expectedBldr.Append(htmlOpen)
                .Append(_htmlHeader)
                .Append(bookOpen)
                .Append(title)
                .Append("<div class=\"scrIntroSection\">")
                .Append(expectedIntroSectionContent)
                .Append("</div><div class=\"columns\"><div class=\"scrSection\">")
                .Append(expectedChapterVersePara)
                .Append("</div></div>")
                .Append(bookClose);

            ValidateTransform("ParasWithNoSectionHeads", inputBldr.ToString(), expectedBldr.ToString());
        }

        [Test]
        public void FirstSectionWithoutSectionHeads()
        {
            StringBuilder inputBldr = new StringBuilder();
            inputBldr.Append(htmlOpen)
                .Append(_htmlHeader)
                .Append(bookOpen)
                .Append(title)
                .Append(introSectionContent)
                .Append(introSectionHead)
                .Append(introSectionContent)
                .Append(chapterVersePara)
                .Append(scrSectionHead)
                .Append(scrSectionContent)
                .Append(bookClose);

            StringBuilder expectedBldr = new StringBuilder();
            expectedBldr.Append(htmlOpen)
                .Append(_htmlHeader)
                .Append(bookOpen)
                .Append(title)
                .Append("<div class=\"scrIntroSection\">")
                .Append(expectedIntroSectionContent)
                .Append("</div><div class=\"scrIntroSection\">")
                .Append(expectedIntroSectionHead)
                .Append(expectedIntroSectionContent)
                .Append("</div><div class=\"columns\"><div class=\"scrSection\">")
                .Append(expectedChapterVersePara)
                .Append("</div><div class=\"scrSection\">")
                .Append(expectedScrSectionHead)
                .Append(expectedScrSectionContent)
                .Append("</div></div>")
                .Append(bookClose);

            ValidateTransform("FirstSectionWithoutSectionHeads", inputBldr.ToString(), expectedBldr.ToString());
        }

        [Test]
        public void SequentialSectionHeads()
        {
            StringBuilder inputBldr = new StringBuilder();
            inputBldr.Append(htmlOpen)
                .Append(_htmlHeader)
                .Append(bookOpen)
                .Append(title)
                .Append("<h1 class=\"Section_Head\"><span lang=\"zxx\">The final speeches of Moses </span><span class=\"Verse_Number\" lang=\"zxx\">1-5</span></h1>")
                .Append("<h1 class=\"Section_Head_Minor\"><span lang=\"zxx\">The first speech: Moses reviews the past</span></h1>")
                .Append("<h1 class=\"Section_Head\"><span lang=\"zxx\">The Lord's command at Mount Sinai</span></h1>")
                .Append("<p class=\"Paragraph\"><span class=\"Chapter_Number\" lang=\"zxx\">1</span>" +
                    "<span lang=\"zxx\">The Lord had given Moses his laws for the people of Israel.</span></p>")
                .Append("<p class=\"pi\"><span lang=\"zxx\"><span lang=\"zxx\">I give you this land...</span></span></p>")
                .Append("<h1 class=\"Section_Head\"><span lang=\"zxx\">Leaders were appointed</span></h1>")
                .Append("<h1 class=\"Parallel_Passage_Reference\"><span lang=\"zxx\">(Exodus 18.13-27)</span></h1>")
                .Append("<h1 class=\"Speech_Speaker\"><span lang=\"zxx\">Moses said:</span></h1>")
                .Append("<p class=\"Paragraph\"><span class=\"Verse_Number\" lang=\"zxx\">9</span>" +
                    "<span lang=\"zxx\">Straight after the Lord commanded us to leave Mount Sinai...</span></p>")
                .Append(bookClose);

            StringBuilder expectedBldr = new StringBuilder();
            expectedBldr.Append(htmlOpen)
                .Append(_htmlHeader)
                .Append(bookOpen)
                .Append(title)
                .Append("<div class=\"columns\"><div class=\"scrSection\">")
                .Append("<div class=\"Section_Head\"><span lang=\"zxx\">The final speeches of Moses </span><span class=\"Verse_Number\" lang=\"zxx\">1-5</span></div>")
                .Append("<div class=\"Section_Head_Minor\"><span lang=\"zxx\">The first speech: Moses reviews the past</span></div>")
                .Append("<div class=\"Section_Head\"><span lang=\"zxx\">The Lord's command at Mount Sinai</span></div>")
                .Append("<div class=\"Paragraph\"><span class=\"Chapter_Number\" lang=\"zxx\">1</span>" +
                    "<span lang=\"zxx\">The Lord had given Moses his laws for the people of Israel.</span></div>")
                .Append("<div class=\"pi\"><span lang=\"zxx\"><span lang=\"zxx\">I give you this land...</span></span></div>")
                .Append("</div><div class=\"scrSection\">")
                .Append("<div class=\"Section_Head\"><span lang=\"zxx\">Leaders were appointed</span></div>")
                .Append("<div class=\"Parallel_Passage_Reference\"><span lang=\"zxx\">(Exodus 18.13-27)</span></div>")
                .Append("<div class=\"Speech_Speaker\"><span lang=\"zxx\">Moses said:</span></div>")
                .Append("<div class=\"Paragraph\"><span class=\"Verse_Number\" lang=\"zxx\">9</span>" +
                    "<span lang=\"zxx\">Straight after the Lord commanded us to leave Mount Sinai...</span></div>")
                .Append("</div></div>")
                .Append(bookClose);

            ValidateTransform("SequentialSectionHeads", inputBldr.ToString(), expectedBldr.ToString());
        }
        #endregion

        #region Table-handling in Scripture tests
        /// <summary>
        /// Paragraphs before and after a table.
        /// </summary>
        [Test]
        public void Table_ParasBeforeAndAfter()
        {
            StringBuilder inputBldr = new StringBuilder();
            inputBldr.Append(htmlOpen)
                .Append(_htmlHeader)
                .Append(bookOpen)
                .Append(title)
                .Append(scrSectionHead)
                .Append(chapterVersePara)
                .Append(table)
                .Append(scrSectionContent)
                .Append(bookClose);

            StringBuilder expectedBldr = new StringBuilder();
            expectedBldr.Append(htmlOpen)
                .Append(_htmlHeader)
                .Append(bookOpen)
                .Append(title)
                .Append("<div class=\"columns\"><div class=\"scrSection\">")
                .Append(expectedScrSectionHead)
                .Append(expectedChapterVersePara)
                .Append(table)
                .Append(expectedScrSectionContent)
                .Append("</div></div>")
                .Append(bookClose);

            ValidateTransform("Table_ParasBeforeAndAfter", inputBldr.ToString(), expectedBldr.ToString());
        }

        /// <summary>
        /// Paragraphs before and after a table with no section heading.
        /// </summary>
        [Test]
        public void Table_ParasBeforeAndAfter_NoSectionHead()
        {
            StringBuilder inputBldr = new StringBuilder();
            inputBldr.Append(htmlOpen)
                .Append(_htmlHeader)
                .Append(bookOpen)
                .Append(title)
                .Append(chapterVersePara)
                .Append(table)
                .Append(scrSectionContent)
                .Append(bookClose);

            StringBuilder expectedBldr = new StringBuilder();
            expectedBldr.Append(htmlOpen)
                .Append(_htmlHeader)
                .Append(bookOpen)
                .Append(title)
                .Append("<div class=\"columns\"><div class=\"scrSection\">")
                .Append(expectedChapterVersePara)
                .Append(table)
                .Append(expectedScrSectionContent)
                .Append("</div></div>")
                .Append(bookClose);

            ValidateTransform("Table_ParasBeforeAndAfter_NoSectionHead", inputBldr.ToString(), expectedBldr.ToString());
        }

        /// <summary>
        /// Paragraphs before table with no section heading.
        /// </summary>
        [Test]
        public void Table_ParasBefore()
        {
            StringBuilder inputBldr = new StringBuilder();
            inputBldr.Append(htmlOpen)
                .Append(_htmlHeader)
                .Append(bookOpen)
                .Append(title)
                .Append(chapterVersePara)
                .Append(table)
                .Append(bookClose);

            StringBuilder expectedBldr = new StringBuilder();
            expectedBldr.Append(htmlOpen)
                .Append(_htmlHeader)
                .Append(bookOpen)
                .Append(title)
                .Append("<div class=\"columns\"><div class=\"scrSection\">")
                .Append(expectedChapterVersePara)
                .Append(table)
                .Append("</div></div>")
                .Append(bookClose);

            ValidateTransform("Table_ParasBefore", inputBldr.ToString(), expectedBldr.ToString());
        }

        [Test]
        public void Table_ParasAfter()
        {
            StringBuilder inputBldr = new StringBuilder();
            inputBldr.Append(htmlOpen)
                .Append(_htmlHeader)
                .Append(bookOpen)
                .Append(title)
                .Append(chapterPara)
                .Append(table)
                .Append(scrSectionContent)
                .Append(bookClose);

            StringBuilder expectedBldr = new StringBuilder();
            expectedBldr.Append(htmlOpen)
                .Append(_htmlHeader)
                .Append(bookOpen)
                .Append(title)
                .Append("<div class=\"columns\"><div class=\"scrSection\">")
                .Append(expectedChapterPara)
                .Append(table)
                .Append(expectedScrSectionContent)
                .Append("</div></div>")
                .Append(bookClose);

            ValidateTransform("Table_ParasAfter", inputBldr.ToString(), expectedBldr.ToString());
        }

        [Test]
        public void Table()
        {
            StringBuilder inputBldr = new StringBuilder();
            inputBldr.Append(htmlOpen)
                .Append(_htmlHeader)
                .Append(bookOpen)
                .Append(title)
                .Append(chapterVersePara)
                .Append(table)
                .Append(bookClose);

            StringBuilder expectedBldr = new StringBuilder();
            expectedBldr.Append(htmlOpen)
                .Append(_htmlHeader)
                .Append(bookOpen)
                .Append(title)
                .Append("<div class=\"columns\"><div class=\"scrSection\">")
                .Append(expectedChapterVersePara)
                .Append(table)
                .Append("</div></div>")
                .Append(bookClose);

            ValidateTransform("Table", inputBldr.ToString(), expectedBldr.ToString());
        }
        #endregion

        #region Table-handling in both Intro and Scripture tests
        /// <summary>
        /// Paragraphs before and after a table in both introduction and Scripture text.
        /// </summary>
        [Test]
        public void Table_ParasBeforeAndAfter_IntroScr()
        {
            StringBuilder inputBldr = new StringBuilder();
            inputBldr.Append(htmlOpen)
                .Append(_htmlHeader)
                .Append(bookOpen)
                .Append(title)
                .Append(introSectionHead)
                .Append(introSectionContent)
                .Append(table)
                .Append(introSectionContent)
                .Append(scrSectionHead)
                .Append(chapterVersePara)
                .Append(table)
                .Append(scrSectionContent)
                .Append(bookClose);

            StringBuilder expectedBldr = new StringBuilder();
            expectedBldr.Append(htmlOpen)
                .Append(_htmlHeader)
                .Append(bookOpen)
                .Append(title)
                .Append("<div class=\"scrIntroSection\">")
                .Append(expectedIntroSectionHead)
                .Append(expectedIntroSectionContent)
                .Append(table)
                .Append(expectedIntroSectionContent)
                .Append("</div><div class=\"columns\"><div class=\"scrSection\">")
                .Append(expectedScrSectionHead)
                .Append(expectedChapterVersePara)
                .Append(table)
                .Append(expectedScrSectionContent)
                .Append("</div></div>")
                .Append(bookClose);

            ValidateTransform("Table_ParasBeforeAndAfter_IntroScr", inputBldr.ToString(), expectedBldr.ToString());
        }

        /// <summary>
        /// Paragraphs before and after a table with no section heading in both introduction and Scripture text.
        /// </summary>
        [Test]
        public void Table_ParasBeforeAndAfter_NoSectionHead_IntroScr()
        {
            StringBuilder inputBldr = new StringBuilder();
            inputBldr.Append(htmlOpen)
                .Append(_htmlHeader)
                .Append(bookOpen)
                .Append(title)
                .Append(introSectionContent)
                .Append(table)
                .Append(introSectionContent)
                .Append(chapterVersePara)
                .Append(table)
                .Append(scrSectionContent)
                .Append(bookClose);

            StringBuilder expectedBldr = new StringBuilder();
            expectedBldr.Append(htmlOpen)
                .Append(_htmlHeader)
                .Append(bookOpen)
                .Append(title)
                .Append("<div class=\"scrIntroSection\">")
                .Append(expectedIntroSectionContent)
                .Append(table)
                .Append(expectedIntroSectionContent)
                .Append("</div><div class=\"columns\"><div class=\"scrSection\">")
                .Append(expectedChapterVersePara)
                .Append(table)
                .Append(expectedScrSectionContent)
                .Append("</div></div>")
                .Append(bookClose);

            ValidateTransform("Table_ParasBeforeAndAfter_NoSectionHead_IntroScr", inputBldr.ToString(), expectedBldr.ToString());
        }

        /// <summary>
        /// Paragraphs before table with no section heading in both introduction and Scripture text.
        /// </summary>
        [Test]
        public void Table_ParasBefore_IntroScr()
        {
            StringBuilder inputBldr = new StringBuilder();
            inputBldr.Append(htmlOpen)
                .Append(_htmlHeader)
                .Append(bookOpen)
                .Append(title)
                .Append(introSectionContent)
                .Append(table)
                .Append(chapterVersePara)
                .Append(table)
                .Append(bookClose);

            StringBuilder expectedBldr = new StringBuilder();
            expectedBldr.Append(htmlOpen)
                .Append(_htmlHeader)
                .Append(bookOpen)
                .Append(title)
                .Append("<div class=\"scrIntroSection\">")
                .Append(expectedIntroSectionContent)
                .Append(table)
                .Append("</div><div class=\"columns\"><div class=\"scrSection\">")
                .Append(expectedChapterVersePara)
                .Append(table)
                .Append("</div></div>")
                .Append(bookClose);

            ValidateTransform("Table_ParasBefore_IntroScr", inputBldr.ToString(), expectedBldr.ToString());
        }

        /// <summary>
        /// Paragraphs after table with no section heading in both introduction and Scripture text.
        /// </summary>
        [Test]
        public void Table_ParasAfter_IntroScr()
        {
            StringBuilder inputBldr = new StringBuilder();
            inputBldr.Append(htmlOpen)
                .Append(_htmlHeader)
                .Append(bookOpen)
                .Append(title)
                .Append(table)
                .Append(introSectionContent)
                .Append(chapterVersePara)
                .Append(table)
                .Append(scrSectionContent)
                .Append(bookClose);

            StringBuilder expectedBldr = new StringBuilder();
            expectedBldr.Append(htmlOpen)
                .Append(_htmlHeader)
                .Append(bookOpen)
                .Append(title)
                .Append("<div class=\"scrIntroSection\">")
                .Append(table)
                .Append(expectedIntroSectionContent)
                .Append("</div><div class=\"columns\"><div class=\"scrSection\">")
                .Append(expectedChapterVersePara)
                .Append(table)
                .Append(expectedScrSectionContent)
                .Append("</div></div>")
                .Append(bookClose);

            ValidateTransform("Table_ParasAfter_IntroScr", inputBldr.ToString(), expectedBldr.ToString());
        }

        /// <summary>
        /// Only tables with no section headings in both introduction and Scripture text.
        /// </summary>
        [Test]
        public void TableOnly_IntroScr()
        {
            StringBuilder inputBldr = new StringBuilder();
            inputBldr.Append(htmlOpen)
                .Append(_htmlHeader)
                .Append(bookOpen)
                .Append(title)
                .Append(introSectionHead)
                .Append(table)
                .Append(chapterVersePara)
                .Append(table)
                .Append(bookClose);

            StringBuilder expectedBldr = new StringBuilder();
            expectedBldr.Append(htmlOpen)
                .Append(_htmlHeader)
                .Append(bookOpen)
                .Append(title)
                .Append("<div class=\"scrIntroSection\">")
                .Append(expectedIntroSectionHead)
                .Append(table)
                .Append("</div><div class=\"columns\"><div class=\"scrSection\">")
                .Append(expectedChapterVersePara)
                .Append(table)
                .Append("</div></div>")
                .Append(bookClose);

            ValidateTransform("TableOnly_IntroScr", inputBldr.ToString(), expectedBldr.ToString());
        }

        /// <summary>
        /// In the introduction, the table is preceded by two paragraphs. In the Scripture, it is followed by three paragraphs.
        /// </summary>
        [Test]
        public void Table_MultipleParas_IntroScr()
        {
            StringBuilder inputBldr = new StringBuilder();
            inputBldr.Append(htmlOpen)
                .Append(_htmlHeader)
                .Append(bookOpen)
                .Append(title)
                .Append(introSectionContent)
                .Append(introSectionContent)
                .Append(table)
                .Append(chapterVersePara)
                .Append(table)
                .Append(scrSectionContent)
                .Append(scrSectionContent)
                .Append(scrSectionContent)
                .Append(bookClose);

            StringBuilder expectedBldr = new StringBuilder();
            expectedBldr.Append(htmlOpen)
                .Append(_htmlHeader)
                .Append(bookOpen)
                .Append(title)
                .Append("<div class=\"scrIntroSection\">")
                .Append(expectedIntroSectionContent)
                .Append(expectedIntroSectionContent)
                .Append(table)
                .Append("</div><div class=\"columns\"><div class=\"scrSection\">")
                .Append(expectedChapterVersePara)
                .Append(table)
                .Append(expectedScrSectionContent)
                .Append(expectedScrSectionContent)
                .Append(expectedScrSectionContent)
                .Append("</div></div>")
                .Append(bookClose);

            ValidateTransform("Test", inputBldr.ToString(), expectedBldr.ToString());
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
            encloseParasInSections.Transform(XmlReader.Create(new StringReader(input)), args, writer, null);

            return strBldr.ToString();
        }

        /// <summary>
        /// Get input and output of transform when it fails.
        /// </summary>
        private void ValidateTransform(string testName, string input, string expected)
        {
            string output = ApplyTransform(input);
            Console.WriteLine(testName + ((output == expected) ? " passed!" : " failed!"));
            Console.WriteLine(divider);
            Console.WriteLine("Input:");
            Console.WriteLine(input);
            Console.WriteLine(divider);
            Console.WriteLine("Output:");
            Console.WriteLine(output);
            Console.WriteLine(divider);
            Console.WriteLine("Expected Output:");
            Console.WriteLine(expected);
            Assert.AreEqual(expected, output);
        }
        #endregion
    }
}
