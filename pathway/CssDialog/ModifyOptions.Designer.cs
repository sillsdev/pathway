using SIL.Tool;

namespace SIL.PublishingSolution
{
    partial class ModifyOptions
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
            this.LbFeatures = new System.Windows.Forms.Label();
            this.TvFeatures = new System.Windows.Forms.TreeView();
            this.OpenSnippet = new System.Windows.Forms.OpenFileDialog();
            this.TbCssSnippet = new System.Windows.Forms.TextBox();
            this.LbCssSnippet = new System.Windows.Forms.Label();
            this.LvIcon = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.LbFeatureName = new System.Windows.Forms.Label();
            this.TbFeatureName = new System.Windows.Forms.TextBox();
            this.TbOptionName = new System.Windows.Forms.TextBox();
            this.LbOptionName = new System.Windows.Forms.Label();
            this.BtOk = new System.Windows.Forms.Button();
            this.BtCancel = new System.Windows.Forms.Button();
            this.BtApply = new System.Windows.Forms.Button();
            this.BtUp = new System.Windows.Forms.Button();
            this.BtDown = new System.Windows.Forms.Button();
            this.BtEdit = new System.Windows.Forms.Button();
            this.openIcon = new System.Windows.Forms.OpenFileDialog();
            this.BtHelp = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.BtIcon = new System.Windows.Forms.Button();
            this.BtSnippet = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LbFeatures
            // 
            this.LbFeatures.AccessibleName = "LbFeatures";
            this.LbFeatures.AutoSize = true;
            this.LbFeatures.Location = new System.Drawing.Point(20, 22);
            this.LbFeatures.Name = "LbFeatures";
            this.LbFeatures.Size = new System.Drawing.Size(231, 13);
            this.LbFeatures.TabIndex = 7;
            this.LbFeatures.Text = "Add or edit Publication options and their values.";
            // 
            // TvFeatures
            // 
            this.TvFeatures.AccessibleName = "TvFeatures";
            this.TvFeatures.LabelEdit = true;
            this.TvFeatures.Location = new System.Drawing.Point(21, 54);
            this.TvFeatures.Name = "TvFeatures";
            this.TvFeatures.Size = new System.Drawing.Size(271, 256);
            this.TvFeatures.TabIndex = 0;
            this.TvFeatures.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TvFeatures_AfterSelect);
            // 
            // OpenSnippet
            // 
            this.OpenSnippet.FileName = "CssSnippet";
            this.OpenSnippet.Filter = "Cascading Style Sheet (*.css)|*.css";
            // 
            // TbCssSnippet
            // 
            this.TbCssSnippet.AccessibleName = "TbCssSnippet";
            this.TbCssSnippet.Location = new System.Drawing.Point(89, 42);
            this.TbCssSnippet.Name = "TbCssSnippet";
            this.TbCssSnippet.Size = new System.Drawing.Size(150, 20);
            this.TbCssSnippet.TabIndex = 3;
            // 
            // LbCssSnippet
            // 
            this.LbCssSnippet.AccessibleName = "LbCssSnippet";
            this.LbCssSnippet.Location = new System.Drawing.Point(3, 44);
            this.LbCssSnippet.Name = "LbCssSnippet";
            this.LbCssSnippet.Size = new System.Drawing.Size(81, 13);
            this.LbCssSnippet.TabIndex = 9;
            this.LbCssSnippet.Text = "Css Snippet";
            this.LbCssSnippet.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LvIcon
            // 
            this.LvIcon.AccessibleName = "LvIcon";
            this.LvIcon.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.LvIcon.HideSelection = false;
            this.LvIcon.Location = new System.Drawing.Point(25, 98);
            this.LvIcon.Name = "LvIcon";
            this.LvIcon.Size = new System.Drawing.Size(215, 112);
            this.LvIcon.TabIndex = 5;
            this.LvIcon.UseCompatibleStateImageBehavior = false;
            this.LvIcon.View = System.Windows.Forms.View.List;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 10;
            // 
            // LbFeatureName
            // 
            this.LbFeatureName.AccessibleName = "LbFeatureName";
            this.LbFeatureName.Location = new System.Drawing.Point(3, 18);
            this.LbFeatureName.Name = "LbFeatureName";
            this.LbFeatureName.Size = new System.Drawing.Size(81, 13);
            this.LbFeatureName.TabIndex = 15;
            this.LbFeatureName.Text = "Value Name";
            this.LbFeatureName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TbFeatureName
            // 
            this.TbFeatureName.AccessibleName = "TbFeatureName";
            this.TbFeatureName.Location = new System.Drawing.Point(90, 15);
            this.TbFeatureName.Name = "TbFeatureName";
            this.TbFeatureName.Size = new System.Drawing.Size(150, 20);
            this.TbFeatureName.TabIndex = 1;
            // 
            // TbOptionName
            // 
            this.TbOptionName.AccessibleName = "TbOptionName";
            this.TbOptionName.Location = new System.Drawing.Point(398, 54);
            this.TbOptionName.Name = "TbOptionName";
            this.TbOptionName.Size = new System.Drawing.Size(138, 20);
            this.TbOptionName.TabIndex = 2;
            this.TbOptionName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TbOptionName_KeyPress);
            // 
            // LbOptionName
            // 
            this.LbOptionName.AccessibleName = "LbOptionName";
            this.LbOptionName.Location = new System.Drawing.Point(298, 57);
            this.LbOptionName.Name = "LbOptionName";
            this.LbOptionName.Size = new System.Drawing.Size(94, 13);
            this.LbOptionName.TabIndex = 18;
            this.LbOptionName.Text = "Option Name";
            this.LbOptionName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // BtOk
            // 
            this.BtOk.AccessibleName = "BtOk";
            this.BtOk.Location = new System.Drawing.Point(492, 322);
            this.BtOk.Name = "BtOk";
            this.BtOk.Size = new System.Drawing.Size(75, 23);
            this.BtOk.TabIndex = 13;
            this.BtOk.Text = "&Ok";
            this.BtOk.UseVisualStyleBackColor = true;
            this.BtOk.Click += new System.EventHandler(this.BtClose_Click);
            // 
            // BtCancel
            // 
            this.BtCancel.AccessibleName = "BtCancel";
            this.BtCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtCancel.Location = new System.Drawing.Point(573, 322);
            this.BtCancel.Name = "BtCancel";
            this.BtCancel.Size = new System.Drawing.Size(75, 23);
            this.BtCancel.TabIndex = 12;
            this.BtCancel.Text = "C&ancel";
            this.BtCancel.UseVisualStyleBackColor = true;
            this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // BtApply
            // 
            this.BtApply.AccessibleName = "BtApply";
            this.BtApply.Location = new System.Drawing.Point(566, 10);
            this.BtApply.Name = "BtApply";
            this.BtApply.Size = new System.Drawing.Size(75, 23);
            this.BtApply.TabIndex = 11;
            this.BtApply.Text = "&Apply";
            this.BtApply.UseVisualStyleBackColor = true;
            this.BtApply.Visible = false;
            this.BtApply.Click += new System.EventHandler(this.BtApply_Click);
            // 
            // BtUp
            // 
            this.BtUp.AccessibleName = "BtUp";
            this.BtUp.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtUp.Location = new System.Drawing.Point(618, 4);
            this.BtUp.Name = "BtUp";
            this.BtUp.Size = new System.Drawing.Size(23, 23);
            this.BtUp.TabIndex = 8;
            this.BtUp.Text = Common.ConvertUnicodeToString("\\2191");
            this.BtUp.UseVisualStyleBackColor = true;
            this.BtUp.Visible = false;
            // 
            // BtDown
            // 
            this.BtDown.AccessibleName = "BtDown";
            this.BtDown.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtDown.Location = new System.Drawing.Point(618, 28);
            this.BtDown.Name = "BtDown";
            this.BtDown.Size = new System.Drawing.Size(23, 23);
            this.BtDown.TabIndex = 9;
            this.BtDown.Text = Common.ConvertUnicodeToString("\\2193");
            this.BtDown.UseVisualStyleBackColor = true;
            this.BtDown.Visible = false;
            // 
            // BtEdit
            // 
            this.BtEdit.AccessibleName = "BtEdit";
            this.BtEdit.Location = new System.Drawing.Point(89, 68);
            this.BtEdit.Name = "BtEdit";
            this.BtEdit.Size = new System.Drawing.Size(75, 23);
            this.BtEdit.TabIndex = 10;
            this.BtEdit.Text = "&Edit...";
            this.BtEdit.UseVisualStyleBackColor = true;
            this.BtEdit.Click += new System.EventHandler(this.BtEdit_Click);
            // 
            // openIcon
            // 
            this.openIcon.FileName = "icon";
            // 
            // BtHelp
            // 
            this.BtHelp.AccessibleName = "BtHelp";
            this.BtHelp.Location = new System.Drawing.Point(21, 322);
            this.BtHelp.Name = "BtHelp";
            this.BtHelp.Size = new System.Drawing.Size(75, 23);
            this.BtHelp.TabIndex = 19;
            this.BtHelp.Text = "&Help";
            this.BtHelp.UseVisualStyleBackColor = true;
            this.BtHelp.Click += new System.EventHandler(this.BtHelp_Click);
            // 
            // label1
            // 
            this.label1.AccessibleName = "LbFeatures";
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Options and Values";
            // 
            // label2
            // 
            this.label2.AccessibleName = "LbFeatures";
            this.label2.Location = new System.Drawing.Point(3, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Value Icon";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.BtIcon);
            this.panel1.Controls.Add(this.BtSnippet);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.BtEdit);
            this.panel1.Controls.Add(this.TbFeatureName);
            this.panel1.Controls.Add(this.LbFeatureName);
            this.panel1.Controls.Add(this.LvIcon);
            this.panel1.Controls.Add(this.LbCssSnippet);
            this.panel1.Controls.Add(this.TbCssSnippet);
            this.panel1.Location = new System.Drawing.Point(307, 83);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(341, 227);
            this.panel1.TabIndex = 22;
            // 
            // BtIcon
            // 
            this.BtIcon.AccessibleName = "BtIcon";
            this.BtIcon.Location = new System.Drawing.Point(246, 98);
            this.BtIcon.Name = "BtIcon";
            this.BtIcon.Size = new System.Drawing.Size(75, 23);
            this.BtIcon.TabIndex = 23;
            this.BtIcon.Text = "B&rowse...";
            this.BtIcon.UseVisualStyleBackColor = true;
            // 
            // BtSnippet
            // 
            this.BtSnippet.AccessibleName = "BtSnippet";
            this.BtSnippet.Location = new System.Drawing.Point(246, 40);
            this.BtSnippet.Name = "BtSnippet";
            this.BtSnippet.Size = new System.Drawing.Size(75, 23);
            this.BtSnippet.TabIndex = 22;
            this.BtSnippet.Text = "&Browse...";
            this.BtSnippet.UseVisualStyleBackColor = true;
            this.BtSnippet.Click += new System.EventHandler(this.BtSnippet_Click);
            // 
            // ModifyOptions
            // 
            this.AcceptButton = this.BtOk;
            this.AccessibleName = "ModifyOptions";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BtCancel;
            this.ClientSize = new System.Drawing.Size(666, 361);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtHelp);
            this.Controls.Add(this.BtDown);
            this.Controls.Add(this.BtUp);
            this.Controls.Add(this.BtApply);
            this.Controls.Add(this.BtCancel);
            this.Controls.Add(this.BtOk);
            this.Controls.Add(this.LbOptionName);
            this.Controls.Add(this.TbOptionName);
            this.Controls.Add(this.LbFeatures);
            this.Controls.Add(this.TvFeatures);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModifyOptions";
            this.Text = "Modify Options";
            this.Load += new System.EventHandler(this.ModifyOptions_Load);
            this.DoubleClick += new System.EventHandler(this.ModifyOptions_DoubleClick);
            this.Activated += new System.EventHandler(this.ModifyOptions_Activated);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LbFeatures;
        private System.Windows.Forms.TreeView TvFeatures;
        private System.Windows.Forms.OpenFileDialog OpenSnippet;
        private System.Windows.Forms.TextBox TbCssSnippet;
        private System.Windows.Forms.Label LbCssSnippet;
        private System.Windows.Forms.ListView LvIcon;
        private System.Windows.Forms.Label LbFeatureName;
        private System.Windows.Forms.TextBox TbFeatureName;
        private System.Windows.Forms.TextBox TbOptionName;
        private System.Windows.Forms.Label LbOptionName;
        private System.Windows.Forms.Button BtOk;
        private System.Windows.Forms.Button BtCancel;
        private System.Windows.Forms.Button BtApply;
        private System.Windows.Forms.Button BtUp;
        private System.Windows.Forms.Button BtDown;
        private System.Windows.Forms.Button BtEdit;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.OpenFileDialog openIcon;
        private System.Windows.Forms.Button BtHelp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button BtIcon;
        private System.Windows.Forms.Button BtSnippet;
    }
}
