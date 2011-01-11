// --------------------------------------------------------------------------------------------
// <copyright file="FontInternalsTest.cs" from='2009' to='2009' company='SIL International'>
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
// Test methods of FlexDePlugin
// </remarks>
// --------------------------------------------------------------------------------------------
using System;
using SIL.Tool;
using NUnit.Framework;

namespace Test.PsTool
{
    /// <summary>
    ///This is a test class for FontInternalsTest and is intended
    ///to contain all FontInternals Unit Tests
    ///</summary>
    [TestFixture]
    public class FontTest
    {
        /// <summary>
        ///A test Arial Postscript font name
        ///</summary>
        [Test]
        public void ArialTest()
        {
            var root = Environment.GetEnvironmentVariable("SystemRoot");
            var fontFullName = Common.PathCombine(root, "Fonts/Arial.ttf");
            var actual = FontInternals.GetPostscriptName(fontFullName);
            var expected = "ArialMT";
            Assert.AreEqual(expected,actual);
        }

        /// <summary>
        ///A test Arial Postscript font name
        ///</summary>
        [Test]
        public void TimesTest()
        {
            var root = Environment.GetEnvironmentVariable("SystemRoot");
            var fontFullName = Common.PathCombine(root, "Fonts/timesbd.ttf");
            var actual = FontInternals.GetPostscriptName(fontFullName);
            var expected = "TimesNewRomanPS-BoldMT";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test Arial Postscript font name
        ///</summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void YiTest()
        {
            var root = Environment.GetEnvironmentVariable("SystemRoot");
            var fontFullName = Common.PathCombine(root, "Fonts/YI_PLUS.ttf");
            var actual = FontInternals.GetPostscriptName(fontFullName);
            var expected = "YiplusPhonetics";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test Arial Postscript font name
        ///</summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void ScheherazadeTest()
        {
            var root = Environment.GetEnvironmentVariable("SystemRoot");
            var fontFullName = Common.PathCombine(root, "Fonts/sch_gr_alpha9.ttf");
            var actual = FontInternals.GetPostscriptName(fontFullName);
            var expected = "Scheherazade";
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///A test Arial Postscript font name
        ///</summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void SimSunTest()
        {
            var root = Environment.GetEnvironmentVariable("SystemRoot");
            var fontFullName = Common.PathCombine(root, "Fonts/SimSun18030.ttc");
            var actual = FontInternals.GetPostscriptName(fontFullName);
            var expected = "SimSun-18030";
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///A test Arial Postscript font name
        ///</summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void MyriadProTest()
        {
            var root = Environment.GetEnvironmentVariable("SystemRoot");
            var fontFullName = Common.PathCombine(root, "Fonts/MyriadPro-Regular.otf");
            var actual = FontInternals.GetPostscriptName(fontFullName);
            var expected = "MyriadPro-Regular";
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///A test Arial Postscript font name
        ///</summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void SimSun2Test()
        {
            var root = Environment.GetEnvironmentVariable("SystemRoot");
            var fontFullName = Common.PathCombine(root, "Fonts/SimSunb.ttf");
            var actual = FontInternals.GetPostscriptName(fontFullName);
            var expected = "SimSun-ExtB";
            Assert.AreEqual(expected, actual);
        }


        [Test]
        [Category("SkipOnTeamCity")]
        public void GetFontFileNameTest()
        {
            string familyName = "Charis SIL";
            string style = "Regular";
            string actual = FontInternals.GetFontFileName(familyName, style);
            string expected = "CharisSILR.ttf";
            Assert.AreEqual(expected, actual);
        }


        [Test]
        [Category("SkipOnTeamCity")]
        public void GetFontFileNameTest2()
        {
            string familyName = "Charis SIL";
            string style = "Bold";
            string actual = FontInternals.GetFontFileName(familyName, style);
            string expected = "CharisSILB.ttf";
            Assert.AreEqual(expected, actual);
        }


        [Test]
        [Category("SkipOnTeamCity")]
        public void GetFontFileNameTest3()
        {
            string familyName = "Scheherazade Graphite Alpha";
            string style = "Regular";
            string actual = FontInternals.GetFontFileName(familyName, style);
            string expected = "sch_gr_alpha9.ttf";
            Assert.AreEqual(expected, actual);
        }


        [Test]
        [Category("SkipOnTeamCity")]
        public void GetFontFileNameTest4()
        {
            string familyName = "Scheherazade Graphite Alpha";
            string style = "Bold";
            string actual = FontInternals.GetFontFileName(familyName, style);
            string expected = "sch_gr_alpha9.ttf";
            Assert.AreEqual(expected, actual);
        }


        [Test]
        [Category("SkipOnTeamCity")]
        public void GetFontFileNameTest5()
        {
            string familyName = "Yi plus Phonetics";
            string style = "Bold";
            string actual = FontInternals.GetFontFileName(familyName, style);
            string expected = "YI_PLUS.ttf";
            Assert.AreEqual(expected, actual);
        }
        
        
        [Test]
        [Category("SkipOnTeamCity")]
        public void CharisBoldTest2()
        {
            string familyName = "Charis SIL";
            string style = "Bold";
            string actual = FontInternals.GetPostscriptName(familyName, style);
            string expected = "CharisSIL-Bold";
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///A test Arial Postscript font name
        ///</summary>
        [Test]
        public void TimesGraphiteTest()
        {
            var root = Environment.GetEnvironmentVariable("SystemRoot");
            var fontFullName = Common.PathCombine(root, "Fonts/timesbd.ttf");
            var actual = FontInternals.IsGraphite(fontFullName);
            Assert.IsFalse(actual);
        }

        /// <summary>
        ///A test Arial Postscript font name
        ///</summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void CharisGraphiteTest()
        {
            var root = Environment.GetEnvironmentVariable("SystemRoot");
            var fontFullName = Common.PathCombine(root, "Fonts/CharisSILR.ttf");
            var actual = FontInternals.IsGraphite(fontFullName);
            Assert.IsTrue(actual);
        }

        /// <summary>
        ///A test Arial Postscript font name
        ///</summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void ScheherazadeGraphiteTest()
        {
            var root = Environment.GetEnvironmentVariable("SystemRoot");
            var fontFullName = Common.PathCombine(root, "Fonts/sch_gr_alpha9.ttf");
            var actual = FontInternals.IsGraphite(fontFullName);
            Assert.IsTrue(actual);
        }


    }
}