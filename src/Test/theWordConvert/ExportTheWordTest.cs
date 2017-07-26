// --------------------------------------------------------------------------------------------
// <copyright file="ExportTheWordTest.cs" from='2009' to='2014' company='SIL International'>
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
// TheWord Test Support
// </remarks>
// --------------------------------------------------------------------------------------------

#region using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Xsl;
using NMock2;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;
using Test.CssDialog;
#endregion using

namespace Test.theWordConvert
{
    /// ----------------------------------------------------------------------------------------
    /// <summary>
    /// Test functions of Wordpress Convert
    /// </summary>
    /// ----------------------------------------------------------------------------------------
    [TestFixture]
    public class ExportTheWordTest: ExportTheWord
    {
        #region setup
        private static readonly Mockery Mocks = new Mockery();
        private static string _inputPath;
        private static string _outputPath;
        //private static string _expectedPath;
        private static string _converterPath;

        [TestFixtureSetUp]
        public void Setup()
        {
            Common.ProgInstall = PathPart.Bin(Environment.CurrentDirectory, @"/../../DistFiles");
            Common.SupportFolder = "";
            Common.ProgBase = Common.ProgInstall;
            string testPath = PathPart.Bin(Environment.CurrentDirectory, "/theWordConvert/TestFiles");
            _inputPath = Common.PathCombine(testPath, "Input");
            _outputPath = Common.PathCombine(testPath, "output");
            //_expectedPath = Common.PathCombine(testPath, "Expected");
            if (Directory.Exists(_outputPath))
                Directory.Delete(_outputPath, true);
            Directory.CreateDirectory(_outputPath);
            _converterPath = Common.PathCombine(_outputPath, "TheWord");
            FolderTree.Copy(Path.Combine(Common.ProgInstall, "TheWord"), _converterPath);
        }
        #endregion setup

        /// <summary>
        ///A test for ExportTheWord Constructor
        ///</summary>
        [Test]
        public void ExportTheWordConstructorTest()
        {
            var target = new ExportTheWord();
            Assert.IsNotNull(target);
        }

        [Test]
        public void ExportTypeTest()
        {
            var target = new ExportTheWord();
            var actual = target.ExportType;
            Assert.AreEqual("theWord/MySword", actual);
        }

        [Test]
        public void HandleScriptureTest()
        {
            var target = new ExportTheWord();
            var actual = target.Handle("Scripture");
            Assert.IsTrue(actual);
        }

        [Test]
        public void HandleDictionaryTest()
        {
            var target = new ExportTheWord();
            var actual = target.Handle("Dictionary");
            Assert.IsFalse(actual);
        }

        /// <summary>
        ///A test for Export
        ///</summary>
        [Test]
        public void ExportNullTest()
        {
            var target = new ExportTheWord();
            const PublicationInformation projInfo = null;
            var actual = target.Export(projInfo);
            Assert.IsFalse(actual);
        }

        [Test]
        public void LoadXsltTest()
        {
            LoadXslt();
            Assert.AreEqual("System.Text.UTF8Encoding", TheWord.OutputSettings.Encoding.ToString());
        }

        [Test]
        public void CollectTestamentBooksTest()
        {
            var curDir = Environment.CurrentDirectory;
            string vrsPath = PathPart.Bin(Environment.CurrentDirectory, "/../theWordConvert");
            Environment.CurrentDirectory = vrsPath;
            var otBooks = new List<string> ();
            var ntBooks = new List<string> ();
            CollectTestamentBooks(otBooks, ntBooks);
            Environment.CurrentDirectory = curDir;
            Assert.AreEqual(39, otBooks.Count);
            Assert.AreEqual(27, ntBooks.Count);
            Assert.AreEqual("GEN", otBooks[0]);
            Assert.AreEqual("MAL", otBooks[38]);
            Assert.AreEqual("MAT", ntBooks[0]);
            Assert.AreEqual("REV", ntBooks[26]);

        }

        [Test]
        public void LoadXsltParametersTest()
        {
			Common.ParatextData = @"C:\";
			Common.Ssf = FileInput("MP1.ssf");
            var actual = LoadXsltParameters(_inputPath);
            Assert.AreEqual(":", actual.GetParam("refPunc", ""));
            Assert.AreEqual(FileUrlPrefix + Common.PathCombine(@"C:\MP1", "BookNames.xml"), actual.GetParam("bookNames", ""));
        }

        [Test]
        public void OtFlagTest()
        {
            var fullName = FileInput("USX");
            var codeNames = new Dictionary<string, string>();
            var otBooks = new List<string>();
            var actual = OtFlag(fullName, codeNames, otBooks);
            Assert.False(actual);
            Assert.AreEqual(2, codeNames.Count);
        }

        [Test]
        public void TempNameTest()
        {
            const string book = "MAT";
            const string expected = @"<usx><book code= ""MAT""/></usx>";
            string actualTempName = TempName(book);
            var sr = new StreamReader(actualTempName);
            string actual = sr.ReadToEnd();
            sr.Close();
            File.Delete(actualTempName);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ProcessAllBooks
        ///</summary>
        [Test]
        [NUnit.Framework.Category("LongTest")]
        public void ProcessAllBooksTest()
        {
            Common.Testing = true;      // Don't write settings to settings folder.
            var inputParams = Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.ScriptureStyleSettingsCopy.xml");
            Debug.Assert(inputParams != null);
            Param.LoadValues(inputParams);
            LoadMyXslt();
            string fullName = Path.GetTempFileName();
            const bool otFlag = false;
            IEnumerable<string> otBooks = new List<string>(0);
            IEnumerable<string> ntBooks = new List<string>(2) { "MAT", "MRK" };
            var codeNames = new Dictionary<string, string>(2);
            codeNames["MAT"] = FileInput(@"USX\040MAT.usx");
            codeNames["MRK"] = FileInput(@"USX\041MRK.usx");
            var xsltArgs = new XsltArgumentList();
            xsltArgs.AddParam("bookNames", "", "file://" + FileInput("BookNames.xml"));
            var inProcess = (IInProcess)Mocks.NewMock(typeof(IInProcess));
            Expect.Exactly(2).On(inProcess).Method("PerformStep");
            ProcessAllBooks(fullName, otFlag, otBooks, ntBooks, codeNames, xsltArgs, inProcess);
            var sr = new StreamReader(fullName);
            var data = sr.ReadToEnd();
            sr.Close();
            File.Delete(fullName);
            Assert.AreEqual(1773, data.Split(new[] { '\n' }).Length);
            Mocks.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        [NUnit.Framework.Category("LongTest")]
        public void ProcessTestamentTest()
        {
            LoadMyXslt();
            IEnumerable<string> books = new List<string>(2) { "MAT", "MRK" };
            var codeNames = new Dictionary<string, string>(2);
            codeNames["MAT"] = FileInput(@"USX\040MAT.usx");
            codeNames["MRK"] = FileInput(@"USX\041MRK.usx");
            var xsltArgs = new XsltArgumentList();
            xsltArgs.AddParam("bookNames", "", "file://" + FileInput("BookNames.xml"));
            var temp = Path.GetTempFileName();
            var sw = new StreamWriter(temp);
            var inProcess = (IInProcess) Mocks.NewMock(typeof (IInProcess));
            Expect.Exactly(2).On(inProcess).Method("PerformStep");
            ProcessTestament(books, codeNames, xsltArgs, sw, inProcess);
            sw.Close();
            var sr = new StreamReader(temp);
            var data = sr.ReadToEnd();
            sr.Close();
            File.Delete(temp);
            Assert.AreEqual(1750, data.Split(new[] { '\n' }).Length);
            Mocks.VerifyAllExpectationsHaveBeenMet();
        }

        private static void LoadMyXslt()
        {
            var xsltSettings = new XsltSettings {EnableDocumentFunction = true};
            string codePath = PathPart.Bin(Environment.CurrentDirectory, "/../theWordConvert");
            var name = Common.PathCombine(codePath, "theWord.xsl");
            //var readerSettings = new XmlReaderSettings {XmlResolver = FileStreamXmlResolver.GetNullResolver()};
            var readerSettings = new XmlReaderSettings();
            var reader = XmlReader.Create(name, readerSettings);
            TheWord.Load(reader, xsltSettings, null);
        }

        private static void TestDataCase(string code, string fileName, int rec, string expectedResult, bool rtl)
        {
            TestDataCase(code, fileName, rec, expectedResult, null, null, false, rtl, null, null);
        }

        private static void TestDataCase(string code, string fileName, int rec, string expectedResult)
        {
            TestDataCase(code, fileName, rec, expectedResult, null, null, false, false, null, null);
        }

        private static void TestDataCase(string code, string fileName, int rec, string expectedResult, string bookNames, string punc)
        {
            TestDataCase(code, fileName, rec, expectedResult, bookNames, punc, false, false, null, null);
        }

        private static void TestDataCase(string code, string fileName, int rec, string expectedResult, string bookNames, string punc, string glossary, string extraMaterial)
        {
            TestDataCase(code, fileName, rec, expectedResult, bookNames, punc, false, false, null, extraMaterial);
        }

        private static void TestDataCase(string code, string fileName, int rec, string expectedResult, string bookNames, string punc, string glossary)
        {
            TestDataCase(code, fileName, rec, expectedResult, bookNames, punc, false, false, glossary, null);
        }

        private static void TestDataCase(string code, string fileName, int rec, string expectedResult, string bookNames, string punc, bool starSaltillo)
        {
            TestDataCase(code, fileName, rec, expectedResult, bookNames, punc, starSaltillo, false, null, null);
        }

        private static void TestDataCase(string code, string fileName, int rec, string expectedResult, string bookNames, string punc, bool starSaltillo, bool rtl, string glossary, string extraMaterial)
        {
            LoadMyXslt();
            IEnumerable<string> books = new List<string>(1) { code };
            var codeNames = new Dictionary<string, string>(2);
            codeNames[code] = FileInput(fileName);
            var xsltArgs = new XsltArgumentList();
            if (bookNames != null)
            {
                xsltArgs.AddParam("bookNames", "", bookNames);
            }
            else
            {
                xsltArgs.AddParam("bookNames", "", "file://" + FileInput("BookNames.xml"));
            }
            if (punc != null)
            {
                xsltArgs.AddParam("refPunc", "", punc);
            }
            if (starSaltillo)
            {
                xsltArgs.AddParam("noStar", "", "1");
                xsltArgs.AddParam("noSaltilla", "", "1");
            }
            if (rtl)
            {
                xsltArgs.AddParam("rtl", "", "1");
            }
            if (glossary != null)
            {
                xsltArgs.AddParam("glossary", "", glossary);
            }
            if (extraMaterial != null)
            {
                xsltArgs.AddParam("refMaterial", "", extraMaterial);
            }
            var temp = Path.GetTempFileName();
            var sw = new StreamWriter(temp);
            var inProcess = (IInProcess)Mocks.NewMock(typeof(IInProcess));
            Expect.Once.On(inProcess).Method("PerformStep");
            ProcessTestament(books, codeNames, xsltArgs, sw, inProcess);
            sw.Close();
            var sr = new StreamReader(temp);
            var data = string.Empty;
            for (int i = 0; i < rec; i++)
            {
                data = sr.ReadLine();
            }
            sr.Close();
            File.Delete(temp);
            Assert.AreEqual(expectedResult, data);
            Mocks.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void NoMt2()
        {
            var bookNames = "file://" + FileInput("avtBookNames.xml");
            TestDataCase("MAT", "040MAT.usx", 1, "<TS1><font color=teal>H\u0268m Yaaim Me Krais Matyu Kewis\u0268m Mau T\u0268wei</font><Ts><TS1>Niuk me maamrer ne weiw\u0268k me Jisas<Ts><TS3><i>(<a href=\"tw://bible.*?42.3.23-38\">Lu 3:23-38</a>)</i><Ts><RF q=*><a href=\"tw://bible.*?1.22.18\">Jen 22:18</a>; <a href=\"tw://bible.*?13.17.11\">1Kro 17:11</a><Rf>Menmen im hi hewis\u0268m h\u0268ram niuk me maamrer yap\u0268rwe ne weiw\u0268k miut\u0268p me Jisas Krais, kerek h\u0268rak nepenyek ke m\u0268t\u0268k iuwe Devit, h\u0268rak nepenyek hak ke maam n\u0268pu kaiu Ebraham.<CI>", bookNames, ":");
        }

        [Test]
        public void ItalicInFootnote()
        {
            TestDataCase("JHN", "043JHN.usx", 459, "<TS1>Jew Men Hitumatum Imih Jesu Hikwahir<Ts>Nati rarabkokou wanawanan Tafaror Bar ana hiyuw wabin Koksouwen<RF q=+>I baise <i>Hanukkah</i> teo<Rf> i Jerusalemamaim hibogaigiwas.");
        }

        [Test]
        public void WordsOfJesus()
        {
            TestDataCase("REV", "066REV.usx", 395, "Higeno Jisasi'a anage hu'ne, <font color=red>Antahiho, Nagra ame hu'na mizana eri'nena egahuankina, maka vahe'mo'zama nehanaza zmavuzmavate mizana eme zamigahue.</font>");
        }

        [Test]
        public void NdTest()
        {
            TestDataCase("EZR", "015EZR.usx", 1, "<TS1><font color=teal>Ezra</font><Ts><TS1>Cyrus Helps the Exiles to Return<Ts>In the first year of Cyrus king of Persia, in order to fulfill the word of the <font color=green>Lord</font> spoken by Jeremiah, the <font color=green>Lord</font> moved the heart of Cyrus king of Persia to make a proclamation throughout his realm and to put it in writing:<CM>");
        }

        [Test]
        public void TableWithFootnote()
        {
            TestDataCase("EZR", "015EZR.usx", 9, "This was the inventory:<CL><CL>gold dishes \u2014 30<CL>silver dishes \u2014 1,000<CL>silver pans<RF q=+>The meaning of the Hebrew for this word is uncertain.<Rf> \u2014 29<CL>");
        }

        [Test]
        public void TableLineVerse()
        {
            TestDataCase("EZR", "015EZR.usx", 15, "of Shephatiah \u2014 372<CL>");
        }

        [Test]
        public void OptionalBreakTest()
        {
            TestDataCase("EZR", "015EZR.usx", 53, "The gatekeepers of the temple:<CL><CL>the descendants of Shallum, Ater, Talmon, Akkub, Hatita and Shobai \u2014 139<CM>");
        }

        [Test]
        public void BullettedListTest()
        {
            TestDataCase("EZR", "015EZR.usx", 54, "The temple servants:<CL><CI><PI>\u2022 the descendants of<CI><PI2>\u2022 Ziha, Hasupha, Tabbaoth,<CI>");
        }

        [Test]
        public void SubsequentVerseInBulletParaTest()
        {
            TestDataCase("GEN", "001GEN.usx", 4, "God saw that the light was good, and he separated the light from the darkness.");
        }

        [Test]
        public void S2Test()
        {
            TestDataCase("GEN", "001GEN.usx", 237, "<TS1><font size=-1>The Japhethites</font><Ts><PI>\u2022 The sons<RF q=+><i>Sons</i> may mean <i>descendants</i> or <i>successors</i> or <i>nations;</i> also in verses 3, 4, 6, 7, 20-23, 29 and 31.<Rf> of Japheth:<CI><PI2>\u2022 Gomer, Magog, Madai, Javan, Tubal, Meshech and Tiras.<CI>");
        }

        [Test]
        public void Li3Test()
        {
            TestDataCase("GEN", "001GEN.usx", 1399, "<PI>\u2022 The sons of Judah:<CI><PI2>\u2022 Er, Onan, Shelah, Perez and Zerah (but Er and Onan had died in the land of Canaan).<CI>\u2022 The sons of Perez:<CI>\u2022 Hezron and Hamul.<CI>");
        }

        [Test]
        public void Ms1MrTest()
        {
            TestDataCase("PSA", "019PSA.usx", 1, "<TS1><font color=teal>Psalms</font><Ts><TS1>BOOK I<Ts><TS3><i>Psalms 1-41</i><Ts><PI>Blessed is the man<CI><PI2>who does not walk in the counsel of the wicked<CI><PI>or stand in the way of sinners<CI><PI2>or sit in the seat of mockers.<CI>");
        }

        [Test]
        public void DescriptiveIntroTest()
        {
            TestDataCase("PSA", "019PSA.usx", 19, "<TS3><i>A psalm of David. When he fled from his son Absalom.</i><Ts><PI>O <font color=green>Lord</font>, how many are my foes!<CI><PI2>How many rise up against me!<CI>");
        }

        [Test]
        public void PoetryLetterIntroTest()
        {
            TestDataCase("PSA", "019PSA.usx", 1960, "<TS3><i>Aleph</i><Ts><PI>Blessed are they whose ways are blameless,<CI><PI2>who walk according to the law of the <font color=green>Lord</font>.<CI>");
        }

        [Test]
        public void SpeakerTest()
        {
            TestDataCase("SNG", "022SNG.usx", 4, "<PI>Take me away with you\u2013let us hurry!<CI><PI2>Let the king bring me into his chambers.<CL><CM><PI0>Friends<CL><CI><PI>We rejoice and delight in you;<RF q=+>The Hebrew is masculine singular.<Rf><CI><PI2>we will praise your love more than wine.<CL><CM><PI0>Beloved<CL><CI><PI>How right they are to adore you!<CL><CI>");
        }

        [Test]
        public void SpaceInCrossRefNameTest()
        {
            var bookNames = "file://" + FileInput("aauBookNames.xml");
            TestDataCase("PHM", "057PHM.usx", 2, "Hrorkwe mamey okukwe seyr, hromo ine Apia o, hromo wayh Arkipus, hrere nion ma non-orok ono o, seyr uwrsa sios ko, hno a mon ma hokruw sohom o, hme mey kow.<RF q=*><a href=\"tw://bible.*?51.4.17\">Kol 4:17</a>; <a href=\"tw://bible.*?55.2.3\">2 Ti 2:3</a><Rf><CM>", bookNames, ":");
        }

        [Test]
        public void WithoutBookNameTest()
        {
            var bookNames = "file://" + FileInput("aauBookNames.xml");
            TestDataCase("PHM", "057PHM.usx", 22, "Hano me-nonkway ok kamon kokwe senkin, hunkwe ampok, ha liawon ey se kwa lonhan naruok koruay. Payhokuaw, hano uron hokwe hyohyo senkin lon, hom God se ma mesopok me hokuaw, God hiykwe ha hme nion manon-wak e ipan ya lon a.<RF q=*><a href=\"tw://bible.*?50.1.25\">Fl 1:25</a>; <a href=\"tw://bible.*?50.2.24\">2:24</a><Rf><CM>", bookNames, ":");
        }

        [Test]
        public void Verse1BridgeTest()
        {
            TestDataCase("PHM", "0552TI.usx", 1, "<TS1><font color=teal>Timoti \uA78Cina leta heluwena Paulo \uA78Cileleya</font><Ts><TS1>Loyauwedo<Ts><sup>(1-2)</sup> Yauwedo Timoti \uA78Cowa natugu moisa, \uA78Cino leta bewa taugu Paulo \uA78Coiguwega. Taugu Yesu Keliso \uA78Cana tohepwaila, Yehoba \uA78Cina nuwatuhu gide ta yahepwaila yawasida bwebwe\uA78Cana weyahina, beno Yesu Keliso \uA78Coinega. Tamada Yehoba ma \uA78Cida Bada Yesu Keliso sihelauwego ta si\uA78Catemuyamuyaego ma \uA78Coinega \uA78Cumiya daumwala.");
        }

        [Test]
        public void BridgeB4PTest()
        {
            TestDataCase("MAT", "040MAT-bridgeB4.usx", 1, "<sup>(1-2)</sup> Verse one text. Verse two text");
        }

        [Test]
        public void BridgeB4P2Test()
        {
            TestDataCase("MAT", "040MAT-bridgeB4P.usx", 1, "<sup>(1-2)</sup> Verse one text. Verse two text");
        }

		[Test]
		public void BridgeEndPTest()
		{
			TestDataCase("1PE", "1PE-bridge.usx", 3, "(-)<CM>");
		}

		[Test]
		public void BridgeEndQTest()
		{
			TestDataCase("1PE", "1PE-bridge.usx", 28, "(-)<CI>");
		}

		[Test]
		public void BridgeMidPTest()
		{
			TestDataCase("1PE", "1PE-bridge.usx", 19, "(-)");
		}

		[Test]
        public void RefListTest()
        {
            var bookNames = "file://" + FileInput("BookNames-refList.xml");
            TestDataCase("MAT", "040MAT-refList.usx", 26, "<TS1>complex xref test<Ts><RF q=*><a href=\"tw://bible.*?19.2.7\">Pal 2:7</a>; <a href=\"tw://bible.*?19.22.2\">22:2</a>, <a href=\"tw://bible.*?19.22.14\">14</a><Rf>Verse one.", bookNames, ":");
        }

        [Test]
        public void RefList2Test()
        {
            var bookNames = "file://" + FileInput("BookNames-refList.xml");
            TestDataCase("MAT", "040MAT-refList.usx", 27, "<RF q=*><a href=\"tw://bible.*?2.37.11\">Tav 37:11</a>, <a href=\"tw://bible.*?2.37.28\">28</a>; <a href=\"tw://bible.*?2.39.2\">39:2</a>, <a href=\"tw://bible.*?2.39.21-23\">21-23</a><Rf>Verse two.", bookNames, ":");
        }

        [Test]
        public void ExtraMaterialTest()
        {
            var bookNames = "file://" + FileInput("msbBookNames.xml");
            TestDataCase("MAT", "msbMAT.usx", 23, "<PI>“Tandai, may birhen na magabudos tapos magaanak sin lalaki na pagatawagon Emmanuel (na an gusto sabihon, ‘An Dios adi sa aton.’)”<RF q=+>Kitaa sa <a href=\"tw://bible.*?23.7.14\">Isaias 7:14</a><Rf><CM>", bookNames, ":", null, "Kitaa sa ");
        }

		[Test]
		public void ExtraMaterialInsideTest()
		{
			var bookNames = "file://" + FileInput("sgbBookNames.xml");
			TestDataCase("MAT", "sgbMAT.usx", 1, "<TS1><font color=teal>Ya Mangêd ya Habi tungkol kan Apo Jesu-Cristo ya inhulat ni apostol Mateo</font><Ts>Daygên tamon alimbawa hi apo Abraham. Habaytsi ya impahulat ni Apo Namalyari ya tungkol kana, “Naniwala hi apo Abraham ha hinabi ni Apo Namalyari, kabay intad ya ni Apo Namalyari hên ayn kasalanan.”<RF q=+><a href=\"tw://bible.*?1.15.6\">Genesis 15:6</a><Rf> “Gawan kamo bansa.”<RF q=+><a href=\"tw://bible.*?1.12.3b\">Genesis 12:3b</a>; <a href=\"tw://bible.*?1.18.18\">18:18</a>; haka <a href=\"tw://bible.*?1.22.18\">22:18</a><Rf> ‘Bat la kon kinahêmêkan.’”<RF q=+><a href=\"tw://bible.*?19.35.19\">Awit 35:19</a>; haka <a href=\"tw://bible.*?19.69.4\">Awit 69:4</a><Rf> ay miligtas.<RF q=+><a href=\"tw://bible.*?1.6.9\">Genesis 6:9</a>; angga ha <a href=\"tw://bible.*?1.8.22\">8:22</a><Rf> habaytoy anlugurên la.<RF q=+><a href=\"tw://bible.*?11.17.19-24\">1 Hari 17:19-24</a>; haka <a href=\"tw://bible.*?12.4.18-37\">2 Hari 4:18-37</a><Rf>", bookNames, ":", null, "haka |angga ha");
		}

		[Test]
        public void RefSingleChapterTest()
        {
            var bookNames = "file://" + FileInput("BookNames-refList.xml");
            TestDataCase("MAT", "040MAT-refList.usx", 30, "<RF q=*><a href=\"tw://bible.*?1.18.20\u201319:28\">Jen 18:20\u201319:28</a>; <a href=\"tw://bible.*?40.11.24\">Mt 11:24</a>; <a href=\"tw://bible.*?61.2.6\">2Pi 2:6</a>; <a href=\"tw://bible.*?65.1.7\">Ju 7</a><Rf>Hi hetpi werek. Maain w\u0268 God skelim m\u0268t, h\u0268rak God kaknep m\u0268t miyap\u0268r enun n\u0268paa nau Sodom ketike wit Gomora kakn\u0268p kike. Te m\u0268t miyap\u0268r kerek nanweik\u0268n sip nanwet h\u0268m mi, maain God kakn\u0268p iuwe kaknepi.", bookNames, ":");
        }

        [Test]
        public void StarSaltilloTest()
        {
            var bookNames = "file://" + FileInput("BookNames-refList.xml");
            TestDataCase("MAT", "040MAT-refList.usx", 31, "<i>Judá tusha chumu, ñu Belén pebulu,</i><CI><i>vee mujtu aa pebulu chumulaba buute\uA78C pensangue keeñu, ne balejtuu pebulu jutyuve;</i><CI><i>matyu ñu junuren main bale chachi fale,</i><CI><i>kumuinchi in Israel chachillanu washkenu juñu mitya,</i><RF q=+>Miqueas 5.2<Rf> ti pillave, tila bale rukula.", bookNames, ":", true);
        }

        [Test]
        public void RefNoVerseTest()
        {
            var bookNames = "file://" + FileInput("BookNames-refList.xml");
            TestDataCase("MAT", "040MAT-refList.usx", 47, "<RF q=*><a href=\"tw://bible.*?23.23.1\">Ais 23</a>; <a href=\"tw://bible.*?26.26.1\u201328:26\">Esi 26:1\u201328:26</a>; <a href=\"tw://bible.*?29.3.4-8\">Joe 3:4-8</a>; <a href=\"tw://bible.*?30.1.9-10\">Emo 1:9-10</a>; <a href=\"tw://bible.*?36.9.2-4\">Sek 9:2-4</a><Rf>Hi hetpi werek. Maain w\u0268 kerek God skelim m\u0268t, h\u0268rak kaknep m\u0268t ne Taia netike m\u0268t ne Saidon kakn\u0268p kike, te yi m\u0268t au h\u0268rak kakiwep iuwe.", bookNames, ":");
        }

        [Test]
        public void ShortNameTest()
        {
            var bookNames = "file://" + FileInput("akeBookNames.xml");
            TestDataCase("1CO", "0461CO.usx", 2, "Papa so\uA78Csii, Koren pon enakan, kamoro K\u0289rai Sises win\u0268 iyekonekasa\uA78C kon wak\u0289 pe te\uA78Cton kon pe, kamoro m\u0268 aw\u0268r\u0268 na\uA78Cne\uA78C nan am\u0289t\u0289 pe esii\uA78Cma Sises K\u0289rai, uyepuru kon esak\u0289 p\u0268\uA78C na\uA78Cne\uA78C nan, to\uA78C epuru m\u0268r\u0268 awonsi\uA78Ck\u0268 uyepuru kon n\u0268 n\u0268r\u0268:<RF q=*><a href=\"tw://bible.*?44.18.1\">Inkup\u0289\uA78Cp\u0289 18:1</a><Rf>", bookNames, ":");
        }

        [Test]
        public void SpaceAfterRefTest()
        {
            var bookNames = "file://" + FileInput("aauBookNames.xml");
            TestDataCase("MRK", "041MRK.usx", 647, "Hmo prueyn hiy laplap kopi non nak-sau nok nok, wain ma laroray non sakeyn prouk nok, now-ho mon piynay nok, sa Jisas se seyn arnak-nakray, hiy lowswa e.<RF q=*><a href=\"tw://bible.*?19.69.21\">Sng 69:21</a><Rf> Uwr sohiy nak-me, \u201CPereipia, hromkwe lira ey, Elaija po pankaw laye pakane, hye now ko se kandieys kow se.\u201D", bookNames, ":");
        }

        [Test]
        public void RightToLeftBridgeTest()
        {
            TestDataCase("3JN", "0643JN.usx", 14, "<sup>(<rtl>14</rtl>-15)</sup> \u0628\u064e\u0633\u0652 \u0644\u064a \u0631\u064e\u062c\u0627 \u062e\u064e\u0641\u064a\u0641\u0652 \u062a\u064e \u0627\u0631\u0627\u0643\u060c \u0648\u0652\u062b\u0645\u0652 \u0644\u064e\u062b\u0645\u0652 \u0646\u062d\u0652\u0643\u064a. <sup>15</sup> \u0627\u0644\u0633\u0651\u064e\u0644\u0627\u0645 \u064a\u0643\u0648\u0646\u0652 \u0645\u064e\u0639\u0643. \u064a\u0633\u064e\u0644\u0645\u0648\u0646\u0652 \u0639\u064e\u0644\u064e\u064a\u0643 \u0627\u0644\u0651\u0645\u0652\u062d\u0628\u0651\u064a\u0646\u0652. \u0633\u064e\u0644\u0651\u0645 \u0639\u064e \u0627\u0644\u0651\u0645\u0652\u062d\u0628\u0651\u064a\u0646\u0652 \u0643\u0644\u0652 \u0648\u0627\u0650\u062d\u062f\u0652 \u0628\u0627\u0633\u0652\u0645\u0648.<CM>", true);
        }

        [Test]
        public void Mt2B4Mt1Test()
        {
            TestDataCase("GAL", "048GAL.usx", 1, "<TS2><font color=teal size=-1><b>To Suyat ni Pablo diya to mgo Taga-</b></font><Ts><TS1><font color=teal>GALACIA</font><Ts><sup>(1,2)</sup> Siak si Pablo iyan migsuyat to seini diyan iyu no mgo magtutuu no oghihimun duon to mgo kayunsudan no sakup to Galacia.<CM>Igpadomdom ku iyu no seini katongdanan ku to pagka-apustul, kona no otow to migpili dow migsugu kanay ko kona no si Jesu-Cristo yagboy dow to Diyus no Amoy no iyan migbanhaw kandin.<CM>Siak dow to tibo mgo suun ta kani duon ki Cristo nangumusta iyu.");
        }

        [Test]
        public void VerseAbBridgeTest()
        {
            TestDataCase("MAT", "040MATpvt.usx", 1, "<sup>(1a-1b)</sup> Mamey okukwe Jisas Krais so nopwey-om me ma mey hok non. Jisas Krais hiykwe Devit so ney-nona. <sup>1b</sup> Devit hiykwe Abraham so ney-nona.<CI>");
        }

        [Test]
        public void VerseBridgeInsideParaTest()
        {
            TestDataCase("MAT", "040MATtvb.usx", 4, "<sup>(4-5)</sup> Dungure Yesu maan iru di tongwa, \u201COmilin gi dungwa ibal pilaa dire kanungure, te kawn kebil sungwa u wai pire kol warungure, te seki egilungwa ibal gain wiige sungure, te kiraan gi dungwa ibal kwi pilaa dire ka pirungure, te gulungwa ibal alere u kwi pire mile paingure, te kal aa te nekungwa ibal na guunan kanan pirungure, kal iru erungwa main i kane pire, pi Yon di tenana po.");
        }

        [Test]
        public void FootnoteEmbeddedItalicTest()
        {
            TestDataCase("GEN", "001GENheg.usx", 23, "Net bihatang na kon, atuling nga tek noan,<CI><PI>\u201cEli le! Ni halas-sam tom nol au!<CI><PI2>Un seen na banansila el auk seen ni kon.<CI>Nol un sisin na kon banansila el auk sising ngia.<CI>Undeng un daid deng biklobe lia, tiata auk ngali un ngala ka noan \u2018bihata\u2019.\u201d<RF q=+>Se dais Ibranin nam, biklobe li noken \u2018<i>ish</i>\u2019. Nol bihata li \u2018<i>isha</i>\u2019. Tiata se nia, oen kuti dais noan \u201c<i>isha</i> (bihata) daid deng <i>ish</i> (biklobe).\u201d<Rf><CI>");
        }

		[Test]
		public void EmbeddedMarkerTest()
		{
            TestDataCase("MAT", "TnmMAT.usx", 1, "Regular text <b>blah <font color=green>BLAH</font><RF q=+><font color=green>BLAH</font> should be typed differently<Rf> blah.</b>.");
		}

        [Test]
        public void EmbeddedDcMarkerTest()
        {
            TestDataCase("MAT", "TnmMAT.usx", 2, "<i>Deuterocanonical addition</i>");
        }

        [Test]
		public void EmbeddedOrdMarkerTest()
		{
			TestDataCase("MAT", "TnmMAT.usx", 3, "The 1 <sup>st</sup> first item. The 2 <sup>nd</sup> second item.");
		}

		[Test]
		public void EmbeddedPnMarkerTest()
		{
			TestDataCase("MAT", "TnmMAT.usx", 4, "<b>Greg</b> was here.<CI>");
		}

		[Test]
		public void EmbeddedQtLitMarkerTest()
		{
			TestDataCase("MAT", "TnmMAT.usx", 5, "<PI>Roses are red<CI>Violets are blue<CI><PI2>• Selah<CM>");
		}

		[Test]
		public void EmbeddedSlsMarkerTest()
		{
			TestDataCase("MAT", "TnmMAT.usx", 7, "<i>E plurabus unim</i><CM>");
		}

		[Test]
		public void EmbeddedTlMarkerTest()
		{
			TestDataCase("MAT", "TnmMAT.usx", 8, "<i>kwa fair</i>");
		}

		[Test]
		public void EmbeddedEmBdItBditNoScOptBreakMarkerTest()
		{
			TestDataCase("MAT", "TnmMAT.usx", 10, "<i>I insist .</i> <b>Do it now.</b> <i>If not, there will be consequences.</i> <b><i>Don't test me.</i></b> or you will find out what happens. <font size=-1>you have been warned.</font> 1 Cor. 3:17 So we say");
		}

		[Test]
        public void MultiWordBookNameTest()
        {
            var bookNames = "file://" + FileInput("hegBookNames.xml");
            TestDataCase("GEN", "001GENheg2.usx", 4, "<sup>(4a-4b)</sup> Ama Lamtua Allah in koet apan-dapa kua nol apan-kloma kia ka, un dehet ta ela.<CM> <sup>4b</sup> <TS1>Ama Lamtua Allah koet biklobe nol bihata<Ts><TS3><i>(<a href=\"tw://bible.*?40.19:4-6.1\">Matius 19:4-6</a>; <a href=\"tw://bible.*?41.10:4-9.1\">Markus 10:4-9</a>; <a href=\"tw://bible.*?46.6:16.1\">Korintus mesa la 6:16</a>; <a href=\"tw://bible.*?46.15:45.1\">15:45</a>, <a href=\"tw://bible.*?46.15:45.47\">47</a>; <a href=\"tw://bible.*?49.5:31-33.1\">Efesus 5:31-33</a>)</i><Ts>Dedeng AMA LAMTUA Allah halas-sam mana le koet apan-dapa ku nol apan-kloma kia ka,", bookNames, ".");
        }

        [Test]
        public void Glossary()
        {
            var bookNames = "file://" + FileInput("nkoBookNames.xml");
            var glossary = "file://" + FileInput("NKOXXA.usx");
            TestDataCase("JHN", "NKOJHN.usx", 21, "Bɛfɩtɛ́ mʋ bɛɛ, “Mʋ́mʋ́ fʋ́gyí ma? Fʋ́gyí <font color=blue>Elia</font><RF q=*> <i>Elijah</i> Bulu ɔnɔ́sʋ́ ɔtɔɩ́pʋ́ kpɔnkpɔɔnkpɔntɩ ɔkʋ ogyi. Mʋlɔ́wa klʋn ká awíe laláhɛ pʋ́ ahá ánɩ́ bʋtosúm ɩkpɩ ansɩ́tɔ́ Israel ɔmátɔ́ nɩ́. Yudafɔ botosúsu bɛɛ, Elia obéyinkí bá bɛla ahá yáɩ́ asa Kristo amʋ ɔbɛ́ba. Asú Ɔbɔpʋ́ Yohane lɛ́lɩan Elia anfɩ ɔkpa tsɔtsɔɔtsɔsʋ. Mʋ́ sʋ bɛfɩtɛ́ mʋ bɛɛ, “Fʋ́gyí Elia nɩ?” (Mat. 17:10-13; Mar. 9:4; Rom. 11:2-5; Yak. 5:17) <Rf>?”<CM>Ɔlɛlɛ mʋ́ ɔnɔ́ ɔbɛ́ɛ, “Megyí mɩ́gyí Elia.”<CM>Bɛtrá fɩtɛ́ mʋ bɛɛ, “Fʋ́gyí Bulu ɔnɔ́sʋ́ ɔtɔɩ́pʋ́ amʋ́ʋ́ ɔbɛ́ba amʋ?”<CM>Ɔlɛlɛ mʋ́ ɔnɔ́ ɔbɛ́ɛ, “Ó-o.”<CM>", bookNames, ".", glossary);
        }

        [Test]
        public void GlossaryWithRoot()
        {
            var bookNames = "file://" + FileInput("nkoBookNames.xml");
            var glossary = "file://" + FileInput("NKOXXA.usx");
            TestDataCase("JHN", "NKOJHN.usx", 19, "<TS1>Asú Ɔbɔpʋ́ Yohane Bulu Asʋ́n Ɔkanda<Ts><TS3><i>(<a href=\"tw://bible.*?40.3:1-12.1\">Mateo 3:1-12</a>; <a href=\"tw://bible.*?41.1:1-8.1\">Marko 1:1-8</a>; <a href=\"tw://bible.*?42.3:1-18.1\">Luka 3:1-18</a>)</i><Ts><sup>(19-20)</sup> <font color=blue>Yudafɔ</font><RF q=*> <i>Joseph</i> Bɛtɩ ahá ɔtsan-ɔtsan Yosef Ntam Pɔpwɛ anfɩtɔ: <Rf> ahandɛ ánɩ́ bʋbʋ Yerusalem bɔwa Bulu igyí ahapʋ́ pʋ́ Lewifɔ Yohane wá bɛɛ bʋyɛ́fɩtɛ́ mʋ bɛɛ, ma ogyi? Yohane mési asʋansʋ ŋáín amʋ́. Ɔlɛlɛ bláa amʋ́ ɔbɛ́ɛ, “Megyí mɩ́gyí Kristo, (ɔhá ámʋ́ʋ́ Bulu ladá mʋ ofúli amʋ).”", bookNames, ".", glossary);
        }

        /// <summary>
        ///A test for AttachMetadata
        ///</summary>
        [Test]
        public void AttachMetadataTest()
        {
            Common.Testing = true;      // Don't write settings to settings folder.
            var inputParams = Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.ScriptureStyleSettingsCopy.xml");
            Debug.Assert(inputParams != null);
            Param.LoadValues(inputParams);
            var memory = new MemoryStream();
            var sw = new StreamWriter(memory, Encoding.UTF8);
            AttachMetadata(sw);
            sw.Flush();
            memory.Position = 0;
            var sr = new StreamReader(memory);
            var data = sr.ReadToEnd();
            sr.Close();
            sw.Close();
            var lines = data.Split('\n');
            Assert.AreEqual(24, lines.Length);
        }

        /// <summary>
        ///A Bad Converter Path test for ConvertToMySword
        ///</summary>
        [Test]
        public void ConvertToMySwordBadConvertPathTest()
        {
            string resultName = string.Empty;
            string tempTheWordCreatorPath = string.Empty;
            string exportTheWordInputPath = string.Empty;
            // Throws Win32Exception on Windows, ArgumentException on Ubuntu
            try
            {
                //var actual =
                ConvertToMySword(resultName, tempTheWordCreatorPath, exportTheWordInputPath);
            }
            catch (Win32Exception) // Windows
            {
            }
            catch (ArgumentException) // Ubuntu
            {
            }
            //Assert.Throws(typeof (Win32Exception), delegate
            //    {
            //        //var actual =
            //        ConvertToMySword(resultName, tempTheWordCreatorPath, exportTheWordInputPath);
            //    });
            //Assert.AreEqual("<No MySword Result>", actual);
        }

        /// <summary>
        ///A No Output test for ConvertToMySword
        ///</summary>
        [Test]
        public void ConvertToMySwordNoOutputTest()
        {
            string resultName = string.Empty;
            string tempTheWordCreatorPath = _converterPath;
            string exportTheWordInputPath = string.Empty;
            var actual = ConvertToMySword(resultName, tempTheWordCreatorPath, exportTheWordInputPath);
            Assert.AreEqual("<No MySword Result>", actual);
        }

        /// <summary>
        ///A test for ConvertToMySword
        ///</summary>
        [Test]
        [NUnit.Framework.Category("LongTest")]
        [NUnit.Framework.Category("SkipOnTeamCity")]
        public void ConvertToMySwordTest()
        {
            const string vrsName = "vrs.xml";
            var pathPat = new Regex(@"(.*)[\\/]Test([\\/][A-Za-z]+)[\\/]TestFiles[\\/]Input", RegexOptions.IgnoreCase);
            var match = pathPat.Match(_inputPath);
            var inVrs = Path.Combine(match.Groups[1].Value + match.Groups[2].Value, vrsName);
            var exportTheWordAssembly = Assembly.GetAssembly(typeof (ExportTheWord));
            var outPath = Path.GetDirectoryName(exportTheWordAssembly.Location);
            Debug.Assert(!string.IsNullOrEmpty(outPath));
            File.Copy(inVrs, Path.Combine(outPath, vrsName), true);
            const string nkont = "nko.nt";
            var outName = Path.Combine(Path.Combine(_outputPath, "TheWord"), nkont);
            File.Copy(Path.Combine(_inputPath, nkont), outName, true); // overwrite
            const string resultName = nkont;
            string tempTheWordCreatorPath = _converterPath;
            string exportTheWordInputPath = _outputPath;
            var actual = ConvertToMySword(resultName, tempTheWordCreatorPath, exportTheWordInputPath);
            Assert.AreEqual(Path.Combine(_outputPath, "nko.bbl.mybible"), actual);
            Assert.True(File.Exists(actual));
            if (File.Exists(actual))
            {
                File.Delete(actual);
            }
        }

        /// <summary>
        ///A test for CopyTheWordFolderToTemp
        ///</summary>
        [Test]
        public void CopyTheWordFolderToTempTest()
        {
            const string theWord = "TheWord";
            string sourceFolder = Path.Combine(Common.ProgBase, theWord);
            string destFolder = Path.Combine(Path.GetTempPath(), theWord);
            CopyTheWordFolderToTemp(sourceFolder, destFolder);
            Assert.AreEqual(1, new DirectoryInfo(destFolder).GetFiles().Length);
            Directory.Delete(destFolder, true); // Recurse and delete all sub folders too
        }

        /// <summary>
        ///A test for CreateRAMP
        ///</summary>
        [Test]
        public void CreateRampTest()
        {
            Common.Testing = true;
            var projInfo = (IPublicationInformation) Mocks.NewMock(typeof (IPublicationInformation));
            Expect.Once.On(projInfo).GetProperty("DefaultXhtmlFileWithPath").Will(Return.Value(_outputPath));
            Expect.Once.On(projInfo).GetProperty("ProjectInputType").Will(Return.Value("Scripture"));
            CreateRamp(projInfo);
            Mocks.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        ///A test for DisplayMessageReport
        ///</summary>
        [Test]
        public void DisplayMessageReportTest()
        {
            Common.Testing = true;
            DisplayMessageReport();
        }

        /// <summary>
        ///A test for Export
        ///</summary>
        [Test]
        [NUnit.Framework.Category("LongTest")]
        [NUnit.Framework.Category("SkipOnTeamCity")]
        public void ExportTest()
        {
            Common.Testing = true;
            var projInfo = new PublicationInformation();
            var vrsPath = PathPart.Bin(Environment.CurrentDirectory, @"/../theWordConvert");
            VrsName = Path.Combine(vrsPath, "vrs.xml");
            projInfo.DefaultXhtmlFileWithPath = Path.Combine(_outputPath, "name.xhtml"); //Directory name used as output folder
            Common.Ssf = Path.Combine(_inputPath, "nkoNT.ssf"); // Ssf file used for Paratext settings
            const string usxFolder = "USX"; //USX folder must be present for input
            FolderTree.Copy(Path.Combine(_inputPath, usxFolder), Path.Combine(_outputPath, usxFolder));
            var target = new ExportTheWord();
            bool actual = target.Export(projInfo);
            Assert.True(actual);
        }

        /// <summary>
        ///A test for Launch
        ///</summary>
        [Test]
        public void LaunchTest()
        {
            Common.Testing = true;
            const string exportType = "theWord/mySword";
            var publicationInformation = new PublicationInformation();
            var target = new ExportTheWord();
            CommonTestMethod.DisableDebugAsserts();
            bool actual = target.Launch(exportType, publicationInformation);
            Assert.False(actual);
            CommonTestMethod.EnableDebugAsserts();
        }

        /// <summary>
        ///A test for FindParatextProject
        ///</summary>
        [Test]
        public void FindParatextProjectTest()
        {
            Common.FindParatextProject();
            Assert.True(string.IsNullOrEmpty(Common.Ssf));  // Since we are not running from Paratext or PathwayB
		}

        /// <summary>
        ///A test for GetBookNamesUri
        ///</summary>
        [Test]
        public void GetBookNamesUriTest()
        {
			Common.ParatextData = null;
			Common.Ssf = "";
            string expected = FileUrlPrefix + Path.Combine(_inputPath, Path.Combine("USX", "BookNames.xml"));
            string actual = GetBookNamesUri(_inputPath);
            Assert.AreEqual(expected, actual);
       }

        /// <summary>
        ///A test for GetFormat
        ///</summary>
        [Test]
        public void GetFormatTest()
        {
            Common.Testing = true;
            string thewordformatTxt = string.Empty;
            string format = string.Empty;
            string actual = GetFormat(thewordformatTxt, format);
            Assert.AreEqual(string.Empty, actual);
        }

        /// <summary>
        ///A test for GetRtlParam
        ///</summary>
        [Test]
        public void GetRtlParamTest()
        {
            const XsltArgumentList xsltArgs = null;
            Common.Ssf = Path.Combine(_inputPath, "nkoNT.ssf");
            // This test uses English.LDS in the input testfiles.
            GetRtlParam(xsltArgs);
            Assert.False(R2L);
            Common.Ssf = string.Empty;
        }

        /// <summary>
        ///A test for GetSsfValue
        ///</summary>
        [Test]
        public void GetSsfValueTest()
        {
            const string xpath = "//EthnologueCode";
            const string def = "zxx"; // Default
            const string expected = "zxx";
			string ssf = string.Empty;
			string actual = Common.GetSsfValue(xpath, def);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetSsfValue
        ///</summary>
        [Test]
        public void GetSsfValueTest1()
        {
            const string xpath = "//Name";
			string ssf = string.Empty;
			string actual = Common.GetSsfValue(xpath);
            Assert.Null(actual);
        }

        /// <summary>
        ///A test for LaunchFileNavigator
        ///</summary>
        [Test]
        public void LaunchFileNavigatorTest()
        {
            Common.Testing = true;
            string exportTheWordInputPath = string.Empty;
            LaunchFileNavigator(exportTheWordInputPath);
            Assert.AreEqual(0, SubProcess.ExitCode);
        }

        /// <summary>
        ///A test for LoadMetadata
        ///</summary>
        [Test]
        public void LoadMetadataTest()
        {
            Common.Testing = true;      // Don't write settings to settings folder.
            var inputParams = Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.ScriptureStyleSettingsCopy.xml");
            Debug.Assert(inputParams != null);
            Param.LoadValues(inputParams);
            LoadMetadata();
            Assert.Greater(Param.Value.Count, 0);
        }

        /// <summary>
        ///A test for LogStatus
        ///</summary>
        [Test]
        public void LogStatusTest()
        {
            var savedOut = Console.Out;
            var memory = new MemoryStream();
            var writer = new StreamWriter(memory);
            Console.SetOut(writer);
            const string format = "Processing: {0}";
            object[] args = { "MyName.txt" };
            Verbosity = 0;
            LogStatus(format, args);
            writer.Flush();
            memory.Flush();
            Assert.AreEqual(0, memory.Length);
            Console.SetOut(savedOut);
            writer.Close();
        }

        /// <summary>
        ///A test for LogStatus
        ///</summary>
        [Test]
        public void LogStatusTest1()
        {
            var savedOut = Console.Out;
            var memory = new MemoryStream();
            var writer = new StreamWriter(memory);
            Console.SetOut(writer);
            const string format = "Processing: {0}";
            object[] args = { "MyName.txt" };
            Verbosity = 1;
            LogStatus(format, args);
            writer.Flush();
            memory.Flush();
            memory.Position = 0;
            var sr = new StreamReader(memory);
            var data = sr.ReadToEnd();
            sr.Close();
            writer.Close();
            Assert.AreEqual(24, data.Trim().Length);
            Console.SetOut(savedOut);
            writer.Close();
            Verbosity = 0;
        }

        /// <summary>
        ///A test for ReportFailure
        ///</summary>
        [Test]
        public void ReportFailureTest()
        {
            Common.Testing = true;
            Exception ex = new ArgumentException("Test Message");
            ReportFailure(ex);
        }

        /// <summary>
        ///A test for ReportResults
        ///</summary>
        [Test]
        public void ReportResultsTest()
        {
            Common.Testing = true;
            const string nsont = "nko.nt";
            var resultFullName = Path.Combine(_outputPath, nsont);
            File.Copy(Path.Combine(_inputPath, nsont), resultFullName, true);
            var mySwordResult = Path.Combine(_outputPath, "nko.bbl.mysword");
            var exportTheWordInputPath = Path.Combine(Common.ProgInstall, "TheWord");
            const bool expected = true;
            bool actual = ReportResults(resultFullName, mySwordResult, exportTheWordInputPath);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ReportWhenTheWordInstalled
        ///</summary>
        [Test]
        public void ReportWhenTheWordInstalledTest()
        {
            Common.Testing = true;
            const string nsont = "nko.nt";
            var resultFullName = Path.Combine(_outputPath, nsont);
            File.Copy(Path.Combine(_inputPath, nsont), resultFullName, true);
            string theWordFolder =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "The Word");
            string mySwordResult = Path.Combine(_outputPath, "nko.bbl.mysword");
            string exportTheWordInputPath = Path.Combine(Common.ProgInstall, "TheWord");
            ReportWhenTheWordInstalled(resultFullName, theWordFolder, mySwordResult, exportTheWordInputPath);
        }

        /// <summary>
        ///A test for ReportWhenTheWordNotInstalled
        ///</summary>
        [Test]
        public void ReportWhenTheWordNotInstalledTest()
        {
            Common.Testing = true;
            const string nsont = "nko.nt";
            var resultFullName = Path.Combine(_outputPath, nsont);
            File.Copy(Path.Combine(_inputPath, nsont), resultFullName, true);
            var theWordFolder =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "The Word");
            var mySwordResult = Path.Combine(_outputPath, "nko.bbl.mysword");
            var exportTheWordInputPath = Path.Combine(Common.ProgInstall, "TheWord");
            ReportWhenTheWordNotInstalled(resultFullName, theWordFolder, mySwordResult, exportTheWordInputPath);
        }

        /// <summary>
        ///A test for TheWordCreatorTempDirectory
        ///</summary>
        [Test]
        public void TheWordCreatorTempDirectoryTest()
        {
            var theWordFullPath = Path.Combine(Common.ProgInstall, "TheWord");
            string actual = TheWordCreatorTempDirectory(theWordFullPath);
            Assert.True(actual.EndsWith(@"TheWord"), "actual = " + actual);
        }

        /// <summary>
        ///A test for Transform
        ///</summary>
        [Test]
        public void TransformTest()
        {
            LoadXslt();
            var name = Path.GetTempFileName();
            var swIn = new StreamWriter(name);
            swIn.Write("<usx><book code=\"2TI\"/></usx>");
            swIn.Close();
            var xsltArgs = new XsltArgumentList();
            xsltArgs.AddParam("bookNames", "", "file://" + FileInput("BookNames.xml"));
            var memory = new MemoryStream();
            var sw = new StreamWriter(memory);
            Transform(name, xsltArgs, sw);
            sw.Flush();
            File.Delete(name);
            memory.Position = 0;
            var sr = new StreamReader(memory);
            var data = sr.ReadToEnd();
            sr.Close();
            Assert.AreEqual(84, data.Split(new[] { '\n' }).Length);
        }

        /// <summary>
        ///A test for UsxDir
        ///</summary>
        [Test]
        public void UsxDirTest()
        {
            var exportTheWordInputPath = _inputPath;
            var expected = Path.Combine(_inputPath, "USX");
            string actual = UsxDir(exportTheWordInputPath);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for UsxDir
        ///</summary>
        [Test]
        public void UsxDirTest1()
        {
            var exportTheWordInputPath = Path.Combine(_inputPath, "USX");
            Assert.Throws(typeof (FileNotFoundException), () => UsxDir(exportTheWordInputPath));
        }

        /// <summary>
        ///A test for PostTransformMessage
        ///</summary>
        [Test]
        public void PostTransformMessageTest()
        {
            MessageFullName = Path.GetTempFileName();
            const string message = "Test Message";
            PostTransformMessage(message);
            XsltMessageClose();
            var data = FileData.Get(MessageFullName);
            File.Delete(MessageFullName);
            Assert.True(data.Contains(message));
            Assert.True(data.StartsWith("<html>"));
            Assert.True(data.TrimEnd().EndsWith("</html>"));
        }

        //[Test]
        //public void myTest()
        //{

        //}
        #region Private Functions
        private static string FileInput(string fileName)
        {
            return Common.PathCombine(_inputPath, fileName);
        }
        #endregion Private Functions
    }
}
