;Install.au3 - 12/2/2011 greg_trihus@sil.org License: LGPL

Func DoInstall()
	If not FileExists("wget.exe") Then
		FileInstall("res\wget.exe", "wget.exe")
	EndIf
	InstallDotNetIfNecessary()
	if BteVersion() Then
		InstallPathway("SetupPw7Bte")
	Else
		InstallPathway("SetupPw7Se")
	EndIf
	RemoveAllUserFolder()
	RemoveLocalFolder()
EndFunc

Func BteVersion()
	Local $fwFolder
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
	Global $InstallStable, $StableVersionDate
	Local $urlPath
	if $InstallStable Then
		$urlPath = 'http://pathway.googlecode.com/files/'
	Else
		$urlPath = 'http://pathway.sil.org/wp-content/sprint/'
	EndIf
	if not FileExists($name) Then
		RunWait("wget.exe " & $urlPath & $name & $StableVersionDate & ".msi")
	EndIf
EndFunc

Func LaunchInstaller($name)
	Sleep( 100 )
	RunWait(@ComSpec & " /c " & $name & " /passive")
EndFunc

Func RemoveAllUserFolder()
	Local $allusers
	$allusers = EnvGet("ALLUSERSPROFILE")
	;MsgBox(4096, "@OSType variable is:", @OSType)
	if @OSType = "WIN_XP" Or @OSType = "WIN_XPe" or @OSType = "WIN32_NT" Then
		$allusers = $allusers & "\Application Data"
	EndIf
	$allusers = $allusers & "\SIL\Pathway"
	;MsgBox(4096, "AllUsersProfile variable is:", $allusers)
	if FileExists($allusers) Then
		DirRemove($allusers, true)
	EndIf
EndFunc

Func RemoveLocalFolder()
	Local $folder
	$folder = EnvGet("TMP")
	$folder = $folder & "\..\SIL\Pathway"
	;MsgBox(4096, "AllUsersProfile variable is:", $folder)
	if FileExists($folder) Then
		DirRemove($folder, true)
	EndIf
EndFunc

Func InstallDotNetIfNecessary()
	Local $DotNet2
	
	;See http://msdn.microsoft.com/en-us/library/xhz1cfs8(v=VS.90).aspx
	$DotNet2 = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework\policy\v2.0", "50727")
	if @error = 0 Then
		return	;Found dot net
	EndIf
	if @OSArch = "X86" Then
		RunWait("wget.exe http://www.microsoft.com/download/en/confirmation.aspx?id=19")
	Else
		RunWait("wget.exe http://www.microsoft.com/download/en/confirmation.aspx?id=6523")
	EndIf
	RunWait("dotnetfx.exe /q:a /c:""install /l /q""")
EndFunc

Func Check4Fw6()
	Local $Fw6
	$Fw6 = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\SIL\FieldWorks", "RootCodeDir")
	if @error Then
		$Fw6 = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\SIL\FieldWorks", "RootCodeDir")
	EndIf
EndFunc