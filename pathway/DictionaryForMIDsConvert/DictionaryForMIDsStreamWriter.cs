// --------------------------------------------------------------------------------------------
// <copyright file="DictionaryForMIDsStreamWriter.cs" from='2013' to='2013' company='SIL International'>
//      Copyright © 2013, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Stylepick FeatureSheet
// </remarks>
// --------------------------------------------------------------------------------------------

using System.Diagnostics;
using System.IO;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class DictionaryForMIDsStreamWriter
    {
        protected StreamWriter StreamWriter { get; set; }
        public string Directory;
        public string FullPath;

        public DictionaryForMIDsStreamWriter(PublicationInformation projInfo)
        {
            var name = Path.GetFileNameWithoutExtension(projInfo.DefaultXhtmlFileWithPath);
            Directory = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath);
            Debug.Assert(Directory != null);
            FullPath = Common.PathCombine(Directory, name + ".txt");
        }

        public void Open()
        {
            StreamWriter = new StreamWriter(FullPath);
        }

        public void WriteLine(string value)
        {
            StreamWriter.WriteLine(value);
        }

        public void Close()
        {
            StreamWriter.Close();
        }
    }
}
