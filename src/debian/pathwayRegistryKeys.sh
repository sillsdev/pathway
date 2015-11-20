#!/bin/sh
mkdir -p $HOME/.mono/registry/CurrentUser/software/sil/pathway
cp /usr/share/pathway/pathwayValues.xml $HOME/.mono/registry/CurrentUser/software/sil/pathway/values.xml
	if [ -d "/var/lib/fieldworks/SIL/WritingSystemStore/" ]; then
		cp /usr/share/pathway/pathwayWsValues.xml $HOME/.mono/registry/CurrentUser/software/sil/pathway/values.xml
	fi

