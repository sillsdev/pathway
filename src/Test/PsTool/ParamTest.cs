// --------------------------------------------------------------------------------------------
// <copyright file="ParamTest.cs" from='2009' to='2014' company='SIL International'>
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
// Test methods of FlexDePlugin
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.Xml;
using SIL.PublishingSolution;
using NUnit.Framework;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using SIL.Tool;

namespace Test.CssDialog
{
    /// <summary>
    ///This is a test class for ParamTest and is intended
    ///to contain all ParamTest Unit Tests
    ///</summary>
    [TestFixture]
    public class ParamTest: Param
    {
        /// <summary>path to input folder for all tests</summary>
        string _inputBasePath = string.Empty;
        /// <summary>path to root of support files in repository (should not be changed!)</summary>
        string _supportPath = string.Empty;
        /// <summary>path to all users output</summary>
        string _publishingSolutionsData = string.Empty;
        const string _StyleSettings = "StyleSettings.xml";
        private string xmlFile;

        #region Setup
        /// <summary>
        /// Turn off debug.assert messages for unit tests
        /// </summary>
        [TestFixtureSetUp]
        protected void SetUpAll()
        {
            CommonTestMethod.DisableDebugAsserts();
            string currentFolder = PathPart.Bin(Environment.CurrentDirectory, "/CssDialog/TestFiles");
            _inputBasePath = Common.PathCombine(currentFolder, "Input");
            _supportPath = PathPart.Bin(Environment.CurrentDirectory, "/../../DistFiles");
	        Common.Testing = true;
            _publishingSolutionsData = Common.GetAllUserPath();
        }

        [TestFixtureTearDown]
        protected void TearDown()
        {
            CommonTestMethod.EnableDebugAsserts();
        }

        [SetUp]
        protected void SetupEach()
        {
            LoadSettingFile();
        }
        #endregion Setup

        /// <summary>
        ///A test for SettingOutputPath with ProgBase set and Param loaded.
        ///</summary>
        [Test]
        public void SettingOutputPathTest()
        {
            Param.LoadValues(Common.PathCombine(_inputBasePath, _StyleSettings));
            string actual;
            actual = Param.PathwaySettingFilePath;
            var expected = Common.PathCombine(_publishingSolutionsData, _StyleSettings);
            Assert.AreEqual(expected, actual);
            Param.UnLoadValues();
        }

        /// <summary>
        ///A test for UnLoadValues
        ///</summary>
        [Test]
        public void UnLoadValuesTest()
        {
            var root = Param.LoadValues(Common.PathCombine(_inputBasePath, _StyleSettings));
            Assert.AreNotEqual(null, root, "UnLoadValueTest expected data to load into root");
            Param.UnLoadValues();
            try
            {
                var value = Param.Value[Param.InputType];
                Assert.Fail(value + " Values available after UnLoadValues");
            }
            catch (Exception e)
            {
                var expected = new KeyNotFoundException();
                Assert.AreEqual(expected.GetType(), e.GetType());
            }
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [Test]
        public void WriteTest()
        {
            var actual = Common.PathCombine(_inputBasePath, _StyleSettings);
            Param.LoadValues(actual);
            if (!Directory.Exists(_publishingSolutionsData))
                Directory.CreateDirectory(_publishingSolutionsData);
            var schema = Path.GetFileNameWithoutExtension(_StyleSettings) + ".xsd";
            var schemaFullName = Path.Combine(_publishingSolutionsData, schema);
            if (File.Exists(schemaFullName))
                File.Delete(schemaFullName);
            Common.ProgBase = _inputBasePath;
            Param.Write();
            var expected = Common.PathCombine(_publishingSolutionsData, _StyleSettings);
            XmlAssert.AreEqual(expected, actual, _StyleSettings);
            var sourceSchema = Common.PathCombine(_inputBasePath, schema);
            var destinationSchema = Common.PathCombine(_publishingSolutionsData, schema);
            XmlAssert.AreEqual(sourceSchema, destinationSchema, schema);
			Common.ProgBase = null;
			DirectoryInfo directoryInfo = new DirectoryInfo(_publishingSolutionsData);
			//directoryInfo.Delete(true);
            Common.DeleteDirectory(_publishingSolutionsData);
			directoryInfo.Create();
            Param.UnLoadValues();
        }

        /// <summary>
        ///A test for GetFontNameSize
        ///</summary>
        [Test]
        public void GetFontNameSizeTest()
        {
			Common.ProgBase = _supportPath;
            Param.SetFontNameSize();
            string fontName = "Microsoft Sans Serif";
            float fontSize = 9;
            Font expected = new Font(fontName, fontSize);
            Assert.AreEqual(expected, Common.UIFont);
			Common.ProgBase = "";
        }

//yesu

        /// <summary>
        ///A test for GetRole
        ///</summary>
        [Test]
        public void GetRoleTest()
        {
            LoadSettingFile();
            string role = "output user";
            Param.SetRole(role);
            string actual = Param.GetRole();
            Assert.AreEqual(role, actual);
        }

        /// <summary>
        ///A test for SetRole
        ///</summary>
        [Test]
        public void SetRoleTest()
        {
            string role = "Admin";
            Param.SetRole(role);
            string actual = Param.GetRole();
            Assert.AreEqual(role, actual);
        }

        /// <summary>
        ///A test for Name
        ///</summary>
        [Test]
        public void NameTest()
        {
            string newnode = "NewNode";
            string expected = "childNode";

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlFile );
            XmlNode node = xmlDocument.CreateNode(XmlNodeType.Element, newnode, "");

            XmlAttribute xmlAttrib = xmlDocument.CreateAttribute("name");
            xmlAttrib.Value = expected;
            node.Attributes.Append(xmlAttrib);

            string actual;
            actual = Param.Name(node);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for AttrValue
        ///</summary>
        [Test]
        public void AttrValueTest()
        {
            string newnode = "NewNode";
            string expected = "attribValue";

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlFile);
            XmlNode node = xmlDocument.CreateNode(XmlNodeType.Element, newnode, "");

            XmlAttribute xmlAttrib = xmlDocument.CreateAttribute("newAttrib");
            xmlAttrib.Value = expected;
            node.Attributes.Append(xmlAttrib);

            string actual = Param.AttrValue(node, "newAttrib");
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SetValue
        ///</summary>
        [Test]
        public void SetValueTest()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlFile);

            string id = "LayoutSelected";
            string val = "Draft";
            Param.SetValue(id, val);
            string actual = Param.Value[id];
            Assert.AreEqual(val, actual);
        }

        /// <summary>
        ///A test for GetElemByName
        ///</summary>
        [Test]
        public void GetElemByNameTest()
        {
            string path = "//styles/paper/style";
            string name = "Draft"; //
            string elem = "description"; //
            string expected = "Single column draft 11pt A4 with drop capital";
            string actual;
            actual = Param.GetElemByName(path, name, elem);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetListofAttr
        ///</summary>
        [Test]
        public void GetListofAttrTest()
        {
            string path = "//roles/role";
            string attr = "name";
            List<string> expected = new List<string>();
            expected.Add("Output User");
            expected.Add("Project Coordinator");
            expected.Add("Consultant");
            expected.Add("System Designer");

            List<string> actual = Param.GetListofAttr(path, attr);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetItems
        ///</summary>
        [Test]
        public void GetItemsTest()
        {
            string path = "//roles/role";
            XmlNodeList actual;
            actual = Param.GetItems(path);
            Assert.IsTrue(actual.Count == 4);
        }

        /// <summary>
        ///A test for GetAttrSummary
        ///</summary>
        [Test]
        public void GetAttrSummaryTest()
        {
            string path = "//roles/role";
            string attr = "icon";
            string expected =
                "Output User: Graphic/user.png   Project Coordinator: Graphic/coordinator.png   Consultant: Graphic/beginner.png   System Designer: Graphic/designer.png   ";
            string actual;
            actual = Param.GetAttrSummary(path, attr);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetAttrByName
        ///</summary>
        [Test]
        public void GetAttrByNameTest()
        {
            string path = "//roles/role";
            string attr = "icon";
            string name = "Output User";
            string expected = "Graphic/user.png";
            string actual;
            actual = Param.GetAttrByName(path, name, attr);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for AddAttrValue
        ///</summary>
        [Test]
        public void AddAttrValueTest()
        {
            XmlDocument xmlDocument = xmlMap;
            xmlDocument.Load(xmlFile);
            XmlNode node = xmlDocument.CreateNode(XmlNodeType.Element, "newNode", "");
            string attrTag = "newAttrib";
            string val = "newValue";
            //XmlAttribute xmlAttribute = xmlDocument.CreateAttribute("newAttrib1");
            //node.Attributes.Append(xmlAttribute);

            Param.AddAttrValue(node, attrTag, val);
            XmlAttribute actual = node.Attributes[attrTag];
            Assert.IsTrue(actual.Value == val);
        }

        /// <summary>
        ///A test for TaskSheet
        ///</summary>
        [Test]
        public void TaskSheetTest()
        {
            LoadSettingFile();
            string task = "Village check";
            string expected = "Draft";
            string actual;
            actual = Param.TaskSheet(task);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetBitmapTypeTest()
        {
            Common.ProgBase = _supportPath;
            var iconPath = Common.FromRegistry(Value[MissingIcon]);
            var attributes = File.GetAttributes(iconPath);
            try
            {
                File.SetAttributes(iconPath, FileAttributes.ReadOnly);
            }
            catch (Exception)   // Normally the file is read only on Windows Vista & 7
            {
            }
            var bmp = GetBitmap(iconPath);  // Will throw an error if opened for ReadWrite
            var type = bmp.GetType();
            Assert.AreEqual(typeof(Bitmap),type);
            try
            {
                File.SetAttributes(iconPath, attributes);
            }
            catch (Exception)   // Normally the file is read only on Windows Vista & 7
            {
            }
        }

        #region private Method
        private void LoadSettingFile()
        {
            xmlFile = Common.PathCombine(_inputBasePath, _StyleSettings);
            Param.LoadValues(xmlFile);
        }
        #endregion

#if UNFINISHED
    /// <summary>
    ///A test for ValidationCallBack
    ///</summary>
        [Test]
        //[DeploymentItem("CssSettings.dll")]
        public void ValidationCallBackTest()
        {
            object sender = null; // TODO: Initialize to an appropriate value
            ValidationEventArgs args = null; // TODO: Initialize to an appropriate value
            Param_Accessor.ValidationCallBack(sender, args);
            // TODO: A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for validatexml
        ///</summary>
        [Test]
        //[DeploymentItem("CssSettings.dll")]
        public void validatexmlTest()
        {
            string inputFile = string.Empty; // TODO: Initialize to an appropriate value
            XmlValidatingReader expected = null; // TODO: Initialize to an appropriate value
            XmlValidatingReader actual;
            actual = Param_Accessor.validatexml(inputFile);
            Assert.AreEqual(expected, actual);
            // TODO: Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for TaskSheet
        ///</summary>
        [Test]
        public void TaskSheetTest()
        {
            string task = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = Param.TaskSheet(task);
            Assert.AreEqual(expected, actual);
            // TODO: Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for StylePath
        ///</summary>
        [Test]
        public void StylePathTest1()
        {
            string sheet = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = Param.StylePath(sheet);
            Assert.AreEqual(expected, actual);
            // TODO: Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for StylePath
        ///</summary>
        [Test]
        public void StylePathTest()
        {
            string sheet = string.Empty; // TODO: Initialize to an appropriate value
            FileAccess fa = new FileAccess(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = Param.StylePath(sheet, fa);
            Assert.AreEqual(expected, actual);
            // TODO: Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SetValue
        ///</summary>
        [Test]
        public void SetValueTest()
        {
            string id = string.Empty; // TODO: Initialize to an appropriate value
            string val = string.Empty; // TODO: Initialize to an appropriate value
            Param.SetValue(id, val);
            // TODO: A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SetupHelp
        ///</summary>
        [Test]
        public void SetupHelpTest()
        {
            Control f = null; // TODO: Initialize to an appropriate value
            Param.SetupHelp(f);
            // TODO: A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SetRole
        ///</summary>
        [Test]
        public void SetRoleTest()
        {
            string role = string.Empty; // TODO: Initialize to an appropriate value
            Param.SetRole(role);
            // TODO: A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SetAttrValue
        ///</summary>
        [Test]
        public void SetAttrValueTest()
        {
            XmlNode node = null; // TODO: Initialize to an appropriate value
            string name = string.Empty; // TODO: Initialize to an appropriate value
            string val = string.Empty; // TODO: Initialize to an appropriate value
            Param.SetAttrValue(node, name, val);
            // TODO: A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SaveStyleCategories
        ///</summary>
        [Test]
        public void SaveStyleCategoriesTest()
        {
            string style = string.Empty; // TODO: Initialize to an appropriate value
            string cat = string.Empty; // TODO: Initialize to an appropriate value
            string opt = string.Empty; // TODO: Initialize to an appropriate value
            string val = string.Empty; // TODO: Initialize to an appropriate value
            Param.SaveStyleCategories(style, cat, opt, val);
            // TODO: A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SaveSheet
        ///</summary>
        [Test]
        public void SaveSheetTest()
        {
            string sheet = string.Empty; // TODO: Initialize to an appropriate value
            string description = string.Empty; // TODO: Initialize to an appropriate value
            Param.SaveSheet(sheet, description);
            // TODO: A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SaveSettings
        ///</summary>
        [Test]
        public void SaveSettingsTest()
        {
            TableLayoutPanel tlp = null; // TODO: Initialize to an appropriate value
            Param.SaveSettings(tlp);
            // TODO: A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SaveCategories
        ///</summary>
        [Test]
        public void SaveCategoriesTest()
        {
            TreeView tv = null; // TODO: Initialize to an appropriate value
            Param.SaveCategories(tv);
            // TODO: A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for NewIcon
        ///</summary>
        [Test]
        public void NewIconTest()
        {
            string name = string.Empty; // TODO: Initialize to an appropriate value
            string css = string.Empty; // TODO: Initialize to an appropriate value
            Param.NewIcon(name, css);
            // TODO: A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Name
        ///</summary>
        [Test]
        public void NameTest()
        {
            XmlNode node = null; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = Param.Name(node);
            Assert.AreEqual(expected, actual);
            // TODO: Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for LoadValues
        ///</summary>
        [Test]
        public void LoadValuesTest()
        {
            string path = string.Empty; // TODO: Initialize to an appropriate value
            XmlNode expected = null; // TODO: Initialize to an appropriate value
            XmlNode actual;
            actual = Param.LoadValues(path);
            Assert.AreEqual(expected, actual);
            // TODO: Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for LoadStyleCategories
        ///</summary>
        [Test]
        public void LoadStyleCategoriesTest()
        {
            string style = string.Empty; // TODO: Initialize to an appropriate value
            string cat = string.Empty; // TODO: Initialize to an appropriate value
            string opt = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = Param.LoadStyleCategories(style, cat, opt);
            Assert.AreEqual(expected, actual);
            // TODO: Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for LoadSettings
        ///</summary>
        [Test]
        public void LoadSettingsTest()
        {
            Param.LoadSettings();
            // TODO: A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for LoadImages
        ///</summary>
        [Test]
        public void LoadImagesTest()
        {
            ListView lv = null; // TODO: Initialize to an appropriate value
            Param.LoadImages(lv);
            // TODO: A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for LoadImageList
        ///</summary>
        [Test]
        public void LoadImageListTest()
        {
            Param.LoadImageList();
            // TODO: A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for LoadIconMap
        ///</summary>
        [Test]
        //[DeploymentItem("CssSettings.dll")]
        public void LoadIconMapTest()
        {
            Param_Accessor.LoadIconMap();
            // TODO: A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for LoadFeatures
        ///</summary>
        [Test]
        public void LoadFeaturesTest()
        {
            string path = string.Empty; // TODO: Initialize to an appropriate value
            TreeView tv = null; // TODO: Initialize to an appropriate value
            List<string> features = null; // TODO: Initialize to an appropriate value
            Param.LoadFeatures(path, tv, features);
            // TODO: A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for LoadDictionary
        ///</summary>
        [Test]
        //[DeploymentItem("CssSettings.dll")]
        public void LoadDictionaryTest()
        {
            IDictionary<string, string> target = null; // TODO: Initialize to an appropriate value
            string xmlPath = string.Empty; // TODO: Initialize to an appropriate value
            string attr = string.Empty; // TODO: Initialize to an appropriate value
            Param_Accessor.LoadDictionary(target, xmlPath, attr);
            // TODO: A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for LoadCategories
        ///</summary>
        [Test]
        public void LoadCategoriesTest()
        {
            string path = string.Empty; // TODO: Initialize to an appropriate value
            TreeView tv = null; // TODO: Initialize to an appropriate value
            Param.LoadCategories(path, tv);
            // TODO: A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for InsertOption
        ///</summary>
        [Test]
        public void InsertOptionTest()
        {
            XmlNode feature = null; // TODO: Initialize to an appropriate value
            string option = string.Empty; // TODO: Initialize to an appropriate value
            string css = string.Empty; // TODO: Initialize to an appropriate value
            string icon = string.Empty; // TODO: Initialize to an appropriate value
            Param.InsertOption(feature, option, css, icon);
            // TODO: A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for InsertKind
        ///</summary>
        [Test]
        public void InsertKindTest()
        {
            string kind = string.Empty; // TODO: Initialize to an appropriate value
            string name = string.Empty; // TODO: Initialize to an appropriate value
            XmlNode expected = null; // TODO: Initialize to an appropriate value
            XmlNode actual;
            actual = Param.InsertKind(kind, name);
            Assert.AreEqual(expected, actual);
            // TODO: Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetTagIcon
        ///</summary>
        [Test]
        public void GetTagIconTest()
        {
            XmlNode node = null; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = Param.GetTagIcon(node);
            Assert.AreEqual(expected, actual);
            // TODO: Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetTagFile
        ///</summary>
        [Test]
        public void GetTagFileTest()
        {
            XmlNode node = null; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = Param.GetTagFile(node);
            Assert.AreEqual(expected, actual);
            // TODO: Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetRole
        ///</summary>
        [Test]
        public void GetRoleTest()
        {
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = Param.GetRole();
            Assert.AreEqual(expected, actual);
            // TODO: Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetListofAttr
        ///</summary>
        [Test]
        public void GetListofAttrTest()
        {
            string path = string.Empty; // TODO: Initialize to an appropriate value
            string attr = string.Empty; // TODO: Initialize to an appropriate value
            List<string> expected = null; // TODO: Initialize to an appropriate value
            List<string> actual;
            actual = Param.GetListofAttr(path, attr);
            Assert.AreEqual(expected, actual);
            // TODO: Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetItems
        ///</summary>
        [Test]
        public void GetItemsTest()
        {
            string path = string.Empty; // TODO: Initialize to an appropriate value
            XmlNodeList expected = null; // TODO: Initialize to an appropriate value
            XmlNodeList actual;
            actual = Param.GetItems(path);
            Assert.AreEqual(expected, actual);
            // TODO: Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetElemByName
        ///</summary>
        [Test]
        public void GetElemByNameTest()
        {
            string path = string.Empty; // TODO: Initialize to an appropriate value
            string name = string.Empty; // TODO: Initialize to an appropriate value
            string elem = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = Param.GetElemByName(path, name, elem);
            Assert.AreEqual(expected, actual);
            // TODO: Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetAttrSummary
        ///</summary>
        [Test]
        public void GetAttrSummaryTest()
        {
            string path = string.Empty; // TODO: Initialize to an appropriate value
            string attr = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = Param.GetAttrSummary(path, attr);
            Assert.AreEqual(expected, actual);
            // TODO: Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetAttrByName
        ///</summary>
        [Test]
        public void GetAttrByNameTest()
        {
            string path = string.Empty; // TODO: Initialize to an appropriate value
            string name = string.Empty; // TODO: Initialize to an appropriate value
            string attr = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = Param.GetAttrByName(path, name, attr);
            Assert.AreEqual(expected, actual);
            // TODO: Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for FromProg
        ///</summary>
        [Test]
        public void FromProgTest()
        {
            string s = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = Param.FromProg(s);
            Assert.AreEqual(expected, actual);
            // TODO: Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for FilterStyles
        ///</summary>
        [Test]
        public void FilterStylesTest()
        {
            List<string> styleList = null; // TODO: Initialize to an appropriate value
            List<string> expected = null; // TODO: Initialize to an appropriate value
            List<string> actual;
            actual = Param.FilterStyles(styleList);
            Assert.AreEqual(expected, actual);
            // TODO: Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CopySchemaIfNecessary
        ///</summary>
        [Test]
        //[DeploymentItem("CssSettings.dll")]
        public void CopySchemaIfNecessaryTest()
        {
            Param_Accessor.CopySchemaIfNecessary();
            // TODO: A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for AddAttrValue
        ///</summary>
        [Test]
        //[DeploymentItem("CssSettings.dll")]
        public void AddAttrValueTest()
        {
            XmlNode node = null; // TODO: Initialize to an appropriate value
            string attrTag = string.Empty; // TODO: Initialize to an appropriate value
            string val = string.Empty; // TODO: Initialize to an appropriate value
            Param_Accessor.AddAttrValue(node, attrTag, val);
            // TODO: A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for AddBmpIcon
        ///</summary>
        [Test]
        //[DeploymentItem("CssSettings.dll")]
        public void AddBmpIconTest()
        {
            FileSystemInfo ico = null; // TODO: Initialize to an appropriate value
            Image bmp = null; // TODO: Initialize to an appropriate value
            Param_Accessor.AddBmpIcon(ico, bmp);
            // TODO: A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ApplyVariables
        ///</summary>
        [Test]
        //[DeploymentItem("CssSettings.dll")]
        public void ApplyVariablesTest()
        {
            Param_Accessor.ApplyVariables();
            // TODO: A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for AttrValue
        ///</summary>
        [Test]
        public void AttrValueTest()
        {
            XmlNode node = null; // TODO: Initialize to an appropriate value
            string attr = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = Param.AttrValue(node, attr);
            Assert.AreEqual(expected, actual);
            // TODO: Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Check
        ///</summary>
        [Test]
        public void CheckTest()
        {
            TreeNode node = null; // TODO: Initialize to an appropriate value
            Param.Check(node);
            // TODO: A method that does not return a value cannot be verified.");
        }
#endif // UNFINISHED
    }
}