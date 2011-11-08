using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
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
        public HostProgram Host { get { return _hostProgram; } }
        public enum HostProgram
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
            if (executablePath.Contains("FieldWorks 7") || executablePath.Contains("FieldWorks"))
            {
                _hostProgram = HostProgram.FieldWorks;
            }
            else if (executablePath.Contains("Paratext 7"))
            {
                _hostProgram = HostProgram.Paratext;
            }
            else if (executablePath.Contains("PathwayB"))
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
                string ssfFile = database + ".ssf";
                const string ptProjectDir = "My Paratext Projects";
                var sb = new StringBuilder();
                // quick test - check the C drive
                if (File.Exists(Path.Combine("c:\\My Paratext Projects", ssfFile)))
                {
                    return Path.Combine("c:\\My Paratext Projects", ssfFile);
                }
                // not on C drive - iterate through logical drives until we find it           
                foreach (var logicalDrive in Directory.GetLogicalDrives())
                {
                    sb.Length = 0; // clean out the stringbuilder
                    sb.Append(Path.Combine(logicalDrive, ptProjectDir));
                    if (File.Exists(Path.Combine(sb.ToString(), ssfFile)))
                    {
                        return (Path.Combine(sb.ToString(), ssfFile));
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
            }
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
                                Value.Add(nodeName, nodeValue);
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
