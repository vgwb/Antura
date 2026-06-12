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
    public class HomeNativeLanguageSelectorBtn : MonoBehaviour
    {
        public Image Flag;
        public EditionSelectionManager SelectionManager;

        void Start()
        {
            var languageCode = AppManager.I.AppSettings.NativeLanguage;
            var langConfig = AppManager.I.LanguageManager.GetLangConfig(languageCode);
            Flag.sprite = langConfig.FlagIcon;
            if (EditionSelectionManager.MustChooseContentEditions)
            {
                // Auto-open switching panel if no player is detected
                SelectionManager.CompleteSelection();
            }
        }

    }
}
