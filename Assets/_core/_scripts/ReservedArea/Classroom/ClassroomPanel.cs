using Antura.Audio;
using Antura.Core;
using Antura.Database;
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
        int currClassroomIndex;
        bool isValidClassroom;
        bool backButtonWasOn;
        bool isCreatingDemoProfile;
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
                switch (state)
                {
                    case State.Profiles:
                        AppManager.I.NavigationManager.GoBack();
                        break;
                }
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
            GlobalPopups.OpenSelector(popup_title, classroomIDs, OpenClass, showCloseButton, 0);
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
            if (!isOpen)
                return;

            isOpen = false;
            if (hideGlobalUIBackButton && backButtonWasOn)
                GlobalUI.I.BackButton.gameObject.SetActive(true);
            SwitchState(State.Unset);
            this.gameObject.SetActive(false);
        }

        // Demo player also means teacher
        void CreateProfile(bool isDemoPlayer)
        {
            isCreatingDemoProfile = isDemoPlayer;
            this.RestartCoroutine(ref coCreateProfile, CO_CreateProfile(isDemoPlayer));
        }

        IEnumerator CO_CreateProfile(bool isDemoPlayer)
        {
            if (isDemoPlayer)
            {
                if (AppManager.I.PlayerProfileManager.IsDemoUserExisting())
                    GlobalUI.ShowPrompt(id: Database.LocalizationDataId.ReservedArea_DemoUserAlreadyExists);
                else
                {
                    createProfileBgBlocker.gameObject.SetActive(true);
                    yield return null;
                    yield return StartCoroutine(CreateDemoPlayer());
                    OnPlayerCreationComplete();
                }
            }
            else
            {
                createProfileBgBlocker.gameObject.SetActive(true);
                yield return null;
                AppManager.I.NavigationManager.GoToSceneByName(SceneHelper.GetSceneName(AppScene.PlayerCreation), LoadSceneMode.Additive, true);
                PlayerCreationScene.OnCreationComplete.Unsubscribe(OnPlayerCreationComplete);
                PlayerCreationScene.OnCreationComplete.Subscribe(OnPlayerCreationComplete);
            }

            coCreateProfile = null;
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

        void OnPlayerCreationComplete()
        {
            if (!isCreatingDemoProfile)
            {
                // Normal player creation
                SceneManager.UnloadSceneAsync(SceneHelper.GetSceneName(AppScene.PlayerCreation));
            }
            createProfileBgBlocker.SetActive(false);
            OpenClass(currClassroomIndex);
        }

        #endregion

        #region Demo User Helpers

        // This is never updated so it's useless. Leaving it here only for safety because it refers to old Antura code
        static bool testAlmostAtEnd = false;

        IEnumerator CreateDemoPlayer()
        {
            //Debug.Log("creating DEMO USER ");
            yield return null;
            ActivateWaitingScreen(true);
            yield return null;
            string demoUserUiid = AppManager.I.PlayerProfileManager.CreatePlayerProfile(0, true, 1, PlayerGender.M, PlayerTint.Purple, Color.yellow, Color.red, Color.magenta, 4,
                        AppManager.I.AppEdition.editionID,
                        AppManager.I.ContentEdition.ContentID,
                        AppManager.I.AppEdition.AppVersion,
                        true);
            // SelectedPlayerId = demoUserUiid;

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
                if (testAlmostAtEnd)
                    maxJourneyPos = new JourneyPosition(6, 13, 100); // Never TRUE
                yield return StartCoroutine(PopulateDatabaseWithUsefulDataCO(maxJourneyPos));

                AppManager.I.Player.SetMaxJourneyPosition(maxJourneyPos, true, true);
                AppManager.I.Player.ForcePreviousJourneyPosition(maxJourneyPos);
            }
            AppManager.I.Player.AddBones(500);

            if (!testAlmostAtEnd) // Always FALSE
            {
                AppManager.I.Player.SetFinalShown(isInitialising: true);
                AppManager.I.Player.HasFinishedTheGame = true;
                AppManager.I.Player.HasFinishedTheGameWithAllStars = true;
                AppManager.I.Player.HasMaxStarsInCurrentPlaySessions = true;
            }
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
            bool useBestScores = true;

            LogAI logAi = AppManager.I.Teacher.logAI;

            // Add some mood data
            Debug.Log("Start adding mood scores");
            yield return null;
            /*int nMoodData = 15; // @note: not needed
            for (int i = 0; i < nMoodData; i++)
            {
                logAi.LogMood(0, Random.Range(AppConfig.MinMoodValue, AppConfig.MaxMoodValue + 1));
            }
            yield return null;*/

            // Add scores for all play sessions
            Debug.Log("Start adding PS scores");
            yield return null;
            List<LogPlaySessionScoreParams> logPlaySessionScoreParamsList = new List<LogPlaySessionScoreParams>();
            IEnumerable<JourneyPosition> allJPs = AppManager.I.JourneyHelper.GetAllJourneyPositionsUpTo(targetPosition);
            foreach (var jp in allJPs)
            {
                if (jp.Stage <= targetPosition.Stage)
                {
                    int score = useBestScores ? AppConfig.MaxMiniGameScore : UnityEngine.Random.Range(AppConfig.MinMiniGameScore, AppConfig.MaxMiniGameScore);
                    logPlaySessionScoreParamsList.Add(new LogPlaySessionScoreParams(jp, score, 12f));
                    Debug.Log("Add play session score for " + jp.Id);
                }
            }
            logAi.LogPlaySessionScores(0, logPlaySessionScoreParamsList, true);

            // Add scores for all minigames
            Debug.Log("Start adding MiniGame scores");
            yield return null;
            List<LogMiniGameScoreParams> logMiniGameScoreParamses = new List<LogMiniGameScoreParams>();
            List<MiniGameInfo> allMiniGameInfo = AppManager.I.ScoreHelper.GetAllMiniGameInfo();
            for (int i = 0; i < allMiniGameInfo.Count; i++)
            {
                int score = useBestScores
                    ? AppConfig.MaxMiniGameScore
                    : UnityEngine.Random.Range(AppConfig.MinMiniGameScore, AppConfig.MaxMiniGameScore);
                logMiniGameScoreParamses.Add(new LogMiniGameScoreParams(JourneyPosition.InitialJourneyPosition,
                    allMiniGameInfo[i].data.Code, score, 12f));
                //Debug.Log("Add minigame score " + i);
            }
            logAi.LogMiniGameScores(0, logMiniGameScoreParamses, true);
            yield return null;

            // Add scores for some learning data (words/letters/phrases)
            /*var maxPlaySession = AppManager.I.Player.MaxJourneyPosition.ToString();
            var allWordInfo = AppManager.I.Teacher.ScoreHelper.GetAllWordInfo();
            for (int i = 0; i < allWordInfo.Count; i++)
            {
                if (Random.value < 0.3f)
                {
                    var resultsList = new List<Teacher.LogAI.LearnResultParameters>();
                    var newResult = new Teacher.LogAI.LearnResultParameters();
                    newResult.elementId = allWordInfo[i].data.Id;
                    newResult.table = DbTables.Words;
                    newResult.nCorrect = Random.Range(1,5);
                    newResult.nWrong = Random.Range(1, 5);
                    resultsList.Add(newResult);
                    logAi.LogLearn(fakeAppSession, maxPlaySession, MiniGameCode.Assessment_LetterForm, resultsList);
                }
            }
            var allLetterInfo = AppManager.I.Teacher.ScoreHelper.GetAllLetterInfo();
            for (int i = 0; i < allLetterInfo.Count; i++)
            {
                if (Random.value < 0.3f)
                {
                    var resultsList = new List<Teacher.LogAI.LearnResultParameters>();
                    var newResult = new Teacher.LogAI.LearnResultParameters();
                    newResult.elementId = allLetterInfo[i].data.Id;
                    newResult.table = DbTables.Letters;
                    newResult.nCorrect = Random.Range(1, 5);
                    newResult.nWrong = Random.Range(1, 5);
                    resultsList.Add(newResult);
                    logAi.LogLearn(fakeAppSession, maxPlaySession, MiniGameCode.Assessment_LetterForm, resultsList);
                }
            }*/
        }

        #endregion
    }
}
