# Antura Analytics

Antura Docs:
All current updated docs are here: <https://docs.antura.org>

We have a page on what we would like to track from data: <https://docs.antura.org/GameDesign/DataAnalysis.html> and here everything we log in the app: <https://docs.antura.org/Modules/Logging.html>

## Unity Analytics
In Antura we use Unity Analytics <https://docs.unity.com/analytics/>

To access Unity Analytics: <https://dashboard.unity3d.com/organizations/567035/projects/ca7a3389-ec6c-44b1-a44d-aa0da4930165/overview?viewMode=project>
Which offer many default / standard game tracking data, like:

|  |  |
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

## Terminology
**JP** = Journey Position ([see](../Modules/Journey.md))
is identified by a the sequence **X.Y.Z** where X is the Stage, Y the Learning Block, and Z the Play Session.


## Custom Events

### TrackMiniGameScore
When player finishes a minigame
```
Parameters:
{
    { "minigame", miniGameCode.ToString() },
    { "score", score },
    { "duration", (int)duration },
    { "JP", currentJourneyPosition.Id }
};

CustomEvent: myMinigameScore
```

### TrackReachedJourneyPosition
When the player advances in the journey
```
Parameters:
{
    { "JP", jp.Id },
    { "Stage", jp.Stage },
    { "LearningBlock", jp.LearningBlock },
    { "PlaySession", jp.PlaySession }
};

CustomEvent: myLevelUp
```

### CompletedRegistration
When player creates an Avatar (first step in the game)
```
Parameters:
{
    { "id", playerProfile.AvatarId },
    { "bg_color", ColorUtility.ToHtmlStringRGB(playerProfile.BgColor) },
    { "hair_color", ColorUtility.ToHtmlStringRGB(playerProfile.HairColor) },
    { "skin_color", ColorUtility.ToHtmlStringRGB(playerProfile.SkinColor) },
    { "tint", playerProfile.Tint }
    { "myGender", playerProfile.Gender.ToString() },
    { "myAge", playerProfile.Age }
};

CustomEvent: myCompletedRegistration
```

### TrackCompletedFirstContactPhase
When player finishes the initial tutorial
```
CustomEvent: myTutorialComplete
```

### TrackSpentBones
When bones are spent in the Antura Space
```
Parameters:
{ "bonesSpent", nSpent }

CustomEvent: myItemSpent
```

### TrackCustomization
When player customizes Antura in the Antura Space
```
Parameters:
{
    { "customization", customization.GetJsonListOfIds() },
    { "AnturaSpace_playtime", (int)anturaSpacePlayTime }
};

CustomEvent: myAnturaCustomize
```
