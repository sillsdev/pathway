; Te XHTML Export Script
; ======================
;
; OS:     Windows 9x/NT
; Author: Greg Trihus <greg_trihus@sil.org>
;
; Created: 10/21/2013
; Copyright: (c) 2013 SIL International
; License: MIT

$proj = EnvGet("proj")
if $proj = "" Then $proj = "Nkonya"
$Backup = EnvGet("Backup")
if $Backup = "" Then $Backup = "Nkonya 2010-11-04 1419.fwbackup"
$IncOpt = EnvGet("IncOpt")
if $IncOpt = "" Then $IncOpt = "cl"
$InputPath = EnvGet("InputPath")
if $InputPath = "" Then $InputPath = "E:\My FieldWorks\Backups"
$OutputPath = EnvGet("OutputPath")
if $OutputPath = "" Then $OutputPath = "C:\tmp"
;MsgBox, 4, AutoIt Flex XHTML Export, Run Fieldworks and export \"%proj%\" as XHTML. Run?
;IfMsgBox, NO, Goto, denied

$fwHome = EnvGet("FW_HOME")
if not FileExists($fwHome & "\FieldWorks.exe") Then
	$fwHome = RegRead("HKEY_LOCAL_MACHINE\Software\SIL\Fieldworks\7.0", "RootCodeDir")
	if not FileExists($fwHome & "\FieldWorks.exe") Then
		$fwHome = RegRead("HKEY_LOCAL_MACHINE\Software\Wow6432Node\SIL\Fieldworks\7.0", "RootCodeDir")
		if not FileExists($fwHome & "\FieldWorks.exe") Then
			MsgBox(48, "Missing FieldWorks 7", "Unable to find location of FieldWorks 7")
			Exit
		EndIf
	EndIf
EndIf

AdlibRegister("MyAdlib")

MsgBox(4112,"Run Command", $fwHome & "\Fieldworks.exe -app Te -db """ & $proj & """ -restore """ & $InputPath & "\" & $Backup & """ -include " & $IncOpt)
Run($fwHome & "\Fieldworks.exe -app Te -db """ & $proj & """ -restore """ & $InputPath & "\" & $Backup & """ -include " & $IncOpt)
While True
	if WinWaitActive( $proj & " - Translation Editor", "", 200) <> 0 Then
		ExitLoop
	elseif WinWaitActive( $proj & " - FieldWorks Language Explorer", "", 200) <> 0 Then
		MsgBox( 48, "Missing Translation Editor", "Unable to start Translation Editor")
		Send("{ALTDOWN}{F4}{ALTUP}")
		WinWaitClose($proj & " - FieldWorks Language Explorer")
		Exit
	Endif
Wend
Sleep(500)
Send("{ALTDOWN}f{ALTUP}e{DOWN 5}{ENTER}")
WinWaitActive("Export to XHTML")
Sleep(500)
Send("{ALTDOWN}f{ALTUP}{SHIFTDOWN}{HOME}{SHIFTUP}")
Send($OutputPath & "\" & $proj & ".xhtml{ENTER}")
WinWaitActive("[CLASS:CabinetWClass]")
Send("{ALTDOWN}{F4}{ALTUP}")
WinWaitActive($proj & " - Translation Editor","Lexicon")
Send("{ALTDOWN}{F4}{ALTUP}")
WinWaitClose($proj & " - Translation Editor")
Exit

Func MyAdlib()
	if WinActive("Global Writing Systems Changed","writing systems were updated") Then
		Send("{ALTDOWN}n{ALTUP}")
	ElseIf WinActive("Export XHTML", "replace it") Then
		Send("{ALTDOWN}y{ALTUP}")
	EndIf
EndFunc