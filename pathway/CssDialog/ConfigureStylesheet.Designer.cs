namespace SIL.PublishingSolution
{
    partial class ConfigureStylesheet
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
            this.LbStyleSheet = new System.Windows.Forms.Label();
            this.TbStyleSheet = new System.Windows.Forms.TextBox();
            this.LbDescription = new System.Windows.Forms.Label();
            this.RtDescription = new System.Windows.Forms.RichTextBox();
            this.TvFeatures = new System.Windows.Forms.TreeView();
            this.LbFeatures = new System.Windows.Forms.Label();
            this.BtSave = new System.Windows.Forms.Button();
            this.BtCancel = new System.Windows.Forms.Button();
            this.LbSummary = new System.Windows.Forms.Label();
            this.BtPreview = new System.Windows.Forms.Button();
            this.BtModifyOptions = new System.Windows.Forms.Button();
            this.BtCategorie = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.LbStyleSheetName = new System.Windows.Forms.Label();
            this.LbStyleDescription = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LbStyleSheet
            // 
            this.LbStyleSheet.AccessibleName = "LbStyleSheet";
            this.LbStyleSheet.Location = new System.Drawing.Point(3, 43);
            this.LbStyleSheet.Name = "LbStyleSheet";
            this.LbStyleSheet.Size = new System.Drawing.Size(106, 15);
            this.LbStyleSheet.TabIndex = 0;
            this.LbStyleSheet.Text = "Stylesheet Name";
            this.LbStyleSheet.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TbStyleSheet
            // 
            this.TbStyleSheet.AccessibleName = "TbStyleSheet";
            this.TbStyleSheet.Enabled = false;
            this.TbStyleSheet.Location = new System.Drawing.Point(406, 3);
            this.TbStyleSheet.Name = "TbStyleSheet";
            this.TbStyleSheet.Size = new System.Drawing.Size(45, 20);
            this.TbStyleSheet.TabIndex = 1;
            this.TbStyleSheet.Visible = false;
            // 
            // LbDescription
            // 
            this.LbDescription.AccessibleName = "LbDescription";
            this.LbDescription.Location = new System.Drawing.Point(6, 65);
            this.LbDescription.Name = "LbDescription";
            this.LbDescription.Size = new System.Drawing.Size(103, 58);
            this.LbDescription.TabIndex = 2;
            this.LbDescription.Text = "Description";
            this.LbDescription.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // RtDescription
            // 
            this.RtDescription.AccessibleName = "RtDescription";
            this.RtDescription.Enabled = false;
            this.RtDescription.Location = new System.Drawing.Point(333, 7);
            this.RtDescription.Name = "RtDescription";
            this.RtDescription.Size = new System.Drawing.Size(43, 16);
            this.RtDescription.TabIndex = 3;
            this.RtDescription.Text = "";
            this.RtDescription.Visible = false;
            // 
            // TvFeatures
            // 
            this.TvFeatures.AccessibleName = "TvFeatures";
            this.TvFeatures.CheckBoxes = true;
            this.TvFeatures.LabelEdit = true;
            this.TvFeatures.Location = new System.Drawing.Point(22, 177);
            this.TvFeatures.Name = "TvFeatures";
            this.TvFeatures.Size = new System.Drawing.Size(435, 228);
            this.TvFeatures.TabIndex = 4;
            this.TvFeatures.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.TvFeatures_AfterCheck);
            this.TvFeatures.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TvFeatures_AfterSelect);
            // 
            // LbFeatures
            // 
            this.LbFeatures.AccessibleName = "LbFeatures";
            this.LbFeatures.AutoSize = true;
            this.LbFeatures.Location = new System.Drawing.Point(22, 161);
            this.LbFeatures.Name = "LbFeatures";
            this.LbFeatures.Size = new System.Drawing.Size(98, 13);
            this.LbFeatures.TabIndex = 5;
            this.LbFeatures.Text = "Publication Options";
            // 
            // BtSave
            // 
            this.BtSave.AccessibleName = "BtSave";
            this.BtSave.Location = new System.Drawing.Point(301, 446);
            this.BtSave.Name = "BtSave";
            this.BtSave.Size = new System.Drawing.Size(75, 23);
            this.BtSave.TabIndex = 6;
            this.BtSave.Text = "&Save";
            this.BtSave.UseVisualStyleBackColor = true;
            this.BtSave.Click += new System.EventHandler(this.BtSave_Click);
            // 
            // BtCancel
            // 
            this.BtCancel.AccessibleName = "BtCancel";
            this.BtCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtCancel.Location = new System.Drawing.Point(382, 446);
            this.BtCancel.Name = "BtCancel";
            this.BtCancel.Size = new System.Drawing.Size(75, 23);
            this.BtCancel.TabIndex = 7;
            this.BtCancel.Text = "&Cancel";
            this.BtCancel.UseVisualStyleBackColor = true;
            this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // LbSummary
            // 
            this.LbSummary.AccessibleName = "LbSummary";
            this.LbSummary.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LbSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbSummary.Location = new System.Drawing.Point(115, 131);
            this.LbSummary.Name = "LbSummary";
            this.LbSummary.Size = new System.Drawing.Size(305, 23);
            this.LbSummary.TabIndex = 8;
            this.LbSummary.Text = "Summary";
            this.LbSummary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BtPreview
            // 
            this.BtPreview.AccessibleName = "BtPreview";
            this.BtPreview.Location = new System.Drawing.Point(382, 419);
            this.BtPreview.Name = "BtPreview";
            this.BtPreview.Size = new System.Drawing.Size(75, 23);
            this.BtPreview.TabIndex = 9;
            this.BtPreview.Text = "&Preview...";
            this.BtPreview.UseVisualStyleBackColor = true;
            this.BtPreview.Click += new System.EventHandler(this.BtPreview_Click);
            // 
            // BtModifyOptions
            // 
            this.BtModifyOptions.AccessibleName = "BtModifyOptions";
            this.BtModifyOptions.Location = new System.Drawing.Point(23, 420);
            this.BtModifyOptions.Name = "BtModifyOptions";
            this.BtModifyOptions.Size = new System.Drawing.Size(107, 23);
            this.BtModifyOptions.TabIndex = 10;
            this.BtModifyOptions.Text = "&Modify Options...";
            this.BtModifyOptions.UseVisualStyleBackColor = true;
            this.BtModifyOptions.Click += new System.EventHandler(this.BtEdit_Click);
            // 
            // BtCategorie
            // 
            this.BtCategorie.AccessibleName = "BtCategorie";
            this.BtCategorie.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.BtCategorie.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.BtCategorie.Location = new System.Drawing.Point(423, 130);
            this.BtCategorie.Name = "BtCategorie";
            this.BtCategorie.Size = new System.Drawing.Size(28, 25);
            this.BtCategorie.TabIndex = 12;
            this.BtCategorie.Text = "...";
            this.BtCategorie.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.BtCategorie.UseVisualStyleBackColor = true;
            this.BtCategorie.Click += new System.EventHandler(this.BtCategories_Click);
            // 
            // label1
            // 
            this.label1.AccessibleName = "LbSummary";
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(22, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(242, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Modify the stylesheet publication options.";
            // 
            // label2
            // 
            this.label2.AccessibleName = "LbDescription";
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(49, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Categories";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LbStyleSheetName
            // 
            this.LbStyleSheetName.AutoSize = true;
            this.LbStyleSheetName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LbStyleSheetName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbStyleSheetName.Location = new System.Drawing.Point(115, 43);
            this.LbStyleSheetName.Name = "LbStyleSheetName";
            this.LbStyleSheetName.Size = new System.Drawing.Size(68, 15);
            this.LbStyleSheetName.TabIndex = 15;
            this.LbStyleSheetName.Text = "Stylesheet";
            // 
            // LbStyleDescription
            // 
            this.LbStyleDescription.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LbStyleDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbStyleDescription.Location = new System.Drawing.Point(115, 65);
            this.LbStyleDescription.Name = "LbStyleDescription";
            this.LbStyleDescription.Size = new System.Drawing.Size(305, 58);
            this.LbStyleDescription.TabIndex = 16;
            this.LbStyleDescription.Text = "Description";
            // 
            // ConfigureStylesheet
            // 
            this.AcceptButton = this.BtSave;
            this.AccessibleName = "ConfigureStylesheet";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BtCancel;
            this.ClientSize = new System.Drawing.Size(478, 489);
            this.Controls.Add(this.LbStyleDescription);
            this.Controls.Add(this.LbStyleSheetName);
            this.Controls.Add(this.LbSummary);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtCategorie);
            this.Controls.Add(this.BtModifyOptions);
            this.Controls.Add(this.BtPreview);
            this.Controls.Add(this.BtCancel);
            this.Controls.Add(this.BtSave);
            this.Controls.Add(this.LbFeatures);
            this.Controls.Add(this.TvFeatures);
            this.Controls.Add(this.RtDescription);
            this.Controls.Add(this.LbDescription);
            this.Controls.Add(this.TbStyleSheet);
            this.Controls.Add(this.LbStyleSheet);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigureStylesheet";
            this.Text = "Configure Stylesheet";
            this.Load += new System.EventHandler(this.ConfigureStylesheet_Load);
            this.DoubleClick += new System.EventHandler(this.ConfigureStylesheet_DoubleClick);
            this.Activated += new System.EventHandler(this.ConfigureStylesheet_Activated);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LbStyleSheet;
        private System.Windows.Forms.TextBox TbStyleSheet;
        private System.Windows.Forms.Label LbDescription;
        private System.Windows.Forms.RichTextBox RtDescription;
        private System.Windows.Forms.TreeView TvFeatures;
        private System.Windows.Forms.Label LbFeatures;
        private System.Windows.Forms.Button BtSave;
        private System.Windows.Forms.Button BtCancel;
        private System.Windows.Forms.Label LbSummary;
        private System.Windows.Forms.Button BtPreview;
        private System.Windows.Forms.Button BtModifyOptions;
        private System.Windows.Forms.Button BtCategorie;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label LbStyleSheetName;
        private System.Windows.Forms.Label LbStyleDescription;
    }
}
