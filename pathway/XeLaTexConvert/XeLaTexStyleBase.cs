// --------------------------------------------------------------------------------------------
// <copyright file="XeLaTexStyleBase.cs" from='2009' to='2014' company='SIL International'>
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class XeLaTexStyleBase
    {
        protected Dictionary<string, string> LeftPageLayoutProperty;
        protected Dictionary<string, string> RightPageLayoutProperty;
        protected PublicationInformation _projInfo = new PublicationInformation();
    }
}