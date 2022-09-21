using Antura.Database;
using Antura.Environment;
using Antura.Keeper;
using Antura.Profile;
using Antura.Rewards;
using Antura.Book;
using Antura.Helpers;
using Antura.Minigames;
using Antura.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Antura.Core
{
    /// <summary>
    /// Controls the navigation among different scenes in the application.
    /// </summary>
    public class NavigationManager : MonoBehaviour
    {
        public static bool TEST_SKIP_GAMES = false;

        public NavigationData NavData;

        public SceneTransitionManager SceneTransitionManager = new SceneTransitionManager();

        private List<KeyValuePair<AppScene, AppScene>> customTransitions = new List<KeyValuePair<AppScene, AppScene>>();
        private List<KeyValuePair<AppScene, AppScene>> backableTransitions = new List<KeyValuePair<AppScene, AppScene>>();

        #region State Checks

        public bool IsLoadingMinigame { get; private set; }

        public bool IsTransitioningScenes
        {
            get { return SceneTransitionManager.IsTransitioning; }
        }

        public int NumberOfLoadedScenes
        {
            get { return SceneTransitionManager.NumberOfLoadedScenes; }
        }

        public bool IsInFirstLoadedScene
        {
            get { return NumberOfLoadedScenes <= 1; }
        }

        public Action OnSceneStartTransition
        {
            get { return SceneTransitionManager.OnSceneStartTransition; }
            set { SceneTransitionManager.OnSceneStartTransition = value; }
        }

        public Action OnSceneEndTransition
        {
            get { return SceneTransitionManager.OnSceneEndTransition; }
            set { SceneTransitionManager.OnSceneEndTransition = value; }
        }

        #endregion

        #region Initialization

        void OnEnable()
        {
            SceneTransitionManager.OnEnable();
        }

        void OnDisable()
        {
            SceneTransitionManager.OnDisable();
        }

        /// <summary>
        /// Initialize the NavigationManager and its data.
        /// </summary>
        public void Init()
        {
            NavData.Setup();
            InitializeAllowedTransitions();
        }

        /// <summary>
        /// Initialize custom and 'back-enabled' transitions.
        /// </summary>
        private void InitializeAllowedTransitions()
        {
            // Allowed custom transitions
            customTransitions.Add(new KeyValuePair<AppScene, AppScene>(AppScene.Bootstrap, AppScene.Home));
            customTransitions.Add(new KeyValuePair<AppScene, AppScene>(AppScene.Home, AppScene.PlayerCreation));
            customTransitions.Add(new KeyValuePair<AppScene, AppScene>(AppScene.Home, AppScene.ReservedArea));
            customTransitions.Add(new KeyValuePair<AppScene, AppScene>(AppScene.Map, AppScene.AnturaSpace));
            customTransitions.Add(new KeyValuePair<AppScene, AppScene>(AppScene.Map, AppScene.Rewards));
            customTransitions.Add(new KeyValuePair<AppScene, AppScene>(AppScene.Map, AppScene.MiniGame));
            customTransitions.Add(new KeyValuePair<AppScene, AppScene>(AppScene.Kiosk, AppScene.MiniGame));
            customTransitions.Add(new KeyValuePair<AppScene, AppScene>(AppScene.Kiosk, AppScene.Kiosk));
            customTransitions.Add(new KeyValuePair<AppScene, AppScene>(AppScene.MiniGame, AppScene.Kiosk));
            customTransitions.Add(new KeyValuePair<AppScene, AppScene>(AppScene.Rewards, AppScene.AnturaSpace));

            // Transitions that can register for a 'back' function
            backableTransitions.Add(new KeyValuePair<AppScene, AppScene>(AppScene.Home, AppScene.ReservedArea));
            backableTransitions.Add(new KeyValuePair<AppScene, AppScene>(AppScene.Map, AppScene.AnturaSpace));
            backableTransitions.Add(new KeyValuePair<AppScene, AppScene>(AppScene.Map, AppScene.GameSelector));
            backableTransitions.Add(new KeyValuePair<AppScene, AppScene>(AppScene.Map, AppScene.MiniGame));
            backableTransitions.Add(new KeyValuePair<AppScene, AppScene>(AppScene.Kiosk, AppScene.MiniGame));
        }

        /// <summary>
        /// Sets the player navigation data.
        /// </summary>
        /// <param name="_playerProfile">The player profile.</param>
        public void InitPlayerNavigationData(PlayerProfile _playerProfile)
        {
            NavData.Init(_playerProfile);
        }

        #endregion

        #region Automatic navigation API

        public AppScene GetCurrentScene()
        {
            return NavData.CurrentScene;
        }

        /// <summary>
        /// Given the current context, selects the scene that should be loaded next and loads it.
        /// This is related to the 'main' flow of the application.
        /// For 'custom' flows, refer to the custom route methods below.
        /// </summary>
        public void GoToNextScene()
        {
            if (DebugConfig.I.DebugLogEnabled)
            {
                Debug.LogFormat(" ---- NAV MANAGER ({1}) scene {0} ---- ", NavData.CurrentScene, "GoToNextScene");
            }
            switch (NavData.CurrentScene)
            {
                case AppScene.Home:
                    GoToScene(AppScene.Map);
                    break;
                case AppScene.PlayerCreation:
                    GoToScene(AppScene.Intro);
                    break;
                case AppScene.Mood:
                    GoToScene(AppScene.DailyReward);
                    break;
                case AppScene.DailyReward:
                    GoToScene(AppScene.Map);
                    break;
                case AppScene.Map:
                    GoToPlaySession();
                    break;
                case AppScene.Book:
                    break;
                case AppScene.Intro:
                    GoToScene(AppScene.Map);
                    break;
                case AppScene.GameSelector:
                    GoToFirstGameOfPlaySession();
                    break;
                case AppScene.MiniGame:
                    if (AppManager.I.AppSettings.KioskMode)
                    {
                        GoToKiosk();
                    }
                    else
                    {
                        GoToNextGameOfPlaySession();
                    }
                    break;
                case AppScene.AnturaSpace:
                    GoToScene(AppScene.Map);
                    break;
                case AppScene.Rewards:
                    GoToScene(AppScene.AnturaSpace);
                    break;
                case AppScene.PlaySessionResult:
                    GoToScene(AppScene.Map);
                    break;
                case AppScene.Ending:
                    GoToScene(AppScene.AnturaSpace);
                    break;
            }
        }

        private bool CheckDailySceneTrigger()
        {
            bool mustShowDailyScenes = false;
            int numberOfDaysSinceLastReward = AppManager.I.Teacher.logAI.DaysSinceLastDailyReward();

            if (numberOfDaysSinceLastReward <= 0)
            {
                mustShowDailyScenes = false;
            }
            else if (numberOfDaysSinceLastReward > 1)
            {
                // Number of days since last reward is more than 1. Show daily, but zero combo.
                mustShowDailyScenes = true;
                NavData.CurrentPlayer.ConsecutivePlayDays = 1;
            }
            else
            {
                // Number of days since last reward is exactly 1. Good for combo!
                mustShowDailyScenes = true;
                NavData.CurrentPlayer.ConsecutivePlayDays += 1;
            }
            //Debug.Log("numberOfDaysSinceLastReward: " + numberOfDaysSinceLastReward);
            //Debug.Log("ComboPlayDays: " + numberOfDaysSinceLastReward);

            return mustShowDailyScenes;
        }

        /// <summary>
        /// Apply logic for back button in current scene.
        /// </summary>
        public void GoBack()
        {
            /*
            Debug.LogError("HITTING BACK FROM " + NavData.CurrentScene);
            for (int i = 0; i < NavData.PrevSceneStack.Count; i++) {
                Debug.LogError(i + ": " + NavData.PrevSceneStack.ToArray()[i]);
            }
            */

            if (NavData.PrevSceneStack.Count > 0)
            {
                var prevScene = NavData.PrevSceneStack.Pop();
                if (DebugConfig.I.DebugLogEnabled)
                {
                    Debug.LogFormat(" ---- NAV MANAGER ({0}) from scene {1} to {2} ---- ", "GoBack", NavData.CurrentScene, prevScene);
                }
                GoToScene(prevScene);
            }
        }

        public void ReloadScene()
        {
            GoToScene(GetCurrentScene());
        }

        #endregion

        #region Direct navigation (private)

        private void GoToScene(AppScene wantedNewScene, MiniGameData minigameData = null, bool debugMode = false)
        {
            AppScene filteredNewScene = wantedNewScene;
            if (!debugMode)
            {
                bool keepPrevAsBackable = false;
                filteredNewScene = FirstContactManager.I.FilterNavigation(GetCurrentScene(), wantedNewScene, out keepPrevAsBackable);
                if (keepPrevAsBackable)
                {
                    UpdatePrevSceneStack(wantedNewScene);
                }

                if (FirstContactManager.I.IsSequenceFinished())
                {
                    // Additional general checks when entering specific scenes
                    switch (filteredNewScene)
                    {
                        case AppScene.Map:
                            // When coming back to the map, we need to check whether a new daily reward is needed
                            if (CheckDailySceneTrigger())
                            {
                                GoToScene(AppScene.Mood);
                                return;
                            }
                            break;
                    }
                }
            }

            // Scene switch
            UpdatePrevSceneStack(filteredNewScene);
            NavData.CurrentScene = filteredNewScene;

            // check to have closed any possible Keeper Dialog
            KeeperManager.I.ResetKeeper();

            GoToSceneByName(SceneHelper.GetSceneName(filteredNewScene, minigameData));
        }

        private void GoToSceneByName(string sceneName)
        {
            IsLoadingMinigame = sceneName.Substring(0, 5) == "game_";

            if (DebugConfig.I.DebugLogEnabled)
            { Debug.LogFormat(" ==== Loading scene {0} ====", sceneName); }
            SceneTransitionManager.LoadSceneWithTransition(sceneName);

            AppManager.I.Services.Analytics.TrackScene(sceneName);
            LogManager.I.LogInfo(InfoEvent.EnterScene, "{\"Scene\":\"" + sceneName + "\"}");
        }

        private void UpdatePrevSceneStack(AppScene newScene)
        {
            // The stack is updated only for some transitions
            if (backableTransitions.Contains(new KeyValuePair<AppScene, AppScene>(NavData.CurrentScene, newScene)))
            {
                if (NavData.PrevSceneStack.Count == 0 || NavData.PrevSceneStack.Peek() != NavData.CurrentScene)
                {
                    if (DebugConfig.I.DebugLogEnabled)
                    {
                        Debug.Log("Added BACKABLE transition " + NavData.CurrentScene + " to " + newScene);
                    }
                    NavData.PrevSceneStack.Push(NavData.CurrentScene);
                }
            }
        }

        /// <summary>
        /// Launches the game scene.
        /// </summary>
        /// <param name="_miniGame">The mini game.</param>
        private void InternalLaunchGameScene(MiniGameData _miniGame, MinigameLaunchConfiguration _launchConfig = null, bool useLastConfig = false)
        {
            WorldManager.I.CurrentWorld = (WorldID)(NavData.CurrentPlayer.CurrentJourneyPosition.Stage - 1);

            // Ask the teacher for a config, if needed
            if (useLastConfig)
            {
                _launchConfig = AppManager.I.GameLauncher.LastLaunchConfig;
            }
            else if (_launchConfig == null)
            {
                var teacher = AppManager.I.Teacher;
                var difficulty = teacher.GetCurrentDifficulty(_miniGame.Code);
                var numberOfRounds = teacher.GetCurrentNumberOfRounds(_miniGame.Code);
                var tutorialEnabled = teacher.GetTutorialEnabled(_miniGame.Code);
                _launchConfig = new MinigameLaunchConfiguration(difficulty, numberOfRounds, tutorialEnabled, insideJourney: true);
            }
            AppManager.I.GameLauncher.LaunchGame(_miniGame.Code, _launchConfig);
        }

        #endregion

        #region Custom routes

        public void GoToHome(bool debugMode = false)
        {
            if (AppManager.I.AppSettings.KioskMode)
            {
                AppManager.I.AppSettingsManager.SetKioskMode(false);
            }
            CustomGoTo(AppScene.Home, debugMode);
        }

        public void GoToMap(bool debugMode = false)
        {
            CustomGoTo(AppScene.Map, debugMode);
        }

        public void GoToEnding(bool debugMode = false)
        {
            CustomGoTo(AppScene.Ending, debugMode);
        }

        public void GoToPlayerCreation(bool debugMode = false)
        {
            CustomGoTo(AppScene.PlayerCreation, debugMode);
        }

        public void GoToReservedArea(bool debugMode = false)
        {
            CustomGoTo(AppScene.ReservedArea, debugMode);
        }

        public void GoToAnturaSpace(bool debugMode = false)
        {
            CustomGoTo(AppScene.AnturaSpace, debugMode);
        }

        public void GoToKiosk(bool debugMode = false)
        {
            Debug.Log("GoToKiosk");
            AppManager.I.AppSettingsManager.SetKioskMode(true);
            CustomGoTo(AppScene.Kiosk, debugMode);
        }

        /// <summary>
        /// Exit from the current scene. Called while in pause mode during a minigame.
        /// </summary>
        public void ExitToMainMenu()
        {
            Debug.LogFormat(" ---- NAV MANAGER ({1}) scene {0} ---- ", NavData.CurrentScene, "ExitDuringPause");
            switch (NavData.CurrentScene)
            {
                case AppScene.Map:
                    UIDirector.DeactivateAllUI();
                    KeeperManager.I.PlayDialogue(new[]
                        {
                            LocalizationDataId.Map_Exit_1,
                            LocalizationDataId.Map_Exit_2,
                            LocalizationDataId.Map_Exit_3,
                        }.GetRandom(),
                        _callback: () =>
                        {
                            // We also clear the navigation data
                            NavData.PrevSceneStack.Clear();
                            GoToScene(AppScene.Home);
                        });
                    break;
                case AppScene.PlayerCreation:
                    // We also clear the navigation data
                    NavData.PrevSceneStack.Clear();
                    GoToScene(AppScene.Home);
                    break;
                default:
                    if (AppManager.I.AppSettings.KioskMode)
                    {
                        GoToKiosk();
                    }
                    else
                    {
                        GoToScene(AppScene.Map);
                    }
                    break;
            }
        }

        /// <summary>
        /// Internal GoTo to handle custom transitions.
        /// </summary>
        private void CustomGoTo(AppScene targetScene, bool debugMode = false)
        {
            if (debugMode || HasCustomTransitionTo(targetScene))
            {
                Debug.LogFormat(" ---- NAV MANAGER ({0}) scene {1} to {2} ---- ", "CustomGoTo", NavData.CurrentScene, targetScene);
                GoToScene(targetScene, debugMode: debugMode);
            }
            else
            {
                throw new Exception("Cannot go to " + targetScene + " from " + NavData.CurrentScene);
            }
        }

        private bool HasCustomTransitionTo(AppScene targetScene)
        {
            return customTransitions.Contains(new KeyValuePair<AppScene, AppScene>(NavData.CurrentScene, targetScene));
        }

        /// <summary>
        /// Special GoTo for minigames.
        /// </summary>
        public void GoToMiniGameScene()
        {
            bool canTravel;

            switch (NavData.CurrentScene)
            {
                // Normal flow
                case AppScene.MiniGame:
                case AppScene.GameSelector:
                case AppScene.Map:
                    canTravel = true;
                    break;

                // "Fake minigame" flow
                default:
                    canTravel = !NavData.IsJourneyPlaySession;
                    break;
            }

            if (canTravel)
            {
                GoToScene(AppScene.MiniGame, NavData.CurrentMiniGameData);
            }
            else
            {
                throw new Exception("Cannot go to a minigame from the current scene!");
            }
        }

        public bool PrevSceneIsReservedArea()
        {
            if (NavData.PrevSceneStack != null && NavData.PrevSceneStack.Count > 0)
            {
                return NavData.PrevSceneStack.Peek() == AppScene.ReservedArea;
            }
            else
            {
                return false;
            }
        }

        #endregion

        // TODO refactor: move these a more coherent manager, which handles the state of a play session between minigames

        #region temp for demo

        List<EndsessionResultData> EndSessionResults = new List<EndsessionResultData>();

        /// <summary>
        /// Called to notify end minigame with result (pushed continue button on UI).
        /// </summary>
        /// <param name="_stars">The stars.</param>
        public void EndMinigame(int _stars)
        {
            if (NavData.CurrentMiniGameData == null)
            {
                return;
            }
            var res = new EndsessionResultData(_stars, NavData.CurrentMiniGameData);
            EndSessionResults.Add(res);
        }

        /// <summary>
        /// Uses the end session results and reset it.
        /// </summary>
        /// <returns></returns>
        public List<EndsessionResultData> UseEndSessionResults()
        {
            var returnResult = EndSessionResults;
            EndSessionResults = new List<EndsessionResultData>();
            return returnResult;
        }

        /// <summary>
        /// Resets the end session results.
        /// </summary>
        public void ResetEndSessionResults()
        {
            EndSessionResults = new List<EndsessionResultData>();
        }

        /// <summary>
        /// Calculates the unlock item count in accord to gameplay result information.
        /// </summary>
        /// <returns></returns>
        public int CalculateRewardPacksUnlockCount()
        {
            // decrement because the number of stars needed to unlock the first reward is 2.
            return CalculateEarnedStarsCount() - 1;
        }

        /// <summary>
        /// Calculates earned stars in accord to gameplay result information.
        /// </summary>
        /// <returns></returns>
        public int CalculateEarnedStarsCount()
        {
            int totalEarnedStars = 0;
            for (int i = 0; i < EndSessionResults.Count; i++)
            {
                totalEarnedStars += EndSessionResults[i].Stars;
            }
            int earnedStarsCount = 0;
            if (EndSessionResults.Count > 0)
            {
                float starRatio = totalEarnedStars / EndSessionResults.Count;
                // Prevent aproximation errors (0.99f must be == 1 but 0.7f must be == 0)
                earnedStarsCount = starRatio - Mathf.CeilToInt(starRatio) < 0.0001f
                    ? Mathf.CeilToInt(starRatio)
                    : Mathf.RoundToInt(starRatio);
            }
            return earnedStarsCount;
        }

        #endregion


        #region Minigame Launching

        public MiniGameData CurrentMiniGameData
        {
            get { return NavData.CurrentMiniGameData; }
        }

        public List<MiniGameData> CurrentPlaySessionMiniGames
        {
            get { return NavData.CurrentPlaySessionMiniGames; }
        }

        public void InitNewPlaySession(bool directMiniGame = false, MiniGameData dataToUse = null)
        {
            ResetEndSessionResults();
            NavData.IsJourneyPlaySession = !directMiniGame;
            NavData.DirectMiniGameData = dataToUse;

            AppManager.I.Teacher.InitNewPlaySession();
            NavData.SetFirstMinigame();

            if (NavData.IsJourneyPlaySession)
            {
                NavData.CurrentPlaySessionMiniGames = AppManager.I.Teacher.SelectMiniGames();
            }
            else
            {
                NavData.CurrentPlaySessionMiniGames = new List<MiniGameData>();
                NavData.CurrentPlaySessionMiniGames.Add(dataToUse);
            }
        }

        private void GoToPlaySession()
        {
            // This must be called before any play session is started
            InitNewPlaySession();
            LogManager.I.StartPlaySession();

            // From the map
            var jp = NavData.CurrentPlayer.CurrentJourneyPosition;
            if (jp.IsEndGame())
            {
                // Go to the end scene directly
                GoToScene(AppScene.Ending);
            }
            else if (jp.IsAssessment())
            {
                // Direct to the current minigame (which is an assessment)
                InternalLaunchGameScene(NavData.CurrentMiniGameData);
            }
            else
            {
                // Show the games selector
                GoToScene(AppScene.GameSelector);
            }
        }

        private void GoToFirstGameOfPlaySession()
        {
            if (TEST_SKIP_GAMES)
            {
                LogManager.I.EndMiniGame();
                GoToScene(AppScene.PlaySessionResult);
                return;
            }

            // Game selector -> go to the first game
            NavData.SetFirstMinigame();
            InternalLaunchGameScene(NavData.CurrentMiniGameData);
        }

        private void GoToNextGameOfPlaySession()
        {
            if (!NavData.IsJourneyPlaySession)
            {
                //Debug.Log("Finished non-journey MiniGame");
                GoBack();
                return;
            }

            // From one game to the next
            var jp = NavData.CurrentPlayer.CurrentJourneyPosition;
            if (jp.IsAssessment())
            {
                if (AppManager.I.JourneyHelper.PlayerIsAtFinalJourneyPosition())
                {
                    // We finished the whole game: unlock the end scene
                    if (!AppManager.I.Player.HasFinalBeenShown())
                    {
                        AppManager.I.Player.SetGameCompleted();
                    }
                }

                // We finished an assessment.
                bool shouldUnlockReward = NavData.CurrentPlayer.CurrentJourneyPosition.Equals(NavData.CurrentPlayer.MaxJourneyPosition);
                if (shouldUnlockReward)
                {
                    GoToScene(AppScene.Rewards);
                }
                else
                {
                    GoToScene(AppScene.Map);
                }
            }
            else
            {
                // Not an assessment. Do we have any more?
                if (NavData.SetNextMinigame())
                {
                    // Go to the next minigame.
                    InternalLaunchGameScene(NavData.CurrentMiniGameData);
                }
                else
                {
                    // Go to the reward scene.
                    GoToScene(AppScene.PlaySessionResult);
                }
            }
        }

        public void RepeatCurrentGameOfPlaySession()
        {
            if (GetCurrentScene() != AppScene.MiniGame)
                return;

            // Just repeat the minigame as if nothing happened.
            InternalLaunchGameScene(NavData.CurrentMiniGameData, useLastConfig: true);
        }

        #endregion
    }
}
