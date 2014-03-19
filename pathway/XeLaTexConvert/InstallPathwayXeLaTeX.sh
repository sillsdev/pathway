function install ()
{
cd $HOME
rm -rf PwTex
mkdir -p PwTex/TexLive
cd PwTex/TexLive
wget http://ftp.math.purdue.edu/mirrors/ctan.org/systems/texlive/tlnet/install-tl-unx.tar.gz
tar -xzf install-tl-unx.tar.gz
cd ins*
export TEXLIVE_INSTALL_PREFIX=$HOME/PwTex/
echo i|./install-tl -scheme=scheme-minimal -portable
cp -r /usr/lib/pathway/Paratexmf/* $HOME/PwTex/texmf-local
cd $HOME/PwTex/bin/*
./tlmgr conf texmf TEXMFHOME "$HOME/PwTex"
export TEXBIN=$PWD
./tlmgr install latex l3kernel l3packages etoolbox
./tlmgr install lm fontspec euenc tipa xkeyval xunicode
./tlmgr install fancyhdr float graphics hanging mdframed oberdiek
./tlmgr install setspace tools xcolor eso-pic geometry lettrine
./tlmgr install bidi xetex xetex-def
mkdir -p $HOME/bin
echo export PWDTEMP=\$PWD >$HOME/bin/xelatex
echo cd \$HOME/PwTex/bin/\*  >>$HOME/bin/xelatex
echo export PATH=\$PWD:\$PATH  >>$HOME/bin/xelatex
echo cd \"\$PWDTEMP\"  >>$HOME/bin/xelatex
echo xelatex \$1 \$2 \$3 \$4 >>$HOME/bin/xelatex
chmod 775 $HOME/bin/xelatex
mkdir -p $HOME/.mono/registry/CurrentUser/software/sil/pathwayxelatex
echo import os >value.py
echo print "'<values><value name=\"XELATEXDIR\" type=\"string\">' + os.environ[\"TEXBIN\"] + '</value><value name=\"XeLaTexVer\" type=\"string\">1.9</value></values>'" >>value.py
python value.py >$HOME/.mono/registry/CurrentUser/software/sil/pathwayxelatex/values.xml
./fmtutil --all
./mktexlsr
}

	if [ -d $HOME/PwTex ]; then
		echo -n "Reinstall Pathway XeLaTex?";
		read yorn;
		if [ "$yorn" = "y" ]; then
			install;
		fi
	else
		install;
	fi


