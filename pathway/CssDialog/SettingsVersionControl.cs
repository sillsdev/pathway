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
            var sbFile = new StringBuilder();
            sbFile.Append(Common.GetAllUserAppPath());
            sbFile.Append(Path.DirectorySeparatorChar);
            sbFile.Append("SIL");
            sbFile.Append(Path.DirectorySeparatorChar);
            sbFile.Append("Pathway");
            sbFile.Append(Path.DirectorySeparatorChar);
            sbFile.Append("Dictionary");
            sbFile.Append(Path.DirectorySeparatorChar);
            sbFile.Append(Path.GetFileName(appPath));
            string allUserPath = sbFile.ToString();
            if (File.Exists(allUserPath))
            {
                _xmlFileWithPath = allUserPath;
            }
            if (Param.LoadValues(_xmlFileWithPath) == null)
            {
                return;
            }

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

                    if (projSettingsVerNum == "0")
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
                        Version13(GetDirectoryPath(settingsPath, appPath));
                        Param.LoadSettings();
                    }
                    if (int.Parse(projSettingsVerNum) < 14)
                    {
                        Version14(GetDirectoryPath(settingsPath, appPath));
                        Param.LoadSettings();
                    }
                    if (int.Parse(projSchemaVersion) < 16)
                    {
                        Version16(GetDirectoryPath(settingsPath, appPath));
                        Param.LoadSettings();
                    }
                    if (projSettingsVerNum == "17")
                    {
                        Version17(GetDirectoryPath(settingsPath, appPath), projSchemaVersion);
                        Param.LoadSettings();
                    }

                    if (projSettingsVerNum == "18")
                    {
                        Version18(GetDirectoryPath(settingsPath, appPath), projSchemaVersion);
                        Param.LoadSettings();
                    }

                    if (projSettingsVerNum == "19")
                    {
                        Version19(GetDirectoryPath(settingsPath, appPath), projSchemaVersion);
                        Param.LoadSettings();
                    }

                    if (int.Parse(projSettingsVerNum) <= 19)
                    {
                        Version20(GetDirectoryPath(settingsPath, appPath), projSchemaVersion);
                        Param.LoadSettings();
                    }

                    if (int.Parse(projSettingsVerNum) < 26)
                    {
                        Version26(GetDirectoryPath(settingsPath, appPath));
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
            var sbPath = new StringBuilder();

            switch (DirectoryName)
            {
                case "sSchemaPath":
                    sbPath.Append(Common.GetAllUserAppPath());
                    sbPath.Append(Path.DirectorySeparatorChar);
                    sbPath.Append("SIL");
                    sbPath.Append(Path.DirectorySeparatorChar);
                    sbPath.Append("Pathway");
                    sbPath.Append(Path.DirectorySeparatorChar);
                    sbPath.Append(xmlFile[3].ToString());
                    path = sbPath.ToString();
                    //path = Path.Combine(Common.PathCombine(Common.GetAllUserAppPath(), "SIL\\Pathway"), xmlFile[3].ToString());
                    break;
                case "sSettingsPath":
                    sbPath.Append(Common.GetAllUserAppPath());
                    sbPath.Append(Path.DirectorySeparatorChar);
                    sbPath.Append("SIL");
                    sbPath.Append(Path.DirectorySeparatorChar);
                    sbPath.Append("Pathway");
                    sbPath.Append(Path.DirectorySeparatorChar);
                    sbPath.Append(xmlFile[0].ToString());
                    path = sbPath.ToString();
                    //path = Path.Combine(Common.PathCombine(Common.GetAllUserAppPath(), "SIL\\Pathway"), xmlFile[0].ToString());
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

            var xdoc = Common.DeclareXMLDocument(false);
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
            var srcDoc = Common.DeclareXMLDocument(false);
            srcDoc.Load(srcSettingsFile);
            if (srcDoc.DocumentElement == null) { return; } // make sure we have something to copy over
            // load the destination settings file (the one in ProgramData) that is missing the <metadata> block);
            if (!File.Exists(destSettingsFile)) { return; }
            var destDoc = Common.DeclareXMLDocument(false);
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
        private void Version13(string destSettingsFile)
        {
            // load the destination settings file (the one in ProgramData) that is missing the <meta> block for the TOC);
            if (!File.Exists(destSettingsFile)) { return; }
            var destDoc = Common.DeclareXMLDocument(false);
            destDoc.Load(destSettingsFile);
            XmlElement root = destDoc.DocumentElement;
            if (root != null)
            {
                root.SetAttribute("version", "13");
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
        /// Update to change made in version 14 of the XML. The change here is a References feature block for epub / scriptures (only).
        /// </summary>
        /// <param name="destSettingsFile"></param>
        private void Version14(string destSettingsFile)
        {
            // load the destination settings file (the one in ProgramData) that is missing the <meta> block for the TOC);
            if (!File.Exists(destSettingsFile)) { return; }
            var destDoc = Common.DeclareXMLDocument(false);
            destDoc.Load(destSettingsFile);
            XmlElement root = destDoc.DocumentElement;
            if (root != null)
            {
                root.SetAttribute("version", "14");
                // Metadata block
                const string referencesPath = "//stylePick/features/feature[@name=\"References\"]";
                const string featuresPath = "//stylePick/features";
                XmlNode referencesNode = root.SelectSingleNode(referencesPath);
                XmlNode featuresNode = root.SelectSingleNode(featuresPath);
                if (featuresNode == null) { return; }
                if (referencesNode != null)
                {
                    XmlElement newNode = destDoc.CreateElement("feature");
                    newNode.SetAttribute("name", "References");
                    XmlElement optionNode1 = destDoc.CreateElement("option");
                    optionNode1.SetAttribute("name", "After Each Section");
                    optionNode1.SetAttribute("file", "Empty.css");
                    XmlElement optionNode2 = destDoc.CreateElement("option");
                    optionNode2.SetAttribute("name", "At End of Document");
                    optionNode2.SetAttribute("file", "Empty.css");
                    newNode.AppendChild(optionNode1);
                    newNode.AppendChild(optionNode2);
                    featuresNode.AppendChild(newNode);
                }
            }
            destDoc.Save(destSettingsFile);
        }

        /// <summary>
        /// Update to change made in version 15 of the XML. Filter parameters replaced by Preprocessing parameter.
        /// </summary>
        /// <param name="destSettingsFile"></param>
        private void Version16(string destSettingsFile)
        {
            if (!File.Exists(destSettingsFile)) { return; }
            var destDoc = Common.DeclareXMLDocument(false);
            destDoc.Load(destSettingsFile);
            XmlElement root = destDoc.DocumentElement;
            if (root != null)
            {
                root.SetAttribute("version", "16");
                const string settingsPath = "//stylePick/setting";
                XmlNode settingsNode = root.SelectSingleNode(settingsPath);
                if (settingsNode != null)
                {
                    var preprocessingNode = destDoc.CreateElement("property");
                    var nameAttr = destDoc.CreateAttribute("name");
                    nameAttr.Value = "Preprocessing";
                    preprocessingNode.Attributes.Append(nameAttr);
                    var valueAttr = destDoc.CreateAttribute("value");
                    valueAttr.Value = string.Empty;
                    preprocessingNode.Attributes.Append(valueAttr);
                    settingsNode.AppendChild(preprocessingNode);
                }
                const string emptyFilterPath = "//stylePick/setting/property[@name=\"FilterEmptyEntries\"]";
                XmlNode emptyFilterNode = root.SelectSingleNode(emptyFilterPath);
                if (emptyFilterNode != null)
                {
                    Debug.Assert(emptyFilterNode.ParentNode != null);
                    emptyFilterNode.ParentNode.RemoveChild(emptyFilterNode);
                }
                const string brokenFilterPath = "//stylePick/setting/property[@name=\"FilterBrokenLinks\"]";
                XmlNode brokenFilterNode = root.SelectSingleNode(brokenFilterPath);
                if (brokenFilterNode != null)
                {
                    Debug.Assert(brokenFilterNode.ParentNode != null);
                    brokenFilterNode.ParentNode.RemoveChild(brokenFilterNode);
                }

            }
        }

        /// <summary>
        /// Method to update the changed made in version 1
        /// </summary>
        /// <param name="path">Settings file path</param>
        /// <param name="versionNumber">Updating version number</param>
        /// <returns>Nothing</returns>
        public void Version17(string path, string versionNumber)
        {
            if (!File.Exists(path)) { return; }

            var xdoc = Common.DeclareXMLDocument(false);
            xdoc.Load(path);
            XmlElement root = xdoc.DocumentElement;
            if (root != null)
            {
                string sPath = "//stylePick/styles/web/style";
                root.SetAttribute("version", versionNumber);
                XmlNode searchNode = root.SelectSingleNode(sPath);
                if (searchNode != null && searchNode.ChildNodes.Count != 0)
                {
                    XmlNodeList allNodes = root.SelectNodes(sPath);
                    if (allNodes != null && allNodes.Count > 0)
                    {
                        if (searchNode.InnerXml.Contains("ftpaddress"))
                        {
                            if (!searchNode.InnerXml.Contains("webadmin"))
                            {
                                XmlDocumentFragment docFrag = InsertDefaultTagVersion17(xdoc);
                                searchNode.AppendChild(docFrag);
                            }
                        }
                    }
                }
            }
            xdoc.Save(path);
        }

        /// <summary>
        /// Method to update the changed made in version 19
        /// </summary>
        /// <param name="path">Settings file path</param>
        /// <param name="versionNumber">Updating version number</param>
        /// <returns>Nothing</returns>
        public void Version18(string path, string versionNumber)
        {
            if (!File.Exists(path)) { return; }

            var xdoc = Common.DeclareXMLDocument(false);
            xdoc.Load(path);
            XmlElement root = xdoc.DocumentElement;
            if (root != null)
            {
                string sPath = "//stylePick/Metadata/meta[@name='Title']";
                root.SetAttribute("version", versionNumber);
                XmlNode searchNode = root.SelectSingleNode(sPath);
                if (searchNode != null && searchNode.ChildNodes.Count != 0)
                {
                    XmlNodeList allNodes = root.SelectNodes(sPath);
                    if (allNodes != null && allNodes.Count > 0)
                    {
                        XmlDocumentFragment docFrag = InsertDefaultTagVersion18(xdoc);
                        searchNode.AppendChild(docFrag);
                    }
                }
            }
            xdoc.Save(path);
        }

        /// <summary>
        /// Method to update the changed made in version 20
        /// </summary>
        /// <param name="path">Settings file path</param>
        /// <param name="versionNumber">Updating version number</param>
        /// <returns>Nothing</returns>
        public void Version19(string path, string versionNumber)
        {
            if (!File.Exists(path)) { return; }

            var xdoc = Common.DeclareXMLDocument(false);
            xdoc.Load(path);
            XmlElement root = xdoc.DocumentElement;
            if (root != null)
            {
                string sPath = "//stylePick/styles/others/style";
                root.SetAttribute("version", versionNumber);
                XmlNodeList searchNode = root.SelectNodes(sPath);
                if (searchNode != null && searchNode.Count != 0)
                {
                    foreach (XmlNode styleNode in searchNode)
                    {
                        XmlDocumentFragment docFrag = InsertDefaultTagVersion19(xdoc);
                        styleNode.AppendChild(docFrag);
                    }
                }
            }
            xdoc.Save(path);
        }

        /// <summary>
        /// Method to update the changed made in version 20
        /// </summary>
        /// <param name="path">Settings file path</param>
        /// <param name="versionNumber">Updating version number</param>
        /// <returns>Nothing</returns>
        public void Version20(string path, string versionNumber)
        {
            if (!File.Exists(path)) { return; }

            var xdoc = Common.DeclareXMLDocument(false);
            xdoc.Load(path);
            XmlElement root = xdoc.DocumentElement;
            if (root != null)
            {
                string sPath = "//stylePick/styles/mobile/style";
                root.SetAttribute("version", versionNumber);
                XmlNode searchNode = root.SelectSingleNode(sPath);
                if (searchNode != null && searchNode.ChildNodes.Count != 0)
                {
                    XmlNodeList allNodes = root.SelectNodes(sPath);
                    if (allNodes != null && allNodes.Count > 0)
                    {
                        if (searchNode.InnerXml.Contains("FileProduced"))
                        {
                            if (!searchNode.InnerXml.Contains("Language"))
                            {
                                XmlDocumentFragment docFrag = InsertDefaultTagVersion17(xdoc);
                                searchNode.AppendChild(docFrag);
                            }
                        }
                    }
                }
            }
            xdoc.Save(path);
        }

        /// <summary>
        /// Update to change made in version 15 of the XML. The change here is a References feature block for Dictionary / scriptures (only).
        /// </summary>
        /// <param name="destSettingsFile"></param>
        private void Version26(string destSettingsFile)
        {
            // load the destination settings file (the one in ProgramData) that is missing the <meta> block for the TOC);
            if (!File.Exists(destSettingsFile)) { return; }
            var destDoc = Common.DeclareXMLDocument(false);
            destDoc.Load(destSettingsFile);
            XmlElement root = destDoc.DocumentElement;
            if (root != null)
            {
                root.SetAttribute("version", "26");
                // Metadata block
                const string referencesPath = "//stylePick/features/feature[@name=\"Page Number\"]";
                const string featuresPath = "//stylePick/features";
                XmlNode referencesNode = root.SelectSingleNode(referencesPath);
                XmlNode featuresNode = root.SelectSingleNode(featuresPath);
                if (featuresNode == null) { return; }
                if (referencesNode != null)
                {
                    XmlElement newNode = destDoc.CreateElement("feature");
                    newNode.SetAttribute("name", "Page Number");

                    XmlElement optionNode11 = destDoc.CreateElement("option");
                    optionNode11.SetAttribute("name", "Top Inside Margin");
                    optionNode11.SetAttribute("file", "PageNumber_TopInside.css");
                    optionNode11.SetAttribute("type", "Mirrored");

                    XmlElement optionNode12 = destDoc.CreateElement("option");
                    optionNode12.SetAttribute("name", "Top Outside Margin");
                    optionNode12.SetAttribute("file", "PageNumber_TopOutside.css");
                    optionNode12.SetAttribute("type", "Mirrored");


                    XmlElement optionNode13 = destDoc.CreateElement("option");
                    optionNode13.SetAttribute("name", "Top Center");
                    optionNode13.SetAttribute("file", "PageNumber_TopCenter_Mirrored.css");
                    optionNode13.SetAttribute("type", "Mirrored");


                    XmlElement optionNode14 = destDoc.CreateElement("option");
                    optionNode14.SetAttribute("name", "Bottom Inside Margin");
                    optionNode14.SetAttribute("file", "PageNumber_BottomInside.css");
                    optionNode14.SetAttribute("type", "Mirrored");


                    XmlElement optionNode15 = destDoc.CreateElement("option");
                    optionNode15.SetAttribute("name", "Bottom Outside Margin");
                    optionNode15.SetAttribute("file", "PageNumber_BottomOutside.css");
                    optionNode15.SetAttribute("type", "Mirrored");


                    XmlElement optionNode16 = destDoc.CreateElement("option");
                    optionNode16.SetAttribute("name", "Bottom Center");
                    optionNode16.SetAttribute("file", "PageNumber_BottomCenter_Mirrored.css");
                    optionNode16.SetAttribute("type", "Mirrored");

                    //EveryPage

                    XmlElement optionNode1 = destDoc.CreateElement("option");
                    optionNode1.SetAttribute("name", "Top Center");
                    optionNode1.SetAttribute("file", "PageNumber_TopCenter.css");
                    optionNode1.SetAttribute("type", "Every Page");

                    XmlElement optionNode2 = destDoc.CreateElement("option");
                    optionNode2.SetAttribute("name", "Bottom Center");
                    optionNode2.SetAttribute("file", "PageNumber_BottomCenter.css");
                    optionNode2.SetAttribute("type", "Every Page");

                    XmlElement optionNode3 = destDoc.CreateElement("option");
                    optionNode3.SetAttribute("name", "None");
                    optionNode3.SetAttribute("file", "PageNumber_None.css");
                    optionNode3.SetAttribute("type", "Both");

                    newNode.AppendChild(optionNode11);
                    newNode.AppendChild(optionNode12);
                    newNode.AppendChild(optionNode13);
                    newNode.AppendChild(optionNode14);
                    newNode.AppendChild(optionNode15);
                    newNode.AppendChild(optionNode16);

                    newNode.AppendChild(optionNode1);
                    newNode.AppendChild(optionNode2);
                    newNode.AppendChild(optionNode3);

                    featuresNode.ReplaceChild(newNode, referencesNode);
                    //featuresNode.RemoveChild(referencesNode);
                    //featuresNode.AppendChild(newNode);
                }

                if (destSettingsFile.ToLower().Contains("scripture"))
                {
                    const string referencesPathRF = "//stylePick/features/feature[@name=\"Reference Format\"]";
                    const string featuresPathRF = "//stylePick/features";
                    XmlNode referencesNodeRF = root.SelectSingleNode(referencesPathRF);
                    XmlNode featuresNodeRF = root.SelectSingleNode(featuresPathRF);
                    if (featuresNodeRF == null) { return; }
                    if (referencesNodeRF != null)
                    {
                        XmlElement newNode = destDoc.CreateElement("feature");
                        newNode.SetAttribute("name", "Reference Format");

                        XmlElement optionNode11 = destDoc.CreateElement("option");
                        optionNode11.SetAttribute("name", "Genesis 1:1");
                        optionNode11.SetAttribute("file", "Running_Head_Mirrored_Verse.css");
                        optionNode11.SetAttribute("type", "Mirrored");

                        XmlElement optionNode12 = destDoc.CreateElement("option");
                        optionNode12.SetAttribute("name", "Genesis 1");
                        optionNode12.SetAttribute("file", "Running_Head_Mirrored_Chapter.css");
                        optionNode12.SetAttribute("type", "Mirrored");


                        XmlElement optionNode13 = destDoc.CreateElement("option");
                        optionNode13.SetAttribute("name", "Genesis 1-2");
                        optionNode13.SetAttribute("file", "Running_Head_Paged_Chapter.css");
                        optionNode13.SetAttribute("type", "Mirrored");


                        XmlElement optionNode14 = destDoc.CreateElement("option");
                        optionNode14.SetAttribute("name", "Gen 1:1");
                        optionNode14.SetAttribute("file", "Running_Head_Mirrored_Verse_Abbr.css");
                        optionNode14.SetAttribute("type", "Mirrored");


                        XmlElement optionNode15 = destDoc.CreateElement("option");
                        optionNode15.SetAttribute("name", "Gen 1");
                        optionNode15.SetAttribute("file", "Running_Head_Mirrored_Chapter_Abbr.css");
                        optionNode15.SetAttribute("type", "Mirrored");


                        XmlElement optionNode16 = destDoc.CreateElement("option");
                        optionNode16.SetAttribute("name", "Gen 1-2");
                        optionNode16.SetAttribute("file", "Running_Head_Paged_Chapter_Abbr.css");
                        optionNode16.SetAttribute("type", "Mirrored");

                        //EveryPage

                        XmlElement optionNode1 = destDoc.CreateElement("option");
                        optionNode1.SetAttribute("name", "Genesis 1:1-2:1");
                        optionNode1.SetAttribute("file", "Running_Head_Paged_Verse.css");
                        optionNode1.SetAttribute("type", "Every Page");

                        XmlElement optionNode2 = destDoc.CreateElement("option");
                        optionNode2.SetAttribute("name", "Gen 1:1-2:1");
                        optionNode2.SetAttribute("file", "Running_Head_Paged_Verse_Abbr.css");
                        optionNode2.SetAttribute("type", "Every Page");

                        newNode.AppendChild(optionNode11);
                        newNode.AppendChild(optionNode12);
                        newNode.AppendChild(optionNode13);
                        newNode.AppendChild(optionNode14);
                        newNode.AppendChild(optionNode15);
                        newNode.AppendChild(optionNode16);

                        newNode.AppendChild(optionNode1);
                        newNode.AppendChild(optionNode2);

                        featuresNode.ReplaceChild(newNode, referencesNodeRF);
                    }
                    else
                    {
                        XmlElement newNode = destDoc.CreateElement("feature");
                        newNode.SetAttribute("name", "Reference Format");

                        XmlElement optionNode11 = destDoc.CreateElement("option");
                        optionNode11.SetAttribute("name", "Genesis 1:1");
                        optionNode11.SetAttribute("file", "Running_Head_Mirrored_Verse.css");
                        optionNode11.SetAttribute("type", "Mirrored");

                        XmlElement optionNode12 = destDoc.CreateElement("option");
                        optionNode12.SetAttribute("name", "Genesis 1");
                        optionNode12.SetAttribute("file", "Running_Head_Mirrored_Chapter.css");
                        optionNode12.SetAttribute("type", "Mirrored");


                        XmlElement optionNode13 = destDoc.CreateElement("option");
                        optionNode13.SetAttribute("name", "Genesis 1-2");
                        optionNode13.SetAttribute("file", "Running_Head_Paged_Chapter.css");
                        optionNode13.SetAttribute("type", "Mirrored");


                        XmlElement optionNode14 = destDoc.CreateElement("option");
                        optionNode14.SetAttribute("name", "Gen 1:1");
                        optionNode14.SetAttribute("file", "Running_Head_Mirrored_Verse_Abbr.css");
                        optionNode14.SetAttribute("type", "Mirrored");


                        XmlElement optionNode15 = destDoc.CreateElement("option");
                        optionNode15.SetAttribute("name", "Gen 1");
                        optionNode15.SetAttribute("file", "Running_Head_Mirrored_Chapter_Abbr.css");
                        optionNode15.SetAttribute("type", "Mirrored");


                        XmlElement optionNode16 = destDoc.CreateElement("option");
                        optionNode16.SetAttribute("name", "Gen 1-2");
                        optionNode16.SetAttribute("file", "Running_Head_Paged_Chapter_Abbr.css");
                        optionNode16.SetAttribute("type", "Mirrored");

                        //EveryPage

                        XmlElement optionNode1 = destDoc.CreateElement("option");
                        optionNode1.SetAttribute("name", "Genesis 1:1-2:1");
                        optionNode1.SetAttribute("file", "Running_Head_Paged_Verse.css");
                        optionNode1.SetAttribute("type", "Every Page");

                        XmlElement optionNode2 = destDoc.CreateElement("option");
                        optionNode2.SetAttribute("name", "Gen 1:1-2:1");
                        optionNode2.SetAttribute("file", "Running_Head_Paged_Verse_Abbr.css");
                        optionNode2.SetAttribute("type", "Every Page");

                        newNode.AppendChild(optionNode11);
                        newNode.AppendChild(optionNode12);
                        newNode.AppendChild(optionNode13);
                        newNode.AppendChild(optionNode14);
                        newNode.AppendChild(optionNode15);
                        newNode.AppendChild(optionNode16);

                        newNode.AppendChild(optionNode1);
                        newNode.AppendChild(optionNode2);

                        featuresNode.AppendChild(newNode);
                    }
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
        /// To insert the xml default tag part into the alluser's XML file.
        /// </summary>
        /// <param name="xdoc">XMLDocument object</param>
        /// <returns>XmlDocumentFragment to attach in XMLFile</returns>
        private static XmlDocumentFragment InsertDefaultTagVersion17(XmlDocument xdoc)
        {
            const string toInsert = @"
            <styleProperty name=""weburl"" value="""" />
            <styleProperty name=""webadminusrnme"" value="""" />
            <styleProperty name=""webadminpwd"" value="""" />
            <styleProperty name=""webadminsiteNme"" value="""" />
            <styleProperty name=""webemailid"" value="""" />
            <styleProperty name=""webftpfldrnme"" value="""" />
           ";
            XmlDocumentFragment docFrag = xdoc.CreateDocumentFragment();
            docFrag.InnerXml = toInsert;
            return docFrag;
        }

        /// <summary>
        /// To insert the xml default tag part into the alluser's XML file.
        /// </summary>
        /// <param name="xdoc">XMLDocument object</param>
        /// <returns>XmlDocumentFragment to attach in XMLFile</returns>
        private static XmlDocumentFragment InsertDefaultTagVersion18(XmlDocument xdoc)
        {
            const string toInsert = @"<PreviousProjectName></PreviousProjectName>";
            XmlDocumentFragment docFrag = xdoc.CreateDocumentFragment();
            docFrag.InnerXml = toInsert;
            return docFrag;
        }

        /// <summary>
        /// To insert the xml default tag part into the alluser's XML file.
        /// </summary>
        /// <param name="xdoc">XMLDocument object</param>
        /// <returns>XmlDocumentFragment to attach in XMLFile</returns>
        private static XmlDocumentFragment InsertDefaultTagVersion19(XmlDocument xdoc)
        {
            const string toInsert = "<styleProperty name=\"IncludeImage\" value=\"Yes\" />";
            XmlDocumentFragment docFrag = xdoc.CreateDocumentFragment();
            docFrag.InnerXml = toInsert;
            return docFrag;
        }

        /// <summary>
        /// To insert the xml default tag part into the alluser's XML file.
        /// </summary>
        /// <param name="xdoc">XMLDocument object</param>
        /// <returns>XmlDocumentFragment to attach in XMLFile</returns>
        private static XmlDocumentFragment InsertDefaultTagVersion20(XmlDocument xdoc)
        {
            const string toInsert = @"
            <styleProperty name=""Language"" value=""English"" />            
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