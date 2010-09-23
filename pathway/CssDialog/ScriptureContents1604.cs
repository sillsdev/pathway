// --------------------------------------------------------------------------------------------
#region // Copyright (c) 2009, SIL International. All Rights Reserved.
// <copyright file="Contents.cs" from='2009' to='2009' company='SIL International'>
//		Copyright (c) 2009, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Sets the filter for flex plugin to export main with/without flexReversal
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Windows.Forms;
using SIL.FieldWorks.Common.FwUtils;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public partial class ScriptureContents1 : Form, IScriptureContents
    {
        public ScriptureContents1()
        {
            InitializeComponent();
        }

        #region Properties
        public string DatabaseName { set; get; }

        public string OutputLocationPath
        {
            get
            {
                return TxtLocation.Text;
            }
        }

        public bool ExistingPublication
        {
           get
            {
                return ChkExistingPublication.Checked;
            }
        }

        public string ExistingLocationPath
        {
            get
            {
                return TxtExistingPublication.Text;
            }
        }

        public string PublicationName
        {
            set
            {
                TxtName.Text = value;
            }
            get
            {
                return TxtName.Text;
            }
        }
        #endregion Properties

        private void Contents_Load(object sender, EventArgs e)
        {
            CancelButton = BtnCancel;
            AcceptButton = BtnOk;

            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            filePath = Common.PathCombine(filePath, Application.ProductName);
            filePath = Common.PathCombine(filePath, DatabaseName);
            string fileName = Common.GetNewFolderName(filePath, "Scripture");
            string destinationFolder = Common.PathCombine(filePath, fileName);
            Directory.CreateDirectory(destinationFolder);
            TxtLocation.Text = destinationFolder;
            if (string.IsNullOrEmpty(TxtName.Text))
                TxtName.Text = Path.GetFileName(fileName);
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (!validateInput())
                return;
            DialogResult = ChkExistingPublication.Checked ? DialogResult.No : DialogResult.Yes;
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnXHTMLBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowDialog();
            try
            {
            TxtLocation.Text = folderDlg.SelectedPath;
        }
            catch (NotSupportedException)
            {
                TxtLocation.Text = "";
            }
        }

        protected bool validateInput()
        {
            bool isValid = true;
            if (TxtName.Text.Trim() == string.Empty)
            {
                TxtName.Focus();
                isValid = false;
            }
            return isValid;
        }

        protected void ValidateDirectoryLocation()
        {
            string location = TxtLocation.Text;
            bool isLocation = Directory.Exists(location);
          
            bool isExistingLocation = true;
            if (ChkExistingPublication.Checked)
            {
                string existingDir = TxtExistingPublication.Text;
                isExistingLocation = Directory.Exists(existingDir);
            }

            BtnOk.Enabled = isLocation && isExistingLocation;
        }

        private void TxtLocation_TextChanged(object sender, EventArgs e)
        {
            ValidateDirectoryLocation();
        }

        private void ChkExistingPublication_CheckedChanged(object sender, EventArgs e)
        {
            ValidateDirectoryLocation();
            lblExistingDirectory.Enabled = ChkExistingPublication.Checked;
            TxtExistingPublication.Enabled = ChkExistingPublication.Checked;
            BtnExistingPublication.Enabled = ChkExistingPublication.Checked;
        }

        private void BtnExistingPublication_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowDialog();
            try
            {
            TxtExistingPublication.Text = folderDlg.SelectedPath;
        }
            catch (NotSupportedException)
            {
                TxtExistingPublication.Text = "";
            }
        }

        private void TxtExistingDirectory_TextChanged(object sender, EventArgs e)
        {
            ValidateDirectoryLocation();
        }

        //private void SectionFilter_Activated(object sender, EventArgs e)
        //{
        //    Common.SetFont(this);
        //}

        private void TxtName_TextChanged(object sender, EventArgs e)
        {
            ValidateDirectoryLocation();
        }

        //private void ChkFilterLexicon_CheckedChanged(object sender, EventArgs e)
        //{
        //    ValidateDirectoryLocation();
        //}
    }
}
