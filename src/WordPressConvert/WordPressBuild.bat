if exist setup.py goto rightLoc
cd ..\..
if exist setup.py goto rightLoc
cd ..
:rightLoc
rem requires Python 2.5 (www.python.org) with modules Py2Exe, lxml
python setup.py
set dst=..\..\DistFiles\Wordpress
if not exist %dst% mkdir %dst%
copy dist\*.* %dst%\*.*
copy *.htt %dst%
copy WordPress.bat %dst%
copy "WordPress site Setup.txt" %dst%
copy site.css %dst%
copy blogExport.py %dst%