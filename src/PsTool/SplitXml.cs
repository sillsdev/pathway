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
// File: SplitXml.cs
// Responsibility: Greg Trihus
// ---------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace SIL.Tool
{
    public class SplitXml : XmlOneToMany
    {
        public string BaseFolder = "";
        public string Folder = "html5";
        public string Prefix = "s";
        public string FileLetter = "";
        public string Extension = ".html";
        private int _letterNum;
        public string FileName;
        public string FileFullPath;
        public string CssName;
        public long TargetSize = 50L*1024L;
	    public bool OneEntry;
        private int _entryLevel;
	    private readonly string _splitterClass;
        public readonly List<string> FileList = new List<string>();
        public readonly List<string> LetterLabels = new List<string>();
        public readonly List<string> LetterLinks = new List<string>();
        public readonly Dictionary<string, string> IdLinks = new Dictionary<string, string>();

        public SplitXml(string inputFullPath, string splitterClass) : base(inputFullPath)
        {
            NoXmlHeader = true;
	        _splitterClass = splitterClass;
            FileList.Clear();
            LetterLabels.Clear();
            LetterLinks.Clear();
            IdLinks.Clear();
            DeclareBefore(XmlNodeType.Element, AtLetterHead);
            DeclareBefore(XmlNodeType.Element, StyleSheetLink);
            DeclareFirstChild(XmlNodeType.Element, AtLetter);
            DeclareBefore(XmlNodeType.Element, AtBook);
            DeclareBefore(XmlNodeType.Element, SetEntryLevel);
            DeclareAfter(XmlNodeType.EndElement, CheckSizeAtEntry);
            DeclareBefore(XmlNodeType.Attribute, ReplaceStyleSheetHref);
            DeclareBefore(XmlNodeType.Attribute, ReplaceHyperlinkHref);
            DeclareBefore(XmlNodeType.Attribute, ReplaceHyperlinkId);
        }

        #region Setup folders
        public void CreateOutputFolderAt()
        {
            CreateOutputFolderAt(BaseFolder);
        }

        public string CreateOutputFolderAt(string path)
        {
            BaseFolder = path;
            var outFolder = Path.Combine(path, Folder);
            if (!Directory.Exists(outFolder))
            {
                Directory.CreateDirectory(outFolder);
            }
            return outFolder;
        }

        public string AddCss(string css)
        {
            var outFolder = Path.Combine(BaseFolder, Folder, "css");
            if (!Directory.Exists(outFolder))
            {
                Directory.CreateDirectory(outFolder);
            }
            CssName = Path.GetFileName(css);
            Debug.Assert(CssName != null, "CssName != null");
            var outFullName = Path.Combine(outFolder, CssName);
            File.Copy(css, outFullName, true);
            return outFullName;
        }

        public string AddJs(string js)
        {
            var outFolder = Path.Combine(BaseFolder, Folder, "js");
            if (!Directory.Exists(outFolder))
            {
                Directory.CreateDirectory(outFolder);
            }
            var jsName = Path.GetFileName(js);
            Debug.Assert(jsName != null, "jsName != null");
            var outFullName = Path.Combine(outFolder, jsName);
            File.Copy(js, outFullName, true);
            return outFullName;
        }
        #endregion Setup folders

        #region Break FIles at entry based on size
        private void SetEntryLevel(XmlReader r)
        {
            var myClass = r.GetAttribute("class");
            if (string.IsNullOrEmpty(myClass) || r.Name != "div") return;
            if (!myClass.StartsWith("entry") && !myClass.StartsWith("minorentry") && !myClass.Contains("reversalindexentry") && !myClass.StartsWith("scrSection")) return;
            _entryLevel = r.Depth;
            if (Writing || _letterNum == 0) return;
            var nextFileFullName = NewFileName();
            var nextFileName = Path.GetFileName(nextFileFullName);
            FileList.Add(nextFileName);
            NextWriter(nextFileFullName);
            WriteXmlHeader();
        }

        private void CheckSizeAtEntry(XmlReader r)
        {
            if (r.Depth != _entryLevel) return;
            if (!Writing ) return;
            if (!OneEntry && CurrentSize() < TargetSize) return;
            SkipNode = true;
            Finished();
        }
        #endregion Break FIles at entry based on size

        #region Each letter starts a file
        private void AtLetterHead(XmlReader r)
        {
            if (!Writing || r.GetAttribute("class") != _splitterClass) return;
            SkipNode = true;
            Finished();
        }

        private string _letterLang;
        private string _letter;
        private void AtLetter(XmlReader r)
        {
			var cssClass = r.GetAttribute("class");
            if (cssClass != "letter") return;
			_letterLang = r.GetAttribute("lang");
			_letter = GetString();
			var newFileFullName = NewFileName();
            var newFileName = Path.GetFileName(newFileFullName);
            FileList.Add(newFileName);
            LetterLabels.Add(_letter);
            LetterLinks.Add(newFileName);
            NextWriter(newFileFullName);
            SetTitle();
            WriteXmlHeader();
		    WriteLetterHeader(_letterLang, _letter, r);
		}

        private void AtBook(XmlReader r)
        {
            var cssClass = r.GetAttribute("class");
            if (cssClass != "scrBookName") return;
            _letter = GetString();
            var newFileFullName = NewFileName();
            var newFileName = Path.GetFileName(newFileFullName);
            FileList.Add(newFileName);
            LetterLabels.Add(_letter);
            LetterLinks.Add(newFileName);
            NextWriter(newFileFullName);
            SetTitle();
            WriteXmlHeader();
        }

        private string NewFileName()
        {
            _letterNum += 1;
            return CurrentFileName();
        }

        private string CurrentFileName()
        {
            FileName = Prefix + FileLetter + _letterNum + Extension;
            return FileFullPath = Path.Combine(BaseFolder, Folder, FileName);
        }

        private void SetTitle()
        {
            TitleDefault = _letter;
        }
        #endregion Each letter starts a file

        #region Replace Css link
        private string _replaceHref;
        private void StyleSheetLink(XmlReader r)
        {
            if (r.Name != "link") return;
            var rel = r.GetAttribute("rel");
            if (string.IsNullOrEmpty(rel) || rel != "stylesheet") return;
            if (string.IsNullOrEmpty(CssName)) return;
            _replaceHref = "css/" + CssName;
        }

        private void ReplaceStyleSheetHref(XmlReader r)
        {
            if (r.Name != "href") return;
            if (string.IsNullOrEmpty(_replaceHref)) return;
            ReplaceAttrValue = _replaceHref;
            _replaceHref = null;
        }
        #endregion Replace Css link

        #region Shorten hyperlinks
        private void ReplaceHyperlinkHref(XmlReader r)
        {
            if (r.Name != "href") return;
            if (!r.Value.StartsWith("#") || !r.Value.Contains("-") || r.Value.Length < 20) return;
            ReplaceAttrValue = r.Value.Substring(0, r.Value.IndexOf('-'));
        }

        private void ReplaceHyperlinkId(XmlReader r)
        {
            if (r.Name != "id" && r.Name != "entryguid") return;
            if (!r.Value.Contains("-") || r.Value.Length < 20) return;
            ReplaceAttrValue = r.Value.Substring(0, r.Value.IndexOf('-'));
            if (r.Name != "id") return;
            IdLinks[ReplaceAttrValue] = Path.GetFileName(CurrentFileName());
        }

        #endregion Shorten hyperlinks

    }
}
