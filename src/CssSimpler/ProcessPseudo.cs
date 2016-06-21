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

namespace CssSimpler
{
    public class ProcessPseudo : XmlCopy
    {
        private readonly Dictionary<string, List<XmlElement>> _beforeTargets = new Dictionary<string, List<XmlElement>>();
        private readonly Dictionary<string, List<XmlElement>> _afterTargets = new Dictionary<string, List<XmlElement>>();
        private const int StackSize = 30;
        private readonly ArrayList _classes = new ArrayList(StackSize);
        private readonly ArrayList _savedLastClass = new ArrayList(StackSize);
        private readonly ArrayList _savedSibling = new ArrayList(StackSize);
        private string _lastClass = String.Empty;
        private readonly SortedSet<string> _needHigher;

        public ProcessPseudo(string input, string output, XmlDocument xmlCss, SortedSet<string> needHigher)
            : base(input, output)
        {
            _needHigher = needHigher;
            CollectTargets(xmlCss);
            DeclareBefore(XmlNodeType.Attribute, SaveClass);
            DeclareBefore(XmlNodeType.Element, SaveSibling);
            DeclareBefore(XmlNodeType.Element, InsertBefore);
            DeclareBeforeEnd(XmlNodeType.EndElement, InsertAfter);
            DeclareBeforeEnd(XmlNodeType.EndElement, UnsaveClass);
            Parse();
        }

        private void InsertBefore(XmlReader r)
        {
            var nextClass = r.GetAttribute("class");
            if (ApplyBestRule(r.Depth + 1, nextClass, _beforeTargets, nextClass)) return;
            var keyClass = KeyClass(r.Depth);
            if (_classes.Count > r.Depth && ApplyBestRule(r.Depth, GetTargetKey(r.Name, keyClass), _beforeTargets, keyClass)) return;
            var target = GetTargetKey(r.Name, _lastClass);
            ApplyBestRule(r.Depth, target, _beforeTargets, _lastClass);
        }

        private void InsertAfter(int depth, string name)
        {
            var index = depth + 1;
            if (index >= _savedLastClass.Count) return;
            var endClass = _savedLastClass[index] as string;
            if (endClass == null) return;
            var target1 = GetTargetKey(_classes[index] as string, endClass);
            var target2 = GetTargetKey(name, endClass);
            if (ApplyBestRule(depth, target1, _afterTargets, endClass)) return;
            if (ApplyBestRule(index, target2, _afterTargets, endClass)) return;
            ApplyBestRule(index, endClass, _afterTargets, endClass);
        }

        private void UnsaveClass(int depth, string name)
        {
            var index = depth + 1;
            if (index >= _savedLastClass.Count) return;
            _savedLastClass[index] = null;
            _classes[index] = null;
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
                        InsertContent(node, myClass);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool Applies(XmlNode node, int index)
        {
            if (!RequiredFirst(node)) return false;
            while (node != null && node.Name == "PSEUDO")
            {
                node = node.PreviousSibling;
            }
            var requireParent = false;
            // We should be at the tag / class for the rule being applied so look before it.
            while (node != null)
            { 
                node = node.PreviousSibling;
                if (node == null) break;
                switch (node.Name)
                {
                    case "PARENTOF":
                        requireParent = true;
                        continue;
                    case "CLASS":
                        string name = node.ChildNodes[0].InnerText;
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
                        string precedingName = node.ChildNodes[0].InnerText;
                        if (_classes[index] as string != precedingName && precedingName != "span") return false;
                        if (precedingName != "span") index -= 1;
                        break;
                    case "SIBLING":
                        node = node.PreviousSibling;
                        Debug.Assert(node != null, "Nothing preceding SIBLING");
                        string siblingName = node.ChildNodes[0].InnerText;
                        int position = _savedSibling.IndexOf(siblingName);
                        if (position == -1 || position == _savedSibling.Count - 1) return false;
                        index -= 1;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            return true;

        }

        private bool MatchClass(int index, string name)
        {
            if (index >= _classes.Count) return false;
            var classNames = _classes[index] as string;
            if (classNames == null) return false;
            if (!classNames.Split(' ').Contains(name)) return false;
            return true;
        }

        private bool RequiredFirst(XmlNode node)
        {
            var firstChild = node.SelectSingleNode("parent::*//PSEUDO[name='first-child']");
            return firstChild == null || _firstSibling;
        }

        private void InsertContent(XmlNode node, string myClass)
        {
            var content = node.SelectSingleNode("following-sibling::PROPERTY[name='content']/value");
            Debug.Assert(content != null);
            var val = content.InnerText;
            WriteContent(val.Substring(1,val.Length - 2), myClass.Replace(" ", ""));  // Remove quotes
        }

        private void SaveClass(XmlReader r)
        {
            if (r.Name == "class")
            {
                if (!_needHigher.Contains(r.Value))
                {
                    _lastClass = r.Value;
                }
                if (r.Depth >= _classes.Count)
                {
                    while (_classes.Count < r.Depth)
                    {
                        _savedLastClass.Add(null);
                        _classes.Add(null);
                    }
                    _savedLastClass.Add(_lastClass);
                    _classes.Add(r.Value);
                }
                else
                {
                    _savedLastClass[r.Depth] = _lastClass;
                    _classes[r.Depth] = r.Value;
                }
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
            if (_classes.Count <= depth) return null;
            while (depth > 0)
            {
                var proposedClass = _classes[depth] as string;
                if (proposedClass != null && !_needHigher.Contains(proposedClass))
                    return proposedClass;
                depth -= 1;
            }
            return null;
        }

    }
}
