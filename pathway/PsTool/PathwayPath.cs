// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2010, SIL International. All Rights Reserved.
// <copyright from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: PathwayPath.cs
// Responsibility: Trihus
// ---------------------------------------------------------------------------------------------
using System.IO;
using System.Windows.Forms;

namespace SIL.Tool
{
    /// ----------------------------------------------------------------------------------------
    /// <summary>
    /// Get the Path names that were created by the installer in the registry
    /// </summary>
    /// ----------------------------------------------------------------------------------------
    public class PathwayPath
    {
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the directory for the Pathway application.
        /// </summary>
        /// <returns>
        /// name or the directory or string.Empty if the directory name isn't in the registry.
        /// </returns>
        /// ------------------------------------------------------------------------------------
        public static string GetPathwayDir()
        {
            string pathwayDir = string.Empty;
            object regObj;
            try
            {
                if (RegistryHelperLite.RegEntryExists(RegistryHelperLite.CompanyKeyCurrentUser,
                    "Pathway", "PathwayDir", out regObj))
                {
                    Common.SupportFolder = "";
                    return (string)regObj;
                }
                if (RegistryHelperLite.RegEntryExists(RegistryHelperLite.CompanyKeyLocalMachine,
                    "Pathway", "PathwayDir", out regObj))
                {
                    Common.SupportFolder = "";
                    return (string)regObj;
                }
                var fwKey = RegistryHelperLite.CompanyKeyLocalMachine.OpenSubKey("FieldWorks");
                if (fwKey != null && Common.fromPlugin)
                {
                    if (!RegistryHelperLite.RegEntryExists(fwKey, "8.0", "RootCodeDir", out regObj))
                        if (!RegistryHelperLite.RegEntryExists(fwKey, "7.0", "RootCodeDir", out regObj))
                            if (!RegistryHelperLite.RegEntryExists(fwKey, "", "RootCodeDir", out regObj))
                                regObj = string.Empty;
                    pathwayDir = (string)regObj;
                    // The next line helps those using the developer version of FieldWorks
                    pathwayDir = pathwayDir.Replace("DistFiles", @"Output\Debug").Replace("distfiles", @"Output\Debug");
                }
                if (!File.Exists(Path.Combine(pathwayDir, "PsExport.dll")))
                    pathwayDir = string.Empty;
                if (pathwayDir == string.Empty)
                {
                    pathwayDir = Path.GetDirectoryName(Application.ExecutablePath);
                }
                // If after all this fall back code, we can't find PsExport in the resulting folder, we're in trouble
                //if (!File.Exists(Path.Combine(pathwayDir, "PsExport.dll")) && !Common.Testing)
                //    Debug.Fail("Unable to find Pathway directory in registry.");
                // If the Support folder exists, it should be used.
                Common.SupportFolder = Directory.Exists(Path.Combine(pathwayDir, "PathwaySupport")) ? "PathwaySupport" : "";
            }
            catch { }
            return pathwayDir;
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the directory for the ConTeXt software for the XeTeX back end.
        /// </summary>
        /// 
        /// <returns>
        /// name or the directory or string.Empty if the directory name isn't in the registry.
        /// </returns>
        /// ------------------------------------------------------------------------------------
        public static string GetCtxDir()
        {
            object regObj;
            if (RegistryHelperLite.RegEntryExists(RegistryHelperLite.CompanyKeyLocalMachine,
                                                  "PwCtx", "ConTeXtDir", out regObj))
            {
                return (string)regObj;
            }
            return "";
        }
    }
}
