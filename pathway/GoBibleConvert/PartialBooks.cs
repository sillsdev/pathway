// --------------------------------------------------------------------------------------
// <copyright file="PartialBooks.cs" from='2014' to='2014' company='SIL International'>
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
// </remarks>
// --------------------------------------------------------------------------------------
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SIL.PublishingSolution
{
    public static class PartialBooks
    {
        private static readonly Regex ChPat = new Regex(@"\\c ([0-9]+)\s*");
        private static readonly Regex VrsPat = new Regex(@"\\v ([0-9]+)");
        private const string EmptyCh = "\\c {0}\n{1}";
        private const string EmptyContent = "\\v 1\n";
        private const string EmptyVrs = "\\v {0}\n";
        private static readonly UTF8Encoding Utf8NoBom = new UTF8Encoding(false);
        private const bool DoNotAppend = false;

        public static void AddChapters(string dirName)
        {
            var dirInfo = new DirectoryInfo(dirName);
            foreach (FileInfo fileInfo in dirInfo.GetFiles())
            {
                var input = new StreamReader(fileInfo.FullName);
                var output = new StringBuilder();
                var data = input.ReadToEnd();
                input.Close();
                var p = 0;
                var nextCh = 1;
                foreach (Match match in ChPat.Matches(data))
                {
                    output.Append(data.Substring(p, match.Index - p));
                    p = match.Index;
                    var c = int.Parse(match.Groups[1].Value);
                    while (nextCh < c)
                    {
                        output.Append(string.Format(EmptyCh, nextCh, EmptyContent));
                        nextCh += 1;
                    }
                    output.Append(match.Value);
                    p += match.Value.Length;
                    nextCh = c + 1;
                    if (data.Substring(p, 3) == "\\c ")
                    {
                        output.Append(EmptyContent);
                    }
                    else
                    {
                        AddEmptyVerses(data, p, output);
                    }
                }
                output.Append(data.Substring(p, data.Length - p));
                var writer = new StreamWriter(fileInfo.FullName, DoNotAppend, Utf8NoBom);
                writer.Write(output.ToString());
                writer.Close();
            }
        }

        private static void AddEmptyVerses(string data, int p, StringBuilder output)
        {
            var vrsMatch = VrsPat.Match(data.Substring(p));
            if (vrsMatch.Success)
            {
                var v = int.Parse(vrsMatch.Groups[1].Value);
                var nextVrs = 1;
                while (nextVrs < v)
                {
                    output.Append(string.Format(EmptyVrs, nextVrs));
                    nextVrs += 1;
                }
            }
        }
    }
}
