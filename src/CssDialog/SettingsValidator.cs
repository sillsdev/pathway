// --------------------------------------------------------------------------------------------
// <copyright file="SettingsValidator.cs" from='2009' to='2014' company='SIL International'>
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
// Validation of settings file
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using L10NSharp;
using SilTools;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class SettingsValidator
    {
        #region Private Variables
        private bool isProcessSucess = true;
        private string errorTag = "";
        private string allUserSettingsPath = string.Empty;
        private readonly XmlDocument settingsDoc = new XmlDocument();
        private readonly XmlDocument settingsDictDoc = new XmlDocument();
        private readonly XmlDocument settingsScriDoc = new XmlDocument();
        private XmlNode settingsPNode;
        private XmlNode settingsDictPNode;
        private XmlNode settingsScriPNode;
        private static readonly Dictionary<string, string> replaceString = new Dictionary<string, string>();
        private readonly ArrayList boolValue = new ArrayList();
        private readonly ArrayList mediaList = new ArrayList();
        private enum FileName
        {
            StyleSettings, DictionaryStyleSettings, ScriptureStyleSettings
        }
        private string fileNamewithPath = string.Empty;
        private static bool _fromPlugin;
        private string currentValidatingFileName = string.Empty;
        #endregion

        #region Constructor
        public SettingsValidator()
        {
            replaceString["%(AppData)s"] = Common.GetAllUserAppPath();
            replaceString["%(Documents)s"] = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            replaceString["$(Base)s"] = "Publications";
            replaceString["%(CurrentProject)s"] = "sena3";
            replaceString["$(StyleSheet)s_$(DateTime)s"] = "GPS11L11" + "_" + DateTime.Now.ToString("yyyy-MM-dd_hhmmss");

            boolValue.Add("True");
            boolValue.Add("False");

            mediaList.Add("paper");
            mediaList.Add("mobile");
            mediaList.Add("web");
            mediaList.Add("others");
        }
        #endregion

        /// <summary>
        /// Method to validate the each tag in the StyleSettings.xml, DictionaryStyleSettings.xml and
        /// ScriptureStyleSettings contains valid data. It will replace to new file when the tag value is
        /// incorrect. Before that it will get conformation from the user.
        /// </summary>
        /// <param name="settingsFilewithPath">Filepath of StyleSettings.xml file</param>
        /// <param name="fromPlugin">Validation from FLEX plugin / Application</param>
        /// <returns>true = contains valid settings / false = close the application</returns>
        public bool ValidateSettingsFile(string settingsFilewithPath, bool fromPlugin)
        {
            _fromPlugin = fromPlugin;
            string fileName = Path.GetFileName(settingsFilewithPath);
            allUserSettingsPath = Common.PathCombine(Common.GetAllUserPath(), fileName);

            if (File.Exists(allUserSettingsPath)) ProcessSettingsFile(allUserSettingsPath);
            string inputtype = GetInputType(allUserSettingsPath);
            string allUsersPathWithoutFileName = Common.PathCombine(Common.GetAllUserPath(), inputtype);

            if (inputtype == Common.ProjectType.Dictionary.ToString())
            {
                fileNamewithPath = Common.PathCombine(allUsersPathWithoutFileName, FileName.DictionaryStyleSettings + ".xml");
                if (File.Exists(fileNamewithPath)) ProcessDictionarySettingFile(fileNamewithPath, inputtype);
            }
            else
            {
                fileNamewithPath = Common.PathCombine(allUsersPathWithoutFileName, FileName.ScriptureStyleSettings + ".xml");
                if (File.Exists(fileNamewithPath)) ProcessScriptureSettingFile(fileNamewithPath, inputtype);
            }
            return isProcessSucess;
        }

        protected static string GetInputType(string SettingsPath)
        {
            if (!File.Exists(SettingsPath)) return "Dictionary";
            XmlNode result = Common.GetXmlNode(SettingsPath, "//settings/property[@name='InputType']");
            return result.Attributes["value"].Value;
        }

        /// <summary>
        /// Method to process ScriptuteStyleSettings.xml file, It Process all validations.
        /// </summary>
        /// <param name="fileNamewithPath">Settings filepath</param>
        /// <param name="inputtype">scripture</param>
        protected void ProcessScriptureSettingFile(string fileNamewithPath, string inputtype)
        {
            settingsScriDoc.Load(fileNamewithPath);
            settingsScriPNode = settingsScriDoc.DocumentElement;
            bool isValidScriSettings = ProcessValidator(settingsScriPNode, true);
            if (!isValidScriSettings)
            {
                CopySettingsFile(FileName.ScriptureStyleSettings.ToString(), inputtype, fileNamewithPath);
            }
        }

        /// <summary>
        /// Method to process DictionaryStyleSettings.xml file, It Process all validations.
        /// </summary>
        /// <param name="fileNamewithPath">Settings filepath</param>
        /// <param name="inputtype">Dictionary</param>
        protected void ProcessDictionarySettingFile(string fileNamewithPath, string inputtype)
        {
            settingsDictDoc.Load(fileNamewithPath);
            settingsDictPNode = settingsDictDoc.DocumentElement;
            bool isValidDictSettings = ProcessValidator(settingsDictPNode, true);
            if (!isValidDictSettings)
            {
                CopySettingsFile(FileName.DictionaryStyleSettings.ToString(), inputtype, fileNamewithPath);
            }
        }

        /// <summary>
        /// Add the childnode to the settings file.
        /// </summary>
        /// <param name="settingsXML"></param>
        /// <param name="childNode"></param>
        /// <param name="node"></param>
        private static void AppendChildNode(XmlDocument settingsXML, XmlNode childNode, XmlNode node)
        {
            string toInsert = node.OuterXml;
            XmlDocumentFragment docFrag = settingsXML.CreateDocumentFragment();
            docFrag.InnerXml = toInsert;
            childNode.AppendChild(docFrag);
        }

        protected void ProcessSettingsFile(string fileNamewithPath)
        {
            currentValidatingFileName = fileNamewithPath;
            settingsDoc.Load(fileNamewithPath);
            settingsPNode = settingsDoc.DocumentElement;
            bool isValidSettings = ProcessValidator(settingsPNode, false);
            if (!isValidSettings)
            {
                CopySettingsFile(FileName.StyleSettings.ToString(), "", fileNamewithPath);
            }
        }


        protected void CopySettingsFile(string fileName, string supportPath, string filePath)
        {
            var msg = String.Empty;
            if (errorTag.IndexOf("|") > 0)
            {
                string[] errMessage = errorTag.Split('|');
                msg = LocalizationManager.GetString("SettingsValidator.CopySettingsFile.Message1",
                    "Settings file  \"{0}\".xml" + " is invalid. Do you want to overwrite it with the setting file previously installed? \r\n (Specifically, \"{1}\" property has an invalid path.)", "");
                msg = String.Format(msg, filePath, errMessage[0]);
            }
            else
            {
                msg = LocalizationManager.GetString("SettingsValidator.CopySettingsFile.Message2",
                   "Settings file  \"{0}\".xml" + " is invalid. Do you want to overwrite it with the setting file previously installed? \r\n (Specifically, \"{1}\" property has an invalid value.)", "");
                msg = String.Format(msg, filePath, errorTag);
            }

            DialogResult result;
            if (!Common.Testing)
            {
				string caption = LocalizationManager.GetString("CopySettingsFile.MessageBoxCaption.Caption", "Information", "");
				result = Utils.MsgBox(msg, caption,
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else
            {
                result = DialogResult.Yes;
            }

            if (result == DialogResult.Yes)
            {
                string programFolder = Path.GetDirectoryName(Param.SettingPath);
                string allUsersFolder = Path.GetDirectoryName(allUserSettingsPath);
                if (supportPath.Length > 0 && allUsersFolder.IndexOf(supportPath) == -1)
                {
                    allUsersFolder = Common.PathCombine(allUsersFolder, supportPath);
                }
                string programPath = Common.PathCombine(programFolder, fileName + ".xml");
                Common.MigrateCustomSheet(Common.PathCombine(allUsersFolder, fileName + ".xml"), programPath);
                isProcessSucess = true;
            }
            else
            {
                isProcessSucess = false;
            }
        }


        protected bool ProcessValidator(XmlNode parentNode, bool addionalValidation)
        {
            if (!ValidateOutputPath(parentNode)) return false;
            if (!ValidateUserSheetPath(parentNode)) return false;
            if (!ValidatePublicationLocation(parentNode)) return false;
            if (!ValidateMasterSheetFolder(parentNode)) return false;
            if (!ValidateIconPath(parentNode)) return false;
            if (!ValidateSamplePath(parentNode)) return false;
            if (!ValidateDefaultIcon(parentNode)) return false;
            if (!ValidateLastTask(parentNode)) return false;
            // edb 9/6/2011 (mono) -- CssEditor contains a Windows path. The editor looks like it only gets called from
            // ModifyOptions, which looks like a PublishingSolutions dialog (at least, I can't see any place to
            // get to it from the ConfigurationTool or ExportThroughPathway dialog). Removing the validation for now.
            if (!ValidateSelectedIcon(parentNode)) return false;
            if (!ValidateMisingIcon(parentNode)) return false;
            if (!ValidateConfigureDictionary(parentNode)) return false;
            if (!ValidateReversalIndexes(parentNode)) return false;
            if (!ValidateGrammerSketch(parentNode)) return false;
            if (!ValidateExtraProcessing(parentNode)) return false;
            if (!ValidateLayoutSelected(parentNode)) return false;
            if (!ValidateMedia(parentNode)) return false;
            if (!ValidateDefaultConfigureDictionary(parentNode)) return false;
            if (!ValidateDefaultReversalIndexes(parentNode)) return false;
            if (!ValidateDefaultGrammerSketch(parentNode)) return false;
            if (!ValidateDefaultExtraProcessing(parentNode)) return false;
            if (!ValidateDefaultLayoutSelected(parentNode)) return false;
            if (!ValidateDefaultMedia(parentNode)) return false;
            if (!ValidateFeatureName(parentNode)) return false;
            if (!ValidateFeatureOptionName(parentNode)) return false;
            if (!ValidateStyleName(parentNode)) return false;
            if (!ValidateStyleFile(parentNode)) return false;
            if (!ValidateTaskName(parentNode)) return false;
            if (!ValidateTaskStyle(parentNode)) return false;
            if (!ValidateTaskIcon(parentNode)) return false;
            if (addionalValidation)
            {
                if (!ValidateBaseStyles(parentNode)) return false;
                if (!ValidateColumnWidthColumnName(parentNode)) return false;
                if (!ValidateColumnWidthColumnWidth(parentNode)) return false;
            }
            return true;
        }

        //OutputPath is valid and editable
        protected bool ValidateOutputPath(XmlNode parentNode)
        {
            bool isWritePermission = false;
            try
            {
                const string methodname = "OutputPath";
                string outputPath = GetOutputPath(parentNode);
                isWritePermission = IsDirectoryWritable(outputPath);
                if (!isWritePermission)
                {
                    errorTag = methodname + "|" + outputPath;
                }
            }
            catch { }
            return isWritePermission;
        }


        protected static bool IsDirectoryWritable(string outputPath)
        {
            bool isWritePermission = true;
            try
            {
                if (!Directory.Exists(outputPath))
                {
                    isWritePermission = false;
                }
            }
            catch { }
            return isWritePermission;
        }


        protected static string GetOutputPath(XmlNode parentNode)
        {
            const string xPath = "//property[@name=\"OutputPath\"]";
            XmlNode childNode = parentNode.SelectSingleNode(xPath);
            string path = childNode.Attributes["value"].Value.Replace("%(AppData)s/", "");
            return Common.PathCombine(replaceString["%(AppData)s"], Common.DirectoryPathReplace(path));
        }

        //UserSheetPath is valid and editable
        protected bool ValidateUserSheetPath(XmlNode parentNode)
        {
            bool isWritePermission = false;
            try
            {
                const string methodname = "UserSheetPath";
                const string xPath = "//stylePick/settings/property[@name=\"UserSheetPath\"]";
                XmlNode childNode = parentNode.SelectSingleNode(xPath);
                string path = childNode.Attributes["value"].Value.Replace("%(AppData)s/", "");
                string appPath = Common.PathCombine(replaceString["%(AppData)s"], Common.DirectoryPathReplace(path));
                isWritePermission = IsDirectoryWritable(appPath);
                if (!isWritePermission)
                {
                    errorTag = methodname + "|" + appPath;
                }
            }
            catch { }
            return isWritePermission;

        }

        //PublicationLocation is valid and editable
        protected bool ValidatePublicationLocation(XmlNode parentNode)
        {
            bool isWritePermission = false;
            try
            {
                const string methodname = "PublicationLocation";
                string defaultPath = "%(Documents)s\\Publications";
                defaultPath = defaultPath.Replace("%(Documents)s", replaceString["%(Documents)s"]);

                string outputPath = GetOutputPath(parentNode);
                isWritePermission = IsDirectoryWritable(outputPath);
                if (!isWritePermission)
                {
                    try
                    {
                        Directory.CreateDirectory(defaultPath);
                        isWritePermission = true;
                    }
                    catch (Exception)
                    {
                        errorTag = methodname + "|" + defaultPath;
                        isWritePermission = false;
                    }
                }
            }
            catch { }
            return isWritePermission;
        }

        //MasterSheetFolder exists in From program folder
        protected bool ValidateMasterSheetFolder(XmlNode parentNode)
        {
            try
            {
                const string methodname = "MasterSheetPath";
                const string xPath = "//stylePick/settings/property[@name=\"MasterSheetPath\"]";
                XmlNode childNode = parentNode.SelectSingleNode(xPath);
                string appPath = Common.GetApplicationPath();
                if (_fromPlugin)
                {
					appPath = PathwayPath.GetSupportPath(appPath, Common.SupportFolder, false);
                }
	            string path = PathwayPath.GetSupportPath(appPath, childNode.Attributes["value"].Value, false);

                if (!Directory.Exists(path))
                {
                    errorTag = methodname + "|" + path;
                    return false;
                }
            }
            catch { }
            return true;
        }

        //IconPath exists in From program folder
        protected bool ValidateIconPath(XmlNode parentNode)
        {
            try
            {
                const string methodname = "IconPath";
                const string xPath = "//stylePick/settings/property[@name=\"IconPath\"]";
                XmlNode childNode = parentNode.SelectSingleNode(xPath);
                string appPath = Common.GetApplicationPath();
                if (_fromPlugin)
                {
					appPath = PathwayPath.GetSupportPath(appPath, Common.SupportFolder, false);
                }
				string path = PathwayPath.GetSupportPath(appPath, childNode.Attributes["value"].Value, false);
                if (!Directory.Exists(path))
                {
                    errorTag = methodname + "|" + path;
                    return false;
                }
            }
            catch { }
            return true;
        }

        //SamplePath exists in From program folder
        protected bool ValidateSamplePath(XmlNode parentNode)
        {
            try
            {
                const string methodname = "SamplePath";
                const string xPath = "//stylePick/settings/property[@name=\"SamplePath\"]";
                XmlNode childNode = parentNode.SelectSingleNode(xPath);
                string appPath = Common.GetApplicationPath();
                if (_fromPlugin)
                {
					appPath = PathwayPath.GetSupportPath(appPath, Common.SupportFolder, false);
                }
				string path = PathwayPath.GetSupportPath(appPath, childNode.Attributes["value"].Value, false);

				if (!Directory.Exists(path))
                {
                    errorTag = methodname + "|" + path;
                    return false;
                }
            }
            catch { }
            return true;
        }

        //BaseStyles file exists in the MasterSheetPath folder
        protected bool ValidateBaseStyles(XmlNode parentNode)
        {
            try
            {
                const string methodname = "BaseStyles";
                const string xPathMaster = "//stylePick/settings/property[@name=\"MasterSheetPath\"]";
                const string xPathStyles = "//stylePick/settings/property[@name=\"BaseStyles\"]";
                XmlNode childNodeMaster = parentNode.SelectSingleNode(xPathMaster);
                XmlNode childNodeStyles = parentNode.SelectSingleNode(xPathStyles);
                string partialPath = Common.PathCombine(childNodeMaster.Attributes["value"].Value,
                                                        childNodeStyles.Attributes["value"].Value);
                string appPath = Common.GetApplicationPath();
                if (_fromPlugin)
                {
					appPath = PathwayPath.GetSupportPath(appPath, Common.SupportFolder, false);
                }
				string path = PathwayPath.GetSupportPath(appPath, partialPath, false);
                if (!File.Exists(path))
                {
                    errorTag = methodname + "|" + path;
                    return false;
                }
            }
            catch { }
            return true;
        }

        //DefaultIcon exists in the IconPath folder
        protected bool ValidateDefaultIcon(XmlNode parentNode)
        {
            try
            {
                const string methodname = "DefaultIcon";
                const string xPath = "//stylePick/settings/property[@name=\"DefaultIcon\"]";
                XmlNode childNode = parentNode.SelectSingleNode(xPath);
                string appPath = Common.GetApplicationPath();
                if (_fromPlugin)
                {
					appPath = PathwayPath.GetSupportPath(appPath, Common.SupportFolder, false);
                }
				string path = PathwayPath.GetSupportPath(appPath, childNode.Attributes["value"].Value, false);
                if (!File.Exists(path))
                {
                    errorTag = methodname + "|" + path;
                    return false;
                }
            }
            catch { }
            return true;
        }

        //LastTask contains the name of a task among the tasks/task elements
        protected bool ValidateLastTask(XmlNode parentNode)
        {
            try
            {
                const string methodname = "LastTask";
                const string xPathTasks = "//stylePick/tasks/task";
                const string xPathLastTask = "//stylePick/settings/property[@name=\"LastTask\"]";
                XmlNodeList childNodeTasks = parentNode.SelectNodes(xPathTasks);
                XmlNode childNodeLastTask = parentNode.SelectSingleNode(xPathLastTask);
                if (childNodeTasks != null)
                {
                    foreach (XmlNode cNode in childNodeTasks)
                    {
                        if (cNode.Attributes["name"].Value.ToLower() == childNodeLastTask.Attributes["value"].Value.ToLower())
                        {
                            return true;
                        }
                    }
                    errorTag = methodname;
                    return false;
                }
            }
            catch { }
            return true;
        }

        //SelectedIcon exists in the program folder
        protected bool ValidateSelectedIcon(XmlNode parentNode)
        {
            try
            {
                const string methodname = "SelectedIcon";
                const string xPath = "//stylePick/settings/property[@name=\"SelectedIcon\"]";
                XmlNode childNode = parentNode.SelectSingleNode(xPath);
                string appPath = Common.GetApplicationPath();
                if (_fromPlugin)
                {
					appPath = PathwayPath.GetSupportPath(appPath, Common.SupportFolder, false);
                }
				string path = PathwayPath.GetSupportPath(appPath, childNode.Attributes["value"].Value, false);
                if (!File.Exists(path))
                {
                    errorTag = methodname + "|" + path;
                    return false;
                }
            }
            catch { }
            return true;
        }

        //MisingIcon exists in the program folder
        protected bool ValidateMisingIcon(XmlNode parentNode)
        {
            try
            {
                const string methodname = "MisingIcon";
                const string xPath = "//stylePick/settings/property[@name=\"MisingIcon\"]";
                XmlNode childNode = parentNode.SelectSingleNode(xPath);
                if (childNode != null)
                {
                    string path = Common.PathCombine(Common.GetApplicationPath(), childNode.Attributes["value"].Value);
                    if (!File.Exists(path))
                    {
                        errorTag = methodname + "|" + path;
                        return false;
                    }
                }
            }
            catch { }
            return true;
        }

        //ConfigureDictionary setting is True or False
        protected bool ValidateConfigureDictionary(XmlNode parentNode)
        {
            try
            {
                const string methodname = "ConfigureDictionary";
                const string xPath = "//stylePick/settings/property[@name=\"ConfigureDictionary\"]";
                XmlNode childNode = parentNode.SelectSingleNode(xPath);

                if (childNode == null && fileNamewithPath.ToLower().IndexOf("dictionary") > 0)
                    return false;

                if (childNode != null)
                {
                    string result = childNode.Attributes["value"].Value;

                    if (!boolValue.Contains(result))
                    {
                        errorTag = methodname;
                        return false;
                    }
                }
            }
            catch { }
            return true;
        }

        //ReversalIndexes setting is True or False
        protected bool ValidateReversalIndexes(XmlNode parentNode)
        {
            try
            {
                const string methodname = "ReversalIndexes";
                const string xPath = "//stylePick/settings/property[@name=\"ReversalIndexes\"]";
                XmlNode childNode = parentNode.SelectSingleNode(xPath);
                if (childNode != null && childNode.Attributes != null)
                {
                    string result = childNode.Attributes["value"].Value;
                    if (!boolValue.Contains(result))
                    {
                        errorTag = methodname;
                        return false;
                    }
                }
            }
            catch { }
            return true;
        }

        //GrammarSketch setting is True or False
        protected bool ValidateGrammerSketch(XmlNode parentNode)
        {
            try
            {
                const string methodname = "GrammerSketch";
                const string xPath = "//stylePick/settings/property[@name=\'GrammerSketch\']";
                XmlNode childNode = parentNode.SelectSingleNode(xPath);
                if (childNode != null && childNode.Attributes != null)
                {
                    string result = childNode.Attributes["value"].Value;
                    if (!boolValue.Contains(result))
                    {
                        errorTag = methodname;
                        return false;
                    }
                }
            }
            catch { }
            return true;
        }

        //ExtraProcessing setting is True or False
        protected bool ValidateExtraProcessing(XmlNode parentNode)
        {
            try
            {
                const string methodname = "ExtraProcessing";
                const string xPath = "//stylePick/settings/property[@name=\"ExtraProcessing\"]";
                XmlNode childNode = parentNode.SelectSingleNode(xPath);
                if (childNode != null && childNode.Attributes != null)
                {
                    string result = childNode.Attributes["value"].Value;
                    if (!boolValue.Contains(result))
                    {
                        errorTag = methodname;
                        return false;
                    }
                }
            }
            catch { }
            return true;
        }

        //LayoutSelected setting contains the name of a style sheet in //styles/*/style
        protected bool ValidateLayoutSelected(XmlNode parentNode)
        {
            try
            {
                const string methodname = "LayoutSelected";
                const string xPathStyles = "//stylePick/styles/*/style";
                const string xPathSelectedStyle = "//stylePick/settings/property[@name=\"LayoutSelected\"]";
                XmlNodeList childNodeTasks = parentNode.SelectNodes(xPathStyles);
                XmlNode childNodeLastTask = parentNode.SelectSingleNode(xPathSelectedStyle);
                if (childNodeTasks != null && childNodeLastTask != null)
                {
                    foreach (XmlNode cNode in childNodeTasks)
                    {
                        if (cNode.Attributes["name"].Value == childNodeLastTask.Attributes["value"].Value)
                        {
                            return true;
                        }
                    }
                    errorTag = methodname;
                    return false;
                }
            }
            catch { }
            return true;
        }

        //Media setting should be paper, mobile, or web
        protected bool ValidateMedia(XmlNode parentNode)
        {
            try
            {
                const string methodname = "Media";
                const string xPath = "//stylePick/settings/property[@name=\"Media\"]";
                XmlNode childNode = parentNode.SelectSingleNode(xPath);
                if (childNode != null)
                {
                    string result = childNode.Attributes["value"].Value;
                    if (!mediaList.Contains(result))
                    {
                        errorTag = methodname;
                        return false;
                    }
                }
            }
            catch { }
            return true;
        }

        //ConfigureDictionary default setting is True or False
        protected bool ValidateDefaultConfigureDictionary(XmlNode parentNode)
        {
            try
            {
                const string methodname = "DefaultConfigureDictionary";
                const string xPath = "//stylePick/defaultSettings/property[@name=\"ConfigureDictionary\"]";
                XmlNode childNode = parentNode.SelectSingleNode(xPath);
                if (childNode != null && childNode.Attributes != null)
                {
                    string result = childNode.Attributes["value"].Value;
                    if (!boolValue.Contains(result))
                    {
                        errorTag = methodname;
                        return false;
                    }
                }
            }
            catch { }
            return true;
        }

        //ReversalIndexes default setting is True or False
        protected bool ValidateDefaultReversalIndexes(XmlNode parentNode)
        {
            try
            {
                const string methodname = "DefaultReversalIndexes";
                const string xPath = "//stylePick/defaultSettings/property[@name=\"ReversalIndexes\"]";
                XmlNode childNode = parentNode.SelectSingleNode(xPath);
                if (childNode != null && childNode.Attributes != null)
                {
                    string result = childNode.Attributes["value"].Value;
                    if (!boolValue.Contains(result))
                    {
                        errorTag = methodname;
                        return false;
                    }
                }
            }
            catch { }
            return true;
        }

        //GrammarSketch default setting is True or False
        protected bool ValidateDefaultGrammerSketch(XmlNode parentNode)
        {
            try
            {
                const string methodname = "DefaultGrammerSketch";
                const string xPath = "//stylePick/defaultSettings/property[@name=\"GrammerSketch\"]";
                XmlNode childNode = parentNode.SelectSingleNode(xPath);
                if (childNode != null && childNode.Attributes != null)
                {
                    string result = childNode.Attributes["value"].Value;
                    if (!boolValue.Contains(result))
                    {
                        errorTag = methodname;
                        return false;
                    }
                }
            }
            catch { }
            return true;
        }

        //ExtraProcessing default setting is True or False
        protected bool ValidateDefaultExtraProcessing(XmlNode parentNode)
        {
            try
            {
                const string methodname = "DefaultExtraProcessing";
                const string xPath = "//stylePick/defaultSettings/property[@name=\"ExtraProcessing\"]";
                XmlNode childNode = parentNode.SelectSingleNode(xPath);
                if (childNode != null && childNode.Attributes != null)
                {
                    string result = childNode.Attributes["value"].Value;
                    if (!boolValue.Contains(result))
                    {
                        errorTag = methodname;
                        return false;
                    }
                }
            }
            catch { }
            return true;
        }

        //LayoutSelected default setting contains the name of a style sheet in //styles/*/style
        protected bool ValidateDefaultLayoutSelected(XmlNode parentNode)
        {
            try
            {
                const string methodname = "DefaultLayoutSelected";
                const string xPathStyles = "//stylePick/styles/*/style";
                const string xPathSelectedStyle = "//stylePick/defaultSettings/property[@name=\"LayoutSelected\"]";
                XmlNodeList childNodeTasks = parentNode.SelectNodes(xPathStyles);
                XmlNode childNodeLastTask = parentNode.SelectSingleNode(xPathSelectedStyle);
                if (childNodeTasks != null)
                {
                    foreach (XmlNode cNode in childNodeTasks)
                    {
                        if (cNode.Attributes["name"].Value == childNodeLastTask.Attributes["value"].Value)
                        {
                            return true;
                        }
                    }
                    errorTag = methodname;
                    return false;
                }
            }
            catch { }
            return true;
        }

        //Media default setting should be paper, mobile, or web
        protected bool ValidateDefaultMedia(XmlNode parentNode)
        {
            try
            {
                const string methodname = "Media";
                const string xPath = "//stylePick/defaultSettings/property[@name=\"Media\"]";
                XmlNode childNode = parentNode.SelectSingleNode(xPath);
                if (childNode != null)
                {
                    string result = childNode.Attributes["value"].Value;
                    if (!mediaList.Contains(result))
                    {
                        errorTag = methodname;
                        return false;
                    }
                }
            }
            catch { }
            return true;
        }

        //Each features/feature/@name should be unique
        protected bool ValidateFeatureName(XmlNode parentNode)
        {
            try
            {
                const string methodname = "FeatureName";
                const string xPath = "//stylePick/features/feature";
                XmlNodeList childNode = parentNode.SelectNodes(xPath);
                var styleNameList = new ArrayList();
                if (childNode != null)
                    foreach (XmlNode node in childNode)
                    {
                        string value = node.Attributes["name"].Value;
                        if (styleNameList.Contains(value))
                        {
                            errorTag = methodname;
                            return false;
                        }
                        styleNameList.Add(value);
                    }
            }
            catch { }
            return true;
        }

        //Each features/feature/option/@name should be unique for that option
        protected bool ValidateFeatureOptionName(XmlNode parentNode)
        {
            try
            {
                const string methodname = "FeatureOptionName";
                const string xPath = "//stylePick/features/feature";
                XmlNodeList childNode = parentNode.SelectNodes(xPath);
                if (childNode != null)
                    foreach (XmlNode node in childNode)
                    {
                        if (node.Attributes != null)
                        {
                            string value = node.Attributes["name"].Value;

                            if (value.ToLower() == "reference format") continue;
                            string xPathOption = "//stylePick/features/feature[@name=\"" + value + "\"]/option";
                            XmlNodeList OptionNode = parentNode.SelectNodes(xPathOption);
                            var optionList = new ArrayList();
                            if (OptionNode != null)
                                foreach (XmlNode oNode in OptionNode)
                                {
                                    string oValue = oNode.Attributes["name"].Value;
                                    if (oValue == "Top Center" || oValue == "Bottom Center") continue;
                                    if (optionList.Contains(oValue))
                                    {
                                        errorTag = methodname;
                                        return false;
                                    }
                                    optionList.Add(oValue);
                                }
                        }
                    }
            }
            catch { }
            return true;
        }

        /// <summary>
        /// Each file given in the features/feature/option/@file should exist in the MasterSheetPath
        /// </summary>
        /// <param name="parentNode">Current Parent Node</param>
        /// <returns>return boolean value true or false</returns>
        protected bool ValidateFeatureOptionFile(XmlNode parentNode)
        {
            try
            {
                const string methodname = "FeatureOptionFile";
                const string xPath = "//stylePick/features/feature";
                XmlNodeList childNode = parentNode.SelectNodes(xPath);
                if (childNode != null)
                    foreach (XmlNode node in childNode)
                    {
                        string value = node.Attributes["name"].Value;
                        string xPathOption = "//stylePick/features/feature[@name=\"" + value + "\"]/option";
                        XmlNodeList OptionNode = parentNode.SelectNodes(xPathOption);
                        if (OptionNode != null)
                            foreach (XmlNode oNode in OptionNode)
                            {
                                string oValue = oNode.Attributes["file"].Value;
                                const string xPathMaster = "//stylePick/settings/property[@name=\"MasterSheetPath\"]";
                                XmlNode childNodeMaster = parentNode.SelectSingleNode(xPathMaster);
                                string partialPath = Common.PathCombine(childNodeMaster.Attributes["value"].Value,
                                                                        oValue);
                                string appPath = Common.GetApplicationPath();
                                if (_fromPlugin)
                                {
                                    appPath = Common.PathCombine(appPath, Common.SupportFolder);
                                }
                                string path = Common.PathCombine(appPath, partialPath);
                                if (!File.Exists(path))
                                {
                                    if (!File.Exists(path.Replace("Styles\\Dictionary", "Copyrights")))
                                    {
                                        errorTag = methodname + "|" + path;
                                        return false;
                                    }
                                }
                            }
                    }
            }
            catch { }
            return true;
        }

        //Each styles/*/style/@name should be unique for the media type.
        protected bool ValidateStyleName(XmlNode parentNode)
        {
            try
            {
                const string methodname = "StyleName";
                foreach (string media in mediaList)
                {
                    string xPathMStyles = "//stylePick/styles/" + media + "/style";
                    XmlNodeList childNode = parentNode.SelectNodes(xPathMStyles);
                    if (childNode != null)
                    {
                        var optionList = new ArrayList();
                        foreach (XmlNode node in childNode)
                        {
                            string oValue = node.Attributes["name"].Value;
                            if (optionList.Contains(oValue))
                            {
                                errorTag = methodname;
                                return false;
                            }
                            optionList.Add(oValue);
                        }
                    }
                }
            }
            catch { }
            return true;
        }

        //It will return DictionaryForMids css name
        private string GetNodeString()
        {
            return "dictionaryformids.css";
        }

        //Each styles/*/style/@file should exist on MasterSheetPath or on OutputPath
        protected bool ValidateStyleFile(XmlNode parentNode)
        {
            try
            {
                string masterPath = GetMasterPath(parentNode);
                string outputPath = GetOutputPath(parentNode);
                const string methodname = "StyleFile";
                foreach (string media in mediaList)
                {
                    string xPathMStyles = "//stylePick/styles/" + media + "/style";
                    XmlNodeList childNode = parentNode.SelectNodes(xPathMStyles);
                    if (childNode != null)
                    {
                        foreach (XmlNode node in childNode)
                        {
                            string oValue = node.Attributes["file"].Value;
                            if (oValue.ToLower() == GetNodeString())//Since there is no properties for DictionaryForMids, Validation is not needed
                                return true;
                            string oType = "Standard";
                            if (node.Attributes["type"] != null)
                            {
                                oType = node.Attributes["type"].Value;
                            }
                            string path = Common.PathCombine(masterPath, oValue);

                            if (path.ToLower().IndexOf("dictionary") > 0 && oValue.ToLower() == "gobible.css")
                                return true;

                            if (oType != "Custom" && !File.Exists(path) && !File.Exists(Common.PathCombine(outputPath, oValue)))
                            {
                                errorTag = methodname;
                                return false;
                            }
                        }
                    }
                }
            }
            catch { }
            return true;
        }

        protected static string GetMasterPath(XmlNode parentNode)
        {
            string appPath = Common.GetApplicationPath();
            if (_fromPlugin)
            {
				appPath = PathwayPath.GetSupportPath(appPath, Common.SupportFolder, false);

            }
            const string xPathMaster = "//property[@name=\"MasterSheetPath\"]";
            XmlNode masterNode = parentNode.SelectSingleNode(xPathMaster);
            string masterValue = masterNode.Attributes["value"].Value;
			string fileName = PathwayPath.GetSupportPath(appPath, masterValue, false);
			return fileName;
        }

        //Each tasks/task/@name should be unique
        protected bool ValidateTaskName(XmlNode parentNode)
        {
            try
            {
                const string methodname = "TaskName";
                const string xPathTasks = "//stylePick/tasks/task";
                XmlNodeList childNode = parentNode.SelectNodes(xPathTasks);
                if (childNode != null)
                {
                    var optionList = new ArrayList();
                    foreach (XmlNode node in childNode)
                    {
                        string oValue = node.Attributes["name"].Value;
                        if (optionList.Contains(oValue))
                        {
                            errorTag = methodname;
                            return false;
                        }
                        optionList.Add(oValue);
                    }
                }
            }
            catch { }
            return true;
        }

        //Each tasks/task/@style should exists in styles//style
        protected bool ValidateTaskStyle(XmlNode parentNode)
        {
            try
            {
                const string methodname = "TaskStyle";
                const string xPathTasks = "//stylePick/tasks/task";
                const string xPathPStyles = "//stylePick/styles//style";
                XmlNodeList childNodeStyles = parentNode.SelectNodes(xPathPStyles);
                XmlNodeList childNodeTasks = parentNode.SelectNodes(xPathTasks);
                ArrayList stylesList = GetStylesList(childNodeStyles);
                if (childNodeTasks != null)
                {
                    foreach (XmlNode node in childNodeTasks)
                    {
                        string oValue = node.Attributes["style"].Value;
                        if (!stylesList.Contains(oValue))
                        {
                            errorTag = methodname;
                            return false;
                        }
                    }
                }
            }
            catch { }
            return true;
        }

        /// <summary>
        /// To load the Styles names
        /// </summary>
        /// <param name="childNodeStyles">XmlNode to search</param>
        /// <returns>List of stylenames</returns>
        protected static ArrayList GetStylesList(XmlNodeList childNodeStyles)
        {
            var stylesList = new ArrayList();
            if (childNodeStyles != null)
            {
                foreach (XmlNode node in childNodeStyles)
                {
                    string oValue = node.Attributes["name"].Value;
                    stylesList.Add(oValue);
                }
            }
            return stylesList;
        }

        //Each icon tasks/task/@icon should exist on IconPath
        protected bool ValidateTaskIcon(XmlNode parentNode)
        {
            try
            {
                const string methodname = "TaskIcon";
                const string xPathTasks = "//stylePick/tasks/task";
                XmlNodeList childNode = parentNode.SelectNodes(xPathTasks);
                if (childNode != null)
                {
                    foreach (XmlNode node in childNode)
                    {
                        string oValue = node.Attributes["icon"].Value;
                        string appPath = Common.GetApplicationPath();
                        if (_fromPlugin)
                        {
							appPath = PathwayPath.GetSupportPath(appPath, Common.SupportFolder, false);
                        }
						string iconFilePath = PathwayPath.GetSupportPath(appPath, oValue, false);
                        if (!File.Exists(iconFilePath))
                        {
                            errorTag = methodname + "|" + iconFilePath;
                            return false;
                        }

                    }
                }
            }
            catch { }
            return true;
        }

        //Each column-width/column/@name should evaluate to an integer in sequential order beginning with 0
        protected bool ValidateColumnWidthColumnName(XmlNode parentNode)
        {
            try
            {
                const string methodname = "ColumnWidthColumnName";
                const string xPath = "//stylePick/column-width/column";
                XmlNodeList childNode = parentNode.SelectNodes(xPath);
                if (childNode != null)
                {
                    int i = 0;
                    foreach (XmlNode node in childNode)
                    {
                        string name = node.Attributes["name"].Value;
                        if (!Common.ValidateInteger(name) || name != i.ToString())
                        {
                            errorTag = methodname;
                            return false;
                        }
                        i++;
                    }
                }
            }
            catch { }
            return true;
        }

        //Each column-width/column/@width should be an integer
        protected bool ValidateColumnWidthColumnWidth(XmlNode parentNode)
        {
            try
            {
                const string methodname = "ColumnWidthColumnWidth";
                const string xPath = "//stylePick/column-width/column";
                XmlNodeList childNode = parentNode.SelectNodes(xPath);
                if (childNode != null)
                    foreach (XmlNode node in childNode)
                    {
                        string value = node.Attributes["width"].Value;
                        if (!Common.ValidateInteger(value))
                        {
                            errorTag = methodname;
                            return false;
                        }
                    }
            }
            catch { }
            return true;
        }
    }
}
