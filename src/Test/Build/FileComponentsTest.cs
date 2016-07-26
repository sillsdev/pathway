// --------------------------------------------------------------------------------------------
// <copyright file="FileComponentsTest.cs" from='2011' to='2014' company='SIL International'>
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

using System.IO;
using System.Text;
using System.Xml;
using BuildStep;
using NUnit.Framework;
using SIL.Tool;

namespace Test.Build
{
    [TestFixture]
    public class FileComponentsTest : FileComponents
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
        public void MakeIdTest()
        {
            ResetIds();
            var actual = MakeId("ConfigurationTool");
            Assert.AreEqual("configurationtool", actual);
            var actual2 = MakeId("ConfigurationTool");
            Assert.AreEqual("configurationtool2", actual2);
        }

        [Test]
        public void ProcessTreeTest()
        {
            if (Common.IsUnixOS())
            {
                return;
            }
            ResetIds();
            var inputGuids = _tf.Input("FileLibrary.xml");
            LoadGuids(inputGuids);
            ApplicationFileName = "Application.wxs";
            DirectoryInfo directoryInfo = new DirectoryInfo(_tf.Input(@"output\Release"));
            foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
            {
                ResetFileComponents();
                ProcessTree((XmlElement)XDoc.SelectSingleNode("//*[@Id='APPLICATIONFOLDER']"), directory.FullName);
                AddFeatures();
            }
            
            // Check File & Features Match
            var actualPath = _tf.Output("Application.wxs");
            var writer = XmlTextWriter.Create(actualPath, new XmlWriterSettings{Indent = true, Encoding = Encoding.UTF8});
            XDoc.WriteTo(writer);
            writer.Close();
            XmlAssert.AreEqual(_tf.Expected("Application.wxs"),actualPath, "File Feature format changed");
            
            // Check no Guids created
            var actualGuids = _tf.Output("FileLibrary.xml");
            SaveGuids(actualGuids);
            XmlAssert.AreEqual(inputGuids, actualGuids, "Guids changed");
        }
    }
}
