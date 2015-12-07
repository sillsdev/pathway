// --------------------------------------------------------------------------------------------
// <copyright file="Substitution.cs" from='2009' to='2014' company='SIL International'>
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
// 
// </remarks>
// --------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace BuildStep
{
    /// <summary>
    /// Class has methods to do systematic substitution within control files.
    /// </summary>
    public class Substitution
    {
        #region Properties
        /// <summary>
        /// Gets or sets path name where substitution takes place
        /// </summary>
        public string TargetPath { get; set; }

        /// <summary>
        /// Gets or sets input file name (used when pattern changes are done)
        /// </summary>
        public string InputFile { get; set; }

        /// <summary>
        /// Gets or sets input template (used by substitution)
        /// </summary>
        public string InputTemplate { get; set; }

        /// <summary>
        /// Gets or sets dictionary of names and their replacement values
        /// </summary>
        public Dictionary<string, string> NameMap { get; set; }

        /// <summary>
        /// Gets or sets output file to be created
        /// </summary>
        public string OutputFile { get; set; }

        /// <summary>
        /// Gets or sets open and closing bracket for block substitutions.
        /// </summary>
        public string Brackets { get; set; }
        #endregion Properties

        #region FileSubstitute
        /// <summary>
        /// Substitute using NameMap in InpudTemplate producing OutputFile in TargetPath.
        /// </summary>
        public void FileSubstitute()
        {
            var templateFullName = TargetPath != null ? Path.Combine(TargetPath, InputTemplate) : InputTemplate;
            string inp = FileData.Get(templateFullName);

            var del = new MyDelegate(NameMap);
            // Find %(xx)<yy> and look up xx in dicMap and if it is not blank include yy in result.
            if (Brackets == null || Brackets.Length < 2)
                Brackets = "<>";
            string blockPattern = string.Format(@"%\(([^)]*)\){0}([^{1}]*){1}", Brackets.Substring(0, 1), Brackets.Substring(1, 1));
            string res = DoSubstitute(inp, blockPattern, RegexOptions.Multiline, del.MyOptional);
            inp = res;
            // Find %(xx)s in line and look up xx in dicMap. If found put the value in place of this designator.
            res = DoSubstitute(inp, @"%\(([^)]*)\)s", RegexOptions.None, del.MyValue);

            var outputFullName = TargetPath != null ? Path.Combine(TargetPath, OutputFile) : OutputFile;
            StreamWriter sw = File.CreateText(outputFullName);
            sw.Write(res);
            sw.Close();
        }

        /// <summary>
        /// Substitutes names in template with values from map creating output file
        /// </summary>
        public void FileSubstitute(string template, Dictionary<string, string> map, string output)
        {
            InputTemplate = template;
            NameMap = map;
            OutputFile = output;
            FileSubstitute();
        }

        /// <summary>
        /// Substitutes names in template with values from map creating output file by removing -tpl from template name
        /// </summary>
        public void FileSubstitute(string template, Dictionary<string, string> map)
        {
            InputTemplate = template;
            NameMap = map;
            OutputFile = template.Replace("-tpl", "");
            FileSubstitute();
        }
        #endregion FileSubstitute

        #region UpdateGroup1
        /// <summary>
        /// Sustitute all occurances of pattern.Group(1) with myValue
        /// </summary>
        public void UpdateGroup1(string pattern, string myValue)
        {
            var inputFullPath = TargetPath != null ? Path.Combine(TargetPath, InputFile) : InputFile; 
            string data = FileData.Get(inputFullPath);
            var mats = Regex.Matches(data, pattern);
            var outputFullName = TargetPath != null ? Path.Combine(TargetPath, OutputFile) : OutputFile;
            var ostr = new StreamWriter(outputFullName);
            int beg = 0;
            foreach (Match m in mats)
            {
                ostr.Write(data.Substring(beg, m.Groups[1].Index - beg));
                ostr.Write(myValue);
                beg = m.Groups[1].Index + m.Groups[1].Length;
            }
            ostr.Write(data.Substring(beg));
            ostr.Close();
        }
        #endregion UpdateGroup1

        #region MyDelegate
        /// <summary>
        /// myDelegate class contains the dictionary of settings and insert functions for template
        /// </summary>
        public class MyDelegate
        {
            #region Private Variables
            /// <summary>
            /// Keeps track of dictionary used by all substitution methods
            /// </summary>
            readonly Dictionary<string, string> _myDicMap;
            #endregion

            /// <summary>
            /// Delegate returns string for match m
            /// </summary>
            public delegate string DoSubValue(Match m);

            #region Constructor
            /// <summary>
            /// Initializes a new instance of the MyDelegate class. Retains dicMap for use by methods.
            /// </summary>
            public MyDelegate(Dictionary<string, string> dicMap)
            {
                _myDicMap = dicMap;
            }
            #endregion

            #region myOptional
            /// <summary>
            /// Delegate function for angle bracket matching
            /// </summary>
            /// <param name="m">match containing two groups: variable name and optional lines</param>
            /// <returns>value to be inserted in output</returns>
            public string MyOptional(Match m)
            {
                if (_myDicMap.ContainsKey(m.Groups[1].Value))
                {
                    if (_myDicMap[m.Groups[1].Value] != "")
                        return m.Groups[2].Value;
                    return "";
                }
                return null;
            }
            #endregion

            #region myValue
            /// <summary>
            /// Delegate function for simple variable substitution
            /// </summary>
            /// <param name="m">match containing the variable name</param>
            /// <returns>value to be inserted</returns>
            public string MyValue(Match m)
            {
                return _myDicMap.ContainsKey(m.Groups[1].Value) ? _myDicMap[m.Groups[1].Value] : null;
            }
            #endregion
        }
        #endregion MyDelegate

        #region DoSubstitute
        /// <summary>
        /// generic substitution
        /// </summary>
        /// <param name="inp">input data</param>
        /// <param name="pattern">pattern to match</param>
        /// <param name="options">options for pattern matching</param>
        /// <param name="value">delegate function to find value to insert for each match</param>
        /// <returns>input data with substitutions made</returns>
        public string DoSubstitute(string inp, string pattern, RegexOptions options, MyDelegate.DoSubValue value)
        {
            int st = 0;
            string res = "";
            while (true)
            {
                Match m = Regex.Match(inp.Substring(st), pattern, options);
                if (m.Success != true)
                    break;
                int idx = m.Index;
                res += inp.Substring(st, idx);
                res += value(m);
                st += idx + m.Length;
            }
            res += inp.Substring(st);
            return res;
        }
        #endregion DoSubstitute
    }
}