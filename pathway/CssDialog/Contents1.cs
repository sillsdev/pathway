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
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public partial class Contents1 : Form, IExportContents
    {
        public Contents1()
        {
            InitializeComponent();
        }

        #region Properties

        public string DatabaseName { set; get; }

        public bool ExportMain
        {
            set
            {
                ChkFilterLexicon.Checked = value;
            }
            get
            {
                return ChkFilterLexicon.Checked;
            }
        }

        public bool ExportReversal
        {
            set
            {
                ChkFilterReversal.Checked = value;
            }
            get
            {
                return ChkFilterReversal.Checked;
            }
        }

        public bool ExportGrammar
        {
            set
            {
                ChkFilterGrammar.Checked = value;
            }
            get
            {
                return ChkFilterGrammar.Checked;
            }
        }

        public bool ReversalExists
        {
            set
            {
                ChkFilterGrammar.Enabled = value;
            }
        }

        public bool GrammarExists
        {
            set
            {
                ChkFilterGrammar.Enabled = value;
            }
        }

        public bool ExistingDirectoryInput
        {
           get
            {
                return ChkExistingDictionary.Checked;
            }
        }

        public string OutputLocationPath
        {
            get
            {
                return TxtLocation.Text;
            }
        }

        public string ExistingDirectoryLocationPath
        {
            get
            {
                return TxtExistingDirectory.Text;
            }
        }

        public string DictionaryName
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
            CancelButton = btnCancel;
            AcceptButton = BtnOk;


            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            filePath = Common.PathCombine(filePath, Application.ProductName);
            filePath = Common.PathCombine(filePath, DatabaseName);
            string fileName = Common.GetNewFolderName(filePath, "Dictionary");
            string destinationFolder = Common.PathCombine(filePath, fileName);
            Directory.CreateDirectory(destinationFolder);
            TxtLocation.Text = destinationFolder;
            if (string.IsNullOrEmpty(TxtName.Text))
                TxtName.Text = Path.GetFileName(fileName);
            ChkFilterLexicon.Checked = ChkFilterReversal.Checked = true;
        }

        private void BtnSectionFilterContinue_Click(object sender, EventArgs e)
        {
            if (!validateInput())
                return;
            DialogResult = ChkExistingDictionary.Checked ? DialogResult.No : DialogResult.Yes;
            Close();
        }

        private void BtnSectionFilterCancel_Click(object sender, EventArgs e)
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
            if (ChkExistingDictionary.Checked)
            {
                string existingDir = TxtExistingDirectory.Text;
                isExistingLocation = Directory.Exists(existingDir);
            }

            BtnOk.Enabled = isLocation && isExistingLocation && (ChkFilterLexicon.Checked || ChkFilterReversal.Checked);
        }

        private void TxtLocation_TextChanged(object sender, EventArgs e)
        {
            ValidateDirectoryLocation();
        }

        private void ChkExistingDictionary_CheckedChanged(object sender, EventArgs e)
        {
            ValidateDirectoryLocation();
            lblExistingDirectory.Enabled = ChkExistingDictionary.Checked;
            TxtExistingDirectory.Enabled = ChkExistingDictionary.Checked;
            BtnExistingDirectory.Enabled = ChkExistingDictionary.Checked;
        }

        private void BtnExistingDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowDialog();
            try
            {
            TxtExistingDirectory.Text = folderDlg.SelectedPath;
        }
            catch (NotSupportedException)
            {
                TxtExistingDirectory.Text = "";
            }
        }

        private void TxtExistingDirectory_TextChanged(object sender, EventArgs e)
        {
            ValidateDirectoryLocation();
        }

        private void TxtName_TextChanged(object sender, EventArgs e)
        {
            ValidateDirectoryLocation();
        }

        private void ChkFilterLexicon_CheckedChanged(object sender, EventArgs e)
        {
            ValidateDirectoryLocation();
        }

        private void ChkFilterReversal_CheckedChanged(object sender, EventArgs e)
        {
            ValidateDirectoryLocation();
        }

    }
}
