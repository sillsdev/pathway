// --------------------------------------------------------------------------------------------
// <copyright file="Json.cs" from='2009' to='2014' company='SIL International'>
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
// Create JSON file
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace SIL.PublishingSolution
{
    /// <summary>
    /// Combines all css into a single file by implementing @import
    /// </summary>
    public class Json
    {
        private StringBuilder sbJSON;

        private string _outputFile;
        private int _tagcount = 0;

        public void Create(string outputFile)
        {
            _outputFile = outputFile;
            sbJSON = new StringBuilder();
        }

        public void Close()
        {
            using (StreamWriter writer = new StreamWriter(_outputFile, true))
            {
                writer.Write(sbJSON.ToString());
            }
        }

        public void StartTag()
        {
            sbJSON.Append("{");
            _tagcount++;
        }

        public void EndTag()
        {
            sbJSON.Append("}");
            _tagcount--;
        }

        public void WriteTag(string tag)
        {
            sbJSON.Append("\"");
            sbJSON.Append(tag);
            sbJSON.Append("\":");
        }

        public void WriteText(string txt)
        {
            sbJSON.Append("\"");
            sbJSON.Append(txt);
            sbJSON.Append("\"");
            if (_tagcount == 1)
            {
                sbJSON.Append(",");
            }
        }

        public void WriteRaw(string txt)
        {
            sbJSON.Append(txt);
            if (_tagcount == 1)
            {
                sbJSON.Append(",");
            }
        }

        public void WriteTextNoComma(string txt)
        {
            sbJSON.Append("\"");
            sbJSON.Append(txt);
            sbJSON.Append("\"");
        }

        public void WriteComma()
        {
            sbJSON.Append(",");
        }
    }
}
