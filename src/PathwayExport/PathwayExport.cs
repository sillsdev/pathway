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
				Environment.Exit(-1);
			}
			else if (args.Length == 1 && args[0].ToLower() == "--help")
			{
				Usage();
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
				if (Param.DatabaseName == "DatabaseName") Common.CallerSetting = new CallerSetting {SettingsFullPath = projectInfo.DefaultXhtmlFileWithPath};
				Common.LoadHyphenationSettings();

				List<IExportProcess> backend = new List<IExportProcess>();
				backend = Backend.LoadExportAssembly(Common.AssemblyPath);
				foreach (IExportProcess lProcess in backend)
				{
					if (exportType == "openoffice/libreoffice" && lProcess.ExportType.ToLower() == "openoffice/libreoffice")
					{
						Common._outputType = Common.OutputType.ODT;
						projectInfo.FinalOutput = "odt";
						lProcess.Export(projectInfo);
						Environment.Exit(0);
						// process = new ExportLibreOffice();
					}
					if (lProcess.ExportType.ToLower() == "e-book (epub2 and epub3)" && (exportType == "e-book (.epub)" || exportType == "e-book (epub2 and epub3)"))
					{
						// process = new Exportepub();
						Common._outputType = Common.OutputType.EPUB;
						lProcess.Export(projectInfo);
						Environment.Exit(0);
					}
					if (lProcess.ExportType.ToLower() == "browser (html5)" && exportType == "browser (html5)")
					{
						// process = new Exportepub();
						Common._outputType = Common.OutputType.HTML5;
						lProcess.Export(projectInfo);
						Environment.Exit(0);
					}
					if (exportType == "pdf (using openoffice/libreoffice)" && lProcess.ExportType.ToLower() == "openoffice/libreoffice")
					{
						projectInfo.FinalOutput = "pdf";
						projectInfo.OutputExtension = "pdf";
						lProcess.Export(projectInfo);
						Environment.Exit(0);
						// process = new ExportLibreOffice();
					}
					if (exportType == "pdf (using prince)" && lProcess.ExportType.ToLower() == "pdf (using prince)")
					{
						// process = new ExportPdf();
						Common._outputType = Common.OutputType.PDF;
						lProcess.Export(projectInfo);
						Environment.Exit(0);
					}
					if (exportType == "xelatex" && lProcess.ExportType.ToLower() == "xelatex")
					{
						// process = new ExportXeLaTex();
						Common._outputType = Common.OutputType.XELATEX;
						lProcess.Export(projectInfo);
						Environment.Exit(0);
					}
					if (exportType == "dictionaryformids" && lProcess.ExportType.ToLower() == "dictionaryformids")
					{
						// process = new ExportDictionaryForMIDs();
						Common._outputType = Common.OutputType.MOBILE;
						lProcess.Export(projectInfo);
						Environment.Exit(0);
					}
					if (exportType == "indesign" && lProcess.ExportType.ToLower() == "indesign")
					{
						//  process = new ExportInDesign();
						Common._outputType = Common.OutputType.IDML;
						lProcess.Export(projectInfo);
						Environment.Exit(0);
					}
					if (lProcess.ExportType.ToLower() == "go bible" && (exportType == "gobible" || exportType == "go bible"))
					{
						projectInfo.ProjectName = "Go_Bible";
						projectInfo.SelectedTemplateStyle = "GoBible";
						//  process = new ExportGoBible();
						Common._outputType = Common.OutputType.MOBILE;
						lProcess.Export(projectInfo);
						Environment.Exit(0);
					}
					if (exportType == "sword" && lProcess.ExportType.ToLower() == "sword")
					{
						//  process = new ExportSword();
						lProcess.Export(projectInfo);
						Environment.Exit(0);
					}
					if (exportType == "theword/mysword" && lProcess.ExportType.ToLower() == "theword/mysword")
					{
						// process = new ExportTheWord();
						lProcess.Export(projectInfo);
						Environment.Exit(0);
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
                            Console.WriteLine(string.Join(" ", args));
                            Console.Write(@"PathwayExport : Unknown Export Target");
                            Console.WriteLine(@"Try 'PathwayExport --help' for more information.");
                            Environment.Exit(-2);
                        }
                        break;
                    case "--directory":
                    case "-d":
                        exportDirectory = args[i++];
                        exportDirectory = exportDirectory.Replace("'", "").Replace("\\\\", "\\");
                        argFilledDirectory = true;
                        if (!Directory.Exists(exportDirectory))
                        {
							Console.WriteLine(string.Join(" ", args));
							Console.Write(@"PathwayExport : Export directory not exists");
                            Console.WriteLine(@"Try 'PathwayExport --help' for more information.");
                            Environment.Exit(-3);
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
						Common.databaseName = Param.DatabaseName.Replace("'", "").Trim();
						break;
					case "--nunit":
		                if (args[i++].ToLower() == "true")
		                {
			                Common.Testing = true;
		                }
		                break;
                }
            }

            CheckArgumentMissing(argFilledExportType, argFilledDirectory, argFilledFiles, argFilledCss, args);
        }

        /// <summary>
        /// //if (!argFilledExportType || !argFilledDirectory || !argFilledFiles || !argFilledCss)
        /// </summary>
        /// <param name="argFilledExportType"></param>
        /// <param name="argFilledDirectory"></param>
        /// <param name="argFilledFiles"></param>
        /// <param name="argFilledCss"></param>
        private static void CheckArgumentMissing(bool argFilledExportType, bool argFilledDirectory, bool argFilledFiles, bool argFilledCss, string[] args)
        {
            // If Files to process is missing, then we cannot proceed
            if (!argFilledFiles)
            {
				Console.WriteLine(string.Join(" ", args));
				Console.Write("PathwayExport: Files Missing. Cannot Proceed.");
                Console.WriteLine("Try 'PathwayExport --help' for more information.");
                Environment.Exit(-4);
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
				Console.WriteLine(string.Join(" ", args));
				Console.Write("PathwayExport: CSS Files not mentioned");
                Console.WriteLine("Try 'PathwayExport --help' for more information.");
                Environment.Exit(-5);
            }
        }

        private static bool CheckExportType()
        {
            bool isCorrectExportType = false;
            switch (exportType)
            {
                case "e-book (.epub)":
                case "e-book (epub2 and epub3)":
				case "browser (html5)":
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
