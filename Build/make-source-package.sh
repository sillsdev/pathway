#!/bin/bash

# make-source-package.sh
#
# Download and create source packages of Pathway
#
# Usage: make-source-package [BRANCH]
#
#        BRANCH    the branch or commit to package (default: "master")
#
# Authored:  2012-10-03 by Jonathan Marsden <jmarsden@fastmail.fm>
# Revised    2015-01-21 by Bill Martin to include "utopic"
# Revised    2015-12-18 by Greg Trihus to apply to Pathway (add branch)
# Refactored 2016-01-20 by Neil Mayhew (originally release.sh)

set -e # Exit on error

BRANCH=${1:-master}

DEVTOOLS="git wget ca-certificates devscripts fakeroot"

# Install development tools as required
if ! dpkg-query -W $DEVTOOLS >/dev/null
then
    echo "$0: Installing $DEVTOOLS"
    sudo apt-get update
    sudo apt-get install -y --no-install-recommends $DEVTOOLS
fi

# Set up a temporary directory
TMPDIR=$(mktemp -d)
trap "rm -rf $TMPDIR" 0
PKGDIR=$TMPDIR/package
mkdir -p $PKGDIR

# Bring git up to date
git remote update -p

# Export the source from git
(cd "./$(git rev-parse --show-cdup)" && git archive ${BRANCH}) | tar -x -C $PKGDIR

cd $PKGDIR

# Download dependencies
Build/getDependencies.sh

# Delete unwanted non-source files
find . -type f -iname "*.exe" -delete

# Make an unsigned source package without cleaning
debuild -S -nc -us -uc

PACKAGE=$(dpkg-parsechangelog | sed -n '/^Source: /s///p')
VERSION=$(dpkg-parsechangelog | sed -n '/^Version: /s///p')

cd $OLDPWD

dcmd mv $TMPDIR/${PACKAGE}_${VERSION}_source.{changes,build} .

dcmd ls -l ${PACKAGE}_${VERSION}_source.{changes,build}

ln -sf ${PACKAGE}_${VERSION}_source.changes latest.changes

echo "$0: Completed"
