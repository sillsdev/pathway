#!/bin/bash

# build-binary-packages.sh
#
# Build binary packages from a source package
#
# Usage: build-binary-packages SRCPKG [DISTRO ...]
#
#        SRCPKG    the .dsc file of a source package (eg pathway_1.13.4.dsc)
#        DISTRO    a distro name to build for (eg trusty)
#
# Authored:  2012-10-03 by Jonathan Marsden <jmarsden@fastmail.fm>
# Revised    2015-01-21 by Bill Martin to include "utopic"
# Revised    2015-12-18 by Greg Trihus to apply to Pathway (add branch)
# Refactored 2016-01-20 by Neil Mayhew (originally release.sh)

set -e # Exit on error

SRCPKG=${1:?}
DISTROS=${@:2}
DISTROS=${DISTROS:-"precise trusty wily xenial sid"}

PBUILDFOLDER=${PBUILDFOLDER:-~/pbuilder}
DEVTOOLS="pbuilder devscripts fakeroot ubuntu-dev-tools"

# Install development tools as required
if ! dpkg-query -W $DEVTOOLS >/dev/null
then
    echo "$0: Installing $DEVTOOLS"
    sudo apt-get update
    sudo apt-get install -y --no-install-recommends $DEVTOOLS
fi

rm -f ~/.pbuilderrc
# Install ~/.pbuilderrc unless there already is one
[ -f ~/.pbuilderrc ] || cat >~/.pbuilderrc <<"EOF"
# Codenames for Debian suites according to their alias.
UNSTABLE_CODENAME="sid"
TESTING_CODENAME="wheezy"
STABLE_CODENAME="squeeze"

# List of Debian suites.
DEBIAN_SUITES=($UNSTABLE_CODENAME $TESTING_CODENAME $STABLE_CODENAME
    experimental unstable testing stable)

# List of Ubuntu suites. Update these when needed.
UBUNTU_SUITES=(
  xenial wily vivid utopic
  trusty saucy raring quantal
  precise oneiric natty maverick
  lucid karmic jaunty intrepid hardy gutsy)

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
BINDMOUNTS=$BUILDRESULT # To allow results to be saved

PKGNAME_LOGFILE=yes
PKGNAME_LOGFILE_EXTENTION=_$ARCH.build # Pre-0.216 (xenial)
PKGNAME_LOGFILE_EXTENSION=_$ARCH.build

## Use local packages from ~/pbuilder/deps
#DEPDIR=~/pbuilder/deps
#OTHERMIRROR="deb file://"$DEPDIR" ./"
#BINDMOUNTS=$DEPDIR
HOOKDIR=~/pbuilder/hooks
#EXTRAPACKAGES="apt-utils"

if [ "$DIST" = "lucid" ];then
    # Running an Ubuntu Lucid chroot. Add lucid-updates mirror site too.
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

cd ~/*/
debchange --local=+$DIST "Build for $DIST"
debchange --release --distribution=$DIST ""
# Removed unwanted trailing 1 from distribution suffix
sed -i -e "1s/+${DIST}1/+${DIST}/" debian/changelog
EOF
  chmod 0755 ${PBUILDFOLDER}/hooks/A05suffix
fi

PKGNAME=$(sed -n "/^Source: /s///p" "$SRCPKG")
PKGVERSION=$(sed -n "/^Version: /s///p" "$SRCPKG")
PKGARCHES=$(sed -n "/^Architecture: /s///p" "$SRCPKG")

has-arch() { grep -qw "$1" <<<"$PKGARCHES"; }

for DIST in $DISTROS; do
  export DIST	# $DIST is used in .pbuilderrc

  for ARCH in amd64 i386; do

    # See if we need to build for this architecture
    if [ $ARCH = amd64 ]; then
      has-arch any || has-arch $ARCH || has-arch all || continue
    else
      has-arch any || has-arch $ARCH || continue
    fi

    # Create pbuilder chroot if it doesn't already exist
    BUILDTYPE=$DIST
    if [ $ARCH != $(dpkg --print-architecture) ]; then
      BUILDTYPE+=-$ARCH
    fi
    [ -f $PBUILDFOLDER/$BUILDTYPE-base.tgz ] || pbuilder-dist $DIST $ARCH create

    # Update and clean pbuilder chroot, add extra packages so A05suffix hook works
    pbuilder-dist $DIST $ARCH update --extrapackages "lsb-release devscripts"
    pbuilder-dist $DIST $ARCH clean

    # Build in pbuilder
    if [ $ARCH = amd64 ]; then
      # Don't build a source package - we already have one
      BUILDOPTS="--debbuildopts -b"
    else
      # Don't build arch-independent packages - that's done with the default arch
      BUILDOPTS="--binary-arch"
    fi
    pbuilder-dist $DIST $ARCH build $BUILDOPTS "$SRCPKG"
  done

  ls -lR $PBUILDFOLDER/${DIST}_result

  # Move the resulting .deb files to a results folder under the current directory
  mkdir -p debs/$DIST
  dcmd mv -f $PBUILDFOLDER/${DIST}_result/"${PKGNAME}_${PKGVERSION}"*.{build,changes} debs/$DIST/
done

dcmd ls -l debs/*/"${PKGNAME}_${PKGVERSION}"*.{build,changes}

echo "$0: Completed"
