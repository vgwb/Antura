using Antura.Audio;
using DG.Tweening;
using UnityEngine;

namespace Antura.Rewards
{
    /// <summary>
    /// Bar that fills up to show the results of a play session
    /// </summary>
    public class EndsessionBar : MonoBehaviour
    {
        public RectTransform BarContainer;
        public RectTransform Bar;
        public EndsessionAchievement[] Achievements;

        [System.NonSerialized]
        public Tween ShowTween;

        Vector2 barSizeDelta;
        float[] achievementsPercent = new[] { 0.333f, 0.666f, 1 };
        float singleMinigameStarPercent;
        int totMinigameStarsGained;
        Tween barTween, shakeTween;

        #region Unity

        void Awake()
        {
            barSizeDelta = Bar.sizeDelta;
            ShowTween = this.GetComponent<RectTransform>().DOAnchorPosX(-1300, 0.35f).From().SetAutoKill(false).Pause();
            Bar.sizeDelta = new Vector2(barSizeDelta.x, 0);
            shakeTween = DOTween.Sequence().SetAutoKill(false).Pause()
                .Append(BarContainer.DOScaleX(1.25f, 0.001f))
                .Append(BarContainer.DOScaleX(1, 0.3f));
        }

        void OnDestroy()
        {
            ShowTween.Kill();
            barTween.Kill();
            shakeTween.Kill();
        }

        #endregion

        #region Public Methods

        internal void Show(int _totMinigamesStarsAvailable)
        {
            totMinigameStarsGained = 0;
            singleMinigameStarPercent = 1f / _totMinigamesStarsAvailable;
            Bar.sizeDelta = new Vector2(barSizeDelta.x, 0);
            ShowTween.Restart();
        }

        internal void Hide()
        {
            barTween.Kill();
            shakeTween.Rewind();
            ShowTween.Rewind();
            foreach (EndsessionAchievement ach in Achievements)
            {
                ach.AchieveReward(false);
                ach.AchieveStar(false);
            }
        }

        internal void IncreaseBy(int _numMinigameStars)
        {
            totMinigameStarsGained += _numMinigameStars;
            barTween.Kill();
            float toPerc = singleMinigameStarPercent * totMinigameStarsGained;
            Vector2 to = new Vector2(barSizeDelta.x, barSizeDelta.y * toPerc);
            barTween = Bar.DOSizeDelta(to, 0.2f);
            shakeTween.Restart();
            for (int i = 0; i < Achievements.Length; ++i)
            {
                EndsessionAchievement ach = Achievements[i];
                bool shouldAchieve = i == 3 && Mathf.Approximately(toPerc, 1) || toPerc >= achievementsPercent[i];
                if (!ach.IsRewardAchieved && shouldAchieve)
                    ach.AchieveReward(true);
                ach.AchieveStar(shouldAchieve);
            }
            AudioManager.I.PlaySound(EndsessionResultPanel.I.SfxIncreaseBar);
        }

        #endregion
    }
}
