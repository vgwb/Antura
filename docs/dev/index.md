---
title: Developer docs
---
Here are all the docs for developers and designers.
The docs are _always WIP_ please report any error or requests for details (on Discord or via GitHub issue).  
The repository of this project is: <https://github.com/vgwb/Antura>  

## Discover Quests
To create a new Quest [Design Discover Quests](DiscoverModule/index.md)  

## Game Design

- [Arabic Language](GameDesign/ArabicLanguage.md)
- [Data nalysis](GameDesign/DataAnalysis.md)
- [Learning Goals](GameDesign/LearningGoals.md)

## HowTo
**User and Tester guides**

- [Debug Shortcuts](HowTo/DebugShortcuts.md) cheats and keyboard shortcuts
- [Export Database](HowTo/ExportPlayerDatabase.md) how to export player databases

**Developer guides**

- [Install](HowTo/INSTALL.md) how to install and configure the Unity project
- [Build](HowTo/Build.md) how to build the app for mobile devices
- [Create Edition](HowTo/CreateEdition.md)
- [Collaborator](HowTo/Collaborator.md) how to collaborate
- [Developer Guidelines and Tips](HowTo/DeveloperGuidelines.md) general guidelines for developers that want to contribute to the project
- [Refactoring Guidelines](HowTo/RefactoringGuidelines.md)
- [Export Google Sheet Data](HowTo/ExportGoogleSheetData.md) as JSON files
- [Drawing Font](HowTo/DrawingsFont.md) how to create the drawings font Atlas with TextMeshPro
- [How To regenerate DoxyGen docs](HowTo/APIDocsGeneration.md)

## Core Modules / Architecture

- [Project Structure](Modules/ProjectStructure.md) the structure of the Unity project.
- [Application Flow](Modules/ApplicationFlow.md) the complete flow of the application, useful if you want to understand the call flow.
- [Antura and LivingLetters](Modules/AnturaLivingLetters.md)
- [Arabic Rendering](Modules/ArabicRendering.md)
- [Database](Modules/Database.md) the datasets used everywhere.
- [Database Schemas](Modules/DatabaseSchemas.md) details on db tables and schemas
- [Database Management](Modules/DatabaseManagement.md)
- [Data Flow](Modules/DataFlow.md) how the learning content data flows from the database, to the teacher system, and finally to minigames.
- [Journey](Modules/Journey.md)
- [Localization](Modules/Localization.md)
- [Logging](Modules/Logging.md) the implementation and use of the Logging System.
- [MiniGame](Modules/MiniGame.md) how a MiniGame works, how to create new ones and link them to the core application.
- [Player Profile](Modules/PlayerProfile.md) how the Player Profile Manager works, a subsystem that allows the use of several profiles on the same device.
- [Shaders](Modules/Shaders.md) the list of shaders used in this project.
- [Teacher AI](Modules/Teacher.md) how the Teacher System works. This subsystem is responsible for providing learning content data to the minigames.
- [Analytics](Modules/Analytics.md) the implementation and use of the Logging System.

## MiniGames docs

- [Assessments](Minigames/Assessments.md)
- [Balloons](Minigames/Balloons.md)
- [ColorTickle](Minigames/ColorTickle.md)
- [DancingDots](Minigames/DancingDots.md)
- [Egg](Minigames/Egg.md)
- [FastCrowd](Minigames/FastCrowd.md)
- [HideAndSeek](Minigames/HideAndSeek.md)
