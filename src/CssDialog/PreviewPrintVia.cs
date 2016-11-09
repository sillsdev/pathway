// --------------------------------------------------------------------------------------------
// <copyright file="PreviewPrintVia.cs" from='2009' to='2014' company='SIL International'>
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
// Dialog to enter the settings for printing Dictionaries from Flex (or orthers)
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using L10NSharp;
using SilTools;
using SIL.Tool;

namespace SIL.PublishingSolution
{
	public partial class PreviewPrintVia : Form
	{
		#region PrintVia Constructors
		public string AttribName = "name";
		public string ElementDesc = "description";
		public string AttribShown = "shown";
		public string AttribPreviewFile1 = "previewfile1";
		public string AttribPreviewFile2 = "previewfile2";
		public string AttribCSSName = "file";
		public string StyleName = "type";
		private bool _isUnixOs = false;
		public string SelectedStyle = string.Empty;

		DataSet DataSetForGrid = new DataSet();

		private string _previewFileName1 = string.Empty;
		private string _previewFileName2 = string.Empty;
		private static string _helpTopic = string.Empty;
		private bool _showEdit = false;
		private string _path;

		public string InputType;
		public PreviewPrintVia()
		{
			InitializeComponent();
			_helpTopic = "User_Interface/Dialog_boxes/Select_Layout_dialog_box.htm";
		}
		public PreviewPrintVia(string media, string path, bool showEdit)
		{
			InitializeComponent();
			_path = path;
			_helpTopic = "User_Interface/Dialog_boxes/Select_Layout_dialog_box.htm";
			_showEdit = showEdit;
		}
		#endregion PrintVia Constructors

		private void PreviewPrintVia_Load(object sender, EventArgs e)
		{
			_isUnixOs = Common.IsUnixOS();
			CreateColumn();
			LoadGridValues(sender);
			ShowHelp.ShowHelpTopic(this, _helpTopic, _isUnixOs, false);
			CreateToolTip();
			btnEdit.Visible = _showEdit;
			btnPrevious.Visible = false;
			btnNext.Visible = false;
			ShowPreview(1);
		}

		private void CreateToolTip()
		{
			ToolTip toolTip = new ToolTip();
			toolTip.ShowAlways = true;
			toolTip.SetToolTip(btnPrevious, "Show Page 1");
			toolTip.SetToolTip(btnNext, "Show Page 2");
			toolTip.SetToolTip(btnEdit, "Modify the Properties of the Selected Layout");
		}

		private void LoadGridValues(object sender)
		{
			if (grid.Rows.Count > 0)
			{
				DataSetForGrid.Tables["Styles"].Clear();
			}
			DataRow row;
			string xPathLayouts = "//styles/*/style[@approvedBy='GPS' or @shown='Yes']";
			XmlNodeList cats = Param.GetItems(xPathLayouts); // add condition for Standard
			int rowId = 0;
			int selectedRowId = 0;
			foreach (XmlNode xml in cats)
			{
				XmlAttribute name = xml.Attributes[AttribName];
				XmlAttribute shown = xml.Attributes[AttribShown];
				XmlAttribute previewFile1 = xml.Attributes[AttribPreviewFile1];
				XmlAttribute previewFile2 = xml.Attributes[AttribPreviewFile2];
				XmlAttribute cssFilename = xml.Attributes[AttribCSSName];
				XmlAttribute styleType = xml.Attributes[StyleName];

				string currentMedia = xml.ParentNode.Name;
				XmlNode xmlDesc = xml.SelectSingleNode(ElementDesc);
				string desc = string.Empty;
				if (xmlDesc != null)
					desc = xmlDesc.InnerText;

				string show = shown != null ? shown.Value : "Yes";
				if (show != "Yes")
				{
					continue;
				}

				row = DataSetForGrid.Tables["Styles"].NewRow();
				row["Name"] = name != null ? name.Value : string.Empty;
				row["Description"] = desc;
				row["previewFile1"] = previewFile1 != null && previewFile1.Value != null ? previewFile1.Value : string.Empty;
				row["previewFile2"] = previewFile2 != null && previewFile2.Value != null ? previewFile2.Value : string.Empty;
				row["mediaType"] = currentMedia;
				row["fileName"] = cssFilename != null && cssFilename.Value != null ? cssFilename.Value : string.Empty;
				row["styleType"] = styleType != null && styleType.Value != null ? styleType.Value : string.Empty;

				if (name != null && name.Value == SelectedStyle)
				{
					selectedRowId = rowId;
				}
				rowId++;

				DataSetForGrid.Tables["Styles"].Rows.Add(row);
			}
			grid.DataSource = DataSetForGrid.Tables["Styles"];
			grid.Columns[0].Width = 140;
			grid.Columns[1].Width = 265;
			grid.Columns[4].Visible = false;
			grid.Refresh();

			if (grid.Columns.Count > 0)
			{
				grid.Columns[2].Visible = false; // Preview File 1
				grid.Columns[3].Visible = false; // Preview File 2     
				grid.Columns[5].Visible = false; // CSS Filename     
				grid.Columns[6].Visible = false; // StyleType
			}
			grid.Columns[0].HeaderText = LocalizationManager.GetString("PreviewPrintVia.Column.Name", "Name");
			grid.Columns[1].HeaderText = LocalizationManager.GetString("PreviewPrintVia.Column.Description","Description");

			if (grid.RowCount > 0)
			{
				grid.ClearSelection();
				grid.Rows[selectedRowId].Selected = true;
				grid_RowEnter(sender, null);
			}

		}

		private void CreateColumn()
		{
			string tableName = "Styles";
			DataTable table = new DataTable(tableName);
			DataColumn column = new DataColumn
						{
							DataType = Type.GetType("System.String"),
							ColumnName = "Name",
							Caption = "Name",
							ReadOnly = false,
							Unique = false
						};
			table.Columns.Add(column);

			column = new DataColumn
						 {
							 DataType = Type.GetType("System.String"),
							 ColumnName = "Description",
							 Caption = "Description",
							 ReadOnly = false,
							 Unique = false
						 };
			table.Columns.Add(column);

			// Create column.
			column = new DataColumn
						 {
							 DataType = Type.GetType("System.String"),
							 ColumnName = "PreviewFile1",
							 Caption = "PreviewFile1",
							 ReadOnly = false,
							 Unique = false,
							 MaxLength = 150
						 };
			table.Columns.Add(column);

			// Create column.
			column = new DataColumn
						 {
							 DataType = Type.GetType("System.String"),
							 ColumnName = "PreviewFile2",
							 Caption = "PreviewFile2",
							 ReadOnly = false,
							 Unique = false,
							 MaxLength = 150
						 };
			table.Columns.Add(column);

			// Create column.
			column = new DataColumn
						 {
							 DataType = Type.GetType("System.String"),
							 ColumnName = "MediaType",
							 Caption = "MediaType",
							 ReadOnly = false,
							 Unique = false,
							 MaxLength = 150
						 };
			table.Columns.Add(column);

			// Create CSS Filename.
			column = new DataColumn
						 {
							 DataType = Type.GetType("System.String"),
							 ColumnName = "FileName",
							 Caption = "FileName",
							 ReadOnly = false,
							 Unique = false,
							 MaxLength = 150
						 };
			table.Columns.Add(column);

			// Create StyleType.
			column = new DataColumn
			{
				DataType = Type.GetType("System.String"),
				ColumnName = "StyleType",
				Caption = "StyleType",
				ReadOnly = false,
				Unique = false,
				MaxLength = 150
			};
			table.Columns.Add(column);

			DataSetForGrid.Tables.Add(table);
		}

		private void BtnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void BtnOk_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		private void grid_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (grid.RowCount > 0)
				{
					if (grid.SelectedCells.Count != 0)
					{
						int rowid = grid.SelectedCells[0].RowIndex;
						string file1 = grid[2, rowid].Value.ToString();
						_previewFileName1 = Common.PathCombine(_path, file1);
						pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
						SelectedStyle = grid[0, rowid].Value.ToString();
						string file2 = grid[3, rowid].Value.ToString();
						_previewFileName2 = Common.PathCombine(_path, file2);
					}
				}
			}
			catch
			{
			}
		}

		private void ShowPreview(int page)
		{
			pictureBox1.Visible = false;
			string preview;
			btnPrevious.Visible = true;
			btnNext.Visible = true;
			ShowPreviewMessage(string.Empty);
			if (page == 1)
			{
				if (grid.SelectedRows[0].Cells[6].Value.ToString().ToLower() == "custom")
				{
					ShowPreview(ref _previewFileName1);
					btnPrevious.Enabled = false;
					btnNext.Enabled = false;
				}
				else
				{
					btnPrevious.Enabled = false;
					btnNext.Enabled = true;
				}
				preview = _previewFileName1;
			}
			else
			{
				preview = _previewFileName2;
				btnPrevious.Enabled = true;
				btnNext.Enabled = false;
			}

			ShowPreviewMessage(preview);
		}

		private void ShowPreviewMessage(string preview)
		{
			if (File.Exists(preview))
			{
				lblPreview.Text = LocalizationManager.GetString("PreviewPrintVia.lblPreview", "Sample data in this layout:", "");
				pictureBox1.Visible = true;
				pictureBox1.Image = Image.FromFile(preview);
			}
			else if (grid.SelectedRows[0].Cells[6].Value.ToString().ToLower() == "custom" && (grid.SelectedRows[0].Cells[4].Value.ToString().ToLower() == "paper" || grid.SelectedRows[0].Cells[4].Value.ToString().ToLower() == "others"))
			{
				ShowImageForPreviewLayout("PreviewMessage.jpg", "Sample data for a custom stylesheet:");
			}
			else
			{
				ShowImageForPreviewLayout("NoPreview.jpg", "Sample data not available:");
			}
		}

		private void ShowImageForPreviewLayout(string imageFileName, string message)
		{
			string pathwayDirectory = Common.AssemblyPath;
			pathwayDirectory = Common.PathCombine(pathwayDirectory, "Styles");
			if (!Directory.Exists(pathwayDirectory))
			{
				pathwayDirectory = Path.GetDirectoryName(Common.AssemblyPath);
				pathwayDirectory = Common.PathCombine(pathwayDirectory, "Styles");
			}
			pathwayDirectory = Common.PathCombine(pathwayDirectory, Param.Value["InputType"]);
			pathwayDirectory = Common.PathCombine(pathwayDirectory, "Preview");
			string preview = Common.PathCombine(pathwayDirectory, imageFileName);
			if (File.Exists(preview))
			{
				pictureBox1.Visible = true;
				pictureBox1.Image = Image.FromFile(preview);
				btnPrevious.Enabled = false;
				btnNext.Enabled = false;
			}
			lblPreview.Text = message;
			btnPrevious.Visible = false;
			btnNext.Visible = false;
		}

		public void ShowPreview(ref string _previewFileName1)
		{
			try
			{
				CreatePreviewFile(ref _previewFileName1);
			}
			catch { }
		}

		private void CreatePreviewFile(ref string _previewFileName1)
		{
			try
			{
				string settingPath = Path.GetDirectoryName(Param.SettingPath);
				string inputPath = Common.PathCombine(settingPath, "Styles");
				inputPath = Common.PathCombine(inputPath, Param.Value["InputType"]);
				string stylenamePath = Common.PathCombine(inputPath, "Preview");
				string selectedTypeValue = grid.SelectedRows[0].Cells[6].Value.ToString();
				string _loadType = Param.Value["InputType"];// "Dictionary";
				if (selectedTypeValue != "standard")
				{
					bool isPreviewFileExist = File.Exists(_previewFileName1);

					if (isPreviewFileExist == false)
					{
						string FileName = grid.SelectedRows[0].Cells[5].Value.ToString();
						string cssMergeFullFileName = Param.StylePath(FileName);
						string PsSupportPath = Common.PathCombine(Common.LeftString(cssMergeFullFileName, "Pathway"),
															"Pathway");
						string PsSupportPathfrom = Common.GetApplicationPath();
						string previewFile = _loadType + "Preview.xhtml";
						string xhtmlPreviewFilePath = Common.PathCombine(PsSupportPath, previewFile);
						string xhtmlPreviewFile_fromPath = Common.PathCombine(PsSupportPathfrom, previewFile);
						if (!File.Exists(xhtmlPreviewFilePath))
						{
							if (File.Exists(xhtmlPreviewFile_fromPath))
							{
								File.Copy(xhtmlPreviewFile_fromPath, xhtmlPreviewFilePath);
							}
						}

						if (!(File.Exists(xhtmlPreviewFilePath) && File.Exists(cssMergeFullFileName)))
						{
							return;
						}

						PublicationInformation ps = new PublicationInformation();
						ps.DefaultXhtmlFileWithPath = xhtmlPreviewFilePath;
						ps.DefaultCssFileWithPath = cssMergeFullFileName;
						string fileName = Path.GetTempFileName();
						ps.ProjectName = fileName;
						ps.DictionaryOutputName = fileName;
						ps.DictionaryPath = Path.GetDirectoryName(xhtmlPreviewFilePath);
						ps.ProjectInputType = _loadType;

						bool success = PrincePreview(ps);

						if (!success)
						{
							success = LOPreview(ps);
						}

						if (!success)
						{
							var message = LocalizationManager.GetString("PreviewPrintVia.CreatePreviewFile.Message", "Sorry a preview of this stylesheet is not available. Please install PrinceXML or LibreOffice to enable the preview.", "");
							string caption = LocalizationManager.GetString("PreviewPrintVia.CreatePreviewFile.Caption", "Select Layout", "");
							Utils.MsgBox(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
						}

						_previewFileName1 = Common.PathCombine(stylenamePath, "PreviewMessage.jpg");

					}

				}
			}
			catch { }
		}

		private bool PrincePreview(PublicationInformation projInfo)
		{
			bool success = false;
            //TODO: REPLACE WITH CALL TO PATHWY EXPORT
            //ExportPdf exportPdf = new ExportPdf();
			//success = exportPdf.Export(projInfo);
			// copy to preview folder *******************
			return success;
		}

		private bool LOPreview(PublicationInformation projInfo)
		{
			bool success = false;
            //TODO: REPLACE WITH CALL TO PATHWY EXPORT
            //ExportLibreOffice openOffice = new ExportLibreOffice();
			//success = openOffice.Export(projInfo);
			return success;
		}

		private void btnNext_Click(object sender, EventArgs e)
		{
			if (Path.GetFileName(_previewFileName1) == "Preview.pdf1.jpg")
			{
				_previewFileName2 = _previewFileName1.Replace("pdf1", "pdf2");
			}
			ShowPreview(2);
		}

		private void btnPrevious_Click(object sender, EventArgs e)
		{
			ShowPreview(1);
		}

		private void BtnHelp_Click(object sender, EventArgs e)
		{
			ShowHelp.ShowHelpTopicKeyPress(this, _helpTopic, Common.IsUnixOS());
		}

		private void btnEdit_Click(object sender, EventArgs e)
		{
			Param.SetValue(Param.LayoutSelected, grid.SelectedRows[0].Cells[0].Value.ToString());
			Param.DefaultValue[Param.LayoutSelected] = grid.SelectedRows[0].Cells[0].Value.ToString();
			Param.Write();
			ConfigurationTool configurationTool = new ConfigurationTool();
			configurationTool.InputType = InputType;
			configurationTool.MediaType = grid.SelectedRows[0].Cells[4].Value.ToString();
			configurationTool.Style = grid.SelectedRows[0].Cells[0].Value.ToString().Replace(' ', '&');
			configurationTool.StartPosition = FormStartPosition.CenterScreen;
			configurationTool.IsExportOptionFromFlexOrParatext = true;
			configurationTool.ShowDialog();

			SelectedStyle = configurationTool.Style;

			Param.LoadSettings();
			LoadGridValues(sender);
		}

		private void PreviewPrintVia_KeyUp(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.F1)
				{
					ShowHelp.ShowHelpTopic(this, _helpTopic, Common.IsUnixOS(), true);
				}
			}
			catch { }
		}

		private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
		{

			var myCursor = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;

			if (grid.RowCount > 0)
			{
				try
				{
					ShowPreview(1);
				}
				catch
				{
				}
			}
			Cursor.Current = myCursor;

		}
	}
}
