---
title: Logging System
nav_order: 0
---
# Logging System

This document describes the Logging System implemented in Antura.
Logging allows the application to gather information on the player's progression and save it to the database (see [Database](Database.md)).

The Logging System is based on three main classes: the MinigameLogManager, the LogManager, and the LogAI.

- The **MinigameLogManager** is the concrete implementation of **ILogManager** that is passed to the Game Context instance and provides access to the logging functionalities from MiniGames.
- The **LogManager** is the entry point for all log functionality throughout the application, used both by the MinigameLogManager and the rest of the core codebase.
- The **LogAI** mediates the logging of data by the core application so that correct values are inserted into the database and filters it according to the learning rules that can be configured by the expert.

`@todo: add MinigameLogManager -> LogManager -> LogAI -> Database graph`

The data that can be logged is divided in two broad categories:
- **History data** represents the temporal progression of the player as the app is used.
- **Score data** represents the summary achievements of players.

## History data

Four types of history data are supported:
- Generic data on application usage (**LogInfoData**)
- Data on learning achievements by the player (**LogLearnData**)
- Data on daily mood levels of the player (**LogMoodData**)
- Data on play-related measurements logged by MiniGames (**LogPlayData**)

Each type follows a specific path in the logging system.

## Mood logging

Mood logging is performed in the Mood scene.
The scene obtains from the player an integer value between *AppConfig.minMoodValue* and *AppConfig.maxMoodValue*.
This value is then passed to *LogAI.LogMood(int mood)*, which inserts it as a new *LogMoodData* instance inside the database, with a timestamp.

## Info logging

Info logging is performed whenever a specific **InfoEvent** is encountered.
Calls to LogInfo are scattered throughout the core application.
Whenever logging should be performed, a call to **LogManager.LogInfo(InfoEvent infoEvent, string parametersString = "")** is performed, with additional optional string parameters.
The LogManager then routes the call to **LogAI.LogInfo()**, which inserts the event as a new *LogInfoData* instance inside the database, with a timestamp.

## Play logging

Play data is defined by the class **LogPlayData** and represents skill-related measurements logged during play during MiniGames.

The logging is a two-phases procedure.
As a first phase, the MiniGame gathers measurements whenever it detects that a play action has been performed. This can happen anytime while playing the MiniGame.

This is achieved through a call to **OnGameplaySkillAction(PlaySkill _ability, float _score)** by the MiniGame code.
This will create a **PlayResultParameters** instance.

The MiniGame is responsible fo deciding what skill the action belongs to (thorugh the PlaySkill enumerator) and what value to assign to the action.
**Note that values on skill measurements of different MiniGames have no relation to each other and should only be used to compare different sessions of the same MiniGame**

**PlayResultParameters** instances are buffered by the MinigameLogManager during play.
As the MiniGame ends, the MinigameLogManager collects the buffered instances and flushes them to the LogManager.

The LogManager then routes the call to **LogAI.LogPlay()**, which inserts the list of values inside the database as new instances of *LogPlayData*, with a timestamp.

This could be, as an example, the reaction time when selecting the correct answer, or the accuracy of movement for a player-controlled character.

`@todo: PlaySkill`

## Learning logging

Learning data is defined by the class **LogLearnData** and represents the learning achievements of the player regarding a specific vocabulary content (letter, word, etc.), hereby referred to as *Learning Item* for ease of discussion.

These measurements are very important for the teaching goals, so the system is designed to help MiniGames in standardizing their measurements.

Basically, the MiniGame collects data on the number of correct and wrong answers related to a given learning content instance, and a summary success value is retrieved from these numbers.
The following text explains the process in detail.

The logging is a two-phases procedure.
As a first phase, the MiniGame gathers measurements whenever it detects that a learning action has been completed.
This can happen anytime while playing the MiniGame and is related to a specific Learning Item.
The MiniGame code may call5 `MinigameLogManager.OnAnswer(ILivingLetterData _data, bool _isPositiveResult)`, determining whether an interaction with a specific learning content instance (represented by the ILivingLetterData reference) is to be considered positive or negative.
This will create a **ILivingLetterAnswerData** instance.

**ILivingLetterAnswerData** instances are buffered by the MinigameLogManager during play.
As the MiniGame ends, the MinigameLogManager collects the buffered instances and creates a **LearnResultParameters** instance for each Learning Item found in the buffered answers, which represents a summary performance value for that Datum.

A list of **LearnResultParameters** instances is thus created, which are then flushed to the LogManager.

The LogManager then routes the call to **LogAI.LogLearn()**.
The LogAI will then retrieved the **MiniGameLearnRules** for that specific MiniGame, a collection of parameters used to define how the obtained values should be interpreted .

The **MiniGameLearnRules** define how to convert the correct/wrong measurements to a summary **[-1,1]** *grade* value that can be later used to compare the current performance to other play sessions for the same MiniGame or of other MiniGames.

**MiniGameLearnRules** can be parametrized through its fields based on the MiniGame's nature:
- **VoteLogic voteLogic** defines the underlying logic for how to interpet the number of correct and wrong results to determine the outcome of a single MiniGame related to a specific data content.
- **float logicParameter** parameterizes the vote logic. For example, in case of VoteLogic.Threshold, this is the threshold value.
- **float minigameVoteSkewOffset** is used to offset all grade values if a MiniGame is found to not correctly represent the correct outcome.
- **float minigameImportanceWeight** is multiplied to the final grade to give more or less importance to the MiniGame as a learning tool.

After a grade is obtained for each Learning Item found in the play session, the LogAI inserts the results of values inside the database as new instances of *LogLearnData*, with a timestamp.
At this time, the overall learning score for the Learning Item is also updated using a moving average to reflect its current state.

**As an example**, a MiniGame may show a specific letter of the alphabet five times during play, each time requiring the player to recognize the letter.
If the player recognizes the letter three out of the five times it is shown, the MinigameLogManager registers a value of 3 correct answers and 2 wrong answers for that specific letter.

If the MiniGame learning rules define that the correct-to-wrong ratio should be considered as a threshold, and that at least 50% of answers must be correct, the final grade for that play session will be 1 (3 out of 5).

## Score data

In addition to history data logging, the Logging System also supports logging of overall score values for most of the content found in the game.
This score value is used to represent the current learning proficiency of the player and is also used by the Teacher System to control the learning progression.

In particular, the system logs:
1) Current learning score value for Letters, Words, Phrases inside a `VocabularyScoreData` instance. This is a value in the `[-1,1]` range and represents a moving average of past learning scores for that learning item. A score update is performed at the end of each MiniGame. See the above discussion on *Learning logging* for further details.

2) MiniGame-related score levels for MiniGames, PlaySessions, LearningBlocks inside a **JourneyScoreData** instance.
This is a value in the `[1,3]` range and represents the maximum score achieved for that item. A score update is performed at the end of each MiniGame, play session, or learning block respectively.

This happens at different times and in different ways:
- For Minigames, the score is saved at the end of the MiniGame itself, through a call to `MiniGame.EndGame(int stars, int score)`, subsequently to `MinigameLogManager.OnMiniGameResult(...)`,	`LogMinigameScore.LogMinigameScore(...)`, and finally `LogAI.LogMiniGameScore(...)`.
- For Play Sessions, the score is saved at the end of the play session, i.e. when the PlaySessionResult scene is accessed. This is performed in `PlaySessionResultManager.Start()`, which calls `LogManager.LogPlaySessionScore(...)` and finally `LogAI.LogPlaySessionScore(...)`.
- For Learning Blocks, @TODO: THIS IS NOT DEFINED!!

To generate the score values, the LogAI implements a few utility methods that help in updating the score values kept in the database using the correct logic:

- `UpdateScoreDataWithMaximum()` updates the score value by keeping only the maximum value between the current and the new one.
- `UpdateScoreDataWithMovingAverage()` updates the score value by performing a moving average on the score value.

### Refactoring Notes

- LogAI and LogManager are a bit redundant, could be merged.
- LogAI could be separated from the teacher.
- Better nomenclature in general is needed
