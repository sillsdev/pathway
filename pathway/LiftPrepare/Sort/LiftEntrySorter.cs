using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using System.Text;
using System.Xml;


namespace SIL.PublishingSolution.Sort
{
    public class LiftEntrySorter
    {
        private LiftDocument liftToSort;
        private SortedDictionary<string,LiftEntry> sortedEntryKeyPairs;
        private XmlNode rootLiftNode;
        private Palaso.WritingSystems.Collation.IcuRulesCollator collator;
        private string icuRules = Environ.defaultIcuRules;

        public void sort(LiftReader liftReaderToBeSorted, LiftWriter sortedLiftWriter)
        {
            prepLiftForSort(liftReaderToBeSorted);
            moveEntriesIntoSortedEntryKeyPairs();
            reinsertSortedEntriesIntoLiftTree(rootLiftNode);
            finalizeAndWriteSortedLift(sortedLiftWriter);
        }

        private void prepLiftForSort(LiftReader liftReaderToBeSorted)
        {
            liftToSort = new LiftDocument();
            liftToSort.Load(liftReaderToBeSorted);
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
                    resolveDuplicateEntries(entry, entry.getKey());
                }
                rootLiftNode.RemoveChild(entry.asNode());
            }
        }

        /* Currently just a placeholder with minimal functionality.
         * The "production" version will probably need to interact 
         * with the user.
         */
        private void resolveDuplicateEntries(LiftEntry duplicateEntry, string duplicateKey)
        {
            var currentEntry = sortedEntryKeyPairs[duplicateKey];
            sortedEntryKeyPairs.Remove(duplicateKey);
            try
            {
                sortedEntryKeyPairs.Add(duplicateKey + "0", currentEntry);
            }
            catch (Exception)
            {
                resolveDuplicateEntries(currentEntry, duplicateKey + "0");
            }
            try
            {
                sortedEntryKeyPairs.Add(duplicateKey + "1", duplicateEntry);
            }
            catch (Exception)
            {
                resolveDuplicateEntries(duplicateEntry, duplicateKey + "1");
            }
        }

        private void reinsertSortedEntriesIntoLiftTree(XmlNode liftRoot)
        {
            foreach (var entryKeyPair in sortedEntryKeyPairs)
            {
                var entry = entryKeyPair.Value;
                liftRoot.AppendChild(entry.asNode());
            }
        }

        private void finalizeAndWriteSortedLift(LiftWriter sortedLiftWriter)
        {
            liftToSort.Save(sortedLiftWriter);
        }
        
        public void addRules(params string[] rules)
        {
            foreach (string rule in rules)
            {
                icuRules += rule;
            }
        }

        public string getRules()
        {
            return icuRules;
        }
 
    }
}