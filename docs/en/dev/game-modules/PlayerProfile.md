---
title: Player Profile
nav_order: 0
---
# Player Profile

A Player Profile contains data on a given player, allowing multiple players to use the application and to keep updates on their progression.

## Contents

A Player Profile contains the following data:

* **Profile information**
  * *Id*: unique Id assigned to the player.
  * *Age*: age of the player.
  * *Name*: player name.
  * *AvatarId*: chosen avatar image Id.
* **Journey progression state**
  * *ProfileCompletion*: a value that defines the state of the profile in respect to tutorial scenes.
  * *CurrentJourneyPosition*: the current selected position in the map journey.
  * *MaxJourneyPosition*: the maximum reached position in the map journey.
* **Rewards state**
  * *TotalNumberOfBones*: number of bones collected.
  * *CurrentAnturaCustomizations*: selected customization for Antura.

Note that additional progression data is also contained in the runtime database (see [Database](Database.md)).

Note that each profile is assigned an unique Id.
This Id is used for:

* selecting and identifying the player profile by the Player Profile Manager
* identifying the database assigned to the player (see [Database](Database.md))

## Serialization

All data related to the Player Profile is serialized and saved inside the dynamic database.
Refer to the [Database](Database.md).

## Creation & Deletion

The Player Profile Manager handles creation, selection, and deletion of player profiles.
The system is designed to support a maximum number of players, defined as `PlayerProfileManager.MaxNumberOfPlayerProfiles`.

A list of existing player profiles can be retrieved from the `AppManager.GameSettings.AvailablePlayers`.

Whenever a Player Profile is created, an exclusive Avatar Id is also selected, which represents the avatar image assigned to that profile.

`PlayerProfileManager.CurrentPlayer` holds the current player profile.
`AppManager.I.GameSettings.LastActivePlayerId` contains the Id of the profile last accessed through the application.

At runtime, creation, deletion, and selection of player profiles is performed in the Home (`_Start`) scene through the **Profile Selector**.


## Refactoring Notes

- Mood and PlaySkill values in PlayerProfile are not used. Should be removed.
