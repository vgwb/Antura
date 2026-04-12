---
title: Quest Development Guide
---

# Quest Development Guide

> **Target audience:** Quest developers joining the Antura project.
> **Scope:** Environment setup, folder structure, scene authoring, asset library usage, and quest wiring.
> **Note:** This guide assumes English-only development with no voice-over (no localization tables or audio assets required).

**Read first:** [Quest Design Guide](quest-design.md): covers design principles, the Yarn command reference, and the full content + scripting workflow. This development guide picks up where that one leaves off.

**See also:** [Script Writing Guidelines](quest-scripts-guidelines.md): for dialogue polish and VO/localization workflows (not needed during English-only dev).

## 1. Unity Setup

Install Unity and the project as [documented here](../how-to/INSTALL.md)

### One-Time Editor Configuration

After first opening the project:

1. **Play from Bootstrap only.** Never press Play from a random scene. Set the bootstrap scene as default:
   - `Edit → Project Settings → Editor → Default Play Mode Scene` → `Assets/_core/_scenes/app_Bootstrap`

2. **Set Game window to 16:9:**
   - In the Game view dropdown, pick **1920×1080** or any 16:9 ratio.

3. **Addressables:**
   - `Window → Asset Management → Addressables → Groups`
   - Set **Play Mode Script → Fast Mode** (avoids a full build for editor testing).

4. **First-time Addressables build** (required before making any player build):
   - `Addressables Groups → Build → New Build → Default Build Script`

5. **Normalize audio addressables** (run once, and after adding new audio):
   - `Antura menu → Localization → Normalize Addressables`

## 2. VS Code Setup

### Required Extensions

| Extension | Publisher | Purpose |
|-----------|-----------|---------|
| **C# Dev Kit** | Microsoft | Full C# IntelliSense, refactor, debug |
| **Unity** | Microsoft | Unity-specific code actions & debugger attach |
| **YarnSpinner** | YarnSpinner Team | Syntax highlighting & preview for `.yarn` files |
| **GitLens** | GitKraken | Inline blame, history, branch visualization |
| **EditorConfig for VS Code** | EditorConfig | Respects `.editorconfig` (4-space indent, LF) |

### Recommended Extensions

| Extension | Purpose |
|-----------|---------|
| **TODO Highlight** | Surfaces `// (refactor):` and `// TODO:` markers |
| **Markdown All in One** | Preview for `_README design notes.md` files |
| **DotENV** | Syntax for any `.env` config files |

### VS Code ↔ Unity Integration

In Unity: `Edit → Preferences → External Tools → External Script Editor` → **Visual Studio Code**
Click **Regenerate project files** to create `.csproj` files VS Code uses for IntelliSense.

## 3. Quest Folder Structure

### Where Quests Live

All quests are under:
```
Assets/_discover/_quests/
```

Quests are organized by country prefix and a sequential number:
```
_quests/
├── _Quests List.asset        ← Central registry (add your quest here)
├── _DEV/                     ← Sandbox for experimental quests
├── _TUTORIAL/                ← Tutorial quests
├── FR_00 Geo France/         ← Country overview quest
├── FR_01 Paris/              ← City quest
├── FR_02 Angers School/
├── PL_00 Geo Poland/
└── ...
```

### Creating a New Quest Folder

1. **Pick a prefix and number.** Use the country ISO code + next available index, e.g. `IT_01`.
2. **Duplicate an existing quest folder** (e.g. `FR_01 Paris`) as your starting template.
3. **Rename all files** inside to match your new quest ID. Every file that contains the old quest name should be renamed. A typical quest folder after renaming:

```
IT_01 Rome/
├── IT_01 Rome - Quest Data.asset       ← ScriptableObject (quest metadata)
├── IT_01 Rome - Quest Prefab.prefab    ← Scene root prefab
├── IT_01 Rome - Yarn Project.yarnproject
├── IT_01 Rome - Yarn Script.yarn       ← Dialogue & logic
├── IT_01 _README design notes.md       ← Design notes (keep this!)
├── discover it_01 rome.unity           ← Main scene
├── _activities/                        ← Activity configs
├── _assets/                            ← 3D models specific to this quest
├── _cards/                             ← CardData ScriptableObjects
└── _topics/                            ← TopicData ScriptableObjects
```

> **Naming conventions:**
> - Scene file: `discover xx_NN name.unity` (all lowercase, spaces)
> - Asset files: `XX_NN Name - Asset Type.asset` (Title Case, dash separator)
> - Prefab: `XX_NN Name - Quest Prefab.prefab`

### Register the Quest

Open `Assets/_discover/_quests/_Quests List.asset` in the Inspector and add your new `QuestData` asset to the list. Without this step, the quest won't appear in the game.

## 4. Quest Data ScriptableObject

The `QuestData` asset (`.asset` file) is the metadata hub for your quest. Create or configure it via the Inspector.

### Key Fields to Fill (English-only, no VO)

| Field | Type | Notes |
|-------|------|-------|
| `Id` | string | Unique slug, e.g. `it_01_rome`. Used in code and save data. |
| `IdDisplay` | string | Short display ID shown in editor UI, e.g. `IT_01` |
| `Status` | enum | Set to `Development` while building; switch to `Production` when ready |
| `TitleEn` | string | Quest title in English |
| `Country` | enum | e.g. `Italy` |
| `Location` | LocationData | Reference to a LocationData asset |
| `Thumbnail` | AssetData | Reference thumbnail image |
| `Topics` | List\<TopicData\> | 1–4 topic assets that define the knowledge content |
| `Cards` | List\<CardData\> | Extra cards not covered by Topics |
| `Difficulty` | enum | `Tutorial`, `Easy`, `Normal`, `Difficult` |
| `Duration` | int | Estimated minutes |
| `targetAge` | enum | `Ages6to10` is typical |
| `Gameplay` | List\<GameplayType\> | e.g. `Seek`, `Story`, `Collect` |
| `questPrefab` | AssetReferenceGameObject | Addressable reference to the Quest Prefab |
| `YarnProject` | YarnProject | The `.yarnproject` asset for this quest |
| `YarnScript` | TextAsset | The `.yarn` file |

**Leave blank for English-only / no-VO dev:**
- `QuestStringsTable`: no localization table needed
- `QuestAssetsTable`: no audio table needed
- `Title` / `Description` LocalizedString references, leave unset; `TitleEn` is sufficient

## 5. Game Library: AssetData, Cards, Topics, and Prefabs

The "Game Library" is the shared pool of reusable knowledge assets and 3D prefabs that quests reference rather than re-create.

### Knowledge Hierarchy

```
TopicData
└── CoreCard (CardData)
    └── Connections[] → CardData (place, person, object, event…)
        └── Words[] → WordData (vocabulary)
```

- A **TopicData** groups a cluster of related cards around a central concept.
- A **CardData** represents one knowledge unit (a monument, a dish, a historical figure, etc.).
- **WordData** is vocabulary linked to a card.

### CardData Assets

Global cards live in `Assets/_discover/_data/Cards/`. Quest-specific cards go in the quest's own `_cards/` subfolder.

To create a new card:
- Right-click in the Project panel → `Create → Antura → Discover → Card Data`
- Fill in: `Id`, `TitleEn`, `Type` (Place, Person, Object, Event, Concept…), `Country`, `Subjects`, `Importance`
- Link an `ImageAsset` (reference an `AssetData` pointing to the image)
- Leave all LocalizedString fields empty for English-only dev

### TopicData Assets

Quest-specific topics go in the quest's `_topics/` subfolder. Global topics in `Assets/_discover/_data/Topics/`.

To create a topic:
- Right-click → `Create → Antura → Discover → Topic Data`
- Set `CoreCard`
- Add `Connections`: each entry has a `ConnectedCard`, a `ConnectionType` (e.g. `LocatedIn`, `CreatedBy`, `IsA`), and a `ConnectionStrength` (0.1–1.0)

### AssetData: Images and Media

`AssetData` is a lightweight wrapper that points to actual sprites/textures via Addressables. Global assets are in `Assets/_discover/_data/Assets/`. Reference these from `CardData.ImageAsset`.

For new images (quest-specific art):
1. Drop the image file into the quest's `_assets/` folder
2. Mark it as an **Addressable** in the Inspector
3. Create an `AssetData` ScriptableObject pointing to it
4. Reference that `AssetData` from your card

### 3D Prefabs in Scenes

Reusable 3D objects live in `Assets/_discover/Prefabs/`. The main categories:

| Folder | Contents |
|--------|----------|
| `3D Common Prefabs/` | Benches, trees, props usable in any scene |
| `Characters/` | Player avatar, NPCs, tutor characters |
| `Interactive/` | Collectible items, doors, levers, triggers |
| `Country Prefab/` | Country-specific building templates |
| `Quest Special/` | One-off prefabs for specific quests |

To add a prefab to a scene: drag it from the Project panel into the scene or hierarchy. For interactive objects, ensure the prefab has a **Collider** and the appropriate interaction script attached.

## 6. YarnSpinner: Writing Quest Dialogue

Dialogue and quest logic are written in `.yarn` files using the YarnSpinner format.

### File Setup

Every quest has two Yarn files:
- `XX_NN Name - Yarn Script.yarn`: your dialogue and logic
- `XX_NN Name - Yarn Project.yarnproject`: the project file that groups scripts

The project file must reference the script file. Open the `.yarnproject` asset and ensure your `.yarn` file is listed under **Source Files**.

### Node Anatomy

Each Yarn node maps to one dialogue moment or interaction:

```yarn
title: quest_start
position: 0,0
tags:
type: panel
color: red
actor: NARRATOR
---
<<set $TOTAL_COINS = 0>>
<<area area_intro>>
Welcome to Rome! Let's explore the Colosseum.
<<target marker_colosseum>>
Follow the arrow to reach our first stop.
===
```

**Required header fields:**

| Field | Description |
|-------|-------------|
| `title` | Unique node name used in `<<jump>>` and task references |
| `type` | `panel` (full-screen), `text` (bubble), `choice`, `quiz`, `panel_endgame` |
| `color` | Editor color coding: `red`, `blue`, `green`, `yellow` |
| `actor` | Who is speaking: `NARRATOR`, `TUTOR`, or an NPC id |

> **English-only:** Write dialogue text directly in the node body. No `#line:xxx` tags needed, only required for audio/localization lookup.

### Common Yarn Commands

| Command | Example | Effect |
|---------|---------|--------|
| `<<set>>` | `<<set $met_guide = true>>` | Set a variable |
| `<<declare>>` | `<<declare $coins = 0>>` | Declare with default value |
| `<<card>>` | `<<card colosseum>>` | Display a knowledge card |
| `<<asset>>` | `<<asset map_image>>` | Show an image |
| `<<target>>` | `<<target marker_id>>` | Point navigation arrow at marker |
| `<<area>>` | `<<area area_gate>>` | Activate an interactive zone |
| `<<action>>` | `<<action taskStart task_collect>>` | Fire a quest action |
| `<<trigger>>` | `<<trigger sfx_fanfare>>` | Fire a named event |
| `<<inventory>>` | `<<inventory add bread>>` | Add item to player inventory |
| `<<jump>>` | `<<jump talk_npc_1>>` | Jump to another node |
| `<<if>>` / `<<else>>` / `<<endif>>` | Conditionals |  |

### Choices

```yarn
title: talk_guide
type: choice
actor: TUTOR
---
Do you know what the Colosseum was used for?
-> Gladiator fights!
    That's right! #line:correct
    <<set $knows_colosseum = true>>
    <<jump reward_correct>>
-> A marketplace?
    Close, but no. It was actually an arena.
    <<jump quest_continue>>
===
```

### Endgame Node

Every quest must have a `panel_endgame` node that triggers quest completion:

```yaml
title: quest_end
type: panel_endgame
color: green
actor: NARRATOR
---
Great job! You explored the Colosseum and learned about Ancient Rome.
<<action questEnd>>
===
```

## 7. Technical Quest Setup: Step by Step

This section covers how to wire everything together in Unity so the quest actually runs.

### Step 1 - Create the Quest Scene

1. Duplicate an existing scene (e.g. `discover fr_01 paris.unity`) and rename it to `discover xx_nn name.unity`.
2. Open the new scene. Remove France-specific geometry and prefabs.
3. Keep the top-level manager objects:
   - `QuestManager` (or whatever the scene root manager is named)
   - `DialogueRunner` (YarnSpinner)
   - `DiscoverDialoguePresenter`
   - Player spawn point

### Step 2 - Create the Quest Prefab

1. In the scene hierarchy, create a new empty GameObject named `QuestPrefab_IT_01_Rome`.
2. Add all quest-specific environment objects as children.
3. Drag this GameObject to `Assets/_discover/_quests/IT_01 Rome/` to create the `IT_01 Rome - Quest Prefab.prefab`.
4. In `QuestData`, set `questPrefab` to reference this prefab's Addressable GUID.

   > To make it Addressable: select the prefab → Inspector → check **Addressable** → assign to the `Discover` group.

### Step 3 - Hook Up YarnSpinner

1. Select the `DialogueRunner` GameObject in the scene.
2. Set **Yarn Project** to `IT_01 Rome - Yarn Project`.
3. Set **Start Node** to `quest_start` (or whatever your opening node is named).
4. In `QuestData`, set:
   - `YarnProject` → the `.yarnproject` asset
   - `YarnScript` → the `.yarn` TextAsset

### Step 4 - Define Tasks

Tasks are defined directly on GameObjects in the scene (not in ScriptableObjects). For each task:

1. Create an empty GameObject under the `QuestManager` hierarchy, e.g. `Task_CollectBread`.
2. Add the `QuestTask` component.
3. Configure:
   - `Code` - unique identifier used in Yarn: `<<action taskStart task_collect_bread>>`
   - `Type` - `Collect`, `Interact`, or `Performance`
   - For `Collect`: set `ItemsContainer` (parent of collectible objects) and `ItemCount`
   - For `Interact`: set `InteractGO` (the NPC or object to interact with)
   - `TaskNode` - the Yarn node that describes this task to the player
   - `NodeSuccess` - Yarn node to jump to on completion

**Collectible items** must have:
- A `Collider` set to **Is Trigger**
- The `CollectibleItem` (or equivalent) component
- An `ItemTag` matching the task's `ItemTag` field

### Step 5 - Interactive Areas

Interactive areas are trigger zones that fire Yarn nodes when the player enters them.

1. Create an empty GameObject, add a `BoxCollider` → check **Is Trigger**.
2. Add the `DiscoverArea` (or `QuestArea`) component.
3. Set `AreaId` to match the string used in `<<area area_id>>` in Yarn.
4. Optionally set an `OnEnterNode` (Yarn node to jump to on entry).

### Step 6 - Navigation Targets (Markers)

Directional arrows on-screen point the player to a target.

1. Place an empty GameObject at the target location, e.g. `Marker_Colosseum`.
2. Give it a descriptive name matching the string in `<<target marker_colosseum>>`.
3. Register it with the `TargetManager` (typically a component on the `QuestManager` root) - drag it into the `Targets` list with a matching ID string.

### Step 7 - World Setup (Optional)

To customize environment (time of day, weather):
1. Create a `WorldSetupData` asset: right-click → `Create → Antura → Discover → World Setup Data`.
2. Configure `Time`, `Weather`, `Animals`, etc.
3. Assign it to `QuestData.WorldSetup`.

### Step 8 - Register and Test

1. Add the `QuestData` asset to `_Quests List.asset`.
2. Press Play from `app_Bootstrap`.
3. Navigate to the Discover module and select your quest.
4. Use the debug shortcut **SHIFT+C** in-editor to see available shortcuts for the current mode.
5. Press `0/1/2/3` to end a mini-game quickly when testing flow.

## 8. Knowledge Cards in a Scene (Inline Display)

When the player triggers a `<<card card_id>>` command in Yarn, the UI displays the card. The `card_id` must exactly match the `Id` field of a `CardData` asset that is referenced by the quest (via Topics or directly via `QuestData.Cards`).

**Checklist for a card to display correctly:**
- Card's `Id` matches the string in `<<card id>>`
- Card is reachable from `QuestData` (either in `Cards` list or in a `TopicData` linked from `Topics`)
- Card has `TitleEn` filled in
- Card has an `ImageAsset` assigned (or leave it null so the UI will show a placeholder)

## 9. Coding Conventions Reminder

When writing any C# scripts for a quest:

- Namespace: `Antura.Discover` (or `Antura.MiniGames.<GameName>` for game logic)
- 4-space indentation, Unix line endings
- Allman brace style for classes and methods; single-line `if` is fine for trivial guards
- No copyright headers
- Leave a `// (refactor):` comment if touching known unstable areas (see `CLAUDE.md`)
- Commit format: `feat(discover): add IT_01 Rome quest scene`

## 10. Testing and Debugging

### Playing the Quest in the Editor

Always press Play from `Assets/_core/_scenes/app_Bootstrap`, never from the quest scene directly. The bootstrap initialises all subsystems (database, player profile, teacher AI) that the Discover module depends on.

To speed up iteration:
- In the Debug panel (triple-click bottom-right of screen, requires `AppConstants.DebugPanelEnabled = true`) you can jump directly to any quest.
- `SHIFT+C` shows all available keyboard shortcuts for the current mode.
- `SPACE` skips intro/end panels.
- `0/1/2/3` force-ends an activity with 0–3 stars.

### Jumping to a Specific Yarn Node

The `DialogueRunner` component (on the scene's manager root) has a **Start Node** field. Set it to any node title (e.g. `step_c_start`) to test mid-quest without replaying the whole flow. Remember to reset it to `quest_start` before committing.

You can also type a node title into the Yarn Graph editor's search bar and double-click to jump to it visually.

### Common Problems and Fixes

| Symptom | Likely cause | Fix |
|---------|-------------|-----|
| Quest doesn't appear in the Discover menu | Not added to `_Quests List.asset` | Add `QuestData` to the list |
| Yarn script runs but quest doesn't load | `questPrefab` Addressable reference is empty or GUID is wrong | Re-assign in QuestData Inspector; rebuild Addressables |
| `<<card card_id>>` shows nothing / placeholder | Card ID typo, or card not linked to QuestData | Check `Id` field on CardData exactly matches the command string; verify card is in Topics or Cards list |
| `<<area area_id>>` doesn't restrict movement | Area ID case mismatch, or DiscoverArea component missing | IDs are case-sensitive; confirm component is on the collider object |
| `<<activity settings_id>>` never starts | Activity Settings asset missing or ID mismatch | Create the asset via `Create → Discover → Activity → [type]Settings`; name must exactly match the command string |
| `<<target marker_id>>` shows no arrow | Marker not registered in TargetManager, or ID mismatch | Add the marker GameObject to TargetManager's Targets list with the correct ID |
| `<<action action_id>>` does nothing | Action not wired in ActionManager | Add an entry in the scene's ActionManager component pointing to the correct method/GameObject |
| Interactable doesn't open dialogue | Yarn Node Title field empty or wrong | Set it to exactly the node title in your `.yarn` file |
| Prefab changes not reflected in scene | Prefab instance overrides are blocking | In hierarchy, right-click prefab instance → **Prefab → Revert** |
| Addressables error on play | Fast Mode not set, or Addressables group not built | `Addressables Groups → Play Mode Script → Fast Mode`; rebuild if needed |
| Card audio plays but no text visible | `TitleEn` field is empty on CardData | Fill in `TitleEn` |
| Quest freezes at a `<<if>>` block | Variable used without `<<declare>>` | Declare all quest-specific variables at the top of `quest_start` |

### Testing the Full Flow Without VO or Localization

Since we're working English-only with no VO:

- `#line:` tags on dialogue lines are **not required** during dev. The game will display the raw text without them.
- `QuestStringsTable` and `QuestAssetsTable` in QuestData can be left unassigned,  the UI falls back to `TitleEn` and English text in the Yarn script.
- Audio replay buttons will be inactive (no audio asset), this is expected behaviour during dev.

### Useful Unity Console Filters

In the Console window, filter by these log prefixes to isolate quest-related output:

- `[QuestManager]`: quest lifecycle events (start, end, task transitions)
- `[YarnAntura]`: Yarn command execution trace
- `[Addressables]`: asset loading errors
- `[Activity]`: activity launch and result events

### Validating Before Committing

Before pushing a branch:
1. Run the quest end-to-end at least once from `app_Bootstrap`.
2. Confirm `quest_end` is reached and the star/card screen appears.
3. Check the Unity Console has no errors (warnings are acceptable).
4. Verify the quest `Status` field in QuestData is still set to `Development` (don't accidentally ship WIP content).

---

## 11. Quick Reference Checklist

When setting up a new quest from scratch, work through this list in order:

- [ ] Create quest folder `XX_NN Name/` under `_quests/`
- [ ] Create `QuestData` asset, fill `Id`, `TitleEn`, `Country`, `Status: Development`
- [ ] Create `TopicData` + `CardData` assets in `_topics/` and `_cards/`
- [ ] Link Topics and Cards to QuestData
- [ ] Write `.yarn` script with at minimum `quest_start` and `quest_end` nodes
- [ ] Create Yarn Project asset, link script to it
- [ ] Create quest scene, keep manager GameObjects, replace environment
- [ ] Build Quest Prefab from scene hierarchy, mark Addressable
- [ ] Assign `questPrefab` (Addressable ref) in QuestData
- [ ] Assign `YarnProject` and `YarnScript` in QuestData
- [ ] Add Tasks to scene for each gameplay objective
- [ ] Add Interactive Areas matching `<<area id>>` commands
- [ ] Add Navigation Markers matching `<<target id>>` commands
- [ ] Add QuestData to `_Quests List.asset`
- [ ] Play from `app_Bootstrap` and verify quest appears and runs end-to-end
