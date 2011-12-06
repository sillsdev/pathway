;Options.au3 - 12/2/2011 greg_trihus@sil.org License: LGPL
#include "Progress.au3"

Func Options($left, $top)
	Local $stable, $latest, $message, $options, $sil, $pathway, $line, $back, $cancel, $next, $msg
	
	$options = GUICreate("Options", 660, 550, $left, $top, 1)
	$sil = GUICtrlCreatePic("sil.jpg", 8, 40, 224, 191, $SS_CENTERIMAGE)
	$pathway = GUICtrlCreatePic("PWIcon1.jpg", 40, 272, 146, 152, $SS_CENTERIMAGE)
	$line = GUICtrlCreateGraphic(20, 444, 600, 2, $SS_BLACKFRAME)
	$back = GUICtrlCreateButton("Back", 328, 464, 87, 28)
	$cancel = GUICtrlCreateButton("Cancel", 432, 464, 87, 28)
	$next = GUICtrlCreateButton("Next", 536, 464, 87, 28)
	$message = GUICtrlCreateLabel("You can get the stable version or the latest version. Which version of Pathway would you like to install?", 256, 24, 350, 180)
	GUICtrlSetFont($message, 14, 400, 0, "Tahoma")
	GUICtrlCreateGroup("Pathway Version Option", 344, 272, 160, 80)
	$stable = GUICtrlCreateRadio("Stable", 384, 296, 64, 16)
	$latest = GUICtrlCreateRadio("Latest", 384, 320, 64, 16)
	GUICtrlCreateGroup("", -99, -99, 1, 1)
	GUICtrlSetState($stable, $GUI_CHECKED)

	GUISetState(@SW_SHOW)
	While 1
		$msg = GUIGetMsg()
		Switch $msg
		Case $GUI_EVENT_CLOSE
			Exit
		Case $back
			Options_OnBack("License", $options)
			ExitLoop
		Case $cancel
			Exit
		Case $next
			Options_OnNext("Options", $stable)
		Case $stable, $latest
		Case Else
			if $msg > 0 Then
				MsgBox(0, "Unrecognized", "Message=" & $msg)
			EndIf
		EndSwitch
	Wend
EndFunc

Func Options_OnBack($title, $window)
	GUIDelete($window)
	WinSetState($title, "", @SW_SHOW)
EndFunc

Func Options_OnNext($title, $stable)
	Local $pos
	Global $InstallStable
	$pos = WinGetPos($title)
	WinSetState($title, "", @SW_HIDE)
	$InstallStable = (BitAND(GUICtrlRead($stable), $GUI_CHECKED) <> 0)
	Progress($pos[0], $pos[1])
EndFunc
