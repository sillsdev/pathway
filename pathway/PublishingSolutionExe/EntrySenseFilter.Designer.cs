namespace SIL.PublishingSolution
{
    partial class EntrySenseFilter
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
            this.label1 = new System.Windows.Forms.Label();
            this.TxtFilter = new System.Windows.Forms.TextBox();
            this.radAnywhere = new System.Windows.Forms.RadioButton();
            this.radWholeItem = new System.Windows.Forms.RadioButton();
            this.radAtEnd = new System.Windows.Forms.RadioButton();
            this.radAtStart = new System.Windows.Forms.RadioButton();
            this.BtnOk = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.ChkMatchCase = new System.Windows.Forms.CheckBox();
            this.radioNone = new System.Windows.Forms.RadioButton();
            this.radNotEqual = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter the text to search for";
            // 
            // TxtFilter
            // 
            this.TxtFilter.Location = new System.Drawing.Point(15, 25);
            this.TxtFilter.Name = "TxtFilter";
            this.TxtFilter.Size = new System.Drawing.Size(388, 20);
            this.TxtFilter.TabIndex = 1;
            this.TxtFilter.Text = "stu";
            // 
            // radAnywhere
            // 
            this.radAnywhere.AutoSize = true;
            this.radAnywhere.Location = new System.Drawing.Point(187, 64);
            this.radAnywhere.Name = "radAnywhere";
            this.radAnywhere.Size = new System.Drawing.Size(72, 17);
            this.radAnywhere.TabIndex = 2;
            this.radAnywhere.Text = "Anywhere";
            this.radAnywhere.UseVisualStyleBackColor = true;
            // 
            // radWholeItem
            // 
            this.radWholeItem.AutoSize = true;
            this.radWholeItem.Location = new System.Drawing.Point(15, 64);
            this.radWholeItem.Name = "radWholeItem";
            this.radWholeItem.Size = new System.Drawing.Size(79, 17);
            this.radWholeItem.TabIndex = 3;
            this.radWholeItem.Text = "Whole Item";
            this.radWholeItem.UseVisualStyleBackColor = true;
            // 
            // radAtEnd
            // 
            this.radAtEnd.AutoSize = true;
            this.radAtEnd.Location = new System.Drawing.Point(103, 88);
            this.radAtEnd.Name = "radAtEnd";
            this.radAtEnd.Size = new System.Drawing.Size(57, 17);
            this.radAtEnd.TabIndex = 4;
            this.radAtEnd.Text = "At End";
            this.radAtEnd.UseVisualStyleBackColor = true;
            // 
            // radAtStart
            // 
            this.radAtStart.AutoSize = true;
            this.radAtStart.Location = new System.Drawing.Point(103, 64);
            this.radAtStart.Name = "radAtStart";
            this.radAtStart.Size = new System.Drawing.Size(60, 17);
            this.radAtStart.TabIndex = 5;
            this.radAtStart.Text = "At Start";
            this.radAtStart.UseVisualStyleBackColor = true;
            // 
            // BtnOk
            // 
            this.BtnOk.AccessibleName = "btnOk";
            this.BtnOk.Location = new System.Drawing.Point(103, 129);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(75, 23);
            this.BtnOk.TabIndex = 11;
            this.BtnOk.Text = "&Ok";
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.AccessibleName = "btnCancel";
            this.BtnCancel.Location = new System.Drawing.Point(210, 129);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 12;
            this.BtnCancel.Text = "&Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // ChkMatchCase
            // 
            this.ChkMatchCase.AutoSize = true;
            this.ChkMatchCase.Checked = true;
            this.ChkMatchCase.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkMatchCase.Location = new System.Drawing.Point(285, 65);
            this.ChkMatchCase.Name = "ChkMatchCase";
            this.ChkMatchCase.Size = new System.Drawing.Size(83, 17);
            this.ChkMatchCase.TabIndex = 13;
            this.ChkMatchCase.Text = "Match Case";
            this.ChkMatchCase.UseVisualStyleBackColor = true;
            // 
            // radioNone
            // 
            this.radioNone.AutoSize = true;
            this.radioNone.Checked = true;
            this.radioNone.Location = new System.Drawing.Point(187, 88);
            this.radioNone.Name = "radioNone";
            this.radioNone.Size = new System.Drawing.Size(51, 17);
            this.radioNone.TabIndex = 14;
            this.radioNone.TabStop = true;
            this.radioNone.Text = "None";
            this.radioNone.UseVisualStyleBackColor = true;
            // 
            // radNotEqual
            // 
            this.radNotEqual.AutoSize = true;
            this.radNotEqual.Location = new System.Drawing.Point(15, 88);
            this.radNotEqual.Name = "radNotEqual";
            this.radNotEqual.Size = new System.Drawing.Size(72, 17);
            this.radNotEqual.TabIndex = 15;
            this.radNotEqual.Text = "Not Equal";
            this.radNotEqual.UseVisualStyleBackColor = true;
            // 
            // EntrySenseFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 178);
            this.Controls.Add(this.radNotEqual);
            this.Controls.Add(this.radioNone);
            this.Controls.Add(this.ChkMatchCase);
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.radAtStart);
            this.Controls.Add(this.radAtEnd);
            this.Controls.Add(this.radWholeItem);
            this.Controls.Add(this.radAnywhere);
            this.Controls.Add(this.TxtFilter);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EntrySenseFilter";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Filter for Items Containing...";
            this.Load += new System.EventHandler(this.EntrySenseFilter_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtFilter;
        private System.Windows.Forms.RadioButton radAnywhere;
        private System.Windows.Forms.RadioButton radWholeItem;
        private System.Windows.Forms.RadioButton radAtEnd;
        private System.Windows.Forms.RadioButton radAtStart;
        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.CheckBox ChkMatchCase;
        private System.Windows.Forms.RadioButton radioNone;
        private System.Windows.Forms.RadioButton radNotEqual;
    }
}