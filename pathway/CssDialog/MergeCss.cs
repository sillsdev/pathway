// --------------------------------------------------------------------------------------------
// <copyright file="MergeCss.cs" from='2009' to='2009' company='SIL International'>
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
// Combines all css into a single file by implementing @import
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    /// <summary>
    /// Combines all css into a single file by implementing @import
    /// </summary>
    public class MergeCss
    {
        #region Properties
        /// <summary>output location</summary>
        public string OutputLocation { get; set; }
        #endregion Properties

        #region Private variables
        /// <summary>Contains the path name to be used as a relative path for all css</summary>
        string _cssPath = string.Empty;
        /// <summary>Name of output generated</summary>
        string _outName = string.Empty;
        #endregion Member variables

        #region Destructor
        /// <summary>
        /// Remove any temp file created
        /// </summary>
        ~MergeCss()
        {
            if (_outName != string.Empty)
                if (File.Exists(_outName))
                    File.Delete(_outName);
        }
        #endregion Destructor

        #region Public Methods
        /// <summary>
        /// Create temporary CSS that combines all imported
        /// </summary>
        /// <param name="css">root css file</param>
        /// <returns>name of temporary file</returns>
        public string Make(string css, string outputFileName)
        {
            if (string.IsNullOrEmpty(css)) return string.Empty;
            var tmp = Path.GetTempPath();
            Debug.Assert(tmp != null);
            tmp = Path.IsPathRooted(OutputLocation) ? OutputLocation : (!string.IsNullOrEmpty(OutputLocation) ? Common.PathCombine(tmp, OutputLocation) : tmp);
            if (!Directory.Exists(tmp))
                Directory.CreateDirectory(tmp);
            Random rn = new Random();
            if (Common.Testing)
            {
                _outName = Common.PathCombine(tmp, outputFileName);
            }
            else
            {
                _outName = Common.PathCombine(tmp, rn.Next() + outputFileName);
            }
            _cssPath = Path.GetDirectoryName(css);
            var sw = new StreamWriter(_outName);
            ImportFile(css, sw);
            sw.Close();
            return _outName;
        }
        #endregion Public Methods

        #region Private Methods
        /// <summary>
        /// Read a css file into the temporary file
        /// </summary>
        /// <param name="name">name of css file to read</param>
        /// <param name="sw">output stream to write contents</param>
        public void ImportFile(string name, TextWriter sw)
        {
            StreamReader sr;
            var validName = name;
            if (!File.Exists(name))
            {
                validName = Common.PathCombine(_cssPath, Path.GetFileName(name));
                if (!File.Exists(validName))
                {
                    try
                    {
                        validName = Param.StylePath(Path.GetFileName(name));
                    }
                    catch (KeyNotFoundException ex)
                    {
                        return;
                    }
                    if (!File.Exists(validName)) return;
                }
            }
            ImportLayoutStyles(validName);
            sr = new StreamReader(validName);
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                Match m = Regex.Match(line, "@import \"(.*)\";", RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    ImportFile(m.Groups[1].Value, sw);
                    continue;
                }
                sw.WriteLine(line);
                sw.Write(sr.ReadToEnd());
            }
            sr.Close();
        }

        private void ImportLayoutStyles(string name)
        {
            var sbuilder = new StringBuilder();
            string validName = IsLayoutExist(name);
            if(!File.Exists(validName))
            {
                return;
            }

            var reader = new StreamReader(validName);
            while (!reader.EndOfStream)
            {
                string line1 = reader.ReadLine();
                if (line1 != null)
                    if (line1.IndexOf("Running_Head_") >= 0 || line1.IndexOf("PageNumber_") >= 0)
                    {
                        continue;
                    }
                    sbuilder.AppendLine(line1);
            }
            reader.Close();
            string searchText = "@import \"" + Path.GetFileName(validName) + "\";"; //@import "LayoutTwoCol.css";
            string replaceText = sbuilder.ToString();
            Common.StreamReplaceInFile(name, searchText, replaceText);
        }

        private string IsLayoutExist(string name)
        {
            string validName = name;
            var sr = new StreamReader(name);
            while (!sr.EndOfStream)
            {
                string line = sr.ReadToEnd();
                Match m = Regex.Match(line, "@import \"(Layout*.*)\";", RegexOptions.IgnoreCase);
                Match p = Regex.Match(line, "@import \"(Running_Head_Every_Page.*)\";", RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    string fileName = m.Groups[1].Value;
                    if (fileName != "Layout_01.css" && p.Success)
                    {
                        validName = Common.PathCombine(_cssPath, fileName);
                        if (!File.Exists(validName))
                        {
                            try
                            {
                                validName = Param.StylePath(fileName);
                            }
                            catch (KeyNotFoundException ex)
                            {
                               
                            }
                        }
                    }
                }
            }
            sr.Close();
            return validName;
        }
    }
        #endregion Private Methods
    
}
