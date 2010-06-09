// --------------------------------------------------------------------------------------------
// <copyright file="ContentXMLTest.cs" from='2009' to='2009' company='SIL International'>
//      Copyright © 2009, SIL International. All Rights Reserved.   
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
using System.IO;
using NUnit.Framework;
using System.Windows.Forms;
using SIL.PublishingSolution;
using SIL.Tool;

#endregion Using

namespace Test.OpenOfficeConvert
{
    [TestFixture]
    [Category("BatchTest")]
    public class ContentXMLTest
    {
        #region Private Variables
        Styles _styleName;
        Utility _util;
        string _errorFile;
        private string _inputPath;
        private string _outputPath;
        private string _expectedPath;
        ProgressBar _progressBar;
        private TimeSpan _totalTime;
        private PublicationInformation _projInfo;
        #endregion Private Variables

        #region SetUp

        public Utility M_util
        {
            get { return _util; }
        }

        [TestFixtureSetUp]
        protected void SetUp()
        {
            Common.Testing = true;
            _styleName = new Styles();
            _util = new Utility();
            _projInfo = new PublicationInformation();
            _errorFile = Common.PathCombine(Path.GetTempPath(), "temp.odt");
            _progressBar = new ProgressBar();
            string testPath = PathPart.Bin(Environment.CurrentDirectory, "/OpenOfficeConvert/TestFiles");
            _inputPath = Common.PathCombine(testPath, "input");
            _outputPath = Common.PathCombine(testPath, "output");
            _expectedPath = Common.PathCombine(testPath, "expected");
            _projInfo.ProgressBar = _progressBar;
            _projInfo.OutputExtension = "odm";
            _projInfo.ProjectInputType = "Dictionary";
            Common.SupportFolder = "";
            Common.ProgInstall = Common.DirectoryPathReplace(Environment.CurrentDirectory + "/../../../PsSupport");
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

        private string FileExpected(string fileName)
        {
            return Common.PathCombine(_expectedPath, fileName);
        }
        #endregion PrivateFunctions

        #region Public Functions
        ///<summary>
        ///Nested Div Test
        /// <summary>
        /// </summary>      
        [Test]
        public void NestedDivCase1()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "NestedDivCase1";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);

            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        [Test]
        public void NestedDivCase2()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "NestedDivCase2";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }
        [Test]
        public void NestedDivCase3()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "NestedDivCase3";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        [Test]
        public void NestedDivCase4()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "NestedDivCase4";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        ///em and % test
        /// <summary>
        /// </summary>      
        [Test]
        public void EMTest1()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "EMTest1";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }
        [Test]
        public void EMTest2()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";

            const string file = "EMTest2";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }
        [Test]
        public void EMTest3()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "EMTest3";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }
        ///<summary>
        /// TD-90 .locator .letter
        /// <summary>
        /// </summary>      
        [Test]
        public void AncestorChildTest()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "AncestorChildTest";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }
        ///<summary>
        /// TD-91 .letter[class~='current']
        /// <summary>
        /// </summary>      
        [Test]
        public void ClassNameValueTest()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "ClassNameValueTest";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }
        ///<summary>
        /// TD-92 .pronunciation + .pronunciation[lang=en_UK]::before
        /// <summary>
        /// </summary>      
        [Test]
        public void PrecedesPseudoLangTest()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "PrecedesPseudoLangTest";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        /// TD-148 .pronunciation + .pronunciation::before
        /// <summary>
        /// </summary>      
        [Test]
        public void PrecedesPseudoTestA()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "PrecedesPseudoTestA";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        /// TD-83. Open Office column-gap: 1.5em;  
        /// <summary>
        /// </summary>      
        [Test]
        public void ColumnGap()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "ColumnGap";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        /// TD-170 Open Office: triangle should appear before all senses
        /// TD-171 Open Office: sense # is wrong font size
        /// TD-172 open office: visibility  
        /// <summary>
        /// </summary>      
        ///[Test]
        public void Visibility()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "VisibilityTest";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        ///TD81 line-height: always syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void LineHeight()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "LineHeight";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        ///TD-181 White-space:pre;
        /// <summary>
        /// </summary>      
        [Test]
        public void Whitespace()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "Whitespace";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        ///TD130 (Remove AutoWidth and set Column Width for columns)
        /// <summary>
        /// </summary>      
        [Test]
        public void RemoveAutoWidth()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "RemoveAutoWidth";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }
        ///<summary>
        ///TD130 (Remove AutoWidth and set Column Width for columns)
        /// <summary>
        /// </summary>      
        [Test]
        public void InlineBlock()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "InlineBlock";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        ///Test pink and blue in output
        /// <summary>
        /// </summary>      
        [Test]
        public void LanguageColor()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "LanguageColor";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        ///FullBuangTest
        /// <summary>
        /// </summary>      
        [Test]
        [Category("LongTest")]
        public void BuangExport()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "BuangExport";
            DateTime startTime = DateTime.Now;

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");
            _totalTime = DateTime.Now - startTime;

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        /// Compare the processing time
        /// <summary>
        /// </summary>      
        [Test]
        [Category("LongTest")]
        public void BuangExportPerformanceTest()
        {
            if (_totalTime.Milliseconds == 0)
            {
                BuangExport();
            }

            var expectedTime = new TimeSpan(0, 0, 14);
            Assert.Greater(expectedTime, _totalTime, "Performance Affected");
        }

        ///<summary>
        ///FullBuangTest
        /// <summary>
        /// </summary>      
        [Test]
        [Category("LongTest")]
        public void BuangX()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "BuangX";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        /// TD98 (Clear:both)
        /// <summary>
        /// </summary>      
        ///[Test]
        public void ClearTest()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "ClearTest";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        /// TD207 (Pseudo: before)
        /// <summary>
        /// </summary>      
        [Test]
        public void PseudoAfter()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "PseudoAfter";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        /// TD-149 (Open Office @page)
        /// <summary>
        /// </summary>      
        [Test]
        public void PageHeaderFooterTestA()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "PageHeaderFooterTestA";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }
        [Test]
        public void PageHeaderFooterTestB()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "PageHeaderFooterTestB";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }
        [Test]
        public void PageHeaderFooterTestC()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "PageHeaderFooterTestC";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        /// TD-59  font-size: larger; and font-size: smaller;
        /// <summary>
        /// </summary>      
        [Test]
        public void Larger()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "Larger";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        /// TD-204  unable to put / before Tok Pisin;
        /// <summary>
        /// </summary>      
        [Test]
        public void PrecedesPseudoTestB()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "PrecedesPseudoTestB";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        /// TD-222   Image width:50%
        /// <summary>
        /// </summary>      
        [Test]
        public void Picture_Width()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "PictureWidth";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        /// TD-227   set language for data. 
        /// <summary>
        /// </summary>      
        [Test]
        public void Language()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "Language";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        /// TD-204   unable to put tok / pisin. 
        /// <summary>
        /// </summary>      
        [Test]
        public void ClassContent()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "ClassContent";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, "ClassContent in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, "ClassContent Failed");
        }
        ///<summary>
        /// TD-304   Open Office div.header
        /// <summary>
        /// </summary>      
        [Test]
        public void div_header()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "div_header";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, "div_header in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, "div_header Failed");
        }

        ///<summary>
        /// TD-347  Implement content: normal;  
        /// <summary>
        /// </summary>      
        [Test]
        public void ContentNormalTest()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "ContentNormalTest";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, "ContentNormalTest Failed in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, "ContentNormalTest Failed in Content.xml");
        }

        ///<summary>
        /// TD-341 Implement keywords related to footnotes
        /// <summary>
        /// </summary>      
        [Test]
        public void FootnoteTest()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "Footnote";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, "FootnoteTest Failed in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, "FootnoteTest Failed in Content.xml");
        }

        ///<summary>
        /// TD-343 Implement lists
        /// <summary>
        /// </summary>      
        [Test]
        public void ListOlUl()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "ListOlUl";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, "ListOlUl Failed in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, "ListOlUl Failed in Content.xml");
        }

        ///<summary>
        /// TD-362 Image Attribute
        /// <example img[src='Thomsons-gazelle1.jpg'] { float:left;}>
        /// <summary>
        /// </summary>      
        [Test]
        public void Picture_ImageAttrib()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "ImageAttrib";

            //StyleXML
            string inputcss = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(inputcss, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            string inputxhtml = FileInput(file + ".xhtml");
            PublicationInformation projInfo = new PublicationInformation();
            projInfo.DefaultXhtmlFileWithPath = inputxhtml;
            projInfo.DefaultCssFileWithPath = inputcss;

            PreExportProcess preProcessor = new PreExportProcess(projInfo);
            preProcessor.GetTempFolderPath();
            preProcessor.ImagePreprocess();
            _projInfo.DefaultXhtmlFileWithPath = preProcessor.ProcessedXhtml;
            _projInfo.TempOutputFolder = FileOutput(file);
            //contentXML.CreateContent(m_pb, input, contentOutput,styleName, "odm");
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, "ImageAttrib in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, "ImageAttrib in Content.xml");
        }

        ///<summary>
        /// TD-416 chapter number should align with top of text not with bottom
        /// <summary>
        /// </summary>      
        [Test]
        public void DropCap()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML();
            const string file = "DropCap";
            _projInfo.ProjectInputType = "Scripture";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        /// TD-429 -  Handling Anchor Tag 
        /// <summary>
        /// </summary>      
        [Test]
        public void AnchorTag()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML();
            const string file = "AnchorTag";
            _projInfo.ProjectInputType = "Scripture";


            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        /// TD-444 -  Support new footnote export format
        /// <summary>
        /// </summary>      
        [Test]
        public void FootNoteFormatedTest()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML();
            const string file = "FootNoteFormat";
            _projInfo.ProjectInputType = "Scripture";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        /// TD-453 - Index doesn't have three columns in this data
        /// <summary>
        /// </summary>      
        [Test]
        public void ColumnCountTest()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "ColumnCount";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        /// TD-205 - Open Office embedded spans aren't formatted according to css
        /// <summary>
        /// </summary>      
        [Test]
        public void SpanStyleTest()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "spanstyle";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        /// TD-458 - Index sense divider spacing
        /// <summary>
        /// </summary>      
        [Test]
        public void SpaceBeforeTest()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "spacebefore";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        /// TD-479 -  create property to substitute quote characters
        /// <summary>
        /// </summary>      
        [Test]
        public void ReplacePrinceQuoteTest()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "ReplacePrinceQuote";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        /// TD-518 -  divider line missing after introduction
        /// <summary>
        /// </summary>      
        [Test]
        public void EmptyDivTag()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "EmptyDivTag";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        /// TD-654 
        /// <summary>
        /// </summary>      
        [Test]
        public void VerseNumber()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML(); 
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "VerseNumber";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        /// TD-349 -  width: auto
        /// <summary>
        /// </summary>      
        [Test]
        public void AutoWidth()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML();
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "AutoWidth";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        ///<summary>
        /// TD-1239 -  Two column output from TE can conflict with two column stylesheets, not wrap correctly at section breaks.
        /// <summary>
        /// </summary>      
        [Test]
        public void RemoveScrSectionClass()
        {
            var contentXML = new ContentXML();
            var stylesXML = new StylesXML();
            _projInfo.ProjectInputType = "Scripture";
            const string file = "RemoveScrSectionClass";

            //StyleXML
            string input = FileInput(file + ".css");
            string styleOutput = FileOutput(file + "Styles.xml");
            _styleName = stylesXML.CreateStyles(input, styleOutput, _errorFile, true);
            string styleExpected = FileExpected(file + "styles.xml");

            // ContentXML
            _projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            _projInfo.TempOutputFolder = FileOutput(file);
            contentXML.CreateContent(_projInfo, _styleName);
            _projInfo.TempOutputFolder = _projInfo.TempOutputFolder + "Content.xml";
            string contentExpected = FileExpected(file + "Content.xml");

            XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }
        #endregion Public Functions
    }
}