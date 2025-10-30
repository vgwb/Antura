---
title: Dev guide
nav_order: 0
---
# Developer Guidelines and Tips

Developers should follow these guidelines for contributing to the project.

## Coding conventions

- Indent using four spaces (no tabs)
- Use Unix newline
- Use [Allman style](http://en.wikipedia.org/wiki/Indent_style#Allman_style) braces, but for `if` and `for`
- Use **camelCase** for internal and private fields
- Use **CamelCase** for public fields
- Use **CamelCase** for all methods, public and private, for classes, enum types and enum values.
- Use **THIS_STYLE** for constants.
- Regions can be used to group code logically. No nested regiones. Use **CamelCase** for region names.
- No copyright notice nor author metadata should be present at the start of the file, unless it is of a third party
- **Never** commit if you encounter compilation errors or warnings.

## Naming conventions

- Use *MiniGame*, not *Minigame* (but *minigame* when lowercase)
- All data related to the learning content should be referred to as *Vocabulary data* (instead of the triad Letter/Word/Phrase)
- All data related to the journey progression should be referred to as *Journey data* (which consists of Stage + LearningBlock + PlaySession)

## Namespaces

The whole codebase is under the **Antura** namespace.
The main systems can be accessed through the Antura namespace and thus fall under it.

All minigames are under the **Antura.MiniGames** namespace.
Each minigame needs its own namespace in the form of **Antura.MiniGames.GAME_ID** with GAME_ID being the name of the minigame.

Most core code will be in a subsystem.
Specific subsystem code is inside a **Antura.SUBSYSTEM** namespace, where SUBSYSTEM is the subsystem's name.
What follows is a list of subsystems with their namespaces:

- **Antura.Core** for the core managers and data of the application.
- **Antura.AnturaSpace** for code related to the Antura Space scene.
- **Antura.PlayerBook** for code related to the Player Book scene.
- **Antura.GamesSelector** handles the Games Selector scene.
- **Antura.Animations** for general animation utilities.
- **Antura.Database** for database access and organization.
- **Antura.LivingLetters** for scripts related to the Living Letter characters.
- et cetera...

**Never commit anything without a namespace, nor anything under the root Antura namespace**

## Git Ignore

there are several fiels and directories put under Git Ignore.. the useful from Dev POV are:

```bash
Local/
Local.meta
```

if you create a Assets/Local directory, you can put inside whatever personal you want, and won't be versioned.

## Coding Guidelines
- We can use any external plugin, obviously the fewer the better
- All code and app architecture must be self speaking (in English) to be easily read by new/external contributors
- Flexibility: no hardcoded vars. Anything should be parameterized (for app variations in other languages or modular evolution)
- Git commit rule: One feature is One commit rule
- Commit a feature with all its dependencies when it’s working and tested at least once
- The commit message should be a simple description of what has been done (the Git log will be used to generate the app changelog so pay attention), with a prefix like:
[plugin] lorem ipsum v 1.x.x
[bug] fixed xxx
[feature] added this in that
[changed] behaviour x now does y
[cgl] cleaned minigame x code     
- C# only + Unix line endings
- Use `Antura` namespace for the app, and `Antura.minigame` for specific minigames classes.
- No compilation errors (red) allowed in the dev branch. Always buildable.
- No compilation warnings (yellow) allowed in master branch (if possible)
- All the code must be released under the MIT License 
- Everyone should commit just to dev branch (eventually their own) when a feature / debug is done and tested. Do no push to origin without some local testing. Team leads will merge into master to trigger testing builds.

