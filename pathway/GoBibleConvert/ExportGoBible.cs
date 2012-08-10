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
            try
            {
                var curdir = Environment.CurrentDirectory;
                var myCursor = Cursor.Current;
                Cursor.Current = Cursors.WaitCursor;
                var inProcess = new InProcess(0, 6);
                inProcess.Show();
                inProcess.PerformStep();
                string exportGoBiblePath = string.Empty;
                exportGoBiblePath = Path.GetDirectoryName(projInfo.DefaultCssFileWithPath);
                processFolder = exportGoBiblePath;
                CreateCollectionsTextFile(exportGoBiblePath);
                var iconFullName = Common.FromRegistry(Common.PathCombine("GoBible/GoBibleCore", "Icon.png"));
                var iconDirectory = Path.GetDirectoryName(iconFullName);
                _iconFile = Path.GetFileName(iconFullName);
                const bool overwrite = true;
                if (iconDirectory != exportGoBiblePath)
                    File.Copy(iconFullName, Path.Combine(exportGoBiblePath, _iconFile), overwrite);

                BuildApplication();
                success = true;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                success = false;
            }
            return success;
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
                sw.WriteLine(@"USFM-TitleTag: \h");
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

        protected void BuildApplication()
        {
            const string Creator = "GoBibleCreator.jar";
            const string prog = "java";
            var creatorPath = Common.PathCombine("GoBible", Creator);
            var creatorFullPath = Common.FromRegistry(creatorPath);
            var progFolder = SubProcess.GetLocation(prog);
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
