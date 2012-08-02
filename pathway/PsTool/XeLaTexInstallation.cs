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
// File: XeLaTexInstallation.cs
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
    public class XeLaTexInstallation
    {
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the directory for the XeLaTeX software for the XeTeX back end.
        /// </summary>
        /// 
        /// <returns>
        /// name or the directory or string.Empty if the directory name isn't in the registry.
        /// </returns>
        /// ------------------------------------------------------------------------------------
        public static string GetXeLaTexDir()
        {
            string pathwayXeLaTeXPath = string.Empty;
            string osName = Common.GetOsName();
            if (osName == "Windows7")
            {
                pathwayXeLaTeXPath =
                   Common.GetValueFromRegistry("SOFTWARE\\Wow6432Node\\SIL\\PathwayXeLaTeX",
                                               "XeLaTexDir");
            }
            else if (osName == "Windows XP")
            {
                pathwayXeLaTeXPath = Common.GetValueFromRegistry("SOFTWARE\\SIL\\PathwayXeLaTeX", "XeLaTexDir");
            }
            else if (Common.IsUnixOS())
            {
                if (Common.ReadingCommandPromptOutputValue("uname", "-m").IndexOf("i686") >= 0)
                   pathwayXeLaTeXPath = "/usr/lib/pwtex/bin/i386-bin";
                else if (Common.ReadingCommandPromptOutputValue("uname", "-m").IndexOf("x86_64") >= 0)
                    pathwayXeLaTeXPath = "/usr/lib/pwtex/bin/x86_64-linux";
            }
            return pathwayXeLaTeXPath;
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Check the XeLaTeX version.
        /// </summary>
        /// 
        /// <returns>
        /// True if the installed version of XeLaTeX matches the expected version of Pathway.
        /// </returns>
        /// ------------------------------------------------------------------------------------
        public static bool CheckXeLaTexVersion()
        {
            object regObj;
            if (RegistryHelperLite.RegEntryExists(RegistryHelperLite.CompanyKeyLocalMachine,
                                                  "PathwayXeLaTeX", "XeLaTexVer", out regObj))
            {
                return (string)regObj == "1.2";
            }
            return false;
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the directory for the XeLaTeX software for the XeTeX back end.
        /// </summary>
        /// 
        /// <returns>
        /// name or the directory or string.Empty if the directory name isn't in the registry.
        /// </returns>
        /// ------------------------------------------------------------------------------------
        public static int GetXeLaTexFontCount()
        {
            return RegistryAccess.GetIntRegistryValue("XeLaTexFontCount", 0);
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the directory for the XeLaTeX software for the XeTeX back end.
        /// </summary>
        /// 
        /// <returns>
        /// name or the directory or string.Empty if the directory name isn't in the registry.
        /// </returns>
        /// ------------------------------------------------------------------------------------
        public static void SetXeLaTexFontCount(int count)
        {
            RegistryAccess.SetIntRegistryValue("XeLaTexFontCount", count);
        }

    }
}
