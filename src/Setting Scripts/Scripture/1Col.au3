;-----------------------------------------------------------------------------
; Name:        1Col.au3
; Purpose:     Script ConfigurationTool to set custom margins
;              (Edited script created by AutoItRecorder.)
;
; Author:      <greg_trihus@sil.org>
;
; Created:     2013/10/30
; Copyright:   (c) 2013 SIL International
; Licence:     <MIT>
;-----------------------------------------------------------------------------
Opt("WinWaitDelay",100)
Opt("WinDetectHiddenText",1)
Opt("MouseCoordMode",0)
$TitleMatchStart = 1
Opt("WinTitleMatchMode", $TitleMatchStart) 

Send("{SHIFTDOWN}")
Run("C:\Program Files (x86)\SIL\Pathway7\ConfigurationTool.exe")
_WinWaitActivate("Pathway Configuration Tool - BTE","")
Send("{SHIFTUP}")
MouseMove(380,13)
MouseDown("left")
MouseMove(479,16)
MouseUp("left")
MouseClick("left",51,429,1)
MouseClick("left",52,156,1)
MouseClick("left",301,189,1)
MouseClick("left",78,41,1)
Send("{SHIFTDOWN}m{SHIFTUP}y{SHIFTDOWN}s{SHIFTUP}tyle")
MouseClick("left",953,168,1)
MouseClick("left",1030,276,1)
MouseClick("left",1030,276,1)
MouseClick("left",1030,276,1)
MouseClick("left",1030,291,1)
MouseClick("left",319,46,1)
_WinWaitActivate("Select Your Organization - Scripture","")
MouseClick("left",181,167,1)
_WinWaitActivate("Set Defaults - Scripture","")
MouseClick("left",217,71,1)
MouseClick("left",119,367,1)
MouseClick("left",136,104,1)
MouseClick("left",200,185,1)
Send("{SHIFTDOWN}t{SHIFTUP}est{SPACE}1{SPACE}col{TAB}desc{TAB}")
MouseClick("left",142,127,1)
MouseClick("left",32,179,1)
MouseClick("left",215,130,1)
MouseClick("left",114,397,1)
MouseClick("left",194,464,1)
_WinWaitActivate("Pathway Configuration Tool - BTE","")
MouseClick("left",1153,13,1)

#region --- Internal functions Au3Recorder Start ---
Func _WinWaitActivate($title,$text,$timeout=0)
	WinWait($title,$text,$timeout)
	If Not WinActive($title,$text) Then WinActivate($title,$text)
	WinWaitActive($title,$text,$timeout)
EndFunc
#endregion --- Internal functions Au3Recorder End ---
