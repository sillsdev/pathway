using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PdfToImage;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class PdftoJpg
    {
        public void ConvertPdftoJpg(string cssFullFileName,bool fromPreview)
        {
            string cssMergeFullFileName;
            if (fromPreview)
            {
                cssMergeFullFileName = Common.MakeSingleCSS(cssFullFileName, "");
            }
            else
            {
                cssMergeFullFileName = cssFullFileName;
            }
            string tempFolderPath = Path.GetTempPath();
            string outputPdfFile = Path.Combine(tempFolderPath, "Preview.pdf");

            string PsSupportPath = Path.Combine(Common.LeftString(cssFullFileName, "Pathway"), "Pathway");
            string xhtmlPreviewFilePath = Path.Combine(PsSupportPath , "PreviewXhtml.xhtml");

            if (!(File.Exists(xhtmlPreviewFilePath) && File.Exists(cssMergeFullFileName)))
            {
                return;
            }

            Pdf pdf = new Pdf(xhtmlPreviewFilePath, cssMergeFullFileName);
            pdf.Create(outputPdfFile);
            //outputPdfFile = outputPdfFile + ".pdf";
            ConvertImage(outputPdfFile);
        }

        public void ConvertPdftoJpg()
        {

            string tempFolderPath = Path.GetTempPath();
            string outputPdfFile = Path.Combine(tempFolderPath, "Preview.pdf");
            ConvertImage(outputPdfFile);
        }

        private void ConvertImage(string filename)
        {
            PDFConvert converter = new PDFConvert();
            string fileExtenstion = ".jpg";
            bool Converted = false;
            converter.OutputToMultipleFile = true;
            converter.FirstPageToConvert = 1;
            converter.LastPageToConvert = 2;
            converter.FitPage = false;
            converter.JPEGQuality = 20;
            converter.OutputFormat = "jpeg";
            System.IO.FileInfo input = new FileInfo(filename);
            string output = string.Format("{0}\\{1}{2}", input.Directory, input.Name, fileExtenstion);
            //If the output file exist alrady be sure to add a random name at the end until is unique!
            while (File.Exists(output))
            {
                output = output.Replace(fileExtenstion, string.Format("{1}{0}", fileExtenstion, DateTime.Now.Ticks));
            }
            Converted = converter.Convert(input.FullName, output);
        }
    }
}
