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
// File: DataCreator.cs
// Responsibility: Greg Trihus
// ---------------------------------------------------------------------------------------------
using System.IO;
using System.Windows.Forms;

namespace SIL.Tool
{
	public static class DataCreator
	{
		public static CreatorProgram Creator;

		public enum CreatorProgram
		{
			Paratext7,
			Paratext8,
			FieldWorks8,
			FieldWorks9,
			Unknown
		}

		static DataCreator()
		{
			string executablePath = Path.GetDirectoryName(Application.ExecutablePath);
			if (executablePath == null || Creator != CreatorProgram.Unknown)
			{
				return;
			}
			if (executablePath.ToLower().Contains("fieldworks 8"))
			{
				Creator = CreatorProgram.FieldWorks8;
			}
			else if (executablePath.ToLower().Contains("fieldworks"))
			{
				Creator = CreatorProgram.FieldWorks9;
			}
			else if (executablePath.ToLower().Contains("paratext 7"))
			{
				Creator = CreatorProgram.Paratext7;
			}
			else if (executablePath.ToLower().Contains("paratext 8"))
			{
				Creator = CreatorProgram.Paratext8;
			}
			else
			{
				// This could be the configuration tool, nunit test, etc. -
				// whatever it is, it doesn't have a settings file we need to look at.
				Creator = CreatorProgram.Unknown;
			}
		}
	}
}
