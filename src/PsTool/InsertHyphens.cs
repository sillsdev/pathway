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
// File: InsertHyphens.cs
// Responsibility: Greg Trihus
// ---------------------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using SIL.Extensions;

namespace SIL.Tool
{
    public class InsertHyphens : XmlCopy
    {
	    private readonly Dictionary<string, string> _hyphenatedWords;
	    private readonly string _lang;
	    private Regex _wordPattern;

	    public InsertHyphens(string input, string output, string langCodeName, Dictionary<string,string>hyphenatedWords)
            : base(input, output, false)
        {
	        _lang = langCodeName.Split(':')[0];
	        _hyphenatedWords = hyphenatedWords;
			SetWordPattern();
	        DeclareBefore(XmlNodeType.Attribute, SaveLang);
			DeclareBefore(XmlNodeType.Text, TextNode);
			DeclareAfter(XmlNodeType.Text, StopSkipping);
			Langs.RemoveRange(0, Langs.Count);
		}

	    private void SetWordPattern()
	    {
		    SortedSet<char> letterSet = new SortedSet<char>();
		    foreach (var w in _hyphenatedWords.Keys)
		    {
				letterSet.AddRange(w.ToCharArray());
			}
		    var pattern = string.Empty;
		    var before = "[";
		    const string except = @"[]|\";
		    const string spaces = " \t\r\n";
		    foreach (char c in letterSet)
		    {
			    var s = c.ToString();
			    if (spaces.Contains(s)) continue;
			    if (except.Contains(s))
			    {
				    pattern += before + @"\" + c;
			    }
			    else
			    {
				    pattern += before + c;
			    }
			    before = "|";
		    }
		    pattern += "]+";
		    _wordPattern = new Regex(pattern, RegexOptions.Compiled);
	    }

	    private void SaveLang(XmlReader r)
	    {
		    if (r.Name != "lang") return;
			AddInHierarchy(r, Langs);
	    }

		private void TextNode(XmlReader r)
		{
			if (GetLang(r) != _lang) return;
			var val = r.Value;
			var matches = _wordPattern.Matches(val);
			var result = string.Empty;
			var i = 0;
			foreach (Match match in matches)
			{
				if (match.Index > i)
				{
					result += val.Substring(i, match.Index - i);
					i = match.Index;
				}
				if (_hyphenatedWords.ContainsKey(match.Value))
				{
					result += _hyphenatedWords[match.Value];
				}
				else
				{
					result += match.Value;
				}
				i += match.Length;
			}
			if (i < val.Length)
			{
				result += val.Substring(i);
			}
			WriteTextValue(result);
			SkipNode = true;
		}

		private void StopSkipping(XmlReader r)
		{
			SkipNode = false;
		}
	}
}
