using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace PrincessConvert
{

partial class multipleFiles : System.Windows.Forms.Form
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
        this.lbFiles = new System.Windows.Forms.ListBox();
        this.btnCancel = new System.Windows.Forms.Button();
        this.btnOK = new System.Windows.Forms.Button();
        this.btnUp = new System.Windows.Forms.Button();
        this.btnDown = new System.Windows.Forms.Button();
        this.btnUpStart = new System.Windows.Forms.Button();
        this.btnDownEnd = new System.Windows.Forms.Button();
        this.btnBrowse = new System.Windows.Forms.Button();
        this.FolderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
        this.lblDocumentsFolderName = new System.Windows.Forms.Label();
        this.btnRemoveSelectedFiles = new System.Windows.Forms.Button();
        this.btnSort = new System.Windows.Forms.Button();
        this.lblTotalNumberOfInputFilesToProcess = new System.Windows.Forms.Label();
        this.tbTotalNumberOfInputFilesToProcess = new System.Windows.Forms.TextBox();
        this.rbSingleOutputFile = new System.Windows.Forms.RadioButton();
        this.SuspendLayout();
        // 
        // lbFiles
        // 
        this.lbFiles.FormattingEnabled = true;
        this.lbFiles.Location = new System.Drawing.Point(37, 37);
        this.lbFiles.Name = "lbFiles";
        this.lbFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
        this.lbFiles.Size = new System.Drawing.Size(422, 173);
        this.lbFiles.TabIndex = 0;
        // 
        // btnCancel
        // 
        this.btnCancel.Location = new System.Drawing.Point(450, 299);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(75, 23);
        this.btnCancel.TabIndex = 1;
        this.btnCancel.Text = "Cancel";
        this.btnCancel.UseVisualStyleBackColor = true;
        this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
        // 
        // btnOK
        // 
        this.btnOK.Location = new System.Drawing.Point(369, 299);
        this.btnOK.Name = "btnOK";
        this.btnOK.Size = new System.Drawing.Size(75, 23);
        this.btnOK.TabIndex = 2;
        this.btnOK.Text = "OK";
        this.btnOK.UseVisualStyleBackColor = true;
        this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
        // 
        // btnUp
        // 
        this.btnUp.Location = new System.Drawing.Point(466, 65);
        this.btnUp.Name = "btnUp";
        this.btnUp.Size = new System.Drawing.Size(41, 23);
        this.btnUp.TabIndex = 4;
        this.btnUp.UseVisualStyleBackColor = true;
        this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
        // 
        // btnDown
        // 
        this.btnDown.Location = new System.Drawing.Point(466, 95);
        this.btnDown.Name = "btnDown";
        this.btnDown.Size = new System.Drawing.Size(41, 23);
        this.btnDown.TabIndex = 5;
        this.btnDown.UseVisualStyleBackColor = true;
        this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
        // 
        // btnUpStart
        // 
        this.btnUpStart.Location = new System.Drawing.Point(466, 36);
        this.btnUpStart.Name = "btnUpStart";
        this.btnUpStart.Size = new System.Drawing.Size(41, 23);
        this.btnUpStart.TabIndex = 6;
        this.btnUpStart.UseVisualStyleBackColor = true;
        this.btnUpStart.Click += new System.EventHandler(this.btnUpStart_Click);
        // 
        // btnDownEnd
        // 
        this.btnDownEnd.Location = new System.Drawing.Point(466, 124);
        this.btnDownEnd.Name = "btnDownEnd";
        this.btnDownEnd.Size = new System.Drawing.Size(41, 23);
        this.btnDownEnd.TabIndex = 7;
        this.btnDownEnd.UseVisualStyleBackColor = true;
        this.btnDownEnd.Click += new System.EventHandler(this.btnDownEnd_Click);
        // 
        // btnBrowse
        // 
        this.btnBrowse.AutoEllipsis = true;
        this.btnBrowse.Location = new System.Drawing.Point(384, 215);
        this.btnBrowse.Name = "btnBrowse";
        this.btnBrowse.Size = new System.Drawing.Size(75, 23);
        this.btnBrowse.TabIndex = 8;
        this.btnBrowse.Text = "Browse ...";
        this.btnBrowse.UseVisualStyleBackColor = true;
        this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
        // 
        // lblDocumentsFolderName
        // 
        this.lblDocumentsFolderName.AutoSize = true;
        this.lblDocumentsFolderName.Location = new System.Drawing.Point(43, 15);
        this.lblDocumentsFolderName.Name = "lblDocumentsFolderName";
        this.lblDocumentsFolderName.Size = new System.Drawing.Size(54, 13);
        this.lblDocumentsFolderName.TabIndex = 9;
        this.lblDocumentsFolderName.Text = "xxxLabel1";
        // 
        // btnRemoveSelectedFiles
        // 
        this.btnRemoveSelectedFiles.AutoEllipsis = true;
        this.btnRemoveSelectedFiles.Location = new System.Drawing.Point(465, 187);
        this.btnRemoveSelectedFiles.Name = "btnRemoveSelectedFiles";
        this.btnRemoveSelectedFiles.Size = new System.Drawing.Size(57, 23);
        this.btnRemoveSelectedFiles.TabIndex = 10;
        this.btnRemoveSelectedFiles.Text = "Remove";
        this.btnRemoveSelectedFiles.UseVisualStyleBackColor = true;
        this.btnRemoveSelectedFiles.Click += new System.EventHandler(this.btnRemoveSelectedFiles_Click);
        // 
        // btnSort
        // 
        this.btnSort.AutoEllipsis = true;
        this.btnSort.Location = new System.Drawing.Point(466, 158);
        this.btnSort.Name = "btnSort";
        this.btnSort.Size = new System.Drawing.Size(57, 23);
        this.btnSort.TabIndex = 11;
        this.btnSort.Text = "Sort";
        this.btnSort.UseVisualStyleBackColor = true;
        this.btnSort.Click += new System.EventHandler(this.btnSort_Click);
        // 
        // lblTotalNumberOfInputFilesToProcess
        // 
        this.lblTotalNumberOfInputFilesToProcess.AutoSize = true;
        this.lblTotalNumberOfInputFilesToProcess.Location = new System.Drawing.Point(84, 220);
        this.lblTotalNumberOfInputFilesToProcess.Name = "lblTotalNumberOfInputFilesToProcess";
        this.lblTotalNumberOfInputFilesToProcess.Size = new System.Drawing.Size(199, 13);
        this.lblTotalNumberOfInputFilesToProcess.TabIndex = 12;
        this.lblTotalNumberOfInputFilesToProcess.Text = "Input files processed in the above order. ";
        // 
        // tbTotalNumberOfInputFilesToProcess
        // 
        this.tbTotalNumberOfInputFilesToProcess.Location = new System.Drawing.Point(37, 217);
        this.tbTotalNumberOfInputFilesToProcess.Name = "tbTotalNumberOfInputFilesToProcess";
        this.tbTotalNumberOfInputFilesToProcess.Size = new System.Drawing.Size(41, 20);
        this.tbTotalNumberOfInputFilesToProcess.TabIndex = 13;
        this.tbTotalNumberOfInputFilesToProcess.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        // 
        // rbSingleOutputFile
        // 
        this.rbSingleOutputFile.AutoSize = true;
        this.rbSingleOutputFile.Checked = true;
        this.rbSingleOutputFile.Location = new System.Drawing.Point(37, 266);
        this.rbSingleOutputFile.Name = "rbSingleOutputFile";
        this.rbSingleOutputFile.Size = new System.Drawing.Size(282, 17);
        this.rbSingleOutputFile.TabIndex = 15;
        this.rbSingleOutputFile.TabStop = true;
        this.rbSingleOutputFile.Text = "One output file. Output filename based on folder name.";
        this.rbSingleOutputFile.UseVisualStyleBackColor = true;
        // 
        // multipleFiles
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(535, 329);
        this.Controls.Add(this.rbSingleOutputFile);
        this.Controls.Add(this.tbTotalNumberOfInputFilesToProcess);
        this.Controls.Add(this.lblTotalNumberOfInputFilesToProcess);
        this.Controls.Add(this.btnSort);
        this.Controls.Add(this.btnRemoveSelectedFiles);
        this.Controls.Add(this.lblDocumentsFolderName);
        this.Controls.Add(this.btnBrowse);
        this.Controls.Add(this.btnDownEnd);
        this.Controls.Add(this.btnUpStart);
        this.Controls.Add(this.btnDown);
        this.Controls.Add(this.btnUp);
        this.Controls.Add(this.btnOK);
        this.Controls.Add(this.btnCancel);
        this.Controls.Add(this.lbFiles);
        this.Name = "multipleFiles";
        this.Text = "Combine PDFs for these files in the displayed order.";
        this.Load += new System.EventHandler(this.multipleFiles_Load);
        this.ResumeLayout(false);
        this.PerformLayout();

    }
    internal System.Windows.Forms.ListBox lbFiles;
    internal System.Windows.Forms.Button btnCancel;
    internal System.Windows.Forms.Button btnOK;
    internal System.Windows.Forms.Button btnUp;
    internal System.Windows.Forms.Button btnDown;
    internal System.Windows.Forms.Button btnUpStart;
    internal System.Windows.Forms.Button btnDownEnd;
    internal System.Windows.Forms.Button btnBrowse;
    internal System.Windows.Forms.FolderBrowserDialog FolderBrowserDialog1;
    internal System.Windows.Forms.Label lblDocumentsFolderName;
    internal System.Windows.Forms.Button btnRemoveSelectedFiles;
    internal System.Windows.Forms.Button btnSort;
    internal System.Windows.Forms.Label lblTotalNumberOfInputFilesToProcess;
    internal System.Windows.Forms.TextBox tbTotalNumberOfInputFilesToProcess;
    internal System.Windows.Forms.RadioButton rbSingleOutputFile;
 
}
}