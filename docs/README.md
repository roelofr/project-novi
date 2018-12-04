# Project Novi

This is the Project Novi, a project operated in 2014 to create brand new software
to run on the smartscreens located in the T-lobby of Windesheim University of Applied Sciences.

## Purpose

The purpose of the software was to give passers-by a quick method of finding their classroom,
view the weather (without looking outside) and to check for departures of their train from
Zwolle Station.

It's supposed to draw attention, so it's very bright and colourful.

## Browse the code

The code is written in C# using a C# Windows Forms application. This means you can
only *really* work on the project using a Windows install and Visual Studio installed.

If you're running on Mac or Linux, you can just [browse the source files](../src/) instead.

## Read the documentation

This directory contains the (Dutch) documentation for the Project Novi application.

It's recommended reading order is as follows:

1. [Functioneel Ontwerp](functioneel-ontwerp.md)
2. [Technisch Ontwerp](technisch-ontwerp.md)
3. [Handleiding](handleiding.md)

The [Testrapport](testrapport.md) is more of a reference of the most-recently executed tests.

### The documentation as PDF

Since our documentation comes shipped with a [LaTeX](https://www.latex-project.org/) template and is written in Markdown, we can use
[Pandoc](https://pandoc.org/) to convert it to a nicely formatted and indexed PDF file.

#### Download a release

The releases come with a PDF bundle containing all documentation. See the GitHub
releases page for the bundle.

#### Build natively (Linux, Mac)

If you've got LaTeX and Pandoc locally installed, you can simply use the makefile
to build the docs. Simply run the following command in the `docs/` directory to make
the PDF files:

```
make all
```

#### Build via Docker (Linux, Mac, Windows)

You can also build the assets using a Pandoc container for Docker. Open a Unix shell
(Bash shell on Windows) and run the following command from the `docs/` directory to
make the PDF files:

```shell
docker run -v "$(pwd):/opt/docs" -w /opt/docs roelofr/pandoc make all
```

**Note**: The Docker image containing Pandoc and LaTeX is a rough 3 GB, since a LOT of packages come included.
If you're on Linux or Mac, I'd highly recommend you install LaTeX and Pandoc locally instead.
