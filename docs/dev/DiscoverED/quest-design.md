# Design Discover Quests (for designers)

Any Quest of the Discover Module is composed of a Unity Scene and an Homer flow.


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

## Homer

- the nodes that need to be called from Unity need a "permalink".
- we send "commands" from Homer to Unity using Metadata

### Node Metadata

**Permalink**  
unique string. needed to call a specific node from Unity

**Assets**  
We can upload images that will be downloaded into the Unity project
_any_ image needs to have a displaimer about copyright (used for the credits)
we can't use copyrighted images.

**native**  
If true, the node is displayed first in native language (useful for introdctions)

**Action**  
Triggers an action in Unity when displaying the node.

**Action_Post** 
Triggers an action in Unity when closing the node.

**NextTarget**
wants the code of a target defined in Unity, to point the Camera and show where to go next

**Mood**  
not used yet. will be used to change tone of voice.

**Balloon Type**
To change the balloon graphics
Choices are:

- quiz
- speech (default)
- whisper
- thought

## Unity

### QuestData
`QuestData` must be added to the `/Discover/Data/` folder and then to the QuestsList (to be listed)

### Scene
A scene has all the standard prefabs and these two main Game Objects:

**World**  
(all static / common 3D assets), that can be shared among different quests

**Level**  
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

Images are imported from Homer. and we can add custom textures if needed.

All assets must stay inside the Quest folder.

## Translations and Audio

The default language is english.  
We'll make all translations and validations.
And later we'll create all the voice overs.

