---
title: Install and Configure
---
# Install and Configure

## Requirements

- Mac or Windows computer
- On Windows install Git: <https://git-scm.com>

## Install Unity

1. Download **Unity Hub** from [unity.com/download](https://unity.com/download).
2. In Unity Hub → **Installs** → **Install Editor** → select **Unity 6.3 LTS** (Unity 6.x series).
3. During installation, add these modules:
   - **Android Build Support** (if targeting mobile)
   - **iOS Build Support** (optional)
   - **Windows/Mac/Linux Build Support** (desktop)

## Download the Project
There are two ways:
A. the **simple** is to download the zipped package (from Clone or Download green button)

B. the **advanced** is to use a Git client to track the changes and update the project as it gets developed:

- Install a free GitHub client (<https://fork.dev> is good for developers, <https://desktop.github.com> is a simpler one for anybody)
- fetch the project from this repository <https://github.com/vgwb/Antura> (you can read <https://docs.github.com/en/desktop/overview/getting-started-with-github-desktop> for some help using GitHub Desktop)

## Open the project
 Unity Hub → **Add** → browse to the `Antura_Unity/` folder.

> **Unity version:** The project `ProjectSettings/ProjectVersion.txt` pins the exact 6.3 build. If Unity Hub offers an upgrade prompt, **decline** — always match the pinned version.

### Editor Setup

Set the Build Setup to Desktop, iOS or Android platform.  
Set the Game window to **16:9** (under iOS there is a preset called iPhone 5 Wide (16:9))  

## Play
Open the project in Unity and open scene `_app/_scenes/app_Bootstrap` and press Play

