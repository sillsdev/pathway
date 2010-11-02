set base=..\..\..
set cfg=bin\%1
if exist %base%\ConfigurationTool\postBuild.bat goto anyCpu
set base=..\..\..\..
set cfg=bin\x86\%1
:anyCpu
xcopy %base%\ThirdParty\gsdll32.dll . /y
xcopy %base%\LiftPrepare\Lib\PalasoLib\*.dll . /y
xcopy %base%\PathwayB\%cfg%\PathwayB.* . /y
xcopy %base%\OpenOfficeConvert\%cfg%\OpenOfficeConvert.* . /y
xcopy %base%\LiftPrepare\%cfg%\LiftPrepare.* . /y
xcopy %base%\InDesignConvert\%cfg%\InDesignConvert.* . /y
if "%1" == "CorporateBTE" goto justgobible
if "%1" == "Corporate7BTE" goto justgobible
if "%1" == "CorporateSE" goto nogobible
if "%1" == "Corporate7SE" goto nogobible
xcopy %base%\PdfConvert\%cfg%\PdfConvert.* . /y
xcopy %base%\WordPressConvert\%cfg%\WordPressConvert.* . /y
xcopy %base%\XeTeXConvert\%cfg%\XeTeXConvert.* . /y
xcopy %base%\LogosConvert\%cfg%\LogosConvert.* . /y
if "%1" == "ReleaseSE" goto nogobible
if "%1" == "Release7SE" goto nogobible
:justgobible
xcopy %base%\epubConvert\%cfg%\epubConvert.* . /y
xcopy %base%\GoBibleConvert\%cfg%\GoBibleConvert.* . /y
xcopy %base%\ParatextSupport\%cfg%\ParatextSupport.* . /y
xcopy %base%\PsSupport\ScriptureStyleSettings.xml . /q /y
goto endBible
:nogobible
del TE_XHTML-to-Libronix_MainFile.xslt
del TE_XHTML-to-Libronix_NonScrolling.xslt
del TE_XHTML-to-Libronix_ResourcesFile.xslt
del TE_XHTML-to-Phone_XHTML.xslt
del scriptureTemplate.tpl
del ScriptureStyleSettings.xml
:endBible

if exist styles rmdir styles /s /q
mkdir Styles
xcopy %base%\PsSupport\Styles .\Styles  /i /s /q /y
xcopy %base%\PsSupport\Icons .\Icons  /i /s /q /y
xcopy %base%\PsSupport\Graphic .\Graphic  /i /s /q /y
xcopy %base%\PsSupport\Loc .\Loc  /i /s /q /y
xcopy %base%\PsSupport\Samples .\Samples  /i /s /q /y

mkdir Help
xcopy %base%\Build\Installer\Pathway*.chm .\Help /i /s /q /y

xcopy %base%\PsSupport\DictionaryStyleSettings.xml . /q /y
xcopy %base%\PsSupport\StyleSettings.xml . /q /y
xcopy %base%\PsSupport\StyleSettings.xsd . /q /y


rem for preview
xcopy %base%\PsSupport\previewdll\* . /i /s /q /y
xcopy %base%\PsSupport\*.xhtml . /i /s /q /y

if exist styles rmdir OfficeFiles /s /q
mkdir OfficeFiles

xcopy %base%\PsSupport\OfficeFiles .\OfficeFiles  /i /s /q /y
