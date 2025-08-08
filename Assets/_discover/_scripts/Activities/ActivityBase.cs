using DG.DeInspektor.Attributes;
using UnityEngine;

namespace Antura.Discover.Activities
{
    public class ActivityBase : MonoBehaviour
    {
        [Header("Common properties")]
        public string ActivityCode;
        [Range(1, 600)]
        [SerializeField] int timerSeconds = 60;

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
                activityPanel.PauseTimer();
                activityPanel.Hide();
            }
            else
            {
                Debug.Log("ActivityBase: FAIL");
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
