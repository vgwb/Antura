using Antura.Core;
using Antura.Database;
using Antura.Debugging;
using Antura.FSM;
using Antura.Rewards;
using Antura.UI;
using Antura.Utilities;
using System;
using UnityEngine;

namespace Antura.Minigames
{
    /// <summary>
    /// Base abstract class for all minigame in-scene managers.
    /// Main entry point for the logic of a minigame.
    /// </summary>
    // refactor: this could be better organized to signal what the minigame needs to access, and what the core needs
    public abstract class MiniGameController : SingletonMonoBehaviour<MiniGameController>, IGame
    {
        #region Configuration

        /// <summary>
        /// The current game context. Managers are accessed through this.
        /// </summary>
        public IGameContext Context { get; private set; }

        /// <summary>
        /// Specify which is the game configuration class for this game
        /// </summary>
        protected abstract IGameConfiguration GetConfiguration();

        #endregion

        #region State Manager

        /// <summary>
        /// Access the GameStateManager that controls the minigame's FSM.
        /// </summary>
        public StateMachineManager StateManager
        {
            get { return stateManager; }
        }

        StateMachineManager stateManager = new StateMachineManager();

        public IState GetCurrentState()
        {
            return StateManager.CurrentState;
        }

        public void SetCurrentState(IState state)
        {
            StateManager.CurrentState = state;
        }

        /// <summary>
        /// Specify which is the first state of this game using this method
        /// </summary>
        protected abstract IState GetInitialState();

        #endregion

        #region Outcome

        /// <summary>
        /// State reached when the minigame ends.
        /// Exists regardless of the specific minigame.
        /// </summary>
        private OutcomeGameState OutcomeState;

        /// <summary>
        /// The score in number of stars assigned to this minigame.
        /// </summary>
        public int StarsScore { get; private set; }


        /// <summary>
        /// Value of the score measured inside the minigame.
        /// </summary>
        public int CurrentScore
        {
            get => currentScore;
            set
            {
                currentScore = value;
                Context.GetOverlayWidget().SetStarsScore(currentScore);
            }
        }
        private int currentScore;

        /// <summary>
        /// Maximimum score that can be achieved with this game.
        /// </summary>
        public virtual int MaxScore { get; }

        public float Difficulty
        {
            get
            {
                if (AppManager.I.AppEdition.AutomaticDifficulty)
                {
                    return DifficultyForScore(CurrentScore);
                }

                return GetConfiguration().Difficulty;
            }
        }

        public float DifficultyForScore(int score)
        {
            float diff = score / ((float)MaxScore * 0.8f); // at 80% we get max difficulty
            return Mathf.Clamp01(diff);
        }

        public const float VERY_EASY = 0.2f;
        public const float EASY = 0.4f;
        public const float NORMAL = 0.6f;
        public const float HARD = 0.8f;
        public const float VERY_HARD = 1f;

        #endregion

        #region Events

        /// <summary>
        /// Event raised whenever the game ends.
        /// </summary>
        public event GameResultAction OnGameEnded;

        #endregion

        #region Common State

        /// <summary>
        /// Signals whether the MiniGame has been initialized.
        /// </summary>
        bool initialized;

        /// <summary>
        /// Value of gravity before the game was started.
        /// </summary>
        Vector3 oldGravity;

        /// <summary>
        /// Signals whether the minigame must pause.
        /// </summary>
        bool hasToPause;

        /// <summary>
        /// Gravity
        /// </summary>
        protected virtual Vector3 GetGravity()
        {
            return Vector3.up * (-80);
        }

        #endregion

        #region Initialisation

        protected virtual void Start()
        {
            Initialize(GetConfiguration().Context);
        }

        /// <summary>
        /// Initializes the minigame with the given context.
        /// </summary>
        void Initialize(IGameContext context)
        {
            Context = context;
            OutcomeState = new OutcomeGameState(this);

            OnInitialize(context);

            PlayTitle(StepOne);
        }

        void StepOne()
        {
            if (GetConfiguration().AutoPlayIntro)
                PlayIntro(StepTwo);
            else
                StepTwo();
        }

        void StepTwo()
        {
            SetCurrentState(GetInitialState());

            oldGravity = Physics.gravity;
            Physics.gravity = GetGravity();
            initialized = true;
        }

        private void PlayTitle(Action onComplete)
        {
            if (AppManager.I.AppEdition.PlayTitleAtMiniGameStart)
            {
                Context.GetAudioManager().PlayDialogue(GetConfiguration().TitleLocalizationId, onComplete);
            }
            else
            {
                onComplete();
            }
        }

        public void PlayIntro(Action onComplete)
        {
            if (AppManager.I.AppEdition.PlayIntroAtMiniGameStart)
            {
                var id = GetConfiguration().IntroLocalizationId;
                if (id != LocalizationDataId.None)
                    Context.GetAudioManager().PlayDialogue(id, onComplete);
                else
                    onComplete();
            }
            else
            {
                onComplete();
            }
        }

        public void PlayTutorial(Action onComplete = null)
        {
            Context.GetAudioManager().PlayDialogue(GetConfiguration().TutorialLocalizationId, onComplete);
        }
        public void PlayTutorialConditional(bool condition, Action onComplete = null)
        {
            if (condition)
                PlayTutorial(onComplete);
            else
                onComplete?.Invoke();
        }

        /// <summary>
        /// Implement game's construction steps inside this method.
        /// </summary>
        protected abstract void OnInitialize(IGameContext context);

        #endregion

        #region End Game

        /// <summary>
        /// This must be called whenever the minigame ends.
        /// Called by the minigame logic.
        /// </summary>
        public void EndGame(int stars, int score)
        {
            if (BotTester.I.Config.BotEnabled)
            {
                // Fake max score with the bot, so we can advance
                stars = 3;
                score = 3;
            }

            StarsScore = stars;

            if (OnGameEnded != null)
            {
                OnGameEnded(stars, score);
            }

            // Log trace game result
            Context.GetLogManager().OnGameEnded(stars);

            SetCurrentState(OutcomeState);
        }

        void ForceCurrentMinigameEnd(int value)
        {
            EndGame(value, value);
        }

        protected virtual void HandleSceneSkip()
        {
            if (stateManager.CurrentState != OutcomeState)
            {
                EndGame(3, 3);  // max stars
            }
            else
            {
                EndgameResultPanel.I.Continue();
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// Do not override Update/FixedUpdate; just implement Update and UpdatePhysics inside game states
        /// </summary>
        void Update()
        {
            if (Context == null)
                return;

            stateManager.Update(Time.deltaTime);

            var inputManager = Context.GetInputManager();
            var audioManager = Context.GetAudioManager();

            // TODO: move this outside this method (actually it is useless with the current implementation of PauseMenu)
            inputManager.Enabled = !(GlobalUI.PauseMenu.IsMenuOpen);

            if ((AppManager.I.IsAppSuspended || hasToPause) &&
                !SceneTransitioner.IsShown && GetCurrentState() != OutcomeState
                && !GlobalUI.PauseMenu.IsMenuOpen)
            {
                //GlobalUI.PauseMenu.OpenMenu(true);
            }
            hasToPause = false;

            inputManager.Update(Time.deltaTime);
            audioManager.Update();
        }

        void FixedUpdate()
        {
            stateManager.UpdatePhysics(Time.fixedDeltaTime);
        }

        #endregion

        #region System Events

        void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                hasToPause = true;
            }
        }

        void OnEnable()
        {
            DebugManager.OnForceCurrentMinigameEnd += ForceCurrentMinigameEnd;
            DebugManager.OnSkipCurrentScene += HandleSceneSkip;
        }

        void OnDisable()
        {
            DebugManager.OnForceCurrentMinigameEnd -= ForceCurrentMinigameEnd;
            DebugManager.OnSkipCurrentScene -= HandleSceneSkip;
        }


        void OnDestroy()
        {
            if (initialized)
            {
                Physics.gravity = oldGravity;
            }

            if (Context != null)
            {
                Context.Reset();
            }
        }

        #endregion
    }
}
