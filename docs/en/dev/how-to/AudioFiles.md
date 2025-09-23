---
title: Audio Files
nav_order: 0
---
# How to record audio files

## Format
All voice audio (dialogs, words, etc), should be **MONO**, **44.1KHz** files and should be well **trimmed** at beginning (at zero crossing!)

Level shoudl be normalized with a peak reference at -6db

In the Unity project they are saved in **MP3** format, with middle quality VBR (Variable Bit Rate).

To easy the voice audio normalization we have a macro script for Audacity: you can find it here: [Antura Normalize Vocabulary - Audacity macro.txt](Antura_Normalize_Vocabulary_Audacity_Macro.txt)

## Why?
because we have lots of voice audio files and don't want to waste app space, and voice recordings just need mono and medium compression.

## Tools
[Audacity](https://www.audacityteam.org/) is a perfect free multiplatform tool to edit audio files.

on macOS we use the free utility [xAct](http://xact.scottcbrown.org/) to batch convert audio files.