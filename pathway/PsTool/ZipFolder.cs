// --------------------------------------------------------------------------------------------
// <copyright file="ZipFolder.cs" from='2009' to='2009' company='SIL International'>
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
// Implementation for zip file folder
// </remarks>
// --------------------------------------------------------------------------------------------

#region Using
using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using SIL.Tool;
using SIL.Tool.Localization;

#endregion

namespace SIL.Tool
{
    /// <summary>
    /// Implementation for zip file folder
    /// </summary>
    public class ZipFolder
    {
        #region AddToZip(string filename, string archive, Boolean addCompresed)
        /// <summary>
        /// Add a single file to an existing Zip archive, with the option of adding the file
        /// as deflated or stored (no compression). As of 10/19/2010, these are the only compression 
        /// options supported by the ICSharpCode library.
        /// Note: Files are currently added to the root of the archive.
        /// </summary>
        /// <param name="filename">Path and filename of the file to add to the archive</param>
        /// <param name="archive">Zip archive that we want to add the file to</param>
        /// <param name="addCompresed">true if compressed, false if stored</param>
        public void AddToZip(string filename, string archive, Boolean addCompresed)
        {
            if (!File.Exists(filename) || !File.Exists(archive))
            {
                return;
            }

            try
            {

                ZipFile zf = new ZipFile(archive);
                zf.BeginUpdate();
                // NameTransform nukes the path info from Filename (so it gets added to the top level of the archive)
                zf.NameTransform = new ZipNameTransform(Path.GetDirectoryName(filename));
                zf.Add(filename, (addCompresed) ? CompressionMethod.Deflated : CompressionMethod.Stored);
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
                    }
                    LocDB.Message("defErrMsg", "File has been exported successfully.", msg, LocDB.MessageTypes.Info, LocDB.MessageDefault.First);
                }
            }
            catch (Exception e)
            {
                if (e.Message != null)
                {
                    var msg = new[] {e.Message};
                    LocDB.Message("defErrMsg", e.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                }
            }
        }
        #endregion
    }
}