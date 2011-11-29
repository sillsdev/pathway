// --------------------------------------------------------------------------------------
// <copyright file="FontWarningDialog.cs" from='2009' to='2011' company='SIL International'>
//      Copyright © 2010, 2011 SIL International. All Rights Reserved.
//
//      Distributable under the terms specified in the LICENSING.txt file.
// </copyright>
// <author>Erik Brommers</author>
// <email>erik_brommers@sil.org</email>
// Last reviewed:
// 
// <remarks>
// Font Warning Dialog / Missing font dialog (see below)
//
// Font Warning dialog
// The .epub spec allows us to embed fonts in the document itself, to support code points
// that e-book readers don't normally support. In order to embed these fonts, users need
// to have the right to redistribute them.
//
// This dialog is displayed when the Pathway epub export can't find the proper rights
// within the font file (i.e., the font file does not say that it is redistributable
// under the OFL or GPL license, nor does it say that it was made by SIL).
// 
// This does NOT mean that the file cannot be redistributed. The user might have created
// the font themselves, or they might have been granted the right to redistribute the
// font -- we don't know. So this dialog is displayed to ask them what to do. The user
// has the following options:
// - Embed the font
// - Substitute a font that is freely redistributable
// - Cancel the export
// --------------------------
// Missing font dialog
// This dialog also is displayed -- with some different text -- when a font is missing
// from the system. This can happen if the user running Pathway is not the user who 
// created the document (e.g., a publication coordinator for a language area). In this 
// case, the user has two options:
// - Substitute a font that is freely redistributable
// - Cancel the export
// (they can't embed the font since it's not on their system.)
//
// </remarks>
// --------------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using epubConvert.Properties;
using SIL.Tool;

namespace epubConvert
{
    public partial class FontWarningDlg : Form
    {
        // properties
        /// <summary>
        /// The font that we're asking the user about (missing / license warning)
        /// </summary>
        public string MyEmbeddedFont { get; set; }
        /// <summary>
        /// The font that the user chose to replace the current problematic font.
        /// </summary>
        public string SelectedFont { get; set; }
        /// <summary>
        /// Returns the number of remaining font warnings that need to be worked through
        /// </summary>
        public int RemainingIssues { get; set; }
        /// <summary>
        /// The languages that are using this font
        /// </summary>
        public string Languages { get; set; }
        /// <summary>
        /// Did the user click the "do this for the rest of the issues" checkbox?
        /// </summary>
        public bool RepeatAction { get; set; }
        /// <summary>
        /// Embed the font anyway (not available if the font is missing)
        /// </summary>
        /// <returns></returns>
        public bool UseFontAnyway()
        {
            return (!rdoEmbedFont.Visible) ? false : rdoEmbedFont.Checked;
        }

        // variables
        private PrivateFontCollection pfc = new PrivateFontCollection();
        private string[] _silFonts;

        // methods
        /// <summary>
        /// Constructor.
        /// </summary>
        public FontWarningDlg()
        {
            InitializeComponent();
        }
        
        // display warning dialog
        private void FontWarningDlg_Load(object sender, EventArgs e)
        {
            icnWarning.Image = SystemIcons.Warning.ToBitmap();
            EmbeddedFont curfont = new EmbeddedFont(MyEmbeddedFont);
            if (curfont.Filename == null)
            {
                // show "missing font" UI (only option is to substitute)
                Text = String.Format(Resources.MissingFontTitle, MyEmbeddedFont);
                txtWarning.Text = String.Format(Resources.MissingFontWarning, MyEmbeddedFont, Languages);
                grpOptions.Text = "";
                rdoEmbedFont.Visible = false;
                rdoConvertToSILFont.Visible = false;
                lblSubstituteSILFont.Visible = true;
                lblSubstituteSILFont.Text = Resources.ConvertToSILFont;
                ddlSILFonts.Enabled = true;
            }
            else
            {
                // show "non SIL font" UI (radio buttons with the option to just embed)
                Text = Resources.FontWarningDlgTitle;
                txtWarning.Text = String.Format(Resources.EmbedFontsWarning, MyEmbeddedFont, Languages);
                grpOptions.Text = Resources.EmbedFontOptions;
                rdoEmbedFont.Text = Resources.EmbedFont;
                rdoConvertToSILFont.Text = Resources.ConvertToSILFont;
                lblSubstituteSILFont.Visible = false;
            }
            if (ddlSILFonts.Items.Count == 0)
            {
                // update the possible replacements based on what's installed on this system
                ddlSILFonts.Items.AddRange(_silFonts);
                if (ddlSILFonts.Items.Count > 0)
                {
                    ddlSILFonts.SelectedIndex = 0;
                }
            }
            if (RemainingIssues > 0)
            {
                chkRepeatAction.Visible = true;
                chkRepeatAction.Text = String.Format(Resources.RepeatAction, RemainingIssues);
                chkRepeatAction.Checked = RepeatAction;
            }
        }

        /// <summary>
        /// Scans the current workstation's Fonts directory and builds the list of available SIL fonts
        /// </summary>
        public int BuildSILFontList()
        {
            string[] files = FontInternals.GetInstalledFontFiles();
            foreach (var file in files)
            {
                // if the font is freely redistributable, add it to the list
                if (FontInternals.IsFreeFont(file))
                {
                    pfc.AddFontFile(file);
                }
            }
            int index = 0;
            _silFonts = new string[pfc.Families.Length];
            foreach (var fontFamily in pfc.Families)
            {
                _silFonts[index++] = fontFamily.GetName(0);
            }
            return index;
        }

        private void ddlSILFonts_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedFont = ddlSILFonts.SelectedItem.ToString();
        }

        private void rdoConvertToSILFont_CheckedChanged(object sender, EventArgs e)
        {
            // user wants to convert to an SIL font - enable the drop-down and set the selected font
            // to the selected item in the drop-down (or the first item in the list, if there isn't a current
            // selection)
            if (ddlSILFonts.SelectedIndex == -1)
            {
                ddlSILFonts.SelectedItem = 0;
            }
            SelectedFont = ddlSILFonts.SelectedItem.ToString();
            ddlSILFonts.Enabled = true;
            ddlSILFonts.Focus();
        }

        private void rdoEmbedFont_CheckedChanged(object sender, EventArgs e)
        {
            // user wants to embed the default font anyway - disable the drop-down and set the selected font
            // to the non-SIL font
            SelectedFont = MyEmbeddedFont;
            ddlSILFonts.Enabled = false;
        }

        private void chkRepeatAction_CheckedChanged(object sender, EventArgs e)
        {
            RepeatAction = chkRepeatAction.Checked;
        }

    }
}
