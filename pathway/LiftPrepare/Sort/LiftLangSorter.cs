using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using System.Text;
using System.Xml;

namespace SIL.PublishingSolution.Sort
{
    public class LiftLangSorter
    {
        private LiftDocument liftToSort;
        private SortedDictionary<string, LiftEntry> sortedEntryKeyPairs;
        private XmlNode rootLiftNode;
        private Palaso.WritingSystems.Collation.IcuRulesCollator collator;

        public LiftLangSorter()
        {
            
        }
 
        public LiftLangSorter (LiftDocument liftToSort)
        {
            this.liftToSort = liftToSort;
        }

        public LiftLangSorter (LiftReader liftToSort)
        {
            this.liftToSort = new LiftDocument();
            this.liftToSort.Load(liftToSort);
        }

        public void sortWritingSystems()
        {
            //rootLiftNode = liftToSort.SelectSingleNode("lift");
            var nodesWithLangAttributes = liftToSort.nodesWithLangAttributes();
            var nodes = getListFromXMLNodList(nodesWithLangAttributes);
            int countToLastSibling;
            for (int i = 0; i < nodes.Count; i += countToLastSibling)
            {
                countToLastSibling = getCountToLastSibling(i, nodes[i], nodes);
                nodes.Sort(i,countToLastSibling,new WritingSystemNodeComparer());
                replaceSortedNodes(nodes.GetRange(i,countToLastSibling));
                
            }
        }

        public List<XmlNode> getListFromXMLNodList(XmlNodeList xmlNodeList)
        {
            var list = new List<XmlNode>();
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                list.Add(xmlNode);
            }
            return list;
        }

        private int getCountToLastSibling(int indexOfFirstSibling, XmlNode firstSibling, List<XmlNode> listOfNodes)
        {
            int count = 0;
            var contexOfFirstSibling = getContext(firstSibling);
            string contextOfCurrentSibling;
            do
            {
                var currentSibling = listOfNodes[count++];
                contextOfCurrentSibling = getContext(currentSibling);
            } while (contexOfFirstSibling == contextOfCurrentSibling);
            return count > 1 ? count-1 : count; //Is this the best sol'n here?
        }

        private string getContext(XmlNode node)
        {
            if (node.ParentNode == null)
            {
                return node.Name;
            }
            return getContext(node.ParentNode) + "/" + node.Name;
        }

        public void replaceSortedNodes(List<XmlNode> sortedNodes)
        {
            if (sortedNodes.Count == 0)
            {
                return;
            }
            var parentNode = sortedNodes[0].ParentNode;
            var previousSibling = sortedNodes[0].PreviousSibling;
            foreach (XmlNode node in sortedNodes)
            {
                try
                {
                    if (previousSibling == null)
                    {
                        parentNode.RemoveChild(node);
                        parentNode.PrependChild(node);
                    }
                    else
                    {
                        parentNode.RemoveChild(node);
                        parentNode.InsertAfter(node, previousSibling);
                        previousSibling = node;
                    }
                }
                catch (Exception)
                {
                    Debug.Assert(false, parentNode.Name + " || " + getContext(node) + " || " + node.Name + " || " + node.ParentNode.Name + " || " + sortedNodes[0].Name);
                }
            }
        }
    }

    class WritingSystemNodeComparer : IComparer<XmlNode>
    {
        public int Compare(XmlNode x, XmlNode y)
        {
            return String.Compare(x.Attributes["lang"].Value, y.Attributes["lang"].Value);
        }
    }

    class WritingSystem
    {
        public string context { get; private set; }

        public string language { get; private set; }

        public WritingSystem(string context, string language)
        {
            this.context = context;
            this.language = language;
        }

        public WritingSystem(XmlNode nodeWithLangAttribute)
        {
            context = getContext(nodeWithLangAttribute);
            language = nodeWithLangAttribute.Attributes["lang"].Value;
        }

        private string getContext(XmlNode node)
        {
            if (node.ParentNode == null)
            {
                return node.Name;
            }
            return getContext(node.ParentNode) + "/" + node.Name;
        }

        public bool hasTheSameContextAs(WritingSystem theOtherWritingSystem)
        {
            return context == theOtherWritingSystem.context;
        }

        public bool equals(WritingSystem theOtherWritingSystem)
        {
            return context == theOtherWritingSystem.context && language == theOtherWritingSystem.language;
        }
    }

    class WritingSystems : IEnumerable
    {
        private List<WritingSystem> writingSystems;

        public WritingSystems(XmlNodeList nodesWithLangAttributes)
        {
            foreach (XmlNode node in nodesWithLangAttributes)
            {
                if (!this.contains(node))
                {
                    writingSystems.Add(new WritingSystem(node));
                }
            }
        }

        public bool contains(WritingSystem writingSystemToCheck)
        {
            foreach (var writingSystem in writingSystems)
            {
                if (writingSystemToCheck.equals(writingSystem))
                {
                    return true;
                }
            }
            return false;
        }

        public bool contains(XmlNode nodeWithLangAttribute)
        {
            var temporaryWritingSystem = new WritingSystem(nodeWithLangAttribute);
            return contains(temporaryWritingSystem);
        }

        public IEnumerator GetEnumerator()
        {
            return writingSystems.GetEnumerator();
        }
    }
}