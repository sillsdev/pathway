using SIL.PublishingSolution;
using NUnit.Framework;
namespace Test.CssParserTest
{
   
    /// <summary>
    ///This is a test class for StyleAttributeTest and is intended
    ///to contain all StyleAttributeTest Unit Tests
    ///</summary>
    [TestFixture]
    public class StyleAttributeTest
    {
        private StyleAttribute target;
        #region Setup
        [TestFixtureSetUp]
        protected void SetUp()
        {
            target = new StyleAttribute();
        }
        #endregion Setup

        /// <summary>
        ///A test for SetAttribute
        ///</summary>
        [Test]
        public void SetAttributeTest2()
        {
            string className = string.Empty;
            string attributeName = "font-size";
            string attributeValue = "50pt";
            target.SetAttribute(attributeName, attributeValue);
            
            string expected = "pt";
            Assert.AreEqual(expected, target.Unit);

            Assert.AreEqual(className, target.ClassName);

            float expectedFloat = 50.0F;
            Assert.AreEqual(expectedFloat, target.NumericValue);

            Assert.AreEqual(attributeName, target.Name);
        }

        /// <summary>
        ///A test for SetAttribute
        ///</summary>
        [Test]
        public void SetAttributeTest1()
        {
            string className = "t1"; 
            string attributeName = "font-size"; 
            string attributeValue = "50pt"; 

            target.SetAttribute(className, attributeName, attributeValue);
            
            string expected = "pt";
            Assert.AreEqual(expected,target.Unit);
            
            float expectedFloat = 50.0F;
            Assert.AreEqual(expectedFloat, target.NumericValue);

            Assert.AreEqual(className, target.ClassName);
            Assert.AreEqual(attributeName, target.Name);



        }

        /// <summary>
        ///A test for SetAttribute
        ///</summary>
        [Test]
        public void SetAttributeTest()
        {
            string className = string.Empty;
            string attributeName = string.Empty; 
            string attributeValue = "50pt";
            target.SetAttribute(attributeValue);

            string expected = "pt";
            Assert.AreEqual(expected, target.Unit);

            float expectedFloat = 50.0F;
            Assert.AreEqual(expectedFloat, target.NumericValue);

            Assert.AreEqual(className, target.ClassName);
            Assert.AreEqual(attributeName, target.Name);

        }
    }
}
