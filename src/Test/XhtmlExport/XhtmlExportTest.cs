// --------------------------------------------------------------------------------------------
// <copyright file="XhtmlExportTest.cs" from='2009' to='2014' company='SIL International'>
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
// Test methods of FlexDePlugin
// </remarks>
// --------------------------------------------------------------------------------------------

#region Using
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Resources;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Win32;
using System.IO;
using System.Xml;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

#endregion Using

namespace Test.XhtmlExport
{
	/// <summary>
	/// Test methods of FlexDePlugin
	/// </summary>
	[TestFixture]
	[Category("BatchTest")]
	//[assembly: RegistryPermissionAttribute(SecurityAction.RequestMinimum, ViewAndModify = "HKEY_LOCAL_MACHINE")]
	public class XhtmlExportTest
	{
		#region Setup
		/// <summary>holds path to script file</summary>
		private static string _scriptPath = string.Empty;
		/// <summary>holds full name of the script engine</summary>
		private static string _autoIt = string.Empty;

		private static TestFiles _tf;

		/// <summary>
		/// setup Input, Expected, and Output paths relative to location of program
		/// </summary>
		[TestFixtureSetUp]
		protected void SetUp()
		{
			Common.Testing = true;
			RegistryKey myKey = Registry.LocalMachine.OpenSubKey("Software\\Classes\\AutoIt3Script\\Shell\\Run\\Command", false);
			if (myKey != null)
			{
				_autoIt = myKey.GetValue("", "").ToString();
				if (_autoIt.EndsWith(@" ""%1"" %*"))
					_autoIt = _autoIt.Substring(0, _autoIt.Length - 8);
				else if (_autoIt.EndsWith(@" ""%1"""))
					_autoIt = _autoIt.Substring(0, _autoIt.Length - 5);     // Remove "%1" at end
			}
			_scriptPath = PathPart.Bin(Environment.CurrentDirectory, "/XhtmlExport");
			_tf = new TestFiles("XhtmlExport");
			var pwf = Common.PathCombine(Common.GetAllUserAppPath(), "SIL");
			var zf = new FastZip();
			zf.ExtractZip(_tf.Input("Pathway.zip"), pwf, ".*");
		}
		#endregion Setup

		/// <summary>
		/// Runs Pathway on the data and applies the back end
		/// </summary>
		private static void PathawyB(string project, string layout, string inputType, string backend)
		{
			PathwayB(project, layout, inputType, backend, "xhtml");
		}

		/// <summary>
		/// Runs Pathway on the data and applies the back end
		/// </summary>
		private static void PathwayB(string project, string layout, string inputType, string backend, string format)
		{
			Common.Testing = true;
			const bool overwrite = true;
			const string message = "";
			var xhtmlName = project + "." + format;
			var xhtmlInput = _tf.Input(xhtmlName);
			var xhtmlOutput = _tf.SubOutput(project, xhtmlName);
			File.Copy(xhtmlInput, xhtmlOutput, overwrite);
			var cssName = layout + ".css";
			var cssInput = _tf.Input(cssName);
			var cssOutput = _tf.SubOutput(project, cssName);
			File.Copy(cssInput, cssOutput, overwrite);
			var workingFolder = Path.GetDirectoryName(xhtmlInput);
			if (format == "usx")
			{
				FolderTree.Copy(_tf.Input("gather"), Common.PathCombine(_tf.Output("NKOu3"), "gather"));
				FolderTree.Copy(_tf.Input("figures"), Common.PathCombine(_tf.Output("NKOu3"), "figures"));
				workingFolder = Path.GetDirectoryName(xhtmlOutput);
				List<XmlDocument> usxBooksToExport = new List<XmlDocument>();
				usxBooksToExport.Add(xmlInnerText(xhtmlOutput));

				Dictionary<string, object> xslParams;
				xslParams = new Dictionary<string, object>();
				DateTime dateTime = new DateTime(2013, 8, 27);
				xslParams.Add("dateTime", dateTime.Date);
				xslParams.Add("user", "Tester");
				xslParams.Add("projName", "NKOu3");
				xslParams.Add("stylesheet", "usfm");
				xslParams.Add("ws", "en");
				xslParams.Add("fontName", "Times");
				xslParams.Add("fontSize", "12");

				ParatextPathwayLink converter = new ParatextPathwayLink("NKOu3", xslParams);
				converter.ConvertUsxToPathwayXhtmlFile(usxBooksToExport[0].InnerXml, xhtmlOutput.Replace(".usx", ".xhtml"));
			}

			string pathwayBFile = Common.PathCombine(Common.AssemblyPath, "PathwayB.exe");
			if (!File.Exists(pathwayBFile))
			{
				pathwayBFile = Path.GetDirectoryName(Common.AssemblyPath);
				pathwayBFile = Common.PathCombine(pathwayBFile, "PathwayB.exe");
			}

			Param.SetLoadType = "Scripture";
			Param.LoadSettings();

			EnableConfigurationSettings(workingFolder, project);

			var p1 = new Process();
			p1.StartInfo.UseShellExecute = false;
			StringBuilder arg = new StringBuilder(string.Format("-f \"{0}\" ", xhtmlOutput));
			arg.Append(string.Format("-c \"{0}\" ", cssOutput));
			arg.Append(string.Format("-t \"{0}\" ", backend));
			arg.Append(string.Format("-i {0} ", inputType));
			arg.Append(string.Format("-n \"{0}\" ", project));
			arg.Append(string.Format("-if {0} ", format));
			arg.Append(string.Format("-d \"{0}\" ", workingFolder));
			p1.StartInfo.Arguments = arg.ToString();
			p1.StartInfo.WorkingDirectory = _tf.Output(null);
			p1.StartInfo.FileName = pathwayBFile;

			p1.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
			p1.Start();
			if (p1.Id <= 0)
				throw new MissingSatelliteAssemblyException(project);
			p1.WaitForExit();
			switch (backend)
			{
				case "OpenOffice/LibreOffice":
					OdtCheck(project, message);
					break;
				case "InDesign":
					IdmlCheck(project, message);
					break;
				case "E-Book (Epub2 and Epub3)":
					//TODO: Epub needs more than a file compare test.
					FileCheck(project, ".epub", message);
					break;
				case "Go Bible":
					FileCheck(project, ".jar", message);
					break;
				case "XeLaTex":
					FileCheck(project, ".tex", message.Length > 0 ? message : "Tex file mismatch");
					break;
				default:
					throw new AssertionException("Unrecognized backend type");
			}
		}

		private static void EnableConfigurationSettings(string outputDirectory, string projectName)
		{
			Param.SetValue(Param.PrintVia, projectName);
			Param.SetValue(Param.LayoutSelected, "A4 Cols ApplicationStyles");
			// Publication Information tab
			Param.UpdateTitleMetadataValue(Param.Title, projectName, false);
			Param.UpdateMetadataValue(Param.Description, projectName + "Description");
			Param.UpdateMetadataValue(Param.Creator, projectName + "Creator");
			Param.UpdateMetadataValue(Param.Publisher, projectName + "Publisher");
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


		private static XmlDocument xmlInnerText(string usxfileName)
		{
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(usxfileName);
			return xmlDoc;
		}

		private static void TextCheck(string project, string ext, ArrayList ex, string message)
		{
			var fileExpect = _tf.SubExpected(project, project + ext);
			var fileOutput = _tf.SubOutput(project, project + ext);
			TextFileAssert.AreEqualEx(fileExpect, fileOutput, ex, message);
		}

		private static void FileCheck(string project, string ext, string message)
		{
			var fileExpect = _tf.SubExpected(project, project + ext);
			var fileOutput = _tf.SubOutput(project, project + ext);
			FileAssert.AreEqual(fileExpect, fileOutput, message);
		}

		private static void IdmlCheck(string project, string message)
		{
			var idmlExpect = _tf.SubExpected(project, project + ".idml");
			var idmlOutput = _tf.SubOutput(project, project + ".idml");
			IdmlTest.AreEqual(idmlExpect, idmlOutput, message);
		}

		private static void OdtCheck(string project, string message)
		{
			var odtExpectDir = _tf.Expected(project);
			var odtOutputDir = _tf.Output(project);
			OdtTest.AreEqual(odtExpectDir, odtOutputDir, message);
		}

		#region Gondwana Sample Open Office
		/// <summary>
		/// Gondwana Sample Open Office Back End Test
		/// </summary>
		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void GondwanaSampleOpenOfficeTest()
		{
			PathawyB("Gondwana Sample", "Gondwana Sample", "Dictionary", "OpenOffice/LibreOffice");
		}
		#endregion Gondwana Sample Open Office

		#region Nkonya Sample Open Office
		/// <summary>
		/// Nkonya Sample Open Office Back End Test
		/// </summary>
		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void NkonyaSampleOpenOfficeTest()
		{
			PathawyB("Nkonya Sample", "Nkonya Sample", "Scripture", "OpenOffice/LibreOffice");
		}
		#endregion Nkonya Sample Open Office

		#region Paratext NKOu3 Open Office
		/// <summary>
		/// Paratext NKOu3 Open Office Back End Test
		/// </summary>
		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void NKOu3OpenOfficeTest()
		{
			PathwayB("NKOu3", "NKOu3", "Scripture", "OpenOffice/LibreOffice", "usx");
		}
		#endregion Nkonya Sample Open Office

		#region Gondwana Sample InDesign
		/// <summary>
		/// Gondwana Sample InDesign Back End Test
		/// </summary>
		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void GondwanaSampleInDesignTest()
		{
			PathawyB("Gondwana Sample", "Gondwana Sample", "Dictionary", "InDesign");
		}
		#endregion Gondwana Sample InDesign

		#region Nkonya Sample InDesign
		/// <summary>
		/// Nkonya Sample InDesign Back End Test
		/// </summary>
		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void NkonyaSampleInDesignTest()
		{
			PathawyB("Nkonya Sample", "Nkonya Sample", "Scripture", "InDesign");
		}
		#endregion Nkonya Sample InDesign

		#region Paratext NKOu3 InDesign
		/// <summary>
		/// Paratext NKOu3 InDesign Back End Test
		/// </summary>
		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void NKOu3InDesignTest()
		{
			PathwayB("NKOu3", "NKOu3", "Scripture", "InDesign", "usx");
		}
		#endregion Nkonya Sample Open Office

		#region Gondwana Sample XeLaTex
		/// <summary>
		/// Gondwana Sample XeLaTex Back End Test
		/// </summary>
		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void GondwanaSampleXeLaTexTest()
		{
			PathawyB("Gondwana Sample", "Gondwana Sample", "Dictionary", "XeLaTex");
		}
		#endregion Gondwana Sample XeLaTex

		#region Paratext NKOu3 XeLaTex
		/// <summary>
		/// Paratext NKOu3 XeLaTex Back End Test
		/// </summary>
		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void NKOu3XeLaTexTest()
		{
			PathwayB("NKOu3", "NKOu3", "Scripture", "XeLaTex", "usx");
		}
		#endregion Nkonya Sample XeLaTex
	}
}