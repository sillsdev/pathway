;-----------------------------------------------------------------------------
; Name:        ParatextA4.au3
; Purpose:     Script ConfigurationTool to test Scripture A4 stylesheet
;              (Edited script created by AutoItRecorder.)
;
; Author:      <greg_trihus@sil.org>
;
; Created:     2013/10/18
; Copyright:   (c) 2013 SIL International
; Licence:     <MIT>
;-----------------------------------------------------------------------------
Opt("WinWaitDelay",100)
Opt("WinDetectHiddenText",1)
Opt("MouseCoordMode",0)
$TitleMatchStart = 1
Opt("WinTitleMatchMode", $TitleMatchStart) 

Send("{SHIFTDOWN}")
Run("C:\\Program Files (x86)\\SIL\\Pathway7\\ConfigurationTool.exe")
_WinWaitActivate("Pathway Configuration Tool - BTE","")
Send("{SHIFTUP}")
MouseMove(343,19)
MouseDown("left")
MouseMove(440,20)
MouseUp("left")
MouseClick("left",62,433,1)
MouseClick("left",56,139,1)
MouseClick("left",321,50,1)
_WinWaitActivate("Select Your Organization - Scripture","")
MouseClick("left",182,158,1)
_WinWaitActivate("Set Defaults - Scripture","")
MouseClick("left",221,72,1)
MouseClick("left",221,88,1)
MouseClick("left",158,100,1)
MouseClick("left",174,182,1)
Send("{SHIFTDOWN}t{SHIFTUP}itle")
MouseClick("left",186,260,1)
Send("{SHIFTDOWN}c{SHIFTUP}reator")
MouseClick("left",107,402,1)
MouseClick("left",182,464,1)
_WinWaitActivate("Pathway Configuration Tool - BTE","")
MouseClick("left",1149,11,1)

#region --- Internal functions Au3Recorder Start ---
Func _WinWaitActivate($title,$text,$timeout=0)
	WinWait($title,$text,$timeout)
	If Not WinActive($title,$text) Then WinActivate($title,$text)
	WinWaitActive($title,$text,$timeout)
EndFunc
#endregion --- Internal functions Au3Recorder End ---
