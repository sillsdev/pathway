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
// File: CallerSetting.cs
// Responsibility: Greg Trihus
// ---------------------------------------------------------------------------------------------
using System;
using System.IO;
using System.Linq;
using System.Xml;
using SIL.WritingSystems;
using IniParser;
using IniParser.Model;
using SIL.WritingSystems.Migration;

namespace SIL.Tool
{
	public class CallerSetting : IDisposable
	{
		public DataCreator.CreatorProgram Caller;
		public string DatabaseName;
		private string _settingsFullPath;

		public string SettingsFullPath
		{
			set
			{
				_dataFolder = Path.GetDirectoryName(value);
				DatabaseName = Path.GetFileNameWithoutExtension(value);
				if (DataCreator.Creator == DataCreator.CreatorProgram.Paratext7)
				{
					_dataFolder = Path.Combine(_dataFolder, DatabaseName);
				}
				_settingsFullPath = value;
			}
			get { return _settingsFullPath; }
		}

		public readonly WritingSystemDefinition WritingSystem;
		public IniData LanguageData;
		private string _dataFolder;
		private string _ldmlFolder;

		public CallerSetting()
		{
			_xDoc.RemoveAll();
			if (!Sldr.IsInitialized) Sldr.Initialize(true);
			WritingSystem = new WritingSystemDefinition();
			Caller = DataCreator.Creator;
			switch (Caller)
			{
				case DataCreator.CreatorProgram.Paratext8:
					_dataFolder = RegistryHelperLite.FallbackStringValue("Paratext/8", "Settings_Directory");
					break;
				case DataCreator.CreatorProgram.Paratext7:
					_dataFolder = RegistryHelperLite.FallbackStringValue("ScrChecks/1.0/Settings_Directory");
					break;
				case DataCreator.CreatorProgram.FieldWorks:
					_dataFolder = RegistryHelperLite.FallbackStringValue("SIL/FieldWorks/8", "ProjectsDir");
					SetupLdmlFolder();
					break;
			}
			if (Caller != DataCreator.CreatorProgram.Unknown) return;
			FindDataFolder();
		}

		public CallerSetting(string database)
		{
			_xDoc.RemoveAll();
			if (!Sldr.IsInitialized) Sldr.Initialize(true);
			WritingSystem = new WritingSystemDefinition();
			DatabaseName = database;
			if (database != "DatabaseName") FindDataFolder();
			if (Caller == DataCreator.CreatorProgram.FieldWorks)
			{
				SetupLdmlFolder();
			}
			if (Caller != null) return;
			Caller = DataCreator.Creator;
		}

		private void SetupLdmlFolder()
		{
			// Since we are only reading the Writing Systems, we make a copy and migrate.
			_ldmlFolder = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetTempFileName()));
			FolderTree.Copy(Common.GetLDMLPath(), _ldmlFolder);
			var migrator = new LdmlInFolderWritingSystemRepositoryMigrator(_ldmlFolder, null);
			migrator.Migrate();
		}

		public void Dispose()
		{
			if (Sldr.IsInitialized) Sldr.Cleanup();
			if (Directory.Exists(_ldmlFolder)) Directory.Delete(_ldmlFolder, true);
		}

		public void FindDataFolder()
		{
			if (string.IsNullOrEmpty(DatabaseName)) return;
			if (!string.IsNullOrEmpty(_dataFolder)) return;
			var folder = RegistryHelperLite.FallbackStringValue("Paratext/8", "Settings_Directory");
			if (!string.IsNullOrEmpty(folder))
			{
				if (TestFolder(folder, DataCreator.CreatorProgram.Paratext8)) return;
			}
			folder = RegistryHelperLite.FallbackStringValue("ScrChecks/1.0/Settings_Directory");
			if (!string.IsNullOrEmpty(folder))
			{
				if (TestFolder(folder, DataCreator.CreatorProgram.Paratext7)) return;
			}
			folder = RegistryHelperLite.FallbackStringValue("SIL/FieldWorks/8", "ProjectsDir");
			if (!string.IsNullOrEmpty(folder))
			{
				TestFolder(folder, DataCreator.CreatorProgram.FieldWorks);
			}
		}

		private bool TestFolder(string folder, DataCreator.CreatorProgram program)
		{
			folder = Path.Combine(folder, DatabaseName);
			if (!Directory.Exists(folder)) return false;
			Caller = program;
			_dataFolder = folder;
			return true;
		}

		public string File(string name)
		{
			if (string.IsNullOrEmpty(_dataFolder)) return name;
			return (Path.Combine(_dataFolder, name));
		}

		public string GetSettingsName()
		{
			if (!string.IsNullOrEmpty(SettingsFullPath)) return SettingsFullPath;
			FindDataFolder();
			if (string.IsNullOrEmpty(_dataFolder)) return null;
			switch (Caller)
			{
				case DataCreator.CreatorProgram.Paratext8:
					_settingsFullPath = Path.Combine(_dataFolder, "Settings.xml");
					break;
				case DataCreator.CreatorProgram.Paratext7:
					_settingsFullPath = Path.Combine(_dataFolder, "..", DatabaseName + ".ssf");
					break;
				case DataCreator.CreatorProgram.FieldWorks:
					_settingsFullPath = Path.Combine(_dataFolder, DatabaseName + ".fwdata");
					break;
			}
			return SettingsFullPath;
		}

		private readonly XmlDocument _xDoc = new XmlDocument();

		public void LoadSettings()
		{
			if (_xDoc.DocumentElement != null && _xDoc.DocumentElement.HasChildNodes) return;
			if (GetSettingsName() == null) return;
			XmlReader xr = null;
			try
			{
				xr = XmlReader.Create(SettingsFullPath);
				_xDoc.Load(xr);
			}
			catch (Exception)
			{
				// on error (either not exist or has DTD) just use an empty settings file
				_xDoc.LoadXml("<root/>");
			}
			finally
			{
				try
				{
					xr.Close();
				}
				catch (Exception)
				{
					// try to close, if it doesn't close, it probably isn't open
				}
			}
		}

		public string GetFont()
		{
			LoadSettings();
			var node = _xDoc.SelectSingleNode("//ScriptureText/DefaultFont");
			return node != null? node.InnerText: null;
		}

		public string GetIsoCode()
		{
			LoadSettings();
			var node = _xDoc.SelectSingleNode("//ScriptureText/LanguageIsoCode");
			var code = node != null? node.InnerText: null;
			var parts = code != null? code.Split(':'): null;
			return parts != null? parts.ElementAt(0): null;
		}

		public string GetName()
		{
			LoadSettings();
			var node = _xDoc.SelectSingleNode("//ScriptureText/FullName");
			return node != null? node.InnerText: null;
		}

		public string GetCopyright()
		{
			LoadSettings();
			var node = _xDoc.SelectSingleNode("//ScriptureText/Copyright");
			return node != null? node.InnerText: null;
		}

		public string GetSettingValue(string xpath)
		{
			LoadSettings();
			var node = _xDoc.SelectSingleNode(xpath);
			return node != null? node.InnerText: null;
		}

		public void LoadWritingSystem()
		{
			var iso = GetIsoCode();
			if (iso == null) return;
			LoadWritingSystem(iso);
		}

		public void LoadWritingSystem(string iso)
		{
			if (string.IsNullOrEmpty(_dataFolder)) return;
			switch (Caller)
			{
				case DataCreator.CreatorProgram.Paratext8:
					LoadLdml(_dataFolder, iso);
					break;
				case DataCreator.CreatorProgram.Paratext7:
					if (LanguageData != null) return;
					var language = GetLanguage();
					var languageFullName = Path.Combine(_dataFolder, "..", language + ".lds");
					LanguageData = null;
					if (System.IO.File.Exists(languageFullName))
					{
						var parser = new FileIniDataParser();
						LanguageData = parser.ReadFile(languageFullName);
					}
					break;
				case DataCreator.CreatorProgram.FieldWorks:
					LoadLdml(_ldmlFolder, iso);
					break;
			}
		}

		private void LoadLdml(string path, string iso)
		{
			if (WritingSystem.Language == iso) return;
			var ldmlFilePath = Path.Combine(path, iso + ".ldml");
			if (!System.IO.File.Exists(ldmlFilePath)) return;
			var ldmlAdaptor = new LdmlDataMapper(new WritingSystemFactory());
			ldmlAdaptor.Read(ldmlFilePath, WritingSystem);
		}

		public bool IsRightToLeft()
		{
			var iso = GetIsoCode();
			if (iso == null) return false;
			return IsRightToLeft(iso);
		}

		public bool IsRightToLeft(string iso)
		{
			LoadWritingSystem(iso);
			switch (Caller)
			{
				case DataCreator.CreatorProgram.Paratext8:
					return WritingSystem.RightToLeftScript;
				case DataCreator.CreatorProgram.Paratext7:
					return LanguageData != null && LanguageData["General"].ContainsKey("RTL") && LanguageData["General"]["RTL"].ToUpper() == "T";
				case DataCreator.CreatorProgram.FieldWorks:
					return WritingSystem.RightToLeftScript;
			}
			return false;
		}

		public string GetLanguageFont()
		{
			var iso = GetIsoCode();
			if (iso == null) return string.Empty;
			return GetLanguageFont(iso);
		}

		public string GetLanguageFont(string iso)
		{
			LoadWritingSystem(iso);
			switch (Caller)
			{
				case DataCreator.CreatorProgram.Paratext8:
					return WritingSystem.DefaultFont.Name;
				case DataCreator.CreatorProgram.Paratext7:
					return LanguageData["General"]["font"];
				case DataCreator.CreatorProgram.FieldWorks:
					return WritingSystem.DefaultFont.Name;
			}
			return string.Empty;
		}

		public string GetLanguageFontFeatures()
		{
			var iso = GetIsoCode();
			if (iso == null) return string.Empty;
			return GetLanguageFontFeatures(iso);
		}

		public string GetLanguageFontFeatures(string iso)
		{
			LoadWritingSystem(iso);
			switch (Caller)
			{
				case DataCreator.CreatorProgram.Paratext8:
					return WritingSystem.DefaultFont.Features;
				case DataCreator.CreatorProgram.Paratext7:
					return LanguageData != null && LanguageData["General"].ContainsKey("fontFeatureSettings") ? LanguageData["General"]["fontFeatureSettings"] : "";
				case DataCreator.CreatorProgram.FieldWorks:
					return WritingSystem.DefaultFont.Features;
			}
			return string.Empty;
		}

		public string GetLanguage()
		{
			var iso = GetIsoCode();
			if (iso == null) return string.Empty;
			return GetLanguage(iso);
		}

		public string GetLanguage(string iso)
		{
			switch (Caller)
			{
				case DataCreator.CreatorProgram.Paratext8:
				case DataCreator.CreatorProgram.Paratext7:
					LoadSettings();
					var node = _xDoc.SelectSingleNode("//ScriptureText/Language");
					return node != null? node.InnerText: null;
				case DataCreator.CreatorProgram.FieldWorks:
					LoadWritingSystem(iso);
					return WritingSystem.Language.Name;
			}
			return string.Empty;
		}

		public string PicturePath(string name)
		{
			switch (Caller)
			{
				case DataCreator.CreatorProgram.Paratext8:
				case DataCreator.CreatorProgram.Paratext7:
                    var hiRes = !string.IsNullOrEmpty(_dataFolder)? Path.Combine(_dataFolder, "local", "figures", name): Path.Combine("local", "figures", name);
					if (System.IO.File.Exists(hiRes)) return hiRes;
                    var loResName = Path.GetFileNameWithoutExtension(name) + ".jpg";
                    return !string.IsNullOrEmpty(_dataFolder) ? Path.Combine(_dataFolder, "figures", loResName): Path.Combine("figures", loResName);
				case DataCreator.CreatorProgram.FieldWorks:
					return !string.IsNullOrEmpty(_dataFolder) ? Path.Combine(_dataFolder, "LinkedFiles", "Pictures", name): Path.Combine("Pictures", name);
			}
			return !string.IsNullOrEmpty(_dataFolder) ? Path.Combine(_dataFolder, name): name;
		}
	}
}
