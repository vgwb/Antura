using System.Collections;
using Antura.Core;
using Antura.Language;
using Antura.Scenes;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    public class SwitchLearningEditionButton : MonoBehaviour
    {
        public Image iconImage;
        public TextRender nameText;

        public SwitchEditionPanel switchEditionPanel;
        private LearningContentID contentId;

        public Image selectedImage;
        public LearningContentID ContentId => contentId;

        public void Setup(int index, ContentEditionConfig editionEditionConfig)
        {
            this.contentId = editionEditionConfig.ContentID;

            iconImage.sprite = editionEditionConfig.TransitionLogo;
            nameText.SetText(editionEditionConfig.Title);
        }

        public void OnClick()
        {
            StartCoroutine(OnClickCO());
        }

        private IEnumerator OnClickCO()
        {
            HomeScene.HasSelectedLearningEdition = true;
            AppManager.I.AppSettingsManager.SetLearningContentID(contentId);
            yield return AppManager.I.ReloadEdition();
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
