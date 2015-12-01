// --------------------------------------------------------------------------------------------
// <copyright file="UpdateAssemblies.cs" from='2009' to='2014' company='SIL International'>
//      Copyright ( c ) 2009, SIL International. All Rights Reserved.   
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace BuildTasks
{
    public class UpdateAssemblies : Task
    {
        #region Properties
        #region RootFolder
        private string _rootFolder;
        [Required]
        public string RootFolder
        {
            get { return _rootFolder; }
            set { _rootFolder = value; }
        }
        #endregion RootFolder

        #region Product
        private string _product;
        [Required]
        public string Product
        {
            get { return _product; }
            set { _product = value; }
        }
        #endregion RootFolder

        #region BuildNumber
        private string _buildNumber;
        [Required]
        public string BuildNumber
        {
            get { return _buildNumber; }
            set { _buildNumber = value; }
        }
        #endregion BuildNumber
        #endregion Properties

        public override bool Execute()
        {
            var exp = new Regex(@"([0-9]+\.[0-9]+\.[0-9]+)\.[0-9]+");
            var match = exp.Match(_buildNumber);
            if (!match.Success)
                return false;
            UpdateVersion(_rootFolder, _buildNumber);
            var result = UpdateProduct(_product, _buildNumber);
            const bool overwrite = true;
            File.Copy(result, _product, overwrite);
            File.Delete(result);
            return true;
        }

        #region UpdateVersion
        /// <summary>
        /// Updates version # in all files Named AssemblyInfo in the tree with root curPath
        /// </summary>
        public static void UpdateVersion(string curPath, string version)
        {
            var aiName = "AssemblyInfo.cs";
            var di = new DirectoryInfo(curPath);
            if (di.GetFiles(aiName).Length > 0)
            {
                var sub = new Substitution { InputFile = aiName, OutputFile = aiName, TargetPath = curPath };
                //MessageBox.Show(curPath);
                try
                {
                    sub.UpdateGroup1(@".assembly. AssemblyFileVersion..([0-9.]+)...", version);
                    sub.UpdateGroup1(@".assembly. AssemblyVersion..([0-9.]+)...", version);
                }
                catch (Exception)
                {
                }
            }
            var fiList = di.GetDirectories();
            foreach (var fi in fiList)
            {
                var startsWith = fi.Name.Substring(0, 1);
                if (startsWith == "." || startsWith == "_")
                    continue;
                UpdateVersion(Path.Combine(curPath, fi.Name), version);
            }
        }
        #endregion UpdateVersion

        #region UpdateProduct
        public static string UpdateProduct(string product, string buildNumber)
        {
            var prodDoc = new XmlDocument{XmlResolver = null};
            prodDoc.Load(product);
            XmlProcessingInstruction lastOne = null;
            bool foundIt = false;
            Debug.Assert(prodDoc.DocumentElement != null, "prodDoc.DocumentElement != null");
            foreach (var childNode in prodDoc.ChildNodes.Cast<XmlNode>().Where(childNode => childNode.NodeType == XmlNodeType.ProcessingInstruction).Cast<XmlProcessingInstruction>())
            {
                if (childNode.Value.StartsWith("BUILD_NUMBER"))
                {
                    childNode.Value = string.Format(@"BUILD_NUMBER=""{0}""", buildNumber);
                    foundIt = true;
                }
                else
                {
                    lastOne = childNode;
                }
            }
            if (!foundIt)
            {
                var verProcInst = prodDoc.CreateProcessingInstruction("define",
                    string.Format(@"BUILD_NUMBER=""{0}""", buildNumber));
                prodDoc.InsertAfter(verProcInst, lastOne);
            }
            var tempName = Path.GetTempFileName();
            var writer = XmlTextWriter.Create(tempName, new XmlWriterSettings {Indent = true, Encoding = Encoding.UTF8});
            prodDoc.Save(writer);
            writer.Close();
            return tempName;
        }
        #endregion UpdateProduct
    }
}
