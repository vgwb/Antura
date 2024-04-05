using DG.DeInspektor.Attributes;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class DialoguesUI : MonoBehaviour
    {
        #region Serialized

        [DeEmptyAlert]
        [SerializeField] DialogueSignal signal;

        #endregion
        
        #region Public Methods

        public void ShowDialogueSignalFor(EdLivingLetter ll)
        {
            signal.ShowFor(ll);
        }

        public void HideDialogueSignal()
        {
            signal.Hide();
        }

        #endregion
    }
}