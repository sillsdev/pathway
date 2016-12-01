namespace SIL.PublishingSolution
{
    partial class SelectOrganizationDialog
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
			this.l10NSharpExtender1 = new L10NSharp.UI.L10NSharpExtender(this.components);
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOther = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.lblOrganization = new System.Windows.Forms.Label();
			this.lblOther = new System.Windows.Forms.Label();
			this.lblInstructions = new System.Windows.Forms.Label();
			this.btnHelp = new System.Windows.Forms.Button();
			this.ddlOrganization = new System.Windows.Forms.ComboBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			((System.ComponentModel.ISupportInitialize)(this.l10NSharpExtender1)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// l10NSharpExtender1
			// 
			this.l10NSharpExtender1.LocalizationManagerId = "Pathway";
			this.l10NSharpExtender1.PrefixForNewItems = "SelectOrganizationDialog";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.btnCancel, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.btnCancel, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.btnCancel, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.btnCancel, "SelectOrganizationDialog.btnCancel");
			this.btnCancel.Location = new System.Drawing.Point(199, 145);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 25);
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOther
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.btnOther, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.btnOther, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.btnOther, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.btnOther, "SelectOrganizationDialog.btnOther");
			this.btnOther.Location = new System.Drawing.Point(280, 96);
			this.btnOther.Name = "btnOther";
			this.btnOther.Size = new System.Drawing.Size(73, 23);
			this.btnOther.TabIndex = 4;
			this.btnOther.Text = "O&ther...";
			this.btnOther.UseVisualStyleBackColor = true;
			this.btnOther.Click += new System.EventHandler(this.btnOther_Click);
			// 
			// btnOK
			// 
			this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.btnOK, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.btnOK, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.btnOK, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.btnOK, "SelectOrganizationDialog.btnOK");
			this.btnOK.Location = new System.Drawing.Point(124, 145);
			this.btnOK.Name = "btnOK";
			this.btnOK.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.btnOK.Size = new System.Drawing.Size(69, 25);
			this.btnOK.TabIndex = 5;
			this.btnOK.Text = "&OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// lblOrganization
			// 
			this.lblOrganization.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblOrganization.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblOrganization, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblOrganization, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.lblOrganization, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.lblOrganization, "SelectOrganizationDialog.lblOrganization");
			this.lblOrganization.Location = new System.Drawing.Point(8, 63);
			this.lblOrganization.Name = "lblOrganization";
			this.lblOrganization.Size = new System.Drawing.Size(110, 30);
			this.lblOrganization.TabIndex = 1;
			this.lblOrganization.Text = "My o&rganization:";
			this.lblOrganization.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblOther
			// 
			this.lblOther.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.SetColumnSpan(this.lblOther, 3);
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblOther, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblOther, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.lblOther, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.lblOther, "SelectOrganizationDialog.lblOther");
			this.lblOther.Location = new System.Drawing.Point(8, 93);
			this.lblOther.Name = "lblOther";
			this.lblOther.Size = new System.Drawing.Size(266, 43);
			this.lblOther.TabIndex = 3;
			this.lblOther.Text = "If your organization is not on the above list, click the Other... button:";
			this.lblOther.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblInstructions
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.lblInstructions, 4);
			this.lblInstructions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblInstructions, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblInstructions, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.lblInstructions, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.lblInstructions, "SelectOrganizationDialog.lblInstructions");
			this.lblInstructions.Location = new System.Drawing.Point(8, 5);
			this.lblInstructions.Name = "lblInstructions";
			this.lblInstructions.Size = new System.Drawing.Size(351, 58);
			this.lblInstructions.TabIndex = 11;
			this.lblInstructions.Text = "Select the organization you are representing, Pathway will automatically fill in " +
    "copyright statements, etc. based on your selection.";
			this.lblInstructions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnHelp
			// 
			this.btnHelp.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.btnHelp, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.btnHelp, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.btnHelp, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.btnHelp, "SelectOrganizationDialog.btnHelp");
			this.btnHelp.Location = new System.Drawing.Point(282, 145);
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.Size = new System.Drawing.Size(75, 25);
			this.btnHelp.TabIndex = 7;
			this.btnHelp.Text = "&Help";
			this.btnHelp.UseVisualStyleBackColor = true;
			this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
			// 
			// ddlOrganization
			// 
			this.ddlOrganization.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.tableLayoutPanel1.SetColumnSpan(this.ddlOrganization, 3);
			this.ddlOrganization.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlOrganization.FormattingEnabled = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.ddlOrganization, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.ddlOrganization, null);
			this.l10NSharpExtender1.SetLocalizingId(this.ddlOrganization, "SelectOrganizationDialog.SelectOrganizationDialog.ddlOrganization");
			this.ddlOrganization.Location = new System.Drawing.Point(124, 66);
			this.ddlOrganization.Name = "ddlOrganization";
			this.ddlOrganization.Size = new System.Drawing.Size(232, 21);
			this.ddlOrganization.TabIndex = 2;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.ColumnCount = 4;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.57692F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 39.42308F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 81F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 84F));
			this.tableLayoutPanel1.Controls.Add(this.btnOK, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblInstructions, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.btnOther, 3, 2);
			this.tableLayoutPanel1.Controls.Add(this.ddlOrganization, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.btnHelp, 3, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblOther, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.btnCancel, 2, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblOrganization, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.MaximumSize = new System.Drawing.Size(367, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(5);
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(367, 185);
			this.tableLayoutPanel1.TabIndex = 12;
			// 
			// SelectOrganizationDialog
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(368, 185);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.l10NSharpExtender1.SetLocalizableToolTip(this, null);
			this.l10NSharpExtender1.SetLocalizationComment(this, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this, "SelectOrganizationDialog.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SelectOrganizationDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select Your Organization";
			this.Load += new System.EventHandler(this.SelectOrganizationDialog_Load);
			((System.ComponentModel.ISupportInitialize)(this.l10NSharpExtender1)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private L10NSharp.UI.L10NSharpExtender l10NSharpExtender1 = null;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOther;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblOrganization;
        private System.Windows.Forms.ComboBox ddlOrganization;
        private System.Windows.Forms.Label lblOther;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.Button btnHelp;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}