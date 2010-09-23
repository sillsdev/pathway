if "%1" == "ReleaseSE" goto nobible
if "%1" == "Release7SE" goto nobible
xcopy ParatextSupport.* ..\..\..\ConfigurationTool\bin\%1 /y
xcopy ParatextSupport.* ..\..\..\PsExport\bin\%1 /y
xcopy ParatextSupport.* ..\..\..\PublishingSolutionEXE\bin\%1 /y
:nobible