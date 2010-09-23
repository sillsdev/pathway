set base=C:\Projects\PublishingSolution
if not exist "E:\btai\PublishingSolution" goto notOnE
set base=E:\btai\PublishingSolution
:notOnE
if not exist "C:\svn\btai\PublishingSolution" goto not64
set base=C:\svn\btai\PublishingSolution
:not64
set src=%BASE%\PsExport\bin\Debug\
set thirdParty=%BASE%\ThirdParty\
set installBase=C:\Program Files\SIL\Pathway7

rem the first line here works with the development version the second, the installed version.
set dst=%installBase%
rem set dst=C:\Progra~1\SIL\FieldWorks

rem Make sure that the new directory exists.
if not exist "%installBase%" mkdir "%installBase%"

xcopy %base%\ConfigurationTool\bin\Debug "%DST%" /y
xcopy %base%\ParatextSupport\bin\Debug\ParatextSupport.dll "%DST%" /y
xcopy %base%\ParatextSupport\bin\Debug\ParatextSupport.pdb "%DST%" /y
xcopy %SRC%CssDialog.dll "%DST%" /y
xcopy %SRC%CssDialog.pdb "%DST%" /y
xcopy %SRC%CSSParser.dll "%DST%" /y
xcopy %SRC%CSSParser.pdb "%DST%" /y
xcopy %SRC%PsExport.dll "%DST%" /y
xcopy %SRC%PsExport.pdb "%DST%" /y
xcopy %SRC%PsTool.dll "%DST%" /y
xcopy %SRC%PsTool.pdb "%DST%" /y
xcopy %BASE%\Build\Installer\Pathway_Configuration_Tool_BTE.chm "%DST%\Help" /y
xcopy %thirdParty%Antlr3.Runtime.dll "%DST%" /y
xcopy %thirdParty%Prince.dll "%DST%" /y
rem xcopy %SRC%IKVM*.* "%DST%" /y
rem xcopy %SRC%saxon*.* "%DST%" /y

xcopy %BASE%\PsSupport\*.* "%DST%" /s /q /y

rem Now copy all the Backends to the Pathway directory instead of to a backends directory.
rem xcopy %BASE%\PublishingSolutionExe\Bin\Debug\Backends\*.* "%DST%\PathwaySupport\Backends" /y /q
xcopy %BASE%\ConfigurationTool\bin\Debug\*Convert.* "%DST%" /y

xcopy %BASE%\..\XeTeX\xhtml2DEX.xsl "%DST%" /y
rem md %DST%\PathwaySupport\DEXCTX
rem xcopy %BASE%\..\XeTeX\DEXCTX\*.* %DST%\PathwaySupport\DEXCTX /s /q

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
%BASE%\Build\Installer\Pathway7.reg
pause
