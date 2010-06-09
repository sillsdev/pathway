using System;
using System.Collections.Generic;
using System.IO;

using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace SIL.PublishingSolution
{
    public static class Environ
    {
        public const string pathToFilterTemplates = @"..\..\LiftPrepare\TestFiles\Input\";
        public const string pathToTransformTemplate = @"..\..\LiftPrepare\TestFiles\Input\";
        public const string defaultIcuRules = @"[alternate shifted]";
        private static XmlSchema _liftXMLSchema = new XmlSchema();

        private const string pathToLiftSchema = @"..\..\LiftPrepare\TestFiles\Input\lift.xsd";
        private static string _pathToLiftSchema = pathToLiftSchema;
        public static string PathToLiftSchema
        {
            get
            {
                return _pathToLiftSchema;
            }
            set
            {
                _pathToLiftSchema = value;
            }
        }

        public static XmlSchema liftXMLSchema
        {
            get
            {
                _liftXMLSchema = XmlSchema.Read(new XmlTextReader(PathToLiftSchema), liftValidationEventHandler);
                return _liftXMLSchema;
            }
        }

        public static void liftValidationEventHandler(object sender, ValidationEventArgs args)
        {
            throw new InvalidLiftException();
        }

        public class InvalidLiftException : Exception
        {
            public InvalidLiftException()
                : base("LIFT File failed to validate.")
            {
            }
        }
    }
}