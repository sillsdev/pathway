// --------------------------------------------------------------------------------------------
// <copyright file="StyleCategories.cs" from='2009' to='2014' company='SIL International'>
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
// Styles Category
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using JWTools;
using SIL.Tool;
using SIL.Tool.Localization;

namespace SIL.PublishingSolution
{
    public partial class StyleCategories : Form
    {
        public StyleCategories()
        {
            InitializeComponent();
        }

        private void StyleCategories_Load(object sender, EventArgs e)
        {
            LocDB.Localize(this, null);
            Param.SetupHelp(this);
            var cats = Param.GetItems("categories/category/item");
            DgStyleCategories.ColumnCount = cats.Count;
            DgStyleCategories.ColumnHeadersVisible = true;
            var hdrStyle = new DataGridViewCellStyle
                               {
                                   BackColor = Color.Gray,
                                   Font = new Font("Charis SIL", 8, FontStyle.Bold),
                                   WrapMode = DataGridViewTriState.True,
                                   Alignment = DataGridViewContentAlignment.MiddleCenter
                               };
            DgStyleCategories.ColumnHeadersDefaultCellStyle = hdrStyle;
            for (var i = 0; i < cats.Count; i++)
            {
                DgStyleCategories.Columns[i].Name = string.Format("{1}: {0}", cats[i].Attributes["name"].Value, cats[i].ParentNode.Attributes["name"].Value);
            }
            var styles = Param.GetItems("styles/paper/style");
            DgStyleCategories.RowCount = styles.Count;
            DgStyleCategories.RowHeadersVisible = true;
            var rhdrStyle = new DataGridViewCellStyle
                                {
                                    BackColor = Color.Gray,
                                    Font = new Font("Charis SIL", 8, FontStyle.Bold)
                                };
            DgStyleCategories.RowHeadersDefaultCellStyle = rhdrStyle;
            for (var i = 0; i < styles.Count; i++)
                DgStyleCategories.Rows[i].HeaderCell.Value = styles[i].Attributes["name"].Value;
            DgStyleCategories.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

            var windings = new Font("Wingdings", 10);
            var middleCenter = DataGridViewContentAlignment.MiddleCenter;

            DgStyleCategories.DefaultCellStyle.Font = windings;
            DgStyleCategories.DefaultCellStyle.Alignment = middleCenter;
            DgStyleCategories.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DataGridViewCell cell;
            for (var r = 0; r < styles.Count; r++)
            {
                for (var c = 0; c < cats.Count; c++)
                {
                    cell = DgStyleCategories.Rows[r].Cells[c];
                    if(cell.Value == null) continue;

                    cell.Value = Param.LoadStyleCategories(Param.Name(styles[r]), Param.Name(cats[c].ParentNode), Param.Name(cats[c]));
                    cell.Style.ForeColor = (string)cell.Value == YES ? Color.Green : Color.Red;
                }
            }
        }

        private void BtSave_Click(object sender, EventArgs e)
        {
            XmlNodeList cats = Param.GetItems("categories/category/item");
            for (var row = 0; row < DgStyleCategories.Rows.Count; row++)
                for (var col = 0; col < DgStyleCategories.Columns.Count; col++)
                {
                    Param.SaveStyleCategories((string)DgStyleCategories.Rows[row].HeaderCell.Value, cats[col].ParentNode.Attributes["name"].Value, cats[col].Attributes["name"].Value, (string)DgStyleCategories.Rows[row].Cells[col].Value);
                }
            Param.Write();
            Close();
        }

        private void BtCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DgStyleCategories_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            SetCellValue(DgStyleCategories.Rows[e.RowIndex].Cells[e.ColumnIndex]);
        }

        public const string YES = "\x78";
        public const string NO = "\x70";
        private static void SetCellValue(DataGridViewCell cell)
        {
            Debug.Assert(cell != null, "Null cell value");
            if ((string)cell.Value != NO)
            {
                cell.Value = NO;
                cell.Style.ForeColor = Color.Red;
            }
            else
            {
                cell.Value = YES;
                cell.Style.ForeColor = Color.Green;
            }
        }

        private void StyleCategories_DoubleClick(object sender, EventArgs e)
        {
#if DEBUG
            var dlg = new JWTools.Localizer(LocDB.DB);
            dlg.ShowDialog();
#endif
        }

        private void StyleCategories_Activated(object sender, EventArgs e)
        {
            Common.SetFont(this);
        }
    }
}
