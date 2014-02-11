// --------------------------------------------------------------------------------------------
// <copyright file="TaskStyles.cs" from='2009' to='2009' company='SIL International'>
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
// 
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using JWTools;
using SIL.Tool;
using SIL.Tool.Localization;

namespace SIL.PublishingSolution
{
    public partial class ConfigureTasks : Form
    {
        public string Task { get; set; }

        public string ProjectName { get; set; }

        public ConfigureTasks()
        {
            InitializeComponent();
        }

        private void ConfigureTasks_Load(object sender, EventArgs e)
        {
            LocDB.Localize(this, null);
            Param.SetupHelp(this);
            SetTasks();
            LsTasks.Text = Task;
            SetStyles();
            LsStyles.Text = Param.TaskSheet(Task);
            SetDescription();
            SetDynamic();
        }

        private void SetTasks()
        {
            LsTasks.Items.Clear();
            LsTasks.Items.AddRange(Param.GetListofAttr("tasks/task", "name").ToArray());
        }

        private void BtSave_Click(object sender, EventArgs e)
        {
            DoApply();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void BtCategories_Click(object sender, EventArgs e)
        {
            var dlg = new Categories();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                SetStyles();
                SetDynamic();
                LsStyles.SelectedIndex = LsStyles.Items.Count - 1;
            }
        }

        private void BtPreview_Click(object sender, EventArgs e)
        {
            if(!CheckTaskSelected()) return;
            var dlg = new Preview { Sheet = Param.TaskSheet(LsTasks.Text), ParentForm = this };
            dlg.Show();
        }

        private bool CheckTaskSelected()
        {
            if(LsTasks.SelectedIndex < 0)
            {
                if (LsTasks.Items.Count >= 1)
                {
                    LsTasks.SelectedIndex = LsTasks.Items.Count - 1; ;
                }
                else
                {
                    string errMsg = "Please select the Task in the List";
                    var msg = new[] {errMsg};
                    LocDB.Message("defErrMsg", errMsg, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                    return false;
                }
            }
            return true;
        }

        private bool CheckStyleSelected()
        {
            if (LsStyles.SelectedIndex < 0)
            {
                if (LsStyles.Items.Count >= 1)
                {
                    LsStyles.SelectedIndex = LsStyles.Items.Count - 1; ;
                }
                else
                {
                    string errMsg = "Please select the Style in the List";
                    var msg = new[] {errMsg};
                    LocDB.Message("defErrMsg", errMsg, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                    return false;
                }
            }
            return true;
        }

        private void BtFeatures_Click(object sender, EventArgs e)
        {
            if (!CheckStyleSelected()) return;
            var dlg = new ConfigureStylesheet { StyleSheet = LsStyles.Text, ProjectName = this.ProjectName};
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LsStyles.Items.Clear();
                LsStyles.Items.AddRange(Param.GetListofAttr("styles/paper/style", "name").ToArray());
                LsStyles.Text = dlg.StyleSheet;
            }
        }

        private void BtApply_Click(object sender, EventArgs e)
        {
            DoApply();
        }

        private void LsTasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            Task = LsTasks.Text;
            LsStyles.ClearSelected();
            var sheet = Param.TaskSheet(Task);
            if (LsStyles.Items.Contains(sheet))
            {
                LsStyles.Text = sheet;
                SetDescription();
            }
        }

        private void LsStyles_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetDescription();
        }

        private void DoApply()
        {
            if (!CheckStyleSelected() || !CheckTaskSelected()) return;

            var task = Param.InsertKind("task", LsTasks.Text);
            Param.SetAttrValue(task, "style", LsStyles.Text);
            Param.Write();
        }

        private void SetDynamic()
        {
            LbSummary.Text = "Select a publication task and then the stylesheet it should use.";
            BtConfigure.Visible = (Param.UserRole == "Consultant" || Param.UserRole == "System Designer");
            BtAdvanced.Visible = BtConfigure.Visible;
        }

        private void SetStyles()
        {
            LsStyles.Items.Clear();
            var styleList = Param.GetListofAttr("styles/paper/style", "name");
            LsStyles.Items.AddRange(Param.FilterStyles(styleList).ToArray());
        }

        private void SetDescription()
        {
            RtDescription.Text = "";
            var sheet = LsStyles.Text;
            if (LsStyles.Items.Contains(sheet))
                RtDescription.Text = Param.GetElemByName("styles/paper/style", sheet, "Description");
        }

        private void ConfigureTasks_DoubleClick(object sender, EventArgs e)
        {
#if DEBUG
            var dlg = new Localizer(LocDB.DB);
            dlg.ShowDialog();
#endif
        }

        private void ConfigureTasks_Activated(object sender, EventArgs e)
        {
            Common.SetFont(this);
        }

        private void BtAdvanced_Click(object sender, EventArgs e)
        {
            if (!CheckStyleSelected()) return;
            var mergeCss = new MergeCss();
            var myCss = mergeCss.Make(Param.StylePath(LsStyles.Text), "Temp1.css");
            var newCss = SettingBl.GetNewFileName(LsStyles.Text, "job", Param.Value[Param.InputPath], "onsave");
            File.Copy(myCss, newCss, true);
            File.Copy(myCss, Common.PathCombine(Param.Value[Param.OutputPath], Path.GetFileName(newCss)), true);
            File.Delete(myCss);

            var projectFullName = Common.PathCombine(Param.Value[Param.InputPath], ProjectName + ".de");
            var plugIn = !File.Exists(projectFullName);
            string cssResult;
            if (Param.Value[Param.InputType] == "Scripture")
            {
                var dlg = new ScriptureSetting(
                    Param.Value[Param.InputPath],
                    newCss,
                    plugIn,
                    Param.Value[Param.CurrentInput],
                    plugIn ? "" : projectFullName);
                dlg.AddFileToXML(newCss, false.ToString());
                if (dlg.ShowDialog() != DialogResult.OK) return;
                cssResult = dlg.JobName;
            }
            else
            {
                var dlg = new DictionarySetting(
                    Param.Value[Param.InputPath],
                    newCss,
                    plugIn,
                    Param.Value[Param.CurrentInput],
                    plugIn ? "" : projectFullName);
                dlg.AddFileToXML(newCss, false.ToString());
                if (dlg.ShowDialog() != DialogResult.OK) return;
                cssResult = dlg.JobName;
            }
        }

        private void BtTaskAdd_Click(object sender, EventArgs e)
        {
            string currTask = string.Empty;
            if (!CheckTaskSelected()) return;
            var cdialog = new ConfigureTaskDialog("Add", LsTasks.SelectedItem.ToString());
            cdialog.ShowDialog();
            SetTasks();
            if (cdialog.Task != null)
            {
                currTask = cdialog.Task;
            }
            LsTasks.Text = currTask;
        }

        private void BtStylesheetAdd_Click(object sender, EventArgs e)
        {
            if (!CheckStyleSelected()) return;

            var csheetdialog = new ConfigureStylesheetDialog("Add", LsStyles.SelectedItem.ToString());
            csheetdialog.ShowDialog();
            SetStyles();
            LsStyles.SelectedIndex = LsStyles.Items.Count - 1;
        }

        private void BtTaskModify_Click(object sender, EventArgs e)
        {
            if (!CheckTaskSelected()) return;
            int currIndex = LsTasks.SelectedIndex;
            var cdialog = new ConfigureTaskDialog("Modify", LsTasks.SelectedItem.ToString());
            cdialog.ShowDialog();
            SetTasks();
            LsTasks.SelectedIndex = currIndex;
        }

        private void BtStylesheetModify_Click(object sender, EventArgs e)
        {
            if (!CheckStyleSelected()) return;

            int currIndex = LsStyles.SelectedIndex;
            var csheetdialog = new ConfigureStylesheetDialog("Modify", LsStyles.SelectedItem.ToString());
            csheetdialog.ShowDialog();
            SetStyles();
            LsStyles.SelectedIndex = currIndex;
        }

        private void BtTaskDelete_Click(object sender, EventArgs e)
        {
            if (!ValidateInputText(LsTasks))
            {
                return;
            }

            var msg = new[] { LsTasks.SelectedItem.ToString() };
            DialogResult result = LocDB.Message("msgConfirmFileDeletion", "Are you sure you want to delete the task : " + LsTasks.SelectedItem,
                              msg, LocDB.MessageTypes.WarningYN, LocDB.MessageDefault.First);
            if (result == DialogResult.No)
                return;
                Param.RemoveXMLNode(Param.SettingOutputPath,
                                    "/stylePick/tasks/task[@name='" + LsTasks.SelectedItem + "']");
                LsTasks.SelectedItem = LsTasks.SelectedIndex - 1;
                LsTasks.Items.Remove(LsTasks.SelectedItem);
                LsTasks.SelectedIndex = LsTasks.Items.Count - 1;
            
        }

        private void BtStylesheetDelete_Click(object sender, EventArgs e)
        {
            if (!ValidateInputText(LsStyles))
            {
                return;
            }

            var msg = new[] { LsStyles.SelectedItem.ToString() };
            DialogResult result = LocDB.Message("msgConfirmFileDeletion", "Are you sure you want to delete the stylesheet : " + LsStyles.SelectedItem,
                              msg, LocDB.MessageTypes.WarningYN, LocDB.MessageDefault.First);
            if (result == DialogResult.No)
                return;
                 Param.RemoveXMLNode(Param.SettingOutputPath,
                                     "/stylePick/styles/paper/style[@name='" + LsStyles.SelectedItem + "']");
                 LsStyles.Items.Remove(LsStyles.SelectedItem);
                 LsStyles.SelectedIndex = LsStyles.Items.Count - 1;
             
        }

        private static bool ValidateInputText(ListBox lstBox)
        {
            const string message = "Tasklist or Sheetlist cannot be empty.";
            var msgTask = new[] { message };
            if (lstBox.Items.Count == 1)
            {
                LocDB.Message("defErrMsg", message,
                              msgTask, LocDB.MessageTypes.Info, LocDB.MessageDefault.First);
                return false;
            }
            return true;
        }
    }
}
