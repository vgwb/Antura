using Antura.Audio;
using Antura.Core;
using Antura.Database;
using Antura.Discover;
using Antura.Profile;
using Antura.Scenes;
using Antura.Teacher;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.DeInspektor.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Antura.UI
{
    public class ClassroomPanel : MonoBehaviour
    {
        enum State
        {
            Unset,
            Profiles,
            ProfileDetail,
            Options,
            Info
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
        [SerializeField] ClassroomOptionsPanel optionsPanel;
        [DeEmptyAlert]
        [SerializeField] ClassroomInfoPanel infoPanel;
        [DeEmptyAlert]
        [SerializeField] Button btChangeClass;
        [DeEmptyAlert]
        [SerializeField] Button btCreateTeacher;
        [DeEmptyAlert]
        [SerializeField] GameObject createProfileBgBlocker;
        [DeEmptyAlert]
        [SerializeField] GameObject pleaseWaitPanel;

        #endregion

        const string noClassroomId = "-";
        readonly List<string> classroomIDs = new() { noClassroomId, "A", "B", "C", "D", "E", "F" };
        State state = State.Unset;
        bool isOpen;
        int currClassroomIndex = 0;
        bool isValidClassroom;
        bool backButtonWasOn;
        List<PlayerProfilePreview> allProfiles;
        readonly Dictionary<int, List<PlayerProfilePreview>> profilesByClassroomIndex = new();
        Coroutine coCreateProfile;

        #region Unity

        void Start()
        {
            Refresh();

            btChangeClass.onClick.AddListener(() => OpenSelectClassroomPopup());
            header.BtClose.onClick.AddListener(() =>
            {
                AppManager.I.NavigationManager.GoBack();
            });
            btCreateTeacher.onClick.AddListener(() => CreateProfile(true));
            header.BtClass.onToggleOn.AddListener(() => SwitchState(State.Profiles));
            header.BtOptions.onToggleOn.AddListener(() => SwitchState(State.Options));
            header.BtInfo.onToggleOn.AddListener(() => SwitchState(State.Info));
            profilesPanel.BtCreateProfile.onClick.AddListener(() => CreateProfile(false));

            optionsPanel.gameObject.SetActive(false);
            infoPanel.gameObject.SetActive(false);
            createProfileBgBlocker.gameObject.SetActive(false);
            if (!isOpen)
                this.gameObject.SetActive(false);

            profilesPanel.OnProfileClicked.Subscribe(OnProfileClicked);
            detailPanel.OnBackClicked.Subscribe(OnBackFromProfileDetailsClicked);
            detailPanel.OnDeleteProfileRequested.Subscribe(OnDeleteProfileRequested);
            detailPanel.OnEditProfileRequested.Subscribe(OnEditProfileRequested);
        }

        void OnDestroy()
        {
            this.StopAllCoroutines();
            profilesPanel.OnProfileClicked.Unsubscribe(OnProfileClicked);
            detailPanel.OnBackClicked.Unsubscribe(OnBackFromProfileDetailsClicked);
            detailPanel.OnDeleteProfileRequested.Unsubscribe(OnDeleteProfileRequested);
            detailPanel.OnEditProfileRequested.Unsubscribe(OnEditProfileRequested);
            PlayerCreationScene.OnCreationComplete.Unsubscribe(OnPlayerCreationComplete);
        }

        #endregion

        #region Public Methods

        public void OpenSelectClassroomPopup(bool showCloseButton = true)
        {
            var popup_title = LocalizationManager.GetNewLocalized("profile.chooseclasse");
            GlobalPopups.OpenSelector(popup_title, classroomIDs, OpenClass, showCloseButton, currClassroomIndex);
        }

        public void OpenClass(int classroomIndex)
        {
            Close();

            isOpen = true;
            currClassroomIndex = classroomIndex;
            if (currClassroomIndex > 0)
            {
                isValidClassroom = true;
                AudioManager.I.StopMusic();
                AppManager.I.AppSettingsManager.SaveMusicSetting(false);
            }

            this.gameObject.SetActive(true);
            AppManager.I.AppSettingsManager.SetClassroomMode(currClassroomIndex);
            if (hideGlobalUIBackButton)
            {
                backButtonWasOn = GlobalUI.I.BackButton.gameObject.activeSelf;
                GlobalUI.I.BackButton.gameObject.SetActive(false);
            }
            header.SetTitle(isValidClassroom, classroomIDs[currClassroomIndex]);
            SwitchState(State.Profiles);
        }

        #endregion

        #region Methods

        void Refresh()
        {
            allProfiles = AppManager.I.PlayerProfileManager.GetPlayersIconData();
            profilesByClassroomIndex.Clear();
            foreach (PlayerProfilePreview profile in allProfiles)
            {
                if (profile.Classroom >= classroomIDs.Count)
                {
                    Debug.LogError($"Player \"{profile.PlayerName}\" has an invalid Classroom ID ({profile.Classroom}): should be between 0 and {classroomIDs.Count - 1}. Ignoring it");
                    continue;
                }
                if (!profilesByClassroomIndex.ContainsKey(profile.Classroom))
                    profilesByClassroomIndex.Add(profile.Classroom, new List<PlayerProfilePreview>());
                profilesByClassroomIndex[profile.Classroom].Add(profile);
            }
        }

        void SwitchState(State toState, PlayerProfilePreview? profile = null)
        {
            if (state == toState)
                return;
            if (toState == State.ProfileDetail && profile == null)
            {
                Debug.LogError($"ClassroomPanel: can't switch to {toState} state without passing profile parameter");
                return;
            }

            // DISCOVER INTEGRATION... we don't have any other method to check
            // selection here in the reserved Area :(
            Debug.Log("SwitchState profile.uuid " + (profile != null ? profile?.Uuid : "null"));
            Debug.Log("SwitchState DiscoverAppManager exists " + (DiscoverAppManager.I != null));
            if (!string.IsNullOrEmpty(profile?.Uuid))
            {
                if (DiscoverAppManager.I != null)
                {
                    DiscoverAppManager.I.InitializeFromLegacyUuid(profile?.Uuid);
                }
                else
                {
                    Debug.LogWarning("DiscoverAppManager.I is null when attempting to initialize from legacy UUID.");
                }
            }

            state = toState;
            profilesPanel.Open(toState == State.Profiles);
            detailPanel.Open(toState == State.ProfileDetail);
            optionsPanel.gameObject.SetActive(toState == State.Options);
            infoPanel.gameObject.SetActive(toState == State.Info);
            switch (state)
            {
                case State.Profiles:
                    Refresh();
                    header.BtClass.Toggle(true);
                    bool hasProfiles = profilesByClassroomIndex.ContainsKey(currClassroomIndex);
                    if (hasProfiles)
                        profilesPanel.Fill(profilesByClassroomIndex[currClassroomIndex]);
                    else
                        profilesPanel.Fill(new List<PlayerProfilePreview>());
                    break;
                case State.ProfileDetail:
                    header.BtClass.Toggle(true);
                    detailPanel.Fill((PlayerProfilePreview)profile);
                    break;
            }
        }

        void Close()
        {
            // if (!isOpen)
            //     return;

            isOpen = false;
            if (hideGlobalUIBackButton && backButtonWasOn)
                GlobalUI.I.BackButton.gameObject.SetActive(true);
            SwitchState(State.Unset);
            this.gameObject.SetActive(false);
        }

        void CreateProfile(bool createTeacher)
        {
            this.RestartCoroutine(ref coCreateProfile, CO_CreateProfile(createTeacher));
        }

        IEnumerator CO_CreateProfile(bool createTeacher)
        {
            createProfileBgBlocker.gameObject.SetActive(true);
            yield return null;
            yield return StartCoroutine(CreateClassroomPlayer(createTeacher));
            OnTeacherCreationComplete();

            // if (createTeacher)
            // {
            //     yield return null;
            //     yield return StartCoroutine(CreateClassroomPlayer());
            //     OnTeacherCreationComplete();
            // }
            // else
            // {
            //     yield return null;
            //     AppManager.I.NavigationManager.GoToSceneByName(SceneHelper.GetSceneName(AppScene.PlayerCreation), LoadSceneMode.Additive, true);
            //     PlayerCreationScene.OnCreationComplete.Unsubscribe(OnPlayerCreationComplete);
            //     PlayerCreationScene.OnCreationComplete.Subscribe(OnPlayerCreationComplete);
            // }

            coCreateProfile = null;
        }

        void OnTeacherCreationComplete()
        {
            createProfileBgBlocker.SetActive(false);
            OpenClass(currClassroomIndex);
        }

        void OnPlayerCreationComplete()
        {
            SceneManager.UnloadSceneAsync(SceneHelper.GetSceneName(AppScene.PlayerCreation));
            createProfileBgBlocker.SetActive(false);
            // here we must unlock it

            OpenClass(currClassroomIndex);
        }

        void DeleteProfile(string profileUuid)
        {
            AppManager.I.PlayerProfileManager.DeletePlayerProfile(profileUuid);
            SwitchState(State.Profiles);
            OpenClass(currClassroomIndex);
        }

        #endregion

        #region Callbacks

        void OnProfileClicked(PlayerProfilePreview profile)
        {

            SwitchState(State.ProfileDetail, profile);
        }

        void OnBackFromProfileDetailsClicked()
        {
            SwitchState(State.Profiles);
        }

        void OnDeleteProfileRequested(PlayerProfilePreview profile)
        {
            GlobalUI.ShowPrompt(id: Database.LocalizationDataId.UI_AreYouSure, _onYesCallback: () => DeleteProfile(profile.Uuid), _onNoCallback: () => { });
        }

        void OnEditProfileRequested(PlayerProfilePreview profile)
        {
            GlobalPopups.OpenTextInput("Edit profile name", profile.PlayerName, detailPanel.AssignNewProfileName);
        }


        #endregion

        #region Demo User Helpers

        IEnumerator CreateClassroomPlayer(bool createTeacher = true)
        {
            yield return null;
            ActivateWaitingScreen(true);
            yield return null;
            if (createTeacher)
            {
                AppManager.I.PlayerProfileManager.CreatePlayerProfile(
                    currClassroomIndex, true, 1,
                    PlayerGender.M, PlayerTint.Purple, Color.yellow, Color.red, Color.magenta, 4,
                            AppManager.I.AppEdition.editionID,
                            AppManager.I.ContentEdition.ContentID,
                            AppManager.I.AppEdition.AppVersion,
                            true);
                // SelectedPlayerId = demoUserUiid;
            }
            else
            {
                AppManager.I.PlayerProfileManager.CreatePlayerProfile(
                    currClassroomIndex, true, 7,
                    PlayerGender.M, PlayerTint.None, Color.yellow, Color.red, Color.white, 4,
                            AppManager.I.AppEdition.editionID,
                            AppManager.I.ContentEdition.ContentID,
                            AppManager.I.AppEdition.AppVersion,
                            false);
            }

            // Populate with complete data
            // Find all content editions with the current native language
            List<ContentConfig> allConfigs = new List<ContentConfig>();
            SelectLearningContentPanel.FindAllContentEditions(allConfigs, AppManager.I.AppSettings.NativeLanguage);

            foreach (ContentConfig config in allConfigs)
            {
                ContentProfile contentProfile = AppManager.I.PlayerProfileManager.GetContentProfile(config.ContentID);
                AppManager.I.NavigationManager.NavData.CurrentContent = contentProfile;

                yield return AppManager.I.ReloadEdition();

                JourneyPosition maxJourneyPos = AppManager.I.JourneyHelper.GetFinalJourneyPosition(considerEndSceneToo: true);
                yield return StartCoroutine(PopulateDatabaseWithUsefulDataCO(maxJourneyPos));

                AppManager.I.Player.SetMaxJourneyPosition(maxJourneyPos, true, true);
                AppManager.I.Player.ForcePreviousJourneyPosition(maxJourneyPos);
                AppManager.I.Player.SetCurrentJourneyPosition(1, 1, 1, true);
            }
            AppManager.I.Player.AddBones(500);

            AppManager.I.Player.SetFinalShown(isInitialising: true);
            AppManager.I.Player.HasFinishedTheGame = true;
            AppManager.I.Player.HasFinishedTheGameWithAllStars = true;
            AppManager.I.Player.HasMaxStarsInCurrentPlaySessions = true;

            AppManager.I.FirstContactManager.ForceToFinishedSequence();
            AppManager.I.FirstContactManager.ForceAllCompleted();
            AppManager.I.RewardSystemManager.UnlockAllPacks();

            ActivateWaitingScreen(false);
        }

        void ActivateWaitingScreen(bool status)
        {
            pleaseWaitPanel.gameObject.SetActive(status);
            // GlobalUI.I.BackButton.gameObject.SetActive(!status);
        }

        IEnumerator PopulateDatabaseWithUsefulDataCO(JourneyPosition targetPosition)
        {
            bool useBestScores = false;

            LogAI logAi = AppManager.I.Teacher.logAI;

            // Add some mood data
            //Debug.Log("Start adding mood scores");
            yield return null;

            // Add scores for all play sessions
            // Debug.Log("Start adding PS scores");
            yield return null;
            List<LogPlaySessionScoreParams> logPlaySessionScoreParamsList = new List<LogPlaySessionScoreParams>();
            IEnumerable<JourneyPosition> allJPs = AppManager.I.JourneyHelper.GetAllJourneyPositionsUpTo(targetPosition);
            foreach (var jp in allJPs)
            {
                if (jp.Stage <= targetPosition.Stage)
                {
                    int score = useBestScores ? AppConfig.MaxMiniGameScore : UnityEngine.Random.Range(AppConfig.MinMiniGameScore, AppConfig.MaxMiniGameScore);
                    logPlaySessionScoreParamsList.Add(new LogPlaySessionScoreParams(jp, score, 12f));
                    // Debug.Log("Add play session score for " + jp.Id);
                }
            }
            logAi.LogPlaySessionScores(0, logPlaySessionScoreParamsList, true);

            // Add scores for all minigames
            // Debug.Log("Start adding MiniGame scores");
            yield return null;
            List<LogMiniGameScoreParams> logMiniGameScoreParamses = new List<LogMiniGameScoreParams>();
            List<MiniGameInfo> allMiniGameInfo = AppManager.I.ScoreHelper.GetAllMiniGameInfo();
            for (int i = 0; i < allMiniGameInfo.Count; i++)
            {
                int score = useBestScores
                    ? AppConfig.MaxMiniGameScore
                    : AppConfig.MinMiniGameScore;
                logMiniGameScoreParamses.Add(new LogMiniGameScoreParams(JourneyPosition.InitialJourneyPosition,
                    allMiniGameInfo[i].data.Code, score, 12f));
                //Debug.Log("Add minigame score " + i);
            }
            logAi.LogMiniGameScores(0, logMiniGameScoreParamses, true);
            yield return null;

        }

        #endregion
    }
}
