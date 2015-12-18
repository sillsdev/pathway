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
		public string ExportType = "cancel";

		public EpubExportTypeDlg()
		{
			Common.SetupLocalization();
			InitializeComponent();
			if (Common.L10NMngr != null)
			{
				Common.L10NMngr.AddString("EpubExportTypeDlg.lblMessage.Part1", MessageBuilder.ToString(), "", "", "");
				Common.L10NMngr.AddString("EpubExportTypeDlg.lblMessage.Part2", MessageBuilder1.ToString(), "", "", "");
			}
		}

		private void EpubExportTypeDlg_Load(object sender, EventArgs e)
		{
			IcnInfo.Image = SystemIcons.Information.ToBitmap();
			string msgPart1 = LocalizationManager.GetString("EpubExportTypeDlg.lblMessage.Part1", MessageBuilder.ToString());
			string msgFolder = LocalizationManager.GetString("EpubExportTypeDlg.lblMessage.Folder", "Folder") + @" ";
			string msgPart2 = LocalizationManager.GetString("EpubExportTypeDlg.lblMessage.Part2", MessageBuilder1.ToString());
			string msgNote = LocalizationManager.GetString("EpubExportTypeDlg.lblMessage.NoteCaption", "Note:") + @" ";
			string msgNoteMessage = LocalizationManager.GetString("EpubExportTypeDlg.lblMessage.NoteMsg", "Some readers cannot display Epub3 files correctly.");

			rtbControl.Text = msgPart1 + msgFolder + msgPart2 + msgNote + msgNoteMessage;
		}

		private static StringBuilder MessageBuilder1
		{
			get
			{
				var messageBuilder1 = new StringBuilder(300);
				messageBuilder1.Append("to open the folder that contains both Epub files and the HTML5. You can display the HTML files in your internet browser. ");
				messageBuilder1.Append("\r\n");
				messageBuilder1.Append("\r\n");
				return messageBuilder1;
			}
		}

		private static StringBuilder MessageBuilder
		{
			get
			{
				var messageBuilder = new StringBuilder(300);
				messageBuilder.Append("The export process is finished. There are separate files for Epub2, Epub3 and HTML5. ");
				messageBuilder.Append("\r\n");
				messageBuilder.Append("\r\n");
				messageBuilder.Append("Click a button below to open the desired Epub file in your default reader. Alternatively, click ");
				return messageBuilder;
			}
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
				var msg = LocalizationManager.GetString("EpubExportTypeDlg.ValidateAndDisplay.Message", "\r\n Do you want to Validate ePub file?", "");
				string caption = LocalizationManager.GetString("EpubExportTypeDlg.ValidateAndDisplay.Caption", "Export Complete", "");
				if (Utils.MsgBox(Resources.ExportCallingEpubValidator + msg,
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
			ExportType = "epub2";
			Close();
		}

		private void btnexprtEpub3_Click(object sender, EventArgs e)
		{
			ExportType = "epub3";
			Close();
		}

		private void btnexprtfolder_Click(object sender, EventArgs e)
		{
			ExportType = "folder";
			Close();
		}

		private void btnexprtCancel_Click(object sender, EventArgs e)
		{
			ExportType = "cancel";
			Close();
		}

		private void rtbControl_Enter(object sender, EventArgs e)
		{
			IcnInfo.Focus();
		}
	}
}
