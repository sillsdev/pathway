prefix=/usr
bindst=~/git/pathway/pathway/bin
binsrc=~/git/pathway/pathway
builddir=bin/x86/Release7BTE
BUILD_NUMBER=1.1.2.2205

build:
	xbuild /target:ReBuild /property:Configuration=Release7BTE\;Platform=x86\;BUILD_NUMBER=$(BUILD_NUMBER) PathwayLinux.sln
	rm -r -f $(bindst)
	mkdir $(bindst)
	mkdir $(bindst)/Copyrights
	mkdir $(bindst)/epub
	mkdir -p $(bindst)/epubcheck-1.2/lib
	mkdir $(bindst)/GoBible
	mkdir $(bindst)/Graphic
	mkdir $(bindst)/Help
	mkdir $(bindst)/Hypenation_Languages
	mkdir $(bindst)/Icons
	mkdir $(bindst)/InDesignFiles
	mkdir $(bindst)/LiftSupport
	mkdir $(bindst)/Loc
	mkdir $(bindst)/OfficeFiles
	mkdir $(bindst)/Samples
	mkdir $(bindst)/Styles
	cp -r $(binsrc)/ConfigurationTool/$(builddir)/* $(bindst)
	cp $(binsrc)/CssDialog/$(builddir)/* $(bindst)
	cp $(binsrc)/CssParser/$(builddir)/* $(bindst)
	cp $(binsrc)/epubConvert/$(builddir)/* $(bindst)
	cp $(binsrc)/epubValidator/$(builddir)/* $(bindst)
	cp -r $(binsrc)/ThirdParty/epubcheck-1.2/* $(bindst)
	cp $(binsrc)/GoBibleConvert/$(builddir)/* $(bindst)
	cp $(binsrc)/InDesignConvert/$(builddir)/* $(bindst)
	cp $(binsrc)/LiftPrepare/$(builddir)/* $(bindst)
	cp $(binsrc)/LogosConvert/$(builddir)/* $(bindst)
	cp $(binsrc)/OpenOfficeConvert/$(builddir)/* $(bindst)
	cp $(binsrc)/ParatextSupport/$(builddir)/* $(bindst)
	cp $(binsrc)/PathwayB/$(builddir)/* $(bindst)
	cp $(binsrc)/PsExport/$(builddir)/* $(bindst)
	cp -r $(binsrc)/PsSupport/* $(bindst)
	cp $(binsrc)/PsTool/$(builddir)/* $(bindst)
	cp $(binsrc)/WordPressConvert/$(builddir)/* $(bindst)
	cp $(binsrc)/XeLaTexConvert/$(builddir)/* $(bindst)
	#chmod 777 $(bindst)/*
	#chmod 777 $(bindst)/*StyleSettings.*
	#chmod 777 $(bindst)/Copyrights/*
	#chmod -R 777 $(bindst)/epub/*
	#chmod -R 777 $(bindst)/epubcheck-1.2/*
	#chmod 777 $(bindst)/GoBible/*
	#chmod 777 $(bindst)/GoBible/GoBibleCore/*
	#chmod 777 $(bindst)/Graphic/*
	#chmod 777 $(bindst)/Icons/*
	#chmod 777 $(bindst)/Samples/*
	#chmod 777 $(bindst)/Samples/Dictionary/*
	#chmod 777 $(bindst)/Samples/Scripture/*
	#chmod 777 $(bindst)/Styles/*
	#chmod 777 $(bindst)/Styles/Dictionary/*
	#chmod 777 $(bindst)/Styles/Scripture/*
	#chmod 777 $(bindst)/Template/*
	#chmod 777 $(bindst)/Template/Dictionary/*
	#chmod 777 $(bindst)/Template/Scripture/*


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
	nant clean
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
	sudo rm -rf debian/pathway
	rm -f debian/*.log *.log debian/*.debhelper debian/*.substvars
	rm -f ../*.dsc ../pathway_*.tar.gz ../pathway_*.build
	rm -f ../*.changes ../pathway*.deb


