// --------------------------------------------------------------------------------------------
// <copyright file="ZeroCheck.cs" from='2009' to='2014' company='SIL International'>
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
// 
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Runtime.Serialization;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Collections.Generic;

namespace BuildTasks
{
    public class ZeroCheck : Task
    {
        public override bool Execute()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "Files");
            DoZeroCheck(path);
            return true;
        }

        #region DoZeroCheck
        /// <summary>
        /// Throws ZeroLengthFileException if the folder tree contains a zero length file.
        /// </summary>
        private void DoZeroCheck(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                if (fileInfo.Length == 0)
                {
                    Log.LogMessage(MessageImportance.High, fileInfo.FullName + " is a zero length file.");
                    throw new ZeroLengthFileException(fileInfo.FullName);
                }
            }
            foreach (DirectoryInfo subDirectoryInfo in directoryInfo.GetDirectories())
            {
                DoZeroCheck(subDirectoryInfo.FullName);
            }
        }
        #endregion DoZeroCheck

    }

    #region ZeroLengthFileException
    [Serializable]
    public class ZeroLengthFileException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ZeroLengthFileException()
        {
        }

        public ZeroLengthFileException(string message)
            : base(message)
        {
        }

        public ZeroLengthFileException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected ZeroLengthFileException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
    #endregion ZeroLengthFileException
}
