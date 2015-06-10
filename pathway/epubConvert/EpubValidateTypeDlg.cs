using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SIL.Tool;

namespace epubConvert
{
    public partial class EpubValidateTypeDlg : Form
    {
        public string _exportType = "epub2";

        public EpubValidateTypeDlg()
        {
            Common.SetupLocalization("epubConvert");
            InitializeComponent();
        }

        private void EpubValidateTypeDlg_Load(object sender, EventArgs e)
        {
            IcnInfo.Image = SystemIcons.Information.ToBitmap();
            StringBuilder messageBuilder = new StringBuilder(200);

            messageBuilder.Append("To continue, please select one of the actions below:");
            messageBuilder.Append(Environment.NewLine);
            messageBuilder.Append(Environment.NewLine);

            messageBuilder.Append(" \t\u25CF  Click Epub2. \n\n To validate the epub2 version of the file.\n");
            messageBuilder.Append(Environment.NewLine);
            messageBuilder.Append(" \t\u25CF  Click Epub3. \n\n To validate the epub3 version of the file.\n");
            messageBuilder.Append(Environment.NewLine);
            messageBuilder.Append(" \t\u25CF  Click Both. \n\n To validate the epub2 and epub3 both the versions.\n");
            messageBuilder.Append(Environment.NewLine);
            messageBuilder.Append(" \t\u25CF  Click Neither.\n\n To cancel the validate.\n");

            lblMessage.Text = messageBuilder.ToString();

        }

        private void btnValidateEpub2_Click(object sender, EventArgs e)
        {
            _exportType = "epub2";
            Close();
        }

        private void btnValidateEpub3_Click(object sender, EventArgs e)
        {
            _exportType = "epub3";
            Close();
        }

        private void btnBoth_Click(object sender, EventArgs e)
        {
            _exportType = "both";
            Close();
        }

        private void btnNeither_Click(object sender, EventArgs e)
        {
            _exportType = "neigher";
            Close();
        }
    }
}
