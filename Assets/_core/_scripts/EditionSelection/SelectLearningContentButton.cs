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
        public Image BookImage;
        public Image iconImage;
        public Image Flag;

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

            Flag.sprite = editionConfig.LearningLanguageConfig.FlagIcon;

            if (editionConfig.LearnMethod.ID == LearnMethodID.LearnToRead)
            {
                Flag.gameObject.SetActive(false);
                // BookImage.color = new Color(255f / 253f, 255f / 182, 255f / 182);
            }
            else
            {
                Flag.gameObject.SetActive(true);
                // BookImage.color = Color.white;
            }
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
