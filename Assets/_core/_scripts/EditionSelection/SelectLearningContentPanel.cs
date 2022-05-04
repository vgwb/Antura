using System.Collections.Generic;
using System.Linq;
using Antura.Core;
using DG.Tweening;
using UnityEngine;

namespace Antura.UI
{
    public class SelectLearningContentPanel : MonoBehaviour
    {
        public SelectNativeLanguageButton SelectNativeLanguageButton;

        public SelectLearningContentButton prefabButton;
        private List<SelectLearningContentButton> buttons = new List<SelectLearningContentButton>();

        public void OnEnable()
        {
            HasPerformedSelection = false;

            foreach (var button in buttons)
                Destroy(button.gameObject);
            buttons.Clear();

            // Find all content editions with the current native language
            for (var iContentEdition = 0; iContentEdition < AppManager.I.AppEdition.ContentEditions.Length; iContentEdition++)
            {
                var contentEditionConfig = AppManager.I.AppEdition.ContentEditions[iContentEdition];
                bool isSupported = contentEditionConfig.SupportedNativeLanguages.Contains(AppManager.I.AppSettings.NativeLanguage);
                if (!isSupported) continue;

                var buttonGO = Instantiate(prefabButton.gameObject, prefabButton.transform.parent, true);
                buttonGO.transform.localScale = Vector3.one;
                buttonGO.SetActive(true);
                var button = buttonGO.GetComponent<SelectLearningContentButton>();
                button.Setup(contentEditionConfig);
                buttons.Add(button);
            }
            //ContinueButton.Bt.onClick.AddListener(ConfirmCurrentSelection);
            prefabButton.gameObject.SetActive(false);
        }

        public bool HasPerformedSelection;
        public void ConfirmSelection(LearningContentID contentId)
        {
            AppManager.I.AppSettingsManager.SetLearningContentID(contentId);
            RefreshSelection();
            HasPerformedSelection = true;
        }

        public void RefreshSelection()
        {
            foreach (var button in buttons)
            {
                if (button.ContentId == AppManager.I.AppSettings.ContentID)
                    button.SetSelected();
                else
                    button.SetUnselected();
            }
        }

        public RectTransform scrollRectTr;

        private bool isOpen;
        public void Open()
        {
            scrollRectTr.anchoredPosition = new Vector2(0, 500);
            scrollRectTr.DOAnchorPos(new Vector2(0, 0), 0.35f);

            SelectNativeLanguageButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(180, -100);
            SelectNativeLanguageButton.GetComponent<RectTransform>().DOAnchorPos(new Vector2(180, 70), 0.35f);

            gameObject.SetActive(true);
            isOpen = true;
        }

        public void Close()
        {
            gameObject.SetActive(false);
            isOpen = false;
        }

        public bool IsOpen()
        {
            return isOpen;
        }
    }
}
