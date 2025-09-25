---
title: Egg
nav_order: 0
---
# MiniGame: Egg


## Testing procedure
Total tests: 4
- Variations
	1. Egg_letters
    2. Egg_sequence
- Difficulty Levels: ininfluent

#### Shared Difficulty
- Higher difficulty hides visual hints
- The number of letters shown increases with difficulty


### Shortcuts
_none_

## Variations

### 1. Egg_letters
Player must find the correct letter.

#### Scoring
- 3 stars if...
- 2 stars if...
- 1 star if...
---
### 2. Egg_sequence
Player must find the correct letters in order.

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

### Objectives

**Pedagogical**: Associate a letter shape and its pronunciation.
**Play**: Press the right button to hatch the egg.

### Mechanics

**Attempt based**, no timer. N rounds (3/4)

At each round, an egg will appear rolling from the right side of the screen. It will stop in the center and emit the sound of a letter while happily bouncing in place.

Under the egg, there's a series of buttons with letters. The player will have to press the right button repeatedly to break the egg and hatch the LL. Would be nice if each LL had something procedural, to make it different, like a punk crest or a hat.

Pressing on the egg will repeat the letter/sequence-of-letters and the eventual button pattern.

- In initial difficulty levels, where just a single letter needs to be pressed, as soon as the egg speaks its letter the letter buttons will illuminate in sequence and each will say its letter.
- In later difficulty levels, where the egg will speak a series of letters, the buttons will activate/speak in the correct sequence. The player will have to repeat the pattern _(like in the old table game Simon)_.
- Later on, the buttons will not activate at all, so it will be all up to the player.

**NOTE**: in case of sequence of letters, where the same letter repeats more than once, the same button will have to be pressed more than once.

### Antura

Antura's face appears from the right and inhales, sucking the buttons in his mouth. Then spits them out in scrambled position and disappears.

### Difficulty variations
#### Gameplay
More buttons (3 to 8).

#### Pedagogical

- Buttons emit their letter's sound when the round start.
- Buttons don't emit their letter's sound.
- Multiple letters to guess correctly (extra layer, might represent a word, even if never spoken as a full word until we guess the correct pattern). Buttons light in correct pattern.
- Same as above, but buttons don't light nor say anything.

### End round (repeated per round)

#### Success
The egg will break (automatically, no dragging of the shell) and the LL will appear jumping happily. If we were playing with a sequence of letters, all the letters will be spoken and an LL for each letter will jump out and position itself horizontally. Otherwise just the single letter we were playing with.

#### Failure

**Condition**: N wrong attempts (3).

With each wrong attempt, the egg will rotate/move 90° to the left. After N wrong attempts, the wrong buttons will POOF away, and the egg will roll off screen.
