using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using str = Microsoft.VisualBasic.Strings;
namespace PrincessConvert
{
    public partial class Main : Form
    {
    // program name
	// used in me.text and in about
	public static string sProgramName = "Princess";
	public static string sProgramVersion = "1.1";
	public static string sProgramVersionFull = sProgramVersion;
	public static string sProgramCopyright = "(c) 2009 Jim Albright";
		// I am abusing this on purpose.
	public static string sProgramCopyrightHoldersName = "2009-12-05";

	public static string sProgramDescription = "Princess is a Windows(tm) Graphical User Interface to help you use Prince xml. " + "Princess adds the following functionality to Prince:" + Constants.vbCrLf + Constants.vbCrLf + "    * Interactive picture insertion" + Constants.vbCrLf + "    * Interactive picture placement" + Constants.vbCrLf + "    * Interactive picture sizing" + Constants.vbCrLf + "    * Interactive tracking";

	// files
		// beware that this may change
	public static string sProgramDirectory = Directory.GetCurrentDirectory();
		// beware that this may change
	public static string sRequiredFilesFolder = sProgramDirectory + "\\RequiredFiles";
	// following files depend on above coming first
	//TODO public static string sMyPrincessFolder = My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\\My Princess";
    public static string sMyPrincessFolder =  Environment.SpecialFolder.MyDocuments + "\\My Princess";
	public static string sINIfile = sMyPrincessFolder + "\\princess.ini";
		// tab delimeted text
	public static string sLocalizationBackupFile = sRequiredFilesFolder + "\\localization.copy";
		// tab delimeted text
	public static string sLocalizationFile = sRequiredFilesFolder + "\\localization.txt";
	//  public static sPrincessFolder As String = My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\My Princess"
	public static string sPrinceEXE = "c:\\Program Files\\Prince\\Engine\\bin\\prince.exe";
	public static string sRemoveTrackingColor = sRequiredFilesFolder + "\\PrincessRemoveTrackingColor.css";
	public static string sShowTrackingColor = sRequiredFilesFolder + "\\PrincessShowTrackingColor.css";
	public static string sPrincessDocumentation = sRequiredFilesFolder + "\\Princess documentation.txt";
	public static string[] sMultipleFileNames = new string[10001];
	public bool blnProcessMultipleInputFiles = false;
	// so far have not needed to use the temp file/folder
	//public static sTempFolder As String = My.Computer.FileSystem.SpecialDirectories.Temp & "\Princess"
	//public static sTempFileName As String = sTempFolder & "\temp.tmp"
	//    Public utf8 As System.Text.UTF8Encoding = New System.Text.UTF8Encoding(False)

		// enclose file names in quotes because of spaces
	public static string quote = "\"";

	public static int filenum = 1;

	// set Adobe Acrobat or Web Browser

	public static bool blnBrowserOn = false;

	// variables
	//    Public aFiles(10000) As String
	//    Public sLineHeightInPoints As String = "12"
	public static int iAction = 0;
	public static int iSelectedItem = 1;
	public static int iLastActionNumber = 0;
	public static string sURLinput;
	public static bool blnProcessURL = false;
	public static string sToggleTracking = string.Empty;

		// store actions to undo/redo
	public static string[,] arrayActions = new string[1001, 2];
		// currently not used
	//TODO public  arrayGraphicFileNames;
	public static bool blnFileAlreadyOpen;
		// to disable tracking on internet files
	public static bool blnFileFromInternet;
	public bool blnOutputMultipleFiles = false;
	public static bool blnPDFshowBooksmarks = false;
	public static bool blnPDFshowScrollbars = false;
	public static bool blnPDFshowSinglePage = true;
	public static bool blnPDFshowThumbnails = false;
	public static bool blnPDFshowToolbar = false;
	public static bool blnPDFzoomOn = false;
	public static bool blnRedo = false;
	public static bool blnTreeViewSelected = false;
	public static bool blnUndo = false;
	public static bool blnXMLfileProcessed = false;
	public static decimal dColumnGapInPoints;
	public static decimal dColumnWidthInPoints = 400;
	public static decimal dMarginBottomInPoints;
	public static decimal dMarginLeftInPoints;
	public static decimal dMarginRightInPoints;
	public static decimal dMarginTopInPoints;
	public static decimal dPageHeightInPoints;
	public static decimal dPageWidthInPoints;
	public static int iActionNumber = 0;
		// future for changing interface language
	public static Int16 iLanguageSelected;
	public static int iLastPageNumber = 0;
	public static int dLineHeightInPoints = 12;
		// future for changing interface language
	public static Int16 iMaximumLocalizationLanguages = 6;
		// future for changing interface language
	public static  Int16 iMaximumLocalizationStrings = 2000;
	public static int iNumberOfColumns;
	public static int iNumberOfFilesToProcess = 0;
	public static int iPDFpageNumber = 1;
	public int iPDFzoomAmount = 100;
	public static Int16 iTracking;
	public static Image imageClipImage;
	public static string sAction;
	public static string sChapterNumber;
	public static string sCliptext;
	public static string sColumnHeightInLines = "20";
	public static string sColumnHeightInPoints = "500";
	public static string sDocumentFileName = "#none#";
	public static string sDocumentFolder = "#none#";
	public static string sDocumentName = "#none#";
	public static string sDocumentPath;
	public static string sDocumentsFolder = "#none#";
	public static string sEncryption = "#none#";
	public static string sGPSStylesheetFileName = "#none#";
	public static string sGPSStylesheetName = "#none#";
	public static string sGeneratedStylesheetFileName = "#none#";
	public static string sGraphicsFolder = sMyPrincessFolder + "\\figures";
	public static string sInputFileName = "#none#";
	public static string sInputFormat = "auto";
		// future for changing interface language
	public static string[,] sLocalizationStrings = new string[iMaximumLocalizationStrings + 1, iMaximumLocalizationLanguages + 1];
	public static string sLogCurrentName;
	public static string sLogHistoryName;
	public static string sMasterFileName;
	public static string sModifiedFileName;
	public static string sPDFoutputName;
	public static string sPageHeight = "8in";
	public static string sPageOrientation = "Portrait";
	public static string sPageWidth = "5in";
	public static string sSelectedItem;
	public static string sSelectedPictureFileName = "#none#";
	public static string sSelectedPictureName = "#none#";
	public static string sSelectedText;
	public static string sSettingName;
	public static string sStylesheetFileName = "#none#";
	public static string sStylesheetName = "#none#";

	public static string sTracking;

	public static bool blnAlternateSelector = false;
	// translate
	public static string sClickConvertToCreatePDF = "Click Convert to create PDF.";
    public static string sPadding = "\r\n"; // Constants.vbCrLf + Constants.vbCrLf;
	public static string sDocumentNone = "Document: none ";
	public static string sNoPDFavailable = "No PDF available for viewing yet.";
	public static string sNothingMatched = "Selected text didn't match anything." + Constants.vbCrLf + "Avoid text with special formatting like bold, italic, or superscript.";
	public static string sNothingSelected = "No text selected.";
	public static string sSelectDocument = "Select document to process.";
	public static string sSelectStylesheet = "Select stylesheet.";
	public static string sSelectedTextMatchNotUnique = "Selected text matches more than one time." + Constants.vbCrLf + "Please select more words or select from a different part of the paragraph.";
	public static string sStartOver = "Start over?";
	public static string sStartOverOrContinue = "Yes, erase previous work and start over." + Constants.vbCrLf + "No, continue with previous work.";
	public static string sStylesheetNone = "Stylesheet: none";
	public static string sToolbarHidden = "Toolbar hidden";
	public static string sToolbarShowing = "Toolbar showing";
	public static string sTrackingHidden = "Tracking hidden";

	public static string sTrackingShowing = "Tracking showing";

	public static DataTable table = new DataTable("fileTable");
	// Declare variables for DataColumn and DataRow objects.
	public static DataColumn column;
	public static DataRow row;
	private DataSet dataSet;
    //settings settings = new settings();
    //picture picture = new picture();
    //multipleFiles multipleFiles = new multipleFiles();
    public Main()
	{
        InitializeComponent();
		// This call is required by the Windows Form Designer.
        //InitializeComponent();
        //// Add any initializationAfterthe InitializeComponent() call.
        //My.Computer.Clipboard.Clear();
        //this.Show();
        //this.Update();

		if (Directory.Exists(sRequiredFilesFolder)) {
		//good
		} else {
			Directory.CreateDirectory(sRequiredFilesFolder);
		}

        //if (Directory.Exists(sMyPrincessFolder)) {
        ////good
        //} else {
        //    Directory.CreateDirectory(sMyPrincessFolder);
        //}

		this.Text = sProgramName + " " + sProgramVersion;
		hideAlternateSelector();
		pnlURL.Hide();
		readIniFile();
		locatePrinceExe();
		enableGPSstylesheet();
		getGeneratedStylesheetName();
		enableGeneratedStylesheet();
		displayInfo();
		turnOnOffEditing();
		// PictureBox1.Hide()
		// 2010-03-20 disable keyboard shortcuts
		//        Keyboard.HookKeyboard()
		//       Keyboard.CheckHooked()

		displayPDFifExists();
		//makeDataTable()
	}

	public void displayPDFifExists()
	{
		if (File.Exists(sPDFoutputName)) {
			displayPDF();
			btnClose.Visible = true;
		//         resetActions()
		} else {
			if (DebuggingModeToolStripMenuItem.Checked == true) {
				MessageBox.Show(sDocumentFileName + " " + sDocumentName);
			}
			if (sDocumentName == "#none#") {
				// todo copy the document file 
                //tbFeedback.Text = File.ReadAllText(sPrincessDocumentation);
				btnClose.Visible = false;
			} else {
				// both document and stylesheet exist
				//tbFeedback.Text = sNoPDFavailable + Constants.vbCrLf + sClickConvertToCreatePDF;
				btnClose.Visible = false;
			}
		}

	}
	public static void getGeneratedStylesheetName()
	{
		//TODO VER 4.0 dynamic sStylesheetNameMinusPath = getFullFileNameWithoutExtension(sGPSStylesheetFileName) + ".CSS";
		//   Dim sPath = Path.GetFullPath(sGPSStylesheetFileName)
		//TODO VER 4.0 dynamic sPath = Path.GetDirectoryName(sGPSStylesheetFileName);
		//sGeneratedStylesheetFileName = sPath + "\\" + sStylesheetNameMinusPath;
	}
	public void enableGPSstylesheet()
	{
		if (File.Exists(sGPSStylesheetFileName)) {
			GPSStylesheetToolStripMenuItem.Enabled = true;
		} else {
			GPSStylesheetToolStripMenuItem.Enabled = false;
		}

	}
	public void enableGeneratedStylesheet()
	{
		if (File.Exists(sGeneratedStylesheetFileName)) {
			GeneratedStylesheetToolStripMenuItem.Enabled = true;
		} else {
			GeneratedStylesheetToolStripMenuItem.Enabled = false;

		}

	}

	private void locatePrinceExe()
	{
		if (File.Exists(sPrinceEXE)) {
		// good
		} else {
			// where is Prince.exe
			//dynamic response = MessageBox.Show("Princess needs to find her Prince in order to do any work." + Constants.vbCrLf + "Is Prince.exe on this computer?", "Princess needs to find her Prince.", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            DialogResult response = MessageBox.Show("Princess needs to find her Prince in order to do any work." + Constants.vbCrLf + "Is Prince.exe on this computer?", "Princess needs to find her Prince.", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
			if (response == System.Windows.Forms.DialogResult.Yes) {
				OpenFileDialog1.Title = "Please locate Prince\\Engine\\bin\\prince.exe";
				OpenFileDialog1.FileName = "";
				OpenFileDialog1.Filter = "bin\\prince.exe (*.exe)|*.exe;|All files (*.*)|*.*";
				this.OpenFileDialog1.Multiselect = false;

				OpenFileDialog1.ShowDialog();
				sPrinceEXE = OpenFileDialog1.FileName;
				writeIniFile();
			} else if (response == System.Windows.Forms.DialogResult.No) {
				MessageBox.Show("Before you will be able to process any files you will need to install Prince." + Constants.vbCrLf + "Prince offers a free Personal license for interactive use on a single computer (as per the license agreement)." + Constants.vbCrLf + "Download at www.princexml.com.", "Please install Prince", MessageBoxButtons.OK, MessageBoxIcon.Information);
			} else if (response == System.Windows.Forms.DialogResult.Cancel) {
				// close message
			}
		}



	}

	private string getProjectPathFromFullName(string sProjectFileName)
	{
		string temp = null;
		// TODO temp = str.Left(sProjectFileName, str.InStrRev(sProjectFileName, "\\"));
		return temp;
	}

	private void DocumentLocalToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
	{
		//OpenFileDialog1.InitialDirectory = sMyPrincessFolder + "\\input";
		OpenFileDialog1.FileName = "";
		OpenFileDialog1.Filter = "XML files (*.xml, *.xhtml, *.html, *.htm)|*.xml;*.xhtml;*.html;*.htm;|All files (*.*)|*.*";
		OpenFileDialog1.Multiselect = false;

		OpenFileDialog1.ShowDialog();
		sInputFileName = OpenFileDialog1.FileName;
		blnProcessMultipleInputFiles = false;
		blnProcessURL = false;

		makeFileNamesFromInputFileName();

		writeIniFile();
		startOverOrContinue();
		displayInfo();
		turnOnOffEditing();

	}
	public string getFileNameWithoutExtensionFromFullName(string filename)
	{
		if (filename == "#none#") {
		// skip
		} else {
			filename = Path.GetFileName(filename);
			filename = Path.GetFileNameWithoutExtension(filename);


			//       filename = getFileNameWithExtensionFromFullName(filename)
			// assume one .
			//      filename = str.Left(filename, InStr(filename, ".") - 1)
		}
		return filename;
	}
	public string getFullFileNameWithoutExtension(string filename)
	{
		if (filename == "#none#") {
		// skip
		} else {
			// assume one . assumption removed
			filename = Path.GetFileNameWithoutExtension(filename);
			//       filename = str.Left(filename, InStr(filename, ".") - 1)
		}
		return filename;

	}
	public static string getFileNameWithExtensionFromFullName(string filename)
	{

		//    Dim i As Int16
		if (filename == "#none#" | filename == "OpenFileDialog1") {
		// skip
		} else {
			filename = Path.GetFileName(filename);

			//            Do While InStr(filename, "\") > 0
			//i = InStr(filename, "\")
			// filename = str.Mid(filename, InStr(filename, "\") + 1)
			//   Loop
		}
		return filename;
	}
	public void turnOnOffEditing()
	{
		if (blnProcessMultipleInputFiles == true | blnProcessURL == true) {
			TrackingMinus05MenuItem.Enabled = false;
			TrackingMinus10MenuItem.Enabled = false;
			TrackingMinus15MenuItem.Enabled = false;
			TrackingMinus20MenuItem.Enabled = false;
			TrackingMinus25MenuItem.Enabled = false;
			TrackingMinus30MenuItem.Enabled = false;
			TrackingMinus35MenuItem.Enabled = false;
			TrackingMinus40MenuItem.Enabled = false;
			TrackingNoneMenuItem.Enabled = false;
			TrackingPlus05MenuItem.Enabled = false;
			TrackingPlus10MenuItem.Enabled = false;
			TrackingPlus15MenuItem.Enabled = false;
			TrackingPlus20MenuItem.Enabled = false;
			TrackingPlus25MenuItem.Enabled = false;
			TrackingPlus30MenuItem.Enabled = false;
			TrackingPlus35MenuItem.Enabled = false;
			TrackingPlus40MenuItem.Enabled = false;
			TrackingPlus60MenuItem.Enabled = false;
			InsertHereToolStripMenuItem.Enabled = false;
			SelectToolStripMenuItem.Enabled = false;
			RemoveToolStripMenuItem.Enabled = false;
			RemoveAllToolStripMenuItem.Enabled = false;
			FolderToolStripMenuItem.Enabled = false;
			btnAlternateSelector.Visible = false;
			DocumentSourceToolStripMenuItem.Enabled = false;

		} else {
			TrackingMinus05MenuItem.Enabled = true;
			TrackingMinus10MenuItem.Enabled = true;
			TrackingMinus15MenuItem.Enabled = true;
			TrackingMinus20MenuItem.Enabled = true;
			TrackingMinus25MenuItem.Enabled = true;
			TrackingMinus30MenuItem.Enabled = true;
			TrackingMinus35MenuItem.Enabled = true;
			TrackingMinus40MenuItem.Enabled = true;
			TrackingNoneMenuItem.Enabled = true;
			TrackingPlus05MenuItem.Enabled = true;
			TrackingPlus10MenuItem.Enabled = true;
			TrackingPlus15MenuItem.Enabled = true;
			TrackingPlus20MenuItem.Enabled = true;
			TrackingPlus25MenuItem.Enabled = true;
			TrackingPlus30MenuItem.Enabled = true;
			TrackingPlus35MenuItem.Enabled = true;
			TrackingPlus40MenuItem.Enabled = true;
			TrackingPlus60MenuItem.Enabled = true;
			InsertHereToolStripMenuItem.Enabled = true;
			SelectToolStripMenuItem.Enabled = true;
			RemoveToolStripMenuItem.Enabled = true;
			RemoveAllToolStripMenuItem.Enabled = true;
			FolderToolStripMenuItem.Enabled = true;
			displayPictureMenuItems();
			btnAlternateSelector.Visible = true;
			DocumentSourceToolStripMenuItem.Enabled = true;

		}
	}

	public void displayInfo()
	{
		string sDisplayDocumentName = getFileNameWithExtensionFromFullName(sInputFileName);
		if (blnProcessMultipleInputFiles) {
			sDisplayDocumentName = sDisplayDocumentName + "/ [" + iNumberOfFilesToProcess.ToString() + " files]";
		}

		if (sDisplayDocumentName == null | sDisplayDocumentName.Contains("#none#")) {
			if (sStylesheetName == null | sStylesheetName.Contains("#none#")) {
				// no document; no stylesheet
				this.Text = sProgramName + " " + sProgramVersionFull;
			} else {
				// no document;  stylesheet
				this.Text = sDocumentNone + sStylesheetNone + " - " + sProgramName;
			}
		} else {
			if (string.IsNullOrEmpty(sStylesheetName) | sStylesheetName.Contains("#none#")) {
				// document; no stylesheet
				this.Text = "Document: " + sDisplayDocumentName + " - " + sStylesheetNone + " - " + sProgramName;
			} else {
				// document; stylesheet
				this.Text = "Document: " + sDisplayDocumentName + " - Stylesheet: " + sStylesheetName + " - " + sProgramName;
			}
		}

	}
	private void startOverOrContinue()
	{
		//dynamic response = null;
		// assuming no doucment we can't start over and
		// assuming multiple documents to process you only process individual files 
	    DialogResult response;
		if (sDocumentName.Contains("#none#") | blnProcessMultipleInputFiles == true | blnProcessURL == true) {
		// skip
		//ElseIf iNumberOfFilesToProcess = 0 Then
		// skip
		} else if (sDocumentFileName.StartsWith("http")) {
		// skip
		} else if (File.Exists(sDocumentFileName) == true) {
			// ask to use previous work

			response = MessageBox.Show(sStartOverOrContinue, sStartOver, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (response == DialogResult.Yes) {
				// make copy to use -- just starting out
				File.Delete(sDocumentFileName);
				File.Copy(sInputFileName, sInputFileName + ".copy");
				if (convertUsingPrince()) {
					iPDFpageNumber = 1;
					displayPDF();
				}
			//      resetActions()
			} else {
				// continue using previous
			}
		} else {
			if (sInputFileName == "#none#" | sInputFileName == null) {
				//       resetActions()
				MessageBox.Show("No file selected yet. Please select a document to process.", "Missing document", MessageBoxButtons.OK, MessageBoxIcon.Information);
			} else {
				File.Copy(sInputFileName, sInputFileName + ".copy");
			}
		}
	}
	private void PDFToolStripMenuItem1_Click(System.Object sender, System.EventArgs e)
	{
		displayPDF();

	}
	public void displayPDF()
	{
		try {
			this.Cursor = Cursors.Default;
			this.ProgressBar1.Value = 0;
			//Me.SendToBack() ' see about hiding some of the screen jumping
			//AxAcroPDF1.Hide()
			//AxAcroPDF1.MakeDirty()
			//AxAcroPDF1.SuspendLayout()
			AxAcroPDF1.LoadFile(sPDFoutputName);
			AxAcroPDF1.setCurrentPage(iPDFpageNumber);
			AxAcroPDF1.setShowScrollbars(blnPDFshowScrollbars);
			AxAcroPDF1.setShowToolbar(blnPDFshowToolbar);
			if (blnPDFshowSinglePage == true) {
				AxAcroPDF1.setView("Fit");
			} else {
			}
			AxAcroPDF1.setPageMode(blnPDFshowSinglePage.ToString());
			if (blnPDFshowThumbnails == true) {
				AxAcroPDF1.setPageMode("thumbs");
			} else if (blnPDFshowBooksmarks == true) {
				AxAcroPDF1.setPageMode("bookmarks");
			} else {
				AxAcroPDF1.setPageMode("none");
			}

			displayPageNumber();


			tsbtnToggleToolbar.Text = sToolbarHidden;
			if (blnPDFzoomOn == true) {
				AxAcroPDF1.setZoom(iPDFzoomAmount);
			} else {
				// skip
			}
			// AxAcroPDF1.Refresh()
			AxAcroPDF1.BringToFront();
		//AxAcroPDF1.Show()
		//  Me.Show()
		//   AxAcroPDF1.Show()
		} catch (Exception ex) {
			MessageBox.Show("Problem opening PDF file." + "\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

	}

	// -------------------------------------------------
	// INI file
	// -------------------------------------------------
	public void readIniFile()
	{
		if (File.Exists(sINIfile)) {
			try {
				FileSystem.FileOpen(filenum, sINIfile, OpenMode.Input, OpenAccess.Read,OpenShare.Default,1);
				string temp = inputLine();
				// read start of file
				readSettings();
				FileSystem.FileClose(filenum);
			} catch (Exception ex) {
				// corrupt file - write blank
				//   If DebuggingModeToolStripMenuItem.Checked Then
				MessageBox.Show(ex.Message + Constants.vbCrLf + "Fixing ini file so this won't happen again", "INI read error", MessageBoxButtons.OK, MessageBoxIcon.Information);
				//  End If
				FileSystem.FileClose(filenum);
				writeIniFile();
				// if reading file
			}
		} else {
			// create ini file
			writeIniFile();
			// if file doesn't exist
		}
	}
	public static string inputLine()
	{
		string temp = string.Empty;
		temp = FileSystem.LineInput(filenum);
		temp = temp.Replace("\"", "");
		return temp;
	}
	private void readSettings()
	{
		readProcessingMultiple();
		readProcessingURL();
		readDocumentFileName();
		readURL();
		readStylesheetFileName();
		readGPSStylesheetFileName();
		readInputFormat();
		readEncryption();
		readGraphicsFolder();
		readCurrentPageNumber();
		readLastPageNumber();
		readMarginLeftInPoints();
		readMarginRightInPoints();
		readMarginTopInPoints();
		readMarginBottomInPoints();
		readColumnGapInPoints();
		readNumberOfColumns();
		readPageWidth();
		readPageHeight();
		readPageOrientation();
		readLineHeightInPoints();
		readColumnWidthInPoints();
		readPageWidthInPoints();
		readPageHeightInPoints();
		readOutputMultipleFiles();
		readCurrentGraphicFileName();
		readDocumentsFolder();
		readPDFshowToolbar();
		readPDFshowSinglePage();
		readPDFshowScrollbars();
		readPDFshowThumbnails();
		readPDFshowBookmarks();
		readPDFzoomOn();
		readPDFzoomAmount();
		readPrinceExeFileName();
		readNumberOfFilesToProcess();
		readFileNamesToProcess();
	}
	public void writeIniFile()
	{
		try {
			FileSystem.FileOpen(filenum, sINIfile, OpenMode.Output, OpenAccess.Write,OpenShare.Default,1);
			FileSystem.WriteLine(filenum, "******************************** start of ini file ********************************");
			writeDefaultSettings();
			FileSystem.WriteLine(filenum, "******************************** end of ini file ********************************");
			FileSystem.FileClose(filenum);
		} catch (Exception ex) {
			// 
			MessageBox.Show("Error trying to write the Prince.ini file." + Constants.vbCrLf + ex.Message, "Can not create file", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
	}
	//Private Sub writeDescriptionOfFile()

	//WriteLine(filenum, "settings name")
	//End Sub
	private void writeDefaultSettings()
	{
		writeProcessingMultiple();
		writeProcessingURL();
		writeDocumentFileName();
		writeURL();
		writeStylesheetFileName();
		writeGPSStylesheetFileName();
		writeInputFormat();
		writeEncryption();
		writeGraphicsFolder();
		writeCurrentPageNumber();
		writeLastPageNumber();
		writeMarginLeftInPoints();
		writeMarginRightInPoints();
		writeMarginTopInPoints();
		writeMarginBottomInPoints();
		writeColumnGapInPoints();
		writeNumberOfColumns();
		writePageWidth();
		writePageHeight();
		writePageOrientation();
		writeLineHeightInPoints();
		writeColumnWidthInPoints();
		writePageWidthInPoints();
		writePageHeightInPoints();
		writeOutputMultipleFiles();
		writeCurrentGraphicFileName();
		writeDocumentsFolder();
		writePDFshowToolbar();
		writePDFshowSinglePage();
		writePDFshowScrollbars();
		writePDFshowThumbnails();
		writePDFshowBookmarks();
		writePDFzoomOn();
		writePDFzoomAmount();
		writePrinceExeFileName();
		writeNumberOfFilesToProcess();
		writeFileNamesToProcess();
	}
	private void readProcessingMultiple()
	{
		string temp = inputLine();
		blnProcessMultipleInputFiles = trueOrFalse(inputLine());
	}
	private void writeProcessingMultiple()
	{
		FileSystem.WriteLine(filenum, "Processing Multiple");
		FileSystem.WriteLine(filenum, blnProcessMultipleInputFiles);
	}
	private void readProcessingURL()
	{
		string temp = inputLine();
		blnProcessURL = trueOrFalse(inputLine());
	}
	private void writeProcessingURL()
	{
		FileSystem.WriteLine(filenum, "Processing URL");
		FileSystem.WriteLine(filenum, blnProcessURL);
	}
	private void writeDocumentFileName()
	{
		FileSystem.WriteLine(filenum, "document file name");
		FileSystem.WriteLine(filenum, sInputFileName);
	}
	private void readURL()
	{
		string temp = inputLine();
		sURLinput = inputLine();
		// always work starting from copy
		makeFileNamesFromInputFileName();

	}
	private void writeURL()
	{
		FileSystem.WriteLine(filenum, "URL name");
		FileSystem.WriteLine(filenum, sURLinput);
	}
	private void readDocumentFileName()
	{
		string temp = inputLine();
		sInputFileName = inputLine();
		// always work starting from copy
		makeFileNamesFromInputFileName();

	}
	private void makeFileNamesFromInputFileName()
	{
		//yyyy()
		if (sInputFileName == "#none#") {
		// skip
		} else if (blnProcessURL) {
			sDocumentName = sInputFileName;

			sDocumentPath = sMyPrincessFolder + "\\";
			sPDFoutputName = sMyPrincessFolder + "\\" + sDocumentName + ".pdf";
			sLogCurrentName = sDocumentPath + "log-current.txt";
			sLogHistoryName = sDocumentPath + "log-history.txt";


		} else if (blnProcessMultipleInputFiles) {
            multipleFiles multipleFiles = new multipleFiles();
			if (multipleFiles.rbSingleOutputFile.Checked == true) {
				sDocumentFileName = sDocumentsFolder;
				sDocumentName = getFileNameWithExtensionFromFullName(sDocumentsFolder);
				sDocumentPath = sDocumentsFolder;
				sPDFoutputName = sDocumentPath + "\\" + sDocumentName + ".pdf";
				sLogCurrentName = sDocumentPath + "log-current.txt";
				sLogHistoryName = sDocumentPath + "log-history.txt";

			} else {
				sDocumentFileName = sInputFileName + ".copy";
				sDocumentName = getFileNameWithoutExtensionFromFullName(sInputFileName);
				sDocumentPath = getProjectPathFromFullName(sInputFileName);
				sPDFoutputName = sDocumentPath + sDocumentName + ".pdf";
				sLogCurrentName = sDocumentPath + "log-current.txt";
				sLogHistoryName = sDocumentPath + "log-history.txt";
			}
		// single file
		} else {
			sDocumentFileName = sInputFileName + ".copy";
			sDocumentName = getFileNameWithoutExtensionFromFullName(sInputFileName);
			sDocumentPath = getProjectPathFromFullName(sInputFileName);
			sPDFoutputName = sDocumentPath + sDocumentName + ".pdf";
			sLogCurrentName = sDocumentPath + "log-current.txt";
			sLogHistoryName = sDocumentPath + "log-history.txt";


		}

	}
	private void writePrinceExeFileName()
	{
		FileSystem.WriteLine(filenum, "Prince.exe path");
		FileSystem.WriteLine(filenum, sPrinceEXE);
	}
	private void readPrinceExeFileName()
	{
		string temp = inputLine();
		sPrinceEXE = inputLine();
	}
	private void writeGPSStylesheetFileName()
	{
		FileSystem.WriteLine(filenum, "GPS stylesheet file name");
		FileSystem.WriteLine(filenum, sGPSStylesheetFileName);
	}
	private void readGPSStylesheetFileName()
	{
		string temp = inputLine();
		sGPSStylesheetFileName = inputLine();
		sGPSStylesheetName = getFileNameWithExtensionFromFullName(sStylesheetFileName);
	}
	private void writeStylesheetFileName()
	{
		FileSystem.WriteLine(filenum, "stylesheet file name");
		FileSystem.WriteLine(filenum, sStylesheetFileName);
	}
	private void readStylesheetFileName()
	{
		string temp = inputLine();
		sStylesheetFileName = inputLine();
		sStylesheetName = getFileNameWithExtensionFromFullName(sStylesheetFileName);
	}
	private void writeInputFormat()
	{
		FileSystem.WriteLine(filenum, "input format");
		FileSystem.WriteLine(filenum, sInputFormat);
	}
	private void readInputFormat()
	{
		string temp = inputLine();
		sInputFormat = inputLine();
	}
	private void writeEncryption()
	{
		FileSystem.WriteLine(filenum, "encryption");
		FileSystem.WriteLine(filenum, sEncryption);
	}
	private void readEncryption()
	{
		string temp = inputLine();
		sEncryption = inputLine();
	}
	private void writeCurrentGraphicFileName()
	{
		FileSystem.WriteLine(filenum, "current graphic file name");
		FileSystem.WriteLine(filenum, sSelectedPictureFileName);
	}
	private void readCurrentGraphicFileName()
	{
		string temp = inputLine();
		sSelectedPictureFileName = inputLine();
	}
	private void writeGraphicsFolder()
	{
		FileSystem.WriteLine(filenum, "graphics folder");
		FileSystem.WriteLine(filenum, sGraphicsFolder);
	}
	private void readGraphicsFolder()
	{
		string temp = inputLine();
		sGraphicsFolder = inputLine();
	}
	private void writeDocumentsFolder()
	{
		FileSystem.WriteLine(filenum, "multiple documents to process folder name");
		FileSystem.WriteLine(filenum, sDocumentsFolder);
	}
	private void readDocumentsFolder()
	{
		string temp = inputLine();
		sDocumentsFolder = inputLine();
	}
	private void writeNumberOfFilesToProcess()
	{
		FileSystem.WriteLine(filenum, "number of files to process (multiple)");
		FileSystem.WriteLine(filenum, iNumberOfFilesToProcess);
	}
	private void readNumberOfFilesToProcess()
	{
		string temp = inputLine();
		iNumberOfFilesToProcess = int.Parse(inputLine());
	}
	private void writeFileNamesToProcess()
	{
		FileSystem.WriteLine(filenum, "file names to process");
		//     For Each fileName In multipleFiles.lbFiles.Items
		foreach (string filename_loopVariable in sMultipleFileNames) {
			string filename = filename_loopVariable;
			if (filename == null) {
			// skip
			} else {
				FileSystem.WriteLine(filenum, filename);
			}
		}

		//        For x = 0 To iNumberOfFilesToProcess - 1
		//       Next
	}
	private void readFileNamesToProcess()
	{
        multipleFiles multipleFiles = new multipleFiles();
		string temp = inputLine();
		multipleFiles.lbFiles.Items.Clear();
		for (int x = 0; x <= iNumberOfFilesToProcess - 1; x++) {
			temp = inputLine();
			multipleFiles.lbFiles.Items.Add(temp);
			sMultipleFileNames[x] = temp;
		}
	}

	private void writeLastPageNumber()
	{
		FileSystem.WriteLine(filenum, "last PDF page number");
		FileSystem.WriteLine(filenum, iLastPageNumber);
	}
	private void readLastPageNumber()
	{
		string temp = inputLine();
		iLastPageNumber = int.Parse(inputLine());
	}
	private void writePageWidth()
	{
		FileSystem.WriteLine(filenum, "page width");
		FileSystem.WriteLine(filenum, sPageWidth);
	}
	private void readPageWidth()
	{
		string temp = inputLine();
		sPageWidth = inputLine();
	}
	private void writePageHeight()
	{
		FileSystem.WriteLine(filenum, "page height");
		FileSystem.WriteLine(filenum, sPageHeight);
	}
	private void readPageHeight()
	{
		string temp = inputLine();
		sPageHeight = inputLine();
	}
	private void writePageOrientation()
	{
		FileSystem.WriteLine(filenum, "page orientation");
		FileSystem.WriteLine(filenum, sPageOrientation);
	}
	private void readPageOrientation()
	{
		string temp = inputLine();
		sPageOrientation = inputLine();
	}
	private void writePageHeightInPoints()
	{
		FileSystem.WriteLine(filenum, "page height in points");
		FileSystem.WriteLine(filenum, dPageHeightInPoints);
	}
	private void readPageHeightInPoints()
	{
		string temp = inputLine();
		dPageHeightInPoints = int.Parse(inputLine());
	}
	private void writePageWidthInPoints()
	{
		FileSystem.WriteLine(filenum, "page width in points");
		FileSystem.WriteLine(filenum, dPageWidthInPoints);
	}
	private void readPageWidthInPoints()
	{
		string temp = inputLine();
		dPageWidthInPoints = decimal.Parse(inputLine());
	}
	private void writeCurrentPageNumber()
	{
		FileSystem.WriteLine(filenum, "current PDF page number");
		FileSystem.WriteLine(filenum, iPDFpageNumber);
	}
	private void readCurrentPageNumber()
	{
		string temp = inputLine();
		iPDFpageNumber = int.Parse(inputLine());
	}
	private void writeLineHeightInPoints()
	{
		FileSystem.WriteLine(filenum, "line height in points");
		if (dLineHeightInPoints < 5) {
			FileSystem.WriteLine(filenum, 15);
		} else {
			FileSystem.WriteLine(filenum, dLineHeightInPoints);
		}
	}
	private void readLineHeightInPoints()
	{
		string temp = inputLine();
		dLineHeightInPoints = int.Parse(inputLine());
	}
	private void writeMarginLeftInPoints()
	{
		FileSystem.WriteLine(filenum, "Margin left in points");
		FileSystem.WriteLine(filenum, dMarginLeftInPoints);
	}
	private void readMarginLeftInPoints()
	{
		string temp = inputLine();
		dMarginLeftInPoints = int.Parse(inputLine());
	}
	private void writeMarginRightInPoints()
	{
		FileSystem.WriteLine(filenum, "Margin right in points");
		FileSystem.WriteLine(filenum, dMarginRightInPoints);
	}
	private void readMarginRightInPoints()
	{
		string temp = inputLine();
		dMarginRightInPoints = int.Parse(inputLine());
	}
	private void writeMarginTopInPoints()
	{
		FileSystem.WriteLine(filenum, "Margin top in points");
		FileSystem.WriteLine(filenum, dMarginTopInPoints);
	}
	private void readMarginTopInPoints()
	{
		string temp = inputLine();
		dMarginTopInPoints = int.Parse(inputLine());
	}
	private void writeMarginBottomInPoints()
	{
		FileSystem.WriteLine(filenum, "Margin bottom in points");
		FileSystem.WriteLine(filenum, dMarginBottomInPoints);
	}
	private void readMarginBottomInPoints()
	{
		string temp = inputLine();
		dMarginBottomInPoints = int.Parse(inputLine());
	}

	private void writeColumnGapInPoints()
	{
		FileSystem.WriteLine(filenum, "Column gap in points");
		FileSystem.WriteLine(filenum, dColumnGapInPoints);
	}
	private void readColumnGapInPoints()
	{
		string temp = inputLine();
		dColumnGapInPoints = int.Parse(inputLine());
	}
	private void writeNumberOfColumns()
	{
		FileSystem.WriteLine(filenum, "Number of columns");
		FileSystem.WriteLine(filenum, iNumberOfColumns);
	}
	private void readNumberOfColumns()
	{
		string temp = inputLine();
		iNumberOfColumns = int.Parse(inputLine());
	}
	private void writeColumnWidthInPoints()
	{
		FileSystem.WriteLine(filenum, "column width in points");
		FileSystem.WriteLine(filenum, dColumnWidthInPoints);
	}
	private void readColumnWidthInPoints()
	{
		string temp = inputLine();
		dColumnWidthInPoints = int.Parse(inputLine());
	}
	private void writePDFshowToolbar()
	{
		FileSystem.WriteLine(filenum, "PDF show toolbar");
		FileSystem.WriteLine(filenum, blnPDFshowToolbar);
	}
	private void readPDFshowToolbar()
	{
		string temp = inputLine();
		blnPDFshowToolbar = trueOrFalse(inputLine());
	}
	private void writePDFshowSinglePage()
	{
		FileSystem.WriteLine(filenum, "PDF show single page");
		FileSystem.WriteLine(filenum, blnPDFshowSinglePage);
	}
	private void readPDFshowSinglePage()
	{
		string temp = inputLine();
		blnPDFshowSinglePage = trueOrFalse(inputLine());
	}
	private void writePDFshowScrollbars()
	{
		FileSystem.WriteLine(filenum, "PDF show scrollbars");
		FileSystem.WriteLine(filenum, blnPDFshowScrollbars);
	}
	private void readPDFshowScrollbars()
	{
		string temp = inputLine();
		blnPDFshowScrollbars = trueOrFalse(inputLine());
		//       PDFdisplay.cbShowScrollBars.Checked = blnPDFshowScrollbars
	}
	private void writePDFshowThumbnails()
	{
		FileSystem.WriteLine(filenum, "PDF show thumbnails");
		FileSystem.WriteLine(filenum, blnPDFshowThumbnails);
	}
	private void readPDFshowThumbnails()
	{
		string temp = inputLine();
		blnPDFshowThumbnails = trueOrFalse(inputLine());
		//      PDFdisplay.cbShowThumbnails.Checked = blnPDFshowThumbnails
	}
	private void writePDFshowBookmarks()
	{
		FileSystem.WriteLine(filenum, "PDF show bookmarks");
		FileSystem.WriteLine(filenum, blnPDFshowBooksmarks);
	}
	private void readPDFshowBookmarks()
	{
		string temp = inputLine();
		blnPDFshowBooksmarks = trueOrFalse(inputLine());
		//     PDFdisplay.cbPDFshowBookmarks.Checked = blnPDFshowBooksmarks
	}
	private void writePDFzoomOn()
	{
		FileSystem.WriteLine(filenum, "PDF zoom on");
		FileSystem.WriteLine(filenum, blnPDFzoomOn);
	}
	private void readPDFzoomOn()
	{
		string temp = inputLine();
		blnPDFzoomOn = trueOrFalse(inputLine());
		//    PDFdisplay.cbPDFzoomOn.Checked = blnPDFzoomOn

	}
	private void writePDFzoomAmount()
	{
		FileSystem.WriteLine(filenum, "PDF zoom amount");
		FileSystem.WriteLine(filenum, this.iPDFzoomAmount);
	}
	private void readPDFzoomAmount()
	{
		string temp = inputLine();
		this.iPDFzoomAmount = int.Parse(inputLine());
		//   PDFdisplay.numericUpDownPDFzoom.Value = Me.iPDFzoomAmount
	}
	private void writeProcessMultipleInputFiles()
	{
		FileSystem.WriteLine(filenum, "Output multiple files");
		FileSystem.WriteLine(filenum, this.blnProcessMultipleInputFiles);
	}
	private void readProcessMultipleInputFiles()
	{
		string temp = inputLine();
		this.blnProcessMultipleInputFiles = trueOrFalse(inputLine());
	}
	private void writeOutputMultipleFiles()
	{
		FileSystem.WriteLine(filenum, "Process multiple input files");
		FileSystem.WriteLine(filenum, this.blnOutputMultipleFiles);
	}
	private void readOutputMultipleFiles()
	{
		string temp = inputLine();
		this.blnOutputMultipleFiles = trueOrFalse(inputLine());
	}

	private bool trueOrFalse(string x)
	{
		if (x == "#TRUE#") {
			return true;
		} else {
			return false;
		}
	}

	private void INIToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
	{
		try {
			tbFeedback.Text = Constants.vbCrLf + File.ReadAllText(sINIfile) + Constants.vbCrLf + Constants.vbCrLf;
			showFeedback();

		} catch (Exception ex) {
			// don't show INI file]
			writeIniFile();
			tbFeedback.Text = Constants.vbCrLf + File.ReadAllText(sINIfile) + Constants.vbCrLf + Constants.vbCrLf;
			showFeedback();
		}
	}

	private void FolderToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
	{
		getGraphicFilesToDisplay();
	}

	private void StylesheetToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
	{
		OpenFileDialog1.InitialDirectory = sMyPrincessFolder + "\\CSS";

		OpenFileDialog1.FileName = "";
		OpenFileDialog1.Filter = "Stylesheet files (*.css)|*.css;|All files (*.*)|*.*";
		this.OpenFileDialog1.Multiselect = false;

		OpenFileDialog1.ShowDialog();
		sStylesheetFileName = OpenFileDialog1.FileName;
		sStylesheetName = getFileNameWithExtensionFromFullName(sStylesheetFileName);
		getValuesFromStylesheet();
		writeIniFile();
		displayInfo();
		turnOnOffEditing();
		convertWithPrinceAndDisplayPDF();

	}
	public static void makeTextCanonical()
	{
		string sText = File.ReadAllText(sDocumentFileName);
		sText = regexReplace(sText, Constants.vbCrLf + "<\\?xml", "<\\?xml").ToString();
		sText = regexReplace(sText, Constants.vbCrLf, " ").ToString();
		sText = regexReplace(sText, "  ", " ").ToString();
		sText = regexReplace(sText, "<", Constants.vbCrLf + "<").ToString();
		sText = regexReplace(sText, Constants.vbCrLf + "<span", "<span").ToString();
		sText = regexReplace(sText, Constants.vbCrLf + "<img ", "<img ").ToString();
		sText = regexReplace(sText, Constants.vbCrLf + "<a ", "<a ").ToString();
		sText = regexReplace(sText, Constants.vbCrLf + "</", "</").ToString();
		sText = regexReplace(sText, Constants.vbCrLf + "<!--", "<!--").ToString();
		// keep comment on same line
		sText = sText.Trim();
		File.WriteAllText(sDocumentFileName, sText);
	}
	private object insertTracking(ref string line)
	{
		string oldLine = line;
		string oldTrackingValue = null;
		try {
			// remove previous tracking if found
			oldTrackingValue = Regex.Match(line, " tracking=" + quote + "[+-]\\d.\\d\\dpt" + quote).ToString();
			oldTrackingValue = Regex.Match(oldTrackingValue, "[+-]\\d.\\d\\dpt").ToString();
			// don't store tracking="   just the value
			//   storeAction(oldTrackingValue, sCliptext)
			//  storeAction(sTracking, sCliptext)
			line = regexReplace(line, " tracking=" + quote + "[+-]\\d.\\d\\dpt" + quote, "").ToString();
            string sStartOfString = null;
            string sRestOfString = null;
			//if (Strings.InStr(1,line, "<p>")) {
            if (line.IndexOf("<p>") > 0)
            {
				line = "<p >" + str.Mid(line, 4);
			}
            int iBreakPosition = line.IndexOf(" ");
			string sTrackingToInsert = "tracking=" + quote + sTracking + quote;
			sStartOfString = str.Left(line, iBreakPosition);
			sRestOfString = str.Mid(line, iBreakPosition);
			return sStartOfString + sTrackingToInsert + sRestOfString;
		} catch (Exception ex) {
			MessageBox.Show("Error inserting tracking" + Constants.vbCrLf + ex.Message + Constants.vbCrLf + "Avoid () as they cause problems.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			return oldLine;
		}
	}
	public static string regexReplace(string sInput, string sFind, string sReplace)
	{
		try {
			// nul input allowed
			Regex expression = null;
			expression = new Regex(sFind);
			return expression.Replace(sInput, sReplace);
		} catch (Exception ex) {
			return sInput;
		}
	}

	public void convertWithPrinceAndDisplayPDF()
	{
		getValuesFromStylesheet();
		if (convertUsingPrince()) {
			displayPDF();
		} else {
			if (DebuggingModeToolStripMenuItem.Checked == true) {
				if (File.Exists(sDocumentFileName)) {
					MessageBox.Show(sDocumentFileName + " found = " + File.Exists(sDocumentFileName).ToString());
				} else if (!File.Exists(sStylesheetFileName)) {
					MessageBox.Show(sStylesheetFileName + " found = " + File.Exists(sStylesheetFileName).ToString());
				} else if (!File.Exists(sPrinceEXE)) {
					MessageBox.Show(sPrinceEXE + " found = " + File.Exists(sPrinceEXE).ToString());
				} else {
					MessageBox.Show("No PDF produced", "Sorry.", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
		}
	}
	private void btnConvert_Click(System.Object sender, System.EventArgs e)
	{
		convertWithPrinceAndDisplayPDF();
	}

	private void getLastPageNumber()
	{
		// this is done when processing -- last page number is part of stats output in stdErr
	}
	private void turnOffTracking()
	{
		TrackingPlus15MenuItem.Enabled = false;
		TrackingNoneMenuItem.Enabled = false;
		TrackingMinus10MenuItem.Enabled = false;
		TrackingMinus15MenuItem.Enabled = false;
		TrackingMinus20MenuItem.Enabled = false;
		TrackingMinus25MenuItem.Enabled = false;
		TrackingMinus30MenuItem.Enabled = false;
		TrackingMinus35MenuItem.Enabled = false;
		TrackingMinus40MenuItem.Enabled = false;
	}
	private void turnOnTracking()
	{
		TrackingPlus15MenuItem.Enabled = true;
		TrackingNoneMenuItem.Enabled = true;
		TrackingMinus10MenuItem.Enabled = true;
		TrackingMinus15MenuItem.Enabled = true;
		TrackingMinus20MenuItem.Enabled = true;
		TrackingMinus25MenuItem.Enabled = true;
		TrackingMinus30MenuItem.Enabled = true;
		TrackingMinus35MenuItem.Enabled = true;
		TrackingMinus40MenuItem.Enabled = true;
	}

	private void main_keypress(object sender, System.Windows.Forms.KeyPressEventArgs e)
	{
		if (((Control.ModifierKeys & Keys.Right) == Keys.Right)) {
			MessageBox.Show("ctrl right");
		} else if (Keys.Right == Keys.Right) {
			MessageBox.Show("right");
		}

	}

	private void btnChangePictureSettings_Click(System.Object sender, System.EventArgs e)
	{
		changePictureSettings();
	}
	private void changePictureSettings()
	{
        picture picture = new picture();
		if (sGraphicsFolder == "#none#") {
			// get graphics folder
			getGraphicFilesToDisplay();
		} else {
			picture.Show();
			picture.BringToFront();
		}

	}

	private void SettingsToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
	{
        settings settings = new settings();
		settings.getClassInfo();
		settings.BringToFront();
		settings.Show();
	}
	private void CurrentToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
	{
		if (File.Exists(sLogCurrentName)) {
			tbFeedback.Text = sPadding + File.ReadAllText(sLogCurrentName) + sPadding;
			showFeedback();
		} else {
			MessageBox.Show("Prince failed to start. Verify that input and css are valid files.", "No log file created", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
	}
	private void updateLogHistoryFile()
	{
		if (File.Exists(sLogCurrentName)) {
			if (File.Exists(sLogHistoryName)) {
				// history file has newest on top
				File.AppendAllText(sLogCurrentName, File.ReadAllText(sLogHistoryName));
				File.Delete(sLogHistoryName);
				File.Copy(sLogCurrentName, sLogHistoryName);
			} else {
				// no history yet
				File.Copy(sLogCurrentName, sLogHistoryName);
			}
			File.Delete(sLogCurrentName);
		} else {
			// skip
			//   MessageBox.Show("No history files created yet.")

		}
	}
	private void HistoryToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
	{
		tbFeedback.Text = sPadding + File.ReadAllText(sLogHistoryName) + sPadding;
		showFeedback();
	}
	private void getGraphicFilesToDisplay()
	{
		if (sGraphicsFolder == "#none#") {
		// don't set starting location
		} else {
			FolderBrowserDialog1.SelectedPath = sGraphicsFolder;
		}
		FolderBrowserDialog1.Description = "Graphics folder";
		FolderBrowserDialog1.ShowDialog();
		sGraphicsFolder = FolderBrowserDialog1.SelectedPath;
		writeIniFile();
		displayPictureMenuItems();
	}
	private void btnClose_Click(System.Object sender, System.EventArgs e)
	{
		displayPDFifExists();
	}
	private void InsertHereToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
	{
		getValuesFromStylesheet();
		// save selected text

		getCliptext();

		sSelectedText = sCliptext;
		// select a graphic to insert and turn on the insert button
		if (sSelectedText == null) {
		//skip ... warnining mess given in getClipBoardText

		} else {
            picture picture = new picture();
			picture.btnInsertGraphicHere.Visible = true;
			picture.btnOK.Visible = false;
			picture.BringToFront();
			picture.Show();

		}

	}
	private void AboutPrinceToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
	{
        AboutBox1 AboutBox1 = new AboutBox1();
		AboutBox1.Show();
		AboutBox1.BringToFront();
	}
	private void ContentsToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
	{
	}
	private void showDocumentation()
	{
		tbFeedback.Text = sPadding + File.ReadAllText(sPrincessDocumentation) + sPadding;
		showFeedback();
	}
	private void showFeedback()
	{
		tbFeedback.BringToFront();
		btnClose.BringToFront();
		btnClose.Visible = true;
	}
	private void LicenseToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
	{
		tbFeedback.Text = sPadding + "Your Prince license covers Princess also." + sPadding;
		showFeedback();
	}
	private void StylesheetToolStripMenuItem1_Click(System.Object sender, System.EventArgs e)
	{
		tbFeedback.Text = sPadding + File.ReadAllText(sStylesheetFileName) + sPadding;
		showFeedback();
	}
	private void DocumentSourceToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
	{
		makeTextCanonical();
		tbFeedback.Text = sPadding + File.ReadAllText(sDocumentFileName) + sPadding;
		showFeedback();
	}

	private void PrintToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
	{
		AxAcroPDF1.Print();
	}

	private void RemoveToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
	{
		findPictureFileNameFromSelected();
        picture picture = new picture();
		picture.btnRemove.Show();
		picture.btnOK.Hide();
		picture.Show();
	}

	private void RemoveAllToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
	{
        picture picture = new picture();
		string sBackupFileName = sDocumentFileName + "_before_pictures_deleted";
		if (File.Exists(sBackupFileName)) {
			MessageBox.Show(sBackupFileName + " exists. " + Constants.vbCrLf + "Please rename or delete.", "Backup file already exists.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		} else {
			if (MessageBox.Show("This will remove all pictures from " + sDocumentFileName + "." + Constants.vbCrLf + "Your original document is never touched. Continue to remove all pictures?", "Remove all pictures", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes) {
				File.Copy(sDocumentFileName, sBackupFileName);
				string sText = File.ReadAllText(sDocumentFileName);
				sText = regexReplace(sText, picture.sDeletePicture, "");
				File.WriteAllText(sDocumentFileName, sText);
				convertWithPrinceAndDisplayPDF();

			}
		}
	}


	private void ExitToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
	{
		if ((AxAcroPDF1 != null)) {
			AxAcroPDF1.Dispose();
			AxAcroPDF1 = null;
		}
		// you need a double shut down here in order to avoid
		// memory at 00000014 not able to read type of error
		// ordinarily close should be sufficient

		Keyboard.UnhookKeyboard();

		this.Close();

		Application.Exit();

	}

	private void DocumentInternetToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
	{
		blnFileFromInternet = true;
		sDocumentFileName = Interaction.InputBox("URL","","",50,50); //todo
		if (string.IsNullOrEmpty(sDocumentFileName)) {
		// skip processing
		} else {
			writeIniFile();

		}
	}

	public string getClipboardText()
	{
		AxAcroPDF1.BringToFront();
		//    Clipboard.Clear()
		SendKeys.SendWait("(^c)");
		try {
			sCliptext =  Clipboard.GetText();
			// tsTextBox1.Text = sCliptext
			if (sCliptext == null) {
				MessageBox.Show(sNothingSelected, "Try again", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return null;
			} else if (sCliptext.Contains("(") | sCliptext.Contains(")")) {
				MessageBox.Show("Try another selection without parens.", "Try again", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return null;
			} else {
				// MessageBox.Show("good: " + cliptext)
				return sCliptext;
			}
		} catch (Exception ex) {
			MessageBox.Show("Please copy some text to the clipboard first " + Constants.vbCrLf + ex.Message);
			return null;
		}
	}

	private void getClipboard()
	{
		Clipboard.Clear();
		SendKeys.SendWait("(^c)");
		sCliptext = Clipboard.GetText();
		// tsTextBox1.Text = sCliptext
	}
	private Image getClipboardPicture()
	{
		// Clipboard.Clear()
		SendKeys.SendWait("(^c)");
		return Clipboard.GetImage();
	}
	private void tsbtnStart_Click(System.Object sender, System.EventArgs e)
	{
		// If blnBrowserOn = True Then
		// SendKeys.SendWait("^{HOME}")
		// Else
		AxAcroPDF1.gotoFirstPage();
		// End If
		iPDFpageNumber = 1;
		displayPageNumber();
		writeIniFile();
	}
	private void tsbtnPrevious_Click(System.Object sender, System.EventArgs e)
	{
		//If blnBrowserOn = True Then
		// SendKeys.SendWait("{LEFT}")
		// Else
		AxAcroPDF1.gotoPreviousPage();
		//End If
		if (iPDFpageNumber == 1) {
		// don't back up
		} else {
			iPDFpageNumber = iPDFpageNumber - 1;
		}
		displayPageNumber();
		writeIniFile();
	}
	private void tsbtnNext_Click(System.Object sender, System.EventArgs e)
	{
		//If blnBrowserOn = True Then
		// SendKeys.SendWait("{RIGHT}")
		// Else
		AxAcroPDF1.gotoNextPage();
		//End If
		if (iPDFpageNumber == iLastPageNumber) {
		// don't advance
		} else {
			iPDFpageNumber = iPDFpageNumber + 1;
			//           getLastPageNumber()
		}
		displayPageNumber();
		writeIniFile();
	}
	private void tsbtnEnd_Click(System.Object sender, System.EventArgs e)
	{
		// If blnBrowserOn = True Then
		// SendKeys.SendWait("^{END}")
		// Else
		AxAcroPDF1.gotoLastPage();
		//  End If
		//  AxAcroPDF1.setCurrentPage(iLastPageNumber)
		iPDFpageNumber = iLastPageNumber;
		displayPageNumber();
		writeIniFile();
	}
	private void tsbtnCurrent_Click(System.Object sender, System.EventArgs e)
	{
		// If blnBrowserOn = True Then
		// SendKeys.SendWait("(^+N)" + iPDFpageNumber)
		// Else
		AxAcroPDF1.setCurrentPage(iPDFpageNumber);
		// End If
		displayPageNumber();
		writeIniFile();
	}
	private void displayPageNumber()
	{
		if (iLastPageNumber == 0) {
			tstbCurrentPageNumber.Text = iPDFpageNumber.ToString();
			tsbtnCurrent.Text = "";
		} else {
			tstbCurrentPageNumber.Text = iPDFpageNumber.ToString();
			tsbtnCurrent.Text = " / " + iLastPageNumber.ToString();
		}
	}
	private void tsbtnToggleToolbar_Click(System.Object sender, System.EventArgs e)
	{
		if (tsbtnToggleToolbar.Text == sToolbarHidden) {
			tsbtnToggleToolbar.Text = sToolbarShowing;
			AxAcroPDF1.setShowToolbar(true);
		} else {
			tsbtnToggleToolbar.Text = sToolbarHidden;
			AxAcroPDF1.setShowToolbar(false);
		}
	}


	private void tsbtnConvert_Click(System.Object sender, System.EventArgs e)
	{
		this.Cursor = Cursors.WaitCursor;
		convertWithPrinceAndDisplayPDF();
	}

	private void TrackingPlus60MenuItem_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "+0.60pt";
		processTracking();
	}
	private void TrackingPlus40MenuItem_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "+0.40pt";
		processTracking();
	}
	private void TrackingPlus35MenuItem_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "+0.35pt";
		processTracking();
	}
	private void TrackingPlus30MenuItem_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "+0.30pt";
		processTracking();
	}
	private void TrackingPlus25MenuItem_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "+0.25pt";
		processTracking();
	}
	private void TrackingPlus20MenuItem_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "+0.20pt";
		processTracking();
	}
	private void TrackingPlus15MenuItem_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "+0.15pt";
		processTracking();
	}
	private void TrackingPlus10MenuItem_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "+0.10pt";
		processTracking();
	}
	private void TrackingPlus05MenuItem_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "+0.05pt";
		processTracking();
	}
	private void TrackingNoneMenuItem_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "-0.00pt";
		processTracking();
	}
	private void TrackingMinus05MenuItem_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "-0.05pt";
		processTracking();
	}
	private void TrackingMinus10MenuItem_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "-0.10pt";
		processTracking();
	}
	private void TrackingMinus15MenuItem_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "-0.15pt";
		processTracking();
	}
	private void TrackingMinus20MenuItem_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "-0.20pt";
		processTracking();
	}
	private void TrackingMinus25MenuItem_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "-0.25pt";
		processTracking();
	}
	private void TrackingMinus30MenuItem_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "-0.30pt";
		processTracking();
	}
	private void TrackingMinus35MenuItem_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "-0.35pt";
		processTracking();
	}
	private void TrackingMinus40MenuItem_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "-0.40pt";
		processTracking();
	}

	private void displayPictureMenuItems()
	{
		if (sGraphicsFolder == "#none#" | sGraphicsFolder == null) {
			InsertHereToolStripMenuItem.Enabled = false;
			SelectToolStripMenuItem.Enabled = false;
			RemoveToolStripMenuItem.Enabled = false;
			RemoveAllToolStripMenuItem.Enabled = false;
		// you select text befoer coming here

		//       ElseIf sSelectedPictureFileName = "#none#" Then
		//            SelectToolStripMenuItem.Enabled = False
		//          RemoveToolStripMenuItem.Enabled = False
		} else {
			InsertHereToolStripMenuItem.Enabled = true;
			SelectToolStripMenuItem.Enabled = true;
			RemoveToolStripMenuItem.Enabled = true;
			RemoveAllToolStripMenuItem.Enabled = true;
		}
	}



	private bool convertUsingPrince()
	{
		//WebBrowser1.Dispose()


		int percentCompleted = 0;
		if (File.Exists(sPrinceEXE)) {
			bool pdfCreated = false;
			try {
				if (File.Exists(sPDFoutputName))
					File.Delete(sPDFoutputName);

			} catch (Exception ex) {
				MessageBox.Show("Another program has locked the output file: " + Constants.vbCrLf + sPDFoutputName + Constants.vbCrLf + "Please close the program displaying the file and start over. Princess will remember your settings.", "File locked", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				System.Environment.Exit(0);

			}

			updateLogHistoryFile();
			//    Dim sOptions As String = " --server --progress --verbose  "
			string sOptions = " --verbose --server --log-stats ";
			//--------------------------------------------------------


			string sArguments = null;

			// todo ifMultipleFilesGenerateDocumentNameWithAllFilesIncluded();


			if (string.IsNullOrEmpty(sStylesheetFileName) | sStylesheetFileName == "#none#") {
				sArguments = sOptions + " --log " + quote + sLogCurrentName + quote + " --output " + quote + sPDFoutputName + quote + " --input " + quote + sInputFormat + quote + " " + quote + sDocumentFileName + quote;
			} else {
				if (sToggleTracking == sTrackingHidden) {
					sArguments = sOptions + " --style " + quote + sStylesheetFileName + quote + " --style " + quote + sRemoveTrackingColor + quote + " --log " + quote + sLogCurrentName + quote + " --output " + quote + sPDFoutputName + quote + " --input " + quote + sInputFormat + quote + " " + quote + sDocumentFileName + quote;

				} else {
					if (blnProcessMultipleInputFiles == true) {
						sArguments = sOptions + " --style " + quote + sStylesheetFileName + quote + " --style " + quote + sShowTrackingColor + quote + " --log " + quote + sLogCurrentName + quote + " --output " + quote + sPDFoutputName + quote + " --input " + quote + sInputFormat + quote + " " + sDocumentFileName;
					} else {
						sArguments = sOptions + " --style " + quote + sStylesheetFileName + quote + " --style " + quote + sShowTrackingColor + quote + " --log " + quote + sLogCurrentName + quote + " --output " + quote + sPDFoutputName + quote + " --input " + quote + sInputFormat + quote + " " + quote + sDocumentFileName + quote;
					}

				}

			}
			// create a new process
			Process myPrince = new Process();
			string sErrorInput = "start";

			// set the file name and the command line args
			myPrince.StartInfo.FileName = sPrinceEXE;
			myPrince.StartInfo.Arguments = sArguments;

			// start the process in a hidden window
			myPrince.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
			myPrince.StartInfo.CreateNoWindow = true;

			// stdErr redirected
			myPrince.StartInfo.RedirectStandardError = true;
			myPrince.StartInfo.UseShellExecute = false;
			myPrince.Start();
			int iStart = (int)DateAndTime.Timer;
			string test = null;
			bool blnWarning = false;
			LogToolStripMenuItem.BackColor = Color.Black;

			// this part never fires
			while (!((sErrorInput == null))) {
				// While Not myPrince.HasExited
				sErrorInput = myPrince.StandardError.ReadLine();
				if (sErrorInput == null) {
					break; // TODO: might not be correct. Was : Exit Do
				}

				if (sErrorInput.StartsWith("prg|")) {
					test = sErrorInput.Substring(4);
					percentCompleted = Convert.ToInt16(test);
					bWkrPrince.ReportProgress(percentCompleted, sErrorInput);
				} else if (sErrorInput.Contains("total_page_count|")) {
					test = sErrorInput.Substring(21);
					iLastPageNumber = Convert.ToInt16(test);
				} else if (sErrorInput.Contains("fin|failure")) {
					LogToolStripMenuItem.BackColor = Color.Red;
				} else if (sErrorInput.Contains("Could not resolve host")) {
					MessageBox.Show("Please verify that you are connected to the internet and try again.", "No connection to internet found.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				} else if (sErrorInput.Contains("fin|success")) {
					if (blnWarning == true) {
						LogToolStripMenuItem.BackColor = Color.Yellow;
					} else {
						LogToolStripMenuItem.BackColor = Color.Lime;
					}
				} else if (sErrorInput.Contains("msg|wrn|")) {
					LogToolStripMenuItem.BackColor = Color.Yellow;
					blnWarning = true;
				}
			}


			// if the process doesn't complete within
			// 10 second, kill it
			// will probably need to remove this as the loop above keeps control
			myPrince.WaitForExit(10000);
			//   MessageBox.Show("waiting " + sArguments)
			int iEnd = (int)DateAndTime.Timer;
			if (myPrince.HasExited) {
			//    MessageBox.Show("Prince completed " & iEnd - iStart & " seconds.")
			} else {
				myPrince.Kill();
				// this will say 0 seconds now
				MessageBox.Show("Prince process terminated as it is taking more than " + (iEnd - iStart) + " seconds.");

			}
			pdfCreated = File.Exists(sPDFoutputName);
			// MessageBox.Show("Prince completed.")
			this.ProgressBar1.Value = 0;

			// WebBrowser1.Dispose()

			this.Cursor = Cursors.Default;
			return pdfCreated;
		} else {
			// where is prince.exe?
			locatePrinceExe();
			this.Cursor = Cursors.Default;

			return false;
		}

	}

	private void btnStart_Click(System.Object sender, System.EventArgs e)
	{
		// probably need to connect this with the Convert button
		// consider adding a Cancel putton .... or change Convert to Cancel
		// make sure to disable convert start so you can only have one instance running
		bWkrPrince.RunWorkerAsync();
	}
	private void bWkrPrince_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
	{
		try {
			if (bWkrPrince.CancellationPending)
				return;
			// process Prince
			//  pass back progress message
			convertUsingPrince();

			//          bWkrPrince.ReportProgress(0, fname)
			bWkrPrince.Dispose();

		} catch (Exception ex) {
			// MessageBox.Show(ex.Message.ToString)

		}

	}
	private void bWkrPrince_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
	{
		// changing the color of the progress bar can't be done ... but if needed we could use a text box to simulate a progress bar
		this.ProgressBar1.BringToFront();
		this.ProgressBar1.Value = e.ProgressPercentage;
	}

	private void bWkrPrince_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
	{
		// If fp.ListBox1.Items.Count > 0 Then fp.ListBox1.SelectedIndex = 0
	}

	private void btnStop_Click(System.Object sender, System.EventArgs e)
	{
		bWkrPrince.CancelAsync();
	}

	private void Button1_Click(System.Object sender, System.EventArgs e)
	{
		// this is for testing purposes
		if (blnBrowserOn == true) {
			blnBrowserOn = false;
			this.AxAcroPDF1.BringToFront();
		} else {
			// Toggle button inactivated ******************
			// WebBrowser causes crash as it is not properly removed right now
			//          blnBrowserOn = True
			//          Me.WebBrowser1.BringToFront()
			//          Me.WebBrowser1.Navigate(Me.sPDFoutputName)
		}

	}

	private void PageSetupToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
	{
		// ***************** still working on this item 
		// ****** don't know if we need all of this power or just a simple
		// one as set up in configuration settings
		// At least at first I wont't add this as it is in the CSS
		//
		// Initialize the dialog's PrinterSettings property to hold user
		// defined printer settings.
		PageSetupDialog1.PageSettings = new System.Drawing.Printing.PageSettings();

		//Show the dialog storing the result.
		DialogResult result = PageSetupDialog1.ShowDialog();

		// If the result is OK, display selected settings in
		// ListBox1. These values can be used when printing the
		// document.
		if ((result == DialogResult.OK)) {
			object[] results = new object[] {
				PageSetupDialog1.PageSettings.Margins,
				PageSetupDialog1.PageSettings.PaperSize,
				PageSetupDialog1.PageSettings.Landscape,
				PageSetupDialog1.PrinterSettings.PrinterName,
				PageSetupDialog1.PrinterSettings.PrintRange
			};
			//            ListBox1.Items.AddRange(results)
		}
		PageSetupDialog1.ShowDialog();

	}

	private void SelectToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
	{
		if (sGraphicsFolder == "#none#") {
			getGraphicFilesToDisplay();
		} else {
			//skip
		}
		if (findPictureFileNameFromSelected() == "#none#") {
			MessageBox.Show("Sorry for the error. Be sure to select a picture first." + Constants.vbCrLf + "If you have selected a picture then please report this as bug #1 showing up.", "Picture not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
			this.Cursor = Cursors.Default;
		} else {
            picture picture = new picture();
			picture.Show();
		}
	}

	private string findPictureFileNameFromSelected()
	{
		try {
			// if we can find matching image on clipboard with graphic file
			Image X = getClipboardPicture();
            Bitmap bitmapImage = new Bitmap(X);
			// this has no meaning when X is Nothing so trips error
			if (X.Width == 0) {
				return "#error#";
			} else {
                return findMatchingPicture(bitmapImage);

			}
		} catch (Exception ex) {
			this.Cursor = Cursors.Default;
			// MessageBox.Show(ex.ToString)
			return sSelectedPictureFileName;

		}
		// compare to all the other bitmaps and return file name of selected graphic
	}

    private string findMatchingPicture(Bitmap selectedPicture)
	{
		this.Cursor = Cursors.WaitCursor;
		Application.DoEvents();
		bool are_identical = true;
		Bitmap bm1 = selectedPicture;
		Bitmap bm2 = null;
		foreach (string graphic_loopVariable in Directory.GetFiles(sGraphicsFolder)) {
            string graphic = graphic_loopVariable;
			if (graphic.Contains("&")) {
				MessageBox.Show("'&' found in file name. " + Constants.vbCrLf + graphic.ToString() + Constants.vbCrLf + "Please rename file or remove from this folder." + Constants.vbCrLf + "Since we are working with xml the '&' has special meaning. Ignoring this file.", "Illegal file name.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			} else {
				bm2 = new Bitmap(Image.FromFile(graphic)); 
				int wid = Math.Min(bm1.Width, bm2.Width);
				int hgt = Math.Min(bm1.Height, bm2.Height);
				if ((bm1.Width == bm2.Width) & (bm1.Height == bm2.Height)) {
					if (areGraphicsIdentical(bm1, bm2) == true) {
						sSelectedPictureFileName = graphic;
						this.Cursor = Cursors.Default;
						return sSelectedPictureFileName;
					} else {
						are_identical = false;
						bm2.Dispose();
					}
					are_identical = false;
					bm2.Dispose();
				}
				bm2.Dispose();
				// didn't seem to help
			}

		}

		this.Cursor = Cursors.Default;
		bm1.Dispose();
		return "#error#";
	}
	private bool areGraphicsIdentical(Bitmap bm1, Bitmap bm2)
	{
		int wid = Math.Min(bm1.Width, bm2.Width);
		int hgt = Math.Min(bm1.Height, bm2.Height);
		for (int x = 0; x <= wid - 1; x++) {
			for (int y = 0; y <= hgt - 1; y++) {
				//     Dim pix1R = bm1.GetPixel(x, y).G
				//    Dim pix2R = bm2.GetPixel(x, y).R
				//   Dim diffR As Integer
				//  If pix2R > pix1R Then
				//diffR = pix2R - pix1R
				//Else
				//diffR = pix1R - pix2R
				//End If

				int pix1G = bm1.GetPixel(x, y).G;
				int pix2G = bm2.GetPixel(x, y).G;
				int diffG = 0;
				if (pix2G > pix1G) {
					diffG = pix2G - pix1G;
				} else {
					diffG = pix1G - pix2G;
				}

                int pix1B = bm1.GetPixel(x, y).B;
                int pix2B = bm2.GetPixel(x, y).B;
				int diffB = 0;
				if (pix2B > pix1B) {
					diffB = pix2B - pix1B;
				} else {
					diffB = pix1B - pix2B;
				}
				//   If diffR + diffG + diffB > 72 Then
				if (diffG + diffB > 72) {
					return false;
				} else {
					// good ... we are within 24 of each color
				}
			}
		}
		return true;
	}

	private void Button2_Click(System.Object sender, System.EventArgs e)
	{
		dgvFilesToProcess.BringToFront();
		//  DataGridView1.
	}

	private void OpenFileDialogXML_FileOk(System.Object sender, System.ComponentModel.CancelEventArgs e)
	{
        multipleFiles multipleFiles = new multipleFiles();
		// sFile = OpenFileDialogXML.FileName
		multipleFiles.lbFiles.Items.AddRange(OpenFileDialogXML.FileNames);
		//addFilesTOlbFiles()
		blnXMLfileProcessed = false;
	}
	//  Public Sub addFilesTOlbFiles()
	//Dim i As Integer
	//   For i = 0 To aFiles.Length - 1
	//      If aFiles(i) <> "" Then
	//         If verifyFileIsHTML(aFiles(i)) Then
	//            If blnFileAlreadyOpen = False Then
	//               addFile(aFiles(i))
	//          End If
	//     Else
	// ' not HTML
	//            End If
	//       End If
	//  Next i
	//  ' Popup.enableNext(True)
	// End Sub
	//  Public Sub addFile(ByVal filename As String)
	// Dim temp As Int16
	//     If InStr(filename, ".pu.") Then
	// ' skip any popup files as it is illogical to add popups to popups
	//     Else
	//         row = table.NewRow()
	// '   row("Name") = getFileNameWithoutExtensionFromFullName(filename)
	// '   row("Path") = filename
	//         row.Item("Name") = getFileNameWithoutExtensionFromFullName(filename)
	//         row.Item("Path") = filename
	//         temp = table.Rows.IndexOf(row)
	//         If table.Rows.IndexOf(row) = -1 Then
	//             Try
	//                 table.Rows.Add(row)
	//             Catch ex As Exception
	// ' can't add if not unique
	//             End Try
	//         Else
	// ' already exists so skip
	//         End If
	//     End If
	// End Sub
	//  Public Sub deleteFile(ByVal filename As String)
	// 'table.Rows.Remove(system.Data.DataRow("Path")  
	//     row("Name") = getFileNameWithoutExtensionFromFullName(filename)
	//     row("Path") = aFiles(filename)
	//     table.Rows.Add(row)
	// End Sub
	public object verifyFileIsHTML(string fileToVerify)
	{
		// see that file exists and that it starts with '<html' 
		string line1 = null;
		string line2 = null;
		string line3 = null;
		try {
			blnFileAlreadyOpen = false;
			FileSystem.FileOpen(filenum, fileToVerify, OpenMode.Input, OpenAccess.Read, OpenShare.Shared,1);
			// see if either first or second or third line of file starts with '<html' or '<HTML'
			line1 = inputLine();
			line2 = inputLine();
			line3 = inputLine();
            if (line1.IndexOf("<html") > 0 | line1.IndexOf("<HTML") > 0 | line2.IndexOf("<html") > 0 | line2.IndexOf("<HTML") > 0 | line3.IndexOf("<html") > 0 | line3.IndexOf("<HTML") > 0)
            {
                //(str.InStr(line1, "<html") > 0 | str.InStr(line1, "<HTML") > 0 | str.InStr(line2, "<html") > 0 | str.InStr(line2, "<HTML") > 0 | str.InStr(line3, "<html") > 0 | str.InStr(line3, "<HTML") > 0) {
				FileSystem.FileClose(filenum);
				return true;
			}
			FileSystem.FileClose(filenum);
		} catch (Exception ex) {
			// file is open in another program
			// please close file in other program and try again
			FileSystem.FileClose(filenum);
			blnFileAlreadyOpen = true;
			return true;
		}
		return false;
	}
	//  Public Sub makeDataTable()
	//
	//    ' Create new DataColumn, set DataType, ColumnName 
	//    ' and add to DataTable.    
	//        column = New DataColumn()
	//
	//        column.DataType = System.Type.GetType("System.String")
	//        column.ColumnName = "Name"
	//        column.ReadOnly = True
	//        column.Unique = False
	//
	//    ' Add the Column to the DataColumnCollection.
	//        table.Columns.Add(column)
	//    ' Create second column.
	//        column = New DataColumn()
	//        column.DataType = System.Type.GetType("System.String")
	//        column.ColumnName = "Path"
	//        column.AutoIncrement = False
	//        column.Caption = "Path"
	//        column.ReadOnly = False
	//        column.Unique = False
	//
	//    ' Add the column to the table.
	//        table.Columns.Add(column)
	//
	//    ' Make the ID column the primary key column.
	//    Dim PrimaryKeyColumns(0) As DataColumn
	//   ' PrimaryKeyColumns(0) = table.Columns("Path")
	//       PrimaryKeyColumns(0) = table.Columns("Name")
	//       table.PrimaryKey = PrimaryKeyColumns
	//
	//    ' Instantiate the DataSet variable.
	//        dataSet = New DataSet()
	//
	//    ' Add the new DataTable to the DataSet.
	//        dataSet.Tables.Add(table)
	//        dgvFilesToProcess.DataSource = table
	//    End Sub
	private void tstbCurrentPageNumber_Click(System.Object sender, System.EventArgs e)
	{
		if (tstbCurrentPageNumber.Text == null) {
		// skip
		} else {
			iPDFpageNumber = int.Parse(tstbCurrentPageNumber.Text);
		}
		displayPDF();
	}
	private void getDocumentFilesToProcess()
	{
        multipleFiles multipleFiles = new multipleFiles();
		multipleFiles.Show();
	}
	//    Private Sub displayFilesInFolder()
	//        aFiles = Directory.GetFiles(sDocumentsFolder)
	//        addFilesTOlbFiles()
	//        blnXMLfileProcessed = False
	//        dgvFilesToProcess.Show()
	//        dgvFilesToProcess.BringToFront()
	//    End Sub

	private void DocumentMultipleToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
	{
		blnProcessMultipleInputFiles = true;
		blnProcessURL = false;
		getDocumentFilesToProcess();
		// sInputFileName = OpenFileDialog1.FileName

		makeFileNamesFromInputFileName();
		writeIniFile();
		// startOverOrContinue()
		displayInfo();
		turnOnOffEditing();

		//      xxx()
	}

	private void PDFDisplayToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
	{
        PDFdisplay PDFdisplay = new PDFdisplay();
		PDFdisplay.BringToFront();
		PDFdisplay.Show();

	}


	private void DebuggingModeToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
	{
		if (DebuggingModeToolStripMenuItem.Checked == true) {
			DebuggingModeToolStripMenuItem.Checked = false;
		} else {
			DebuggingModeToolStripMenuItem.Checked = true;
		}
	}


	private void dgvFilesToProcess_CellContentClick(System.Object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
	{
	}

	private void MoveToBottomToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
	{
		//        picture.rbBottom.Checked = True
        picture picture = new picture();
		picture.btnOKprocess();
	}

	private void GPSStylesheetToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
	{
		OpenFileDialog1.FileName = "";
		OpenFileDialog1.Filter = "GPS Stylesheet files (*.isty)|*.isty;|All files (*.*)|*.*";
		this.OpenFileDialog1.Multiselect = false;

		OpenFileDialog1.ShowDialog();
		sGPSStylesheetFileName = OpenFileDialog1.FileName;
		sGPSStylesheetName = getFileNameWithExtensionFromFullName(sStylesheetFileName);
		CSS();
		sStylesheetFileName = sGeneratedStylesheetFileName;
		writeIniFile();
		displayInfo();
	}
	private void CSS()
	{
        createCSS createcss = new createCSS();
        createcss.Show();
	}

	private void GPSStylesheetToolStripMenuItem1_Click(System.Object sender, System.EventArgs e)
	{
		tbFeedback.Text = sPadding + File.ReadAllText(sGPSStylesheetFileName) + sPadding;
		showFeedback();
	}

	private void GeneratedStylesheetToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
	{
		viewGeneratedStylesheet();

	}
	public void viewGeneratedStylesheet()
	{
		tbFeedback.Text = sGeneratedStylesheetFileName + sPadding + File.ReadAllText(sGeneratedStylesheetFileName) + sPadding;
		showFeedback();

	}

	// Private Sub UndoToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UndoToolStripButton.Click
	//    If getUndoAction() Then
	//       showTest()
	//      sCliptext = sSelectedItem
	//     sTracking = sAction
	//    makeTextCanonical()
	//    ' sTracking used with sCliptext to do the work
	//'           If blnRedo = True Then
	//             iActionNumber -= 1
	//        End If
	//       blnUndo = True
	//      blnRedo = False
	//
	//       showTest()
	//      findTextInSourceAndApplyTracking(sCliptext) ' removed condition here as it already worked
	//     convertWithPrinceAndDisplayPDF()
	//    showUndoRedo()
	//   showTest()
	//      Else
	//         Beep()
	//    End If

	// End Sub

	//    Public Sub storeAction(ByRef sAction, ByRef sSelectedItem)
	//' tracking amount  and selected text
	//
	//   If blnUndo = True Then
	//
	//   ElseIf blnRedo = True Then
	//
	//   Else
	//      If arrayActions(iActionNumber, iAction) = sAction And arrayActions(iActionNumber, iSelectedItem) = sSelectedItem Then
	//' skip
	//       Else
	//          moveActionsUp()
	//         showTest()
	//        If sAction = "" Then sAction = "-0.00pt"
	//       showTest()
	//      arrayActions(iActionNumber, iAction) = sAction
	//             arrayActions(iActionNumber, iSelectedItem) = sSelectedItem
	//            iLastActionNumber = Math.Max(iActionNumber, iLastActionNumber)
	//           showTest()
	//      End If
	// End If
	//   End Sub

	//  Public Function getUndoAction()
	//' tracking amount  and selected text
	//   If iActionNumber > 0 Then
	//      showTest()
	//
	//       sAction = arrayActions(iActionNumber, iAction)
	//      sSelectedItem = arrayActions(iActionNumber, iSelectedItem)
	//     iActionNumber -= 1
	//    showTest()
	//
	//       Return True

	//  Else
	//     Return False

	//  End If
	// End Function

	// Private Sub RedoToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RedoToolStripButton.Click
	//    If getRedoAction() Then
	//       showTest()
	//      sCliptext = sSelectedItem
	//     sTracking = sAction
	//    makeTextCanonical()
	// ' sTracking used with sCliptext to do the work
	//        If blnUndo = True Then
	//           iActionNumber += 1
	//      End If
	//        blnRedo = True
	//         blnUndo = False
	//
	//           findTextInSourceAndApplyTracking(sCliptext) ' removed condition here as it already worked
	//          convertWithPrinceAndDisplayPDF()
	//         showUndoRedo()
	//        showTest()
	//   Else
	//      Beep()
	// End If
	// End Sub

	//  Private Function getRedoAction()
	//' tracking amount  and selected text
	//     If iActionNumber > iLastActionNumber Then
	//        Return False
	//
	//   Else
	//      iActionNumber += 1
	//     sAction = arrayActions(iActionNumber, iAction)
	//    sSelectedItem = arrayActions(iActionNumber, iSelectedItem)
	//   showTest()
	//        Return True
	//   End If
	// End Function
	// Private Sub resetActions()
	//    iActionNumber = 0
	//   iLastActionNumber = 0
	//'  arrayActions.
	//    showUndoRedo()
	//    showTest()
	// End Sub

	//  Private Sub showUndoRedo()
	//      If iActionNumber > 0 Then
	//         UndoToolStripButton.Enabled = True
	//    Else
	//       UndoToolStripButton.Enabled = False
	//  End If
	//    If iActionNumber < iLastActionNumber Then
	//       RedoToolStripButton.Enabled = True
	//  Else
	//     RedoToolStripButton.Enabled = False
	//   End If

	//  End Sub
	//  Private Sub showTest()
	//     TextBox1.Text = iActionNumber
	//    TextBox2.Text = sAction
	//   TextBox3.Text = arrayActions(1, iAction) + " 1 " + _
	//  arrayActions(2, iAction) + " 2 " + _
	// arrayActions(3, iAction) + " 3 " + _
	//arrayActions(4, iAction) + " 4 " + _
	//   arrayActions(5, iAction) + " 5 " + _
	//  arrayActions(6, iAction) + " 6 "
	// TextBox3.BringToFront()

	//    End Sub
	//   Private Sub moveActionsUp()
	//      For x = iLastActionNumber To iActionNumber
	//         arrayActions(x + 1, iAction) = arrayActions(x, iAction)
	//        arrayActions(x + 1, iSelectedItem) = arrayActions(x, iSelectedItem)
	//   Next
	//  iLastActionNumber = iLastActionNumber + 1
	//
	// End Sub

	private void btnAlternateSelector_Click(System.Object sender, System.EventArgs e)
	{
		if (blnAlternateSelector == true) {
			hideAlternateSelector();
			blnAlternateSelector = false;
		} else {
			showAlternateSelector();
			blnAlternateSelector = true;
		}

	}
	private void showAlternateSelector()
	{
		makeTextCanonical();
		this.Cursor = Cursors.WaitCursor;
		this.Width = 1000;
		TreeView1.Show();
		LoadTreeViewFromXmlFile();
		//TreeView1.Font = "Scheherazade"
		this.Cursor = Cursors.Default;

	}
	private void hideAlternateSelector()
	{
		this.Width = 805;
		TreeView1.Hide();
		this.Cursor = Cursors.Default;

	}
	public void getCliptext()
	{
		if (blnTreeViewSelected == true) {
			sCliptext = TreeView1.SelectedNode.Text;
		} else {
			sCliptext = getClipboardText();
		}
	}

	private void processTracking()
	{
		makeTextCanonical();
		getCliptext();
		if (sCliptext == null) {
		// MessageBox.Show("sCliptext = Nothing") ' skip
		} else {
			sCliptext = Regex.Replace(sCliptext, Constants.vbCrLf, " ");
			// MessageBox.Show(tmp)
			if (findTextInSourceAndApplyTracking(sCliptext) == true) {
				convertWithPrinceAndDisplayPDF();
			} else {
				//  MessageBox.Show("find text False") ' skip
				// nothing changed so don't process
			}
			//        showUndoRedo()

		}
	}
	private bool findTextInSourceAndApplyTracking(string tmp)
	{
		try {
			if (isSelectedTextUnique(tmp) == true) {
				// look at document line by line and find match
				string line = "";
				string sText = "";
				StreamReader sr = new StreamReader(sDocumentFileName);
				while (!(sr.EndOfStream)) {
					line = sr.ReadLine();
					// sCliptext may have been modified by isSelectedTextUnique to find Unique
					if (line.Contains(sCliptext)) {
						// found matching line
						// insert tracking
						sText = sText + insertTracking(ref line);
					} else {
						sText = sText + line;
					}
				}
				sr.Close();
				File.WriteAllText(sDocumentFileName, sText);
				return true;
			} else {
				// skip as no unique match found
				// messages handled in isSelectedTextUnique
				return false;
			}
			//return "unknown";
		} catch (Exception ex) {
			MessageBox.Show("Error, but will try to continue." + Constants.vbCrLf + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
			return false;
		}
	}
	public bool isSelectedTextUnique(string tmp)
	{
		string sTextFromFile = File.ReadAllText(sDocumentFileName);
		int iCountOfMatchingStrings = 0;
		// check to see that string is unique

		if (blnTreeViewSelected == true) {
			tbAlternateSelector.Text = sTextFromFile;
			//   Dim test As String = Encoding.UTF8.Convert(tbAlternateSelector.Text)
			iCountOfMatchingStrings = Regex.Matches(tbAlternateSelector.Text, tmp).Count;

		} else {
			iCountOfMatchingStrings = Regex.Matches(sTextFromFile, tmp).Count;

		}

		int iStringLength = 0;
		if (iCountOfMatchingStrings == 0) {
			//while (Strings.InStr(sCliptext, " ") > 0) {
            while (sCliptext.IndexOf(" ") > 0) {
				// try removing first word which may really be a span like verse number
				sCliptext = str.Mid(sCliptext, str.InStr(sCliptext, " ",CompareMethod.Text) + 1);
				iCountOfMatchingStrings = Regex.Matches(sTextFromFile, sCliptext).Count;
				if (iCountOfMatchingStrings == 1)
					return true;
				// try removing last word which may really be a span like footnote
				iStringLength = str.Len(sCliptext);
				if (iStringLength > 10) {
					sCliptext = str.Left(sCliptext, iStringLength - 5);
					iCountOfMatchingStrings = Regex.Matches(sTextFromFile, sCliptext).Count;
					if (iCountOfMatchingStrings == 1)
						return true;
				} else {
					//skip
				}
			}
			// give up
			MessageBox.Show(quote + tmp + quote + Constants.vbCrLf + sNothingMatched, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
		} else if (iCountOfMatchingStrings > 1) {
			MessageBox.Show(quote + tmp + quote + Constants.vbCrLf + sSelectedTextMatchNotUnique, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
		} else {
			return true;
		}
		return false;
	}

	// Load a TreeView control from an XML file.
	private void LoadTreeViewFromXmlFile()
	{
		try {
			// Load the XML document.
			XmlDocument xml_doc = new XmlDocument();
			xml_doc.Load(sDocumentFileName);

			// Add the root node's children to the TreeView.
			TreeView1.Nodes.Clear();
			AddTreeViewChildNodes(TreeView1.Nodes, xml_doc.DocumentElement);
			TreeView1.ExpandAll();

		} catch (Exception ex) {
			MessageBox.Show("Error opening file " + Constants.vbCrLf + ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}
	}
	// Add the children of this XML node 
	// to this child nodes collection.
	private void AddTreeViewChildNodes(TreeNodeCollection parent_nodes, XmlNode xml_node)
	{
		foreach (XmlNode child_node in xml_node.ChildNodes) {
			// Make the new TreeView node.
			if (child_node.Name == "span") {
			// skip
			} else if (child_node.Name == "#text") {
			// skip
			// removes head
			} else if (child_node.Name == "head") {
			// skip

			} else {
				TreeNode new_node = parent_nodes.Add(child_node.Name);
				// parent_nodes.Add(child_node.Value )
				// Recursively make this node's descendants.
				AddTreeViewChildNodes(new_node.Nodes, child_node);
				if (string.IsNullOrEmpty(child_node.Value)) {
				// skip
				} else {
					if (new_node.Nodes.Count == 0) {
						new_node.Text = child_node.Value.Replace("  ", " "); //Strings.Replace(child_node.Value, "  ", " ");
					}
				}
				// If this is a leaf node, make sure it's visible.
			}


			//            new_node.EnsureVisible()
		}
	}

	private void TreeView1_AfterSelect(System.Object sender, System.Windows.Forms.TreeViewEventArgs e)
	{
		blnTreeViewSelected = true;
	}


	private void FullWidthToolStripButton_Click(System.Object sender, System.EventArgs e)
	{
		AxAcroPDF1.setZoom(100);

	}
	private void getWidthAndHeightFromStylesheet()
	{
		if (File.Exists(sStylesheetFileName)) {
			try {
				FileSystem.FileOpen(filenum, sStylesheetFileName, OpenMode.Input, OpenAccess.Read,OpenShare.Default,1);
				string temp = null;
				do {
					temp = Strings.Trim(inputLine());


				} while (!(temp.Contains("size:") & temp.Contains("portrait")));
				// Or EOF(filenum)
				string[] arrayTemp = null;
				arrayTemp = temp.Split(' ');
				sPageWidth = arrayTemp[1];
				sPageHeight = arrayTemp[2];
				sPageOrientation = arrayTemp[3];

				dPageWidthInPoints = 72 * int.Parse(sPageWidth.Replace("in", ""));
				dPageHeightInPoints = 72 * int.Parse(sPageHeight.Replace("in", "")); 



				FileSystem.FileClose(filenum);
			} catch (Exception ex) {
				// corrupt file - write blank
				if (DebuggingModeToolStripMenuItem.Checked) {
					MessageBox.Show(ex.Message + Constants.vbCrLf + "Couldn't find height and width in stylesheet", "Stylesheet read error", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				FileSystem.FileClose(filenum);
				writeIniFile();
				// if reading file
			}
		} else {
			// skip
		}
	}
	public void getValuesFromStylesheet()
	{
		if (File.Exists(sStylesheetFileName)) {
			// only interested in @page and div
			string temp = null;
			try {
				FileSystem.FileOpen(filenum, sStylesheetFileName, OpenMode.Input, OpenAccess.Read,OpenShare.Default,1);
				do {
					temp = Strings.Trim(inputLine());
					// remove spaces
					if (temp.StartsWith("//") | temp.StartsWith("/*")) {
					// skip
					} else {
						if (temp.Contains("@page") | temp.Contains("div.")) {
							getPageAndDivInfo();
						} else {
							// not interesteed
						}
					}
				} while (!(FileSystem.EOF(filenum)));
				// stop at end of file
				calculateColumnSize();
				FileSystem.FileClose(filenum);
				writeIniFile();
			} catch (Exception ex) {
				// corrupt file - write blank
				if (DebuggingModeToolStripMenuItem.Checked) {
					MessageBox.Show(ex.Message + Constants.vbCrLf + "Couldn't find something in stylesheet", "Stylesheet read error", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				FileSystem.FileClose(filenum);
				writeIniFile();
				// if reading con
			}
		} else {
			// skip
		}
	}
	private void getPageAndDivInfo()
	{
		string temp = null;
		string[] arrayTemp = null;
		do {
			temp = Strings.Trim(inputLine());
			// remove spaces
			temp = temp.Replace(Constants.vbTab, "");
			// remove tabs
			temp = Strings.Trim(temp).Replace(";", "");
			// remove trailing semicolon
			temp = Strings.Trim(temp);
			// remove spaces
			if (temp == null) {
			// skip
			} else if (temp.Contains("//")) {
			// skip
			} else if (temp.Contains("/*")) {
				if (temp.Contains("*/")) {
				//skip
				} else {
					while (!(temp.Contains("*/"))) {
						temp = Strings.Trim(inputLine());
						// skip
					}
				}
			} else if (temp.Contains("portrait") | temp.Contains("landscape")) {
				arrayTemp = temp.Split(' ');
				sPageWidth = arrayTemp[1];
				sPageHeight = arrayTemp[2];
				sPageOrientation = arrayTemp[3];
				dPageWidthInPoints = decimal.Parse(convertToPoints(sPageWidth).ToString()); // TODO 612
				dPageHeightInPoints = decimal.Parse(convertToPoints(sPageHeight).ToString());  // TODO 792
			// use the last set of columns
			} else if (temp.Contains("margin-top") | temp.Contains("margin-top")) {
				arrayTemp = temp.Split(' ');
				dMarginTopInPoints = decimal.Parse(convertToPoints(arrayTemp[1]).ToString());
			// use the last set of columns
			} else if (temp.Contains("margin-bottom") | temp.Contains("margin-bottom")) {
				arrayTemp = temp.Split(' ');
				dMarginBottomInPoints = decimal.Parse(convertToPoints(arrayTemp[1]).ToString());
			// use the last set of columns
			} else if (temp.Contains("margin-inside") | temp.Contains("margin-left")) {
				arrayTemp = temp.Split(' ');
				dMarginLeftInPoints = decimal.Parse(convertToPoints(arrayTemp[1]).ToString());
			} else if (temp.Contains("margin-outside") | temp.Contains("margin-right")) {
				arrayTemp = temp.Split(' ');
				dMarginRightInPoints = decimal.Parse(convertToPoints(arrayTemp[1]).ToString());
			} else if (temp.Contains("margin") | temp.Contains("margin-right")) {
				arrayTemp = temp.Split(' ');
				dMarginTopInPoints = decimal.Parse(convertToPoints(arrayTemp[1]).ToString());
				try {
					string x = arrayTemp[2];
					dMarginRightInPoints = decimal.Parse(convertToPoints(arrayTemp[2]).ToString());
					try {
						string y = arrayTemp[3];
						dMarginBottomInPoints = decimal.Parse(convertToPoints(arrayTemp[3]).ToString());
						try {
							string z = arrayTemp[4];
							dMarginLeftInPoints = decimal.Parse(convertToPoints(arrayTemp[4]).ToString());
						} catch (Exception ex) {
							dMarginLeftInPoints = dMarginBottomInPoints;
						}
					} catch (Exception ex) {
						dMarginBottomInPoints = dMarginRightInPoints;
						dMarginLeftInPoints = dMarginRightInPoints;
					}
				} catch (Exception ex) {
					dMarginRightInPoints = dMarginTopInPoints;
					dMarginBottomInPoints = dMarginTopInPoints;
					dMarginLeftInPoints = dMarginTopInPoints;
				}

			// use the last set of columns
			} else if (temp.Contains("columns:")) {
				arrayTemp = temp.Split(' ');
				iNumberOfColumns = int.Parse(convertToPoints(arrayTemp[4]).ToString());
			} else if (temp.Contains("column-gap:")) {
				arrayTemp = temp.Split(' ');
				dColumnGapInPoints = int.Parse(convertToPoints(arrayTemp[1]).ToString());
			} else if (temp.Contains("line-height:")) {
				arrayTemp = temp.Split(' ');
				dLineHeightInPoints = int.Parse(convertToPoints(arrayTemp[1]).ToString());
			}
		} while (!(temp.Contains("}")));
		// stop at end of div or page



	}

    private decimal convertToPoints(string sInput, double defaultValue)
    {
        decimal defValue = (decimal) defaultValue;
        if (sInput.Contains("pc"))
        {
            return 12 * decimal.Parse(sInput.Replace("pc", ""));
        }
        else if (sInput.Contains("px"))
        {
            return (decimal)0.75 * decimal.Parse(sInput.Replace("px", ""));
        }
        else if (sInput.Contains("pt"))
        {
            return 1 * decimal.Parse(sInput.Replace("pt", ""));
        }
        else if (sInput.Contains("mm"))
        {
            return (decimal)2.834645669 * decimal.Parse(sInput.Replace("mm", ""));
        }
        else if (sInput.Contains("cm"))
        {
            return (decimal)28.34645669 * decimal.Parse(sInput.Replace("cm", ""));
        }
        else if (sInput.Contains("in"))
        {
            return 72 * decimal.Parse(sInput.Replace("in", ""));
        }
        else if (sInput == "0")
        {
            return 0;
        }
        else if (sInput.Contains("em"))
        {
            MessageBox.Show("EM not allowed for margins. Please use pt, pc, mm, cm, or in. Making a guess.",
                            "Unit not allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            sInput = sInput.Replace("em", "");
            return decimal.Parse(sInput) * dLineHeightInPoints;
        }

        return defValue;
    }

    private decimal convertToPoints(string sInput)
    {
        convertToPoints(sInput, 0);
        //if (sInput.Contains("pc"))
        //{
        //    return 12 * Strings.Replace(sInput, "pc", "");
        //}
        //else if (sInput.Contains("px"))
        //{
        //    return 0.75 * Strings.Replace(sInput, "px", "");
        //}
        //else if (sInput.Contains("pt"))
        //{
        //    return 1 * Strings.Replace(sInput, "pt", "");
        //}
        //else if (sInput.Contains("mm"))
        //{
        //    return 2.834645669 * Strings.Replace(sInput, "mm", "");
        //}
        //else if (sInput.Contains("cm"))
        //{
        //    return 28.34645669 * Strings.Replace(sInput, "cm", "");
        //}
        //else if (sInput.Contains("in"))
        //{
        //    return 72 * Strings.Replace(sInput, "in", "");
        //}
        //else if (sInput == "0")
        //{
        //    return 0;
        //}
        //else if (sInput.Contains("em"))
        //{
        //    MessageBox.Show("EM not allowed for margins. Please use pt, pc, mm, cm, or in. Making a guess.",
        //                    "Unit not allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        //    sInput = sInput.Replace("em", "");
        //    sInput = int.Parse(sInput) * dLineHeightInPoints;
        //    return sInput;
        //}
        ////} else if (sInput.Contains("none")) {
        ////    return 0;


        ////}


        //MessageBox.Show("Stylesheet error while trying to convert " + Constants.vbCrLf + sInput + Constants.vbCrLf + "to points." + Constants.vbCrLf + "Perhaps the units are missing or incorrectly abbreviated." + Constants.vbCrLf + "Pictures will be affected." + Constants.vbCrLf + "Processing will continue.", "Conversion error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return 320; // 

    }
	private void calculateColumnSize()
	{
		dColumnWidthInPoints = dPageWidthInPoints - dMarginLeftInPoints - dMarginRightInPoints;

		if (iNumberOfColumns > 1) {
			dColumnWidthInPoints = (dColumnWidthInPoints - dColumnGapInPoints) / iNumberOfColumns;
		}


	}

	private void ifMultipleFilesGenerateDocumentNameWithAllFilesIncluded()
	{
		if (blnProcessURL == true) {
			sDocumentFileName = sURLinput;


		} else {

			if (blnProcessMultipleInputFiles == true) {
				string tempPath = sDocumentsFolder.Replace("\\", "/"); //str.Replace(sDocumentsFolder, "\\", "/");
				sDocumentFileName = "";
				// change document file name to include all documents
				// error
                multipleFiles multipleFiles = new multipleFiles();
				if (multipleFiles.verifyCopyExists() == true) {
					foreach (string filename_loopVariable in sMultipleFileNames) {
						string filename = filename_loopVariable;
						if (filename == null) {
						//skip
						} else {
							sDocumentFileName += " " + quote + tempPath + "/" + filename + ".copy" + quote;

						}
					}
					// remove first and last " mark
					sDocumentFileName = str.Mid(sDocumentFileName, 1);
					sDocumentFileName = str.Mid(sDocumentFileName, 1, str.Len(sDocumentFileName) - 1);
				//            sDocumentFileName = str.Replace(sDocumentFileName, "\", "/")
				} else {
					// skip
				}

			} else {
				// skip
			}
		}

	}

	private void DocumentURLToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
	{
		pnlURL.Show();
		pnlURL.BringToFront();

	}

	private void OK_Button_Click(System.Object sender, System.EventArgs e)
	{
		sDocumentFileName = tbURL.Text;
		sURLinput = tbURL.Text;
		blnProcessMultipleInputFiles = false;
		blnProcessURL = true;
		turnOnOffEditing();
		sInputFileName = str.Mid(sURLinput, 7).Replace("/", "");
		makeFileNamesFromInputFileName();
		writeIniFile();
		displayInfo();
		convertWithPrinceAndDisplayPDF();
		pnlURL.Hide();

	}

	private void Cancel_Button_Click(System.Object sender, System.EventArgs e)
	{
		pnlURL.Hide();
		blnProcessMultipleInputFiles = false;
		blnProcessURL = false;

	}

	public void goRight()
	{
		//  If Me.Focused = True Then
		if (iPDFpageNumber == iLastPageNumber) {
			// don't advance
			Interaction.Beep();
		} else {
			iPDFpageNumber = iPDFpageNumber + 1;
			//           getLastPageNumber()
		}
		//        displayPDF()
		displayPageNumber();
		AxAcroPDF1.gotoNextPage();

		// 2010 displayPDF()
		// writeIniFile()
		//Else
		//End If

	}
	public void goLeft()
	{
		// If Me.Focused = True Then
		if (iPDFpageNumber == 1) {
			// don't back up
			Interaction.Beep();
		} else {
			iPDFpageNumber = iPDFpageNumber - 1;
		}
		displayPageNumber();
		AxAcroPDF1.gotoPreviousPage();
		// displayPDF()
		// writeIniFile()
		//Else
		// not having focus so skip
		// End If

	}

	public void decreaseTrackingAndProcess()
	{
		switch (sTracking) {
			case "+0.60pt":
				sTracking = "+0.40pt";
				break;
			case "+0.40pt":
				sTracking = "+0.35pt";
				break;
			case "+0.35pt":
				sTracking = "+0.30pt";
				break;
			case "+0.30pt":
				sTracking = "+0.25pt";
				break;
			case "+0.25pt":
				sTracking = "+0.20pt";
				break;
			case "+0.20pt":
				sTracking = "+0.15pt";
				break;
			case "+0.15pt":
				sTracking = "+0.10pt";
				break;
			case "+0.10pt":
				sTracking = "+0.05pt";
				break;
			case "+0.05pt":
				sTracking = "+0.00pt";
				break;
			case "+0.00pt":
				sTracking = "-0.05pt";
				break;
			case "-0.00pt":
				sTracking = "-0.05pt";
				break;
			case "-0.05pt":
				sTracking = "-0.10pt";
				break;
			case "-0.10pt":
				sTracking = "-0.15pt";
				break;
			case "-0.15pt":
				sTracking = "-0.20pt";
				break;
			case "-0.20pt":
				sTracking = "-0.25pt";
				break;
			case "-0.25pt":
				sTracking = "-0.30pt";
				break;
			case "-0.30pt":
				sTracking = "-0.35pt";
				break;
			case "-0.35pt":
				sTracking = "-0.40pt";
				break;
			case "-0.40pt":
				sTracking = "-0.40pt";
				Interaction.Beep();

				break;



		}
		processTracking();
	}
	public void increaseTrackingAndProcess()
	{
		switch (sTracking) {
			case "+0.60pt":
				sTracking = "+0.60pt";
				Interaction.Beep();
				break;
			case "+0.40pt":
				sTracking = "+0.60pt";
				break;
			case "+0.35pt":
				sTracking = "+0.40pt";
				break;
			case "+0.30pt":
				sTracking = "+0.35pt";
				break;
			case "+0.25pt":
				sTracking = "+0.30pt";
				break;
			case "+0.20pt":
				sTracking = "+0.25pt";
				break;
			case "+0.15pt":
				sTracking = "+0.20pt";
				break;
			case "+0.10pt":
				sTracking = "+0.15pt";
				break;
			case "+0.05pt":
				sTracking = "+0.10pt";
				break;
			case "+0.00pt":
				sTracking = "+0.05pt";
				break;
			case "-0.00pt":
				sTracking = "+0.05pt";
				break;
			case "-0.05pt":
				sTracking = "-0.00pt";
				break;
			case "-0.10pt":
				sTracking = "-0.05pt";
				break;
			case "-0.15pt":
				sTracking = "-0.10pt";
				break;
			case "-0.20pt":
				sTracking = "-0.15pt";
				break;
			case "-0.25pt":
				sTracking = "-0.20pt";
				break;
			case "-0.30pt":
				sTracking = "-0.25pt";
				break;
			case "-0.35pt":
				sTracking = "-0.30pt";
				break;
			case "-0.40pt":
				sTracking = "-0.35pt";

				break;
		}
		processTracking();

	}

	public void goHome()
	{
		iPDFpageNumber = 1;
		//       SendKeys.SendWait("^{HOME}")
		displayPageNumber();
		displayPDF();
		// writeIniFile()

	}
	public void goEnd()
	{
		iPDFpageNumber = iLastPageNumber;
		//        SendKeys.SendWait("^{END}")
		displayPageNumber();
		displayPDF();
		// writeIniFile()

	}

	private void plus05_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "+0.05pt";
		processTracking();
	}
	private void plus10_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "+0.10pt";
		processTracking();
	}
	private void plus15_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "+0.15pt";
		processTracking();
	}
	private void plus20_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "+0.20pt";
		processTracking();
	}
	private void plus25_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "+0.25pt";
		processTracking();
	}
	private void plus30_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "+0.30pt";
		processTracking();
	}
	private void plus35_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "+0.35pt";
		processTracking();
	}
	private void plus40_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "+0.40pt";
		processTracking();
	}
	private void plus60_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "+0.60pt";
		processTracking();
	}
	private void none_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "+0.00pt";
		processTracking();
	}
	private void minus05_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "-0.05pt";
		processTracking();
	}
	private void minus10_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "-0.10pt";
		processTracking();
	}
	private void minus15_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "-0.15pt";
		processTracking();
	}
	private void minus20_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "-0.20pt";
		processTracking();
	}
	private void minus25_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "-0.25pt";
		processTracking();
	}
	private void minus30_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "-0.30pt";
		processTracking();
	}
	private void minus35_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "-0.35pt";
		processTracking();
	}
	private void minus40_Click(System.Object sender, System.EventArgs e)
	{
		sTracking = "-0.40pt";
		processTracking();
	}

	private void trackingMinus05_Click(System.Object sender, System.EventArgs e)
	{
		decreaseTrackingAndProcess();

	}
	private void trackingPlus05_Click(System.Object sender, System.EventArgs e)
	{
		increaseTrackingAndProcess();
	}

	//    Private Sub tsbtnToggleTracking_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtnToggleTracking.Click
	//        If tsbtnToggleTracking.Text = sTrackingHidden Then
	//            tsbtnToggleTracking.Text = sTrackingShowing
	//    ' toggle toolbar
	//            SendKeys.SendWait("(F8)")
	//
	//        Else
	//            tsbtnToggleTracking.Text = sTrackingHidden
	//        End If
	//        convertWithPrinceAndDisplayPDF()
	//
	//    End Sub

	private void cbColorizeTracking_CheckedChanged(System.Object sender, System.EventArgs e)
	{
		if (cbColorizeTracking.Checked == true) {
			sToggleTracking = sTrackingShowing;
		// toggle toolbar
		//  SendKeys.SendWait("(F8)")

		} else {
			sToggleTracking = sTrackingHidden;
		}
		convertWithPrinceAndDisplayPDF();

	}

    private void SettingsToolStripMenuItem_Click_1(object sender, EventArgs e)
    {

    }

	//   Public Sub UpdatePageNumber()
	//       iPDFpageNumber = GetPageNumber(AxAcroPDF1.Handle).ToString()
	//   End Sub

	//    Public Function GetPageNumber(ByRef hwnd)
	// Dim item, page, child, s
	//     For Each item In Win32Helper.GetChildWindows(hwnd)
	// '// get the edit control with the parent named "AVToolBarView" that does not end with %
	//         If ("AVToolBarView" = Win32Helper.GetWindowText(item)) Then
	//             For Each child In Win32Helper.GetChildWindows(item)
	//                 s = Win32Helper.GetWindowText(child)
	// ' if end of string s is not %
	//                 If str.Right(s, 1) <> "%" Then
	// ' page number found here
	//                     page = s
	//                     Return page
	//                 End If
	//             Next
	//         End If
	//     Next
	//     Return 0 ' didn't find a page number
	// End Function
	//End Class



	//Public NotInheritable Class Win32Helper'
	//<System.Runtime.InteropServices.DllImport("user32.dll", _
	//EntryPoint:="SetForegroundWindow", _
	//CallingConvention:=Runtime.InteropServices.CallingConvention.StdCall, _
	//CharSet:=Runtime.InteropServices.CharSet.Unicode, SetLastError:=True)> _
	//Public Shared Function _
	//     SetForegroundWindow(ByVal handle As IntPtr) As Boolean
	//' Leave function empty

	//End Function

	//    <System.Runtime.InteropServices.DllImport("user32.dll", _
	//    EntryPoint:="ShowWindow", _
	//    CallingConvention:=Runtime.InteropServices.CallingConvention.StdCall, _
	//    CharSet:=Runtime.InteropServices.CharSet.Unicode, SetLastError:=True)> _
	//    Public Shared Function ShowWindow(ByVal handle As IntPtr, _
	//                                 ByVal nCmd As Int32) As Boolean
	//    ' Leave function empty 
	//
	//    End Function
	//    Public Shared Function GetWindowText(ByVal hWnd)
	//Dim sb, count
	//    count = GetWindowTextLength(hWnd) + 1
	//    StringBuilder(sb = New StringBuilder(count))
	//    GetWindowText(hWnd, sb, count)
	//    Return sb.ToString()
	//    }
	// End Function

	//  Public Shared Function GetChildWindows(ByVal parent)
	//          List result = new List();
	//          GCHandle listHandle = GCHandle.Alloc(result);
	//      Try
	//              EnumChildWindows(parent, EnumWindow, GCHandle.ToIntPtr(listHandle));
	//      Finally
	//          If (listHandle.IsAllocated) Then
	//              listHandle.Free()
	//          End If
	//          Return result
	//      End Try
	//  End Function
    }
    

static class Keyboard
{
	[DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
	public static extern int UnhookWindowsHookEx(int hHook);
	[DllImport("user32", EntryPoint = "SetWindowsHookExA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

	public static extern int SetWindowsHookEx(int idHook, KeyboardHookDelegate lpfn, int hmod, int dwThreadId);
	[DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

	private static extern int GetAsyncKeyState(int vKey);
	[DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

	private static extern int CallNextHookEx(int hHook, int nCode, int wParam, KBDLLHOOKSTRUCT lParam);
   // Main main = new Main();
	public struct KBDLLHOOKSTRUCT
	{
		public int vkCode;
		public int scanCode;
		public int flags;
		public int time;
		public int dwExtraInfo;
	}

	// skip so only process one time

	private static bool blnSkip = true;
	// Low-Level Keyboard Constants
	private const int HC_ACTION = 0;
	private const int LLKHF_EXTENDED = 0x1;
	private const int LLKHF_INJECTED = 0x10;
	private const int LLKHF_ALTDOWN = 0x20;

	private const int LLKHF_UP = 0x80;
	// Virtual Keys
	public const  int VK_TAB = 0x9;
	public const  int VK_CONTROL_LEFT = 162;
	public const  int VK_CONTROL_RIGHT = 163;
	public const  int VK_ALT_LEFT = 164;
	public const  int VK_ALT_RIGHT = 165;
	public const  int VK_SHIFT_LEFT = 164;
	public const  int VK_SHIFT_RIGHT = 161;
	public const  int VK_ESCAPE = 0x1b;
	public const  int VK_DELETE = 0x2e;
	public const  int VK_END = 35;
	public const  int VK_HOME = 36;
	public const  int VK_RIGHT = 39;
	public const  int VK_LEFT = 37;
	public const  int VK_PLUS = 187;
	public const  int VK_MINUS = 189;
	private const int WH_KEYBOARD_LL = 13;
	public static int KeyboardHandle;

	// Implement this function to block as many
	// key combinations as you'd like
	public static bool IsHooked(ref KBDLLHOOKSTRUCT Hookstruct)
	{

		Debug.WriteLine("Hookstruct.vkCode: " + Hookstruct.vkCode);
		Debug.WriteLine(Hookstruct.vkCode == VK_ESCAPE);
		Debug.WriteLine(Hookstruct.vkCode == VK_TAB);

		// If (Hookstruct.vkCode = VK_CONTROL) And _
		//  CBool(GetAsyncKeyState(VK_MINUS) Then
		//
		//
		//        End If
		//
		//  Call HookedState("Ctrl + Esc blocked")
		//  Return True
		//  End If

		//        If (Hookstruct.vkCode = VK_TAB) And _
		//          CBool(Hookstruct.flags And _
		//          LLKHF_ALTDOWN) Then
		//
		//        Call HookedState("Alt + Tab blockd")
		//        Return True
		//        End If
		// = True Then
        Main main = new Main();
		switch (Hookstruct.vkCode) {
			case VK_RIGHT:
				if (processOnce()) {
                    main.goRight();
					return false;
				}
				break;
			case VK_LEFT:
				if (processOnce()) {
                    main.goLeft();
					return false;
				}
				break;
			case VK_MINUS:
				if (processOnce()) {
                    main.goLeft();
					return false;
				}
				break;
			case VK_ALT_LEFT:
				if (Convert.ToBoolean(GetAsyncKeyState(VK_MINUS))) {
                    main.decreaseTrackingAndProcess();
				}
				if (Convert.ToBoolean(GetAsyncKeyState(VK_PLUS))) {
                    main.increaseTrackingAndProcess();
				}
				if (Convert.ToBoolean(GetAsyncKeyState(VK_LEFT))) {
                    main.goHome();
				}
				if (Convert.ToBoolean(GetAsyncKeyState(VK_RIGHT))) {
                    main.goEnd();
				}

				break;
			case VK_ALT_RIGHT:
				if (Convert.ToBoolean(GetAsyncKeyState(VK_MINUS))) {
                    main.decreaseTrackingAndProcess();
				}
				if (Convert.ToBoolean(GetAsyncKeyState(VK_PLUS))) {
                    main.increaseTrackingAndProcess();
				}
				break;
			case VK_CONTROL_LEFT:
				if (Convert.ToBoolean(GetAsyncKeyState(VK_HOME))) {
                    main.goHome();
				}
				if (Convert.ToBoolean(GetAsyncKeyState(VK_END))) {
                    main.goEnd();
				}
				if (Convert.ToBoolean(GetAsyncKeyState(VK_LEFT))) {
                    main.goHome();
				}
				if (Convert.ToBoolean(GetAsyncKeyState(VK_RIGHT))) {
                    main.goEnd();
				}

				break;

			case VK_CONTROL_RIGHT:
				if (Convert.ToBoolean(GetAsyncKeyState(VK_HOME))) {
                    main.goHome();
				}
				if (Convert.ToBoolean(GetAsyncKeyState(VK_END))) {
                    main.goEnd();
				}

				break;
			case VK_HOME:
                main.goHome();
				break;
			case VK_END:
                main.goEnd();

				break;



		}


		//Else
		// skip processing
		//End If

		//        If (Hookstruct.vkCode = VK_RIGHT) Then ' 40 is right arrow
		// If processOnce() Then
		// Main.goRight()
		// End If
		// Call HookedState("Right")
		// Return False
		// End If

		//     If (Hookstruct.vkCode = VK_ESCAPE) And _
		//           CBool(Hookstruct.flags And _
		//             LLKHF_ALTDOWN) Then
		//
		// Call HookedState("Alt + Escape blocked")
		// Return True
		// End If

		return false;
	}


	private static void HookedState(string Text)
	{
		//   MessageBox.Show("Hooked State " + Text)
		Debug.WriteLine(Text);
	}

	public static int KeyboardCallback(int Code, int wParam, ref KBDLLHOOKSTRUCT lParam)
	{

		if ((Code == HC_ACTION)) {
			Debug.WriteLine("Calling IsHooked");

			if ((IsHooked(ref lParam))) {
				return 1;
			}

		}

		return CallNextHookEx(KeyboardHandle, Code, wParam, lParam);

	}
	public delegate int KeyboardHookDelegate(int Code, int wParam, ref KBDLLHOOKSTRUCT lParam);

	[MarshalAs(UnmanagedType.FunctionPtr)]
	private static KeyboardHookDelegate callback;
	public static void HookKeyboard()
	{
		callback = new KeyboardHookDelegate(KeyboardCallback);

		KeyboardHandle = SetWindowsHookEx(WH_KEYBOARD_LL, callback, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]).ToInt32(), 0);

		CheckHooked();
	}

	public static void CheckHooked()
	{
		if ((Hooked())) {
			Debug.WriteLine("Keyboard hooked");
		} else {
			Debug.WriteLine("Keyboard hook failed: " + "LastDllError"); //  todo clarification Err().LastDllError);
		}
	}

	private static bool Hooked()
	{
		return KeyboardHandle != 0;
	}

	public static void UnhookKeyboard()
	{
		if ((Hooked())) {
			UnhookWindowsHookEx(KeyboardHandle);
		}
	}

	public static bool processOnce()
	{
		if (blnSkip == true) {
			blnSkip = false;
			return false;
		} else {
			blnSkip = true;
			return true;
		}

	}

}



    }


    

