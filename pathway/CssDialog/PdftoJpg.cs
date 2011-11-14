using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using PdfToImage;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class PdftoJpg
    {
        public string ConvertPdftoJpg(string cssFullFileName, bool fromPreview, string loadType)
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

            string PsSupportPath = Path.Combine(Common.LeftString(cssFullFileName, "Pathway"), "Pathway");
            string PsSupportPathfrom = Common.GetApplicationPath();
            string previewFile = loadType + "Preview.xhtml";
            string xhtmlPreviewFilePath = Path.Combine(PsSupportPath, previewFile);
            string xhtmlPreviewFile_fromPath = Path.Combine(PsSupportPathfrom, previewFile);
            if (!File.Exists(xhtmlPreviewFilePath))
            {
                if (File.Exists(xhtmlPreviewFile_fromPath))
                {
                    File.Copy(xhtmlPreviewFile_fromPath, xhtmlPreviewFilePath);
                }
            }

            if (!(File.Exists(xhtmlPreviewFilePath) && File.Exists(cssMergeFullFileName)))
            {
                return string.Empty;
            }
            PublicationInformation ps = new PublicationInformation();
            ps.DefaultXhtmlFileWithPath = xhtmlPreviewFilePath;
            ps.DefaultCssFileWithPath = cssMergeFullFileName;
            ps.FinalOutput = "pdf";
            ps.OutputExtension = "pdf";
            string fileName = Path.GetTempFileName();
            ps.ProjectName = fileName;
            ps.JpgPreview = true;
            ps.DictionaryOutputName = fileName;
            ps.DictionaryPath = Path.GetDirectoryName(xhtmlPreviewFilePath);
            ps.ProjectInputType = loadType;

            ExportLibreOffice openOffice = new ExportLibreOffice();
            openOffice.Export(ps);

            //Pdf pdf = new Pdf(xhtmlPreviewFilePath, cssMergeFullFileName);
            //pdf.Create(outputPdfFile);
            //string outputPdfFile = Path.Combine(ps.DictionaryPath, Path.GetFileNameWithoutExtension(fileName) + ".pdf");
            string outputPdfFile = Path.Combine(ps.DictionaryPath, Path.GetFileNameWithoutExtension(openOffice.GeneratedPdfFileName) + ".pdf");

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            TimeSpan timeSpan = new TimeSpan(0, 2, 0); // 2 mins
            while (!File.Exists(outputPdfFile))
            {
                if (stopWatch.Elapsed > timeSpan)
                {
                    stopWatch.Stop();
                    return "";
                }
                Application.DoEvents();
            }
            //MessageBox.Show(stopWatch.Elapsed.ToString());
            ConvertImage(outputPdfFile, fileName);
            return ps.ProjectName;
        }

        private void ConvertPdftoJpg()
        {
            //string tempFolderPath = Path.GetTempPath();
            //string outputPdfFile = Path.Combine(tempFolderPath, "Preview.pdf");
            //ConvertImage(outputPdfFile);
        }

        private void ConvertImage(string filename, string outputFileName)
        {
            try
            {
                //string previewFile = outputFileName;
                string outputPath = Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(outputFileName)) + ".pdf";
                File.Copy(filename, outputPath, true);
                Common.DeleteFile(filename);
                string osName = Common.GetOsName();
                string fileExtenstion = ".jpg";
                bool Converted = false;
                FileInfo input = new FileInfo(outputPath);
                if (osName == "Unix")
                {
                    PDFtoImageConverter converter = new PDFtoImageConverter();
                    converter.OutputToMultipleFile = true;
                    converter.FirstPageToConvert = 1;
                    converter.LastPageToConvert = 2;
                    converter.FitPage = false;
                    converter.JPEGQuality = 75;
                    converter.OutputFormat = "jpeg";

                    string output = string.Format("{0}/{1}{2}", input.Directory, input.Name, fileExtenstion);
                    //If the output file exist alrady be sure to add a random name at the end until is unique!
                    while (File.Exists(output))
                    {
                        output = output.Replace(fileExtenstion, string.Format("{1}{0}", fileExtenstion, DateTime.Now.Ticks));
                    }
                    Converted = converter.Convert(input.FullName, output);
                }
                else
                {
                    PDFConvert converter = new PDFConvert();
                    converter.OutputToMultipleFile = true;
                    converter.FirstPageToConvert = 1;
                    converter.LastPageToConvert = 2;
                    converter.FitPage = false;
                    converter.JPEGQuality = 75;
                    converter.OutputFormat = "jpeg";

                    string output = string.Format("{0}/{1}{2}", input.Directory, input.Name, fileExtenstion);
                    //If the output file exist alrady be sure to add a random name at the end until is unique!
                    while (File.Exists(output))
                    {
                        output = output.Replace(fileExtenstion, string.Format("{1}{0}", fileExtenstion, DateTime.Now.Ticks));
                    }
                    Converted = converter.Convert(input.FullName, output);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
