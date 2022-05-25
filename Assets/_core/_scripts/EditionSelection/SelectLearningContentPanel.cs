using System.Collections.Generic;
using System.Linq;
using Antura.Audio;
using Antura.Core;
using Antura.Database;
using Antura.Language;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    public class SelectLearningContentPanel : MonoBehaviour
    {
        public TextRender QuestionText;
        public SelectNativeLanguageButton SelectNativeLanguageButton;

        public SelectLearningContentButton prefabButton;
        private List<SelectLearningContentButton> buttons = new List<SelectLearningContentButton>();

        public void OnEnable()
        {
            // HACK: arabic legacy should instead be arabic when entering here
            if (AppManager.I.AppSettings.NativeLanguage == LanguageCode.arabic_legacy)
                AppManager.I.AppSettingsManager.SetNativeLanguage(LanguageCode.arabic);

            HasPerformedSelection = false;

            foreach (var button in buttons)
                Destroy(button.gameObject);
            buttons.Clear();

            // Find all content editions with the current native language
            for (var iContentEdition = 0; iContentEdition < AppManager.I.AppEdition.ContentEditions.Length; iContentEdition++)
            {
                var contentEditionConfig = AppManager.I.AppEdition.ContentEditions[iContentEdition];

                bool isSupported = contentEditionConfig.SupportsLanguage(AppManager.I.AppSettings.NativeLanguage);

                // HACK: For Arabic, we also show the Arabic_Legacy contents
                if (!isSupported && AppManager.I.AppSettings.NativeLanguage == LanguageCode.arabic)
                {
                    isSupported = contentEditionConfig.OverridenNativeLanguages.Contains(LanguageCode.arabic_legacy);
                }
                if (!isSupported) continue;

                var buttonGO = Instantiate(prefabButton.gameObject, prefabButton.transform.parent, true);
                buttonGO.transform.localScale = Vector3.one;
                buttonGO.SetActive(true);
                var button = buttonGO.GetComponent<SelectLearningContentButton>();
                button.Setup(contentEditionConfig);
                buttons.Add(button);
            }
            prefabButton.gameObject.SetActive(false);

            var key = LocalizationDataId.Learn_What;
            AudioManager.I.PlayDialogue(LocalizationManager.GetLocalizationData(key), AppManager.I.AppSettings.NativeLanguage);
            QuestionText.SetOverridenLanguageText(AppManager.I.AppSettings.NativeLanguage, key);
        }

        public bool HasPerformedSelection;
        public void ConfirmSelection(LearningContentID contentId)
        {
            // HACK: if we are looking for arabic, but we need to actually use arabic_legacy, do so now
            var content = AppManager.I.AppEdition.ContentEditions.FirstOrDefault(x => x.ContentID == contentId);
            if (AppManager.I.AppSettings.NativeLanguage == LanguageCode.arabic && content.OverridenNativeLanguages.Contains(LanguageCode.arabic_legacy))
            {
                AppManager.I.AppSettingsManager.SetNativeLanguage(LanguageCode.arabic_legacy);
            }

            AppManager.I.AppSettingsManager.SetLearningContentID(contentId);
            RefreshSelection();
            HasPerformedSelection = true;

            Overlay.enabled = true;
            Overlay.DOColor(new Color(1,1,1,1), 0.35f);
        }

        public void RefreshSelection()
        {
            foreach (var button in buttons)
            {
                if (button.ContentId == AppManager.I.AppSettings.ContentID)
                    button.SetSelected();
                else
                    button.SetUnselected();
            }
        }

        public RectTransform questionRectTr;
        public RectTransform scrollRectTr;
        private Color BGColor;
        public Image BG;
        public Image Overlay;

        private bool isOpen;
        public void Open()
        {
            gameObject.SetActive(true);
            isOpen = true;

            Overlay.color = new Color(1,1,1, 0);
            Overlay.enabled = false;

            questionRectTr.gameObject.SetActive(true);
            questionRectTr.anchoredPosition = new Vector2(0, 500);
            questionRectTr.DOAnchorPos(new Vector2(0, 0), 0.35f);

            scrollRectTr.anchoredPosition = new Vector2(0, 1000);
            scrollRectTr.DOAnchorPos(new Vector2(0, 0), 0.35f).SetDelay(0.5f);
            if (BGColor == default) BGColor = BG.color;
            BG.color = new Color(BGColor.r, BGColor.g, BGColor.b, 0f);
            BG.DOColor(BGColor, 0.35f);
        }

        public void Close()
        {
            questionRectTr.DOAnchorPos(new Vector2(0, 500), 0.35f);

            gameObject.SetActive(false);
            isOpen = false;
        }

        public bool IsOpen()
        {
            return isOpen;
        }
    }
}
