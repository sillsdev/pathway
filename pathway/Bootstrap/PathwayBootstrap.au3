;PathwayBootstrap.au3 - 12/2/2011 greg_trihus@sil.org License: LGPL
#include "Welcome.au3"

;Global $StableVersionDate = '-0.7.1-2011-04-12'
Global $StableVersionDate = ''
Global $LatestSuffix = 'Testing-1.1.4.2376'
Global $DEL_Installer = True
Global $Bootstrap_version = "Version 1.11"

Opt('MustDeclareVars', 1)

DoUI()

Exit 

Func DoUI()
	Global $DEL_Installer
	
	If not FileExists("sil.jpg") Then
		FileInstall("res\sil.jpg", "sil.jpg")
	EndIf
	If not FileExists("PWIcon1.jpg") Then
		FileInstall("res\PWIcon1.jpg", "PWIcon1.jpg")
	EndIf
	If not FileExists("License.rtf") Then
		FileInstall("res\License.rtf", "License.rtf")
	EndIf
	If FileExists("License.rtf") Then
		Welcome()
	Else
		MsgBox(4096, "Priviledge error", "Please rerun the bootstrap program with Administrative priviledges.")
	Endif
	$DEL_Installer = True
	CleanUp("sil.jpg")
	CleanUp("PWIcon1.jpg")
	CleanUp("License.rtf")
	CleanUp("PathwayBootstrap.ini")
EndFunc

Func CleanUp($name)
	if @error or Not $DEL_Installer Then
		Return
	EndIf
	Local $attrib
	$attrib = FileGetAttrib($name)
	if not @error Then
		;MsgBox(4096,"Status",$name & " found.")
		if Not StringInStr($attrib, "R") Then
			;MsgBox(4096,"Status","Old " & $name & " being delted.")
			FileDelete($name)
		EndIf
	Endif
EndFunc

