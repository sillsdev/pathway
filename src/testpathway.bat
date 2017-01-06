rem testpathway.bat
set savedir=%cd%
cd Test\bin\x86\Debug
"C:\Program Files (x86)\NUnit 2.5.7\bin\net-2.0\nunit-console-x86.exe"  Export\test.dll /out=testOut.txt /err=testErr.txt
cd %savedir%
