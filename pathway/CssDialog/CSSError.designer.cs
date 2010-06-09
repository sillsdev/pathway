namespace SIL.PublishingSolution
{
    partial class CSSError
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        //private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            //if (disposing && (components != null))
            //{
            //    components.Dispose();
            //}
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CSSWindow = new System.Windows.Forms.RichTextBox();
            this.lstErrorReport = new System.Windows.Forms.ListBox();
            this.BtnClose = new System.Windows.Forms.Button();
            this.BtnSave = new System.Windows.Forms.Button();
            this.lblCSSFileName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CSSWindow
            // 
            this.CSSWindow.AccessibleName = "CSSWindow";
            this.CSSWindow.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CSSWindow.Location = new System.Drawing.Point(12, 32);
            this.CSSWindow.Name = "CSSWindow";
            this.CSSWindow.Size = new System.Drawing.Size(665, 268);
            this.CSSWindow.TabIndex = 0;
            this.CSSWindow.Text = "";
            // 
            // lstErrorReport
            // 
            this.lstErrorReport.AccessibleName = "lstErrorReport";
            this.lstErrorReport.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstErrorReport.FormattingEnabled = true;
            this.lstErrorReport.ItemHeight = 16;
            this.lstErrorReport.Location = new System.Drawing.Point(13, 308);
            this.lstErrorReport.Name = "lstErrorReport";
            this.lstErrorReport.Size = new System.Drawing.Size(665, 132);
            this.lstErrorReport.TabIndex = 1;
            this.lstErrorReport.Click += new System.EventHandler(this.lstErrorReport_Click);
            // 
            // BtnClose
            // 
            this.BtnClose.AccessibleName = "BtnClose";
            this.BtnClose.Location = new System.Drawing.Point(603, 449);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(75, 23);
            this.BtnClose.TabIndex = 3;
            this.BtnClose.Text = "&Close";
            this.BtnClose.UseVisualStyleBackColor = true;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // BtnSave
            // 
            this.BtnSave.AccessibleName = "BtnSave";
            this.BtnSave.Location = new System.Drawing.Point(522, 449);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(75, 23);
            this.BtnSave.TabIndex = 2;
            this.BtnSave.Text = "&Save";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // lblCSSFileName
            // 
            this.lblCSSFileName.AccessibleName = "lblCSSFileName";
            this.lblCSSFileName.AutoSize = true;
            this.lblCSSFileName.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCSSFileName.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblCSSFileName.Location = new System.Drawing.Point(13, 15);
            this.lblCSSFileName.Name = "lblCSSFileName";
            this.lblCSSFileName.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblCSSFileName.Size = new System.Drawing.Size(58, 14);
            this.lblCSSFileName.TabIndex = 4;
            this.lblCSSFileName.Text = "CSSFile";
            this.lblCSSFileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CSSError
            // 
            this.AccessibleName = "CSSError";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 478);
            this.Controls.Add(this.lblCSSFileName);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.BtnClose);
            this.Controls.Add(this.lstErrorReport);
            this.Controls.Add(this.CSSWindow);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CSSError";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CSSError";
            this.Load += new System.EventHandler(this.CSSError_Load);
            this.DoubleClick += new System.EventHandler(this.CSSError_DoubleClick);
            this.Activated += new System.EventHandler(this.CSSError_Activated);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox CSSWindow;
        private System.Windows.Forms.ListBox lstErrorReport;
        private System.Windows.Forms.Button BtnClose;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Label lblCSSFileName;
    }
}
