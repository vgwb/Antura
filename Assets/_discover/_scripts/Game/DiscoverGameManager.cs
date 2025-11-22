using System.Collections;
using Antura.Utilities;
using UnityEngine;

namespace Antura.Discover
{
    public enum GameplayState
    {
        None, // no state defined
        Changing, // here if the state is currently changing
        Setup, // setup here we set things useful for gameplay
        Intro, // intro state
        Play3D, // here we play in the world
        PlayActivity, // here we play a 2D activity
        Map, // here if we're in the world map
        Dialogue, // all dialogues magic dialogs are here
        End, // end the game, show results
    }

    public class DiscoverGameManager : SingletonMonoBehaviour<DiscoverGameManager>
    {
        [Header("Quest")]
        [Tooltip("Quest to load at startup (optional)")]
        public QuestData StartingQuest;

        [Header("Managers (optional)")]
        [SerializeField] private QuestManager questManager;
        [SerializeField] private YarnAnturaManager yarnAnturaManager;
        [SerializeField] private ActionManager actionManager;

        [Header("ReadOnly")]
        public GameplayState State { get; private set; }
        /// <summary>Last play state (used to know to which play state to return after a dialogue/etc. state)</summary>
        public GameplayState LastPlayState { get; private set; }
        // when we pause the game we use this global var
        public bool isPaused;

        Coroutine coChangeState;

        // sets stars when the game ends. hack we need to check state
        private int GameEndStars = -1;

        void OnEnable()
        {
            // Hook Yarn dialogue complete to resume gameplay
            var yarn = yarnAnturaManager != null ? yarnAnturaManager : FindFirstObjectByType<YarnAnturaManager>(FindObjectsInactive.Include);
            if (yarn != null)
            {
                yarn.OnDialogueStart += OnYarnDialogueStart;
                yarn.OnDialogueComplete += OnYarnDialogueComplete;
            }
        }

        void OnDisable()
        {
            var yarn = yarnAnturaManager != null ? yarnAnturaManager : FindFirstObjectByType<YarnAnturaManager>(FindObjectsInactive.Include);
            if (yarn != null)
            {
                yarn.OnDialogueStart -= OnYarnDialogueStart;
                yarn.OnDialogueComplete -= OnYarnDialogueComplete;
            }
        }

        IEnumerator Start()
        {
            // Enter Setup state first to let managers initialize (ActionManager, QuestManager, etc.)
            ChangeState(GameplayState.Setup, true);

            // Let one frame pass for Start() of managers and task registration
            yield return null;


            if (!questManager)
                questManager = FindFirstObjectByType<QuestManager>(FindObjectsInactive.Include);
            if (!actionManager)
                actionManager = FindFirstObjectByType<ActionManager>(FindObjectsInactive.Include);
            if (!yarnAnturaManager)
                yarnAnturaManager = FindFirstObjectByType<YarnAnturaManager>(FindObjectsInactive.Include);

            // Determine quest to use: prefer prefab-assigned QuestManager.CurrentQuest, otherwise StartingQuest
            var questToUse = questManager.CurrentQuest != null ? questManager.CurrentQuest : StartingQuest;
            if (questToUse != null)
            {
                questManager.CurrentQuest = questToUse;
                if (yarnAnturaManager != null)
                {
                    // Initialize DialogueRunner with project and string table
                    if (yarnAnturaManager.Runner != null)
                    {
                        yarnAnturaManager.Runner.SetProject(questToUse.YarnProject);
                        yarnAnturaManager.LineProvider.SetStringTable(questToUse.QuestStringsTable);
                        yarnAnturaManager.LineProvider.SetAssetTable(questToUse.QuestAssetsTable);
                    }
                }
            }

            // Initialize intro: set Dialogue state and start Yarn init via QuestManager
            //ChangeState(GameplayState.Intro, true);
            if (questManager != null)
            {
                questManager.QuestStart();
            }
        }

        void OnDestroy()
        {
            this.StopAllCoroutines();
        }

        /// <summary>Changing state takes one frame unless forced to be immediate</summary>
        public void ChangeState(GameplayState newState, bool immediate = true)
        {
            if (newState == State)
                return;

            this.RestartCoroutine(ref coChangeState, CO_ChangeState(newState, immediate));
        }

        public void ChangeToPreviousState()
        {
            this.RestartCoroutine(ref coChangeState, CO_ChangeState(LastPlayState, true));
        }

        IEnumerator CO_ChangeState(GameplayState newState, bool immediate)
        {
            LastPlayState = State;
            if (!immediate)
            {
                State = GameplayState.Changing;
                yield return null;
            }

            // // Store last play state
            // bool newIsPlay = newState == GameplayState.Play3D || newState == GameplayState.PlayActivity;
            // bool currIsPlay = State == GameplayState.Play3D || State == GameplayState.PlayActivity;
            // if (newIsPlay)
            // {
            //     LastPlayState = newState;
            // }
            // else if (currIsPlay)
            // {
            //     LastPlayState = State;
            // }

            State = newState;
            Debug.Log("<color=#d8249c>Changed state to " + State + "</color>");
        }

        void OnYarnDialogueStart()
        {
            ChangeState(GameplayState.Dialogue, true);
        }

        void OnYarnDialogueComplete()
        {
            Debug.Log("-- discoverGameManager - OnYarnDialogueComplete in state " + State);
            // After intro/dialogue completes, return to 3D play only if we're handling the intro
            if (State == GameplayState.PlayActivity)
            {

            }
            else
            {
                ChangeState(GameplayState.Play3D, true);
            }

            QuestManager.I.TaskManager.CheckAndRunEndTaskNode();

            if (GameEndStars >= 0)
            {
                Debug.Log("QUIT GAME");
                DiscoverAppManager.I.GoToQuestMenu();
            }
        }

        public void GameEnd(QuestEnd questResult, int currentCookies)
        {
            Debug.Log("Setting GamePlayState to End with result " + questResult.stars);
            DiscoverAppManager.I.RecordQuestEnd(questResult, currentCookies);
            GameEndStars = questResult.stars;
            ChangeState(GameplayState.End, true);
        }
    }
}
