rem PwTexFiles.bat 9/15/2011 gt - after using setuppwtl.bat this batch creates the files to install
set idir=c:\pwtex
if not "%1"=="" set idir=%1
set dst=..\BuildPathway\Files\pwtex
if not "%2"=="" set dst=%2
mkdir %dst%\temp
echo This is a place holder>%dst%\temp\readme.txt
xcopy %idir%\texmf %dst%\texmf /i /s /q /y
xcopy %idir%\texmf-config %dst%\texmf-config /i /s /q /y
xcopy %idir%\texmf-dist %dst%\texmf-dist /i /s /q /y
xcopy %idir%\texmf-local %dst%\texmf-local /i /s /q /y
xcopy %idir%\texmf-var %dst%\texmf-var /i /s /q /y
mkdir %dst%\bin\win32
xcopy %idir%\bin\win32\fc-cache.exe %dst%\bin\win32
xcopy %idir%\bin\win32\fc-cat.exe %dst%\bin\win32
xcopy %idir%\bin\win32\fc-list.exe %dst%\bin\win32
xcopy %idir%\bin\win32\fc-match.exe %dst%\bin\win32
xcopy %idir%\bin\win32\icudt*.dll %dst%\bin\win32
xcopy %idir%\bin\win32\kpathsea*.dll %dst%\bin\win32
xcopy %idir%\bin\win32\teckit_compile.exe %dst%\bin\win32
xcopy %idir%\bin\win32\tex.dll %dst%\bin\win32
xcopy %idir%\bin\win32\tex.exe %dst%\bin\win32
xcopy %idir%\bin\win32\xdvipdfmx.exe %dst%\bin\win32
xcopy %idir%\bin\win32\xelatex.exe %dst%\bin\win32
xcopy %idir%\bin\win32\xetex.exe %dst%\bin\win32
del %dst%\texmf-var\fonts\cache\*.cache-3
