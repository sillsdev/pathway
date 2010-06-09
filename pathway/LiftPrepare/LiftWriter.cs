using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace SIL.PublishingSolution
{
    public class LiftWriter : XmlTextWriter
    {
        public LiftWriter(String fileName) : base(fileName,null)
        {
            
        }
    }
}