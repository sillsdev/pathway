// --------------------------------------------------------------------------------------------
// <copyright file="GoBibleTest.cs" from='2009' to='2014' company='SIL International'>
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
// GoBible Test Support
// </remarks>
// --------------------------------------------------------------------------------------------
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using NUnit.Framework;
using SIL.Tool;

namespace Test
{
    public static class GoBibleTest
    {
        /// <summary>
        /// Compares GoBible .jar files to see if the content is the same
        /// </summary>
        public static void AreEqual(string expectPath, string outputPath)
        {
            const string BibleData = "Bible Data";
            var outTmpDir = Common.PathCombine(Path.GetTempPath(),Path.GetRandomFileName());
            var expTmpDir = Common.PathCombine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(outTmpDir);
            Directory.CreateDirectory(expTmpDir);
            FastZip fastZip = new FastZip();
            fastZip.ExtractZip(outputPath, outTmpDir, string.Empty);
            fastZip.ExtractZip(expectPath, expTmpDir, string.Empty);
            DirectoryCompare(Common.PathCombine(expTmpDir, BibleData), Common.PathCombine(outTmpDir, BibleData));
            Directory.Delete(outTmpDir, true);
            Directory.Delete(expTmpDir, true);
        }

        private static void DirectoryCompare(string expDir, string outDir)
        {
            var expInfo = new DirectoryInfo(expDir);
            var outInfo = new DirectoryInfo(outDir);
            var expDirs = expInfo.GetDirectories();
            var outDirs = outInfo.GetDirectories();
            Assert.AreEqual(expDirs.Length, outDirs.Length);
            foreach (DirectoryInfo directoryInfo in expDirs)
            {
                var outFullName = Common.PathCombine(outDir, directoryInfo.Name);
                if (!Directory.Exists(outFullName))
                    Assert.Fail();
                DirectoryCompare(directoryInfo.FullName, outFullName);
            }
            var expFiles = expInfo.GetFiles();
            var outFiles = outInfo.GetFiles();
            Assert.AreEqual(expFiles.Length, outFiles.Length);
            foreach (FileInfo fileInfo in expFiles)
            {
                var outFullName = Common.PathCombine(outDir, fileInfo.Name);
                FileAssert.AreEqual(fileInfo.FullName, outFullName);
            }
        }


    }
}
