; -- CodeDlg.iss --
;
; This script shows how to insert custom wizard pages into Setup and how to handle
; these pages. Furthermore it shows how to 'communicate' between the [Code] section
; and the regular Inno Setup sections using {code:...} constants. Finally it shows
; how to customize the settings text on the 'Ready To Install' page.

#define MyAppSetupName 'PathwayBootstrap'
#define MyAppVersion '2.0'
;#define use_Libreoffice
;#define use_dotnetfx20
;#define use_dotnetfx20lp

#define MyAppURL "http://build.palaso.org/repository/download/bt405/42328:id/"
#define MyAppExeName "SetupPw7SETesting-1.12.0.4318.msi"
#define MyAppExe1Name "SetupPw7BTETesting-1.10.0.4242.msi"
#define IsExternal ""


[Setup]
AppName={#MyAppSetupName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppSetupName} {#MyAppVersion}
AppCopyright=Copyright © SIL International 2007-2014
VersionInfoVersion={#MyAppVersion}
VersionInfoCompany=SIL International
AppPublisher=SIL International
;AppPublisherURL=http://...
;AppSupportURL=http://...
;AppUpdatesURL=http://...
AppID={{F768F6BA-F164-4599-BC26-DCCFC2F76855}
OutputBaseFilename={#MyAppSetupName}-{#MyAppVersion}
DefaultGroupName={#MyAppSetupName}
DefaultDirName={pf}\{#MyAppSetupName}
UninstallDisplayIcon={app}\PathwayBootstrap.exe
OutputDir=bin
SourceDir=.
AllowNoIcons=yes
;SetupIconFile=PathwayBootstrapIcon
SolidCompression=yes
WizardImageFile=LeftSideBanner.bmp
WizardSmallImageFile=sil.bmp
;MinVersion default value: "0,5.0 (Windows 2000+) if Unicode Inno Setup, else 4.0,4.0 (Windows 95+)"
;MinVersion=0,5.0
PrivilegesRequired=admin
ArchitecturesAllowed=x86 x64 ia64
ArchitecturesInstallIn64BitMode=x64 ia64


[Languages]
Name: "en"; MessagesFile: "compiler:Default.isl"
Name: "de"; MessagesFile: "compiler:Languages\German.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "src\PathwayBootstrap-x64.exe"; DestDir: "{app}"; DestName: "PathwayBootstrap.exe"; Check: IsX64
Source: "src\PathwayBootstrap-IA64.exe"; DestDir: "{app}"; DestName: "PathwayBootstrap.exe"; Check: IsIA64
Source: "src\PathwayBootstrap.exe"; DestDir: "{app}"; Check: not Is64BitInstallMode

[Icons]
Name: "{group}\PathwayBootstrap"; Filename: "{app}\PathwayBootstrap"
Name: "{group}\{cm:UninstallProgram,PathwayBootstrap}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\PathwayBootstrap"; Filename: "{app}\PathwayBootstrap.exe"; Tasks: desktopicon
Name: "{userappdata}\SIL\Pathway\PathwayBootstrap"; Filename: "{app}\PathwayBootstrap.exe"; Tasks: quicklaunchicon


[Registry]
Root: HKCU; Subkey: "Software\My Company"; Flags: uninsdeletekeyifempty
Root: HKCU; Subkey: "Software\My Company\Pathway"; Flags: uninsdeletekey
Root: HKCU; Subkey: "Software\My Company\Pathway\Settings"; ValueType: string; ValueName: "Name"; ValueData: "{code:GetUser|Name}"
Root: HKCU; Subkey: "Software\My Company\Pathway\Settings"; ValueType: string; ValueName: "Company"; ValueData: "{code:GetUser|Company}"
; etc.


#include "scripts\silproducts.iss"    

#include "scripts\products\winversion.iss"
#include "scripts\products\fileversion.iss"
  

[Code]
function InitializeSetup(): Boolean;
var
oldVersion: String;
uninstaller: String;
ErrorCode: Integer;
begin
	//init windows version
	//initwinversion();


if RegKeyExists(HKEY_LOCAL_MACHINE,
'SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{F768F6BA-F164-4599-BC26-DCCFC2F76855}_is1') then
begin
RegQueryStringValue(HKEY_LOCAL_MACHINE,
'SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{F768F6BA-F164-4599-BC26-DCCFC2F76855}_is1',
'DisplayVersion', oldVersion);
if (CompareVersion(oldVersion, '10.0.0.4006') < 0) then
begin
if MsgBox('Version ' + oldVersion + ' of Code Beautifier Collection is already installed. Continue to use this old version?',
mbConfirmation, MB_YESNO) = IDYES then
begin
Result := False;
end
else
begin
RegQueryStringValue(HKEY_LOCAL_MACHINE,
'SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{F768F6BA-F164-4599-BC26-DCCFC2F76855}_is1',
'UninstallString', uninstaller);
ShellExec('runas', uninstaller, '/SILENT', '', SW_HIDE, ewWaitUntilTerminated, ErrorCode);
Result := True;
end;
end
else
begin
MsgBox('Version ' + oldVersion + ' of Code Beautifier Collection is already installed. This installer will exit.',
mbInformation, MB_OK);
Result :=True;
end;
end
else
begin
Result := True;
end;
end;


