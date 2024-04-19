using Antura.UI;
using DG.DeInspektor.Attributes;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class TextChoiceBox : AbstractChoiceBox
    {
        #region Serialized

        [Header("References - Specific")]
        [DeEmptyAlert]
        [SerializeField] TextRender textRender;
        [DeEmptyAlert]
        [SerializeField] TMP_Text tf;

        #endregion

        #region Methods

        protected override Sequence CreateShowTween()
        {
            const float duration = 0.5f;
            return DOTween.Sequence()
                .Join(box.DOAnchorMin(new Vector2(1, 0), duration).From().SetEase(Ease.OutBack))
                .Join(bg.DOAnchorMax(new Vector2(0, 1), duration * 0.85f).From().SetEase(Ease.OutBack))
                .Join(bg.DOAnchorPosX(400, duration * 0.85f).From().SetEase(Ease.OutBack))
                .Insert(duration * 0.4f, numbox.DOScale(0, duration * 0.8f).From().SetEase(Ease.OutBounce));
        }
        
        protected override Sequence CreateHoverTween()
        {
            const float duration = 0.35f;
            return DOTween.Sequence()
                .Join(bg.DOLocalRotate(new Vector3(0, 0, 2.5f), duration).SetEase(Ease.OutQuad));
        }

        protected override Sequence CreateSelectTween()
        {
            const float duration = 0.35f;
            const float scale = 1.27f;
            return DOTween.Sequence()
                .Join(box.DOScale(scale, duration).SetEase(Ease.OutBack))
                .Join(tf.transform.DOScale(1 / scale, duration).SetEase(Ease.OutBack))
                .Join(bg.DOScale(scale, duration * 0.75f).SetEase(Ease.OutBack))
                .Join(btConfirm.GetComponent<RectTransform>().DOAnchorPosX(160, duration).From(true).SetEase(Ease.OutBack))
                .Join(numbox.DOAnchorPos(new Vector2(100, 32), duration).SetRelative().SetEase(Ease.OutBack));
        }

        protected override void SetText(string text)
        {
            textRender.SetText(text);
        }

        #endregion
    }
}