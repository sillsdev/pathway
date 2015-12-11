using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace PrincessConvert
{
    partial class createCSS : System.Windows.Forms.Form
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
            this.btnConvert = new System.Windows.Forms.Button();
            this.lblGPSStylesheetName = new System.Windows.Forms.Label();
            this.lblGeneratedStylesheetName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            //btnConvert
            //
            this.btnConvert.Location = new System.Drawing.Point(189, 213);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(75, 23);
            this.btnConvert.TabIndex = 0;
            this.btnConvert.Text = "Button1";
            this.btnConvert.UseVisualStyleBackColor = true;
            //
            //lblGPSStylesheetName
            //
            this.lblGPSStylesheetName.AutoSize = true;
            this.lblGPSStylesheetName.Location = new System.Drawing.Point(44, 42);
            this.lblGPSStylesheetName.Name = "lblGPSStylesheetName";
            this.lblGPSStylesheetName.Size = new System.Drawing.Size(142, 13);
            this.lblGPSStylesheetName.TabIndex = 1;
            this.lblGPSStylesheetName.Text = "xxx GPS stylesheet file name";
            //
            //lblGeneratedStylesheetName
            //
            this.lblGeneratedStylesheetName.AutoSize = true;
            this.lblGeneratedStylesheetName.Location = new System.Drawing.Point(44, 55);
            this.lblGeneratedStylesheetName.Name = "lblGeneratedStylesheetName";
            this.lblGeneratedStylesheetName.Size = new System.Drawing.Size(168, 13);
            this.lblGeneratedStylesheetName.TabIndex = 2;
            this.lblGeneratedStylesheetName.Text = "xxx generated stylesheet file name";
            //
            //createCSS
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 266);
            this.Controls.Add(this.lblGeneratedStylesheetName);
            this.Controls.Add(this.lblGPSStylesheetName);
            this.Controls.Add(this.btnConvert);
            this.Name = "createCSS";
            this.Text = "createCSS";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        internal System.Windows.Forms.Button btnConvert;
        internal System.Windows.Forms.Label lblGPSStylesheetName;
        internal System.Windows.Forms.Label lblGeneratedStylesheetName;

    }
}