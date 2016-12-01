ifndef prefix
prefix=/usr
endif
ifndef bininst
bininst=/lib/pwtex
endif
ifndef binsrc
binsrc=$(PWD)
endif
ifndef bindst
bindst=$(binsrc)/output
endif
ifndef BUILD_NUMBER
BUILD_NUMBER=1.15.4.5366
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
	cd ../../DistFiles;xbuild /verbosity:detailed /target:XeLaTexBeforeBuild /property:Configuration=PathwayXeLaTeX\;BUILD_NUMBER=$(BUILD_NUMBER)\;Platform='$(Platform)'\;OS=Linux\;OutputPath=. /flp:LogFile=log.txt
	rm -r -f $(bindst)
	mkdir -p $(bindst)
	cp -r $(binsrc)/../../output/pwtex/* $(bindst)

install:
	mkdir -p $(DESTDIR)$(prefix)$(bininst)
	cp -r $(bindst)/* $(DESTDIR)$(prefix)$(bininst)
	mkdir -p $(DESTDIR)$(prefix)/share/doc/pathway-xelatex
	chmod 777 $(DESTDIR)$(prefix)/share/doc/pathway-xelatex
	mkdir -p $(DESTDIR)$(prefix)/share/pwtex
	chmod 777 $(DESTDIR)$(prefix)/share/pwtex
	cp -r debian/pathwayXelatexRegistryKeys.sh $(DESTDIR)$(prefix)/share/pwtex/pathwayXelatexRegistryKeys.sh
	python -c 'print "<values><value name=\"XeLaTexDir\" type=\"string\">/usr$(bininst)/</value><value name=\"XeLaTexVer\" type=\"string\">$(BUILD_NUMBER)</value></values>"' >$(DESTDIR)$(prefix)/share/pwtex/pathwayXelatexValues.xml
binary:
	exit 0

tlsetup:
	cd ..;xbuild /t:TlGetInstaller\;TlInstall /property:OS=Linux\;OutputPath=. BuildPathwaySprint.csproj

clean:
	rm -rf bin ../pathway-xelatex* ../../output/pwtex ../*/bin ../*/obj debian/pathway-xelatex

uninstall:
	-sudo apt-get -y purge pathway-xelatex
	sudo rm -rf $(DESTDIR)$(prefix)$(bininst)
	sudo rm -rf $(DESTDIR)$(prefix)/share/doc/pathway-xelatex
	-rm -rf ~/.mono/registry/CurrentUser/software/sil/PathwayXeLaTeX
	-rm -rf ~/.config/fieldworks/registry/LocalMachine/software/sil/PathwayXeLaTeX
	sudo rm -rf $(DESTDIR)/etc/mono/registry/LocalMachine/software/sil/PathwayXeLaTeX

clean-build:
	sudo rm -rf debian/pathway-xelatex bin
	rm -f debian/*.log *.log debian/*.debhelper debian/*.substvars debian/files
	rm -f ../*.dsc ../pathway-xelatex*.tar.gz ../pathway-xelatex*.build ../pathway-xelatex*.diff.gz
	rm -f ../*.changes ../pathway*.deb


