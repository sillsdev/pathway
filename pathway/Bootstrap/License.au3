;License.au3 - display license and ask for acceptance - 12/2/2011 greg_trihus@sil.org
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
; Needs: MSCOMCT2.OCX in system32 but it's probably already there
#include <GuiRichEdit.au3>
#include "Options.au3"

Func License($left, $top)
	Global $closeUp
	Global $license
	Local $acceptLicense, $richText, $pathway, $line, $back, $cancel, $next, $msg
	
	$license = GUICreate("License", 660, 550, $left, $top)
	$pathway = GUICtrlCreateIcon("icon.ico", -1, 40, 272, 146, 152, $SS_CENTERIMAGE)
	$line = GUICtrlCreateGraphic(20, 444, 600, 2, $SS_BLACKFRAME)
	$back = GUICtrlCreateButton("Back", 328, 464, 87, 28)
	$cancel = GUICtrlCreateButton("Cancel", 432, 464, 87, 28)
	$next = GUICtrlCreateButton("Next", 536, 464, 87, 28)

    $richText = _GUICtrlRichEdit_Create($license, "License", 256, 24, 350, 380, _
            BitOR($ES_MULTILINE, $WS_VSCROLL, $ES_AUTOVSCROLL))
	_GuiCtrlRichEdit_SetText($richText, "")
	;_GUICtrlRichEdit_StreamFromFile($richText, @ScriptDir & "\License.rtf")
	_GUICtrlRichEdit_StreamFromFile($richText, "License.rtf")
	_GuiCtrlRichEdit_GotoCharPos($richText, 1)

	$acceptLicense = GUICtrlCreateCheckbox("Yes, I accept the terms of the license", 264, 416, 340, 28)
	GUICtrlSetState($acceptLicense, $GUI_UNCHECKED)
	GUICtrlSetFont($acceptLicense, 10, 400, 0, "Tahoma")
	GUICtrlSetState($next, $GUI_DISABLE)
	;MsgBox(0, "Controls", "AcceptLicense=" & $acceptLicense & " Next=" & $next)

	; Load PNG image
;~ 	_GDIPlus_Startup()
;~ 	GLobal $silImage = _GDIPlus_ImageLoadFromFile("sil.png")
	Global $licenseGraphic = _GDIPlus_GraphicsCreateFromHWND($license)
	GUIRegisterMsg($WM_PAINT, "LICENSE_WM_PAINT")

	GUISetState(@SW_SHOW)
	While $closeUp == False
		$msg = GUIGetMsg()
		Switch $msg
		Case $GUI_EVENT_CLOSE, $cancel
			$closeUp = True
		Case $back
			GUIRegisterMsg($WM_PAINT, "IMPROVE_WM_PAINT")
			WinSetState("Improve", "", @SW_SHOW)
			ExitLoop
		Case $next
			License_OnNext("License")
		Case $acceptLicense -1
			License_OnAcceptLicense($acceptLicense, $next)
		Case $acceptLicense
			License_OnAcceptLicense($acceptLicense +1, $next)
		Case Else
			if $msg > 0 Then
				;MsgBox(0, "Unrecognized", "Message=" & $msg)
			EndIf
		EndSwitch
	Wend
    ; Clean up resources
	_GDIPlus_GraphicsDispose($licenseGraphic)
;~ 	_GDIPlus_ImageDispose($helpImproveImage)
;~ 	_GDIPlus_Shutdown()
	GUIDelete($license)
EndFunc

Func LICENSE_WM_PAINT($hWnd, $Msg, $wParam, $lParam)
	_WinAPI_RedrawWindow($license, 0, 0, $RDW_UPDATENOW)
	_GDIPlus_GraphicsDrawImage($licenseGraphic, $silImage, 52, 40)
	_WinAPI_RedrawWindow($license, 0, 0, $RDW_VALIDATE)
	Return $GUI_RUNDEFMSG
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
