c:
set path=C:\Windows\Microsoft.NET\Framework\v3.5;%path%
cd C:\svn\pathway
if "%1" == "" goto end
if "%2" == "" goto fullSprint
msbuild BuildPathwaySprint.csproj /p:BUILD_NUMBER=%1 /t:%2
goto end

:fullSprint
msbuild BuildPathwaySprint.csproj /p:BUILD_NUMBER=%1 /t:%2

:end