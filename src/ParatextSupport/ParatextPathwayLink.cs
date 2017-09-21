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
using L10NSharp;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class ParatextPathwayLink
    {
        #region private variables

        private readonly Dictionary<string, object> _mXslParams;
        private readonly string _mProjectName;
        private string _mDatabaseName;
        private string _mOutputLocationPath;
        private string _mFormat;
        private string _mPublicationName;
        private readonly XslCompiledTransform _mCleanUsx = new XslCompiledTransform();
        private readonly XslCompiledTransform _mSeparateIntoBooks = new XslCompiledTransform();
        private readonly XslCompiledTransform _mUsxToXhtml = new XslCompiledTransform();
        private readonly XslCompiledTransform _mEncloseParasInSections = new XslCompiledTransform();
		private readonly XslCompiledTransform _mMoveLangUp = new XslCompiledTransform();

		#endregion

		#region public variables

		public string MOutputLocationPath
        {
            get { return _mOutputLocationPath; }
            set { _mOutputLocationPath = value; }
        }

        public string MFormat
        {
            get { return _mFormat; }
            set { _mFormat = value; }
        }

        public string MPublicationName
        {
            get { return _mPublicationName; }
            set { _mPublicationName = value; }
        }

        public string MDatabaseName
        {
            get { return _mDatabaseName; }
            set { _mDatabaseName = value; }
        }

        #endregion

        #region public Methods

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="ParatextPathwayLink"/> class.
        /// This method is used by PathwayB. It will be called by Reflection.
        /// </summary>
        /// <param name="projName">Name of the project (from scrText.Name)</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="ws">The writing system locale.</param>
        /// <param name="userWs">The user writing system locale.</param>
        /// <param name="userName">Name of the user.</param>
        /// ------------------------------------------------------------------------------------
        // ReSharper disable UnusedMember.Global
        public ParatextPathwayLink(string projName, string databaseName, string ws, string userWs, string userName)
        // ReSharper restore UnusedMember.Global
        {
            if (ws == "en")
                ws = "zxx";

            _mProjectName = projName;
            MDatabaseName = databaseName;
            Common.databaseName = databaseName;
            // Set parameters for the XSLT.
            _mXslParams = new Dictionary<string, object>();
            _mXslParams.Add("ws", ws);
            _mXslParams.Add("userWs", userWs);
            DateTime now = DateTime.Now;
            _mXslParams.Add("dateTime", now.Date);
            _mXslParams.Add("user", userName);
            _mXslParams.Add("projName", projName);

            LoadStyleSheets();
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="ParatextPathwayLink"/> class.
        /// Used by Paratext. Called by Reflection.
        /// </summary>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="xslParams">The parameters from Paratext for the XSLT.</param>
        /// ------------------------------------------------------------------------------------
        public ParatextPathwayLink(string databaseName, Dictionary<string, object> xslParams)
        {
            MDatabaseName = databaseName;
	        Param.DatabaseName = databaseName;
            _mXslParams = xslParams;
            _mProjectName = xslParams["projName"].ToString();
            // If the writing system is undefined or set (by default) to English, add a writing system code
            // that should not have a dictionary to prevent all words from being marked as misspelled.
            object strWs;
            if (_mXslParams.TryGetValue("ws", out strWs))
            {
                if ((string)strWs == "en")
                    _mXslParams["ws"] = "zxx";
            }
            else
            {
                Debug.Fail("Missing writing system parameter for XSLT");
                _mXslParams.Add("ws", "zxx");
            }
            if (!_mXslParams.ContainsKey("langInfo"))
            {
                _mXslParams.Add("langInfo", Common.ParaTextDcLanguage(databaseName,false));
            }
            LoadStyleSheets();
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Exports to the specified Scripture books to pathway.
        /// (Called from Paratext by Reflection.)
        /// </summary>
        /// <param name="usxBooksToExport">The XML document representation of the Scripture
        /// books in USFM file.</param>
        /// ------------------------------------------------------------------------------------
        // ReSharper disable UnusedMember.Global
        public void ExportToPathway(List<XmlDocument> usxBooksToExport)
        // ReSharper restore UnusedMember.Global
        {
            DialogResult result;
            if (!Common.Testing)
            {
                ScriptureContents dlg = new ScriptureContents();
                dlg.DatabaseName = MDatabaseName;
                result = dlg.ShowDialog();
                if (result != DialogResult.Cancel)
                {
                    ExportProcess(usxBooksToExport, dlg.PublicationName, dlg.Format, dlg.OutputLocationPath, result);
                }
            }
            else
            {
                result = new DialogResult();
                ExportProcess(usxBooksToExport, MPublicationName, MFormat, MOutputLocationPath, result);
            }
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Combines USX of multiple books into a single XmlDocument. This method is called
        /// from PathwayB by Reflection.
        /// </summary>
        /// <param name="usxBooksToExport">The Scripture books in USX format to export.</param>
        /// <param name="format">Needs to be the name reported by the back end Handler.</param>
        /// <returns>a single XmlDocument containing all books.</returns>
        /// ------------------------------------------------------------------------------------
        // ReSharper disable UnusedMember.Global
        public XmlDocument CombineUsxDocs(List<XmlDocument> usxBooksToExport, string format)
        // ReSharper restore UnusedMember.Global
        {
            Debug.Assert(usxBooksToExport != null && usxBooksToExport.Count > 0);
            MFormat = format;

            if (MFormat == "CadreBible" || MFormat == "Go Bible" || MFormat == "Sword")
                ExportUsxRawToUsx(usxBooksToExport);

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

        public void ExportUsxRawToUsx(List<XmlDocument> usxBooksToExport)
        {
            if (MFormat != "Go Bible")
            {
                string vrsFileDest = Common.PathCombine(MOutputLocationPath, "versification.vrs");
                string ldsFileDest = Common.PathCombine(MOutputLocationPath, "English.lds");

                string paratextProjectLocation = string.Empty;
                object paraTextprojectPath;
                if (RegistryHelperLite.RegEntryExists(RegistryHelperLite.ParatextKey,
                                                      "Settings_Directory", "", out paraTextprojectPath))
                {
                    paratextProjectLocation = (string)paraTextprojectPath;
                    paratextProjectLocation = Common.PathCombine(paratextProjectLocation, _mProjectName);
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

                MOutputLocationPath = Common.PathCombine(MOutputLocationPath, "USX");
            }
            else
            {
                MOutputLocationPath = Common.PathCombine(MOutputLocationPath, "SFM");
            }


            if (!Directory.Exists(MOutputLocationPath))
                Directory.CreateDirectory(MOutputLocationPath);

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

                GetBookName(list, ref bookName);

                // Create argument list
                XsltArgumentList args = new XsltArgumentList();
                foreach (string paramName in _mXslParams.Keys)
                    args.AddParam(paramName, "", _mXslParams[paramName]);

                // Step 1. Separate books into their own elements for subsequent processing.
                StringBuilder separatedBooks = new StringBuilder();
                XmlWriter htmlw1 = XmlWriter.Create(separatedBooks, _mUsxToXhtml.OutputSettings);
                _mSeparateIntoBooks.Transform(XmlReader.Create(new StringReader(usx)), null, htmlw1, null);

                // Step 2. Remove line breaks for next step (to prevent creation of empty spans).
                StringBuilder cleanUsx = new StringBuilder();
                XmlWriter htmlw2 = XmlWriter.Create(cleanUsx, _mCleanUsx.OutputSettings);
                _mCleanUsx.Transform(XmlReader.Create(new StringReader(separatedBooks.ToString())), null, htmlw2, null);

                // Step 3. Convert the SFMs to styles recognized by Pathway. Also, change the structure of the
                //       following elements to Pathway's format: book title, chapters, figures, footnotes.
                StringBuilder html = new StringBuilder();
                XmlWriter htmlw3 = XmlWriter.Create(html, _mUsxToXhtml.OutputSettings);
                _mUsxToXhtml.Transform(XmlReader.Create(new StringReader(cleanUsx.ToString())), args, htmlw3, null);

                cleanUsx = cleanUsx.Replace("utf-16", "utf-8");
                cleanUsx = cleanUsx.Replace("<usfm>", "<USX>");
                cleanUsx = cleanUsx.Replace("</usfm>", "</USX>");

                string bookFileName = Common.PathCombine(MOutputLocationPath, bookName + ".usx");
                XmlDocument doc = new XmlDocument();
                XmlTextWriter txtWriter = new XmlTextWriter(bookFileName, null);
                txtWriter.Formatting = Formatting.Indented;
                txtWriter.WriteRaw(cleanUsx.ToString());
                doc.Save(txtWriter);
                txtWriter.Close();

                if (MFormat == "Go Bible")
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
        /// Converts the USX to an XHTML file for Pathway. Called by PathwayB with Reflection.
        /// </summary>
        /// <param name="usx">The XML document representation of the USFM file.</param>
        /// <param name="fileName">file name with full path where xhtml file will be written</param>
        /// ------------------------------------------------------------------------------------
        // ReSharper disable UnusedMember.Global
        public void ConvertUsxToPathwayXhtmlFile(string usx, string fileName)
        // ReSharper restore UnusedMember.Global
        {
            // Create argument list
            XsltArgumentList args = new XsltArgumentList();
            foreach (string paramName in _mXslParams.Keys)
                args.AddParam(paramName, "", _mXslParams[paramName]);

            // Step 1. Separate books into their own elements for subsequent processing.
            StringBuilder separatedBooks = new StringBuilder();
            XmlWriter htmlw1 = XmlWriter.Create(separatedBooks, _mUsxToXhtml.OutputSettings);
            _mSeparateIntoBooks.Transform(XmlReader.Create(new StringReader(usx.Replace(">﻿ </book>", "></book>"))), null, htmlw1, null);

            //Replace stylename "nb" to "p"
            //"nb" is invalid class in Usx file
            separatedBooks = separatedBooks.Replace("style=\"nb\"", "style=\"p\"");

            // Step 2. Remove line breaks for next step (to prevent creation of empty spans).
            StringBuilder cleanUsx = new StringBuilder();
            XmlWriter htmlw2 = XmlWriter.Create(cleanUsx, _mCleanUsx.OutputSettings);
            _mCleanUsx.Transform(XmlReader.Create(new StringReader(separatedBooks.ToString())), null, htmlw2, null);

            // Step 3. Convert the SFMs to styles recognized by Pathway. Also, change the structure of the
            //       following elements to Pathway's format: book title, chapters, figures, footnotes.
            StringBuilder html3 = new StringBuilder();
            XmlWriter htmlw3 = XmlWriter.Create(html3, _mUsxToXhtml.OutputSettings);
            _mUsxToXhtml.Transform(XmlReader.Create(new StringReader(cleanUsx.ToString())), args, htmlw3, null);

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;

			// Step 4. Move paragraphs into appropriate section type (as determined by the paragraph styles) and
			//       include the Scripture sections within columns.
			StringBuilder html4 = new StringBuilder();
			XmlWriter htmlw4 = XmlWriter.Create(html4, _mEncloseParasInSections.OutputSettings);
            XmlReader reader4 = XmlReader.Create(new StringReader(html3.ToString()), settings);
            _mEncloseParasInSections.Transform(reader4, null, htmlw4, null);

			// Step 5. Move language code to body. Remove spans with only language tag
			FileStream xhtmlFile = new FileStream(fileName, FileMode.Create);
			XmlWriter htmlw5 = XmlWriter.Create(xhtmlFile, _mMoveLangUp.OutputSettings);
			XmlReader reader5 = XmlReader.Create(new StringReader(html4.ToString()), settings);
			_mMoveLangUp.Transform(reader5, null, htmlw5, null);
			xhtmlFile.Close();
		}

		#endregion

		#region private methods

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the style sheets that are used to transform from Paratext USX to XHTML.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadStyleSheets()
        {
            // Create stylesheets
            _mCleanUsx.Load(XmlReader.Create(
                Assembly.GetExecutingAssembly().GetManifestResourceStream(
                "ParatextSupport.XML_without_line_breaks.xsl")));
            _mSeparateIntoBooks.Load(XmlReader.Create(
                Assembly.GetExecutingAssembly().GetManifestResourceStream(
                "ParatextSupport.SeparateIntoBooks.xsl")));
            _mUsxToXhtml.Load(XmlReader.Create(
                Assembly.GetExecutingAssembly().GetManifestResourceStream(
                "ParatextSupport.UsfxToXhtml.xsl")));
            _mEncloseParasInSections.Load(XmlReader.Create(
                Assembly.GetExecutingAssembly().GetManifestResourceStream(
                "ParatextSupport.EncloseParasInSections.xsl")));
			_mMoveLangUp.Load(XmlReader.Create(
				Assembly.GetExecutingAssembly().GetManifestResourceStream(
				"ParatextSupport.MoveLangUp.xsl")));
		}

		private void ExportProcess(List<XmlDocument> usxBooksToExport, string publicationName, string format, string outputLocationPath, DialogResult result)
        {
#if (TIME_IT)
                DateTime dt1 = DateTime.Now;    // time this thing
#endif
	        var inProcess = new InProcess(0, 6);
            var myCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            inProcess.Text = "Scripture Export";
            inProcess.Show();
            inProcess.PerformStep();
            inProcess.ShowStatus = true;
            inProcess.SetStatus("Processing Scripture Export");

            string pubName = publicationName;

            MFormat = format;

            // Get the file name as set on the dialog.
            MOutputLocationPath = outputLocationPath;
            inProcess.PerformStep();
            if (MFormat.StartsWith("theWord"))
            {
                ExportUsx(usxBooksToExport);
            }

            inProcess.PerformStep();

            string cssFullPath = Common.PathCombine(MOutputLocationPath, pubName + ".css");
            StyToCss styToCss = new StyToCss();
            styToCss.ConvertStyToCss(_mDatabaseName, cssFullPath);
            string fileName = Common.PathCombine(MOutputLocationPath, pubName + ".xhtml");
            inProcess.PerformStep();

            if (File.Exists(fileName))
            {
                var msg = LocalizationManager.GetString("ParatextPathwayLink.ExportProcess.Message1", " already exists. Overwrite?", "");
                result = MessageBox.Show(string.Format("{0}" + Environment.NewLine + msg, fileName), string.Empty, MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    fileName = Common.PathCombine(MOutputLocationPath, pubName + "-" + DateTime.Now.Second + ".xhtml");
                }
                else if (result == DialogResult.No)
                {
                    return;
                }
            }
            inProcess.PerformStep();
            XmlDocument scrBooksDoc = CombineUsxDocs(usxBooksToExport, MFormat);
            inProcess.PerformStep();
            if (string.IsNullOrEmpty(scrBooksDoc.InnerText))
            {
                var message = LocalizationManager.GetString("ParatextPathwayLink.ExportProcess.Message2", "The current book has no content to export.", "");
                MessageBox.Show(message, string.Empty, MessageBoxButtons.OK);
                return;
            }
            ConvertUsxToPathwayXhtmlFile(scrBooksDoc.InnerXml, fileName);
	        Cursor.Current = myCursor;
            inProcess.PerformStep();
            inProcess.Close();

            if (!Common.Testing)
            {
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
            var usxDir = Common.PathCombine(MOutputLocationPath, "USX");
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

        private static void GetBookName(XmlNodeList list, ref string bookName)
        {
            if (list != null)
            {
                foreach (XmlNode xmlNode in list)
                {
                    if (xmlNode.Attributes != null)
                    {
                        try
                        {
                            if (xmlNode.Attributes["code"] != null)
                            {
                                bookName = xmlNode.Attributes["code"].Value;
                            }
                            else
                            {
                                if (xmlNode.Attributes["id"] != null)
                                    bookName = xmlNode.Attributes["id"].Value;
                            }
                        }
                        catch (NullReferenceException)
                        {
                        }
                    }
                }
            }
        }

        #endregion
    }
}
