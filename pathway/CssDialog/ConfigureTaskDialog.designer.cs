// --------------------------------------------------------------------------------------------
// <copyright file="OOMapProperty.cs" from='2009' to='2014' company='SIL International'>
//      Copyright (C) 2009, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Implements Fieldworks Utility Interface for Pathway
// </remarks>
// --------------------------------------------------------------------------------------------

namespace SIL.PublishingSolution
{
    partial class ConfigureTaskDialog
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
            this.txtTaskName = new System.Windows.Forms.TextBox();
            this.BtOk = new System.Windows.Forms.Button();
            this.BtCancel = new System.Windows.Forms.Button();
            this.LblCaption = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.CmbStyleName = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // txtTaskName
            // 
            this.txtTaskName.Location = new System.Drawing.Point(80, 12);
            this.txtTaskName.Name = "txtTaskName";
            this.txtTaskName.Size = new System.Drawing.Size(207, 20);
            this.txtTaskName.TabIndex = 0;
            // 
            // BtOk
            // 
            this.BtOk.Location = new System.Drawing.Point(129, 70);
            this.BtOk.Name = "BtOk";
            this.BtOk.Size = new System.Drawing.Size(75, 23);
            this.BtOk.TabIndex = 1;
            this.BtOk.Text = "&Ok";
            this.BtOk.UseVisualStyleBackColor = true;
            this.BtOk.Click += new System.EventHandler(this.BtOk_Click);
            // 
            // BtCancel
            // 
            this.BtCancel.Location = new System.Drawing.Point(210, 70);
            this.BtCancel.Name = "BtCancel";
            this.BtCancel.Size = new System.Drawing.Size(75, 23);
            this.BtCancel.TabIndex = 2;
            this.BtCancel.Text = "&Cancel";
            this.BtCancel.UseVisualStyleBackColor = true;
            this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // LblCaption
            // 
            this.LblCaption.AutoSize = true;
            this.LblCaption.Location = new System.Drawing.Point(12, 15);
            this.LblCaption.Name = "LblCaption";
            this.LblCaption.Size = new System.Drawing.Size(62, 13);
            this.LblCaption.TabIndex = 3;
            this.LblCaption.Text = "Task Name";
            this.LblCaption.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Style Name";
            // 
            // CmbStyleName
            // 
            this.CmbStyleName.FormattingEnabled = true;
            this.CmbStyleName.Location = new System.Drawing.Point(79, 42);
            this.CmbStyleName.Name = "CmbStyleName";
            this.CmbStyleName.Size = new System.Drawing.Size(206, 21);
            this.CmbStyleName.TabIndex = 5;
            // 
            // ConfigureTaskDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(303, 105);
            this.Controls.Add(this.CmbStyleName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LblCaption);
            this.Controls.Add(this.BtCancel);
            this.Controls.Add(this.BtOk);
            this.Controls.Add(this.txtTaskName);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigureTaskDialog";
            this.ShowIcon = false;
            this.Text = "Configure Task Dialog";
            this.Load += new System.EventHandler(this.ConfigureTaskDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTaskName;
        private System.Windows.Forms.Button BtOk;
        private System.Windows.Forms.Button BtCancel;
        private System.Windows.Forms.Label LblCaption;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CmbStyleName;
    }
}