// --------------------------------------------------------------------------------------------
// <copyright file="NewDictionary.cs" from='2009' to='2009' company='SIL International'>
//      Copyright © 2009, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// New Dictionary creation
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Windows.Forms;
using System.IO;
using JWTools;
using SIL.Tool;
using SIL.Tool.Localization;

namespace SIL.PublishingSolution
{
    #region Class NewPublication
    public partial class NewPublication : Form
    {
        #region Constructor
        public NewPublication()
        {
            InitializeComponent();
        }
        #endregion

        #region private variable
        private string _supportFolder;
        bool _automaticProjectName = true;
        string _mergedCSS;
        string _projectType = string.Empty;
        #endregion

        #region public variable
        public string CSSTemplate;
        public string XhtmlFile;
        public string ProjectName;
        public string DicPath;
        public bool Success = false;
        #endregion

        #region Events

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void lstSourceCSS_SelectedIndexChanged(object sender, EventArgs e)
        {
            string xhtmlFile, cssFile;
            xhtmlFile = Common.PathCombine(Common.PathCombine(_supportFolder, _projectType), "Template.xhtml");
            try
            {
                if (lstSourceCSS.SelectedIndex > 0)
                {
                    webTemplatePreview.Visible = true;
                    cssFile = Common.PathCombine(Common.PathCombine(_supportFolder, _projectType), lstSourceCSS.Text);
                    _mergedCSS = Common.MakeSingleCSS(cssFile,"");
                    string xhtmlPreviewFilePath = Preview.CreatePreviewFile(xhtmlFile, _mergedCSS, "Template", true);
                    webTemplatePreview.Navigate(xhtmlPreviewFilePath);
                }
                else
                {
                    webTemplatePreview.Visible = false;
                }
            }
            catch { } 
        }

        private void NewPublication_Load(object sender, EventArgs e)
        {
            LocDB.Localize(this, null);     // Form Controls
            Common.HelpProv.SetHelpNavigator(this, HelpNavigator.Topic);
            Common.HelpProv.SetHelpKeyword(this, "NewPublication.htm");
            txtLocation.Text = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            // load CSS Templates
			_supportFolder = Common.FromRegistry("Template");
            lstSourceCSS.Items.Add("Blank");
        }

        private void btnXHTMLBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.FileName = "";
            openFile.Filter = "XHTML Files(*.xhtml)|*.xhtml|Lift files(*.lift)|*.lift";
            openFile.InitialDirectory = txtXHTML.Text;
            openFile.ShowDialog();
            txtXHTML.Text = openFile.FileName;
            txtXHTML.Focus();  
        }

        private void btnLocationBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowDialog();
            txtLocation.Text = folderDlg.SelectedPath;   
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtDicName.Text.Trim() == string.Empty)
            {
                LocDB.Message("errEnterDictName", "Please type Dictionary Name", null, LocDB.MessageTypes.Info, LocDB.MessageDefault.First);
                txtDicName.Focus();
            }
            else if (txtLocation.Text.Trim() == string.Empty)
            {
                LocDB.Message("errSelectDictLoc", "Please select Dictionary location", null, LocDB.MessageTypes.Info, LocDB.MessageDefault.First);
                txtDicName.Focus();
            }
            else if (!File.Exists(txtXHTML.Text))
            {
                LocDB.Message("errSelectXHTMLFile", "Please select XHTML filename with path", null, LocDB.MessageTypes.Info, LocDB.MessageDefault.First);
                txtXHTML.Focus();
            }
            else
            {
                if (lstSourceCSS.SelectedIndex > 0) // CSS File
                {
                    CSSTemplate = _mergedCSS;   
                }
             
                XhtmlFile = txtXHTML.Text;
                ProjectName = txtDicName.Text;
                DicPath = txtLocation.Text;
                if (!Directory.Exists(DicPath))
                {
                    DirectoryInfo di = Directory.CreateDirectory(DicPath);
                }
                Success = true;
                this.Close();
            }
        }
  
        private void txtLocation_TextChanged(object sender, EventArgs e)
        {
            string fileName;
            DicPath = txtLocation.Text;
            if (txtDicName.Text.Trim() != string.Empty)
            {
                fileName = txtDicName.Text.Trim();
                int numberIndex = fileName.IndexOfAny("0123456789".ToCharArray());
                if (numberIndex > 0)
                    fileName = fileName.Substring(0, numberIndex);
            }
        }

        private void txtDicName_KeyPress(object sender, KeyPressEventArgs e)
        {
            _automaticProjectName = false;
        }

        private void txtDicName_TextChanged(object sender, EventArgs e)
        {

        }

        private void NewPublication_DoubleClick(object sender, EventArgs e)
        {
#if DEBUG
            var dlg = new Localizer(LocDB.DB);
            dlg.ShowDialog();
#endif
        }

        private void NewProject_Activated(object sender, EventArgs e)
        {
            Common.SetFont(this);

        }
        #endregion

        private void txtXHTML_Validated(object sender, EventArgs e)
        {
            _projectType = Common.GetProjectType(txtXHTML.Text);
            if (_automaticProjectName)
            {
                txtDicName.Text = Common.GetNewFolderName(DicPath, _projectType);
            }
            lblName.Text = _projectType + " Name";
            lblLocation.Text = _projectType + " Location";
            this.Text = "New " + _projectType;
            LoadCSSTemplate();
        }

        private void LoadCSSTemplate()
        {
            lstSourceCSS.Items.Clear(); 
            string[] fileName = Directory.GetFiles(Common.PathCombine(_supportFolder, _projectType), "*.css");
            string selCSS;
            lstSourceCSS.Items.Add("Blank");
            foreach (string fName in fileName)
            {
                selCSS = fName.Substring(fName.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                lstSourceCSS.Items.Add(selCSS);
            }
        }
    }
    #endregion
}
