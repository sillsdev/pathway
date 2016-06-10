// --------------------------------------------------------------------------------------------
// <copyright file="FlexPluginTest.cs" from='2009' to='2009' company='SIL International'>
//      Copyright ( c ) 2009, SIL International. All Rights Reserved.   
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
using System.Text;
using System.Windows.Forms;
using System.Xml;
using JWTools;
using SIL.PublishingSolution;
using SIL.Tool;
using SIL.Tool.Localization;
using epubValidator;
using Test;

namespace TestBed
{
    public partial class FlexPluginTest : Form
    {
        private static string designerPath = "c:/AccessName/";
        private static readonly List<string> FileNames = new List<string>();
        private static string sourceFolder = "c:\\temp";
        private Progress pb;

        public FlexPluginTest()
        {
            InitializeComponent();
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            var fd = new FolderBrowserDialog();
            fd.ShowDialog();
            TxtInput.Text = fd.SelectedPath;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            var fd = new FolderBrowserDialog {ShowNewFolderButton = true};
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
                DirectoryInfo di = new DirectoryInfo(designerPath);
                Common.CleanDirectory(di);
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

        private void BtnTETest_Click(object sender, EventArgs e)
        {
            var plugin = new PsExport {DataType = "Scripture"};
            string outputpath = "c:/1pe/1pe";
            plugin.Export(outputpath);

        }

        private void BtnFlexTest_Click(object sender, EventArgs e)
        {
            PrintVia printVia = new PrintVia();
            printVia.InputType = "Dictionary";
            printVia.ShowDialog();

            string target = printVia.BackEnd; //"OpenOffice";
            var tpe = new PsExport {Destination = target, DataType = printVia.InputType};
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
            projInfo.ProjectFileWithPath = projInfo.ProjectPath;
            projInfo.DictionaryPath = projInfo.ProjectPath;
            exportInDesign.Export(projInfo);
        }

        private void Btn_InputPath_Click(object sender, EventArgs e)
        {
            txtInputPath.Text =
                GetFilePath(
                    "XHTML Files|*.xhtml|XML Files|*.xml|Tex|*.tex|Zip Files|*.zip|usx Files|*.usx|sfm Files|*.sfm|Text Files|*.txt|All Files|*.*");
            txtCSSInput.Text = Path.ChangeExtension(txtInputPath.Text, "css");
        }

        private string GetFilePath(string fileType)
        {
            var fd = new OpenFileDialog();
            fd.Filter = fileType;
	        fd.FilterIndex = 8;
            fd.ShowDialog();

            return fd.FileName;
        }

        private void BtnInputCSS_Click(object sender, EventArgs e)
        {

			txtCSSInput.Text = GetFilePath("STY Files|*.sty|CSS Files|*.css");
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
            FileNames.Clear();
            if (TxtInput.Text.Length <= 0)
            {
                MessageBox.Show("Please select the inputPath");
                return;
            }
            IncreaseFileSizeFromZeroBites(TxtInput.Text);
            MessageBox.Show("Completed for " + FileNames.Count + " files");
            int fileCount = 1;
            using (TextWriter streamWriter =
                new StreamWriter("c:\\filenamesWithZeroBytes.txt"))
            {
                foreach (string fileName in FileNames)
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
                    FileNames.Add(filePath);
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
                string destinationDir = Common.PathCombine(inputPath, dir.Name);

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
            exportOdt.Export(projInfo);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FileNames.Clear();
            sourceFolder = "c:\\temp";
            GetFilesFromFolder(sourceFolder);

            // Get files from Xml
            List<string> fileNamesXml = new List<string>();
            XmlNode returnValue = null;
            XmlDocument LoadedDoc = new XmlDocument();
            LoadedDoc.Load("c:\\FileLibrary.xml");
            const string xPath = "//FileLibrary";
            XmlElement root = LoadedDoc.DocumentElement;
            if (root != null)
            {
                returnValue = root.SelectSingleNode(xPath);
                foreach (XmlNode xmlNode in returnValue.ChildNodes)
                {
                    string path = xmlNode.Attributes.GetNamedItem("Path").Value;
                    fileNamesXml.Add(path);
                }
            }

            //Find missing files in xml 
            foreach (string file in FileNames)
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
                FileNames.Add(fileName);
            }
            foreach (var directoryInfo in dir.GetDirectories())
            {
                if (directoryInfo.Name.Substring(0, 1) == ".svn")
                    continue;
                GetFilesFromFolder(directoryInfo.FullName);
            }

        }

        private void FlexPluginTest_Load(object sender, EventArgs e)
        {
            var supportFolder = PathPart.Bin(Environment.CurrentDirectory, "/../../DistFiles");
            FolderTree.Copy(supportFolder, ".");
           // LocalizationSetup();
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
        }

        private void StyConvert_Click(object sender, EventArgs e)
        {
#if !Not7
            StyToCss styToCss = new StyToCss();
            styToCss.StyFullPath = txtCSSInput.Text;
            string outputCSS = txtCSSInput.Text.Replace(".sty", ".css");
            styToCss.ConvertStyToCss("TestBed", outputCSS, string.Empty);
            MessageBox.Show(@"Exported in " + outputCSS);
#endif
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtInputPath.Text))
            {
                MessageBox.Show("Please enter the valid XHTML path");
                return;
            }
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
            var tpe = new PsExport {Destination = target, DataType = dlg.InputType};
            tpe.Export(txtInputPath.Text);

        }

        private void btnXeLaTex_Click(object sender, EventArgs e)
        {
            //char c = '\u25C6';
            //string hex = ((int)c).ToString("X4"); // Now hex = "0123"
            //string a = "\u25C6";

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
            var exportXeLaTex = new ExportXeLaTex();
            var projInfo = new PublicationInformation();

            projInfo.ProjectPath = Path.GetDirectoryName(txtInputPath.Text);
            projInfo.DefaultXhtmlFileWithPath = txtInputPath.Text;
            projInfo.DefaultCssFileWithPath = txtCSSInput.Text;
            projInfo.ProjectInputType = radDictionary.Checked ? "Dictionary" : "Scripture";

            projInfo.DefaultRevCssFileWithPath = txtCSSInput.Text.Replace("main", "flexrev");
            if (projInfo.ProjectInputType.ToLower() == "dictionary")
                projInfo.IsReversalExist = true;

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
                File.Copy(xhtmlFileName, xhtmlFileName.Replace(".", "main."), true);
                xhtmlFileName = xhtmlFileName.Replace(".", "main.");
                projInfo.DefaultXhtmlFileWithPath = xhtmlFileName;
            }

            string inputCssFileName = cssFileName;
            inputCssFileName = Path.GetFileName(inputCssFileName);
            if (!inputCssFileName.ToLower().Contains("main"))
            {
                File.Copy(cssFileName, cssFileName.Replace(".css", "main.css"), true);
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
            projInfo.IsReversalExist = false;
            projInfo.IsLexiconSectionExist = true;


            string getDirectoryName = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath);
            projInfo.DefaultRevCssFileWithPath = Common.PathCombine(getDirectoryName, "flexrev.css");

            projInfo.ProjectName = "EBook (epub)_" + DateTime.Now.Date.ToShortDateString() + "_" +
                                   DateTime.Now.Date.ToShortTimeString();

            Exportepub epubConvert = new Exportepub();
            epubConvert.TocLevel = "1";
            epubConvert.Export(projInfo);
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

            SfmToUsx sfMtoUsx = new SfmToUsx();
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
            //ramp.Create(Common.PathCombine(folderPath, "MyTest"), folderPath);

            string x =
                "eyJjcmVhdGVkX2F0IjoiVGh1LCAzMSBPY3QgMjAxMyAwODo0Nzo0OCBHTVQiLCJyYW1wLmlzX3JlYWR5IjoiWSIsImRjLnRpdGxlIjoiVGVzdCIsImJyb2FkX3R5cGUiOiJ2ZXJuYWN1bGFyIiwiZGMudHlwZS5tb2RlIjpbIlRleHQiXSwiZGMuZm9ybWF0Lm1lZGl1bSI6WyJQYXBlciJdLCJkYy5kZXNjcmlwdGlvbi5zdGFnZSI6InJvdWdoX2RyYWZ0IiwidmVyc2lvbi50eXBlIjoiZmlyc3QiLCJyYW1wLnZlcm5hY3VsYXJtYXRlcmlhbHN0eXBlIjoic2NyaXB0dXJlIiwiZGMudHlwZS5zY3JpcHR1cmVUeXBlIjoiQmlibGUgdGV4dCBjb21wbGV0ZSIsImRjLnRpdGxlLnNjcmlwdHVyZVNjb3BlIjpbIldOVDpOZXcgVGVzdGFtZW50Il0sImRjLnR5cGUuc2Nob2xhcmx5V29yayI6IkJvb2siLCJkYy5zdWJqZWN0LnN1YmplY3RMYW5ndWFnZSI6eyIwIjp7ImRpYWxlY3QiIDogIiIsICIgIjogImF2dDphdnQifX0sInN1YmplY3Quc3ViamVjdExhbmd1YWdlLmhhcyI6IlkiLCJkYy5sYW5ndWFnZS5pc28iOnsiMCI6eyJkaWFsZWN0IiA6ICIiLCAiICI6ICJhdnQ6YXZ0In19LCJkYy5jb250cmlidXRvciI6eyIwIjp7IiAiOiAiQmlsbCBEeWNrIiwgInJvbGUiOiAiY29tcGlsZXIifX0sImRjLmRhdGUubW9kaWZpZWQiOiIyMDEzLTEwLTMxIiwiZm9ybWF0LmV4dGVudC50ZXh0IjoiMCIsImZvcm1hdC5leHRlbnQuaW1hZ2VzIjoiMCIsImRjLnN1YmplY3Quc2lsRG9tYWluIjpbIkxJTkc6TGluZ3Vpc3RpY3MiXSwidHlwZS5kb21haW5TdWJ0eXBlLkxJTkciOlsibGV4aWNvbiAoTElORykiXSwiZGMuc3ViamVjdCI6eyIwIjp7IiAiOiAiQmlibGUiLCAibGFuZyI6ICJlbmcifX0sImRjLnJlbGF0aW9uLmNvbmZvcm1zdG8iOiJUVEYiLCJkYy5yaWdodHNIb2xkZXIiOnsiMCI6eyIgIjogIld5Y2xpZmZlIEJpYmxlIFRyYW5zbGF0b3JzIn19LCJkYy5yaWdodHMiOiLCqSAyMDEzIFNJTCBJbnRlcm5hdGlvbmFsLiBUaGlzIHdvcmsgaXMgbGljZW5zZWQgdW5kZXIgdGhlIENyZWF0aXZlIENvbW1vbnMgQXR0cmlidXRpb24tTm9uQ29tbWVyY2lhbC1TaGFyZUFsaWtlIDMuMCBVbnBvcnRlZCBMaWNlbnNlLlRvIHZpZXcgYSBjb3B5IG9mIHRoaXMgbGljZW5zZSwgdmlzaXQgaHR0cDovL2NyZWF0aXZlY29tbW9ucy5vcmcvbGljZW5zZXMvYnktbmMtc2EvMy4wLyIsInNpbC5zZW5zaXRpdml0eS5tZXRhZGF0YSI6IlB1YmxpYyIsInNpbC5zZW5zaXRpdml0eS5wcmVzZW50YXRpb24iOiJQdWJsaWMiLCJzaWwuc2Vuc2l0aXZpdHkuc291cmNlIjoiSW5zaXRlIHVzZXJzIiwiZmlsZXMiOnsiMCI6eyIgIjogIjE5ODUzNDYwMTNtZXJnZWRDNV9Db2xzX0FwcGxpY2F0aW9uU3R5bGVzLmNzcyIsICJkZXNjcmlwdGlvbiI6ICIxOTg1MzQ2MDEzbWVyZ2VkQzVfQ29sc19BcHBsaWNhdGlvblN0eWxlcyBzdHlsZXNoZWV0IiwgInJlbGF0aW9uc2hpcCI6ICJzb3VyY2UifSwiMSI6eyIgIjogIkM1X0NvbHNfQXBwbGljYXRpb25TdHlsZXMub2R0IiwgImRlc2NyaXB0aW9uIjogIkM1X0NvbHNfQXBwbGljYXRpb25TdHlsZXMgb2R0IGRvY3VtZW50IiwgInJlbGF0aW9uc2hpcCI6ICJwcmVzZW50YXRpb24iLCAiaXNfcHJpbWFyeSI6ICJZIiwgInNpbFB1YmxpYyI6ICJZIn0sIjIiOnsiICI6ICJDNV9Db2xzX0FwcGxpY2F0aW9uU3R5bGVzLnhodG1sIiwgImRlc2NyaXB0aW9uIjogIkM1X0NvbHNfQXBwbGljYXRpb25TdHlsZXMgWEhUTUwgZmlsZSIsICJyZWxhdGlvbnNoaXAiOiAic291cmNlIn19LCJkZXNjcmlwdGlvbi5oYXMiOiJZIiwiZGMuZGVzY3JpcHRpb24iOnsiMCI6eyIgIiA6ICJOZXcgVGVzdGFtZW50IiwgImxhbmciOiAiZW5nIn19LCJzdGF0dXMiOiJyZWFkeSJ9";
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

        private void btnOsis_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtInputPath.Text))
            {
                MessageBox.Show("Please enter the valid USX file");
                return;
            }

			UsxToOSIS usxToSis = new UsxToOSIS();
			string output = txtInputPath.Text.Replace(".usx", ".xml");
			usxToSis.ConvertUsxToOSIS(txtInputPath.Text, output);
			//usxToSis.ConvertUsxToOSIS(txtInputPath.Text, output, "eng");
			MessageBox.Show("Done");

        }

        private void btnSword_Click(object sender, EventArgs e)
        {

            if (!File.Exists(txtInputPath.Text))
            {
                MessageBox.Show("Please select the valid usx file");
                return;
            }

            if (Path.GetExtension(txtInputPath.Text) != ".usx")
            {
                MessageBox.Show("Please select the valid usx file");
                return;
            }

            string directoryName = string.Empty;
            directoryName = Path.GetFileName(Path.GetDirectoryName(txtInputPath.Text));
            if (directoryName.ToLower() != "usx")
            {
                MessageBox.Show("Folder name should be [usx]");
                return;
            }


            string xhtmlFile = string.Empty;
            if (File.Exists(txtInputPath.Text))
            {
                xhtmlFile = Path.GetDirectoryName(txtInputPath.Text);
                xhtmlFile = Path.GetDirectoryName(xhtmlFile);
                xhtmlFile = Common.PathCombine(xhtmlFile, "Test.xhtml");
            }

            ExportSword swordObj = new ExportSword();
            PublicationInformation projInfo = new PublicationInformation();
            projInfo.ProjectPath = Path.GetDirectoryName(xhtmlFile);
            projInfo.DefaultXhtmlFileWithPath = xhtmlFile;
            projInfo.ProjectFileWithPath = projInfo.ProjectPath;
            swordObj.Export(projInfo);
            MessageBox.Show("Done");
        }

        private void btnHtml5_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtInputPath.Text))
            {
                MessageBox.Show("Please select the valid usx file");
                return;
            }
            string postData = "";
            if (File.Exists(txtInputPath.Text))
            {
                StreamReader reader = new StreamReader(txtInputPath.Text);
                do
                {
                    postData += reader.ReadLine();
                } while (reader.Peek() != -1);
                reader.Close();

                const string webApiurl = "http://html5.validator.nu?showsource=yes&level=error";
                string responseContent = Common.ExecuteWebAPIRequest(webApiurl, postData);
                if (responseContent != null)
                {
                    string filename = string.Format(@"{0}\{1}", System.IO.Path.GetTempPath(), "result.html");
                    File.WriteAllText(filename, responseContent);
                    Process.Start(filename);
                }
            }
        }
    }
}