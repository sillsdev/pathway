// --------------------------------------------------------------------------------------------
// <copyright file="FolderTree.cs" from='2009' to='2009' company='SIL International'>
//      Copyright © 2009, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Works with folder trees in file system
// </remarks>
// --------------------------------------------------------------------------------------------

using System.IO;

namespace SIL.Tool
{
    /// <summary>
    /// Works with folder trees in file system
    /// </summary>
    public static class FolderTree
    {
        /// <summary>
        /// Copy source folder tree to destination
        /// </summary>
        public static void Copy(string src, string dst)
        {
            if (!Directory.Exists(dst))
                Directory.CreateDirectory(dst);
            var di = new DirectoryInfo(src);
            foreach (var fileInfo in di.GetFiles())
            {
                string dstFullName = Common.PathCombine(dst, fileInfo.Name);
                File.Copy(fileInfo.FullName, dstFullName, true);
                File.SetAttributes(dstFullName, File.GetAttributes(fileInfo.FullName));
            }
            foreach (var directoryInfo in di.GetDirectories())
            {
                if (directoryInfo.Name.Substring(0, 1) == ".")
                    continue;
                string dstFullName = Common.PathCombine(dst, directoryInfo.Name);
                Copy(directoryInfo.FullName, dstFullName);
                Directory.SetCreationTime(dstFullName, Directory.GetCreationTime(directoryInfo.FullName));
            }
        }
    }
}
