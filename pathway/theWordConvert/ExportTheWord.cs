// --------------------------------------------------------------------------------------
// <copyright file="ExportTheWord.cs" from='2012' to='2012' company='SIL International'>
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
// See: http://www.theword.net/
// </remarks>
// --------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class ExportTheWord : IExportProcess
    {
        private static readonly XslCompiledTransform TheWord = new XslCompiledTransform();
        protected string processFolder;
        protected string restructuredFullName;
        protected string collectionFullName;
        protected string collectionName;
        protected static ProgressBar _pb;
        private const string RedirectOutputFileName = "Convert.log";

        public string ExportType
        {
            get
            {
                return "theWord";
            }
        }

        public bool Handle(string inputDataType)
        {
            return inputDataType.ToLower() == "scripture";
        }

        /// <summary>
        /// Entry point for TheWord export
        /// </summary>
        /// <param name="exportType">scripture / dictionary</param>
        /// <param name="publicationInformation">structure with other necessary information about project.</param>
        /// <returns></returns>
        public bool Launch(string exportType, PublicationInformation publicationInformation)
        {
            return Export(publicationInformation);
        }

        /// <summary>
        /// Entry point for TheWord converter
        /// </summary>
        /// <param name="projInfo">values passed including xhtml and css names</param>
        /// <returns>true if succeeds</returns>
        public bool Export(PublicationInformation projInfo)
        {
            bool success;
            var inProcess = new InProcess(0, 6);
            try
            {
                var myCursor = Cursor.Current;
                Cursor.Current = Cursors.WaitCursor;
                
                inProcess.Show();
                inProcess.PerformStep();
                var xsltSettings = new XsltSettings() { EnableDocumentFunction = true };
                var inputXsl = Assembly.GetExecutingAssembly().GetManifestResourceStream("SIL.PublishingSolution.theWord.xsl");
                Debug.Assert(inputXsl != null);
                TheWord.Load(XmlReader.Create(inputXsl), xsltSettings, null);
                var exportTheWordInputPath = Path.GetDirectoryName(projInfo.DefaultCssFileWithPath);

                Param.LoadSettings();
                Param.SetValue(Param.InputType, "Scripture");
                Param.LoadSettings();

                string layout = Param.GetItem("//settings/property[@name='LayoutSelected']/@value").Value;

                string TheWordFullPath = Common.FromRegistry("TheWord");
                string tempTheWordCreatorPath = TheWordCreatorTempDirectory(TheWordFullPath);

                success = true;

                inProcess.Close();
                Cursor.Current = myCursor;

                var jarFile = string.Empty;
                if (File.Exists(jarFile))
                {
                    // Failed to send the .jar to a bluetooth device. Tell the user to do it manually.
                    string msg = string.Format("Please copy the file {0} to your phone.\n\nDo you want to open the folder?", jarFile);
                    DialogResult dialogResult = MessageBox.Show(msg, "Go Bible Export", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (dialogResult == DialogResult.Yes)
                    {
                        string dirPath = Path.GetDirectoryName(jarFile);
                        Process.Start(dirPath);
                        //Process.Start("explorer.exe", dirPath);
                    }

                }
                else
                {
                    MessageBox.Show("Failed Exporting TheWord Process.", "Go Bible Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                //DeleteTempFiles(exportTheWordInputPath);
                Common.CleanupExportFolder(exportTheWordInputPath);
                Common.DeleteDirectory(tempTheWordCreatorPath);
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

        private string TheWordCreatorTempDirectory(string theWordFullPath)
        {
            var theWordDirectoryName = Path.GetFileNameWithoutExtension(theWordFullPath);
            var tempFolder = Path.GetTempPath();
            var folder = Path.Combine(tempFolder, theWordDirectoryName);
            if (Directory.Exists(folder))
                Directory.Delete(folder, true);

            CopyTheWordCreatorFolderToTemp(theWordFullPath, folder);

            return folder;
        }

        private void CopyTheWordCreatorFolderToTemp(string sourceFolder, string destFolder)
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
                        CopyTheWordCreatorFolderToTemp(folder, dest);
                    }
                }
            }
            catch
            {
                
            }
        }

        /// <summary>
        /// Uses Java to create TheWord application
        /// </summary>
        /// <param name="theWordCreatorPath"></param>
        protected void BuildApplication(string theWordCreatorPath)
        {
            const string creator = "TheWordCreator.jar";
            const string prog = "java";
            var creatorFullPath = Path.Combine(theWordCreatorPath, creator);
            var progFolder = SubProcess.JavaLocation(prog);
            var progFullName = Common.PathCombine(progFolder, prog);
            if (progFullName.EndsWith(".exe"))
            {
                progFullName = progFullName.Substring(0, progFullName.Length - 4);
            }
            collectionFullName = Common.PathCombine(processFolder, "Collections.txt");
            var args = string.Format(@" -Xmx128m -jar ""{0}""  ""{1}""", creatorFullPath, collectionFullName);

            //var args = "-Xmx128m -jar " + @"""" + creatorFullPath + @"""" + " " + @"""" + collectionFullName + @"""";

            SubProcess.RedirectOutput = RedirectOutputFileName;
            SubProcess.RunCommand(processFolder, "java", args, true);
        }

    }
}
