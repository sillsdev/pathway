// --------------------------------------------------------------------------------------------
// <copyright file="SettingsVersionControl.cs" from='2009' to='2014' company='SIL International'>
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
// Validation of settings version
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Data;
using System.Collections;
using SIL.Tool;
using System.Text;

namespace SIL.PublishingSolution
{
    public class SettingsVersionControl
    {
        #region Protected Variable
        protected string Type;
        protected string ProjSchemaPath;

        private string _userFilePath, _pathwayFilePath;
        private XmlDocument _userXml, _pathwayXml;
        private XmlElement _userRoot, _pathwayRoot;
        private bool _compareVersion = false;

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the frmMDIParent class.
        /// </summary>
        public SettingsVersionControl()
        {

        }
        public SettingsVersionControl(string type)
        {
            Type = type;
        }
        #endregion

        public void UpdateSettingsFile(string appPath)
        {
            Dictionary<string, string> inputType = new Dictionary<string, string>();
            inputType.Add("SIL/Pathway/Dictionary/DictionaryStyleSettings.xml", "DictionaryStyleSettings.xml");
            inputType.Add("SIL/Pathway/Scripture/ScriptureStyleSettings.xml", "ScriptureStyleSettings.xml");

            foreach (KeyValuePair<string, string> data in inputType)
            {
                string styleSettingsFile = Common.PathCombine(Path.GetDirectoryName(appPath), data.Value);
                if (!File.Exists(styleSettingsFile))
                {
                    continue;
                }
                if (CreateSettingsFile(appPath, data))
                {
                    continue;
                }

                _pathwayFilePath = Common.PathCombine(Path.GetDirectoryName(appPath), data.Value);
                _userFilePath = Common.PathCombine(Common.GetAllUserAppPath(), data.Key);

                if (!OpenFile())
                {
                    continue;
                }

                if (CompareVersion())
                {
                    continue;
                }

                if (!OpenFile())
                {
                    continue;
                }

                CopyXmlAttribute("//stylePick/settings/property");
                CopyXmlAttribute("//stylePick/defaultSettings/property");

                CopyXmlNode("//styles/*/style[@type='Custom']", "//styles/");

                CopyOrganization("//Organizations/Organization");

                CopyXmlMatadata("//Metadata/meta", "//Metadata/");

                CloseFile();

                if (!Common.Testing)
                {
                    File.Delete(_userFilePath);
                }
            }

            if (!_compareVersion)
            {
                const string settingFileXsd = "StyleSettings.xsd";
                const string settingFileXml = "StyleSettings.xml";
                string pathwayFolder = Common.PathCombine(Common.GetAllUserAppPath(), "SIL");
                pathwayFolder = Common.PathCombine(pathwayFolder, "Pathway");
                File.Copy(Common.PathCombine(Path.GetDirectoryName(appPath), settingFileXsd),
                          Common.PathCombine(pathwayFolder, settingFileXsd), true);
                File.Copy(Common.PathCombine(Path.GetDirectoryName(appPath), settingFileXml),
                          Common.PathCombine(pathwayFolder, settingFileXml), true);
            }

        }

        private bool CreateSettingsFile(string appPath, KeyValuePair<string, string> data)
        {
            bool fileSettingFileCreated = false;
            _pathwayFilePath = Common.PathCombine(Path.GetDirectoryName(appPath), data.Value);
            _userFilePath = Common.PathCombine(Common.GetAllUserAppPath(), data.Key);
            string pathwayFolder = Common.PathCombine(Common.GetAllUserAppPath(), "SIL");
            pathwayFolder = Common.PathCombine(pathwayFolder, "Pathway");
            if (!Directory.Exists(pathwayFolder) || !Directory.Exists(_userFilePath))
            {
                if (!File.Exists(_userFilePath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(_userFilePath));
                }
            }
            if (File.Exists(_userFilePath) && !Directory.Exists(Path.GetDirectoryName(_userFilePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_userFilePath));
            }
            if (!File.Exists(_userFilePath))
            {
                File.Copy(_pathwayFilePath, _userFilePath, true);
                fileSettingFileCreated = true;
            }
            CopySamplePictureFiles(pathwayFolder);
            return fileSettingFileCreated;
        }

        private void CopySamplePictureFiles(string allUserAppPath)
        {
            string picturePath = Common.PathCombine(allUserAppPath, "Pictures");
            if(Directory.Exists(allUserAppPath))
            {
                if (!Directory.Exists(picturePath))
                {
                    Directory.CreateDirectory(picturePath);
                }
                string samplePicturePath = Common.GetPSApplicationPath();
                samplePicturePath = Common.PathCombine(samplePicturePath, "Samples");
                samplePicturePath = Common.PathCombine(samplePicturePath, "Dictionary");
                samplePicturePath = Common.PathCombine(samplePicturePath, "Pictures");

                if (Directory.Exists(samplePicturePath))
                {
                    string[] pictureFilesList = Directory.GetFiles(samplePicturePath, "*.jpg");
                    CopyPictureFiles(pictureFilesList, picturePath);
                    pictureFilesList = Directory.GetFiles(samplePicturePath, "*.tif");
                    CopyPictureFiles(pictureFilesList, picturePath);
                }

                samplePicturePath = Common.GetPSApplicationPath();
                samplePicturePath = Common.PathCombine(samplePicturePath, "Samples");
                samplePicturePath = Common.PathCombine(samplePicturePath, "Scripture");
                samplePicturePath = Common.PathCombine(samplePicturePath, "Pictures");

                if (Directory.Exists(samplePicturePath))
                {
                    string[] pictureFilesList = Directory.GetFiles(samplePicturePath, "*.jpg");
                    CopyPictureFiles(pictureFilesList, picturePath);
                }
            }
        }

        private static void CopyPictureFiles(string[] pictureFilesList, string picturePath)
        {
            foreach (var pictureFile in pictureFilesList)
            {
                string pictureFileName = Path.GetFileName(pictureFile);
                string pictureFileNamewithDirectory = Common.PathCombine(picturePath, pictureFileName);
                if (!File.Exists(pictureFileNamewithDirectory))
                {
                    File.Copy(pictureFile, pictureFileNamewithDirectory, true);
                }
            }
        }

        private void CopyOrganization(string xPath)
        {
            XmlNode userNode = _userRoot.SelectSingleNode(xPath);
            XmlNode pathwayNode = _pathwayRoot.SelectSingleNode(xPath);
            if (pathwayNode != null)
            {
                pathwayNode.InnerText = userNode.InnerText;
            }
        }

        private void CopyXmlMatadata(string xPath, string targetXPath)
        {
            XmlNodeList userNodeList = _userRoot.SelectNodes(xPath);
            foreach (XmlNode userNode in userNodeList)
            {
                if (userNode.Attributes != null)
                {
                    string propName = userNode.Attributes["name"].Value;
                    string parent = targetXPath + "meta[@name='" + propName + "']";

                    XmlNode pathwayNode = _pathwayRoot.SelectSingleNode(parent);
                    if (pathwayNode != null)
                    {
                        pathwayNode.InnerXml = userNode.InnerXml;
                    }
                }
            }
        }

        private void CopyXmlNode(string xPath, string targetXPath)
        {
            XmlNodeList userNodeList = _userRoot.SelectNodes(xPath);
            foreach (XmlNode userNode in userNodeList)
            {
                string parent = targetXPath + userNode.ParentNode.Name;
                XmlNode pathwayNode = _pathwayRoot.SelectSingleNode(parent);
                if (userNode.ParentNode.Name.ToLower() == "web")
                {
                    pathwayNode.InnerXml = userNode.OuterXml;
                }
                else
                {
                    XmlNode newNode = _pathwayXml.ImportNode(userNode, true);
                    pathwayNode.AppendChild(newNode);
                }
            }
        }

        private void CopyXmlAttribute(string xPath)
        {
            XmlNodeList userNodeList = _userRoot.SelectNodes(xPath);
            foreach (XmlNode userNode in userNodeList)
            {
                string propName = userNode.Attributes["name"].Value;
                string propValue = userNode.Attributes["value"].Value;

                if (propValue.Trim().Length == 0)
                {
                    continue;
                }

                XmlNode pathwayNode = _pathwayRoot.SelectSingleNode(xPath + "[@name='" + propName + "']");
                if (pathwayNode != null)
                {
                    pathwayNode.Attributes["value"].Value = propValue;
                }

            }
        }

        private void CloseFile()
        {
            _pathwayXml.Save(_pathwayFilePath);
        }

        private bool CompareVersion()
        {
            const string ver = "version";
            string userVersion = "0";

            if (_userRoot != null && _userRoot.Attributes.GetNamedItem(ver) != null)
            {
                userVersion = _userRoot.Attributes.GetNamedItem(ver).Value;
            }

            string pathwayVersion = "0";

            if (_pathwayRoot != null && _pathwayRoot.Attributes.GetNamedItem(ver) != null)
            {
                pathwayVersion = _pathwayRoot.Attributes.GetNamedItem(ver).Value;
            }
            
            if (userVersion == pathwayVersion)
            {
                _compareVersion = true;
                return true;
            }
            else
            {
                string filePath = _userFilePath;
                _userFilePath = _userFilePath.Replace(".xml", "Temp.xml");
                File.Copy(filePath, _userFilePath, true);
                File.Copy(_pathwayFilePath, filePath, true);

                //Copy xsd file
                const string settingFileXsd = "StyleSettings.xsd";
                string localFilePath = Path.GetDirectoryName(_userFilePath);
                string insallerFilePath = Path.GetDirectoryName(_pathwayFilePath);
                localFilePath = Common.PathCombine(localFilePath, settingFileXsd);
                insallerFilePath = Common.PathCombine(insallerFilePath, settingFileXsd);
                File.Copy(insallerFilePath, localFilePath, true);

                _pathwayFilePath = filePath;

                return false;
            }
        }


        private bool OpenFile()
        {
            if (!File.Exists(_userFilePath) || !File.Exists(_pathwayFilePath))
            {
                return false;
            }
           
            _userXml = Common.DeclareXMLDocument(false);
            _userXml.Load(_userFilePath);
            _userRoot = _userXml.DocumentElement;
            if (_userRoot == null)
            {
                return false;
            }

            _pathwayXml = new XmlDocument();
            _pathwayXml.Load(_pathwayFilePath);
            _pathwayRoot = _pathwayXml.DocumentElement;
            if (_pathwayRoot == null)
            {
                return false;
            }

            return true;
        }
    }
}