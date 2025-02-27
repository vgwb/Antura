using DG.DeInspektor.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    public class ClassroomHeader : MonoBehaviour
    {
        #region Serialized

        [SerializeField] string noClassroomTitle = "NO CLASSROOM";
        [SerializeField] string classroomTitle = "CLASSROOM";
        [SerializeField] string idPrefix = "<size=140%><color=#e9c43f>";
        [SerializeField] string idSuffix = "</color></size>";
        
        [Header("References")]
        [DeEmptyAlert]
        public Button BtClassroom;
        [DeEmptyAlert]
        public Button BtClose;
        [DeEmptyAlert]
        public Button BtCreateTeacher;
        [DeEmptyAlert]
        [SerializeField] Button btOptions;
        [DeEmptyAlert]
        [SerializeField] Button btInfo;
        [DeEmptyAlert]
        [SerializeField] TMP_Text tfTitle;

        #endregion

        #region Public Methods

        public void SetTitle(bool validClassroom, string id)
        {
            tfTitle.text = validClassroom ? $"{classroomTitle} {idPrefix}{id}{idSuffix}" : noClassroomTitle;
        }

        public void ShowExtraButtons(bool doShow)
        {
            BtCreateTeacher.gameObject.SetActive(doShow);
            btOptions.gameObject.SetActive(doShow);
            btInfo.gameObject.SetActive(doShow);
            BtClose.gameObject.SetActive(doShow);
        }

        #endregion
    }
}