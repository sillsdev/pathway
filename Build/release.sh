#!/bin/bash
# release-sh -- Downloads and creates release pacakges of Pathway
#               First optional parameter is a version number e.g. 6.2.2
#               Second optional parameter is a space-delimited set
#                 of distro names to build for, e.g. "lucid maverick"
#               Third option parameter is the branch in the repo to build
#
# Author: Jonathan Marsden <jmarsden@fastmail.fm>
# Date: 2012-10-03
# Revised 2015-01-21 by Bill Martin to include "utopic"
# Revised 2015-12-18 by Greg Trihus to apply to Pathway (add branch)

set -e # Exit on error

PBUILDFOLDER=${PBUILDFOLDER:-~/pbuilder}
OSRELEASES=${2:-"lucid maverick natty oneiric precise quantal raring saucy trusty utopic sid"}
BRANCH=${3:-"master"}
DEVTOOLS="git wget ca-certificates devscripts fakeroot"

# Install development tools as required
sudo apt-get update
sudo apt-get dist-upgrade -y
sudo apt-get install -y --no-install-recommends $DEVTOOLS

# Install ~/.pbuilderrc unless there already is one
[ -f ~/.pbuilderrc ] || cat >~/.pbuilderrc <<"EOF"
# Codenames for Debian suites according to their alias.
UNSTABLE_CODENAME="sid"
TESTING_CODENAME="wheezy"
STABLE_CODENAME="squeeze"

# List of Debian suites.
DEBIAN_SUITES=($UNSTABLE_CODENAME $TESTING_CODENAME $STABLE_CODENAME
    "experimental" "unstable" "testing" "stable")

# List of Ubuntu suites. Update these when needed.
UBUNTU_SUITES=("utopic" "trusty" "saucy" "raring" "quantal" "precise" "oneiric" "natty" "maverick" "lucid"
     "karmic" "jaunty" "intrepid" "hardy" "gutsy")

# Mirrors to use. Update these to your preferred mirror.
DEBIAN_MIRROR="ftp.us.debian.org"
UBUNTU_MIRROR="mirrors.kernel.org"

# Use the changelog of a package to determine the suite to use if none set.
if [ -z "${DIST}" ] && [ -r "debian/changelog" ]; then
    DIST=$(dpkg-parsechangelog | awk '/^Distribution: / {print $2}')
    # Use the unstable suite for Debian experimental packages.
    if [ "${DIST}" == "experimental" ]; then
        DIST="$UNSTABLE_CODENAME"
    fi
fi

# Optionally set a default distribution if none is used. Note that you can set
# your own default (i.e. ${DIST:="unstable"}).
: ${DIST:="$(lsb_release --short --codename)"}

# Optionally change Debian codenames in $DIST to their aliases.
case "$DIST" in
    $UNSTABLE_CODENAME)
        DIST="sid"
        ;;
    $TESTING_CODENAME)
        DIST="wheezy"
        ;;
    $STABLE_CODENAME)
        DIST="squeeze"
        ;;
esac

# Optionally set the architecture to the host architecture if none set. Note
# that you can set your own default (i.e. ${ARCH:="i386"}).
: ${ARCH:="$(dpkg --print-architecture)"}

NAME="$DIST"
if [ -n "${ARCH}" ]; then
    NAME="$NAME-$ARCH"
    DEBOOTSTRAPOPTS=("--arch" "$ARCH" "${DEBOOTSTRAPOPTS[@]}")
fi
BASETGZ="/var/cache/pbuilder/$NAME-base.tgz"
DISTRIBUTION="$DIST"
BUILDRESULT="/var/cache/pbuilder/$NAME/result/"
APTCACHE="/var/cache/pbuilder/$NAME/aptcache/"
BUILDPLACE="/var/cache/pbuilder/build/"

if $(echo ${DEBIAN_SUITES[@]} | grep -q $DIST); then
    # Debian configuration
    MIRRORSITE="http://$DEBIAN_MIRROR/debian/"
    COMPONENTS="main contrib non-free"
elif $(echo ${UBUNTU_SUITES[@]} | grep -q $DIST); then
    # Ubuntu configuration
    MIRRORSITE="http://$UBUNTU_MIRROR/ubuntu/"
    COMPONENTS="main restricted universe multiverse"
else
    echo "Unknown distribution: $DIST"
    exit 1
fi

#############################
# Local mods below here. JM #
#############################

# Work under ~/pbuilder/
BASETGZ=~/pbuilder/"$NAME-base.tgz"
BUILDRESULT=~/pbuilder/"${NAME}_result/"
BUILDPLACE=~/pbuilder/build/
APTCACHE=~/pbuilder/"${NAME}_aptcache/"
APTCACHEHARDLINK=no

## Use local packages from ~/pbuilder/deps
#DEPDIR=~/pbuilder/deps
#OTHERMIRROR="deb file://"$DEPDIR" ./"
#BINDMOUNTS=$DEPDIR
HOOKDIR=~/pbuilder/hooks
#EXTRAPACKAGES="apt-utils"

if [ x"$DIST" = x"lucid" ];then
    # Running a Ubuntu Lucid chroot.  Add lucid-updates mirror site too.
    OTHERMIRROR="deb http://us.archive.ubuntu.com/ubuntu/ ${DIST}-updates main restricted multiverse universe"
fi
EOF

# Install pbuilder hook to add a distro suffix to package versions
mkdir -p ${PBUILDFOLDER}/hooks
if [ ! -f ${PBUILDFOLDER}/hooks/A05suffix ]; then
  cat >${PBUILDFOLDER}/hooks/A05suffix <<"EOF"
#!/bin/bash
# pbuilder hook for adding distro name to package version
#
# Neil Mayhew - 2010-12-08
# Jonathan Marsden - 2012-09-23 Added sed and changed build location

TYPE=$(lsb_release -si)
DIST=$(lsb_release -sc)
USER=$(id -un)
HOST=$(uname -n)

export DEBFULLNAME="pbuilder"
export DEBEMAIL="$USER@$HOST"

if [ "$TYPE" = Ubuntu ]
then
    cd ~/*/
    debchange --local=+$DIST "Build for $DIST"
    # Removed unwanted trailing 1 from distribution suffix
    sed -i -e "1s/+${DIST}1/+${DIST}/" debian/changelog
fi
EOF
  chmod 0755 ${PBUILDFOLDER}/hooks/A05suffix
fi

# Figure out which release to build -- default is latest changelog entry
CHANGELOG=$(readlink -m "$(dirname "$0")/../debian/changelog")
if [ -r "$CHANGELOG" ]; then
  VERSION=$(dpkg-parsechangelog -l"$CHANGELOG" | sed -n '/^Version: /s///p')
  UPSTREAM=${VERSION%-*}
fi
RELEASE=${1:-$UPSTREAM}
if [ -z "$RELEASE" ]; then
  echo "I need a version number to use for the release" >&2
  echo "I didn't find a debian/changelog, and an explicit version wasn't given" >&2
  echo "I expected to find the changelog here:" >&2
  echo "  $CHANGELOG" >&2
  exit 1
fi

# Export the release, ready for creating a source tarball
rm -rf pathway-${RELEASE}
mkdir pathway-${RELEASE}
cd pathway-${RELEASE}

git archive --remote=git://github.com/sillsdev/pathway.git ${BRANCH} | tar -x
Build/getDependencies.sh

# Delete unwanted non-source files here using find
find . -type f -iname "*.exe" -delete

# Create orig tarball
TARBALL=../pathway_${RELEASE}.orig.tar.bz2
if [ ! -e $TARBALL ]; then
  tar -cjf $TARBALL -C .. ${PWD##*/}
fi

# Do an initial unsigned source build in host OS environment
debuild -S -nc -sa -us -uc

cd ..

for i in $OSRELEASES; do
  export DIST=$i	# $DIST is used in .pbuilderrc
  # Ensure pbuilder-dist symlinks for relevant releases exist
  [ -L /usr/bin/pbuilder-$i ] || (cd /usr/bin ; sudo ln -s pbuilder-dist pbuilder-$i)
  [ -L /usr/bin/pbuilder-$i-i386 ] || (cd /usr/bin ; sudo ln -s pbuilder-dist pbuilder-$i-i386)

  # Create pbuilder chroots if they do not already exist
  [ -f ${PBUILDFOLDER}/$i-base.tgz ] || pbuilder-$i create
  [ -f ${PBUILDFOLDER}/$i-i386-base.tgz ] || pbuilder-$i-i386 create

  # Update and clean pbuilder chroots, add extra packages so A05suffix hook works
  pbuilder-$i update --extrapackages "lsb-release devscripts"
  pbuilder-$i-i386 update --extrapackages "lsb-release devscripts"
  pbuilder-$i clean
  pbuilder-$i-i386 clean

  # Build in each pbuilder
  pbuilder-$i build pathway_${RELEASE}-1.dsc
  pbuilder-$i-i386 build --binary-arch pathway_${RELEASE}-1.dsc

  # Copy the resulting .deb files to a results folder under the current directory
  # No longer done 2012-10-05 JM
  mkdir -p pathway-debs-${RELEASE}
  mv -v $(find ${PBUILDFOLDER}/*_result -name "pathway*${RELEASE}*${DIST}*.deb") pathway-debs-${RELEASE}/
done

echo "$0: Completed.  Pathway version ${RELEASE} package files are in " ${PBUILDFOLDER}/*_result

