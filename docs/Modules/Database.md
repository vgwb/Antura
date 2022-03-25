---
layout: default
title: DB Management
parent: Modules
nav_order: 0
---
# Database Management

* TOC
{:toc}

The application uses two different databases for learning data and logging data.

## Learning Data (static database)

Learning data comprehends all information needed to provide a correct learning experience for the players.
This data is included with the application at startup.

This includes:

- **Journey data**: stage, learning block, and play session configurations
- **Vocabulary data**: letters, words, and phrases of the given language.
- **MiniGame data**: details on the available MiniGames and how they tie with Journey and Vocabulary data.

The data is compiled into JSON files contained inside the `_manage/manage_Database/Datasets` folder.
The JSON files are loaded using the `_manage/manage_Database/manage_Database` scene, where consistency checks are performed and the database contents can be inspected.
`@todo: explain DataParsers too.`

The database is converted from JSON to a set of custom assets (deriving from **ScriptableObject**) that contain a table of data, with one asset per data type. The contents of *Database/DatabaseObjects** define the scriptable objects from which the custom assets are derived.
These assets can be found in the **Assets/Resources/Database** folder.
To perform the JSON-to-asset conversion, the **DatabaseLoader** scripts employs a *DataParser* for each data type to load, which defines how to parse the JSON file into the corresponding data structure.

At runtime, **Antura.Db.Database** functions as an entry point for all the assets containing the data tables and is managed by a **Antura.DatabaseManager** instance.

`@todo: describe Journey and MiniGame data?`

## Dynamic Data

Dynamic data is produced as the player uses the application and saved to the system's memory at runtime.

The data is divided in several categories:

- **Logging data** represents the temporal progression of the player as the app is used. This includes:
  - **Log Info Data**: Generic data on application usage
  - **Log Learn Data**: Data on vocabulary learning achievements by the player
  - **Log Mood Data**: Data on daily mood levels of the player
  - **Log Play Data**: Data on play-related measurements logged by MiniGames

- **Score data** represents the summary achievements of players. All score data is contained in **ScoreData** objects. This includes:
  - **Vocabulary Score Data**: Current learning score value for Letters, Words, Phrases
  - **Journey Score Data**: MiniGame-related score levels for MiniGames, PlaySessions, LearningBlocks

- **Reward Pack Unlock Data** is used for customization unlock purposes.

- **Database Information Data** holds summary details on the current database and is used for versioning.

- **Player Profile Data** holds the player's information and current preferences.

See the [Logging](Logging.md) document for further details on logging.

### SQLite

The database is implemented in SQLite.
The SQLite database is loaded and connected to whenever a player profile is selected, and generated if non-existing.
All communication with the SQLite database is performed through a **Antura.Db.DBService** instance, managed by the **DatabaseManager**.
The structure of the SQLite database can be generated a runtime and this is controlled through the `DBService.GenerateTable(bool create, bool drop)`, which can be updated to reflect any changes in the DB scheme.
Note that any change to the database scheme must also prompt a sequential update of **AppConfig.DbSchemeVersion** for versioning to function correctly.

## Profile API

The logging database supports multiple profiles.
A profile can be selected using `Antura.Db.DatabaseManager.LoadDynamicDbForPlayerProfile(int profileId)`, which loads (or creates if it does not exists) a database for logging data of the chosen player.
Player profiles are also supported with:

- New profile creation (through `CreateProfile()`)
- Profile deletion (through `DropProfile()`)

## Reading API

To read learning or logging data, a single entry point is used throughout the application.
**Antura.DatabaseManager** is the entry point and can be access through the public field `AppManager.I.Db`.

The Database Manager provides several methods for each data type represented in the learning and logging data.

To access learning data, the following methods can be used:

- `GetAllXXXData`, which returns a list o all data of the chosen type.
- `GetXXXDataById`, which returns a single data structure of the chosen type given its Id.
- `FindXXXData`, which require a predicate as a parameter to filter the database. The methods returns the list of filtered data of the chosen type.

To access logging data, the following methods can be used:

- For logging data, literal queries in SQL can be also used through `FindXXXDataByQuery()` methods.


## Writing API

To write to the database, the Database Manager provides the following methods:

- `Insert<T>()` to insert new data in the database (used by the logging system)
- `UpdateVocabularyScoreData()` to overwrite the current vocabulary score data for a given element.
- `UpdateJourneyScoreData()` to overwrite the current journey score data for a given element.
- `UpdateRewardPackUnlockData()` to overwrite the current unlock state for a given reward.
- `UpdatePlayerProfileData()` to overwrite the current player profile.

Note that these methods should not be called directly and that all MiniGames should use the *LogManager* to indirectly write to the database.
Note that the vocabulary data is static and thus not writeable at runtime.

## Refactoring notes

- The logging data structures should be better defined. They are, for now, too little strict and not enough documentation on their purpose is available.
- Insert/Update should not be directly exposed and instead be used by the log manager and protected from other uses
- There is a strong dependency in the code on the specific needs of the language (are words/phrase/letters needed in every language?)
- Vocabulary and Journey data should be separated, so to better enforce their nature.
- Localization data should be separated from the rest
