// --------------------------------------------------------------------------------------------
// <copyright file="SettingBl.cs" from='2009' to='2009' company='SIL International'>
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
// Common class written for CSS Setting for Dictionary and Scripture
// </remarks>
// --------------------------------------------------------------------------------------------

#region Using
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Text.RegularExpressions;
    using System.IO;
    using System.Xml;
    using System.Collections;
    using SIL.PublishingSolution;
    using SIL.Tool;
    using SIL.Tool.Localization;

#endregion

namespace SIL.PublishingSolution
{
    /// <summary>
    /// Business Layer for Dictionary and Scripture settings
    /// </summary>
    public class SettingBl
    {
        #region Private Variables

        /// <summary>
        /// Values to load in section listview of Document Tab
        /// </summary>
        // ReSharper disable RedundantExplicitArraySize
        readonly string[] _arrDictSections = new string[12] 
        // ReSharper restore RedundantExplicitArraySize
        { 
            "Cover", "Title", "Rights", "Introduction", "List", "Orthography", "Phonology", "Grammar", "Lexicon", 
            "Index", "Reversal", "Bibliography" 
        };
        public string SolutionName;

/*
        /// <summary>
        /// Values to load in steps listview of Document Tab
        /// </summary>
        string[] arrMainPrepSteps = new string[12] 
        { 
            "Filter Entries", "Filter Senses", "Filter Media", "Filter Text", "Make Minor Entries", 
            "Make Homographs", "Sort Entries", "Make Letter Headers", "Make Grammatical Categories", 
            "Make Cross References", "Order Writing Systems", "Order Fields"
        };
*/

        #endregion


        #region Public Variables
		public string _hyphenationPath = Common.FromRegistry("Hyphenation_Languages");
        public string _locUser = System.Globalization.RegionInfo.CurrentRegion.TwoLetterISORegionName;
        #endregion

        #region Public Functions

        /// <summary>
        /// To swap the ListBox Items based on pressing the Up and Down buttons.
        /// </summary>
        /// <param name="lstCtl">ListBox Control</param>
        /// <param name="dir">Up or Down</param>
        public void SwapListItem(ListBox lstCtl, string dir)
        {
            int index = lstCtl.SelectedIndex;
            object swap = lstCtl.SelectedItem;
            if (index != -1)
            {
                if (dir == "Up")
                {
                    if (lstCtl.SelectedIndex != 0)
                    {
                        lstCtl.Items.RemoveAt(index);
                        lstCtl.Items.Insert(index - 1, swap);
                    }
                }
                else if (dir == "Dn")
                {
                    if (lstCtl.Items.Count - 1 != index)
                    {
                        lstCtl.Items.RemoveAt(index);
                        lstCtl.Items.Insert(index + 1, swap);
                    }
                }
                lstCtl.SelectedItem = swap;
            }
        }

        /// <summary>
        /// To move to Tab position based on the direction
        /// </summary>
        /// <param name="tabCtl">TabControl which need action</param>
        /// <param name="dir">To specify direction P(Previous) or N(Next)</param>
        public void MoveTab(TabControl tabCtl, char dir)
        {
            int tabIndex = 0;
            if (dir == 'P')
            {
                tabIndex = tabCtl.SelectedIndex - 1;
                if (tabIndex == -1)
                {
                    tabIndex = tabCtl.TabPages.Count - 1;
                }
            }
            else if (dir == 'N')
            {
                tabIndex = tabCtl.SelectedIndex + 1;
                if (tabIndex == tabCtl.TabPages.Count)
                {
                    tabIndex = 0;
                }
            }            
            tabCtl.SelectedIndex = tabIndex;
        }

        /// <summary>
        /// To load the Kerning Values
        /// </summary>
        /// <param name="cmbKerning">ComboBox Name</param>
        public void LoadKerningValues(ComboBox cmbKerning)
        {
            cmbKerning.Items.Clear();
            cmbKerning.Items.Add("Metrics");
            cmbKerning.Items.Add("Optical");
            cmbKerning.Items.Add("Manual");
            cmbKerning.SelectedIndex = 0;
        }

        /// <summary>
        /// To load Hyphenation languages dictionary from the local application data path
        /// </summary>
        /// <param name="cmbCtl">ComboBox control name</param>
        public void LoadHyphenationLanguages(ComboBox cmbCtl)
        {
            string _languageName = string.Empty;
            string _languageCode = string.Empty;
            string _countryCode = string.Empty;
            if (Directory.Exists(_hyphenationPath))
            {
                string[] fi = Directory.GetFiles(_hyphenationPath);
                cmbCtl.Items.Clear();
                foreach (string fileName in fi)
                {
                    _languageCode = Path.GetFileName(fileName).Substring(5,2);
                    _countryCode = Path.GetFileName(fileName).Substring(8, 2);
                    _languageName = Common.GetLanguageName(_languageCode);
                    cmbCtl.Items.Add(_languageName + " (" + _countryCode + ")" );
                }
                cmbCtl.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// This method loads the default values for Page and paper sizes and Is set the default value based on
        /// the user locale
        /// </summary>
        /// <param name="pageSize">Page Control</param>
        /// <param name="paperSize">Paper Control</param>
        public void SetPageandPaperSetting(ComboBox pageSize, ComboBox paperSize)
        {
            pageSize.Items.Add("A4");
            pageSize.Items.Add("A5");
            pageSize.Items.Add("C5");
            pageSize.Items.Add("Letter");
            pageSize.Items.Add("Custom");

            paperSize.Items.Add("A4");
            paperSize.Items.Add("Letter");

            if (_locUser == "US")
            {
                pageSize.SelectedIndex = 3;
                paperSize.SelectedIndex = 1;
            }
            else
            {
                pageSize.SelectedIndex = 0;
                paperSize.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// This method to load default margin values
        /// </summary>
        /// <param name="inside">Page left TextBox control</param>
        /// <param name="outside">Page right TextBox control</param>
        /// <param name="top">page top TextBox control</param>
        /// <param name="bottom">page bottom TextBox control</param>
        public void SetPageMargins(TextBox inside, TextBox outside, TextBox top, TextBox bottom)
        {
            inside.Text = ".5in";
            outside.Text = ".5in";
            top.Text = ".5in";
            bottom.Text = ".5in";
        }

        /// <summary>
        /// To load the page Column Count
        /// </summary>
        /// <param name="cmbCtl">Control name</param>
        public void SetColumnCount(ComboBox cmbCtl)
        {
            cmbCtl.Items.Add("1");
            cmbCtl.Items.Add("2");
            cmbCtl.Items.Add("3");
            cmbCtl.Items.Add("4");
            cmbCtl.SelectedIndex = 0;
        }

        /// <summary>
        /// Load Font names into the combolist.
        /// </summary>
        /// <param name="cmbCtl">ComboBox Control name</param>
        /// <param name="fontname">Fontname should need to set default</param>
        public void SetFontName(ComboBox cmbCtl, string fontname)
        {
            var fonts = new System.Drawing.Text.InstalledFontCollection();
            foreach (FontFamily family in fonts.Families)
            {
                cmbCtl.Items.Add(family.Name);
            }
            if (cmbCtl.Items.Contains(fontname))
            {
                cmbCtl.Text = fontname; 
            }
            else
            {
                cmbCtl.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Load font style to combo box with default selection
        /// </summary>
        /// <param name="cmbCtl">Combobox Name</param>
        /// <param name="selectedIndex">Default selection Index</param>
        public void SetFontStyle(ComboBox cmbCtl, sbyte selectedIndex)
        {
            //cmbCtl.Items.Add("Regular");
            //cmbCtl.Items.Add("Bold");
            cmbCtl.Items.Add("Normal");
            cmbCtl.Items.Add("Italic");
            cmbCtl.SelectedIndex = selectedIndex;
        }

        /// <summary>
        /// TO clear all Controls value in TabControl
        /// </summary>
        /// <param name="tabCtl">TabControl Name</param>
        public void ClearControls(TabControl tabCtl)
        {
            for (int tabindex = 0; tabindex < tabCtl.TabPages.Count; tabindex++)
            {
                foreach (Control myControl in tabCtl.TabPages[tabindex].Controls)
                {
                    if (myControl is TextBox)
                    {
                        myControl.Text = string.Empty;
                    }
                    else if (myControl is ComboBox)
                    {
                        var cmb = myControl as ComboBox;
                        cmb.Items.Clear();
                    }
                    else if (myControl is ListBox)
                    {
                        var lst = myControl as ListBox;
                        lst.Items.Clear();
                    }
                    else if (myControl is CheckBox)
                    {
                        var lst = myControl as CheckBox;
                        lst.Checked = false;
                    }
                }
            }
        }

        /// <summary>
        /// To calculate the Value based on FontSize
        /// </summary>
        /// <param name="ctl">FontSize Control</param>
        /// <param name="value">Value to be multiply</param>
        /// <returns>Calculated value</returns>
        public string RelativeValidation(Control ctl, float value)
        {
            string result = string.Empty;
            if (ctl.Text != "")
            {
                string userUnit;
                float fontSize;
                if (ctl.Text.Length > 2)
                {
                    userUnit = ctl.Text.Substring(ctl.Text.Length - 2);
                    fontSize = float.Parse(ctl.Text.Remove(ctl.Text.Length - 2, 2));
                }
                else if (Common.ValidateNumber(ctl.Text))
                {
                    userUnit = "pt";
                    fontSize = float.Parse(ctl.Text);
                }
                else
                {
                    userUnit = "pt";
                    fontSize = 0F;
                }
                result = Math.Round(fontSize * value) + userUnit;
            }
            return result;
        }

        /// <summary>
        /// To add unit value if the field only contains digits.
        /// </summary>
        /// <param name="ctrl">Control name</param>
        /// <returns>value with unit</returns>
        public string ConcateUnit(Control ctrl)
        {
            string textValue = ctrl.Text.Trim();
            if (ctrl is TextBox && Common.ValidateNumber(textValue))
            {
                const string unit = "pt";
                int unitExist = textValue.IndexOf(unit);
                if (unitExist < 1)
                {
                    textValue = textValue + unit;
                }
            }
            return textValue;
        }

        /// <summary>
        /// Function to validate Minimum and Maximum values
        /// </summary>
        /// <param name="controlValue">Control value</param>
        /// <param name="minValue">Minimum value</param>
        /// <param name="maxValue">Maximum value</param>
        /// <returns>string which contains error message</returns>
        public string RangeValidate(string controlValue, string minValue, string maxValue)
        {
            string message = string.Empty;
            if (controlValue.Length >= 3)
            {
                string userUnit = controlValue.Substring(controlValue.Length - 2);
                if (userUnit == "in" || userUnit == "pt" || userUnit == "cm" || userUnit == "pc")
                {
                    float userValue = float.Parse(controlValue.Substring(0, controlValue.Length - 2));
                    float convertedMinValue = Common.UnitConverterOO(minValue, userUnit);
                    float convertedMaxValue = Common.UnitConverterOO(maxValue, userUnit);
                    if (userValue < convertedMinValue || userValue > convertedMaxValue)
                    {
                        message = "Enter a value between " + convertedMinValue + userUnit + " to " + convertedMaxValue + userUnit + " inclusive.";
                        return message;
                    }
                }
                else
                {
                    message = "Please enter the valid Input";
                    return message;
                }
            }
            return message;
        }

        /// <summary>
        /// To validate the line spacing based on the Font-size
        /// </summary>
        /// <param name="ctrl">Line spacing control</param>
        /// <param name="fontSize">Font Size control</param>
        /// <returns>Calculated value</returns>
        public string ValidateLineSpacing(Control ctrl, TextBox fontSize)
        {
            string errorMessage = string.Empty;
            if (fontSize.Text != "" && ctrl.Text != "")
            {
                float foSize = float.Parse(fontSize.Text.Remove(fontSize.Text.Length - 2, 2));
                float lineSpacing = float.Parse(ctrl.Text.Remove(ctrl.Text.Length - 2, 2));
                if (foSize > lineSpacing)
                {
                    errorMessage = "Letter Spacing cannot be smaller than FontSize";
                }
                if (lineSpacing >= (foSize * 2))
                {
                    errorMessage = "Letter Spacing cannot be greater or equal to FontSize * 2";
                }
            }
            else
            {
                fontSize.Focus();
            }
            return errorMessage;
        }

        /// <summary>
        /// To calculate line spacing, Space After and Space Before based on Font-Size
        /// </summary>
        /// <param name="ctrl">Fonttsize control</param>
        /// <param name="txtCtl">Control to be calculated</param>
        /// <param name="dicMap">Dictionary to store the result</param>
        /// <param name="mValue">Value to need to multiply</param>
        public void CalculateFontSizeDependent(Control ctrl, TextBox txtCtl, Dictionary<string, string> dicMap, float mValue)
        {
            var units = new ArrayList {"pt", "pc", "cm", "in"};
            //to Validate Units
            var validSize = new Regex("[^a-zA-Z*|.?a-zA-Z*]");

            if (!validSize.IsMatch(ctrl.Text) || ctrl.Text.Length <= 2)
            {
                ctrl.Text = "";
            }

            string errorMessage = RangeValidate(ctrl.Text, "8pt", "14pt");

            txtCtl.Text = "";

            if (ctrl.Text != "" && (errorMessage == string.Empty))
            {
                 string vInput = ctrl.Text.Substring(ctrl.Text.Length - 2, (ctrl.Text.Length - ctrl.Text.Length) + 2);
                if (units.Contains(vInput))
                {
                    txtCtl.Text = RelativeValidation(ctrl, mValue);
                    dicMap[txtCtl.Name] = txtCtl.Text;
                }
                txtCtl.Focus();  
            }
        } 
        
        /// <summary>
        /// To confirm with the user to save even values are not in range
        /// </summary>
        /// <param name="tabSettings">TabControl Name</param>
        /// <param name="errProvider">Error Provider</param>
        /// <returns>True when no error else false</returns>
        public bool Validations(TabControl tabSettings, ErrorProvider errProvider)
        {
            string showMessage = string.Empty;
            var errControl = new Control();
            foreach (Control mCtl in tabSettings.Controls)
            {
                foreach (Control ctl in mCtl.Controls)
                {
                    if (errProvider.GetError(ctl) != "")
                    {
                        errControl = ctl;
                        showMessage += "In the " + errControl.Parent.Text + " tab, " + errControl.Tag + " value is out of range. " + errProvider.GetError(ctl) + "\r\n";
                    }
                }
            }
            if (showMessage != string.Empty)
            {
                var msg = new[] { showMessage + "\r\n" };
                DialogResult dr = LocDB.Message("errSaveEvenOutofRange", showMessage + "\r\n" + "Do you want to save the values even though they are out of range?", msg, LocDB.MessageTypes.YN,
                LocDB.MessageDefault.First);
                //DialogResult dr = MessageBox.Show(showMessage + "\r\n" + "Do you want to save the values even though they are out of range?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dr == DialogResult.Yes)
                {
                    return true;
                }
                if (errControl.Parent.Name != null)
                {
                    tabSettings.SelectTab(errControl.Parent.Name);
                    errControl.Focus();
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets new file Name for Css while saving
        /// </summary>
        /// <param name="fileName">CSS FileName</param>
        /// <param name="suffix">job or scripture</param>
        /// <param name="path">File location path</param>
        /// <param name="onChange">whether it callinf from onsave (or) Onselct the CSSdropdownlist</param>
        /// <returns>new suffix filename with auto generated number</returns>
        public static string GetNewFileName(string fileName, string suffix, string path, string onChange)
        {
            string sourceCSS = string.Empty;
            int suffixPos = fileName.IndexOf("-" + suffix);
            if ((string.Compare(onChange, "onsave") == 0) || (string.Compare(onChange, "onplugin") == 0))
            {
                sourceCSS = suffixPos >= 0 ? fileName.Substring(0, suffixPos) : Path.GetFileNameWithoutExtension(fileName);
            }
            else if (string.Compare(onChange, "onselect") == 0)
            {
                if (suffixPos == -1)
                {
                    suffixPos = fileName.IndexOf(".css");
                }
                sourceCSS = fileName.Substring(0, suffixPos);
            }
            int counter = 1;
            string newName = Common.PathCombine(path, sourceCSS + "-" + suffix + counter + ".css");
            while (File.Exists(newName))
            {
                newName = Common.PathCombine(path, sourceCSS + "-" + suffix + ++counter + ".css");
            }
            return newName;
        }

        /// <summary>
        /// To load width and the height of the page based on the PageSize select in the dropdown
        /// </summary>
        /// <param name="pageSize">PageSize ComboBox</param>
        /// <param name="paperSize">PaperSize ComboBox</param>
        /// <param name="cropMark">To enable the checkbox if pagesize is A5/C5</param>
        /// <param name="txtWidth">Width Textbox</param>
        /// <param name="txtHeight">Height textbox</param>
        public void PageSizeChange(ComboBox pageSize, ComboBox paperSize, CheckBox cropMark, TextBox txtWidth, TextBox txtHeight)
        {
            if (string.IsNullOrEmpty(pageSize.Text))
                return;

            string cval = pageSize.Text.Substring(0, 1).ToUpper() + pageSize.Text.Substring(1);
            paperSize.Enabled = true;
            cropMark.Enabled = true;
            switch (cval)
            {
                case "A4":
                    pageSize.Text = cval;
                    paperSize.Text = cval;
                    txtWidth.Text = "21cm";
                    txtHeight.Text = "29.7cm";
                    paperSize.Enabled = false;
                    cropMark.Enabled = false;
                    cropMark.Checked = false;
                    break;
                case "A5":
                    paperSize.SelectedIndex = 0;
                    pageSize.Text = cval;
                    txtWidth.Text = "14.8cm";
                    txtHeight.Text = "21cm";
                    break;
                case "C5":
                    paperSize.SelectedIndex = 0;
                    pageSize.Text = cval;
                    txtWidth.Text = "16.2cm";
                    txtHeight.Text = "22.9cm";
                    break;
                case "Letter":
                    pageSize.Text = cval;
                    paperSize.Text = cval;
                    txtWidth.Text = "8.5in";
                    txtHeight.Text = "11in";
                    paperSize.Enabled = false;
                    cropMark.Checked = false;
                    cropMark.Enabled = false;
                    break;
            }
        }

        /// <summary>
        /// To load the CSS files from the Dictionary folder
        /// </summary>
        /// <param name="dictPath">Path of the dictionary</param>
        /// <param name="cssPath">CSS file path</param>
        /// <param name="cmbCtl">ComboBox control name</param>
        /// <param name="callFrom">dictionary/scripture to ignore the suffix and load the remaining CSS files</param>
        /// <param name="fromFlexPlugin">PlugIn / Dictionary Express</param>
        public void LoadSourceCSS(string dictPath, string cssPath, ComboBox cmbCtl, string callFrom, bool fromFlexPlugin)
        {
            string cssFileName = Path.GetFileName(cssPath);
            var dirInfo = new DirectoryInfo(dictPath);
            //To load CSS file based on mode(PlugIn or Dictionary Express)
            if (fromFlexPlugin)
            {
                FileInfo[] rgFiles = dirInfo.GetFiles("*.css");
                // To fill the CSS files to ComboBox
                foreach (FileInfo fileName in rgFiles)
                {
                    if (fileName.Name.IndexOf(".css") > 0)
                    {
                        cmbCtl.Items.Add(fileName.Name);
                    }
                }
            }
            else
            {
                string dicSolution = SolutionName;

                //if (!Path.IsPathRooted(dicSolution))
                //    return;

                var doc = new XmlDocument();
                doc.Load(dicSolution);
                XmlNodeList fileList = doc.SelectNodes("/Project/SolutionExplorer/File");
                if (fileList != null)
                    foreach (XmlNode item in fileList)
                    {
                        var cssName = item.Attributes.GetNamedItem("Name").Value;
                        if (item.Attributes.Count > 0)
                        {

                            var t = item.Attributes.GetNamedItem("Default");
                            if (t.Value == "True")
                            {
                                cssFileName = item.Attributes.GetNamedItem("Name").Value;
                            }
                        }

                        if (cssName.IndexOf(".css") > 0)
                        {
                            cmbCtl.Items.Add(cssName);
                        }
                    }
            } 

            if (cssPath != null && cmbCtl.Items.Contains(cssFileName))
            {
                cmbCtl.Text = cssFileName;
            }
            else if (string.IsNullOrEmpty(cmbCtl.Text) && cmbCtl.Items.Count > 0)
            {
                cmbCtl.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// LoadCss - Loads CSS into private class member variable
        /// </summary>
        /// <param name="cssFileName">CSS Filename</param>
        /// <returns>returns TreeNode</returns>
        public TreeNode LoadCss(string dicPath, string cssFileName)
        {
            var cssTree = new CssParser();
            TreeNode cssTreeNode = cssTree.BuildTree(Common.PathCombine(dicPath, cssFileName));
            return cssTreeNode;
        }

        /// <summary>
        /// To validate SpaceAfter / SpaceBefore should not be less than the FontSize
        /// </summary>
        /// <param name="ctrl">FontSize control</param>
        /// <param name="spaceValue">Value of Space After / Sapce Before</param>
        /// <param name="suffix">To display in message for After / Before</param>
        /// <returns>returns errorMessage</returns>
        public string ValidateSpaceSelector(Control ctrl, string spaceValue, string suffix)
        {
            string errorMessage = string.Empty;
            if (ctrl.Text != "" && spaceValue != "")
            {
                float fontSize = float.Parse(ctrl.Text.Remove(ctrl.Text.Length - 2, 2));
                float spaceSelector = float.Parse(spaceValue.Remove(spaceValue.Length - 2, 2));
                if (fontSize > spaceSelector)
                {
                    errorMessage = "Space " + suffix + " cannot be less than FontSize";
                }
            }
            else
            {
                ctrl.Focus();
            }
            return errorMessage;
        }

        /// <summary>
        /// To set the ToolTip Text for the Button Controls
        /// </summary>
        /// <param name="myToolTip">ToolTip Control</param>
        /// <param name="close">Close Button</param>
        /// <param name="save">Save Button</param>
        /// <param name="previous">Previous Button</param>
        /// <param name="next">Next Button</param>
        public void SetToolTip(ToolTip myToolTip, Button close, Button save, Button previous, Button next)
        {
            myToolTip.SetToolTip(close, "Process input file with the settings producing the output");
            myToolTip.SetToolTip(save, "Save the settings in a file for future use");
            myToolTip.SetToolTip(previous, "To move Previous Tab");
            myToolTip.SetToolTip(next, "To move Next Tab");
        }

        /// <summary>
        /// To swap the listItems based on Up and Down buttons
        /// </summary>
        /// <param name="lstView">ListView Control name</param>
        /// <param name="dir">up or down</param>
        public void SwapListView(ListView lstView, string dir)
        {
            try
            {
                ListViewItem selectedItem = null;
                int index = -1;
                foreach (ListViewItem lvwSelectedItem in lstView.SelectedItems)
                {
                    selectedItem = lvwSelectedItem;
                    index = lvwSelectedItem.Index;
                }
                if (selectedItem == null)
                {
                    selectedItem = lstView.Items[0];
                    index = 0;
                }

                ListViewItem swap = selectedItem;
                if (index != -1)
                {
                    if (dir == "UP")
                    {
                        if (index != 0)
                        {
                            lstView.Items.RemoveAt(index);
                            if (swap != null) lstView.Items.Insert(index - 1, swap);
                        }
                    }
                    else if (dir == "DOWN")
                    {
                        if (lstView.Items.Count - 1 != index)
                        {
                            lstView.Items.RemoveAt(index);
                            if (swap != null) lstView.Items.Insert(index + 1, swap);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: SwapListView", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// To load default values for ListView
        /// </summary>
        /// <param name="lstViewCtl">listView control name</param>
        /// <param name="xhtml">xhtml file</param>
        public void LoadSectionNames(ListView lstViewCtl, string xhtml)
        {
            if (lstViewCtl.Items.Count == 0)
            {
                for (int i = 0; i < _arrDictSections.Length; i++)
                {
                    string sectionName = _arrDictSections[i];
                    var tempListView = new ListViewItem {Text = sectionName};
                    if (sectionName == "Lexicon")
                    {
                        tempListView.SubItems.Add(Path.GetFileName(xhtml));
                    }
                    lstViewCtl.Items.Add(tempListView);
                }
            }
        }

        /// <summary>
        /// Checks if the page size is standard or custom
        /// </summary>
        /// <param name="width">Page Width value</param>
        /// <param name="height">Page Height value</param>
        /// <param name="pageSize">Page Size control</param>
        /// <param name="paperSize">Paper Size control</param>
        /// <param name="cropMark">Crop Mark checkBox control</param>
        public void NonstandardSize(string width, string height, ComboBox pageSize, ComboBox paperSize, CheckBox cropMark)
        {
            try
            {
                string dims = width + "x" + height;
                var standardSize = new Dictionary<string, string>();
                standardSize["21cmx29.7cm"] = "A4";
                standardSize["14.8cmx21cm"] = "A5";
                standardSize["16.2cmx22.9cm"] = "C5";
                standardSize["8.5inx11in"] = "Letter";
                pageSize.Text = standardSize.ContainsKey(dims) ? standardSize[dims] : "Custom";
                if (pageSize.Text == "A4" || pageSize.Text == "Letter")
                {
                    paperSize.Text = pageSize.Text;
                    paperSize.Enabled = false;
                    cropMark.Checked = false;
                    cropMark.Enabled = false;
                }
                else
                {
                    paperSize.Enabled = true;
                    cropMark.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Express: NonstandardSize", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// To remove the tab when load
        /// </summary>
        /// <param name="t">Tab index for remove</param>
        /// <param name="tbCntl">Tab control name</param>
        public void RemoveTab(int t, TabControl tbCntl)
        {
            try
            {
                tbCntl.TabPages.RemoveAt(t);
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Express: RemoveTab", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void DisableColumnControls(string value, Control ctrl1, Control ctrl2)
        {
            if(value == "1")
            {
                ctrl1.Enabled = false;
                ctrl2.Enabled = false;
            }
            else
            {
                ctrl1.Enabled = true;
                ctrl2.Enabled = true;
            }
        }
        #endregion
    }
}
