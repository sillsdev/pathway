set base2=..\..\..
set cfg2=bin\%1
if exist %base2%\ConfigurationTool\postBuild.bat goto anyCpu
set base2=..\..\..\..
set cfg2=bin\x86\%1
:anyCpu
xcopy %base2%\..\lib\gsdll32.dll . /y
xcopy %base2%\..\DistFiles\epubcheck-3.0.1\* .\epubcheck-3.0.1  /i /s /q /y
xcopy %base2%\LiftPrepare\Lib\PalasoLib\*.dll . /y
xcopy %base2%\HelpImprove\%cfg2%\HelpImprove.* . /y
xcopy %base2%\PathwayB\%cfg2%\PathwayB.* . /y
xcopy %base2%\OpenOfficeWriter\%cfg2%\OpenOfficeWriter.* . /y
xcopy %base2%\OpenOfficeConvert_OLD\%cfg2%\OpenOfficeConvert_OLD.* . /y
xcopy %base2%\LiftPrepare\%cfg2%\LiftPrepare.* . /y
xcopy %base2%\InDesignConvert\%cfg2%\InDesignConvert.* . /y
xcopy %base2%\epubConvert\%cfg2%\epubConvert.* . /y
xcopy %base2%\epubValidator\%cfg2%\epubValidator.* . /y
if exist Help rmdir Help /s /q
mkdir Help
if "%1" == "CorporateBTE" goto justgobible
if "%1" == "Corporate7BTE" goto justgobible
if "%1" == "CorporateSE" goto nogobible
if "%1" == "Corporate7SE" goto nogobible
xcopy %base2%\PdfConvert\%cfg2%\PdfConvert.* . /y
xcopy %base2%\PdfLicense\%cfg2%\PdfLicense.* . /y
xcopy %base2%\ApplyPdfLicenseInfo\%cfg2%\ApplyPdfLicenseInfo.* . /y
xcopy %base2%\DictionaryForMIDsConvert\%cfg2%\DictionaryForMIDsConvert.* . /y
xcopy %base2%\WordPressConvert\%cfg2%\WordPressConvert.* . /y
xcopy %base2%\WordPressConvert\%cfg2%\MySql*.dll . /y
xcopy %base2%\XeTeXConvert\%cfg2%\XeTeXConvert.* . /y
xcopy %base2%\XeLaTeXConvert\%cfg2%\XeLaTeXWriter.* . /y
xcopy %base2%\XeTex\%cfg2%\XeTexWriter.* . /y
xcopy %base2%\XeTex\xetexExe .\xetexExe /i /s /y
if "%1" == "ReleaseSE" goto nogobible
if "%1" == "Release7SE" goto nogobible
xcopy %base2%\YouVersionConvert\%cfg2%\YouVersionConvert.* . /y
xcopy %base2%\CadreBibleConvert\%cfg2%\CadreBibleConvert.* . /y
xcopy %base2%\SwordConvert\%cfg2%\SwordConvert.* . /y
:justgobible
xcopy %base2%\GoBibleConvert\%cfg2%\GoBibleConvert.* . /y
xcopy %base2%\theWordConvert\%cfg2%\theWordConvert.* . /y
xcopy %base2%\theWordConvert\%cfg2%\Devart*.* . /y
xcopy %base2%\..\lib\sqlite3.* . /y
xcopy %base2%\theWordConvert\%cfg2%\*.xml . /y
xcopy %base2%\ParatextSupport\%cfg2%\ParatextSupport.* . /y
xcopy %base2%\..\DistFiles\ScriptureStyleSettings.xml . /q /y
rem xcopy %base2%\BuildPathway\HelpBTE\* .\Help /i /s /q /y
goto endBible
:nogobible
del TE_XHTML-to-Libronix_MainFile.xslt
del TE_XHTML-to-Libronix_NonScrolling.xslt
del TE_XHTML-to-Libronix_ResourcesFile.xslt
del TE_XHTML-to-Phone_XHTML.xslt
del pxhtml2xpw-scr.xsl
del scriptureTemplate.tpl
del ScriptureStyleSettings.xml
rem xcopy %base2%\BuildPathway\HelpSE\* .\Help /i /s /q /y
:endBible

if exist styles rmdir styles /s /q
mkdir Styles
xcopy %base2%\..\DistFiles\Styles .\Styles  /i /s /q /y
xcopy %base2%\..\DistFiles\Icons .\Icons  /i /s /q /y
xcopy %base2%\..\DistFiles\Graphic .\Graphic  /i /s /q /y
xcopy %base2%\..\DistFiles\Loc .\Loc  /i /s /q /y
xcopy %base2%\..\DistFiles\Samples .\Samples  /i /s /q /y
xcopy %base2%\..\DistFiles\DictionaryForMIDs .\DictionaryForMIDs  /i /s /q /y

mkdir Help
xcopy %base2%\Build\Installer\Pathway*.chm .\Help /i /s /q /y

xcopy %base2%\..\DistFiles\DictionaryStyleSettings.xml . /q /y
xcopy %base2%\..\DistFiles\StyleSettings.xml . /q /y
xcopy %base2%\..\DistFiles\StyleSettings.xsd . /q /y


rem for preview
xcopy %base2%\..\DistFiles\previewdll\* . /i /s /q /y
xcopy %base2%\..\DistFiles\*.xhtml . /i /s /q /y

if exist styles rmdir OfficeFiles /s /q
mkdir OfficeFiles

xcopy %base2%\..\DistFiles\OfficeFiles .\OfficeFiles  /i /s /q /y
