using System;
using System.Collections.Generic;
using DG.DeInspektor.Attributes;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class ClassroomPanel : MonoBehaviour
    {
        enum State
        {
            Unset,
            Profiles,
            ProfileDetail
        }
        
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

        bool isOpen;
        State state = State.Unset;

        #region Unity

        void Start()
        {
            header.BtClose.onClick.AddListener(Close);
            
            if (!isOpen) this.gameObject.SetActive(false);

            profilesPanel.OnProfileClicked.Subscribe(OnProfileClicked);
            detailPanel.OnBackClicked.Subscribe(OnBackFromProfileDetailsClicked);
        }

        void OnDestroy()
        {
            profilesPanel.OnProfileClicked.Unsubscribe(OnProfileClicked);
            detailPanel.OnBackClicked.Unsubscribe(OnBackFromProfileDetailsClicked);
        }

        #endregion

        #region Public Methods

        public void Open(string classroomId, List<UserProfile> profiles)
        {
            if (isOpen) return;
            
            isOpen = true;
            SwitchState(State.Profiles);
            profilesPanel.Fill(profiles);
            this.gameObject.SetActive(true);
        }
        
        [DeMethodButton(mode = DeButtonMode.PlayModeOnly)]
        public void Close()
        {
            if (!isOpen) return;

            isOpen = false;
            this.gameObject.SetActive(false);
        }
        
        #endregion

        #region Methods

        void SwitchState(State toState, UserProfile profile = null)
        {
            if (state == toState) return;
            if (toState == State.ProfileDetail && profile == null)
            {
                Debug.LogError($"ClassroomPanel: can't switch to {toState} state without passing profile parameter");
                return;
            }
            
            state = toState;
            profilesPanel.Open(toState == State.Profiles);
            detailPanel.Open(toState == State.ProfileDetail);
            switch (state)
            {
                case State.ProfileDetail:
                    detailPanel.Fill(profile.GetProfileDetail());
                    break;
            }
        }

        #endregion

        #region Test

        [DeMethodButton(mode = DeButtonMode.PlayModeOnly)]
        void TestOpen()
        {
            if (isOpen) return;
            
            List<UserProfile> testProfiles = new();
            for (int i = 0; i < 20; i++)
            {
                testProfiles.Add(new UserProfile("StubID", $"User [{i}]", sampleProfileSprite, DateTime.Now));
            }
            Open("C", testProfiles);
        }

        #endregion

        #region Callbacks

        void OnProfileClicked(UserProfile profile)
        {
            SwitchState(State.ProfileDetail, profile);
        }
        
        void OnBackFromProfileDetailsClicked()
        {
            SwitchState(State.Profiles);
        }

        #endregion
    }
}