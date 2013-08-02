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
        private static Process myProcess = new Process();
        private static int _elapsedTime;
        private static bool _eventHandled;

        public static void RunCommand(string instPath, string name, string arg, bool wait)
        {
            _elapsedTime = 0;
            _eventHandled = false;

            try
            {
                // Start a process to print a file and raise an event when done.
                myProcess.StartInfo.FileName = name;
                //myProcess.StartInfo.Verb = name;
                myProcess.StartInfo.Arguments = arg;
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.EnableRaisingEvents = true;
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.StartInfo.UseShellExecute = string.IsNullOrEmpty(RedirectOutput);
                myProcess.StartInfo.WorkingDirectory = instPath;

                myProcess.Exited += new EventHandler(myProcess_Exited);
                myProcess.Start();

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
                //Console.WriteLine("An error occurred trying to print \"{0}\":" + "\n" + ex.Message, arg);
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
            myProcess.Close();
        }

        // Handle Exited event and display process information. 
        private static void myProcess_Exited(object sender, System.EventArgs e)
        {

            _eventHandled = true;
            //  Console.WriteLine("Exit time:    {0}\r\n" + "Exit code:    {1}\r\nElapsed time: {2}", myProcess.ExitTime, myProcess.ExitCode, elapsedTime);
        }

        /// <summary>
        /// Make sure the path contains the proper / for the operating system.
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <returns>normalized path</returns>
        public static string PathCombine(string path1, string path2)
        {
            //if (path1 == null) throw new ArgumentNullException("path1");
            //if (path2 == null) throw new ArgumentNullException("path2");
            path1 = DirectoryPathReplace(path1);
            path2 = DirectoryPathReplace(path2);
            return Path.Combine(path1, path2);
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
    }
}
