if exist PsSupport rmdir PsSupport /s /q
xcopy ..\..\..\PsSupport . /i /s /q /y
del *.xpr
del *.txt
del *.dtd
del XLingPap.xsl
del GenericFont.xml
del JobList.xml
del sectionTypes.xml
xcopy ..\..\..\Build\Installer\readme.rtf /y
xcopy ..\..\..\Build\Installer\license.rtf /y
xcopy ..\..\..\OpenOfficeConvert\bin\Debug\OpenOfficeConvert.* Backends /y
xcopy ..\..\..\PdfConvert\bin\Debug\PdfConvert.* Backends /y
xcopy ..\..\..\LiftPrepare\bin\Debug\LiftPrepare.* Backends /y
xcopy ..\..\..\InDesignConvert\bin\Debug\InDesignConvert.* Backends /y
rem if "%1" == "Release" goto removeXetex
rem if "%1" == "ReleaseBTE" goto removeXetex
rem if "%1" == "ReleaseSE" goto removeXetex
xcopy ..\..\..\XeTeXConvert\bin\%1\XeTeXConvert.* Backends /y
goto dogobible
:removeXetex
rmdir /s /q DeTool
rmdir /s /q DEXCTX
rmdir /s /q xetexPathway
:dogobible
if "%1" == "ReleaseSE" goto nogobible
xcopy ..\..\..\GoBibleConvert\bin\%1\GoBibleConvert.* Backends /y
goto done
:nogobible
rmdir /s /q GoBible
del TE_XHTML-to-Phone_XHTML.xslt
del scriptureTemplate.tpl
del ScriptureStyleSettings.xml
:done
pause
