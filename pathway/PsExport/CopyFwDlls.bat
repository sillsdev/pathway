if "%1" == "ReleaseBTE" goto doit
if "%1" == "ReleaseSE" goto doit
if not "%1" == "Release" goto end
:doit
if not "%FwBase%"=="" goto baseSet
echo Please set FwBase to root of Fieldworks development
goto end
:baseSet
set src=%FwBase%\Output\Debug
rem set src=e:\gtrihus\FW6.0\Output\Debug
if not exist %src%\BasicUtils.dll goto end
copy %src%\BasicUtils.dll ..\..\Dlls\BasicUtils.dll /d
copy %src%\FwUtils.dll ..\..\Dlls\FwUtils.dll /d
copy %src%\XMLUtils.dll ..\..\Dlls\XMLUtils.dll /d
copy %src%\ICSharpCode.SharpZipLib.dll ..\..\Dlls\ICSharpCode.SharpZipLib.dll /d
..\VersionCapture.exe %2
:end