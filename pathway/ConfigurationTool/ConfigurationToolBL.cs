using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
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

        public string InputType = string.Empty;

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
        public string PreviewFileName1 = string.Empty;
        public string PreviewFileName2 = string.Empty;

        public bool AddMode;

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
        }

        public void SetDefaultCSSValue(string cssPath, string loadType)
        {
            _cssPath = cssPath;
            _loadType = loadType;
            parseCss();
        }
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
                return GetValue(task, key, "");
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
                string result = GetValue(task, key, "");
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
                string result = GetValue(task, key, "");
                return result.Length > 0 ? result : "";
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
                if (_cssClass.ContainsKey(task) && _cssClass[task].ContainsKey("margin-left"))
                {
                    return "Bullet";
                }
                return "No change";
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

        private void parseCss()
        {
            Common.SamplePath = Param.Value["SamplePath"].Replace("Samples", "Styles");
            if (string.IsNullOrEmpty(_cssPath)) return;
            CssTree cssTree = new CssTree();
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

        private static string GetNewStyleCount(ArrayList cssNames, string preferedName)
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


        private static string GetDirCount(ArrayList cssNames, string preferedName)
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
                        if (!AddMode)
                            grid[ColumnName, SelectedRowIndex].Value = myControl.Text;
                        break;
                    default:
                        CheckBox checkBox = (CheckBox)myControl;
                        grid[ColumnShown, SelectedRowIndex].Value = checkBox.Checked ? "Yes" : "No";
                        break;
                }
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
            Param.SaveSheet(PreviousStyleName, Param.StylePath(StyleName), currentDescription, type);
            XmlNode baseNode = Param.GetItem("//styles/" + MediaType + "/style[@name='" + PreviousStyleName + "']");
            Param.SetAttrValue(baseNode, AttribType, TypeCustom);
            Param.Write();
            return true;
        }

        public void AddStyle(DataGridView grid, ArrayList cssNames)
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
        public void LoadParam()
        {
            Param.SetValue(Param.InputType, InputType); // Dictionary or Scripture
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
            styleName = styleName.Trim().ToLower();
            for (int row = 0; row < grid.Rows.Count - 1; row++)
            {
                if (grid.Rows[row].Selected)
                    continue; // do not compare with current selection.

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

        //private void ClearTab()
        //{
        //    ClearInfoTab(null);
        //    ClearPropertyTab(null);
        //}

        /// <summary>
        /// ClearTab Info Tab controls
        /// </summary>
        //public void ClearInfoTab(TabPage tabPage)
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
        public void LoadMediaStyle(DataGridView grid, ArrayList cssNames)
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
            grid.DataSource = DataSetForGrid.Tables["Styles"];
            grid.Refresh();
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

        private bool BackUpCSSFile(bool directoryCreated, string folderPath)
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
        private static void BackUpUserSettingFiles(string toPath)
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
            string tableName = "Styles";
            DataTable table = new DataTable(tableName);
            DataColumn column = new DataColumn
                                    {
                                        DataType = Type.GetType("System.String"),
                                        ColumnName = "Name",
                                        Caption = "Name",
                                        ReadOnly = false,
                                        Unique = false
                                    };
            table.Columns.Add(column);

            // Create column.
            column = new DataColumn
                         {
                             DataType = Type.GetType("System.String"),
                             ColumnName = "Description",
                             Caption = "Description",
                             ReadOnly = false,
                             Unique = false
                         };
            table.Columns.Add(column);

            // Create column.
            column = new DataColumn
                         {
                             DataType = Type.GetType("System.String"),
                             ColumnName = "Comment",
                             Caption = "Comment",
                             ReadOnly = false,
                             Unique = false
                         };
            table.Columns.Add(column);

            // Create column.
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

            // Create column.
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

            // Create second column.
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

            // Create column.
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

            // Create column.
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

            // Create column.
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
    }
}
