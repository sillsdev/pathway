// --------------------------------------------------------------------------------------------
// <copyright file="FileComponents.cs" from='2009' to='2014' company='SIL International'>
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
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Collections.Generic;

namespace BuildTasks
{
    public class FileComponents : Task
    {
        #region Properties
        #region BasePath
        private string _basePath = Environment.CurrentDirectory;

        public string BasePath
        {
            get { return _basePath; }
            set { _basePath = value; }
        }
        #endregion BasePath

        #region FilesTemplate
        private string _filesTemplate;
        [Required]
        public string FilesTemplate
        {
            get { return _filesTemplate; }
            set { _filesTemplate = value; }
        }
        #endregion FilesTemplate

        #region FeaturesTemplate
        private string _featuresTemplate;
        [Required]
        public string FeaturesTemplate
        {
            get { return _featuresTemplate; }
            set { _featuresTemplate = value; }
        }
        #endregion FeaturesTemplate
        #endregion Properties

        protected readonly XmlDocument XDoc = new XmlDocument();

        public override bool Execute()
        {
            var map = new Dictionary<string, string>();
            var path = _basePath;
            LoadGuids(Path.Combine(path, "FileLibrary.xml"));
            var directoryInfo = new DirectoryInfo(Path.Combine(path, "Files"));
            foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
            {
                ResetFileComponents();
                ProcessTree((XmlElement)XDoc.SelectSingleNode("//Files"), directory.FullName);
                AddFeatures();
                map[directory.Name + "Files"] = XDoc.SelectSingleNode("//Files//Directory").InnerXml;
                map[directory.Name + "Features"] = XDoc.SelectSingleNode("//Features").InnerXml;
            }
            var sub = new Substitution { TargetPath = path };
            sub.FileSubstitute(_filesTemplate, map, "Files.wxs");
            sub.FileSubstitute(_featuresTemplate, map, "Features.wxs");
            FileData.MoveToWix(Path.Combine(path,"Files.wxs"));
            FileData.MoveToWix(Path.Combine(path, "Features.wxs"));
            SaveGuids(Path.Combine(path, "FileLibrary.xml"));
            return true;
        }

        protected void ResetFileComponents()
        {
            XDoc.RemoveAll();
            XDoc.LoadXml("<partial><Files/><Features/></partial>");
        }

        protected readonly ArrayList CompIds = new ArrayList();

        protected void ProcessTree(XmlElement parent, string path)
        {
            var info = new DirectoryInfo(path);
            if (info.Name.Substring(0,1) == ".") return;
            XmlElement dirElem = CreateFileSystemElement("Directory", info);
            foreach (DirectoryInfo directoryInfo in info.GetDirectories())
            {
                ProcessTree(dirElem, directoryInfo.FullName);
            }
            foreach (FileInfo fileInfo in info.GetFiles())
            {
                var compElem = XDoc.CreateElement("Component");
                var fileElem = CreateFileSystemElement("File", fileInfo);
                AddAttribute("Checksum", "yes", fileElem);
                AddAttribute("KeyPath",  "yes", fileElem);
                AddAttribute("DiskId",   "1",   fileElem);
                AddAttribute("Source", GetSource(fileInfo), fileElem);
                compElem.AppendChild(fileElem);
                var compId = fileElem.Attributes.GetNamedItem("Id").Value;
                CompIds.Add(compId);
                AddAttribute("Id", compId, compElem);
                AddAttribute("Guid", GetGuid(fileInfo), compElem);
                dirElem.AppendChild(compElem);
            }
            parent.AppendChild(dirElem);
        }

        protected void AddFeatures()
        {
            XmlNode features = XDoc.SelectSingleNode("//Features");
            foreach (string compId in CompIds)
            {
                var compElem = XDoc.CreateElement("ComponentRef");
                AddAttribute("Id", compId, compElem);
                features.AppendChild(compElem);
            }
        }

        protected readonly Dictionary<string, string> Guids = new Dictionary<string, string>();

        protected string GetGuid(FileInfo fileInfo)
        {
            var guidPathKey = GetSource(fileInfo).Substring(3);
            if (Guids.ContainsKey(guidPathKey))
                return Guids[guidPathKey];
            var guid = Guid.NewGuid().ToString().ToUpper();
            Guids[guidPathKey] = guid;
            return guid;
        }

        protected void LoadGuids(string libraryPath)
        {
            XmlDocument GuidStore = new XmlDocument();
            GuidStore.Load(libraryPath);
            ResetIds();
            foreach (XmlNode child in GuidStore.DocumentElement.ChildNodes)
                Guids[child.Attributes.GetNamedItem("Path").Value] =
                    child.Attributes.GetNamedItem("ComponentGuid").Value;
        }

        protected void SaveGuids(string libraryPath)
        {
            XmlDocument GuidStore = new XmlDocument();
            GuidStore.LoadXml("<FileLibrary/>");
            foreach (string key in Guids.Keys)
            {
                var guidElem = GuidStore.CreateElement("File");
                AddAttribute("Path", key, guidElem, GuidStore);
                AddAttribute("ComponentGuid", Guids[key], guidElem, GuidStore);
                GuidStore.DocumentElement.AppendChild(guidElem);
            }

            var writer = new XmlTextWriter(libraryPath, Encoding.UTF8);
            GuidStore.WriteTo(writer);
            writer.Close();
        }

        protected string GetSource(FileInfo fileInfo)
        {
            var idx = fileInfo.FullName.IndexOf("\\Files\\");
            return ".." + fileInfo.FullName.Substring(idx);
        }

        private XmlElement CreateFileSystemElement(string tag, FileSystemInfo info)
        {
            var elem = XDoc.CreateElement(tag);
            AddAttribute("Id", MakeId(info.Name), elem);
            var shortName = Path.GetFileName(ShortName(info.FullName));
            AddAttribute("Name", shortName, elem);
            if (info.Name != shortName)
                AddAttribute("LongName", info.Name, elem);
            return elem;
        }

        /// <summary>
        /// Add Attribute tag with value to element
        /// </summary>
        private static void AddAttribute(string tag, string value, XmlElement element, XmlDocument doc)
        {
            var idAttr = doc.CreateAttribute(tag);
            idAttr.Value = value;
            element.Attributes.Append(idAttr);
        }
        private void AddAttribute(string tag, string value, XmlElement element)
        {
            AddAttribute(tag, value, element, XDoc);
        }

        protected static readonly Dictionary<string,int> AllIds = new Dictionary<string, int>();
        protected string MakeId(string name)
        {
            var id = name.ToCharArray();
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_.";
            const int notFound = -1;
            for (int iChar = 0; iChar < id.Length; iChar += 1)
                if (validChars.IndexOf(id[iChar]) == notFound)
                    id[iChar] = '_';
            var sb = new StringBuilder(id.Length);
            sb.Insert(0, id);
            string candidate = sb.ToString();
            const string validFirstChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz_";
            if (validFirstChar.IndexOf(id[0]) == notFound)
                candidate = "_" + candidate;
            const int maxLen = 72;
            if (candidate.Length > maxLen)
                candidate = candidate.Substring(0, maxLen);
            if (AllIds.ContainsKey(candidate))
            {
                // Candidate is not unique: it needs a numerical suffix; see what next available one is:
                var currentMax = AllIds[candidate] + 1;
                AllIds[candidate] = currentMax;
                if (candidate.Length >= maxLen - 3)
                    return candidate.Substring(0, maxLen - 3) + currentMax;
                return candidate + currentMax;
            }
            // If Id is unique, register it first, before returning it:
            AllIds[candidate] = 1;
            return candidate;
        }

        protected void ResetIds()
        {
            Guids.Clear();
            AllIds.Clear();
            CompIds.Clear();
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern uint GetShortPathName(string lpszLongPath, StringBuilder lpszShortPath, uint cchBuffer);

        public string ShortName(string name)
        {
            var length = GetShortPathName(name, null, 0);
            if (length == 0)
                throw new OutOfMemoryException("No space for short name");
            StringBuilder buffer = new StringBuilder(int.Parse(length.ToString()));
            var result = GetShortPathName(name, buffer, length);
            if (result == 0)
                throw new ArgumentException("Unable to create short name");
            return buffer.ToString();
        }
    }
}
