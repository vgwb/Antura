using DG.DeInspektor.Attributes;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class ActivityBase : MonoBehaviour
    {
        [Header("Common properties")]
        [Range(1, 600)]
        [SerializeField] int timerSeconds = 60;

        [SerializeField] GameObject HelpPanel;

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
