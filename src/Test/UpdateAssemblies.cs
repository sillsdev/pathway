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

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using BuildTasks;
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
            Assert.AreEqual("ConfigurationTool", actual);
            var actual2 = MakeId("ConfigurationTool");
            Assert.AreEqual("ConfigurationTool2", actual2);
        }

        [Test]
        public void ShortNameTest()
        {
            if (Common.IsUnixOS())
            {
                return;
            }
            var actual = ShortName(_tf.Input("Files/ConfigurationTool"));
            Assert.AreEqual("CONFIG~1", Path.GetFileName(actual));
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
            DirectoryInfo directoryInfo = new DirectoryInfo(_tf.Input("Files"));
            foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
            {
                ResetFileComponents();
                ProcessTree((XmlElement)XDoc.SelectSingleNode("//Files"), directory.FullName);
                AddFeatures();
            }
            
            // Check File & Features Match
            var actualPath = _tf.Output("partial.xml");
            var writer = new XmlTextWriter(actualPath, Encoding.UTF8);
            XDoc.WriteTo(writer);
            writer.Close();
            XmlAssert.AreEqual(_tf.Expected("Partial ConfigurationTool.wxs"),actualPath, "File Feature format changed");
            
            // Check no Guids created
            var actualGuids = _tf.Output("FileLibrary.xml");
            SaveGuids(actualGuids);
            XmlAssert.AreEqual(inputGuids, actualGuids, "Guids changed");
        }

        [Test]
        public void ExecuteTest()
        {
            FolderTree.Copy(_tf.Input(""), _tf.Output(""));
            BasePath = _tf.Output("");
            FilesTemplate = "FilesPw7-tpl.wxs";
            FeaturesTemplate = "FeaturesPw7-tpl.wxs";
            if (!Common.IsUnixOS())
            {
                Execute();
            }
        }
    }
}
