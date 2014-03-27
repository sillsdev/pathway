;-----------------------------------------------------------------------------
; Name:        RightsSA.au3
; Purpose:     Script ConfigurationTool to set Rights page
;              (Edited script created by AutoItRecorder.)
;
; Author:      <greg_trihus@sil.org>
;
; Created:     2013/11/07
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
MouseMove(368,14)
MouseDown("left")
MouseMove(490,18)
MouseUp("left")
MouseClick("left",335,54,1)
_WinWaitActivate("Select Your Organization - Dictionary","")
MouseClick("left",206,164,1)
_WinWaitActivate("Set Defaults - Dictionary","")
MouseClick("left",165,104,1)
MouseClick("left",184,180,1)
Send("{SHIFTDOWN}t{SHIFTUP}itle")
MouseClick("left",193,258,1)
Send("{SHIFTDOWN}c{SHIFTUP}reator")
MouseClick("left",173,312,1)
Send("{DEL}{DEL}{DEL}{DEL}{DEL}{DEL}{DEL}{DEL}{DEL}{DEL}{DEL}{DEL}{DEL}{DEL}{DEL}{DEL}{DEL}{SHIFTDOWN}j{SHIFTUP}ohn{SPACE}{SHIFTDOWN}d{SHIFTUP}oe")
MouseClick("left",152,134,1)
MouseClick("left",38,183,1)
MouseClick("left",32,261,1)
MouseClick("left",131,408,1)
MouseClick("left",222,123,1)
MouseClick("left",182,249,1)
MouseClick("left",192,466,1)
_WinWaitActivate("Pathway Configuration Tool - BTE","")
MouseClick("left",1155,13,1)

#region --- Internal functions Au3Recorder Start ---
Func _WinWaitActivate($title,$text,$timeout=0)
	WinWait($title,$text,$timeout)
	If Not WinActive($title,$text) Then WinActivate($title,$text)
	WinWaitActive($title,$text,$timeout)
EndFunc
#endregion --- Internal functions Au3Recorder End ---
