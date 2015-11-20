;-----------------------------------------------------------------------------
; Name:        LineSpace24c.au3
; Purpose:     Script ConfigurationTool to create a settings folder for
;               Fixed line height of 24pt.
;              (Edited script created by AutoItRecorder.)
;
; Author:      <greg_trihus@sil.org>
;
; Created:     2013/10/16
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
MouseMove(194,9)
MouseDown("left")
MouseMove(289,9)
MouseUp("left")
MouseClick("left",54,435,1)
MouseClick("left",43,151,1)
MouseClick("left",179,163,1)
MouseClick("left",89,56,1)
Send("{SHIFTDOWN}m{SHIFTUP}y{SHIFTDOWN}c{SHIFTUP}5x2")
MouseClick("left",948,165,1)
MouseClick("left",1079,394,1)
MouseClick("left",1014,450,1)
MouseClick("left",990,422,1)
MouseClick("left",321,52,1)
_WinWaitActivate("Select Your Organization - Scripture","")
MouseMove(187,162)
MouseDown("left")
MouseMove(189,162)
MouseUp("left")
_WinWaitActivate("Set Defaults - Scripture","")
MouseClick("left",180,75,1)
MouseMove(160,367)
MouseDown("left")
MouseMove(160,366)
MouseUp("left")
MouseClick("left",153,102,1)
MouseClick("left",179,181,1)
Send("{SHIFTDOWN}c{SHIFTUP}5x2")
MouseClick("left",147,126,1)
MouseClick("left",53,185,1)
MouseClick("left",145,412,1)
MouseClick("left",179,465,1)
_WinWaitActivate("Pathway Configuration Tool - BTE","")
MouseClick("left",1146,9,1)

#region --- Internal functions Au3Recorder Start ---
Func _WinWaitActivate($title,$text,$timeout=0)
	WinWait($title,$text,$timeout)
	If Not WinActive($title,$text) Then WinActivate($title,$text)
	WinWaitActive($title,$text,$timeout)
EndFunc
#endregion --- Internal functions Au3Recorder End ---
