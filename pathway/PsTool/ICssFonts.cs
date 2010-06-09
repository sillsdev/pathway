// --------------------------------------------------------------------------------------------
// <copyright file="ICssFonts.cs" from='2009' to='2009' company='SIL International'>
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
// ICssFonts returns a list of font names used by CSS file.
// </remarks>
// --------------------------------------------------------------------------------------------

using System.Collections;

namespace SIL.PublishingSolution
{
    public interface ICssFonts
    {
        /// <summary>
        /// Return a list of font names used in the css file.
        /// </summary>
        /// <param name="name">used to name temporary files.</param>
        /// <param name="cssFile">where css info is stored</param>
        /// <returns>an array list of font names</returns>
        ArrayList GetFontList(string name, string cssFile);
    }
}
