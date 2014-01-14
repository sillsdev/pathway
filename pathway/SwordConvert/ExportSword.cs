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
            string osisFilePath = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath);
            string tempSwordCreatorPath = SwordCreatorTempDirectory(swordFullPath);
            string swordTempFolder = tempSwordCreatorPath;
            osisFilePath = Common.PathCombine(osisFilePath, "OSIS");
            CreateDirectoryForSwordOutput(osisFilePath);
            UsxToOSIS usxToOsis = new UsxToOSIS();
            string[] usxFilesList = Directory.GetFiles(usxFilePath, "*.usx");
            
            foreach (var usxfile in usxFilesList)
            {
                string osisFileName = Path.GetFileNameWithoutExtension(usxfile) + ".xml";
                osisFileName = Common.PathCombine(osisFilePath, osisFileName);
                usxToOsis.ConvertUsxToOSIS(usxfile, osisFileName);
            }
            osisFilePath = Path.GetDirectoryName(osisFilePath);
            tempSwordCreatorPath = Common.PathCombine(tempSwordCreatorPath, "OSIS");
            osisFilePath = Common.PathCombine(osisFilePath, "OSIS");
            CopySwordCreatorFolderToTemp(osisFilePath, tempSwordCreatorPath);
          
            string swordOutputLocation = SwordOutputLocation(Path.GetDirectoryName(tempSwordCreatorPath));
            string[] osisFilesList = Directory.GetFiles(tempSwordCreatorPath, "*.xml");
            SwordOutputBuildProcess(swordTempFolder, swordOutputLocation, osisFilesList, usxFilePath);

            CopySwordCreatorFolderToTemp(swordTempFolder, projInfo.ProjectPath);

            return success;
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
            CopySwordCreatorFolderToTemp(swordFullPath, folder);
            return folder;
        }

        public void CopySwordCreatorFolderToTemp(string sourceFolder, string destFolder)
        {
            if (Directory.Exists(destFolder))
            {
                Common.DeleteDirectory(destFolder);
            }
            Directory.CreateDirectory(destFolder);
            string[] files = Directory.GetFiles(sourceFolder);
            try
            {
                foreach (string file in files)
                {
                    string name = Path.GetFileName(file);
                    string dest = Common.PathCombine(destFolder, name);
                    
                        File.Copy(file, dest);
                    
                }

                string[] folders = Directory.GetDirectories(sourceFolder);
                foreach (string folder in folders)
                {
                    string name = Path.GetFileName(folder);
                    string dest = Common.PathCombine(destFolder, name);
                    CopySwordCreatorFolderToTemp(folder, dest);
                }
            }
            catch
            {

            }
        }

        private static string SwordOutputLocation(string swordOutputLocation)
        {
            swordOutputLocation = Common.PathCombine(swordOutputLocation, "modules");
            swordOutputLocation = Common.PathCombine(swordOutputLocation, "texts");
            swordOutputLocation = Common.PathCombine(swordOutputLocation, "ztext");
            swordOutputLocation = Common.PathCombine(swordOutputLocation, "kjv");
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
