namespace SIL.PublishingSolution
{
    partial class ConfigureTasks
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
            this.LsTasks = new System.Windows.Forms.ListBox();
            this.LbTask = new System.Windows.Forms.Label();
            this.LsStyles = new System.Windows.Forms.ListBox();
            this.LbSummary = new System.Windows.Forms.Label();
            this.LbStyles = new System.Windows.Forms.Label();
            this.BtConfigure = new System.Windows.Forms.Button();
            this.BtPreview = new System.Windows.Forms.Button();
            this.BtCancel = new System.Windows.Forms.Button();
            this.BtSave = new System.Windows.Forms.Button();
            this.BtFilter = new System.Windows.Forms.Button();
            this.BtApply = new System.Windows.Forms.Button();
            this.RtDescription = new System.Windows.Forms.RichTextBox();
            this.LbDescription = new System.Windows.Forms.Label();
            this.BtAdvanced = new System.Windows.Forms.Button();
            this.BtTaskAdd = new System.Windows.Forms.Button();
            this.BtTaskModify = new System.Windows.Forms.Button();
            this.BtTaskDelete = new System.Windows.Forms.Button();
            this.BtStylesheetAdd = new System.Windows.Forms.Button();
            this.BtStylesheetModify = new System.Windows.Forms.Button();
            this.BtStylesheetDelete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LsTasks
            // 
            this.LsTasks.AccessibleName = "LsTasks";
            this.LsTasks.FormattingEnabled = true;
            this.LsTasks.Location = new System.Drawing.Point(19, 58);
            this.LsTasks.Name = "LsTasks";
            this.LsTasks.Size = new System.Drawing.Size(191, 212);
            this.LsTasks.Sorted = true;
            this.LsTasks.TabIndex = 0;
            this.LsTasks.SelectedIndexChanged += new System.EventHandler(this.LsTasks_SelectedIndexChanged);
            // 
            // LbTask
            // 
            this.LbTask.AccessibleName = "LbTask";
            this.LbTask.AutoSize = true;
            this.LbTask.Location = new System.Drawing.Point(20, 42);
            this.LbTask.Name = "LbTask";
            this.LbTask.Size = new System.Drawing.Size(31, 13);
            this.LbTask.TabIndex = 1;
            this.LbTask.Text = "Task";
            // 
            // LsStyles
            // 
            this.LsStyles.AccessibleName = "LsStyles";
            this.LsStyles.FormattingEnabled = true;
            this.LsStyles.Location = new System.Drawing.Point(316, 58);
            this.LsStyles.Name = "LsStyles";
            this.LsStyles.Size = new System.Drawing.Size(191, 212);
            this.LsStyles.Sorted = true;
            this.LsStyles.TabIndex = 3;
            this.LsStyles.SelectedIndexChanged += new System.EventHandler(this.LsStyles_SelectedIndexChanged);
            // 
            // LbSummary
            // 
            this.LbSummary.AccessibleName = "LbSummary";
            this.LbSummary.AutoSize = true;
            this.LbSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbSummary.Location = new System.Drawing.Point(20, 17);
            this.LbSummary.Name = "LbSummary";
            this.LbSummary.Size = new System.Drawing.Size(88, 13);
            this.LbSummary.TabIndex = 9;
            this.LbSummary.Text = "SummaryLabel";
            // 
            // LbStyles
            // 
            this.LbStyles.AccessibleName = "LbStyles";
            this.LbStyles.AutoSize = true;
            this.LbStyles.Location = new System.Drawing.Point(313, 42);
            this.LbStyles.Name = "LbStyles";
            this.LbStyles.Size = new System.Drawing.Size(56, 13);
            this.LbStyles.TabIndex = 12;
            this.LbStyles.Text = "Stylesheet";
            // 
            // BtConfigure
            // 
            this.BtConfigure.AccessibleName = "BtConfigure";
            this.BtConfigure.Location = new System.Drawing.Point(16, 312);
            this.BtConfigure.Name = "BtConfigure";
            this.BtConfigure.Size = new System.Drawing.Size(82, 23);
            this.BtConfigure.TabIndex = 16;
            this.BtConfigure.Text = "Co&nfigure...";
            this.BtConfigure.UseVisualStyleBackColor = true;
            this.BtConfigure.Click += new System.EventHandler(this.BtFeatures_Click);
            // 
            // BtPreview
            // 
            this.BtPreview.AccessibleName = "BtPreview";
            this.BtPreview.Location = new System.Drawing.Point(189, 312);
            this.BtPreview.Name = "BtPreview";
            this.BtPreview.Size = new System.Drawing.Size(82, 23);
            this.BtPreview.TabIndex = 15;
            this.BtPreview.Text = "&Preview...";
            this.BtPreview.UseVisualStyleBackColor = true;
            this.BtPreview.Click += new System.EventHandler(this.BtPreview_Click);
            // 
            // BtCancel
            // 
            this.BtCancel.AccessibleName = "BtCancel";
            this.BtCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtCancel.Location = new System.Drawing.Point(189, 343);
            this.BtCancel.Name = "BtCancel";
            this.BtCancel.Size = new System.Drawing.Size(82, 23);
            this.BtCancel.TabIndex = 14;
            this.BtCancel.Text = "&Cancel";
            this.BtCancel.UseVisualStyleBackColor = true;
            this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // BtSave
            // 
            this.BtSave.AccessibleName = "BtSave";
            this.BtSave.Location = new System.Drawing.Point(101, 343);
            this.BtSave.Name = "BtSave";
            this.BtSave.Size = new System.Drawing.Size(82, 23);
            this.BtSave.TabIndex = 13;
            this.BtSave.Text = "&Save";
            this.BtSave.UseVisualStyleBackColor = true;
            this.BtSave.Click += new System.EventHandler(this.BtSave_Click);
            // 
            // BtFilter
            // 
            this.BtFilter.AccessibleName = "BtFilter";
            this.BtFilter.Location = new System.Drawing.Point(513, 58);
            this.BtFilter.Name = "BtFilter";
            this.BtFilter.Size = new System.Drawing.Size(75, 23);
            this.BtFilter.TabIndex = 17;
            this.BtFilter.Text = "&Filter...";
            this.BtFilter.UseVisualStyleBackColor = true;
            this.BtFilter.Click += new System.EventHandler(this.BtCategories_Click);
            // 
            // BtApply
            // 
            this.BtApply.AccessibleName = "BtApply";
            this.BtApply.Location = new System.Drawing.Point(16, 343);
            this.BtApply.Name = "BtApply";
            this.BtApply.Size = new System.Drawing.Size(82, 23);
            this.BtApply.TabIndex = 18;
            this.BtApply.Text = "&Apply";
            this.BtApply.UseVisualStyleBackColor = true;
            this.BtApply.Click += new System.EventHandler(this.BtApply_Click);
            // 
            // RtDescription
            // 
            this.RtDescription.AccessibleName = "RtDescription";
            this.RtDescription.Location = new System.Drawing.Point(316, 303);
            this.RtDescription.Name = "RtDescription";
            this.RtDescription.ReadOnly = true;
            this.RtDescription.Size = new System.Drawing.Size(288, 65);
            this.RtDescription.TabIndex = 20;
            this.RtDescription.Text = "";
            // 
            // LbDescription
            // 
            this.LbDescription.AccessibleName = "LbDescription";
            this.LbDescription.AutoSize = true;
            this.LbDescription.Location = new System.Drawing.Point(313, 287);
            this.LbDescription.Name = "LbDescription";
            this.LbDescription.Size = new System.Drawing.Size(60, 13);
            this.LbDescription.TabIndex = 19;
            this.LbDescription.Text = "Description";
            // 
            // BtAdvanced
            // 
            this.BtAdvanced.AccessibleName = "BtAdvanced";
            this.BtAdvanced.Location = new System.Drawing.Point(101, 312);
            this.BtAdvanced.Name = "BtAdvanced";
            this.BtAdvanced.Size = new System.Drawing.Size(82, 23);
            this.BtAdvanced.TabIndex = 21;
            this.BtAdvanced.Text = "&Advanced...";
            this.BtAdvanced.UseVisualStyleBackColor = true;
            this.BtAdvanced.Click += new System.EventHandler(this.BtAdvanced_Click);
            // 
            // BtTaskAdd
            // 
            this.BtTaskAdd.Location = new System.Drawing.Point(216, 58);
            this.BtTaskAdd.Name = "BtTaskAdd";
            this.BtTaskAdd.Size = new System.Drawing.Size(75, 23);
            this.BtTaskAdd.TabIndex = 22;
            this.BtTaskAdd.Text = "Add";
            this.BtTaskAdd.UseVisualStyleBackColor = true;
            this.BtTaskAdd.Click += new System.EventHandler(this.BtTaskAdd_Click);
            // 
            // BtTaskModify
            // 
            this.BtTaskModify.Location = new System.Drawing.Point(216, 87);
            this.BtTaskModify.Name = "BtTaskModify";
            this.BtTaskModify.Size = new System.Drawing.Size(75, 23);
            this.BtTaskModify.TabIndex = 23;
            this.BtTaskModify.Text = "Modify";
            this.BtTaskModify.UseVisualStyleBackColor = true;
            this.BtTaskModify.Click += new System.EventHandler(this.BtTaskModify_Click);
            // 
            // BtTaskDelete
            // 
            this.BtTaskDelete.Location = new System.Drawing.Point(216, 116);
            this.BtTaskDelete.Name = "BtTaskDelete";
            this.BtTaskDelete.Size = new System.Drawing.Size(75, 23);
            this.BtTaskDelete.TabIndex = 24;
            this.BtTaskDelete.Text = "Delete";
            this.BtTaskDelete.UseVisualStyleBackColor = true;
            this.BtTaskDelete.Click += new System.EventHandler(this.BtTaskDelete_Click);
            // 
            // BtStylesheetAdd
            // 
            this.BtStylesheetAdd.Location = new System.Drawing.Point(513, 87);
            this.BtStylesheetAdd.Name = "BtStylesheetAdd";
            this.BtStylesheetAdd.Size = new System.Drawing.Size(75, 23);
            this.BtStylesheetAdd.TabIndex = 25;
            this.BtStylesheetAdd.Text = "Add";
            this.BtStylesheetAdd.UseVisualStyleBackColor = true;
            this.BtStylesheetAdd.Click += new System.EventHandler(this.BtStylesheetAdd_Click);
            // 
            // BtStylesheetModify
            // 
            this.BtStylesheetModify.Location = new System.Drawing.Point(513, 116);
            this.BtStylesheetModify.Name = "BtStylesheetModify";
            this.BtStylesheetModify.Size = new System.Drawing.Size(75, 23);
            this.BtStylesheetModify.TabIndex = 26;
            this.BtStylesheetModify.Text = "Modify";
            this.BtStylesheetModify.UseVisualStyleBackColor = true;
            this.BtStylesheetModify.Click += new System.EventHandler(this.BtStylesheetModify_Click);
            // 
            // BtStylesheetDelete
            // 
            this.BtStylesheetDelete.Location = new System.Drawing.Point(513, 145);
            this.BtStylesheetDelete.Name = "BtStylesheetDelete";
            this.BtStylesheetDelete.Size = new System.Drawing.Size(75, 23);
            this.BtStylesheetDelete.TabIndex = 27;
            this.BtStylesheetDelete.Text = "Delete";
            this.BtStylesheetDelete.UseVisualStyleBackColor = true;
            this.BtStylesheetDelete.Click += new System.EventHandler(this.BtStylesheetDelete_Click);
            // 
            // ConfigureTasks
            // 
            this.AccessibleName = "ConfigureTasks";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 389);
            this.Controls.Add(this.BtStylesheetDelete);
            this.Controls.Add(this.BtStylesheetModify);
            this.Controls.Add(this.BtStylesheetAdd);
            this.Controls.Add(this.BtTaskDelete);
            this.Controls.Add(this.BtTaskModify);
            this.Controls.Add(this.BtTaskAdd);
            this.Controls.Add(this.BtAdvanced);
            this.Controls.Add(this.RtDescription);
            this.Controls.Add(this.LbDescription);
            this.Controls.Add(this.BtApply);
            this.Controls.Add(this.BtFilter);
            this.Controls.Add(this.BtConfigure);
            this.Controls.Add(this.BtPreview);
            this.Controls.Add(this.BtCancel);
            this.Controls.Add(this.BtSave);
            this.Controls.Add(this.LbStyles);
            this.Controls.Add(this.LbSummary);
            this.Controls.Add(this.LsStyles);
            this.Controls.Add(this.LbTask);
            this.Controls.Add(this.LsTasks);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigureTasks";
            this.Text = "Configure Tasks";
            this.Load += new System.EventHandler(this.ConfigureTasks_Load);
            this.DoubleClick += new System.EventHandler(this.ConfigureTasks_DoubleClick);
            this.Activated += new System.EventHandler(this.ConfigureTasks_Activated);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox LsTasks;
        private System.Windows.Forms.Label LbTask;
        private System.Windows.Forms.ListBox LsStyles;
        private System.Windows.Forms.Label LbSummary;
        private System.Windows.Forms.Label LbStyles;
        private System.Windows.Forms.Button BtConfigure;
        private System.Windows.Forms.Button BtPreview;
        private System.Windows.Forms.Button BtCancel;
        private System.Windows.Forms.Button BtSave;
        private System.Windows.Forms.Button BtFilter;
        private System.Windows.Forms.Button BtApply;
        private System.Windows.Forms.RichTextBox RtDescription;
        private System.Windows.Forms.Label LbDescription;
        private System.Windows.Forms.Button BtAdvanced;
        private System.Windows.Forms.Button BtTaskAdd;
        private System.Windows.Forms.Button BtTaskModify;
        private System.Windows.Forms.Button BtTaskDelete;
        private System.Windows.Forms.Button BtStylesheetAdd;
        private System.Windows.Forms.Button BtStylesheetModify;
        private System.Windows.Forms.Button BtStylesheetDelete;
    }
}
