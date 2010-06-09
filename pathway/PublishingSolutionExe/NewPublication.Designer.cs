namespace SIL.PublishingSolution
{
    partial class NewPublication
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewPublication));
            this.btnTemplate = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.txtDicName = new System.Windows.Forms.TextBox();
            this.lblXHTML = new System.Windows.Forms.Label();
            this.txtXHTML = new System.Windows.Forms.TextBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnXHTMLBrowse = new System.Windows.Forms.Button();
            this.btnLocationBrowse = new System.Windows.Forms.Button();
            this.lblTemplate = new System.Windows.Forms.Label();
            this.lblPreview = new System.Windows.Forms.Label();
            this.webTemplatePreview = new System.Windows.Forms.WebBrowser();
            this.lstSourceCSS = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnTemplate
            // 
            this.btnTemplate.AccessibleName = "btnTemplate";
            this.btnTemplate.Location = new System.Drawing.Point(576, 46);
            this.btnTemplate.Name = "btnTemplate";
            this.btnTemplate.Size = new System.Drawing.Size(106, 23);
            this.btnTemplate.TabIndex = 4;
            this.btnTemplate.Text = "Show &Template";
            this.btnTemplate.UseVisualStyleBackColor = true;
            this.btnTemplate.Visible = false;
            // 
            // lblName
            // 
            this.lblName.AccessibleName = "lblName";
            this.lblName.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblName.Location = new System.Drawing.Point(17, 51);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(134, 13);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Publication Name";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDicName
            // 
            this.txtDicName.AccessibleName = "txtDicName";
            this.txtDicName.Location = new System.Drawing.Point(157, 48);
            this.txtDicName.Name = "txtDicName";
            this.txtDicName.Size = new System.Drawing.Size(277, 21);
            this.txtDicName.TabIndex = 3;
            this.txtDicName.TextChanged += new System.EventHandler(this.txtDicName_TextChanged);
            this.txtDicName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDicName_KeyPress);
            // 
            // lblXHTML
            // 
            this.lblXHTML.AccessibleName = "lblXHTML";
            this.lblXHTML.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblXHTML.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblXHTML.Location = new System.Drawing.Point(1, 24);
            this.lblXHTML.Name = "lblXHTML";
            this.lblXHTML.Size = new System.Drawing.Size(150, 13);
            this.lblXHTML.TabIndex = 3;
            this.lblXHTML.Text = "Select XHTML/Lift File";
            this.lblXHTML.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtXHTML
            // 
            this.txtXHTML.AccessibleName = "txtXHTML";
            this.txtXHTML.Location = new System.Drawing.Point(157, 21);
            this.txtXHTML.Name = "txtXHTML";
            this.txtXHTML.Size = new System.Drawing.Size(442, 21);
            this.txtXHTML.TabIndex = 2;
            this.txtXHTML.Validated += new System.EventHandler(this.txtXHTML_Validated);
            // 
            // lblLocation
            // 
            this.lblLocation.AccessibleName = "lblLocation";
            this.lblLocation.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLocation.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblLocation.Location = new System.Drawing.Point(-3, 78);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(154, 13);
            this.lblLocation.TabIndex = 5;
            this.lblLocation.Text = "Publication Location";
            this.lblLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtLocation
            // 
            this.txtLocation.AccessibleName = "txtLocation";
            this.txtLocation.Location = new System.Drawing.Point(157, 75);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(442, 21);
            this.txtLocation.TabIndex = 5;
            this.txtLocation.TextChanged += new System.EventHandler(this.txtLocation_TextChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleName = "btnCancel";
            this.btnCancel.Location = new System.Drawing.Point(607, 468);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.AccessibleName = "btnOk";
            this.btnOk.Location = new System.Drawing.Point(526, 468);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnXHTMLBrowse
            // 
            this.btnXHTMLBrowse.AccessibleName = "btnXHTMLBrowse";
            this.btnXHTMLBrowse.Location = new System.Drawing.Point(605, 19);
            this.btnXHTMLBrowse.Name = "btnXHTMLBrowse";
            this.btnXHTMLBrowse.Size = new System.Drawing.Size(76, 23);
            this.btnXHTMLBrowse.TabIndex = 1;
            this.btnXHTMLBrowse.Text = "&Browse...";
            this.btnXHTMLBrowse.UseVisualStyleBackColor = true;
            this.btnXHTMLBrowse.Click += new System.EventHandler(this.btnXHTMLBrowse_Click);
            // 
            // btnLocationBrowse
            // 
            this.btnLocationBrowse.AccessibleName = "btnLocationBrowse";
            this.btnLocationBrowse.Location = new System.Drawing.Point(605, 73);
            this.btnLocationBrowse.Name = "btnLocationBrowse";
            this.btnLocationBrowse.Size = new System.Drawing.Size(76, 23);
            this.btnLocationBrowse.TabIndex = 6;
            this.btnLocationBrowse.Text = "Brows&e...";
            this.btnLocationBrowse.UseVisualStyleBackColor = true;
            this.btnLocationBrowse.Click += new System.EventHandler(this.btnLocationBrowse_Click);
            // 
            // lblTemplate
            // 
            this.lblTemplate.AccessibleName = "lblTemplate";
            this.lblTemplate.AutoSize = true;
            this.lblTemplate.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTemplate.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblTemplate.Location = new System.Drawing.Point(12, 113);
            this.lblTemplate.Name = "lblTemplate";
            this.lblTemplate.Size = new System.Drawing.Size(68, 13);
            this.lblTemplate.TabIndex = 11;
            this.lblTemplate.Text = "Template";
            // 
            // lblPreview
            // 
            this.lblPreview.AccessibleName = "lblPreview";
            this.lblPreview.AutoSize = true;
            this.lblPreview.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPreview.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblPreview.Location = new System.Drawing.Point(259, 113);
            this.lblPreview.Name = "lblPreview";
            this.lblPreview.Size = new System.Drawing.Size(59, 13);
            this.lblPreview.TabIndex = 12;
            this.lblPreview.Text = "ShowPreview";
            // 
            // webTemplatePreview
            // 
            this.webTemplatePreview.AccessibleName = "webTemplatePreview";
            this.webTemplatePreview.Location = new System.Drawing.Point(262, 129);
            this.webTemplatePreview.MinimumSize = new System.Drawing.Size(20, 20);
            this.webTemplatePreview.Name = "webTemplatePreview";
            this.webTemplatePreview.Size = new System.Drawing.Size(420, 329);
            this.webTemplatePreview.TabIndex = 8;
            // 
            // lstSourceCSS
            // 
            this.lstSourceCSS.AccessibleName = "lstSourceCSS";
            this.lstSourceCSS.FormattingEnabled = true;
            this.lstSourceCSS.Location = new System.Drawing.Point(12, 129);
            this.lstSourceCSS.Name = "lstSourceCSS";
            this.lstSourceCSS.Size = new System.Drawing.Size(244, 316);
            this.lstSourceCSS.TabIndex = 7;
            this.lstSourceCSS.SelectedIndexChanged += new System.EventHandler(this.lstSourceCSS_SelectedIndexChanged);
            // 
            // NewPublication
            // 
            this.AccessibleName = "NewPublication";
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 503);
            this.Controls.Add(this.lstSourceCSS);
            this.Controls.Add(this.webTemplatePreview);
            this.Controls.Add(this.lblPreview);
            this.Controls.Add(this.lblTemplate);
            this.Controls.Add(this.btnLocationBrowse);
            this.Controls.Add(this.btnXHTMLBrowse);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtLocation);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.txtXHTML);
            this.Controls.Add(this.lblXHTML);
            this.Controls.Add(this.txtDicName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.btnTemplate);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewPublication";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Publication";
            this.Load += new System.EventHandler(this.NewPublication_Load);
            this.DoubleClick += new System.EventHandler(this.NewPublication_DoubleClick);
            this.Activated += new System.EventHandler(this.NewProject_Activated);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTemplate;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtDicName;
        private System.Windows.Forms.Label lblXHTML;
        private System.Windows.Forms.TextBox txtXHTML;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.TextBox txtLocation;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnXHTMLBrowse;
        private System.Windows.Forms.Button btnLocationBrowse;
        private System.Windows.Forms.Label lblTemplate;
        private System.Windows.Forms.Label lblPreview;
        private System.Windows.Forms.WebBrowser webTemplatePreview;
        private System.Windows.Forms.ListBox lstSourceCSS;
    }
}
