using System;
using System.Collections.Generic;
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
		private string m_databaseName;
		private XslCompiledTransform m_cleanUsfx = new XslCompiledTransform();
		private XslCompiledTransform m_usfxToXhtml = new XslCompiledTransform();
		private XslCompiledTransform m_moveTitleSpansToTitle = new XslCompiledTransform();
		private XslCompiledTransform m_moveSpansToParas = new XslCompiledTransform();
		private XslCompiledTransform m_encloseParasInSections = new XslCompiledTransform();
		private XslCompiledTransform m_addImpliedSection = new XslCompiledTransform();
		private XslCompiledTransform m_encloseScrInColumns = new XslCompiledTransform();

		/// <summary>
		/// Initializes a new instance of the <see cref="ParatextPathwayLink"/> class.
		/// </summary>
		/// <param name="projName">Name of the project (from scrText.Name) </param>
		/// <param name="databaseName">Name of the database.</param>
		/// <param name="ws">The writing system locale.</param>
		/// <param name="userWs">The user writing system locale.</param>
		/// <param name="userName">Name of the user.</param>
		public ParatextPathwayLink(string projName, string databaseName, string ws, string userWs, string userName)
		{
            if (ws == "en")
            {
                ws = "zxx";
            }

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

			// Create stylesheets
			m_cleanUsfx.Load(XmlReader.Create(
				Assembly.GetExecutingAssembly().GetManifestResourceStream(
				"ParatextSupport.XML_without_line_breaks.xsl")));
			m_usfxToXhtml.Load(XmlReader.Create(
				Assembly.GetExecutingAssembly().GetManifestResourceStream(
				"ParatextSupport.UsfxToXhtml.xsl")));
			m_moveTitleSpansToTitle.Load(XmlReader.Create(
				Assembly.GetExecutingAssembly().GetManifestResourceStream(
				"ParatextSupport.MoveTitleSpansToTitle.xsl")));
			m_moveSpansToParas.Load(XmlReader.Create(
				Assembly.GetExecutingAssembly().GetManifestResourceStream(
				"ParatextSupport.MoveSpansToParas.xsl")));
			m_encloseParasInSections.Load(XmlReader.Create(
				Assembly.GetExecutingAssembly().GetManifestResourceStream(
				"ParatextSupport.EncloseParasInSections.xsl")));
			m_addImpliedSection.Load(XmlReader.Create(
				Assembly.GetExecutingAssembly().GetManifestResourceStream(
				"ParatextSupport.AddImpliedSection.xsl")));
			m_encloseScrInColumns.Load(XmlReader.Create(
				Assembly.GetExecutingAssembly().GetManifestResourceStream(
				"ParatextSupport.EncloseScrInColumns.xsl")));
		}

		/// <summary>
		/// Exports to the current Scripture book to pathway.
		/// </summary>
		public void ExportToPathway(XmlDocument usfxDoc)
		{
            //// TestBed Code
            //// Save Paratext usfxDoc file.
            // usfxDoc.Save("d:\\usfxDoc.xml");

			if (string.IsNullOrEmpty(usfxDoc.InnerText))
			{
				// TODO: Localize string
				MessageBox.Show("The specified book has no content to export.", string.Empty, MessageBoxButtons.OK);
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
				// ENHANCE/TODO: Add book number/id to end of file when exporting multiple books.
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

				ConvertUsfxToPathwayXhtml(usfxDoc.InnerXml, fileName);

				PsExport exporter = new PsExport();
				exporter.DataType = "Scripture";
				exporter.Export(fileName);
			}
		}

		/// <summary>
		/// Exports to the current Scripture book to pathway.
		/// </summary>
		public void ExportToPathway(List<XmlDocument> usfxDoc)
		{
			throw new NotImplementedException("Instead of a list, the usfxDoc could be passed in as a dictionary keyed " +
				"by the book number/id (which could be added to the XHTML file name)");
		}

		/// <summary>
		/// Converts the USFX to XHTML for Pathway.
		/// </summary>
		/// <param name="usfx">The XML representation of the standard format (USFX).</param>
		/// <param name="fileName">file name with full path where xhtml file will be written</param>
		private void ConvertUsfxToPathwayXhtml(string usfx, string fileName)
		{
			// Create argument list
			XsltArgumentList args = new XsltArgumentList();
			//args.AddExtensionObject("urn:extensions", new XsltExtensions());
			// REVIEW: I removed the following line for this new location.
			//args.AddExtensionObject("urn:usfmextensions", new UsfmXsltExtensions());
			foreach (string paramName in m_xslParams.Keys)
				args.AddParam(paramName, "", m_xslParams[paramName]);

			// Step 0. Remove line breaks.
			StringBuilder cleanUsfx = new StringBuilder();
			XmlWriter htmlw0 = XmlWriter.Create(cleanUsfx, m_cleanUsfx.OutputSettings);
			m_cleanUsfx.Transform(XmlReader.Create(new StringReader(usfx)), null, htmlw0, null);

			// Step 1. Convert the SFMs to styles recognized by Pathway. 
			// (Also, move chapter numbers to the following paragraph, if they are not included there, in preparation for the next steps).
			StringBuilder html = new StringBuilder();
			XmlWriter htmlw = XmlWriter.Create(html, m_usfxToXhtml.OutputSettings);
			m_usfxToXhtml.Transform(XmlReader.Create(new StringReader(cleanUsfx.ToString())), args, htmlw, null);

			XmlReaderSettings settings = new XmlReaderSettings();
			settings.ProhibitDtd = false;

			// Step 2. Move title spans inside a paragraph.
			StringBuilder titleSpansToTitle = new StringBuilder();
			XmlWriter htmlw2 = XmlWriter.Create(titleSpansToTitle, m_moveTitleSpansToTitle.OutputSettings);
			XmlReader reader2 = XmlReader.Create(new StringReader(html.ToString()), settings);
			m_moveTitleSpansToTitle.Transform(reader2, null, htmlw2, null);

			// Step 3. Move any text outside a paragraph element into a paragraph.
			StringBuilder spansToParas = new StringBuilder();
			XmlWriter htmlw3 = XmlWriter.Create(spansToParas, m_moveSpansToParas.OutputSettings);
			XmlReader reader3 = XmlReader.Create(new StringReader(titleSpansToTitle.ToString()), settings);
			m_moveSpansToParas.Transform(reader3, null, htmlw3, null);

			// Step 4. Move paragraphs into appropriate section type (as determined by the paragraph styles).
			StringBuilder parasInSections = new StringBuilder();
			XmlWriter htmlw4 = XmlWriter.Create(parasInSections, m_encloseParasInSections.OutputSettings);
			XmlReader reader4 = XmlReader.Create(new StringReader(spansToParas.ToString()), settings);
			m_encloseParasInSections.Transform(reader4, null, htmlw4, null);

			//// Step 4.5. Add implied section (when there isn't an explicit section head between the intro and Scripture content)
			//StringBuilder implicitSectionAdded = new StringBuilder();
			//XmlWriter htmlw4_5 = XmlWriter.Create(implicitSectionAdded, m_addImpliedSection.OutputSettings);
			//XmlReader reader4_5 = XmlReader.Create(new StringReader(parasInSections.ToString()), settings);
			//m_addImpliedSection.Transform(reader4_5, null, htmlw4_5, null);

			// Step 5. Move Scripture sections into columns element.
			FileStream xhtmlFile = new FileStream(fileName, FileMode.Create);
			XmlWriter htmlw5 = XmlWriter.Create(xhtmlFile, m_encloseScrInColumns.OutputSettings);
			XmlReader reader5 = XmlReader.Create(new StringReader(parasInSections.ToString()), settings);
			m_encloseScrInColumns.Transform(reader5, null, htmlw5, null);
			xhtmlFile.Close();
		}
	}
}
