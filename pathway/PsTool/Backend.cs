// --------------------------------------------------------------------------------------------
// <copyright file="Backend.cs" from='2009' to='2009' company='SIL International'>
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
// Library for Dictionary Express
// </remarks>
// --------------------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using SIL.Tool;



namespace SIL.PublishingSolution

{
    public static class Backend
    {
        private static List<IExportProcess> _backend = new List<IExportProcess>();
        private static ArrayList _exportType = new ArrayList();
        private static VerboseClass verboseClass;
        public static void Load(string path)
        {
            if (!Directory.Exists(path))
            {
                return;
            }

            _backend.Clear();
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.dll"))
            {
                var className = string.Empty;
                if (fileInfo.Name == "OpenOfficeConvert.dll")
                    className = "LibreOffice";
                else if (fileInfo.Name.Contains("Convert"))
                    className = Path.GetFileNameWithoutExtension(fileInfo.Name).Replace("Convert", "");
                else if (fileInfo.Name.Contains("Writer"))
                    className = Path.GetFileNameWithoutExtension(fileInfo.Name).Replace("Writer", "");
                else
                    continue;
                className = "SIL.PublishingSolution.Export" + className;
                IExportProcess exportProcess = CreateObject(fileInfo.FullName, className) as IExportProcess;

                _backend.Add(exportProcess);
            }
        }

        public static ArrayList GetExportType(string inputDataType)
        {
            _exportType.Clear();
            foreach (IExportProcess process in _backend)
            {
                if (process.Handle(inputDataType))
                {
                    if (process.ExportType.ToLower() == "openoffice/libreoffice")
                    {
                        //_exportType.Add("Word (Using OpenOffice)");  // TD-1818 
                        _exportType.Add("Pdf (Using OpenOffice/LibreOffice) ");
                    }
                    _exportType.Add(process.ExportType);
                }
            }
            return _exportType;
        }

        public static bool Launch(string type, PublicationInformation publicationInformation)
        {
            string xhtmlFile = publicationInformation.DefaultXhtmlFileWithPath;
            CreateVerbose(publicationInformation);
            var localType = type.Replace(@"\", "/").ToLower();
            try
            {
                foreach (IExportProcess process in _backend)
                {
                    if (process.ExportType.ToLower() == "openoffice/libreoffice")
                        type = OpenOfficeClassifier(publicationInformation, type); // Cross checking for OpenOffice

                    if (process.ExportType.ToLower() == localType)
                        return process.Export(publicationInformation);
                }
            }
            finally
            {
                publicationInformation.DefaultXhtmlFileWithPath = xhtmlFile;
                ShowVerbose(publicationInformation);
            } 
            return false;
        }

        private static void ShowVerbose(PublicationInformation publicationInformation)
        {
            if (verboseClass.ErrorCount > 0)
            {
                verboseClass.Close();
                string errFileName = Path.GetFileNameWithoutExtension(publicationInformation.DefaultXhtmlFileWithPath) + "_err.html";
                Common.OpenOutput(errFileName);
            }
        }

        private static void CreateVerbose(PublicationInformation publicationInformation)
        {
            verboseClass = VerboseClass.GetInstance();
            verboseClass.ErrorCount = 0; // reset to zero for every launch
            if (!File.Exists(publicationInformation.DefaultCssFileWithPath))
            {
                verboseClass.ErrorFileName = Path.GetFileNameWithoutExtension(publicationInformation.DefaultXhtmlFileWithPath) + "_err.html";
                verboseClass.WriteError("", "", publicationInformation.DefaultCssFileWithPath + "  Css file is missing", "");
            }
        }

        private static string OpenOfficeClassifier(PublicationInformation publicationInformation, string type)
        {
            if (type.ToLower().IndexOf("libreoffice") >= 0)
            {
                publicationInformation.FinalOutput = "odt";
                //if(type.ToLower().IndexOf("word") >= 0)   // TD-1818 
                //{
                //    publicationInformation.FinalOutput = "doc";
                //}
                if (type.ToLower().IndexOf("pdf") >= 0)
                {
                    publicationInformation.FinalOutput = "pdf";
                }
                type = "openoffice/libreoffice";
            }
            return type;
        }

        /// <summary>
        /// Dynamically find an assembly and create an object of the name to class.
        /// </summary>
        /// <param name="className"></param>
        /// <param name="assemblyPath"></param>
        /// <returns></returns>
        static public Object CreateObject(string assemblyPath, string className)
        {
            return CreateObject(assemblyPath, className, null);
        }

        static string CouldNotCreateObjectMsg(string assemblyPath, string className)
        {
            return "XCore found the DLL "
                + assemblyPath
                + " but could not create the class: "
                + className
                + ". If there are no 'InnerExceptions' below, then make sure capitalization is correct and that you include the name space (e.g. XCore.Ticker).";
        }

        /// <summary>
        /// Dynamically find an assembly and create an object of the name to class.
        /// </summary>
        /// <param name="assemblyPath1"></param>
        /// <param name="className1"></param>
        /// <param name="args">args to the constructor</param>
        /// <returns></returns>
        static public Object CreateObject(string assemblyPath1, string className1, object[] args)
        {
            Assembly assembly;
            string assemblyPath = GetAssembly(assemblyPath1, out assembly);

            string className = className1.Trim();
            Object thing = null;
            try
            {
                //Module[] a =  assembly.GetLoadedModules();
                //foreach (var module in a)
                //{
                //    MethodInfo[] b = module.GetMethods();
                //    foreach (var info in b)
                //    {
                //        string c = info.ToString();   
                //    }
                //}

                //FileStream[] f = assembly.GetFiles();
                //Type[] t =  assembly.GetExportedTypes();

                //make the object
                //Object thing = assembly.CreateInstance(className);
                thing = assembly.CreateInstance(className, false, BindingFlags.Instance | BindingFlags.Public,
                    null, args, null, null);
            }
            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                string message = CouldNotCreateObjectMsg(assemblyPath, className);

                Exception inner = err;

                while (inner != null)
                {
                    message += "\r\nInner exception message = " + inner.Message;
                    inner = inner.InnerException;
                }
                throw new ConfigurationErrorsException(message);
            }
            if (thing == null)
            {
                // Bizarrely, CreateInstance is not specified to throw an exception if it can't
                // find the specified class. But we want one.
                throw new ConfigurationErrorsException(CouldNotCreateObjectMsg(assemblyPath, className));
            }
            return thing;
        }

        private static string GetAssembly(string assemblyPath1, out Assembly assembly)
        {
            // Whitespace will cause failures.
            string assemblyPath = assemblyPath1.Trim();
            //allow us to say "assemblyPath="%fwroot%\Src\XCo....  , at least during testing
            // RR: It may allow it, but it crashes, when it can't find the dll.
            //assemblyPath = System.Environment.ExpandEnvironmentVariables(assemblyPath);
            string baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Substring(6);

            try
            {
                assembly = Assembly.LoadFrom(Common.PathCombine(baseDir, assemblyPath));
            }
            catch (Exception)
            {
                try
                {
                    //Try to find without specifying the directory, 
                    //so that we find things that are in the Path environment variable
                    //This is useful in extension situations where the extension's bin directory 
                    //is not the same as the FieldWorks binary directory (e.g. WeSay)
                    assembly = Assembly.LoadFrom(assemblyPath);
                }
                catch (Exception error)
                {
                    throw new RuntimeConfigurationException("XCore Could not find the DLL at :" + assemblyPath, error);
                }
            }
            return assemblyPath;
        }
        /// <summary>
        /// Use this exception when the format of the configuration XML
        /// may be fine, but there is a run-time linking problem with an assembly or class that was specified.
        /// </summary>
        public class RuntimeConfigurationException : ApplicationException
        {
            public RuntimeConfigurationException(string message)
                : base(message)
            {

            }

            /// <summary>
            /// Use this one if you are inside of a catch block where you have access to the original exception
            /// </summary>
            /// <param name="message"></param>
            /// <param name="innerException"></param>
            public RuntimeConfigurationException(string message, Exception innerException)
                : base(message, innerException)
            {

            }
        }

    }
}
