set base=..\..\..
set cfg=bin\%1
if exist %base%\ConfigurationTool\postBuild.bat goto anyCpu
set base=..\..\..\..
set cfg=bin\x86\%1
:anyCpu
if exist PsSupport rmdir PsSupport /s /q
xcopy %base%\PsSupport . /i /s /q /y
del *.xpr
del *.txt
del *.dtd
del XLingPap.xsl
del GenericFont.xml
del JobList.xml
del sectionTypes.xml
xcopy %base%\Build\Installer\readme.rtf /y
xcopy %base%\Build\Installer\license.rtf /y
xcopy %base%\PathwayB\%cfg%\PathwayB.* . /y
xcopy %base%\OpenOfficeConvert\%cfg%\OpenOfficeConvert.* . /y
xcopy %base%\LiftPrepare\%cfg%\LiftPrepare.* . /y
xcopy %base%\InDesignConvert\%cfg%\InDesignConvert.* . /y
rem if "%1" == "Release" goto removeXetex
rem if "%1" == "ReleaseBTE" goto removeXetex
rem if "%1" == "ReleaseSE" goto removeXetex
rem if "%1" == "Release7BTE" goto removeXetex
rem if "%1" == "Release7SE" goto removeXetex
if "%1" == "CorporateBTE" goto removeXetex
if "%1" == "CorporateSE" goto removeXetex
if "%1" == "Corporate7BTE" goto removeXetex
if "%1" == "Corporate7SE" goto removeXetex
xcopy %base%\XeTeXConvert\%cfg%\XeTeXConvert.* . /y
xcopy %base%\PdfConvert\%cfg%\PdfConvert.* . /y
xcopy %base%\WordPressConvert\%cfg%\WordPressConvert.* . /y
xcopy %base%\LogosConvert\%cfg%\LogosConvert.* . /y
goto dogobible
:removeXetex
rmdir /s /q Wordpress
rmdir /s /q xetexPathway
:dogobible
if "%1" == "CorporateSE" goto nogobible
if "%1" == "Corporate7SE" goto nogobible
if "%1" == "ReleaseSE" goto nogobible
if "%1" == "Release7SE" goto nogobible
xcopy %base%\GoBibleConvert\%cfg%\GoBibleConvert.* . /y
xcopy %base%\ParatextSupport\%cfg%\ParatextSupport.* . /y
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
rem pause
