using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using NUnit.Framework;
using NMock2;
using System.Xml;
using SIL.PublishingSolution;

namespace Test.LiftPrepare
{
    [TestFixture]
    [Ignore]
    public partial class LiftPrepareTester : Form
    {
        public const string TestingDirectory = @"../../../TestData/";
        public const string InputDirectory = TestingDirectory + @"Input/";
        public const string ActualOutputDirectory = TestingDirectory + @"ActualOutput/";
        public const string ExpectedOutputDirectory = TestingDirectory + @"ExpectedOutput";
        public Mockery mocks;
        public LiftPreparer liftPreparer;

        public LiftPrepareTester()
        {
            InitializeComponent();
        }

        //[SetUp]
        //public void testSetup()
        //{
        //    mocks = new Mockery();
        //    liftPreparer = new LiftPreparer(TestingDirectory);
        //}

        #region Ignored Tests 
        [Test]
        [Ignore("Old Test")]
        public void liftPrepareLoadTest()
        {
            
        }

        [Test]
        [Ignore("Old Test")]
        public void liftPrepareLoadLift()
        {
            var originalLift = TestingDirectory + @"yi-test.lift";
            liftPreparer.loadLift(originalLift);
        }

        [Test]
        [Ignore("Old Test")]
        public void liftPrepareGetCurrentLift()
        {
            var liftFilePath = TestingDirectory + @"yi-test.lift";
            liftPreparer.loadLift(liftFilePath);
            Assert.AreEqual(typeof(XmlTextReader), liftPreparer.getCurrentLift().GetType());
            
        }

        [Test]
        [Ignore("Old Test")]
        public void liftPrepareLoadFilters()
        {
            var entryAndSenseFilter = TestingDirectory + @"liftEntryAndSenseFilter.xsl";
            var langFilter = TestingDirectory + @"liftLangFilter.xsl";
            var filterURIs = new string[]{entryAndSenseFilter,langFilter};
            liftPreparer.loadFilters(filterURIs);
        }

        [Test]
        [Ignore("Old Test")]
        public void liftPrepareApplyFilters()
        {
            liftPreparer.applyFilters();
            TextFileAssert.AreEqual(TestingDirectory + "afterFilter0.lift", TestingDirectory + "afterFilter0.lift");
            TextFileAssert.AreEqual(TestingDirectory + "afterFilter1.lift", TestingDirectory + "afterFilter1.lift");
        }

        [Test]
        [Ignore("Old Test")]
        public void liftPrepareLoadSorters()
        {
            var sorters = new string[1];
            liftPreparer.loadSorters(sorters);
        }
        #endregion
    }
}