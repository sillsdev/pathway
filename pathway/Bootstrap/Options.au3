;Options.au3 - 12/2/2011 greg_trihus@sil.org License: LGPL
#include "Install.au3"
#include "Complete.au3"

Func Options($left, $top)
	Global $closeUp
	Local $stable, $latest, $message, $options, $sil, $pathway, $line, $back, $cancel, $install, $msg
	
	$options = GUICreate("Options", 660, 550, $left, $top, 1)
	$sil = GUICtrlCreatePic("sil.jpg", 8, 40, 224, 191, $SS_CENTERIMAGE)
	$pathway = GUICtrlCreatePic("PWIcon1.jpg", 40, 272, 146, 152, $SS_CENTERIMAGE)
	$line = GUICtrlCreateGraphic(20, 444, 600, 2, $SS_BLACKFRAME)
	$back = GUICtrlCreateButton("Back", 328, 464, 87, 28)
	$cancel = GUICtrlCreateButton("Cancel", 432, 464, 87, 28)
	$install = GUICtrlCreateButton("Install", 536, 464, 87, 28)
	$message = GUICtrlCreateLabel("You can get the stable version or the latest version. Which version of Pathway would you like to install?", 256, 24, 350, 140)
	GUICtrlSetFont($message, 14, 400, 0, "Tahoma")
	GUICtrlCreateGroup("Pathway Version Option", 344, 220, 160, 80)
	$stable = GUICtrlCreateRadio("Stable", 384, 244, 64, 16)
	$latest = GUICtrlCreateRadio("Latest", 384, 268, 64, 16)
	GUICtrlCreateGroup("", -99, -99, 1, 1)
	GUICtrlSetState($stable, $GUI_CHECKED)

	GUISetState(@SW_SHOW)
	While $closeUp == False
		$msg = GUIGetMsg()
		Switch $msg
		Case $GUI_EVENT_CLOSE, $cancel
			$closeUp = True
		Case $back
			WinSetState("License", "", @SW_SHOW)
			ExitLoop
		Case $install
			Options_OnInstall("Options", $stable)
		Case $stable, $latest
		Case Else
			if $msg > 0 Then
				;MsgBox(0, "Unrecognized", "Message=" & $msg)
			EndIf
		EndSwitch
	Wend
	GUIDelete($options)
EndFunc

Func Options_OnInstall($title, $stable)
	Global $InstallStable
	$InstallStable = (BitAND(GUICtrlRead($stable), $GUI_CHECKED) <> 0)
	Local $bar
	$bar = GUICtrlCreateProgress(265, 360, 350, 28)
	DoInstall($bar)
	Local $pos
	$pos = WinGetPos($title)
	WinSetState($title, "", @SW_HIDE)
	Complete($pos[0], $pos[1])
EndFunc