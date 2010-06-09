echo off
echo .
echo setuptex (to set path to miniCTX) and ...
echo .
call %cd%\miniCTX\setuptex.bat
echo ... typeset DEtool\testDEtool.tex (to show DEtool works OK)
echo .
echo NB: stay in this window
echo .
cd DEtool
cmd /k  DEtool.bat

