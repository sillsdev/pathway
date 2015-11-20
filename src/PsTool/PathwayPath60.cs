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
            var pathwayDir = Path.GetDirectoryName(Application.ExecutablePath);
            Common.SupportFolder = Directory.Exists(Path.Combine(pathwayDir, "PathwaySupport")) ? "PathwaySupport" : "";
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
                return (string) regObj;
            }
            return "";
        }
    }
}
