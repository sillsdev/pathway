// --------------------------------------------------------------------------------------------
// <copyright file="PostscriptLanguageTest.cs" from='2010' to='2010' company='SIL International'>
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
// Test methods of ExportXeTeX
// </remarks>
// --------------------------------------------------------------------------------------------

using System.IO;
using SIL.PublishingSolution;
using NUnit.Framework;
using System.Collections.Generic;
using SIL.Tool;

namespace Test.XeTeXConvert
{
    /// <summary>
    ///This is a test class for PostscriptLanguageTest and is intended
    ///to contain all PostscriptLanguageTest Unit Tests
    ///</summary>
    [TestFixture]
    public class PostscriptLanguageTest : PostscriptLanguage
    {
        /// <summary>
        ///A test for TexFont
        ///</summary>
        [Test]
        public void TexFontTest()
        {
            PostscriptLanguage target = new PostscriptLanguage();
            int num = 1;
            string expected = "CharisSIL";
            string actual = target.TexFont(num);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SetLangClass
        ///</summary>
        [Test]
        public void SetLangClassTest()
        {
            PostscriptLanguage target = new PostscriptLanguage();
            string lang = "bzh";
            string cssClass = "headword";
            target.SetLangClass(lang, cssClass);
        }

        /// <summary>
        ///A test for SaveCache
        ///</summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void SaveCacheTest()
        {
            PostscriptLanguage target = new PostscriptLanguage();
            target.SaveCache();
            var destinationDir = Common.PathCombine(Common.GetAllUserPath(), "cache/CharisSIL");
            Directory.Delete(destinationDir, true);
        }

        /// <summary>
        ///A test for RestoreCache
        ///</summary>
        [Test]
        public void RestoreCacheTest()
        {
            PostscriptLanguage target = new PostscriptLanguage();
            target.RestoreCache();
        }

        /// <summary>
        ///A test for LangXpwFont
        ///</summary>
        [Test]
        public void LangXpwFontTest()
        {
            PostscriptLanguage target = new PostscriptLanguage(); // TODO: Initialize to an appropriate value
            string lang = "cmn";
            var selector = "xitem_." + lang;
            var postscriptName = "SimSung-18030";
            var isGraphite = true;
            target.SetLangClass(lang, selector);
            target.SetClass2Postscript(selector, postscriptName);
            target.AddPostscriptName(postscriptName, isGraphite);
            string actual = target.LangXpwFont(lang);
            string expected = @"\fontb ";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsGraphite
        ///</summary>
        [Test]
        public void IsGraphiteTest()
        {
            PostscriptLanguage target = new PostscriptLanguage();
            int num = 1;
            string expected = "G";
            string actual = target.IsGraphite(num);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetAllFontNames
        ///</summary>
        [Test]
        public void GetAllFontNamesTest()
        {
            string expected = "CharisSIL";
            string actual = GetAllFontNames();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ClassPostscriptName
        ///</summary>
        [Test]
        public void ClassPostscriptNameTest()
        {
            PostscriptLanguage target = new PostscriptLanguage();
            string className = "headword";
            string style = "Regular";
            Dictionary<string, Dictionary<string, string>> cssProperty = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> headwordProperty = new Dictionary<string, string>();
            headwordProperty["font-family"] = "Times New Roman";
            cssProperty[className] = headwordProperty;
            target.ClassPostscriptName(className, style, cssProperty);
            var lang = "tpi";
            target.SetLangClass(lang, className);
            target.SetClass2Postscript(className, "TimesNewRomanPSMT");
            string actual = target.LangXpwFont(lang);
            string expected = @"\fontb ";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for AccumulatePostscriptNames
        ///</summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void AccumulatePostscriptNamesTest()
        {
            const string lang1 = "bzh";
            const string className1 = "headword";
            const string lang2 = "tpi";
            const string className2 = "definition";
            var target = new PostscriptLanguage();
            var cssProperty = new Dictionary<string, Dictionary<string, string>>();
            var headwordProperty = new Dictionary<string, string>();
            headwordProperty["font-family"] = "Charis SIL";
            cssProperty[className1] = headwordProperty;
            var definitionProperty = new Dictionary<string, string>();
            definitionProperty["font-family"] = "Times New Roman";
            cssProperty[className2] = definitionProperty;
            target.AccumulatePostscriptNames(cssProperty);
            target.SetLangClass(lang1, className1);
            target.SetClass2Postscript(className1, "CharisSIL");
            string actual1 = target.LangXpwFont(lang1);
            const string expected1 = @"\fonta ";
            Assert.AreEqual(expected1, actual1);
            target.SetLangClass(lang2, className2);
            target.SetClass2Postscript(className2, "TimesNewRomanPSMT");
            string actual2 = target.LangXpwFont(lang2);
            const string expected2 = @"\fontb ";
            Assert.AreEqual(expected2, actual2);
        }
    }
}