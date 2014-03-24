ifndef prefix
prefix=/usr
endif
ifndef binsrc
binsrc=~/git/pathway/pathway/XeLaTexConvert
endif
ifndef bindst
bindst=$(binsrc)/bin
endif
ifndef bininst
bininst=/lib/pwtex
endif
ifndef target
target=XeLaTexBeforeBuild
endif
ifndef BUILD_NUMBER
BUILD_NUMBER=1.5.0.2445
endif

build:
	xbuild /target:XeLaTexBeforeBuild /property:BUILD_NUMBER=$(BUILD_NUMBER)\;Platform='$(Platform)'\;OS=Linux ../BuildPathway/BuildPathway.csproj
	rm -r -f $(bindst)
	mkdir $(bindst)
	cp -r $(binsrc)/../BuildPathway/Files/pwtex/* $(bindst)

install:
	sudo mkdir -p $(DESTDIR)$(prefix)$(bininst)
	sudo cp -r bin/. $(DESTDIR)$(prefix)$(bininst)
	#sudo mkdir -p $(DESTDIR)$(prefix)/bin
	#sudo cp PathwayB.sh $(DESTDIR)$(prefix)/bin/pathwayB
	#sudo cp ConfigurationTool.sh $(DESTDIR)$(prefix)/bin/ConfigurationTool
	#sudo mkdir -p $(DESTDIR)$(prefix)/share/python-support
	#sudo chmod 777 $(DESTDIR)$(prefix)/share/python-support
	sudo mkdir -p $(DESTDIR)$(prefix)/share/doc/pathway-xelatex
	sudo chmod 777 $(DESTDIR)$(prefix)/share/doc/pathway-xelatex
	sudo mkdir -p $(DESTDIR)$(prefix)/share/pwtex
	sudo chmod 777 $(DESTDIR)$(prefix)/share/pwtex
	sudo cp -r debian/pathwayXelatexRegistryKeys.sh $(DESTDIR)$(prefix)/share/pwtex/pathwayXelatexRegistryKeys.sh
	#sudo mkdir -p $(DESTDIR)$(prefix)/share/pwtex
	#sudo chmod 777 $(DESTDIR)$(prefix)/share/pwtex
	#sudo cp -r debian/WritePathwayKeys.cs $(DESTDIR)$(prefix)/share/pwtex/WritePathwayKeys.cs
	#sudo mkdir -p $(DESTDIR)$(prefix)/share/applications
	#sudo chmod 777 $(DESTDIR)$(prefix)/share/applications
	#sudo cp debian/*.desktop $(DESTDIR)$(prefix)/share/applications
	#sudo mkdir -p $(DESTDIR)$(prefix)/share/pixmaps
	#sudo chmod 777 $(DESTDIR)$(prefix)/share/pixmaps
	#sudo cp debian/*.png $(DESTDIR)$(prefix)/share/pixmaps
	#sudo cp debian/*.xpm $(DESTDIR)$(prefix)/share/pixmaps
	#sudo mkdir -p $(DESTDIR)$(prefix)/share/man/man7
	#sudo chmod 777 $(DESTDIR)$(prefix)/share/man/man7
	#sudo gzip -c debian/pathwayB.7.manpages >$(DESTDIR)$(prefix)/share/man/man7/pathwayB.7.gz
	sudo mkdir -p $(DESTDIR)/etc/mono/registry/LocalMachine/software/sil/PathwayXeLaTeX
	sudo chmod 777 $(DESTDIR)/etc/mono/registry/LocalMachine/software/sil/PathwayXeLaTeX
	sudo python -c 'print "<values><value name=\"XeLaTexDir\" type=\"string\">/usr$(bininst)/</value><value name=\"XeLaTexVer\" type=\"string\">$(BUILD_NUMBER)</value></values>"' >$(DESTDIR)$(prefix)/share/pwtex/pathwayXelatexValues.xml
	sudo cp $(DESTDIR)$(prefix)/share/pwtex/pathwayXelatexValues.xml $(DESTDIR)/etc/mono/registry/LocalMachine/software/sil/PathwayXeLaTeX/values.xml

binary:
	exit 0

tlsetup:
	xbuild /target:TlGetInstaller /property:OS=Linux ../BuildPathwaySprint.csproj
	xbuild /target:TlInstall /property:OS=Linux ../BuildPathwaySprint.csproj

clean:
	xbuild /target:Clean ../BuildPathwaySprint.csproj
	-sudo apt-get -y purge pathway-xelatex
	sudo rm -rf $(DESTDIR)$(prefix)$(bininst)
	sudo rm -rf $(DESTDIR)$(prefix)/share/doc/pathway-xelatex
	#sudo rm -rf $(DESTDIR)$(prefix)/share/man/man7/pathwayB.*
	#sudo rm -rf $(DESTDIR)$(prefix)/share/man/man7/ConfigurationTool.*
	#sudo rm -rf $(DESTDIR)$(prefix)/bin/pathwayB $(DESTDIR)$(prefix)/bin/ConfigurationTool
	#-xdg-desktop-menu uninstall /etc/pathway/sil-ConfigurationTool.desktop
	#sudo rm -rf $(DESTDIR)/etc/pathway
	-rm -rf ~/.mono/registry/CurrentUser/software/sil/PathwayXeLaTeX
	-rm -rf ~/.config/fieldworks/registry/LocalMachine/software/sil/PathwayXeLaTeX
	sudo rm -rf $(DESTDIR)/etc/mono/registry/LocalMachine/software/sil/PathwayXeLaTeX

clean-build:
	sudo rm -rf debian/pathway-xelatex bin
	rm -f debian/*.log *.log debian/*.debhelper debian/*.substvars debian/files
	rm -f ../*.dsc ../pathway-xelatex*.tar.gz ../pathway-xelatex*.build ../pathway-xelatex*.diff.gz
	rm -f ../*.changes ../pathway*.deb


