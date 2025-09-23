---
title: Data Analysis
parent: Game Design
nav_order: 3
---
# Data Analysis

the app logs several types of data during the gameplay to allow analysis of behaviours and results of the learning experience.

see docs [Logging](../language-modules/Logging.md) and [Database](../language-modules/Database.md) for technical details.

these are the main queries that we need to extract from data:

1. Curve of Duration of play sessions (one player, a group)
If this refers to a Play Session: We query LogJourneyScoreData.TotalPlayTime by AppSession.
If this refers to an App Session: We need to query LogInfoData and retrieve play time by comparing timestamps.
(requires PlayTime logging #182 )

1. Curve of Duration of play per day (one player, a group)
Same as above, but we filter by timestamps to retrieve the day.

1. Nb of mini-game/assessment per play session (one player, a group)
I guess this referring to an 'app session'.
In that case: We query LogMinigameScoreData filtering by AppSession

1. Nb of mini-game/assessment per day (one player, a group)
Same as above, but we filter by timestamps to retrieve the day.

1. Learning blocks ID or T0+x date of drop out (one player, a group)
We may query LogJourneyScoreData to retrieve the progression in play sessions and compare timestamps.

1. Antura Space use duration per play session and per day (one player, a group)
We query LogInfo looking for AnturaSpace (enter) and AnturaSpace (exit)

1. Score evolution per mini game (the rough data that we use to create the 3 stars score) (one player, a group)
We query LogMinigameScoreData.Score and filter by Minigame code.

1. The average evolution of score, all minigame (in stars) (one player, a group)
We query LogMinigameScoreData.Score and filter by Minigame code.

1. Evaluate correlation between dropout/non play and score variation/new game apparition (a group)
We can detect dropout by analyzing LogInfo timestamps and either JourneyScoreData or LogJourneyScoreData.
We can detect score variation with LogMinigameScoreData.
We can detect new game apparition with LogMinigameScoreData or MinigameScoreData

1.  Average assessment tries per learning block (to identify potential more difficult learning blocks) (one player, a group)
We can detect failed assessments by querying LogMinigameScoreData, filter by assessment codes, and getting the score.

1.  Ranking of words associated to failure in mini game (one player, a group)
We can retrieve LogLearnData to check scores, play sessions, and minigames.

1.  Mod indicator curve (one player, a group)
We can use LogMoodData

1.  Replayed learning blocks (one player, a group)
We can use LogJourneyScoreData to detect multiple plays for the same play sessions (and thus learning blocks).

1.  More used outfits, colors and accessories, on Antura (one player, a group)
We need to create a LogInfo event that tracks a change in customization