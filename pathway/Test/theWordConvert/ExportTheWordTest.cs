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

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using NMock2;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.TheWordConvertTest
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
        private static Mockery mocks = new Mockery();
        private static string _inputPath;
        private static string _outputPath;
        private static string _expectedPath;

        [TestFixtureSetUp]
        public void Setup()
        {
            Common.ProgInstall = PathPart.Bin(Environment.CurrentDirectory, @"/../PsSupport");
            Common.SupportFolder = "";
            Common.ProgBase = Common.ProgInstall;
            string testPath = PathPart.Bin(Environment.CurrentDirectory, "/theWordConvert/TestFiles");
            _inputPath = Common.PathCombine(testPath, "Input");
            _outputPath = Common.PathCombine(testPath, "output");
            _expectedPath = Common.PathCombine(testPath, "Expected");
            if (Directory.Exists(_outputPath))
                Directory.Delete(_outputPath, true);
            Directory.CreateDirectory(_outputPath);
        }
        #endregion setup

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
            PublicationInformation projInfo = null;
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
            ParatextData = @"C:\";
            Ssf = FileInput("MP1.ssf");
            var actual = LoadXsltParameters();
            Assert.AreEqual(":", actual.GetParam("refPunc", ""));
            Assert.AreEqual(@"file:///C:\MP1\BookNames.xml", actual.GetParam("bookNames", ""));
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
            string book = "MAT";
            string expected = @"<usx><book code= ""MAT""/></usx>";
            string actualTempName = TempName(book);
            var sr = new StreamReader(actualTempName);
            string actual = sr.ReadToEnd();
            sr.Close();
            File.Delete(actualTempName);
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ProcessTestamentTest()
        {
            var xsltSettings = new XsltSettings() { EnableDocumentFunction = true };
            string codePath = PathPart.Bin(Environment.CurrentDirectory, "/../theWordConvert");
            TheWord.Load(XmlReader.Create(Common.PathCombine(codePath, "theWord.xsl")), xsltSettings, null);
            IEnumerable<string> books = new List<string>(2) { "MAT", "MRK" };
            var codeNames = new Dictionary<string, string>(2);
            codeNames["MAT"] = FileInput(@"USX\040MAT.usx");
            codeNames["MRK"] = FileInput(@"USX\041MRK.usx");
            var xsltArgs = new XsltArgumentList();
            xsltArgs.AddParam("bookNames", "", "file:///" + FileInput("BookNames.xml"));
            var temp = Path.GetTempFileName();
            var sw = new StreamWriter(temp);
            var inProcess = (IInProcess) mocks.NewMock(typeof (IInProcess));
            Expect.Exactly(2).On(inProcess).Method("PerformStep");
            ProcessTestament(books, codeNames, xsltArgs, sw, inProcess);
            sw.Close();
            var sr = new StreamReader(temp);
            var data = sr.ReadToEnd();
            sr.Close();
            File.Delete(temp);
            Assert.AreEqual(1750, data.Split(new[] { '\n' }).Length);
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        private static void TestDataCase(string code, string fileName, int rec, string expectedResult, bool rtl)
        {
            TestDataCase(code, fileName, rec, expectedResult, null, null, false, rtl);
        }

        private static void TestDataCase(string code, string fileName, int rec, string expectedResult)
        {
            TestDataCase(code, fileName, rec, expectedResult, null, null, false, false);
        }

        private static void TestDataCase(string code, string fileName, int rec, string expectedResult, string bookNames, string punc)
        {
            TestDataCase(code, fileName, rec, expectedResult, bookNames, punc, false, false);
        }

        private static void TestDataCase(string code, string fileName, int rec, string expectedResult, string bookNames, string punc, bool starSaltillo)
        {
            TestDataCase(code, fileName, rec, expectedResult, bookNames, punc, starSaltillo, false);
        }

        private static void TestDataCase(string code, string fileName, int rec, string expectedResult, string bookNames, string punc, bool starSaltillo, bool rtl)
        {
            var xsltSettings = new XsltSettings() { EnableDocumentFunction = true };
            string codePath = PathPart.Bin(Environment.CurrentDirectory, "/../theWordConvert");
            TheWord.Load(XmlReader.Create(Common.PathCombine(codePath, "theWord.xsl")), xsltSettings, null);
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
                xsltArgs.AddParam("bookNames", "", "file:///" + FileInput("BookNames.xml"));
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
            var temp = Path.GetTempFileName();
            var sw = new StreamWriter(temp);
            var inProcess = (IInProcess)mocks.NewMock(typeof(IInProcess));
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
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void NoMt2()
        {
            var bookNames = "file:///" + FileInput("avtBookNames.xml");
            TestDataCase("MAT", "040MAT.usx", 1, "<TS1><font color=teal>H\u0268m Yaaim Me Krais Matyu Kewis\u0268m Mau T\u0268wei</font><Ts><TS1>Niuk me maamrer ne weiw\u0268k me Jisas<Ts><TS3><i>(<a href=\"tw://bible.*?42.3.23-38\">Lu 3:23-38</a>)</i><Ts><RF q=*><a href=\"tw://bible.*?1.22.18\">Jen 22:18</a>; <a href=\"tw://bible.*?13.17.11\">1Kro 17:11</a><Rf>Menmen im hi hewis\u0268m h\u0268ram niuk me maamrer yap\u0268rwe ne weiw\u0268k miut\u0268p me Jisas Krais, kerek h\u0268rak nepenyek ke m\u0268t\u0268k iuwe Devit, h\u0268rak nepenyek hak ke maam n\u0268pu kaiu Ebraham.<CI>", bookNames, ":");
        }

        [Test]
        public void ItalicInFootnote()
        {
            TestDataCase("JHN", "043JHN.usx", 459, "<TS1>Jew Men Hitumatum Imih Jesu Hikwahir<Ts>Nati rarabkokou wanawanan Tafaror Bar ana hiyuw wabin Koksouwen<RF q=+>I baise <i>Hanukkah</i> teo<Rf> i Jerusalemamaim hibogaigiwas.");
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
            TestDataCase("GEN", "001GEN.usx", 237, "<TS1><font size=-1>The Japhethites</font><Ts><PI>\u2022 The sons<RF q=+><i>Sons </i>may mean <i>descendants </i>or <i>successors </i>or <i>nations; </i>also in verses 3, 4, 6, 7, 20-23, 29 and 31.<Rf> of Japheth:<CI><PI2>\u2022 Gomer, Magog, Madai, Javan, Tubal, Meshech and Tiras.<CI>");
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
            var bookNames = "file:///" + FileInput("aauBookNames.xml");
            TestDataCase("PHM", "057PHM.usx", 2, "Hrorkwe mamey okukwe seyr, hromo ine Apia o, hromo wayh Arkipus, hrere nion ma non-orok ono o, seyr uwrsa sios ko, hno a mon ma hokruw sohom o, hme mey kow.<RF q=*><a href=\"tw://bible.*?51.4.17\">Kol 4:17</a>; <a href=\"tw://bible.*?55.2.3\">2 Ti 2:3</a><Rf><CM>", bookNames, ":");
        }

        [Test]
        public void WithoutBookNameTest()
        {
            var bookNames = "file:///" + FileInput("aauBookNames.xml");
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
        public void RefListTest()
        {
            var bookNames = "file:///" + FileInput("BookNames-refList.xml");
            TestDataCase("MAT", "040MAT-refList.usx", 26, "<TS1>complex xref test<Ts><RF q=*><a href=\"tw://bible.*?19.2.7\">Pal 2:7</a>; <a href=\"tw://bible.*?19.22.2\">22:2</a>, <a href=\"tw://bible.*?19.22.14\">14</a><Rf>Verse one.", bookNames, ":");
        }

        [Test]
        public void RefList2Test()
        {
            var bookNames = "file:///" + FileInput("BookNames-refList.xml");
            TestDataCase("MAT", "040MAT-refList.usx", 27, "<RF q=*><a href=\"tw://bible.*?2.37.11\">Tav 37:11</a>, <a href=\"tw://bible.*?2.37.28\">28</a>; <a href=\"tw://bible.*?2.39.2\">39:2</a>, <a href=\"tw://bible.*?2.39.21-23\">21-23</a><Rf>Verse two.", bookNames, ":");
        }

        [Test]
        public void RefSingleChapterTest()
        {
            var bookNames = "file:///" + FileInput("BookNames-refList.xml");
            TestDataCase("MAT", "040MAT-refList.usx", 30, "<RF q=*><a href=\"tw://bible.*?1.18.20\u201319:28\">Jen 18:20\u201319:28</a>; <a href=\"tw://bible.*?40.11.24\">Mt 11:24</a>; <a href=\"tw://bible.*?61.2.6\">2Pi 2:6</a>; <a href=\"tw://bible.*?65.1.7\">Ju 7</a><Rf>Hi hetpi werek. Maain w\u0268 God skelim m\u0268t, h\u0268rak God kaknep m\u0268t miyap\u0268r enun n\u0268paa nau Sodom ketike wit Gomora kakn\u0268p kike. Te m\u0268t miyap\u0268r kerek nanweik\u0268n sip nanwet h\u0268m mi, maain God kakn\u0268p iuwe kaknepi.", bookNames, ":");
        }

        [Test]
        public void StarSaltilloTest()
        {
            var bookNames = "file:///" + FileInput("BookNames-refList.xml");
            TestDataCase("MAT", "040MAT-refList.usx", 31, "<i>Judá tusha chumu, ñu Belén pebulu,</i><CI><i>vee mujtu aa pebulu chumulaba buute\uA78C pensangue keeñu, ne balejtuu pebulu jutyuve;</i><CI><i>matyu ñu junuren main bale chachi fale,</i><CI><i>kumuinchi in Israel chachillanu washkenu juñu mitya,</i><RF q=+>Miqueas 5.2 <Rf> ti pillave, tila bale rukula.", bookNames, ":", true);
        }

        [Test]
        public void RefNoVerseTest()
        {
            var bookNames = "file:///" + FileInput("BookNames-refList.xml");
            TestDataCase("MAT", "040MAT-refList.usx", 47, "<RF q=*><a href=\"tw://bible.*?23.23.1\">Ais 23</a>; <a href=\"tw://bible.*?26.26.1\u201328:26\">Esi 26:1\u201328:26</a>; <a href=\"tw://bible.*?29.3.4-8\">Joe 3:4-8</a>; <a href=\"tw://bible.*?30.1.9-10\">Emo 1:9-10</a>; <a href=\"tw://bible.*?36.9.2-4\">Sek 9:2-4</a><Rf>Hi hetpi werek. Maain w\u0268 kerek God skelim m\u0268t, h\u0268rak kaknep m\u0268t ne Taia netike m\u0268t ne Saidon kakn\u0268p kike, te yi m\u0268t au h\u0268rak kakiwep iuwe.", bookNames, ":");
        }

        [Test]
        public void ShortNameTest()
        {
            var bookNames = "file:///" + FileInput("akeBookNames.xml");
            TestDataCase("1CO", "0461CO.usx", 2, "Papa so\uA78Csii, Koren pon enakan, kamoro K\u0289rai Sises win\u0268 iyekonekasa\uA78C kon wak\u0289 pe te\uA78Cton kon pe, kamoro m\u0268 aw\u0268r\u0268 na\uA78Cne\uA78C nan am\u0289t\u0289 pe esii\uA78Cma Sises K\u0289rai, uyepuru kon esak\u0289 p\u0268\uA78C na\uA78Cne\uA78C nan, to\uA78C epuru m\u0268r\u0268 awonsi\uA78Ck\u0268 uyepuru kon n\u0268 n\u0268r\u0268:<RF q=*><a href=\"tw://bible.*?44.18.1\">Inkup\u0289\uA78Cp\u0289 18:1</a><Rf>", bookNames, ":");
        }

        [Test]
        public void SpaceAfterRefTest()
        {
            var bookNames = "file:///" + FileInput("aauBookNames.xml");
            TestDataCase("MRK", "041MRK.usx", 647, "Hmo prueyn hiy laplap kopi non nak-sau nok nok, wain ma laroray non sakeyn prouk nok, now-ho mon piynay nok, sa Jisas se seyn arnak-nakray, hiy lowswa e.<RF q=*><a href=\"tw://bible.*?19.69.21\">Sng 69:21</a><Rf> Uwr sohiy nak-me, \u201CPereipia, hromkwe lira ey, Elaija po pankaw laye pakane, hye now ko se kandieys kow se.\u201D", bookNames, ":");
        }

        [Test]
        public void RightToLeftBridgeTest()
        {
            TestDataCase("3JN", "0643JN.usx", 14, "<sup>(<rtl>14</rtl>-15)</sup> \u0628\u064e\u0633\u0652 \u0644\u064a \u0631\u064e\u062c\u0627 \u062e\u064e\u0641\u064a\u0641\u0652 \u062a\u064e \u0627\u0631\u0627\u0643\u060c \u0648\u0652\u062b\u0645\u0652 \u0644\u064e\u062b\u0645\u0652 \u0646\u062d\u0652\u0643\u064a. <sup>15</sup> \u0627\u0644\u0633\u0651\u064e\u0644\u0627\u0645 \u064a\u0643\u0648\u0646\u0652 \u0645\u064e\u0639\u0643. \u064a\u0633\u064e\u0644\u0645\u0648\u0646\u0652 \u0639\u064e\u0644\u064e\u064a\u0643 \u0627\u0644\u0651\u0645\u0652\u062d\u0628\u0651\u064a\u0646\u0652. \u0633\u064e\u0644\u0651\u0645 \u0639\u064e \u0627\u0644\u0651\u0645\u0652\u062d\u0628\u0651\u064a\u0646\u0652 \u0643\u0644\u0652 \u0648\u0627\u0650\u062d\u062f\u0652 \u0628\u0627\u0633\u0652\u0645\u0648.<CM>", true);
        }

        [Test]
        public void mt2b4mt1Test()
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
            TestDataCase("GEN", "001GENheg.usx", 23, "Net bihatang na kon, atuling nga tek noan,<CI><PI>“Eli le! Ni halas-sam tom nol au!<CI><PI2>Un seen na banansila el auk seen ni kon.<CI>Nol un sisin na kon banansila el auk sising ngia.<CI>Undeng un daid deng biklobe lia, tiata auk ngali un ngala ka noan ‘bihata’.”<RF q=+>Se dais Ibranin nam, biklobe li noken ‘<i>ish</i>’. Nol bihata li ‘<i>isha</i>’. Tiata se nia, oen kuti dais noan “<i>isha</i> (bihata) daid deng <i>ish</i> (biklobe).”<Rf><CI>");
        }

        [Test]
        public void MultiWordBookNameTest()
        {
            var bookNames = "file:///" + FileInput("hegBookNames.xml");
            TestDataCase("GEN", "001GENheg2.usx", 4, "<sup>(4a-4b)</sup> Ama Lamtua Allah in koet apan-dapa kua nol apan-kloma kia ka, un dehet ta ela.<CM> <sup>4b</sup> <TS1>Ama Lamtua Allah koet biklobe nol bihata<Ts><TS3><i>(<a href=\"tw://bible.*?40.19:4-6.1\">Matius 19:4-6</a>; <a href=\"tw://bible.*?41.10:4-9.1\">Markus 10:4-9</a>; <a href=\"tw://bible.*?46.6:16.1\">Korintus mesa la 6:16</a>; <a href=\"tw://bible.*?46.15:45.1\">15:45</a>, <a href=\"tw://bible.*?46.15:45.47\">47</a>; <a href=\"tw://bible.*?49.5:31-33.1\">Efesus 5:31-33</a>)</i><Ts>Dedeng AMA LAMTUA Allah halas-sam mana le koet apan-dapa ku nol apan-kloma kia ka,", bookNames, ".");
        }

        //[Test]
        //public void myTest()
        //{
            
        //}

        #region Private Functions
        private static string FileProg(string fileName)
        {
            return Common.PathCombine(Common.GetPSApplicationPath(), fileName);
        }

        private static string FileInput(string fileName)
        {
            return Common.PathCombine(_inputPath, fileName);
        }

        private static string FileOutput(string fileName)
        {
            return Common.PathCombine(_outputPath, fileName);
        }

        private static string FileExpected(string fileName)
        {
            return Common.PathCombine(_expectedPath, fileName);
        }

        /// <summary>
        /// Create a simple PublicationInformation instance
        /// </summary>
        private static PublicationInformation GetProjInfo(string XhtmlName, string BlankName)
        {
            PublicationInformation projInfo = new PublicationInformation();
            File.Copy(FileInput(XhtmlName), FileOutput(XhtmlName), true);
            File.Copy(FileInput(BlankName), FileOutput(BlankName), true);
            projInfo.DefaultXhtmlFileWithPath = FileOutput(XhtmlName);
            projInfo.DefaultCssFileWithPath = FileOutput(BlankName);
            projInfo.IsOpenOutput = false;
            return projInfo;
        }
        #endregion PrivateFunctions
    }
}
