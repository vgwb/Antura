using DG.DeInspektor.Attributes;
using UnityEngine;

namespace Antura.Discover
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

        void Start()
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
            Debug.LogWarning("ActivityBase: should validate the activity");
        }

        #endregion
    }
}
