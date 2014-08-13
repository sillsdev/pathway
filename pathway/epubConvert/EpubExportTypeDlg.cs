using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using epubConvert.Properties;
using epubValidator;
using SIL.Tool;

namespace epubConvert
{
    public partial class EpubExportTypeDlg : Form
    {
        public string _exportType = "cancel";
       public EpubExportTypeDlg()
        {
            InitializeComponent();
        }

        private void EpubExportTypeDlg_Load(object sender, EventArgs e)
        {
            IcnInfo.Image = SystemIcons.Information.ToBitmap();
            StringBuilder messageBuilder = new StringBuilder(200);

            messageBuilder.Append("To continue, please select one of the actions below:");
            messageBuilder.Append(Environment.NewLine);
            messageBuilder.Append(Environment.NewLine);

            messageBuilder.Append(" \t\u25CF  Click Epub2. \n\n To open the epub2 version of the file.\n");
            messageBuilder.Append(Environment.NewLine);
            messageBuilder.Append(" \t\u25CF  Click Epub3. \n\n To open the epub3 version of the file.\n");
            messageBuilder.Append(Environment.NewLine);
            messageBuilder.Append(" \t\u25CF  Click Folder. \n\n To open the folder containing both the versions.\n");
            messageBuilder.Append(Environment.NewLine);
            messageBuilder.Append(" \t\u25CF  Click Cancel.\n\n To cancel the operation.\n");
            
            lblMessage.Text = messageBuilder.ToString();

        }

       private void ValidateAndDisplayResult(string outputFolder, string fileName, string outputPathWithFileName)
        {
            // Postscript - validate the file using our epubcheck wrapper
            if (Common.Testing)
            {
                // Running the unit test - just run the validator and return the result
                var validationResults = Program.ValidateFile(outputPathWithFileName);
                Debug.WriteLine("Exportepub: validation results: " + validationResults);
            }
            else
            {
                if (MessageBox.Show(Resources.ExportCallingEpubValidator + "\r\nDo you want to Validate ePub file",
                    Resources.ExportComplete, MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information) == DialogResult.Yes)
                {

                    ValidateEpub.ValidateEpubFile(outputPathWithFileName);
                }

                DisplayOutput(outputFolder, fileName, outputPathWithFileName);
            }
        }

        private void DisplayOutput(string outputFolder, string fileName, string outputPathWithFileName)
        {
            if (File.Exists(outputPathWithFileName))
            {
                if (Common.IsUnixOS())
                {
                    string epubFileName = fileName.Replace(" ", "") + ".epub";
                    string replaceEmptyCharacterinFileName = Common.PathCombine(outputFolder, epubFileName);
                    if (outputPathWithFileName != replaceEmptyCharacterinFileName && File.Exists(outputPathWithFileName))
                    {
                        File.Copy(outputPathWithFileName, replaceEmptyCharacterinFileName, true);
                    }

                    SubProcess.Run(outputFolder, "ebook-viewer", epubFileName, false);
                }
                else
                {
                    Process.Start(outputPathWithFileName);
                }
            }
        }

        private void btnexprtEpub2_Click(object sender, EventArgs e)
        {
            _exportType = "epub2";
            Close();
        }

        private void btnexprtEpub3_Click(object sender, EventArgs e)
        {
            _exportType = "epub3";
            Close();
        }

        private void btnexprtfolder_Click(object sender, EventArgs e)
        {
            _exportType = "folder";
            Close();
        }

        private void btnexprtCancel_Click(object sender, EventArgs e)
        {
            _exportType = "cancel";
            Close();
        }


    }
}
