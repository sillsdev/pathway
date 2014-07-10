using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
            string modifyContainerFile = Common.PathCombine(Epub3Directory, "META-INF");
            modifyContainerFile = Common.PathCombine(modifyContainerFile, "container.xml");
            if (File.Exists(modifyContainerFile))
            {
                ModifyContainerXMLForEpub3(modifyContainerFile);
            }

            oebpsPath = Common.PathCombine(Epub3Directory, "OEBPS");
            string contentOPFFile  = Common.PathCombine(oebpsPath, "content.opf");

            if(File.Exists(contentOPFFile))
                File.Delete(contentOPFFile);

            var epubManifest = new EpubManifest(_parent, _epubFont);
            var bookId = Guid.NewGuid(); // NOTE: this creates a new ID each time Pathway is run. 
            epubManifest.CreateOpfV3(projInfo, oebpsPath, bookId);

            return epub3export;
        }

        private void ModifyContainerXMLForEpub3(string containerXmlFile)
        {
            Common.StreamReplaceInFile(containerXmlFile, "<?xml version=\"1.0\"?>", "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
        }
    }
}
