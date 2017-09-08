// --------------------------------------------------------------------------------------------
// <copyright file="PathwayB.cs" from='2009' to='2014' company='SIL International'>
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
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;
using System.Xml.Xsl;
using ICSharpCode.SharpZipLib.Zip;
using SIL.Tool;
using System.Collections.Generic;

namespace SIL.PublishingSolution
{
    internal class Program
    {
        public enum InputFormat
        {
            XHTML,
            USFM,
            USX,
            PTBUNDLE
        }

        private static void Main(string[] args)
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


            var exportType = "OpenOffice/LibreOffice";
            bool bOutputSpecified = false;
            var files = new List<string>();
            bool bShowDialog = false;
            try
            {
                int i = 0;
                if (args.Length == 0)
                {
                    Usage();
                    Environment.Exit(0);
                }
                while (i < args.Length)
                {
                    i = ProcessExportType(args, i, projectInfo, files, ref inFormat, ref bShowDialog, ref exportType, ref bOutputSpecified);
                }

				Common.Testing = !projectInfo.IsOpenOutput;

                SettingProcessExportFile(projectInfo, files);
				Common.SaveInputType(projectInfo.ProjectInputType);
                Common.ProgBase = Common.GetPSApplicationPath();
                // load settings from the settings file
	            Param.SetLoadType = projectInfo.ProjectInputType;
                Param.LoadSettings();
                Param.Value[Param.InputType] = projectInfo.ProjectInputType;
                Param.LoadSettings();
                if (bOutputSpecified)
                {
                    // the user has specified an output -- update the settings so we export to that output
                    Param.SetValue(Param.PrintVia, exportType);
                    Param.Write();
                }

                // if the caller wants to display the Export Through Pathway dialog, do it now.
                if (bShowDialog)
                {
                    var dlg = new ExportThroughPathway();
                    dlg.InputType = projectInfo.ProjectInputType;
                    dlg.DatabaseName = projectInfo.ProjectName;
                    if (dlg.ShowDialog() == DialogResult.Yes)
                    {
                        exportType = dlg.Format;
                    }
                    else
                    {
                        // cancel export and exit out of PathwayB
                        Environment.Exit(0);
                    }
                }

                // run headless from the command line
                ProcessInputFormat(inFormat, files, projectInfo);

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

                if (projectInfo.ProjectInputType.ToLower() == "dictionary")
                {
                    Param.SetValue(Param.ReversalIndex, projectInfo.IsReversalExist ? "True" : "False");
                    Param.SetValue(Param.InputType, projectInfo.ProjectInputType);
                    Param.Write();
                }

                var tpe = new PsExport { Destination = Param.Value[Param.PrintVia], DataType = projectInfo.ProjectInputType };
                tpe.ProgressBar = null;
				tpe.Export(projectInfo.DefaultXhtmlFileWithPath);
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

        private static void SettingProcessExportFile(PublicationInformation projectInfo, List<string> files)
        {
            if (string.IsNullOrEmpty(projectInfo.DefaultXhtmlFileWithPath) && files.Count > 0)
            {
                projectInfo.DefaultXhtmlFileWithPath = files[0];
            }
            if (!string.IsNullOrEmpty(projectInfo.ProjectPath))
            {
                if (!string.IsNullOrEmpty(projectInfo.DefaultXhtmlFileWithPath) &&
                    !Path.IsPathRooted(projectInfo.DefaultXhtmlFileWithPath))
                {
                    projectInfo.DefaultXhtmlFileWithPath = Common.PathCombine(projectInfo.ProjectPath,
                                                                              projectInfo.DefaultXhtmlFileWithPath);
                }
                if (!string.IsNullOrEmpty(projectInfo.DefaultCssFileWithPath) &&
                    !Path.IsPathRooted(projectInfo.DefaultCssFileWithPath))
                {
                    projectInfo.DefaultRevCssFileWithPath = Common.PathCombine(projectInfo.ProjectPath,
                                                                               projectInfo.DefaultCssFileWithPath);
                }
                for (int n = 0; n < files.Count; n++)
                {
                    if (!Path.IsPathRooted(files[n]))
                    {
                        files[n] = Common.PathCombine(projectInfo.ProjectPath, files[n]);
                    }
                }
            }
        }

        private static void ProcessInputFormat(InputFormat inFormat, List<string> files, PublicationInformation projectInfo)
        {
            if (inFormat == InputFormat.PTBUNDLE)
            {
                var zf = new FastZip();
                zf.ExtractZip(files[0], projectInfo.ProjectPath, ".*");
                var metadata = Common.DeclareXMLDocument(false);
                metadata.Load(Common.PathCombine(projectInfo.ProjectPath, "metadata.xml"));
                var titleNode = metadata.SelectSingleNode("//bookList[@id='default']/name");
                Param.UpdateTitleMetadataValue(Param.Title, titleNode.InnerText, false);
                var descriptionNode = metadata.SelectSingleNode("//bookList[@id='default']//range");
                Param.UpdateMetadataValue(Param.Description, descriptionNode.InnerText);
                var rightsHolderNode = metadata.SelectSingleNode("//rightsHolder");
                Param.UpdateMetadataValue(Param.Publisher, rightsHolderNode.InnerText);
                var copyrightNode = metadata.SelectSingleNode("//statement");
                Param.UpdateMetadataValue(Param.CopyrightHolder, copyrightNode.InnerText);
                var bookNodes = metadata.SelectNodes("//bookList[@id='default']//book");
                var usxFolder = Common.PathCombine(projectInfo.ProjectPath, "USX");
                var xmlText = new StringBuilder(@"<usx version=""2.0"">");
                var oneBook = Common.DeclareXMLDocument(false);
                foreach (XmlElement bookNode in bookNodes)
                {
                    oneBook.Load(Common.PathCombine(usxFolder, bookNode.SelectSingleNode("@code").InnerText + ".usx"));
                    xmlText.Append(oneBook.DocumentElement.InnerXml);
                    oneBook.RemoveAll();
                }
                xmlText.Append(@"</usx>" + "\r\n");
                var fileName = Common.PathCombine(projectInfo.ProjectPath, titleNode.InnerText + ".xhtml");
                var databaseName = metadata.SelectSingleNode("//identification/abbreviation");
				Common.CallerSetting = new CallerSetting(databaseName.InnerText);
                var linkParam = new Dictionary<string, object>();
                linkParam["dateTime"] = DateTime.Now.ToShortDateString();
                linkParam["user"] = "ukn";
                var wsNode = metadata.SelectSingleNode("//language/iso");
                linkParam["ws"] = wsNode.InnerText;
                linkParam["userWs"] = wsNode.InnerText;
                var nameNode = metadata.SelectSingleNode("//identification/description");
                linkParam["projName"] = nameNode.InnerText;
                var langNameNode = metadata.SelectSingleNode("//language/name");
                linkParam["langInfo"] = string.Format("{0}:{1}", wsNode.InnerText, langNameNode.InnerText);
                ConvertUsx2Xhtml(databaseName, linkParam, xmlText, fileName);
                projectInfo.DefaultXhtmlFileWithPath = fileName;
                var ptxStyle2Css = new XslCompiledTransform();
                ptxStyle2Css.Load(XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream(
                    "PathwayB.ptx2css.xsl")));
                projectInfo.DefaultCssFileWithPath = Common.PathCombine(projectInfo.ProjectPath, titleNode.InnerText + ".css");
                var writer = new XmlTextWriter(projectInfo.DefaultCssFileWithPath, Encoding.UTF8);
                ptxStyle2Css.Transform(Common.PathCombine(projectInfo.ProjectPath, "styles.xml"), null, writer);
                writer.Close();
                metadata.RemoveAll();
                inFormat = InputFormat.XHTML;
            }
            if (inFormat == InputFormat.USFM)
            {
                // convert from USFM to xhtml
	            Common.CallerSetting = new CallerSetting {SettingsFullPath = Path.Combine(projectInfo.ProjectPath, "USX")};
				UsfmToXhtml(projectInfo, files);
            }
            else if (inFormat == InputFormat.USX)
            {
				// convert from USX to xhtml
				Common.CallerSetting = new CallerSetting { SettingsFullPath = Path.Combine(projectInfo.ProjectPath, "USX") };
				UsxToXhtml(projectInfo, files);
            }
            else if (inFormat == InputFormat.XHTML)
            {
                SetFileName(projectInfo, files);
            }
        }

        private static int ProcessExportType(string[] args, int i, PublicationInformation projectInfo, List<string> files,
                                             ref InputFormat inFormat, ref bool bShowDialog, ref string exportType,
                                             ref bool bOutputSpecified)
        {
            switch (args[i++])
            {
                case "--directory":
                case "-d":
                    projectInfo.ProjectPath = args[i++];
                    break;
                case "--files":
                case "-f":
					i = CaptureFileList(args, i, files, projectInfo);
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
                case "--showdialog":
                case "-s":
                    bShowDialog = true;
                    break;
                case "--css":
                case "-c":
                    projectInfo.DefaultCssFileWithPath = args[i++];
                    break;
                case "--target":
                case "-t":
                    //Note: If export type is more than one word, quotes must be used
                    exportType = args[i++];
                    bOutputSpecified = true;
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
					i = CaptureFileList(args, --i, files, projectInfo);
                    if (files.Count > 0)
                    {
                        var lcName = files[0].ToLower();
                        if (lcName.EndsWith(".zip"))
                        {
                            inFormat = SetDefaultsBasedOnInputFormat(InputFormat.PTBUNDLE, projectInfo, lcName);
                            break;
                        }
                        if (lcName.EndsWith(".xhtml"))
                        {
                            inFormat = SetDefaultsBasedOnInputFormat(InputFormat.XHTML, projectInfo, lcName, "Dictionary");
                            break;
                        }
                        if (lcName.EndsWith(".usx"))
                        {
                            inFormat = SetDefaultsBasedOnInputFormat(InputFormat.USX, projectInfo, lcName);
                            break;
                        }
                        if (lcName.EndsWith(".usfm") || lcName.EndsWith(".sfm"))
                        {
                            inFormat = SetDefaultsBasedOnInputFormat(InputFormat.USFM, projectInfo, lcName);
                            break;
                        }
                    }
                    Usage();
                    throw new ArgumentException("Invalid Command Line Argument: " + args[i]);
            }
            return i;
        }

        private static void SetFileName(PublicationInformation projectInfo, List<string> files)
        {
            if (projectInfo.ProjectInputType == "Dictionary")
            {
                // dictionary
                // main - if there's a file name of "main" in the list, we'll use it as the default xhtml;
                // if not, we'll use the first item in the list
                int index = files.FindIndex(
                    something => (something.ToLower().Equals("main.xhtml"))
                    );
                projectInfo.DefaultXhtmlFileWithPath = (index >= 0)
                                                           ? Common.PathCombine(projectInfo.ProjectPath,
                                                                                files[index])
                                                           : Common.PathCombine(projectInfo.ProjectPath, files[0]);
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
                projectInfo.DefaultRevCssFileWithPath =
                    Common.PathCombine(Path.GetDirectoryName(projectInfo.DefaultXhtmlFileWithPath), "FlexRev.css");
                projectInfo.DictionaryPath = Path.GetDirectoryName(projectInfo.ProjectPath);
				DataCreator.Creator = DataCreator.CreatorProgram.FieldWorks;
				Common.CallerSetting = new CallerSetting {SettingsFullPath = projectInfo.ProjectFileWithPath};
            }
            else if (projectInfo.ProjectInputType == "Scripture")
            {
                if (projectInfo.DefaultXhtmlFileWithPath == null)
                {
                    projectInfo.DefaultXhtmlFileWithPath = Common.PathCombine(projectInfo.ProjectPath, files[0]);
                }
				DataCreator.Creator = DataCreator.CreatorProgram.Paratext7;
				Common.CallerSetting = new CallerSetting {SettingsFullPath = projectInfo.DefaultXhtmlFileWithPath};
            }
        }

        private static void ConvertUsx2Xhtml(XmlNode databaseName, Dictionary<string, object> linkParam, StringBuilder xmlText, string fileName)
        {
            try
            {
                // load the ParatextSupport DLL dynamically
	            string paratextSupportDLL = Common.PathCombine(Common.AssemblyPath, "ParatextSupport.dll");

				if (!File.Exists(paratextSupportDLL))
				{
					paratextSupportDLL = Path.GetDirectoryName(Common.AssemblyPath);
					paratextSupportDLL = Common.PathCombine(paratextSupportDLL, "ParatextSupport.dll");
				}

				Assembly asmPtSupport = Assembly.LoadFrom(paratextSupportDLL);
                Type tParatextPathwayLink = asmPtSupport.GetType("SIL.PublishingSolution.ParatextPathwayLink");
                Object objParatextPathwayLink = null;
                if (tParatextPathwayLink != null)
                {
                    Object[] instanceArgs = new object[2];
                    instanceArgs[0] = databaseName.InnerText;
                    instanceArgs[1] = linkParam;
                    objParatextPathwayLink = Activator.CreateInstance(tParatextPathwayLink, instanceArgs);
                    Object[] args = new object[2];
                    args[0] = xmlText.ToString();
                    args[1] = fileName;
                    tParatextPathwayLink.InvokeMember("ConvertUsxToPathwayXhtmlFile", BindingFlags.Default | BindingFlags.InvokeMethod, null, objParatextPathwayLink, args);
                }
            }
            catch (Exception)
            {
            }

        }

        private static InputFormat SetDefaultsBasedOnInputFormat(InputFormat inFormat, PublicationInformation projectInfo, string lcName)
        {
            return SetDefaultsBasedOnInputFormat(inFormat, projectInfo, lcName, "Scripture");
        }
        private static InputFormat SetDefaultsBasedOnInputFormat(InputFormat inFormat, PublicationInformation projectInfo, string lcName, string inputType)
        {
            if (string.IsNullOrEmpty(projectInfo.ProjectPath))
            {
                projectInfo.ProjectPath = Path.GetDirectoryName(lcName);
            }
            projectInfo.ProjectInputType = inputType;
            return inFormat;
        }

		private static int CaptureFileList(string[] args, int i, List<string> files, PublicationInformation projectInfo)
        {

	        if (args[i] == "*")
	        {
		        // wildcard - need to expand the file count to include all .sfm files in this directory
		        var sfmFiles = Directory.GetFiles(projectInfo.ProjectPath, "*.sfm");
		        files.Clear(); // should only be 1 item, but just in case...
				foreach (var sfmFile in sfmFiles)
		        {
					files.Add(Path.GetFileName(sfmFile)); // relative file names (no path)
		        }
		        i++;
		        return i;
	        }
	        // store the files in our internal list for now
            // (single filenames and * will end up as a single element in the list)
            while (args[i].EndsWith(","))
            {
                files.Add(args[i].Substring(0, args[i].Length - 1));
                i++;
            }

            files.Add(args[i++]);
            return i;
        }

        private static void Usage()
        {
            string val = "Usage: PathwayB [Options]";
            Console.Write(val);
            val = "where options include:\r\n";
            Console.Write(val);
            val = "   --input | -i <type>       (required) type of content. Can be one of:\r\n";
            Console.Write(val);
            val = "                             Dictionary   Dictionary content.\r\n";
            Console.Write(val);
            val = "                             Scripture    Scripture content.\r\n";
            Console.Write(val);
            val = "   --inputformat | -if <format>\r\n";
            Console.Write(val);
            val = "                             (required) content input format. Can be one of:\r\n";
            Console.Write(val);
            val = "                             usfm   Unified Standard Format Marker content.\r\n";
            Console.Write(val);
            val = "                             usx    USX content from Paratext 7.x.\r\n";
            Console.Write(val);
            val = "                             xhtml  XHTML from FieldWorks (FLEX or TE).\r\n";
            Console.Write(val);
            val = "                             ptb    Paratext bundle from Paratext 7.3.\r\n";
            Console.Write(val);
            val = "   --directory | -d <path>   (required) full path to the content.\r\n";
            Console.Write(val);
            val = "   --files | -f {<file>[, <file>] | *}\r\n";
            Console.Write(val);
            val = "                             (required) files to process, or * for all files in\r\n";
            Console.Write(val);
            val = "                             the directory specified by the -d flag.\r\n";
            Console.Write(val);
            val = "   --target | -t <format>    (required) desired output format. Can be one of:\r\n";
            Console.Write(val);
            val = "                             \"E-Book (.epub)\"           .epub format.\r\n";
            Console.Write(val);
            val = "                             \"Go Bible\"                 Go Bible .jar format.\r\n";
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
            val = "   --css | -c                stylesheet file name (required for xhtml only).\r\n";
            Console.Write(val);
            val = "   --showdialog | -s         Show the Export Through Pathway dialog, and take\r\n";
            Console.Write(val);
            val = "                             the values for target format, style, etc. from\r\n";
            Console.Write(val);
            val = "                             the user's input on the dialog.\r\n";
            Console.Write(val);
            val = "   --launch | -l             launch resulting output in target back end.\r\n";
            Console.Write(val);
            val = "   --name | -n               [main] Project name.\r\n\r\n\r\n";
            Console.Write(val);
            val = "Examples:\r\n\r\n";
            Console.Write(val);
            val = "   PathwayB.exe -d \"D:\\MyProject\" -if usfm -f * -t \"E-Book (.epub)\" \r\n";
            Console.Write(val);
            val = "                             -i \"Scripture\" -n \"SEN\" \r\n";
            Console.Write(val);
            val = "      Creates an .epub file from the USFM project found in D:\\MyProject.\r\n\r\n";
            Console.Write(val);
            val = "   PathwayB.exe -d \"D:\\MyDict\" -if xhtml -c \"D:\\MyDict\\main.css\" \r\n";
            Console.Write(val);
            val = "                             -f \"main.xhtml\", \"FlexRev.xhtml\" \r\n";
            Console.Write(val);
            val = "                             -t \"E-Book (.epub)\" -i \"Dictionary\" \r\n";
            Console.Write(val);
            val = "                             -n \"Sena 3-01\" \r\n";
            Console.Write(val);
            val = "      Creates an .epub file from the xhtml dictionary found in D:\\MyDict.\r\n";
            Console.Write(val);
            val = "      Both main and reversal index files are included in the output.\r\n\r\n";
            Console.Write(val);
            val = "   PathwayB.exe -d \"D:\\Project2\" -if usfm -f * -i \"Scripture\" -n \"SEN\"-s\r\n";
            Console.Write(val);
            val = "      Displays the Export Through Pathway dialog, then generates output from\r\n";
            Console.Write(val);
            val = "      the USFM project found in D:\\Project2 to the user-specified output \r\n";
            Console.Write(val);
            val = "      format and style.\r\n\r\n";
            Console.Write(val);
            val = "Notes:\r\n\r\n";
            Console.Write(val);
            val = "-  Not all output types may be available, depending on your installation\r\n";
            Console.Write(val);
            val = "   Package. To verify the available output types, open the Configuration\r\n";
            Console.Write(val);
            val = "   Tool, click the Defaults button and click on the Destination drop-down.\r\n";
            Console.Write(val);
            val = "   The available outputs match the selections in this list.\r\n\r\n";
            Console.Write(val);
            val = "-  For dictionary output, the reversal index file needs to be named\r\n";
            Console.Write(val);
            val = "   \"FlexRev.xhtml\". this is to maintain consistency with the file naming\r\n";
            Console.Write(val);
            val = "   convention used in Pathway.\r\n\r\n";
            Console.Write(val);
        }

        /// <summary>
        /// Converts USFM to USX.
        /// </summary>
        /// <param name="projInfo">contains path to Paratext project</param>
        /// <param name="files">List of files to convert. This can be a single file or wildcard (*).</param>
        private static void UsfmToXhtml(PublicationInformation projInfo, List<string> files)
        {
            var docs = new List<XmlDocument>();

            // try to find the stylesheet associated with this project (*.sty)
            var tmpFiles = Directory.GetFiles(projInfo.ProjectPath, "*.sty");
            string styFile = null;
            if (tmpFiles.Length == 0)
            {
                // not here - check in the "gather" subdirectory
                if (Directory.Exists(Common.PathCombine(projInfo.ProjectPath, "gather")))
                // edb 3/30/12 - make sure gather subdirectory exists
                {
                    tmpFiles = Directory.GetFiles(Common.PathCombine(projInfo.ProjectPath, "gather"), "*.sty");
                    if (tmpFiles.Length > 0)
                    {
                        styFile = Common.PathCombine(Common.PathCombine(projInfo.ProjectPath, "gather"), tmpFiles[0]);
                    }
                }
            }
            else
            {
                styFile = Common.PathCombine(projInfo.ProjectPath, tmpFiles[0]);
            }
            if (styFile == null)
            {
                // edb 3/30/12 - no usfm stylesheet (*.sty) found - use the default.sty file
                string defaultStyPath = Common.FromRegistry("default.sty");
                var targetStyPath = Common.PathCombine(projInfo.ProjectPath, "default.sty");
                File.Copy(defaultStyPath, targetStyPath, true);
                styFile = targetStyPath;
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
                string filepattern = Path.IsPathRooted(file)? file: Common.PathCombine(projInfo.ProjectPath, file);
                foreach (string filename in Directory.GetFiles(Path.GetDirectoryName(filepattern), Path.GetFileName(filepattern)))
                {
                    StringWriter stringWriter = new StringWriter();
                    XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
                    XmlDocument doc = new XmlDocument();
                    try
                    {
                        // load the ParatextSupport DLL dynamically
						// load the ParatextSupport DLL dynamically
						string paratextSupportDLL = Common.PathCombine(Common.AssemblyPath, "ParatextSupport.dll");

						if (!File.Exists(paratextSupportDLL))
						{
							paratextSupportDLL = Path.GetDirectoryName(Common.AssemblyPath);
							paratextSupportDLL = Common.PathCombine(paratextSupportDLL, "ParatextSupport.dll");
						}

                        Assembly asmPTSupport = Assembly.LoadFrom(paratextSupportDLL);
                        Type tSfmToUsx = asmPTSupport.GetType("SIL.PublishingSolution.SfmToUsx");
                        Object objSFMtoUsx = null;
                        if (tSfmToUsx != null)
                        {
                            objSFMtoUsx = Activator.CreateInstance(tSfmToUsx);
                            Object[] args = new object[3];
                            args[0] = xmlTextWriter;
                            args[1] = filename;
                            args[2] = filename.Replace(".SFM", ".usx");
                            tSfmToUsx.InvokeMember("ConvertSfmToUsx", BindingFlags.Default | BindingFlags.InvokeMethod, null, objSFMtoUsx, args);
                        }
                    }
                    catch (Exception)
                    {
                    }
                    doc.LoadXml(stringWriter.ToString());
                    xmlTextWriter.Flush();
                    xmlTextWriter.Close();
                    stringWriter.Close();

                    // If we got anything, add it to the list.
                    if (doc.HasChildNodes)
                    {
                        docs.Add(doc);
                    }
                }
            }
            if (docs.Count == 0)
            {
                throw new Exception("Unable to convert USFM documents to Pathway's internal XHTML format.");
            }

            try
            {
				string paratextSupportDLL = Common.PathCombine(Common.AssemblyPath, "ParatextSupport.dll");

				if (!File.Exists(paratextSupportDLL))
				{
					paratextSupportDLL = Path.GetDirectoryName(Common.AssemblyPath);
					paratextSupportDLL = Common.PathCombine(paratextSupportDLL, "ParatextSupport.dll");
				}

                // load the ParatextSupport DLL dynamically
                Assembly asmPTSupport =
					Assembly.LoadFrom(paratextSupportDLL);

                // Convert the stylesheet to css
                // new ScrStylesheet(styFile)
                Type tStyToCSS = asmPTSupport.GetType("SIL.PublishingSolution.StyToCss");
                Object oScrStylesheet = null;
                if (tStyToCSS != null)
                {
                    // new styToCss
                    oScrStylesheet = Activator.CreateInstance(tStyToCSS);
                    // styToCss.StyFullPath = styFile
                    PropertyInfo piStyFullPath = tStyToCSS.GetProperty("StyFullPath", BindingFlags.Public | BindingFlags.Instance);
                    piStyFullPath.SetValue(oScrStylesheet, styFile, null);
                    Object[] args = new object[1];
                    args[0] = Common.PathCombine(projInfo.ProjectPath, projInfo.ProjectName + ".css");
                    tStyToCSS.InvokeMember("ConvertStyToCss", BindingFlags.Default | BindingFlags.InvokeMethod, null, oScrStylesheet, args);
                }
                projInfo.DefaultCssFileWithPath = Common.PathCombine(projInfo.ProjectPath, projInfo.ProjectName + ".css");

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
                    Object[] argsCombine = new object[2];
                    argsCombine[0] = docs;
                    argsCombine[1] = Param.PrintVia;
                    XmlDocument scrBooksDoc = (XmlDocument)tPPL.InvokeMember
                                                                ("CombineUsxDocs",
                                                                 BindingFlags.Default | BindingFlags.InvokeMethod,
                                                                 null, oPPL, argsCombine);
                    if (string.IsNullOrEmpty(scrBooksDoc.InnerText))
                    {
                        throw new Exception("Internal error: unable to combine Usx documents.");
                    }
                    Object[] argsConvert = new object[2];
                    argsConvert[0] = scrBooksDoc.InnerXml;
                    argsConvert[1] = Common.PathCombine(projInfo.ProjectPath, projInfo.ProjectName + ".xhtml");
                    tPPL.InvokeMember("ConvertUsxToPathwayXhtmlFile",
                                                 BindingFlags.Default | BindingFlags.InvokeMethod, null, oPPL,
                                                 argsConvert);
                }
                projInfo.DefaultXhtmlFileWithPath = Common.PathCombine(projInfo.ProjectPath,
                                                                 projInfo.ProjectName + ".xhtml");
            }
            catch (FileNotFoundException)
            {
                throw new ArgumentException(
                    "USFM output depends on ParatextSupport.DLL. Please make sure this library is in the Pathway installation directory.");
            }
            catch (FileLoadException ex)
            {
                throw new ArgumentException("Unable to load a needed library for USFM output. Details: " +
                                            ex.Message);
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
            // try to find the stylesheet associated with this project (*.sty)
            var tmpFiles = Directory.GetFiles(projInfo.ProjectPath, "*.sty");
            string styFile;
            if (tmpFiles.Length == 0)
            {
                // not here - check in the "gather" subdirectory
                tmpFiles = Directory.GetFiles(Common.PathCombine(projInfo.ProjectPath, "gather"), "*.sty");
                styFile = Common.PathCombine(Common.PathCombine(projInfo.ProjectPath, "gather"), tmpFiles[0]);
            }
            else
            {
                styFile = Common.PathCombine(projInfo.ProjectPath, tmpFiles[0]);
            }

            try
            {
				string paratextSupportDLL = Common.PathCombine(Common.AssemblyPath, "ParatextSupport.dll");

				if (!File.Exists(paratextSupportDLL))
				{
					paratextSupportDLL = Common.PathCombine(Path.GetDirectoryName(Common.AssemblyPath), "ParatextSupport.dll");
				}
                // load the ParatextSupport DLL dynamically
				Assembly asm = Assembly.LoadFrom(paratextSupportDLL);

                // Convert the .sty stylesheet to css
                // new ScrStylesheet(styFile)
                Type tStyToCSS = asm.GetType("SIL.PublishingSolution.StyToCSS");
                Object oScrStylesheet = null;
                if (tStyToCSS != null)
                {
                    // new styToCss
                    oScrStylesheet = Activator.CreateInstance(tStyToCSS);
                    // styToCss.StyFullPath = styFile
                    PropertyInfo piStyFullPath = tStyToCSS.GetProperty("StyFullPath", BindingFlags.Public | BindingFlags.Instance);
                    piStyFullPath.SetValue(oScrStylesheet, styFile, null);
                    Object[] args = new object[1];
                    args[0] = Common.PathCombine(projInfo.ProjectPath, projInfo.ProjectName + ".css");
	                tStyToCSS.InvokeMember("ConvertStyToCSS", BindingFlags.Default | BindingFlags.InvokeMethod, null, oScrStylesheet, args);
                }
                projInfo.DefaultCssFileWithPath = Common.PathCombine(projInfo.ProjectPath, projInfo.ProjectName + ".css");

                // collect the files and convert them to XmlDocuments
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
                var docs = new List<XmlDocument>();
                foreach (var file in files)
                {
                    if (File.Exists(Common.PathCombine(projInfo.ProjectPath, file)))
                    {
                        var xmlDoc = new XmlDocument();
                        xmlDoc.XmlResolver = FileStreamXmlResolver.GetNullResolver();
                        xmlDoc.Load(Common.PathCombine(projInfo.ProjectPath, file));
                        docs.Add(xmlDoc);
                    }
                }

                // now convert them to xhtml
                Type tPPL = asm.GetType("SIL.PublishingSolution.ParatextPathwayLink");
                if (tPPL != null)
                {
                    Object[] args = new object[5];
                    args[0] = projInfo.ProjectName;
                    args[1] = projInfo.ProjectName;
                    args[2] = "zxx";
                    args[3] = "zxx";
                    args[4] = "PathwayB";
                    Object oPPL = Activator.CreateInstance(tPPL, args);
                    Object[] argsCombine = new object[2];
                    argsCombine[0] = docs;
					argsCombine[1] = Param.PrintVia;
                    XmlDocument scrBooksDoc = (XmlDocument)tPPL.InvokeMember
                        ("CombineUsxDocs", BindingFlags.Default | BindingFlags.InvokeMethod, null, oPPL, argsCombine);
                    if (string.IsNullOrEmpty(scrBooksDoc.InnerText))
                    {
                        throw new Exception("No content found to convert.");
                    }
                    Object[] argsConvert = new object[2];
                    argsConvert[0] = scrBooksDoc.InnerXml;
                    argsConvert[1] = Common.PathCombine(projInfo.ProjectPath, projInfo.ProjectName.Replace(" ","_") + ".xhtml");
	                tPPL.InvokeMember("ConvertUsxToPathwayXhtmlFile", BindingFlags.Default | BindingFlags.InvokeMethod, null, oPPL, argsConvert);
                }
                projInfo.DefaultXhtmlFileWithPath = Common.PathCombine(projInfo.ProjectPath, projInfo.ProjectName.Replace(" ", "_") + ".xhtml");

            }
            catch (FileNotFoundException)
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
    }
}
