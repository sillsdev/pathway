xcopy ..\..\..\PathwayB\bin\%1\PathwayB.* . /y
xcopy ..\..\..\OpenOfficeConvert\bin\%1\OpenOfficeConvert.* . /y
xcopy ..\..\..\LiftPrepare\bin\%1\LiftPrepare.* . /y
xcopy ..\..\..\InDesignConvert\bin\%1\InDesignConvert.* . /y
if "%1" == "CorporateBTE" goto justgobible
if "%1" == "Corporate7BTE" goto justgobible
if "%1" == "CorporateSE" goto nogobible
if "%1" == "Corporate7SE" goto nogobible
xcopy ..\..\..\PdfConvert\bin\%1\PdfConvert.* . /y
xcopy ..\..\..\WordPressConvert\bin\%1\WordPressConvert.* . /y
xcopy ..\..\..\XeTeXConvert\bin\%1\XeTeXConvert.* . /y
xcopy ..\..\..\LogosConvert\bin\%1\LogosConvert.* . /y
if "%1" == "ReleaseSE" goto nogobible
if "%1" == "Release7SE" goto nogobible
:justgobible
xcopy ..\..\..\GoBibleConvert\bin\%1\GoBibleConvert.* . /y
:nogobible

if exist styles rmdir styles /s /q
mkdir Styles
xcopy ..\..\..\PsSupport\Styles .\Styles  /i /s /q /y
xcopy ..\..\..\PsSupport\Icons .\Icons  /i /s /q /y
xcopy ..\..\..\PsSupport\Graphic .\Graphic  /i /s /q /y
xcopy ..\..\..\PsSupport\Loc .\Loc  /i /s /q /y
xcopy ..\..\..\PsSupport\Samples .\Samples  /i /s /q /y

mkdir Help
xcopy ..\..\..\Build\Installer\Pathway*.chm .\Help /i /s /q /y

xcopy ..\..\..\PsSupport\DictionaryStyleSettings.xml . /q /y
xcopy ..\..\..\PsSupport\ScriptureStyleSettings.xml . /q /y
xcopy ..\..\..\PsSupport\StyleSettings.xml . /q /y
xcopy ..\..\..\PsSupport\StyleSettings.xsd . /q /y

