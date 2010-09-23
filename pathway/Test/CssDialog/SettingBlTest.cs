using System;
using System.IO;
using NUnit.Framework;
using SIL.PublishingSolution;
using System.Windows.Forms;
using System.Collections.Generic;
using SIL.Tool;
using Assert=NUnit.Framework.Assert;

namespace Test.CssDialog
{

    public class MyException : Exception
    {
        public string FullFilePath { get; set; }
        public override string ToString()
        {
            return "{0} is not a valid settings file";
        }
    }
    
    /// <summary>
    ///This is a test class for SettingBlTest and is intended
    ///to contain all SettingBlTest Unit Tests
    ///</summary>
    [TestFixture]
    public class SettingBlTest
    {
        private string _methodName;
        private string _currentFolder;
        private SettingBl target;
        ListView inputLstView = new ListView();
        ListView expectedLstView = new ListView();
        ListBox inputLstBox = new ListBox();
        ListBox expectedLstBox = new ListBox();
        /// <summary>holds path to input folder for all tests</summary>
        string _inputBasePath = string.Empty;
        /// <summary>holds path to expect folder for all tests</summary>
        string _expectBasePath = string.Empty;
        /// <summary>
        /// Turn off debug.assert messages for unit tests
        /// </summary>
        [TestFixtureSetUp]
        protected void SetUp()
        {
            Common.Testing = true;
            target = new SettingBl();
            _currentFolder = Common.PathCombine(Environment.CurrentDirectory, "../../CssDialog/TestFiles");
            _inputBasePath = Common.PathCombine(_currentFolder, "Input");
            _expectBasePath = Common.PathCombine(_currentFolder, "Expected");
        }       

        
        /// <summary>
        ///A test for ValidateSpaceSelector
        ///</summary>
        [Test]

        public void ValidateSpaceSelectorTest()
        {
            _methodName = "ValidateSpaceSelectorTest";
            Control ctl = new Control();
            ctl.Text = "12pt";
            const string spaceValue = "10pt";
            const string suffix = "After";
            const string expected = "Space After cannot be less than FontSize";
            string actual = target.ValidateSpaceSelector(ctl, spaceValue, suffix);
            Assert.AreEqual(expected, actual, _methodName + " test failed");
        }

        /// <summary>
        ///A test for ValidateLineSpacing
        ///</summary>
        [Test]

        public void ValidateLineSpacingTest1()
        {
            _methodName = "ValidateLineSpacingTest1";
            Control ctrl = new Control();
            TextBox fontSize = new TextBox();
            ctrl.Text = "25pt";
            fontSize.Text = "12pt";
            const string expected = "Letter Spacing cannot be greater or equal to FontSize * 2";
            string actual = target.ValidateLineSpacing(ctrl, fontSize);
            Assert.AreEqual(expected, actual, _methodName + " test failed");
        }

        /// <summary>
        ///A test for ValidateLineSpacing
        ///</summary>
        [Test]

        public void ValidateLineSpacingTest2()
        {
            _methodName = "ValidateLineSpacingTest2";
            Control ctrl = new Control();
            TextBox fontSize = new TextBox();
            ctrl.Text = "10pt";
            fontSize.Text = "12pt";
            const string expected = "Letter Spacing cannot be smaller than FontSize";
            string actual = target.ValidateLineSpacing(ctrl, fontSize);
            Assert.AreEqual(expected, actual, _methodName + " test failed");
        }

        /// <summary>
        ///A test for SwapListView
        ///</summary>
        [Test]

        public void SwapListItemTest()
        {
            const string dir = "Dn";
            _methodName = "SwapListItemTest";
            inputLstBox.Items.Add("Test1");
            inputLstBox.Items.Add("Test2");
            inputLstBox.Items.Add("Test3");
            inputLstBox.SetSelected(0,true);

            expectedLstBox.Items.Add("Test2");
            expectedLstBox.Items.Add("Test1");
            expectedLstBox.Items.Add("Test3");
            expectedLstBox.SetSelected(1,true);
            target.SwapListItem(inputLstBox, dir);
            Assert.AreEqual(expectedLstBox.Items[1].ToString(), inputLstBox.Items[1].ToString(), _methodName + " test failed");
        }

        /// <summary>
        ///A test for SwapListView
        ///</summary>
        [Test]

        public void SwapListViewTest()
        {
            _methodName = "SwapListViewTest";
            inputLstView.Items.Add("Test1");
            inputLstView.Items.Add("Test2");
            inputLstView.Items.Add("Test3");
            inputLstView.Items[0].Selected = true;

            expectedLstView.Items.Add("Test2");
            expectedLstView.Items.Add("Test1");
            expectedLstView.Items.Add("Test3");

            const string dir = "DOWN";
            target.SwapListView(inputLstView, dir);
            Assert.IsTrue(inputLstView.Items[0].Text == expectedLstView.Items[0].Text, _methodName + " test failed");
        }

        /// <summary>
        ///A test for SetPageandPaperSetting
        ///</summary>
        [Test]

        public void SetPageandPaperSettingTest1()
        {
            _methodName = "SetPageandPaperSettingTest1";
            ComboBox pageSize = new ComboBox();
            ComboBox paperSize = new ComboBox();
            target.SetPageandPaperSetting(pageSize, paperSize);
            Assert.IsTrue(pageSize.Items.Count > 0 && paperSize.Items.Count > 0, _methodName + " test failed");
        }

        /// <summary>
        ///A test for SetPageandPaperSetting
        ///</summary>
        [Test]

        public void SetPageandPaperSettingTest2()
        {
            _methodName = "SetPageandPaperSettingTest2";
            ComboBox pageSize = new ComboBox();
            ComboBox paperSize = new ComboBox();
            target._locUser = "US";
            target.SetPageandPaperSetting(pageSize, paperSize);
            Assert.IsTrue(pageSize.SelectedItem == "Letter" && paperSize.SelectedItem == "Letter", _methodName + " test failed");
        }

        /// <summary>
        ///A test for SetPageandPaperSetting
        ///</summary>
        [Test]

        public void SetPageandPaperSettingTest3()
        {
            _methodName = "SetPageandPaperSettingTest3";
            ComboBox pageSize = new ComboBox();
            ComboBox paperSize = new ComboBox();
            target._locUser = "UK";
            target.SetPageandPaperSetting(pageSize, paperSize);
            Assert.IsTrue(pageSize.SelectedItem == "A4" && paperSize.SelectedItem == "A4", _methodName + " test failed");
        }

        /// <summary>
        ///A test for SetFontStyle
        ///</summary>
        [Test]

        public void SetFontStyleTest()
        {
            _methodName = "SetFontStyleTest";
            ComboBox cmbCtl = new ComboBox();
            const sbyte selectedIndex = 1;
            target.SetFontStyle(cmbCtl, selectedIndex);
            Assert.IsTrue(cmbCtl.Items.Count > 0 && cmbCtl.SelectedItem.ToString() == "Italic", _methodName + " test failed");
        }

        /// <summary>
        ///A test for SetFontName
        ///</summary>
        [Test]

        public void SetFontNameTest1()
        {
            _methodName = "SetFontNameTest1";
            ComboBox cmbCtl = new ComboBox();
            const string fontname = "Arial";
            target.SetFontName(cmbCtl, fontname);
            Assert.IsTrue(cmbCtl.Items.Count > 0 && cmbCtl.SelectedItem.ToString() == "Arial", _methodName + " test failed");
        }

        /// <summary>
        ///A test for SetFontName
        ///</summary>
        [Test]

        public void SetFontNameTest2()
        {
            _methodName = "SetFontNameTest2";
            ComboBox cmbCtl = new ComboBox();
            const string fontname = "Test";
            target.SetFontName(cmbCtl, fontname);
            Assert.IsTrue(cmbCtl.Items.Count > 0 && cmbCtl.SelectedIndex == 0, _methodName + " test failed");
        }

        /// <summary>
        ///A test for SetColumnCount
        ///</summary>
        [Test]

        public void SetColumnCountTest()
        {
            _methodName = "SetColumnCountTest";
            ComboBox cmbCtl = new ComboBox();
            target.SetColumnCount(cmbCtl);
            Assert.IsTrue(cmbCtl.Items.Count > 0 && cmbCtl.SelectedItem.ToString() == "1", _methodName + " test failed");
        }

        /// <summary>
        ///A test for RelativeValidation
        ///</summary>
        [Test]

        public void RelativeValidationTest1()
        {
            _methodName = "RelativeValidationTest1";
            Control ctl = new Control(); 
            const float value = 1.10F;
            ctl.Text = "2pt";
            const string expected = "2pt";
            string actual = target.RelativeValidation(ctl, value);
            Assert.AreEqual(expected, actual, _methodName + " test failed");
        }

        /// <summary>
        ///A test for RelativeValidation
        ///</summary>
        [Test]

        public void RelativeValidationTest2()
        {
            _methodName = "RelativeValidationTest2";
            Control ctl = new Control(); 
            const float value = 1.10F; 
            ctl.Text = "2";
            const string expected = "2pt";
            string actual = target.RelativeValidation(ctl, value);
            Assert.AreEqual(expected, actual, _methodName + " test failed");
        }

        /// <summary>
        ///A test for RelativeValidation
        ///</summary>
        [Test]

        public void RelativeValidationTest3()
        {
            _methodName = "RelativeValidationTest3";
            Control ctl = new Control(); 
            const float value = 1.10F;
            ctl.Text = "pt";
            const string expected = "0pt";
            string actual = target.RelativeValidation(ctl, value);
            Assert.AreEqual(expected, actual, _methodName + " test failed");
        }

        /// <summary>
        ///A test for RangeValidate
        ///</summary>
        [Test]

        public void RangeValidateTest1()
        {
            _methodName = "RangeValidateTest1";
            const string controlValue = "2pt";
            const string minValue = "1pt";
            const string maxValue = "3pt";
            string expected = string.Empty;
            string actual = target.RangeValidate(controlValue, minValue, maxValue);
            Assert.AreEqual(expected, actual, _methodName + " test failed");
        }

        /// <summary>
        ///A test for RangeValidate
        ///</summary>
        [Test]

        public void RangeValidateTest2()
        {
            _methodName = "RangeValidateTest2";
            const string controlValue = "2pt";
            const string minValue = "3pt";
            const string maxValue = "5pt";
            const string expected = "Enter a value between 3pt to 5pt inclusive.";
            string actual = target.RangeValidate(controlValue, minValue, maxValue);
            Assert.AreEqual(expected, actual, _methodName + " test failed");
        }

        /// <summary>
        ///A test for RangeValidate
        ///</summary>
        [Test]

        public void RangeValidateTest3()
        {
            _methodName = "RangeValidateTest3";
            const string controlValue = "123";
            const string minValue = "1pt";
            const string maxValue = "3pt";
            const string expected = "Please enter the valid Input";
            string actual = target.RangeValidate(controlValue, minValue, maxValue);
            Assert.AreEqual(expected, actual, _methodName + " test failed");
        }

        /// <summary>
        ///A test for LoadHyphenationLanguages
        ///</summary>
        [Test]

        public void LoadHyphenationLanguagesTest()
        {
            string appFolderPath = Common.LeftString(_currentFolder, "PublishingSolution");
            _methodName = "LoadHyphenationLanguagesTest";
            ComboBox cmbCtl = new ComboBox();
            target._hyphenationPath = Common.PathCombine(appFolderPath,"PublishingSolution/PsSupport/Hyphenation_Languages");
            target.LoadHyphenationLanguages(cmbCtl);
            Assert.IsTrue(cmbCtl.Items.Count > 0, _methodName + " test failed");
        }

        /// <summary>
        ///A test for GetNewFileName
        ///</summary>
        [Test]

        public void GetNewFileNameTest1()
        {
            _methodName = "GetNewFileNameTest1";
            string fileName = Common.PathCombine(_inputBasePath, "GetNewFileNameTest1.css"); ;
            const string suffix = "job";
            string path = _expectBasePath;
            const string onChange = "onsave";
            string expected = Common.PathCombine(path, "GetNewFileNameTest1-job1.css");
            string actual = SettingBl.GetNewFileName(fileName, suffix, path, onChange);
            Assert.AreEqual(expected, actual, _methodName + " test failed");
        }

        /// <summary>
        ///A test for ConcateUnit
        ///</summary>
        [Test]

        public void ConcateUnitTest1()
        {
            _methodName = "ConcateUnitTest1";
            TextBox txtBox = new TextBox();
            txtBox.Text = "12pt";
            const string expected = "12pt";
            string actual = target.ConcateUnit(txtBox);
            Assert.AreEqual(expected, actual, _methodName + " test failed");
        }

        /// <summary>
        ///A test for ConcateUnit
        ///</summary>
        [Test]

        public void ConcateUnitTest2()
        {
            _methodName = "ConcateUnitTest2";
            TextBox txtBox = new TextBox();
            txtBox.Text = "12";
            const string expected = "12pt";
            string actual = target.ConcateUnit(txtBox);
            Assert.AreEqual(expected, actual, _methodName + " test failed");
        }

        /// <summary>
        ///A test for ConcateUnit
        ///</summary>
        [Test]

        public void ConcateUnitTest3()
        {
            _methodName = "ConcateUnitTest3";
            TextBox txtBox = new TextBox();
            txtBox.Text = "";
            const string expected = "";
            string actual = target.ConcateUnit(txtBox);
            Assert.AreEqual(expected, actual, _methodName + " test failed");
        }
    }
}
