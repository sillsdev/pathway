// --------------------------------------------------------------------------------------------
// <copyright file="Template.cs" from='2009' to='2009' company='SIL International'>
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
// Creates the Templates Css files
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Windows.Forms;
using JWTools;
using SIL.Tool;
using SIL.Tool.Localization;

namespace SIL.PublishingSolution
{
    #region Class Template
    public partial class Template : Form
    {
        #region Constructor
        public Template()
        {
            InitializeComponent();
        }
        public Template(string projectType)
        {
            InitializeComponent();
            this._projectType = projectType;
        }
        #endregion

        #region private variable
        private string _supportFolder;
        private string _mergedCSS;
        private string _projectType;

        #endregion

        #region public variable
        public string CssTemplate;
        #endregion

        #region Events
        private void lstSourceCSS_SelectedIndexChanged(object sender, EventArgs e)
        {
            string XHTMLFile = _supportFolder + "/Template.xhtml";
            try
            {
                if (lstSourceCSS.SelectedIndex > -1)
                {
                    webTemplatePreview.Visible = true;
                    string CSSFile = Common.PathCombine(_supportFolder, lstSourceCSS.Text);

                    //Library lib = new Library(); 
                    _mergedCSS = Common.MakeSingleCSS(CSSFile,"");
                    string xhtmlPreviewFilePath = Preview.CreatePreviewFile(XHTMLFile, _mergedCSS, "Template", true);
                    webTemplatePreview.Navigate(xhtmlPreviewFilePath);
                }
                else
                {
                    webTemplatePreview.Visible = false;
                }
            }
            catch { }
        }

        private void Template_Load(object sender, EventArgs e)
        {
            LocDB.Localize(this, null);     // Form Controls
			_supportFolder = Common.FromRegistry(@"Template/" + _projectType);
            string[] fileName = Directory.GetFiles(_supportFolder, "*.css");
            foreach (string fName in fileName)
            {
                string selCSS = fName.Substring(fName.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                lstSourceCSS.Items.Add(selCSS);
            }
            if (lstSourceCSS.Items.Count > 0)
            {
                lstSourceCSS.SelectedIndex = 0;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (lstSourceCSS.SelectedIndex > -1)
            {
                CssTemplate = _mergedCSS;
            }
            Close();
        }
        #endregion

        private void Template_DoubleClick(object sender, EventArgs e)
        {
#if DEBUG
            var dlg = new Localizer(LocDB.DB);
            dlg.ShowDialog();
#endif
        }

        private void Template_Activated(object sender, EventArgs e)
        {
            Common.SetFont(this);
        }
    }
    #endregion
}
