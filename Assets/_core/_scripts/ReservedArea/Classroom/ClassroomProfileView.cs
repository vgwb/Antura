using System;
using Antura.Profile;
using DG.DeExtensions;
using DG.DeInspektor.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    public class ClassroomProfileView : MonoBehaviour
    {
        #region Serialized

        [DeEmptyAlert]
        [SerializeField] PlayerIcon playerIcon;
        [DeEmptyAlert]
        [SerializeField] TMP_Text tfName;
        [DeEmptyAlert]
        [SerializeField] Button btMain;

        #endregion

        public PlayerProfilePreview Profile { get; private set; }
        public Button BtMain => btMain;

        #region Public Methods

        public void Fill(PlayerProfilePreview profile)
        {
            Profile = profile;
            playerIcon.Init(profile);
            tfName.text = profile.PlayerName.IsNullOrEmpty() ? "- - -" : profile.PlayerName;
        }

        #endregion
    }
}
