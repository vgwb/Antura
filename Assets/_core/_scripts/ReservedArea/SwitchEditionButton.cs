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

        public void Setup(EditionConfig _editionConfig)
        {
            this.edition = _editionConfig.Edition;

            iconImage.sprite = _editionConfig.EditionIcon;
            nameText.text = _editionConfig.EditionTitle;
        }

        public void OnClick()
        {
            AppManager.I.AppSettingsManager.SetSpecificEdition(edition);
            AppManager.I.ReloadEdition();
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