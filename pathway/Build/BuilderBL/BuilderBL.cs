// --------------------------------------------------------------------------------------------
// <copyright file="BuilderBL.cs" from='2009' to='2009' company='SIL International'>
//      Copyright © 2009, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Builder for the Project
// </remarks>
// --------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SIL.Tool;

namespace Builder
{
    public class BuilderBL
    {
        /// <summary>
        /// Contains version of PublishingSolution Component on entry
        /// </summary>
        public static string CurrentVersion { set; get; }

        #region GetInstPath
        /// <summary>
        /// Return path to installer
        /// </summary>
        public static string GetInstPath()
        {
            return Environment.CurrentDirectory + @"/../../../Installer/";
        }
        #endregion GetInstPath

        #region VersionValidate
        /// <summary>
        /// True if a valid version # is entered
        /// </summary>
        public static bool VersionValidate(string version)
        {
            Match m = Regex.Match(version, @"[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+");
            if (!m.Success)
            {
                MessageBox.Show("Please Enter 4 numbers separated by periods for the version number.");
                return false;
            }
            return true;
        }
        #endregion VersionValidate

        #region UpdateVersion
        /// <summary>
        /// Updates version # in all files Named AssemblyInfo in the tree with root curPath
        /// </summary>
        public static void UpdateVersion(string curPath, string version)
        {
            var aiName = "AssemblyInfo.cs";
            var di = new DirectoryInfo(curPath);
            if (di.GetFiles(aiName).Length > 0)
            {
                var sub = new Substitution { InputFile = aiName, OutputFile = aiName, TargetPath = curPath };
                //MessageBox.Show(curPath);
                try
                {
                    sub.UpdateGroup1(@".assembly. AssemblyFileVersion..([0-9.]+)...", version);
                    sub.UpdateGroup1(@".assembly. AssemblyVersion..([0-9.]+)...", version);
                }
                catch (Exception)
                {
                }
            }
            var fiList = di.GetDirectories();
            foreach (var fi in fiList)
            {
                var startsWith = fi.Name.Substring(0, 1);
                if (startsWith == "." || startsWith == "_")
                    continue;
                UpdateVersion(Common.PathCombine(curPath, fi.Name), version);
            }
        }
        #endregion UpdateVersion

        #region UpdateInstallerDescription
        /// <summary>
        /// Inserts the latest version number into the installer description
        /// </summary>
        public static void UpdateInstallerDescription(string instPath, string instName, string version, string fwVer, string prodName)
        {
            var sub = new Substitution { TargetPath = instPath };
            var map = new Dictionary<string, string>();
            map["PwVer"] = PublishedVersion(version);
            if (!string.IsNullOrEmpty(fwVer))
                map["FwVer"] = PublishedVersion(fwVer);
            map["Product"] = prodName;
            sub.FileSubstitute(instName, map);
        }
        #endregion UpdateInstallerDescription

        #region PublishedVersion
        /// <summary>
        /// Return all but last section of version
        /// </summary>
        public static string PublishedVersion(string version)
        {
            return version.Substring(0, version.LastIndexOf('.'));
        }
        #endregion PublishedVersion

        #region PublicFieldWorksVersion
        /// <summary>
        /// Return all but last section of version
        /// </summary>
        public static string PublicFieldWorksVersion()
        {
            string[] fwVer = new string[2];
            foreach (string [] element in Common.VersionElements())
            {
                fwVer = element;
                break;
            }
            return PublishedVersion(fwVer[1]);
        }
        #endregion PublicFieldWorksVersion

        #region UpdateReadme
        /// <summary>
        /// Inserts version number and date into readme, allows user to edit in comments about changes
        /// </summary>
        public static void UpdateReadme(string instPath, string myNotes, string prodName, string version)
        {
            var lastDot = CurrentVersion.LastIndexOf(".");
            var pattern = prodName + "(" + CurrentVersion.Substring(0, lastDot) + ")";
            var sub = new Substitution { InputFile = myNotes, OutputFile = myNotes, TargetPath = instPath };
            sub.UpdateGroup1(pattern, version.Substring(0, lastDot));
            //Match {\field{\*\fldinst {\rtlch\fcs1 \af0 \ltrch\fcs0 \insrsid4093144  SAVEDATE  \\@ "MMMM d, yyyy"  \\* MERGEFORMAT }}{\fldrslt {\rtlch\fcs1 \af0 \ltrch\fcs0 \lang1024\langfe1024\noproof\insrsid4093144 August 28, 2009}}}
            var datePattern = @"{\\field{[^{]*{[^{]* SAVEDATE[^{]*{[^{]*{[^}]* ([a-zA-Z]* [0-9]*, [0-9]*)}}}";
            sub.UpdateGroup1(datePattern, DateTime.Now.ToString("MMMM d, yyyy"));
            SubProcess.Run(instPath, myNotes);
        }
        #endregion UpdateReadme

        #region SelectDlls
        /// <summary>
        /// Copy Dlls from folder selected by user to folder used for build
        /// </summary>
        public static string SelectDlls(string basePath, string dlls)
        {
            var values = dlls.Split(new [] {" - "}, StringSplitOptions.None);
            if (values[0] == "Dlls601" || values[1].Substring(0,3) != "6.0")
                return "";
            FolderTree.Copy(Common.PathCombine(basePath, values[0]), Common.PathCombine(basePath, "Dlls601"));
            return values[1];
        }
        #endregion SelectDlls

        #region RemoveSubFolders
        /// <summary>
        /// Removes all the subfolders of the given path
        /// </summary>
        /// <param name="path">target path</param>
        public static void RemoveSubFolders(string path)
        {
            var folders = new DirectoryInfo(Common.DirectoryPathReplace(path));
            foreach (DirectoryInfo directoryInfo in folders.GetDirectories())
            {
                var startsWith = directoryInfo.Name.Substring(0, 1);
                if (startsWith == "." || startsWith == "_")
                    continue;
                directoryInfo.Delete(true);
            }
        }

        #endregion RemoveSubFolders

        #region CopyReleaseFiles
        /// <summary>
        /// Copys release files from source to destination using the instPath as a base.
        /// </summary>
        public static void CopyRelaseFiles(string instPath, string source, string dest, string release)
        {
            var filesInfo = new DirectoryInfo(instPath + string.Format(@"../Files/{0}", dest));
            if (!filesInfo.Exists)
                filesInfo.Create();
            string sourceFullName = string.Format("{0}../../{1}/Bin/{2}", instPath, source, release);
            FolderTree.Copy(sourceFullName, filesInfo.FullName);
            //var srcInfo = new DirectoryInfo(instPath + @"..\" + source + @"/Bin/Release");
            //foreach (var fileInfo in srcInfo.GetFiles("*.*", SearchOption.TopDirectoryOnly))
            //    File.Copy(fileInfo.FullName, Common.PathCombine(filesInfo.FullName, fileInfo.Name));
        }
        #endregion CopyReleaseFiles

        #region CopyTree
        /// <summary>
        /// Copys a folder tree from source to destination using the instPath as a base.
        /// </summary>
        public static void CopyTree(string instPath, string source, string dest)
        {
            var filesInfo = new DirectoryInfo(instPath + string.Format(@"../Files/{0}", dest));
            if (!filesInfo.Exists)
                filesInfo.Create();
            FolderTree.Copy(instPath + source, filesInfo.FullName);
        }
        #endregion CopyTree

        #region CopyFile
        /// <summary>
        /// Copys a folder tree from source to destination using the instPath as a base.
        /// </summary>
        public static void CopyFile(string instPath, string name, string dest)
        {
            if (!Directory.Exists(instPath + dest))
                Directory.CreateDirectory(instPath + dest);
            File.Copy(Common.PathCombine(instPath, name), Common.PathCombine(instPath + dest, name), true);
        }
        #endregion CopyTree

        #region RemoveFiles
        /// <summary>
        /// Remove files from source that are installed by another installer using the instPath as a base.
        /// </summary>
        public static void RemoveFiles(string instPath, string source, string dest)
        {
            var filesInfo = new DirectoryInfo(instPath + string.Format(source));
            foreach (var oneFile in filesInfo.GetFiles())
            {
                var target = instPath + @"../Files/" + dest + @"/" + oneFile.Name;
                if (File.Exists(target))
                    File.Delete(target);
            }
        }
        #endregion CopyTree

        #region SetFilesNFeatures
        /// <summary>
        /// Loads the list of Files and Features for the item and sets strings for Files and Features in the map.
        /// </summary>
        public static void SetFilesNFeatures(string item, string instPath, Substitution sub, IDictionary<string, string> map)
        {
            //const string partialPattern = "<!-- Files: -->\n(.*)\n\\<!-- Features: --\\>\n(.*)";
            string data = FileData.Get(string.Format("{0}Partial {1}.wxs", instPath, item));
            var lines = data.Split('\n');
            var featureStart = 0;
            for (; featureStart < lines.Length; featureStart++)
            {
                if (lines[featureStart].TrimEnd('\r') == "<!-- Features: -->")
                    break;
            }
            if (featureStart == lines.Length)
                throw new IOException();
            map[string.Format("{0}Files", item)] = Join(lines, 2, featureStart - 3) + "\n";
            map[string.Format("{0}Features", item)] = Join(lines, featureStart + 1, lines.Length - 1);
        }
        #endregion SetFilesNFeatures

        #region Join
        /// <summary>
        /// returns a string by joining lines from start index to end index.
        /// </summary>
        public static string Join(string[] lines, int start, int end)
        {
            var result = "";
            var delim = "";
            for (var i = start; i <= end; i++)
            {
                result = result + delim + lines[i];
                delim = "\n";
            }
            return result;
        }
        #endregion Join

        #region GetCurrentVersion
        /// <summary>
        /// Returns the current version (being created) of PublishingSolution
        /// </summary>
        public static string GetCurrentVersion(string app)
        {
            string appInfoRelativePath = string.Format("/../../../../{0}/Properties/AssemblyInfo.cs", app);
            var myVersionName = Environment.CurrentDirectory + appInfoRelativePath;
            var data = FileData.Get(myVersionName);
            var m = Regex.Match(data, @".assembly. AssemblyFileVersion..([0-9.]+)...");
            var currentVersion = m.Groups[1].Value;
            var lastDot = currentVersion.LastIndexOf(".");
            return currentVersion.Substring(0, lastDot);
        }
        #endregion GetCurrentVersion

        public static void DoBatch(string instPath, string project, string process, string config)
        {
            string folder = Common.PathCombine(instPath, "../../" + project + "/bin/" + config);
            //MessageBox.Show(folder);
            SubProcess.Run(folder, Common.PathCombine(folder, "../../" + process), config, true);
        }
    }
}
