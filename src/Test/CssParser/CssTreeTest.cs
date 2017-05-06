// --------------------------------------------------------------------------------------------
// <copyright file="CSSTreeTest.cs" from='2009' to='2014' company='SIL International'>
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
// Tests for Css Parser 
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.CssParserTest
{
    [TestFixture]
    public class CSSTreeTest : CssTree
    {
        #region Private Variables
        private Dictionary<string, Dictionary<string, string>> _output;
        private Dictionary<string, Dictionary<string, string>> _expected;
        private Dictionary<string, string> _expectedProperty;
        private string _inputCSSFileWithPath;
        private string _testFolderPath;
        private string _errorMsg;
        #endregion Private Variables

        #region Setup
        [TestFixtureSetUp]
        protected void SetUp()
        {
            _output = new Dictionary<string, Dictionary<string, string>>();
            _expected = new Dictionary<string, Dictionary<string, string>>();
            _testFolderPath = PathPart.Bin(Environment.CurrentDirectory, "/CssParser/TestFiles/cssInput");
        }
        #endregion Setup
        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTC")]
        public void GetFontList()
        {
            string cssFile = "GetFontList.css";
            _inputCSSFileWithPath = Common.PathCombine(_testFolderPath, cssFile);
            ArrayList fontList = GetFontList(_inputCSSFileWithPath);
            for (int i = 0; i < fontList.Count; i += 1)
                fontList[i] = fontList[i].ToString().ToUpper();
            ArrayList expectedFontList = new ArrayList();
			if (Common.UsingMonoVM)
			{
				// Microsoft font packaging for linux has some different file names (and less choices)
				expectedFontList.Add("ARIAL_BLACK.TTF");
				expectedFontList.Add("ARIAL.TTF");
				expectedFontList.Add("ARIAL_BOLD.TTF");
				expectedFontList.Add("ARIAL_ITALIC.TTF");
				expectedFontList.Add("ARIALBI.TTF");
				expectedFontList.Add("TIMES.TTF");
				expectedFontList.Add("TIMESBD.TTF");
				expectedFontList.Add("TIMESI.TTF");
				expectedFontList.Add("TIMES_NEW_ROMAN_BOLD_ITALIC.TTF");
			}
			else 
			{
	            expectedFontList.Add("ARIAL.TTF");
	            expectedFontList.Add("ARIALBD.TTF");
	            expectedFontList.Add("ARIALBI.TTF");
	            expectedFontList.Add("ARIALI.TTF");
	            expectedFontList.Add("ARIBLK.TTF");
	            expectedFontList.Add("ARIALN.TTF");
	            expectedFontList.Add("ARLRDBD.TTF");
	            expectedFontList.Add("ARIALNB.TTF");
	            expectedFontList.Add("ARIALNBI.TTF");
	            expectedFontList.Add("ARIALNI.TTF");
	            expectedFontList.Add("ARIALUNI.TTF");
	            expectedFontList.Add("TIMES.TTF");
	            expectedFontList.Add("TIMESBD.TTF");
	            expectedFontList.Add("TIMESBI.TTF");
	            expectedFontList.Add("TIMESI.TTF");
			}
			// sort both lists to line them up
            expectedFontList.Sort();
			fontList.Sort();
            CollectionAssert.AreEqual(expectedFontList, fontList, "GetFontList Error");
        }


        [Test]
        public void PageClassMirror()
        {
            string cssFile = "pageMirror.css";
            _inputCSSFileWithPath = Common.PathCombine(_testFolderPath, cssFile);
            _output = CreateCssProperty(_inputCSSFileWithPath, true);
            _expected.Clear();
            // @page
            _expectedProperty = new Dictionary<string, string>();
            _expected["@page"] = _expectedProperty;
            _expectedProperty["margin-top"] = "2";
            _expectedProperty["margin-right"] = "4";
            _expectedProperty["margin-bottom"] = "6";
            _expectedProperty["margin-left"] = "8";
            _expectedProperty["page-height"] = "792";
            _expectedProperty["page-width"] = "612";
            _expectedProperty["mirror"] = "true";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page-top-left"] = _expectedProperty;
            _expectedProperty["font-family"] = "Arial";
            _expectedProperty["font-size"] = "2";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page-top-center"] = _expectedProperty;
            _expectedProperty["font-family"] = "Courier";
            _expectedProperty["font-size"] = "3";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page-top-right"] = _expectedProperty;
            _expectedProperty["font-family"] = "Georgia";
            _expectedProperty["font-size"] = "4";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page-bottom-left"] = _expectedProperty;
            _expectedProperty["font-family"] = "Kartika";
            _expectedProperty["font-size"] = "5";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page-bottom-center"] = _expectedProperty;
            _expectedProperty["font-family"] = "Latha";
            _expectedProperty["font-size"] = "6";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page-bottom-right"] = _expectedProperty;
            _expectedProperty["font-family"] = "System";
            _expectedProperty["font-size"] = "7";

            // @page :first 
            _expectedProperty = new Dictionary<string, string>();
            _expected["@page:first"] = _expectedProperty;
            _expectedProperty["margin-top"] = "2";
            _expectedProperty["margin-right"] = "4";
            _expectedProperty["margin-bottom"] = "6";
            _expectedProperty["margin-left"] = "8";
            _expectedProperty["page-height"] = "792";
            _expectedProperty["page-width"] = "612";
            _expectedProperty["mirror"] = "false";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page:first-top-left"] = _expectedProperty;
            _expectedProperty["font-family"] = "Arial";
            _expectedProperty["font-size"] = "2";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page:first-top-center"] = _expectedProperty;
            _expectedProperty["font-family"] = "Courier";
            _expectedProperty["font-size"] = "3";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page:first-top-right"] = _expectedProperty;
            _expectedProperty["font-family"] = "Georgia";
            _expectedProperty["font-size"] = "4";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page:first-bottom-left"] = _expectedProperty;
            _expectedProperty["font-family"] = "Kartika";
            _expectedProperty["font-size"] = "5";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page:first-bottom-center"] = _expectedProperty;
            _expectedProperty["font-family"] = "Latha";
            _expectedProperty["font-size"] = "6";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page:first-bottom-right"] = _expectedProperty;
            _expectedProperty["font-family"] = "System";
            _expectedProperty["font-size"] = "7";

            // @page:left :left 
            _expectedProperty = new Dictionary<string, string>();
            _expected["@page:left"] = _expectedProperty;
            _expectedProperty["margin-top"] = "2";
            _expectedProperty["margin-right"] = "4";
            _expectedProperty["margin-bottom"] = "6";
            _expectedProperty["margin-left"] = "8";
            _expectedProperty["page-height"] = "792";
            _expectedProperty["page-width"] = "612";
            _expectedProperty["mirror"] = "true";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page:left-top-left"] = _expectedProperty;
            _expectedProperty["font-family"] = "Arial";
            _expectedProperty["font-size"] = "2";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page:left-top-center"] = _expectedProperty;
            _expectedProperty["font-family"] = "Courier";
            _expectedProperty["font-size"] = "3";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page:left-top-right"] = _expectedProperty;
            _expectedProperty["font-family"] = "Georgia";
            _expectedProperty["font-size"] = "4";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page:left-bottom-left"] = _expectedProperty;
            _expectedProperty["font-family"] = "Kartika";
            _expectedProperty["font-size"] = "5";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page:left-bottom-center"] = _expectedProperty;
            _expectedProperty["font-family"] = "Latha";
            _expectedProperty["font-size"] = "6";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page:left-bottom-right"] = _expectedProperty;
            _expectedProperty["font-family"] = "System";
            _expectedProperty["font-size"] = "7";

            // @page :right
            _expectedProperty = new Dictionary<string, string>();
            _expected["@page:right"] = _expectedProperty;
            _expectedProperty["margin-top"] = "2";
            _expectedProperty["margin-right"] = "4";
            _expectedProperty["margin-bottom"] = "6";
            _expectedProperty["margin-left"] = "8";
            _expectedProperty["page-height"] = "792";
            _expectedProperty["page-width"] = "612";
            _expectedProperty["mirror"] = "true";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page:right-top-left"] = _expectedProperty;
            _expectedProperty["font-family"] = "Arial";
            _expectedProperty["font-size"] = "2";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page:right-top-center"] = _expectedProperty;
            _expectedProperty["font-family"] = "Courier";
            _expectedProperty["font-size"] = "3";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page:right-top-right"] = _expectedProperty;
            _expectedProperty["font-family"] = "Georgia";
            _expectedProperty["font-size"] = "4";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page:right-bottom-left"] = _expectedProperty;
            _expectedProperty["font-family"] = "Kartika";
            _expectedProperty["font-size"] = "5";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page:right-bottom-center"] = _expectedProperty;
            _expectedProperty["font-family"] = "Latha";
            _expectedProperty["font-size"] = "6";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page:right-bottom-right"] = _expectedProperty;
            _expectedProperty["font-family"] = "System";
            _expectedProperty["font-size"] = "7";

            // letdata
            _expectedProperty = new Dictionary<string, string>();
            _expected["letData"] = _expectedProperty;
            _expectedProperty["font-size"] = "24";

            Assert.IsTrue(CompareNestetedDictionary(), _errorMsg + " test Failed");
        }

        [Test]
        public void PageClassFull()
        {
            string cssFile = "page.css";
            _inputCSSFileWithPath = Common.PathCombine(_testFolderPath, cssFile);
            _output = CreateCssProperty(_inputCSSFileWithPath, true);
            _expected.Clear();
            // @page
            _expectedProperty = new Dictionary<string, string>();
            _expected["@page"] = _expectedProperty;
            _expectedProperty["margin-top"] = "2";
            _expectedProperty["margin-right"] = "4";
            _expectedProperty["margin-bottom"] = "6";
            _expectedProperty["margin-left"] = "8";
            _expectedProperty["page-height"] = "792";
            _expectedProperty["page-width"] = "612";
            _expectedProperty["mirror"] = "false";


            _expectedProperty = new Dictionary<string, string>();
            _expected["@page-top-left"] = _expectedProperty;
            _expectedProperty["font-family"] = "Arial";
            _expectedProperty["font-size"] = "2";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page-top-center"] = _expectedProperty;
            _expectedProperty["font-family"] = "Courier";
            _expectedProperty["font-size"] = "3";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page-top-right"] = _expectedProperty;
            _expectedProperty["font-family"] = "Georgia";
            _expectedProperty["font-size"] = "4";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page-bottom-left"] = _expectedProperty;
            _expectedProperty["font-family"] = "Kartika";
            _expectedProperty["font-size"] = "5";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page-bottom-center"] = _expectedProperty;
            _expectedProperty["font-family"] = "Latha";
            _expectedProperty["font-size"] = "6";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page-bottom-right"] = _expectedProperty;
            _expectedProperty["font-family"] = "System";
            _expectedProperty["font-size"] = "7";

            // @page :first 
            _expectedProperty = new Dictionary<string, string>();
            _expected["@page:first"] = _expectedProperty;
            _expectedProperty["margin-top"] = "2";
            _expectedProperty["margin-right"] = "4";
            _expectedProperty["margin-bottom"] = "6";
            _expectedProperty["margin-left"] = "8";
            _expectedProperty["page-height"] = "792";
            _expectedProperty["page-width"] = "612";
            _expectedProperty["mirror"] = "false";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page:first-top-left"] = _expectedProperty;
            _expectedProperty["font-family"] = "Arial";
            _expectedProperty["font-size"] = "2";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page:first-top-center"] = _expectedProperty;
            _expectedProperty["font-family"] = "Courier";
            _expectedProperty["font-size"] = "3";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page:first-top-right"] = _expectedProperty;
            _expectedProperty["font-family"] = "Georgia";
            _expectedProperty["font-size"] = "4";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page:first-bottom-left"] = _expectedProperty;
            _expectedProperty["font-family"] = "Kartika";
            _expectedProperty["font-size"] = "5";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page:first-bottom-center"] = _expectedProperty;
            _expectedProperty["font-family"] = "Latha";
            _expectedProperty["font-size"] = "6";

            _expectedProperty = new Dictionary<string, string>();
            _expected["@page:first-bottom-right"] = _expectedProperty;
            _expectedProperty["font-family"] = "System";
            _expectedProperty["font-size"] = "7";
            Assert.IsTrue(CompareNestetedDictionary(), _errorMsg + " test Failed");
        }
        private bool CompareNestetedDictionary()
        {
            bool compare = true;
            foreach (KeyValuePair<string, Dictionary<string, string>> className in _expected)
            {
                foreach (KeyValuePair<string, string> property in className.Value)
                {
					try
					{
                    if (property.Value != _output[className.Key][property.Key])
						{
							var e = new InvalidXmlException();
							e.Property = property;
							e.ClassName = className;
							throw e;
						}
					}
				    catch
                    {
                        _errorMsg = className.Key + "-" + property.Key + ":" + property.Value;
						if (!_output.ContainsKey(className.Key))
						{
							_errorMsg = className.Key + " Key is missing";
							foreach (string k in _output.Keys)
								_errorMsg += "\n " + k;
						}
						else if (!_output[className.Key].ContainsKey(property.Key))
							_errorMsg = property.Key + " Key is missing";
						else
						{
							_errorMsg += " Value is:" + _output[className.Key][property.Key];
						}
                        compare = false;
                        return compare;
                    }
                }
            }
            return compare;
        }
	    public class InvalidXmlException : Exception
	    {
	        public KeyValuePair<string, Dictionary<string, string>> ClassName { get; set; }
	        public KeyValuePair<string, string> Property { get; set; }
	        
	        public override string ToString()
	        {
	            return string.Format("{0} of {1} not valid", Property.Key, ClassName.Key);
	        }
	    }
    }
}
