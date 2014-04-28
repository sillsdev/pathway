// --------------------------------------------------------------------------------------------
// <copyright file="ScriptureSetting.cs" from='2009' to='2014' company='SIL International'>
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
// Setting for Scripture
// </remarks>
// --------------------------------------------------------------------------------------------

#region Using
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Collections;
using System.Text;
using JWTools;
using SIL.Tool;
using SIL.Tool.Localization;

#endregion

namespace SIL.PublishingSolution
{
    /// <summary>
    /// Scripture settings
    /// </summary>
    public partial class ScriptureSetting : Form
    {
        #region Private Variables
        readonly string _dicPath;
        readonly string _cssPath;
        readonly string _xhtml;
        string[] _xpath;
        readonly char[] _delim = { '/' };
        readonly string _solutionName;
        readonly string _supportFolder;
        bool _fileSaved;
        string _pageHeaderFooterPos = string.Empty;
        TreeNode _cssTree;
        readonly SettingBl _commonSettings;
        readonly ErrorProvider _errProvider;
        readonly Dictionary<string, string> _dicMap;
        readonly Dictionary<string, string> _dictSectionFilenames;
        readonly Dictionary<string, Dictionary<string, string>> _dictStepFilenames;
        readonly Dictionary<string, string> _dictMainPrepStepsFilenames;
        readonly Dictionary<string, string> _fileTypeFilter;
        readonly ArrayList _stepDtdFile;
        bool _confirmSave;
        readonly bool _plugin;
        readonly ArrayList _singlePage = new ArrayList();
        readonly ArrayList _mirrorPage = new ArrayList();

        readonly string[] _regNames = {"top-left", "top-center", "top-right", "bottom-left", "bottom-center",
                             "bottom-right"};
        #endregion

        #region public variable
        public string JobName;
        public string LastSaved = string.Empty;
        public bool SetAsDefaultCSS { get; set; }
        public bool IsLoaded;
        #endregion

        #region Constructor

        public ScriptureSetting()
        {
            InitializeComponent();
        }
        /// <summary>
        /// To load the default values from CSS
        /// </summary>
        /// <param name="dicPath">de Path</param>
        /// <param name="cssPath">CSS Path</param>
        /// <param name="fromPlugin">Is From PlugIn</param>
        /// <param name="xhtmlName">XHTML Path</param>
        /// <param name="solutionName">Solution Name</param>
        public ScriptureSetting(string dicPath, string cssPath, bool fromPlugin, string xhtmlName, string solutionName)
        {
            _confirmSave = false;
            _stepDtdFile = new ArrayList();
            _fileTypeFilter = new Dictionary<string, string>();
            _dictMainPrepStepsFilenames = new Dictionary<string, string>();
            _dictStepFilenames = new Dictionary<string, Dictionary<string, string>>();
            _dictSectionFilenames = new Dictionary<string, string>();
            _dicMap = new Dictionary<string, string>();
            _errProvider = new ErrorProvider();
            _commonSettings = new SettingBl();
            _fileSaved = false;
            try
            {
                InitializeComponent();
                _dicPath = dicPath;
                _cssPath = cssPath;
                _xhtml = xhtmlName;
                _plugin = fromPlugin;
                _solutionName = solutionName;
				_supportFolder = Common.FromRegistry("");
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }

        #endregion

        #region ScriptureSetting_Load
        private void ScriptureSetting_Load(object sender, EventArgs e)
        {
            ShowHelp.ShowHelpTopic(this, "DocumentTab.htm", Common.IsUnixOS(), false);
            try
            {
                // Localize Controls.
                LocDB.Localize(this, null);   //Form Controls
                ReferenceFormats();
                DoLoad();
                IsLoaded = true;
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }
        #endregion

        #region DoLoad
        /// <summary>
        /// To load defalult values
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
                }
                else
                {
                    ddlShowProperty.Items.Add("ALL");
                }
                // Source CSS
                _commonSettings.LoadSourceCSS(_dicPath, _cssPath, ddlCSS, "scripture", _plugin);
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
                _commonSettings.RemoveTab(7, tabScriptureSettings); // Advanced
                if (_plugin)
                {
                    _commonSettings.RemoveTab(0, tabScriptureSettings);  // Document
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }
        #endregion

        #region DoLoad
        /// <summary>
        /// To load defaults
        /// </summary>
        public void ReferenceFormats()
        {
            try
            {
                _singlePage.Add("Genesis 1:1-15");
                _singlePage.Add("Genesis 1:1 Genesis 1:15");
                _singlePage.Add("Genesis 1:1 1:15");
                _singlePage.Add("Genesis 1, 2");
                _singlePage.Add("Genesis 1");

                _mirrorPage.Add("Genesis 1:1 Genesis 1:15");
                _mirrorPage.Add("Genesis 1");
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }
        #endregion

        #region LoadDefault
        private void LoadDefault()
        {
            try
            {
                _commonSettings.ClearControls(tabScriptureSettings);
                //tabScriptureSettings.SelectTab("tabBasics");
                BasicsPanelDefaults();
                ChapterPanelDefaults();
                HeadingsPanelDefaults();
                FootnotePanelDefaults();
                TextSpacingPanelDefaults();
                OtherPanelDefaults();
                LayoutPanelDefaults();
                //Load from CSS
                string[] dims = GetDefault("PAGE/PROPERTY/size");
                if (dims.Length == 2)
                {
                    txtBasicsWidth.Text = dims[0];
                    txtBasicsHeight.Text = dims[1];
                    _commonSettings.NonstandardSize(txtBasicsWidth.Text, txtBasicsHeight.Text, ddlBasicsPageSize, ddlBasicsPaperSize, chkBasicsCropMark);
                }
                string[] margins = GetDefault("PAGE/PROPERTY/margin");
                switch (margins.Length)
                {
                    case 4:
                        txtBasicsTop.Text = margins[0];
                        txtBasicsOutside.Text = margins[1];
                        txtBasicsBottom.Text = margins[2];
                        txtBasicsInside.Text = margins[3];
                        break;
                    case 3:
                        txtBasicsTop.Text = margins[0];
                        txtBasicsOutside.Text = txtBasicsInside.Text = margins[1];
                        txtBasicsBottom.Text = margins[2];
                        break;
                    case 2:
                        txtBasicsTop.Text = txtBasicsBottom.Text = margins[0];
                        txtBasicsOutside.Text = txtBasicsInside.Text = margins[1];
                        break;
                    case 1:
                        txtBasicsTop.Text = txtBasicsBottom.Text = txtBasicsOutside.Text = txtBasicsInside.Text = margins[0];
                        break;
                }

                SetStringCSSValue(txtBasicsTop, "PAGE/PROPERTY/margin-top");
                SetStringCSSValue(txtBasicsBottom, "PAGE/PROPERTY/margin-bottom");
                SetStringCSSValue(txtBasicsInside, "PAGE/PROPERTY/margin-left");
                SetStringCSSValue(txtBasicsOutside, "PAGE/PROPERTY/margin-right");
                SetStringCSSValue(ddlBasicsPaperSize, "PAGE/PROPERTY/-ps-paper-size");
                if (GetBoolCSSValue("PAGE/PROPERTY/marks") && ddlBasicsPaperSize.Enabled)
                {
                    chkBasicsCropMark.Checked = true;
                }
                else
                {
                    chkBasicsCropMark.Checked = false;
                }
                // Start Others TAB

                var ReferencePositions = new[] { "top-left", "top-center", "top-right" };
                bool isMirrorRefPage = true;
                for (int i = 0; i < ReferencePositions.Length; i++)
                {
                    _pageHeaderFooterPos = ReferencePositions[i];
                    string[] bottomFSize = GetDefault("PAGE/REGION/" + ReferencePositions[i], "PROPERTY/content");
                    if (bottomFSize.Length > 20)
                    {
                        SetStringCSSValue(txtOtherFontSize, "PAGE/REGION/" + ReferencePositions[i], "PROPERTY/font-size");
                        SetStringCSSValue(txtOtherNonConsecutiveReferenceSeparator, "PAGE/PROPERTY/-ps-NonConsecutiveReferenceSeparator-string");
                        txtOtherChapterVerseSeparator.Text = bottomFSize[11].Replace("\"", "").Replace("\'", "");
                        txtOtherConsecutiveReferenceSeparator.Text = bottomFSize[17].Replace("\"", "").Replace("\'", "");
                        ddlOtherReferenceLocation.Text = _pageHeaderFooterPos;
                        isMirrorRefPage = false;
                        break;
                    }
                }
                if (isMirrorRefPage)
                {
                    _pageHeaderFooterPos = "top-left";
                    string[] left = GetDefault("PAGE/PSEUDO/left/REGION/" + _pageHeaderFooterPos, "PSEUDO/REGION/" + _pageHeaderFooterPos + "/PROPERTY/content");
                    if (left.Length > 20)
                    {
                        ddlOtherReferenceLocation.Text = "top-outside";
                        SetStringCSSValue(txtOtherFontSize, "PAGE/PSEUDO/left/REGION/" + _pageHeaderFooterPos, "PAGE/PSEUDO/left/REGION/" + _pageHeaderFooterPos + "/PROPERTY/font-size");
                        SetStringCSSValue(txtOtherNonConsecutiveReferenceSeparator, "PAGE/PROPERTY/-ps-NonConsecutiveReferenceSeparator-string");
                        txtOtherChapterVerseSeparator.Text = left[11].Replace("\"", "").Replace("\'", "");
                        txtOtherConsecutiveReferenceSeparator.Text = left[17].Replace("\"", "").Replace("\'", "");
                    }
                    else
                    {
                        _pageHeaderFooterPos = "top-right";
                        string[] right = GetDefault("PAGE/PSEUDO/left/REGION/" + _pageHeaderFooterPos, "PSEUDO/REGION/" + _pageHeaderFooterPos + "/PROPERTY/content");
                        if (right.Length > 20)
                        {
                            ddlOtherReferenceLocation.Text = "top-outside";
                        }
                    }

                }

                bool isMirrorPageNum = false;
                var PageNumberPositions = new[] { "top-left", "bottom-left", "top-right", "bottom-right" };
                for (int i = 0; i < PageNumberPositions.Length; i++)
                {
                    _pageHeaderFooterPos = PageNumberPositions[i];
                    string[] left = GetDefault("PAGE/PSEUDO/left/REGION/" + _pageHeaderFooterPos + "/PROPERTY/content");
                    if (left.Length == 4)
                    {
                        _pageHeaderFooterPos = "top-left";
                        string[] refleft = GetDefault("PAGE/PSEUDO/left/REGION/" + _pageHeaderFooterPos + "/PROPERTY/content");
                        if (refleft.Length >= 17)
                        {
                            ddlOtherReferenceLocation.Text = "top-outside";
                            SetStringCSSValue(txtOtherFontSize, "PAGE/PSEUDO/left/REGION/" + _pageHeaderFooterPos, "PAGE/PSEUDO/left/REGION/" + _pageHeaderFooterPos + "/PROPERTY/font-size");
                            SetStringCSSValue(txtOtherNonConsecutiveReferenceSeparator, "PAGE/PROPERTY/-ps-NonConsecutiveReferenceSeparator-string");
                            txtOtherChapterVerseSeparator.Text = refleft[11].Replace("\"", "").Replace("\'", "");
                        }
                        else if (refleft.Length == 0)
                        {
                            _pageHeaderFooterPos = "top-center";
                            string[] refcenter = GetDefault("PAGE/PSEUDO/left/REGION/" + _pageHeaderFooterPos + "/PROPERTY/content");
                            if (refcenter.Length > 20)
                            {
                                ddlOtherReferenceLocation.Text = _pageHeaderFooterPos;
                                SetStringCSSValue(txtOtherFontSize, "PAGE/PSEUDO/left/REGION/" + _pageHeaderFooterPos, "PAGE/PSEUDO/left/REGION/" + _pageHeaderFooterPos + "/PROPERTY/font-size");
                                SetStringCSSValue(txtOtherNonConsecutiveReferenceSeparator, "PAGE/PROPERTY/-ps-NonConsecutiveReferenceSeparator-string");
                                txtOtherChapterVerseSeparator.Text = refcenter[11].Replace("\"", "").Replace("\'", "");
                                txtOtherConsecutiveReferenceSeparator.Text = refcenter[17].Replace("\"", "").Replace("\'", "");
                            }
                        }
                        isMirrorPageNum = true;

                        ddlOtherPageNumberLocation.Text = PageNumberPositions[i].IndexOf("top") >= 0 ? "top-inside" : "bottom-outside";
                        break;
                    }
                }
                if (!isMirrorPageNum)
                {
                    PageNumberPositions = new[] { "top-center", "bottom-center" };
                    for (int i = 0; i < PageNumberPositions.Length; i++)
                    {
                        _pageHeaderFooterPos = PageNumberPositions[i];
                        string[] bottomFSize = GetDefault("PAGE/REGION/" + PageNumberPositions[i], "PROPERTY/content");
                        if (bottomFSize.Length == 4)
                        {
                            ddlOtherPageNumberLocation.Text = _pageHeaderFooterPos;
                            break;
                        }
                    }
                }

                // Ends Others TAB
                SetStringCSSValue(txtBasicsGutterWidth, "RULE/CLASS/columns", "PROPERTY/column-gap");
                SetStringCSSValue(ddlBasicsColumns, "RULE/CLASS/columns", "PROPERTY/column-count");
                chkBasicsVerticalRule.Checked = GetBoolCSSValue("RULE/CLASS/columns", "PROPERTY/column-rule");
                string[] returnVal = GetDefault("PAGE/PROPERTY/-ps-BasicsVerticalRuleinGutter");
                if (returnVal.Length >= 1)
                    chkBasicsVerticalRule.Checked = returnVal[0].Replace("\"", "") == "yes";

                SetStringCSSValue(txtBasicsFontSize, "RULE/CLASS/Paragraph", "PROPERTY/font-size");
                SetStringCSSValue(txtBasicsLineSpacing, "RULE/CLASS/Paragraph", "PROPERTY/line-height");
                chkBasicsJustifyParagraphs.Checked = GetBoolCSSValue("RULE/CLASS/Paragraph", "PROPERTY/text-align");
                SetStringCSSValue(ddlBasicsFontStyle, "RULE/CLASS/Paragraph", "PROPERTY/font-style");
                SetStringCSSValue(ddlBasicsFontName, "RULE/CLASS/Paragraph", "PROPERTY/font-family");
                /*Chapter/Verse Tab */
                SetStringCSSValue(txtChapterChapterNumbers, "RULE/CLASS/ChapterNumber/PSEUDO/before", "PROPERTY/content");
                SetStringCSSValue(ddlChapterPositionChapterNumbers, "PAGE/PROPERTY/-ps-positionchapternumbers-string");
                SetStringCSSValue(txtChapterChapterString, "PAGE/PROPERTY/-ps-chapterstring-string");
                SetStringCSSValue(txtChapterPositionString, "PAGE/PROPERTY/-ps-chapterpositionstring-string");
                chkChapterBold.Checked = GetBoolCSSValue("RULE/CLASS/ChapterNumber", "PROPERTY/font-weight");
                chkChapterRaised.Checked = GetBoolCSSValue("RULE/CLASS/ChapterNumber", "PROPERTY/vertical-align");
                chkChapterIncludeVerseNumber.Checked = GetBoolCSSValue("PAGE/PROPERTY/-ps-IncludeVersenumber-string");
                /*Headings Tab*/
                SetStringCSSValue(txtHeadingsMainTitleFontSize, "RULE/CLASS/TitleMain", "PROPERTY/font-size");
                SetStringCSSValue(txtHeadingsSectionHeadingsFontSize, "RULE/CLASS/SectionHead", "PROPERTY/font-size");
                SetStringCSSValue(ddlHeadingsSectionHeadingsFontStyle, "RULE/CLASS/SectionHead", "PROPERTY/font-style");
                chkHeadingsSectionHeadingsCentered.Checked = GetBoolCSSValue("RULE/CLASS/SectionHead", "PROPERTY/text-align");
                SetStringCSSValue(txtHeadingsSectionHeadingsLineSpacing, "RULE/CLASS/SectionHead", "PROPERTY/line-height");
                SetStringCSSValue(txtHeadingsSectionHeadingsSpaceBefore, "RULE/CLASS/SectionHead", "PROPERTY/padding-top");
                SetStringCSSValue(txtHeadingsSectionHeadingsSpaceAfter, "RULE/CLASS/SectionHead", "PROPERTY/padding-bottom");

                SetStringCSSValue(txtHeadingsMajorSectionHeadingsFontSize, "RULE/CLASS/ChapterHead", "PROPERTY/font-size");
                SetStringCSSValue(ddlHeadingsMajorSectionHeadingsFontStyle, "RULE/CLASS/ChapterHead", "PROPERTY/font-style");
                chkMajorHeadingsSectionHeadingsCentered.Checked = GetBoolCSSValue("RULE/CLASS/ChapterHead", "PROPERTY/text-align");
                SetStringCSSValue(txtHeadingsMajorSectionHeadingsLineSpacing, "RULE/CLASS/ChapterHead", "PROPERTY/line-height");
                SetStringCSSValue(txtHeadingsMajorSectionHeadingsSpaceBefore, "RULE/CLASS/ChapterHead", "PROPERTY/padding-top");
                SetStringCSSValue(txtHeadingsMajorSectionHeadingsSpaceAfter, "RULE/CLASS/ChapterHead", "PROPERTY/padding-bottom");
                SetStringCSSValue(txtMajorHeadingsSectionHeadingsReferenceFontSize, "RULE/CLASS/parallelpassagereference", "PROPERTY/font-size");
                SetStringCSSValue(ddlHeadingsMajorSectionHeadingsReferenceFontStyle, "RULE/CLASS/parallelpassagereference", "PROPERTY/font-style");

                SetStringCSSValue(txtHeadingsNewSectionHeadingsFontSize, "RULE/CLASS/SectionHeadMinor", "PROPERTY/font-size");
                SetStringCSSValue(ddlHeadingsNewSectionHeadingsFontStyle, "RULE/CLASS/SectionHeadMinor", "PROPERTY/font-style");
                chkHeadingsNewSectionHeadingsCentered.Checked = GetBoolCSSValue("RULE/CLASS/SectionHeadMinor", "PROPERTY/text-align");
                SetStringCSSValue(txtHeadingsNewSectionHeadingsLineSpacing, "RULE/CLASS/SectionHeadMinor", "PROPERTY/line-height");
                SetStringCSSValue(txtHeadingsNewSectionHeadingsSpaceBefore, "RULE/CLASS/SectionHeadMinor", "PROPERTY/padding-top");
                SetStringCSSValue(txtHeadingsNewSectionHeadingsSpaceAfter, "RULE/CLASS/SectionHeadMinor", "PROPERTY/padding-bottom");
                /*Footnotes Tab*/
                SetStringCSSValue(txtFootnotesFontSizeforNotes, "RULE/CLASS/footnote", "PROPERTY/font-size");
                //SetStringCSSValue(chkFootnotesReferenceInFootnoteInside, "RULE/CLASS/footnote", "PROPERTY/list-style-position");
                chkFootnotesReferenceInFootnoteInside.Checked = GetBoolCSSValue("RULE/CLASS/footnote", "PROPERTY/list-style-position");
                
                SetStringCSSValue(txtFootnotesLineSpacingforNotes, "RULE/CLASS/footnote", "PROPERTY/line-height");
                SetStringCSSValue(ddlFootnotesReferenceInFootnoteFontStyle, "RULE/CLASS/footnote", "PROPERTY/font-style");

                SetStringCSSValue(ddlFootnotesFootnoteCallerFontStyle, "RULE/CLASS/footnote/PSEUDO/footnote-call", "PROPERTY/font-style");

                SetStringCSSValue(ddlFootnotesCrossReferenceCallerFontStyle, "RULE/CLASS/crossReference/PSEUDO/footnote-call", "PROPERTY/font-style");
                /*Text Spacing*/
                SetStringCSSValue(txtTextSpacingBetweenWordsDesired, "RULE/CLASS/Paragraph_continuation ", "PROPERTY/word-spacing");
                SetStringCSSValue(txtTextSpacingBetweenLettersLetterDesired, "RULE/CLASS/Paragraph_continuation ", "PROPERTY/letter-spacing");
                /*Other Tab*/
                SetStringCSSValue(ddlOtherTableHeadersFontStyle, "PAGE/PROPERTY/-ps-TableHeadersFontStyle-string");
                // Add the Default List Items from Array to Sections - if its no from flex plugin
                if (!_plugin)
                {
                    GetPageSectionSteps(lvwDocumentSection, lvwDocumentSteps);
                    // if no Section is added then fill it from Array to load it.
                    //Document Tab
                    LoadScriptureSectionNames(lvwDocumentSection, _xhtml);
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }

        /// <summary>
        /// To load default values for ListView
        /// </summary>
        /// <param name="lstViewCtl">listView control name</param>
        /// <param name="xhtml">xhtml file</param>
        public void LoadScriptureSectionNames(ListView lstViewCtl, string xhtml)
        {
            string[] arrScriptureSections = new string[8] 
            { 
            "Cover", "Title", "Rights", "Introduction", "List", "Lexicon", "Glossary",  "Maps" 
            };

            if (lstViewCtl.Items.Count == 0)
            {
                for (int i = 0; i < arrScriptureSections.Length; i++)
                {
                    string sectionName = arrScriptureSections[i];
                    var tempListView = new ListViewItem { Text = sectionName };
                    if (sectionName == "Lexicon")
                    {
                        tempListView.SubItems.Add(Path.GetFileName(xhtml));
                    }
                    lstViewCtl.Items.Add(tempListView);
                }
            }
        }

        private void SetStringCSSValue(Control ctl, string classXPath)
        {
            string[] val = GetDefault(classXPath);
            if (val.Length == 1)
            {
                ctl.Text = val[0].Replace("\"", "");
            }
        }

        private bool GetBoolCSSValue(string classXPath)
        {
            bool containValue = false;
            string[] val = GetDefault(classXPath);
            if (val.Length > 0)
            {
                if (val[0].IndexOf("no") >= 0 || (classXPath.Contains("marks") && val[0].IndexOf("none") >= 0))
                {
                    return false;
                }
                containValue = true;
            }
            return containValue;
        }

        private void SetStringCSSValue(Control ctl, string classXPath, string propertyXPath)
        {
            string[] val = GetDefault(classXPath, propertyXPath);
            if (val.Length > 0)
            {
                ctl.Text = val[0].Replace("\"", "");
            }
        }

        private bool GetBoolCSSValue(string classXPath, string propertyXPath)
        {
            bool containValue = false;
            string[] val = GetDefault(classXPath, propertyXPath);
            if (val.Length > 0)
            {
                if (propertyXPath.Contains("text-align") && val[0] == "left")
                {
                    return false;
                }
                if (propertyXPath.Contains("font-weight") && val[0] == "normal")
                {
                    return false;
                }
                if (propertyXPath.Contains("vertical-align") && val[0] == "baseline")
                {
                    return false;
                }
                if (propertyXPath.Contains("list-style-position") && val[0] == "none")
                {
                    return false;
                }
                containValue = true;
            }
            return containValue;
        }


        #endregion

        #region LayoutPanelDefaults
        /// <summary>
        /// Layout Panel's Default Values
        /// </summary>
        private void LayoutPanelDefaults()
        {
            try
            {
                //Composer
                ddlLayoutComposer.Items.Add("Paragraph");
                ddlLayoutComposer.Items.Add("Single-line");
                ddlLayoutComposer.SelectedIndex = 0;

                //Kashidas
                LoadYesNo(ddlLayoutKashidas, 0);

                //Character Direction
                ddlLayoutCharacterDirection.Items.Add("ltr");
                ddlLayoutCharacterDirection.Items.Add("rtl");
                ddlLayoutCharacterDirection.SelectedIndex = 0;

                //Diacritic Positioning
                ddlLayoutDiacriticPositioning.Items.Add("Before");
                ddlLayoutDiacriticPositioning.Items.Add("After");
                ddlLayoutDiacriticPositioning.SelectedIndex = 0;

                //Paragraph Justification
                LoadYesNo(ddlLayoutParagraphJustification, 0);

                //Chiness / Japanese Layout
                LoadYesNo(ddlLayoutChinessJapaneseLayout, 0);
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }
        #endregion LayoutPanelDefaults

        #region OtherPanelDefaults
        /// <summary>
        /// Other Panel's Default Values
        /// </summary>
        private void OtherPanelDefaults()
        {
            try
            {
                //Reference Location
                ddlOtherReferenceLocation.Items.Add("top-center");
                ddlOtherReferenceLocation.Items.Add("top-outside");
                ddlOtherReferenceLocation.Items.Add("top-left");
                ddlOtherReferenceLocation.Items.Add("top-right");
                ddlOtherReferenceLocation.SelectedIndex = 0;

                //Page Number Location
                ddlOtherPageNumberLocation.Items.Add("top-inside");
                ddlOtherPageNumberLocation.Items.Add("top-center");
                ddlOtherPageNumberLocation.Items.Add("bottom-center");
                ddlOtherPageNumberLocation.Items.Add("bottom-outside");
                ddlOtherPageNumberLocation.SelectedIndex = 0;

                //Font Style Loading
                _commonSettings.SetFontStyle(ddlOtherTableHeadersFontStyle, 0);
                _commonSettings.SetFontName(ddlOtherBoldItalic1, "Charis SIL");
                _commonSettings.SetFontName(ddlOtherBoldItalic2, "Charis SIL");
                _commonSettings.SetFontName(ddlOtherItalic1, "Charis SIL");
                _commonSettings.SetFontName(ddlOtherItalic2, "Charis SIL");
                _commonSettings.SetFontName(ddlOtherBold1, "Charis SIL");
                _commonSettings.SetFontName(ddlOtherBold2, "Charis SIL");

                LoadReferenceFormat(_mirrorPage);

            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }
        #endregion OtherPanelDefaults

        #region LoadReferenceFormat
        private void LoadReferenceFormat(ArrayList pageType)
        {
            //Reference Formats
            ddlOtherReferenceFormat.Items.Clear();
            for (int i = 0; i < pageType.Count; i++)
            {
                ddlOtherReferenceFormat.Items.Add(pageType[i]);
            }
            ddlOtherReferenceFormat.SelectedIndex = 0;
        }
        #endregion LoadReferenceFormat

        #region TextSpacingPanelDefaults
        /// <summary>
        /// Text Spacing Panel's Default Values
        /// </summary>
        private void TextSpacingPanelDefaults()
        {
            try
            {
                // To Load Kerning Values
                _commonSettings.LoadKerningValues(ddlTextSpacingKerning);
                // To load Hyphenation language dictionary
                _commonSettings.LoadHyphenationLanguages(ddlTextSpacingHyphenationLanguage);
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }
        #endregion

        #region FootnotePanelDefaults
        /// <summary>
        /// Footnote Panel's Default Values
        /// </summary>
        private void FootnotePanelDefaults()
        {
            try
            {
                //Populating Font Style
                _commonSettings.SetFontStyle(ddlFootnotesFootnoteCallerFontStyle, 0);
                _commonSettings.SetFontStyle(ddlFootnotesCrossReferenceCallerFontStyle, 0);
                _commonSettings.SetFontStyle(ddlFootnotesReferenceInFootnoteFontStyle, 0);
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }
        #endregion

        #region LoadYesNo
        /// <summary>
        /// Load Yes No value to combo box with Default selection
        /// </summary>
        /// <param name="myCombo">Combobox Name</param>
        /// <param name="selectedIndex">Default selection Index</param>
        private static void LoadYesNo(ComboBox myCombo, sbyte selectedIndex)
        {
            try
            {
                myCombo.Items.Add("Yes");
                myCombo.Items.Add("No");
                myCombo.SelectedIndex = selectedIndex;
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }
        #endregion

        #region HeadingsPanelDefaults
        /// <summary>
        /// Headings Panel's Default Values
        /// </summary>
        private void HeadingsPanelDefaults()
        {
            try
            {
                //Populating Font Style
                _commonSettings.SetFontStyle(ddlHeadingsSectionHeadingsFontStyle, 0);
                _commonSettings.SetFontStyle(ddlHeadingsMajorSectionHeadingsFontStyle, 0);
                _commonSettings.SetFontStyle(ddlHeadingsMajorSectionHeadingsReferenceFontStyle, 0);
                _commonSettings.SetFontStyle(ddlHeadingsNewSectionHeadingsFontStyle, 0);
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }
        #endregion

        #region ChapterPanelDefaults
        /// <summary>
        /// Chapter/Verse Panel's Default Values
        /// </summary>
        private void ChapterPanelDefaults()
        {
            try
            {
                txtChapterChapterNumbers.Text = "1";
                //ddlChapterPositionChapterNumbers Combobox
                ddlChapterPositionChapterNumbers.Items.Add("Margin");
                ddlChapterPositionChapterNumbers.Items.Add("Inline");
                ddlChapterPositionChapterNumbers.Items.Add("Drop Capital");
                ddlChapterPositionChapterNumbers.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }
        #endregion

        #region BasicsPanelDefaults
        /// <summary>
        /// Basics Panel's Default Values
        /// </summary>
        private void BasicsPanelDefaults()
        {
            try
            {
                _commonSettings.SetPageandPaperSetting(ddlBasicsPageSize, ddlBasicsPaperSize);
                _commonSettings.SetPageMargins(txtBasicsInside, txtBasicsOutside, txtBasicsTop, txtBasicsBottom);
                _commonSettings.SetColumnCount(ddlBasicsColumns);
                _commonSettings.SetFontName(ddlBasicsFontName, "Charis SIL");
                _commonSettings.SetFontStyle(ddlBasicsFontStyle, 0);
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }
        #endregion

        #region AssignValue

        private void AssignValue(object sender, EventArgs e)
        {
            try
            {
                _confirmSave = true;
                string textValue = "";
                var ctrl = ((Control)sender);
                if (ctrl is TextBox)
                {
                    textValue = ctrl.Text;
                }
                else if (ctrl is CheckBox)
                {
                    var chk = ((CheckBox)sender);
                    if (ctrl.Name == "chkOtherIncludeVerseInHeaderReferences")
                    {
                        textValue = chk.Checked ? "yes" : "no";
                    }
                    else if (ctrl.Name == "chkBasicsJustifyParagraphs")
                    {
                        textValue = chk.Checked ? "justify" : "left";
                    }
                    else if (ctrl.Name == "chkChapterBold")
                    {
                        textValue = chk.Checked ? "bolder" : "normal";
                    }
                    else if (ctrl.Name == "chkChapterRaised")
                    {
                        textValue = chk.Checked ? "top" : "baseline";
                    }
                    else if (ctrl.Name == "chkChapterIncludeVerseNumber")
                    {
                        textValue = chk.Checked ? "yes" : "no";
                    }
                    else if (ctrl.Name == "chkHeadingsSectionHeadingsCentered")
                    {
                        textValue = chk.Checked ? "center" : "left";
                    }
                    else if (ctrl.Name == "chkHeadingsSectionHeadingsCentered")
                    {
                        textValue = chk.Checked ? "center" : "left";
                    }
                    else if (ctrl.Name == "chkMajorHeadingsSectionHeadingsCentered")
                    {
                        textValue = chk.Checked ? "center" : "left";
                    }
                    else if (ctrl.Name == "chkHeadingsNewSectionHeadingsCentered")
                    {
                        textValue = chk.Checked ? "center" : "left";
                    }
                    else if (ctrl.Name == "chkBasicsVerticalRule")
                    {
                        _dicMap["isVerticalRule"] = chk.Checked ? "yes" : "no";
                        if (chk.Checked)
                            textValue = chk.Checked.ToString();
                    }
                    else if (ctrl.Name == "chkFootnotesReferenceInFootnoteInside")
                    {
                        textValue = chk.Checked ? "yes" : "no";
                    }
                    else if (chk.Checked)
                    {
                        textValue = chk.Checked.ToString();
                    }
                }
                else if (ctrl is ComboBox)
                {
                    if (ctrl.Text.ToLower() == "top-center")
                    {
                        textValue = _pageHeaderFooterPos = "top-center";
                    }
                    else if (ctrl.Text.ToLower() == "top-inside")
                    {
                        textValue = _pageHeaderFooterPos = "top-inside";
                    }
                    else if (ctrl.Text.ToLower() == "bottom-center")
                    {
                        textValue = _pageHeaderFooterPos = "bottom-center";
                    }
                    else if (ctrl.Text.ToLower() == "bottom-outside")
                    {
                        textValue = _pageHeaderFooterPos = "bottom-outside";
                    }
                    else if (ctrl.Text.ToLower() == "top-outside")
                    {
                        textValue = _pageHeaderFooterPos = "top-outside";
                    }
                    else if (ctrl.Text.ToLower() == "top-left")
                    {
                        textValue = _pageHeaderFooterPos = "top-left";
                    }
                    else if (ctrl.Text.ToLower() == "top-right")
                    {
                        textValue = _pageHeaderFooterPos = "top-right";
                    }
                    else
                    {
                        textValue = ctrl.Text;
                    }
                }
                if (ctrl.Name != "ddlOtherReferenceFormat")
                {
                    textValue = textValue.ToLower();
                }
                _dicMap[ctrl.Name] = textValue;
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }
        #endregion

        #region AssignValueUnit
        private void AssignValueUnit(object sender, EventArgs e)
        {
            string errorMessage = string.Empty;
            try
            {
                _confirmSave = true;
                var ctrl = ((Control)sender);
                string textValue = ctrl.Text.Trim();
                if (ctrl.Name != "txtChapterChapterNumbers")
                {
                    textValue = _commonSettings.ConcateUnit(ctrl);
                }
                ctrl.Text = textValue;
                if (ctrl.Name == "txtBasicsWidth")
                {
                    // Basics Tab
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, "2.91in", "22in");
                }
                else if (ctrl.Name == "txtBasicsHeight")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, "2.91in", "22in");
                }
                else if (ctrl.Name == "txtBasicsInside")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, ".25in", "1.25in");
                }
                else if (ctrl.Name == "txtBasicsOutside")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, ".25in", "1.25in");
                }
                else if (ctrl.Name == "txtBasicsTop")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, ".25in", "1.25in");
                }
                else if (ctrl.Name == "txtBasicsBottom")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, ".25in", "1.25in");
                }
                else if (ctrl.Name == "txtBasicsGutterWidth")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, ".25in", "1in");
                }
                else if (ctrl.Name == "txtBasicsFontSize")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, "8pt", "14pt");
                }
                else if (ctrl.Name == "txtBasicsLineSpacing")
                {
                    errorMessage = _commonSettings.ValidateLineSpacing(ctrl, txtBasicsFontSize);
                }
                else if (ctrl.Name == "txtChapterChapterNumbers")
                {
                    // chapter/verse Tab
                    if (int.Parse(ctrl.Text) > 150)
                    {
                        errorMessage = "Chapter Number cannot be more than 150";
                    }
                }
                else if (ctrl.Name == "txtHeadingsMainTitleFontSize")
                {
                    // Headings Tab
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, "8pt", "14pt");
                }
                else if (ctrl.Name == "txtHeadingsSectionHeadingsFontSize")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, "8pt", "14pt");
                    _commonSettings.CalculateFontSizeDependent(ctrl, txtHeadingsSectionHeadingsLineSpacing, _dicMap, 1.10F);
                    _commonSettings.CalculateFontSizeDependent(ctrl, txtHeadingsSectionHeadingsSpaceAfter, _dicMap, 1.5F);
                    _commonSettings.CalculateFontSizeDependent(ctrl, txtHeadingsSectionHeadingsSpaceBefore, _dicMap, 1.5F);
                }
                else if (ctrl.Name == "txtHeadingsSectionHeadingsLineSpacing")
                {
                    errorMessage = _commonSettings.ValidateLineSpacing(ctrl, txtHeadingsSectionHeadingsFontSize);
                }
                else if (ctrl.Name == "txtHeadingsSectionHeadingsSpaceAfter")
                {
                    errorMessage = _commonSettings.ValidateSpaceSelector(txtHeadingsSectionHeadingsFontSize, ctrl.Text, "After");
                }
                else if (ctrl.Name == "txtHeadingsSectionHeadingsSpaceBefore")
                {
                    errorMessage = _commonSettings.ValidateSpaceSelector(txtHeadingsSectionHeadingsFontSize, ctrl.Text, "Before");
                }
                else if (ctrl.Name == "txtHeadingsSectionHeadingsReferenceFontSize")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, "8pt", "14pt");
                }
                else if (ctrl.Name == "txtHeadingsMajorSectionHeadingsFontSize")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, "8pt", "14pt");
                    _commonSettings.CalculateFontSizeDependent(ctrl, txtHeadingsMajorSectionHeadingsLineSpacing, _dicMap, 1.10F);
                    _commonSettings.CalculateFontSizeDependent(ctrl, txtHeadingsMajorSectionHeadingsSpaceAfter, _dicMap, 1.5F);
                    _commonSettings.CalculateFontSizeDependent(ctrl, txtHeadingsMajorSectionHeadingsSpaceBefore, _dicMap, 1.5F);
                }
                else if (ctrl.Name == "txtHeadingsMajorSectionHeadingsLineSpacing")
                {
                    errorMessage = _commonSettings.ValidateLineSpacing(ctrl, txtHeadingsMajorSectionHeadingsFontSize);
                }
                else if (ctrl.Name == "txtHeadingsMajorSectionHeadingsSpaceAfter")
                {
                    errorMessage = _commonSettings.ValidateSpaceSelector(txtHeadingsMajorSectionHeadingsFontSize, ctrl.Text, "After");
                }
                else if (ctrl.Name == "txtHeadingsMajorSectionHeadingsSpaceBefore")
                {
                    errorMessage = _commonSettings.ValidateSpaceSelector(txtHeadingsMajorSectionHeadingsFontSize, ctrl.Text, "Before");
                }
                else if (ctrl.Name == "txtHeadingsMajorSectionHeadingsReferenceFontSize")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, "8pt", "14pt");
                }
                else if (ctrl.Name == "txtHeadingsNewSectionHeadingsFontSize")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, "8pt", "14pt");
                    _commonSettings.CalculateFontSizeDependent(ctrl, txtHeadingsNewSectionHeadingsLineSpacing, _dicMap, 1.10F);
                    _commonSettings.CalculateFontSizeDependent(ctrl, txtHeadingsNewSectionHeadingsSpaceAfter, _dicMap, 1.5F);
                    _commonSettings.CalculateFontSizeDependent(ctrl, txtHeadingsNewSectionHeadingsSpaceBefore, _dicMap, 1.5F);
                }
                else if (ctrl.Name == "txtHeadingsNewSectionHeadingsLineSpacing")
                {
                    errorMessage = _commonSettings.ValidateLineSpacing(ctrl, txtHeadingsNewSectionHeadingsFontSize);
                }
                else if (ctrl.Name == "txtHeadingsNewSectionHeadingsSpaceAfter")
                {
                    errorMessage = _commonSettings.ValidateSpaceSelector(txtHeadingsNewSectionHeadingsFontSize, ctrl.Text, "After");
                }
                else if (ctrl.Name == "txtHeadingsNewSectionHeadingsSpaceBefore")
                {
                    errorMessage = _commonSettings.ValidateSpaceSelector(txtHeadingsNewSectionHeadingsFontSize, ctrl.Text, "Before");
                }
                else if (ctrl.Name == "txtHeadingsNewSectionHeadingsReferenceFontSize")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, "8pt", "14pt");
                }
                else if (ctrl.Name == "txtFootnotesFontSizeforNotes")
                {
                    // Footnotes Tab
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, "8pt", "14pt");
                    _commonSettings.CalculateFontSizeDependent(ctrl, txtFootnotesLineSpacingforNotes, _dicMap, 1.10F);
                }
                else if (ctrl.Name == "txtFootnotesLineSpacingforNotes")
                {
                    errorMessage = _commonSettings.ValidateLineSpacing(ctrl, txtFootnotesFontSizeforNotes);
                }
                else if (ctrl.Name == "txtTextSpacingBetweenLettersLetterDesired")
                {
                    // Text Spacing Tab
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, "0pt", "6pt");
                }
                else if (ctrl.Name == "txtTextSpacingBetweenLettersLetterMaximum")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, "0pt", "6pt");
                }
                else if (ctrl.Name == "txtTextSpacingBetweenLettersLetterMinimum")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, "0pt", "6pt");
                }
                else if (ctrl.Name == "txtTextSpacingBetweenWordsDesired")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, "0pt", "6pt");
                }
                else if (ctrl.Name == "txtTextSpacingBetweenWordsMaximum")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, "0pt", "6pt");
                }
                else if (ctrl.Name == "txtTextSpacingBetweenWordsMinimum")
                {
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, "0pt", "6pt");
                }
                else if (ctrl.Name == "txtOtherFontSize")
                {
                    // Other Tab
                    errorMessage = _commonSettings.RangeValidate(ctrl.Text, "8pt", "14pt");
                }
                else if (ctrl.Name == "txtLayoutHorizontalScaling")
                {
                    // Layout Tab
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
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }

        #endregion

        #region btnPrevious_Click
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                _commonSettings.MoveTab(tabScriptureSettings, 'P');
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }
        #endregion

        #region btnNext_Click
        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                _commonSettings.MoveTab(tabScriptureSettings, 'N');
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }
        #endregion

        #region btnSave_Click
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (_commonSettings.Validations(tabScriptureSettings, _errProvider))
                {
                    // Generate a new FileName
                    if (!_fileSaved)
                    {
                        //GetNewFileName();
                        JobName = _plugin ? SettingBl.GetNewFileName(ddlCSS.Text, "util", _dicPath, "onsave") : SettingBl.GetNewFileName(ddlCSS.Text, "scripture", _dicPath, "onsave");
                        var saveDlg = new SaveFileDialog { FileName = JobName };

                        if (saveDlg.ShowDialog() == DialogResult.OK)
                        {
                            JobName = saveDlg.FileName;
                            LastSaved = JobName;
                        }
                        else
                        {
                            return;
                        }
                        Text = Path.GetFileName(JobName);
                        // oo utility
                        if (!_plugin)
                        {
                            AddFileToXML(JobName, chkDefaultCSS.Checked.ToString());
                        }
                        _fileSaved = true;
                    }
                    SetCSSTemplate();
                    SetAsDefaultCSS = chkDefaultCSS.Checked;
                    _confirmSave = false;
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }
        #endregion

        #region DicExplorerTree
        /// <summary>
        /// To add File to XML
        /// </summary>
        /// <param name="fileName">File Name</param>
        /// <param name="defaultValue">Default Value</param>
        /// <returns></returns>
        public bool AddFileToXML(string fileName, string defaultValue)
        {
            try
            {
                string fn = Path.GetFileName(fileName);
                string dicSolution = Common.PathCombine(_dicPath, Path.GetFileName(_dicPath) + ".de");
                if (!File.Exists(dicSolution))
                    return false;
                var projXml = new XmlDocument();
                projXml.Load(dicSolution);

                XmlElement root = projXml.DocumentElement;
                // remove all default true;
                if (defaultValue == "True")
                {
                    if (root != null) PopulateTreeViewRemoveDefault(root.FirstChild);
                }

                XmlNode tempNode = projXml.CreateNode("element", "File", "");

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
                Xa.Value = defaultValue;
                tempNode.Attributes.Append(Xa);
                if (root != null) root.FirstChild.AppendChild(tempNode);
                projXml.Save(dicSolution);
                return true;
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
            return false;
        }

        private static void PopulateTreeViewRemoveDefault(XmlNode rootNode)
        {
            try
            {
                foreach (XmlNode tempNode in rootNode)
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
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }
        #endregion

        #region ddlCSS_SelectedIndexChanged
        private void ddlCSS_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _fileSaved = false;
                _cssTree = _commonSettings.LoadCss(_dicPath, ddlCSS.Text);
                LoadDefault();
                _confirmSave = false;
                JobName = _plugin ? SettingBl.GetNewFileName(ddlCSS.Text, "util", _dicPath, "onplugin") : SettingBl.GetNewFileName(ddlCSS.Text, "scripture", _dicPath, "onselect");
                Text = Path.GetFileName(JobName);
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }
        #endregion

        #region SetCSSTemplate
        /// <summary>
        /// To set the CSS Template
        /// </summary>
        public void SetCSSTemplate()
        {
            try
            {
                if (_dicMap.ContainsKey("ddlOtherReferenceLocation") && !_dicMap.ContainsKey("ddlOtherPageNumberLocation"))
                {
                    _dicMap.Add("ddlOtherPageNumberLocation", ddlOtherPageNumberLocation.Text);
                }
                else if (_dicMap.ContainsKey("ddlOtherPageNumberLocation") && !_dicMap.ContainsKey("ddlOtherReferenceLocation"))
                {
                    _dicMap.Add("ddlOtherReferenceLocation", ddlOtherReferenceLocation.Text);
                }
                else if (_dicMap.ContainsKey("ddlOtherReferenceFormat") && !_dicMap.ContainsKey("ddlOtherPageNumberLocation") && !_dicMap.ContainsKey("ddlOtherReferenceLocation"))
                {
                    _dicMap.Add("ddlOtherPageNumberLocation", ddlOtherPageNumberLocation.Text);
                    _dicMap.Add("ddlOtherReferenceLocation", ddlOtherReferenceLocation.Text);
                }
                
                if (_dicMap.ContainsKey("ddlOtherReferenceLocation") || _dicMap.ContainsKey("ddlOtherPageNumberLocation"))
                {
                    string pageloc;
                    if (_dicMap["ddlOtherReferenceLocation"] == "top-center" || _dicMap["ddlOtherReferenceLocation"] == "top-left"
                        || _dicMap["ddlOtherReferenceLocation"] == "top-right")
                    {
                        if (_dicMap["ddlOtherPageNumberLocation"] == "bottom-center" || _dicMap["ddlOtherPageNumberLocation"] == "top-center")
                        {
                            pageloc = InsertPageNumberPosition();
                            _dicMap["pageReference"] = InsertReference(_dicMap["ddlOtherReferenceLocation"], pageloc, false);
                            _dicMap["mirroredReference"] = InsertEmptyMirroredPages();
                        }
                        else if (_dicMap["ddlOtherPageNumberLocation"] == "top-inside" || _dicMap["ddlOtherPageNumberLocation"] == "bottom-outside")
                        {
                            pageloc = InsertPageNumberPosition();
                            _dicMap["pageReference"] = InsertEmptySingleRegions();
                            _dicMap["mirroredReference"] = InsertReference(_dicMap["ddlOtherReferenceLocation"], pageloc, true);
                        }
                    }
                    else if (_dicMap["ddlOtherReferenceLocation"] == "top-outside")
                    {
                        pageloc = InsertPageNumberPosition();
                        _dicMap["pageReference"] = InsertEmptySingleRegions();
                        _dicMap["mirroredReference"] = InsertReference(_dicMap["ddlOtherReferenceLocation"], pageloc, true);
                    }
                }

                _dicMap["link"] = ddlCSS.Text;
                _dicMap["PagePageSize"] = ddlBasicsPageSize.Text;
                _dicMap["PagePaperSize"] = ddlBasicsPaperSize.Text;
                _dicMap["pageWidth"] = txtBasicsWidth.Text;
                _dicMap["pageHeight"] = txtBasicsHeight.Text;
                _dicMap["PageInside"] = txtBasicsInside.Text;
                _dicMap["PageOutside"] = txtBasicsOutside.Text;
                _dicMap["PageTop"] = txtBasicsTop.Text;
                _dicMap["PageBottom"] = txtBasicsBottom.Text;
                _dicMap["PageGutterWidth"] = txtBasicsGutterWidth.Text;
                _dicMap["PageCropMark"] = chkBasicsCropMark.Checked ? "crop" : "none";
                _dicMap["PageVerticalRule"] = chkBasicsVerticalRule.Checked ? "True" : "";
                _dicMap["PageColumn"] = ddlBasicsColumns.Text;
                _dicMap["verbose"] = "yes";
                _dicMap["chkFootnotesReferenceInFootnoteInside"] = chkFootnotesReferenceInFootnoteInside.Checked ? "inside" : "none";
                _dicMap["ddlOtherPageNumberLocation"] = ddlOtherPageNumberLocation.Text == string.Empty ? "top-center" : ddlOtherPageNumberLocation.Text;
                if (!_dicMap.ContainsKey("chkHeadingsNewSectionHeadingsCentered"))
                {
                    _dicMap["chkHeadingsNewSectionHeadingsCentered"] = "left";
                }
                if (!_dicMap.ContainsKey("chkHeadingsNewSectionHeadingsCentered"))
                {
                    _dicMap["chkHeadingsNewSectionHeadingsCentered"] = "left";
                }
                if (!_dicMap.ContainsKey("chkMajorHeadingsSectionHeadingsCentered"))
                {
                    _dicMap["chkMajorHeadingsSectionHeadingsCentered"] = "left";
                }
                if (!_dicMap.ContainsKey("chkHeadingsSectionHeadingsCentered"))
                {
                    _dicMap["chkHeadingsSectionHeadingsCentered"] = "left";
                }
                if (!_dicMap.ContainsKey("chkChapterIncludeVerseNumber"))
                {
                    _dicMap["chkChapterIncludeVerseNumber"] = "no";
                }
                if (!_dicMap.ContainsKey("chkChapterRaised"))
                {
                    _dicMap["chkChapterRaised"] = "baseline";
                }
                if (!_dicMap.ContainsKey("chkChapterBold"))
                {
                    _dicMap["chkChapterBold"] = "normal";
                }
                if (!_dicMap.ContainsKey("chkBasicsJustifyParagraphs"))
                {
                    _dicMap["chkBasicsJustifyParagraphs"] = "left";
                }
                if (!_dicMap.ContainsKey("chkOtherIncludeVerseInHeaderReferences"))
                {
                    _dicMap["chkOtherIncludeVerseInHeaderReferences"] = "no";
                }
                if (!_dicMap.ContainsKey("ddlOtherReferenceFormat"))
                {
                    _dicMap["ddlOtherReferenceFormat"] = ddlOtherReferenceFormat.Text;
                }
                if (!_dicMap.ContainsKey("isVerticalRule") && _dicMap.Count > 0)
                    _dicMap["isVerticalRule"] = _dicMap.ContainsKey("chkBasicsVerticalRule") ? "yes" : "no";

                if (!_plugin)
                {
                    MakeDocumentPreparation();
                }
                // Add the Default List Items from Array to Sections - if its no from flex plugin
                if (!_plugin)
                {
                    GetPageSectionSteps(lvwDocumentSection, lvwDocumentSteps);
                    // if no Section is added then fill it from Array to load it.
                    //Document Tab
                    LoadScriptureSectionNames(lvwDocumentSection, _xhtml);
                }
                var sub = new Substitution();
                sub.FileSubstitute(Common.PathCombine(_supportFolder, "scriptureTemplate.tpl"), _dicMap, JobName);
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }

        private string InsertEmptySingleRegions()
        {
            var build = new StringBuilder();
            InsertEmptyProperty(ref build);
            return build.ToString();
        }

        private void InsertEmptyProperty(ref StringBuilder build)
        {
            for (int i = 0; i < _regNames.Length; i++)
            {
                build.AppendLine("@" + _regNames[i] + "{ content: '';}");
            }
        }

        private string InsertEmptyMirroredPages()
        {
            var build = new StringBuilder();
            build.AppendLine("@page:left");
            build.AppendLine("{");
            //for (int i = 0; i < regNames.Length; i++)
            //{
            //    build.AppendLine("@" + regNames[i] + "{ content: '';}");
            //}
            InsertEmptyProperty(ref build);
            build.AppendLine("}");
            build.AppendLine("@page:right");
            build.AppendLine("{");
            //for (int i = 0; i < regNames.Length; i++)
            //{
            //    build.AppendLine("@" + regNames[i] + "{ content: '';}");
            //}
            InsertEmptyProperty(ref build);
            build.AppendLine("}");
            return build.ToString();
        }

        private string InsertReference(string regionName, string pagePos, bool isMirrorPage)
        {
            var builder = new StringBuilder();
            string regionadded;
            if (!isMirrorPage)
            {
                InsertReferenceContent(regionName, builder, 'S', "page");
                if (pagePos.Length > 0)
                {
                    builder.Append(pagePos);
                }
                regionadded = builder.ToString();
                for (int i = 0; i < _regNames.Length; i++)
                {
                    if (!regionadded.Contains(_regNames[i]))
                    {
                        builder.AppendLine("@" + _regNames[i] + "{ content: '';}");
                    }
                }
            }
            else
            {
                //Page-Left
                builder.AppendLine("@page: left {");
                var tempRegionUsed = new StringBuilder();
                if (regionName != "top-outside")
                {
                    InsertReferenceContent(regionName, tempRegionUsed, 'M', "page:left");
                }
                else
                {
                    InsertReferenceContent("top-left", tempRegionUsed, 'M', "page:left");
                }
                if (pagePos != string.Empty)
                {
                    tempRegionUsed.AppendLine(pagePos);
                }
                string referenceLoc = _dicMap["ddlOtherReferenceLocation"];
                if (referenceLoc == "top-outside")
                {
                    referenceLoc = "top-left";
                }
                if (_dicMap["ddlOtherPageNumberLocation"] == "top-inside")
                {
                    tempRegionUsed.AppendLine("@top-right { content: counter(page);}");
                    for (int i = 0; i < _regNames.Length; i++)
                    {
                        if (referenceLoc != _regNames[i] && "top-right" != _regNames[i])
                        {
                            tempRegionUsed.AppendLine("@" + _regNames[i] + "{ content: '';}");
                        }
                    }
                }
                else if (_dicMap["ddlOtherPageNumberLocation"] == "bottom-outside")
                {
                    tempRegionUsed.AppendLine("@bottom-left { content: counter(page);}");
                    for (int i = 0; i < _regNames.Length; i++)
                    {
                        if (referenceLoc != _regNames[i] && "bottom-left" != _regNames[i])
                        {
                            tempRegionUsed.AppendLine("@" + _regNames[i] + "{ content: '';}");
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < _regNames.Length; i++)
                    {
                        if (tempRegionUsed.ToString().IndexOf(_regNames[i]) == -1)
                        {
                            tempRegionUsed.AppendLine("@" + _regNames[i] + "{ content: '';}");
                        }
                    }
                }
                builder.AppendLine(tempRegionUsed.ToString());
                builder.AppendLine("}");

                //Page-Right
                builder.AppendLine("@page: right{");
                tempRegionUsed.Remove(0, tempRegionUsed.Length);
                if (regionName != "top-outside")
                {
                    InsertReferenceContent(regionName, tempRegionUsed, 'M', "page:right");
                }
                else
                {
                    InsertReferenceContent("top-right", tempRegionUsed, 'M', "page:right");
                }
                if (pagePos != string.Empty)
                {
                    tempRegionUsed.AppendLine(pagePos);
                }

                referenceLoc = _dicMap["ddlOtherReferenceLocation"];
                if (referenceLoc == "top-outside")
                {
                    referenceLoc = "top-right";
                }
                if (_dicMap["ddlOtherPageNumberLocation"] == "top-inside")
                {
                    tempRegionUsed.AppendLine("@top-left { content: counter(page);}");
                    for (int i = 0; i < _regNames.Length; i++)
                    {
                        if (referenceLoc != _regNames[i] && "top-left" != _regNames[i])
                        {
                            tempRegionUsed.AppendLine("@" + _regNames[i] + "{ content: '';}");
                        }
                    }
                }
                else if (_dicMap["ddlOtherPageNumberLocation"] == "bottom-outside")
                {
                    tempRegionUsed.AppendLine("@bottom-right { content: counter(page);}");
                    for (int i = 0; i < _regNames.Length; i++)
                    {
                        if (referenceLoc != _regNames[i] && "bottom-right" != _regNames[i])
                        {
                            tempRegionUsed.AppendLine("@" + _regNames[i] + "{ content: '';}");
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < _regNames.Length; i++)
                    {
                        if (tempRegionUsed.ToString().IndexOf(_regNames[i]) == -1)
                        {
                            tempRegionUsed.AppendLine("@" + _regNames[i] + "{ content: '';}");
                        }
                    }
                }
                builder.AppendLine(tempRegionUsed.ToString());
                builder.AppendLine("}");
            }
            return builder.ToString();
        }

        private void InsertReferenceContent(string regionName, StringBuilder builder, char pageType, string pageName)
        {
            ArrayList chapterSingleFormat = new ArrayList();
            ArrayList chapterMirrorFormat = new ArrayList();

            chapterSingleFormat.Add("content: string(bookname, first) ' ' string(chapter, first) ':' string(verse, first) '-' string(verse, last);");
            chapterSingleFormat.Add("content: string(bookname, first) ' ' string(chapter, first) ':' string(verse, first) \" \" string(bookname, last) ' ' string(chapter, last) ':' string(verse, last);");
            chapterSingleFormat.Add("content: string(bookname, first) ' ' string(chapter, first) ':' string(verse, first) ' ' string(chapter, last) ':' string(verse, last);");
            chapterSingleFormat.Add("content: string(bookname, first) ' ' string(chapter, prev)',' string(chapter, first);");
            chapterSingleFormat.Add("content: string(bookname, first) ' ' string(chapter, first);");

            chapterMirrorFormat.Add("content: string(bookname, first) ' ' string(chapter, first) ':' string(verse, first);");
            chapterMirrorFormat.Add("content: string(bookname, first) ' ' string(chapter, first);");

            builder.AppendLine("@" + regionName);
            builder.AppendLine("{");
            if (pageType == 'S')
            {
                builder.AppendLine(chapterSingleFormat[ddlOtherReferenceFormat.SelectedIndex].ToString());
            }
            else
            {
                if (pageName.IndexOf("left") > 0)
                {
                    builder.AppendLine(chapterMirrorFormat[ddlOtherReferenceFormat.SelectedIndex].ToString());
                }
                else
                {
                    builder.AppendLine(chapterMirrorFormat[ddlOtherReferenceFormat.SelectedIndex].ToString().Replace("first", "last"));
                }
            }
            if (!_dicMap.ContainsKey("txtOtherFontSize"))
            {
                _dicMap["txtOtherFontSize"] = txtOtherFontSize.Text;
            }
            builder.AppendLine("font-size: " + _dicMap["txtOtherFontSize"] + ";");
            builder.AppendLine("}");
        }

        private string InsertPageNumberPosition()
        {
            var builder = new StringBuilder();
            if (_dicMap.ContainsKey("ddlOtherPageNumberLocation"))
                if ((_dicMap["ddlOtherPageNumberLocation"] == "bottom-center") ||
                    (_dicMap["ddlOtherPageNumberLocation"] == "top-center"))
                {
                    builder.AppendLine("@" + _dicMap["ddlOtherPageNumberLocation"]);
                    builder.AppendLine("{");
                    builder.AppendLine("content: counter(page);");
                    builder.AppendLine("}");
                }
            return builder.ToString();
        }

        #endregion

        #region ddlBasicsPageSize_SelectedIndexChanged
        private void ddlBasicsPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _commonSettings.PageSizeChange(ddlBasicsPageSize, ddlBasicsPaperSize, chkBasicsCropMark, txtBasicsWidth, txtBasicsHeight);
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }
        #endregion

        #region btnDone_Click
        /// <summary>
        /// Exit from Scripture Settings
        /// </summary>
        /// <param name="sender">Object name</param>
        /// <param name="e">Events Args</param>
        private void btnDone_Click(object sender, EventArgs e)
        {
            try
            {
                if (_confirmSave)
                {
                    DialogResult dr = LocDB.Message("errWantToSave", "Do you want to save the changes?", null, LocDB.MessageTypes.YN,
                    LocDB.MessageDefault.First);
                    
                    if (dr == DialogResult.Yes)
                    {
                        btnSave_Click(sender, e);
                    }
                    else if (dr == DialogResult.No)
                    {
                        DoClose();
                    }
                }
                else
                {
                    DoClose();
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }
        /// <summary>
        /// To close the Scripture Setting Dialog
        /// </summary>
        public void DoClose()
        {
            try
            {
                if (_plugin)
                {
                    if (_commonSettings.Validations(tabScriptureSettings, _errProvider))
                    {
                        if (ddlCSS.Text != "")
                        {
                            JobName = SettingBl.GetNewFileName(ddlCSS.Text, "util", _dicPath, "onplugin");
                        }
                        SetCSSTemplate();
                    }
                }
                else
                {
                    if (!_fileSaved)
                    {
                        JobName = _cssPath;
                    }
                }
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }
        #endregion

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
            }
        }
        #endregion

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
                const string filter = "*";
                string selectedItem = string.Empty;
                string selectedSubItem = string.Empty;

                int index = 0;
                if (!add)
                {
                    // Edit
                    // Empty - return
                    if (listView.SelectedItems.Count == 0)
                    {
                        LocDB.Message("errSelectandEdit", "Please Select and then Edit", null, LocDB.MessageTypes.Error,
                        LocDB.MessageDefault.First);
                        return;
                    }
                    foreach (ListViewItem lvwSelectedItem in listView.SelectedItems)
                    {
                        selectedItem = lvwSelectedItem.Text;
                        index = lvwSelectedItem.Index;
                        if (selectedItem == "Lexicon")
                        {
                            var msg = new[] { "Lexicon Section" };
                            LocDB.Message("errCannotEdit", "You Cannot edit the Lexicon Section", msg, LocDB.MessageTypes.Error,
                            LocDB.MessageDefault.First);
                            return;
                        }

                        if (lvwSelectedItem.SubItems.Count > 1)
                        {
                            selectedSubItem = lvwSelectedItem.SubItems[1].Text;
                        }
                    }
                }

                var pf = new PopupForm(selectedItem, selectedSubItem, add, filter, fromSection);
                if (pf.ShowDialog() == DialogResult.OK)
                {
                    string returnName;

                    if (pf.FileName == string.Empty)
                    {
                        returnName = "Blank";
                        AddEditListViewItems(returnName, fromSection, index, "", add);
                    }
                    else
                    {
                        returnName = pf.SectionName;
                        if (returnName != null)
                        {
                            string returnFileName = pf.FileName;
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
            }
        }

        #endregion AddEditSectionStep

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
            }
        }

        #endregion

        #region tsDocumentSectionsDelete_Click
        private void tsDocumentSectionsDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvwDocumentSection.SelectedItems.Count < 0)
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
                        LocDB.Message("errDellFile", "You Cannot Delete the Lexicon Section", msg, LocDB.MessageTypes.Error,
                        LocDB.MessageDefault.First);
                        return;
                    }
                }
                lvwDocumentSection.Items.RemoveAt(index);

                lvwDocumentSteps.Items.Clear();

                if (_dictSectionFilenames.ContainsKey(selectedItem))
                {
                    _dictSectionFilenames.Remove(selectedItem);
                    if (_dictStepFilenames.ContainsKey(selectedItem))
                    {
                        _dictStepFilenames.Remove(selectedItem);
                    }
                }
                else if (_dictMainPrepStepsFilenames.ContainsKey(selectedItem))
                {
                    _dictMainPrepStepsFilenames.Remove(selectedItem);
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }
        #endregion

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
            }
        }
        #endregion

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
            }
        }
        #endregion

        #region tsDocumentStepsAdd_Click
        private void tsDocumentStepsAdd_Click(object sender, EventArgs e)
        {
            try
            {
                AddEditSectionStep(false, true, lvwDocumentSteps);
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }
        #endregion

        #region tsDocumentStepsEdit_Click
        private void tsDocumentStepsEdit_Click(object sender, EventArgs e)
        {
            try
            {
                AddEditSectionStep(false, false, lvwDocumentSteps);
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }
        #endregion

        #region tsDocumentStepsDelete_Click
        private void tsDocumentStepsDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvwDocumentSteps.SelectedItems.Count < 0)
                {
                    return;
                }
                string selectedItem = string.Empty;
                int index = 0;
                foreach (ListViewItem lvwSelectedItem in lvwDocumentSteps.SelectedItems)
                {
                    selectedItem = lvwSelectedItem.Text;
                    index = lvwSelectedItem.Index;
                }
                lvwDocumentSteps.Items.RemoveAt(index);

                string selectedItemMain = string.Empty;
                foreach (ListViewItem lvwSelectedItem in lvwDocumentSection.SelectedItems)
                {
                    selectedItemMain = lvwSelectedItem.Text;
                }

                if (selectedItemMain == "Lexicon")
                {
                    if (_dictMainPrepStepsFilenames.ContainsKey(selectedItem))
                    {
                        _dictMainPrepStepsFilenames.Remove(selectedItem);
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
            }
        }
        #endregion

        #region tsDocumentStepsUp_Click
        private void tsDocumentStepsUp_Click(object sender, EventArgs e)
        {
            try
            {
                _commonSettings.SwapListView(lvwDocumentSteps, "UP");
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }
        #endregion

        #region tsDocumentStepsDown_Click
        private void tsDocumentStepsDown_Click(object sender, EventArgs e)
        {
            try
            {
                _commonSettings.SwapListView(lvwDocumentSteps, "DOWN");
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }
        #endregion

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

                        var tempListView = new ListViewItem { Text = keyName };
                        tempListView.SubItems.Add(keyValue);
                        lvwDocumentSection.Items.Add(tempListView);
                    }
                    else
                    {
                        if (lvwDocumentSection.Text == "Lexicon")
                        {
                            _dictMainPrepStepsFilenames[keyName] = keyValue;
                        }
                        else
                        {
                            string selectedItem = string.Empty;
                            foreach (ListViewItem lvwSelectedItem in lvwDocumentSection.SelectedItems)
                            {
                                selectedItem = lvwSelectedItem.Text;
                            }

                            var tempDict = new Dictionary<string, string>();
                            if (_dictStepFilenames.ContainsKey(selectedItem))
                            {
                                tempDict = _dictStepFilenames[selectedItem];
                            }
                            tempDict[keyName] = keyValue;
                            _dictStepFilenames[selectedItem] = tempDict;
                            var tempListView = new ListViewItem { Text = keyName };
                            tempListView.SubItems.Add(keyValue);
                            lvwDocumentSteps.Items.Add(tempListView);
                        }
                    }
                }
                else
                {
                    // Edit
                    if (fromSection)
                    {
                        _dictSectionFilenames[keyName] = keyValue;

                        var tempListView = new ListViewItem { Text = keyName };
                        tempListView.SubItems.Add(keyValue);
                        lvwDocumentSection.Items.RemoveAt(keyIndex);
                        lvwDocumentSection.Items.Insert(keyIndex, tempListView);
                    }
                    else
                    {
                        if (lvwDocumentSection.Text == "Lexicon")
                        {
                            _dictMainPrepStepsFilenames[keyName] = keyValue;
                        }
                        else
                        {
                            var tempDict = new Dictionary<string, string>();
                            tempDict[keyName] = keyValue;
                            _dictStepFilenames[lvwDocumentSection.Text] = tempDict;

                            var tempListView = new ListViewItem { Text = keyName };
                            tempListView.SubItems.Add(keyValue);
                            lvwDocumentSteps.Items.RemoveAt(keyIndex);
                            lvwDocumentSteps.Items.Insert(keyIndex, tempListView);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }
        #endregion AddEditListViewItems

        #region lvwDocumentSection_MouseClick

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvwDocumentSection_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (lvwDocumentSection.SelectedItems.Count <= 0)
                {
                    return;
                }
                string selectedItem = string.Empty;
                foreach (ListViewItem lvwSelectedItem in lvwDocumentSection.SelectedItems)
                {
                    selectedItem = lvwSelectedItem.Text.Trim();
                }

                lvwDocumentSteps.Items.Clear();
                if (selectedItem == "Lexicon")
                {
                    ReadPreparationTabSteps("LIFT");
                }
                else
                {
                    ReadPreparationTabSteps(selectedItem);
                }
                if (lvwDocumentSteps.Items.Count == 0)
                {
                    if (_dictStepFilenames.ContainsKey(selectedItem))
                    {
                        Dictionary<string, string> tempDict = _dictStepFilenames[selectedItem];
                        foreach (KeyValuePair<string, string> tempString in tempDict)
                        {
                            var tempListView = new ListViewItem { Text = tempString.Key };
                            tempListView.SubItems.Add(tempString.Value);
                            lvwDocumentSteps.Items.Add(tempListView);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }
        #endregion

        #region ReadPreparationTabSteps
        /// <summary>
        /// Reads the SectionTypes.xml and sets the xml type
        /// </summary>
        /// <param name="section"></param>
        private void ReadPreparationTabSteps(string section)
        {
            try
            {
                var projXml = new XmlDocument();
                string sectionXmlPath = Common.GetPSApplicationPath() + "/SectionTypes.xml";
                if (!File.Exists(sectionXmlPath))
                {
                    var msg = new[] { "SectionTypes.xml" };
                    LocDB.Message("errInstallFile", "Please Install the SectionTypes.xml", msg, LocDB.MessageTypes.Error,
                    LocDB.MessageDefault.First);
                    return;
                }
                projXml.Load(sectionXmlPath);
                XmlElement root = projXml.DocumentElement;
                string XPath = "xmlType[Description='" + section + "']";
                string dtdPath = Common.GetPSApplicationPath() + "/"; // Sil\DictionaryExpress Path
                if (root != null)
                {
                    XmlNode tempNodeSearch = root.SelectSingleNode(XPath);

                    if (tempNodeSearch != null)
                    {
                        foreach (XmlNode node in tempNodeSearch)
                        {
                            if (node.Name == "step")
                            {
                                XmlAttribute xa = node.Attributes["type"];
                                if (xa != null)
                                {
                                    string stepFileType = xa.Value;
                                    if (stepFileType != null)
                                    {
                                        try
                                        {
                                            string tempKey = node.FirstChild.InnerText;
                                            string tempValue = node.FirstChild.NextSibling.InnerText;
                                            var tempItem = new ListViewItem(tempKey);
                                            tempItem.SubItems.Add(tempValue);
                                            lvwDocumentSteps.Items.Add(tempItem);

                                            tempValue = dtdPath + tempValue;
                                            //if (Section == "Lexicon" || Section == "LIFT")
                                            if (section == "Lexicon" || section == "LIFT")
                                            {
                                                _dictMainPrepStepsFilenames[tempKey] = tempValue;
                                            }
                                            else
                                            {
                                                var tempDict = new Dictionary<string, string>();
                                                tempDict[tempKey] = tempValue;
                                                _dictStepFilenames[section] = tempDict;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            var msg = new[] { ex.Message };
                                            LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                                            LocDB.MessageDefault.First);
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
                                _fileTypeFilter[section] = fileType;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }
        #endregion ReadPreparationTabSteps

        #region MakeDocumentPreparation
        /// <summary>
        /// Reads the data from Dictionary and Writes into CSS File
        /// </summary>
        private void MakeDocumentPreparation()
        {
            try
            {
                var projXml = new XmlDocument();
                XmlNode sectionsNode;
                XmlNode sectionNode;
                XmlNode stepNode;
                XmlNode tempNodeSearch = null;
                XmlAttribute Xa;

                projXml.Load(_solutionName);
                XmlElement root = projXml.DocumentElement;

                const string XPath = "DocumentSettings";
                if (root != null) tempNodeSearch = root.SelectSingleNode(XPath);   // Default to Document Settings
                if (tempNodeSearch != null)
                {
                    tempNodeSearch.RemoveAll();
                    sectionsNode = projXml.CreateNode("element", "Sections", "");

                    for (int i = 0; i < lvwDocumentSection.Items.Count; i++)
                    {
                        string key = lvwDocumentSection.Items[i].Text;

                        if (key == "Lexicon")
                        {
                            sectionNode = projXml.CreateNode("element", "Section", "");
                            //attribute
                            Xa = projXml.CreateAttribute("No");
                            Xa.Value = (i + 1).ToString();
                            sectionNode.Attributes.Append(Xa);

                            Xa = projXml.CreateAttribute("Name");
                            Xa.Value = key;
                            sectionNode.Attributes.Append(Xa);

                            Xa = projXml.CreateAttribute("Value");
                            Xa.Value = Path.GetFileName(_xhtml);
                            sectionNode.Attributes.Append(Xa);

                            sectionsNode.AppendChild(sectionNode); // Section to Sections
                            tempNodeSearch.AppendChild(sectionsNode); // Sections to Document Settings.
                            // Steps writing
                            int suffixNo = 1;
                            foreach (KeyValuePair<string, string> mainKey in _dictMainPrepStepsFilenames)
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
                                sectionNode.AppendChild(stepNode); //Steps Node to Section Node
                            }
                        }
                        else
                        {
                            string fileName;
                            if (_dictSectionFilenames.ContainsKey(key))
                            {
                                fileName = _dictSectionFilenames[key];
                                if (fileName != "none")
                                {
                                    AddFileToSolutinExplorer(fileName, projXml);
                                }
                            }
                            else
                            {
                                fileName = "none";
                            }

                            sectionNode = projXml.CreateNode("element", "Section", "");
                            //attribute
                            Xa = projXml.CreateAttribute("No");
                            Xa.Value = (i + 1).ToString();
                            sectionNode.Attributes.Append(Xa);

                            Xa = projXml.CreateAttribute("Name");
                            Xa.Value = key;
                            sectionNode.Attributes.Append(Xa);

                            Xa = projXml.CreateAttribute("Value");
                            Xa.Value = Path.GetFileName(fileName);
                            sectionNode.Attributes.Append(Xa);

                            sectionsNode.AppendChild(sectionNode); // Section to Sections
                            tempNodeSearch.AppendChild(sectionsNode); // Sections to Document Settings.

                            //// ========================================================= //
                            //// XSLT - Make Steps Addings
                            //// ========================================================= //
                            if (_dictStepFilenames.ContainsKey(key))
                            {
                                Dictionary<string, string> tempDict = _dictStepFilenames[key];
                                int suffixNo = 1;

                                foreach (KeyValuePair<string, string> tempString in tempDict)
                                {
                                    string makeKey = tempString.Key;
                                    fileName = tempString.Value;
                                    if (fileName != "none")
                                    {
                                        AddFileToSolutinExplorer(fileName, projXml);
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
                                    sectionNode.AppendChild(stepNode); //Steps Node to Section Node
                                }
                            }
                        }
                    }
                    // Add DTD Files to Solution Explorer
                    string dtdPath = Common.GetPSApplicationPath() + "/";
                    foreach (string dtdFileName in _stepDtdFile)
                    {
                        if (dtdFileName.Length > 0)
                        {
                            AddFileToSolutinExplorer(Common.PathCombine(dtdPath, dtdFileName), projXml);
                        }
                    }
                    projXml.Save(_solutionName);
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }
        #endregion MakeDocumentPreparation

        #region GetPageSectionSteps
        /// <summary>
        /// Gets the Details from de file and stores in Dictionary
        /// </summary>
        /// <param name="lvwDocumentSection">Section Listview</param>
        /// <param name="lvwPreparationSteps">Steps Listview</param>
        // ReSharper disable ParameterHidesMember
        private void GetPageSectionSteps(ListView lvwDocumentSection, ListView lvwPreparationSteps)
        // ReSharper restore ParameterHidesMember
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

                var projXml = new XmlDocument();

                projXml.Load(_solutionName);
                XmlElement root = projXml.DocumentElement;

                const string XPath = "DocumentSettings/Sections";
                if (root != null)
                {
                    XmlNode tempNodeSearch = root.SelectSingleNode(XPath);
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
                                _dictSectionFilenames[attName] = Path.GetFileName(_xhtml); // default xhtml file
                                attValue = _dictSectionFilenames[attName];
                            }
                            else
                            {
                                _dictSectionFilenames[attName] = attValue;
                            }
                            var tempListView = new ListViewItem { Text = attName };
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
                                            attChildName = childNodes.ChildNodes[ii].Attributes[jj].Value;
                                        }
                                        else if (attChildString == "Value")
                                        {
                                            attChildValue = childNodes.ChildNodes[ii].Attributes[jj].Value;
                                        }
                                    }
                                    if (attName == "Lexicon")
                                    {
                                        _dictMainPrepStepsFilenames[attChildName] = attChildValue;
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
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }
        #endregion GetPageSectionSteps

        #region AddFileToSolutinExplorer
        /// <summary>
        /// Adds the FileName to Solution Explorer 
        /// Based on the prviously opened xmlDocument File.
        /// </summary>
        /// <param name="fileName">File Name</param>
        /// <param name="projXml">Project XML</param>
        /// <returns>Nothing</returns>
        public void AddFileToSolutinExplorer(string fileName, XmlDocument projXml)
        {
            try
            {
                string fn = Path.GetFileName(fileName);
                string fileNamePath = Common.PathCombine(_dicPath, Path.GetFileName(fileName));
                if (!File.Exists(fileNamePath))
                {
                    try
                    {
                        File.Copy(fileName, fileNamePath, true);
                    }
                    catch
                    {
                        return;
                    }

                    XmlElement root = projXml.DocumentElement;
                    XmlNode tempNode = projXml.CreateNode("element", "File", "");

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
                    if (root != null) root.FirstChild.AppendChild(tempNode);
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }
        #endregion AddFileToSolutinExplorer

        #region GetDefault
        /// <summary>
        /// GetDefault - Get default value from css
        /// </summary>
        /// <param name="xPath">string representation of xpath divided by slashes</param>
        /// <returns>array of values in css</returns>
        private string[] GetDefault(string xPath)
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
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
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
                TreeNode prop = null;
                if (cls.Parent.Parent != null)
                {
                    prop = DoXPath(cls.Parent.Parent, 0);
                }
                return AccumulateResult(prop);
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
            return null;
        }
        #endregion GetDefault

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
                    if (_xpath != null)
                        if (child.Text == _xpath[lev])
                        {
                            if (child.Text == "ATTRIB" && (child.FirstNode.Text == _xpath[3]))
                            {
                                if (child.LastNode.Text.Replace("\'", "") == _xpath[_xpath.Length - 1])
                                {
                                    return child;
                                }
                            }
                            else if (child.Text == "PSEUDO" && (child.FirstNode.Text == _xpath[2]))
                            {
                                if (child.NextNode.FirstNode.Text.Replace("\'", "") == _xpath[_xpath.Length - 1])
                                {
                                    return child.NextNode.FirstNode;
                                }
                            }
                            TreeNode res = DoXPath(child, lev + 1);
                            // for Sense Panel
                            if (res == null)
                            {
                                if (_xpath.Length == 5 && child.Text == _xpath[2]
                                     && child.Parent.NextNode != null && child.Parent.NextNode.Text == "PSEUDO")
                                {
                                    if (child.Parent.NextNode.FirstNode.Text == _xpath[_xpath.Length - 1])
                                    {
                                        res = child.Parent.NextNode.FirstNode;
                                    }
                                }
                                else if (_xpath.Length == 4
                                             && child.Parent.NextNode != null && child.Text == _xpath[2] &&
                                             child.Parent.NextNode.Text != "PSEUDO")
                                {
                                    if (_xpath[_xpath.Length - 1] == "END")
                                    {
                                        res = child;
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
                                    if (item.Text == "PROPERTY")
                                    {
                                        if (_xpath != null)
                                            if (item.FirstNode.Text == _xpath[1]
                                               || (_xpath.Length > 3 && node.FirstNode.FirstNode.Text == _xpath[2] && item.FirstNode.Text == _xpath[_xpath.Length - 1]))
                                            {
                                                return item.FirstNode;
                                            }
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
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
            return null;
        }

        private TreeNode DoXPath_New(TreeNode node, int lev)
        {
            try
            {
                if (lev == _xpath.Length)
                    return node;
                foreach (TreeNode child in node.Nodes)
                {
                    if (child.Text == _xpath[lev])
                    {
                        DoXPath(child, lev + 1);
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
            return null;
        }

        #endregion

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
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
            return null;
        }
        #endregion AccumulateResult

        #region ddlBasicsPageSize_TextChanged
        private void ddlBasicsPageSize_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _commonSettings.PageSizeChange(ddlBasicsPageSize, ddlBasicsPaperSize, chkBasicsCropMark, txtBasicsWidth, txtBasicsHeight);
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }
        #endregion

        #region ddlShowProperty_SelectedIndexChanged
        private void ddlShowProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                pnlFootnoteFrame.Enabled = true;
                pnlFootnotesRules.Enabled = true;
                pnlTextSpacingBetweenLetters.Enabled = true;
                pnlTextSpacingBetweenWords.Enabled = true;
                pnlTextSpacingKerning.Enabled = true;
                pnlOtherTypefaceNames.Visible = true;
                pnlOtherUltraXML.Visible = true;

                if (ddlShowProperty.Text == "OpenOffice Document")
                {
                    pnlFootnoteFrame.Enabled = false;
                    pnlFootnotesRules.Enabled = false;
                    pnlTextSpacingBetweenLetters.Enabled = false;
                    pnlTextSpacingBetweenWords.Enabled = false;
                    pnlTextSpacingKerning.Enabled = false;
                    pnlOtherTypefaceNames.Visible = false;
                    pnlOtherUltraXML.Visible = false;
                }
                else if (ddlShowProperty.Text == "PDF")
                {
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error,
                LocDB.MessageDefault.First);
            }
        }
        #endregion

        #region ValidatePageLocation
        private static void ValidatePageLocation(Control location1, Control location2)
        {
            string loc1 = location1.Text;
            string loc2 = location2.Text;
            if (loc1.Length > 0 && loc2.Length > 0)
            {
                if (string.Compare(loc1, loc2) == 0)
                {
                    var msg = new[] { "location"};
                    LocDB.Message("errCannotbeSame", "Reference and Page number location cannot be same. Please change.", msg, LocDB.MessageTypes.Error,
                    LocDB.MessageDefault.First);
                    location1.Focus();
                }
                else if (ValidatePosition(loc1, loc2))
                {
                    var msg = new[] { "position"};
                    LocDB.Message("errCannotbeSame", "Reference and Page number position cannot be same. Please change.", msg, LocDB.MessageTypes.Error,
                    LocDB.MessageDefault.First);
                    location1.Focus();
                }
            }
        }

        private static bool ValidatePosition(string location1, string location2)
        {
            if (location1 == "top-left" && location2 == "top-inside")
            {
                return true;
            }
            if (location1 == "top-right" && location2 == "top-inside")
            {
                return true;
            }
            return false;
        }

        #endregion

        #region ddlOtherPageNumberLocation_SelectedIndexChanged
        /// <summary>
        /// To validate the reference and page number locations when PagenumberLocation combobox index changed.
        /// </summary>
        /// <param name="sender">Combo Box</param>
        /// <param name="e">Event Args</param>
        private void ddlOtherPageNumberLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsLoaded)
            {
                ValidatePageLocation(ddlOtherPageNumberLocation, ddlOtherReferenceLocation);
                txtOtherNonConsecutiveReferenceSeparator.Focus();
                txtOtherConsecutiveReferenceSeparator.Focus();
                txtOtherChapterVerseSeparator.Focus();
                txtOtherFontSize.Focus();
                ddlOtherReferenceLocation.Focus();

            }
            FillReferenceFormat();
        }

        private void FillReferenceFormat()
        {
            if (ddlOtherPageNumberLocation.Text.IndexOf("outside") > 0 || ddlOtherPageNumberLocation.Text.IndexOf("inside") > 0 || ddlOtherReferenceLocation.Text.IndexOf("outside") > 0)
            {
                LoadReferenceFormat(_mirrorPage);
            }
            else
            {
                LoadReferenceFormat(_singlePage);
            }
        }

        #endregion

        #region ddlOtherReferenceLocation_SelectedIndexChanged
        /// <summary>
        /// To validate the reference and page number locations when ReferenceLocation combobox index changed.
        /// </summary>
        /// <param name="sender">Combo Box</param>
        /// <param name="e">Event Args</param>
        private void ddlOtherReferenceLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsLoaded)
            {
                ValidatePageLocation(ddlOtherReferenceLocation, ddlOtherPageNumberLocation);
                txtOtherNonConsecutiveReferenceSeparator.Focus();
                txtOtherConsecutiveReferenceSeparator.Focus();
                txtOtherChapterVerseSeparator.Focus();
                txtOtherFontSize.Focus();
                ddlOtherPageNumberLocation.Focus();
            }
            FillReferenceFormat();
        }
        #endregion

        private void txtOtherFontSize_Leave(object sender, EventArgs e)
        {
            ddlOtherReferenceLocation.Focus();
        }

        private void ddlBasicsColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            _commonSettings.DisableColumnControls(ddlBasicsColumns.Text, txtBasicsGutterWidth, chkBasicsVerticalRule);
        }

        private void tabScriptureSettings_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabScriptureSettings.SelectedIndex == 0)
            {
                ShowHelp.ShowHelpTopic(this, "DocumentTab.htm", Common.IsUnixOS(), false);
            }
            else if (tabScriptureSettings.SelectedIndex == 1)
            {
                ShowHelp.ShowHelpTopic(this, "BasicsTab.htm", Common.IsUnixOS(), false);
            }
            else if (tabScriptureSettings.SelectedIndex == 2)
            {
                ShowHelp.ShowHelpTopic(this, "ChapterVerseTab.htm", Common.IsUnixOS(), false);
            }
            else if (tabScriptureSettings.SelectedIndex == 3)
            {
                ShowHelp.ShowHelpTopic(this, "HeadingsTab.htm", Common.IsUnixOS(), false);
            }
            else if (tabScriptureSettings.SelectedIndex == 4)
            {
                ShowHelp.ShowHelpTopic(this, "FootnotesTab.htm", Common.IsUnixOS(), false);
            }
            else if (tabScriptureSettings.SelectedIndex == 5)
            {
                ShowHelp.ShowHelpTopic(this, "TextSpacingTab.htm", Common.IsUnixOS(), false);
            }
            else if (tabScriptureSettings.SelectedIndex == 6)
            {
                ShowHelp.ShowHelpTopic(this, "OtherTab.htm", Common.IsUnixOS(), false);
            }
        }

        private void ScriptureSetting_DoubleClick(object sender, EventArgs e)
        {
#if DEBUG
            var dlg = new Localizer(LocDB.DB);
            dlg.ShowDialog();
#endif
        }

        private void ScriptureSetting_Activated(object sender, EventArgs e)
        {
            Common.SetFont(this);
        }
    }
}
