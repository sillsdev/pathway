// --------------------------------------------------------------------------------------
// <copyright file="ExportTheWord.cs" from='2012' to='2014' company='SIL International'>
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
// See: http://www.theword.net/
// </remarks>
// --------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class ExportTheWord : IExportProcess
    {
        protected static int Verbosity;
        protected static object ParatextData;
        protected static string Ssf;
        protected static bool R2L;
        protected static string VrsName = "vrs.xml";
        private static object _theWorProgPath;
        private static bool _hasMessages;
        protected static string MessageFullName;

        protected static readonly XslCompiledTransform TheWord = new XslCompiledTransform();

        public string ExportType
        {
            get
            {
                return "theWord/MySword";
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
            if (projInfo == null) return false;
            bool success;
            var myCursor = Cursor.Current;
            var originalDir = Environment.CurrentDirectory;
            var inProcess = new InProcess(0, 7);
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                var assemblyLocation = Assembly.GetExecutingAssembly().Location;
                var codePath = Path.GetDirectoryName(assemblyLocation);
                Debug.Assert(codePath != null);
                Environment.CurrentDirectory = codePath;

                inProcess.Show();

                LoadXslt();
                inProcess.PerformStep();

                var exportTheWordInputPath = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath);
                Debug.Assert(exportTheWordInputPath != null);

                LoadMetadata();
                inProcess.PerformStep();

                FindParatextProject();
                inProcess.PerformStep();

                var xsltArgs = LoadXsltParameters();
                inProcess.PerformStep();

                var otBooks = new List<string>();
                var ntBooks = new List<string>();
                CollectTestamentBooks(otBooks, ntBooks);
                inProcess.PerformStep();

                var output = GetSsfValue("//EthnologueCode", "zxx");
                var fullName = UsxDir(exportTheWordInputPath);
                LogStatus("Processing: {0}", fullName);
                xsltArgs.XsltMessageEncountered += XsltMessage;
                MessageFullName = Common.PathCombine(exportTheWordInputPath, "Messages.html");

                var codeNames = new Dictionary<string, string>();
                var otFlag = OtFlag(fullName, codeNames, otBooks);
                inProcess.AddToMaximum(codeNames.Count*2);
                inProcess.PerformStep();

                var resultName = output + (otFlag ? ".ont" : ".nt");
                var resultFullName = Common.PathCombine(exportTheWordInputPath, resultName);
                ProcessAllBooks(resultFullName, otFlag, otBooks, ntBooks, codeNames, xsltArgs, inProcess);

                if (_hasMessages)
                {
                    XsltMessageClose();
                    DisplayMessageReport();
                }
                xsltArgs.XsltMessageEncountered -= XsltMessage;

                string theWordFullPath = Common.FromRegistry("TheWord");
                string tempTheWordCreatorPath = TheWordCreatorTempDirectory(theWordFullPath);
                xsltArgs.AddParam("refPref", "", "b");
                inProcess.PerformStep();

                var mySwordFullName = Common.PathCombine(tempTheWordCreatorPath, resultName);
                ProcessAllBooks(mySwordFullName, otFlag, otBooks, ntBooks, codeNames, xsltArgs, inProcess);

                var mySwordResult = ConvertToMySword(resultName, tempTheWordCreatorPath, exportTheWordInputPath);
                inProcess.PerformStep();

                if (Directory.Exists(tempTheWordCreatorPath))
                {
                    Common.DeleteDirectory(tempTheWordCreatorPath);
                }

                inProcess.Close();
                Environment.CurrentDirectory = originalDir;
                Cursor.Current = myCursor;

                success = ReportResults(resultFullName, mySwordResult, exportTheWordInputPath);

                Common.CleanupExportFolder(projInfo.DefaultXhtmlFileWithPath, ".tmp,.de", "layout.css", string.Empty);
                CreateRamp(projInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                success = false;
                inProcess.PerformStep();
                inProcess.Close();
                Environment.CurrentDirectory = originalDir;
                Cursor.Current = myCursor;
                ReportFailure(ex);
            }
            Ssf = string.Empty;
            return success;
        }

        protected void DisplayMessageReport()
        {
            var result = !Common.Testing? MessageBox.Show("Display issues encountered during conversion?", "theWord Conversion Messages",
                                            MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error,
                                            MessageBoxDefaultButton.Button1) : DialogResult.No;
            switch (result)
            {
                case DialogResult.Yes:
                    Process.Start(MessageFullName);
                    break;
                case DialogResult.No:
                    break;
                case DialogResult.Cancel:
                    File.Delete(MessageFullName);
                    break;
            }
        }

        protected void CreateRamp(IPublicationInformation projInfo)
        {
            var ramp = new Ramp();
            ramp.Create(projInfo.DefaultXhtmlFileWithPath, ".mybible,.nt,.ont", projInfo.ProjectInputType);
        }

        protected static void ReportFailure(Exception ex)
        {
            if (Common.Testing) return;
            if (ex.Message.Contains("BookNames"))
            {
                MessageBox.Show("Please run the References basic check.", "theWord Export", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(ex.Message, "theWord Export", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected static bool ReportResults(string resultFullName, string mySwordResult, string exportTheWordInputPath)
        {
            bool success;

            if (File.Exists(resultFullName))
            {
                var appData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                var theWordFolder = Common.PathCombine(Common.PathCombine(appData, "The Word"), "Bibles");
                try
                {
                    RegistryHelperLite.RegEntryExists(RegistryHelperLite.TheWordKey, "RegisteredLocation", "",
                                                      out _theWorProgPath);
                }
                catch (SecurityException) // on Ubuntu this error comes too
                {
                    _theWorProgPath = null;
                }
                if (_theWorProgPath != null)
                {
                    if (!File.Exists((string) _theWorProgPath))
                    {
                        _theWorProgPath = null;
                    }
                }
                if (Directory.Exists(theWordFolder) && _theWorProgPath != null)
                {
                    ReportWhenTheWordInstalled(resultFullName, theWordFolder, mySwordResult, exportTheWordInputPath);
                }
                else
                {
                    ReportWhenTheWordNotInstalled(resultFullName, theWordFolder, mySwordResult, exportTheWordInputPath);
                }
                success = true;
            }
            else
            {
                if (!Common.Testing)
                {
                    MessageBox.Show("Failed Exporting TheWord Process.", "theWord Export", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
                success = false;
            }
            return success;
        }

        protected static void ReportWhenTheWordNotInstalled(string resultFullName, string theWordFolder, string mySwordResult, string exportTheWordInputPath)
        {
            const string msgFormat = "Do you want to open the folder with the results?\n\n\u25CF Click Yes.\n\nThe folder with the \"{0}\" file ({2}) will open so you can manually copy it to {1}.\n\nThe MySword file \"{3}\" is also there so you can copy it to your Android device or send it to pathway@sil.org for uploading. \n\n\u25CF Click Cancel to do neither of the above.\n";
            var resultName = Path.GetFileName(resultFullName);
            var resultDir = Path.GetDirectoryName(resultFullName);
            var msg = string.Format(msgFormat, resultName, theWordFolder, resultDir, Path.GetFileName(mySwordResult));
            var dialogResult = !Common.Testing ? MessageBox.Show(msg, "theWord Export", MessageBoxButtons.YesNo,
                MessageBoxIcon.Information) : DialogResult.Cancel;

            if (dialogResult == DialogResult.Yes)
            {
                LaunchFileNavigator(exportTheWordInputPath);
            }
        }

        protected static void ReportWhenTheWordInstalled(string resultFullName, string theWordFolder, string mySwordResult, string exportTheWordInputPath)
        {
            const string msgFormat = "Do you want to start theWord?\n\n\u25CF Click Yes.\n\nThe program will copy the \"{0}\" file to {1} and start theWord. \n\n\u25CF Click No. \n\nThe folder with the \"{0}\" file ({2}) will open so you can manually copy it to {1}.\n\nThe MySword file \"{3}\" is also there so you can copy it to your Android device or send it to pathway@sil.org for uploading. \n\n\u25CF Click Cancel to do neither of the above.\n";
            var resultName = Path.GetFileName(resultFullName);
            var resultDir = Path.GetDirectoryName(resultFullName);
            var msg = string.Format(msgFormat, resultName, theWordFolder, resultDir, Path.GetFileName(mySwordResult));
            var dialogResult = !Common.Testing ? MessageBox.Show(msg, "theWord Export", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) : DialogResult.Cancel;

            if (dialogResult == DialogResult.Yes)
            {
                const bool overwrite = true;
                File.Copy(resultFullName, Common.PathCombine(theWordFolder, resultName), overwrite);
                Process.Start((string) _theWorProgPath);
            }
            else if (dialogResult == DialogResult.No)
            {
                LaunchFileNavigator(exportTheWordInputPath);
            }
        }

        protected static void LaunchFileNavigator(string exportTheWordInputPath)
        {
            const bool noWait = false;
            SubProcess.Run(exportTheWordInputPath, Common.IsUnixOS() ? "nautilus" : "explorer.exe", exportTheWordInputPath, noWait);
        }

        protected static void LoadMetadata()
        {
            Param.LoadSettings();
            Param.SetValue(Param.InputType, "Scripture");
            Param.LoadSettings();
        }

        protected static void LoadXslt()
        {
            var xsltSettings = new XsltSettings { EnableDocumentFunction = true };
            var inputXsl = Assembly.GetExecutingAssembly().GetManifestResourceStream("SIL.PublishingSolution.theWord.xsl");
            Debug.Assert(inputXsl != null);
            var readerSettings = new XmlReaderSettings {XmlResolver = FileStreamXmlResolver.GetNullResolver()};
            var reader = XmlReader.Create(inputXsl, readerSettings);
            TheWord.Load(reader, xsltSettings, null);
        }

        protected static void CollectTestamentBooks(List<string> otBooks, List<string> ntBooks)
        {
            var xmlDoc = Common.DeclareXMLDocument(true);
            var vfs = new StreamReader(VrsName);
            xmlDoc.Load(vfs);
            vfs.Close();
            var codeNodes = xmlDoc.SelectNodes("//@code");
            Debug.Assert(codeNodes != null);
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

        protected void ProcessAllBooks(string fullName, bool otFlag, IEnumerable<string> otBooks, IEnumerable<string> ntBooks, Dictionary<string, string> codeNames, XsltArgumentList xsltArgs, IInProcess inProcess)
        {
            LogStatus("Creating MySword: {0}", Path.GetFileName(fullName));
            // false = do not append but overwrite instead
            var sw = new StreamWriter(fullName, false, new UTF8Encoding(true));
            if (otFlag)
            {
                ProcessTestament(otBooks, codeNames, xsltArgs, sw, inProcess);
            }
            ProcessTestament(ntBooks, codeNames, xsltArgs, sw, inProcess);
            AttachMetadata(sw);
            sw.Close();
        }

        private static void XsltMessage(object o, XsltMessageEncounteredEventArgs a)
        {
            var message = a.Message;
            PostTransformMessage(message);
        }

        private static StreamWriter _messageStream;
        protected static void PostTransformMessage(string message)
        {
            LogStatus("Message {0}", message);
            if (!_hasMessages)
            {
                _hasMessages = true;
                _messageStream = new StreamWriter(MessageFullName);
                _messageStream.WriteLine("<html><head><title>theWord conversion messages</title></head>\n<body>\n<ul>");
            }
            _messageStream.WriteLine("<li>{0}</li>", message);
        }

        protected static void XsltMessageClose()
        {
            _messageStream.WriteLine("</ul>\n</body>\n</html>");
            _messageStream.Close();
            _hasMessages = false;
        }

        protected static void ProcessTestament(IEnumerable<string> books, Dictionary<string, string> codeNames, XsltArgumentList xsltArgs,
                                             StreamWriter sw, IInProcess inProcess)
        {
            foreach (string book in books)
            {
                if (codeNames.ContainsKey(book))
                {
                    LogStatus("Processing {0}", codeNames[book]);
                    Transform(codeNames[book], xsltArgs, sw);
                    inProcess.PerformStep();
                }
                else
                {
                    LogStatus("Creating empty {0}", book);
                    var tempName = TempName(book);
                    Transform(tempName, xsltArgs, sw);
                    File.Delete(tempName);
                }
            }
        }

        protected static void Transform(string name, XsltArgumentList xsltArgs, StreamWriter sw)
        {
            //var readerSettings = new XmlReaderSettings {XmlResolver = FileStreamXmlResolver.GetNullResolver()};
            var readerSettings = new XmlReaderSettings();
            var reader = XmlReader.Create(name, readerSettings);
            TheWord.Transform(reader, xsltArgs, sw);
            reader.Close();
        }

        protected static string TempName(string book)
        {
            var tempName = Path.GetTempFileName();
            var tempStream = new StreamWriter(tempName);
            tempStream.Write(string.Format("<usx><book code= \"{0}\"/></usx>", book));
            tempStream.Close();
            return tempName;
        }

        protected static bool OtFlag(string fullName, Dictionary<string, string> codeNames, List<string> otBooks)
        {
            var codeStart = -1;
            var otFlag = false;
            foreach (string file in Directory.GetFiles(fullName, "*.usx"))
            {
                if (codeStart == -1)
                {
                    var fileDir = Path.GetDirectoryName(file);
                    Debug.Assert(fileDir != null);
                    codeStart = fileDir.Length + 1;
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

        protected static void FindParatextProject()
        {
            if (!string.IsNullOrEmpty(Ssf)) return;
            RegistryHelperLite.RegEntryExists(RegistryHelperLite.ParatextKey, "Settings_Directory", "", out ParatextData);
            var sh = new SettingsHelper(Param.DatabaseName);
            Ssf = sh.GetSettingsFilename();
        }

        protected static string GetSsfValue(string xpath, string def)
        {
            var node = Common.GetXmlNode(Ssf, xpath);
            return (node != null)? node.InnerText : def;
        }

        protected static string GetSsfValue(string xpath)
        {
// Default Parameters are not allowed in Team City build server.
// ReSharper disable IntroduceOptionalParameters.Global
            return GetSsfValue(xpath, null);
// ReSharper restore IntroduceOptionalParameters.Global
        }

        protected static XsltArgumentList LoadXsltParameters()
        {
            var xsltArgs = new XsltArgumentList();
            xsltArgs.AddParam("refPunc", "", GetSsfValue("//ChapterVerseSeparator", ":"));
            xsltArgs.AddParam("bookNames", "", GetBookNamesUri());
            xsltArgs.AddParam("versification", "", VrsName);
            GetRtlParam(xsltArgs);
            return xsltArgs;
        }

        protected static void GetRtlParam(XsltArgumentList xsltArgs)
        {
            R2L = false;
            var language = GetSsfValue("//Language", "English");
            var languagePath = Path.GetDirectoryName(Ssf);
            var languageFile = Common.PathCombine(languagePath, language);
            if (File.Exists(languageFile + ".LDS"))
            {
                languageFile += ".LDS";
            }
            else if (File.Exists(languageFile + ".lds"))
            {
                languageFile += ".lds";
            }
            else
            {
                return;
            }
            foreach (string line in FileData.Get(languageFile).Split(new[] {'\n'}))
            {
                var cleanLine = line.ToLower().Trim();
                if (cleanLine.StartsWith("rtl="))
                {
                    if (cleanLine.Length > 4 && cleanLine.Substring(4, 1) == "t")
                    {
                        xsltArgs.AddParam("rtl", "", "1");
                        R2L = true;
                    }
                }
            }
        }

        protected static string GetBookNamesUri()
        {
            var myProj = Common.PathCombine((string) ParatextData, GetSsfValue("//Name"));
            return "file:///" + Common.PathCombine(myProj, "BookNames.xml");
        }

        protected static string UsxDir(string exportTheWordInputPath)
        {
            var usxDir = Common.PathCombine(exportTheWordInputPath, "USX");
            if (!Directory.Exists(usxDir))
            {
                throw new FileNotFoundException("No USX folder");
            }
            return usxDir;
        }

        protected string TheWordCreatorTempDirectory(string theWordFullPath)
        {
            var theWordDirectoryName = Path.GetFileNameWithoutExtension(theWordFullPath);
            Debug.Assert(theWordDirectoryName != null);
            var tempFolder = Path.GetTempPath();
            var folder = Common.PathCombine(tempFolder, theWordDirectoryName);
            CopyTheWordFolderToTemp(theWordFullPath, folder);
            try
            {
                FolderTree.Copy(Common.PathCombine(folder, Directory.Exists(@"C:\Program Files (x86)") ? "x64" : "x32"), folder);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return folder;
        }

        protected void CopyTheWordFolderToTemp(string sourceFolder, string destFolder)
        {
            if (Directory.Exists(destFolder))
            {
                Common.DeleteDirectory(destFolder);
            }
            Directory.CreateDirectory(destFolder);
            try
            {
                FolderTree.Copy(sourceFolder, destFolder);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        protected static string ConvertToMySword(string resultName, string tempTheWordCreatorPath, string exportTheWordInputPath)
        {
			const string converterName = "TheWordBible2MySword.exe";
			var processStartInfo = new ProcessStartInfo {
	                Arguments = resultName,
	                FileName = converterName,
	                WorkingDirectory = tempTheWordCreatorPath,
	                CreateNoWindow = true
	            };
			if (Common.IsUnixOS())
			{
				processStartInfo.Arguments = string.Format("{0} {1}", converterName, resultName);
				processStartInfo.FileName = "mono";
			}
            Process.Start(processStartInfo).WaitForExit();
            var mySwordFiles = Directory.GetFiles(tempTheWordCreatorPath, "*.mybible");
            var mySwordResult = "<No MySword Result>";
            if (mySwordFiles.Length >= 1)
            {
                var mySwordName = Path.GetFileName(mySwordFiles[0]);
                Debug.Assert(mySwordName != null);
                mySwordResult = Common.PathCombine(exportTheWordInputPath, mySwordName);
                File.Copy(mySwordFiles[0], mySwordResult);
            }
            return mySwordResult;
        }

        protected void AttachMetadata(StreamWriter sw)
        {
            const string format = @"verse.rule=""(<a href[^>]+>)(.*?)(</a>)"" ""$1<font color=defclr6>$2</font>$3""
id=W{0}
charset=0
lang={0}
font={9}
short.title={1}
title={2}
description={3} \
{4}
version.major=1
version.minor=0
version.date={5}
publisher={6}
publish.date={7}
author={6}
creator={8}
source={6}
about={2} \
<p>{4} \
<p>\
<p> . . . . . . . . . . . . . . . . . . .\
<p><b>Creative Commons</b> <i>Atribution-Non Comercial-No Derivatives 4.0</i>\
<p><font color=blue><i>http://creativecommons.org/licenses/by-nc-nd/4.0</i></font>\
";
            var langCode = GetSsfValue("//EthnologueCode", "zxx");
            const bool isConfigurationTool = false;
            var shortTitle = Param.GetTitleMetadataValue("Title", Param.GetOrganization(), isConfigurationTool);
            var fullTitle = GetSsfValue("//FullName", shortTitle);
            var description = Param.GetMetadataValue("Description");
            var copyright = Common.UpdateCopyrightYear(Param.GetMetadataValue("Copyright Holder"));
            var createDate = DateTime.Now.ToString("yyyy.M.d");
            var publisher = Param.GetMetadataValue("Publisher");
            var publishDate = createDate;
            var creator = Param.GetMetadataValue("Creator");
            var font = GetSsfValue("//DefaultFont", "Charis SIL");
            var myFormat = GetFormat("theWordFormat.txt", format);
            if (R2L)
            {
                font += "\r\nr2l=1";
            }
            sw.Write(string.Format(myFormat, langCode, shortTitle, fullTitle, description, copyright, createDate, publisher, publishDate, creator, font));
        }

        protected string GetFormat(string thewordformatTxt, string format)
        {
            var folder = Common.GetAllUserPath();
            var fullPath = Common.PathCombine(Common.PathCombine(folder, "Scripture"), thewordformatTxt);
            if (File.Exists(fullPath))
            {
                var curFormat = FileData.Get(fullPath);
                if (curFormat.StartsWith("verse.rule")) //TD-3785 verse rule is necessary
                {
                    return curFormat;
                }
            }
            if (!Common.Testing)
            {
                var sw = new StreamWriter(fullPath);
                sw.Write(format);
                sw.Close();
            }
            return format;
        }

        protected static void LogStatus(string format, params object[] args)
        {
            if (Verbosity <= 0) return;
            Console.Write("# ");
            Console.WriteLine(format, args);
        }

    }
}
