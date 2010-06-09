namespace SIL.PublishingSolution
{
    partial class SymbolMap
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlAdvancedOptions = new System.Windows.Forms.Panel();
            this.TxtUnicodeSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.ChkAdvanceOptions = new System.Windows.Forms.CheckBox();
            this.SViewerScrollBar = new System.Windows.Forms.VScrollBar();
            this.lblDisplaySymbol = new System.Windows.Forms.Label();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.BtnClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSymbolToCopy = new System.Windows.Forms.Label();
            this.txtCopySymbols = new System.Windows.Forms.TextBox();
            this.SymbolViewer = new System.Windows.Forms.DataGridView();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.CmbFonts = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlAdvancedOptions.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SymbolViewer)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlAdvancedOptions
            // 
            this.pnlAdvancedOptions.AccessibleName = "pnlAdvancedOptions";
            this.pnlAdvancedOptions.AutoSize = true;
            this.pnlAdvancedOptions.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlAdvancedOptions.Controls.Add(this.TxtUnicodeSearch);
            this.pnlAdvancedOptions.Controls.Add(this.lblSearch);
            this.pnlAdvancedOptions.Enabled = false;
            this.pnlAdvancedOptions.Location = new System.Drawing.Point(12, 393);
            this.pnlAdvancedOptions.Name = "pnlAdvancedOptions";
            this.pnlAdvancedOptions.Size = new System.Drawing.Size(568, 35);
            this.pnlAdvancedOptions.TabIndex = 31;
            // 
            // TxtUnicodeSearch
            // 
            this.TxtUnicodeSearch.AccessibleName = "TxtUnicodeSearch";
            this.TxtUnicodeSearch.Location = new System.Drawing.Point(127, 6);
            this.TxtUnicodeSearch.MaxLength = 4;
            this.TxtUnicodeSearch.Name = "TxtUnicodeSearch";
            this.TxtUnicodeSearch.Size = new System.Drawing.Size(100, 20);
            this.TxtUnicodeSearch.TabIndex = 5;
            this.TxtUnicodeSearch.Validated += new System.EventHandler(this.TxtUnicodeSearch_Validated);
            // 
            // lblSearch
            // 
            this.lblSearch.AccessibleName = "lblSearch";
            this.lblSearch.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearch.ForeColor = System.Drawing.Color.Black;
            this.lblSearch.Location = new System.Drawing.Point(-2, 9);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(123, 13);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Search Unicode";
            this.lblSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ChkAdvanceOptions
            // 
            this.ChkAdvanceOptions.AccessibleName = "ChkAdvanceOptions";
            this.ChkAdvanceOptions.AutoSize = true;
            this.ChkAdvanceOptions.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChkAdvanceOptions.ForeColor = System.Drawing.Color.Black;
            this.ChkAdvanceOptions.Location = new System.Drawing.Point(12, 370);
            this.ChkAdvanceOptions.Name = "ChkAdvanceOptions";
            this.ChkAdvanceOptions.Size = new System.Drawing.Size(129, 17);
            this.ChkAdvanceOptions.TabIndex = 24;
            this.ChkAdvanceOptions.Text = "Advanced Options";
            this.ChkAdvanceOptions.UseVisualStyleBackColor = true;
            this.ChkAdvanceOptions.CheckStateChanged += new System.EventHandler(this.ChkAdvanceOptions_CheckStateChanged);
            // 
            // SViewerScrollBar
            // 
            this.SViewerScrollBar.AccessibleName = "SViewerScrollBar";
            this.SViewerScrollBar.Cursor = System.Windows.Forms.Cursors.Default;
            this.SViewerScrollBar.LargeChange = 200;
            this.SViewerScrollBar.Location = new System.Drawing.Point(563, 37);
            this.SViewerScrollBar.Maximum = 65502;
            this.SViewerScrollBar.Name = "SViewerScrollBar";
            this.SViewerScrollBar.Size = new System.Drawing.Size(17, 252);
            this.SViewerScrollBar.SmallChange = 200;
            this.SViewerScrollBar.TabIndex = 30;
            this.SViewerScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.SViewerScrollBar_Scroll);
            // 
            // lblDisplaySymbol
            // 
            this.lblDisplaySymbol.AccessibleName = "lblDisplaySymbol";
            this.lblDisplaySymbol.BackColor = System.Drawing.Color.Transparent;
            this.lblDisplaySymbol.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblDisplaySymbol.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblDisplaySymbol.Location = new System.Drawing.Point(12, 301);
            this.lblDisplaySymbol.Name = "lblDisplaySymbol";
            this.lblDisplaySymbol.Size = new System.Drawing.Size(60, 60);
            this.lblDisplaySymbol.TabIndex = 29;
            this.lblDisplaySymbol.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AccessibleName = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // statusStrip1
            // 
            this.statusStrip1.AccessibleName = "statusStrip1";
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 444);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(594, 22);
            this.statusStrip1.TabIndex = 28;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // BtnClose
            // 
            this.BtnClose.AccessibleName = "BtnClose";
            this.BtnClose.Location = new System.Drawing.Point(424, 301);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(75, 23);
            this.BtnClose.TabIndex = 23;
            this.BtnClose.Text = "&Close";
            this.BtnClose.UseVisualStyleBackColor = true;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // label1
            // 
            this.label1.AccessibleName = "label1";
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(0, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "Font";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSymbolToCopy
            // 
            this.lblSymbolToCopy.AccessibleName = "lblSymbolToCopy";
            this.lblSymbolToCopy.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSymbolToCopy.ForeColor = System.Drawing.Color.Black;
            this.lblSymbolToCopy.Location = new System.Drawing.Point(122, 306);
            this.lblSymbolToCopy.Name = "lblSymbolToCopy";
            this.lblSymbolToCopy.Size = new System.Drawing.Size(140, 13);
            this.lblSymbolToCopy.TabIndex = 26;
            this.lblSymbolToCopy.Text = "Symbols to copy";
            this.lblSymbolToCopy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCopySymbols
            // 
            this.txtCopySymbols.AccessibleName = "txtCopySymbols";
            this.txtCopySymbols.Location = new System.Drawing.Point(264, 303);
            this.txtCopySymbols.Name = "txtCopySymbols";
            this.txtCopySymbols.Size = new System.Drawing.Size(150, 20);
            this.txtCopySymbols.TabIndex = 22;
            // 
            // SymbolViewer
            // 
            this.SymbolViewer.AccessibleName = "SymbolViewer";
            this.SymbolViewer.AllowUserToAddRows = false;
            this.SymbolViewer.AllowUserToDeleteRows = false;
            this.SymbolViewer.AllowUserToOrderColumns = true;
            this.SymbolViewer.AllowUserToResizeColumns = false;
            this.SymbolViewer.AllowUserToResizeRows = false;
            this.SymbolViewer.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.SymbolViewer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.SymbolViewer.ColumnHeadersVisible = false;
            this.SymbolViewer.Location = new System.Drawing.Point(12, 41);
            this.SymbolViewer.MultiSelect = false;
            this.SymbolViewer.Name = "SymbolViewer";
            this.SymbolViewer.ReadOnly = true;
            this.SymbolViewer.RowHeadersVisible = false;
            this.SymbolViewer.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.SymbolViewer.ShowCellErrors = false;
            this.SymbolViewer.ShowRowErrors = false;
            this.SymbolViewer.Size = new System.Drawing.Size(550, 252);
            this.SymbolViewer.TabIndex = 21;
            this.SymbolViewer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SymbolViewer_MouseDown);
            this.SymbolViewer.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.SymbolViewer_CellContentDoubleClick);
            this.SymbolViewer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SymbolViewer_KeyDown);
            // 
            // listBox1
            // 
            this.listBox1.AccessibleName = "listBox1";
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(444, 41);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(120, 251);
            this.listBox1.TabIndex = 25;
            this.listBox1.Visible = false;
            // 
            // CmbFonts
            // 
            this.CmbFonts.AccessibleName = "CmbFonts";
            this.CmbFonts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbFonts.FormattingEnabled = true;
            this.CmbFonts.Location = new System.Drawing.Point(49, 12);
            this.CmbFonts.Name = "CmbFonts";
            this.CmbFonts.Size = new System.Drawing.Size(314, 21);
            this.CmbFonts.TabIndex = 20;
            this.CmbFonts.SelectedIndexChanged += new System.EventHandler(this.CmbFonts_SelectedIndexChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleName = "btnCancel";
            this.btnCancel.Location = new System.Drawing.Point(505, 301);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 32;
            this.btnCancel.Text = "C&ancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // SymbolMap
            // 
            this.AccessibleName = "SymbolMap";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 466);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.pnlAdvancedOptions);
            this.Controls.Add(this.ChkAdvanceOptions);
            this.Controls.Add(this.SViewerScrollBar);
            this.Controls.Add(this.lblDisplaySymbol);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.BtnClose);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblSymbolToCopy);
            this.Controls.Add(this.txtCopySymbols);
            this.Controls.Add(this.SymbolViewer);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.CmbFonts);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SymbolMap";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Symbol Map";
            this.Load += new System.EventHandler(this.SymbolMap_Load);
            this.DoubleClick += new System.EventHandler(this.SymbolMap_DoubleClick);
            this.Activated += new System.EventHandler(this.SymbolMap_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SymbolMap_FormClosed);
            this.pnlAdvancedOptions.ResumeLayout(false);
            this.pnlAdvancedOptions.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SymbolViewer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlAdvancedOptions;
        private System.Windows.Forms.TextBox TxtUnicodeSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.CheckBox ChkAdvanceOptions;
        private System.Windows.Forms.VScrollBar SViewerScrollBar;
        private System.Windows.Forms.Label lblDisplaySymbol;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Button BtnClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSymbolToCopy;
        private System.Windows.Forms.TextBox txtCopySymbols;
        private System.Windows.Forms.DataGridView SymbolViewer;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ComboBox CmbFonts;
        private System.Windows.Forms.Button btnCancel;
    }
}
