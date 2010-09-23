// --------------------------------------------------------------------------------------------
// <copyright file="VerboseClass.cs" from='2009' to='2009' company='SIL International'>
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
// To store the Css errors in the Error.xhmtl file
// </remarks>
// --------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SIL.Tool
{
    /// <summary>
    /// Verbose Class handles the Error generated in css file
    /// </summary>
    public class VerboseClass
    {
        /// <summary>
        /// stores the instance of the VerboseClass
        /// </summary>
        private static VerboseClass instance;

        /// <summary>
        /// stores the value of error writing system is on / off
        /// </summary>
        private static bool showError;

        /// <summary>
        /// stores the value of error already written or not
        /// </summary>
        private static bool errorWritten;

        /// <summary>
        /// stores the value of FileName in which error to be written
        /// </summary>
        private static string errorFileName;

        /// <summary>
        /// stores the error Count
        /// </summary>
        private static int errorCount;

        /// <summary>
        /// stores the value of stream writer
        /// </summary>
        private static StreamWriter fileWriter;

        /// <summary>
        /// Gets or sets a value indicating whether show Error is true / false
        /// </summary>
        public bool ShowError
        {
            get { return showError; }
            set { showError = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the ErrorWritten is true / false
        /// </summary>
        public bool ErrorWritten
        {
            get { return errorWritten; }
            set { errorWritten = value; }
        }

        /// <summary>
        /// Gets or sets the ErrorFileName
        /// </summary>
        public string ErrorFileName
        {
            get { return errorFileName; }
            set { errorFileName = value; }
        }

        /// <summary>
        /// Gets or sets the ErrorCount
        /// </summary>
        public int ErrorCount
        {
            get { return errorCount; }
            set { errorCount = value; }
        }
        /// <summary>
        /// Prevents a default instance of the VerboseClass class from being created
        /// </summary>
        private VerboseClass()
        {
        }

        /// <summary>
        /// Initialize the VerboseClass and returns if already initialized
        /// </summary>
        /// <returns>returns the instance of VerboseClass</returns>
        public static VerboseClass GetInstance()
        {
            if (instance == null)
            {
                instance = new VerboseClass {ErrorCount = 0};
            }
            return instance;
        }

        /// <summary>
        /// Handles the error writing function
        /// </summary>
        /// <param name="className">From which css class the error raised</param>
        /// <param name="attributeName">From which Property</param>
        /// <param name="errorMessage">Error raised message</param>
        /// <param name="attributeValue">From what value the error raised</param>
        public void WriteError(string className, string attributeName, string errorMessage, string attributeValue)
        {
            if (showError)
            {
                if (!errorWritten)
                {
                    string ProductName = Common.GetProductName();
                    var fileStream = new FileInfo(errorFileName);
                    fileWriter = fileStream.CreateText();
                    fileWriter.WriteLine("<html>");
                    fileWriter.WriteLine("<head>");
                    fileWriter.WriteLine("<title>" + ProductName + "</title>");
                    fileWriter.WriteLine("<style type=\"text/css\">");
                    fileWriter.WriteLine(".color_red {color:Black;} .color_blue{color:Black;font-weight:Bold;} .color_green{color:Black;}");
                    fileWriter.WriteLine("</style>");
                    fileWriter.WriteLine("</head>");
                    fileWriter.WriteLine("<body>");
                    fileWriter.WriteLine("<H1> " + ProductName + " - Warning </H1>");
                    fileWriter.WriteLine("<table border=2>");
                    fileWriter.WriteLine("<tr> <td class=\"color_blue\">Class Name</td> <td class=\"color_blue\">Propery Name</td> <td class=\"color_blue\">Value</td> <td class=\"color_blue\">Description</td> </tr>");

                    fileWriter.WriteLine("<tr>");
                    fileWriter.WriteLine("<td class=\"color_red\">" + className + "</td>");
                    fileWriter.WriteLine("<td class=\"color_green\">" + attributeName + "</td>");
                    fileWriter.WriteLine("<td class=\"color_green\">" + attributeValue + "</td>");

                    fileWriter.WriteLine("<td>" + errorMessage + "</td>");
                    fileWriter.WriteLine("</tr>");
                    errorWritten = true;
                }
                else
                {
                    fileWriter.WriteLine("<tr>");
                    fileWriter.WriteLine("<td class=\"color_red\">" + className + "</td>");
                    fileWriter.WriteLine("<td class=\"color_green\">" + attributeName + "</td>");
                    fileWriter.WriteLine("<td class=\"color_green\">" + attributeValue + "</td>");

                    fileWriter.WriteLine("<td>" + errorMessage + "</td>");
                    fileWriter.WriteLine("</tr>");
                }
                errorCount++;
            }
        }

        /// <summary>
        /// Directly writing the error without the class names
        /// </summary>
        /// <param name="content">The direct text to be written</param>
        public void WriteError(string content)
        {
            fileWriter.WriteLine(content);
        }

        /// <summary>
        /// Closing the error file
        /// </summary>
        public void Close()
        {
            fileWriter.Close();
        }
    }
}