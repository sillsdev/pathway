// --------------------------------------------------------------------------------------------
// <copyright file="ContentXMLTest.cs" from='2009' to='2014' company='SIL International'>
//      Copyright (C) 2014, SIL International. All Rights Reserved.
//
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed:
//
// <remarks>
// </remarks>
// --------------------------------------------------------------------------------------------

#region Using
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using NUnit.Framework;
using System.Windows.Forms;
using SIL.PublishingSolution;
using SIL.Tool;

#endregion Using

namespace Test.OpenOfficeConvert
{
	[TestFixture]
	[Category("BatchTest")]
	public class LOContentExportTest
	{
		#region Private Variables

		private string _inputPath;
		private string _outputPath;
		private string _expectedPath;
		private string _expectedlinuxPath;
		ProgressBar _progressBar;
		protected TimeSpan _totalTime;
		private PublicationInformation _projInfo;

		private ValidateXMLFile _validate;
		private string _styleFile;
		private string _contentFile;
		private int _index = 0;
		private bool _isLinux = false;
		private static string _outputBasePath = string.Empty;
		#endregion Private Variables

		#region SetUp

		[TestFixtureSetUp]
		protected void SetUp()
		{
			Common.Testing = true;
			_projInfo = new PublicationInformation();
			_progressBar = new ProgressBar();
			string testPath = PathPart.Bin(Environment.CurrentDirectory, "/OpenOfficeConvert/TestFiles");
			_inputPath = Common.PathCombine(testPath, "input");
			_outputPath = Common.PathCombine(testPath, "output");
			_expectedPath = Common.PathCombine(testPath, "expected");
			_expectedlinuxPath = Common.PathCombine(testPath, "expectedlinux");
			Common.DeleteDirectory(_outputPath);
			Directory.CreateDirectory(_outputPath);
			FolderTree.Copy(FileInput("Pictures"), FileOutput("Pictures"));
			_projInfo.ProgressBar = _progressBar;
			_projInfo.OutputExtension = "odt";
			_projInfo.ProjectInputType = "Dictionary";
			_projInfo.IsFrontMatterEnabled = false;
			_projInfo.FinalOutput = "odt";
			Common.SupportFolder = "";
			Common.ProgInstall = PathPart.Bin(Environment.CurrentDirectory, "/../../DistFIles");
			Common.ProgBase = PathPart.Bin(Environment.CurrentDirectory, "/../../DistFiles"); // for masterDocument
			Common.UseAfterBeforeProcess = true;
			_styleFile = "styles.xml";
			_contentFile = "content.xml";
			_isLinux = Common.IsUnixOS();

			if (!_isLinux)
				LoadParam("Dictionary", "false");
		}

		#endregion

		#region Private Functions
		private string FileInput(string fileName)
		{
			return Common.PathCombine(_inputPath, fileName);
		}

		private string FileOutput(string fileName)
		{
			return Common.PathCombine(_outputPath, fileName);
		}

		private void InLineMethod()
		{
			_validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
			XmlNode x = _validate.GetOfficeNode();
			bool fail = false;

			int counter = 1;
			string exp;
			string inner;
			foreach (XmlNode item in x.ChildNodes)
			{
				switch (counter)
				{
					case 3:
						exp = "locator_dictionary";
						inner = "parent text div div parent text";
						fail = CheckStyleandInnerText(item, exp, inner);
						break;

					case 4:
						exp = "locator_locator_dictionary";
						inner = "parent text";
						fail = CheckStyleandInnerText(item, exp, inner);
						break;

					case 5:
						exp = "locator_dictionary";
						inner = "parent text";
						fail = CheckStyleandInnerText(item, exp, inner);
						break;

					case 6:
						exp = "topara_locator_dictionary";
						inner = "text";
						fail = CheckStyleandInnerText(item, exp, inner);
						break;

					case 7:
						exp = "topara_locator_dictionary";
						inner = "text";
						fail = CheckStyleandInnerText(item, exp, inner);
						break;

					case 8:
						exp = "locator_dictionary";
						inner = "parent text";
						fail = CheckStyleandInnerText(item, exp, inner);
						break;

					default:
						break;

				}
				counter++;
				if (fail)
				{
					Assert.Fail("InlineBlock Test Failed");
				}
			}
		}

		private bool CheckStyleandInnerText(XmlNode item, string exp, string inner)
		{
			string key = "style-name";
			string ns = "text";

			XmlNode y;
			bool fail = false;
			y = _validate.GetAttibute(item, key, ns);

			if (y.Value != exp)
			{
				fail = true;
			}
			string innerText = _validate.GetReplacedInnerText(item);
			if (!fail && innerText != inner)
			{
				fail = true;
			}
			return fail;
		}

		#endregion PrivateFunctions

		//#region Nodes_Test
		#region FileTest

		///<summary>
		///Dictionary Tab Test
		///</summary>
		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void DictionaryTabUnicodeTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "TabUnicode";
			DateTime startTime = DateTime.Now;

			string styleOutput = GetStyleOutput(file);

			_totalTime = DateTime.Now - startTime;

			string styleExpected = Common.PathCombine(_expectedPath, file + "styles.xml");
			string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
			if (Common.UsingMonoVM)
			{
				styleExpected = Common.PathCombine(_expectedlinuxPath, file + "styles.xml");
				contentExpected = Common.PathCombine(_expectedlinuxPath, file + "content.xml");
			}
			XmlAssert.Ignore(styleOutput, "//office:font-face-decls", new Dictionary<string, string> { { "office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0" } });
			XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
			XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
		}

		///<summary>
		///B1pe Full Scripture Test
		/// </summary>
		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void DictionaryMainStyleExport()
		{
			Common.UseAfterBeforeProcess = false;
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "DictionaryMainStyle";
			DateTime startTime = DateTime.Now;

			string styleOutput = GetStyleOutput(file);

			_totalTime = DateTime.Now - startTime;

			string styleExpected = Common.PathCombine(_expectedPath, file + "styles.xml");
			string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
			if (Common.UsingMonoVM)
			{
				styleExpected = Common.PathCombine(_expectedlinuxPath, file + "styles.xml");
				contentExpected = Common.PathCombine(_expectedlinuxPath, file + "content.xml");
			}
			XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
			XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
			Common.UseAfterBeforeProcess = true;
		}

		#endregion

		private string GetStyleOutput(string file)
		{
			LOContent contentXML = new LOContent();
			LOStyles stylesXML = new LOStyles();
			string fileOutput = _index > 0 ? file + _index + ".css" : file + ".css";

			//string input = FileInput(file + ".css");
			string input = FileInput(fileOutput);

			_projInfo.DefaultCssFileWithPath = input;
			_projInfo.TempOutputFolder = _outputPath;

			Dictionary<string, Dictionary<string, string>> cssClass = new Dictionary<string, Dictionary<string, string>>();
			CssTree cssTree = new CssTree();
			cssClass = cssTree.CreateCssProperty(input, true);

			//StyleXML
			string styleOutput = FileOutput(file + _styleFile);
			Dictionary<string, Dictionary<string, string>> idAllClass = stylesXML.CreateStyles(_projInfo, cssClass, styleOutput);

			// ContentXML
			var pageSize = new Dictionary<string, string>();
			pageSize["height"] = cssClass["@page"]["page-height"];
			pageSize["width"] = cssClass["@page"]["page-width"];
			_projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
			_projInfo.TempOutputFolder = FileOutput(file);
			_projInfo.HideSpaceVerseNumber = stylesXML.HideSpaceVerseNumber;

			PreExportProcess preProcessor = new PreExportProcess(_projInfo);
			preProcessor.GetTempFolderPath();
			_projInfo.DefaultXhtmlFileWithPath = preProcessor.ProcessedXhtml;
			if (Param.HyphenEnable)
				preProcessor.IncludeHyphenWordsOnXhtml(_projInfo.DefaultXhtmlFileWithPath);

			AfterBeforeProcess afterBeforeProcess = new AfterBeforeProcess();
			afterBeforeProcess.RemoveAfterBefore(_projInfo, cssClass, cssTree.SpecificityClass, cssTree.CssClassOrder);

			contentXML.CreateStory(_projInfo, idAllClass, cssTree.SpecificityClass, cssTree.CssClassOrder, 325, pageSize);
			_projInfo.TempOutputFolder = _projInfo.TempOutputFolder + _contentFile;
			return styleOutput;
		}

		private static void LoadParam(string inputType, string tocTrueFalse)
		{
			// Verifying the input setting file and css file - in Input Folder
			string settingFile = inputType + "StyleSettings.xml";
			string sFileName = Common.PathCombine(_outputBasePath, settingFile);
			Common.ProgBase = _outputBasePath;

			Param.LoadSettings();
			Param.SetValue(Param.InputType, inputType);
			Param.LoadSettings();
			// setup - ensure that there is a current organization in the StyleSettings xml
			Param.UpdateMetadataValue(Param.TableOfContents, tocTrueFalse);
			Param.Write();
			Param.LoadValues(sFileName);
			Param.SetLoadType = inputType;
			Param.Value["OutputPath"] = _outputBasePath;
			Param.Value["UserSheetPath"] = _outputBasePath;
		}


	}
}