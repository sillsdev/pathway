#region // Copyright (C) 2014, SIL International. All Rights Reserved.
// --------------------------------------------------------------------------------------------
// <copyright file="EpubFont.cs" from='2009' to='2014' company='SIL International'>
//      Copyright (C) 2014, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// </remarks>
// --------------------------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Resources;
using System.Windows.Forms;

namespace epubValidator
{
    public static class ValidateEpub
    {
        public static void ValidateEpubFile(string fileName)
        {   
            // launch the item
            var results = Program.ValidateFile(fileName);
            // display the results
            var dlg = new frmResults();
            dlg.ValidationResults = results;
            dlg.ShowDialog();
        }
    }
}
