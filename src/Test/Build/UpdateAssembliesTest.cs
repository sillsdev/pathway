// --------------------------------------------------------------------------------------------
// <copyright file="UpdateAssembliesTest.cs" from='2011' to='2015' company='SIL International'>
//      Copyright ( c ) 2015, SIL International.  
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

using System.IO;
using BuildStep;
using NUnit.Framework;

namespace Test.Build
{
    [TestFixture]
    public class UpdateAssembliesTest : UpdateAssemblies
    {
        #region Setup

        private TestFiles _tf;
        [TestFixtureSetUp]
        public void Setup()
        {
            _tf = new TestFiles("Build");
        }
        #endregion Setup

        [Test]
        [Category("SkipOnTeamCity")]
        public void UpdateProductTest()
        {
            var result = UpdateProduct(_tf.Input("Pathway.wxs"), "1.13.4.4658", "BTE");
            const bool overwrite = true;
            File.Copy(result, _tf.Output("Pathway.wxs"), overwrite);
            File.Delete(result);
            XmlAssert.AreEqual(_tf.Expected("Pathway.wxs"), _tf.Output("Pathway.wxs"),"Pathway Product File mismatch on insert");
        }

        [Test]
        [Category("SkipOnTeamCity")]
        public void UpdateProduct2Test()
        {
            var result = UpdateProduct(_tf.Input("Pathway2.wxs"), "1.13.4.4658", "SE");
            const bool overwrite = true;
            File.Copy(result, _tf.Output("Pathway2.wxs"), overwrite);
            File.Delete(result);
            XmlAssert.AreEqual(_tf.Expected("Pathway2.wxs"), _tf.Output("Pathway2.wxs"), "Pathway Product File mismatch on change");
        }

    }
}
