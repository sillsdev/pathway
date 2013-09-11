;Improve.au3 - Ask user if ok to collect data - 12/2/2011 greg_trihus@sil.org
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
#include "License.au3"

Func Improve($left, $top)
	Global $closeUp
	Local $helpImprove, $message, $improve, $sil, $pathway, $line, $back, $cancel, $next, $msg
	
	$improve = GUICreate("Improve", 660, 550, $left, $top, 1)
	$sil = GUICtrlCreatePic("sil.jpg", 37, 40, 165, 213, $SS_CENTERIMAGE)
	$pathway = GUICtrlCreateIcon("icon.ico", -1, 40, 272, 146, 152, $SS_CENTERIMAGE)
	$line = GUICtrlCreateGraphic(20, 444, 600, 2, $SS_BLACKFRAME)
	$back = GUICtrlCreateButton("Back", 328, 464, 87, 28)
	$cancel = GUICtrlCreateButton("Cancel", 432, 464, 87, 28)
	$next = GUICtrlCreateButton("Next", 536, 464, 87, 28)
	$message = GUICtrlCreateLabel("Would you be willing to help improve Pathway? If yes, the program will collect data about your usage of the product in order to determine which features are most helpful.", 256, 24, 350, 400)
	GUICtrlSetFont($message, 14, 400, 0, "Tahoma")
	$helpImprove = GUICtrlCreateCheckbox("Yes, I would like to help improve pathway", 300, 272)
	if Get_Help_Improve() Then
		GUICtrlSetState($helpImprove, $GUI_CHECKED)
	Else
		GUICtrlSetState($helpImprove, $GUI_UNCHECKED)
	EndIf

	GUISetState(@SW_SHOW)
	While $closeUp == False
		$msg = GUIGetMsg()
		Switch $msg
		Case $GUI_EVENT_CLOSE, $cancel
			$closeUp = True
		Case $back
			WinSetState("Welcome", "", @SW_SHOW)
			ExitLoop
		Case $next
			Improve_OnNext("Improve")
		Case $helpImprove - 1
			Improve_OnHelpImprove($helpImprove)
		Case Else
			if $msg > 0 Then
				;MsgBox(0, "Unrecognized", "Message=" & $msg)
			EndIf
		EndSwitch
	Wend
	GUIDelete($improve)
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
		RegWrite("HKEY_CURRENT_USER\SOFTWARE\SIL\Pathway", "HelpImprove","REG_SZ", "No")
	Else
		GUICtrlSetState($control, $GUI_CHECKED)
		RegWrite("HKEY_CURRENT_USER\SOFTWARE\SIL\Pathway", "HelpImprove","REG_SZ", "Yes")
	EndIf
EndFunc

Func Get_Help_Improve()
	Local $helpImprove
	$helpImprove = RegRead("HKEY_CURRENT_USER\SOFTWARE\Wow6432Node\SIL\Pathway","HelpImprove")
	if @error Then
		$helpImprove = RegRead("HKEY_CURRENT_USER\SOFTWARE\SIL\Pathway","HelpImprove")
	EndIf
	if @error Then
		RegWrite("HKEY_CURRENT_USER\SOFTWARE\SIL\Pathway", "HelpImprove","REG_SZ", "Yes")
	EndIf
	Return $helpImprove == "Yes"
EndFunc