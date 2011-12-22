;Welcome.au3 - 12/2/2011 greg_trihus@sil.org License: LGPL
#include <GUIConstantsEx.au3>
#include <StaticConstants.au3>
#include <WindowsConstants.au3>
#include "Improve.au3"

Func Welcome()
	Global $closeUp = False
	Local $welcome, $line, $sil, $pathway, $back, $next, $cancel, $message, $msg
	
	Opt("GUIResizeMode", 1)
	
	$welcome = GUICreate("Welcome", 660, 550, 573, 281, 1, $WS_EX_TOPMOST)
	$sil = GUICtrlCreatePic("sil.jpg", 8, 40, 224, 191, $SS_CENTERIMAGE)
	$pathway = GUICtrlCreatePic("PWIcon1.jpg", 40, 272, 146, 152, $SS_CENTERIMAGE)
	$line = GUICtrlCreateGraphic(20, 444, 600, 2, $SS_BLACKFRAME)
	$cancel = GUICtrlCreateButton("Cancel", 432, 464, 87, 28)
	$next = GUICtrlCreateButton("Next", 536, 464, 87, 28)

	$message = GUICtrlCreateLabel("Welcome and thank you for choosing to install Pathway. Pathway will add additional printing and exporting capabilities.", 256, 24, 350, 400)
	GUICtrlSetFont($message, 14, 400, 0, "Tahoma")

	GUISetState(@SW_SHOW)
	While $closeUp == False
		$msg = GUIGetMsg()
		Switch $msg
		Case $GUI_EVENT_CLOSE, $cancel
			$closeUp = True
		Case $next
			Welcome_OnNext("Welcome")
		Case Else
			if $msg > 0 Then
				MsgBox(0, "Unrecognized", "Message=" & $msg)
			EndIf
		EndSwitch
		
	Wend
	GUIDelete($welcome)
EndFunc

Func Welcome_OnNext($title)
	Local $pos
	$pos = WinGetPos($title)
	WinSetState($title, "", @SW_HIDE)
	Improve($pos[0], $pos[1])
EndFunc
