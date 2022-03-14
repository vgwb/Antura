using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Rewards
{
    [RequireComponent(typeof(Image))]
    public class DailyRewardPopup : MonoBehaviour
    {
        public Image Img { get { if (img == null) img = this.GetComponent<Image>(); return img; } }
        public RectTransform RectT { get { if (rectT == null) rectT = this.GetComponent<RectTransform>(); return rectT; } }

        Image img;
        RectTransform rectT;
        Tween popTween;

        #region Unity

        void OnDestroy()
        {
            popTween.Kill();
        }

        #endregion

        #region Public Methods

        public void PopFromTo(Vector2 fromP, Vector2 toP)
        {
            const float duration = 0.4f;
            this.gameObject.SetActive(true);
            RectT.anchoredPosition = fromP;
            popTween = DOTween.Sequence()
                .OnComplete(() => this.gameObject.SetActive(false))
                .Join(rectT.DOAnchorPosX(toP.x, duration).SetEase(Ease.InQuad))
                .Join(rectT.DOAnchorPosY(toP.y, duration))
                .Insert(duration * 0.75f, RectT.DOScale(0.1f, duration * 0.25f));
        }

        #endregion
    }
}
