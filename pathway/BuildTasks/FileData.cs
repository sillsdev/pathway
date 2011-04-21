// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2011, SIL International. All Rights Reserved.
// <copyright from='2011' to='2011' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: FileData.cs
// Responsibility: Trihus
// ---------------------------------------------------------------------------------------------
using System.IO;

namespace BuildTasks
{
    /// <summary>
    /// Simple access to file contents
    /// </summary>
    public static class FileData
    {
        /// <summary>
        /// Returns contents of filePath
        /// </summary>
        public static string Get(string filePath)
        {
            string data = string.Empty;
            if (!File.Exists(filePath)) return data;
            var istr = new StreamReader(filePath);
            data = istr.ReadToEnd();
            istr.Close();
            return data;
        }

        /// <summary>
        /// Moves the filepath to the Wix sub folder
        /// </summary>
        public static void MoveToWix(string filePath)
        {
            var fileDir = Path.GetDirectoryName(filePath);
            var wixPath = Path.Combine(fileDir, "Wix");
            if (!Directory.Exists(wixPath))
                Directory.CreateDirectory(wixPath);
            File.Move(filePath, Path.Combine(wixPath, Path.GetFileName(filePath)));
        }
    }
}
