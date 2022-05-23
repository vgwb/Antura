---
layout: default
title: Audio Files
parent: HowTo
nav_order: 0
---
# How to record audio files

## Format
All voice audio (dialogs, words, etc), shoudl be **MONO**, **44.1KHz** files and should be well **trimmed** at beginning and end.

In the Unity project they are saved in **MP3** format, with middle quality VBR (Variable Bit Rate).

## Why?
because we have lots of voice audio files and don't want to waste app space, and voice recordings just need mono and medium compression.

## Tools
[Audacity](https://www.audacityteam.org/) is a perfect free multiplatform tool to edit audio files.

and on macOS we use the free utility [xAct](http://xact.scottcbrown.org/) to batch convert audio files.