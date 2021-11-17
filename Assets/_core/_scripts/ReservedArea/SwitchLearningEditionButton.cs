using System.Collections;
using Antura.Core;
using Antura.Language;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    public class SwitchLearningEditionButton : MonoBehaviour
    {
        public Image iconImage;
        public TextRender nameText;

        public SwitchEditionPanel switchEditionPanel;
        private int learningEditionIndex;

        public Image selectedImage;
        public int LearningEditionIndex => learningEditionIndex;

        public void Setup(int index, LearningConfig _editionConfig)
        {
            this.learningEditionIndex = index;

            iconImage.sprite = _editionConfig.Icon;
            nameText.text = _editionConfig.Title;
        }

        public void OnClick()
        {
            StartCoroutine(OnClickCO());
        }

        private IEnumerator OnClickCO()
        {
            AppManager.I.AppSettingsManager.SetLearningEditionIndex(learningEditionIndex);
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