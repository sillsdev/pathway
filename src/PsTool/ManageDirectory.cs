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
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace SIL.Tool
{
	/// <summary>
	/// Works with Directory FileNames in file system
	/// </summary>
	public static class ManageDirectory
    {
#if !__MonoCS__
#define UNITY_STANDALONE_WIN
#endif
#if UNITY_STANDALONE_WIN
        // Define GetShortPathName API function.
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern uint GetShortPathName(string lpszLongPath, char[] lpszShortPath, int cchBuffer);
#else
		private static long GetShortPathName(string lpszLongPath, ref char[] lpszShortPath, int cchBuffer)
		{
            var result = string.Empty;
            foreach (var part in lpszLongPath.Split(Path.DirectorySeparatorChar)){
                var str = string.Empty;
				foreach (Match match in Regex.Matches(part, "[a-zA-Z0-9]+"))
				{
					str += match.Value;
				}
				var len = str.Length;
                result += str.Substring(0, len < 6 ? len : 6) + Path.DirectorySeparatorChar.ToString();
			}
            lpszShortPath = result.ToCharArray();
            return lpszShortPath.Length;
		}
#endif

		// Return the short file name for a long file name.
		public static string ShortFileName(string long_name)
		{
			char[] name_chars = new char[2048];
			long length = GetShortPathName(
				long_name, ref name_chars,
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
