// --------------------------------------------------------------------------------------------
// <copyright file="SymbolMap.cs" from='2009' to='2014' company='SIL International'>
//      Copyright © 2014, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Inserts the Symbol of the given font name
// </remarks>
// --------------------------------------------------------------------------------------------

#region using
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Globalization;
using JWTools;
using SIL.Tool;
using SIL.Tool.Localization;

#endregion

namespace SIL.PublishingSolution
{
    /// <summary>
    /// SymbolMap Form
    /// </summary>
    public partial class SymbolMap : Form
    {
        #region Private variable

        /// <summary>
        /// To store the Symbols textbox (DictionarySettings/Senses Tab) Font Name
        /// </summary>
        private readonly string textBoxfontName;

        /// <summary>
        /// To store the symbol and the fontname to the DictionarySetting public variable
        /// </summary>
        private readonly DictionarySetting parent;

        /// <summary>
        /// Counter for symbols
        /// </summary>
        private int cellCount;

        /// <summary>
        /// To get cellXPosition of the selected cell
        /// </summary>
        private int cellXPosition;

        /// <summary>
        /// To get cellYPosition of the selected cell
        /// </summary>
        private int cellYPosition;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the SymbolMap class.
        /// </summary>   
        /// <param name="objDS">Object of DictionarySetting form</param>
        /// <param name="fontName">Symbol Text box Font Name</param>
        public SymbolMap(DictionarySetting objDS,string fontName)
        {
            InitializeComponent();
            parent = objDS;
            textBoxfontName = fontName;
        }
        #endregion

        #region Load Function
        /// <summary>
        /// SymbolMap Load Event
        /// </summary>
        /// <param name="sender">SymbolMap form</param>
        /// <param name="e">Event Args</param>
        private void SymbolMap_Load(object sender, EventArgs e)
        {
            LocDB.Localize(this, null);     // Form Controls
            txtCopySymbols.Text = string.Empty;
            parent.SymbolValue = string.Empty;
            parent.FontName = string.Empty;
            LoadFontNames();
            ConvertNumericToUnicode();
            LoadSymbolsToGrid();
            toolStripStatusLabel1.Text = ConvertStringToUnicode(lblDisplaySymbol.Text).Replace("\\", "U+");
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// To load symbols each time in the SymbolViewer based on 10x20
        /// </summary>
        private void LoadSymbolsToGrid()
        {
            SymbolViewer.ColumnCount = 20;
            SymbolViewer.RowCount = 11;
            for (int i = 0; i < SymbolViewer.RowCount; i++)
            {
                for (int j = 0; j < SymbolViewer.ColumnCount; j++)
                {
                    if (cellCount < listBox1.Items.Count)
                    {
                        var regex = new Regex(@"\\[uU]([0-9A-F]{4})", RegexOptions.IgnoreCase);
                        string line = regex.Replace(listBox1.Items[cellCount].ToString(),
                                                    match =>
                                                    ((char)
                                                     Int32.Parse(match.Value.Substring(2), NumberStyles.HexNumber)).
                                                        ToString());
                        SymbolViewer.Rows[i].Cells[j].Value = line;
                        cellCount++;
                    }
                }
            }
            lblDisplaySymbol.Text = SymbolViewer.Rows[0].Cells[0].Value.ToString();
            if (lblDisplaySymbol.Text.Trim() == "&")
            {
                lblDisplaySymbol.Text += "&";
            }
        }

        /// <summary>
        /// To load installed fontnames into the combobox.
        /// </summary>
        private void LoadFontNames()
        {
            var fonts = new System.Drawing.Text.InstalledFontCollection();
            CmbFonts.Text = "Arial";
            foreach (FontFamily family in fonts.Families)
            {
                CmbFonts.Items.Add(family.Name);
                if (family.Name == textBoxfontName)
                {
                    CmbFonts.Text = textBoxfontName;
                }
            }
        }

        /// <summary>
        /// To set the fontname to the control to avoid non-support fonts.
        /// </summary>
        /// <param name="ctl">Control to set new fontname</param>
        /// <param name="size">Control to set new size</param>
        private void SetFontToControl(Control ctl, float size)
        {
            string fontname = CmbFonts.Text;
            try
            {
                ctl.Font = new Font(fontname, size);
            }
            catch
            {
                ctl.Font = new Font("Charis SIL", size);
            }

            ctl.ForeColor = Color.Black;
        }

        /// <summary>
        /// To load symbol to the label based on logic
        /// </summary>
        /// <param name="fromInput">from events like mouseDown /KeyLeft /KeyRight / KeyUp / KeyDown/ KeyEnter </param>
        private void LoadSymbolToLabel(string fromInput)
        {
            SetFontToControl(lblDisplaySymbol, 24.0F);
            SetFontToControl(txtCopySymbols, 12.0F);
            DataGridView.HitTestInfo hit = SymbolViewer.HitTest(cellXPosition, cellYPosition);
            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                if (fromInput == "Left")
                {
                    // from MouseDown Event
                    lblDisplaySymbol.Text = SymbolViewer.Rows[hit.RowIndex].Cells[hit.ColumnIndex].Value.ToString();
                }
                else if (fromInput == "37")
                {
                    // from KeyLeft Event
                    if (SymbolViewer.CurrentCell.ColumnIndex > 0)
                    {
                        lblDisplaySymbol.Text = SymbolViewer.Rows[SymbolViewer.CurrentCell.RowIndex].Cells[SymbolViewer.CurrentCell.ColumnIndex - 1].Value.ToString();
                    }
                    else if (SymbolViewer.CurrentCell.RowIndex != 0)
                    {
                        SymbolViewer.CurrentCell.Selected = false;
                        SymbolViewer.Rows[SymbolViewer.CurrentCell.RowIndex - 1].Cells[SymbolViewer.CurrentCell.ColumnIndex + (SymbolViewer.ColumnCount - 1)].Selected = true;
                        SymbolViewer.CurrentCell = SymbolViewer.SelectedCells[0];
                        lblDisplaySymbol.Text = SymbolViewer.Rows[SymbolViewer.CurrentCell.RowIndex].Cells[SymbolViewer.CurrentCell.ColumnIndex].Value.ToString();
                    }
                }
                else if (fromInput == "39")
                {
                    // from KeyRight Event
                    if (SymbolViewer.CurrentCell.ColumnIndex < SymbolViewer.ColumnCount - 1)
                    {
                        lblDisplaySymbol.Text = SymbolViewer.Rows[SymbolViewer.CurrentCell.RowIndex].Cells[SymbolViewer.CurrentCell.ColumnIndex + 1].Value.ToString();
                    }
                    else if (SymbolViewer.CurrentCell.RowIndex != (SymbolViewer.RowCount - 1))
                    {
                        SymbolViewer.CurrentCell.Selected = false;
                        SymbolViewer.Rows[SymbolViewer.CurrentCell.RowIndex + 1].Cells[0].Selected = true;
                        SymbolViewer.CurrentCell = SymbolViewer.SelectedCells[0];
                        lblDisplaySymbol.Text = SymbolViewer.Rows[SymbolViewer.CurrentCell.RowIndex].Cells[SymbolViewer.CurrentCell.ColumnIndex].Value.ToString();
                    }
                }
                else if (fromInput == "38" && SymbolViewer.CurrentCell.RowIndex > 0)
                {
                    // from KeyUp Event
                    lblDisplaySymbol.Text = SymbolViewer.Rows[SymbolViewer.CurrentCell.RowIndex - 1].Cells[SymbolViewer.CurrentCell.ColumnIndex].Value.ToString();
                }
                else if (fromInput == "40" && SymbolViewer.CurrentCell.RowIndex < (SymbolViewer.RowCount - 1))
                {
                    // from KeyDown Event
                    lblDisplaySymbol.Text = SymbolViewer.Rows[SymbolViewer.CurrentCell.RowIndex + 1].Cells[SymbolViewer.CurrentCell.ColumnIndex].Value.ToString();
                }
                else if (fromInput == "13")
                {
                    // from KeyEnter Event
                    txtCopySymbols.Text += SymbolViewer.Rows[SymbolViewer.CurrentCell.RowIndex].Cells[SymbolViewer.CurrentCell.ColumnIndex].Value.ToString();
                }

                toolStripStatusLabel1.Text = ConvertStringToUnicode(lblDisplaySymbol.Text).Replace("\\", "U+");
                if (lblDisplaySymbol.Text.Trim() == "&")
                {
                    lblDisplaySymbol.Text += "&";
                }
            }
        }
        #endregion

        #region Convert Functions
        /// <summary>
        /// To Convert the string / symbol to unicode value
        /// </summary>
        /// <param name="inputString">String value for example "‡"</param>
        /// <returns>unicode Value</returns>
        private static string ConvertStringToUnicode(string inputString)
        {
            string unicode = string.Empty;
            System.Globalization.TextElementEnumerator enumerator =
                          StringInfo.GetTextElementEnumerator(inputString);
            while (enumerator.MoveNext())
            {
                string textElement = enumerator.GetTextElement();
                int index = Char.ConvertToUtf32(textElement, 0);
                if (string.Format("{0:X}", index).Length == 2)
                {
                    unicode = "\\" + "00" + string.Format("{0:X}", index);
                }
                else if (string.Format("{0:X}", index).Length == 3)
                {
                    unicode = "\\" + "0" + string.Format("{0:X}", index);
                }
                else if (string.Format("{0:X}", index).Length == 4)
                {
                    unicode = "\\" + string.Format("{0:X}", index);
                }
            }
            return unicode;
        }

        /// <summary>
        /// To Convert the unicode value to string / symbol
        /// </summary>
        private void ConvertNumericToUnicode()
        {
            string hexValue = string.Empty;
            for (long i = 33; i < 65535; i++)
            {
                if (string.Format("{0:X}", i).Length == 2)
                {
                    hexValue = "\\u" + "00" + string.Format("{0:X}", i);
                }
                else if (string.Format("{0:X}", i).Length == 3)
                {
                    hexValue = "\\u" + "0" + string.Format("{0:X}", i);
                }
                else if (string.Format("{0:X}", i).Length == 4)
                {
                    hexValue = "\\u" + string.Format("{0:X}", i);
                }
                listBox1.Items.Add(hexValue);
            }
        }

        /// <summary>
        /// To Convert the unicode value to integer
        /// </summary>
        /// <param name="unicode">Unicode value to convert integer</param>
        public void ConvertUnicodeToNumeric(string unicode)
        {
            try
            {
                cellCount = (Int32.Parse(unicode, NumberStyles.AllowHexSpecifier) - 33);
            }
            catch
            {
                cellCount = 33;
            }
            LoadSymbolsToGrid();
            SymbolViewer.CurrentCell.Selected = false;
            SymbolViewer.Rows[0].Cells[0].Selected = true;
            SymbolViewer.CurrentCell = SymbolViewer.SelectedCells[0];
        }

        #endregion

        #region Events
        /// <summary>
        /// To add the selected symbol to the textbox and to show the Unicode for the selected symbol in statusstrip.
        /// </summary>
        /// <param name="sender">SymbolViewer Control</param>
        /// <param name="e">Event Args</param>
        private void SymbolViewer_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string symbol = Convert.ToString(SymbolViewer.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
            SetFontToControl(txtCopySymbols, 12.0F);
            txtCopySymbols.Text += symbol;
            toolStripStatusLabel1.Text = ConvertStringToUnicode(symbol).Replace("\\", "U+");
        }

        /// <summary>
        /// To get the position of the selected cell and show the selected symbol to label.
        /// </summary>
        /// <param name="sender">SymbolViewer control</param>
        /// <param name="e">Event Args</param>
        private void SymbolViewer_MouseDown(object sender, MouseEventArgs e)
        {
            cellXPosition = e.X;
            cellYPosition = e.Y;
            LoadSymbolToLabel(e.Button.ToString());
        }

        /// <summary>
        /// To allow to move next / previous cell using keyboard arrow keys
        /// </summary>
        /// <param name="sender">SymbolViewer Control</param>
        /// <param name="e">Event Args</param>
        private void SymbolViewer_KeyDown(object sender, KeyEventArgs e)
        {
            LoadSymbolToLabel(e.KeyValue.ToString());
        }

        /// <summary>
        /// To load the symbols to the SymbolViewer based on scroll move.
        /// </summary>
        /// <param name="sender">Scroll Bar</param>
        /// <param name="e">Event Args</param>
        private void SViewerScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            cellCount = SViewerScrollBar.Value;
            LoadSymbolsToGrid();
        }

        /// <summary>
        /// To validate the given search value is valid unicode format or not.
        /// </summary>
        /// <param name="sender">Validation Event calls</param>
        /// <param name="e">Event Args</param>
        private void TxtUnicodeSearch_Validated(object sender, EventArgs e)
        {
            bool isValidUnicode = true;
            if (TxtUnicodeSearch.Text.Length == 3)
            {
                TxtUnicodeSearch.Text = "0" + TxtUnicodeSearch.Text;
            }
            else if (TxtUnicodeSearch.Text.Length == 2)
            {
                if (Int32.Parse(TxtUnicodeSearch.Text) < 33)
                    TxtUnicodeSearch.Text = "21";
                TxtUnicodeSearch.Text = "00" + TxtUnicodeSearch.Text;
            }
            else if (TxtUnicodeSearch.Text.Length <= 1)
            {
                isValidUnicode = false;
            }
            if (isValidUnicode && Regex.IsMatch(TxtUnicodeSearch.Text.ToUpper(), @"[0-9A-F]{4}"))
            {
                lblDisplaySymbol.Text = string.Empty;
                ConvertUnicodeToNumeric(TxtUnicodeSearch.Text.ToUpper());
                lblDisplaySymbol.Text = SymbolViewer.Rows[0].Cells[0].Value.ToString();
                toolStripStatusLabel1.Text = "U+" + TxtUnicodeSearch.Text;
                if (lblDisplaySymbol.Text.Trim() == "&")
                {
                    lblDisplaySymbol.Text += "&";
                }
            }
            else
            {
                SymbolViewer.CurrentCell.Selected = false;
                lblDisplaySymbol.Text = string.Empty;
                toolStripStatusLabel1.Text = string.Empty;
            }
        }

        /// <summary>
        /// To add text into Clipboard and close the Symbol Map.
        /// </summary>
        /// <param name="sender">Button control</param>
        /// <param name="e">Event Args</param>
        private void BtnClose_Click(object sender, EventArgs e)
        {
            parent.SymbolValue = txtCopySymbols.Text;
            parent.FontName = CmbFonts.Text;
            Close();
        }

        /// <summary>
        /// To load the SymbolViewer based on change in fonts selection.
        /// </summary>
        /// <param name="sender">ComboBox Control</param>
        /// <param name="e">Event Args</param>
        private void CmbFonts_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetFontToControl(SymbolViewer, 12.0F);
            LoadSymbolToLabel(string.Empty);
        }

        /// <summary>
        /// To show the advance option when the checkbox is ON else clear the content of the Textbox
        /// </summary>
        /// <param name="sender">CheckBox Control</param>
        /// <param name="e">Event Args</param>
        private void ChkAdvanceOptions_CheckStateChanged(object sender, EventArgs e)
        {
            if (ChkAdvanceOptions.Checked)
            {
                pnlAdvancedOptions.Enabled = true;
                TxtUnicodeSearch.Focus();
            }
            else
            {
                pnlAdvancedOptions.Enabled = false;
                TxtUnicodeSearch.Text = string.Empty;
            }
        }
        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            parent.FontName = CmbFonts.Text;
            Close();
        }

        private void SymbolMap_FormClosed(object sender, FormClosedEventArgs e)
        {
            parent.FontName = CmbFonts.Text;
        }

        private void SymbolMap_DoubleClick(object sender, EventArgs e)
        {
#if DEBUG
            var dlg = new Localizer(LocDB.DB);
            dlg.ShowDialog();
#endif
        }

        private void SymbolMap_Activated(object sender, EventArgs e)
        {
            Common.SetFont(this);
        }
    }
}
