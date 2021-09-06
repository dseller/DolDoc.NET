[![DolDoc.NET](https://github.com/dseller/DolDoc.NET/actions/workflows/github-actions.yml/badge.svg)](https://github.com/dseller/DolDoc.NET/actions/workflows/github-actions.yml) [![CodeQL](https://github.com/dseller/DolDoc.NET/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/dseller/DolDoc.NET/actions/workflows/codeql-analysis.yml)

# DolDoc.NET
A .NET reimplementation of Terry Davis' [✝] [TempleOS](http://www.templeos.org) DolDoc document formatting engine. It is mostly backwards compatible with Davis' documents. I have added some improvements such as higher resolution and support custom bitmap fonts (YAFF).

__This is still heavily under development!__

## Online Blazor proof of concept

You can find a proof of concept of the DolDoc.NET formatting engine [here](http://dseller.github.io). Please note that at the time of writing, user input is not yet implemented. Also note that because DolDoc.NET is heavily under development,
it is very unoptimized. This is extra apparent in the Blazor host.

## Todo list

- [X] Implement basic document rendering
- [ ] Implement forms
- [ ] Design and implement network protocol to allow it to be used as a remote TUI
- [ ] Renderer optimizations
- And more :)

## Screenshots

The built-in file browser:

![filebrowser](https://github.com/dseller/DolDoc.NET/blob/master/Screenshots/FileBrowser.png)

Some of Davis' original documents:

![new1](https://github.com/dseller/DolDoc.NET/blob/master/Screenshots/lbVPW6EQyX.png)
![new2](https://github.com/dseller/DolDoc.NET/blob/master/Screenshots/Os6YZl4Pqf.png)

