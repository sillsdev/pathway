// --------------------------------------------------------------------------------------------
// <copyright file="Param.cs" from='2009' to='2009' company='SIL International'>
//      Copyright © 2009, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Parameter setting for Settingsxml
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
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class InvalidStyleSettingsException : Exception
    {
        public string FullFilePath { get; set; }

        public override string ToString()
        {
            return "{0} is not a valid settings file";
        }
    }

    public class Param
	{
		// Settings
        public const string InputPath = "InputPath";
        public const string OutputPath = "OutputPath";
        public const string UserSheetPath = "UserSheetPath";
        public const string PublicationLocation = "PublicationLocation";
        public const string MasterSheetPath = "MasterSheetPath";
        public const string IconPath = "IconPath";
        public const string SamplePath = "SamplePath";
        public const string DefaultIcon = "DefaultIcon";
        public const string CurrentInput = "CurrentInput";
        public const string InputType = "InputType";
        public const string LastTask = "LastTask";
        public const string PreviewType = "PreviewType";
        public const string CssEditor = "CssEditor";
        public const string SelectedIcon = "SelectedIcon";
        public const string MissingIcon = "MissingIcon";
        public const string Help = "Help";
        // Default Settings
        public const string PrintVia = "PrintVia";
        public const string ConfigureDictionary = "ConfigureDictionary";
        public const string ReversalIndex = "ReversalIndexes";
        public const string GrammarSketch = "GrammerSketch";
        public const string LayoutSelected = "LayoutSelected";
        public const string ExtraProcessing = "ExtraProcessing";
        public const string Media = "Media";

        // Other constants
        private const string DefaultSettingsFileName = "StyleSettings.xml";
        public static string UserRole = "Output User";
        public static string MediaType = "paper";

		#region Public properties
		private static string _LoadType = string.Empty;
        public static string SettingPath
        {
            get
            {
				return Common.FromRegistry(_LoadType + DefaultSettingsFileName);
            }
        }

        public static string SettingOutputPath
        {
            get
            {
                Debug.Assert(Value.ContainsKey(OutputPath), "Settings not loaded");
                return Common.PathCombine(Value[OutputPath], _LoadType + DefaultSettingsFileName);
            }
        }

        public static string SetLoadType
        {
            set
            {
                _LoadType = value;
            }
        }
        public static string UiSettingPath
        {
            get
            {
				return Common.FromRegistry(DefaultSettingsFileName);
            }
        }
        #endregion Public properties

        public static Dictionary<string, string> Value = new Dictionary<string, string>();
        public static Dictionary<string, string> DefaultValue = new Dictionary<string, string>();
        public static Dictionary<string, string> StyleFile = new Dictionary<string, string>();
        public static Dictionary<string, Dictionary<string, string>> featureList = new Dictionary<string, Dictionary<string, string>>();
        public static readonly XmlDocument xmlMap = new XmlDocument { XmlResolver = null };
        private static int _selectedIndex = 1;
        private static int _UnSelectedIndex = 2;
        private static string _configureType = string.Empty;


        /// <summary>
        /// Load settings first from program path and then from user path.
        /// </summary>
        public static void LoadSettings()
        {
            _LoadType = Value.ContainsKey(InputType) ? Value[InputType] : "";
            if (LoadValues(SettingPath) == null) return;
            if (!Directory.Exists(Value[OutputPath]))
            {
                ReplacePStoPathWay();
                Directory.CreateDirectory(Value[OutputPath]);
            }
			
            try
            {
                if (File.Exists(SettingOutputPath))
                    if (LoadValues(SettingOutputPath) == null) return;
            }
            catch (Exception)
            {
                LoadValues(SettingPath);
            }
            MediaType = GetAttrByName("//categories/category", "Media", "select").ToLower();
            LoadDictionary(StyleFile, "styles//style", "file");
            LoadImageList();
        }

        /// <summary>
        /// Function for renaming PublishingSolutions to Pathway.
        /// </summary>
        private static void ReplacePStoPathWay()
        {
            string publishingSolutionsDirectory = Path.Combine(Path.GetDirectoryName(Value[OutputPath]), "PublishingSolutions");
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
            UnLoadValues();
            var reader = validatexml(path);
            xmlMap.Load(reader);
            reader.Close();
            if (!_validateXmlSuccess)
            {
                //MessageBox.Show(_validateXmlError.ToString());
                InvalidStyleSettingsException err = new InvalidStyleSettingsException();
                err.FullFilePath = path;
                throw err;
            }
            XmlNode root = xmlMap.DocumentElement;
            if (root != null)
            {
                LoadDictionary(Value, "settings/property", "value");
                LoadDictionary(DefaultValue, "defaultSettings/property", "value");
                ApplyVariables();
                ArrayList keys = new ArrayList();
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
        /// Validates the Xml File with Schema file 
        /// </summary>
        /// <param name="inputFile">The Input Xml File</param>
        /// <returns>true/false based on the validation</returns>
        private static XmlValidatingReader validatexml(string inputFile)
        {
            //First we create the xmltextreader 
            var xmlr = new XmlTextReader(inputFile);
            //We pass the xmltextreader into the xmlvalidatingreader 
            //This will validate the xml doc with the schema file 
            //NOTE the xml file it self points to the schema file 
            var xmlvread = new XmlValidatingReader(xmlr);

            // Set the validation event handler 
            xmlvread.ValidationEventHandler += ValidationCallBack;
            _validateXmlSuccess = true;

            return xmlvread;
        }

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
            map["AppData"] = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var deleg = new Substitution.MyDelegate(map);
            var mySub = new Substitution();
            var startValue = new Dictionary<string, string>(Value);
            foreach (var key in startValue.Keys)
                Value[key] = mySub.DoSubstitute(startValue[key], @"%\(([^)]*)\)s", RegexOptions.None, deleg.myValue);
        }

        //public static void StyleSettingsValidation(object sender, ValidationEventArgs e)
        //{
        //    throw new XmlSchemaValidationException(e.Message);
        //}

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

        public static void SaveFontNameSize(string name, string size)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(UiSettingPath);
            XmlNode root = xmlDocument.DocumentElement;
            if (root == null) return;
            string xPath = "//stylePick/customize//font";
            XmlNode returnNode = root.SelectSingleNode(xPath);
            if (returnNode == null) return;
            XmlNode fontName = returnNode.Attributes.GetNamedItem("name");
            fontName.Value = name;
            XmlNode strFontSize = returnNode.Attributes.GetNamedItem("size");
            strFontSize.Value = size;
            TextWriter textWriter = new StreamWriter(SettingOutputPath);
            xmlDocument.Save(textWriter);
            textWriter.Close();
        }

        /// <summary>
        /// Sets the Gloabal Font Size and Font Name from StyleSettings.xml
        /// </summary>
        public static void SetFontNameSize()
        {
            XmlDocument xmlDocument = new XmlDocument { XmlResolver = null };
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
        public static string GetAttrSummary(string path, string attr)
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
        /// <param name="node"></param>
        /// <returns></returns>
        public static string GetTagIcon(XmlNode node)
        {
            var attr = node.Attributes.GetNamedItem("icon");
            return (attr != null) ? attr.InnerText : "";
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
                                    opt.StateImageIndex = _UnSelectedIndex;
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
        /// <param name="path"></param>
        /// <param name="tv"></param>
        public static void LoadCategories(string path, TreeView tv)
        {
            foreach (XmlNode node in GetItems(path))
            {
                var tn = new TreeNode { Text = Name(node) };
                var selection = AttrValue(node, "select");
                foreach (XmlNode child in node.ChildNodes)
                {
                    var name = Name(child);
                    var opt = new TreeNode { Text = name, Checked = (name == selection) };
                    tn.Nodes.Add(opt);
                }
                tn.Expand();
                tv.Nodes.Add(tn);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        public static void SetupHelp(Control f)
        {
            var hp = Common.HelpProv;
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

        private static bool busyChecking;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public static void Check(TreeNode node)
        {
            if (busyChecking) return;
            busyChecking = true;
            Debug.Assert(node != null, "Null node value");
            if (node.Parent == null)
            {
                //node.Checked = false;
                node.Checked = true;
                //node.StateImageIndex = _UnSelectedIndex;
                //node.StateImageIndex = 10;
                node.SelectedImageIndex = -1;
            }
            else
            {
                foreach (TreeNode treeNode in node.Parent.Nodes)
                {
                    treeNode.Checked = false;
                    treeNode.StateImageIndex = _selectedIndex;
                }
                node.Checked = true;
                node.StateImageIndex = _UnSelectedIndex;
            }
            busyChecking = false;
        }

        /// <summary>
        /// Outputs one of the mobile attributes: FileProduced, RedLetter, Information, Copyright, or Icon
        /// </summary>
        /// <returns>true if successful</returns>
        public static bool WriteMobileAttrib(string key, string value)
        {
            XmlNode baseNode = GetItem("//mobileProperty/mobilefeature[@name='" + key + "']");

            if (baseNode == null) return false;
            {
                SetAttrValue(baseNode, "select", value);
            }
            Write();
            return true;
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
            TextWriter textWriter = new StreamWriter(SettingOutputPath);
            xmlMap.Save(textWriter);
            textWriter.Close();
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
        /// 
        /// </summary>
        /// <param name="tlp"></param>
        public static void SaveSettings(TableLayoutPanel tlp)
        {
            Debug.Assert(tlp != null, "Invalid control");
            for (var i = 0; i < tlp.Controls.Count; i += 2)
            {
                var key = tlp.Controls[i].Text;
                var val = tlp.Controls[i + 1].Text;
                SetValue(key, val);
            }
            Write();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="val"></param>
        public static void SetValue(string id, string val)
        {
            val = Common.DirectoryPathReplaceWithSlash(val);
            Debug.Assert(Value.ContainsKey(id), "Invalid id: " + id);
            if (Value[id] == val) return;
            Value[id] = val;
            var node = xmlMap.SelectSingleNode(string.Format("stylePick/settings/property[@name=\"{0}\"]", id));
            Debug.Assert(node != null);
            var valueAttr = node.Attributes.GetNamedItem("value");
            valueAttr.Value = val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="val"></param>
        public static void SetDefaultValue(string id, string val)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tv"></param>
        public static void SaveCategories(TreeView tv)
        {
            Debug.Assert(tv != null, "Invalid control");
            var baseNode = xmlMap.SelectSingleNode("stylePick/categories");
            Debug.Assert(baseNode != null);
            baseNode.RemoveAll();
            foreach (TreeNode node in tv.Nodes)
            {
                var cat = xmlMap.CreateElement("category");
                AddAttrValue(cat, "name", node.Text);
                var select = xmlMap.CreateAttribute("select");
                foreach (TreeNode child in node.Nodes)
                {
                    var item = xmlMap.CreateElement("item");
                    AddAttrValue(item, "name", child.Text);
                    if (child.Checked)
                        select.Value = child.Text;
                    cat.AppendChild(item);
                }
                cat.Attributes.Append(select);
                baseNode.AppendChild(cat);
            }
            Write();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="description"></param>
        /// <param name="fileNamewithPath"></param>
        public static void SaveSheet(string sheet, string fileNamewithPath, string description)
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
            Write();
        }

        public static void SaveSheet(string sheet, string fileNamewithPath, string description, string configureType)
        {
            _configureType = configureType;
            SaveSheet(sheet, fileNamewithPath, description);
        }

        //public static void SaveSheet_OLD(string sheet, string description)
        //{
        //    Debug.Assert(sheet != "", "Missing sheet name");
        //    var baseNode = xmlMap.SelectSingleNode("stylePick/styles");
        //    Debug.Assert(baseNode != null);
        //    var style = baseNode.SelectSingleNode(string.Format("style[@name=\"{0}\"]", sheet));
        //    if (style == null)
        //    {
        //        style = xmlMap.CreateElement("style");
        //        AddAttrValue(style, "name", sheet);
        //        var fn = Common.PathCombine(Value[UserSheetPath], sheet + ".css");
        //        if (File.Exists(fn))
        //        {
        //            var i = 0;
        //            do
        //            {
        //                fn = Common.PathCombine(Value[UserSheetPath], string.Format("{0}{1}.css", sheet, ++i));
        //            } while (File.Exists(fn));
        //        }
        //        StyleFile.Add(sheet, Path.GetFileName(fn));
        //        AddAttrValue(style, "file", Path.GetFileName(fn));
        //        baseNode.AppendChild(style);
        //    }
        //    var descNode = style.SelectSingleNode("description");
        //    if (descNode == null)
        //    {
        //        descNode = xmlMap.CreateElement("description");
        //        descNode.InnerText = description;
        //        style.AppendChild(descNode);
        //    }
        //    else
        //    {
        //        descNode.InnerText = description;
        //    }
        //    Write();
        //}

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="styleList"></param>
        /// <returns></returns>
        public static List<string> FilterStyles(List<string> styleList)
        {
            var stylesExcluded = new List<string>();
            foreach (var style in styleList)
            {
                var styleNode = xmlMap.SelectSingleNode(string.Format("/stylePick/styles/" + MediaType + "/style[@name=\"{0}\"]", style));
                Debug.Assert(styleNode != null, "Missing style " + style);
                var exclude = styleNode.SelectNodes("criteria[@action=\"exclude\"]");
                if (exclude == null) continue;
                foreach (XmlNode criteria in exclude)
                {
                    var cat = criteria.Attributes["category"].Value;
                    var opt = criteria.Attributes["option"].Value;
                    var catNode = xmlMap.SelectSingleNode(string.Format("/stylePick/categories/category[@name=\"{0}\"]", cat));
                    Debug.Assert(catNode != null, "Missing category " + cat);
                    if (opt != catNode.Attributes["select"].Value) continue;
                    stylesExcluded.Add(style);
                }
            }
            foreach (var s in stylesExcluded)
            {
                styleList.Remove(s);
            }
            return styleList;
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
        public static string StylePath(string sheet, FileAccess fa)
        {
            var fn = sheet.Contains(".") ? sheet : StyleFile.ContainsKey(sheet) ? StyleFile[sheet] :  "";
            var fPath = Common.PathCombine(Value[UserSheetPath], fn);
            if (fa == FileAccess.Write) return fPath;
            if (!File.Exists(fPath))
            {
				fPath = Common.FromRegistry(Common.PathCombine(Value[MasterSheetPath], fn));
                //Debug.Assert(File.Exists(fPath),string.Format("StyleSheet file {0} missing", fPath));
            }
            return fPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static XmlNode InsertKind(string kind, string name)
        {
            var sel = xmlMap.SelectSingleNode(string.Format("stylePick/{0}s/{1}[@name=\"{2}\"]", kind, kind, name));
            if (sel != null) return sel;
            var node = xmlMap.CreateElement(kind);
            AddAttrValue(node, "name", name);
            var baseNode = xmlMap.SelectSingleNode(string.Format("stylePick/{0}s", kind));
            baseNode.AppendChild(node);
            return node;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="option"></param>
        /// <param name="css"></param>
        /// <param name="icon"></param>
        public static void InsertOption(XmlNode feature, string option, string css, string icon)
        {
            if (option == "") return;
            var sel = feature.SelectSingleNode("option[@name=\"" + option + "\"]");
            if (sel == null)
            {
                var opt = xmlMap.CreateElement("option");
                AddAttrValue(opt, "name", option);
                AddAttrValue(opt, "file", css);
                if (icon != "")
                    AddAttrValue(opt, "icon", icon);
                feature.AppendChild(opt);
                return;
            }
            SetAttrValue(sel, "file", css);
            if (icon != "")
            {
                SetAttrValue(sel, "icon", icon);
                if (css != "")
                    featureIcon[css.ToLower()] = Path.GetFileName(icon);
            }
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
            //return GetListofAttr("roles", "select")[0];
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
        /// To get the path of the file
        /// </summary>
        /// <param name="filePath">File path with filename</param>
        /// <param name="fileName">CSS filename</param>
        /// <returns></returns>
        public static string GetValidFilePath(string filePath, string fileName)
        {
            if (!File.Exists(filePath))
            {
                string stylesPath = Path.Combine(Value["MasterSheetPath"], fileName);
                filePath = Path.Combine(Path.GetDirectoryName(SettingPath), stylesPath);
                if (!File.Exists(filePath))
                {
                    filePath = Path.Combine(Param.Value["UserSheetPath"], fileName);
                }
            }
            return filePath;
        }

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
        /// 
        /// </summary>
        /// <param name="lv"></param>
        public static void LoadImages(ListView lv)
        {
            lv.Items.Clear();
            lv.SmallImageList = imageListSmall;
			var di = new DirectoryInfo(Common.FromRegistry(Value[IconPath]));
            var exts = new List<string> { ".png" };
            //var exts = new List<string> { ".ico", ".bmp" };
            foreach (var fi in di.GetFiles())
            {
                var lcName = fi.Name.ToLower();
                if (exts.Contains(Path.GetExtension(lcName)))
                    lv.Items.Add(new ListViewItem { Text = lcName, ImageIndex = iconIdx[lcName] });
            }
        }

    	/// <summary>
        /// Load ListView icons from image folder
        /// </summary>
        public static void LoadImageList()
        {
            LoadIconMap();
            imageListSmall.Images.Clear();
            imageListLarge.Images.Clear();
            imageListLarge.ImageSize = new Size(32, 32);
			AddBmpIcon(new FileInfo(Common.FromRegistry(Value[MissingIcon])), new Bitmap(Common.FromRegistry(Value[MissingIcon])));
			AddBmpIcon(new FileInfo(Common.FromRegistry(Value[SelectedIcon])), new Bitmap(Common.FromRegistry(Value[SelectedIcon])));
			AddBmpIcon(new FileInfo(Common.FromRegistry(Value[DefaultIcon])), new Bitmap(Common.FromRegistry(Value[DefaultIcon])));
			var di = new DirectoryInfo(Common.FromRegistry(Value[IconPath]));
            //var icoList = di.GetFiles("*.ico");
            //foreach (var ico in icoList)
            //{
            //    var icon = new Icon(ico.FullName);
            //    var bmp = icon.ToBitmap();
            //    AddBmpIcon(ico, bmp);
            //}
            var bmpList = di.GetFiles("*.png");
            foreach (var bmp in bmpList)
                AddBmpIcon(bmp, new Bitmap(bmp.FullName));
            imageListCount = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="css"></param>
        public static void NewIcon(string name, string css)
        {
            var nm = Path.GetFileName(name).ToLower();
            featureIcon[css.ToLower()] = Path.GetFileName(nm);
            var myName = Common.PathCombine(Value[IconPath], nm).ToLower();
            var fi = new FileInfo(myName);
            if (Path.GetExtension(myName) == ".png")
            {
                var icon = new Icon(myName);
                var bmp = icon.ToBitmap();
                AddBmpIcon(fi, bmp);
            }
            else
            {
                AddBmpIcon(fi, new Bitmap(myName));
            }
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
    }
}
