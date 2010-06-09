echo off
echo .
echo setuptex (to set path to miniCTX) and ...
echo .
call %cd%\miniCTX\setuptex.bat
echo ... typeset minitest.tex (to show miniCTX works OK)
echo .
echo NB: stay in this window
echo .
cmd /k %cd%\miniRuby\bin\ruby.exe %cd%\miniCTX\texmf-context\scripts\context\ruby\texexec.rb --xtx minitest.tex --batch --once