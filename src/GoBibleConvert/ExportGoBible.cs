// --------------------------------------------------------------------------------------
// <copyright file="ExportGoBible.cs" from='2012' to='2014' company='SIL International'>
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
using L10NSharp;
using SilTools;


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
        List<string> _duplicateBooks;
        private bool _isLinux;

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
#if (TIME_IT)
                DateTime dt1 = DateTime.Now;    // time this thing
#endif
            var inProcess = new InProcess(0, 7);
            try
            {
                var myCursor = Cursor.Current;
                Cursor.Current = Cursors.WaitCursor;
                inProcess.Text = "GoBible Export";
                inProcess.Show();
                inProcess.PerformStep();
                inProcess.ShowStatus = true;
                inProcess.SetStatus("Processing GoBible Export");
				Param.LoadSettings();
				Param.SetValue(Param.InputType, "Scripture");
				Param.LoadSettings();

                string fileTitle = "GoBibleOutput" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() +
                                   DateTime.Now.Year.ToString();

                _isLinux = Common.UnixVersionCheck();

                string exportGoBibleInputPath = string.Empty;
                exportGoBibleInputPath = Path.GetDirectoryName(projInfo.DefaultCssFileWithPath).Replace("\\\\","\\");
                processFolder = exportGoBibleInputPath;
                PartialBooks.AddChapters(Common.PathCombine(processFolder, "SFM"));
                inProcess.PerformStep();
                CreateCollectionsTextFile(exportGoBibleInputPath, fileTitle);
                inProcess.PerformStep();
                var iconFullName = Common.FromRegistry(Common.PathCombine("GoBible/GoBibleCore", "Icon.png"));
                var iconDirectory = Path.GetDirectoryName(iconFullName);
                _iconFile = Path.GetFileName(iconFullName);
                const bool overwrite = true;
                if (iconDirectory != exportGoBibleInputPath)
                    File.Copy(iconFullName, Common.PathCombine(exportGoBibleInputPath, _iconFile), overwrite);

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

                string goBibleCreatorPath = Common.PathCombine(tempGoBibleCreatorPath, "GoBibleCore");
                inProcess.PerformStep();
                string languageLocationPath = Common.PathCombine(goBibleFullPath, "User Interface");
                languageLocationPath = Common.PathCombine(languageLocationPath, languageSelection);
                string[] filePaths = Directory.GetFiles(languageLocationPath, "*.properties");
                goBibleCreatorPath = Common.PathCombine(goBibleCreatorPath, "ui.properties");

                UIPropertiesCopyToTempFolder(goBibleCreatorPath, filePaths);

                BuildApplication(tempGoBibleCreatorPath);

                string jarFile = string.Empty;
                if (String.IsNullOrEmpty(NoSp(GetInfo(Param.Title))))
                {
                    jarFile = Common.PathCombine(processFolder, fileTitle + ".jar");
                }
                else
                {
                    jarFile = Common.PathCombine(processFolder, NoSp(GetInfo(Param.Title)) + ".jar");
                }
                
                inProcess.PerformStep();
				string caption = LocalizationManager.GetString("ExportGoBible.ExportClick.Caption", "Go Bible Export", "");
                if (File.Exists(jarFile))
                {
                    Common.CleanupExportFolder(projInfo.DefaultXhtmlFileWithPath, ".tmp,.de", string.Empty, string.Empty);
                    CreateRamp(projInfo);
                    Common.DeleteDirectory(tempGoBibleCreatorPath);

                    success = true;
                    Cursor.Current = myCursor;
                    inProcess.PerformStep();
                    inProcess.Close();

                    if (!Common.Testing)
                    {
                        // Failed to send the .jar to a bluetooth device. Tell the user to do it manually.
                        var msg = LocalizationManager.GetString("ExportGoBible.ExportClick.Message1", "Please copy the file {0} to your phone.\n\nDo you want to open the folder?", "");
                        msg = string.Format(msg, jarFile);
						DialogResult dialogResult = Utils.MsgBox(msg, caption, MessageBoxButtons.YesNo,
                                                                    MessageBoxIcon.Information);

                        if (dialogResult == DialogResult.Yes)
                        {
                            string dirPath = Path.GetDirectoryName(jarFile);
                            Process.Start(dirPath);
                        }
                    }

                }
                else
                {
                    success = false;
                    Cursor.Current = myCursor;
                    inProcess.PerformStep();
                    inProcess.Close();
                    if (!Common.Testing)
                    {
                        var msg = LocalizationManager.GetString("ExportGoBible.ExportClick.Message2", "Failed Exporting GoBible Process.", "");
						Utils.MsgBox(msg, caption, MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                    }
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                success = false;
                inProcess.PerformStep();
                inProcess.Close();
            }
            return success;
        }

        public void CreateRamp(PublicationInformation projInfo)
        {
            Ramp ramp = new Ramp();
            ramp.Create(projInfo.DefaultXhtmlFileWithPath, ".jad,.jar", projInfo.ProjectInputType);
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

        public string GoBibleCreatorTempDirectory(string goBibleFullPath)
        {
            var goBibleDirectoryName = Path.GetFileNameWithoutExtension(goBibleFullPath);
            var tempFolder = Path.GetTempPath();
            var folder = Common.PathCombine(tempFolder, goBibleDirectoryName);
            if (Directory.Exists(folder))
            {
                DirectoryInfo di = new DirectoryInfo(folder);
                Common.CleanDirectory(di);
            }
            CopyGoBibleCreatorFolderToTemp(goBibleFullPath, folder);

            return folder;
        }

        public void CopyGoBibleCreatorFolderToTemp(string sourceFolder, string destFolder)
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
                    File.Copy(file, dest, true);
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

        public void CreateCollectionsTextFile(string exportGoBiblePath, string fileTitle)
        {
            string fileLoc = Common.PathCombine(exportGoBiblePath, "Collections.txt");

            if (File.Exists(fileLoc))
            {
                File.Delete(fileLoc);
            }
            using (StreamWriter sw = new StreamWriter(fileLoc))
            {
                var info = "Bible text exported from Paratext, " + GetInfo(Common.UpdateCopyrightYear(Param.CopyrightHolder));
                sw.WriteLine("Info: " + info);

                if (_isLinux)
                {
                    sw.WriteLine(@"Source-Text: /SFM");
                }
                else
                {
                    sw.WriteLine(@"Source-Text: \SFM");
                }

                sw.WriteLine("Source-Format: usfm");
                sw.WriteLine("Source-FileExtension: sfm");
                sw.WriteLine("Phone-Icon-Filepath: Icon.png");
                //sw.WriteLine("Application-Name: " + GetInfo(Param.Title)); - this line makes output unusable (bug in GoBibleCreator?)

				if (string.IsNullOrEmpty(GetInfo(Param.Publisher)))
                sw.WriteLine("MIDlet-Vendor: " + GetInfo(Param.Publisher));
				else
					sw.WriteLine("MIDlet-Vendor: SIL International");

                //sw.WriteLine("MIDlet-Info-URL: http://wap.mygbdomain.org"); - we need to find out best place to post Go Bible modules
                sw.WriteLine("Codepage: UTF-8");
                sw.WriteLine("RedLettering: false");
                if (string.IsNullOrEmpty(Common.BookNameTag))
                    Common.BookNameTag = "id";

                string title = GetInfo(Param.Title);

                if (String.IsNullOrEmpty(title))
                    title = fileTitle;

                sw.WriteLine(@"USFM-TitleTag: \" + Common.BookNameTag);
                sw.WriteLine("Collection: " + title);

                string sfmFiles = Common.PathCombine(exportGoBiblePath, "SFM");

                if (Directory.Exists(sfmFiles))
                {
                    string[] filesList = Directory.GetFiles(sfmFiles);
                    foreach (var name in filesList)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(name);
                        sw.WriteLine("Book: " + fileName);
                    }
                    //Common.BookNameCollection.Clear();
                }
                sw.Flush();
                sw.Close();
            }

            collectionFullName = fileLoc;
        }


        public string GetInfo(string metadataValue)
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

        public bool IsDuplicateBooks(XmlNodeList books)
        {
            _duplicateBooks = new List<string>();
            List<string> allBooks = new List<string>();
            foreach (XmlNode bookNode in books)
            {
                string bookName = bookNode.Value;
                if (allBooks.Contains(bookName))
                {
                    if (!_duplicateBooks.Contains(bookName))
                        _duplicateBooks.Add(bookName);
                }
                else
                {
                    allBooks.Add(bookName);
                }
            }
            return _duplicateBooks.Count > 0;
        }

        /// <summary>
        /// Uses Java to create GoBible application
        /// </summary>
        /// <param name="goBibleCreatorPath"></param>
        public void BuildApplication(string goBibleCreatorPath)
        {
            const string creator = "GoBibleCreator.jar";
            const string prog = "java";
            var creatorFullPath = Common.PathCombine(goBibleCreatorPath, creator);
            //var progFullName = SubProcess.JavaFullName(prog);
            //if (progFullName.EndsWith(".exe"))
            //{
            //    progFullName = progFullName.Substring(0, progFullName.Length - 4);
            //}
            collectionFullName = Common.PathCombine(processFolder, "Collections.txt");
            var args = string.Format(@" -Xmx128m -jar ""{0}""  ""{1}""", creatorFullPath, collectionFullName);
            SubProcess.RedirectOutput = RedirectOutputFileName;
            SubProcess.RunCommand(processFolder, prog, args, true);
        }

        /// <summary>
        /// returns the project name from the path
        /// </summary>
        /// <param name="projInfo">data on project</param>
        /// <returns>Project Name</returns>
        public string GetProjectName(IPublicationInformation projInfo)
        {
            var scrDir = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath);
            var projDir = Path.GetDirectoryName(scrDir);
            return Path.GetFileName(projDir);
        }
    }
}
