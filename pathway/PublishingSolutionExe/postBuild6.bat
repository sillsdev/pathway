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
xcopy ..\..\..\OpenOfficeConvert\bin\%1\OpenOfficeConvert.* . /y
xcopy ..\..\..\LiftPrepare\bin\%1\LiftPrepare.* . /y
xcopy ..\..\..\InDesignConvert\bin\%1\InDesignConvert.* . /y
rem if "%1" == "Release" goto removeXetex
rem if "%1" == "ReleaseBTE" goto removeXetex
rem if "%1" == "ReleaseSE" goto removeXetex
if "%1" == "CorporateSE" goto removeXetex
if "%1" == "CorporateBTE" goto removeXetex
xcopy ..\..\..\XeTeXConvert\bin\%1\XeTeXConvert.* . /y
xcopy ..\..\..\PdfConvert\bin\%1\PdfConvert.* . /y
xcopy ..\..\..\WordPressConvert\bin\%1\WordPressConvert.* . /y
xcopy ..\..\..\LogosConvert\bin\%1\LogosConvert.* . /y
goto dogobible
:removeXetex
rmdir /s /q Wordpress
rmdir /s /q xetexPathway
:dogobible
if "%1" == "ReleaseSE" goto nogobible
if "%1" == "CorporateSE" goto nogobible
xcopy ..\..\..\GoBibleConvert\bin\%1\GoBibleConvert.* . /y
goto done
:nogobible
rmdir /s /q GoBible
del TE_XHTML-to-Libronix_MainFile.xslt
del TE_XHTML-to-Libronix_NonScrolling.xslt
del TE_XHTML-to-Libronix_ResourcesFile.xslt
del TE_XHTML-to-Phone_XHTML.xslt
del scriptureTemplate.tpl
del ScriptureStyleSettings.xml
:done
pause
