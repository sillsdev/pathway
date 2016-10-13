// --------------------------------------------------------------------------------------------
// <copyright file="ExportDictionaryForMIDs.cs" from='2013' to='2014' company='SIL International'>
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
// Stylepick FeatureSheet
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Xml;
using ICSharpCode.SharpZipLib.Zip;
using L10NSharp;
using SilTools;
using SIL.Tool;


namespace SIL.PublishingSolution
{
    public class ExportDictionaryForMIDs : IExportProcess
    {
        private const string LogName = "Convert.log";
        private static DictionaryForMIDsInput _DictionaryForMIDsInput;
        protected static string WorkDir;
        protected static Dictionary<string, Dictionary<string, string>> CssClass;
        protected static DictionaryForMIDsStyle ContentStyles = new DictionaryForMIDsStyle();
		protected static bool _isUnixOS = false;

        #region Properties
        #region ExportType
        public string ExportType
        {
            get
            {
                return "DictionaryForMIDs";
            }
        }
        #endregion ExportType

        #region Handle
        public bool Handle(string inputDataType)
        {
            return (inputDataType.ToLower() == "dictionary");
        }
        #endregion Handle
        #endregion Properties

        /// <summary>
        /// Entry point for Dictionary for Mids export
        /// </summary>
        /// <param name="exportType">scripture / dictionary</param>
        /// <param name="publicationInformation">structure with other necessary information about project.</param>
        /// <returns></returns>
        protected void Launch(string exportType, PublicationInformation publicationInformation)
        {
            Export(publicationInformation);
        }

        public bool Export(PublicationInformation projInfo)
        {
            bool success = false;
            projInfo.OutputExtension = "jar";
            if (projInfo == null || string.IsNullOrEmpty(projInfo.DefaultXhtmlFileWithPath) ||
                string.IsNullOrEmpty(projInfo.DefaultCssFileWithPath))
            {
                return false;
            }
            WorkDir = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath);
            _isUnixOS = Common.UnixVersionCheck();
            var inProcess = new InProcess(0, 8);
            var curdir = Environment.CurrentDirectory;
            var myCursor = Cursor.Current;
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                inProcess.Show();
                inProcess.PerformStep();

                LoadParameters();
                inProcess.PerformStep();

                LoadCss(projInfo);
                _DictionaryForMIDsInput = null;
                inProcess.PerformStep();

                ReformatData(projInfo);
                inProcess.PerformStep();

                CreateProperties(projInfo);
                inProcess.PerformStep();

                CreateDictionaryForMIDs(projInfo);
                inProcess.PerformStep();

                CreateSubmission(projInfo);
                inProcess.PerformStep();

                ReportReults(projInfo);
                inProcess.PerformStep();
                success = true;
            }
            catch (Exception ex)
            {
                var msg = LocalizationManager.GetString("ExportDictionaryForMIDs.ExportClick.Message", "{0} Display partial results?", "");
				string caption = LocalizationManager.GetString("ExportDictionaryForMIDs.ExportClick.Caption", "Report: Failure", "");
				var result = Utils.MsgBox(string.Format(msg, ex.Message), caption, MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (result == DialogResult.Yes)
                {
                    DisplayOutput(projInfo);
                }
            }

            inProcess.Close();
            Environment.CurrentDirectory = curdir;
            Cursor.Current = myCursor;
            CleanUp(projInfo.DefaultXhtmlFileWithPath);
            if (Common.Testing == false)
            {
                MoveJarFile(projInfo);
                CreateRAMP(projInfo);
                Common.CleanupExportFolder(projInfo.DefaultXhtmlFileWithPath, ".txt,.xhtml,.css,.log,.jar,.jad,.properties,.xml", String.Empty, "pictures");
            }
            return success;
        }

        protected static void CleanUp(string name)
        {
            Common.CleanupExportFolder(name, ".tmp,.jar,.bat,.de", String.Empty, "Empty_Jar-Jad,dictionary");
        }

        protected void MoveJarFile(PublicationInformation projInfo)
        {
            var folder = Directory.GetDirectories(Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath), "DfM_*");
            if (folder.Length == 0) {
                return;
            }
            for (int i = 0; i < folder.Length; i++)
            {
                var file = Directory.GetFiles(folder[i]);
                foreach (var s1 in file)
                {
                    File.Move(s1, Common.PathCombine(Path.GetDirectoryName(folder[i]), Path.GetFileName(s1)));
                }
                Directory.Delete(folder[i]);
            }
        }

        private void CreateRAMP(PublicationInformation projInfo)
        {
            Ramp ramp = new Ramp();
            ramp.Create(projInfo.DefaultXhtmlFileWithPath, ".jad,.jar", projInfo.ProjectInputType);
        }

        protected void LoadCss(PublicationInformation projInfo)
        {
            var cssTree = new CssTree();
            CssClass = cssTree.CreateCssProperty(projInfo.DefaultCssFileWithPath, true);
        }

        protected void LoadParameters()
        {
            Param.LoadSettings();
            Param.SetValue(Param.InputType, "Dictionary");
            Param.LoadSettings();
        }

        protected void ReformatData(PublicationInformation projInfo)
        {
            var outFile = new DictionaryForMIDsStreamWriter(projInfo);
            outFile.Open();
            var className = projInfo.IsLexiconSectionExist ? "definition" : "headref";
            var input = Input(projInfo);
            var sensePath = projInfo.IsLexiconSectionExist ? "//*[@class = 'entry']//*[@id]" : "//*[@class = 'headref']/parent::*";
            if (input.Fw83())
            {
                sensePath = "//*[@entryguid]";
            }
            foreach (XmlNode sense in input.SelectNodes(sensePath))
            {
                if (!DictionaryForMIDsRec.HasChildClass(sense, className)) continue;
                var rec = new DictionaryForMIDsRec { CssClass = CssClass, Styles = ContentStyles };
                rec.AddHeadword(sense);
                rec.AddBeforeSense(sense);
                rec.AddSense(sense);
                rec.AddAfterSense(sense);
                rec.AddReversal(sense, className);
                outFile.WriteLine(rec.Rec);
            }
            outFile.Close();
        }

        protected void CreateProperties(PublicationInformation projInfo)
        {
            var input = Input(projInfo);
            var myProps = new DictionaryForMidsProperties(projInfo, ContentStyles);
            myProps.SetLanguage(1, input.VernacularIso(), input.VernacularName());
            myProps.SetLanguage(2, input.AnalysisIso(), input.AnalysisName());
            myProps.InfoText = GetInfo();
            myProps.Write();
            myProps.Close();
        }

        private string GetInfo()
        {
            var sb = new StringBuilder();
            const bool isConfigurationTool = false;
            sb.Append(Param.GetTitleMetadataValue("Title", Param.GetOrganization(), isConfigurationTool));
            AddMetaItem(sb, "Description");
            AddMetaItem(sb, "Copyright Holder");
            AddMetaItem(sb, "Creator");
            return sb.ToString();
        }

        private static void AddMetaItem(StringBuilder sb, string item)
        {
            var val = Param.GetMetadataValue(item);
            if (val != "")
            {
                sb.Append(" ");
                sb.Append(val);
            }
        }

        protected void CreateDictionaryForMIDs(PublicationInformation projInfo)
        {
            var output = new DictionaryForMIDsStreamWriter(projInfo);
            Debug.Assert(output.Directory != null);
            var DictionaryForMIDsPath = Common.FromRegistry("Dic4Mid");
            var creatorPath = Common.PathCombine(DictionaryForMIDsPath, "DfM-Creator");
            FolderTree.Copy(creatorPath, output.Directory);

            const string prog = "java";
            SubProcess.RedirectOutput = LogName;
			var args1 = string.Format (@"-jar DfM-Creator.jar -DictionaryGeneration .{0}main.txt . .", Path.DirectorySeparatorChar);
            SubProcess.RunCommand(output.Directory, prog, args1, true);
			var args2 = string.Format (@"-jar DfM-Creator.jar -JarCreator .{0}dictionary{0} .{0}Empty_Jar-Jad{0} .", Path.DirectorySeparatorChar);
            SubProcess.RunCommand(output.Directory, prog, args2, true);
        }

        protected void CreateSubmission(PublicationInformation projInfo)
        {
            var output = new DictionaryForMIDsStreamWriter(projInfo);
            var folder = Directory.GetDirectories(output.Directory, "DfM_*");
            if (folder.Length == 0)
                return;                 // Output was not created!
            var folderName = Path.GetFileName(folder[0]);
            var date = DateTime.Now.ToString("y.M.d");
            var folderParts = folderName.Split('_');
            var submissionName = string.Format("DictionaryForMIDs_{0}_{1}_{2}.zip", date, folderParts[1], folderParts[2]);
            var submissionFullName = Common.PathCombine(output.Directory, submissionName);
            var zip = new FastZip();
            const bool recurse = true;
            zip.CreateZip(submissionFullName, folder[0], recurse, ".*");
        }

        protected void ReportReults(PublicationInformation projInfo)
        {
            var output = new DictionaryForMIDsStreamWriter(projInfo);
            var msg = LocalizationManager.GetString("ExportDictionaryForMIDs.ReportResults.Message", "Dictionary for Mid output successfully created in {0}. Display output files?", "");
            var result = !Common.Testing ? MessageBox.Show(string.Format(msg, output.Directory), "Results", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) :
                DialogResult.No;
            if (result == DialogResult.Yes)
            {
                DisplayOutput(projInfo);
            }
        }

        private static void DisplayOutput(PublicationInformation projInfo)
        {
            var output = new DictionaryForMIDsStreamWriter(projInfo);
            const bool noWait = false;
            if (_isUnixOS)
            {
                SubProcess.Run("", "nautilus", Common.HandleSpaceinLinuxPath(output.Directory), noWait);
            }
            else
            {
                SubProcess.Run(output.Directory, "explorer.exe", output.Directory, noWait);
            }
        }

        protected DictionaryForMIDsInput Input(PublicationInformation projInfo)
        {
            if (_DictionaryForMIDsInput != null)
                return _DictionaryForMIDsInput;
            _DictionaryForMIDsInput = new DictionaryForMIDsInput(projInfo);
            return _DictionaryForMIDsInput;
        }
    }
}
