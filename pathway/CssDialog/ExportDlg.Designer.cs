namespace SIL.PublishingSolution
{
    partial class ExportDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportDlg));
            this.lblExportType = new System.Windows.Forms.Label();
            this.cmbExportType = new System.Windows.Forms.ComboBox();
            this.btn_Ok = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblExportType
            // 
            this.lblExportType.AccessibleName = "lblExportType";
            this.lblExportType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExportType.ForeColor = System.Drawing.Color.Black;
            this.lblExportType.Location = new System.Drawing.Point(1, 24);
            this.lblExportType.Name = "lblExportType";
            this.lblExportType.Size = new System.Drawing.Size(75, 13);
            this.lblExportType.TabIndex = 0;
            this.lblExportType.Text = "Export Type";
            this.lblExportType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbExportType
            // 
            this.cmbExportType.AccessibleName = "cmbExportType";
            this.cmbExportType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExportType.FormattingEnabled = true;
            this.cmbExportType.Location = new System.Drawing.Point(82, 21);
            this.cmbExportType.Name = "cmbExportType";
            this.cmbExportType.Size = new System.Drawing.Size(207, 21);
            this.cmbExportType.TabIndex = 1;
            // 
            // btn_Ok
            // 
            this.btn_Ok.AccessibleName = "btn_Ok";
            this.btn_Ok.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Ok.Location = new System.Drawing.Point(115, 63);
            this.btn_Ok.Name = "btn_Ok";
            this.btn_Ok.Size = new System.Drawing.Size(84, 23);
            this.btn_Ok.TabIndex = 2;
            this.btn_Ok.Text = "&Ok";
            this.btn_Ok.UseVisualStyleBackColor = true;
            this.btn_Ok.Click += new System.EventHandler(this.btn_Ok_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleName = "btnCancel";
            this.btnCancel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(205, 63);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(84, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ExportDlg
            // 
            this.AcceptButton = this.btn_Ok;
            this.AccessibleName = "ExportDlg";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 103);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btn_Ok);
            this.Controls.Add(this.cmbExportType);
            this.Controls.Add(this.lblExportType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportDlg";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export";
            this.Load += new System.EventHandler(this.ExportDlg_Load);
            this.DoubleClick += new System.EventHandler(this.ExportDlg_DoubleClick);
            this.Activated += new System.EventHandler(this.ExportDlg_Activated);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblExportType;
        private System.Windows.Forms.ComboBox cmbExportType;
        private System.Windows.Forms.Button btn_Ok;
        private System.Windows.Forms.Button btnCancel;
    }
}
