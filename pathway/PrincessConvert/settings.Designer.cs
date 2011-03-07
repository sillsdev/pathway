using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace PrincessConvert
{

    partial class settings : System.Windows.Forms.Form
    {

        //Form overrides dispose to clean up the component list.
        [System.Diagnostics.DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components != null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        //Required by the Windows Form Designer

        private System.ComponentModel.IContainer components;
        //NOTE: The following procedure is required by the Windows Form Designer
        //It can be modified using the Windows Form Designer.  
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle DataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.DataGridView1 = new System.Windows.Forms.DataGridView();
            this.FontDialog1 = new System.Windows.Forms.FontDialog();
            this.btnFont = new System.Windows.Forms.Button();
            this.tbColumnWidthInPoints = new System.Windows.Forms.TextBox();
            this.lblLineHeight = new System.Windows.Forms.Label();
            this.lbInputUnits = new System.Windows.Forms.ListBox();
            this.lbLineHeightUnits = new System.Windows.Forms.ListBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.btnConvert = new System.Windows.Forms.Button();
            this.lblPageSize = new System.Windows.Forms.Label();
            this.lbPageSize = new System.Windows.Forms.ListBox();
            this.btnCalculateNumberOfLines = new System.Windows.Forms.Button();
            this.tbColumnHeightInLines = new System.Windows.Forms.TextBox();
            this.btnCalculateColumnWidthInPoints = new System.Windows.Forms.Button();
            this.tbPageHeightInPoints = new System.Windows.Forms.TextBox();
            this.lblPageHeightInPoints = new System.Windows.Forms.Label();
            this.tbPageWidthInPoints = new System.Windows.Forms.TextBox();
            this.lblPageWidth = new System.Windows.Forms.Label();
            this.NumericUpDownColumnGapInPoints = new System.Windows.Forms.NumericUpDown();
            this.lblConvertToPoints = new System.Windows.Forms.Label();
            this.tbConvertedValue = new System.Windows.Forms.TextBox();
            this.tbInputValueToConvert = new System.Windows.Forms.TextBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.NumericUpDownLeftMarginPoints = new System.Windows.Forms.NumericUpDown();
            this.Label2 = new System.Windows.Forms.Label();
            this.NumericUpDownRightMarginInPoints = new System.Windows.Forms.NumericUpDown();
            this.lblMarginLeft = new System.Windows.Forms.Label();
            this.NumericUpDownBottomMarginInPoints = new System.Windows.Forms.NumericUpDown();
            this.lblMarginTopInPoints = new System.Windows.Forms.Label();
            this.NumericUpDownTopMarginInPoints = new System.Windows.Forms.NumericUpDown();
            this.lbColumnGapUnits = new System.Windows.Forms.ListBox();
            this.NumericUpDownLineHeight = new System.Windows.Forms.NumericUpDown();
            this.NumericUpDownNumberOfColumns = new System.Windows.Forms.NumericUpDown();
            this.lblNumberOfColumn = new System.Windows.Forms.Label();
            this.lblColumnGap = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)this.DataGridView1).BeginInit();
            this.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.NumericUpDownColumnGapInPoints).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.NumericUpDownLeftMarginPoints).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.NumericUpDownRightMarginInPoints).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.NumericUpDownBottomMarginInPoints).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.NumericUpDownTopMarginInPoints).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.NumericUpDownLineHeight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.NumericUpDownNumberOfColumns).BeginInit();
            this.SuspendLayout();
            //
            //DataGridView1
            //
            this.DataGridView1.AllowUserToAddRows = false;
            this.DataGridView1.AllowUserToDeleteRows = false;
            this.DataGridView1.AllowUserToOrderColumns = true;
            DataGridViewCellStyle3.BackColor = System.Drawing.Color.Aquamarine;
            this.DataGridView1.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle3;
            this.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView1.Location = new System.Drawing.Point(37, 312);
            this.DataGridView1.Name = "DataGridView1";
            this.DataGridView1.Size = new System.Drawing.Size(700, 150);
            this.DataGridView1.TabIndex = 1;
            //
            //btnFont
            //
            this.btnFont.Location = new System.Drawing.Point(48, 482);
            this.btnFont.Name = "btnFont";
            this.btnFont.Size = new System.Drawing.Size(75, 23);
            this.btnFont.TabIndex = 3;
            this.btnFont.Text = "Font";
            this.btnFont.UseVisualStyleBackColor = true;
            //
            //tbColumnWidthInPoints
            //
            this.tbColumnWidthInPoints.Location = new System.Drawing.Point(148, 214);
            this.tbColumnWidthInPoints.Name = "tbColumnWidthInPoints";
            this.tbColumnWidthInPoints.Size = new System.Drawing.Size(40, 20);
            this.tbColumnWidthInPoints.TabIndex = 36;
            this.tbColumnWidthInPoints.WordWrap = false;
            //
            //lblLineHeight
            //
            this.lblLineHeight.AutoSize = true;
            this.lblLineHeight.Location = new System.Drawing.Point(207, 85);
            this.lblLineHeight.Name = "lblLineHeight";
            this.lblLineHeight.Size = new System.Drawing.Size(59, 13);
            this.lblLineHeight.TabIndex = 37;
            this.lblLineHeight.Text = "Line height";
            //
            //lbInputUnits
            //
            this.lbInputUnits.AllowDrop = true;
            this.lbInputUnits.FormattingEnabled = true;
            this.lbInputUnits.Location = new System.Drawing.Point(490, 31);
            this.lbInputUnits.Name = "lbInputUnits";
            this.lbInputUnits.Size = new System.Drawing.Size(56, 17);
            this.lbInputUnits.TabIndex = 39;
            //
            //lbLineHeightUnits
            //
            this.lbLineHeightUnits.AllowDrop = true;
            this.lbLineHeightUnits.FormattingEnabled = true;
            this.lbLineHeightUnits.Location = new System.Drawing.Point(443, 157);
            this.lbLineHeightUnits.Name = "lbLineHeightUnits";
            this.lbLineHeightUnits.Size = new System.Drawing.Size(56, 17);
            this.lbLineHeightUnits.TabIndex = 40;
            //
            //btnCancel
            //
            this.btnCancel.Location = new System.Drawing.Point(513, 215);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 41;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            //
            //btnOK
            //
            this.btnOK.Location = new System.Drawing.Point(422, 215);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 42;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            //
            //Panel1
            //
            this.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Panel1.Controls.Add(this.btnConvert);
            this.Panel1.Controls.Add(this.lblPageSize);
            this.Panel1.Controls.Add(this.lbPageSize);
            this.Panel1.Controls.Add(this.btnCalculateNumberOfLines);
            this.Panel1.Controls.Add(this.tbColumnHeightInLines);
            this.Panel1.Controls.Add(this.btnCalculateColumnWidthInPoints);
            this.Panel1.Controls.Add(this.tbPageHeightInPoints);
            this.Panel1.Controls.Add(this.lblPageHeightInPoints);
            this.Panel1.Controls.Add(this.tbPageWidthInPoints);
            this.Panel1.Controls.Add(this.lblPageWidth);
            this.Panel1.Controls.Add(this.NumericUpDownColumnGapInPoints);
            this.Panel1.Controls.Add(this.lblConvertToPoints);
            this.Panel1.Controls.Add(this.tbConvertedValue);
            this.Panel1.Controls.Add(this.tbInputValueToConvert);
            this.Panel1.Controls.Add(this.Label3);
            this.Panel1.Controls.Add(this.NumericUpDownLeftMarginPoints);
            this.Panel1.Controls.Add(this.Label2);
            this.Panel1.Controls.Add(this.NumericUpDownRightMarginInPoints);
            this.Panel1.Controls.Add(this.lblMarginLeft);
            this.Panel1.Controls.Add(this.NumericUpDownBottomMarginInPoints);
            this.Panel1.Controls.Add(this.lblMarginTopInPoints);
            this.Panel1.Controls.Add(this.NumericUpDownTopMarginInPoints);
            this.Panel1.Controls.Add(this.lbColumnGapUnits);
            this.Panel1.Controls.Add(this.NumericUpDownLineHeight);
            this.Panel1.Controls.Add(this.NumericUpDownNumberOfColumns);
            this.Panel1.Controls.Add(this.lblNumberOfColumn);
            this.Panel1.Controls.Add(this.lblColumnGap);
            this.Panel1.Controls.Add(this.btnOK);
            this.Panel1.Controls.Add(this.btnCancel);
            this.Panel1.Controls.Add(this.lbLineHeightUnits);
            this.Panel1.Controls.Add(this.lbInputUnits);
            this.Panel1.Controls.Add(this.lblLineHeight);
            this.Panel1.Controls.Add(this.tbColumnWidthInPoints);
            this.Panel1.Location = new System.Drawing.Point(38, 9);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(579, 249);
            this.Panel1.TabIndex = 0;
            //
            //btnConvert
            //
            this.btnConvert.Location = new System.Drawing.Point(490, 54);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(75, 23);
            this.btnConvert.TabIndex = 73;
            this.btnConvert.Text = "Convert";
            this.btnConvert.UseVisualStyleBackColor = true;
            //
            //lblPageSize
            //
            this.lblPageSize.AutoSize = true;
            this.lblPageSize.Location = new System.Drawing.Point(29, 24);
            this.lblPageSize.Name = "lblPageSize";
            this.lblPageSize.Size = new System.Drawing.Size(53, 13);
            this.lblPageSize.TabIndex = 72;
            this.lblPageSize.Text = "Page size";
            //
            //lbPageSize
            //
            this.lbPageSize.FormattingEnabled = true;
            this.lbPageSize.Location = new System.Drawing.Point(148, 20);
            this.lbPageSize.Name = "lbPageSize";
            this.lbPageSize.Size = new System.Drawing.Size(181, 17);
            this.lbPageSize.TabIndex = 71;
            //
            //btnCalculateNumberOfLines
            //
            this.btnCalculateNumberOfLines.Location = new System.Drawing.Point(197, 212);
            this.btnCalculateNumberOfLines.Name = "btnCalculateNumberOfLines";
            this.btnCalculateNumberOfLines.Size = new System.Drawing.Size(140, 23);
            this.btnCalculateNumberOfLines.TabIndex = 70;
            this.btnCalculateNumberOfLines.Text = "Calculate number of lines per page";
            this.btnCalculateNumberOfLines.UseVisualStyleBackColor = true;
            //
            //tbColumnHeightInLines
            //
            this.tbColumnHeightInLines.Location = new System.Drawing.Point(342, 212);
            this.tbColumnHeightInLines.Name = "tbColumnHeightInLines";
            this.tbColumnHeightInLines.Size = new System.Drawing.Size(40, 20);
            this.tbColumnHeightInLines.TabIndex = 69;
            this.tbColumnHeightInLines.WordWrap = false;
            //
            //btnCalculateColumnWidthInPoints
            //
            this.btnCalculateColumnWidthInPoints.Location = new System.Drawing.Point(8, 214);
            this.btnCalculateColumnWidthInPoints.Name = "btnCalculateColumnWidthInPoints";
            this.btnCalculateColumnWidthInPoints.Size = new System.Drawing.Size(130, 23);
            this.btnCalculateColumnWidthInPoints.TabIndex = 68;
            this.btnCalculateColumnWidthInPoints.Text = "Calculate width in points";
            this.btnCalculateColumnWidthInPoints.UseVisualStyleBackColor = true;
            //
            //tbPageHeightInPoints
            //
            this.tbPageHeightInPoints.Location = new System.Drawing.Point(341, 52);
            this.tbPageHeightInPoints.Name = "tbPageHeightInPoints";
            this.tbPageHeightInPoints.Size = new System.Drawing.Size(40, 20);
            this.tbPageHeightInPoints.TabIndex = 67;
            this.tbPageHeightInPoints.WordWrap = false;
            //
            //lblPageHeightInPoints
            //
            this.lblPageHeightInPoints.AutoSize = true;
            this.lblPageHeightInPoints.Location = new System.Drawing.Point(207, 59);
            this.lblPageHeightInPoints.Name = "lblPageHeightInPoints";
            this.lblPageHeightInPoints.Size = new System.Drawing.Size(106, 13);
            this.lblPageHeightInPoints.TabIndex = 66;
            this.lblPageHeightInPoints.Text = "Page height in points";
            //
            //tbPageWidthInPoints
            //
            this.tbPageWidthInPoints.Location = new System.Drawing.Point(149, 56);
            this.tbPageWidthInPoints.Name = "tbPageWidthInPoints";
            this.tbPageWidthInPoints.Size = new System.Drawing.Size(40, 20);
            this.tbPageWidthInPoints.TabIndex = 65;
            this.tbPageWidthInPoints.WordWrap = false;
            //
            //lblPageWidth
            //
            this.lblPageWidth.AutoSize = true;
            this.lblPageWidth.Location = new System.Drawing.Point(27, 59);
            this.lblPageWidth.Name = "lblPageWidth";
            this.lblPageWidth.Size = new System.Drawing.Size(102, 13);
            this.lblPageWidth.TabIndex = 64;
            this.lblPageWidth.Text = "Page width in points";
            //
            //NumericUpDownColumnGapInPoints
            //
            this.NumericUpDownColumnGapInPoints.Location = new System.Drawing.Point(148, 180);
            this.NumericUpDownColumnGapInPoints.Maximum = new decimal(new int[] {
			99,
			0,
			0,
			0
		});
            this.NumericUpDownColumnGapInPoints.Name = "NumericUpDownColumnGapInPoints";
            this.NumericUpDownColumnGapInPoints.Size = new System.Drawing.Size(40, 20);
            this.NumericUpDownColumnGapInPoints.TabIndex = 62;
            this.NumericUpDownColumnGapInPoints.Value = new decimal(new int[] {
			6,
			0,
			0,
			0
		});
            //
            //lblConvertToPoints
            //
            this.lblConvertToPoints.AutoSize = true;
            this.lblConvertToPoints.Location = new System.Drawing.Point(440, 13);
            this.lblConvertToPoints.Name = "lblConvertToPoints";
            this.lblConvertToPoints.Size = new System.Drawing.Size(88, 13);
            this.lblConvertToPoints.TabIndex = 61;
            this.lblConvertToPoints.Text = "Convert to Points";
            //
            //tbConvertedValue
            //
            this.tbConvertedValue.Location = new System.Drawing.Point(444, 56);
            this.tbConvertedValue.Name = "tbConvertedValue";
            this.tbConvertedValue.Size = new System.Drawing.Size(40, 20);
            this.tbConvertedValue.TabIndex = 60;
            this.tbConvertedValue.WordWrap = false;
            //
            //tbInputValueToConvert
            //
            this.tbInputValueToConvert.Location = new System.Drawing.Point(444, 28);
            this.tbInputValueToConvert.Name = "tbInputValueToConvert";
            this.tbInputValueToConvert.Size = new System.Drawing.Size(40, 20);
            this.tbInputValueToConvert.TabIndex = 59;
            this.tbInputValueToConvert.WordWrap = false;
            //
            //Label3
            //
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(207, 151);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(116, 13);
            this.Label3.TabIndex = 58;
            this.Label3.Text = "Margin bottom in points";
            //
            //NumericUpDownLeftMarginPoints
            //
            this.NumericUpDownLeftMarginPoints.Location = new System.Drawing.Point(148, 149);
            this.NumericUpDownLeftMarginPoints.Maximum = new decimal(new int[] {
			99,
			0,
			0,
			0
		});
            this.NumericUpDownLeftMarginPoints.Name = "NumericUpDownLeftMarginPoints";
            this.NumericUpDownLeftMarginPoints.Size = new System.Drawing.Size(40, 20);
            this.NumericUpDownLeftMarginPoints.TabIndex = 57;
            this.NumericUpDownLeftMarginPoints.Value = new decimal(new int[] {
			12,
			0,
			0,
			0
		});
            //
            //Label2
            //
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(27, 115);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(104, 13);
            this.Label2.TabIndex = 56;
            this.Label2.Text = "Margin right in points";
            //
            //NumericUpDownRightMarginInPoints
            //
            this.NumericUpDownRightMarginInPoints.Location = new System.Drawing.Point(148, 113);
            this.NumericUpDownRightMarginInPoints.Maximum = new decimal(new int[] {
			99,
			0,
			0,
			0
		});
            this.NumericUpDownRightMarginInPoints.Name = "NumericUpDownRightMarginInPoints";
            this.NumericUpDownRightMarginInPoints.Size = new System.Drawing.Size(40, 20);
            this.NumericUpDownRightMarginInPoints.TabIndex = 55;
            this.NumericUpDownRightMarginInPoints.Value = new decimal(new int[] {
			12,
			0,
			0,
			0
		});
            //
            //lblMarginLeft
            //
            this.lblMarginLeft.AutoSize = true;
            this.lblMarginLeft.Location = new System.Drawing.Point(27, 151);
            this.lblMarginLeft.Name = "lblMarginLeft";
            this.lblMarginLeft.Size = new System.Drawing.Size(98, 13);
            this.lblMarginLeft.TabIndex = 54;
            this.lblMarginLeft.Text = "Margin left in points";
            //
            //NumericUpDownBottomMarginInPoints
            //
            this.NumericUpDownBottomMarginInPoints.Location = new System.Drawing.Point(341, 149);
            this.NumericUpDownBottomMarginInPoints.Maximum = new decimal(new int[] {
			99,
			0,
			0,
			0
		});
            this.NumericUpDownBottomMarginInPoints.Name = "NumericUpDownBottomMarginInPoints";
            this.NumericUpDownBottomMarginInPoints.Size = new System.Drawing.Size(40, 20);
            this.NumericUpDownBottomMarginInPoints.TabIndex = 53;
            this.NumericUpDownBottomMarginInPoints.Value = new decimal(new int[] {
			12,
			0,
			0,
			0
		});
            //
            //lblMarginTopInPoints
            //
            this.lblMarginTopInPoints.AutoSize = true;
            this.lblMarginTopInPoints.Location = new System.Drawing.Point(207, 115);
            this.lblMarginTopInPoints.Name = "lblMarginTopInPoints";
            this.lblMarginTopInPoints.Size = new System.Drawing.Size(99, 13);
            this.lblMarginTopInPoints.TabIndex = 52;
            this.lblMarginTopInPoints.Text = "Margin top in points";
            //
            //NumericUpDownTopMarginInPoints
            //
            this.NumericUpDownTopMarginInPoints.Location = new System.Drawing.Point(341, 113);
            this.NumericUpDownTopMarginInPoints.Maximum = new decimal(new int[] {
			99,
			0,
			0,
			0
		});
            this.NumericUpDownTopMarginInPoints.Name = "NumericUpDownTopMarginInPoints";
            this.NumericUpDownTopMarginInPoints.Size = new System.Drawing.Size(40, 20);
            this.NumericUpDownTopMarginInPoints.TabIndex = 51;
            this.NumericUpDownTopMarginInPoints.Value = new decimal(new int[] {
			12,
			0,
			0,
			0
		});
            //
            //lbColumnGapUnits
            //
            this.lbColumnGapUnits.AllowDrop = true;
            this.lbColumnGapUnits.FormattingEnabled = true;
            this.lbColumnGapUnits.Location = new System.Drawing.Point(444, 134);
            this.lbColumnGapUnits.Name = "lbColumnGapUnits";
            this.lbColumnGapUnits.Size = new System.Drawing.Size(56, 17);
            this.lbColumnGapUnits.TabIndex = 49;
            //
            //NumericUpDownLineHeight
            //
            this.NumericUpDownLineHeight.DecimalPlaces = 2;
            this.NumericUpDownLineHeight.Increment = new decimal(new int[] {
			25,
			0,
			0,
			131072
		});
            this.NumericUpDownLineHeight.Location = new System.Drawing.Point(341, 83);
            this.NumericUpDownLineHeight.Maximum = new decimal(new int[] {
			18,
			0,
			0,
			0
		});
            this.NumericUpDownLineHeight.Minimum = new decimal(new int[] {
			8,
			0,
			0,
			0
		});
            this.NumericUpDownLineHeight.Name = "NumericUpDownLineHeight";
            this.NumericUpDownLineHeight.Size = new System.Drawing.Size(52, 20);
            this.NumericUpDownLineHeight.TabIndex = 48;
            this.NumericUpDownLineHeight.Value = new decimal(new int[] {
			11,
			0,
			0,
			0
		});
            //
            //NumericUpDownNumberOfColumns
            //
            this.NumericUpDownNumberOfColumns.Location = new System.Drawing.Point(149, 83);
            this.NumericUpDownNumberOfColumns.Maximum = new decimal(new int[] {
			6,
			0,
			0,
			0
		});
            this.NumericUpDownNumberOfColumns.Minimum = new decimal(new int[] {
			1,
			0,
			0,
			0
		});
            this.NumericUpDownNumberOfColumns.Name = "NumericUpDownNumberOfColumns";
            this.NumericUpDownNumberOfColumns.Size = new System.Drawing.Size(40, 20);
            this.NumericUpDownNumberOfColumns.TabIndex = 47;
            this.NumericUpDownNumberOfColumns.Value = new decimal(new int[] {
			1,
			0,
			0,
			0
		});
            //
            //lblNumberOfColumn
            //
            this.lblNumberOfColumn.AutoSize = true;
            this.lblNumberOfColumn.Location = new System.Drawing.Point(27, 85);
            this.lblNumberOfColumn.Name = "lblNumberOfColumn";
            this.lblNumberOfColumn.Size = new System.Drawing.Size(98, 13);
            this.lblNumberOfColumn.TabIndex = 46;
            this.lblNumberOfColumn.Text = "Number of columns";
            //
            //lblColumnGap
            //
            this.lblColumnGap.AutoSize = true;
            this.lblColumnGap.Location = new System.Drawing.Point(27, 182);
            this.lblColumnGap.Name = "lblColumnGap";
            this.lblColumnGap.Size = new System.Drawing.Size(63, 13);
            this.lblColumnGap.TabIndex = 44;
            this.lblColumnGap.Text = "Column gap";
            //
            //settings
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 666);
            this.Controls.Add(this.btnFont);
            this.Controls.Add(this.DataGridView1);
            this.Controls.Add(this.Panel1);
            this.Name = "settings";
            this.Text = "settings";
            ((System.ComponentModel.ISupportInitialize)this.DataGridView1).EndInit();
            this.Panel1.ResumeLayout(false);
            this.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.NumericUpDownColumnGapInPoints).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.NumericUpDownLeftMarginPoints).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.NumericUpDownRightMarginInPoints).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.NumericUpDownBottomMarginInPoints).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.NumericUpDownTopMarginInPoints).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.NumericUpDownLineHeight).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.NumericUpDownNumberOfColumns).EndInit();
            this.ResumeLayout(false);

        }
        internal System.Windows.Forms.DataGridView DataGridView1;
        internal System.Windows.Forms.FontDialog FontDialog1;
        internal System.Windows.Forms.Button btnFont;
        internal System.Windows.Forms.TextBox tbColumnWidthInPoints;
        internal System.Windows.Forms.Label lblLineHeight;
        internal System.Windows.Forms.ListBox lbInputUnits;
        internal System.Windows.Forms.ListBox lbLineHeightUnits;
        internal System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.Button btnOK;
        internal System.Windows.Forms.Panel Panel1;
        internal System.Windows.Forms.Label lblColumnGap;
        internal System.Windows.Forms.Label lblNumberOfColumn;
        internal System.Windows.Forms.NumericUpDown NumericUpDownNumberOfColumns;
        internal System.Windows.Forms.NumericUpDown NumericUpDownLineHeight;
        internal System.Windows.Forms.ListBox lbColumnGapUnits;
        internal System.Windows.Forms.NumericUpDown NumericUpDownTopMarginInPoints;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.NumericUpDown NumericUpDownLeftMarginPoints;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.NumericUpDown NumericUpDownRightMarginInPoints;
        internal System.Windows.Forms.Label lblMarginLeft;
        internal System.Windows.Forms.NumericUpDown NumericUpDownBottomMarginInPoints;
        internal System.Windows.Forms.Label lblMarginTopInPoints;
        internal System.Windows.Forms.Label lblConvertToPoints;
        internal System.Windows.Forms.TextBox tbConvertedValue;
        internal System.Windows.Forms.TextBox tbInputValueToConvert;
        internal System.Windows.Forms.NumericUpDown NumericUpDownColumnGapInPoints;
        internal System.Windows.Forms.TextBox tbPageWidthInPoints;
        internal System.Windows.Forms.Label lblPageWidth;
        internal System.Windows.Forms.TextBox tbPageHeightInPoints;
        internal System.Windows.Forms.Label lblPageHeightInPoints;
        internal System.Windows.Forms.Button btnCalculateColumnWidthInPoints;
        internal System.Windows.Forms.Button btnCalculateNumberOfLines;
        internal System.Windows.Forms.TextBox tbColumnHeightInLines;
        internal System.Windows.Forms.ListBox lbPageSize;
        internal System.Windows.Forms.Label lblPageSize;
        internal System.Windows.Forms.Button btnConvert;

    }
}