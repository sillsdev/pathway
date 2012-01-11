;Install.au3 - 12/2/2011 greg_trihus@sil.org License: LGPL

;DoInstall()

Func DoInstall($bar)
	If not FileExists("wget.exe") Then
		FileInstall("res\wget.exe", "wget.exe")
	EndIf
	InstallDotNetIfNecessary()
	GUICtrlSetData($bar, 10)
	if BteVersion() Then
		GUICtrlSetData($bar, 20)
		InstallPathway("SetupPw7BTE")
	Else
		GUICtrlSetData($bar, 20)
		InstallPathway("SetupPw7SE")
	EndIf
	InstallVersions()
	GUICtrlSetData($bar, 30)
	InstallJavaIfNecessary()
	GUICtrlSetData($bar, 40)
	InstallLibreOfficeIfNecessary()
	GUICtrlSetData($bar, 50)
	InstallPrinceXmlIfNecessary()
	GUICtrlSetData($bar, 60)
	InstallPdfReaderIfNecessary()
	GUICtrlSetData($bar, 65)
	InstallEpubReaderIfNecessary()
	GUICtrlSetData($bar, 70)
	InstallXeLaTeXIfNecessary()
	GUICtrlSetData($bar, 80)
	RemoveAllUserFolder()
	GUICtrlSetData($bar, 90)
	RemoveLocalFolder()
	GUICtrlSetData($bar, 100)
	FileDelete("wget.exe")
EndFunc

Func BteVersion()
	Local $fwFolder
	RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\ScrChecks\1.0\Settings_Directory","")
	if @error Then
		RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\ScrChecks\1.0\Settings_Directory","")
	EndIf
	if @error Then
		$fwFolder = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\SIL\FieldWorks\7.0", "RootCodeDir")
		if @error Then
			$fwFolder = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\SIL\FieldWorks\7.0", "RootCodeDir")
		EndIf
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

Func InstallPathway($name)
	Global $InstallStable, $StableVersionDate
	If $InstallStable Then
		$name = $name & $StableVersionDate
	Endif
	$name = $name & ".msi"
	;MsgBox(4096,"Status","Installing " & $name)
	CleanUp($name)
	GetInstaller($name)
	LaunchInstaller($name)
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
	CleanUp($name)
EndFunc

Func RemoveAllUserFolder()
	Local $allusers, $folder, $tmpFolder, $oldFolder
	$allusers = EnvGet("ALLUSERSPROFILE")
	;MsgBox(4096, "@OSType variable is:", @OSType)
	if @OSType = "WIN_XP" Or @OSType = "WIN_XPe" or @OSType = "WIN32_NT" Then
		$allusers = $allusers & "\Application Data"
	EndIf
	$folder = $allusers & "\SIL\Pathway"
	;MsgBox(4096, "AllUsersProfile variable is:", $allusers)
	if FileExists($folder) Then
		;MsgBox(4096,"Status","Removing " & $allusers)
		$tmpFolder = EnvGet("TMP")
		$oldFolder = $tmpFolder & "\..\SIL\PathwayOld"
		DirCopy($folder, $oldFolder, 1)
		DirRemove($folder, true)
	EndIf
EndFunc

Func RemoveLocalFolder()
	Local $folder, $tmpFolder, $oldFolder
	$tmpFolder = EnvGet("TMP")
	$folder = $tmpFolder & "\..\SIL\Pathway"
	;MsgBox(4096, "AllUsersProfile variable is:", $folder)
	if FileExists($folder) Then
		;MsgBox(4096,"Status","Removing " & $folder)
		$oldFolder = $tmpFolder & "\..\SIL\PathwayOld"
		DirCopy($folder, $oldFolder, 1)
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
		GetFromUrl("dotnetfx.exe", "http://pathway.sil.org/wp-content/sprint/dotnetfx.exe")
		;RunWait("dotnetfx.exe /q:a /c:""install /l /q""")
		RunWait("dotnetfx.exe")
		CleanUp("dotnetfx.exe")
	Else
		;MsgBox(4096,"Status","Installing dot net 64...")
		GetFromUrl("NetFx64.exe", "http://pathway.sil.org/wp-content/sprint/NetFx64.exe")
		;RunWait("NetFx64.exe /q:a /c:""install /l /q""")
		RunWait("NetFx64.exe")
		CleanUp("NetFx64.exe")
	EndIf
EndFunc

Func InstallVersions()
	Local $attrib, $name
	$name = "PathwayBootstrap.ini"
	$attrib = FileGetAttrib($name)
	if @error = 0 Then
		;MsgBox(4096,"Status",$name & " found.")
		if Not StringInStr($attrib, "R") Then
			;MsgBox(4096,"Status","Old " & $name & " being deleted.")
			FileDelete($name)
		EndIf
	Endif
	if not FileExists($name) Then
		;MsgBox(4096,"Status","Installing " & $name)
		FileInstall("res\PathwayBootstrap.ini", "PathwayBootstrap.ini")
	EndIf
EndFunc

Func InstallJavaIfNecessary()
	Local $ver, $path, $latest
	;See http://stackoverflow.com/questions/2951804/how-to-check-java-installation-from-batch-script
	$ver = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Runtime Environment", "CurrentVersion")
	if @error = 0 Then
		;MsgBox(4096,"Status","Java version " & $ver)
		$path = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Runtime Environment\" & $ver, "JavaHome")
		if FileExists( $path & "\bin\java.exe") Then
			return ;Already installed
		EndIf
	EndIf
	;if MsgBox(35,"No Java","Java is used by the Epub validator among other things. It is not installed in your computer. Would you like to install Java?") = 6 Then
	;	LaunchSite("http://java.com/en/download/index.jsp")
	;EndIf
	$latest = IniRead("PathwayBootstrap.Ini", "Versions", "Java", "6u30")
	if @OSArch = "X86" Then
		GetFromUrl("jre-" & $latest & "-windows-i586-s.exe", "http://pathway.sil.org/wp-content/sprint/jre-" & $latest & "-windows-i586-s.exe")
		RunWait("jre-" & $latest & "-windows-i586-s.exe /s /v/qn""ALL IEXPLORER=1 MOZILLA=1 REBOOT=Suppress""")
		CleanUp("jre-" & $latest & "-windows-i586-s.exe")
	Else
		GetFromUrl("jre-" & $latest & "-windows-x64.exe", "http://pathway.sil.org/wp-content/sprint/jre-" & $latest & "-windows-x64.exe")
		RunWait("jre-" & $latest & "-windows-x64.exe /s /v/qn""ALL IEXPLORER=1 MOZILLA=1 REBOOT=Suppress""")
		CleanUp("jre-" & $latest & "-windows-x64.exe")
	Endif
EndFunc

Func InstallLibreOfficeIfNecessary()
	Local $ver, $cmd, $latest
	$ver = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Classes\opendocument.WriterDocument\CurVer", "")
	if @error = 0 Then
		;MsgBox(4096,"Status","Libre Office version " & $ver)
		$cmd = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Classes\" & $ver & "\shell\open\command", "")
		if StringInStr($cmd, "swriter.exe") > 0 Then
			return ;Already installed
		EndIf
	EndIf
	;if MsgBox(35,"No Libre Office","Libre Office is one of the main output destinations. It is not installed in your computer. Would you like to install Libre Office?") = 6 Then
	;	LaunchSite("http://www.libreoffice.org/download/")
	;EndIf
	$latest = IniRead("PathwayBootstrap.Ini", "Versions", "LibreOffice", "3.4.3")
	GetFromUrl("LibO_" & $latest & "_Win_x86_install_multi.exe", "http://download.documentfoundation.org/libreoffice/stable/" & $latest & "/win/x86/LibO_" & $latest & "_Win_x86_install_multi.exe")
	RunWait("LibO_" & $latest & "_Win_x86_install_multi.exe")
	CleanUp("LibO_" & $latest & "_Win_x86_install_multi.exe")
EndFunc

Func InstallPrinceXmlIfNecessary()
	Local $path, $latest
	$path = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Prince_is1", "InstallLocation")
	if @error Then
		$path = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Prince_is1", "InstallLocation")
	EndIf
	if @error = 0 Then
		;MsgBox(4096,"Status","PrinceXml path " & $path)
		if FileExists( $path & "Prince.exe") Then
			return ;Already installed
		EndIf
	EndIf
	;if MsgBox(35,"No PrinceXml","PrinceXml is used to create new previews and is one of the output destinations. It is not installed in your computer. Would you like to install PrinceXml?") = 6 Then
	;	LaunchSite("http://www.princexml.com/download/")
	;EndIf
	$latest = IniRead("PathwayBootstrap.Ini", "Versions", "Prince", "8.0")
	GetFromUrl("prince-" & $latest & "-setup.exe", "http://www.princexml.com/download/prince-" & $latest & "-setup.exe")
	RunWait("prince-" & $latest & "-setup.exe")
	CleanUp("prince-" & $latest & "-setup.exe")
EndFunc

Func InstallPdfReaderIfNecessary()
	Local $ver, $cmd, $latest
	$ver = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.pdf", "")
	if @error = 0 Then
		;MsgBox(4096,"Status","Libre Office version " & $ver)
		$cmd = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Classes\" & $ver & "\shell\open\command", "")
		if @error = 0 Then
			return ;Already installed
		EndIf
	EndIf
	;if MsgBox(35,"No Pdf Reader","The Pdf Reader displays Pdf results after they are produced by various destinations. None installed in your computer. Would you like to install a Pdf Reader?") = 6 Then
	;	LaunchSite("http://get.adobe.com/reader/")
	;EndIf
	$latest = IniRead("PathwayBootstrap.Ini", "Versions", "FoxitReader", "513.1201")
	GetFromUrl("FoxitReader" & $latest & "_enu_Setup.exe", "http://cdn01.foxitsoftware.com/pub/foxit/reader/desktop/win/5.x/5.1/enu/FoxitReader" & $latest & "_enu_Setup.exe")
	RunWait("FoxitReader" & $latest & "_enu_Setup.exe")
	CleanUp("FoxitReader" & $latest & "_enu_Setup.exe")
EndFunc

Func InstallEpubReaderIfNecessary()
	Local $ver, $cmd, $viewer, $latest
	$ver = RegRead("HKEY_CURRENT_USER\Software\Classes\.epub", "")
	if @error = 0 Then
		;MsgBox(4096,"Status","Libre Office version " & $ver)
		$cmd = RegRead("HKEY_CURRENT_USER\SOFTWARE\Classes\" & $ver & "\shell\open\command", "")
		if @error = 0 Then
			return ;Already installed
		EndIf
	EndIf
	;if MsgBox(35,"No Pdf Reader","The Pdf Reader displays Pdf results after they are produced by various destinations. None installed in your computer. Would you like to install a Pdf Reader?") = 6 Then
	;	LaunchSite("http://get.adobe.com/reader/")
	;EndIf
	$latest = IniRead("PathwayBootstrap.Ini", "Versions", "Calibre", "0.8.31")
	GetFromUrl("calibre-" & $latest & ".msi", "http://downloads.sourceforge.net/project/calibre/0.8.31/calibre-" & $latest & ".msi")
	RunWait(@ComSpec & " /c calibre-" & $latest & ".msi")  ;.msi files must be launched from command processor
	CleanUp("calibre-" & $latest & ".msi")
	if @OSArch = "X86" Then
		$viewer = "C:\Program Files\Calibre2\ebook-viewer.exe"
	Else
		$viewer = "C:\Program Files (x86)\Calibre2\ebook-viewer.exe"
	EndIf
	If FileExists($viewer) Then
		RegWrite("HKEY_CURRENT_USER\Software\Classes\.epub", "", "REG_SZ", "ebook-viewer.1")
		RegWrite("HKEY_CURRENT_USER\Software\Classes\ebook-viewer.1")
		RegWrite("HKEY_CURRENT_USER\Software\Classes\ebook-viewer.1\shell")
		RegWrite("HKEY_CURRENT_USER\Software\Classes\ebook-viewer.1\shell\open")
		RegWrite("HKEY_CURRENT_USER\Software\Classes\ebook-viewer.1\shell\open\command", "", "REG_SZ", """" & $viewer & """ ""%1""")
	EndIf
EndFunc

Func InstallXeLaTeXIfNecessary()
	Global $InstallStable
	Local $path, $SaveGlobal, $ver, $latest
	$path = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\SIL\PathwayXeLaTeX", "XeLaTexDir")
	if @error Then
		$path = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\SIL\PathwayXeLaTeX", "XeLaTexDir")
	EndIf
	if @error = 0 Then
		;MsgBox(4096,"Status","XeLaTeX path " & $path)
		if FileExists( $path & "bin\win32\xelatex.exe") Then
			$ver = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\SIL\PathwayXeLaTeX", "XeLaTexVer")
			if @error Then
				$ver = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\SIL\PathwayXeLaTeX", "XeLaTexVer")
			EndIf
			$latest = IniRead("PathwayBootstrap.Ini", "Versions", "XeLaTex", "1.4")
			if $ver = $latest Then
				return ;Already installed
			EndIf
		EndIf
	EndIf
	if MsgBox(35,"No XeLaTeX","XeLaTeX is one of the output destinations (similar to a typesetting system). It is not installed in your computer. Would you like to install XeLaTeX?") = 6 Then
		$SaveGlobal = $InstallStable
		$InstallStable = False
		InstallPathway("SetupXeLaTeXTesting")
		$InstallStable = $SaveGlobal
	EndIf
EndFunc

Func GetFromUrl($name, $url)
	Local $attrib
	$attrib = FileGetAttrib($name)
	if @error = 0 Then
		;MsgBox(4096,"Status",$name & " found.")
		if Not StringInStr($attrib, "R") Then
			;MsgBox(4096,"Status","Old " & $name & " being delted.")
			FileDelete($name)
		EndIf
	Endif
	if not FileExists($name) Then
		;MsgBox(4096,"Status","Downloading " & $urlPath & $name)
		RunWait("wget.exe " & $url)
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