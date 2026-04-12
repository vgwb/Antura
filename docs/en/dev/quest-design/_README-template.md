# XX_NN QuestName — Design Notes

> **How to use this file:**
> Copy it into your quest folder as `XX_NN _README design notes.md`.
> Fill it in **before** writing the Yarn script or opening Unity.
> Keep it up to date as the quest evolves — it is the shared reference for the whole team.

---

## Overview

| Field | Value |
|-------|-------|
| **Quest ID** | `xx_nn_questname` |
| **Display ID** | `XX_NN` |
| **Country** | e.g. Italy |
| **Location** | e.g. Rome — Colosseum area |
| **Status** | Development / Production / Validated |
| **Target age** | 6–10 |
| **Difficulty** | Tutorial / Easy / Normal / Difficult |
| **Estimated duration** | ~X minutes |
| **Gameplay style(s)** | e.g. Collect & Deliver, Landmark Tour |

---

## Learning Goal

*What will a child know or be able to do after this quest that they didn't before?*

> Example: "Name the 4 ingredients used to bake a baguette."

Keep it **specific and testable**. If you can't test it, it's too vague.

---

## Mission Statement

*One sentence: what does the player do, and why?*

> Example: "Help a baker find the four stolen ingredients scattered across Paris's most famous landmarks."

---

## Knowledge Content

### Topics (1–3)

| Topic ID | Name | Notes |
|----------|------|-------|
| `topic_id` | Topic Name | New / already exists in DB |

### Cards (5–12 target; split quest if > 12)

| Card ID | Title | Type | Status |
|---------|-------|------|--------|
| `card_id` | Card Name | Place / Person / Object / Event / Concept | ready / TODO / BROKEN |

### Vocabulary (5–10 words)

| Word | Language | Notes |
|------|----------|-------|
| word | EN | Include at least 1 social phrase |

---

## Characters

| Name | Role | Actor type | Description |
|------|------|-----------|-------------|
| NPC Name | Quest-giver / Landmark NPC / Spawned local | `SENIOR_M` / `ADULT_F` / etc. | One-sentence personality + function |

*The player character is always the Cat. Antura (the dog) can be the cause of the problem.*

---

## Emotional Arc

- **Problem:** *What is wrong at the start? (Antura did something / someone needs help / something is lost)*
- **Adventure:** *How does the player explore and build competence?*
- **Climax:** *What is the final challenge or effort?*
- **Resolution:** *How is the problem fixed?*
- **Pride:** *How does the child get credit? (card list, stars, offline activity)*

---

## Gameplay Flow

Use the A/B/C/D scene skeleton. Each step should take the player to a new place or NPC, deliver 1–3 cards, and include one small task.

### Scene A — Hook (1–2 min)

- **Location:** ...
- **NPC:** ...
- **Cards shown:** `card_id`, `card_id`
- **Task/action:** Quick "try it" interaction so the child acts immediately
- **Yarn node:** `quest_start`

### Scene B — Practice (3–5 min)

- **Location:** ...
- **NPC:** ...
- **Cards shown:** `card_id`
- **Activity:** `<<activity activity_id result_node>>`
- **Yarn node:** `step_b_start`

### Scene C — Use It (3–5 min)

- **Location:** ...
- **NPC:** ...
- **Cards shown:** `card_id`
- **Activity / dialogue choice:** ...
- **Yarn node:** `step_c_start`

### Scene D — Wrap (1–2 min)

- **Recap:** which words/cards are reviewed
- **Cultural tip:** one bonus card
- **Final activity:** `<<activity final_assessment result_node>>`
- **Offline activity proposal:** *e.g. "Draw your favourite building from this quest."*
- **Yarn node:** `quest_end`

---

## Final Assessment Questions

List 3 questions for the final activity. Mark the correct answer in **bold**.

1. Question? → **Correct**, Wrong, Wrong
2. Question? → **Correct**, Wrong, Wrong
3. Question? → **Correct**, Wrong, Wrong

---

## Scene & Asset Notes

*Notes for the developer: what 3D environment is needed, any existing scenes to reuse, special prefabs or props required, camera focus points, etc.*

- Environment: ...
- Reuse existing scene: `discover_xx_nn_existing.unity` / new scene needed
- Special props: ...
- Camera focuses: ...

---

## Open Questions / TODOs

- [ ] ...
- [ ] ...
