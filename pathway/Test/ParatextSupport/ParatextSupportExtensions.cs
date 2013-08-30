using System.Xml.Xsl;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.ParatextSupport
{
    /// <summary>
    /// ParatextSupportExtensions uses reflection to expose items for testing in the ParatextSupport dll. 
    /// </summary>
    public static class ParatextSupportExtensions
    {
        public static XslCompiledTransform UsxToUsfmXslt(ParatextPathwayLink converter)
        {
            return (XslCompiledTransform)ReflectionHelperLite.GetField(converter, "m_usxToXhtml");
        }

        public static XslCompiledTransform EncloseParasInSectionsXslt(ParatextPathwayLink converter)
        {
            return (XslCompiledTransform)ReflectionHelperLite.GetField(converter, "m_encloseParasInSections");
        }
    }
}
