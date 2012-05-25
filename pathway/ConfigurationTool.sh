#!/bin/sh
mkdir -p ~/.mono/registry/CurrentUser/software/sil/pathway
cp /etc/mono/registry/LocalMachine/software/sil/pathway/* ~/.mono/registry/CurrentUser/software/sil/pathway
exec /usr/bin/mono /usr/lib/pathway/ConfigurationTool.exe "$@"

