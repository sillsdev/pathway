;Advanced.au3 advanced options screen - 3/5/2012 greg_trihus@sil.org
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
#include <ButtonConstants.au3>

Func Advanced($left, $top)
	Global $closeUp
	Global $advanced
	Global $INS_Num, $INS_Size, $INS_DotNet, $INS_Java, $INS_Office, $INS_Epub, $INS_Pdf, $INS_Prince, $INS_XeLaTex, $INS_Dic4MID, $INS_PdfLicense, $INS_YouVersion, $DEL_Installer
	Local $message, $size, $dotnet, $java, $office, $epub, $pdf, $prince, $xelatex, $dic4mid, $pdflicense, $youversion, $delIns, $pathway, $line, $close, $cancel, $msg

	$advanced = GUICreate("License", 660, 550, $left, $top)
	$pathway = GUICtrlCreateIcon("icon.ico", -1, 40, 272, 146, 152, $SS_CENTERIMAGE)
	$line = GUICtrlCreateGraphic(20, 444, 600, 2, $SS_BLACKFRAME)
	$cancel = GUICtrlCreateButton("Cancel", 432, 464, 87, 28)
	$close = GUICtrlCreateButton("Close", 536, 464, 87, 28)
	$message = GUICtrlCreateLabel("After analyzing what is available in your computer, Pathway bootstrap will download and install the following applications.", 256, 24, 350, 124)
	GUICtrlSetFont($message, 14, 400, 0, "Tahoma")
	$dotnet = GUICtrlCreateCheckbox("Microsoft Dotnet 4.0 - " & DotNetSize() & "MB", 264, 150, 340, 28)
	$java = GUICtrlCreateCheckbox("Java runtime (Go Bible, Epub) - 16.4MB", 264, 180, 340, 28)
	$office = GUICtrlCreateCheckbox("Libre Office - 190MB", 264, 210, 340, 28)
	$epub = GUICtrlCreateCheckbox("Epub (e-book reader) - 43.2MB", 264, 240, 340, 28)
	$pdf = GUICtrlCreateCheckbox("PDF reader - 13.8MB", 264, 270, 340, 28)
	$prince = GUICtrlCreateCheckbox("PrinceXML (XHTML to PDF) - 4.0MB", 264, 300, 340, 28)
	$xelatex = GUICtrlCreateCheckbox("XeLaTeX (typesetting) - 32.4MB", 264, 330, 340, 28)
	;$dic4mid = GUICtrlCreateCheckbox("DictionaryForMIDs destination - 2.2MB", 264, 360, 340, 28)
	;$pdflicense = GUICtrlCreateCheckbox("PdfLicenseManager - 3.5MB", 264, 390, 340, 28)
	;$youversion = GUICtrlCreateCheckbox("YouVersion (web pages) - 20.4MB", 264, 360, 340, 28)
	$delIns = GUICtrlCreateCheckbox("Delete installers after using them", 264, 416, 340, 28)

	;$dotnetinfo = GUICtrlCreateButton("info", 594, 150, 32, 32, $BS_ICON, $WS_EX_TRANSPARENT)
	;GUICtrlSetImage( $dotnetinfo, "info.ico", -1, 0)


	GUICtrlSetState($dotnet, $GUI_DISABLE)
	Advanced_SetDefault($INS_DotNet, $dotnet)
	Advanced_SetDefault($INS_Java, $java)
	Advanced_SetDefault($INS_Office, $office)
	Advanced_SetDefault($INS_Epub, $epub)
	Advanced_SetDefault($INS_Pdf, $pdf)
	Advanced_SetDefault($INS_Prince, $prince)
	Advanced_SetDefault($INS_XeLaTex, $xelatex)
	;Advanced_SetDefault($INS_Dic4MID, $dic4mid)
	;Advanced_SetDefault($INS_PdfLicense, $pdflicense)
	;Advanced_SetDefault($INS_YouVersion, $youversion)
	Advanced_SetDefault(True, $delins)

	Advanced_SetTip($dotnet, "Dot net is a Microsoft platform required for Pathway", $INS_DotNet)
	Advanced_SetTip($java, "Java is used to create Go Bible or Dictionary for MID." & @LF & "It is also used to add copyright and license information to PDF files." & @LF & "It is used if you validate the EPUB files.", $INS_Java)
	Advanced_SetTip($office, "Libre Office is the preferred output path for print or PDF publication.", $INS_Office)
	Advanced_SetTip($epub, "Epub readers are available for all platforms" & @LF & "so Epub provides a way to publish one format that can be used in all locations." & @LF & "Pathway can insall a epub reader called Calibre ebook." & @LF & "Calibre can also be used to convert the epub format into Kindle format and load the ebook onto a device.", $INS_Epub)
	Advanced_SetTip($pdf, "Portiable Document Format or PDF is the most widely accepted output format." & @LF & "The only drawback of this format is the page size is fixed." & @LF & "Consquently it doesn't work well for mobile phones." & @LF & "After Pathway creates a PDF, it will use the reader to display the results.", $INS_Pdf)
	Advanced_SetTip($prince, "The PrinceXml program creates a PDF file from the export of Flex." & @LF & "Some users have preferred this output although more effort has been used for the Libre Office output." & @LF & "PrinceXml is also used to create previews for the ConfigurationTool on Windows.", $INS_Prince)
	Advanced_SetTip($xelatex, "XeLaTeX is a very powerful yet free typesetting package. Pathway uses it to create a PDF file." & @LF & "Those familiar with Tex may choose this option for the non-roman or other powerful formatting options.", $INS_XeLaTex)
	;Advanced_SetTip($dic4mid, "DictionaryForMIDs provides a dictionary destination that will run on a feature phone or on an Android phone or tablet with a separate app.", $INS_Dic4MID)
	;Advanced_SetTip($pdflicense, "Pathway will use PdfLicenseManager if installed to add copyright and license information to PDF files.", $INS_PdfLicense)
	;MsgBox(0, "Controls", "AcceptLicense=" & $acceptLicense & " Next=" & $next)

	; Load PNG image
;~ 	_GDIPlus_Startup()
;~ 	GLobal $silImage = _GDIPlus_ImageLoadFromFile("sil.png")
	Global $advancedGraphic = _GDIPlus_GraphicsCreateFromHWND($advanced)
	GUIRegisterMsg($WM_PAINT, "ADVANCED_WM_PAINT")

	GUISetState(@SW_SHOW)
	While $closeUp == False
		$msg = GUIGetMsg()
		Switch $msg
		Case $GUI_EVENT_CLOSE, $cancel
			$closeUp = True
		Case $close
			$INS_Num = 0
			$INS_Size = 0
			$INS_DotNet = Advanced_State($dotnet, DotNetSize())
			$INS_Java = Advanced_State($java, 16.4)
			$INS_Office = Advanced_State($office, 190.0)
			$INS_Epub = Advanced_State($epub, 43.2)
			$INS_Pdf = Advanced_State($pdf, 13.8)
			$INS_Prince = Advanced_State($prince, 4.0)
			$INS_XeLaTex = Advanced_State($xelatex, 111.0)
			;$INS_Dic4MID = Advanced_State($dic4mid, 2.2)
			;$INS_PdfLicense = Advanced_State($pdflicense, 3.5)
			;$INS_YouVersion = Advanced_State($youversion, 20.4)
			$DEL_Installer = Advanced_State($delins, 0)
			GUIRegisterMsg($WM_PAINT, "OPTIONS_WM_PAINT")
			WinSetState("Options", "", @SW_SHOW)
			ExitLoop
		;Case $dotnetinfo
		;	MsgBox(64, "Info", "Dot net is a Microsoft platform required for Pathway")
		Case Else
			if $msg > 0 Then
				;MsgBox(0, "Unrecognized", "Message=" & $msg)
			EndIf
		EndSwitch
	Wend
    ; Clean up resources
	_GDIPlus_GraphicsDispose($advancedGraphic)
;~ 	_GDIPlus_ImageDispose($helpImproveImage)
;~ 	_GDIPlus_Shutdown()
	GUIDelete($advanced)
EndFunc

Func ADVANCED_WM_PAINT($hWnd, $Msg, $wParam, $lParam)
	_WinAPI_RedrawWindow($advanced, 0, 0, $RDW_UPDATENOW)
	_GDIPlus_GraphicsDrawImage($advancedGraphic, $silImage, 52, 40)
	_WinAPI_RedrawWindow($advanced, 0, 0, $RDW_VALIDATE)
	Return $GUI_RUNDEFMSG
EndFunc

Func Advanced_SetDefault($value, $control)
	if $value Then
		GUICtrlSetState($control, $GUI_CHECKED)
	Else
		GUICtrlSetState($control, $GUI_UNCHECKED)
	EndIf
	GUICtrlSetFont($control, 10, 400, 0, "Tahoma")
EndFunc

Func Advanced_State($control, $size)
	Global $INS_Num, $INS_Size
	;MsgBox(0, "Report", "Control=" & $control & " value=" & GUICtrlRead($control) & " checked=" & $GUI_CHECKED & " unchecked=" & $GUI_UNCHECKED)
	if BitAND(GUICtrlRead($control), $GUI_CHECKED) Then
		if $size > 0 Then
			$INS_Num = $INS_Num + 1
			$INS_Size = $INS_Size + $size
		EndIf
		Return True
	Else
		Return False
	EndIf
EndFunc

Func Advanced_SetTip($control, $msg, $INS_flag)
	if not $INS_flag Then
		$msg = $msg & @LF & "It is unchecked because it is already installed."
	EndIf
	GUICtrlSetTip( $control, $msg)
EndFunc

Func DotNetSize()
	; see: http://www.microsoft.com/en-us/download/details.aspx?id=17851
	if @OSArch = "X86" Then
		Return 850
	Else
		Return 2000
	EndIf
EndFunc