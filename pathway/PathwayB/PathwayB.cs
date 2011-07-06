// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2010, SIL International. All Rights Reserved.
// <copyright from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: PathwayB.cs
// Responsibility: Greg Trihus
// ---------------------------------------------------------------------------------------------
using System;
using System.IO;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    class Program
    {
        static void Main(string[] args)
        {
            var projectInfo = new PublicationInformation
                                  {
                                      ProjectInputType = "Dictionary",
                                      DefaultXhtmlFileWithPath = null,
                                      DefaultCssFileWithPath = null,
                                      IsOpenOutput = false,
                                      ProjectName = "main",
                                  };
            var backendPath = Common.ProgInstall;
            var exportType = "OpenOffice/LibreOffice";
            try
            {
                int i = 0;
                while (i < args.Length)
                {
                    switch (args[i++])
                    {
                        case "--xhtml":
                        case "-x":
                            projectInfo.DefaultXhtmlFileWithPath = args[i++];
                            break;
                        case "--css":
                        case "-c":
                            projectInfo.DefaultCssFileWithPath = args[i++];
                            break;
                        case "--target":
                        case "-t":
                            //Note: If export type is more than one word, quotes must be used
                            exportType = args[i++];
                            break;
                        case "--input":
                        case "-i":
                            projectInfo.ProjectInputType = args[i++];
                            break;
                        case "--launch":
                        case "-l":
                            projectInfo.IsOpenOutput = true;
                            break;
                        case "--name":
                        case "-n":
                            projectInfo.ProjectName = args[i++];
                            break;
                        default:
                            Usage();
                            throw new ArgumentException("Invalid Command Line Argument");
                    }
                }
                Common.Testing = false;
                //_projectInfo.ProgressBar = null;

                if (projectInfo.DefaultXhtmlFileWithPath == null || projectInfo.DefaultCssFileWithPath == null)
                {
                    Usage();
                    throw new ArgumentException("Missing required option.");
                }
                if (!File.Exists(projectInfo.DefaultXhtmlFileWithPath))
                    throw new ArgumentException(string.Format("Missing {0}", projectInfo.DefaultXhtmlFileWithPath));
                if (!File.Exists(projectInfo.DefaultCssFileWithPath))
                    throw new ArgumentException(string.Format("Missing {0}", projectInfo.DefaultCssFileWithPath));
                projectInfo.DictionaryPath = Path.GetDirectoryName(projectInfo.DefaultXhtmlFileWithPath);

                if (backendPath.Length == 0)
                {
                    backendPath = Common.GetPSApplicationPath();
                }

                Common.ProgBase = Common.GetPSApplicationPath();
                Param.LoadSettings();
                
                Backend.Load(backendPath);

                Common.ShowMessage = false;
                projectInfo.DictionaryOutputName = projectInfo.ProjectName;
                Backend.Launch(exportType, projectInfo);
            }
            catch (Exception err)
            {
                Console.Write(err.Message);
                Environment.Exit(-1);
            }
            Environment.Exit(0);
        }
        static void Usage()
        {
            var msg = "Usage PathwayB [Options]\r\n";
            msg += "--xhtml -x\tconent file name (required)\r\n";
            msg += "--css -c\tlayout file name (required)\r\n";
            msg += "--target -t\tTarget: [OpenOffice/LibreOffice], InDesign alpha, etc.\r\n";
            msg += "--input -i\t[Dictionary] or Scripture\r\n";
            msg += "--launch -l\tlaunch resulting output in target back end.\r\n";
            msg += "--name -n\t[main] Project name\r\n";
            Console.Write(msg);
        }
    }
}
