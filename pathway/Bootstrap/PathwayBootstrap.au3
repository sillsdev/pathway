;PathwayBootstrap.au3 - 12/2/2011 greg_trihus@sil.org License: LGPL
#include "Welcome.au3"

Global $StableVersionDate = '-0.7.1-2011-04-12'

Opt('MustDeclareVars', 1)

DoUI()

Func DoUI()
	If not FileExists("sil.jpg") Then
		FileInstall("res\sil.jpg", "sil.jpg")
	EndIf
	If not FileExists("PWIcon1.jpg") Then
		FileInstall("res\PWIcon1.jpg", "PWIcon1.jpg")
	EndIf
	If not FileExists("License.rtf") Then
		FileInstall("res\License.rtf", "License.rtf")
	EndIf
	Welcome()
EndFunc