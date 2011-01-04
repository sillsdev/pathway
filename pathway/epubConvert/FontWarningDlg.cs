using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;
using epubConvert.Properties;
using SIL.Tool;

namespace epubConvert
{
    public partial class FontWarningDlg : Form
    {
        // properties
        public string MyEmbeddedFont { get; set; }
        public string SelectedFont { get; set; }
        public int RemainingIssues { get; set; }
        public string Languages { get; set; }
        public bool RepeatAction { get; set; }

        // variables
        private PrivateFontCollection pfc = new PrivateFontCollection();
        private string[] _silFonts;

        // methods
        public FontWarningDlg()
        {
            InitializeComponent();
        }

        public bool UseFontAnyway()
        {
            return (!rdoEmbedFont.Visible) ? false : rdoEmbedFont.Checked;
        }

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
            string fontFolder = FontInternals.GetFontFolderPath();
            string[] files = Directory.GetFiles(fontFolder, "*.ttf");
            foreach (var file in files)
            {
                if (FontInternals.IsSILFont(file))
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
