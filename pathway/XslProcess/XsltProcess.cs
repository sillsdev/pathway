using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using SIL.Tool;

namespace PSXslProcess
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
        /// <param name="myParams">pass a dictionary of parameters and values</param>
        /// <param name="PsAppPath"> </param>
        /// <returns>results or error message</returns>
        public string XsltTransform(string inputFile, string xsltFile, string ext, string PsAppPath)
        {
            if (!File.Exists(inputFile))
                return string.Empty;

            if (!File.Exists(xsltFile))
                return inputFile;

            if (string.IsNullOrEmpty(ext) || ext == "null")
                ext = ".xhtml";

            try
            {
                string path = PsAppPath;
                string outputPath = Path.GetDirectoryName(inputFile);
                string result = PathCombine(outputPath, Path.GetFileNameWithoutExtension(inputFile) + ext);

                if (File.Exists(result))
                {
                    File.Delete(result);
                }

                string xsltPath = PathCombine(path, xsltFile);

                //Create the XslCompiledTransform and load the stylesheet.
                var xsltReader = XmlReader.Create(xsltPath);
                XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xsltReader.NameTable);
                namespaceManager.AddNamespace("x", "http://www.w3.org/1999/xhtml");
                namespaceManager.AddNamespace("fn", "http://www.w3.org/2005/xpath-functions");
                var xslt = new XslCompiledTransform();
                var xsltTransformSettings = new XsltSettings { EnableDocumentFunction = true };
                xslt.Load(xsltReader, xsltTransformSettings, null);
                xsltReader.Close();

                //Create an XsltArgumentList.
                var xslArg = new XsltArgumentList();

                //Add an object 
                //var obj = new FlexString();
                //var fun = new XmlFun(); // string-length replaed with stringLength
                //xslArg.AddExtensionObject("urn:reversal-conv", obj);
                //xslArg.AddExtensionObject("http://www.w3.org/2005/xpath-functions", fun);

                //if (myParams != null)
                //    foreach (string param in myParams.Keys)
                //    {
                //        xslArg.AddParam(param, "", myParams[param]);
                //    }

                //Transform the file. and writing to temporary File
                var setting = new XmlReaderSettings { ProhibitDtd = false, XmlResolver = null };
                XmlReader reader = XmlReader.Create(inputFile, setting);
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
                //var writer = new XmlTextWriter(result, Encoding.UTF8) {Namespaces = true};
                //writer.Formatting = XsltFormat;
                if (ext.ToLower().Contains("xhtml"))
                {
                    writer.WriteStartDocument();
                    //writer.WriteDocType("html", "-//W3C//DTD XHTML 1.0 Strict//EN", "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd", null);
                }
                xslt.Transform(reader, xslArg, writer);
                writer.Close();
                reader.Close();
                return result;
            }
            catch (FileNotFoundException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
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
            //if (path1 == null) throw new ArgumentNullException("path1");
            //if (path2 == null) throw new ArgumentNullException("path2");
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
