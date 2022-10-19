using System.Collections;
using Antura.Audio;
using Antura.Core;
using Antura.Database;
using Antura.Language;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    public class SelectNativeLanguageButton : MonoBehaviour
    {
        public TextRender nameText;
        public TextRender Label;
        public UIButton button;
        public Image Flag;

        public SelectNativeLanguagePanel parentPanel;

        private LanguageCode languageCode;
        public LanguageCode LanguageCode => languageCode;

        public void Setup(LanguageCode languageCode)
        {
            this.languageCode = languageCode;
            nameText.SetOverridenLanguageText(languageCode, LocalizationDataId.Language_Name);
            Label.SetText(languageCode.ToString());
            var langConfig = AppManager.I.LanguageSwitcher.GetLangConfig(languageCode);
            Flag.sprite = langConfig.FlagIcon;
        }

        public void OnClick()
        {
            parentPanel.SelectedButton = this;
            parentPanel.ConfirmSelection(languageCode);
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
