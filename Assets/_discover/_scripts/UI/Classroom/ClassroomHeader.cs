using DG.DeInspektor.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Minigames.DiscoverCountry
{
    public class ClassroomHeader : MonoBehaviour
    {
        #region Serialized

        [DeEmptyAlert]
        public Button BtClassroom;
        [DeEmptyAlert]
        public Button BtLanguage;
        [DeEmptyAlert]
        public Button BtClose;
        [DeEmptyAlert]
        public TMP_Text TfClassId;

        #endregion
    }
}