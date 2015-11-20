if exist DistFiles rmdir DistFiles /s /q
set src=..\..\..
if exist %src%\PublishingSolution.sln goto copies
set src=..\..\..\..
:copies
xcopy %src%\..\DistFiles . /i /s /q /y
xcopy %src%\OpenOfficeConvert\bin\%1\OpenOfficeConvert.* . /y
rem xcopy %src%\InDesignConvert\bin\%1\InDesignConvert.* . /y
rem xcopy %src%\PdfConvert\bin\%1\PdfConvert.* . /y
