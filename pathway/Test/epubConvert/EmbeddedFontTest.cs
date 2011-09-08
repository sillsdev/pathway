// --------------------------------------------------------------------------------------------
// <copyright file="EmbeddedFontTest.cs" from='2009' to='2010' company='SIL International'>
//      Copyright © 2010, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Erik Brommers</author>
// <email>erik_brommers@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Test methods of EmbeddedFont class
// </remarks>
// --------------------------------------------------------------------------------------------
using System.IO;
using epubConvert;
using NUnit.Framework;
using SIL.Tool;

namespace Test.epubConvert
{
    /// <summary>
    ///This is a test class for FontInternalsTest and is intended
    ///to contain all FontInternals Unit Tests
    ///</summary>
    [TestFixture]
    public class EmbeddedFontTest
    {
        /// <summary>
        /// Test a known non-SIL font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void NonSilFontTest()
        {
            Assert.IsTrue(FontInternals.IsInstalled("Arial"));
            var arialFont = new EmbeddedFont("Arial");
            Assert.IsFalse(arialFont.SILFont);
            const string fontFilename = "Arial.ttf";
            Assert.IsTrue(fontFilename.ToLower().Equals(Path.GetFileName(arialFont.Filename).ToLower()));
        }

        /// <summary>
        /// Test the SIL Abyssinica font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void AbyssinicaTest()
        {
            Assert.IsTrue(FontInternals.IsInstalled("Abyssinica SIL"));
            var silFont = new EmbeddedFont("Abyssinica SIL");
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
        }

        /// <summary>
        /// Test the SIL Andika font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void AndikaTest()
        {
            var silFont = new EmbeddedFont("Andika Basic");
            Assert.IsTrue(FontInternals.IsInstalled("Andika Basic"));
            Assert.IsTrue(silFont.SILFont);
            Assert.IsFalse(silFont.Serif);
        }

        /// <summary>
        /// Test the Charis SIL font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void CharisTest()
        {
            var silFont = new EmbeddedFont("Charis SIL");
            Assert.IsTrue(FontInternals.IsInstalled("Charis SIL"));
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
        }
        /// <summary>
        /// Test the Dai Banna font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void DaiBannaTest()
        {
            var silFont = new EmbeddedFont("Dai Banna SIL Book");
            Assert.IsTrue(FontInternals.IsInstalled("Dai Banna SIL Book"));
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
        }
        /// <summary>
        /// Test the Doulos SIL font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void DoulosTest()
        {
            var silFont = new EmbeddedFont("Doulos SIL");
            Assert.IsTrue(FontInternals.IsInstalled("Doulos SIL"));
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
        }
        /// <summary>
        /// Test the SIL Ezra font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void EzraTest()
        {
            var silFont = new EmbeddedFont("Ezra SIL");
            Assert.IsTrue(FontInternals.IsInstalled("Ezra SIL"));
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
        }
        /// <summary>
        /// Test the SIL Galatia font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void GalatiaTest()
        {
            var silFont = new EmbeddedFont("Galatia SIL");
            Assert.IsTrue(FontInternals.IsInstalled("Galatia SIL"));
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
        }
        /// <summary>
        /// Test the SIL Gentium Basic font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void GentiumTest()
        {
            var silFont = new EmbeddedFont("Gentium");
            Assert.IsTrue(FontInternals.IsInstalled("Gentium"));
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
        }
        /// <summary>
        /// Test the SIL Scheharazade) font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void ScheharazadeTest()
        {
            var silFont = new EmbeddedFont("Scheherazade");
            Assert.IsTrue(FontInternals.IsInstalled("Scheherazade"));
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
        }
        /// <summary>
        /// Test the SIL Sophia Nubian font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void SophiaNubianTest()
        {
            var silFont = new EmbeddedFont("Sophia Nubian");
            Assert.IsTrue(FontInternals.IsInstalled("Sophia Nubian"));
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
        }
    }
}