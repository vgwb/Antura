using Antura.Core;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Antura.UI
{
    public class SwitchEditionPanel : MonoBehaviour
    {
        public CurrentEditionIcon currentIcon;

        public SwitchEditionButton prefabButton;
        private List<SwitchEditionButton> buttons = new List<SwitchEditionButton>();

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
            foreach (var editionConfig in AppManager.I.ParentEdition.ChildEditions)
            {
                var edition = editionConfig.Edition;
                var buttonGO = Instantiate(prefabButton.gameObject);
                buttonGO.transform.SetParent(prefabButton.transform.parent);
                buttonGO.transform.localScale = Vector3.one;
                buttonGO.SetActive(true);
                var button = buttonGO.GetComponent<SwitchEditionButton>();
                button.Setup(editionConfig);
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
                if (button.Edition == AppManager.I.AppSettings.SpecificEdition) button.SetSelected();
            }
            currentIcon.OnEnable();
        }
    }
}