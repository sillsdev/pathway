using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PrincessConvert
{
    public partial class PDFdisplay : Form
    {
        Main main = new Main();
        public PDFdisplay()
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            // Add any initializationAfterthe InitializeComponent() call.
            cbShowSinglePage.Checked = Main.blnPDFshowSinglePage;
            cbShowScrollBars.Checked = Main.blnPDFshowScrollbars;
            cbShowPDFToolbar.Checked = Main.blnPDFshowToolbar;
            // AxAcroPDF1.setView("Fit")
            cbShowThumbnails.Checked = Main.blnPDFshowThumbnails;
            cbPDFshowBookmarks.Checked = Main.blnPDFshowBooksmarks;
            cbPDFzoomOn.Checked = Main.blnPDFzoomOn;
            numericUpDownPDFzoom.Value = main.iPDFzoomAmount;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Main.blnPDFshowScrollbars = this.cbShowScrollBars.Checked;
            Main.blnPDFshowToolbar = this.cbShowPDFToolbar.Checked;
            // AxAcroPDF1.setView("Fit")
            Main.blnPDFshowSinglePage = this.cbShowSinglePage.Checked;
            Main.blnPDFshowThumbnails = this.cbShowThumbnails.Checked;
            Main.blnPDFshowBooksmarks = this.cbPDFshowBookmarks.Checked;
            Main.blnPDFzoomOn = this.cbPDFzoomOn.Checked;
            main.iPDFzoomAmount = (int)this.numericUpDownPDFzoom.Value;
            main.writeIniFile();
            main.displayPDF();
            this.Close();

        }

        private void cbShowThumbnails_CheckedChanged(object sender, EventArgs e)
        {
            if (cbShowThumbnails.Checked == true)
            {
                cbPDFshowBookmarks.Checked = false;
            }
            //else
            //{
            //    // skip
            //}

        }

        private void cbPDFshowBookmarks_CheckedChanged(object sender, EventArgs e)
        {
            if (cbPDFshowBookmarks.Checked == true)
            {
                cbShowThumbnails.Checked = false;
            }
            //else
            //{
            //    // skip
            //}

        }
    }
}
