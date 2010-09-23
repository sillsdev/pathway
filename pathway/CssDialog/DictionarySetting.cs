// --------------------------------------------------------------------------------------------
// <copyright file="DictionarySetting.cs" from='2009' to='2009' company='SIL International'>
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
// Settings for Dictionary
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Collections;
using JWTools;
using SIL.Tool;
using SIL.Tool.Localization;

namespace SIL.PublishingSolution
{
    /// <summary>
    /// Events and code for DictionarySettings form
    /// </summary>
    public partial class DictionarySetting : Form
    {
        #region private variable
        TreeNode _cssTree;
        bool _confirmSave;
        bool _fileSaved;
        string _pageHeaderFooterPos = string.Empty;
        string _langTagProperty = string.Empty;
        string _textWritingSystemName = string.Empty;
        readonly string _supportFolder;
        readonly string _dicPath;
        readonly string _cssPath;
        readonly string _xhtml;
        readonly string _solutionName;
        string[] _xpath;
        readonly bool _plugin;
        readonly char[] _delim = { '/' };
        readonly SettingBl _commonSettings = new SettingBl();
        readonly public ErrorProvider _errProvider = new ErrorProvider();
        readonly ArrayList _units = new ArrayList();
        readonly ArrayList _stepDtdFile = new ArrayList();
        readonly Dictionary<string, string> _dicMap = new Dictionary<string, string>();
        readonly Dictionary<string, string> _dictLexiconPrepStepsFilenames = new Dictionary<string, string>();
        readonly Dictionary<string, string> _dictSectionFilenames = new Dictionary<string, string>();
        readonly Dictionary<string, string> _fileTypeFilter = new Dictionary<string, string>();
        readonly Dictionary<string, string> _dictLangTagProperty = new Dictionary<string, string>();
        readonly Dictionary<string, Dictionary<string, string>> _dictStepFilenames = new Dictionary<string, Dictionary<string, string>>();
        #endregion

        #region public variable
        public string JobName;
        public string LastSaved = string.Empty;
        public bool SetAsDefaultCSS { get; set; }
        public string OutputexportType;
        public string SymbolValue;
        public string FontName;
        #endregion

        #region Constructor
        /// <summary>
        /// To load the default values
        /// </summary>
        /// <param name="dicPath">De Path</param>
        /// <param name="cssPath">CSS Path</param>
        /// <param name="fromPlugin">Is FlugIn or Not</param>
        /// <param name="xhtmlName">XHTML File Name</param>
        /// <param name="solutionName">Solution Name</param>
        public DictionarySetting(string dicPath, string cssPath, bool fromPlugin, string xhtmlName, string solutionName)
        {
            try
            {
                InitializeComponent();
                _dicPath = dicPath;
                _cssPath = cssPath;
                _plugin = fromPlugin;
                _xhtml = xhtmlName;
                _solutionName = solutionName;
                _supportFolder = Common.GetPSApplicationPath();
            }
            catch (Exception ex)
            {
                var msg = new[] {ex.Message};
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: Constructor", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public DictionarySetting()
        {
            InitializeComponent();
        }
 
        #endregion Constructor

        #region Load
        #region DictionarySetting_Load
        private void DictionarySetting_Load(object sender, EventArgs e)
        {
            LocDB.Localize(this, null);     // Form Controls
            Common.HelpProv.SetHelpNavigator(this, HelpNavigator.Topic);
            Common.HelpProv.SetHelpKeyword(this, "DictionaryDocumentTab.htm");
            try
            {
                DoLoad();
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: Load", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion DictionarySetting_Load

        #region DoLoad
        /// <summary>
        /// To load the default values
        /// </summary>
        public void DoLoad()
        {
            try
            {
                _commonSettings.SetToolTip(tTDone, btnDone, btnSave, btnPrevious, btnNext);
                _commonSettings.SolutionName = _solutionName;
                // OO Utility
                if (_plugin)
                {
                    chkDefaultCSS.Visible = false;
                    btnDone.Text = "OK";
                    lblShowProperty.Text = "Output Type";
                    // Source CSS
                    _commonSettings.LoadSourceCSS(_dicPath, _cssPath, ddlCSS, "dictionary", true);
                }
                else
                {
                    ddlShowProperty.Items.Add("ALL");
                    // Source CSS
                    _commonSettings.LoadSourceCSS(_dicPath, _cssPath, ddlCSS, "dictionary", false);
                }

                //Load CSS
                _cssTree = _commonSettings.LoadCss(_dicPath, ddlCSS.Text);
                LoadDefault();
                _confirmSave = false;
                // Show Property - OP
                ArrayList exportType = Common.GetExportType();
                foreach (string item in exportType)
                {
                    ddlShowProperty.Items.Add(item);
                }
                ddlShowProperty.SelectedIndex = _plugin ? 0 : 1;

                // Remove tabs not implemented
                _commonSettings.RemoveTab(9, tabDicSetting); // Advanced
                _commonSettings.RemoveTab(5, tabDicSetting);  // Fields
                //commonSettings.RemoveTab(1, tabDicSetting);  // Preparation
                if (_plugin)
                {
                    _commonSettings.RemoveTab(0, tabDicSetting);  // Document
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: DoLoad", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion DoLoad

        #region GetDefault
        /// <summary>
        /// GetDefault - Get default value from css
        /// </summary>
        /// <param name="s_xpath">string representation of xpath divided by slashes</param>
        /// <returns>array of values in css</returns>
        public string[] GetDefault(string xPath)
        {
            try
            {
                _xpath = xPath.Split(_delim);
                TreeNode prop = DoXPath(_cssTree, 0);
                return AccumulateResult(prop);
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting GetDefault", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        private string[] GetDefault(string xPath, string xPath2)
        {
            try
            {
                // First search finds rule for class
                _xpath = xPath.Split(_delim);
                TreeNode cls = DoXPath(_cssTree, 0);
                if (cls == null)
                    return new string[0];
                // Second search finds proprty
                _xpath = xPath2.Split(_delim);
                TreeNode prop = DoXPath(cls.Parent.Parent, 0);
                return AccumulateResult(prop);
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: GetDefaultx2", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        private string[] GetDefault(string xPath1, string xPath2, string xPath3)
        {
            try
            {
                // First search finds Media for ClassAndProperty
                _xpath = xPath1.Split(_delim);
                TreeNode root = DoXPath(_cssTree, 0);
                if (root == null)
                    return new string[0];
                // Second search finds ClassAndProperty for Class
                _xpath = xPath2.Split(_delim);
                TreeNode cls = DoXPath(root.Parent, 0);
                if (cls == null)
                    return new string[0];
                // Third search finds proprty
                _xpath = xPath3.Split(_delim);
                TreeNode prop = DoXPath(cls.Parent.Parent, 0);
                return AccumulateResult(prop);
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: GetDefaultx3", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }
        #endregion GetDefault

        #region GetDefaultClass
        private string GetDefaultClass(string xPath)
        {
            try
            {
                _xpath = xPath.Split(_delim);
                TreeNode prop = DoXPath(_cssTree, 0);
                if (prop != null)
                {
                    return prop.Text;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: GetDefaultClass", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        #endregion GetDefaultClass

        #region DoXPath
        /// <summary>
        /// DoXPath - recursively traverse tree from node using xpath
        /// </summary>
        /// <param name="node">node of parse tree to start with</param>
        /// <param name="lev">list of terms in path</param>
        /// <returns>node of resulting property</returns>
        private TreeNode DoXPath(TreeNode node, int lev)
        {
            try
            {
                if (lev == _xpath.Length)
                    return node;
                foreach (TreeNode child in node.Nodes)
                {
                    if (child.Text == _xpath[lev])
                    {
                        if (child.Text == "ATTRIB" && (child.FirstNode.Text == _xpath[3]))
                        {
                            if (child.LastNode.Text.Replace("\'", "") == _xpath[_xpath.Length - 1])
                            {
                                return child;
                            }
                        }
                        TreeNode res = DoXPath(child, lev + 1);
                        // for Sense Panel
                        if (res == null)
                        {
                            if (_xpath.Length == 5 && child.Text == _xpath[2]
                                && child.Parent.NextNode.Text == "PSEUDO")
                            {
                                if (child.Parent.NextNode.FirstNode.Text == _xpath[_xpath.Length - 1])
                                {
                                    res = child.Parent.NextNode.FirstNode;
                                }
                            }
                            else if (_xpath.Length == 6 && child.Text == _xpath[2])
                                //&& child.Parent.NextNode.NextNode.FirstNode.Text == xpath[5])
                            {
                                if (child.Parent.NextNode != null && child.Parent.NextNode.Text == _xpath[3])
                                {
                                    if (child.Parent.NextNode.NextNode != null && child.Parent.NextNode.NextNode.FirstNode.Text == _xpath[5])
                                    {
                                        res = child;
                                    }
                                }
                            }
                        }
                        if (res != null)
                            return res;
                    }
                    else if (child.Text == "REGION")
                    {
                        if (child.FirstNode.Text == _pageHeaderFooterPos)
                        {
                            foreach (TreeNode item in child.Nodes)
                            {
                                if (item.Text == "PROPERTY" && item.FirstNode.Text == _xpath[1])
                                {
                                    return item.FirstNode;
                                }
                            }
                        }
                    }
                    else if (lev == _xpath.Length - 1)
                    {
                        return null;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: DoXPath", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }
        #endregion DoXPath

        #region AccumulateResult
        /// <summary>
        /// AccumulateResult - convert nodes on same level as property name to string array
        /// </summary>
        /// <param name="prop">node of property name</param>
        /// <returns>string array of values of property</returns>
        private static string[] AccumulateResult(TreeNode prop)
        {
            try
            {
                if (prop == null)
                    return new string[0];
                var resList = new List<string>();
                while (true)
                {
                    prop = prop.NextNode;
                    if (prop == null)
                        break;

                    // If value starts with one of these characters, assume it is a number
                    if ("0123456789.-".IndexOf(prop.Text[0]) > 0)
                    {
                        string val = prop.Text;
                        prop = prop.NextNode;
                        if (prop != null)
                        {
                            resList.Add(val + prop.Text);
                            continue;
                        }
                        resList.Add(val);
                        break;
                    }
                    resList.Add(prop.Text);
                }
                var res = new string[resList.Count];
                resList.CopyTo(res);
                return res;
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: AccumulateResult", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }
        #endregion AccumulateResult

        #region LoadLanguages
        /// <summary>
        /// To load languages from the lang attribute from the XHTML file in the Dictionary.de folder
        /// </summary>
        /// <param name="item">ListBox Control Name</param>
        private void LoadLanguages(ListBox item)
        {
            try
            {
                Dictionary<string, string> arrLang = new Dictionary<string, string>();
                //MessageBox.Show("LoadLanguages file=" + xhtml);
                XmlTextReader rdr = new XmlTextReader(_xhtml);
                rdr.XmlResolver = null;
                string language;
                while (rdr.Read())
                {
                    language = rdr.GetAttribute("lang");
                    if (!string.IsNullOrEmpty(language))
                    {
                        arrLang[language] = language;
                    }
                }
                rdr.Close();

                foreach (string lang in arrLang.Values)
                {
                    if (lang != "utf-8")
                    {
                        item.Items.Add(lang);
                    }
                }

            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: LoadLanguages", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion LoadLanguages

        #region LoadDefault
        private void LoadDefault()
        {
            var value = new string[5] { "txtFF", "txtFS", "txtC", "txtLS", "txtPHP" };
            var propName = new string[5] { "fontfamily", "fontsize", "color", "letterspacing", "princehyphenatepatterns" };
            try
            {
                _commonSettings.ClearControls(tabDicSetting);

                // Page Tab Values
                _commonSettings.SetPageandPaperSetting(ddlPagePageSize, ddlPagePaperSize);
                _commonSettings.SetPageMargins(txtPageInside, txtPageOutside, txtPageTop, txtPageBottom);
                _commonSettings.SetColumnCount(ddlPageColumn);
                SetFontWeight();
                _commonSettings.SetFontStyle(ddlHeadingsLetterFontStyle, 0);
                if (txtHeadingsLetterFontSize.Text == "")
                    txtHeadingsLetterFontSize.Text = "24pt";
                txtHeadingsLetterLineSpacing.Text = _commonSettings.RelativeValidation(txtHeadingsLetterFontSize, 1.10F);
                _dicMap["txtHeadingsLetterLineSpacing"] = txtHeadingsLetterLineSpacing.Text;
                txtHeadingsLetterSpaceAfter.Text = _commonSettings.RelativeValidation(txtHeadingsLetterFontSize, 1.5F);
                _dicMap["txtHeadingsLetterSpaceAfter"] = txtHeadingsLetterSpaceAfter.Text;
                txtHeadingsLetterSpaceBefore.Text = _commonSettings.RelativeValidation(txtHeadingsLetterFontSize, 1.5F);
                _dicMap["txtHeadingsLetterSpaceBefore"] = txtHeadingsLetterSpaceBefore.Text;

                txtHeadingsRunningFontSize.Text = "11pt";

                txtEntriesHangingIndent.Text = "2pc";
                txtEntriesSpaceAfter.Text = "2pt";
                txtEntriesSpaceBefore.Text = "1pt";

                LoadLanguages(lbTextWritingSystem); // To fill the languages from the XHTML file.
                _commonSettings.SetFontName(ddlTextTagFontName, "Charis SIL");
                txtTextBetweenLettersLetterDesired.Text = "0";
                _commonSettings.LoadKerningValues(ddlTextKerning);
                _commonSettings.LoadHyphenationLanguages(ddlTextHyphenationLanguage);
                    // To load Hyphenation language dictionary
                if (lbTextWritingSystem.Items.Count > 0)
                {
                    lbTextWritingSystem.SelectedIndex = 0;
                    if (_dictLangTagProperty.ContainsKey(lbTextWritingSystem.Items[0].ToString()))
                    {
                        chkTextIncludeLanguageTags.Checked = true;
                        LoadLanguageProperty(lbTextWritingSystem.Items[0].ToString());
                    }
                }

                _commonSettings.SetColumnCount(ddlIndexesColumns);

                ddlMediaPosition.Items.Add("right");
                ddlMediaPosition.Items.Add("left");
                ddlMediaPosition.Items.Add("center");
                ddlMediaPosition.SelectedIndex = 0;

                chkMediaImport.Checked = true;

                ddlMediaSize.Items.Add("2.5cm high");
                ddlMediaSize.Items.Add("2.5cm wide");
                ddlMediaSize.SelectedIndex = 0;
                _commonSettings.SetFontStyle(ddlMediaHeaderFontStyle, 1);
                ddlMediaRules.Items.Add("all");
                ddlMediaRules.Items.Add("inside");
                ddlMediaRules.Items.Add("outside");
                ddlMediaRules.Items.Add("horizondal");
                ddlMediaRules.Items.Add("vertical");
                ddlMediaRules.SelectedIndex = 0;

                ddlFieldsBasis.Items.Add("stem");
                ddlFieldsBasis.Items.Add("root");
                //ddlFieldsBasis.SelectedIndex = 0;

                ddlSensesNumberSenses.Items.Add("numbers");
                ddlSensesNumberSenses.Items.Add("symbols");
                ddlSensesNumberSenses.SelectedIndex = 0;

                if (txtSensesPunctuation.Text == "")
                    txtSensesPunctuation.Text = ")";

                ddlHeadingsPageNumberLocation.Items.Add("top center");
                ddlHeadingsPageNumberLocation.Items.Add("bottom Center");
                ddlHeadingsPageNumberLocation.SelectedIndex = 0;

                string[] dims = GetDefault("PAGE/PROPERTY/size");
                if (dims.Length == 2)
                {
                    txtPageWidth.Text = dims[0];
                    txtPageHeight.Text = dims[1];
                    _commonSettings.NonstandardSize(txtPageWidth.Text, txtPageHeight.Text, ddlPagePageSize,
                                                   ddlPagePaperSize, chkPageCropMark);
                }
                string[] margins = GetDefault("PAGE/PROPERTY/margin");
                switch (margins.Length)
                {
                    case 4:
                        txtPageTop.Text = margins[0];
                        txtPageOutside.Text = margins[1];
                        txtPageBottom.Text = margins[2];
                        txtPageInside.Text = margins[3];
                        break;
                    case 3:
                        txtPageTop.Text = margins[0];
                        txtPageOutside.Text = txtPageInside.Text = margins[1];
                        txtPageBottom.Text = margins[2];
                        break;
                    case 2:
                        txtPageTop.Text = txtPageBottom.Text = margins[0];
                        txtPageOutside.Text = txtPageInside.Text = margins[1];
                        break;
                    case 1:
                        txtPageTop.Text = txtPageBottom.Text = txtPageOutside.Text = txtPageInside.Text = margins[0];
                        break;
                }
                string[] margint = GetDefault("PAGE/PROPERTY/margin-top");
                if (margint.Length == 1)
                {
                    txtPageTop.Text = margint[0];
                }
                string[] marginb = GetDefault("PAGE/PROPERTY/margin-bottom");
                if (marginb.Length == 1)
                {
                    txtPageBottom.Text = marginb[0];
                }
                string[] marginl = GetDefault("PAGE/PROPERTY/margin-left");
                if (marginl.Length == 1)
                {
                    txtPageInside.Text = marginl[0];
                }
                string[] marginr = GetDefault("PAGE/PROPERTY/margin-right");
                if (marginr.Length == 1)
                {
                    txtPageOutside.Text = marginr[0];
                }
                string[] mark = GetDefault("PAGE/PROPERTY/marks");
                if (mark.Length > 0)
                {
                    if (ddlPagePaperSize.Enabled && mark[0] == "crop")
                    {
                        chkPageCropMark.Checked = true;
                    }
                    else
                    {
                        chkPageCropMark.Checked = false;
                    }
                }
                string[] regTop = GetDefault("PAGE/REGION/top-center");
                if (regTop.Length > 0)
                {
                    ddlHeadingsPageNumberLocation.Text = "Top center";
                    chkHeadingsGuideWords.Checked = true;
                    string[] topFSize = GetDefault("PAGE/REGION/top-center", "PROPERTY/font-size");
                    if (topFSize.Length > 0)
                    {
                        txtHeadingsRunningFontSize.Text = topFSize[0];
                    }
                }
                string[] regBottom = GetDefault("PAGE/REGION/bottom-center");
                if (regBottom.Length > 0)
                {
                    ddlHeadingsPageNumberLocation.Text = "Bottom Center";
                    chkHeadingsGuideWords.Checked = true;
                    string[] bottomFSize = GetDefault("PAGE/REGION/bottom-center", "PROPERTY/font-size");
                    if (bottomFSize.Length > 0)
                    {
                        txtHeadingsRunningFontSize.Text = bottomFSize[0];
                    }
                }

                // Add the Default List Items from Array to Sections - if its no from flex plugin
                if (!_plugin)
                {
                    GetPageSectionSteps(lvwDocumentSection, lvwPreparationSteps);
                    // if no Section is added then fill it from Array to load it.
                    //Document Tab
                    _commonSettings.LoadSectionNames(lvwDocumentSection, _xhtml);
                }

                string[] paperSize = GetDefault("PAGE/PROPERTY/-ps-paper-size");
                if (paperSize.Length == 1)
                {
                    ddlPagePaperSize.Text = paperSize[0];
                }
                string[] headString = GetDefault("PAGE/PROPERTY/-ps-heading-string");
                if (headString.Length == 1)
                {
                    txtHeadingsString.Text = headString[0].Replace("\"", "");
                }
                string[] gutter = GetDefault("RULE/CLASS/letData", "PROPERTY/column-gap");
                if (gutter.Length > 0)
                {
                    txtPageGutterWidth.Text = gutter[0];
                }
                string[] cols = GetDefault("RULE/CLASS/letData", "PROPERTY/column-count");
                if (cols.Length > 0)
                {
                    ddlPageColumn.Text = cols[0];
                }
                string[] colRule = GetDefault("RULE/CLASS/letData", "PROPERTY/column-rule");
                if (colRule.Length > 0)
                {
                    chkPageVerticalRule.Checked = colRule[0] != "none";
                }
                string[] fHeadFamily = GetDefault("RULE/CLASS/letHead", "PROPERTY/font-family");
                if (fHeadFamily.Length > 0)
                {
                    txtHeadingsFontName.Text = fHeadFamily[0].Replace("\"", "");
                }
                else
                {
                    string[] fFamily = GetDefault("RULE/CLASS/letter", "PROPERTY/font-family");
                    if (fFamily.Length > 0)
                    {
                        txtHeadingsFontName.Text = fFamily[0].Replace("\"", "");
                    }
                }
                string[] fSize = GetDefault("RULE/CLASS/letHead", "PROPERTY/font-size");
                if (fSize.Length > 0)
                {
                    txtHeadingsLetterFontSize.Text = fSize[0];
                }
                string[] fWeight = GetDefault("RULE/CLASS/letHead", "PROPERTY/font-weight");
                if (fWeight.Length > 0)
                {
                    ddlHeadingsFontWeight.Text = fWeight[0];
                }
                string[] fStyle = GetDefault("RULE/CLASS/letHead", "PROPERTY/font-style");
                if (fStyle.Length > 0)
                {
                    ddlHeadingsLetterFontStyle.Text = fStyle[0];
                }
                //string[] tAlign = GetDefault("RULE/CLASS/letHead", "PROPERTY/text-align");
                //if (tAlign.Length > 0)
                //{
                //    chkPageVerticalRule.Checked = true;
                //}
                string[] hAlign = GetDefault("RULE/CLASS/letter", "PROPERTY/text-align");
                if (hAlign.Length > 0)
                {
                    chkHeadingsLetterCentered.Checked = hAlign[0] != "none";
                }
                string[] lSpacing = GetDefault("RULE/CLASS/letHead", "PROPERTY/line-height");
                if (lSpacing.Length > 0)
                {
                    txtHeadingsLetterLineSpacing.Text = lSpacing[0];
                }
                string[] pTop = GetDefault("RULE/CLASS/letHead", "PROPERTY/padding-top");
                if (pTop.Length > 0)
                {
                    txtHeadingsLetterSpaceBefore.Text = pTop[0];
                }
                string[] pBottom = GetDefault("RULE/CLASS/letHead", "PROPERTY/padding-bottom");
                if (pBottom.Length > 0)
                {
                    txtHeadingsLetterSpaceAfter.Text = pBottom[0];
                }
                string[] bBottom = GetDefault("RULE/CLASS/letHead", "PROPERTY/border-bottom");
                if (bBottom.Length > 0)
                {
                    chkHeadingsLetterDividerLine.Checked = bBottom[0] != "none";
                    //chkHeadingsLetterDividerLine.Checked = true;
                }
                

                string[] guideHomograph = GetDefault("PAGE/PROPERTY/-ps-HeadingsHomonymNumber");
                if (guideHomograph.Length > 0)
                {
                    chkHeadingsHomonymNumber.Checked = guideHomograph[0] != "none";
                }
                string[] guideWords = GetDefault("PAGE/PROPERTY/-ps-HeadingsGuideWords");
                if (guideWords.Length > 0)
                {
                    chkHeadingsGuideWords.Checked = guideWords[0] != "none";
                }
                string[] entryTIndent = GetDefault("RULE/CLASS/entry", "PROPERTY/text-indent");
                if (entryTIndent.Length > 0)
                {
                    txtEntriesHangingIndent.Text = entryTIndent[0].Replace("-", "");
                }
                string[] entryTFontSize = GetDefault("RULE/CLASS/entry", "PROPERTY/font-size");
                if (entryTFontSize.Length > 0)
                {
                    txtEntriesFontSize.Text = entryTFontSize[0];
                    txtEntriesLineSpacing.Text = _commonSettings.RelativeValidation(txtEntriesFontSize, 1.10F);
                    _dicMap["txtEntriesLineSpacing"] = txtEntriesLineSpacing.Text;
                }
                string[] entryLSpace = GetDefault("RULE/CLASS/entry", "PROPERTY/line-height");
                if (entryLSpace.Length > 0)
                {
                    txtEntriesLineSpacing.Text = entryLSpace[0];
                }
                string[] entrySBefore = GetDefault("RULE/CLASS/entry", "PROPERTY/padding-top");
                if (entrySBefore.Length > 0)
                {
                    txtEntriesSpaceBefore.Text = entrySBefore[0];
                }
                string[] entrySAfter = GetDefault("RULE/CLASS/entry", "PROPERTY/padding-bottom");
                if (entrySAfter.Length > 0)
                {
                    txtEntriesSpaceAfter.Text = entrySAfter[0];
                }
                string[] sensePara = GetDefault("RULE/CLASS/sense", "PROPERTY/display");
                if (sensePara.Length > 0)
                {
                    chkSensesSensesParagraph.Checked = sensePara[0] == "block";
                }
                string[] sensePunc = GetDefault("RULE/CLASS/xsensenumber/PSEUDO/after", "PROPERTY/content");
                if (sensePunc.Length > 0)
                {
                    if (sensePunc[0].Replace("\"", "") != "none")
                    {
                        txtSensesPunctuation.Text = sensePunc[0].Replace("\"", "");
                        ddlSensesNumberSenses.Text = "numbers";
                    }
                    else
                    {
                        txtSensesPunctuation.Text = "";
                    }
                }

                string[] senseSymbol = GetDefault("RULE/CLASS/xsensenumber/PSEUDO/before", "PROPERTY/content");
                if (senseSymbol.Length > 0)
                {
                    string senseContent = senseSymbol[0].Replace("\"", "");
                    senseContent = senseContent.Replace("'", "").Trim();

                    if (senseContent != "none")
                    {
                        string allSymbols = string.Empty;
                        if (senseContent.IndexOf(' ') > 0)
                        {
                            string[] symbols = senseContent.Split(' ');
                            foreach (string symb in symbols)
                            {
                                allSymbols = allSymbols + Common.ConvertUnicodeToString(symb.Replace("\"", ""));
                            }
                        }
                        else
                        {
                            allSymbols = Common.ConvertUnicodeToString(senseSymbol[0].Replace("\"", ""));
                        }
                        txtSensesSymbols.Text = allSymbols;
                        ddlSensesNumberSenses.Text = "symbols";
                    }
                    else
                    {
                        txtSensesSymbols.Text = "";
                    }
                }
                string[] symbolFontName = GetDefault("RULE/CLASS/xsensenumber/PSEUDO/before", "PROPERTY/font-family");
                txtSensesSymbols.Font = symbolFontName.Length == 1
                                            ? new Font(symbolFontName[0].Replace("\"", ""), 12.0F)
                                            : new Font("arial", 12.0F);
                if (lbTextWritingSystem.Items.Count > 0)
                {
                    string fontfamily = string.Empty;
                    string fontsize = string.Empty;
                    string color = string.Empty;
                    string letterspacing = string.Empty;
                    string princehyphenatepatterns = string.Empty;
                    Dictionary<string, string> dictList = new Dictionary<string, string>();
                    dictList.Add("txtFF", "fontfamily");
                    dictList.Add("txtFS", "fontsize");
                    dictList.Add("txtC", "color");
                    dictList.Add("txtLS", "letterspacing");
                    dictList.Add("txtPHP", "princehyphenatepatterns");

                    foreach (string item in lbTextWritingSystem.Items)
                    {
                        string[] txtFF = GetDefault("RULE/ANY/ATTRIB/lang/ATTRIBEQUAL/" + item, "PROPERTY/font-family");
                        string[] txtFS = GetDefault("RULE/ANY/ATTRIB/lang/ATTRIBEQUAL/" + item, "PROPERTY/font-size");
                        string[] txtC = GetDefault("RULE/ANY/ATTRIB/lang/ATTRIBEQUAL/" + item, "PROPERTY/color");
                        string[] txtLS = GetDefault("RULE/ANY/ATTRIB/lang/ATTRIBEQUAL/" + item, "PROPERTY/letter-spacing");
                        string[] txtPHP = GetDefault("RULE/ANY/ATTRIB/lang/ATTRIBEQUAL/" + item, "PROPERTY/prince-hyphenate-patterns");
                        fontfamily = GetValue(fontfamily, txtFF);
                        fontsize = GetValue(fontsize, txtFS);
                        color = GetValue(color, txtC);
                        letterspacing = GetValue(letterspacing, txtLS);
                        princehyphenatepatterns = GetValue(princehyphenatepatterns, txtPHP);
                        _dictLangTagProperty[item] = fontfamily + "," + fontsize + "," + color + "," + letterspacing + "," + princehyphenatepatterns;
                    }
                }

                string mediaRule = string.Empty;
                var lstRule = new string[] { "all", "inside", "outside", "horizondal", "vertical" };
                foreach (string rname in lstRule)
                {
                    string[] iMedia = GetDefault("MEDIA/" + rname);
                    if (iMedia.Length > 0)
                    {
                        ddlMediaRules.Text = rname;
                        mediaRule = rname;
                    }
                }
                string[] importMedia = GetDefault("MEDIA/" + mediaRule, "RULE/CLASS/common", "PROPERTY/import");
                chkMediaImport.Checked = importMedia.Length > 0;
                string[] posMedia = GetDefault("MEDIA/" + mediaRule, "RULE/CLASS/common", "PROPERTY/position");
                if (posMedia.Length > 0)
                {
                    ddlMediaPosition.Text = posMedia[0];
                }
                string[] fStyleMedia = GetDefault("MEDIA/" + mediaRule, "RULE/CLASS/common", "PROPERTY/font-style");
                if (fStyleMedia.Length > 0)
                {
                    ddlMediaHeaderFontStyle.Text = fStyleMedia[0];
                }
                string[] fSizeMedia = GetDefault("MEDIA/" + mediaRule, "RULE/CLASS/common", "PROPERTY/font-size");
                if (fSizeMedia.Length > 0)
                {
                    txtMediaDataFontSize.Text = fSizeMedia[0];
                }
                string[] sizeMedia = GetDefault("MEDIA/" + mediaRule, "RULE/CLASS/common", "PROPERTY/size");
                if (sizeMedia.Length > 0)
                {
                    ddlMediaSize.Text = sizeMedia[0] + " " + sizeMedia[1];
                }

                string[] colCountIndexes = GetDefault("RULE/CLASS/revData", "PROPERTY/column-count");
                if (colCountIndexes.Length > 0)
                {
                    ddlIndexesColumns.Text = colCountIndexes[0];
                }
                string[] colGapIndexes = GetDefault("RULE/CLASS/revData", "PROPERTY/column-gap");
                if (colGapIndexes.Length > 0)
                {
                    txtIndexesGutterWidth.Text = colGapIndexes[0];
                }
                string[] colRuleIndexes = GetDefault("RULE/CLASS/revData", "PROPERTY/column-rule");
                if (colRuleIndexes.Length > 0)
                {
                    chkIndexesVerticalRuleInGutter.Checked = colRuleIndexes[0] != "none";
                }
                string[] indexSeperator = GetDefault("RULE/CLASS/revSense/PRECEDES/CLASS/revSense", "PROPERTY/content");
                if (indexSeperator.Length > 0)
                {
                    if (indexSeperator[0] == "&amp;")
                    {
                        indexSeperator[0] = "&";
                    }
                    txtIndexesSenseSeperator.Text = indexSeperator[0].Trim();
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: LoadDefault", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion LoadDefault

        private static string GetValue(string propName, string[] value)
        {
            propName = value.Length > 0 ? value[0] : "";
            return propName;
        }

        #region SetFontWeight
        /// <summary>
        /// To load default font-weight values
        /// </summary>
        private void SetFontWeight()
        {
            try
            {
                ddlHeadingsFontWeight.Items.Add("normal");
                ddlHeadingsFontWeight.Items.Add("bold");
                ddlHeadingsFontWeight.Items.Add("bolder");
                ddlHeadingsFontWeight.Items.Add("lighter");
                ddlHeadingsFontWeight.Items.Add("100");
                ddlHeadingsFontWeight.Items.Add("200");
                ddlHeadingsFontWeight.Items.Add("300");
                ddlHeadingsFontWeight.Items.Add("400");
                ddlHeadingsFontWeight.Items.Add("500");
                ddlHeadingsFontWeight.Items.Add("600");
                ddlHeadingsFontWeight.Items.Add("700");
                ddlHeadingsFontWeight.Items.Add("800");
                ddlHeadingsFontWeight.Items.Add("900");
                ddlHeadingsFontWeight.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: SetFontWeight", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion SetFontWeight

        #region GetPageSectionSteps
        /// <summary>
        /// Gets the Details from de file and stores in Dictionary
        /// </summary>
        /// <param name="lvwDocumentSection">Section Listview</param>
        /// <param name="lvwPreparationSteps">Steps Listview</param>
        private void GetPageSectionSteps(ListView lvwDocumentSection, ListView lvwPreparationSteps)
        {
            try
            {
                if (lvwDocumentSection != null)
                {
                    lvwDocumentSection.Items.Clear();
                }
                if (lvwPreparationSteps != null)
                {
                    lvwPreparationSteps.Items.Clear();
                }

                XmlDocument projXml = new XmlDocument();
                string XPath = "";
                XmlElement root = null;
                XmlNode tempNodeSearch = null;

                projXml.Load(_solutionName);
                root = projXml.DocumentElement;

                XPath = "DocumentSettings/Sections";
                tempNodeSearch = root.SelectSingleNode(XPath);   // Default to Document Settings
                string attName = "";
                string attValue = "";

                if (tempNodeSearch != null)
                {
                    for (int i = 0; i < tempNodeSearch.ChildNodes.Count; i++)
                    {
                        for (int j = 0; j < tempNodeSearch.ChildNodes[i].Attributes.Count; j++)
                        {
                            string attString = tempNodeSearch.ChildNodes[i].Attributes[j].Name;
                            if (attString == "Name")
                            {
                                attName = tempNodeSearch.ChildNodes[i].Attributes[j].Value;
                            }
                            else if (attString == "Value")
                            {
                                attValue = tempNodeSearch.ChildNodes[i].Attributes[j].Value;
                            }
                        }

                        if (attName == "Lexicon")
                        {
                            _dictSectionFilenames[attName] = Path.GetFileName(attValue); // default xhtml file
                            attValue = _dictSectionFilenames[attName];
                        }
                        else
                        {
                            _dictSectionFilenames[attName] = attValue;
                        }
                        var tempListView = new ListViewItem();
                        tempListView.Text = attName;
                        tempListView.SubItems.Add(attValue);
                        if (lvwDocumentSection != null) lvwDocumentSection.Items.Add(tempListView);

                        if (tempNodeSearch.ChildNodes[i].HasChildNodes)
                        {
                            XmlNode childNodes = tempNodeSearch.ChildNodes[i];
                            for (int ii = 0; ii < childNodes.ChildNodes.Count; ii++)
                            {
                                string attChildName = "";
                                string attChildValue = "";
                                for (int jj = 0; jj < childNodes.ChildNodes[ii].Attributes.Count; jj++)
                                {
                                    string attChildString = childNodes.ChildNodes[ii].Attributes[jj].Name;
                                    if (attChildString == "Name")
                                    {
                                        attChildName = childNodes.ChildNodes[ii].Attributes[jj].Value.ToString();
                                    }
                                    else if (attChildString == "Value")
                                    {
                                        attChildValue = childNodes.ChildNodes[ii].Attributes[jj].Value.ToString();
                                    }
                                }
                                if (attName == "Lexicon")
                                {
                                    _dictLexiconPrepStepsFilenames[attChildName] = attChildValue;
                                }
                                else
                                {
                                    var tempDict = new Dictionary<string, string>();
                                    if (_dictStepFilenames.ContainsKey(attName))
                                    {
                                        tempDict = _dictStepFilenames[attName];
                                    }
                                    tempDict[attChildName] = attChildValue;
                                    _dictStepFilenames[attName] = tempDict;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: GetPageSectionSteps", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion GetPageSectionSteps

        #region LoadListBox
        private void LoadListBox(string prefix, ListBox lb)
        {
            try
            {
                int secNum = 1;
                while (true)
                {
                    string secRef = prefix + secNum.ToString();
                    string[] sec = GetDefault(secRef);
                    if (sec.Length == 0)
                        break;
                    string secName = "";
                    for (int i = 0; i < sec.Length - 1; i++)
                        secName += (sec[i] + " ");
                    lb.Items.Add(secName);
                    secNum += 1;
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: LoadListBox", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion LoadListBox

        #endregion Load

        #region Events

        #region ddlPagePageSize_SelectedIndexChanged
        private void ddlPagePageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //NonstandardSize();
                //PageSizeChange();
                _commonSettings.PageSizeChange(ddlPagePageSize, ddlPagePaperSize, chkPageCropMark, txtPageWidth, txtPageHeight);
                txtPageWidth.Focus();
                txtPageHeight.Focus();
                ddlPagePageSize.Focus();
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: ddlPagePageSize", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion ddlPagePageSize_SelectedIndexChanged

        #region ddlShowProperty_SelectedIndexChanged
        private void ddlShowProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Enable All
                //if (tabDicSetting.TabPages.Count > 8)
                //    tabDicSetting.TabPages[9].Enabled = true;
                //pnlTextBetweenWords.Enabled = true;
                //lblTextKerning.Enabled = true;
                //ddlTextKerning.Enabled = true;
                //pnlTypefaceNames.Enabled = true;
                //pnlTextLetterRange.Enabled = true;
                //pnlSensesFilter.Visible = true;
                //pnlIndexCreate.Visible = true;
                //pnlMediaImport.Enabled = true;
                //pnlEntriesFilter.Visible = true;
                //pnlMediaFilter.Visible = true;

                if (ddlShowProperty.Text == "OpenOffice Document" || ddlShowProperty.Text == "PDF")
                {
                    if (tabDicSetting.TabPages.Count > 8)
                        tabDicSetting.TabPages[9].Enabled = false;
                    pnlTextBetweenWords.Enabled = false;
                    lblTextKerning.Enabled = false;
                    ddlTextKerning.Enabled = false;
                    pnlTypefaceNames.Enabled = false;
                    pnlTextLetterRange.Enabled = false;
                    pnlSensesFilter.Visible = false;
                    pnlIndexCreate.Visible = false;
                    pnlMediaImport.Enabled = false;
                    pnlEntriesFilter.Visible = false;
                    pnlMediaFilter.Visible = false;

                }
                else if (ddlShowProperty.Text == "InDesign")
                {
                }
                OutputexportType = ddlShowProperty.Text;
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: ddlShowProperty", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion ddlShowProperty_SelectedIndexChanged

        #region ddlSensesNumberSenses_SelectedIndexChanged
        private void ddlSensesNumberSenses_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlSensesNumberSenses.Text != "symbols")
                {
                    txtSensesSymbols.Text = "";
                    lblSennsesSymbols.Enabled = false;
                    txtSensesSymbols.Enabled = false;
                    txtSensesSymbols.BackColor = Color.White;
                    btnSymbolBrowse.Enabled = false;
                    txtSensesPunctuation.Text = ")";
                    txtSensesPunctuation.Enabled = true;
                }
                else
                {
                    txtSensesPunctuation.Text = "";
                    txtSensesPunctuation.Enabled = false;
                    lblSennsesSymbols.Enabled = true;
                    txtSensesSymbols.Enabled = true;
                    txtSensesSymbols.BackColor = Color.LightGray;
                    btnSymbolBrowse.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: ddlSensesNumberSenses", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion ddlSensesNumberSenses_SelectedIndexChanged

        #region ddlHeadingsPageNumberLocation_SelectedIndexChanged
        private void ddlHeadingsPageNumberLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                AssignValue(sender, e);
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: ddlHeadingsPageNumberLocation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion ddlHeadingsPageNumberLocation_SelectedIndexChanged

        #region txtSensesSymbols_TextChanged
        private void txtSensesSymbols_TextChanged(object sender, EventArgs e)
        {
            try
            {
                chkSensesSensesParagraph.Focus();
                txtSensesSymbols.Focus();
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: txtSensesSymbols", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion txtSensesSymbols_TextChanged

        #region ddlPagePageSize_TextChanged
        private void ddlPagePageSize_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _commonSettings.PageSizeChange(ddlPagePageSize, ddlPagePaperSize, chkPageCropMark, txtPageWidth, txtPageHeight);
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: ddlPagePageSize", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion ddlPagePageSize_TextChanged

        #region btnTextFontColor_Click
        private void btnTextFontColor_Click(object sender, EventArgs e)
        {
            try
            {
                ColorDialog clrDlg = new ColorDialog();
                // Allows the user to get help. (The default is false.)
                clrDlg.ShowHelp = true;
                // Update the fullString box color if the user clicks OK 
                if (clrDlg.ShowDialog() == DialogResult.OK)
                {
                    string hexColor = string.Format("0x{0:X8}", clrDlg.Color.ToArgb());
                    lblColorCode.Text = " - " + "#" + hexColor.Substring(hexColor.Length - 6, 6);
                    lblTextFontColor.BackColor = clrDlg.Color;
                    AssignValue(sender, e);
                }
                //dicMap[ctrl.Name] = textValue;
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region btnHeadingsFontName_Click
        private void btnHeadingsFontName_Click(object sender, EventArgs e)
        {
            try
            {
                FontDialog fntDlg = new FontDialog();
                if (txtHeadingsFontName.Text != "")
                {
                    FontStyle fStyle;
                    switch (ddlHeadingsLetterFontStyle.Text)
                    {
                        case "regular":
                            if (ddlHeadingsFontWeight.Text != "normal")
                            {
                                fStyle = FontStyle.Bold;
                            }
                            else
                            {
                                fStyle = FontStyle.Regular;
                            }
                            break;
                        case "italic":
                            if (ddlHeadingsFontWeight.Text == "normal")
                            {
                                fStyle = FontStyle.Italic;
                            }
                            else
                            {
                                fStyle = FontStyle.Bold | FontStyle.Italic;
                            }
                            break;
                        default:
                            fStyle = FontStyle.Regular;
                            break;
                    }
                    float size = float.Parse(txtHeadingsLetterFontSize.Text.Substring(0, txtHeadingsLetterFontSize.Text.Length - 2));
                    Font fnt = new Font(txtHeadingsFontName.Text.ToString(), size, fStyle);
                    fntDlg.Font = fnt;
                }
                fntDlg.ShowDialog();
                txtHeadingsFontName.Text = fntDlg.Font.FontFamily.Name;
                txtHeadingsLetterFontSize.Text = fntDlg.Font.Size.ToString();
                //ddlHeadingsLetterFontStyle.Text = fntDlg.Font.Style.ToString();
                if (fntDlg.Font.Style.ToString() == "Italic")
                {
                    ddlHeadingsFontWeight.Text = "normal";
                    ddlHeadingsLetterFontStyle.Text = fntDlg.Font.Style.ToString();
                }
                else if (fntDlg.Font.Style.ToString() == "Bold")
                {
                    ddlHeadingsLetterFontStyle.Text = "regular";
                }
                else if (fntDlg.Font.Style.ToString() == "Bold, Italic")
                {
                    ddlHeadingsLetterFontStyle.Text = "italic";
                }
                else if (fntDlg.Font.Style.ToString() == "Regular")
                {
                    ddlHeadingsFontWeight.Text = "normal";
                    ddlHeadingsLetterFontStyle.Text = "regular";
                }
                txtHeadingsFontName.Focus();
                ddlHeadingsFontWeight.Focus();
                txtHeadingsLetterFontSize.Focus();
                ddlHeadingsLetterFontStyle.Focus();
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region btnDone_Click
        private void btnDone_Click(object sender, EventArgs e)
        {
            DoClose();
        }
        /// <summary>
        /// To close Dictionary Settings Dialog
        /// </summary>
        public void DoClose()
        {
            try
            {
                //DialogResult dr = new DialogResult();
                if (_confirmSave)
                {
                    DialogResult dr = LocDB.Message("errWantToSave", "Do you want to save the changes?", null, LocDB.MessageTypes.YN,
                    LocDB.MessageDefault.First);
   
                    //LocDB.Message("errWantToSave", "Do you want to save the changes?", null, LocDB.MessageTypes.Error,
                    //LocDB.MessageDefault.First);
                    //dr = MessageBox.Show("Do you want to save the changes?", "Dictionary Settings", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    if (dr == DialogResult.Yes)
                    {
                        btnSave_Click(null, null);
                    }
                }
                else if (_solutionName != "")
                {
                    // Updata de.xml
                    MakeDocumentPreparation();
                }
                else
                {
                    FlexNewFile();
                    SetCSSTemplate();
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region btnSave_Click
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string saveJobName = string.Empty;
                if (_commonSettings.Validations(tabDicSetting, _errProvider))
                {
                    // Generate a new FileName
                    saveJobName = JobName;
                    if (_plugin)
                    {
                        FlexNewFile();
                    }
                    else
                    {
                        JobName = SettingBl.GetNewFileName(ddlCSS.Text, "job", _dicPath, "onsave");
                        MakeDocumentPreparation();
                    }

                    //if (!fileSaved)
                    if (_confirmSave)
                    {
                        SaveFileDialog saveDlg = new SaveFileDialog();
                        saveDlg.DefaultExt = ".css";
                        saveDlg.InitialDirectory = _dicPath;
                        saveDlg.Filter = "Cascading Style Sheet (*.css)|*.css";
                        saveDlg.FileName = JobName;

                        if (saveDlg.ShowDialog() == DialogResult.OK)
                        {
                            JobName = saveDlg.FileName;
                            LastSaved = JobName;
                        }
                        else
                        {
                            JobName = saveJobName;
                            return;
                        }
                        this.Text = Path.GetFileName(JobName);
                        // oo utility
                        if (!_plugin)
                        {
                            AddFileToXML(JobName, chkDefaultCSS.Checked.ToString());
                        }
                        _fileSaved = true;

                        SetCSSTemplate();
                        SetAsDefaultCSS = chkDefaultCSS.Checked;
                        _confirmSave = false;
                    }
                    else
                    {
                        JobName = saveJobName;
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FlexNewFile()
        {
            string processCSS;
            if (ddlCSS.Text != "")
            {
                processCSS = ddlCSS.Text;
            }
            else
            {
                processCSS = _cssPath;
            }
            JobName = SettingBl.GetNewFileName(processCSS, "util", _dicPath, "onplugin");
        }
        #endregion

        #region ddlCSS_SelectedIndexChanged
        private void ddlCSS_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _fileSaved = false;
                //LoadCss();
                _cssTree = _commonSettings.LoadCss(_dicPath, ddlCSS.Text);
                LoadDefault();
                _confirmSave = false;
                JobName = ddlCSS.Text;
                this.Text = Path.GetFileName(JobName);
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region btnNext_Click
        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                _commonSettings.MoveTab(tabDicSetting, 'N');
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region btnPrevious_Click
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                _commonSettings.MoveTab(tabDicSetting, 'P');
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region btnSymbolBrowse_Click
        private void btnSymbolBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                string textBoxFontName = txtSensesSymbols.Text.Trim().Length > 0 ? txtSensesSymbols.Font.Name : "Charis SIL";
                SymbolMap sc = new SymbolMap(this, textBoxFontName);
                sc.ShowDialog();
                if (SymbolValue.Length > 10)
                {
                    LocDB.Message("errGtr10Char", "Symbols should not be greater than 10 characters, Please try again.", null, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                    //MessageBox.Show("Symbols should not be greater than 10 characters, Please try again.", "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSensesSymbols.Text = string.Empty;
                }
                else
                {
                    txtSensesSymbols.Font = new Font(FontName, 12.0F);
                    txtSensesSymbols.Text = SymbolValue;
                    _dicMap["fontName"] = FontName;
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region toolStripButton30_Click
        private void toolStripButton30_Click(object sender, EventArgs e)
        {
            try
            {
                _commonSettings.SwapListItem(lbTextWritingSystem, "Dn");
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region toolStripButton38_Click
        /*
        private void toolStripButton38_Click(object sender, EventArgs e)
        {
            try
            {
                commonSettings.SwapListItem(lbTextWritingSystem, "Dn");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
*/
        #endregion

        #region btnTextTagFontColor_Click
        private void btnTextTagFontColor_Click(object sender, EventArgs e)
        {
            try
            {
                ColorDialog clrDlg = new ColorDialog();
                clrDlg.ShowHelp = true;
                if (clrDlg.ShowDialog() == DialogResult.OK)
                {
                    string HexColor = string.Format("0x{0:X8}", clrDlg.Color.ToArgb());
                    lblTextTagFontColorCode.Text = "#" + HexColor.Substring(HexColor.Length - 6, 6);
                    lblTextTagFontColor.BackColor = clrDlg.Color;
                    AssignValue(sender, e);
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region toolStripButton36_Click
        private void toolStripButton36_Click(object sender, EventArgs e)
        {
            try
            {
                _commonSettings.SwapListItem(lbTextWritingSystem, "Up");
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region toolStripButton29_Click
        private void toolStripButton29_Click(object sender, EventArgs e)
        {
            try
            {
                lbTextWritingSystem.Items.RemoveAt(lbTextWritingSystem.SelectedIndex);
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region lbTextWritingSystem_Click
        private void lbTextWritingSystem_Click(object sender, EventArgs e)
        {
            try
            {
                CallLangProperty();
                _textWritingSystemName = lbTextWritingSystem.Text;
                if (_dictLangTagProperty.ContainsKey(_textWritingSystemName))
                {
                    chkTextIncludeLanguageTags.Checked = true;
                    LoadLanguageProperty(_textWritingSystemName);
                }
                else
                {
                    chkTextIncludeLanguageTags.Checked = false;
                    ddlTextTagFontName.SelectedIndex = -1;
                    txtTextTagFontSize.Text = "";
                    lblTextTagFontColorCode.Text = "";
                    lblTextTagFontColor.BackColor = System.Drawing.Color.Black;
                    if (ddlTextHyphenationLanguage.Items.Count > 0)
                        ddlTextHyphenationLanguage.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region lblTextTagFontColorCode_TextChanged
        private void lblTextTagFontColorCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (lblTextTagFontColorCode.Text != "")
                    lblTextTagFontColor.BackColor = System.Drawing.ColorTranslator.FromHtml(lblTextTagFontColorCode.Text);
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region chkTextIncludeLanguageTags_CheckedChanged
        private void chkTextIncludeLanguageTags_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!chkTextIncludeLanguageTags.Checked)
                {
                    txtTextTagFontSize.Text = "";
                    ddlTextTagFontName.SelectedIndex = -1;
                    txtTextBetweenLettersLetterDesired.Text = "0";
                    lblTextTagFontColor.Text = "";
                    lblTextTagFontColorCode.Text = "";
                    if (lbTextWritingSystem.SelectedIndex >= 0)
                    _dictLangTagProperty.Remove(lbTextWritingSystem.SelectedItem.ToString());
                    if (ddlTextHyphenationLanguage.Items.Count > 0)
                        ddlTextHyphenationLanguage.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region toolStripButton29_Click_1
        private void toolStripButton29_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (lbTextWritingSystem.SelectedIndex >= 0)
                {
                    lbTextWritingSystem.Items.RemoveAt(lbTextWritingSystem.SelectedIndex);
                    lbTextWritingSystem.SelectedIndex = lbTextWritingSystem.Items.Count - 1;
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region toolStripButton37_Click_2
        private void toolStripButton37_Click_2(object sender, EventArgs e)
        {
            try
            {
                _commonSettings.SwapListItem(lbTextWritingSystem, "Up");
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region CssBrowse_Click
        private void CssBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "CSS files|*.css";
            ofd.ShowDialog();
            string fileNameFullPath = ofd.FileName;
            if (fileNameFullPath.Length > 0)
            {
                string fileName = Path.GetFileName(ofd.FileName);
                if (_plugin)  // Flex Plugin
                {

                    string fileNamePath = Common.PathCombine(_dicPath, Path.GetFileName(fileName));
                    try
                    {
                        File.Copy(fileName, fileNamePath, true);
                    }
                    catch (Exception ex)
                    {
                        var msg = new[] { ex.Message };
                        LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                        //MessageBox.Show(ex.Message, "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (ddlCSS.Items.Contains(fileName))
                    {
                        ddlCSS.Items.Remove(fileName);
                    }
                    ddlCSS.Items.Add(fileName);
                    ddlCSS.Text = fileName;
                }
                else  // UI
                {
                    var xd = new XmlDocument();
                    xd.Load(_solutionName);
                    if (AddFileToSolutinExplorer(fileNameFullPath, xd, true))
                    {
                        if (ddlCSS.Items.Contains(fileName))
                        {
                            ddlCSS.Items.Remove(fileName);
                        }
                        ddlCSS.Items.Add(fileName);
                        ddlCSS.Text = fileName;
                        xd.Save(_solutionName);
                    }
                }
            }
        }
        #endregion CssBrowse_Click

        #endregion Events

        #region ReadPreparationTabSteps
        /// <summary>
        /// Reads the SectionTypes.xml and sets the xml type
        /// </summary>
        /// <param name="Section"></param>
        private void ReadPreparationTabSteps(string Section)
        {
            try
            {
                XmlDocument projXml = new XmlDocument();
                string sectionXmlPath = Common.GetPSApplicationPath() + "/SectionTypes.xml";
                if (!File.Exists(sectionXmlPath))
                {
                    var msg = new[] {"SectionTypes.xml"};
                    LocDB.Message("errInstallFile", "Please Install the SectionTypes.xml", msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                    //MessageBox.Show("Please Install the SectionTypes.xml");
                    return;
                }
                projXml.Load(sectionXmlPath);
                XmlElement root = null;
                root = projXml.DocumentElement;
                XmlNode tempNodeSearch = root; // Default to Root Node
                string XPath = "xmlType[Description='" + Section + "']";
                string dtdPath = Common.GetPSApplicationPath() + "/"; // Sil\DictionaryExpress Path
                tempNodeSearch = root.SelectSingleNode(XPath);

                if (tempNodeSearch != null)
                {
                    foreach (XmlNode node in tempNodeSearch)
                    {
                        if (node.Name == "step")
                        {
                            XmlAttribute xa = node.Attributes["type"];
                            if (xa != null)
                            {
                                string stepFileType = xa.Value.ToString();
                                string tempKey = string.Empty;
                                string tempValue = string.Empty;
                                if (stepFileType != null)
                                {
                                    try
                                    {
                                        tempKey = node.FirstChild.InnerText;
                                        tempValue = node.FirstChild.NextSibling.InnerText;
                                        ListViewItem tempItem = new ListViewItem(tempKey);
                                        tempItem.SubItems.Add(tempValue);
                                        lvwPreparationSteps.Items.Add(tempItem);

                                        tempValue = dtdPath + tempValue;
                                        //if (Section == "Lexicon" || Section == "LIFT")
                                        if (Section == "Lexicon" || Section == "LIFT")
                                        {
                                            _dictLexiconPrepStepsFilenames[tempKey] = tempValue;
                                        }
                                        else
                                        {
                                            Dictionary<string, string> tempDict = new Dictionary<string, string>();
                                            tempDict[tempKey] = tempValue;
                                            _dictStepFilenames[Section] = tempDict;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        var msg = new[] { ex.Message };
                                        LocDB.Message("errInstlFile", ex.Message, msg, LocDB.MessageTypes.Error,
                                                      LocDB.MessageDefault.First);
                                        //MessageBox.Show(ex.Message, "Dictionary Setting: ReadPreparationTabSteps", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }

                            if (node.HasChildNodes)
                            {
                                foreach (XmlNode subnode in node)
                                {
                                    if (subnode.Name == "substep")
                                    {
                                        XmlAttribute xmlAttrib = node.Attributes["type"];
                                        if (xmlAttrib != null)
                                        {
                                            if (subnode.HasChildNodes)
                                            {
                                                string tempFileName = subnode.FirstChild.InnerText;
                                                _stepDtdFile.Add(tempFileName);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (node.Name == "FileType")
                        {
                            string fileType = node.InnerText;
                            _fileTypeFilter[Section] = fileType;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("errInstlFile", ex.Message, msg, LocDB.MessageTypes.Error,
                              LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: ReadPreparationTabSteps", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion ReadPreparationTabSteps

        #region makeCss
        /// <summary>
        /// To set the CSS Template
        /// </summary>
        public void SetCSSTemplate()
        {
            try
            {
                //Dictionary<string, string> dicMap = new Dictionary<string, string>();
                _dicMap["link"] = ddlCSS.Text;
                _dicMap["PagePageSize"] = ddlPagePageSize.Text;
                _dicMap["PagePaperSize"] = ddlPagePaperSize.Text;
                _dicMap["pageWidth"] = txtPageWidth.Text;
                _dicMap["pageHeight"] = txtPageHeight.Text;
                _dicMap["PageInside"] = txtPageInside.Text;
                _dicMap["PageOutside"] = txtPageOutside.Text;
                _dicMap["PageTop"] = txtPageTop.Text;
                _dicMap["PageBottom"] = txtPageBottom.Text;
                _dicMap["PageGutterWidth"] = txtPageGutterWidth.Text;
                _dicMap["PageCropMark"] = chkPageCropMark.Checked ? "crop" : "none";
                _dicMap["chkPageVerticalRule"] = chkPageVerticalRule.Checked ? "solid 1pt #aa0000" : "none";
                _dicMap["PageVerticalRule"] = chkPageVerticalRule.Checked ? "True" : "";
                _dicMap["chkIndexesVerticalRuleInGutter"] = chkIndexesVerticalRuleInGutter.Checked ? "solid 1pt #aa0000;" : "none";
                _dicMap["PageColumn"] = ddlPageColumn.Text;
                _dicMap["PageColumnGT1"] = (ddlPageColumn.Text != "1") ? ddlPageColumn.Text : "";
                _dicMap["verbose"] = "yes";
                _dicMap["IncludeLanguageProperty"] = _langTagProperty;
                _dicMap["SensesSensesParagraph"] = chkSensesSensesParagraph.Checked ? "block" : "inline";
                _dicMap["chkHeadingsLetterCentered"] = chkHeadingsLetterCentered.Checked ? "center" : "none";
                _dicMap["chkHeadingsLetterDividerLine"] = chkHeadingsLetterDividerLine.Checked
                                                             ? ".5pt solid #a00"
                                                             : "none";
                
                if (txtSensesPunctuation.Text == "")
                {
                    _dicMap["txtSensesPunctuation"] = "none";
                }
                if (txtSensesSymbols.Text == "")
                {
                    _dicMap["txtSensesSymbols"] = "none";
                    _dicMap["xSenseVisible"] = "visible";
                    _dicMap["xSenseVisibleFont"] = "normal";
                }
                else
                {
                    _dicMap["xSenseVisible"] = "hidden";
                    _dicMap["xSenseVisibleFont"] = "2pt"; //  write font size as 2pt
                }
                if (!chkHeadingsHomonymNumber.Checked)
                {
                    _dicMap["-ps-HeadingsHomonymNumber"] = "none";
                }
                if (_dicMap.ContainsKey("ddlIndexesColumns") && !_dicMap.ContainsKey("txtIndexesGutterWidth"))
                {
                    _dicMap.Add("txtIndexesGutterWidth","12pt");
                }
                if(!_dicMap.ContainsKey("txtIndexesSenseSeperator"))
                {
                    _dicMap["txtIndexesSenseSeperator"] = ", ";
                }
                else
                {
                    _dicMap["txtIndexesSenseSeperator"] = txtIndexesSenseSeperator.Text + " ";
                }
                var sub = new Substitution();
                sub.FileSubstitute(Common.PathCombine(_supportFolder, "dicTemplate.tpl"), _dicMap, JobName);
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("errInstlFile", ex.Message, msg, LocDB.MessageTypes.Error,
                              LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: makeCss", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion makeCss

        #region MakeDocumentPreparation
        /// <summary>
        /// Reads the data from Dictionary and Writes into CSS File
        /// </summary>
        private void MakeDocumentPreparation()
        {
            try
            {
                XmlDocument projXml = new XmlDocument();
                string XPath = "";
                XmlElement root = null;
                XmlNode sectionNode = null;
                XmlNode sectnNode = null;
                XmlNode stepNode = null;
                XmlNode tempNodeSearch = null;
                XmlAttribute Xa;
                bool blankODTExists = false;
                projXml.Load(_solutionName);
                root = projXml.DocumentElement;

                XPath = "DocumentSettings";
                tempNodeSearch = root.SelectSingleNode(XPath);   // Default to Document Settings
                if (tempNodeSearch == null)
                {
                    tempNodeSearch = projXml.CreateNode("element", "DocumentSettings", "");
                    root.AppendChild(tempNodeSearch);
                }
                else
                {
                    tempNodeSearch.RemoveAll();
                }
                tempNodeSearch.RemoveAll();

                string fileName = string.Empty;
                string makeKey = string.Empty;
                sectionNode = projXml.CreateNode("element", "Sections", "");

                for (int i = 0; i < lvwDocumentSection.Items.Count; i++)
                {
                    string key = lvwDocumentSection.Items[i].Text;

                    if (key == "Lexicon")
                    {
                        sectnNode = projXml.CreateNode("element", "Section", "");
                        //attribute
                        Xa = projXml.CreateAttribute("No");
                        Xa.Value = (i + 1).ToString();
                        sectnNode.Attributes.Append(Xa);

                        Xa = projXml.CreateAttribute("Name");
                        Xa.Value = key;
                        sectnNode.Attributes.Append(Xa);

                        Xa = projXml.CreateAttribute("Value");
                        if (_dictSectionFilenames.ContainsKey(key))
                        {
                            fileName = _dictSectionFilenames[key];
                            Xa.Value = Path.GetFileName(fileName);
                        }
                        else
                        {
                            Xa.Value = Path.GetFileName(_xhtml);
                        }
                       
                        sectnNode.Attributes.Append(Xa);

                        sectionNode.AppendChild(sectnNode); // Section to Sections
                        tempNodeSearch.AppendChild(sectionNode); // Sections to Document Settings.
                        // Steps writing
                        int suffixNo = 0;
                        string extension = Path.GetExtension(fileName);

                        if (extension == ".lift" || extension == ".xml")
                        {
                            foreach (KeyValuePair<string, string> mainKey in _dictLexiconPrepStepsFilenames)
                            {
                                stepNode = projXml.CreateNode("element", "Step", "");

                                Xa = projXml.CreateAttribute("No");
                                suffixNo++;
                                Xa.Value = suffixNo.ToString();
                                stepNode.Attributes.Append(Xa);

                                Xa = projXml.CreateAttribute("Name");
                                Xa.Value = mainKey.Key;
                                stepNode.Attributes.Append(Xa);

                                Xa = projXml.CreateAttribute("Value");
                                Xa.Value = Path.GetFileName(mainKey.Value);
                                stepNode.Attributes.Append(Xa);
                                sectnNode.AppendChild(stepNode); //Steps Node to Section Node
                            }
                        }
                        AddFileToSolutinExplorer(fileName, projXml, false);
                        continue;
                    }
                    else if (_dictSectionFilenames.ContainsKey(key))
                    {
                        fileName = _dictSectionFilenames[key];
                        if (fileName != "none")
                        {
                            AddFileToSolutinExplorer(fileName, projXml, false);
                        }

                        if (key == "Blank")
                        {
                            blankODTExists = true;
                        }
                    }
                    //else if (key == "Blank") 
                    //{
                    //    blankODTExists = true;
                    //}
                    else
                    {
                        fileName = "none";
                    }

                    sectnNode = projXml.CreateNode("element", "Section", "");
                    //attribute
                    Xa = projXml.CreateAttribute("No");
                    Xa.Value = (i + 1).ToString();
                    sectnNode.Attributes.Append(Xa);

                    Xa = projXml.CreateAttribute("Name");
                    Xa.Value = key;
                    sectnNode.Attributes.Append(Xa);

                    Xa = projXml.CreateAttribute("Value");
                    Xa.Value = Path.GetFileName(fileName);
                    sectnNode.Attributes.Append(Xa);

                    sectionNode.AppendChild(sectnNode); // Section to Sections
                    tempNodeSearch.AppendChild(sectionNode); // Sections to Document Settings.

                    //// ========================================================= //
                    //// XSLT - Make Steps Addings
                    //// ========================================================= //
                    if (_dictStepFilenames.ContainsKey(key))
                    {
                        Dictionary<string, string> tempDict = new Dictionary<string, string>();
                        tempDict = _dictStepFilenames[key];
                        int suffixNo = 0;

                        foreach (KeyValuePair<string, string> tempString in tempDict)
                        {
                            makeKey = tempString.Key;
                            fileName = tempString.Value;
                            if (fileName != "none")
                            {
                                AddFileToSolutinExplorer(fileName, projXml, false);
                            }
                            stepNode = projXml.CreateNode("element", "Step", "");

                            Xa = projXml.CreateAttribute("No");
                            suffixNo++;
                            Xa.Value = suffixNo.ToString();
                            stepNode.Attributes.Append(Xa);

                            Xa = projXml.CreateAttribute("Name");
                            Xa.Value = makeKey;
                            stepNode.Attributes.Append(Xa);

                            Xa = projXml.CreateAttribute("Value");
                            Xa.Value = Path.GetFileName(fileName);
                            stepNode.Attributes.Append(Xa);
                            sectnNode.AppendChild(stepNode); //Steps Node to Section Node
                        }
                    }
                }
                // Add DTD Files to Solution Explorer
                string dtdPath = Common.GetPSApplicationPath() + "/";
                foreach (string dtdFileName in _stepDtdFile)
                {
                    if (dtdFileName.Length > 0)
                    {
                        AddFileToSolutinExplorer(Common.PathCombine(dtdPath, dtdFileName), projXml, false);
                    }
                }

                if (blankODTExists)
                {
                    AddFileToSolutinExplorer(Common.PathCombine(dtdPath, "Blank.odt"), projXml, false);
                }
                projXml.Save(_solutionName);

            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("errInstlFile", ex.Message, msg, LocDB.MessageTypes.Error,
                              LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: MakeDocumentPreparation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion MakeDocumentPreparation

        #region DicExplorerTree
        #region AddFileToSolutinExplorer
        /// <summary>
        /// Adds the FileName to Solution Explorer 
        /// Based on the prviously opened xmlDocument File.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="projXml"></param>
        /// <param name="overWrite"></param>
        /// <returns>Nothing</returns>
        public bool AddFileToSolutinExplorer(string fileName, XmlDocument projXml, bool overWrite)
        {
            try
            {
                string fn = Path.GetFileName(fileName);
                string fileNamePath = Common.PathCombine(_dicPath, Path.GetFileName(fileName));
                DialogResult confirm = DialogResult.No;
                if (overWrite && File.Exists(fileNamePath))
                {
                    confirm = LocDB.Message("errFileAlreadyExits", "File Already Exists. Do you want to replace it?", null, LocDB.MessageTypes.YN,
                    LocDB.MessageDefault.First);
                    if (confirm == DialogResult.No)
                    {
                        return false;
                    }
                }

                try
                {
                    File.Copy(fileName, fileNamePath, true);
                }
                catch
                {
                    return false;
                }

                if (confirm == DialogResult.Yes)
                {
                    return true;
                }

                XmlElement root = projXml.DocumentElement;
                XmlNode tempNode;
                tempNode = projXml.CreateNode("element", "File", "");

                //attribute
                XmlAttribute Xa = projXml.CreateAttribute("Name");
                Xa.Value = fn;
                tempNode.Attributes.Append(Xa);

                Xa = projXml.CreateAttribute("Type");
                Xa.Value = "File";
                tempNode.Attributes.Append(Xa);

                Xa = projXml.CreateAttribute("Visible");
                Xa.Value = "True";
                tempNode.Attributes.Append(Xa);

                Xa = projXml.CreateAttribute("Default");
                Xa.Value = "False";
                tempNode.Attributes.Append(Xa);
                root.FirstChild.AppendChild(tempNode);
                return true;

                return false;
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("errInstlFile", ex.Message, msg, LocDB.MessageTypes.Error,
                              LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: AddFileToSolutinExplorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        #endregion AddFileToSolutinExplorer

        #region AddFileToXML
        /// <summary>
        /// Add file to Solution Explorer xml file
        /// </summary>
        /// <param name="fileName">Input file Name</param>
        /// <param name="Default">Default setting</param>
        /// <returns>True / False</returns>
        public bool AddFileToXML(string fileName, string Default)
        {
            try
            {
                string fn = Path.GetFileName(fileName);
                string dicSolution = _solutionName;
                if (!File.Exists(dicSolution))
                    return false;
                XmlDocument projXml = new XmlDocument();
                projXml.Load(dicSolution);

                XmlElement root = projXml.DocumentElement;
                // remove all default true;
                if (Default == "True")
                {
                    PopulateTreeViewRemoveDefault(root.FirstChild);
                }

                XmlNode tempNode;
                tempNode = projXml.CreateNode("element", "File", "");

                //attribute
                XmlAttribute Xa = projXml.CreateAttribute("Name");
                Xa.Value = fn;
                tempNode.Attributes.Append(Xa);

                Xa = projXml.CreateAttribute("Type");
                Xa.Value = "File";
                tempNode.Attributes.Append(Xa);

                Xa = projXml.CreateAttribute("Visible");
                Xa.Value = "True";
                tempNode.Attributes.Append(Xa);

                Xa = projXml.CreateAttribute("Default");
                Xa.Value = Default;
                tempNode.Attributes.Append(Xa);
                root.FirstChild.AppendChild(tempNode);
                projXml.Save(dicSolution);
                return true;
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("errInstlFile", ex.Message, msg, LocDB.MessageTypes.Error,
                              LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: AddFileToXML", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }
        #endregion AddFileToXML

        #region PopulateTreeViewRemoveDefault
        private void PopulateTreeViewRemoveDefault(XmlNode Root)
        {
            try
            {
                foreach (XmlNode tempNode in Root)
                {
                    XmlAttribute file = tempNode.Attributes["Default"];
                    if (file != null)
                    {
                        if (file.Value == "True")
                        {
                            file.Value = "False";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("errInstlFile", ex.Message, msg, LocDB.MessageTypes.Error,
                              LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: PopulateTreeViewRemoveDefault", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion PopulateTreeViewRemoveDefault
        #endregion DicExplorerTree

        #region ClearControls
        /// <summary>
        /// To clear the values from controls
        /// </summary>
        public void ClearControls()
        {
            try
            {
                for (int tabindex = 0; tabindex < tabDicSetting.TabPages.Count; tabindex++)
                {
                    foreach (Control myControl in tabDicSetting.TabPages[tabindex].Controls)
                    {
                        //if (myControl.Name == "txtHeadingsString") return;  // Do not Clear
                        //dicMap[myControl.Name] = string.Empty;
                        if (myControl is TextBox)
                        {
                            myControl.Text = string.Empty;
                        }
                        else if (myControl is ComboBox)
                        {
                            ComboBox cmb = myControl as ComboBox;
                            cmb.Items.Clear();
                        }
                        else if (myControl is ListBox)
                        {
                            ListBox lst = myControl as ListBox;
                            lst.Items.Clear();
                        }
                        else if (myControl is CheckBox)
                        {
                            CheckBox lst = myControl as CheckBox;
                            lst.Checked = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("errInstlFile", ex.Message, msg, LocDB.MessageTypes.Error,
                              LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: ClearControls", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion ClearControls

        #region AssignValueUnit
        public void AssignValueUnit(object sender, EventArgs e)
        {
            try
            {
                _confirmSave = true;
                //to Validate Units
                _units.Add("pt");
                _units.Add("pc");
                _units.Add("cm");
                _units.Add("in");

                string textValue;
                string errorMessage = string.Empty;
                Control ctrl = ((Control)sender);
                textValue = _commonSettings.ConcateUnit(ctrl);
                ctrl.Text = textValue;
                // Page Tab
                if (ctrl.Name == "txtPageWidth")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, "2.91in", "22in");
                }
                else if (ctrl.Name == "txtPageHeight")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, "2.91in", "22in");
                }
                else if (ctrl.Name == "txtPageInside")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, ".25in", "1.575in");
                }
                else if (ctrl.Name == "txtPageOutside")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, ".25in", "1.575in");
                }
                else if (ctrl.Name == "txtPageTop")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, ".25in", "1.575in");
                }
                else if (ctrl.Name == "txtPageBottom")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, ".25in", "1.575in");
                }
                else if (ctrl.Name == "txtPageGutterWidth")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, "6pt", "1in");
                }
                else if (ctrl.Name == "txtHeadingsLetterFontSize")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, "8pt", "14pt");
                    _commonSettings.CalculateFontSizeDependent(ctrl, txtHeadingsLetterLineSpacing, _dicMap, 1.10F);
                    _commonSettings.CalculateFontSizeDependent(ctrl, txtHeadingsLetterSpaceAfter, _dicMap, 1.5F);
                    _commonSettings.CalculateFontSizeDependent(ctrl, txtHeadingsLetterSpaceBefore, _dicMap, 1.5F);
                }
                else if (ctrl.Name == "txtHeadingsLetterLineSpacing")
                {
                    errorMessage = _commonSettings.ValidateLineSpacing(ctrl, txtHeadingsLetterFontSize);
                }
                else if (ctrl.Name == "txtHeadingsLetterSpaceAfter")
                {
                    errorMessage = _commonSettings.ValidateSpaceSelector(txtHeadingsLetterFontSize, ctrl.Text, "After");
                }
                else if (ctrl.Name == "txtHeadingsLetterSpaceBefore")
                {
                    errorMessage = _commonSettings.ValidateSpaceSelector(txtHeadingsLetterFontSize, ctrl.Text, "Before");
                }
                else if (ctrl.Name == "txtHeadingsRunningFontSize")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, "8pt", "14pt");
                }
                else if (ctrl.Name == "txtEntriesHangingIndent")
                {
                    errorMessage = _commonSettings.RangeValidate(
                        ctrl.Text,
                        "0in",
                        "1in");
                }
                else if (ctrl.Name == "txtEntriesFontSize")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, "8pt", "14pt");
                    _commonSettings.CalculateFontSizeDependent(ctrl, txtEntriesLineSpacing, _dicMap, 1.10F);
                }
                else if (ctrl.Name == "txtEntriesLineSpacing")
                {
                    errorMessage = _commonSettings.ValidateLineSpacing(ctrl, txtEntriesFontSize);
                }
                else if (ctrl.Name == "txtEntriesSpaceBefore")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, "0pt", "12pt");
                    //m_errorMessage = commonSettings.ValidateSpaceSelector(txtEntriesFontSize, ctrl.Text, "Before");
                }
                else if (ctrl.Name == "txtEntriesSpaceAfter")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, "0pt", "12pt");
                    //m_errorMessage = commonSettings.ValidateSpaceSelector(txtEntriesFontSize, ctrl.Text, "After");
                }
                else if (ctrl.Name == "txtIndexesGutterWidth")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, "6pt", "1in");
                }
                else if (ctrl.Name == "txtTextTagFontSize")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, "6pt", "14pt");
                }
                else if (ctrl.Name == "txtTextFontSize")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, "8pt", "14pt");
                }
                else if (ctrl.Name == "txtTextBetweenLettersLetterDesired")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, "0pt", "6pt");
                }
                else if (ctrl.Name == "txtMediaDataFontSize")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, "6pt", "14pt");
                }

                if (errorMessage.Trim().Length != 0)
                {
                    _errProvider.SetError(ctrl, errorMessage);
                }
                else
                {
                    _errProvider.SetError(ctrl, "");
                }
                _dicMap[ctrl.Name] = textValue;
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("errInstlFile", ex.Message, msg, LocDB.MessageTypes.Error,
                              LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: AssignValueUnit", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion AssignValueUnit

        #region AssignValuePageUnit
        public bool AssignValuePageUnit1(object sender, EventArgs e)
        {
            try
            {
                _confirmSave = true;
                //to Validate Units
                _units.Add("pt");
                _units.Add("pc");
                _units.Add("cm");
                _units.Add("in");

                string textValue;
                string errorMessage = string.Empty;
                Control ctrl = ((Control)sender);
                textValue = _commonSettings.ConcateUnit(ctrl);
                ctrl.Text = textValue;
                // Page Tab
                if (ctrl.Name == "txtPageInside")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, ".25in", "1.575in");
                }
                else if (ctrl.Name == "txtPageOutside")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, ".25in", "1.575in");
                }
                else if (ctrl.Name == "txtPageTop")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, ".25in", "1.575in");
                }
                else if (ctrl.Name == "txtPageBottom")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, ".25in", "1.575in");
                }
                else if (ctrl.Name == "txtPageGutterWidth")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, "6pt", "1in");
                }

                if (errorMessage.Trim().Length != 0)
                {
                    _errProvider.SetError(ctrl, errorMessage);
                    return true;
                }
                _errProvider.SetError(ctrl, "");
                return false;
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("errInstlFile", ex.Message, msg, LocDB.MessageTypes.Error,
                              LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: AssignValueUnit", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }
        #endregion AssignValuePageUnit

        #region AssignValue
        private void AssignValue(object sender, EventArgs e)
        {
            try
            {
                _confirmSave = true;
                string textValue = "";
                Control ctrl = ((Control)sender);
                if (ctrl is TextBox)
                {
                    //TextBox txt = ((TextBox)sender);
                    textValue = ctrl.Text;
                    if (ctrl.Name == "txtSensesSymbols")
                    {
                        if (textValue.Length > 1)
                        {
                            char[] symbol = textValue.ToCharArray();
                            textValue = string.Empty;
                            foreach (char symb in symbol)
                            {
                                //m_textValue = textValue + " " + Common.ConvertStringToUnicode(symb.ToString());
                                textValue = textValue + Common.ConvertStringToUnicode(symb.ToString());
                            }
                        }
                        else
                        {
                            textValue = Common.ConvertStringToUnicode(textValue);
                        }
                    }
                }
                else if (ctrl is CheckBox)
                {
                    CheckBox chk = ((CheckBox)sender);
                    if (chk.Checked)
                    {
                        textValue = chk.Checked.ToString();
                    }
                    else if (ctrl.Name == "chkHeadingsHomonymNumber" ||
                        ctrl.Name == "chkHeadingsGuideWords" || ctrl.Name == "chkIndexesVerticalRuleInGutter")
                    {
                        textValue = "none";
                    }
                    //else if (ctrl.Name == "chkPageCropMark" || ctrl.Name == "chkPageVerticalRule" || ctrl.Name == "chkHeadingsLetterCentered" || ctrl.Name == "chkHeadingsLetterDividerLine" || ctrl.Name == "chkHeadingsHomonymNumber" || ctrl.Name == "chkHeadingsGuideWords" || ctrl.Name == "chkEntriesUseGloss" || ctrl.Name == "chkSensesSensesParagraph" || ctrl.Name == "chkTextIncludeLanguageTags" || ctrl.Name == "txtTextJustifiedParagraphs" || ctrl.Name == "chkMediaImport" || ctrl.Name == "chkIndexesVerticalRuleInGutter")
                    //{
                    //    textValue = "none";
                    //}
                }
                //else if (ctrl is Button)
                //{
                    //if (ctrl.Name == "btnTextFontColor") 
                    //{
                    //    textValue = lbl.BackColor.ToArgb().ToString();
                    //}
                //}
                else if (ctrl is ComboBox)
                {
                    if (ctrl.Text.ToLower() == "top center")
                    {
                        textValue = "@top";
                        _pageHeaderFooterPos = "top-center";
                    }
                    else if (ctrl.Text.ToLower() == "bottom center")
                    {
                        textValue = "@bottom";
                        _pageHeaderFooterPos = "bottom-center";
                    }
                    else
                    {
                        textValue = ctrl.Text;
                    }
                }
                _dicMap[ctrl.Name] = textValue;
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("errInstlFile", ex.Message, msg, LocDB.MessageTypes.Error,
                              LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: AssignValue", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion AssignValue

        #region CallLangProperty
        private void CallLangProperty()
        {
            try
            {
                if (_textWritingSystemName.Trim().Length != 0 && ddlTextTagFontName.Text.Trim().Length != 0)
                {
                    _dictLangTagProperty[_textWritingSystemName] = "\"" + ddlTextTagFontName.SelectedItem + "\"" + "," + txtTextTagFontSize.Text + "," + lblTextTagFontColorCode.Text + "," + txtTextBetweenLettersLetterDesired.Text + "," + "\"" + ddlTextHyphenationLanguage.SelectedItem + "\"";
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("errInstlFile", ex.Message, msg, LocDB.MessageTypes.Error,
                              LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: CallLangProperty", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion CallLangProperty

        #region SaveLanguageProperty
        private void SaveLanguageProperty()
        {
            try
            {
                _langTagProperty = "";
                foreach (KeyValuePair<string, string> kvp in _dictLangTagProperty)
                {
                    _langTagProperty = _langTagProperty + "*[lang=\'" + kvp.Key + "\']{ "; // +keyValue.Value + "}" + "\r\n";
                    string[] splitValue = kvp.Value.Split(',');
                    string[] property = new string[5] { "font-family:", "font-size:", "color:", "letter-spacing:", "prince-hyphenate-patterns:" };
                    for (int i = 0; i < splitValue.Length; i++)
                    {
                        if (splitValue[i] != "")
                        {
                            _langTagProperty = _langTagProperty + "\r\n" + property[i] + splitValue[i] + ";";
                        }
                    }
                    _langTagProperty = _langTagProperty + "\r\n" + "}" + "\r\n";
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("errInstlFile", ex.Message, msg, LocDB.MessageTypes.Error,
                              LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: SaveLanguageProperty", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion SaveLanguageProperty

        #region LoadLanguageProperty
        private void LoadLanguageProperty(string selItem)
        {
            try
            {
                if (_dictLangTagProperty.Count > 0 && _dictLangTagProperty.ContainsKey(selItem))
                {
                    string[] splitValue = _dictLangTagProperty[selItem].Split(',');
                    if (splitValue[0] == "")
                    {
                        ddlTextTagFontName.SelectedIndex = -1;
                    }
                    else
                    {
                        ddlTextTagFontName.Text = splitValue[0].Replace("\"", "").ToString();
                    }
                    txtTextTagFontSize.Text = splitValue[1];
                    lblTextTagFontColorCode.Text = splitValue[2];
                    txtTextBetweenLettersLetterDesired.Text = splitValue[3];
                    if (splitValue[4] == "")
                    {
                        ddlTextHyphenationLanguage.SelectedIndex = -1;
                    }
                    else
                    {
                        ddlTextHyphenationLanguage.Text = splitValue[4].Replace("\"", "").ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("errInstlFile", ex.Message, msg, LocDB.MessageTypes.Error,
                              LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: LoadLanguageProperty", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion LoadLanguageProperty

        #region lbTextWritingSystem_Validating
        private void lbTextWritingSystem_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                _textWritingSystemName = lbTextWritingSystem.Text;
                CallLangProperty();
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("errInstlFile", ex.Message, msg, LocDB.MessageTypes.Error,
                              LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: lbTextWritingSystem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion lbTextWritingSystem_Validating

        #region tabText_Leave
        private void tabText_Leave(object sender, EventArgs e)
        {
            try
            {
                _textWritingSystemName = lbTextWritingSystem.Text;
                CallLangProperty();
                if (_dictLangTagProperty.ContainsKey(_textWritingSystemName))
                {
                    SaveLanguageProperty();
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("errInstlFile", ex.Message, msg, LocDB.MessageTypes.Error,
                              LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting: tabText_Leave", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion tabText_Leave

        #region Documentation Tab
        #region Documentation Events
        #region tsDocumentSectionsDelete_Click
        private void tsDocumentSectionsDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvwDocumentSection.SelectedItems.Count < 0 || lvwDocumentSection.Items.Count == 0)
                {
                    return;
                }
                string selectedItem = string.Empty;
                int index = 0;
                foreach (ListViewItem lvwSelectedItem in lvwDocumentSection.SelectedItems)
                {
                    selectedItem = lvwSelectedItem.Text;
                    index = lvwSelectedItem.Index;
                    if (selectedItem == "Lexicon")
                    {
                        var msg = new[] { "Lexicon Section" };
                        LocDB.Message("errDelFile", "You Cannot Delete the Lexicon Section", msg, LocDB.MessageTypes.Error,
                                      LocDB.MessageDefault.First);
                        //MessageBox.Show("You Cannot Delete the Lexicon Section", "Dictionary Express");
                        return;
                    }
                }
                lvwDocumentSection.Items.RemoveAt(index);

                lvwPreparationSteps.Items.Clear();

                if (_dictSectionFilenames.ContainsKey(selectedItem))
                {
                    _dictSectionFilenames.Remove(selectedItem);
                    if (_dictStepFilenames.ContainsKey(selectedItem))
                    {
                        _dictStepFilenames.Remove(selectedItem);
                    }
                }
                else if (_dictLexiconPrepStepsFilenames.ContainsKey(selectedItem))
                {
                    _dictLexiconPrepStepsFilenames.Remove(selectedItem);
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                              LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion tsDocumentSectionsDelete_Click

        #region tsDocumentSectionsAdd_Click
        private void tsDocumentSectionsAdd_Click(object sender, EventArgs e)
        {
            try
            {
                AddEditSectionStep(true, true, lvwDocumentSection);
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                              LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion tsDocumentSectionsAdd_Click

        #region tsDocumentSectionsEdit_Click
        private void tsDocumentSectionsEdit_Click(object sender, EventArgs e)
        {
            try
            {
                AddEditSectionStep(true, false, lvwDocumentSection);
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                              LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion tsDocumentSectionsEdit_Click

        #region tsDocumentSectionsUp_Click
        private void tsDocumentSectionsUp_Click(object sender, EventArgs e)
        {
            try
            {
                _commonSettings.SwapListView(lvwDocumentSection, "UP");
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                              LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion tsDocumentSectionsUp_Click

        #region tsDocumentSectionsDown_Click
        private void tsDocumentSectionsDown_Click(object sender, EventArgs e)
        {
            try
            {
                _commonSettings.SwapListView(lvwDocumentSection, "DOWN");
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                              LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion tsDocumentSectionsDown_Click

        #region tsDocumentStepsAdd_Click
        private void tsDocumentStepsAdd_Click(object sender, EventArgs e)
        {
            try
            {
                AddEditSectionStep(false, true, lvwPreparationSteps);
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                              LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion tsDocumentStepsAdd_Click

        #region tsDocumentStepsEdit_Click
        private void tsDocumentStepsEdit_Click(object sender, EventArgs e)
        {
            try
            {
                AddEditSectionStep(false, false, lvwPreparationSteps);
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                              LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion tsDocumentStepsEdit_Click

        #region tsDocumentStepsUp_Click
        private void tsDocumentStepsUp_Click(object sender, EventArgs e)
        {
            try
            {
                _commonSettings.SwapListView(lvwPreparationSteps, "UP");
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                              LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion tsDocumentStepsUp_Click

        #region tsDocumentStepsDown_Click
        private void tsDocumentStepsDown_Click(object sender, EventArgs e)
        {
            try
            {
                _commonSettings.SwapListView(lvwPreparationSteps, "DOWN");
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                              LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion tsDocumentStepsDown_Click

        #region tsDocumentStepsDelete_Click
        private void tsDocumentStepsDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvwPreparationSteps.SelectedItems.Count < 0 || lvwPreparationSteps.Items.Count == 0)
                {
                    return;
                }
                string selectedItem = string.Empty;
                int index = 0;
                foreach (ListViewItem lvwSelectedItem in lvwPreparationSteps.SelectedItems)
                {
                    selectedItem = lvwSelectedItem.Text;
                    index = lvwSelectedItem.Index;
                }
                lvwPreparationSteps.Items.RemoveAt(index);

                string selectedItemLexicon = string.Empty;
                foreach (ListViewItem lvwSelectedItem in lvwDocumentSection.SelectedItems)
                {
                    selectedItemLexicon = lvwSelectedItem.Text;
                }

                if (selectedItemLexicon == "Lexicon")
                {
                    if (_dictLexiconPrepStepsFilenames.ContainsKey(selectedItem))
                    {
                        _dictLexiconPrepStepsFilenames.Remove(selectedItem);
                    }
                }
                else if (_dictStepFilenames.ContainsKey(selectedItem))
                {
                    _dictStepFilenames.Remove(selectedItem);
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                              LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion tsDocumentStepsDelete_Click

        #region lvwDocumentSection_MouseClick
        private void lvwDocumentSection_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (lvwDocumentSection.SelectedItems.Count <= 0)
                {
                    return;
                }
                string selectedItem = string.Empty;
                int index = 0;
                foreach (ListViewItem lvwSelectedItem in lvwDocumentSection.SelectedItems)
                {
                    selectedItem = lvwSelectedItem.Text.Trim();
                    index = lvwSelectedItem.Index;
                }

                lvwPreparationSteps.Items.Clear();
                if (selectedItem == "Lexicon")
                {
                    ReadPreparationTabSteps("LIFT");
                }
                else
                {
                    ReadPreparationTabSteps(selectedItem);
                }
                if (lvwPreparationSteps.Items.Count == 0)
                {
                    if (_dictStepFilenames.ContainsKey(selectedItem))
                    {
                        Dictionary<string, string> tempDict = new Dictionary<string, string>();
                        tempDict = _dictStepFilenames[selectedItem];
                        foreach (KeyValuePair<string, string> tempString in tempDict)
                        {
                            ListViewItem tempListView = new ListViewItem();
                            tempListView.Text = tempString.Key;
                            tempListView.SubItems.Add(tempString.Value);
                            lvwPreparationSteps.Items.Add(tempListView);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                              LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion lvwDocumentSection_MouseClick
        #endregion Documentation Events

        #region SwapListView
        /// <summary>
        /// To swap the listItems based on Up and Down buttons
        /// </summary>
        /// <param name="lstBox"></param>
        /// <param name="dir"></param>
        private void SwapListView(ListView lstBox, string dir)
        {
            try
            {
                ListViewItem selectedItem = null;
                int index = -1;
                foreach (ListViewItem lvwSelectedItem in lstBox.SelectedItems)
                {
                    selectedItem = lvwSelectedItem;
                    index = lvwSelectedItem.Index;
                }

                ListViewItem swap = selectedItem;
                if (index != -1)
                {
                    if (dir == "UP")
                    {
                        if (index != 0)
                        {
                            lstBox.Items.RemoveAt(index);
                            lstBox.Items.Insert(index - 1, swap);
                        }
                    }
                    else if (dir == "DOWN")
                    {
                        if (lstBox.Items.Count - 1 != index)
                        {
                            lstBox.Items.RemoveAt(index);
                            lstBox.Items.Insert(index + 1, swap);
                        }
                    }
                    //LstBox.SelectedItem = Swap;
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
        #endregion SwapListView

        #region AddEditSectionStep
        /// <summary>
        /// Add/Edit the Section / Steps ListView Items
        /// </summary>
        /// <param name="fromSection">True - from Section </param>
        /// <param name="add">True - for Addition </param>
        /// <param name="listView">Listview Section/Step</param>
        private void AddEditSectionStep(bool fromSection, bool add, ListView listView)
        {
            try
            {
                string filter = "*";
                string returnName = string.Empty;
                string returnFileName = string.Empty;
                string selectedItem = string.Empty;
                string selectedSubItem = string.Empty;

                int index = 0;
                if (!add) // Edit
                {
                    if (listView.SelectedItems.Count == 0) // Empty - return
                    {
                        LocDB.Message("errSelectandEdit", "Please Select and then Edit", null, LocDB.MessageTypes.Error,
                                      LocDB.MessageDefault.First);
                        //MessageBox.Show("Please Select and then Edit ");
                        return;
                    }
                    foreach (ListViewItem lvwSelectedItem in listView.SelectedItems)
                    {
                        selectedItem = lvwSelectedItem.Text;
                        index = lvwSelectedItem.Index;
                        //if (selectedItem == "Lexicon" || selectedItem == "Blank")
                        if (selectedItem == "Blank")
                        {
                            var msg = new[] { "Section" };
                            LocDB.Message("errCannotEdit", "You Cannot edit this Section", msg, LocDB.MessageTypes.Error,
                            LocDB.MessageDefault.First);
                            //MessageBox.Show("You Cannot edit this Section", "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        if (lvwSelectedItem.SubItems.Count > 1)
                        {
                            selectedSubItem = lvwSelectedItem.SubItems[1].Text;
                        }
                    }
                }

                PopupForm pf = new PopupForm(selectedItem, selectedSubItem, add, filter, fromSection);
                if (pf.ShowDialog() == DialogResult.OK)
                {
                    if (pf.FileName == string.Empty)
                    {
                        if (add)
                        {
                            returnName = "Blank";
                            AddEditListViewItems(returnName, fromSection, index, "none", add);
                        }
                    }
                    else
                    {
                        returnName = pf.SectionName;
                        if (returnName != null)
                        {
                            returnFileName = pf.FileName;
                            AddEditListViewItems(returnName, fromSection, index, returnFileName, add);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion AddEditSectionStep

        #region AddEditListViewItems
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="fromSection"></param>
        /// <param name="keyIndex"></param>
        /// <param name="keyValue"></param>
        /// <param name="add"></param>
        private void AddEditListViewItems(string keyName, bool fromSection, int keyIndex, string keyValue, bool add)
        {
            try
            {
                if (add)
                {
                    if (fromSection)
                    {
                        _dictSectionFilenames[keyName] = keyValue;

                        ListViewItem tempListView = new ListViewItem();
                        tempListView.Text = keyName;
                        tempListView.SubItems.Add(keyValue);
                        lvwDocumentSection.Items.Add(tempListView);
                    }
                    else
                    {
                        if (lvwDocumentSection.Text == "Lexicon")
                        {
                            _dictLexiconPrepStepsFilenames[keyName] = keyValue;
                        }
                        else
                        {
                            string selectedItem = string.Empty;
                            foreach (ListViewItem lvwSelectedItem in lvwDocumentSection.SelectedItems)
                            {
                                selectedItem = lvwSelectedItem.Text;
                            }

                            Dictionary<string, string> tempDict = new Dictionary<string, string>();
                            if (_dictStepFilenames.ContainsKey(selectedItem))
                            {
                                tempDict = _dictStepFilenames[selectedItem];
                            }
                            tempDict[keyName] = keyValue;
                            _dictStepFilenames[selectedItem] = tempDict;
                            ListViewItem tempListView = new ListViewItem();
                            tempListView.Text = keyName;
                            tempListView.SubItems.Add(keyValue);
                            lvwPreparationSteps.Items.Add(tempListView);
                        }
                    }
                }
                else // Edit
                {
                    if (fromSection)
                    {
                        _dictSectionFilenames[keyName] = keyValue;

                        ListViewItem tempListView = new ListViewItem();
                        tempListView.Text = keyName;
                        tempListView.SubItems.Add(keyValue);
                        lvwDocumentSection.Items.RemoveAt(keyIndex);
                        lvwDocumentSection.Items.Insert(keyIndex, tempListView);
                    }
                    else
                    {
                        if (lvwDocumentSection.Text == "Lexicon")
                        {
                            _dictLexiconPrepStepsFilenames[keyName] = keyValue;
                        }
                        else
                        {
                            Dictionary<string, string> tempDict = new Dictionary<string, string>();
                            tempDict[keyName] = keyValue;
                            _dictStepFilenames[lvwDocumentSection.Text] = tempDict;

                            ListViewItem tempListView = new ListViewItem();
                            tempListView.Text = keyName;
                            tempListView.SubItems.Add(keyValue);
                            lvwPreparationSteps.Items.RemoveAt(keyIndex);
                            lvwPreparationSteps.Items.Insert(keyIndex, tempListView);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
                //MessageBox.Show(ex.Message, "Dictionary Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion AddEditListViewItems

        private void txtTextTagFontSize_TextChanged(object sender, EventArgs e)
        {

        }
        #endregion Documentation Tab

        private void ddlPageColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            _commonSettings.DisableColumnControls(ddlPageColumn.Text, txtPageGutterWidth, chkPageVerticalRule);
        }

        private void ddlIndexesColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            _commonSettings.DisableColumnControls(ddlIndexesColumns.Text, txtIndexesGutterWidth, chkIndexesVerticalRuleInGutter);
        }

        private void tabDicSetting_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabDicSetting.SelectedIndex == 0)
            {
                Common.HelpProv.SetHelpNavigator(this, HelpNavigator.Topic);
                Common.HelpProv.SetHelpKeyword(this, "DictionaryDocumentTab.htm");
            }
            else if (tabDicSetting.SelectedIndex == 1)
            {
                Common.HelpProv.SetHelpNavigator(this, HelpNavigator.Topic);
                Common.HelpProv.SetHelpKeyword(this, "PageTab.htm");
            }
            else if (tabDicSetting.SelectedIndex == 2)
            {
                Common.HelpProv.SetHelpNavigator(this, HelpNavigator.Topic);
                Common.HelpProv.SetHelpKeyword(this, "DictionaryHeadingsTab.htm");
            }
            else if (tabDicSetting.SelectedIndex == 3)
            {
                Common.HelpProv.SetHelpNavigator(this, HelpNavigator.Topic);
                Common.HelpProv.SetHelpKeyword(this, "EntriesTab.htm");
            }
            else if (tabDicSetting.SelectedIndex == 4)
            {
                Common.HelpProv.SetHelpNavigator(this, HelpNavigator.Topic);
                Common.HelpProv.SetHelpKeyword(this, "SensesTab.htm");
            }
            //else if (tabDicSetting.SelectedIndex == 5)
            //{
                //Common.help.SetHelpNavigator(this, HelpNavigator.Topic);
                //Common.help.SetHelpKeyword(this, "PageTab.htm");
            //}
            else if (tabDicSetting.SelectedIndex == 5)
            {
                Common.HelpProv.SetHelpNavigator(this, HelpNavigator.Topic);
                Common.HelpProv.SetHelpKeyword(this, "DictionaryTextTab.htm");
            }
            else if (tabDicSetting.SelectedIndex == 6)
            {
                Common.HelpProv.SetHelpNavigator(this, HelpNavigator.Topic);
                Common.HelpProv.SetHelpKeyword(this, "MediaTab.htm");
            }
            else if (tabDicSetting.SelectedIndex == 7)
            {
                Common.HelpProv.SetHelpNavigator(this, HelpNavigator.Topic);
                Common.HelpProv.SetHelpKeyword(this, "IndexesTab.htm");
            }
            //else if (tabDicSetting.SelectedIndex == 9)
            //{
                //Common.help.SetHelpNavigator(this, HelpNavigator.Topic);
                //Common.help.SetHelpKeyword(this, "PageTab.htm");
            //}
            
        }

        private void DictionarySetting_DoubleClick(object sender, EventArgs e)
        {
#if DEBUG
            var dlg = new Localizer(LocDB.DB);
            dlg.ShowDialog();
#endif
        }

        private void DictionarySetting_Activated(object sender, EventArgs e)
        {
            Common.SetFont(this);
        }

        private void tbnFieldsStyles_Click(object sender, EventArgs e)
        {

        }

    }
}
