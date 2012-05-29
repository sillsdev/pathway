#!/bin/sh
mkdir -p ~/.mono/registry/CurrentUser/software/sil/pathway
python -c 'print "<values>\n<value name=\"PathwayDir\"\ntype=\"string\">/usr/lib/pathway/</value>\n<value name=\"WritingSystemStore\"\ntype=\"string\">~/.local/share/SIL/WritingSystemStore/</value>\n</values>"' >~/.mono/registry/CurrentUser/software/sil/pathway/values.xml
#cp /etc/mono/registry/LocalMachine/software/sil/pathway/* ~/.mono/registry/CurrentUser/software/sil/pathway
exec /usr/bin/mono /usr/lib/pathway/ConfigurationTool.exe "$@"

