// --------------------------------------------------------------------------------------------
// <copyright file="ContentXMLTest.cs" from='2009' to='2014' company='SIL International'>
//      Copyright (C) 2014, SIL International. All Rights Reserved.
//
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed:
//
// <remarks>
// </remarks>
// --------------------------------------------------------------------------------------------

#region Using
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using NUnit.Framework;
using System.Windows.Forms;
using SIL.PublishingSolution;
using SIL.Tool;

#endregion Using

namespace Test.OpenOfficeConvert
{
    [TestFixture]
    [Category("BatchTest")]
    [Category("SkipOnTeamCity")]
    public class LOContentXMLTest
    {
        #region Private Variables
        private string _inputPath;
        private string _outputPath;
        private string _expectedPath;
	    private string _expectedlinuxPath;
        ProgressBar _progressBar;
        private PublicationInformation _projInfo;

        private ValidateXMLFile _validate;
        private string _styleFile;
        private string _contentFile;
        private int _index = 0;
        private bool _isLinux = false;
        private static string _outputBasePath = string.Empty;
        #endregion Private Variables

        #region SetUp

        //public Utility M_util
        //{
        //    get { return _util; }
        //}

        [TestFixtureSetUp]
        protected void SetUp()
        {
            Common.Testing = true;
            //_styleName = new Styles();
            //_util = new Utility();
            _projInfo = new PublicationInformation();
            _progressBar = new ProgressBar();
            string testPath = PathPart.Bin(Environment.CurrentDirectory, "/OpenOfficeConvert/TestFiles");
            _inputPath = Common.PathCombine(testPath, "input");
            _outputPath = Common.PathCombine(testPath, "output");
			_expectedlinuxPath = Common.PathCombine(testPath, "expectedlinux");
			_expectedPath = Common.PathCombine(testPath, "expected");

            Common.DeleteDirectory(_outputPath);
            Directory.CreateDirectory(_outputPath);
            FolderTree.Copy(FileInput("Pictures"), FileOutput("Pictures"));
            _projInfo.ProgressBar = _progressBar;
            _projInfo.OutputExtension = "odt";
            _projInfo.ProjectInputType = "Dictionary";
            _projInfo.IsFrontMatterEnabled = false;
            _projInfo.FinalOutput = "odt";
            Common.SupportFolder = "";
            Common.ProgInstall = PathPart.Bin(Environment.CurrentDirectory, "/../../DistFIles");
            Common.ProgBase = PathPart.Bin(Environment.CurrentDirectory, "/../../DistFiles"); // for masterDocument
	        Common.UseAfterBeforeProcess = true;
            _styleFile = "styles.xml";
            _contentFile = "content.xml";

            _isLinux = Common.IsUnixOS();

			if (!_isLinux)
				LoadParam("Dictionary", "false");
        }

        #endregion

        #region Private Functions
        private string FileInput(string fileName)
        {
            return Common.PathCombine(_inputPath, fileName);
        }

        private string FileOutput(string fileName)
        {
            return Common.PathCombine(_outputPath, fileName);
        }

        private void InLineMethod()
        {
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            XmlNode x = _validate.GetOfficeNode();
            bool fail = false;

            int counter = 1;
            string exp;
            string inner;
            foreach (XmlNode item in x.ChildNodes)
            {
                switch (counter)
                {
                    case 4:
						exp = "locator_dictionary";
						inner = "parent text div div parent text";
                        fail = CheckStyleandInnerText(item, exp, inner);
                        break;

                    case 5:
						exp = "locator_locator_dictionary";
						inner = "parent text";
                        fail = CheckStyleandInnerText(item, exp, inner);
                        break;

					case 6:
						exp = "locator_dictionary";
						inner = "parent text";
						fail = CheckStyleandInnerText(item, exp, inner);
						break;

                    case 7:
                        exp = "topara_locator_dictionary";
                        inner = "text";
                        fail = CheckStyleandInnerText(item, exp, inner);
                        break;

                    case 8:
                        exp = "topara_locator_dictionary";
                        inner = "text";
                        fail = CheckStyleandInnerText(item, exp, inner);
                        break;

                    case 9:
                        exp = "locator_dictionary";
                        inner = "parent text";
                        fail = CheckStyleandInnerText(item, exp, inner);
                        break;

                    default:
                        break;

                }
                counter++;
                if (fail)
                {
                    Assert.Fail("InlineBlock Test Failed");
                }
            }
        }

        private bool CheckStyleandInnerText(XmlNode item, string exp, string inner)
        {
            string key = "style-name";
            string ns = "text";

            XmlNode y;
            bool fail = false;
            y = _validate.GetAttibute(item, key, ns);

            if (y.Value != exp)
            {
                fail = true;
            }
            string innerText = _validate.GetReplacedInnerText(item);
            if (!fail && innerText != inner)
            {
                fail = true;
            }
            return fail;
        }

        #endregion PrivateFunctions

        //#region Nodes_Test
        [Test]
        public void NestedDivCase1_Node()
        {

            const string file = "NestedDivCase1";
            _projInfo.ProjectInputType = "Dictionary";
            GetStyleOutput(file);


            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = "t1_body";
            string content = "T1 class";
            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1);

            _validate.ClassName = "t2_t1_body";
            content = "T2 class";

            returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1);

            _validate.ClassName = "t3_t2_t1_body";
            content = "T3 class";

            returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1);
        }
        [Test]
        public void NestedDivCase2_Node()
        {
            const string file = "NestedDivCase2";
            _projInfo.ProjectInputType = "Dictionary";
            GetStyleOutput(file);

            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = "t1_body";
            string content = "T1 class";
            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1);

            _validate.ClassName = "t2_t1_body";
            content = "T2 class";
            returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1);

            _validate.ClassName = "t3_t2_t1_body";
            content = "T3 class";
            returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1);

            _validate.ClassName = "t4_t1_body";
            content = "T4 class";
            returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1);

        }
        [Test]
        public void NestedDivCase3_Node()
        {
            const string file = "NestedDivCase3";
            _projInfo.ProjectInputType = "Dictionary";
            GetStyleOutput(file);


            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = "t1_body";
            string content = "T1 class";
            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1);

            _validate.ClassName = "t3_t1_body";
            content = "T3 class";
            returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1);

            _validate.ClassName = "t4_t3_t1_body";
            content = "T4 class";
            returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1);


        }
        [Test]
        public void NestedDivCase4_Node()
        {
            const string file = "NestedDivCase4";
            _projInfo.ProjectInputType = "Dictionary";
            GetStyleOutput(file);


            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = "t1_body";
            string content = "T1 class";
            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1);

            _validate.ClassName = "t2_t1_body";
            content = "T2 class";
            returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1);

            _validate.ClassName = "t3_t2_t1_body";
            content = "T3 class";
            returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1);

            _validate.ClassName = "t4_t3_t2_t1_body";
            content = "T4 class";
            returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1);

            _validate.ClassName = "t4_t2_t1_body";
            content = "T4 class";
            returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1);

            _validate.ClassName = "t4_t1_body";
            content = "T4 class";
            returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1);

        }

        ///<summary>
        ///em and % test
        /// </summary>
        [Test]
        public void EMTest1_Node()
        {
            const string file = "EMTest1";
            _projInfo.ProjectInputType = "Dictionary";
            GetStyleOutput(file);

            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = "letData_body";
            string inner = "letdata <text:span text:style-name=\"slotname_letData_body\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">slotname </text:span><text:span text:style-name=\"name_letData_body\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">nameclass </text:span>";
            bool returnValue = _validate.ValidateNodeInnerXmlSubNode(inner);
            Assert.IsTrue(returnValue);
        }

        [Test]
        public void EMTest2_Node()
        {
            const string file = "EMTest2";
            _projInfo.ProjectInputType = "Dictionary";
            GetStyleOutput(file);


            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = "a_body";
            string inner = "Class A <text:span text:style-name=\"b_a_body\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">Class B </text:span>";
            bool returnValue = _validate.ValidateNodeInnerXmlSubNode(inner);
            Assert.IsTrue(returnValue);
        }

        [Test]
        public void LetterSpacing_Node()
        {
            const string file = "EMTest3";
            _projInfo.ProjectInputType = "Dictionary";
            GetStyleOutput(file);

            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = "entry_none";
            string inner = "Class A <text:span text:style-name=\"sense_entry_none\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">Class B </text:span>";
            bool returnValue = _validate.ValidateNodeInnerXmlSubNode(inner);
            Assert.IsTrue(returnValue);
        }

        [Test]
        public void Paragraph()
        {
            const string file = "para";
            _projInfo.ProjectInputType = "Scripture";
            string styleOutput = GetStyleOutput(file);

            _validate = new ValidateXMLFile(styleOutput);

			string xpath = "//style:style[@style:name='first_body']/style:paragraph-properties";
            string content = "<style:paragraph-properties fo:padding-top=\"12pt\" fo:border-bottom=\"0.5pt solid #ffffff\" fo:border-top=\"0.5pt solid #ffffff\" fo:border-left=\"0.5pt solid #ffffff\" fo:border-right=\"0.5pt solid #ffffff\" fo:padding-bottom=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" />";
            _validate.GetOuterXml = true;
            bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "reference-ref failed");

            xpath = "//style:style[@style:name='p_body']/style:paragraph-properties";
            content = "<style:paragraph-properties fo:padding-top=\"12pt\" fo:border-bottom=\"0.5pt solid #ffffff\" fo:border-top=\"0.5pt solid #ffffff\" fo:border-left=\"0.5pt solid #ffffff\" fo:border-right=\"0.5pt solid #ffffff\" fo:padding-bottom=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" />";
            _validate.GetOuterXml = true;
            returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "reference-ref failed");
        }


        ///<summary>
        /// TD-90 .locator .letter
        /// </summary>
        [Test]
        public void AncestorChildTest1_Node()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "AncestorChildTest";
            string styleOutput = GetStyleOutput(file);


            //Content Test - First
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
			_validate.ClassName = "firstoftype_letter_firstoftype_lastoftype_locator_dictionary";
            string content = "a";
            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "span");
            Assert.IsTrue(returnValue1);

			_validate.ClassName = "letter_xitem_firstoftype_lastoftype_locator_dictionary";
            content = "c";
            returnValue1 = _validate.ValidateOfficeTextNode(content, "span");
            Assert.IsTrue(returnValue1);

			_validate.ClassName = "letter1_firstoftype_lastoftype_locator_dictionary";
            content = "j orange";
            returnValue1 = _validate.ValidateOfficeTextNode(11, content, "span");
            Assert.IsTrue(returnValue1);

			_validate.ClassName = "current_letter_firstoftype_lastoftype_locator_dictionary";
            content = "w";
            returnValue1 = _validate.ValidateOfficeTextNode(content, "span");
            Assert.IsTrue(returnValue1);

            //Note - The Styles will be created while processing xhtml(content.xml)
            //Style Test - Second
            _validate = new ValidateXMLFile(styleOutput);
			_validate.ClassName = "letter_firstoftype_lastoftype_locator_dictionary";
            _validate.ClassProperty.Add("fo:font-weight", "700");
            _validate.ClassProperty.Add("style:font-weight-complex", "700");
            _validate.ClassProperty.Add("fo:font-size", "20pt");
            _validate.ClassProperty.Add("style:font-size-complex", "20pt");
            _validate.ClassProperty.Add("fo:color", "#0000ff");
            bool returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

			_validate.ClassName = "letter_xitem_firstoftype_lastoftype_locator_dictionary";
            _validate.ClassProperty.Add("fo:font-weight", "700");
            _validate.ClassProperty.Add("style:font-weight-complex", "700");
            _validate.ClassProperty.Add("fo:font-size", "20pt");
            _validate.ClassProperty.Add("style:font-size-complex", "20pt");
            _validate.ClassProperty.Add("fo:color", "#0000ff");
            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

			_validate.ClassName = "current_letter_firstoftype_lastoftype_locator_dictionary";
            _validate.ClassProperty.Add("fo:color", "#ff0000");
            _validate.ClassProperty.Add("fo:background-color", "#aaff00");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);
        }

		///<summary>
		/// TD-4531 Definition appears bold and should be regular
		/// </summary>
		[Test]
		[Category("SkipOnTeamCity")]
		public void AncestorChildTest2_Node()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "AncestorTestForNewFlexVersion";

			string styleOutput = GetStyleOutput(file);

			//Content Test - First
			_validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
			_validate.ClassName = "span_.bzh_mainheadword_entry_body";
			string content = "=W=";
			bool returnValue1 = _validate.ValidateOfficeTextNode(content, "span");
			Assert.IsTrue(returnValue1);
			_validate.ClassName = "span_.en_definitionorgloss_sense_sensecontent_senses_entry_body";
			content = "Second person; labialisation of the initial consonant of Class 3 verbs beginning with gg";
			if (Common.UsingMonoVM)
				content = "Second person; labialisation of the initial consonant of\n Class 3 verbs beginning with gg";

			returnValue1 = _validate.ValidateOfficeTextNode(content, "span");
			Assert.IsTrue(returnValue1);

			//Note - The Styles will be created while processing xhtml(content.xml)
			//Style Test - Second
			_validate = new ValidateXMLFile(styleOutput);
			_validate.ClassName = "span_.bzh_mainheadword_entry_body";
			_validate.ClassProperty.Add("fo:font-weight", "700");
			_validate.ClassProperty.Add("style:font-weight-complex", "700");
			_validate.ClassProperty.Add("fo:font-size", "10pt");
			_validate.ClassProperty.Add("style:font-size-complex", "10pt");
			bool returnValue = _validate.ValidateNodeAttributesNS(false);
			Assert.IsTrue(returnValue);

			_validate.ClassName = "span_.en_definitionorgloss_sense_sensecontent_senses_entry_body";
			_validate.ClassProperty.Add("fo:font-size", "10pt");
			_validate.ClassProperty.Add("style:font-size-complex", "10pt");
			returnValue = _validate.ValidateNodeAttributesNS(false);
			Assert.IsTrue(returnValue);
		}

		///<summary>
		/// TD-4528 - V should be italic and not bold
		/// TD-4529 - Person should be italic
		/// </summary>
		[Test]
		[Category("SkipOnTeamCity")]
		public void ItalicOnFlex83Test_Node()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "ItalicOnFlex83";

			string styleOutput = GetStyleOutput(file);

			//Content Test - First
			_validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
			_validate.ClassName = "span_.en_partofspeech_morphosyntaxanalysis_sense_sensecontent_senses_entry_letData_body";
			string content = "v";
			bool returnValue1 = _validate.ValidateOfficeTextNode(content, "span");
			Assert.IsTrue(returnValue1);
			_validate.ClassName = "span_.en_name_slot_slots_morphosyntaxanalysis_sense_sensecontent_senses_entry_letData_body";
			content = "Person";
			returnValue1 = _validate.ValidateOfficeTextNode(content, "span");
			Assert.IsTrue(returnValue1);

			//Note - The Styles will be created while processing xhtml(content.xml)
			//Style Test - Second
			_validate = new ValidateXMLFile(styleOutput);
			_validate.ClassName = "span_.en_partofspeech_morphosyntaxanalysis_sense_sensecontent_senses_entry_letData_body";
			_validate.ClassProperty.Add("fo:font-size", "10pt");
			_validate.ClassProperty.Add("style:font-size-complex", "10pt");
			bool returnValue = _validate.ValidateNodeAttributesNS(false);
			Assert.IsTrue(returnValue);

			_validate.ClassName = "span_.en_name_slot_slots_morphosyntaxanalysis_sense_sensecontent_senses_entry_letData_body";
			_validate.ClassProperty.Add("fo:font-size", "10pt");
			_validate.ClassProperty.Add("style:font-size-complex", "10pt");
			returnValue = _validate.ValidateNodeAttributesNS(false);
			Assert.IsTrue(returnValue);

			////Parent style for the above two test, which applied to all chile tags
			//_validate.ClassName = "span_.en_name_slot_slots_morphosyntaxanalysis_sense_sensecontent_senses_entry_letData_body";
			//_validate.ClassProperty.Add("fo:font-style", "italic");
			//returnValue = _validate.ValidateNodeAttributesNS(false);
			//Assert.IsTrue(returnValue);

		}

		///<summary>
		/// TD-4527 Missing slot punctuation
		/// </summary>
		[Test]
		[Category("SkipOnTeamCity")]
		public void PseudoBeforeOnFlex83Test_Node()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "PseudoBeforeOnOnFlex83";

			GetStyleOutput(file);

			//Content Test - First
			_validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
			_validate.ClassName = "span_.en_name_slot_slots_morphosyntaxanalysis_sense_sensecontent_senses_entry_letData_body";
			string content = ": Person";
			bool returnValue1 = _validate.ValidateOfficeTextNode(content, "span");
			Assert.IsTrue(returnValue1);
		}

        [Test]
        public void PseudoQuotes1_Node()
        {
            const string file = "PseudoQuotes1";

            _projInfo.ProjectInputType = "Dictionary";
            GetStyleOutput(file);

            //Content Test
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
			string content = "∖U8658 A a \". " + Common.ConvertUnicodeToString("\\2666") + "\" B b \".  \" C c '  ** ' ** D d # v \" v =\"\" ∖ ' E e \"^ F f $@ G g ! ";
            XmlNode officeNode = _validate.GetOfficeNode();
            Assert.IsNotNull(officeNode, "Office node is null");
            Assert.AreEqual(content, officeNode.InnerText, "PseudoQuotes1 Test failed");

        }

        ///<summary>
        /// TD-429 -  Handling Anchor Tag
        /// </summary>
        [Test]
        public void AnchorTag_Node()
        {
            const string file = "AnchorTag";
            _projInfo.ProjectInputType = "Scripture";
            GetStyleOutput(file);

            //Content Test - First
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = string.Empty;
            string xpath = "//text:span[@text:style-name='scrFootnoteMarker_Paragraph_scrSection_scrBook_scrBody']";
            string content = "<text:a xlink:type=\"simple\" xlink:href=\"#f7be51147-aa97-40a2-ba86-4df84849e9f9\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">a</text:a>";
            bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "reference-ref failed");
            // ="
            xpath = "//text:span[@text:style-name='scrFootnoteMarker_Paragraph_scrSection_scrBook_scrBody']";
            content = "<text:a xlink:type=\"simple\" xlink:href=\"#f7be51147-aa97-40a2-ba86-4df84849e9f9\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">a</text:a>";
            returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "reference - mark failed");
        }

        [Test]
        public void CrossReference_Node()
        {
            const string file = "CrossRef";
            _projInfo.ProjectInputType = "Dictionary";
            GetStyleOutput(file);

            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = string.Empty;
            string xpath = "//text:span[@text:style-name='link_entry_letData_dicBody']";
            string content = "<text:a xlink:type=\"simple\" xlink:href=\"#nema1\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">nema1 source</text:a>";
            bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "reference-ref failed");

            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = string.Empty;
            xpath = "//text:span[@text:style-name='headword1_.bzh_a_entry_letData_dicBody']";
			content = "<text:bookmark text:name=\"nema3\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\" />nema3 text";
            returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "reference-ref failed");
        }

        [Test]
        public void CrossReferenceFileTest_Node()
        {
            const string file = "CrossRef";
            _projInfo.ProjectInputType = "Dictionary";
            GetStyleOutput(file);
            string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
            if (!_isLinux)
            {
                using (var p = Process.Start(Environment.GetEnvironmentVariable("COMSPEC"), string.Format("/c fc {0} {1} >{2}temp.txt", contentExpected, _projInfo.TempOutputFolder, file)))
                {
                    p.WaitForExit();
                    Debug.Print(FileData.Get(file + "temp.txt"));
                }
            }
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }


        ///<summary>
        /// TD-91 .letter[class~='current']
        /// </summary>
        [Test]
        public void ClassNameValueTest_Node()
        {

            const string file = "ClassNameValueTest";
            _projInfo.ProjectInputType = "Dictionary";
            string styleOutput = GetStyleOutput(file);

            _validate = new ValidateXMLFile(styleOutput);
			_validate.ClassName = "current_letter_firstoftype_lastoftype_locator_dictionary";

            _validate.ClassProperty.Add("fo:color", "#ff0000");
            _validate.ClassProperty.Add("fo:background-color", "#aaff00");
            bool returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            //Content Test - First
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
			_validate.ClassName = "current_letter_firstoftype_lastoftype_locator_dictionary";
            string content = "w";
            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "span");
            Assert.IsTrue(returnValue1);
        }

		[Test]
		public void MaxHeightPictureTest_Node()
		{
			const string file = "PictureMaxHeight";

			_projInfo.ProjectInputType = "Dictionary";
			GetStyleOutput(file);

			_validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
			_validate.ClassName = "fr2";
			_validate.ClassProperty.Add("svg:height", "72pt");
			bool returnValue1 = _validate.ValidateDrawNodeAttributesNS(0, "");
			Assert.IsTrue(returnValue1);
		}

        ///<summary>
        /// TD-1607. main.odm format - percentage test
        /// </summary>
        [Test]
        public void ColumnGapPercent()
        {
            const string file = "ColumnGapPercent";
            _projInfo.ProjectInputType = "Dictionary";
            GetStyleOutput(file);

            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = string.Empty;

            string xpath = "//style:style[@style:name='Sect_letData']/style:section-properties";
            _validate.ClassProperty.Add("text:dont-balance-text-columns", "false");
            bool returnValue = _validate.ValidateNodeAttributesNS(0, xpath);
            Assert.IsTrue(returnValue);

            xpath = "//style:style[@style:name='Sect_letData']/style:section-properties/style:columns";
            _validate.ClassProperty.Add("fo:column-count", "2");
            returnValue = _validate.ValidateNodeAttributesNS(0, xpath);
            Assert.IsTrue(returnValue);

            _validate.ClassProperty.Add("style:color", "#aa0000");
            _validate.ClassProperty.Add("style:width", "1pt");
            returnValue = _validate.ValidateNodeAttributesNS(1, xpath);
            Assert.IsTrue(returnValue);


            _validate.ClassProperty.Add("style:rel-width", "4*");
            _validate.ClassProperty.Add("fo:start-indent", "0in");
            _validate.ClassProperty.Add("fo:end-indent", "0.125in");
            returnValue = _validate.ValidateNodeAttributesNS(2, xpath);
            Assert.IsTrue(returnValue);

            _validate.ClassProperty.Add("style:rel-width", "4*");
            _validate.ClassProperty.Add("fo:start-indent", "0.125in");
            _validate.ClassProperty.Add("fo:end-indent", "0in");
            returnValue = _validate.ValidateNodeAttributesNS(3, xpath);
            Assert.IsTrue(returnValue);

            ////////////////////////////////

            xpath = "//style:style[@style:name='Sect_letHead']/style:section-properties";
            _validate.ClassProperty.Add("text:dont-balance-text-columns", "false");
            returnValue = _validate.ValidateNodeAttributesNS(0, xpath);
            Assert.IsTrue(returnValue);


            xpath = "//style:style[@style:name='Sect_letHead']/style:section-properties/style:columns";
            _validate.ClassProperty.Add("fo:column-count", "3");
            returnValue = _validate.ValidateNodeAttributesNS(0, xpath);
            Assert.IsTrue(returnValue);

            _validate.ClassProperty.Add("style:color", "#0000ff");
            _validate.ClassProperty.Add("style:width", "6pt");
            returnValue = _validate.ValidateNodeAttributesNS(1, xpath);
            Assert.IsTrue(returnValue);


            _validate.ClassProperty.Add("style:rel-width", "2.5*");
            _validate.ClassProperty.Add("fo:start-indent", "0in");
            _validate.ClassProperty.Add("fo:end-indent", "0.1666667in");
            returnValue = _validate.ValidateNodeAttributesNS(2, xpath);
            Assert.IsTrue(returnValue);

            _validate.ClassProperty.Add("style:rel-width", "2.5*");
            _validate.ClassProperty.Add("fo:start-indent", "0.1666667in");
            _validate.ClassProperty.Add("fo:end-indent", "0.1666667in");
            returnValue = _validate.ValidateNodeAttributesNS(3, xpath);
            Assert.IsTrue(returnValue);

            _validate.ClassProperty.Add("style:rel-width", "2.5*");
            _validate.ClassProperty.Add("fo:start-indent", "0.1666667in");
            _validate.ClassProperty.Add("fo:end-indent", "0in");
            returnValue = _validate.ValidateNodeAttributesNS(4, xpath);
            Assert.IsTrue(returnValue);

        }

        ///<summary>
        /// TD-83. Open Office column-gap: 1.5em;
        /// </summary>
        [Test]
        public void ColumnGap_Node()
        {
            const string file = "ColumnGap";
            _projInfo.ProjectInputType = "Dictionary";
            GetStyleOutput(file);

            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = string.Empty;

            string xpath = "//style:style[@style:name='Sect_letData']/style:section-properties";
            _validate.ClassProperty.Add("text:dont-balance-text-columns", "false");
            bool returnValue = _validate.ValidateNodeAttributesNS(0, xpath);
            Assert.IsTrue(returnValue);

            xpath = "//style:style[@style:name='Sect_letData']/style:section-properties/style:columns";
            _validate.ClassProperty.Add("fo:column-count", "2");
            returnValue = _validate.ValidateNodeAttributesNS(0, xpath);
            Assert.IsTrue(returnValue);

            _validate.ClassProperty.Add("style:color", "#ff0000");
            _validate.ClassProperty.Add("style:width", "10pt");
            returnValue = _validate.ValidateNodeAttributesNS(1, xpath);
            Assert.IsTrue(returnValue);


            _validate.ClassProperty.Add("style:rel-width", "4*");
            _validate.ClassProperty.Add("fo:start-indent", "0in");
            _validate.ClassProperty.Add("fo:end-indent", "0.125in");
            returnValue = _validate.ValidateNodeAttributesNS(2, xpath);
            Assert.IsTrue(returnValue);

            _validate.ClassProperty.Add("style:rel-width", "4*");
            _validate.ClassProperty.Add("fo:start-indent", "0.125in");
            _validate.ClassProperty.Add("fo:end-indent", "0in");
            returnValue = _validate.ValidateNodeAttributesNS(3, xpath);
            Assert.IsTrue(returnValue);

            xpath = "//style:style[@style:name='Sect_xitem']/style:section-properties";
            _validate.ClassProperty.Add("text:dont-balance-text-columns", "true");
            returnValue = _validate.ValidateNodeAttributesNS(0, xpath);
            Assert.IsTrue(returnValue);


            xpath = "//style:style[@style:name='Sect_xitem']/style:section-properties/style:columns";
            _validate.ClassProperty.Add("fo:column-count", "3");
            returnValue = _validate.ValidateNodeAttributesNS(0, xpath);
            Assert.IsTrue(returnValue);

            _validate.ClassProperty.Add("style:color", "#0000ff");
            _validate.ClassProperty.Add("style:width", "6pt");
            returnValue = _validate.ValidateNodeAttributesNS(1, xpath);
            Assert.IsTrue(returnValue);

            _validate.ClassProperty.Add("style:rel-width", "2.333333*");
            _validate.ClassProperty.Add("fo:start-indent", "0in");
            _validate.ClassProperty.Add("fo:end-indent", "0.25in");
            returnValue = _validate.ValidateNodeAttributesNS(2, xpath);
            Assert.IsTrue(returnValue);

            _validate.ClassProperty.Add("style:rel-width", "2.333333*");
            _validate.ClassProperty.Add("fo:start-indent", "0.25in");
            _validate.ClassProperty.Add("fo:end-indent", "0.25in");
            returnValue = _validate.ValidateNodeAttributesNS(3, xpath);
            Assert.IsTrue(returnValue);

            _validate.ClassProperty.Add("style:rel-width", "2.333333*");
            _validate.ClassProperty.Add("fo:start-indent", "0.25in");
            _validate.ClassProperty.Add("fo:end-indent", "0in");
            returnValue = _validate.ValidateNodeAttributesNS(4, xpath);
            Assert.IsTrue(returnValue);

        }

        ///<summary>
        /// </summary>
        [Test]
        public void ColumnGapLong()
        {
            const string file = "ColumnGapLong";
            _projInfo.ProjectInputType = "Dictionary";
            GetStyleOutput(file);

            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = string.Empty;

            string xpath = "//style:style[@style:name='Sect_letData']/style:section-properties";
            _validate.ClassProperty.Add("text:dont-balance-text-columns", "false");
            bool returnValue = _validate.ValidateNodeAttributesNS(0, xpath);
            Assert.IsTrue(returnValue);

            xpath = "//style:style[@style:name='Sect_letData']/style:section-properties/style:columns";
            _validate.ClassProperty.Add("fo:column-count", "2");
            returnValue = _validate.ValidateNodeAttributesNS(0, xpath);
            Assert.IsTrue(returnValue);

            _validate.ClassProperty.Add("style:color", "#aa0000");
            _validate.ClassProperty.Add("style:width", "1pt");
            returnValue = _validate.ValidateNodeAttributesNS(1, xpath);
            Assert.IsTrue(returnValue);


            _validate.ClassProperty.Add("style:rel-width", "2.99*");
            _validate.ClassProperty.Add("fo:start-indent", "0in");
            _validate.ClassProperty.Add("fo:end-indent", "0.005in");
            returnValue = _validate.ValidateNodeAttributesNS(2, xpath);
            Assert.IsTrue(returnValue);

            _validate.ClassProperty.Add("style:rel-width", "2.99*");
            _validate.ClassProperty.Add("fo:start-indent", "0.005in");
            _validate.ClassProperty.Add("fo:end-indent", "0in");
            returnValue = _validate.ValidateNodeAttributesNS(3, xpath);
            Assert.IsTrue(returnValue);

            ////////////////////////////////
            xpath = "//style:style[@style:name='Sect_letHead']/style:section-properties";
            _validate.ClassProperty.Add("text:dont-balance-text-columns", "false");
            returnValue = _validate.ValidateNodeAttributesNS(0, xpath);
            Assert.IsTrue(returnValue);

            xpath = "//style:style[@style:name='Sect_letHead']/style:section-properties/style:columns";
            _validate.ClassProperty.Add("fo:column-count", "3");
            returnValue = _validate.ValidateNodeAttributesNS(0, xpath);
            Assert.IsTrue(returnValue);

            _validate.ClassProperty.Add("style:color", "#aa0000");
            _validate.ClassProperty.Add("style:width", "1pt");
            returnValue = _validate.ValidateNodeAttributesNS(1, xpath);
            Assert.IsTrue(returnValue);

            _validate.ClassProperty.Add("style:rel-width", "1.555556*");
            _validate.ClassProperty.Add("fo:start-indent", "0in");
            _validate.ClassProperty.Add("fo:end-indent", "0.2222222in");
            returnValue = _validate.ValidateNodeAttributesNS(2, xpath);
            Assert.IsTrue(returnValue);

            _validate.ClassProperty.Add("style:rel-width", "1.555556*");
            _validate.ClassProperty.Add("fo:start-indent", "0.2222222in");
            _validate.ClassProperty.Add("fo:end-indent", "0.2222222in");
            returnValue = _validate.ValidateNodeAttributesNS(3, xpath);
            Assert.IsTrue(returnValue);

            _validate.ClassProperty.Add("style:rel-width", "1.555556*");
            _validate.ClassProperty.Add("fo:start-indent", "0.2222222in");
            _validate.ClassProperty.Add("fo:end-indent", "0in");
            returnValue = _validate.ValidateNodeAttributesNS(4, xpath);
            Assert.IsTrue(returnValue);

        }

        ///<summary>
        /// TD-170 Open Office: triangle should appear before all senses
        /// TD-171 Open Office: sense # is wrong font size
        /// TD-172 open office: visibility
        /// </summary>
        [Test]
        public void Visibility_Node()
        {
            const string file = "VisibilityTest";
            _projInfo.ProjectInputType = "Dictionary";
           GetStyleOutput(file);

            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
			_validate.ClassName = "xsensenumber_.en_sense_senses_entry_letData_dicBody";
            string content = Common.ConvertUnicodeToString("\\25ba") + "1)";
            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "span");
            Assert.IsTrue(returnValue1);
        }

        ///<summary>
        /// </summary>
        [Test]
        public void Counter1_Node()
        {
            const string file = "Counter1";
            _projInfo.ProjectInputType = "Dictionary";
            GetStyleOutput(file);

            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
			_validate.ClassName = "firstoftype_sense_article_sectionletter_dictionary";
            _validate.GetInnerText = true;
            string content = "1.21";
            bool returnValue1 = _validate.ValidateOfficeTextNodeList(1, content, "span");
            Assert.IsTrue(returnValue1, "Counter1 - Content Failure");

            content = "2.21";
            returnValue1 = _validate.ValidateOfficeTextNodeList(2, content, "span");
            Assert.IsTrue(returnValue1, "Counter1 - Content Failure");

			_validate.ClassName = "lastoftype_sense_article_sectionletter_dictionary";


            content = "2.42";
            returnValue1 = _validate.ValidateOfficeTextNodeList(2, content, "span");
            Assert.IsTrue(returnValue1, "Counter1 - Content Failure");

			content = "1.42";
            returnValue1 = _validate.ValidateOfficeTextNodeList(1, content, "span");
            Assert.IsTrue(returnValue1, "Counter1 - Content Failure");
        }

        ///<summary>
        /// </summary>
        [Test]
        public void GlossaryReferenceNode()
        {
            const string file = "GlossaryReference";
            _projInfo.ProjectInputType = "Scripture";
            GetStyleOutput(file);
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
			string xpath = "//text:p[1]/text:span[@text:style-name='SeeInGlossary_.nko_Paragraph_scrSection_scrBook_scrBody']";
            string content = "<text:bookmark-start text:name=\"k_3\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\" /><text:bookmark-ref text:reference-format=\"text\" text:ref-name=\"w_2\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">Abraham</text:bookmark-ref><text:bookmark-end text:name=\"k_3\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\" />";
            bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "FootNote - Content Failure");

			xpath = "//text:p[2]/text:span[@text:style-name='SeeInGlossary_.nko_Paragraph_scrSection_scrBook_scrBody']";
            content = "<text:bookmark-start text:name=\"w_2\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\" /><text:bookmark-ref text:reference-format=\"text\" text:ref-name=\"k_3\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">Abraham</text:bookmark-ref><text:bookmark-end text:name=\"w_2\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\" />";
            returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "FootNote - Content Failure");
        }

        ///<summary>
        /// </summary>
        [Test]
        public void GlossaryReferenceTitleNode()
        {
            const string file = "GlossaryReferenceTitle";
            _projInfo.ProjectInputType = "Scripture";
            GetStyleOutput(file);
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
			string xpath = "//text:p[1]/text:span[@text:style-name='SeeInGlossary_.nko_Paragraph_scrSection_scrBook_scrBody']";
            string content = "<text:bookmark-start text:name=\"k_3\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\" /><text:bookmark-ref text:reference-format=\"text\" text:ref-name=\"sample text\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">Abraham</text:bookmark-ref><text:bookmark-end text:name=\"k_3\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\" />";
            bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "FootNote - Content Failure");

			xpath = "//text:p[2]/text:span[@text:style-name='SeeInGlossary_.nko_Paragraph_scrSection_scrBook_scrBody']";
            content = "<text:bookmark-start text:name=\"sample text\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\" /><text:bookmark-ref text:reference-format=\"text\" text:ref-name=\"k_3\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">Abraham</text:bookmark-ref><text:bookmark-end text:name=\"sample text\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\" />";
            returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "FootNote - Content Failure");
        }

        ///<summary>
        /// </summary>
        [Test]
        public void Counter2_Node()
        {
            const string file = "Counter2";
            _projInfo.ProjectInputType = "Dictionary";
            GetStyleOutput(file);

            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
			_validate.ClassName = "firstoftype_sense_article_sectionletter_dictionary";
            _validate.GetInnerText = true;
            string content = "1.0.01";
            bool returnValue1 = _validate.ValidateOfficeTextNodeList(1, content, "span");
            Assert.IsTrue(returnValue1, "Counter2 - Content Failure");

			content = "2.0.41";
            returnValue1 = _validate.ValidateOfficeTextNodeList(2, content, "span");
            Assert.IsTrue(returnValue1, "Counter2 - Content Failure");

			_validate.ClassName = "lastoftype_sense_article_sectionletter_dictionary";
			content = "1.0.02";
			returnValue1 = _validate.ValidateOfficeTextNodeList(1, content, "span");
			Assert.IsTrue(returnValue1, "Counter2 - Content Failure");

            content = "2.0.52";
            returnValue1 = _validate.ValidateOfficeTextNodeList(2, content, "span");
            Assert.IsTrue(returnValue1, "Counter2 - Content Failure");
        }

        ///<summary>
        /// </summary>
        [Test]
        public void Counter3_Node()
        {
            const string file = "Counter3";
            _projInfo.ProjectInputType = "Dictionary";
            GetStyleOutput(file);

            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
			_validate.ClassName = "firstoftype_sense_article_sectionletter_dictionary";
            _validate.GetInnerText = true;
            string content = "1.11";
            bool returnValue1 = _validate.ValidateOfficeTextNodeList(1, content, "span");
            Assert.IsTrue(returnValue1, "Counter3 - Content Failure");

            content = "2.11";
            returnValue1 = _validate.ValidateOfficeTextNodeList(2, content, "span");
            Assert.IsTrue(returnValue1, "Counter3 - Content Failure");

			_validate.ClassName = "lastoftype_sense_article_sectionletter_dictionary";

            content = "2.22";
            returnValue1 = _validate.ValidateOfficeTextNodeList(2, content, "span");
            Assert.IsTrue(returnValue1, "Counter3 - Content Failure");

			content = "1.22";
			returnValue1 = _validate.ValidateOfficeTextNodeList(1, content, "span");
			Assert.IsTrue(returnValue1, "Counter3 - Content Failure");
        }

        ///<summary>
        /// </summary>
        [Test]
        public void FootNote_Node()
        {
            const string file = "Footnote";
            _projInfo.ProjectInputType = "Dictionary";
            string styleOutput = GetStyleOutput(file);

            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            string xpath = "//text:note[@text:id='ftn1']";
            _validate.ClassName = string.Empty;
            _validate.GetOuterXml = true;
            string content = "<text:note text:id=\"ftn1\" text:note-class=\"footnote\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:note-citation text:label=\"‡ \">‡ </text:note-citation><text:note-body><text:p text:style-name=\"footnote\"><text:span text:style-name=\"footnote..footnote-marker\"></text:span><text:span text:style-name=\"footnote_first_section_scriptureText_scrBody\">1:1: You can use the add spaces button to separate the Unicode characters.</text:span></text:p></text:note-body></text:note>";
            bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "FootNote - Content Failure");

            _validate = new ValidateXMLFile(styleOutput);
            _validate.ClassName = "footnote..footnote-call";
            _validate.ClassProperty.Add("fo:color", "#800080");
            _validate.ClassProperty.Add("fo:font-size", "12pt");
            _validate.ClassProperty.Add("style:text-position", "super 55%");
            _validate.ClassProperty.Add("text:display", "prince-footnote");
            //NOTE: If ttf-mscorefonts-installer is on Linux, the font-family is Arial, if not it is Verdana
            //_validate.ClassProperty.Add("fo:font-family", Common.IsUnixOS() ? "Verdana" : "Arial");
            _validate.ClassProperty.Add("fo:font-weight", "400");
            bool returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue, "Footnote cal - Style Failure");

            _validate = new ValidateXMLFile(styleOutput);
            _validate.ClassName = "footnote..footnote-marker";
            _validate.ClassProperty.Add("fo:color", "#ff0000");
            _validate.ClassProperty.Add("fo:font-size", "10pt");
            _validate.ClassProperty.Add("text:display", "prince-footnote");
            //NOTE: If ttf-mscorefonts-installer is on Linux, the font-family is Arial, if not it is Verdana
            //_validate.ClassProperty.Add("fo:font-family", Common.IsUnixOS() ? "Verdana" : "Arial");
            _validate.ClassProperty.Add("fo:font-weight", "700");
            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue, "Footnote Marker - Style Failure");
        }

        ///<summary>
        /// TD-1909
        /// </summary>
        [Test]
        public void FootnoteSpanContent_Node()
        {
            const string file = "FootnoteSpanContent";
            _projInfo.ProjectInputType = "Scripture";
            GetStyleOutput(file);

            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            string xpath = "//text:note/text:note-body/text:p[@text:style-name='NoteGeneralParagraph']";
            _validate.ClassName = string.Empty;
            _validate.GetOuterXml = true;
			string content = "<text:p text:style-name=\"NoteGeneralParagraph\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:span text:style-name=\"NoteGeneralParagraph..footnote-marker\">21:1 </text:span><text:span text:style-name=\"AlternateReading_.zxx_NoteGeneralParagraph_Paragraph_scrSection_columns_scrBook_scrBody\">converted-values </text:span><text:span text:style-name=\"span_.zxx_NoteGeneralParagraph_Paragraph_scrSection_columns_scrBook_scrBody\">You can use the add spaces button to separate the Unicode characters.</text:span></text:p>";
            bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "FootNote - Content Failure");
        }

        ///<summary>
        /// TD-2758
        /// </summary>
        [Test]
        public void Body()
        {
            const string file = "body";
            _projInfo.ProjectInputType = "Scripture";
            GetStyleOutput(file);

            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
			string xpath = "//text:p[@text:style-name='cover_dicBody']";
            _validate.ClassName = string.Empty;
            _validate.GetOuterXml = true;
			string content = "<text:p text:style-name=\"cover_dicBody\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">body </text:p>";
            bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "Body - Content Failure");
        }

        [Test]
        public void FootNoteFormat_Node()
        {
            const string file = "FootNoteFormat";
            _projInfo.ProjectInputType = "Dictionary";
            GetStyleOutput(file);
            //text:n ote text:id="ftn1"
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            string xpath = "//text:note[@text:id='ftn1']";
            _validate.ClassName = string.Empty;
            _validate.GetOuterXml = true;

			string content = "<text:note text:id=\"ftn1\" text:note-class=\"footnote\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:note-citation text:label=\"a\">a</text:note-citation><text:note-body><text:p text:style-name=\"NoteGeneralParagraph\"><text:span text:style-name=\"NoteGeneralParagraph..footnote-marker\">1:1 = </text:span><text:span text:style-name=\"AlternateReading_.x-kal_NoteGeneralParagraph_Paragraph_scrSection_scrBook_scrBody\">Les juges:</text:span><text:span text:style-name=\"span_.x-kal_NoteGeneralParagraph_Paragraph_scrSection_scrBook_scrBody\">poque de leur histoire, les par des juges. des personnes par Dieu. Dieu les chargeait plus de une ou plusieurs tribus en guerre et de diriger le peuple. Ils rendaient aussi la justice. </text:span><text:span text:style-name=\"AlternateReading_.x-kal_NoteGeneralParagraph_Paragraph_scrSection_scrBook_scrBody\">Moab:</text:span><text:span text:style-name=\"span_.x-kal_NoteGeneralParagraph_Paragraph_scrSection_scrBook_scrBody\">pays fertile situest de la mer Morte.</text:span></text:p></text:note-body></text:note>";
            bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "FootNoteFormat - Content Failure");
        }

        ///<summary>
        ///TD81 line-height: always syntax in Styles.xml
        /// </summary>
        [Test]
        public void LineHeight_Node()
        {
            const string file = "LineHeight";
            _projInfo.ProjectInputType = "Dictionary";
            string styleOutput = GetStyleOutput(file);

            //Content Test - First
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = "t1_body";
            _validate.ClassNameTrim = false;
            string content = "Text t1";
            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1, "LineHeight - Content 1 Failure");

            _validate.ClassName = "inner1_t1_body";
            _validate.ClassNameTrim = false;
            content = "Text Inner1";
            returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1, "LineHeight - Content 2 Failure");

            _validate.ClassName = "inner2_t1_body";
            _validate.ClassNameTrim = false;
            content = "Text Inner2";
            returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1, "LineHeight - Content 3 Failure");

            _validate.ClassName = "inner3_t1_body";
            _validate.ClassNameTrim = false;
            content = "Text Inner3";
            returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1, "LineHeight - Content 4 Failure");

            _validate.ClassName = "inner4_t1_body";
            _validate.ClassNameTrim = false;
            content = "Text Inner4";
            returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1, "LineHeight - Content 5 Failure");

            _validate.ClassName = "noheight_t1_body";
            _validate.ClassNameTrim = false;
            content = "Line Height is None";
            returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1, "LineHeight - Content 6 Failure");

            //Note - The Styles will be created while processing xhtml(content.xml)
            //Style Test - Second
            _validate = new ValidateXMLFile(styleOutput);
            _validate.ClassName = "inner1_t1_body";
            _validate.ClassProperty.Add("style:line-height-at-least", "48pt");
            bool returnValue = _validate.ValidateNodeAttributesNS(true);
            Assert.IsTrue(returnValue, "LineHeight - Style 1 Failure");

            _validate.ClassProperty.Add("fo:font-size", "24pt");
            _validate.ClassProperty.Add("fo:font-size-complex", "24pt");
            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue, "LineHeight - Style 1a Failure");

            _validate.ClassName = "inner2_t1_body";
            _validate.ClassProperty.Add("style:line-height-at-least", "108pt");
            returnValue = _validate.ValidateNodeAttributesNS(true);
            Assert.IsTrue(returnValue, "LineHeight - Style 2 Failure");

            _validate.ClassName = "inner3_t1_body";
            _validate.ClassProperty.Add("style:line-height-at-least", "72pt");
            returnValue = _validate.ValidateNodeAttributesNS(true);
            Assert.IsTrue(returnValue, "LineHeight - Style 2 Failure");

            _validate.ClassProperty.Add("fo:font-size", "24pt");
            _validate.ClassProperty.Add("fo:font-size-complex", "24pt");
            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue, "LineHeight - Style 1a Failure");

            _validate.ClassName = "inner4_t1_body";
            _validate.ClassProperty.Add("style:line-height-at-least", "108pt");
            returnValue = _validate.ValidateNodeAttributesNS(true);
            Assert.IsTrue(returnValue, "LineHeight - Style 2 Failure");

            _validate.ClassName = "noheight_t1_body";
            _validate.ClassProperty.Add("fo:color", "#ffa500");
            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue, "LineHeight - Style 2 Failure");

            string xpath = "//style:style[@style:name='normal_inner1_t1_body']";
            _validate.ClassName = string.Empty;
            _validate.GetOuterXml = true;
            string inner =
                "<style:style xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" style:name=\"normal_inner1_t1_body\" style:family=\"paragraph\" style:parent-style-name=\"inner1_t1_body\"><style:paragraph-properties style:line-height-at-least=\"24pt\" /><style:text-properties fo:color=\"#ffa500\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style>";

            returnValue = _validate.ValidateNodeInnerXml(xpath, inner);
            Assert.IsTrue(returnValue);

        }

        ///<summary>
        ///TD-181 White-space:pre;
        /// </summary>
        [Test]
        public void Whitespace_Node()
        {
            const string file = "whitespace";
            _projInfo.ProjectInputType = "Dictionary";
           GetStyleOutput(file);

            //Content Test - First
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            //Note: without zero it should come. content is correct but same style (space) should come
            _validate.ClassName = "space_body";
            _validate.ClassNameTrim = true;
            string content = "This is a sample Data";
            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1, "Whitespace - Content 2 Failure");

            _validate.ClassName = "nowrap_body";
            _validate.ClassNameTrim = true;
            content = "Goo (programming language), a programming language in the Lisp family";
            returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1, "Whitespace - Content 3 Failure");

            _validate.ClassName = "normal_body";
            _validate.ClassNameTrim = true;
            content = "Goo (programming language), a programming language in the Lisp family";
            returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1, "Whitespace - Content 4 Failure");

            _validate.ClassName = "space_body";
            _validate.ClassNameTrim = false;
            content = "This is a sample Data";
            returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1, "Whitespace - Content 1 Failure");

        }

        ///<summary>
        /// TD-1776 TeluguGondwana_
        /// </summary>
        [Test]
        public void TeluguGondwana_Node()
        {
            const string file = "TeluguGondwana";

            _projInfo.ProjectInputType = "Dictionary";
            GetStyleOutput(file);

            //Content Test - First
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = "span_.ggo-Telu-IN_TitleMain_scrBook_scrBody";
            string content = Common.ConvertUnicodeToString("\\C06");
            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "span");
            Assert.IsTrue(returnValue1);
        }
        ///<summary>
        ///TD130 (Remove AutoWidth and set Column Width for columns)
        /// .letData { column-fill: balance; }
        /// </summary>
        [Test]
        public void RemoveAutoWidth_Node()
        {
            const string file = "RemoveAutoWidth";
            _projInfo.ProjectInputType = "Dictionary";
            GetStyleOutput(file);

            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            string xpath = "//style:style[@style:name='Sect_letData']";
            _validate.ClassName = string.Empty;
            _validate.ClassProperty.Add("text:dont-balance-text-columns", "false");
            bool returnValue = _validate.ValidateNodeAttributesNS(1, xpath);
            Assert.IsTrue(returnValue);
        }


        ///<summary>
        ///TD130 (Remove AutoWidth and set Column Width for columns)
        /// </summary>
        [Test]
        public void InlineBlock_Node()
        {
            const string file = "InlineBlock";
            _projInfo.ProjectInputType = "Dictionary";
            GetStyleOutput(file);
            InLineMethod();
        }


        ///<summary>
        ///Test pink and blue in output
        /// </summary>
        [Test]
        public void LanguageColor_Node()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "LanguageColor";
            string styleOutput = GetStyleOutput(file);

            //Content Test - First
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
			_validate.ClassName = "xlanguagetag_.en_xitem_.pt_definitionL2_.pt_sense_senses_entry_letData_dicBody";
            _validate.ClassNameTrim = false;
            string content = "Por ";
            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "span");
            Assert.IsTrue(returnValue1, "LanguageColor - Content 1 Failure");

			_validate.ClassName = "xitem_.en_definitionL2_.pt_sense_senses_entry_letData_dicBody";
            _validate.ClassNameTrim = true;
            content = "he";
            returnValue1 = _validate.ValidateOfficeTextNode(content, "span");
            Assert.IsTrue(returnValue1, "LanguageColor - Content 2 Failure");

            //Note - The Styles will be created while processing xhtml(content.xml)
            //Style Test - Second
            _validate = new ValidateXMLFile(styleOutput);
			_validate.ClassName = "xlanguagetag_.en_xitem_.pt_definitionL2_.pt_sense_senses_entry_letData_dicBody";
            _validate.ClassProperty.Add("fo:color", "#2F60FF");
            bool returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue, "LanguageColor - Style Failure");

			_validate.ClassName = "xitem_.en_definitionL2_.pt_sense_senses_entry_letData_dicBody";
            _validate.ClassProperty.Add("fo:color", "#FF00FF");
            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue, "LanguageColor - Style Failure");

        }

        /// <summary>
        /// </summary>
        [Test]
        public void PseudoAfter_Node()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "PseudoAfter";

            GetStyleOutput(file);

            //Content Test
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
			_validate.ClassName = "pronunciation_.bzh-fonipa_pronunciations_entry_letData_dicBody";
            string content = "text]";
            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "span");
            Assert.IsTrue(returnValue1);

            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = "span_entry_letData_dicBody";
            content = "again";
            returnValue1 = _validate.ValidateOfficeTextNode(content, "span");
            Assert.IsTrue(returnValue1);
        }


        [Test]
        public void LanguageInlineBlock_Node()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "LanguageInline";
            GetStyleOutput(file);

            //Content Test
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
			_validate.ClassName = "partofspeech_.tam_entry_letData_dicBody";
            string content = "Select a destination";
            bool returnValue1 = _validate.ValidateOfficeTextNodeList(1, content, "para");
            Assert.IsTrue(returnValue1);

            returnValue1 = _validate.ValidateOfficeTextNodeList(2, content, "para");
            Assert.IsTrue(returnValue1);

            returnValue1 = _validate.ValidateOfficeTextNodeList(3, content, "para");
            Assert.IsTrue(returnValue1);
        }


        /// <summary>
        /// </summary>
        [Test]
        public void PseudoAfter1_Node()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "PseudoAfter1";

            GetStyleOutput(file);

            //Content Test
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = "letHead_div_dicBody";
            string content = "A a<text:span text:style-name=\"entry_letHead_div_dicBody\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">entry</text:span>###";
            bool returnValue1 = _validate.ValidateOfficeTextNodeList(1, content, "para");
            Assert.IsTrue(returnValue1, "PseudoAfter1 - Content Failure");

            content = "B b***";
            returnValue1 = _validate.ValidateOfficeTextNodeList(2, content, "para");
            Assert.IsTrue(returnValue1, "PseudoAfter1 - 2nd Content Failure");

        }

        ///<summary>
        /// TD-59  font-size: larger; and font-size: smaller;
        /// </summary>
        [Test]
        public void Larger_Node()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "Larger";

            string styleOutput = GetStyleOutput(file);

            //Content Test - First
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = "p4_body";
            _validate.ClassNameTrim = true;
            string content = "<text:span text:style-name=\"child1_p4_body\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"> text </text:span><text:span text:style-name=\"child2_p4_body\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">text </text:span>";
            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1, "Larger - Content 1 Failure");

            _validate = new ValidateXMLFile(styleOutput);
            _validate.ClassName = "child1_p4_body";
            _validate.ClassProperty.Add("fo:font-size", "120");
            bool returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            _validate.ClassName = "child2_p4_body";
            _validate.ClassProperty.Add("fo:font-size", "53");
            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);
        }

        ///<summary>
        /// TD-222   Image width:50%
        /// </summary>
        [Test]
        public void Picture_Width_Node()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "PictureWidth";
            GetStyleOutput(file);

            //Content Test - First
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = "entry1_letData_dicBody";
            _validate.ClassNameTrim = true;
            _validate.GetOuterXml = true;
	        string content =
				"<text:p text:style-name=\"entry1_letData_dicBody\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><draw:frame draw:style-name=\"fr3\" draw:name=\"Frame3\" text:anchor-type=\"paragraph\" draw:z-index=\"1\" svg:width=\"25%\" svg:height=\"72pt\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\"><draw:text-box fo:min-height=\"1in\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><draw:frame draw:style-name=\"gr3\" draw:name=\"Graphics3\" text:anchor-type=\"paragraph\" svg:height=\"72pt\"><draw:image xlink:type=\"simple\" xlink:show=\"embed\" xlink:actuate=\"onLoad\" xlink:href=\"Pictures/leftindexmacro1.jpg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" /><svg:title>leftindexmacro1.jpg</svg:title></draw:frame><text:p text:style-name=\"pictureCaption_pictureRight_entry1_letData_dicBody\"><text:p text:style-name=\"CmPicturepublishStemCaptionSenseNumber_pictureCaption_pictureRight_entry1_letData_dicBody\">1</text:p><text:p text:style-name=\"CmPicturepublishStemCaptionCaptionPub_.pt_pictureCaption_pictureRight_entry1_letData_dicBody\">cala</text:p></text:p></draw:text-box></draw:frame><text:span text:style-name=\"headword_.seh_entry1_letData_dicBody\">cala</text:span><text:span text:style-name=\"headword_.seh\"><text:variable-set text:name=\"Left_Guideword_L\" text:display=\"none\" text:formula=\"ooow: \" office:value-type=\"string\" office:string-value=\"\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"headword_.seh\"><text:variable-set text:name=\"RLeft_Guideword_L\" text:display=\"none\" text:formula=\"ooow: cala\" office:value-type=\"string\" office:string-value=\"cala\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"headword_.seh\"><text:variable-set text:name=\"Right_Guideword_R\" text:display=\"none\" text:formula=\"ooow: \" office:value-type=\"string\" office:string-value=\"\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"headword_.seh\"><text:variable-set text:name=\"RRight_Guideword_R\" text:display=\"none\" text:formula=\"ooow: cala\" office:value-type=\"string\" office:string-value=\"cala\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span /> <text:span text:style-name=\"partofspeech_.pt_grammaticalinfo_sense_senses_entry1_letData_dicBody\">N</text:span><text:span text:style-name=\"xlanguagetag_.en_xitem_.pt_definitionL2_.pt_sense_senses_entry1_letData_dicBody\">Por </text:span><text:span text:style-name=\"xitem_.pt_definitionL2_.pt_sense_senses_entry1_letData_dicBody\">dedo</text:span><text:span text:style-name=\"xlanguagetag_.en_xitem_.en_definitionL2_.pt_sense_senses_entry1_letData_dicBody\">Eng </text:span><text:span text:style-name=\"xitem_.en_definitionL2_.pt_sense_senses_entry1_letData_dicBody\">finger</text:span></text:p>";

            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1, "PictureWidth - Content 1 Failure");
        }

        ///<summary>
        /// TD-1855 Picture / PictureBox error
        /// </summary>
        [Test]
		[Category("SkipOnTeamCity")]
        public void Picture_Mrk_Node()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "mrk";
            GetStyleOutput(file);

            //Content Test - First
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = "section_scriptureText_body";
            _validate.ClassNameTrim = true;
            _validate.GetOuterXml = true;
			string content = "<text:p text:style-name=\"section_scriptureText_body\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><draw:frame draw:style-name=\"fr2\" draw:name=\"Frame2\" text:anchor-type=\"paragraph\" draw:z-index=\"1\" svg:width=\"292.5pt\" svg:height=\"72pt\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\"><draw:text-box fo:min-height=\"1in\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><draw:frame draw:style-name=\"gr2\" draw:name=\"Graphics2\" text:anchor-type=\"paragraph\" svg:width=\"292.5pt\" svg:height=\"72pt\"><draw:image xlink:type=\"simple\" xlink:show=\"embed\" xlink:actuate=\"onLoad\" xlink:href=\"Pictures/lb00296c.png\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" /><svg:title>lb00296c.png</svg:title></draw:frame><text:p text:style-name=\"caption_img_section_scriptureText_body\"><text:p text:style-name=\"caption_img_section_scriptureText_body\">Yohanis, Tuka Sarani Dh (Mark.1.4-6)</text:p></text:p></draw:text-box></draw:frame><text:p text:style-name=\"p_section_scriptureText_body\">Lodo Yerusalem, saku eele sasala miu. </text:p><text:p text:style-name=\"p_section_scriptureText_body\">Lodo Yerusalem, saku eele sasala miu.</text:p><text:p text:style-name=\"p_section_scriptureText_body\"><text:span text:style-name=\"verseNumber_p_section_scriptureText_body\">  7 </text:span>Tuka Sarani hia Lamatua Yesus!.  <text:span text:style-name=\"verseNumber_p_section_scriptureText_body\"> 8 </text:span>intro text goes here intro text goes here intro text goes here intro text goes here intro text goes here intro text goes here </text:p></text:p>";

	        if (Common.UsingMonoVM)
		        content = "<text:p text:style-name=\"section_scriptureText_body\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><draw:frame draw:style-name=\"fr2\" draw:name=\"Frame2\" text:anchor-type=\"paragraph\" draw:z-index=\"1\" svg:width=\"292.5pt\" svg:height=\"72pt\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\"><draw:text-box fo:min-height=\"1in\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><draw:frame draw:style-name=\"gr2\" draw:name=\"Graphics2\" text:anchor-type=\"paragraph\" svg:width=\"292.5pt\" svg:height=\"72pt\"><draw:image xlink:type=\"simple\" xlink:show=\"embed\" xlink:actuate=\"onLoad\" xlink:href=\"Pictures/figure/lb00296c.png\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" /><svg:title>figure/lb00296c.png</svg:title></draw:frame><text:p text:style-name=\"caption_img_section_scriptureText_body\"><text:p text:style-name=\"caption_img_section_scriptureText_body\">Yohanis, Tuka Sarani Dh (Mark.1.4-6)</text:p></text:p></draw:text-box></draw:frame><text:p text:style-name=\"p_section_scriptureText_body\">Lodo Yerusalem, saku eele sasala miu. </text:p><text:p text:style-name=\"p_section_scriptureText_body\">Lodo Yerusalem, saku eele sasala miu.</text:p><text:p text:style-name=\"p_section_scriptureText_body\"><text:span text:style-name=\"verseNumber_p_section_scriptureText_body\">  7 </text:span>Tuka Sarani hia Lamatua Yesus!.  <text:span text:style-name=\"verseNumber_p_section_scriptureText_body\"> 8 </text:span>intro text goes here intro text goes here intro text goes here intro text goes here intro text goes here intro text goes here </text:p></text:p>";
            bool returnValue1 = _validate.ValidateOfficeTextNodeForPicture(content, "para");
            Assert.IsTrue(returnValue1, "Picture_Mrk - Content 1 Failure");
        }

        ///<summary>
        /// TD-1855 Picture / PictureBox error
        /// </summary>
        [Test]
        public void ImageNoCaption_Node()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "ImageNoCaption";
            GetStyleOutput(file);

            //Content Test - First
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = string.Empty;
            _validate.GetOuterXml = true;
            _validate.XPath = "//draw:frame[@draw:style-name='gr2']";
            string content = "<draw:frame draw:style-name=\"gr2\" draw:name=\"Graphics2\" text:anchor-type=\"paragraph\" svg:width=\"72pt\" svg:height=\"72pt\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\"><draw:image xlink:type=\"simple\" xlink:show=\"embed\" xlink:actuate=\"onLoad\" xlink:href=\"Pictures/c.jpg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" /><svg:title>c.jpg</svg:title></draw:frame>";
            bool returnValue = _validate.ValidateOfficeTextNode(0, content, "para");
            Assert.IsTrue(returnValue);
        }
        ///<summary>
        /// TD-227 Set language for data.
        /// </summary>
        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void Language_Node()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "Language";
            string styleOutput = GetStyleOutput(file);

            //Content Test - First
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = "xitem_.en_main_body";
            string content = "sample text file";
            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "span");
            Assert.IsTrue(returnValue1);

            _validate.ClassName = "xitem_.fr_main_body";
            content = "exemple de fichier texte";
            returnValue1 = _validate.ValidateOfficeTextNode(content, "span");
            Assert.IsTrue(returnValue1);

            _validate.ClassName = "xitem_.es_main_body";
            content = "Este es muestra texto";
            returnValue1 = _validate.ValidateOfficeTextNode(content, "span");
            Assert.IsTrue(returnValue1);

            _validate.ClassName = "xitem_.fr-FR_main_body";
            content = "exemple de fichier texte";
            returnValue1 = _validate.ValidateOfficeTextNode(content, "span");
            Assert.IsTrue(returnValue1);

            _validate.ClassName = "xitem_.es-ES_main_body";
            content = "Este es muestra texto";
            returnValue1 = _validate.ValidateOfficeTextNode(content, "span");
            Assert.IsTrue(returnValue1);

	        if (Common.UsingMonoVM)
		        return;

            //Note - The Styles will be created while processing xhtml(content.xml)
            //Style Test - Second
            _validate = new ValidateXMLFile(styleOutput);
            _validate.ClassName = "xitem_.en_main_body";
            _validate.ClassProperty.Add("fo:language", "en");
            _validate.ClassProperty.Add("fo:country", "US");
            bool returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            _validate.ClassName = "xitem_.fr_main_body";
            _validate.ClassProperty.Add("fo:language", "fr");
            _validate.ClassProperty.Add("fo:country", "FR");
            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            _validate.ClassName = "xitem_.es_main_body";
            _validate.ClassProperty.Add("fo:language", "es");
            _validate.ClassProperty.Add("fo:country", "ES");
            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            _validate.ClassName = "xitem_.fr-FR_main_body";
            _validate.ClassProperty.Add("fo:language", "fr");
            _validate.ClassProperty.Add("fo:country", "FR");
            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            _validate.ClassName = "xitem_.es-ES_main_body";
            _validate.ClassProperty.Add("fo:language", "es");
            _validate.ClassProperty.Add("fo:country", "ES");
            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

        }


        ///<summary>
        /// TD-204   unable to put tok / pisin.
        /// </summary>
        [Test]
        public void ClassContent_Node()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "ClassContent";

            string styleOutput = GetStyleOutput(file);
            //Content Test - First
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
			_validate.ClassName = "xlanguagetag_.en_xitem_.en_definition_.en_sense_senses_entry_letData_dicBody";
            _validate.ClassNameTrim = true;
			string content = "Eng";
            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "span");
            Assert.IsTrue(returnValue1, "ClassContent - Content 1 Failure");

            //Note - The Styles will be created while processing xhtml(content.xml)
            //Style Test - Second
            _validate = new ValidateXMLFile(styleOutput);
			_validate.ClassName = "xlanguagetag_.en_xitem_.tpi_definition_.en_sense_senses_entry_letData_dicBody";
            _validate.ClassProperty.Add("fo:font-size", "38pt");
            _validate.ClassProperty.Add("style:font-size-complex", "38pt");
            _validate.ClassProperty.Add("fo:color", "#ff0000");
            bool returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue, "ClassContent - Style 1 Failure");
        }


        ///<summary>
        /// TD-304   Open Office div.header
        /// </summary>
        [Test]
        public void div_header_Node()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "div_header";
            string styleOutput = GetStyleOutput(file);

            //Content Test - First
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
			_validate.ClassName = "header_main";
            _validate.ClassNameTrim = true;
            string content = "Main Title";
            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1, "div_header - Content 1 Failure");

            //Note - The Styles will be created while processing xhtml(content.xml)
            //Style Test - Second
            _validate = new ValidateXMLFile(styleOutput);
			_validate.ClassName = "header_main";
            _validate.ClassProperty.Add("fo:font-size", "24pt");
            _validate.ClassProperty.Add("style:font-size-complex", "24pt");
            bool returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue, "div_header - Style 1 Failure");
        }


        ///<summary>
        /// TD-347  Implement content: normal;
        /// </summary>
        [Test]
        public void ContentNormalTest_Node()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "ContentNormalTest";

            GetStyleOutput(file);

            //Content Test - First
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = "entry_letData_dicBody";
            _validate.ClassNameTrim = false;
            _validate.GetInnerText = true;
            string content = "ba VPor roubar Eng steal the converted valuesPor the converted values Eng child of Fatima Por the converted values Eng child of Fatima 11Por child of god Por the converted values Eng child of Fatima Por the converted values Por Unicode characters Eng child of Fatima ";

            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1, "ContentNormalTest - Content 1 Failure");

        }

        ///<summary>
        /// TD-343 Implement lists
        /// </summary>
        [Test]
        public void ListOlUl_Node()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "ListOlUl";
            string styleOutput = GetStyleOutput(file);

            //Content Test - First
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = string.Empty;
            _validate.ClassNameTrim = false;
            string content = "one1 ";
			string xpath = "//text:list[@text:style-name='ol']/text:list-item/text:p[@text:style-name='li.ol_a1_section_body']";
            _validate.GetInnerText = true;
            bool returnValue = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue, "ListOlUl - Content 1 Failure");

			xpath = "//text:list[@text:style-name='ol']/text:list-item/text:p[@text:style-name='li.ol_a2_section_body']";
            _validate.GetInnerText = true;
            returnValue = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue, "ListOlUl - Content 1 Failure");

			xpath = "//text:list[@text:style-name='ol']/text:list-item/text:p[@text:style-name='li.ol_a3_section_body']";
            _validate.GetInnerText = true;
            returnValue = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue, "ListOlUl - Content 1 Failure");

			xpath = "//text:list[@text:style-name='ol']/text:list-item/text:p[@text:style-name='li.ol_a4_section_body']";
            _validate.GetInnerText = true;
            returnValue = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue, "ListOlUl - Content 1 Failure");

			xpath = "//text:list[@text:style-name='ol']/text:list-item/text:p[@text:style-name='li.ol_a5_section_body']";
            _validate.GetInnerText = true;
            returnValue = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue, "ListOlUl - Content 1 Failure");

			xpath = "//text:list[@text:style-name='ol']/text:list-item/text:p[@text:style-name='li.ol_a6_section_body']";
            _validate.GetInnerText = true;
            returnValue = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue, "ListOlUl - Content 1 Failure");

			xpath = "//text:list[@text:style-name='ol']/text:list-item/text:p[@text:style-name='li.ol_a7_section_body']";
            _validate.GetInnerText = true;
            returnValue = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue, "ListOlUl - Content 1 Failure");

			xpath = "//text:list[@text:style-name='ol']/text:list-item/text:p[@text:style-name='li.ol_a8_section_body']";
            _validate.GetInnerText = true;
            returnValue = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue, "ListOlUl - Content 1 Failure");

            //Note - The Styles will be created while processing xhtml(content.xml)
            //Style Test - Second
            _validate = new ValidateXMLFile(styleOutput);
            _validate.ClassName = string.Empty;
            content = "<text:list-level-style-bullet text:level=\"1\" text:style-name=\"Bullet_20_Symbols\" style:num-suffix=\".\" text:bullet-char=\"" + Common.ConvertUnicodeToString("\\2022") + "\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><style:list-level-properties text:list-level-position-and-space-mode=\"label-alignment\"><style:list-level-label-alignment text:label-followed-by=\"listtab\" text:list-tab-stop-position=\"0.5in\" fo:text-indent=\"-0.25in\" fo:margin-left=\"0.5in\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:list-level-properties></text:list-level-style-bullet>";
            xpath = "//text:list-style[@style:name='ol.a2']";
            returnValue = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue, "ListOlUl - Style 1 Failure");

            content = "<text:list-level-style-bullet text:level=\"1\" text:style-name=\"Bullet_20_Symbols\" style:num-suffix=\".\" text:bullet-char=\"" + Common.ConvertUnicodeToString("\\25E6") + "\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><style:list-level-properties text:list-level-position-and-space-mode=\"label-alignment\"><style:list-level-label-alignment text:label-followed-by=\"listtab\" text:list-tab-stop-position=\"0.5in\" fo:text-indent=\"-0.25in\" fo:margin-left=\"0.5in\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:list-level-properties></text:list-level-style-bullet>";
            xpath = "//text:list-style[@style:name='ol.a3']";
            returnValue = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue, "ListOlUl - Style 1 Failure");

            content = "<text:list-level-style-bullet text:level=\"1\" text:style-name=\"Bullet_20_Symbols\" style:num-suffix=\".\" text:bullet-char=\"" + Common.ConvertUnicodeToString("\\25AA") + "\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><style:list-level-properties text:list-level-position-and-space-mode=\"label-alignment\"><style:list-level-label-alignment text:label-followed-by=\"listtab\" text:list-tab-stop-position=\"0.5in\" fo:text-indent=\"-0.25in\" fo:margin-left=\"0.5in\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:list-level-properties></text:list-level-style-bullet>";
            xpath = "//text:list-style[@style:name='ol.a4']";
            returnValue = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue, "ListOlUl - Style 1 Failure");

            content = "<text:list-level-style-number text:level=\"1\" text:style-name=\"Numbering_20_Symbols\" style:num-suffix=\".\" style:num-format=\"1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><style:list-level-properties text:list-level-position-and-space-mode=\"label-alignment\"><style:list-level-label-alignment text:label-followed-by=\"listtab\" text:list-tab-stop-position=\"0.5in\" fo:text-indent=\"-0.25in\" fo:margin-left=\"0.5in\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:list-level-properties></text:list-level-style-number>";
            xpath = "//text:list-style[@style:name='ol.a5']";
            returnValue = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue, "ListOlUl - Style 1 Failure");

            content = "<text:list-level-style-number text:level=\"1\" text:style-name=\"Numbering_20_Symbols\" style:num-suffix=\".\" style:num-format=\"i\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><style:list-level-properties text:list-level-position-and-space-mode=\"label-alignment\"><style:list-level-label-alignment text:label-followed-by=\"listtab\" text:list-tab-stop-position=\"0.5in\" fo:text-indent=\"-0.25in\" fo:margin-left=\"0.5in\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:list-level-properties></text:list-level-style-number>";
            xpath = "//text:list-style[@style:name='ol.a6']";
            returnValue = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue, "ListOlUl - Style 1 Failure");

            content = "<text:list-level-style-number text:level=\"1\" text:style-name=\"Numbering_20_Symbols\" style:num-suffix=\".\" style:num-format=\"I\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><style:list-level-properties text:list-level-position-and-space-mode=\"label-alignment\"><style:list-level-label-alignment text:label-followed-by=\"listtab\" text:list-tab-stop-position=\"0.5in\" fo:text-indent=\"-0.25in\" fo:margin-left=\"0.5in\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:list-level-properties></text:list-level-style-number>";
            xpath = "//text:list-style[@style:name='ol.a7']";
            returnValue = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue, "ListOlUl - Style 1 Failure");

            content = "<text:list-level-style-number text:level=\"1\" text:style-name=\"Numbering_20_Symbols\" style:num-suffix=\".\" style:num-format=\"a\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><style:list-level-properties text:list-level-position-and-space-mode=\"label-alignment\"><style:list-level-label-alignment text:label-followed-by=\"listtab\" text:list-tab-stop-position=\"0.5in\" fo:text-indent=\"-0.25in\" fo:margin-left=\"0.5in\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:list-level-properties></text:list-level-style-number>";
            xpath = "//text:list-style[@style:name='ol.a8']";
            returnValue = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue, "ListOlUl - Style 1 Failure");

            content = "<text:list-level-style-number text:level=\"1\" text:style-name=\"Numbering_20_Symbols\" style:num-suffix=\".\" style:num-format=\"A\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><style:list-level-properties text:list-level-position-and-space-mode=\"label-alignment\"><style:list-level-label-alignment text:label-followed-by=\"listtab\" text:list-tab-stop-position=\"0.5in\" fo:text-indent=\"-0.25in\" fo:margin-left=\"0.5in\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:list-level-properties></text:list-level-style-number>";
            xpath = "//text:list-style[@style:name='ol.a9']";
            returnValue = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue, "ListOlUl - Style 1 Failure");
        }

        ///<summary>
        /// TD-362 Image Attribute
        /* <example img[src='Thomsons-gazelle1.jpg'] { float:left;}> */
        /// </summary>
        [Test]
        public void Picture_ImageAttrib_Node()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "ImageAttrib";

            _projInfo.DefaultCssFileWithPath = FileInput(file + ".css");
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");

            PreExportProcess preProcessor = new PreExportProcess(_projInfo);
            preProcessor.GetTempFolderPath();
            preProcessor.ImagePreprocess(false);
            _projInfo.DefaultXhtmlFileWithPath = preProcessor.ProcessedXhtml;

            string styleOutput = GetStyleOutput(_projInfo);

            //Content Test - First
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = string.Empty;
            _validate.GetOuterXml = true;
            _validate.XPath = "//draw:frame[@draw:style-name='fr2']";
			string content = "<draw:frame draw:style-name=\"fr2\" draw:name=\"Frame2\" text:anchor-type=\"paragraph\" draw:z-index=\"1\" svg:width=\"288pt\" svg:height=\"144pt\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\"><draw:text-box fo:min-height=\"1in\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><draw:frame draw:style-name=\"gr2\" draw:name=\"Graphics2\" text:anchor-type=\"paragraph\" svg:width=\"288pt\" svg:height=\"144pt\"><draw:image xlink:type=\"simple\" xlink:show=\"embed\" xlink:actuate=\"onLoad\" xlink:href=\"Pictures/1.jpg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" /><svg:title>1.jpg</svg:title></draw:frame><text:p text:style-name=\"img_image_firstoftype_sense_article_sectionletter_dictionary\"> Flea </text:p></draw:text-box></draw:frame>";
            bool returnValue = _validate.ValidateOfficeTextNode(0, content, "para");
            Assert.IsTrue(returnValue);

            _validate.XPath = "//draw:frame[@draw:name='Graphics2']";
            content = "<draw:frame draw:style-name=\"gr2\" draw:name=\"Graphics2\" text:anchor-type=\"paragraph\" svg:width=\"288pt\" svg:height=\"144pt\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\"><draw:image xlink:type=\"simple\" xlink:show=\"embed\" xlink:actuate=\"onLoad\" xlink:href=\"Pictures/1.jpg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" /><svg:title>1.jpg</svg:title></draw:frame>";
            returnValue = _validate.ValidateOfficeTextNode(0, content, "para");
            Assert.IsTrue(returnValue);

            //Style Test - Second
            _validate = new ValidateXMLFile(styleOutput);
            _validate.ClassName = "fr2";

            _validate.ClassProperty.Add("style:vertical-pos", "from-top");
            _validate.ClassProperty.Add("style:vertical-rel", "paragraph-content");
            _validate.ClassProperty.Add("style:horizontal-pos", "center");
            _validate.ClassProperty.Add("style:horizontal-rel", "paragraph");

            _validate.ClassProperty.Add("run-through", "foreground");
            _validate.ClassProperty.Add("number-wrapped-paragraphs", "no-limit");
            _validate.ClassProperty.Add("wrap-contour", "false");

            _validate.ClassProperty.Add("text:anchor-type", "paragraph");

            returnValue = _validate.ValidateNodeAttributesNS(1, string.Empty);
            Assert.IsTrue(returnValue);

            _validate.ClassName = "fr3";
            _validate.ClassProperty.Add("style:vertical-pos", "from-top");
            _validate.ClassProperty.Add("style:vertical-rel", "paragraph-content");
            _validate.ClassProperty.Add("style:horizontal-pos", "center");
            _validate.ClassProperty.Add("style:horizontal-rel", "paragraph");

            _validate.ClassProperty.Add("run-through", "foreground");
            _validate.ClassProperty.Add("number-wrapped-paragraphs", "no-limit");
            _validate.ClassProperty.Add("wrap-contour", "false");

            _validate.ClassProperty.Add("text:anchor-type", "paragraph");

            returnValue = _validate.ValidateNodeAttributesNS(1, string.Empty);
            Assert.IsTrue(returnValue);
        }


        ///<summary>
        /// TD-416 chapter number should align with top of text not with bottom
        /// </summary>
        [Test]
        public void DropCap_Node()
        {
            const string file = "DropCap";
            _projInfo.ProjectInputType = "Scripture";

            string styleOutput = GetStyleOutput(file);

            //Content Test - First
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = "ChapterNumber1";
            _validate.ChildClassName = "ChapterNumber_Paragraph_scrSection_scrBook_scrBody";
            _validate.ChildClassType = "span";
            string content = "1";
            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1);

            //Note - The Styles will be created while processing xhtml(content.xml)
            ////Style Test - Second
            _validate = new ValidateXMLFile(styleOutput);
            string xpath = "//style:style[@style:name='ChapterNumber1']";
            _validate.ClassName = string.Empty;
            string inner = "<style:paragraph-properties style:vertical-align=\"auto\" fo:float=\"left\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\">" +
            "<style:drop-cap style:lines=\"2\" style:distance=\"0.20cm\" style:length=\"1\" />" +
            "</style:paragraph-properties>";

            bool returnValue = _validate.ValidateNodeInnerXml(xpath, inner);
            Assert.IsTrue(returnValue);
        }

        ///<summary>
        /// TD-453 - Index doesn't have three columns in this data
        /// </summary>
        [Test]
        public void ColumnCountTest_Node()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "ColumnCount";
            GetStyleOutput(file);

            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = string.Empty;

            string xpath = "//style:style[@style:name='Sect_letHead']/style:section-properties";
            _validate.ClassProperty.Add("text:dont-balance-text-columns", "false");
            bool returnValue = _validate.ValidateNodeAttributesNS(0, xpath);
            Assert.IsTrue(returnValue);

            xpath = "//style:style[@style:name='Sect_letHead']/style:section-properties/style:columns";
            _validate.ClassProperty.Add("fo:column-count", "1");
            returnValue = _validate.ValidateNodeAttributesNS(0, xpath);
            Assert.IsTrue(returnValue);

            xpath = "//style:style[@style:name='Sect_letData']/style:section-properties";
            _validate.ClassProperty.Add("text:dont-balance-text-columns", "true");
            returnValue = _validate.ValidateNodeAttributesNS(0, xpath);
            Assert.IsTrue(returnValue);

            xpath = "//style:style[@style:name='Sect_letData']/style:section-properties/style:columns";
            _validate.ClassProperty.Add("fo:column-count", "4");
            returnValue = _validate.ValidateNodeAttributesNS(0, xpath);
            Assert.IsTrue(returnValue);

            _validate.ClassProperty.Add("style:rel-width", "1.875*");
            _validate.ClassProperty.Add("fo:start-indent", "0in");
            _validate.ClassProperty.Add("fo:end-indent", "0.125in");
            returnValue = _validate.ValidateNodeAttributesNS(1, xpath);
            Assert.IsTrue(returnValue);

            _validate.ClassProperty.Add("style:rel-width", "1.875*");
            _validate.ClassProperty.Add("fo:start-indent", "0.125in");
            _validate.ClassProperty.Add("fo:end-indent", "0.125in");
            returnValue = _validate.ValidateNodeAttributesNS(2, xpath);
            Assert.IsTrue(returnValue);

            _validate.ClassProperty.Add("style:rel-width", "1.875*");
            _validate.ClassProperty.Add("fo:start-indent", "0.125in");
            _validate.ClassProperty.Add("fo:end-indent", "0.125in");
            returnValue = _validate.ValidateNodeAttributesNS(3, xpath);
            Assert.IsTrue(returnValue);

            _validate.ClassProperty.Add("style:rel-width", "1.875*");
            _validate.ClassProperty.Add("fo:start-indent", "0.125in");
            _validate.ClassProperty.Add("fo:end-indent", "0in");
            returnValue = _validate.ValidateNodeAttributesNS(4, xpath);
            Assert.IsTrue(returnValue);
        }

        ///<summary>
        /// </summary>
        [Test]
        public void SpanStyleTest_Node()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "spanstyle";

            string styleOutput = GetStyleOutput(file);

            //Content Test - First
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
			_validate.ClassName = "xitem_.tpi_definition_.en_sense_senses_entry_letData_dicBody";
            _validate.ClassNameTrim = false;
            string content = "spirit/ tewel em i lusim bodi long nait long driman";
            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "span");
            Assert.IsTrue(returnValue1, "spacebefore - Content 1 Failure");

			//Note - The Styles will be created while processing xhtml(content.xml)
			//Style Test - Second
			_validate = new ValidateXMLFile(styleOutput);
			_validate.ClassName = "span_.bzh_xitem_.en_definition_.en_sense_senses_entry_letData_dicBody";
			_validate.ClassProperty.Add("fo:color", "#ff0000");
			bool returnValue = _validate.ValidateNodeAttributesNS(false);
			Assert.IsTrue(returnValue, "spacebefore - Style Failure");
        }

        ///<summary>
        /// TD-458 - Index sense divider spacing
        /// </summary>
        [Test]
        public void SpaceBeforeTest_Node()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "spacebefore";

            //StyleXML
            GetStyleOutput(file);
            //Content Test - First
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = "revEntry_revData_revAppendix";
            _validate.ClassNameTrim = false;
            string content = "<text:span text:style-name=\"headword_.en_revEntry_revData_revAppendix\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">??</text:span><text:span text:style-name=\"revSense_.bss_revEntry_revData_revAppendix\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">booy</text:span><text:span text:style-name=\"revSense_.bss_revEntry_revData_revAppendix\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">; the</text:span><text:span text:style-name=\"revSense_.bss_revEntry_revData_revAppendix\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">; child</text:span><text:span text:style-name=\"revSense_.bss_revEntry_revData_revAppendix\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">; went</text:span>";
            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1, "spacebefore - Content 1 Failure");
        }

        ///<summary>
        /// TD-479 -  create property to substitute quote characters
        /// </summary>
        [Test]
        public void ReplacePrinceQuoteTest_Node()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "ReplacePrinceQuote";
            GetStyleOutput(file);

            //Content Test - First
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
			_validate.ClassName = "span_.bgt_Line1_columns_scrBook_scrBody";
            _validate.ClassNameTrim = true;
            string content = "I Jerusalem, inau ku tutua sina ghahira kori hidigna na vathe.";
            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "span");
            Assert.IsTrue(returnValue1, "ReplacePrinceQuote - Content 1 Failure");

			_validate.ClassName = "span_.bgt_Line2_columns_scrBook_scrBody";
            _validate.ClassNameTrim = true;
            content = "Ahai ke vaututunia, imanea teo keda toatogha ke boi toke ke vaututunia.";
            returnValue1 = _validate.ValidateOfficeTextNode(content, "span");
            Assert.IsTrue(returnValue1, "ReplacePrinceQuote - Content 2 Failure");
        }

        ///<summary>
        /// TD-518 -  divider line missing after introduction
        /// </summary>
        [Test]
        public void EmptyDivTag_Node()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "EmptyDivTag";
            string styleOutput = GetStyleOutput(file);

            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = "border";
            _validate.ClassNameTrim = false;
            bool returnValue1 = _validate.ValidateOfficeTextNode("para");
            Assert.IsTrue(returnValue1, "EmptyDivTag - Content  Failure");

            //Note - The Styles will be created while processing xhtml(content.xml)
            //Style Test - Second
            _validate = new ValidateXMLFile(styleOutput);
            _validate.ClassName = "border";

            _validate.ClassProperty.Add("fo:border-top", ".5pt solid #000000");
            _validate.ClassProperty.Add("fo:text-align", "center");
            _validate.ClassProperty.Add("fo:margin-left", "1in");
            _validate.ClassProperty.Add("fo:margin-right", "1in");
            _validate.ClassProperty.Add("fo:margin-top", "9pt");
            _validate.ClassProperty.Add("fo:margin-bottom", "18pt");
            //TODO AFTER FINISHING BORDER PROPERTY CHECK THIS.
            //bool returnValue = _validate.ValidateNodeAttributesNS(true);
            //Assert.IsTrue(returnValue, "EmptyDivTag - Style Failure");

        }
        ///<summary>
        /// TD-654
        /// </summary>
        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void VerseNumber_Node()
        {

            _projInfo.ProjectInputType = "Dictionary";
            const string file = "VerseNumber";

            string styleOutput = GetStyleOutput(file);
            //
            //Content Test - First
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
			_validate.ClassName = "VerseNumber_.cwa_Paragraph_scrSection_columns_scrBook_scrBody";
            string content = "1";
            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "span");
            Assert.IsTrue(returnValue1);

            //Note - The Styles will be created while processing xhtml(content.xml)
            //Style Test - Second
            _validate = new ValidateXMLFile(styleOutput);
			_validate.ClassName = "VerseNumber_.cwa_Paragraph_scrSection_columns_scrBook_scrBody";

            _validate.ClassProperty.Add("fo:font-family", "Charis SIL");
            _validate.ClassProperty.Add("style:font-name-complex", "Charis SIL");
            _validate.ClassProperty.Add("style:text-position", "super 55%");
            _validate.ClassProperty.Add("fo:color", "#ff0000");
            _validate.ClassProperty.Add("fo:font-style", "italic");
            _validate.ClassProperty.Add("fo:background-color", "#0000ff");

            bool returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue, "VerseNumber - Style Failure");
        }


        ///<summary>
        /// TD-349 -  width: auto
        /// </summary>
        [Test]
        public void AutoWidth_Node()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "AutoWidth";

            GetStyleOutput(file);

            //First Node
            string xpath = "//text:p[@text:style-name='pictureCaption_scrBody']";
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = string.Empty;
            _validate.ClassProperty.Add("fo:min-width", "111.529411764706pt");

            bool returnValue = _validate.ValidateNodeAttributesNS(1, xpath);
            Assert.IsTrue(returnValue);
        }

        /// <summary>
        /// Remove color if the background color is white. This is done for
        /// pdf output thru odt output.
        /// </summary>
        [Test]
        public void BackgroundWhiteColor_Removal_Node()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "BackgroundWhiteRemoval";

            string styleOutput = GetStyleOutput(file);

            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = "t1_body";
            string content = "T1 class";
            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1);

            _validate.ClassName = "t2_t1_body";
            content = "T2 class";

            returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1);

            _validate.ClassName = "t3_t2_t1_body";
            content = "T3 class";

            returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1);

            _validate = new ValidateXMLFile(styleOutput);
            _validate.ClassName = "t1_body";
            _validate.ClassProperty.Add("fo:text-align", "left");
            _validate.ClassProperty.Add("fo:background-color", "#008000");
            bool returnValue = _validate.ValidateNodeAttributesNS(true);
            Assert.IsTrue(returnValue);

            _validate.ClassProperty.Add("fo:font-size", "12pt");
            _validate.ClassProperty.Add("style:font-size-complex", "12pt");
            _validate.ClassProperty.Add("fo:color", "#ff0000");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            _validate.ClassName = "t2_t1_body";
            _validate.ClassProperty.Add("fo:text-align", "left");
            returnValue = _validate.ValidateNodeAttributesNS(true);
            Assert.IsTrue(returnValue);

            _validate.ClassProperty.Add("fo:color", "#0000ff");
            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            _validate.ClassProperty.Add("fo:background-color", "#ffffff");
            returnValue = _validate.ValidateNodeAttributesNS(true);
            Assert.IsFalse(returnValue);  // white color should not be there.
        }

        ///<summary>
        /// TD-1239 -  Two column output from TE can conflict with two column stylesheets, not wrap correctly at section breaks.
        /// </summary>
        [Test]
        public void RemoveScrSectionClass_Node()
        {
            //TODO NEGATIVE TEST FOR SCRSECTION IS NOT CREATED, SHOULD BE DONE
            _projInfo.ProjectInputType = "Scripture";
            const string file = "RemoveScrSectionClass";

            GetStyleOutput(file);

            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = string.Empty;

            string xpath = "//style:style[@style:name='Sect_columns2']/style:section-properties";
            _validate.ClassProperty.Add("text:dont-balance-text-columns", "false");
            bool returnValue = _validate.ValidateNodeAttributesNS(0, xpath);
            Assert.IsTrue(returnValue);

            xpath = "//style:style[@style:name='Sect_columns1']/style:section-properties";
            _validate.ClassProperty.Add("text:dont-balance-text-columns", "true");
            returnValue = _validate.ValidateNodeAttributesNS(0, xpath);
            Assert.IsTrue(returnValue);

        }

        ///<summary>
        /// TD-1239 -  Two column output from TE can conflict with two column stylesheets, not wrap correctly at section breaks.
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void TableProperty()
        {
            //TODO NEGATIVE TEST FOR SCRSECTION IS NOT CREATED, SHOULD BE DONE
            _projInfo.ProjectInputType = "Scripture";
            const string file = "TableProperty";

			string styleOutput = GetStyleOutput(file);

			string styleExpected = Common.PathCombine(_expectedPath, file + "styles.xml");
			string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
			XmlAssert.Ignore(styleOutput, "//office:font-face-decls", new Dictionary<string, string> { { "office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0" } });
			TextFileAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
			TextFileAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");

        }

        ///<summary>
        /// TD-89	Open Office .slots:before
        /// </summary>
        [Test]
        public void SignificantSpace()
        {
            const string file = "SignificantSpace";
            _projInfo.ProjectInputType = "Dictionary";
            GetStyleOutput(file);

            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = "entry_letData_dicBody";
            //_validate.GetInnerText = true;
            string content = "<text:span text:style-name=\"partofspeech_.en_sense_senses_entry_letData_dicBody\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">No space between the spans -------------------------&gt; v</text:span><text:span text:style-name=\"definition_.en_sense_senses_entry_letData_dicBody\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">light </text:span><text:span text:style-name=\"blue_senses_entry_letData_dicBody\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">(COMING TOGETHER) </text:span>";
            bool returnValue1 = _validate.ValidateOfficeTextNodeList(1, content, "para");
            Assert.IsTrue(returnValue1, "SignificantSpace - Content Failure");

            content = "<text:span text:style-name=\"partofspeech_.en_gram_sense_senses_entry_letData_dicBody\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">Two end span and start span -------------------------&gt; v</text:span><text:span text:style-name=\"definition_.en_sense_senses_entry_letData_dicBody\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">light (COMING WITH ONE SPACE)</text:span>";
            returnValue1 = _validate.ValidateOfficeTextNodeList(2, content, "para");
            Assert.IsTrue(returnValue1, "SignificantSpace - Content Failure");

            content = "<text:span text:style-name=\"partofspeech_.en_sense_senses_entry_letData_dicBody\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">  One end span and start span -------------------------&gt; v</text:span><text:span text:style-name=\"definition_.en_sense_senses_entry_letData_dicBody\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">light (COMING WITH ONE SPACE)</text:span>";
            returnValue1 = _validate.ValidateOfficeTextNodeList(3, content, "para");
            Assert.IsTrue(returnValue1, "SignificantSpace - Content Failure");

            content = "<text:span text:style-name=\"partofspeech_.en_sense_senses_entry_letData_dicBody\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">Horizontal continues span without cr lf - long space ---&gt; v</text:span><text:span text:style-name=\"definition_.en_sense_senses_entry_letData_dicBody\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">light (COMING WITH ONE SPACE)</text:span>";
            returnValue1 = _validate.ValidateOfficeTextNodeList(4, content, "para");
            Assert.IsTrue(returnValue1, "SignificantSpace - Content Failure");
        }
        ///<summary>
        /// TD96 text-indent
        /// </summary>
        [Test]
        public void Textindent_Node()
        {
            const string file = "Textindent";
            _projInfo.ProjectInputType = "Dictionary";
            string styleOutput = GetStyleOutput(file);

            //Content Test - First
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = "t1_body";
            string content = "You and all the people of Israel: It is by the name of Jesus Christ of Nazareth, whom you crucified but whom God raised from the dead, that this man stands before you healed. <text:span text:style-name=\"verse_t1_body\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">11</text:span>He is \" 'the stone you builders rejected, which has become the capstone.[a]'[b] <text:span text:style-name=\"verse_t1_body\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">12</text:span><text:span text:style-name=\"salvation_t1_body\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"> Salvation is found in no one else, for there is no other name under heaven given to men by which we must be saved.\" </text:span>";
            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1);

            //Note - The Styles will be created while processing xhtml(content.xml)
            //Style Test - Second
            _validate = new ValidateXMLFile(styleOutput);
            _validate.ClassName = "t1_body";
            _validate.ClassProperty.Add("fo:text-indent", "100pt");
            bool returnValue = _validate.ValidateNodeAttributesNS(true);
            Assert.IsTrue(returnValue);
        }
        ///<summary>
        /// BackgroundColor
        /// </summary>
        [Test]
        public void BackgroundColor_Node()
        {
            const string file = "BackgroundColor";
            _projInfo.ProjectInputType = "Dictionary";
            string styleOutput = GetStyleOutput(file);

            //Content Test - First
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = "divColor_body";
            _validate.GetInnerText = true;
            string content = "Span Color class Div Color class";
            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1, "div color failed");


            _validate.ClassName = "spanColor_divColor_body";
            _validate.GetInnerText = true;
            content = "Span Color class";
            returnValue1 = _validate.ValidateOfficeTextNode(content, "span");
            Assert.IsTrue(returnValue1, "span color failed");

            //Note - The Styles will be created while processing xhtml(content.xml)
            //Style Test - Second
            _validate = new ValidateXMLFile(styleOutput);
            _validate.ClassName = "divColor_body";
            _validate.ClassProperty.Add("fo:background-color", "#008000");
            bool returnValue = _validate.ValidateNodeAttributesNS(true);
            Assert.IsTrue(returnValue, "div does not have background color");

            _validate.ClassName = "spanColor_divColor_body";
            _validate.ClassProperty.Add("fo:background-color", "#ffff00");
            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue, "span does not have background color ");

            _validate.ClassName = "spanColor_divColor_body";
            _validate.ClassProperty.Add("fo:background-color", "#ffff00");
            returnValue = _validate.ValidateNodeAttributesNS(true);
            Assert.IsFalse(returnValue, "para node has background color");
        }
        ///<summary>
        /// DisplayNone
        /// </summary>
        [Test]
        public void DisplayNone_Node()
        {
            const string file = "DisplayNone";
            _projInfo.ProjectInputType = "Dictionary";
            string styleOutput = GetStyleOutput(file);

            //Content Test - First
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = "divColor1_body";
            _validate.GetInnerText = true;
            string content = "Span Color1 class Divcolor 1";
            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1, "div color failed");


            _validate.ClassName = "spanColor2_divColor2_body";
            _validate.GetInnerText = true;
            content = "Span Color2 class";
            returnValue1 = _validate.ValidateOfficeTextNode(content, "span");
            Assert.IsTrue(returnValue1, "span color failed");

            //Note - The Styles will be created while processing xhtml(content.xml)
            //Style Test - Second
            _validate = new ValidateXMLFile(styleOutput);

            _validate.ClassName = "spanColor2_divColor2_body";
            _validate.ClassProperty.Add("fo:background-color", "#ff0000");
            bool returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue, "span does not have background color ");

        }
        ///<summary>
        /// Display Inline
        /// </summary>
        [Test]
        public void DisplayInline_Node()
        {
            const string file = "DisplayInline";
            _projInfo.ProjectInputType = "Dictionary";
            GetStyleOutput(file);

            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = "letter_letHead_dicBody";
            _validate.GetInnerText = true;
            string content = "A a";
            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "span");
            Assert.IsTrue(returnValue1, "DisplayInline failed");
        }

		[Test]
		public void PageDictionaryTest1()
	    {
			// Single Page - Top Center
			const string file = "DictionaryPageTest";
			_projInfo.ProjectInputType = "Dictionary";
			_index = 1;
			string styleOutput = GetStyleOutput(file);
			const string xpath = "//style:master-page[@style:name=\"XHTML\"]";
			_validate = new ValidateXMLFile(styleOutput) {GetOuterXml = true};
			const string content = "<style:master-page style:name=\"XHTML\" style:page-layout-name=\"pm2\" style:next-style-name=\"XHTML\" " +
			                       "xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" " +
			                       "xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:span text:style-name=\"MT1\"><text:variable-get " +
			                       "text:name=\"Left_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" />" +
			                       "</text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RLeft_Guideword_L\" office:value-type=\"string\" " +
			                       "xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:tab /><text:span text:style-name=\"MT3\">" +
			                       "<text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:header><style:footer>" +
			                       "<text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><draw:frame draw:style-name=\"Mfr1\" " +
			                       "draw:name=\"Frame1\" text:anchor-type=\"paragraph\" svg:y=\"42.25pt\" fo:min-width=\"145pt\" draw:z-index=\"1\" " +
			                       "xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" " +
			                       "xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\"><draw:text-box fo:min-height=\"19.00pt\"><text:p text:style-name=\"MP1\">" +
			                       "<text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Right_Guideword_R\" office:value-type=\"string\" " +
			                       "xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"MT2\">" +
			                       "<text:variable-get text:name=\"RRight_Guideword_R\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" />" +
			                       "</text:span></text:p></draw:text-box></draw:frame></text:p></style:footer></style:master-page>";
			bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
			Assert.IsTrue(returnValue1, "Single Page with Top Center Test failed");
	    }

		[Test]
		public void PageDictionaryTest2()
		{
			// Single Page - Bottom Center
			const string file = "DictionaryPageTest";
			_projInfo.ProjectInputType = "Dictionary";
			_index = 2;
			string styleOutput = GetStyleOutput(file);
			const string xpath = "//style:master-page[@style:name=\"XHTML\"]";
			_validate = new ValidateXMLFile(styleOutput) { GetOuterXml = true };
			const string content = "<style:master-page style:name=\"XHTML\" style:page-layout-name=\"pm2\" style:next-style-name=\"XHTML\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Left_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RLeft_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span></text:p></style:header><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><draw:frame draw:style-name=\"Mfr1\" draw:name=\"Frame1\" text:anchor-type=\"paragraph\" svg:y=\"42.25pt\" fo:min-width=\"145pt\" draw:z-index=\"1\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\"><draw:text-box fo:min-height=\"19.00pt\"><text:p text:style-name=\"MP1\"><text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Right_Guideword_R\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RRight_Guideword_R\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span></text:p></draw:text-box></draw:frame><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:footer></style:master-page>";
			bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
			Assert.IsTrue(returnValue1, "Single Page with Bottom Center Test failed");
		}

		[Test]
		public void PageDictionaryTest3()
		{
			// Mirrored Page - Top Inside Margin
			const string file = "DictionaryPageTest";
			_projInfo.ProjectInputType = "Dictionary";
			_index = 3;
			string styleOutput = GetStyleOutput(file);
			string xpath = "//style:master-page[@style:name=\"Left_20_Page\"]";
			_validate = new ValidateXMLFile(styleOutput) { GetOuterXml = true };
			string content = "<style:master-page style:name=\"Left_20_Page\" style:display-name=\"Left Page\" style:page-layout-name=\"pm4\" style:next-style-name=\"Right_20_Page\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Left_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RLeft_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:tab /><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:header></style:master-page>";
			bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
			Assert.IsTrue(returnValue1, "Mirrored Page with Top Inside Margin - Left Page Test failed");

			xpath = "//style:master-page[@style:name=\"Right_20_Page\"]";
			_validate = new ValidateXMLFile(styleOutput) { GetOuterXml = true };
			content = "<style:master-page style:name=\"Right_20_Page\" style:display-name=\"Right Page\" style:page-layout-name=\"pm5\" style:next-style-name=\"Left_20_Page\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span><text:tab /><text:tab /><text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Left_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RLeft_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span></text:p></style:header><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><draw:frame draw:style-name=\"Mfr1\" draw:name=\"Frame1\" text:anchor-type=\"paragraph\" svg:y=\"42.25pt\" fo:min-width=\"145pt\" draw:z-index=\"1\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\"><draw:text-box fo:min-height=\"19.00pt\"><text:p text:style-name=\"MP1\"><text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Right_Guideword_R\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RRight_Guideword_R\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span></text:p></draw:text-box></draw:frame></text:p></style:footer></style:master-page>";
			returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
			Assert.IsTrue(returnValue1, "Mirrored Page with Top Inside Margin - Right Page Test failed");
		}

		[Test]
		public void PageDictionaryTest4()
		{
			// Mirrored Page - Top Center
			const string file = "DictionaryPageTest";
			_projInfo.ProjectInputType = "Dictionary";
			_index = 4;
			string styleOutput = GetStyleOutput(file);
			string xpath = "//style:master-page[@style:name=\"Left_20_Page\"]";
			_validate = new ValidateXMLFile(styleOutput) { GetOuterXml = true };
			string content = "<style:master-page style:name=\"Left_20_Page\" style:display-name=\"Left Page\" style:page-layout-name=\"pm4\" style:next-style-name=\"Right_20_Page\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Left_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RLeft_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:header></style:master-page>";
			bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
			Assert.IsTrue(returnValue1, "Mirrored Page with Top Center - Left Page Test failed");

			xpath = "//style:master-page[@style:name=\"Right_20_Page\"]";
			_validate = new ValidateXMLFile(styleOutput) { GetOuterXml = true };
			content = "<style:master-page style:name=\"Right_20_Page\" style:display-name=\"Right Page\" style:page-layout-name=\"pm5\" style:next-style-name=\"Left_20_Page\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span><text:tab /><text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Left_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RLeft_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span></text:p></style:header><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><draw:frame draw:style-name=\"Mfr1\" draw:name=\"Frame1\" text:anchor-type=\"paragraph\" svg:y=\"42.25pt\" fo:min-width=\"145pt\" draw:z-index=\"1\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\"><draw:text-box fo:min-height=\"19.00pt\"><text:p text:style-name=\"MP1\"><text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Right_Guideword_R\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RRight_Guideword_R\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span></text:p></draw:text-box></draw:frame></text:p></style:footer></style:master-page>";
			returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
			Assert.IsTrue(returnValue1, "Mirrored Page with Top Center - Right Page Test failed");
		}

		[Test]
		public void PageDictionaryTest5()
		{
			// Mirrored Page - Bottom Inside Margin
			const string file = "DictionaryPageTest";
			_projInfo.ProjectInputType = "Dictionary";
			_index = 5;
			string styleOutput = GetStyleOutput(file);
			string xpath = "//style:master-page[@style:name=\"Left_20_Page\"]";
			_validate = new ValidateXMLFile(styleOutput) { GetOuterXml = true };
			string content = "<style:master-page style:name=\"Left_20_Page\" style:display-name=\"Left Page\" style:page-layout-name=\"pm4\" style:next-style-name=\"Right_20_Page\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Left_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RLeft_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span></text:p></style:header><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:footer></style:master-page>";
			bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
			Assert.IsTrue(returnValue1, "Mirrored Page with Bottom Inside Margin - Left Page Test failed");

			xpath = "//style:master-page[@style:name=\"Right_20_Page\"]";
			_validate = new ValidateXMLFile(styleOutput) { GetOuterXml = true };
			content = "<style:master-page style:name=\"Right_20_Page\" style:display-name=\"Right Page\" style:page-layout-name=\"pm5\" style:next-style-name=\"Left_20_Page\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><draw:frame draw:style-name=\"Mfr1\" draw:name=\"Frame1\" text:anchor-type=\"paragraph\" svg:y=\"42.25pt\" fo:min-width=\"145pt\" draw:z-index=\"1\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\"><draw:text-box fo:min-height=\"19.00pt\"><text:p text:style-name=\"MP1\"><text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Right_Guideword_R\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RRight_Guideword_R\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span></text:p></draw:text-box></draw:frame><style:header><text:p text:style-name=\"Header\"><text:span text:style-name=\"MT1\" /></text:p></style:header><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:footer><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:span text:style-name=\"MT1\"><text:tab /><text:tab /><text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Left_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RLeft_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span></text:span></text:p></style:header></style:master-page>";
			returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
			Assert.IsTrue(returnValue1, "Mirrored Page with Bottom Inside Margin - Right Page Test failed");
		}

		[Test]
		public void PageDictionaryTest6()
		{
			// Mirrored Page - Bottom Outside Margin
			const string file = "DictionaryPageTest";
			_projInfo.ProjectInputType = "Dictionary";
			_index = 6;
			string styleOutput = GetStyleOutput(file);
			string xpath = "//style:master-page[@style:name=\"Left_20_Page\"]";
			_validate = new ValidateXMLFile(styleOutput) { GetOuterXml = true };
			string content = "<style:master-page style:name=\"Left_20_Page\" style:display-name=\"Left Page\" style:page-layout-name=\"pm4\" style:next-style-name=\"Right_20_Page\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Left_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RLeft_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span></text:p></style:header><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:footer></style:master-page>";
			bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
			Assert.IsTrue(returnValue1, "Mirrored Page with Bottom Outside Margin - Left Page Test failed");

			xpath = "//style:master-page[@style:name=\"Right_20_Page\"]";
			_validate = new ValidateXMLFile(styleOutput) { GetOuterXml = true };
			content = "<style:master-page style:name=\"Right_20_Page\" style:display-name=\"Right Page\" style:page-layout-name=\"pm5\" style:next-style-name=\"Left_20_Page\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><draw:frame draw:style-name=\"Mfr1\" draw:name=\"Frame1\" text:anchor-type=\"paragraph\" svg:y=\"42.25pt\" fo:min-width=\"145pt\" draw:z-index=\"1\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\"><draw:text-box fo:min-height=\"19.00pt\"><text:p text:style-name=\"MP1\"><text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Right_Guideword_R\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RRight_Guideword_R\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span></text:p></draw:text-box></draw:frame><text:tab /><text:tab /><style:header><text:p text:style-name=\"Header\"><text:span text:style-name=\"MT1\" /></text:p></style:header><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:footer><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:span text:style-name=\"MT1\"><text:tab /><text:tab /><text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Left_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RLeft_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span></text:span></text:p></style:header></style:master-page>";
			returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
			Assert.IsTrue(returnValue1, "Mirrored Page with Bottom Outside Margin - Right Page Test failed");
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void ApplySpanFontStyleTestFull()
		{
			const string file = "ApplySpanFontStyleTestFull";
			_projInfo.ProjectInputType = "Dictionary";
			string styleOutput = GetStyleOutput(file);
			string xpath = "//style:style[@style:name='span_.bzh-fonipa_form_pronunciation_pronunciations_entry_letData_dicBody']";
			_validate = new ValidateXMLFile(styleOutput);
			_validate.ClassName = string.Empty;
			_validate.ClassProperty.Add("fo:font-family", "Doulos SIL");
			bool returnValue = _validate.ValidateNodeAttributesNS(1, xpath);
			Assert.IsTrue(returnValue, "Apply Span Font Doulos SIL Test failed");

			xpath = "//style:style[@style:name='span_.bzh_mainheadword_entry_letData_dicBody']";
			_validate = new ValidateXMLFile(styleOutput);
			_validate.ClassName = string.Empty;
			_validate.ClassProperty.Add("fo:font-family", "Charis SIL");
			returnValue = _validate.ValidateNodeAttributesNS(1, xpath);
			Assert.IsTrue(returnValue, "Apply Span Font Charis SIL Test failed");
		}


		[Test]
		public void PageDictionaryTest7()
		{
			// Mirrored Page - Bottom Center
			const string file = "DictionaryPageTest";
			_projInfo.ProjectInputType = "Dictionary";
			_index = 7;
			string styleOutput = GetStyleOutput(file);
			string xpath = "//style:master-page[@style:name=\"Left_20_Page\"]";
			_validate = new ValidateXMLFile(styleOutput) { GetOuterXml = true };
			string content = "<style:master-page style:name=\"Left_20_Page\" style:display-name=\"Left Page\" style:page-layout-name=\"pm4\" style:next-style-name=\"Right_20_Page\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Left_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RLeft_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span></text:p></style:header><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:footer></style:master-page>";
			bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
			Assert.IsTrue(returnValue1, "Mirrored Page with Bottom Center - Left Page Test failed");

			xpath = "//style:master-page[@style:name=\"Right_20_Page\"]";
			_validate = new ValidateXMLFile(styleOutput) { GetOuterXml = true };
			content = "<style:master-page style:name=\"Right_20_Page\" style:display-name=\"Right Page\" style:page-layout-name=\"pm5\" style:next-style-name=\"Left_20_Page\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><draw:frame draw:style-name=\"Mfr1\" draw:name=\"Frame1\" text:anchor-type=\"paragraph\" svg:y=\"42.25pt\" fo:min-width=\"145pt\" draw:z-index=\"1\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\"><draw:text-box fo:min-height=\"19.00pt\"><text:p text:style-name=\"MP1\"><text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Right_Guideword_R\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RRight_Guideword_R\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span></text:p></draw:text-box></draw:frame><text:tab /><style:header><text:p text:style-name=\"Header\"><text:span text:style-name=\"MT1\" /></text:p></style:header><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:footer><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:span text:style-name=\"MT1\"><text:tab /><text:tab /><text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Left_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RLeft_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span></text:span></text:p></style:header></style:master-page>";
			returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
			Assert.IsTrue(returnValue1, "Mirrored Page with Bottom Center - Right Page Test failed");
		}

		[Test]
		public void PageDictionaryTest8()
		{
			// Mirrored Page - None
			const string file = "DictionaryPageTest";
			_projInfo.ProjectInputType = "Dictionary";
			_index = 8;
			string styleOutput = GetStyleOutput(file);
			string xpath = "//style:master-page[@style:name=\"Left_20_Page\"]";
			_validate = new ValidateXMLFile(styleOutput) { GetOuterXml = true };
			string content = "<style:master-page style:name=\"Left_20_Page\" style:display-name=\"Left Page\" style:page-layout-name=\"pm4\" style:next-style-name=\"Right_20_Page\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Left_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RLeft_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span></text:p></style:header></style:master-page>";
			bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
			Assert.IsTrue(returnValue1, "Mirrored Page with None - Left Page Test failed");

			xpath = "//style:master-page[@style:name=\"Right_20_Page\"]";
			_validate = new ValidateXMLFile(styleOutput) { GetOuterXml = true };
			content = "<style:master-page style:name=\"Right_20_Page\" style:display-name=\"Right Page\" style:page-layout-name=\"pm5\" style:next-style-name=\"Left_20_Page\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><draw:frame draw:style-name=\"Mfr1\" draw:name=\"Frame1\" text:anchor-type=\"paragraph\" svg:y=\"42.25pt\" fo:min-width=\"145pt\" draw:z-index=\"1\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\"><draw:text-box fo:min-height=\"19.00pt\"><text:p text:style-name=\"MP1\"><text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Right_Guideword_R\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RRight_Guideword_R\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span></text:p></draw:text-box></draw:frame></text:p></style:footer><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:span text:style-name=\"MT1\"><text:tab /><text:tab /><text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Left_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RLeft_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span></text:span></text:p></style:header></style:master-page>";
			returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
			Assert.IsTrue(returnValue1, "Mirrored Page with None - Right Page Test failed");
		}

		[Test]
		public void PageDictionaryTest9()
		{
			// Mirrored Page - Bottom Inside Margin + Center Title Option enabled
			const string file = "DictionaryPageTest";
			_projInfo.ProjectInputType = "Dictionary";
			_index = 5;
			string styleOutput = GetStyleOutput(file);
			string xpath = "//style:master-page[@style:name=\"Left_20_Page\"]";
			_validate = new ValidateXMLFile(styleOutput) { GetOuterXml = true };
			string content = "<style:master-page style:name=\"Left_20_Page\" style:display-name=\"Left Page\" style:page-layout-name=\"pm4\" style:next-style-name=\"Right_20_Page\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Left_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RLeft_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span></text:p></style:header><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:footer></style:master-page>";
			bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
			Assert.IsTrue(returnValue1, "Mirrored Page with Bottom Inside Margin - Left Page Test failed");

			xpath = "//style:master-page[@style:name=\"Right_20_Page\"]";
			_validate = new ValidateXMLFile(styleOutput) { GetOuterXml = true };
			content = "<style:master-page style:name=\"Right_20_Page\" style:display-name=\"Right Page\" style:page-layout-name=\"pm5\" style:next-style-name=\"Left_20_Page\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><draw:frame draw:style-name=\"Mfr1\" draw:name=\"Frame1\" text:anchor-type=\"paragraph\" svg:y=\"42.25pt\" fo:min-width=\"145pt\" draw:z-index=\"1\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\"><draw:text-box fo:min-height=\"19.00pt\"><text:p text:style-name=\"MP1\"><text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Right_Guideword_R\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RRight_Guideword_R\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span></text:p></draw:text-box></draw:frame><style:header><text:p text:style-name=\"Header\"><text:span text:style-name=\"MT1\" /></text:p></style:header><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:footer><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:span text:style-name=\"MT1\"><text:tab /><text:tab /><text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Left_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RLeft_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span></text:span></text:p></style:header></style:master-page>";
			returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
			Assert.IsTrue(returnValue1, "Mirrored Page with Bottom Inside Margin - Right Page Test failed");
		}

	    [Test]
	    public void PageFrontMatterOption1()
	    {
			// FrontMatter - All Options
			const string file = "FrontMatterOptions";
			_projInfo.ProjectInputType = "Dictionary";
			_index = 1;
			string styleOutput = GetStyleOutput(file);
			string xpath = "//office:master-styles";
			_validate = new ValidateXMLFile(styleOutput) { GetOuterXml = true };
			string content = "<office:master-styles xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\"><style:master-page style:name=\"Standard\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Cover_20_Page\" style:display-name=\"Cover Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Title_20_Page\" style:display-name=\"Title Page\" style:next-style-name=\"CopyRight_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"CopyRight_20_Page\" style:display-name=\"CopyRight Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"TableofContents_20_Page\" style:display-name=\"TableofContents Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:page-number style:num-format=\"i\" text:select-page=\"current\">xv</text:page-number></text:p></style:footer></style:master-page><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"First_20_Page\" style:display-name=\"First Page\" style:next-style-name=\"XHTML\" style:page-layout-name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:header></style:master-page><style:master-page style:name=\"XHTML\" style:page-layout-name=\"pm2\" style:next-style-name=\"XHTML\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Left_Guideword_L\" office:value-type=\"string\" /></text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RLeft_Guideword_L\" office:value-type=\"string\" /></text:span><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:header><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><draw:frame draw:style-name=\"Mfr1\" draw:name=\"Frame1\" text:anchor-type=\"paragraph\" svg:y=\"55.94291pt\" fo:min-width=\"145pt\" draw:z-index=\"1\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\"><draw:text-box fo:min-height=\"19.00pt\"><text:p text:style-name=\"MP1\"><text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Right_Guideword_R\" office:value-type=\"string\" /></text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RRight_Guideword_R\" office:value-type=\"string\" /></text:span></text:p></draw:text-box></draw:frame></text:p></style:footer></style:master-page></office:master-styles>";
			bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
			Assert.IsTrue(returnValue1, "Mirrored Page with None - Left Page Test failed");
	    }

		[Test]
		public void PageFrontMatterOption2()
		{
			// FrontMatter - All Options except TOC
			const string file = "FrontMatterOptions";
			_projInfo.ProjectInputType = "Dictionary";
			_index = 2;
			string styleOutput = GetStyleOutput(file);
			string xpath = "//office:master-styles";
			_validate = new ValidateXMLFile(styleOutput) { GetOuterXml = true };
			string content = "<office:master-styles xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\"><style:master-page style:name=\"Standard\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Cover_20_Page\" style:display-name=\"Cover Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Title_20_Page\" style:display-name=\"Title Page\" style:next-style-name=\"CopyRight_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"CopyRight_20_Page\" style:display-name=\"CopyRight Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"TableofContents_20_Page\" style:display-name=\"TableofContents Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:page-number style:num-format=\"i\" text:select-page=\"current\">xv</text:page-number></text:p></style:footer></style:master-page><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"First_20_Page\" style:display-name=\"First Page\" style:next-style-name=\"XHTML\" style:page-layout-name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:header></style:master-page><style:master-page style:name=\"XHTML\" style:page-layout-name=\"pm2\" style:next-style-name=\"XHTML\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /></office:master-styles>";
			bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
			Assert.IsTrue(returnValue1, "Mirrored Page with None - Left Page Test failed");
		}

		[Test]
		public void PageFrontMatterOption3()
		{
			// FrontMatter - All Options except copyright, TOC
			const string file = "FrontMatterOptions";
			_projInfo.ProjectInputType = "Dictionary";
			_index = 3;
			string styleOutput = GetStyleOutput(file);
			string xpath = "//office:master-styles";
			_validate = new ValidateXMLFile(styleOutput) { GetOuterXml = true };
			string content = "<office:master-styles xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\"><style:master-page style:name=\"Standard\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Cover_20_Page\" style:display-name=\"Cover Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Title_20_Page\" style:display-name=\"Title Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"TableofContents_20_Page\" style:display-name=\"TableofContents Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:page-number style:num-format=\"i\" text:select-page=\"current\">xv</text:page-number></text:p></style:footer></style:master-page><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"First_20_Page\" style:display-name=\"First Page\" style:next-style-name=\"XHTML\" style:page-layout-name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:header></style:master-page><style:master-page style:name=\"XHTML\" style:page-layout-name=\"pm2\" style:next-style-name=\"XHTML\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /></office:master-styles>";
			bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
			Assert.IsTrue(returnValue1, "Mirrored Page with None - Left Page Test failed");
		}

		[Test]
		public void PageFrontMatterOption4()
		{
			// FrontMatter - Only CoverImage and Title
			const string file = "FrontMatterOptions";
			_projInfo.ProjectInputType = "Dictionary";
			_index = 4;
			string styleOutput = GetStyleOutput(file);
			string xpath = "//office:master-styles";
			_validate = new ValidateXMLFile(styleOutput) { GetOuterXml = true };
			string content = "<office:master-styles xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\"><style:master-page style:name=\"Standard\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Cover_20_Page\" style:display-name=\"Cover Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Title_20_Page\" style:display-name=\"Title Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"TableofContents_20_Page\" style:display-name=\"TableofContents Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:page-number style:num-format=\"i\" text:select-page=\"current\">xv</text:page-number></text:p></style:footer></style:master-page><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"First_20_Page\" style:display-name=\"First Page\" style:next-style-name=\"XHTML\" style:page-layout-name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:header></style:master-page><style:master-page style:name=\"XHTML\" style:page-layout-name=\"pm2\" style:next-style-name=\"XHTML\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /></office:master-styles>";
			bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
			Assert.IsTrue(returnValue1, "Mirrored Page with None - Left Page Test failed");
		}

	    [Test]
        public void PageTest1()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "PageTest";
            _index = 1;
            string styleOutput = GetStyleOutput(file);
            _index = 0; // reset

            string xpath = "//office:automatic-styles";
            _validate = new ValidateXMLFile(styleOutput);
            string content = string.Empty;
            if (_isLinux)
            {
				content = "<style:style style:name=\"PageHeaderFooter0\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"First top Left\" fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"Right top Center\" fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" /></style:style><style:style style:name=\"PageHeaderFooter3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter4\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter5\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter12\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter13\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter14\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter15\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter16\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter17\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter18\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter19\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"Right top Center\" fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter20\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter21\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter22\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter23\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"MP1\" style:family=\"paragraph\" style:parent-style-name=\"Frame_20_contents\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:paragraph-properties fo:text-align=\"start\" style:justify-single-word=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Charis SIL\" style:font-name-asian=\"Charis SIL\" style:font-name-complex=\"Charis SIL\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"Mfr1\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"left\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" draw:textarea-vertical-align=\"middle\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:style style:name=\"Mfr3\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"center\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:page-layout style:name=\"pm1\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style /></style:page-layout><style:page-layout style:name=\"pm2\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#0000ff\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm6\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm4\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"15.84pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm5\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm13\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#0000ff\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties></style:page-layout>";
            }
            else
            {
				content = "<style:style style:name=\"PageHeaderFooter0\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"First top Left\" fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"Right top Center\" fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" /></style:style><style:style style:name=\"PageHeaderFooter3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter4\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter5\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter12\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter13\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter14\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter15\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter16\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter17\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter18\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter19\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"Right top Center\" fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter20\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter21\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter22\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter23\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"MP1\" style:family=\"paragraph\" style:parent-style-name=\"Frame_20_contents\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:paragraph-properties fo:text-align=\"start\" style:justify-single-word=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Charis SIL\" style:font-name-asian=\"Charis SIL\" style:font-name-complex=\"Charis SIL\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"Mfr1\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"left\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" draw:textarea-vertical-align=\"middle\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:style style:name=\"Mfr3\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"center\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:page-layout style:name=\"pm1\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style /></style:page-layout><style:page-layout style:name=\"pm2\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#0000ff\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm6\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm4\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"15.84pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm5\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm13\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#0000ff\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties></style:page-layout>";
            }
            bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "PageTest failed");

            xpath = "//office:master-styles";
            _validate = new ValidateXMLFile(styleOutput);
            if (_isLinux)
            {
                content = "<style:master-page style:name=\"Standard\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Cover_20_Page\" style:display-name=\"Cover Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Title_20_Page\" style:display-name=\"Title Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"TableofContents_20_Page\" style:display-name=\"TableofContents Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:page-number style:num-format=\"i\" text:select-page=\"current\">xv</text:page-number></text:p></style:footer></style:master-page><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"First_20_Page\" style:display-name=\"First Page\" style:next-style-name=\"XHTML\" style:page-layout-name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"XHTML\" style:page-layout-name=\"pm2\" style:next-style-name=\"XHTML\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" />";
            }
            else
            {
                content =
					"<style:master-page style:name=\"Standard\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Cover_20_Page\" style:display-name=\"Cover Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Title_20_Page\" style:display-name=\"Title Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"TableofContents_20_Page\" style:display-name=\"TableofContents Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:page-number style:num-format=\"i\" text:select-page=\"current\">xv</text:page-number></text:p></style:footer></style:master-page><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"First_20_Page\" style:display-name=\"First Page\" style:next-style-name=\"Left_20_Page\" style:page-layout-name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:header></style:master-page><style:master-page style:name=\"Left_20_Page\" style:display-name=\"Left Page\" style:page-layout-name=\"pm4\" style:next-style-name=\"Right_20_Page\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Right_20_Page\" style:display-name=\"Right Page\" style:page-layout-name=\"pm5\" style:next-style-name=\"Left_20_Page\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" />";
            }
            returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "PageTest-master failed");
        }


        [Test]
        public void PageTest2()
        {
			_projInfo.ProjectInputType = "Dictionary";
            const string file = "PageTest";
            _index = 2;
            string styleOutput = GetStyleOutput(file);
            _index = 0; // reset
            string xpath = "//office:automatic-styles";
            _validate = new ValidateXMLFile(styleOutput);
            string content = string.Empty;
            if (_isLinux)
            {
				content = "<style:style style:name=\"PageHeaderFooter0\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter4\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter5\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter12\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter13\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#0000ff\" fo:font-size=\"14pt\" fo:font-style=\"italic\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter14\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter15\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter16\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter17\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter18\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter19\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter20\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter21\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter22\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter23\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"MP1\" style:family=\"paragraph\" style:parent-style-name=\"Frame_20_contents\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:paragraph-properties fo:text-align=\"start\" style:justify-single-word=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Charis SIL\" style:font-name-asian=\"Charis SIL\" style:font-name-complex=\"Charis SIL\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"Mfr1\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"left\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" draw:textarea-vertical-align=\"middle\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:style style:name=\"Mfr3\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"center\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:page-layout style:name=\"pm1\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style /></style:page-layout><style:page-layout style:name=\"pm2\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#0000ff\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm6\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm4\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#008000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"15.84pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm5\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm13\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#0000ff\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties></style:page-layout>";
            }
            else
            {
				content = "<style:style style:name=\"PageHeaderFooter0\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter4\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter5\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter12\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter13\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#0000ff\" fo:font-size=\"14pt\" fo:font-style=\"italic\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter14\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter15\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter16\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter17\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter18\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter19\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter20\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter21\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter22\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter23\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"MP1\" style:family=\"paragraph\" style:parent-style-name=\"Frame_20_contents\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:paragraph-properties fo:text-align=\"start\" style:justify-single-word=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Charis SIL\" style:font-name-asian=\"Charis SIL\" style:font-name-complex=\"Charis SIL\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"Mfr1\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"left\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" draw:textarea-vertical-align=\"middle\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:style style:name=\"Mfr3\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"center\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:page-layout style:name=\"pm1\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style /></style:page-layout><style:page-layout style:name=\"pm2\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#0000ff\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm6\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm4\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#008000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"15.84pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm5\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm13\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#0000ff\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties></style:page-layout>";
            }
            bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "PageTest failed");

            xpath = "//office:master-styles";
            _validate = new ValidateXMLFile(styleOutput);
            if (_isLinux)
            {
                content = "<style:master-page style:name=\"Standard\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Cover_20_Page\" style:display-name=\"Cover Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Title_20_Page\" style:display-name=\"Title Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"TableofContents_20_Page\" style:display-name=\"TableofContents Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:page-number style:num-format=\"i\" text:select-page=\"current\">xv</text:page-number></text:p></style:footer></style:master-page><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"First_20_Page\" style:display-name=\"First Page\" style:next-style-name=\"XHTML\" style:page-layout-name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"XHTML\" style:page-layout-name=\"pm2\" style:next-style-name=\"XHTML\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" />";
            }
            else
            {
                content = "<style:master-page style:name=\"Standard\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Cover_20_Page\" style:display-name=\"Cover Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Title_20_Page\" style:display-name=\"Title Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"TableofContents_20_Page\" style:display-name=\"TableofContents Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:page-number style:num-format=\"i\" text:select-page=\"current\">xv</text:page-number></text:p></style:footer></style:master-page><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"First_20_Page\" style:display-name=\"First Page\" style:next-style-name=\"Left_20_Page\" style:page-layout-name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Left_20_Page\" style:display-name=\"Left Page\" style:page-layout-name=\"pm4\" style:next-style-name=\"Right_20_Page\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:header></style:master-page><style:master-page style:name=\"Right_20_Page\" style:display-name=\"Right Page\" style:page-layout-name=\"pm5\" style:next-style-name=\"Left_20_Page\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" />";
            }
            returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "PageTest-master failed");
        }

        [Test]
        public void PageTest3()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "PageTest";
            _index = 3;
            string styleOutput = GetStyleOutput(file);
            _index = 0; // reset

            string xpath = "//office:automatic-styles";
            _validate = new ValidateXMLFile(styleOutput);
            string content = string.Empty;
            if (_isLinux)
            {
				content = "<style:style style:name=\"PageHeaderFooter0\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter4\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter5\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter12\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ffa500\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter13\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter14\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter15\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter16\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter17\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter18\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter19\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter20\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter21\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter22\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter23\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"MP1\" style:family=\"paragraph\" style:parent-style-name=\"Frame_20_contents\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:paragraph-properties fo:text-align=\"start\" style:justify-single-word=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Charis SIL\" style:font-name-asian=\"Charis SIL\" style:font-name-complex=\"Charis SIL\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"Mfr1\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"left\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" draw:textarea-vertical-align=\"middle\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:style style:name=\"Mfr3\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"center\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:page-layout style:name=\"pm1\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style /></style:page-layout><style:page-layout style:name=\"pm2\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#0000ff\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm6\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm4\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"15.84pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm5\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ee82ee\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm13\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#0000ff\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties></style:page-layout>";
            }
            else
            {
				content = "<style:style style:name=\"PageHeaderFooter0\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter4\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter5\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter12\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ffa500\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter13\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter14\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter15\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter16\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter17\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter18\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter19\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter20\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter21\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter22\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter23\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"MP1\" style:family=\"paragraph\" style:parent-style-name=\"Frame_20_contents\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:paragraph-properties fo:text-align=\"start\" style:justify-single-word=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Charis SIL\" style:font-name-asian=\"Charis SIL\" style:font-name-complex=\"Charis SIL\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"Mfr1\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"left\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" draw:textarea-vertical-align=\"middle\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:style style:name=\"Mfr3\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"center\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:page-layout style:name=\"pm1\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style /></style:page-layout><style:page-layout style:name=\"pm2\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#0000ff\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm6\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm4\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"15.84pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm5\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ee82ee\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm13\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#0000ff\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties></style:page-layout>";
            }
            bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "PageTest failed");

            xpath = "//office:master-styles";
            _validate = new ValidateXMLFile(styleOutput);
            if (_isLinux)
            {
                content = "<style:master-page style:name=\"Standard\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Cover_20_Page\" style:display-name=\"Cover Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Title_20_Page\" style:display-name=\"Title Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"TableofContents_20_Page\" style:display-name=\"TableofContents Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:page-number style:num-format=\"i\" text:select-page=\"current\">xv</text:page-number></text:p></style:footer></style:master-page><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"First_20_Page\" style:display-name=\"First Page\" style:next-style-name=\"XHTML\" style:page-layout-name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"XHTML\" style:page-layout-name=\"pm2\" style:next-style-name=\"XHTML\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" />";
            }
            else
            {
                content = "<style:master-page style:name=\"Standard\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Cover_20_Page\" style:display-name=\"Cover Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Title_20_Page\" style:display-name=\"Title Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"TableofContents_20_Page\" style:display-name=\"TableofContents Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:page-number style:num-format=\"i\" text:select-page=\"current\">xv</text:page-number></text:p></style:footer></style:master-page><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"First_20_Page\" style:display-name=\"First Page\" style:next-style-name=\"Left_20_Page\" style:page-layout-name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Left_20_Page\" style:display-name=\"Left Page\" style:page-layout-name=\"pm4\" style:next-style-name=\"Right_20_Page\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:header></style:master-page><style:master-page style:name=\"Right_20_Page\" style:display-name=\"Right Page\" style:page-layout-name=\"pm5\" style:next-style-name=\"Left_20_Page\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" />";
            }
            returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "PageTest-master failed");
        }

        [Test]
        public void PageTest4()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "PageTest";
            _index = 4;
            string styleOutput = GetStyleOutput(file);
            _index = 0; // reset

            string xpath = "//office:automatic-styles";
            _validate = new ValidateXMLFile(styleOutput);
            string content = string.Empty;
            if (_isLinux)
            {
                content =
					"<style:style style:name=\"PageHeaderFooter0\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"Right top left\" fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" /></style:style><style:style style:name=\"PageHeaderFooter2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ffff00\" fo:font-size=\"16pt\" fo:font-style=\"italic\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ffa500\" fo:font-size=\"8pt\" fo:font-style=\"italic\" fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter4\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter5\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter12\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter13\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter14\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ffff00\" fo:font-size=\"16pt\" fo:font-style=\"italic\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter15\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ffa500\" fo:font-size=\"8pt\" fo:font-style=\"italic\" fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter16\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter17\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter18\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"Right top left\" fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter19\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" /></style:style><style:style style:name=\"PageHeaderFooter20\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ffff00\" fo:font-size=\"16pt\" fo:font-style=\"italic\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter21\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ffa500\" fo:font-size=\"8pt\" fo:font-style=\"italic\" fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter22\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter23\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"MP1\" style:family=\"paragraph\" style:parent-style-name=\"Frame_20_contents\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:paragraph-properties fo:text-align=\"start\" style:justify-single-word=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Charis SIL\" style:font-name-asian=\"Charis SIL\" style:font-name-complex=\"Charis SIL\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"Mfr1\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"left\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" draw:textarea-vertical-align=\"middle\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:style style:name=\"Mfr3\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"center\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:page-layout style:name=\"pm1\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style /></style:page-layout><style:page-layout style:name=\"pm2\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" svg:height=\"14.21pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#0000ff\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" svg:height=\"14.21pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm6\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" svg:height=\"14.21pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm4\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#008000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" svg:height=\"15.84pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm5\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ee82ee\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" svg:height=\"14.21pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm13\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#0000ff\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties></style:page-layout>";
            }
            else
            {
                content =
					"<style:style style:name=\"PageHeaderFooter0\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"Right top left\" fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" /></style:style><style:style style:name=\"PageHeaderFooter2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ffff00\" fo:font-size=\"16pt\" fo:font-style=\"italic\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ffa500\" fo:font-size=\"8pt\" fo:font-style=\"italic\" fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter4\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter5\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter12\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter13\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter14\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ffff00\" fo:font-size=\"16pt\" fo:font-style=\"italic\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter15\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ffa500\" fo:font-size=\"8pt\" fo:font-style=\"italic\" fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter16\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter17\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter18\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"Right top left\" fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter19\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" /></style:style><style:style style:name=\"PageHeaderFooter20\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ffff00\" fo:font-size=\"16pt\" fo:font-style=\"italic\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter21\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ffa500\" fo:font-size=\"8pt\" fo:font-style=\"italic\" fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter22\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter23\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"MP1\" style:family=\"paragraph\" style:parent-style-name=\"Frame_20_contents\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:paragraph-properties fo:text-align=\"start\" style:justify-single-word=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Charis SIL\" style:font-name-asian=\"Charis SIL\" style:font-name-complex=\"Charis SIL\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"Mfr1\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"left\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" draw:textarea-vertical-align=\"middle\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:style style:name=\"Mfr3\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"center\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:page-layout style:name=\"pm1\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style /></style:page-layout><style:page-layout style:name=\"pm2\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" svg:height=\"14.21pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#0000ff\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" svg:height=\"14.21pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm6\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" svg:height=\"14.21pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm4\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#008000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" svg:height=\"15.84pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm5\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ee82ee\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" svg:height=\"14.21pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm13\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#0000ff\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties></style:page-layout>";
            }
            bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "PageTest failed");

            xpath = "//office:master-styles";
            _validate = new ValidateXMLFile(styleOutput);
            if (_isLinux)
            {
                content =
                    "<style:master-page style:name=\"Standard\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Cover_20_Page\" style:display-name=\"Cover Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Title_20_Page\" style:display-name=\"Title Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"TableofContents_20_Page\" style:display-name=\"TableofContents Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:page-number style:num-format=\"i\" text:select-page=\"current\">xv</text:page-number></text:p></style:footer></style:master-page><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"First_20_Page\" style:display-name=\"First Page\" style:next-style-name=\"XHTML\" style:page-layout-name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"XHTML\" style:page-layout-name=\"pm2\" style:next-style-name=\"XHTML\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" />";
            }
            else
            {
                content =
					"<style:master-page style:name=\"Standard\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Cover_20_Page\" style:display-name=\"Cover Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Title_20_Page\" style:display-name=\"Title Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"TableofContents_20_Page\" style:display-name=\"TableofContents Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:page-number style:num-format=\"i\" text:select-page=\"current\">xv</text:page-number></text:p></style:footer></style:master-page><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"First_20_Page\" style:display-name=\"First Page\" style:next-style-name=\"Left_20_Page\" style:page-layout-name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span><text:tab /><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:header><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:footer></style:master-page><style:master-page style:name=\"Left_20_Page\" style:display-name=\"Left Page\" style:page-layout-name=\"pm4\" style:next-style-name=\"Right_20_Page\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:header><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:footer></style:master-page><style:master-page style:name=\"Right_20_Page\" style:display-name=\"Right Page\" style:page-layout-name=\"pm5\" style:next-style-name=\"Left_20_Page\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span><text:tab /><text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Left_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RLeft_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:tab /><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:header><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><style:header><text:p text:style-name=\"Header\"><text:span text:style-name=\"MT1\" /></text:p></style:header><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:footer></style:master-page>";
            }
            returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "PageTest-master failed");
        }

        [Test]
        public void PageTest5()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "PageTest";
            _index = 5;
            string styleOutput = GetStyleOutput(file);
            _index = 0; // reset

            string xpath = "//office:automatic-styles";
            _validate = new ValidateXMLFile(styleOutput);
            string content = string.Empty;
            if (_isLinux)
            {
                content =
					"<style:style style:name=\"PageHeaderFooter0\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#0000ff\" fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter4\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ffff00\" fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter5\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter12\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter13\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#0000ff\" fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter14\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter15\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter16\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ffff00\" fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter17\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter18\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter19\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#0000ff\" fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter20\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter21\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter22\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ffff00\" fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter23\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"MP1\" style:family=\"paragraph\" style:parent-style-name=\"Frame_20_contents\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:paragraph-properties fo:text-align=\"center\" style:justify-single-word=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /><style:paragraph-properties fo:text-align=\"start\" style:justify-single-word=\"false\" fo:background-color=\"#ffffff\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /><style:paragraph-properties fo:text-align=\"start\" style:justify-single-word=\"false\" fo:background-color=\"#ffffff\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Charis SIL\" style:font-name-asian=\"Charis SIL\" style:font-name-complex=\"Charis SIL\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /><style:paragraph-properties fo:text-align=\"end\" style:justify-single-word=\"false\" fo:background-color=\"#ffffff\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"Mfr1\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"center\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" draw:textarea-vertical-align=\"middle\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:style style:name=\"Mfr3\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"center\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:page-layout style:name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style /></style:page-layout><style:page-layout style:name=\"pm2\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" svg:height=\"14.21pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" svg:height=\"14.21pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm6\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" svg:height=\"14.21pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm13\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties></style:page-layout>";
            }
            else
            {
                content =
					"<style:style style:name=\"PageHeaderFooter0\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#0000ff\" fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter4\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ffff00\" fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter5\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter12\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter13\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#0000ff\" fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter14\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter15\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter16\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ffff00\" fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter17\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter18\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter19\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#0000ff\" fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter20\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter21\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter22\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ffff00\" fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter23\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"MP1\" style:family=\"paragraph\" style:parent-style-name=\"Frame_20_contents\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:paragraph-properties fo:text-align=\"center\" style:justify-single-word=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /><style:paragraph-properties fo:text-align=\"start\" style:justify-single-word=\"false\" fo:background-color=\"#ffffff\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /><style:paragraph-properties fo:text-align=\"start\" style:justify-single-word=\"false\" fo:background-color=\"#ffffff\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Charis SIL\" style:font-name-asian=\"Charis SIL\" style:font-name-complex=\"Charis SIL\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /><style:paragraph-properties fo:text-align=\"end\" style:justify-single-word=\"false\" fo:background-color=\"#ffffff\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"Mfr1\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"center\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" draw:textarea-vertical-align=\"middle\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:style style:name=\"Mfr3\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"center\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:page-layout style:name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style /></style:page-layout><style:page-layout style:name=\"pm2\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" svg:height=\"14.21pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" svg:height=\"14.21pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm6\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" svg:height=\"14.21pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm13\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties></style:page-layout>";
            }
            bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "PageTest failed");

            xpath = "//office:master-styles";
            _validate = new ValidateXMLFile(styleOutput);
            if (_isLinux)
            {
                content =
                    "<style:master-page style:name=\"Standard\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Cover_20_Page\" style:display-name=\"Cover Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Title_20_Page\" style:display-name=\"Title Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"TableofContents_20_Page\" style:display-name=\"TableofContents Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:page-number style:num-format=\"i\" text:select-page=\"current\">xv</text:page-number></text:p></style:footer></style:master-page><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"First_20_Page\" style:display-name=\"First Page\" style:next-style-name=\"XHTML\" style:page-layout-name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"XHTML\" style:page-layout-name=\"pm2\" style:next-style-name=\"XHTML\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" />";
            }
            else
            {
                content =
					"<style:master-page style:name=\"Standard\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Cover_20_Page\" style:display-name=\"Cover Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Title_20_Page\" style:display-name=\"Title Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"TableofContents_20_Page\" style:display-name=\"TableofContents Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:page-number style:num-format=\"i\" text:select-page=\"current\">xv</text:page-number></text:p></style:footer></style:master-page><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"First_20_Page\" style:display-name=\"First Page\" style:next-style-name=\"XHTML\" style:page-layout-name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:header><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:footer></style:master-page><style:master-page style:name=\"XHTML\" style:page-layout-name=\"pm2\" style:next-style-name=\"XHTML\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:header><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:footer></style:master-page>";
            }
            returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "PageTest-master failed");
        }

        [Test]
        public void PageTest6()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "PageTest";
            _index = 6;
            string styleOutput = GetStyleOutput(file);
            _index = 0; // reset

            string xpath = "//office:automatic-styles";
            _validate = new ValidateXMLFile(styleOutput);
            string content = string.Empty;
            if (_isLinux)
            {
                content =
					"<style:style style:name=\"PageHeaderFooter0\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"***\" /></style:style><style:style style:name=\"PageHeaderFooter1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" /></style:style><style:style style:name=\"PageHeaderFooter2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"@@@\" fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter4\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter5\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter12\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter13\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter14\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter15\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter16\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter17\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter18\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"***\" /></style:style><style:style style:name=\"PageHeaderFooter19\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" /></style:style><style:style style:name=\"PageHeaderFooter20\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"@@@\" fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter21\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter22\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter23\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"MP1\" style:family=\"paragraph\" style:parent-style-name=\"Frame_20_contents\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:paragraph-properties fo:text-align=\"start\" style:justify-single-word=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Charis SIL\" style:font-name-asian=\"Charis SIL\" style:font-name-complex=\"Charis SIL\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"Mfr1\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"left\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" draw:textarea-vertical-align=\"middle\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:style style:name=\"Mfr3\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"center\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:page-layout style:name=\"pm1\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style /></style:page-layout><style:page-layout style:name=\"pm2\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm6\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm4\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"15.84pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm5\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ee82ee\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm13\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties></style:page-layout>";
            }
            else
            {
                content =
					"<style:style style:name=\"PageHeaderFooter0\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"***\" /></style:style><style:style style:name=\"PageHeaderFooter1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" /></style:style><style:style style:name=\"PageHeaderFooter2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"@@@\" fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter4\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter5\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter12\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter13\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter14\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter15\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter16\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter17\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter18\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"***\" /></style:style><style:style style:name=\"PageHeaderFooter19\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" /></style:style><style:style style:name=\"PageHeaderFooter20\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"@@@\" fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter21\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter22\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter23\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"MP1\" style:family=\"paragraph\" style:parent-style-name=\"Frame_20_contents\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:paragraph-properties fo:text-align=\"start\" style:justify-single-word=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Charis SIL\" style:font-name-asian=\"Charis SIL\" style:font-name-complex=\"Charis SIL\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"Mfr1\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"left\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" draw:textarea-vertical-align=\"middle\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:style style:name=\"Mfr3\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"center\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:page-layout style:name=\"pm1\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style /></style:page-layout><style:page-layout style:name=\"pm2\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm6\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm4\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"15.84pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm5\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ee82ee\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm13\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties></style:page-layout>";
            }
            bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "PageTest failed");

            xpath = "//office:master-styles";
            _validate = new ValidateXMLFile(styleOutput);
            if (_isLinux)
            {
                content =
                    "<style:master-page style:name=\"Standard\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Cover_20_Page\" style:display-name=\"Cover Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Title_20_Page\" style:display-name=\"Title Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"TableofContents_20_Page\" style:display-name=\"TableofContents Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:page-number style:num-format=\"i\" text:select-page=\"current\">xv</text:page-number></text:p></style:footer></style:master-page><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"First_20_Page\" style:display-name=\"First Page\" style:next-style-name=\"XHTML\" style:page-layout-name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"XHTML\" style:page-layout-name=\"pm2\" style:next-style-name=\"XHTML\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" />";
            }
            else
            {
                content =
                    "<style:master-page style:name=\"Standard\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Cover_20_Page\" style:display-name=\"Cover Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Title_20_Page\" style:display-name=\"Title Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"TableofContents_20_Page\" style:display-name=\"TableofContents Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:page-number style:num-format=\"i\" text:select-page=\"current\">xv</text:page-number></text:p></style:footer></style:master-page><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"First_20_Page\" style:display-name=\"First Page\" style:next-style-name=\"Left_20_Page\" style:page-layout-name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:header></style:master-page><style:master-page style:name=\"Left_20_Page\" style:display-name=\"Left Page\" style:page-layout-name=\"pm4\" style:next-style-name=\"Right_20_Page\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Right_20_Page\" style:display-name=\"Right Page\" style:page-layout-name=\"pm5\" style:next-style-name=\"Left_20_Page\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span><text:tab /><text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Left_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RLeft_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span></text:p></style:header></style:master-page>";
            }
            returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "PageTest-master failed");
        }

        [Test]
        public void PageTest7()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "PageTest";
            _index = 7;
            string styleOutput = GetStyleOutput(file);
            _index = 0; // reset

            string xpath = "//office:automatic-styles";
            _validate = new ValidateXMLFile(styleOutput);

            string content = string.Empty;
            if (_isLinux)
            {
                content =
					"<style:style style:name=\"PageHeaderFooter0\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ffff00\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter4\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter5\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter12\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter13\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ffff00\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter14\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter15\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter16\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ff0000\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter17\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter18\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter19\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ffff00\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter20\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter21\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter22\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter23\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"MP1\" style:family=\"paragraph\" style:parent-style-name=\"Frame_20_contents\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:paragraph-properties fo:text-align=\"start\" style:justify-single-word=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Charis SIL\" style:font-name-asian=\"Charis SIL\" style:font-name-complex=\"Charis SIL\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"Mfr1\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"left\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" draw:textarea-vertical-align=\"middle\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:style style:name=\"Mfr3\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"center\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:page-layout style:name=\"pm1\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style /></style:page-layout><style:page-layout style:name=\"pm2\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm6\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm4\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#008000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" svg:height=\"15.84pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm5\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm13\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties></style:page-layout>";
            }
            else
            {
				content =
		            "<style:style style:name=\"PageHeaderFooter0\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ffff00\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter4\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter5\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter12\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter13\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ffff00\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter14\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter15\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter16\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ff0000\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter17\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter18\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter19\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ffff00\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter20\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter21\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter22\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter23\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"MP1\" style:family=\"paragraph\" style:parent-style-name=\"Frame_20_contents\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:paragraph-properties fo:text-align=\"start\" style:justify-single-word=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Charis SIL\" style:font-name-asian=\"Charis SIL\" style:font-name-complex=\"Charis SIL\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"Mfr1\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"left\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" draw:textarea-vertical-align=\"middle\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:style style:name=\"Mfr3\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"center\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:page-layout style:name=\"pm1\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style /></style:page-layout><style:page-layout style:name=\"pm2\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm6\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm4\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#008000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" svg:height=\"15.84pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm5\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm13\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties></style:page-layout>";
            }
            bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "PageTest failed");

            xpath = "//office:master-styles";
            _validate = new ValidateXMLFile(styleOutput);
            if (_isLinux)
            {
                content =
                    "<style:master-page style:name=\"Standard\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Cover_20_Page\" style:display-name=\"Cover Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Title_20_Page\" style:display-name=\"Title Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"TableofContents_20_Page\" style:display-name=\"TableofContents Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:page-number style:num-format=\"i\" text:select-page=\"current\">xv</text:page-number></text:p></style:footer></style:master-page><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"First_20_Page\" style:display-name=\"First Page\" style:next-style-name=\"XHTML\" style:page-layout-name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"XHTML\" style:page-layout-name=\"pm2\" style:next-style-name=\"XHTML\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" />";
            }
            else
            {
                content =
                    "<style:master-page style:name=\"Standard\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Cover_20_Page\" style:display-name=\"Cover Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Title_20_Page\" style:display-name=\"Title Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"TableofContents_20_Page\" style:display-name=\"TableofContents Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:page-number style:num-format=\"i\" text:select-page=\"current\">xv</text:page-number></text:p></style:footer></style:master-page><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"First_20_Page\" style:display-name=\"First Page\" style:next-style-name=\"Left_20_Page\" style:page-layout-name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:header></style:master-page><style:master-page style:name=\"Left_20_Page\" style:display-name=\"Left Page\" style:page-layout-name=\"pm4\" style:next-style-name=\"Right_20_Page\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:header><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:footer></style:master-page><style:master-page style:name=\"Right_20_Page\" style:display-name=\"Right Page\" style:page-layout-name=\"pm5\" style:next-style-name=\"Left_20_Page\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span><text:tab /><text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Left_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RLeft_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span></text:p></style:header></style:master-page>";
            }
            returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "PageTest-master failed");
        }

        [Test]
        public void PageTest8()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "PageTest";
            _index = 8;
            string styleOutput = GetStyleOutput(file);
            _index = 0; // reset

            string xpath = "//office:automatic-styles";
            _validate = new ValidateXMLFile(styleOutput);
            string content = string.Empty;
            if (_isLinux)
            {
                content =
					"<style:style style:name=\"PageHeaderFooter0\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ff0000\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ffff00\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter4\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter5\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter12\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter13\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter14\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ff0000\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter15\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter16\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ff0000\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter17\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter18\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter19\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter20\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ff0000\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter21\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ffff00\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter22\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter23\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"MP1\" style:family=\"paragraph\" style:parent-style-name=\"Frame_20_contents\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:paragraph-properties fo:text-align=\"start\" style:justify-single-word=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Charis SIL\" style:font-name-asian=\"Charis SIL\" style:font-name-complex=\"Charis SIL\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"Mfr1\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"left\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" draw:textarea-vertical-align=\"middle\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:style style:name=\"Mfr3\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"center\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:page-layout style:name=\"pm1\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style /></style:page-layout><style:page-layout style:name=\"pm2\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" svg:height=\"14.21pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm6\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm4\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#008000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" svg:height=\"15.84pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm5\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ee82ee\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" svg:height=\"14.21pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm13\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties></style:page-layout>";
            }
            else
            {
                content =
					"<style:style style:name=\"PageHeaderFooter0\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ff0000\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ffff00\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter4\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter5\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter12\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter13\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter14\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ff0000\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter15\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter16\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ff0000\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter17\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter18\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter19\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter20\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ff0000\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter21\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:color=\"#ffff00\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter22\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter23\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"MP1\" style:family=\"paragraph\" style:parent-style-name=\"Frame_20_contents\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:paragraph-properties fo:text-align=\"start\" style:justify-single-word=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Charis SIL\" style:font-name-asian=\"Charis SIL\" style:font-name-complex=\"Charis SIL\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"Mfr1\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"left\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" draw:textarea-vertical-align=\"middle\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:style style:name=\"Mfr3\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"center\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:page-layout style:name=\"pm1\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style /></style:page-layout><style:page-layout style:name=\"pm2\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" svg:height=\"14.21pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm6\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm4\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#008000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" svg:height=\"15.84pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm5\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ee82ee\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" svg:height=\"14.21pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm13\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties></style:page-layout>";
            }
            bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "PageTest failed");

            xpath = "//office:master-styles";
            _validate = new ValidateXMLFile(styleOutput);
            if (_isLinux)
            {
                content =
                    "<style:master-page style:name=\"Standard\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Cover_20_Page\" style:display-name=\"Cover Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Title_20_Page\" style:display-name=\"Title Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"TableofContents_20_Page\" style:display-name=\"TableofContents Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:page-number style:num-format=\"i\" text:select-page=\"current\">xv</text:page-number></text:p></style:footer></style:master-page><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"First_20_Page\" style:display-name=\"First Page\" style:next-style-name=\"XHTML\" style:page-layout-name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"XHTML\" style:page-layout-name=\"pm2\" style:next-style-name=\"XHTML\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" />";
            }
            else
            {
                content =
                    "<style:master-page style:name=\"Standard\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Cover_20_Page\" style:display-name=\"Cover Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Title_20_Page\" style:display-name=\"Title Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"TableofContents_20_Page\" style:display-name=\"TableofContents Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:page-number style:num-format=\"i\" text:select-page=\"current\">xv</text:page-number></text:p></style:footer></style:master-page><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"First_20_Page\" style:display-name=\"First Page\" style:next-style-name=\"Left_20_Page\" style:page-layout-name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:header><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:footer></style:master-page><style:master-page style:name=\"Left_20_Page\" style:display-name=\"Left Page\" style:page-layout-name=\"pm4\" style:next-style-name=\"Right_20_Page\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:header><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:footer></style:master-page><style:master-page style:name=\"Right_20_Page\" style:display-name=\"Right Page\" style:page-layout-name=\"pm5\" style:next-style-name=\"Left_20_Page\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:header><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><style:header><text:p text:style-name=\"Header\"><text:span text:style-name=\"MT1\" /></text:p></style:header><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:footer></style:master-page>";
            }
            returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "PageTest-master failed");
        }

        ///<summary>
        /// PageTest9
        /// </summary>
        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void PageTest9()
        {
            const string file = "PageTest9";
            _projInfo.ProjectInputType = "Dictionary";
            string styleOutput = GetStyleOutput(file);

            string xpath = "//office:automatic-styles";
            _validate = new ValidateXMLFile(styleOutput);
			string content = "<style:style style:name=\"PageHeaderFooter0\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"\" style:writing-mode=\"lr-tb\" fo:font-family=\"Verdana\" style:font-name-complex=\"Verdana\" fo:font-weight=\"700\" fo:font-size=\"12pt\" fo:margin-top=\"100%\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"\" fo:margin-top=\"100%\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"\" style:writing-mode=\"lr-tb\" fo:font-family=\"Verdana\" style:font-name-complex=\"Verdana\" fo:font-weight=\"700\" fo:font-size=\"12pt\" fo:margin-top=\"100%\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"\" /></style:style><style:style style:name=\"PageHeaderFooter4\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"\" /></style:style><style:style style:name=\"PageHeaderFooter5\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"\" /></style:style><style:style style:name=\"PageHeaderFooter12\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"string(guidewordfirst)\" style:writing-mode=\"lr-tb\" fo:font-family=\"Charis SIL\" style:font-name-complex=\"Charis SIL\" fo:font-weight=\"700\" fo:font-size=\"12pt\" fo:margin-top=\"28.34646pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter13\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:margin-top=\"28.34646pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter14\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"string(guidewordlast)\" style:writing-mode=\"lr-tb\" fo:font-family=\"Charis SIL\" style:font-name-complex=\"Charis SIL\" fo:font-weight=\"700\" fo:font-size=\"12pt\" fo:margin-top=\"28.34646pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter15\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"\" /></style:style><style:style style:name=\"PageHeaderFooter16\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"\" /></style:style><style:style style:name=\"PageHeaderFooter17\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"\" /></style:style><style:style style:name=\"PageHeaderFooter18\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"string(guidewordfirst)\" style:writing-mode=\"lr-tb\" fo:font-family=\"Charis SIL\" style:font-name-complex=\"Charis SIL\" fo:font-weight=\"700\" fo:font-size=\"12pt\" fo:margin-top=\"28.34646pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter19\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:margin-top=\"28.34646pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter20\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"string(guidewordlast)\" style:writing-mode=\"lr-tb\" fo:font-family=\"Charis SIL\" style:font-name-complex=\"Charis SIL\" fo:font-weight=\"700\" fo:font-size=\"12pt\" fo:margin-top=\"28.34646pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter21\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"\" /></style:style><style:style style:name=\"PageHeaderFooter22\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"\" /></style:style><style:style style:name=\"PageHeaderFooter23\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"\" /></style:style><style:style style:name=\"MP1\" style:family=\"paragraph\" style:parent-style-name=\"Frame_20_contents\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:paragraph-properties fo:text-align=\"end\" style:justify-single-word=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Charis SIL\" style:font-name-asian=\"Charis SIL\" style:font-name-complex=\"Charis SIL\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"Mfr1\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"right\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" draw:textarea-vertical-align=\"middle\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:style style:name=\"Mfr3\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"center\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:page-layout style:name=\"pm1\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"595.2756pt\" fo:page-height=\"841.8898pt\" fo:margin-top=\"57pt\" fo:margin-right=\"113.3858pt\" fo:margin-bottom=\"57pt\" fo:margin-left=\"113.3858pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style /></style:page-layout><style:page-layout style:name=\"pm2\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"595.2756pt\" fo:page-height=\"841.8898pt\" fo:margin-top=\"57pt\" fo:margin-right=\"113.3858pt\" fo:margin-bottom=\"57pt\" fo:margin-left=\"113.3858pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#0000ff\" fo:page-width=\"595.2756pt\" fo:page-height=\"841.8898pt\" fo:margin-top=\"57pt\" fo:margin-right=\"113.3858pt\" fo:margin-bottom=\"57pt\" fo:margin-left=\"113.3858pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm6\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"595.2756pt\" fo:page-height=\"841.8898pt\" fo:margin-top=\"57pt\" fo:margin-right=\"113.3858pt\" fo:margin-bottom=\"57pt\" fo:margin-left=\"113.3858pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm4\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#008000\" fo:page-width=\"595.2756pt\" fo:page-height=\"841.8898pt\" fo:margin-top=\"57pt\" fo:margin-right=\"113.3858pt\" fo:margin-bottom=\"57pt\" fo:margin-left=\"113.3858pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"15.84pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm5\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ffa500\" fo:page-width=\"595.2756pt\" fo:page-height=\"841.8898pt\" fo:margin-top=\"57pt\" fo:margin-right=\"113.3858pt\" fo:margin-bottom=\"57pt\" fo:margin-left=\"113.3858pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"595.2756pt\" fo:page-height=\"841.8898pt\" fo:margin-top=\"57pt\" fo:margin-right=\"113.3858pt\" fo:margin-bottom=\"57pt\" fo:margin-left=\"113.3858pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm13\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"595.2756pt\" fo:page-height=\"841.8898pt\" fo:margin-top=\"57pt\" fo:margin-right=\"113.3858pt\" fo:margin-bottom=\"57pt\" fo:margin-left=\"113.3858pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#0000ff\" fo:page-width=\"595.2756pt\" fo:page-height=\"841.8898pt\" fo:margin-top=\"57pt\" fo:margin-right=\"113.3858pt\" fo:margin-bottom=\"57pt\" fo:margin-left=\"113.3858pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties></style:page-layout>";
	        if (Common.UsingMonoVM)
				content = "<style:style style:name=\"PageHeaderFooter0\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"\" style:writing-mode=\"lr-tb\" fo:font-family=\"Verdana\" style:font-name-complex=\"Verdana\" fo:font-weight=\"700\" fo:font-size=\"12pt\" fo:margin-top=\"100%\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"\" fo:margin-top=\"100%\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"\" style:writing-mode=\"lr-tb\" fo:font-family=\"Verdana\" style:font-name-complex=\"Verdana\" fo:font-weight=\"700\" fo:font-size=\"12pt\" fo:margin-top=\"100%\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"\" /></style:style><style:style style:name=\"PageHeaderFooter4\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"\" /></style:style><style:style style:name=\"PageHeaderFooter5\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"\" /></style:style><style:style style:name=\"PageHeaderFooter12\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"string(guidewordfirst)\" style:writing-mode=\"lr-tb\" fo:font-family=\"Charis SIL\" style:font-name-complex=\"Charis SIL\" fo:font-weight=\"700\" fo:font-size=\"12pt\" fo:margin-top=\"28.34646pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter13\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:margin-top=\"28.34646pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter14\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"string(guidewordlast)\" style:writing-mode=\"lr-tb\" fo:font-family=\"Charis SIL\" style:font-name-complex=\"Charis SIL\" fo:font-weight=\"700\" fo:font-size=\"12pt\" fo:margin-top=\"28.34646pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter15\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"\" /></style:style><style:style style:name=\"PageHeaderFooter16\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"\" /></style:style><style:style style:name=\"PageHeaderFooter17\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"\" /></style:style><style:style style:name=\"PageHeaderFooter18\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"string(guidewordfirst)\" style:writing-mode=\"lr-tb\" fo:font-family=\"Charis SIL\" style:font-name-complex=\"Charis SIL\" fo:font-weight=\"700\" fo:font-size=\"12pt\" fo:margin-top=\"28.34646pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter19\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"|counter(page)|\" fo:margin-top=\"28.34646pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter20\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"string(guidewordlast)\" style:writing-mode=\"lr-tb\" fo:font-family=\"Charis SIL\" style:font-name-complex=\"Charis SIL\" fo:font-weight=\"700\" fo:font-size=\"12pt\" fo:margin-top=\"28.34646pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter21\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"\" /></style:style><style:style style:name=\"PageHeaderFooter22\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"\" /></style:style><style:style style:name=\"PageHeaderFooter23\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"\" /></style:style><style:style style:name=\"MP1\" style:family=\"paragraph\" style:parent-style-name=\"Frame_20_contents\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:paragraph-properties fo:text-align=\"end\" style:justify-single-word=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Charis SIL\" style:font-name-asian=\"Charis SIL\" style:font-name-complex=\"Charis SIL\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"Mfr1\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"right\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" draw:textarea-vertical-align=\"middle\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:style style:name=\"Mfr3\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"center\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:page-layout style:name=\"pm1\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"595.2756pt\" fo:page-height=\"841.8898pt\" fo:margin-top=\"57pt\" fo:margin-right=\"113.3858pt\" fo:margin-bottom=\"57pt\" fo:margin-left=\"113.3858pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style /></style:page-layout><style:page-layout style:name=\"pm2\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"595.2756pt\" fo:page-height=\"841.8898pt\" fo:margin-top=\"57pt\" fo:margin-right=\"113.3858pt\" fo:margin-bottom=\"57pt\" fo:margin-left=\"113.3858pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#0000ff\" fo:page-width=\"595.2756pt\" fo:page-height=\"841.8898pt\" fo:margin-top=\"57pt\" fo:margin-right=\"113.3858pt\" fo:margin-bottom=\"57pt\" fo:margin-left=\"113.3858pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm6\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"595.2756pt\" fo:page-height=\"841.8898pt\" fo:margin-top=\"57pt\" fo:margin-right=\"113.3858pt\" fo:margin-bottom=\"57pt\" fo:margin-left=\"113.3858pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm4\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#008000\" fo:page-width=\"595.2756pt\" fo:page-height=\"841.8898pt\" fo:margin-top=\"57pt\" fo:margin-right=\"113.3858pt\" fo:margin-bottom=\"57pt\" fo:margin-left=\"113.3858pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"15.84pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm5\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ffa500\" fo:page-width=\"595.2756pt\" fo:page-height=\"841.8898pt\" fo:margin-top=\"57pt\" fo:margin-right=\"113.3858pt\" fo:margin-bottom=\"57pt\" fo:margin-left=\"113.3858pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"595.2756pt\" fo:page-height=\"841.8898pt\" fo:margin-top=\"57pt\" fo:margin-right=\"113.3858pt\" fo:margin-bottom=\"57pt\" fo:margin-left=\"113.3858pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm13\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"595.2756pt\" fo:page-height=\"841.8898pt\" fo:margin-top=\"57pt\" fo:margin-right=\"113.3858pt\" fo:margin-bottom=\"57pt\" fo:margin-left=\"113.3858pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#0000ff\" fo:page-width=\"595.2756pt\" fo:page-height=\"841.8898pt\" fo:margin-top=\"57pt\" fo:margin-right=\"113.3858pt\" fo:margin-bottom=\"57pt\" fo:margin-left=\"113.3858pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties></style:page-layout>";

			bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);

            Assert.IsTrue(returnValue1, "PageTest failed");

            xpath = "//office:master-styles";
            _validate = new ValidateXMLFile(styleOutput);
            content = "<style:master-page style:name=\"Standard\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Cover_20_Page\" style:display-name=\"Cover Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Title_20_Page\" style:display-name=\"Title Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"TableofContents_20_Page\" style:display-name=\"TableofContents Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:page-number style:num-format=\"i\" text:select-page=\"current\">xv</text:page-number></text:p></style:footer></style:master-page><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"First_20_Page\" style:display-name=\"First Page\" style:next-style-name=\"Left_20_Page\" style:page-layout-name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Left_20_Page\" style:display-name=\"Left Page\" style:page-layout-name=\"pm4\" style:next-style-name=\"Right_20_Page\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Left_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RLeft_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span></text:p></style:header><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><draw:frame draw:style-name=\"Mfr1\" draw:name=\"Frame1\" text:anchor-type=\"paragraph\" svg:y=\"56.25pt\" fo:min-width=\"100pt\" draw:z-index=\"1\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\"><draw:text-box fo:min-height=\"19.00pt\"><text:p text:style-name=\"MP1\"><text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Right_Guideword_R\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RRight_Guideword_R\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span></text:p></draw:text-box></draw:frame></text:p></style:footer></style:master-page><style:master-page style:name=\"Right_20_Page\" style:display-name=\"Right Page\" style:page-layout-name=\"pm5\" style:next-style-name=\"Left_20_Page\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header><text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Left_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RLeft_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:tab /><text:span text:style-name=\"MT3\"><text:page-number text:select-page=\"current\">4</text:page-number></text:span><text:tab /><text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Left_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RLeft_Guideword_L\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span></text:p></style:header><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><draw:frame draw:style-name=\"Mfr1\" draw:name=\"Frame1\" text:anchor-type=\"paragraph\" svg:y=\"56.25pt\" fo:min-width=\"100pt\" draw:z-index=\"1\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\"><draw:text-box fo:min-height=\"19.00pt\"><text:p text:style-name=\"MP1\"><text:span text:style-name=\"MT1\"><text:variable-get text:name=\"Right_Guideword_R\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"MT2\"><text:variable-get text:name=\"RRight_Guideword_R\" office:value-type=\"string\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span></text:p></draw:text-box></draw:frame></text:p></style:footer></style:master-page>";

	        if (Common.UsingMonoVM)
		        content = "<style:master-page style:name=\"Standard\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Cover_20_Page\" style:display-name=\"Cover Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Title_20_Page\" style:display-name=\"Title Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"TableofContents_20_Page\" style:display-name=\"TableofContents Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:page-number style:num-format=\"i\" text:select-page=\"current\">xv</text:page-number></text:p></style:footer></style:master-page><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"First_20_Page\" style:display-name=\"First Page\" style:next-style-name=\"XHTML\" style:page-layout-name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"XHTML\" style:page-layout-name=\"pm2\" style:next-style-name=\"XHTML\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" />";

            returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "PageTest-master failed");

        }
		///<summary>
		/// PageTestForTitle
		/// </summary>
		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTeamCity")]
		public void PageTestTitle()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "PageTest";
			_index = 10;
			string styleOutput = GetStyleOutput(file);
			_index = 0; // reset
			string xpath = "//office:automatic-styles";
			_validate = new ValidateXMLFile(styleOutput);
			string content = string.Empty;
			content = "<style:style style:name=\"PageHeaderFooter0\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter4\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter5\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter12\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter13\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"IBAN -- ENGLISH\" style:writing-mode=\"lr-tb\" fo:font-family=\"Charis SIL AmArea\" style:font-name-complex=\"Charis SIL AmArea\" fo:font-weight=\"400\" fo:font-size=\"11pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter14\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter15\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter16\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter17\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter18\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter19\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter20\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter21\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter22\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter23\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"MP1\" style:family=\"paragraph\" style:parent-style-name=\"Frame_20_contents\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:paragraph-properties fo:text-align=\"start\" style:justify-single-word=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Charis SIL\" style:font-name-asian=\"Charis SIL\" style:font-name-complex=\"Charis SIL\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"Mfr1\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"left\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" draw:textarea-vertical-align=\"middle\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:style style:name=\"Mfr3\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"center\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:page-layout style:name=\"pm1\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style /></style:page-layout><style:page-layout style:name=\"pm2\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#0000ff\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm6\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm4\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#008000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"15.84pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm5\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm13\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#0000ff\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties></style:page-layout>";

			if (Common.UsingMonoVM)
				content = "<style:style style:name=\"PageHeaderFooter0\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter4\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter5\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter12\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties fo:font-weight=\"700\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter13\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties content=\"IBAN -- ENGLISH\" style:writing-mode=\"lr-tb\" fo:font-family=\"Charis SIL AmArea\" style:font-name-complex=\"Charis SIL AmArea\" fo:font-weight=\"400\" fo:font-size=\"11pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"PageHeaderFooter14\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter15\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter16\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter17\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter18\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter19\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter20\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter21\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter22\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"PageHeaderFooter23\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties /></style:style><style:style style:name=\"MP1\" style:family=\"paragraph\" style:parent-style-name=\"Frame_20_contents\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:paragraph-properties fo:text-align=\"start\" style:justify-single-word=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT1\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT2\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Times New Roman\" style:font-name-asian=\"Times New Roman\" style:font-name-complex=\"Times New Roman\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"MT3\" style:family=\"text\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:text-properties style:font-name=\"Charis SIL\" style:font-name-asian=\"Charis SIL\" style:font-name-complex=\"Charis SIL\" fo:font-size=\"12pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:style><style:style style:name=\"Mfr1\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"left\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" draw:textarea-vertical-align=\"middle\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:style style:name=\"Mfr3\" style:family=\"graphic\" style:parent-style-name=\"Frame\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:graphic-properties style:vertical-pos=\"from-top\" style:vertical-rel=\"page\" style:horizontal-pos=\"center\" style:horizontal-rel=\"paragraph\" fo:background-color=\"transparent\" style:background-transparency=\"100%\" fo:padding=\"0in\" fo:border=\"none\" style:shadow=\"none\" style:flow-with-text=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:columns fo:column-count=\"1\" fo:column-gap=\"0in\" /></style:graphic-properties></style:style><style:page-layout style:name=\"pm1\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style /></style:page-layout><style:page-layout style:name=\"pm2\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#0000ff\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm6\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style /><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm4\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#008000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"15.84pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm5\" style:page-usage=\"mirrored\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties><style:header-style><style:header-footer-properties fo:margin-bottom=\"14.21pt\" fo:min-height=\"28.42pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style><style:header-footer-properties fo:margin-bottom=\"0.0pt\" fo:min-height=\"0.0pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style></style:page-layout><style:page-layout style:name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm13\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#ff0000\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:page-layout><style:page-layout style:name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:page-layout-properties fo:background-color=\"#0000ff\" fo:page-width=\"419.5276pt\" fo:page-height=\"595.2756pt\" fo:margin-top=\"85.03937pt\" fo:margin-right=\"85.03937pt\" fo:margin-bottom=\"85.03937pt\" fo:margin-left=\"85.03937pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><style:background-image /><style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" /></style:page-layout-properties></style:page-layout>";

			bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
			Assert.IsTrue(returnValue1, "PageTest failed");

			xpath = "//office:master-styles";
			_validate = new ValidateXMLFile(styleOutput);
			content = "<style:master-page style:name=\"Standard\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Cover_20_Page\" style:display-name=\"Cover Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm1\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"Title_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Title_20_Page\" style:display-name=\"Title Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"TableofContents_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"TableofContents_20_Page\" style:display-name=\"TableofContents Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm7\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:footer><text:p text:style-name=\"Footer\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:tab /><text:page-number style:num-format=\"i\" text:select-page=\"current\">xv</text:page-number></text:p></style:footer></style:master-page><style:master-page style:name=\"Dummy_20_Page\" style:display-name=\"Dummy Page\" style:next-style-name=\"First_20_Page\" style:page-layout-name=\"pm12\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"First_20_Page\" style:display-name=\"First Page\" style:next-style-name=\"Left_20_Page\" style:page-layout-name=\"pm3\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" /><style:master-page style:name=\"Left_20_Page\" style:display-name=\"Left Page\" style:page-layout-name=\"pm4\" style:next-style-name=\"Right_20_Page\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><text:tab xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\" /><text:span text:style-name=\"MT1\" style:horizontal-pos=\"center\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">IBAN -- ENGLISH</text:span></style:master-page><style:master-page style:name=\"Right_20_Page\" style:display-name=\"Right Page\" style:page-layout-name=\"pm5\" style:next-style-name=\"Left_20_Page\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" />";
			returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
			Assert.IsTrue(returnValue1, "PageTest-master failed");
		}
        ///<summary>
        /// DictionaryT9Test
        /// </summary>
        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void DictionaryT9Test()
        {
            const string file = "t9";
            _projInfo.ProjectInputType = "Dictionary";
            GetStyleOutput(file);

            //Content Test - First
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = string.Empty;
            string xpath = "//draw:frame";
            //_validate.GetInnerText = true;
            _validate.GetOuterXml = true;
			string content = "<draw:frame draw:style-name=\"fr2\" draw:name=\"Frame2\" text:anchor-type=\"paragraph\" draw:z-index=\"1\" svg:height=\"72pt\" xmlns:svg=\"urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\"><draw:text-box fo:min-height=\"1in\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\"><draw:frame draw:style-name=\"gr2\" draw:name=\"Graphics2\" text:anchor-type=\"paragraph\" svg:height=\"72pt\"><draw:image xlink:type=\"simple\" xlink:show=\"embed\" xlink:actuate=\"onLoad\" xlink:href=\"Pictures/nowaitress.jpg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" /><svg:title>nowaitress.jpg</svg:title></draw:frame><text:p text:style-name=\"caption_image_firstoftype_sense_article_sectionletter_dictionary\">a waitress</text:p></draw:text-box></draw:frame>";
            bool returnValue = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue);
        }
        ///<summary>
        /// TD96 text-indent
        /// </summary>
        [Test]
        public void MasterDocument()
        {
            ExportLibreOffice exportOpenOffice = new ExportLibreOffice();
            var projInfo = new PublicationInformation();

            CopyInputToOutput();

            string lexiconFull = FileOutput("main.xhtml");
            string revFull = FileOutput("FlexRev.xhtml");
            string lexiconCSS = FileOutput("main.css");
            string revCSS = FileOutput("FLExRev.css");

            ProgressBar pb = new ProgressBar();
            projInfo.ProgressBar = pb;
            //projInfo.ProjectFileWithPath = _projectFile;
            projInfo.IsLexiconSectionExist = File.Exists(lexiconFull);
            projInfo.IsReversalExist = File.Exists(revFull);
            //projInfo.IsReversalExist = Param.Value[Param.ReversalIndex] == "True";
            projInfo.SwapHeadword = false;
            projInfo.FromPlugin = true;
            projInfo.DefaultCssFileWithPath = lexiconCSS;
            projInfo.DefaultRevCssFileWithPath = revCSS;
            projInfo.DefaultXhtmlFileWithPath = lexiconFull;
            projInfo.ProjectInputType = "Dictionary";
            projInfo.DictionaryPath = Path.GetDirectoryName(lexiconFull);
            projInfo.ProjectName = Path.GetFileNameWithoutExtension(lexiconFull);

            exportOpenOffice.Export(projInfo);
            Assert.IsTrue(CheckFileExist(), "Master Document Failed.");

        }

        [Test]
        public void SuperVerticalAlign()
        {
            const string file = "SuperVerticalAlign";
            _projInfo.ProjectInputType = "Dictionary";
            string styleOutput = GetStyleOutput(file);

            _validate = new ValidateXMLFile(styleOutput);
            _validate.ClassName = "b_div_a_body";
            _validate.ClassProperty.Add("fo:font-size", "15");
            _validate.ClassProperty.Add("style:font-size-complex", "15");
            _validate.ClassProperty.Add("style:text-position", "super 55%");
            bool returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            _validate.ClassName = "c_div_a_body";
            _validate.ClassProperty.Add("fo:font-size", "12pt");
            _validate.ClassProperty.Add("style:font-size-complex", "12pt");
            _validate.ClassProperty.Add("style:text-position", "super 55%");
            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            _validate.ClassName = "d_div_a_body";
            _validate.ClassProperty.Add("fo:font-size", "20");
            _validate.ClassProperty.Add("style:font-size-complex", "20");
            _validate.ClassProperty.Add("style:text-position", "sub 55%");
            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            _validate.ClassName = "e_div_a_body";
            _validate.ClassProperty.Add("fo:font-size", "40");
            _validate.ClassProperty.Add("style:font-size-complex", "40");
            _validate.ClassProperty.Add("style:text-position", "super 55%");
            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);
        }

        ///TD-1857 - Footnote ruler is short
        [Test]
        public void FootnoteSeperator_Node()
        {
            const string file = "FootnoteSeperator";
            _projInfo.ProjectInputType = "Scripture";
            string styleOutput = GetStyleOutput(file);

            _validate = new ValidateXMLFile(styleOutput);

            string content = string.Empty;



            string xpath = "//office:automatic-styles/style:page-layout[@style:name='pm1']/style:page-layout-properties/style:footnote-sep";

            bool result = Common.UnixVersionCheck();

            if (result)  // from the file access
            {
                content = "<style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" />";
            }
            else
            {
                content = "<style:footnote-sep style:width=\"0.0071in\" style:line-style=\"solid\" style:distance-before-sep=\"30%\" style:distance-after-sep=\"30%\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" />";
            }
            _validate.GetOuterXml = true;
            bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "FootnoteSeperator test failed");

            if (result)  // from the file access
            {
                xpath = "//office:automatic-styles/style:page-layout[@style:name='pm6']/style:page-layout-properties/style:footnote-sep";
                content = "<style:footnote-sep style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" />";
            }
            else
            {
                xpath = "//office:automatic-styles/style:page-layout[@style:name='pm5']/style:page-layout-properties/style:footnote-sep";
                content = "<style:footnote-sep style:width=\"0.0071in\" style:line-style=\"solid\" style:distance-before-sep=\"30%\" style:distance-after-sep=\"30%\" style:color=\"#000000\" style:adjustment=\"centre\" style:rel-width=\"100%\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\" />";
            }
            _validate.GetOuterXml = true;
            returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "FootnoteSeperator test failed");

        }

        ///TD-1941
        [Test]
        public void ReferenceMarkNode()
        {
            const string file = "ReferenceMark";
            _projInfo.ProjectInputType = "Scripture";
            GetStyleOutput(file);

            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);

			string xpath = "//text:span[@text:style-name='scrFootnoteMarker_Paragraph_scrSection_columns_scrBook_scrBody']/text:a[@xlink:href='#fbcf3087e-dcad-43bc-ba61-1ca2f4ffbb63']";
            string content = "Reference here";
            bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "Reference with text test failed");

			xpath = "//text:span[@text:style-name='AlternateReading_.nko_Test3_Paragraph_scrSection_columns_scrBook_scrBody']";
            content = "Child";
            returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "Reference without text test failed");

        }

        ///TD-1944
        [Test]
        public void FootnoteVerseNumberNode1()
        {
            const string file = "FootnoteVerseNumber";
            _projInfo.ProjectInputType = "Scripture";
            GetStyleOutput(file);

            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.GetInnerText = true;
            string xpath = "//text:note[@text:id='ftn1']/text:note-body/text:p[@text:style-name='NoteGeneralParagraph']";
            string content = "21:1-2 = Nf-nyb igyi obubw Olifb.";
            bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "First footnote versenumber test failed");

            xpath = "//text:note[@text:id='ftn2']/text:note-body/text:p[@text:style-name='NoteGeneralParagraph']";
            content = "21:44 = You can use the add spaces button .";
            returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "Second footnote versenumber test failed");

        }

        ///TD-3143
        [Test]
        public void FootnoteVerseNumberNode2()
        {
            const string file = "FootnoteVerse2";
            _projInfo.ProjectInputType = "Scripture";
            GetStyleOutput(file);

            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.GetOuterXml = true;
            string xpath = "//text:note[@text:id='ftn1']/text:note-body";
            string content = "<text:note-body xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:p text:style-name=\"NoteCrossHYPHENReferenceParagraph\"><text:span text:style-name=\"NoteCrossHYPHENReferenceParagraph..footnote-marker\">1.19 </text:span><text:span text:style-name=\"span_.zxx_NoteCrossHYPHENReferenceParagraph_Paragraph_scrSection_columns_scrBook_scrBody\">Dan. 8:16, 9:21</text:span></text:p></text:note-body>";
            bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "Footnote versenumber special case '1.19-1' test failed");

            xpath = "//text:note[@text:id='ftn2']/text:note-body";
            content = "<text:note-body xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:p text:style-name=\"NoteGeneralParagraph\"><text:span text:style-name=\"span_.zxx_NoteGeneralParagraph_Paragraph_scrSection_columns_scrBook_scrBody\">Bahasa Yunani bilang \"Badiri di Allah pung muka\". Ini bisa pung arti \"karja par Tuhan\". Mar bisa pung arti lai \u2018Badiri di Allah pung muka\u2019. Malekat yang badiri di Allah pung muka pung kuasa labe dari malekat laeng. Jadi, Gabriel bukang malekat biasa.</text:span></text:p></text:note-body>";
            returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "Footnote versenumber special case '1.19-2' test failed");

            xpath = "//text:note[@text:id='ftn3']/text:note-body";
            content = "<text:note-body xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:p text:style-name=\"NoteCrossHYPHENReferenceParagraph\"><text:span text:style-name=\"NoteCrossHYPHENReferenceParagraph..footnote-marker\">1.27 </text:span><text:span text:style-name=\"span_.zxx_NoteCrossHYPHENReferenceParagraph_Paragraph_scrSection_columns_scrBook_scrBody\">Mat. 1:18</text:span></text:p></text:note-body>";
            returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "Footnote versenumber special case '1.27' test failed");

            xpath = "//text:note[@text:id='ftn4']/text:note-body";
            content = "<text:note-body xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:p text:style-name=\"NoteCrossHYPHENReferenceParagraph\"><text:span text:style-name=\"NoteCrossHYPHENReferenceParagraph..footnote-marker\">1.32-33 </text:span><text:span text:style-name=\"span_.zxx_NoteCrossHYPHENReferenceParagraph_Paragraph_scrSection_columns_scrBook_scrBody\">2Sam. 7:12, 13, 16; Yes. 9:6</text:span></text:p></text:note-body>";
            returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "Footnote versenumber special case '1.32-33' test failed");

            xpath = "//text:note[@text:id='ftn5']/text:note-body";
            content = "<text:note-body xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:p text:style-name=\"NoteCrossHYPHENReferenceParagraph\"><text:span text:style-name=\"NoteCrossHYPHENReferenceParagraph..footnote-marker\">2.41 </text:span><text:span text:style-name=\"span_.zxx_NoteCrossHYPHENReferenceParagraph_Paragraph_scrSection_columns_scrBook_scrBody\">Kel. 12:1-27; Ul. 16:1-8</text:span></text:p></text:note-body>";
            returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "Footnote versenumber special case '2.41-1' test failed");

            xpath = "//text:note[@text:id='ftn6']/text:note-body";
            content = "<text:note-body xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:p text:style-name=\"NoteGeneralParagraph\"><text:span text:style-name=\"NoteGeneralParagraph..footnote-marker\">2.41 </text:span><text:span text:style-name=\"span_.zxx_NoteGeneralParagraph_Paragraph_scrSection_columns_scrBook_scrBody\">Hari basar Paska Yahudi tu, orang Yahudi inga waktu dong pung tete nene moyang kaluar dari negara Mesir. Dolo dong jadi orang suru-suru di tampa tu, mar Allah kasi kaluar dong la bawa dong ka tana yang Antua su janji par dong.</text:span></text:p></text:note-body>";
            returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "Footnote versenumber special case '2.41-2' test failed");

        }

        ///TD-2497
        [Test]
        public void HardSpaceAfterVerseNumberNode()
        {
            const string file = "HardSpaceAfterVerseNumber";
            _projInfo.ProjectInputType = "Scripture";
            GetStyleOutput(file);

            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.GetInnerText = true;
			string xpath = "//text:p/text:span[@text:style-name='span_.zxx_Paragraph_scrSection_scrBody']";
            string content = "1\u00A0Sejay i tonton Jesu Criston edafod David tan si Abraham.";
            bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "Hard Space after versenumber test failed");
        }

        ///TD-2666
        [Test]
        public void NoImageNode()
        {
            const string file = "NoImage";
            _projInfo.ProjectInputType = "Dictionary";
           GetStyleOutput(file);

            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.GetInnerText = true;
            string xpath = "//text:p[@text:style-name='entry_letData_dicBody'][2]";
            string content = "Applepro A FruitFruity";
            bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "Hard Space after versenumber test failed");
        }

        ///TD-2700
        [Test]
        public void HomographSpaceNode()
        {
            const string file = "HomographSpace";
            _projInfo.ProjectInputType = "Dictionary";
            GetStyleOutput(file);
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.GetInnerText = true;
            string xpath = "//text:p[@text:style-name='letData_dicBody']";
            string content = "slllep";
            bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "Hard Space after versenumber test failed");
        }

        ///TD-2739
        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void UserIndicFont()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "UserIndicFont";
            string styleOutput = GetStyleOutput(file);

            //Note - The Styles will be created while processing xhtml(content.xml)
            //Style Test - Second
            _validate = new ValidateXMLFile(styleOutput);

            _validate.ClassName = "headword_.te_entry_letData_dicBody";
	        if (Common.UsingMonoVM)
	        {
		        _validate.ClassProperty.Add("fo:font-family", "Times New Roman");
		        _validate.ClassProperty.Add("style:font-name-complex", "Times New Roman");
	        }
	        else
	        {
		        _validate.ClassProperty.Add("fo:font-family", "Arial Unicode MS");
		        _validate.ClassProperty.Add("style:font-name-complex", "Arial Unicode MS");
	        }
	        _validate.ClassProperty.Add("fo:font-size", "10pt");

            bool returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);
        }

        ///TD-2717
        [Test]
        public void SpaceIssueOnSenseNode()
        {
            const string file = "SpaceIssueOnSense";
            _projInfo.ProjectInputType = "Dictionary";
            GetStyleOutput(file);
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.GetInnerText = true;
            string xpath = "//text:p[@text:style-name='entry_letData_dicBody'][1]";
            string content = "bel n  14. 1) war. 2) fighting. 3) bloodshed. 4) spectacle (something which pulls crowds). You can use the add spaces button to separate... because of the trouble which began in the city...";
            bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "Hard Space after versenumber test failed");

            xpath = "//text:p[@text:style-name='entry_letData_dicBody'][2]";
            content = "be1 pro. they, them, it, pronoun for nouns of classes 2,8,14 and 19.";
            returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "Hard Space after versenumber test failed");

            xpath = "//text:p[@text:style-name='entry_letData_dicBody'][3]";
            content = "be2 1) conj. and. 2) with. the trouble. He and the child went. / He went with the child.";
            returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "Hard Space after versenumber test failed");
        }

        ///<summary>
        /// prince-text-replace
        /// </summary>
        [Test]
        public void TextReplaceTest_Node()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "TextReplace";
            GetStyleOutput(file);

            //Content Test
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
			_validate.ClassName = "span_.bgt_NoteGeneralParagraph_scrSection_scrBook_scrBody";
            string content = "Kori haghore Grik " + Common.ConvertUnicodeToString("\\201C") + "Oti veikisighi." + Common.ConvertUnicodeToString("\\201D") + " Na puhi kena eia mara i hau bali tateli aua nidia na dotho, imarea kena kisia na bakodia ara kuladia kiloau.";
            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "span");
            Assert.IsTrue(returnValue1);
        }

        ///<summary>
        /// TD-2908
        /// </summary>
        [Test]
        public void KeepLineSeperator()
        {
            const string file = "KeepLineSeperator";
            _projInfo.ProjectInputType = "Dictionary";
            GetStyleOutput(file);

            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
			string xpath = "//text:span[@text:style-name='span_.kup_example_.kup_examples_sense_senses_entry_letData_dicBody']";
            _validate.ClassName = string.Empty;
            string content = "<text:line-break xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\" />Ne aban horip emahan barezat soh.";
            bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "KeepLine Seperator test failed");

			xpath = "//text:span[@text:style-name='span_.tpi_translation_.en_translations_examples_sense_senses_entry_letData_dicBody']";
            _validate.ClassName = string.Empty;
            content = "<text:line-break xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\" />When the bad man came I ran away.";
            returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "KeepLine Seperator test failed");
        }


        ///<summary>
        /// TD-3539 - Avoid conversion process for image from "TIF" to "JPG"
        /// </summary>
        [Test]
        public void KeepImageExtension()
        {
            const string file = "KeepImageExtension";
            _projInfo.ProjectInputType = "Dictionary";
            GetStyleOutput(file);

            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            string xpath = "//draw:frame[@draw:style-name='gr2']/draw:image";
            _validate.ClassName = string.Empty;
            _validate.GetOuterXml = true;
            string content = "<draw:image xlink:type=\"simple\" xlink:show=\"embed\" xlink:actuate=\"onLoad\" xlink:href=\"Pictures/kitchen cooking.tif\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xmlns:draw=\"urn:oasis:names:tc:opendocument:xmlns:drawing:1.0\" />";
            bool returnValue1 = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue1, "Keep Image Extension test failed");
        }

		/// <summary>
		/// Test for the SpanTagCorrection Function
		/// TD-4717 - Default Scripture Export Crashes
		/// </summary>
		[Test]
	    public void SpanTagCorrectionTest()
		{
			string contentInput1 = "<text:span text:style-name=\"NoteGeneralParagraph..footnote-marker\">1.1 </text:span><text:span text:style-name=\"QuotedText_.zxx_NoteGeneralParagraph_Line2_scrSection_columns_scrBook_scrBody\">the heavens and the earth</text:span><text:span text:style-name=\"span_.zxx_NoteGeneralParagraph_Line2_scrSection_columns_scrBook_scrBody\">: “The heavens and the earth” stood for the universe.</text:span>";
			string contentExpected1 = "<text:span text:style-name=\"NoteGeneralParagraph..footnote-marker\">1.1 </text:span><text:span text:style-name=\"QuotedText_.zxx_NoteGeneralParagraph_Line2_scrSection_columns_scrBook_scrBody\">the heavens and the earth</text:span><text:span text:style-name=\"span_.zxx_NoteGeneralParagraph_Line2_scrSection_columns_scrBook_scrBody\">: “The heavens and the earth” stood for the universe.</text:span>";
			string contentOutput1 = LOContent.SpanTagCorrection(contentInput1);
			Assert.AreEqual(contentExpected1, contentOutput1);

			string contentInput2 = "<text:span text:style-name=\"NoteGeneralParagraph..footnote-marker\"><text:span text:style-name=\"NoteGeneralParagraph..footnote-marker\"><text:span text:style-name=\"NoteCrossHYPHENReferenceParagraph..footnote-marker\">1.3: </text:span><text:span text:style-name=\"span_.zxx_NoteCrossHYPHENReferenceParagraph_Paragraph_scrSection_columns_scrBook_scrBody\">2 Cor 4.6.</text:span>";
			string contentExpected2 = "<text:span text:style-name=\"NoteGeneralParagraph..footnote-marker\"><text:span text:style-name=\"NoteGeneralParagraph..footnote-marker\"><text:span text:style-name=\"NoteCrossHYPHENReferenceParagraph..footnote-marker\">1.3: </text:span><text:span text:style-name=\"span_.zxx_NoteCrossHYPHENReferenceParagraph_Paragraph_scrSection_columns_scrBook_scrBody\">2 Cor 4.6.</text:span></text:span></text:span>";
			string contentOutput2 = LOContent.SpanTagCorrection(contentInput2);
			Assert.AreEqual(contentExpected2, contentOutput2);
		}

        #region Indesign TestCases
        ///<summary>
        /// Indesign PseudoBefore
        /// </summary>
        [Test]
        public void PseudoBefore_Node()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "PseudoBefore";
            GetStyleOutput(file);

            //Content Test - First
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
			_validate.ClassName = "a_b_letHead_dicBody";
            string content = Common.ConvertUnicodeToString("\\2666") + "let Head";
            bool returnValue1 = _validate.ValidateOfficeTextNodeList(1, content, "para");
            Assert.IsTrue(returnValue1);

			_validate.ClassName = "a_b_letData_dicBody";
            content = ": let Data";
            returnValue1 = _validate.ValidateOfficeTextNodeList(1, content, "para");
            Assert.IsTrue(returnValue1);
        }

        ///<summary>
        /// Indesign Parent1
        /// </summary>
        [Test]
        public void Parent1_Node()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "Parent1";
            string styleOutput = GetStyleOutput(file);

            //Content Test - First
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
			_validate.ClassName = "xitem_.en_main_body";
            string content = "sample text file";
            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1);

            //Note - The Styles will be created while processing xhtml(content.xml)
            //Style Test - Second
            _validate = new ValidateXMLFile(styleOutput);
			_validate.ClassName = "xitem_.en_main_body";
            _validate.ClassProperty.Add("fo:color", "#ff0000");

            bool returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);
        }

        ///<summary>
        /// Indesign Precede1
        /// </summary>
        [Test]
        public void Precede1_Node()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "Precede1";
            string styleOutput = GetStyleOutput(file);

            //Content Test - First
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
			_validate.ClassName = "xitem_main_body";
            string content = "sample text file";
            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1);

			_validate.ClassName = "xitem_.en_main_body";
            content = "sample text file";
            returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1);

            //Note - The Styles will be created while processing xhtml(content.xml)
            //Style Test - Second
            _validate = new ValidateXMLFile(styleOutput);
			_validate.ClassName = "xitem_main_body";
            _validate.ClassProperty.Add("fo:color", "#0000ff");
            bool returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

			_validate.ClassName = "xitem_.en_main_body";
			_validate.ClassProperty.Add("fo:color", "#ffa500");
            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

        }

        ///<summary>
        /// TD-2617
        /// </summary>
        [Test]
        public void SpaceTest()
        {
            _projInfo.ProjectInputType = "Scripture";
            const string file = "SpaceTest";
            GetStyleOutput(file);

            //Content Test - First
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
			string xpath = "//text:p[@text:style-name='ChapterNumber1']";
			string content = "<text:span text:style-name=\"ChapterNumber_.zxx_Paragraph_scrBook_scrBody\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">1</text:span><text:span text:style-name=\"ChapterNumber_.zxx\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:variable-set text:name=\"Left_Guideword_L\" text:display=\"none\" text:formula=\"ooow: \" office:value-type=\"string\" office:string-value=\"\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"ChapterNumber_.zxx\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:variable-set text:name=\"RLeft_Guideword_L\" text:display=\"none\" text:formula=\"ooow: \" office:value-type=\"string\" office:string-value=\"\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"ChapterNumber_.zxx\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:variable-set text:name=\"Right_Guideword_R\" text:display=\"none\" text:formula=\"ooow: \" office:value-type=\"string\" office:string-value=\"\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span text:style-name=\"ChapterNumber_.zxx\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:variable-set text:name=\"RRight_Guideword_R\" text:display=\"none\" text:formula=\"ooow: \" office:value-type=\"string\" office:string-value=\"\" xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" /></text:span><text:span xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\" /><text:span text:style-name=\"span_.zxx_Paragraph_scrBook_scrBody\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"><text:span text:style-name=\"VerseNumber_.zxx_Paragraph_scrBook_scrBody\">  1 </text:span>Yesu Kristo, Owi</text:span><text:span text:style-name=\"SeeInGlossary_.zxx_Paragraph_scrBook_scrBody\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">Dawid</text:span><text:span text:style-name=\"span_.zxx_Paragraph_scrBook_scrBody\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"> mv na,</text:span><text:span text:style-name=\"SeeInGlossary_.zxx_Paragraph_scrBook_scrBody\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">Abraham</text:span><text:span text:style-name=\"span_.zxx_Paragraph_scrBook_scrBody\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\"> mvat sapace invpt</text:span>";
            bool returnValue = _validate.ValidateNodeInnerXml(xpath, content);
            Assert.IsTrue(returnValue);

        }

        ///<summary>
        /// Indesign PseudoContains
        /// </summary>
        [Test]
        public void PseudoContains_Node()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "PseudoContains";
            GetStyleOutput(file);

            //Content Test - First
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
			_validate.ClassName = "xitem_main_body";
            string content = "contain";
            bool returnValue1 = _validate.ValidateOfficeTextNodeList(1, content, "para");
            Assert.IsTrue(returnValue1);

            content = "XXcontain";
            returnValue1 = _validate.ValidateOfficeTextNodeList(2, content, "para");
            Assert.IsTrue(returnValue1, "Counter1 - Content Failure");


			_validate.ClassName = "xitem_.en_main_body";
            content = "sakple text file lang";
            returnValue1 = _validate.ValidateOfficeTextNodeList(1, content, "para");
            Assert.IsTrue(returnValue1, "Counter1 - Content Failure");
        }

        ///<summary>
        /// Indesign MultiClass
        /// </summary>
        [Test]
        public void MultiClass_Node()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "multiClass";
            string styleOutput = GetStyleOutput(file);

            //Content Test - First
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
			_validate.ClassName = "a_b_c_main_body";
            string content = "sample text file b a c";
            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1);

            _validate.ClassName = "a_main_body";
            content = "sample text file a";
            returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1);

            _validate.ClassName = "b_main_body";
            content = "sample text file b";
            returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1);

            _validate.ClassName = "a_c_main_body";
            content = "sample text file a c";
            returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1);


            //Note - The Styles will be created while processing xhtml(content.xml)
            //Style Test - Second
            _validate = new ValidateXMLFile(styleOutput);


			_validate.ClassName = "a_b_c_main_body";
            _validate.ClassProperty.Add("fo:color", "#ffa500");
            _validate.ClassProperty.Add("fo:font-size", "24pt");

            bool returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            _validate.ClassName = "a_main_body";
            _validate.ClassProperty.Add("fo:color", "#0000ff");
            _validate.ClassProperty.Add("fo:font-size", "18pt");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);


            _validate.ClassName = "b_main_body";
            _validate.ClassProperty.Add("fo:color", "#ff0000");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);


            _validate.ClassName = "a_c_main_body";
            _validate.ClassProperty.Add("fo:color", "#ffa500");
            _validate.ClassProperty.Add("fo:font-size", "18pt");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);
        }

		///<summary>
		/// LO MultiClass
		/// </summary>
		[Test]
		public void SubEntryMultiClass_Node()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "NewMultiClassSubEntry";
			string styleOutput = GetStyleOutput(file);
			//Content Test - First
			_validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
			_validate.ClassName = "mainentrysubentry_subentry_mainentrysubentries_subentries_entry_letData_body";
			_validate.GetInnerText = true;
			string content = "wirek hen comp. [ˈwiɾ.ək ʁɛn] earlier, old, pertaining to or belonging to an earlier time.";
			bool returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
			Assert.IsTrue(returnValue1);

			//Note - The Styles will be created while processing xhtml(content.xml)
			//Style Test - Second
			_validate = new ValidateXMLFile(styleOutput);

			_validate.ClassName = "mainentrysubentry_subentry_mainentrysubentries_subentries_entry_letData_body";
			_validate.ClassProperty.Add("fo:text-indent", "-12pt");
			_validate.ClassProperty.Add("fo:margin-left", "33pt");
			bool returnValue = _validate.ValidateNodeAttributesNS(true);
			Assert.IsTrue(returnValue);

			_validate.ClassName = "subentry.-subentries_entry";
			_validate.ClassProperty.Add("text:display", "block");
			returnValue = _validate.ValidateNodeAttributesNS(false);
			Assert.IsTrue(returnValue);
		}

		///<summary>
		/// Indesign MultiClass
		/// </summary>
		[Test]
		public void PunctuationPseudoChild_Node()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "NewPunctuationPsuedoChild";
			GetStyleOutput(file);
			//Content Test - First
			_validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
			_validate.ClassName = "span_.bzh-fonipa_form_pronunciation_pronunciations_entry_letData_dicBody";
			_validate.GetInnerText = true;
			string content = "[ᵐbə.ˈᵑɡoᵑɢ]";
			bool returnValue1 = _validate.ValidateOfficeTextNode(content, "span");
			Assert.IsTrue(returnValue1);

			_validate.ClassName = "span_.bzh-fonipa_form_pronunciation_pronunciations_mainentrysubentry_subentry_mainentrysubentries_subentries_entry_letData_dicBody";
			_validate.GetInnerText = true;
			content = "[ᵐbə.ˈᵑɡoᵑɢ l̪in]";
			returnValue1 = _validate.ValidateOfficeTextNode(content, "span");
			Assert.IsTrue(returnValue1);
		}


        ///<summary>
        /// Counter
        /// </summary>
        [Test]
        public void CounterPart_Node()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "CounterPart";
            GetStyleOutput(file);

            //Content Test - First
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.ClassName = "entry_letData_dicBody";
            _validate.GetInnerText = true;
            string content = "(sem. domains: counterparts.)";
            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1);
        }
        #region FileTest

        ///<summary>
        ///Full Scripture Test
        /// </summary>
        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void BughotugospelsExport()
        {
            _projInfo.ProjectInputType = "Scripture";
            const string file = "Bughotu-gospels";
            string styleOutput = GetStyleOutput(file);
            string styleExpected = Common.PathCombine(_expectedPath, "Bughotu-gospelsstyles.xml");
            string contentExpected = Common.PathCombine(_expectedPath, "Bughotu-gospelscontent.xml");
            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        ///Telugu Font FootnoteMarkerTestcase Scripture Test
        ///</summary>
        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void TeluguFootnoteMarkerTest()
        {
            _projInfo.ProjectInputType = "Scripture";
            const string file = "TeluguFootnoteMarkerTest";
            string styleOutput = GetStyleOutput(file);
            string styleExpected = Common.PathCombine(_expectedPath, "TeluguFootnoteMarkerTeststyles.xml");
            string contentExpected = Common.PathCombine(_expectedPath, "TeluguFootnoteMarkerTestcontent.xml");
            XmlAssert.Ignore(styleOutput, "//office:font-face-decls", new Dictionary<string, string> { { "office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0" } });
            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        ///TokPisinExport Full Scripture Test
        /// </summary>
        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void TokPisinExport()
        {
            _projInfo.ProjectInputType = "Scripture";
            const string file = "TokPisin";
            string styleOutput = GetStyleOutput(file);
            string styleExpected = Common.PathCombine(_expectedPath, file + "styles.xml");
            string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
			if (Common.UsingMonoVM)
			{
				styleExpected = Common.PathCombine(_expectedlinuxPath, file + "styles.xml");
				contentExpected = Common.PathCombine(_expectedlinuxPath, file + "content.xml");
			}
            XmlAssert.Ignore(styleOutput, "//office:font-face-decls", new Dictionary<string, string> { { "office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0" } });
            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }


        ///<summary>
        ///Kabwa Full Scripture Test
        /// </summary>
        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void zKabwaExport()
        {
            _projInfo.ProjectInputType = "Scripture";
            const string file = "Kabwa";
            string styleOutput = GetStyleOutput(file);
            string styleExpected = Common.PathCombine(_expectedPath, file + "styles.xml");
            string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
            XmlAssert.Ignore(styleOutput, "//office:font-face-decls", new Dictionary<string, string> { { "office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0" } });
            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

		///<summary>
		///Dictionary Insert HardSpace Test
		///</summary>
		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void DictionaryInsertHardSpaceTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "InsertHardSpace";
			string styleOutput = GetStyleOutput(file);
			string styleExpected = Common.PathCombine(_expectedPath, file + "styles.xml");
			string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
			if (Common.UsingMonoVM)
			{
				styleExpected = Common.PathCombine(_expectedlinuxPath, file + "styles.xml");
				contentExpected = Common.PathCombine(_expectedlinuxPath, file + "content.xml");
			}
            XmlAssert.Ignore(styleOutput, "//office:font-face-decls", new Dictionary<string, string> { { "office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0" } });
			XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
			XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
		}

		///<summary>
		///Dictionary Insert HardSpace Test
		///</summary>
		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void EntryStyleTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "EntryStyle";
			string styleOutput = GetStyleOutput(file);
			string styleExpected = Common.PathCombine(_expectedPath, file + "styles.xml");
			string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
			if (Common.UsingMonoVM)
			{
				styleExpected = Common.PathCombine(_expectedlinuxPath, file + "styles.xml");
				contentExpected = Common.PathCombine(_expectedlinuxPath, file + "content.xml");
			}
			XmlAssert.Ignore(styleOutput, "//office:font-face-decls", new Dictionary<string, string> { { "office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0" } });
			XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
			XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
		}

        ///<summary>
        ///B1pe Full Scripture Test
        /// </summary>
        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void B1peExport()
        {
            _projInfo.ProjectInputType = "Scripture";
            const string file = "B1pe";
            string styleOutput = GetStyleOutput(file);
            string styleExpected = Common.PathCombine(_expectedPath, file + "styles.xml");
            string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }



		///<summary>
		///MexicoStyle Dictionary main export Test TD-4708
        /// </summary>
        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
		public void MainStyleChangesinMexicoExport()
        {
            _projInfo.ProjectInputType = "Dictionary";
			const string file = "StyleChangesinMexico";
            string styleOutput = GetStyleOutput(file);
            string styleExpected = Common.PathCombine(_expectedPath, file + "styles.xml");
            string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
			if (Common.UsingMonoVM)
			{
				styleExpected = Common.PathCombine(_expectedlinuxPath, file + "styles.xml");
				contentExpected = Common.PathCombine(_expectedlinuxPath, file + "content.xml");
			}
            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }


		///<summary>
		///MexicoStyle Dictionary reversal export Test TD-4708
        /// </summary>
        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
		public void ReversalStyleChangesinMexicoExport()
        {
            _projInfo.ProjectInputType = "Dictionary";
			const string file = "StyleChangesinMexicoReversal";
            string styleOutput = GetStyleOutput(file);
            string styleExpected = Common.PathCombine(_expectedPath, file + "styles.xml");
            string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
			if (Common.UsingMonoVM)
			{
				styleExpected = Common.PathCombine(_expectedlinuxPath, file + "styles.xml");
				contentExpected = Common.PathCombine(_expectedlinuxPath, file + "content.xml");
			}
            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

		///<summary>
		///MexicoStyle Dictionary Slash symbol
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
		public void SlashSymbolExport()
        {
            _projInfo.ProjectInputType = "Dictionary";
			const string file = "SlashSymbol";
            string styleOutput = GetStyleOutput(file);
            string styleExpected = Common.PathCombine(_expectedPath, file + "styles.xml");
            string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
			if (Common.UsingMonoVM)
			{
				styleExpected = Common.PathCombine(_expectedlinuxPath, file + "styles.xml");
				contentExpected = Common.PathCombine(_expectedlinuxPath, file + "content.xml");
			}
            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }


        ///<summary>
        ///Table structure Test
        /// </summary>
        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void Table1Test()
        {
            _projInfo.ProjectInputType = "Scripture";
            const string file = "Table";

            string styleOutput = GetStyleOutput(file);

            string styleExpected = Common.PathCombine(_expectedPath, file + "styles.xml");
            string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
            XmlAssert.Ignore(styleOutput, "//office:font-face-decls", new Dictionary<string, string> { { "office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0" } });
            TextFileAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            TextFileAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        ///Table structure Test
        /// </summary>
        [Test]
		[Category("SkipOnTeamCity")]
        public void Table2Test()
        {
            _projInfo.ProjectInputType = "Scripture";
            const string file = "Table2";

            string styleOutput = GetStyleOutput(file);

            string styleExpected = Common.PathCombine(_expectedPath, file + "styles.xml");
            string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
			XmlAssert.Ignore(styleOutput, "//office:font-face-decls", new Dictionary<string, string> { { "office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0" } });
            TextFileAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            TextFileAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }


        ///<summary>
        /// Space Issue fixed, before any modification pls contact sankar
        /// </summary>
        [Test]
        public void CrossRefSpaceTest()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "CrossRefSpace";

            string styleOutput = GetStyleOutput(file);

            string styleExpected = Common.PathCombine(_expectedPath, file + "styles.xml");
            string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
            XmlAssert.Ignore(styleOutput, "//office:font-face-decls", new Dictionary<string, string> { { "office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0" } });
            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            if (!_isLinux)
            {
                using (var p = Process.Start(Environment.GetEnvironmentVariable("COMSPEC"), string.Format("/c fc {0} {1} >{2}temp.txt", contentExpected, _projInfo.TempOutputFolder, file)))
                {
                    p.WaitForExit();
                    Debug.Print(FileData.Get(file + "temp.txt"));
                }
            }
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        /// Space Issue fixed, before any modification pls contact sankar
        /// </summary>
        [Test]
        public void WhiteSpaceNextLineTest()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "WhiteSpaceNextLine";

            string styleOutput = GetStyleOutput(file);

            string styleExpected = Common.PathCombine(_expectedPath, file + "styles.xml");
            string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
            XmlAssert.Ignore(styleOutput, "//office:font-face-decls", new Dictionary<string, string> { { "office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0" } });
            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        ///B1pe Full Scripture Test
        /// </summary>
        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void ImageNoCaptionFileTest()
        {
            _projInfo.ProjectInputType = "Scripture";
            const string file = "ImageEmptyCaption";

            string styleOutput = GetStyleOutput(file);

            string styleExpected = Common.PathCombine(_expectedPath, file + "styles.xml");
            string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
            XmlAssert.Ignore(styleOutput, "//office:font-face-decls", new Dictionary<string, string>{{"office","urn:oasis:names:tc:opendocument:xmlns:office:1.0"}});
            TextFileAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            TextFileAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }
        ///<summary>
        ///B1pe Full Scripture Test
        /// </summary>
        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void TeTestExport()
        {
            _projInfo.ProjectInputType = "Scripture";
            const string file = "TeTest";
            string styleOutput = GetStyleOutput(file);
            string styleExpected = Common.PathCombine(_expectedPath, file + "styles.xml");
            string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
            XmlAssert.Ignore(styleOutput, "//office:font-face-decls", new Dictionary<string, string> { { "office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0" } });
            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        ///FullBuangTest
        /// </summary>
        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void BuangExport()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "BuangExport";
            string styleOutput = GetStyleOutput(file);
            string styleExpected = Common.PathCombine(_expectedPath, file + "styles.xml");
            string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
			if (Common.UsingMonoVM)
			{
				styleExpected = Common.PathCombine(_expectedlinuxPath, file + "styles.xml");
				contentExpected = Common.PathCombine(_expectedlinuxPath, file + "content.xml");
			}
            XmlAssert.Ignore(styleOutput, "//office:font-face-decls", new Dictionary<string, string> { { "office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0" } });
            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        /// TD-3560
        /// </summary>
        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void WhiteSpaceTest()
        {
            _projInfo.ProjectInputType = "Scripture";
            const string file = "InsertWhiteSpace";
            string styleOutput = GetStyleOutput(file);
            string styleExpected = Common.PathCombine(_expectedPath, file + "styles.xml");
            string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
            XmlAssert.Ignore(styleOutput, "//office:font-face-decls", new Dictionary<string, string> { { "office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0" } });
            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        /// TD-3554
        /// </summary>
        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void IsBookNameChanged()
        {
            _projInfo.ProjectInputType = "Scripture";
            const string file = "BookNameChanged";
            string styleOutput = GetStyleOutput(file);
            string styleExpected = Common.PathCombine(_expectedPath, file + "styles.xml");
            string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
			if (Common.UsingMonoVM)
			{
				styleExpected = Common.PathCombine(_expectedlinuxPath, file + "styles.xml");
				contentExpected = Common.PathCombine(_expectedlinuxPath, file + "content.xml");
			}
            XmlAssert.Ignore(styleOutput, "//office:font-face-decls", new Dictionary<string, string> { { "office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0" } });
            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        /// TD-3563
        /// </summary>
        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void SpacesInDictionary()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "SpacesInDictionary";
            string styleOutput = GetStyleOutput(file);
            string styleExpected = Common.PathCombine(_expectedPath, file + "styles.xml");
            string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
			if (Common.UsingMonoVM)
			{
				styleExpected = Common.PathCombine(_expectedlinuxPath, file + "styles.xml");
				contentExpected = Common.PathCombine(_expectedlinuxPath, file + "content.xml");
			}
            XmlAssert.Ignore(styleOutput, "//office:font-face-decls", new Dictionary<string, string> { { "office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0" } });
            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

		///<summary>
        /// TD-3607
        /// </summary>
        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void GuidewordLength()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "GuidewordLength";
            string styleOutput = GetStyleOutput(file);
            string styleExpected = Common.PathCombine(_expectedPath, file + "styles.xml");
            string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
			if (Common.UsingMonoVM)
			{
				styleExpected = Common.PathCombine(_expectedlinuxPath, file + "styles.xml");
				contentExpected = Common.PathCombine(_expectedlinuxPath, file + "content.xml");
			}
            XmlAssert.Ignore(styleOutput, "//office:font-face-decls", new Dictionary<string, string> { { "office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0" } });
            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        /// TD-3275
        /// </summary>
        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void OutlineLevel()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "OutlineLevel";
            string styleOutput = GetStyleOutput(file);
            string styleExpected = Common.PathCombine(_expectedPath, file + "styles.xml");
            string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
			if (Common.UsingMonoVM)
			{
				styleExpected = Common.PathCombine(_expectedlinuxPath, file + "styles.xml");
				contentExpected = Common.PathCombine(_expectedlinuxPath, file + "content.xml");
			}
            XmlAssert.Ignore(styleOutput, "//office:font-face-decls", new Dictionary<string, string> { { "office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0" } });
            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }


        ///<summary>
        /// TD-3680
        /// </summary>
        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void FrontMatterDirection()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "FrontMatterDirection";
            string styleOutput = GetStyleOutput(file);
            string styleExpected = Common.PathCombine(_expectedPath, file + "styles.xml");
            string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
            XmlAssert.Ignore(styleOutput, "//office:font-face-decls", new Dictionary<string, string> { { "office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0" } });
            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        /// TD-3603
        /// </summary>
        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void RevarsalHeader()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "RevarsalHeader";
            string styleOutput = GetStyleOutput(file);
            string styleExpected = Common.PathCombine(_expectedPath, file + "styles.xml");
            string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
			if (Common.UsingMonoVM)
			{
				styleExpected = Common.PathCombine(_expectedlinuxPath, file + "styles.xml");
				contentExpected = Common.PathCombine(_expectedlinuxPath, file + "content.xml");
			}
            XmlAssert.Ignore(styleOutput, "//office:font-face-decls", new Dictionary<string, string> { { "office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0" } });
            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        /// TD-3681
        /// </summary>
        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void RightToLeftHeader()
        {
            _projInfo.ProjectInputType = "Scripture";
            const string file = "RightToLeftHeader";
            string styleOutput = GetStyleOutput(file);
            string styleExpected = Common.PathCombine(_expectedPath, file + "styles.xml");
            string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
            XmlAssert.Ignore(styleOutput, "//office:font-face-decls", new Dictionary<string, string> { { "office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0" } });
            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }


        ///<summary>
        /// TD-3608
        /// </summary>
        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void RevarsalEmptyPage()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "RevarsalEmptyPage";
            string styleOutput = GetStyleOutput(file);
            string styleExpected = Common.PathCombine(_expectedPath, file + "styles.xml");
            string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
			if (Common.UsingMonoVM)
			{
				styleExpected = Common.PathCombine(_expectedlinuxPath, file + "styles.xml");
				contentExpected = Common.PathCombine(_expectedlinuxPath, file + "content.xml");
			}
            XmlAssert.Ignore(styleOutput, "//office:font-face-decls", new Dictionary<string, string> { { "office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0" } });
            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }


        ///<summary>
        /// TD-3563
        /// </summary>
        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void MainTitleDisplay()
        {
            _projInfo.ProjectInputType = "Scripture";
            const string file = "MainTitleDisplay";
            string styleOutput = GetStyleOutput(file);
            string styleExpected = Common.PathCombine(_expectedPath, file + "styles.xml");
            string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
            XmlAssert.Ignore(styleOutput, "//office:font-face-decls", new Dictionary<string, string> { { "office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0" } });
            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }


        ///<summary>
        /// TD-3498
        /// </summary>
        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void KeepWithNextTest()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "KeepWithNextTest";
            string styleOutput = GetStyleOutput(file);
            string styleExpected = Common.PathCombine(_expectedPath, file + "styles.xml");
            string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
            XmlAssert.Ignore(styleOutput, "//office:font-face-decls", new Dictionary<string, string> { { "office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0" } });
            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");

            const string styleFilename = "KeepWithNextStyleTest.xml";

            string input = FileInput(styleFilename);
            string output = FileOutput(styleFilename);
            File.Copy(input, output, true);
            ExportLibreOffice.InsertKeepWithNextinEntryStyle(FileOutput(""), styleFilename);
            styleExpected = Common.PathCombine(_expectedPath, styleFilename);
            XmlAssert.AreEqual(styleExpected, output, styleFilename + " in styles.xml");
        }

        ///<summary>
        /// TD-4210
        /// The word "itamo­kori" has hidden hyphenation by unicode hyphen character(\u00AD).
        /// Can copy and paste it in notepad.
        /// </summary>
        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void HyphenationWordTest()
        {
            _projInfo.ProjectInputType = "Scripture";
            const string file = "HyphenationRuleLO";
            Param.HyphenEnable = true;
            GetStyleOutput(file);
            _validate = new ValidateXMLFile(_projInfo.TempOutputFolder);
            _validate.GetInnerText = true;
            _validate.ClassName = "Paragraph_scrBook_scrBody";
			const string content = "17 Epʉraꞌan awonsiꞌkɨ Tepiꞌ, kin pe teꞌsen pʉꞌkʉ pona, 14 kaisa rɨ itamokori ton uꞌtɨsaꞌ esiꞌpʉ mɨrɨ.";
            bool returnValue1 = _validate.ValidateOfficeTextNode(content, "para");
            Assert.IsTrue(returnValue1, "Hyphenation word test failure");
            Param.HyphenEnable = false;

        }


		///<summary>
		/// Buang Ws Test
		/// </summary>
		[Test]
		[Category("SkipOnTeamCity")]
		public void FlexGuidewordStyleTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "NewGuidewordStyle2";
			string styleOutput = GetStyleOutput(file);
			string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
			if (Common.UsingMonoVM)
			{
				contentExpected = Common.PathCombine(_expectedlinuxPath, file + "content.xml");
			}
			XmlAssert.Ignore(styleOutput, "//office:font-face-decls", new Dictionary<string, string> { { "office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0" } });
			XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
		}

		///<summary>
		///New Flex Dictionary Buang Color Test
		///</summary>
		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void BuangColorTestFlex83()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "NewFlexBuangColor";
			string styleOutput = GetStyleOutput(file);
			string styleExpected = Common.PathCombine(_expectedPath, file + "styles.xml");
			string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
			XmlAssert.Ignore(styleOutput, "//office:font-face-decls", new Dictionary<string, string> { { "office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0" } });

			if (Common.UsingMonoVM)
			{
				styleExpected = Common.PathCombine(_expectedlinuxPath, file + "styles.xml");
				contentExpected = Common.PathCombine(_expectedlinuxPath, file + "content.xml");
			}
			XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
			XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
		}

		///<summary>
		/// Anchor with space Test
		/// </summary>
		[Test]
		[Category("SkipOnTeamCity")]
		public void AnchorSpaceFlex83Test()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "AnchorSpaceFlex83";
			string styleOutput = GetStyleOutput(file);
			string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
			if (Common.UsingMonoVM)
				contentExpected = Common.PathCombine(_expectedlinuxPath, file + "content.xml");
			XmlAssert.Ignore(styleOutput, "//office:font-face-decls", new Dictionary<string, string> { { "office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0" } });
			XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
		}

		///<summary>
		/// TD-4673
		/// </summary>
		[Test]
		[Category("ShortTest")]
		[Category("SkipOnTeamCity")]
		public void SelfSpaceTestInDictionary()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "selfspacetest";
			string styleOutput = GetStyleOutput(file);
			string styleExpected = Common.PathCombine(_expectedPath, file + "styles.xml");
			string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
			if (Common.UsingMonoVM)
			{
				styleExpected = Common.PathCombine(_expectedlinuxPath, file + "styles.xml");
				contentExpected = Common.PathCombine(_expectedlinuxPath, file + "content.xml");
			}
			XmlAssert.Ignore(styleOutput, "//office:font-face-decls", new Dictionary<string, string> { { "office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0" } });
			XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
			XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
		}

		///<summary>
		///Space issue fixed for etymology Test TD-4617
		///</summary>
		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void SpaceAfterClosingBraceTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "SpaceAfterClosingBrace";
			string styleOutput = GetStyleOutput(file);
			string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
			if (Common.UsingMonoVM)
			{
				contentExpected = Common.PathCombine(_expectedlinuxPath, file + "content.xml");
			}
			XmlAssert.Ignore(styleOutput, "//office:font-face-decls", new Dictionary<string, string> { { "office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0" } });
			XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
		}

		///<summary>
		///Space issue fixed for parentPrecede Test TD-4651
		///</summary>
		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void ParentPrecedeSpaceTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "ParentPrecedeSpace";
			string styleOutput = GetStyleOutput(file);
			string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
			if (Common.UsingMonoVM)
			{
				contentExpected = Common.PathCombine(_expectedlinuxPath, file + "content.xml");
			}
			XmlAssert.Ignore(styleOutput, "//office:font-face-decls", new Dictionary<string, string> { { "office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0" } });
			XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
		}

		///<summary>
		///Avoid space after the picture in new FLEX8.3 Test TD-4675
		///</summary>
		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void AfterPictureSpaceTest()
		{
			_projInfo.ProjectInputType = "Dictionary";
			const string file = "PictureSpaceAfter";
			string styleOutput = GetStyleOutput(file);
			string contentExpected = Common.PathCombine(_expectedPath, file + "content.xml");
			if(Common.UsingMonoVM)
				contentExpected = Common.PathCombine(_expectedlinuxPath, file + "content.xml");

			XmlAssert.Ignore(styleOutput, "//office:font-face-decls", new Dictionary<string, string> { { "office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0" } });
			XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
		}


		#endregion
		#endregion

		private void CopyInputToOutput()
        {
            string[] files = new[] { "main.odt", "flexrev.odt", "main.odm", "flexrev.css", "main.xhtml", "flexrev.xhtml", "main.css", "flexrev.css" };
            foreach (string file in files)
            {
                string outputfile = FileOutput(file);
                File.Delete(outputfile);
            }

            files = new[] { "main.xhtml", "FlexRev.xhtml", "main.css", "FLExRev.css" };
            foreach (string file in files)
            {
                CopyExistingFile(file);
            }
        }

        private bool CheckFileExist()
        {
            bool returnValue = true;
            string[] files = new[] { "main.odt", "FlexRev.odt", "main.odm" };
            foreach (string file in files)
            {
                string outputfile = FileOutput(file);
                if (!File.Exists(outputfile))
                {
                    returnValue = false;
                    break;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Copies a file if it exists from the input test path to the output
        /// </summary>
        /// <param name="fileName">file to be copied if it exists</param>
        private void CopyExistingFile(string fileName)
        {
            if (File.Exists(FileInput(fileName)))
                File.Copy(FileInput(fileName), FileOutput(fileName), true);
        }

        private string GetStyleOutput(string file)
        {
            LOContent contentXML = new LOContent();
            LOStyles stylesXML = new LOStyles();
            string fileOutput = _index > 0 ? file + _index + ".css" : file + ".css";

            //string input = FileInput(file + ".css");
            string input = FileInput(fileOutput);

            _projInfo.DefaultCssFileWithPath = input;
            _projInfo.TempOutputFolder = _outputPath;

            Dictionary<string, Dictionary<string, string>> cssClass = new Dictionary<string, Dictionary<string, string>>();
            CssTree cssTree = new CssTree();
            cssClass = cssTree.CreateCssProperty(input, true);

            //StyleXML
            string styleOutput = FileOutput(file + _styleFile);
            Dictionary<string, Dictionary<string, string>> idAllClass = stylesXML.CreateStyles(_projInfo, cssClass, styleOutput);

            // ContentXML
            var pageSize = new Dictionary<string, string>();
            pageSize["height"] = cssClass["@page"]["page-height"];
            pageSize["width"] = cssClass["@page"]["page-width"];
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            _projInfo.HideSpaceVerseNumber = stylesXML.HideSpaceVerseNumber;

            PreExportProcess preProcessor = new PreExportProcess(_projInfo);
            preProcessor.GetTempFolderPath();
            _projInfo.DefaultXhtmlFileWithPath = preProcessor.ProcessedXhtml;
            if (Param.HyphenEnable)
                preProcessor.IncludeHyphenWordsOnXhtml(_projInfo.DefaultXhtmlFileWithPath);

            AfterBeforeProcess afterBeforeProcess = new AfterBeforeProcess();
            afterBeforeProcess.RemoveAfterBefore(_projInfo, cssClass, cssTree.SpecificityClass, cssTree.CssClassOrder);

            contentXML.CreateStory(_projInfo, idAllClass, cssTree.SpecificityClass, cssTree.CssClassOrder, 325, pageSize);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + _contentFile;
            return styleOutput;
        }

        private string GetStyleOutput(PublicationInformation projInfo)
        {
            LOContent contentXML = new LOContent();
            LOStyles stylesXML = new LOStyles();
            projInfo.TempOutputFolder = _outputPath;
            string file = Path.GetFileNameWithoutExtension(_projInfo.DefaultXhtmlFileWithPath);

            Dictionary<string, Dictionary<string, string>> cssClass = new Dictionary<string, Dictionary<string, string>>();
            CssTree cssTree = new CssTree();
            cssClass = cssTree.CreateCssProperty(projInfo.DefaultCssFileWithPath, true);

            //StyleXML
            string fileOutput = _index > 0 ? file + _index + _styleFile : file + _styleFile;
            string styleOutput = FileOutput(fileOutput);
            Dictionary<string, Dictionary<string, string>> idAllClass = stylesXML.CreateStyles(_projInfo, cssClass, styleOutput);

            // ContentXML
            var pageSize = new Dictionary<string, string>();
            pageSize["height"] = cssClass["@page"]["page-height"];
            pageSize["width"] = cssClass["@page"]["page-width"];
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateStory(_projInfo, idAllClass, cssTree.SpecificityClass, cssTree.CssClassOrder, 325, pageSize);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + _contentFile;
            return styleOutput;
        }

        private static void LoadParam(string inputType, string tocTrueFalse)
        {
            // Verifying the input setting file and css file - in Input Folder
            string settingFile = inputType + "StyleSettings.xml";
            string sFileName = Common.PathCombine(_outputBasePath, settingFile);
            Common.ProgBase = _outputBasePath;

            Param.LoadSettings();
            Param.SetValue(Param.InputType, inputType);
            Param.LoadSettings();
            // setup - ensure that there is a current organization in the StyleSettings xml
            Param.UpdateMetadataValue(Param.TableOfContents, tocTrueFalse);
            Param.Write();


            Param.LoadValues(sFileName);
            Param.SetLoadType = inputType;
            Param.Value["OutputPath"] = _outputBasePath;
            Param.Value["UserSheetPath"] = _outputBasePath;
        }
    }
}