namespace epubValidator
{
    partial class ValidationDialog
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ValidationDialog));
			this.ofp = new System.Windows.Forms.OpenFileDialog();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.txtFilename = new System.Windows.Forms.TextBox();
			this.btnValidate = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.lblInstructions = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblStatus = new System.Windows.Forms.Label();
			this.l10NSharpExtender1 = new L10NSharp.UI.L10NSharpExtender(this.components);
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.l10NSharpExtender1)).BeginInit();
			this.SuspendLayout();
			// 
			// ofp
			// 
			this.ofp.DefaultExt = "*.epub";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.btnBrowse);
			this.groupBox1.Controls.Add(this.txtFilename);
			this.l10NSharpExtender1.SetLocalizableToolTip(this.groupBox1, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.groupBox1, null);
			this.l10NSharpExtender1.SetLocalizingId(this.groupBox1, "ValidationDialog.groupBox1");
			this.groupBox1.Location = new System.Drawing.Point(17, 124);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(527, 65);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "&File to Validate:";
			// 
			// btnBrowse
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.btnBrowse, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.btnBrowse, null);
			this.l10NSharpExtender1.SetLocalizingId(this.btnBrowse, "ValidationDialog.btnBrowse");
			this.btnBrowse.Location = new System.Drawing.Point(432, 25);
			this.btnBrowse.Margin = new System.Windows.Forms.Padding(4);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(75, 23);
			this.btnBrowse.TabIndex = 1;
			this.btnBrowse.Text = "&Browse...";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// txtFilename
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtFilename, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtFilename, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtFilename, "ValidationDialog.txtFilename");
			this.txtFilename.Location = new System.Drawing.Point(8, 28);
			this.txtFilename.Margin = new System.Windows.Forms.Padding(4);
			this.txtFilename.Name = "txtFilename";
			this.txtFilename.Size = new System.Drawing.Size(416, 20);
			this.txtFilename.TabIndex = 0;
			this.txtFilename.TextChanged += new System.EventHandler(this.txtFilename_TextChanged);
			// 
			// btnValidate
			// 
			this.btnValidate.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnValidate.Enabled = false;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.btnValidate, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.btnValidate, null);
			this.l10NSharpExtender1.SetLocalizingId(this.btnValidate, "ValidationDialog.btnValidate");
			this.btnValidate.Location = new System.Drawing.Point(393, 213);
			this.btnValidate.Margin = new System.Windows.Forms.Padding(4);
			this.btnValidate.Name = "btnValidate";
			this.btnValidate.Size = new System.Drawing.Size(75, 23);
			this.btnValidate.TabIndex = 1;
			this.btnValidate.Text = "&Validate";
			this.btnValidate.UseVisualStyleBackColor = true;
			this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
			// 
			// label1
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.label1, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.label1, null);
			this.l10NSharpExtender1.SetLocalizingId(this.label1, "ValidationDialog.label1");
			this.label1.Location = new System.Drawing.Point(111, 17);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(433, 43);
			this.label1.TabIndex = 2;
			this.label1.Text = resources.GetString("label1.Text");
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::epubValidator.Properties.Resources.epub_logo_color1;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.pictureBox1, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.pictureBox1, null);
			this.l10NSharpExtender1.SetLocalizingId(this.pictureBox1, "ValidationDialog.pictureBox1");
			this.pictureBox1.Location = new System.Drawing.Point(17, 16);
			this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(64, 64);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 3;
			this.pictureBox1.TabStop = false;
			// 
			// lblInstructions
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblInstructions, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblInstructions, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblInstructions, "ValidationDialog.lblInstructions");
			this.lblInstructions.Location = new System.Drawing.Point(111, 85);
			this.lblInstructions.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblInstructions.Name = "lblInstructions";
			this.lblInstructions.Size = new System.Drawing.Size(433, 29);
			this.lblInstructions.TabIndex = 4;
			this.lblInstructions.Text = "Click the Validate button below to start the process.";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.btnCancel, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.btnCancel, null);
			this.l10NSharpExtender1.SetLocalizingId(this.btnCancel, "ValidationDialog.btnCancel");
			this.btnCancel.Location = new System.Drawing.Point(476, 213);
			this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// lblStatus
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblStatus, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblStatus, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblStatus, "ValidationDialog.lblStatus");
			this.lblStatus.Location = new System.Drawing.Point(16, 218);
			this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(348, 31);
			this.lblStatus.TabIndex = 6;
			this.lblStatus.Text = "Validating file - please wait.";
			this.lblStatus.Visible = false;
			// 
			// l10NSharpExtender1
			// 
			this.l10NSharpExtender1.LocalizationManagerId = "Pathway";
			this.l10NSharpExtender1.PrefixForNewItems = "ValidationDialog";
			// 
			// ValidationDialog
			// 
			this.AcceptButton = this.btnValidate;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(559, 254);
			this.Controls.Add(this.lblStatus);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.lblInstructions);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnValidate);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.l10NSharpExtender1.SetLocalizableToolTip(this, null);
			this.l10NSharpExtender1.SetLocalizationComment(this, null);
			this.l10NSharpExtender1.SetLocalizingId(this, "ValidationDialog.WindowTitle");
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ValidationDialog";
			this.ShowIcon = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Validate .epub File";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.l10NSharpExtender1)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog ofp;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtFilename;
        private System.Windows.Forms.Button btnValidate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblStatus;
        private L10NSharp.UI.L10NSharpExtender l10NSharpExtender1;
    }
}

