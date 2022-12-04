.PHONY: all fo

all: fo to handleiding test

fo:
	pandoc --toc -V lang=dutch -V documentclass=report --template default.latex -o functioneel-ontwerp.pdf functioneel-ontwerp.md

to:
	pandoc --toc -V lang=dutch -V documentclass=report --template default.latex -o technisch-ontwerp.pdf technisch-ontwerp.md

handleiding:
	pandoc --toc -V lang=dutch -V documentclass=report --template default.latex -o handleiding.pdf handleiding.md

test:
	pandoc --toc -V lang=dutch -V documentclass=report --template default.latex -o testrapport.pdf testrapport.md
