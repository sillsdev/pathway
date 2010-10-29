// --------------------------------------------------------------------------------------------
// <copyright file="MapOfficeAttributeTest.cs" from='2009' to='2009' company='SIL International'>
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
// Map Office Attribute Test
// </remarks>
// --------------------------------------------------------------------------------------------

#region Using

using System;
using System.Collections.Generic;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

#endregion Using

namespace Test.OpenOfficeWriter
{
    
    [Category("BatchTest")]
    public class OOMapPropertyTest
    {
        #region Private Variables
        MapProperty _mapOfficeAttribute;
        Dictionary<string, string> _dicAttributeInformation;
        Dictionary<string, string> _dicAttributeInfo;
        StyleAttribute _attributeInfo;
        #endregion Private Variables

        #region Setup
        [TestFixtureSetUp]
        protected void SetUp()
        {
            _attributeInfo = new StyleAttribute();
            _mapOfficeAttribute = new MapProperty();
            _dicAttributeInfo = new Dictionary<string, string>();
            Common.SupportFolder = "";
            Common.ProgInstall = Common.DirectoryPathReplace(Environment.CurrentDirectory + "/../../../PsSupport");
        }
        #endregion Setup

        #region Public Functions
        /// <summary>
        /// Testing "margin" attribute with 4 parameters
        /// </summary>
        [Test]
        public void MapNonClassMargin4Param()
        {
            _attributeInfo.Name = "margin";
            _attributeInfo.StringValue = "1,in,2,in,3,in,4,in";
            _dicAttributeInformation = _mapOfficeAttribute.MapMultipleValue(_attributeInfo);
            /// ENHANCE Test from css file
            _dicAttributeInfo.Clear();
            _dicAttributeInfo.Add("margin-top", "1in");
            _dicAttributeInfo.Add("margin-right", "2in");
            _dicAttributeInfo.Add("margin-bottom", "3in");
            _dicAttributeInfo.Add("margin-left", "4in");
            bool compare = true;
            foreach (KeyValuePair<string, string> dicData in _dicAttributeInfo)
            {
                if (dicData.Value != _dicAttributeInformation[dicData.Key])
                {
                    compare = false;
                    break;
                }
            }
            Assert.IsTrue(compare, "margin 4 parameters test Failed");
        }

        /// <summary>
        /// Testing "margin" attribute with 3 parameters
        /// </summary>
        [Test]
        public void MapNonClassMargin3Param()
        {
            _attributeInfo.Name = "margin";
            _attributeInfo.StringValue = "1,in,2,in,3,in";
            _dicAttributeInformation = _mapOfficeAttribute.MapMultipleValue(_attributeInfo);

            _dicAttributeInfo.Clear();
            _dicAttributeInfo.Add("margin-top", "1in");
            _dicAttributeInfo.Add("margin-right", "2in");
            _dicAttributeInfo.Add("margin-bottom", "3in");
            _dicAttributeInfo.Add("margin-left", "2in");
            bool compare = true;
            foreach (KeyValuePair<string, string> dicData in _dicAttributeInfo)
            {
                if (dicData.Value != _dicAttributeInfo[dicData.Key])
                {
                    compare = false;
                    break;
                }
            }
            Assert.IsTrue(compare, "margin 3 parameters test Failed");
        }

        /// <summary>
        /// Testing "margin" attribute with 2 parameters
        /// </summary>
        [Test]
        public void MapNonClassMargin2Param()
        {
            _attributeInfo.Name = "margin";
            _attributeInfo.StringValue = "1,in,2,in";
            _dicAttributeInformation = _mapOfficeAttribute.MapMultipleValue(_attributeInfo);
            /// ENHANCE Test from css file
            _dicAttributeInfo.Clear();
            _dicAttributeInfo.Add("margin-top", "1in");
            _dicAttributeInfo.Add("margin-right", "2in");
            _dicAttributeInfo.Add("margin-bottom", "1in");
            _dicAttributeInfo.Add("margin-left", "2in");
            bool compare = true;
            foreach (KeyValuePair<string, string> dicData in _dicAttributeInfo)
            {
                if (dicData.Value != _dicAttributeInformation[dicData.Key])
                {
                    compare = false;
                    break;
                }
            }
            Assert.IsTrue(compare, "margin 2 parameters test Failed");
        }

        /// <summary>
        /// Testing "margin" attribute with 1 parameters
        /// </summary>
        [Test]
        public void MapNonClassMargin1Param()
        {
            _attributeInfo.Name = "margin";
            _attributeInfo.StringValue = "1,in";
            _dicAttributeInformation = _mapOfficeAttribute.MapMultipleValue(_attributeInfo);
            /// ENHANCE Test from css file
            _dicAttributeInfo.Clear(); 
            _dicAttributeInfo.Add("margin-top", "1in");
            _dicAttributeInfo.Add("margin-right", "1in");
            _dicAttributeInfo.Add("margin-bottom", "1in");
            _dicAttributeInfo.Add("margin-left", "1in");
            bool compare = true;
            foreach (KeyValuePair<string, string> dicData in _dicAttributeInfo)
            {
                if (dicData.Value != _dicAttributeInfo[dicData.Key])
                {
                    compare = false;
                    break;
                }
            }
            Assert.IsTrue(compare, "margin 1 parameters test Failed");           
        }

        /// <summary>
        /// Testing "margin" attribute with 2 parameters with swapping input parameters.
        /// </summary>
        [Test]
        public void MapNonClassMargin2ParamSwap()
        {
            _attributeInfo.Name = "margin";
            _attributeInfo.StringValue = "2,in,1,in";
            _dicAttributeInformation = _mapOfficeAttribute.MapMultipleValue(_attributeInfo);
            /// ENHANCE Test from css file
            _dicAttributeInfo.Clear();
            _dicAttributeInfo.Add("margin-top", "1in");
            _dicAttributeInfo.Add("margin-right", "2in");
            _dicAttributeInfo.Add("margin-bottom", "1in");
            _dicAttributeInfo.Add("margin-left", "2in");
            bool compare = true;
            foreach (KeyValuePair<string, string> dicData in _dicAttributeInfo)
            {
                if (dicData.Value != _dicAttributeInformation[dicData.Key])
                {
                    compare = false;
                    break;
                }
            }
            Assert.IsFalse(compare, "margin 2 parameters swap test Failed");
        }

        //////////////////////////////KY
        /// <summary>
        /// Testing "padding" attribute with 4 parameters
        /// </summary>
        [Test]
        public void MapNonClassPadding4Param()
        {
            _attributeInfo.Name = "padding";
            _attributeInfo.StringValue = "1,in,2,in,3,in,4,in";
            _dicAttributeInformation = _mapOfficeAttribute.MapMultipleValue(_attributeInfo);
            /// ENHANCE Test from css file
            _dicAttributeInfo.Clear();
            _dicAttributeInfo.Add("padding-top", "1in");
            _dicAttributeInfo.Add("padding-right", "2in");
            _dicAttributeInfo.Add("padding-bottom", "3in");
            _dicAttributeInfo.Add("padding-left", "4in");
            bool compare = true;
            foreach (KeyValuePair<string, string> dicData in _dicAttributeInfo)
            {
                if (dicData.Value != _dicAttributeInformation[dicData.Key])
                {
                    compare = false;
                    break;
                }
            }
            Assert.IsTrue(compare, "padding 4 parameters test Failed");
        }

        /// <summary>
        /// Testing "padding" attribute with 3 parameters
        /// </summary>
        [Test]
        public void MapNonClassPadding3Param()
        {
            _attributeInfo.Name = "padding";
            _attributeInfo.StringValue = "1,in,2,in,3,in";
            _dicAttributeInformation = _mapOfficeAttribute.MapMultipleValue(_attributeInfo);
            /// ENHANCE Test from css file
            _dicAttributeInfo.Clear();
            _dicAttributeInfo.Add("padding-top", "1in");
            _dicAttributeInfo.Add("padding-right", "2in");
            _dicAttributeInfo.Add("padding-bottom", "3in");
            _dicAttributeInfo.Add("padding-left", "2in");
            bool compare = true;
            foreach (KeyValuePair<string, string> dicData in _dicAttributeInfo)
            {
                if (dicData.Value != _dicAttributeInformation[dicData.Key])
                {
                    compare = false;
                    break;
                }
            }
            Assert.IsTrue(compare, "padding 3 parameters test Failed");
        }

        /// <summary>
        /// Testing "padding" attribute with 2 parameters
        /// </summary>
        [Test]
        public void MapNonClassPadding2Param()
        {
            _attributeInfo.Name = "padding";
            _attributeInfo.StringValue = "1,in,2,in";
            _dicAttributeInformation = _mapOfficeAttribute.MapMultipleValue(_attributeInfo);
            /// ENHANCE Test from css file
            _dicAttributeInfo.Clear();
            _dicAttributeInfo.Add("padding-top", "1in");
            _dicAttributeInfo.Add("padding-right", "2in");
            _dicAttributeInfo.Add("padding-bottom", "1in");
            _dicAttributeInfo.Add("padding-left", "2in");
            bool compare = true;
            foreach (KeyValuePair<string, string> dicData in _dicAttributeInfo)
            {
                if (dicData.Value != _dicAttributeInformation[dicData.Key])
                {
                    compare = false;
                    break;
                }
            }
            Assert.IsTrue(compare, "padding 2 parameters test Failed");
        }

        /// <summary>
        /// Testing "padding" attribute with 1 parameters
        /// </summary>
        [Test]
        public void MapNonClassPadding1Param()
        {
            _attributeInfo.Name = "padding";
            _attributeInfo.StringValue = "1,in";
            _dicAttributeInformation = _mapOfficeAttribute.MapMultipleValue(_attributeInfo);
            /// ENHANCE Test from css file
            _dicAttributeInfo.Clear();
            _dicAttributeInfo.Add("padding-top", "1in");
            _dicAttributeInfo.Add("padding-right", "1in");
            _dicAttributeInfo.Add("padding-bottom", "1in");
            _dicAttributeInfo.Add("padding-left", "1in");
            bool compare = true;
            foreach (KeyValuePair<string, string> dicData in _dicAttributeInfo)
            {
                if (dicData.Value != _dicAttributeInformation[dicData.Key])
                {
                    compare = false;
                    break;
                }
            }
            Assert.IsTrue(compare, "padding 1 parameters test Failed");
        }

        /// <summary>
        /// Testing "padding" attribute with 2 parameters with swapping input parameters.
        /// </summary>
        [Test]
        public void MapNonClassPadding2ParamSwap()
        {
            _attributeInfo.Name = "padding";
            _attributeInfo.StringValue = "2,in,1,in";
            _dicAttributeInformation = _mapOfficeAttribute.MapMultipleValue(_attributeInfo);
            /// ENHANCE Test from css file
            _dicAttributeInfo.Clear();
            _dicAttributeInfo.Add("padding-top", "1in");
            _dicAttributeInfo.Add("padding-right", "2in");
            _dicAttributeInfo.Add("padding-bottom", "1in");
            _dicAttributeInfo.Add("padding-left", "2in");
            bool compare = true;
            foreach (KeyValuePair<string, string> dicData in _dicAttributeInfo)
            {
                if (dicData.Value != _dicAttributeInformation[dicData.Key])
                {
                    compare = false;
                    break;
                }
            }
            Assert.IsFalse(compare, "padding 2 parameters swap test Failed");
        }

        /// <summary>
        /// Testing "border-bottom" attribute with 3 parameters with input parameters.
        /// </summary>
        [Test]
        public void MapClassBorderBottomParam()
        {
            _attributeInfo.Name = "border-bottom";
            _attributeInfo.StringValue = "2,in,solid,#aa0000";
            StyleAttribute getValue = _mapOfficeAttribute.MapSingleValue(_attributeInfo );
            /// ENHANCE Test from css file
            string compare = "2in solid #aa0000";
            Assert.AreEqual(compare, getValue.StringValue,"border-bottom test Failed");
        }

        /// <summary>
        /// Testing "border-bottom" attribute with 3 parameters with swapping input parameters.
        /// </summary>
        [Test]
        public void MapClassBorderBottomParamSwap()
        {
            _attributeInfo.Name = "border-bottom";
            _attributeInfo.StringValue = "solid,2,in,#aa0000";
            
            StyleAttribute getValue = _mapOfficeAttribute.MapSingleValue(_attributeInfo);
            /// ENHANCE Test from css file
            string compare = "solid 2in #aa0000";
            Assert.AreEqual(compare, getValue.StringValue, "border-bottom test Failed");
        }
        #endregion Public Functions
    }
}