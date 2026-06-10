# DEV_110 — Undertale-style Fight

A **standalone, Yarn-driven** turn-based battle for the DEV_110 quest prototype.
It does **not** use the `ActivityBase` / `ActivityManager` framework — it only needs
`YarnAnturaManager.I` and `DiscoverGameManager.I`, which already live in the Discover scene.

The C# owns the **UI, ACT/ITEM/MERCY menu, player HP and the bullet-hell dodge phase**.
All *content* (which acts exist, what they say, which one is "correct", when you can spare)
is authored in the **Yarn script** and pushed to the fight via the `fight_*` commands below.

---

## Scripts

| File | Role |
|---|---|
| `UndertaleFight.cs` | Main controller / state machine + the Yarn `fight_*` command bridge. |
| `FightSoul.cs` | The movable "heart" (arrows / WASD), clamped to the dodge box, with i-frames and a hit event. |
| `FightBullet.cs` | One projectile: moves (optionally zig-zag / wave), hit-tests the soul, despawns off-box or on lifetime. |
| `FightTypes.cs` | Serializable config: `ActOption`, `DodgeWave`, `SpawnShape`. |
| `ProjectilePattern.cs` | `ProjectilePattern` asset + `Emitter` + shared `ProjectilePatternMath` (used by both runtime and the editor preview). |
| `ProjectilePatternRunner.cs` | Runtime player: spawns a pattern's bullets (no prefab needed). |
| `Editor/ProjectilePatternDesigner.cs` | The visual pattern tool (animated preview). |
| `Editor/ProjectilePatternInspector.cs` | Adds an "Open in Pattern Designer" button to the asset. |
| `DebugFightLauncher.cs` | Play-mode inspector button to start a fight without the full quest. |

Namespace: `Antura.Discover.DEV110` (editor tool in `Antura.Discover.DEV110.EditorTools`).

---

## Yarn command / function reference

Author these inside your `.yarn` nodes. They drive the fight that is currently on screen.

| Yarn | Effect |
|---|---|
| `<<fight_start>>` | Show the battle UI and enter the main menu (player's turn). |
| `<<fight_menu>>` | Return control to the ACT / ITEM / MERCY menu (end of an ACT branch that didn't finish the fight). |
| `<<fight_dodge "waveId">>` | Run the named `DodgeWave` (bullet-hell). **Blocks the node** until the wave ends. |
| `<<fight_damage n>>` | Remove `n` HP (scripted hit). Ends the fight as a loss if HP reaches 0. |
| `<<fight_heal n>>` | Restore `n` HP (capped at MaxHP). |
| `<<fight_spare_enable>>` | Unlock MERCY → Spare. Call this after the **correct** act. |
| `<<fight_end "win">>` / `<<fight_end "lose">>` | Finish the fight. Plays `WinNode` / `LoseNode` if set, else returns to the world. |
| `fight_hp()` | Function → current HP (int). e.g. `<<if fight_hp() <= 0>>`. |
| `fight_can_spare()` | Function → bool, true once spare is unlocked. |

Sub-activities use the **existing** framework command, not a new one:
`<<activity "settingsCode" "ReturnNode">>` launches a standard activity (Quiz/Match/…) and
jumps to `ReturnNode` when it closes; read the score with `GetActivityResult("settingsCode")`.

---

## How a turn flows

1. A Yarn node runs `<<fight_start>>` → menu appears (**ACT / ITEM / MERCY**).
2. **ACT** opens a submenu built from the inspector `ActOptions`. Picking one starts that option's
   Yarn node (`YarnAnturaManager.StartDialogue`).
3. That node plays lines and may `<<activity …>>`, `<<fight_dodge …>>`, `<<fight_spare_enable>>`,
   then **must** end with `<<fight_menu>>` or `<<fight_end …>>`.
4. **MERCY → Spare** (enabled only after `fight_spare_enable`) → win.
5. HP reaching 0 during a dodge → automatic `fight_end "lose"`.

**Win** = spare after the correct ACT(s). **Lose** = HP hits 0.

> While the battle is on screen, `UndertaleFight.Update()` keeps re-asserting the `Dialogue`
> gameplay state, because `DiscoverGameManager` reverts to `Play3D` (re-enabling world movement)
> every time a dialogue node ends. This keeps the player locked in the fight between turns.

---

## Editor wiring checklist

Put `UndertaleFight` on an **always-active** GameObject (a child `BattleRoot` panel is what gets
toggled on/off). Assign:

- **Player**: `MaxHP`. **Item**: `ItemHealAmount`, `ItemUses` (`-1` = unlimited).
- **End nodes** (optional): `WinNode`, `LoseNode` Yarn node names.
- **UI – Root & HP**: `BattleRoot` (panel), `HPFill` (a filled `Image`) and/or `HPLabel` (TMP).
- **UI – Main menu**: `MainMenu` container + `ActButton`, `ItemButton`, `MercyButton`.
- **UI – ACT submenu**: `ActMenu` container, `ActOptionsContainer` (parent for the list),
  `ActOptionTemplate` (a **disabled** Button used as the clone source), `ActBackButton`.
- **UI – Lose menu** (optional): `LoseMenu` panel + `TryAgainButton` (restarts the fight at full HP)
  and `ExitBattleButton` (runs the normal lose flow → `LoseNode` or back to the world). Leave
  `LoseMenu` unassigned to keep the old behaviour where losing ends the fight immediately.
- **UI – Dodge**: `DodgeBox` (a `RectTransform` defining the play area) with a **`FightSoul`** child
  (the heart) inside it; optional `BulletContainer` (defaults to the box).
- **ActOptions** list: per entry set `Label`, `YarnNode`, `DisabledAfterUse`.
- **DodgeWaves** list: per wave set `Id`, then **either** drag a `Pattern` (recommended — see below)
  **or** fill the legacy fields `BulletPrefab` (an Image + `FightBullet`), `BulletsPerSpawn`,
  `SpawnInterval`, `Speed`, `Duration`, `DamagePerHit`, `Shape` (RandomEdges / TopDown / Sides).
  If `Pattern` is set, the legacy fields are ignored and no prefab is needed.

Legacy bullet prefab = a small UI `Image` with a `FightBullet` component (RectTransform required).

To test fast: add `DebugFightLauncher`, assign the `Fight` reference, tick `AutoLaunch` or press the
play-mode button.

---

## Projectile patterns (recommended)

Author enemy attacks visually instead of tuning raw spawn fields.

1. **Open the tool**: `Tools ▸ Antura ▸ Projectile Pattern Designer` (or click *Open in Pattern
   Designer* on any `ProjectilePattern` asset). Press **New** to create an asset.
2. **Add emitters** (left panel). Each emitter is one attack with:
   - *Timing*: `StartTime`, `Bursts`, `BurstInterval`.
   - *Emission* `Shape`: **Aimed** (at the player), **Fan**, **Ring** (360°), **Stream** (rain from an
     `Edge`), **Spiral** (Ring + `SpinPerBurst`). `Count`, `SpreadAngle`, `BaseAngle` as relevant.
     Angles: **0° = down, 90° = right, 180° = up, 270° = left**.
   - *Movement*: **Straight**, **ZigZag**, or **Wave** (sideways `WaveAmplitude` / `WaveFrequency`).
   - *Appearance*: `Sprite` (**empty = plain white square**, or drop a PNG), `Color`, `Size`, `Speed`,
     `Damage`.
3. **Preview** (right panel) animates live — ▶ Play / ⏸ / ⟲ Restart / Loop / speed / scrub. It runs the
   exact runtime maths, so it's WYSIWYG. Set `Preview Box` to match your in-game `DodgeBox` for a 1:1
   match (positions auto-fit any box size; speeds/sizes are in pixels).
4. **Use it**: on `UndertaleFight`, set a `DodgeWave` `Id` and drag the pattern into its `Pattern`
   field. Yarn still triggers it with `<<fight_dodge "Id">>`. No bullet prefab required.

The dummy soul in the preview sits at the centre; **Aimed** shots target that point in preview, and the
live player's soul in-game.

---

## Example Yarn nodes (copy into the quest's `.yarn`)

> Not auto-added to `DEV_110 title - Yarn Script.yarn` on purpose: that file's explicit `#line:` tags
> are tied to the per-language string tables in `_localizations\`. Paste manually so the localization
> importer stays consistent. Untagged prototype lines are fine.

```yarn
title: Fight
---
The lion and the she-wolf close in. There is no running now.
<<fight_start>>
===

title: Act_Reason
---
Dante steadies his voice and tries to reason with the beasts.
<<fight_dodge "beast_lunge">>
<<if fight_hp() <= 0>>
    <<fight_end "lose">>
<<else>>
    For a moment, they hesitate.
    <<fight_spare_enable>>
    <<fight_menu>>
<<endif>>
===

title: Act_Recite
---
// An ACT that launches a standard activity, then returns here
<<activity "quiz_dante_canto" "Act_Recite_Result">>
===

title: Act_Recite_Result
---
<<if GetActivityResult("quiz_dante_canto") > 0>>
    The words ring true; the forest itself seems to listen.
    <<fight_spare_enable>>
<<else>>
    The verse falters.
    <<fight_dodge "beast_lunge">>
<<endif>>
<<fight_menu>>
===
```

Point the inspector `ActOptions` at `Act_Reason` / `Act_Recite`, set `WinNode` / `LoseNode`, and add a
`DodgeWave` whose `Id` is `beast_lunge`.
