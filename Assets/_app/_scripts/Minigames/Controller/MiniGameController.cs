using Antura.Core;
using Antura.Debugging;
using Antura.FSM;
using Antura.Rewards;
using Antura.UI;
using Antura.Utilities;
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
        public StateMachineManager StateManager {
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
            SetCurrentState(GetInitialState());

            oldGravity = Physics.gravity;
            Physics.gravity = GetGravity();
            initialized = true;
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
            StarsScore = stars;

            if (OnGameEnded != null) {
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
            stateManager.Update(Time.deltaTime);

            var inputManager = Context.GetInputManager();
            var audioManager = Context.GetAudioManager();

            // TODO: move this outside this method (actually it is useless with the current implementation of PauseMenu)
            inputManager.Enabled = !(GlobalUI.PauseMenu.IsMenuOpen);

            if ((AppManager.I.IsPaused || hasToPause) && !SceneTransitioner.IsShown && GetCurrentState() != OutcomeState) {
                GlobalUI.PauseMenu.OpenMenu(true);
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
            if (pause) {
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
            if (initialized) {
                Physics.gravity = oldGravity;
            }

            if (Context != null) {
                Context.Reset();
            }
        }

        #endregion
    }
}