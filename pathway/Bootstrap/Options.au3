;Options.au3 - find out which option user prefers - 12/2/2011 greg_trihus@sil.org
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
#include "Advanced.au3"
#include "Install.au3"
#include "Complete.au3"

Func Options($left, $top)
	Global $closeUp
	Global $INS_Num, $INS_Size
	Options_Defaults()
;~ 	Global $INS_YouVersion = Not YouVersionInstalled(20.4)
	Local $stable, $latest, $message, $options, $sil, $pathway, $line, $back, $advanced, $cancel, $install, $msg, $note, $minimal
	
	$options = GUICreate("Options", 660, 550, $left, $top, 1)
	$sil = GUICtrlCreatePic("sil.jpg", 37, 40, 165, 213, $SS_CENTERIMAGE)
	$pathway = GUICtrlCreateIcon("icon.ico", -1, 40, 272, 146, 152, $SS_CENTERIMAGE)
	$line = GUICtrlCreateGraphic(20, 444, 600, 2, $SS_BLACKFRAME)
	$advanced = GUICtrlCreateButton("Advanced", 224, 464, 87, 28)
	$back = GUICtrlCreateButton("Back", 328, 464, 87, 28)
	$cancel = GUICtrlCreateButton("Cancel", 432, 464, 87, 28)
	$install = GUICtrlCreateButton("Install", 536, 464, 87, 28)
	$message = GUICtrlCreateLabel("You can get the stable version or the latest version. Which version of Pathway would you like to install?", 256, 24, 350, 140)
	GUICtrlSetFont($message, 14, 400, 0, "Tahoma")
	GUICtrlCreateGroup("Pathway Version Option", 344, 220, 160, 80)
	$stable = GUICtrlCreateRadio("Stable", 384, 244, 64, 16)
	$latest = GUICtrlCreateRadio("Latest", 384, 268, 64, 16)
	GUICtrlCreateGroup("", -99, -99, 1, 1)
	if Fw73orLater() Then
		GUICtrlSetState($stable, $GUI_DISABLE)
		GUICtrlSetState($latest, $GUI_CHECKED)
		GUICtrlCreateLabel("Fieldworks Version incompatible with stable", 344, 310)
	Else
		GUICtrlSetState($stable, $GUI_CHECKED)
	EndIf
	$note = GUICtrlCreateLabel("NOTE: Bootstrap will install " & $INS_Num & " support program(s) amounting to " & $INS_Size & "MB. (See Advanced button for details.)", 256, 400, 350, 40)
	GUICtrlSetFont($note, 8.5, 400, 0, "Tahoma")
	$minimal = GUICtrlCreateCheckbox( "Minimal install", 506, 375, 100, 25)

	GUISetState(@SW_SHOW)
	While $closeUp == False
		$msg = GUIGetMsg()
		Switch $msg
		Case $GUI_EVENT_CLOSE, $cancel
			$closeUp = True
		Case $back
			WinSetState("License", "", @SW_SHOW)
			ExitLoop
		Case $advanced
			Options_OnAdvanced("Options")
			GUICtrlSetData($note, "NOTE: Bootstrap will install " & $INS_Num & " support program(s) amounting to " & $INS_Size & "MB. (See Advanced button for details.)")
		Case $install
			Options_OnInstall("Options", $stable)
		Case $minimal
			Options_OnMinimal($minimal)
		Case $stable, $latest
		Case Else
			if $msg > 0 Then
				;MsgBox(0, "Unrecognized", "Message=" & $msg)
			EndIf
		EndSwitch
	Wend
	GUIDelete($options)
EndFunc

Func Options_OnMinimal($control)
	if BitAND(GUICtrlRead($control), $GUI_CHECKED) Then
		Options_Minimal()
	Else
		Options_Defaults()
	EndIf
EndFunc

Func Options_Defaults()
	;MsgBox(1, "Status", "Set Defaults")
	Global $INS_Num = 0
	Global $INS_Size = 0
	Global $INS_DotNet = Not DotNetInstalled(DotNetSize())
	Global $INS_Java = Not JavaInstalled(16.4)
	Global $INS_Office = Not OfficeInstalled(190.0)
	Global $INS_Epub = Not EpubInstalled(43.2)
	Global $INS_Pdf = Not PdfInstalled(13.8)
	Global $INS_Prince = Not PrinceInstalled(4.0)
	Global $INS_XeLaTex = Not XeLaTexInstalled(32.4)
EndFunc

Func Options_Minimal()
	;MsgBox(1, "Status", "Set minimal")
	Global $INS_Num = 0
	Global $INS_Size = 0
	Global $INS_DotNet = False
	Global $INS_Java = False
	Global $INS_Office = False
	Global $INS_Epub = False
	Global $INS_Pdf = False
	Global $INS_Prince = False
	Global $INS_XeLaTex = False
EndFunc

Func Options_OnAdvanced($title)
	Local $pos
	$pos = WinGetPos($title)
	WinSetState($title, "", @SW_HIDE)
	Advanced($pos[0], $pos[1])
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