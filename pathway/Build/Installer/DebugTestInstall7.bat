set base=C:\Projects\PublishingSolution
set installBase=C:\Program Files\SIL\Pathway7
if not exist "E:\btai\PublishingSolution" goto notOnE
set base=E:\btai\PublishingSolution
:notOnE
if not exist "C:\svn\btai\PublishingSolution" goto not64
set base=C:\svn\btai\PublishingSolution
set installBase=C:\Program Files (x86)\SIL\Pathway7
:not64
if not exist "C:\svn\google\pathway" goto notPw64
set base=C:\svn\google\pathway
set installBase=C:\Program Files (x86)\SIL\Pathway7
:notPw64
set cfg=\bin\Debug
set src=%BASE%\PsExport%cfg%
if exist %src%\PsExport.dll goto goodCfg
set cfg=\bin\x86\Debug
set src=%base%\PsExport%cfg%
:goodCfg

rem the first line here works with the development version the second, the installed version.
set dst=%installBase%
rem set dst=C:\Progra~1\SIL\FieldWorks

rem Make sure that the new directory exists.
if not exist "%installBase%" mkdir "%installBase%"

xcopy %base%\ConfigurationTool%cfg% "%DST%" /y
xcopy %base%\ParatextSupport%cfg%\ParatextSupport.dll "%DST%" /y
xcopy %base%\ParatextSupport%cfg%\ParatextSupport.pdb "%DST%" /y
xcopy %SRC%\CssDialog.dll "%DST%" /y
xcopy %SRC%\CssDialog.pdb "%DST%" /y
xcopy %SRC%\CSSParser.dll "%DST%" /y
xcopy %SRC%\CSSParser.pdb "%DST%" /y
xcopy %SRC%\PsExport.dll "%DST%" /y
xcopy %SRC%\PsExport.pdb "%DST%" /y
xcopy %SRC%\PsTool.dll "%DST%" /y
xcopy %SRC%\PsTool.pdb "%DST%" /y
xcopy %BASE%\Build\Installer\Pathway_Configuration_Tool_BTE.chm "%DST%\Help" /y
xcopy %SRC%\Antlr3.Runtime.dll "%DST%" /y
xcopy %SRC%\Prince.dll "%DST%" /y
rem xcopy %SRC%\IKVM*.* "%DST%" /y
rem xcopy %SRC%\saxon*.* "%DST%" /y

xcopy %BASE%\PsSupport\*.* "%DST%" /s /q /y

rem Now copy all the Backends to the Pathway directory instead of to a backends directory.
rem xcopy %BASE%\PublishingSolutionExe%cfg%\Backends\*.* "%DST%\PathwaySupport\Backends" /y /q
xcopy %BASE%\ConfigurationTool%cfg%\*Convert.* "%DST%" /y

rem the first line here works with the development version the second, the installed version.
if Exist "C:\Program Files\SIL\FieldWorks 7\Language Explorer\Configuration" goto FwInstalled
if "%FwBase%" == "" goto noFw
xcopy %BASE%\Build\Installer\UtilityCatalogIncludePublishingSolution.xml "%FwBase%\DistFiles\Language Explorer\Configuration" /y
goto noFw
:FwInstalled
xcopy %BASE%\Build\Installer\UtilityCatalogIncludePublishingSolution.xml "C:\Program Files\SIL\FieldWorks 7\Language Explorer\Configuration" /y
:noFw

if not exist "%USERPROFILE%\Application Data\Adobe\InDesign\Version 6.0\en_US\Scripts" goto notxp
xcopy %BASE%\PsSupport\InDesignFiles\Dictionary\Scripts "%USERPROFILE%\Application Data\Adobe\InDesign\Version 6.0\en_US\Scripts" /s/y
:notxp
if not exist "%USERPROFILE%\AppData\Roaming\Adobe\InDesign\Version 6.0\en_US\Scripts" goto not7
xcopy %BASE%\PsSupport\InDesignFiles\Dictionary\Scripts "%USERPROFILE%\AppData\Roaming\Adobe\InDesign\Version 6.0\en_US\Scripts" /s/y
:not7
:done

set pathext=.reg;%pathext%
if not exist "C:\svn\btai\PublishingSolution" goto not64reg
%BASE%\Build\Installer\Pathway7-64.reg
goto regdone
:not64reg
%BASE%\Build\Installer\Pathway7.reg
:regdone
pause
