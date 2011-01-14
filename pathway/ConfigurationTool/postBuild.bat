set base2=..\..\..
set cfg2=bin\%1
if exist %base2%\ConfigurationTool\postBuild.bat goto anyCpu
set base2=..\..\..\..
set cfg2=bin\x86\%1
:anyCpu
xcopy %base2%\ThirdParty\gsdll32.dll . /y
xcopy %base2%\ThirdParty\epubcheck-1.1\* .\epubcheck-1.1  /i /s /q /y
xcopy %base2%\LiftPrepare\Lib\PalasoLib\*.dll . /y
xcopy %base2%\PathwayB\%cfg2%\PathwayB.* . /y
xcopy %base2%\OpenOfficeConvert\%cfg2%\OpenOfficeConvert.* . /y
xcopy %base2%\LiftPrepare\%cfg2%\LiftPrepare.* . /y
xcopy %base2%\InDesignConvert\%cfg2%\InDesignConvert.* . /y
xcopy %base2%\epubConvert\%cfg2%\epubConvert.* . /y
xcopy %base2%\epubValidator\%cfg2%\epubValidator.* . /y
if "%1" == "CorporateBTE" goto justgobible
if "%1" == "Corporate7BTE" goto justgobible
if "%1" == "CorporateSE" goto nogobible
if "%1" == "Corporate7SE" goto nogobible
xcopy %base2%\PdfConvert\%cfg2%\PdfConvert.* . /y
xcopy %base2%\WordPressConvert\%cfg2%\WordPressConvert.* . /y
xcopy %base2%\XeTeXConvert\%cfg2%\XeTeXConvert.* . /y
if "%1" == "ReleaseSE" goto nogobible
if "%1" == "Release7SE" goto nogobible
:justgobible
xcopy %base2%\GoBibleConvert\%cfg2%\GoBibleConvert.* . /y
xcopy %base2%\LogosConvert\%cfg2%\LogosConvert.* . /y
xcopy %base2%\ParatextSupport\%cfg2%\ParatextSupport.* . /y
xcopy %base2%\PsSupport\ScriptureStyleSettings.xml . /q /y
goto endBible
:nogobible
del TE_XHTML-to-Libronix_MainFile.xslt
del TE_XHTML-to-Libronix_NonScrolling.xslt
del TE_XHTML-to-Libronix_ResourcesFile.xslt
del TE_XHTML-to-Phone_XHTML.xslt
del pxhtml2xpw-scr.xsl
del scriptureTemplate.tpl
del ScriptureStyleSettings.xml
:endBible

if exist styles rmdir styles /s /q
mkdir Styles
xcopy %base2%\PsSupport\Styles .\Styles  /i /s /q /y
xcopy %base2%\PsSupport\Icons .\Icons  /i /s /q /y
xcopy %base2%\PsSupport\Graphic .\Graphic  /i /s /q /y
xcopy %base2%\PsSupport\Loc .\Loc  /i /s /q /y
xcopy %base2%\PsSupport\Samples .\Samples  /i /s /q /y

mkdir Help
xcopy %base2%\Build\Installer\Pathway*.chm .\Help /i /s /q /y

xcopy %base2%\PsSupport\DictionaryStyleSettings.xml . /q /y
xcopy %base2%\PsSupport\StyleSettings.xml . /q /y
xcopy %base2%\PsSupport\StyleSettings.xsd . /q /y


rem for preview
xcopy %base2%\PsSupport\previewdll\* . /i /s /q /y
xcopy %base2%\PsSupport\*.xhtml . /i /s /q /y

if exist styles rmdir OfficeFiles /s /q
mkdir OfficeFiles

xcopy %base2%\PsSupport\OfficeFiles .\OfficeFiles  /i /s /q /y
