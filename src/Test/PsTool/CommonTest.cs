// --------------------------------------------------------------------------------------------
// <copyright file="CommonTest.cs" from='2009' to='2014' company='SIL International'>
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
// Test methods of FlexDePlugin
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using SIL.PublishingSolution;
using SIL.Tool;
using Assert = NUnit.Framework.Assert;

namespace Test.PsTool
{
    /// <summary>
    ///This is a test class for CommonTest
    ///</summary>
    [TestFixture]
    public partial class CommonTest
    {
        private XmlDocument _doc;
        PublicationInformation _target;
        XmlDocument actualDocument;
        private string _projectFilePath;
        string _allUserPath;
        string _inputBasePath = string.Empty;
        string _outputBasePath = string.Empty;
        string _expectBasePath = string.Empty;

        /// <summary>
        /// Turn off debug.assert messages for unit tests
        /// </summary>
        [TestFixtureSetUp]
        protected void SetUp()
        {
            _doc = new XmlDocument();
            _target = new PublicationInformation();
            actualDocument = Common.DeclareXMLDocument(false);
            LoadInputDocument("Dictionary1.de");
            _allUserPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Common.SupportFolder = "";
            _outputBasePath = Common.PathCombine(GetTestPath(), "Output");
            _inputBasePath = Common.PathCombine(GetTestPath(), "InputFiles");
            _expectBasePath = Common.PathCombine(GetTestPath(), "Expected");
            if (Directory.Exists(_outputBasePath))
                Directory.Delete(_outputBasePath, true);
            Directory.CreateDirectory(_outputBasePath);
            Common.ProgInstall = PathPart.Bin(Environment.CurrentDirectory, @"/../../DistFiles");
            Common.Testing = true;
        }


        #region ValidateNumber
        /// <summary>
        ///A test for ValidateNumber
        ///</summary>
        [Test]
        public void ValidateNumber1()
        {
            string number = "123"; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual = Common.ValidateNumber(number);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ValidateNumber
        ///</summary>
        [Test]
        public void ValidateNumber2()
        {
            string number = "Number"; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual = Common.ValidateNumber(number);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ValidateNumber
        ///</summary>
        [Test]
        public void ValidateNumberNullEmpty()
        {
            bool expected = false; // TODO: Initialize to an appropriate value

            string number = ""; // TODO: Initialize to an appropriate value
            bool actual = Common.ValidateNumber(number);
            Assert.AreEqual(expected, actual);

            number = null; // TODO: Initialize to an appropriate value
            actual = Common.ValidateNumber(number);
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region ValidateAlphabets
        /// <summary>
        ///A test for ValidateAlphabets
        ///</summary>
        [Test]
        public void ValidateAlphabets1()
        {
            string stringValue = "color"; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual = Common.ValidateAlphabets(stringValue);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ValidateAlphabets
        ///</summary>
        [Test]
        public void ValidateAlphabets2()
        {
            string stringValue = "123"; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual = Common.ValidateAlphabets(stringValue);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ValidateAlphabets
        ///</summary>
        [Test]
        public void ValidateAlphabetsNullEmpty()
        {
            bool expected = false; // TODO: Initialize to an appropriate value

            string stringValue = ""; // TODO: Initialize to an appropriate value
            bool actual = Common.ValidateAlphabets(stringValue);
            Assert.AreEqual(expected, actual);

            stringValue = null; // TODO: Initialize to an appropriate value
            actual = Common.ValidateAlphabets(stringValue);
            Assert.AreEqual(expected, actual);
        }

        #endregion

        /// <summary>
        ///A test for RightString
        ///</summary>
        [Test]
        public void RightString1()
        {
            string fullString = "Left_Right";
            string splitString = "_";
            string expected = "Right";
            string actual = Common.RightString(fullString, splitString);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for RightString
        ///</summary>
        [Test]
        public void RightString2()
        {
            string fullString = "LeftRight";
            string splitString = "_";
            string expected = "LeftRight";
            string actual = Common.RightString(fullString, splitString);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for RightString
        ///</summary>
        [Test]
        public void RightStringSplitNullEmpty()
        {
            string fullString = "LeftRight";
            string expected = "LeftRight";

            string splitString = null;
            string actual = Common.RightString(fullString, splitString);
            Assert.AreEqual(expected, actual);

            splitString = "";
            actual = Common.RightString(fullString, splitString);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for RightString
        ///</summary>
        [Test]
        public void RightStringFullStringNullEmpty()
        {
            string fullString = "";
            string splitString = null;
            string expected = "";
            string actual = Common.RightString(fullString, splitString);
            Assert.AreEqual(expected, actual);

            fullString = null;
            actual = Common.RightString(fullString, splitString);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for LeftString
        ///</summary>
        [Test]
        public void LeftString1()
        {
            string fullString = "Left_Right";
            string splitString = "_";
            string expected = "Left";
            string actual = Common.LeftString(fullString, splitString);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for LeftString
        ///</summary>
        [Test]
        public void LeftString2()
        {
            string fullString = "LeftRight";
            string splitString = "_";
            string expected = "LeftRight";
            string actual = Common.LeftString(fullString, splitString);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for LeftString
        ///</summary>
        [Test]
        public void LeftStringSplitNullEmpty()
        {
            string fullString = "LeftRight";
            string expected = "LeftRight";

            string splitString = null;
            string actual = Common.LeftString(fullString, splitString);
            Assert.AreEqual(expected, actual);

            splitString = "";
            actual = Common.LeftString(fullString, splitString);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for LeftString
        ///</summary>
        [Test]
        public void LeftStringFullStringNullEmpty()
        {
            string fullString = "";
            string splitString = null;
            string expected = "";
            string actual = Common.LeftString(fullString, splitString);
            Assert.AreEqual(expected, actual);

            fullString = null;
            actual = Common.LeftString(fullString, splitString);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for RightRemove
        ///</summary>
        [Test]
        public void RightRemove1()
        {
            string fullString = "Left_Right";
            string splitString = "_";
            string expected = "Left";
            string actual = Common.RightRemove(fullString, splitString);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for RightRemove
        ///</summary>
        [Test]
        public void RightRemove2()
        {
            string fullString = "LeftRight";
            string splitString = "_";
            string expected = "LeftRight";
            string actual = Common.RightRemove(fullString, splitString);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for RightRemove
        ///</summary>
        [Test]
        public void RightRemoveSplitNullEmpty()
        {
            string fullString = "LeftRight";
            string expected = "LeftRight";

            string splitString = null;
            string actual = Common.RightRemove(fullString, splitString);
            Assert.AreEqual(expected, actual);

            splitString = "";
            actual = Common.RightRemove(fullString, splitString);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for RightRemove
        ///</summary>
        [Test]
        public void RightRemoveFullStringNullEmpty()
        {
            string fullString = "";
            string splitString = null;
            string expected = "";
            string actual = Common.RightRemove(fullString, splitString);
            Assert.AreEqual(expected, actual);

            fullString = null;
            actual = Common.RightRemove(fullString, splitString);
            Assert.AreEqual(expected, actual);
        }


        /*
         * // TODO: Convert this to UnitConvertor
        /// <summary>
        ///A test for ConvertToInch
        ///</summary>
        [Test]
        public void ConvertToInchTest()
        {
            string attribute = string.Empty;
            float expected = 0F;
            float actual;
            actual = Common.ConvertToInch(attribute);
            Assert.AreEqual(expected, actual);

        }
         */

        /// <summary>
        ///A test for GetAllUserPathWithSilPs
        ///</summary>
        [Test]
        public void GetAllUserPathWithSilPs()
        {
            //string allUserPath = Common.DirectoryPathReplace(_allUserPath + "/SIL/Pathway");
            string allUserPath = Common.GetAllUserPath();
            string expected = allUserPath;
            string actual = Common.GetAllUserPath();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetFiledWorksPath
        ///</summary>
        [Test]
        public void GetFiledWorksPathTest()
        {
            string allUserPath = _allUserPath;
            allUserPath += Path.DirectorySeparatorChar + "SIL" +
                           Path.DirectorySeparatorChar + "FieldWorks" +
                           Path.DirectorySeparatorChar;

            string expected = allUserPath;
            string actual = Common.GetFiledWorksPath();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetTempFolderPath
        ///</summary>
        [Test]
        public void GetTempFolderPathTest()
        {
            string expected = Path.GetTempPath();
            string actual = Path.GetTempPath();
            Assert.AreEqual(expected, actual);

        }

        ///// <summary>
        /////A test for GetTempFlexUtilityPath
        /////</summary>
        /// Note: This is not used anymore
        //[Test]
        //public void GetTempFlexUtilityPathTest()
        //{
        //    string path = @"\SIL\ooUtility\";
        //    string expected = Common.PathCombine(Path.GetTempPath(), path);
        //    string actual = Common.GetTempFlexUtilityPath();
        //    Assert.AreEqual(expected, actual);
        //}

        /// <summary>
        ///A test for GetPSApplicationPath
        ///</summary>
        [Test]
        public void GetPSApplicationPathTest()
        {
            string expected = PathPart.Bin(Environment.CurrentDirectory, "/../../DistFiles");
            string actual = Common.GetPSApplicationPath();
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///A test for SetFont
        ///</summary>
        [Test]
        public void SetFontTest()
        {
            Font font = new Font("Times New Roman", 10);
            Form frm = new Form();
            Button btnRefresh = new Button();
            btnRefresh.Text = "Button";
            btnRefresh.Font = font;

            Label lblCaption = new Label();
            lblCaption.Text = "Caption";
            lblCaption.Font = font;

            frm.Controls.Add(btnRefresh);
            frm.Controls.Add(lblCaption);

            Common.UIFont = new Font("Arial", 20);
            Common.SetFont(frm);

            Assert.AreEqual(Common.UIFont, frm.Controls[0].Font);
        }

        ///// <summary>
        /////A test for ConvertUnicodeToString
        /////</summary>
        //[Test]
        //public void ConvertUnicodeToStringTest6()
        //{
        //    string parameter = "\"\\2666h";// Double quote is wrong
        //    string expected = "";
        //    string actual = Common.ConvertUnicodeToString(parameter);
        //    Assert.AreEqual(expected, actual);

        //    parameter = "''\\2666'";// Single quote is wrong
        //    actual = Common.ConvertUnicodeToString(parameter);
        //    Assert.AreEqual(expected, actual);
        //}

        /// <summary>
        ///A test for ConvertUnicodeToString
        ///</summary>
        [Test]
        public void ConvertUnicodeToStringTest7()
        {
            string parameter = "\"\\2666h";// Double quote is wrong
            string expected = "";
            string actual = Common.ConvertUnicodeToString(parameter);
            Assert.AreEqual(expected, actual);

            parameter = "''\\2666'";// Single quote is wrong
            actual = Common.ConvertUnicodeToString(parameter);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ConvertUnicodeToString
        ///</summary>
        [Test]
        public void ConvertUnicodeToStringTestNullEmpty()
        {
            string parameter = "";// Double quote is wrong
            string expected = "";
            string actual = Common.ConvertUnicodeToString(parameter);
            Assert.AreEqual(expected, actual);

            parameter = null;// Single quote is wrong
            actual = Common.ConvertUnicodeToString(parameter);
            Assert.AreEqual(expected, actual);
        }

        ///// <summary>
        /////A test for ConvertStringToUnicode // TODO: Pending
        /////</summary>
        //[Test]
        //public void ConvertStringToUnicodeTest()
        //{
        //    string inputString = "{"; //
        //    string expected = "\007D"; //
        //    string actual = Common.ConvertStringToUnicode(inputString);
        //    Assert.AreEqual(expected, actual);
        //}

        ///// <summary>  // TODO ONLY used in Tests
        /////A test for PublishingSolutionsEnvironmentReset
        /////</summary>
        //[Test]
        //public void PublishingSolutionsEnvironmentResetTest()
        //{
        //    Common.PublishingSolutionsEnvironmentReset();

        //}
        /// <summary>
        ///A test for GetNumericChar
        ///</summary>
        [Test]
        public void GetNumericCharTest1()
        {
            string inputValue = "12pt";
            int counter = 0;
            int counterExpected = 2;
            string expected = "12";
            string actual = Common.GetNumericChar(inputValue, out counter);
            Assert.AreEqual(counterExpected, counter);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetNumericChar
        ///</summary>
        [Test]
        public void GetNumericCharTest2()
        {
            string inputValue = "12";
            int counter = 0;
            int counterExpected = 2;
            string expected = "12";
            string actual = Common.GetNumericChar(inputValue, out counter);
            Assert.AreEqual(counterExpected, counter);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetNumericChar
        ///</summary>
        [Test]
        public void GetNumericCharTest3()
        {
            string inputValue = "pt";
            int counter = 0;
            int counterExpected = 0;
            string expected = "";
            string actual = Common.GetNumericChar(inputValue, out counter);
            Assert.AreEqual(counterExpected, counter);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetNumericChar
        ///</summary>
        [Test]
        public void GetNumericCharTestNullEmpty()
        {
            string inputValue = "";
            int counter = 0;
            int counterExpected = 0;
            string expected = "";
            string actual = Common.GetNumericChar(inputValue, out counter);
            Assert.AreEqual(counterExpected, counter);
            Assert.AreEqual(expected, actual);

            inputValue = null;
            expected = "";
            actual = Common.GetNumericChar(inputValue, out counter);
            Assert.AreEqual(counterExpected, counter);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for LanguageCodeAndName
        ///</summary>
        [Test]
        public void LanguageCodeAndNameTest()
        {
            Dictionary<string, string> expected = Common.LanguageCodeAndName(); // TODO: same input and expected
            Dictionary<string, string> actual = Common.LanguageCodeAndName();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetLargerSmaller
        ///</summary>
        [Test]
        public void GetLargerSmallerTest1()
        {
            float parentFont = 1.1F; // TODO: Initialize to an appropriate value
            string type = "larger"; // TODO: Initialize to an appropriate value
            int expected = 1; // TODO: Initialize to an appropriate value
            int actual = Common.GetLargerSmaller(parentFont, type);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for GetLargerSmaller
        ///</summary>
        [Test]
        public void GetLargerSmallerTest2()
        {
            float parentFont = 12.5F; // TODO: Initialize to an appropriate value
            string type = "smaller"; // TODO: Initialize to an appropriate value
            int expected = 9; // TODO: Initialize to an appropriate value
            int actual = Common.GetLargerSmaller(parentFont, type);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for isRightFieldworksVersion
        ///</summary>
        [Test]
        public void isRightFieldworksVersionTest()
        {
            bool expected = false; // Note : Clarification with Greg.
            bool actual = false; // Common.isRightFieldworksVersion();
            Assert.AreEqual(expected, actual);
        }

        ///<summary>
        ///A test for UnitConverterOO // TODO - Should be changed to UnitConverter
        ///</summary>


        /// <summary>
        ///A test for UnitConverter - Default to pt Converstion
        ///</summary>
        [Test]
        public void UnitConverterTest1()
        {
            string inputValue = "1in";
            string expected = "72";
            string actual = Common.UnitConverter(inputValue);
            Assert.AreEqual(expected, actual);


            //if (attributeUnit == "Topt")

        }
        /// <summary>
        ///A test for UnitConverter - Default to pt Converstion
        ///</summary>
        [Test]
        public void UnitConverterTest2()
        {
            string inputValue = "1pc";
            string expected = "12";
            string actual = Common.UnitConverter(inputValue);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for UnitConverter - Default to pt Converstion
        ///</summary>
        [Test]
        public void UnitConverterTest3()
        {
            string inputValue = "1px";
            string expected = "0.75";
            string actual = Common.UnitConverter(inputValue);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for UnitConverter - Default to pt Converstion
        ///</summary>
        [Test]
        public void UnitConverterTest4()
        {
            string inputValue = "1cm";
            string expected = "28.34646";
            string actual = Common.UnitConverter(inputValue);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for UnitConverter - Default to pt Converstion
        ///</summary>
        [Test]
        public void UnitConverterTest5()
        {
            string inputValue = "100%";
            string expected = "100%";
            string actual = Common.UnitConverter(inputValue);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for UnitConverter - Default to pt Converstion
        ///</summary>
        [Test]
        public void UnitConverterTest6()
        {
            string inputValue = "1em";
            string expected = "100%";
            string actual = Common.UnitConverter(inputValue);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for UnitConverter - Default to pt Converstion
        ///</summary>
        [Test]
        public void UnitConverterTestNullEmpty()
        {
            string expected = string.Empty;

            string inputValue = "";
            string actual = Common.UnitConverter(inputValue);
            Assert.AreEqual(expected, actual);

            inputValue = null;
            actual = Common.UnitConverter(inputValue);
            Assert.AreEqual(expected, actual);

        }

        ///<summary>
        ///A test for UnitConverter 2 Parameter
        ///</summary>
        [Test]
        public void UnitConverter2ParamTest1()
        {
            string inputValue = "1cm";
            string outputUnit = "in";
            string expected = "0.3937008";
            string actual = Common.UnitConverter(inputValue, outputUnit);
            Assert.AreEqual(expected, actual);
        }

        ///<summary>
        ///A test for UnitConverter 2 Parameter
        ///</summary>
        [Test]
        public void UnitConverter2ParamTest2()
        {
            string inputValue = "1in";
            string outputUnit = "cm";
            string expected = "2.54";
            string actual = Common.UnitConverter(inputValue, outputUnit);
            Assert.AreEqual(expected, actual);
        }

        ///<summary>
        ///A test for UnitConverter 2 Parameter
        ///</summary>
        [Test]
        public void UnitConverter2ParamTest3()
        {
            string inputValue = "1pt";
            string outputUnit = "in";
            string expected = "0.01388889";
            string actual = Common.UnitConverter(inputValue, outputUnit);
            Assert.AreEqual(expected, actual);
        }

        ///<summary>
        ///A test for UnitConverter 2 Parameter
        ///</summary>
        [Test]
        public void UnitConverter2ParamTest4()
        {
            string inputValue = "1pt";
            string outputUnit = "cm";
            string expected = "0.03527778";
            string actual = Common.UnitConverter(inputValue, outputUnit);
            Assert.AreEqual(expected, actual);
        }

        ///<summary>
        ///A test for UnitConverter 2 Parameter
        ///</summary>
        [Test]
        public void UnitConverter2ParamTest5()
        {
            string inputValue = "1pc";
            string outputUnit = "in";
            string expected = "0.1666667";
            string actual = Common.UnitConverter(inputValue, outputUnit);
            Assert.AreEqual(expected, actual);
        }

        ///<summary>
        ///A test for UnitConverter 2 Parameter
        ///</summary>
        [Test]
        public void UnitConverter2ParamTest6()
        {
            string inputValue = "1ex";
            string outputUnit = "em";
            string expected = "0.5";
            string actual = Common.UnitConverter(inputValue, outputUnit);
            Assert.AreEqual(expected, actual);
        }

        ///<summary>
        ///A test for UnitConverter 2 Parameter
        ///</summary>
        [Test]
        public void UnitConverter2ParamSameUnit()
        {
            string inputValue = "1in";
            string outputUnit = "in";
            string expected = "1";
            string actual = Common.UnitConverter(inputValue, outputUnit);
            Assert.AreEqual(expected, actual);
        }

        ///<summary>
        ///A test for UnitConverter 2 Parameter
        ///</summary>
        [Test]
        public void UnitConverter2ParamUnitNullEmpty()
        {
            string expected = "1in";

            string inputValue = "1in";
            string outputUnit = "";
            string actual = Common.UnitConverter(inputValue, outputUnit);
            Assert.AreEqual(expected, actual);

             inputValue = "1in";
             outputUnit = null;
             expected = "1in";
             actual = Common.UnitConverter(inputValue, outputUnit);
            Assert.AreEqual(expected, actual);
        }

        ///<summary>
        ///A test for UnitConverter 2 Parameter
        ///</summary>
        [Test]
        public void UnitConverter2ParamInputNullEmpty()
        {
            string expected = "";

            string inputValue = "";
            string outputUnit = "";
            string actual = Common.UnitConverter(inputValue, outputUnit);
            Assert.AreEqual(expected, actual);

             inputValue = null;
             outputUnit = "pt";
             actual = Common.UnitConverter(inputValue, outputUnit);
            Assert.AreEqual(expected, actual);
        }

        ///<summary>
        ///A test for migration of custom styles to the latest stylesettings.
        ///</summary>
        [Test]
        public void MigrateCustomStyleSheet()
        {
            string fileName = "DictionaryStyleSettings.xml";
            string actual = Common.PathCombine(_outputBasePath, fileName);
            string expected = Common.PathCombine(_expectBasePath, fileName);
            string customSettingsFile = Common.PathCombine(_inputBasePath, fileName);
            string installerSettingsFile = Common.PathCombine(_inputBasePath, "Actual" + fileName);
            string oCustomSettingsFile = Common.PathCombine(_outputBasePath, fileName);
            string oInstallerSettingsFile = Common.PathCombine(_outputBasePath, "Actual" + fileName);
            File.Copy(customSettingsFile, oCustomSettingsFile);
            File.Copy(installerSettingsFile, oInstallerSettingsFile);
            Common.MigrateCustomSheet(oCustomSettingsFile, oInstallerSettingsFile);
            XmlAssert.AreEqual(expected, actual, "Dictionary migration test failed");

            fileName = "ScriptureStyleSettings.xml";
            actual = Common.PathCombine(_outputBasePath, fileName);
            expected = Common.PathCombine(_expectBasePath, fileName);
            customSettingsFile = Common.PathCombine(_inputBasePath, fileName);
            installerSettingsFile = Common.PathCombine(_inputBasePath, "Actual" + fileName);
            oCustomSettingsFile = Common.PathCombine(_outputBasePath, fileName);
            oInstallerSettingsFile = Common.PathCombine(_outputBasePath, "Actual" + fileName);
            File.Copy(customSettingsFile, oCustomSettingsFile);
            File.Copy(installerSettingsFile, oInstallerSettingsFile);
            Common.MigrateCustomSheet(oCustomSettingsFile, oInstallerSettingsFile);
            XmlAssert.AreEqual(expected, actual, "Scripture migration test failed");
        }

		/// <summary>
		/// GetFontFeaturesTest
		/// Reads .ssf File and .lds File - Gets the Font Features
		/// Then Tested whether the FontFeaturesString and FontFeaturesSettingsString are correct
		/// </summary>
		[Test]
		public void GetFontFeaturesTest()
		{
			String fileName = "ncoSibe.ssf";
			string inputFolder = Common.PathCombine(_inputBasePath, "GetFontFeaturesTest");
			Common.Ssf = Common.PathCombine(inputFolder, fileName);
			Common.GetFontFeatures();
			Common.Ssf = string.Empty;
			Assert.AreEqual("\"litr\" 0,\"apos\" 1", Common.FontFeaturesString);
			Assert.AreEqual("\r\n -webkit-font-feature-settings: \"litr\" 0,\"apos\" 1; \r\n -moz-font-feature-settings: \"litr\" 0,\"apos\" 1; \r\n" +
										" -ms-font-feature-settings: \"litr\" 0,\"apos\" 1; \r\n" +
										" font-feature-settings: \"litr\" 0,\"apos\" 1; \r\n", Common.FontFeaturesSettingsString);
		}

        #region LanguageTests
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        // If unit fails, please confirm the below file exist,
        // C:\ProgramData\SIL\WritingSystemStore\ur.ldml
        public void GetTextDirection()
        {
            Common.TextDirectionLanguageFile = null; // This test depends on this variable not being set in advance
            string expected = "ltr";
            string NkonyaCode = "nko";  //Used in Nkonya dataset
            string SenaCode = "seh";    //Used in Sena dataset
            string UrduCode = "ur";     //Used in TestABS dataset
            string actual = Common.GetTextDirection(NkonyaCode);
            Assert.AreEqual(expected, actual);
            actual = Common.GetTextDirection(SenaCode);
            Assert.AreEqual(expected, actual);
            actual = Common.GetTextDirection(UrduCode);
            if(!Common.IsUnixOS()){ expected = "rtl";}
            Assert.AreEqual(expected, actual);
        }

       #endregion

        #region PathCombine
        ///<summary>
        ///A test for PathCombine
        ///</summary>
        [Test]
        public void PathCombine1()
        {
            string expected = "";
            string path1 = "";
            string path2= "";
            string actual = Common.PathCombine(path1, path2);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PathCombine2()
        {
            string dirSep = Path.DirectorySeparatorChar.ToString();
            string path1 = Path.GetTempPath();
            string path2 = "combine";

            string expected = Common.PathCombine(path1, path2);
            expected = expected.Replace("\\", dirSep);
            expected = expected.Replace("/", dirSep);

            string actual = Common.PathCombine(path1, path2);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PathCombine3()
        {
            string dirSep = Path.DirectorySeparatorChar.ToString();
            string path1 = Path.GetTempPath();
            string path2 = "..\\combine";

            string expected = Common.PathCombine(path1, path2);
            expected = expected.Replace("\\", dirSep);
            expected = expected.Replace("/", dirSep);

            string actual = Common.PathCombine(path1, path2);

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void PathCombine4()
        {
            string dirSep = Path.DirectorySeparatorChar.ToString();
            string path1 = Path.GetTempPath();
            path1 = path1.Replace('\\', '/'); // for Linux Test
            string path2 = "combine";

            string expected = Common.PathCombine(path1, path2);
            expected = expected.Replace("\\", dirSep);
            expected = expected.Replace("/", dirSep);

            string actual = Common.PathCombine(path1, path2);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PathCombine5()
        {
            string dirSep = Path.DirectorySeparatorChar.ToString();
            string path1 = Path.GetTempPath();
            path1 = path1.Replace('\\', '/'); // for Linux Test
            string path2 = "../combine";

            string expected = Common.PathCombine(path1, path2);
            expected = expected.Replace("\\", dirSep);
            expected = expected.Replace("/", dirSep);

            string actual = Common.PathCombine(path1, path2);
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region File Related Tests
        /// <summary>
        ///A test for FileInsertText
        ///</summary>
        [Test]
        public void FileInsertTextTest()
        {
            string fileName = "FileInsert.xhtml";
            string sourceFile = GetFileNameWithPath(fileName);
            string output = GetFileNameWithOutputPath(fileName);
            string expected = GetFileNameWithExpectedPath(fileName);
            string textToInsert = "Text At Top";
            CopyToOutput(sourceFile, output);
            Common.FileInsertText(output, textToInsert);
            TextFileAssert.AreEqual(expected, output);

        }

        /// <summary>
        ///A test for GetNewFileName
        ///</summary>
        [Test]
        public void GetNewFileNameTest1()
        {
            string fileName = "ProgressBar.xhtml";
            string filePath = GetFileNameWithPath(fileName);
            string expected = "ProgressBar1.xhtml";
            string actual = Common.GetNewFileName(filePath, fileName);
            actual = Path.GetFileName(actual);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for GetNewFileName // TODO - More Analyse
        ///</summary>
        [Test]
        public void GetNewFileNameTestNullEmpty()
        {
            string fileName = "";
            string filePath = GetFileNameWithPath(fileName);
            string expected = "1";
            fileName = null;
            string actual = Common.GetNewFileName(filePath, fileName);
            actual = Path.GetFileName(actual);
            Assert.AreEqual(expected, actual);

            fileName = "";
            filePath = GetFileNameWithPath(fileName);
            expected = "1";
            actual = Common.GetNewFileName(filePath, fileName);
            actual = Path.GetFileName(actual);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for GetNewFolderName
        ///</summary>
        [Test]
        public void GetNewFolderNameTest1()
        {
            //string filePath = Common.PathCombine(GetTestPath(),"Input") ;
            string filePath = GetTestPath();
            string folderName = "Input";
            string expected = "Input";
            string actual = Common.GetNewFolderName(filePath, folderName);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for GetNewFolderName
        ///</summary>
        [Test]
        public void GetNewFolderNameTest2()
        {
            //string filePath = Common.PathCombine(GetTestPath(),"Input") ;
            string filePath = GetTestPath();
            string folderName = "InputFiles";
            string expected = "InputFiles1";
            string actual = Common.GetNewFolderName(filePath, folderName);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for EpubInsertCoverCSSStyle(from Epub output) file added in css. TD-3361
        ///</summary>
        [Test]
        public void WriteEpubInsertCoverCSSStyle()
        {
            const string cssFileName = "EpubInsertCoverCSSStyle.css";
            string sourceCssFile = GetFileNameWithPath(cssFileName);
            string output = GetFileNameWithOutputPath(cssFileName);
            string expected = GetFileNameWithExpectedPath(cssFileName);

            CopyToOutput(sourceCssFile, output);
            PreExportProcess preExport = new PreExportProcess();
            preExport.InsertCoverPageImageStyleInCSS(output);
            TextFileAssert.AreEqual(expected, output);
        }

		/// <summary>
		///A test for GetLanguageCode
		/// Checks whether the Language Code returned is as expected
		///</summary>
	    [Test]
	    public void GetLanguageCodeWithVernacularTest()
	    {
			const string fileName = "LanguageCodeTest.xhtml";
			var sourceXhtmlFile = GetFileNameWithPath(fileName);
			var outputXhtmlFile = Common.PathCombine(_outputBasePath, fileName);
			CopyToOutput(sourceXhtmlFile, outputXhtmlFile);
		    string theLanguageCode = Common.GetLanguageCode(outputXhtmlFile, "Dictionary", true);
			Assert.AreEqual(theLanguageCode, "bzh:Buang, Mapos;");
	    }

		/// <summary>
		///A test for GetLanguageCode - (1) With <span class="headword" lang="seh">
		/// Uses sena3
		/// Checks whether the Language Code returned is as expected
		///</summary>
		[Test]
		public void GetLanguageCodeTest1()
		{
			const string fileName = "LanguageCodeTest1.xhtml";
			var sourceXhtmlFile = GetFileNameWithPath(fileName);
			var outputXhtmlFile = Common.PathCombine(_outputBasePath, fileName);
			CopyToOutput(sourceXhtmlFile, outputXhtmlFile);
			string theLanguageCode = Common.GetLanguageCode(outputXhtmlFile, "Dictionary", true);
			Assert.AreEqual(theLanguageCode, "seh:Sena;");
		}

		/// <summary>
		///A test for GetLanguageCode - (2) With <span class="headword" lang="ggo-Telu-IN">
		/// Uses Gondwana Sample
		/// Checks whether the Language Code returned is as expected
		///</summary>
		[Test]
		public void GetLanguageCodeTest2()
		{
			const string fileName = "LanguageCodeTest2.xhtml";
			var sourceXhtmlFile = GetFileNameWithPath(fileName);
			var outputXhtmlFile = Common.PathCombine(_outputBasePath, fileName);
			CopyToOutput(sourceXhtmlFile, outputXhtmlFile);
			string theLanguageCode = Common.GetLanguageCode(outputXhtmlFile, "Dictionary", true);
			Assert.AreEqual(theLanguageCode, "ggo-Telu-IN:Gondi, Southern (Telugu, India);");
		}

		/// <summary>
		///A test for GetLanguageCode - (3) With <span class="mainheadword"><span lang="bzh">
		/// Checks whether the Language Code returned is as expected
		///</summary>
		[Test]
		public void GetLanguageCodeTest3()
		{
			const string fileName = "LanguageCodeTest3.xhtml";
			var sourceXhtmlFile = GetFileNameWithPath(fileName);
			var outputXhtmlFile = Common.PathCombine(_outputBasePath, fileName);
			CopyToOutput(sourceXhtmlFile, outputXhtmlFile);
			string theLanguageCode = Common.GetLanguageCode(outputXhtmlFile, "Dictionary", true);
			Assert.AreEqual(theLanguageCode, "bzh:Buang, Mapos;");
		}

		/// <summary>
		///A test for GetLanguageCode - (4) With <span class="mainheadword"><span xm:lang="bzh">
		/// Checks whether the Language Code returned is as expected
		///</summary>
		[Test]
		public void GetLanguageCodeTest4()
		{
			const string fileName = "LanguageCodeTest4.xhtml";
			var sourceXhtmlFile = GetFileNameWithPath(fileName);
			var outputXhtmlFile = Common.PathCombine(_outputBasePath, fileName);
			CopyToOutput(sourceXhtmlFile, outputXhtmlFile);
			string theLanguageCode = Common.GetLanguageCode(outputXhtmlFile, "Dictionary", true);
			Assert.AreEqual(theLanguageCode, "bzh:Buang, Mapos;");
		}

		/// <summary>
		///A test for GetLanguageCode (Without Vernacular)
		/// Checks whether the Language Code returned is as expected
		///</summary>
		[Test]
		public void GetLanguageCodeWithOutVernacularTest()
		{
			const string fileName = "LanguageCodeTest.xhtml";
			var sourceXhtmlFile = GetFileNameWithPath(fileName);
			var outputXhtmlFile = Common.PathCombine(_outputBasePath, fileName);
			CopyToOutput(sourceXhtmlFile, outputXhtmlFile);
			string theLanguageCode = Common.GetLanguageCode(outputXhtmlFile, "Dictionary", false);
			Assert.AreEqual(theLanguageCode, "en:English;tpi:Tok Pisin;bzh:Buang, Mapos;bzh-fonipa:Buang, Mapos;");
		}

        /// <summary>
        ///A test for XML settings file migration
        ///</summary>
        [Test]
        public void XmlMigrationTest()
        {
            string inputBackUpFile = Common.PathCombine(_inputBasePath, "BackUpFile.xml");
            string inputNnewFile = Common.PathCombine(_inputBasePath, "NewSettings.xml");

            string outputBackUpFile = Common.PathCombine(_outputBasePath, "BackUpFile.xml");
            string outputNewFile = Common.PathCombine(_outputBasePath, "NewSettings.xml");

            string expectedNewFile = Common.PathCombine(_expectBasePath, "NewSettings.xml");

            File.Copy(inputBackUpFile, outputBackUpFile, true);
            File.Copy(inputNnewFile, outputNewFile, true);

            Common.MigrateCustomSheet(outputBackUpFile, outputNewFile);

            FileAssert.AreEqual(expectedNewFile, outputNewFile, "Configuration Tool - Migration Test failed");
        }

		/// <summary>
		/// Test for the function UpdateLicenseAttributesTest
		/// </summary>
	    [Test]
	    public void UpdateLicenseAttributesTest()
	    {
			string creatorTool = "XeLaTex";
		    string destLicenseXml = "SIL_License.xml";
		    string organization = "SIL International";
			string exportTitle = "Gondwana Sample";
		    string copyrightURL = "http://creativecommons.org/licenses/by-nc-sa/4.0/";

		    string testFolderName = "UpdateLicenseAttributesTest";

		    string inputDataFolder = Common.PathCombine(_inputBasePath, testFolderName);
			string outputDataFolder = Common.PathCombine(_outputBasePath, testFolderName);
			string expectedDataFolder = Common.PathCombine(_expectBasePath, testFolderName);

			string expectedFile = Common.PathCombine(expectedDataFolder, destLicenseXml);
		    string outputFile = Common.PathCombine(outputDataFolder, destLicenseXml);

			Common.CopyFolderandSubFolder(inputDataFolder, outputDataFolder, true);

			string utcDateTime = DateTimeOffset.UtcNow.ToString("o");
			Common.UpdateLicenseAttributes(creatorTool, "Dictionary", outputFile, organization, exportTitle, copyrightURL, utcDateTime);

			TextFileAssert.CheckLineAreEqualEx(expectedFile, outputFile, new ArrayList { 6, 19, 45, 50});

			string fileData = FileData.Get(outputFile);

			StringAssert.Contains(utcDateTime, fileData);

	    }

        #endregion

        #region SaveInFolderTests
        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void GetSaveInFolderTest()
        {
			if(Common.UsingMonoVM)
				return;
            string template = "$(Documents)s/$(Base)s/$(CurrentProject)s/Dictionary/$(StyleSheet)s_$(DateTime)s";
            string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string database = "{Current_Project}";
            string layout = "Quick";
            string dateTime = DateTime.Now.ToString("yyyy-MM-dd_HHmmss");
            string expected = Common.PathCombine(ManageDirectory.ShortFileName(documents), @"Publications\{Current_Project}\Dictionary\Quick_" + dateTime);
            string actual = Common.GetSaveInFolder(template, database, layout);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CustomSavedInFolderTest()
        {
            string dateTime = DateTime.Now.ToString("yyyy-MM-dd_hhmmss");
            bool actual = Common.CustomSaveInFolder(dateTime);
            Assert.AreEqual(false, actual);
        }
        #endregion GetSaveInFolderTest

        #region private Methods
        private XmlNode LoadXmlDocument(string node, bool rootAdd)
        {
            XmlNode xmlNode;
            if (rootAdd)
            {
                _doc.LoadXml("<root>" + node + "</root>");
            }
            else
            {
                _doc.LoadXml(node);
            }
            xmlNode = _doc.DocumentElement;
            return xmlNode;
        }
        private void LoadInputDocument(string fileName)
        {
            _projectFilePath = GetFileNameWithPath(fileName);
            actualDocument.Load(_projectFilePath);
            _target.ProjectDeXML = actualDocument;
        }
        private static string GetPath(string place,string filename)
        {
            return Common.PathCombine(GetTestPath(), Common.PathCombine(place, filename));
        }
        private static string GetTestPath()
        {
            return PathPart.Bin(Environment.CurrentDirectory, "/PsTool/TestFiles/");
        }
        private static string GetFileNameWithPath(string fileName)
        {
            return Common.DirectoryPathReplace(GetPath("InputFiles", fileName));
        }
        private static string GetFileNameWithOutputPath(string fileName)
        {
            return Common.DirectoryPathReplace(GetPath("Output", fileName));
        }
        private static string GetFileNameWithExpectedPath(string fileName)
        {
            return Common.DirectoryPathReplace(GetPath("Expected", fileName));
        }
        private static void CopyToOutput(string input, string output)
        {
            if (File.Exists(input))
                File.Copy(input, output, true);
        }
        #endregion
    }
}
