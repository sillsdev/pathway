using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;


namespace LiftPrepare
{
    class LiftSorter
    {
        private LiftDocument liftToSort;
        private SortedDictionary<string,LiftEntry> sortedEntryKeyPairs;
        private XmlNode rootLiftNode;
        private Palaso.WritingSystems.Collation.IcuRulesCollator collator;

        public void sort(LiftReader liftReaderToBeSorted, LiftWriter sortedLiftWriter)
        {
            prepLiftForSort(liftReaderToBeSorted);
            moveEntriesIntoSortedEntryKeyPairs();
            reinsertSortedEntriesIntoLiftTree(rootLiftNode);
            parseLiftForWritingSystemContexts();
            finalizeAndWriteSortedLift(sortedLiftWriter);
        }

        private void prepLiftForSort(LiftReader liftReaderToBeSorted)
        {
            liftToSort = new LiftDocument();
            liftToSort.Load(liftReaderToBeSorted);
            const string icuRules = @"[alternate shifted]"; //causes punctuation to be ignored.
            collator = new Palaso.WritingSystems.Collation.IcuRulesCollator(icuRules);
            sortedEntryKeyPairs = new SortedDictionary<string, LiftEntry>(collator);
            rootLiftNode = liftToSort.getLiftNode();
        }

        private void moveEntriesIntoSortedEntryKeyPairs()
        {
            var entries = liftToSort.getEntries();
            foreach (LiftEntry entry in entries)
            {
                try
                {
                    sortedEntryKeyPairs.Add(entry.getKey(), entry);
                }
                catch (Exception)
                {
                    resolveDuplicateEntries(entry);
                }
                rootLiftNode.RemoveChild(entry.asNode());
            }
        }

        /* Currently just a placeholder with minimal functionality.
         * The "production" version will probably need to interact 
         * with the user.
         */
        private void resolveDuplicateEntries(LiftEntry duplicateEntry)
        {
            var duplicateKey = duplicateEntry.getKey();
            var currentEntry = sortedEntryKeyPairs[duplicateKey];

            sortedEntryKeyPairs.Remove(duplicateKey);
            sortedEntryKeyPairs.Add(duplicateKey+"0",currentEntry);
            sortedEntryKeyPairs.Add(duplicateKey+"1",duplicateEntry);
        }

        private void reinsertSortedEntriesIntoLiftTree(XmlNode liftRoot)
        {
            foreach (var entryKeyPair in sortedEntryKeyPairs)
            {
                var entry = entryKeyPair.Value;
                liftRoot.AppendChild(entry.asNode());
            }
        }

        private void parseLiftForWritingSystemContexts()
        {
            var elementsWithLangAttributes = rootLiftNode.SelectNodes("lift/entry/descendant::*[@lang]");
            var writingSystemContexts = new List<string>();
            foreach (XmlNode element in elementsWithLangAttributes)
            {
                if (!writingSystemContexts.Contains(element.Attributes["lang"].Value))
                {
                    writingSystemContexts.Add(element.Attributes["lang"].Value);
                }
            }
        }

        private void finalizeAndWriteSortedLift(LiftWriter sortedLiftWriter)
        {
            liftToSort.Save(sortedLiftWriter);
        }

        
    }
}
