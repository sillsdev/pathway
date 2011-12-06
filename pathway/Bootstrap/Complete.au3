;Complete.au3 - 12/2/2011 greg_trihus@sil.org License: LGPL

Func Complete($left, $top)
	Local $helpComplete, $message, $complete, $sil, $pathway, $line, $back, $cancel, $finish, $msg
	
	$complete = GUICreate("Complete", 660, 550, $left, $top, 1)
	$sil = GUICtrlCreatePic("sil.jpg", 8, 40, 224, 191, $SS_CENTERIMAGE)
	$pathway = GUICtrlCreatePic("PWIcon1.jpg", 40, 272, 146, 152, $SS_CENTERIMAGE)
	$line = GUICtrlCreateGraphic(20, 444, 600, 2, $SS_BLACKFRAME)
	$back = GUICtrlCreateButton("Back", 328, 464, 87, 28)
	$cancel = GUICtrlCreateButton("Cancel", 432, 464, 87, 28)
	$finish = GUICtrlCreateButton("Finish", 536, 464, 87, 28)
	$message = GUICtrlCreateLabel("Congradulations! Installation Complete!", 256, 24, 350, 400, $SS_CENTER)
	GUICtrlSetFont($message, 14, 400, 0, "Tahoma")

	GUISetState(@SW_SHOW)
	While 1
		$msg = GUIGetMsg()
		Switch $msg
		Case $GUI_EVENT_CLOSE
			Exit
		Case $back
			Complete_OnBack("Welcome", $complete)
			ExitLoop
		Case $cancel
			Exit
		Case $finish
			Complete_OnFinish("Complete")
		Case Else
			if $msg > 0 Then
				MsgBox(0, "Unrecognized", "Message=" & $msg)
			EndIf
		EndSwitch
	Wend
EndFunc

Func Complete_OnBack($title, $window)
	GUIDelete($window)
	WinSetState($title, "", @SW_SHOW)
EndFunc

Func Complete_OnFinish($title)
	Exit
EndFunc
