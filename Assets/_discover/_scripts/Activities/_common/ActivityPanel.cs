using Antura.UI;
using DG.DeExtensions;
using DG.DeInspektor.Attributes;
using UnityEngine;

namespace Antura.Discover.Activities
{
    /// <summary>
    /// Main container/canvas and controller for the activities, which has ActivityOverlay and the specific Activity as children.
    /// Retrieves the required elements dynamically at runtime in order to prevent dependencies.
    /// Calls methods on ActivityBase when the eventual timer elapses or the validate button is pressed
    /// </summary>
    public class ActivityPanel : MonoBehaviour
    {
        #region Serialized

        [SerializeField] bool hasTimer;
        [Range(1, 600)]
        [SerializeField] int timerSeconds = 60;
        #endregion

        bool initialized;
        bool currHasTimer;
        ActivityOverlay overlay;
        ActivityBase activityBase;

        #region Unity + INIT

        void Init()
        {
            if (initialized)
                return;

            initialized = true;

            activityBase = this.GetComponentInChildren<ActivityBase>();
            if (activityBase == null)
            {
                Debug.LogError($"ActivityPanel: couldn't find ActivityBase child");
                return;
            }

            overlay = this.GetComponentInChildren<ActivityOverlay>();
            if (overlay == null)
            {
                Debug.LogError($"ActivityPanel: couldn't find ActivityOverlay child");
                return;
            }
        }

        void Awake()
        {
            Init();
        }

        void Start()
        {
            overlay.BtClose.onClick.AddListener(activityBase.ExitWithoutPoints);
            overlay.BtHelp.onClick.AddListener(activityBase.ToggleHelp);
            overlay.BtValidate.onClick.AddListener(activityBase.Validate);

            GlobalUI.PauseMenu.OnPauseToggled.Subscribe(OnGlobalPauseToggled);
            overlay.Timer.OnTimerElapsed.Subscribe(OnTimerElapsed);
        }

        void OnDestroy()
        {
            GlobalUI.PauseMenu.OnPauseToggled.Unsubscribe(OnGlobalPauseToggled);
            overlay.Timer.OnTimerElapsed.Unsubscribe(OnTimerElapsed);
        }

        #endregion

        #region Public Methods

        public void Open()
        {
            Init();
            // Use settings exposed by the concrete activity
            Show(activityBase.HasTimer, activityBase.TimerSeconds);
            activityBase.Open();
        }

        /// <summary>
        /// Shows the activity panel and uses the timer options set via Inspector
        /// </summary>
        [DeMethodButton(mode = DeButtonMode.PlayModeOnly)]
        public void Show()
        {
            Init();
            currHasTimer = hasTimer;
            DoShow(true);
            overlay.SetTimer(hasTimer, timerSeconds);
        }

        /// <summary>
        /// Shows the activity panel and allows to pass custom timer options
        /// </summary>
        public void Show(bool pHasTimer, int pTimerSeconds)
        {
            Init();
            currHasTimer = pHasTimer;
            DoShow(true);
            overlay.SetTimer(pHasTimer, pTimerSeconds);
        }

        /// <summary>
        /// Closes the activity panel
        /// </summary>
        [DeMethodButton(mode = DeButtonMode.PlayModeOnly)]
        public void Hide()
        {
            Init();
            overlay.Timer.CancelTimer();
            DoShow(false);
        }

        /// <summary>
        /// Enables/disables the validate button in the Overlay
        /// </summary>
        [DeMethodButton(null, 0, true, mode = DeButtonMode.PlayModeOnly)]
        public void EnableValidateButton(bool enable)
        {
            Init();
            overlay.BtValidate.interactable = enable;
        }

        /// <summary>
        /// Pauses the timer
        /// </summary>
        [DeMethodButton(mode = DeButtonMode.PlayModeOnly)]
        public void PauseTimer()
        {
            Init();
            overlay.Timer.PauseTimer();
        }

        /// <summary>
        /// Resumes the timer
        /// </summary>
        [DeMethodButton(mode = DeButtonMode.PlayModeOnly)]
        public void ResumeTimer()
        {
            Init();
            overlay.Timer.ResumeTimer();
        }

        #endregion

        #region Methods

        void DoShow(bool show)
        {
            this.gameObject.SetActive(show);
            EnableValidateButton(false);

            // TODO: Animate in/out
        }

        #endregion

        #region Callbacks

        void OnGlobalPauseToggled(bool paused)
        {
            if (!currHasTimer)
                return;

            if (paused)
                PauseTimer();
            else
                ResumeTimer();
        }

        void OnTimerElapsed()
        {
            if (activityBase != null)
                activityBase.TimerElapsed();
        }

        #endregion
    }
}
