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
using System.Reflection;
using Microsoft.Win32;
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
                if (args.Length == 0)
                {
                    Usage();
                    Environment.Exit(0);                    
                }
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
                    // convert from USFM to xhtml
                    UsfmToXhtml(projectInfo, files);
                }
                else if (inFormat == InputFormat.USX)
                {
                    // convert from USX to xhtml
                    UsxToXhtml(projectInfo, files);
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
                //Console.WriteLine("xhtml: " + projectInfo.DefaultXhtmlFileWithPath);
                //Console.WriteLine("css: " + projectInfo.DefaultCssFileWithPath);
                //Console.WriteLine("Reversal:" + projectInfo.IsReversalExist);

                if (projectInfo.DefaultXhtmlFileWithPath == null)
                {
                    Usage();
                    throw new ArgumentException("Missing required option: (DefaultXhtmlFileWithPath).");
                }
                if (projectInfo.DefaultCssFileWithPath == null)
                {
                    Usage();
                    throw new ArgumentException("Missing required option: (DefaultCssFileWithPath).");
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

                //var tpe = new PsExport { Destination = exportType, DataType = projectInfo.ProjectInputType };
                //tpe.Export(projectInfo.ProjectPath);

                Backend.Load(backendPath);
                Common.ShowMessage = false;
                projectInfo.DictionaryOutputName = projectInfo.ProjectName;
                bool result = Backend.Launch(exportType, projectInfo);
                if (result == true)
                {
                    Console.WriteLine("PathwayB: export process succeeded at " + DateTime.Now);
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
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
        /// <param name="files">List of files to convert. This can be a single file or wildcard (*).</param>
        /// <returns></returns>
        private static void UsfmToXhtml(PublicationInformation projInfo, List<string> files)
        {
            var docs = new List<XmlDocument>();

            // try to find the stylesheet associated with this project (*.sty)
            var tmpFiles = Directory.GetFiles(projInfo.ProjectPath, "*.sty");
            string styFile;
            if (tmpFiles.Length == 0)
            {
                // not here - check in the "gather" subdirectory
                tmpFiles = Directory.GetFiles(Path.Combine(projInfo.ProjectPath, "gather"), "*.sty");
                styFile = Path.Combine(Path.Combine(projInfo.ProjectPath, "gather"), tmpFiles[0]);
            }
            else
            {
                styFile = Path.Combine(projInfo.ProjectPath, tmpFiles[0]);
            }
            // Work on the USFM data
            // first convert to USX);
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
            if (files.Count == 0)
            {
                throw new Exception("No files found to convert.");
            }
            foreach (var file in files)
            {
                string filename = Path.Combine(projInfo.ProjectPath, file);
                if (File.Exists(filename))
                {
                    var streamReader = new StreamReader(filename);
                    string usfm = string.Empty;
                    try
                    {
                        usfm = streamReader.ReadToEnd();
                    }
                    catch (OutOfMemoryException ex)
                    {
                        Console.Write(ex.Message);
                    }
                    streamReader.Close();
                    XmlDocument doc = new XmlDocument();
                    using (XmlWriter xmlw = doc.CreateNavigator().AppendChild())
                    {
                        // Convert to USX. Uses reflection to find the ParatextShared DLL (dynamically loaded).
                        try
                        {
                            Assembly asm = Assembly.LoadFrom(Path.Combine(PathwayPath.GetPathwayDir(), "ParatextShared.dll"));
                            // new ScrStylesheet(styFile)
                            Type tScrStylesheet = asm.GetType("Paratext.ScrStylesheet");
                            Object oScrStylesheet = null;
                            if (tScrStylesheet != null)
                            {
                                Object[] args = new object[1];
                                args[0] = styFile;
                                oScrStylesheet = Activator.CreateInstance(tScrStylesheet, args);
                            }
                            // UsfmToUsx.Convert
                            Type tUsfmToUsx = asm.GetType("Paratext.UsfmToUsx");
                            if (tUsfmToUsx != null)
                            {
                                Object[] args = new object[4];
                                args[0] = xmlw;
                                args[1] = usfm;
                                args[2] = oScrStylesheet;
                                args[3] = false;
                                Object oClass = Activator.CreateInstance(tUsfmToUsx, args);
                                Object oResult = tUsfmToUsx.InvokeMember("Convert", BindingFlags.Default | BindingFlags.InvokeMethod, null, oClass, null);
                            }
                        }
                        catch (FileNotFoundException ex)
                        {
                            throw new ArgumentException("USFM output depends on ParatextShared.DLL and NetLoc.DLL. Please make sure these libraries are in the Pathway installation directory.");
                        }
                        catch (FileLoadException ex)
                        {
                            throw new ArgumentException("Unable to load a needed library for USFM output. Details: " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                        // this code can replace the entire block above, if we want to directly link instead of dynamically loading the
                        // ParatextShared dll.
                        //UsfmToUsx converter = new UsfmToUsx(xmlw, usfm, new ScrStylesheet(styFile), false);
                        //converter.Convert();
                        xmlw.Flush();
                    }
                    docs.Add(doc);
                }
            }

            Assembly asmPTSupport = null;

            try
            {

                // load the ParatextSupport DLL dynamically
                asmPTSupport = Assembly.LoadFrom(Path.Combine(PathwayPath.GetPathwayDir(), "ParatextSupport.dll"));

                //StyToCSS styToCss = new StyToCSS();
                //styToCss.StyFullPath = styFile;
                //styToCss.ConvertStyToCSS(Path.Combine(projInfo.ProjectPath, projInfo.ProjectName + ".css"));

                // Convert the stylesheet to css
                // new ScrStylesheet(styFile)
                Type tStyToCSS = asmPTSupport.GetType("SIL.PublishingSolution.StyToCSS");
                Object oScrStylesheet = null;
                if (tStyToCSS != null)
                {
                    // new styToCss
                    oScrStylesheet = Activator.CreateInstance(tStyToCSS);
                    // styToCss.StyFullPath = styFile
                    PropertyInfo piStyFullPath = tStyToCSS.GetProperty("StyFullPath", BindingFlags.Public | BindingFlags.Instance);
                    piStyFullPath.SetValue(oScrStylesheet, styFile, null);
                    // styToCss.ConvertStyToCSS(Path.Combine(projInfo.ProjectPath, projInfo.ProjectName + ".css"));
                    Object[] args = new object[1];
                    args[0] = Path.Combine(projInfo.ProjectPath, projInfo.ProjectName + ".css");
                    Object oResult = tStyToCSS.InvokeMember("ConvertStyToCSS", BindingFlags.Default | BindingFlags.InvokeMethod, null, oScrStylesheet, args);
                }
                projInfo.DefaultCssFileWithPath = Path.Combine(projInfo.ProjectPath, projInfo.ProjectName + ".css");

                // now convert to xhtml
                Type tPPL = asmPTSupport.GetType("SIL.PublishingSolution.ParatextPathwayLink");
                if (tPPL != null)
                {
                    Object[] args = new object[5];
                    args[0] = projInfo.ProjectName;
                    args[1] = projInfo.ProjectName;
                    args[2] = "zxx";
                    args[3] = "zxx";
                    args[4] = "PathwayB";
                    Object oPPL = Activator.CreateInstance(tPPL, args);
                    Object[] argsCombine = new object[1];
                    argsCombine[0] = docs;
                    XmlDocument scrBooksDoc = (XmlDocument)tPPL.InvokeMember
                        ("CombineUsxDocs", BindingFlags.Default | BindingFlags.InvokeMethod, null, oPPL, argsCombine);
                    if (string.IsNullOrEmpty(scrBooksDoc.InnerText))
                    {
                        throw new Exception("No content found to convert.");
                    }
                    Object[] argsConvert = new object[2];
                    argsConvert[0] = scrBooksDoc.InnerXml;
                    argsConvert[1] = Path.Combine(projInfo.ProjectPath, projInfo.ProjectName + ".xhtml");
                    var oRet = tPPL.InvokeMember("ConvertUsxToPathwayXhtmlFile", BindingFlags.Default | BindingFlags.InvokeMethod, null, oPPL, argsConvert);
                }
                // ParatextPathwayLink link = new ParatextPathwayLink(projInfo.ProjectName, projInfo.ProjectName, "zxx", "zxx", "PathwayB");
                // XmlDocument scrBooksDoc = link.CombineUsxDocs(docs);
                // if (string.IsNullOrEmpty(scrBooksDoc.InnerText))
                // {
                //     throw new Exception("No content found to convert.");
                // }
                // link.ConvertUsxToPathwayXhtmlFile(scrBooksDoc.InnerXml, Path.Combine(projInfo.ProjectPath, projInfo.ProjectName + ".xhtml"));
                projInfo.DefaultXhtmlFileWithPath = Path.Combine(projInfo.ProjectPath, projInfo.ProjectName + ".xhtml");

            }
            catch (FileNotFoundException ex)
            {
                throw new ArgumentException("USFM output depends on ParatextSupport.DLL. Please make sure this library is in the Pathway installation directory.");
            }
            catch (FileLoadException ex)
            {
                throw new ArgumentException("Unable to load a needed library for USFM output. Details: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        /// <summary>
        /// Converts USX file(s) to a single XHTML file.
        /// </summary>
        /// <param name="projInfo"></param>
        /// <param name="files">List of files to convert. This can be a single file or wildcard (*).</param>
        private static void UsxToXhtml(PublicationInformation projInfo, List<string> files)
        {
            //// try to find the stylesheet associated with this project (*.sty)
            //var tmpFiles = Directory.GetFiles(projInfo.ProjectPath, "*.sty");
            //string styFile;
            //if (tmpFiles.Length == 0)
            //{
            //    // not here - check in the "gather" subdirectory
            //    tmpFiles = Directory.GetFiles(Path.Combine(projInfo.ProjectPath, "gather"), "*.sty");
            //    styFile = Path.Combine(Path.Combine(projInfo.ProjectPath, "gather"), tmpFiles[0]);
            //}
            //else
            //{
            //    styFile = Path.Combine(projInfo.ProjectPath, tmpFiles[0]);
            //}
            //// Convert the stylesheet to css
            //StyToCSS styToCss = new StyToCSS();
            //styToCss.StyFullPath = styFile;
            //styToCss.ConvertStyToCSS(Path.Combine(projInfo.ProjectPath, projInfo.ProjectName + ".css"));
            //projInfo.DefaultCssFileWithPath = Path.Combine(projInfo.ProjectPath, projInfo.ProjectName + ".css");

            //if (files[0] == "*")
            //{
            //    // wildcard - need to expand the file count to include all .sfm files in this directory
            //    var usxFiles = Directory.GetFiles(projInfo.ProjectPath, "*.sfm");
            //    files.Clear(); // should only be 1 item, but just in case...
            //    foreach (var usxFile in usxFiles)
            //    {
            //        files.Add(Path.GetFileName(usxFile)); // relative file names (no path)
            //    }
            //}
            //if (files.Count == 0)
            //{
            //    throw new Exception("No files found to convert.");
            //}
            //var docs = new List<XmlDocument>();
            //foreach (var file in files)
            //{
            //    if (File.Exists(Path.Combine(projInfo.ProjectPath, file)))
            //    {
            //        var xmlDoc = new XmlDocument {XmlResolver = null};
            //        xmlDoc.Load(Path.Combine(projInfo.ProjectPath, file));
            //        docs.Add(xmlDoc);
            //    }
            //}
            //// convert to xhtml
            //ParatextPathwayLink link = new ParatextPathwayLink(projInfo.ProjectName, projInfo.ProjectName, "zxx",
            //                                                    "zxx", "PathwayB");
            //XmlDocument scrBooksDoc = link.CombineUsxDocs(docs);

            //if (string.IsNullOrEmpty(scrBooksDoc.InnerText))
            //{
            //    throw new Exception("No content found to convert.");
            //}
            //link.ConvertUsxToPathwayXhtmlFile(scrBooksDoc.InnerXml,
            //                                    Path.Combine(projInfo.ProjectPath, projInfo.ProjectName + ".xhtml"));
            //projInfo.DefaultXhtmlFileWithPath = Path.Combine(projInfo.ProjectPath, projInfo.ProjectName + ".xhtml");

        }

        //private string ParatextInstallDir()
        //{
        //    const string PTWUpdateFile = @"PtwUpdateFile";
        //    using (var key = Registry.CurrentUser.OpenSubKey(PTWUpdateFile))
        //    {
        //        using (RegistryKey subkey = key.OpenSubKey(@"shell\Open\command"))
        //        {
        //            if (subkey.GetValue())
        //            if (subkey.GetValue("ProductName").ToString().ToLower().IndexOf(ExecutableName.ToLower()) != -1)
        //            {
        //                return true;
        //            }
        //        }
        //    }

        //}

    }
}
