#!/bin/bash
MONO_PREFIX=/opt/mono4-sil
PATH="${MONO_PREFIX}/bin:$PATH"
LD_LIBRARY_PATH="${MONO_PREFIX}/lib:$LD_LIBRARY_PATH"
PKG_CONFIG_PATH="${MONO_PREFIX}/lib/pkgconfig:$PKG_CONFIG_PATH"
MONO_GAC_PREFIX="${MONO_PREFIX}:/usr"

export MONO_PREFIX LD_LIBRARY_PATH PKG_CONFIG_PATH MONO_GAC_PREFIX

xbuild /t:Build /property:Configuration=Debug\;Solution=Pathway.sln build/Palaso.proj
AssertUiEnabled=false xbuild /t:Test /property:Configuration=Debug\;Solution=Pathway.sln build/Palaso.proj