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

rem copy backends
xcopy ..\..\..\OpenOfficeConvert\bin\%1\OpenOfficeConvert.* . /y
xcopy ..\..\..\PdfConvert\bin\%1\PdfConvert.* . /y
xcopy ..\..\..\InDesignConvert\bin\%1\InDesignConvert.* . /y
xcopy ..\..\..\WordPressConvert\bin\%1\WordPressConvert.* . /y
xcopy ..\..\..\XeTeXConvert\bin\%1\XeTeXConvert.* . /y

if "%1" == "ReleaseSE" goto nogobible
xcopy ..\..\..\GoBibleConvert\bin\%1\GoBibleConvert.* . /y
goto done
:nogobible
rmdir /s /q GoBible
del TE_XHTML-to-Phone_XHTML.xslt
del scriptureTemplate.tpl
del ScriptureStyleSettings.xml
:done

:end