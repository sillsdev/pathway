// --------------------------------------------------------------------------------------
// <copyright file="ExportLogos.cs" from='2009' to='2009' company='SIL International'>
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
// </remarks>
// --------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class ExportLogos : IExportProcess 
    {
        protected string processFolder;
        protected string restructuredFullName;
        protected string outputPathBase;
        protected string outputNameBase;
        protected static ProgressBar _pb;

        public string ExportType
        {
            get
            {
                return "Logos Alpha";
            }
        }

        public bool Handle(string inputDataType)
        {
            bool returnValue = false;
            if (inputDataType.ToLower() == "scripture" || inputDataType.ToLower() == "dictionary")
            {
                returnValue = true;
            }
            return returnValue;
        }

        /// <summary>
        /// Entry point for Logos export
        /// </summary>
        /// <param name="exportType">scripture / dictionary</param>
        /// <param name="publicationInformation">structure with other necessary information about project.</param>
        /// <returns></returns>
        public bool Launch(string exportType, PublicationInformation publicationInformation)
        {
            return Export(publicationInformation);
        }

        /// <summary>
        /// Entry point for Logos converter
        /// </summary>
        /// <param name="projInfo">values passed including xhtml and css names</param>
        /// <returns>true if succeeds</returns>
        public bool Export(PublicationInformation projInfo)
        {
            if (projInfo == null)
                throw new ArgumentNullException("projInfo");
            var curdir = Environment.CurrentDirectory;
            try
            {
                var myCursor = Cursor.Current;
                Cursor.Current = Cursors.WaitCursor;
                CreateMetadata(projInfo);
                CreateStylesheet(projInfo);
                CreatePopups(projInfo);
                CreateContent(projInfo);
                ZipResults(projInfo);
                Cursor.Current = myCursor;
                if (projInfo.IsOpenOutput)
                {
                    string result = outputPathBase + ".zip";
                    string msg = string.Format("Please submit the file {0} for compiling", result);
                    MessageBox.Show(msg, "Logos Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                UpdateGUID(projInfo);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Logos Export", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                Environment.CurrentDirectory = curdir;
            }
            return true;
        }

        private void UpdateGUID(PublicationInformation projInfo)
        {
            string FileGuidPath = Path.Combine(Common.GetPSApplicationPath(),"FileGuids.xml");
            if (!File.Exists(FileGuidPath)) return;
            XmlDocument xmlDoc = new XmlDocument { XmlResolver = null };
            xmlDoc.Load(FileGuidPath);
            string outputName = Path.GetFileNameWithoutExtension(projInfo.DefaultXhtmlFileWithPath);
            string xPath = "//files/file[@name='" + outputName + "']";
            //"//st:style[@st:name='" + name.Value + "']
            var node = xmlDoc.SelectSingleNode(xPath);
            if (node == null)
            {
                string newGuid = Guid.NewGuid().ToString();
                XmlElement elmRoot = xmlDoc.DocumentElement;

                XmlElement newFile = xmlDoc.CreateElement("file");

                // Add the Id attribute.
                XmlAttribute attName = xmlDoc.CreateAttribute("name");
                attName.Value = outputName;
                newFile.Attributes.Append(attName);

                XmlAttribute attGUID = xmlDoc.CreateAttribute("guid");
                attName.Value = newGuid;
                newFile.Attributes.Append(attGUID);

                elmRoot.AppendChild(newFile);

                xmlDoc.Save(FileGuidPath);
            }

        }

        private void ZipResults(PublicationInformation projInfo)
        {
            string processedXhtmlFile = projInfo.DefaultXhtmlFileWithPath;
            outputNameBase = Path.GetFileNameWithoutExtension(processedXhtmlFile);
            outputPathBase = Common.PathCombine(Path.GetDirectoryName(processedXhtmlFile), outputNameBase);
            outputNameBase = "SL_BI_" + outputNameBase;
            processFolder = Path.GetDirectoryName(processedXhtmlFile);
            if (Directory.Exists(outputPathBase))
                Directory.Delete(outputPathBase);
            Directory.CreateDirectory(outputPathBase);
            File.Move(outputPathBase + ".xml", Common.PathCombine(outputPathBase, outputNameBase + ".xml"));
            File.Move(outputPathBase + "_Content.xml", Common.PathCombine(outputPathBase, outputNameBase + "_Content.xml"));
            File.Move(outputPathBase + "_Styles.xml", Common.PathCombine(outputPathBase, outputNameBase + "_Styles.xml"));
            File.Move(outputPathBase + "_Popups.xml", Common.PathCombine(outputPathBase, outputNameBase + "_Popups.xml"));
            var zipFolder = new ZipFolder();
            zipFolder.CreateZip(outputPathBase, outputPathBase + ".zip", 0);
        }

        private void CreateContent(PublicationInformation projInfo)
        {
            ApplyTransform(projInfo, "TE_XHTML-to-Libronix_Content.xslt", "_Content");
        }

        private void CreatePopups(PublicationInformation projInfo)
        {
            ApplyTransform(projInfo, "TE_XHTML-to-Libronix_Popups.xslt", "_Popups");
        }

        private void CreateStylesheet(PublicationInformation projInfo)
        {
            ApplyTransform(projInfo, "TE_XHTML-to-Libronix_Styles.xslt", "_Styles");
        }

        private void CreateMetadata(PublicationInformation projInfo)
        {
            ApplyTransform(projInfo, "TE_XHTML-to-Libronix_Metadata.xslt", "");
        }

        protected void ApplyTransform(PublicationInformation projInfo, string xsltName, string ending)
        {
            string processedXhtmlFile = projInfo.DefaultXhtmlFileWithPath;
			string xsltFullName = Common.FromRegistry(xsltName);
            processFolder = Path.GetDirectoryName(processedXhtmlFile);
            var xsltParam = new Dictionary<string, string> {{"currentYear", DateTime.Now.Year.ToString()}};
            Common.XsltProcess(processedXhtmlFile, xsltFullName, ending + ".xml", xsltParam);
        }
    }
}
