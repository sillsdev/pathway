using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace PrincessConvert
{



    partial class PDFdisplay : System.Windows.Forms.Form
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
            this.cbShowPDFToolbar = new System.Windows.Forms.CheckBox();
            this.cbShowSinglePage = new System.Windows.Forms.CheckBox();
            this.cbShowThumbnails = new System.Windows.Forms.CheckBox();
            this.cbShowScrollBars = new System.Windows.Forms.CheckBox();
            this.cbPDFshowBookmarks = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.numericUpDownPDFzoom = new System.Windows.Forms.NumericUpDown();
            this.cbPDFzoomOn = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPDFzoom)).BeginInit();
            this.SuspendLayout();
            // 
            // cbShowPDFToolbar
            // 
            this.cbShowPDFToolbar.AutoSize = true;
            this.cbShowPDFToolbar.Location = new System.Drawing.Point(12, 12);
            this.cbShowPDFToolbar.Name = "cbShowPDFToolbar";
            this.cbShowPDFToolbar.Size = new System.Drawing.Size(112, 17);
            this.cbShowPDFToolbar.TabIndex = 0;
            this.cbShowPDFToolbar.Text = "Show PDF toolbar";
            this.cbShowPDFToolbar.UseVisualStyleBackColor = true;
            this.cbShowPDFToolbar.Click += new System.EventHandler(this.cbPDFshowBookmarks_CheckedChanged);
            // 
            // cbShowSinglePage
            // 
            this.cbShowSinglePage.AutoSize = true;
            this.cbShowSinglePage.Location = new System.Drawing.Point(12, 51);
            this.cbShowSinglePage.Name = "cbShowSinglePage";
            this.cbShowSinglePage.Size = new System.Drawing.Size(110, 17);
            this.cbShowSinglePage.TabIndex = 1;
            this.cbShowSinglePage.Text = "Show single page";
            this.cbShowSinglePage.UseVisualStyleBackColor = true;
            // 
            // cbShowThumbnails
            // 
            this.cbShowThumbnails.AutoSize = true;
            this.cbShowThumbnails.Location = new System.Drawing.Point(12, 90);
            this.cbShowThumbnails.Name = "cbShowThumbnails";
            this.cbShowThumbnails.Size = new System.Drawing.Size(106, 17);
            this.cbShowThumbnails.TabIndex = 2;
            this.cbShowThumbnails.Text = "Show thumbnails";
            this.cbShowThumbnails.UseVisualStyleBackColor = true;
            this.cbShowThumbnails.Click += new System.EventHandler(this.cbShowThumbnails_CheckedChanged);
            this.cbShowThumbnails.CheckedChanged += new System.EventHandler(this.cbShowThumbnails_CheckedChanged);
            // 
            // cbShowScrollBars
            // 
            this.cbShowScrollBars.AutoSize = true;
            this.cbShowScrollBars.Location = new System.Drawing.Point(12, 129);
            this.cbShowScrollBars.Name = "cbShowScrollBars";
            this.cbShowScrollBars.Size = new System.Drawing.Size(100, 17);
            this.cbShowScrollBars.TabIndex = 3;
            this.cbShowScrollBars.Text = "Show scrollbars";
            this.cbShowScrollBars.UseVisualStyleBackColor = true;
            // 
            // cbPDFshowBookmarks
            // 
            this.cbPDFshowBookmarks.AutoSize = true;
            this.cbPDFshowBookmarks.Location = new System.Drawing.Point(124, 90);
            this.cbPDFshowBookmarks.Name = "cbPDFshowBookmarks";
            this.cbPDFshowBookmarks.Size = new System.Drawing.Size(108, 17);
            this.cbPDFshowBookmarks.TabIndex = 4;
            this.cbPDFshowBookmarks.Text = "Show bookmarks";
            this.cbPDFshowBookmarks.UseVisualStyleBackColor = true;
            this.cbPDFshowBookmarks.Click += new System.EventHandler(this.cbPDFshowBookmarks_CheckedChanged);
            this.cbPDFshowBookmarks.CheckedChanged += new System.EventHandler(this.cbPDFshowBookmarks_CheckedChanged);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(162, 227);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(56, 27);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(224, 227);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(56, 27);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // numericUpDownPDFzoom
            // 
            this.numericUpDownPDFzoom.Location = new System.Drawing.Point(124, 167);
            this.numericUpDownPDFzoom.Maximum = new decimal(new int[] {
            1600,
            0,
            0,
            0});
            this.numericUpDownPDFzoom.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownPDFzoom.Name = "numericUpDownPDFzoom";
            this.numericUpDownPDFzoom.Size = new System.Drawing.Size(59, 20);
            this.numericUpDownPDFzoom.TabIndex = 7;
            this.numericUpDownPDFzoom.Value = new decimal(new int[] {
            67,
            0,
            0,
            0});
            // 
            // cbPDFzoomOn
            // 
            this.cbPDFzoomOn.AutoSize = true;
            this.cbPDFzoomOn.Location = new System.Drawing.Point(12, 167);
            this.cbPDFzoomOn.Name = "cbPDFzoomOn";
            this.cbPDFzoomOn.Size = new System.Drawing.Size(111, 17);
            this.cbPDFzoomOn.TabIndex = 8;
            this.cbPDFzoomOn.Text = "Use zoom amount";
            this.cbPDFzoomOn.UseVisualStyleBackColor = true;
            // 
            // PDFdisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.cbPDFzoomOn);
            this.Controls.Add(this.numericUpDownPDFzoom);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cbPDFshowBookmarks);
            this.Controls.Add(this.cbShowScrollBars);
            this.Controls.Add(this.cbShowThumbnails);
            this.Controls.Add(this.cbShowSinglePage);
            this.Controls.Add(this.cbShowPDFToolbar);
            this.Name = "PDFdisplay";
            this.Text = "PDFdisplay";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPDFzoom)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        internal System.Windows.Forms.CheckBox cbShowPDFToolbar;
        internal System.Windows.Forms.CheckBox cbShowSinglePage;
        internal System.Windows.Forms.CheckBox cbShowThumbnails;
        internal System.Windows.Forms.CheckBox cbShowScrollBars;
        internal System.Windows.Forms.CheckBox cbPDFshowBookmarks;
        internal System.Windows.Forms.Button btnOK;
        protected internal System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.NumericUpDown numericUpDownPDFzoom;
        internal System.Windows.Forms.CheckBox cbPDFzoomOn;

    }
}