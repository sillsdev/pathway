// --------------------------------------------------------------------------------------------
// <copyright file="PathPart.cs" from='2009' to='2014' company='SIL International'>
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
// ODT Test Support
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using SIL.Tool;

namespace Test
{
    public static class PathPart
    {
        public static string Bin(string currentDir, string addedPath)
        {
            Debug.Print("CurrentDir={0}", currentDir);
            var binFolderPart = currentDir.IndexOf(Path.DirectorySeparatorChar + "output" + Path.DirectorySeparatorChar, StringComparison.Ordinal);
            return Common.DirectoryPathReplace(currentDir.Substring(0, binFolderPart) + "/src/Test" + addedPath);
        }
    }
}
