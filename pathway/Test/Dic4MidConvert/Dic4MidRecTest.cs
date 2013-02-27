// --------------------------------------------------------------------------------------------
// <copyright file="Dic4MidRecTest.cs" from='2013' to='2013' company='SIL International'>
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
// Test methods of Dic4MidRec
// </remarks>
// --------------------------------------------------------------------------------------------
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.Dic4MidConvert
{
    public class Dic4MidRecTest : Dic4MidRec
    {
        #region Setup

        private TestFiles _testFiles;

        [TestFixtureSetUp]
        public void Setup()
        {
            _testFiles = new TestFiles("Dic4MidConvert");
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
            var ContentStyles = new Dic4MidStyle();
            var rec = new Dic4MidRec {CssClass = CssClass, Styles = ContentStyles};
            var input = new Dic4MidInput(projInfo);
            var node = input.SelectNodes("//*[@class = 'partofspeech']//text()")[0];
            rec.AddStyleTag(node);
            Assert.AreEqual(2, ContentStyles.NumStyles);
        }

        [Test]
        public void AddAfterTest()
        {
            PublicationInformation projInfo = new PublicationInformation();
            projInfo.DefaultXhtmlFileWithPath = _testFiles.Input("sena3-ipa.xhtml");
            projInfo.DefaultCssFileWithPath = _testFiles.Input("sena3-ipa.css");
            var cssTree = new CssTree();
            CssClass = cssTree.CreateCssProperty(projInfo.DefaultCssFileWithPath, true);
            var ContentStyles = new Dic4MidStyle();
            var rec = new Dic4MidRec { CssClass = CssClass, Styles = ContentStyles };
            var input = new Dic4MidInput(projInfo);
            var node = input.SelectNodes("//*[@class = 'xsensenumber']")[0];
            rec.AddAfter(node);
            Assert.AreEqual(") ", rec.Rec);
        }
}
}
