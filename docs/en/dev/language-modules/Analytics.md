---
layout: default
title: Analytics
parent: Modules
nav_order: 0
---
# Antura Analytics

We have a page on what we would like to track from data: <https://docs.antura.org/GameDesign/DataAnalysis.html> and here everything we log locally in the app: <https://docs.antura.org/Modules/Logging.html>

To access the online analytics: [Antura Unity Analytics Dashboard](https://dashboard.unity3d.com/organizations/567035/projects/ca7a3389-ec6c-44b1-a44d-aa0da4930165/overview?viewMode=project)

## Unity Analytics
In Antura we use Unity Analytics <https://docs.unity.com/analytics/>, which offer many default / standard game tracking data, like:

| Metric | Description |
|---|---|
| DAU | Number of unique players per day |
|DAU (new vs. returning)|Percent of DAU who were new on that day |
| WAU | Number of unique players in the previous seven days|
|MAU | Number of unique players in the previous 30 days |
| DAU per MAU | Percentage of MAU who played on a particular day (DAU/MAU)|
|New users | Daily users who are new that day |
|Session length |Time elapsed from when the user starts the app, and exits|
|Number of sessions|Number of sessions played that day|
|Sessions per user|Average number of sessions for each user|
|Total daily play time|Total playing time of all players on that day|
|Daily play time per DAU|Average playing time of users playing on that day|
|Day 1 retention|Percentage of users who returned to your game after one day|
|Day 7 retention|Percentage of users who returned to your game after one week|
|Day 30 retention|Percentage of users who returned to your game after 30 days|

And we added some custom event to track our players progression in the game and the gameplay.    
Unity Analytics (the new beta version we are enabled to use) has also an SQL Data Export to create custom queries (<https://docs.unity.com/analytics/SQLDataExplorer.html>)

## Environments
we have two environments:
- **production** (published apps)
- **dev** (from editor and dev builds)

## Terminology
**JP** = Journey Position ([see](./Journey.md))
is identified by a the sequence **X.Y.Z** where X is the Stage, Y the Learning Block, and Z the Play Session.

## Custom Events

### Shared Parameters
every custom events sends also these params:
```
myPlayerUuid: PlayerUUID
myEdition: ContentID
myNativeLang: string Iso3 Code
```

### Track Learning
Correct or wrong answer? (in Assessments)

CustomEvent: ` myLearning`

Parameters:
```
myMinigame: miniGameCode.ToString()
myJP: currentJourneyPosition.Id
myStage: currentJourneyPosition.Stage
myLearningBlock: currentJourneyPosition.LearningBlock
myPlaySession: currentJourneyPosition.PlaySession
myLearningDataType: answer._data.DataType
myLearningDataId: answer._data.Id
myLearningIsCorrect: answer._isPositiveResult
```

### TrackMiniGameScore
When player finishes a minigame

CustomEvent: `myMinigameScore`

Parameters:
```
minigame: string code
score: int (0,1,2,3 stars)
duration: int seconds
JP: Journey Position
```

### TrackReachedJourneyPosition
When the player advances in the journey

CustomEvent: `myLevelUp`

Parameters:
```
JP: Journey Position
Stage: int
LearningBlock: int
PlaySession: int
TotalPlayTime: int seconds played in minigames
TotalStars: int
TotalBones: int
```

### CompletedRegistration
When player creates an Avatar (first step in the game)

CustomEvent: `myCompletedRegistration`

Parameters:
```
myGender: string ("Undefined", "M", "F")
myAge: int
myProfileNumber: how many profiles exist in this edition
myAvatar_Face: id
myAvatar_BgColor: string color
myAvatar_HairColor: string color
myAvatar_SkinColor: string color
```

### TrackCompletedFirstContactPhase
When player finishes the initial tutorial

CustomEvent: `myTutorialComplete`
Parameters:
```
myPhase: string
```

### Track Shop / Item Bought
When bones are spent in the Antura Space

CustomEvent: `myItemBought`

Parameters:
```
myBonesSpent: int
myItemBought: string item code
```

### Track Antura Customization
When player customizes Antura in the Antura Space

CustomEvent: `myAnturaCustomize`

Parameters:
```
myDuration: int time spent in Antura Space
myAntura_Head: object name
myAntura_EarL: object name
myAntura_EarR: object name
myAntura_Nose: object name
myAntura_Jaw: object name
myAntura_Neck: object name
myAntura_Back: object name
myAntura_Tail: object name
myAntura_Texture: object name
myAntura_Deca: object name
```

### TrackPlayerMood
When player replies to the mood question (once a day)

CustomEvent: `myPlayerMood`

Parameters:
```
myPlayerMood: int (1-5)
```

### TrackBook
When the book is used.. what does player do?

CustomEvent: `myBook`

Parameters:
```
myBookAction: string action (letter, word, launch minigame)
myBookObject: string object code (letter, word, minigame code)
```
