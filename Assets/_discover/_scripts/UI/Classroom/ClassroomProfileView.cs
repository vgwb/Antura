using System;
using DG.DeInspektor.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Minigames.DiscoverCountry
{
    public class ClassroomProfileView : MonoBehaviour
    {
        #region Serialized

        [DeEmptyAlert]
        [SerializeField] Image profileImg;
        [DeEmptyAlert]
        [SerializeField] TMP_Text tfName;
        [DeEmptyAlert]
        [SerializeField] Button btEdit;

        #endregion

        #region Public Methods

        public void Fill(UserProfile profile)
        {
            profileImg.sprite = profile.ProfilePic;
            tfName.text = profile.Name;
        }

        #endregion
    }
}