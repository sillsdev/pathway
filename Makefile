ifndef prefix
prefix=/usr
endif
ifndef binsrc
binsrc=${PWD}
endif
ifndef bindst
bindst=$(binsrc)/output/Release
endif
ifndef BUILD_NUMBER
BUILD_NUMBER=1.13.4.4657
endif
ifndef Platform
Platform=Any CPU
endif
ifndef MONO_PREFIX
MONO_PREFIX=/opt/mono4-sil
endif
ifndef GDK_SHARP
GDK_SHARP=$(MONO_PREFIX)/lib/mono/gtk-sharp-3.0
endif
ifndef LD_LIBRARY_PATH
LD_LIBRARY_PATH=$(MONO_PREFIX)/lib
endif
ifndef PKG_CONFIG_PATH
PKG_CONFIG_PATH=$(MONO_PREFIX)/lib/pkgconfig
endif
ifndef MONO_GAC_PREFIX
MONO_GAC_PREFIX=$(MONO_PREFIX)
endif
ifndef MONO_MWF_SCALING
MONO_MWF_SCALING=disable
endif
PATH := $(MONO_PREFIX)/bin:$(PATH)

build:
	xbuild /t:ReBuild /p:BUILD_NUMBER=$(BUILD_NUMBER)\;Configuration=Release\;Platform='$(Platform)'\;OS=Linux\;SolutionDir=$(binsrc)/ Pathway.sln

debug:
	xbuild /t:ReBuild /p:BUILD_NUMBER=$(BUILD_NUMBER)\;Configuration=Debug\;Platform='$(Platform)'\;OS=Linux\;SolutionDir=$(binsrc)/ Pathway.sln

buildStep:
	xbuild /t:ReBuild /p:BUILD_NUMBER=$(BUILD_NUMBER)\;Configuration=Debug\;Platform='$(Platform)'\;OS=Linux\;SolutionDir=$(binsrc)/\;OutputPath=src/BuildStep/bin/Debug src/BuildStep/BuildStep.csproj

tests:
	nunit-console -exclude=SkipOnTeamCity\;LongTest -labels -nodots output/Debug/Test.dll

install:
	mkdir -p $(DESTDIR)$(prefix)/lib/pathway
	cp -r $(bindst)/. $(DESTDIR)$(prefix)/lib/pathway
	mkdir -p $(DESTDIR)$(prefix)/bin
	cp src/PathwayB.sh $(DESTDIR)$(prefix)/bin/pathwayB
	cp src/ConfigurationTool.sh $(DESTDIR)$(prefix)/bin/ConfigurationTool
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


