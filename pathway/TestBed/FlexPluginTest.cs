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
            txtInputPath.Text = GetFilePath("XHTML Files|*.xhtml|XML Files|*.xml|Tex|*.tex");
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
            //MessageBox.Show("Code is commented");
            //if (!File.Exists(txtInputPath.Text))
            //{
            //    MessageBox.Show("Please enter the valid XHTML path");
            //    return;
            //}

            //if (!File.Exists(txtCSSInput.Text))
            //{
            //    MessageBox.Show("Please enter the valid CSS path");
            //    return;
            //}

            //Common.Testing = chkPage.Checked;

            //PublicationInformation projInfo = new PublicationInformation();
            //projInfo.ProjectPath = Path.GetDirectoryName(txtInputPath.Text);
            //projInfo.DictionaryPath = Path.GetDirectoryName(txtInputPath.Text);
            //projInfo.DefaultXhtmlFileWithPath = txtInputPath.Text;
            //projInfo.DefaultCssFileWithPath = txtCSSInput.Text;
            //projInfo.ProgressBar = new ProgressBar();
            //projInfo.DictionaryOutputName = "test";
            //projInfo.ProjectInputType = radDictionary.Checked ? "Dictionary" : "Scripture";
            //projInfo.FinalOutput = "odt";
            //projInfo.IsExtraProcessing = true;

            //ExportOpenOffice_OLD exportOdt = new ExportOpenOffice_OLD();
            //exportOdt.Export(projInfo);

            //if (Common.Testing)
            //{
            //    string file = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath) + "\\" + Path.GetFileNameWithoutExtension(projInfo.DefaultXhtmlFileWithPath) + "." + projInfo.FinalOutput;
            //    if (File.Exists(file))
            //        Process.Start(file);
            //}
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

            projInfo.ProjectPath = Path.GetDirectoryName(txtInputPath.Text);
            projInfo.DictionaryPath = Path.GetDirectoryName(txtInputPath.Text);
            projInfo.DefaultXhtmlFileWithPath = txtInputPath.Text;
            projInfo.DefaultCssFileWithPath = txtCSSInput.Text;
            projInfo.ProgressBar = new ProgressBar();
            projInfo.DictionaryOutputName = "test";

            ProjType = Common.GetProjectType(projInfo.DefaultXhtmlFileWithPath);
            projInfo.ProjectInputType = ProjType;

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
            var target = new ExportLogos();
            PublicationInformation projInfo = new PublicationInformation();
            projInfo.DefaultXhtmlFileWithPath = txtInputPath.Text;
            projInfo.DefaultCssFileWithPath = txtCSSInput.Text;

            var actual = target.Export(projInfo);
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
            else if (Common.GetOsName().ToUpper() == "UNIX")
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

        private void SetToPHP(string UserSystemGuid, string OSNameVersion, string OSServicePack, string UserSystemName, string UserIPAddress,
            string PathwayVersion, string JavaVersion, string TEVersion, string LibraofficeVersion, string Prince, string XelatexVersion,
            string IndesignVersion, string WeSay, string Bloom, string FontLists, string BrowserList, string GeoLocation, string Language, string FrameworkVersion)
        {

            // this is what we are sending
            string post_data = "&FrameworkVersionsss=sdf";

            // this is where we will send it
            //string uri = "http://myphpapps.com.cws10.my-hosting-panel.com/configdb.php?firstname=ram&lastname=ar&age=25";
            string uri = "http://myphpapps.com.cws10.my-hosting-panel.com/configdb.php" + "?UserSystemGuid=" +
                         UserSystemGuid +
                         "&OSNameVersion=" + OSNameVersion + "&OSServicePack=" + OSServicePack +
                         "&UserSystemName=" + UserSystemName + "&UserIPAddress=" + UserIPAddress + "&PathwayVersion=" +
                         PathwayVersion + "&JavaVersion=" + JavaVersion + "&TEVersion=" + TEVersion + "&LibraofficeVersion=" + LibraofficeVersion +
                         "&Prince=" + Prince + "&XelatexVersion=" + XelatexVersion + "&IndesignVersion=" + IndesignVersion + "&WeSay=" + WeSay +
                         "&Bloom=" + Bloom + "&FontLists=" + FontLists + "&BrowserList=" + BrowserList + "&GeoLocation=" + GeoLocation +
                         "&Language=" + Language + "&FrameworkVersion=" + FrameworkVersion + "";

            // create a request
            HttpWebRequest request = (HttpWebRequest)
                                     WebRequest.Create(uri);
            request.KeepAlive = false;
            request.ProtocolVersion = HttpVersion.Version10;
            request.Method = "POST";

            // turn our request string into a byte stream
            byte[] postBytes = Encoding.ASCII.GetBytes(post_data);

            // this is important - make sure you specify type this way
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postBytes.Length;
            Stream requestStream = request.GetRequestStream();

            // now send it
            requestStream.Write(postBytes, 0, postBytes.Length);
            requestStream.Close();

            // grab te response and print it out to the console along with the status code
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Console.WriteLine(new StreamReader(response.GetResponseStream()).ReadToEnd());
            Console.WriteLine(response.StatusCode);


        }

        private void btnGetSoftwareData_Click(object sender, EventArgs e)
        {
            try
            {

                string UserSystemGuid = string.Empty; //200
                string OSName = string.Empty; //100
                string OSNameVersion = string.Empty;
                string OSServicePack = string.Empty;
                string UserSystemName = string.Empty; //100
                string UserIPAddress = string.Empty; //100
                string PathwayVersion = string.Empty; //45
                string TEVersion = string.Empty; //45
                string JavaVersion = string.Empty; //45
                string LibraofficeVersion = string.Empty; //45
                string Prince = string.Empty; //45
                string XelatexVersion = string.Empty; //45
                string IndesignVersion = string.Empty; //45
                string WeSay = string.Empty; //45
                string Bloom = string.Empty; //45
                string FontLists = string.Empty; //500
                string BrowserList = string.Empty; //200
                string GeoLocation = string.Empty; //100
                string Language = string.Empty; //45
                string FrameworkVersion = string.Empty; //100
                string Paratext = string.Empty;

                UserSystemGuid = Guid.NewGuid().ToString();
                OSNameVersion = GetOsName() + " - " + GetOsVersion();
                OSName = GetOsName();
                OSServicePack = GetOsVersion();
                UserSystemName = Environment.MachineName;

                IPHostEntry ip = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ipAddress in ip.AddressList)
                {
                    if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                    {
                        UserIPAddress = ipAddress.ToString();
                    }
                }

                /* Pathway Version  */

                string appName = Assembly.GetAssembly(this.GetType()).Location;
                AssemblyName assemblyName = AssemblyName.GetAssemblyName(appName);
                PathwayVersion = assemblyName.Version.ToString();

                /* Java Version  */

                if (OSName == "Windows7")
                {
                    JavaVersion =
                        Common.GetValueFromRegistry("SOFTWARE\\Wow6432Node\\JavaSoft\\Java Runtime Environment",
                                                    "CurrentVersion");
                }
                else if (OSName == "Windows XP")
                {
                    JavaVersion = Common.GetValueFromRegistry("SOFTWARE\\JavaSoft\\Java Runtime Environment",
                                                              "CurrentVersion");
                }

                if (OSName == "Windows7")
                {
                    XelatexVersion = Common.GetValueFromRegistry("SOFTWARE\\Wow6432Node\\SIL\\PathwayXeLaTeX", "XeLaTexVer");
                }
                else if (OSName == "Windows XP")
                {
                    XelatexVersion = Common.GetValueFromRegistry("SOFTWARE\\SIL\\PathwayXeLaTeX", "XeLaTexVer");
                }

                if (OSName == "Windows7")
                {
                    PathwayVersion = Common.GetValueFromRegistry("SOFTWARE\\Wow6432Node\\SIL\\Pathway", "PathwayDir");
                    PathwayVersion = Common.RightRemove(PathwayVersion, "\\");

                    PathwayVersion = Common.LeftRemove(PathwayVersion, "SIL\\");

                }
                else if (OSName == "Windows XP")
                {
                    PathwayVersion = Common.GetValueFromRegistry("SOFTWARE\\SIL\\Pathway", "PathwayDir");
                    PathwayVersion = Common.RightRemove(PathwayVersion, "\\");

                    PathwayVersion = Common.LeftRemove(PathwayVersion, "SIL\\");
                }

                if (OSName == "Windows7")
                {
                    LibraofficeVersion =
                        Common.GetValueFromRegistry("SOFTWARE\\Wow6432Node\\LibreOffice\\UNO\\InstallPath",
                                                    "");
                }
                else if (OSName == "Windows XP")
                {
                    LibraofficeVersion = Common.GetValueFromRegistry("SOFTWARE\\SIL\\PathwayXeLaTeX",
                                                                     "");
                }

                if (OSName == "Windows7")
                {
                    Paratext = Common.GetValueFromRegistry("SOFTWARE\\Wow6432Node\\ScrChecks\\1.0\\Program_Files_Directory_Ptw7", "");
                }
                else if (OSName == "Windows XP")
                {
                    Paratext = Common.GetValueFromRegistry("SOFTWARE\\ScrChecks\\1.0\\Program_Files_Directory_Ptw7", "");
                }

                if (OSName == "Windows7")
                {
                    TEVersion = Common.GetValueFromRegistry("SOFTWARE\\Wow6432Node\\SIL\\FieldWorks\\7.0", "RootCodeDir");

                    TEVersion = Common.RightRemove(TEVersion, "\\");

                    TEVersion = Common.LeftRemove(TEVersion, "SIL\\");

                }
                else if (OSName == "Windows XP")
                {
                    TEVersion = Common.GetValueFromRegistry("SOFTWARE\\SIL\\FieldWorks\\7.0", "RootCodeDir");

                    TEVersion = Common.RightRemove(TEVersion, "\\");

                    TEVersion = Common.LeftRemove(TEVersion, "SIL\\");
                }

                if (OSName == "Windows7")
                {
                    Prince = Common.GetValueFromRegistry("SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Prince_is1", "DisplayName");
                    
                }
                else if (OSName == "Windows XP")
                {
                    Prince = Common.GetValueFromRegistry("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Prince_is1", "DisplayName");
                }

                if (OSName == "Windows7")
                {
                    IndesignVersion = Common.GetValueFromRegistry("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\InDesign.exe", "Path");
                    IndesignVersion = Common.LeftRemove(IndesignVersion, "Adobe\\");
                }
                else if (OSName == "Windows XP")
                {
                    IndesignVersion = Common.GetValueFromRegistry("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\InDesign.exe", "Path");
                    IndesignVersion = Common.LeftRemove(IndesignVersion, "Adobe\\");
                }


                Language = CultureInfo.CurrentCulture.EnglishName;
                GeoLocation = Language.Substring(Language.IndexOf('(') + 1,
                                                 Language.LastIndexOf(')') - Language.IndexOf('(') - 1);

                if (OSName == "Windows7")
                {
                    BrowserList = GetValueFromRegistryHTTPCLASSROOT("http\\shell\\open\\command", "");

                    BrowserList = Common.RightRemove(BrowserList,".exe");

                    BrowserList = Common.LeftRemove(BrowserList, "\\");

                    //if(BrowserList.Contains("Chrome"))
                    //{
                    //    BrowserList = "Default Browser";
                    //    BrowserList = BrowserList + " - " +  GetValueFromRegistryHTTPCurrentUser("Software\\Google\\Update\\Clients\\{8A69D345-D564-463c-AFF1-A69D9E530F96}", "name");
                    //    BrowserList = BrowserList + " - " + GetValueFromRegistryHTTPCurrentUser("Software\\Google\\Update\\Clients\\{8A69D345-D564-463c-AFF1-A69D9E530F96}", "pv");
                    //}
                    //else
                    //{
                    //    BrowserList = "";
                    //}

                    //BrowserList = BrowserList + ", IE Version " +
                    //              Common.GetValueFromRegistry("SOFTWARE\\Wow6432Node\\Microsoft\\Internet Explorer",
                    //                                          "Version");
                }
                else if (OSName == "Windows XP")
                {
                    BrowserList = GetValueFromRegistryHTTPCLASSROOT("http\\shell\\open\\command", "");
                    BrowserList = Common.RightRemove(BrowserList, ".exe");

                    BrowserList = Common.LeftRemove(BrowserList, "\\");
                    //if (BrowserList.Contains("Chrome"))
                    //{
                    //    BrowserList = "Default Browser";
                    //    BrowserList = BrowserList + " - " + GetValueFromRegistryHTTPCurrentUser("Software\\Google\\Update\\Clients\\{8A69D345-D564-463c-AFF1-A69D9E530F96}", "name");
                    //    BrowserList = BrowserList + " - " + GetValueFromRegistryHTTPCurrentUser("Software\\Google\\Update\\Clients\\{8A69D345-D564-463c-AFF1-A69D9E530F96}", "pv");
                    //}
                    //else
                    //{
                    //    BrowserList = "";
                    //}

                    //BrowserList = BrowserList + ", IE Version " +
                    //              Common.GetValueFromRegistry("SOFTWARE\\Microsoft\\Internet Explorer", "Version");
                }

                if (OSName == "Windows7")
                {
                    RegistryKey installedVersions =
                        Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\Wow6432Node\\Microsoft\\NET Framework Setup\\NDP");
                    if (installedVersions != null)
                    {
                        string[] versionNames = installedVersions.GetSubKeyNames();
                        FrameworkVersion =
                            Convert.ToDouble(versionNames[versionNames.Length - 1].Remove(0, 1),
                                             CultureInfo.InvariantCulture)
                                .ToString(CultureInfo.InvariantCulture);
                    }
                }
                else if (OSName == "Windows XP")
                {
                    RegistryKey installedVersions =
                        Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\Microsoft\\NET Framework Setup\\NDP");
                    if (installedVersions != null)
                    {
                        string[] versionNames = installedVersions.GetSubKeyNames();
                        FrameworkVersion =
                            Convert.ToDouble(versionNames[versionNames.Length - 1].Remove(0, 1),
                                             CultureInfo.InvariantCulture)
                                .ToString(CultureInfo.InvariantCulture);
                    }
                }

                SetToPHP(UserSystemGuid, OSName, OSServicePack, UserSystemName, UserIPAddress, PathwayVersion,
                         JavaVersion, TEVersion, LibraofficeVersion, Prince, XelatexVersion, IndesignVersion, WeSay,
                         Bloom, FontLists, BrowserList, GeoLocation, Language, FrameworkVersion);

            }
            catch { }
        }
      
        public string GetOsName()
        {
            OperatingSystem osInfo = Environment.OSVersion;

            switch (osInfo.Platform)
            {
                case System.PlatformID.Win32NT:
                    switch (osInfo.Version.Major)
                    {
                        case 3:
                            return "Windows NT 3.51";
                            break;
                        case 4:
                            return "Windows NT 4.0";
                            break;
                        case 5:
                            if (osInfo.Version.Minor == 0)
                                return "Windows 2000";
                            else
                                return "Windows XP";
                            break;
                        case 6:
                            if (osInfo.Version.Minor == 1)
                                return "Windows7";
                            break;
                    }
                    break;

            }
            return osInfo.Platform.ToString();
        }

        public string GetOsVersion()
        {
            OperatingSystem osInfo = Environment.OSVersion;

            return osInfo.VersionString;
        }

        private void TestDataInsertToDB()
        {
            string error_Message;
            string connectionString = "Server=localhost;Database=pathwayusers;Uid=root;Pwd=123456;";
            using (MySqlConnection con1 = new MySqlConnection(connectionString))
            {
                try
                {
                    string storedprocedurename = "spAddNewTable";
                    MySqlCommand command = con1.CreateCommand(); //new MySqlCommand(sqlCommand, con1);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = storedprocedurename;
                    command.Parameters.Add(new MySqlParameter("newtablevalue", MySqlDbType.VarChar, 45)).Value =
                        "Pathway";
                    command.CommandTimeout = 500;
                    con1.Open();

                    command.ExecuteNonQuery();
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    switch (ex.Number)
                    {
                        case 0:
                            error_Message = "Cannot connect to server.  Contact administrator";
                            break;
                        case 1045:
                            error_Message = "Invalid username/password, please try again";
                            break;
                    }
                }
                catch (Exception ex1)
                {
                    error_Message = ex1.Message.ToString();
                }
                finally
                {
                    con1.Close();
                }
            }
        }

        private void UserDataInsertToDB()
        {
            string error_Message;
            string connectionString =
                "Server=12.177.127.237;port=3306;Database=pathwayusers;Uid=PathwayBootstrap;Pwd=mKs24SzJDPebRNWj;Protocol=socket;";

            using (MySqlConnection con1 = new MySqlConnection(connectionString))
            {
                try
                {
                    string storedprocedurename = "spAddNewTable";
                    MySqlCommand command = con1.CreateCommand(); //new MySqlCommand(sqlCommand, con1);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = storedprocedurename;
                    command.Parameters.Add(new MySqlParameter("newtablevalue", MySqlDbType.VarChar, 45)).Value =
                        "Samdoss";
                    command.CommandTimeout = 500;
                    con1.Open();

                    command.ExecuteNonQuery();
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    switch (ex.Number)
                    {
                        case 0:
                            error_Message = "Cannot connect to server.  Contact administrator";
                            break;
                        case 1045:
                            error_Message = "Invalid username/password, please try again";
                            break;
                    }
                }
                catch (Exception ex1)
                {
                    error_Message = ex1.Message.ToString();
                }
                finally
                {
                    con1.Close();
                }
            }
        }

        private static string GetUserDataFromRegistry(string subKey, string keyName)
        {
            // Opening the registry key

            RegistryKey rk = Registry.LocalMachine;
            // Open a subKey as read-only

            RegistryKey sk1 = rk.OpenSubKey(subKey);
            // If the RegistrySubKey doesn't exist -> (null)

            if (sk1 == null)
            {
                return null;
            }
            else
            {
                try
                {
                    // If the RegistryKey exists I get its value
                    return (string)sk1.GetValue(keyName.ToUpper());
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        private static string GetValueFromRegistryHTTPCurrentUser(string subKey, string keyName)
        {
            // Opening the registry key

            RegistryKey rk = Registry.CurrentUser;
            // Open a subKey as read-only

            RegistryKey sk1 = rk.OpenSubKey(subKey);
            // If the RegistrySubKey doesn't exist -> (null)

            if (sk1 == null)
            {
                return null;
            }
            else
            {
                try
                {
                    // If the RegistryKey exists I get its value
                    return (string)sk1.GetValue(keyName.ToUpper());
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        private static string GetValueFromRegistryHTTPCLASSROOT(string subKey, string keyName)
        {
            // Opening the registry key

            RegistryKey rk = Registry.ClassesRoot;
            // Open a subKey as read-only

            RegistryKey sk1 = rk.OpenSubKey(subKey);
            // If the RegistrySubKey doesn't exist -> (null)

            if (sk1 == null)
            {
                return null;
            }
            else
            {
                try
                {
                    // If the RegistryKey exists I get its value
                    return (string)sk1.GetValue(keyName.ToUpper());
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        private string GetGeoLocationByIP(string strIPAddress)
        {

            //Create a WebRequest
            WebRequest rssReq = WebRequest.Create("http://freegeoip.appspot.com/xml/" + strIPAddress);

            //Create a Proxy
            WebProxy px = new WebProxy("http://freegeoip.appspot.com/xml/" + strIPAddress, true);

            //Assign the proxy to the WebRequest
            rssReq.Proxy = px;

            //Set the timeout in Seconds for the WebRequest
            rssReq.Timeout = 2000;



            //Get the WebResponse

            WebResponse rep = rssReq.GetResponse();

            //Read the Response in a XMLTextReader

            XmlTextReader xtr = new XmlTextReader(rep.GetResponseStream());

            //Create a new DataSet

            DataSet ds = new DataSet();

            //Read the Response into the DataSet
            //<Status>true</Status>
            //<Ip>72.14.247.141</Ip>
            //<CountryCode>US</CountryCode>
            //<CountryName>United States</CountryName>
            //<RegionCode>CA</RegionCode>
            //<RegionName>California</RegionName>
            //<City>Mountain View</City>
            //<ZipCode>94043</ZipCode>
            //<Latitude>37.4192</Latitude>
            //<Longitude>-122.057</Longitude>

            ds.ReadXml(xtr);
            DataTable dt = ds.Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {

                if (dt.Rows[0]["Status"].ToString().Equals("true", StringComparison.CurrentCultureIgnoreCase))
                {
                    return dt.Rows[0]["CountryCode"].ToString();

                }
            }
            return String.Empty;
        }

    }
}