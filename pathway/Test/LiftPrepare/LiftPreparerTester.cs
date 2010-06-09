using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using NMock2;
using NUnit.Framework;
using SIL.PublishingSolution;

namespace Test.LiftPrepare
{
    [TestFixture]
    [Ignore]
    public class LiftPreparerTester
    {
        private static string TestingDirectory = Environment.CurrentDirectory + @"\..\..\LiftPrepare\TestFiles\\";
        private static string InputDirectory = TestingDirectory + @"Input\";
        private  string ActualOutputDirectory = TestingDirectory + @"Output\";
        private  string ExpectedOutputDirectory = TestingDirectory + @"Expected\";
        private Mockery mocks;
        private LiftPreparer liftPreparer;

        [TestFixtureSetUp]
        public void Setup()
        {
            if (!Directory.Exists(TestEnvironment.ActualOutputRoot))
                Directory.CreateDirectory(TestEnvironment.ActualOutputRoot);
            var buangPath = TestEnvironment.ActualOutputRoot + @"buang";
            if (!Directory.Exists(buangPath))
                Directory.CreateDirectory(buangPath);
            var yiPath = TestEnvironment.ActualOutputRoot + @"yi";
            if (!Directory.Exists(yiPath))
                Directory.CreateDirectory(yiPath);
            var sena2Path = TestEnvironment.ActualOutputRoot + @"sena2";
            if (!Directory.Exists(sena2Path))
                Directory.CreateDirectory(sena2Path);
            var akoosePath = TestEnvironment.ActualOutputRoot + @"akoose";
            if (!Directory.Exists(akoosePath))
                Directory.CreateDirectory(akoosePath);
        }

        [SetUp]
        public void testSetup()
        {
        }

        [Test]
        public void testAkooseFilters()
        {
            liftPreparer = new LiftPreparer(ActualOutputDirectory + @"akoose\");
            liftPreparer.loadLift(InputDirectory + @"akoose\akoose.lift");
            var entryAndSenseFilter = InputDirectory + @"liftEntryAndSenseFilter.xsl";
            var langFilter = InputDirectory + @"liftLangFilter.xsl";
            var filterURIs = new string[] { entryAndSenseFilter, langFilter };
            liftPreparer.loadFilters(filterURIs);
            liftPreparer.applyFilters();
            FileAssert.AreEqual(ActualOutputDirectory + @"akoose\afterFilter0.lift", ExpectedOutputDirectory + @"akoose\afterFilter0.lift");
            FileAssert.AreEqual(ActualOutputDirectory + @"akoose\afterFilter1.lift", ExpectedOutputDirectory + @"akoose\afterFilter1.lift");
        }

        [Test]
        public void testBuangFilters()
        {
            liftPreparer = new LiftPreparer(ActualOutputDirectory + @"buang\");
            liftPreparer.loadLift(InputDirectory + @"buang\buang.lift");
            var entryAndSenseFilter = InputDirectory + @"liftEntryAndSenseFilter.xsl";
            var langFilter = InputDirectory + @"liftLangFilter.xsl";
            var filterURIs = new string[] { entryAndSenseFilter, langFilter };
            liftPreparer.loadFilters(filterURIs);
            liftPreparer.applyFilters();
            FileAssert.AreEqual(ActualOutputDirectory + @"buang\afterFilter0.lift", ExpectedOutputDirectory + @"buang\afterFilter0.lift");
            FileAssert.AreEqual(ActualOutputDirectory + @"buang\afterFilter1.lift", ExpectedOutputDirectory + @"buang\afterFilter1.lift");
        }

        [Test]
        public void testYiFilters()
        {
            liftPreparer = new LiftPreparer(ActualOutputDirectory + @"yi\");
            liftPreparer.loadLift(InputDirectory + @"yi\yi.lift");
            var entryAndSenseFilter = InputDirectory + @"liftEntryAndSenseFilter.xsl";
            var langFilter = InputDirectory + @"liftLangFilter.xsl";
            var filterURIs = new string[] { entryAndSenseFilter, langFilter };
            liftPreparer.loadFilters(filterURIs);
            liftPreparer.applyFilters();
            FileAssert.AreEqual(ActualOutputDirectory + @"yi\afterFilter0.lift", ExpectedOutputDirectory + @"yi\afterFilter0.lift");
            FileAssert.AreEqual(ActualOutputDirectory + @"yi\afterFilter1.lift", ExpectedOutputDirectory + @"yi\afterFilter1.lift");
        }

        [Test]
        public void testSena2Filters()
        {
            liftPreparer = new LiftPreparer(ActualOutputDirectory + @"sena2\");
            liftPreparer.loadLift(InputDirectory + @"sena2\sena2.lift");
            var entryAndSenseFilter = InputDirectory + @"liftEntryAndSenseFilter.xsl";
            var langFilter = InputDirectory + @"liftLangFilter.xsl";
            var filterURIs = new string[] { entryAndSenseFilter, langFilter };
            liftPreparer.loadFilters(filterURIs);
            liftPreparer.applyFilters();
            FileAssert.AreEqual(ActualOutputDirectory + @"sena2\afterFilter0.lift", ExpectedOutputDirectory + @"sena2\afterFilter0.lift");
            FileAssert.AreEqual(ActualOutputDirectory + @"sena2\afterFilter1.lift", ExpectedOutputDirectory + @"sena2\afterFilter1.lift");
        }

        [Test]
        public void testAkooseSorting()
        {
            liftPreparer = new LiftPreparer(ActualOutputDirectory + @"akoose\");
            liftPreparer.loadLift(ActualOutputDirectory + @"akoose\afterFilter1.lift");
            liftPreparer.applySort();
            FileAssert.AreEqual(ActualOutputDirectory + @"akoose\afterSorter2.lift", ExpectedOutputDirectory + @"akoose\afterSorter2.lift");

        }

        [Test]
        public void testYiWritingSystemSorting()
        {
            liftPreparer = new LiftPreparer(ActualOutputDirectory + @"yi\");
            liftPreparer.loadLift(InputDirectory + @"yi\yi-verysmall.lift");
            liftPreparer.sortWritingSystems();
        }
    }
}