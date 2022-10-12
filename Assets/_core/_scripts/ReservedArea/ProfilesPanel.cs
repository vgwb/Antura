using Antura.Book;
using Antura.Core;
using Antura.Profile;
using Antura.Teacher;
using Antura.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.ReservedArea
{
    public class ProfilesPanel : MonoBehaviour
    {
        [Header("References")]
        public TextRender PlayerInfoText;

        public GameObject PlayerIconContainer;
        public GameObject PlayerIconPrefab;
        public GameObject ProfileCommandsContainer;
        public GameObject pleaseWaitPanel;

        private string SelectedPlayerId;

        void Start()
        {
            ResetAll();
        }

        void ResetAll()
        {
            SelectedPlayerId = "";
            RefreshPlayerIcons();
            RefreshUI();
        }

        void RefreshPlayerIcons()
        {
            foreach (Transform t in PlayerIconContainer.transform)
            {
                Destroy(t.gameObject);
            }

            List<PlayerIconData> players = AppManager.I.PlayerProfileManager.GetPlayersIconData();

            foreach (var player in players)
            {
                var newIcon = Instantiate(PlayerIconPrefab);
                newIcon.transform.SetParent(PlayerIconContainer.transform, false);
                newIcon.GetComponent<PlayerIcon>().Init(player);
                newIcon.GetComponent<UIButton>().Bt.onClick.AddListener(() => OnSelectPlayerProfile(player.Uuid));
            }
        }

        void RefreshUI()
        {
            // highlight selected profile
            ProfileCommandsContainer.SetActive(SelectedPlayerId != "");
            SetPlayerInfoText();
            foreach (Transform t in PlayerIconContainer.transform)
            {
                t.GetComponent<PlayerIcon>().Select(SelectedPlayerId);
            }
        }

        public void OnSelectPlayerProfile(string uuid)
        {
            //Debug.Log("OnSelectPlayerProfile " + uuid);
            SelectedPlayerId = SelectedPlayerId != uuid ? uuid : "";
            RefreshUI();
        }

        void SetPlayerInfoText()
        {
            if (SelectedPlayerId != "")
            {
                PlayerInfoText.text = "player id: " + SelectedPlayerId;
            }
            else
            {
                PlayerInfoText.text = "";
            }
        }

        public void OnOpenSelectedPlayerProfile()
        {
            //Debug.Log("OPEN " + SelectedPlayerId);
            AppManager.I.PlayerProfileManager.SetPlayerAsCurrentByUUID(SelectedPlayerId);
            BookManager.I.OpenBook(BookArea.Player);
        }

        public void OnDeleteSelectPlayerProfile()
        {
            GlobalUI.ShowPrompt(id: Database.LocalizationDataId.UI_AreYouSure, _onYesCallback: DoDeleteSelectPlayerProfile, _onNoCallback: DoNothing);
        }

        void DoNothing()
        {
        }

        void DoDeleteSelectPlayerProfile()
        {
            //Debug.Log("DELETE " + SelectedPlayerId);
            AppManager.I.PlayerProfileManager.DeletePlayerProfile(SelectedPlayerId);
            ResetAll();
        }

        public void OnExportSelectPlayerProfile()
        {
            if (AppManager.I.DB.ExportPlayerDb(SelectedPlayerId))
            {
                string dbPath;
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    dbPath = string.Format(@"{0}/{1}", AppConfig.DbExportFolder,
                        AppConfig.GetPlayerDatabaseFilename(SelectedPlayerId));
                    GlobalUI.ShowPrompt("Get the DB from iTunes app:\n" + dbPath);
                }
                else
                {
                    // Android or Desktop
                    dbPath = string.Format(@"{0}/{1}/{2}", Application.persistentDataPath, AppConfig.DbExportFolder,
                        AppConfig.GetPlayerDatabaseFilename(SelectedPlayerId));
                    GlobalUI.ShowPrompt("The DB is here:\n" + dbPath);
                }
            }
            else
            {
                GlobalUI.ShowPrompt("Could not export the database.");
            }
        }

        public void OnCreateDemoPlayer()
        {
            if (AppManager.I.PlayerProfileManager.IsDemoUserExisting())
            {
                GlobalUI.ShowPrompt(id: Database.LocalizationDataId.ReservedArea_DemoUserAlreadyExists);
            }
            else
            {
                //GlobalUI.ShowPrompt(Database.LocalizationDataId.UI_AreYouSure, DoCreateDemoPlayer, DoNothing, Keeper.KeeperMode.NativeNoSubtitles);
                DoCreateDemoPlayer();
            }
        }

        void DoCreateDemoPlayer()
        {
            StartCoroutine(CreateDemoPlayer());
        }

        #region Demo User Helpers

        private static bool TEST_ALMOST_AT_END = false;

        public IEnumerator CreateDemoPlayer()
        {
            //Debug.Log("creating DEMO USER ");
            yield return null;
            activateWaitingScreen(true);
            yield return null;
            var demoUserUiid = AppManager.I.PlayerProfileManager.CreatePlayerProfile(true, 1, PlayerGender.M, PlayerTint.Purple, Color.yellow, Color.red, Color.magenta, 4,
                        AppManager.I.AppEdition.editionID,
                        AppManager.I.ContentEdition.ContentID,
                        AppManager.I.AppEdition.AppVersion,
                        true);
            SelectedPlayerId = demoUserUiid;

            // Populate with complete data
            var maxJourneyPos = AppManager.I.JourneyHelper.GetFinalJourneyPosition(considerEndSceneToo: true);
            if (TEST_ALMOST_AT_END) maxJourneyPos = new JourneyPosition(6, 13, 100);
            yield return StartCoroutine(PopulateDatabaseWithUsefulDataCO(maxJourneyPos));

            AppManager.I.Player.SetMaxJourneyPosition(maxJourneyPos, true, true);
            AppManager.I.Player.AddBones(500);
            AppManager.I.Player.ForcePreviousJourneyPosition(maxJourneyPos);

            if (!TEST_ALMOST_AT_END)
            {
                AppManager.I.Player.SetFinalShown(isInitialising: true);
                AppManager.I.Player.HasFinishedTheGame = true;
                AppManager.I.Player.HasFinishedTheGameWithAllStars = true;
                AppManager.I.Player.HasMaxStarsInCurrentPlaySessions = true;
            }
            AppManager.I.FirstContactManager.ForceToFinishedSequence();
            AppManager.I.FirstContactManager.ForceAllCompleted();
            AppManager.I.RewardSystemManager.UnlockAllPacks();

            ResetAll();
            activateWaitingScreen(false);
        }

        void activateWaitingScreen(bool status)
        {
            pleaseWaitPanel.gameObject.SetActive(status);
            GlobalUI.I.BackButton.gameObject.SetActive(!status);
        }

        IEnumerator PopulateDatabaseWithUsefulDataCO(JourneyPosition targetPosition)
        {
            bool useBestScores = true;

            var logAi = AppManager.I.Teacher.logAI;

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
            var logPlaySessionScoreParamsList = new List<LogPlaySessionScoreParams>();
            var allJPs = AppManager.I.JourneyHelper.GetAllJourneyPositionsUpTo(targetPosition);
            foreach (var jp in allJPs)
            {
                if (jp.Stage <= targetPosition.Stage)
                {
                    int score = useBestScores ? AppConfig.MaxMiniGameScore : Random.Range(AppConfig.MinMiniGameScore, AppConfig.MaxMiniGameScore);
                    logPlaySessionScoreParamsList.Add(new LogPlaySessionScoreParams(jp, score, 12f));
                    Debug.Log("Add play session score for " + jp.Id);
                }
            }
            logAi.LogPlaySessionScores(0, logPlaySessionScoreParamsList, true);

            // Add scores for all minigames
            Debug.Log("Start adding MiniGame scores");
            yield return null;
            var logMiniGameScoreParamses = new List<LogMiniGameScoreParams>();
            var allMiniGameInfo = AppManager.I.ScoreHelper.GetAllMiniGameInfo();
            for (int i = 0; i < allMiniGameInfo.Count; i++)
            {
                int score = useBestScores
                    ? AppConfig.MaxMiniGameScore
                    : Random.Range(AppConfig.MinMiniGameScore, AppConfig.MaxMiniGameScore);
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
