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
using epubConvert;
using NUnit.Framework;

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
        public void NonSilFontTest()
        {
            var arialFont = new EmbeddedFont("Arial");
            Assert.IsTrue(EmbeddedFont.IsInstalled(arialFont.Filename));
            Assert.IsFalse(arialFont.SILFont);
            const string fontFilename = "arial.ttf";
            Assert.AreEqual(fontFilename, arialFont.Filename);
        }

        /// <summary>
        /// Test the SIL Abyssinica font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void AbyssinicaTest()
        {
            var silFont = new EmbeddedFont("Abyssinica");
            Assert.IsTrue(EmbeddedFont.IsInstalled(silFont.Filename));
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
            const string weight = "normal";
            Assert.AreEqual(weight, silFont.Weight);
            const string style = "normal";
            Assert.AreEqual(style, silFont.Style);
        }

        /// <summary>
        /// Test the SIL Andika font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void AndikaTest()
        {
            var silFont = new EmbeddedFont("Andika");
            Assert.IsTrue(EmbeddedFont.IsInstalled(silFont.Filename));
            Assert.IsTrue(silFont.SILFont);
            Assert.IsFalse(silFont.Serif);
            const string weight = "normal";
            Assert.AreEqual(weight, silFont.Weight);
            const string style = "normal";
            Assert.AreEqual(style, silFont.Style);
        }

        /// <summary>
        /// Test the Apparatus SIL font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void ApparatusTest()
        {
            var silFont = new EmbeddedFont("Apparatus SIL");
            Assert.IsTrue(EmbeddedFont.IsInstalled(silFont.Filename));
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
            const string weight = "normal";
            Assert.AreEqual(weight, silFont.Weight);
            const string style = "normal";
            Assert.AreEqual(style, silFont.Style);
        }
        /// <summary>
        /// Test the Charis SIL font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void CharisTest()
        {
            var silFont = new EmbeddedFont("Charis SIL");
            Assert.IsTrue(EmbeddedFont.IsInstalled(silFont.Filename));
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
            const string weight = "normal";
            Assert.AreEqual(weight, silFont.Weight);
            const string style = "normal";
            Assert.AreEqual(style, silFont.Style);
        }
        /// <summary>
        /// Test the Dai Banna font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void DaiBannaTest()
        {
            var silFont = new EmbeddedFont("Dai Banna");
            Assert.IsTrue(EmbeddedFont.IsInstalled(silFont.Filename));
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
            const string weight = "normal";
            Assert.AreEqual(weight, silFont.Weight);
            const string style = "normal";
            Assert.AreEqual(style, silFont.Style);
        }
        /// <summary>
        /// Test the Doulos SIL font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void DoulosTest()
        {
            var silFont = new EmbeddedFont("Doulos SIL");
            Assert.IsTrue(EmbeddedFont.IsInstalled(silFont.Filename));
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
            const string weight = "normal";
            Assert.AreEqual(weight, silFont.Weight);
            const string style = "normal";
            Assert.AreEqual(style, silFont.Style);
        }
        /// <summary>
        /// Test the SIL Ezra font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void EzraTest()
        {
            var silFont = new EmbeddedFont("Ezra SIL");
            Assert.IsTrue(EmbeddedFont.IsInstalled(silFont.Filename));
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
            const string weight = "normal";
            Assert.AreEqual(weight, silFont.Weight);
            const string style = "normal";
            Assert.AreEqual(style, silFont.Style);
        }
        /// <summary>
        /// Test the SIL Galatia font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void GalatiaTest()
        {
            var silFont = new EmbeddedFont("Galatia SIL");
            Assert.IsTrue(EmbeddedFont.IsInstalled(silFont.Filename));
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
            const string weight = "normal";
            Assert.AreEqual(weight, silFont.Weight);
            const string style = "normal";
            Assert.AreEqual(style, silFont.Style);
        }
        /// <summary>
        /// Test the SIL Gentium Basic font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void GentiumTest()
        {
            var silFont = new EmbeddedFont("Gentium Basic");
            Assert.IsTrue(EmbeddedFont.IsInstalled(silFont.Filename));
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
            const string weight = "normal";
            Assert.AreEqual(weight, silFont.Weight);
            const string style = "normal";
            Assert.AreEqual(style, silFont.Style);
        }
        /// <summary>
        /// Test the SIL Lateef font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void LateefTest()
        {
            var silFont = new EmbeddedFont("Lateef");
            Assert.IsTrue(EmbeddedFont.IsInstalled(silFont.Filename));
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
            const string weight = "normal";
            Assert.AreEqual(weight, silFont.Weight);
            const string style = "normal";
            Assert.AreEqual(style, silFont.Style);
        }
        /// <summary>
        /// Test the SIL Nuosu (Yi) font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void NuosuTest()
        {
            var silFont = new EmbeddedFont("Nuosu");
            Assert.IsTrue(EmbeddedFont.IsInstalled(silFont.Filename));
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
            const string weight = "normal";
            Assert.AreEqual(weight, silFont.Weight);
            const string style = "normal";
            Assert.AreEqual(style, silFont.Style);
        }
        /// <summary>
        /// Test the SIL Padauk font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void PadaukTest()
        {
            var silFont = new EmbeddedFont("Padauk");
            Assert.IsTrue(EmbeddedFont.IsInstalled(silFont.Filename));
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
            const string weight = "normal";
            Assert.AreEqual(weight, silFont.Weight);
            const string style = "normal";
            Assert.AreEqual(style, silFont.Style);
        }
        /// <summary>
        /// Test the SIL Scheharazade) font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void ScheharazadeTest()
        {
            var silFont = new EmbeddedFont("Scheherazade");
            Assert.IsTrue(EmbeddedFont.IsInstalled(silFont.Filename));
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
            const string weight = "normal";
            Assert.AreEqual(weight, silFont.Weight);
            const string style = "normal";
            Assert.AreEqual(style, silFont.Style);
        }
        /// <summary>
        /// Test the SIL Sophia Nubian font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void SophiaNubianTest()
        {
            var silFont = new EmbeddedFont("Sophia Nubian");
            Assert.IsTrue(EmbeddedFont.IsInstalled(silFont.Filename));
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
            const string weight = "normal";
            Assert.AreEqual(weight, silFont.Weight);
            const string style = "normal";
            Assert.AreEqual(style, silFont.Style);
        }
        /// <summary>
        /// Test the SIL Tai Heritage Pro font
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void TaiHeritageProTest()
        {
            var silFont = new EmbeddedFont("Tai Heritage Pro");
            Assert.IsTrue(EmbeddedFont.IsInstalled(silFont.Filename));
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
            const string weight = "normal";
            Assert.AreEqual(weight, silFont.Weight);
            const string style = "normal";
            Assert.AreEqual(style, silFont.Style);
        }

    }
}