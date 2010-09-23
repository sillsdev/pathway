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
    ///
    /// </summary>
    /// ----------------------------------------------------------------------------------------
    public class PathwayPath
    {
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the directory for the Pathway application.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        public static string GetPathwayDir()
        {
            var pathwayDir = Path.GetDirectoryName(Application.ExecutablePath);
            Common.SupportFolder = Directory.Exists(Path.Combine(pathwayDir, "PathwaySupport")) ? "PathwaySupport" : "";
            return pathwayDir;
        }
    }
}
