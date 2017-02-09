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
using System.Diagnostics;
using System.IO;
using System.Xml;
using SIL.Tool;

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
			Debug.Assert(_folder != null, "_folder != null");
			StyleSheet = string.Empty;
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
				var nextStyleSheet = rdr.GetAttribute("href");
				if (nextStyleSheet == null) return;
				if (nextStyleSheet.ToLower().StartsWith("file:///"))
				{
					nextStyleSheet = nextStyleSheet.Substring(8);
				}
				if (!Path.IsPathRooted(nextStyleSheet))
				{
					nextStyleSheet = Path.Combine(_folder, nextStyleSheet);
				}
				if (string.IsNullOrEmpty(StyleSheet))
				{
					StyleSheet = nextStyleSheet;
				}
				else
				{
					if (!File.Exists(nextStyleSheet)) return;
					string content;
					using (var sr = new StreamReader(StyleSheet))
					{
						content = sr.ReadToEnd();
						sr.Close();
					}
					using (var sr = new StreamReader(nextStyleSheet))
					{
						content += sr.ReadToEnd();
						sr.Close();
					}
					using (var sw = new StreamWriter(StyleSheet))
					{
						sw.Write(content);
						sw.Close();
					}
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
