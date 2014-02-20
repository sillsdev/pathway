// --------------------------------------------------------------------------------------------
// <copyright file="ConfigureStylesheet.cs" from='2009' to='2014' company='SIL International'>
//      Copyright (C) 2009, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// 
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using JWTools;
using SIL.Tool;
using SIL.Tool.Localization;

namespace SIL.PublishingSolution
{
    public partial class ConfigureStylesheet : Form
    {
        #region StyleSheet

       
        private string styleSheet;
        private string existingCssFile;
        public string StyleSheet
        {
            get
            {
                return LbStyleSheetName.Text;
            }
            set
            {
                LbStyleSheetName.Text = styleSheet = value;
            }
        }
        #endregion StyleSheet

        private FeatureSheet featureSheet;

        public string ProjectName;

        public ConfigureStylesheet()
        {
            InitializeComponent();
        }


        private void ConfigureStylesheet_Load(object sender, EventArgs e)
        {
            LocDB.Localize(this, null);
            Param.SetupHelp(this);
            LbStyleSheetName.Text = styleSheet;
            existingCssFile = styleSheet;
            Debug.Assert(LbStyleSheetName.Text != "", "No Stylesheet given!");
            LbSummary.Text = Param.GetAttrSummary("categories/category", "select");
            LbStyleDescription.Text = Param.GetElemByName("styles/paper/style", LbStyleSheetName.Text, "Description");
            featureSheet = new FeatureSheet(Param.StylePath(LbStyleSheetName.Text));
            TvFeatures.Enabled = featureSheet.ReadToEnd();
            Param.LoadFeatures("features/feature", TvFeatures, TvFeatures.Enabled ? featureSheet.Features : null);
            BtModifyOptions.Visible = Param.UserRole == "System Designer";
        }

        private void BtSave_Click(object sender, EventArgs e)
        {
            bool dlgOk = SaveCancel();
            if (dlgOk == false)
            {
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private bool SaveCancel()
        {
            var currentSheet = Param.GetListofAttr("styles/paper/style", "name");
            string filePath = string.Empty;
            if (currentSheet.Contains(LbStyleSheetName.Text))
            {
                var msg = new[] { LbStyleSheetName.Text };
                var result = LocDB.Message("errReplaceStyleSheet", "Replace " + LbStyleSheetName.Text + " style sheet?", msg, LocDB.MessageTypes.WarningYNC,
                LocDB.MessageDefault.First);
                if (result == DialogResult.Cancel)
                {
                    return false;
                }
                if (result == DialogResult.No)
                {
                    var msgs = new string[] { "Update sheet name" };
                    LocDB.Message("defErrMsg", "Update sheet name", msgs, LocDB.MessageTypes.Info,
                    LocDB.MessageDefault.First);
                    return false;
                }

                string fileName = Param.GetAttrByName("styles/paper/style", LbStyleSheetName.Text, "file");
                Param.RemoveXMLNode(Param.SettingOutputPath,
                                        "/stylePick/styles/paper/style[@name='" + LbStyleSheetName.Text + "']");

                filePath = Param.GetValidFilePath(filePath, fileName);
            }

            if (File.Exists(filePath))
                Param.SaveSheet(LbStyleSheetName.Text, filePath, LbStyleDescription.Text);

            if (TvFeatures.Enabled)
            {
                featureSheet.SaveFeatures(TvFeatures);
                featureSheet.Sheet = Param.StylePath(LbStyleSheetName.Text, FileAccess.Write);
                featureSheet.Write();
            }
            else
            {
                string destPath = Param.StylePath(styleSheet, FileAccess.Write);
                if(styleSheet.Length == 0 ||destPath.Length == 0)
                return false;

                if (File.Exists(destPath))
                    return true;
                File.Copy(Common.PathCombine(Param.Value[Param.InputPath], styleSheet + ".css"), destPath);
            }
            return true;
        }

        

        private void BtCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void BtPreview_Click(object sender, EventArgs e)
        {
            featureSheet.SaveFeatures(TvFeatures);
            featureSheet.Sheet = Param.StylePath("StyleFeaturesTemp.css", FileAccess.Write);
            featureSheet.Write();
            var dlg = new Preview { Sheet = featureSheet.Sheet, ParentForm = this };
            dlg.Show();
            File.Delete(featureSheet.Sheet); 
        }

        private void BtEdit_Click(object sender, EventArgs e)
        {
            var dlg = new ModifyOptions();
            dlg.ShowDialog();
        }

        private void BtCategories_Click(object sender, EventArgs e)
        {
            var dlg = new StyleCategories();
            dlg.ShowDialog();
        }

        private void TvFeatures_AfterCheck(object sender, TreeViewEventArgs e)
        {
            Param.Check(e.Node);
        }

        private void ConfigureStylesheet_DoubleClick(object sender, EventArgs e)
        {
#if DEBUG
            var dlg = new Localizer(LocDB.DB);
            dlg.ShowDialog();
#endif
        }

        private void ConfigureStylesheet_Activated(object sender, EventArgs e)
        {
            Common.SetFont(this);
        }

        private void TvFeatures_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Param.Check(e.Node);
        }
    }
}
