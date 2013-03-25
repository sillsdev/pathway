	if [ -d $HOME/PwTex ]; then
		echo -n "Reinstall Pathway XeLaTex?";
		read yorn;
		if [ "$yorn" = "y" ]; then
			install;
		fi
	else
		install;
	fi

function install ()
{
cd $HOME
rm -rf PwTex
mkdir -p PwTex/TexLive
cd PwTex/TexLive
wget http://ftp.math.purdue.edu/mirrors/ctan.org/systems/texlive/tlnet/install-tl-unx.tar.gz
tar -xzf install-tl-unx.tar.gz
cd ins*
cd bin/i*
export TEXLIVE_INSTALL_PREFIX=$HOME/PwTex/
echo i|./install-tl -scheme=scheme-minimal -portable
cp -r /usr/lib/pathway/Paratexmf/* $HOME/PwTex/texmf
cd $HOME/PwTex/bin/*
./tlmgr install latex l3kernel l3packages etoolbox
./tlmgr install lm fontspec euenc tipa xkeyval xunicode
./tlmgr install fancyhdr float graphics hanging mdframed oberdiek
./tlmgr install setspace tools xcolor eso-pic geometry lettrine
./tlmgr install xetex xetex-def
mkdir -p $HOME/bin
echo export PWDTEMP=\$PWD >$HOME/bin/xelatex
echo cd \$HOME/PwTex/bin/\*  >>$HOME/bin/xelatex
echo export PATH=\$PWD:\$PATH  >>$HOME/bin/xelatex
echo cd \"\$PWDTEMP\"  >>$HOME/bin/xelatex
echo xelatex \$1 \$2 \$3 \$4 >>$HOME/bin/xelatex
chmod 775 $HOME/bin/xelatex
sudo mkdir -p /etc/mono/registry/LocalMachine/software/sil/PathwayXeLaTeX
sudo chmod 777 /etc/mono/registry/LocalMachine/software/sil/PathwayXeLaTeX
echo import os >value.py
echo print "'<values><value name=\"XeLaTexDir\" type=\"string\">' + os.environ[\"HOME\"] + '/PwTex/</value><value name=\"XeLaTexVer\" type=\"string\">1.9</value></values>'" >>value.py
sudo python value.py >/etc/mono/registry/LocalMachine/software/sil/PathwayXeLaTeX/values.xml
}
