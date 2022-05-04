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
            var locKeyText = $"Learn_{editionConfig.LearningLanguage}";
            var locKey = Enum.Parse<LocalizationDataId>(locKeyText, true);
            nameText.SetText(LocalizationManager.GetLocalizationData(locKey).GetNativeText());
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
