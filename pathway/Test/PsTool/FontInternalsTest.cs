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
using System.IO;
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
			var fontFullName = FontInternals.GetFontFileName("Arial", "normal");
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
			var fontFullName = FontInternals.GetFontFileName("Times", "normal");
            var actual = FontInternals.GetPostscriptName(fontFullName);
            var expected = "TimesNewRomanPSMT";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test Arial Postscript font name
        ///</summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void YiTest()
        {
			var fontFullName = FontInternals.GetFontFileName("Yi", "normal");
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
			var fontFullName = FontInternals.GetFontFileName("Scheherazade", "normal");
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
			var fontFullName = FontInternals.GetFontFileName("SimSun-18030", "normal");
            var actual = FontInternals.GetPostscriptName(fontFullName);
            var expected = "SimSun-18030";
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///Myriad Pro Postscript font name
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
			var fontFullName = FontInternals.GetFontFileName("SimSun-ExtB", "normal");
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
            Assert.AreEqual(expected, Path.GetFileName(actual));
        }


        [Test]
        [Category("SkipOnTeamCity")]
        public void GetFontFileNameTest2()
        {
            string familyName = "Charis SIL";
            string style = "Bold";
            string actual = FontInternals.GetFontFileName(familyName, style);
            string expected = "CharisSILB.ttf";
            Assert.AreEqual(expected, Path.GetFileName(actual));
        }


        [Test]
        [Category("SkipOnTeamCity")]
        public void GetFontFileNameTest3()
        {
            string familyName = "Doulos SIL";
            string style = "Regular";
            string actual = FontInternals.GetFontFileName(familyName, style);
            string expected = "DoulosSILR.ttf";
            Assert.AreEqual(expected, Path.GetFileName(actual));
        }


        [Test]
        [Category("SkipOnTeamCity")]
        public void GetFontFileNameTest4()
        {
            string familyName = "Doulos SIL";
            string style = "Bold";
            string actual = FontInternals.GetFontFileName(familyName, style);
            string expected = "DoulosSILR.ttf";
            Assert.AreEqual(expected, Path.GetFileName(actual));
        }


        [Test]
        [Category("SkipOnTeamCity")]
        public void GetFontFileNameTest5()
        {
            string familyName = "Yi plus Phonetics";
            string style = "Bold";
            string actual = FontInternals.GetFontFileName(familyName, style);
            string expected = "YI_PLUS.ttf";
            Assert.AreEqual(expected, Path.GetFileName(actual));
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
        ///Test whether Arial is a Graphite font (should be false)
        ///</summary>
        [Test]
        public void ArialGraphiteTest()
        {
            string familyName = "Arial";
            string style = "Regular";
            string fontFullName = FontInternals.GetFontFileName(familyName, style);
            var actual = FontInternals.IsGraphite(fontFullName);
            Assert.IsFalse(actual);
        }

        /// <summary>
        ///Test whether Charis is a Graphite font (should be true)
        ///</summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void CharisGraphiteTest()
        {
            string familyName = "Charis SIL";
            string style = "Regular";
            string fontFullName = FontInternals.GetFontFileName(familyName, style);
            var actual = FontInternals.IsGraphite(fontFullName);
            Assert.IsTrue(actual);
        }

        /// <summary>
        ///Test whether Scheherazade is a Graphite font (should be true)
        ///</summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void ScheherazadeGraphiteTest()
        {
			var fontFullName = FontInternals.GetFontFileName("Scheherazade", "normal");
            var actual = FontInternals.IsGraphite(fontFullName);
            Assert.IsTrue(actual);
        }
    }
}