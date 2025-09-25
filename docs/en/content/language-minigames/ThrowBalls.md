---
title: Throw Balls
nav_order: 0
---

## Testing procedure
Total tests: 3
- Variations
    1. ThrowBalls_letter
	2. ThrowBalls_words
	3. ThrowBalls_lettersinwords
- Difficulty Levels: ???


### Shared Difficulty
- Number of balls decreases
- ????? (TODO, not clear)


### Shortcuts
_none_

## Variations
### 1. ThrowBalls_letter
Player must find the correct letter.

#### Scoring
- 3 stars if...
- 2 stars if...
- 1 star if...
---
### 2. ThrowBalls_words
Player must find the correct word.

#### Scoring
- 3 stars if...
- 2 stars if...
- 1 star if...
---
### 3. ThrowBalls_lettersinwords
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

## Open Questions
- Explain difficulty use

---

## Game Design Docs

**Pedagogical Objectives**: Differentiate a letter among others.
**Play Objectives**: Use physics skills to hit the right target.

### Mechanics

**Attempt-based**: no time limit, just balls limit per round.

Depending on the round/difficulty-level, the LLs can be:

- Static (on ground or above a pile of boxes)
- Above a pile of boxes that oscillates left and right.
- Jumping/dancing (on the ground or above a pile of boxes—oscillating or not)
- Hidden behind bushes (player will have to get the correct timing to hit them when they pop out, before they hide again)

**NOTE**: hitting the boxes will involve the physics engine: the boxes will bounce around until they become static and disappear with a POOF, while the LL will fall flat on the ground and stand up (this might add a tactical choice: waste a ball to make an LL fall and be easier to hit).

### Antura

When the player throws the ball, sometimes Antura will appear from off-screen and grab it before it lands. He will then run around with it, and the player will have to grab the ball and drag it away from his mouth in order to re-capture it (without losing it). If the player doesn't do it on time Antura will run away and the ball/life will be lost.

**Note**: the ball is almost as big as Antura's face (to allow for easier interaction).

**Technical note:** if dragging the ball is a problem (both technically or animation-wise, since it would require Antura to stop and pull back a little, like dogs do when you want to steal their ball) we could just have the player "touch" the ball.

### Feedback

When a wrong LL is hit, she strongly bounces (animation with LL bending back then pushing the ball forward with her chest) the ball back against the screen (the ball will be lost). Each time the screen is hit, a "cracked glass" effect appears.

### Difficulty variations

#### Gameplay

- More and more moving elements etc (oscillating crates, LLs hiding behind bushes, flying LLs, etc)

#### Pedagogical

- In more basic levels the actual letter to hit could be shown on screen, while later on it will not be shown at all
- Hardest difficulties will show more letters and possibly letters that are more similar to each other, so the player will be more skilled in recognizing them

### Endgame

The game will end under one of these conditions:

- Too many rounds were lost
- The required amount of rounds were won

#### Success - repeated multiple times per game

The right LL will disappear with a POOF and reappear (POOF again, hooray) in front of the camera, VICTORYRAYS behind her, and play a "hooray" animation. The wrong LLs will dance in the background _(using one of the first dance animation from DANCING DOTS)_.

The letter/word audio will play the instant the reappearing POOF begins.

**Note**: this must be a fun but quick animation, since it will happen various times for every successful round.

#### Failure - repeated multiple times per game

**Condition**: no more balls and not enough correct LLs hit.

All the wrong LLs will disappear with a POOF, while the right LL will be evidenced (ray of light?) and will play a "provocation" animation (as if stating that she won, instead of the player—for example, hands on her hips, chest out). If the LL was hidden behind a bush, she will come out before playing the animation.

The letter/word audio will play the instant the right LL begins striking her pose.

### Variations

- The LLs are all valid targets, and they must be hit in the correct order to compose a word.
- LLs contain images instead of words
