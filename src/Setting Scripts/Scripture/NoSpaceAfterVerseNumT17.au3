;-----------------------------------------------------------------------------
; Name:        NoSpaceAfterVerseNumT17.au3
; Purpose:     Script ConfigurationTool to set no space after verse number
;              (Edited script created by AutoItRecorder.)
;
; Author:      <greg_trihus@sil.org>
;
; Created:     2013/12/03
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
MouseMove(221,8)
MouseDown("left")
MouseMove(314,8)
MouseUp("left")
MouseClick("left",73,431,1)
MouseClick("left",41,153,1)
MouseClick("left",232,166,1)
MouseClick("left",72,53,1)
Send("{SHIFTDOWN}n{SHIFTUP}o{SHIFTDOWN}s{SHIFTUP}pace{SHIFTDOWN}a{SHIFTUP}fter{SHIFTDOWN}v{SHIFTUP}erse{SHIFTDOWN}n{SHIFTUP}um")
MouseClick("left",944,168,1)
MouseClick("left",974,554,1)
MouseClick("left",323,55,1)
_WinWaitActivate("Select Your Organization - Scripture","")
MouseClick("left",180,162,1)
_WinWaitActivate("Set Defaults - Scripture","")
MouseClick("left",174,77,1)
MouseClick("left",173,362,1)
MouseClick("left",155,104,1)
MouseClick("left",163,185,1)
Send("{SHIFTDOWN}n{SHIFTUP}o{SHIFTDOWN}s{SHIFTUP}p{SHIFTDOWN}a{SHIFTUP}ft{SHIFTDOWN}v{SHIFTUP}rs{SHIFTDOWN}n{SHIFTUP}um")
MouseClick("left",134,130,1)
MouseClick("left",40,180,1)
MouseClick("left",107,396,1)
MouseClick("left",199,466,1)
_WinWaitActivate("Pathway Configuration Tool - BTE","")
MouseClick("left",1157,9,1)

#region --- Internal functions Au3Recorder Start ---
Func _WinWaitActivate($title,$text,$timeout=0)
	WinWait($title,$text,$timeout)
	If Not WinActive($title,$text) Then WinActivate($title,$text)
	WinWaitActive($title,$text,$timeout)
EndFunc
#endregion --- Internal functions Au3Recorder End ---

