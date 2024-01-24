---
layout: default
title: Balloons
parent: Minigames
nav_order: 0
---
# MiniGame: Balloons

## Testing procedure
Total tests: 4
- Variations
    1. Balloons_letter
    2. Balloons_spelling
    3. Balloons_words
    4. Balloons_counting
- Difficulty Levels: not used

#### Shared Difficulty
Ignored

### Shortcuts
_none_

## Variations

### 1. Balloons_letter
Player must find the correct letter.

#### Scoring
- 3 stars if...
- 2 stars if...
- 1 star if...
---
### 2. Balloons_spelling
Player must find the correct letters in order.

#### Difficulty
Ignored

#### Scoring
- 3 stars if...
- 2 stars if...
- 1 star if...
---
### 3. Balloons_words
Player must find the correct word.

#### Scoring
- 3 stars if...
- 2 stars if...
- 1 star if...
---
### 4. Balloons_counting
Player must find the correct words in order.

#### Scoring
- 3 stars if...
- 2 stars if...
- 1 star if...
---
## Developer notes

## Issues

## Warnings to be fixed

**tons of**:
- [Warning] [BalloonsGame] [OnPoppedNonRequiredBalloon] Animator has not been initialized.
- [MakeWordPromptGreen] Animator has not been initialized.
- [Warning] [LetterPromptController] [OnStateChanged] Animator

## Optimization

there are many deactivate GameObjects.
some are very big, like `Environment_old` can they be deleted?
