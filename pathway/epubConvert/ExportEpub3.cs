using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SIL.Tool;

namespace epubConvert
{
    public class ExportEpub3
    {
        public bool IsUnixOs;
        public List<string> SplitFiles = new List<string>();
        public string Epub3Directory = string.Empty;


        /// <summary>
        /// Entry point for epub 3 converter
        /// </summary>
        /// <param name="projInfo">values passed including epub2 exported files and changed to epub3 support</param>
        /// <returns>true if succeeds</returns>
        public bool Export(PublicationInformation projInfo)
        {
            bool epub3export = false;

            string modifyContainerFile = Common.PathCombine(Epub3Directory, "META-INF");
            modifyContainerFile = Common.PathCombine(modifyContainerFile, "container.xml");
            if (File.Exists(modifyContainerFile))
            {
                ModifyContainerXMLForEpub3(modifyContainerFile);
            }



            return epub3export;
        }

        private void ModifyContainerXMLForEpub3(string containerXmlFile)
        {
            Common.StreamReplaceInFile(containerXmlFile, "<?xml version=\"1.0\"?>", "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
        }
    }
}
