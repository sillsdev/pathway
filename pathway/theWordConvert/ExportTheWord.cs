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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class ExportTheWord : IExportProcess
    {
        static int _verbosity = 0;

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
            var myCursor = Cursor.Current;
            var originalDir = Environment.CurrentDirectory;
            var inProcess = new InProcess(0, 6);
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                var assemblyLocation = Assembly.GetExecutingAssembly().Location;
                Environment.CurrentDirectory = Path.GetDirectoryName(assemblyLocation);
                
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

                //string layout = Param.GetItem("//settings/property[@name='LayoutSelected']/@value").Value;

                FindParatextProject(exportTheWordInputPath);
                var xsltArgs = new XsltArgumentList();
                xsltArgs.AddParam("refPunc", "", GetSsfValue("//ChapterVerseSeparator"));
                xsltArgs.AddParam("bookNames", "", GetBookNamesUri());

                var otBooks = new List<string>();
                var ntBooks = new List<string>();
                CollectTestamentBooks(otBooks, ntBooks);

                var output = GetSsfValue("//EthnologueCode");
                var fullName = UsxDir(exportTheWordInputPath);
                LogStatus("Processing: {0}", fullName);
                var codeNames = new Dictionary<string, string>();
                var otFlag = OtFlag(fullName, codeNames, otBooks);
                var resultName = output + (otFlag ? ".ont" : ".nt");
                var resultFullName = Path.Combine(exportTheWordInputPath, resultName);
                LogStatus("Creating: {0}", resultName);
                // false = do not append but overwrite instead
                var sw = new StreamWriter(resultFullName, false, new UTF8Encoding(true));
                if (otFlag)
                {
                    ProcessTestament(otBooks, codeNames, xsltArgs, sw);
                }
                ProcessTestament(ntBooks, codeNames, xsltArgs, sw);
                AttachMetadata(sw);
                sw.Close();

                string TheWordFullPath = Common.FromRegistry("TheWord");
                string tempTheWordCreatorPath = TheWordCreatorTempDirectory(TheWordFullPath);
                xsltArgs.AddParam("refPref", "", "b");

                var mySwordFullName = Path.Combine(tempTheWordCreatorPath, resultName);
                LogStatus("Creating MySword: {0}", resultName);
                // false = do not append but overwrite instead
                sw = new StreamWriter(mySwordFullName, false, new UTF8Encoding(true));
                if (otFlag)
                {
                    ProcessTestament(otBooks, codeNames, xsltArgs, sw);
                }
                ProcessTestament(ntBooks, codeNames, xsltArgs, sw);
                AttachMetadata(sw);
                sw.Close();

                var myProc = Process.Start(new ProcessStartInfo
                    {
                        Arguments = resultName,
                        FileName = "TheWordBible2MySword.exe",
                        WorkingDirectory = tempTheWordCreatorPath,
                        CreateNoWindow = true
                    });
                myProc.WaitForExit();
                var mySwordFiles = Directory.GetFiles(tempTheWordCreatorPath, "*.mybible");
                if (mySwordFiles.Length >= 1)
                {
                    File.Copy(mySwordFiles[0], Path.Combine(exportTheWordInputPath, Path.GetFileNameWithoutExtension(mySwordFiles[0])));
                }

                Directory.Delete(tempTheWordCreatorPath, true);
                success = true;

                inProcess.Close();
                Environment.CurrentDirectory = originalDir;
                Cursor.Current = myCursor;

                if (File.Exists(resultFullName))
                {
                    // Tell the user to do it manually.
                    var theWordFolder = @"C:\ProgramData\The Word\Bibles";
                    string msg = string.Format("Please copy the file {0} to {1} for theWord.\nCopy {2} to your the Bibles folder of MySword on your Phone. You can also send pathway@sil.org for uploading.\n\nDo you want to launch theWord?", resultFullName, theWordFolder, mySwordFullName);
                    DialogResult dialogResult = MessageBox.Show(msg, "theWord Export", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);

                    if (dialogResult == DialogResult.Yes)
                    {
                        File.Copy(resultFullName, Path.Combine(theWordFolder, resultName));
                        Process.Start(@"C:\Program Files (x86)\The Word\theword.exe");
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        const bool noWait = false;
                        if (Common.IsUnixOS())
                        {
                            SubProcess.Run(exportTheWordInputPath, "nautilus", exportTheWordInputPath, noWait);
                        }
                        else
                        {
                            SubProcess.Run(exportTheWordInputPath, "explorer.exe", exportTheWordInputPath, noWait);
                        }
                    }

                }
                else
                {
                    MessageBox.Show("Failed Exporting TheWord Process.", "theWord Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                Environment.CurrentDirectory = originalDir;
                Cursor.Current = myCursor;
            }
            return success;
        }

        private void AttachMetadata(StreamWriter sw)
        {
            var format = @"id=W{0}
charset=0
lang={0}
font={8}
short.title={0}
title={1}
description={2} \
{3}
version.major=1
version.minor=0
version.date={4}
publisher={5}
publish.date={6}
author={5}
creator={7}
source={5}
about={1} \
<p>{3} \
<p>\
<p> . . . . . . . . . . . . . . . . . . .\
<p><b>Creative Commons</b> <i>Atribution-Non Comercial-No Derivatives 3.0</i>\
<p><font color=blue><i>http://creativecommons.org/licenses/by-nc-nd/3.0</i></font>\
<p><b>Your are free: <i>To Share</i></b>  — to copy, distribute and transmit the work\
<p><b><i>Under the following conditions:</i></b>\
<p><b>• Attribution.</b> You must attribute the work in the manner specified by the author or licensor (but not in any way that suggests that they endorse you or your use of the work).\
<p><b>• Noncommercial.</b> You may not use this work for commercial purposes.\
<p><b>• No Derivative Works.</b> You may not alter, transform, or build upon this work.\
<p><b><i>With the understanding:</i></b>\
<p><b>• Waiver.</b> Any of the above conditions can be waived if you get permission from the copyright holder.\
<p><b>• Other Rights.</b> In no way are any of the following rights affected by the license:\
<p>— Your fair dealing or fair use rights;\
<p>— The author's moral rights;\
<p>— Rights other persons may have either in the work itself or in how the work is used, such as publicity or privacy rights.\
<p><b>Notice</b> — For any reuse or distribution, you must make clear to others the license terms of this work.\
";
            var langCode = GetSsfValue("//EthnologueCode");
            const bool isConfigurationTool = false;
            var title = Param.GetTitleMetadataValue("Title", Param.GetOrganization(), isConfigurationTool);
            var description = Param.GetMetadataValue("Description");
            var copyright = Param.GetMetadataValue("Copyright Holder");
            var createDate = DateTime.Now.ToString("yyyy.M.d");
            var publisher = Param.GetMetadataValue("Publisher");
            var publishDate = createDate;
            var creator = Param.GetMetadataValue("Creator");
            var font = GetSsfValue("//DefaultFont");
            sw.Write(string.Format(format, langCode, title, description, copyright, createDate, publisher, publishDate, creator, font));
        }

        private static void CollectTestamentBooks(List<string> otBooks, List<string> ntBooks)
        {
            var xmlDoc = Common.DeclareXMLDocument(true);
            var vfs = new StreamReader("vrs.xml");
            xmlDoc.Load(vfs);
            vfs.Close();
            var codeNodes = xmlDoc.SelectNodes("//@code");
            var bkCount = 0;
            foreach (XmlNode node in codeNodes)
            {
                if (++bkCount <= 39)
                {
                    otBooks.Add(node.InnerText);
                }
                else
                {
                    ntBooks.Add(node.InnerText);
                }
            }
        }

        private static void ProcessTestament(IEnumerable<string> books, Dictionary<string, string> codeNames, XsltArgumentList xsltArgs,
                                             StreamWriter sw)
        {
            foreach (string book in books)
            {
                if (codeNames.ContainsKey(book))
                {
                    LogStatus("Processing {0}", codeNames[book]);
                    TheWord.Transform(codeNames[book], xsltArgs, sw);
                }
                else
                {
                    LogStatus("Creating empty {0}", book);
                    var tempName = TempName(book);
                    TheWord.Transform(tempName, xsltArgs, sw);
                    File.Delete(tempName);
                }
            }
        }

        private static string TempName(string book)
        {
            var tempName = Path.GetTempFileName();
            var tempStream = new StreamWriter(tempName);
            tempStream.Write(string.Format("<usx><book code= \"{0}\"/></usx>", book));
            tempStream.Close();
            return tempName;
        }

        private static bool OtFlag(string fullName, Dictionary<string, string> codeNames, List<string> otBooks)
        {
            var codeStart = -1;
            var otFlag = false;
            foreach (string file in Directory.GetFiles(fullName, "*.usx"))
            {
                if (codeStart == -1)
                {
                    codeStart = Path.GetDirectoryName(file).Length + 1;
                }
                var curCode = file.Substring(codeStart, 3);
                codeNames[curCode] = file;
                if (otBooks.Contains(curCode))
                {
                    otFlag = true;
                }
            }
            return otFlag;
        }

        static XmlDocument _ssfDoc;
        static object _paratextData;
        private static bool FindParatextProject(string exportTheWordInputPath)
        {
            _ssfDoc = Common.DeclareXMLDocument(true);
            Debug.Assert(exportTheWordInputPath != null);
            var usxDir = UsxDir(exportTheWordInputPath);
            var usxFiles = Directory.GetFiles(usxDir, "*.usx");
            if (usxFiles.Length < 1)
            {
                throw new FileNotFoundException("No usx files.");
            }
            var xDoc = Common.DeclareXMLDocument(true);
            xDoc.Load(usxFiles[0]);
            var idNode = xDoc.SelectSingleNode("//*[@style='id']");
            if (idNode == null)
            {
                throw new FileLoadException("No id node");
            }
            string idText = idNode.InnerText;
            if (idText.StartsWith("- "))
            {
                idText = idText.Substring(2);
            }
            RegistryHelperLite.RegEntryExists(RegistryHelperLite.ParatextKey, "Settings_Directory", "", out _paratextData);
            foreach (string ssf in Directory.GetFiles((string)_paratextData, "*.ssf"))
            {
                var sr = new StreamReader(ssf);
                _ssfDoc.Load(sr);
                sr.Close();
                var fullNameNode = _ssfDoc.SelectSingleNode("//FullName");
                if (fullNameNode == null)
                {
                    continue;
                }
                if (fullNameNode.InnerText == idText)
                {
                    return true;
                }
            }
            return false;
        }

        private static string GetSsfValue(string xpath)
        {
            var node = _ssfDoc.SelectSingleNode(xpath);
            Debug.Assert(node != null);
            return node.InnerText;
        }

        private static string GetBookNamesUri()
        {
            var myProj = Path.Combine((string) _paratextData, GetSsfValue("//Name"));
            return "file:///" + Path.Combine(myProj, "BookNames.xml");
        }

        private static string UsxDir(string exportTheWordInputPath)
        {
            var usxDir = Path.Combine(exportTheWordInputPath, "USX");
            if (!Directory.Exists(usxDir))
            {
                throw new FileNotFoundException("No USX folder");
            }
            return usxDir;
        }

        private string TheWordCreatorTempDirectory(string theWordFullPath)
        {
            var theWordDirectoryName = Path.GetFileNameWithoutExtension(theWordFullPath);
            var tempFolder = Path.GetTempPath();
            var folder = Path.Combine(tempFolder, theWordDirectoryName);
            if (Directory.Exists(folder))
                Directory.Delete(folder, true);

            CopyTheWordFolderToTemp(theWordFullPath, folder);
            if (Directory.Exists(@"C:\Program Files (x86)"))
            {
                CopyFolderContents(Path.Combine(folder, "x64"), folder);
            }
            else
            {
                CopyFolderContents(Path.Combine(folder, "x32"), folder);
            }

            return folder;
        }

        private void CopyTheWordFolderToTemp(string sourceFolder, string destFolder)
        {
            if (Directory.Exists(destFolder))
            {
                Common.DeleteDirectory(destFolder);
            }
            Directory.CreateDirectory(destFolder);
            CopyFolderContents(sourceFolder, destFolder);
        }

        private void CopyFolderContents(string sourceFolder, string destFolder)
        {
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
                    CopyTheWordFolderToTemp(folder, dest);
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

        static void LogStatus(string format, params object[] args)
        {
            if (_verbosity > 0)
            {
                Console.Write("# ");
                Console.WriteLine(format, args);
            }
        }

    }
}
