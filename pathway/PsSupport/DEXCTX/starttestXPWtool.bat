echo off
echo .
echo setuptex (to set path to miniCTX) and ...
echo .
call %cd%\miniCTX\setuptex.bat
echo ... typeset XPWtool\testXPWtool.tex (to show XPWtool works OK)
echo .
echo NB: stay in this window
echo .
cd xetexPathway
cmd /k  XPWtool.bat

