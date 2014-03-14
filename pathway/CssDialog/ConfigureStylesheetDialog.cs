// --------------------------------------------------------------------------------------------
// <copyright file="ConfigureStylesheetDialog.cs" from='2009' to='2014' company='SIL International'>
//      Copyright (C) 2014, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Implements Fieldworks Utility Interface for Pathway
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using SIL.Tool.Localization;

namespace SIL.PublishingSolution
{
    public partial class ConfigureStylesheetDialog : Form
    {
        private readonly string _buttonType = "Add";
        private readonly string _value = string.Empty;
        public string[] prevValues;
        public ConfigureStylesheetDialog(string buttonType, string value)
        {
            _buttonType = buttonType;
            _value = value;
            InitializeComponent();
        }

        private void BtCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog {FileName = "", Filter = "CSS Files(*.css)|*.css"};
            openFile.ShowDialog();
            TxtCSSFileName.Text = Path.GetFileName(openFile.FileName);
            TxtCSSFileName.Focus();
        }

        private void BtOk_Click(object sender, EventArgs e)
        {
            if(ValidateInput())
            {
                const string xPath = "styles/paper/style";
                ValidateSheetName(TxtStyleSheetName.Text, xPath);
                Close();

            }
        }

        private bool ValidateSheetName(string newName, string xPath)
        {
            List<string> styleList = Param.GetListofAttr(xPath, "name");
            if (styleList.Contains(newName) && _buttonType == "Add")
            {
                LocDB.Message("errFileAlreadyExitsNoOverwrite", "File Already Exists, Please give the new name.",
                              null, LocDB.MessageTypes.Info, LocDB.MessageDefault.First);
            }
            else
            {
                string filePath = Param.GetValidFilePath(TxtCSSFileName.Text, Path.GetFileName(TxtCSSFileName.Text));

                if (_buttonType == "Modify")
                {
                    Param.RemoveXMLNode(Param.SettingOutputPath,
                                        "/stylePick/styles/paper/style[@name='" + prevValues[0] + "']");
                }
                Param.SaveSheet(TxtStyleSheetName.Text, filePath, TxtDescription.Text);
            }
            return true;
        }

        private bool ValidateInput()
        {
            string filePath = Param.GetValidFilePath(TxtCSSFileName.Text, Path.GetFileName(TxtCSSFileName.Text));
            if(string.IsNullOrEmpty(TxtStyleSheetName.Text))
            {
                TxtStyleSheetName.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(TxtCSSFileName.Text))
            {
                TxtCSSFileName.Focus();
                return false;
            }
            if (!File.Exists(filePath))
            {
                var msg = new[] { "Css file path is invalid, Please give the valid input file" };
                LocDB.Message("defErrMsg", "Css file path is invalid, Please give the valid input file", msg, LocDB.MessageTypes.Warning, LocDB.MessageDefault.First);
                return false;
            }
            if (string.IsNullOrEmpty(TxtDescription.Text))
            {
                TxtDescription.Focus();
                return false;
            }
            if (_buttonType == "Modify")
            {
                if (prevValues[0] == TxtStyleSheetName.Text && prevValues[1] == TxtCSSFileName.Text && prevValues[2] == TxtCSSFileName.Text)
                {
                    return false;
                }
            }
            return true;
        }

        private void ConfigureStylesheetDialog_Load(object sender, EventArgs e)
        {
            if(_buttonType == "Modify")
            {
                TxtStyleSheetName.Text = _value;
                TxtCSSFileName.Text = Param.GetAttrByName("styles/paper/style", _value, "file");
                TxtDescription.Text = Param.GetElemByName("styles/paper/style", _value, "Description");
                prevValues = new[] { TxtStyleSheetName.Text, TxtCSSFileName.Text, TxtDescription.Text};
            }
            
        }

        private void TxtStyleSheetName_KeyPress(object sender, KeyPressEventArgs e)
        {
        }
    }
}
