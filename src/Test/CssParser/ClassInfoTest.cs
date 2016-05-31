// --------------------------------------------------------------------------------------------
// <copyright file="ClassInfoTest.cs" from='2009' to='2014' company='SIL International'>
//      Copyright ( c ) 2014, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// 
// </remarks>
// --------------------------------------------------------------------------------------------

using System.Collections;
using NUnit.Framework;
using SIL.Tool;

namespace Test.CssParserTest
{
    [TestFixture]
    public class ClassInfoTest : ClassInfo
    {
        #region Private Variables

        private ClassAttrib _classAttrib;
        private ClassInfo _classInfo;
        
        #endregion Private Variables

        #region Setup
        [TestFixtureSetUp]
        protected void SetUp()
        {
        }
        #endregion Setup

        [Test]
        public void SetClassAttrib()
        {
            _classAttrib = new ClassAttrib();
            string className = "testa";
            ArrayList attribute = new ArrayList();
            attribute.Add("eng");
            attribute.Add("US1");
            _classAttrib.SetClassAttrib(className, attribute);
            Assert.AreEqual(className, _classAttrib.ClassName, "SetClassAttrib test Failed");
            Assert.AreEqual(attribute, _classAttrib.Attribute, "SetClassAttrib test Failed");
        }

        [Test]
        public new void CoreClass()
        {
            _classInfo = new ClassInfo();
            ClassAttrib _expectedClassAttrib = new ClassAttrib();
            ClassAttrib _outputClassAttrib = new ClassAttrib();

            string className = "testa";
            ArrayList attribute = new ArrayList();
            attribute.Add("eng");
            attribute.Add("US1");
            _expectedClassAttrib.SetClassAttrib(className, attribute);

            _classInfo.CoreClass = _expectedClassAttrib;
            _outputClassAttrib = _classInfo.CoreClass;
            Assert.AreEqual(_outputClassAttrib.ClassName , _expectedClassAttrib.ClassName , "CoreClass test Failed");
            Assert.AreEqual(_outputClassAttrib.Attribute , _expectedClassAttrib.Attribute, "CoreClass test Failed");
        }

        [Test]
        public new void Precede()
        {
            _classInfo = new ClassInfo();
            ClassAttrib _expectedClassAttrib = new ClassAttrib();
            ClassAttrib _outputClassAttrib = new ClassAttrib();

            string className = "testa";
            ArrayList attribute = new ArrayList();
            attribute.Add("eng");
            attribute.Add("US1");
            _expectedClassAttrib.SetClassAttrib(className, attribute);

            _classInfo.Precede = _expectedClassAttrib;
            _outputClassAttrib = _classInfo.Precede;
            Assert.AreEqual(_outputClassAttrib.ClassName, _expectedClassAttrib.ClassName, "CoreClass test Failed");
            Assert.AreEqual(_outputClassAttrib.Attribute, _expectedClassAttrib.Attribute, "CoreClass test Failed");
        }

        [Test]
        public new void Ancestor()
        {
            _classInfo = new ClassInfo();
            ClassAttrib _expectedClassAttrib = new ClassAttrib();
            ClassAttrib _outputClassAttrib = new ClassAttrib();

            string className = "testa";
            ArrayList attribute = new ArrayList();
            attribute.Add("eng");
            attribute.Add("US1");
            _expectedClassAttrib.SetClassAttrib(className, attribute);

            _classInfo.Ancestor = _expectedClassAttrib;
            _outputClassAttrib = _classInfo.Ancestor;
            Assert.AreEqual(_outputClassAttrib.ClassName, _expectedClassAttrib.ClassName, "CoreClass test Failed");
            Assert.AreEqual(_outputClassAttrib.Attribute, _expectedClassAttrib.Attribute, "CoreClass test Failed");
        }

        [Test]
        public new void ParentPrecede()
        {
            _classInfo = new ClassInfo();
            ClassAttrib _expectedClassAttrib = new ClassAttrib();
            ClassAttrib _outputClassAttrib = new ClassAttrib();

            string className = "testa";
            ArrayList attribute = new ArrayList();
            attribute.Add("eng");
            attribute.Add("US1");
            _expectedClassAttrib.SetClassAttrib(className, attribute);

            _classInfo.ParentPrecede = _expectedClassAttrib;
            _outputClassAttrib = _classInfo.ParentPrecede; 
            Assert.AreEqual(_outputClassAttrib.ClassName, _expectedClassAttrib.ClassName, "CoreClass test Failed");
            Assert.AreEqual(_outputClassAttrib.Attribute, _expectedClassAttrib.Attribute, "CoreClass test Failed");
        }
    }
}
