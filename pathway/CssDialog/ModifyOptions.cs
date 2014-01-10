// --------------------------------------------------------------------------------------------
// <copyright file="FeatureEdit.cs" from='2009' to='2009' company='SIL International'>
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
// Css Feature Edit
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using JWTools;
using SIL.Tool;
using SIL.Tool.Localization;

namespace SIL.PublishingSolution
{
    public partial class ModifyOptions : Form
    {
        public ModifyOptions()
        {
            InitializeComponent();
        }

        private void ModifyOptions_Load(object sender, EventArgs e)
        {
            LocDB.Localize(this, null);
            Param.LoadFeatures("features/feature", TvFeatures, null);
            Param.LoadImages(LvIcon);
            Param.SetupHelp(this);
        }

        private void TvFeatures_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (TvFeatures.SelectedNode.Parent == null)
            {
                TbFeatureName.Text = TvFeatures.SelectedNode.Text;
                TbOptionName.Text = "";
                TbCssSnippet.Text = "";
                SelectIcon(null);
            }
            else
            {
                TbFeatureName.Text = TvFeatures.SelectedNode.Parent.Text;
                TbOptionName.Text = TvFeatures.SelectedNode.Text;
                var tag = (XmlNode) TvFeatures.SelectedNode.Tag;
                TbCssSnippet.Text = Param.GetTagFile(tag);
                SelectIcon(tag);
            }
            
        }

        private void BtClose_Click(object sender, EventArgs e)
        {
            DoApply();
            Close();
        }

        private void BtCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtApply_Click(object sender, EventArgs e)
        {
            DoApply();
        }

        private string GetBestName()
        {
            var fn = TbCssSnippet.Text;
            if (fn == "")
            {
                if (TbOptionName.Text == "")
                    return "";
                fn = TbFeatureName.Text + "_" + TbOptionName.Text + ".css";
            }
            return fn;
        }

        private void CreateCustomSnippet(string ifn)
        {
            if (File.Exists(ifn)) return;
            var mfn = Common.PathCombine(Param.Value[Param.MasterSheetPath], TbCssSnippet.Text);
            if (File.Exists(mfn))
            {
                File.Copy(mfn, ifn);
            }
            else
            {
                try
                {
                    string DirName = Path.GetDirectoryName(ifn);
                    string fileName = Common.PathCombine(DirName.Substring(DirName.LastIndexOf(Path.DirectorySeparatorChar) + 1), Path.GetFileName(ifn));
					string ExePath = Common.FromRegistry("Styles");
                    File.Copy(Common.PathCombine(ExePath, fileName), ifn);
                }
                catch (Exception ex)
                {

                }

            }
        }

        private void BtEdit_Click(object sender, EventArgs e)
        {
            var bfn = GetBestName();
            if (bfn == "") return;
            var upath = Param.Value[Param.UserSheetPath];
            var ifn = Common.PathCombine(upath, bfn);
            CreateCustomSnippet(ifn);
            var p = Process.Start(Param.Value[Param.CssEditor], ifn);
            p.WaitForExit();
            if (File.Exists(ifn))
                TbCssSnippet.Text = Path.GetFileName(ifn);

        }

        private void BtIcon_Click(object sender, EventArgs e)
        {
            if (TbCssSnippet.Text == "") return;
            var ipath = Param.Value[Param.IconPath];
            openIcon.DefaultExt = ".bmp";
            openIcon.FileName = "";
            openIcon.InitialDirectory = ipath;
            openIcon.Filter = "Image (*.png)|*.png";
            //openIcon.Filter = "Icon (*.ico)|*.ico|Bitmap (*.bmp)|*.bmp";
            openIcon.AddExtension = true;
            if (openIcon.ShowDialog() != DialogResult.OK) return;
            var rfn = openIcon.FileName;
            var fn = Path.GetFileName(rfn);
            if (Path.GetDirectoryName(rfn) != ipath)
                File.Copy(rfn, Common.PathCombine(ipath, fn));
            Param.NewIcon(fn, TbCssSnippet.Text);
            Param.LoadImages(LvIcon);
            foreach (ListViewItem li in LvIcon.Items)
                li.Selected = (li.Text == fn);
        }

        private void SelectIcon(XmlNode tag)
        {
            foreach (ListViewItem node in LvIcon.Items)
            {
                if (tag != null)
                {
                    var iconName = Param.GetTagIcon(tag);
                    node.Selected = (iconName == node.Text);
                    continue;
                }
                node.Selected = false;
            }
        }
        private void DoApply()
        {
            var feature = Param.InsertKind("feature", TbFeatureName.Text);
            var selectedIcon = LvIcon.SelectedItems;
            var iconName = selectedIcon.Count > 0 ? selectedIcon[0].Text : "";
            Param.InsertOption(feature, TbOptionName.Text, TbCssSnippet.Text, iconName);
            Param.Write();
            Param.LoadImageList();
            Param.LoadFeatures("features/feature", TvFeatures, null);
        }

        private void BtHelp_Click(object sender, EventArgs e)
        {
			SubProcess.Run(Common.FromRegistry("Help"), "SilDek.html", false);
        }

        private void ModifyOptions_DoubleClick(object sender, EventArgs e)
        {
#if DEBUG
            var dlg = new Localizer(LocDB.DB);
            dlg.ShowDialog();
#endif
        }

        private void ModifyOptions_Activated(object sender, EventArgs e)
        {
            Common.SetFont(this);
        }

        private void BtSnippet_Click(object sender, EventArgs e)
        {
            var bfn = GetBestName();
            if (bfn == "") return;
            var upath = Param.Value[Param.UserSheetPath];
            var ifn = Common.PathCombine(upath, bfn);
            CreateCustomSnippet(ifn);
            OpenSnippet.DefaultExt = ".css";
            OpenSnippet.FileName = bfn;
            OpenSnippet.InitialDirectory = upath;
            OpenSnippet.Filter = "Cascading Style Sheet (*.css)|*.css";
            OpenSnippet.AddExtension = true;

            if (OpenSnippet.ShowDialog() != DialogResult.OK) return;
            var fn = Path.GetFileName(OpenSnippet.FileName);
            TbCssSnippet.Text = fn;
            if (Path.GetDirectoryName(OpenSnippet.FileName) != upath)
                File.Copy(OpenSnippet.FileName, Common.PathCombine(upath, fn));
        }

        private void TbOptionName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit((e.KeyChar)))
            {
                e.Handled = true;
            } 
            if (e.KeyChar == '\b')
            {
                e.Handled = false;
            }

        }


    }
}
