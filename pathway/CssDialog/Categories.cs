// --------------------------------------------------------------------------------------------
// <copyright file="Categories.cs" from='2009' to='2009' company='SIL International'>
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
// Css Category
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Windows.Forms;
using JWTools;
using SIL.Tool;
using SIL.Tool.Localization;

namespace SIL.PublishingSolution
{
    public partial class Categories : Form
    {
        public Categories()
        {
            InitializeComponent();
        }

        private void Categories_Load(object sender, EventArgs e)
        {
            LocDB.Localize(this, null);
            Param.LoadCategories("categories/category", TvCategories);
            BtSettings.Visible = Param.UserRole == "System Designer";
            Param.SetupHelp(this);
        }

        private void BtSave_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Param.SaveCategories(TvCategories);
            Close();
        }

        private void BtCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtSettings_Click(object sender, EventArgs e)
        {
            var dlg = new GlobalSettings();
            dlg.ShowDialog();
        }

        private void TvCategories_AfterCheck(object sender, TreeViewEventArgs e)
        {
            Param.Check(e.Node);
        }

        private void Categories_DoubleClick(object sender, EventArgs e)
        {
            var dlg = new Localizer(LocDB.DB);
            dlg.ShowDialog();
        }

        private void Categories_Activated(object sender, EventArgs e)
        {
            Common.SetFont(this);
        }
    }
}
