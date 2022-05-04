using System;
using Antura.Core;
using System.Collections;
using System.Collections.Generic;
using Antura.Database;
using Antura.Scenes;
using Antura.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace Antura.Utilities
{
    public class EditionSelectorBtn : MonoBehaviour
    {
        public TextRender TextRender;
        public EditionSelectionManager SelectionManager;

        void Start()
        {
            var locKeyText = $"Learn_{AppManager.I.ContentEdition.LearningLanguage}";
            var locKey = Enum.Parse<LocalizationDataId>(locKeyText, true);
            TextRender.SetOverridenLanguageText(AppManager.I.AppSettings.NativeLanguage, locKey);

            bool isVisible = AppManager.I.AppEdition.HasMultipleContentEditions;
            gameObject.SetActive(isVisible);

            if (EditionSelectionManager.MustChooseContentEditions)
            {
                // Auto-open switching panel if no player is detected
                SelectionManager.CompleteSelection(firstTime:true);
            }
        }

    }
}
