using Antura.Core;
using Antura.UI;
using DG.Tweening;
using System;
using Antura.Database;
using DG.DeExtensions;
using UnityEngine;

namespace Antura.Profile
{
    public class ConfirmationPanelUI : MonoBehaviour
    {
        public UIButton SkipButton;
        public TextRender nativeMessage;
        public TextRender learningMessage;

        public Action onSkip;

        private Tween stepTween;

        void Awake()
        {
            gameObject.SetActive(false);
            SkipButton.Bt.onClick.RemoveAllListeners();
            SkipButton.Bt.onClick.AddListener(DoSkip);

            var textRenders = SkipButton.GetComponentsInChildren<TextRender>(true);
            foreach (var textRender in textRenders)
            {
                textRender.LocalizationId = LocalizationDataId.UI_Btn_Skip;
                textRender.SetSentence(textRender.LocalizationId);
            }
        }

        public void Show(LocalizationDataId locId)
        {
            gameObject.SetActive(true);
            stepTween?.Kill();
            GetComponent<RectTransform>().anchoredPosition.SetY(-1000);
            stepTween = GetComponent<RectTransform>().DOAnchorPosY(-230, 1f).SetEase(Ease.OutBack).SetDelay(2f).Play();

            nativeMessage.SetText(LocalizationManager.GetNative(locId));
            learningMessage.SetText(LocalizationManager.GetHelp(locId));
        }

        public void Hide()
        {
            stepTween?.Kill();
            stepTween = GetComponent<RectTransform>().DOAnchorPosY(-1030, 0.25f).SetEase(Ease.InBack).Play().OnComplete(() => { gameObject.SetActive(false); });
        }

        void DoSkip()
        {
            onSkip?.Invoke();
            Hide();
        }
    }
}
