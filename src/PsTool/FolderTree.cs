// --------------------------------------------------------------------------------------------
// <copyright file="FolderTree.cs" from='2009' to='2014' company='SIL International'>
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
// Works with folder trees in file system
// </remarks>
// --------------------------------------------------------------------------------------------

using System.IO;
using System.Runtime.InteropServices;

namespace SIL.Tool
{
    /// <summary>
    /// Works with folder trees in file system
    /// </summary>
    public static class FolderTree
    {
	// Define GetShortPathName API function.
	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	static extern uint GetShortPathName(string lpszLongPath, char[] lpszShortPath, int cchBuffer);


	/// <summary>
	/// Copy source folder tree to destination
	/// </summary>
	public static void Copy(string src, string dst)
	{
		Copy(src, dst, true);
	}

        /// <summary>
        /// Copy source folder tree to destination
        /// </summary>
	public static void Copy(string src, string dst, bool copyAttr)
        {
            if (!Directory.Exists(dst))
                Directory.CreateDirectory(dst);

            var di = new DirectoryInfo(src);
            foreach (FileInfo fileInfo in di.GetFiles())
            {
                string dstFullName = Common.PathCombine(dst, fileInfo.Name);
                FileInfo dstInfo = new FileInfo(dstFullName);
                if (dstInfo.Exists && dstInfo.IsReadOnly)
                    dstInfo.Attributes = dstInfo.Attributes & ~FileAttributes.ReadOnly;
                File.Copy(fileInfo.FullName, dstFullName, true);
		if (copyAttr)
		{
			File.SetAttributes (dstFullName, File.GetAttributes (fileInfo.FullName));
		}
            }
            foreach (var directoryInfo in di.GetDirectories())
            {
                if (directoryInfo.Name.Substring(0, 1) == ".")
                    continue;
                string dstFullName = Common.PathCombine(dst, directoryInfo.Name);
                Copy(directoryInfo.FullName, dstFullName, copyAttr);
                Directory.SetCreationTime(dstFullName, Directory.GetCreationTime(directoryInfo.FullName));
            }
        }

		// Return the short file name for a long file name.
		public static string ShortFileName(string long_name)
		{
			char[] name_chars = new char[2048];
			long length = GetShortPathName(
				long_name, name_chars,
				name_chars.Length);

			string short_name = new string(name_chars);
			return short_name.Substring(0, (int)length);
		}

		// Return the long file name for a short file name.
		public static string LongFileName(string short_name)
		{
			return new FileInfo(short_name).FullName;
		}
    }
}
