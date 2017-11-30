// --------------------------------------------------------------------------------------------
// <copyright file="EmbeddedFontTest.cs" from='2009' to='2014' company='SIL International'>
//      Copyright ( c ) 2014, SIL International. All Rights Reserved.
//
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
// <author>Erik Brommers</author>
// <email>erik_brommers@sil.org</email>
// Last reviewed:
//
// <remarks>
// Test methods of EmbeddedFont class
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Xml;
using epubConvert;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test
{
	[TestFixture]
	public class epubFontTest : EpubFont
	{
		private static TestFiles _tf;

		public epubFontTest():base(new Exportepub())
		{
			
		}

		[TestFixtureSetUp]
		public void Setup()
		{
			_tf = new TestFiles ("epubConvert");
		}

		[Test]
		/// <summary>
		/// GetLanguageForReversalNumberTest tests for sharing violation TD-4885
		/// </summary>
		public void GetLanguageForReversalNumberTest()
		{
			var inFile = _tf.SubInput ("EpubFont", "FlexRev.xhtml");
			var xr = XmlReader.Create (inFile);
			var result = GetLanguageForReversalNumber (inFile, "zxx");
			xr.Close ();
			Assert.AreEqual ("zxx", result);
		}
	}
}

