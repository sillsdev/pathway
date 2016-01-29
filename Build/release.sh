#!/bin/bash

# release-sh
#
# Create release packages of Pathway
#
# Usage: release [BRANCH [DISTRO ...]]
#
#        BRANCH    the branch or commit in the repo to build (eg "master")
#        DISTRO    a distro name to build for (eg "trusty")
#
# Default values for BRANCH and DISTROs are determined by the lower-level
# scripts that do the work

set -e # Exit on error

BRANCH=$1
DISTROS=${@:2}

DIR=$(dirname "$0")

"$DIR"/make-source-package.sh $BRANCH
"$DIR"/build-binary-packages.sh $(dcmd --dsc latest.changes) $DISTROS
