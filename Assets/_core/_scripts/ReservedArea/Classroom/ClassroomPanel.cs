using System;
using System.Collections;
using System.Collections.Generic;
using Antura.Core;
using Antura.Profile;
using Antura.Scenes;
using DG.DeInspektor.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        [DeEmptyAlert]
        [SerializeField] GameObject createProfileBgBlocker;
        
        #endregion

        public const string NoClassroomId = "-";
        readonly List<string> classroomIDs = new() {NoClassroomId, "A", "B", "C", "D", "E", "F"};
        State state = State.Unset;
        bool isOpen;
        int currClassroomIndex;
        bool isValidClassroom;
        bool backButtonWasOn;
        List<PlayerIconData> allProfiles;
        readonly Dictionary<int, List<PlayerIconData>> profilesByClassroomIndex = new();
        Coroutine coCreateProfile;

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
            profilesPanel.BtCreateProfile.onClick.AddListener(CreateProfile);
            
            createProfileBgBlocker.gameObject.SetActive(false);
            if (!isOpen) this.gameObject.SetActive(false);

            profilesPanel.OnProfileClicked.Subscribe(OnProfileClicked);
            detailPanel.OnBackClicked.Subscribe(OnBackFromProfileDetailsClicked);
        }

        void OnDestroy()
        {
            this.StopAllCoroutines();
            profilesPanel.OnProfileClicked.Unsubscribe(OnProfileClicked);
            detailPanel.OnBackClicked.Unsubscribe(OnBackFromProfileDetailsClicked);
            PlayerCreationScene.OnCreationComplete.Unsubscribe(OnPlayerCreationComplete);
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
            currClassroomIndex = classroomIndex;
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
        
        void CreateProfile()
        {
            this.RestartCoroutine(ref coCreateProfile, CO_CreateProfile());
        }

        IEnumerator CO_CreateProfile()
        {
            createProfileBgBlocker.gameObject.SetActive(true);
            yield return null;
            
            AppManager.I.NavigationManager.GoToSceneByName(SceneHelper.GetSceneName(AppScene.PlayerCreation), LoadSceneMode.Additive, true);
            PlayerCreationScene.OnCreationComplete.Unsubscribe(OnPlayerCreationComplete);
            PlayerCreationScene.OnCreationComplete.Subscribe(OnPlayerCreationComplete);
            
            coCreateProfile = null;
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

        void OnPlayerCreationComplete()
        {
            SceneManager.UnloadSceneAsync(SceneHelper.GetSceneName(AppScene.PlayerCreation));
            createProfileBgBlocker.SetActive(false);
            Refresh();
            Open(currClassroomIndex);
        }

        #endregion
    }
}