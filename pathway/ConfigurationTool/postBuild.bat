if exist BackEnds rmdir BackEnds /s /q
mkdir BackEnds
xcopy ..\..\..\OpenOfficeConvert\bin\%1\OpenOfficeConvert.* .\Backends /y
xcopy ..\..\..\PdfConvert\bin\%1\PdfConvert.* .\Backends /y
xcopy ..\..\..\LiftPrepare\bin\%1\LiftPrepare.* .\Backends /y
xcopy ..\..\..\InDesignConvert\bin\%1\InDesignConvert.* .\Backends /y
if "%1" == "ReleaseSE" goto nogobible
xcopy ..\..\..\GoBibleConvert\bin\%1\GoBibleConvert.* .\Backends /y
:nogobible
xcopy ..\..\..\XeTeXConvert\bin\%1\XeTeXConvert.* .\Backends /y

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
xcopy ..\..\..\PsSupport\styleSettings.xsd . /q /y

