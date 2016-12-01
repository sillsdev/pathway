using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using SIL.Tool;

//This will be called by macro externally. so, please do not remove this since it is not called by any other classes.

namespace ApplyPDFLicenseInfo
{
    class Program
    {
        static List<string> _readLicenseFilesBylines = new List<string>();

        static void Main(string[] args)
        {
            Console.WriteLine("running.....");
            string allUserPath = GetAllUserPath();
            ReadPathinLicenseFile(allUserPath);
            if (_readLicenseFilesBylines.Count < 0)
            {
                return;
            }
            string executePath = _readLicenseFilesBylines[0];
            string workingDirectory = Path.GetDirectoryName(_readLicenseFilesBylines[1]);
            string xhtmlFile = _readLicenseFilesBylines[1];
            string exportTitle = _readLicenseFilesBylines[2];
            string creatorTool = _readLicenseFilesBylines[3];
            string inputType = _readLicenseFilesBylines[4];
            string commonTesting = _readLicenseFilesBylines[5];
            string pdfFileName = string.Empty;

            pdfFileName = ProcessLicensePdf(pdfFileName, executePath);

            if (exportTitle == string.Empty)
                exportTitle = "ExportPdf";

            exportTitle = exportTitle.Replace(" ", "_") + ".pdf";
            if (workingDirectory != null) exportTitle = Path.Combine(workingDirectory, exportTitle);
            string licencePdfFile = pdfFileName.Replace(".pdf", "1.pdf");

            ShowPDFFile(licencePdfFile, exportTitle, commonTesting, pdfFileName);

            System.Globalization.TextInfo myTI = new System.Globalization.CultureInfo("en-US", false).TextInfo;
            inputType = myTI.ToTitleCase(inputType);
            if (creatorTool.ToLower() == "libreoffice")
            {
                while (true)
                {
                    var foundOpenDoc = false;
                    foreach (var process in Process.GetProcesses())
                    {
                        try
                        {
                            var name = process.ProcessName;
                            if (name.Contains("soffice"))
                            {
                                foundOpenDoc = true;
                            }
                        }
                        catch (InvalidOperationException)
                        {
                            // If the process has already exited
                        }
                    }
                    if (!foundOpenDoc) break;
                    Thread.Sleep(1000);
                }
                Common.CleanupExportFolder(xhtmlFile, ".tmp,.de,.exe,.jar,.xml,.odt,.odm", "layout.css", string.Empty);
                LoadParameters(inputType);
                CreateRAMP(xhtmlFile, inputType);
                Common.CleanupExportFolder(xhtmlFile, ".xhtml,.xml,.css", "layout.css", string.Empty);
            }
            if (creatorTool.ToLower().Contains("prince"))
            {
                string cleanExtn = ".tmp,.de,.exe,.jar,.xml";
                Common.CleanupExportFolder(xhtmlFile, cleanExtn, "layout.css", string.Empty);
                LoadParameters(inputType);
                xhtmlFile = xhtmlFile.Replace(".pdf", ".xhtml");
                if (File.Exists(xhtmlFile))
                {
                    CreateRAMP(xhtmlFile, inputType);
                    cleanExtn = ".xhtml,.xml,.css";
                    Common.CleanupExportFolder(xhtmlFile, cleanExtn, "layout.css", string.Empty);
                }
            }
        }

        private static void ShowPDFFile(string licencePdfFile, string exportTitle, string commonTesting, string pdfFileName)
        {
            if (File.Exists(licencePdfFile))
            {
                File.Copy(licencePdfFile, exportTitle, true);
                var fileInfo = new FileInfo(licencePdfFile);
                while (true)
                {
                    var newFileInfo = new FileInfo(exportTitle);
                    if (newFileInfo.Length == fileInfo.Length) break;
                    Thread.Sleep(1000);
                }

                if (commonTesting.ToLower().Contains("false"))
                {
                    using (var process = new Process())
                    {
                        process.StartInfo.FileName = exportTitle;
                        process.Start();
                    }
                }
            }
            else
            {
                if(pdfFileName != exportTitle)
                    File.Copy(pdfFileName, exportTitle, true);
                var fileInfo = new FileInfo(licencePdfFile);
                while (true)
                {
                    var newFileInfo = new FileInfo(exportTitle);
                    if (newFileInfo.Length == fileInfo.Length) break;
                    Thread.Sleep(1000);
                }


                if (commonTesting.ToLower().Contains("false"))
                {
                    using (var process = new Process())
                    {
                        process.StartInfo.FileName = exportTitle;
                        process.Start();
                    }
                }
            }

            if (File.Exists(pdfFileName) && File.Exists(exportTitle) && pdfFileName != exportTitle)
            {
                File.Delete(pdfFileName);
            }
            if (File.Exists(licencePdfFile) && File.Exists(exportTitle))
            {
                File.Delete(licencePdfFile);
            }
        }

        private static void LoadParameters(string inputType)
        {
            Param.LoadSettings();
            Param.SetValue(Param.InputType, inputType);
            Param.LoadSettings();
        }

        private static void CreateRAMP(string executePath, string inputType)
        {
            const string outputExtn = ".pdf";
            Ramp ramp = new Ramp();
            ramp.Create(executePath, outputExtn, inputType);
        }

        private static string ProcessLicensePdf(string pdfFileName, string executePath)
        {
            string[] pdfFiles = Directory.GetFiles(executePath, "*.pdf");
            string getCopyrightPdfFileName;
            if (pdfFiles.Length > 0)
            {
                pdfFileName = pdfFiles[0];
            }
            if (pdfFileName != string.Empty || pdfFileName != null)
            {
                var getFileName = Path.GetFileName(pdfFileName);
                if (File.Exists(pdfFileName))
                {
                    var isUnix = SetLicense.UnixVersionCheck();

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
