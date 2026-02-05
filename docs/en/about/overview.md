# Learn with Antura: an open educational game and framework

![Learn with Antura](../../assets/img/antura_gametitle.jpg)

_Learn with Antura_ is an open source educational game ecosystem designed to support learning through exploration, interaction, and narrative.

It started in 2016 as a humanitarian initiative for children in fragile contexts and has grown into a modular platform that can be adapted with partners to new **countries, languages, curricula, and cultural contexts**.

This website documents both the playable game and the underlying **Discover Framework**.

## At a glance

- **Audience**: primarily children aged 6–12 (usable in class or at home)
- **Format**: short sessions (typical quests 5–15 minutes) + teacher-led debrief activities
- **Platforms**: built with Unity; cross-platform by design
- **Offline-first**: after installation, the game does not require internet to play
- **No ads / no in-app purchases**
- **Modular**: content can evolve without rewriting the core systems

For educators and classroom usage, start from the [Manual](../manual/index.md).

## What makes Antura different

### Learning through exploration

Antura embeds learning inside a playful 3D world. Children progress by exploring places, talking to characters, solving tasks, and collecting knowledge.

### Transparent, reviewable content

Everything that exists in the game also exists in a browsable form on the public website: quests, knowledge cards, dialogues, and translations.

- Browse open content: [https://antura.org/en/content/](https://antura.org/en/content/)

This supports partner workflows such as pedagogical review, linguistic validation, and cultural adaptation without requiring Unity builds.

### Proven impact and a long-term open approach

Antura has been evaluated in real-world contexts and improved over time based on external feedback.

- Independent evaluation and evidence: [Impact evaluation](./impact.md)
- Project history and partners: [History](./history.md)

## Two learning modules

Antura currently supports two complementary modules:

### Learn Languages

A collection of mini-games focused on letters, words, and vocabulary.

- Details: [Learn Languages module](../manual/learnlanguage_module.md)

### Discover Cultures

Short quest-based adventures where children learn culture (and language) through stories, tasks, and collected knowledge cards.

- Start here: [Discover introduction](../manual/discover_introduction.md)
- Detailed feature overview: [Discover features](../manual/discover_module.md)
- Player controls and quest flow: [How to play Discover quests](../manual/discover_how_to_play.md)

## Key features

If you are new to the project, these are the main gameplay and content systems you will encounter:

- **Worlds & locations**: children explore immersive 3D environments (from abstract scenes to real places).
- **Quests**: structured missions that combine narrative, exploration, and tasks.
- **Activities**: short reusable mini-games (quizzes, puzzles, memory, ordering, etc.) embedded in quests or playable from the Book.
- **Knowledge Cards & Topics**: “atoms of knowledge” collected in quests and connected into topics/graphs for review and classroom discussion.
- **Characters & dialogue**: NPC-driven interactions with multilingual subtitles and voice.
- **Teacher/classroom support**: classroom setup, short-session format, and post-session debrief activities.

For the detailed breakdown (with screenshots), see [Discover features](../manual/discover_module.md) and the educator workflow in the [Classroom guide](../manual/classroom_guide.md).

## Browse real game content

All educational content is published in a browsable form and kept in sync with the latest game data builds.

- Content hub: [Open Content](../content/index.md)
- Discover Cultures: [Quests](../content/quests/index.md), [Topics](../content/topics/index.md), [Cards](../content/cards/index.md), [Words](../content/words/index.md), [Activities](../content/activities/index.md), [Locations](../content/locations/index.md)
- Learn Languages: [Language Curriculum](../content/language-curriculum/index.md), [Language Minigames](../content/language-minigames/index.md)
- Examples: [Tutorial quest](../content/quests/quest/tutorial.md), [Paris! quest](../content/quests/quest/fr_01.md), [Discover Warszawa quest](../content/quests/quest/pl_01.md), [tutorial dialogue script](../content/quests/quest/tutorial-script.md)

## Developer-, designer-, and educator-friendly workflow

A key characteristic of the Antura ecosystem is that game content is **transparent and reviewable outside the game**. This supports a workflow that is unusual in educational games: educators and partners can inspect what will ship (quests, dialogue, topics/cards, activities, translations) without requiring Unity builds.

In practice this enables:

- **Content review & feedback**: review dialogue scripts and translations, validate terminology/tone, and iterate on educational materials earlier in the cycle.
- **Data-driven content production**: the Discover Framework keeps core systems separate from narrative/educational content, so new quests and learning paths can be designed and iterated primarily as content/data.
- **Teacher and classroom orientation**: teachers can preview materials, and institutions can audit alignment with curricula before classroom use.

For partner-facing creation workflows, see the [Quest design guide](../dev/quest-design/index.md).

## The Discover Framework

The Discover Framework is the reusable backbone that powers the game. It is designed so partners can build new learning experiences without rebuilding an entire game.

Core building blocks:

- **Worlds & locations**: exploratory 3D environments
- **Quests**: narrative missions with tasks and progression
- **Activities**: reusable mini-games embedded in quests or played from the Book
- **Knowledge Cards & Topics**: atomic pieces of content connected as a knowledge graph
- **Characters & dialogue**: guided interactions, multilingual text/audio

For detailed design and creation workflows, see [Quest design guide](../dev/quest-design/index.md).

## Why this matters for Erasmus+ partners

Antura is designed for multi-stakeholder collaboration. A typical Erasmus+ partnership can combine:

- **Educators & schools**: co-design topics/quests aligned to curricula, run classroom pilots, produce teacher materials and training
- **Researchers**: evaluate learning outcomes and classroom usability, iterate based on evidence
- **Developers & studios**: extend the framework, create new activities, improve tooling and pipelines
- **Institutions & funders**: support a sustainable, reusable, open core that can be replicated across countries

Typical work packages (example structure):

- **WP1 Management & coordination**: consortium governance, planning, reporting
- **WP2 Pedagogical co-design**: curriculum alignment, content guidelines, teacher toolkit draft
- **WP3 Content production**: topics/cards/dialogues + localization and cultural validation
- **WP4 Technical development**: framework improvements, new quests/activities, tooling and pipelines
- **WP5 Pilots & evaluation**: classroom pilots, data collection, iteration cycles
- **WP6 Dissemination & sustainability**: communication, replication guidelines, long-term maintenance plan

To learn about the current Erasmus+ project context, see [Erasmus+](./erasmus.md).

## Open source and licensing

The project is open source to enable transparency, sustainability, and institutional reuse.

- Overview and licenses: [Open Source](./open-source.md) and [License](./license.md)
- Source code: [https://github.com/vgwb/Antura/](https://github.com/vgwb/Antura/)

## Get started

- Install: [Install](../manual/install.md)
- Classroom setup (profiles, language settings): [Setup](../manual/setup.md)
- Practical classroom workflow: [Classroom guide](../manual/classroom_guide.md)

If you want to partner with us, see [Contact](./contact.md).
