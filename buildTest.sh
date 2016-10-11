#!/bin/bash
RELEASE=${1:-"1.15.3.5222"}
TARGET=${2:-"Test"}
MONO_PREFIX=/opt/mono4-sil
PATH="${MONO_PREFIX}/bin:$PATH"
LD_LIBRARY_PATH="${MONO_PREFIX}/lib:$LD_LIBRARY_PATH"
PKG_CONFIG_PATH="${MONO_PREFIX}/lib/pkgconfig:$PKG_CONFIG_PATH"
MONO_GAC_PREFIX="${MONO_PREFIX}:/usr"

export MONO_PREFIX LD_LIBRARY_PATH PKG_CONFIG_PATH MONO_GAC_PREFIX

#xbuild /t:Build /property:Configuration=Debug\;Solution=Pathway.sln Build/Palaso.proj
AssertUiEnabled=false xbuild /t:${TARGET} /property:Configuration=Debug\;Solution=Pathway.sln\;BuildTaskDir=lib\;ApplicationName=Pathway\;ApplicationNameLC=pathway\;ExtraExcludeCategories=SkipOnLinux\;TestNamePat=Test\;BUILD_NUMBER=${RELEASE} Build/Palaso.proj

