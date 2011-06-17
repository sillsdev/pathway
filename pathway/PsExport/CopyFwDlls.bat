if "%1" == "CorporateBTE" goto doit
if "%1" == "CorporateSE" goto doit
if "%1" == "ReleaseBTE" goto doit
if "%1" == "ReleaseSE" goto doit
if "%1" == "Release7BTE" goto doit
if "%1" == "Release7SE" goto doit
if not "%1" == "Release" goto end
:doit
rem Rather than assuming the user is a developer
rem and has a development copy of an old FieldWorks
rem the .dlls needed have been placed in folders and
rem are selected by the SetVersion program that runs at
rem the beginning of the build (when the Configuration 
rem name is set to one of the Release options in Visual
rem Studio). So the copies are not made from the current
rem development of fieldworks.
rem if not "%FwBase%"=="" goto baseSet
rem echo Please set FwBase to root of Fieldworks development
rem goto end
rem :baseSet
rem set src=%FwBase%\Output\Debug
rem rem set src=e:\gtrihus\FW6.0\Output\Debug
rem if not exist %src%\BasicUtils.dll goto end
rem copy %src%\BasicUtils.dll ..\..\Dlls\BasicUtils.dll /d
rem copy %src%\FwUtils.dll ..\..\Dlls\FwUtils.dll /d
rem copy %src%\XMLUtils.dll ..\..\Dlls\XMLUtils.dll /d
rem copy %src%\ICSharpCode.SharpZipLib.dll ..\..\Dlls\ICSharpCode.SharpZipLib.dll /d

rem the version capture logic is also integrated into SetVersion
rem ..\VersionCapture.exe %2
rem rem display version #s for person building installer
rem rem notepad ..\..\..\PsSupport\FieldworksVersions.txt

rem setup paths
set base=..\..\..
set cfg=bin\%1
if exist %base%\PsExport\CopyFwDlls.bat goto anyCpu
set base=..\..\..\..
set cfg=bin\x86\%1
:anyCpu

rem copy backends
xcopy %base%\OpenOfficeWriter\%cfg%\OpenOfficeWriter.* . /y
xcopy %base%\InDesignConvert\%cfg%\InDesignConvert.* . /y
xcopy %base%\epubConvert\%cfg%\epubConvert.* . /y
xcopy %base%\epubValidator\%cfg%\epubValidator.* . /y
rem the epubcheck subdir needs to be copied alongside the epubValidator files
xcopy %base%\ThirdParty\epubcheck-1.2\* .\epubcheck-1.2  /i /s /q /y

xcopy %base%\PathwayB\%cfg%\PathwayB.* . /y
if "%1" == "CorporateBTE" goto justgobible
if "%1" == "Corporate7BTE" goto justgobible
if "%1" == "CorporateSE" goto nogobible
if "%1" == "Corporate7SE" goto nogobible
xcopy %base%\PdfConvert\%cfg%\PdfConvert.* . /y
xcopy %base%\WordPressConvert\%cfg%\WordPressConvert.* . /y
xcopy %base%\XeTeXConvert\%cfg%\XeTeXConvert.* . /y
xcopy %base%\XeTex\%cfg%\XeTexWriter.* . /y
xcopy %base%\XeTex\xetexExe .\xetexExe /i /s /q /y

:justgobible
if "%1" == "ReleaseSE" goto nogobible
xcopy %base%\GoBibleConvert\%cfg%\GoBibleConvert.* . /y
xcopy %base%\YouVersionConvert\%cfg%\YouVersionConvert.* . /y
xcopy %base%\LogosConvert\%cfg%\LogosConvert.* . /y
goto done
:nogobible
rmdir /s /q GoBible
del TE_XHTML-to-Libronix_MainFile.xslt
del TE_XHTML-to-Libronix_NonScrolling.xslt
del TE_XHTML-to-Libronix_ResourcesFile.xslt
del TE_XHTML-to-Phone_XHTML.xslt
del pxhtml2xpw-scr.xsl
del scriptureTemplate.tpl
del ScriptureStyleSettings.xml
:done

:end