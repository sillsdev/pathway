// --------------------------------------------------------------------------------------------
// <copyright file="ExportTheWordTest.cs" from='2009' to='2014' company='SIL International'>
//      Copyright © 2014, SIL International. All Rights Reserved.   
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
            TestDataCase("MAT", "040MAT.usx", 1, "<TS1><font color=teal>Hɨm Yaaim Me Krais Matyu Kewisɨm Mau Tɨwei</font><Ts><TS1>Niuk me maamrer ne weiwɨk me Jisas<Ts><TS3><i>(<a href=\"tw://bible.*?42.3.23-38\">Lu 3:23-38</a>)</i><Ts><RF q=*><a href=\"tw://bible.*?1.22.18\">Jen 22:18</a>; <a href=\"tw://bible.*?13.17.11\">1Kro 17:11</a><Rf>Menmen im hi hewisɨm hɨram niuk me maamrer yapɨrwe ne weiwɨk miutɨp me Jisas Krais, kerek hɨrak nepenyek ke mɨtɨk iuwe Devit, hɨrak nepenyek hak ke maam nɨpu kaiu Ebraham.<CI>", bookNames, ":");
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
            TestDataCase("EZR", "015EZR.usx", 9, "This was the inventory:<CL><CL>gold dishes — 30<CL>silver dishes — 1,000<CL>silver pans<RF q=+>The meaning of the Hebrew for this word is uncertain.<Rf> — 29<CL>");
        }

        [Test]
        public void TableLineVerse()
        {
            TestDataCase("EZR", "015EZR.usx", 15, "of Shephatiah — 372<CL>");
        }

        [Test]
        public void OptionalBreakTest()
        {
            TestDataCase("EZR", "015EZR.usx", 53, "The gatekeepers of the temple:<CL><CL>the descendants of Shallum, Ater, Talmon, Akkub, Hatita and Shobai — 139<CM>");
        }

        [Test]
        public void BullettedListTest()
        {
            TestDataCase("EZR", "015EZR.usx", 54, "The temple servants:<CL><CI><PI>• the descendants of<CI><PI2>• Ziha, Hasupha, Tabbaoth,<CI>");
        }

        [Test]
        public void SubsequentVerseInBulletParaTest()
        {
            TestDataCase("GEN", "001GEN.usx", 4, "God saw that the light was good, and he separated the light from the darkness.");
        }

        [Test]
        public void S2Test()
        {
            TestDataCase("GEN", "001GEN.usx", 237, "<TS1><font size=-1>The Japhethites</font><Ts><PI>• The sons<RF q=+><i>Sons </i> may mean <i>descendants </i> or <i>successors </i> or <i>nations; </i> also in verses 3, 4, 6, 7, 20-23, 29 and 31.<Rf> of Japheth:<CI><PI2>• Gomer, Magog, Madai, Javan, Tubal, Meshech and Tiras.<CI>");
        }

        [Test]
        public void Li3Test()
        {
            TestDataCase("GEN", "001GEN.usx", 1399, "<PI>• The sons of Judah:<CI><PI2>• Er, Onan, Shelah, Perez and Zerah (but Er and Onan had died in the land of Canaan).<CI>• The sons of Perez:<CI>• Hezron and Hamul.<CI>");
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
            TestDataCase("SNG", "022SNG.usx", 4, "<PI>Take me away with you–let us hurry!<CI><PI2>Let the king bring me into his chambers.<CL><CM><PI0>Friends<CL><CI><PI>We rejoice and delight in you;<RF q=+>The Hebrew is masculine singular.<Rf><CI><PI2>we will praise your love more than wine.<CL><CM><PI0>Beloved<CL><CI><PI>How right they are to adore you!<CL><CI>");
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
            TestDataCase("PHM", "0552TI.usx", 1, "<TS1><font color=teal>Timoti ꞌina leta heluwena Paulo ꞌileleya</font><Ts><TS1>Loyauwedo<Ts><sup>(1-2)</sup> Yauwedo Timoti ꞌowa natugu moisa, ꞌino leta bewa taugu Paulo ꞌoiguwega. Taugu Yesu Keliso ꞌana tohepwaila, Yehoba ꞌina nuwatuhu gide ta yahepwaila yawasida bwebweꞌana weyahina, beno Yesu Keliso ꞌoinega. Tamada Yehoba ma ꞌida Bada Yesu Keliso sihelauwego ta siꞌatemuyamuyaego ma ꞌoinega ꞌumiya daumwala.");
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
            TestDataCase("MAT", "040MAT-refList.usx", 30, "<RF q=*><a href=\"tw://bible.*?1.18.20–19:28\">Jen 18:20–19:28</a>; <a href=\"tw://bible.*?40.11.24\">Mt 11:24</a>; <a href=\"tw://bible.*?61.2.6\">2Pi 2:6</a>; <a href=\"tw://bible.*?65.1.7\">Ju 7</a><Rf>Hi hetpi werek. Maain wɨ God skelim mɨt, hɨrak God kaknep mɨt miyapɨr enun nɨpaa nau Sodom ketike wit Gomora kaknɨp kike. Te mɨt miyapɨr kerek nanweikɨn sip nanwet hɨm mi, maain God kaknɨp iuwe kaknepi.", bookNames, ":");
        }

        [Test]
        public void StarSaltilloTest()
        {
            var bookNames = "file:///" + FileInput("BookNames-refList.xml");
            TestDataCase("MAT", "040MAT-refList.usx", 31, "<i>Judá tusha chumu, ñu Belén pebulu,</i><CI><i>vee mujtu aa pebulu chumulaba buuteꞌ pensangue keeñu, ne balejtuu pebulu jutyuve;</i><CI><i>matyu ñu junuren main bale chachi fale,</i><CI><i>kumuinchi in Israel chachillanu washkenu juñu mitya,</i><RF q=+>Miqueas 5.2 <Rf> ti pillave, tila bale rukula.", bookNames, ":", true);
        }

        [Test]
        public void RefNoVerseTest()
        {
            var bookNames = "file:///" + FileInput("BookNames-refList.xml");
            TestDataCase("MAT", "040MAT-refList.usx", 47, "<RF q=*><a href=\"tw://bible.*?23.23.1\">Ais 23</a>; <a href=\"tw://bible.*?26.26.1–28:26\">Esi 26:1–28:26</a>; <a href=\"tw://bible.*?29.3.4-8\">Joe 3:4-8</a>; <a href=\"tw://bible.*?30.1.9-10\">Emo 1:9-10</a>; <a href=\"tw://bible.*?36.9.2-4\">Sek 9:2-4</a><Rf>Hi hetpi werek. Maain wɨ kerek God skelim mɨt, hɨrak kaknep mɨt ne Taia netike mɨt ne Saidon kaknɨp kike, te yi mɨt au hɨrak kakiwep iuwe.", bookNames, ":");
        }

        [Test]
        public void ShortNameTest()
        {
            var bookNames = "file:///" + FileInput("akeBookNames.xml");
            TestDataCase("1CO", "0461CO.usx", 2, "Papa soꞌsii, Koren pon enakan, kamoro Kʉrai Sises winɨ iyekonekasaꞌ kon wakʉ pe teꞌton kon pe, kamoro mɨ awɨrɨ naꞌneꞌ nan amʉtʉ pe esiiꞌma Sises Kʉrai, uyepuru kon esakʉ pɨꞌ naꞌneꞌ nan, toꞌ epuru mɨrɨ awonsiꞌkɨ uyepuru kon nɨ nɨrɨ:<RF q=*><a href=\"tw://bible.*?44.18.1\">Inkupʉꞌpʉ 18:1</a><Rf>", bookNames, ":");
        }

        [Test]
        public void SpaceAfterRefTest()
        {
            var bookNames = "file:///" + FileInput("aauBookNames.xml");
            TestDataCase("MRK", "041MRK.usx", 647, "Hmo prueyn hiy laplap kopi non nak-sau nok nok, wain ma laroray non sakeyn prouk nok, now-ho mon piynay nok, sa Jisas se seyn arnak-nakray, hiy lowswa e.<RF q=*><a href=\"tw://bible.*?19.69.21\">Sng 69:21</a><Rf> Uwr sohiy nak-me, “Pereipia, hromkwe lira ey, Elaija po pankaw laye pakane, hye now ko se kandieys kow se.”", bookNames, ":");
        }

        [Test]
        public void RightToLeftBridgeTest()
        {
            TestDataCase("3JN", "0643JN.usx", 14, "<sup>(<rtl>14</rtl>-15)</sup> بَسْ لي رَجا خَفيفْ تَ اراك، وْثمْ لَثمْ نحْكي. <sup>15</sup> السَّلام يكونْ مَعك. يسَلمونْ عَلَيك الّمْحبّينْ. سَلّم عَ الّمْحبّينْ كلْ واِحدْ باسْمو.<CM>", true);
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
            TestDataCase("MAT", "040MATtvb.usx", 4, "<sup>(4-5)</sup> Dungure Yesu maan iru di tongwa, “Omilin gi dungwa ibal pilaa dire kanungure, te kawn kebil sungwa u wai pire kol warungure, te seki egilungwa ibal gain wiige sungure, te kiraan gi dungwa ibal kwi pilaa dire ka pirungure, te gulungwa ibal alere u kwi pire mile paingure, te kal aa te nekungwa ibal na guunan kanan pirungure, kal iru erungwa main i kane pire, pi Yon di tenana po.");
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
