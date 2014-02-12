// --------------------------------------------------------------------------------------------
// <copyright file="LiftEntry.cs" from='2009' to='2014' company='SIL International'>
//      Copyright © 2014, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// 
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace SIL.PublishingSolution
{
    public class LiftEntry
    {
        private XmlNode entryNode;

        public LiftEntry(XmlNode entry)
        {
            this.entryNode = entry;
        }

        public string getGuid()
        {
            return this.entryNode.Attributes["guid"].Value;
        }

        public string getKey()
        {   
            return headWord() + morphType() + homoGraphNumber();
        }

        private string headWord()
        {
            return entryNode.SelectSingleNode("lexical-unit/form/text").InnerText;
        }

        private string morphType()
        {
            var traitWithMorphType = entryNode.SelectSingleNode(@"trait[@name='morph-type']");
            return (traitWithMorphType == null || traitWithMorphType.Attributes["value"] == null) ? null : "-" + traitWithMorphType.Attributes["value"].Value;
        }

        private string homoGraphNumber()
        {
            return entryNode.Attributes["order"] != null ? "-" + entryNode.Attributes["order"].Value : null;
        }

        private readonly List<string> _field = new List<string>();
        private readonly Dictionary<string,int> _fieldCount = new Dictionary<string, int>();

        public List<string> getFields()
        {
            _field.Clear();
            _fieldCount.Clear();
            insertFieldNames(entryNode.ChildNodes);
            return _field;
        }

        private void insertFieldNames(XmlNodeList children)
        {
            foreach (XmlNode child in children)
            {
                if (!_field.Contains(child.Name))
                {
                    _field.Add(child.Name);
                    _fieldCount[child.Name] = 1;
                }
                else
                {
                    _fieldCount[child.Name] += 1;
                }
                insertFieldNames(child.ChildNodes);
            }
        }

        public int getFieldCount(string field)
        {
            if (_field.Contains(field))
                return _fieldCount[field];
            return 0;
        }

        public XmlNode asNode()
        {
            return entryNode;
        }
    }

    public class LiftEntries : IEnumerable
    {
        private List<LiftEntry> entries = new List<LiftEntry>();

        public LiftEntries(XmlNodeList entries)
        {
            foreach (XmlNode entry in entries)
            {
                this.entries.Add(new LiftEntry(entry));
            }
        }

        public IEnumerator GetEnumerator()
        {
            return entries.GetEnumerator();
        }

        public int Count()
        {
            return entries.Count;
        }

        public LiftEntry this[int index]
        {
            get
            {
                return entries[index];
            }
        }
    }

    class LiftEntriesEnum : IEnumerator
    {
        public LiftEntry[] entries;
        private int position = -1;

        public LiftEntriesEnum(LiftEntry[] entries)
        {
            this.entries = entries;
        }

        public bool MoveNext()
        {
            position++;
            return (position < entries.Length);
        }

        public void Reset()
        {
            position = -1;
        }

        public object Current
        {
            get
            {
                try
                {
                    return entries[position];
                }
                catch (IndexOutOfRangeException)
                {
                    
                    throw new InvalidOperationException();
                }
            }
        }
    }
}