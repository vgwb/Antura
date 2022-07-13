using Antura.Core;
using Antura.Database;
using Antura.Minigames;
using Antura.Profile;
using Antura.Rewards;
using Antura.Teacher;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Antura.Debugging
{
    /// <summary>
    /// General manager for debug purposes.
    /// Allows debugging via forcing the value of some parameters of the application.
    /// </summary>
    public class DebugManager : MonoBehaviour
    {
        public static DebugManager I;

        public bool DebugPanelEnabled;
        public bool DebugPanelOpened;

        public delegate void OnSkipCurrentSceneDelegate();

        public static event OnSkipCurrentSceneDelegate OnSkipCurrentScene;

        public delegate void OnForceCurrentMinigameEndDelegate(int value);

        public static event OnForceCurrentMinigameEndDelegate OnForceCurrentMinigameEnd;

        private GameObject debugPanelGO;

        #region Launch Options

        public int Stage = 1;
        public int LearningBlock = 1;
        public int PlaySession = 1;

        public void SetDebugJourneyPos(JourneyPosition jp)
        {
            Stage = jp.Stage;
            LearningBlock = jp.LearningBlock;
            PlaySession = jp.PlaySession;
        }

        public float Difficulty = 0.5f;
        public int NumberOfRounds = 1;

        #endregion

        #region App Options

        public bool VerboseTeacher
        {
            get { return DebugConfig.I.VerboseTeacher; }
            set { DebugConfig.I.VerboseTeacher = value; }
        }

        /// <summary>
        /// Stops a MiniGame from playing if the PlaySession database does not allow that MiniGame to be played at a given position.
        /// </summary>
        public bool SafeLaunch = true;

        /// <summary>
        /// If SafeLaunch is on, the DebugManager will correct the journey position so the minimum JP is selected
        /// </summary>
        public bool AutoCorrectJourneyPos = true;

        private bool _ignoreJourneyData = false;

        public bool IgnoreJourneyData
        {
            get { return _ignoreJourneyData; }
            set
            {
                _ignoreJourneyData = value;
                Teacher.ConfigAI.ForceJourneyIgnore = _ignoreJourneyData;
            }
        }

        #endregion

        /// <summary>
        /// Gets or sets a value indicating whether [first contact completed].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [first contact completed]; otherwise, <c>false</c>.
        /// </value>
        public bool FirstContactCompleted
        {
            get { return FirstContactManager.I.IsSequenceFinished(); }
            set
            {
                if (value)
                {
                    FirstContactManager.I.ForceToFinishedSequence();
                }
                else
                {
                    FirstContactManager.I.ForceToStartOfSequence();
                }
            }
        }

        #region Unity events

        void Awake()
        {
            I = this;

            if (DebugConfig.I.DebugPanelEnabledAtStartup)
            {
                EnableDebugPanel();
            }
        }

        void Update()
        {
            if (!DebugPanelOpened)
            {
                // RESERVED AREA
                if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.R))
                {
                    AppManager.I.NavigationManager.GoToReservedArea();
                }

                // ADD BONES
                if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.B))
                {
                    AddBones();
                }

                // SKIPS
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("DEBUG - SPACE : skip");
                    if (OnSkipCurrentScene != null)
                        OnSkipCurrentScene();
                }

                // END MINIGAMES
                if (Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.Alpha0))
                {
                    ForceCurrentMinigameEnd(0);
                }

                if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
                {
                    ForceCurrentMinigameEnd(1);
                }

                if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
                {
                    ForceCurrentMinigameEnd(2);
                }

                if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
                {
                    ForceCurrentMinigameEnd(3);
                }

                /// VARIOUS TESTS
                if (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.T))
                {
                    AppManager.I.Services.Notifications.TestLocalNotification();
                }

            }
        }

        #endregion

        #region Actions

        public void CreateTestProfile()
        {
            AppManager.I.PlayerProfileManager.CreatePlayerProfile(true, 1, PlayerGender.M, PlayerTint.Orange, Color.yellow, Color.red, Color.magenta, 4,
                AppManager.I.AppEdition.editionID,
                AppManager.I.ContentEdition.ContentID,
                AppManager.I.AppEdition.AppVersion);
            AppManager.I.NavigationManager.GoToHome(debugMode: true);
        }

        public void CreateOldProfile()
        {
            AppManager.I.PlayerProfileManager.CreatePlayerProfile(false, 1, PlayerGender.F, PlayerTint.Green, Color.yellow, Color.red, Color.magenta, 4,
                AppManager.I.AppEdition.editionID,
                AppManager.I.ContentEdition.ContentID,
                AppManager.I.AppEdition.AppVersion);
            AppManager.I.NavigationManager.GoToHome(debugMode: true);
        }

        public void AddBones()
        {
            AppManager.I.Player.AddBones(50);
        }

        public void ForceCurrentMinigameEnd(int stars)
        {
            if (OnForceCurrentMinigameEnd != null)
            {
                Debug.Log("DEBUG - Force Current Minigame End with stars: " + stars);
                OnForceCurrentMinigameEnd(stars);
            }
        }

        public void EnableDebugPanel()
        {
            DebugPanelEnabled = true;
            if (debugPanelGO == null)
            {
                debugPanelGO = Instantiate(Resources.Load(AppConfig.RESOURCES_PATH_DEBUG_PANEL) as GameObject);
            }
        }

        public void LaunchMiniGame(MiniGameCode miniGameCodeSelected, float difficulty)
        {
            AppManager.I.Player.CurrentJourneyPosition.SetPosition(Stage, LearningBlock, PlaySession);

            Difficulty = difficulty;

            Debug.Log("LaunchMiniGame " + miniGameCodeSelected + " PS: " + AppManager.I.Player.CurrentJourneyPosition + " Diff: " +
                      Difficulty + " Tutorial: " + AppConfig.MinigameTutorialsEnabled);

            var config = new MinigameLaunchConfiguration(Difficulty, NumberOfRounds,
                tutorialEnabled: AppConfig.MinigameTutorialsEnabled, directGame: true);
            AppManager.I.GameLauncher.LaunchGame(miniGameCodeSelected, config);
        }

        public void ResetAll(bool clearOnly)
        {
            AppManager.I.PlayerProfileManager.ResetEverything(clearOnly);
            PlayerPrefs.DeleteAll();
            if (!clearOnly)
                AppManager.I.NavigationManager.GoToHome(debugMode: true);
            Debug.Log("Reset ALL players, DBs, and PlayerPrefs.");
        }

#if UNITY_EDITOR

        [MenuItem("Antura/Utility/Delete Profiles")]
        public static void ResetAllCommand()
        {
            DebugManager.I.ResetAll(true);
        }

#endif

        #endregion

        #region Navigation

        public void GoToHome()
        {
            AppManager.I.NavigationManager.GoToHome(debugMode: true);
        }

        public void GoToMap()
        {
            AppManager.I.NavigationManager.GoToMap(debugMode: true);
        }

        public void GoToNext()
        {
            AppManager.I.NavigationManager.GoToNextScene();
        }

        public void GoToEnd()
        {
            AppManager.I.NavigationManager.GoToEnding(debugMode: true);
        }

        public void GoToReservedArea()
        {
            AppManager.I.NavigationManager.GoToReservedArea(debugMode: true);
        }

        public void GoToKiosk()
        {
            AppManager.I.NavigationManager.GoToKiosk(debugMode: true);
        }

        public void ForwardMaxJourneyPos()
        {
            JourneyPosition newPos = AppManager.I.JourneyHelper.FindNextJourneyPosition(AppManager.I.Player.MaxJourneyPosition);
            if (newPos != null)
            {
                AppManager.I.Player.SetMaxJourneyPosition(newPos, true);
            }
            GoToMap();
        }

        public void ForceMaxJourneyPos()
        {
            AppManager.I.Player.SetMaxJourneyPosition(new JourneyPosition(Stage, LearningBlock, PlaySession), true, true);
            AppManager.I.Player.UpdatePreviousJourneyPosition();    // Antura is considered as having been there the whole time

            // Unlock everything up to there
            foreach (var jp in AppManager.I.JourneyHelper.GetAllJourneyPositionsUpTo(new JourneyPosition(Stage, LearningBlock, PlaySession)))
            {
                AppManager.I.RewardSystemManager.UnlockAllRewardPacksForJourneyPosition(jp);
            }

            GoToMap();
        }

        public void SecondToLastJourneyPos()
        {
            JourneyPosition newPos = AppManager.I.JourneyHelper.GetFinalJourneyPosition();
            newPos.PlaySession = 1;
            if (newPos != null)
            {
                AppManager.I.Player.SetMaxJourneyPosition(newPos, true);
                AppManager.I.Player.UpdatePreviousJourneyPosition();     // Antura is considered as having been there the whole time
                FirstContactCompleted = true;
            }
            GoToMap();
        }

        public void ResetMaxJourneyPos()
        {
            AppManager.I.Player.ResetMaxJourneyPosition();
            GoToMap();
        }

        #endregion

        #region Rewards

        public void UnlockAll()
        {
            AppManager.I.Player.SetMaxJourneyPosition(new JourneyPosition(6, 15, 100), true);
        }

        public void UnlockFirstReward()
        {
            AppManager.I.RewardSystemManager.UnlockFirstSetOfRewards();
        }

        public void UnlockCurrentPlaySessionRewards()
        {
            var unlockedPacks = AppManager.I.RewardSystemManager.UnlockAllRewardPacksForJourneyPosition(AppManager.I.Player.CurrentJourneyPosition);
            foreach (RewardPack unlockedPack in unlockedPacks)
            {
                Debug.LogFormat("Pack unlocked: {0}", unlockedPack.ToString());
            }
        }

        public void UnlockAllRewards()
        {
            AppManager.I.RewardSystemManager.UnlockAllPacks();
        }

        #endregion
    }
}
