;PathwayBootstrap.au3 - main script, setup globals - 12/2/2011 greg_trihus@sil.org
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
#include "Welcome.au3"

;Global $XeLaTexSuffix = 'Testing-1.7.0.3077.msi' - currently installing from Team City
Global $DEL_Installer = True

Opt('MustDeclareVars', 1)

DoUI()

Exit 

Func DoUI()
	Global $DEL_Installer
	
	If not FileExists("sil.png") Then
		FileInstall("res\sil.png", "sil.png")
	EndIf
	If not FileExists("icon.ico") Then
		FileInstall("res\icon.ico", "icon.ico")
	EndIf
	If not FileExists("License.rtf") Then
		FileInstall("res\License.rtf", "License.rtf")
	EndIf
	If not FileExists("PathwayBootstrap.ini") Then
		FileInstall("res\PathwayBootstrap.ini", "PathwayBootstrap.ini")
	EndIf
	If not FileExists("PathwayBuild.ini") Then
		FileInstall("res\PathwayBuild.ini", "PathwayBuild.ini")
	EndIf
	Local $BuildNumber = IniRead("PathwayBuild.ini", "PathwayVersion", "buildNumber", "1.13.4.4657")
	;Global $StableVersionDate = '-0.7.1-2011-04-12'
	Global $Version = "1.13.4"
	Local $asResult = StringRegExp($BuildNumber, "([0-9]+.[0-9]+.[0-9]+.[0-9]+)", 1)
	If @error == 0 Then
		$Version = $asResult[0]
	EndIf
	Global $StableSuffix = '-' & $Version
	Global $LatestSuffix = 'Test-' & $Version
	Global $Bootstrap_version = 'Version ' & $BuildNumber

	If FileExists("License.rtf") Then
		Welcome()
	Else
		MsgBox(4096, "Priviledge error", "Please rerun the bootstrap program with Administrative priviledges.")
	Endif
	$DEL_Installer = True
	CleanUp("sil.png")
	CleanUp("icon.ico")
	CleanUp("License.rtf")
	CleanUp("PathwayBootstrap.ini")
	CleanUp("PathwayBuild.ini")
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

