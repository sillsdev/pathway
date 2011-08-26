if exist setup.py goto rightLoc
cd ..\..
if exist setup.py goto rightLoc
cd ..
:rightLoc
rem requires Python (www.python.org) with modules Py2Exe
python setup.py
