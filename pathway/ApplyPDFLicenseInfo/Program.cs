using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using SIL.PublishingSolution;
using SIL.Tool;

namespace ApplyPDFLicenseInfo
{
    class Program
    {
        static List<string> _readLicenseFilesBylines = new List<string>();

        static void Main(string[] args)
        {
            bool isUnix = false;
            Console.WriteLine("running.....");
            //Thread.Sleep(2500);
            string allUserPath = GetAllUserPath();
            string licenseFileName = ReadPathinLicenseFile(allUserPath);

            if (_readLicenseFilesBylines.Count < 0)
            {
                return;
            }
            string executePath = _readLicenseFilesBylines[0];
            string workingDirectory = Path.GetDirectoryName(_readLicenseFilesBylines[1]);
            string xhtmlFile = _readLicenseFilesBylines[1];
            string exportTitle = _readLicenseFilesBylines[2];
            string creatorTool = _readLicenseFilesBylines[3];
            //Console.WriteLine(executePath);
            string pdfFileName = string.Empty;
            
            //Console.WriteLine(pdfFiles.Length.ToString());
            //Thread.Sleep(2500);
            //Console.WriteLine(pdfFiles[0].ToString());
            //Thread.Sleep(500);
            pdfFileName = ProcessLicensePdf(pdfFileName, executePath);

            exportTitle = exportTitle.Replace(" ", "_") + ".pdf";
            exportTitle = Path.Combine(workingDirectory, exportTitle);
            string licencePdfFile = pdfFileName.Replace(".pdf", "1.pdf");

            //Thread.Sleep(2500);
            if (File.Exists(licencePdfFile))
            {
                File.Copy(licencePdfFile, exportTitle, true);
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = exportTitle;
                    process.Start();
                }
            }

            if (File.Exists(pdfFileName) && File.Exists(exportTitle))
                File.Delete(pdfFileName);

            if (creatorTool.ToLower() == "libreoffice")
            {
                Common.CleanupExportFolder(xhtmlFile, ".tmp,.de,.exe,.jar,.xml,.odt,.odm", "layout", string.Empty);
                CreateRAMP(xhtmlFile);
            }
            //Thread.Sleep(500);
        }

        private static void CreateRAMP(string executePath)
        {
            string outputExtn = string.Empty;
            outputExtn = ".pdf";
            Ramp ramp = new Ramp();
            ramp.Create(executePath, outputExtn, "Dictionary");
        }

        private static string ProcessLicensePdf(string pdfFileName, string executePath)
        {
            string getFileName;
            bool isUnix = false;
            string[] pdfFiles = Directory.GetFiles(executePath, "*.pdf");
            string getCopyrightPdfFileName;
            if (pdfFiles.Length > 0)
            {
                pdfFileName = pdfFiles[0];
                //    Console.WriteLine("pdfFileName = pdfFiles[0];");
            }
            if (pdfFileName != string.Empty || pdfFileName != null)
            {
                getFileName = Path.GetFileName(pdfFileName);
                //   Console.WriteLine("getFileName = Path.GetFileName(pdfFileName);");
                //   Console.WriteLine(getFileName);
                if (File.Exists(pdfFileName))
                {
                    isUnix = SetLicense.UnixVersionCheck();

                    if (isUnix)
                    {
                        Console.WriteLine(getFileName.ToString());
                        Console.WriteLine("Java Command Executing");

                        getCopyrightPdfFileName = getFileName.Replace(".pdf", "1.pdf");
                        string argumentValue = "-jar 'pdflicensemanager-2.3.jar' 'putXMP' '" + getFileName.ToString() + "' '" +
                                               getCopyrightPdfFileName + "' 'SIL_License.xml'";
                        Console.WriteLine(argumentValue.ToString());
                        SetLicense.RunCommand(executePath, "java", argumentValue, true);

                        Console.WriteLine(executePath.ToString());

                        //Thread.Sleep(1500);
                        Console.WriteLine("Java Command Executed");
                        Console.WriteLine("Done");
                    }
                    else
                    {
                        Console.WriteLine(getFileName.ToString());
                        Console.WriteLine("Java Command Executing");

                        getCopyrightPdfFileName = getFileName.Replace(".pdf", "1.pdf");
                        string argumentValue = "-jar pdflicensemanager-2.3.jar putXMP " + getFileName.ToString() + " " +
                                               getCopyrightPdfFileName + " SIL_License.xml";
                        Console.WriteLine(argumentValue.ToString());
                        SetLicense.RunCommand(executePath, "java", argumentValue, true);

                        Console.WriteLine(executePath.ToString());

                        //Thread.Sleep(1500);
                        Console.WriteLine("Java Command Executed");
                        Console.WriteLine("Done");
                    }
                }
            }
            return pdfFileName;
        }

        private static string ReadPathinLicenseFile(string allUserPath)
        {
            string fileLoc = Path.Combine(allUserPath, "License.txt");
            string executePath = string.Empty;
            int countRead = 0;

            if (File.Exists(fileLoc))
            {
                using (StreamReader reader = new StreamReader(fileLoc))
                {
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        _readLicenseFilesBylines.Add(line);
                    }

                    reader.Close();

                    executePath = fileLoc;
                }
            }
            return executePath;
        }

        private string ReadPathinLicenseFile(string allUserPath, int readLineNumber)
        {
            string fileLoc = Path.Combine(allUserPath, "License.txt");
            string executePath = string.Empty;
            int countRead = 0;

            if (File.Exists(fileLoc))
            {
                using (StreamReader reader = new StreamReader(fileLoc))
                {
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        _readLicenseFilesBylines.Add(line);
                    }

                    reader.Close();

                    executePath = _readLicenseFilesBylines[readLineNumber].ToString();
                }
            }
            return executePath;
        }

        /// <summary>
        /// Get all user path
        /// </summary>
        /// <returns></returns>
        public static string GetAllUserPath()
        {
            string allUserPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            allUserPath += "/SIL/Pathway";
            return SetLicense.DirectoryPathReplace(allUserPath);
        }
    }
}
