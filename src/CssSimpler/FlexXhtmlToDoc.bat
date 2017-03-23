@echo off
REM FlexXhtmlToDoc.bat - 21-Mar-2017 Greg Trihus
set myProg=\SIL\Pathway\Export\CssSimpler.exe
set progDir=C:\Program Files
if exist "%progDir%%myProg%" goto foundIt
set progDir=%ProgramFiles(x86)%
if exist "%progDir%%myProg%" goto foundIt
set progDir=%ProgramFiles%
if exist "%progDir%%myProg%" goto fountIt
echo CssSimpler.exe not found
goto done

:foundIt
@echo on
"%progDir%%myProg%" -femrd %1 %2 %3 %4 %5 %6 %7 %8 %9
set xhtmlfile=%1
set docfile=%xhtmlfile:xhtml=doc%
copy %xhtmlfile% %docfile%
start %docfile%
@echo off
:done
pause