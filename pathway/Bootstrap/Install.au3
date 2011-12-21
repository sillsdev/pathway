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
	InstallJavaIfNecessary()
	GUICtrlSetData($bar, 77)
	InstallLibreOfficeIfNecessary()
	GUICtrlSetData($bar, 84)
	InstallPrinceXmlIfNecessary()
	GUICtrlSetData($bar, 87)
	InstallXeLaTeXIfNecessary()
	GUICtrlSetData($bar, 90)
	InstallPdfReaderIfNecessary()
	GUICtrlSetData($bar, 93)
	RemoveAllUserFolder()
	GUICtrlSetData($bar, 96)
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

Func InstallJavaIfNecessary()
	Local $ver, $path
	;See http://stackoverflow.com/questions/2951804/how-to-check-java-installation-from-batch-script
	$ver = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Runtime Environment", "CurrentVersion")
	if @error = 0 Then
		;MsgBox(4096,"Status","Java version " & $ver)
		$path = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Runtime Environment\" & $ver, "JavaHome")
		if FileExists( $path & "\bin\java.exe") Then
			return ;Already installed
		EndIf
	EndIf
	if MsgBox(35,"No Java","Java is used by the Epub validator among other things. It is not installed in your computer. Would you like to install Java?") = 6 Then
		LaunchSite("http://java.com/en/download/index.jsp")
	EndIf
EndFunc

Func InstallLibreOfficeIfNecessary()
	Local $ver, $cmd
	$ver = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Classes\opendocument.WriterDocument\CurVer", "")
	if @error = 0 Then
		;MsgBox(4096,"Status","Libre Office version " & $ver)
		$cmd = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Classes\" & $ver & "\shell\open\command", "")
		if StringInStr($cmd, "swriter.exe") > 0 Then
			return ;Already installed
		EndIf
	EndIf
	if MsgBox(35,"No Libre Office","Libre Office is one of the main output destinations. It is not installed in your computer. Would you like to install Libre Office?") = 6 Then
		LaunchSite("http://www.libreoffice.org/download/")
	EndIf
EndFunc

Func InstallPrinceXmlIfNecessary()
	Local $path
	$path = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Prince_is1", "InstallLocation")
	if @error <> 0 Then
		$path = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Prince_is1", "InstallLocation")
	EndIf
	if @error = 0 Then
		;MsgBox(4096,"Status","PrinceXml path " & $path)
		if FileExists( $path & "Prince.exe") Then
			return ;Already installed
		EndIf
	EndIf
	if MsgBox(35,"No PrinceXml","PrinceXml is used to create new previews and is one of the output destinations. It is not installed in your computer. Would you like to install PrinceXml?") = 6 Then
		LaunchSite("http://www.princexml.com/download/")
	EndIf
EndFunc

Func InstallXeLaTeXIfNecessary()
	Global $InstallStable
	Local $path, $SaveGlobal
	$path = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\SIL\PathwayXeLaTeX", "XeLaTexDir")
	if @error <> 0 Then
		$path = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\SIL\PathwayXeLaTeX", "XeLaTexDir")
	EndIf
	if @error = 0 Then
		;MsgBox(4096,"Status","XeLaTeX path " & $path)
		if FileExists( $path & "bin\win32\xelatex.exe") Then
			return ;Already installed
		EndIf
	EndIf
	if MsgBox(35,"No XeLaTeX","XeLaTeX is one of the output destinations (similar to a typesetting system). It is not installed in your computer. Would you like to install XeLaTeX?") = 6 Then
		$SaveGlobal = $InstallStable
		$InstallStable = False
		InstallPathway("SetupXeLaTeXTesting")
		$InstallStable = $SaveGlobal
	EndIf
EndFunc

Func InstallPdfReaderIfNecessary()
	Local $ver, $cmd
	$ver = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.pdf", "")
	if @error = 0 Then
		;MsgBox(4096,"Status","Libre Office version " & $ver)
		$cmd = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Classes\" & $ver & "\shell\open\command", "")
		if @error = 0 Then
			return ;Already installed
		EndIf
	EndIf
	if MsgBox(35,"No Pdf Reader","The Pdf Reader displays Pdf results after they are produced by various destinations. None installed in your computer. Would you like to install a Pdf Reader?") = 6 Then
		LaunchSite("http://get.adobe.com/reader/")
	EndIf
EndFunc

Func LaunchSite($url)
	Local $http, $cmd
	;See http://stackoverflow.com/questions/5501349/open-website-in-the-users-default-browser-without-letting-them-launch-anything
	$http = RegRead("HKEY_CURRENT_USER\Software\Classes\http\shell\open\command", "")
	if @error = 0 Then
		$cmd = StringReplace($http, "%1", $url)
	Else
		$http = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Classes\HTTP\shell\open\command", "")
		$cmd = $http & " " & $url
	EndIf
	RunWait($cmd)
EndFunc

Func Check4Fw6()
	Local $Fw6
	$Fw6 = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\SIL\FieldWorks", "RootCodeDir")
	if @error Then
		$Fw6 = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\SIL\FieldWorks", "RootCodeDir")
	EndIf
EndFunc