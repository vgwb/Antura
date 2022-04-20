using System.Collections;
using Antura.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    public class SelectLearningContentButton : MonoBehaviour
    {
        public TextRender nameText;
        public UIButton button;
        public Image iconImage;

        public SelectLearningContentPanel parentPanel;

        private LearningContentID contentId;
        public LearningContentID ContentId => contentId;

        public void Setup(ContentEditionConfig editionEditionConfig)
        {
            this.contentId = editionEditionConfig.ContentID;

            iconImage.sprite = editionEditionConfig.TransitionLogo;
            nameText.SetText(editionEditionConfig.Title);
        }

        public void OnClick()
        {
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
