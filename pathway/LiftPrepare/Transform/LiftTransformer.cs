using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;

namespace SIL.PublishingSolution.Transform
{
    public class LiftTransformer
    {
        public LiftTransformer()
        {
            
        }

        public XmlDocument applyTo(LiftDocument documentToTransform)
        {
            var transformer = new XslCompiledTransform();
            transformer.Load(Environ.pathToTransformTemplate + @"liftTransform.xsl", null, null);
            var buffer = new StringBuilder();
            var writer = new StringWriter(buffer);
            transformer.Transform(documentToTransform.asXmlDocument(),null,writer);
            writer.Close();
            var reader = new StringReader(buffer.ToString());
            var doc = new XmlDocument();
            doc.XmlResolver = null;
            doc.Load(reader);
            return doc;
        }
    }
}
