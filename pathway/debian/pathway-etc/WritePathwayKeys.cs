#!/usr/bin/csharp

// This script assumes MONO_REGISTRY_PATH is set to a writeable path.

using System;
using System.IO;

var localkey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("Software\\sil\\pathway");
localkey.SetValue("PathwayDir", "/usr/lib/pathway"), Microsoft.Win32.RegistryValueKind.String);
localkey.SetValue("WritingSystemStore", "~/.local/share/SIL/WritingSystemStore/"), Microsoft.Win32.RegistryValueKind.String);

