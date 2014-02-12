// --------------------------------------------------------------------------------------
// <copyright file="ValidationDlg.cs" from='2010' to='2014' company='SIL International'>
//      Copyright © 2014 SIL International. All Rights Reserved.
//
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
// <author>Erik Brommers</author>
// <email>erik_brommers@sil.org</email>
// Last reviewed:
// 
// <remarks>
// epub Validator. This program calls the open source epubcheck utility to validate
// the given epub file. epubcheck is hosted on Google Code at: 
// http://code.google.com/p/epubcheck/
// </remarks>
// --------------------------------------------------------------------------------------
using System;
using System.IO;
using System.Windows.Forms;
using System.Resources;

namespace epubValidator
{
    public partial class ValidationDialog : Form
    {
        public string FileName { get; set; }

        ResourceManager _rm = new ResourceManager("epubValidator.Properties.Resources", typeof(ValidationDialog).Assembly);
        public ValidationDialog()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = _rm.GetString("Title");
            label1.Text = _rm.GetString("Description");
            if (FileName != null)
            {
                txtFilename.Text = FileName;
                lblInstructions.Text = _rm.GetString("InstructionsClickValidate");
            }
            else if (Program.args.Length == 2)
            {
                txtFilename.Text = Program.args[1];
                lblInstructions.Text = _rm.GetString("InstructionsClickValidate");
            }
            else
            {
                lblInstructions.Text = _rm.GetString("InstructionsSpecifyFilename");
            }
            lblStatus.Text = _rm.GetString("PleaseWait");
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (ofp.ShowDialog() == DialogResult.OK)
            {
                txtFilename.Text = ofp.FileName;
                btnValidate.Enabled = true;
            }
        }

        private void txtFilename_TextChanged(object sender, EventArgs e)
        {
            btnValidate.Enabled = File.Exists(txtFilename.Text);
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            // disable the UI and display the "please wait" label
            btnValidate.Enabled = false;
            btnBrowse.Enabled = false;
            txtFilename.Enabled = false;
            lblStatus.Visible = true;
            var myCursor = Cursor.Current;
            Cursor = Cursors.WaitCursor;
            // launch the item
            var results = Program.ValidateFile(txtFilename.Text);
            // display the results
            Cursor = myCursor;
            var dlg = new frmResults();
            dlg.ValidationResults = results;
            dlg.ShowDialog();
            //close out
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
