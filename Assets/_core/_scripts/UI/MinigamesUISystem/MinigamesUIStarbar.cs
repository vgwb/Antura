using DG.Tweening;
using UnityEngine;

namespace Antura.UI
{
    /// <summary>
    /// Shows the number of obtained stars available in a minigame.
    /// </summary>
    public class MinigamesUIStarbar : ABSMinigamesUIComponent
    {
        public RectTransform ProgressBar;

        private MinigamesUISingleStar[] stars;
        private float[] starPercentages = new[] { 0.333f, 0.666f, 1 };
        private Vector2 progressBarFullSize;
        private Tween gotoTween;

        #region Unity

        void Awake()
        {
            stars = this.GetComponentsInChildren<MinigamesUISingleStar>(true);
            progressBarFullSize = ProgressBar.sizeDelta;
            ProgressBar.sizeDelta = new Vector2(progressBarFullSize.x, 0);
        }

        void OnDestroy()
        {
            gotoTween.Kill();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sends the progress bar to the given percentage (<code>0 to 1</code>),
        /// unlocking eventual stars.
        /// </summary>
        /// <param name="_percentage">Percentage to reach (<code>0 to 1)</code></param>
        public void Goto(float _percentage)
        {
            if (gotoTween != null)
                gotoTween.Kill();
            gotoTween = ProgressBar.DOSizeDelta(new Vector2(progressBarFullSize.x, progressBarFullSize.y * _percentage), 0.2f)
                .OnUpdate(() =>
                {
                    for (int i = 0; i < stars.Length; ++i)
                    {
                        float starPercent = starPercentages[i];
                        float progressPercent = ProgressBar.sizeDelta.y / progressBarFullSize.y;
                        if (progressPercent >= starPercent || Mathf.Approximately(progressPercent, starPercent))
                        {
                            stars[i].Gain();
                        }
                        else
                        {
                            stars[i].Lose();
                        }
                    }
                });
        }

        /// <summary>
        /// Sends the progress bar to the point of the given star index (<code>0 to 2</code>).
        /// </summary>
        /// <param name="_starIndex">Index of the star to reach (<code>0 to 2)</code></param>
        public void GotoStar(int _starIndex)
        {
            Goto(starPercentages[_starIndex]);
        }

        #endregion
    }
}
