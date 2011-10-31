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
using System.Diagnostics;
using System.IO;
using System.Xml;
using SIL.Tool;
using System.Collections.Generic;

namespace SIL.PublishingSolution
{
    class Program
    {
        public enum InputFormat
        {
            XHTML,
            USFM,
            USX,
            PTBUNDLE
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
            var files = new List<string>();
            try
            {
                int i = 0;
                int fileNum = 0;
                while (i < args.Length)
                {
                    switch (args[i++])
                    {
                        case "--directory":
                        case "-d":
                            projectInfo.ProjectPath = args[i++];
                            break;
                        case "--files":
                        case "-f":
                            // store the files in our internal list for now 
                            // (single filenames and * will end up as a single element in the list)
                            while (args[i].EndsWith(","))
                            {
                                files.Add(args[i].Substring(0, args[i].Length - 1));
                                i++;
                            }
                            files.Add(args[i++]);
                            break;
                        case "--inputformat":
                        case "-if":
                            var format = args[i++];
                            switch (format)
                            {
                                case "xhtml":
                                    inFormat = InputFormat.XHTML;
                                    break;
                                case "usfm":
                                    inFormat = InputFormat.USFM;
                                    break;
                                case "usx":
                                    inFormat = InputFormat.USX;
                                    break;
                                case "ptb":
                                    inFormat = InputFormat.PTBUNDLE;
                                    break;
                            }
                            break;
                        case "--xhtml":
                        case "-x":
                            // retained for backwards compatibility (not documented)
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
                            throw new ArgumentException("Invalid Command Line Argument: " + args[i]);
                    }
                }
                // run headless from the command line
                Common.Testing = true;
                //_projectInfo.ProgressBar = null;

                if (inFormat == InputFormat.USFM)
                {
                    // convert from USFM to USX
                    // convert from USX to xhtml
//                    projectInfo.DefaultXhtmlFileWithPath = args[i++];
                }
                else if (inFormat == InputFormat.USX)
                {
                    // convert from USX to xhtml
                    ConvertUsxToXhtml(projectInfo, files);
//                    projectInfo.DefaultXhtmlFileWithPath = args[i++];
                }
                else if (inFormat == InputFormat.XHTML)
                {
                    if (projectInfo.ProjectInputType == "Dictionary")
                    {
                        // dictionary
                        // main - if there's a file name of "main" in the list, we'll use it as the default xhtml; 
                        // if not, we'll use the first item in the list
                        int index = files.FindIndex(
                            something => (something.ToLower().Equals("main.xhtml"))
                            );
                        projectInfo.DefaultXhtmlFileWithPath = (index >= 0) ? 
                            Path.Combine(projectInfo.ProjectPath, files[index]) :
                            Path.Combine(projectInfo.ProjectPath, files[0]);
                        // reversal index - needs to be named "flexrev.xhtml" 
                        // (for compatibility with transforms in Pathway)
                        index = files.FindIndex(
                            something => (something.ToLower().Equals("flexrev.xhtml"))
                            );
                        projectInfo.IsReversalExist = (index >= 0);

                        projectInfo.IsLexiconSectionExist = File.Exists(projectInfo.DefaultXhtmlFileWithPath);
                        projectInfo.ProjectFileWithPath = projectInfo.DefaultXhtmlFileWithPath;
                        projectInfo.SwapHeadword = false;
                        projectInfo.FromPlugin = true;
                        projectInfo.DefaultRevCssFileWithPath = Path.Combine(Path.GetDirectoryName(projectInfo.DefaultXhtmlFileWithPath), "FlexRev.css");
                        projectInfo.DictionaryPath = Path.GetDirectoryName(projectInfo.ProjectPath);

                    }
                    else if (projectInfo.ProjectInputType == "Scripture")
                    {
                        projectInfo.DefaultXhtmlFileWithPath = Path.Combine(projectInfo.ProjectPath, files[0]);
                    }
                }

                // troubleshooting... remove
                Console.WriteLine("xhtml: " + projectInfo.DefaultXhtmlFileWithPath);
                Console.WriteLine("css: " + projectInfo.DefaultCssFileWithPath);
                Console.WriteLine("Reversal:" + projectInfo.IsReversalExist);

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
                Console.WriteLine("PathwayB encountered an error while processing: " + err.Message);
                if (err.StackTrace != null)
                {
                    Console.WriteLine(err.StackTrace);
                }
                Environment.Exit(-1);
            }
            Environment.Exit(0);
        }
        static void Usage()
        {
            Console.Write("Usage: PathwayB [Options]\r\n\r\n");
            Console.Write("where options include:\r\n");
            Console.Write("   --input | -i <type>       (required) type of content. Can be one of:\r\n");
            Console.Write("                             Dictionary   Dictionary content.\r\n");
            Console.Write("                             Scripture    Scripture content.\r\n");
            Console.Write("   --inputformat | -if <format>\r\n");
            Console.Write("                             (required) content input format. Can be one of:\r\n");
            Console.Write("                             usfm   Unified Standard Format Marker content.\r\n");
            Console.Write("                             usx    USX content from Paratext 7.x.\r\n");
            Console.Write("                             xhtml  XHTML from FieldWorks (FLEX or TE).\r\n");
            Console.Write("                             ptb    Paratext bundle from Paratext 7.3.\r\n");
            Console.Write("   --directory | -d <path>   (required) full path to the content.\r\n");
            Console.Write("   --files | -f {<file>[, <file>] | *}\r\n");
            Console.Write("                             (required) files to process, or * for all files in\r\n");
            Console.Write("                             the directory specified by the -d flag.\r\n");
            Console.Write("   --target | -t <format>    (required) desired output format. Can be one of:\r\n");
            Console.Write("                             \"E-Book (.epub)\"           .epub format.\r\n");
            Console.Write("                             \"Go Bible\"                 Go Bible .jar format.\r\n");
            Console.Write("                             \"Logos Alpha\"              Libronix format.\r\n");
            Console.Write("                             \"PDF (using OpenOffice/LibreOffice)\" \r\n");
            Console.Write("                                                          .pdf format.\r\n");
            Console.Write("                             \"PDF (using Prince)\"       .pdf format.\r\n");
            Console.Write("                             \"InDesign\"                 .idml format.\r\n");
            Console.Write("                             \"OpenOffice/LibreOffice\"   .odt format.\r\n");
            Console.Write("                             \"XeLaTex\"                  .tex format.\r\n");
            Console.Write("   --css | -c                stylesheet file name (required for xhtml only).\r\n");
            Console.Write("   --launch | -l             launch resulting output in target back end.\r\n");
            Console.Write("   --name | -n               [main] Project name.\r\n\r\n");
            Console.Write("Notes:\r\n");
            Console.Write("- Not all output types may be available, depending on your installation\r\n");
            Console.Write("  Package. To verify the available output types, open the Configuration\r\n");
            Console.Write("  Tool, click the Defaults button and click on the Destination drop-down.\r\n");
            Console.Write("  The available outputs match the selections in this list.\r\n");
            Console.Write("- For dictionary output, the reversal index file needs to be named\r\n");
            Console.Write("  \"FlexRev.xhtml\". this is to maintain consistency with the file naming\r\n");
            Console.Write("  convention used in Pathway.\r\n");
        }

        /// <summary>
        /// Converts USFM to USX.
        /// </summary>
        /// <param name="files">List of files to convert. This can be a single file or wildcard (*) as well.</param>
        /// <returns></returns>
        private static XmlDocument ConvertUsfmToUsx(List<string> files)
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

        /// <summary>
        /// Converts USX file(s) to a single XHTML file.
        /// </summary>
        /// <param name="projInfo"></param>
        /// <param name="files">List of files to convert. This can be a single file or wildcard (*) as well.</param>
        private static void ConvertUsxToXhtml(PublicationInformation projInfo, List<string> files)
        {
            string cssFullPath = Path.Combine(projInfo.ProjectPath, projInfo.ProjectName + ".css");
            StyToCSS styToCss = new StyToCSS();
            styToCss.ConvertStyToCSS(projInfo.ProjectName, cssFullPath);
            projInfo.DefaultCssFileWithPath = cssFullPath;
            string fileName = Path.Combine(projInfo.ProjectPath, projInfo.ProjectName + ".xhtml");
            ParatextPathwayLink paratextPathwayLink = new ParatextPathwayLink(projInfo.ProjectName, projInfo.ProjectName, "zxx", "zxx", "PathwayB");

            XmlDocument scrBooksDoc;
            if (files[0] == "*")
            {
                // wildcard - need to expand the file count to include all .sfm files in this directory
                var usxFiles = Directory.GetFiles(projInfo.ProjectPath, "*.sfm");
                files.Clear(); // should only be 1 item, but just in case...
                foreach (var usxFile in usxFiles)
                {
                    files.Add(Path.GetFileName(usxFile)); // relative file names (no path)
                }
            }
            if (files.Count > 1)
            {
                var docs = new List<XmlDocument>();
                foreach (var file in files)
                {
                    if (File.Exists(Path.Combine(projInfo.ProjectPath, file)))
                    {
                        var xmlDoc = new XmlDocument { XmlResolver = null };
                        xmlDoc.Load(Path.Combine(projInfo.ProjectPath, file));
                        docs.Add(xmlDoc);
                    }
                }
                // need to merge the documents before converting
                scrBooksDoc = paratextPathwayLink.CombineUsxDocs(docs);
            }
            else if (files.Count == 1)
            {
                scrBooksDoc = new XmlDocument { XmlResolver = null };
                scrBooksDoc.Load(Path.Combine(projInfo.ProjectPath, files[0]));
            }
            else
            {
                Console.WriteLine("Unable to convert from USX - no files specified.");
                return;
            }

            if (string.IsNullOrEmpty(scrBooksDoc.InnerText))
            {
                Console.WriteLine("The current book has no content to export.");
                return;
            }
            paratextPathwayLink.ConvertUsxToPathwayXhtmlFile(scrBooksDoc.InnerXml, fileName);
            
        }

    }
}
