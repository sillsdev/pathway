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
cd ../pathway-${RELEASE}
cp -r ../pathway/. .
rm -rf .git
cp ../pathway/lib/L10NSharp.* lib/.
cp ../pathway/debian/changelog debian/.
cp ../pathway/debian/control debian/control
cp ../pathway/debian/rules debian/rules
#cp -rf ../pathway/debian/source debian/.
#cp ../pathway/DistFiles/*.csproj DistFiles/.
cd ..

# Delete unwanted non-source files here using find
find pathway-${RELEASE} -type f -iname "*.hhc" -delete
#find pathway-${RELEASE} -type f -iname "*.dll" -delete
find pathway-${RELEASE} -type f -iname "*.exe" -delete

# Tar it up and create symlink for .orig.bz2
tar jcf pathway-${RELEASE}.tar.bz2 pathway-${RELEASE} || exit 3
ln -fs pathway-${RELEASE}.tar.bz2 pathway_${RELEASE}.orig.tar.bz2

# Do an initial unsigned source build in host OS environment
cd pathway-${RELEASE}

if [ "$(dpkg --print-architecture)" == "amd64" ]; then
   debuild -eBUILD_NUMBER=${RELEASE} -ebinsrc=${PWD} -us -uc || exit 4
else
   debuild -eBUILD_NUMBER=${RELEASE} -ePlatform=x86 -ebinsrc=${PWD} -us -uc || exit 4
fi
cd ..

