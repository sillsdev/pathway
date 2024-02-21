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
// File: ProcessPseudo.cs (from SimpleCss5.cs)
// Responsibility: Greg Trihus
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;
using SIL.Tool;

namespace CssSimpler
{
    public class ProcessPseudo : XmlCopy
    {
        private readonly Dictionary<string, List<XmlElement>> _beforeTargets = new Dictionary<string, List<XmlElement>>();
        private readonly Dictionary<string, List<XmlElement>> _afterTargets = new Dictionary<string, List<XmlElement>>();
        private const int StackSize = 30;
        private readonly ArrayList _savedSibling = new ArrayList(StackSize);
        private string _lastClass = String.Empty;
        private string _precedingClass = String.Empty;

        public ProcessPseudo(string input, string output, XmlDocument xmlCss)
            : base(input, output, false)
        {
            CollectTargets(xmlCss);
            DeclareBefore(XmlNodeType.Attribute, SaveClass);
			DeclareBefore(XmlNodeType.Element, Program.EntryReporter);
			DeclareBefore(XmlNodeType.Element, InsertLastChildBefore);
			DeclareBefore(XmlNodeType.Element, SaveSibling);
            DeclareBefore(XmlNodeType.Element, InsertBefore);
            DeclareFirstChild(XmlNodeType.Element, InsertFirstChild);
			DeclareAfter(XmlNodeType.EndElement, InsertLastChildAfter);
			DeclareBeforeEnd(XmlNodeType.EndElement, InsertAfter);
            DeclareBeforeEnd(XmlNodeType.EndElement, UnsaveClass);
			DeclareBeforeEnd(XmlNodeType.EndElement, UnsaveLang);
			SpaceClass = null;
            Parse();
        }

		private void InsertBefore(XmlReader r)
        {
            var nextClass = r.GetAttribute("class");
			AddInHierarchy(Langs, r.Depth, r.GetAttribute("lang"));
	        Suffix = "-pb";
			if (ApplyBestRule(r.Depth, nextClass, _beforeTargets, nextClass)) return;
            if (ApplyBestRule(r.Depth, GetTargetKey(r.Name, nextClass), _beforeTargets, nextClass)) return;
            var keyClass = KeyClass(r.Depth);
            if (Classes.Count > r.Depth && ApplyBestRule(r.Depth, GetTargetKey(r.Name, keyClass), _beforeTargets, keyClass)) return;
            var target = GetTargetKey(r.Name, _lastClass);
            ApplyBestRule(r.Depth, target, _beforeTargets, _lastClass);
        }

        private XmlNode _savedFirstNode;
        private string _firstClass = string.Empty;
        private void InsertFirstChild(XmlReader r)
        {
	        if (_savedFirstNode == null) return;
	        InsertContent(_savedFirstNode, _firstClass);
	        _savedFirstNode = null;
	        _firstClass = string.Empty;
        }

		private XmlNode _savedLastNode;
		private string _lastChildClass = string.Empty;
	    private int _lastDepth = 0;
		private void InsertLastChildBefore(XmlReader r)
		{
			InsertLastChildAfter(r);
			_lastDepth = r.Depth;
			ClearLastChild();
		}

	    private void InsertLastChildAfter(XmlReader r)
	    {
		    if (r.Depth == _lastDepth) return;
		    if (_savedLastNode == null) return;
		    InsertContent(_savedLastNode, _lastChildClass);
			ClearLastChild();
		}

		private void ClearLastChild()
		{
			_savedLastNode = null;
			_lastChildClass = string.Empty;
		}

		private void InsertAfter(int depth, string name)
        {
            var index = depth + 1;
            var endClass = Classes.Count > index? Classes[index] as string: null;
			var prevClass = Classes.Count > depth ? Classes[depth] as string : null;
			Suffix = "-pa";
            var target1 = GetTargetKey(name, prevClass);
            if (ApplyBestRule(index, target1, _afterTargets, endClass)) return;
            ApplyBestRule(index, endClass, _afterTargets, endClass);
        }

        private void UnsaveClass(int depth, string name)
        {
	        _precedingClass = RemoveFromHierarchy(depth, Classes);
        }

		private void UnsaveLang(int depth, string name)
		{
			RemoveFromHierarchy(depth, Langs);
		}

		private bool ApplyBestRule(int index, string target, Dictionary<string, List<XmlElement>> targets, string myClass)
        {
            if (target == null) return false;
            foreach (var t in target.Split(' '))
            {
                if (!targets.ContainsKey(t)) continue;
                foreach (var node in targets[t])
                {
                    if (Applies(node, index))
                    {
	                    if (myClass == null)
	                    {
		                    myClass = t.Contains(":") ? t.Split(new[] {':'})[1] : t;
	                    }
                        else if (myClass.Contains(' '))
                        {
                            myClass = myClass.Split(' ')[0];
                        }
                        if (targets == _beforeTargets)
                        {
	                        if (_savedFirstNode != null) continue;
	                        _savedFirstNode = node;
	                        _firstClass = myClass;
                        }
                        else
                        {
	                        if (_lastChild)
	                        {
		                        if (_savedLastNode == null)
		                        {
									_savedLastNode = node;
									_lastChildClass = myClass;
									// We keep scanning because other rules will apply
								}
							}
	                        else
	                        {
								var inserted = InsertContent(node, myClass);
								if (inserted) return true;
							}
						}
                    }
                }
            }
            return false;
        }

	    private bool Applies(XmlNode node, int index)
	    {
		    for (var i = index; i > 0; i--)
		    {
			    if (AppliesAtLevel(node, i)) return true;
		    }
		    return false;
	    }

	    private bool _lastChild;

	    private bool AppliesAtLevel(XmlNode node, int index)
		{
			if (!RequiredFirst(node)) return false;
            if (!RequiredNotFirst(node)) return false;
			_lastChild = false;
            while (node != null && node.Name == "PSEUDO")
            {
	            if (node.FirstChild.InnerText == "last-child") _lastChild = true;
                node = node.PreviousSibling;
            }
            var requireParent = false;
            // We should be at the tag / class for the rule being applied so look before it.
			if (node != null && node.ChildNodes.Count > 1 && node.ChildNodes[1].Name == "ATTRIB")
			{
				node = node.NextSibling;
			}
            while (node != null)
            {
                node = node.PreviousSibling;
                if (node == null) break;
                switch (node.Name)
                {
                    case "PARENTOF":
                        requireParent = true;
                        break;
                    case "CLASS":
                        string name = node.FirstChild.InnerText;
                        while (!requireParent && index > 0 && !MatchClass(index, name))
                        {
                            index -= 1;
                        }
                        requireParent = false;
                        if (!MatchClass(index, name)) return false;
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
                        string tagName = node.FirstChild.InnerText;
                        if (tagName != "span" && tagName != "a")
                            throw new NotImplementedException();
						if (!CheckAttrib(node, index)) return false;
						break;
                    default:
                        throw new NotImplementedException();
                }
            }
            return true;

        }

		private bool CheckAttrib(XmlNode node, int index)
		{
			if (node.ChildNodes.Count <= 1) return true;
			var attrNode = node.ChildNodes[1];
			if (attrNode.Name == "ATTRIB")
			{
				if (!AttribEval(index, attrNode)) return false;
			}
			else
			{
				throw new NotImplementedException("non-ATTRIB modifier");
			}
			return true;
		}

		private static readonly char[] Quotes = { '\'', '"' };
		private bool AttribEval(int index, XmlNode attrNode)
		{
			var attrName = attrNode.FirstChild.InnerText;
			if (attrName != "lang") throw new NotImplementedException("Attributes other than lang not implemented");
			var actualVal = (Langs.Count > index)? Langs[index] as string: null;
			var attrOp = attrNode.ChildNodes[1].Name;
			switch (attrOp)
			{
				case "BEGINSWITH":
				case "ATTRIBEQUAL":
					var expVal = attrNode.LastChild.InnerText.Trim(Quotes);
					if (actualVal != expVal) return false;
					break;
				default:
					throw new NotImplementedException("Attribute operator not equal or begins with");
			}
			return true;
		}

		private bool MatchClass(int index, string name)
        {
            if (index >= Classes.Count) return false;
            var classNames = Classes[index] as string;
            if (classNames == null) return false;
            return classNames.Split(' ').Contains(name);
        }

        private bool RequiredFirst(XmlNode node)
        {
            var reqFirstChild = node.SelectSingleNode("parent::*//PSEUDO[name='first-child' and not(parent::PSEUDO)]");
            return reqFirstChild == null || _firstSibling;
        }

		private bool RequiredNotFirst(XmlNode node)
        {
            var reqNotFirstChild = LookupNotFirstChild(node);
            return reqNotFirstChild == null || !_firstSibling;
        }

        private static XmlNode LookupNotFirstChild(XmlNode node)
        {
            return node.SelectSingleNode("parent::*//PSEUDO[name='first-child' and parent::PSEUDO]");
        }

        private bool InsertContent(XmlNode node, string myClass)
        {
            var content = node.SelectSingleNode("following-sibling::PROPERTY[name='content']/value");
	        if (content == null) return false;	/* previously some flex or Pathway rules might set font for before after content w/o the content */
            var val = content.InnerText.Replace(@"\'", @"'");
            var properties = node.SelectNodes("parent::*/PROPERTY");
            Debug.Assert(properties != null);
            myClass = properties.Count <= 1 ? null : myClass.Replace(" ", "");
            WriteContent(val.Substring(1, val.Length - 2), myClass);  // Remove quotes
	        return true;
        }

        private void SaveClass(XmlReader r)
        {
	        if (r.Name != "class") return;
			AddInHierarchy(r, Classes);
        }

		private int _nextFirst = -1;
        private bool _firstSibling;
        private void SaveSibling(XmlReader r)
        {
            _firstSibling = r.Depth == _nextFirst;
			if (_firstSibling)
			{
				_savedSibling.Clear();
			}
			var myClass = r.GetAttribute("class");
            if (!string.IsNullOrEmpty(myClass))
            {
                _savedSibling.Add(myClass);
            }
            _nextFirst = r.Depth + 1;
        }

        private void CollectTargets(XmlDocument xmlCss)
        {
            Debug.Assert(xmlCss != null, "xmlCss != null");
            GetPseudoRuleTargets(xmlCss, "before", _beforeTargets);
            GetPseudoRuleTargets(xmlCss, "after", _afterTargets);
        }

        private static void GetPseudoRuleTargets(XmlDocument xmlCss, string kind, Dictionary<string, List<XmlElement>> targets)
        {
            var pseudos = xmlCss.SelectNodes(string.Format("//RULE/PSEUDO[name = '{0}']", kind));
            Debug.Assert(pseudos != null, "pseudos != null");
            foreach (XmlElement pseudo in pseudos)
            {
                var rule = pseudo.ParentNode as XmlElement;
                Debug.Assert(rule != null, "rule is null");
                var target = GetTargetKey(rule.GetAttribute("target"), rule.GetAttribute("lastClass"));
                if (!targets.ContainsKey(target))
                {
                    targets[target] = new List<XmlElement> {pseudo};
                }
                else
                {
                    // insert pseduo node so rules are process in order of priority
                    var index = 0;
                    var added = false;
                    foreach (var term in targets[target])
                    {
                        var targetRule = term.ParentNode as XmlElement;
                        Debug.Assert(targetRule != null, "targetRule != null");
                        var ruleTerms = int.Parse(targetRule.GetAttribute("term"));
                        // ReSharper disable once TryCastAlwaysSucceeds
                        var curRule = pseudo.ParentNode as XmlElement;
                        var curTerms = int.Parse(curRule.GetAttribute("term"));
                        if (curTerms >= ruleTerms)
                        {
                            targets[target].Insert(index, pseudo);
                            added = true;
                            break;
                        }
                        index++;
                    }
                    if (!added)
                    {
                        targets[target].Add(pseudo);
                    }
                }
            }
        }

        private static string GetTargetKey(string target, string lastClass)
        {
	        if (target != "span" && target != "xitem") return target;
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
