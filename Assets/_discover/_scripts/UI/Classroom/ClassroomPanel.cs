using System;
using System.Collections.Generic;
using Antura.Core;
using Antura.Minigames.DiscoverCountry.Popups;
using Antura.ReservedArea;
using Antura.UI;
using DG.DeInspektor.Attributes;
using UnityEngine;
using Random = UnityEngine.Random;

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

        [SerializeField] bool hideGlobalUIBackButton = true;
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
        bool isValidClassroom;
        bool backButtonWasOn;
        State state = State.Unset;

        #region Unity

        void Start()
        {
            // Stub data
            List<string> classroomIDs = ReservedAreaManager.GetStubClassroomIDs();
            
            header.BtClassroom.onClick.AddListener(() => {
                GlobalPopups.OpenSelector("Choose classroom", classroomIDs, x => {
                    Open(classroomIDs[x], TestGenerateStubProfiles());
                }, true, 0);
            });
            header.BtClose.onClick.AddListener(() => {
                switch (state)
                {
                    case State.Profiles:
                        AppManager.I.NavigationManager.GoBack();
                        break;
                }
            });
            
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
            Close();
            
            isOpen = true;
            isValidClassroom = classroomId != UserProfile.NoClassroomId;
            AppManager.I.AppSettingsManager.SetClassroomMode(isValidClassroom ? 1 : 0);
            if (hideGlobalUIBackButton)
            {
                backButtonWasOn = GlobalUI.I.BackButton.gameObject.activeSelf;
                GlobalUI.I.BackButton.gameObject.SetActive(false);
            }
            header.SetTitle(classroomId != UserProfile.NoClassroomId, classroomId);
            SwitchState(State.Profiles);
            profilesPanel.Fill(profiles);
            this.gameObject.SetActive(true);
        }
        
        [DeMethodButton(mode = DeButtonMode.PlayModeOnly)]
        public void Close()
        {
            if (!isOpen) return;

            isOpen = false;
            if (hideGlobalUIBackButton && backButtonWasOn) GlobalUI.I.BackButton.gameObject.SetActive(true);
            SwitchState(State.Unset);
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
            header.ShowExtraButtons(toState == State.Profiles);
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

        public List<UserProfile> TestGenerateStubProfiles()
        {
            int tot = Random.Range(10, 31);
            List<UserProfile> testProfiles = new();
            for (int i = 0; i < tot; i++)
            {
                testProfiles.Add(new UserProfile("StubID", $"User [{i}]", sampleProfileSprite, DateTime.Now));
            }
            return testProfiles;
        }

        [DeMethodButton(mode = DeButtonMode.PlayModeOnly)]
        void TestOpen()
        {
            if (isOpen) return;
            
            Open("C", TestGenerateStubProfiles());
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