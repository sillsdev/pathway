// --------------------------------------------------------------------------------------------
// <copyright file="XeLaTexInstallation.cs" from='2009' to='2014' company='SIL International'>
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
            if (osName.Contains("Windows"))
            {
                pathwayXeLaTeXPath =
                   Common.GetValueFromRegistry("SOFTWARE\\Wow6432Node\\SIL\\PathwayXeLaTeX",
                                               "XeLaTexDir");
                if (string.IsNullOrEmpty(pathwayXeLaTeXPath))
                { // Handle 32-bit Windows 7 and XP
                    pathwayXeLaTeXPath = Common.GetValueFromRegistry("SOFTWARE\\SIL\\PathwayXeLaTeX", "XeLaTexDir");
                }
            }
            else if (Common.IsUnixOS())
            {
                pathwayXeLaTeXPath = Common.GetValueFromRegistryFromCurrentUser("SOFTWARE\\SIL\\PathwayXeLaTeX", "XeLaTexDir");
				if (string.IsNullOrEmpty(pathwayXeLaTeXPath))
				{
		            if (Common.ReadingCommandPromptOutputValue("uname", "-m").IndexOf("i686") >= 0)
		               pathwayXeLaTeXPath = "/usr/lib/pwtex/bin/i386-linux";
		            else if (Common.ReadingCommandPromptOutputValue("uname", "-m").IndexOf("x86_64") >= 0)
		                pathwayXeLaTeXPath = "/usr/lib/pwtex/bin/x86_64-linux";
				}
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
				return (string)regObj == "1.13.3";
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
