// --------------------------------------------------------------------------------------------
// <copyright file="SettingsHelper.cs" from='2009' to='2014' company='SIL International'>
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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace SIL.Tool
{
    /// <summary>
    /// Encapsulates the settings held in the host program's settings file.
    /// Note: this currently only pulls data out of Paratext .ssf files.
    /// </summary>
    public class SettingsHelper
    {
        public Dictionary<string, string> Value = new Dictionary<string, string>();
        public string Database { get; set; }
        private bool _isLoaded;
        private HostProgram _hostProgram;
        private enum HostProgram
        {
            Paratext,
            FieldWorks,
            PathwayB,
            Other
        }

        public SettingsHelper(string database)
        {
            Database = database;
            _isLoaded = false;
            // figure out who's calling us
            string executablePath = Path.GetDirectoryName(Application.ExecutablePath);
            if (executablePath == null)
            {
                return;
            }
            if (executablePath.ToLower().Contains("fieldworks"))
            {
                _hostProgram = HostProgram.FieldWorks;
            }
            else if (executablePath.ToLower().Contains("paratext"))
            {
                _hostProgram = HostProgram.Paratext;
            }
			else if (executablePath.ToLower().Contains("pathwayb"))
            {
                _hostProgram = HostProgram.PathwayB;
            }
            else
            {
                // This could be the configuration tool, nunit test, etc. - 
                // whatever it is, it doesn't have a settings file we need to look at.
                _hostProgram = HostProgram.Other;
            }
        }

        /// <summary>
        /// Override - returns 
        /// </summary>
        /// <returns>the path and filename to the currently selected language descrptions settings</returns>
        public string GetLanguageFilename()
        {
            var ssf = GetSettingsFilename(Database);
            var languageFileName = Common.GetXmlNode(ssf, "//Language").InnerText + ".lds";
            var folder = Path.GetDirectoryName(ssf);
            return Common.PathCombine(folder, languageFileName);
        }

        /// <summary>
        /// Override - returns the path and filename to the currently selected database name
        /// </summary>
        /// <returns></returns>
        public string GetSettingsFilename()
        {
            return GetSettingsFilename(Database);
        }

        /// <summary>
        /// Returns the path and filename to the .ssf file for the given database name
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public string GetSettingsFilename(string database)
        {
            // sanity check
            if (database.Trim().Length < 1)
            {
                return string.Empty;
            }

            // Paratext (or PathwayB)
            if (_hostProgram == HostProgram.Paratext || _hostProgram == HostProgram.PathwayB)
            {
                // (Note that PathwayB _might_ have a project file we can poke at - no guarantee)
                object paraTextprojectPath;
                string ssfFile = database + ".ssf";
                if (Common.UnixVersionCheck())
                {
                    var windowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
                    if (windowsIdentity != null)
                    {
                        string userName = windowsIdentity.Name;
                        string registryPath = "/home/" + userName + "/.config/paratext/registry/LocalMachine/software/scrchecks/1.0/settings_directory/";
                        while (Directory.Exists(registryPath))
                        {
                            if (File.Exists(Common.PathCombine(registryPath, "values.xml")))
                            {
                                XmlDocument doc = new XmlDocument();
                                doc.Load(Common.PathCombine(registryPath, "values.xml"));
                                paraTextprojectPath = doc.InnerText;
                                Environment.SetEnvironmentVariable("ParatextProjPath", paraTextprojectPath.ToString());
                                return Common.PathCombine(paraTextprojectPath.ToString(), ssfFile);
                            }
                            //string ParatextProjectPath = Environment.GetEnvironmentVariable("ParatextProjPath");
                        }
                    }
                }
                else
                {
                    if (RegistryHelperLite.RegEntryExists(RegistryHelperLite.ParatextKey,
                                                          "Settings_Directory", "", out paraTextprojectPath))
                    {
                        string settingFilePath = Common.PathCombine((string) paraTextprojectPath, ssfFile);
                        if (File.Exists(settingFilePath))
                            return settingFilePath;
                        else
                            return string.Empty;
                    }
                }
                // not found on logical drives. 
                Debug.WriteLine(ssfFile + " does not exist.");
                return string.Empty;
            }
            if (_hostProgram == HostProgram.FieldWorks)
            {

				// For Fieldworks, the <databasename>.fwdata file contains the list of writing systems
				// in use, in a format like this:
				//<AnalysisWss>                 << not in use
				//<Uni>en pt</Uni>
				//</AnalysisWss>
				//<CurVernWss>                  << in use
				//<Uni>seh</Uni>
				//</CurVernWss>
				//<CurAnalysisWss>              << in use
				//<Uni>pt en</Uni>
				//</CurAnalysisWss>
				//<CurPronunWss>                << in use (but I don't think this is exported?)
				//<Uni>seh-fonipa-x-etic</Uni>
				//</CurPronunWss>
				//<VernWss>                     << not in use
				//<Uni>seh seh-fonipa-x-etic</Uni>
				//</VernWss>

				object fwprojectPath;
	            string fwdataFile = database + "/" + database + ".fwdata";
				if (Common.UnixVersionCheck())
				{
					var windowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
					if (windowsIdentity != null)
					{
						string userName = windowsIdentity.Name;
						string registryPath = "/home/" + userName + "/.config/fieldworks/registry/LocalMachine/software/sil/fieldworks/";
						if (Directory.Exists(Common.PathCombine(registryPath, "8")))
							registryPath = Common.PathCombine(registryPath, "8");
						else
						{
							if (Directory.Exists(Common.PathCombine(registryPath, "7")))
								registryPath = Common.PathCombine(registryPath, "7");
						}
						while (Directory.Exists(registryPath))
						{
							if (File.Exists(Common.PathCombine(registryPath, "values.xml")))
							{
								XmlDocument doc = new XmlDocument();
								doc.Load(Common.PathCombine(registryPath, "values.xml"));
								fwprojectPath = doc.SelectSingleNode("/values/value[@name=='ProjectsDir'");
								if (fwprojectPath == null) return string.Empty;
								Environment.SetEnvironmentVariable("FieldworksProjPath", fwprojectPath.ToString());
								return Common.PathCombine(fwprojectPath.ToString(), fwdataFile);
							}
						}
					}
				}
				else
				{
					fwprojectPath = SilTools.Utils.FwProjectsPath;
					if (Directory.Exists((string)fwprojectPath))
					{
						string settingFilePath = Common.PathCombine((string)fwprojectPath, fwdataFile);
						if (File.Exists(settingFilePath))
							return settingFilePath;
						else
							return string.Empty;
					}
				}

				Debug.WriteLine(fwdataFile + " does not exist.");
				return string.Empty;
            }
			// not found on logical drives. 
            if (_hostProgram == HostProgram.Other)
            {
                
            }
            return string.Empty;
        }

        /// <summary>
        /// Clears out the Value dictionary
        /// </summary>
        public void ClearValues()
        {
            Value.Clear();
        }

        /// <summary>
        /// Loads the Value dictionary from the .ldml or .ssf file
        /// </summary>
        public void LoadValues()
        {
            if (_isLoaded)
            {
                ClearValues();
                _isLoaded = false;
            }
            // if there is no settings file available, exit out
            var settingsFile = GetSettingsFilename();
            if (!File.Exists(settingsFile) || _hostProgram == HostProgram.Other)
            {
                return;
            }
            // open up the settings file and populate our dictionary
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(settingsFile)
                             {
                                 WhitespaceHandling = WhitespaceHandling.None
                             };
                string nodeName = null, nodeValue = null;

                // Parse the file and fill our internal Dictionary.
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            nodeName = reader.Name;
                            break;
                        case XmlNodeType.Text:
                        case XmlNodeType.CDATA:
                            nodeValue = reader.Value;
                            break;
                        case XmlNodeType.ProcessingInstruction:
                        case XmlNodeType.Comment:
                        case XmlNodeType.XmlDeclaration:
                        case XmlNodeType.Document:
                        case XmlNodeType.DocumentType:
                        case XmlNodeType.EntityReference:
                            break;
                        case XmlNodeType.EndElement:
                            // reached the end of an element -
                            // see if we collected a new entry for our dictionary
                            if (nodeName != null)
                            {
                                // yes - add it now
	                            if (!Value.ContainsKey(nodeName))
	                            {
		                            Value.Add(nodeName, nodeValue);
	                            }
								nodeName = null;
								nodeValue = null;
                            }
                            break;
                    }
                }
                _isLoaded = true;
            }

            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
    }
}
