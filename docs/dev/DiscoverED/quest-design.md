# How to Design Antura Quests

## Homer

- the nodes that need to be called from Unity need a "permalink".
- we send "commands" to Unity by Metadata
  - `Asset`
  - `Action`
  - `ActionPost`
  - 


## Unity

a quest needs:

1. `QuestData` in the `/Discover/Data/` folder
2. a scene which has these two main components:
    - **World** (all static / common 3D assets), that can be shared among different quests
    - **Level** that has all the dynamic elements of this quest

**Interactables**
