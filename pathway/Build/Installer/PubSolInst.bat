set base=C:\svn\btai\PublishingSolution
set src=%BASE%\PsExport\bin\Debug\

rem if not "%FwBase%"=="" goto baseSet
rem echo Please set FwBase to root of Fieldworks development
rem goto done
rem :baseSet
rem the first line here works with the development version the second, the installed version.
rem set dst=%FwBase%\Output\Debug
if exist C:\Progra~2\SIL\FieldWorks goto Win64
set dst=C:\Progra~1\SIL\FieldWorks
goto dstset
:Win64
set dst=C:\Progra~2\SIL\FieldWorks
:dstset

xcopy %SRC%Antlr3.Runtime.dll %DST% /y
xcopy %SRC%CssDialog.dll %DST% /y
xcopy %SRC%CssDialog.pdb %DST% /y
xcopy %SRC%CSSParser.dll %DST% /y
xcopy %SRC%CSSParser.pdb %DST% /y
xcopy %SRC%Prince.dll %DST% /y
xcopy %SRC%PsExport.dll %DST% /y
xcopy %SRC%PsExport.pdb %DST% /y
xcopy %SRC%PsTool.dll %DST% /y
xcopy %SRC%PsTool.pdb %DST% /y
rem xcopy %SRC%IKVM*.* %DST% /y
rem xcopy %SRC%saxon*.* %DST% /y

rd "%DST%\PathwaySupport" /s/q
md "%DST%\PathwaySupport"
xcopy %BASE%\PsSupport\*.* %DST%\PathwaySupport /s /q
xcopy %BASE%\PublishingSolutionExe\Bin\Debug\*Convert.* %DST% /y /q

xcopy %BASE%\..\XeTeX\xhtml2DEX.xsl %DST%\PathwaySupport /y
rem md %DST%\PathwaySupport\DEXCTX
rem xcopy %BASE%\..\XeTeX\DEXCTX\*.* %DST%\PathwaySupport\DEXCTX /s /q

rem the first line here works with the development version the second, the installed version.
if not exist "%DST%\..\..\DistFiles\Language Explorer\Configuration" goto nodev
xcopy %BASE%\Build\Installer\UtilityCatalogIncludePublishingSolution.xml "%DST%\..\..\DistFiles\Language Explorer\Configuration" /y
goto cnfins
:nodev
xcopy %BASE%\Build\Installer\UtilityCatalogIncludePublishingSolution.xml "%DST%\Language Explorer\Configuration" /y
:cnfins

if not exist "%USERPROFILE%\Application Data\Adobe\InDesign\Version 6.0\en_US\Scripts" goto notxp
xcopy %BASE%\PsSupport\InDesignFiles\Dictionary\Scripts "%USERPROFILE%\Application Data\Adobe\InDesign\Version 6.0\en_US\Scripts" /s/y
:notxp
if not exist "%USERPROFILE%\AppData\Roaming\Adobe\InDesign\Version 6.0\en_US\Scripts" goto not7
xcopy %BASE%\PsSupport\InDesignFiles\Dictionary\Scripts "%USERPROFILE%\AppData\Roaming\Adobe\InDesign\Version 6.0\en_US\Scripts" /s/y
:not7
:done
pause