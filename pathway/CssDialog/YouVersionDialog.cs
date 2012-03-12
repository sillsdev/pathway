using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public partial class YouVersionDialog : Form
    {
        public YouVersionDialog()
        {
            InitializeComponent();
        }

        private void btn_browse_Click(object sender, EventArgs e)
        {
            txtInputBundle.Text = GetFilePath("ZIP Files|*.zip");
        }

        private string GetFilePath(string fileType)
        {
            var fd = new OpenFileDialog();
            fd.Filter = fileType;
            fd.ShowDialog();
            return fd.FileName;
        }

        private void YouVersionDialog_Load(object sender, EventArgs e)
        {

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            PublicationInformation projInfo = new PublicationInformation();
            projInfo.DefaultXhtmlFileWithPath = txtInputBundle.Text.Trim();

            ExportYouVersion exportYouVersion = new ExportYouVersion();
            exportYouVersion.Export(projInfo);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
