using DG.DeInspektor.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Minigames.DiscoverCountry
{
    public class ClassroomProfileLevelSectionView : MonoBehaviour
    {
        #region Serialized

        [DeEmptyAlert]
        [SerializeField] TMP_Text tfTitle;
        [DeEmptyAlert]
        [SerializeField] Slider slider;

        #endregion

        #region Public Methods

        public void Fill(UserProfileDetail.LanguageLevelSection section)
        {
            tfTitle.text = section.Name;
            slider.value = section.CompletionPerc;
        }

        #endregion
    }
}