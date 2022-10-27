using System;
using System.Collections;
using Antura.Core;
using Antura.Minigames;
using Antura.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Antura.Debugging
{
    public class DebugPanel : MonoBehaviour
    {
        public static DebugPanel I;

        [Header("References")]
        public GameObject Panel;

        public GameObject Container;
        public GameObject PrefabRow;
        public GameObject PrefabMiniGameButton;

        public TextMeshProUGUI InfoText;

        public InputField InputStage;
        public InputField InputLearningBlock;
        public InputField InputPlaySession;

        public Toggle TutorialEnabledToggle;
        public Toggle VerboseTeacherToggle;
        public Toggle SafeLaunchToggle;
        public Toggle AutoCorrectJourneyPosToggle;

        public Toggle BotEnabledToggle;
        public Toggle BotStopAtJPToggle;
        public Slider BotDelay;
        public Slider BotGameSpeed;

        public bool TutorialEnabled
        {
            get { return AppConfig.MinigameTutorialsEnabled; }
            set { AppConfig.MinigameTutorialsEnabled = value; }
        }

        public bool VerboseTeacher
        {
            get { return DebugManager.I.VerboseTeacher; }
            set { DebugManager.I.VerboseTeacher = value; }
        }

        public bool SafeLaunch
        {
            get { return DebugManager.I.SafeLaunch; }
            set { DebugManager.I.SafeLaunch = value; }
        }

        public bool AutoCorrectJourneyPos
        {
            get { return DebugManager.I.AutoCorrectJourneyPos; }
            set { DebugManager.I.AutoCorrectJourneyPos = value; }
        }

        private bool advancedSettingsEnabled;

        private int clickCounter;
        private Dictionary<string, bool> playedMinigames = new Dictionary<string, bool>();

        public GameObject ProfilePanel;
        public GameObject NavigationPanel;

        void Awake()
        {
            if (I != null)
            {
                Destroy(gameObject);
            }
            else
            {
                I = this;
                DontDestroyOnLoad(gameObject);
            }

            if (Panel.activeSelf)
            {
                Panel.SetActive(false);
            }
        }

        #region Buttons

        #region Open / Close

        public void OnClickOpen()
        {
            clickCounter++;
            if (clickCounter >= 3)
            {
                if (AppManager.I.RootConfig.LoadedAppEdition.OpenBugReportOnHiddenButton)
                {
                    FindObjectOfType<UserReportingScript>().CreateUserReport();
                    clickCounter = 0;
                }
                else
                {
                    Open();
                }
            }
        }

        public void OnClickClose()
        {
            Close();
        }

        #endregion

        #region Launching

        public void ResetPlayTest()
        {
            playedMinigames.Clear();
            BuildUI();
        }

        #endregion

        #region Navigation

        public void GoToHome()
        {
            DebugManager.I.GoToHome();
            Close();
        }

        public void GoToMap()
        {
            DebugManager.I.GoToMap();
            Close();
        }

        public void GoToNext()
        {
            DebugManager.I.GoToNext();
            Close();
        }

        public void GoToEnd()
        {
            DebugManager.I.GoToEnd();
            Close();
        }

        public void GoToReservedArea()
        {
            //WidgetPopupWindow.I.Close();
            DebugManager.I.GoToReservedArea();
            Close();
        }

        public void GoToKiosk()
        {
            DebugManager.I.GoToKiosk();
            Close();
        }

        #endregion

        #region Journey

        public void ForwardMaxJourneyPos()
        {
            DebugManager.I.ForwardMaxJourneyPos();
            Close();
        }

        public void SecondToLastJourneyPos()
        {
            DebugManager.I.SecondToLastJourneyPos();
            Close();
        }

        public void ResetMaxJourneyPos()
        {
            DebugManager.I.ResetMaxJourneyPos();
            Close();
        }

        public void ForceMaxJourneyPos()
        {
            DebugManager.I.SetDebugJourneyPos(GetCurrentJourneyPositionInUI());
            DebugManager.I.ForceMaxJourneyPos();
            Close();
        }

        #endregion

        #region test
        public void TestLocalNotification()
        {
            AppManager.I.Services.Notifications.TestLocalNotification();
            Close();
        }
        #endregion

        #region Profiles

        public void ResetAll()
        {
            Close();
            DebugManager.I.ResetAll(clearOnly:false);
        }

        public void OnCreateTestProfile()
        {
            DebugManager.I.CreateTestProfile();
            Close();
        }

        public void OnCreateOldProfile()
        {
            DebugManager.I.CreateOldProfile();
            Close();
        }

        public bool FirstContactCompleted
        {
            get { return DebugManager.I.FirstContactCompleted; }
            set { DebugManager.I.FirstContactCompleted = value; }
        }

        #endregion

        public void OnReportBug()
        {
            AppManager.I.OpenSupportForm();
        }

        #endregion

        #region Internal

        private void Open()
        {
            BuildUI();
            Panel.SetActive(true);
            DebugManager.I.DebugPanelOpened = true;
        }

        private void Close()
        {
            clickCounter = 0;
            Panel.SetActive(false);
            DebugManager.I.DebugPanelOpened = false;
        }

        #endregion

        #region UI

        #if !UNITY_EDITOR
        private void Update()
        {
            BotTester.I.Config.StopBeforeJP = GetCurrentJourneyPositionInUI();
        }
        #endif

        private void BuildUI()
        {
            if (AppManager.I.Player != null)
            {
                InputStage.text = AppManager.I.Player.CurrentJourneyPosition.Stage.ToString();
                InputLearningBlock.text = AppManager.I.Player.CurrentJourneyPosition.LearningBlock.ToString();
                InputPlaySession.text = AppManager.I.Player.CurrentJourneyPosition.PlaySession.ToString();
            }

            DisplayInfoText();

            TutorialEnabledToggle.isOn = TutorialEnabled;
            AutoCorrectJourneyPosToggle.isOn = AutoCorrectJourneyPos;
            VerboseTeacherToggle.isOn = VerboseTeacher;
            SafeLaunchToggle.isOn = SafeLaunch;

            BotEnabledToggle.isOn = BotTester.I.Config.BotEnabled;
            BotEnabledToggle.onValueChanged.RemoveAllListeners();
            BotEnabledToggle.onValueChanged.AddListener(v => BotTester.I.Config.BotEnabled = v);

            #if UNITY_EDITOR
            BotStopAtJPToggle.gameObject.SetActive(false);
            #else
            BotStopAtJPToggle.gameObject.SetActive(true);
            #endif

            BotStopAtJPToggle.isOn = BotTester.I.Config.EnableStopBeforeJP;
            BotStopAtJPToggle.onValueChanged.RemoveAllListeners();
            BotStopAtJPToggle.onValueChanged.AddListener(v => BotTester.I.Config.EnableStopBeforeJP = v);

            BotDelay.value = BotTester.I.Config.Delay;
            BotDelay.onValueChanged.RemoveAllListeners();
            BotDelay.onValueChanged.AddListener(v => BotTester.I.Config.Delay = v);

            BotGameSpeed.value = BotTester.I.Config.GameSpeed;
            BotGameSpeed.onValueChanged.RemoveAllListeners();
            BotGameSpeed.onValueChanged.AddListener(v => BotTester.I.Config.GameSpeed = v);

            try
            {
                var mainMiniGamesList = MiniGamesUtilities.GetMainMiniGameList(false, MiniGamesUtilities.MiniGameSortLogic.Alphanumeric);
                var difficultiesForTesting = MiniGamesUtilities.GetMiniGameDifficultiesForTesting();

                EmptyContainer(Container);

                foreach (var mainMiniGame in mainMiniGamesList)
                {
                    var newRow = Instantiate(PrefabRow);
                    newRow.transform.SetParent(Container.transform, false);

                    newRow.GetComponent<DebugMiniGameRow>().Title.text = mainMiniGame.MainId;

                    foreach (var gameVariation in mainMiniGame.variations)
                    {
                        Debug.Assert(difficultiesForTesting.ContainsKey(gameVariation.data.Code),
                            "No difficulty for testing setup for game variation " + gameVariation.data.Code);
                        var difficulties = difficultiesForTesting[gameVariation.data.Code];

                        foreach (var difficulty in difficulties)
                        {
                            var btnGO = Instantiate(PrefabMiniGameButton);
                            btnGO.transform.SetParent(newRow.transform, false);
                            bool gamePlayed;
                            playedMinigames.TryGetValue(GetDictKey(gameVariation.data.Code, difficulty), out gamePlayed);
                            btnGO.GetComponent<DebugMiniGameButton>().Init(this, gameVariation, gamePlayed, difficulty);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            // Advanced settings
            SafeLaunchToggle.gameObject.SetActive(advancedSettingsEnabled);
            AutoCorrectJourneyPosToggle.gameObject.SetActive(advancedSettingsEnabled);
            VerboseTeacherToggle.gameObject.SetActive(advancedSettingsEnabled);

            ProfilePanel.SetActive(advancedSettingsEnabled);
            NavigationPanel.SetActive(advancedSettingsEnabled);
        }

        private void DisplayInfoText()
        {
            InfoText.text = "";

            if (AppManager.I.Player != null)
            {
                InfoText.text += "Current JP: " + AppManager.I.Player.CurrentJourneyPosition + "\n";
            }
            if (AppManager.I.NavigationManager.CurrentMiniGameData != null)
            {
                InfoText.text += "Current MiniGame: " + AppManager.I.NavigationManager.CurrentMiniGameData.Code + "\n";
                InfoText.text += AppManager.I.GameLauncher.GetCurrentMiniGameConfigSummary();
            }

        }

        #endregion

        #region Actions

        private string GetDictKey(MiniGameCode minigameCode, float difficulty)
        {
            return minigameCode.ToString() + difficulty.ToString("F1");
        }

        private JourneyPosition GetCurrentJourneyPositionInUI()
        {
            return new JourneyPosition(int.Parse(InputStage.text), int.Parse(InputLearningBlock.text),
               int.Parse(InputPlaySession.text));
        }

        public void LaunchMiniGame(MiniGameCode minigameCode, float difficulty)
        {
            playedMinigames[GetDictKey(minigameCode, difficulty)] = true;

            var debugJP = GetCurrentJourneyPositionInUI();

            if (!DebugManager.I.SafeLaunch || AppManager.I.Teacher.CanMiniGameBePlayedAfterMinPlaySession(debugJP, minigameCode))
            {
                LaunchMiniGameAtJourneyPosition(minigameCode, difficulty, debugJP);
            }
            else
            {
                if (DebugManager.I.SafeLaunch)
                {
                    JourneyPosition minJP = AppManager.I.JourneyHelper.GetMinimumJourneyPositionForMiniGame(minigameCode);
                    if (minJP == null)
                    {
                        Debug.LogWarningFormat(
                            "Minigame {0} could not be selected for any PlaySession. Please check the PlaySession data table.",
                            minigameCode);
                    }
                    else
                    {
                        Debug.LogErrorFormat("Minigame {0} cannot be selected PS {1}. Minimum PS is: {2}", minigameCode, debugJP, minJP);

                        if (AutoCorrectJourneyPos)
                        {
                            LaunchMiniGameAtJourneyPosition(minigameCode, difficulty, minJP);
                        }
                    }
                }
            }
        }

        private void LaunchMiniGameAtJourneyPosition(MiniGameCode minigameCode, float difficulty, JourneyPosition journeyPosition)
        {
            WidgetPopupWindow.I.Close();
            DebugManager.I.SetDebugJourneyPos(journeyPosition);
            DebugManager.I.LaunchMiniGame(minigameCode, difficulty);
            Close();
        }

        public void OnEndMinigame()
        {
            DebugManager.I.ForceCurrentMinigameEnd(2);
            Close();
        }

        public void OnAddBones()
        {
            DebugManager.I.AddBones();
        }

        public void ToggleAdvancedSettings(bool choice)
        {
            advancedSettingsEnabled = choice;
            BuildUI();
        }

        #endregion

        #region Utilities

        private void EmptyContainer(GameObject container)
        {
            foreach (Transform t in container.transform)
            {
                Destroy(t.gameObject);
            }
        }

        #endregion

        public void OpenReportPanel()
        {
            StartCoroutine(DoCreateUserReport());
        }

        private IEnumerator DoCreateUserReport()
        {
            Close();
            yield return new WaitForEndOfFrame();
            FindObjectOfType<UserReportingScript>().CreateUserReport();
        }
    }
}
