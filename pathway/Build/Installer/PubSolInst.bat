set base=E:\btai\PublishingSolution
set src=%BASE%\PsExport\bin\Debug\

if not "%FwBase%"=="" goto baseSet
echo Please set FwBase to root of Fieldworks development
goto done
:baseSet
rem the first line here works with the development version the second, the installed version.
set dst=%FwBase%\Output\Debug
rem set dst=C:\Progra~1\SIL\FieldWorks

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
xcopy %BASE%\PublishingSolutionExe\Bin\Debug\Backends\*.* %DST%\PathwaySupport\Backends /y /q

xcopy %BASE%\..\XeTeX\xhtml2DEX.xsl %DST%\PathwaySupport /y
rem md %DST%\PathwaySupport\DEXCTX
rem xcopy %BASE%\..\XeTeX\DEXCTX\*.* %DST%\PathwaySupport\DEXCTX /s /q

rem the first line here works with the development version the second, the installed version.
xcopy %BASE%\Build\Installer\UtilityCatalogIncludePublishingSolution.xml "%DST%\..\..\DistFiles\Language Explorer\Configuration" /y
rem xcopy %BASE%\Build\Installer\UtilityCatalogIncludePublishingSolution.xml "%DST%\Language Explorer\Configuration" /y

if not exist "%USERPROFILE%\Application Data\Adobe\InDesign\Version 6.0\en_US\Scripts" goto notxp
xcopy %BASE%\PsSupport\InDesignFiles\Dictionary\Scripts "%USERPROFILE%\Application Data\Adobe\InDesign\Version 6.0\en_US\Scripts" /s/y
:notxp
if not exist "%USERPROFILE%\AppData\Roaming\Adobe\InDesign\Version 6.0\en_US\Scripts" goto not7
xcopy %BASE%\PsSupport\InDesignFiles\Dictionary\Scripts "%USERPROFILE%\AppData\Roaming\Adobe\InDesign\Version 6.0\en_US\Scripts" /s/y
:not7
:done
pause