// --------------------------------------------------------------------------------------------
// <copyright file="XsltProcess.cs" from='2009' to='2014' company='SIL International'>
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
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class XsltProcess
    {
        #region XsltProcess(string inputFile, string xsltFile, string ext)
        public Formatting XsltFormat = Formatting.None;
        public bool IncludeUtf8BomIdentifier = true;

        /// <summary>
        /// Process Input file with xslt and produce result with same name as input but extension changed.
        /// </summary>
        /// <param name="inputFile">input file</param>
        /// <param name="xsltFile">xslt file name. It is assumed to be in Dictionary Express folder</param>
        /// <param name="ext">new extension</param>
        /// <param name="PsAppPath"> </param>
        /// <returns>results or error message</returns>
        public string XsltTransform(string inputFile, string xsltFile, string ext, string PsAppPath)
        {
            if (!File.Exists(inputFile))
                throw new ArgumentException("No Input file name given.");

            if (!File.Exists(xsltFile))
                throw new ArgumentException("Input file doesn't exist");

            if (string.IsNullOrEmpty(ext) || ext == "null")
                ext = ".xhtml";

            string path = PsAppPath;
            string outputPath = Path.GetDirectoryName(inputFile);
            string result = PathCombine(outputPath, Path.GetFileNameWithoutExtension(inputFile) + ext);

            if (File.Exists(result))
            {
                File.Delete(result);
            }

            var xsltPath = Path.IsPathRooted(xsltFile)? xsltFile: PathCombine(path, xsltFile);

            //Create the XslCompiledTransform and load the stylesheet.
            var xsltReader = XmlReader.Create(xsltPath);
            var namespaceManager = new XmlNamespaceManager(xsltReader.NameTable);
            namespaceManager.AddNamespace("x", "http://www.w3.org/1999/xhtml");
            namespaceManager.AddNamespace("fn", "http://www.w3.org/2005/xpath-functions");
            var xslt = new XslCompiledTransform();
            var xsltTransformSettings = new XsltSettings { EnableDocumentFunction = true };
            xslt.Load(xsltReader, xsltTransformSettings, null);
            xsltReader.Close();

            //Create an XsltArgumentList.
            var xslArg = new XsltArgumentList();

            //Transform the file. and writing to temporary File
			var setting = new XmlReaderSettings { DtdProcessing = DtdProcessing.Parse, XmlResolver = null };
            var reader = XmlReader.Create(inputFile, setting);
            var writerSettings = new XmlWriterSettings();
            if (!IncludeUtf8BomIdentifier || !ext.ToLower().Contains("xhtml"))
            {
                writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            }
            if (IncludeUtf8BomIdentifier)
            {
                writerSettings.Encoding = Encoding.UTF8;
            }
            else
            {
                writerSettings.Encoding = new UTF8Encoding(IncludeUtf8BomIdentifier);
                IncludeUtf8BomIdentifier = true;   // reset to true for next time if it has been changed
            }
            if (XsltFormat == Formatting.Indented)
            {
                writerSettings.Indent = true;
                XsltFormat = Formatting.None;       // reset to None for next time if it has been changed
            }
            var writer = XmlWriter.Create(result, writerSettings);
            if (ext.ToLower().Contains("xhtml"))
            {
                writer.WriteStartDocument();
            }
            xslt.Transform(reader, xslArg, writer);
            writer.Close();
            reader.Close();
            return result;
        }
        #endregion

        /// <summary>
        /// Make sure the path contains the proper / for the operating system.
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <returns>normalized path</returns>
        public string PathCombine(string path1, string path2)
        {
            path1 = DirectoryPathReplace(path1);
            path2 = DirectoryPathReplace(path2);
            return Common.PathCombine(path1, path2);
        }

        /// <summary>
        /// Make sure the path contains the proper / for the operating system.
        /// </summary>
        /// <param name="path">input path</param>
        /// <returns>normalized path</returns>
        public string DirectoryPathReplace(string path)
        {
            if (string.IsNullOrEmpty(path)) return path;

            string returnPath = path.Replace('/', Path.DirectorySeparatorChar);
            returnPath = returnPath.Replace('\\', Path.DirectorySeparatorChar);
            return returnPath;

        }

    }
}
