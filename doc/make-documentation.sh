#!/bin/bash

ArgumentOne="$1"
ArgumentTwo="$2"
ArgumentThree="$3"

function buildFo() {
	pandoc --toc -V lang=dutch -V documentclass=report --template default.latex -o functioneel-ontwerp.pdf functioneel-ontwerp.md
	return $?
}
function buildTo() {
	pandoc --toc -V lang=dutch -V documentclass=report --template default.latex -o technisch-ontwerp.pdf technisch-ontwerp.md
	return $?
}
function buildManual() {
	pandoc --toc -V lang=dutch -V documentclass=report --template default.latex -o handleiding.pdf handleiding.md
	return $?
}
function buildTest() {
	pandoc --toc -V lang=dutch -V documentclass=report --template default.latex -o testrapport.pdf testrapport.md
	return $?
}

if [ -z $ArgumentOne ] || [ $ArgumentOne = "all" ]; then
	buildFo;
	buildTo;
	buildManual;
else
	if [ $ArgumentOne = "fo" ] || [ $ArgumentTwo = "fo" ] || $ArgumentThree = "fo" ]; then
		buildFo;
	fi
	if [ $ArgumentOne = "to" ] || [ $ArgumentTwo = "to" ] || $ArgumentThree = "to" ]; then
		buildTo;
	fi
	if [ $ArgumentOne = "manual" ] || [ $ArgumentTwo = "manual" ] || $ArgumentThree = "manual" ]; then
		buildManual;
	fi
	if [ $ArgumentOne = "test" ] || [ $ArgumentTwo = "test" ] || $ArgumentThree = "test" ]; then
		buildTest;
	fi
fi

exit 0
