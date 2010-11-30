// --------------------------------------------------------------------------------------------
// <copyright file="FlexPluginTest.cs" from='2009' to='2009' company='SIL International'>
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
// FlexPlugin Test
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using SIL.PublishingSolution;
using SIL.PublishingSolution.Sort;
using SIL.Tool;
using Test;

namespace TestBed
{
    public partial class FlexPluginTest : Form
    {
        private static string designerPath = "c:/AccessName/";
        private static List<string> fileNames = new List<string>();
        private static string sourceFolder = "c:\\temp";


        public FlexPluginTest()
        {
            InitializeComponent();
        }

        //private void BtnFlexTest_Click(object sender, EventArgs e)
        //{
        //    var plugin = new FlexDePlugin();
        //    plugin.Process();
        //}

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            var fd = new FolderBrowserDialog();
            //fd.ShowNewFolderButton = true;
            fd.ShowDialog();
            TxtInput.Text = fd.SelectedPath;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            var fd = new FolderBrowserDialog { ShowNewFolderButton = true };
            fd.ShowDialog();
            TxtOutput.Text = fd.SelectedPath;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (TxtInput.Text.Length <= 0)
                return;

            if (TxtOutput.Text.Length <= 0)
                return;

            if (TxtInput.Text == TxtOutput.Text)
            {
                MessageBox.Show("Both Path should not be the same");
                return;
            }
            string mypath = TxtInput.Text;
            designerPath = TxtOutput.Text;
            if (Directory.Exists(designerPath))
            {
                Directory.Delete(designerPath, true);
            }
            Directory.CreateDirectory(designerPath);
            PutAccessibleName(mypath);
            MessageBox.Show("Completed");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        public void PutAccessibleName(string src)
        {
            string source = src;
            var di = new DirectoryInfo(src);

            if (di.Name.ToLower() == "properties")
                return;

            string fstr;
            var filter = "*.designer.cs";
            designerPath += "/";
            foreach (var fileInfo in di.GetFiles(filter))
            {
                string dest = di.Root.ToString();
                string destPath = source.Replace(dest, designerPath);
                if (!Directory.Exists(destPath))
                {
                    Directory.CreateDirectory(destPath);
                }

                string sourceFile = fileInfo.Name;
                var fs = new FileStream(Common.PathCombine(src, sourceFile), FileMode.Open);
                var sr = new StreamReader(fs);

                string filePath = Common.PathCombine(destPath, sourceFile);
                var fs2 = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                var sw2 = new StreamWriter(fs2);
                bool iniComp = false;


                while ((fstr = sr.ReadLine()) != null)
                {
                    if (!iniComp && fstr.Contains("InitializeComponent()"))
                    {
                        iniComp = true;
                    }
                    else if (iniComp)
                    {

                        if (fstr.Contains(".Name ="))
                        {
                            string newString = fstr.Replace(".Name", ".AccessibleName");
                            sw2.WriteLine(newString);
                        }
                        else if (fstr.Contains(".AccessibleName ="))
                        {
                            continue;
                        }
                        if (fstr.Contains("#endregion")) iniComp = false;
                    }
                    sw2.WriteLine(fstr);
                }
                sr.Close();
                fs.Close();
                sw2.Close();
                fs2.Close();
            }

            foreach (var directoryInfo in di.GetDirectories())
            {
                if (directoryInfo.Name.Substring(0, 1) == ".")
                    continue;
                PutAccessibleName(directoryInfo.FullName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        public void PutAccessibleNamenew(string src)
        {
            string source = src;
            var di = new DirectoryInfo(src);

            if (di.Name.ToLower() == "properties")
                return;

            string fstr;
            var filter = "*.designer.cs";
            designerPath += "/";
            foreach (var fileInfo in di.GetFiles(filter))
            {
                string dest = di.Root.ToString();
                string destPath = source.Replace(dest, designerPath);
                if (!Directory.Exists(destPath))
                {
                    Directory.CreateDirectory(destPath);
                }

                string sourceFile = fileInfo.Name;
                var fs = new FileStream(Common.PathCombine(src, sourceFile), FileMode.Open);
                var sr = new StreamReader(fs);

                string filePath = Common.PathCombine(destPath, sourceFile);
                var fs2 = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                var sw2 = new StreamWriter(fs2);
                //bool iniComp = false;


                while ((fstr = sr.ReadLine()) != null)
                {
                    //if (!iniComp && fstr.Contains("InitializeComponent()"))
                    //{
                    //    iniComp = true;
                    //}
                    //else if (iniComp)
                    //{

                    if (fstr.Contains(".Name ="))
                    {
                        string newString = fstr.Replace(".Name", ".AccessibleName");
                        sw2.WriteLine(newString);
                    }
                    else if (fstr.Contains(".AccessibleName ="))
                    {
                        continue;
                    }
                    //if (fstr.Contains("#endregion")) iniComp = false;
                    //}
                    sw2.WriteLine(fstr);
                }
                sr.Close();
                fs.Close();
                sw2.Close();
                fs2.Close();
            }

            foreach (var directoryInfo in di.GetDirectories())
            {
                if (directoryInfo.Name.Substring(0, 1) == ".")
                    continue;
                PutAccessibleName(directoryInfo.FullName);
            }
        }

        private void BtnTETest_Click(object sender, EventArgs e)
        {
            var plugin = new PsExport { DataType = "Scripture" };
            //string outputpath = "c:/main/main";
            //string outputpath = "c:/jas/jas";
            string outputpath = "c:/1pe/1pe";
            //string outputpath = "c:/Bughotu/Bughotu";
            plugin.Export(outputpath);

        }
      
        private void BtnFlexTest_Click(object sender, EventArgs e)
        {
            //var rule = new string[1] { @"&B < b < A < a" };
            //string path = "c:\\lifttest\\buangverysmall.lift";
            //string outputPath = "c:\\lifttest\\output.lift";
            //LiftEntrySorter ls = new LiftEntrySorter();
            //ls.addRules(rule);
            //LiftWriter lw = new LiftWriter(outputPath);
            //LiftReader lr = new LiftReader(path);
            //ls.sort(lr,lw);
            //return;

            //MessageBox.Show(Common.ValidateStartsWithAlphabet(txtInputPath.Text).ToString());
            //return;

            //string outputpath = "c:\\file1.xhtml";
            //string splitclass = "scrbook";
            //List<string> files = Common.SplitXhtmlFile(outputpath, splitclass);
            //return;

        }

        private void InDesign_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtInputPath.Text))
            {
                MessageBox.Show("Please enter the valid XHTML path");
                return;
            }

            if (!File.Exists(txtCSSInput.Text))
            {
                MessageBox.Show("Please enter the valid CSS path");
                return;
            }
            ExportInDesign exportInDesign = new ExportInDesign();
            PublicationInformation projInfo = new PublicationInformation();

            projInfo.ProjectPath = Path.GetDirectoryName(txtInputPath.Text);
            projInfo.DefaultXhtmlFileWithPath = txtInputPath.Text;
            projInfo.DefaultCssFileWithPath = txtCSSInput.Text;

            //string projectPath = Path.GetDirectoryName(txtInputPath.Text);
            //string xhtmlFileWithPath = txtInputPath.Text;
            //string cssFileWithPath = txtCSSInput.Text;
            projInfo.ProjectFileWithPath = projInfo.ProjectPath;
            exportInDesign.Export(projInfo);
        }

        private void Btn_InputPath_Click(object sender, EventArgs e)
        {
            txtInputPath.Text = GetFilePath("XHTML Files|*.xhtml|XML Files|*.xml");
            txtCSSInput.Text = Path.ChangeExtension(txtInputPath.Text, "css");
        }

        private string GetFilePath(string fileType)
        {
            var fd = new OpenFileDialog();
            fd.Filter = fileType;
            fd.ShowDialog();
            
            return fd.FileName;
        }

        private void BtnInputCSS_Click(object sender, EventArgs e)
        {

            txtCSSInput.Text = GetFilePath("CSS Files|*.css");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            if (!File.Exists(txtInputPath.Text))
            {
                MessageBox.Show("Please enter the valid XHTML path");
                return;
            }

            if (!File.Exists(txtCSSInput.Text))
            {
                MessageBox.Show("Please enter the valid CSS path");
                return;
            }

            Common.Testing = chkPage.Checked;

            PublicationInformation projInfo = new PublicationInformation();
            projInfo.ProjectPath = Path.GetDirectoryName(txtInputPath.Text);
            projInfo.DictionaryPath = Path.GetDirectoryName(txtInputPath.Text);
            projInfo.DefaultXhtmlFileWithPath = txtInputPath.Text;
            projInfo.DefaultCssFileWithPath = txtCSSInput.Text;
            projInfo.ProgressBar = new ProgressBar();
            projInfo.DictionaryOutputName = "test";
            projInfo.ProjectInputType = radDictionary.Checked ? "Dictionary" : "Scripture";
            projInfo.FinalOutput = "odt";
            projInfo.IsExtraProcessing = true;

            ExportOpenOffice exportOdt = new ExportOpenOffice();
            exportOdt.Export(projInfo);

            if(Common.Testing)
            {
                string file = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath) + "\\" + Path.GetFileNameWithoutExtension(projInfo.DefaultXhtmlFileWithPath) + "." +projInfo.FinalOutput;
                if (File.Exists(file))
                    Process.Start(file);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmTreeView tv = new frmTreeView();
            tv.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ANTLR tv = new ANTLR();
            tv.ShowDialog();
        }

        private void btnFileSizeZero_Click(object sender, EventArgs e)
        {
            fileNames.Clear();
            if (TxtInput.Text.Length <= 0)
            {
                MessageBox.Show("Please select the inputPath");
                return;
            }
            IncreaseFileSizeFromZeroBites(TxtInput.Text);
            MessageBox.Show("Completed for " + fileNames.Count + " files");
            int fileCount = 1;
            using (TextWriter streamWriter =
                        new StreamWriter("c:\\filenamesWithZeroBytes.txt"))
            {
                foreach (string fileName in fileNames)
                {
                    streamWriter.WriteLine(fileCount++ + "  - " + fileName);
                }
            }
        }

        private void IncreaseFileSizeFromZeroBites(string inputPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(inputPath);
            FileInfo[] fileInfo = directoryInfo.GetFiles();
            foreach (FileInfo file in fileInfo)
            {
                if (file.Length == 0)
                {
                    string filePath = inputPath + "\\" + file.Name;
                    fileNames.Add(filePath);
                    if (chkIncrease.Checked)
                    {
                        using (TextWriter streamWriter =
                            new StreamWriter(filePath))
                        {
                            streamWriter.Write("                                ");
                            streamWriter.Close();
                        }
                    }
                }
            }

            // Process subdirectories.
            DirectoryInfo[] dirs = directoryInfo.GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                if (dir.Name == ".svn") 
                    continue;
                
                // Process sub directories
                string destinationDir = Path.Combine(inputPath, dir.Name);

                // Call recursively.
                IncreaseFileSizeFromZeroBites(destinationDir);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtInputPath.Text))
            {
                MessageBox.Show("Please enter the valid XHTML path");
                return;
            }

            if (!File.Exists(txtCSSInput.Text))
            {
                MessageBox.Show("Please enter the valid CSS path");
                return;
            }

            OOExportOpenOffice exportOdt = new OOExportOpenOffice();
            //ExportOdt exportOdt = new ExportOdt();
            PublicationInformation projInfo = new PublicationInformation();

            projInfo.ProjectInputType = "Dictionary";
            projInfo.ProjectPath = Path.GetDirectoryName(txtInputPath.Text);
            projInfo.DictionaryPath = Path.GetDirectoryName(txtInputPath.Text);
            projInfo.DefaultXhtmlFileWithPath = txtInputPath.Text;
            projInfo.DefaultCssFileWithPath = txtCSSInput.Text;
            projInfo.ProgressBar = new ProgressBar();
            projInfo.DictionaryOutputName = "test";
            exportOdt.Export(projInfo);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            fileNames.Clear();
            sourceFolder = "c:\\temp";
            GetFilesFromFolder(sourceFolder);

            // Get files from Xml
            List<string> fileNamesXml = new List<string>();
            XmlNode returnValue = null;
            XmlDocument LoadedDoc = new XmlDocument();
            LoadedDoc.Load("c:\\FileLibrary.xml");
            string XPath = "//FileLibrary";
            XmlElement root = LoadedDoc.DocumentElement;
            if (root != null)
            {
                returnValue = root.SelectSingleNode(XPath);
                foreach (XmlNode xmlNode in returnValue.ChildNodes)
                {
                    string path = xmlNode.Attributes.GetNamedItem("Path").Value;
                    fileNamesXml.Add(path);
                }
            }

            //Find missing files in xml 
            foreach (string file in fileNames)
            {
                if (!fileNamesXml.Contains(file))
                {
                    string newGuid = Guid.NewGuid().ToString();
                    //Add a File Node
                    XmlNode newNode = LoadedDoc.CreateNode("element", "File", "");
                    XmlAttribute xmlAttrib = LoadedDoc.CreateAttribute("Path");
                    xmlAttrib.Value = file;
                    newNode.Attributes.Append(xmlAttrib);

                    xmlAttrib = LoadedDoc.CreateAttribute("ComponentGuid");
                    xmlAttrib.Value = newGuid;
                    newNode.Attributes.Append(xmlAttrib);
                    root.LastChild.AppendChild(newNode);
                }
            }
            LoadedDoc.Save("c:\\FileLibrary.xml");
        }

        public static void GetFilesFromFolder(string srcfolder)
        {

            var dir = new DirectoryInfo(srcfolder);
            foreach (FileInfo fileInfo in dir.GetFiles())
            {
                string dstFullName = Common.PathCombine(srcfolder, fileInfo.Name);
                FileInfo dstInfo = new FileInfo(dstFullName);
                string fileName = dstInfo.FullName.Replace(sourceFolder, "Files");
                fileNames.Add(fileName);
            }
            foreach (var directoryInfo in dir.GetDirectories())
            {
                if (directoryInfo.Name.Substring(0, 1) == ".svn")
                    continue;
                GetFilesFromFolder(directoryInfo.FullName);
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            //PdftoJpg pd = new PdftoJpg();
            //pd.ConvertPdftoJpg(); 
        }

        private void FlexPluginTest_Load(object sender, EventArgs e)
        {
            var supportFolder = PathPart.Bin(Environment.CurrentDirectory, "/../PsSupport");
            FolderTree.Copy(supportFolder, ".");
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            var target = new ExportLogos();
            PublicationInformation projInfo = new PublicationInformation();
            projInfo.DefaultXhtmlFileWithPath = txtInputPath.Text;
            projInfo.DefaultCssFileWithPath = txtCSSInput.Text;

            var actual = target.Export(projInfo);
        }

        private void btnParaText_Click(object sender, EventArgs e)
        {
            ParatextPathwayLink paraText = new ParatextPathwayLink("MyprojName", "MydatabaseName", "en", "en", "Sankar");
            XmlDocument usfxDoc = new XmlDocument();
            usfxDoc.Load(txtInputPath.Text);
            paraText.ExportToPathway(usfxDoc);
        }


    }
    
}