using Antura.Profile;
using DG.DeInspektor.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    public class ClassroomProfileQuestView : MonoBehaviour
    {
        #region Serialized

        [DeEmptyAlert]
        [SerializeField] TMP_Text tfTitle;
        [DeEmptyAlert]
        [SerializeField] Image[] stars;
        [SerializeField] Color inactiveStarColor = Color.gray;
        [SerializeField] Color activeStarColor = Color.yellow;

        #endregion

        #region Public Methods

        public void Fill(ClassroomProfileDetail.DiscoverQuest quest)
        {
            tfTitle.text = quest.Name;
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].color = i < quest.Stars ? activeStarColor : inactiveStarColor;
            }
        }

        #endregion
    }
}