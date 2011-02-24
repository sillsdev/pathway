#!/bin/sh

pw_base=$(dirname "$0")/../..
pw_inst=/usr/lib/pathway
pw_cfg=/bin/x86/Debug

chmod 777 ${pw_inst}
rm -f -r ${pw_inst}
mkdir -p ${pw_inst}
chmod 777 ${pw_inst}

cp ${pw_base}/ThirdParty/gsdll32.dll ${pw_inst}
cp -r ${pw_base}/ThirdParty/epubcheck-1.1 ${pw_inst}/.
cp ${pw_base}/LiftPrepare/lib/PalasoLib/*.dll ${pw_inst}
cp ${pw_base}/PathwayB${pw_cfg}/PathwayB.* ${pw_inst}
cp ${pw_base}/OpenOfficeConvert${pw_cfg}/OpenOfficeConvert.* ${pw_inst}
cp ${pw_base}/LiftPrepare${pw_cfg}/LiftPrepare.* ${pw_inst}
cp ${pw_base}/epubConvert${pw_cfg}/epubConvert.* ${pw_inst}
cp ${pw_base}/epubValidator${pw_cfg}/epubValidator.* ${pw_inst}

if [ "$1" != "CorporateBTE" -a "$1" != "Corporate7BTE" -a "$1" != "CorporateSE" -a "$1" != "Corporate7SE" ]
then
cp ${pw_base}/PdfConvert${pw_cfg}/PdfConvert.* ${pw_inst}
cp ${pw_base}/WordPressConvert${pw_cfg}/WordPressConvert.* ${pw_inst}
cp ${pw_base}/XeTeXConvert${pw_cfg}/XeTeXConvert.* ${pw_inst}
fi

if [ "$1" != "CorporateSE" -a "$1" != "Corporate7SE" -a "$1" != "ReleaseSE" -a "$1" != "Release7SE" ]
then
cp ${pw_base}/GoBibleConvert${pw_cfg}/GoBibleConvert.* ${pw_inst}
cp ${pw_base}/YouVersionConvert${pw_cfg}/YouVersionConvert.* ${pw_inst}
cp ${pw_base}/LogosConvert${pw_cfg}/LogosConvert.* ${pw_inst}
cp ${pw_base}/ParatextSupport${pw_cfg}/ParatextSupport.* ${pw_inst}
cp ${pw_base}/PsSupport/ScriptureStyleSettings.xml ${pw_inst}
fi

cp -r ${pw_base}/PsSupport/OfficeFiles ${pw_inst}/.

cp -r ${pw_base}/PsSupport/Styles ${pw_inst}/.
cp -r ${pw_base}/PsSupport/Icons ${pw_inst}/.
cp -r ${pw_base}/PsSupport/Graphic ${pw_inst}/.
cp -r ${pw_base}/PsSupport/Loc ${pw_inst}/.
cp -r ${pw_base}/PsSupport/Samples ${pw_inst}/.

mkdir -p ${pw_inst}/Help
if [ "$1" = "CorporateSE" -o "$1" = "Corporate7SE" -o "$1" = "ReleaseSE" -o "$1" = "Release7SE" ]
then
cp ${pw_base}/Build/Installer/Pathway*_SE.chm ${pw_inst}/Help
else
cp ${pw_base}/Build/Installer/Pathway*_BTE.chm ${pw_inst}/Help
fi

cp ${pw_base}/PsSupport/DictionaryStyleSettings.xml ${pw_inst}
cp ${pw_base}/PsSupport/StyleSettings.xml ${pw_inst}
cp ${pw_base}/PsSupport/StyleSettings.xsd ${pw_inst}
#cp ${pw_base}/PsSupport/previewdll/* ${pw_inst} 
cp ${pw_base}/PsSupport/*.xhtml ${pw_inst}

cp ${pw_base}/ConfigurationTool${pw_cfg}/* ${pw_inst}
cp ${pw_base}/PsExport${pw_cfg}/* ${pw_inst}

cp ${pw_base}/PsSupport/* ${pw_inst}

if [ "$1" = "CorporateSE" -o "$1" = "Corporate7SE" -o "$1" = "ReleaseSE" -o "$1" = "Release7SE" ]
then
rm TE_*.xslt
rm *-scr.xsl
rm scripture*.tpl
rm Scripture*.xml
fi

pw_key=~/.mono/registry/CurrentUser/software/sil/pathway
mkdir -p ${pw_key}
echo \<values\>\<value name=\"PathwayDir\" type=\"string\"\>${pw_inst}\<\/value\>\<\/values\> >${pw_key}/values.xml

echo mono ${pw_inst}/PathwayB.exe >/usr/bin/PathwayB
chmod 755 /usr/bin/PathwayB

echo mono ${pw_inst}/ConfigurationTool.exe >/usr/bin/ConfigurationTool
chmod 755 /usr/bin/ConfigurationTool

exit 0
