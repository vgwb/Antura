using DG.DeInspektor.Attributes;
using TMPro;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class ClassroomProfileLevelView : MonoBehaviour
    {
        #region Serialized

        [DeEmptyAlert]
        [SerializeField] TMP_Text tfTitle;
        [DeEmptyAlert]
        [SerializeField] ClassroomProfileLevelSectionView sectionPrefab;

        #endregion
    }
}