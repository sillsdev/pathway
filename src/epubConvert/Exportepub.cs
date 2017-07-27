// --------------------------------------------------------------------------------------
// <copyright file="Exportepub.cs" from='2009' to='2014' company='SIL International'>
//      Copyright (C) 2014, SIL International. All Rights Reserved.
//
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
// <author>Erik Brommers</author>
// <email>erik_brommers@sil.org</email>
// Last reviewed:
//
// <remarks>
// epub export
//
// .epub files are zipped archives with the following file structure:
// |-mimetype
// |-META-INF
// | `-container.xml
// |-OEBPS
//   |-content.opf
//   |-toc.ncx
//   |-<any fonts and other files embedded into the archive>
//   |-<list of files in book (C) xhtml format + .css for styling>
//   '-<any images referenced in book files>
//
// See also http://www.openebook.org/2007/ops/OPS_2.0_final_spec.html
// </remarks>
// --------------------------------------------------------------------------------------

// uncomment this to write out performance timings
//#define TIME_IT

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using epubConvert;
using epubConvert.Properties;
using epubValidator;
using L10NSharp;
using SilTools;
using SIL.Tool;


namespace SIL.PublishingSolution
{
	/// <summary>
	/// Possible values for the MissingFont and NonSilFont properties.
	/// </summary>
	public enum FontHandling
	{
		EmbedFont,
		SubstituteDefaultFont,
		PromptUser,
		CancelExport
	}

	public class Exportepub : IExportProcess
	{
		private EpubFont _epubFont;
		private EpubManifest _epubManifest;
		Epub3Transformation _exportEpub3;
		private bool _isIncludeImage = true;

		private bool _isNoteTargetReferenceExists;
		private bool _isUnixOs;

		//        protected static PostscriptLanguage _postscriptLanguage = new PostscriptLanguage();
		public string InputType = "dictionary";

		public const string ReferencesFilename = "zzReferences.xhtml";

		// property implementations
		public bool EmbedFonts { get; set; }
		public bool IncludeFontVariants { get; set; }
		public string TocLevel { get; set; }
		public int MaxImageWidth { get; set; }
		private XslCompiledTransform addRevId;
		private XslCompiledTransform noXmlSpace;
		protected XslCompiledTransform fixEpub;

		public int BaseFontSize { get; set; }
		public int DefaultLineHeight { get; set; }

		/// <summary>
		/// Fallback font (if the embedded font is missing or non-SIL)
		/// </summary>
		public string DefaultFont { get; set; }

		public string DefaultAlignment { get; set; }
		public string ChapterNumbers { get; set; }
		public string References { get; set; }
		public FontHandling MissingFont { get; set; } // note that this doesn't use all the enum values
		public FontHandling NonSilFont { get; set; }
		private ArrayList PseudoClass = new ArrayList();
		public bool PageBreak;
		private readonly Dictionary<string, string> _tocIdMapping = new Dictionary<string, string>();
		readonly List<string> _tocIDs = new List<string>();

		// interface methods
		public string ExportType
		{
			get
			{
				return "E-Book (Epub2 and Epub3)"; //E-Book (.epub)
			}
		}

		/// <summary>
		/// Returns what input data types this export process handles. The epub exporter
		/// currently handles scripture and dictionary data types.
		/// </summary>
		/// <param name="inputDataType">input data type to test</param>
		/// <returns>true if this export process handles the specified data type</returns>
		public bool Handle(string inputDataType)
		{
			var returnValue = inputDataType.ToLower() == "dictionary" || inputDataType.ToLower() == "scripture";
			return returnValue;
		}

		/// <summary>
		/// Entry point for epub converter
		/// </summary>
		/// <param name="projInfo">values passed including xhtml and css names</param>
		/// <returns>true if succeeds</returns>
		public bool Export(PublicationInformation projInfo)
		{
			Common.SetupLocalization();
			if (projInfo == null)
				return false;
			const bool success = true;

			#region Set up progress reporting

#if (TIME_IT)
						DateTime dt1 = DateTime.Now;    // time this thing
			#endif
			var myCursor = Common.UseWaitCursor();
			var curdir = Environment.CurrentDirectory;
			var inProcess = Common.SetupProgressReporting(20, "Export " + ExportType);

			#endregion Set up progress reporting

			PreExportProcess preProcessor;
			EpubToc epubToc;
			var bookId = SetupConversion(projInfo, inProcess, out preProcessor, out epubToc);

			string[] langArray;
			var outputFolder = PreprocessXhtml(projInfo, inProcess, preProcessor, out langArray);

			string mergedCss;
			string defaultCss;
			string tempCssFile;
			var tempFolder = ProcessingCss(projInfo, inProcess, preProcessor, out mergedCss, out defaultCss, out tempCssFile);

			FixIssuesWithFlexXhtml(projInfo, preProcessor, langArray, inProcess);

			AddNavitation(inProcess, preProcessor);

			var frontMatter = AddFrontMatter(inProcess, preProcessor, tempFolder);

			List<string> splitFiles;
			var htmlFiles = SplitIntoSections(projInfo, inProcess, frontMatter, preProcessor, defaultCss, langArray,
				out splitFiles);

			var contentFolder = CreateContentStructure(projInfo, inProcess, tempFolder);

			AddEndNotesIfNecessary(inProcess, contentFolder, preProcessor, splitFiles);

			if (!FontEmbedding(projInfo, inProcess, langArray, mergedCss, contentFolder, curdir, myCursor)) return false;

			CopyContentToEpubFolder(inProcess, mergedCss, defaultCss, htmlFiles, contentFolder);

			InsertChapterLinks(inProcess, contentFolder);

			UpdateHyperlinks(inProcess, contentFolder);

			ProcessImages(inProcess, tempFolder, contentFolder);

			CreateEpubManifest(projInfo, inProcess, contentFolder, bookId, epubToc, tempCssFile);

			var epub3Path = MakeCopyOfEpub2(inProcess, projInfo);

			RemoveAudioVisual(inProcess, contentFolder);

			string outputPathWithFileName;
			var fileName = PackageEpub2(projInfo, inProcess, contentFolder, outputFolder, out outputPathWithFileName);

			var outputPathWithFileNameV3 = PackageEpub3(projInfo, inProcess, epub3Path);

			outputPathWithFileName = ValidateEpubs(inProcess, outputPathWithFileName, outputFolder, fileName,
				ref outputPathWithFileNameV3);

			CleanUp(inProcess, outputPathWithFileName, outputPathWithFileNameV3);

			CreateArchiveSubmission(projInfo, inProcess, epub3Path);

			FinalCleanUp(inProcess, outputPathWithFileName, outputPathWithFileNameV3);

			#region Close Reporting

			inProcess.Close();

			Environment.CurrentDirectory = curdir;
			Cursor.Current = myCursor;

			#endregion Close Reporting

			return success;
		}

		protected static void FinalCleanUp(InProcess inProcess, string outputPathWithFileName, string outputPathWithFileNameV3)
		{
			inProcess.SetStatus("Final Clean up");
			Common.CleanupExportFolder(outputPathWithFileName, ".xhtml,.xml,.css", string.Empty, "Test,pictures,AudioVisual");
			if (!Common.Testing)
			{
				Common.CleanupExportFolder(outputPathWithFileNameV3, ".xhtml,.xml,.css", string.Empty, "Test,pictures,AudioVisual");
				var folder =Path.GetDirectoryName(Path.GetDirectoryName(outputPathWithFileNameV3));
				var pictures = Path.Combine(folder, "pictures");
				if (Directory.Exists(pictures))
				{
					try
					{
						Directory.Delete(pictures, true);
					}
					catch
					{
						// don't worry if we can't clean up pictures folder
					}
			    }
			    var audioVisual = Path.Combine(folder, "AudioVisual");
			    if (Directory.Exists(audioVisual))
			    {
					try
					{
						Directory.Delete(audioVisual, true);
					}
					catch
					{
						// don't worry if we can't clean up folder
					}
			    }
		    }
		    inProcess.PerformStep();
	    }

	    protected void CreateArchiveSubmission(PublicationInformation projInfo, InProcess inProcess, string epub3Path)
	    {
		    inProcess.SetStatus("Archive");
		    CreateRAMP(projInfo);
		    if (!Common.Testing)
		    {
			    projInfo.DefaultXhtmlFileWithPath = Common.PathCombine(epub3Path, Path.GetFileName(projInfo.DefaultXhtmlFileWithPath));
			    CreateRAMP(projInfo);
		    }
		    inProcess.PerformStep();
	    }

	    protected static void CleanUp(InProcess inProcess, string outputPathWithFileName, string outputPathWithFileNameV3)
	    {
		    inProcess.SetStatus("Clean up");
		    Common.CleanupExportFolder(outputPathWithFileName, ".tmp,.de", "_1.x", string.Empty);
		    if (!Common.Testing)
		    {
			    Common.CleanupExportFolder(outputPathWithFileNameV3, ".tmp,.de,.zip", "_1.x", "META-INF,OEBPS");
		    }
		    inProcess.PerformStep();
	    }

	    protected string ValidateEpubs(InProcess inProcess, string outputPathWithFileName, string outputFolder, string fileName,
		    ref string outputPathWithFileNameV3)
	    {
		    inProcess.SetStatus("Validate");

		    #region Option Dialog box

		    bool isOutputDilalogNeeded = true;
		    if (!Common.Testing)
		    {
			    string caption = LocalizationManager.GetString("EpubExportTypeDlg.ValidateAndDisplay.Caption", "Export Complete", "");
			    if (Utils.MsgBox(
				        LocalizationManager.GetString("Exportepub.ValidateMsgBox",
					        Resources.ExportCallingEpubValidator + "\r\n Do you want to Validate ePub files?"), caption,
				        MessageBoxButtons.YesNo,
				        MessageBoxIcon.Information) == DialogResult.Yes)
			    {
				    ValidateResult(outputPathWithFileName); // Epub2 ExportType
				    ValidateResult(outputPathWithFileNameV3); //Epub3 ExportType
			    }

			    #region Option Dialog box

			    if (!Common.Testing && isOutputDilalogNeeded)
			    {
				    outputPathWithFileName = RenameEpubFileName(outputPathWithFileName, "epub2");
				    outputPathWithFileNameV3 = RenameEpubFileName(outputPathWithFileNameV3, "epub3");

				    var exportTypeDlg = new EpubExportTypeDlg();
				    exportTypeDlg.ShowDialog();
				    if (exportTypeDlg.ExportType == "epub2")
				    {
					    outputFolder = Path.GetDirectoryName(outputPathWithFileName);
					    DisplayOutput(outputFolder, fileName, ref outputPathWithFileName);
				    }
				    else if (exportTypeDlg.ExportType == "epub3")
				    {
					    outputFolder = Path.GetDirectoryName(outputPathWithFileNameV3);
					    DisplayOutput(outputFolder, fileName, ref outputPathWithFileNameV3);
				    }
				    else if (exportTypeDlg.ExportType == "folder")
				    {
					    if (_isUnixOs)
					    {
						    SubProcess.Run("", "nautilus", Common.HandleSpaceinLinuxPath(Path.GetDirectoryName(outputFolder)), false);
					    }
					    else
					    {
						    SubProcess.Run(Path.GetDirectoryName(outputFolder), "explorer.exe", Path.GetDirectoryName(outputFolder), false);
					    }
				    }
			    }

			    #endregion Option Dialog box
		    }

		    #endregion Option Dialog box

		    inProcess.PerformStep();
		    return outputPathWithFileName;
	    }

	    protected string PackageEpub3(PublicationInformation projInfo, InProcess inProcess, string epub3Path)
	    {
		    inProcess.SetStatus("Packaging for Epub3");

		    string fileNameV3 = CreateFileNameFromTitle(projInfo);
		    string outputPathWithFileNameV3 = null;
		    if (epub3Path != null)
		    {
			    Compress(epub3Path, Common.PathCombine(epub3Path, fileNameV3));
			    outputPathWithFileNameV3 = Common.PathCombine(epub3Path, fileNameV3) + ".epub";
		    }
#if (TIME_IT)
            TimeSpan tsTotal = DateTime.Now - dt1;
            Debug.WriteLine("Exportepub: time spent in .epub conversion: " + tsTotal);
#endif
		    inProcess.PerformStep();
		    return outputPathWithFileNameV3;
	    }

	    protected string PackageEpub2(PublicationInformation projInfo, InProcess inProcess, string contentFolder,
		    string outputFolder, out string outputPathWithFileName)
	    {
		    inProcess.SetStatus("Packaging for Epub2");
		    if (_isUnixOs)
		    {
			    AddDtdInXhtml(contentFolder);
		    }
		    string fileName = CreateFileNameFromTitle(projInfo);
		    Compress(projInfo.TempOutputFolder, Common.PathCombine(outputFolder, fileName));
		    outputPathWithFileName = Common.PathCombine(outputFolder, fileName) + ".epub";
		    return fileName;
	    }

	    protected string MakeCopyOfEpub2(InProcess inProcess, PublicationInformation projInfo)
	    {
		    string epub3Path = projInfo.ProjectPath;
		    epub3Path = Common.PathCombine(epub3Path, "Epub3");
		    Common.CopyFolderandSubFolder(projInfo.TempOutputFolder, epub3Path, true);
		    if (File.Exists(projInfo.DefaultXhtmlFileWithPath))
			    File.Copy(projInfo.DefaultXhtmlFileWithPath,
				    Common.PathCombine(epub3Path, Path.GetFileName(projInfo.DefaultXhtmlFileWithPath)), true);

		    if (File.Exists(projInfo.DefaultCssFileWithPath))
			    File.Copy(projInfo.DefaultCssFileWithPath,
				    Common.PathCombine(epub3Path, Path.GetFileName(projInfo.DefaultCssFileWithPath)), true);

		    if (File.Exists(projInfo.DefaultRevCssFileWithPath))
			    File.Copy(projInfo.DefaultRevCssFileWithPath,
				    Common.PathCombine(epub3Path, Path.GetFileName(projInfo.DefaultRevCssFileWithPath)), true);

			var avFolder = Path.Combine(projInfo.ProjectPath, "AudioVisual");
			if (Directory.Exists(avFolder))
			{
				FolderTree.Copy(avFolder, Path.Combine(epub3Path, "OEBPS", "AudioVisual"));
			}

			_exportEpub3 = new Epub3Transformation(this, _epubFont);
		    _exportEpub3.Epub3Directory = epub3Path;
		    _exportEpub3.InProcess = inProcess;
		    _exportEpub3.Export(projInfo);
		    return epub3Path;
	    }

		private void RemoveAudioVisual(InProcess inProcess, string contentFolder)
		{
			XmlDocument xmlDocument = Common.DeclareXMLDocument(false);
			var namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
			namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
			var xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, DtdProcessing = DtdProcessing.Parse };
			var files = Directory.GetFiles(contentFolder, "PartFile*.*");
			inProcess.AddToMaximum(files.Length);
			foreach (var file in files)
			{
				inProcess.PerformStep();
				xmlDocument.RemoveAll();
				var xmlReader = XmlReader.Create(file, xmlReaderSettings);
				xmlDocument.Load(xmlReader);
				xmlReader.Close();
				var scriptAttrNode = xmlDocument.SelectSingleNode("//@onclick");
				if (scriptAttrNode == null) continue;
				var audioNodes = xmlDocument.SelectNodes("//xhtml:audio", namespaceManager);
				foreach (XmlElement node in audioNodes)
				{
					node.ParentNode.RemoveChild(node.NextSibling);
					node.ParentNode.RemoveChild(node);
				}
				xmlDocument.Save(file);
			}
		}

		protected void CreateEpubManifest(PublicationInformation projInfo, InProcess inProcess, string contentFolder, Guid bookId,
		    EpubToc epubToc, string tempCssFile)
	    {
		    inProcess.SetStatus("Generating .epub TOC and manifest");
		    _epubManifest.CreateOpf(projInfo, contentFolder, bookId);
			epubToc.CreateNcx(projInfo, contentFolder, bookId);
			ModifyTOCFile(contentFolder);
			ReplaceEmptyHrefandXmlLangtoLang(inProcess, contentFolder);
		    if (File.Exists(tempCssFile))
		    {
			    File.Delete(tempCssFile);
		    }
		    inProcess.PerformStep();
	    }

	    protected void ProcessImages(InProcess inProcess, string tempFolder, string contentFolder)
	    {
		    inProcess.SetStatus("Processing images");
		    ProcessImages(tempFolder, contentFolder);
		    inProcess.PerformStep();
	    }

	    protected void UpdateHyperlinks(InProcess inProcess, string contentFolder)
	    {
#if (TIME_IT)
            DateTime dtRefStart = DateTime.Now;
#endif
		    inProcess.SetStatus("Processing hyperlinks");
		    if (InputType.ToLower() == "scripture" && References.Contains("End"))
		    {
			    UpdateReferenceHyperlinks(contentFolder, inProcess);
			    UpdateReferenceSourcelinks(contentFolder, inProcess);
		    }
		    FixRelativeHyperlinks(contentFolder);

#if (TIME_IT)
            TimeSpan tsRefTotal = DateTime.Now - dtRefStart;
            Debug.WriteLine("Exportepub: time spent fixing reference hyperlinks: " + tsRefTotal);
#endif
		    inProcess.PerformStep();
	    }

	    protected void InsertChapterLinks(InProcess inProcess, string contentFolder)
	    {
		    inProcess.SetStatus("Insert Chapter Links");
		    InsertChapterLinkBelowBookName(contentFolder);
		    InsertReferenceLinkInTocFile(contentFolder);
		    inProcess.PerformStep();
	    }

	    protected void CopyContentToEpubFolder(InProcess inProcess, string mergedCss, string defaultCss, List<string> htmlFiles,
		    string contentFolder)
	    {
		    inProcess.SetStatus("Copy contents and styles to Epub");
		    CopyStylesAndContentToEpub(inProcess, mergedCss, defaultCss, htmlFiles, contentFolder);
		    inProcess.PerformStep();
	    }

	    protected bool FontEmbedding(PublicationInformation projInfo, InProcess inProcess, string[] langArray, string mergedCss,
		    string contentFolder, string curdir, Cursor myCursor)
	    {
		    inProcess.SetStatus("Processing fonts");
		    if (!FontProcessing(projInfo, langArray, mergedCss, contentFolder))
		    {
			    // user cancelled the epub conversion - clean up and exit
			    Environment.CurrentDirectory = curdir;
			    Cursor.Current = myCursor;
			    inProcess.Close();
			    return false;
		    }
		    inProcess.PerformStep();
		    return true;
	    }

	    protected void AddEndNotesIfNecessary(InProcess inProcess, string contentFolder, PreExportProcess preProcessor,
		    List<string> splitFiles)
	    {
// extract references file if specified
		    if (References.Contains("End") && InputType.ToLower().Equals("scripture"))
		    {
			    inProcess.SetStatus("Creating endnote references file");
			    CreateReferencesFile(contentFolder, preProcessor.ProcessedXhtml);
			    splitFiles.Add(Common.PathCombine(contentFolder, ReferencesFilename));
		    }
	    }

	    protected string CreateContentStructure(PublicationInformation projInfo, InProcess inProcess, string tempFolder)
	    {
		    inProcess.SetStatus("Creating structure");
		    string contentFolder = CreateContentStructure(projInfo, tempFolder);
		    inProcess.PerformStep();
		    return contentFolder;
	    }

	    protected List<string> SplitIntoSections(PublicationInformation projInfo, InProcess inProcess, List<string> frontMatter,
		    PreExportProcess preProcessor, string defaultCss, string[] langArray, out List<string> splitFiles)
	    {
		    inProcess.SetStatus("Add Sections");
		    var htmlFiles = new List<string>();
		    splitFiles = new List<string>();
		    splitFiles.AddRange(frontMatter);
		    SplittingFrontMatter(projInfo, preProcessor, defaultCss, splitFiles);
		    SplittingReversal(projInfo, langArray, defaultCss, splitFiles);
		    AddBooksMoveNotes(inProcess, htmlFiles, splitFiles);
		    inProcess.PerformStep();
		    return htmlFiles;
	    }

	    protected List<string> AddFrontMatter(InProcess inProcess, PreExportProcess preProcessor, string tempFolder)
	    {
// insert the front matter items as separate files in the output folder
		    inProcess.SetStatus("Adding Front Matter");
		    preProcessor.SkipChapterInformation = TocLevel;
		    var frontMatter = preProcessor.InsertFrontMatter(tempFolder, false);
		    inProcess.PerformStep();
		    return frontMatter;
	    }

	    protected static void AddNavitation(InProcess inProcess, PreExportProcess preProcessor)
	    {
		    inProcess.SetStatus("Adding Navigation");
		    preProcessor.PrepareBookNameAndChapterCount();
		    inProcess.PerformStep();
	    }

	    protected void FixIssuesWithFlexXhtml(PublicationInformation projInfo, PreExportProcess preProcessor, string[] langArray,
		    InProcess inProcess)
	    {
		    XhtmlNamespaceHack(projInfo, preProcessor, langArray);
		    Common.ApplyXslt(preProcessor.ProcessedXhtml, noXmlSpace);
		    Common.ApplyXslt(preProcessor.ProcessedXhtml, fixEpub);
		    inProcess.PerformStep();
	    }

	    protected string ProcessingCss(PublicationInformation projInfo, InProcess inProcess, PreExportProcess preProcessor,
		    out string mergedCss, out string defaultCss, out string tempCssFile)
	    {
		    inProcess.SetStatus("Preprocessing stylesheet");
		    var cssFullPath = CssFullPath(projInfo);
		    Common.WriteDefaultLanguages(projInfo, cssFullPath);
		    var tempFolder = Path.GetDirectoryName(preProcessor.ProcessedXhtml);
		    mergedCss = MergeAndFilterCss(preProcessor, tempFolder, cssFullPath);
		    /* Modify the content in css file for before after css style process */
		    preProcessor.InsertPseudoContentProperty(mergedCss, PseudoClass);
		    CustomizeCss(mergedCss);
		    var niceNameCss = NiceNameCss(projInfo, tempFolder, ref mergedCss);
		    defaultCss = Path.GetFileName(niceNameCss);
		    tempCssFile = mergedCss.Replace(".css", "tmp.css");
		    File.Copy(mergedCss, tempCssFile, true);

		    Common.SetDefaultCSS(projInfo.DefaultXhtmlFileWithPath, defaultCss);
		    Common.SetDefaultCSS(preProcessor.ProcessedXhtml, defaultCss);
		    if (!File.Exists(mergedCss))
		    {
			    File.Copy(tempCssFile, mergedCss, true);
		    }

		    inProcess.PerformStep();
		    return tempFolder;
	    }

	    protected string PreprocessXhtml(PublicationInformation projInfo, InProcess inProcess, PreExportProcess preProcessor,
		    out string[] langArray)
	    {
		    inProcess.SetStatus("Preprocessing content");

		    var glossorywords = WriteGlossaryLink(projInfo);
		    GlossaryLinkReferencing(projInfo, glossorywords);

		    InsertBeforeAfterInXhtml(projInfo);
		    var outputFolder = SetOutputFolderAndCurrentDirectory(projInfo);
		    Common.SetProgressBarValue(projInfo.ProgressBar, projInfo.DefaultXhtmlFileWithPath);
		    XhtmlPreprocessing(projInfo, preProcessor);
		    langArray = _epubFont.InitializeLangArray(projInfo);
		    inProcess.PerformStep();
		    return outputFolder;
	    }

	    protected Guid SetupConversion(PublicationInformation projInfo, InProcess inProcess, out PreExportProcess preProcessor,
		    out EpubToc epubToc)
	    {
		    inProcess.SetStatus("Setup");
		    var bookId = Guid.NewGuid(); // NOTE: this creates a new ID each time Pathway is run.
		    PageBreak = InputType.ToLower() == "dictionary" && GetPageBreakStatus(projInfo.SelectedTemplateStyle);

		    #region LoadXslts

		    addRevId = LoadAddRevIdXslt();
		    noXmlSpace = LoadNoXmlSpaceXslt();
		    fixEpub = LoadFixEpubXslt();
		    Param.SetLoadType = projInfo.ProjectInputType;
		    Param.LoadSettings();

		    #endregion

		    #region Create EpubFolder

		    if (!Common.Testing)
		    {
			    CreateEpubFolder(projInfo);
		    }
		    else
		    {
			    if (!String.IsNullOrEmpty(projInfo.DictionaryPath))
				    projInfo.ProjectPath = projInfo.DictionaryPath;
			    else if (!String.IsNullOrEmpty(projInfo.ProjectPath))
				    projInfo.DictionaryPath = projInfo.ProjectPath;
		    }

		    #endregion

		    preProcessor = new PreExportProcess(projInfo);
		    preProcessor.RemoveBrokenImage();

		    _isUnixOs = Common.UnixVersionCheck();
		    if (_isUnixOs)
		    {
			    Common.RemoveDTDForLinuxProcess(projInfo.DefaultXhtmlFileWithPath, "epub");
		    }

		    _isIncludeImage = GetIncludeImageStatus(projInfo.SelectedTemplateStyle);

		    _isNoteTargetReferenceExists = Common.NodeExists(projInfo.DefaultXhtmlFileWithPath, "");

		    _epubFont = new EpubFont(this);
		    _epubManifest = new EpubManifest(this, _epubFont);
		    _epubManifest.LoadPropertiesFromSettings();
		    LoadOtherFeatures();
		    epubToc = new EpubToc(projInfo.ProjectInputType, TocLevel);
		    inProcess.PerformStep();
		    return bookId;
	    }

	    private static string RenameEpubFileName(string oldEpubFileName, string epubVersion)
        {
            string newEpubFileName = oldEpubFileName.Replace(".epub", "_" + epubVersion + ".epub");
            File.Move(oldEpubFileName, newEpubFileName);
            return newEpubFileName;
        }

        protected void ReplaceEmptyHrefandXmlLangtoLang(InProcess inProcess, string contentFolder)
        {
            string[] files = Directory.GetFiles(contentFolder, "*.xhtml");
			inProcess?.AddToMaximum(files.Length);
            foreach (string file in files)
            {
				Common.StreamReplaceInFile(file, " xml:lang=\"", " lang=\"");
				Common.StreamReplaceInFile(file, "a href=\"#\"", "a");
				Common.StreamReplaceInFile(file, " lang=\"\"", "");
				inProcess?.PerformStep();
            }
        }
        private void GlossaryLinkReferencing(PublicationInformation projInfo,Dictionary<string,Dictionary<string,string>> glossoryreferncelist)
        {
            string tocFiletoUpdate = projInfo.DefaultXhtmlFileWithPath;
            XmlDocument xmlDoc = Common.DeclareXMLDocument(true);
            var namespaceManager = new XmlNamespaceManager(xmlDoc.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");

            if (!File.Exists(tocFiletoUpdate))
                return;
            xmlDoc.Load(tocFiletoUpdate);
            XmlNodeList tagList = xmlDoc.GetElementsByTagName("a");
            if (tagList.Count > 0)
            {
                foreach (KeyValuePair<string, Dictionary<string, string>> glossory in glossoryreferncelist)
                {
                    string glossorykey = glossory.Key;
                    Dictionary<string, string> glossoryvalue = glossory.Value;

                    foreach (XmlNode tagValue in tagList)
                    {
                        if (tagValue.Attributes != null && (tagValue.Attributes.Count > 0 && tagValue.Attributes["id"] != null) && tagValue.Attributes["class"].Value == "Glossary_Key")
                        {
                            if (glossoryvalue.ContainsKey(tagValue.Attributes["id"].Value))
                            {
                                tagValue.Attributes["href"].Value = "#" + glossorykey;
                                string xpathkeyword = ".//xhtml:a[@class='Glossaryvaluehref'][@id='" + glossorykey + "']";
                                XmlNode keywordnode = xmlDoc.SelectSingleNode(xpathkeyword, namespaceManager);
                                if (keywordnode != null && keywordnode.Attributes != null &&
                                    tagValue.Attributes["id"].Value.Length > 0)
                                {
                                    if (keywordnode.Attributes["href"].Value.Replace("#","").Trim().Length == 0)
                                    {
                                        keywordnode.Attributes["href"].Value = "#" + tagValue.Attributes["id"].Value;
                                    }
                                }
                            }
                        }
                    }

                }
            }
            xmlDoc.Save(tocFiletoUpdate);
        }

        private static void ModifyTOCFile(string oebpsPath)
        {
            //NCX to XHTML
            string ncxfile = Common.PathCombine(oebpsPath, "toc.ncx");
            string ncxPath = Common.PathCombine(oebpsPath, "toc1.ncx");
            File.Copy(ncxfile, ncxPath);
            string xhtmlPath = Common.PathCombine(oebpsPath, "File3TOC00000_.xhtml");
            if (File.Exists(ncxPath))
            {
                var tocHtml = CreateFileTocByNcx();
                Common.ApplyXslt(ncxPath, tocHtml);
                File.Copy(ncxPath, xhtmlPath, true);
                File.Delete(ncxPath);
            }
        }

        public static XslCompiledTransform CreateFileTocByNcx()
        {
            var ncxtoxhtmlStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("epubConvert.NcxToXhtml.xsl");
            Debug.Assert(ncxtoxhtmlStream != null);
            var ncxtoxhtml = new XslCompiledTransform();
            ncxtoxhtml.Load(XmlReader.Create(ncxtoxhtmlStream));
            return ncxtoxhtml;
        }

        private Dictionary<string, Dictionary<string, string>> WriteGlossaryLink(PublicationInformation projInfo)
        {
            Dictionary<string, Dictionary<string, string>> glossorywordsDictionaries = new Dictionary<string, Dictionary<string, string>>();

            string file = projInfo.DefaultXhtmlFileWithPath;
            XmlDocument xmlDocument = Common.DeclareXMLDocument(true);
            var namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
			var xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, DtdProcessing = DtdProcessing.Parse };
            //Common.DeclareXmlReaderSettings(false);

            if (!File.Exists(file))
                return null;

            XmlReader xmlReader = XmlReader.Create(file, xmlReaderSettings);
            xmlDocument.Load(xmlReader);
            xmlReader.Close();
            if (projInfo.ProjectInputType.ToLower().Equals("scripture"))
            {
                const string xPath = ".//xhtml:a[@class='Glossaryvaluehref']";
                XmlNodeList nodes = xmlDocument.SelectNodes(xPath, namespaceManager);
                Debug.Assert(nodes != null);
                if (nodes.Count > 0)
                {
                    foreach (XmlNode booknode in nodes)
                    {
                        Dictionary<string, string> glossorywordsDictionary = new Dictionary<string,string>();
                        string booknodeid = booknode.Attributes["id"].Value;
                        string booknodetext = booknode.InnerText.Trim();
                        string xpathkeyword = ".//xhtml:a[@class='Glossary_Key'][translate(text(),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='" + booknodetext.ToLower() + "' or translate(@title,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='" + booknodetext.ToLower() + "']";
                        XmlNodeList keywordnodesList = xmlDocument.SelectNodes(xpathkeyword, namespaceManager);
                        if (keywordnodesList.Count > 0)
                        {
                            foreach (XmlNode keywordnode in keywordnodesList)
                            {
                                string glossoryid = keywordnode.Attributes["id"].Value;
                                if (glossoryid != null)
                                {
                                    glossorywordsDictionary.Add(glossoryid,keywordnode.InnerText.Trim());
                                }
                            }
                        }
                        if (glossorywordsDictionary.Count > 0)
                        {

                            glossorywordsDictionaries.Add(booknodeid,glossorywordsDictionary);
                        }
                    }
                }
            }
            return glossorywordsDictionaries;
        }
        private static void CreateEpubFolder(PublicationInformation projInfo)
        {
            Common.CopyFolderandSubFolder(projInfo.DictionaryPath, Common.PathCombine(projInfo.DictionaryPath, "Epub2"), true);
            var di = new DirectoryInfo(projInfo.DictionaryPath);
            Common.CleanFile(di);
            projInfo.DefaultXhtmlFileWithPath = Common.PathCombine(Common.PathCombine(projInfo.DictionaryPath, "Epub2"),
                                                                   Path.GetFileName(projInfo.DefaultXhtmlFileWithPath));
            projInfo.DefaultCssFileWithPath = Common.PathCombine(Common.PathCombine(projInfo.DictionaryPath, "Epub2"),
                                                                 Path.GetFileName(projInfo.DefaultCssFileWithPath));
            projInfo.DefaultRevCssFileWithPath = Common.PathCombine(Common.PathCombine(projInfo.DictionaryPath, "Epub2"),
                                                                    Path.GetFileName(projInfo.DefaultRevCssFileWithPath));

            projInfo.ProjectPath = projInfo.DictionaryPath;
            projInfo.DictionaryPath = Common.PathCombine(projInfo.DictionaryPath, "Epub2");
        }

        private void LoadOtherFeatures()
        {
            string layout = Param.GetItem("//settings/property[@name='LayoutSelected']/@value").Value;
            Dictionary<string, string> othersfeature = Param.GetItemsAsDictionary("//stylePick/styles/others/style[@name='" + layout + "']/styleProperty");
            EmbedFonts = !othersfeature.ContainsKey("EmbedFonts") || (othersfeature["EmbedFonts"].Trim().Equals("Yes"));
            IncludeFontVariants = !othersfeature.ContainsKey("IncludeFontVariants") || (othersfeature["IncludeFontVariants"].Trim().Equals("Yes"));
            if (othersfeature.ContainsKey("MaxImageWidth"))
            {
                try
                {
                    MaxImageWidth = int.Parse(othersfeature["MaxImageWidth"].Trim());
                }
                catch (Exception)
                {
                    MaxImageWidth = 600;
                }
            }
            else
            {
                MaxImageWidth = 600;
            }
            TocLevel = othersfeature.ContainsKey("TOCLevel") ? othersfeature["TOCLevel"].Trim() : "";
            DefaultFont = othersfeature.ContainsKey("DefaultFont") ? othersfeature["DefaultFont"].Trim() : "Charis SIL";
            DefaultAlignment = othersfeature.ContainsKey("DefaultAlignment") ? othersfeature["DefaultAlignment"].Trim() : "Justified";
            ChapterNumbers = othersfeature.ContainsKey("ChapterNumbers") ? othersfeature["ChapterNumbers"].Trim() : "Drop Cap";
            References = othersfeature.ContainsKey("References") ? othersfeature["References"].Trim() : "After Each Section";

            // base font size
            if (othersfeature.ContainsKey("BaseFontSize"))
            {
                try
                {
                    BaseFontSize = int.Parse(othersfeature["BaseFontSize"].Trim());
                }
                catch (Exception)
                {
                    BaseFontSize = 13;
                }
            }
            else
            {
                BaseFontSize = 13;
            }
            // default line height
            if (othersfeature.ContainsKey("DefaultLineHeight"))
            {
                try
                {
                    DefaultLineHeight = int.Parse(othersfeature["DefaultLineHeight"].Trim());
                }
                catch (Exception)
                {
                    DefaultLineHeight = 125;
                }
            }
            else
            {
                DefaultLineHeight = 125;
            }
            // Missing Font
            // Note that the Embed Font enum value doesn't apply here (if it were to appear, we'd fall to the Default
            // "Prompt user" case
            if (othersfeature.ContainsKey("MissingFont"))
            {
                switch (othersfeature["MissingFont"].Trim())
                {
                    case "Use Fallback Font":
                        MissingFont = FontHandling.SubstituteDefaultFont;
                        break;
                    case "Cancel Export":
                        MissingFont = FontHandling.CancelExport;
                        break;
                    default: // "Prompt User" case goes here
                        MissingFont = FontHandling.PromptUser;
                        break;
                }
            }
            else
            {
                MissingFont = FontHandling.PromptUser;
            }
            // Non SIL Font
            if (othersfeature.ContainsKey("NonSILFont"))
            {
                switch (othersfeature["NonSILFont"].Trim())
                {
                    case "Embed Font Anyway":
                        NonSilFont = FontHandling.EmbedFont;
                        break;
                    case "Use Fallback Font":
                        NonSilFont = FontHandling.SubstituteDefaultFont;
                        break;
                    case "Cancel Export":
                        NonSilFont = FontHandling.CancelExport;
                        break;
                    default: // "Prompt User" case goes here
                        NonSilFont = FontHandling.PromptUser;
                        break;
                }
            }
            else
            {
                NonSilFont = FontHandling.PromptUser;
            }
        }

        private void ValidateResult(string outputPathWithFileName)
        {
            // Postscript - validate the file using our epubcheck wrapper
            if (Common.Testing)
            {
                // Running the unit test - just run the validator and return the result
                var validationResults = Program.ValidateFile(outputPathWithFileName);
                Debug.WriteLine("Exportepub: validation results: " + validationResults);
            }
            else
            {
                ValidateEpub.ValidateEpubFile(outputPathWithFileName);
            }
        }

        private void DisplayOutput(string outputFolder, string fileName, ref string outputPathWithFileName)
        {
            if (File.Exists(outputPathWithFileName))
            {
                if (_isUnixOs)
                {
                    string epubFileName = Common.PathCombine(outputFolder, fileName.Replace(" ", "") + ".epub");
                    File.Move(outputPathWithFileName, epubFileName);
                    outputPathWithFileName = epubFileName;
                    SubProcess.Run(outputFolder, "ebook-viewer", epubFileName, false);
                }
                else
                {
                    Process.Start(outputPathWithFileName);
                }
            }
        }

	    protected string CreateFileNameFromTitle(PublicationInformation projInfo)
        {
            string fileName = Path.GetFileNameWithoutExtension(projInfo.DefaultXhtmlFileWithPath);
            if (!Common.Testing)
            {
                fileName = _epubManifest.Title;
                if (string.IsNullOrEmpty(_epubManifest.Title))
                {
                    fileName = Path.GetFileNameWithoutExtension(projInfo.DefaultXhtmlFileWithPath);
                }
                fileName = Common.ReplaceSymbolToUnderline(fileName);
            }
            return fileName;
        }

        private void CopyStylesAndContentToEpub(InProcess inProcess, string mergedCss, string defaultCss, List<string> htmlFiles, string contentFolder)
        {
            var cssPath = Common.PathCombine(contentFolder, defaultCss);
            if (File.Exists(mergedCss))
                File.Copy(mergedCss, cssPath, true);

            var tocFiletoUpdate = string.Empty;
            SplitPageSections(inProcess, htmlFiles, contentFolder, tocFiletoUpdate);
            RemoveDuplicateBookName(contentFolder);
        }

        private bool FontProcessing(PublicationInformation projInfo, string[] langArray, string mergedCss, string contentFolder)
        {
            // First, get the list of fonts used in this project
            _epubFont.BuildFontsList();
            // Embed fonts if needed
            if (EmbedFonts)
            {
                if (!_epubFont.EmbedAllFonts(langArray, contentFolder))
                {
                    return false; // user aborted
                }
            }
            // update the CSS file to reference any fonts used by the writing systems
            // (if they aren't embedded in the .epub, we'll still link to them here)

            _epubFont.ReferenceFonts(mergedCss, projInfo);
            return true; // successful
        }

        private string CreateContentStructure(PublicationInformation projInfo, string tempFolder)
        {
            var sb = new StringBuilder();
            sb.Append(tempFolder);
            sb.Append(Path.DirectorySeparatorChar);
            sb.Append("epub");
            string strFromOfficeFolder = Common.PathCombine(Common.GetPSApplicationPath(), "epub");
			if (!Directory.Exists(strFromOfficeFolder))
			{
				strFromOfficeFolder = Path.GetDirectoryName(Common.AssemblyPath);
				strFromOfficeFolder = Common.PathCombine(strFromOfficeFolder, "epub");
			}

            projInfo.TempOutputFolder = sb.ToString();
            CopyFolder(strFromOfficeFolder, projInfo.TempOutputFolder);
            // set the folder where our epub content goes
            sb.Append(Path.DirectorySeparatorChar);
            sb.Append("OEBPS");
            string contentFolder = sb.ToString();
            if (!Directory.Exists(contentFolder))
            {
                Directory.CreateDirectory(contentFolder);
            }
            return contentFolder;
        }

        private void AddBooksMoveNotes(InProcess inProcess, List<string> htmlFiles, List<string> splitFiles)
        {
            string xsltFullName = GetXsltFile();
			string getPsApplicationPath = Common.AssemblyPath;
	        string xsltProcessExe = Common.PathCombine(getPsApplicationPath, "XslProcess.exe");
			if (!File.Exists(xsltProcessExe))
			{
				getPsApplicationPath = Path.GetDirectoryName(Common.AssemblyPath);
				xsltProcessExe = Common.PathCombine(getPsApplicationPath, "XslProcess.exe");
			}

            inProcess.SetStatus("Apply Xslt Process in html file");
            if (File.Exists(xsltProcessExe))
            {
				inProcess.AddToMaximum(splitFiles.Count);
                foreach (string file in splitFiles)
                {
                    const string outputExtension = "_.xhtml";
                    var inputExtension = Path.GetExtension(file);
                    Debug.Assert(inputExtension != null);
                    var xhtmlOutputFile = file.Replace(inputExtension, outputExtension);
					if (!File.Exists(file))
						continue;
                    if (file.Contains("File2Cpy"))
                    {
                        File.Copy(file, xhtmlOutputFile);
                    }
                    else
                    {
                        Common.XsltProcess(file, xsltFullName, outputExtension);
                    }
                    if (File.Exists(xhtmlOutputFile))
                    {
                        File.Delete(file);  // clean up the un-transformed file
                    }
                    else if (File.Exists(file)) // SE version doesn't have the XSLT Transform
                    {
                        File.Move(file, xhtmlOutputFile);
                    }
                    htmlFiles.Add(xhtmlOutputFile);
					inProcess.PerformStep();
                }
            }
        }

        private void SplittingFrontMatter(PublicationInformation projInfo, PreExportProcess preProcessor, string defaultCss, List<string> splitFiles)
        {
            foreach (var file in splitFiles)
            {
                Common.SetDefaultCSS(file, defaultCss);
            }

            if ((InputType.ToLower().Equals("dictionary") && projInfo.IsLexiconSectionExist) ||
                (InputType.ToLower().Equals("scripture")))
            {
                if (projInfo.FileToProduce.ToLower() != "one")
                {
                    splitFiles.AddRange(SplitFile(preProcessor.ProcessedXhtml, projInfo));
                }
                else
                {
                    splitFiles.Add(preProcessor.ProcessedXhtml);
                }
            }

            if (_isIncludeImage == false)
            {
                foreach (string file in splitFiles)
                {
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                    if (fileNameWithoutExtension != null && fileNameWithoutExtension.IndexOf(@"PartFile", StringComparison.CurrentCulture) == 0)
                    {
                        RemoveNodeInXhtmlFile(file);
                    }
                }
            }
        }

        private void SplittingReversal(PublicationInformation projInfo, string[] langArray, string defaultCss, List<string> splitFiles)
        {
            // If we are working with a dictionary and have a reversal index, process it now)
            if (projInfo.IsReversalExist)
            {
                var revFile = Common.PathCombine(Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath), "FlexRev.xhtml");

				Common.ApplyXslt(revFile, noXmlSpace);
				Common.ApplyXslt(revFile, fixEpub);

				ReversalHacks(addRevId, langArray, defaultCss, revFile);
                // now split out the html as needed
                var fileNameWithPath = Common.SplitXhtmlFile(revFile, "letHead", "RevIndex", true);
                splitFiles.AddRange(fileNameWithPath);
            }
        }

        private void ReversalHacks(XslCompiledTransform addRevId, string[] langArray, string defaultCss, string revFile)
        {
            // EDB 10/20/2010 - TD-1629 - remove when merged CSS passes validation
            // (note that the rev file uses a "FlexRev.css", not "main.css"

            if (_isUnixOs)
            {
                Common.RemoveDTDForLinuxProcess(revFile, "epub");
            }
            Common.SetDefaultCSS(revFile, defaultCss);
            // EDB 10/29/2010 FWR-2697 - remove when fixed in FLEx
            Common.StreamReplaceInFile(revFile, "<ReversalIndexEntry_Self", "<span class='ReversalIndexEntry_Self'");
            Common.StreamReplaceInFile(revFile, "</ReversalIndexEntry_Self", "</span");
            ReversalXhtmlNamespaceHack(langArray, revFile);
            Common.ApplyXslt(revFile, addRevId);      // also removes xml:space="preserve" attributes
        }

        private static void ReversalXhtmlNamespaceHack(string[] langArray, string revFile)
        {
            if (langArray.Length > 0)
            {
                Common.StreamReplaceInFile(revFile,
                                            "<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"utf-8\" lang=\"utf-8\"",
                                            string.Format(
                                                "<html  xmlns='http://www.w3.org/1999/xhtml' xml:lang='{0}' dir='{1}'",
                                                langArray[0], Common.GetTextDirection(langArray[0])));
                Common.StreamReplaceInFile(revFile,
                                            "<html>",
                                            string.Format(
                                                "<html  xmlns='http://www.w3.org/1999/xhtml' xml:lang='{0}' dir='{1}'>",
                                                langArray[0], Common.GetTextDirection(langArray[0])));
                Common.StreamReplaceInFile(revFile, " lang=\"", " xml:lang=\"");
            }
        }

        private static void XhtmlNamespaceHack(PublicationInformation projInfo, PreExportProcess preProcessor, string[] langArray)
        {
            // EDB 10/22/2010
            // HACK: we need the preprocessed image file names (preprocessor.imageprocess()), but
            // it's missing the xml namespace that makes it a valid xhtml file. We'll add it here.
            // (The unprocessed html works fine, but doesn't have the updated links to the image files in it,
            // so we can't use it.)
            // TODO: remove this line when TE provides valid XHTML output.

            if (langArray.Length > 0)
            {
                Common.StreamReplaceInFile(preProcessor.ProcessedXhtml, "<html>",
                                            string.Format(
                                                "<html xmlns='http://www.w3.org/1999/xhtml' xml:lang='{0}' dir='{1}'>",
                                                langArray[0], Common.GetTextDirection(langArray[0])));
                // The TE export outputs both xml:lang and lang parameters
                if (projInfo.ProjectInputType.ToLower() == "scripture")
                    Common.StreamReplaceInFile(preProcessor.ProcessedXhtml, "xml:lang=\"utf-8\" lang=\"utf-8\"", "xml:lang=\"utf-8\"");
                Common.StreamReplaceInFile(preProcessor.ProcessedXhtml, " lang=\"", " xml:lang=\"");
            }
        }

        private static string CssFullPath(PublicationInformation projInfo)
        {
            var cssFolder = Path.GetDirectoryName(projInfo.DefaultCssFileWithPath);
            var cssFullPath = Common.PathCombine(cssFolder, "epub.css");
            if (!File.Exists(cssFullPath))
            {
                cssFullPath = projInfo.DefaultCssFileWithPath;
            }
            else
            {
                string expCssLine = "\r\n" + "@import \"" + Path.GetFileName(projInfo.DefaultCssFileWithPath) + "\";";
                Common.FileInsertText(cssFullPath, expCssLine);
            }
            return cssFullPath;
        }

        private static string NiceNameCss(PublicationInformation projInfo, string tempFolder, ref string mergedCss)
        {
            var niceNameCss = Common.PathCombine(tempFolder, "book.css");
            projInfo.DefaultCssFileWithPath = niceNameCss;
            if (niceNameCss != mergedCss)
            {
                if (File.Exists(niceNameCss))
                {
                    File.Delete(niceNameCss);
                }
                File.Copy(mergedCss, niceNameCss);
                mergedCss = niceNameCss;
            }
            return niceNameCss;
        }

        private static MergeCss _mc; // When mc is disposed it also deletes the merged file

        private static string MergeAndFilterCss(PreExportProcess preProcessor, string tempFolder, string cssFullPath)
        {
            var tempFolderName = Path.GetFileName(tempFolder);
            _mc = new MergeCss { OutputLocation = tempFolderName };
            var mergedCss = _mc.Make(cssFullPath, "book.css");
            preProcessor.RemoveDeclaration(mergedCss, "@top-");
            preProcessor.RemoveDeclaration(mergedCss, "@bottom-");
            preProcessor.RemoveDeclaration(mergedCss, "@footnote");
            preProcessor.RemoveDeclaration(mergedCss, "@page");
            preProcessor.RemoveStringInCss(mergedCss, "string-set:");
            preProcessor.RemoveStringInCss(mergedCss, "-moz-column-");
            preProcessor.RemoveStringInCss(mergedCss, "column-fill:");
            preProcessor.RemoveStringInCss(mergedCss, "-ps-outline-");
            preProcessor.RemoveStringInCss(mergedCss, "float:");
            preProcessor.RemoveStringInCss(mergedCss, "-ps-fixed-line-height:");
            preProcessor.RemoveStringInCss(mergedCss, "content: leader(");
            preProcessor.ReplaceStringInCss(mergedCss);
            preProcessor.SetDropCapInCSS(mergedCss);
            preProcessor.InsertCoverPageImageStyleInCSS(mergedCss);
            preProcessor.InsertSectionHeadID();
            return mergedCss;
        }

        private static string SetOutputFolderAndCurrentDirectory(PublicationInformation projInfo)
        {
            var outputFolder = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath); // finished .epub goes here
            Debug.Assert(outputFolder != null);
            Environment.CurrentDirectory = outputFolder;
            return outputFolder;
        }

        private static void XhtmlPreprocessing(PublicationInformation projInfo, PreExportProcess preProcessor)
        {
            Common.StreamReplaceInFile(preProcessor.ProcessedXhtml, "&nbsp;", Common.NonBreakingSpace);
            preProcessor.GetTempFolderPath();
            preProcessor.ImagePreprocess(false);
            preProcessor.MoveBookcodeFRTtoFront(preProcessor.ProcessedXhtml);
            preProcessor.SetTitleValueOnReversal(projInfo.DefaultXhtmlFileWithPath);
            if (projInfo.SwapHeadword)
            {
                preProcessor.SwapHeadWordAndReversalForm();
            }
        }

        protected static XslCompiledTransform LoadFixEpubXslt()
        {
            var fixEpub = new XslCompiledTransform();
            fixEpub.Load(XmlReader.Create(Common.UsersXsl("FixEpub.xsl")));
            return fixEpub;
        }

		private static XslCompiledTransform LoadNoXmlSpaceXslt()
        {
            var noXmlSpaceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("epubConvert.noXmlSpace.xsl");
            Debug.Assert(noXmlSpaceStream != null);
            var noXmlSpace = new XslCompiledTransform();
            noXmlSpace.Load(XmlReader.Create(noXmlSpaceStream));
            return noXmlSpace;
        }

        private static XslCompiledTransform LoadAddRevIdXslt()
        {
            var addRevIdStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("epubConvert.addRevId.xsl");
            Debug.Assert(addRevIdStream != null);
            var addRevId = new XslCompiledTransform();
            addRevId.Load(XmlReader.Create(addRevIdStream));
            return addRevId;
        }

        private void CreateRAMP(PublicationInformation projInfo)
        {
            var ramp = new Ramp { ProjInputType = projInfo.ProjectInputType };
            ramp.Create(projInfo.DefaultXhtmlFileWithPath, ".epub", projInfo.ProjectInputType);
        }

        protected void SplitPageSections(InProcess inProcess, List<string> htmlFiles, string contentFolder, string tocFiletoUpdate)
        {
	        inProcess?.AddToMaximum(htmlFiles.Count);
	        foreach (string file in htmlFiles)
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
	            if (fileNameWithoutExtension == null) continue;
	            var prefixLen = fileNameWithoutExtension.Length > 7 ? 8 : 1;
	            string name = fileNameWithoutExtension.Substring(0, prefixLen);
	            string substring = fileNameWithoutExtension.Substring(prefixLen);
	            string dest = Common.PathCombine(contentFolder, name + substring.PadLeft(6, '0') + ".xhtml");

	            if (_isUnixOs)
	            {
		            Common.RemoveDTDForLinuxProcess(file, "epub");
	            }

	            if (File.Exists(file))
	            {
		            File.Move(file, dest);
	            }

	            // split the file into smaller pieces if needed
	            var files = new List<string>();

	            if (!PageBreak && InputType.ToLower() == "dictionary")
	            {
		            files = SplitBook(dest);
	            }

	            if (InputType.ToLower() == "scripture")
	            {
		            files = SplitBook(dest);
	            }

	            if (files != null && files.Count > 1)
	            {
		            if (File.Exists(dest))
			            File.Delete(dest);
	            }

	            if (dest.Contains("File3TOC"))
	            {
		            tocFiletoUpdate = dest;
		            GetTocId(tocFiletoUpdate);
	            }

	            if (files != null && files.Count > 0 && files[0].Contains("PartFile"))
	            {
		            MapTocIdAndSectionHeadId(files);
	            }
	            inProcess?.PerformStep();
            }
            UpdateTocIdAfterFileSplit(tocFiletoUpdate);
        }

        private void UpdateTocIdAfterFileSplit(string tocFiletoUpdate)
        {
            if (tocFiletoUpdate == string.Empty || !File.Exists(tocFiletoUpdate))
            {
                return;
            }
            XmlDocument xmlDoc = Common.DeclareXMLDocument(true);
            var namespaceManager = new XmlNamespaceManager(xmlDoc.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            xmlDoc.Load(tocFiletoUpdate);
            XmlNodeList tagList = xmlDoc.GetElementsByTagName("a");
            if (tagList.Count > 0)
            {
                foreach (XmlNode tagValue in tagList)
                {
                    if (tagValue.Attributes != null && (tagValue.Attributes.Count > 0 && tagValue.Attributes["href"] != null))
                    {
                        if (_tocIdMapping.ContainsKey(tagValue.Attributes["href"].Value))
                            tagValue.Attributes["href"].Value = _tocIdMapping[tagValue.Attributes["href"].Value];
                    }
                }
            }
            xmlDoc.Save(tocFiletoUpdate);
        }

        private void MapTocIdAndSectionHeadId(List<string> files)
        {
            var hasBookmark = false;
            if (files.Count > 0)
            {
                var firstName = Path.GetFileName(files[0]);
                Debug.Assert(!string.IsNullOrEmpty(firstName));
                foreach (string idVal in _tocIDs)
                {
                    var name = Path.GetFileNameWithoutExtension(idVal);
                    Debug.Assert(!string.IsNullOrEmpty(name));
                    if (!idVal.Contains("#"))
                    {
                        if (firstName.Contains(name))
                            _tocIdMapping.Add(idVal, firstName);
                    }
                    else
                    {
                        hasBookmark = true;
                    }
                }
            }
            if (!hasBookmark) return;
            foreach (string partFile in files)
            {
                XmlDocument xDoc = Common.DeclareXMLDocument(true);
                var namespaceManager = new XmlNamespaceManager(xDoc.NameTable);
                namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
                xDoc.Load(partFile);
                XmlNodeList divList = xDoc.GetElementsByTagName("div");
                if (divList.Count > 0)
                {
                    foreach (XmlNode divTag in divList)
                    {
                        if (divTag.Attributes != null && divTag.Attributes.Count > 0 && divTag.Attributes["class"] != null &&
                            divTag.Attributes["id"] != null
                            && divTag.Attributes["class"].Value == "Section_Head")
                        {
                            string sectionHeadId = divTag.Attributes["id"].Value;
                            foreach (string idVal in _tocIDs)
                            {
                                if (idVal.IndexOf('#') > 0)
                                {
                                    var val = idVal.Split('#');
                                    var oldPartFileName = Common.LeftString(val[0], "_");
                                    var newPartFileName =
                                        Common.LeftString(Path.GetFileNameWithoutExtension(partFile), "_");
                                    if (oldPartFileName == newPartFileName && val[1] == sectionHeadId)
                                    {
                                        if (!_tocIdMapping.ContainsKey(idVal))
                                            _tocIdMapping.Add(idVal,
                                                              Path.GetFileName(partFile) + "#" + sectionHeadId);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void GetTocId(string tocFiletoUpdate)
        {
            XmlDocument xDoc = Common.DeclareXMLDocument(true);
            var namespaceManager = new XmlNamespaceManager(xDoc.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            xDoc.Load(tocFiletoUpdate);
            XmlNodeList anchorList = xDoc.GetElementsByTagName("a");
            if (anchorList.Count > 0)
            {
                foreach (XmlNode variable1 in anchorList)
                {
                    if (variable1.Attributes != null && variable1.Attributes.Count > 0 && variable1.Attributes["href"] != null)
                    {
                        _tocIDs.Add(variable1.Attributes["href"].Value);
                    }
                }
            }
        }

        private bool GetIncludeImageStatus(string cssFileName)
        {
            try
            {
                if (cssFileName.Trim().Length == 0)
                {
                    return true;
                }
                Param.LoadSettings();
                var xDoc = Common.DeclareXMLDocument(false);
                string path = Param.SettingOutputPath;
                xDoc.Load(path);
                var xPath = "//stylePick/styles/others/style[@file='" + cssFileName +
                            ".css']/styleProperty[@name='IncludeImage']/@value";
                var includeImageNode = xDoc.SelectSingleNode(xPath);
                if (includeImageNode != null && includeImageNode.InnerText == "No")
                    _isIncludeImage = false;
            }
            catch
            {
            }
            return _isIncludeImage;
        }

        private void RemoveNodeInXhtmlFile(string fileName)
        {
            //Removed NoteTargetReference tag from XHTML file
            XmlDocument xDoc = Common.DeclareXMLDocument(false);
            var namespaceManager = new XmlNamespaceManager(xDoc.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            xDoc.Load(fileName);
            XmlElement elmRoot = xDoc.DocumentElement;
            //If includeImage is false, removes the img -> parent tag
            if (_isIncludeImage == false)
            {
                string[] pictureClass = { "pictureCaption", "pictureColumn", "picturePage" };
                foreach (string clsName in pictureClass)
                {
                    string xPath = "//xhtml:div[@class='" + clsName + "']";
                    if (elmRoot != null)
                    {
                        XmlNodeList pictCaptionNode = elmRoot.SelectNodes(xPath, namespaceManager);
                        if (pictCaptionNode != null && pictCaptionNode.Count > 0)
                        {
                            for (int i = 0; i < pictCaptionNode.Count; i++)
                            {
                                var parentNode = pictCaptionNode[i].ParentNode;
                                if (parentNode != null)
                                    parentNode.RemoveChild(pictCaptionNode[i]);
                            }
                        }
                    }
                }

                if (elmRoot != null && _isIncludeImage == false)
                {
                    XmlNodeList imgNodes = elmRoot.GetElementsByTagName("img");
                    if (imgNodes.Count > 0)
                    {
                        int imgCount = imgNodes.Count;
                        for (int i = 0; i < imgCount; i++)
                        {
                            var parentNode = imgNodes[0].ParentNode;
                            if (parentNode != null)
                                parentNode.RemoveChild(imgNodes[0]);
                        }
                    }
                }
            }
            xDoc.Save(fileName);
        }

        #region Private Functions
        #region Handle After Before
        /// <summary>
        /// Inserting After & Before content to XHTML file
        /// </summary>
        private void InsertBeforeAfterInXhtml(PublicationInformation projInfo)
        {
            if (projInfo == null) return;
            if (projInfo.DefaultXhtmlFileWithPath == null || projInfo.DefaultCssFileWithPath == null) return;
            if (projInfo.DefaultXhtmlFileWithPath.Trim().Length == 0 || projInfo.DefaultCssFileWithPath.Trim().Length == 0) return;

			var cssTree = new CssTree();
			Dictionary<string, Dictionary<string, string>> cssClass = cssTree.CreateCssProperty(projInfo.DefaultCssFileWithPath, true);

			var afterBeforeProcess = new AfterBeforeProcessEpub();
			afterBeforeProcess.RemoveAfterBefore(projInfo, cssClass, cssTree.SpecificityClass, cssTree.CssClassOrder);
			PseudoClass = afterBeforeProcess._psuedoClassName;

			if (projInfo.IsReversalExist && projInfo.ProjectInputType.ToLower() == "dictionary")
            {
				cssClass = cssTree.CreateCssProperty(projInfo.DefaultRevCssFileWithPath, true);
				string originalDefaultXhtmlFileName = projInfo.DefaultXhtmlFileWithPath;
                projInfo.DefaultXhtmlFileWithPath = Common.PathCombine(Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath), "FlexRev.xhtml");
				RemovePseudoBefore(cssTree);
				var afterBeforeProcessReversal = new AfterBeforeProcessEpub();
				afterBeforeProcessReversal.RemoveAfterBefore(projInfo, cssClass, cssTree.SpecificityClass, cssTree.CssClassOrder);
				Common.StreamReplaceInFile(projInfo.DefaultXhtmlFileWithPath, "&nbsp;", Common.NonBreakingSpace);
                projInfo.DefaultXhtmlFileWithPath = originalDefaultXhtmlFileName;
            }
        }

        /// <summary>
        /// Remove After & Before in Css file
        /// </summary>
        private static void RemovePseudoBefore(CssTree cssTree)
        {
            if (cssTree.SpecificityClass.ContainsKey("subentry"))
            {
                ArrayList aa = cssTree.SpecificityClass["subentry"];
                foreach (ClassInfo myvar in aa)
                {
                    if (myvar.Pseudo.ToLower() == "before")
                        myvar.Content = "";
                }
            }
        }

        #endregion

        #region xslt processing
        /// <summary>
        /// Helper method that copies the epub xslt file into the temp directory, optionally inserts some
        /// processing commands, then returns the path to the modified file.
        /// </summary>
        /// <returns>Full path / filename of the xslt file</returns>
        private string GetXsltFile()
        {
            string xsltFullName = Common.FromRegistry("Xhtml-Epub.xslt");
            if (!File.Exists(xsltFullName))
                return "";
            var tempXslt = Common.PathCombine(Path.GetTempPath(), Path.GetFileName(xsltFullName));
            File.Copy(xsltFullName, tempXslt, true);
            xsltFullName = tempXslt;

            // Modify the local XSLT for the following conditions:
            // - Scriptures with a inline footnotes (References == "After Each Section"):
            //   adds the

            if (InputType.ToLower().Equals("scripture") && References.Contains("Section"))
            {
                // add references inline, after each section (first change)
                const string searchText = "<!-- Section div reference processing -->";
                var sbRef = new StringBuilder();
                sbRef.AppendLine(searchText);
                sbRef.AppendLine("<xsl:if test=\"@class = 'scrSection'\">");
                sbRef.Append("<xsl:if test=\"(count(descendant::xhtml:span[@class='Note_General_Paragraph']) +");
                sbRef.AppendLine(" count(descendant::xhtml:span[@class='Note_CrossHYPHENReference_Paragraph'])) > 0\">");
                sbRef.AppendLine("<xsl:element name=\"ul\">");
                sbRef.AppendLine("<xsl:attribute name=\"class\"><xsl:text>footnotes</xsl:text></xsl:attribute>");
                sbRef.AppendLine("<!-- general notes - use the note title for the list bullet -->");
                sbRef.AppendLine("<xsl:for-each select=\"descendant::xhtml:span[@class='Note_General_Paragraph']\">");
                sbRef.AppendLine("<xsl:element name=\"li\">");
                sbRef.AppendLine("<xsl:attribute name=\"id\"><xsl:text>FN_</xsl:text><xsl:value-of select=\"@id\"/></xsl:attribute>");
                sbRef.AppendLine("<xsl:element name=\"a\">");
                sbRef.AppendLine("<xsl:attribute name=\"href\"><xsl:text>#</xsl:text><xsl:value-of select=\"@id\"/></xsl:attribute>");
                sbRef.AppendLine("<xsl:text>[</xsl:text><xsl:value-of select=\"@title\"/><xsl:text>]</xsl:text>");
                sbRef.AppendLine("</xsl:element><xsl:text> </xsl:text><xsl:value-of select=\".\"/></xsl:element>");
                sbRef.AppendLine("</xsl:for-each>");
                sbRef.AppendLine("<!-- cross-references - use) the verse number for the list bullet -->");
                sbRef.AppendLine("<xsl:for-each select=\"descendant::xhtml:span[@class='Note_CrossHYPHENReference_Paragraph']\">");
                sbRef.AppendLine("<xsl:element name=\"li\">");
                sbRef.AppendLine("<xsl:attribute name=\"id\"><xsl:text>FN_</xsl:text><xsl:value-of select=\"@id\"/></xsl:attribute>");
                sbRef.AppendLine("<xsl:element name=\"a\">");
                sbRef.AppendLine("<xsl:attribute name=\"href\"><xsl:text>#</xsl:text><xsl:value-of select=\"@id\"/></xsl:attribute>");
                if (_isNoteTargetReferenceExists)
                {
                    sbRef.AppendLine("<xsl:value-of select=\"xhtml:span[@class='Note_Target_Reference']\"/>");
                    sbRef.AppendLine("</xsl:element><xsl:text> </xsl:text>");
					sbRef.AppendLine("<xsl:for-each select=\"xhtml:span[not(@class ='Note_Target_Reference')]\"><xsl:value-of select=\".\"/></xsl:for-each>");
                    sbRef.AppendLine("</xsl:element></xsl:for-each>");
                }
                else
                {
                    sbRef.AppendLine("<xsl:value-of select=\"preceding::xhtml:span[@class='Chapter_Number'][1]\"/><xsl:text>:</xsl:text>");
                    sbRef.AppendLine("<xsl:value-of select=\"preceding::xhtml:span[@class='Verse_Number'][1]\"/>");
                    sbRef.AppendLine("</xsl:element><xsl:text> </xsl:text><xsl:value-of select=\".\"/></xsl:element></xsl:for-each>");
                }

                sbRef.AppendLine("</xsl:element></xsl:if></xsl:if>");
                Common.StreamReplaceInFile(xsltFullName, searchText, sbRef.ToString());
                // add references inline (second change)
                sbRef.Length = 0;
                const string searchText2 = "<!-- secondary Section div reference processing -->";
                sbRef.AppendLine(searchText2);
                sbRef.Append("<xsl:if test=\"(count(descendant::xhtml:span[@class='Note_General_Paragraph']) + ");
                sbRef.AppendLine("count(descendant::xhtml:span[@class='Note_CrossHYPHENReference_Paragraph'])) > 0\">");
                sbRef.AppendLine("<xsl:element name=\"ul\">");
                sbRef.AppendLine("<xsl:attribute name=\"class\"><xsl:text>footnotes</xsl:text></xsl:attribute>");
                sbRef.AppendLine("<!-- general) notes - use the note title for the list bullet -->");
                sbRef.AppendLine("<xsl:for-each select=\"descendant::xhtml:span[@class='Note_General_Paragraph']\">");
                sbRef.AppendLine("<xsl:element name=\"li\">");
                sbRef.AppendLine("<xsl:attribute name=\"id\"><xsl:text>FN_</xsl:text><xsl:value-of select=\"@id\"/></xsl:attribute>");
                sbRef.AppendLine("<xsl:element name=\"a\">");
                sbRef.AppendLine("<xsl:attribute name=\"href\"><xsl:text>#</xsl:text><xsl:value-of select=\"@id\"/></xsl:attribute>");
                sbRef.AppendLine("<xsl:text>[</xsl:text><xsl:value-of select=\"@title\"/><xsl:text>]</xsl:text>");
                sbRef.AppendLine("</xsl:element><xsl:text> </xsl:text>");
				sbRef.AppendLine("<xsl:for-each select=\"xhtml:span\"><xsl:copy><xsl:for-each select=\"@*\"><xsl:copy/></xsl:for-each><xsl:apply-templates/></xsl:copy></xsl:for-each>");
				sbRef.AppendLine("</xsl:element></xsl:for-each>");
                sbRef.AppendLine("<!-- cross-references - use the verse number for the list bullet -->");
                sbRef.AppendLine("<xsl:for-each select=\"descendant::xhtml:span[@class='Note_CrossHYPHENReference_Paragraph']\">");
                sbRef.AppendLine("<xsl:element name=\"li\">");
                sbRef.AppendLine("<xsl:attribute name=\"id\"><xsl:text>FN_</xsl:text><xsl:value-of select=\"@id\"/></xsl:attribute>");
                sbRef.AppendLine("<xsl:element name=\"a\">");
                sbRef.AppendLine("<xsl:attribute name=\"href\"><xsl:text>#</xsl:text><xsl:value-of select=\"@id\"/></xsl:attribute>");
                if (_isNoteTargetReferenceExists)
                {
                    sbRef.AppendLine("<xsl:value-of select=\"xhtml:span[@class='Note_Target_Reference']\"/>");
                    sbRef.AppendLine("</xsl:element><xsl:text> </xsl:text>");
					sbRef.AppendLine("<xsl:for-each select=\"xhtml:span\"><xsl:copy><xsl:for-each select=\"@*\"><xsl:copy/></xsl:for-each><xsl:apply-templates/></xsl:copy></xsl:for-each>");
                    sbRef.AppendLine("</xsl:element>");
                }
                else
                {
                    sbRef.AppendLine("<xsl:value-of select=\"preceding::xhtml:span[@class='Chapter_Number'][1]\"/><xsl:text>:</xsl:text>");
                    sbRef.AppendLine("<xsl:value-of select=\"preceding::xhtml:span[@class='Verse_Number'][1]\"/>");
                    sbRef.AppendLine("</xsl:element><xsl:text> </xsl:text><xsl:value-of select=\".\"/></xsl:element>");
                }

                sbRef.AppendLine("</xsl:for-each></xsl:element></xsl:if>");
                Common.StreamReplaceInFile(xsltFullName, searchText2, sbRef.ToString());
            }
            return xsltFullName;
        }
        #endregion

        #region CSS processing
        /// <summary>
        /// Modifies the CSS based on the parameters from the Configuration Tool:
        /// - BaseFontSize
        /// - DefaultLineHeight
        /// - DefaultAlignment
        /// - ChapterNumbers
        /// </summary>
        /// <param name="cssFile"></param>
        private void CustomizeCss(string cssFile)
        {
            if (!File.Exists(cssFile)) return;
            // BaseFontSize and DefaultLineHeight - body element only
            var sb = new StringBuilder();
            sb.AppendLine("body {");
            sb.Append("font-size: ");
            sb.Append(BaseFontSize);
            sb.AppendLine("pt;");
            sb.Append("line-height: ");
            sb.Append(DefaultLineHeight);
            sb.AppendLine("%;");
            Common.StreamReplaceInFile(cssFile, "body {", sb.ToString());
            // ChapterNumbers - scripture only
            if (InputType.ToLower() == "scripture")
            {
                // ChapterNumbers (drop cap or in margin) - .Chapter_Number and .Paragraph1 class elements
                sb.Length = 0;  // reset the stringbuilder
                sb.AppendLine(".Chapter_Number {");
                sb.Append("font-size: ");
                if (ChapterNumbers == "Drop Cap")
                {
                    sb.AppendLine("250%;");
                    // vertical alignment of Cap specified by setting the padding-top to (defaultlineheight / 2)
                    sb.Append("padding-top: ");
                    sb.Append(BaseFontSize / 2);
                    sb.AppendLine("pt;");
                }
                else
                {
                    sb.AppendLine("24pt;");
                }
                Common.StreamReplaceInFile(cssFile, ".Chapter_Number {", sb.ToString());
            }
            // DefaultAlignment - several spots in the css file
            sb.Length = 0; // reset the stringbuilder
            sb.Append("text-align: ");
            sb.Append(DefaultAlignment.ToLower());
            sb.AppendLine(";");
            Common.StreamReplaceInFile(cssFile, "text-align:left;", sb.ToString());
        }
        #endregion

        #region string GetBookId(string xhtmlFileName)
        /// <summary>
        /// Returns a book ID to be used in the .opf file. This is similar to the GetBookName call, but here
        /// we're wanting something that (1) doesn't start with a numeric value and (2) is unique.
        /// </summary>
        /// <param name="xhtmlFileName"></param>
        /// <returns></returns>
        public string GetBookId(string xhtmlFileName)
        {
            try
            {
                XmlDocument xmlDocument = Common.DeclareXMLDocument(false);
                var namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
                namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
				var xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, DtdProcessing = DtdProcessing.Parse }; //Common.DeclareXmlReaderSettings(false);
                var xmlReader = XmlReader.Create(xhtmlFileName, xmlReaderSettings);
                xmlDocument.Load(xmlReader);
                xmlReader.Close();
                // should only be one of these after splitting out the chapters.
                XmlNodeList nodes;
                if (InputType.ToLower().Equals("dictionary"))
                {
                    nodes = xmlDocument.SelectNodes("//xhtml:div[@class='letter']", namespaceManager);
                }
                else
                {
                    // no scrBookName - use Title_Main
                    nodes = xmlDocument.SelectNodes("//xhtml:div[@class='Title_Main']", namespaceManager);
                    if (nodes == null || nodes.Count == 0)
                    {
                        // start out with the book code (e.g., 2CH for 2 Chronicles)
                        nodes = xmlDocument.SelectNodes("//xhtml:span[@class='scrBookCode']", namespaceManager);
                    }
                    if (nodes == null || nodes.Count == 0)
                    {
                        // no book code - use scrBookName
                        nodes = xmlDocument.SelectNodes("//xhtml:span[@class='scrBookName']", namespaceManager);
                    }
                    if (nodes == null || nodes.Count == 0)
                    {
                        // If no Title_Main, scrBookCode, scrBookName - use Chapter_Number(Landmarks)
                        nodes = xmlDocument.SelectNodes("//xhtml:span[@class='Chapter_Number']", namespaceManager);
                    }
                }
                if (nodes != null && nodes.Count > 0)
                {
                    var sb = new StringBuilder();
                    // just in case the name starts with a number, prepend "id"
                    sb.Append("id");
                    // remove any whitespace in the node text (the ID can't have it)
                    sb.Append("boooknode");
                    return (sb.ToString());
                }
                // fall back on just the file name
                return Path.GetFileName(xhtmlFileName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                if (ex.StackTrace != null)
                {
                    Debug.WriteLine(ex.StackTrace);
                }
                return Path.GetFileName(xhtmlFileName);
            }
        }
        #endregion

        #region Relative Hyperlink processing
        /// <summary>
        /// Returns the list of "broken" relative hyperlink hrefs in the given file (i.e.,
        /// relative hyperlinks that don't have a target within the file). This can happen when
        /// the xhtml file gets split out into multiple pieces, and the target for an href ends up
        /// in a different file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private IEnumerable<string> FindBrokenRelativeHrefIds(string filePath)
        {
            if (!File.Exists(filePath)) return null;
            const string searchText = "a href=\"#"; // denotes a relative href
            var brokenRelativeHrefIds = new List<string>();
            var reader = new StreamReader(filePath);
            var content = reader.ReadToEnd();
            reader.Close();
            int start = content.IndexOf(searchText, 0, StringComparison.Ordinal);
            if (start != -1)
            {
                start += searchText.Length;
            }
            while (start != -1)
            {
                // next instance of a relative hyperlink ref - read until the closing quote
                int stop = (content.IndexOf("\"", start, StringComparison.Ordinal) - start);
                if (stop == -1) { break; }
                var hrefId = content.Substring(start, (stop));
                // not found -- this link is broken
                if (!brokenRelativeHrefIds.Contains(hrefId))
                {
                    brokenRelativeHrefIds.Add(hrefId);
                }
                start = content.IndexOf(searchText, (start + stop), StringComparison.Ordinal);
                if (start != -1)
                {
                    start += searchText.Length;
                }
            }
            return brokenRelativeHrefIds;
        }

        private void FixRelativeHyperlinks(string contentFolder)
        {
            string[] files = Directory.GetFiles(contentFolder, "PartFile*.xhtml");
            string[] revFiles = Directory.GetFiles(contentFolder, "RevIndex*.xhtml");
            //inProcess.AddToMaximum(files.Length);
            var preExport = new PreExportProcess();
            var dictHyperlinks = new Dictionary<string, string>();
            var sourceList = new List<string>();
            var targetList = new List<string>();
            var targettempList = new List<string>();
            var fileDict = new Dictionary<string, string>();

            foreach (string targetFile in files)
            {
                preExport.GetReferenceList(targetFile, sourceList, targettempList);

                targetList.AddRange(targettempList);
                foreach (string target in targettempList)
                {
                    fileDict[target] = Path.GetFileName(targetFile);
                }
                targettempList.Clear();
            }

            foreach (string target in targetList)
            {
                if (sourceList.Contains(target) && !dictHyperlinks.ContainsKey(target))
                {
                    dictHyperlinks.Add(target, fileDict[target] + "#" + target);
                }
            }

            if (dictHyperlinks.Count > 0)
            {
                foreach (string targetFile in files)
                {
                    RemoveSpanVerseNumberNodeInXhtmlFile(targetFile);
                    ReplaceAllBrokenHrefs(targetFile, dictHyperlinks);
                }
                foreach (string targetFile in revFiles)
                {
                    RemoveSpanVerseNumberNodeInXhtmlFile(targetFile);
                    ReplaceAllBrokenHrefs(targetFile, dictHyperlinks);
                }
            }
            else
            {
                if (files.Length > 0)
                {
                    foreach (string targetFile in files)
                    {
                        RemoveSpanVerseNumberNodeInXhtmlFile(targetFile);
                    }
                }
                if (revFiles.Length > 0)
                {
                    foreach (string targetFile in revFiles)
                    {
                        RemoveSpanVerseNumberNodeInXhtmlFile(targetFile);
                    }
                }
            }
        }

        private void RemoveSpanVerseNumberNodeInXhtmlFile(string fileName)
        {
            //Removed NoteTargetReference tag from XHTML file
            XmlDocument xDoc = Common.DeclareXMLDocument(true);
            var namespaceManager = new XmlNamespaceManager(xDoc.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            xDoc.Load(fileName);
            var elmRoot = xDoc.DocumentElement;

            //If includeImage is false, removes the img -> parent tag

            const string xPath = "//xhtml:div[@class='scrBook']/xhtml:span[@class='Verse_Number']";
            if (elmRoot != null)
            {
                XmlNodeList divNode = elmRoot.SelectNodes(xPath, namespaceManager);
                if (divNode != null && divNode.Count > 0)
                {
                    for (int i = 0; i < divNode.Count; i++)
                    {
                        var parentNode = divNode[i].ParentNode;
                        if (parentNode != null)
                            parentNode.RemoveChild(divNode[i]);
                    }
                }
            }

            xDoc.Save(fileName);
        }

        private void ReplaceAllBrokenHrefs(string filePath, Dictionary<string, string> dictHyperlinks)
        {
            if (!File.Exists(filePath)) return;
            var reader = new StreamReader(filePath);
            string content = reader.ReadToEnd();
            reader.Close();
            var contentWriter = new StringBuilder();
            const string searchText = "a href=\"#";
            int startIndex = 0;
            bool done = false;
            while (!done)
            {
                int nextIndex = content.IndexOf(searchText, startIndex, StringComparison.Ordinal);
                if (nextIndex >= 0)
                {
                    // find the href target
                    int stopIndex = content.IndexOf("\"", nextIndex + searchText.Length, StringComparison.Ordinal);
                    var target = content.Substring(nextIndex + searchText.Length, stopIndex - (nextIndex + searchText.Length));
                    // is it in our dictionary?
                    string newValue;
                    if (dictHyperlinks.TryGetValue(target, out newValue))
                    {
                        // yes - write the corrected text to the output file
                        contentWriter.Append(content.Substring(startIndex, nextIndex - startIndex));
                        contentWriter.Append("a href=\"");
                        contentWriter.Append(newValue);
                    }
                    else
                    {
                        // no - write out the existing text to the output file
                        contentWriter.Append(content.Substring(startIndex, stopIndex - startIndex));
                    }
                    // update startIndex
                    startIndex = stopIndex;
                }
                else
                {
                    // no more relative hyperlinks
                    contentWriter.Append(content.Substring(startIndex, content.Length - startIndex));
                    done = true;
                }
            }
            var writer = new StreamWriter(filePath);
            writer.Write(contentWriter);
            writer.Close();
        }
        #endregion

        #region Image processing
        /// <summary>
        /// This method handles the images for the .epub file. Each image is resized and renamed (to .png) if necessary, then
        /// copied to the .epub folder. Any references to the image files from the .xhtml are also updated if needed.
        /// </summary>
        /// <param name="tempFolder"></param>
        /// <param name="contentFolder"></param>
        private void ProcessImages(string tempFolder, string contentFolder)
        {
            string[] imageFiles = Directory.GetFiles(tempFolder);
            bool renamedImages = false;
            foreach (string file in imageFiles)
            {
                Image image;
                // ReSharper disable PossibleNullReferenceException
                switch (Path.GetExtension(file) == null ? "" : Path.GetExtension(file).ToLower())
                // ReSharper restore PossibleNullReferenceException
                {
                    case ".jpg":
                    case ".jpeg":
                    case ".gif":
                    case ".png":
                        // .epub supports this image format - just copy the thing over
                        string name = Path.GetFileName(file);
                        string dest = Common.PathCombine(contentFolder, name);
                        // sanity check - if the image is gigantic, scale it
                        image = Image.FromFile(file);
                        if (image.Width > MaxImageWidth)
                        {
                            // need to scale image
                            var img = ResizeImage(image);
                            var extension = Path.GetExtension(file);
                            if (extension != null)
                                switch (extension.ToLower())
                                {
                                    case ".jpg":
                                    case ".jpeg":
                                        img.Save(dest, System.Drawing.Imaging.ImageFormat.Jpeg);
                                        break;
                                    case ".gif":
                                        img.Save(dest, System.Drawing.Imaging.ImageFormat.Gif);
                                        break;
                                    default:
                                        img.Save(dest, System.Drawing.Imaging.ImageFormat.Png);
                                        break;
                                }
                        }
                        else
                        {
                            File.Copy(file, dest);
                        }
                        break;
                    case ".bmp":
                    case ".tif":
                    case ".tiff":
                    case ".ico":
                    case ".wmf":
                    case ".pcx":
                    case ".cgm":
                        // TE (and others?) support these file types, but .epub doesn't -
                        // convert them to .png if we can
                        var imageName = Path.GetFileNameWithoutExtension(file) + ".png";
                        using (var fileStream = new FileStream(Common.PathCombine(contentFolder, imageName), FileMode.CreateNew))
                        {
                            image = Image.FromFile(file);
                            if (image.Width > MaxImageWidth)
                            {
                                var img = ResizeImage(image);
                                img.Save(fileStream, System.Drawing.Imaging.ImageFormat.Png);
                            }
                            else
                            {
                                image.Save(fileStream, System.Drawing.Imaging.ImageFormat.Png);
                            }
                        }
                        renamedImages = true;
                        break;
                }
            }
            // be sure to clean up any hyperlink references to the old file types
            if (renamedImages)
            {
                CleanupImageReferences(contentFolder);
            }
        }

        /// <summary>
        /// Resizes the given image down to MaxImageWidth pixels and returns the result.
        /// </summary>
        /// <param name="image">File to resize</param>
        private Image ResizeImage(Image image)
        {
            if (MaxImageWidth < 100)
                MaxImageWidth = 100; //Set Minimum value

            float nPercent = ((float)MaxImageWidth / (float)image.Width);
            var destW = (int)(image.Width * nPercent);
            var destH = (int)(image.Height * nPercent);
            var b = new Bitmap(destW, destH);
            var g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(image, 0, 0, destW, destH);
            //g.Dispose();
            return (Image)b;
        }

        /// <summary>
        /// The .epub format doesn't support all image file types; when we copied the image files over, we had
        /// to convert the unsupported file types to .png. Here we'll do a search/replace for all references to
        /// the old versions.
        /// </summary>
        /// <param name="contentFolder">OEBPS folder containing all the xhtml files we need to clean up</param>
        private void CleanupImageReferences(string contentFolder)
        {
            string[] files = Directory.GetFiles(contentFolder, "*.xhtml");
            foreach (string file in files)
            {
                // using a streaming approach to reduce the memory footprint of this method
                // (we had Regex.Replace before, but it was using >100MB of data on larger dictionaries)
                var reader = new StreamReader(file);
                var writer = new StreamWriter(file + ".tmp");
                Int32 next;
                while ((next = reader.Read()) != -1)
                {
                    var b = (char)next;
                    if (b == '.') // found a period - is it a filename extension that we need to change?
                    {
                        // copy the period and the next 3 characters into a string
                        const int len = 4;
                        var buf = new char[len];
                        buf[0] = b;
                        reader.Read(buf, 1, 3);
                        var data = new string(buf);
                        // is this an unsupported filename extension?
                        switch (data)
                        {
                            case ".bmp":
                            case ".ico":
                            case ".wmf":
                            case ".pcx":
                            case ".cgm":
                                // yes - replace with ".png"
                                writer.Write(".png");
                                break;
                            case ".tif":
                                // yes, but this could be either ".tif" or ".tiff" -
                                // find out which one by peeking at the next character
                                int nextchar = reader.Peek();
                                if (((char)nextchar) == 'f')
                                {
                                    // ".tiff" case
                                    reader.Read(); // move the reader up one position (consume the "f")
                                    // replace with ".png"
                                    writer.Write(".png");
                                }
                                else
                                {
                                    // ".tif" case - replace it with ".png"
                                    writer.Write(".png");
                                }
                                break;
                            default:
                                // not an unsupported extension - just write the data we collected
                                writer.Write(data);
                                break;
                        }
                    }
                    else // not a "."
                    {
                        writer.Write((char)next);
                    }
                }
                reader.Close();
                writer.Close();
                // replace the original file with the new one
                File.Delete(file);
                File.Move((file + ".tmp"), file);
            }
        }
        #endregion

        #region File Processing Methods
        /// <summary>
        /// Returns true if the specified search text string is found in the given file.
        /// Modified to use the Knuth-Morris-Pratt search algorithm
        /// (http://en.wikipedia.org/wiki/Knuth%E2%80%93Morris%E2%80%93Pratt_algorithm)
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        private bool IsStringInFile(string filePath, string searchText)
        {

            try
            {
                XmlTextReader reader = Common.DeclareXmlTextReader(filePath, true);
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        string idString = searchText.Replace("id=\"", "").Replace("\"", "");
                        if (reader.Name == "div" || reader.Name == "span" || reader.Name=="a")
                        {

                            string id = reader.GetAttribute("id");
                            if (id == idString)
                            {
                                reader.Close();
                                return true;
                            }
                        }
                    }
                }
                reader.Close();
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Helper method to change the relative hyperlinks in the references file to absolute ones.
        /// This is done after the scripture files are split out into individual books of 100K or less in size.
        /// </summary>
        /// <param name="contentFolder"></param>
        /// <param name="inProcess"></param>
        private void UpdateReferenceHyperlinks(string contentFolder, InProcess inProcess)
        {
            var outFilename = Common.PathCombine(contentFolder, ReferencesFilename);
            var hrefs = FindBrokenRelativeHrefIds(outFilename);
            //inProcess.AddToMaximum(hrefs.Count + 1);
            var reader = new StreamReader(outFilename);
            var content = new StringBuilder();
            content.Append(reader.ReadToEnd());
            reader.Close();
            string[] files = Directory.GetFiles(contentFolder, "PartFile*.xhtml");
            int index = 0;
            bool looped = false;
            foreach (var href in hrefs)
            {
				if (string.IsNullOrEmpty(href))
					continue; // A glossary term with no glossary entry
                // find where the target is for this reference -
                // since the lists are sequential in the references file, we're using an index instead
                // of a foreach loop (so the search continues in the same file the last href left off on).
                while (true)
                {
                    // search the current file in the list
                    if (IsStringInFile(files[index], href))
                    {
                        content.Replace(("a href=\"#" + href + "\""),
                                        ("a href=\"" + Path.GetFileName(files[index]) + "#" + href + "\""));
						index = 0;
                        break;
                    }
                    // update the index and try again
                    index++;
                    if (index == files.Length)
                    {
						index = 0;
						looped = true;
                        if (looped)
							break; // already searched through the list -- this item isn't found, get out
                    }
                }
                inProcess.PerformStep();
                looped = false;
            }
            var writer = new StreamWriter(outFilename);
            writer.Write(content);
            writer.Close();
            inProcess.PerformStep();
        }

        /// <summary>
        /// Helper method to change the relative hyperlinks in the references file to absolute ones.
        /// This is done after the scripture files are split out into individual books of 100K or less in size.
        /// </summary>
        /// <param name="contentFolder"></param>
        /// <param name="inProcess"></param>
        private void UpdateReferenceSourcelinks(string contentFolder, InProcess inProcess)
        {
            string[] files = Directory.GetFiles(contentFolder, "PartFile*.xhtml");
            foreach (var file in files)
            {
                XmlDocument xmlDocument = Common.DeclareXMLDocument(false);
                var namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
                namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
				var xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, DtdProcessing = DtdProcessing.Parse };
                var xmlReader = XmlReader.Create(file, xmlReaderSettings);
                xmlDocument.Load(xmlReader);
                xmlReader.Close();
                XmlNodeList footnoteNodes = null;
                footnoteNodes = xmlDocument.SelectNodes("//span[@class='Note_General_Paragraph']/a");

                if (footnoteNodes == null || footnoteNodes.Count == 0)
                {
                    footnoteNodes = xmlDocument.SelectNodes("//xhtml:span[@class='Note_General_Paragraph']/xhtml:a", namespaceManager);
                }

                if (footnoteNodes != null)
                {
                    foreach (XmlNode footnoteNode in footnoteNodes)
                    {
                        if (footnoteNode.Attributes != null)
                            footnoteNode.Attributes["href"].Value = "zzReferences.xhtml" + footnoteNode.Attributes["href"].Value;
                    }
                }

                footnoteNodes = xmlDocument.SelectNodes("//span[@class='Note_CrossHYPHENReference_Paragraph']/a");

                if (footnoteNodes == null || footnoteNodes.Count == 0)
                {
                    footnoteNodes = xmlDocument.SelectNodes("//xhtml:span[@class='Note_CrossHYPHENReference_Paragraph']/xhtml:a", namespaceManager);
                    if (footnoteNodes == null)
                        return;
                }

                foreach (XmlNode footnoteNode in footnoteNodes)
                {
                    if (footnoteNode.Attributes != null)
                        footnoteNode.Attributes["href"].Value = "zzReferences.xhtml" + footnoteNode.Attributes["href"].Value;
                }

                xmlDocument.Save(file);
                inProcess.PerformStep();
            }
        }

		/// <summary>
		/// Creates a separate references file at the end of the xhtml files in scripture content, for both footnotes and cross-references.
		/// Each reference links back relatively to the source xhtml, so that the links can be updated when the content is split into
		/// smaller chunks.
		/// </summary>
		/// <param name="outputFolder"></param>
		/// <param name="xhtmlFileName"></param>
		private void CreateReferencesFile(string outputFolder, string xhtmlFileName)
        {
            // sanity check - return if the references are to be left in the text
            if (References.Contains("Section")) { return; }
            // collect all cross-references and footnotes in the content file
            XmlDocument xmlDocument = Common.DeclareXMLDocument(false);
            var namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
			var xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, DtdProcessing = DtdProcessing.Parse }; //Common.DeclareXmlReaderSettings(false);
            var xmlReader = XmlReader.Create(xhtmlFileName, xmlReaderSettings);
            xmlDocument.Load(xmlReader);
            xmlReader.Close();
            // pick your nodes
            var crossRefNodes = xmlDocument.SelectNodes("//xhtml:span[@class='Note_CrossHYPHENReference_Paragraph']", namespaceManager);
            var footnoteNodes = xmlDocument.SelectNodes("//xhtml:span[@class='Note_General_Paragraph']", namespaceManager);
            if (crossRefNodes == null && footnoteNodes == null)
            {
                // nothing to pull out -- just exit
                return;
            }
            // file preamble
            var sbPreamble = new StringBuilder();
            sbPreamble.Append("<?xml version='1.0' encoding='utf-8'?><!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.1//EN' 'http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd'>");
            sbPreamble.Append("<html xmlns='http://www.w3.org/1999/xhtml'><head><title>");
            sbPreamble.AppendLine("</title><link rel='stylesheet' href='book.css' type='text/css' /></head>");
            sbPreamble.Append("<body class='scrBody'><div class='Front_Matter'>");
            var outFilename = Common.PathCombine(outputFolder, ReferencesFilename);
            var outFile = new StreamWriter(outFilename);
            outFile.WriteLine(sbPreamble.ToString());
            // iterate through the files and pull out each reference hyperlink
            if (footnoteNodes != null && footnoteNodes.Count > 0)
            {
                outFile.WriteLine("<h1>Endnotes</h1>");
                outFile.WriteLine("<ul>");
                foreach (XmlNode footnoteNode in footnoteNodes)
                {
                    outFile.Write("<li id=\"FN_");
                    WriteAttributeValue(outFile, footnoteNode, "id");
                    outFile.Write("\"><a href=\"");
                    outFile.Write("#");
                    WriteAttributeValue(outFile, footnoteNode, "id");
                    outFile.Write("\">[");
                    WriteAttributeValue(outFile, footnoteNode, "title");
                    outFile.Write("] ");
                    outFile.Write("</a> ");
                    XmlNode bookNode = footnoteNode.SelectSingleNode("preceding::xhtml:span[@class='scrBookName'][1]", namespaceManager);
                    if (bookNode != null)
                    {
                        outFile.Write("<span class=\"BookName\"><b>" + bookNode.InnerText + "</b></span>");
                    }
                    outFile.Write(" ");
                    outFile.Write(CleanupSpans(footnoteNode.InnerXml));
                    outFile.WriteLine("</li>");
                }
                outFile.WriteLine("</ul>");
            }
            if (crossRefNodes != null && crossRefNodes.Count > 0)
            {
                outFile.WriteLine("<h1>References</h1>");
                outFile.WriteLine("<ul>");
                foreach (XmlNode crossRefNode in crossRefNodes)
                {
                    outFile.Write("<li id=\"FN_");
                    WriteAttributeValue(outFile, crossRefNode, "id");
                    outFile.Write("\"><a href=\"");
                    outFile.Write("#");
                    WriteAttributeValue(outFile, crossRefNode, "id");
                    outFile.Write("\">");
                    XmlNode bookNode = crossRefNode.SelectSingleNode("preceding::xhtml:span[@class='Title_Main'][1]", namespaceManager);
                    if (bookNode != null)
                    {
                        outFile.Write(bookNode.InnerText);
                    }
                    outFile.Write(" ");
                    XmlNode chapterNode = crossRefNode.SelectSingleNode("preceding::xhtml:span[@class='Chapter_Number'][1]", namespaceManager);
                    if (chapterNode != null)
                    {
                        outFile.Write(chapterNode.InnerText);
                    }
                    outFile.Write(":");
                    XmlNode verseNode = crossRefNode.SelectSingleNode("preceding::xhtml:span[@class='Verse_Number'][1]", namespaceManager);
                    if (verseNode != null)
                    {
                        outFile.Write(verseNode.InnerText);
                    }
                    outFile.Write("</a> ");
                    XmlNode refText = crossRefNode.SelectSingleNode("xhtml:span[not (@class)]",namespaceManager);
                    if (refText != null)
                    {
                        outFile.Write(CleanupSpans(refText.OuterXml));
                    }
                    else
                    {
                        outFile.Write(CleanupSpans(crossRefNode.InnerXml));
                    }
                    outFile.WriteLine("</li>");
                }
                outFile.WriteLine("</ul>");
            }

            outFile.WriteLine("</div></body></html>");
            outFile.Flush();
            outFile.Close();
        }

        private static void WriteAttributeValue(StreamWriter outFile, XmlNode footnoteNode, string attributeName)
        {
            Debug.Assert(outFile != null);
            if (footnoteNode == null || footnoteNode.Attributes == null)
            {
                Debug.WriteLine("No Footnote atribute {0}", attributeName);
            }
            else
            {
                try
                {
                    outFile.Write(footnoteNode.Attributes[attributeName].Value);
                }
                catch (NullReferenceException e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }

        private string CleanupSpans(string text)
        {
            var sb = new StringBuilder(text);
            sb.Replace(" lang", " xml:lang");
            sb.Replace("xmlns=\"http://www.w3.org/1999/xhtml\"", "");
            return sb.ToString();
        }

        /// <summary>
        /// Splits the specified xhtml file out into multiple files, either based on letter (dictionary) or book (scripture).
        /// This method was adapted from ExportOpenOffice.cs.
        /// </summary>
        /// <param name="temporaryCvFullName"></param>
        /// <param name="pubInfo"></param>
        /// <returns></returns>
        private IEnumerable<string> SplitFile(string temporaryCvFullName, PublicationInformation pubInfo)
        {
            List<string> fileNameWithPath;
            if (InputType.ToLower().Equals("dictionary"))
            {
                fileNameWithPath = Common.SplitXhtmlFile(temporaryCvFullName, "letHead", true);
            }
            else
            {
                fileNameWithPath = Common.SplitXhtmlFile(temporaryCvFullName, "scrBook", false);
            }
            return fileNameWithPath;
        }

        /// <summary>
        /// Splits a book file into smaller files, based on file size.
        /// </summary>
        /// <param name="xhtmlFilename">file to split into smaller pieces</param>
        /// <returns></returns>
        private List<string> SplitBook(string xhtmlFilename)
        {
            const long maxSize = 204800; // 200KB
            // sanity check - make sure the file exists
            if (!File.Exists(xhtmlFilename))
            {
                return null;
            }
            var fileNames = new List<string>();
            // is it worth splitting this file?
            var fi = new FileInfo(xhtmlFilename);
            if (fi.Length <= maxSize)
            {
                // not worth splitting this file - just return it
                fileNames.Add(xhtmlFilename);
                return fileNames;
            }

            // If we got here, it's worth our time to split the file out.
            var reader = new StreamReader(xhtmlFilename);
            string content = reader.ReadToEnd();
            reader.Close();

            string bookcode = "<span class=\"scrBookCode\">" + GetBookId(xhtmlFilename) + "</span>";
            string head = content.Substring(0, content.IndexOf("<body", StringComparison.Ordinal));
            var startIndex = 0;
            var fileIndex = 1;
            var sb = new StringBuilder();
            while (true)
            {
                // look for a good breaking point after our soft maximum size
                var outFile = Common.PathCombine(Path.GetDirectoryName(xhtmlFilename), (Path.GetFileNameWithoutExtension(xhtmlFilename) + fileIndex.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') + ".xhtml"));
                int softMax = startIndex + (int)(maxSize / 2);
                int realMax;
                if (softMax > content.Length)
                {
                    realMax = -1;
                }
                else
                {
                    var divClass = InputType.ToLower() == "scripture" ? "Section_Head" : "entry";
                    var target = string.Format("<div class=\"{0}", divClass);
                    realMax = content.IndexOf(target, softMax, StringComparison.Ordinal);
                }
                StreamWriter writer;
                if (realMax == -1)
                {
                    if (startIndex == 0)
                    {
                        // can't split this file (no section breaks after the soft limit) - just return it
                        fileNames.Add(xhtmlFilename);
                        return fileNames;
                    }
                    // no more section heads - just pull in the rest of the content
                    // write out head + substring(startIndex to the end)
                    sb.Append(head);
                    if (InputType.ToLower() == "scripture")
                    {
                        sb.Append("<body class=\"scrBody\"><div class=\"scrBook\">");
                        sb.Append(bookcode);
                    }
                    else
                    {
                        sb.Append("<body class=\"dicBody\"><div class=\"letData\">");
                    }

                    sb.AppendLine(content.Substring(startIndex));
                    writer = new StreamWriter(outFile);
                    writer.Write(sb.ToString());
                    writer.Close();
                    // add this file to fileNames)))
                    fileNames.Add(outFile);
                    break;
                }
                // build the content
                if (startIndex == 0)
                {
                    // for the first section, we go from the start of the file to realMax
                    sb.Append(content.Substring(0, (realMax - startIndex)));
                    sb.AppendLine("</div></body></html>"); // close out the xhtml
                }
                else
                {
                    // for the subsequent sections, we need the head + the substring (startIndex to realMax)
                    sb.Append(head);
                    var bodyClass = InputType.ToLower() == "scripture" ? "scrBody" : "dicBody";
                    var divClass = InputType.ToLower() == "scripture" ? "scrBook" : "letData";
                    sb.Append(string.Format("<body class=\"{0}\"><div class=\"{1}\">", bodyClass, divClass));
                    sb.Append(content.Substring(startIndex, (realMax - startIndex)));
                    sb.AppendLine("</div></body></html>"); // close out the xhtml
                }
                // write the string buffer content out to file
                writer = new StreamWriter(outFile);
                writer.Write(sb.ToString());
                writer.Close();
                // add this file to fileNames
                fileNames.Add(outFile);
                // move the indices up for the next file chunk
                startIndex = realMax;
                // reset the stringbuilder
                sb.Length = 0;
                fileIndex++;
            }
            // return the result
            return fileNames;
        }

        /// <summary>
        /// Copies the selected source folder and its subdirectories to the destination folder path.
        /// This is a recursive method.
        /// </summary>
        /// <param name="sourceFolder"></param>
        /// <param name="destFolder"></param>
        private void CopyFolder(string sourceFolder, string destFolder)
        {
            if (Directory.Exists(destFolder))
            {
                var di = new DirectoryInfo(destFolder);
                Common.CleanDirectory(di);
            }
            Directory.CreateDirectory(destFolder);
            string[] files = Directory.GetFiles(sourceFolder);
            try
            {
                foreach (string file in files)
                {
                    string name = Path.GetFileName(file);
                    string dest = Common.PathCombine(destFolder, name);
                    // Special processing for the mimetype file - don't copy it now; copy it after
                    // compressing the rest of the archive (in Compress() below) as a stored / not compressed
                    // file in the archive. This is keeping in line with the .epub OEBPS Container Format (OCF)
                    // recommendations: http://www.idpf.org/ocf/ocf1.0/download/ocf10.htm.
                    if (name != "mimetype")
                    {
                        File.Copy(file, dest);
                    }
                }

                string[] folders = Directory.GetDirectories(sourceFolder);
                foreach (string folder in folders)
                {
                    string name = Path.GetFileName(folder);
                    string dest = Common.PathCombine(destFolder, name);
                    if (name != ".svn")
                    {
                        CopyFolder(folder, dest);
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Compresses the selected folder's contents and saves the archive in the specified outputPath
        /// with the extension .epub.
        /// </summary>
        /// <param name="sourceFolder">Folder to compress</param>
        /// <param name="outputPath">Output path and filename (without extension)</param>
        public void Compress(string sourceFolder, string outputPath)
        {
            var mOdt = new ZipFolder();
            string outputPathWithFileName = outputPath + ".epub";

            // add the content to the existing epub.zip file
            string zipFile = Common.PathCombine(sourceFolder, "epub.zip");
            string contentFolder = Common.PathCombine(sourceFolder, "OEBPS");
            if (Directory.Exists(contentFolder))
            {
                string[] files = Directory.GetFiles(contentFolder);
                mOdt.AddToZip(files, zipFile);
	            var avFolder = Common.PathCombine(contentFolder, "AudioVisual");
	            if (Directory.Exists(avFolder))
	            {
		            var avFiles = Directory.GetFiles(avFolder);
					mOdt.AddToZip(avFiles, zipFile);
	            }
                var sb = new StringBuilder();
                sb.Append(sourceFolder);
                sb.Append(Path.DirectorySeparatorChar);
                sb.Append("META-INF");
                sb.Append(Path.DirectorySeparatorChar);
                sb.Append("container.xml");
                var containerFile = new[] { sb.ToString() };
                mOdt.AddToZip(containerFile, zipFile);
                // copy the results to the output directory
                File.Copy(zipFile, outputPathWithFileName, true);
            }
        }

        #endregion

        private void AddDtdInXhtml(string contentFolder)
        {
            string[] files = Directory.GetFiles(contentFolder, "*.xhtml");
            foreach (string file in files)
            {
                Common.AddingDTDForLinuxProcess(file);
            }
        }

        #region EPUB metadata handlers

        protected void InsertChapterLinkBelowBookName(string contentFolder)
        {
            string[] files = Directory.GetFiles(contentFolder, "PartFile*.xhtml");
            var chapterIdList = new List<string>();
            string fileName;
            var xmlDocument = Common.DeclareXMLDocument(true);
            var namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
			var xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, DtdProcessing = DtdProcessing.Parse };//Common.DeclareXmlReaderSettings(false);
            foreach (string sourceFile in files)
            {
                if (!File.Exists(sourceFile)) return;
                fileName = Path.GetFileName(sourceFile);
                XmlReader xmlReader = XmlReader.Create(sourceFile, xmlReaderSettings);
                xmlDocument.Load(xmlReader);
                xmlReader.Close();

                if (InputType.ToLower().Equals("scripture"))
                {
                    XmlNodeList nodes = xmlDocument.SelectNodes(".//xhtml:div[@class='Chapter_Number']", namespaceManager);
                    if (nodes != null)
                        foreach (XmlNode chapterNode in nodes)
                        {
                            if (chapterNode.Attributes != null && (chapterNode.Attributes.Count > 0 && chapterNode.Attributes["id"] != null))
                            {
                                string value = fileName + "#" + chapterNode.Attributes["id"].Value;
                                if (!chapterIdList.Contains(value))
                                    chapterIdList.Add(value);
                            }
                        }
                }
            }
            foreach (string sourceFile in files)
            {
                fileName = Path.GetFileNameWithoutExtension(sourceFile);
                if (fileName != null && (fileName.LastIndexOf("_01", StringComparison.Ordinal) == fileName.Length - 3 || fileName.LastIndexOf("_", StringComparison.Ordinal) == fileName.Length - 1))
                {
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sourceFile);
                    if (fileNameWithoutExtension != null)
                    {
                        string[] valueList1 = fileNameWithoutExtension.Split('_');
                        if (OnlyOneChapter(chapterIdList, valueList1[0]))
                            continue;
                        XmlReader xmlReader = XmlReader.Create(sourceFile, xmlReaderSettings);
                        xmlDocument.Load(xmlReader);
                        xmlReader.Close();
                        string xPath = ".//xhtml:div[@class='Title_Secondary']";
                        XmlNodeList nodes = xmlDocument.SelectNodes(xPath, namespaceManager);
                        Debug.Assert(nodes != null);

                        if (nodes.Count == 0)
                        {
                            xPath = ".//xhtml:div[@class='Title_Main']";
                            nodes = xmlDocument.SelectNodes(xPath, namespaceManager);
                            Debug.Assert(nodes != null);
                        }

                        if (nodes.Count > 0)
                        {
                            var next = nodes[nodes.Count - 1].NextSibling;
                            if (next != null && next.Attributes != null)
                            {
                                while (next != null && (next.Attributes != null && next.Attributes.GetNamedItem("class").InnerText.ToLower().Contains("title")))
                                    next = next.NextSibling;
                            }
                            var book = string.Empty;
                            foreach (string variable in chapterIdList)
                            {
                                string[] valueList = variable.Split('_');
                                if (valueList[0] != valueList1[0] && valueList[2] != book)
                                    continue;
                                book = valueList[2];

                                Debug.Assert(xmlDocument.DocumentElement != null);
                                XmlNode nodeContent = xmlDocument.CreateElement("a", xmlDocument.DocumentElement.NamespaceURI);
                                Debug.Assert(nodeContent != null && nodeContent.Attributes != null);
                                XmlAttribute attribute = xmlDocument.CreateAttribute("href");
                                attribute.Value = variable;
                                nodeContent.Attributes.Append(attribute);
                                XmlAttribute attribute2 = xmlDocument.CreateAttribute("class");
                                attribute2.Value = "chapternumberlink";
                                nodeContent.Attributes.Append(attribute2);

                                nodeContent.InnerText = GetChapterNumber(variable);
                                Debug.Assert(next != null && next.ParentNode != null);
                                next.ParentNode.InsertBefore(nodeContent, next);
                                Debug.Assert(xmlDocument.DocumentElement != null);
                                XmlNode spaceNode = xmlDocument.CreateElement("span", xmlDocument.DocumentElement.NamespaceURI);
                                spaceNode.InnerText = " ";
                                next.ParentNode.InsertBefore(spaceNode, next);
                            }
                        }
                    }
                    xmlDocument.Save(sourceFile);
                }
            }
        }

        protected void InsertReferenceLinkInTocFile(string contentFolder)
        {
            string referenceFileName = Common.PathCombine(contentFolder, "zzReferences.xhtml");
            if (!File.Exists(referenceFileName))
                return;
            string[] files = Directory.GetFiles(contentFolder, "File3TOC*.xhtml");
            var xmlDocument = Common.DeclareXMLDocument(true);
            var namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
			var xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, DtdProcessing = DtdProcessing.Parse };

            foreach (string sourceFile in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(sourceFile);
                if (fileName != null)
                {
                    XmlReader xmlReader = XmlReader.Create(sourceFile, xmlReaderSettings);
                    xmlDocument.Load(xmlReader);
                    xmlReader.Close();
                    const string xPath = ".//xhtml:div[@class='Contents'][1]/xhtml:ul";
                    XmlNodeList nodes = xmlDocument.SelectNodes(xPath, namespaceManager);
                    Debug.Assert(nodes != null);

                    if (nodes.Count > 0)
                    {
                        var next = nodes[nodes.Count - 1].LastChild;
                        if (next != null && next.Attributes != null)
                        {
// ReSharper disable PossibleNullReferenceException
                            XmlNode spaceNode = xmlDocument.CreateElement("li", xmlDocument.DocumentElement.NamespaceURI);
// ReSharper restore PossibleNullReferenceException
// ReSharper disable PossibleNullReferenceException
                            XmlNode nodeContent = xmlDocument.CreateElement("a", xmlDocument.DocumentElement.NamespaceURI);
// ReSharper restore PossibleNullReferenceException
                            Debug.Assert(nodeContent != null && nodeContent.Attributes != null);
                            XmlAttribute attribute = xmlDocument.CreateAttribute("href");
                            attribute.Value = "zzReferences.xhtml";
                            nodeContent.Attributes.Append(attribute);
                            nodeContent.InnerText = "Endnotes";
                            spaceNode.AppendChild(nodeContent);
                            nodes[nodes.Count - 1].InsertAfter(spaceNode, next);
                        }
                    }
                }
                xmlDocument.Save(sourceFile);
            }
        }

        private void RemoveDuplicateBookName(string contentFolder)
        {
            string[] files = Directory.GetFiles(contentFolder, "PartFile*.xhtml");
            XmlDocument xmlDocument = Common.DeclareXMLDocument(true);
            var namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
			var xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, DtdProcessing = DtdProcessing.Parse }; //Common.DeclareXmlReaderSettings(false);
            foreach (string sourceFile in files)
            {
                if (!File.Exists(sourceFile)) return;
                XmlReader xmlReader = XmlReader.Create(sourceFile, xmlReaderSettings);
                xmlDocument.Load(xmlReader);
                xmlReader.Close();

                if (InputType.ToLower().Equals("scripture"))
                {
                    const string xPath = ".//xhtml:div[@class='scrBook']";
                    XmlNodeList nodes = xmlDocument.SelectNodes(xPath, namespaceManager);
                    Debug.Assert(nodes != null);
                    if (nodes.Count > 0)
                    {
                        string titleMainInnerText = string.Empty;
                        const string xPathValue = ".//xhtml:div[@class='Title_Main']";
                        XmlNodeList titleNodes = xmlDocument.SelectNodes(xPathValue, namespaceManager);
                        Debug.Assert(titleNodes != null);
                        if (titleNodes.Count > 0)
                        {
                            titleMainInnerText = titleNodes[0].InnerText;
                        }

                        var nodeInnerText = new StringBuilder();
                        nodeInnerText.Append(nodes[0].InnerXml);
                        nodeInnerText = nodeInnerText.Replace(titleMainInnerText + "</div>" + titleMainInnerText, titleMainInnerText + "</div>");
                        nodes[0].InnerXml = nodeInnerText.ToString();
                    }
                }
                xmlDocument.Save(sourceFile);
            }
        }

        private bool OnlyOneChapter(IEnumerable<string> chapterIdList, string s)
        {
            int count = 0;
            foreach (string s1 in chapterIdList)
            {
                var values = s1.Split('_');
                if (values[0] == s)
                    count += 1;
            }
            return count == 1;
        }

        private string GetChapterNumber(string value)
        {
            string[] valueList = value.Split('_');
            return valueList[valueList.Length - 1].ToLower().Replace("chapter", "");
        }

        private bool GetPageBreakStatus(string cssFileName)
        {
            try
            {
                if (cssFileName.Trim().Length == 0) { return false; }
                Param.LoadSettings();
                XmlDocument xDoc = Common.DeclareXMLDocument(false);
                string path = Param.SettingOutputPath;
                xDoc.Load(path);
                PageBreak = true;
                string xPath = "//stylePick/styles/others/style[@name='" + cssFileName + "']/styleProperty[@name='PageBreak']/@value";
                XmlNode includeImageNode = xDoc.SelectSingleNode(xPath);
                if (includeImageNode != null && includeImageNode.InnerText == "No")
                    PageBreak = false;

            }
            catch { }

            return PageBreak;
        }
        #endregion

        #endregion
    }
}
