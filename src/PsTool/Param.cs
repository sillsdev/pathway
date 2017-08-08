// --------------------------------------------------------------------------------------------
// <copyright file="Param.cs" from='2009' to='2014' company='SIL International'>
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;
using L10NSharp;
using SIL.PublishingSolution;

namespace SIL.Tool
{
    public class InvalidStyleSettingsException : Exception
    {
        public string FullFilePath { get; set; }
        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (FullFilePath == null)
            {
                return "No filepath set for settings file.";
            }
            sb.Append(FullFilePath);
            sb.AppendLine(" is not a valid settings file.");
            if (ErrorMessage != null)
            {
                sb.AppendLine("Validation error details:");
                sb.AppendLine(ErrorMessage);
            }
            return sb.ToString();
        }
    }

    public class Param
	{
		// Settings
        public const string InputPath = "InputPath";
        public const string OutputPath = "OutputPath";
        private const string UserSheetPath = "UserSheetPath";
        public const string PublicationLocation = "PublicationLocation";
        public const string MasterSheetPath = "MasterSheetPath";
        private const string IconPath = "IconPath";
        public const string SamplePath = "SamplePath";
        public const string DefaultIcon = "DefaultIcon";
        public const string CurrentInput = "CurrentInput";
        public const string InputType = "InputType";
        public const string LastTask = "LastTask";
        public const string PreviewType = "PreviewType";
        public const string SelectedIcon = "SelectedIcon";
        public const string MissingIcon = "MissingIcon";
        public const string Help = "Help";
        // Default Settings
        public const string PrintVia = "PrintVia";
        public const string ConfigureDictionary = "ConfigureDictionary";
        public const string ReversalIndex = "ReversalIndexes";
        public const string Preprocessing = "Preprocessing";
        public const string GrammarSketch = "GrammerSketch";
        public const string LayoutSelected = "LayoutSelected";
        public const string ExtraProcessing = "ExtraProcessing";
        public const string Media = "Media";
        // TD-2344: Publication Information (15 Dublin Core metadata elements: http://dublincore.org/documents/dces/)
        public const string Title = "Title";
        public static string DatabaseName = "DatabaseName";
        public const string Creator = "Creator";
        public const string Publisher = "Publisher";
        public const string Description = "Description";
        public const string CopyrightHolder = "Copyright Holder";
        // the rest are not in the UI, but we'll write out what we can to disk (language, date, etc.)
        public const string Type = "Type";
        public const string Source = "Source";
        public const string Format = "Format";
        public const string Contributor = "Contributor";
        public const string Relation = "Relation";


        public const string Coverage = "Coverage";
        public const string Subject = "Subject";
        public const string Date = "Date";
        public const string Language = "Language";
        public const string Identifier = "Identifier";
        // TD-2344: Front Matter
        public const string CoverPage = "Cover Page";
        public const string CoverPageFilename = "Cover Page Filename";
        public const string CoverPageTitle = "Cover Page Title";
        public const string TitlePage = "Title Page";
        public const string CopyrightPage = "Copyright Page";
        public const string CopyrightPageFilename = "Copyright Page Filename";
        public const string TableOfContents = "Table of Contents";
       
        // Other constants
        private const string DefaultSettingsFileName = "StyleSettings.xml";
        public static string UserRole = "Output User";
        public static string MediaType = "paper";

		#region Public properties
        private static string _loadType = string.Empty;//"Dictionary";
        public static string SettingPath
        {
            get
            {
				return Common.FromRegistry(_loadType + DefaultSettingsFileName);
            }
        }

		public static string PathwaySettingFilePath
		{
			get
			{
				if(File.Exists(Path.Combine(Common.GetAllUserPath(), _loadType + DefaultSettingsFileName)))
				{
					return Path.Combine(Common.GetAllUserPath(), _loadType + DefaultSettingsFileName);
				}
				else
				{
					return Path.Combine(Path.Combine(Common.GetAllUserPath(), _loadType), _loadType + DefaultSettingsFileName);
				}
			}
		}
		

        public static string SettingOutputPath
        {
            get
            {
                Debug.Assert(Value.ContainsKey(OutputPath), "Settings not loaded");
                return Common.PathCombine(Value[OutputPath], _loadType + DefaultSettingsFileName);
            }
        }

        public static string SetLoadType
        {
            set
            {
                _loadType = value;
            }
        }
        public static string UiSettingPath
        {
            get
            {
				return Common.FromRegistry(DefaultSettingsFileName);
            }
        }

        public static Dictionary<string, Dictionary<string, string>> UiLanguageFontSettings
        {
            get { return _uiLanguageFontSettings; }
            set { _uiLanguageFontSettings = value; }
        }

        #endregion Public properties

        public static Dictionary<string, string> Value = new Dictionary<string, string>();
        public static Dictionary<string, string> DefaultValue = new Dictionary<string, string>();
        public static Dictionary<string, string> StyleFile = new Dictionary<string, string>();
        public static Dictionary<string, Dictionary<string, string>> featureList = new Dictionary<string, Dictionary<string, string>>();
        private static Dictionary<string, Dictionary<string, string>> _uiLanguageFontSettings = new Dictionary<string, Dictionary<string, string>>();
        public static readonly XmlDocument xmlMap = Common.DeclareXMLDocument(false);
        private static int _selectedIndex = 1;
        private const int UnSelectedIndex = 2;
        private static string _configureType = string.Empty;
        public static bool IsHyphen = false;
        public static string HyphenLang = string.Empty;
        public static string HyphenFilepath = string.Empty;
        public static bool HyphenEnable = false;
        public static List<string> HyphenationSelectedLanguagelist = new List<string>();

        /// <summary>
        /// Load settings first from program path and then from user path.
        /// </summary>
        public static void LoadSettings()
        {
	        if (LoadValues(PathwaySettingFilePath) == null)
	        {
				if (LoadValues(SettingPath) == null) return;
	        }
            MediaType = GetAttrByName("//categories/category", "Media", "select").ToLower();
            LoadDictionary(StyleFile, "styles//style", "file");
            LoadImageList();
        }

		public static string GetInputType(string settingsPath)
		{
			string projectInputType = string.Empty;
			if (File.Exists(settingsPath))
			{
				var settingsReader = XmlReader.Create(settingsPath);
				var settingsDoc = new XmlDocument();
				settingsDoc.Load(settingsReader);
				settingsReader.Close();
				// ReSharper disable once PossibleNullReferenceException
				projectInputType = settingsDoc.SelectSingleNode("//settings/property[@name='InputType']/@value").InnerText;
				Param.SetLoadType = projectInputType;
			}
			return projectInputType;
		}

        public static string GetCSSFileNameFromLayoutSelected(string settingsPath)
        {
            string projectLayoutSelected = string.Empty;
            string cssFileName = string.Empty;
            if (File.Exists(settingsPath))
            {
                var settingsReader = XmlReader.Create(settingsPath);
                var settingsDoc = new XmlDocument();
                settingsDoc.Load(settingsReader);
                settingsReader.Close();
                // ReSharper disable once PossibleNullReferenceException
                projectLayoutSelected = settingsDoc.SelectSingleNode("//settings/property[@name='LayoutSelected']/@value").InnerText;
                if(!string.IsNullOrEmpty(projectLayoutSelected))
                {
                    cssFileName = settingsDoc.SelectSingleNode(string.Format("//paper/style[@name='{0}']/@file", projectLayoutSelected)).InnerText;
                }
            }
            return cssFileName;
        }

        /// <summary>
        /// Function for renaming PublishingSolutions to Pathway.
        /// </summary>
        private static void ReplacePStoPathWay()
        {
            string publishingSolutionsDirectory = Common.PathCombine(Path.GetDirectoryName(Value[OutputPath]), "PublishingSolutions");
            if (Directory.Exists(publishingSolutionsDirectory))
            {
                string oldPath = publishingSolutionsDirectory;
                string newPath = oldPath.Replace("PublishingSolutions", "Pathway");
                if (Directory.Exists(oldPath))
                {
                    try
                    {
                        Directory.Move(oldPath, newPath);
                    }
                    catch
                    {

                    }
                }
            }
        }

        /// <summary>
        /// Common code to load and validate settings file
        /// </summary>
        /// <param name="path">path name of settings file</param>
        /// <returns></returns>
        public static XmlNode LoadValues(string path)
        {
            // sanity check for non-existent file
            if (!File.Exists(path))
            {
                return null;
            }
            UnLoadValues();

			try
			{
			    var sr = new StreamReader(path);
			    var result = LoadValues(sr.BaseStream);
                sr.Close();
			    return result;
			}
			catch (Exception ex) 
			{
				var sb = new StringBuilder();
				sb.Append("Unable to Load file: ");
				sb.Append(path);
				sb.AppendLine(".");
				sb.AppendLine("The full exception encountered is shown below.");
				sb.AppendLine();
				sb.AppendLine(ex.ToString());
                var msg = LocalizationManager.GetString("Param.LoadValues.Message", sb.ToString(), "");
                MessageBox.Show(msg, "Fatal Error");
				return null;
			}
        }

        public static XmlNode LoadValues(Stream stream)
        {
            var xmlReaderSettings = new XmlReaderSettings {ValidationType = ValidationType.Schema};
            var resolver = new XmlUrlResolver {Credentials = System.Net.CredentialCache.DefaultCredentials};
            xmlReaderSettings.XmlResolver = Common.IsUnixOS() ? resolver : null;
            xmlReaderSettings.ValidationEventHandler += ValidationCallBack;
            _validateXmlSuccess = true;
            var reader = XmlReader.Create(stream, xmlReaderSettings);
            xmlMap.Load(reader);
            reader.Close();
            if (!_validateXmlSuccess)
            {
                throw new InvalidStyleSettingsException { ErrorMessage = _validateXmlError.ToString() };
            }
            XmlNode root = xmlMap.DocumentElement;
            if (root != null)
            {
                LoadDictionary(Value, "settings/property", "value");
                LoadDictionary(DefaultValue, "defaultSettings/property", "value");
                ApplyVariables();
                var keys = new ArrayList();
                foreach (string key in Value.Keys)
                    keys.Add(key);
                foreach (string key in keys)
                {
                    Value[key] = Common.DirectoryPathReplace(Value[key]);
                }
            }
            return root;
        }

        /// <summary>
        /// Reset the static Param data to its launch state
        /// </summary>
        public static void UnLoadValues()
        {
            xmlMap.RemoveAll();
            Value.Clear();
        }

        private static bool _validateXmlSuccess;
        private static readonly StringBuilder _validateXmlError = new StringBuilder();

        /// <summary>
        /// Validation Event Argument added to the Validate Xml Method
        /// </summary>
        /// <param name="sender">sender object </param>
        /// <param name="args">The argument to be captured</param>
        private static void ValidationCallBack(object sender, ValidationEventArgs args)
        {
            //Display the validation error. This is only called on error 
            _validateXmlSuccess = false;
            //Validation failed 
            _validateXmlError.Append(args.Message + "\\n");
        }

        private static void ApplyVariables()
        {
            var map = new Dictionary<string, string>();
            map["AppData"] = Common.GetAllUserAppPath();
            var deleg = new Substitution.MyDelegate(map);
            var mySub = new Substitution();
            var startValue = new Dictionary<string, string>(Value);
            foreach (var key in startValue.Keys)
                Value[key] = mySub.DoSubstitute(startValue[key], @"%\(([^)]*)\)s", RegexOptions.None, deleg.myValue);
        }

        public static void LoadDictionary(IDictionary<string, string> target, string xmlPath, string attr)
        {
            target.Clear();
            var settings = GetItems(xmlPath);
            if (settings != null)
                foreach (XmlNode node in settings)
                    target[Name(node)] = AttrValue(node, attr);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static XmlNodeList GetItems(string path)
        {
            XmlNode root = xmlMap.DocumentElement;
            if (root == null) return null;
            return root.SelectNodes(path);
        }

        /// <summary>
        /// <mobileProperty>
        /// <mobilefeature name="FileProduced" select="One" />
        /// <mobilefeature name="RedLetter" select="No" />
        /// </mobileProperty>
        /// </summary>
        /// <param name="path"></param>
        /// <returns>return dictionary[name] = selelct </returns>
        public static Dictionary<string,string> GetItemsAsDictionary(string path)
        {
            Dictionary<string, string> attrib = new Dictionary<string, string>();
            XmlNode root = xmlMap.DocumentElement;
            if (root == null) return null;
            XmlNodeList list = root.SelectNodes(path);
            try
            {
                foreach (XmlNode node in list)
                {
                    string key = node.Attributes[0].Value;
                    string val = node.Attributes[1].Value;
                    attrib[key] = val;
                }
            }
            catch
            {
            }
            return attrib;
        }
 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static XmlNode GetItem(string path)
        {
            XmlNode root = xmlMap.DocumentElement;
            if (root == null) return null;
            return root.SelectSingleNode(path);
        }

        /// <summary>
        /// Remove the XMLNode using xpath 
        /// </summary>
        /// <param name="xmlFileNameWithPath">File Name</param>
        /// <param name="xPath">Xpath for the XML Node</param>
        /// <returns>Returns XMLNodeList </returns>
        public static void RemoveXMLNode(string xmlFileNameWithPath, string xPath)
        {
            XmlNode node = xmlMap.SelectSingleNode(xPath);
            node.ParentNode.RemoveChild(node);
            xmlMap.Save(xmlFileNameWithPath);
        }

        /// <summary>
        /// Sets the Gloabal Font Size and Font Name from StyleSettings.xml
        /// </summary>
        protected static void SetFontNameSize()
        {
            XmlDocument xmlDocument = Common.DeclareXMLDocument(false);
            xmlDocument.Load(UiSettingPath);
            XmlNode root = xmlDocument.DocumentElement;
            Debug.Assert(root != null);
            string xPath = "//stylePick/customize//font";
            XmlNode returnNode = root.SelectSingleNode(xPath);
            string fontName = returnNode.Attributes.GetNamedItem("name").Value;
            string strFontSize = returnNode.Attributes.GetNamedItem("size").Value;
            float fontSize = float.Parse(strFontSize);
            Common.UIFont = new Font(fontName, fontSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        protected static string GetAttrSummary(string path, string attr)
        {
            var summary = "";
            foreach (XmlNode node in GetItems(path))
                summary += string.Format("{0}: {1}   ", Name(node), AttrValue(node, attr));
            return summary;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        public static List<string> GetListofAttr(string path, string attr)
        {
            var list = new List<string>();
            foreach (XmlNode node in GetItems(path))
            {
                if (node.Attributes.GetNamedItem(attr) != null)
                {
                    list.Add(node.Attributes.GetNamedItem(attr).Value);
                }
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        public static string GetAttrByName(string path, string name, string attr)
        {
            foreach (XmlNode node in GetItems(path))
                if (Name(node).ToLower() == name.ToLower())
                    return node.Attributes.GetNamedItem(attr).Value;
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="elem"></param>
        /// <returns></returns>
        public static string GetElemByName(string path, string name, string elem)
        {
            foreach (XmlNode node in GetItems(path))
                if (Name(node).ToLower() == name.ToLower())
                    foreach (XmlNode child in node.ChildNodes)
                        if (child.Name.ToLower() == elem.ToLower())
                            return child.InnerText.Trim();
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string GetTagFile(XmlNode node)
        {
            return node.Attributes.GetNamedItem("file").InnerText;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="tv"></param>
        /// <param name="features"></param>
        public static void LoadFeatures(string path, TreeView tv, List<string> features)
        {

            tv.StateImageList = new ImageList();
            if (iconIdx.ContainsKey("deselect.png"))
                tv.StateImageList.Images.Add(imageListSmall.Images[iconIdx["deselect.png"]]);
            if (iconIdx.ContainsKey("select.png"))
                tv.StateImageList.Images.Add(imageListSmall.Images[iconIdx["select.png"]]);

            tv.ImageList = imageListSmall;
            tv.SelectedImageIndex = 1;

            foreach (XmlNode node in GetItems(path))
            {
                var tn = new TreeNode { Text = Name(node), Checked = true };
                featureList[tn.Text] = new Dictionary<string, string>();
                foreach (XmlNode child in node.ChildNodes)
                {
                    var file = AttrValue(child, "file");
                    var opt = new TreeNode { Text = Name(child), Tag = child };
                    featureList[tn.Text][opt.Text] = file;
                    var fn = file.ToLower();
                    if (fn != "")
                    {
                        if (featureIcon.ContainsKey(fn))
                            //opt.ImageIndex = iconIdx[featureIcon[fn]];
                            if (features != null)
                            {
                                //opt.Checked = features.Contains(fn);
                                bool exists = features.Contains(fn);
                                opt.Checked = exists;
                                if (exists)
                                {
                                    //opt.ImageIndex = 1;
                                    opt.StateImageIndex = _selectedIndex;
                                }
                                else
                                {
                                    //opt.ImageIndex = 2;
                                    opt.StateImageIndex = UnSelectedIndex;
                                }

                            }
                    }
                    tn.Nodes.Add(opt);
                }
                tn.Expand();
                tv.Nodes.Add(tn);
            }
            var position = tv.SelectedNode;
            if (position == null)
                tv.Nodes[0].EnsureVisible();
            else
                position.EnsureVisible();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        public static void SetupHelp(Control f)
        {
            var hp = ShowHelp.HelpProv;
            try
            {
                hp.HelpNamespace = Value["Help"];
            }
            catch (Exception)
            {
                hp.HelpNamespace = "PsDom.chm";
            }
            hp.SetHelpNavigator(f, HelpNavigator.Topic);
            hp.SetHelpKeyword(f, f.Name + ".htm");
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Write()
        {
            Debug.Assert(xmlMap.DocumentElement != null);
            var folder = Path.GetDirectoryName(SettingOutputPath);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            var writer = new XmlTextWriter(SettingOutputPath, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            xmlMap.Save(writer);
            writer.Close();
            CopySchemaIfNecessary();
        }

        /// <summary>
        /// Copy schema from program path to all user common folder for publishing solutions
        /// </summary>
        public static void CopySchemaIfNecessary()
        {
            try
            {
                var schema = AttrValue(xmlMap.DocumentElement, "xsi:noNamespaceSchemaLocation");
                var dstSchemaPath = Common.PathCombine(Path.GetDirectoryName(SettingOutputPath), schema);
                if (!File.Exists(dstSchemaPath))
					File.Copy(Common.FromRegistry(schema), dstSchemaPath);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Sets a Parameter value
        /// </summary>
        /// <param name="id"></param>
        /// <param name="val"></param>
        public static void SetValue(string id, string val)
        {
            try
            {
                val = Common.DirectoryPathReplaceWithSlash(val);
                Debug.Assert(Value.ContainsKey(id), "Invalid id: " + id);
                Value[id] = val;
                var node = xmlMap.SelectSingleNode(string.Format("stylePick/settings/property[@name=\"{0}\"]", id));
                Debug.Assert(node != null && node.Attributes != null);
                if(node != null)
				{
                    var valueAttr = node.Attributes.GetNamedItem("value");
                    valueAttr.Value = val;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to set value (name=" + id + ", value=" + val + ")", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="val"></param>
        public static void SetDefaultValue(string id, string val)
        {
            try
            {
                val = Common.DirectoryPathReplaceWithSlash(val);
                Debug.Assert(DefaultValue.ContainsKey(id), "Invalid id: " + id);
                if (DefaultValue[id] == val) return;
                DefaultValue[id] = val;
                var node = xmlMap.SelectSingleNode(string.Format("stylePick/defaultSettings/property[@name=\"{0}\"]", id));
                Debug.Assert(node != null);
                var valueAttr = node.Attributes.GetNamedItem("value");
                valueAttr.Value = val;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to set default value (name=" + id + ", value=" + val + ")", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="description"></param>
        /// <param name="fileNamewithPath"></param>
        private static void SaveSheet(string sheet, string fileNamewithPath, string description)
        {
            Debug.Assert(sheet != "", "Missing sheet name");
            var baseNode = xmlMap.SelectSingleNode("stylePick/styles/" + MediaType);
            Debug.Assert(baseNode != null);
            var style = baseNode.SelectSingleNode(string.Format("style[@name=\"{0}\"]", sheet));
            style = xmlMap.CreateElement("style");
            AddAttrValue(style, "name", sheet);
            //sheet
            var fn = Common.PathCombine(Value[UserSheetPath], sheet + ".css");
            if (File.Exists(fn))
            {
                var i = 0;
                do
                {
                    fn = Common.PathCombine(Value[UserSheetPath], string.Format("{0}{1}.css", sheet, ++i));
                } while (File.Exists(fn));
            }

            if (File.Exists(fileNamewithPath))
            {
                if (_configureType.ToLower() == "standard")
                {
                    string fileName = Path.GetFileName(fileNamewithPath);
                    StreamWriter writeCss = new StreamWriter(fn);
                    writeCss.WriteLine("/* Please do not edit @-import statement - It may cause unexpected results */");
                    writeCss.WriteLine("@import \"" + fileName + "\";");
                    writeCss.Flush();
                    writeCss.Close();
                }
                else 
                {
                    File.Copy(fileNamewithPath, fn, true);
                }
            }

            if (StyleFile.ContainsKey(sheet))
            {
                StyleFile[sheet] = Path.GetFileName(fn);
            }
            else
            {
                StyleFile.Add(sheet, Path.GetFileName(fn));
            }

            AddAttrValue(style, "file", Path.GetFileName(fn));
            baseNode.AppendChild(style);

            var descNode = style.SelectSingleNode("description");
            if (descNode == null)
            {
                descNode = xmlMap.CreateElement("description");
                descNode.InnerText = description;
                style.AppendChild(descNode);
            }
            else
            {
                descNode.InnerText = description;
            }
            AddAttrValue(style, "shown", "Yes");
            WebInfo(baseNode, style);        
            Write();
        }

        public static void WebInfo(XmlNode baseNode, XmlNode style)
        {
            if (MediaType != "web")
                return;

            XmlNode styleProp = xmlMap.CreateElement("styleProperty");
            AddAttrValue(styleProp, "name", "ftpaddress");
            AddAttrValue(styleProp, "value", String.Empty);
            style.AppendChild(styleProp);

            styleProp = xmlMap.CreateElement("styleProperty");
            AddAttrValue(styleProp, "name", "ftpuserid");
            AddAttrValue(styleProp, "value", String.Empty);
            style.AppendChild(styleProp);

            styleProp = xmlMap.CreateElement("styleProperty");
            AddAttrValue(styleProp, "name", "ftppwd");
            AddAttrValue(styleProp, "value", String.Empty);
            style.AppendChild(styleProp);

            styleProp = xmlMap.CreateElement("styleProperty");
            AddAttrValue(styleProp, "name", "dbservername");
            AddAttrValue(styleProp, "value", String.Empty);
            style.AppendChild(styleProp);

            styleProp = xmlMap.CreateElement("styleProperty");
            AddAttrValue(styleProp, "name", "dbname");
            AddAttrValue(styleProp, "value", String.Empty);
            style.AppendChild(styleProp);

            styleProp = xmlMap.CreateElement("styleProperty");
            AddAttrValue(styleProp, "name", "dbuserid");
            AddAttrValue(styleProp, "value", String.Empty);
            style.AppendChild(styleProp);

            styleProp = xmlMap.CreateElement("styleProperty");
            AddAttrValue(styleProp, "name", "dbpwd");
            AddAttrValue(styleProp, "value", String.Empty);
            style.AppendChild(styleProp);

            styleProp = xmlMap.CreateElement("styleProperty");
            AddAttrValue(styleProp, "name", "weburl");
            AddAttrValue(styleProp, "value", String.Empty);
            style.AppendChild(styleProp);

            styleProp = xmlMap.CreateElement("styleProperty");
            AddAttrValue(styleProp, "name", "webadminusrnme");
            AddAttrValue(styleProp, "value", String.Empty);
            style.AppendChild(styleProp);

            styleProp = xmlMap.CreateElement("styleProperty");
            AddAttrValue(styleProp, "name", "webadminpwd");
            AddAttrValue(styleProp, "value", String.Empty);
            style.AppendChild(styleProp);

            styleProp = xmlMap.CreateElement("styleProperty");
            AddAttrValue(styleProp, "name", "webadminsiteNme");
            AddAttrValue(styleProp, "value", String.Empty);
            style.AppendChild(styleProp);

            styleProp = xmlMap.CreateElement("styleProperty");
            AddAttrValue(styleProp, "name", "webemailid");
            AddAttrValue(styleProp, "value", String.Empty);
            style.AppendChild(styleProp);

            styleProp = xmlMap.CreateElement("styleProperty");
            AddAttrValue(styleProp, "name", "webftpfldrnme");
            AddAttrValue(styleProp, "value", String.Empty);
            style.AppendChild(styleProp);

            styleProp = xmlMap.CreateElement("styleProperty");
            AddAttrValue(styleProp, "name", "comment");
            AddAttrValue(styleProp, "value", String.Empty);
            style.AppendChild(styleProp);
        }

        public static void SaveSheet(string sheet, string fileNamewithPath, string description, string configureType)
        {
            _configureType = configureType;
            SaveSheet(sheet, fileNamewithPath, description);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="style"></param>
        /// <param name="cat"></param>
        /// <param name="opt"></param>
        /// <param name="val"></param>
        public static void SaveStyleCategories(string style, string cat, string opt, string val)
        {
            if (val != StyleCategories.YES && val != StyleCategories.NO) return;
            var sel = xmlMap.SelectSingleNode(string.Format("/stylePick/styles/" + MediaType + "/style[@name=\"{0}\"]", style));
            Debug.Assert(sel != null, "Missing style " + style);
            var prev = sel.SelectSingleNode(string.Format("criteria[@category=\"{0}\" and @option=\"{1}\"]", cat, opt));
            var newVal = val == StyleCategories.YES ? "include" : "exclude";
            if (prev != null)
                prev.Attributes["action"].Value = newVal;
            else
            {
                prev = xmlMap.CreateElement("criteria");
                AddAttrValue(prev, "category", cat);
                AddAttrValue(prev, "option", opt);
                AddAttrValue(prev, "action", newVal);
                sel.AppendChild(prev);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="style"></param>
        /// <param name="cat"></param>
        /// <param name="opt"></param>
        /// <returns></returns>
        public static string LoadStyleCategories(string style, string cat, string opt)
        {
            var sel = xmlMap.SelectSingleNode(string.Format("/stylePick/styles/" + MediaType + "/style[@name=\"{0}\"]", style));
            Debug.Assert(sel != null, "Missing style " + style);
            var prev = sel.SelectSingleNode(string.Format("criteria[@category=\"{0}\" and @option=\"{1}\"]", cat, opt));
            if (prev == null) return null;
            return prev.Attributes["action"].Value == "include" ? StyleCategories.YES : StyleCategories.NO;
        }

        public static void AddAttrValue(XmlNode node, string attrTag, string val)
        {
            var attr = xmlMap.CreateAttribute(attrTag);
            attr.Value = val;
            node.Attributes.Append(attr);
        }

        private static void AddNodeText(XmlNode node, string elementName, string innerText)
        {
            var attr = xmlMap.CreateNode(XmlNodeType.Element, elementName, "");
            attr.InnerText = innerText;
            node.AppendChild(attr);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public static string StylePath(string sheet)
        {
            return StylePath(sheet, FileAccess.Read);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="fa"></param>
        /// <returns></returns>
        private static string StylePath(string sheet, FileAccess fa)
        {
            var fn = sheet.Contains(".css") ? sheet : StyleFile.ContainsKey(sheet) ? StyleFile[sheet] :  "";
            var fPath = Common.PathCombine(Value[UserSheetPath], fn);
            if (fa == FileAccess.Write) return fPath;
            if (!File.Exists(fPath))
            {
				fPath = Common.FromRegistry(Common.PathCombine(Value[MasterSheetPath], fn));
            }
            return fPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public static void SetAttrValue(XmlNode node, string name, string val)
        {
            if(node == null) return;
            var attr = node.Attributes.GetNamedItem(name);
            if (attr == null)
            {
                AddAttrValue(node, name, val);
            }
            else
            {
                attr.Value = val;
            }
        }

        public static void SetNodeText(XmlNode node, string name, string val)
        {
            var attr = node.SelectSingleNode(name);
            if (attr == null)
            {
                AddNodeText(node, name, val);
            }
            else
            {
                attr.InnerText = val;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        public static string AttrValue(XmlNode node, string attr)
        {
            var attrNode = node.Attributes.GetNamedItem(attr);
            if (attrNode == null) return "";
            return attrNode.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string Name(XmlNode node)
        {
            string retValue = AttrValue(node, "name");
            if(retValue.Length == 0)
                retValue = AttrValue(node, "Name");
            if (retValue.Length == 0)
                retValue = AttrValue(node, "NAME");
            return retValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetRole()
        {

            List<string> ls = GetListofAttr("roles", "select");
            if (ls.Count > 0)
                return ls[0];

            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        public static void SetRole(string role)
        {
            XmlNode node = xmlMap.SelectSingleNode("stylePick/roles/@select");
            if (node == null) return;
            node.Value = role;
            UserRole = role;
            Write();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static string TaskSheet(string task)
        {
            return GetAttrByName("tasks/task", task, "style");
        }

        /// <summary>
        /// To set the properties for Scripture GoBible
        /// </summary>
        /// <param name="attribName"></param>
        /// <param name="attribValue"></param>
        /// <param name="styleName">style in which to look for attribute</param>
        public static void UpdateMobileAtrrib(string attribName, string attribValue, string styleName)
        {
            string searchStyleName = styleName;
            XmlNode node = Param.GetItem("//stylePick/styles/mobile/style[@name='" + searchStyleName + "']/styleProperty[@name='" + attribName + "']");
            if (node == null)
            {
                // styleProperty node doesn't exist yet (this happens when the user clicks "new" instead of "copy") -
                // create it now - s/b like this: <styleProperty name="[attribName]" value="[attribValue]" />
                XmlNode baseNode = GetItem("//stylePick/styles/mobile/style[@name='" + searchStyleName + "']");
                var childNode = xmlMap.CreateNode(XmlNodeType.Element, "styleProperty", "");
                AddAttrValue(childNode, "name", attribName);
                AddAttrValue(childNode, "value", attribValue);
                baseNode.AppendChild(childNode);
                Write();
            }
            else
            {
                // attribute is there - just set the value
                SetAttrValue(node, "value", attribValue);
                Write();
            }
        }

        /// <summary>
        /// To set the properties for "others" branch items (just .epub for now)
        /// </summary>
        /// <param name="attribName"></param>
        /// <param name="attribValue"></param>
        /// <param name="styleName">style name in which to look for attribute</param>
        public static void UpdateOthersAtrrib(string attribName, string attribValue, string styleName)
        {
            string searchStyleName = styleName;
            XmlNode node = GetItem("//stylePick/styles/others/style[@name='" + searchStyleName + "']/styleProperty[@name='" + attribName + "']");
            if (node == null)
            {
                // styleProperty node doesn't exist yet (this happens when the user clicks "new" instead of "copy") -
                // create it now - s/b like this: <styleProperty name="[attribName]" value="[attribValue]" />
                XmlNode baseNode = GetItem("//stylePick/styles/others/style[@name='" + searchStyleName + "']");
                var childNode = xmlMap.CreateNode(XmlNodeType.Element, "styleProperty", "");
                AddAttrValue(childNode, "name", attribName);
                AddAttrValue(childNode, "value", attribValue);
                if (baseNode != null)
                {
                    baseNode.AppendChild(childNode);
                    Write();
                }
            }
            else
            {
                // attribute is there - just set the value
                SetAttrValue(node, "value", attribValue);
                Write();
            }
        }

        // note that the organization name and metadata values are stored encode on the user's machine. This is to sanitize
        // input from users on text fields.
        #region Metadata methods
        /// <summary>
        /// Returns the current organization specified by the Select Your Organization dialog. If none is specified,
        /// this method returns an empty string.
        /// </summary>
        /// <returns>The current organization (string).</returns>
        public static string GetOrganization()
        {
            string organization;
            try
            {
                organization = "";
                // get the organization
                if (Value.ContainsKey("Organization"))
                {
                    organization = Value["Organization"];
                }
            }
            catch (Exception)
            {
                organization = "";
            }
            return XmlConvert.DecodeName(organization);
        }

        /// <summary>
        /// Override to GetMetadataValue. Returns the metadata value for the current organization.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetMetadataValue (string name)
        {
            string organization = GetOrganization();
            return GetMetadataValue(name, organization);
        }

        /// <summary>
        /// Returns the value of the specified Metadata element:
        /// - if there is a current value, this method returns that value first
        /// - if not, it will fall back on the default for the organization
        /// - if there is no organization default, it will fall back on the defaultValue
        /// - if there is no defaultValue (shouldn't happen), it will return null
        /// </summary>
        /// <param name="Name">meta name to retrieve</param>
        /// <param name="Organization">user's publishing organization (SIL, etc.)</param>
        /// <returns></returns>
        public static string GetMetadataValue(string Name, string Organization)
        {
            XmlNode node;
            try
            {
                node = GetItem("//stylePick/Metadata/meta[@name='" + Name + "']/currentValue");
            }
            catch (Exception e)
            {
                // Exception (key not found?) - bail out with a null value
                Debug.WriteLine(e.ToString());
                return null;
            }
            if (node == null || node.InnerText == "")
            {
                // no current value - attempt to get the organization default
                node = GetItem("//stylePick/Metadata/meta[@name='" + Name + "']/organizationDefault[@organizationName='" + XmlConvert.EncodeName(Organization) + "']");
				if (node == null || node.InnerText == "")
                {
                    // no organization default for this node - get the default value
                    node = GetItem("//stylePick/Metadata/meta[@name='" + Name + "']/defaultValue");
                    // test for null node is in the return value below
                }
            }
            return (node == null) ? null : (XmlConvert.DecodeName(node.InnerText.Trim()));
        }

        /// <summary>
        /// To set the properties for "metadata" branch items.
        /// </summary>
        /// <param name="name">Name of metadata element to update</param>
        /// <param name="currValue">Value to set as Current Value</param>
        public static void UpdateMetadataValue(string name, string currValue)
        {
            try
            {
                VerifyMetaNode(name);

                XmlNode node = GetItem("//stylePick/Metadata/meta[@name='" + name + "']/currentValue");
                var newValue = " ";
                if(currValue != null && currValue.Trim().Length > 0)
                {
                    newValue = currValue.Trim();
                }

                //var newValue = (Value.Trim().Length > 0) ? Value.Trim() : " ";
                if (node == null)
                {
                    // currentValue node doesn't exist yet - create it now
                    XmlNode baseNode = GetItem("//stylePick/Metadata/meta[@name='" + name + "']");
                    var childNode = xmlMap.CreateNode(XmlNodeType.Element, "currentValue", "");
                    childNode.InnerText = XmlConvert.EncodeName(newValue);
                    baseNode.AppendChild(childNode);
                    Write();
                }
                else
                {
                    // mode is there - just set the value
                    node.InnerText = XmlConvert.EncodeName(newValue);
                    Write();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to update Metadata Value (Name=" + name + ", Value=" + currValue + ")", ex);
            }
        }

        /// <summary>
        /// Returns the value of the Title Metadata element:
        /// - if there is a current value, this method returns that value first
        /// - if not, it will fall back on the default for the organization
        /// </summary>
        /// <param name="name">meta name to retrieve</param>
        /// <param name="organization">user's publishing organization (SIL, etc.)</param>
        /// <param name="isConfigurationTool">method calling from ConfigurationTool/Not </param>
        /// <returns></returns>
        public static string GetTitleMetadataValue(string name, string organization, bool isConfigurationTool)
        {
            XmlNode node;
            string lastSavedDatabase = string.Empty;
            try
            {
                if (isConfigurationTool)
                {
                    node = GetItem("//stylePick/Metadata/meta[@name='" + name + "']/defaultValue");
                }
                else
                {
                    node = GetItem("//stylePick/Metadata/meta[@name='" + name + "']/PreviousProjectName");
                    if (node != null)
                        lastSavedDatabase = node.InnerText;

                    if (lastSavedDatabase != DatabaseName)
                    {
                        node = GetItem("//stylePick/Metadata/meta[@name='" + name + "']/defaultValue");
                    }
                    else
                    {
                        node = GetItem("//stylePick/Metadata/meta[@name='" + name + "']/currentValue");
                    }
                }
            }
            catch (Exception e)
            {
                // Exception (key not found?) - bail out with a null value
                Debug.WriteLine(e.ToString());
                return null;
            }
            return (node == null) ? null : (XmlConvert.DecodeName(node.InnerText.Trim()));
        }

        /// <summary>
        /// Update the title Value in settings file:
        /// - if there is a current value, this method returns that value first
        /// - if not, it will fall back on the default for the organization
        /// - If assigns the project name value for validation.
        /// </summary>
        /// <param name="name">Name of metadata element to update</param>
        /// <param name="value">Value to set as Current Value</param>
        /// <param name="isConfigurationTool">Method calling from ConfigurationTool or Not </param>
        public static void UpdateTitleMetadataValue(string name, string value, bool isConfigurationTool)
        {
            try
            {
                XmlNode node;
                VerifyMetaNode(name);
                if (isConfigurationTool)
                {
                    node = GetItem("//stylePick/Metadata/meta[@name='" + name + "']/defaultValue");
                    if (node != null)
                    {
                        node.InnerText = value;
                        Write();
                    }
                }
                else
                {
                    node = GetItem("//stylePick/Metadata/meta[@name='" + name + "']/currentValue");
                    if (node != null)
                    {
                        node.InnerText = XmlConvert.EncodeName(value);
                        Write();
                    }
                    XmlNode projNode = GetItem("//stylePick/Metadata/meta[@name='" + name + "']/PreviousProjectName");
                    if (projNode != null)
                    {
                        projNode.InnerText = Common.databaseName;
                        Write();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to update Metadata Value (Name=" + name + ", Value=" + value + ")", ex);
            }
        }

        private static void VerifyMetaNode(string Name)
        {
            XmlNode node = GetItem("//stylePick/Metadata/meta[@name='" + Name + "']/defaultValue");
            if (node == null)
            {
                XmlNode baseNode = GetItem("//stylePick/Metadata/meta[@name='" + Name + "']");
                var childNode = xmlMap.CreateNode(XmlNodeType.Element, "defaultValue", "");
                childNode.InnerText = " ";
                baseNode.AppendChild(childNode);
                Write();
            }
        }

        public static string GetMetadataCurrentValue(string name)
        {
            XmlNode node;
            try
            {
                node = GetItem("//stylePick/Metadata/meta[@name='" + name + "']/currentValue");
                if(node != null && node.InnerText.Length == 0)
                {
                    node = GetItem("//stylePick/Metadata/meta[@name='" + name + "']/defaultValue");
                }
            }
            catch (Exception e)
            {
                // Exception (key not found?) - bail out with a null value
                Debug.WriteLine(e.ToString());
                return null;
            }
            return (node == null) ? null : (XmlConvert.DecodeName(node.InnerText.Trim()));
        }

        #endregion Metadata methods

        #region LoadImageList
        /// <summary>
        /// small images for the ListView controls
        /// </summary>
        static readonly ImageList imageListSmall = new ImageList();

        /// <summary>
        /// large (32x32) images for the ListView controls
        /// </summary>
        static readonly ImageList imageListLarge = new ImageList();

        /// <summary>
        /// The number of images in imageListSmall and imageListLarge
        /// </summary>
        static int imageListCount;

        /// <summary>
        /// Given the icon name, return its index in imageListSmall (or imageListLarge)
        /// </summary>
        static readonly Dictionary<string, int> iconIdx = new Dictionary<string, int>();

        /// <summary>
        /// Given the name of the css file in lower case, return the icon to be used for it.
        /// </summary>
        static readonly Dictionary<string, string> featureIcon = new Dictionary<string, string>();

    	/// <summary>
        /// Load ListView icons from image folder
        /// </summary>
        private static void LoadImageList()
        {
            LoadIconMap();
            imageListSmall.Images.Clear();
            imageListLarge.Images.Clear();
            imageListLarge.ImageSize = new Size(32, 32);
			AddBmpIcon(new FileInfo(Common.FromRegistry(Value[MissingIcon])), GetBitmap(Common.FromRegistry(Value[MissingIcon])));
			AddBmpIcon(new FileInfo(Common.FromRegistry(Value[SelectedIcon])), GetBitmap(Common.FromRegistry(Value[SelectedIcon])));
			AddBmpIcon(new FileInfo(Common.FromRegistry(Value[DefaultIcon])), GetBitmap(Common.FromRegistry(Value[DefaultIcon])));
			var di = new DirectoryInfo(Common.FromRegistry(Value[IconPath]));
            var bmpList = di.GetFiles("*.png");
            foreach (var bmp in bmpList)
                AddBmpIcon(bmp, GetBitmap(bmp.FullName));
            imageListCount = 0;
        }

        /// <summary>
        /// Returns and in memory Image of the Bitmap from disk
        /// </summary>
        /// <param name="name">path to bitmap file</param>
        /// <returns>memory image of Bitmap</returns>
        protected static Image GetBitmap(string name)
        {
            FileStream fileStream = new FileStream(name, FileMode.Open, FileAccess.Read);
            var bmp = new Bitmap(fileStream);
            fileStream.Close();
            return bmp;
        }

        /// <summary>
        /// Add file represented by ico to large and small icon lists.
        /// </summary>
        /// <param name="ico">gives name of file</param>
        /// <param name="bmp">gives a bitmap of the file</param>
        private static void AddBmpIcon(FileSystemInfo ico, Image bmp)
        {
            imageListSmall.Images.Add(bmp);
            imageListLarge.Images.Add(bmp);
            iconIdx[ico.Name.ToLower()] = imageListCount;
            imageListCount++;
        }

        /// <summary>
        /// Load the XML file with the map from css name to icon name into the member cssIcon member variable
        /// </summary>
        private static void LoadIconMap()
        {
            foreach (XmlNode node in GetItems("features/feature/option"))
                try
                {
                    featureIcon[AttrValue(node, "file").ToLower()] = Path.GetFileName(AttrValue(node, "icon")).ToLower();
                }
                catch (Exception)
                {
                    featureIcon[AttrValue(node, "file").ToLower()] = Path.GetFileName(Value[DefaultIcon]);
                }

        }
        #endregion LoadImageList

        #region UI Language Fonts

        public static void LoadUiLanguageFontInfo()
        {
            var xmlDoc = new XmlDocument();
            string fileName = Common.PathCombine(Common.GetAllUserAppPath(), @"SIL\Pathway\UserInterfaceLanguage.xml");
            if (!File.Exists(fileName))
            {
				string pathwayDirectory = Common.AssemblyPath;
                if (pathwayDirectory != null)
                {
                    string installedLocalizationsFolder = Path.Combine(pathwayDirectory, "localizations");

					if (!Directory.Exists(installedLocalizationsFolder))
					{
						installedLocalizationsFolder = Path.GetDirectoryName(Common.AssemblyPath);
						installedLocalizationsFolder = Common.PathCombine(installedLocalizationsFolder, "localizations");
					}

                    string xmlSourcePath = Common.PathCombine(installedLocalizationsFolder, "UserInterfaceLanguage.xml");
	                if (File.Exists(xmlSourcePath))
	                {
		                File.Copy(xmlSourcePath, fileName, true);
	                }
	                else
	                {
		                return;
	                }
                }
            }

	        if (File.Exists(fileName))
	        {
		        var content = File.ReadAllText(fileName);

				if (Common.IsUnixOS() && content.Contains("Microsoft Sans Serif"))
					content = content.Replace("Microsoft Sans Serif", "Liberation Serif");

		        xmlDoc.LoadXml(content);
		        var uiFontInfos = xmlDoc.SelectNodes("//UILanguage/fontstyle/font");
		        foreach (XmlNode fontInfo in uiFontInfos)
		        {
			        if (fontInfo.Attributes != null)
			        {
				        string lang = fontInfo.Attributes["lang"].InnerText;
				        string fontName = fontInfo.Attributes["name"].InnerText;
				        string fontSize = fontInfo.Attributes["size"].InnerText;
				        UiLanguageFontSettings[lang] = new Dictionary<string, string> {{fontName, fontSize}};
			        }
		        }
				xmlDoc.Save(fileName);
	        }
        }

        public static void GetFontValues(string langId, ref string fontName, ref string fontSize)
	    {
            try
            {
                if (UiLanguageFontSettings.ContainsKey(langId))
                {
                    Dictionary<string, string> fontInfo = Param.UiLanguageFontSettings[langId];
                    foreach (KeyValuePair<string, string> fontValue in fontInfo)
                    {
                        fontName = fontValue.Key;
                        fontSize = fontValue.Value;
                    }
                }
            }
            catch
            {

                fontName = "Microsoft Sans Serif";
                fontSize = "8";
            }
	    }
        #endregion
    }
}
