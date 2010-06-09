using System;
using System.Collections;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace SIL.PublishingSolution.Filter
{
    public class LiftFilterChooseStatement
    {
        public string xpath { get; set; }
        public bool removeMatchingEntries { get; set; }
        private string _applyTo;
        public string applyTo //have to validate applyTo - so no automatic get/set here.
        {
            get
            {
                return _applyTo; 
            }
            set
            {
                if (isValidApplyTo(value))
                {
                    _applyTo = value;
                }
            }
        }

        public LiftFilterChooseStatement(string xpath, bool removeMatchingEntries, string applyTo)
        {
            this.xpath = xpath;
            this.removeMatchingEntries = removeMatchingEntries;
            if (isValidApplyTo(applyTo))
                this.applyTo = applyTo;
        }

        public LiftFilterChooseStatement(LiftFilterChooseStatement that)
        {
            this.xpath = that.xpath;
            this.removeMatchingEntries = that.removeMatchingEntries;
            this.applyTo = that.applyTo;
        }

        public object Clone()
        {
            return new LiftFilterChooseStatement(this);
        }

        public XmlDocumentFragment getFilterAsXmlNode(XmlDocument parentDocument)
        {
            var filterAsXmlDocumentFragment = parentDocument.CreateDocumentFragment();
            var removeMatchingEntriesString = removeMatchingEntries ? @"'true'" : @"'false'";
            var filterAsXslString = @"<when test=""" + xpath + @"""><value-of select=""" + removeMatchingEntriesString + @"""/></when>";
            filterAsXmlDocumentFragment.InnerXml = filterAsXslString;
            return filterAsXmlDocumentFragment;
        }

        public bool isValidApplyTo(string applyTo)
        {
            if (applyTo != "sense" && applyTo != "entry" && applyTo !="lang")
            {
                throw new LiftFilterChooseStatementException();
            }
            return true;
        }
    }

    public abstract class LiftFilterChooseStatments : IEnumerable, ICloneable
    {
        protected List<LiftFilterChooseStatement> filters;

        public LiftFilterChooseStatments()
        {
            filters = new List<LiftFilterChooseStatement>();
        }

        public LiftFilterChooseStatments(List<LiftFilterChooseStatement> filters)
        {
            this.filters = filters;
        }

        public void add(LiftFilterChooseStatement filterChooseStatement)
        {
            filters.Add(filterChooseStatement);
        }

        public void add(string xpath, bool removeMatchingEntries, string applyTo)
        {
            filters.Add(new LiftFilterChooseStatement(xpath, removeMatchingEntries, applyTo));
        }

        public IEnumerator GetEnumerator()
        {
            return filters.GetEnumerator();
        }

        public abstract object Clone();
    }

    public class LiftFilterChooseStatementException : Exception
    {
        public LiftFilterChooseStatementException()
            : base("Invalid applyTo - must be 'sense', 'entry' or 'lang'.")
        {

        }
    }
}
