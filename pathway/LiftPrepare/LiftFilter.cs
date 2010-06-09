using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace LiftPrepare
{
    public class LiftFilter : XmlDocument
    {
        public LiftFilter(string uri)
        {
            this.Load(uri);
        }
    }
}
