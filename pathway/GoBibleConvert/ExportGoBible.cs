// --------------------------------------------------------------------------------------
// <copyright file="ExportGoBible.cs" from='2012' to='2012' company='SIL International'>
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
// Go Bible export Version 2.4.3
// See: btai/Documentation/GoBible Documentation.docx
// See: http://www.crosswire.org/gobible/
// See: http://www.jolon.org
// See: http://en.wikipedia.org/wiki/Go_Bible
// See: http://gbcpreprocessor.codeplex.com
// See: http://code.google.com/p/gobible/wiki/GoBibleRoadmap
// Emulators (for testing): http://jolon.org/vanillaforum/comments.php?DiscussionID=57
// </remarks>
// --------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using SIL.Tool;
using System.Threading;


namespace SIL.PublishingSolution
{
    public class ExportGoBible : IExportProcess
    {
        protected string processFolder;
        protected string restructuredFullName;
        protected string collectionFullName;
        protected string collectionName;
        protected static ProgressBar _pb;
        private string _iconFile;
        private const string RedirectOutputFileName = "Convert.log";
        List<string> DuplicateBooks;

        public string ExportType
        {
            get
            {
                return "Go Bible";
            }
        }

        public bool Handle(string inputDataType)
        {
            return inputDataType.ToLower() == "scripture";
        }

        /// <summary>
        /// Entry point for GoBible export
        /// </summary>
        /// <param name="exportType">scripture / dictionary</param>
        /// <param name="publicationInformation">structure with other necessary information about project.</param>
        /// <returns></returns>
        public bool Launch(string exportType, PublicationInformation publicationInformation)
        {
            return Export(publicationInformation);
        }

        /// <summary>
        /// Entry point for GoBible converter
        /// </summary>
        /// <param name="projInfo">values passed including xhtml and css names</param>
        /// <returns>true if succeeds</returns>
        public bool Export(PublicationInformation projInfo)
        {
            bool success;
            var inProcess = new InProcess(0, 6);
            try
            {
                var curdir = Environment.CurrentDirectory;
                var myCursor = Cursor.Current;
                Cursor.Current = Cursors.WaitCursor;
                
                inProcess.Show();
                inProcess.PerformStep();
                string exportGoBibleInputPath = string.Empty;
                exportGoBibleInputPath = Path.GetDirectoryName(projInfo.DefaultCssFileWithPath);
                processFolder = exportGoBibleInputPath;
                CreateCollectionsTextFile(exportGoBibleInputPath);
                var iconFullName = Common.FromRegistry(Common.PathCombine("GoBible/GoBibleCore", "Icon.png"));
                var iconDirectory = Path.GetDirectoryName(iconFullName);
                _iconFile = Path.GetFileName(iconFullName);
                const bool overwrite = true;
                if (iconDirectory != exportGoBibleInputPath)
                    File.Copy(iconFullName, Path.Combine(exportGoBibleInputPath, _iconFile), overwrite);
                
                Param.LoadSettings();
                Param.SetValue(Param.InputType, "Scripture");
                Param.LoadSettings();
                string layout = Param.GetItem("//settings/property[@name='LayoutSelected']/@value").Value;
                Dictionary<string, string> mobilefeature = Param.GetItemsAsDictionary("//stylePick/styles/mobile/style[@name='" + layout + "']/styleProperty");
                string languageSelection = string.Empty;
                inProcess.PerformStep();
                if (mobilefeature.ContainsKey("Language") && mobilefeature["Language"] != null)
                {
                    languageSelection = mobilefeature["Language"].ToString();
                }

                string goBibleFullPath = Common.FromRegistry("GoBible");

                string tempGoBibleCreatorPath = GoBibleCreatorTempDirectory(goBibleFullPath);

                string goBibleCreatorPath = Path.Combine(tempGoBibleCreatorPath, "GoBibleCore");
                inProcess.PerformStep();
                string languageLocationPath = Path.Combine(goBibleFullPath, "User Interface");
                languageLocationPath = Path.Combine(languageLocationPath, languageSelection);
                string[] filePaths = Directory.GetFiles(languageLocationPath, "*.properties");
                goBibleCreatorPath = Path.Combine(goBibleCreatorPath, "ui.properties");

                UIPropertiesCopyToTempFolder(goBibleCreatorPath, filePaths);

                BuildApplication(tempGoBibleCreatorPath);
                success = true;
                inProcess.PerformStep();
                inProcess.Close();
                Cursor.Current = myCursor;
                inProcess.PerformStep();
                string jarFile = Path.Combine(processFolder, NoSp(GetInfo(Param.Title)) + ".jar");

                if (File.Exists(jarFile))
                {
                    // Failed to send the .jar to a bluetooth device. Tell the user to do it manually.
                    string msg = string.Format("Please copy the file {0} to your phone", jarFile);
                    MessageBox.Show(msg, "Go Bible Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed Exporting GoBible Process.", "Go Bible Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                DeleteTempFiles(exportGoBibleInputPath);
                Common.DeleteDirectory(tempGoBibleCreatorPath);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                success = false;
                inProcess.PerformStep();
                inProcess.Close();
            }
            return success;
        }

        private static string NoSp(string p)
        {
            return string.Concat(p.Split());
        }

        private static void UIPropertiesCopyToTempFolder(string goBibleCreatorPath, string[] filePaths)
        {
            try
            {
                if (File.Exists(filePaths[0]))
                    File.Copy(filePaths[0], goBibleCreatorPath, true);
            }
            catch
            {
            }
        }

        private void DeleteTempFiles(string exportGoBibleInputPath)
        {
            if (File.Exists(Path.Combine(exportGoBibleInputPath, _iconFile)))
            {
                File.Delete(Path.Combine(exportGoBibleInputPath, _iconFile));
            }

            var outputFiles = Directory.GetFiles(processFolder);
            foreach (var outputFile in outputFiles)
            {
                try
                {
                    // Did we modify this file during our export? If so, delete it
                    if (outputFile.EndsWith(".xhtml"))
                    {
                        File.Delete(outputFile);
                    }
                    if (outputFile.EndsWith(".css"))
                    {
                        File.Delete(outputFile);
                    }
                    if (outputFile.EndsWith(".tmp"))
                    {
                        File.Delete(outputFile);
                    }
                    // delete the Scripture.de / Dictionary.de file as well
                    if (outputFile.EndsWith(".de"))
                    {
                        File.Delete(outputFile);
                    }
                }
                catch (Exception)
                {
                    // problem with this file - just continue with the next one
                    continue;
                }
            }
        }

        private string GoBibleCreatorTempDirectory(string goBibleFullPath)
        {
            var goBibleDirectoryName = Path.GetFileNameWithoutExtension(goBibleFullPath);
            var tempFolder = Path.GetTempPath();
            var folder = Path.Combine(tempFolder, goBibleDirectoryName);
            if (Directory.Exists(folder))
                Directory.Delete(folder, true);

            CopyGoBibleCreatorFolderToTemp(goBibleFullPath, folder);

            return folder;
        }

        private void CopyGoBibleCreatorFolderToTemp(string sourceFolder, string destFolder)
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
                    if (name != "User Interface")
                    {
                        CopyGoBibleCreatorFolderToTemp(folder, dest);
                    }
                }
            }
            catch
            {
                
            }
        }

        private void CreateCollectionsTextFile(string exportGoBiblePath)
        {
            string fileLoc = Path.Combine(exportGoBiblePath, "Collections.txt");

            if (File.Exists(fileLoc))
            {
                File.Delete(fileLoc);
            }
            using (StreamWriter sw = new StreamWriter(fileLoc))
            {
                var info = "Bible text exported from Paratext.";
                sw.WriteLine("Info: " + info);
                sw.WriteLine(@"Source-Text: \SFM");
                sw.WriteLine("Source-Format: usfm");
                sw.WriteLine("Source-FileExtension: sfm");
                sw.WriteLine("Phone-Icon-Filepath: Icon.png");
                sw.WriteLine("Application-Name: " + GetInfo(Param.Title));
                sw.WriteLine("MIDlet-Vendor: " + GetInfo(Param.Title) + " Vendor");
                sw.WriteLine("MIDlet-Info-URL: http://wap.mygbdomain.org");
                sw.WriteLine("Codepage: UTF-8");
                sw.WriteLine("RedLettering: false");
                sw.WriteLine(@"USFM-TitleTag: \" + Common.BookNameTag);
                sw.WriteLine("Collection: " + GetInfo(Param.Title));

                string sfmFiles = Path.Combine(exportGoBiblePath, "SFM");

                if (Directory.Exists(sfmFiles))
                {
                    foreach (string name in Common.BookNameCollection)
                    {
                        sw.WriteLine("Book: " + name);
                    }
                    Common.BookNameCollection.Clear();
                }
                sw.Flush();
                sw.Close();
            }

            collectionFullName = fileLoc;
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

        protected bool IsDuplicateBooks(XmlNodeList books)
        {
            DuplicateBooks = new List<string>();
            List<string> allBooks = new List<string>();
            foreach (XmlNode bookNode in books)
            {
                string bookName = bookNode.Value;
                if (allBooks.Contains(bookName))
                {
                    if (!DuplicateBooks.Contains(bookName))
                        DuplicateBooks.Add(bookName);
                }
                else
                {
                    allBooks.Add(bookName);
                }
            }
            return DuplicateBooks.Count > 0;
        }

        /// <summary>
        /// Uses Java to create GoBible application
        /// </summary>
        /// <param name="goBibleCreatorPath"></param>
        protected void BuildApplication(string goBibleCreatorPath)
        {
            const string Creator = "GoBibleCreator.jar";
            const string prog = "java";
            var creatorFullPath = Path.Combine(goBibleCreatorPath, Creator);
            var progFolder = SubProcess.GetLocation(prog);
            progFolder = JavaProgFolder(progFolder);
            var progFullName = Common.PathCombine(progFolder, prog);
            if (progFullName.EndsWith(".exe"))
            {
                progFullName = progFullName.Substring(0, progFullName.Length - 4);
            }
            var args = string.Format(@"-Xmx128m -jar ""{0}"" ""{1}""", creatorFullPath, collectionFullName);
            SubProcess.RedirectOutput = RedirectOutputFileName;
            SubProcess.Run(processFolder, progFullName, args, true);
        }

        /// <summary>
        /// Check for the Java program in it's normal location on Windows
        /// </summary>
        /// <param name="progFolder">path to Java program</param>
        /// <returns>path to Java program</returns>
        protected static string JavaProgFolder(string progFolder)
        {
            if (string.IsNullOrEmpty(progFolder))
            {
                var info = new DirectoryInfo("C:\\Program Files\\Java");
                foreach (DirectoryInfo directoryInfo in info.GetDirectories("jdk*"))
                {
                    progFolder = Path.Combine(directoryInfo.FullName, "bin");
                    if (File.Exists(Path.Combine(progFolder, "java.exe")))
                        break;
                }
                if (string.IsNullOrEmpty(progFolder))
                {
                    foreach (DirectoryInfo directoryInfo in info.GetDirectories("jre*"))
                    {
                        progFolder = Path.Combine(directoryInfo.FullName, "bin");
                        if (File.Exists(Path.Combine(progFolder, "java.exe")))
                            break;
                    }
                }
            }
            return progFolder;
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
