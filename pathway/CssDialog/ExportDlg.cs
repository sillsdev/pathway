// --------------------------------------------------------------------------------------------
// <copyright file="ExportDlg.cs" from='2009' to='2009' company='SIL International'>
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
// Export Dialog for Output Types
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Windows.Forms;
using System.Collections;
using JWTools;
using SIL.Tool;
using SIL.Tool.Localization;

namespace SIL.PublishingSolution
{
    #region Class ExportDlg
    public partial class ExportDlg : Form
    {
        public string ExportType { get; set; }

        #region Constructor
        public ExportDlg()
        {
            InitializeComponent();
        }
        #endregion

        #region private function
        private void ExportDlg_Load(object sender, EventArgs e)
        {
            LocDB.Localize(this, null);     // Form Controls
            Common.HelpProv.SetHelpNavigator(this, HelpNavigator.Topic);
            Common.HelpProv.SetHelpKeyword(this, "Exporting.htm");
            ArrayList exportType = Backend.GetExportType(ExportType);
            if (exportType.Count > 0)
            {
                foreach (string item in exportType)
                {
                    cmbExportType.Items.Add(item);
                }
                cmbExportType.SelectedIndex = 0;
            }
            else
            {
                var msg = new[] { "Please Install the Plugin Backends" };
                LocDB.Message("defErrMsg", "Please Install the Plugin Backends", msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                this.Close();
            }
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            ExportType = cmbExportType.SelectedItem.ToString();
            DialogResult = DialogResult.OK;
            Close(); 
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ExportType = string.Empty;
            DialogResult = DialogResult.Cancel;
            Close();
        }
        #endregion

        private void ExportDlg_DoubleClick(object sender, EventArgs e)
        {
#if DEBUG
            var dlg = new Localizer(LocDB.DB);
            dlg.ShowDialog();
#endif
        }

        private void ExportDlg_Activated(object sender, EventArgs e)
        {
            Common.SetFont(this);
        }
    }
    #endregion

}
