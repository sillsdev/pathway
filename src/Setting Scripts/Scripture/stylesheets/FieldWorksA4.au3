;-----------------------------------------------------------------------------
; Name:        FieldWorksA4.au3
; Purpose:     Script ConfigurationTool to test A4 stylesheet
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
MouseMove(363,12)
MouseDown("left")
MouseMove(464,9)
MouseUp("left")
MouseClick("left",295,138,1)
MouseClick("left",319,63,1)
_WinWaitActivate("Select Your Organization - Dictionary","")
MouseClick("left",171,164,1)
_WinWaitActivate("Set Defaults - Dictionary","")
MouseClick("left",199,71,1)
MouseClick("left",228,92,1)
MouseClick("left",157,103,1)
MouseClick("left",187,180,1)
Send("{SHIFTDOWN}a{SHIFTUP}4{BACKSPACE}{BACKSPACE}{SHIFTDOWN}f{SHIFTUP}ield{SHIFTDOWN}w{SHIFTUP}orks{SHIFTDOWN}a{SHIFTUP}4")
MouseClick("left",139,134,1)
MouseClick("left",40,182,1)
MouseClick("left",206,130,1)
MouseClick("left",169,249,1)
MouseClick("left",110,404,1)
MouseClick("left",187,462,1)
_WinWaitActivate("Pathway Configuration Tool - BTE","")
MouseClick("left",1149,5,1)

#region --- Internal functions Au3Recorder Start ---
Func _WinWaitActivate($title,$text,$timeout=0)
	WinWait($title,$text,$timeout)
	If Not WinActive($title,$text) Then WinActivate($title,$text)
	WinWaitActive($title,$text,$timeout)
EndFunc
#endregion --- Internal functions Au3Recorder End ---
