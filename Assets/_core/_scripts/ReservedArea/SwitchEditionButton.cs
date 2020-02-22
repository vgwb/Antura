using Antura.Core;
using Antura.Language;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    public class SwitchEditionButton : MonoBehaviour
    {
        public Image iconImage;
        public TextRender nameText;

        public SwitchEditionPanel switchEditionPanel;
        private AppEditions edition;

        public Image selectedImage;
        public AppEditions Edition => edition;

        public void Setup(AppEditions _edition)
        {
            this.edition = _edition;

            iconImage.sprite = AppManager.I.Edition.EditionIcon;
            nameText.text = AppManager.I.Edition.EditionTitle;
        }

        public void OnClick()
        {
            AppManager.I.AppSettingsManager.SetLoadedEdition(edition);
            switchEditionPanel.RefreshSelection();
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