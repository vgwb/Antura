using Antura.Core;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Antura.UI
{
    public class SwitchEditionPanel : MonoBehaviour
    {
        public CurrentEditionIcon currentIcon;

        public SwitchLearningEditionButton prefabButton;
        private List<SwitchLearningEditionButton> buttons = new List<SwitchLearningEditionButton>();

        public void Open()
        {
            GlobalUI.ShowPauseMenu(false);
            gameObject.SetActive(true);
        }

        public void Close()
        {
            GlobalUI.ShowPauseMenu(true);
            gameObject.SetActive(false);
        }

        public void Awake()
        {
            for (var iLearningEdition = 0; iLearningEdition < AppManager.I.AppEdition.LearningEditions.Length; iLearningEdition++)
            {
                var config = AppManager.I.AppEdition.LearningEditions[iLearningEdition];
                var buttonGO = Instantiate(prefabButton.gameObject);
                buttonGO.transform.SetParent(prefabButton.transform.parent);
                buttonGO.transform.localScale = Vector3.one;
                buttonGO.SetActive(true);
                var button = buttonGO.GetComponent<SwitchLearningEditionButton>();
                button.Setup(iLearningEdition, config);
                buttons.Add(button);
            }

            prefabButton.gameObject.SetActive(false);
        }

        void OnEnable()
        {
            RefreshSelection();
        }

        public void RefreshSelection()
        {
            foreach (var button in buttons)
            {
                button.SetUnselected();
                if (button.LearningEditionIndex == AppManager.I.AppSettings.LearningEditionIndex) button.SetSelected();
            }
            currentIcon.OnEnable();
        }
    }
}