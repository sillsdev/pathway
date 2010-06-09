namespace SIL.PublishingSolution
{
    partial class Template
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Template));
            this.lstSourceCSS = new System.Windows.Forms.ListBox();
            this.webTemplatePreview = new System.Windows.Forms.WebBrowser();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.lblTemplate = new System.Windows.Forms.Label();
            this.lblPreview = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstSourceCSS
            // 
            this.lstSourceCSS.AccessibleName = "lstSourceCSS";
            this.lstSourceCSS.FormattingEnabled = true;
            this.lstSourceCSS.Location = new System.Drawing.Point(14, 41);
            this.lstSourceCSS.Name = "lstSourceCSS";
            this.lstSourceCSS.Size = new System.Drawing.Size(224, 446);
            this.lstSourceCSS.TabIndex = 0;
            this.lstSourceCSS.SelectedIndexChanged += new System.EventHandler(this.lstSourceCSS_SelectedIndexChanged);
            // 
            // webTemplatePreview
            // 
            this.webTemplatePreview.AccessibleName = "webTemplatePreview";
            this.webTemplatePreview.Location = new System.Drawing.Point(253, 41);
            this.webTemplatePreview.MinimumSize = new System.Drawing.Size(23, 20);
            this.webTemplatePreview.Name = "webTemplatePreview";
            this.webTemplatePreview.Size = new System.Drawing.Size(570, 445);
            this.webTemplatePreview.TabIndex = 41;
            // 
            // btnClose
            // 
            this.btnClose.AccessibleName = "btnClose";
            this.btnClose.Location = new System.Drawing.Point(736, 499);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(87, 23);
            this.btnClose.TabIndex = 42;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.AccessibleName = "btnSelect";
            this.btnSelect.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSelect.Location = new System.Drawing.Point(643, 499);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(87, 23);
            this.btnSelect.TabIndex = 43;
            this.btnSelect.Text = "&Apply";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // lblTemplate
            // 
            this.lblTemplate.AccessibleName = "lblTemplate";
            this.lblTemplate.AutoSize = true;
            this.lblTemplate.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTemplate.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.lblTemplate.Location = new System.Drawing.Point(23, 23);
            this.lblTemplate.Name = "lblTemplate";
            this.lblTemplate.Size = new System.Drawing.Size(75, 13);
            this.lblTemplate.TabIndex = 44;
            this.lblTemplate.Text = "Templates";
            // 
            // lblPreview
            // 
            this.lblPreview.AccessibleName = "lblPreview";
            this.lblPreview.AutoSize = true;
            this.lblPreview.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPreview.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.lblPreview.Location = new System.Drawing.Point(282, 23);
            this.lblPreview.Name = "lblPreview";
            this.lblPreview.Size = new System.Drawing.Size(59, 13);
            this.lblPreview.TabIndex = 45;
            this.lblPreview.Text = "ShowPreview";
            // 
            // Template
            // 
            this.AccessibleName = "Template";
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 534);
            this.Controls.Add(this.lblPreview);
            this.Controls.Add(this.lblTemplate);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.webTemplatePreview);
            this.Controls.Add(this.lstSourceCSS);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Template";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Template";
            this.Load += new System.EventHandler(this.Template_Load);
            this.DoubleClick += new System.EventHandler(this.Template_DoubleClick);
            this.Activated += new System.EventHandler(this.Template_Activated);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstSourceCSS;
        private System.Windows.Forms.WebBrowser webTemplatePreview;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Label lblTemplate;
        private System.Windows.Forms.Label lblPreview;
    }
}
