// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2016, SIL International. All Rights Reserved.
// <copyright from='2016' to='2016' company='SIL International'>
//		Copyright (c) 2016, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: LoadClasses.cs (from SimpleCss5.cs)
// Responsibility: Greg Trihus
// ---------------------------------------------------------------------------------------------
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace CssSimpler
{
    public class LoadClasses : XmlParser
    {
        public readonly List<string> UniqueClasses = new List<string>();
        public string StyleSheet;
        private readonly string _folder;

        public LoadClasses(string xmlFullName) : base(xmlFullName)
        {
            _folder = Path.GetDirectoryName(xmlFullName);
            Declare(XmlNodeType.Element, CaptureStylesheet);
            Declare(XmlNodeType.Attribute, GetClasses);
            Parse();
            Close();
        }

        private void CaptureStylesheet(XmlReader rdr)
        {
            if (rdr.LocalName != "link") return;
            if (rdr.GetAttribute("rel") == "stylesheet")
            {
                StyleSheet = rdr.GetAttribute("href");
                if (StyleSheet == null) return;
                if (StyleSheet.ToLower().StartsWith("file:///"))
                {
                    StyleSheet = StyleSheet.Substring(8);
                }
                if (!Path.IsPathRooted(StyleSheet))
                {
                    StyleSheet = Path.Combine(_folder, StyleSheet);
                }
            }
        }

        private void GetClasses(XmlReader r)
        {
            if (r.LocalName == "class")
            {
                var myclass = r.Value;
                if (myclass.Contains(" "))
                {
                    foreach (var s in myclass.Split(' '))
                    {
                        if (!string.IsNullOrEmpty(s))
                        {
                            AddUnique(s);
                        }
                    }
                }
                else
                {
                    AddUnique(myclass);
                }
            }
        }

        private void AddUnique(string myclass)
        {
            if (!UniqueClasses.Contains(myclass))
            {
                UniqueClasses.Add(myclass);
            }
        }
    }
}
