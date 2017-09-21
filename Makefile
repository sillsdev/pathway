ifndef prefix
prefix=/usr
endif
ifndef binsrc
binsrc=${PWD}
endif
ifndef bindst
bindst=$(binsrc)/output/Release
endif

build:
	make -f Makefile46 buildStep
	make -f Makefile46 build
	make -f Makefile40 build

debug:
	make -f Makefile46 debug
	make -f Makefile40 debug

buildStep:
	make -f Makefile46 buildStep
	make -f Makefile40 buildStep

tests:
	make -f Makefile46 tests
	make -f Makefile40 tests

install:
	mkdir -p $(DESTDIR)$(prefix)/lib/pathway
	cp -r $(bindst)/. $(DESTDIR)$(prefix)/lib/pathway
	mkdir -p $(DESTDIR)$(prefix)/bin
	cp src/PathwayB.sh $(DESTDIR)$(prefix)/bin/pathwayB
	cp src/ConfigurationTool.sh $(DESTDIR)$(prefix)/bin/ConfigurationTool
	cp src/CssSimpler.sh $(DESTDIR)$(prefix)/bin/CssSimpler
	cp src/PathwayExport.sh $(DESTDIR)$(prefix)/bin/PathwayExport
	cp src/PdfLicense.sh $(DESTDIR)$(prefix)/bin/PdfLicense
	cp src/ApplyPDFLicenseInfo.sh $(DESTDIR)$(prefix)/bin/ApplyPDFLicenseInfo
	mkdir -p $(DESTDIR)$(prefix)/share/python-support
	chmod 777 $(DESTDIR)$(prefix)/share/python-support
	mkdir -p $(DESTDIR)$(prefix)/share/doc/pathway
	chmod 777 $(DESTDIR)$(prefix)/share/doc/pathway
	mkdir -p $(DESTDIR)$(prefix)/share/pathway
	chmod 777 $(DESTDIR)$(prefix)/share/pathway
	cp -r debian/pathwayValues.xml $(DESTDIR)$(prefix)/share/pathway/pathwayValues.xml
	cp -r debian/pathwayWsValues.xml $(DESTDIR)$(prefix)/share/pathway/pathwayWsValues.xml
	cp -r debian/pathwayRegistryKeys.sh $(DESTDIR)$(prefix)/share/pathway/pathwayRegistryKeys.sh
	cp -r src/XeLaTexConvert/InstallPathwayXeLaTeX.sh $(DESTDIR)$(prefix)/share/pathway/InstallPathwayXeLaTeX.sh
	mkdir -p $(DESTDIR)$(prefix)/share/applications
	chmod 777 $(DESTDIR)$(prefix)/share/applications
	cp debian/*.desktop $(DESTDIR)$(prefix)/share/applications
	mkdir -p $(DESTDIR)$(prefix)/share/pixmaps
	chmod 777 $(DESTDIR)$(prefix)/share/pixmaps
	cp debian/*.png $(DESTDIR)$(prefix)/share/pixmaps
	cp debian/*.xpm $(DESTDIR)$(prefix)/share/pixmaps
	mkdir -p $(DESTDIR)$(prefix)/share/man
	chmod 777 $(DESTDIR)$(prefix)/share/man

binary:
	exit 0

clean:
	rm -rf output/*

uninstall:
	-sudo apt-get -y remove pathway
	sudo rm -rf $(DESTDIR)$(prefix)/lib/pathway
	sudo rm $(DESTDIR)$(prefix)/bin/pathwayB
	sudo rm $(DESTDIR)$(prefix)/bin/ConfigurationTool
	sudo rm $(DESTDIR)$(prefix)/bin/CssSimpler
	sudo rm $(DESTDIR)$(prefix)/bin/PathwayExport
	sudo rm -rf $(DESTDIR)$(prefix)/share/doc/pathway
	sudo rm -rf $(DESTDIR)$(prefix)/share/man/man7/pathway*
	sudo rm -rf $(DESTDIR)$(prefix)/share/man/man7/ConfigurationTool*
	sudo rm -rf $(DESTDIR)$(prefix)/bin/pathwayB $(DESTDIR)$(prefix)/bin/ConfigurationTool
	sudo rm -rf $(DESTDIR)$(prefix)/share/pathway
	sudo rm -rf $(DESTDIR)/etc/profile.d/pathway*
	-xdg-desktop-menu uninstall /etc/pathway/sil-ConfigurationTool.desktop
	sudo rm -rf $(DESTDIR)/etc/pathway
	-rm -rf ~/.mono/registry/CurrentUser/software/sil/pathway
	-rm -rf ~/.local/share/SIL/Pathway

clean-build:
	rm -rf debian/pathway bin
	rm -f debian/*.log *.log debian/*.debhelper debian/*.substvars debian/files
	rm -f *.dsc pathway_*.tar.gz pathway_*.build pathway_*.diff.gz
	rm -f *.changes pathway*.deb


