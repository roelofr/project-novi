#!/usr/bin/env bash

# Get git root and go there
ROOT_DIR="$( git rev-parse --show-toplevel )"
cd "$ROOT_DIR"

# Create a build function that uses Docker. Avoid the user
# having to install the entire LaTeX + Pandoc toolchain.
function build() {
	filename=$1

    echo -e "Building \033[0;34m${filename}.md\033[0m..."

    docker run \
        --rm \
        --volume "${ROOT_DIR}/doc:/data" \
        pandoc/latex:2.19 \
        --table-of-contents \
        --variable=lang=dutch \
        --variable=documentclass=report \
        --template="default.latex" \
        --output="build/${filename}.pdf" \
        "${filename}.md"

	echo -e "\033[0;32mBuild completed\033[0m"
}

args=("$@")

# If no arguments are passed, build all files
if [ -z $args ]; then
	echo -e "\033[0;33mNo files specified, building all files\033[0m"
	args=("fo" "to" "manual" "test")
fi

# Stop on failure
set -e

# Ensure the build directory exists
test -d doc/build || mkdir doc/build

# Yee haw 🤠
for filename in "${args[@]}"; do
	case $filename in
		"fo")
			build "functioneel-ontwerp"
			;;
		"to")
			build "technisch-ontwerp"
			;;
		"manual")
			build "handleiding"
			;;
		"test")
			build "testrapport"
			;;
		*)
			echo -e "\033[0;31mInvalid file: ${filename}\033[0m"
			exit 1
			;;
	esac
done
