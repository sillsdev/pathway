// --------------------------------------------------------------------------------------------
// <copyright file="PathwayExport.cs" from='2009' to='2014' company='SIL International'>
//      Copyright ( c ) 2014, SIL International. All Rights Reserved.
//
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed:
//
// <remarks>
//
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.IO;
using SIL.Tool;
using System.Collections.Generic;
using System.Reflection;
using SIL.PublishingSolution;

namespace SIL.PublishingSolution
{
    internal class Program
    {
        private static bool _showHelp;
        private enum InputFormat
        {
            XHTML,
            USFM,
            USX,
            PTBUNDLE
        }

        private enum ExportFormat
        {
            Dictionary,
            Scripture
        }
        private static string exportType;
        private static string exportDirectory;
        private static List<string> files = new List<string>();
        private static bool filledExportType;
        private static bool filledDirectory;
        private static bool filledCss;
        private static PublicationInformation projectInfo;

        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Write("PathwayExport: ");
                Console.WriteLine("Try 'PathwayExport --help' for more information.");
                Console.Read();
                Environment.Exit(0);
            }
            else if (args.Length == 1 && args[0].ToLower() == "--help")
	        {
		        Usage();
		        Console.Read();
		        Environment.Exit(0);
	        }
	        else
	        {
		        projectInfo = new PublicationInformation();
		        ArgumentUsage(args);

		        projectInfo.ProjectPath = exportDirectory;
		        foreach (string f in files)
		        {
			        if (f.ToLower().Contains(".css"))
			        {
				        if (f.ToLower().Contains("flexrev"))
				        {
					        projectInfo.DefaultRevCssFileWithPath = f;
				        }
				        else
				        {
					        projectInfo.DefaultCssFileWithPath = f;
				        }
			        }
			        if (f.ToLower().EndsWith(".xhtml"))
			        {
				        if (f.ToLower().Contains("flexrev"))
				        {
					        projectInfo.IsReversalExist = true;
				        }
				        else
				        {
					        projectInfo.DefaultXhtmlFileWithPath = f;
					        if (f.ToLower().Contains("main"))
						        projectInfo.IsLexiconSectionExist = true;
				        }
			        }
		        }

		        // Find Export Format
		        string settingsPath = Path.Combine(Common.GetAllUserPath(), "StyleSettings.xml");
		        projectInfo.ProjectInputType = Param.GetInputType(settingsPath);

		        if (projectInfo.IsLexiconSectionExist == true)
			        projectInfo.ProjectName = "main";
		        else if (projectInfo.IsReversalExist == true)
			        projectInfo.ProjectName = "flexrev";

		        projectInfo.IsOpenOutput = !Common.Testing;

		        SetFileName();
		        projectInfo.ProjectPath = Path.GetDirectoryName(projectInfo.DefaultXhtmlFileWithPath);
		        LoadParameters(projectInfo.ProjectInputType);

				List<IExportProcess> backend = new List<IExportProcess>();
		        backend = Backend.LoadExportAssembly(Common.AssemblyPath);
		        foreach (IExportProcess lProcess in backend)
		        {

			        if (exportType == "openoffice/libreoffice")
			        {
				        Common._outputType = Common.OutputType.ODT;
				        projectInfo.FinalOutput = "odt";
						lProcess.Export(projectInfo);
				        break;
				        // process = new ExportLibreOffice();
			        }
			        if (exportType == "e-book (.epub)" || exportType == "e-book (epub2 and epub3)")
			        {
				        // process = new Exportepub();
				        Common._outputType = Common.OutputType.EPUB;
						lProcess.Export(projectInfo);
						break;
			        }
			        if (exportType == "pdf (using openoffice/libreoffice)")
			        {
				        projectInfo.FinalOutput = "pdf";
				        projectInfo.OutputExtension = "pdf";
						lProcess.Export(projectInfo);
						break;
				        // process = new ExportLibreOffice();
			        }
			        if (exportType == "pdf (using prince)")
			        {
				        // process = new ExportPdf();
				        Common._outputType = Common.OutputType.PDF;
						lProcess.Export(projectInfo);
						break;
			        }
			        if (exportType == "xelatex")
			        {
				        // process = new ExportXeLaTex();
				        Common._outputType = Common.OutputType.XELATEX;
						lProcess.Export(projectInfo);
						break;
			        }
			        if (exportType == "dictionaryformids")
			        {
				        // process = new ExportDictionaryForMIDs();
				        Common._outputType = Common.OutputType.MOBILE;
						lProcess.Export(projectInfo);
						break;
			        }
			        if (exportType == "indesign")
			        {
				        //  process = new ExportInDesign();
				        Common._outputType = Common.OutputType.IDML;
						lProcess.Export(projectInfo);
						break;
			        }
			        if (exportType == "gobible" || exportType == "go bible")
			        {
				        projectInfo.ProjectName = "Go_Bible";
				        projectInfo.SelectedTemplateStyle = "GoBible";
				        //  process = new ExportGoBible();
				        Common._outputType = Common.OutputType.MOBILE;
						lProcess.Export(projectInfo);
						break;
			        }
			        if (exportType == "sword")
			        {
				        //  process = new ExportSword();
						lProcess.Export(projectInfo);
						break;
			        }
			        if (exportType == "theword/mysword")
			        {
				        // process = new ExportTheWord();
						lProcess.Export(projectInfo);
						break;
			        }
		        }
	        }
        }

		private static void LoadParameters(string inputType)
		{
			Param.LoadSettings();
		}

        private static void ArgumentUsage(string[] args)
        {
            bool argFilledExportType = false;
            bool argFilledDirectory = false;
            bool argFilledFiles = false;
            bool argFilledCss = false;

            int i = 0;
            while (i < args.Length)
            {
                switch (args[i++])
                {
                    case "--target":
                    case "-t":
                        exportType = args[i++];
                        exportType = exportType.Replace("'", "").Trim().ToLower();
                        argFilledExportType = true;
                        if (!CheckExportType())
                        {
                            Console.Write(@"PathwayExport : Unknown Export Type");
                            Console.WriteLine(@"Try 'PathwayExport --help' for more information.");
                            Console.Read();
                            Environment.Exit(0);
                        }
                        break;
                    case "--directory":
                    case "-d":
                        exportDirectory = args[i++];
                        exportDirectory = exportDirectory.Replace("'", "").Replace("\\\\", "\\");
                        argFilledDirectory = true;
                        if (!Directory.Exists(exportDirectory))
                        {
                            Console.Write(@"PathwayExport : Export directory not exists");
                            Console.WriteLine(@"Try 'PathwayExport --help' for more information.");
                            Console.Read();
                            Environment.Exit(0);
                        }
                        projectInfo.ProjectPath = exportDirectory;
                        break;
                    case "--files":
                    case "-f":
                        argFilledFiles = true;
                        i = CaptureFileList(args, i, files);
                        break;
                    case "--css":
                    case "-c":
                        argFilledCss = true;
                        files.Add(args[i++]);
                        break;
					case "--database":
					case "-b":
						Param.DatabaseName = args[i++];
						Param.DatabaseName = Param.DatabaseName.Replace("'", "").Trim();
						break;
					case "--nunit":
		                if (args[i++].ToLower() == "true")
		                {
			                Common.Testing = true;
		                }
		                break;
                }
            }

            CheckArgumentMissing(argFilledExportType, argFilledDirectory, argFilledFiles, argFilledCss);
        }

        /// <summary>
        /// //if (!argFilledExportType || !argFilledDirectory || !argFilledFiles || !argFilledCss)
        /// </summary>
        /// <param name="argFilledExportType"></param>
        /// <param name="argFilledDirectory"></param>
        /// <param name="argFilledFiles"></param>
        /// <param name="argFilledCss"></param>
        private static void CheckArgumentMissing(bool argFilledExportType, bool argFilledDirectory, bool argFilledFiles, bool argFilledCss)
        {
            // If Files to process is missing, then we cannot proceed
            if (!argFilledFiles)
            {
                Console.Write("PathwayExport: Files Missing. Cannot Proceed.");
                Console.WriteLine("Try 'PathwayExport --help' for more information.");
                Console.Read();
                Environment.Exit(0);
            }
            else
            {
                // If target export type is not mentioned,
                if (!argFilledExportType)
                {
                    //  get the default export type from the settings file and store it in the exportType
                    exportType = GetDefaultExportFileFromSettingsFile();
                    // set filledExportType to true
                    filledExportType = true;
                }

                if (!argFilledDirectory)
                {
                    foreach (string f in files)
                    {
                        if (Directory.Exists(Path.GetDirectoryName(f)))
                        {
                            exportDirectory = Path.GetDirectoryName(f);
                            filledDirectory = true;
                            break;
                        }
                    }
                }

                if (!argFilledCss)
                {
                    foreach (string f in files)
                    {
                        if (f.EndsWith(".css"))
                        {
                            filledCss = true;
                            break;
                        }
                    }
                }
            }
            if (!filledCss)
            {
                Console.Write("PathwayExport: CSS Files not mentioned");
                Console.WriteLine("Try 'PathwayExport --help' for more information.");
                Console.Read();
                Environment.Exit(0);
            }
        }

        private static bool CheckExportType()
        {
            bool isCorrectExportType = false;
            switch (exportType)
            {
                case "e-book (.epub)":
                case "e-book (epub2 and epub3)":
                case "gobible":
                case "go bible":
                case "dictionaryformids":
                case "pdf (using openoffice/libreoffice)":
                case "pdf (using prince)":
                case "indesign":
                case "openoffice/libreoffice":
                case "xelatex":
                case "sword":
                case "theword/mysword":
                    isCorrectExportType = true;
                    break;
            }
            return isCorrectExportType;
        }

        private static int CaptureFileList(string[] args, int i, List<string> files)
        {
            // store the files in our internal list for now
            // (single filenames and * will end up as a single element in the list)
            string fname = string.Empty;
            while (args[i].EndsWith(","))
            {
                fname = args[i].Substring(0, args[i].Length - 1);
                files.Add(fname.Replace("\\\\","\\"));
                i++;
            }
            files.Add(args[i++]);
            return i;
        }

        private static void Usage()
        {
            string val = "The usage is PathwayExport[--target<format>][--directory <filePath>] inputfile(s)";
            Console.Write(val);
            val = "   --target | -t <format>    (required) desired output format. Can be one of:\r\n";
            Console.Write(val);
            val = "                             \"E-Book (.epub)\"           .epub format.\r\n";
            Console.Write(val);
            val = "                             \"GoBible\"                  GoBible .jar format.\r\n";
            Console.Write(val);
            val = "                             \"PDF (using OpenOffice/LibreOffice)\" \r\n";
            Console.Write(val);
            val = "                                                          .pdf format.\r\n";
            Console.Write(val);
            val = "                             \"PDF (using Prince)\"       .pdf format.\r\n";
            Console.Write(val);
            val = "                             \"InDesign\"                 .idml format.\r\n";
            Console.Write(val);
            val = "                             \"OpenOffice/LibreOffice\"   .odt format.\r\n";
            Console.Write(val);
            val = "                             \"XeLaTex\"                  .tex format.\r\n";
            Console.Write(val);
            val = "                             \"sword\"                    format.\r\n";
            Console.Write(val);
            val = "                             \"theword/mysword\"          format.\r\n";
            Console.Write(val);
            val = "   --directory | -d <path>   (required) full path to the content.\r\n";
            Console.Write(val);
            val = "   --files | -f {<file>[, <file>] | *}\r\n";
            Console.Write(val);
            val = "                             (required) files to process, or * for all files in\r\n";
            Console.Write(val);
            val = "                             the directory specified by the -d flag.\r\n";
            Console.Write(val);
            val = "   --css | -c                stylesheet file name (required for xhtml only).\r\n";
            Console.Write(val);
            //val = "   --showdialog | -s         Show the Export Through Pathway dialog, and take\r\n";
            //Console.Write(val);
            //val = "                             the values for target format, style, etc. from\r\n";
            //Console.Write(val);
            //val = "                             the user's input on the dialog.\r\n";
            //Console.Write(val);
            //val = "   --launch | -l             launch resulting output in target back end.\r\n";
            //Console.Write(val);
            //val = "   --name | -n               [main] Project name.\r\n\r\n\r\n";
            //Console.Write(val);
            //val = "Examples:\r\n\r\n";
            //Console.Write(val);
            //val = "   PathwayB.exe -d \"D:\\MyProject\" -if usfm -f * -t \"E-Book (.epub)\" \r\n";
            //Console.Write(val);
            //val = "                             -i \"Scripture\" -n \"SEN\" \r\n";
            //Console.Write(val);
            //val = "      Creates an .epub file from the USFM project found in D:\\MyProject.\r\n\r\n";
            //Console.Write(val);
            //val = "   PathwayB.exe -d \"D:\\MyDict\" -if xhtml -c \"D:\\MyDict\\main.css\" \r\n";
            //Console.Write(val);
            //val = "                             -f \"main.xhtml\", \"FlexRev.xhtml\" \r\n";
            //Console.Write(val);
            //val = "                             -t \"E-Book (.epub)\" -i \"Dictionary\" \r\n";
            //Console.Write(val);
            //val = "                             -n \"Sena 3-01\" \r\n";
            //Console.Write(val);
            //val = "      Creates an .epub file from the xhtml dictionary found in D:\\MyDict.\r\n";
            //Console.Write(val);
            //val = "      Both main and reversal index files are included in the output.\r\n\r\n";
            //Console.Write(val);
            //val = "   PathwayB.exe -d \"D:\\Project2\" -if usfm -f * -i \"Scripture\" -n \"SEN\"-s\r\n";
            //Console.Write(val);
            //val = "      Displays the Export Through Pathway dialog, then generates output from\r\n";
            //Console.Write(val);
            //val = "      the USFM project found in D:\\Project2 to the user-specified output \r\n";
            //Console.Write(val);
            //val = "      format and style.\r\n\r\n";
            //Console.Write(val);
            //val = "Notes:\r\n\r\n";
            //Console.Write(val);
            //val = "-  Not all output types may be available, depending on your installation\r\n";
            //Console.Write(val);
            //val = "   Package. To verify the available output types, open the Configuration\r\n";
            //Console.Write(val);
            //val = "   Tool, click the Defaults button and click on the Destination drop-down.\r\n";
            //Console.Write(val);
            //val = "   The available outputs match the selections in this list.\r\n\r\n";
            //Console.Write(val);
            //val = "-  For dictionary output, the reversal index file needs to be named\r\n";
            //Console.Write(val);
            //val = "   \"FlexRev.xhtml\". this is to maintain consistency with the file naming\r\n";
            //Console.Write(val);
            //val = "   convention used in Pathway.\r\n\r\n";
            Console.Write(val);
        }

        private InputFormat GetFileFormat(string format)
        {
            InputFormat inFormat = InputFormat.XHTML;
            switch (format.ToLower())
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
            return inFormat;
        }

        private static string GetEntryAssemblyName()
        {
            string entryAssemblyName = string.Empty;
            if (!(String.IsNullOrEmpty(Convert.ToString(Assembly.GetEntryAssembly()))))
            {
                entryAssemblyName = Assembly.GetEntryAssembly().FullName;
            }
            else
            {
                entryAssemblyName = "configurationtool";
            }

            return entryAssemblyName.Trim().ToLower();
        }

        private static void SetFileName()
        {
            if (projectInfo.ProjectInputType == "Dictionary")
            {
                // dictionary
                // main - if there's a file name of "main" in the list, we'll use it as the default xhtml;
                // if not, we'll use the first item in the list
                int indexMain = files.FindIndex(
                    something => (something.ToLower().EndsWith("main.xhtml"))
                    );
                projectInfo.DefaultXhtmlFileWithPath = (indexMain >= 0)
                                                           ? Common.PathCombine(projectInfo.ProjectPath,
                                                                                files[indexMain])
                                                           : Common.PathCombine(projectInfo.ProjectPath, files[0]);
                // reversal index - needs to be named "flexrev.xhtml"
                // (for compatibility with transforms in Pathway)
                int indexRev = files.FindIndex(
                    something => (something.ToLower().EndsWith("flexrev.xhtml"))
                    );
				projectInfo.IsReversalExist = (indexMain >= 0 && indexRev >= 0);

	            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(projectInfo.DefaultXhtmlFileWithPath);
	            if (fileNameWithoutExtension != null &&
	                (File.Exists(projectInfo.DefaultXhtmlFileWithPath) && fileNameWithoutExtension.ToLower() == "main"))
		            projectInfo.IsLexiconSectionExist = true;

                projectInfo.ProjectFileWithPath = null;
                projectInfo.SwapHeadword = false;
                projectInfo.FromPlugin = true;
	            if (indexMain >= 0 && indexRev >= 0)
	            {
		            projectInfo.IsODM = true;
	            }
				else
	            {
		            if (string.IsNullOrEmpty(projectInfo.DefaultCssFileWithPath))
		            {
			            projectInfo.DefaultCssFileWithPath = projectInfo.DefaultRevCssFileWithPath;
			            projectInfo.DefaultRevCssFileWithPath = null;
		            }
	            }
	            //projectInfo.DefaultRevCssFileWithPath =
                //    Common.PathCombine(Path.GetDirectoryName(projectInfo.DefaultXhtmlFileWithPath), "FlexRev.css");
                projectInfo.DictionaryPath = projectInfo.ProjectPath;
            }
            else if (projectInfo.ProjectInputType == "Scripture")
            {
                if (projectInfo.DefaultXhtmlFileWithPath == null)
                {
                    projectInfo.DefaultXhtmlFileWithPath = Common.PathCombine(projectInfo.ProjectPath, files[0]);
                }
                projectInfo.ProjectFileWithPath = null;
                projectInfo.DictionaryPath = projectInfo.ProjectPath;
                projectInfo.FromPlugin = true;
                projectInfo.IsLexiconSectionExist = false;
				string dictionaryName = Common.PathCombine(Path.GetDirectoryName(projectInfo.DefaultXhtmlFileWithPath), Path.GetFileNameWithoutExtension(projectInfo.DefaultXhtmlFileWithPath));
				projectInfo.DictionaryOutputName = dictionaryName;
				projectInfo.ProjectName = Path.GetFileNameWithoutExtension(projectInfo.DefaultXhtmlFileWithPath);

            }
        }

        /// <summary>
        /// Gets the default export type from the Dictionary / Scripture Settings File
        /// </summary>
        /// <returns></returns>
        private static string GetDefaultExportFileFromSettingsFile()
        {
            return "openoffice/libreoffice";
        }


    }
}
