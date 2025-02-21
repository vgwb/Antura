using System;
using System.Collections.Generic;
using Antura.Core;
using Antura.Profile;
using DG.DeInspektor.Attributes;
using UnityEngine;

namespace Antura.UI
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

        public const string NoClassroomId = "-";
        readonly List<string> classroomIDs = new() {NoClassroomId, "A", "B", "C", "D", "E", "F"};
        State state = State.Unset;
        bool isOpen;
        bool isValidClassroom;
        bool backButtonWasOn;
        List<PlayerIconData> allProfiles;
        readonly Dictionary<int, List<PlayerIconData>> profilesByClassroomIndex = new();

        #region Unity

        void Start()
        {
            Refresh();
            
            header.BtClassroom.onClick.AddListener(() => OpenSelectClassroomPopup());
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

        public void OpenSelectClassroomPopup(bool showCloseButton = true)
        {
            GlobalPopups.OpenSelector("Choose classroom", classroomIDs, Open, showCloseButton, 0);
        }

        public void Open(int classroomIndex)
        {
            Close();
            
            isOpen = true;
            isValidClassroom = classroomIndex > 0;
            this.gameObject.SetActive(true);
            AppManager.I.AppSettingsManager.SetClassroomMode(isValidClassroom ? 1 : 0);
            if (hideGlobalUIBackButton)
            {
                backButtonWasOn = GlobalUI.I.BackButton.gameObject.activeSelf;
                GlobalUI.I.BackButton.gameObject.SetActive(false);
            }
            header.SetTitle(isValidClassroom, classroomIDs[classroomIndex]);
            SwitchState(State.Profiles);
            bool hasProfiles = profilesByClassroomIndex.ContainsKey(classroomIndex);
            if (hasProfiles) profilesPanel.Fill(profilesByClassroomIndex[classroomIndex]);
            else profilesPanel.Fill(new List<PlayerIconData>());
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

        void Refresh()
        {
            allProfiles = AppManager.I.PlayerProfileManager.GetPlayersIconData();
            profilesByClassroomIndex.Clear();
            foreach (PlayerIconData profile in allProfiles)
            {
                if (profile.Classroom >= classroomIDs.Count)
                {
                    Debug.LogError($"Player \"{profile.PlayerName}\" has an invalid Classroom ID ({profile.Classroom}): should be between 0 and {classroomIDs.Count - 1}. Ignoring it");
                    continue;
                }
                if (!profilesByClassroomIndex.ContainsKey(profile.Classroom)) profilesByClassroomIndex.Add(profile.Classroom, new List<PlayerIconData>());
                profilesByClassroomIndex[profile.Classroom].Add(profile);
            }
        }

        void SwitchState(State toState, PlayerIconData? profile = null)
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
                    detailPanel.Fill(new ClassroomProfileDetail((PlayerIconData)profile));
                    break;
            }
        }

        #endregion

        #region Callbacks

        void OnProfileClicked(PlayerIconData profile)
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