using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using SIL.Tool;
using System.Threading;


namespace SIL.PublishingSolution
{
    public class ExportSword : IExportProcess
    {
        protected string _processFolder;
        private const string RedirectOutputFileName = "Convert.log";
        private bool _isLinux;

        public string ExportType
        {
            get
            {
                return "Sword";
            }
        }

        public bool Handle(string inputDataType)
        {
            return inputDataType.ToLower() == "scripture";
        }

        /// <summary>
        /// Entry point for Sword export
        /// </summary>
        /// <param name="exportType">scripture / dictionary</param>
        /// <param name="publicationInformation">structure with other necessary information about project.</param>
        /// <returns></returns>
        public bool Launch(string exportType, PublicationInformation publicationInformation)
        {
            return Export(publicationInformation);
        }

        /// <summary>
        /// Entry point for Sword converter
        /// </summary>
        /// <param name="projInfo">values passed including xhtml and css names</param>
        /// <returns>true if succeeds</returns>
        public bool Export(PublicationInformation projInfo)
        {
            bool success = false;
            string swordFullPath = Common.FromRegistry("Sword");
            string usxFilePath = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath);
            projInfo.ProjectPath = usxFilePath;
            usxFilePath = Common.PathCombine(usxFilePath, "usx");
            string osisFilePath = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath);
            string tempSwordCreatorPath = SwordCreatorTempDirectory(swordFullPath);
            string swordTempFolder = tempSwordCreatorPath;
            osisFilePath = Common.PathCombine(osisFilePath, "OSIS");
            CreateDirectoryForSwordOutput(osisFilePath);
            UsxToOSIS usxToOsis = new UsxToOSIS();
            string[] usxFilesList = Directory.GetFiles(usxFilePath, "*.usx");

            //Usx to Osis Conversion Process Step
            UsxtoOsisConvertProcess(usxFilesList, osisFilePath, usxToOsis);


            osisFilePath = Path.GetDirectoryName(osisFilePath);
            tempSwordCreatorPath = Common.PathCombine(tempSwordCreatorPath, "OSIS");
            osisFilePath = Common.PathCombine(osisFilePath, "OSIS");
            CopySwordCreatorFolderToTemp(osisFilePath, tempSwordCreatorPath, null);

            string swordOutputLocation = SwordOutputLocation(Path.GetDirectoryName(tempSwordCreatorPath));
            string[] osisFilesList = Directory.GetFiles(tempSwordCreatorPath, "*.xml");

            //Start Process to Osis To Osis2Mod Conversion
            SwordOutputBuildProcess(swordTempFolder, swordOutputLocation, osisFilesList, Path.GetDirectoryName(usxFilePath));

            string swordOutputExportLocation = Common.PathCombine(projInfo.ProjectPath, "SwordOutput");
            List<string> restricttoCopyExtensions = new List<string>();
            restricttoCopyExtensions.Add(".exe");
            restricttoCopyExtensions.Add(".dll");

            CopySwordCreatorFolderToTemp(swordTempFolder, swordOutputExportLocation, restricttoCopyExtensions);

            string sLanguageCode = JustLanguageCode(projInfo.DefaultXhtmlFileWithPath, projInfo.ProjectInputType);

            WriteModConfigFile(swordOutputExportLocation, sLanguageCode);

            return success;
        }

        

        private string JustLanguageCode(string xhtmlFileNameWithPath, string projectInputType)
        {
            string languageCode = Common.GetLanguageCode(xhtmlFileNameWithPath, projectInputType);
            if (languageCode.Contains(":"))
            {
                languageCode = languageCode.Substring(0, languageCode.IndexOf(':'));
            }
            return languageCode;
        }

        private void WriteModConfigFile(string swordOutputLocation, string languageCode)
        {
            swordOutputLocation = Common.PathCombine(swordOutputLocation, "mods.d");

            if (Directory.Exists(swordOutputLocation))
            {
                Common.DeleteDirectory(swordOutputLocation);
            }
            Directory.CreateDirectory(swordOutputLocation);

            string Newfile = Common.PathCombine(swordOutputLocation, languageCode + ".conf");
            var fs2 = new FileStream(Newfile, FileMode.Create, FileAccess.Write);
            var sw2 = new StreamWriter(fs2);
            

            WriteConfig(sw2, "[KJV]");
            WriteConfig(sw2, "DataPath=./modules/texts/ztext/kjv/");
            WriteConfig(sw2, "ModDrv=zText");
            WriteConfig(sw2, "Encoding=UTF-8");
            WriteConfig(sw2, "BlockType=BOOK");
            WriteConfig(sw2, "CompressType=ZIP");
            WriteConfig(sw2, "SourceType=OSIS");
            WriteConfig(sw2, "Lang=en");
            WriteConfig(sw2, "GlobalOptionFilter=OSISStrongs");
            WriteConfig(sw2, "GlobalOptionFilter=OSISMorph");
            WriteConfig(sw2, "GlobalOptionFilter=OSISFootnotes");
            WriteConfig(sw2, "GlobalOptionFilter=OSISHeadings");
            WriteConfig(sw2, "GlobalOptionFilter=OSISRedLetterWords");
            WriteConfig(sw2, "OSISqToTick=false");
            WriteConfig(sw2, "Feature=StrongsNumbers");
            WriteConfig(sw2, "MinimumVersion=1.5.9");
            WriteConfig(sw2, "SwordVersionDate=2006-10-09");
            WriteConfig(sw2, "Version=2.3");
            WriteConfig(sw2, "Description=King James Version (1769) with Strongs Numbers and Morphology");
            WriteConfig(sw2, "About=");
            WriteConfig(sw2, "TextSource=bf.org, eBible.org, crosswire.org");
            WriteConfig(sw2, "LCSH=Bible. English.");
            WriteConfig(sw2, "DistributionLicense=General public license for distribution for any purpose");

            sw2.Close();
            fs2.Close();
        }

        private static void WriteConfig(StreamWriter sw2, string content)
        {
            sw2.WriteLine(content);
        }

        private static void UsxtoOsisConvertProcess(string[] usxFilesList, string osisFilePath, UsxToOSIS usxToOsis)
        {
            foreach (var usxfile in usxFilesList)
            {
                string osisFileName = Path.GetFileNameWithoutExtension(usxfile) + ".xml";
                osisFileName = Common.PathCombine(osisFilePath, osisFileName);
                usxToOsis.ConvertUsxToOSIS(usxfile, osisFileName);
            }
        }

        private string SwordCreatorTempDirectory(string swordFullPath)
        {
            var swordDirectoryName = Path.GetFileNameWithoutExtension(swordFullPath);
            var tempFolder = Path.GetTempPath();
            var folder = Common.PathCombine(tempFolder, swordDirectoryName);
            if (Directory.Exists(folder))
            {
                DirectoryInfo di = new DirectoryInfo(folder);
                Common.CleanDirectory(di);
            }
            CopySwordCreatorFolderToTemp(swordFullPath, folder, null);
            return folder;
        }

        public void CopySwordCreatorFolderToTemp(string sourceFolder, string destFolder, List<string> restricttoCopyExtensions)
        {
            if (Directory.Exists(destFolder))
            {
                Common.DeleteDirectory(destFolder);
            }
            Directory.CreateDirectory(destFolder);

            if (Directory.Exists(sourceFolder))
            {
                string[] files = Directory.GetFiles(sourceFolder);
                try
                {
                    CopyFilesSourceToDestLocation(destFolder, restricttoCopyExtensions, files);

                    string[] folders = Directory.GetDirectories(sourceFolder);
                    foreach (string folder in folders)
                    {
                        string name = Path.GetFileName(folder);
                        string dest = Common.PathCombine(destFolder, name);
                        CopySwordCreatorFolderToTemp(folder, dest, restricttoCopyExtensions);
                    }
                }
                catch { }
            }
        }

        private static void CopyFilesSourceToDestLocation(string destFolder, List<string> restricttoCopyExtensions, string[] files)
        {
            foreach (string file in files)
            {
                if (restricttoCopyExtensions != null && restricttoCopyExtensions.Count > 1)
                {
                    bool isExtensionAvailable = false;
                    foreach (var extensions in restricttoCopyExtensions)
                    {
                        if (file.Contains(extensions))
                        {
                            isExtensionAvailable = true;
                            break;
                        }
                    }

                    if (!isExtensionAvailable)
                    {
                        string name = Path.GetFileName(file);
                        string dest = Common.PathCombine(destFolder, name);
                        File.Copy(file, dest, true);
                    }
                }
                else
                {
                    string name = Path.GetFileName(file);
                    string dest = Common.PathCombine(destFolder, name);
                    File.Copy(file, dest, true);
                }
            }
        }

        private static string SwordOutputLocation(string swordOutputLocation)
        {
            swordOutputLocation = Common.PathCombine(swordOutputLocation, "modules");
            swordOutputLocation = Common.PathCombine(swordOutputLocation, "texts");
            swordOutputLocation = Common.PathCombine(swordOutputLocation, "ztext");
            swordOutputLocation = Common.PathCombine(swordOutputLocation, "sil");
            return swordOutputLocation;
        }

        private static void CreateDirectoryForSwordOutput(string swordOutputLocation)
        {
            if (!Directory.Exists(swordOutputLocation))
            {
                Directory.CreateDirectory(swordOutputLocation);
            }
        }

        /// <summary>
        /// Uses Sword Output Execute Process
        /// </summary>
        /// <param name="Sword Output"></param>
        protected void SwordOutputBuildProcess(string processFolder, string swordOutputPath, string[] osisFilesList, string projectPath)
        {
            string Creator = "osis2mod";
            //Creator = Common.PathCombine(processFolder, Creator);
            string moreArguments = "-z -v KJV"; //"-z -v KJV -d 0";
            foreach (var osisFile in osisFilesList)
            {
                var args = string.Format(@"""{0}"" ""{1}"" {2}", swordOutputPath, osisFile, moreArguments);
                //SubProcess.RunCommandWithErrorLog(processFolder, Creator, args, true , projectPath);

                const bool noWait = false;
                string stdOutput = string.Empty;
                string stdOutErr = string.Empty;
                SubProcess.Run(processFolder, Creator, args, true);
                //SubProcess.RunCommandWithErrorLog(processFolder, Creator, args, true, projectPath);
                moreArguments = "-a -z -v KJV";

            }
        }

        private string GetInfo(string metadataValue)
        {
            string organization;
            try
            {
                // get the organization
                organization = Param.Value["Organization"];
            }
            catch (Exception)
            {
                // shouldn't happen (ExportThroughPathway dialog forces the user to select an organization), 
                // but just in case, specify a default org.
                organization = "SIL International";
            }
            var sb = new StringBuilder();
            var value = Param.GetMetadataValue(metadataValue, organization);
            // check for null / empty values
            if (value == null) return "";
            if (value.Trim().Length < 1) return "";
            // if we got here, there's a metadata value that can be pulled out and formatted
            sb.Append(value);

            return sb.ToString();
        }


        /// <summary>
        /// returns the project name from the path
        /// </summary>
        /// <param name="projInfo">data on project</param>
        /// <returns>Project Name</returns>
        protected string GetProjectName(IPublicationInformation projInfo)
        {
            var scrDir = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath);
            var projDir = Path.GetDirectoryName(scrDir);
            return Path.GetFileName(projDir);
        }
    }
}
