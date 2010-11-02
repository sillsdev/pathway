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
            Assert.IsFalse(arialFont.SILFont);
            const string fontFilename = "arial.ttf";
            Assert.AreEqual(fontFilename, arialFont.Filename);
        }

        /// <summary>
        /// Test the SIL Abyssinica font
        /// </summary>
        [Test]
        public void AbyssinicaTest()
        {
            var silFont = new EmbeddedFont("Abyssinica");
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
            const string weight = "regular";
            Assert.AreEqual(weight, silFont.Weight);
            const string style = "regular";
            Assert.AreEqual(style, silFont.Style);
            const string fontFilename = "Abyssinica_SIL.ttf";
            Assert.AreEqual(fontFilename, silFont.Filename);
        }

        /// <summary>
        /// Test the SIL Andika font
        /// </summary>
        [Test]
        public void AndikaTest()
        {
            var silFont = new EmbeddedFont("Andika");
            Assert.IsTrue(silFont.SILFont);
            Assert.IsFalse(silFont.Serif);
            const string weight = "regular";
            Assert.AreEqual(weight, silFont.Weight);
            const string style = "regular";
            Assert.AreEqual(style, silFont.Style);
            const string fontFilename = "AndBasR.ttf";
            Assert.AreEqual(fontFilename, silFont.Filename);
        }

        /// <summary>
        /// Test the Apparatus SIL font
        /// </summary>
        [Test]
        public void ApparatusTest()
        {
            var silFont = new EmbeddedFont("Apparatus SIL");
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
            const string weight = "regular";
            Assert.AreEqual(weight, silFont.Weight);
            const string style = "regular";
            Assert.AreEqual(style, silFont.Style);
            const string fontFilename = "AppSILR.ttf";
            Assert.AreEqual(fontFilename, silFont.Filename);
        }
        /// <summary>
        /// Test the Charis SIL font
        /// </summary>
        [Test]
        public void CharisTest()
        {
            var silFont = new EmbeddedFont("Charis SIL");
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
            const string weight = "regular";
            Assert.AreEqual(weight, silFont.Weight);
            const string style = "regular";
            Assert.AreEqual(style, silFont.Style);
            const string fontFilename = "CharisSILR.ttf";
            Assert.AreEqual(fontFilename, silFont.Filename);
        }
        /// <summary>
        /// Test the Dai Banna font
        /// </summary>
        [Test]
        public void DaiBannaTest()
        {
            var silFont = new EmbeddedFont("Dai Banna");
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
            const string weight = "regular";
            Assert.AreEqual(weight, silFont.Weight);
            const string style = "regular";
            Assert.AreEqual(style, silFont.Style);
            const string fontFilename = "DBSILBR.ttf";
            Assert.AreEqual(fontFilename, silFont.Filename);
        }
        /// <summary>
        /// Test the Doulos SIL font
        /// </summary>
        [Test]
        public void DoulosTest()
        {
            var silFont = new EmbeddedFont("Doulos SIL");
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
            const string weight = "regular";
            Assert.AreEqual(weight, silFont.Weight);
            const string style = "regular";
            Assert.AreEqual(style, silFont.Style);
            const string fontFilename = "DoulosSILR.ttf";
            Assert.AreEqual(fontFilename, silFont.Filename);
        }
        /// <summary>
        /// Test the SIL Ezra font
        /// </summary>
        [Test]
        public void EzraTest()
        {
            var silFont = new EmbeddedFont("Ezra");
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
            const string weight = "regular";
            Assert.AreEqual(weight, silFont.Weight);
            const string style = "regular";
            Assert.AreEqual(style, silFont.Style);
            const string fontFilename = "SILEOT_0.ttf";
            Assert.AreEqual(fontFilename, silFont.Filename);
        }
        /// <summary>
        /// Test the SIL Galatia font
        /// </summary>
        [Test]
        public void GalatiaTest()
        {
            var silFont = new EmbeddedFont("Galatia");
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
            const string weight = "regular";
            Assert.AreEqual(weight, silFont.Weight);
            const string style = "regular";
            Assert.AreEqual(style, silFont.Style);
            const string fontFilename = "GalSILR201.ttf";
            Assert.AreEqual(fontFilename, silFont.Filename);
        }
        /// <summary>
        /// Test the SIL Gentium font
        /// </summary>
        [Test]
        public void GentiumTest()
        {
            var silFont = new EmbeddedFont("Gentium");
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
            const string weight = "regular";
            Assert.AreEqual(weight, silFont.Weight);
            const string style = "regular";
            Assert.AreEqual(style, silFont.Style);
            const string fontFilename = "GenBasR.ttf";
            Assert.AreEqual(fontFilename, silFont.Filename);
        }
        /// <summary>
        /// Test the SIL Lateef font
        /// </summary>
        [Test]
        public void LateefTest()
        {
            var silFont = new EmbeddedFont("Lateef");
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
            const string weight = "regular";
            Assert.AreEqual(weight, silFont.Weight);
            const string style = "regular";
            Assert.AreEqual(style, silFont.Style);
            const string fontFilename = "LateefRegOT.ttf";
            Assert.AreEqual(fontFilename, silFont.Filename);
        }
        /// <summary>
        /// Test the SIL Nuosu (Yi) font
        /// </summary>
        [Test]
        public void NuosuTest()
        {
            var silFont = new EmbeddedFont("Nuosu");
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
            const string weight = "regular";
            Assert.AreEqual(weight, silFont.Weight);
            const string style = "regular";
            Assert.AreEqual(style, silFont.Style);
            const string fontFilename = "NuosuSIL.ttf";
            Assert.AreEqual(fontFilename, silFont.Filename);
        }
        /// <summary>
        /// Test the SIL Padauk font
        /// </summary>
        [Test]
        public void PadaukTest()
        {
            var silFont = new EmbeddedFont("Padauk");
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
            const string weight = "regular";
            Assert.AreEqual(weight, silFont.Weight);
            const string style = "regular";
            Assert.AreEqual(style, silFont.Style);
            const string fontFilename = "Padauk.ttf";
            Assert.AreEqual(fontFilename, silFont.Filename);
        }
        /// <summary>
        /// Test the SIL Scheharazade) font
        /// </summary>
        [Test]
        public void ScheharazadeTest()
        {
            var silFont = new EmbeddedFont("Scheharazade");
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
            const string weight = "regular";
            Assert.AreEqual(weight, silFont.Weight);
            const string style = "regular";
            Assert.AreEqual(style, silFont.Style);
            const string fontFilename = "ScheherazadeRegOT.ttf";
            Assert.AreEqual(fontFilename, silFont.Filename);
        }
        /// <summary>
        /// Test the SIL Sophia Nubian font
        /// </summary>
        [Test]
        public void SophiaNubianTest()
        {
            var silFont = new EmbeddedFont("Sophia Nubian");
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
            const string weight = "regular";
            Assert.AreEqual(weight, silFont.Weight);
            const string style = "regular";
            Assert.AreEqual(style, silFont.Style);
            const string fontFilename = "SNR.ttf";
            Assert.AreEqual(fontFilename, silFont.Filename);
        }
        /// <summary>
        /// Test the SIL Tai Heritage Pro font
        /// </summary>
        [Test]
        public void TaiHeritageProTest()
        {
            var silFont = new EmbeddedFont("Tai Heritage Pro");
            Assert.IsTrue(silFont.SILFont);
            Assert.IsTrue(silFont.Serif);
            const string weight = "regular";
            Assert.AreEqual(weight, silFont.Weight);
            const string style = "regular";
            Assert.AreEqual(style, silFont.Style);
            const string fontFilename = "TaiHeritagePro.ttf";
            Assert.AreEqual(fontFilename, silFont.Filename);
        }

    }
}