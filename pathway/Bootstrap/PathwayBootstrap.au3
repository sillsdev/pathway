If not FileExists("wget.exe") Then
	FileInstall("res\wget.exe", "wget.exe")
EndIf
InstallDotNetIfNecessary()
if BteVersion() Then
	InstallPathway("SetupPw7Bte.msi")
Else
	InstallPathway("SetupPw7Se.msi")
EndIf
RemoveAllUserFolder()
RemoveLocalFolder()
Exit

Func BteVersion()
	RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\ScrChecks\1.0\Settings_Directory","")
	if @error Then
		$fwFolder = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\SIL\FieldWorks\7.0", "RootCodeDir")
		if @error Then
			return 0
		Else
			if not FileExists( $fwFolder & "\TE.EXE" ) Then
				return 0
			EndIf
		Endif
	Endif
	return 1
EndFunc

Func InstallPathway($name)
	RemoveOldSetup($name)
	GetInstaller($name)
	LaunchInstaller($name)
EndFunc

Func RemoveOldSetup($name)
	Local $attrib
	$attrib = FileGetAttrib($name)
	if @error = 0 Then
		if Not StringInStr($attrib, "R") Then
			FileDelete($name)
		EndIf
	Endif
EndFunc

Func GetInstaller($name)
	if not FileExists($name) Then
		RunWait("wget.exe http://pathway.sil.org/wp-content/sprint/" & $name)
	EndIf
EndFunc

Func LaunchInstaller($name)
	Sleep( 100 )
	RunWait(@ComSpec & " /c " & $name & " /passive")
EndFunc

Func RemoveAllUserFolder()
	Local $allusers
	$allusers = EnvGet("ALLUSERSPROFILE")
	MsgBox(4096, "@OSType variable is:", @OSType)
	if @OSType = "WIN_XP" Or @OSType = "WIN_XPe" or @OSType = "WIN32_NT" Then
		$allusers = $allusers & "\Application Data"
	EndIf
	$allusers = $allusers & "\SIL\Pathway"
	MsgBox(4096, "AllUsersProfile variable is:", $allusers)
	if FileExists($allusers) Then
		DirRemove($allusers, true)
	EndIf
EndFunc

Func RemoveLocalFolder()
	Local $folder
	$folder = EnvGet("TMP")
	$folder = $folder & "\..\SIL\Pathway"
	MsgBox(4096, "AllUsersProfile variable is:", $folder)
	if FileExists($folder) Then
		DirRemove($folder, true)
	EndIf
EndFunc

Func InstallDotNetIfNecessary()
	$DotNet2 = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\NET Framework Setup\NDP\v2.0.50727", "Install")
	if @error Then
		$DotNet2 = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework Setup\NDP\v2.0.50727", "Install")
	EndIf
	if @error = 0 Then
		return	;Found dot net
	EndIf
	if @OSArch = "X86" Then
		RunWait("wget.exe http://www.microsoft.com/download/en/confirmation.aspx?id=19")
		Run("dotnetfx.exe")
	Else
		RunWait("wget.exe http://www.microsoft.com/download/en/confirmation.aspx?id=6523")
		Run("dotnetfx.exe")
	EndIf
EndFunc

