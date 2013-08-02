using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace ApplyPDFLicenseInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine("running.....");

            //Thread.Sleep(2500);

            string allUserPath = GetAllUserPath();
            var executePath = ReadPathinLicenseFile(allUserPath);
            string pdfFileName = string.Empty;
            string[] pdfFiles = Directory.GetFiles(executePath, "*.pdf");
            string getFileName = string.Empty;
            string getCopyrightPdfFileName = string.Empty;

            //   Console.WriteLine(pdfFiles.Length.ToString());

            //  Thread.Sleep(2500);

            //   Console.WriteLine(pdfFiles[0].ToString());

            //    Thread.Sleep(500);

            if (pdfFiles.Length > 0)
            {
                pdfFileName = pdfFiles[0];

                //   Console.WriteLine("pdfFileName = pdfFiles[0];");
            }
            if (pdfFileName != string.Empty || pdfFileName != null)
            {
                getFileName = Path.GetFileName(pdfFileName);
                //     Console.WriteLine("getFileName = Path.GetFileName(pdfFileName);");

                //    Console.WriteLine(getFileName);

                if (File.Exists(pdfFileName))
                {

                    Console.WriteLine(getFileName.ToString());
                    Console.WriteLine("Java Command Executing");

                    getCopyrightPdfFileName = getFileName.Replace(".pdf", "1.pdf");
                    string argumentValue = "-jar pdflicensemanager-2.3.jar putXMP " + getFileName.ToString() + " " +
                                           getCopyrightPdfFileName + " SIL_License.xml";
                    Console.WriteLine(argumentValue.ToString());
                    SetLicense.RunCommand(executePath, "java", argumentValue, true);

                    Console.WriteLine(executePath.ToString());

                    //      Thread.Sleep(1500);
                    Console.WriteLine("Java Command Executed");
                    Console.WriteLine("Done");
                }
            }
            //  Thread.Sleep(2500);
            if (File.Exists(pdfFileName.Replace(".pdf", "1.pdf")))
            {
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = pdfFileName.Replace(".pdf", "1.pdf");
                    process.Start();
                }
            }
            //  Thread.Sleep(500);
            //if (args.Length < 1)
            //{
            //    return;
            //}

            //string fileLoc = Path.Combine(Directory.GetCurrentDirectory(), "sample" + DateTime.Now.Ticks + "1.txt");


            //FileStream fs = null;
            //if (!File.Exists(fileLoc))
            //{
            //    using (fs = File.Create(fileLoc))
            //    {

            //    }
            //}

            //if (File.Exists(fileLoc))
            //{
            //    using (StreamWriter sw = new StreamWriter(fileLoc))
            //    {
            //        sw.Write("Some sample text for the file");
            //    }
            //}

            //SetLicense.RunCommand(output.Directory, processFullPath, output.Directory, true);

            //XsltProcess xsltProcess = new XsltProcess();
            //xsltProcess.XsltTransform(args[0], args[1], args[2], args[3]);
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
                        executePath = line;
                    }

                    reader.Close();
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
