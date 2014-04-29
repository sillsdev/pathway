// --------------------------------------------------------------------------------------------
// <copyright file="Substitution.cs" from='2009' to='2014' company='SIL International'>
//      Copyright ( c ) 2014, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Class has methods to do systematic substitution within control files.
// </remarks>
// --------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SIL.Tool
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
            readonly Dictionary<string, string> myDicMap;
            #endregion

            /// <summary>
            /// Delegate returns string for match m
            /// </summary>
            public delegate string doSubValue(Match m);

            #region Constructor
            /// <summary>
            /// Initializes a new instance of the MyDelegate class. Retains dicMap for use by methods.
            /// </summary>
            public MyDelegate(Dictionary<string, string> dicMap)
            {
                myDicMap = dicMap;
            }
            #endregion

            #region myValue
            /// <summary>
            /// Delegate function for simple variable substitution
            /// </summary>
            /// <param name="m">match containing the variable name</param>
            /// <returns>value to be inserted</returns>
            public string myValue(Match m)
            {
                return myDicMap.ContainsKey(m.Groups[1].Value) ? myDicMap[m.Groups[1].Value] : null;
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
        public string DoSubstitute(string inp, string pattern, RegexOptions options, MyDelegate.doSubValue value)
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