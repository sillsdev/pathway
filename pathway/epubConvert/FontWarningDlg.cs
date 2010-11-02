using System;
using System.Drawing;
using System.Windows.Forms;
using epubConvert.Properties;

namespace epubConvert
{
    public partial class FontWarningDlg : Form
    {
        public string MyEmbeddedFont { get; set; }
        public string SelectedFont { get; set; }

        // Note: this needs to be updated as the font folk free fabulous new font-families.
        public string[] AvailableSILFonts = new string[]
        {
                    "Abyssinica",
                    "Andika",
                    "Apparatus",
                    "Scheharazade",
                    "Lateef",
                    "Charis",
                    "Dai Banna",
                    "Doulos",
                    "Ezra",
                    "Galatia",
                    "Gentium",
                    "Nuosu",
                    "Padauk",
                    "SIL IPA Unicode",
                    "Sophia",
                    "Tai Heritage Pro"
        };

        public FontWarningDlg()
        {
            InitializeComponent();
        }

        public bool UseFontAnyway()
        {
            return rdoEmbedFont.Checked;
        }

        private void FontWarningDlg_Load(object sender, EventArgs e)
        {
            icnWarning.Image = SystemIcons.Warning.ToBitmap();
            Text = Resources.FontWarningDlgTitle;
            txtWarning.Text = String.Format(Resources.EmbedFontsWarning, MyEmbeddedFont);
            grpOptions.Text = Resources.EmbedFontOptions;
            rdoEmbedFont.Text = Resources.EmbedFont;
            rdoConvertToSILFont.Text = Resources.ConvertToSILFont;
            ddlSILFonts.Items.AddRange(AvailableSILFonts);
            ddlSILFonts.SelectedIndex = 0;
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

    }
}
