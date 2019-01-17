using Antura.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Test
{
    /// <summary>
    /// Helper class to test the MinigamesUIPanel
    /// <seealso cref="MinigamesUI"/>
    /// </summary>
    public class Tester_MinigamesUIPanel : MonoBehaviour
    {
        public enum UIPanelType
        {
            Unset,
            Timer,
            Lives,
            Starbar
        }

        public UIPanelType PanelType;
        public Button[] DisabledUntilSetup;

        Button[] bts;

        #region Unity

        void Start()
        {
            bts = this.GetComponentsInChildren<Button>(true);
            foreach (Button button in bts) {
                Button bt = button;
                bt.onClick.AddListener(Refresh);
            }
        }

        void OnDestroy()
        {
            foreach (Button bt in bts) bt.onClick.RemoveAllListeners();
        }

        #endregion

        #region Public Methods

        public void Refresh()
        {
            bool isTargetUISetup = false;
            switch (PanelType) {
            case UIPanelType.Timer:
                isTargetUISetup = MinigamesUI.Timer.IsSetup;
                break;
            case UIPanelType.Lives:
                isTargetUISetup = MinigamesUI.Lives.IsSetup;
                break;
            default:
                return;
            }

            foreach (Button bt in DisabledUntilSetup) {
                bt.interactable = isTargetUISetup;
            }
        }

        #endregion
    }
}