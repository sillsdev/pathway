// --------------------------------------------------------------------------------------------
// <copyright file="FlexString.cs" from='2009' to='2014' company='SIL International'>
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
// 
// </remarks>
// --------------------------------------------------------------------------------------------

using System.Text.RegularExpressions;

namespace SIL.Tool
{
    public class FlexString
    {
        /// <summary>
        /// This Method is used to avoid the regex function in XSLT
        /// Sense Number returns the sense, homograph number, sense number.
        /// <param name="input"> comes from the xslt file node</param>
        /// <param name="checkMethod"> Check the type</param>
        /// <returns>Mathched Pattern Value</returns>        
        /// </summary>
        public string SenseNumber(string input, string checkMethod)
        {
            string returnValue = "";
            Match test;

            if (checkMethod == "sense")
            {
                //Mathches the String.
                test = Regex.Match(input, "([^0-9]+)");
                returnValue = test.Groups[0].ToString();
            }

            else if (checkMethod == "homograph")  // homograph
            {
                test = Regex.Match(input, "([a-zA-Z][0-9]{1,3})"); // numeric precedes with alphabet- xhomographNumber
                test = Regex.Match(test.Groups[0].ToString(), "([0-9]{1,2})"); // only numeric
                returnValue = test.Groups[0].ToString();
            }
            else if (checkMethod == "sensenumber")
            {
                test = Regex.Match(input, "( [0-9]+)");   // with space - xsensenumber
                returnValue = test.Groups[0].ToString();

            }
            return returnValue;
        }
    }
}