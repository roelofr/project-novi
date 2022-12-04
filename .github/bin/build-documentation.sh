#!/usr/bin/env bash

set -e

# Go to root
ROOT_DIR="$( git rev-parse --show-toplevel )"
cd "$ROOT_DIR"

echo -e "Current working dir: \033[1;32m$( pwd )\033[0m"
DOC_DIR="$ROOT_DIR/doc"

# Create output dir
test -d doc/build || mkdir doc/build

for file in doc/*.md; do
    basename="$( basename "$file" )"

    echo -e "Building \033[0;34m$basename\033[0m..."

    docker run \
        --rm \
        --volume "${DOC_DIR}:/data" \
        pandoc/latex:${PANDOC_VERSION:-latest} \
        --toc \
        --variable=lang=dutch \
        --variable=documentclass=report \
        --template="default.latex" \
        --output="build/${basename%.*}.pdf" \
        "${basename}"
done
