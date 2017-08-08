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

using System;
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

		[Test]
		public void ManifestAvContentTest()
		{
			const string testName = "ManifestAvContent";
			var output = _tf.Output(testName + ".opf");
			var wavInput = _tf.Input(testName + ".wav");
			var mp3Input = _tf.Input(testName + ".mp3");
			var oggInput = _tf.Input(testName + ".ogg");
			var mp4Input = _tf.Input(testName + ".mp4");
			var files = new String[] {wavInput, mp3Input, oggInput, mp4Input};
			var xws = new XmlWriterSettings {Indent = false};
			var xw = XmlWriter.Create(output, xws);
			xw.WriteStartElement("opf");
			ManifestAvContent(xw, files);
			xw.WriteEndElement();
			xw.Close();
			var xDoc = Common.DeclareXMLDocument(true);
			var xrs = new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore };
			var xr = XmlReader.Create(output, xrs);
			xDoc.Load(xr);
			xr.Close();
			var wavNode = xDoc.SelectSingleNode("//item") as XmlElement;
			Assert.IsNotNull(wavNode);
			Assert.AreEqual("audio/vnd.wav", wavNode.Attributes["media-type"].Value);
			var mp3Node = xDoc.SelectSingleNode("//item[contains(@href,'.mp3')]") as XmlElement;
			Assert.IsNotNull(mp3Node);
			Assert.AreEqual("audio/mpeg3", mp3Node.Attributes["media-type"].Value);
			var oggNode = xDoc.SelectSingleNode("//item[contains(@href,'.ogg')]") as XmlElement;
			Assert.IsNotNull(oggNode);
			Assert.AreEqual("audio/ogg", oggNode.Attributes["media-type"].Value);
		}
	}
}

