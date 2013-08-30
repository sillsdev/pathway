;Complete.au3 - Inform user process is complete - 12/2/2011 greg_trihus@sil.org
;Copyright (c) 2013 SIL International(R)
;
;This program is free software: you can redistribute it and/or modify
;it under the terms of the GNU General Public License as published by
;the Free Software Foundation, either version 3 of the License, or
;(at your option) any later version.
;
;This program is distributed in the hope that it will be useful,
;but WITHOUT ANY WARRANTY; without even the implied warranty of
;MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
;GNU General Public License for more details.
;
;You should have received a copy of the GNU General Public License
;along with this program.  If not, see <http://www.gnu.org/licenses/>.
;

Func Complete($left, $top)
	Global $closeUp
	Local $helpComplete, $message, $complete, $sil, $pathway, $line, $back, $cancel, $finish, $msg
	
	$complete = GUICreate("Complete", 660, 550, $left, $top, 1)
	$sil = GUICtrlCreatePic("sil.jpg", 8, 40, 224, 191, $SS_CENTERIMAGE)
	$pathway = GUICtrlCreateIcon("icon.ico", -1, 40, 272, 146, 152, $SS_CENTERIMAGE)
	$line = GUICtrlCreateGraphic(20, 444, 600, 2, $SS_BLACKFRAME)
	$back = GUICtrlCreateButton("Back", 328, 464, 87, 28)
	$cancel = GUICtrlCreateButton("Cancel", 432, 464, 87, 28)
	$finish = GUICtrlCreateButton("Finish", 536, 464, 87, 28)
	$message = GUICtrlCreateLabel("Congratulations! The process is Complete!", 256, 24, 350, 400, $SS_CENTER)
	GUICtrlSetFont($message, 14, 400, 0, "Tahoma")

	GUISetState(@SW_SHOW)
	While $closeUp == False
		$msg = GUIGetMsg()
		Switch $msg
		Case $GUI_EVENT_CLOSE, $cancel, $finish
			$closeUp = True
		Case $back
			WinSetState("Options", "", @SW_SHOW)
			ExitLoop
		Case Else
			if $msg > 0 Then
				;MsgBox(0, "Unrecognized", "Message=" & $msg)
			EndIf
		EndSwitch
	Wend
	GUIDelete($complete)
EndFunc
