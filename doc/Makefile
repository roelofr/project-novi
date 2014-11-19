.PHONY: all fo

all: fo to handleiding

fo:
	pandoc --toc -V lang=dutch -V documentclass=report --template default.latex -o functioneel\ ontwerp.pdf Functioneel\ Ontwerp.md

to:
	pandoc --toc -V lang=dutch -V documentclass=report --template default.latex -o technisch\ ontwerp.pdf Technisch\ Ontwerp.md

handleiding:
	pandoc --toc -V lang=dutch -V documentclass=report --template default.latex -o handleiding.pdf Handleiding.md
