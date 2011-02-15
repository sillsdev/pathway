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
using System.Text.RegularExpressions;
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
        #endregion Private Methods
    }
}
