/*

JScript to generate a partial WIX source for the installer files.

The contents of the output files "Partial*.wxs" should be copied and pasted into
a suitable full WIX source. If pasted within Visual Studio, proper indentation
will be appiled. (None is generated in the output of this script.)

*/

var fso = new ActiveXObject("Scripting.FileSystemObject");
var Ids = new Array(); // Used to keep all manufactured IDs unique.
var IdInputs = new Array(); // Used to keep all manufactured IDs unique.

// Get script path details:
var iLastBackslash = WScript.ScriptFullName.lastIndexOf("\\");
var ScriptPath = WScript.ScriptFullName.slice(0, iLastBackslash);

// Get root path:
var RootPath = ScriptPath.slice(0, ScriptPath.lastIndexOf("\\"));

// Set up File Library to record component GUIDs:
var xmlFileLibrary = new ActiveXObject("Msxml2.DOMDocument.6.0");
xmlFileLibrary.async = false;
xmlFileLibrary.load("FileLibrary.xml");
if (xmlFileLibrary.parseError.errorCode != 0)
{
	var myErr = xmlFileLibrary.parseError;
	alert("XML error in FileLibrary.xml: " + myErr.reason + "\non line " + myErr.line + " at position " + myErr.linepos);
	showPage("BlockedContentWarning", false);
	showPage("DOMDocumentError", true);
	WScript.Quit();
}
var FileLibraryNode = xmlFileLibrary.selectSingleNode("FileLibrary");
var EditedLibrary = false;

MakeFileSource("Partial PwCtx.wxs", "Files\\PwCtx");

if (EditedLibrary)
{
	// Save modified FileLibrary:
	var tsoLib = fso.OpenTextFile("FileLibrary.xml", 2, true, 0);
	tsoLib.Write(xmlFileLibrary.xml);
	tsoLib.Close();
}

/* WScript.Echo("Done."); */

function MakeFileSource(OutputFileName, SourceFileFolder)
{
	// Get file folder absolute path:
	var FileFolderPath = fso.BuildPath(RootPath, SourceFileFolder);
	var FileAndFolderTree = GetFileAndFolderTree(FileFolderPath);

	var tso = fso.OpenTextFile(OutputFileName, 2, true, -1);
	tso.WriteLine('<!-- Files: -->');
	var ComponentIds = TreeOutput(FileAndFolderTree, tso);
	var iComp;
	tso.WriteLine('');
	tso.WriteLine('<!-- Features: -->');
	for (iComp = 0; iComp < ComponentIds.length; iComp++)
		tso.WriteLine('<ComponentRef Id="' + ComponentIds[iComp] + '"/>');
	tso.Close();
}

// Recursively output a WIX fragment describing the given file and folder tree.
function TreeOutput(Tree, tso)
{
	var ComponentIds = new Array();
	var FolderPath = Tree.FolderPath;
	var ShortFolderName = Tree.ShortName;
	var FolderName = FolderPath.slice(FolderPath.lastIndexOf("\\") + 1)
	var DirId = MakeId("", FolderName, false);
	
	tso.Write('<Directory Id="' + DirId + '" Name="' + ShortFolderName + '" ');
	if (ShortFolderName != FolderName)
		tso.Write('LongName="' + FolderName + '"');
	tso.WriteLine('>');
	
	// Recurse over subfolders:
	var Subfolders = Tree.SubfolderList;
	var folder;
	for (folder = 0; folder < Subfolders.length; folder++)
		ComponentIds = ComponentIds.concat(TreeOutput(Subfolders[folder], tso));

	// Output files:
	var Files = Tree.FileList;
	var file;
	
	for (file = 0; file < Files.length; file++)
	{
		var curFile = Files[file];
		var LongName = curFile.LongName;
		var ShortName = curFile.ShortName;
		var Id = MakeId("", LongName, false);
		var Guid = null;
		
		// Make relative path to source by removing FW root path from absolute file path:
		var RelativeSource = curFile.Path.slice(1 + RootPath.length);
		
		// Test if curFile already exists in FileLibrary.
		// If it does, then use the existing GUID.
		// Else create a new GUID and add it to FileLibrary.
		var SelectString = "//File[translate(@Path, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')='" + RelativeSource.toLowerCase() + "']";
		var LibSearch = xmlFileLibrary.selectSingleNode(SelectString);
		if (LibSearch)
			Guid = LibSearch.getAttribute("ComponentGuid");
		else
		{
			// This is an unknown file:
			Guid = MakeGuid();
			
			// Add file to File Library:
			var NewFileElement = xmlFileLibrary.createElement("File");
			NewFileElement.setAttribute("Path", RelativeSource);
			NewFileElement.setAttribute("ComponentGuid", Guid);

			var FirstFile = FileLibraryNode.selectSingleNode("File[1]");
			FileLibraryNode.insertBefore(NewFileElement, FirstFile);
			FileLibraryNode.insertBefore(xmlFileLibrary.createTextNode("\n"), FirstFile);
			FileLibraryNode.insertBefore(xmlFileLibrary.createTextNode("\t"), FirstFile);
			
			EditedLibrary = true;
		}
		
		tso.WriteLine('<Component Id="' + Id + '" Guid="' + Guid + '">');
		ComponentIds.push(Id);
		tso.Write('<File Id="' + Id + '" Name="' + ShortName + '" ');
		if (LongName != ShortName)
			tso.Write('LongName="' + LongName + '" ');
		if (FolderName == "tools")
			tso.Write('ReadOnly="yes" ');
		tso.WriteLine('Checksum="yes" KeyPath="yes" DiskId="1" Source="..\\' + RelativeSource + '"/>');
		tso.WriteLine('</Component>');
	}
	
	tso.Writeline('</Directory>');
	
	return ComponentIds;
}

// Returns a suitable Id based on the given name. (Removes spaces, etc.)
// Identifiers may contain ASCII characters A-Z, a-z, digits, underscores (_), or periods (.).
// Every identifier must begin with either a letter or an underscore.
// Space is limited to 72 chars, so after that, we just truncate the string.
// If MatchInputs is set to true, this function remembers the inputs it deals with, so that if the same
// inputs are presented again, the same output is given, but if unique inputs are given,
// the output will be unique.
function MakeId(Prefix, Name, MatchInputs)
{
	if (MatchInputs)
	{
		// Test inputs to see if we've had them before:
		var i;
		for (i = 0; i < IdInputs.length; i++)
		{
			if (IdInputs[i].Prefix == Prefix && IdInputs[i].Name == Name)
				return IdInputs[i].Output;
		}
	}
	var MaxLen = 72;
	var Id = Name.split("");
	var ValidChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_.";
	
	for (iChar = 0; iChar < Id.length; iChar++)
		if (ValidChars.indexOf(Id[iChar]) == -1)
			Id[iChar] = "_";

	var Candidate = Prefix + Id.join("");

	var ValidFirstChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz_";
	if (ValidFirstChar.indexOf(Candidate.split("")[0]) == -1)
		Candidate = "_" + Candidate;
	
	if (Candidate.length > MaxLen)
		Candidate = Candidate.slice(0, MaxLen);

	// Test Id to make sure it is unique:
	var fUnique = true;
	for (i = 0; i < Ids.length; i++)
	{
		if (Candidate == Ids[i].Root)
		{
			// Candidate is not unique: it needs a numerical suffix; see what next available one is:
			var CurrentMax = Ids[i].MaxIndex + 1;
			Candidate = Candidate.slice(0, MaxLen - 3) + CurrentMax;
			Ids[i].MaxIndex = CurrentMax;
			fUnique = false;
			break;
		}
	}
	// If Id is unique, register it first, before returning it:
	if (fUnique)
	{
		var NewId = new Object();
		NewId.Root = Candidate;
		NewId.MaxIndex = 1;
		Ids.push(NewId);
	}
	if (MatchInputs)
	{
		var NewIdInput = new Object();
		NewIdInput.Prefix = Prefix;
		NewIdInput.Name = Name;
		NewIdInput.Output = Candidate;
		IdInputs.push(NewIdInput);
	}
	return Candidate;
}

function MakeGuid()
{ 
	return new ActiveXObject("Scriptlet.Typelib").Guid.substr(1,36); 
}

// Recurses given FolderPath and returns an object containing two arrays:
// 1) array of objects containing full names, short names and path strings of files in the folder;
// 2) array of immediate subfolders, which recursively contain their files and subfolders.
// The returned object also includes the folder path for itself.
function GetFileAndFolderTree(FolderPath)
{
	var Results = new Object();
	Results.FolderPath = FolderPath;
	Results.FileList = new Array();
	Results.SubfolderList = new Array();

	// Check if current Folder is a Subversion metadata folder:
	if (FolderPath.slice(-4) == ".svn")
		return Results; // Don't include SVN folders.
	
	// Add files in current folder:
	var Folder = fso.GetFolder(FolderPath);
	Results.ShortName = Folder.ShortName;
	var FileIterator = new Enumerator(Folder.files);
	for (; !FileIterator.atEnd(); FileIterator.moveNext())
	{
		var FileObject = new Object();
		FileObject.Path = FileIterator.item().Path;
		FileObject.LongName = FileIterator.item().Name;
		FileObject.ShortName = FileIterator.item().ShortName;
		Results.FileList.push(FileObject);
	}

	// Now recurse all subfolders:
	var SubfolderIterator = new Enumerator(Folder.SubFolders);
	for (; !SubfolderIterator.atEnd(); SubfolderIterator.moveNext())
		Results.SubfolderList.push(GetFileAndFolderTree(SubfolderIterator.item().Path));
		
	return Results;
}
