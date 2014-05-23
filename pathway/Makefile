ifndef prefix
prefix=/usr
endif
ifndef binsrc
binsrc=~/git/pathway/pathway
endif
ifndef bindst
bindst=$(binsrc)/bin
endif
ifndef target
target=Daily
endif
ifndef BUILD_NUMBER
BUILD_NUMBER=1.1.2.2205
endif
ifndef Platform
Platform=x86
endif

build:
	#xbuild /target:ReBuild /property:Configuration=$(target)\;Platform=x86\;BUILD_NUMBER=$(BUILD_NUMBER)\;OS=Linux PublishingSolution.sln
	xbuild /target:$(target) /property:BUILD_NUMBER=$(BUILD_NUMBER)\;Platform='$(Platform)'\;OS=Linux BuildPathwaySprint.csproj
	rm -r -f $(bindst)
	mkdir $(bindst)
	cp -r $(binsrc)/BuildPathway/Files/ConfigurationTool/* $(bindst)

install:
	sudo mkdir -p $(DESTDIR)$(prefix)/lib/pathway
	sudo cp -r bin/. $(DESTDIR)$(prefix)/lib/pathway
	sudo mkdir -p $(DESTDIR)$(prefix)/bin
	sudo cp PathwayB.sh $(DESTDIR)$(prefix)/bin/pathwayB
	sudo cp ConfigurationTool.sh $(DESTDIR)$(prefix)/bin/ConfigurationTool
	sudo mkdir -p $(DESTDIR)$(prefix)/share/python-support
	sudo chmod 777 $(DESTDIR)$(prefix)/share/python-support
	sudo mkdir -p $(DESTDIR)$(prefix)/share/doc/pathway
	sudo chmod 777 $(DESTDIR)$(prefix)/share/doc/pathway
	sudo mkdir -p $(DESTDIR)$(prefix)/share/pathway
	sudo chmod 777 $(DESTDIR)$(prefix)/share/pathway
	sudo cp -r debian/pathwayValues.xml $(DESTDIR)$(prefix)/share/pathway/pathwayValues.xml
	sudo cp -r debian/pathwayWsValues.xml $(DESTDIR)$(prefix)/share/pathway/pathwayWsValues.xml
	sudo cp -r debian/pathwayRegistryKeys.sh $(DESTDIR)$(prefix)/share/pathway/pathwayRegistryKeys.sh
	sudo cp -r XeLaTexConvert/InstallPathwayXeLaTeX.sh $(DESTDIR)$(prefix)/share/pathway/InstallPathwayXeLaTeX.sh
	sudo mkdir -p $(DESTDIR)$(prefix)/share/applications
	sudo chmod 777 $(DESTDIR)$(prefix)/share/applications
	sudo cp debian/*.desktop $(DESTDIR)$(prefix)/share/applications
	sudo mkdir -p $(DESTDIR)$(prefix)/share/pixmaps
	sudo chmod 777 $(DESTDIR)$(prefix)/share/pixmaps
	sudo cp debian/*.png $(DESTDIR)$(prefix)/share/pixmaps
	sudo cp debian/*.xpm $(DESTDIR)$(prefix)/share/pixmaps
	sudo mkdir -p $(DESTDIR)$(prefix)/share/man
	sudo chmod 777 $(DESTDIR)$(prefix)/share/man

binary:
	exit 0

clean:
	xbuild /target:Clean BuildPathwaySprint.csproj

uninstall:
	-sudo apt-get -y remove pathway
	sudo rm -rf $(DESTDIR)$(prefix)/lib/pathway
	sudo rm -rf $(DESTDIR)$(prefix)/share/doc/pathway
	sudo rm -rf $(DESTDIR)$(prefix)/share/man/man7/pathway*
	sudo rm -rf $(DESTDIR)$(prefix)/share/man/man7/ConfigurationTool*
	sudo rm -rf $(DESTDIR)$(prefix)/bin/pathwayB $(DESTDIR)$(prefix)/bin/ConfigurationTool
	sudo rm -rf $(DESTDIR)$(prefix)/share/pathway
	sudo rm -rf $(DESTDIR)/etc/profile.d/pathway*
	-xdg-desktop-menu uninstall /etc/pathway/sil-ConfigurationTool.desktop
	sudo rm -rf $(DESTDIR)/etc/pathway
	-rm -rf ~/.mono/registry/CurrentUser/software/sil/pathway
	-rm -rf .local/share/SIL/Pathway

clean-build:
	sudo rm -rf debian/pathway bin
	rm -f debian/*.log *.log debian/*.debhelper debian/*.substvars debian/files
	rm -f ../*.dsc ../pathway_*.tar.gz ../pathway_*.build ../pathway_*.diff.gz
	rm -f ../*.changes ../pathway*.deb


