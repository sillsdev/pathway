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
            const string fontFilename = "arial.ttf";
            Assert.AreEqual(fontFilename, Path.GetFileName(arialFont.Filename));
        }

        /// <summary>
        /// Test the SIL Abyssinica font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void AbyssinicaTest()
        {
            Assert.IsTrue(FontInternals.IsInstalled("Abyssinica"));
            var silFont = new EmbeddedFont("Abyssinica");
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
            var silFont = new EmbeddedFont("Andika");
            Assert.IsTrue(FontInternals.IsInstalled("Andika"));
            Assert.IsTrue(silFont.SILFont);
            Assert.IsFalse(silFont.Serif);
        }

        /// <summary>
        /// Test the Apparatus SIL font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void ApparatusTest()
        {
            var silFont = new EmbeddedFont("Apparatus SIL");
            Assert.IsTrue(FontInternals.IsInstalled("Apparatus SIL"));
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
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
            var silFont = new EmbeddedFont("Dai Banna");
            Assert.IsTrue(FontInternals.IsInstalled("Dai Banna"));
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
            var silFont = new EmbeddedFont("Gentium Basic");
            Assert.IsTrue(FontInternals.IsInstalled("Gentium Basic"));
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
        }
        /// <summary>
        /// Test the SIL Lateef font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void LateefTest()
        {
            var silFont = new EmbeddedFont("Lateef");
            Assert.IsTrue(FontInternals.IsInstalled("Lateef"));
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
        }
        /// <summary>
        /// Test the SIL Nuosu (Yi) font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void NuosuTest()
        {
            var silFont = new EmbeddedFont("Nuosu");
            Assert.IsTrue(FontInternals.IsInstalled("Nuosu"));
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
        }
        /// <summary>
        /// Test the SIL Padauk font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void PadaukTest()
        {
            var silFont = new EmbeddedFont("Padauk");
            Assert.IsTrue(FontInternals.IsInstalled("Padauk"));
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
        /// <summary>
        /// Test the SIL Tai Heritage Pro font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void TaiHeritageProTest()
        {
            var silFont = new EmbeddedFont("Tai Heritage Pro");
            Assert.IsTrue(FontInternals.IsInstalled("Tai Heritage Pro"));
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
        }

    }
}