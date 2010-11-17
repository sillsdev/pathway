namespace SIL.PublishingSolution
{
    partial class PrintVia
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintVia));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkAvoidOdtCrash = new System.Windows.Forms.CheckBox();
            this.txtSaveInFolder = new System.Windows.Forms.TextBox();
            this.btnBrwsSaveInFolder = new System.Windows.Forms.Button();
            this.BtnBrwsLayout = new System.Windows.Forms.Button();
            this.chkExtraProcessing = new System.Windows.Forms.CheckBox();
            this.cmbSelectLayout = new System.Windows.Forms.ComboBox();
            this.chkGramSketch = new System.Windows.Forms.CheckBox();
            this.chkRevIndexes = new System.Windows.Forms.CheckBox();
            this.chkConfigDictionary = new System.Windows.Forms.CheckBox();
            this.cmbPrintVia = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnOk = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnHelp = new System.Windows.Forms.Button();
            this.tt_PrintVia = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPolicy = new System.Windows.Forms.Button();
            this.chkPolicy = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.AccessibleName = "Group";
            this.groupBox1.Controls.Add(this.chkAvoidOdtCrash);
            this.groupBox1.Controls.Add(this.txtSaveInFolder);
            this.groupBox1.Controls.Add(this.btnBrwsSaveInFolder);
            this.groupBox1.Controls.Add(this.BtnBrwsLayout);
            this.groupBox1.Controls.Add(this.chkExtraProcessing);
            this.groupBox1.Controls.Add(this.cmbSelectLayout);
            this.groupBox1.Controls.Add(this.chkGramSketch);
            this.groupBox1.Controls.Add(this.chkRevIndexes);
            this.groupBox1.Controls.Add(this.chkConfigDictionary);
            this.groupBox1.Controls.Add(this.cmbPrintVia);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(16, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(637, 274);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // chkAvoidOdtCrash
            // 
            this.chkAvoidOdtCrash.AutoSize = true;
            this.chkAvoidOdtCrash.Location = new System.Drawing.Point(161, 242);
            this.chkAvoidOdtCrash.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkAvoidOdtCrash.Name = "chkAvoidOdtCrash";
            this.chkAvoidOdtCrash.Size = new System.Drawing.Size(475, 21);
            this.chkAvoidOdtCrash.TabIndex = 15;
            this.chkAvoidOdtCrash.Text = "Reduce the number of style names to keep Open Office from crashing.";
            this.chkAvoidOdtCrash.UseVisualStyleBackColor = true;
            this.chkAvoidOdtCrash.Visible = false;
            this.chkAvoidOdtCrash.MouseHover += new System.EventHandler(this.ChkAvoidOdtCrashMouseHover);
            // 
            // txtSaveInFolder
            // 
            this.txtSaveInFolder.AccessibleName = "SaveIn";
            this.txtSaveInFolder.Location = new System.Drawing.Point(161, 210);
            this.txtSaveInFolder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSaveInFolder.Name = "txtSaveInFolder";
            this.txtSaveInFolder.Size = new System.Drawing.Size(375, 22);
            this.txtSaveInFolder.TabIndex = 14;
            // 
            // btnBrwsSaveInFolder
            // 
            this.btnBrwsSaveInFolder.AccessibleName = "Browse";
            this.btnBrwsSaveInFolder.Location = new System.Drawing.Point(541, 208);
            this.btnBrwsSaveInFolder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBrwsSaveInFolder.Name = "btnBrwsSaveInFolder";
            this.btnBrwsSaveInFolder.Size = new System.Drawing.Size(32, 30);
            this.btnBrwsSaveInFolder.TabIndex = 13;
            this.btnBrwsSaveInFolder.Text = "...";
            this.btnBrwsSaveInFolder.UseVisualStyleBackColor = true;
            this.btnBrwsSaveInFolder.Click += new System.EventHandler(this.btnBrwsSaveInFolder_Click);
            // 
            // BtnBrwsLayout
            // 
            this.BtnBrwsLayout.AccessibleName = "Preview";
            this.BtnBrwsLayout.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtnBrwsLayout.BackgroundImage")));
            this.BtnBrwsLayout.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtnBrwsLayout.Location = new System.Drawing.Point(541, 145);
            this.BtnBrwsLayout.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnBrwsLayout.Name = "BtnBrwsLayout";
            this.BtnBrwsLayout.Size = new System.Drawing.Size(32, 30);
            this.BtnBrwsLayout.TabIndex = 12;
            this.BtnBrwsLayout.UseVisualStyleBackColor = true;
            this.BtnBrwsLayout.Click += new System.EventHandler(this.BtnBrwsLayout_Click);
            // 
            // chkExtraProcessing
            // 
            this.chkExtraProcessing.AccessibleName = "Extra";
            this.chkExtraProcessing.AutoSize = true;
            this.chkExtraProcessing.Location = new System.Drawing.Point(161, 183);
            this.chkExtraProcessing.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkExtraProcessing.Name = "chkExtraProcessing";
            this.chkExtraProcessing.Size = new System.Drawing.Size(397, 21);
            this.chkExtraProcessing.TabIndex = 10;
            this.chkExtraProcessing.Text = "Insert first and last book, chapter and / or verse in header.";
            this.chkExtraProcessing.UseVisualStyleBackColor = true;
            // 
            // cmbSelectLayout
            // 
            this.cmbSelectLayout.AccessibleName = "Layout";
            this.cmbSelectLayout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSelectLayout.FormattingEnabled = true;
            this.cmbSelectLayout.Location = new System.Drawing.Point(161, 148);
            this.cmbSelectLayout.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbSelectLayout.Name = "cmbSelectLayout";
            this.cmbSelectLayout.Size = new System.Drawing.Size(375, 24);
            this.cmbSelectLayout.TabIndex = 9;
            this.cmbSelectLayout.SelectedIndexChanged += new System.EventHandler(this.cmbSelectLayout_SelectedIndexChanged);
            // 
            // chkGramSketch
            // 
            this.chkGramSketch.AutoSize = true;
            this.chkGramSketch.Location = new System.Drawing.Point(161, 119);
            this.chkGramSketch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkGramSketch.Name = "chkGramSketch";
            this.chkGramSketch.Size = new System.Drawing.Size(136, 21);
            this.chkGramSketch.TabIndex = 8;
            this.chkGramSketch.Text = "Grammar Sketch";
            this.chkGramSketch.UseVisualStyleBackColor = true;
            this.chkGramSketch.CheckedChanged += new System.EventHandler(this.chkGramSketch_CheckedChanged);
            // 
            // chkRevIndexes
            // 
            this.chkRevIndexes.AutoSize = true;
            this.chkRevIndexes.Location = new System.Drawing.Point(161, 91);
            this.chkRevIndexes.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkRevIndexes.Name = "chkRevIndexes";
            this.chkRevIndexes.Size = new System.Drawing.Size(138, 21);
            this.chkRevIndexes.TabIndex = 7;
            this.chkRevIndexes.Text = "Reversal Indexes";
            this.chkRevIndexes.UseVisualStyleBackColor = true;
            this.chkRevIndexes.CheckedChanged += new System.EventHandler(this.chkRevIndexes_CheckedChanged);
            // 
            // chkConfigDictionary
            // 
            this.chkConfigDictionary.AutoSize = true;
            this.chkConfigDictionary.Location = new System.Drawing.Point(161, 63);
            this.chkConfigDictionary.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkConfigDictionary.Name = "chkConfigDictionary";
            this.chkConfigDictionary.Size = new System.Drawing.Size(166, 21);
            this.chkConfigDictionary.TabIndex = 6;
            this.chkConfigDictionary.Text = "Configured Dictionary";
            this.chkConfigDictionary.UseVisualStyleBackColor = true;
            this.chkConfigDictionary.CheckedChanged += new System.EventHandler(this.chkConfigDictionary_CheckedChanged);
            // 
            // cmbPrintVia
            // 
            this.cmbPrintVia.AccessibleName = "PrintVia";
            this.cmbPrintVia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPrintVia.FormattingEnabled = true;
            this.cmbPrintVia.Location = new System.Drawing.Point(161, 25);
            this.cmbPrintVia.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbPrintVia.Name = "cmbPrintVia";
            this.cmbPrintVia.Size = new System.Drawing.Size(261, 24);
            this.cmbPrintVia.TabIndex = 5;
            this.cmbPrintVia.SelectedIndexChanged += new System.EventHandler(this.cmbPrintVia_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(59, 215);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 17);
            this.label5.TabIndex = 4;
            this.label5.Text = "Save in folder:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(41, 186);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "Extra processing:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(64, 153);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Select layout:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 65);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(148, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Select data to include:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(95, 30);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Print via:";
            // 
            // BtnOk
            // 
            this.BtnOk.Location = new System.Drawing.Point(329, 373);
            this.BtnOk.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(100, 28);
            this.BtnOk.TabIndex = 2;
            this.BtnOk.Text = "OK";
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(437, 373);
            this.BtnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(100, 28);
            this.BtnCancel.TabIndex = 3;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtnHelp
            // 
            this.BtnHelp.Location = new System.Drawing.Point(545, 373);
            this.BtnHelp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnHelp.Name = "BtnHelp";
            this.BtnHelp.Size = new System.Drawing.Size(100, 28);
            this.BtnHelp.TabIndex = 4;
            this.BtnHelp.Text = "Help";
            this.BtnHelp.UseVisualStyleBackColor = true;
            this.BtnHelp.Click += new System.EventHandler(this.BtnHelp_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnPolicy);
            this.groupBox2.Controls.Add(this.chkPolicy);
            this.groupBox2.Location = new System.Drawing.Point(17, 297);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(636, 68);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            // 
            // btnPolicy
            // 
            this.btnPolicy.Location = new System.Drawing.Point(529, 19);
            this.btnPolicy.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnPolicy.Name = "btnPolicy";
            this.btnPolicy.Size = new System.Drawing.Size(99, 28);
            this.btnPolicy.TabIndex = 2;
            this.btnPolicy.Text = "Policy";
            this.btnPolicy.UseVisualStyleBackColor = true;
            this.btnPolicy.Click += new System.EventHandler(this.btnPolicy_Click);
            // 
            // chkPolicy
            // 
            this.chkPolicy.Location = new System.Drawing.Point(157, 10);
            this.chkPolicy.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkPolicy.Name = "chkPolicy";
            this.chkPolicy.Size = new System.Drawing.Size(379, 49);
            this.chkPolicy.TabIndex = 1;
            this.chkPolicy.Text = "I am conforming to the policies related to copyright, permission and publication." +
                "";
            this.chkPolicy.UseVisualStyleBackColor = true;
            this.chkPolicy.CheckedChanged += new System.EventHandler(this.chkPolicy_CheckedChanged);
            // 
            // PrintVia
            // 
            this.AccessibleName = "Print via...";
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 418);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.BtnHelp);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "PrintVia";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Print via...";
            this.Load += new System.EventHandler(this.PrintVia_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbPrintVia;
        private System.Windows.Forms.CheckBox chkExtraProcessing;
        private System.Windows.Forms.ComboBox cmbSelectLayout;
        private System.Windows.Forms.CheckBox chkGramSketch;
        private System.Windows.Forms.CheckBox chkRevIndexes;
        private System.Windows.Forms.CheckBox chkConfigDictionary;
        private System.Windows.Forms.Button btnBrwsSaveInFolder;
        private System.Windows.Forms.Button BtnBrwsLayout;
        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnHelp;
        private System.Windows.Forms.TextBox txtSaveInFolder;
        private System.Windows.Forms.CheckBox chkAvoidOdtCrash;
        private System.Windows.Forms.ToolTip tt_PrintVia;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnPolicy;
        private System.Windows.Forms.CheckBox chkPolicy;
    }
}