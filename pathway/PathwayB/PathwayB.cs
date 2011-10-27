// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2010, SIL International. All Rights Reserved.
// <copyright from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: PathwayB.cs
// Responsibility: Greg Trihus
// ---------------------------------------------------------------------------------------------
using System;
using System.IO;
using System.Xml;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    class Program
    {
        public enum InputFormat
        {
            XHTML,
            USFM,
            USX
        }

        static void Main(string[] args)
        {
            InputFormat inFormat = InputFormat.XHTML;
            var projectInfo = new PublicationInformation
                                  {
                                      ProjectInputType = "Dictionary",
                                      DefaultXhtmlFileWithPath = null,
                                      DefaultCssFileWithPath = null,
                                      IsOpenOutput = false,
                                      ProjectName = "main",
                                  };
            var backendPath = Common.ProgInstall;
            var exportType = "OpenOffice/LibreOffice";
            try
            {
                int i = 0;
                while (i < args.Length)
                {
                    switch (args[i++])
                    {
                        case "--directory":
                        case "-d":
                            projectInfo.ProjectPath = args[i++];
                            break;
                        case "--usx":
                        case "-u":
                            inFormat = InputFormat.USX;
                            if (args[1 + 1] == "*")
                            {
                            }
                            else 
                            { 
                            }
                            break;
                        case "--usfm":
                        case "-m":
                            inFormat = InputFormat.USFM;
                            break;
                        case "--xhtml":
                        case "-x":
                            inFormat = InputFormat.XHTML;
                            projectInfo.DefaultXhtmlFileWithPath = args[i++];
                            break;
                        case "--css":
                        case "-c":
                            projectInfo.DefaultCssFileWithPath = args[i++];
                            break;
                        case "--target":
                        case "-t":
                            //Note: If export type is more than one word, quotes must be used
                            exportType = args[i++];
                            break;
                        case "--input":
                        case "-i":
                            projectInfo.ProjectInputType = args[i++];
                            break;
                        case "--launch":
                        case "-l":
                            projectInfo.IsOpenOutput = true;
                            break;
                        case "--name":
                        case "-n":
                            projectInfo.ProjectName = args[i++];
                            break;
                        case "-?":
                        case "-h":
                            Usage();
                            Environment.Exit(0);
                            break;
                        default:
                            Usage();
                            throw new ArgumentException("Invalid Command Line Argument");
                    }
                }
                // run headless from the command line
                Common.Testing = true;
                //_projectInfo.ProgressBar = null;

                if (inFormat == InputFormat.USFM)
                {
                    // convert from USFM to USX
                    // convert from USX to xhtml
                    projectInfo.DefaultXhtmlFileWithPath = args[i++];
                }
                else if (inFormat == InputFormat.USX)
                {
                    // convert from USX to xhtml
                    projectInfo.DefaultXhtmlFileWithPath = args[i++];
                }

                if (projectInfo.DefaultXhtmlFileWithPath == null || projectInfo.DefaultCssFileWithPath == null)
                {
                    Usage();
                    throw new ArgumentException("Missing required option.");
                }
                if (!File.Exists(projectInfo.DefaultXhtmlFileWithPath))
                    throw new ArgumentException(string.Format("Missing {0}", projectInfo.DefaultXhtmlFileWithPath));
                if (!File.Exists(projectInfo.DefaultCssFileWithPath))
                    throw new ArgumentException(string.Format("Missing {0}", projectInfo.DefaultCssFileWithPath));
                projectInfo.DictionaryPath = Path.GetDirectoryName(projectInfo.DefaultXhtmlFileWithPath);

                if (backendPath.Length == 0)
                {
                    backendPath = Common.GetPSApplicationPath();
                }

                Common.ProgBase = Common.GetPSApplicationPath();
                Param.LoadSettings();
                
                Backend.Load(backendPath);

                Common.ShowMessage = false;
                projectInfo.DictionaryOutputName = projectInfo.ProjectName;
                Backend.Launch(exportType, projectInfo);
            }
            catch (Exception err)
            {
                Console.Write(err.Message);
                Environment.Exit(-1);
            }
            Environment.Exit(0);
        }
        static void Usage()
        {
            var msg = "Usage PathwayB [Options]\r\n";
            msg += "--xhtml -x\tconent file name (required)\r\n";
            msg += "--css -c\tlayout file name (required)\r\n";
            msg += "--target -t\tTarget: [OpenOffice/LibreOffice], InDesign alpha, etc.\r\n";
            msg += "--input -i\t[Dictionary] or Scripture\r\n";
            msg += "--launch -l\tlaunch resulting output in target back end.\r\n";
            msg += "--name -n\t[main] Project name\r\n";
            Console.Write(msg);
        }
        /// <summary>
        /// Converts USFM to USX.
        /// </summary>
        /// <param name="usfm">The standard format for a book.</param>
        /// <returns></returns>
        private XmlDocument ConvertUsfmToUsfx(string usfm, int bookNum)
        {
            XmlDocument doc = new XmlDocument();
            //using (XmlWriter xmlw = doc.CreateNavigator().AppendChild())
            //{
            //    // Convert to XML
            //    UsfmToUsx converter = new UsfmToUsx(xmlw, usfm, scrText.ScrStylesheet(bookNum), false);
            //    converter.Convert();
            //    xmlw.Flush();
            //}
            return doc;
        }

    }
}
