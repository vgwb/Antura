# Project Structure

* TOC
{:toc}

This document describes the organization of the project folders.

## Folders

The project is separated into 4 main folders:

- `_app` contains all assets and scripts related to the general application. Under `_scripts`, the code for each of the app's subsystems is in its own folder.
- `_games` contains all assets and scripts related to the MiniGames.
  - Each MiniGame has its own sub-folder, with the game name as the folder name.
  - `_gametemplate` contains a template for creating a new MiniGame from scratch.
- `_manage` contains scenes useful for previewing and/or manage data.
- `_tests` contains scenes and scripts for internal testings.

Other folders have special purposes:

- `Plugins` contains device specific plugins.
- `Resources` contains settings and data that is loaded at runtime.
- `Standard Assets` contains third party plugins and code.
