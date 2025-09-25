---
title: Design Discover Quests
---

# Design Discover Quests

Any Quest of the Discover Module is composed of a Unity Scene and an Yarn script.

## Design Doc
We always start with a Design Doc (usually Google Doc) with these important content:

- **Title**:
- **Description**: short introduction 
- **Knowledge Content**: what will the player learn / what answers will be able to give?
- **Mission**: what the player must do finish the quest (gameplay / objective)
- **Characters**: who will we talk to?
- **Assets**: what 3D / images / sound do we need for this quest?
- **Environment**: where does the play take place?
- **Story / Flow**: detailed description of the gamplay


## Unity

### QuestData
`QuestData` must be added to the `/Discover/_data/` folder and then to the QuestsList (to be listed)

### Scene
A scene has all the standard prefabs and these two main Game Objects:

**World**  
(all static / common 3D assets), that can be shared among different quests

**Quest**  
that has all the dynamic elements of this quest

### Components
**ActionManager**  
Is added to the Level GO.

**Interactables**
this is the main component to add to and interative object.
it can triggers Homer Nodes or Unity Actions

## Assets
We can use any of these 3D assets: <https://kenney.nl/assets/category:3D?sort=update>    
Most of them are already prefabs in the Unity Project

We can also use monuments assets from the game [Hexplorando](https://store.steampowered.com/app/2736590/Hexplorando/)

The 3D assets must be **low-poly**, flat textures.

All assets must stay inside the Quest folder.

### Images
We need small images.
These are the formats:

- JPG 85% quality or PNG (if alpha is needed)
- 256 px for small icons
- 512 px for medium icons / small images
- 1024 px for big mages
(dmensions are for the shorter side)
let's use multiple of 4 for dimensions

let's register the original url of the image in the Source field of AssetData

## Translations and Audio

The default language is english.  
We'll make all translations and validations.
And later we'll create all the voice overs.


## Script writing

### 🎙️ Voice Actors

| Enum String | Description | Recommended Use |
| ------------- | --- | --- |
| **Default**   | Female, neutral, clear diction, mid pitch, minimal emotion.                    | System voice, UI prompts, fallback lines.                      |
| **Silent**    | No spoken words, but can use non-verbal sounds (sighs, laughs, effort grunts). | Animals, mute NPCs, pantomime puzzles.                         |
| **Narrator**  | Male, warm, engaging, enthusiastic.                                            | Main storyteller voice for quests, intros, dramatic narration. |
| **Special**   | FX voice, such as a robot, creature, or animal.                                | Robots, magical beings, non-human characters.                  |
| **Adult\_F**  | Female adult voice, warm, conversational, adaptable.                           | Parents, teachers, villagers, shopkeepers.                     |
| **Adult\_M**  | Male adult voice, natural, medium pitch, flexible tone.                        | Guards, mentors, everyday adult NPCs.                          |
| **Senior\_F** | Elder female, warm, gentle rasp, storyteller cadence.                          | Wise old lady, grandmother, oracle.                            |
| **Senior\_M** | Elder male, deeper, slower, authoritative but kind.                            | Village elder, grandfather, historian.                         |
| **Kid\_F**    | Young female, bright, energetic, quick speech, high pitch.                     | Schoolmates, playful side characters.                          |
| **Kid\_M**    | Young male, playful, curious, lively tone, mid-high pitch.                     | Curious friend, companion, playful NPC.                        |


