using Antura.UI;
using DG.DeExtensions;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Rewards
{
    public class DailyRewardUI : MonoBehaviour
    {
        public Color unlockedColor = Color.green;

        public Sprite bonesSprite, usedBonesSprite;
        public Sprite test1Sprite;
        public Sprite test2Sprite;

        public Image bgImg;
        public Image imageUI;
        public TextMeshProUGUI amountTextUI;

        public Image lockUI;
        public CanvasGroup unlockUI;

        public TextRender dayTextUI;

        //Color lockedColor;
        Tween unlockTween;
        Tween bounceTween;

        void Awake()
        {
            // @note: Lock is not used anymore, we hide it
            //lockedColor = bgImg.color;
            lockUI.gameObject.SetActive(false);

            unlockTween = DOTween.Sequence().SetAutoKill(false).Pause()
                .Append(this.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f))
                .Join(bgImg.DOColor(unlockedColor, 0.5f))
                .Join(unlockUI.DOFade(0, 0.5f).From())
                .Join(imageUI.transform.DOPunchRotation(new Vector3(0, 0, 20), 0.8f));
            unlockTween.ForceInit();
        }

        void OnDestroy()
        {
            unlockTween.Kill();
            bounceTween.Kill();
        }

        public void SetReward(DailyRewardManager.DailyReward reward)
        {
            SetRewardType(reward.rewardType);
            SetRewardAmount(reward.amount);
        }

        private void SetRewardType(DailyRewardType rewardType)
        {
            switch (rewardType)
            {
                case DailyRewardType.Bones:
                    imageUI.sprite = bonesSprite;
                    break;
                case DailyRewardType.Test1:
                    imageUI.sprite = test1Sprite;
                    break;
                case DailyRewardType.Test2:
                    imageUI.sprite = test2Sprite;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("rewardType", rewardType, null);
            }
        }

        private void SetRewardAmount(int amount)
        {
            amountTextUI.text = amount.ToString();
        }

        public void Bounce(bool doBounce)
        {
            if (!doBounce)
            {
                if (bounceTween != null)
                    bounceTween.Rewind();
            }
            else
            {
                if (bounceTween == null)
                {
                    bounceTween = this.transform.DOScale(this.transform.localScale * 0.9f, 0.4f)
                        .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo)
                        .SetAutoKill(false);
                }
                bounceTween.Restart();
            }
        }

        public void SetLocked()
        {
            unlockTween.Rewind();
            bgImg.SetAlpha(0.6f);
            imageUI.SetAlpha(0.6f);
            unlockUI.gameObject.SetActive(false);
        }

        public void SetReadyToBeUnlocked()
        {
            SetLocked();
            bgImg.SetAlpha(1f);
            imageUI.SetAlpha(1f);
        }

        public void SetUnlocked(bool animate = false)
        {
            bgImg.color = unlockedColor;
            imageUI.sprite = usedBonesSprite;
            Bounce(false);
            if (!animate)
            {
                bgImg.SetAlpha(1);
                imageUI.SetAlpha(1);
                unlockUI.gameObject.SetActive(true);
            }
            else
            {
                unlockTween.Restart();
            }
        }

        public void SetDay(int day)
        {
            dayTextUI.text = "Day " + day;
        }

        public void HideDay()
        {
            dayTextUI.gameObject.SetActive(false);
        }
    }

}
