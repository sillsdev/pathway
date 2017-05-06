#!/bin/bash
export MONO_PREFIX=/opt/mono4-sil
export GDK_SHARP=${MONO_PREFIX}/lib/mono/gtk-sharp-3.0
export LD_LIBRARY_PATH=${MONO_PREFIX}/lib:${LD_LIBRARY_PATH}
export PKG_CONFIG_PATH=${MONO_PREFIX}/lib/pkgconfig:${PKG_CONFIG_PATH}
export MONO_GAC_PREFIX=${MONO_PREFIX}:${MONO_GAC_PREFIX}
export MONO_MWF_SCALING=disable
export PATH=${MONO_PREFIX}/bin:$PATH
export MONO_REGISTRY_PATH=${HOME}/.mono:${HOME}/.config/paratext/registry:${HOME}/.config/fieldworks/registry
mono ../packages/NUnit.Runners.Net4.2.6.4/tools/nunit-console-x86.exe ../output/Debug/Export/Test.dll -xml=Test-Result.xml -labels -out=Test-Output.txt -exclude=SkipOnTeamCity ${1} >Test-Log.txt
