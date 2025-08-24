using Demigiant.DemiTools;
using Demigiant.DemiTools.DeUnityExtended;
using System.Collections;
using UnityEngine;
using TMPro;
using DG.DeInspektor.Attributes;
using DG.Tweening;

namespace Antura.Discover.Activities
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

        [Header("Optional Result Banner")]
        [SerializeField] CanvasGroup resultBanner;
        [SerializeField] TextMeshProUGUI resultLabel;

        #endregion

        public ActivityTimer Timer => timer;
        public DeUIButton BtClose => btClose;
        public DeUIButton BtHelp => btHelp;
        public DeUIButton BtValidate => btValidate;
        public CanvasGroup ResultBanner => resultBanner;
        public TextMeshProUGUI ResultLabel => resultLabel;

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

        /// <summary>
        /// Shows a simple result banner for a brief time
        /// </summary>
        public IEnumerator ShowResultBanner(string text, Color color, float duration = 1f)
        {
            if (resultBanner == null || resultLabel == null)
            {
                yield return new WaitForSecondsRealtime(duration);
                yield break;
            }
            resultLabel.text = text ?? string.Empty;
            resultLabel.color = color;
            resultBanner.gameObject.SetActive(true);
            resultBanner.alpha = 0f;

            // Fade in, wait, fade out
            resultBanner.DOKill();
            var seq = DOTween.Sequence()
                .Append(resultBanner.DOFade(1f, 0.2f).SetUpdate(true))
                .AppendInterval(duration)
                .Append(resultBanner.DOFade(0f, 0.25f).SetUpdate(true))
                .OnComplete(() => resultBanner.gameObject.SetActive(false));
            seq.SetUpdate(true);
            yield return new WaitForSecondsRealtime(0.2f + duration + 0.25f);
        }

        #endregion
    }
}
