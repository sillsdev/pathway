using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using str = Microsoft.VisualBasic.Strings;
using Microsoft.VisualBasic.MyServices;
using Microsoft.VisualBasic.FileIO;
using FileSystem = Microsoft.VisualBasic.FileSystem;

namespace PrincessConvert
{
    public partial class picture : Form
    {
        	// Public sGraphicFileName As String
	// Public sGraphicName As String
	public decimal dPercentOfWidthForPicture;
	public string sPictureClass;
	public decimal dScale;
	public decimal dScale2column;
	public string sPictureDocumentHeightInPoints;
	public string sPicturePaddingTop;
	public string sPicturePaddingBottom;
	public decimal dPictureDocumentHeightInPoints;
	public decimal dPictureDocumentVerticalPaddingInPoints;
	public int iPictureWidthInPixels;
	public int iPictureHeightInPixels;
	public decimal dPictureDocumentWidthInPoints;
	//    Public dPictureWidthInGr As Decimal = 1.0
		// enclose file names in quotes because of spaces
	public static string quote = "\"";
	//<div class="pictureSingleColumnTop" height="217.9pt" width="156pt" margin-bottom="3.1pt" margin-top="0pt" margin-left="" margin-right="" ><img src="C:\d\Research\c2x\figure\hk00099c.png"></img></div> 
	public string sDeletePicture = "<div class=" + quote + "picture" + ".*?" + "</div>";
	public decimal dMarginBottomTotal;
	public decimal dMarginTopTotal;
	public decimal dMarginBottom;
	public decimal dMarginTop;
	public string sMarginBottom;
	public string sMarginTop;
	public decimal dMarginLeft;
	public decimal dMarginRight;
	public string sMarginLeft;
	public string sMarginRight;
	public string sPicturePlacement;
    Main main = new Main();
	public int iPictureSizeInLines;
        public picture()
        {
            InitializeComponent();
        }

        private void picture_Load(object sender, EventArgs e)
        {
            //  setValuesForNumberOfLinesForPicture()
            // display saved values
            main.getValuesFromStylesheet();
            main.writeIniFile();
            this.tbColumnWidth.Text = Main.dColumnWidthInPoints.ToString();
            this.tbLineHeight.Text = Main.dLineHeightInPoints.ToString();

            // toggle these to set 1 column value
            this.rbPictureSpans1Column.Checked = true;
            if (Main.sSelectedPictureFileName == "#none#")
            {
                selectPicture();
            }
            else
            {
                showPicture();
            }
            this.BringToFront();
        }

	public void selectPicture()
	{
		PictureDialog.FileName = Main.sSelectedPictureFileName;
		PictureDialog.Filter = "Picture files (*.tif, *.gif, *.png, *.jpg)|*.tif;*.tiff;*.gif;*.png;*.jpg;*.jpeg;|All files (*.*)|*.*";
		PictureDialog.FilterIndex = 1;
		PictureDialog.Multiselect = false;
		PictureDialog.InitialDirectory = Main.sGraphicsFolder;
		PictureDialog.ShowDialog();
		Main.sSelectedPictureFileName = PictureDialog.FileName;
		if (Main.sSelectedPictureFileName == "#none#") {
		// skip
		} else {
			showPicture();
		}

	}
	private void showPicture()
	{
		this.Show();
		this.BringToFront();
		setUnitOptions();
		//  Me.rb0LineAbove.Select()
		// Me.rb0LineBelow.Select()
		//Me.rbBottom.Select()


		pictureBox2.Show();
		pictureBox2.BringToFront();
		pictureBox2.BorderStyle = BorderStyle.FixedSingle;
		//        PictureBox1.Picture = Picture.FromFile(arrayGraphicFileNames(lbGraphics.SelectedIndex))
		pictureBox2.Image = Image.FromFile(Main.sSelectedPictureFileName);
		Main.sSelectedPictureName = getFileNameWithoutExtensionFromFullName(Main.sSelectedPictureFileName).ToString();
		this.tbGraphicFileName.Text = Main.sSelectedPictureName;
		displayGraphicFileName();

	}
	private void displayGraphicFileName()
	{
		this.Text = Main.sSelectedPictureFileName;
		calculateAndDisplayPictureValues();
	}
	public void calculateAndDisplayPictureValues()
	{
		getWidthAndHeightOfPictureInPixels();
		setDefaultUnits();
		calculateAndDisplayPictureWidthValue();
		calculateAndDisplayPublishedPictureWidthValue();
		calculateAndDisplayScale();
		calculateAndDisplayPercentOfWidthForPicture();
		calculateAndDisplayPictureHeightValue();
		calculateAndDisplayPublishedPictureHeightValue();
		calculateAndDisplaySuggestedLineHeight();

	}
	private void calculateAndDisplayScale()
	{
		//   Dim dScale As Decimal = Main.dColumnWidthInPoints / iPictureWidthInPixels
		//tbScale.Text = Math.Round(dScale * 100).ToString + "%"
		decimal points = convertUnitsFromPixels("points",decimal.Parse(tbPictureWidthInPixels.Text));
		if (points == 0) {
			dScale = 1;
		} else {
			dScale = decimal.Parse(tbPublishedWidthInPixels.Text) / points;
			tbScale.Text = Math.Round(dScale * 100).ToString() + "%";

		}
	}
	public void calculateAndDisplayPublishedPictureHeightValue()
	{
		//        Dim dScale As Decimal = Main.dColumnWidthInPoints / iPictureWidthInPixels
		//   tbPublishedHeightInPixels.Text = dScale / 100 * convertUnitsFromPixels(lbOriginalHeightUnits.SelectedItem, iPictureHeightInPixels)
		tbPublishedHeightInPixels.Text = (dScale * convertUnitsFromPixels("points", iPictureHeightInPixels)).ToString();

	}

	public void calculateAndDisplayPublishedPictureWidthValue()
	{
		//        tbPublishedWidth.Text = convertUnitsFromPixels(lbOriginalWidthUnits.SelectedItem, Main.dColumnWidthInPoints * dScale)
		//    tbPublishedWidthInPixels.Text = Main.dColumnWidthInPoints * dScale

		getPictureDocumentWidth();
		dPictureDocumentWidthInPoints = dPictureDocumentWidthInPoints;
		tbPublishedWidthInPixels.Text = dPictureDocumentWidthInPoints.ToString();

		// tbPublishedWidthInPixels.Text = iPictureWidthInPixels * dScale


	}
	public void calculateAndDisplayPictureWidthValue()
	{
		//   tbPictureWidth.Text = convertUnitsFromPixels(lbOriginalWidthUnits.SelectedItem, iPictureWidthInPixels) * dScale
		tbPictureWidthInPixels.Text = iPictureWidthInPixels.ToString();

	}
	public decimal convertUnitsFromPixels(string sSelectedUnit, decimal dValue)
	{
		decimal dConvertedValue = 0;
		switch (sSelectedUnit) {
			case "pixels":
				dConvertedValue = dValue * 1;
				break;
			//   Case "lines" ' computer not printer
			//      dConvertedValue = dValue * 0.75 / Main.dLineHeightInPoints
			case "points":
				// computer not printer
				dConvertedValue = dValue * (decimal)0.75;
				break;
			case "pica":
				// computer not printer
                dConvertedValue = dValue * (decimal)0.0625;
				break;
			case "inches":
                dConvertedValue = dValue * (decimal)0.010416667;
				break;
			case "cm":
                dConvertedValue = dValue * (decimal)0.026458333;
				break;
			case "mm":
                dConvertedValue = dValue * (decimal)0.264583333;
				break;
			default:
				break;
			// skip
		}


		return dConvertedValue;

	}

	public decimal convertUnitsToPoints(ref string sSelectedUnit, ref decimal dValue)
	{
		decimal dConvertedValue = 0;
		switch (sSelectedUnit) {
			case "pixels":
                dConvertedValue = dValue * (decimal)0.75;
				break;
			// Case "lines" ' computer not printer
			//     dConvertedValue = dValue * 0.75 / Main.sLineHeightInPoints
			case "points":
				// computer not printer
				dConvertedValue = dValue * 1;
				break;
			case "pica":
				// computer not printer
				dConvertedValue = dValue * 12;
				break;
			case "inches":
				dConvertedValue = dValue * 72;
				break;
			case "cm":
                dConvertedValue = dValue * (decimal)28.346456693;
				break;
			case "mm":
                dConvertedValue = dValue * (decimal)2.834645669;
				break;
			default:
				break;
			// skip
		}


		return dConvertedValue;

	}
	public decimal convertUnitsFromPoints(string sSelectedUnit, decimal dValue)
	{
		decimal dConvertedValue = 0;
		switch (sSelectedUnit) {
			case "pixels":
                dConvertedValue = dValue * (decimal)1.333333333;
				break;
			case "lines":
				// computer not printer
				dConvertedValue = dValue / Main.dLineHeightInPoints;
				break;
			case "points":
				// computer not printer
				dConvertedValue = dValue * 1;
				break;
			case "pica":
				// computer not printer
                dConvertedValue = dValue * (decimal)0.083333333;
				break;
			case "inches":
                dConvertedValue = dValue * (decimal)0.013888889;
				break;
			case "cm":
                dConvertedValue = dValue * (decimal)0.035277778;
				break;
			case "mm":
                dConvertedValue = dValue * (decimal)0.352777778;
				break;
			default:
				break;
			// skip
		}


		return dConvertedValue;

	}

	public void calculateAndDisplayPictureHeightValue()
	{
		tbPictureHeightInPixels.Text = convertUnitsFromPixels(lbOriginalHeightUnits.SelectedItem.ToString(), iPictureHeightInPixels).ToString();
	}

	public void getWidthAndHeightOfPictureInPixels()
	{
		if (Main.sSelectedPictureFileName == "#none#") {
		// skip
		} else {
			string test = Main.sSelectedPictureFileName;
			ImageLoader MyImage = new ImageLoader(ref Main.sSelectedPictureFileName);
			// BMP not supported
			if (MyImage.Type == ImageLoader.ImageType.PNG | MyImage.Type == ImageLoader.ImageType.GIF | MyImage.Type == ImageLoader.ImageType.JPEG | MyImage.Type == ImageLoader.ImageType.TIFF) {
				iPictureWidthInPixels = int.Parse(MyImage.Width.ToString());
				iPictureHeightInPixels = int.Parse(MyImage.Height.ToString());
				tbPictureWidthInPixels.Text = iPictureWidthInPixels.ToString();
				tbPictureHeightInPixels.Text = iPictureHeightInPixels.ToString();
			}
		}

	}
	public void setDefaultUnits()
	{
		lbOriginalWidthUnits.SelectedItem = "pixels";
		lbOriginalHeightUnits.SelectedItem = "pixels";
		lbColumnWidthUnits.SelectedItem = "points";
		lbLineHeightUnits.SelectedItem = "points";

	}
	private void btnOK_Click(System.Object sender, System.EventArgs e)
	{
		btnOKprocess();
	}
	public void btnOKprocess()
	{
		getGraphicSettings();
		main.writeIniFile();
		if (findTextInSourceAndApplyPictureAttributes() == true) {
			this.Close();
			main.convertWithPrinceAndDisplayPDF();
		}

	}
	public void getGraphicSettings()
	{
		getPictureClass();
		iPictureSizeInLines = (int)NumericUpDownHeightInLines.Value;
		// dPictureWidthInGr = NumericUpDownWidthInGr.Value
		getPictureDocumentWidth();
		dMarginTop = Main.dLineHeightInPoints * NumericUpDownMarginTopInLines.Value;
		dMarginBottom = Main.dLineHeightInPoints * NumericUpDownMarginBottomInLines.Value;
		if (rbBottom.Checked == true) {
			//      sPicturePlacement = "bottom"
			dMarginTopTotal = dMarginTop + dPictureDocumentVerticalPaddingInPoints;
			dMarginBottomTotal = 0;
		} else if (rbTop.Checked == true) {
			//     sPicturePlacement = "top"
			dMarginTopTotal = 0;
			dMarginBottomTotal = dMarginBottom + dPictureDocumentVerticalPaddingInPoints;
		} else if (rbInline.Checked == true) {
			//    sPicturePlacement = "inline"
			dMarginTopTotal = dMarginTop + (dPictureDocumentVerticalPaddingInPoints / 2);
			dMarginBottomTotal = dMarginBottom + (dPictureDocumentVerticalPaddingInPoints / 2);
		} else {
			//   sPicturePlacement = "#none#"
		}
		sMarginTop = dMarginTopTotal.ToString() + "pt";
		sMarginBottom = dMarginBottomTotal.ToString() + "pt";
	}
	private void getPictureDocumentWidth()
	{
		if (rbPictureSpans2Columns.Checked == true) {
			//        dPictureDocumentWidthInPoints = (Main.iNumberOfColumns * Main.dColumnWidthInPoints) + Main.dColumnGapInPoints
			dPictureDocumentWidthInPoints = (Main.iNumberOfColumns * Main.dColumnWidthInPoints) + Main.dColumnGapInPoints;
			//dPictureDocumentWidthInPoints = Main.dColumnWidthInPoints
			rbInline.Enabled = true;
		//     If rbInline.Checked = True Then
		// rbBottom.Checked = True
		// Else
		//     ' skip
		// End If

		} else {
			// assume dScale = 1   * dScale
			dPictureDocumentWidthInPoints = Main.dColumnWidthInPoints;
			rbInline.Enabled = true;
		}
		dPictureDocumentWidthInPoints = dPictureDocumentWidthInPoints * dPercentOfWidthForPicture;
	}
	private void getPictureClass()
	{
		if (rbPictureSpans2Columns.Checked == true) {
			if (rbBottom.Checked) {
				sPictureClass = "pictureDoubleColumnBottom";
			} else if (rbTop.Checked) {
				sPictureClass = "pictureDoubleColumnTop";
			} else {
				// not allowed
			}

		} else {
			if (rbBottom.Checked) {
				sPictureClass = "pictureSingleColumnBottom";
			} else if (rbTop.Checked) {
				sPictureClass = "pictureSingleColumnTop";
			} else {
				sPictureClass = "pictureSingleColumnInline";
			}
		}
	}
	private object insertPictureAttributesIntoDeletingPrevious(string line)
	{
		removePictureAttributesFromLine(line);
		string sStartOfString = null;
		string sRestOfString = null;
		int iBreakPosition = line.IndexOf(' ');
		string sPictureAttributesToInsert = pictureAttributesToInsert();
		sStartOfString = str.Left(line, iBreakPosition);
		//sStartOfString = "<div "
		sRestOfString = str.Mid(line, iBreakPosition);
		//sRestOfString = "> " + str.Mid(line, iBreakPosition)
		return sStartOfString + sPictureAttributesToInsert + sRestOfString;
	}
	public void removePictureAttributesFromLine(string Line)
	{
		// remove previous picture attributes if found
		Line = Main.regexReplace(Line, " class=" + quote + ".*?" + quote, "");
		Line = Main.regexReplace(Line, " height=" + quote + ".*?" + quote, "");
		Line = Main.regexReplace(Line, " width=" + quote + ".*?" + quote, "");
		Line = Main.regexReplace(Line, " margin-top=" + quote + ".*?" + quote, "");
		Line = Main.regexReplace(Line, " margin-bottom=" + quote + ".*?" + quote, "");
		Line = Main.regexReplace(Line, " margin-left=" + quote + ".*?" + quote, "");
		Line = Main.regexReplace(Line, " margin-right=" + quote + ".*?" + quote, "");
		Line = Main.regexReplace(Line, " float=" + quote + ".*?" + quote, "");
		Line = Main.regexReplace(Line, " padding-top=" + quote + ".*?" + quote, "");
		Line = Main.regexReplace(Line, " padding-bottom=" + quote + ".*?" + quote, "");
		Line = Main.regexReplace(Line, " widthGr=" + quote + ".*?" + quote, "");
		Line = Main.regexReplace(Line, " size=" + quote + ".*?" + quote, "");
		// padding and widthGr and size no longer used

	}
	public string pictureAttributesToInsert()
	{
		//Dim sSize As String = iPictureSizeInLines * Main.sLineHeightInPoints
		string sPictureAttributesToInsert = "class=" + quote + sPictureClass + quote + " " + "height=" + quote + sPictureDocumentHeightInPoints + quote + " " + "width=" + quote + dPictureDocumentWidthInPoints.ToString() + "pt" + quote + " " + "margin-bottom=" + quote + sMarginBottom + quote + " " + "margin-top=" + quote + sMarginTop + quote + " " + "margin-left=" + quote + sMarginLeft + quote + " " + "margin-right=" + quote + sMarginRight + quote + " ";


		//"margin-left=" + quote + sMarginLeft + quote + " " + _
		//       "margin-right=" + quote + sMarginRight + quote + " " + _
		//      "float=" + quote + sPicturePlacement + quote + " "
		//   "padding-top=" + quote + sPicturePaddingTop + quote + " " + _
		//   "padding-bottom=" + quote + sPicturePaddingBottom + quote + " "
		//  "widthGr=" + quote + dPictureWidthInGr.ToString + "gr" + quote + " " + _

		return sPictureAttributesToInsert;
	}
	private object insertPictureAttributesInto(string line)
	{
		// remove previous picture attributes if found
		removePictureAttributesFromLine(line);
		string sStartOfString = null;
		string sRestOfString = null;
		int iBreakPosition = line.IndexOf(' ');
		sStartOfString = str.Left(line, iBreakPosition);
		sRestOfString = str.Mid(line, iBreakPosition);
		return sStartOfString + pictureAttributesToInsert() + sRestOfString;
	}
	private bool findTextInSourceAndApplyPictureAttributes()
	{
		try {
			Main.makeTextCanonical();
			// look at document line by line and find match
			string sSearchingForPictureName = Main.sSelectedPictureName.ToUpper();
			bool matchFound = false;
			string lineToSearch = null;
			string line = "";
			string sText = "";
			StreamReader sr = new StreamReader(Main.sDocumentFileName);
			while (!(sr.EndOfStream)) {
				line = sr.ReadLine();
				// upper case both line to search and searaching for Picture name
				lineToSearch = line.ToUpper();
				if (lineToSearch.Contains(sSearchingForPictureName)) {
					// found matching line
					// insert picture text
					// sText is being built up line by line
					sText = sText + insertPictureAttributesIntoDeletingPrevious(line);
					matchFound = true;
				} else {
					sText = sText + line;
				}
			}
			sr.Close();
			File.WriteAllText(Main.sDocumentFileName, sText);
			Main.makeTextCanonical();
			return matchFound;
		} catch (Exception ex) {
			MessageBox.Show("Error" + Constants.vbCrLf + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
			return false;
		}
	}
	public object getFileNameWithoutExtensionFromFullName(string filename)
	{
		if (filename != null) {
			filename = getFileNameWithExtensionFromFullName(filename);
			// assume one .
			filename = str.Left(filename, filename.IndexOf(".") - 1);
		} else {
			// skip
		}
		return filename;
	}
	public string getFileNameWithExtensionFromFullName(string filename)
	{
		Int16 i = default(Int16);
		if (filename != null) {
			while (filename.IndexOf("\\") > 0) {
				i = (short)filename.IndexOf("\\");
				filename = str.Mid(filename, filename.IndexOf("\\") + 1);
			}
		} else {
			// skip
		}
		return filename;
	}
	// Public Sub displayGraphicFilesInListBox()
	// Dim i As Integer = 0
	// Dim filename As String
	//     Try
	//         For Each filename In Directory.GetFiles(Main.sGraphicsFolder)
	//             If filename.Contains(".png") Or _
	//             filename.Contains(".jpg") Or _
	//             filename.Contains(".jpeg") Or _
	//             filename.Contains(".tiff") Then
	//                 lbGraphics.Items.Add(filename)
	//         lbGraphics.Items.Add(getFileNameWithExtensionFromFullName(filename))
	// '                arrayGraphicFileNames(i) = filename
	//               i = i + 1
	//              End If
	//          Next
	//          lbGraphics.Show()
	//          If Main.sSelectedPictureFileName = "#none#" Then
	//              lbGraphics.SelectedIndex = 0
	//         Else
	//              lbGraphics.SelectedItem = Main.sSelectedPictureFileName
	//        End If
	//
	//        Catch ex As Exception
	//    ' missing file
	//        End Try
	//    End Sub
	private void btnInsertGraphicHere_Click(System.Object sender, System.EventArgs e)
	{
		// find out where we are atand insert graphic before paragraph
		this.Cursor = Cursors.WaitCursor;
	    Main main = new Main();
        main.getCliptext();
		string sFoundText = Main.sCliptext;
		if (sFoundText == null) {
		// skip
		} else {
			if (findTextInSourceAndInsertGraphic(sFoundText) == true) {
                main.convertWithPrinceAndDisplayPDF();
			} else {
				// skip
			}
			// create PDF and view it
		}
		// turn off the insert button
		btnInsertGraphicHere.Visible = false;
		btnOK.Visible = true;
		this.Cursor = Cursors.Default;
		this.Hide();
	}
	private bool findTextInSourceAndInsertGraphic(string tmp)
	{
		try {
            Main main = new Main();
            if (main.isSelectedTextUnique(Main.sSelectedText) == true)
            {
				// make sure that the text is on separate lines
				Main.makeTextCanonical();
				// look at document line by line and find match
				string line = "";
				string sText = "";
				StreamReader sr = new StreamReader(Main.sDocumentFileName);
				while (!(sr.EndOfStream)) {
					line = sr.ReadLine();
					// we have possibly reduced the text to just this on sCliptext
					if (line.Contains(Main.sCliptext)) {
						// found matching line
						// insert tracking
						sText = sText + insertGraphic(ref line);
					} else {
						sText = sText + line;
					}
				}
				sr.Close();
				File.WriteAllText(Main.sDocumentFileName, sText);
				return true;
			} else {
				// skip as no unique match found
				return false;
			}
			//return "unknown";

		} catch (Exception ex) {
			MessageBox.Show("Error, but will try to continue." + Constants.vbCrLf + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
			return false;
		}
	}
	private object insertGraphic(ref string line)
	{
		// remove previous graphic if found
		string oldLine = line;
		try {
			getGraphicSettings();
			string sPictureAttributesToInsert = null;
			sPictureAttributesToInsert = pictureAttributesToInsert();
			//<div size="8lines" margin-bottom="" margin-top="13pt" float="bottom" margin-left="" margin-right=""><img src="figure\LB00310C.png" alt=""></img></div>
			string sGraphicToInsert = null;
			sGraphicToInsert = "<div " + sPictureAttributesToInsert + "><img src=" + quote + Main.sSelectedPictureFileName + quote + "></img></div>";
			return sGraphicToInsert + line;
		} catch (Exception ex) {
			MessageBox.Show("Error inserting graphic" + Constants.vbCrLf + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			return oldLine;
		}
	}
	private void btnRemove_Click(System.Object sender, System.EventArgs e)
	{
		// find matching picture
		// delete picture
		this.Cursor = Cursors.WaitCursor;
		if (findTextInSourceAndDeletePicture() == true) {
			main.convertWithPrinceAndDisplayPDF();
		} else {
			// skip
		}
		// turn off the insert button
		btnRemove.Visible = false;
		btnOK.Visible = true;
		this.Cursor = Cursors.Default;
		this.Hide();
	}
	private bool findTextInSourceAndDeletePicture()
	{
		try {
			Main.makeTextCanonical();
			// look at document line by line and find match
			string sSearchingForPictureName = Main.sSelectedPictureName.ToUpper();
			bool matchFound = false;
			string lineToSearch = null;
			string line = "";
			string sText = "";
			StreamReader sr = new StreamReader(Main.sDocumentFileName);
			while (!(sr.EndOfStream)) {
				line = sr.ReadLine();
				// upper case both line to search and searaching for Picture name
				lineToSearch = line.ToUpper();
				if (lineToSearch.Contains(sSearchingForPictureName)) {
					// found matching line
					// insert picture text
					sText = sText + deletePicture(line);
					matchFound = true;
				} else {
					sText = sText + line;
				}
			}
			sr.Close();
			File.WriteAllText(Main.sDocumentFileName, sText);
			Main.makeTextCanonical();
			return matchFound;
		} catch (Exception ex) {
			MessageBox.Show("Error" + Constants.vbCrLf + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
			return false;
		}
	}
	private string deletePicture(string line)
	{
		// remove previous picture 
		//<div size="***error***" margin-top="" margin-bottom="" margin-left="" margin-right="" float=""><img src="figure\HK00038C.png" alt=""></img></div>
		line = Main.regexReplace(line, sDeletePicture, "");
		return line;
	}
	// Public Sub seeWhatPictureLoaderCanDo()
	//Dim MyImage As New ImageLoader("C:\d\Research\c2x\figure\CN01656b.png")
	//   If MyImage.Type = ImageLoader.ImageType.PNG Then
	//      Debug.Print(MyImage.Width.ToString)
	//     MyImage.LoadBitmap()
	//    Debug.Print(MyImage.Bitmap.Width.ToString)
	// End If


	// End Sub

	//  Private Sub tbLineHeight_MouseClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbLineHeight.MouseClick
	//
	//        warningMessageForChangeThisSetting()
	//    End Sub
	//    Private Sub tbLineHeight_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbLineHeight.MouseEnter
	//        warningMessageForChangeThisSetting()
	//    End Sub
	//    Private Sub tbLineHeight_MouseOver(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbLineHeight.MouseHover
	//        warningMessageForChangeThisSetting()
	//    End Sub
	//    Private Sub tbColumnWidth_MouseOver(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbColumnWidth.MouseEnter
	//        warningMessageForChangeThisSetting()
	//    End Sub
	private void tbLineHeight_TextChanged(System.Object sender, System.EventArgs e)
	{
		if (Main.blnAlternateSelector == true) {
		// skip warning message
		} else {
			warningMessageForChangeThisSetting();

		}
	}

	private void tbColumnWidth_TextChanged(System.Object sender, System.EventArgs e)
	{
		if (Main.blnAlternateSelector == true) {
		// skip warning message
		} else {
			warningMessageForChangeThisSetting();

		}
	}
	private void warningMessageForChangeThisSetting()
	{
		if (Main.sSelectedPictureFileName == "#none#") {
			MessageBox.Show("To change this setting use Configuration/Settings", "Not allowed to change setting here.", MessageBoxButtons.OK, MessageBoxIcon.Information);
		} else {
			// skip
		}

	}

	private void setUnitOptions()
	{
		lbOriginalWidthUnits.Items.Add("pixels");
		lbOriginalWidthUnits.Items.Add("points");
		lbOriginalWidthUnits.Items.Add("pica");
		lbOriginalWidthUnits.Items.Add("inches");
		lbOriginalWidthUnits.Items.Add("mm");
		lbOriginalWidthUnits.Items.Add("cm");
		lbOriginalHeightUnits.Items.Add("pixels");
		lbOriginalHeightUnits.Items.Add("lines");
		lbOriginalHeightUnits.Items.Add("points");
		lbOriginalHeightUnits.Items.Add("pica");
		lbOriginalHeightUnits.Items.Add("inches");
		lbOriginalHeightUnits.Items.Add("mm");
		lbOriginalHeightUnits.Items.Add("cm");
		lbLineHeightUnits.Items.Add("pixels");
		lbLineHeightUnits.Items.Add("points");
		lbLineHeightUnits.Items.Add("pica");
		lbLineHeightUnits.Items.Add("inches");
		lbLineHeightUnits.Items.Add("mm");
		lbLineHeightUnits.Items.Add("cm");
		lbColumnWidthUnits.Items.Add("pixels");
		lbColumnWidthUnits.Items.Add("points");
		lbColumnWidthUnits.Items.Add("pica");
		lbColumnWidthUnits.Items.Add("inches");
		lbColumnWidthUnits.Items.Add("mm");
		lbColumnWidthUnits.Items.Add("cm");

	}

	//  Private Sub lbOriginalWidthUnits_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbOriginalWidthUnits.SelectedIndexChanged
	//   calculateAndDisplayPictureValues()
	//  Main.writeIniFile()
	//  End Sub

	//  Private Sub lbOriginalHeightUnits_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbOriginalHeightUnits.SelectedIndexChanged
	//     calculateAndDisplayPictureValues()
	//    Main.writeIniFile()

	// End Sub
	private void calculateAndDisplaySuggestedLineHeight()
	{
		//      Dim iPictureWidthInPoints As Integer
		//If NumericUpDownWidthInGr.Value <= 1 Then
		//  Dim dScaleOfPictureToFullColumn = Main.dColumnWidthInPoints / convertUnitsFromPixels("points", iPictureWidthInPixels)
		//  Dim dScaleOfPictureToFullPageWidth = Main.dPageWidthInPoints / convertUnitsFromPixels("points", iPictureWidthInPixels)
		//       If NumericUpDownWidthInGr.Value = 3 Then
		// dScale = dScaleOfPictureToFullPageWidth
		// Else
		//     dScale = dScaleOfPictureToFullColumn * NumericUpDownWidthInGr.Value
		// End If
		//Else
		// dScale = Main.dColumnWidthInPoints / (convertUnitsFromPixels("points", iPictureWidthInPixels)) / Main.dColumnWidthInPoints ' convert so both are in points
		//        End If


		try {
			// get height rounded to tenth of point
			dPictureDocumentHeightInPoints = Math.Round((dScale * convertUnitsFromPixels("points", iPictureHeightInPixels)) * 10) / 10;
			sPictureDocumentHeightInPoints = dPictureDocumentHeightInPoints.ToString() + "pt";
			// round up
			NumericUpDownHeightInLines.Value = Math.Ceiling(convertUnitsFromPoints("lines", dPictureDocumentHeightInPoints));
			// round to tentof of point
			decimal dPictureDocumentVerticalPaddingInPointsBeforeRounding = (NumericUpDownHeightInLines.Value * Main.dLineHeightInPoints) - dPictureDocumentHeightInPoints;
			dPictureDocumentVerticalPaddingInPoints = Math.Round(dPictureDocumentVerticalPaddingInPointsBeforeRounding * 10) / 10;

		} catch (Exception ex) {
			// gracefully skip error
		}
	}
	private void lbColumnWidthUnits_SelectedIndexChanged(System.Object sender, System.EventArgs e)
	{
		//      tbColumnWidth.Text = convertUnitsFromPoints(lbColumnWidthUnits.SelectedItem, Main.dColumnWidthInPoints)

	}

	private void lbLineHeightUnits_SelectedIndexChanged(System.Object sender, System.EventArgs e)
	{
		//     tbLineHeight.Text = convertUnitsFromPoints(lbLineHeightUnits.SelectedItem, Main.dLineHeightInPoints)

	}

	private void btnBrowse_Click(System.Object sender, System.EventArgs e)
	{
		this.Hide();
		selectPicture();

	}

	private void btnCancel_Click(object sender, System.EventArgs e)
	{
		this.Cursor = Cursors.Default;
		this.Close();
	}

	//  Private Sub NumericUpDownWidthInGr_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDownWidthInGr.ValueChanged
	//       dPictureWidthInGr = NumericUpDownWidthInGr.Value
	//        Me.calculateAndDisplayPictureValues()
	//End Sub

	private void NumericUpDownMarginBottom_ValueChanged(System.Object sender, System.EventArgs e)
	{
		//   btnOK.Enabled = False

	}



	private void rbBottom_CheckedChanged(System.Object sender, System.EventArgs e)
	{
		//   btnOK.Enabled = False

	}

	private void rbPictureSpans2Columns_CheckedChanged(System.Object sender, System.EventArgs e)
	{
		getPictureDocumentWidth();
		// btnOK.Enabled = False
		this.calculateAndDisplayPictureValues();

	}

	private void calculateAndDisplayPercentOfWidthForPicture()
	{
		dPercentOfWidthForPicture = NumericUpDownPercentOfColumnForPicture.Value / 100;

	}

	private void NumericUpDownPercentOfColumnForPicture_ValueChanged(System.Object sender, System.EventArgs e)
	{
		//    btnOK.Enabled = False
		calculateAndDisplayPercentOfWidthForPicture();
		this.calculateAndDisplayPictureValues();
	}

	private void btnCalculateWidthAndHeight_Click(System.Object sender, System.EventArgs e)
	{
		// btnOK.Enabled = True
		this.calculateAndDisplayPictureValues();
	}


	private void rbPictureSpans1Column_CheckedChanged(System.Object sender, System.EventArgs e)
	{
	}


	private void lblColumnWidthInPoints_Click(System.Object sender, System.EventArgs e)
	{
	}

}
/// <summary>
/// The ImageLoader class can retrieve image metadata using very little memory.
/// </summary>
/// <remarks>After constructing an ImageLoader, the object will expose
/// properties including file type, height, width, bits per pixel, etc.
/// The caller should first check that Type is not equal xNone
/// to confirm the object was constructed successfully.
/// 
/// There are two methods that add more functionality: LoadBitmap and UnloadBitmap.
/// These control when and how the Bitmap property is loaded into memory.
/// 
/// Submitted by Robert Chapin
/// Chapin Information Services, Inc.  http://www.info-svc.com/
/// 24 January 2008
/// 
/// This class is based on and inspired by David Crowell's code from
/// http://www.freevbcode.com/ShowCode.Asp?ID=112
/// Parts that were obviously buggy or not working in Mr Crowell's version
/// have been corrected.  The rest of his algorithm is essentially unchanged.
/// 
/// This class is being published in like kind at www.freevbcode.com
/// It is also being used in the latest beta version of ThetaWall at
/// http://www.info-svc.com/software/thetawall/
/// 
/// <example><code>
/// Dim MyImage As New ImageLoader("C:\myfolder\pic1.jpg")
/// If ExtraImage.Type = ImageLoader.ImageType.JPEG Then
///     Debug.Print(MyImage.Width.ToString)
///     MyImage.LoadBitmap()
///     Debug.Print(MyImage.Bitmap.Width.ToString)
/// End If
/// </code></example>
/// </remarks>
/// <permission cref="Security.Permissions.FileIOPermission">The class constructor
/// will throw a System.Security.SecurityException object if it is run from
/// a network drive in the default environment.  This is caused by .NET performing
/// an implicit security check before executing the Sub New().  It is assumed
/// all security matters are being handled by the caller and are outside the
/// intended scope of this class.</permission>
public class ImageLoader
{

	private Bitmap FullImage;
	private Size Dimensions = new Size(0, 0);
	private int ImageBPP;
	private string FilePath;
	private int FileSize;

	private ImageType FileType = ImageType.xNone;
	public enum ImageType : int
	{
		/// <summary>
		/// The xNone type indicates the object is empty or failed to load an image file.
		/// </summary>
		/// <remarks></remarks>
		xNone = 0,
		GIF = 1,
		JPEG = 2,
		PNG = 3,
		BMP = 4,
		TIFF = 5
	}

	/// <summary>
	/// Creates an ImageLoader object, then reads metadata from the specified path.
	/// </summary>
	/// <param name="Path">Absolute path to any supported image file.</param>
	/// <remarks>A massive memory savings is achieved by not using a Bitmap object to retrieve metadata.</remarks>

	public ImageLoader(ref string Path)
	{
		FilePath = Path;
		try {
			FileSize = (int)FileSystem.FileLen(Path);
			ReadImageInfo();
		} catch (Exception ex) {
			FailedImageHandler(ref ex, ref Path);
		}

	}

	#region "Properties and Methods"

	/// <summary>
	/// Holds a System.Drawing.Bitmap object 
	/// </summary>
	/// <value>GDI+ Bitmap</value>
	/// <returns></returns>
	/// <remarks>Loaded with a valid imageAftercalling the LoadBitmap() method.
	/// At all other times, IsNothing(ImageLoader.Bitmap) === True</remarks>
	public Bitmap Bitmap {
		get { return FullImage; }
	}

	/// <summary>
	/// Image sample size in Bits Per Pixel
	/// </summary>
	/// <value>Typically 1 - 48 bpp</value>
	/// <returns></returns>
	/// <remarks></remarks>
	public int ColorDepth {
		get { return ImageBPP; }
	}

	/// <summary>
	/// Image height in pixels
	/// </summary>
	/// <value></value>
	/// <returns></returns>
	/// <remarks></remarks>
	public int Height {
		get { return Dimensions.Height; }
	}

	/// <summary>
	/// Size of the image file in bytes
	/// </summary>
	/// <value></value>
	/// <returns></returns>
	/// <remarks></remarks>
	public int Length {
		get { return FileSize; }
	}

	/// <summary>
	/// Reads the entire image file into the Bitmap property
	/// </summary>
	/// <returns></returns>
	/// <remarks></remarks>
	public bool LoadBitmap()
	{
		bool functionReturnValue = false;
		functionReturnValue = false;
		if (FileType != ImageType.xNone) {
			try {
				FullImage = new Bitmap(FilePath);
				functionReturnValue = true;
			} catch (Exception ex) {
				FailedImageHandler(ref ex, ref FilePath);
			}
		}
		return functionReturnValue;
	}

	/// <summary>
	/// The path that was used to contruct the ImageLoader.
	/// </summary>
	/// <value></value>
	/// <returns></returns>
	/// <remarks></remarks>
	public string Path {
		get { return FilePath; }
	}

	/// <summary>
	/// The image dimensions as a System.Drawing.Size object.
	/// </summary>
	/// <value></value>
	/// <returns></returns>
	/// <remarks></remarks>
	public Size Size {
		get { return Dimensions; }
	}

	/// <summary>
	/// The image type detected by the ImageLoader constructor.
	/// </summary>
	/// <value>ImageLoader.ImageType enumeration</value>
	/// <returns></returns>
	/// <remarks></remarks>
	public ImageType Type {
		get { return FileType; }
	}

	/// <summary>
	/// Frees memory referenced by the Bitmap property.
	/// </summary>
	/// <remarks>This method can be used when the Bitmap is no longer needed.
	/// The ImageLoader will retain its metadata until it passes out of scope.</remarks>
	public void UnLoadBitmap()
	{
		FullImage = null;
	}

	/// <summary>
	/// Image width in pixels.
	/// </summary>
	/// <value></value>
	/// <returns></returns>
	/// <remarks></remarks>
	public int Width {
		get { return Dimensions.Width; }
	}

	#endregion

	//#region "Member Functions"

	/// <summary>
	/// Performs all failure mode tasks for Exceptions thrown during file reads.
	/// </summary>
	/// <param name="ex">The Exception object caught by the caller.</param>
	/// <param name="Path">The absolute path to the file that threw the Exception.</param>
	/// <remarks>FailedImageHandler() is called automatically by LoadBitmap() and ReadImageInfo().
	/// This is an ideal place to centralize file error handling.</remarks>
	private void FailedImageHandler(ref Exception ex, ref string Path)
	{
		FileType = ImageType.xNone;
		switch (Interaction.MsgBox(Application.ProductName + " was unable to render an image from" + Constants.vbCrLf + FilePath + Constants.vbCrLf + Constants.vbCrLf + "Click Abort to close " +Application.ProductName + " and view the folder." + Constants.vbCrLf + "Click Retry to rename the file using *.bad" + Constants.vbCrLf + "Click Ignore to continue with the error.", MsgBoxStyle.AbortRetryIgnore, "Image File Problem")) {

			case MsgBoxResult.Abort:
				//todo when output comes check this. Process.Start(System.Linq.Strings.Left(Path, System.Linq.Strings.InStrRev(Path, "\\")));
				System.Environment.Exit(0);
				break;
			case MsgBoxResult.Retry:
				try {
                    FileSystem.FileCopy(Path, Path + ".bad");
				} catch (Exception ex2) {
					Interaction.MsgBox("Unable to rename file.  " + ex2.Message, MsgBoxStyle.Information, "Rename Failed");
				}
				break;
		}
	}

	/// <summary>
	/// Reads image headers to determine the file type and attributes. (metadata)
	/// </summary>
	/// <remarks></remarks>

	private void ReadImageInfo()
	{
		//Define constants
		const int BUFFERSIZE = 4096;
		const int MinimumFileLength = 25;
		//Files smaller than this number of bytes are treated as invalid.
		const int TIFFFieldLen = 12;
		//Each field in the IFD is 12 bytes long.

		byte[] BMPHeader = {
			66,
			77
		};
		byte[] GIFHeader = {
			71,
			73,
			70
		};
		byte[] JPEGHeader = {
			0xff,
			0xd8,
			0xff
		};
		byte[] PNGHeader = {
			137,
			80,
			78,
			71,
			13,
			10,
			26,
			10
		};
		byte[] TIFFHeader = {
			0x49,
			0x49,
			0x2a,
			0x0
		};
		byte[] TIFFHeaderRev = {
			0x4d,
			0x4d,
			0x0,
			0x2a
		};
		byte[] TIFFWidthTag = {
			0x0,
			0x1
		};
		byte[] TIFFWidthTagRev = {
			0x1,
			0x0
		};
		byte[] TIFFHeightTag = {
			0x1,
			0x1
		};
		byte[] TIFFDepthTag = {
			0x2,
			0x1
		};
		byte[] TIFFDepthTagRev = {
			0x1,
			0x2
		};

		//Dimension locals
		bool NormalByteOrder = false;
		//Normal here means string byte order, where the least-significant byte is stored first.

		int BufferedBytes = 0;
		//Used in all buffer flow checks.
		int FileOffset = 0;
		//Tracks where the buffer begins.  Not the same as stream.position which can be beyond EOF.
		int TempPos = 0;
		//Position within the buffer.
		int TempField = 0;
		//Receives return values and flags.
		int TIFFHeaderLen = 0;
		//Receives the IFD offset, which will be at least 8 bytes followed by a 2-byte count of fields.

		byte[] bBuf = new byte[BUFFERSIZE];
		//Arrays are zero-based and (n) is UBound, not Count.

		//Open the image file.
		System.IO.FileStream InStream = new System.IO.FileStream(FilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);

		//Read the file and fill the buffer.
		BufferedBytes = InStream.Read(bBuf, 0, BUFFERSIZE);

		//Check buffer underflow.
		if (BufferedBytes < MinimumFileLength)
			goto ErrorOut;

		if (bBuf[0] == PNGHeader[0] && bBuf[1] == PNGHeader[1] && bBuf[2] == PNGHeader[2] && bBuf[3] == PNGHeader[3] && bBuf[4] == PNGHeader[4] && bBuf[5] == PNGHeader[5] && bBuf[6] == PNGHeader[6] && bBuf[7] == PNGHeader[7]) {
			// this is a PNG file

			FileType = ImageType.PNG;
			NormalByteOrder = false;

			// get bit depth
			switch (bBuf[25]) {
				case 0:
					// greyscale
					ImageBPP = bBuf[24];

					break;
				case 2:
					// RGB encoded
					ImageBPP = bBuf[24] * 3;

					break;
				case 3:
					// Palette based, 8 bpp
					ImageBPP = 8;

					break;
				case 4:
					// greyscale with alpha
					ImageBPP = bBuf[24] * 2;

					break;
				case 6:
					// RGB encoded with alpha
					ImageBPP = bBuf[24] * 4;

					break;
				default:
					// This value is outside of it's normal range, so we'll assume
					// that this is not a valid file
					FileType = ImageType.xNone;

					break;
			}

			if (FileType != ImageType.xNone) {
				// if the image is valid then

				// get the width  'Note: Reverse byte order
				Dimensions.Width = Mult(bBuf[16], bBuf[17], bBuf[18], bBuf[19], NormalByteOrder);

				// get the height
				Dimensions.Height = Mult(bBuf[20], bBuf[21], bBuf[22], bBuf[23], NormalByteOrder);
			}

		} else if (bBuf[0] == GIFHeader[0] && bBuf[1] == GIFHeader[1] && bBuf[2] == GIFHeader[2]) {
			// this is a GIF file

			FileType = ImageType.GIF;
			NormalByteOrder = true;

			// get the width
			Dimensions.Width = Mult(bBuf[6], bBuf[7], NormalByteOrder);

			// get the height
			Dimensions.Height = Mult(bBuf[8], bBuf[9], NormalByteOrder);

			// get bit depth
			ImageBPP = (bBuf[10] & 7) + 1;

		} else if (bBuf[0] == BMPHeader[0] && bBuf[1] == BMPHeader[1]) {
			// this is a BMP file

			FileType = ImageType.BMP;
			NormalByteOrder = true;

			// get the width
			Dimensions.Width = Mult(bBuf[18], bBuf[19], NormalByteOrder);

			// get the height
			Dimensions.Height = Mult(bBuf[22], bBuf[23], NormalByteOrder);

			// get bit depth
			ImageBPP = bBuf[28];

		} else if (bBuf[0] == bBuf[1] && (bBuf[0] == TIFFHeader[0] || bBuf[0] == TIFFHeaderRev[0])) {
			// this is a TIFF file

			NormalByteOrder = (bBuf[0] == TIFFHeader[0]);

			//Test for valid byte order on second word, a 16-bit representation of decimal 42.
			if (Mult(bBuf[2], bBuf[3], NormalByteOrder) == 42) {
				FileType = ImageType.TIFF;
			} else {
				FileType = ImageType.xNone;
			}

			//Find the Image File Directory (IFD)

			if (FileType != ImageType.xNone) {
				//Note: The IFD may appearAfterthe payload.

				//First read the offset
				TIFFHeaderLen = Mult(bBuf[4], bBuf[5], bBuf[6], bBuf[7], NormalByteOrder);

				//Must seek end of payload.
				if (TIFFHeaderLen + 2 + TIFFFieldLen * 4 >= BufferedBytes) {

					FileOffset = TIFFHeaderLen;
					if (FileOffset < InStream.Length) {
						InStream.Position = FileOffset;
						BufferedBytes = InStream.Read(bBuf, 0, BUFFERSIZE);
						TIFFHeaderLen = 0;
					} else {
						FileType = ImageType.xNone;
					}

				}

				//Now find the Width tag, 
				TempField = 0;

				for (TempPos = TIFFHeaderLen + 2; TempPos <= BufferedBytes - 1; TempPos += TIFFFieldLen) {
					if (NormalByteOrder) {
						if (bBuf[TempPos] == TIFFWidthTag[0] && bBuf[TempPos + 1] == TIFFWidthTag[1]) {
							TempField = 1;
							break; // TODO: might not be correct. Was : Exit For
						}
					} else if (bBuf[TempPos] == TIFFWidthTagRev[0] && bBuf[TempPos + 1] == TIFFWidthTagRev[1]) {
						TempField = 1;
						break; // TODO: might not be correct. Was : Exit For
					}

				}

				if (TempField == 0)
					FileType = ImageType.xNone;

			}

			//Find the Width

			if (FileType != ImageType.xNone) {
				//Get Number Type
				TempField = Mult(bBuf[TempPos + 2], bBuf[TempPos + 3], NormalByteOrder);

				//Get Width Number
				if (TempField == 3) {
					Dimensions.Width = Mult(bBuf[TempPos + 8], bBuf[TempPos + 9], NormalByteOrder);
				} else if (TempField == 4) {
					Dimensions.Width = Mult(bBuf[TempPos + 8], bBuf[TempPos + 9], bBuf[TempPos + 10], bBuf[TempPos + 11], NormalByteOrder);
				} else {
					FileType = ImageType.xNone;
				}

			}

			//Find the Height

			if (FileType != ImageType.xNone) {
				TempPos += TIFFFieldLen;

				//Get Number Type
				TempField = Mult(bBuf[TempPos + 2], bBuf[TempPos + 3], NormalByteOrder);

				//Get Height Number
				if (TempField == 3) {
					Dimensions.Height = Mult(bBuf[TempPos + 8], bBuf[TempPos + 9], NormalByteOrder);
				} else if (TempField == 4) {
					Dimensions.Height = Mult(bBuf[TempPos + 8], bBuf[TempPos + 9], bBuf[TempPos + 10], bBuf[TempPos + 11], NormalByteOrder);
				} else {
					FileType = ImageType.xNone;
				}

			}

			//Find the Color Depth

			if (FileType != ImageType.xNone) {
				TempPos += TIFFFieldLen;

				//Check if BPP Field is Present, else BPP defaults to 1.
				ImageBPP = 0;
				if (NormalByteOrder) {
					if (!(bBuf[TempPos] == TIFFDepthTag[0] && bBuf[TempPos + 1] == TIFFDepthTag[1]))
						ImageBPP = 1;
				} else {
					if (!(bBuf[TempPos] == TIFFDepthTagRev[0] && bBuf[TempPos + 1] == TIFFDepthTagRev[1]))
						ImageBPP = 1;
				}


				if (ImageBPP == 0) {
					//Check count of BPP numbers
					TempField = Mult(bBuf[TempPos + 4], bBuf[TempPos + 5], bBuf[TempPos + 6], bBuf[TempPos + 7], NormalByteOrder);

					//Extract BPP numbers
					//Value(s) will fit in field.
					if (TempField <= 2) {
						ImageBPP = Mult(bBuf[TempPos + 8], bBuf[TempPos + 9], NormalByteOrder);
						if (TempField == 2) {
							ImageBPP += Mult(bBuf[TempPos + 10], bBuf[TempPos + 11], NormalByteOrder);
						}

					//Must get the file offset of the BPP numbers
					} else {
						TempPos = Mult(bBuf[TempPos + 8], bBuf[TempPos + 9], bBuf[TempPos + 10], bBuf[TempPos + 11], NormalByteOrder);


						if (TempPos > FileOffset + BufferedBytes - 2) {
							FileOffset = TempPos;
							if (FileOffset < InStream.Length) {
								InStream.Position = TempPos;
								BufferedBytes = InStream.Read(bBuf, 0, BUFFERSIZE);
							} else {
								FileType = ImageType.xNone;
							}

						}

						//Extract BPP Numbers
						for (int Counter = 0; Counter <= TempField - 1; Counter++) {
							ImageBPP += Mult(bBuf[TempPos + Counter], bBuf[TempPos + Counter + 1], NormalByteOrder);
						}
					}
				}
			}

			//Reset buffer
			if (FileOffset != 0 && FileType == ImageType.xNone) {
				FileOffset = 0;
				InStream.Position = FileOffset;
				BufferedBytes = InStream.Read(bBuf, 0, BUFFERSIZE);
			}

		}

		if (FileType == ImageType.xNone) {
			// if the file is not one of the above types then
			// check to see if it is a JPEG file

			NormalByteOrder = false;

			TempField = 0;
			TempPos = 0;
			while (TempField == 0 & TempPos < BufferedBytes - 10) {
				// loop through looking for the byte sequence FF,D8,FF
				// which marks the begining of a JPEG file
				// lPos will be left at the postion of the start
				if (bBuf[TempPos] == JPEGHeader[0] && bBuf[TempPos + 1] == JPEGHeader[1] && bBuf[TempPos + 2] == JPEGHeader[2])
					TempField = 1;

			}

			if (TempField == 1) {
				TempPos += 2;

				TempField = 0;
				while (TempField == 0 & FileOffset < InStream.Length) {
					// loop through the markers until we find the one 
					//starting with FF,C0 which is the block containing the 
					//image information

					while (!(bBuf[TempPos] == 0xff && bBuf[TempPos + 1] != 0xff)) {
						// loop until we find the beginning of the next marker
						TempPos += 1;
						if ((TempPos >= BufferedBytes - 10))
							goto ErrorOut;
					}

					// move pointer up
					TempPos += 1;

					switch (bBuf[TempPos]) {
						case 0xc0:
						case 193:
						case 194:
						case 195:
						case 0xc5:
						case 198:
						case 199:
						case 0xc9:
						case 202:
						case 203:
						case 0xcd:
						case 206:
						case 207:
							// we found the right block
							FileType = ImageType.JPEG;
							TempField = 1;

							break;
						default:
							//Found an Application Data Segment (or equiv), which must be parsed for its payload length and then skipped.
							TempPos += 1;
							//Corrects off-by-one error in original algorithm.
							TempPos += Mult(bBuf[TempPos], bBuf[TempPos + 1], NormalByteOrder);

							// check for end of buffer
							if (TempPos > BufferedBytes - 10) {
								FileOffset += TempPos;
								if (FileOffset < InStream.Length) {
									InStream.Position = FileOffset;
									BufferedBytes = InStream.Read(bBuf, 0, BUFFERSIZE);
									TempPos = 0;
								} else {
									TempField = 1;
									//Error Out
								}
							}
							break;
					}

				}

				if (FileType == ImageType.JPEG) {
					// get the height
					Dimensions.Height = Mult(bBuf[TempPos + 4], bBuf[TempPos + 5], NormalByteOrder);

					// get the width
					Dimensions.Width = Mult(bBuf[TempPos + 6], bBuf[TempPos + 7], NormalByteOrder);

					// get the color depth
					ImageBPP = bBuf[TempPos + 8] * 8;
				}
			}
		}
		ErrorOut:

		InStream.Close();

		if (FileType == ImageType.xNone) {
			//Err().Raise(531, "ImageParser", "Unable to decode image headers.");
            throw new Exception("Unable to decode image headers.");
            
		}

	}

	/// <summary>
	/// Coverts 2 Bytes into an Integer
	/// </summary>
	/// <param name="lsb">The least-significant byte</param>
	/// <param name="msb">The most-significant byte</param>
	/// <param name="NormalByteOrder">Must be set to False when providing parameters in reverse order.</param>
	/// <returns>32-bit Signed Integer</returns>
	/// <remarks>Parameters are in left-to-right (string) order.</remarks>
	private int Mult(byte lsb, byte msb, bool NormalByteOrder)
	{
		int functionReturnValue = 0;
		if (NormalByteOrder) {
			functionReturnValue = (Convert.ToInt32(msb) << 8) | Convert.ToInt32(lsb);
		} else {
			functionReturnValue = (Convert.ToInt32(lsb) << 8) | Convert.ToInt32(msb);
		}
		return functionReturnValue;
	}

	/// <summary>
	/// Coverts 4 Bytes into an Integer
	/// </summary>
	/// <param name="lsb">The least-significant byte</param>
	/// <param name="m2b">The middle-second byte</param>
	/// <param name="m3b">The middle-third byte</param>
	/// <param name="msb">The most-significant byte</param>
	/// <param name="NormalByteOrder">Must be set to False when providing parameters in reverse order.</param>
	/// <returns>32-bit Signed Integer</returns>
	/// <remarks>Parameters are in left-to-right (string) order.</remarks>
	private int Mult(byte lsb, byte m2b, byte m3b, byte msb, bool NormalByteOrder)
	{
		int functionReturnValue = 0;
		if (NormalByteOrder) {
			functionReturnValue = (Convert.ToInt32(msb) << 24) | (Convert.ToInt32(m3b) << 16) | (Convert.ToInt32(m2b) << 8) | Convert.ToInt32(lsb);
		} else {
			functionReturnValue = (Convert.ToInt32(lsb) << 24) | (Convert.ToInt32(m2b) << 16) | (Convert.ToInt32(m3b) << 8) | Convert.ToInt32(msb);
		}
		return functionReturnValue;
	}
    }
}
