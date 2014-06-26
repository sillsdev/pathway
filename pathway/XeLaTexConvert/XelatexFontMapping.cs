// --------------------------------------------------------------------------------------------
// <copyright file="ExportXeLaTexConvert.cs" from='2009' to='2014' company='SIL International'>
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
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class XelatexFontMapping
    {
        public XelatexFontMapping()
        {
            _isLinux = Common.IsUnixOS();
        }


        private bool _isLinux = false;

        public Dictionary<string, string> GetFontList(Dictionary<string, string> fontName)
        {
            Dictionary<string, string> fontList = new Dictionary<string, string>();
            if (_isLinux)
            {
                foreach (var font in fontName)
                {
                    if (!FontInternals.IsInstalled(font.Value))
                    {
                        var fullName = FontInternals.GetFontName(font.Key, string.Empty);
                        if (fullName.Length > 0) // if length > 0, it's installed
                        {
                            // this style is installed on the machine - add it to the list
                            fontList.Remove(font.Key);
                            fontList.Add(font.Key, Path.GetFileName(fullName));
                        }
                    }
                }
            }
            else
            {
                return fontName;
            }
            return fontList;
        }
    }
}
