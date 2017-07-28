#!/bin/bash
# makeDeb.sh -- Creates release pacakges of Pathway
#               First optional parameter is a version number e.g. 6.2.2.20
#
# Author: Greg Trihus <greg_trihus@sil.org>
# Date: 2016-01-11

RELEASE=${1:-"1.13.4.4756"}
rm -rf ../pathway-*
rm -rf ../pathway_*
mkdir ../pathway-${RELEASE}
#git archive HEAD | tar -x -C ../pathway-${RELEASE} || exit 2
#cd ../pathway-${RELEASE}
cp -r . ../pathway-${RELEASE}
rm -rf ../pathway-${RELEASE}/.git
rm -rf ../pathway-${RELEASE}/src/XeLaTexConvert/TexLive
rm -rf ../pathway-${RELEASE}/src/XeLaTexConvert/XeLaTexExe
rm -rf ../pathway-${RELEASE}/src/XeLaTexConvert/debian/pathway-xelatex
cp lib/L10NSharp.* ../pathway-${RELEASE}/lib/.
cp debian/changelog ../pathway-${RELEASE}/debian/.
cp debian/control ../pathway-${RELEASE}/debian/control
cp debian/rules ../pathway-${RELEASE}/debian/rules
#cp -rf ../pathway/debian/source debian/.
#cp ../pathway/DistFiles/*.csproj DistFiles/.
cd ..

# Delete unwanted non-source files here using find
find pathway-${RELEASE} -type f -iname "*.hhc" -delete
#find pathway-${RELEASE} -type f -iname "*.dll" -delete
find pathway-${RELEASE} -type f -iname "*.exe" -delete
find pathway-${RELEASE} -type f -iname "pathway-xelatex*" -delete
find pathway-${RELEASE} -type d -iname bin -exec rm -rf {} \;
find pathway-${RELEASE} -type d -iname obj -exec rm -rf {} \;

# Tar it up and create symlink for .orig.bz2
tar jcf pathway-${RELEASE}.tar.bz2 pathway-${RELEASE} || exit 3
ln -fs pathway-${RELEASE}.tar.bz2 pathway_${RELEASE}.orig.tar.bz2

# Do an initial unsigned source build in host OS environment
cd pathway-${RELEASE}

if [ "$(dpkg --print-architecture)" == "amd64" ]; then
   debuild -eBUILD_NUMBER=${RELEASE} -ebinsrc=${PWD} -us -uc || exit 4
else
   dpkg-buildpackage -eBUILD_NUMBER=${RELEASE} -ePlatform=x86 -ebinsrc=${PWD} -us -uc || exit 4
fi
cd ..

