using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using epubConvert.Properties;
using epubValidator;
using L10NSharp;
using SilTools;
using SIL.Tool;

namespace epubConvert
{
    public partial class EpubExportTypeDlg : Form
    {
        public string _exportType = "cancel";
      
       public EpubExportTypeDlg()
       {
	       Common.SetupLocalization();
	       InitializeComponent();
       }

        private void EpubExportTypeDlg_Load(object sender, EventArgs e)
        {
            IcnInfo.Image = SystemIcons.Information.ToBitmap();
            StringBuilder messageBuilder = new StringBuilder(300);

            messageBuilder.Append("The export process is finished. There is a separate file for\n");
            messageBuilder.Append("Epub2, Epub3 and HTML5.");
            messageBuilder.Append(Environment.NewLine);
            messageBuilder.Append(Environment.NewLine);

            messageBuilder.Append("Click a button below to open the desired Epub file in your\n");
            messageBuilder.Append("default reader. Alternatively, click");

            StringBuilder messageBuilder1 = new StringBuilder(300);
            messageBuilder1.Append("to open the folder\n");
            messageBuilder1.Append("that contains both Epub files and the HTML5. You can\n");
            messageBuilder1.Append("display the HTML files in your internet browser.\n");

            messageBuilder1.Append(Environment.NewLine);

            lblMessage.SelectionFont = new Font("Charis SIL", 12, FontStyle.Regular);
			lblMessage.SelectedText = LocalizationManager.GetString("EpubExportTypeDlg.EpubFileType.Message", messageBuilder.ToString(), "");
            lblMessage.SelectionFont = new Font("Charis SIL", 12, FontStyle.Bold);
            lblMessage.SelectedText = " Folder ";
            lblMessage.SelectionFont = new Font("Charis SIL", 12, FontStyle.Regular);
            lblMessage.SelectedText = messageBuilder1.ToString();

            lblMessage.SelectionFont = new Font("Charis SIL", 12, FontStyle.Bold);
            lblMessage.SelectedText = "Note: ";
            lblMessage.SelectionFont = new Font("Charis SIL", 12, FontStyle.Regular);
            lblMessage.SelectedText = "Some readers cannot display Epub3 files correctly.";
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
                var msg = LocalizationManager.GetString("EpubExportTypeDlg.ValidateAndDisplay.Message", "\r\nDo you want to Validate ePub file?", "");
				string caption = LocalizationManager.GetString("EpubExportTypeDlg.ValidateAndDisplay.Caption", "Export Complete", "");
				if(Utils.MsgBox(Resources.ExportCallingEpubValidator + msg,
					caption, MessageBoxButtons.YesNo,
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
