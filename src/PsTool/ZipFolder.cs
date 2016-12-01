// --------------------------------------------------------------------------------------------
// <copyright file="ZipFolder.cs" from='2009' to='2014' company='SIL International'>
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
// Implementation for zip file folder
// </remarks>
// --------------------------------------------------------------------------------------------

#region Using
using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using SIL.Tool.Localization;

#endregion

namespace SIL.Tool
{
    /// <summary>
    /// Implementation for zip file folder
    /// </summary>
    public class ZipFolder
    {
        #region AddToZip(string[] filename, string archive)
        /// <summary>
        /// Add files to an existing Zip archive, 
        /// </summary>
        /// <param name="filename">Array of path / filenames to add to the archive</param>
        /// <param name="archive">Zip archive that we want to add the file to</param>
        public void AddToZip(string[] filename, string archive)
        {
            if (!File.Exists(archive))
            {
                return;
            }

            try
            {
                ZipFile zf = new ZipFile(archive);
                zf.BeginUpdate();
                // path relative to the archive
                zf.NameTransform = new ZipNameTransform(Path.GetDirectoryName(archive));
                foreach (var file in filename)
                {
                    // skip if this isn't a real file
                    if (!File.Exists(file))
                    {
                        continue;
                    }
                    zf.Add(file, CompressionMethod.Deflated);
                }
                zf.CommitUpdate();
                zf.Close();
            }
            catch (Exception e)
            {
                if (e.Message != null)
                {
                    var msg = new[] { e.Message };
                    LocDB.Message("defErrMsg", e.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                }
            }
        }
        #endregion

        #region CreateZip(string sourceFolder, string outputFileWithPath, int errCount)
        /// <summary>
        /// Compress Generated file
        /// </summary>
        /// <param name="sourceFolder">Folder Name</param>
        /// <param name="outputFileWithPath">Output FileName</param>
        /// <param name="errCount">Display Error Count in messagebox </param>
        public void CreateZip(string sourceFolder, string outputFileWithPath, int errCount)
        {
            try
            {
                if(!Directory.Exists(sourceFolder) || outputFileWithPath.Length == 0)
                {
                    return;
                }
                
                var zf = new FastZip();
                zf.CreateZip(outputFileWithPath, sourceFolder, true, string.Empty);
                if(Common.ShowMessage && !Common.Testing)
                {
                    string strMessage = "File has been exported successfully";
                    var msg = new[] { strMessage + ". "};
                    if(errCount > 0)
                    {
                        msg = new[] {strMessage + " with " + errCount + "error(s)."};
						Console.WriteLine(msg.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                if (e.Message != null)
                {
                    Console.WriteLine(new[] {e.Message});
                }
            }
        }
        #endregion
    }
}