// --------------------------------------------------------------------------------------------
// <copyright file="LocalizationSetup.cs" from='2009' to='2009' company='SIL International'>
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
// Language setup screen for localization
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using JWTools;
using SIL.Tool;
using SIL.Tool.Localization;

namespace SIL.PublishingSolution
{
    #region Class LocalizationSetup
    public partial class LocalizationSetup : Form
    {
        private string LanguageAtLoadingTime;
        private Font FontAtLoadingTime;
        public bool Localization = false;
        #region Constructor
        public LocalizationSetup()
        {
            InitializeComponent();
        }
        #endregion

        #region Private Functions
        private void LocalizationSetup_Load(object sender, EventArgs e)
        {
            LocDB.Localize(this, null);  // Localization
            cmbLanguages.Items.Clear();
            foreach (LocLanguage lang in LocDB.DB.Languages)
            {
                cmbLanguages.Items.Add(lang.Name);
            }
            cmbLanguages.Items.Add("English");

            CmbFontName.Items.Clear();
            var fonts = new InstalledFontCollection();
            for (int i = 0; i < fonts.Families.Length; i++)
            {
                String test = fonts.Families[i].Name;
                CmbFontName.Items.Add(test);
            }

            for (int i = 8; i <= 12; i++)
            {
                CmbFontSize.Items.Add(i);
            }
            CmbFontSize.Items.Insert(1,"8.25");
            LocLanguage[] lang1 = LocDB.DB.Languages;
            if(LocDB.DB.PrimaryIsEnglish)
            {
                cmbLanguages.Text = "English";
            }
            else
            {
                string primaryLang = LocDB.DB.PrimaryLanguage.Name;
                cmbLanguages.Text = primaryLang;
                
            }
            LanguageAtLoadingTime = cmbLanguages.Text; // If font not applied then set to default
            FontAtLoadingTime = Common.UIFont;
            CmbFontName.Text = Common.UIFont.Name;
            CmbFontSize.Text = Common.UIFont.Size.ToString();

            Common.SetFont(this);
        }


        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (cmbLanguages.Text.ToLower() == "english")
            {
                Param.SaveFontNameSize(CmbFontName.Text, CmbFontSize.Text);
                Param.SetFontNameSize(); // Global Font Name and Size for all UI Forms
            }
            else
            {
                Font newFont = new Font(CmbFontName.Text, float.Parse(CmbFontSize.Text));
                SetFontNameSizeLoc(newFont, cmbLanguages.Text);
                GetFontNameSizeFromLoc(cmbLanguages.Text);
            }
            Localization = true;
            Close();
        }
        #endregion

        private void LocalizationSetup_DoubleClick(object sender, EventArgs e)
        {
#if DEBUG
            var dlg = new Localizer(LocDB.DB);
            dlg.ShowDialog();
#endif
        }

        private void CmdClose_Click(object sender, EventArgs e)
        {
            Common.UIFont = FontAtLoadingTime;
            LocDB.DB.SetPrimary(LanguageAtLoadingTime); // If font not applied then set to default
            this.Close();
        }

        private void CmbFontName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplySetting();
        }

        private void ApplySetting()
        {
            try
            {
                if (CmbFontSize.Text.Length > 0 && CmbFontName.Text.Length > 0)
                {
                    string fontName = CmbFontName.Text;
                    float fontSize = float.Parse(CmbFontSize.Text);
                    var myFont = new Font(fontName, fontSize);

                    foreach (Control ctl in this.Controls)
                    {
                        if (ctl is Label || ctl is Button)
                        {
                            ctl.Font = myFont;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                
                Console.Write(ex.Message);
            }
        }

        private void CmbFontSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplySetting();
        }

        private void cmbLanguages_SelectedValueChanged(object sender, EventArgs e)
        {
            string lang = cmbLanguages.Text;
            if (lang == "English")
            {
                lang = null;
                Param.SetFontNameSize();
            }
            else
            {
                GetFontNameSizeFromLoc(lang);
            }
            LocDB.DB.SetPrimary(lang);
            Control[] ctl = new[] {cmbLanguages};
            LocDB.Localize(this, ctl);

            Common.SetFont(this); // Cmblanguage related
            CmbFontName.Text = Common.UIFont.Name;
            CmbFontSize.Text = Common.UIFont.Size.ToString();

        }
        /// <summary>
        /// Gets the Gloabal Font Size and Font Name from styleSettings.xml
        /// </summary>
        public static void GetFontNameSizeFromLoc(string language)
        {
            LocLanguage langID = LocDB.DB.FindLanguageByName(language);
            XmlDocument xmlDocument = new XmlDocument { XmlResolver = null };
            string folderPath = Common.PathCombine(Common.GetAllUserPath(), "Loc");

            var localPath = "";
            if (language == "English")
            {
                localPath = Common.PathCombine(folderPath, "Pslocalization.xml");
            }
            else
            {
                localPath = Common.PathCombine(folderPath, langID.ID + ".xml");
            }
            if (!File.Exists(localPath)) return;

            xmlDocument.Load(localPath);
            XmlNode root = xmlDocument.DocumentElement;
            string xPath = "//Language";
            XmlNode returnNode = root.SelectSingleNode(xPath);
            if (returnNode == null)
            {
                Common.UIFont = new Font("Times New Roman", 10); // TODO Greg Clarification.
            }
            else
            {
                string fontName = returnNode.Attributes.GetNamedItem("Font").Value;
                string strFontSize = returnNode.Attributes.GetNamedItem("Size").Value;
                float fontSize = float.Parse(strFontSize);
                Common.UIFont = new Font(fontName, fontSize);
            }
        }

        /// <summary>
        /// Gets the Gloabal Font Size and Font Name from styleSettings.xml
        /// </summary>
        public static void SetFontNameSizeLoc(Font newFont, string language)
        {
            LocLanguage langID = LocDB.DB.FindLanguageByName(language);
            XmlDocument xmlDocument = new XmlDocument { XmlResolver = null };
            string folderPath = Common.PathCombine(Common.GetAllUserPath(), "Loc");

            var localPath = Common.PathCombine(folderPath, langID.ID + ".xml");

            if (!File.Exists(localPath)) return;

            xmlDocument.Load(localPath);
            XmlNode root = xmlDocument.DocumentElement;
            string xPath = "//Language";
            XmlNode returnNode = root.SelectSingleNode(xPath);
            if (returnNode == null)
            {
                Common.UIFont = new Font("Times New Roman", 10); // TODO Greg Clarification.
            }
            else
            {
                XmlNode fontName = returnNode.Attributes.GetNamedItem("Font");
                fontName.Value = newFont.Name;
                XmlNode strFontSize = returnNode.Attributes.GetNamedItem("Size");
                strFontSize.Value = newFont.Size.ToString();
                xmlDocument.Save(localPath);
            }
        }
    }
    #endregion
}
