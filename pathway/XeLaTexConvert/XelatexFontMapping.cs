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
                var installedFontList = InstalledFontList();

                foreach (var cfont in fontName)
                {
                    var fontFileName = CheckFontFileName(cfont);
                    if (installedFontList.ContainsKey(fontFileName))
                    {
                        fontList.Remove(cfont.Key);
                        fontList.Add(cfont.Key, cfont.Value);
                    }
                    else
                    {
                        var fullName = FontInternals.GetFontName(cfont.Key, string.Empty);
                        if (fullName.Length > 0) // if length > 0, it's installed
                        {
                            // this style is installed on the machine - add it to the list
                            fontList.Remove(cfont.Key);
                            fontList.Add(cfont.Key, Path.GetFileName(fullName));
                        }
                        else
                        {
                            if (fullName.Length == 0)
                            {
                                string fontCode = Common.LeftString(cfont.Key, "-");
                                
                                fullName = FontInternals.GetFontName(fontCode, string.Empty);
                                if (fullName.Length != 0 && fullName.Length > 0) // if length > 0, it's installed
                                {
                                    // this style is installed on the machine - add it to the list
                                    fontList.Remove(cfont.Key);
                                    fontList.Add(cfont.Key, Path.GetFileName(fullName));
                                }
                                else
                                {
                                    if (cfont.Key == "ggo-Telu-IN")
                                    {
                                        if (fontList.ContainsKey("te"))
                                        {
                                            fontList.Remove(cfont.Key);
                                            fontList.Add(cfont.Key, fontList["te"]);
                                        }
                                    }
                                    else
                                    {
                                        // this style is installed on the machine - add it to the list
                                        fontList.Remove(cfont.Key);
                                        fontList.Add(cfont.Key, "Charis SIL");
                                    }
                                }
                            }
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

        private static string CheckFontFileName(KeyValuePair<string, string> cfont)
        {
            string fontFileName;
            fontFileName = cfont.Value == "Scheherazade Graphite Alpha" ? "Scheherazade" : cfont.Value;
            return fontFileName;
        }

        public static Dictionary<string, string> InstalledFontList()
        {
            Dictionary<string, string> installedFontList = new Dictionary<string, string>();
            string[] files = FontInternals.GetInstalledFontFiles();
            PrivateFontCollection pfc = new PrivateFontCollection();
            foreach (var file in files)
            {
                pfc.AddFontFile(file);
            }
            foreach (var fontFamily in pfc.Families)
            {
                if (!installedFontList.ContainsKey(fontFamily.GetName(0)) && fontFamily.GetName(0) != string.Empty)
                    installedFontList.Add(fontFamily.GetName(0), fontFamily.GetName(0));
            }
            return installedFontList;
        }
    }
}
