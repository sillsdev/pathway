#!/bin/sh

debchange -v "${1?}" "${2:-CI build}"
debchange -r ""
