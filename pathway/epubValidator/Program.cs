// --------------------------------------------------------------------------------------
// <copyright file="Program.cs" from='2010' to='2011' company='SIL International'>
//      Copyright © 2010, 2011 SIL International. All Rights Reserved.
//
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
// <author>Erik Brommers</author>
// <email>erik_brommers@sil.org</email>
// Last reviewed:
// 
// <remarks>
// epub Validator. This program calls the open source epubcheck utility to validate
// the given epub file. epubcheck is hosted on Google Code at: 
// http://code.google.com/p/epubcheck/
// </remarks>
// --------------------------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using SIL.Tool;

namespace epubValidator
{
    public static class Program
    {
        public static String[] args;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            args = Environment.GetCommandLineArgs();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ValidationDialog());
        }

        public static string ValidateFile (string Filename)
        {
            if (Filename == null)
            {
                return "No filename specified.";
            }
            if (File.Exists(Filename))
            {
                const string prog = "java.exe";
                var progFolder = SubProcess.GetLocation(prog);
                var progFullName = Common.PathCombine(progFolder, prog);
                var sb = new StringBuilder();
                sb.Append("-jar");
                sb.Append(" \"");
                var strAppDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
                sb.Append(strAppDir.Substring(6)); // Remove the leading file:/ from the CodeBase result
                //sb.Append(".");
                sb.Append(Path.DirectorySeparatorChar);
                sb.Append("epubcheck-1.1");
                sb.Append(Path.DirectorySeparatorChar);
                sb.Append("epubcheck-1.1.jar");
                sb.Append("\" ");
                sb.Append("\"");
                sb.Append(Filename);
                sb.Append("\"");
                var procArgs = sb.ToString();
                const int timeout = 60;
                var proc = new Process
                               {
                                   StartInfo =
                                       {
                                           FileName = progFullName,
                                           Arguments = procArgs,
                                           RedirectStandardError = true,
                                           RedirectStandardOutput = true,
                                           WindowStyle = ProcessWindowStyle.Hidden,
                                           UseShellExecute = false
                                       }
                               };

                proc.Start();

                proc.WaitForExit
                    (
                        (timeout <= 0)
                        ? int.MaxValue : timeout * 100 *
                           60
                    );

                var errorMessage = proc.StandardError.ReadToEnd();
                proc.WaitForExit();

                var outputMessage = proc.StandardOutput.ReadToEnd();
                proc.WaitForExit();
                if (errorMessage.Length > 0) return errorMessage;
                if (outputMessage.Length > 0) return outputMessage;
                return null;
            }
            else
            {
                return ("Invalid filename:" + Filename);
            }
        }
    }
}
