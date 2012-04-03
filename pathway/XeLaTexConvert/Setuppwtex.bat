rem Setuppwtex.bat 9/15/2011 gt - use with install-tl (TexLive) to create the Pathway TexLive installation
set idir=%SVN_DIR%\XeLaTexConvert\XeLaTexExe
if not "%1"=="" set idir=%1
echo # texlive.profile written on Tue Sep 13 10:00:58 2011 UTC>tlpw.profile
echo # It will NOT be updated and reflects only the>>tlpw.profile
echo # installation profile at installation time.>>tlpw.profile
echo selected_scheme scheme-custom>>tlpw.profile
echo TEXDIR %idir%>>tlpw.profile
echo TEXMFCONFIG $TEXMFSYSCONFIG>>tlpw.profile
echo TEXMFHOME $TEXMFLOCAL>>tlpw.profile
echo TEXMFLOCAL %idir%/texmf-local>>tlpw.profile
echo TEXMFSYSCONFIG %idir%/texmf-config>>tlpw.profile
echo TEXMFSYSVAR %idir%/texmf-var>>tlpw.profile
type tlpw2.pro>>tlpw.profile
call install-tl-advanced -profile tlpw.profile
cd %idir%
cd bin\win32
call tlmgr install latex l3kernel l3packages etoolbox
call tlmgr install lm fontspec euenc tipa xkeyval xunicode
call tlmgr install fancyhdr float graphics hanging mdframed oberdiek 
call tlmgr install setspace tools xcolor eso-pic geometry
call tlmgr install xetex xetex-def
call %SVN_DIR%\XeLaTexConvert\hyplst.bat
