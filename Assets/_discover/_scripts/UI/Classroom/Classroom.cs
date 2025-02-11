using System;
using System.Collections.Generic;
using DG.DeInspektor.Attributes;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class Classroom : MonoBehaviour
    {
        #region Serialized

        [DeEmptyAlert]
        [SerializeField] ClassroomHeader header;
        [DeEmptyAlert]
        [SerializeField] ClassroomProfilesPanel profilesPanel;
        [DeEmptyAlert]
        [SerializeField] ClassroomProfile profile;
        [DeEmptyAlert]
        [SerializeField] ClassroomProfileDetailPanel detailPanel;
        
        [Header("Test stuff")]
        [SerializeField] Sprite sampleProfilePic;

        #endregion

        #region Public Methods

        public void Open(string classroomId, List<UserProfile> profiles)
        {
            
        }
        
        #endregion

        #region Methods

        [DeMethodButton(mode = DeButtonMode.PlayModeOnly)]
        void TestOpen()
        {
            List<UserProfile> testProfiles = new();
            for (int i = 0; i < 10; i++)
            {
                UserProfile profile = new UserProfile("StubID", "User XXX", sampleProfilePic, DateTime.Now);
            }
        }
        
        void Close()
        {
            
        }

        #endregion
    }
}