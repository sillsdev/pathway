using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using SIL.Tool;
using epubConvert;

namespace SIL.PublishingSolution
{
    public class Epub3Transformation
    {
        private readonly Exportepub _parent;
        private readonly EpubFont _epubFont;
        public bool IsUnixOs;
        public List<string> SplitFiles = new List<string>();
        public string Epub3Directory = string.Empty;
        private readonly XslCompiledTransform _transformObj = new XslCompiledTransform();

        public Epub3Transformation(Exportepub exportepub, EpubFont epubFont)
        {
            _parent = exportepub;
            _epubFont = epubFont;
        }

        /// <summary>
        /// Entry point for epub 3 converter
        /// </summary>
        /// <param name="projInfo">values passed including epub2 exported files and changed to epub3 support</param>
        /// <returns>true if succeeds</returns>
        public bool Export(PublicationInformation projInfo)
        {
            bool epub3export = false;
            string oebpsPath;
            oebpsPath = Common.PathCombine(Epub3Directory, "OEBPS");

            var xhmltohtml5Space = Loadxhmltohtml5Xslt();

            string[] filesList = null;

            filesList = Convertxhtmltohtml5(oebpsPath, xhmltohtml5Space);

            ModifyContainerXML();

            ModifyContentOPF(projInfo, oebpsPath);
            epub3export = true;

            return epub3export;
        }

        private void ModifyContentOPF(PublicationInformation projInfo, string oebpsPath)
        {
            string contentOPFFile = Common.PathCombine(oebpsPath, "content.opf");

            if (File.Exists(contentOPFFile))
                File.Delete(contentOPFFile);

            var epubManifest = new EpubManifest(_parent, _epubFont);
            var bookId = Guid.NewGuid(); // NOTE: this creates a new ID each time Pathway is run. 
            epubManifest.CreateOpfV3(projInfo, oebpsPath, bookId);
        }

        private void ModifyContainerXML()
        {
            string modifyContainerFile = Common.PathCombine(Epub3Directory, "META-INF");
            modifyContainerFile = Common.PathCombine(modifyContainerFile, "container.xml");
            if (File.Exists(modifyContainerFile))
            {
                ModifyContainerXMLForEpub3(modifyContainerFile);
            }
        }

        private string[] Convertxhtmltohtml5(string oebpsPath, XslCompiledTransform xhmltohtml5Space)
        {
            string[] filesList = null;
            if (Directory.Exists(oebpsPath))
            {
                filesList = Directory.GetFiles(oebpsPath);
                foreach (var curFile in filesList)
                {
                    FileInfo fileInfo = new FileInfo(curFile);
                    if (fileInfo.Extension == ".xhtml")
                    {
                        Common.ApplyXslt(curFile, xhmltohtml5Space);

                        if (File.Exists(curFile))
                        {
                            File.Copy(curFile, curFile.Replace(".xhtml", ".html"), true);
                            File.Delete(curFile);
                        }
                    }
                }
            }
            return filesList;
        }

        private void ModifyContainerXMLForEpub3(string containerXmlFile)
        {
            Common.StreamReplaceInFile(containerXmlFile, "<?xml version=\"1.0\"?>", "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
        }

        private static XslCompiledTransform Loadxhmltohtml5Xslt()
        {
            var xhmltohtml5Stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("epubConvert.xhtmltohtml5.xslt");
            Debug.Assert(xhmltohtml5Stream != null);
            var xhmltohtml5 = new XslCompiledTransform();
            xhmltohtml5.Load(XmlReader.Create(xhmltohtml5Stream));
            return xhmltohtml5;
        }

    }
}
