using DG.DeInspektor.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Minigames.DiscoverCountry
{
    public class ClassroomProfileDetailPanel : MonoBehaviour
    {
        #region Serialized

        [DeEmptyAlert]
        [SerializeField] Image profileImg;
        [DeEmptyAlert]
        [SerializeField] TMP_Text tfName;
        [DeEmptyAlert]
        [SerializeField] ClassroomProfileLevelView levelViewPrefab;
        [DeEmptyAlert]
        [SerializeField] ClassroomProfileQuestView questViewPrefab;

        #endregion
    }
}