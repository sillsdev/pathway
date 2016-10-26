// --------------------------------------------------------------------------------------------
// <copyright file="EmbeddedFontTest.cs" from='2009' to='2014' company='SIL International'>
//      Copyright ( c ) 2014, SIL International. All Rights Reserved.   
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
    [Category("ShortTest")]
    public class EmbeddedFontTest
    {
        /// <summary>
        /// Test a known Free font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void FreeFontTest()
        {
            Assert.IsTrue(FontInternals.IsInstalled("Arial"));
            var arialFont = new EmbeddedFont("Arial");
            Assert.IsTrue(arialFont.CanRedistribute);
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
            Assert.IsTrue(silFont.CanRedistribute);
            Assert.IsTrue(silFont.Serif);
        }

        /// <summary>
        /// Test the SIL Andika font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void AndikaTest()
        {
            string fontName = "Andika Basic";
            if(Common.IsUnixOS())
            {
                fontName = "Andika";
            }
            var silFont = new EmbeddedFont(fontName);
            Assert.IsTrue(FontInternals.IsInstalled(fontName));
            Assert.IsTrue(silFont.CanRedistribute);
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
            Assert.IsTrue(silFont.CanRedistribute);
            Assert.IsTrue(silFont.Serif);
        }
        /// <summary>
        /// Test the Dai Banna font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void DaiBannaTest()
        {
            string fontName = "Dai Banna SIL Book";
            if (Common.IsUnixOS())
            {
                fontName = "dai-banna";
            }
            var silFont = new EmbeddedFont(fontName);
            if (!Common.IsUnixOS())
            {
                Assert.IsTrue(FontInternals.IsInstalled(fontName));
                Assert.IsTrue(silFont.CanRedistribute);
            }
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
            Assert.IsTrue(silFont.CanRedistribute);
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
            Assert.IsTrue(silFont.CanRedistribute);
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
            Assert.IsTrue(silFont.CanRedistribute);
            Assert.IsTrue(silFont.Serif);
        }
        /// <summary>
        /// Test the SIL Gentium Basic font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void GentiumTest()
        {
            string fontName = "Gentium";
            if (Common.IsUnixOS())
            {
                fontName = "Gentiumbasic";
            }
            var silFont = new EmbeddedFont(fontName);
            if(!Common.IsUnixOS())
            {
                Assert.IsTrue(FontInternals.IsInstalled(fontName));
                Assert.IsTrue(silFont.CanRedistribute);
            }
            Assert.IsTrue(silFont.Serif);
        }
        /// <summary>
        /// Test the Linux Libertine font. This font is not created by SIL, but is redistributable under the
        /// GPL and OFL licenses. This serif font is used in Wikipedia's logo; you can download it 
        /// here: http://www.linuxlibertine.org/
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void LibertineTest()
        {
            var font = new EmbeddedFont("Linux Libertine");
            if (FontInternals.IsInstalled("Linux Libertine"))
            {
                Assert.IsFalse(FontInternals.IsSILFont(font.Filename));
                Assert.IsTrue(font.CanRedistribute);
                Assert.IsTrue(font.Serif);
            }
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
            Assert.IsTrue(silFont.CanRedistribute);
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
            Assert.IsTrue(silFont.CanRedistribute);
            Assert.IsTrue(silFont.Serif);
        }
    }
}