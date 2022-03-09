using Antura.Audio;
using DG.Tweening;
using UnityEngine;

namespace Antura.UI
{
    /// <summary>
    /// A single star icon for the MinigamesUIStarbar script.
    /// </summary>
    public class MinigamesUISingleStar : MonoBehaviour
    {
        public Transform Star;

        private bool hasStar;
        private Tween loseTween;
        private Tween gainTween;

        #region Unity

        void Awake()
        {
            // Tweens
            loseTween = Star.DOScale(0.001f, 0.25f).SetAutoKill(false).Pause()
                .OnComplete(() => Star.gameObject.SetActive(false));
            loseTween.ForceInit();
            gainTween = Star.DOScale(0.001f, 0.5f).From().SetEase(Ease.OutElastic, 1.70f, 0.5f).SetAutoKill(false).Pause();

            loseTween.Complete();
        }

        void OnDestroy()
        {
            loseTween.Kill();
            gainTween.Kill();
        }

        #endregion

        #region Public Methods

        public void Gain()
        {
            if (hasStar)
            { return; }
            AudioManager.I.PlaySound(Sfx.ScoreUp);
            hasStar = true;
            loseTween.Rewind();
            Star.gameObject.SetActive(true);
            gainTween.Restart();
        }

        public void Lose()
        {
            if (!hasStar)
            { return; }

            hasStar = false;
            gainTween.Complete();
            Star.gameObject.SetActive(true);
            loseTween.Restart();
        }

        #endregion
    }
}
