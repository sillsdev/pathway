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
target=Release7BTE
endif
ifndef BUILD_NUMBER
BUILD_NUMBER=1.1.2.2205
endif

build:
	#xbuild /target:ReBuild /property:Configuration=$(target)\;Platform=x86\;BUILD_NUMBER=$(BUILD_NUMBER)\;OS=Linux PublishingSolution.sln
	xbuild /target:$(target) /property:BUILD_NUMBER=$(BUILD_NUMBER)\;OS=Linux BuildPathwaySprint.csproj
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
	gawk 'BEGIN {print "<values><value name=\"PathwayDir\" type=\"string\">$(prefix)/lib/pathway/</value><value name=\"WritingSystemStore\" type=\"string\">~/.local/share/SIL/WritingSystemStore/</value></values>"}' >debian/pathway-etc/values.xml
	sudo mkdir -p $(DESTDIR)/etc/pathway
	sudo cp -r debian/pathway-etc/. $(DESTDIR)/etc/pathway

binary:
	exit 0

clean:
	xbuild /target:Clean BuildPathwaySprint.csproj
	-sudo apt-get -y remove pathway
	sudo rm -rf $(DESTDIR)$(prefix)/lib/pathway
	sudo rm -rf $(DESTDIR)$(prefix)/share/doc/pathway
	sudo rm -rf $(DESTDIR)$(prefix)/share/man/man7/pathwayB.*
	sudo rm -rf $(DESTDIR)$(prefix)/share/man/man7/ConfigurationTool.*
	sudo rm -rf $(DESTDIR)$(prefix)/bin/pathwayB $(DESTDIR)$(prefix)/bin/ConfigurationTool
	xdg-desktop-menu uninstall /etc/pathway/sil-ConfigurationTool.desktop
	sudo rm -rf $(DESTDIR)/etc/pathway
	rm -rf ~/.mono/registry/CurrentUser/software/sil/pathway
	rm -rf debian/pathway-etc/values.xml

clean-build:
	sudo rm -rf debian/pathway bin
	rm -f debian/*.log *.log debian/*.debhelper debian/*.substvars debian/files
	rm -f ../*.dsc ../pathway_*.tar.gz ../pathway_*.build ../pathway_*.diff.gz
	rm -f ../*.changes ../pathway*.deb


