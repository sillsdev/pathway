// --------------------------------------------------------------------------------------------
// <copyright file="InStylesTest2.cs" from='2009' to='2014' company='SIL International'>
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
// Test Cases for InDesign StylesTest
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Xml;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.InDesignConvert
{
	[TestFixture]
	public class InStylesTest2 : ValidateXMLFile
	{
		#region Private Variables
		private Dictionary<string, string> _expected = new Dictionary<string, string>();
		private string _testFolderPath = string.Empty;
		private InStyles _stylesXML;
		private InStory _storyXML;
		private string _outputPath;
		private string _outputStyles;
		private string _fileNameWithPath;
		private Dictionary<string, Dictionary<string, string>> _idAllClass = new Dictionary<string, Dictionary<string, string>>();
		PublicationInformation projInfo = new PublicationInformation();
		#endregion

		#region Public Variables
		private Dictionary<string, Dictionary<string, string>> _cssProperty;
		private CssTree _cssTree;
		#endregion

		#region Setup
		[TestFixtureSetUp]
		protected void SetUp()
		{
			_stylesXML = new InStyles();
			_storyXML = new InStory();
			_testFolderPath = PathPart.Bin(Environment.CurrentDirectory, "/InDesignConvert/TestFiles");
			ClassProperty = _expected;  //Note: All Reference address initialized here
			_outputPath = Common.PathCombine(_testFolderPath, "output");
			_outputStyles = Common.PathCombine(_outputPath, "Resources");
			projInfo.TempOutputFolder = _outputPath;
			_cssProperty = new Dictionary<string, Dictionary<string, string>>();
			_cssTree = new CssTree();
		}
		#endregion Setup

		#region Public Functions

		#region Visibility
		[Test]
		[Category("SkipOnTC")]
		public void Visibility()
		{
			string _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/Visibility.xhtml");
			string _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/Visibility.css");
			_cssProperty = _cssTree.CreateCssProperty(_inputCSS, true);
			_idAllClass = _stylesXML.CreateIDStyles(_outputStyles, _cssProperty);
			projInfo.DefaultXhtmlFileWithPath = _inputXHTML;
			_storyXML.CreateStory(projInfo, _idAllClass, _cssTree.SpecificityClass, _cssTree.CssClassOrder);
			const string classname = "a_1";
			XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + classname + "\"]";
			string fileNameWithPath = Common.PathCombine(_outputStyles, "Styles.xml");
			XmlNodeList nodesList = Common.GetXmlNodeListInDesignNamespace(fileNameWithPath, XPath);
			XmlNode node = nodesList[0];
			XmlAttributeCollection attrb = node.Attributes;
			string result = attrb["FillColor"].Value;
			Assert.AreEqual("Color/Paper", result, classname + " test Failed");
		}
		#endregion

		#region LineHeight
		[Test]
		[Category("SkipOnTC")]
		public void LineHeight1()
		{
			string _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/LineHeight.xhtml");
			string _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/LineHeight.css");
			_cssProperty = _cssTree.CreateCssProperty(_inputCSS, true);
			_idAllClass = _stylesXML.CreateIDStyles(_outputStyles, _cssProperty);
			projInfo.DefaultXhtmlFileWithPath = _inputXHTML;
			_storyXML.CreateStory(projInfo, _idAllClass, _cssTree.SpecificityClass, _cssTree.CssClassOrder);

			string classname = "entry1_1";
			string _xPath = "//RootParagraphStyleGroup[1]/ParagraphStyle[@Name = \"" + classname + "\"]/Properties[1]/Leading[1]";
			_fileNameWithPath = Common.PathCombine(_outputStyles, "Styles.xml");
			XmlNode node = Common.GetXmlNodeInDesignNamespace(_fileNameWithPath, _xPath);
			string result = node.InnerText;
			Assert.AreEqual(result, "28", classname + "test failed");

			classname = "entry2_1";
			_xPath = "//RootParagraphStyleGroup[1]/ParagraphStyle[@Name = \"" + classname + "\"]/Properties[1]/Leading[1]";
			_fileNameWithPath = Common.PathCombine(_outputStyles, "Styles.xml");
			node = Common.GetXmlNodeInDesignNamespace(_fileNameWithPath, _xPath);
			result = node.InnerText;
			Assert.AreEqual(result, "14", classname + "test failed");

			classname = "entry3_1";
			_xPath = "//RootParagraphStyleGroup[1]/ParagraphStyle[@Name = \"" + classname + "\"]/Properties[1]/Leading[1]";
			_fileNameWithPath = Common.PathCombine(_outputStyles, "Styles.xml");
			node = Common.GetXmlNodeInDesignNamespace(_fileNameWithPath, _xPath);
			result = node.InnerText;
			Assert.AreEqual(result, "28", classname + "test failed");

			classname = "entry4_1";
			_xPath = "//RootParagraphStyleGroup[1]/ParagraphStyle[@Name = \"" + classname + "\"]/Properties[1]/Leading[1]";
			_fileNameWithPath = Common.PathCombine(_outputStyles, "Styles.xml");
			node = Common.GetXmlNodeInDesignNamespace(_fileNameWithPath, _xPath);
			result = node.InnerText;
			Assert.AreEqual(result, "24", classname + "test failed");

			classname = "entry5_1";
			_xPath = "//RootParagraphStyleGroup[1]/ParagraphStyle[@Name = \"" + classname + "\"]/Properties[1]/Leading[1]";
			_fileNameWithPath = Common.PathCombine(_outputStyles, "Styles.xml");
			node = Common.GetXmlNodeInDesignNamespace(_fileNameWithPath, _xPath);
			result = node.InnerText;
			Assert.AreEqual(result, "28", classname + "test failed");
		}

		[Test]
		[Category("SkipOnTC")]
		public void LineHeight2()
		{
			string _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/LineHeight.xhtml");
			string _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/LineHeight.css");
			_cssProperty = _cssTree.CreateCssProperty(_inputCSS, true);
			_idAllClass = _stylesXML.CreateIDStyles(_outputStyles, _cssProperty);
			projInfo.DefaultXhtmlFileWithPath = _inputXHTML;
			_storyXML.CreateStory(projInfo, _idAllClass, _cssTree.SpecificityClass, _cssTree.CssClassOrder);

			string classname = "entry6_1";
			string _xPath = "//RootParagraphStyleGroup[1]/ParagraphStyle[@Name = \"" + classname + "\"]/Properties[1]/Leading[1]";
			_fileNameWithPath = Common.PathCombine(_outputStyles, "Styles.xml");
			XmlNode node = Common.GetXmlNodeInDesignNamespace(_fileNameWithPath, _xPath);
			string result = node.Attributes["type"].Value;
			result = result + "_" + node.InnerText;
			Assert.AreEqual(result, "unit_14", classname + "test failed");
		}
		#endregion

		#endregion
	}
}
