// --------------------------------------------------------------------------------------------
// <copyright file="XeLaTexContentTest.cs" from='2009' to='2014' company='SIL International'>
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
// WordPress Test Support
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.XeLatex
{
	[TestFixture]
	public class XeLaTexContentTest : ExportXeLaTex
	{
		#region Private Variables
		private string _inputPath;
		private string _outputPath;
		private string _expectedPath;
		private bool _isLinux;
		private PublicationInformation _projInfo;
		private Dictionary<string, List<string>> _classInlineStyle;
		Dictionary<string, string> _langFontCodeandName = new Dictionary<string, string>();
		#endregion

		#region Setup
		[TestFixtureSetUp]
		protected void SetUpAll()
		{
			Common.Testing = true;
			Common.ProgInstall = PathPart.Bin(Environment.CurrentDirectory, @"/../../DistFiles");
			Common.SupportFolder = "";
			Common.ProgBase = Common.ProgInstall;
			_isLinux = Common.IsUnixOS();
			_projInfo = new PublicationInformation();
			string testPath = PathPart.Bin(Environment.CurrentDirectory, "/XeLatex/TestFiles");
			_inputPath = Common.PathCombine(testPath, "input");
			_outputPath = Common.PathCombine(testPath, "output");

			if (_isLinux)
			{
				_expectedPath = Common.PathCombine(testPath, "LinuxExpected");
			}
			else
			{
				_expectedPath = Common.PathCombine(testPath, "Expected");
			}

			if (Directory.Exists(_outputPath))
				Directory.Delete(_outputPath, true);
			Directory.CreateDirectory(_outputPath);
			_projInfo.ProjectPath = testPath;
			_classInlineStyle = new Dictionary<string, List<string>>();
			string pathwayDirectory = Common.AssemblyPath;
			string styleSettingFile = Common.PathCombine(pathwayDirectory, "StyleSettings.xml");

			if (!File.Exists(styleSettingFile))
			{
				styleSettingFile = Path.GetDirectoryName(Common.AssemblyPath);
				styleSettingFile = Common.PathCombine(styleSettingFile, "StyleSettings.xml");
			}

			ValidateXMLVersion(styleSettingFile);
			Common.ProgInstall = pathwayDirectory;
			Param.LoadSettings();
			Param.SetValue(Param.InputType, "Scripture");
			Param.LoadSettings();
			Common.UseAfterBeforeProcess = true;
		}

		#endregion Setup

		#region Public Functions

		private void ValidateXMLVersion(string filePath)
		{
			var versionControl = new SettingsVersionControl();
			var validator = new SettingsValidator();
			if (File.Exists(filePath))
			{
				versionControl.UpdateSettingsFile(filePath);
				bool isValid = validator.ValidateSettingsFile(filePath, true);
				if (!isValid)
				{
				}
			}
		}

		#region Nunits

		/// <summary>
		///A test for Export Xelatex For Scripture
		///</summary>
		[Test]
		[Category("LongTest")]
		[Category("SkipOnTC")]
		public void ScriptureExportTest()
		{
			string inputSourceDirectory = FileInput("ExportXelatex");
			string outputDirectory = FileOutput("ExportXelatex");
			if (Directory.Exists(outputDirectory))
			{
				Directory.Delete(outputDirectory, true);
			}
			FolderTree.Copy(inputSourceDirectory, outputDirectory);
			Param.SetLoadType = "Scripture";
			Param.LoadSettings();
			_projInfo.ProjectPath = outputDirectory;
			_projInfo.ProjectInputType = "Scripture";
			_projInfo.DefaultXhtmlFileWithPath = Common.PathCombine(outputDirectory, "ScriptureInput.xhtml");
			_projInfo.DefaultCssFileWithPath = Common.PathCombine(outputDirectory, "ScriptureInput.css");

			EnableConfigurationSettings(outputDirectory);

			var target = new ExportXeLaTex();
			const bool expectedResult = true;
			bool actual = target.Export(_projInfo);
			Assert.AreEqual(expectedResult, actual);
		}

		private void EnableConfigurationSettings(string outputDirectory)
		{
			Param.SetValue(Param.PrintVia, "Xelatex");
			Param.SetValue(Param.LayoutSelected, "A4 Cols ApplicationStyles");
			// Publication Information tab
			Param.UpdateTitleMetadataValue(Param.Title, "XelatexExport", false);
			Param.UpdateMetadataValue(Param.Description, "XelatexDescription");
			Param.UpdateMetadataValue(Param.Creator, "XelatexCreator");
			Param.UpdateMetadataValue(Param.Publisher, "XelatexPublisher");
			Param.UpdateMetadataValue(Param.CopyrightHolder, "SIL International");
			// also persist the other DC elements
			Param.UpdateMetadataValue(Param.Subject, "Bible");
			Param.UpdateMetadataValue(Param.Date, DateTime.Today.ToString("yyyy-MM-dd"));
			Param.UpdateMetadataValue(Param.CoverPage, "True");

			string pathwayDirectory = Common.AssemblyPath;
			string coverImageFilePath = Common.PathCombine(pathwayDirectory, "Graphic");

			if (!Directory.Exists(coverImageFilePath))
			{
				coverImageFilePath = Path.GetDirectoryName(Common.AssemblyPath);
				coverImageFilePath = Common.PathCombine(coverImageFilePath, "Graphic");
			}

			coverImageFilePath = Common.PathCombine(coverImageFilePath, "cover.png");
			Param.UpdateMetadataValue(Param.CoverPageFilename, coverImageFilePath);

			Param.UpdateMetadataValue(Param.CoverPageTitle, "True");
			Param.UpdateMetadataValue(Param.TitlePage, "True");
			Param.UpdateMetadataValue(Param.CopyrightPage, "True");

			string copyrightsFilePath = Common.PathCombine(pathwayDirectory, "Copyrights");
			copyrightsFilePath = Common.PathCombine(copyrightsFilePath, "SIL_CC-by-nc-sa.xhtml");
			Param.UpdateMetadataValue(Param.CopyrightPageFilename, copyrightsFilePath);

			Param.UpdateMetadataValue(Param.TableOfContents, "True");
			Param.SetValue(Param.Media, "paper");
			Param.SetValue(Param.PublicationLocation, outputDirectory);
			Param.Write();

		}

		/// <summary>
		///A test for Export Xelatex For Scripture
		///</summary>
		[Test]
		[Category("LongTest")]
		[Category("SkipOnTC")]
		public void DictionaryExportTest()
		{
			string inputSourceDirectory = FileInput("ExportXelatex");
			string outputDirectory = FileOutput("ExportXelatex");
			if (Directory.Exists(outputDirectory))
			{
				Directory.Delete(outputDirectory, true);
			}
			FolderTree.Copy(inputSourceDirectory, outputDirectory);
			Param.SetLoadType = "Dictionary";
			Param.LoadSettings();
			_projInfo.ProjectPath = outputDirectory;
			_projInfo.ProjectInputType = "Dictionary";
			_projInfo.DefaultXhtmlFileWithPath = Common.PathCombine(outputDirectory, "DictionaryInput.xhtml");
			_projInfo.DefaultCssFileWithPath = Common.PathCombine(outputDirectory, "DictionaryInput.css");

			EnableConfigurationSettings(outputDirectory);

			var target = new ExportXeLaTex();
			const bool expectedResult = true;
			bool actual = target.Export(_projInfo);
			Assert.AreEqual(expectedResult, actual);
		}


		/// <summary>
		/// Multi Parent Test - .subsenses > .sense > .xsensenumber { font-size:10pt;}
		/// Parent comes as multiple times
		/// </summary>

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void TextAlignTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "TextAlign";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void TextIndentTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "Textindent";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void ListSmallTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "ListSmall";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void TextColorTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "Color";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void ChapterNumberTest()
		{
			_projInfo.ProjectInputType = "Scripture";
			const string file = "ChapterNumber";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void ChapterNumberOnHeaderTest()
		{
			_projInfo.ProjectInputType = "Scripture";
			const string file = "ChapterNumberOnHeader";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void DropCapTest()
		{
			_projInfo.ProjectInputType = "Scripture";
			const string file = "DropCap";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void FontStyleItalicTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "FontStyleItalic";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void Inherit()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "inherit";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void FontStyleNormalTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "FontStyleNormal";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void FontWeight()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "FontWeight";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void FontVariantSmallCapTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "FontVariantSmallCap";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void FontVariantNormalTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "FontVariantNormal";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void FontSizePointTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "FontSizePoint";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void WordSpace()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "WordSpace";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void LineHeight()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "LineHeight";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void FontSizeCmToPointTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "FontSizeCmToPoint";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void FontSizeXXSmallTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "FontSizeXXSmall";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void FontWeightBoldTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "FontWeightBold";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void FontWeightNormalTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "FontWeightNormal";
			ExportProcess(file);
			FileCompare(file);
		}


		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void FontWeightBoldRegular()  // TD-2330()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "FontWeightBoldRegular";
			ExportProcess(file);
			FileCompare(file);
		}


		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void FontBoldItalicTest()  // TD-2188()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "FontBoldItalic";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void FontWeight400Test()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "FontWeight400";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void FontWeight700Test()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "FontWeight700";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void TextAlignCenterTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "TextAlignCenter";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void MarginTopTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "MarginTop";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void MarginBottomTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "MarginBottom";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void DisplayBlockTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "DisplayBlock";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void TextAlignRightTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "TextAlignRight";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void PseudoAfterTest()
		{
			//Added with Unicode
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "PseudoAfter";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void GautamiFontTest()
		{
			//Added with Unicode
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "GautamiFontTest";
			ExportProcess(file);
			FileCompare(file);
		}


		//[Test]
		//[Category("SkipOnTC")]
		//public void TextDecorationTest()
		//{
		//    _projInfo.ProjectInputType = "Dictionary";
		//    const string file = "TextDecoration";
		//    ExportProcess(file);
		//    FileCompare(file);
		//}

		#region NestedDiv

		[Test]
		public void NestedDiv1()
		{
			const string file = "NestedDivCase1";
			_projInfo.ProjectInputType = "Dictionary";
			ExportProcess(file);
			FileCompare(file);
		}


		[Test]
		public void NestedDiv2()
		{
			const string file = "NestedDivCase2";
			_projInfo.ProjectInputType = "Dictionary";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		public void NestedDiv3()
		{
			const string file = "NestedDivCase3";
			_projInfo.ProjectInputType = "Dictionary";
			ExportProcess(file);
			FileCompare(file);
		}


		[Test]
		public void NestedDiv4()
		{
			const string file = "NestedDivCase4";
			_projInfo.ProjectInputType = "Dictionary";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		public void NestedDiv5()
		{
			const string file = "NestedDivCase5";
			_projInfo.ProjectInputType = "Dictionary";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		public void NestedDiv6()
		{
			const string file = "NestedDivCase6";
			_projInfo.ProjectInputType = "Dictionary";
			ExportProcess(file);
			FileCompare(file);
		}


		[Test]
		public void NestedSpan1()
		{
			const string file = "NestedSpan1";
			_projInfo.ProjectInputType = "Dictionary";
			ExportProcess(file);
			FileCompare(file);
		}
		#endregion

		[Test]
		public void EMTest1()
		{
			const string file = "EMTest1";
			_projInfo.ProjectInputType = "Dictionary";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		public void Font1()
		{
			const string file = "Font1";
			_projInfo.ProjectInputType = "Dictionary";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		public void FontParent()
		{
			const string file = "FontParent";
			_projInfo.ProjectInputType = "Dictionary";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("LongTest")]
		[Category("SkipOnTC")]
		public void PictureCaptionTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "PictureWidth";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void ImageBaseTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			_projInfo.ProjectPath = _inputPath;
			const string file = "ImageBase";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void TextIndentPcTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "TextindentPC";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void MarginRightTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "MarginRight";
			ExportProcess(file);
			FileCompare(file);
		}


		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void UnderlineTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "Underline";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		public void DisplayNone()
		{
			const string file = "DisplayNone";
			_projInfo.ProjectInputType = "Dictionary";
			ExportProcess(file);
			FileCompare(file);
		}



		[Test]
		[Category("SkipOnTC")]
		public void InputCaseFW83Alpha()
		{
			const string file = "inputcasefw83alpha";
			_projInfo.ProjectInputType = "Dictionary";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		public void Larger()
		{
			const string file = "Larger";
			_projInfo.ProjectInputType = "Dictionary";
			ExportProcess(file);
			FileCompare(file);
		}


		[Test]
		//TD-2302 font-family: Sans-Serif;
		public void FontFamily2()
		{
			const string file = "FontFamily2";
			_projInfo.ProjectInputType = "Dictionary";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		//TD-2302 font-family: Sans-Serif;
		public void FontFamily4()
		{
			const string file = "FontFamily4";
			_projInfo.ProjectInputType = "Dictionary";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		//TD-2303 font-family: Gentium
		public void FontFamily1()
		{
			const string file = "FontFamily1";
			_projInfo.ProjectInputType = "Dictionary";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		//TD-2303 font-family: Gentium
		public void FontFamily2a()
		{
			const string file = "FontFamily2a";
			_projInfo.ProjectInputType = "Dictionary";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		//TD-2303 font-family: Gentium
		public void FontFamily3()
		{
			const string file = "FontFamily3";
			_projInfo.ProjectInputType = "Dictionary";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		//TD-2303 font-family: Gentium
		public void FontFamily4a()
		{
			const string file = "FontFamily4a";
			_projInfo.ProjectInputType = "Dictionary";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		//TD-2303 font-family: Gentium
		public void FontFamily5()
		{
			const string file = "FontFamily5";
			_projInfo.ProjectInputType = "Dictionary";
			ExportProcess(file);
			FileCompare(file);
		}

		///<summary>
		///font with font-feature-settings
		///To Test whether the font-feature-settings have been applied for the XeLaTex Output
		/// </summary>
		[Test]
		public void FontFamily7()
		{
			const string file = "FontFamily7";
			_projInfo.ProjectInputType = "Scripture";
			ExportProcess(file);
			string texOutput = FileOutput(file + ".tex");
			string expectedString1 = "\\font\\divnco=";
			string expectedString2 = "/GR:litr = 0:apos = 1\" at 14pt";
			bool isFound = false;
			using (StreamReader sr = File.OpenText(texOutput))
			{
				string outputFileString = string.Empty;
				while ((outputFileString = sr.ReadLine()) != null)
				{
					if (outputFileString.Contains(expectedString1) && outputFileString.Contains(expectedString2))
					{
						isFound = true;
						break;
					}
				}
			}
			Assert.AreEqual(true, isFound);
		}

		[Test]
		//TD-2059 font-family: "<default serif>", serif;
		public void FontFamily6()
		{
			const string file = "FontFamily6";
			_projInfo.ProjectInputType = "Dictionary";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void TextAlignJustifyTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "TextAlignJustify";
			ExportProcess(file);
			FileCompare(file);
		}


		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void Counter1Test()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "Counter1";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void Counter2Test()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "Counter2";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void Counter3Test()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "Counter3";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void LanguageTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "Lang";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void LetterspaceTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "Letterspace";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void LineHeightNoneTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "LineHeightNone";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("SkipOnTC")]
		public void LineHeightPercentageTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "LineHeightPercentage";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("SkipOnTC")]
		public void LineHeightPointTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "LineHeightPoint";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void PaddingLeftTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "PaddingLeft";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void PageBGColorTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "PageBGColor";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void PageSizeTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "PageSize";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void FootNote1Test()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "Footnote";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void FootNote2Test()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "Footnote2";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void FootNoteUnicodeTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "Footnote_unicode";
			ExportProcess(file);
			FileCompare(file);
		}


		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void ReplaceStringTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "ReplaceString";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("SkipOnTC")]
		public void FlexRevTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "FlexRev";
			ExportProcess(file);
			FileCompare(file);
		}


		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void VerticalAlignTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "VerticalAlign";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("LongTest")]
		[Category("SkipOnTC")]
		public void UnicodeSymbolTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "UnicodeSymbol";
			ExportProcess(file);
			FileCompare(file);
		}


		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void HashSymbolTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			_projInfo.ProjectPath = _inputPath;
			const string file = "HashSymbol";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void Precede1()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "Precede1";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void PrecedesPseudoLangTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "PrecedesPseudoLangTest";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void PrecedesPseudoTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "PrecedesPseudoTest";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void PrecedesPseudoTestA()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "PrecedesPseudoTestA";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void PrecedesPseudoTestB()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "PrecedesPseudoTestB";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void Parent1()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "Parent1";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void VisibilityTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "Visibility";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void multiClass()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "multiClass";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void TaggedText()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "TaggedText";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void TextFontSizeTestC()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "TextFontSizeTestC";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void MissingCurlyBracesTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "MissingCurlyBracesTest";
			ExportProcess(file);
			FileCompare(file);
		}


		[Test]
		[Category("LongTest")]
		[Category("SkipOnTC")]
		public void VisibilityCensorPackageTest()
		{
			const string testFileName = "VisibilityPackage";
			var inputname = testFileName + ".tex";
			var xeLatexFullFile = FileOutput(inputname);
			const bool overwrite = true;
			File.Copy(FileInput(inputname), xeLatexFullFile, overwrite);
			var imgPath = new Dictionary<string, string>();
			UpdateXeLaTexFontCacheIfNecessary();
			Common.Testing = true;
			CallXeLaTex(_projInfo, xeLatexFullFile, true, imgPath);
			var outname = testFileName + ".log";
			TextFileAssert.AreEqualEx(FileExpected(outname), FileOutput(outname), new ArrayList { 2, 13, 14, 15, 16, 17 });
		}

		[Test]
		[Category("LongTest")]
		[Category("SkipOnTC")]
		public void ColumnCount()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "ColumnCount";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void ColumnGap()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "ColumnGap";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void XeLaTexPath()
		{
			if (Directory.Exists(XeLaTexInstallation.GetXeLaTexDir()))
				Assert.AreEqual(@":\pwtex\", XeLaTexInstallation.GetXeLaTexDir().Substring(1));
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void XeLaTexVersion()
		{
			Assert.IsTrue(XeLaTexInstallation.CheckXeLaTexVersion());
		}

		[Test]
		public void XeLaTexFontCount()
		{
			XeLaTexInstallation.SetXeLaTexFontCount(1);
			Assert.AreEqual(1, XeLaTexInstallation.GetXeLaTexFontCount());
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void LineBreak()
		{
			_projInfo.ProjectInputType = "Scripture";
			const string file = "LineBreak";
			ExportProcess(file);
			FileCompare(file);
		}



		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void DoubleUnderlineTest()
		{
			_projInfo.ProjectInputType = "Scripture";
			const string file = "DoubleUnderlineTest";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("LongTest")]
		[Category("SkipOnTC")]
		public void FootNoteMarker()
		{
			_projInfo.ProjectInputType = "Scripture";
			const string file = "FootNoteMarker";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void TOCLetters()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "TOCLetters";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("LongTest")]
		[Category("SkipOnTC")]
		public void XeLaTexUpdateCache()
		{
			UpdateXeLaTexFontCacheIfNecessary();
			var systemFontList = System.Drawing.FontFamily.Families;
			Assert.AreEqual(systemFontList.Length, XeLaTexInstallation.GetXeLaTexFontCount());
		}


		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTC")]
		public void TwoColumnInputCase()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "TwoColumnInputCase";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("SkipOnTC")]
		public void HangingIndent()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "HangingIndent";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("SkipOnTC")]
		public void BRTagInputCase()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "BRTag";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("SkipOnTC")]
		public void BidiTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "Bidi";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("SkipOnTC")]
		public void HideVerseNumYes()
		{
			_projInfo.ProjectInputType = "Scripture";
			const string file = "HideVerseNumYes";
			ExportProcess(file);
			FileCompare(file);
		}

		[Test]
		[Category("SkipOnTC")]
		public void HideVerseNumNo()
		{
			_projInfo.ProjectInputType = "Scripture";
			const string file = "HideVerseNumNo";
			ExportProcess(file);
			FileCompare(file);
		}

		/// <summary>
		///A test for Export Xelatex For Scripture
		///</summary>
		[Test]
		[Category("SkipOnTC")]
		public void XelatexCoverPageTitleTest()
		{
			string inputSourceDirectory = FileInput("ExportXelatex");
			string outputDirectory = FileOutput("ExportXelatex");
			if (Directory.Exists(outputDirectory))
			{
				Directory.Delete(outputDirectory, true);
			}
			FolderTree.Copy(inputSourceDirectory, outputDirectory);
			Param.LoadSettings();
			_projInfo.ProjectPath = outputDirectory;
			_projInfo.ProjectInputType = "Dictionary";
			_projInfo.DefaultXhtmlFileWithPath = Common.PathCombine(outputDirectory, "CoverPageTitle.xhtml");
			_projInfo.DefaultCssFileWithPath = Common.PathCombine(outputDirectory, "CoverPageTitle.css");

			EnableConfigurationSettings(outputDirectory);

			var target = new ExportXeLaTex();
			target.Export(_projInfo);

			string outputResultFile = _projInfo.ProjectPath;
			outputResultFile = Path.Combine(outputResultFile, "CoverPageTitle.tex");
			string expectedResultFile = FileExpected("CoverPageTitle" + ".tex");
			TextFileAssert.CheckLineAreEqualEx(expectedResultFile, outputResultFile, new ArrayList { 1, 10, 15, 20 });
		}

		#endregion

		#region Private Functions

		private void FileCompare(string file)
		{
			string texOutput = FileOutput(file + ".tex");
			string texExpected = FileExpected(file + ".tex");
			TextFileAssert.AreEqual(texOutput, texExpected, file + " in tex ");
		}

		private void ExportProcess(string file)
		{
			string input = FileInput(file + ".xhtml");
			_projInfo.DefaultXhtmlFileWithPath = input;
			_langFontCodeandName = new Dictionary<string, string>();
			GetXhtmlFileFontCodeandFontName(_projInfo.DefaultXhtmlFileWithPath);
			input = FileInput(file + ".css");
			_projInfo.DefaultCssFileWithPath = input;

			_projInfo.TempOutputFolder = _outputPath;
			_projInfo.OutputExtension = ".tex";

			Dictionary<string, Dictionary<string, string>> cssClass = new Dictionary<string, Dictionary<string, string>>();
			CssTree cssTree = new CssTree();
			cssTree.OutputType = Common.OutputType.XELATEX;
			cssClass = cssTree.CreateCssProperty(input, true);
			int pageWidth = Common.GetPictureWidth(cssClass, _projInfo.ProjectInputType);

			string xetexFullFile = Common.PathCombine(_outputPath, file + ".tex");
			StreamWriter xetexFile = new StreamWriter(xetexFullFile);

			XeLaTexStyles styles = new XeLaTexStyles();
			_classInlineStyle = styles.CreateXeTexStyles(_projInfo, xetexFile, cssClass);

			XeLaTexContent content = new XeLaTexContent();
			Dictionary<string, List<string>> classInlineText = styles._classInlineText;
			Dictionary<string, Dictionary<string, string>> newProperty = content.CreateContent(_projInfo, cssClass, xetexFile, _classInlineStyle, cssTree.SpecificityClass, cssTree.CssClassOrder, classInlineText, pageWidth);

			CloseFile(xetexFile);

			ModifyXeLaTexStyles modifyXeTexStyles = new ModifyXeLaTexStyles();
			modifyXeTexStyles.ModifyStylesXML(_projInfo.ProjectPath, xetexFile, newProperty, cssClass, xetexFullFile, string.Empty, _langFontCodeandName, true);

		}

		private void GetXhtmlFileFontCodeandFontName(string xhtmlFileName)
		{
			if (!File.Exists(xhtmlFileName)) return;
			XmlDocument xdoc = Common.DeclareXMLDocument(false);
			xdoc.Load(xhtmlFileName);
			XmlNodeList metaNodes = xdoc.GetElementsByTagName("meta");
			if (metaNodes != null && metaNodes.Count > 0)
			{
				try
				{
					foreach (XmlNode metaNode in metaNodes)
					{
						FontFamily[] systemFontList = System.Drawing.FontFamily.Families;
						foreach (FontFamily systemFont in systemFontList)
						{
							if (metaNode.Attributes["content"].Value.ToLower() == systemFont.Name.ToLower())
							{
								_langFontCodeandName.Add(metaNode.Attributes["name"].Value, metaNode.Attributes["content"].Value);
								break;
							}
						}
						//if (metaNode.Attributes["name"].Value == "linkedFilesRootDir")
						//{
						//    imageRootPath = metaNode.Attributes["content"].Value;
						//    break;
						//}
					}
				}
				catch { }
			}
		}

		private string FileInput(string fileName)
		{
			return Common.PathCombine(_inputPath, fileName);
		}

		private string FileOutput(string fileName)
		{
			return Common.PathCombine(_outputPath, fileName);
		}

		private string FileExpected(string fileName)
		{
			return Common.PathCombine(_expectedPath, fileName);
		}

		private void CloseFile(StreamWriter xeLatexFile)
		{
			xeLatexFile.WriteLine();
			//xeLatexFile.WriteLine(@"\bye");
			xeLatexFile.WriteLine(@"\end{document}");
			xeLatexFile.Flush();
			xeLatexFile.Close();
		}

		#endregion PrivateFunctions

		#endregion
	}
}
