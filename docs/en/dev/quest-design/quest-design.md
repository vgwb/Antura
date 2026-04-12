---
title: Quest Design & Development Guide
---

# Quest Design & Development Guide

This is a guide for creating a new **Discover Quest** in *Learn with Antura*, from the first idea to a working Unity scene.

> **Real examples**
> We invite to play all the quests in the game and [study their open content pages](../../content/quests/index.md) here in this website first.

---

## Overview: what a Quest is

A quest is a short playable session (5–15 minutes) in which the player explores a location, talks to NPCs, collects **Knowledge Cards**, and completes one or more **Activities**. Every quest teaches a specific set of educational content through story and play.

The quest is driven by a **Yarn script** (`.yarn` file) that controls all dialogues, progression logic, and game commands. The 3D scene is built in Unity and all interactive elements are wired to nodes in the script.

A quest folder contains:
```
_quests/XX_00 QuestName/
    XX_00 _README design notes.md   ← design document (you write this first)
    XX_00 QuestName - Yarn Script.yarn
    XX_00 QuestName - Yarn Project.yarnproject
    XX_00 QuestName - Quest Data.asset
    XX_00 QuestName - Quest Prefab.prefab
    _assets/                         ← quest-specific art assets
```

## How to Write a Good Quest

This section is the most important part of the guide. Read it before touching Unity or Yarn. Good quests are built top-down: 

**principle → structure → content → script**. 

A quest that breaks these principles will feel like a classroom lesson dressed up as a game, and children will switch off.

### Core design principles

**1. One purpose per quest**
Each quest targets exactly one topic cluster, one behavioral goal (help someone / fix something / discover a place), and 5–10 key words. If you find yourself writing 15 cards for one quest, split it into two quests.

**2. Short and chunked**
Hard stop at **12–13 minutes**. An optional bonus section can extend to 15'. Target 3–4 scenes per quest. Each scene follows the same micro-pattern: short NPC moment → one activity → tiny reflection. Children can put the device down after any scene without feeling lost.

**3. Bilingual by touch**
The target language (French, Polish, etc.) plays by default for all voice lines. Any line can be tapped to hear the native-language version. Never force the child to stay in a language they do not understand.

**4. Low cognitive load**
- Lines ≤ 8–10 words (absolute maximum: 12 words, only for lists).
- One instruction at a time. Never give two tasks in the same line.
- Icons must be unambiguous and consistent across every quest.
- Every audio line must be re-playable (ear icon, same position in every scene).
- Never remove a UI element mid-scene — if it appears, it stays until the scene ends.

**5. Show, don't lecture**
NPCs model behavior through props, gestures, and environment cues. The baker *holds* the flour bag. The Mermaid *reaches for* her missing sword. The scene *already shows* the problem before the NPC explains it. Dialogue confirms what the child's eyes already suspect.

**6. Celebrate tiny wins**
Award a small progress signal (points, card flip, sound) on *every* correct interaction, including wrong answers (softer sound, "Almost! Try the red one."). Reserve stars and gems for major milestones (completing a scene, finishing the quest). Avoid long stretches of play with no feedback.

**7. No dead ends**
After two failed attempts at any activity, offer a graceful skip path with reduced reward. A child should never feel blocked. The skip path teaches the same content but removes the competitive pressure.

**8. Repeatable audio everywhere**
Every spoken line and every card has a replay button. This is not a nice-to-have — it is the core language-learning mechanic. Design each dialogue node assuming the child will tap replay 3–5 times.

---

### Vocabulary targets per quest

- **5–10 words**, tightly themed (do not mix topics).
- Include **at least one social phrase**: *please*, *thank you*, *excuse me*, *hello*, *help me*.
- Each word must appear a **minimum of 3 times** across the quest in different contexts: once when introduced (card shown), once in activity, once in final assessment.
- Display format: picture + written label + audio. Tapping cycles target language → native language.
- Show the card list in the quest detail panel *before* the child starts, so they know what they will learn.

### The quest skeleton (repeatable blueprint)

Every quest should map to this four-scene structure. The scenes can be built as separate areas of the 3D environment or as sequential phases within one area.

| Scene | Duration | What happens |
|---|---|---|
| **A — Hook** | 1–2 min | NPC intro (10–15 s). Show 3 key words as tappable word-cards (picture + audio). Quick "try it" task so the child interacts immediately. |
| **B — Practice** | 3–5 min | One activity using the vocabulary (seek 5–6 objects / sort / simple rhythm). NPC is present and reacts to progress. |
| **C — Use it** | 3–5 min | Short dialogue choice (2–3 turns using the new words). Optional: micro-parkour / "spot-the-sign" environmental puzzle. |
| **D — Wrap** | 1–2 min | Recap the words. Show one cultural tip card. Award stars (0–3). List all cards gained. Propose an offline activity (draw, discuss, make). |

Scene A must hook the child within the first 30 seconds. If the first thing they see is a wall of dialogue, the quest has already failed.

### Difficulty curve across a country's quests

A country typically has many quests with different difficulties:

| Quest Type | Level | Design focus |
|---|---|---|
| Tutorial / Safety | No failure | Tap-to-hear, follow big arrows, collect 3 items, single-button actions only. |
| First steps | Very easy | Short seek-and-find + 1 guided dialogue choice. One mechanic. |
| Pattern practice | Easy | Repeat simple request/response with variations. Introduce light 2D/3D movement. |
| Multi-step task | Medium | Order matters. Select ingredients/steps. Time buffer is generous. |
| Light challenge | Medium-hard | Optional parkour/platform route (safe alt-route always available). Jump & avoid, no pits. |
| Branching goal | Hard | Pick one of two activities for the theme. Embed a social behavior (asking for help). |
| Wrap-up / Festival | Varied | Mix of previous mechanics. Short finale with cultural flavor. Star target slightly higher. |

Keep platforming **optional or with rails / auto-jump**. Motion comfort: slow camera, wide field of view, large colliders. A child failing a jump should never feel stupid, they should feel like they want to try again.

### NPC dialogue rules

These rules apply to every spoken line in the Yarn scripts.

- **≤ 70 characters per line**, prefer Subject-Verb-Object order.
- **One idea per line**. Split any line that contains "and" or "but" joining two clauses.
- **Error recovery**: never say "Wrong!" — say `"Almost! Try the red one."` or `"Not quite. Look again."`.
- **Avoid**: sarcasm, idioms, dense humour, rhetorical questions. Favour literal + warm.
- **One cultural greeting** per quest (`Bonjour!`, `Hallo!`, `Ciao!`). After first use, switch to `Hello`.
- **Antura** is always the cause of a problem, but also always redeemable, he is clumsy, not mean. Never make children feel bad for him or against him.

### Activities and mechanics: guardrails

Use **at most 1–2 mechanics per quest**. Introducing a new mechanic costs the child 30–60 seconds of orientation that is time not spent learning.

| Mechanic | Notes |
|---|---|
| **Seek & find** | Big silhouettes, 5–6 items, hotspot hint pulse after ~15 s. Always a guaranteed completion path. |
| **Sort / match** | Food groups, transport vs. places, recycle bins. Max 6 pairs. |
| **Rhythm / tap** | 3–5 beats. Say-after-me with a visual meter. No penalty for slow response. |
| **Ask-for-help mini-dialogue** | Choose a polite opener; NPC responds warmly; unlock a shortcut. Teaches a social phrase in context. |

**Always** provide a no-stress success path: after 2 failed attempts, offer a skip with reduced KP reward. Log the skip — it is valuable design feedback.

### Emotional arc

Every quest needs a simple emotional journey for the child:

1. **Problem**: something is wrong (Antura stole it / it is lost / someone needs help).
2. **Adventure**: the child explores and collects, building competence.
3. **Climax**: the final activity or challenge, a moment of real effort.
4. **Resolution**: the problem is fixed, the NPC is happy, the child gets credit.
5. **Pride**: the card list, the stars, the offline activity proposal.

If a quest does not have all five stages, it will feel incomplete even if the content is correct.

### The "3 encounters" rule

Each card / vocabulary word must appear in at least **3 different encounters** across the quest:

1. **Introduction**, card shown by NPC, audio plays.
2. **Active use**, child picks, sorts, or hears the word in the activity.
3. **Recall**, word appears in the final assessment or wrap dialogue.

A card seen only once will not be remembered, but if seen three times in different contexts it will.

### Environmental storytelling

Design the 3D space so it *shows* the learning before the NPC *says* it. Examples from existing quests:
- The baker's counter is empty: the ingredients are visibly missing before he speaks.
- The sword holder near the Mermaid is visibly empty: the problem is communicated spatially.
- The solar system model has gaps: the missing planets are communicated visually.

This means the child arrives at an NPC having already formed a hypothesis. The NPC confirms it. That confirmation is satisfying and reinforces memory.

## Phase 1: Educational Content — Topics & Cards

**Start here, before any game design.** The quest's educational value comes entirely from what you decide to teach.

### 1.1 Define the learning goal

Answer this one question first: *What will a child know or be able to do after playing this quest that they did not know or could not do before?*

Keep it concrete and testable:
- ✅ "Name the 4 ingredients of a baguette"
- ✅ "Identify a defensive castle vs. a pleasure castle"
- ✅ "Know the order of the 8 planets from the Sun"
- ❌ "Learn about French culture" (too vague)

A single quest should cover **5–10 facts/concepts**, distributed across the play session so they feel discovered rather than recited. If you reach 12 or more, split the quest. Cognitive overload is the single biggest killer of retention in this age group.

> See **"Vocabulary targets per quest"** in the *How to Write a Good Quest* section for the word-count rules and the 3-encounters principle.

### 1.2 Define the Topics

A **Topic** is a thematic cluster of cards (e.g., `solar_system`, `french_schools`, `warsaw`). Topics appear in the player's Book and let them review everything they learned after the quest.

Write down 1–3 topics that cover your content. If a topic already exists in the database, reuse it. If it is new, note it with `TODO:` so the database team knows to create it.

### 1.3 Define the Cards

A **Knowledge Card** is the atomic unit of learning. Each card has:
- an ID (snake_case, e.g., `eiffel_tower`, `heliocentric_model`)
- a title
- an image
- short text (1–3 sentences, age-appropriate)
- audio

Cards are shown in-game via `<<card card_id>>` commands in the Yarn script.

**For each fact you want to teach, there should be a card.**

List all cards you need. Use the `// CARDS` section at the top of the Yarn script (auto-generated by the editor) to track which cards are `ready`, `BROKEN` (exists but asset is missing), or `TODO` (needs to be created).

Example from FR_01 Paris:
```
// CARDS
// - capital_paris          ← ready
// - food_baguette          ← ready
// - eiffel_tower           ← ready
// - notre_dame_de_paris    ← ready
//   - TODO gargoyle        ← needs to be created
```

**Card design rules:**
- One card = one clear concept
- Text uses present simple, 5–12 words per sentence
- Avoid jargon; keep language accessible to age 6–10
- Images should be clear, colourful, real-world photographs or illustrations

---

## Phase 2: Quest Design

With your content defined, now design the *game* around it.

### 2.1 Choose a Location

The location is the physical world the player explores. It must:
- Be real and recognizable (Paris streets, Wrocław river, Toruń old town)
- Give natural reasons to visit the places where learning happens
- Fit the educational content (you would not teach about Copernicus in Paris)

The same 3D environment can host multiple quests (e.g., Warsaw appears across several PL quests). Check existing scenes before requesting new ones.

### 2.2 Choose a Gameplay Style

Pick the dominant style for the quest. Most quests combine two or three.

| Style | Description | Example quest |
|---|---|---|
| **Collect & Deliver** | Find scattered items and bring them to a quest-giver | FR_01 Paris (baguette ingredients) |
| **Landmark Tour** | Visit a series of places, each with an NPC and a short task | PL_01 Warsaw (7 monuments) |
| **Find Your Way** | Navigate a branching space to reach the correct destination | FR_02 Angers School (find your classroom) |
| **Fix a Problem** | Someone (often Antura) caused chaos; restore order | PL_02 Wrocław Dwarves |
| **Discovery** | Visit stations to collect knowledge cards, usually linear | PL_07 Solar System (8 planets) |
| **Scavenger Hunt** | Find items hidden at architectural/contextual clues | FR_05 Loire Castles (knight vs. prince outfit) |

Choose the style that makes the **educational content feel natural**. The gameplay loop should force the player to encounter each teaching point, not skip it.

### 2.3 Write the Mission Statement

One clear sentence that tells the player what to do and why. The mission is the hook.

> *FR_01: Help a baker find the four stolen ingredients (Flour, Salt, Water, Yeast) scattered across Paris's most famous landmarks.*

> *PL_07: Rescue Antura, who is stuck in Copernicus's house — but first you must fix the Solar System map.*

### 2.4 Design the Progression

Break the quest into 3–6 steps. Each step should:
- take the player to a new place or NPC
- deliver 1–3 cards
- include one small task or activity
- unlock the next step

The steps should have a clear **narrative cause-and-effect chain** — not just a checklist. The player should always know why they are going somewhere.

**Example: FR_01 Paris progression**
1. Baker asks for help → go to Eiffel Tower (flour)
2. Return flour → baker sends you to Notre-Dame (salt)
3. Return salt → baker sends you to the Louvre (water)
4. Return water → baker sends you to Arc de Triomphe (yeast)
5. Return yeast → final activity → quest end

**Example: PL_01 Warsaw progression**
1. Mermaid's sword is stolen → trail leads to Chopin Monument
2. Chopin's notes scattered → trail leads to Wars & Sawa
3. Sawa is separated → trail leads to Sigismund Column
4. Crown pieces fallen → trail leads to Parliament (sword found!)
5. Parliament → Palace of Culture → National Stadium
6. Return sword to Mermaid → final quiz

### 2.5 Design the Characters

Every NPC in the quest needs:
- a **role** (quest-giver, landmark guide, random local)
- a **voice type** (see the actor table in Section 4)
- **2–3 things they say** (their personality, their fact, their request)

Keep NPCs functional. They exist to teach something or to move the story. Avoid NPCs that only say "hello".

**Main NPC types:**
- **Quest-giver**: starts the mission, sends the player somewhere
- **Landmark NPC**: explains the place and gives the task for that step
- **Spawned NPC**: randomly placed extras that say one educational fact from a list (no story role)

The player character is always **the Cat**. Antura (the dog) is usually the cause of problems the player must fix — he is a narrative excuse to visit places and help people.

---

## Phase 3: Activities

**Activities** are the mini-games embedded in quests. They are the primary assessment and engagement mechanic.

### 3.1 Available Activity Types

| Activity | Command | Description |
|---|---|---|
| **Memory** | `<<activity memory_id result_node>>` | Find matching pairs of cards |
| **Match** | `<<activity match_id result_node>>` | Match related cards (e.g., tool → shape) |
| **Order** | `<<activity order_id result_node>>` | Put items in correct sequence (e.g., planets from Sun) |
| **Jigsaw** | `<<activity jigsaw_id result_node>>` | Complete a puzzle image |
| **Canvas** | `<<activity canvas_id result_node>>` | Wipe away a surface to reveal the image below |
| **Money** | `<<activity money_id result_node>>` | Pay the correct amount of coins |
| **Quiz** | `<<activity quiz_id result_node>>` | Answer multiple-choice questions with card images |
| **Piano** | `<<activity piano_id result_node>>` | Repeat a musical sequence |

Each activity has a **settings asset** (created in Unity by the game designer) that defines its specific content (which cards to use, number of items, etc.). The settings asset ID is what you put in the Yarn command.

A difficulty level can be specified as an optional third parameter:
```
<<activity memory_eiffel result_node tutorial>>   // easiest
<<activity memory_eiffel result_node easy>>
<<activity memory_eiffel result_node normal>>
<<activity memory_eiffel result_node expert>>     // hardest
```

### 3.2 Placing Activities in the Quest

Activities work best when they:
- occur at the end of a step, as a "gate" to unlock the reward
- use the cards that were just shown to the player in dialogue
- feel like a natural consequence of the story ("solve the puzzle to open the chest")

**Pattern used in almost every quest:**
```
NPC explains → shows cards → "solve the puzzle!" → <<activity ...>> → chest unlocked
```

### 3.3 Checking Activity Results

To check whether the player completed an activity (and how well):
```
<<if GetActivityResult("activity_settings_id") > 0>>
    Great! You solved it.
<<else>>
    Try again!
    <<jump try_again>>
<<endif>>
```
`GetActivityResult` returns 0 for fail, 1+ for completion.

### 3.4 Final Assessment Activity

Every quest ends with a **final assessment activity** that reviews the key content. Use a `match` or `quiz` activity with cards from across the whole quest. This is the "did you learn it?" moment.

```
title: baker_finish
actor: SENIOR_M
---
Let's see if you remember what you learned in Paris. #line:076b3e3
<<activity match_paris_final final_activity_done>>
===
```

---

## Phase 4: The Yarn Script

The Yarn script is the heart of the quest. It controls all dialogue, logic, and game commands.

> **Yarn Spinner documentation** (built-in language features — variables, `<<if>>`, `<<jump>>`, `<<stop>>`, `<<wait>>`, options syntax, etc.):
> [https://docs.yarnspinner.dev/](https://docs.yarnspinner.dev/)
>
> This guide documents only the **Antura custom commands and functions** that extend Yarn Spinner. For everything else — conditionals, loops, standard variables, built-in commands — refer to the official Yarn docs above.

---

### 4.1 File Header

Every quest script starts with a comment block identifying the quest and listing available scene elements. The `<scene_data>` block is **auto-generated by the Unity editor — do not edit it manually**.

```yarn
// quest_id | Quest Name

// <scene_data>
// DO NOT EDIT THIS SECTION. It is auto-generated by the editor.
// TOPICS
// - topic_id
// CARDS
// - card_id          ← ready
//   - TODO card_id   ← needs to be created
//   - BROKEN card_id ← exists but assets missing
// TASKS
// - task_id
// ACTIVITIES
// - activity_settings_id
// ACTABLE
// WORDS: word1, word2
// </scene_data>
```

---

### 4.2 Node Structure

Every piece of dialogue or logic is a **node**. A node has:
- a `title` (unique within this script)
- metadata tags (`position`, `group`, `color`, `type`, `actor`)
- content between `---` and `===`

```yarn
title: my_node_name
position: 400,0
group: my_group
color: blue
actor: ADULT_F
---
This is the dialogue content. #line:0abcdef
===
```

**Node metadata reference:**

| Tag | Values | Purpose |
|---|---|---|
| `position` | `x,y` | Visual position in the Yarn graph editor |
| `group` | any string | Groups nodes visually in the editor |
| `color` | `red`, `blue`, `yellow`, `green`, `purple` | Node color in the graph |
| `type` | `panel`, `panel_endgame`, `quiz`, `task` | Special rendering/behaviour |
| `actor` | see table below | Voice actor for TTS/VO |
| `spawn_group` | any string | Marks node as spawned NPC; Unity uses the group to assign the right NPC |
| `tags` | space-separated | Node-level tags (e.g. `noRepeatLastLine`, `mood=HAPPY`) |
| `image` | asset ID | Background image shown for the node (panel nodes) |

**Actor types:**

| Actor | Voice profile |
|---|---|
| `NARRATOR` | Male, warm, enthusiastic — main storyteller |
| `ADULT_F` | Female adult, conversational, warm |
| `ADULT_M` | Male adult, natural, medium pitch |
| `SENIOR_F` | Female, gentle, storyteller cadence |
| `SENIOR_M` | Male, deeper, authoritative but kind |
| `KID_F` | Female child, energetic, high pitch |
| `KID_M` | Male child, playful, curious |
| `GUIDE_F` / `GUIDE_M` | Tour guide style |
| `SILENT` | No voice — for animals, mute NPCs |

---

### 4.3 Global Pre-declared Variables

These variables are available in **every** quest without a `<<declare>>` statement. Set them in `quest_start` where needed.

| Variable | Type | Default | Meaning |
|---|---|---|---|
| `$IS_DESKTOP` | bool | false | True when running on desktop (keyboard/mouse) |
| `$EASY_MODE` | bool | false | True when easy mode is active |
| `$TOTAL_COINS` | int | 0 | Total coins collected in this quest session |
| `$COLLECTED_ITEMS` | int | 0 | Generic item counter (use for simple collect tasks) |
| `$MAX_PROGRESS` | float | 0 | Max possible progress score |
| `$CURRENT_PROGRESS` | float | 0 | Current progress score |
| `$CURRENT_ITEM` | string | "" | ID of the currently active/selected item |

Use `<<declare $my_var = value>>` for **quest-specific** variables.

---

### 4.4 Required Nodes

Every quest must have these three nodes:

```yarn
title: quest_start
color: red
type: panel
actor: NARRATOR
---
// Set global variables
<<set $TOTAL_COINS = 0>>
<<set $COLLECTED_ITEMS = 0>>

// Declare quest-specific variables here
<<declare $step = 0>>
<<declare $met_guide = false>>

// Show 1-2 opening cards, set the initial area and target
<<card location_card>>
Welcome to LOCATION. #line:xxxxxxx
<<area area_init>>
<<target first_npc>>
===


title: quest_end
color: green
type: panel_endgame
actor: NARRATOR
---
Congratulations! #line:xxxxxxx
<<jump post_quest_activity>>
===


title: post_quest_activity
color: green
type: panel
tags: proposal
actor: NARRATOR
---
Now draw YOUR_OFFLINE_ACTIVITY. #line:xxxxxxx
<<quest_end>>
===
```

### 4.5 Antura Custom Commands

These commands are defined in `YarnAnturaManager.cs` and are available in every quest script. The **DEV quest** (`_TUTORIAL/_DEV`) contains a working example of nearly every command.

---

#### `<<action action_id>>`
Fires a named action defined in the scene's `ActionManager`. Use for things like opening doors, activating elevators, showing/hiding objects, playing sound effects.

```yarn
<<action activate_elevator>>
<<action open_chest>>
<<action play_sfx>>
```
Action IDs are configured in Unity by the developer. Ask the developer for the available action IDs for your scene.

---

#### `<<activity settings_id [return_node] [difficulty]>>`
Launches an activity mini-game and jumps to `return_node` when it finishes. The third parameter sets difficulty.

```yarn
<<activity memory_eiffel result_node>>
<<activity memory_eiffel result_node tutorial>>   // easiest
<<activity memory_eiffel result_node easy>>
<<activity memory_eiffel result_node normal>>
<<activity memory_eiffel result_node expert>>     // hardest
```
If `return_node` is omitted the script stops after the activity. Always provide one. After the jump, check the result:
```yarn
<<if GetActivityResult("memory_eiffel") > 0>>
    You solved it! #line:xxxxxxx
<<else>>
    Try again. #line:xxxxxxx
    <<jump retry_node>>
<<endif>>
```

---

#### `<<area area_id>>`
Sets the active navigation boundaries. Only one area is active at a time; calling `<<area>>` replaces the previous one.

```yarn
<<area area_init>>    // small starting zone
<<area area_bakery>>  // open bakery section
<<area area_all>>     // the full level
<<area off>>          // remove all boundaries (free roam)
```

---

#### `<<asset asset_id [zoom]>>`
Displays a raw image asset (not a Card) as a postcard/overlay. Used for tutorial UI images, background photos, and custom illustrations.

```yarn
<<asset tutorial_move>>
Use your finger to walk. #line:037d71d
<<asset tutorial_move zoom>>   // show larger
<<asset_hide>>                 // hide current asset
```

---

#### `<<camera_focus camera_id>>`
Smoothly moves the camera to a named focus point in the scene, pointing the player's attention to a place. Blocks dialogue until the camera movement completes.

```yarn
<<camera_focus camera_eiffell>>
Go to the Eiffel Tower. #line:baker_5
<<camera_reset>>   // return camera to follow the player
```

---

#### `<<camera_map_distance N>>`
Sets the camera's map zoom level (distance from player). Useful for wide establishing shots or tight close-ups.

```yarn
<<camera_map_distance 10>>   // zoomed in
<<camera_map_distance 30>>   // wide view
```

---

#### `<<card card_id [zoom] [silent] [collect]>>`
Shows a Knowledge Card popup. Parameters can be combined in any order.

```yarn
<<card eiffel_tower>>                    // show card normally
<<card eiffel_tower zoom>>               // open card in zoom/detail mode
<<card eiffel_tower silent>>             // show card without audio/title announcement
<<card eiffel_tower zoom silent>>        // zoom + silent
<<card food_baguette collect>>           // show AND add to player's card collection
<<card_hide>>                            // hide the current card
```

---

#### `<<collect [item_tag]>>`
Registers collection of the current interactable object (used in `item`-tagged nodes), or collects a named item by tag.

```yarn
// Standard usage — in an item node (tags: item):
title: item_honey
tags: item
---
<<card food_honey>>
Honey. #line:0817d3c
<<collect>>
===

// Collect a specific named item from anywhere:
<<collect key_gold>>
```

---

#### `<<cookies_add N>>`
Adds (or removes, if negative) cookies to the player's cookie jar.

```yarn
<<cookies_add 5>>
<<cookies_add -2>>
```

---

#### `<<inventory item_id [add|remove]>>`
Adds or removes an item (by card ID) from the player's persistent inventory.

```yarn
<<inventory key_gold>>          // add (default)
<<inventory key_gold add>>      // explicit add
<<inventory key_gold remove>>   // remove
```

---

#### `<<party_join npc_id>>`
Makes an NPC follow the player. `npc_id` must match the `Interactable.Id` in Unity.

```yarn
<<party_join npc_sawa>>
<<party_release npc_sawa>>   // stop following
<<party_release>>            // release everyone
<<party_formation "line">>   // change formation: "line", "circle", "V"
```

---

#### `<<quest_end>>`
Ends the quest, calculates stars, and returns the player to the map.

```yarn
<<quest_end>>      // auto-calculate stars from progress
<<quest_end 3>>    // force a specific star count (0–3)
```
Always call `<<quest_end>>` from inside the `post_quest_activity` node (or from `quest_end` if skipping the post-quest proposal).

---

#### `<<SetActive gameobject_name [true|false]>>`
Activates or deactivates a named Unity GameObject. Used to reveal chests, unlock portals, spawn objects.

```yarn
<<SetActive chest_flour true>>    // show and enable
<<SetActive chest_flour false>>   // hide and disable
<<SetActive chest_flour>>         // shorthand for true
```

---

#### `<<SetInteractable interactable_id [true|false]>>`
Enables or disables the interaction trigger on a specific Interactable (shows/hides the interact icon).

```yarn
<<SetInteractable npc_baker true>>
<<SetInteractable portal_exit false>>
```

---

#### `<<SetMapIcon interactable_id state>>`
Changes the visual state of an icon on the world map for the given interactable.

```yarn
<<SetMapIcon chest_flour "done">>      // mark as completed
<<SetMapIcon npc_baker "on">>          // active/available
<<SetMapIcon portal_exit "off">>       // hidden
<<SetMapIcon npc_baker "default">>     // reset to default state
```
Valid states: `"on"`, `"off"`, `"done"`, `"default"`.

---

#### `<<target interactable_id>>`
Places a visual waypoint arrow on an object or character to guide the player.

```yarn
<<target baker>>          // guide player to the baker NPC
<<target chest_flour>>    // guide player to a chest
<<target off>>            // remove the waypoint
```

---

#### `<<task_start task_id [callback_node]>>`
Starts a task and optionally registers a node to jump to when the task auto-completes (e.g., after collecting all required items).

```yarn
<<task_start go_eiffel npc_eiffel_arrived>>
```

Close a task manually with:
```yarn
<<task_end go_eiffel>>           // success
<<task_end go_eiffel fail>>      // failure
<<task_end>>                     // end current task (no ID needed)
```

---

#### `<<teleport actable_id>>`
Instantly teleports the player to a named teleport destination in the scene.

```yarn
<<teleport portal_roof>>
<<teleport spawn_start>>
```

---

#### `<<trigger actable_id>>`
Fires a named trigger on an Actable object — a lighter-weight alternative to `<<action>>` for simple trigger-response setups.

```yarn
<<trigger open_door>>
<<trigger play_cutscene>>
```

---

### 4.6 Antura Custom Functions

Functions return a value and are used inside `<<if>>` conditions or `<<set>>` expressions.

---

#### Activity functions

```yarn
// Returns 0 if failed, ≥1 if completed (higher = better result)
<<if GetActivityResult("memory_eiffel") > 0>>
```

---

#### Card functions

```yarn
// Returns true if the player has unlocked (collected) this card
<<if card_unlocked("eiffel_tower")>>
    You already have this card! #line:xxxxxxx
<<endif>>
```

---

#### Cookie / Coin functions

```yarn
// Returns the current cookie count
<<if GetCookies() >= 10>>

// Returns total coins collected in this quest session
<<if GetTotalCoins() >= 5>>
```

---

#### Inventory functions

```yarn
// Does the player have this item?
<<if has_item("key_gold")>>

// How many of this item does the player have?
<<if item_count("food_baguette") >= 3>>

// Does the player have at least N of this item?
<<if has_item_at_least("food_baguette", 3)>>

// Can the player collect (more of) this item?
<<if can_collect("food_baguette")>>
```

---

#### Task functions

```yarn
// Returns the ID string of the currently active task
<<if GetCurrentTask() == "go_eiffel">>

// How many items have been collected for this task?
<<if GetCollectedItem("collect_coins") >= 5>>

// Has this task been completed?
<<if HasCompletedTask("collect_coins")>>
```

---

### 4.7 Line Tags

Every spoken dialogue line must end with a `#line:` tag — a unique hash used for localization and voice recording. **Never invent a hash; let the Yarn editor generate it automatically.**

```yarn
This is the Eiffel Tower. #line:0da46e8
```

Additional per-line tags:

| Tag | Syntax | Meaning |
|---|---|---|
| `#line:HASH` | `#line:0da46e8` | **Required.** Unique localization + VO identifier |
| `#native` | `#native` | Line is in the local language; do not translate |
| `#task:id` | `#task:go_eiffel` | Sets this line as the description shown in the task panel |
| `#card:id` | `#card:eiffel_tower` | Inline card shown with a choice option |
| `#highlight` | `#highlight` | Visually highlights this choice (use for "skip" / "no" / main action) |
| `#shadow:HASH` | `#shadow:0da46e8` | Reuses existing VO audio; text is different but not re-recorded |
| `#do_not_translate` | `#do_not_translate` | This line stays in the original language across all localizations |

---

### 4.8 Dialogue Patterns

#### Simple linear NPC

```yarn
title: talk_baker
actor: SENIOR_M
---
Hello! I am a baker. #line:baker_0
Can you help me find the ingredients? #line:baker_1
<<area area_all>>
<<target npc_eiffel>>
===
```

#### Branching choices

```yarn
title: talk_guide
actor: GUIDE_F
---
What do you want to know? #line:0070084
-> What is the Eiffel Tower? #line:0d91dc0
    It is a tall iron tower, 300 meters high. #line:0f17af0
-> Where are we? #line:09dd1da
    We are in Paris. #line:02b627d
-> Nothing. Bye. #line:0fe0732 #highlight
===
```

#### Random lines (`=>`)
Each visit shows one random line from the list. Used for spawned ambient NPCs.

```yarn
title: spawned_local
actor: ADULT_F
spawn_group: paris_locals
---
=> The Mermaid is a symbol of Warsaw. #line:xxxxxxx
=> The Wisła River flows through Warsaw. #line:xxxxxxx
=> I use the tram to get around. #line:xxxxxxx
===
```

#### Conditional gate

```yarn
title: talk_notre_dame
actor: SENIOR_M
---
<<if $BAGUETTE_STEP < 1>>
    Come back later. #line:come_back_later
<<elseif $BAGUETTE_STEP == 1>>
    This is Notre-Dame Cathedral. #line:06f3fa2
    <<action activate_teleporter>>
<<else>>
    We already visited here. #line:already_solved
<<endif>>
===
```

#### Node with `noRepeatLastLine` tag
By default, when a node ends with a choices block, the last spoken line is repeated above the choices. Add this tag to suppress that repetition.

```yarn
title: ask_options
tags: noRepeatLastLine
---
-> Option A #line:xxxxxxx
-> Option B #line:xxxxxxx
===
```

---

### 4.9 Organizing the Graph

- Group nodes by story beat: `group: mermaid`, `group: chopin`, `group: finale`
- Main flow runs **left to right** (increasing X)
- Spawned/random NPCs go to the side (high X or negative Y)
- Color convention:

| Color | Use |
|---|---|
| `red` | `quest_start` and any critical-path init nodes |
| `blue` | Main NPC conversations |
| `yellow` | Items, chests, collectibles |
| `green` | Tasks, completions, `quest_end` |
| `purple` | Activities and spawned/ambient NPCs |

---

## Phase 5: Unity Scene Setup

Once the Yarn script is written and reviewed, the developer sets up the 3D scene in Unity.

### 5.1 Create the Quest Data Asset

In Unity:
1. Right-click in `Assets/_discover/_data/` → Create → Discover → QuestData
2. Name it `XX_00 QuestName - Quest Data`
3. Fill in: Quest ID, Display Name, Scene reference, Yarn Project reference
4. Add to the `QuestsList` asset in `_discover/_data/`

### 5.2 Set Up the Yarn Project

1. Create a new `.yarnproject` file in the quest folder
2. Add the quest's `.yarn` file and `_common script.yarn` as sources
3. Enable auto-importing of line metadata

### 5.3 Scene Structure

The scene has two root GameObjects:
- `World` — static environment assets shared across quests
- `Quest` — all quest-specific GameObjects (NPCs, chests, triggers, areas)

Under `Quest`, you will typically have:
```
Quest/
  Interactables/      ← NPCs (each has an Interactable component with a node title)
  Items/              ← collectable objects
  Actions/            ← triggered GameObjects (doors, elevators, chests)
  Areas/              ← NavMesh area colliders
  Cameras/            ← camera focus points
  Targets/            ← waypoint targets
```

### 5.4 Wire Interactables to Yarn Nodes

Every interactive object (NPC, item, trigger zone) needs:
1. An `Interactable` component
2. The **Yarn Node Title** field filled with the matching node title from your script

When the player interacts with the object, the Yarn runner jumps to that node.

### 5.5 Configure Action Manager

For every `<<action action_id>>` in your script, add a matching entry in the scene's `ActionManager` component, pointing to the Unity method or GameObject that should respond.

### 5.6 Configure Activity Settings Assets

For every `<<activity settings_id ...>>` call, there must be a matching Activity Settings asset in Unity:
1. Right-click → Create → Discover → Activity → [type]Settings
2. Name it exactly as the ID used in the script
3. Assign cards, images, difficulty parameters

### 5.7 Set Up Areas

For `<<area area_id>>` commands, create NavMesh Area collider objects named exactly as the area IDs in the script.

---

## Phase 6: Writing the Design Notes 

Every quest folder must have a design notes file (`CODE _README design notes.md`). Write this **before** you write the Yarn script. It is the reference document for the whole team.

Use this template:

```markdown
## Game Design Notes

**Mission Statement**
[One sentence: what does the player do and why?]

### Characters
- **[Name]**: [role and 1-sentence description]
- ...

### Knowledge Content
- **[Topic]**: [fact 1]
- **[Topic]**: [fact 2]
- ...

### Gameplay Flow

#### Step 0: [Name]
- **Location:** ...
- **Action:** ...

#### Step 1: [Name]
- **Requirement:** [what unlocks this step]
- **Action:** ...
- **Activity:** ...
- **Reward:** ...

[... repeat for each step ...]

#### Final: [Name]
- **Action:** Final activity
- **Reward:** Quest complete

### Final Assessment Questions
1. [Question]? → **Correct answer**, wrong, wrong
2. [Question]? → **Correct answer**, wrong, wrong
3. [Question]? → **Correct answer**, wrong, wrong
```

---

## Phase 7: Dialogue Writing Guidelines

All spoken text in the Yarn script must follow these rules so it works for young players (age 6–10) and survives translation and VO recording. These rules are tighter than typical game writing: they exist because the child's native language may not be the language on screen, and because children of this age have limited working memory.

- **Present simple tense** ("This is the Eiffel Tower", not "This was built in...")
- **5–10 words per sentence** (absolute max 12, only for color/item lists)
- **One idea per sentence** — split anything with "and" or "but" joining two clauses
- **Every spoken line ends with `.`, `?`, or `!`**
- **Max one `!` per short exchange** — overusing exclamation marks makes everything feel the same
- **High-frequency vocabulary**: help, find, friend, flag, big, small, red, blue
- **One cultural greeting per country** (`Bonjour!`, `Hallo!`, etc.) — then use `Hello` for the rest of the quest
- **Keep cultural nouns** (Eiffel Tower, Copernicus) but simplify every word around them
- **No idioms or abstract metaphors** ("claim your victory" → "get your prize")
- **Error recovery lines must be warm** ("Almost! Try the red one." not "Wrong, try again.")
- **Repeat the key word** in at least 3 lines across the quest (introduction → activity → wrap)
- **Max 3–4 dialogue lines per node before a player action** — never let a node become a lecture

For the full rule set, examples, and the AI rewriting prompt, see [Quest Script Writing Guidelines](quest-scripts-guidelines.md).

---

## Phase 8: Rewards and Telemetry

### Rewards

| Reward | When | Notes |
|---|---|---|
| **KP (Knowledge Points)** | Every interaction — correct gets full KP, wrong gets a small amount | Diminishes as the child masters the content |
| **Stars (0–3)** | Quest completion, based on core objectives | Time is secondary to completion quality |
| **Gems** | Only when `bestStars` improves for this quest | Quest cap = 3 gems total. Most cards grant 0–1 gem once; centerpiece cards grant 2–3. |
| **Cards** | Shown at end of each scene and in final wrap | Card list visible in quest detail panel before starting |

Stars are awarded based on **what the child did**, not how fast. Penalising speed discourages careful reading and re-listening.

### Telemetry events to log

Log these events for design reviews and learning analytics:

| Event | When to fire |
|---|---|
| `quest_run_end` | Quest finishes (include stars, duration, skip count) |
| `card_interaction` | Any card is shown or tapped (include card ID, tap-to-native count) |
| `task_claimed` | A task is completed |
| `gem_claimed` | A gem is awarded |
| `milestone_crossed` | A major progression gate is passed |
| `activity_attempt` | Each attempt at an activity (include pass/fail, attempt number) |
| `audio_replay` | Child replays a line (high replay count = confusing line or key vocabulary) |

---

## Quick Reference: Quest Checklist

Use this before submitting a quest for review.

### Design checklist
- [ ] Learning goal is specific and testable
- [ ] 5–10 knowledge cards defined (split quest if more than 12)
- [ ] Each key word appears at least 3 times across the quest (intro → activity → wrap)
- [ ] At least one social phrase included in the vocabulary
- [ ] 1–3 topics identified
- [ ] Mission statement written (one sentence)
- [ ] Emotional arc present: problem → adventure → climax → resolution → pride
- [ ] 3D environment pre-communicates the problem visually (environmental storytelling)
- [ ] Progression maps to the Scene A/B/C/D skeleton
- [ ] Total play time estimated ≤ 12–13 minutes
- [ ] At most 2 mechanics used across the whole quest
- [ ] Every activity has a graceful skip path (after 2 failed attempts)
- [ ] Final assessment activity covers the whole quest
- [ ] Post-quest offline activity defined (drawing, discussion, making something)
- [ ] Cultural facts reviewed by a native speaker or local expert
- [ ] Design notes file (`_README design notes.md`) complete

### Yarn script checklist
- [ ] `quest_start` node with variable declarations
- [ ] `quest_end` and `post_quest_activity` nodes
- [ ] Every spoken line has `#line:` tag
- [ ] Every spoken line ends with `.`, `?`, or `!`
- [ ] No line exceeds 15 words
- [ ] Spawned NPC nodes have `spawn_group:` tag
- [ ] Conditional gates block out-of-order access
- [ ] Activity result is checked and handled (retry or continue)
- [ ] `<<target off>>` called after `<<target x>>` when done
- [ ] `<<camera_reset>>` called after `<<camera_focus x>>`
- [ ] All card IDs exist (no `TODO` left unresolved)
- [ ] `_common script.yarn` is included in the Yarn Project

### Unity checklist
- [ ] QuestData asset created and added to QuestsList
- [ ] YarnProject created, both `.yarn` files linked
- [ ] All Interactables wired to correct Yarn node titles
- [ ] All `<<action>>` IDs wired in ActionManager
- [ ] All `<<activity>>` settings assets created
- [ ] All `<<area>>` colliders named and placed
- [ ] All `<<target>>` waypoint GameObjects present
- [ ] All `<<camera_focus>>` points present in scene
- [ ] `<<SetActive>>` objects start in the correct state

---

## Additional Design Suggestions

These are ideas not yet formally part of any quest, offered as starting points for future design refinement.

---

### Transitions as micro-teaching moments

The brief transition between areas (boat ride, tram journey, portal jump) is usually dead time. Consider using it for a quick one-line recap: *"You found the flour. Three more ingredients to go!"* or a single word card displayed during the 5-second travel animation. Keep it under 10 seconds and skippable — but it is a free learning slot.

---

### The Antura personality brief

Define Antura's archetype for your quest before writing: is he **clumsy** (knocked things over by accident), **curious** (touched something he shouldn't have), or **playful** (started a game that got out of hand)? This one decision makes all his related dialogue feel coherent and gives the NPC reactions a consistent emotional tone.

### Playtesting checklist for age 6–10

Before marking a quest ready for review, test it specifically with this age group and watch for:
- Does the child know where to go without reading the task panel?
- Do they tap replay on any line more than 3 times? (If yes: rewrite that line.)
- Do they get stuck at any activity for more than 90 seconds without attempting the hint?
- Do they skip the wrap scene? (If yes: the wrap is too slow or the reward is unclear.)
- Can they name one thing they learned immediately after finishing?

### Node pacing rule: the 3-line trigger

No dialogue node should have more than **3–4 spoken lines before the player must do something**. If a node has 5+ lines, split it into two nodes with a brief interactive moment (a card tap, a choice, a movement prompt) in between. Passive listening for more than ~20 seconds causes disengagement in this age group.

### Replayability: spawned NPCs as ambient vocabulary reinforcement

The `spawn_group` NPCs (random locals) are a largely untapped vocabulary resource. Each spawned NPC's `=>` lines should always include at least one line that **reuses a quest vocabulary word in a new sentence context**. This gives replaying children extra encounters with the target words without changing the main quest flow.

---

### Cultural sensitivity review step

Before a quest ships, at minimum one review by a **native speaker who is also a primary school educator** (or parent) from the target country. Things to check:
- Are all factual claims accurate for a child's level?
- Are any cultural representations stereotyped or oversimplified in a harmful way?
- Is the single cultural greeting spelled and used correctly?
- Are any NPC names or references potentially confusing or offensive in the local culture?

Document the review and who did it in the `_README design notes.md` file.

---

### The post-quest offline activity: make it specific

The current quests propose things like "draw your favourite castle" or "draw the 8 planets." These are good. Make them even better by:
- Tying the activity directly to a vocabulary word: "Draw a **baguette** and label the 4 ingredients."
- Suggesting a family conversation: "Ask someone at home if they have eaten **bouillabaisse**."
- Proposing a classroom discussion: "Which of the two castles would you choose to live in? Why?"

The offline activity is the quest's longest-lasting educational echo — do not make it generic.

---

### Consider a "quest preview card"

Before the child enters a quest, show a single card: location image + quest name + 3 vocabulary words they will learn. This primes their attention and gives parents/teachers a quick briefing. It only needs 10–15 seconds and can be skipped. Studies on language learning consistently show that previewing target vocabulary improves retention during the lesson.

---

## Quests on the Website: Public Reference and Feedback

All Discover quests are published on the Antura website in a human-readable, browsable form — directly from the game data. This is one of the most important features of the project: **you can review, audit, and share a quest without launching Unity**.

**Live content hub:** [https://antura.org/en/content/](https://antura.org/en/content/)

---

### What is published

The website automatically generates a page for every quest, showing:
- The quest title, location, and topic
- Every Knowledge Card used in the quest (with image, text, and audio)
- The full dialogue script (in the target language and native language side by side)
- The quest's vocabulary words
- The complete activity list
- The credits

Example quest pages:
- [Tutorial quest](../../content/quests/quest/tutorial.md)
- [Paris quest (FR_01)](../../content/quests/quest/fr_01.md)
- [Discover Warszawa (PL_01)](../../content/quests/quest/pl_01.md)


### Why this matters for designers

**Use the website to:**

**1. Study existing quests before designing your own.**
The published pages are the fastest way to understand how content is structured without needing a Unity build. Read the dialogue flow, the card sequence, and the vocabulary list of existing quests before writing your own.

**2. Review your own quest before it ships.**
Once your quest data is imported and built, its page appears on the website. Read through the entire dialogue as a teacher or parent would — not as a developer. This often reveals lines that are too long, knowledge gaps, or progression that is unclear on paper but confusing in practice.

**3. Share quests with non-technical collaborators.**
Educators, curriculum reviewers, translators, and cultural consultants do not need Unity. Send them the quest URL and they can review every line of dialogue, every card, every vocabulary word — and give feedback via GitHub issues or directly to the team.

**4. Pedagogical and linguistic audit.**
The page format mirrors the "transparent, reviewable content" principle of the project. Stakeholders (school partners, Erasmus+ institutions, NGOs) can audit what children will actually see and hear before any build is made.

**5. Collect community feedback.**
The public URL can be linked from social media, newsletters, or classroom materials. Feedback from teachers and parents who have played a quest is one of the most valuable inputs for the next revision.
