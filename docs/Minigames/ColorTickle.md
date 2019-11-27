# MiniGame: Color Tickle

## Testing procedure
Total tests: 1
- Variations:
    1. ColorTickle
- Difficulty Levels: ininfluent

### Shared Difficulty
- Available lives decrease
- Antura activation time decreases

### Shortcuts
_none_

## Variations

### 1. ColorTickle
Player must color the letter.

#### Scoring
- 3 stars if...
- 2 stars if...
- 1 star if...
---
## Developer notes

## Issues

## Warnings to be fixed

## Optimization

---

## Game Design Docs

3 Lives per round (= per Letter)
- Each time you draw out of the borders (=Tickle) you lose a life

What is drawn outside of the borders should appear “lighter”
- I.e. if the LETTER is draw in RED, you should use LIGHT RED outside of the borders
- NOTE: you can do it by using a 50% brighter color

We keep the version (game_ColorTickle) with the Tickle anim
- But we do not stop the drawing, the Player must react and raise his/her finger

The Arabic Letters must appear in a correct format (but we can zoom more)
- It cannot go out of the “White Square” (please do not scale it and leave it centered)...
- … and it cannot appear as “bold” as now (i.e. the dots connects together)
- It seems that you currently using the the font as Bold with Outline…
- … please use a smaller Bold (dots should not connect together) and a very thin black outline

Antura is an element of surprise/disruption
- Enters from the side, leaves a short time to let the player react and then barks scaring the LL
- The LL will change animation when scared, making it very difficult/impossible to draw
- So the player is obliged to raise his/her finger to avoid drawing outside of the borders
- NOTE: it would cool that Antura only enter when the player is really actively drawing!

### Scoring

- All mini-games must return a score (0 to 3 stars)
  - 0 stars are returned only if the player fails ALL ROUNDS
  - 3 stars are a very good performance, not necessarily PERFECT

The Score could be calculated with a combination of bonus/malus:

BONUS:
- Complete a Letter
- Number of Lives left at the end of a round

MALUS
- Number of Lives lost in a round
- Amount of “out-of-borders” that has been colored
