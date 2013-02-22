// --------------------------------------------------------------------------------------------
// <copyright file="ExportDic4Mid.cs" from='2013' to='2013' company='SIL International'>
//      Copyright © 2013, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Stylepick FeatureSheet
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Xml;
using SIL.Tool;


namespace SIL.PublishingSolution
{
    public class ExportDic4Mid : IExportProcess
    {
        protected static string WorkDir;
        protected static Dictionary<string, Dictionary<string, string>> CssClass;

        #region Properties
        #region ExportType
        public string ExportType
        {
            get
            {
                return "Dic4Mid";
            }
        }
        #endregion ExportType

        #region Handle
        public bool Handle(string inputDataType)
        {
            return (inputDataType.ToLower() == "dictionary");
        }
        #endregion Handle
        #endregion Properties

        /// <summary>
        /// Entry point for InDesign export
        /// </summary>
        /// <param name="exportType">scripture / dictionary</param>
        /// <param name="publicationInformation">structure with other necessary information about project.</param>
        /// <returns></returns>
        public bool Launch(string exportType, PublicationInformation publicationInformation)
        {
            return Export(publicationInformation);
        }

        public bool Export(PublicationInformation projInfo)
        {
            bool success = false;
            WorkDir = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath);
            bool isUnixOS = Common.UnixVersionCheck();
            var inProcess = new InProcess(0, 6);
            var curdir = Environment.CurrentDirectory;
            var myCursor = Cursor.Current;
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                inProcess.Show();
                inProcess.PerformStep();

                LoadParameters();
                inProcess.PerformStep();

                LoadCss(projInfo);
                inProcess.PerformStep();

                ReformatData(projInfo);
                inProcess.PerformStep();

                CreateProperties(projInfo);
                inProcess.PerformStep();

                CreateDic4Mid(projInfo);
                inProcess.PerformStep();

                CreateSubmission(projInfo);
                inProcess.PerformStep();

                ReportReults(projInfo);
                inProcess.PerformStep();
                success = true;
            }
            catch (Exception)
            {
            }

            inProcess.Close();
            Environment.CurrentDirectory = curdir;
            Cursor.Current = myCursor;
            return success;
        }

        protected void LoadCss(PublicationInformation projInfo)
        {
            var cssTree = new CssTree();
            CssClass = cssTree.CreateCssProperty(projInfo.DefaultCssFileWithPath, true);
        }

        protected void LoadParameters()
        {
            Param.LoadSettings();
            Param.SetValue(Param.InputType, "Dictionary");
            Param.LoadSettings();
        }

        protected void CreateSubmission(PublicationInformation projInfo)
        {
            throw new NotImplementedException();
        }

        protected void ReportReults(PublicationInformation projInfo)
        {
            throw new NotImplementedException();
        }

        protected void CreateProperties(PublicationInformation projInfo)
        {
            throw new NotImplementedException();
        }

        protected void CreateDic4Mid(PublicationInformation projInfo)
        {
            throw new NotImplementedException();
        }

        protected void ReformatData(PublicationInformation projInfo)
        {
            var xml = new XmlDocument {XmlResolver = null};
            var streamReader = new StreamReader(projInfo.DefaultXhtmlFileWithPath);
            xml.Load(streamReader);
            streamReader.Close();
            var nsmgr = GetNamespaceManager(xml);
            foreach (XmlNode sense in xml.SelectNodes("//*['entry']/xhtml:div", nsmgr))
            {

            }

            Debug.Assert(xml.DocumentElement != null);
            xml.DocumentElement.RemoveAll();
        }

        private static XmlNamespaceManager GetNamespaceManager(XmlDocument xmlDocument)
        {
            var root = xmlDocument.DocumentElement;
            Debug.Assert(root != null, "Missing xml document");
            var nsManager = new XmlNamespaceManager(xmlDocument.NameTable);
            foreach (XmlAttribute attribute in root.Attributes)
            {
                var namePart = attribute.Name.Split(':');
                if (namePart[0] == "xmlns")
                    nsManager.AddNamespace(namePart[1], attribute.Value);
            }
            nsManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            return nsManager;
        }
    }
}
