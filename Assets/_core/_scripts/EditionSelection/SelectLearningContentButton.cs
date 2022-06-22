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

            nameText.SetOverridenLanguageText(parentPanel.SelectedNativeCode, LocKey);
        }

        public LocalizationDataId LocKey
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
            parentPanel.SelectedButton = this;
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
