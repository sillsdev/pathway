;Advanced.au3 - 3/5/2012 greg_trihus@sil.org License: LGPL
; Needs: MSCOMCT2.OCX in system32 but it's probably already there
#include <GuiRichEdit.au3>

Func Advanced($left, $top)
	Global $closeUp
	Global $INS_DotNet, $INS_Java, $INS_Office, $INS_Epub, $INS_Pdf, $INS_Prince, $INS_XeLaTex, $INS_YouVersion, $DEL_Installer
	Local $message, $size, $dotnet, $java, $office, $epub, $pdf, $prince, $xelatex, $youversion, $delIns, $advanced, $sil, $pathway, $line, $close, $cancel, $msg
	
	$advanced = GUICreate("License", 660, 550, $left, $top)
	$sil = GUICtrlCreatePic("sil.jpg", 8, 40, 224, 191, $SS_CENTERIMAGE)
	$pathway = GUICtrlCreatePic("PWIcon1.jpg", 40, 272, 146, 152, $SS_CENTERIMAGE)
	$line = GUICtrlCreateGraphic(20, 444, 600, 2, $SS_BLACKFRAME)
	$cancel = GUICtrlCreateButton("Cancel", 432, 464, 87, 28)
	$close = GUICtrlCreateButton("Close", 536, 464, 87, 28)
	$message = GUICtrlCreateLabel("After analyzing what is available in your computer, Pathway bootstrap will download and install the following applications.", 256, 24, 350, 124)
	GUICtrlSetFont($message, 14, 400, 0, "Tahoma")
	if @OSArch = "X86" Then
		$size = "22.4MB"
	Else
		$size = "45.2MB"
	EndIf
	$dotnet = GUICtrlCreateCheckbox("Microsoft Dotnet 2.0 - " & $size, 264, 150, 340, 28)
	$java = GUICtrlCreateCheckbox("Java runtime (Go Bible, Epub) - 16.4MB", 264, 180, 340, 28)
	$office = GUICtrlCreateCheckbox("Libre Office - 190MB", 264, 210, 340, 28)
	$epub = GUICtrlCreateCheckbox("Epub (e-book reader) - 43.2MB", 264, 240, 340, 28)
	$pdf = GUICtrlCreateCheckbox("PDF reader - 13.8MB", 264, 270, 340, 28)
	$prince = GUICtrlCreateCheckbox("PrinceXML (XHTML to PDF) - 4.0MB", 264, 300, 340, 28)
	$xelatex = GUICtrlCreateCheckbox("XeLaTeX (typesetting) - 32.4MB", 264, 330, 340, 28)
	$youversion = GUICtrlCreateCheckbox("YouVersion (web pages) - 20.4MB", 264, 360, 340, 28)
	$delIns = GUICtrlCreateCheckbox("Delete installers after using them", 264, 416, 340, 28)

GUICtrlSetState($dotnet, $GUI_DISABLE)
	Advanced_SetDefault($INS_DotNet, $dotnet)
	Advanced_SetDefault($INS_Java, $java)
	Advanced_SetDefault($INS_Office, $office)
	Advanced_SetDefault($INS_Epub, $epub)
	Advanced_SetDefault($INS_Pdf, $pdf)
	Advanced_SetDefault($INS_Prince, $prince)
	Advanced_SetDefault($INS_XeLaTex, $xelatex)
	Advanced_SetDefault($INS_YouVersion, $youversion)
	Advanced_SetDefault(True, $delins)
	;MsgBox(0, "Controls", "AcceptLicense=" & $acceptLicense & " Next=" & $next)

	GUISetState(@SW_SHOW)
	While $closeUp == False
		$msg = GUIGetMsg()
		Switch $msg
		Case $GUI_EVENT_CLOSE, $cancel
			$closeUp = True
		Case $close
			$INS_DotNet = Advanced_State($dotnet)
			$INS_Java = Advanced_State($java)
			$INS_Office = Advanced_State($office)
			$INS_Epub = Advanced_State($epub)
			$INS_Pdf = Advanced_State($pdf)
			$INS_Prince = Advanced_State($prince)
			$INS_XeLaTex = Advanced_State($xelatex)
			$INS_YouVersion = Advanced_State($youversion)
			$DEL_Installer = Advanced_State($delins)
			WinSetState("Options", "", @SW_SHOW)
			ExitLoop
		Case Else
			if $msg > 0 Then
				;MsgBox(0, "Unrecognized", "Message=" & $msg)
			EndIf
		EndSwitch
	Wend
	GUIDelete($advanced)
EndFunc

Func Advanced_SetDefault($value, $control)
	if $value Then
		GUICtrlSetState($control, $GUI_CHECKED)
	Else
		GUICtrlSetState($control, $GUI_UNCHECKED)
	EndIf
	GUICtrlSetFont($control, 10, 400, 0, "Tahoma")
EndFunc	

Func Advanced_State($control)
	;MsgBox(0, "Report", "Control=" & $control & " value=" & GUICtrlRead($control) & " checked=" & $GUI_CHECKED & " unchecked=" & $GUI_UNCHECKED)
	if BitAND(GUICtrlRead($control), $GUI_CHECKED) Then
		Return True
	Else
		Return False
	EndIf
EndFunc
