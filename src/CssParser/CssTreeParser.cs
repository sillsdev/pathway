// --------------------------------------------------------------------------------------------
// <copyright file="CSSTreeParser.cs" from='2009' to='2014' company='SIL International'>
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
// Css Tree Parser
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Text;
using SIL.Tool;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using SIL.PublishingSolution.Compiler;

namespace SIL.PublishingSolution
{
    /// <summary>
    /// CSharp logic for calling Antlr generated parser and returning results as properties.
    /// </summary>
    public class CssTreeParser
    {
        #region Properties
        /// <summary>
        /// Gets the root of the AST tree containing the parsed data.
        /// </summary>
        public CommonTree Root { get; private set; }

        /// <summary>
        /// Gets a list of strings with error messages identified in input stream.
        /// </summary>
        public List<string> Errors { get; private set; }
        #endregion Properties

        #region Parse
        /// <summary>
        /// Parses input stream to tokens and then according to CSS grammar.
        /// </summary>
        /// <param name="inp">file name of input stream.</param>
        public void Parse(string inp)
        {
	        if (String.IsNullOrEmpty(inp))
		        return;

            string fullpath;
            if (Path.IsPathRooted(inp))
                fullpath = inp;
            else
                fullpath = Path.Combine(Environment.CurrentDirectory, inp);

	        if (!File.Exists(fullpath))
	        {
				throw new FileNotFoundException (fullpath);
	        }

            ICharStream input = new ANTLRFileStream(fullpath, Encoding.UTF8);
            csst3Lexer lex = new csst3Lexer(input);
            CommonTokenStream tokens = new CommonTokenStream(lex);
            csst3Parser parser = new csst3Parser(tokens);

            // return results as parameters
            try
            {
                Root = (CommonTree)parser.stylesheet().Tree;
                Errors = parser.GetErrors();
            }
            catch (Exception)
            {
                Errors = parser.GetErrors();
                throw;
            }
        }
        #endregion Parse

        #region StringTree
        /// <summary>
        /// The resulting AST tree can be represented as a string for comparison
        /// </summary>
        /// <returns>string representation of parse tree.</returns>
        public string StringTree()
        {
            return ((ITree)Root).ToStringTree();
        }
        #endregion StringTree

        /// <summary>
        /// Combines string elements of list into string
        /// </summary>
        /// <returns>resulting string</returns>
        public string ErrorText()
        {
            string result = "";
            foreach (string s in Errors)
                result = result + s + "\r\n";
            return result;
        }
        /// <summary>
        /// Remove "line 0:-1"
        /// </summary>
        /// <returns>resulting string</returns>
        public void ValidateError()
        {
            List<string> err = new List<string>();
            err.AddRange(Errors);
            foreach (string s in err)
            {
                if (s.IndexOf("line 0:-1") > -1)
                {
                    Errors.Remove(s);
                }
            }
        }

        #region Children
        /// <summary>
        /// Children will enumerate children of node t.
        /// </summary>
        /// <param name="t">node for which children are to be identified.</param>
        /// <returns>returns one child at a time.</returns>
        public IEnumerable<CommonTree> Children(CommonTree t)
        {
            for (int i = 0; i < t.ChildCount; i++)
                yield return (CommonTree)t.GetChild(i);
        }
        #endregion Children
    }
}
