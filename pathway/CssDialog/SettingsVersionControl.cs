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
        protected string _xmlFileWithPath;
        protected string ProjSchemaPath;

        private string _userFilePath, _pathwayFilePath;
        private XmlDocument _userXml, _pathwayXml;
        private XmlElement _userRoot, _pathwayRoot;


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

                SetPath(appPath, data);

                if (!OpenFile())
                {
                    continue;
                }

                if (CompareVersion())
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
                    File.Copy(_pathwayFilePath, _userFilePath, true);
                }
            }
            const string settingFile = "StyleSettings.xsd";

            if (!Common.Testing)
            {
                File.Copy(Common.PathCombine(Path.GetDirectoryName(_pathwayFilePath), settingFile),
                          Common.PathCombine(Path.GetDirectoryName(_userFilePath), settingFile), true);
            }

        }

        private void SetPath(string appPath, KeyValuePair<string, string> data)
        {
            if (Common.Testing)
            {
                string testPath = Environment.CurrentDirectory;
                testPath = Common.LeftString(testPath, "\\bin\\Debug");
                string input = Common.PathCombine(testPath, "ConfigurationTool\\TestFiles\\input");
                string output = Common.PathCombine(testPath, "ConfigurationTool\\TestFiles\\Output");

                _userFilePath = Common.PathCombine(input, data.Value);
                _pathwayFilePath = Path.Combine(output, data.Value);

                File.Copy(_userFilePath, _pathwayFilePath, true);

            }
            else
            {
                _userFilePath = Common.PathCombine(Common.GetAllUserAppPath(), data.Key);
                _pathwayFilePath = Path.Combine(Path.GetDirectoryName(appPath), data.Value);
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
                string propName = userNode.Attributes["name"].Value;
                string parent = targetXPath + "meta[@name='" + propName + "']";

                XmlNode pathwayNode = _pathwayRoot.SelectSingleNode(parent);
                //pathwayNode.ParentNode.RemoveChild(pathwayNode);
                if (pathwayNode != null)
                {
                    pathwayNode.InnerXml = userNode.InnerXml;
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
                    pathwayNode.InnerXml = userNode.InnerXml;
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
            _userXml.Save(_userFilePath);
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
                return true;
            }
            else
            {
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

            _pathwayXml = Common.DeclareXMLDocument(false);
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