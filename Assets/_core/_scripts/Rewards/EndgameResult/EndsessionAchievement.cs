using Antura.Audio;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Rewards
{
    /// <summary>
    /// Represents an earned achievement (reward) in the EndsessionResultPanel
    /// </summary>
    public class EndsessionAchievement : MonoBehaviour
    {
        public Color LockedColor = Color.red;

        [Header("References")]
        public Transform Star;

        public Image RewardBg;
        public RawImage Reward;
        public Image Lock;
        public Sprite UnlockedSprite;

        public bool IsRewardAchieved { get; private set; }
        public bool IsStarAchieved { get; private set; }
        bool hasReward;
        Sprite lockedSprite;
        Color defColor;
        Sequence achieveRewardTween;
        Tween achieveStarTween;

        #region Unity

        void Awake()
        {
            hasReward = RewardBg.gameObject.activeSelf;
            lockedSprite = Lock.sprite;
            defColor = RewardBg.color;
            RewardBg.color = LockedColor;
            Star.gameObject.SetActive(false);

            achieveStarTween = Star.DOPunchScale(Vector3.one * 1.25f, 0.4f).SetAutoKill(false).Pause();
            if (hasReward)
            {
                achieveRewardTween = DOTween.Sequence().SetAutoKill(false).Pause()
                    .InsertCallback(0.2f, () => Lock.sprite = UnlockedSprite)
                    .Join(Lock.transform.DOPunchScale(Vector3.one * 1.3f, 0.2f))
                    .Insert(0.4f,
                        Lock.transform.DOScale(0.0001f, 0.25f).SetEase(Ease.InQuad).OnComplete(() => Lock.gameObject.SetActive(false)))
                    .Join(Lock.transform.DORotate(new Vector3(0, 0, 220), 0.25f, RotateMode.FastBeyond360))
                    .Join(RewardBg.DOColor(defColor, 0.25f).SetEase(Ease.Linear));
            }
        }

        void OnDestroy()
        {
            achieveRewardTween.Kill();
            achieveStarTween.Kill();
        }

        #endregion

        #region Public Methods

        internal void AchieveReward(bool _doAchieve, bool _immediate = false)
        {
            if (IsRewardAchieved == _doAchieve)
                return;

            IsRewardAchieved = _doAchieve;
            if (hasReward)
            {
                Lock.sprite = lockedSprite;
                Lock.gameObject.SetActive(true);
            }
            if (_doAchieve)
            {
                if (_immediate)
                    achieveRewardTween.Complete();
                else
                    achieveRewardTween.Restart();
            }
            else
                achieveRewardTween.Rewind();
        }

        internal void AchieveStar(bool _doAchieve)
        {
            if (IsStarAchieved == _doAchieve)
                return;

            IsStarAchieved = _doAchieve;
            if (_doAchieve)
            {
                Star.gameObject.SetActive(true);
                achieveStarTween.Restart();
                AudioManager.I.PlaySound(EndsessionResultPanel.I.SfxGainStar);
            }
            else
            {
                Star.gameObject.SetActive(false);
                achieveStarTween.Rewind();
            }
        }

        #endregion
    }
}
