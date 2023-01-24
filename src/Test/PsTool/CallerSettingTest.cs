// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2017, SIL International. All Rights Reserved.

// <copyright from='2017' to='2017' company='SIL International'>
//		Copyright (c) 2017, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>

#endregion
//
// File: CallerSettingTest.cs
// Responsibility: Greg Trihus
// ---------------------------------------------------------------------------------------------
using System.IO;
using NUnit.Framework;
using SIL.Tool;

namespace Test.PsTool
{
	///<summary>
	/// CallerSettingTest Unit Tests
	///</summary>
	[TestFixture]
	[Category("SkipOnTeamCity")]
	public class CallerSettingTest
	{
		[Test]
		public void CallerSettingConstructor()
		{
			using (var result = new CallerSetting())
				Assert.IsNotNull(result.Caller);
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void KbrCallerSettingTest()
		{
			using (var result = new CallerSetting("KBRosU"))
				Assert.AreEqual(DataCreator.CreatorProgram.Paratext7, result.Caller);
		}

		[Test]
		public void KbrGetFontTest()
		{
			using (var cs = new CallerSetting("KBRosU"))
				Assert.AreEqual("Verdana", cs.GetFont());
		}

		[Test]
		public void KbrGetIsoCodeTest()
		{
			using (var cs = new CallerSetting("KBRosU"))
				Assert.AreEqual("drg", cs.GetIsoCode());
		}

		[Test]
		public void KbrIsRightToLeftTest()
		{
			using (var cs = new CallerSetting("KBRosU"))
				Assert.IsFalse(cs.IsRightToLeft());
		}

		[Test]
		public void KbrGetLanguageTest()
		{
			using (var cs = new CallerSetting("KBRosU"))
				Assert.AreEqual("Rungus", cs.GetLanguage());
		}

		[Test]
		public void D33CallerSettingTest()
		{
			using (var result = new CallerSetting("D33"))
				Assert.AreEqual(DataCreator.CreatorProgram.Paratext7, result.Caller);
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void D33GetFontTest()
		{
			using (var cs = new CallerSetting("D33"))
				Assert.AreEqual("Faruma", cs.GetFont());
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void D33GetIsoCodeTest()
		{
			using (var cs = new CallerSetting("D33"))
				Assert.AreEqual("dv", cs.GetIsoCode());
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void D33IsRightToLeftTest()
		{
			using (var cs = new CallerSetting("D33"))
				Assert.IsTrue(cs.IsRightToLeft());
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void D33GetLanguageFontTest()
		{
			using (var cs = new CallerSetting("D33"))
				Assert.AreEqual("Faruma", cs.GetLanguageFont());
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void D33GetLanguageTest()
		{
			using (var cs = new CallerSetting("D33"))
				Assert.AreEqual("Dhivehi", cs.GetLanguage());
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void GondwanaIsRightToLeftTest()
		{
			using (var cs = new CallerSetting("Gondwana Sample"))
				Assert.IsFalse(cs.IsRightToLeft("ggo-Telu-IN"));
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void GondwanaGetLanguageTest()
		{
			using (var cs = new CallerSetting("Gondwana Sample"))
				Assert.AreEqual("Southern Gondi", cs.GetLanguage("ggo-Telu-IN"));
		}

		[Test]
		public void GondwanaGetSettingsNameTest()
		{
			using (var cs = new CallerSetting("Gondwana Sample"))
				if (Path.GetDirectoryName(cs.GetSettingsName()) != @"C:\fwrepo\fw\DistFiles\Projects\Gondwana Sample") // FieldWorks Developer setup
					Assert.AreEqual(@"C:\ProgramData\SIL\FieldWorks\Projects\Gondwana Sample\Gondwana Sample.fwdata", cs.GetSettingsName());
		}
	}
}