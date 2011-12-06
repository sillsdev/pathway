;License.au3 - 12/2/2011 greg_trihus@sil.org License: LGPL
; Needs: MSCOMCT2.OCX in system32 but it's probably already there
#include <GuiRichEdit.au3>
#include "Options.au3"

Func License($left, $top)
	Local $acceptLicense, $richText, $license, $sil, $pathway, $line, $back, $cancel, $next, $msg
	
	$license = GUICreate("License", 660, 550, $left, $top)
	$sil = GUICtrlCreatePic("sil.jpg", 8, 40, 224, 191, $SS_CENTERIMAGE)
	$pathway = GUICtrlCreatePic("PWIcon1.jpg", 40, 272, 146, 152, $SS_CENTERIMAGE)
	$line = GUICtrlCreateGraphic(20, 444, 600, 2, $SS_BLACKFRAME)
	$back = GUICtrlCreateButton("Back", 328, 464, 87, 28)
	$cancel = GUICtrlCreateButton("Cancel", 432, 464, 87, 28)
	$next = GUICtrlCreateButton("Next", 536, 464, 87, 28)

    $richText = _GUICtrlRichEdit_Create($license, "License", 256, 24, 350, 380, _
            BitOR($ES_MULTILINE, $WS_VSCROLL, $ES_AUTOVSCROLL))
	_GuiCtrlRichEdit_SetText($richText, "")
	_GUICtrlRichEdit_StreamFromFile($richText, @ScriptDir & "\License.rtf")
	_GuiCtrlRichEdit_GotoCharPos($richText, 1)

	$acceptLicense = GUICtrlCreateCheckbox("Yes, I accept the terms of the license", 264, 416, 272, 16)
	GUICtrlSetState($acceptLicense, $GUI_UNCHECKED)
	GUICtrlSetFont($acceptLicense, 10, 400, 0, "Tahoma")
	GUICtrlSetState($next, $GUI_DISABLE)
	;MsgBox(0, "Controls", "AcceptLicense=" & $acceptLicense & " Next=" & $next)

	GUISetState(@SW_SHOW)
	While 1
		$msg = GUIGetMsg()
		Switch $msg
		Case $GUI_EVENT_CLOSE
			Exit
		Case $back
			License_OnBack("Improve", $license)
			ExitLoop
		Case $cancel
			Exit
		Case $next
			License_OnNext("License")
		Case $acceptLicense -1
			License_OnAcceptLicense($acceptLicense, $next)
		Case $acceptLicense
			License_OnAcceptLicense($acceptLicense +1, $next)
		Case Else
			if $msg > 0 Then
				MsgBox(0, "Unrecognized", "Message=" & $msg)
			EndIf
		EndSwitch
	Wend
EndFunc

Func License_OnBack($title, $window)
	GUIDelete($window)
	WinSetState($title, "", @SW_SHOW)
EndFunc

Func License_OnNext($title)
	Local $pos
	$pos = WinGetPos($title)
	WinSetState($title, "", @SW_HIDE)
	Options($pos[0], $pos[1])
EndFunc

Func License_OnAcceptLicense($control, $next)
	;MsgBox(0, "Report", "Control=" & $control & " value=" & GUICtrlRead($control) & " checked=" & $GUI_CHECKED & " unchecked=" & $GUI_UNCHECKED)
	if BitAND(GUICtrlRead($control), $GUI_CHECKED) Then
		GUICtrlSetState($control, $GUI_UNCHECKED)
		GUICtrlSetState($next, $GUI_DISABLE)
	Else
		GUICtrlSetState($control, $GUI_CHECKED)
		GUICtrlSetState($next, $GUI_ENABLE)
	EndIf
EndFunc
