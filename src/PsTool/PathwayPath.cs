// --------------------------------------------------------------------------------------------
// <copyright file="PathwayPath.cs" from='2009' to='2014' company='SIL International'>
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
				if (Path.PathSeparator == '/' || Path.PathSeparator == ':') //Test for Linux (see: http://www.mono-project.com/FAQ:_Technical)
	            {
		            const string myPathwayDir = "/usr/lib/pathway";
		            RegistryAccess.SetStringRegistryValue("PathwayDir", myPathwayDir);
		            Common.SupportFolder = "";
		            return myPathwayDir;
	            }
	            if (GetPathwayInstallerDirectoryForCurrentUserRegistry(out pathwayDir))
                {
                    return pathwayDir;
                }

                if (GetPathwayInstallerDirectoryForLocalMachineRegistry(out pathwayDir))
                {
                    return pathwayDir;
                }
                
                string pathwayInstalledLocation = "C:\\Program Files (x86)\\SIL\\Pathway7\\";
                if (Directory.Exists(pathwayInstalledLocation))
                {
                    Common.SupportFolder = "";
                    return pathwayInstalledLocation;
                }

                pathwayInstalledLocation = "C:\\Program Files\\SIL\\Pathway7\\";
                if (Directory.Exists(pathwayInstalledLocation))
                {
                    Common.SupportFolder = "";
                    return pathwayInstalledLocation;
                }

                var fwKey = RegistryHelperLite.CompanyKeyLocalMachine.OpenSubKey("FieldWorks");
                if (fwKey != null)
                {
                    if (!RegistryHelperLite.RegEntryExists(fwKey, "8.0", "RootCodeDir", out regObj))
                        if (!RegistryHelperLite.RegEntryExists(fwKey, "7.0", "RootCodeDir", out regObj))
                            if (!RegistryHelperLite.RegEntryExists(fwKey, "", "RootCodeDir", out regObj))
                                regObj = string.Empty;
                    pathwayDir = (string)regObj;
                    // The next line helps those using the developer version of FieldWorks
                    pathwayDir = pathwayDir.Replace("DistFiles", @"Output\Debug").Replace("distfiles", @"Output\Debug");
                }
                if (!File.Exists(Common.PathCombine(pathwayDir, "PsExport.dll")))
                    pathwayDir = string.Empty;
                if (pathwayDir == string.Empty)
                {
                    pathwayDir = Path.GetDirectoryName(Application.ExecutablePath);
                }
                // If after all this fall back code, we can't find PsExport in the resulting folder, we're in trouble
                //if (!File.Exists(Common.PathCombine(pathwayDir, "PsExport.dll")) && !Common.Testing)
                //    Debug.Fail("Unable to find Pathway directory in registry.");
                // If the Support folder exists, it should be used.
                Common.SupportFolder = Directory.Exists(Common.PathCombine(Path.GetDirectoryName(pathwayDir), "PathwaySupport")) ? "PathwaySupport" : "";
            }
            catch { }
            return pathwayDir;
        }

        private static bool GetPathwayInstallerDirectoryForCurrentUserRegistry(out string pathwayDir)
        {
            bool isAvailable = false;
            pathwayDir = null;
            try
            {
                object regObj;
                if (RegistryHelperLite.RegEntryExists(RegistryHelperLite.CompanyKeyCurrentUser,
                                                      "Pathway", "PathwayDir", out regObj))
                {
                    Common.SupportFolder = "";
                    pathwayDir = (string)regObj;
                    isAvailable = true;
                }
            }
            catch
            {
            }
            return isAvailable;
        }

        private static bool GetPathwayInstallerDirectoryForLocalMachineRegistry(out string pathwayDir)
        {
            bool isAvailable = false;
            pathwayDir = null;
            try
            {
                object regObj;
                if (RegistryHelperLite.RegEntryExists(RegistryHelperLite.CompanyKeyLocalMachine,
                                                      "Pathway", "PathwayDir", out regObj))
                {
                    Common.SupportFolder = "";
                    pathwayDir = (string)regObj;
                    isAvailable = true;
                }
            }
            catch
            {
            }
            return isAvailable;
        }

		public static string GetSupportPath(string theAppPath, string supportFileOrFolder, bool isFile)
		{
			string thePath = Common.PathCombine(theAppPath, supportFileOrFolder);
			if (isFile)
			{
				if (!File.Exists(thePath))
				{
					thePath = Common.PathCombine(Path.GetDirectoryName(theAppPath), supportFileOrFolder);
				}
			}
			else
			{
				if (!Directory.Exists(thePath))
				{
					thePath = Common.PathCombine(Path.GetDirectoryName(theAppPath), supportFileOrFolder);
				}
			}
			return thePath;
		}
    }
}
