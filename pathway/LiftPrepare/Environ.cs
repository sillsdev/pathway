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
        private static string currentDir = Environment.CurrentDirectory;
        private static int binFolderPart = currentDir.IndexOf(Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar);
        private static string baseDir =  currentDir.Substring(0, binFolderPart);
        public static string pathToFilterTemplates = baseDir + @"\LiftPrepare\TestFiles\Input\";
        public static string pathToTransformTemplate = baseDir + @"\LiftPrepare\TestFiles\Input\";
        public const string defaultIcuRules = @"[alternate shifted]";
        private static XmlSchema _liftXMLSchema = new XmlSchema();

        private static string pathToLiftSchema = baseDir + @"\LiftPrepare\TestFiles\Input\lift.xsd";
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