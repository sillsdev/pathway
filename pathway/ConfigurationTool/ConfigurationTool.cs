﻿// --------------------------------------------------------------------------------------------
// <copyright file="ConfiguraionTool.cs" from='2010' to='2011' company='SIL International'>
//      Copyright © 2010, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// Last reviewed: 
// 
// <remarks>
// Creates the Configuration Tool for Dictionary and Scripture 
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public partial class ConfigurationTool : Form
    {
        #region Private Variables
        private readonly ArrayList _cssNames = new ArrayList();
        //private string _fileName = string.Empty;
        private static ConfigurationToolBL _configurationToolBL = new ConfigurationToolBL();
        //private TextWriter _writeCss;
        ErrorProvider _errProvider = new ErrorProvider();
        DictionarySetting _ds = new DictionarySetting();
        //        DataSet _dataSet = new DataSet();
        UndoRedo _redoundo;
        private bool _isCreatePreview = false;
        private string _caption = _configurationToolBL.Caption;
        private string _redoUndoBufferValue = string.Empty;
        // Undo Redo
        //private string _currentControl = string.Empty;
        //private string _previousControl = string.Empty;
        // Undo Redo

        //private string _previousStyleName = string.Empty;
        private Color _selectedColor = SystemColors.InactiveBorder; //Color.FromArgb(255, 204, 102);
        private Color _deSelectedColor = SystemColors.Control;
        private string _styleName;
        private string _fileProduce = "One";
        private readonly string InputType;
        private readonly string MediaTypeEXE;
        private readonly string StyleEXE;
        private string _lastSelectedLayout = string.Empty;
        //private string _configurationToolBL.StyleName;
        private string _selectedStyle;
        TabPage tabdic = new TabPage();
        TabPage tabmob = new TabPage();
        private TraceSwitch _traceOn = new TraceSwitch("General", "Trace level for application");
        #endregion

        #region Constructor
        public ConfigurationTool(string inputType, string mediaType, string style)
        {
            Trace.WriteLineIf(_traceOn.Level == TraceLevel.Verbose, "ConfigurationTool Constructor");
            InitializeComponent();
            InputType = inputType;
            MediaTypeEXE = mediaType;
            StyleEXE = style.Replace('&', ' ');
        }
        #endregion

        #region Control Events
        private void ConfigurationTool_Load(object sender, EventArgs e)
        {
            Trace.WriteLineIf(_traceOn.Level == TraceLevel.Verbose, "ConfigurationTool_Load");
            //try
            //{
                tabdic = tabControl1.TabPages[1];
                tabmob = tabControl1.TabPages[2];
                tabControl1.TabPages.Remove(tabControl1.TabPages[2]);
                btnMobile.Enabled = false;
                _redoundo = new UndoRedo(tsUndo, tsRedo);
                MinimumSize = new Size(497, 183);
                LoadSettings();
                SetInputTypeButton();
                _configurationToolBL.CreateGridColumn();
                _configurationToolBL.LoadParam(); // Load DictionaryStyleSettings / ScriptureStyleSettings
                ShowDataInGrid();
                _redoundo.Reset();

                _configurationToolBL.SetPreviousLayoutSelect(stylesGrid);
                PopulateFeatureSheet(); //For TD-1194 // Load Default Values
                SetInfoTabValue();
                SetMediaType();
                if (!File.Exists(Common.FromRegistry("ScriptureStyleSettings.xml")))
                {
                    btnScripture.Enabled = false;
                    btnScripture.Visible = false;
                    btnDictionary.Visible = false;
                }
                SetFocusToName();

                //For the task TD-1481
                btnWeb.Enabled = false;
                btnOthers.Enabled = false;
            //}
            //catch(Exception ex)
            //{
            //    string ss = ex.Message;
            //}
        }

        /// <summary>
        /// When the condigurationtool is run from EXE, the mediatype has changed 
        /// by Prinvia dialog selection
        /// </summary>
        private void SetMediaType()
        {
            Trace.WriteLineIf(_traceOn.Level == TraceLevel.Verbose, "ConfigurationTool: SetMediaType");
            try
            {
                if (MediaTypeEXE.Length != 0)
                {
                    _configurationToolBL.MediaType = MediaTypeEXE.ToLower();
                    SideBar();
                    _configurationToolBL.SelectRow(stylesGrid, StyleEXE);
                }
                else
                {
                    SetSideBar();
                }
            }
            catch { }
        }

        /// <summary>
        /// When the condigurationtool is run from EXE, the load settings are changed.
        /// </summary>
        private void LoadSettings()
        {
            Trace.WriteLineIf(_traceOn.Level == TraceLevel.Verbose, "ConfigurationTool: LoadSettings");
            //Note: Configuration tool can run by two ways
            //Note: 1 -  Standalone Application
            //Note: 2 -  The ConfigurationTool.EXE is called by PrintVia dialog from FLEX.);
            if (InputType.Length == 0)
            {
                //It will call when the Configtool from Application
                RemoveSettingsFile();
                SettingsValidation(Param.SettingPath);
                Param.LoadSettings(); // Load StyleSetting.xml
                _configurationToolBL.InputType = Param.Value["InputType"];
            }
            else
            {
                //It will call when the Configtool from exe
                Param.LoadSettings(); // Load StyleSetting.xml
                _configurationToolBL.InputType = InputType;
                Param.SetLoadType = InputType;
            }
        }

        private static void RemoveSettingsFile()
        {
            string allUsersPath = Common.GetAllUserPath();
            if (ModifierKeys == Keys.Shift)
            {
                if (Directory.Exists(allUsersPath))
                {
                    Directory.Delete(allUsersPath, true);
                }
            }
        }


        #region SettingsValidation
        private void SettingsValidation(string filePath)
        {
            var versionControl = new SettingsVersionControl();
            var Validator = new SettingsValidator();
            if (File.Exists(filePath))
            {
                versionControl.UpdateSettingsFile(filePath);
                bool isValid = Validator.ValidateSettingsFile(filePath, false);
                if (!isValid)
                {
                    _lastSelectedLayout = Param.Value["LayoutSelected"];
                    this.Close();
                }
            }
        }
        #endregion

        private void SetInputTypeButton()
        {
            Trace.WriteLineIf(_traceOn.Level == TraceLevel.Verbose, "ConfigurationTool: SetInputTypeButton");
            if (_configurationToolBL.InputType.ToLower() == "scripture")
            {
                btnScripture.BackColor = _selectedColor;
                btnDictionary.BackColor = _deSelectedColor;
                btnScripture.Focus();
                lblSenseLayout.Visible = false;
                ddlSense.Visible = false;
            }
            else
            {
                btnDictionary.BackColor = _selectedColor;
                btnScripture.BackColor = _deSelectedColor;
                btnDictionary.Focus();
                lblSenseLayout.Visible = true;
                ddlSense.Visible = true;
            }
        }

        private void tsDelete_Click(object sender, EventArgs e)
        {
            _redoundo.Reset();
            string name = lblInfoCaption.Text;
            string msg = "Are you sure you want to delete the " + name + " stylesheet?";
            string caption = "Delete Stylesheet";
            DialogResult result = MessageBox.Show(msg, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (result != DialogResult.OK) return;

            try
            {
                //_redoundo.Set(Common.Action.Delete, _configurationToolBL.StyleName, null, "", string.Empty);
                if (_configurationToolBL.SelectedRowIndex >= 0)
                {
                    string selectedTypeValue = stylesGrid[_configurationToolBL.ColumnType, _configurationToolBL.SelectedRowIndex].Value.ToString();
                    if (selectedTypeValue != _configurationToolBL.TypeStandard)
                    {
                        _cssNames.Remove(_configurationToolBL.StyleName);
                        _configurationToolBL.RemoveXMLNode(_configurationToolBL.StyleName);
                        stylesGrid.Rows.RemoveAt(stylesGrid.Rows.GetFirstRow(DataGridViewElementStates.Selected));
                        if (_configurationToolBL.SelectedRowIndex == stylesGrid.Rows.Count) // Is last row?
                            _configurationToolBL.SelectedRowIndex = _configurationToolBL.SelectedRowIndex - 1;
                        stylesGrid.Rows[_configurationToolBL.SelectedRowIndex].Selected = true;
                        //_configurationToolBL.SelectedRowIndex--;
                        SetInfoTabValue();

                    }
                    else
                    {
                        MessageBox.Show("Factory style sheet can not be deleted", _caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Please select a style sheet to delete", _caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch { }
            _configurationToolBL.PreviousStyleName =
                stylesGrid.Rows[_configurationToolBL.SelectedRowIndex].Cells[0].Value.ToString();
        }

        private void tsSend_Click(object sender, EventArgs e)
        {
            string tempfolder = Path.GetTempPath();
            string folderName = Path.GetFileNameWithoutExtension(Path.GetTempFileName());
            string folderPath = Path.Combine(tempfolder, folderName);
            bool directoryCreated = _configurationToolBL.CopyCustomStyleToSend(folderPath);
            if (directoryCreated)
            {
                try
                {
                    ZipFolder zf = new ZipFolder();
                    string projType = GetProjType();
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    string zipFileName = Path.GetFileNameWithoutExtension(Path.GetTempFileName());
                    string zipOutput = Path.Combine(path, zipFileName + ".zip");
                    zf.CreateZip(folderPath, zipOutput, 0);
                    const string MailTo = "ToAddress";
                    string MailSubject = projType + " Style Sheets and Setting file";
                    string MailBody = "(Please attach the exported " + "%20" + zipOutput + " with this mail.)" +
                                      "%0D%0A" + "%0D%0A";
                    try
                    {
                        MailBody += "Extract the zip folder content to an appropriate folder on your hard drive." +
                                    "%0D%0A" + "%0D%0A";
                        MailBody = GetMailBody(projType, MailBody);
                    }
                    catch
                    {
                    }
                    System.Diagnostics.Process.Start(string.Format("mailto:{0}?Subject={1}&Body={2}", MailTo,
                                                                   MailSubject, MailBody));

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, _caption);
                }
            }
        }

        /// <summary>
        /// Method to add the file location to copy the attached files.
        /// </summary>
        /// <param name="projType">Dictionary / Scipture</param>
        /// <param name="MailBody">Existing content</param>
        /// <returns></returns>
        private static string GetMailBody(string projType, string MailBody)
        {
            if (projType.Length <= 0) return MailBody;
            MailBody += "Copy the settings file (" + projType + "StyleSettings.xml and StyleSettings.xsd) to the path" + "%0D";
            MailBody += "-------------------------------------------------------------------------------------------------------------" + "%0D%0A";
            MailBody += "Windows XP:" + "%0D%0A";
            MailBody += @"C:\Program Files\SIL\Pathway" + "%0D%0A";
            MailBody += @"C:\Documents and Settings\All Users\Application Data\SIL\Pathway\" + projType + "%0D%0A";
            MailBody += "Windows Vista or Windows 7:" + "%0D%0A";
            MailBody += @"C:\Program Files\SIL\Pathway" + "%0D%0A";
            MailBody += @"C:\Users\All Users\Application Data\SIL\Pathway\" + projType + "%0D%0A" + "%0D%0A";

            MailBody += "Copy the all css files to the path" + "%0D%0A";
            MailBody += "------------------------------------------" + "%0D%0A";
            MailBody += "Windows XP:" + "%0D%0A";
            MailBody += @"C:\Program Files\SIL\Pathway\Styles\" + projType + "%0D%0A";
            MailBody += @"C:\Documents and Settings\All Users\Application Data\SIL\Pathway\" + projType + "%0D%0A";
            MailBody += "Windows Vista or Windows 7:" + "%0D%0A";
            MailBody += @"C:\Program Files\SIL\Pathway\Styles\" + projType + "%0D%0A";
            MailBody += @"C:\Users\All Users\Application Data\SIL\Pathway\" + projType + "%0D%0A";
            return MailBody;
        }

        private static string GetProjType()
        {
            string projType = string.Empty;
            if (Param.Value["InputType"] != null)
            {
                projType = Param.Value["InputType"];
            }
            return projType;
        }

        private void tsNew_Click(object sender, EventArgs e)
        {
            try
            {
                _configurationToolBL.AddMode = true;
                _configurationToolBL.AddStyle(stylesGrid, _cssNames);
                _configurationToolBL.LoadMediaStyle(stylesGrid, _cssNames);
                _configurationToolBL.SelectRow(stylesGrid, _configurationToolBL.NewStyleName);
                SetInfoTabValue();
                txtName.Select();
                EnableToolStripButtons(true);
                //AddNewRow();
                //SetFocusToName();
                //EnableToolStripButtons(false);
            }
            catch { }
        }

        private void AddNewRow()
        {
            DataRow row = _configurationToolBL.DataSetForGrid.Tables["Styles"].NewRow(); ;
            _configurationToolBL.DataSetForGrid.Tables["Styles"].Rows.Add(row);
            stylesGrid.Refresh();
            _configurationToolBL.SelectRow(stylesGrid, "");
            SetInfoTabValue();
            string newName = _configurationToolBL.GetNewStyleName(_cssNames, "new");
            txtName.Text = newName;
            lblInfoCaption.Text = newName;
        }

        private void SetFocusToName()
        {
            Trace.WriteLineIf(_traceOn.Level == TraceLevel.Verbose, "ConfigurationTool: SetFocusToName");
            tabControl1.SelectedTab = tabControl1.TabPages[0];
            txtName.Focus();
        }
        private void tsDefault_Click(object sender, EventArgs e)
        {
            try
            {
                //var dlg = new PrintVia("Set Defaults");
                var dlg = new PrintVia();
                dlg.InputType = _configurationToolBL.InputType;
                dlg.DatabaseName = "{Project_Name}";
                dlg.Media = _configurationToolBL.MediaType;
                dlg.ShowDialog();
            }
            catch { }
        }

        
        private void ShowDataInGrid()
        {
            Trace.WriteLineIf(_traceOn.Level == TraceLevel.Verbose, "ConfigurationTool: ShowDataInGrid");
            _selectedStyle = Param.Value["LayoutSelected"];
            _redoundo.SettingOutputPath = Param.SettingOutputPath;
            tabControl1.SelectedTab = tabControl1.TabPages[0];
            lblType.Text = _configurationToolBL.InputType;
            _configurationToolBL.LoadMediaStyle(stylesGrid, _cssNames);
            _configurationToolBL.GridColumnWidth(stylesGrid);
            _configurationToolBL.PreviousValue = txtName.Text;
        }

        private void SetSideBar()
        {
            btnPaper.BackColor = _deSelectedColor;
            btnMobile.BackColor = _deSelectedColor;
            btnWeb.BackColor = _deSelectedColor;
            btnOthers.BackColor = _deSelectedColor;

            btnPaper.FlatAppearance.BorderSize = 0;
            btnMobile.FlatAppearance.BorderSize = 0;
            btnWeb.FlatAppearance.BorderSize = 0;
            btnOthers.FlatAppearance.BorderSize = 0;
            if (_configurationToolBL.MediaType == "paper")
            {
                btnPaper.BackColor = _selectedColor;
                btnPaper.FlatAppearance.BorderSize = 1;
            }
            else if (_configurationToolBL.MediaType == "mobile")
            {
                btnMobile.BackColor = _selectedColor;
                btnMobile.FlatAppearance.BorderSize = 1;
            }
            //else if (_configurationToolBL.MediaType == "web")
            //{
            //    btnWeb.BackColor = _selectedColor;
            //    btnWeb.FlatAppearance.BorderSize = 1;
            //}
            //else if (_configurationToolBL.MediaType == "others")
            //{
            //    btnOthers.BackColor = _selectedColor;
            //    btnOthers.FlatAppearance.BorderSize = 1;
            //}
        }

        private void WriteCss(object sender)
        {
            StreamWriter writeCss = null;
            string file1;
            string attribValue = Common.GetTextValue(sender, out file1);
            if (_configurationToolBL.PreviousValue == attribValue) return;

            try
            {
                string path = Param.Value["UserSheetPath"]; // all user path
                string file = Common.PathCombine(path, _configurationToolBL.FileName);
                string importStatement;

                //Reading the existing file for 1st Line (@import statement)
                var sr = new StreamReader(file);
                while ((importStatement = sr.ReadLine()) != null)
                {
                    if (importStatement.Contains("@import"))
                    {
                        break;
                    }
                }
                sr.Close();

                //Start Writing the Changes
                writeCss = new StreamWriter(file);
                if (!string.IsNullOrEmpty(importStatement))
                    writeCss.WriteLine(importStatement);

                var value = new Dictionary<string, string>();
                string attribute = "Justified";
                string key = ddlJustified.Text;
                _configurationToolBL.WriteAtImport(writeCss, attribute, key);

                attribute = "VerticalJustify";
                key = ddlVerticalJustify.Text;
                _configurationToolBL.WriteAtImport(writeCss, attribute, key);

                attribute = "Page Size";
                key = ddlPagePageSize.Text;
                _configurationToolBL.WriteAtImport(writeCss, attribute, key);

                attribute = "Columns";
                key = ddlPageColumn.Text;
                _configurationToolBL.WriteAtImport(writeCss, attribute, key);

                attribute = "Font Size";
                key = ddlFontSize.Text;
                _configurationToolBL.WriteAtImport(writeCss, attribute, key);

                attribute = "Leading";
                key = ddlLeading.Text;
                _configurationToolBL.WriteAtImport(writeCss, attribute, key);

                attribute = "Pictures";
                key = ddlPicture.Text;
                _configurationToolBL.WriteAtImport(writeCss, attribute, key);

                attribute = "Running Head";
                key = ddlRunningHead.Text;
                _configurationToolBL.WriteAtImport(writeCss, attribute, key);

                attribute = "Rules";
                key = ddlRules.Text;
                _configurationToolBL.WriteAtImport(writeCss, attribute, key);

                attribute = "Sense";
                key = ddlSense.Text;
                _configurationToolBL.WriteAtImport(writeCss, attribute, key);

                //Writing TextBox Values into Css
                if (txtPageGutterWidth.Text.Length > 0)
                {
                    value["column-gap"] = txtPageGutterWidth.Text;
                    _configurationToolBL.WriteCssClass(writeCss, "letData", value);
                }

                value.Clear();
                value["margin-top"] = txtPageTop.Text;
                value["margin-right"] = txtPageOutside.Text;
                value["margin-bottom"] = txtPageBottom.Text;
                value["margin-left"] = txtPageInside.Text;
                value["-ps-fileproduce"] = "\"" + _fileProduce + "\"";
                _configurationToolBL.WriteCssClass(writeCss, "page", value);

                if (_configurationToolBL.MediaType.ToLower() == "mobile")
                {
                    UpdateFileProduced();
                }

                writeCss.Flush();
                writeCss.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sorry, your recent changes cannot be saved because Pathway cannot find the stylesheet file '" + ex.Message + "'", _caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                //writeCss.Close();
            }
        }

        private void UpdateFileProduced()
        {
            XmlNode node = Param.GetItem("//mobileProperty/mobilefeature[@name='FileProduced']");
            Param.SetAttrValue(node, "select", _fileProduce);
        }

        #endregion

        /// <summary>
        /// transfers grid row values to InfoPanel
        ///
        /// 
        /// </summary
        private void SetInfoTabValue()
        {
            Trace.WriteLineIf(_traceOn.Level == TraceLevel.Verbose, "ConfigurationTool: SetInfoTabValue");
            if (stylesGrid.RowCount > 0)
            {
                //try
                //{
                _configurationToolBL.StyleName = stylesGrid[_configurationToolBL.ColumnName, _configurationToolBL.SelectedRowIndex].Value.ToString();
                _configurationToolBL.PreviewFileName1 = stylesGrid[_configurationToolBL.PreviewFile1, _configurationToolBL.SelectedRowIndex].Value.ToString();
                _configurationToolBL.PreviewFileName2 = stylesGrid[_configurationToolBL.PreviewFile2, _configurationToolBL.SelectedRowIndex].Value.ToString();
                //}
                //catch (Exception)
                //{
                // _configurationToolBL.SelectedRowIndex--;                    
                // _configurationToolBL.StyleName = stylesGrid[_configurationToolBL.ColumnName, _configurationToolBL.SelectedRowIndex].Value.ToString();

                // _configurationToolBL.SelectRow(stylesGrid, _configurationToolBL.StyleName);
                //} 
                txtName.Text = lblInfoCaption.Text = _configurationToolBL.StyleName;
                _configurationToolBL.FileName = stylesGrid[_configurationToolBL.ColumnFile, _configurationToolBL.SelectedRowIndex].Value.ToString();
                txtDesc.Text = stylesGrid[_configurationToolBL.ColumnDescription, _configurationToolBL.SelectedRowIndex].Value.ToString();
                txtComment.Text = stylesGrid[_configurationToolBL.ColumnComment, _configurationToolBL.SelectedRowIndex].Value.ToString();
                bool check = stylesGrid[_configurationToolBL.ColumnShown, _configurationToolBL.SelectedRowIndex].Value.ToString().ToLower() == "yes" ? true : false;
                chkAvailable.Checked = check;
                txtApproved.Text = stylesGrid[_configurationToolBL.AttribApproved, _configurationToolBL.SelectedRowIndex].Value.ToString();
                string type = stylesGrid[_configurationToolBL.ColumnType, _configurationToolBL.SelectedRowIndex].Value.ToString();
                if (type == _configurationToolBL.TypeStandard)
                {
                    EnableDisablePanel(false);
                    //tsDelete.Enabled = false;
                    tsPreview.Enabled = true;
                    txtApproved.Visible = true;
                    lblApproved.Visible = true;
                }
                else
                {
                    EnableDisablePanel(true);
                    //tsPreview.Enabled = false;
                    txtApproved.Visible = false;
                    lblApproved.Visible = false;
                }
            }
            SetCSSTabValue();
        }

        /// <summary>
        /// Fills Values in Display property Tab and Properties Tab
        /// </summary>
        private void SetCSSTabValue()
        {
            _errProvider.Clear();
            if (txtName.Text.Length <= 0) return;

            string path = Param.StylePath(txtName.Text);
            _configurationToolBL.SetDefaultCSSValue(path, Param.Value["InputType"]);

            double left = Math.Round(double.Parse(_configurationToolBL.MarginLeft), 0);
            txtPageInside.Text = left + "pt";
            double right = Math.Round(double.Parse(_configurationToolBL.MarginRight), 0);
            txtPageOutside.Text = right + "pt";
            double top = Math.Round(double.Parse(_configurationToolBL.MarginTop), 0);
            txtPageTop.Text = top + "pt";
            double bottom = Math.Round(double.Parse(_configurationToolBL.MarginBottom), 0);
            txtPageBottom.Text = bottom + "pt";

            txtPageGutterWidth.Text = _configurationToolBL.GutterWidth;
            if (_configurationToolBL.GutterWidth.IndexOf('%') == -1)
            {
                if (txtPageGutterWidth.Text.Length > 0)
                    txtPageGutterWidth.Text = txtPageGutterWidth.Text + "pt";
            }
            ddlPageColumn.SelectedItem = _configurationToolBL.ColumnCount;
            ddlFontSize.SelectedItem = _configurationToolBL.FontSize;
            ddlLeading.SelectedItem = _configurationToolBL.Leading;
            ddlPicture.SelectedItem = _configurationToolBL.Picture;
            ddlJustified.SelectedItem = _configurationToolBL.JustifyUI;
            ddlPagePageSize.SelectedItem = _configurationToolBL.PageSize;
            ddlRunningHead.SelectedItem = _configurationToolBL.RunningHeader;
            ddlRules.SelectedItem = _configurationToolBL.ColumnRule;
            ddlSense.SelectedItem = _configurationToolBL.Sense;
            ddlVerticalJustify.SelectedItem = _configurationToolBL.VerticalJustify;

            try
            {
                if (_configurationToolBL.InputType.ToLower() == "scripture" && _configurationToolBL.MediaType.ToLower() == "mobile")
                {

                    ddlFiles.SelectedItem = _configurationToolBL.FileProduced;
                    SetMobileSummary(null, null);
                }
                else
                {
                    ddlFileProduceDict.SelectedItem = _configurationToolBL.FileProduced.Trim();
                    SetStyleSummary();

                }
            }

            catch
            {
            }
            _isCreatePreview = false;
        }

        ///// <summary>
        ///// If the value of margins are invalid at load time, the values are shown in red color(TD-1331).
        ///// </summary>
        //private void SetTextColor()
        //{
        //    CompareMarginValues(txtPageTop, 18);
        //    CompareMarginValues(txtPageInside, 18);
        //    CompareMarginValues(txtPageOutside, 18);
        //    CompareMarginValues(txtPageBottom, 18);
        //}

        /// <summary>
        /// Fills Values in Display property Tab
        /// 
        /// </summary>
        private void PopulateFeatureSheet()
        {
            Trace.WriteLineIf(_traceOn.Level == TraceLevel.Verbose, "ConfigurationTool: PopulateFeatureSheet");
            TreeView TvFeatures = new TreeView();
            _configurationToolBL.PopulateFeatureLists(TvFeatures);
            try
            {
                foreach (TreeNode tn in TvFeatures.Nodes)
                {
                    string task = tn.Text;
                    foreach (TreeNode ctn in tn.Nodes)
                    {
                        switch (task)
                        {
                            case "Page Size":
                                ddlPagePageSize.Items.Add(ctn.Text);
                                break;

                            case "Columns":
                                ddlPageColumn.Items.Add(ctn.Text);
                                break;

                            case "Leading":
                                ddlLeading.Items.Add(ctn.Text);
                                break;

                            case "Font Size":
                                ddlFontSize.Items.Add(ctn.Text);
                                break;

                            case "Running Head":
                                ddlRunningHead.Items.Add(ctn.Text);
                                break;

                            case "Rules":
                                ddlRules.Items.Add(ctn.Text);
                                break;

                            case "Pictures":
                                ddlPicture.Items.Add(ctn.Text);
                                break;

                            case "Sense":
                                ddlSense.Items.Add(ctn.Text);
                                break;

                            case "Justified":
                                ddlJustified.Items.Add(ctn.Text);
                                break;

                            case "VerticalJustify":
                                ddlVerticalJustify.Items.Add(ctn.Text);
                                break;

                            case "FileProduced":
                                ddlFiles.Items.Add(ctn.Text);
                                ddlFileProduceDict.Items.Add(ctn.Text);
                                break;

                            case "RedLetter":
                                ddlRedLetter.Items.Add(ctn.Text);
                                break;
                        }
                    }
                }
            }
            catch
            {
            }
        }
        /// <summary>
        /// Fill the Tab Control values into Text Box
        /// </summary>
        private void SetStyleSummary()
        {
            try
            {
                string leading = (ddlLeading.Text.Length > 0) ? " Line Spacing " + ddlLeading.Text + ", " : " ";
                string fontSize = (ddlFontSize.Text.Length > 0) ? " Base FontSize - " + ddlFontSize.Text + ", " : "";

                string rules = ddlRules.Text == "Yes"
                                   ? "With Divider Lines, "
                                   : "Without Divider Lines, ";
                string justified = ddlJustified.Text == "Yes"
                                       ? "Justified, "
                                       : "";
                string picture = ddlPicture.Text == "Yes"
                                     ? "With picture "
                                     : "Without picture ";
                string pageSize = (ddlPagePageSize.Text.Length > 0) ? ddlPagePageSize.Text + "," : "";
                string pageColumn = (ddlPageColumn.Text.Length > 0) ? ddlPageColumn.Text + " Column(s), " : "";
                string gutter = txtPageGutterWidth.Text.Length > 0 ? "Column Gap - " + txtPageGutterWidth.Text + ", " : "";

                string marginTop = txtPageTop.Text.Length > 0 ? "Margin Top - " + txtPageTop.Text + ", " : "";
                string marginBottom = txtPageBottom.Text.Length > 0 ? "Margin Bottom - " + txtPageBottom.Text + ", " : "";
                string marginInside = txtPageInside.Text.Length > 0 ? "Margin Inside - " + txtPageInside.Text + ", " : "";
                string marginOutside = txtPageOutside.Text.Length > 0 ? "Margin Outside - " + txtPageOutside.Text + ", " : "";
                string sense = (ddlSense.Text.Length > 0) ? " Sense Layout - " + ddlSense.Text + "," : "";
                string combined = pageSize + " " +
                                  pageColumn + "  " +
                                  gutter + " " +
                                  marginTop + " " +
                                  marginBottom + " " +
                                  marginInside + " " +
                                  marginOutside + " " +
                                  leading + " " +
                                  fontSize + " " +
                                  ddlRunningHead.Text + " " +
                                  rules + " " +
                                  justified + " " +
                                  sense + " " +
                                  picture + ".";
                txtCss.Text = combined;
            }
            catch { }
        }

        #region Clear All Tab Controls

        /// <summary>
        /// ClearTab the controls
        /// </summary>



        #endregion

        private void EnableToolStripButtons(bool enable)
        {
            string selectedTypeValue = stylesGrid[_configurationToolBL.ColumnType, _configurationToolBL.SelectedRowIndex].Value.ToString();
            if (selectedTypeValue != _configurationToolBL.TypeStandard)
            {
                tsNew.Enabled = enable;
                tsDelete.Enabled = enable;
                tsSaveAs.Enabled = enable;
                //tsPreview.Enabled = true;
            }
        }

        /// <summary>
        /// Enable or Disable the Panel controls
        /// </summary>
        /// <param name="IsEnable">True or False</param>
        private void EnableDisablePanel(bool IsEnable)
        {
            tsDelete.Enabled = IsEnable;
            tabDisplay.Enabled = IsEnable;
            tabMobile.Enabled = IsEnable;
        }

        private void txtPageGutterWidth_Validated(object sender, EventArgs e)
        {
            try
            {
                bool result = Common.AssignValuePageUnit(txtPageGutterWidth, null);
                _errProvider = Common._errProvider;
                if (_errProvider.GetError(txtPageGutterWidth) != "")
                {
                    _errProvider.SetError(txtPageGutterWidth, _errProvider.GetError(txtPageGutterWidth));
                }
                else
                {
                    ValidatePageWidthMargins(sender, e);
                }
                WriteCss(txtPageGutterWidth);
            }
            catch { }
        }

        private void txtPageInside_Validated(object sender, EventArgs e)
        {
            try
            {
                bool result = Common.AssignValuePageUnit(txtPageInside, null);
                _errProvider = Common._errProvider;
                if (_errProvider.GetError(txtPageInside) != "")
                {
                    _errProvider.SetError(txtPageInside, _errProvider.GetError(txtPageInside));
                }
                else
                {
                    ValidatePageWidthMargins(sender, e);
                }

                WriteCss(sender);
            }
            catch { }
        }

        private void txtPageOutside_Validated(object sender, EventArgs e)
        {
            try
            {
                bool result = Common.AssignValuePageUnit(txtPageOutside, null);
                _errProvider = Common._errProvider;
                if (_errProvider.GetError(txtPageOutside) != "")
                {
                    _errProvider.SetError(txtPageOutside, _errProvider.GetError(txtPageOutside));
                }
                else
                {
                    ValidatePageWidthMargins(sender, e);
                }
                WriteCss(sender);
            }
            catch { }
        }

        private void txtPageTop_Validated(object sender, EventArgs e)
        {
            try
            {
                bool result = Common.AssignValuePageUnit(txtPageTop, null);
                _errProvider = Common._errProvider;
                if (_errProvider.GetError(txtPageTop) != "")
                {
                    _errProvider.SetError(txtPageTop, _errProvider.GetError(txtPageTop));
                }
                else
                {
                    ValidatePageHeightMargins(sender, e);
                }
                WriteCss(sender);
            }
            catch { }
        }

        private void txtPageBottom_Validated(object sender, EventArgs e)
        {
            try
            {
                bool result = Common.AssignValuePageUnit(txtPageBottom, null);
                _errProvider = Common._errProvider;
                if (_errProvider.GetError(txtPageBottom) != "")
                {
                    _errProvider.SetError(txtPageBottom, _errProvider.GetError(txtPageBottom));
                }
                else
                {
                    ValidatePageHeightMargins(sender, e);
                }
                WriteCss(sender);
            }
            catch { }
        }

        private void ConfigurationTool_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                setLastSelectedLayout();
                setDefaultInputType();

            }
            catch { }
        }

        private void setDefaultInputType()
        {
            Param.SetValue(Param.InputType, ""); // loading settingsxml.
            Param.LoadSettings();
            Param.SetValue(Param.InputType, _configurationToolBL.InputType); // last input type
            Param.Write();
            Param.CopySchemaIfNecessary();
        }
        private void setLastSelectedLayout()
        {
            //Check and move
            string layoutName = txtName.Text;
            if (layoutName.Length == 0)
                layoutName = _lastSelectedLayout;
            Param.SetValue(Param.LayoutSelected, layoutName); // last layout
            Param.Write();
        }
        private void btnDictionary_Click(object sender, EventArgs e)
        {
            try
            {
                btnMobile.Enabled = false;
                btnWeb.Enabled = false;
                btnOthers.Enabled = false;
                setLastSelectedLayout();
                _configurationToolBL.WriteMedia();
                _configurationToolBL.InputType = "Dictionary";
                SetInputTypeButton();
                _configurationToolBL.LoadParam();
                _configurationToolBL.ClearPropertyTab(tabDisplay);
                PopulateFeatureSheet(); //For TD-1194 // Load Default Values
                _configurationToolBL.SetPreviousLayoutSelect(stylesGrid);
                SetSideBar();
                ShowDataInGrid();
                SetMobilePropertyTab();
                _redoundo.Reset();
            }
            catch
            {
            }
        }
        private void btnScripture_Click(object sender, EventArgs e)
        {
            try
            {
                btnMobile.Enabled = true;
                btnWeb.Enabled = false;
                btnOthers.Enabled = false;
                setLastSelectedLayout();
                _configurationToolBL.WriteMedia();
                _configurationToolBL.InputType = "Scripture";
                SetInputTypeButton();
                _configurationToolBL.LoadParam();
                _configurationToolBL.ClearPropertyTab(tabDisplay);
                _configurationToolBL.ClearPropertyTab(tabmob);
                PopulateFeatureSheet(); //For TD-1194 // Load Default Values
                _configurationToolBL.SetPreviousLayoutSelect(stylesGrid);
                SetSideBar();
                ShowDataInGrid();
                SetMobilePropertyTab();
                _redoundo.Reset();
            }
            catch
            {
            }
        }

        private void SideBar()
        {
            try
            {
                _configurationToolBL.WriteMedia();
                setLastSelectedLayout();
                _configurationToolBL.LoadParam();
                SetSideBar();
                ShowDataInGrid();
                _redoundo.Reset();
                SetMobilePropertyTab();
            }
            catch
            {
            }
        }

        private void SetMobilePropertyTab()
        {
            tabControl1.TabPages.Remove(tabControl1.TabPages[1]);
            if (_configurationToolBL.MediaType == "mobile")
            {

                tabControl1.TabPages.Add(tabmob);
                Dictionary<string, string> mobilefeature = Param.GetItemsAsDictionary("//mobileProperty/mobilefeature");
                if (mobilefeature.Count > 0)
                {
                    string key = "FileProduced";
                    if (mobilefeature.ContainsKey(key))
                    {
                        ddlFiles.Text = mobilefeature[key];
                    }
                    key = "RedLetter";
                    if (mobilefeature.ContainsKey(key))
                    {
                        ddlRedLetter.Text = mobilefeature[key];
                    }
                    key = "Information";
                    if (mobilefeature.ContainsKey(key))
                    {
                        txtInformation.Text = mobilefeature[key];
                    }
                    key = "Copyright";
                    if (mobilefeature.ContainsKey(key))
                    {
                        txtCopyright.Text = mobilefeature[key];
                    }
                    key = "Icon";
                    if (mobilefeature.ContainsKey(key))
                    {
                        try
                        {
                            string iconPath = mobilefeature[key];
                            mobileIcon.Load(iconPath);
                        }
                        catch
                        {
                        }
                    }
                }
                SetMobileSummary(null, null);
            }
            else
            {
                tabControl1.TabPages.Add(tabdic);
                SetStyleSummary();
            }
        }


        private void txtName_Validated(object sender, EventArgs e)
        {
            try
            {
                txtName.Text = txtName.Text.Trim();
                if (!NoDuplicateStyleName() || !ValidateStyleName(txtName.Text))
                {
                    txtName.Text = _configurationToolBL.PreviousValue;
                    stylesGrid.Rows[_configurationToolBL.SelectedRowIndex].Cells[0].Value =
                        _configurationToolBL.PreviousValue;
                    txtName.Focus();
                    return;
                }
                string styleName = txtName.Text;
                _configurationToolBL.FileName = txtName.Text + ".css";
                stylesGrid[_configurationToolBL.ColumnFile, _configurationToolBL.SelectedRowIndex].Value =
                    _configurationToolBL.FileName;

                if (_configurationToolBL.AddMode) // Add
                {
                    //_redoundo.Set(Common.Action.New, _configurationToolBL.StyleName, null, "", string.Empty);

                    Param.StyleFile[styleName] = _configurationToolBL.FileName;
                    string errMsg = _configurationToolBL.CreateCssFile(_configurationToolBL.FileName);
                    if (errMsg.Length > 0)
                    {
                        MessageBox.Show("Sorry, your recent changes cannot be saved because Pathway cannot find the stylesheet file '" + errMsg + "'", _caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    }
                    _configurationToolBL.AddNew(txtName.Text);
                    EnableToolStripButtons(true);
                    _configurationToolBL.LoadMediaStyle(stylesGrid, _cssNames);
                    _configurationToolBL.SelectRow(stylesGrid, _configurationToolBL.PreviousValue);
                    SetInfoTabValue();
                }
                else // Edit
                {
                    if (_configurationToolBL.PreviousValue == styleName)
                    {
                        return;
                    }
                    //file -> fileNew1 -> fileNew.css
                    string path = Param.Value["UserSheetPath"];
                    string fromFile = Common.PathCombine(path, _configurationToolBL.PreviousValue + ".css");
                    string toFile = Common.PathCombine(path, _configurationToolBL.FileName);
                    if (File.Exists(fromFile))
                        try
                        {
                            File.Move(fromFile, toFile);
                        }
                        catch
                        {
                        }
                    Param.StyleFile[styleName] = _configurationToolBL.FileName;
                    _configurationToolBL.WriteAttrib(_configurationToolBL.AttribName, sender);
                    _cssNames.Remove(_configurationToolBL.PreviousValue);
                    EnableToolStripButtons(true);
                    IsLayoutSelectedStyle();
                }
                _configurationToolBL.StyleName = txtName.Text;
                lblInfoCaption.Text = txtName.Text;

            }
            catch { }
        }

        //Note - Check This
        private void IsLayoutSelectedStyle()
        {
            if (_selectedStyle == _configurationToolBL.PreviousValue) // LayoutSelected Value
            {
                Param.SetValue(Param.LayoutSelected, _configurationToolBL.StyleName);
            }
        }

        #region ValidateStartsWithAlphabet(string stringValue)
        /// <summary>
        /// Validate a given string is Starts with alphabets or not 
        /// </summary>
        /// <param name="stringValue">string</param>
        /// <returns>True/False</returns>
        public bool ValidateStyleName(string stringValue)
        {
            string result = string.Empty;
            bool valid = true;
            //if (!Common.ValidateStartsWithAlphabet(stringValue))
            //{
            //    result = "Please enter the Style name starts with alphabet";
            //}
            //else
            //{

            foreach (char ch in stringValue)
            {
                if (!(ch == '-' || ch == '_' || ch == ' ' || ch == '.' || ch == '(' || ch == ')' ||
                    (ch >= 48 && ch <= 57) || (ch >= 65 && ch <= 90) || (ch >= 97 && ch <= 122)))
                {
                    result = "Please avoid " + ch + " in the Style name";
                    break;
                }
            }
   
            //}
            if (result != string.Empty)
            {
                MessageBox.Show(result, _caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                valid = false;
            }
            return valid;
        }
        #endregion
        private bool NoDuplicateStyleName()
        {
            bool result = true;
            if (txtName.Text.ToLower() != _configurationToolBL.PreviousStyleName.ToLower()) //Is new styleName?
            {
                // Add - Check whether the same name already exist
                // Edit- Check it, while the name changed in Stylename textBox.
                if (_configurationToolBL.IsNameExists(stylesGrid, txtName.Text))
                {
                    MessageBox.Show("Stylesheet Name [" + txtName.Text + "] already exists", _caption,
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    result = false;
                    txtName.Focus();
                }
            }
            return result;
        }


        private void txtDesc_Validated(object sender, EventArgs e)
        {
            try
            {
                _configurationToolBL.WriteAttrib(_configurationToolBL.ElementDesc, sender);
                EnableToolStripButtons(true);

            }
            catch { }
        }

        private void chkAvailable_Validated(object sender, EventArgs e)
        {
            try
            {
                _configurationToolBL.WriteAttrib(_configurationToolBL.AttribShown, sender);
                EnableToolStripButtons(true);

            }
            catch { }
            // _redoundo.Set(Common.Action.Edit, _configurationToolBL.StyleName, "chkAvailable", _configurationToolBL.PreviousValue, chkAvailable.Checked.ToString());
            //_configurationToolBL.PreviousValue = chkAvailable.Checked.ToString();
        }

        private void txtComment_Validated(object sender, EventArgs e)
        {
            try
            {
                _configurationToolBL.WriteAttrib(_configurationToolBL.ElementComment, sender);
                EnableToolStripButtons(true);

            }
            catch { }
        }

        private void SetGotFocusValue(object sender, EventArgs e)
        {
            try
            {
                string control;
                //if (_configurationToolBL.PreviousValue != string.Empty)
                //{
                //    _redoundo.Set(Common.Action.Edit, _configurationToolBL.StyleName, _currentControl, _configurationToolBL.PreviousValue);
                //}
                _configurationToolBL.PreviousValue = Common.GetTextValue(sender, out control);
                _redoUndoBufferValue = _configurationToolBL.PreviousValue;
                _redoundo.PreviousControl = _redoundo.CurrentControl;
                _redoundo.CurrentControl = control;

            }
            catch { }
        }

        private void ddlPagePageSize_Validated(object sender, EventArgs e)
        {
            WriteCss(sender);
        }

        private void ddlPageColumn_Validated(object sender, EventArgs e)
        {
            WriteCss(sender);
        }

        private void ddlJustified_Validated(object sender, EventArgs e)
        {
            WriteCss(sender);
        }

        private void ddlSense_Validated(object sender, EventArgs e)
        {
            WriteCss(sender);
        }

        private void ddlPicture_Validated(object sender, EventArgs e)
        {
            WriteCss(sender);
        }

        private void ddlLeading_Validated(object sender, EventArgs e)
        {
            WriteCss(sender);
        }

        private void ddlRunningHead_Validated(object sender, EventArgs e)
        {
            WriteCss(sender);
        }

        private void ddlRules_Validated(object sender, EventArgs e)
        {
            WriteCss(sender);
        }

        private void ddlFontSize_Validated(object sender, EventArgs e)
        {
            WriteCss(sender);
        }

        private void chkAvailable_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                _configurationToolBL.UpdateGrid(chkAvailable, stylesGrid);

            }
            catch { }
            // NOTE - Pending
            //_redoundo.Set(Common.Action.Edit, sender); 
        }
        private void tsUndo_Click(object sender, EventArgs e)
        {
            try
            {
                ModifyData control = _redoundo.Undo(Common.Action.Edit, _styleName, _redoundo.CurrentControl, _configurationToolBL.PreviousValue);
                _configurationToolBL.PreviousValue = string.Empty;
                if (control.Action != Common.Action.Edit) // Add or Delete
                {
                    _configurationToolBL.LoadParam();
                    _configurationToolBL.ClearPropertyTab(tabDisplay);
                    PopulateFeatureSheet();
                    _configurationToolBL.SetPreviousLayoutSelect(stylesGrid);
                    ShowDataInGrid();
                    _configurationToolBL.SelectRow(stylesGrid, control.EditStyleName);
                }
                else // Edit
                {
                    bool success = SetUI(control);
                    if (!success)
                    {
                        // Ignore Recent modification. Ex: 123 - ignores 3 and gives 12 
                        tsUndo_Click(sender, e);
                    }
                }
            }
            catch { }
        }

        private bool SetUI(ModifyData control)
        {
            bool success = true;
            string controlName = control.FileName;
            string controlText = control.ControlText;

            Control[] ctls = tabInfo.Controls.Find(controlName, true);
            if (ctls.Length > 0)
            {
                Control ctl = ctls[0];
                if (ctl.Text == controlText)
                {
                    success = false;
                }
                else
                {
                    _configurationToolBL.SelectRow(stylesGrid, control.EditStyleName);
                    if (ctl is TextBox)
                    {
                        TextBox textBox = (TextBox)ctl;
                        textBox.Text = controlText;
                        textBox.Focus();
                        textBox.SelectAll();
                    }
                    else if (ctl is CheckBox)
                    {
                        CheckBox checkBox = (CheckBox)ctl;
                        checkBox.Checked = controlText == "True" ? true : false;
                    }
                    else
                    {
                        ctl.Text = controlText;
                        ctl.Focus();
                        ctl.Select();
                    }
                }
                _configurationToolBL.UpdateGrid(ctl, stylesGrid);
            }
            return success;
        }
        private void tsRedo_Click(object sender, EventArgs e)
        {
            try
            {
                ModifyData control = _redoundo.Redo();
                if (control.Action != Common.Action.Edit) // Add or Delete
                {
                    _configurationToolBL.LoadParam();
                    _configurationToolBL.ClearPropertyTab(tabDisplay);
                    PopulateFeatureSheet();
                    _configurationToolBL.SetPreviousLayoutSelect(stylesGrid);
                    ShowDataInGrid();
                    _configurationToolBL.SelectRow(stylesGrid, control.EditStyleName);
                }
                else // Edit
                {
                    bool success = SetUI(control);
                    if (!success)
                    {
                        // Ignore Recent modification. Ex: 123 - ignores 3 and gives 12 
                        tsRedo_Click(sender, e);
                    }
                }

            }
            catch { }
        }

        private void ValidateLineHeight(object sender, EventArgs e)
        {
            if (ddlFontSize.Text.Length > 0 && ddlLeading.Text != "No Change")
            {
                int selectedfontSize = int.Parse(ddlFontSize.Text);
                int selectedLineHeight = int.Parse(ddlLeading.Text);
                int expectedLineHeight = selectedfontSize * 120 / 100;
                string errMessage = selectedLineHeight < expectedLineHeight ? "Line height should be 120% of font size" : "";
                _errProvider.SetError(txtPageInside, errMessage);
            }
        }

        private void ValidatePageWidthMargins(object sender, EventArgs e)
        {
            int marginLeft = GetDefaultValue(txtPageInside.Text);
            int marginRight = GetDefaultValue(txtPageOutside.Text);
            int columnGap = GetDefaultValue(txtPageGutterWidth.Text);
            int expectedWidth = GetPageSize(ddlPagePageSize.Text, "Width") / 2;
            int outputWidth = marginLeft + columnGap + marginRight;

            Control ctrl = ((Control)sender);
            string errMessage = outputWidth > expectedWidth ? "The combination of the column gap, left and right margins should not exceed half of the page width." : "";
            _errProvider.SetError(ctrl, errMessage);
            _errProvider.SetError(txtPageInside, errMessage);
            _errProvider.SetError(txtPageOutside, errMessage);
            _errProvider.SetError(txtPageGutterWidth, errMessage);
            SetStyleSummary();
        }

        //private static void CompareMarginValues(Control txtBox, int lowestBound)
        //{
        //    int marginInPoint = int.Parse(Common.UnitConverter(txtBox.Text, "pt"));
        //    if (marginInPoint < lowestBound)
        //    {
        //        txtBox.ForeColor = Color.Red;
        //    }
        //    else
        //    {
        //        txtBox.ForeColor = Color.Black;
        //    }
        //}

        private void ValidatePageHeightMargins(object sender, EventArgs e)
        {
            int marginTop = GetDefaultValue(txtPageTop.Text);
            int marginBottom = GetDefaultValue(txtPageBottom.Text);
            int expectedHeight = GetPageSize(ddlPagePageSize.Text, "Height") / 4;
            int outputHeight = marginTop + marginBottom;

            Control ctrl = ((Control)sender);
            string errMessage = outputHeight > expectedHeight ? "The combination of the margin-Top and margin-bottom should not exceed a quarter of the page height." : "";
            
            _errProvider.SetError(ctrl, errMessage);
            if (errMessage.Length == 0)
            {
                _errProvider.SetError(txtPageTop, errMessage);
                _errProvider.SetError(txtPageBottom, errMessage);
            }
            //FillCssValues();
            SetStyleSummary();
        }

        private static int GetPageSize(string paperSize, string dimension)
        {
            int pageWidth = 612;
            int pageHeight = 792;
            switch (paperSize)
            {
                case "A4":
                    pageWidth = 595;
                    pageHeight = 842;
                    break;
                case "A5":
                    pageWidth = 420;
                    pageHeight = 595;
                    break;
                case "C5":
                    pageWidth = 459;
                    pageHeight = 649;
                    break;
                case "A6":
                    pageWidth = 298;
                    pageHeight = 420;
                    break;
                case "Half letter":
                    pageWidth = 396;
                    pageHeight = 612;
                    break;
                case "5.25in x 8.25in":
                    pageWidth = 378;
                    pageHeight = 594;
                    break;
                case "5.8in x 8.7in":
                    pageWidth = 418;
                    pageHeight = 626;
                    break;
                case "6in x 9in":
                    pageWidth = 432;
                    pageHeight = 648;
                    break;
            }

            if (dimension == "Height")
            {
                return pageHeight;
            }
            return pageWidth;
        }


        private static int GetDefaultValue(string value)
        {
            if (value.Length > 0 && Common.ValidateNumber(value.Replace("pt", "")))
            {
                return int.Parse(value.Replace("pt", ""));
            }
            return 0;

        }

        private void Set(object sender, EventArgs e)
        {
            _isCreatePreview = true;
            SetStyleSummary();
        }

        private void SetMobileSummary(object sender, EventArgs e)
        {
            string comma = ", ";
            string red = (ddlRedLetter.Text.Length > 0 && ddlRedLetter.Text.ToLower() == "yes") ? " Red Letter  " : "";
            if (red.Length == 0)
                comma = "";
            string files = (ddlFiles.Text.Length > 0) ? " Numbers of files produced -  " + ddlFiles.Text + comma : " ";

            string combined =
                files + " " +
                red;

            txtCss.Text = combined;
        }

        private void txtApproved_Validated(object sender, EventArgs e)
        {
            try
            {
                _configurationToolBL.WriteAttrib(_configurationToolBL.AttribApproved, sender);
                EnableToolStripButtons(true);

            }
            catch { }
        }

        private void ConfigurationTool_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                //if (e.Control && e.KeyCode == Keys.Delete)
                if (stylesGrid.Focused && e.KeyCode == Keys.Delete)
                {
                    if (tsDelete.Enabled)
                    {
                        tsDelete_Click(sender, null);
                    }
                }
                else if (e.KeyCode == Keys.F1)
                {
                    Common.PathwayHelpSetup(btnScripture.Enabled, Common.FromRegistry("Help"));
                    Common.HelpProv.SetHelpNavigator(this, HelpNavigator.Topic);
                    Common.HelpProv.SetHelpKeyword(this, "Overview.htm");
                    SendKeys.Send("{F1}");
                }
            }
            catch { }
        }

        private void stylesGrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                _isCreatePreview = false;
                if (!_configurationToolBL.AddMode)
                {
                    _configurationToolBL.SelectedRowIndex = e.RowIndex;
                    SetInfoTabValue();
                }
                else
                {
                    _configurationToolBL.AddMode = false;
                }
            }
            catch { }
        }

        private void txtName_Enter(object sender, EventArgs e)
        {
            SetGotFocusValue(sender, e);
        }

        private void btnPaper_Click(object sender, EventArgs e)
        {
            _configurationToolBL.MediaType = "paper";
            SideBar();
        }

        private void btnMobile_Click(object sender, EventArgs e)
        {
            _configurationToolBL.MediaType = "mobile";
            SideBar();
        }

        private void btnWeb_Click(object sender, EventArgs e)
        {
            //_configurationToolBL.MediaType = "web";
            //SideBar();
        }

        private void btnOthers_Click(object sender, EventArgs e)
        {
            //_configurationToolBL.MediaType = "others";
            //SideBar();
        }

        private void stylesGrid_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            try
            {
                string columnIndex = e.Column.Index.ToString();
                string columnWidth = e.Column.Width.ToString();
                _configurationToolBL.SaveColumnWidth(columnIndex, columnWidth);
            }
            catch { }
        }

        private void txtDesc_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                _configurationToolBL.UpdateGrid(txtDesc, stylesGrid);
                _redoundo.Set(Common.Action.Edit, _configurationToolBL.StyleName, "txtDesc", _redoUndoBufferValue, txtDesc.Text);
                _redoUndoBufferValue = txtDesc.Text;

            }
            catch { }
        }

        private void txtComment_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                _configurationToolBL.UpdateGrid(txtComment, stylesGrid);
                _redoundo.Set(Common.Action.Edit, _configurationToolBL.StyleName, "txtComment", _redoUndoBufferValue, txtComment.Text);
                _redoUndoBufferValue = txtComment.Text;
            }
            catch { }
        }

        private void txtName_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                _configurationToolBL.UpdateGrid(txtName, stylesGrid);
                _redoundo.Set(Common.Action.Edit, _configurationToolBL.StyleName, "txtName", _redoUndoBufferValue, txtName.Text);
                _redoUndoBufferValue = txtName.Text;
            }
            catch { }
        }

        private void ddlPageColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtPageGutterWidth.Text.Length == 0)
                    txtPageGutterWidth.Text = "18pt";
                txtPageGutterWidth.Enabled = true;
            }
            catch { }
            //Set(sender, e);
            //txtPageGutterWidth_Validated(sender, e);
        }

        private void tsPreview_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = string.Empty;
                //if (_isCreatePreview)
                {
                    string cssFile = Param.StylePath(_configurationToolBL.FileName);
                    PdftoJpg pd = new PdftoJpg();
                    fileName = pd.ConvertPdftoJpg(cssFile, true);
                    //return;
                }

                string settingPath = Path.GetDirectoryName(Param.SettingPath);
                string inputPath = Common.PathCombine(settingPath, "Styles");
                inputPath = Common.PathCombine(inputPath, Param.Value["InputType"]);
                string stylenamePath = Common.PathCombine(inputPath, "Preview");
                if (!Directory.Exists(stylenamePath)) return;
                String imageFile = Common.PathCombine(stylenamePath, _configurationToolBL.PreviewFileName1);
                String imageFile1 = Common.PathCombine(stylenamePath, _configurationToolBL.PreviewFileName2);

                if (!(File.Exists(imageFile) && File.Exists(imageFile1)))
                {
                    imageFile = Common.PathCombine(Common.GetAllUserPath(), Path.GetFileNameWithoutExtension(fileName) + ".pdf1.jpg");
                    imageFile1 = Common.PathCombine(Common.GetAllUserPath(), Path.GetFileNameWithoutExtension(fileName) + ".pdf2.jpg");
                }
                if (!File.Exists(imageFile))
                {
                    MessageBox.Show("Preview - File1 is not found");
                }
                if (!File.Exists(imageFile1))
                {
                    MessageBox.Show("Preview - File2 is not found");
                }
                if (File.Exists(imageFile) || File.Exists(imageFile1))
                {
                    PreviewConfig preview = new PreviewConfig(imageFile,
                                                              imageFile1)
                                                {
                                                    Text = ("Preview - " + _configurationToolBL.StyleName)
                                                };
                    preview.Icon = this.Icon;
                    preview.ShowDialog();
                }
                string xPath = "//styles/" + _configurationToolBL.MediaType + "/style[@name='" + _configurationToolBL.StyleName + "']";
                XmlNode baseNode = Param.GetItem(xPath);
                if (baseNode != null)
                {
                    Param.SetAttrValue(baseNode,"previewfile1",imageFile);
                    Param.SetAttrValue(baseNode, "previewfile2", imageFile1);
                    Param.Write();
                }
            }
            catch { }
        }

        private void Preview_PDF_Export(string inputPath)
        {
            string _backendPath = Common.ProgInstall;
            Backend.Load(_backendPath);
            PublicationInformation _projectInfo = new PublicationInformation();
            _projectInfo.DictionaryOutputName = _configurationToolBL.StyleName + "_" + _configurationToolBL.FileName;
            //_projectInfo.IsOpenOutput = false;
            _projectInfo.IsOpenOutput = true;
            string sampleFilePath = Path.GetDirectoryName(Path.GetDirectoryName(Common.GetPSApplicationPath()));
            string xthmlPath = Common.PathCombine(sampleFilePath, "BuangExport.xhtml");
            _projectInfo.DefaultXhtmlFileWithPath = xthmlPath;
            string cssPath = Common.PathCombine(inputPath, _configurationToolBL.FileName);
            _projectInfo.DefaultCssFileWithPath = cssPath;
            Backend.Launch("Pdf", _projectInfo);
        }

        private void ddlVerticalJustify_Validated(object sender, EventArgs e)
        {
            WriteCss(sender);
        }

        private void txtInformation_Validated(object sender, EventArgs e)
        {
            try
            {
                Param.WriteMobileAttrib("Information", txtInformation.Text);
            }
            catch
            {
            }
        }

        private void txtCopyright_Validated(object sender, EventArgs e)
        {
            try
            {
                Param.WriteMobileAttrib("Copyright", txtCopyright.Text);
            }
            catch
            {
            }
        }

        private void ddlFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _fileProduce = ddlFiles.Text;
                Param.WriteMobileAttrib("FileProduced", ddlFiles.Text);
                SetMobileSummary(sender, e);
            }
            catch { }
        }

        private void ddlRedLetter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Param.WriteMobileAttrib("RedLetter", ddlRedLetter.Text);
                SetMobileSummary(sender, e);
            }
            catch { }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "png (*.png) |*.png";
            openFile.ShowDialog();

            string filename = openFile.FileName;
            if (filename != "")
            {
                try
                {
                    Image iconImage = Image.FromFile(filename);
                    double height = iconImage.Height;
                    double width = iconImage.Width;
                    if (height != 20 || width != 20)
                    {
                        MessageBox.Show("Please choose the icon with 20 x 20 dim.", _caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    string userPath = (Param.Value["UserSheetPath"]);
                    string imgFileName = Path.GetFileName(filename);
                    string toPath = Path.Combine(userPath, imgFileName);
                    File.Copy(filename, toPath, true);
                    Param.WriteMobileAttrib("Icon", toPath);
                    mobileIcon.Image = iconImage;
                }
                catch { }
            }
        }

        private void ddlFileProduceDict_Validated(object sender, EventArgs e)
        {
            try
            {
                _fileProduce = ddlFileProduceDict.Text;
                WriteCss(sender);
            }
            catch { }
        }

        private void ddlFiles_Validated(object sender, EventArgs e)
        {
            WriteCss(sender);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Text == "Display Properties")
            {
                txtPageInside_Validated(txtPageInside, null);
                txtPageOutside_Validated(txtPageOutside, null);
                txtPageTop_Validated(txtPageTop, null);
                txtPageBottom_Validated(txtPageBottom, null);
                txtPageGutterWidth_Validated(txtPageGutterWidth, null);
            }
        }

        private void tsSaveAs_Click(object sender, EventArgs e)
        {
            try
            {
                //_redoundo.Set(Common.Action.Copy, _configurationToolBL.StyleName, null, "", string.Empty);
                if (_configurationToolBL.CopyStyle(stylesGrid, _cssNames))
                {
                    _configurationToolBL.LoadMediaStyle(stylesGrid, _cssNames);
                    _configurationToolBL.SelectRow(stylesGrid, _configurationToolBL.PreviousStyleName);
                    SetInfoTabValue();
                    txtName.Select();
                }

            }
            catch { }
        }
    }
}
