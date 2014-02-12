using System;
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