---
title: Game Architecture and Modules
---

# Game Architecture and Modules

## Core architecture

- [Project Structure](./ProjectStructure.md) overview of folders, assemblies, and conventions.
- [Application Flow](./ApplicationFlow.md) the app lifecycle and manager-driven scene orchestration.
- [MiniGame](./MiniGame.md) how minigames are structured and integrated with the core.
- [Teacher AI](./Teacher.md) provides learning content to minigames (selection, difficulty, timing).
- [Journey](./Journey.md) progression model and how content unlocks over time.

## Data and persistence

- [Database](./Database.md) data sets used across the app.
- [Database Schemas](./DatabaseSchemas.md) table structures and relationships.
- [Database Management](./DatabaseManagement.md) import/export, migration, and validation.
- [Data Flow](./DataFlow.md) path from DB to Teacher to minigames.
- [Player Profile](./PlayerProfile.md) multiple profiles per device and save data handling.

## Localization and content pipeline

- [Localization](./Localization.md) text/audio tables, Addressables, and per-locale groups.
- [Arabic Rendering](./ArabicRendering.md) RTL specifics, shaping, and UI considerations.
- [Cat Update](./catupdate.md) catalog/addressables update notes and workflows.

## Systems and telemetry

- [Logging](./Logging.md) logging categories, sinks, and usage.
- [Analytics](./Analytics.md) events, funnels, and reporting pipelines.

## Rendering and assets

- [Shaders](./Shaders.md) list and notes on shaders used in the project.

## Gameplay and characters

- [Antura and LivingLetters](./AnturaLivingLetters.md) character systems and interactions.
