// --------------------------------------------------------------------------------------------
// <copyright file="FontInternalsTest.cs" from='2009' to='2014' company='SIL International'>
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
			var fontFullName = FontInternals.GetFontFileName("Times New Roman", "normal");
            var actual = FontInternals.GetPostscriptName(fontFullName);
            var expected = "TimesNewRomanPSMT";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test Arial Postscript font name
        ///</summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void AbyssinicaTest()
        {
			var fontFullName = FontInternals.GetFontFileName("Abyssinica SIL", "normal");
            var actual = FontInternals.GetPostscriptName(fontFullName);
            var expected = "AbyssinicaSIL";
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


        [Test]
        [Category("SkipOnTeamCity")]
        public void GetFontFileNameTest()
        {
            string familyName = "Charis SIL";
            string style = "Regular";
            string actual = FontInternals.GetFontFileName(familyName, style);
            string expected = "CharisSIL-R.ttf";
            Assert.AreEqual(expected, Path.GetFileName(actual));
        }


        [Test]
        [Category("SkipOnTeamCity")]
        public void GetFontFileNameTest2()
        {
            string familyName = "Charis SIL";
            string style = "Bold";
            string actual = FontInternals.GetFontFileName(familyName, style);
            string expected = "CharisSIL-B.ttf";
            Assert.AreEqual(expected, Path.GetFileName(actual));
        }


        [Test]
        [Category("SkipOnTeamCity")]
        public void GetFontFileNameTest3()
        {
            string familyName = "Doulos SIL";
            string style = "Regular";
            string actual = FontInternals.GetFontFileName(familyName, style);
            string expected = "DoulosSIL-R.ttf";
            if (Common.IsUnixOS())
            {
                expected = "DoulosSIL-R.ttf";
            }
            Assert.AreEqual(expected, Path.GetFileName(actual));
        }

        
        [Test]
        [Category("SkipOnTeamCity")]
        public void CharisBoldTest()
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
        ///Test whether Scheherazade is a Graphite font (should be false)
        ///</summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void ScheherazadeGraphiteTest()
        {
			var fontFullName = FontInternals.GetFontFileName("Scheherazade", "normal");
            var actual = FontInternals.IsGraphite(fontFullName);
            Assert.IsFalse(actual);
        }
    }
}