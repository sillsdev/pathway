namespace JWTools
{
    partial class Localizer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Localizer));
            this.m_labelGroups = new System.Windows.Forms.Label();
            this.m_tree = new System.Windows.Forms.TreeView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.m_btnNew = new System.Windows.Forms.ToolStripButton();
            this.m_btnSave = new System.Windows.Forms.ToolStripButton();
            this.m_btnProperties = new System.Windows.Forms.ToolStripButton();
            this.m_btnClose = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.m_btnPrev = new System.Windows.Forms.ToolStripButton();
            this.m_btnNext = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.m_labelFilter = new System.Windows.Forms.ToolStripLabel();
            this.m_comboFilter = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.m_labelLanguage = new System.Windows.Forms.ToolStripLabel();
            this.m_comboLanguage = new System.Windows.Forms.ToolStripComboBox();
            this.m_lblRemaining = new System.Windows.Forms.ToolStripLabel();
            this.m_rtbInfo = new System.Windows.Forms.RichTextBox();
            this.m_labelYourLanguage = new System.Windows.Forms.Label();
            this.m_textYourLanguage = new System.Windows.Forms.TextBox();
            this.m_lblShortcutKey = new System.Windows.Forms.Label();
            this.m_comboShortcutKey = new System.Windows.Forms.ComboBox();
            this.m_lblToolTip = new System.Windows.Forms.Label();
            this.m_textToolTip = new System.Windows.Forms.TextBox();
            this.m_labelNeedsAttention = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_labelGroups
            // 
            this.m_labelGroups.AccessibleName = "m_labelGroups";
            this.m_labelGroups.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_labelGroups.Location = new System.Drawing.Point(12, 38);
            this.m_labelGroups.Name = "m_labelGroups";
            this.m_labelGroups.Size = new System.Drawing.Size(214, 21);
            this.m_labelGroups.TabIndex = 53;
            this.m_labelGroups.Text = "Groups of Settings:";
            this.m_labelGroups.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // m_tree
            // 
            this.m_tree.AccessibleName = "m_tree";
            this.m_tree.FullRowSelect = true;
            this.m_tree.HideSelection = false;
            this.m_tree.Location = new System.Drawing.Point(12, 61);
            this.m_tree.Name = "m_tree";
            this.m_tree.Size = new System.Drawing.Size(228, 605);
            this.m_tree.TabIndex = 70;
            this.m_tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.cmdTreeSelChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.AccessibleName = "toolStrip1";
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_btnNew,
            this.m_btnSave,
            this.m_btnProperties,
            this.m_btnClose,
            this.toolStripSeparator1,
            this.m_btnPrev,
            this.m_btnNext,
            this.toolStripSeparator2,
            this.m_labelFilter,
            this.m_comboFilter,
            this.toolStripSeparator3,
            this.m_labelLanguage,
            this.m_comboLanguage,
            this.m_lblRemaining});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(954, 36);
            this.toolStrip1.TabIndex = 73;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // m_btnNew
            // 
            this.m_btnNew.AccessibleName = "m_btnNew";
            this.m_btnNew.Image = ((System.Drawing.Image)(resources.GetObject("m_btnNew.Image")));
            this.m_btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_btnNew.Name = "m_btnNew";
            this.m_btnNew.Size = new System.Drawing.Size(44, 33);
            this.m_btnNew.Text = "New...";
            this.m_btnNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.m_btnNew.ToolTipText = "Create a new language";
            this.m_btnNew.Click += new System.EventHandler(this.cmdNewLanguage);
            // 
            // m_btnSave
            // 
            this.m_btnSave.AccessibleName = "m_btnSave";
            this.m_btnSave.Image = ((System.Drawing.Image)(resources.GetObject("m_btnSave.Image")));
            this.m_btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_btnSave.Name = "m_btnSave";
            this.m_btnSave.Size = new System.Drawing.Size(35, 33);
            this.m_btnSave.Text = "Save";
            this.m_btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.m_btnSave.ToolTipText = "Save the changes you have made thus far";
            this.m_btnSave.Click += new System.EventHandler(this.cmdSave);
            // 
            // m_btnProperties
            // 
            this.m_btnProperties.AccessibleName = "m_btnProperties";
            this.m_btnProperties.Image = ((System.Drawing.Image)(resources.GetObject("m_btnProperties.Image")));
            this.m_btnProperties.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_btnProperties.Name = "m_btnProperties";
            this.m_btnProperties.Size = new System.Drawing.Size(60, 33);
            this.m_btnProperties.Text = "Properties";
            this.m_btnProperties.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.m_btnProperties.Click += new System.EventHandler(this.cmdProperties);
            // 
            // m_btnClose
            // 
            this.m_btnClose.AccessibleName = "m_btnClose";
            this.m_btnClose.Image = ((System.Drawing.Image)(resources.GetObject("m_btnClose.Image")));
            this.m_btnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_btnClose.Name = "m_btnClose";
            this.m_btnClose.Size = new System.Drawing.Size(37, 33);
            this.m_btnClose.Text = "Close";
            this.m_btnClose.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this.m_btnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.m_btnClose.ToolTipText = "Close (exit) the Localizer Dialog";
            this.m_btnClose.Click += new System.EventHandler(this.cmdClose);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.AccessibleName = "toolStripSeparator1";
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 36);
            // 
            // m_btnPrev
            // 
            this.m_btnPrev.AccessibleName = "m_btnPrev";
            this.m_btnPrev.Image = ((System.Drawing.Image)(resources.GetObject("m_btnPrev.Image")));
            this.m_btnPrev.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_btnPrev.Name = "m_btnPrev";
            this.m_btnPrev.Size = new System.Drawing.Size(33, 33);
            this.m_btnPrev.Text = "Prev";
            this.m_btnPrev.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.m_btnPrev.ToolTipText = "Go to the Previous item.";
            this.m_btnPrev.Click += new System.EventHandler(this.cmdPreviousItem);
            // 
            // m_btnNext
            // 
            this.m_btnNext.AccessibleName = "m_btnNext";
            this.m_btnNext.Image = ((System.Drawing.Image)(resources.GetObject("m_btnNext.Image")));
            this.m_btnNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_btnNext.Name = "m_btnNext";
            this.m_btnNext.Size = new System.Drawing.Size(34, 33);
            this.m_btnNext.Text = "Next";
            this.m_btnNext.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.m_btnNext.ToolTipText = "Go to the Next item";
            this.m_btnNext.Click += new System.EventHandler(this.cmdNextItem);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.AccessibleName = "toolStripSeparator2";
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 36);
            // 
            // m_labelFilter
            // 
            this.m_labelFilter.AccessibleName = "m_labelFilter";
            this.m_labelFilter.Name = "m_labelFilter";
            this.m_labelFilter.Size = new System.Drawing.Size(35, 33);
            this.m_labelFilter.Text = "Filter:";
            // 
            // m_comboFilter
            // 
            this.m_comboFilter.AccessibleName = "m_comboFilter";
            this.m_comboFilter.Name = "m_comboFilter";
            this.m_comboFilter.Size = new System.Drawing.Size(230, 36);
            this.m_comboFilter.ToolTipText = "Use this to limit which items appear in the Groups of Settings tree";
            this.m_comboFilter.SelectedIndexChanged += new System.EventHandler(this.cmdFilterChanged);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.AccessibleName = "toolStripSeparator3";
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 36);
            // 
            // m_labelLanguage
            // 
            this.m_labelLanguage.AccessibleName = "m_labelLanguage";
            this.m_labelLanguage.Name = "m_labelLanguage";
            this.m_labelLanguage.Size = new System.Drawing.Size(58, 33);
            this.m_labelLanguage.Text = "Language:";
            // 
            // m_comboLanguage
            // 
            this.m_comboLanguage.AccessibleName = "m_comboLanguage";
            this.m_comboLanguage.Name = "m_comboLanguage";
            this.m_comboLanguage.Size = new System.Drawing.Size(160, 36);
            this.m_comboLanguage.SelectedIndexChanged += new System.EventHandler(this.cmdLanguageChanged);
            // 
            // m_lblRemaining
            // 
            this.m_lblRemaining.AccessibleName = "m_lblRemaining";
            this.m_lblRemaining.Name = "m_lblRemaining";
            this.m_lblRemaining.Size = new System.Drawing.Size(64, 33);
            this.m_lblRemaining.Text = "(Remaining)";
            // 
            // m_rtbInfo
            // 
            this.m_rtbInfo.AccessibleName = "m_rtbInfo";
            this.m_rtbInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.m_rtbInfo.Location = new System.Drawing.Point(249, 46);
            this.m_rtbInfo.Name = "m_rtbInfo";
            this.m_rtbInfo.ReadOnly = true;
            this.m_rtbInfo.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.m_rtbInfo.Size = new System.Drawing.Size(690, 302);
            this.m_rtbInfo.TabIndex = 74;
            this.m_rtbInfo.Text = "(info)";
            this.m_rtbInfo.DoubleClick += new System.EventHandler(this.cmdEditDescriptions);
            // 
            // m_labelYourLanguage
            // 
            this.m_labelYourLanguage.AccessibleName = "m_labelYourLanguage";
            this.m_labelYourLanguage.Location = new System.Drawing.Point(246, 351);
            this.m_labelYourLanguage.Name = "m_labelYourLanguage";
            this.m_labelYourLanguage.Size = new System.Drawing.Size(96, 22);
            this.m_labelYourLanguage.TabIndex = 60;
            this.m_labelYourLanguage.Text = "Your Language:";
            this.m_labelYourLanguage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_textYourLanguage
            // 
            this.m_textYourLanguage.AccessibleName = "m_textYourLanguage";
            this.m_textYourLanguage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_textYourLanguage.ForeColor = System.Drawing.Color.Navy;
            this.m_textYourLanguage.Location = new System.Drawing.Point(348, 348);
            this.m_textYourLanguage.Multiline = true;
            this.m_textYourLanguage.Name = "m_textYourLanguage";
            this.m_textYourLanguage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.m_textYourLanguage.Size = new System.Drawing.Size(591, 174);
            this.m_textYourLanguage.TabIndex = 63;
            this.m_textYourLanguage.Text = "(Your Language)";
            this.m_textYourLanguage.TextChanged += new System.EventHandler(this.cmdLanguageValueChanged);
            // 
            // m_lblShortcutKey
            // 
            this.m_lblShortcutKey.AccessibleName = "m_lblShortcutKey";
            this.m_lblShortcutKey.Location = new System.Drawing.Point(246, 532);
            this.m_lblShortcutKey.Name = "m_lblShortcutKey";
            this.m_lblShortcutKey.Size = new System.Drawing.Size(80, 23);
            this.m_lblShortcutKey.TabIndex = 75;
            this.m_lblShortcutKey.Text = "Shortcut Key:";
            this.m_lblShortcutKey.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_comboShortcutKey
            // 
            this.m_comboShortcutKey.AccessibleName = "m_comboShortcutKey";
            this.m_comboShortcutKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_comboShortcutKey.FormattingEnabled = true;
            this.m_comboShortcutKey.Location = new System.Drawing.Point(348, 528);
            this.m_comboShortcutKey.Name = "m_comboShortcutKey";
            this.m_comboShortcutKey.Size = new System.Drawing.Size(92, 28);
            this.m_comboShortcutKey.TabIndex = 76;
            // 
            // m_lblToolTip
            // 
            this.m_lblToolTip.AccessibleName = "m_lblToolTip";
            this.m_lblToolTip.Location = new System.Drawing.Point(246, 562);
            this.m_lblToolTip.Name = "m_lblToolTip";
            this.m_lblToolTip.Size = new System.Drawing.Size(65, 23);
            this.m_lblToolTip.TabIndex = 77;
            this.m_lblToolTip.Text = "ToolTip:";
            this.m_lblToolTip.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_textToolTip
            // 
            this.m_textToolTip.AccessibleName = "m_textToolTip";
            this.m_textToolTip.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_textToolTip.Location = new System.Drawing.Point(348, 562);
            this.m_textToolTip.Multiline = true;
            this.m_textToolTip.Name = "m_textToolTip";
            this.m_textToolTip.Size = new System.Drawing.Size(591, 77);
            this.m_textToolTip.TabIndex = 78;
            // 
            // m_labelNeedsAttention
            // 
            this.m_labelNeedsAttention.AccessibleName = "m_labelNeedsAttention";
            this.m_labelNeedsAttention.ForeColor = System.Drawing.Color.Red;
            this.m_labelNeedsAttention.Location = new System.Drawing.Point(246, 643);
            this.m_labelNeedsAttention.Name = "m_labelNeedsAttention";
            this.m_labelNeedsAttention.Size = new System.Drawing.Size(693, 23);
            this.m_labelNeedsAttention.TabIndex = 79;
            this.m_labelNeedsAttention.Text = "(attention needed)";
            this.m_labelNeedsAttention.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Localizer
            // 
            this.AccessibleName = "Localizer";
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(954, 679);
            this.Controls.Add(this.m_labelNeedsAttention);
            this.Controls.Add(this.m_textToolTip);
            this.Controls.Add(this.m_lblToolTip);
            this.Controls.Add(this.m_comboShortcutKey);
            this.Controls.Add(this.m_lblShortcutKey);
            this.Controls.Add(this.m_rtbInfo);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.m_labelYourLanguage);
            this.Controls.Add(this.m_textYourLanguage);
            this.Controls.Add(this.m_tree);
            this.Controls.Add(this.m_labelGroups);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Localizer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Localizer";
            this.Load += new System.EventHandler(this.cmdLoad);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.cmdClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label m_labelGroups;
        private System.Windows.Forms.TreeView m_tree;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton m_btnClose;
        private System.Windows.Forms.ToolStripButton m_btnSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton m_btnPrev;
        private System.Windows.Forms.ToolStripButton m_btnNext;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel m_labelFilter;
        private System.Windows.Forms.ToolStripComboBox m_comboFilter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel m_labelLanguage;
        private System.Windows.Forms.ToolStripComboBox m_comboLanguage;
        private System.Windows.Forms.ToolStripButton m_btnNew;
        private System.Windows.Forms.RichTextBox m_rtbInfo;
        private System.Windows.Forms.Label m_labelYourLanguage;
        private System.Windows.Forms.TextBox m_textYourLanguage;
        private System.Windows.Forms.ToolStripButton m_btnProperties;
        private System.Windows.Forms.Label m_lblShortcutKey;
        private System.Windows.Forms.ComboBox m_comboShortcutKey;
        private System.Windows.Forms.Label m_lblToolTip;
        private System.Windows.Forms.TextBox m_textToolTip;
        private System.Windows.Forms.ToolStripLabel m_lblRemaining;
        private System.Windows.Forms.Label m_labelNeedsAttention;
    }
}
