// --------------------------------------------------------------------------------------------
// <copyright file="ClassAttribute.cs" from='2009' to='2014' company='SIL International'>
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
// Handling attributes
// </remarks>
// --------------------------------------------------------------------------------------------

namespace SIL.PublishingSolution
{
    public class ClassAttribute
    {
        /// <summary>
        /// h2[level] { 
        ///  color:red; 
        ///}
        ///h2[level='1'] { 
        ///  color:green; 
        ///}
        /// For storing above class attributes
        /// </summary>
        public string Name;
        public string AttributeValue;
        public string AttributeSeperator;
        /// <summary>
        /// To set the Attribute
        /// </summary>
        /// <param name="attribName">Attribute Name</param>
        public void SetAttribute(string attribName)
        {
            Name = attribName.Replace("\'", "");
            AttributeValue = string.Empty;
            AttributeSeperator = string.Empty;
        }
        /// <summary>
        /// To set the Attribute
        /// </summary>
        /// <param name="attribName">Attribute Name</param>
        /// <param name="attribSeperator">Attribute Seperator</param>
        /// <param name="attribValue">Attribute Value</param>
        public void SetAttribute(string attribName, string attribSeperator, string attribValue)
        {
            Name = attribName.Replace("\'", "");
            AttributeValue = attribValue.Replace("\'", "");
            AttributeSeperator = attribSeperator.Replace("\'", "");
        }
    }
}