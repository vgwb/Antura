using UnityEngine;

namespace Antura.Discover.Activities
{
    public enum Difficulty
    {
        Default = 0,
        Tutorial = 1,
        Easy = 2,
        Normal = 3,
        Expert = 4
    }


    public class ActivityBase : MonoBehaviour
    {
        [Header("Common properties")]
        public string ActivityCode;

        [Tooltip("Timer for the activity, if enabled")]
        [SerializeField] bool hasTimer = true;
        [Range(1, 600)]
        [SerializeField] int timerSeconds = 60;

        [Tooltip("Points to add when the activity is successful")]
        [SerializeField] int PointsSuccess = 0;

        [Tooltip("Points to add when the activity fails")]
        [SerializeField] int PointsFail = -1;

        [SerializeField] GameObject HelpPanel;

        [Tooltip("Optional permalink of the Node with the mission")]
        public string NodeDescription;

        [Tooltip("Optional permalink of the Node when success")]
        public string NodeSuccess;

        [Tooltip("Optional permalink of the Node when fail")]
        public string NodeFail;

        private ActivityPanel activityPanel;

        public void Open()
        {
            activityPanel = GetComponentInParent<ActivityPanel>();
            Init();
        }

        public virtual void Init()
        {

        }


        #region Public Methods

        /// <summary>
        /// Called by <see cref="ActivityOverlay"/> when the Help buttons is clicked,
        /// should toggle (show/hide) help panel
        /// </summary>
        public void ToggleHelp()
        {

        }

        /// <summary>
        /// Called by <see cref="ActivityOverlay"/> when the eventual timer has elapsed
        /// </summary>
        public void TimerElapsed()
        {
            // TODO: Fail
            Debug.LogWarning("ActivityBase: should fail because timer has elapsed");
        }

        /// <summary>
        /// Called by <see cref="ActivityOverlay"/> when the validate button is clicked
        /// </summary>
        public void Validate()
        {
            bool result = DoValidate();
            if (result)
            {
                Debug.Log("ActivityBase: SUCCESS");
                QuestManager.I.AddProgressPoints(PointsSuccess);
                activityPanel.PauseTimer();
                activityPanel.Hide();
            }
            else
            {
                Debug.Log("ActivityBase: FAIL");
                QuestManager.I.AddProgressPoints(PointsFail);
                activityPanel.Hide();
            }
        }

        public virtual bool DoValidate()
        {
            // Default implementation, should be overridden by derived classes
            return false;
        }

        #endregion
    }
}
