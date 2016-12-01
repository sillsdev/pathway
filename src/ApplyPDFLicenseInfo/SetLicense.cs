// --------------------------------------------------------------------------------------------
// <copyright file="SetLicense.cs" from='2009' to='2014' company='SIL International'>
//      Copyright ( c ) 2009, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Set PDF License
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace ApplyPDFLicenseInfo
{
    public static class SetLicense
    {
        public static int ExitCode;
        public static string RedirectOutput;
        public static string LastError;
        private static readonly Process MyProcess = new Process();
        private static int _elapsedTime;
        private static bool _eventHandled;

        public static void RunCommand(string instPath, string name, string arg, bool wait)
        {
            _elapsedTime = 0;
            _eventHandled = false;

            try
            {
                // Start a process to print a file and raise an event when done.
                MyProcess.StartInfo.FileName = name;
                MyProcess.StartInfo.Arguments = arg;
                MyProcess.StartInfo.CreateNoWindow = true;
                MyProcess.EnableRaisingEvents = true;
                MyProcess.StartInfo.CreateNoWindow = true;
                MyProcess.StartInfo.UseShellExecute = string.IsNullOrEmpty(RedirectOutput);
                MyProcess.StartInfo.WorkingDirectory = instPath;

                MyProcess.Exited += new EventHandler(myProcess_Exited);
                MyProcess.Start();

            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(RedirectOutput))
                {
                    string result = string.Empty;
                    StreamWriter streamWriter = new StreamWriter(Path.Combine(instPath, RedirectOutput));
                    var errorArgs = string.Format("An error occurred trying to print \"{0}\":" + "\n" + ex.Message, arg);
                    result += errorArgs;
                    streamWriter.Write(result);
                    streamWriter.Close();
                    RedirectOutput = null;
                }
                return;
            }

            // Wait for Exited event, but not more than 30 seconds. 
            const int SLEEP_AMOUNT = 100;
            while (!_eventHandled)
            {
                _elapsedTime += SLEEP_AMOUNT;
                if (_elapsedTime > 30000)
                {
                    break;
                }
                Thread.Sleep(SLEEP_AMOUNT);
            }
            MyProcess.Close();
        }

        // Handle Exited event and display process information. 
// ReSharper disable InconsistentNaming
        private static void myProcess_Exited(object sender, System.EventArgs e)
// ReSharper restore InconsistentNaming
        {

            _eventHandled = true;
        }

        /// <summary>
        /// Make sure the path contains the proper / for the operating system.
        /// </summary>
        /// <param name="path">input path</param>
        /// <returns>normalized path</returns>
        public static string DirectoryPathReplace(string path)
        {
            if (string.IsNullOrEmpty(path)) return path;

            string returnPath = path.Replace('/', Path.DirectorySeparatorChar);
            returnPath = returnPath.Replace('\\', Path.DirectorySeparatorChar);
            return returnPath;
        }

        public static bool UnixVersionCheck()
        {
            bool isRecentVersion = false;
            try
            {
                string getOSName = GetOsName();
                string majorVersion = string.Empty;
                if (getOSName.IndexOf("Unix") >= 0)
                {
                    isRecentVersion = true;
                    majorVersion = getOSName.Substring(0, 11);
                }
                if (majorVersion == "Unix 3.2.0.")
                {
                    isRecentVersion = true;
                }
            }
            catch { }
            return isRecentVersion;
        }

        public static string GetOsName()
        {
            OperatingSystem osInfo = Environment.OSVersion;
			var versionString = osInfo.VersionString;
            switch (osInfo.Platform)
            {
                case System.PlatformID.Win32NT:
                    switch (osInfo.Version.Major)
                    {
                        case 3:
                            versionString = "Windows NT 3.51";
                            break;
                        case 4:
                            versionString = "Windows NT 4.0";
                            break;
                        case 5:
                            versionString = osInfo.Version.Minor == 0 ? "Windows 2000" : "Windows XP";
                            break;
                        case 6:
		                    versionString = osInfo.Version.Minor == 1 ? "Windows7" : "Windows8";
		                    break;
                    }
                    break;

            }
            return versionString;
        }
    }
}
