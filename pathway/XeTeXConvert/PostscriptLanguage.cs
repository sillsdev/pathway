// --------------------------------------------------------------------------------------------
// <copyright file="PostscriptLanguage.cs" from='2010' to='2010' company='SIL International'>
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
// Manage a list of postscript fonts for XeTeX
// </remarks>
// --------------------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class PostscriptLanguage
    {
        private const int _numFonts = 5;
        protected ArrayList _postscriptNames = new ArrayList(_numFonts);
        protected ArrayList _graphite = new ArrayList(_numFonts);
        protected Dictionary<string, string> _class2postscript = new Dictionary<string, string>();
        protected Dictionary<string, string> _lang2class = new Dictionary<string, string>();
        private const string PartialCachePath = "/PwCtx/miniCTX/texmf-mswin/fonts/cache";

        public PostscriptLanguage()
        {
            const string postscriptName = "CharisSIL";
            const bool isGraphite = true;
            AddPostscriptName(postscriptName, isGraphite);
        }

        public void AccumulatePostscriptNames(Dictionary<string, Dictionary<string, string>> cssProperty)
        {
            foreach (string key in cssProperty.Keys)
            {
                foreach (string property in cssProperty[key].Keys)
                {
                    if (property == "font-family")
                    {
                        string family = cssProperty[key][property];
                        string postscriptName = FontInternals.GetPostscriptName(family, "Regular");
                        SetClass2Postscript(key, postscriptName);
                        if (!_postscriptNames.Contains(postscriptName))
                            AddPostscriptName(postscriptName, FontInternals.IsGraphite(family, "Regular"));
                    }
                }
            }
        }

        public void SetLangClass(string lang, string cssClass)
        {
            _lang2class[lang] = cssClass;
        }

        public string LangXpwFont(string lang)
        {
            if (!_lang2class.ContainsKey(lang))
                return @"\fonta ";
            string cssClass = _lang2class[lang];
            int num = 0;
            if (_class2postscript.ContainsKey(cssClass))
            {
                string postscriptFont = _class2postscript[cssClass];
                num = _postscriptNames.IndexOf(postscriptFont);
            }
            ArrayList XPWfont = new ArrayList {@"\fonta ", @"\fontb ", @"\fontc ", @"\fontd ", @"\fonte "};
            return (string) XPWfont[num < XPWfont.Count? num: 0];
        }

        public string TexFont(int num)
        {
            if (_postscriptNames.Count < num) return "";
            num -= 1;
            return (string)_postscriptNames[num];
        }

        public string IsGraphite(int num)
        {
            if (_postscriptNames.Count < num) return "";
            num -= 1;
            return (bool)_graphite[num] ? "G" : "";
        }

        public void ClassPostscriptName(string className, string style, Dictionary<string, Dictionary<string, string>> cssProperty)
        {
            if (cssProperty.ContainsKey(className) && cssProperty[className].ContainsKey("font-family"))
            {
                string vernacularFamilyName = cssProperty[className]["font-family"];
                string vernacularPostscriptName = FontInternals.GetPostscriptName(vernacularFamilyName, style);
                if (!_postscriptNames.Contains(vernacularPostscriptName))
                {
                    _postscriptNames.Add(vernacularPostscriptName);
                    _graphite.Add(FontInternals.IsGraphite(vernacularFamilyName, style));
                }
            }
        }

        public void SaveCache()
        {
            var sourceDir = Common.PathCombine(Environment.GetEnvironmentVariable("SystemDrive"), PartialCachePath);
            var destinationDir = Common.PathCombine(Common.GetAllUserPath(), "cache/" + GetAllFontNames());
            if (!Directory.Exists(destinationDir))
                FolderTree.Copy(sourceDir,destinationDir);
        }

        public void RestoreCache()
        {
            var sourceDir = Common.PathCombine(Common.GetAllUserPath(), "cache/" + GetAllFontNames());
            if (!Directory.Exists(sourceDir))
                return;
            var destinationDir = Common.PathCombine(Environment.GetEnvironmentVariable("SystemDrive"), PartialCachePath);
            FolderTree.Copy(sourceDir, destinationDir);
        }

        protected string GetAllFontNames()
        {
            var allFontNames = "";
            var delim = "";
            foreach (string name in _postscriptNames)
            {
                allFontNames += delim + name;
                delim = "-";
            }
            return allFontNames;
        }

        #region InternalMethodsForTesting
        public void AddPostscriptName(string postscriptName, bool isGraphite)
        {
            _postscriptNames.Add(postscriptName);
            _graphite.Add(isGraphite);
        }

        public void SetClass2Postscript(string selector, string postscriptName)
        {
            _class2postscript[selector] = postscriptName;
        }
        #endregion

    }
}
