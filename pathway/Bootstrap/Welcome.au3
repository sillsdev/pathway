;Welcome.au3 - display welcome message - 12/2/2011 greg_trihus@sil.org
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
#include <GUIConstantsEx.au3>
#include <StaticConstants.au3>
#include <WinAPI.au3>
#include <GDIPlus.au3>
#include <WindowsConstants.au3>
#include "Improve.au3"

Func Welcome()
	Global $closeUp = False
	Global $Bootstrap_version
	Global $welcome
	Local $line, $pathway, $back, $next, $cancel, $message, $version, $msg
	
	Opt("GUIResizeMode", 1)
	
	$welcome = GUICreate("Welcome", 660, 550, 573, 281, 1, $WS_EX_TOPMOST)


	;MsgBox( 4096, "status", "context: " & $dc)

	$pathway = GUICtrlCreateIcon("icon.ico", -1, 40, 272, 146, 152, $SS_CENTERIMAGE)
	$line = GUICtrlCreateGraphic(20, 444, 600, 2, $SS_BLACKFRAME)
	$cancel = GUICtrlCreateButton("Cancel", 432, 464, 87, 28)
	$next = GUICtrlCreateButton("Next", 536, 464, 87, 28)

	$message = GUICtrlCreateLabel("Welcome and thank you for choosing to install Pathway. Pathway will add additional printing and exporting capabilities.", 256, 24, 350, 400)
	GUICtrlSetFont($message, 14, 400, 0, "Tahoma")

	$version = GUICtrlCreateLabel($Bootstrap_version, 0, 0, 350, 28)

	; Load PNG image
	_GDIPlus_Startup()
	GLobal $silImage = _GDIPlus_ImageLoadFromFile("sil.png")
	Global $welcomeGraphic = _GDIPlus_GraphicsCreateFromHWND($welcome)
	GUIRegisterMsg($WM_PAINT, "WELCOME_WM_PAINT")

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
				;MsgBox(0, "Unrecognized", "Message=" & $msg)
			EndIf
		EndSwitch
		
	Wend
    ; Clean up resources
	_GDIPlus_GraphicsDispose($welcomeGraphic)
	_GDIPlus_ImageDispose($silImage)
	_GDIPlus_Shutdown()

	GUIDelete($welcome)
EndFunc

Func WELCOME_WM_PAINT($hWnd, $Msg, $wParam, $lParam)
	_WinAPI_RedrawWindow($welcome, 0, 0, $RDW_UPDATENOW)
	_GDIPlus_GraphicsDrawImage($welcomeGraphic, $silImage, 52, 40)
	_WinAPI_RedrawWindow($welcome, 0, 0, $RDW_VALIDATE)
	Return $GUI_RUNDEFMSG
EndFunc

Func Welcome_OnNext($title)
	Local $pos
	$pos = WinGetPos($title)
	WinSetState($title, "", @SW_HIDE)
	Improve($pos[0], $pos[1])
EndFunc
