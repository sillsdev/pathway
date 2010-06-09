namespace SIL.PublishingSolution
{
    partial class Categories
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
            this.LbCategories = new System.Windows.Forms.Label();
            this.TvCategories = new System.Windows.Forms.TreeView();
            this.BtSave = new System.Windows.Forms.Button();
            this.BtCancel = new System.Windows.Forms.Button();
            this.BtSettings = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LbCategories
            // 
            this.LbCategories.AccessibleName = "LbCategories";
            this.LbCategories.AutoSize = true;
            this.LbCategories.Location = new System.Drawing.Point(12, 9);
            this.LbCategories.Name = "LbCategories";
            this.LbCategories.Size = new System.Drawing.Size(57, 13);
            this.LbCategories.TabIndex = 9;
            this.LbCategories.Text = "Categories";
            this.LbCategories.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TvCategories
            // 
            this.TvCategories.AccessibleName = "TvCategories";
            this.TvCategories.CheckBoxes = true;
            this.TvCategories.LabelEdit = true;
            this.TvCategories.Location = new System.Drawing.Point(15, 26);
            this.TvCategories.Name = "TvCategories";
            this.TvCategories.Size = new System.Drawing.Size(239, 413);
            this.TvCategories.TabIndex = 10;
            this.TvCategories.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.TvCategories_AfterCheck);
            // 
            // BtSave
            // 
            this.BtSave.AccessibleName = "BtSave";
            this.BtSave.Location = new System.Drawing.Point(96, 445);
            this.BtSave.Name = "BtSave";
            this.BtSave.Size = new System.Drawing.Size(75, 23);
            this.BtSave.TabIndex = 11;
            this.BtSave.Text = "&Save";
            this.BtSave.UseVisualStyleBackColor = true;
            this.BtSave.Click += new System.EventHandler(this.BtSave_Click);
            // 
            // BtCancel
            // 
            this.BtCancel.AccessibleName = "BtCancel";
            this.BtCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtCancel.Location = new System.Drawing.Point(177, 445);
            this.BtCancel.Name = "BtCancel";
            this.BtCancel.Size = new System.Drawing.Size(75, 23);
            this.BtCancel.TabIndex = 12;
            this.BtCancel.Text = "&Cancel";
            this.BtCancel.UseVisualStyleBackColor = true;
            this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // BtSettings
            // 
            this.BtSettings.AccessibleName = "BtSettings";
            this.BtSettings.Location = new System.Drawing.Point(15, 445);
            this.BtSettings.Name = "BtSettings";
            this.BtSettings.Size = new System.Drawing.Size(75, 23);
            this.BtSettings.TabIndex = 13;
            this.BtSettings.Text = "S&ettings";
            this.BtSettings.UseVisualStyleBackColor = true;
            this.BtSettings.Click += new System.EventHandler(this.BtSettings_Click);
            // 
            // Categories
            // 
            this.AccessibleName = "Categories";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(269, 481);
            this.Controls.Add(this.BtSettings);
            this.Controls.Add(this.BtCancel);
            this.Controls.Add(this.BtSave);
            this.Controls.Add(this.TvCategories);
            this.Controls.Add(this.LbCategories);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Categories";
            this.Text = "Categories";
            this.Load += new System.EventHandler(this.Categories_Load);
            this.DoubleClick += new System.EventHandler(this.Categories_DoubleClick);
            this.Activated += new System.EventHandler(this.Categories_Activated);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LbCategories;
        private System.Windows.Forms.TreeView TvCategories;
        private System.Windows.Forms.Button BtSave;
        private System.Windows.Forms.Button BtCancel;
        private System.Windows.Forms.Button BtSettings;
    }
}
