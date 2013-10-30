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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Xml;
using JWTools;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using SIL.PublishingSolution;
using SIL.PublishingSolution.Sort;
using SIL.Tool;
using SIL.Tool.Localization;
using System.Data.Common;
using System.Data;
using TestBed.Properties;
using epubValidator;
using Test;

namespace TestBed
{
    public partial class FlexPluginTest : Form
    {
        private static string designerPath = "c:/AccessName/";
        private static List<string> fileNames = new List<string>();
        private static string sourceFolder = "c:\\temp";
        private Progress pb;

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
            // Lift Test
            //var rule = new string[1] { @"&B < b < A < a" };
            //string path = "c:\\lifttest\\buangverysmall.lift";
            //string outputPath = "c:\\lifttest\\output.lift";
            //LiftEntrySorter ls = new LiftEntrySorter();
            //ls.addRules(rule);
            //LiftWriter lw = new LiftWriter(outputPath);
            //LiftReader lr = new LiftReader(path);
            //ls.sort(lr,lw);
            //return;

            //Validation 
            //MessageBox.Show(Common.ValidateStartsWithAlphabet(txtInputPath.Text).ToString());
            //return;

            // One per section Test
            //string outputpath = "c:\\file1.xhtml";
            //string splitclass = "scrbook";
            //List<string> files = Common.SplitXhtmlFile(outputpath, splitclass);
            //return;


            PrintVia printVia = new PrintVia();
            printVia.InputType = "Dictionary";
            printVia.ShowDialog();

            string target = printVia.BackEnd; //"OpenOffice";
            var tpe = new PsExport { Destination = target, DataType = printVia.InputType };
            tpe.Export(txtInputPath.Text);





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
            projInfo.ProjectInputType = radDictionary.Checked ? "Dictionary" : "Scripture";
            //string projectPath = Path.GetDirectoryName(txtInputPath.Text);
            //string xhtmlFileWithPath = txtInputPath.Text;
            //string cssFileWithPath = txtCSSInput.Text;
            projInfo.ProjectFileWithPath = projInfo.ProjectPath;
            projInfo.DictionaryPath = projInfo.ProjectPath;
            exportInDesign.Export(projInfo);
        }

        private void Btn_InputPath_Click(object sender, EventArgs e)
        {
            txtInputPath.Text = GetFilePath("XHTML Files|*.xhtml|XML Files|*.xml|Tex|*.tex|Zip Files|*.zip|usx Files|*.usx|sfm Files|*.sfm|Text Files|*.txt|All Files|*.*");
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

            txtCSSInput.Text = GetFilePath("CSS Files|*.css|STY Files|*.sty");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {


            //ArrayList mylist = new ArrayList();
            //bool isSectionHead = false, isChapterNumber = false, isVerseNumber = false;
            //string sectionHead = string.Empty, fromChapterNumber = string.Empty, currentChapterNumber = string.Empty, firstVerseNumber = string.Empty, lastVerseNumber = string.Empty;
            //string formatString = string.Empty;
            //string textString = string.Empty;
            //int playOrder = 0;
            //StringBuilder sb = new StringBuilder();
            //XmlWriter ncx = new XmlTextWriter(@"d:/test.xml", Encoding.UTF8);
            //XmlDocument xdoc = new XmlDocument { XmlResolver = null };
            //XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xdoc.NameTable);
            //namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            //XmlReaderSettings xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, ProhibitDtd = false };
            //XmlReader xmlReader = XmlReader.Create(@"C:\Users\James\Documents\Publications\BM2\EBook (epub)_2012-06-21_0232\OEBPS\PartFile00001_01.xhtml", xmlReaderSettings);
            //xdoc.Load(xmlReader);
            //xmlReader.Close();
            //XmlNodeList nodes;
            //nodes = xdoc.SelectNodes("//xhtml:div[@class='scrBook']", namespaceManager);

            //foreach (XmlNode node in nodes)
            //{
            //    using (XmlReader reader = XmlReader.Create(new StringReader(node.OuterXml)))
            //    {
            //        // Parse the file and display each of the nodes.
            //        while (reader.Read())
            //        {
            //            switch (reader.NodeType)
            //            {
            //                case XmlNodeType.Element:
            //                    string className = reader.GetAttribute("class");
            //                    if (className == "Section_Head")
            //                    {
            //                        if (fromChapterNumber == currentChapterNumber)
            //                        {
            //                            textString = textString + "-" + lastVerseNumber;
            //                        }
            //                        else
            //                        {
            //                            textString = textString + "-" + currentChapterNumber + ":" +
            //                                           lastVerseNumber;
            //                        }
            //                        if (textString.Trim().Length > 4)
            //                        {
            //                            // write out the node
            //                            ncx.WriteStartElement("navPoint");
            //                            ncx.WriteAttributeString("id", "dtb:uid");
            //                            ncx.WriteAttributeString("playOrder", playOrder.ToString());
            //                            ncx.WriteStartElement("navLabel");
            //                            ncx.WriteElementString("text", textString);
            //                            ncx.WriteEndElement(); // navlabel
            //                            ncx.WriteStartElement("content");
            //                            ncx.WriteAttributeString("src", sb.ToString());
            //                            ncx.WriteEndElement(); // meta
            //                            //ncx.WriteEndElement(); // meta
            //                            //ncx.WriteEndElement(); // navPoint
            //                            playOrder++;
            //                        }
            //                        if (textString.Trim().Length > 4)
            //                        {
            //                            ncx.WriteEndElement(); // navPoint
            //                            textString = string.Empty;
            //                        }
            //                        textString = string.Empty;
            //                        firstVerseNumber = string.Empty;
            //                        isSectionHead = true;
            //                    }
            //                    else if (className == "Chapter_Number")
            //                    {
            //                        isChapterNumber = true;
            //                    }
            //                    else if (className != null && className.IndexOf("Verse_Number") == 0)
            //                    {
            //                        isVerseNumber = true;
            //                    }
            //                    break;
            //                case XmlNodeType.Text:
            //                    if (isSectionHead)
            //                    {
            //                        sectionHead = reader.Value;
            //                        isSectionHead = false;
            //                    }
            //                    if (isChapterNumber)
            //                    {
            //                        currentChapterNumber = reader.Value;
            //                        isChapterNumber = false;
            //                    }
            //                    if (isVerseNumber)
            //                    {
            //                        if (firstVerseNumber.Trim().Length == 0 && sectionHead.Length > 0)
            //                        {
            //                            firstVerseNumber = reader.Value;
            //                            fromChapterNumber = currentChapterNumber;
            //                            textString = sectionHead + " " + currentChapterNumber + ":" + firstVerseNumber;
            //                        }
            //                        lastVerseNumber = reader.Value;
            //                        isVerseNumber = false;
            //                    }
            //                    break;
            //                case XmlNodeType.XmlDeclaration:
            //                case XmlNodeType.ProcessingInstruction:
            //                    break;
            //                case XmlNodeType.Comment:
            //                    break;
            //                case XmlNodeType.EndElement:
            //                    break;
            //            }
            //        }
            //    }
            //    //using (XmlReader reader = XmlReader.Create(new StringReader(node.OuterXml)))
            //    //{
            //    //    // Parse the file and display each of the nodes.
            //    //    while (reader.Read())
            //    //    {
            //    //        switch (reader.NodeType)
            //    //        {
            //    //            case XmlNodeType.Element:
            //    //                string one = reader.Name;
            //    //                string className = reader.GetAttribute("class");
            //    //                if (className == "Section_Head")
            //    //                {
            //    //                    if (fromChapterNumber == currentChapterNumber)
            //    //                    {
            //    //                        formatString = formatString + "-" + lastVerseNumber;
            //    //                    }
            //    //                    else
            //    //                    {
            //    //                        formatString = formatString + "-" + currentChapterNumber + ":" +
            //    //                                       lastVerseNumber;
            //    //                    }
            //    //                    mylist.Add(formatString);
            //    //                    formatString = string.Empty;
            //    //                    firstVerseNumber = string.Empty;
            //    //                    isSectionHead = true;
            //    //                }
            //    //                else if (className == "Chapter_Number")
            //    //                {
            //    //                    isChapterNumber = true;
            //    //                }
            //    //                else if (className != null && className.IndexOf("Verse_Number") == 0)
            //    //                {
            //    //                    isVerseNumber = true;
            //    //                }
            //    //                break;
            //    //            case XmlNodeType.Text:
            //    //                if (isSectionHead)
            //    //                {
            //    //                    sectionHead = reader.Value;
            //    //                    isSectionHead = false;
            //    //                    //VerseNumber = "0";
            //    //                }
            //    //                if (isChapterNumber)
            //    //                {
            //    //                    currentChapterNumber = reader.Value;
            //    //                    isChapterNumber = false;
            //    //                }
            //    //                if (isVerseNumber)
            //    //                {
            //    //                    if (firstVerseNumber.Trim().Length == 0 && sectionHead.Length > 0)
            //    //                    {
            //    //                        firstVerseNumber = reader.Value;
            //    //                        fromChapterNumber = currentChapterNumber;
            //    //                        formatString = sectionHead + " " + currentChapterNumber + ":" + firstVerseNumber;
            //    //                    }

            //    //                    lastVerseNumber = reader.Value;
            //    //                    isVerseNumber = false;
            //    //                }
            //    //                break;
            //    //            case XmlNodeType.XmlDeclaration:
            //    //            case XmlNodeType.ProcessingInstruction:
            //    //                break;
            //    //            case XmlNodeType.Comment:
            //    //                break;
            //    //            case XmlNodeType.EndElement:
            //    //                break;
            //    //        }
            //    //    }
            //    //}
            //}


            ////MessageBox.Show("Code is commented");
            ////if (!File.Exists(txtInputPath.Text))
            ////{
            ////    MessageBox.Show("Please enter the valid XHTML path");
            ////    return;
            ////}

            ////if (!File.Exists(txtCSSInput.Text))
            ////{
            ////    MessageBox.Show("Please enter the valid CSS path");
            ////    return;
            ////}

            ////Common.Testing = chkPage.Checked;

            ////PublicationInformation projInfo = new PublicationInformation();
            ////projInfo.ProjectPath = Path.GetDirectoryName(txtInputPath.Text);
            ////projInfo.DictionaryPath = Path.GetDirectoryName(txtInputPath.Text);
            ////projInfo.DefaultXhtmlFileWithPath = txtInputPath.Text;
            ////projInfo.DefaultCssFileWithPath = txtCSSInput.Text;
            ////projInfo.ProgressBar = new ProgressBar();
            ////projInfo.DictionaryOutputName = "test";
            ////projInfo.ProjectInputType = radDictionary.Checked ? "Dictionary" : "Scripture";
            ////projInfo.FinalOutput = "odt";
            ////projInfo.IsExtraProcessing = true;

            ////ExportOpenOffice_OLD exportOdt = new ExportOpenOffice_OLD();
            ////exportOdt.Export(projInfo);

            ////if (Common.Testing)
            ////{
            ////    string file = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath) + "\\" + Path.GetFileNameWithoutExtension(projInfo.DefaultXhtmlFileWithPath) + "." + projInfo.FinalOutput;
            ////    if (File.Exists(file))
            ////        Process.Start(file);
            ////}
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

            ExportLibreOffice exportOdt = new ExportLibreOffice();
            //ExportOdt exportOdt = new ExportOdt();
            PublicationInformation projInfo = new PublicationInformation();

            string ProjType = "Dictionary";

            if (radScripture.Checked)
            {
                ProjType = "Scripture";
            }


            projInfo.FinalOutput = "odt";
            projInfo.ProjectInputType = ProjType;
            projInfo.ProjectPath = Path.GetDirectoryName(txtInputPath.Text);
            projInfo.DictionaryPath = Path.GetDirectoryName(txtInputPath.Text);
            projInfo.DefaultXhtmlFileWithPath = txtInputPath.Text;
            projInfo.DefaultCssFileWithPath = txtCSSInput.Text;
            projInfo.ProgressBar = new ProgressBar();
            projInfo.DictionaryOutputName = "test";

            ProjType = Common.GetProjectType(projInfo.DefaultXhtmlFileWithPath);
            projInfo.ProjectInputType = ProjType;

            ////if you enable below five lines it will execute both files and produced odm.
            //projInfo.DefaultRevCssFileWithPath = txtCSSInput.Text.Replace("main","Flexrev");
            //projInfo.IsReversalExist = true;
            //projInfo.IsLexiconSectionExist = true;
            //projInfo.FromPlugin = true;
            //projInfo.FinalOutput = "odm";

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
            LocalizationSetup();
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected static void LocalizationSetup()
        {
            JW_Registry.RootKey = @"SOFTWARE\The Seed Company\Dictionary Express!";
            LocDB.SetAppTitle();
            LocDB.BaseName = "PsLocalization.xml";
            var folderPath = Common.PathCombine(Common.GetAllUserAppPath(), @"SIL\Pathway\Dictionary");
            var localizationPath = Common.PathCombine(folderPath, "Loc");
            if (!Directory.Exists(localizationPath))
            {
                Directory.CreateDirectory(localizationPath);
                File.Copy(Common.FromRegistry(@"Loc/" + LocDB.BaseName),
                          Common.PathCombine(localizationPath, LocDB.BaseName));
            }
            LocDB.Initialize(folderPath);
        }

        private void btnParaText_Click(object sender, EventArgs e)
        {
            string os = Common.GetOsName();
            MessageBox.Show(os);
            return;
#if !Not7
            ParatextPathwayLink paraText = new ParatextPathwayLink("NKOu1", "NKOu1", "en", "en", "Sankar");
            XmlDocument usfxDoc = new XmlDocument();
            usfxDoc.Load(txtInputPath.Text);
            paraText.ExportToPathway(usfxDoc);
#endif
        }

        private void StyConvert_Click(object sender, EventArgs e)
        {
#if !Not7
            StyToCSS styToCss = new StyToCSS();
            styToCss.StyFullPath = txtCSSInput.Text;
            string outputCSS = txtCSSInput.Text.Replace(".sty", ".css");
            styToCss.ConvertStyToCSS("TestBed", outputCSS);
            MessageBox.Show("Exported in " + outputCSS);
#endif
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtInputPath.Text))
            {
                MessageBox.Show("Please enter the valid XHTML path");
                return;
            }

            //if (!File.Exists(txtCSSInput.Text))
            //{
            //    MessageBox.Show("Please enter the valid CSS path");
            //    return;
            //}
            ExportGoBible exportGoBible = new ExportGoBible();
            PublicationInformation projInfo = new PublicationInformation();

            projInfo.ProjectPath = Path.GetDirectoryName(txtInputPath.Text);
            projInfo.DefaultXhtmlFileWithPath = txtInputPath.Text;
            projInfo.DefaultCssFileWithPath = txtCSSInput.Text;

            projInfo.ProjectFileWithPath = projInfo.ProjectPath;
            projInfo.DictionaryPath = projInfo.ProjectPath;
            exportGoBible.Export(projInfo);
        }

        private void btnFlexTest2_Click(object sender, EventArgs e)
        {
            ExportThroughPathway dlg = new ExportThroughPathway();
            dlg.InputType = "Dictionary";
            dlg.ShowDialog();

            string target = dlg.Format; //"OpenOffice";
            var tpe = new PsExport { Destination = target, DataType = dlg.InputType };
            tpe.Export(txtInputPath.Text);

        }

        private void btnXeLaTex_Click(object sender, EventArgs e)
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
            ExportXeLaTex exportXeLaTex = new ExportXeLaTex();
            PublicationInformation projInfo = new PublicationInformation();

            projInfo.ProjectPath = Path.GetDirectoryName(txtInputPath.Text);
            projInfo.DefaultXhtmlFileWithPath = txtInputPath.Text;
            projInfo.DefaultCssFileWithPath = txtCSSInput.Text;
            projInfo.ProjectInputType = radDictionary.Checked ? "Dictionary" : "Scripture";
            projInfo.ProjectFileWithPath = projInfo.ProjectPath;
            projInfo.DictionaryPath = projInfo.ProjectPath;
            exportXeLaTex.Export(projInfo);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            string cc = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
            string[] filePaths = Directory.GetFiles(@"C:\ProgramData\SIL\WritingSystemStore\", "*.ldml");
            StringBuilder newProperty = new StringBuilder();
            Dictionary<string, string> fontList1 = new Dictionary<string, string>();
            foreach (string filePath in filePaths)
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(filePath);
                XmlNode node =
                    xmlDocument.SelectSingleNode(
                        "/ldml/special[1]/*[namespace-uri()='urn://palaso.org/ldmlExtensions/v1' and local-name()='defaultFontFamily'][1]/@value");
                newProperty.AppendLine("div[lang='" + fileName + "']{ font-family: \"" + node.Value + "\";}");
                newProperty.AppendLine("span[lang='" + fileName + "']{ font-family: \"" + node.Value + "\";}");
            }

            string path = @"d:\TELanguage.css";
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.Write(newProperty.ToString());
                }
            }
        }

        private void btnDotNet_Click(object sender, EventArgs e)
        {
            //HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework Setup\NDP\v2.0.50727
            //HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\.NETFramework\policy\v2.0

            bool isDotnetInstalled = IsDotNet2IsInstalled(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v2.0.50727",
                                                          "Version");

            if (isDotnetInstalled)
            {
                //Policy directory
                isDotnetInstalled = IsDotNet2IsInstalled(@"SOFTWARE\Microsoft\.NETFramework\policy\v2.0", "50727");
            }

            if (isDotnetInstalled)
            {
                MessageBox.Show("DotNet 2.0 is installed in this System.");
            }
            else
            {
                MessageBox.Show("DotNet 2.0 is not installed");
            }
        }

        private bool IsDotNet2IsInstalled(string registrySubDirectory, string keyName)
        {
            string registryValue = string.Empty;
            if (Common.GetOsName() == "Windows7")
                registryValue = Common.GetValueFromRegistry(registrySubDirectory, keyName);
            else if (Common.GetOsName() == "Windows XP")
                registryValue = Common.GetValueFromRegistry(registrySubDirectory, keyName);
            else if (Common.UnixVersionCheck())
            {
                System.Security.Principal.WindowsPrincipal p =
                    System.Threading.Thread.CurrentPrincipal as System.Security.Principal.WindowsPrincipal;
                string userName = p.Identity.Name;
                while (Directory.Exists("/home/" + userName + "/.winetrickscache/dotnet20/"))
                {
                    registryValue = "Exists";
                    break;
                }
            }

            if (registryValue.Length > 0)
                return true;

            return false;
        }

        private void btnGetSoftwareData_Click(object sender, EventArgs e)
        {
            Process.Start("http://myphpapps.com.cws10.my-hosting-panel.com/getdata.php");
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnYouVersion_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtInputPath.Text))
            {
                MessageBox.Show("Please enter the valid XHTML path");
                return;
            }

            ExportYouVersion exportYouVersion = new ExportYouVersion();
            PublicationInformation projInfo = new PublicationInformation();

            projInfo.ProjectPath = Path.GetDirectoryName(txtInputPath.Text);
            projInfo.DefaultXhtmlFileWithPath = txtInputPath.Text;
            projInfo.ProjectInputType = radDictionary.Checked ? "Dictionary" : "Scripture";
            projInfo.ProjectFileWithPath = projInfo.ProjectPath;
            projInfo.DictionaryPath = projInfo.ProjectPath;
            exportYouVersion.Export(projInfo);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtInputPath.Text))
            {
                MessageBox.Show("Please enter the valid XHTML path");
                return;
            }

            XhtmlToHtml xhtmlToHtml = new XhtmlToHtml();
            xhtmlToHtml.Convert(txtInputPath.Text);
            MessageBox.Show("Exported.");
        }

        private void btnWordPress_Click(object sender, EventArgs e)
        {

            if (!File.Exists(txtInputPath.Text))
            {
                MessageBox.Show("Please enter the valid XHTML path");
                return;
            }

            PublicationInformation projInfo = new PublicationInformation();

            projInfo.ProjectPath = Path.GetDirectoryName(txtInputPath.Text);
            projInfo.DefaultXhtmlFileWithPath = txtInputPath.Text;
            projInfo.ProjectInputType = radDictionary.Checked ? "Dictionary" : "Scripture";
            projInfo.ProjectFileWithPath = projInfo.ProjectPath;
            projInfo.DictionaryPath = projInfo.ProjectPath;

            ExportWordPress exportWS = new ExportWordPress();
            exportWS.Export(projInfo);

            //ExportWordPress XhtmlToBlog = new ExportWordPress();
            //XhtmlToBlog.Export(projInfo);
            //MessageBox.Show("Xhtml has been Exported.");

            ExportXhtmlToSqlData xhtmlToSqlData = new ExportXhtmlToSqlData();
            xhtmlToSqlData.MysqlDataFileName = "data.sql";
            xhtmlToSqlData._projInfo = projInfo;
            xhtmlToSqlData.XhtmlToBlog();

            WebonaryMysqlDatabaseTransfer webonaryMysql = new WebonaryMysqlDatabaseTransfer();
            webonaryMysql.projInfo = projInfo;
            webonaryMysql.CreateDatabase("CreateUser-Db.sql", "sym147_Webroot", "pathway1234", "204.93.172.30", "3306", "samdoss");

            webonaryMysql.InstallWordPressPHPPage("http://pathwaywebonary.com.cws10.my-hosting-panel.com", "samdoss123", "Sam Wordpress", "Samdoss", "arthur", "samdoss@live.com", "1");

            webonaryMysql.Drop2reset("drop2reset.sql", "sym147_Webroot", "pathway1234", "204.93.172.30", "3306", "sym147_webonary");

            webonaryMysql.EmptyWebonary("EmptyWebonary.sql", "sym147_Webroot", "pathway1234", "204.93.172.30", "3306", "sym147_webonary", "http://pathwaywebonary.com.cws10.my-hosting-panel.com", "samdoss123", "Webonary Site");

            webonaryMysql.Data("data.sql", "sym147_Webroot", "pathway1234", "204.93.172.30", "3306", "sym147_webonary", "http://pathwaywebonary.com.cws10.my-hosting-panel.com", "samdoss123");

            MessageBox.Show("Data Exported to Wordpress.");
        }

        private void btnEpub_Click(object sender, EventArgs e)
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
            PublicationInformation projInfo = new PublicationInformation();

            string xhtmlFileName = txtInputPath.Text;
            string cssFileName = txtCSSInput.Text;

            projInfo.DefaultXhtmlFileWithPath = xhtmlFileName;
            projInfo.DefaultCssFileWithPath = cssFileName;

            string inputHtmlFileName = xhtmlFileName;
            inputHtmlFileName = Path.GetFileName(inputHtmlFileName);
            if (!inputHtmlFileName.ToLower().Contains("main"))
            {
                File.Copy(xhtmlFileName, xhtmlFileName.Replace(".", "main."));
                xhtmlFileName = xhtmlFileName.Replace(".", "main.");
                projInfo.DefaultXhtmlFileWithPath = xhtmlFileName;
            }

            string inputCssFileName = cssFileName;
            inputCssFileName = Path.GetFileName(inputCssFileName);
            if (!inputCssFileName.ToLower().Contains("main"))
            {
                File.Copy(cssFileName, cssFileName.Replace(".css", "main.css"));
                cssFileName = cssFileName.Replace(".css", "main.css");
                projInfo.DefaultCssFileWithPath = cssFileName;
            }

            Exportepub epub = new Exportepub();

            string ProjType = "Dictionary";

            if (radScripture.Checked)
            {
                ProjType = "Scripture";
            }


            projInfo.FinalOutput = "epub";
            projInfo.ProjectInputType = ProjType;
            projInfo.ProjectPath = Path.GetDirectoryName(xhtmlFileName);
            projInfo.DictionaryPath = Path.GetDirectoryName(xhtmlFileName);

            projInfo.ProgressBar = new ProgressBar();
            projInfo.DictionaryOutputName = "test";

            ProjType = Common.GetProjectType(projInfo.DefaultXhtmlFileWithPath);
            projInfo.ProjectInputType = ProjType;

            //Param.LoadSettings();
            //Param.Value[Param.InputType] = projInfo.ProjectInputType;
            //Param.LoadSettings();
            //// the user has specified an output -- update the settings so we export to that output
            ////Param.SetDefaultValue(Param.PrintVia, exportType);
            //Param.SetValue(Param.PrintVia, "E-Book (.epub)");
            //Param.Write();


            //var tpe = new PsExport { Destination = "E-Book (.epub)", DataType = projInfo.ProjectInputType };
            //tpe.ProgressBar = null;
            //tpe.Export(projInfo.DefaultXhtmlFileWithPath);


            projInfo.IsReversalExist = false;
            projInfo.IsLexiconSectionExist = true;


            string getDirectoryName = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath);
            projInfo.DefaultRevCssFileWithPath = Path.Combine(getDirectoryName, "flexrev.css");

            projInfo.ProjectName = "EBook (epub)_" + DateTime.Now.Date.ToShortDateString() + "_" +
                                   DateTime.Now.Date.ToShortTimeString();

            Exportepub epubConvert = new Exportepub();
            epubConvert.TocLevel = "1";
            epubConvert.Export(projInfo);
        }

        private void btnSty2XML_Click(object sender, EventArgs e)
        {
#if !Not7
            StyToXML styToCss = new StyToXML();
            styToCss.StyFullPath = txtCSSInput.Text;
            string outputCSS = txtCSSInput.Text.Replace(".sty", ".XML");
            styToCss.ConvertStyToXML("TestBed", outputCSS);
            MessageBox.Show("Exported in " + outputCSS);
#endif
        }

        private void btnDBL_Metadata_Click(object sender, EventArgs e)
        {
#if !Not7
            DBLMetadata dblMetadata = new DBLMetadata();
            dblMetadata.StyFullPath = txtCSSInput.Text;
            string outputCSS = Path.Combine(Path.GetDirectoryName(txtCSSInput.Text), "metadata.XML");
            dblMetadata.CreateMetadata("TestBed", outputCSS);
            MessageBox.Show("Exported in " + outputCSS);
#endif
        }

        private void btnUsx2SFM_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtInputPath.Text))
            {
                MessageBox.Show("Please enter the valid USX file");
                return;
            }

            UsxToSFM usxToSfm = new UsxToSFM();
            usxToSfm.ConvertUsxToSFM(txtInputPath.Text, Path.GetDirectoryName(txtInputPath.Text) + "\\output.sfm");
        }

        private void btnRemoveBom_Click(object sender, EventArgs e)
        {
            if (File.Exists(txtInputPath.Text))
            {
                string temporaryCvFullName = txtInputPath.Text;
                var utf8WithoutBom = new UTF8Encoding(false);
                string noBom = txtInputPath.Text;
                noBom = noBom.Replace(".", "bom.");
                var reader = new StreamReader(temporaryCvFullName);
                var writer = new StreamWriter(noBom, false, utf8WithoutBom);
                writer.Write(reader.ReadToEnd());
                reader.Close();
                writer.Close();

                MessageBox.Show("Updated sucessfully.");
            }
        }

        private void btnSfm2Usx_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtInputPath.Text))
            {
                MessageBox.Show("Please enter the valid USX file");
                return;
            }

            SFMtoUsx sfMtoUsx = new SFMtoUsx();
            sfMtoUsx.ConvertSFMtoUsx(txtInputPath.Text, Path.GetDirectoryName(txtInputPath.Text) + "\\output.usx");
            MessageBox.Show("Done");
        }

        private void btnPrinceExport_Click(object sender, EventArgs e)
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
            ExportPdf exportPDFPrince = new ExportPdf();
            PublicationInformation projInfo = new PublicationInformation();

            projInfo.ProjectPath = Path.GetDirectoryName(txtInputPath.Text);
            projInfo.DefaultXhtmlFileWithPath = txtInputPath.Text;
            projInfo.DefaultCssFileWithPath = txtCSSInput.Text;
            projInfo.ProjectInputType = radDictionary.Checked ? "Dictionary" : "Scripture";
            projInfo.ProjectFileWithPath = projInfo.ProjectPath;
            projInfo.DictionaryPath = projInfo.ProjectPath;
            exportPDFPrince.Export(projInfo);
        }

        private void button8_Click_1(object sender, EventArgs e)
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
            var exportDictionaryForMIDs = new ExportDictionaryForMIDs();
            PublicationInformation projInfo = new PublicationInformation();

            projInfo.ProjectPath = Path.GetDirectoryName(txtInputPath.Text);
            projInfo.DefaultXhtmlFileWithPath = txtInputPath.Text;
            projInfo.DefaultCssFileWithPath = txtCSSInput.Text;

            projInfo.ProjectFileWithPath = projInfo.ProjectPath;
            projInfo.DictionaryPath = projInfo.ProjectPath;
            exportDictionaryForMIDs.Export(projInfo);
        }

        private void btnEpubValidator_Click(object sender, EventArgs e)
        {
            var outputPathWithFileName = txtInputPath.Text;
            // Running the unit test - just run the validator and return the result
            var validationResults = epubValidator.Program.ValidateFile(outputPathWithFileName);
            Debug.WriteLine("Exportepub: validation results: " + validationResults);
            // we've succeeded if epubcheck returns no errors
            //success = (validationResults.Contains("No errors or warnings detected"));

            MessageBox.Show(validationResults);
            var validationDialog = new ValidationDialog();
            validationDialog.FileName = outputPathWithFileName;
            validationDialog.ShowDialog();
        }

        private void btnJson_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtInputPath.Text))
            {
                MessageBox.Show("Please enter the valid XML file");
                return;
            }

            var folderPath = Path.GetDirectoryName(txtInputPath.Text);

            //Ramp ramp = new Ramp();
            //SetRampData(ramp);
            //ramp.Create(Path.Combine(folderPath, "MyTest"), folderPath);

            string x = "eyJjcmVhdGVkX2F0IjoiVHVlLCAyOSBPY3QgMjAxMyAxMzozMzoxOCBHTVQiLCJyYW1wLmlzX3JlYWR5IjoiWSIsImRjLnRpdGxlIjoiRGVmYXVsdCBUaXRsZSIsImJyb2FkX3R5cGUiOiJ2ZXJuYWN1bGFyIiwiZGMudHlwZS5tb2RlIjpbIlRleHQiLCAiR3JhcGhpYyJdLCJkYy5mb3JtYXQubWVkaXVtIjpbIlBhcGVyIl0sImRjLmRlc2NyaXB0aW9uLnN0YWdlIjoicm91Z2hfZHJhZnQiLCJ2ZXJzaW9uLnR5cGUiOiJmaXJzdCIsInJhbXAudmVybmFjdWxhcm1hdGVyaWFsc3R5cGUiOiJzY3JpcHR1cmUiLCJkYy50eXBlLnNjcmlwdHVyZVR5cGUiOiJCaWJsZSB0ZXh0IGNvbXBsZXRlIiwiZGMudGl0bGUuc2NyaXB0dXJlU2NvcGUiOlsiSkFTOkphbWVzIiwgIjFQRToxIFBldGVyIiwgIjJQRToyIFBldGVyIiwgIjFKTjoxIEpvaG4iLCAiMkpOOjIgSm9obiIsICJdLCJkYy50eXBlLnNjaG9sYXJseVdvcmsiOiJCb29rIiwiZGMuc3ViamVjdC5zdWJqZWN0TGFuZ3VhZ2UiOnsiMCI6eyJkaWFsZWN0IiA6ICIiLCAiICI6ICJhYWk6YWFpIn19LCJzdWJqZWN0LnN1YmplY3RMYW5ndWFnZS5oYXMiOiJZIiwiZGMubGFuZ3VhZ2UuaXNvIjp7IjAiOnsiZGlhbGVjdCIgOiAiIiwgIiAiOiAiYWFpOmFhaSJ9fSwiZGMuY29udHJpYnV0b3IiOnsiMCI6eyIgIjogIkdUZXN0IiwgInJvbGUiOiAiY29tcGlsZXIifX0sImRjLmRhdGUubW9kaWZpZWQiOiIyMDEzLTEwLTI5IiwiZm9ybWF0LmV4dGVudC50ZXh0IjoiMCIsImZvcm1hdC5leHRlbnQuaW1hZ2VzIjoiMyIsImRjLnN1YmplY3Quc2lsRG9tYWluIjpbIkxJTkc6TGluZ3Vpc3RpY3MiXSwidHlwZS5kb21haW5TdWJ0eXBlLkxJTkciOlsibGV4aWNvbiAoTElORykiXSwiZGMuc3ViamVjdCI6eyIwIjp7IiAiOiAiQmlibGUiLCAibGFuZyI6ICJlbmcifX0sInJlbGF0aW9uLnJlcXVpcmVzLmhhcyI6IlkiLCJkYy5yZWxhdGlvbi5yZXF1aXJlcyI6eyIwIjp7IiAiOiAiIn19LCJkYy5yZWxhdGlvbi5jb25mb3Jtc3RvIjoiVFRGIiwiZGMucmlnaHRzSG9sZGVyIjp7IjAiOnsiICI6ICJXeWNsaWZmZSBCaWJsZSBUcmFuc2xhdG9ycyJ9fSwiZGMucmlnaHRzIjoiwqkgMjAxMiBXeWNsaWZmZSBCaWJsZSBUcmFuc2xhdG9ycywgSW5jLiAgVGhpcyB3b3JrIGlzIGxpY2Vuc2VkIHVuZGVyIHRoZSBDcmVhdGl2ZSBDb21tb25zIEF0dHJpYnV0aW9uLU5vbkNvbW1lcmNpYWwtTm9EZXJpdnMgMy4wIFVucG9ydGVkIExpY2Vuc2UuVG8gdmlldyBhIGNvcHkgb2YgdGhpcyBsaWNlbnNlLCB2aXNpdCBodHRwOi8vY3JlYXRpdmVjb21tb25zLm9yZy9saWNlbnNlcy9ieS1uYy1uZC8zLjAvIiwic2lsLnNlbnNpdGl2aXR5Lm1ldGFkYXRhIjoiUHVibGljIiwic2lsLnNlbnNpdGl2aXR5LnByZXNlbnRhdGlvbiI6IlB1YmxpYyIsInNpbC5zZW5zaXRpdml0eS5zb3VyY2UiOiJJbnNpdGUgdXNlcnMiLCJmaWxlcyI6eyIwIjp7IiAiOiAiMTkxNjU3MjAyNm1lcmdlZEE1QXBwbGljYXRpb25TdHlsZXMuY3NzIiwgImRlc2NyaXB0aW9uIjogIjE5MTY1NzIwMjZtZXJnZWRBNUFwcGxpY2F0aW9uU3R5bGVzIHN0eWxlc2hlZXQiLCAicmVsYXRpb25zaGlwIjogInNvdXJjZSJ9LCIxIjp7IiAiOiAiQTVBcHBsaWNhdGlvblN0eWxlcy5vZHQiLCAiZGVzY3JpcHRpb24iOiAiQTVBcHBsaWNhdGlvblN0eWxlcyBvZHQgZG9jdW1lbnQiLCAicmVsYXRpb25zaGlwIjogInByZXNlbnRhdGlvbiIsICJpc19wcmltYXJ5IjogIlkiLCAic2lsUHVibGljIjogIlkifSwiMiI6eyIgIjogIkE1QXBwbGljYXRpb25TdHlsZXMueGh0bWwiLCAiZGVzY3JpcHRpb24iOiAiQTVBcHBsaWNhdGlvblN0eWxlcyBYSFRNTCBmaWxlIiwgInJlbGF0aW9uc2hpcCI6ICJzb3VyY2UifX0sInN0YXR1cyI6InJlYWR5In0=";
            string y = EncodeDecodeBase64(x, "decode");

            MessageBox.Show("Done");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputText">string to encode / decode</param>
        /// <param name="operation">encode/decode</param>
        /// <returns></returns>
        private string EncodeDecodeBase64(string inputText, string operation)
        {
            string convertedText = string.Empty;
            if (operation == "encode")
            {
                byte[] bytesToEncode = Encoding.UTF8.GetBytes(inputText);
                convertedText = Convert.ToBase64String(bytesToEncode);
            }
            else if (operation == "decode")
            {
                byte[] decodedBytes = Convert.FromBase64String(inputText);
                convertedText = Encoding.UTF8.GetString(decodedBytes);
            }
            return convertedText;
        }

        private void SetRampData(Ramp ramp)
        {
            RampFile rampFile;

            //ramp.RampId = "ykmb9i6zlh";
            ramp.CreatedOn = DateTime.Now.ToString("r");
            ramp.Ready = "Y";
            ramp.Title = "Gondwana / English / Telegu / Hindi";
            ramp.BroadType = "wider_audience";
            ramp.TypeMode = "Text,Photograph,Software application";
            ramp.FormatMedium = "Paper,Other";
            ramp.DescStage = "rough_draft";
            ramp.VersionType = "first";
            ramp.TypeScholarlyWork = "Other";
            ramp.AddSubjLanguage("gon: Gondi");
            ramp.CoverageSpacialRegionHas = "Y";
            ramp.AddCoverageSpacialCountry("IN: India, Andhra Pradesh");
            ramp.SubjectLanguageHas = "Y";
            ramp.AddLanguageIso("eng: English");
            ramp.AddLanguageIso("tel: Telugu");
            ramp.AddLanguageIso("hin: Hindi");
            ramp.AddLanguageScript("Latn: Latin");
            ramp.AddLanguageScript("Telu: Telugu");
            ramp.AddLanguageScript("Deva: Devanagari(Nagari)");
            ramp.AddContributor("Mark Penny,researcher");
            ramp.FormatExtentText = "8";
            ramp.FormatExtentImages = "2";
            ramp.DescSponsership = "SIL International";
            ramp.DescTableofContentsHas = " ";
            ramp.SilDomain = "LING: Linguistics";
            ramp.DomainSubTypeLing = "language documentation(LING)";
            ramp.AddSubject("foreign languages and literature;dictionary;lexicon;,eng");
            ramp.RelRequiresHas = "Y";
            ramp.AddRelRequires("OFL");
            ramp.RelConformsto = "odf";
            ramp.AddRightsHolder("© 2013 SIL International®");
            ramp.Rights = "creative commons share alike";
            ramp.SilSensitivityMetaData = "Public";
            ramp.SilSensitivityPresentation = "Public";
            ramp.SilSensitivitySource = "Insite users";

            rampFile = new RampFile();
            rampFile.FileName = "main.odm";
            rampFile.FileDescription = "Master document";
            rampFile.FileRelationship = "presentation";
            rampFile.FileIsPrimary = "Y";
            rampFile.FileSilPublic = "Y";
            ramp.AddFile(rampFile);

            rampFile = new RampFile();
            rampFile.FileName = "main.odt";
            rampFile.FileDescription = "main";
            rampFile.FileRelationship = "presentation";
            rampFile.FileSilPublic = "Y";
            ramp.AddFile(rampFile);

            rampFile = new RampFile();
            rampFile.FileName = "FlexRev.odt";
            rampFile.FileDescription = "English Reversal Index";
            rampFile.FileRelationship = "presentation";
            rampFile.FileSilPublic = "Y";
            ramp.AddFile(rampFile);

            rampFile = new RampFile();
            rampFile.FileName = "main.xhtml";
            rampFile.FileDescription = "main";
            rampFile.FileRelationship = "source";
            ramp.AddFile(rampFile);

            rampFile = new RampFile();
            rampFile.FileName = "FlexRev.xhtml";
            rampFile.FileDescription = "English Reversal Index";
            rampFile.FileRelationship = "source";
            ramp.AddFile(rampFile);

            rampFile = new RampFile();
            rampFile.FileName = "2087191251mergedmain.css";
            rampFile.FileDescription = "main stylesheet";
            rampFile.FileRelationship = "source";
            ramp.AddFile(rampFile);

            rampFile = new RampFile();
            rampFile.FileName = "119888425mergedFlexRev.css";
            rampFile.FileDescription = "Reversal stylesheet";
            rampFile.FileRelationship = "source";
            ramp.AddFile(rampFile);

            ramp.Status = "ready";

        }

        private void button13_Click(object sender, EventArgs e)
        {
            pb = new Progress();
            pb.Show();
        }

        private void FlexPluginTest_Click(object sender, EventArgs e)
        {
            if (pb != null)
            {
                pb.Close();
            }
        }
    }
}