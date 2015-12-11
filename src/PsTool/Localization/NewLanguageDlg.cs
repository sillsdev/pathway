/**********************************************************************************************
 * Dll:     JWTools
 * File:    NewLanguageDlg.cs
 * Author:  John Wimbish
 * Created: 08 Dec 2007
 * Purpose: Localization tool.
 * Legal:   Copyright (c) 2005-08, John S. Wimbish. All Rights Reserved.  
 *********************************************************************************************/
#region Using
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using SIL.Tool.Localization;
#endregion

namespace JWTools
{
    public partial class DlgNewLanguage : Form
    {
        // Public Attrs ----------------------------------------------------------------------
        #region Attr{g}: int FontSize
        public int FontSize
        {
            get
            {
                string s = ComboFontSize.Text.Trim();
                if (string.IsNullOrEmpty(s))
                    return 0;

                try
                {
                    return Convert.ToInt16(s);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }
        #endregion
        #region Attr{g}: string FontName
        public string FontName
        {
            get
            {
                return ComboFontName.Text.Trim();
            }
        }
        #endregion
        #region Attr{g}: string Abbreviation
        public string Abbreviation
        {
            get
            {
                return m_textAbbrev.Text.ToLower().Trim();
            }
        }
        #endregion
        #region Attr{g}: string LanguageName
        public string LanguageName
        {
            get
            {
                return m_textName.Text.Trim();
            }
        }
        #endregion

        // Controls --------------------------------------------------------------------------
        #region Attr{g}: ComboBox ComboFontName
        ComboBox ComboFontName
        {
            get
            {
                Debug.Assert(null != m_comboFontName);
                return m_comboFontName;
            }
        }
        #endregion
        #region Attr{g}: ComboBox ComboFontSize
        ComboBox ComboFontSize
        {
            get
            {
                Debug.Assert(null != m_comboFontSize);
                return m_comboFontSize;
            }
        }
        #endregion
        #region Attr{g}: string AbbrevError
        string AbbrevError
        {
            set
            {
                m_lblAbbrevError.Text = value;
            }
        }
        #endregion
        #region Attr{g}: string NameError
        string NameError
        {
            set
            {
                m_lblNameError.Text = value;
            }
        }
        #endregion

        // Scaffolding -----------------------------------------------------------------------
        #region Constructor()
        public DlgNewLanguage()
        {
            InitializeComponent();
        }
        #endregion
        bool m_bPropertiesMode = false;
        #region Method: void SetAsPropertiesMode(string sAbbreviation, string sLanguageName)
        public void SetAsPropertiesMode(string sAbbreviation, string sLanguageName)
        {
            Text = ("Properties for " + sLanguageName);

            m_textAbbrev.Text = sAbbreviation;
            m_textName.Text = sLanguageName;

            m_btnOK.Text = "OK";

            m_bPropertiesMode = true;
        }
        #endregion

        // Events ----------------------------------------------------------------------------
        #region Event: cmdLoad
        private void cmdLoad(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            // The list of Font Names
            string[] vN = new string[FontFamily.Families.Length];
            for (int i = 0; i < FontFamily.Families.Length; i++)
                vN[i] = FontFamily.Families[i].Name;
            ComboFontName.Items.Clear();
            ComboFontName.Items.Add("");
            ComboFontName.Items.AddRange(vN);

            // The list of Font Sizes
            int[] vnS = new int[] { 8, 10, 11, 12, 13, 14, 15, 16, 17, 
                18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 
                34, 38, 42, 46, 50 };
            string[] vsS = new string[vnS.Length];
            for (int i = 0; i < vnS.Length; i++)
                vsS[i] = vnS[i].ToString();
            ComboFontSize.Items.Clear();
            ComboFontSize.Items.Add("");
            ComboFontSize.Items.AddRange(vsS);

            Cursor = Cursors.Default;
        }
        #endregion
        #region Event: cmdClosing
        private void cmdClosing(object sender, FormClosingEventArgs e)
        {
            // Allow the person to cancel if that is what they wish (That is, don't
            // do error checking which would make it impossible to cancel the dialog!)
            if (DialogResult.Cancel == DialogResult)
                return;

            // We must have an abbreviation
            if (string.IsNullOrEmpty(Abbreviation))
            {
                AbbrevError = "Please supply an abbreviation";
                e.Cancel = true;
                return;
            }

            // The abbreviation cannot conflict with anything else in the LocDb
            LocLanguage language = LocDB.DB.FindLanguage(Abbreviation);
            if (!m_bPropertiesMode && null != language)
            {
                AbbrevError = "This abbreviation is already taken; please choose another.";
                e.Cancel = true;
                return;
            }

            // We must have a language name
            if (string.IsNullOrEmpty(LanguageName))
            {
                NameError = "Please supply a language name";
                e.Cancel = true;
                return;
            }

            // The name cannot conflict with anything else in the LocDb
            language = LocDB.DB.FindLanguageByName(LanguageName);
            if (!m_bPropertiesMode && null != language)
            {
                NameError = "This name is already taken; please choose another.";
                e.Cancel = true;
                return;
            }

            e.Cancel = false;
        }
        #endregion
        #region Event: cmdAbbrevChanged - erase any AbbrevError message
        private void cmdAbbrevChanged(object sender, EventArgs e)
        {
            AbbrevError = "";
        }
        #endregion
        #region Event: cmdNameChanged - erase any NameError message
        private void cmdNameChanged(object sender, EventArgs e)
        {
            NameError = "";
        }
        #endregion

        private void m_btnOK_Click(object sender, EventArgs e)
        {

        }
    }
}