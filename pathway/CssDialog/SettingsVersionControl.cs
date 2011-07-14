using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Data;
using System.Collections;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class SettingsVersionControl
    {
        #region Protected Variable
        protected string Type;
        protected string _xmlFileWithPath;
        protected string ProjSchemaPath;
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

        /// <summary>
        /// Method to validate the alluser's settings files are updated, if no it migrate the latest changes
        /// into XML file without copy.
        /// </summary>
        /// <param name="appPath">Allusers settings XML path</param>
        public void UpdateSettingsFile(string appPath)
        {
            _xmlFileWithPath = appPath;
            string allUserPath = Path.Combine(Path.Combine(Common.GetAllUserAppPath(),"SIL\\Pathway"), Path.GetFileName(appPath));
            if (File.Exists(allUserPath))
            {
                _xmlFileWithPath = allUserPath;
            }
            Param.LoadValues(_xmlFileWithPath);

            //MigrateProjectSettingsFile(_xmlFileWithPath, "sSchemaPath", "sSettingsPath", appPath);
            MigrateProjectSettingsFile(_xmlFileWithPath, "projSchemaPath", "projSettingsPath", appPath);
        }

        /// <summary>
        /// Migrate Project settings file to the latest version
        /// </summary>
        /// <param name="appPath">all users path</param>
        /// <param name="schemaPath">Schema file path</param>
        /// <param name="settingsPath">Settings file path</param>
        /// <param name="installerPath">Installer path</param>
        private void MigrateProjectSettingsFile(string appPath, string schemaPath, string settingsPath, string installerPath)
        {
            Param.LoadSettings();
            string appSchemaVersion = GetAttrByName(GetDirectoryPath("appSchemaPath", installerPath), "xs:schema", "version");
            string projSchemaVersion = GetAttrByName(GetDirectoryPath(schemaPath, appPath), "xs:schema", "version");
            string projSettingsVerNum = GetAttrByName(GetDirectoryPath(settingsPath, appPath), "xs:schema", "version");
            if (File.Exists(GetDirectoryPath(schemaPath, appPath)))
            {
                if (projSchemaVersion != appSchemaVersion)
                {
                    File.Copy(GetDirectoryPath("appSchemaPath", installerPath), GetDirectoryPath(schemaPath, appPath), true);
                    projSchemaVersion = appSchemaVersion;
                }

                if (projSettingsVerNum != projSchemaVersion)
                {
                    //string appSettingsPath = GetDirectoryPath("appSettingsPath", appPath);
                    //if (settingsPath.IndexOf("proj") == 0)
                    //{
                    //    appSettingsPath = GetDirectoryPath("appProjSettingsPath", appPath);
                    //}
                    bool copiedMetadataBlock = false;

                    if(projSettingsVerNum == "0")
                    {
                        Version1(GetDirectoryPath(settingsPath, appPath), projSchemaVersion);
                    }
                    if (int.Parse(projSettingsVerNum) < 10)
                    {
                        Version10(GetDirectoryPath(settingsPath, appPath), installerPath, projSchemaVersion);
                        copiedMetadataBlock = true;
                        // XML was updated - reload the values
                        Param.LoadSettings();
                    }
                    if (int.Parse(projSettingsVerNum) < 13 && !copiedMetadataBlock)
                    {
                        Version13(GetDirectoryPath(settingsPath, appPath), projSchemaVersion);
                        Param.LoadSettings();
                    }
                    //Version2(GetDirectoryPath(settingsPath, appPath), appSettingsPath, projSchemaVersion);
                    string usrPath = GetDirectoryPath(settingsPath, appPath);
                    string insPath = Common.PathCombine(Path.GetDirectoryName(installerPath), Path.GetFileName(usrPath));
                    Common.MigrateCustomSheet(usrPath, insPath);
                }
            }
        }



        /// <summary>
        /// Method to return the path combinations
        /// </summary>
        /// <param name="DirectoryName">Directory name to get the complete path</param>
        /// <param name="appPath">default path</param>
        /// <returns></returns>
        private static string GetDirectoryPath(string DirectoryName, string appPath)
        {
            var xmlFile = new ArrayList(4)
                {
                                  "StyleSettings.xml",
                                  "DictionaryStyleSettings.xml",
                                  "ScriptureStyleSettings.xml",
                                  "StyleSettings.xsd"
                              };

            string projFile = Param.Value["InputType"].ToLower() == "scripture" ? xmlFile[2].ToString() : xmlFile[1].ToString();

            string path = string.Empty;
            switch (DirectoryName)
            {
                case "sSchemaPath":
                    path = Path.Combine(Common.PathCombine(Common.GetAllUserAppPath(), "SIL\\Pathway"), xmlFile[3].ToString());
                    break;
                case "sSettingsPath":
                    path = Path.Combine(Common.PathCombine(Common.GetAllUserAppPath(), "SIL\\Pathway"), xmlFile[0].ToString());
                    break;
                case "projSchemaPath":
                    path = Path.Combine(Path.GetDirectoryName(Param.SettingOutputPath), xmlFile[3].ToString());
                    break;
                case "projSettingsPath":
                    path = Path.Combine(Path.GetDirectoryName(Param.SettingOutputPath), projFile);
                    break;
                case "appSchemaPath":
                    path = Path.Combine(Path.GetDirectoryName(appPath), xmlFile[3].ToString());
                    break;
                case "appSettingsPath":
                    path = Path.Combine(Path.GetDirectoryName(appPath), xmlFile[0].ToString());
                    break;
                case "appProjSettingsPath":
                    path = Path.Combine(Path.GetDirectoryName(appPath), projFile);
                    break;
            }
            return path;
        }

        /// <summary>
        /// Method to update the changed made in version 1
        /// </summary>
        /// <param name="path">Settings file path</param>
        /// <param name="versionNumber">Updating version number</param>
        /// <returns>Nothing</returns>
        public void Version1(string path, string versionNumber)
        {
            if (!File.Exists(path)) { return; }

            var xdoc = new XmlDocument { XmlResolver = null };
            xdoc.Load(path);
            XmlElement root = xdoc.DocumentElement;
            if (root != null)
            {
                string sPath = "//stylePickstyles";
                root.SetAttribute("version", versionNumber);
                XmlNode searchNode = root.SelectSingleNode(sPath);
                if (searchNode != null && searchNode.ChildNodes.Count != 0)
                {
                    sPath = sPath + "/style";
                    XmlNodeList allNodes = root.SelectNodes(sPath);
                    if (allNodes != null && allNodes.Count > 0)
                    {
                        XmlNode newNode = xdoc.CreateNode("element", "paper", "");
                        searchNode.AppendChild(newNode);
                        if (!searchNode.InnerXml.Contains("web") || !searchNode.InnerXml.Contains("mobile"))
                        {
                            XmlDocumentFragment docFrag = InsertDefaultTag(xdoc);
                            searchNode.AppendChild(docFrag);
                        }
                        foreach (XmlNode node in allNodes)
                        {
                            if (!node.InnerXml.Contains("paper"))
                            {
                                newNode.AppendChild(node);
                            }
                        }
                    }
                }
            }
            xdoc.Save(path);
        }

        /// <summary>
        /// Update to changes made for TD-2344 (publication info) schema. This consists of copying over the Organizations
        /// and Metadata blocks from the StyleSettings.xml in the Program Files directory to the xml settings file in the
        /// ProgramData directory.
        /// </summary>
        /// <param name="destSettingsFile"></param>
        /// <param name="srcSettingsFile"></param>
        /// <param name="projSchemaVersion"></param>
        private void Version10(string destSettingsFile, string srcSettingsFile, string projSchemaVersion)
        {
            // load the source settings file (the one in Program Files) that has the <metadata> block
            if (!File.Exists(srcSettingsFile)) { return; }
            var srcDoc = new XmlDocument {XmlResolver = null};
            srcDoc.Load(srcSettingsFile);
            if (srcDoc.DocumentElement == null) {return;} // make sure we have something to copy over
            // load the destination settings file (the one in ProgramData) that is missing the <metadata> block);
            if (!File.Exists(destSettingsFile)) { return; }
            var destDoc = new XmlDocument { XmlResolver = null };
            destDoc.Load(destSettingsFile);
            XmlElement root = destDoc.DocumentElement;
            if (root != null)
            {
                root.SetAttribute("version", projSchemaVersion);
                // Organizations block
                string sPath = "//stylePick/Organizations";
                XmlNode orgsNode = srcDoc.DocumentElement.SelectSingleNode(sPath);
                if (orgsNode != null)
                {
                    XmlNode newOrgsNode = destDoc.ImportNode(orgsNode, true); // copy it into the ProgramData doc
                    root.AppendChild(newOrgsNode); // set the parent
                }
                // Metadata block
                sPath = "//stylePick/Metadata";
                XmlNode metadataNode = srcDoc.DocumentElement.SelectSingleNode(sPath);
                if (metadataNode != null)
                {
                    XmlNode newMetaNode = destDoc.ImportNode(metadataNode, true);
                    root.AppendChild(newMetaNode);
                    
                }
            }
            destDoc.Save(destSettingsFile);
        }

        /// <summary>
        /// Update to change made in version 13 of the XML. Here there is only one change - adding a TOC element.
        /// </summary>
        /// <param name="destSettingsFile"></param>
        /// <param name="projSchemaVersion"></param>
        private void Version13(string destSettingsFile, string projSchemaVersion)
        {
            // load the destination settings file (the one in ProgramData) that is missing the <meta> block for the TOC);
            if (!File.Exists(destSettingsFile)) { return; }
            var destDoc = new XmlDocument { XmlResolver = null };
            destDoc.Load(destSettingsFile);
            XmlElement root = destDoc.DocumentElement;
            if (root != null)
            {
                root.SetAttribute("version", projSchemaVersion);
                // Metadata block
                const string sPath = "//stylePick/Metadata";
                XmlNode metadataNode = root.SelectSingleNode(sPath);
                if (metadataNode != null)
                {
                    XmlElement newMetaNode = destDoc.CreateElement("meta");
                    newMetaNode.SetAttribute("name", "Table of Contents");
                    newMetaNode.SetAttribute("outputTypes", "all");
                    XmlElement defaultValueNode = destDoc.CreateElement("defaultValue");
                    defaultValueNode.InnerText = "false";
                    metadataNode.AppendChild(newMetaNode);
                    newMetaNode.AppendChild(defaultValueNode);
                }
            }
            destDoc.Save(destSettingsFile);
        }

        /// <summary>
        /// Method to update the changed made in version 2
        /// </summary>
        /// <param name="oldFile">Old version file</param>
        /// <param name="recentFile">Recent version file</param>
        /// <returns>Nothing</returns>
        public void Version2(string oldFile, string recentFile, string versionNumber)
        {
            try
            {
                var oReader = new XmlTextReader(oldFile);
                var rReader = new XmlTextReader(recentFile);

                var oData = new DataSet();
                oData.EnforceConstraints = false;
                oData.ReadXml(oReader, XmlReadMode.Auto);
                
                var rData = new DataSet();
                rData.EnforceConstraints = false;
                rData.ReadXml(rReader, XmlReadMode.Auto);

                rData.Merge(oData);
                rData.WriteXml(oldFile);

                rReader.Close();
                oReader.Close();

                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(oldFile);

                XmlElement ele = xdoc.DocumentElement;
                
                if (ele != null)
                {
                    ele.SetAttribute("version", versionNumber);
                    ele.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                    ele.SetAttribute("noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance"
                                     , "StyleSettings.xsd");

                    RemoveDuplicateNode(ele, "//stylePick/settings/property");
                    RemoveDuplicateNode(ele, "//stylePick/defaultSettings/property");
                    RemoveDuplicateChileNode(ele, "//stylePick/features", "feature", "option");
                    RemoveDuplicateChileNode(ele, "//stylePick/categories", "category", "item");
                    RemoveDuplicateNode(ele, "//stylePick/styles/paper/style");
                    RemoveDuplicateNode(ele, "//stylePick/styles/mobile/style");
                    RemoveDuplicateNode(ele, "//stylePick/styles/web/style");
                    RemoveDuplicateNode(ele, "//stylePick/styles/others/style");
                    RemoveDuplicateNode(ele, "//stylePick/roles/role");
                    RemoveDuplicateNode(ele, "//stylePick/tasks/task");
                    RemoveDuplicateNode(ele, "//stylePick/column-width/column");
                }

                xdoc.Save(oldFile);
            }
            catch (System.Exception ex)
            {

            }
        }

        /// <summary>
        /// To remove the duplicate childnode in the XML file based in parent and child combinations.
        /// </summary>
        /// <param name="ele">DocumentElement</param>
        /// <param name="xPath">xPath to search</param>
        /// <param name="parent">Parent node name</param>
        /// <param name="child">Child node name</param>
        private static void RemoveDuplicateChileNode(XmlNode ele, string xPath, string parent, string child)
        {
            if (ele != null)
            {
                XmlNodeList ndLst = ele.SelectNodes(xPath + @"/" + parent);
                if (ndLst != null)
                    foreach (XmlNode o in ndLst)
                    {
                        RemoveDuplicateNode(ele, xPath + @"/" + parent + "[@name=\"" + o.Attributes["name"].Value + "\"]//" + child);
                    }
            }
        }

        /// <summary>
        /// To remove the duplicate node in the XML file based xPath.
        /// </summary>
        /// <param name="ele">DocumentElement</param>
        /// <param name="xPath">xPath to search</param>
        private static void RemoveDuplicateNode(XmlNode ele, string xPath)
        {
            if (ele != null)
            {
                XmlNodeList ndLst = ele.SelectNodes(xPath);
                if (ndLst != null)
                    if (ndLst.Count > 1)
                    {
                        ArrayList node = new ArrayList();
                        foreach (XmlNode nodeName in ndLst)
                        {
                            if (node.Contains(nodeName.Attributes["name"].Value))
                            {
                                nodeName.ParentNode.RemoveChild(nodeName);
                            }
                            else
                            {
                                node.Add(nodeName.Attributes["name"].Value);
                            }
                        }
                    }
            }
        }

        /// <summary>
        /// To insert the xml default tag part into the alluser's XML file.
        /// </summary>
        /// <param name="xdoc">XMLDocument object</param>
        /// <returns>XmlDocumentFragment to attach in XMLFile</returns>
        private static XmlDocumentFragment InsertDefaultTag(XmlDocument xdoc)
        {
            const string toInsert = @"
            <mobile>
              <style name=""GPS11J11"" file=""GPS11J11.css"" approvedBy=""GPS"">
                <description>5.25x8.25in - 1 Col - Justified - Charis 11 on 13</description>
              </style>
            </mobile>
            <web>
              <style name=""GPS11J11"" file=""GPS11J11.css"" approvedBy=""GPS"">
                <description>5.25x8.25in - 1 Col - Justified - Charis 11 on 13</description>
              </style>
            </web>
            <others>
            <style name=""GPS11J11"" file=""GPS11J11.css"" approvedBy=""GPS"">
                <description>5.25x8.25in - 1 Col - Justified - Charis 11 on 13</description>
            </style>
            </others>
           ";
            XmlDocumentFragment docFrag = xdoc.CreateDocumentFragment();
            docFrag.InnerXml = toInsert;
            return docFrag;
        }

        /// <summary>
        /// Method to return the version number, if no version number attribute in the XML tag it return empty.
        /// </summary>
        /// <param name="xmlFileWithPath">XML filename with path</param>
        /// <param name="name">search tag</param>
        /// <param name="attr">search attribute</param>
        /// <returns>version number 0/1/empty</returns>
        public static string GetAttrByName(string xmlFileWithPath, string name, string attr)
        {
            if (!File.Exists(xmlFileWithPath)) return "";
            var xdoc = new XmlDocument();
            xdoc.Load(xmlFileWithPath);
            XmlNode node = xdoc.DocumentElement;
            if (node != null && node.Attributes.GetNamedItem(attr) != null)
                return node.Attributes.GetNamedItem(attr).Value;
            return "0";
        }
    }
}