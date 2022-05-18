using System;
using System.Collections;
using Antura.Core;
using Antura.Database;
using Antura.Language;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    public class SelectLearningContentButton : MonoBehaviour
    {
        public TextRender nameText;
        public UIButton button;
        public Image iconImage;

        public SelectLearningContentPanel parentPanel;

        private LearningContentID contentId;
        public LearningContentID ContentId => contentId;

        public void Setup(ContentEditionConfig editionConfig)
        {
            this.contentId = editionConfig.ContentID;

            iconImage.sprite = editionConfig.TransitionLogo;

            var code = editionConfig.LearningLanguage;
            if (code == LanguageCode.arabic_legacy) code = LanguageCode.arabic;
            var locKeyText = $"Learn_{code}";
            var locKey = Enum.Parse<LocalizationDataId>(locKeyText, true);
            nameText.SetOverridenLanguageText(AppManager.I.AppSettings.NativeLanguage, locKey);
        }

        public void OnClick()
        {
            parentPanel.ConfirmSelection(contentId);
        }

        public void SetUnselected()
        {
            button.Bt.interactable = true;
        }
        public void SetSelected()
        {
            button.Bt.interactable = false;
        }
    }
}
