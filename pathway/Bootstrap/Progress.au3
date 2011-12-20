;Progress.au3 - 12/2/2011 greg_trihus@sil.org License: LGPL
#include "Install.au3"
#include "Complete.au3"
Global $InstallStable = True

Func Progress($left, $top)
	Local $bar, $message, $progress, $sil, $pathway, $line, $back, $cancel, $confirm, $msg
	
	$progress = GUICreate("Progress", 660, 550, $left, $top, 1)
	$sil = GUICtrlCreatePic("sil.jpg", 8, 40, 224, 191, $SS_CENTERIMAGE)
	$pathway = GUICtrlCreatePic("PWIcon1.jpg", 40, 272, 146, 152, $SS_CENTERIMAGE)
	$line = GUICtrlCreateGraphic(20, 444, 600, 2, $SS_BLACKFRAME)
	$back = GUICtrlCreateButton("Back", 328, 464, 87, 28)
	$cancel = GUICtrlCreateButton("Cancel", 432, 464, 87, 28)
	$confirm = GUICtrlCreateButton("Confirm", 536, 464, 87, 28)
	$message = GUICtrlCreateLabel("Ready to Install", 256, 200, 350, 180, $SS_CENTER)
	GUICtrlSetFont($message, 14, 400, 0, "Tahoma")
	$bar = GUICtrlCreateProgress(265, 272, 350, 28)

	GUISetState(@SW_SHOW)
	While 1
		$msg = GUIGetMsg()
		Switch $msg
		Case $GUI_EVENT_CLOSE
			Exit
		Case $back
			Progress_OnBack("Options", $progress)
			ExitLoop
		Case $cancel
			Exit
		Case $confirm
			GUICtrlSetData($message, "Installation Progress...")
			Progress_OnConfirm("Progress", $bar)
		Case Else
			if $msg > 0 Then
				MsgBox(0, "Unrecognized", "Message=" & $msg)
			EndIf
		EndSwitch
	Wend
EndFunc

Func Progress_OnBack($title, $window)
	GUIDelete($window)
	WinSetState($title, "", @SW_SHOW)
EndFunc

Func Progress_OnConfirm($title, $bar)
	DoInstall($bar)
	Local $pos
	$pos = WinGetPos($title)
	WinSetState($title, "", @SW_HIDE)
	Complete($pos[0], $pos[1])
EndFunc