// --------------------------------------------------------------------------------------------
// <copyright file="XmlFun.cs" from='2009' to='2014' company='SIL International'>
//      Copyright © 2014, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Library for Pathway
// </remarks>
// --------------------------------------------------------------------------------------------

using System;

namespace SIL.Tool
{
    /// <summary>Duplicates functions of xml eliminating need to access Internet to get them.</summary>
    public class XmlFun
    {
        public string data(string d)
        {
            return d;
        }

        public string substring(string s, int start, int len)
        {
            return s.Substring(start - 1, len);
        }

        public int stringLength(string s)
        {
            return s.Length;
        }

        // abs used to report progress
        public float abs(float n)
        {
            if (Common.XsltProgressBar != null)
                Common.XsltProgressBar.PerformStep();
            return Math.Abs(n);
        }
    }
}