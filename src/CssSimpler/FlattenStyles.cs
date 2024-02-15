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
// File: FlattenStyles.cs
// Responsibility: Greg Trihus
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using SIL.Tool;

namespace CssSimpler
{
    public class FlattenStyles : XmlCopy
    {
        public int Structure;
        public bool DivBlocks;
        private readonly Dictionary<string, List<XmlElement>> _ruleIndex = new Dictionary<string, List<XmlElement>>();
        private readonly List<int> _displayBlockRuleNum = new List<int>();
        private const int StackSize = 30;
        private readonly ArrayList _levelRules = new ArrayList(StackSize);
        private readonly ArrayList _savedSibling = new ArrayList(StackSize);
        //private string _lastClass = String.Empty;
        private string _precedingClass = String.Empty;
        private string _headerTag;
	    private bool _stylesheetPresent;

        public FlattenStyles(string input, string output, XmlDocument xmlCss, bool noXmlHeader, string decorateStyles)
            : base(input, output, noXmlHeader)
        {
			StyleDecorate = decorateStyles;
            MakeRuleIndex(xmlCss);
            IdentifyDisplayBlockRules(xmlCss);
            _xmlCss = xmlCss;
            Suffix = string.Empty;
	        _stylesheetPresent = false;
            DeclareBefore(XmlNodeType.Attribute, SaveClassLangDir);
			DeclareBefore(XmlNodeType.Element, Program.EntryReporter);
			DeclareBefore(XmlNodeType.Element, SaveSibling);
            DeclareBefore(XmlNodeType.Element, InsertBefore);
			DeclareBefore(XmlNodeType.Element, RemoveExtraStylesheets);
            DeclareBefore(XmlNodeType.EndElement, SetForEnd);
            DeclareBefore(XmlNodeType.Text, TextNode);
            DeclareBefore(XmlNodeType.EntityReference, OtherNode);
            DeclareBefore(XmlNodeType.Whitespace, OtherNode);
            DeclareBefore(XmlNodeType.SignificantWhitespace, OtherNode);
            DeclareBefore(XmlNodeType.CDATA, OtherNode);
            DeclareBeforeEnd(XmlNodeType.EndElement, DivEnds);
            DeclareBeforeEnd(XmlNodeType.EndElement, UnsaveClass);
			DeclareBeforeEnd(XmlNodeType.EndElement, UnsaveLang);
			SpaceClass = "sp";
			Classes.RemoveRange(0, Classes.Count);
			Langs.RemoveRange(0, Langs.Count);
        }

	    private void RemoveExtraStylesheets(XmlReader r)
	    {
		    if (r.Name != "link") return;
		    if (r.GetAttribute("rel") != "stylesheet") return;
		    if (_stylesheetPresent)
		    {
			    SkipNode = true;
		    }
		    else
		    {
			    _stylesheetPresent = true;
		    }
	    }

	    private void IdentifyDisplayBlockRules(XmlDocument xmlCss)
        {
            var dispBlockNodes = xmlCss.SelectNodes("//*[name='display' and value='block']/parent::*/@pos");
            Debug.Assert(dispBlockNodes != null, "dispBlockNodes != null");
            foreach (XmlAttribute pos in dispBlockNodes)
            {
                _displayBlockRuleNum.Add(int.Parse(pos.InnerText));
            }
        }

        private void InsertBefore(XmlReader r)
        {
            var nextClass = r.GetAttribute("class");
            SkipNode = r.Name == "span";
			CollectRules(r, GetRuleKey(r.Name, nextClass));
	        for (var i = r.Depth; i >= 0; i--)
	        {
				CollectRules(r, GetRuleKey(r.Name, KeyClass(i)));
			}
			CollectRules(r, nextClass);
            CollectRules(r, GetRuleKey(r.Name, ""));
            SkipNode = IsNotBlockRule(r.Depth);
            if (!SkipNode)
            {
                var outClass = GetStyle(r);
	            if (nextClass != null && nextClass != outClass)
	            {
					ReplaceClass = outClass;
				}
				if (r.Name == "span" && DivBlocks)
                {
                    ReplaceLocalName = "div";
                }
            }
        }

        private bool IsNotBlockRule(int depth)
        {
            _ruleNums.Clear();
            if (SkipNode && GetLevelRules(depth))
            {
                foreach (var num in _ruleNums)
                {
                    if (!_displayBlockRuleNum.Contains(num)) continue;
                    return false;
                }
            }
            return SkipNode;
        }

        private void SetForEnd(XmlReader r)
        {
            SkipNode = r.Name == "span";
        }

        private void DivEnds(int depth, string name)
        {
	        if (name != "link")
	        {
				SkipNode = name == "span";
			}
			SkipNode = IsNotBlockRule(depth);
            if (_levelRules.Count > depth)
            {
                _levelRules[depth] = null;
            }
        }

        private void UnsaveClass(int depth, string name)
        {
	        _precedingClass = RemoveFromHierarchy(depth, Classes);
        }

		private void UnsaveLang(int depth, string name)
		{
			RemoveFromHierarchy(depth, Langs);
		}

		private void CollectRules(XmlReader r, string target)
        {
            if (target == null) return;
            var dirty = false;
            var index = r.Depth;
            var found = _levelRules.Count > index && _levelRules[index] != null ? (List<XmlElement>)_levelRules[index] : new List<XmlElement>();
            foreach (var t in target.Split(' '))
            {
                var targets = _ruleIndex;
                if (!targets.ContainsKey(t)) continue;
                foreach (var node in targets[t])
                {
                    if (!Applies(node, r)) continue;
	                AddInTermOrder(found, node);
                    dirty = true;
                }
            }
            if (dirty)
            {
                AddInHierarchy(_levelRules, index, found);
            }
        }

	    private static void AddInTermOrder(IList<XmlElement> found, XmlElement node)
	    {
		    var rule = GetRule(node);
		    var terms = GetTerms(rule);
		    var added = false;
		    for (var i = found.Count; i-- > 0; )
		    {
			    if (GetTerms(GetRule(found[i])) < terms) continue;
			    if (i + 1 == found.Count)
			    {
				    found.Add(node);
			    }
			    else
			    {
				    found.Insert(i + 1, node);
			    }
			    added = true;
			    break;
		    }
		    if (!added)
		    {
			    found.Insert(0, node);
		    }

	    }

		private bool Applies(XmlNode node, XmlReader r)
		{
			var index = r.Depth;
			while (node != null && node.Name == "PROPERTY")
			{
				node = node.PreviousSibling;
			}
			// We should be at the tag / class for the rule being applied so look before it.
			if (node != null && node.ChildNodes.Count == 1)
			{
				node = node.PreviousSibling;
			}
			_matchIndex.Clear();
			_matchNode.Clear();
			while (true)
			{
				if (ApplyBacktrack(node, r, index)) return true;
				if (_matchIndex.Count == 0) break;
				index = _matchIndex.Pop() - 1;
				node = _matchNode.Pop();
			}
			return false;
		}

		private readonly Stack<int> _matchIndex = new Stack<int>();
		private readonly Stack<XmlNode> _matchNode = new Stack<XmlNode>();

		private bool ApplyBacktrack(XmlNode node, XmlReader r, int index)
		{
			var requireParent = false;
			while (node != null)
            {
                switch (node.Name)
                {
                    case "PARENTOF":
                        requireParent = true;
                        break;
                    case "CLASS":
                        if (node.ChildNodes.Count > 1)
                        {
                            if (!CheckAttrib(node, r)) return false;
                            break;
                        }
                        string name = node.FirstChild.InnerText;
                        while (!requireParent && index > 0 && !MatchClass(index, name))
                        {
                            index -= 1;
                        }
                        if (!MatchClass(index, name)) return false;
		                if (!requireParent)
		                {
							_matchIndex.Push(index);
							_matchNode.Push(node);
						}
						requireParent = false;
						index -= 1;
                        break;
                    case "PRECEDES":
                        if (_firstSibling) return false;
                        node = node.PreviousSibling;
                        Debug.Assert(node != null, "Nothing preceding PRECEDES");
                        string precedingName = node.FirstChild.InnerText;
                        if (_precedingClass != precedingName && precedingName != "span") return false;
                        break;
                    case "SIBLING":
                        node = node.PreviousSibling;
                        Debug.Assert(node != null, "Nothing preceding SIBLING");
                        string siblingName = node.FirstChild.InnerText;
                        int position = _savedSibling.IndexOf(siblingName);
                        if (position == -1 || position == _savedSibling.Count - 1) return false;
                        break;
                    case "TAG":
                        if (!CheckAttrib(node, r)) return false;
                        break;
                    default:
                        throw new NotImplementedException();
                }
                node = node.PreviousSibling;
            }
            return true;

        }

        private static bool CheckAttrib(XmlNode node, XmlReader r)
        {
            if (node.ChildNodes.Count <= 1) return true;
            var attrNode = node.ChildNodes[1];
            if (attrNode.Name == "ATTRIB")
            {
                if (!AttribEval(r, attrNode)) return false;
            }
            else
            {
                throw new NotImplementedException("non-ATTRIB modifier");
            }
            return true;
        }

        private static readonly char[] Quotes = {'\'', '"'};
        private static bool AttribEval(XmlReader r, XmlNode attrNode)
        {
            var attrName = attrNode.FirstChild.InnerText;
            var actualVal = r.GetAttribute(attrName);
            var attrOp = attrNode.ChildNodes[1].Name;
            switch (attrOp)
            {
                case "BEGINSWITH":
                case "ATTRIBEQUAL":
                    var expVal = attrNode.LastChild.InnerText.Trim(Quotes);
                    if (actualVal != expVal) return false;
                    break;
                default:
                    throw new NotImplementedException();
            }
            return true;
        }

        private bool MatchClass(int index, string name)
        {
            if (index >= Classes.Count) return false;
            var classNames = Classes[index] as string;
            if (classNames == null) return false;
            if (!classNames.Split(' ').Contains(name)) return false;
            return true;
        }

        private void TextNode(XmlReader r)
        {
            ReplaceLocalName = _headerTag;
            _headerTag = null;
            WriteContent(r.Value, GetStyle(r, 1), GetLang(r), false);
            SkipNode = true;
        }

        public readonly Dictionary<string, string> RuleStyleMap = new Dictionary<string, string>();
		public readonly Dictionary<string, string> RuleProps = new Dictionary<string, string>();
		public readonly Dictionary<string, string> FirstRuleKey = new Dictionary<string, string>();
		private readonly Dictionary<string, int> _usedStyles = new Dictionary<string, int>();

        // ReSharper disable once UnusedMethodReturnValue.Local
        private string GetStyle(XmlReader r)
        {
            return GetStyle(r, 0);
        }

		private readonly SortedSet<string> _blockTags = new SortedSet<string> {"div", "body"};

		private string GetStyle(XmlReader r, int adjustLevel)
        {
            var myClass = GetClass(r);
            if (myClass == null) return null;
            if (myClass.Contains(" "))
            {
                myClass = myClass.Split(' ')[0];
            }
			//if (myClass == "headword")
			//{
			//	Debug.Print("break;");
			//}
			var ruleNums = GetRuleNumbers(r, adjustLevel);
	        var block = _blockTags.Contains(r.Name) || HasBlock(ruleNums);
            var key = myClass + ":" + (block? "B":"S") + ruleNums;
            if (_usedStyles.ContainsKey(myClass))
            {
                if (RuleStyleMap.ContainsKey(key))
                {
                    myClass = RuleStyleMap[key];
                }
                else
                {
					if (!RuleProps.ContainsKey(myClass))
					{
						var firstRuleNums = FirstRuleKey[myClass];
						var firstProperties = GetFlatProperties(firstRuleNums, null, block);
						RuleProps[myClass] = string.Join("", firstProperties);
					}
					var properties = string.Join("", GetFlatProperties(ruleNums, null, block));
		            if (RuleProps[myClass] == properties)
		            {
			            RuleStyleMap[key] = myClass;
			            return myClass;
		            }
					for (var n = 2; n <= _usedStyles[myClass]; n += 1)
					{
						var testClass = myClass + n;
						if (RuleProps[testClass] != properties) continue;
						RuleStyleMap[key] = testClass;
						return testClass;
					}
					_usedStyles[myClass] += 1;
	                myClass += _usedStyles[myClass];
	                RuleStyleMap[key] = myClass;
	                RuleProps[myClass] = properties;
                }
            }
            else
            {
                _usedStyles[myClass] = 1;
                RuleStyleMap[key] = myClass;
	            FirstRuleKey[myClass] = ruleNums;
            }
            return myClass;
        }

	    private bool HasBlock(string ruleNums)
	    {
		    return ruleNums.Split(',').Any(rule => _displayBlockRuleNum.Contains(int.Parse(rule)));
	    }

	    private readonly List<int> _ruleNums = new List<int>();
        private string GetRuleNumbers(XmlReader r, int adjustLevel)
        {
            _ruleNums.Clear();
			GetLevelRules(r.Depth - adjustLevel);
			_ruleNums.Add(-1);
	        MergeLevelRules(r.Depth - adjustLevel - 1);
            return string.Join(",", _ruleNums);
        }

	    private bool GetLevelRules(int i)
        {
            if (_levelRules.Count <= i) return false;
            var levelList = _levelRules[i] as List<XmlElement>;
            if (levelList == null) return false;
            foreach (XmlElement node in levelList)
            {
	            var pos = GetPos(GetRule(node));
                if (_ruleNums.Contains(pos)) continue;
                _ruleNums.Add(pos);
            }
	        return true;
        }

		private void MergeLevelRules(int level)
		{
			var rulePos = new List<int>();
			var ruleTerms = new List<int>();
			for (var i = level; i >= 0; i -= 1)
			{
				if (_levelRules.Count <= i) continue;
				var levelList = _levelRules[i] as List<XmlElement>;
				if (levelList == null) continue;
				foreach (XmlElement node in levelList)
				{
					var rule = GetRule(node);
					var pos = GetPos(rule);
					if (rulePos.Contains(pos)) continue;
					var terms = GetTerms(rule);
					var addAt = 0;
					for (var j = ruleTerms.Count; --j >= 0;)
					{
						if (terms > ruleTerms[j]) continue;
						addAt = j + 1;
						break;
					}
					if (addAt >= ruleTerms.Count)
					{
						ruleTerms.Add(terms);
						rulePos.Add(pos);
					}
					else
					{
						ruleTerms.Insert(addAt, pos);
						rulePos.Insert(addAt, pos);
					}
				}
			}
			_ruleNums.AddRange(rulePos);
		}

		private readonly SortedDictionary<string, string> _reverseMap = new SortedDictionary<string, string>();
        private readonly XmlDocument _flatCss = new XmlDocument();
        private readonly XmlDocument _xmlCss;
        private readonly SortedSet<string> _notInherted = new SortedSet<string> {"column", "float", "clear", "width", "margin", "padding", "display", "border"};
        public XmlDocument MakeFlatCss()
        {
            _flatCss.RemoveAll();
            _flatCss.LoadXml("<ROOT/>");
            CopyNonStyleNodes();
            foreach (var key in RuleStyleMap.Keys)
            {
                _reverseMap[RuleStyleMap[key]] = key;
            }
            var pos = 1;
            foreach (var style in _reverseMap.Keys)
            {
                //if (style == "translation-st")
                //{
                //    Debug.Print("break;");
                //}
                var ruleNode = _flatCss.CreateElement("RULE");
                var classNode = _flatCss.CreateElement("CLASS");
                var classNameNode = _flatCss.CreateElement("name");
                classNameNode.InnerText = DecorateExceptions.Contains(style) ? style: style + StyleDecorate;
                classNode.AppendChild(classNameNode);
                ruleNode.AppendChild(classNode);
                Debug.Assert(_flatCss.DocumentElement != null, "_flatCss.DocumentElement != null");
                _flatCss.DocumentElement.AppendChild(ruleNode);
                ruleNode.SetAttribute("pos", pos.ToString());
                var activeRules = _reverseMap[style];
	            var ruleIndex = activeRules.IndexOf(":", StringComparison.Ordinal);
	            var blockRule = activeRules.Substring(ruleIndex + 1, 1) == "B";
				var ruleListText = activeRules.Substring(ruleIndex + 2);
                var commentNode = _flatCss.CreateElement("COMMENT");
                var commentValueNode = _flatCss.CreateElement("value");
                commentValueNode.InnerText = "Rule List: " + ruleListText;
                commentNode.AppendChild(commentValueNode);
                ruleNode.AppendChild(commentNode);
                GetFlatProperties(ruleListText, ruleNode, blockRule);
            }
            return _flatCss;
        }

		private SortedSet<string> GetFlatProperties(string ruleListText, XmlElement ruleNode, bool blockRule)
	    {
			var incProps = new SortedSet<string>();
			var formatProperties = new SortedSet<string>();
			var inherited = false;
		    foreach (Match m in Regex.Matches(ruleListText, @"[\d-]+"))
		    {
			    if (m.Value == "-1")
			    {
				    inherited = true;
				    continue;
			    }
			    var pattern = string.Format("//*[@pos='{0}']/PROPERTY", m.Value);
			    var propNodes = _xmlCss.SelectNodes(pattern);
			    if (propNodes == null) continue;
			    foreach (XmlElement node in propNodes)
			    {
				    Debug.Assert(node != null, "node != null");
				    var propValueNode = node.SelectSingleNode(".//value");
				    if (propValueNode != null && propValueNode.InnerText == "inherit") continue;
				    var propNameNode = node.SelectSingleNode(".//name");
				    if (propNameNode == null) continue;
				    var name = propNameNode.InnerText;
				    if (incProps.Contains(name)) continue;
				    var prefixMatch = Regex.Match(name, @"[a-z]+");
				    if (inherited && (_notInherted.Contains(prefixMatch.Value) || name.StartsWith("-"))) continue;
				    if (!blockRule && BlockProperty(name)) continue;
				    incProps.Add(name);
				    if (propValueNode != null) formatProperties.Add(string.Format("{0}:{1};", name, propValueNode.InnerText));
				    ruleNode?.AppendChild(_flatCss.ImportNode(node, true));
			    }
		    }
		    return formatProperties;
	    }

		private readonly SortedSet<string> _blockProperties = new SortedSet<string> { "text-align", "text-indent", "column", "float", "clear", "width", "margin", "padding", "display", "border", "line" };

		private bool BlockProperty(string name)
		{
			return _blockProperties.Any(blockProperty => name.Contains(blockProperty));
		}

	    private void CopyNonStyleNodes()
	    {
			CopyMatchedNodes("//*[@term='1' and count(TAG) = 1]");
			CopyMatchedNodes("//*[ANY]");
	    }

	    private void CopyMatchedNodes(string pattern)
        {
            var nodes = _xmlCss.SelectNodes(pattern);
	        if (nodes == null) return;
            foreach (XmlElement node in nodes)
            {
                Debug.Assert(_flatCss.DocumentElement != null, "_flatCss.DocumentElement != null");
                _flatCss.DocumentElement.AppendChild(_flatCss.ImportNode(node, true));
            }
        }

        private void OtherNode(XmlReader r)
        {
            SkipNode = false;
        }

        private void SaveClassLangDir(XmlReader r)
        {
            if (r.Name == "class")
            {
                AddInHierarchy(r, Classes);
                switch (r.Value)
                {
                    case "entry":
                        if (Structure >= 3)
                        {
                            _headerTag = "h3";
                        }
                        break;
                    case "letter":
                        if (Structure >= 2)
                        {
                            _headerTag = "h2";
                        }
                        break;
                }
            }
            else if (r.Name == "lang")
            {
                AddInHierarchy(r, Langs);
            }
			else if (r.Name == "dir")
            {
	            Dir = r.Value;
            }
		}

		private int _nextFirst = -1;
        private bool _firstSibling;

        private void SaveSibling(XmlReader r)
        {
            _firstSibling = r.Depth == _nextFirst;
            var myClass = r.GetAttribute("class");
            if (!string.IsNullOrEmpty(myClass))
            {
                if (_firstSibling)
                {
                    _savedSibling.Clear();
                }
                _savedSibling.Add(myClass);
            }
            _nextFirst = r.Depth + 1;
        }

        private void MakeRuleIndex(XmlDocument xmlCss)
        {
            Debug.Assert(xmlCss != null, "xmlCss != null");
            var targets = _ruleIndex;
            var styleRules = xmlCss.SelectNodes("//RULE/PROPERTY[1]");
            Debug.Assert(styleRules != null, "styleRules != null");
            foreach (XmlElement styleRule in styleRules)
            {
                var rule = styleRule.ParentNode as XmlElement;
                Debug.Assert(rule != null, "rule is null");
                var target = GetRuleKey(rule.GetAttribute("target"), rule.GetAttribute("lastClass"));
                if (!targets.ContainsKey(target))
                {
                    targets[target] = new List<XmlElement> {styleRule};
                }
                else
                {
                    // insert pseduo node so rules are process in order of priority
                    var index = 0;
                    var added = false;
                    foreach (var term in targets[target])
                    {
                        var targetRule = GetRule(term);
	                    var ruleTerms = GetTerms(targetRule);
                        // ReSharper disable once TryCastAlwaysSucceeds
                        var curRule = GetRule(styleRule);
                        var curTerms = GetTerms(curRule);
                        if (curTerms >= ruleTerms)
                        {
                            targets[target].Insert(index, styleRule);
                            added = true;
                            break;
                        }
                        index++;
                    }
                    if (!added)
                    {
                        targets[target].Add(styleRule);
                    }
                }
            }
        }

		private static int GetPos(XmlElement targetRule)
		{
			return int.Parse(targetRule.GetAttribute("pos"));
		}

		private static int GetTerms(XmlElement targetRule)
	    {
		    var ruleTerms = int.Parse(targetRule.GetAttribute("term"));
		    return ruleTerms;
	    }

	    private static XmlElement GetRule(XmlElement term)
	    {
		    var targetRule = term.ParentNode as XmlElement;
		    Debug.Assert(targetRule != null, "targetRule != null");
		    return targetRule;
	    }

	    private static string GetRuleKey(string target, string lastClass)
        {
            if (target == "span" || target == "xitem")
            {
                if (lastClass == null) return null;
                if (!lastClass.Contains(" "))
                {
                    target = string.Format("{0}:{1}", target, lastClass);
                }
                else
                {
                    var sb = new StringBuilder();
                    var first = true;
                    foreach (var s in lastClass.Split(' '))
                    {
                        if (!first)
                        {
                            sb.Append(" ");
                        }
                        else
                        {
                            first = false;
                        }
                        sb.Append(string.Format("{0}:{1}", target, s));
                    }
                    target = sb.ToString();
                }
            }
            return target;
        }

        private string KeyClass(int depth)
        {
            if (Classes.Count <= depth) return null;
            while (depth > 0)
            {
                var proposedClass = Classes[depth] as string;
                if (proposedClass != null)
                    return proposedClass;
                depth -= 1;
            }
            return null;
        }
    }
}
