// --------------------------------------------------------------------------------------------
#region // Copyright (c) 2009, SIL International. All Rights Reserved.
// <copyright file="PrintVia.cs" from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Dialog to enter the settings for printing Dictionaries from Flex (or orthers)
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using SIL.FieldWorks.Common.FwUtils;
using SIL.Tool;
using SIL.Tool.Localization;

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

        public string SelectedStyle = string.Empty;

        DataSet DataSetForGrid = new DataSet();

        private string _previewFileName1 = string.Empty;
        private string _previewFileName2 = string.Empty;
        private static string _helpTopic = string.Empty;
        private string _media;
        private string _path;
        public PreviewPrintVia()
        {
            InitializeComponent();
            _helpTopic = "User_Interface/Dialog_boxes/Print_via.htm";
        }
        public PreviewPrintVia(string media, string path)
        {
            InitializeComponent();
            _media = media;
            _path = path;
            _helpTopic = "User_Interface/Dialog_boxes/Set_Defaults_dialog_box.htm";
        }
        #endregion PrintVia Constructors

        private void PreviewPrintVia_Load(object sender, EventArgs e)
        {

            CreateColumn();

            DataRow row;
            XmlNodeList cats = Param.GetItems("//styles/" + _media + "/style"); // add condition for Standard
            int rowId = 0;
            int selectedRowId = 0;
            foreach (XmlNode xml in cats)
            {
                XmlAttribute name = xml.Attributes[AttribName];
                XmlAttribute shown = xml.Attributes[AttribShown];
                XmlAttribute previewFile1 = xml.Attributes[AttribPreviewFile1];
                XmlAttribute previewFile2 = xml.Attributes[AttribPreviewFile2];

                XmlNode xml1 = xml.SelectSingleNode(ElementDesc);
                string desc = string.Empty;
                if (xml1 != null)
                    desc = xml1.InnerText;

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

                if (name != null && name.Value == SelectedStyle)
                {
                    selectedRowId = rowId;
                }
                rowId++;

                DataSetForGrid.Tables["Styles"].Rows.Add(row);
            }
            grid.DataSource = DataSetForGrid.Tables["Styles"];
            grid.Columns[0].Width = 100;
            grid.Columns[1].Width = 198;
            grid.Refresh();

            if (grid.Columns.Count > 0)
            {
                grid.Columns[2].Visible = false; // Preview File 1
                grid.Columns[3].Visible = false; // Preview File 2     
            }

            if (grid.RowCount > 0)
            {
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
            if (grid.RowCount > 0)
            {
                try
                {
                    
                    int rowid = grid.SelectedCells[0].RowIndex;
                    string file1 = grid[2, rowid].Value.ToString();
                    _previewFileName1 = Common.PathCombine(_path, file1);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    ShowPreview(1);
                    SelectedStyle = grid[0, rowid].Value.ToString();
                    string file2 = grid[3, rowid].Value.ToString();
                    _previewFileName2 = Common.PathCombine(_path, file2);
                }
                catch
                {
                }
            }
        }

        private void ShowPreview(int page)
        {
            pictureBox1.Visible = false;
            string preview;

            if (page == 1)
            {
                preview = _previewFileName1;
                btnPrevious.Enabled = false;
                btnNext.Enabled = true;
            }
            else
            {
                preview = _previewFileName2;
                btnPrevious.Enabled = true;
                btnNext.Enabled = false;
            }

            if (File.Exists(preview))
            {
                pictureBox1.Visible = true;
                pictureBox1.Image = Image.FromFile(preview);
            }
          
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            ShowPreview(2);
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            ShowPreview(1);
        }
    }
}
  