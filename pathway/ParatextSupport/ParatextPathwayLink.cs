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
		private XslCompiledTransform m_cleanUsx = new XslCompiledTransform();
		private XslCompiledTransform m_separateIntoBooks = new XslCompiledTransform();
		private XslCompiledTransform m_usxToXhtml = new XslCompiledTransform();
		private XslCompiledTransform m_encloseParasInSections = new XslCompiledTransform();
		//private XslCompiledTransform m_encloseScrInColumns = new XslCompiledTransform();

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
            //// Save Paratext usxDoc file.
            // usfxDoc.Save("d:\\usxDoc.xml");
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

				// Get the file name as set on the dialog.
				string outputLocationPath = dlg.OutputLocationPath;
                
                string cssFullPath = Path.Combine(outputLocationPath, pubName + ".css");
                StyToCSS styToCss = new StyToCSS();
                styToCss.ConvertStyToCSS(m_projectName, cssFullPath);
				string fileName = Path.Combine(outputLocationPath, pubName + ".xhtml");

				if (File.Exists(fileName))
				{
					// TODO: Localize string
					result = MessageBox.Show(string.Format("{0}" + Environment.NewLine +
						" already exists. Overwrite?", fileName), string.Empty,
					    MessageBoxButtons.YesNo);
					if (result == DialogResult.No)
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
			// TODO: ProgressBar progressBar = new ProgressBar();
			ScriptureContents dlg = new ScriptureContents();
			dlg.DatabaseName = m_databaseName;
			DialogResult result = dlg.ShowDialog();
			if (result != DialogResult.Cancel)
			{
				string pubName = dlg.PublicationName;

				// Get the file name as set on the dialog.
				string outputLocationPath = dlg.OutputLocationPath;

				string cssFullPath = Path.Combine(outputLocationPath, pubName + ".css");
				StyToCSS styToCss = new StyToCSS();
				styToCss.ConvertStyToCSS(m_projectName, cssFullPath);
				string fileName = Path.Combine(outputLocationPath, pubName + ".xhtml");

				if (File.Exists(fileName))
				{
					// TODO: Localize string
					result = MessageBox.Show(string.Format("{0}" + Environment.NewLine +
						" already exists. Overwrite?", fileName), string.Empty,
						MessageBoxButtons.YesNo);
					if (result == DialogResult.No)
						return;
				}

				XmlDocument scrBooksDoc = CombineUsxDocs(usxBooksToExport);

				if (string.IsNullOrEmpty(scrBooksDoc.InnerText))
				{
					// TODO: Localize string
					MessageBox.Show("The current book has no content to export.", string.Empty, MessageBoxButtons.OK);
					return;
				}
				ConvertUsxToPathwayXhtmlFile(scrBooksDoc.InnerXml, fileName);

				PsExport exporter = new PsExport();
				exporter.DataType = "Scripture";
				exporter.Export(fileName);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Combines USX of multiple books into a single XmlDocument.
		/// </summary>
		/// <param name="usxBooksToExport">The Scripture books in USX format to export.</param>
		/// <returns>a single XmlDocument containing all books.</returns>
		/// ------------------------------------------------------------------------------------
		private XmlDocument CombineUsxDocs(List<XmlDocument> usxBooksToExport)
		{
			Debug.Assert(usxBooksToExport != null && usxBooksToExport.Count > 0);

			XmlDocument allBooks = usxBooksToExport[0];
			if (usxBooksToExport.Count == 1)
				return allBooks;

			for (int iDoc = 1; iDoc < usxBooksToExport.Count; iDoc++)
			{
				foreach (XmlNode nodeToAdd in usxBooksToExport[iDoc].SelectSingleNode("/usfm").ChildNodes)
				{
					XmlNode prevNode = allBooks.SelectSingleNode("usfm").LastChild;
					XmlNode commonParent = prevNode.ParentNode;
					commonParent.InsertAfter(allBooks.ImportNode(nodeToAdd, true), prevNode);
				}
			}

			return allBooks;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Converts the USX to an XHTML file for Pathway.
		/// </summary>
		/// <param name="usx">The XML document representation of the USFM file.</param>
		/// <param name="fileName">file name with full path where xhtml file will be written</param>
		/// ------------------------------------------------------------------------------------
		private void ConvertUsxToPathwayXhtmlFile(string usx, string fileName)
		{
			// Create argument list
			XsltArgumentList args = new XsltArgumentList();
			foreach (string paramName in m_xslParams.Keys)
				args.AddParam(paramName, "", m_xslParams[paramName]);

			// Step 1. Separate books into their own elements for subsequent processing.
			StringBuilder separatedBooks = new StringBuilder();
			XmlWriter htmlw05 = XmlWriter.Create(separatedBooks, m_usxToXhtml.OutputSettings);
			m_separateIntoBooks.Transform(XmlReader.Create(new StringReader(usx)), null, htmlw05, null);

			// Step 2. Remove line breaks for next step (to prevent creation of empty spans).
			StringBuilder cleanUsx = new StringBuilder();
			XmlWriter htmlw0 = XmlWriter.Create(cleanUsx, m_cleanUsx.OutputSettings);
			m_cleanUsx.Transform(XmlReader.Create(new StringReader(separatedBooks.ToString())), null, htmlw0, null);

			// Step 3. Convert the SFMs to styles recognized by Pathway. Also, change the structure of the 
			//       following elements to Pathway's format: book title, chapters, figures, footnotes.
			StringBuilder html = new StringBuilder();
			XmlWriter htmlw = XmlWriter.Create(html, m_usxToXhtml.OutputSettings);
			m_usxToXhtml.Transform(XmlReader.Create(new StringReader(cleanUsx.ToString())), args, htmlw, null);

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
