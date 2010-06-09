namespace SIL.PublishingSolution
{
    partial class PopupForm
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
            this.txtReturnName = new System.Windows.Forms.TextBox();
            this.txtReturnFileName = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.lblFileName = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblNote = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtReturnName
            // 
            this.txtReturnName.AccessibleName = "txtReturnName";
            this.txtReturnName.Location = new System.Drawing.Point(21, 33);
            this.txtReturnName.Name = "txtReturnName";
            this.txtReturnName.Size = new System.Drawing.Size(269, 21);
            this.txtReturnName.TabIndex = 0;
            // 
            // txtReturnFileName
            // 
            this.txtReturnFileName.AccessibleName = "txtReturnFileName";
            this.txtReturnFileName.Location = new System.Drawing.Point(21, 84);
            this.txtReturnFileName.Name = "txtReturnFileName";
            this.txtReturnFileName.Size = new System.Drawing.Size(508, 21);
            this.txtReturnFileName.TabIndex = 1;
            // 
            // btnOk
            // 
            this.btnOk.AccessibleName = "btnOk";
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(436, 116);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(77, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // lblName
            // 
            this.lblName.AccessibleName = "lblName";
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(18, 17);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(89, 13);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "Section name ";
            // 
            // lblFileName
            // 
            this.lblFileName.AccessibleName = "lblFileName";
            this.lblFileName.AutoSize = true;
            this.lblFileName.Location = new System.Drawing.Point(18, 68);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(67, 13);
            this.lblFileName.TabIndex = 4;
            this.lblFileName.Text = "File Name ";
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleName = "btnCancel";
            this.btnCancel.Location = new System.Drawing.Point(519, 116);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(77, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // lblNote
            // 
            this.lblNote.AccessibleName = "lblNote";
            this.lblNote.AutoSize = true;
            this.lblNote.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNote.Location = new System.Drawing.Point(18, 126);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(348, 13);
            this.lblNote.TabIndex = 6;
            this.lblNote.Text = "Note: Empty File Name will be treated as Blank Page";
            // 
            // btnBrowse
            // 
            this.btnBrowse.AccessibleName = "btnBrowse";
            this.btnBrowse.Location = new System.Drawing.Point(535, 84);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(61, 21);
            this.btnBrowse.TabIndex = 7;
            this.btnBrowse.Text = "&Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.BtnBrowse_Click);
            // 
            // PopupForm
            // 
            this.AccessibleName = "PopupForm";
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 160);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.lblNote);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblFileName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtReturnFileName);
            this.Controls.Add(this.txtReturnName);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PopupForm";
            this.ShowIcon = false;
            this.Text = "PopupForm";
            this.Load += new System.EventHandler(this.PopupForm_Load);
            this.DoubleClick += new System.EventHandler(this.PopupForm_DoubleClick);
            this.Activated += new System.EventHandler(this.PopupForm_Activated);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtReturnName;
        private System.Windows.Forms.TextBox txtReturnFileName;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.Button btnBrowse;
    }
}
