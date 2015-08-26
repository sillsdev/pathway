using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using L10NSharp;
using SIL.PublishingSolution.Properties;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public partial class UILanguageDialog : Form
    {
        public ConfigurationTool CTool = new ConfigurationTool();
        public ConfigurationToolBL CToolBl = new ConfigurationToolBL();
        private string _uiLanguage = "en";
        private string _fontName = "Microsoft Sans Serif";
        private string _fontSize = "8.25";

        public UILanguageDialog()
        {
            InitializeComponent();
        }


        private void LoadUiLanguages()
        {
            ddlUILanguage.Items.Clear();
            foreach (var lang in LocalizationManager.GetUILanguages(true))
            {
                var item = new ComboBoxItem(lang.Name,lang.NativeName);
                ddlUILanguage.Items.Add(item);
            }
            ddlUILanguage.SelectedItem =
                ddlUILanguage.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == _uiLanguage);
        }

        private void LoadInstalledFonts()
        {
            ddlFontName.Items.Clear();
            foreach (FontFamily font in FontFamily.Families)
            {
                ddlFontName.Items.Add(font.Name);
                ddlFontName.SelectedIndex = 0;
            }
            ddlFontName.SelectedItem = _fontName;
        }

        private void LoadFontSize()
        {
            ddlFontSize.Items.Clear();
            ddlFontSize.Items.Add("7");
            ddlFontSize.Items.Add("8");
            ddlFontSize.Items.Add("9");
            ddlFontSize.Items.Add("10");
            ddlFontSize.Items.Add("11");
            ddlFontSize.Items.Add("12");
            ddlFontSize.Items.Add("14");
            ddlFontSize.Items.Add("16");
            ddlFontSize.Items.Add("18");
            ddlFontSize.Items.Add("20");
            ddlFontSize.Items.Add("22");
            ddlFontSize.Items.Add("24");
            ddlFontSize.Items.Add("26");
            ddlFontSize.Items.Add("28");
            ddlFontSize.Items.Add("36"); 
            ddlFontSize.Items.Add("48"); 
            ddlFontSize.Items.Add("72");
            ddlFontSize.SelectedItem = _fontSize;
        }

        private void UILanguageDialog_Load(object sender, EventArgs e)
        {
            string fileName = Common.PathCombine(Common.GetAllUserAppPath(), @"SIL\Pathway\UserInterfaceLanguage.xml");
            var xd = new XmlDocument();
            xd.Load(fileName);
            var selectSingleNode = xd.SelectSingleNode("//UILanguage/string");
            if (selectSingleNode != null)
                _uiLanguage = selectSingleNode.InnerText;
            var singleNode = xd.SelectSingleNode("//UILanguage/fontstyle/font[@lang='" + _uiLanguage + "']/@name");
            if (singleNode != null)
                _fontName = singleNode.InnerText;
            var xmlNode = xd.SelectSingleNode("//UILanguage/fontstyle/font[@lang='" + _uiLanguage + "']/@size");
            if (xmlNode != null)
                _fontSize = xmlNode.InnerText;

            LoadUiLanguages();
            LoadInstalledFonts();
            LoadFontSize();
            UpdateFontOnL10NSharp(_uiLanguage);

            rtbPreview.Text = ddlUILanguage.SelectedItem.ToString();
            //rtbPreview.Font = new Font(ddlFontName.SelectedItem.ToString(),
              //  float.Parse(ddlFontSize.SelectedItem.ToString(), CultureInfo.InvariantCulture.NumberFormat));
        }

        private void ddlFontName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFontSize.SelectedItem != null)
                rtbPreview.Font = new Font(ddlFontName.SelectedItem.ToString(),
                    float.Parse(ddlFontSize.SelectedItem.ToString(), CultureInfo.InvariantCulture.NumberFormat));
        }

        private void ddlFontSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFontName.SelectedItem != null)
                rtbPreview.Font = new Font(ddlFontName.SelectedItem.ToString(),
                    float.Parse(ddlFontSize.SelectedItem.ToString(), CultureInfo.InvariantCulture.NumberFormat));
        }

        private void ddlUILanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            rtbPreview.Text = ddlUILanguage.SelectedItem.ToString();
            if (ddlFontName.SelectedItem != null && ddlFontSize.SelectedItem != null)
                rtbPreview.Font = new Font(ddlFontName.SelectedItem.ToString(),
                    float.Parse(ddlFontSize.SelectedItem.ToString(), CultureInfo.InvariantCulture.NumberFormat));
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowL10NsharpDlg();
            this.Close();
        }

        public static void ShowL10NsharpDlg()
        {
            Common.L10NMngr.ShowLocalizationDialogBox(false);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var lang = ((ComboBoxItem) ddlUILanguage.SelectedItem).Value;
            LocalizationManager.SetUILanguage(lang, true);
            Settings.Default.UserInterfaceLanguage = lang;
            LocalizationManager.ReapplyLocalizationsToAllObjects(Common.L10NMngr.Id);
            Common.SaveLocalizationSettings(Settings.Default.UserInterfaceLanguage, ddlFontName.SelectedItem.ToString(), ddlFontSize.SelectedItem.ToString());
            this.Close();
        }

        private void ddlUILanguage_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            var lang = ((ComboBoxItem)ddlUILanguage.SelectedItem).Value;
            rtbPreview.Text = ddlUILanguage.SelectedItem.ToString();
            string fileName = Common.PathCombine(Common.GetAllUserAppPath(), @"SIL\Pathway\UserInterfaceLanguage.xml");
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(fileName);
            var fontNameNode = xmlDoc.SelectSingleNode("//UILanguage/fontstyle/font[@lang='" + lang + "']/@name");
            if (fontNameNode != null)
                _fontName = fontNameNode.InnerText;
            var fontSizeNode = xmlDoc.SelectSingleNode("//UILanguage/fontstyle/font[@lang='" + lang + "']/@size");
            if (fontSizeNode != null)
                _fontSize = fontSizeNode.InnerText;

            ddlFontName.SelectedItem = _fontName;
            ddlFontSize.SelectedItem = _fontSize;
        }

        /// <summary>
        /// To set the font-name and font-size to the controls in the form.
        /// </summary>
        /// <param name="langId"></param>
        public void UpdateFontOnL10NSharp(string langId)
        {
            string fontName = "Microsoft Sans Serif";
            float fontSize = 8.25F;

            fontName = _fontName;
            fontSize = float.Parse(_fontSize);

            //For all labels and textboxes
            List<Control> allControls = GetAllControls(this);
            allControls.ForEach(k => k.Font = new Font(fontName, fontSize));
        }


        /// <summary>
        /// Get the list of controls in the form
        /// </summary>
        /// <param name="container"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<Control> GetAllControls(Control container, List<Control> list)
        {
            foreach (Control c in container.Controls)
            {
                if (c.Controls.Count > 0)
                    list = GetAllControls(c, list);
                else
                {
                    if (c.Name != "ddlFontName" && c.Name != "ddlFontSize" && c.Name != "ddlUILanguage" && c.Name != "linkLabel1")
                        list.Add(c);
                }
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        private List<Control> GetAllControls(Control container)
        {
            return GetAllControls(container, new List<Control>());
        }
    }
}
