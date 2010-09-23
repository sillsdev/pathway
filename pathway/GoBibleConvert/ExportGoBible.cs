// --------------------------------------------------------------------------------------
// <copyright file="ExportGoBible.cs" from='2009' to='2009' company='SIL International'>
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
// Go Bible export
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
using System.IO;
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

        public string ExportType
        {
            get
            {
                return "GoBible";
            }
        }

        public bool Handle(string inputDataType)
        {
            bool returnValue = false;
            if (inputDataType.ToLower() == "scripture")
            {
                returnValue = true;
            }
            return returnValue;
        }

        /// <summary>
        /// Entry point for GoBible export
        /// </summary>
        /// <param name="exportType">scripture / dictionary</param>
        /// <param name="publicationInformation">structure with other necessary information about project.</param>
        /// <returns></returns>
        public bool Launch(string exportType, PublicationInformation publicationInformation)
        {
            //if (!Handle(exportType))
            //    return false;
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
            //try
            //{
            var curdir = Environment.CurrentDirectory; // $(Desktop)\\Scripture
            //MessageBox.Show(string.Format("Preparing to convert {0} for cell phone", projInfo.DefaultXhtmlFileWithPath), "GoBible Export");
            var myCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            var inProcess = new InProcess(0, 4);
            inProcess.Show();
            inProcess.PerformStep();
            Restructure(projInfo, inProcess);
            inProcess.PerformStep();
            CreateCollection();
            inProcess.PerformStep();
            BuildApplication();
            inProcess.PerformStep();
            inProcess.Close();
            Cursor.Current = myCursor;
            if (projInfo.IsOpenOutput)
            {
                string result = Common.PathCombine(processFolder, collectionName + ".jar");
                string msg = string.Format("Please copy the file {0} to your phone", result);
                MessageBox.Show(msg, "GoBible Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            Environment.CurrentDirectory = curdir;
            success = true;
            //}
            //catch (Exception ex)
            //{
            //    var msg = ex.Message;
            //    success = false;
            //}
            return success;
        }

        protected void Restructure(PublicationInformation projInfo, IInProcess inProcess)
        {
            // The first process for phone apps is to restructure the XHTML file that has been
            // exported from FieldWorks. This starts by identifying the XHTML file to
            // process. The process folder is "$(Local Settings)\\Temp\\Preprocess".

            // Merge the individual CSS stylesheets into one temporary stylesheet and
            // copy it to the process folder.
            string mergedCssPathName = GetMergedCSS(projInfo);
            // Preprocess the xhtml file to replace image names, and link to the merged css file.
            string processedXhtmlFile = GetProcessedXhtml(projInfo, mergedCssPathName);

            Common.xsltProgressBar = inProcess.Bar();
            inProcess.AddToMaximum(Chapters(processedXhtmlFile));

            // Next, get the transformation (XSLT) file, which has been put in the folder
            // "C:\SIL\btai\PublishingSolution\PublishingSolutionExe\bin\Debug".
            // The path for the default XHTML file is "$(Desktop)\\Scripture".
            string cvFileName = Path.GetFileNameWithoutExtension(projInfo.DefaultXhtmlFileWithPath) + "_cv";
            const string xsltName = "TE_XHTML-to-Phone_XHTML.xslt";
			string xsltFullName = Common.FromRegistry(xsltName);
            processFolder = Path.GetDirectoryName(processedXhtmlFile);

            // Transform the given XHTML file into a restructured version. Copy the results into
            // the project solution folder, "$(Desktop)\\Scripture".
            //Common.Xslt2Process(processedXhtmlFile, xsltFullName, "_cv.xhtml");
            Common.XsltProcess(processedXhtmlFile, xsltFullName, "_cv.xhtml");
            string temporaryCvFullName = Common.PathCombine(processFolder, cvFileName + ".xhtml");
            string projectPath = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath);
            restructuredFullName = Path.Combine(projectPath, cvFileName + ".xhtml");
            if (restructuredFullName != temporaryCvFullName)
                File.Copy(temporaryCvFullName, restructuredFullName, true);
        }

        protected static int Chapters(string name)
        {
            if (!File.Exists(name))
                Thread.Sleep(300);
            XmlDocument xmlDocument = new XmlDocument {XmlResolver = null};
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings {XmlResolver = null, ProhibitDtd = false};
            XmlReader xmlReader = XmlReader.Create(name, xmlReaderSettings);
            xmlDocument.Load(xmlReader);
            xmlReader.Close();
            var nodes = xmlDocument.SelectNodes("//xhtml:span[@class='Chapter_Number']", namespaceManager);
            return nodes.Count;
        }

        /// <summary>
        /// Create collections files (using variables set up by Restructure
        /// </summary>
        protected void CreateCollection()
        {
            var sourceText = Path.GetFileName(restructuredFullName);

            // Calculate collection Name
            char[] delim = new char[] { '_' };
            var sourceNameSections = Path.GetFileNameWithoutExtension(sourceText).Split(delim);
            collectionName = sourceNameSections[0];

            // Calculate book list
            XmlDocument xmlDocument = new XmlDocument { XmlResolver = null };
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings {XmlResolver = null, ProhibitDtd = false};
            XmlReader xmlReader = XmlReader.Create(restructuredFullName, xmlReaderSettings);
            xmlDocument.Load(xmlReader);
            xmlReader.Close();
            XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            xmlNamespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            var books = xmlDocument.SelectNodes("//xhtml:div[@class='scrBook']/@title", xmlNamespaceManager);
            collectionFullName = Path.Combine(processFolder, "Collections.txt");

            // Set Default Collection Parameters
            var red = "false";
            var info = "Bible text exported from FieldWorks Translation Editor.";
			var iconPath = Common.FromRegistry(Common.PathCombine("GoBible/GoBibleCore/Icon", "Icon.png"));

            // Load User Interface Collection Parameters
            Param.LoadSettings();
            Param.SetValue(Param.InputType, "Scripture");
            Param.LoadSettings();
            Dictionary<string, string> mobilefeature = Param.GetItemsAsDictionary("//mobileProperty/mobilefeature");
            if (mobilefeature.ContainsKey("RedLetter") && mobilefeature["RedLetter"] == "Yes")
                red = "true";
            if (mobilefeature.ContainsKey("Information"))
                info = mobilefeature["Information"].Trim();
            if (mobilefeature.ContainsKey("Copyright"))
                info += " " + mobilefeature["Copyright"].Trim();
            if (mobilefeature.ContainsKey("Icon"))
                iconPath = mobilefeature["Icon"];

            // Write collection file
            TextWriter textWriter = new StreamWriter(collectionFullName);
            textWriter.WriteLine("Info: " + info);
            textWriter.WriteLine("Phone-Icon-Filepath: " + iconPath); // path to 20x20 *.png icon file
            textWriter.WriteLine("RedLettering: " + red);  // relies on correct tag
            textWriter.WriteLine("Source-Text: " + sourceText);
            textWriter.WriteLine("Source-Format: xhtml_te");
            textWriter.WriteLine("Collection: " + collectionName);
            foreach (XmlNode xmlNode in books)
            {
                textWriter.WriteLine("Book: " + xmlNode.Value);
            }
            textWriter.Close();
        }

        protected void BuildApplication()
        {
            const string Creator = "GoBibleCreator.jar";
            const string prog = "java.exe";
            var creatorPath = Common.PathCombine("GoBible", Creator);
			var creatorFullPath = Common.FromRegistry(creatorPath);
            var progFolder = SubProcess.GetLocation(prog);
            var progFullName = Common.PathCombine(progFolder, prog);
            var args = string.Format(@"-Xmx128m -jar ""{0}"" ""{1}""", creatorFullPath, collectionFullName);
            SubProcess.Run(processFolder, progFullName, args, true);
        }

        /// <summary>
        /// Preprocess the xhtml file to replace image names, and link to the merged css file.
        /// </summary>
        /// <param name="projInfo">information about input data</param>
        /// <param name="mergedCSS">full path of file containing style sheet</param>
        /// <returns>path name to processed xthml file</returns>
        private static string GetProcessedXhtml(PublicationInformation projInfo, string mergedCSS)
        {
            //Environment.CurrentDirectory = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath);
            //string processedXhtml = Common.ImagePreprocess(projInfo.DefaultXhtmlFileWithPath);
            //ReplaceSlashToREVERSE_SOLIDUS(processedXhtml);
            //if (projInfo.SwapHeadword)
            //    SwapHeadWordAndReversalForm(processedXhtml);
            //string defaultCSS = Path.GetFileName(mergedCSS);
            //Common.SetDefaultCSS(processedXhtml, defaultCSS);
            //return processedXhtml;
            return projInfo.DefaultXhtmlFileWithPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projInfo"></param>
        /// <returns>mergedCSS</returns>
        private string GetMergedCSS(PublicationInformation projInfo)
        {
            PreExportProcess preProcessor = new PreExportProcess(projInfo);
            var mc = new MergeCss { OutputLocation = "Preprocess" };
            string mergedCSS = mc.Make(projInfo.DefaultCssFileWithPath);
            preProcessor.ReplaceStringInCss(mergedCSS);
            preProcessor.SetDropCapInCSS(mergedCSS);
            var savedCss = Common.PathCombine(Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath),
                                              Path.GetFileName(mergedCSS));
            File.Copy(mergedCSS, savedCss);
            return savedCss;
        }
    }
}
