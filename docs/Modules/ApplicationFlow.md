# Application Flow

* TOC
{:toc}

`@todo: make sure to add a summary to all the main managers so to generate the docs.`
`@todo: add an app flow diagram too?`

## Core and Minigame code

The code of Antura is separated in two main sections.

- **Core** code is related to the main application. This code should not be touched by MiniGames developers.
- **MiniGames** code is produced by MiniGames developers. This code should be dependant on the core Code, but not the other way around.
This allows MiniGames to be created and removed at will.

Note that part of the *core* code can be used by MiniGame code, such as *Tutorial* or *UI* code, this is referred to as *shared MiniGames code* and is part of the core application.

## App Manager

The AppManager is the core of the application.
It is instantiated as a Singleton, accessible as **AppManager.I** and works as a general manager and entry point for all other systems and managers.

The AppManager is used to start, reset, pause, and exit the game.
It also controls the general flow of the application.

The `AppManager.GameSetup()` method functions as the entry point of the application, regardless of player profile.
All subsystems initialisation is carried out in this method.
The `AppManager.InitTeacherForPlayer()` method instead initializes all subsystems and loads all data related to a specific player profile and must be called whenever a new profile is selected.

## Flow

This section details the flow of the player inside the application and what classes and sub-systems are affected.

The flow of the whole application is handled by the `Navigation Manager`, which controls the transitions between different scenes in the application.

### Start, Home and Intro

The entry point for the application is the **Start scene** (`app/_scenes/_Start`), managed by the *Home Manager*.
This scene initialises the *App Manager*, shows the *Profile Selector UI* to allow the user to select a profile throught the *Player Profile Manager*.
This is performed through a set call to the **PlayerProfileManager.CurrentPlayer** property.

The static learning database, the player's logging database, and the teacher system are loaded at this point through a call to **AppManager.InitTeacherForPlayer()**.
After the profile selection is confirmed through the UI, the *Home Manager* calls the *Navigation Manager* to advance the application.

The application flow may then change depending on whether we are using a new profile (*first encounter*) or not.

If we are in the *first encounter* phase, the *Navigation Manager* will first load the **Intro scene**, which is controlled by an *Intro Manager*.
From there, the user eventually accesses the **Map scene**.
If the first encounter is instead passed, the Map is accessed directly.

### The Map scene

The **Map scene** is the central hub for the player.
Stages (map levels), Learning Blocks (ropes) and Play Sessions (dots, with larger dots representing assessments) are setup according to the data obtained from the Database.
This is achieved through `Antura.Map.MiniMap.GetAllPlaySessionStateForStage()`.

The **Map scene** allows several actions to be performed through its UI:

- The user may access the **Antura Space scene**.
- The user may access the **Player Book scene**.
- The user may start a new *Play Session* by reaching one of the available (i.e. unlocked) pins on the map and pressing the *play* button.

### Play Session start

When the user selects *play*, the **Antura.Map.MiniMap** method is called, which initialises the new play session by notifying the teacher system through `Teacher.TeacherAI.InitialiseCurrentPlaySession()`, which resets the current play session status and selects the MiniGames to play for that play session (the amount of which is defined by the constant `ConfigAI.numberOfMinigamesPerPlaySession`).
Refer to [Teacher](Teacher.md) for details on MiniGame selection for a given play session.

`** WARNING: the CurrentMiniGameInPlaySession data is now handled by the Teacher and PlayerProfile, but this is bad. Refactor it, then detail it here. **`

Depending on whether the next PlaySession is an Assessment or not, the navigation may change:

- If the next PlaySession is an Assessment, the *Navigation Manager* calls `GoToGameScene(MiniGameData _miniGame)` directly. Refer to the next section.

- If the next PlaySession is MiniGames, the *Navigation Manager* instead accesses the **Game Selector scene**, which is responsible for showing in a playful way what MiniGames were selected by the Teacher System.
The Game Selector scene will first automatically call the method `GamesSelector.AutoLoadMinigames()`, which calls `GamesSelector.Show()` passing the list of currently selected MiniGames.
The method also adds the delegate `GameSelector.GoToMiniGame()` to the `GamesSelector.OnComplete` event, triggered when the user finishes interaction with the Games Selector.
`GameSelector.GoToMiniGame()` will at last signal the *Navigation Manager* to access the first selected MiniGame.

### MiniGame Start

Any call to `NavigationManager.GoToGameScene(MiniGameData _miniGame)` triggers a subsequent call to `MiniGameLauncher.LaunchGame(MiniGameCode miniGameCode)` to launch the next of the MiniGames for that play session.

The **MiniGameLauncher** is responsible for the correct launch of MiniGames with teacher-approved data.
The start of a MiniGame is initialised by a call to `MiniGameLauncher.LaunchGame(MiniGameCode miniGameCode)`.
The launcher then calls `TeacherAI.GetCurrentDifficulty(MiniGameCode miniGameCode)` to obtain the difficulty value for the specific MiniGame session, generates a **GameConfiguration** instance with the correct difficulty settings, and starts the MiniGame through `MiniGameAPI.StartGameMiniGameCode(MiniGameCode _gameCode, GameConfiguration _gameConfiguration)`

The `MiniGameAPI.StartGameMiniGameCode()` method first retrieves the data related to the specified MiniGame from the database for later retrieval and assigns it to `AppManager.CurrentMinigame`.
The process then calls `MiniGameAPI.ConfigureMiniGame` to retrieve the concrete `IGameConfiguration` and **IGameContext** for the given MiniGame code, assigning them to the MiniGame static **IGameConfiguration** concrete implementation.

At this point, the Teacher System is queried to retrieve a set of **QuestionPack** instances that define the learning content that the MiniGame should access and that are accessible through the `IGameConfiguration.Questions` field.
Refer to [Teacher](Teacher.md) and [MiniGame](MiniGame.md) docs for further details on how the learning data is selected and passed to MiniGames.

At last, the MiniGame being correctly configured, it can be started, and the *Navigation Manager* will thus load the scene that matches the specific MiniGame.

### MiniGame Play

MiniGames are responsible for handling their internal state, while the core application waits for the MiniGame to end.
Refer to [MiniGame](MiniGame.md) for details on how the MiniGame flow is implemented.

### MiniGame End

The MiniGame logic is required to call `MiniGame.EndGame()` to end gameplay.

As a MiniGame ends, the end game panel is shown, and after user interaction the game is exited.
_note that the actual flow is_
```
OutcomeGameState.EnterState() ->
MinigamesStarsWidget.Show() ->
GameResultUI.ShowEndgameResult() ->
EndgameResultPanel.Show() ->
EndgameResultPanel.Continue()
```

As a MiniGame ends, the *Navigation Manager* may either:
- start the next MiniGame (refer to the *MiniGame Start* section)
- access the **Play Session Results scene** if the MiniGame was the last for the play session
- or access the **Rewards scene** if an assessment play session was completed

From the Play Session Results scene, or from the Rewards Scene, the player will then return to the Map scene, updating the maximum reached journey position through `NavigationManager.MaxJourneyPositionProgress()` if needed.

## Refactoring notes

- AppManager and InstantiateManager spawn managers in several ways, standardize manager creation and access.
- Many subsystems have their own singleton and should instead be represented in AppManager
