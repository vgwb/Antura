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

        public TextRender ContentJourneyLabel;

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

            if (editionConfig.LearnMethod.ShowFlag)
            {
                Flag.gameObject.SetActive(false);
                // BookImage.color = new Color(255f / 253f, 255f / 182, 255f / 182);
            }
            else
            {
                Flag.gameObject.SetActive(true);
                // BookImage.color = Color.white;
            }

            if (AppManager.PROFILE_INVERSION)
            {
                // Load the data tied to this, if exists
                var contentID = editionConfig.ContentID;
                var data = AppManager.I.DB.GetContentProfileData(contentID);
                if (data != null)
                {
                    if (AppManager.VERBOSE_INVERSION) Debug.LogError($"[Inversion] Loaded MAX JP for {contentID} as {data.MaxStage}-{data.MaxLearningBlock}-{data.MaxPlaySession}");
                    ContentJourneyLabel.text = $"{data.MaxStage}-{data.MaxLearningBlock}-{data.MaxPlaySession}";
                }
                else
                {
                    if (AppManager.VERBOSE_INVERSION) Debug.LogError($"[Inversion] No data for {contentID}, using initial JP");
                    ContentJourneyLabel.text = JourneyPosition.InitialJourneyPosition.ToDisplayedString(withPlaySession:true);
                }
            }
        }

        public LocalizationDataId LocKey
        {
            get
            {
                var locKeyText = Content.LearnMethod.LearningContentButtonKey(Content.LearningLanguage);
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
