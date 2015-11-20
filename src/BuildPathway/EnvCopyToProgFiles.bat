echo off
if "%1"=="" goto showHelp
if "%1"=="-h" goto showHelp
goto envCheck
:showHelp
echo.--------------------------------------------------------------------------
echo. EnvCopyToProgFiles.bat
echo.
echo. Purpose: This utility copies DLLs and other needed files from the SVN 
echo.         development directory to either the Pathway or FieldWorks 
echo.         directory. This is similar to the CopyToProgFiles.bat file, but
echo.         uses environment variables and a parameter (debug or release) to
echo.         control the input and output directories.
echo. 
echo. System environment vars:
echo.         SVN_DIR -> Subversion directory for the Pathway \
echo.                    project, e.g., c:\svn\pathway
echo.         FW_HOME -> Fieldworks installation directory, 
echo.                    e.g., C:\Program Files\SIL\FieldWorks 7
echo.         PW_HOME -> Pathway installation directory, 
echo.                    e.g., c:\Program Files\SIL\Pathway7
echo.
echo. Batch Parameters:
echo. 
echo. 1. "debug" or "release" : specifies the release to copy over
echo. 2. "x86" or "any" : specifies the CPU build to be copied over
echo.                     (if not specified, the x86 build will be copied)
echo.
echo.--------------------------------------------------------------------------
rem goto end

:envCheck
echo.
echo.==========================================================================
echo.
echo. EnvCopyToProgFiles.bat
echo.
echo. (c) 2010, 2011 SIL International, inc.
echo. 
echo. Building: %1
echo. Setting environment variables...
if not exist %SVN_DIR% goto notInEnvVars
set base=%SVN_DIR%
set thirdParty=%BASE%\ThirdParty
if /i "%2"=="any" set CPU=
if /i "%2"=="x86" set CPU=\x86
if /i "%2"=="" set CPU=\x86
if /i "%1"=="debug" goto debugEnv
goto releaseEnv

:debugEnv
echo.
echo. [Debug]
set dst=%PW_HOME%
set installBase=%DST%
set cfg=\bin%CPU%\Debug
goto startCopy

:releaseEnv
echo.
echo. [Release]
set dst=%FW_HOME%
set installBase=%DST%
set cfg=\bin%CPU%\Release
goto startCopy

:notInEnvVars
echo * Error: SVN_DIR not specified in your environment variables. 
echo * This batch file relies on the following environment variables:
echo *        SVN_DIR -> top-level Subversion directory, e.g., c:\svn\btai
echo *        FW_HOME -> Fieldworks installation directory, 
echo *                   e.g., C:\Program Files\SIL\FieldWorks 7
echo *        PW_HOME -> Pathway installation directory, 
echo *                   e.g., c:\Program Files\SIL\Pathway 7
goto end

rem Make sure that the new directory exists.
if not exist "%installBase%" mkdir "%installBase%"

:startCopy
set SRC=%BASE%\PsExport%cfg%
echo. base source directory: %base%
echo. destination directory: %DST%
echo. 
echo. Copying files...
echo.--------------------------------------------------------------------------

rem run the postbuild.bat file in the configuration tool - 
rem this stages the files we need in the ConfigurationTool's output directory
cd %BASE%\ConfigurationTool%cfg%
call %BASE%\ConfigurationTool\postBuild.bat debug
cd %BASE%\BuildPathway

rem now copy the files from the ConfigurationTool output directory to the destination folder
xcopy %base%\ConfigurationTool%cfg% "%DST%" /y
xcopy %base%\ConfigurationTool%cfg%\epubcheck-3.0.1\* "%DST%"\epubcheck-3.0.1 /i /s /q /y

rem nuke the extra entries in the Print Via dialog
cd %DST%
del OpenOfficeConvert_OLD.dll
cd %BASE%\BuildPathway

rem ** edb 1/14/2011: these should be taken care of by calling postBuild.bat and copying over the results
rem ** in the code blocks above
rem copy over the Converters as well
rem xcopy %base%\OpenOfficeConvert%cfg%\OpenOfficeConvert.* "%DST%" /y
rem xcopy %base%\LiftPrepare%cfg%\LiftPrepare.* "%DST%" /y
rem xcopy %base%\InDesignConvert%cfg%\InDesignConvert.* "%DST%" /y
rem xcopy %base%\LogosConvert%cfg%\LogosConvert.* "%DST%" /y
rem xcopy %base%\epubConvert%cfg%\epubConvert.* "%DST%" /y
rem xcopy %base%\epubValidator%cfg%\epubValidator.* "%DST%" /y
rem xcopy %base%\PdfConvert%cfg%\PdfConvert.* "%DST%" /y
rem xcopy %base%\WordPressConvert%cfg%\WordPressConvert.* "%DST%" /y
rem xcopy %base%\XeTeXConvert%cfg%\XeTeXConvert.* "%DST%" /y
rem xcopy %base%\GoBibleConvert%cfg%\GoBibleConvert.* "%DST%" /y

xcopy %base%\ParatextSupport%cfg%\ParatextSupport.dll "%DST%" /y
xcopy %SRC%\CssDialog.dll "%DST%" /y
xcopy %SRC%\CSSParser.dll "%DST%" /y
xcopy %SRC%\PsExport.dll "%DST%" /y
xcopy %SRC%\PsTool.dll "%DST%" /y
xcopy %BASE%\BuildPathway\Pathway_Configuration_Tool_BTE.chm "%DST%\Help" /y
xcopy %BASE%\ConfigurationTool%cfg%\Help\* "%DST%Help" /i /s /q /y
xcopy %SRC%\Antlr3.Runtime.dll "%DST%" /y
xcopy %SRC%\Prince.dll "%DST%" /y
rem xcopy %SRC%\IKVM*.* "%DST%" /y
rem xcopy %SRC%\saxon*.* "%DST%" /y

if /i "%1"=="release" goto nopdb
xcopy %base%\ParatextSupport%cfg%\ParatextSupport.pdb "%DST%" /y
xcopy %SRC%\CssDialog.pdb "%DST%" /y
xcopy %SRC%\CSSParser.pdb "%DST%" /y
xcopy %SRC%\PsExport.pdb "%DST%" /y
xcopy %SRC%\PsTool.pdb "%DST%" /y

:nopdb
xcopy %BASE%\..\DistFiles\*.* "%DST%" /s /q /y
xcopy %BASE%\XslProcess%cfg%\XslProcess.* "%DST%" /q /y
xcopy %BASE%\XeTex\xetexExe "%DST%\xetexExe" /i /s /q /y

:doneConvert
rem the first line here works with the development version the second, the installed version.
if Exist "%FW_HOME%\Language Explorer\Configuration" goto FwInstalled
if "%FwBase%" == "" goto noFw
xcopy %BASE%\BuildPathway\UtilityCatalogIncludePublishingSolution.xml "%FwBase%\DistFiles\Language Explorer\Configuration" /y
goto noFw

:FwInstalled
xcopy %BASE%\BuildPathway\UtilityCatalogIncludePublishingSolution.xml "%FW_HOME%\Language Explorer\Configuration" /y
:noFw

if not exist "%USERPROFILE%\Application Data\Adobe\InDesign\Version 6.0\en_US\Scripts" goto notxp
xcopy %BASE%\..\DistFiles\InDesignFiles\Dictionary\Scripts "%USERPROFILE%\Application Data\Adobe\InDesign\Version 6.0\en_US\Scripts" /s/y

:notxp
if not exist "%USERPROFILE%\AppData\Roaming\Adobe\InDesign\Version 6.0\en_US\Scripts" goto not7
xcopy %BASE%\..\DistFiles\InDesignFiles\Dictionary\Scripts "%USERPROFILE%\AppData\Roaming\Adobe\InDesign\Version 6.0\en_US\Scripts" /s/y

:not7
:done
echo.
echo. Registering path...
echo.--------------------------------------------------------------------------
set pathext=.reg;%pathext%
if not "%PROCESSOR_ARCHITECTURE%" == "AMD64" goto win32reg
%BASE%\BuildPathway\Pathway7-64.reg
goto regdone

:win32reg
%BASE%\BuildPathway\Pathway7.reg

:regdone
echo.--------------------------------------------------------------------------
echo.
echo. Copy process completed at %TIME%. 
echo. Have an outstanding day!
echo.
echo.==========================================================================
echo.
:end