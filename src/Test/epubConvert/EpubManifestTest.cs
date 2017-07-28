// --------------------------------------------------------------------------------------------
// <copyright file="EpubManifestTest.cs" from='2009' to='2017' company='SIL International'>
//      Copyright ( c ) 2017, SIL International. All Rights Reserved.
//
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed:
//
// <remarks>
// Test methods of EmbeddedFont class
// </remarks>
// --------------------------------------------------------------------------------------------

using System.Xml;
using epubConvert;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.epubConvert
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Test functions of epub Convert
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class EpubManifestTest : EpubManifest
	{
		#region setup
		private static TestFiles _tf;

		[TestFixtureSetUp]
		public void Setup()
		{
			Common.Testing = true;
			_tf = new TestFiles("epubConvert");
		}

		public EpubManifestTest() : base(null, null)
		{
		}

		public EpubManifestTest(Exportepub exportepub, EpubFont epubFont) : base(exportepub, epubFont)
		{
		}
		#endregion setup

		[Test]
		public void IsScriptedTest()
		{
			const string testName = "IsScripted";
			var input = _tf.Copy(testName + ".html");
			var result = isScripted(input);
			Assert.IsTrue(result);
			var xDoc = Common.DeclareXMLDocument(true);
			var xrs = new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore };
			var xr = XmlReader.Create(input, xrs);
			xDoc.Load(xr);
			xr.Close();
			var ns = new XmlNamespaceManager(xDoc.NameTable);
			Assert.IsNotNull(xDoc.DocumentElement, "output file doesn't load as XML document");
			ns.AddNamespace("xhtml", xDoc.DocumentElement.NamespaceURI);
			var node = xDoc.SelectSingleNode("//xhtml:audio", ns);
			Assert.IsNotNull(node);
			Assert.AreEqual(2, node.ChildNodes.Count, "should have source element and fall back text");
			Assert.AreEqual(node.InnerText, "Missing ninth day of Bhadra.wav");
			var srcNode = xDoc.SelectSingleNode("//xhtml:source/@src", ns);
			Assert.IsNotNull(srcNode, "source node with src attribute missing");
			Assert.AreEqual(srcNode.InnerText, "AudioVisual/ninth day of Bhadra.wav");
		}

	}
}

