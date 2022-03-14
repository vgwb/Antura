using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Minigames
{
    // refactor: should be handled as other widgets are
    public class RadialGadget : MonoBehaviour
    {
        public Image Radial;

        public bool IsPulsing { get; private set; }
        private Tween pulseTween;

        #region Unity

        void Awake()
        {
            pulseTween = Radial.transform.DOScale(Radial.transform.localScale * 1.05f, 0.3f).SetEase(Ease.InOutQuad)
                .SetLoops(-1, LoopType.Yoyo)
                .SetAutoKill(false).Pause();
        }

        void OnDestroy()
        {
            pulseTween.Kill();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the timer at the given percentage, 0 to 1
        /// </summary>
        /// <param name="_percentage">Fill percentage, 0 to 1 (fills counterclockwise, contrary to minigamesUI timer)</param>
        /// <param name="_pulse">If TRUE also pulses the filled percentage, otherwise stops any pulsing already playing
        /// (same as calling <code>PulseOn</code> or <code>PulseOff</code> after setting the percentage)</param>
        public void SetPercentage(float _percentage, bool _pulse = true)
        {
            Radial.fillAmount = _percentage;
            if (_pulse)
                PulseOn();
            else
                PulseOff();
        }

        /// <summary>
        /// Starts the pulsing of the filled percentage
        /// </summary>
        public void PulseOn()
        {
            IsPulsing = true;
            pulseTween.PlayForward();
        }

        /// <summary>
        /// Stops the pulsing of the filled percentage
        /// </summary>
        public void PulseOff()
        {
            IsPulsing = false;
            pulseTween.Rewind();
        }

        #endregion
    }
}
