using SIL.PublishingSolution;
using NUnit.Framework;
namespace Test.CssParser
{
   
    /// <summary>
    ///This is a test class for StyleAttributeTest and is intended
    ///to contain all StyleAttributeTest Unit Tests
    ///</summary>
    [TestFixture]
    public class ClassAttributeTest
    {
        private ClassAttribute target;
        #region Setup
        [TestFixtureSetUp]
        protected void SetUp()
        {
            target = new ClassAttribute();
        }
        #endregion Setup

 
        /// <summary>
        ///A test for SetAttribute
        ///</summary>
        [Test]
        public void SetAttributeTest1()
        {

            string attribSeperator = "'t1'";
            string attributeName = "'font-size'";
            string attributeValue = "'50'";

            target.SetAttribute(attributeName,attribSeperator, attributeValue);

            string expected = "t1";
            Assert.AreEqual(expected, target.AttributeSeperator);

            expected = "font-size";
            Assert.AreEqual(expected, target.Name);

            expected = "50";
            Assert.AreEqual(expected, target.AttributeValue);

        }

        /// <summary>
        ///A test for SetAttribute
        ///</summary>
        [Test]
        public void SetAttributeTest()
        {
            string attributeSeperator = string.Empty;
            string attributeName = "'font-size'";
            string attributeValue = string.Empty;

            target.SetAttribute(attributeName);

            string expected = "font-size";
            Assert.AreEqual(expected, target.Name);
            Assert.AreEqual(attributeSeperator, target.AttributeSeperator);
            Assert.AreEqual(attributeValue, target.AttributeValue);

        }
    }
}
