---
title: Discover Quest Design
---

# Discover Quest Design

*Learn with Antura* uses **DiscoverEd**, an in-house framework for building playable, educational narrative-driven quests. Each quest is a 5–15 minute session where a player explores a real location, meets characters, collects Knowledge Cards, and completes small activities, all driven by a **quest script** that controls dialogue, logic, and game commands.

A quest is built from three layers:

- **Content**: Knowledge Cards, Topics, and vocabulary
- **Script**: a text file driving all dialogue, progression, and game commands
- **Scene**: a Unity 3D scene wired to the script through Interactables, Tasks, Areas, and Activities

> [!note] About the tech stack
> DiscoverED is currently working with **Unity**, but is an architecture that can be easily ported to new platforms and engines.
> [Yarn Spinner](https://www.yarnspinner.dev/) is an opensource Interactive Narrative tool


---

## Reading order

There are three guides in this section. Read them in order the first time; refer back to individual sections as needed.

| # | Guide | Who reads it | When |
|---|-------|-------------|------|
| 1 | [Quest Design Guide](quest-design.md) | Designer + developer | Before opening Unity or Yarn — covers principles, structure, Yarn commands, and the full design process |
| 2 | [Quest Development Guide](quest-development.md) | Developer | When building the scene — Unity setup, folder structure, wiring, debugging |
| 3 | [Script Writing Guidelines](quest-scripts-guidelines.md) | Anyone editing dialogue | When polishing Yarn text for VO recording or translation (English-only devs can defer this) |


## Workflow for a new quest

```
1. Read the Design Guide         → understand principles & commands
2. Fill in _README design notes  → write the quest design before touching Unity
3. Create quest folder & assets  → Topics, Cards, Yarn script
4. Follow the Development Guide  → set up Unity scene, wire everything, test
5. Polish dialogue (if VO/l10n)  → apply Script Writing Guidelines
```

A blank quest README template is available at [`_README-template.md`](_README-template.md) — copy it into your quest folder and rename it before you start writing.

---

## Related documentation

- [How To: Install](../how-to/INSTALL.md) — first-time Unity project setup
- [How To: Developer Guidelines](../how-to/DeveloperGuidelines.md) — coding conventions
- [How To: Localization](../how-to/Localization.md) — adding languages and VO (post-English-dev step)
- [Game Modules overview](../game-modules/index.md) — where Discover fits in the broader app
