using System;
using System.Collections;
using Antura.Audio;
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

        private ContentEditionConfig Content;
        public SelectLearningContentPanel parentPanel;

        private LearningContentID contentId;
        public LearningContentID ContentId => contentId;

        public void Setup(ContentEditionConfig editionConfig)
        {
            Content = editionConfig;
            this.contentId = editionConfig.ContentID;

            iconImage.sprite = editionConfig.TransitionLogo;

            var code = editionConfig.LearningLanguage;
            if (code == LanguageCode.arabic_legacy) code = LanguageCode.arabic;

            nameText.SetOverridenLanguageText(AppManager.I.AppSettings.NativeLanguage, LocKey);
        }

        private LocalizationDataId LocKey
        {
            get
            {
                var locKeyText = Content.LearnMethod.ID == LearnMethodID.LearnToRead ? $"Learn_Read" : $"Learn_{Content.LearningLanguage}";
                var locKey = Enum.Parse<LocalizationDataId>(locKeyText, true);
                return locKey;
            }
        }

        public void OnClick()
        {
            AudioManager.I.PlayDialogue(LocalizationManager.GetLocalizationData(LocKey), AppManager.I.AppSettings.NativeLanguage);
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
