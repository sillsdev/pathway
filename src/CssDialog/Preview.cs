// --------------------------------------------------------------------------------------------
// <copyright file="Preview.cs" from='2009' to='2014' company='SIL International'>
//      Copyright (C) 2014, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Preview for the selected Css
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using SIL.Tool;
using SIL.Tool.Localization;

namespace SIL.PublishingSolution
{
    public class Preview
    {
        public string Sheet { get; set; }
        public Form ParentForm { get; set; }
        /// <summary>
        /// To show the preview
        /// </summary>
        public void Show()
        {
            if (Param.Value.ContainsKey(Param.PreviewType))
                    if (Param.Value[Param.PreviewType] == "Prince")
                        PdfPreview();
                    else
                    {
                        Debug.Fail("Preview type not implemented");
                    }
        }

        private static bool inProcess;
        /// <summary>
        /// To show the PDF Preview
        /// </summary>
        public void PdfPreview()
        {
            string outName = CreatePreview();
            if (outName != string.Empty)
            {
                SubProcess.Run(Param.Value[Param.OutputPath], outName);
            }
        }

        public string CreatePreview()
        {
            Debug.Assert(ParentForm != null);
            if (inProcess) 
            {
                return string.Empty;
            }
            inProcess = true;
            var cr = ParentForm.Cursor;
            ParentForm.Cursor = Cursors.WaitCursor;
            Debug.Assert(!string.IsNullOrEmpty(Param.Value[Param.CurrentInput]), "The given key was not present in the dictionary.");
            var xhtml = Param.Value[Param.CurrentInput];

            if (string.IsNullOrEmpty(xhtml) || !File.Exists(xhtml)) return string.Empty;
            string PreviewCSSPath = Param.StylePath(Sheet);
            var mergedCss = new MergeCss();
            string cssCombine = mergedCss.Make(PreviewCSSPath, "Temp1.css");

            var returnXhtml = CreatePreviewFile(xhtml, cssCombine, "preview", true);
            var pdf = new Pdf(returnXhtml, cssCombine);
            var outName = Common.PathCombine(Param.Value[Param.OutputPath], Path.GetFileNameWithoutExtension(xhtml) + ".pdf");
            try
            {
                pdf.Create(outName);
            }
            catch (Pdf.MISSINGPRINCE)
            {
                LocDB.Message("errInstallPrinceXML", "PrinceXML must be downloaded and installed from www.PrinceXML.com", null, LocDB.MessageTypes.Error,
                              LocDB.MessageDefault.First);
                ParentForm.Cursor = cr;
                inProcess = false;
                return string.Empty;
            }
            ParentForm.Cursor = cr;
            inProcess = false;
            return outName;
        }

        #region CreatePreviewFile(string xhtmlFile, string cssFile, string outputFileName)
        /// <summary>
        /// Return the name of the preview html name in the link tag.
        /// </summary>
        /// <param name="xhtmlFile">XHTML File</param>
        /// <param name="cssFile">CSS File</param>
        /// <param name="outputFileName">Output File</param>
        /// <returns>Preview FilePath</returns>
        public static string CreatePreviewFile(string xhtmlFile, string cssFile, string outputFileName, bool excerptPreview)
        {
            PublicationInformation projInfo =  new PublicationInformation();
            projInfo.DefaultXhtmlFileWithPath = xhtmlFile;
            projInfo.DefaultCssFileWithPath = cssFile;
            if (!File.Exists(xhtmlFile)) return string.Empty;
            const int stopAtLineNo = 100;
            PreExportProcess preProcessor = new PreExportProcess(projInfo);
            preProcessor.GetTempFolderPath();
            preProcessor.ImagePreprocess(false);
            xhtmlFile = preProcessor.ProcessedXhtml;
            string linkCss = outputFileName + ".css";
            string previewPath = Path.GetDirectoryName(xhtmlFile);
            if (File.Exists(cssFile))
                File.Copy(cssFile, Common.PathCombine(previewPath, linkCss),true);
            string xhtmlPreviewFilePath = Common.PathCombine(previewPath, outputFileName + ".html");
            if (!excerptPreview)
            {
                Common.SetDefaultCSS(xhtmlPreviewFilePath, linkCss);
                return xhtmlFile;
            }

            XmlTextReader reader;
            XmlTextWriter writer = new XmlTextWriter(xhtmlPreviewFilePath, null);

            try
            {
                reader = Common.DeclareXmlTextReader(xhtmlFile, true);
                writer.WriteStartDocument();
                int lineCount = 0;
                while (reader.Read())
                {
                    if (reader.IsEmptyElement)
                    {
                        writer.WriteStartElement(reader.Name);
                        string readName = reader.Name;
                        if (reader.HasAttributes)
                        {
                            for (int i = 0; i < reader.AttributeCount; i++)
                            {
                                reader.MoveToAttribute(i);
                                if (string.Compare(readName, "link", true) == 0 && reader.Name == "href")
                                {
                                    writer.WriteAttributeString(reader.Name, linkCss);
                                }
                                else
                                {
                                    writer.WriteAttributeString(reader.Name, reader.Value);
                                }
                            }
                        }
                        writer.WriteEndElement();
                        readName = "";
                        continue;
                    }
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            writer.WriteStartElement(reader.Name);

                            if (reader.HasAttributes)
                            {
                                for (int i = 0; i < reader.AttributeCount; i++)
                                {
                                    reader.MoveToAttribute(i);
                                    writer.WriteAttributeString(reader.Name, reader.Value);
                                }
                            }
                            break;
                        case XmlNodeType.EndElement:
                            writer.WriteEndElement();
                            break;
                        case XmlNodeType.Text:
                            writer.WriteString(reader.Value);
                            lineCount++;
                            break;
                    }
                    if (lineCount > stopAtLineNo)
                        break;
                }
                writer.Flush();
                writer.Close();
                reader.Close();
            }
            catch (XmlException e)
            {
				Console.WriteLine(e.Message);
            }
            return xhtmlPreviewFilePath;
        }

        #endregion

    }
}
