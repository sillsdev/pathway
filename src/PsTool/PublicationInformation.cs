// --------------------------------------------------------------------------------------------
// <copyright file="PublicationInformation.cs" from='2009' to='2014' company='SIL International'>
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

// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using System.Collections;
using SIL.Tool.Localization;

namespace SIL.Tool
{
    public interface IPublicationInformation
    {
        string DefaultXhtmlFileWithPath { get; set; }
        string DefaultCssFileWithPath { get; set; }
        string ProjectInputType { get; set; }
    }

    /// <summary>
    /// PublicationInformation Class contains the details of Dictionary/Scripture Project information
    /// and the information about the files/folders.
    /// </summary>
    public class PublicationInformation : IPublicationInformation
    {
        #region Private variable
        private TreeView _dictExplorer;
        private bool _hideFile = false;
        private bool _isLexiconExist;
        private bool _isReversalExist;
        private bool _isExtraProcessing = true;
        private bool _swapHeadword;
        private string _projectMode;
        private bool _isOpenOutput;
        private string _dictionaryPath;
        private string _fullPath = string.Empty;
        private string _subPathValue = string.Empty;
        private string _projectPath;
        private string _tempOutputFolder;
        private string _projectName;
        private string _projectFileWithPath;
        private string _projectInputType;
        private string _outputExtension;
        private string _dictionaryOutputName;
        private string _DefaultRevCssFileWithPath;
        private string _DefaultCssFileWithPath;
        private string _DefaultXhtmlFileWithPath;
        private string _hideSpaceVerseNumber;
        private XmlDocument _DeXml = new XmlDocument();
        private bool _fromPlugin = false;
        public string FinalOutput;
        public string FileToProduce = string.Empty;
        public bool MoveStyleToContent = false;
        public bool JpgPreview = false;
        public string DefaultFontName = "Times New Roman";
        public float DefaultFontSize = 12;
        private bool _isFrontMatterEnabled = false;
        private bool _isTitlePageEnabled = false;
        private bool _isODM;
        public string _headerFontName = "Times New Roman";
        private string _reversalFontName = "Times New Roman";
        public string _selectedTemplateStyle = string.Empty;

        private string _headerReferenceFormat = string.Empty;
        private string _includeXRefSymbol;
        private string _includeFootnoteSymbol;
        private string _splitFileByLetter;
        private string _mainLastFileName;
	    private bool _isAnchorInherited = false;

        #endregion

        #region public Variable
        private readonly string _userRole = string.Empty;
        public ProgressBar ProgressBar;
        public ArrayList FileSequence;
        #endregion

        #region Properties
        public bool IsOpenOutput
        {
            get { return _isOpenOutput; }
            set { _isOpenOutput = value; }
        }

        public string ProjectMode
        {
            get { return _projectMode; }
            set { _projectMode = value; }
        }

        public string DictionaryPath
        {
            get { return _dictionaryPath; }
            set { _dictionaryPath = value; }
        }

        public string TempOutputFolder
        {
            get { return _tempOutputFolder; }
            set { _tempOutputFolder = value; }
        }

        public string ProjectName
        {
            get { return _projectName; }
            set { _projectName = value; }
        }

        public string ProjectFileWithPath
        {
            get { return _projectFileWithPath; }
            set { _projectFileWithPath = value; }
        }

        public string ProjectInputType
        {
            get { return _projectInputType; }
            set { _projectInputType = value; }
        }

        public string ProjectPath
        {
            get { return _projectPath; }
            set { _projectPath = value; }
        }

        public XmlDocument ProjectDeXML
        {
            get { return _DeXml; }
            set { _DeXml = value; }
        }

        public string DefaultXhtmlFileWithPath
        {
            get { return _DefaultXhtmlFileWithPath; }
            set { _DefaultXhtmlFileWithPath = value; }
        }

        public string DefaultCssFileWithPath
        {
            get { return _DefaultCssFileWithPath; }
            set { _DefaultCssFileWithPath = value; }
        }

        public string DefaultRevCssFileWithPath
        {
            get { return _DefaultRevCssFileWithPath; }
            set { _DefaultRevCssFileWithPath = value; }
        }


        public string DictionaryOutputName
        {
            get { return _dictionaryOutputName; }
            set { _dictionaryOutputName = value; }
        }

        public bool IsReversalExist
        {
            get { return _isReversalExist; }
            set { _isReversalExist = value; }
        }

        public bool IsExtraProcessing
        {
            get { return _isExtraProcessing; }
            set { _isExtraProcessing = value; }
        }

        public bool IsLexiconSectionExist
        {
            get { return _isLexiconExist; }
            set { _isLexiconExist = value; }
        }

        public bool SwapHeadword
        {
            get { return _swapHeadword; }
            set { _swapHeadword = value; }
        }

        public bool FromPlugin
        {
            get { return _fromPlugin; }
            set { _fromPlugin = value; }
        }

        public string OutputExtension
        {
            get { return _outputExtension; }
            set { _outputExtension = value; }
        }

        public bool IsFrontMatterEnabled
        {
            get { return _isFrontMatterEnabled; }
            set { _isFrontMatterEnabled = value; }
        }

        public bool IsTitlePageEnabled
        {
            get { return _isTitlePageEnabled; }
            set { _isTitlePageEnabled = value; }
        }

        
        public string HeaderFontName
        {
            get { return _headerFontName; }
            set { _headerFontName = value; }
        }

        public string ReversalFontName
        {
            get { return _reversalFontName; }
            set { _reversalFontName = value; }
        }

        public string SelectedTemplateStyle
        {
            get { return _selectedTemplateStyle; }
            set { _selectedTemplateStyle = value; }
        }

        public string HeaderReferenceFormat
        {
            get { return _headerReferenceFormat; }
            set { _headerReferenceFormat = value; }
        }

        public string IncludeXRefSymbol
        {
            get { return _includeXRefSymbol; }
            set { _includeXRefSymbol = value; }
        }

        public string SplitFileByLetter
        {
            get { return _splitFileByLetter; }
            set { _splitFileByLetter = value; }
        }

        public string MainLastFileName
        {
            get { return _mainLastFileName; }
            set { _mainLastFileName = value; }
        }
        
        public string HideSpaceVerseNumber
        {
            get { return _hideSpaceVerseNumber; }
            set { _hideSpaceVerseNumber = value; }
        }

        public string IncludeFootnoteSymbol
        {
            get { return _includeFootnoteSymbol; }
            set { _includeFootnoteSymbol = value; }
        }

        public bool IsODM
        {
            get { return _isODM; }
            set { _isODM = value; }
        }

	    public bool IsAnchorInherited
	    {
		    get { return _isAnchorInherited; }
		    set { _isAnchorInherited = value; }
	    }

	    #endregion


        public PublicationInformation()
        {
            _isOpenOutput = true;
            FileSequence = null;
        }


        #region The Project File ".de" Related Methods

        /// <summary>
        /// Searchs the Single Node and returns the Node
        /// </summary>
        /// <param name="xPath">The Path to be searched </param>
        /// <returns>Returns the Matched Node</returns>
        public XmlNode SearchNode(string xPath)
        {
            XmlNode returnNode = null;
            try
            {
                if (GetRootNode() != null)
                    returnNode = GetRootNode().SelectSingleNode(xPath);
            }
            catch
            {
            } 
            return returnNode;
        }

        public void LoadProjectFile(string projFile)
        {
            ProjectFileWithPath = projFile;
            _DeXml.XmlResolver = FileStreamXmlResolver.GetNullResolver();
            _DeXml.Load(projFile);
        }

        /// <summary>
        /// Updates the Project Nodes for Later Versions.
        /// </summary>
        private void UpdateProjectFile()
        {
            _DeXml.XmlResolver = FileStreamXmlResolver.GetNullResolver();
            _DeXml.Load(_projectFileWithPath);
            XmlElement root = GetRootNode();
            const string xPath = "/Project/SolutionExplorer";
            XmlNode searchNode = root.SelectSingleNode(xPath);
            if (searchNode == null)
            {
                const string filePath = "/Project/File";
                XmlNodeList fileNode = root.SelectNodes(filePath);
                XmlNode newNode = _DeXml.CreateNode("element", "SolutionExplorer", "");
                if (fileNode != null)
                    foreach (XmlNode item in fileNode)
                    {
                        newNode.AppendChild(item); 
                    }
                root.AppendChild(newNode);
                _DeXml.Save(_projectFileWithPath);
                _DeXml.Load(_projectFileWithPath);
            }
        }

        /// <summary>
        /// Opens the project file .de 
        /// </summary>
        /// <param name="dictionaryExplorer">The Solution Explorer</param>
        public void OpenProjectFile(TreeView dictionaryExplorer)
        {
            UpdateProjectFile();
            _dictExplorer = dictionaryExplorer;
            XmlElement type = GetRootNode();
            _projectInputType = type.GetAttribute("Type");
            if (_projectInputType.Length == 0)
            {
                _projectInputType = "Dictionary";
            }
            ProjectProperty(_projectMode);
            PopulateDicExplorer(dictionaryExplorer);
        }

        /// <summary>
        /// Gets the Documents root node
        /// </summary>
        /// <returns>Returns the documents root node</returns>
        public XmlElement GetRootNode()
        {
            XmlElement returnValue = _DeXml.DocumentElement;
            return returnValue;
        }

        /// <summary>
        /// Gets the Document Settings Value by xpath
        /// </summary>
        /// <returns>Returns the node of Document Settings</returns>
        public XmlNode GetDictionarySettingNode()
        {
            const string xPath = "DocumentSettings";
            return SearchNode(xPath);
        }

        /// <summary>
        /// Gets the Solution Explorer Value by xpath
        /// </summary>
        /// <returns>Returns the node of Solution Explorer</returns>
        public XmlNode GetSolutionExplorerNode()
        {
            const string xPath = "SolutionExplorer";
            return SearchNode(xPath);
        }

        /// <summary>
        /// Saves the de File
        /// </summary>
        public void SaveProject()
        {
            _DeXml.Save(_projectFileWithPath);
        }
        #endregion

        #region  The UI "Solution Explorer" Related Methods

        /// <summary>
        /// Checks the file type in Solution Explorer before adding the File
        /// </summary>
        /// <param name="node">The Root Node to be searched</param>
        /// <param name="fileType">File's Extension to be searched</param>
        /// <returns>Returns True/Fals</returns>
        public bool FindFileTypeExist(TreeNode node, string fileType)
        {
            bool returnValue = false;
            try
            {
                foreach (TreeNode childNode in node.Nodes)
                {
                    if (childNode.Text.IndexOf(fileType) > 0)
                    {
                        returnValue = true;
                        break;
                    }
                }
                return returnValue;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Sets the default attribute of the Nodes to False
        /// </summary>
        /// <param name="root">The Root Node</param>
        /// <param name="fileExtension">File Extensions to be removed.</param>
        public void DicExplorerRemoveDefault(XmlNode root, string[] fileExtension)
        {

            ArrayList al = new ArrayList();
            al.AddRange(fileExtension);
            foreach (XmlNode childNode in root)
            {

                XmlAttribute fileName = childNode.Attributes["Name"];
                string fileExt;
                if (fileName != null)
                {
                    fileExt = Path.GetExtension(fileName.Value).ToLower();
                    if (al.Contains(fileExt))
                    {
                        XmlAttribute defaultFile = childNode.Attributes["Default"];
                        if (defaultFile != null)
                        {

                            if (defaultFile.Value == "True")
                            {
                                defaultFile.Value = "False";
                            }
                            if (childNode.HasChildNodes)
                            {
                                DicExplorerRemoveDefault(childNode, fileExtension);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Copies the Directories
        /// </summary>
        /// <param name="sourceFolder">From Folder Path</param>
        /// <param name="destinationFolder">Destination Path</param>
        /// <param name="parentRecursivePath">Recursive Parent of destinationFolder path</param>
        public void CopyDirectory(DirectoryInfo sourceFolder, DirectoryInfo destinationFolder, string parentRecursivePath)
        {
            if (sourceFolder.Name.StartsWith("."))  // Ignore .svn folders that are part of subversion repository
                return;
            string destPathParent = parentRecursivePath;
            AddFolderToXML(destinationFolder.FullName, destPathParent);

            destPathParent += "/" + destinationFolder.Name;

            // Copy all files.
            FileInfo[] files = sourceFolder.GetFiles();
            foreach (FileInfo file in files)
            {
                AddFileToXML(file.FullName, "False", false, destPathParent, true, true);
            }
            // Process subdirectories.
            DirectoryInfo[] dirs = sourceFolder.GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                // Get destinationFolder directory.
                string destinationDir = Common.PathCombine(destinationFolder.FullName, dir.Name);

                // Call CopyDirectory() recursively.
                CopyDirectory(dir, new DirectoryInfo(destinationDir), destPathParent);
            }
        }

        /// <summary>
        /// Loads the file from .de to the Solution Explorer
        /// </summary>
        /// <param name="dictionaryExplorer">The Solution Explorer</param>
        public void PopulateDicExplorer(TreeView dictionaryExplorer)
        {
            if (dictionaryExplorer == null) return;
            dictionaryExplorer.Nodes.Clear();
            var projNode = new TreeNode
                               {
                                   Text = _projectName,
                                   ImageIndex = 3,
                                   Tag = Common.FileType.Project,
                                   SelectedImageIndex = 3
                               };
            dictionaryExplorer.Nodes.Add(projNode);
            //To load projectFile at each time(TD-418)
            _DeXml.Load(_projectFileWithPath);
            XmlElement root = _DeXml.DocumentElement;
            if (root != null) PopulateDicExplorerNode(projNode, root.FirstChild);
            dictionaryExplorer.ExpandAll();
        }

        /// <summary>
        /// Loads the file from .de to the Solution Explorer 
        /// Setting the Pictures of the Node according the file/folder type
        /// </summary>
        /// <param name="projNode">Node to be Handled</param>
        /// <param name="subNode">The xml Node</param>
        public void PopulateDicExplorerNode(TreeNode projNode, XmlNode subNode)
        {
            foreach (XmlNode xn in subNode.ChildNodes)
            {
                XmlAttribute attVisible = null;
                try
                {
                    attVisible = xn.Attributes["Visible"];
                }
                catch {}
                string visible = attVisible == null ? "True" : attVisible.Value;
                var imageNode = new TreeNode();
                try
                {
                    attVisible = xn.Attributes["Type"];
                }
                catch {}
                if (attVisible != null)
                {
                    string fileName = "";
                    string defaultCSS;
                    try
                    {
                        fileName = xn.Attributes["Name"].Value;
                    }
                    catch
                    {
                    }

                    try
                    {
                        defaultCSS = xn.Attributes["Default"].Value;
                    }
                    catch
                    {
                        defaultCSS = "False";
                    }

                    if (attVisible.Value == "Folder")
                    {
                        imageNode.ImageIndex = 4; // Normal picture for Folders
                        imageNode.SelectedImageIndex = 5;  // Open picture - for Folders
                        imageNode.Tag = Common.FileType.Directory;   // Directory
                    }
                    else
                    {
                        if (defaultCSS.ToLower() == "true")
                        {

                            string ext = Path.GetExtension(fileName);
                            if (ext == ".css")
                            {

                                imageNode.ImageIndex = 7;  // Default CSS File
                                imageNode.SelectedImageIndex = 7;
                                _DefaultCssFileWithPath =!string.IsNullOrEmpty(_dictionaryPath)? Common.PathCombine(_dictionaryPath, fileName) : fileName;
                            }
                            else
                            {
                                imageNode.ImageIndex = 8;  // Default File
                                imageNode.SelectedImageIndex = 2;

                                if (_projectMode.ToLower() == "open")
                                {
                                    _DefaultXhtmlFileWithPath = Common.PathCombine(_dictionaryPath, fileName); // xhtml
                                    if (fileName.IndexOf(".xhtml") >= 0)
                                    {
                                        Common.GetLinkedCSS(_DefaultXhtmlFileWithPath);
                                    }
                                }
                            }
                        }
                        else
                        {
                            imageNode.ImageIndex = 2;  //  Files
                        }
                        imageNode.Tag = Common.FileType.File;
                    }
                    if (_userRole != "System Designer" && fileName.IndexOf(".css") >= 0)
                    {
                        continue;
                    }
                    if (!_hideFile)
                    {
                        if (visible == "False")
                        {
                            var fileType = (Common.FileType)imageNode.Tag;
                            if (fileType == Common.FileType.File)
                            {
                                imageNode.ImageIndex = 1;
                                imageNode.SelectedImageIndex = 1;   // Files Excluded 
                                imageNode.Tag = Common.FileType.FileExcluded;
                            }
                            else if (fileType == Common.FileType.Directory)
                            {
                                imageNode.ImageIndex = 6;
                                imageNode.SelectedImageIndex = 6;   // Directory Excluded 
                                imageNode.Tag = Common.FileType.DirectoryExcluded;
                            }
                        }
                        imageNode.Text = fileName;
                        projNode.Nodes.Add(imageNode);
                    }
                    else
                    {
                        if (visible == "True")
                        {
                            imageNode.Text = fileName;
                            projNode.Nodes.Add(imageNode);
                        }
                    }
                }

                if (xn.HasChildNodes)
                {
                    if (xn.ChildNodes.Count > 0)
                    {
                        PopulateDicExplorerNode(imageNode, xn);  // calling recursive Nodes.
                    }
                }
            }
        }


        /// <summary>
        /// Add the given file to the Project xml file
        /// </summary>
        /// <param name="fullFileName">File with full path</param>
        /// <param name="setDefault">Used to set default css</param>
        /// <param name="addToParentFolder">Forced to add in root of Solution Explorer</param>
        /// <param name="destPathParent">Parent of current folder - used for recursive copy</param>
        /// <param name="showFileExist">To Show the information about file already exist</param>
        /// <param name="visible"></param>
        /// <returns>Returns True/False</returns>
        public bool AddFileToXML(string fullFileName, string setDefault, bool addToParentFolder, string destPathParent, bool showFileExist, bool visible)
        {
            if (!File.Exists(fullFileName))
            {
                return false;
            }

            string fileName = Path.GetFileName(fullFileName);
            string fileNamePath;
            // Root directory
            if (addToParentFolder)
            {
                fileNamePath = Common.PathCombine(_dictionaryPath, fileName);
            }
            else
            {
                if (destPathParent != "")
                {
                    // used for recursive copy
                    fileNamePath = Common.PathCombine(_fullPath + destPathParent, fileName);
                }
                else
                {
                    // sub directory
                    fileNamePath = Common.PathCombine(_fullPath, fileName);
                }
            }

            if (IsFileExist(Path.GetFileName(fileNamePath), "True"))
            {
                if (showFileExist)
                {
                    var msg = new[] { "File Already Exist in Dictionary Express." };
                    LocDB.Message("defErrMsg", "File Already Exist in Dictionary Express.", msg, LocDB.MessageTypes.Info, LocDB.MessageDefault.First);
                }
            }
            else
            {
                try
                {
                    // Remove if file Visible="False"
                    if (IsFileExist(Path.GetFileName(fileNamePath), "False"))
                    {
                        RemoveFile(Path.GetFileName(fileNamePath));
                    }
                    if (fullFileName != fileNamePath)
                    {
                        File.Copy(fullFileName, fileNamePath, true);
                    }
                    // File
                    bool returnValue = XMLOperation(fileName, 'F', setDefault, addToParentFolder, "", destPathParent, visible);
                    if (destPathParent == "")
                    {
                        PopulateDicExplorer(_dictExplorer);
                    }
                    return returnValue;
                }
                catch
                {
                }
            }
            return false;
        }

        /// <summary>
        /// Find file exist in de.xml (Dictionary Solution)
        /// </summary>
        /// <param name="fileName">Source File Name</param>
        /// <param name="visible">File Mode = "True/False"</param>
        /// <returns>Returns True/False</returns>
        public bool IsFileExist(string fileName, string visible)
        {
            fileName = Path.GetFileName(fileName);
            bool result = false;
            string xPath = "//File[@Name='" + fileName + "' and @Visible='" + visible + "']";
            XmlNode resultNode = SearchNode(xPath);

            if (resultNode != null)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Remove file from de.xml (Dictionary Solution)
        /// </summary>
        /// <param name="fileName">Source File Name</param>
        public void RemoveFile(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            XmlElement root = ProjectDeXML.DocumentElement;
            string xPath = "SolutionExplorer";
            if (root != null)
            {
                XmlNode solnNode = root.SelectSingleNode(xPath);

                xPath = "//File[@Name='" + fileName + "']";
                XmlNode fileNode = ProjectDeXML.SelectSingleNode(xPath);
                if (fileNode != null)
                {
                    solnNode.RemoveChild(fileNode);
                }
            }
            SaveProject();
        }

        /// <summary>
        /// Adds the Folder to the Project xml file
        /// </summary>
        /// <param name="folderNameWithPath">Folder with full path</param>
        /// <param name="destPathParent">Parent of current folder - used for recursive copy</param>
        /// <returns>Returns True/False</returns>
        private bool AddFolderToXML(string folderNameWithPath, string destPathParent)
        {
            try
            {
                if (Directory.Exists(folderNameWithPath))
                {
                    return false;
                }
                Directory.CreateDirectory(folderNameWithPath);
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                return false;
            }

            string folderName = Path.GetFileName(folderNameWithPath);
            bool returnValue = XMLOperation(folderName, 'D', "", false, "", destPathParent, true);
            if (destPathParent == "")
            {
                PopulateDicExplorer(_dictExplorer);
            }
            return returnValue;
        }

        /// <summary>
        /// Xml Operation for add/delete/setdefault/exclude/include in project
        /// </summary>
        /// <param name="fileName">File Name with Full Path</param>
        /// <param name="fileType">File Type. F-file , D-Directory</param>
        /// <param name="setDefaultCSS">To set the default Css</param>
        /// <param name="addToParentFolder">Forced to add in Root</param>
        /// <param name="actionToBeTaken">Perform the add/delete/setdefault/exclude/include</param>
        /// <param name="destPathParent">Parent of current folder</param>
        /// <param name="visible">File visible</param>
        /// <returns>Returns True/False </returns>
        private bool XMLOperation(string fileName, char fileType, string setDefaultCSS, bool addToParentFolder, string actionToBeTaken, string destPathParent, bool visible)
        {
            XmlNode newNode;
            XmlElement root = GetRootNode();
            if (root == null) return false;
            XmlAttribute xmlAttrib;
            XmlNode searchNode = GetSolutionExplorerNode();
            // Force to add in Root Node
            if (!addToParentFolder)
            {
                // if Destination Path Exists
                string subPath = _subPathValue;
                // This block used for Bulk copy
                if (destPathParent != "")
                {
                    subPath += destPathParent;
                }
                if (subPath.IndexOf('/') >= 0)
                {
                    string searchFileName = subPath.Substring(subPath.LastIndexOf('/') + 1, subPath.Length - subPath.LastIndexOf('/') - 1);
                    string[] folderCount = subPath.Split('/');
                    string xPath = "/Project/SolutionExplorer";
                    for (int i = 1; i < folderCount.Length - 1; i++)
                    {
                        xPath += "/File/";
                    }
                    xPath += "/File[@Name='" + searchFileName + "']";
                    searchNode = root.SelectSingleNode(xPath);
                }
            }

            if (searchNode != null)
            {
                XmlAttribute attrib;
                switch (actionToBeTaken.ToLower())
                {
                    case "setasdefault":  // css default
                        string[] ext;
                        string fileExt = Path.GetExtension(fileName);
                        if (fileExt == ".css")
                        {
                            ext = new[] { ".css" };
                            _DefaultCssFileWithPath = Common.PathCombine(_dictionaryPath, fileName);
                        }
                        else
                        {
                            ext = new[] { ".xhtml", ".lift" };
                            _DefaultXhtmlFileWithPath = Common.PathCombine(_dictionaryPath, fileName);
                            DESetAttribute("//Project/DocumentSettings/Sections/Section[@Name='Main']", "Value", fileName);
                        }
                        
                        DicExplorerRemoveDefault(root.FirstChild, ext);  // Remove the Default Css
                        attrib = searchNode.Attributes["Default"];
                        if(attrib == null) break;
                        attrib.Value = "True";
                        break;

                    case "remove": // delete
                        searchNode.ParentNode.RemoveChild(searchNode);
                        break;

                    case "exclude":
                        attrib = searchNode.Attributes["Visible"];
                        attrib.Value = "False";
                        break;
                    case "include":

                        attrib = searchNode.Attributes["Visible"];
                        attrib.Value = "True";
                        break;

                    case "rename":
                        // used for new Name.
                        attrib = searchNode.Attributes["Name"];
                        attrib.Value = setDefaultCSS;
                        break;

                    case "":
                        string setFileType = fileType == 'D' ? "Folder" : "File";
                        newNode = _DeXml.CreateNode("element", "File", "");
                        //attribute
                        xmlAttrib = _DeXml.CreateAttribute("Name");
                        xmlAttrib.Value = fileName;
                        newNode.Attributes.Append(xmlAttrib);

                        //"Folder" or File;
                        xmlAttrib = _DeXml.CreateAttribute("Type");
                        xmlAttrib.Value = setFileType;

                        newNode.Attributes.Append(xmlAttrib);

                        xmlAttrib = _DeXml.CreateAttribute("Visible");
                        xmlAttrib.Value = visible.ToString(); //  "True";
                        newNode.Attributes.Append(xmlAttrib);

                        // if true , clear all default values
                        if (setDefaultCSS.ToLower() == "true")
                        {
                            string[] exten = new[] { ".css" };
                            DicExplorerRemoveDefault(root.FirstChild, exten);
                        }

                        xmlAttrib = _DeXml.CreateAttribute("Default");
                        xmlAttrib.Value = setDefaultCSS;
                        newNode.Attributes.Append(xmlAttrib);

                        searchNode.AppendChild(newNode);
                        break;
                }
            }
            _DeXml.Save(_projectFileWithPath);
            return true;
        }
        
        #region Image Files Copy to Local Folder

        /// <summary>
        /// To sets the projects type/version and date of creation/modification
        /// </summary>
        /// <param name="mode">The Project mode (new / open)</param>
        public void ProjectProperty(string mode)
        {
            try
            {
                XmlNode propertyCreated;
                XmlNode propertyCreatedOn;
                XmlNode searchNode = null;
                XmlNode modifyNode = null;
                XmlAttribute xattribute;
                XmlElement root = _DeXml.DocumentElement;
                string xPath = "PropertyGroup";
                if (root != null)
                {
                    searchNode = root.SelectSingleNode(xPath);
                    if (searchNode == null)
                    {
                        searchNode = _DeXml.CreateNode("element", "PropertyGroup", "");
                        root.AppendChild(searchNode);
                        mode = "new";
                    }
                    if (mode == "new")
                    {
                        //Setting Project Type and Version.
                        xattribute = _DeXml.CreateAttribute("Type");
                        xattribute.Value = _projectInputType;
                        root.Attributes.Append(xattribute);
                        xattribute = _DeXml.CreateAttribute("Version");
                        xattribute.Value = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                        root.Attributes.Append(xattribute);
                        xattribute = _DeXml.CreateAttribute("ShowError");
                        xattribute.Value = "True";
                        root.Attributes.Append(xattribute);
                        // Setting User, Date
                        propertyCreated = _DeXml.CreateNode("element", "Creation", "");
                        propertyCreatedOn = _DeXml.CreateNode("element", "CreatedOn", "");
                        propertyCreatedOn.InnerText = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");
                        propertyCreated.AppendChild(propertyCreatedOn);
                        propertyCreatedOn = _DeXml.CreateNode("element", "CreatedBy", "");
                        propertyCreatedOn.InnerText = Environment.UserName;
                        propertyCreated.AppendChild(propertyCreatedOn);
                        searchNode.AppendChild(propertyCreated);
                    }
                }
                xPath = "/Project/PropertyGroup/Modification";
                if (root != null) modifyNode = root.SelectSingleNode(xPath);
                if (modifyNode != null)
                {
                    searchNode.RemoveChild(modifyNode);
                }
                propertyCreated = _DeXml.CreateNode("element", "Modification", "");
                propertyCreatedOn = _DeXml.CreateNode("element", "ModifiedOn", "");
                propertyCreatedOn.InnerText = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");
                propertyCreated.AppendChild(propertyCreatedOn);
                propertyCreatedOn = _DeXml.CreateNode("element", "ModifiedBy", "");
                propertyCreatedOn.InnerText = Environment.UserName;
                propertyCreated.AppendChild(propertyCreatedOn);
                if (searchNode != null) searchNode.AppendChild(propertyCreated);
                _DeXml.Save(_projectFileWithPath);
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
            }
        }


        /// <summary>
        /// Set Attribute value in DE.XML
        /// </summary>
        /// <param name="xPath">xpath node</param>
        /// <param name="attributeName">Attribute Name</param>
        /// <param name="attributeValue">Attribute Value</param>
        public void DESetAttribute(string xPath, string attributeName, string attributeValue)
        {
            XmlNode returnNode = GetRootNode().SelectSingleNode(xPath);
            if (returnNode == null) return;
            var nameElement = (XmlElement)returnNode;
            nameElement.SetAttribute(attributeName, attributeValue);
            _DeXml.Save(_projectFileWithPath);
        }

        /// <summary>
        /// Get Attribute value in DE.XML
        /// </summary>
        /// <param name="nodeName">xpath node</param>
        /// <param name="attributeName">Attribute Name</param>
        /// <returns></returns>
        public string DEGetAttribute(string nodeName, string attributeName)
        {
            string attributeValue = string.Empty;
            string xPath = "//" + nodeName;
            XmlNode returnNode = GetRootNode().SelectSingleNode(xPath);
            if (returnNode != null)
            {
                XmlAttribute attrib = returnNode.Attributes[attributeName];
                if (attrib != null)
                {
                    attributeValue = attrib.Value;
                }
            }
            return attributeValue;
        }

        /// <summary>
        /// Sorting the Folder and Files in Tree Node
        /// </summary>
        /// <param name="projectFile">File to Sort the File Types</param>
        public void SortFolderFileTypes(string projectFile)
        {
            this.LoadProjectFile(projectFile);
            XmlNode explorer = GetSolutionExplorerNode();
            XmlNode folderNode = _DeXml.CreateNode("element", "dummy", "");
            XmlNode fileNode = _DeXml.CreateNode("element", "dummy", "");
            XmlNode allType = _DeXml.CreateNode("element", "dummy", "");
            int count = explorer.ChildNodes.Count;
            for (int i = 0; i < count; i++)
            {
                XmlNode xmlNode = explorer.FirstChild;
                XmlAttribute attrib = xmlNode.Attributes["Type"];
                if (attrib != null)
                {
                    if (attrib.Value == "Folder")
                    {
                        folderNode.AppendChild(xmlNode);
                    }
                    else if (attrib.Value == "File")
                    {
                        fileNode.AppendChild(xmlNode);
                    }
                    else
                    {
                        allType.AppendChild(xmlNode);
                    }
                }
            }

            explorer.RemoveAll();

            count = folderNode.ChildNodes.Count;
            for (int i = 0; i < count; i++)
            {
                XmlNode xmlNode = folderNode.FirstChild;
                explorer.AppendChild(xmlNode);
            }

            count = fileNode.ChildNodes.Count;
            for (int i = 0; i < count; i++)
            {
                XmlNode xmlNode = fileNode.FirstChild;
                explorer.AppendChild(xmlNode);
            }

            count = allType.ChildNodes.Count;
            for (int i = 0; i < count; i++)
            {
                XmlNode xmlNode = allType.FirstChild;
                explorer.AppendChild(xmlNode);
            }

            this.SaveProject();


        }

        #endregion
        #endregion
    }
}