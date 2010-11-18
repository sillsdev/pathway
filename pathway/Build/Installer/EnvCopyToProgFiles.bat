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
echo.         SVN_DIR -> Subversion directory for the PublishingSolution \
echo.                    project, e.g., c:\svn\btai\PublishingSolution
echo.         FW_HOME -> Fieldworks installation directory, 
echo.                    e.g., C:\Program Files\SIL\FieldWorks 7
echo.         PW_HOME -> Pathway installation directory, 
echo.                    e.g., c:\Program Files\SIL\Pathway 7
echo.
echo. Batch Parameter:
echo. 
echo.  "debug" or "release" : specifies the release to copy over
echo.
echo.--------------------------------------------------------------------------
rem goto end

:envCheck
echo.
echo.==========================================================================
echo.
echo. EnvCopyToProgFiles.bat
echo.
echo. (c) 2010 SIL International, inc.
echo. 
echo. Building: %1
echo. Setting environment variables...
if not exist %SVN_DIR% goto notInEnvVars
set base=%SVN_DIR%
set thirdParty=%BASE%\ThirdParty
if /i "%1"=="debug" goto debugEnv
goto releaseEnv

:debugEnv
echo.
echo. [Debug]
set dst=%PW_HOME%
set installBase=%DST%
set cfg=\bin\Debug
goto startCopy

:releaseEnv
echo.
echo. [Release]
set dst=%FW_HOME%
set installBase=%DST%
set cfg=\bin\Release
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
echo. destination directory: %FW_HOME%
echo. 
echo. Copying files...
echo.--------------------------------------------------------------------------
xcopy %base%\ConfigurationTool%cfg% "%DST%" /y
xcopy %base%\ParatextSupport%cfg%\ParatextSupport.dll "%DST%" /y
xcopy %SRC%\CssDialog.dll "%DST%" /y
xcopy %SRC%\CSSParser.dll "%DST%" /y
xcopy %SRC%\PsExport.dll "%DST%" /y
xcopy %SRC%\PsTool.dll "%DST%" /y
xcopy %BASE%\Build\Installer\Pathway_Configuration_Tool_BTE.chm "%DST%\Help" /y
xcopy %SRC%\Antlr3.Runtime.dll "%DST%" /y
xcopy %SRC%\Prince.dll "%DST%" /y
rem xcopy %SRC%\IKVM*.* "%DST%" /y
rem xcopy %SRC%\saxon*.* "%DST%" /y

if /i "%1"=="debug" goto nopdb
xcopy %base%\ParatextSupport%cfg%\ParatextSupport.pdb "%DST%" /y
xcopy %SRC%\CssDialog.pdb "%DST%" /y
xcopy %SRC%\CSSParser.pdb "%DST%" /y
xcopy %SRC%\PsExport.pdb "%DST%" /y
xcopy %SRC%\PsTool.pdb "%DST%" /y

:nopdb
xcopy %BASE%\PsSupport\*.* "%DST%" /s /q /y

rem Now copy all the Backends to the Pathway directory instead of to a backends directory.
rem xcopy %BASE%\PublishingSolutionExe\Bin\Debug\Backends\*.* "%DST%\PathwaySupport\Backends" /y /q

if /i "%1"=="debug" goto debugConvert
xcopy %BASE%\ConfigurationTool%cfg%7SE\*Convert.* "%DST%" /y
goto :doneConvert

:debugConvert
xcopy %BASE%\ConfigurationTool%cfg%\*Convert.* "%DST%" /y


:doneConvert
rem the first line here works with the development version the second, the installed version.
if Exist "%FW_HOME%\Language Explorer\Configuration" goto FwInstalled
if "%FwBase%" == "" goto noFw
xcopy %BASE%\Build\Installer\UtilityCatalogIncludePublishingSolution.xml "%FwBase%\DistFiles\Language Explorer\Configuration" /y
goto noFw

:FwInstalled
xcopy %BASE%\Build\Installer\UtilityCatalogIncludePublishingSolution.xml "%FW_HOME%\Language Explorer\Configuration" /y
:noFw

if not exist "%USERPROFILE%\Application Data\Adobe\InDesign\Version 6.0\en_US\Scripts" goto notxp
xcopy %BASE%\PsSupport\InDesignFiles\Dictionary\Scripts "%USERPROFILE%\Application Data\Adobe\InDesign\Version 6.0\en_US\Scripts" /s/y

:notxp
if not exist "%USERPROFILE%\AppData\Roaming\Adobe\InDesign\Version 6.0\en_US\Scripts" goto not7
xcopy %BASE%\PsSupport\InDesignFiles\Dictionary\Scripts "%USERPROFILE%\AppData\Roaming\Adobe\InDesign\Version 6.0\en_US\Scripts" /s/y

:not7
:done
echo.
echo. Registering path...
echo.--------------------------------------------------------------------------
set pathext=.reg;%pathext%
if not exist "%SVN_DIR%\PublishingSolution" goto win32reg
%BASE%\Build\Installer\Pathway7-64.reg
goto regdone

:win32reg
%BASE%\Build\Installer\Pathway7.reg

:regdone
echo.
echo. Copy process complete. Have an outstanding day!
echo.
echo.==========================================================================
echo.
:end