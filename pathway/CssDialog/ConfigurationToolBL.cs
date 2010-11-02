using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using SIL.Tool;


namespace SIL.PublishingSolution
{
    public class ConfigurationToolBL
    {
        #region Private Variables
        private string _cssPath;
        private string _loadType;
        private Dictionary<string, Dictionary<string, string>> _cssClass =
                            new Dictionary<string, Dictionary<string, string>>();
        Dictionary<string, string> standardSize = new Dictionary<string, string>();
        public string Caption = "Pathway Configuration Tool";
        public TraceSwitch _traceOnBL = new TraceSwitch("General", "Trace level for application");
        
        #endregion

        #region Public Variable
        public DataSet DataSetForGrid = new DataSet();

        public string MediaType = string.Empty;
        public string StyleName = string.Empty;
        public int SelectedRowIndex = 0;

        public string AttribFile = "file";
        public string AttribName = "name";
        public string AttribType = "type";
        public string AttribShown = "shown";
        public string AttribApproved = "approvedBy";
        public string AttribPreviewFile1 = "previewfile1";
        public string AttribPreviewFile2 = "previewfile2";

        public string inputTypeBL = string.Empty;

        public string ElementDesc = "description";
        public string ElementComment = "comment";

        public string TypeStandard = "Standard";
        public string TypeCustom = "Custom";
        public string PreviousStyleName = string.Empty;
        public string NewStyleName = string.Empty;

        public string PreviousValue = string.Empty;
        //public string CurrentControl = string.Empty;
        //public string PreviousControl = string.Empty;

        //private string _selectedStyle;
        public string FileName = string.Empty;
        public string FileType = string.Empty;
        public string PreviewFileName1 = string.Empty;
        public string PreviewFileName2 = string.Empty;

        public bool AddMode1;
        public enum ScreenMode { Load, New, View, Edit, Delete, SaveAs,Modify };
        public ScreenMode _screenMode;

        CssTree cssTree = new CssTree();

        //Column Index
        public int ColumnName = 0;
        public int ColumnDescription = 1;
        public int ColumnComment = 2;
        public int ColumnType = 3;
        public int ColumnShown = 4;  // 5 is used for approved by
        public int ColumnFile = 6;
        public int PreviewFile1 = 7;
        public int PreviewFile2 = 8;
        #endregion

        #region Protected Variables
        protected readonly ArrayList _cssNames = new ArrayList();
        //protected string _fileName = string.Empty;
        //protected static ConfigurationToolBL _configurationToolBL = new ConfigurationToolBL();
        //protected TextWriter _writeCss;
        ErrorProvider _errProvider = new ErrorProvider();
        DictionarySetting _ds = new DictionarySetting();
        //DataSet _dataSet = new DataSet();
        UndoRedo _redoundo;
        protected bool _isCreatePreview1;
        protected string _caption = string.Empty;
        //protected string _caption = Caption;
        protected string _redoUndoBufferValue = string.Empty;
        // Undo Redo
        //protected string _currentControl = string.Empty;
        //protected string _previousControl = string.Empty;
        // Undo Redo

        //protected string _previousStyleName = string.Empty;
        protected Color _selectedColor = SystemColors.InactiveBorder; //Color.FromArgb(255, 204, 102);
        protected Color _deSelectedColor = SystemColors.Control;
        protected string _styleName;
        protected string _previousStyleName;
        protected string _fileProduce = "One";
        public string MediaTypeEXE;
        public string StyleEXE;
        protected string _lastSelectedLayout = string.Empty;
        //protected string StyleName;
        protected string _selectedStyle;
        TabPage tabdic = new TabPage();
        TabPage tabmob = new TabPage();
        protected TraceSwitch _traceOn = new TraceSwitch("General", "Trace level for application");

        private ConfigurationTool cTool;
        #endregion

        #region Constructor
        public ConfigurationToolBL()
        {
            standardSize["595x842"] = "A4";
            standardSize["420x595"] = "A5";
            standardSize["459x649"] = "C5";
            standardSize["298x420"] = "A6";
            standardSize["612x792"] = "Letter";
            standardSize["396x612"] = "Half letter";
            standardSize["378x594"] = "5.25in x 8.25in";
            standardSize["418x626"] = "5.8in x 8.7in";
            standardSize["432x648"] = "6in x 9in";

            _screenMode = ScreenMode.Load;  
        }

        //public void SetDefaultCSSValue(string cssPath, string loadType)
        //{
        //    _cssPath = cssPath;
        //    _loadType = loadType;
        //    ParseCSS();
        //}
        #endregion

        #region Properties

        public string MarginLeft
        {
            get
            {
                string task = "@page";
                string key = "margin-left";
                return GetValue(task, key, "0");
            }
        }

        public string MarginRight
        {
            get
            {
                string task = "@page";
                string key = "margin-right";
                return GetValue(task, key, "0");
            }
        }

        public string MarginTop
        {
            get
            {
                string task = "@page";
                string key = "margin-top";
                return GetValue(task, key, "0");
            }
        }

        public string MarginBottom
        {
            get
            {
                string task = "@page";
                string key = "margin-bottom";
                return GetValue(task, key, "0");
            }
        }

        public string RunningHeader
        {
            get
            {
                string defaultValue = string.Empty;
                {
                    if (_loadType == "Dictionary")
                    {
                        string task = "@page:left-top-right";
                        string key = "content";
                        string result = GetValue(task, key, "false");
                        if (result.IndexOf("page") > 0)
                        {
                            return "Mirrored";
                        }
                        defaultValue = "Every Page";
                    }
                    else
                    {
                        string task = "@page:left-top-left";
                        string key = "content";
                        string result = GetValue(task, key, "false");
                        if (result.IndexOf("verse") > 0)
                        {
                            return "Genesis 1:1";
                        }

                        task = "@page:left-top-left";
                        result = GetValue(task, key, "false");
                        if (result.IndexOf("chapter") > 0)
                        {
                            return "Genesis 1";
                        }

                        task = "@page-top-center";
                        result = GetValue(task, key, "false");
                        if (result.IndexOf("verse") > 0)
                        {
                            return "Genesis 1:1-2:1";
                        }

                        defaultValue = "Genesis 1-2";
                    }
                }
                return defaultValue;

            }
        }

        public string ColumnCount
        {
            get
            {
                string task = "letData";
                if (_loadType == "Scripture")
                {
                    task = "columns";
                }
                string key = "column-count";
                return GetValue(task, key, "1");
            }
        }

        public string ColumnRule
        {
            get
            {
                string task = "letData";
                if (_loadType == "Scripture")
                {
                    task = "columns";
                }
                string key = "column-rule-width";
                string result = GetValue(task, key, "0");
                return result != "0" ? "Yes" : "No";
            }
        }

        public string GutterWidth
        {
            get
            {
                string task = "letData";
                if (_loadType == "Scripture")
                {
                    task = "columns";
                }
                string key = "column-gap";
                string result = GetValue(task, key, "18");
                return result.Length > 0 ? result : "18";
            }
        }

        public string PageSize
        {
            get
            {
                string task = "@page";
                string key = "page-width";
                string key1 = "page-height";
                string width = GetValue(task, key, "612");
                width = Math.Round(double.Parse(width)).ToString();
                string height = GetValue(task, key1, "792");
                height = Math.Round(double.Parse(height)).ToString();
                string pageSize = PageSize1(width, height);
                return pageSize;
            }
        }

        public string Leading
        {
            get
            {
                string task = "entry";
                if (_loadType == "Scripture")
                {
                    task = "Paragraph";
                }
                string key = "line-height";
                return GetValue(task, key, "No Change");
            }
        }

        public string FontSize
        {
            get
            {
                string task = "entry";
                if (_loadType == "Scripture")
                {
                    task = "Paragraph";
                }
                string key = "font-size";
                return GetValue(task, key, "No Change");
            }
        }

        public string JustifyUI
        {
            get
            {
                string task = "entry";
                if (_loadType == "Scripture")
                {
                    task = "scrSection";
                }
                string key = "text-align";
                string align = GetValue(task, key, "No");
                if (align == "justify")
                {
                    return "Yes";
                }
                return "No";
            }
        }

        public string VerticalJustify
        {
            get
            {
                string task = "@page";
                string key = "-ps-vertical-justification";
                string align = GetValue(task, key, "Top");
                return align;
            }
        }



        public string Picture
        {
            get
            {
                string task = "pictureRight";
                string key = "display";
                string display = GetValue(task, key, "No");
                if (display == "none")
                {
                    return "No";
                }
                return "Yes";
            }
        }

        public string Sense
        {
            get
            {
                string task = "sense";
                if (_cssClass.ContainsKey(task) && _cssClass[task].ContainsKey("class-margin-left"))
                {
                    return "Bullet";
                }
                return "No Change";
            }
        }

        public string FileProduced
        {
            get
            {
                string task = "@page";
                string key = "-ps-fileproduce";
                string file = GetValue(task, key, "One");
                return file.Replace("\"", "");
            }
        }
        #endregion

        #region Methods
        public void SetClassReference(ConfigurationTool configTool)
        {
            cTool = configTool;
        }

        protected void SetInputTypeButton()
        {
            Trace.WriteLineIf(_traceOnBL.Level == TraceLevel.Verbose, "ConfigurationTool: SetInputTypeButton");
            if (inputTypeBL.ToLower() == "scripture")
            {
                cTool.BtnScripture.BackColor = _selectedColor;
                cTool.BtnDictionary.BackColor = _deSelectedColor;
                cTool.BtnScripture.Focus();
                cTool.LblSenseLayout.Visible = false;
                cTool.DdlSense.Visible = false;
            }
            else
            {
                cTool.BtnDictionary.BackColor = _selectedColor;
                cTool.BtnScripture.BackColor = _deSelectedColor;
                cTool.BtnDictionary.Focus();
                cTool.LblSenseLayout.Visible = true;
                cTool.DdlSense.Visible = true;
            }
        }

        public string AssemblyFileVersion
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyFileVersionAttribute)attributes[0]).Version;
                //return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        /// <summary>
        /// Method to add the file location to copy the attached files.
        /// </summary>
        /// <param name="projType">Dictionary / Scipture</param>
        /// <param name="MailBody">Existing content</param>
        /// <returns></returns>
        protected static string GetMailBody(string projType, string MailBody)
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

        protected static string GetProjType()
        {
            string projType = string.Empty;
            if (Param.Value["InputType"] != null)
            {
                projType = Param.Value["InputType"];
            }
            return projType;
        }

        protected void AddNewRow()
        {
            DataRow row = DataSetForGrid.Tables["Styles"].NewRow(); ;
            DataSetForGrid.Tables["Styles"].Rows.Add(row);
            cTool.StylesGrid.Refresh();
            SelectRow(cTool.StylesGrid, "");
            ShowInfoValue();
            string newName = GetNewStyleName(_cssNames, "new");
            cTool.TxtName.Text = newName;
            cTool.LblInfoCaption.Text = newName;
        }

        protected void SetFocusToName()
        {
            Trace.WriteLineIf(_traceOnBL.Level == TraceLevel.Verbose, "ConfigurationTool: SetFocusToName");
            cTool.TabControl1.SelectedTab = cTool.TabControl1.TabPages[0];
            cTool.TxtName.Focus();
        }

        protected void ShowDataInGrid()
        {
            Trace.WriteLineIf(_traceOnBL.Level == TraceLevel.Verbose, "ConfigurationTool: ShowDataInGrid");
            _selectedStyle = Param.Value["LayoutSelected"];
            _redoundo.SettingOutputPath = Param.SettingOutputPath;
            cTool.TabControl1.SelectedTab = cTool.TabControl1.TabPages[0];
            cTool.LblType.Text = inputTypeBL;
            ShowStyleInGrid(cTool.StylesGrid, _cssNames);
            GridColumnWidth(cTool.StylesGrid);
            PreviousValue = cTool.TxtName.Text;
        }

        protected void SetSideBar()
        {
            cTool.BtnPaper.BackColor = _deSelectedColor;
            cTool.BtnMobile.BackColor = _deSelectedColor;
            cTool.BtnWeb.BackColor = _deSelectedColor;
            cTool.BtnOthers.BackColor = _deSelectedColor;

            cTool.BtnPaper.FlatAppearance.BorderSize = 0;
            cTool.BtnMobile.FlatAppearance.BorderSize = 0;
            cTool.BtnWeb.FlatAppearance.BorderSize = 0;
            cTool.BtnOthers.FlatAppearance.BorderSize = 0;
            if (MediaType == "paper")
            {
                cTool.BtnPaper.BackColor = _selectedColor;
                cTool.BtnPaper.FlatAppearance.BorderSize = 1;
            }
            else if (MediaType == "mobile")
            {
                cTool.BtnMobile.BackColor = _selectedColor;
                cTool.BtnMobile.FlatAppearance.BorderSize = 1;
            }
            //else if (MediaType == "web")
            //{
            //    cTool.BtnWeb.BackColor = _selectedColor;
            //    cTool.BtnWeb.FlatAppearance.BorderSize = 1;
            //}
            //else if (MediaType == "others")
            //{
            //    cTool.BtnOthers.BackColor = _selectedColor;
            //    cTool.BtnOthers.FlatAppearance.BorderSize = 1;
            //}
        }

        public void WriteCss()
        {
            if ((_screenMode != ScreenMode.Modify) || (FileType.ToLower() == "standard")) return;
            StreamWriter writeCss = null;
            //string file1;
            //string attribValue = Common.GetTextValue(sender, out file1);
            //if (PreviousValue == attribValue) return;

            try
            {
                string path = Param.Value["UserSheetPath"]; // all user path
                string file = Common.PathCombine(path, FileName);
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
                string key = cTool.DdlJustified.Text;
                WriteAtImport(writeCss, attribute, key);

                attribute = "VerticalJustify";
                key = cTool.DdlVerticalJustify.Text;
                WriteAtImport(writeCss, attribute, key);

                attribute = "Page Size";
                key = cTool.DdlPagePageSize.Text;
                WriteAtImport(writeCss, attribute, key);

                attribute = "Columns";
                key = cTool.DdlPageColumn.Text;
                WriteAtImport(writeCss, attribute, key);

                attribute = "Font Size";
                key = cTool.DdlFontSize.Text;
                WriteAtImport(writeCss, attribute, key);

                attribute = "Leading";
                key = cTool.DdlLeading.Text;
                WriteAtImport(writeCss, attribute, key);

                attribute = "Pictures";
                key = cTool.DdlPicture.Text;
                WriteAtImport(writeCss, attribute, key);

                attribute = "Running Head";
                key = cTool.DdlRunningHead.Text;
                WriteAtImport(writeCss, attribute, key);

                attribute = "Rules";
                key = cTool.DdlRules.Text;
                WriteAtImport(writeCss, attribute, key);

                attribute = "Sense";
                key = cTool.DdlSense.Text;
                WriteAtImport(writeCss, attribute, key);

                //Writing TextBox Values into Css
                if (cTool.TxtPageGutterWidth.Text.Length > 0)
                {
                    value["column-gap"] = cTool.TxtPageGutterWidth.Text;
                    WriteCssClass(writeCss, "letData", value);
                }

                value.Clear();
                value["margin-top"] = cTool.TxtPageTop.Text;
                value["margin-right"] = cTool.TxtPageOutside.Text;
                value["margin-bottom"] = cTool.TxtPageBottom.Text;
                value["margin-left"] = cTool.TxtPageInside.Text;
                value["-ps-fileproduce"] = "\"" + _fileProduce + "\"";
                WriteCssClass(writeCss, "page", value);
                writeCss.Flush();
                writeCss.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sorry, your recent changes cannot be saved because Pathway cannot find the stylesheet file '" + ex.Message + "'", _caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                //writeCss.Close();
            }
            _screenMode = ScreenMode.Edit;  
        }

        

        //private void UpdateMobileAtrrib(string attribName, string attribValue)
        //{
        //    string searchStyleName = StyleName;
        //    XmlNode node = Param.GetItem("//stylePick/styles/mobile/style[@name='" + searchStyleName + "']/styleProperty[@name='" + attribName + "']");
        //    Param.SetAttrValue(node, "value", attribValue);
        //    Param.Write();
        //}

        /// <summary>
        /// transfers grid row values to InfoPanel
        ///
        /// 
        /// </summary
        protected void ShowInfoValue()
        {
            if (!(_screenMode == ScreenMode.View || _screenMode == ScreenMode.Edit)) return;
            Trace.WriteLineIf(_traceOnBL.Level == TraceLevel.Verbose, "ConfigurationTool: ShowInfoValue");
            if (cTool.StylesGrid.RowCount > 0)
            {
                //try
                //{
                StyleName = cTool.StylesGrid[ColumnName, SelectedRowIndex].Value.ToString();
                PreviewFileName1 = cTool.StylesGrid[PreviewFile1, SelectedRowIndex].Value.ToString();
                PreviewFileName2 = cTool.StylesGrid[PreviewFile2, SelectedRowIndex].Value.ToString();
                //}
                //catch (Exception)
                //{
                // SelectedRowIndex--;                    
                // StyleName =  cTool.StylesGrid[ColumnName, SelectedRowIndex].Value.ToString();

                // SelectRow( cTool.StylesGrid, StyleName);
                //} 
                cTool.TxtName.Text = cTool.LblInfoCaption.Text = StyleName;
                FileName = cTool.StylesGrid[ColumnFile, SelectedRowIndex].Value.ToString();
                FileType = cTool.StylesGrid[ColumnType, SelectedRowIndex].Value.ToString();
                cTool.TxtCss.Text = cTool.StylesGrid[ColumnDescription, SelectedRowIndex].Value.ToString();
                cTool.TxtDesc.Text = cTool.StylesGrid[ColumnDescription, SelectedRowIndex].Value.ToString();
                cTool.TxtComment.Text = cTool.StylesGrid[ColumnComment, SelectedRowIndex].Value.ToString();
                bool check = cTool.StylesGrid[ColumnShown, SelectedRowIndex].Value.ToString().ToLower() == "yes" ? true : false;
                cTool.ChkAvailable.Checked = check;
                cTool.TxtApproved.Text = cTool.StylesGrid[AttribApproved, SelectedRowIndex].Value.ToString();
                string type = cTool.StylesGrid[ColumnType, SelectedRowIndex].Value.ToString();
                if (type == TypeStandard)
                {
                    EnableDisablePanel(false);
                    //tsDelete.Enabled = false;
                    cTool.TsPreview.Enabled = true;
                    cTool.TxtApproved.Visible = true;
                    cTool.LblApproved.Visible = true;
                }
                else
                {
                    EnableDisablePanel(true);
                    //tsPreview.Enabled = false;
                    cTool.TxtApproved.Visible = false;
                    cTool.LblApproved.Visible = false;
                }
            }

            if (cTool.TabControl1.SelectedIndex == 1)
                ShowCSSValue();
            _screenMode = ScreenMode.Edit;
        }

        /// <summary>
        /// Fills Values in Display property Tab and Properties Tab
        /// </summary>
        protected void ShowCSSValue()
        {
            _screenMode = ScreenMode.View;
            _errProvider.Clear();
            if (cTool.TxtName.Text.Length <= 0) return;
            string path = Param.StylePath(cTool.TxtName.Text);
            //if (_cssClass.Count == 0) // Add
                ParseCSS(path, Param.Value["InputType"]);
            //SetDefaultCSSValue(path, Param.Value["InputType"]);

            double left = Math.Round(double.Parse(MarginLeft), 0);
            cTool.TxtPageInside.Text = left + "pt";
            double right = Math.Round(double.Parse(MarginRight), 0);
            cTool.TxtPageOutside.Text = right + "pt";
            double top = Math.Round(double.Parse(MarginTop), 0);
            cTool.TxtPageTop.Text = top + "pt";
            double bottom = Math.Round(double.Parse(MarginBottom), 0);
            cTool.TxtPageBottom.Text = bottom + "pt";

            cTool.TxtPageGutterWidth.Text = GutterWidth;
            if (GutterWidth.IndexOf('%') == -1)
            {
                if (cTool.TxtPageGutterWidth.Text.Length > 0)
                    cTool.TxtPageGutterWidth.Text = cTool.TxtPageGutterWidth.Text + "pt";
            }
            cTool.DdlPageColumn.SelectedItem = ColumnCount;
            cTool.DdlFontSize.SelectedItem = FontSize;
            cTool.DdlLeading.SelectedItem = Leading;
            cTool.DdlPicture.SelectedItem = Picture;
            cTool.DdlJustified.SelectedItem = JustifyUI;
            cTool.DdlPagePageSize.SelectedItem = PageSize;
            cTool.DdlRunningHead.SelectedItem = RunningHeader;
            cTool.DdlRules.SelectedItem = ColumnRule;
            cTool.DdlSense.SelectedItem = Sense;
            cTool.DdlVerticalJustify.SelectedItem = VerticalJustify;
            try
            {
                if (inputTypeBL.ToLower() == "scripture" && MediaType.ToLower() == "mobile")
                {
                    string filePath = string.Empty;
                   if(File.Exists(Param.SettingOutputPath))
                   {
                       filePath = Param.SettingOutputPath;
                   }
                   else if (File.Exists(Param.SettingPath))
                   {
                       filePath = Param.SettingPath;
                   }
                   XmlNodeList baseNode1 = Param.GetItems("//styles/" + MediaType + "/style[@name='" + StyleName + "']/styleProperty");
                   foreach (XmlNode VARIABLE in baseNode1)
                   {
                       string attribName = VARIABLE.Attributes["name"].Value;
                       string attribValue = VARIABLE.Attributes["value"].Value;
                       if (attribName.ToLower() == "fileproduced")
                       {
                           cTool.DdlFiles.SelectedItem = attribValue;
                       }
                       else if (attribName.ToLower() == "redletter")
                       {
                           cTool.DdlRedLetter.SelectedItem = attribValue;
                       }
                       else if (attribName.ToLower() == "information")
                       {
                           cTool.TxtInformation.Text = attribValue;
                       }
                       else if (attribName.ToLower() == "copyright")
                       {
                           cTool.TxtCopyright.Text = attribValue;
                       }

                   }  
                    SetMobileSummary(null, null);
                }
                else
                {
                    cTool.DdlFileProduceDict.SelectedItem = FileProduced.Trim();
                    ShowCssSummary();
                }
            }
            catch
            {
            }
            _screenMode = ScreenMode.Edit;
        }

        ///// <summary>
        ///// If the value of margins are invalid at load time, the values are shown in red color(TD-1331).
        ///// </summary>
        //protected void SetTextColor()
        //{
        //    CompareMarginValues(cTool.TxtPageTop, 18);
        //    CompareMarginValues(cTool.TxtPageInside, 18);
        //    CompareMarginValues(cTool.TxtPageOutside, 18);
        //    CompareMarginValues(cTool.TxtPageBottom, 18);
        //}

        /// <summary>
        /// Fills Values in Display property Tab
        /// 
        /// </summary>
        protected void PopulateFeatureSheet()
        {
            Trace.WriteLineIf(_traceOnBL.Level == TraceLevel.Verbose, "ConfigurationTool: PopulateFeatureSheet");
            TreeView TvFeatures = new TreeView();
            PopulateFeatureLists(TvFeatures);
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
                                cTool.DdlPagePageSize.Items.Add(ctn.Text);
                                break;

                            case "Columns":
                                cTool.DdlPageColumn.Items.Add(ctn.Text);
                                break;

                            case "Leading":
                                cTool.DdlLeading.Items.Add(ctn.Text);
                                break;

                            case "Font Size":
                                cTool.DdlFontSize.Items.Add(ctn.Text);
                                break;

                            case "Running Head":
                                cTool.DdlRunningHead.Items.Add(ctn.Text);
                                break;

                            case "Rules":
                                cTool.DdlRules.Items.Add(ctn.Text);
                                break;

                            case "Pictures":
                                cTool.DdlPicture.Items.Add(ctn.Text);
                                break;

                            case "Sense":
                                cTool.DdlSense.Items.Add(ctn.Text);
                                break;

                            case "Justified":
                                cTool.DdlJustified.Items.Add(ctn.Text);
                                break;

                            case "VerticalJustify":
                                cTool.DdlVerticalJustify.Items.Add(ctn.Text);
                                break;

                            case "FileProduced":
                                cTool.DdlFiles.Items.Add(ctn.Text);
                                cTool.DdlFileProduceDict.Items.Add(ctn.Text);
                                break;

                            case "RedLetter":
                                cTool.DdlRedLetter.Items.Add(ctn.Text);
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
        public void ShowCssSummary()
        {
            try
            {
                string leading = (cTool.DdlLeading.Text.Length > 0) ? " Line Spacing " + cTool.DdlLeading.Text + ", " : " ";
                string fontSize = (cTool.DdlFontSize.Text.Length > 0) ? " Base FontSize - " + cTool.DdlFontSize.Text + ", " : "";

                string rules = cTool.DdlRules.Text == "Yes"
                                   ? "With Divider Lines, "
                                   : "Without Divider Lines, ";
                string justified = cTool.DdlJustified.Text == "Yes"
                                       ? "Justified, "
                                       : "";
                string picture = cTool.DdlPicture.Text == "Yes"
                                     ? "With picture "
                                     : "Without picture ";
                string pageSize = (cTool.DdlPagePageSize.Text.Length > 0) ? cTool.DdlPagePageSize.Text + "," : "";
                string pageColumn = (cTool.DdlPageColumn.Text.Length > 0) ? cTool.DdlPageColumn.Text + " Column(s), " : "";
                string gutter = cTool.TxtPageGutterWidth.Text.Length > 0 ? "Column Gap - " + cTool.TxtPageGutterWidth.Text + ", " : "";

                string marginTop = cTool.TxtPageTop.Text.Length > 0 ? "Margin Top - " + cTool.TxtPageTop.Text + ", " : "";
                string marginBottom = cTool.TxtPageBottom.Text.Length > 0 ? "Margin Bottom - " + cTool.TxtPageBottom.Text + ", " : "";
                string marginInside = cTool.TxtPageInside.Text.Length > 0 ? "Margin Inside - " + cTool.TxtPageInside.Text + ", " : "";
                string marginOutside = cTool.TxtPageOutside.Text.Length > 0 ? "Margin Outside - " + cTool.TxtPageOutside.Text + ", " : "";
                string sense = (cTool.DdlSense.Text.Length > 0) ? " Sense Layout - " + cTool.DdlSense.Text + "," : "";
                string combined = pageSize + " " +
                                  pageColumn + "  " +
                                  gutter + " " +
                                  marginTop + " " +
                                  marginBottom + " " +
                                  marginInside + " " +
                                  marginOutside + " " +
                                  leading + " " +
                                  fontSize + " " +
                                  cTool.DdlRunningHead.Text + " " +
                                  rules + " " +
                                  justified + " " +
                                  sense + " " +
                                  picture + ".";
                cTool.TxtCss.Text = combined;
            }
            catch { }
        }

        protected void EnableToolStripButtons(bool enable)
        {
            string selectedTypeValue = cTool.StylesGrid[ColumnType, SelectedRowIndex].Value.ToString();
            if (selectedTypeValue != TypeStandard)
            {
                cTool.TsNew.Enabled = enable;
                cTool.TsDelete.Enabled = enable;
                cTool.TsSaveAs.Enabled = enable;
                //tsPreview.Enabled = true;
            }
        }

        /// <summary>
        /// Enable or Disable the Panel controls
        /// </summary>
        /// <param name="IsEnable">True or False</param>
        protected void EnableDisablePanel(bool IsEnable)
        {
            cTool.TsDelete.Enabled = IsEnable;
            cTool.TabDisplay.Enabled = IsEnable;
            cTool.TabMobile.Enabled = IsEnable;
            cTool.TabInfo.Enabled = IsEnable;
        }

        protected void setDefaultInputType()
        {
            //Param.SetValue(Param.InputType, ""); // loading settingsxml.
            //Param.LoadSettings();
            Param.SetValue(Param.InputType, inputTypeBL); // last input type
            Param.Write();
            Param.CopySchemaIfNecessary();
        }

        protected void setLastSelectedLayout()
        {
            //Check and move
            string layoutName = cTool.TxtName.Text;
            if (layoutName.Length == 0)
                layoutName = _lastSelectedLayout;
            Param.SetValue(Param.LayoutSelected, layoutName); // last layout
            Param.Write();
        }

        public void SideBar()
        {
            try
            {
                WriteMedia();
                setLastSelectedLayout();
                LoadParam();
                SetSideBar();
                ShowDataInGrid();
                _redoundo.Reset();
                SetMobilePropertyTab();
            }
            catch
            {
            }
        }

        protected void SetMobilePropertyTab()
        {
            cTool.TabControl1.TabPages.Remove(cTool.TabControl1.TabPages[1]);
            if (MediaType == "mobile")
            {

                cTool.TabControl1.TabPages.Add(tabmob);
                Dictionary<string, string> mobilefeature = Param.GetItemsAsDictionary("//mobileProperty/mobilefeature");
                if (mobilefeature.Count > 0)
                {
                    string key = "FileProduced";
                    if (mobilefeature.ContainsKey(key))
                    {
                        cTool.DdlFiles.Text = mobilefeature[key];
                    }
                    key = "RedLetter";
                    if (mobilefeature.ContainsKey(key))
                    {
                        cTool.DdlRedLetter.Text = mobilefeature[key];
                    }
                    key = "Information";
                    if (mobilefeature.ContainsKey(key))
                    {
                        cTool.TxtInformation.Text = mobilefeature[key];
                    }
                    key = "Copyright";
                    if (mobilefeature.ContainsKey(key))
                    {
                        cTool.TxtCopyright.Text = mobilefeature[key];
                    }
                    key = "Icon";
                    if (mobilefeature.ContainsKey(key))
                    {
                        try
                        {
                            string iconPath = mobilefeature[key];
                            cTool.MobileIcon.Load(iconPath);
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
                cTool.TabControl1.TabPages.Add(tabdic);
                ShowCssSummary();
            }
        }

        //Note - Check This
        protected void IsLayoutSelectedStyle()
        {
            if (_selectedStyle == PreviousValue) // LayoutSelected Value
            {
                Param.SetValue(Param.LayoutSelected, StyleName);
            }
        }

        #region ValidateStartsWithAlphabet(string stringValue)
        /// <summary>
        /// Validate a given string is Starts with alphabets or not 
        /// </summary>
        /// <param name="stringValue">string</param>
        /// <returns>True/False</returns>
        protected bool ValidateStyleName(string stringValue)
        {
            string result = string.Empty;
            bool valid = true;
            if (!Common.ValidateStartsWithAlphabet(stringValue))
            {
                result = "Style name should not be empty. Please enter the valid name";
            }
            else
            {
                foreach (char ch in stringValue)
                {
                    if (!(ch == '-' || ch == '_' || ch == ' ' || ch == '.' || ch == '(' || ch == ')' ||
                        (ch >= 48 && ch <= 57) || (ch >= 65 && ch <= 90) || (ch >= 97 && ch <= 122)))
                    {
                        result = "Please avoid " + ch + " in the Style name";
                        break;
                    }
                }
            }
            if (result != string.Empty)
            {
                MessageBox.Show(result, _caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                valid = false;
            }
            return valid;
        }
        #endregion
        protected bool NoDuplicateStyleName()
        {
            bool result = true;
            if (cTool.TxtName.Text.ToLower() != PreviousStyleName.ToLower()) //Is new styleName?
            {
                // Add - Check whether the same name already exist
                // Edit- Check it, while the name changed in Stylename textBox.
                if (IsNameExists(cTool.StylesGrid, cTool.TxtName.Text))
                {
                    MessageBox.Show("Stylesheet Name [" + cTool.TxtName.Text + "] already exists", _caption,
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    result = false;
                    //cTool.TxtName.Focus();
                }
            }
            return result;
        }

        protected bool SetUI(ModifyData control)
        {
            bool success = true;
            string controlName = control.FileName;
            string controlText = control.ControlText;

            Control[] ctls = cTool.TabInfo.Controls.Find(controlName, true);
            if (ctls.Length > 0)
            {
                Control ctl = ctls[0];
                if (ctl.Text == controlText)
                {
                    success = false;
                }
                else
                {
                    SelectRow(cTool.StylesGrid, control.EditStyleName);
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
                UpdateGrid(ctl, cTool.StylesGrid);
            }
            return success;
        }

        protected static int GetPageSize(string paperSize, string dimension)
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

        protected static int GetDefaultValue(string value)
        {
            if (value.Length > 0 && Common.ValidateNumber(value.Replace("pt", "")))
            {
                return int.Parse(value.Replace("pt", ""));
            }
            return 0;

        }

        protected void Preview_PDF_Export(string inputPath)
        {
            string _backendPath = Common.ProgInstall;
            Backend.Load(_backendPath);
            PublicationInformation _projectInfo = new PublicationInformation();
            _projectInfo.DictionaryOutputName = StyleName + "_" + FileName;
            //_projectInfo.IsOpenOutput = false;
            _projectInfo.IsOpenOutput = true;
            string sampleFilePath = Path.GetDirectoryName(Path.GetDirectoryName(Common.GetPSApplicationPath()));
            string xthmlPath = Common.PathCombine(sampleFilePath, "BuangExport.xhtml");
            _projectInfo.DefaultXhtmlFileWithPath = xthmlPath;
            string cssPath = Common.PathCombine(inputPath, FileName);
            _projectInfo.DefaultCssFileWithPath = cssPath;
            Backend.Launch("Pdf", _projectInfo);
        }

        //protected static void CompareMarginValues(Control txtBox, int lowestBound)
        //{
        //    int marginInPoint = int.Parse(Common.UnitConverter(cTool.TxtBox.Text, "pt"));
        //    if (marginInPoint < lowestBound)
        //    {
        //        cTool.TxtBox.ForeColor = Color.Red;
        //    }
        //    else
        //    {
        //        cTool.TxtBox.ForeColor = Color.Black;
        //    }
        //}

        /// <summary>
        /// When the condigurationtool is run from EXE, the mediatype has changed 
        /// by Prinvia dialog selection
        /// </summary>
        protected void SetMediaType()
        {
            Trace.WriteLineIf(_traceOnBL.Level == TraceLevel.Verbose, "ConfigurationTool: SetMediaType");
            try
            {
                if (MediaTypeEXE.Length != 0)
                {
                    MediaType = MediaTypeEXE.ToLower();
                    SideBar();
                    SelectRow(cTool.StylesGrid, StyleEXE);
                }
                else
                {
                    SetSideBar();
                }
            }
            catch { }
        }

        /// <summary>
        /// removes selected row in grid
        /// </summary>
        public void RemoveXMLNode(string styleName)
        {
            string fileName;
            if (File.Exists(Param.SettingOutputPath))
                fileName = Param.SettingOutputPath;
            else
                fileName = Param.SettingPath;

            string xPath = "//styles/" + MediaType + "/style[@name='" + styleName + "']";
            XmlNode removableNode = Param.GetItem(xPath);
            if (removableNode == null) return;
            string cssFile = removableNode.Attributes["file"].Value;
            Param.RemoveXMLNode(fileName, xPath);
            try
            {
                // .Css file deletion
                string path = Param.Value["UserSheetPath"]; //Common.GetAllUserPath(); 
                string file = Common.PathCombine(path, cssFile);
                if (File.Exists(file))
                    File.Delete(file);
            }
            catch
            {
            }
        }

        protected void ParseCSS(string cssPath, string loadType)
        {
            _cssPath = cssPath;
            _loadType = loadType;
            Common.SamplePath = Param.Value["SamplePath"].Replace("Samples", "Styles");
            if (string.IsNullOrEmpty(_cssPath)) return;
            _cssClass = cssTree.CreateCssProperty(_cssPath, false);
        }

        public string GetValue(string task, string key, string defaultValue)
        {
            if (_cssClass.ContainsKey(task) && _cssClass[task].ContainsKey(key))
            {
                return _cssClass[task][key];
            }
            return defaultValue;
        }

        public string PageSize1(string width, string height)
        {
            string dims = width + "x" + height;
            string pageSize = standardSize.ContainsKey(dims) ? standardSize[dims] : "Custom";
            return pageSize;
        }

        public string GetNewStyleName(ArrayList cssNames, string mode)
        {
            string preferedName = "CustomSheet-1";
            if (mode == "new")
            {
                if (cssNames.Count > 0)
                {
                    preferedName = GetNewStyleCount(cssNames, preferedName);
                }
            }
            else
            {
                preferedName = StyleName;
                if (StyleName.IndexOf('(') == -1)
                {
                    preferedName = "Copy of " + StyleName;
                }
                if (cssNames.Count > 0)
                {
                    if (cssNames.Contains(preferedName))
                    {
                        preferedName = GetDirCount(cssNames, preferedName);
                    }
                }
            }
            return preferedName;
        }

        protected static string GetNewStyleCount(ArrayList cssNames, string preferedName)
        {
            int temp = 1;
            int max = 0;
            foreach (string styleName in cssNames)
            {
                if (styleName.IndexOf("Copy") >= 0)
                    continue;

                string[] ss = styleName.Split('-');
                try
                {
                    temp = int.Parse(ss[1]);
                }
                catch
                {
                }
                if (max < temp) { max = temp; }
                preferedName = ss[0] + "-" + (max + 1);
            }
            if (cssNames.Contains(preferedName))
            {
                preferedName = GetNewStyleCount(cssNames, preferedName);
            }
            return preferedName;
        }

        protected static string GetDirCount(ArrayList cssNames, string preferedName)
        {
            int oldValue = 1;
            if (preferedName.IndexOf('(') == -1)
            {
                preferedName = preferedName.Replace("Copy of", "Copy(2) of");
                if (!cssNames.Contains(preferedName))
                {
                    return preferedName;
                }
            }
            int startPos = preferedName.IndexOf('(');
            int endPos = preferedName.IndexOf(')');
            if (startPos > 0)
                oldValue = int.Parse(preferedName.Substring(startPos + 1, ((endPos - 1) - (startPos))));
            int newValue = oldValue + 1;
            preferedName = preferedName.Replace(oldValue.ToString(), newValue.ToString());
            if (cssNames.Contains(preferedName))
            {
                preferedName = GetDirCount(cssNames, preferedName);
            }
            return preferedName;
        }

        /// <summary>
        /// When data is typed in the info tab, the entry on the grid is updated
        /// </summary>
        public void UpdateGrid(Control myControl, DataGridView grid)
        {
            if (SelectedRowIndex >= 0)
            {
                switch (myControl.Name)
                {
                    case "txtDesc":
                        grid[ColumnDescription, SelectedRowIndex].Value = myControl.Text;
                        break;
                    case "txtComment":
                        grid[ColumnComment, SelectedRowIndex].Value = myControl.Text;
                        break;
                    case "txtName":
                        if (_screenMode == ScreenMode.Modify)
                            grid[ColumnName, SelectedRowIndex].Value = myControl.Text;
                        break;
                    default:
                        CheckBox checkBox = (CheckBox)myControl;
                        grid[ColumnShown, SelectedRowIndex].Value = checkBox.Checked ? "Yes" : "No";
                        break;
                }
                _screenMode = ScreenMode.Modify;   
            }
        }

        public bool CopyStyle(DataGridView grid, ArrayList cssNames)
        {
            var currentRow = grid.Rows[SelectedRowIndex];
            if (currentRow == null) return false;
            //var currentDescription = currentRow.Cells[ColumnDescription].Value.ToString();
            var currentApprovedBy = grid[AttribApproved, SelectedRowIndex].Value.ToString();
            string type = grid[ColumnType, SelectedRowIndex].Value.ToString();
            PreviousStyleName = GetNewStyleName(cssNames, "copy");
            if (PreviousStyleName.Length > 100)
            {
                MessageBox.Show("Styles should not be greater than 100 characters.", Caption, MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return false;
            }
            var currentDescription = "Based on " + currentApprovedBy + " stylesheet " + StyleName;
            Param.SaveSheet(PreviousStyleName, Param.StylePath(FileName), currentDescription, type);
            XmlNode baseNode = Param.GetItem("//styles/" + MediaType + "/style[@name='" + PreviousStyleName + "']");
            Param.SetAttrValue(baseNode, AttribType, TypeCustom);
            //To add StyleProperty for Mobile media
            if (inputTypeBL.ToLower() == "scripture" && MediaType.ToLower() == "mobile")
            {
                XmlNodeList mobileBaseNode =
                    Param.GetItems("//styles/" + MediaType + "/style[@name='" +
                                   grid[AttribName, SelectedRowIndex].Value + "']/styleProperty");
                foreach (XmlNode stylePropertyNode in mobileBaseNode)
                {
                    baseNode.AppendChild(stylePropertyNode.Clone());
                }
            }
            Param.Write();
            return true;
        }

        protected void AddStyleInXML(DataGridView grid, ArrayList cssNames)
        {
            var currentRow = grid.Rows[SelectedRowIndex];
            if (currentRow == null) return;
            var currentDescription = currentRow.Cells[ColumnDescription].Value.ToString();
            string type = grid[ColumnType, SelectedRowIndex].Value.ToString();
            NewStyleName = GetNewStyleName(cssNames, "new");
            Param.SaveSheet(NewStyleName, Param.StylePath(StyleName), currentDescription, type);
            XmlNode baseNode = Param.GetItem("//styles/" + MediaType + "/style[@name='" + NewStyleName + "']");
            Param.SetAttrValue(baseNode, AttribType, TypeCustom);
            Param.Write();
        }

        /// <summary>
        ///
        /// </summary>
        protected void LoadParam()
        {
            Trace.WriteLineIf(_traceOnBL.Level == TraceLevel.Verbose, "ConfigurationToolBL: LoadParam");
            Param.SetValue(Param.InputType, inputTypeBL); // Dictionary or Scripture
            Param.LoadSettings();
            MediaType = Param.MediaType;
        }

        public void WriteAtImport(StreamWriter writeCss, string attribute, string key)
        {
            if (key.Length == 0) return;
            if (Param.featureList.ContainsKey(attribute))
            {
                var values = Param.featureList[attribute];
                string fileName = values[key];
                writeCss.WriteLine("@import \"" + fileName + "\";");
            }
        }

        public void PopulateFeatureLists(TreeView TvFeatures)
        {
            string defaultSheet = Param.DefaultValue.ContainsKey(Param.LayoutSelected) ? Param.DefaultValue[Param.LayoutSelected] : string.Empty; ;
            if (defaultSheet.Length == 0) return;
            var featureSheet = new FeatureSheet(Param.StylePath(defaultSheet));
            featureSheet.ReadToEnd();
            Param.LoadFeatures("features/feature", TvFeatures, TvFeatures.Enabled ? featureSheet.Features : null);
        }

        public void WriteCssClass(StreamWriter writeCss, string className, Dictionary<string, string> value)
        {
            string precedeChar = className.ToLower() == "page" ? "@" : ".";
            if (value.Count > 0)
            {
                writeCss.WriteLine(precedeChar + className + "{");
                foreach (var pair in value)
                {
                    if (pair.Value.Length > 0)
                        writeCss.WriteLine(pair.Key + ":" + pair.Value + ";");
                }
                writeCss.WriteLine("}");
            }
        }

        public string CreateCssFile(string fileName)
        {
            string success = string.Empty;
            try
            {
                string path = Param.Value["UserSheetPath"]; // all user path
                string file = Common.PathCombine(path, fileName);
                string importStatement = "@import \"Default.css\";";
                StreamWriter writeCss = new StreamWriter(file);
                writeCss.WriteLine(importStatement);
                writeCss.Flush();
                writeCss.Close();
            }
            catch (Exception ex)
            {
                success = ex.Message;
            }
            return success;
        }

        public void WriteMedia()
        {
            XmlNode baseNode = Param.GetItem("//categories/category[@name = \"Media\"]");
            Param.SetAttrValue(baseNode, "select", MediaType);
            Param.Write();
        }

        public void AddNew(string styleName)
        {
            XmlNode baseNode = Param.GetItem("//styles/" + MediaType + "/style");
            if (baseNode == null) return;

            XmlNode copyNode = baseNode.Clone();

            Param.SetAttrValue(copyNode, AttribName, styleName); // style Name
            Param.SetAttrValue(copyNode, AttribFile, FileName); // css file name

            Param.SetAttrValue(copyNode, AttribType, TypeCustom); // custom
            Param.SetNodeText(copyNode, ElementDesc, ""); // description
            Param.SetNodeText(copyNode, ElementComment, ""); // comment
            baseNode.ParentNode.AppendChild(copyNode);

            Param.Write();
            //AddMode = false;
        }

        public bool SelectRow(DataGridView grid, string sheet)
        {
            bool result = false;
            for (int i = 0; i < grid.Rows.Count; i++)
            {
                if (grid.Rows[i].Cells[ColumnName].Value.ToString() == sheet)
                {
                    grid.Rows[i].Selected = true;
                    SelectedRowIndex = i;
                    result = true;
                    break;
                }
            }
            return result;
        }

        public string SetPreviousLayoutSelect(DataGridView grid)
        {
            Trace.WriteLineIf(_traceOnBL.Level == TraceLevel.Verbose, "ConfigurationToolBL: SetPreviousLayoutSelect");
            string lastLayout = string.Empty;
            bool selectedNotExist = true;

            if (Param.Value.ContainsKey(Param.LayoutSelected))
            {
                lastLayout = Param.Value[Param.LayoutSelected];
                if (SelectRow(grid, lastLayout))
                {
                    selectedNotExist = false;
                }
            }
            if (selectedNotExist && Param.DefaultValue.ContainsKey(Param.LayoutSelected))
            {
                lastLayout = Param.DefaultValue[Param.LayoutSelected];
                SelectRow(grid, lastLayout);
            }
            return lastLayout;
        }

        public void GridColumnWidth(DataGridView grid)
        {
            XmlNodeList columns = Param.GetItems("//column-width/column");
            int i = 0;
            foreach (XmlNode xmlNode in columns)
            {
                string value = xmlNode.Attributes["width"].Value;
                int width = int.Parse(value);
                grid.Columns[i].Width = width;
                i++;
            }
        }

        public bool WriteAttrib(string key, object sender)
        {
            bool result = false;
            string file;
            string attribValue = Common.GetTextValue(sender, out file);
            if (PreviousValue == attribValue)
            {
                return result;
            }

            string searchStyleName = StyleName;
            XmlNode baseNode = Param.GetItem("//styles/" + MediaType + "/style[@name='" + searchStyleName + "']");

            if (baseNode == null) return result;

            if (key == AttribName)
            {
                Param.SetAttrValue(baseNode, key, attribValue);
                Param.SetAttrValue(baseNode, AttribFile, FileName);
            }
            else if (key == ElementDesc || key == ElementComment)
            {
                Param.SetNodeText(baseNode, key, attribValue);
            }
            else
            {
                Param.SetAttrValue(baseNode, key, attribValue);
            }
            Param.Write();
            return true;
        }

        /// <summary>
        /// checks whether stylesheet name already exists
        /// </summary>
        /// <returns>return true or false</returns>
        public bool IsNameExists(DataGridView grid, string styleName)
        {
            bool result = false;
            if (PreviousValue.ToLower() == styleName.ToLower()) return result;
            string selectedGridName = string.Empty;  
            styleName = styleName.Trim().ToLower();
            for (int row = 0; row < grid.Rows.Count - 1; row++)
            {
                if (grid.Rows[row].Selected)
                {
                    continue; // do not compare with current selection.
                }

                if (grid[ColumnName, row].Value.ToString().ToLower() == styleName)
                {
                    result = true;
                }
            }

            XmlNodeList xmlNodeList = Param.GetItems("//styles//style");
            if (xmlNodeList != null)
            {
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    XmlNode xn = xmlNode.Attributes.GetNamedItem("name");

                    if (xn != null && xn.Value.ToLower() == styleName.ToLower())
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        public void SaveColumnWidth(string columnIndex, string columnWidth)
        {
            XmlNode baseNode = Param.GetItem("//column-width/column[@name = \"" + columnIndex + "\"]");
            Param.SetAttrValue(baseNode, "width", columnWidth);
            Param.Write();
        }

        //protected void ClearTab()
        //{
        //    ClearInfoTab(null);
        //    ClearPropertyTab(null);
        //}

        /// <summary>
        /// ClearTab Info Tab controls
        /// </summary>
        //protected void ClearInfoTab(TabPage tabPage)
        //{
        //    foreach (Control ctl in tabPage.Controls)
        //    {
        //        if (ctl is TextBox)
        //        {
        //            var textBox = (TextBox)ctl;
        //            textBox.Text = "";
        //        }
        //        else if (ctl is ComboBox)
        //        {
        //            var comboBox = (ComboBox)ctl;
        //            comboBox.SelectedIndex = -1;
        //        }
        //        else if (ctl is CheckBox)
        //        {
        //            var checkBox = (CheckBox)ctl;
        //            checkBox.Checked = false;
        //        }
        //    }
        //}

        /// <summary>
        /// ClearTab Property Tab controls
        /// </summary>
        public void ClearPropertyTab(TabPage tabPage)
        {
            foreach (Control ctl in tabPage.Controls)
            {
                if (ctl is TextBox)
                {
                    var textBox = (TextBox)ctl;
                    textBox.Text = "";
                }
                else if (ctl is ComboBox)
                {
                    var comboBox = (ComboBox)ctl;
                    comboBox.Items.Clear();
                    comboBox.SelectedIndex = -1;
                }
            }
        }

        /// <summary>
        /// Loads styles from Settings XML files
        /// </summary>
        public void ShowStyleInGrid(DataGridView grid, ArrayList cssNames)
        {
            DataSetForGrid.Tables["Styles"].Clear();
            DataRow row;
            XmlNodeList cats = Param.GetItems("//styles/" + MediaType + "/style");
            foreach (XmlNode xml in cats)
            {
                XmlAttribute name = xml.Attributes[AttribName];
                XmlAttribute file = xml.Attributes[AttribFile];
                XmlAttribute type = xml.Attributes[AttribType];
                XmlAttribute shown = xml.Attributes[AttribShown];
                XmlAttribute approvedBy = xml.Attributes[AttribApproved];
                XmlAttribute previewFile1 = xml.Attributes[AttribPreviewFile1];
                XmlAttribute previewFile2 = xml.Attributes[AttribPreviewFile2];

                XmlNode xml1 = xml.SelectSingleNode(ElementDesc);
                string desc = string.Empty;
                if (xml1 != null)
                    desc = xml1.InnerText;

                xml1 = xml.SelectSingleNode(ElementComment);
                string comment = string.Empty;
                if (xml1 != null)
                    comment = xml1.InnerText;

                row = DataSetForGrid.Tables["Styles"].NewRow();
                row["Name"] = name != null ? name.Value : string.Empty; //name.Value;
                //if (row["Name"].ToString().IndexOf("CustomSheet") >= 0)
                //    cssNames.Add(row["Name"]);
                if (row["Name"].ToString().IndexOf("Copy") >= 0 || row["Name"].ToString().IndexOf("Custom") >= 0)
                {
                    if (!cssNames.Contains(row["Name"]))
                        cssNames.Add(row["Name"]);
                }
                row["File"] = file.Value;
                row["Description"] = desc;
                row["Comment"] = comment;
                row["Type"] = type != null ? type.Value : TypeStandard;
                row["Shown"] = shown != null ? shown.Value : "Yes"; // shown.Value;
                row["ApprovedBy"] = approvedBy != null ? approvedBy.Value : string.Empty; //approvedBy.Value;
                row["previewFile1"] = previewFile1 != null && previewFile1.Value != null ? previewFile1.Value : string.Empty;
                row["previewFile2"] = previewFile2 != null && previewFile2.Value != null ? previewFile2.Value : string.Empty;
                DataSetForGrid.Tables["Styles"].Rows.Add(row);
            }
            //DataView dataView = DataSetForGrid.Tables["Styles"].DefaultView;
            //dataView.Sort = "Type DESC, Name";
            //grid.DataSource = dataView.Table;
            grid.DataSource = DataSetForGrid.Tables["Styles"];
            grid.Refresh();

            for (int i = 0; i < grid.Columns.Count; i++)
            {
                grid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            if (grid.Columns.Count > 0)
            {
                grid.Columns[5].Visible = false; // Hiding the ApprovedBy column
                grid.Columns[6].Visible = false; // Hiding the File Name       
                grid.Columns[7].Visible = false; // Preview File 1
                grid.Columns[8].Visible = false; // Preview File 2      

            }

        }

        public bool CopyCustomStyleToSend(string folderPath)
        {
            bool directoryCreated = false;
            directoryCreated = BackUpCSSFile(directoryCreated, folderPath);
            BackUpUserSettingFiles(folderPath);
            return directoryCreated;
        }

        protected bool BackUpCSSFile(bool directoryCreated, string folderPath)
        {
            XmlNodeList cats = Param.GetItems("//styles/" + MediaType + "/style");
            foreach (XmlNode xml in cats)
            {
                XmlAttribute type = xml.Attributes[AttribType];
                XmlAttribute file = xml.Attributes[AttribFile];
                if (file != null)
                {
                    string path = Path.Combine(Path.GetDirectoryName(Param.SettingPath), Path.Combine("styles", Param.Value["InputType"]));
                    if (type != null && type.Value == TypeCustom)
                    {
                        string OutputPath = Path.GetDirectoryName(Path.GetDirectoryName(Param.SettingOutputPath));
                        path = Path.Combine(OutputPath, Param.Value["InputType"]);
                    }

                    string fromFile = Path.Combine(path, file.Value);
                    if (File.Exists(fromFile))
                    {
                        if (!directoryCreated)
                        {
                            if (Directory.Exists(folderPath))
                            {
                                Directory.Delete(folderPath);
                            }
                            Directory.CreateDirectory(folderPath);
                            directoryCreated = true;
                        }

                        string toFile = Path.Combine(folderPath, file.Value);
                        File.Copy(fromFile, toFile, true);
                    }
                }
            }
            return directoryCreated;
        }

        //<summary>
        //Method to copy the settings file(DictionaryStyleSettings.xml/ScriptureSettings.xml) 
        //from the Alluser path.
        //</summary>
        protected static void BackUpUserSettingFiles(string toPath)
        {
            string projType = Param.Value["InputType"];
            string sourcePath = Path.Combine(Common.GetAllUserPath(), projType);
            if (!Directory.Exists(sourcePath)) return;
            string[] filePaths = Directory.GetFiles(sourcePath);
            foreach (string filePath in filePaths)
            {
                string fileName = Path.GetFileName(filePath);
                if (fileName.IndexOf(".xml") > 0 || fileName.IndexOf(".xsd") > 0)
                {
                    File.Copy(filePath, Path.Combine(toPath, fileName));
                }
            }
        }

        /// <summary>
        /// creating datasource for grid
        /// </summary>
        /// <returns>returns DataSet</returns>
        public void CreateGridColumn()
        {
            Trace.WriteLineIf(_traceOnBL.Level == TraceLevel.Verbose, "ConfigurationToolBL: CreateGridColumn");
            string tableName = "Styles";
            DataTable table = new DataTable(tableName);
            DataColumn column = new DataColumn
                                    {
                                        DataType = Type.GetType("System.String"),
                                        ColumnName = "Name",
                                        Caption = "Name",
                                        ReadOnly = false,
                                        Unique = false,
                                        MaxLength = 50,
                                    };
            table.Columns.Add(column);
            // Create Description column.
            column = new DataColumn
                         {
                             DataType = Type.GetType("System.String"),
                             ColumnName = "Description",
                             Caption = "Description",
                             ReadOnly = false,
                             Unique = false,
                             MaxLength = 250
                         };
            table.Columns.Add(column);

            // Create Comment column.
            column = new DataColumn
                         {
                             DataType = Type.GetType("System.String"),
                             ColumnName = "Comment",
                             Caption = "Comment",
                             ReadOnly = false,
                             Unique = false,
                             MaxLength = 250
                         };
            table.Columns.Add(column);

            // Create Type column.
            column = new DataColumn
                         {
                             DataType = Type.GetType("System.String"),
                             ColumnName = "Type",
                             Caption = "Type",
                             ReadOnly = false,
                             Unique = false,
                             MaxLength = 10
                         };
            table.Columns.Add(column);

            // Create Shown column.
            column = new DataColumn
                         {
                             DataType = Type.GetType("System.String"),
                             ColumnName = "Shown",
                             Caption = "Shown",
                             MaxLength = 10,
                             ReadOnly = false,
                             Unique = false
                         };
            table.Columns.Add(column);

            // Create Approvedby column.
            column = new DataColumn
                         {
                             DataType = Type.GetType("System.String"),
                             ColumnName = "Approvedby",
                             Caption = "Approvedby",
                             ReadOnly = false,
                             Unique = false,
                             MaxLength = 10
                         };
            table.Columns.Add(column);

            // Create File column.
            column = new DataColumn
                         {
                             DataType = Type.GetType("System.String"),
                             ColumnName = "File",
                             Caption = "File",
                             ReadOnly = false,
                             Unique = false,
                             MaxLength = 100
                         };
            table.Columns.Add(column);

            // Create PreviewFile1 column.
            column = new DataColumn
                         {
                             DataType = Type.GetType("System.String"),
                             ColumnName = "PreviewFile1",
                             Caption = "PreviewFile1",
                             ReadOnly = false,
                             Unique = false,
                             MaxLength = 150
                         };
            table.Columns.Add(column);

            // Create PreviewFile2 column.
            column = new DataColumn
                         {
                             DataType = Type.GetType("System.String"),
                             ColumnName = "PreviewFile2",
                             Caption = "PreviewFile2",
                             ReadOnly = false,
                             Unique = false,
                             MaxLength = 150
                         };
            table.Columns.Add(column);

            // Instantiate the DataSet variable.
            //_dataSet = new DataSet();
            // Add the new DataTable to the DataSet.
            DataSetForGrid.Clear();
            DataSetForGrid.Tables.Clear();
            DataSetForGrid.Tables.Add(table);
        }

        /// <summary>
        /// This for Nunit Test - no need of 
        /// </summary>
        public void SetCssDictionartyToTest()
        {
            Dictionary<string, string> attrib = new Dictionary<string, string>();
            attrib["column-count"] = "2";
            _cssClass["letData"] = attrib;

            attrib = new Dictionary<string, string>();
            attrib["text-align"] = "justify";
            _cssClass["entry"] = attrib;

        }

        public void SetGotFocusValueBL(object sender)
        {
            try
            {
                string control;
                //if (PreviousValue != string.Empty)
                //{
                //    _redoundo.Set(Common.Action.Edit, StyleName, _currentControl, PreviousValue);
                //}
                PreviousValue = Common.GetTextValue(sender, out control);
               _redoUndoBufferValue = PreviousValue;
                _redoundo.PreviousControl = _redoundo.CurrentControl;
                _redoundo.CurrentControl = control;
                _previousStyleName = PreviousValue;

            }
            catch { }
        }

        public void ShowMobileSummaryBL()
        {
            string comma = ", ";
            string red = (cTool.DdlRedLetter.Text.Length > 0 && cTool.DdlRedLetter.Text.ToLower() == "yes") ? " Red Letter  " : "";
            if (red.Length == 0)
                comma = "";
            string files = (cTool.DdlFiles.Text.Length > 0) ? " Numbers of files produced -  " + cTool.DdlFiles.Text + comma : " ";

            string combined =
                files + " " +
                red;

            cTool.TxtCss.Text = combined;
        }

        public void ValidatePageHeightMarginsBL(object sender)
        {
            int marginTop = GetDefaultValue(cTool.TxtPageTop.Text);
            int marginBottom = GetDefaultValue(cTool.TxtPageBottom.Text);
            int expectedHeight = GetPageSize(cTool.DdlPagePageSize.Text, "Height") / 4;
            int outputHeight = marginTop + marginBottom;

            Control ctrl = ((Control)sender);
            string errMessage = outputHeight > expectedHeight ? "The combination of the margin-Top and margin-bottom should not exceed a quarter of the page height." : "";

            _errProvider.SetError(ctrl, errMessage);
            if (errMessage.Length == 0)
            {
                _errProvider.SetError(cTool.TxtPageTop, errMessage);
                _errProvider.SetError(cTool.TxtPageBottom, errMessage);
            }
            //FillCssValues();
            ShowCssSummary();
        }

        public void ValidatePageWidthMarginsBL(object sender)
        {
            int marginLeft = GetDefaultValue(cTool.TxtPageInside.Text);
            int marginRight = GetDefaultValue(cTool.TxtPageOutside.Text);
            int columnGap = GetDefaultValue(cTool.TxtPageGutterWidth.Text);
            int expectedWidth = GetPageSize(cTool.DdlPagePageSize.Text, "Width") / 2;
            int outputWidth = marginLeft + columnGap + marginRight;

            Control ctrl = ((Control)sender);
            string errMessage = outputWidth > expectedWidth ? "The combination of the column gap, left and right margins should not exceed half of the page width." : "";
            _errProvider.SetError(ctrl, errMessage);
            _errProvider.SetError(cTool.TxtPageInside, errMessage);
            _errProvider.SetError(cTool.TxtPageOutside, errMessage);
            _errProvider.SetError(cTool.TxtPageGutterWidth, errMessage);
            ShowCssSummary();
        }

        public void ValidateLineHeightBL()
        {
            if (cTool.DdlFontSize.Text.Length > 0 && cTool.DdlLeading.Text != "No Change")
            {
                int selectedfontSize = int.Parse(cTool.DdlFontSize.Text);
                int selectedLineHeight = int.Parse(cTool.DdlLeading.Text);
                int expectedLineHeight = selectedfontSize * 120 / 100;
                string errMessage = selectedLineHeight < expectedLineHeight ? "Line height should be 120% of font size" : "";
                _errProvider.SetError(cTool.TxtPageInside, errMessage);
            }
        }
        #endregion

        #region Event Method
        public void btnBrowse_ClickBL()
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
                    Param.UpdateMobileAtrrib("Icon", toPath, StyleName);
                    cTool.MobileIcon.Image = iconImage;
                }
                catch { }
            }
        }

        public void ddlRedLetter_SelectedIndexChangedBL(object sender, EventArgs e)
        {
            try
            {
                Param.UpdateMobileAtrrib("RedLetter", cTool.DdlRedLetter.Text, StyleName);
                SetMobileSummary(sender, e);
            }
            catch { }
        }

        public void ddlFiles_SelectedIndexChangedBL(object sender, EventArgs e)
        {
            try
            {
                _fileProduce = cTool.DdlFiles.Text;
                Param.UpdateMobileAtrrib("FileProduced", cTool.DdlFiles.Text, StyleName);
                SetMobileSummary(sender, e);
            }
            catch { }
        }

        public void txtCopyright_ValidatedBL()
        {
            try
            {
                Param.UpdateMobileAtrrib("Copyright", cTool.TxtCopyright.Text, StyleName);
            }
            catch
            {
            }
        }

        public void txtInformation_ValidatedBL()
        {
            try
            {
                Param.UpdateMobileAtrrib("Information", cTool.TxtInformation.Text, StyleName);
            }
            catch
            {
            }
        }

        public void tsPreview_ClickBL()
        {
            try
            {
                string settingPath = Path.GetDirectoryName(Param.SettingPath);
                string inputPath = Common.PathCombine(settingPath, "Styles");
                inputPath = Common.PathCombine(inputPath, Param.Value["InputType"]);
                string stylenamePath = Common.PathCombine(inputPath, "Preview");

                String imageFile = Common.PathCombine(stylenamePath, PreviewFileName1);
                String imageFile1 = Common.PathCombine(stylenamePath, PreviewFileName2);

                string selectedTypeValue = cTool.StylesGrid[ColumnType, SelectedRowIndex].Value.ToString();
                if (selectedTypeValue != TypeStandard)
                {
                    bool isPreviewFileExist = false;
                    if (File.Exists(PreviewFileName1) && File.Exists(PreviewFileName2))
                    {
                        isPreviewFileExist = true;
                    }
                    string fileName = string.Empty;
                    //if (_isCreatePreview)
                    if (_screenMode == ScreenMode.Modify || _screenMode == ScreenMode.SaveAs || _screenMode == ScreenMode.New || !isPreviewFileExist)
                    {
                        WriteCss();
                        ShowCSSValue();
                        _screenMode = ScreenMode.Edit;
                        PleaseWait st = new PleaseWait();
                        st.ShowDialog();
                        string cssFile = Param.StylePath(FileName);
                        PdftoJpg pd = new PdftoJpg();
                        fileName = pd.ConvertPdftoJpg(cssFile, true);
                        //_isCreatePreview = false;
                        

                        if (!Directory.Exists(stylenamePath)) return;

                        if (!(File.Exists(imageFile) && File.Exists(imageFile1)))
                        {
                            imageFile = Common.PathCombine(Common.GetAllUserPath(),
                                                           Path.GetFileNameWithoutExtension(fileName) + ".pdf1.jpg");
                            PreviewFileName1 = imageFile;
                            imageFile1 = Common.PathCombine(Common.GetAllUserPath(),
                                                            Path.GetFileNameWithoutExtension(fileName) + ".pdf2.jpg");
                            PreviewFileName2 = imageFile1;
                        }

                        string xPath = "//styles/" + MediaType + "/style[@name='" + StyleName + "']";
                        XmlNode baseNode = Param.GetItem(xPath);
                        if (baseNode != null)
                        {
                            Param.SetAttrValue(baseNode, "previewfile1", imageFile);
                            Param.SetAttrValue(baseNode, "previewfile2", imageFile1);
                            Param.Write();
                        }
                    }
                    else
                    {
                        imageFile = PreviewFileName1;
                        imageFile1 = PreviewFileName2;
                    }

                }

                if (File.Exists(imageFile) || File.Exists(imageFile1))
                {
                    PreviewConfig preview = new PreviewConfig(imageFile,
                                                              imageFile1)
                                                {
                                                    Text = ("Preview - " + StyleName)
                                                };
                    preview.Icon = cTool.Icon;
                    preview.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Preview is not available", "Pathway");
                }
            }
            catch { }
        }

        public void ddlPageColumn_SelectedIndexChangedBL(object sender, EventArgs e)
        {
            try
            {
                if (cTool.TxtPageGutterWidth.Text.Length == 0)
                    cTool.TxtPageGutterWidth.Text = "18pt";
                cTool.TxtPageGutterWidth.Enabled = true;
            }
            catch { }

            //txtPageGutterWidth_Validated(sender, e);
        }

        public void txtName_KeyUpBL()
        {
            try
            {
                UpdateGrid(cTool.TxtName, cTool.StylesGrid);
                _redoundo.Set(Common.Action.Edit, StyleName, "txtName", _redoUndoBufferValue, cTool.TxtName.Text);
                _redoUndoBufferValue = cTool.TxtName.Text;
            }
            catch { }
        }

        public void txtComment_KeyUpBL()
        {
            try
            {
                UpdateGrid(cTool.TxtComment, cTool.StylesGrid);
                _redoundo.Set(Common.Action.Edit, StyleName, "txtComment", _redoUndoBufferValue, cTool.TxtComment.Text);
                _redoUndoBufferValue = cTool.TxtComment.Text;
            }
            catch { }
        }

        public void txtDesc_KeyUpBL()
        {
            try
            {
                UpdateGrid(cTool.TxtDesc, cTool.StylesGrid);
                _redoundo.Set(Common.Action.Edit, StyleName, "txtDesc", _redoUndoBufferValue, cTool.TxtDesc.Text);
                _redoUndoBufferValue = cTool.TxtDesc.Text;

            }
            catch { }
        }

        public void stylesGrid_ColumnWidthChangedBL(DataGridViewColumnEventArgs e)
        {
            try
            {
                string columnIndex = e.Column.Index.ToString();
                string columnWidth = e.Column.Width.ToString();
                SaveColumnWidth(columnIndex, columnWidth);
            }
            catch { }
        }


        public void stylesGrid_RowEnterBL(DataGridViewCellEventArgs e)
        {
            try
            {
                if (_screenMode == ScreenMode.Modify) // Add
                {
                    WriteCss();
                }
                _screenMode = ScreenMode.View;
                SelectedRowIndex = e.RowIndex;
                ShowInfoValue();
                //_screenMode = ScreenMode.Edit;
                //if (_screenMode == ScreenMode.Modify || _screenMode == ScreenMode.Edit) // Add
                //{
                    
                //    ShowInfoValue();
                //    //WriteCss();
                //    _screenMode = ScreenMode.Edit;
                //}
                //else
                //{
                //    ShowInfoValue();
                //}

                //if (!AddMode)
                //{
                //    SelectedRowIndex = e.RowIndex;
                //    ShowInfoValue();
                //}
                //else
                //{
                //    AddMode = false;
                //}
                //_isCreatePreview = false;
            }
            catch { }
        }

        public void txtApproved_ValidatedBL(object sender)
        {
            try
            {
                WriteAttrib(AttribApproved, sender);
                EnableToolStripButtons(true);

            }
            catch { }
        }

        public void tsRedo_ClickBL(object sender, EventArgs e)
        {
            try
            {
                ModifyData control = _redoundo.Redo();
                if (control.Action != Common.Action.Edit) // Add or Delete
                {
                    LoadParam();
                    ClearPropertyTab(cTool.TabDisplay);
                    PopulateFeatureSheet();
                    SetPreviousLayoutSelect(cTool.StylesGrid);
                    ShowDataInGrid();
                    SelectRow(cTool.StylesGrid, control.EditStyleName);
                }
                else // Edit
                {
                    bool success = SetUI(control);
                    if (!success)
                    {
                        // Ignore Recent modification. Ex: 123 - ignores 3 and gives 12 
                        tsRedo_ClickBL(sender, e);
                    }
                }

            }
            catch { }
        }

        public void tsUndo_ClickBL(object sender, EventArgs e)
        {
            try
            {
                ModifyData control = _redoundo.Undo(Common.Action.Edit, _styleName, _redoundo.CurrentControl, PreviousValue);
                PreviousValue = string.Empty;
                if (control.Action != Common.Action.Edit) // Add or Delete
                {
                    LoadParam();
                    ClearPropertyTab(cTool.TabDisplay);
                    PopulateFeatureSheet();
                    SetPreviousLayoutSelect(cTool.StylesGrid);
                    ShowDataInGrid();
                    SelectRow(cTool.StylesGrid, control.EditStyleName);
                }
                else // Edit
                {
                    bool success = SetUI(control);
                    if (!success)
                    {
                        // Ignore Recent modification. Ex: 123 - ignores 3 and gives 12 
                        tsUndo_ClickBL(sender, e);
                    }
                }
            }
            catch { }
        }

        public void chkAvailable_CheckedChangedBL()
        {
            try
            {
                UpdateGrid(cTool.ChkAvailable, cTool.StylesGrid);

            }
            catch { }
            // NOTE - Pending
            //_redoundo.Set(Common.Action.Edit, sender); 
        }

        public void txtComment_ValidatedBL(object sender)
        {
            try
            {
                WriteAttrib(ElementComment, sender);
                EnableToolStripButtons(true);

            }
            catch { }
        }

        public void chkAvailable_ValidatedBL(object sender)
        {
            try
            {
                WriteAttrib(AttribShown, sender);
                EnableToolStripButtons(true);

            }
            catch { }
            // _redoundo.Set(Common.Action.Edit, StyleName, "chkAvailable", PreviousValue, cTool.cTool.ChkAvailable.Checked.ToString());
            //PreviousValue = cTool.ChkAvailable.Checked.ToString();
        }

        public void txtDesc_ValidatedBL(object sender)
        {
            try
            {
                WriteAttrib(ElementDesc, sender);
                EnableToolStripButtons(true);

            }
            catch { }
        }

        public void txtName_ValidatingBL(object sender)
        {
            if (_screenMode != ScreenMode.Modify) return;
            try
            {
                cTool.TxtName.Text = cTool.TxtName.Text.Trim();
                if (cTool._previousTxtName == cTool.TxtName.Text) return;

                bool isNoDuplicateStyleName = NoDuplicateStyleName();
                bool isValidateStyleName = ValidateStyleName(cTool.TxtName.Text);

                if (!isNoDuplicateStyleName || !isValidateStyleName)
                {
                    cTool.TxtName.Text = _previousStyleName;
                    cTool.StylesGrid.Rows[SelectedRowIndex].Cells[0].Value = _previousStyleName;
                    cTool.TxtName.Focus();
                    return;
                }
                string styleName = cTool.TxtName.Text;
                FileName = cTool.TxtName.Text + ".css";
                cTool.StylesGrid[ColumnFile, SelectedRowIndex].Value = FileName;

                if (_screenMode == ScreenMode.New) // Add
                {
                    //_redoundo.Set(Common.Action.New, StyleName, null, "", string.Empty);

                    Param.StyleFile[styleName] = FileName;
                    string errMsg = CreateCssFile(FileName);
                    if (errMsg.Length > 0)
                    {
                        MessageBox.Show("Sorry, your recent changes cannot be saved because Pathway cannot find the stylesheet file '" + errMsg + "'", _caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    }
                    AddNew(cTool.TxtName.Text);
                    EnableToolStripButtons(true);
                    ShowStyleInGrid(cTool.StylesGrid, _cssNames);
                    SelectRow(cTool.StylesGrid, PreviousValue);
                    ShowInfoValue();
                }
                else if(_screenMode == ScreenMode.Modify)
                {
                    if (PreviousValue == styleName)
                    {
                        return;
                    }
                    //file -> fileNew1 -> fileNew.css
                    string path = Param.Value["UserSheetPath"];
                    string fromFile = Common.PathCombine(path, PreviousValue + ".css");
                    string toFile = Common.PathCombine(path, FileName);
                    if (File.Exists(fromFile))
                        try
                        {
                            File.Move(fromFile, toFile);
                        }
                        catch
                        {
                        }
                    Param.StyleFile[styleName] = FileName;
                    WriteAttrib(AttribName, sender);
                    _cssNames.Remove(PreviousValue);
                    EnableToolStripButtons(true);
                    IsLayoutSelectedStyle();
                }
                StyleName = cTool.TxtName.Text;
                cTool.LblInfoCaption.Text = cTool.TxtName.Text;

            }
            catch { }
        }

        public void btnScripture_ClickBL()
        {
            try
            {
                WriteCss();
                cTool.BtnMobile.Enabled = true;
                cTool.BtnWeb.Enabled = false;
                cTool.BtnOthers.Enabled = false;
                setLastSelectedLayout();
                WriteMedia();
                inputTypeBL = "Scripture";
                SetInputTypeButton();
                LoadParam();
                ClearPropertyTab(cTool.TabDisplay);
                ClearPropertyTab(tabmob);
                PopulateFeatureSheet(); //For TD-1194 // Load Default Values
                SetPreviousLayoutSelect(cTool.StylesGrid);
                SetSideBar();
                ShowDataInGrid();
                SetMobilePropertyTab();
                _redoundo.Reset();
            }
            catch
            {
            }
        }

        public void btnDictionary_ClickBL()
        {
            try
            {
                WriteCss(); 
                cTool.BtnMobile.Enabled = false;
                cTool.BtnWeb.Enabled = false;
                cTool.BtnOthers.Enabled = false;
                setLastSelectedLayout();
                WriteMedia();
                inputTypeBL = "Dictionary";
                SetInputTypeButton();
                LoadParam();
                ClearPropertyTab(cTool.TabDisplay);
                PopulateFeatureSheet(); //For TD-1194 // Load Default Values
                SetPreviousLayoutSelect(cTool.StylesGrid);
                SetSideBar();
                ShowDataInGrid();
                SetMobilePropertyTab();
                _redoundo.Reset();
            }
            catch
            {
            }
        }

        public void ConfigurationTool_FormClosingBL()
        {
            try
            {
                setLastSelectedLayout();
                setDefaultInputType();
                WriteCss();
            }
            catch { }
        }

        public void txtPageTop_ValidatedBL(object sender, EventArgs e)
        {
            try
            {
                bool result = Common.AssignValuePageUnit(cTool.TxtPageTop, null);
                _errProvider = Common._errProvider;
                if (_errProvider.GetError(cTool.TxtPageTop) != "")
                {
                    _errProvider.SetError(cTool.TxtPageTop, _errProvider.GetError(cTool.TxtPageTop));
                }
                else
                {
                    ValidatePageHeightMargins(sender, e);
                }
            }
            catch { }
        }

        public void txtPageOutside_ValidatedBL(object sender, EventArgs e)
        {
            try
            {
                bool result = Common.AssignValuePageUnit(cTool.TxtPageOutside, null);
                _errProvider = Common._errProvider;
                if (_errProvider.GetError(cTool.TxtPageOutside) != "")
                {
                    _errProvider.SetError(cTool.TxtPageOutside, _errProvider.GetError(cTool.TxtPageOutside));
                }
                else
                {
                    ValidatePageWidthMargins(sender, e);
                }
            }
            catch { }
        }

        private void ValidateLineHeight(object sender, EventArgs e)
        {
            ValidateLineHeightBL();
        }

        private void ValidatePageWidthMargins(object sender, EventArgs e)
        {
            ValidatePageWidthMarginsBL(sender);
        }

        private void ValidatePageHeightMargins(object sender, EventArgs e)
        {
            ValidatePageHeightMarginsBL(sender);
        }

        private void SetMobileSummary(object sender, EventArgs e)
        {
            ShowMobileSummaryBL();
        }

        public void txtPageInside_ValidatedBL(object sender, EventArgs e)
        {
            try
            {
                bool result = Common.AssignValuePageUnit(cTool.TxtPageInside, null);
                _errProvider = Common._errProvider;
                if (_errProvider.GetError(cTool.TxtPageInside) != "")
                {
                    _errProvider.SetError(cTool.TxtPageInside, _errProvider.GetError(cTool.TxtPageInside));
                }
                else
                {
                    ValidatePageWidthMargins(sender, e);
                }
            }
            catch { }
        }

        public void txtPageGutterWidth_ValidatedBL(object sender, EventArgs e)
        {
            try
            {
                bool result = Common.AssignValuePageUnit(cTool.TxtPageGutterWidth, null);
                _errProvider = Common._errProvider;
                if (_errProvider.GetError(cTool.TxtPageGutterWidth) != "")
                {
                    _errProvider.SetError(cTool.TxtPageGutterWidth, _errProvider.GetError(cTool.TxtPageGutterWidth));
                }
                else
                {
                    ValidatePageWidthMargins(sender, e);
                }
            }
            catch { }
        }

        public void tsDefault_ClickBL()
        {
            try
            {
                var dlg = new PrintVia("Set Defaults");
                dlg.InputType = inputTypeBL;
                dlg.DatabaseName = "{Project_Name}";
                dlg.Media = MediaType;
                dlg.ShowDialog();
            }
            catch { }
        }

        public void tsNew_ClickBL()
        {
            try
            {
                _screenMode = ScreenMode.New;
                AddStyleInXML(cTool.StylesGrid, _cssNames);
                ShowStyleInGrid(cTool.StylesGrid, _cssNames);
                SelectRow(cTool.StylesGrid, NewStyleName);
                WriteCss();
                ShowInfoValue();
                cTool.TxtName.Select();
                //EnableToolStripButtons(true);
                ////AddNewRow();
                ////SetFocusToName();
                ////EnableToolStripButtons(false);
            }
            catch { }
        }

        public void tsSend_ClickBL()
        {
            WriteCss();
            string tempfolder = Path.GetTempPath();
            string folderName = Path.GetFileNameWithoutExtension(Path.GetTempFileName());
            string folderPath = Path.Combine(tempfolder, folderName);
            bool directoryCreated = CopyCustomStyleToSend(folderPath);
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

        public void txtPageBottom_ValidatedBL(object sender, EventArgs e)
        {
            try
            {
                bool result = Common.AssignValuePageUnit(cTool.TxtPageBottom, null);
                _errProvider = Common._errProvider;
                if (_errProvider.GetError(cTool.TxtPageBottom) != "")
                {
                    _errProvider.SetError(cTool.TxtPageBottom, _errProvider.GetError(cTool.TxtPageBottom));
                }
                else
                {
                    ValidatePageHeightMargins(sender, e);
                }
            }
            catch { }
        }

        public void ConfigurationTool_LoadBL()
        {
            _screenMode = ScreenMode.Load;

            Trace.WriteLineIf(_traceOn.Level == TraceLevel.Verbose, "ConfigurationTool_Load");
            tabdic = cTool.TabControl1.TabPages[1];
            if (cTool.TabControl1.TabPages.Count > 2)
            {
                tabmob = cTool.TabControl1.TabPages[2];
                cTool.TabControl1.TabPages.Remove(cTool.TabControl1.TabPages[2]);
            }
            cTool.BtnMobile.Enabled = false;
            _redoundo = new UndoRedo(cTool.TsUndo, cTool.TsRedo);
            cTool.MinimumSize = new Size(497, 183);
            cTool.LoadSettings();
            SetInputTypeButton();
            CreateGridColumn();
            LoadParam(); // Load DictionaryStyleSettings / ScriptureStyleSettings
            ShowDataInGrid();
            _redoundo.Reset();

            SetPreviousLayoutSelect(cTool.StylesGrid);
            PopulateFeatureSheet(); //For TD-1194 // Load Default Values
            //ShowInfoValue();
            SetMediaType();
            if (!File.Exists(Common.FromRegistry("ScriptureStyleSettings.xml")))
            {
                cTool.BtnScripture.Enabled = false;
                cTool.BtnScripture.Visible = false;
                cTool.BtnDictionary.Visible = false;
            }
            SetFocusToName();

            //For the task TD-1481
            cTool.BtnWeb.Enabled = false;
            cTool.BtnOthers.Enabled = false;

            _screenMode = ScreenMode.View;
            ShowInfoValue();
            _screenMode = ScreenMode.Edit;
        }

        public void SetModifyMode(bool setEdited)
        {
            if (_screenMode == ScreenMode.Edit || setEdited)
            {
                _screenMode = ScreenMode.Modify;
                setEdited = false;
            }
        }

        public void ConfigurationTool_KeyUpBL(object sender, KeyEventArgs e)
        {
            try
            {
                //if (e.Control && e.KeyCode == Keys.Delete)
                if (cTool.StylesGrid.Focused && e.KeyCode == Keys.Delete)
                {
                    if (cTool.TsDelete.Enabled)
                    {
                        cTool.tsDelete_Click(sender, null);
                    }
                }
                else if (e.KeyCode == Keys.F1)
                {
                    Common.PathwayHelpSetup(cTool.BtnScripture.Enabled, Common.FromRegistry("Help"));
                    Common.HelpProv.SetHelpNavigator(cTool, HelpNavigator.Topic);
                    Common.HelpProv.SetHelpKeyword(cTool, "Overview.htm");
                    SendKeys.Send("{F1}");
                }

                //Show Version when Ctrl+F12
                if (e.Control && e.KeyCode == Keys.F12)
                {
                    cTool.Text = "Pathway Configuration Tool - " + AssemblyFileVersion;
                }
            }
            catch { }
        }

        public void tsDelete_ClickBL()
        {
            _screenMode = ScreenMode.Delete;
            _redoundo.Reset();
            //StyleName = cTool.StylesGrid[ColumnName, SelectedRowIndex].Value.ToString();
            //cTool.LblInfoCaption.Text = StyleName;
            string name = cTool.LblInfoCaption.Text;
            string msg = "Are you sure you want to delete the " + name + " stylesheet?";
            //string msg = "Are you sure you want to delete the " + StyleName + " stylesheet?";
            string caption = "Delete Stylesheet";
            if (!cTool._fromNunit)
            {
                DialogResult result = MessageBox.Show(msg, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning,
                                                      MessageBoxDefaultButton.Button2);
                if (result != DialogResult.OK) return;
            }
            try
            {
                //_redoundo.Set(Common.Action.Delete, StyleName, null, "", string.Empty);
                if (SelectedRowIndex >= 0)
                {
                    string selectedTypeValue = cTool.StylesGrid[ColumnType, SelectedRowIndex].Value.ToString();
                    
                    if (selectedTypeValue != TypeStandard)
                    {
                        _cssNames.Remove(StyleName);
                        RemoveXMLNode(StyleName);
                        cTool.StylesGrid.Rows.RemoveAt(cTool.StylesGrid.Rows.GetFirstRow(DataGridViewElementStates.Selected));
                        if (SelectedRowIndex == cTool.StylesGrid.Rows.Count) // Is last row?
                            SelectedRowIndex = SelectedRowIndex - 1;
                        cTool.StylesGrid.Rows[SelectedRowIndex].Selected = true;
                        //SelectedRowIndex--;
                        //WriteCss();
                        _screenMode = ScreenMode.Edit;
                        ShowInfoValue();
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
            PreviousStyleName = cTool.StylesGrid.Rows[SelectedRowIndex].Cells[0].Value.ToString();
            //cTool.LblInfoCaption.Text = PreviousStyleName;
            WriteCss();
            
        }

        public void tabControl1_SelectedIndexChangedBL()
        {
            if (cTool.TabControl1.SelectedTab.Text == "Display Properties")
            {
                txtPageInside_ValidatedBL(cTool.TxtPageInside, null);
                txtPageOutside_ValidatedBL(cTool.TxtPageOutside, null);
                txtPageTop_ValidatedBL(cTool.TxtPageTop, null);
                txtPageBottom_ValidatedBL(cTool.TxtPageBottom, null);
                txtPageGutterWidth_ValidatedBL(cTool.TxtPageGutterWidth, null);
            }
            else if (cTool.TabControl1.SelectedIndex ==1)
            {
                ShowCSSValue();
            }
        }

        public void ddlFileProduceDict_ValidatedBL(object sender)
        {
            try
            {
                _fileProduce = cTool.DdlFileProduceDict.Text;
                //WriteCss(sender);
            }
            catch { }
        }

        public void tsSaveAs_ClickBL()
        {
            try
            {
                _screenMode = ScreenMode.SaveAs;
                //_redoundo.Set(Common.Action.Copy, StyleName, null, "", string.Empty);
                if (CopyStyle(cTool.StylesGrid, _cssNames))
                {
                    ShowStyleInGrid(cTool.StylesGrid, _cssNames);
                    SelectRow(cTool.StylesGrid, PreviousStyleName);
                    WriteCss();
                    ShowInfoValue();
                    cTool.TxtName.Select();
                }
                
                
                //_screenMode = ScreenMode.Add;
                //AddStyleInXML(cTool.StylesGrid, _cssNames);
                //ShowStyleInGrid(cTool.StylesGrid, _cssNames);
                //SelectRow(cTool.StylesGrid, NewStyleName);

                //ShowInfoValue();
                //cTool.TxtName.Select();
                //EnableToolStripButtons(true);

            }
            catch { }
        }


        #endregion
    }
}
