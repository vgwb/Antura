using Demigiant.DemiTools;
using Demigiant.DemiTools.DeUnityExtended;
using DG.DeInspektor.Attributes;
using UnityEngine;

namespace Antura.Discover
{
    public class ActivityOverlay : MonoBehaviour
    {
        #region Serialized

        [Header("References")]
        [DeEmptyAlert]
        [SerializeField] ActivityTimer timer;
        [DeEmptyAlert]
        [SerializeField] DeUIButton btClose;
        [DeEmptyAlert]
        [SerializeField] DeUIButton btHelp;
        [DeEmptyAlert]
        [SerializeField] DeUIButton btValidate;

        #endregion

        public ActivityTimer Timer => timer;
        public DeUIButton BtClose => btClose;
        public DeUIButton BtHelp => btHelp;
        public DeUIButton BtValidate => btValidate;

        #region Public Methods

        /// <summary>
        /// Set the timer with custom options 
        /// </summary>
        public void SetTimer(bool hasTimer, int seconds)
        {
            timer.gameObject.SetActive(hasTimer);
            if (hasTimer)
                timer.RestartTimer(seconds);
            else
                timer.CancelTimer();
        }

        #endregion
    }
}