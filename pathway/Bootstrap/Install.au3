;Install.au3 - 12/2/2011 greg_trihus@sil.org License: LGPL

;DoInstall()

Func DoInstall($bar)
	If not FileExists("wget.exe") Then
		FileInstall("res\wget.exe", "wget.exe")
	EndIf
	InstallDotNetIfNecessary()
	GUICtrlSetData($bar, 14)
	if BteVersion() Then
		GUICtrlSetData($bar, 28)
		InstallPathway("SetupPw7BTE", $bar)
	Else
		GUICtrlSetData($bar, 28)
		InstallPathway("SetupPw7SE", $bar)
	EndIf
	RemoveAllUserFolder()
	GUICtrlSetData($bar, 84)
	RemoveLocalFolder()
	GUICtrlSetData($bar, 100)
EndFunc

Func BteVersion()
	Local $fwFolder
	RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\ScrChecks\1.0\Settings_Directory","")
	if @error Then
		$fwFolder = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\SIL\FieldWorks\7.0", "RootCodeDir")
		if @error Then
			;MsgBox(4096,"Status","No BTE")
			return 0
		Else
			if not FileExists( $fwFolder & "\TE.EXE" ) Then
				;MsgBox(4096,"Status","No TE")
				return 0
			EndIf
			;MsgBox(4096,"Status","TE found")
		Endif
	Endif
	;MsgBox(4096,"Status","BTE found")
	return 1
EndFunc

Func InstallPathway($name, $bar)
	Global $InstallStable, $StableVersionDate
	If $InstallStable Then
		$name = $name & $StableVersionDate
	Endif
	$name = $name & ".msi"
	;MsgBox(4096,"Status","Installing " & $name)
	RemoveOldSetup($name)
	GUICtrlSetData($bar, 42)
	GetInstaller($name)
	GUICtrlSetData($bar, 56)
	LaunchInstaller($name)
	GUICtrlSetData($bar, 70)
EndFunc

Func RemoveOldSetup($name)
	Local $attrib
	$attrib = FileGetAttrib($name)
	if @error = 0 Then
		;MsgBox(4096,"Status",$name & " found.")
		if Not StringInStr($attrib, "R") Then
			;MsgBox(4096,"Status","Old " & $name & " being delted.")
			FileDelete($name)
		EndIf
	Endif
EndFunc

Func GetInstaller($name)
	Global $InstallStable
	Local $urlPath
	if $InstallStable Then
		$urlPath = 'http://pathway.googlecode.com/files/'
	Else
		$urlPath = 'http://pathway.sil.org/wp-content/sprint/'
	EndIf
	if not FileExists($name) Then
		;MsgBox(4096,"Status","Downloading " & $urlPath & $name)
		RunWait("wget.exe " & $urlPath & $name)
	EndIf
EndFunc

Func LaunchInstaller($name)
	Sleep( 100 )
	;MsgBox(4096,"Status","Launching passive installer " & $name)
	RunWait(@ComSpec & " /c " & $name)
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
	;MsgBox(4096,"Status","Removing " & $allusers)
	DirRemove($allusers, true)
	EndIf
EndFunc

Func RemoveLocalFolder()
	Local $folder
	$folder = EnvGet("TMP")
	$folder = $folder & "\..\SIL\Pathway"
	;MsgBox(4096, "AllUsersProfile variable is:", $folder)
	if FileExists($folder) Then
		;MsgBox(4096,"Status","Removing " & $folder)
		DirRemove($folder, true)
	EndIf
EndFunc

Func InstallDotNetIfNecessary()
	Local $DotNet2
	
	;See http://msdn.microsoft.com/en-us/library/xhz1cfs8(v=VS.90).aspx
	$DotNet2 = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\.NETFramework\Policy\v2.0", "50727")
	if @error = 0 Then
		;MsgBox(4096,"Status","Installing dot net is unnecessary...")
		return	;Found dot net
	EndIf
	if @OSArch = "X86" Then
		;MsgBox(4096,"Status","Installing dot net x86...")
		RunWait("wget.exe http://pathway.sil.org/wp-content/sprint/dotnetfx.exe")
		RunWait("dotnetfx.exe /q:a /c:""install /l /q""")
	Else
		;MsgBox(4096,"Status","Installing dot net 64...")
		RunWait("wget.exe http://pathway.sil.org/wp-content/sprint/NetFx64.exe")
		RunWait("NetFx64.exe /q:a /c:""install /l /q""")
	EndIf
EndFunc

Func Check4Fw6()
	Local $Fw6
	$Fw6 = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\SIL\FieldWorks", "RootCodeDir")
	if @error Then
		$Fw6 = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\SIL\FieldWorks", "RootCodeDir")
	EndIf
EndFunc