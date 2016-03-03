ifndef prefix
prefix=/usr
endif
ifndef binsrc
binsrc=~/git/pathway/src/XeLaTexConvert
endif
ifndef bindst
bindst=$(binsrc)/bin
endif
ifndef bininst
bininst=/lib/pwtex
endif
ifndef target
target=ReBuild
endif
ifndef BUILD_NUMBER
BUILD_NUMBER=1.5.0.2445
endif

build:
	cd ../../DistFiles;xbuild /verbosity:detailed /target:XeLaTexBeforeBuild /property:Configuration=PathwayXeLaTeX\;BUILD_NUMBER=$(BUILD_NUMBER)\;Platform='$(Platform)'\;OS=Linux\;OutputPath=. /flp:LogFile=log.txt
	rm -r -f $(bindst)
	mkdir $(bindst)
	cp -r $(binsrc)/../../output/pwtex/* $(bindst)

install:
	mkdir -p $(DESTDIR)$(prefix)$(bininst)
	cp -r bin/. $(DESTDIR)$(prefix)$(bininst)
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


