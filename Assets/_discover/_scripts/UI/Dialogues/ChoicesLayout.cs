using Demigiant.DemiTools;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Minigames.DiscoverCountry
{
    public class ChoicesLayout : MonoBehaviour
    {
        public enum ChoicesType
        {
            Unset,
            Text,
            Image
        }

        #region EVENTS

        public readonly ActionEvent<int> OnConfirmChoice = new("ChoicesLayout.OnConfirmChoice");

        #endregion

        #region Serialized

        public ChoicesType Type;

        #endregion

        public bool interactable {
            get { return allButtons[0].interactable; }
            set { SetInteractable(value); }
        }
        
        Button[] allButtons;

        #region Unity

        void Awake()
        {
            allButtons = this.GetComponentsInChildren<Button>();
        }

        #endregion

        #region Methods

        void SetInteractable(bool interactable)
        {
            foreach (Button bt in allButtons) bt.interactable = interactable;
        }

        #endregion
    }
}