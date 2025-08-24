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
        [SerializeField] private QuestTaskManager taskManager;
        [SerializeField] private YarnAnturaManager yarnAnturaManager;
        [SerializeField] private YarnConversationController yarnConversationController;
        [SerializeField] private ActionManager actionManager;

        [Header("Readonly")]
        public GameplayState State { get; private set; }
        /// <summary>Last play state (used to know to which play state to return after a dialogue/etc. state)</summary>
        public GameplayState LastPlayState { get; private set; }
        // when we pause the game we use this global var
        public bool isPaused;
        private bool isIntroRunning;

        Coroutine coChangeState;

        void OnEnable()
        {
            // Hook Yarn dialogue complete to resume gameplay
            var yarn = yarnAnturaManager != null ? yarnAnturaManager : FindFirstObjectByType<YarnAnturaManager>(FindObjectsInactive.Include);
            if (yarn != null)
            {
                yarn.OnDialogueComplete += OnYarnDialogueComplete;
            }
        }

        void OnDisable()
        {
            var yarn = yarnAnturaManager != null ? yarnAnturaManager : FindFirstObjectByType<YarnAnturaManager>(FindObjectsInactive.Include);
            if (yarn != null)
            {
                yarn.OnDialogueComplete -= OnYarnDialogueComplete;
            }
        }

        IEnumerator Start()
        {
            State = GameplayState.None;

            if (!questManager)
                questManager = FindFirstObjectByType<QuestManager>(FindObjectsInactive.Include);
            if (!actionManager)
                actionManager = FindFirstObjectByType<ActionManager>(FindObjectsInactive.Include);
            if (!taskManager)
                taskManager = FindFirstObjectByType<QuestTaskManager>(FindObjectsInactive.Include);
            if (!yarnAnturaManager)
                yarnAnturaManager = FindFirstObjectByType<YarnAnturaManager>(FindObjectsInactive.Include);
            if (!yarnConversationController)
                yarnConversationController = FindFirstObjectByType<YarnConversationController>(FindObjectsInactive.Include);

            // Determine quest to use: prefer prefab-assigned QuestManager.CurrentQuest, otherwise StartingQuest
            if (questManager != null)
            {
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
                        }
                    }
                }
            }

            // Enter Setup state first to let managers initialize (ActionManager, QuestManager, etc.)
            ChangeState(GameplayState.Setup, true);

            // Let one frame pass for Start() of managers and task registration
            yield return null;

            // Initialize intro: set Dialogue state and start Yarn init via QuestManager
            ChangeState(GameplayState.Intro, true);
            isIntroRunning = true;
            if (questManager != null)
            {
                questManager.StartDialogue("init");
            }
        }

        void OnDestroy()
        {
            this.StopAllCoroutines();
        }

        /// <summary>Changing state takes one frame unless forced to be immediate</summary>
        public void ChangeState(GameplayState newState, bool immediate = false)
        {
            if (newState == State)
                return;

            this.RestartCoroutine(ref coChangeState, CO_ChangeState(newState, immediate));
        }
        IEnumerator CO_ChangeState(GameplayState newState, bool immediate)
        {
            if (!immediate)
            {
                State = GameplayState.Changing;
                yield return null;
            }

            // Store last play state
            bool newIsPlay = newState == GameplayState.Play3D || newState == GameplayState.PlayActivity;
            bool currIsPlay = State == GameplayState.Play3D || State == GameplayState.PlayActivity;
            if (newIsPlay)
            {
                LastPlayState = newState;
            }
            else if (currIsPlay)
            {
                LastPlayState = State;
            }

            State = newState;
        }

        void OnYarnDialogueComplete()
        {
            // After intro/dialogue completes, return to 3D play only if we're handling the intro
            if (isIntroRunning)
            {
                isIntroRunning = false;
                ChangeState(GameplayState.Play3D, true);
            }
        }
    }
}
