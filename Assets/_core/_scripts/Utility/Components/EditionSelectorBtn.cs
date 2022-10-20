using System;
using Antura.Core;
using System.Collections;
using System.Collections.Generic;
using Antura.Database;
using Antura.Language;
using Antura.Scenes;
using Antura.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Utilities
{
    public class EditionSelectorBtn : MonoBehaviour
    {
        public TextRender nameText;
        public Image iconImage;
        public Image Flag;
        public EditionSelectionManager SelectionManager;

        void Start()
        {
            ContentEditionConfig editionConfig = AppManager.I.ContentEdition;
            var languageCode = AppManager.I.ContentEdition.LearningLanguage;
            if (languageCode == LanguageCode.arabic_legacy)
                languageCode = LanguageCode.arabic;
            var locKeyText = $"Learn_{languageCode}";
            var locKey = Enum.Parse<LocalizationDataId>(locKeyText, true);
            nameText.SetOverridenLanguageText(AppManager.I.AppSettings.NativeLanguage, locKey);
            iconImage.sprite = editionConfig.TransitionLogo;

            Flag.sprite = editionConfig.LearningLanguageConfig.FlagIcon;
            if (editionConfig.LearnMethod.ID == LearnMethodID.LearnToRead)
            {
                Flag.gameObject.SetActive(false);
            }
            else
            {
                Flag.gameObject.SetActive(true);
            }

            bool isVisible = AppManager.I.AppEdition.HasMultipleContentEditions;
            gameObject.SetActive(isVisible);

            if (EditionSelectionManager.MustChooseContentEditions)
            {
                // Auto-open switching panel if no player is detected
                SelectionManager.CompleteSelection();
            }
        }

    }
}
