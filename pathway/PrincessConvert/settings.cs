using System.Windows.Forms;
using Microsoft.VisualBasic;
using System;
using System.Data;
using System.IO;

namespace PrincessConvert
{
    public partial class settings : Form
    {
        		// enclose file names in quotes because of spaces
	public string quote = "\"";
	private string sPublication5x8in = "5in x 8in";
	private string sPublicationLetter = "Letter - 8.5in x 11in";
	private string sPublicationA4 = "A4 - 210mm x 297mm";
	//    Public dColumnWidthInPoints As Integer
	public string line;
	//public string[,] arrayClasses = new string[10001, 2]; // todo
    public string[,] arrayClasses = new string[100, 2];
	public string[] splitLine = new string[1];
	int iClassNumber = 0;
	int iCount = 1;
	int iClass = 0;
	int iType = 0;
    private string sTempFileName = string.Empty; // TODO Main.sDocumentFileName + "temp";
	string sTempTypeClass;
	// Create a new DataTable.
	public DataTable table = new DataTable("fileTable");
	// Declare variables for DataColumn and DataRow objects.
	public DataColumn column;
	public DataRow row;
	private DataSet dataSet;
	public settings()
	{
		//Load += settingsLoad;
		// This call is required by the Windows Form Designer.
		InitializeComponent();
		// Add any initializationAfterthe InitializeComponent() call.
	}       

	private void settingsLoad()
	{
		populateListBoxPageSize();
		NumericUpDownLineHeight.Value = Main.dLineHeightInPoints;
		NumericUpDownColumnGapInPoints.Value = Main.dColumnGapInPoints;
		NumericUpDownLeftMarginPoints.Value = Main.dMarginLeftInPoints;
		NumericUpDownRightMarginInPoints.Value = Main.dMarginRightInPoints;
		NumericUpDownTopMarginInPoints.Value = Main.dMarginTopInPoints;
		NumericUpDownBottomMarginInPoints.Value = Main.dMarginBottomInPoints;
		// illegal value
		if (Main.iNumberOfColumns == 0) {
			Main.iNumberOfColumns = 1;
		} else {
			// skip 
		}
		NumericUpDownNumberOfColumns.Value = Main.iNumberOfColumns;

		NumericUpDownLineHeight.Value = Main.dLineHeightInPoints;


		fillListBoxes();
		tbInputValueToConvert.Text = 1.5.ToString();
		lbInputUnits.SelectedItem = "cm";
		//    lbPageSize.SelectedItem = Main.sPageSelection

	}

	private void fillListBoxes()
	{
		lbInputUnits.Items.Add("pixels");
		lbInputUnits.Items.Add("points");
		lbInputUnits.Items.Add("pica");
		lbInputUnits.Items.Add("inches");
		lbInputUnits.Items.Add("mm");
		lbInputUnits.Items.Add("cm");
		lbLineHeightUnits.Items.Add("pixels");
		lbLineHeightUnits.Items.Add("points");
		lbLineHeightUnits.Items.Add("pica");
		lbLineHeightUnits.Items.Add("inches");
		lbLineHeightUnits.Items.Add("mm");
		lbColumnGapUnits.Items.Add("cm");
		lbColumnGapUnits.Items.Add("pixels");
		lbColumnGapUnits.Items.Add("points");
		lbColumnGapUnits.Items.Add("pica");
		lbColumnGapUnits.Items.Add("inches");
		lbColumnGapUnits.Items.Add("mm");
		lbColumnGapUnits.Items.Add("cm");
	}

	public void getClassInfo()
	{
		if (File.Exists(Main.sDocumentFileName)) {
			string sText = File.ReadAllText(Main.sDocumentFileName);
			// array  type, class, count

			sText = Main.regexReplace(sText, "<span", Constants.vbCrLf + "<span");
			sText = Main.regexReplace(sText, ">", ">" + Constants.vbCrLf);
			File.WriteAllText(sTempFileName, sText);

			StreamReader sr = new StreamReader(sTempFileName);
			while (!(sr.EndOfStream)) {
				line = sr.ReadLine();
				line = line.Trim();
				if (line.Contains("class=")) {
					// found matching line
					// identify class and p, div, span
					// getClassName(line)
					splitLine = line.Split(' ');
					foreach (string y_loopVariable in splitLine) {
						string y = y_loopVariable;
						if (y.StartsWith("class=")) {
							if (isThisClassNew(ref y)) {
								iClassNumber += 1;
								arrayClasses[iClassNumber, iType] = sTempTypeClass;
								arrayClasses[iClassNumber, iCount] = "1"; // todo test with input case

							} else {
							}
						}
					}
				} else {
					// no class found so try another line
					if (line.StartsWith("<")) {
						if (line.Contains("<!DOCTYPE") | line.StartsWith("</") | line.StartsWith("<html ") | line.StartsWith("<head>") | line.StartsWith("<head ") | line.StartsWith("<meta ") | line.StartsWith("<title ") | line.StartsWith("<br>") | line.StartsWith("<title>") | line.StartsWith("<style ") | line.StartsWith("<a ") | line.StartsWith("<link ")) {
							// skip
							sTempTypeClass = "";
						} else if (line.Contains(" ")) {
							sTempTypeClass = Strings.Mid(line, 1, Strings.InStr(line, " ",CompareMethod.Text));
						} else {
							sTempTypeClass = Strings.Mid(line, 1, Strings.InStr(line, ">",CompareMethod.Text));
						}
						if (isThisTypeNew(ref sTempTypeClass)) {
							iClassNumber += 1;
							arrayClasses[iClassNumber, iType] = sTempTypeClass;
							arrayClasses[iClassNumber, iCount] =  "1"; // todo test with input case// 1;
						} else {
							// skip
						}
					}
				}
			}
			makeDataTable();

			sr.Close();

		} else {
			// skip

		}

	}
	private bool isThisClassNew(ref string y)
	{
		string sType = null;
		string sClass = null;
		string sCurrentTypeClass = null;
		if (string.IsNullOrEmpty(y)) {
			return false;
		} else {
			sType = splitLine[iType].Replace("<", "").Trim();
			sClass = y.Replace("class=", "").Replace(">", "").Replace(Main.quote, "").Trim();
			sTempTypeClass = sType + "." + sClass;
			for (int i = 0; i <= iClassNumber; i++) {
				sCurrentTypeClass = arrayClasses[i, iType];
				if (sTempTypeClass == sCurrentTypeClass) {
					// class already found ... increment count
					arrayClasses[i, iCount] += 1;
					return false;
				}
			}
			return true;
		}
	}

	private bool isThisTypeNew(ref string sTempTypeClass)
	{
		string sType = null;
		string sCurrentTypeClass = null;
		if (string.IsNullOrEmpty(sTempTypeClass)) {
			return false;
		} else {
			// remove open and close wedge and trim spaces
			sType = sTempTypeClass.Replace("<", "").Replace(">", "").Trim();
			sTempTypeClass = sType;
			for (int i = 0; i <= iClassNumber; i++) {
				sCurrentTypeClass = arrayClasses[i, iType];
				if (sTempTypeClass == sCurrentTypeClass) {
					// class already found ... increment count
					arrayClasses[i, iCount] += 1;
					return false;
				}
			}
			return true;
		}
	}

	public void makeDataTable()
	{

		try {
			// Create new DataColumn, set DataType, ColumnName 
			// and add to DataTable.    
			DataColumn column = new DataColumn();

			column.DataType = System.Type.GetType("System.String");
			column.ColumnName = "Name";
			column.ReadOnly = true;
			column.Unique = false;

			// Add the Column to the DataColumnCollection.
			table.Columns.Add(column);

			// Create second column.
			column = new DataColumn();
			column.DataType = System.Type.GetType("System.Int16");
			column.ColumnName = "Count";
			column.AutoIncrement = false;
			column.Caption = "Count";
			column.ReadOnly = true;
			column.Unique = false;

			// Add the column to the table.
			table.Columns.Add(column);

			column = new DataColumn();
			column.DataType = System.Type.GetType("System.String");
			column.ColumnName = "Font name";
			column.ReadOnly = false;
			column.Unique = false;
			table.Columns.Add(column);

			column = new DataColumn();
			column.DataType = System.Type.GetType("System.String");
			column.ColumnName = "Font style";
			column.ReadOnly = true;
			column.Unique = false;
			table.Columns.Add(column);

			column = new DataColumn();
			column.DataType = System.Type.GetType("System.String");
			column.ColumnName = "Script";
			column.ReadOnly = true;
			column.Unique = false;
			table.Columns.Add(column);

			// Make the ID column the primary key column.
			DataColumn[] PrimaryKeyColumns = new DataColumn[1];
			PrimaryKeyColumns[0] = table.Columns["Name"];
			table.PrimaryKey = PrimaryKeyColumns;

			// Instantiate the DataSet variable.
			dataSet = new DataSet();

			// Add the new DataTable to the DataSet.
			dataSet.Tables.Add(table);
			DataGridView1.DataSource = table;

			DataGridViewComboBoxColumn comboBoxColumn1 = new DataGridViewComboBoxColumn();
			comboBoxColumn1.Items.AddRange(8, 8.5, 9, 9.5, 9.75, 10, 10.25, 10.5, 10.75, 11);
			comboBoxColumn1.ValueType = System.Type.GetType("System.Decimal");
			comboBoxColumn1.Name = "Font size";
			comboBoxColumn1.ReadOnly = false;
			//      comboBoxColumn.Unique = False
			DataGridView1.Columns.Add(comboBoxColumn1);
			column = new DataColumn();
			column.DataType = System.Type.GetType("System.String");
			column.ColumnName = "Font color";
			column.ReadOnly = true;
			column.Unique = false;
			table.Columns.Add(column);

			DataGridViewComboBoxColumn comboBoxColumnColor = new DataGridViewComboBoxColumn();
			comboBoxColumnColor.Items.AddRange(8, 8.5, 9, 9.5, 9.75, 10, 10.25, 10.5, 10.75, 11);
			comboBoxColumnColor.ValueType = System.Type.GetType("System.color");
			comboBoxColumnColor.Name = "Font size";
			comboBoxColumnColor.ReadOnly = false;
			//      comboBoxColumn.Unique = False
			DataGridView1.Columns.Add(comboBoxColumnColor);


			DataGridViewComboBoxColumn comboBoxColumn2 = new DataGridViewComboBoxColumn();
			comboBoxColumn2.Items.AddRange(1, 2, 3, 4);
			comboBoxColumn2.ValueType = System.Type.GetType("System.Int16");
			comboBoxColumn2.Name = "Columns";
			comboBoxColumn2.ReadOnly = false;
			//      comboBoxColumn.Unique = False
			DataGridView1.Columns.Add(comboBoxColumn2);
			DataGridView1.ShowCellErrors = false;
			DataGridView1.Show();
		} catch (Exception ex) {
			MessageBox.Show("data grid cell " + ex.Message);

		}

		addInfoToGrid();
		//Popup.dataGridViewHTMLfilesToProcess.DataSource = table
	}
	private void catchDataErrorEvent()
	{
		//  MessageBox.Show("data grid error ")
	}

	private void addInfoToGrid()
	{
		string sClassName = "";
		string iCountForClass = "";
		for (int i = 0; i <= iClassNumber; i++) {
			sClassName = arrayClasses[i, iType];
			iCountForClass = arrayClasses[i, iCount];
			addClassInfoToRow(sClassName, iCountForClass);
		}

	}
	private void addClassInfoToRow(string sClassName, string iCountForClass)
	{
		//dynamic temp = null;
		if (sClassName == null) {
		// skip
		} else {
			row = table.NewRow();
			row["Name"] = sClassName;
			row["Count"] = iCountForClass;
			//temp = table.Rows.IndexOf(row);
			if (table.Rows.IndexOf(row) == -1) {
				try {
					table.Rows.Add(row);
				} catch (Exception ex) {
					// can't add if not unique
				}
			} else {
				// already exists so skip
			}

		}

	}
	private void addFontInfoToRow(string sClassName, int iCountForClass)
	{
		//dynamic temp = null;
		if (sClassName == null) {
		// skip
		} else {
			//row = table. 
			DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			row["Font name"] = FontDialog1.Font;
			row["Font color"] = FontDialog1.Color;

			//temp = table.Rows.IndexOf(row);
			if (table.Rows.IndexOf(row) == -1) {
				try {
					table.Rows.Add(row);
				} catch (Exception ex) {
					// can't add if not unique
				}
			} else {
				// already exists so skip
			}

		}

	}

	private void btnFont_Click(System.Object sender, System.EventArgs e)
	{
		FontDialog1.ShowColor = true;
		FontDialog1.ShowEffects = true;
		FontDialog1.FontMustExist = true;
		FontDialog1.ShowDialog();

	}
	public void populateListBoxPageSize()
	{
		lbPageSize.Items.Add(sPublicationLetter);
		lbPageSize.Items.Add(sPublication5x8in);
		lbPageSize.Items.Add(sPublicationA4);

	}
	//'  cbLineSpacing.ValueTyp = System.Type.GetType("System.Decimal")
	//
	//   cbLineSpacing.Items.Add("10pt")
	//  cbLineSpacing.Items.Add("10.5pt")
	// cbLineSpacing.Items.Add("11pt")
	//  cbLineSpacing.Items.Add("11.5pt")
	// cbLineSpacing.Items.Add("12pt")
	//cbLineSpacing.Items.Add("12.5pt")
	// cbLineSpacing.Items.Add("13pt")

	// End Sub

	private void btnCancel_Click(System.Object sender, System.EventArgs e)
	{
		this.Close();

	}


	private void btnOK_Click(System.Object sender, System.EventArgs e)
	{
		// calculate and save line height and column width in points
		//   Main.dColumnWidthInPoints = picture.convertUnitsToPoints(Me.lbInputUnits.SelectedItem, Me.tbColumnWidth.Text)
		//  Main.sLineHeightInPoints = picture.convertUnitsToPoints(Me.lbLineHeightUnits.SelectedItem, Me.NumericUpDownLineHeight.Value)
        Main main = new Main();

		main.writeIniFile();
		this.Close();

	}

	private void lbColumnWidthUnits_SelectedIndexChanged(System.Object sender, System.EventArgs e)
	{
	}




	private void NumericUpDownNumberOfColumns_ValueChanged(System.Object sender, System.EventArgs e)
	{
		tbColumnWidthInPoints.Text = null;
	}



	private void NumericUpDownRightMarginInPoints_ValueChanged(System.Object sender, System.EventArgs e)
	{
		tbColumnWidthInPoints.Text = null;
	}

	private void NumericUpDownLeftMarginPoints_ValueChanged(System.Object sender, System.EventArgs e)
	{
		tbColumnWidthInPoints.Text = null;

	}

	private void NumericUpDownColumnGap_ValueChanged(System.Object sender, System.EventArgs e)
	{
		tbColumnWidthInPoints.Text = null;

	}

	private void btnCalculateColumnWidthInPoints_Click(System.Object sender, System.EventArgs e)
	{
		calculateColumnWidthInPoints();
	}
	private void calculateColumnWidthInPoints()
	{
		Main.iNumberOfColumns = (int)NumericUpDownNumberOfColumns.Value;
		Main.dColumnGapInPoints = NumericUpDownColumnGapInPoints.Value;
		if (NumericUpDownNumberOfColumns.Value < 2) {
			// if for some reason it thinks it is zero reset it to 1
			Main.iNumberOfColumns = 1;
			Main.dColumnGapInPoints = 0;
		} else {
			if (Main.dColumnGapInPoints < 3) {
				Main.dColumnGapInPoints = 3;
			} else {
				//skip
			}
		}
		// Main.iNumberOfColumns = NumericUpDownNumberOfColumns.Value
		// Main.dColumnGapInPoints = NumericUpDownColumnGapInPoints.Value
		NumericUpDownNumberOfColumns.Value = Main.iNumberOfColumns;
		NumericUpDownColumnGapInPoints.Value = Main.dColumnGapInPoints;
		Main.dMarginLeftInPoints = NumericUpDownLeftMarginPoints.Value;
		Main.dMarginRightInPoints = NumericUpDownRightMarginInPoints.Value;

		// If Main.iNumberOfColumns > 1 Then
		// If Main.dColumnGapInPoints < 3 Then
		// Main.dColumnGapInPoints = 3
		// Else
		// 'skip
		// End If
		// Else
		//Main.dColumnGapInPoints = 0
		//End If
		//NumericUpDownColumnGapInPoints.Value = Main.dColumnGapInPoints
		int dMarginsAndGap =(int)(Main.dColumnGapInPoints + Main.dMarginLeftInPoints + Main.dMarginRightInPoints);
		Main.dColumnWidthInPoints = (Main.dPageWidthInPoints - dMarginsAndGap) / Main.iNumberOfColumns;
		tbColumnWidthInPoints.Text = Main.dColumnWidthInPoints.ToString();
        Main main = new Main();

        main.writeIniFile();
	}
	private void calculateNumberOfLines()
	{
		Main.dMarginTopInPoints = NumericUpDownTopMarginInPoints.Value;
		Main.dMarginBottomInPoints = NumericUpDownBottomMarginInPoints.Value;
		if (Main.iNumberOfColumns > 1) {
			if (Main.dColumnGapInPoints < 3) {
				Main.dColumnGapInPoints = 3;
			} else {
				//skip
			}
		} else {
			Main.dColumnGapInPoints = 0;
		}
		NumericUpDownColumnGapInPoints.Value = Main.dColumnGapInPoints;
		Main.dColumnWidthInPoints = (Main.dPageWidthInPoints - (Main.dColumnGapInPoints + Main.dMarginLeftInPoints + Main.dMarginRightInPoints)) / Main.iNumberOfColumns;
		tbColumnWidthInPoints.Text = Main.dColumnWidthInPoints.ToString();
        Main main = new Main();
        main.writeIniFile();
	}

	private void lbPageSize_SelectedIndexChanged(System.Object sender, System.EventArgs e)
	{
		// Select lbPageSize.SelectedItem
		//     Case sPublication5x8in
		// Main.sPageSelection = sPublication5x8in
		// Main.dPageWidthInPoints = 5 * 72
		// tbPageWidthInPoints.Text = Main.dPageWidthInPoints
		// Main.dPageHeightInPoints = 8 * 72
		// tbPageHeightInPoints.Text = Main.dPageHeightInPoints
		//     Case Else
		// End Select
		//2009-08-05
	}

	private void btnConvert_Click(System.Object sender, System.EventArgs e)
	{
		//tbConvertedValue.Text = picture.convertUnitsToPoints(this.lbInputUnits.SelectedItem, tbInputValueToConvert.Text);
        //Todo after picture form
	}


	private void btnCalculateNumberOfLines_Click(System.Object sender, System.EventArgs e)
	{
		Main.dLineHeightInPoints = (int)NumericUpDownLineHeight.Value;
        Main.dMarginTopInPoints = (int)NumericUpDownTopMarginInPoints.Value;
		Main.dMarginBottomInPoints = NumericUpDownBottomMarginInPoints.Value;
        Main.dPageHeightInPoints = decimal.Parse(tbPageHeightInPoints.Text);
		int dMargins = (int) (Main.dMarginTopInPoints + Main.dMarginBottomInPoints);
		Main.sColumnHeightInPoints = (Main.dPageHeightInPoints - dMargins).ToString();
		Main.sColumnHeightInLines = (decimal.Parse(Main.sColumnHeightInPoints) / Main.dLineHeightInPoints).ToString();
		tbColumnHeightInLines.Text = Main.sColumnHeightInLines;
        Main main = new Main();
		main.writeIniFile();
	}
      
    }
}
