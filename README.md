[![DolDoc.NET](https://github.com/dseller/DolDoc.NET/actions/workflows/github-actions.yml/badge.svg)](https://github.com/dseller/DolDoc.NET/actions/workflows/github-actions.yml) [![CodeQL](https://github.com/dseller/DolDoc.NET/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/dseller/DolDoc.NET/actions/workflows/codeql-analysis.yml)

# DolDoc.NET

A .NET reimplementation of Terry Davis' [✝] [TempleOS](http://www.templeos.org) DolDoc document formatting engine. It is mostly backwards compatible with Davis' documents. I have added some improvements such as higher resolution and support for custom bitmap fonts ([YAFF](https://github.com/robhagemans/hoard-of-bitfonts)).

**This is still heavily under development!**

## Sponsors

[<img src="https://resources.jetbrains.com/storage/products/company/brand/logos/jb_beam.png" width="128" height="128">](https://jb.gg/OpenSourceSupport)

Jetbrains has kindly provided me with a Jetbrains Rider open source license for developing this project. Thank you Jetbrains! 

## Online Blazor proof of concept

You can find a proof of concept of the DolDoc.NET formatting engine [here](http://dseller.github.io).

__The Blazor POC is a very old version of DolDoc.NET! Please use the native application from this repository instead.__

## Todo list

- [x] Implement basic document rendering
- [x] Implement forms
- [x] Window compositor
- [x] Renderer optimizations
- [ ] Fix editor inaccuracies
- [ ] Extend forms
- [ ] Design and implement network protocol to allow it to be used as a remote TUI
- [ ] Implement HolyC compiler + VM
- [ ] Migrate away from OpenGL legacy

## Examples

There are a few examples in the Examples folder:

- Shell: an experimental shell using Lua as its scripting language.
- FileBrowser: simple file browser using TempleOS directory listings.
- SimpleForm: a test form that uses C# bindings.
- TodoList: a test todo list app using C# bindings.

## Screenshots

The shell example:

![shell](https://github.com/dseller/DolDoc.NET/blob/master/Screenshots/shell.png)

The file browser example:

![filebrowser](https://github.com/dseller/DolDoc.NET/blob/master/Screenshots/FileBrowser.png)

The todo list app:

![todo](https://github.com/dseller/DolDoc.NET/blob/master/Screenshots/todolist.png)

Some of Davis' original documents:

![new1](https://github.com/dseller/DolDoc.NET/blob/master/Screenshots/lbVPW6EQyX.png)
![new2](https://github.com/dseller/DolDoc.NET/blob/master/Screenshots/Os6YZl4Pqf.png)
