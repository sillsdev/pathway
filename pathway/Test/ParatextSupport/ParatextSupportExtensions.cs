// --------------------------------------------------------------------------------------------
// <copyright file="ParatextSupportExtensions.cs" from='2009' to='2014' company='SIL International'>
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

using System.Xml.Xsl;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.ParatextSupport
{
    /// <summary>
    /// ParatextSupportExtensions uses reflection to expose items for testing in the ParatextSupport dll. 
    /// </summary>
    public static class ParatextSupportExtensions
    {
        public static XslCompiledTransform UsxToUsfmXslt(ParatextPathwayLink converter)
        {
            return (XslCompiledTransform)ReflectionHelperLite.GetField(converter, "m_usxToXhtml");
        }

        public static XslCompiledTransform EncloseParasInSectionsXslt(ParatextPathwayLink converter)
        {
            return (XslCompiledTransform)ReflectionHelperLite.GetField(converter, "m_encloseParasInSections");
        }
    }
}
