using DG.DeInspektor.Attributes;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Discover
{
    public class TextAndImageChoiceBox : AbstractChoiceBox
    {
        #region Serialized

        [Header("References - Specific")]
        [DeEmptyAlert]
        [SerializeField] TMP_Text tf;
        [DeEmptyAlert]
        [SerializeField] Image img;
        [DeEmptyAlert]
        [SerializeField] RectTransform textContainer, imgContainer, imgBg, imgBox;

        #endregion
        
        protected override BoxType boxType => BoxType.TextAndImage;

        #region Methods

        protected override Sequence CreateShowTween()
        {
            const float duration = 0.5f;
            return DOTween.Sequence()
                .Join(imgBox.DOAnchorMin(new Vector2(0, 1), duration).From().SetEase(Ease.OutBack))
                .Join(imgBg.DOAnchorMax(new Vector2(1, 0), duration * 0.85f).From().SetEase(Ease.OutBack))
                .Join(imgBg.DOAnchorPosY(300, duration * 0.85f).From(true).SetEase(Ease.OutBack))
                .Join(box.DOAnchorMin(new Vector2(0, 1), duration).From().SetEase(Ease.OutBack))
                .Join(bg.DOAnchorMax(new Vector2(1, 0), duration * 0.85f).From().SetEase(Ease.OutBack))
                .Join(bg.DOAnchorPosY(256, duration * 0.85f).From().SetEase(Ease.OutBack))
                .Insert(duration * 0.4f, numbox.DOScale(0, duration * 0.8f).From().SetEase(Ease.OutBounce))
                .Insert(duration * 0.4f, icoTranslation.transform.DOScale(0, duration * 0.8f).From().SetEase(Ease.OutBounce));
        }

        protected override Sequence CreateHoverTween()
        {
            const float duration = 0.35f;
            return DOTween.Sequence()
                .Join(imgBg.DOLocalRotate(new Vector3(0, 0, 2.5f), duration).SetEase(Ease.OutQuad));
        }

        protected override Sequence CreateSelectTween()
        {
            const float duration = 0.35f;
            const float imgScale = 1.18f;
            const float textScale = 1.05f;
            return DOTween.Sequence()
                .Join(imgContainer.DOAnchorPosY(20, duration).SetRelative().SetEase(Ease.OutBack))
                .Join(textContainer.DOAnchorPosY(-5, duration).SetRelative().SetEase(Ease.OutBack))
                .Join(imgBox.DOScale(imgScale, duration).SetEase(Ease.OutBack))
                .Join(imgBg.DOScale(imgScale, duration * 0.75f).SetEase(Ease.OutBack))
                .Join(box.DOScale(textScale, duration).SetEase(Ease.OutBack))
                .Join(bg.DOScale(textScale, duration * 0.75f).SetEase(Ease.OutBack))
                .Join(tf.transform.DOScale(1 / textScale, duration).SetEase(Ease.OutBack))
                .Join(btConfirm.GetComponent<RectTransform>().DOAnchorPosY(-150, duration).From(true).SetEase(Ease.OutBack))
                .Join(numbox.DOAnchorPos(new Vector2(30, 32), duration).SetRelative().SetEase(Ease.OutBack))
                .Join(icoTranslation.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-100, -32), duration).SetRelative().SetEase(Ease.OutBack));
        }

        public override void Show(NodeChoice choiceNode, bool isQuiz, bool doUseLearningLanguage)
        {
            base.Show(choiceNode, isQuiz, doUseLearningLanguage);

            Sprite sprite = choiceNode.GetImage();
            img.sprite = sprite;
            img.color = sprite == null ? new Color(0.38f, 0.38f, 0.38f) : Color.white;
        }

        #endregion
    }
}
