using System.Collections;
using Antura.Core;
using Antura.Language;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    public class SwitchLanguageButton : MonoBehaviour
    {
        public Image flagImage;
        public TextRender nameText;

        public SwitchLanguagePanel switchLanguagePanel;
        private LanguageCode langCode;

        public Image selectedImage;
        public LanguageCode Language => langCode;

        public void Setup(LanguageCode langCode)
        {
            this.langCode = langCode;

            flagImage.sprite = LanguageSwitcher.I.GetLangConfig(langCode).FlagIcon;
            nameText.text = LanguageSwitcher.I.GetLangConfig(langCode).LocalizedName;
        }

        public void OnClick()
        {
            StartCoroutine(OnClickCO());
        }
        private IEnumerator OnClickCO()
        {
            yield return AppManager.I.ResetLanguageSetup(langCode);
            switchLanguagePanel.RefreshSelection();
            AppManager.I.NavigationManager.GoToHome(true);
        }

        public void SetUnselected()
        {
            selectedImage.enabled = false;
        }
        public void SetSelected()
        {
            selectedImage.enabled = true;
        }
    }
}
