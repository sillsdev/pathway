// --------------------------------------------------------------------------------------------
// <copyright file="ProjectInformationTest.cs" from='2009' to='2014' company='SIL International'>
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
using NUnit.Framework;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using SIL.Tool;
using Assert = NUnit.Framework.Assert;

namespace Test.PsTool
{
    /// <summary>
    ///This is a test class for ProjectInformationTest and is intended
    ///to contain all ProjectInformationTest Unit Tests
    ///</summary>
    [TestFixture]
    public class ProjectInformationTest
    {
        private XmlDocument _doc;
        PublicationInformation _target;
        XmlDocument actualDocument;
        private XmlNode _expected;
        private string _projectFilePath;
        public string _node;

        /// <summary>
        /// Turn off debug.assert messages for unit tests
        /// </summary>
        [SetUp]
        protected void SetUp()
        {
            _doc = Common.DeclareXMLDocument(false);
            _target = new PublicationInformation();
            actualDocument = Common.DeclareXMLDocument(false);
            LoadInputDocument("Dictionary1.de");
            var outputPath = Common.PathCombine(GetTestPath(), "Output");
            if (Directory.Exists(outputPath))
                Directory.Delete(outputPath, true);
            Directory.CreateDirectory(outputPath);
            Common.Testing = true;
        }

        /// <summary>
        ///A test for GetRootNode
        ///</summary>
        [Test]
        public void GetRootNodeTest()
        {
            _node ="<SolutionExplorer><File Name=\"main.xhtml\" Type=\"File\" Visible=\"True\" Default=\"True\" /><File Name=\"Final-job1.css\" Type=\"File\" Visible=\"True\" Default=\"True\" /><File Name=\"Final-job2.css\" Type=\"File\" Visible=\"True\" Default=\"False\" /></SolutionExplorer><DocumentSettings><Sections><Section No=\"1\" Name=\"Cover\" Value=\"none\" /><Section No=\"2\" Name=\"Title\" Value=\"none\" /><Section No=\"3\" Name=\"Rights\" Value=\"none\" /><Section No=\"4\" Name=\"Introduction\" Value=\"none\" /><Section No=\"5\" Name=\"List\" Value=\"none\" /><Section No=\"6\" Name=\"Orthography\" Value=\"none\" /><Section No=\"7\" Name=\"Phonology\" Value=\"none\" /><Section No=\"8\" Name=\"Grammar\" Value=\"none\" /><Section No=\"9\" Name=\"Main\" Value=\"main.xhtml\" /><Section No=\"10\" Name=\"Index\" Value=\"none\" /><Section No=\"11\" Name=\"Reversal\" Value=\"none\" /><Section No=\"12\" Name=\"Bibliography\" Value=\"none\" /></Sections></DocumentSettings><PropertyGroup><Creation><CreatedOn>30-Jun-2009 10:49:03</CreatedOn><CreatedBy>James</CreatedBy></Creation><Modification><ModifiedOn>06-Aug-2009 19:19:54</ModifiedOn><ModifiedBy>James</ModifiedBy></Modification></PropertyGroup>";
            _expected = LoadXmlDocument(_node, true);
            XmlElement actual = _target.GetRootNode();
            Assert.AreEqual(_expected.InnerXml, actual.InnerXml);
        }

        [Test]
        public void SearchNodeTest()
        {
            _node = "<Sections><Section No=\"1\" Name=\"Cover\" Value=\"none\" /><Section No=\"2\" Name=\"Title\" Value=\"none\" /><Section No=\"3\" Name=\"Rights\" Value=\"none\" /><Section No=\"4\" Name=\"Introduction\" Value=\"none\" /><Section No=\"5\" Name=\"List\" Value=\"none\" /><Section No=\"6\" Name=\"Orthography\" Value=\"none\" /><Section No=\"7\" Name=\"Phonology\" Value=\"none\" /><Section No=\"8\" Name=\"Grammar\" Value=\"none\" /><Section No=\"9\" Name=\"Main\" Value=\"main.xhtml\" /><Section No=\"10\" Name=\"Index\" Value=\"none\" /><Section No=\"11\" Name=\"Reversal\" Value=\"none\" /><Section No=\"12\" Name=\"Bibliography\" Value=\"none\" /></Sections>";
            _expected = LoadXmlDocument(_node, false);
            string xPath = "//Project/DocumentSettings/Sections";
            XmlNode actual = _target.SearchNode(xPath);
            Assert.AreEqual(_expected.InnerXml, actual.InnerXml);
            //Assert.AreEqual(_expected, actual);
        }

        /// <summary>
        ///A test for IsFileExist
        ///</summary>
        [Test]
        public void IsFileExistTest()
        {
            _node = "<SolutionExplorer><File Name=\"main.xhtml\" Type=\"File\" Visible=\"True\" Default=\"True\" /><File Name=\"Final-job1.css\" Type=\"File\" Visible=\"True\" Default=\"True\" /><File Name=\"Final-job2.css\" Type=\"File\" Visible=\"True\" Default=\"False\" /></SolutionExplorer><DocumentSettings><Sections><Section No=\"1\" Name=\"Cover\" Value=\"none\" /><Section No=\"2\" Name=\"Title\" Value=\"none\" /><Section No=\"3\" Name=\"Rights\" Value=\"none\" /><Section No=\"4\" Name=\"Introduction\" Value=\"none\" /><Section No=\"5\" Name=\"List\" Value=\"none\" /><Section No=\"6\" Name=\"Orthography\" Value=\"none\" /><Section No=\"7\" Name=\"Phonology\" Value=\"none\" /><Section No=\"8\" Name=\"Grammar\" Value=\"none\" /><Section No=\"9\" Name=\"Main\" Value=\"main.xhtml\" /><Section No=\"10\" Name=\"Index\" Value=\"none\" /><Section No=\"11\" Name=\"Reversal\" Value=\"none\" /><Section No=\"12\" Name=\"Bibliography\" Value=\"none\" /></Sections></DocumentSettings><PropertyGroup><Creation><CreatedOn>30-Jun-2009 10:49:03</CreatedOn><CreatedBy>James</CreatedBy></Creation><Modification><ModifiedOn>06-Aug-2009 19:19:54</ModifiedOn><ModifiedBy>James</ModifiedBy></Modification></PropertyGroup>";
            LoadXmlDocument(_node, true);
            bool actual = _target.IsFileExist("main.xhtml", "True");
            Assert.AreEqual(true, actual);
        }

        /// <summary>
        ///A test for GetSolutionExplorerNode
        ///</summary>
        [Test]
        public void GetSolutionExplorerNodeTest()
        {
            _node = "<SolutionExplorer><File Name=\"main.xhtml\" Type=\"File\" Visible=\"True\" Default=\"True\" /><File Name=\"Final-job1.css\" Type=\"File\" Visible=\"True\" Default=\"True\" /><File Name=\"Final-job2.css\" Type=\"File\" Visible=\"True\" Default=\"False\" /></SolutionExplorer>";
            _expected = LoadXmlDocument(_node, false);
            XmlNode actual = _target.GetSolutionExplorerNode();
            Assert.AreEqual(_expected.InnerXml, actual.InnerXml);

        }

        /// <summary>
        ///A test for GetDictionarySettingNode
        ///</summary>
        [Test]
        public void GetDictionarySettingNodeTest()
        {
            _node = "<DocumentSettings><Sections><Section No=\"1\" Name=\"Cover\" Value=\"none\" /><Section No=\"2\" Name=\"Title\" Value=\"none\" /><Section No=\"3\" Name=\"Rights\" Value=\"none\" /><Section No=\"4\" Name=\"Introduction\" Value=\"none\" /><Section No=\"5\" Name=\"List\" Value=\"none\" /><Section No=\"6\" Name=\"Orthography\" Value=\"none\" /><Section No=\"7\" Name=\"Phonology\" Value=\"none\" /><Section No=\"8\" Name=\"Grammar\" Value=\"none\" /><Section No=\"9\" Name=\"Main\" Value=\"main.xhtml\" /><Section No=\"10\" Name=\"Index\" Value=\"none\" /><Section No=\"11\" Name=\"Reversal\" Value=\"none\" /><Section No=\"12\" Name=\"Bibliography\" Value=\"none\" /></Sections></DocumentSettings>";
            _expected = LoadXmlDocument(_node, false);
            XmlNode actual = _target.GetDictionarySettingNode();
            Assert.AreEqual(_expected.InnerXml, actual.InnerXml);
        }

        /// <summary>
        ///A test for FindFileTypeExist
        ///</summary>
        [Test]
        public void FindFileTypeExistTest()
        {
            TreeNode myTree = new TreeNode();
            myTree.Nodes.Add("Project", "Project");
            myTree.Nodes.Add("Main.xhtml", "Main.xhtml");
            myTree.Nodes.Add("FlexRev.xhtml", "FlexRev.xhtml");
            _node = "<SolutionExplorer><File Name=\"main.xhtml\" Type=\"File\" Visible=\"True\" Default=\"True\" /><File Name=\"Final-job1.css\" Type=\"File\" Visible=\"True\" Default=\"True\" /><File Name=\"Final-job2.css\" Type=\"File\" Visible=\"True\" Default=\"False\" /></SolutionExplorer><DocumentSettings><Sections><Section No=\"1\" Name=\"Cover\" Value=\"none\" /><Section No=\"2\" Name=\"Title\" Value=\"none\" /><Section No=\"3\" Name=\"Rights\" Value=\"none\" /><Section No=\"4\" Name=\"Introduction\" Value=\"none\" /><Section No=\"5\" Name=\"List\" Value=\"none\" /><Section No=\"6\" Name=\"Orthography\" Value=\"none\" /><Section No=\"7\" Name=\"Phonology\" Value=\"none\" /><Section No=\"8\" Name=\"Grammar\" Value=\"none\" /><Section No=\"9\" Name=\"Main\" Value=\"main.xhtml\" /><Section No=\"10\" Name=\"Index\" Value=\"none\" /><Section No=\"11\" Name=\"Reversal\" Value=\"none\" /><Section No=\"12\" Name=\"Bibliography\" Value=\"none\" /></Sections></DocumentSettings><PropertyGroup><Creation><CreatedOn>30-Jun-2009 10:49:03</CreatedOn><CreatedBy>James</CreatedBy></Creation><Modification><ModifiedOn>06-Aug-2009 19:19:54</ModifiedOn><ModifiedBy>James</ModifiedBy></Modification></PropertyGroup>";
            LoadXmlDocument(_node, true);
            // Existing Extenstion
            bool actual = _target.FindFileTypeExist(myTree, ".xhtml");
            Assert.AreEqual(true, actual);

            // Non Existing Extenstion
            actual = _target.FindFileTypeExist(myTree, ".Ext");
            Assert.AreEqual(false, actual);
        }

        /// <summary>
        ///A test for DEGetAttribute
        ///</summary>
        [Test]
        public void DEGetAttributeTest()
        {
            string fileName = "DictionarySave.de";
            string sourceFile = GetFileNameWithPath(fileName);
            fileName = "dummy.de"; // nothing to do with xml, but addfiletoxml uses this
            string output = GetFileNameWithOutputPath(fileName);
            CopyToOutput(sourceFile, output);
            _target.LoadProjectFile(output);

            string nodeName = "Section";
            string attributeName = "No";
            string expected = "1";
            string actual = _target.DEGetAttribute(nodeName, attributeName);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for LoadProjectFile
        ///</summary>
        [Test]
        public void LoadProjectFileTest()
        {
            string projFile = GetFileNameWithPath("Test.xhtml");
            _target.LoadProjectFile(projFile);
            XmlDocument xmlDocument = Common.DeclareXMLDocument(false);
            xmlDocument.Load(projFile);
            Assert.AreEqual(xmlDocument.InnerXml, _target.ProjectDeXML.InnerXml);
        }

        /// <summary>
        ///A test for DicExplorerRemoveDefault
        ///</summary>
        [Test]
        public void DicExplorerRemoveDefaultTest()
        {
            _node = "<SolutionExplorer><File Name=\"Final-job1.css\" Default=\"False\" /></SolutionExplorer>";
            _expected = LoadXmlDocument(_node, false);

            _node = "<SolutionExplorer><File Name=\"Final-job1.css\"  Default=\"True\" /></SolutionExplorer>";
            XmlNode input = LoadXmlDocument(_node, false);

            string[] fileExtension = new[] {".css"};
            _target.DicExplorerRemoveDefault(input, fileExtension);

            Assert.AreEqual(_expected.InnerXml, input.InnerXml);
        }

        /// <summary>
        ///A test for ProjectProperty
        ///</summary>
        [Test]
        public void ProjectPropertyTest()
        {
            string mode = "new";
            string fileName = "Dictionary1.de";
            string sourceFile = GetFileNameWithPath(fileName);
            string output = GetFileNameWithOutputPath(fileName);
            CopyToOutput(sourceFile, output);

            _target.LoadProjectFile(output);
            _target.ProjectProperty(mode);

            //TODO - CLARIFY TIME MODIFICATION
        }

        /// <summary>
        ///A test for OpenProjectFile
        ///</summary>
        [Test]
        public void OpenProjectFileTest()
        {
            string fileName = "Dictionary1.de";
            string sourceFile = GetFileNameWithPath(fileName);
            string output = GetFileNameWithOutputPath(fileName);
            CopyToOutput(sourceFile, output);

            _target.LoadProjectFile(output);
            _target.ProjectMode = string.Empty;
            TreeView dictionaryExplorer = new TreeView();
            _target.OpenProjectFile(dictionaryExplorer);
            // It calls againt ProjectProperty
            //TODO1 - CLARIFY TIME MODIFICATION
        }

        /// <summary>
        ///A test for SaveProject
        ///</summary>
        [Test]
        public void SaveProjectTest()
        {
            string fileName = "DictionarySave.de";
            string sourceFile = GetFileNameWithPath(fileName);
            string output = GetFileNameWithOutputPath(fileName);
            string expected = GetFileNameWithExpectedPath(fileName);
            CopyToOutput(sourceFile, output);
            _target.LoadProjectFile(output);
            _target.RemoveFile("main.xhtml");
            _target.SaveProject();

            _doc.Load(expected);
            XmlNode xmlNodeExpected = _doc.DocumentElement;

            _doc.Load(output);
            XmlNode xmlNodeOutput = _doc.DocumentElement;

            Assert.AreEqual(xmlNodeExpected.InnerXml, xmlNodeOutput.InnerXml);
        }

        /// <summary>
        ///A test for RemoveFile
        ///</summary>
        [Test]
        public void RemoveFileTest()
        {
            string fileName = "DictionaryRemove.de";
            string sourceFile = GetFileNameWithPath(fileName);
            string output = GetFileNameWithOutputPath(fileName);
            string expected = GetFileNameWithExpectedPath(fileName);
            CopyToOutput(sourceFile, output);
            _target.LoadProjectFile(output);
            _target.RemoveFile("main.xhtml");
            _target.SaveProject();

            _doc.Load(expected);
            XmlNode xmlNodeExpected = _doc.DocumentElement;

            _doc.Load(output);
            XmlNode xmlNodeOutput = _doc.DocumentElement;

            Assert.AreEqual(xmlNodeExpected.InnerXml, xmlNodeOutput.InnerXml);
        }

        /// <summary>
        ///A test for PopulateDicExplorerNode
        ///</summary>
        [Test]
        public void PopulateDicExplorerNodeTest()
        {
            TreeNode expectedNode = new TreeNode();
            TreeNode childNode = new TreeNode();
            childNode.Text = "main.xhtml";
            childNode.ImageIndex = 8;
            childNode.SelectedImageIndex = 2;
            childNode.Tag = Common.FileType.File;
            expectedNode.Nodes.Add(childNode);

            childNode = new TreeNode();
            childNode.Text = "Final-job1.css";
            childNode.ImageIndex = 7;
            childNode.SelectedImageIndex = 7;
            childNode.Tag = Common.FileType.File;
            expectedNode.Nodes.Add(childNode);

            childNode = new TreeNode();
            childNode.Text = "Final-job2.css";
            childNode.ImageIndex = 2;
            //childNode.SelectedImageIndex = 7;
            childNode.Tag = Common.FileType.File;
            expectedNode.Nodes.Add(childNode);


            TreeNode outputNode = new TreeNode();
            string fileName = "Dictionary1.de";
            string sourceFile = GetFileNameWithPath(fileName);

            _doc.Load(sourceFile);
            XmlNode subNode = _doc.DocumentElement;
            _target.ProjectMode = string.Empty;
            _target.PopulateDicExplorerNode(outputNode, subNode.FirstChild);

            //Assert.AreEqual(expectedNode.FirstNode, outputNode.FirstNode);

        }

        /// <summary>
        ///A test for PopulateDicExplorer
        ///</summary>
        [Test]
        public void PopulateDicExplorerTest()
        {
            TreeView outputTv = new TreeView();
            TreeNode expectedNode = new TreeNode();
            TreeNode childNode = new TreeNode();
            childNode.Text = "main.xhtml";
            childNode.ImageIndex = 8;
            childNode.SelectedImageIndex = 2;
            childNode.Tag = Common.FileType.File;
            expectedNode.Nodes.Add(childNode);

            childNode = new TreeNode();
            childNode.Text = "Final-job1.css";
            childNode.ImageIndex = 7;
            childNode.SelectedImageIndex = 7;
            childNode.Tag = Common.FileType.File;
            expectedNode.Nodes.Add(childNode);

            childNode = new TreeNode();
            childNode.Text = "Final-job2.css";
            childNode.ImageIndex = 2;
            //childNode.SelectedImageIndex = 7;
            childNode.Tag = Common.FileType.File;
            expectedNode.Nodes.Add(childNode);


            string fileName = "Dictionary1.de";
            string sourceFile = GetFileNameWithPath(fileName);

	        var xr = XmlReader.Create(sourceFile);
            _doc.Load(xr);
			xr.Close();
            _target.ProjectMode = string.Empty;
            _target.LoadProjectFile(sourceFile);
            _target.PopulateDicExplorer(outputTv);

            //Assert.AreEqual(expectedTreeView, outputTv);

        }

        /// <summary>
        ///A test for DESetAttribute
        ///</summary>
        [Test]
        public void DESetAttributeTest()
        {
            string fileName = "Dictionary1.de";
            string sourceFile = GetFileNameWithPath(fileName);
            string output = GetFileNameWithOutputPath("DictionarySetAttrib.de");
            CopyToOutput(sourceFile, output);

            _target.LoadProjectFile(output);

            string nodeName = "Project";
            string attributeName = "ShowError";
            string attributeValue = "False";
            _target.DESetAttribute("//" + nodeName, attributeName, attributeValue);

            string expected = "False";
            string actual = _target.DEGetAttribute(nodeName, attributeName);
            Assert.AreEqual(expected, actual);


        }

        /// <summary>
        ///A test for SortFolderFileTypes
        ///</summary>
        [Test]
        public void SortFolderFileTypesTest()
        {
            string fileName = "SortFolderFiles.de";
            string sourceFile = GetFileNameWithPath(fileName);
            string output = GetFileNameWithOutputPath(fileName);
            string expected = GetFileNameWithExpectedPath(fileName);
            CopyToOutput(sourceFile, output);

            string projectFile = output;
            _target.SortFolderFileTypes(projectFile);
            TextFileAssert.AreEqual(expected,output);

        }



        //// TODO - It has lot of private variables - it should be analysed
        ///// <summary>
        /////A test for AddImageFiles
        /////</summary>
        ////[Test]
        //public void AddImageFilesTest()
        //{
        //    string filename = string.Empty;
        //    string projectName = string.Empty;
        //    _target.AddImageFiles(filename, projectName);
        //}

        ///// <summary>
        /////A test for CreateProjectFile
        /////</summary>
        ////[Test]
        //public void CreateProjectFileTest()
        //{
        //    TreeView dictionaryExplorer = new TreeView();
        //    _target.CreateProjectFile(dictionaryExplorer);
        //}


    #region private Methods
        private XmlNode LoadXmlDocument(string node, bool rootAdd)
        {
            XmlNode xmlNode;
            if (rootAdd)
            {
                _doc.LoadXml("<root>" + node + "</root>");
            }
            else
            {
                _doc.LoadXml(node);
            }
            xmlNode = _doc.DocumentElement;
            return xmlNode;
        }

        private void LoadInputDocument(string fileName)
        {
            _projectFilePath = GetFileNameWithPath(fileName);
            actualDocument.Load(_projectFilePath);
            _target.ProjectDeXML = actualDocument;
        }

        private string GetFileNameWithPath(string fileName)
        {
            fileName = PathPart.Bin(Environment.CurrentDirectory,"/PsTool/TestFiles/InputFiles/" + fileName);
            return fileName;
        }
        private static string GetFileNameWithOutputPath(string fileName)
        {
            fileName = GetPath("Output", fileName);
            return fileName;
        }
        private static string GetFileNameWithExpectedPath(string fileName)
        {
            fileName = GetPath("Expected", fileName);
            return fileName;
        }
        private static string GetPath(string place, string filename)
        {
            string path = GetTestPath();
            path = Common.PathCombine(path, Common.PathCombine(place, filename));
            return path;
        }
        private static string GetTestPath()
        {
            string path = PathPart.Bin(Environment.CurrentDirectory, "/PsTool/TestFiles/");
            return path;
        }

        private static void CopyToOutput(string input, string output)
        {
            if (File.Exists(input))
                File.Copy(input, output, true);
        }
        #endregion
    }
}
