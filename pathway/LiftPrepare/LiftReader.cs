using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.Xml.Schema;
using SIL.PublishingSolution;

namespace SIL.PublishingSolution
{
    public class LiftReader : XmlTextReader
    {
        public LiftReader(String liftFile) : base(liftFile)
        {
            if (!isValidLift(liftFile))
            {
                throw new Environ.InvalidLiftException();
            }
            
        }

        private bool isValidLift(String liftFile)
        {
            var validatingReaderSettings = new XmlReaderSettings();
            validatingReaderSettings.Schemas.Add(Environ.liftXMLSchema);
            validatingReaderSettings.Schemas.Add(null,Environ.PathToLiftSchema);
            validatingReaderSettings.ValidationType = ValidationType.Schema;
            validatingReaderSettings.ValidationEventHandler += Environ.liftValidationEventHandler;
            var validatingReader = XmlReader.Create(liftFile,validatingReaderSettings);
            try
            {
                while (validatingReader.Read())
                {
                    //Simply need to read through the entire file.
                }
                return true;
            }
            catch (Environ.InvalidLiftException)
            {
                return false;
            }
        }
    }
}