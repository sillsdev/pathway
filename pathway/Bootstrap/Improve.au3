;Improve.au3 - 12/2/2011 greg_trihus@sil.org License: LGPL
#include "License.au3"

Func Improve($left, $top)
	Local $helpImprove, $message, $improve, $sil, $pathway, $line, $back, $cancel, $next, $msg
	
	$improve = GUICreate("Improve", 660, 550, $left, $top, 1)
	$sil = GUICtrlCreatePic("sil.jpg", 8, 40, 224, 191, $SS_CENTERIMAGE)
	$pathway = GUICtrlCreatePic("PWIcon1.jpg", 40, 272, 146, 152, $SS_CENTERIMAGE)
	$line = GUICtrlCreateGraphic(20, 444, 600, 2, $SS_BLACKFRAME)
	$back = GUICtrlCreateButton("Back", 328, 464, 87, 28)
	$cancel = GUICtrlCreateButton("Cancel", 432, 464, 87, 28)
	$next = GUICtrlCreateButton("Next", 536, 464, 87, 28)
	$message = GUICtrlCreateLabel("Would you be willing to help imrpove Pathway? If yes, the program will collect data about your usage of the product in order to determine which features are most helpful.", 256, 24, 350, 400)
	GUICtrlSetFont($message, 14, 400, 0, "Tahoma")
	$helpImprove = GUICtrlCreateCheckbox("Yes, I would like to help imrove pathway", 300, 272, 220, 16)
	GUICtrlSetState($helpImprove, $GUI_CHECKED)

	GUISetState(@SW_SHOW)
	While 1
		$msg = GUIGetMsg()
		Switch $msg
		Case $GUI_EVENT_CLOSE
			Exit
		Case $back
			Improve_OnBack("Welcome", $improve)
			ExitLoop
		Case $cancel
			Exit
		Case $next
			Improve_OnNext("Improve")
		Case $helpImprove - 1
			Improve_OnHelpImprove($helpImprove)
		Case Else
			if $msg > 0 Then
				MsgBox(0, "Unrecognized", "Message=" & $msg)
			EndIf
		EndSwitch
	Wend
EndFunc

Func Improve_OnBack($title, $window)
	GUIDelete($window)
	WinSetState($title, "", @SW_SHOW)
EndFunc

Func Improve_OnNext($title)
	Local $pos
	$pos = WinGetPos($title)
	WinSetState($title, "", @SW_HIDE)
	License($pos[0], $pos[1])
EndFunc

Func Improve_OnHelpImprove($control)
    Local $val
	;MsgBox(0, "Report", "Control=" & $control & " value=" & GUICtrlRead($control) & " checked=" & $GUI_CHECKED & " unchecked=" & $GUI_UNCHECKED)
	if BitAND(GUICtrlRead($control), $GUI_CHECKED) Then
		GUICtrlSetState($control, $GUI_UNCHECKED)
	Else
		GUICtrlSetState($control, $GUI_CHECKED)
	EndIf
EndFunc
