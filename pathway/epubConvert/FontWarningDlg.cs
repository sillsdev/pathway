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
        public int RemainingIssues { get; set; }
        public string Languages { get; set; }
        public bool RepeatAction { get; set; }

        // Note: this needs to be updated as the font folk free fabulous new font-families.
        public string[] AvailableSILFonts = new string[]
        {
                    "Abyssinica",
                    "Andika",
                    "Apparatus SIL",
                    "Scheharazade",
                    "Lateef",
                    "Charis SIL",
                    "Dai Banna",
                    "Doulos SIL",
                    "Ezra",
                    "Galatia",
                    "Gentium",
                    "Nuosu",
                    "Padauk",
                    "Sophia Nubian",
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
            txtWarning.Text = String.Format(Resources.EmbedFontsWarning, MyEmbeddedFont, Languages);
            grpOptions.Text = Resources.EmbedFontOptions;
            rdoEmbedFont.Text = Resources.EmbedFont;
            rdoConvertToSILFont.Text = Resources.ConvertToSILFont;
            if (ddlSILFonts.Items.Count == 0)
            {
                // update the possible replacements based on what's installed on this system
                foreach (var availableSilFont in AvailableSILFonts)
                {
                    if (EmbeddedFont.IsInstalled(availableSilFont))
                    {
                        ddlSILFonts.Items.Add(availableSilFont);
                    }
                }
                ddlSILFonts.SelectedIndex = 0;
            }
            if (RemainingIssues > 0)
            {
                chkRepeatAction.Visible = true;
                chkRepeatAction.Text = String.Format(Resources.RepeatAction, RemainingIssues);
                chkRepeatAction.Checked = RepeatAction;
            }
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
