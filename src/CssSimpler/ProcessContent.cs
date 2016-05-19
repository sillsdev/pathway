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
// File: ProcessContent.cs (from SimpleCss5.cs)
// Responsibility: Greg Trihus
// ---------------------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;

namespace CssSimpler
{
    public class ProcessContent : XmlCopy
    {
        private Stack<string> elem = new Stack<string>();
        private Stack<string> clas = new Stack<string>();
        private readonly Dictionary<string, List<XmlNode>> _contClass;
        private string _lastClass;
        private string _precedingTag;
        private string _precedingClass;
        private int _precedingDepth;
        private string _currentTag;
        private string _currentClass;
        private int _currentDepth;
 
        public ProcessContent(string input, string output, Dictionary<string, List<XmlNode>> contClass) : base(input, output)
        {
            _contClass = contClass;
            DeclareBefore(XmlNodeType.Element, BuildPath);
            DeclareAfter(XmlNodeType.EndElement, ReducePath);
            DeclareBefore(XmlNodeType.Element, InsertBeforeContent);
            Parse();
        }

        private void InsertBeforeContent(XmlReader r)
        {
            if (_lastClass == null) return;
            if (_contClass.ContainsKey(_lastClass))
            {
                foreach (var node in _contClass[_lastClass])
                {
                    Debug.Assert(node.ParentNode != null, "node.ParentNode != null");
                    Debug.Assert(node.ParentNode.Attributes != null, "node.ParentNode.Attributes != null");
                    var target = node.ParentNode.Attributes["target"];
                    if (r.Name != target.InnerText)
                        continue;
                    var criteria = node.SelectSingleNode("parent::*/PSEUDO[name='before']");
                    if (criteria == null)
                        continue;
                    criteria = criteria.PreviousSibling;
                    var level = -1;
                    var eval = true;
                    while (criteria != null && eval)
                    {
                        switch (criteria.LocalName)
                        {
                            case "PRECEDES":
                                criteria = criteria.PreviousSibling;
                                Debug.Assert(criteria != null, "criteria != null");
                                if (criteria.LocalName == "TAG")
                                {
                                    eval = criteria.FirstChild.InnerText == _precedingTag && r.Depth == _precedingDepth;
                                }
                                else
                                {
                                    eval = criteria.FirstChild.InnerText == _precedingClass && r.Depth == _precedingDepth;
                                }
                                break;
                            case "TAG":
                                eval =  elem.Contains(criteria.FirstChild.InnerText);
                                if (eval)
                                {
                                    var newLevel = (level > 0)? elem.ToList().IndexOf(criteria.FirstChild.InnerText, level) : elem.ToList().IndexOf(criteria.FirstChild.InnerText);
                                    eval = newLevel > level;
                                    level = newLevel;
                                }
                                break;
                            case "PARENTOF":
                                break;
                            case "CLASS":
                                eval =  clas.Contains(criteria.FirstChild.InnerText);
                                if (eval)
                                {
                                    var newLevel = (level > 0)? clas.ToList().IndexOf(criteria.FirstChild.InnerText, level): clas.ToList().IndexOf(criteria.FirstChild.InnerText);
                                    eval = newLevel > level;
                                    level = newLevel;
                                }
                                break;
                            case "PSEUDO":
                                switch (criteria.FirstChild.InnerText)
                                {
                                    case "first-child":
                                        eval = r.Depth > _precedingDepth;
                                        break;
                                    case "last-child":
                                        break;
                                }
                                break;
                        }
                        criteria = criteria.PreviousSibling;
                    }

                }
            }
        }

        private void BuildPath(XmlReader r)
        {
            if (elem.Count > r.Depth)
            {
                elem.Pop();
                clas.Pop();
            }
            elem.Push(r.Name);
            var myClass = r.GetAttribute("class");
            clas.Push(r.GetAttribute("class"));
            if (myClass != null)
            {
                _lastClass = myClass;
            }
            else
            {
                _lastClass = GetLastClass();
            }
            TrackPrevious(r);
        }

        private void TrackPrevious(XmlReader r)
        {
            _precedingTag = _currentTag;
            _precedingClass = _currentClass;
            _precedingDepth = _currentDepth;
            _currentTag = elem.Peek();
            _currentClass = clas.Peek();
            _currentDepth = r.Depth;
        }

        private void ReducePath(XmlReader r)
        {
            elem.Pop();
            clas.Pop();
            _lastClass = GetLastClass();
        }

        private string GetLastClass()
        {
            for (int index = clas.Count - 1; index >= 0; index--)
            {
                var val = clas.ToArray()[index];
                if (val != null)
                    return val;
            }
            return null;
        }

    }
}
