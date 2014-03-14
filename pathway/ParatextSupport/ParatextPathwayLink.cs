// --------------------------------------------------------------------------------------------
// <copyright file="ParatextPathwayLink.cs" from='2009' to='2014' company='SIL International'>
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
// 
// </remarks>
// --------------------------------------------------------------------------------------------

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
    public class ParatextPathwayLink
    {
        private Dictionary<string, object> m_xslParams;
        private string m_projectName;
        private string m_databaseName;
        private string m_outputLocationPath;
        private string m_format;
        private XslCompiledTransform m_cleanUsx = new XslCompiledTransform();
        private XslCompiledTransform m_separateIntoBooks = new XslCompiledTransform();
        private XslCompiledTransform m_usxToXhtml = new XslCompiledTransform();
        private XslCompiledTransform m_encloseParasInSections = new XslCompiledTransform();

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="ParatextPathwayLink"/> class.
        /// </summary>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="xslParams">The parameters from Paratext for the XSLT.</param>
        /// ------------------------------------------------------------------------------------
        public ParatextPathwayLink(string databaseName, Dictionary<string, object> xslParams)
        {
            m_databaseName = databaseName;
            m_xslParams = xslParams;

            // If the writing system is undefined or set (by default) to English, add a writing system code 
            // that should not have a dictionary to prevent all words from being marked as misspelled.
            object strWs;
            if (m_xslParams.TryGetValue("ws", out strWs))
            {
                if ((string)strWs == "en")
                    m_xslParams["ws"] = "zxx";
            }
            else
            {
                Debug.Fail("Missing writing system parameter for XSLT");
                m_xslParams.Add("ws", "zxx");
            }
            object projObj;
            if (m_xslParams.TryGetValue("projName", out projObj))
                m_projectName = (string)projObj;
            if (!m_xslParams.ContainsKey("langInfo"))
            {
                m_xslParams.Add("langInfo", Common.ParaTextDcLanguage(databaseName));
            }
            LoadStyleSheets();
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="ParatextPathwayLink"/> class.
        /// This method overload is included only for backward compatibility with earlier versions 
        /// of Paratext.
        /// </summary>
        /// <param name="projName">Name of the project (from scrText.Name)</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="ws">The writing system locale.</param>
        /// <param name="userWs">The user writing system locale.</param>
        /// <param name="userName">Name of the user.</param>
        /// ------------------------------------------------------------------------------------
        public ParatextPathwayLink(string projName, string databaseName, string ws, string userWs, string userName)
        {
            if (ws == "en")
                ws = "zxx";

            m_projectName = projName;
            m_databaseName = databaseName;
            Common.databaseName = databaseName;
            // Set parameters for the XSLT.
            m_xslParams = new Dictionary<string, object>();
            m_xslParams.Add("ws", ws);
            m_xslParams.Add("userWs", userWs);
            DateTime now = DateTime.Now;
            m_xslParams.Add("dateTime", now.Date);
            m_xslParams.Add("user", userName);
            m_xslParams.Add("projName", projName);

            LoadStyleSheets();
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Loads the style sheets that are used to transform from Paratext USX to XHTML.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private void LoadStyleSheets()
        {
            // Create stylesheets
            m_cleanUsx.Load(XmlReader.Create(
                Assembly.GetExecutingAssembly().GetManifestResourceStream(
                "ParatextSupport.XML_without_line_breaks.xsl")));
            m_separateIntoBooks.Load(XmlReader.Create(
                Assembly.GetExecutingAssembly().GetManifestResourceStream(
                "ParatextSupport.SeparateIntoBooks.xsl")));
            m_usxToXhtml.Load(XmlReader.Create(
                Assembly.GetExecutingAssembly().GetManifestResourceStream(
                "ParatextSupport.UsfxToXhtml.xsl")));
            m_encloseParasInSections.Load(XmlReader.Create(
                Assembly.GetExecutingAssembly().GetManifestResourceStream(
                "ParatextSupport.EncloseParasInSections.xsl")));
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Exports to the current Scripture book to pathway.
        /// </summary>
        /// <param name="usxDoc">The XML document representation of the USFM file.</param>
        /// ------------------------------------------------------------------------------------
        public void ExportToPathway(XmlDocument usxDoc)
        {
            //// TestBed Code
            if (string.IsNullOrEmpty(usxDoc.InnerText))
            {
                // TODO: Localize string
                MessageBox.Show("The current book has no content to export.", string.Empty, MessageBoxButtons.OK);
                return;
            }
            ScriptureContents dlg = new ScriptureContents();
            dlg.DatabaseName = m_databaseName;
            DialogResult result = dlg.ShowDialog();
            if (result != DialogResult.Cancel)
            {
                string pubName = dlg.PublicationName;

                m_format = dlg.Format;

                // Get the file name as set on the dialog.
                m_outputLocationPath = dlg.OutputLocationPath;

                string cssFullPath = Common.PathCombine(m_outputLocationPath, pubName + ".css");
                StyToCSS styToCss = new StyToCSS();
                styToCss.ConvertStyToCSS(m_projectName, cssFullPath);
                string fileName = Common.PathCombine(m_outputLocationPath, pubName + ".xhtml");

                if (File.Exists(fileName))
                {
                    // TODO: Localize string
                    result = MessageBox.Show(string.Format("{0}" + Environment.NewLine +
                        " already exists. Overwrite?", fileName), string.Empty,
                        MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                        fileName = Common.PathCombine(m_outputLocationPath, pubName + "-" + DateTime.Now.Second + ".xhtml");
                    else if (result == DialogResult.No)
                        return;
                }

                try
                {
                    Application.UseWaitCursor = true;
                    ConvertUsxToPathwayXhtmlFile(usxDoc.InnerXml, fileName);
                }
                finally
                {
                    Application.UseWaitCursor = false;
                }

                PsExport exporter = new PsExport();
                exporter.DataType = "Scripture";
                exporter.Export(fileName);
            }
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Exports to the specified Scripture books to pathway.
        /// </summary>
        /// <param name="usxBooksToExport">The XML document representation of the Scripture 
        /// books in USFM file.</param>
        /// ------------------------------------------------------------------------------------
        public void ExportToPathway(List<XmlDocument> usxBooksToExport)
        {
            bool success;
            ScriptureContents dlg = new ScriptureContents();
            dlg.DatabaseName = m_databaseName;
            DialogResult result = dlg.ShowDialog();
            if (result != DialogResult.Cancel)
            {
#if (TIME_IT)
                DateTime dt1 = DateTime.Now;    // time this thing
#endif

                var inProcess = new InProcess(0, 6);
                var curdir = Environment.CurrentDirectory;
                var myCursor = Cursor.Current;
                Cursor.Current = Cursors.WaitCursor;
                inProcess.Text = "Scripture Export";
                inProcess.Show();
                inProcess.PerformStep();
                inProcess.ShowStatus = true;
                inProcess.SetStatus("Processing Scripture Export");

                string pubName = dlg.PublicationName;

                m_format = dlg.Format;

                // Get the file name as set on the dialog.
                m_outputLocationPath = dlg.OutputLocationPath;
                inProcess.PerformStep();
                if (m_format.StartsWith("theWord"))
                {
                    ExportUsx(usxBooksToExport);
                }

                inProcess.PerformStep();

                string cssFullPath = Common.PathCombine(m_outputLocationPath, pubName + ".css");
                StyToCSS styToCss = new StyToCSS();
                styToCss.ConvertStyToCSS(m_projectName, cssFullPath);
                string fileName = Common.PathCombine(m_outputLocationPath, pubName + ".xhtml");
                inProcess.PerformStep();

                if (File.Exists(fileName))
                {
                    // TODO: Localize string
                    result = MessageBox.Show(string.Format("{0}" + Environment.NewLine +
                        " already exists. Overwrite?", fileName), string.Empty,
                        MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        fileName = Common.PathCombine(m_outputLocationPath, pubName + "-" + DateTime.Now.Second + ".xhtml");
                    }
                    else if (result == DialogResult.No)
                    {
                        return;
                    }
                }
                inProcess.PerformStep();
                XmlDocument scrBooksDoc = CombineUsxDocs(usxBooksToExport);
                inProcess.PerformStep();
                if (string.IsNullOrEmpty(scrBooksDoc.InnerText))
                {
                    // TODO: Localize string
                    MessageBox.Show("The current book has no content to export.", string.Empty, MessageBoxButtons.OK);
                    return;
                }
                ConvertUsxToPathwayXhtmlFile(scrBooksDoc.InnerXml, fileName);
                success = true;
                Cursor.Current = myCursor;
                inProcess.PerformStep();
                inProcess.Close();

                PsExport exporter = new PsExport();
                exporter.DataType = "Scripture";
                exporter.Export(fileName);
                
            }
        }

        /// <summary>
        /// Write the USX files to the output folder in a sub folder called USX
        /// </summary>
        /// <param name="usxBooksToExport"></param>
        private void ExportUsx(List<XmlDocument> usxBooksToExport)
        {
            var usxDir = Common.PathCombine(m_outputLocationPath, "USX");
            Directory.CreateDirectory(usxDir);
            foreach (XmlDocument xmlDocument in usxBooksToExport)
            {
                var codeNode = xmlDocument.SelectSingleNode("//@code");
                Debug.Assert(codeNode != null);
                var name = codeNode.InnerText + ".usx";
                var fullName = Common.PathCombine(usxDir, name);
                var fw = new StreamWriter(fullName);
                xmlDocument.Save(fw);
                fw.Close();
            }
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Combines USX of multiple books into a single XmlDocument.
        /// </summary>
        /// <param name="usxBooksToExport">The Scripture books in USX format to export.</param>
        /// <returns>a single XmlDocument containing all books.</returns>
        /// ------------------------------------------------------------------------------------
        public XmlDocument CombineUsxDocs(List<XmlDocument> usxBooksToExport)
        {
            Debug.Assert(usxBooksToExport != null && usxBooksToExport.Count > 0);

            if (m_format == "CadreBible" || m_format == "Go Bible" || m_format == "Sword")
                ExportUSXRawToUSX(usxBooksToExport);

            XmlDocument allBooks = usxBooksToExport[0];
            if (usxBooksToExport.Count == 1)
                return allBooks;

            for (int iDoc = 1; iDoc < usxBooksToExport.Count; iDoc++)
            {
                foreach (XmlNode nodeToAdd in usxBooksToExport[iDoc].SelectSingleNode("/usfm|/usx").ChildNodes)
                {
                    XmlNode prevNode = allBooks.SelectSingleNode("usfm|usx").LastChild;
                    XmlNode commonParent = prevNode.ParentNode;
                    commonParent.InsertAfter(allBooks.ImportNode(nodeToAdd, true), prevNode);
                }
            }

            return allBooks;
        }

        public void ExportUSXRawToUSX(List<XmlDocument> usxBooksToExport)
        {
            if (m_format != "Go Bible")
            {
                string vrsFileDest = Common.PathCombine(m_outputLocationPath, "versification.vrs");
                string ldsFileDest = Common.PathCombine(m_outputLocationPath, "English.lds");

                string paratextProjectLocation = string.Empty;
                object paraTextprojectPath;
                if (RegistryHelperLite.RegEntryExists(RegistryHelperLite.ParatextKey,
                                                      "Settings_Directory", "", out paraTextprojectPath))
                {
                    paratextProjectLocation = (string)paraTextprojectPath;
                    paratextProjectLocation = Common.PathCombine(paratextProjectLocation, m_projectName);
                    paratextProjectLocation = Common.PathCombine(paratextProjectLocation, "gather");

                    string vrsFileName = "eng";
                    string ldsFileName = "*"; //project file name associate file.

                    if (!Directory.Exists(paratextProjectLocation))
                    {
                        paratextProjectLocation = (string)paraTextprojectPath;
                    }
                    string ssfFileName = Common.PathCombine(paratextProjectLocation, Common.databaseName + ".ssf");
                    XmlDocument xmlDoc = Common.DeclareXMLDocument(false);
                    xmlDoc.Load(ssfFileName);
                    string xPath = "//Language";
                    XmlNode list = xmlDoc.SelectSingleNode(xPath);
                    if (list != null)
                    {
                        ldsFileName = list.InnerText;
                    }

                    if (File.Exists(Common.PathCombine(paratextProjectLocation, vrsFileName + ".vrs")))
                        File.Copy(Common.PathCombine(paratextProjectLocation, vrsFileName + ".vrs"), vrsFileDest);

                    if (File.Exists(Common.PathCombine(paratextProjectLocation, ldsFileName + ".lds")))
                        File.Copy(Common.PathCombine(paratextProjectLocation, ldsFileName + ".lds"), ldsFileDest);

                }

                m_outputLocationPath = Common.PathCombine(m_outputLocationPath, "USX");
            }
            else
            {
                m_outputLocationPath = Common.PathCombine(m_outputLocationPath, "SFM");
            }


            if (!Directory.Exists(m_outputLocationPath))
                Directory.CreateDirectory(m_outputLocationPath);

            for (int iDoc = 0; iDoc < usxBooksToExport.Count; iDoc++)
            {
                XmlDocument scrBooksDoc = usxBooksToExport[iDoc];
                string usx = scrBooksDoc.InnerXml;

                var nsmgr1 = new XmlNamespaceManager(scrBooksDoc.NameTable);
                nsmgr1.AddNamespace("style", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
                nsmgr1.AddNamespace("fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");
                nsmgr1.AddNamespace("text", "urn:oasis:names:tc:opendocument:xmlns:text:1.0");

                string xpath = "//book";
                string bookName = string.Empty;
                XmlNodeList list = scrBooksDoc.SelectNodes(xpath, nsmgr1);
                if (list != null)
                {
                    foreach (XmlNode xmlNode in list)
                    {
                        if (xmlNode.Attributes != null)
                        {
                            try
                            {
                                bookName = xmlNode.Attributes["code"].Value;
                            }
                            catch (NullReferenceException)
                            {
                                bookName = xmlNode.Attributes["id"].Value;
                            }
                        }
                    }
                }

                // Create argument list
                XsltArgumentList args = new XsltArgumentList();
                foreach (string paramName in m_xslParams.Keys)
                    args.AddParam(paramName, "", m_xslParams[paramName]);

                // Step 1. Separate books into their own elements for subsequent processing.
                StringBuilder separatedBooks = new StringBuilder();
                XmlWriter htmlw1 = XmlWriter.Create(separatedBooks, m_usxToXhtml.OutputSettings);
                m_separateIntoBooks.Transform(XmlReader.Create(new StringReader(usx)), null, htmlw1, null);

                // Step 2. Remove line breaks for next step (to prevent creation of empty spans).
                StringBuilder cleanUsx = new StringBuilder();
                XmlWriter htmlw2 = XmlWriter.Create(cleanUsx, m_cleanUsx.OutputSettings);
                m_cleanUsx.Transform(XmlReader.Create(new StringReader(separatedBooks.ToString())), null, htmlw2, null);

                // Step 3. Convert the SFMs to styles recognized by Pathway. Also, change the structure of the 
                //       following elements to Pathway's format: book title, chapters, figures, footnotes.
                StringBuilder html = new StringBuilder();
                XmlWriter htmlw3 = XmlWriter.Create(html, m_usxToXhtml.OutputSettings);
                m_usxToXhtml.Transform(XmlReader.Create(new StringReader(cleanUsx.ToString())), args, htmlw3, null);

                cleanUsx = cleanUsx.Replace("utf-16", "utf-8");
                cleanUsx = cleanUsx.Replace("<usfm>", "<USX>");
                cleanUsx = cleanUsx.Replace("</usfm>", "</USX>");

                string bookFileName = Common.PathCombine(m_outputLocationPath, bookName + ".usx");
                XmlDocument doc = new XmlDocument();
                XmlTextWriter txtWriter = new XmlTextWriter(bookFileName, null);
                txtWriter.Formatting = Formatting.Indented;
                txtWriter.WriteRaw(cleanUsx.ToString());
                doc.Save(txtWriter);
                txtWriter.Close();

                if (m_format == "Go Bible")
                {
                    UsxToSFM usxToSfm = new UsxToSFM();
                    string targetFile = bookFileName.Replace(".usx", ".sfm");
                    usxToSfm.ConvertUsxToSFM(bookFileName, targetFile);

                    if (File.Exists(targetFile))
                    {
                        File.Delete(bookFileName);
                    }
                }
            }
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Converts the USX to an XHTML file for Pathway.
        /// </summary>
        /// <param name="usx">The XML document representation of the USFM file.</param>
        /// <param name="fileName">file name with full path where xhtml file will be written</param>
        /// ------------------------------------------------------------------------------------
        public void ConvertUsxToPathwayXhtmlFile(string usx, string fileName)
        {
            // Create argument list
            XsltArgumentList args = new XsltArgumentList();
            foreach (string paramName in m_xslParams.Keys)
                args.AddParam(paramName, "", m_xslParams[paramName]);

            // Step 1. Separate books into their own elements for subsequent processing.
            StringBuilder separatedBooks = new StringBuilder();
            XmlWriter htmlw1 = XmlWriter.Create(separatedBooks, m_usxToXhtml.OutputSettings);
            m_separateIntoBooks.Transform(XmlReader.Create(new StringReader(usx)), null, htmlw1, null);

            // Step 2. Remove line breaks for next step (to prevent creation of empty spans).
            StringBuilder cleanUsx = new StringBuilder();
            XmlWriter htmlw2 = XmlWriter.Create(cleanUsx, m_cleanUsx.OutputSettings);
            m_cleanUsx.Transform(XmlReader.Create(new StringReader(separatedBooks.ToString())), null, htmlw2, null);

            // Step 3. Convert the SFMs to styles recognized by Pathway. Also, change the structure of the 
            //       following elements to Pathway's format: book title, chapters, figures, footnotes.
            StringBuilder html = new StringBuilder();
            XmlWriter htmlw3 = XmlWriter.Create(html, m_usxToXhtml.OutputSettings);
            m_usxToXhtml.Transform(XmlReader.Create(new StringReader(cleanUsx.ToString())), args, htmlw3, null);

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ProhibitDtd = false;

            // Step 4. Move paragraphs into appropriate section type (as determined by the paragraph styles) and 
            //       include the Scripture sections within columns.
            FileStream xhtmlFile = new FileStream(fileName, FileMode.Create);
            XmlWriter htmlw4 = XmlWriter.Create(xhtmlFile, m_encloseParasInSections.OutputSettings);
            XmlReader reader4 = XmlReader.Create(new StringReader(html.ToString()), settings);
            m_encloseParasInSections.Transform(reader4, null, htmlw4, null);
            xhtmlFile.Close();
        }
    }
}
