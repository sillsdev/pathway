#region // Copyright (C) 2014, SIL International. All Rights Reserved.
// --------------------------------------------------------------------------------------------
// <copyright file="Program.cs" from='2009' to='2014' company='SIL International'>
//      Copyright (C) 2014, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>Runs ans XSLT table on an input file</remarks>
// --------------------------------------------------------------------------------------------
#endregion
using System;
using System.Diagnostics;
using SIL.PublishingSolution;


namespace PSXslProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 4)
                {
                    throw new ArgumentException("XslProcess: Wrong number of arguments");
                }
                XsltProcess xsltProcess = new XsltProcess();
                xsltProcess.XsltTransform(args[0], args[1], args[2], args[3]);
            }
            catch (Exception e)
            {
                Debug.Print(e.Message);
                Environment.ExitCode = -1;
            }
        }
    }
}
