# MiniGame: Maze

![](images/Maze.jpg)
![](images/MazeTutorial.jpg)

## Testing procedure
Total tests: 1
- Variations
	1. Maze
- Difficulty Levels: ininfluent

### Shared Difficulty
- Timer decreases with difficulty

### Shortcuts
_none_

## Variations

### 1. Maze
Player must draw the correct letter.

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

**Pedagogical Objectives**: Learning the shape of a letter by drawing it (addendum: also learn how to draw a letter correctly).

**Play Objectives**: Along a racing track, draw as fast as possible the correct path so the LL can reach the end and grab the bonuses.

### Mechanics + Visuals

**Time-based** (very very time-based).

**Camera**: top-view (perspective).

The player will have to draw a path along a race track (aka the letter to trace) as fast as possible, using the correct drawing direction (start to finish line), while the LL (who is riding an idle rocket) waits. As soon as the player reaches the end point, the LL will ignite the rocket and follow the drawn path at a very high speed.

When a round starts, we hear the sound of the letter.

**NOTE**: the rocket will have to be colorful/cartoonish, to make it clear it's a fun-rocket and not a missile.

- When the path incorrectly exits the track, the LL will behave as if it hit the track's side and start bouncing around _(kind of like Mario Kart when struck by lightning, but possibly even more ridiculous)_ then get back on the track where the path got back.
- If the LL hits the track's side too many times, the LL is thrown off the rocket and the round is lost.
- **The track will contain various bonuses** (fruits?), so more points will be gained if the drawn path not only stays inside the track, but also allows the LL to go over and grab these bonuses. This allows for larger tracks and a difficulty-based margin of error, but also more bonuses when following the track/letter precisely.
- Eventual dots will have to be "pressed" at the end, and should appear like buttons instead of external tracks.
- **When a player loses a round**, if he still didn't lose the whole game (as in: didn't lose N rounds), he can either move on to another letter, or use a "retry card" to retry the same failed letter.

  - The retry system is actually always in place. If the player fails a letter it will always be pushed to repeat it. But he can use a "move on" card to change to a new one—and lose some time as a penalty_ (this would be also interesting to store as player data, to know what letters the kid skipped more).

### Antura

If the player doesn't complete the path on time, Antura appears and chases the LL. The LL ignites the rocket and escapes off-screen (still chased by Antura). The round is lost.

### Difficulty variations
**Gameplay**

Less time to draw.
Time attack should be an important factor, so we reward the player more than punishing him if he goes faster than necessary.

**Pedagogical**

We should follow the Lebanese approach to letter sequences, so the harder the pedagogical difficulty, the more complex the letter to draw

### Endgame

The game will end when the game timer elapses (timer will need to be paused during "new round" animations).

#### Success - repeated multiple times per game

The LL reaches the track's end, and fireworks/other-effects appear.

#### Failure - repeated multiple times per game

- Condition A: the path (and thus the speeding LL) hit the track's border too many times. The LL falls from the rocket. The rocket starts rocketing upwards into the sky (see sketch to have an idea of the upwards movement: not straight but spiraling) until it hits the screen and bounces away. The screen will crack (same effect as THROW BALLS).
NOTE: the rocket's spiral/bounce will need to be pretty fast, both because the game is fast and to move on quickly to the next round (or to retry the current one - see "retry card" concept).
- Condition B: the player takes too long to draw the path, and Antura chases the LL. The LL ignites the rocket and runs away off-screen, chased by Antura.
