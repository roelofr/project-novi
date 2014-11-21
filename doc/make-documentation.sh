#!/bin/bash

ArgumentOne="$1"
ArgumentTwo="$2"
ArgumentThree="$3"

function buildFo() {
	`pandoc --toc -V lang=dutch -V documentclass=report --template default.latex -o functioneel\ ontwerp.pdf Functioneel\ Ontwerp.md`
	return $?
}
function buildTo() {
	`pandoc --toc -V lang=dutch -V documentclass=report --template default.latex -o technisch\ ontwerp.pdf Technisch\ Ontwerp.md`
	return $?
}
function buildManual() {
	`pandoc --toc -V lang=dutch -V documentclass=report --template default.latex -o handleiding.pdf Handleiding.md`
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
fi

exit 0