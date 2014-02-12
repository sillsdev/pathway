// --------------------------------------------------------------------------------------------
// <copyright file="GlobalSettings.cs" from='2009' to='2014' company='SIL International'>
//      Copyright © 2014, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// GlobalSettings
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using JWTools;
using SIL.Tool;
using SIL.Tool.Localization;

namespace SIL.PublishingSolution
{
    public partial class GlobalSettings : Form
    {
        #region private variables

        readonly ErrorProvider _errProvider = new ErrorProvider();
        #endregion

        public GlobalSettings()
        {
            InitializeComponent();
            var count = Param.Value.Count;
            TlSettings.Height = 27 * count;
            Height = TlSettings.Height + 100;
            BtOk.Location = new Point(BtOk.Location.X, Height - 70);
            BtCancel.Location = new Point(BtCancel.Location.X, Height - 70);
            BtReset.Location = new Point(BtReset.Location.X, Height - 70);
            TlSettings.RowCount = count;
            TlSettings.ColumnCount = 2;
            TlSettings.ColumnStyles.Clear();
            TlSettings.ColumnStyles.Add(new ColumnStyle { Width = 100 });
            TlSettings.ColumnStyles.Add(new ColumnStyle { Width = 250 });
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            LocDB.Localize(this, null);
            DoLoad();
            Param.SetupHelp(this);
        }

        private void DoLoad()
        {
            TlSettings.RowStyles.Clear();
            foreach (var v in Param.Value)
            {
                TlSettings.RowStyles.Add(new RowStyle { Height = 20 });
                var lb = new Label { Text = v.Key, Width = 100 };
                TlSettings.Controls.Add(lb);
                ArrayList comboControl = new ArrayList();
                comboControl.Add("PreviewType");
                comboControl.Add("InputType");
                comboControl.Add("LastTask");
                if (comboControl.Contains(v.Key))
                {
                    ComboBox tb = new ComboBox();
                    object[] prevType;
                    if(v.Key == "PreviewType")
                    {
                        prevType = new[] { "Prince", "Open Office" };
                        tb.Items.AddRange(prevType);
                    }
                    else if (v.Key == "InputType")
                    {
                        prevType = new[] { "Dictionary", "Scripture" };
                        tb.Items.AddRange(prevType);
                    }
                    else if (v.Key == "LastTask")
                    {
                        List<string> tasks = Param.GetListofAttr("tasks/task", "name");
                        foreach (string task in tasks)
                        {
                            tb.Items.Add(task);
                        }
                    }
                    tb.Text = v.Value;
                    tb.Size = new Size(121, 21);
                    tb.DropDownStyle = ComboBoxStyle.DropDownList;
                    tb.Name = v.Key;
                    TlSettings.Controls.Add(tb);
                }
                else
                {
                    string val = Common.DirectoryPathReplace(v.Value);
                    var tb = new TextBox { Text = val, Width = 340, Name = v.Key };
                    tb.Leave += new System.EventHandler(this.tb_Leave);
                TlSettings.Controls.Add(tb);
            }
        }
        }

        private void BtOk_Click(object sender, EventArgs e)
        {
            if (ValidateInputs())
            {
            Param.SaveSettings(TlSettings);
            Close();
            }
        }

        private void tb_Leave(object sender, EventArgs e)
        {
            string fieldName = ((Control) sender).Name;
            string fieldValue = ((Control)sender).Text;
			string relativePath = Common.ProgBase;
            string errorMessage = string.Empty;
            if (fieldName == "InputPath" || fieldName == "OutputPath" || fieldName == "UserSheetPath")
            {
                if (fieldValue.Length == 0)
                {
                    errorMessage = "Please enter the " + fieldName + " value";
                }
                else if (!Directory.Exists(fieldValue))
                {
                    errorMessage = "Path name given for " + fieldName + " is not valid. Please select the valid path";
                }
            }
            else if (fieldName == "MasterSheetPath" || fieldName == "IconPath" || fieldName == "SamplePath")
            {
                string fieldNewValue = Common.PathCombine(relativePath, fieldValue);
                if (fieldValue.Length == 0)
                {
                    errorMessage = "Please enter the " + fieldName + " value";
                }
                else if (!Directory.Exists(fieldNewValue))
                {
                     errorMessage = "Path name given for " + fieldName + " is not valid. Please give the valid path";
                }
            }
            else if(fieldName == "BaseStyles")
            {
                string  masterSheetPath = Common.PathCombine(relativePath, TlSettings.Controls["MasterSheetPath"].Text);
                string fieldNewValue = Common.PathCombine(masterSheetPath, fieldValue);
                string fileType = Path.GetFileName(fieldNewValue);
                if (fieldValue.Length == 0)
                {
                    errorMessage = "Please enter the " + fieldName + " file.";
                }
                else if (!File.Exists(fieldNewValue) || fileType.IndexOf(".css") == -1)
                {
                     errorMessage = "Path name given for " + fieldName + " is not exist. Please give CSS image file.";
                }
            }
            else if (fieldName == "DefaultIcon" || fieldName == "SelectedIcon" || fieldName == "MissingIcon")
            {
                string fieldNewValue = Common.PathCombine(relativePath, fieldValue);
                fieldNewValue = Common.DirectoryPathReplaceWithSlash(fieldNewValue);
                string fileType = Path.GetFileName(fieldNewValue);
                if (fieldValue.Length == 0)
                {
                    errorMessage = "Please enter the " + fieldName + " icon.";
                }
                else if (!File.Exists(fieldNewValue) || fileType.IndexOf(".png") == -1)
                {
                     errorMessage = "Path name given for " + fieldName + " is not valid. Please give PNG image file.";
                }
            }
            else if (fieldName == "Help")
            {
                string fieldNewValue = Common.PathCombine(relativePath + "\\Help\\", fieldValue);
                string fileType = Path.GetFileName(fieldValue);
                if (fieldValue.Length == 0)
                {
                    errorMessage = "Please enter the " + fieldName + " file";
                }
                else if (!File.Exists(fieldNewValue) || !(fileType.IndexOf(".chm") > 0 || fileType.IndexOf(".htm") > 0))
                {
                     errorMessage = "Path name given for " + fieldName + " is not valid. Please give .chm or .htm help file.";
                }
            }
            else if (fieldName == "CurrentInput")
            {
                string fileType = Path.GetFileName(fieldValue);
                if (fieldValue.Length == 0)
                {
                    errorMessage = "Please enter the " + fieldName + " file.";
                }
                else if (!File.Exists(fieldValue) || !(fileType.IndexOf(".xhtml") > 0 || fileType.IndexOf(".lift") > 0))
                {
                     errorMessage = "Path name given for " + fieldName + " is not valid. Please give XHTML or LIFT file.";
                }
            }
            _errProvider.SetError((Control)sender, errorMessage);
        }

        public bool ValidateInputs()
        {
            foreach (Control ctl in TlSettings.Controls)
            {
                if (_errProvider.GetError(ctl) != "")
                {
                    ctl.Focus();
                    return false;
                }
            }
            return true;
        }

        private void BtCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Settings_DoubleClick(object sender, EventArgs e)
        {
#if DEBUG
            var dlg = new Localizer(LocDB.DB);
            dlg.ShowDialog();
#endif
        }

        private void GlobalSettings_Activated(object sender, EventArgs e)
        {
            Common.SetFont(this);
        }
    }
}
