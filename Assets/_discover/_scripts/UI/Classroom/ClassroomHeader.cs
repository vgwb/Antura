using DG.DeInspektor.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Minigames.DiscoverCountry
{
    public class ClassroomHeader : MonoBehaviour
    {
        #region Serialized

        [DeEmptyAlert]
        [SerializeField] Button btLanguage;
        [DeEmptyAlert]
        [SerializeField] Button btClose;

        #endregion
        
        public Button BtLanguage => btLanguage;
        public Button BtClose => btClose;
    }
}