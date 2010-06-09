@ECHO OFF

REM author: Hans Hagen - PRAGMA ADE - Hasselt NL - www.pragma-ade.com

REM Installatie van Minimal ConTeXt
REM
REM Instellingen ten behoeve van CONTEXT (TEX) installatie.
REM Deze file moet worden aangeroepen in de file
REM "autoexec.bat" die te vinden is onder c:\. Voeg aan die
REM file toe:
REM
REM   call c:\tex\setuptex.bat
REM
REM U kunt ook de file setuptex.bat naar c:\ copieren; in
REM dat geval is de aanroep
REM
REM   call setuptex.bat
REM
REM Er kan een (bestaande) 'tex root' worden meegegeven:
REM
REM   call [path]setuptex.bat [tex root]
REM
REM Als het niet goed gaat, controlleer dan of de file
REM attributen goed staan. Voer eventueel eerst het volgende
REM commando uit:
REM
REM   attrib -r *.bat
REM
REM De volledige tex boom dient te worden gecopieerd naar
REM harde schijf. Het texpad wordt hier ingesteld. Eventueel
REM kan setuptex.bat worden aangeroepen met een pad.

:userpath

if "%SETUPTEX%"=="done" goto done

if "%~s1"=="" goto selftest

set TEXPATH=%~s1
if exist "%TEXPATH%texmf\tex\plain\base\plain.tex" goto start

set TEXPATH=%~s1\
if exist "%TEXPATH%texmf\tex\plain\base\plain.tex" goto start

:selftest

set TEXPATH=%~d0%~p0
if exist "%TEXPATH%texmf\tex\plain\base\plain.tex" goto start

set TEXPATH=%~d0%~p0\
if exist "%TEXPATH%texmf\tex\plain\base\plain.tex" goto start

:continue

if "%SETUPTEX%"=="bootstrap" goto guesspath

if "%CD%"=="" goto guesspath

set SETUPTEX=bootstrap

%0 "%CD%"

REM ~ SET TEXPATH=%CD%
REM ~ if exist %TEXPATH%texmf\tex\plain\base\plain.tex goto start

REM ~ SET TEXPATH=%CD%\
REM ~ if exist %TEXPATH%texmf\tex\plain\base\plain.tex goto start

:guesspath

set TEXPATH=r:\tex\
if exist "%TEXPATH%texmf\tex\plain\base\plain.tex" goto start

set TEXPATH=e:\tex\
if exist "%TEXPATH%texmf\tex\plain\base\plain.tex" goto start

set TEXPATH=d:\tex\
if exist "%TEXPATH%texmf\tex\plain\base\plain.tex" goto start

set TEXPATH=c:\tex\
if exist "%TEXPATH%texmf\tex\plain\base\plain.tex" goto start

echo no valid 'tex root' is specified (e.g. c:\mytex)

goto done

:start

REM We use the standard TEXMF directory structure (TDS) and therefore
REM we set up the most important variables here. We could cover this in
REM texmf.cnf but since we don't know what is installed on the system
REM we follow a safe route.

REM The second argument, when given, specified the
REM texmf-project path.

REM binaries & formats
set TEXMFOS=%TEXPATH%texmf-mswin

REM base TeX files & fonts
set TEXMFMAIN=%TEXPATH%texmf
REM ConTeXt
set TEXMFCONTEXT=%TEXPATH%texmf-context
REM cache for LuaTeX
set TEXMFCACHE=%TEXPATH%texmf-cache

REM user additions or modifications
set TEXMFLOCAL=%TEXPATH%texmf-local

REM optional (by/for Hans)
set TEXMFFONTS=%TEXPATH%texmf-fonts
set TEXMFEXTRA=%TEXPATH%texmf-extra
set TEXMFPROJECT=%TEXPATH%texmf-project

set VARTEXMF=%TMP%\texmf-var
set HOMETEXMF=

if not "%CTXDEVTXPATH%"=="" SET CTXDEVTXPATH=
if not "%CTXDEVMPPATH%"=="" SET CTXDEVMPPATH=
if not "%CTXDEVMFPATH%"=="" SET CTXDEVMFPATH=

if not "%CTXDEVPLPATH%"=="" SET CTXDEVPLPATH=
if not "%CTXDEVRBPATH%"=="" SET CTXDEVRBPATH=
if not "%CTXDEVPYPATH%"=="" SET CTXDEVPYPATH=
if not "%CTXDEVJVPATH%"=="" SET CTXDEVJVPATH=

if "%OSFONTDIR%"=="" SET OSFONTDIR=%SYSTEMROOT%/fonts

set TEXMFCNF=%TEXPATH%texmf{-local,-context,}/web2c

if "%2"=="" goto noproject

set TEXMFPROJECT=%TEXPATH%texmf-%2

:noproject

set TEXMF={$TEXMFPROJECT,$TEXMFFONTS,$TEXMFLOCAL,$TEXMFOS,$TEXMFCONTEXT,$TEXMFEXTRA,!!$TEXMFMAIN}
set TEXMFDBS=$TEXMF

REM Om problemen met installeren te voorkomen, coderen we de
REM FORMAT en MEM gebieden hard.

set TEXFORMATS=%TEXMFOS%/web2c{/$engine,}
set MPMEMS=%TEXFORMATS%

REM We voegen *voor* aan het pad de gebieden toe waar de TEX
REM en CONTEXT gerelateerde binaries staan.

if "%SETUPTEX%"=="DONE" goto done

set PATH=%TEXMFOS%\bin;%PATH%

REM Voor de zekerheid voegen we ook het PERL en RUBY pad toe,
REM dit wil namelijk bij de installatie nog wel eens misgaan
REM op een netwerk.

if not "extrapath"=="extrapath" goto nopath

if exist "%TEXMFCONTEXT%\SCRIPTS\PERL\CONTEXT"  set path=%TEXMFCONTEXT%\SCRIPTS\PERL\CONTEXT;%PATH%
if exist "%TEXMFCONTEXT%\SCRIPTS\RUBY\CONTEXT"  set path=%TEXMFCONTEXT%\SCRIPTS\RUBY\CONTEXT;%PATH%

if exist "%TEXMFCONTEXT%\CONTEXT\PERL"   set path=%TEXMFCONTEXT%\CONTEXT\PERL;%PATH%
if exist "%TEXMFCONTEXT%\CONTEXT\PERLTK" set path=%TEXMFCONTEXT%\CONTEXT\PERLTK;%PATH%
if exist "%TEXMFCONTEXT%\CONTEXT\RUBY"   set path=%TEXMFCONTEXT%\CONTEXT\RUBY;%PATH%

if exist "%TEXMFPROJECT%\CONTEXT\PERL"   set path=%TEXMFPROJECT%\CONTEXT\PERL;%PATH%
if exist "%TEXMFPROJECT%\CONTEXT\PERLTK" set path=%TEXMFPROJECT%\CONTEXT\PERLTK;%PATH%
if exist "%TEXMFPROJECT%\CONTEXT\RUBY"   set path=%TEXMFPROJECT%\CONTEXT\RUBY;%PATH%
if exist "%TEXMFPROJECT%\CONTEXT\BIN"    set path=%TEXMFPROJECT%\CONTEXT\BIN;%PATH%

set RUBYLIB=%TEXMFCONTEXT%\SCRIPTS\CONTEXT\RUBY;%RUBYLIB%

:nopath

REM Alleen voor speciale doeleinden:

REM SET EXAMPLEROOT=%VARTEXMF%

REM Dat is alles. We onthouden dat deze file is uitgevoerd.

REM To be sure (set in texmf.cnf):

set TEXINPUTS=
set MPINPUTS=
set MFINPUTS=

set SETUPTEX=done

set CTXMINIMAL=yes

:done

rem done

