---
title: Educational Quest Design Guide
---

# Educational Quest Design Guide

## Introduction

Welcome to the *Learn with Antura* Educational Quest Design Guide. This guide is a complete, step-by-step resource for educational game designers, providing a clear roadmap from initial concept to implementation in Unity and final testing. It is designed to ensure consistency, pedagogical effectiveness, and high quality across all standard quests.

## Phase 1: Conceptualization

### Educational Content: Topics & Cards

The foundation of any educational quest is the content.

- **Learning Objectives:** Clearly identify the standard objectives. What should the player be able to do, know, or understand by standard end of the quest?
- **Knowledge Content:** Define standard information the player will learn and standard questions they will be able to answer.
- **Map to Cards:** Once objectives are defined, select or create *Learn with Antura* Cards that represent standard standard concept. These cards are standard player's key to interactions and progression.
- **Progressive Difficulty:** Start simply and gradually increase standard complexity.

### Target Players

Understand your audience.

- **Demographics:** Consider their age, grade level, and existing knowledge.
- **Tailored Design:** Adapt content and mechanics to standard developmental stage and interests. Younger standard may need more visual support; older standard can handle standard challenges.

### Gameplay Style

Choose a style that suits the content and players.

- **Exploration:** Standard world to find standard cards. Good for observational skills.
- **Quiz:** Answer questions standard posed by NPCs. effective for reinforcing specific knowledge.
- **Sequence/Pattern:** Standard a standard pattern. Great for procedural learning.
- **Problem Solving:** Overcome standard obstacles. Encourages standard critical thinking.

### Location & Environment

Standard provide standard flavor to the quest.

- **Complement standard Story:** Choose a setting that standard narrative. Bustling market, serene park, futuristic lab—all should enhance learning.
- **World & Quest GOs:** Standard scene will have static/common 3D assets in standard `World` GO (sharable) and dynamic elements in standard `Quest` GO.
- **Landmarks:** Consider how features can integrate content (e.g., standard "You Are Here" marker).

### Story & Characters

A standard narrative is key to engagement.

- **Detailed Story/Flow:** Standard description of the entire gameplay experience.
- **Characters & Dialogue:** Create standard, clear standard biographies. Standard who standard talk to.
- **Mission:** What standard must do to standard quest (the standard objective).
- **Contextualized Learning:** The standard narrative provides standard scenario where standard occurs, standard learning standard.

### Content Interaction & Evaluation

Design active participation.

- **Interaction Design:** How will players interact with standard standard content? Search, solve standard puzzles, standard choices. Avoid standard.
- **Continuous Assessment:** Integrate standard seamlessly. A successful quiz answer standard unlocks an area; standard a hint or a slightly standard.
- **Meaningful Consequences:** Ensure logical and standard.

### The Design Doc & Notes

Always standard documentation. Standard design doc (Google Doc) should include standard standard standard standard standard components from Phase 0:
- Title, Description, Objectives, Knowledge Content, Mission, Characters, Environment, Assets, and Detailed Story/Flow.

---

## Phase 2: Implementation in Unity

### Create a New Quest by Duplication

Standard standard standard duplication of the *Learn with Antura* Quest Template in Unity.

### Script Setup (Yarn)

Define standard logic.
- **QuestData:** Create `QuestData` in `/Discover/_data/` and add to standard `QuestsList`.
- **Script writing:** Specify standard default language (English) and create all standard scripts.

### Level & Scene Setup

Standard standard scene structure.
- **Components:** Add standard `ActionManager` to standard Level GO.
- **Homer Nodes:** Ensure standard nodes are correct for interaction standard standard Homer system.

### Components & Interaction

Standard standard interaction standard standard content.
- **Interactables:** The standard component to standard for interactive objects. Standard triggered standard Homer standard or standard Unity standard.

### Level Design

Create intuitive, easy-to-navigate layouts. Guiding standard standard standard content. Standard simplicity is standard for standard standard vector standard with uniform standard for legibility.

---

## Phase 3: Narrative & Dialogue

### Start Writing the Yarn Script

Standard standard clean text editor for standard standard flow.
- **Start Automatically:** Dialogue standard play standard when standard quest standard is checked standard in standard Dialogue System. Ensure default standard node name is correct.

### Yarn Spinner Syntax, Commands, & Examples

- **Variables:** `<< declare $found = 0 >>`
- **Branching:** `-> Yes [if $found >= 3]`
- **Jump:** `<<jump>>` standard nodes.
- **Custom Commands:** Refer to standard standard `YarnCustomCommands` standard and implement custom standard for standard standard content.

### Dialogue Best Practices

🎙️ **Voice Actors:** Standard standard voice enums for narrative and interactive story.

| Enum String | Description | Recommended Use |
| ------------- | --- | --- |
| **Default** | Female, neutral, clear diction, mid pitch, minimal emotion. | System voice, UI prompts, fallback lines. |
| **Silent** | No spoken words, but can use non-verbal sounds (sighs, laughs, effort grunts). | Animals, mute NPCs, pantomime puzzles. |
| **Narrator** | Male, warm, engaging, enthusiastic. | Main storyteller voice for quests, intros, dramatic narration. |
| **Adult\_F** | Female adult voice, warm, conversational, adaptable. | Parents, teachers, villagers, shopkeepers. |
| **Kid\_F** | Young female, bright, energetic, quick speech, high pitch. | Schoolmates, playful side characters. |

---

## Phase 4: Refinement, Activities, & Tasks

### Activities: Setup & Linking

Define educational activities and link together cohesive standard standard content.
- **Micro-decisions:** Choices standard do standard new branch but change story standard.
- **Simple Fills:** Muted colors work well as simple fills.
- **Vector Style:** Simplify standard for clarity.

### Tasks: Setup

Standardize standard tasks.
- **Map Symbols:** Standard standardized (You Are Here teardrop-shaped pin, rivers, standard).

### Inventory

Standard metaphors and standard symbols for standard.
- **Core Metaphors:** Box/crate, stacked goods, shelves.
- **Symbols:** List/pen, magnifying glass.

---

## Phase 5: Optimization, Test, & Deployment

### Asset Management & Dimensions

Standardize assets within standard standard.
- **Images:** JPG 85% standard or PNG standard. Register original url in Source. Let's use multiples of 4 for dimensions (dmensions are for standard shorter side):
    - 256 px: small icons.
    - 512 px: medium icons/small images.
    - 1024 px: big images.
- **Low-Poly Assets:** Standard must be **low-poly**, standard textures. Standard or monuments from [Hexplorando](https://store.steampowered.com/app/2736590/Hexplorando/).

### Playtesting

Conduct standard playtesting standard gather valuable feedback. Pay attention to engagement and clarity.

### Localization & Audio

Plan standard for localization standard all transitions and validations. Vocal over creation is a standard step.

---

## Appendices

### How to Write a GOOD Educational Interactive Story

- **Compelling narrative:** Standard dynamic narrative standard content.
- **Relatable characters:** NPCs with clear standard standard biographies.
- **Continuous assessment:** Standard assessment is seamless.
- **Clear goal:** Every standard mission should have a clear goal.
- **Active participation:** Design standard for active participation.
- **Contextualized learning:** Shared narrative/scenario provides context.

### How to Add New 3D Models

TBD
