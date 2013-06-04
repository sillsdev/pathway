// --------------------------------------------------------------------------------------------
// <copyright file="DictionaryForMIDsRecTest.cs" from='2013' to='2013' company='SIL International'>
//      Copyright © 2013, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Test methods of DictionaryForMIDsRec
// </remarks>
// --------------------------------------------------------------------------------------------
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.DictionaryForMIDsConvert
{
    public class DictionaryForMIDsConvertRecTest : DictionaryForMIDsRec
    {
        #region Setup

        private TestFiles _testFiles;

        [TestFixtureSetUp]
        public void Setup()
        {
            _testFiles = new TestFiles("DictionaryForMIDsConvert");
        }

        #endregion Setup

        [Test]
        public void AddStyleTagTest()
        {
            PublicationInformation projInfo = new PublicationInformation();
            projInfo.DefaultXhtmlFileWithPath = _testFiles.Input("sena3-imba.xhtml");
            projInfo.DefaultCssFileWithPath = _testFiles.Input("sena3-imba.css");
            var cssTree = new CssTree();
            CssClass = cssTree.CreateCssProperty(projInfo.DefaultCssFileWithPath, true);
            var ContentStyles = new DictionaryForMIDsStyle();
            var rec = new DictionaryForMIDsRec {CssClass = CssClass, Styles = ContentStyles};
            var input = new DictionaryForMIDsInput(projInfo);
            var node = input.SelectNodes("//*[@class = 'partofspeech']//text()")[0];
            rec.AddStyleTag(node);
            Assert.AreEqual(2, ContentStyles.NumStyles);
        }

        [Test]
        public void AddStyleTagLangTest()
        {
            PublicationInformation projInfo = new PublicationInformation();
            projInfo.DefaultXhtmlFileWithPath = _testFiles.Input("wasp.xhtml");
            projInfo.DefaultCssFileWithPath = _testFiles.Input("wasp.css");
            var cssTree = new CssTree();
            CssClass = cssTree.CreateCssProperty(projInfo.DefaultCssFileWithPath, true);
            var ContentStyles = new DictionaryForMIDsStyle();
            var rec = new DictionaryForMIDsRec { CssClass = CssClass, Styles = ContentStyles };
            var input = new DictionaryForMIDsInput(projInfo);
            var node = input.SelectNodes("(//*[@class='xitem'])/*")[1];
            rec.AddStyleTag(node);
            Assert.AreEqual(2, ContentStyles.NumStyles);
            Assert.AreEqual("153,51,102", rec.Styles.FontColor(2));
        }

        [Test]
        public void AddAfterTest()
        {
            PublicationInformation projInfo = new PublicationInformation();
            projInfo.DefaultXhtmlFileWithPath = _testFiles.Input("sena3-ipa.xhtml");
            projInfo.DefaultCssFileWithPath = _testFiles.Input("sena3-ipa.css");
            var cssTree = new CssTree();
            CssClass = cssTree.CreateCssProperty(projInfo.DefaultCssFileWithPath, true);
            var ContentStyles = new DictionaryForMIDsStyle();
            var rec = new DictionaryForMIDsRec { CssClass = CssClass, Styles = ContentStyles };
            var input = new DictionaryForMIDsInput(projInfo);
            var node = input.SelectNodes("//*[@class = 'xsensenumber']")[0];
            rec.AddAfter(node);
            Assert.AreEqual(") ", rec.Rec);
        }
}
}
