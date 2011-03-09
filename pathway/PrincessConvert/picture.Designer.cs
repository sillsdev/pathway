using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace PrincessConvert
{

    partial class picture : System.Windows.Forms.Form
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
            this.rbTop = new System.Windows.Forms.RadioButton();
            this.rbBottom = new System.Windows.Forms.RadioButton();
            this.rbInline = new System.Windows.Forms.RadioButton();
            this.panelPosition = new System.Windows.Forms.Panel();
            this.Panel3 = new System.Windows.Forms.Panel();
            this.RadioButton1 = new System.Windows.Forms.RadioButton();
            this.RadioButton2 = new System.Windows.Forms.RadioButton();
            this.RadioButton3 = new System.Windows.Forms.RadioButton();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblMarginTopInLines = new System.Windows.Forms.Label();
            this.lblMarginBottomInLines = new System.Windows.Forms.Label();
            this.lblGraphicSize = new System.Windows.Forms.Label();
            this.lblGraphicFloat = new System.Windows.Forms.Label();
            this.btnInsertGraphicHere = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnRemove = new System.Windows.Forms.Button();
            this.PictureDialog = new System.Windows.Forms.OpenFileDialog();
            this.tbLineHeight = new System.Windows.Forms.TextBox();
            this.tbColumnWidth = new System.Windows.Forms.TextBox();
            this.tbPictureWidthInPixels = new System.Windows.Forms.TextBox();
            this.tbPictureHeightInPixels = new System.Windows.Forms.TextBox();
            this.lblLineHeightInPoints = new System.Windows.Forms.Label();
            this.lblColumnWidthInPoints = new System.Windows.Forms.Label();
            this.lblImageWidth = new System.Windows.Forms.Label();
            this.lblHeight = new System.Windows.Forms.Label();
            this.lbOriginalWidthUnits = new System.Windows.Forms.ListBox();
            this.lbOriginalHeightUnits = new System.Windows.Forms.ListBox();
            this.lblPublishedHeight = new System.Windows.Forms.Label();
            this.lblPublishedWidth = new System.Windows.Forms.Label();
            this.tbPublishedHeightInPixels = new System.Windows.Forms.TextBox();
            this.tbPublishedWidthInPixels = new System.Windows.Forms.TextBox();
            this.lbColumnWidthUnits = new System.Windows.Forms.ListBox();
            this.lbLineHeightUnits = new System.Windows.Forms.ListBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.tbGraphicFileName = new System.Windows.Forms.TextBox();
            this.tbScale = new System.Windows.Forms.TextBox();
            this.lblScale = new System.Windows.Forms.Label();
            this.lblGraphicWidthInGr = new System.Windows.Forms.Label();
            this.NumericUpDownWidthInGr = new System.Windows.Forms.NumericUpDown();
            this.NumericUpDownHeightInLines = new System.Windows.Forms.NumericUpDown();
            this.NumericUpDownMarginRightInPoints = new System.Windows.Forms.NumericUpDown();
            this.NumericUpDownMarginLeftInPoints = new System.Windows.Forms.NumericUpDown();
            this.NumericUpDownMarginBottomInLines = new System.Windows.Forms.NumericUpDown();
            this.NumericUpDownMarginTopInLines = new System.Windows.Forms.NumericUpDown();
            this.lblMarginLeftInPoints = new System.Windows.Forms.Label();
            this.lblMarginRightInPoints = new System.Windows.Forms.Label();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.rbPictureSpans1Column = new System.Windows.Forms.RadioButton();
            this.rbPictureSpans2Columns = new System.Windows.Forms.RadioButton();
            this.NumericUpDownPercentOfColumnForPicture = new System.Windows.Forms.NumericUpDown();
            this.lblPicturePercentWidth = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.panelPosition.SuspendLayout();
            this.Panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownWidthInGr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownHeightInLines)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownMarginRightInPoints)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownMarginLeftInPoints)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownMarginBottomInLines)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownMarginTopInLines)).BeginInit();
            this.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownPercentOfColumnForPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // rbTop
            // 
            this.rbTop.AutoSize = true;
            this.rbTop.Location = new System.Drawing.Point(18, 3);
            this.rbTop.Name = "rbTop";
            this.rbTop.Size = new System.Drawing.Size(44, 17);
            this.rbTop.TabIndex = 0;
            this.rbTop.TabStop = true;
            this.rbTop.Text = "Top";
            this.rbTop.UseVisualStyleBackColor = true;
            // 
            // rbBottom
            // 
            this.rbBottom.AutoSize = true;
            this.rbBottom.Location = new System.Drawing.Point(18, 26);
            this.rbBottom.Name = "rbBottom";
            this.rbBottom.Size = new System.Drawing.Size(58, 17);
            this.rbBottom.TabIndex = 1;
            this.rbBottom.TabStop = true;
            this.rbBottom.Text = "Bottom";
            this.rbBottom.UseVisualStyleBackColor = true;
            // 
            // rbInline
            // 
            this.rbInline.AutoSize = true;
            this.rbInline.Location = new System.Drawing.Point(18, 48);
            this.rbInline.Name = "rbInline";
            this.rbInline.Size = new System.Drawing.Size(54, 17);
            this.rbInline.TabIndex = 2;
            this.rbInline.TabStop = true;
            this.rbInline.Text = "In text";
            this.rbInline.UseVisualStyleBackColor = true;
            // 
            // panelPosition
            // 
            this.panelPosition.Controls.Add(this.Panel3);
            this.panelPosition.Controls.Add(this.rbTop);
            this.panelPosition.Controls.Add(this.rbBottom);
            this.panelPosition.Controls.Add(this.rbInline);
            this.panelPosition.Location = new System.Drawing.Point(6, 143);
            this.panelPosition.Name = "panelPosition";
            this.panelPosition.Size = new System.Drawing.Size(114, 68);
            this.panelPosition.TabIndex = 5;
            // 
            // Panel3
            // 
            this.Panel3.Controls.Add(this.RadioButton1);
            this.Panel3.Controls.Add(this.RadioButton2);
            this.Panel3.Controls.Add(this.RadioButton3);
            this.Panel3.Location = new System.Drawing.Point(4, -70);
            this.Panel3.Name = "Panel3";
            this.Panel3.Size = new System.Drawing.Size(114, 55);
            this.Panel3.TabIndex = 6;
            // 
            // RadioButton1
            // 
            this.RadioButton1.AutoSize = true;
            this.RadioButton1.Location = new System.Drawing.Point(18, 3);
            this.RadioButton1.Name = "RadioButton1";
            this.RadioButton1.Size = new System.Drawing.Size(44, 17);
            this.RadioButton1.TabIndex = 0;
            this.RadioButton1.TabStop = true;
            this.RadioButton1.Text = "Top";
            this.RadioButton1.UseVisualStyleBackColor = true;
            // 
            // RadioButton2
            // 
            this.RadioButton2.AutoSize = true;
            this.RadioButton2.Location = new System.Drawing.Point(18, 26);
            this.RadioButton2.Name = "RadioButton2";
            this.RadioButton2.Size = new System.Drawing.Size(58, 17);
            this.RadioButton2.TabIndex = 1;
            this.RadioButton2.TabStop = true;
            this.RadioButton2.Text = "Bottom";
            this.RadioButton2.UseVisualStyleBackColor = true;
            // 
            // RadioButton3
            // 
            this.RadioButton3.AutoSize = true;
            this.RadioButton3.Location = new System.Drawing.Point(19, 51);
            this.RadioButton3.Name = "RadioButton3";
            this.RadioButton3.Size = new System.Drawing.Size(54, 17);
            this.RadioButton3.TabIndex = 2;
            this.RadioButton3.TabStop = true;
            this.RadioButton3.Text = "In text";
            this.RadioButton3.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(448, 415);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(53, 24);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(507, 415);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(53, 24);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblMarginTopInLines
            // 
            this.lblMarginTopInLines.AutoSize = true;
            this.lblMarginTopInLines.Location = new System.Drawing.Point(7, 229);
            this.lblMarginTopInLines.Name = "lblMarginTopInLines";
            this.lblMarginTopInLines.Size = new System.Drawing.Size(87, 13);
            this.lblMarginTopInLines.TabIndex = 12;
            this.lblMarginTopInLines.Text = "Margin top (lines)";
            // 
            // lblMarginBottomInLines
            // 
            this.lblMarginBottomInLines.AutoSize = true;
            this.lblMarginBottomInLines.Location = new System.Drawing.Point(7, 271);
            this.lblMarginBottomInLines.Name = "lblMarginBottomInLines";
            this.lblMarginBottomInLines.Size = new System.Drawing.Size(104, 13);
            this.lblMarginBottomInLines.TabIndex = 13;
            this.lblMarginBottomInLines.Text = "Margin bottom (lines)";
            // 
            // lblGraphicSize
            // 
            this.lblGraphicSize.AutoSize = true;
            this.lblGraphicSize.Location = new System.Drawing.Point(7, 471);
            this.lblGraphicSize.Name = "lblGraphicSize";
            this.lblGraphicSize.Size = new System.Drawing.Size(106, 13);
            this.lblGraphicSize.TabIndex = 14;
            this.lblGraphicSize.Text = "Graphic height (lines)";
            // 
            // lblGraphicFloat
            // 
            this.lblGraphicFloat.AutoSize = true;
            this.lblGraphicFloat.Location = new System.Drawing.Point(4, 130);
            this.lblGraphicFloat.Name = "lblGraphicFloat";
            this.lblGraphicFloat.Size = new System.Drawing.Size(79, 13);
            this.lblGraphicFloat.TabIndex = 15;
            this.lblGraphicFloat.Text = "Graphic float to";
            // 
            // btnInsertGraphicHere
            // 
            this.btnInsertGraphicHere.Location = new System.Drawing.Point(448, 415);
            this.btnInsertGraphicHere.Name = "btnInsertGraphicHere";
            this.btnInsertGraphicHere.Size = new System.Drawing.Size(53, 24);
            this.btnInsertGraphicHere.TabIndex = 16;
            this.btnInsertGraphicHere.Text = "Insert";
            this.btnInsertGraphicHere.UseVisualStyleBackColor = true;
            this.btnInsertGraphicHere.Visible = false;
            this.btnInsertGraphicHere.Click += new System.EventHandler(this.btnInsertGraphicHere_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Location = new System.Drawing.Point(120, 54);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(459, 353);
            this.pictureBox2.TabIndex = 6;
            this.pictureBox2.TabStop = false;
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(437, 415);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(64, 24);
            this.btnRemove.TabIndex = 17;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Visible = false;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // PictureDialog
            // 
            this.PictureDialog.FileName = "OpenFileDialog1";
            // 
            // tbLineHeight
            // 
            this.tbLineHeight.Enabled = false;
            this.tbLineHeight.Location = new System.Drawing.Point(4, 32);
            this.tbLineHeight.Name = "tbLineHeight";
            this.tbLineHeight.Size = new System.Drawing.Size(40, 20);
            this.tbLineHeight.TabIndex = 19;
            this.tbLineHeight.WordWrap = false;
            this.tbLineHeight.TextChanged += new System.EventHandler(this.tbLineHeight_TextChanged);
            // 
            // tbColumnWidth
            // 
            this.tbColumnWidth.Enabled = false;
            this.tbColumnWidth.Location = new System.Drawing.Point(4, 6);
            this.tbColumnWidth.Name = "tbColumnWidth";
            this.tbColumnWidth.Size = new System.Drawing.Size(40, 20);
            this.tbColumnWidth.TabIndex = 20;
            this.tbColumnWidth.WordWrap = false;
            this.tbColumnWidth.TextChanged += new System.EventHandler(this.tbColumnWidth_TextChanged);
            // 
            // tbPictureWidthInPixels
            // 
            this.tbPictureWidthInPixels.Enabled = false;
            this.tbPictureWidthInPixels.Location = new System.Drawing.Point(218, 414);
            this.tbPictureWidthInPixels.Name = "tbPictureWidthInPixels";
            this.tbPictureWidthInPixels.Size = new System.Drawing.Size(40, 20);
            this.tbPictureWidthInPixels.TabIndex = 21;
            // 
            // tbPictureHeightInPixels
            // 
            this.tbPictureHeightInPixels.Enabled = false;
            this.tbPictureHeightInPixels.Location = new System.Drawing.Point(218, 439);
            this.tbPictureHeightInPixels.Name = "tbPictureHeightInPixels";
            this.tbPictureHeightInPixels.Size = new System.Drawing.Size(40, 20);
            this.tbPictureHeightInPixels.TabIndex = 22;
            // 
            // lblLineHeightInPoints
            // 
            this.lblLineHeightInPoints.AutoSize = true;
            this.lblLineHeightInPoints.Location = new System.Drawing.Point(45, 35);
            this.lblLineHeightInPoints.Name = "lblLineHeightInPoints";
            this.lblLineHeightInPoints.Size = new System.Drawing.Size(96, 13);
            this.lblLineHeightInPoints.TabIndex = 23;
            this.lblLineHeightInPoints.Text = "Line height (points)";
            // 
            // lblColumnWidthInPoints
            // 
            this.lblColumnWidthInPoints.AutoSize = true;
            this.lblColumnWidthInPoints.Location = new System.Drawing.Point(45, 9);
            this.lblColumnWidthInPoints.Name = "lblColumnWidthInPoints";
            this.lblColumnWidthInPoints.Size = new System.Drawing.Size(107, 13);
            this.lblColumnWidthInPoints.TabIndex = 24;
            this.lblColumnWidthInPoints.Text = "Column width (points)";
            // 
            // lblImageWidth
            // 
            this.lblImageWidth.AutoSize = true;
            this.lblImageWidth.Location = new System.Drawing.Point(260, 417);
            this.lblImageWidth.Name = "lblImageWidth";
            this.lblImageWidth.Size = new System.Drawing.Size(105, 13);
            this.lblImageWidth.TabIndex = 25;
            this.lblImageWidth.Text = "Original width (pixels)";
            // 
            // lblHeight
            // 
            this.lblHeight.AutoSize = true;
            this.lblHeight.Location = new System.Drawing.Point(260, 442);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(109, 13);
            this.lblHeight.TabIndex = 26;
            this.lblHeight.Text = "Original height (pixels)";
            // 
            // lbOriginalWidthUnits
            // 
            this.lbOriginalWidthUnits.AllowDrop = true;
            this.lbOriginalWidthUnits.FormattingEnabled = true;
            this.lbOriginalWidthUnits.Location = new System.Drawing.Point(374, 308);
            this.lbOriginalWidthUnits.Name = "lbOriginalWidthUnits";
            this.lbOriginalWidthUnits.Size = new System.Drawing.Size(56, 17);
            this.lbOriginalWidthUnits.TabIndex = 27;
            this.lbOriginalWidthUnits.Visible = false;
            // 
            // lbOriginalHeightUnits
            // 
            this.lbOriginalHeightUnits.AllowDrop = true;
            this.lbOriginalHeightUnits.FormattingEnabled = true;
            this.lbOriginalHeightUnits.Location = new System.Drawing.Point(374, 339);
            this.lbOriginalHeightUnits.Name = "lbOriginalHeightUnits";
            this.lbOriginalHeightUnits.Size = new System.Drawing.Size(56, 17);
            this.lbOriginalHeightUnits.TabIndex = 28;
            this.lbOriginalHeightUnits.Visible = false;
            // 
            // lblPublishedHeight
            // 
            this.lblPublishedHeight.AutoSize = true;
            this.lblPublishedHeight.Location = new System.Drawing.Point(260, 493);
            this.lblPublishedHeight.Name = "lblPublishedHeight";
            this.lblPublishedHeight.Size = new System.Drawing.Size(122, 13);
            this.lblPublishedHeight.TabIndex = 32;
            this.lblPublishedHeight.Text = "Published height (points)";
            // 
            // lblPublishedWidth
            // 
            this.lblPublishedWidth.AutoSize = true;
            this.lblPublishedWidth.Location = new System.Drawing.Point(260, 468);
            this.lblPublishedWidth.Name = "lblPublishedWidth";
            this.lblPublishedWidth.Size = new System.Drawing.Size(118, 13);
            this.lblPublishedWidth.TabIndex = 31;
            this.lblPublishedWidth.Text = "Published width (points)";
            // 
            // tbPublishedHeightInPixels
            // 
            this.tbPublishedHeightInPixels.Enabled = false;
            this.tbPublishedHeightInPixels.Location = new System.Drawing.Point(218, 489);
            this.tbPublishedHeightInPixels.Name = "tbPublishedHeightInPixels";
            this.tbPublishedHeightInPixels.Size = new System.Drawing.Size(40, 20);
            this.tbPublishedHeightInPixels.TabIndex = 30;
            // 
            // tbPublishedWidthInPixels
            // 
            this.tbPublishedWidthInPixels.Enabled = false;
            this.tbPublishedWidthInPixels.Location = new System.Drawing.Point(218, 464);
            this.tbPublishedWidthInPixels.Name = "tbPublishedWidthInPixels";
            this.tbPublishedWidthInPixels.Size = new System.Drawing.Size(40, 20);
            this.tbPublishedWidthInPixels.TabIndex = 29;
            // 
            // lbColumnWidthUnits
            // 
            this.lbColumnWidthUnits.AllowDrop = true;
            this.lbColumnWidthUnits.FormattingEnabled = true;
            this.lbColumnWidthUnits.Location = new System.Drawing.Point(229, 124);
            this.lbColumnWidthUnits.Name = "lbColumnWidthUnits";
            this.lbColumnWidthUnits.Size = new System.Drawing.Size(56, 17);
            this.lbColumnWidthUnits.TabIndex = 33;
            // 
            // lbLineHeightUnits
            // 
            this.lbLineHeightUnits.AllowDrop = true;
            this.lbLineHeightUnits.FormattingEnabled = true;
            this.lbLineHeightUnits.Location = new System.Drawing.Point(218, 96);
            this.lbLineHeightUnits.Name = "lbLineHeightUnits";
            this.lbLineHeightUnits.Size = new System.Drawing.Size(56, 17);
            this.lbLineHeightUnits.TabIndex = 34;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(504, 27);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 35;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // tbGraphicFileName
            // 
            this.tbGraphicFileName.Enabled = false;
            this.tbGraphicFileName.Location = new System.Drawing.Point(180, 27);
            this.tbGraphicFileName.Name = "tbGraphicFileName";
            this.tbGraphicFileName.Size = new System.Drawing.Size(318, 20);
            this.tbGraphicFileName.TabIndex = 36;
            // 
            // tbScale
            // 
            this.tbScale.Enabled = false;
            this.tbScale.Location = new System.Drawing.Point(133, 491);
            this.tbScale.Name = "tbScale";
            this.tbScale.Size = new System.Drawing.Size(41, 20);
            this.tbScale.TabIndex = 37;
            this.tbScale.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblScale
            // 
            this.lblScale.AutoSize = true;
            this.lblScale.Location = new System.Drawing.Point(136, 475);
            this.lblScale.Name = "lblScale";
            this.lblScale.Size = new System.Drawing.Size(34, 13);
            this.lblScale.TabIndex = 38;
            this.lblScale.Text = "Scale";
            // 
            // lblGraphicWidthInGr
            // 
            this.lblGraphicWidthInGr.AutoSize = true;
            this.lblGraphicWidthInGr.Location = new System.Drawing.Point(153, 61);
            this.lblGraphicWidthInGr.Name = "lblGraphicWidthInGr";
            this.lblGraphicWidthInGr.Size = new System.Drawing.Size(95, 13);
            this.lblGraphicWidthInGr.TabIndex = 40;
            this.lblGraphicWidthInGr.Text = "Graphic width in gr";
            // 
            // NumericUpDownWidthInGr
            // 
            this.NumericUpDownWidthInGr.AllowDrop = true;
            this.NumericUpDownWidthInGr.DecimalPlaces = 1;
            this.NumericUpDownWidthInGr.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.NumericUpDownWidthInGr.Location = new System.Drawing.Point(156, 89);
            this.NumericUpDownWidthInGr.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.NumericUpDownWidthInGr.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.NumericUpDownWidthInGr.Name = "NumericUpDownWidthInGr";
            this.NumericUpDownWidthInGr.Size = new System.Drawing.Size(102, 20);
            this.NumericUpDownWidthInGr.TabIndex = 41;
            this.NumericUpDownWidthInGr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumericUpDownWidthInGr.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // NumericUpDownHeightInLines
            // 
            this.NumericUpDownHeightInLines.Location = new System.Drawing.Point(4, 487);
            this.NumericUpDownHeightInLines.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.NumericUpDownHeightInLines.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.NumericUpDownHeightInLines.Name = "NumericUpDownHeightInLines";
            this.NumericUpDownHeightInLines.Size = new System.Drawing.Size(102, 20);
            this.NumericUpDownHeightInLines.TabIndex = 42;
            this.NumericUpDownHeightInLines.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumericUpDownHeightInLines.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // NumericUpDownMarginRightInPoints
            // 
            this.NumericUpDownMarginRightInPoints.Location = new System.Drawing.Point(4, 371);
            this.NumericUpDownMarginRightInPoints.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.NumericUpDownMarginRightInPoints.Name = "NumericUpDownMarginRightInPoints";
            this.NumericUpDownMarginRightInPoints.Size = new System.Drawing.Size(102, 20);
            this.NumericUpDownMarginRightInPoints.TabIndex = 43;
            this.NumericUpDownMarginRightInPoints.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // NumericUpDownMarginLeftInPoints
            // 
            this.NumericUpDownMarginLeftInPoints.Location = new System.Drawing.Point(4, 329);
            this.NumericUpDownMarginLeftInPoints.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.NumericUpDownMarginLeftInPoints.Name = "NumericUpDownMarginLeftInPoints";
            this.NumericUpDownMarginLeftInPoints.Size = new System.Drawing.Size(102, 20);
            this.NumericUpDownMarginLeftInPoints.TabIndex = 44;
            this.NumericUpDownMarginLeftInPoints.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // NumericUpDownMarginBottomInLines
            // 
            this.NumericUpDownMarginBottomInLines.Location = new System.Drawing.Point(4, 287);
            this.NumericUpDownMarginBottomInLines.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.NumericUpDownMarginBottomInLines.Name = "NumericUpDownMarginBottomInLines";
            this.NumericUpDownMarginBottomInLines.Size = new System.Drawing.Size(102, 20);
            this.NumericUpDownMarginBottomInLines.TabIndex = 45;
            this.NumericUpDownMarginBottomInLines.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // NumericUpDownMarginTopInLines
            // 
            this.NumericUpDownMarginTopInLines.Location = new System.Drawing.Point(4, 245);
            this.NumericUpDownMarginTopInLines.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.NumericUpDownMarginTopInLines.Name = "NumericUpDownMarginTopInLines";
            this.NumericUpDownMarginTopInLines.Size = new System.Drawing.Size(102, 20);
            this.NumericUpDownMarginTopInLines.TabIndex = 46;
            this.NumericUpDownMarginTopInLines.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblMarginLeftInPoints
            // 
            this.lblMarginLeftInPoints.AutoSize = true;
            this.lblMarginLeftInPoints.Location = new System.Drawing.Point(7, 312);
            this.lblMarginLeftInPoints.Name = "lblMarginLeftInPoints";
            this.lblMarginLeftInPoints.Size = new System.Drawing.Size(97, 13);
            this.lblMarginLeftInPoints.TabIndex = 48;
            this.lblMarginLeftInPoints.Text = "Margin Left (points)";
            // 
            // lblMarginRightInPoints
            // 
            this.lblMarginRightInPoints.AutoSize = true;
            this.lblMarginRightInPoints.Location = new System.Drawing.Point(7, 355);
            this.lblMarginRightInPoints.Name = "lblMarginRightInPoints";
            this.lblMarginRightInPoints.Size = new System.Drawing.Size(104, 13);
            this.lblMarginRightInPoints.TabIndex = 49;
            this.lblMarginRightInPoints.Text = "Margin Right (points)";
            // 
            // Panel1
            // 
            this.Panel1.Controls.Add(this.rbPictureSpans1Column);
            this.Panel1.Controls.Add(this.rbPictureSpans2Columns);
            this.Panel1.Location = new System.Drawing.Point(6, 70);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(106, 52);
            this.Panel1.TabIndex = 50;
            // 
            // rbPictureSpans1Column
            // 
            this.rbPictureSpans1Column.AutoSize = true;
            this.rbPictureSpans1Column.Location = new System.Drawing.Point(18, 6);
            this.rbPictureSpans1Column.Name = "rbPictureSpans1Column";
            this.rbPictureSpans1Column.Size = new System.Drawing.Size(69, 17);
            this.rbPictureSpans1Column.TabIndex = 1;
            this.rbPictureSpans1Column.TabStop = true;
            this.rbPictureSpans1Column.Text = "1 Column";
            this.rbPictureSpans1Column.UseVisualStyleBackColor = true;
            // 
            // rbPictureSpans2Columns
            // 
            this.rbPictureSpans2Columns.AutoSize = true;
            this.rbPictureSpans2Columns.Location = new System.Drawing.Point(18, 29);
            this.rbPictureSpans2Columns.Name = "rbPictureSpans2Columns";
            this.rbPictureSpans2Columns.Size = new System.Drawing.Size(69, 17);
            this.rbPictureSpans2Columns.TabIndex = 0;
            this.rbPictureSpans2Columns.TabStop = true;
            this.rbPictureSpans2Columns.Text = "2 Column";
            this.rbPictureSpans2Columns.UseVisualStyleBackColor = true;
            // 
            // NumericUpDownPercentOfColumnForPicture
            // 
            this.NumericUpDownPercentOfColumnForPicture.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NumericUpDownPercentOfColumnForPicture.Location = new System.Drawing.Point(4, 436);
            this.NumericUpDownPercentOfColumnForPicture.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NumericUpDownPercentOfColumnForPicture.Name = "NumericUpDownPercentOfColumnForPicture";
            this.NumericUpDownPercentOfColumnForPicture.Size = new System.Drawing.Size(102, 20);
            this.NumericUpDownPercentOfColumnForPicture.TabIndex = 51;
            this.NumericUpDownPercentOfColumnForPicture.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumericUpDownPercentOfColumnForPicture.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.NumericUpDownPercentOfColumnForPicture.Click += new System.EventHandler(this.NumericUpDownPercentOfColumnForPicture_ValueChanged);
            // 
            // lblPicturePercentWidth
            // 
            this.lblPicturePercentWidth.AutoSize = true;
            this.lblPicturePercentWidth.Location = new System.Drawing.Point(7, 418);
            this.lblPicturePercentWidth.Name = "lblPicturePercentWidth";
            this.lblPicturePercentWidth.Size = new System.Drawing.Size(134, 13);
            this.lblPicturePercentWidth.TabIndex = 52;
            this.lblPicturePercentWidth.Text = "Percent of width for picture";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(4, 57);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(75, 13);
            this.Label1.TabIndex = 53;
            this.Label1.Text = "Graphic spans";
            // 
            // picture
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 516);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.lblPicturePercentWidth);
            this.Controls.Add(this.NumericUpDownPercentOfColumnForPicture);
            this.Controls.Add(this.Panel1);
            this.Controls.Add(this.lblMarginRightInPoints);
            this.Controls.Add(this.lblMarginLeftInPoints);
            this.Controls.Add(this.NumericUpDownMarginTopInLines);
            this.Controls.Add(this.NumericUpDownMarginBottomInLines);
            this.Controls.Add(this.NumericUpDownMarginLeftInPoints);
            this.Controls.Add(this.NumericUpDownMarginRightInPoints);
            this.Controls.Add(this.NumericUpDownHeightInLines);
            this.Controls.Add(this.lblScale);
            this.Controls.Add(this.tbScale);
            this.Controls.Add(this.tbGraphicFileName);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.lblPublishedHeight);
            this.Controls.Add(this.lblPublishedWidth);
            this.Controls.Add(this.tbPublishedHeightInPixels);
            this.Controls.Add(this.tbPublishedWidthInPixels);
            this.Controls.Add(this.lbOriginalHeightUnits);
            this.Controls.Add(this.lbOriginalWidthUnits);
            this.Controls.Add(this.lblHeight);
            this.Controls.Add(this.lblImageWidth);
            this.Controls.Add(this.lblColumnWidthInPoints);
            this.Controls.Add(this.lblLineHeightInPoints);
            this.Controls.Add(this.tbPictureHeightInPixels);
            this.Controls.Add(this.tbPictureWidthInPixels);
            this.Controls.Add(this.tbColumnWidth);
            this.Controls.Add(this.tbLineHeight);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnInsertGraphicHere);
            this.Controls.Add(this.lblGraphicFloat);
            this.Controls.Add(this.lblGraphicSize);
            this.Controls.Add(this.lblMarginBottomInLines);
            this.Controls.Add(this.lblMarginTopInLines);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.panelPosition);
            this.Controls.Add(this.lblGraphicWidthInGr);
            this.Controls.Add(this.NumericUpDownWidthInGr);
            this.Controls.Add(this.lbColumnWidthUnits);
            this.Controls.Add(this.lbLineHeightUnits);
            this.Name = "picture";
            this.Text = "picture";
            this.Load += new System.EventHandler(this.picture_Load);
            this.panelPosition.ResumeLayout(false);
            this.panelPosition.PerformLayout();
            this.Panel3.ResumeLayout(false);
            this.Panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownWidthInGr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownHeightInLines)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownMarginRightInPoints)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownMarginLeftInPoints)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownMarginBottomInLines)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownMarginTopInLines)).EndInit();
            this.Panel1.ResumeLayout(false);
            this.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownPercentOfColumnForPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        internal System.Windows.Forms.RadioButton rbTop;
        internal System.Windows.Forms.RadioButton rbBottom;
        internal System.Windows.Forms.RadioButton rbInline;
        internal System.Windows.Forms.Panel panelPosition;
        internal System.Windows.Forms.PictureBox pictureBox2;
        internal System.Windows.Forms.Button btnOK;
        internal System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.Label lblMarginTopInLines;
        internal System.Windows.Forms.Label lblMarginBottomInLines;
        internal System.Windows.Forms.Label lblGraphicSize;
        internal System.Windows.Forms.Label lblGraphicFloat;
        internal System.Windows.Forms.Button btnInsertGraphicHere;
        internal System.Windows.Forms.Button btnRemove;
        internal System.Windows.Forms.Panel Panel3;
        internal System.Windows.Forms.RadioButton RadioButton1;
        internal System.Windows.Forms.RadioButton RadioButton2;
        internal System.Windows.Forms.RadioButton RadioButton3;
        internal System.Windows.Forms.OpenFileDialog PictureDialog;
        internal System.Windows.Forms.TextBox tbLineHeight;
        internal System.Windows.Forms.TextBox tbColumnWidth;
        internal System.Windows.Forms.TextBox tbPictureWidthInPixels;
        internal System.Windows.Forms.TextBox tbPictureHeightInPixels;
        internal System.Windows.Forms.Label lblLineHeightInPoints;
        internal System.Windows.Forms.Label lblColumnWidthInPoints;
        internal System.Windows.Forms.Label lblImageWidth;
        internal System.Windows.Forms.Label lblHeight;
        internal System.Windows.Forms.ListBox lbOriginalWidthUnits;
        internal System.Windows.Forms.ListBox lbOriginalHeightUnits;
        internal System.Windows.Forms.Label lblPublishedHeight;
        internal System.Windows.Forms.Label lblPublishedWidth;
        internal System.Windows.Forms.TextBox tbPublishedHeightInPixels;
        internal System.Windows.Forms.TextBox tbPublishedWidthInPixels;
        internal System.Windows.Forms.ListBox lbColumnWidthUnits;
        internal System.Windows.Forms.ListBox lbLineHeightUnits;
        internal System.Windows.Forms.Button btnBrowse;
        internal System.Windows.Forms.TextBox tbGraphicFileName;
        internal System.Windows.Forms.TextBox tbScale;
        internal System.Windows.Forms.Label lblScale;
        internal System.Windows.Forms.Label lblGraphicWidthInGr;
        internal System.Windows.Forms.NumericUpDown NumericUpDownWidthInGr;
        internal System.Windows.Forms.NumericUpDown NumericUpDownHeightInLines;
        internal System.Windows.Forms.NumericUpDown NumericUpDownMarginRightInPoints;
        internal System.Windows.Forms.NumericUpDown NumericUpDownMarginLeftInPoints;
        internal System.Windows.Forms.NumericUpDown NumericUpDownMarginBottomInLines;
        internal System.Windows.Forms.NumericUpDown NumericUpDownMarginTopInLines;
        internal System.Windows.Forms.Label lblMarginLeftInPoints;
        internal System.Windows.Forms.Label lblMarginRightInPoints;
        internal System.Windows.Forms.Panel Panel1;
        internal System.Windows.Forms.RadioButton rbPictureSpans1Column;
        internal System.Windows.Forms.RadioButton rbPictureSpans2Columns;
        internal System.Windows.Forms.NumericUpDown NumericUpDownPercentOfColumnForPicture;
        internal System.Windows.Forms.Label lblPicturePercentWidth;
        internal System.Windows.Forms.Label Label1;

    }
}