using System;
using System.Collections.Generic;
using DG.DeInspektor.Attributes;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class ClassroomPanel : MonoBehaviour
    {
        #region Serialized

        [DeEmptyAlert]
        [SerializeField] ClassroomHeader header;
        [DeEmptyAlert]
        [SerializeField] ClassroomProfilesPanel profilesPanel;
        [DeEmptyAlert]
        [SerializeField] ClassroomProfileDetailPanel detailPanel;
        
        [Header("Test stuff")]
        [SerializeField] Sprite sampleProfileSprite;

        #endregion

        #region Unity

        void Start()
        {
            this.gameObject.SetActive(false);
        }

        #endregion

        #region Public Methods

        public void Open(string classroomId, List<UserProfile> profiles)
        {
            profilesPanel.Fill(profiles);
            this.gameObject.SetActive(true);
        }
        
        #endregion

        #region Methods

        void Close()
        {
            this.gameObject.SetActive(false);
        }

        #endregion

        #region Test

        [DeMethodButton(mode = DeButtonMode.PlayModeOnly)]
        void TestOpen()
        {
            List<UserProfile> testProfiles = new();
            for (int i = 0; i < 20; i++)
            {
                testProfiles.Add(new UserProfile("StubID", "User XXX", sampleProfileSprite, DateTime.Now));
            }
            Open("C", testProfiles);
        }

        #endregion
    }
}