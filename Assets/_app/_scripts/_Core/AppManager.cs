using Antura.Audio;
using Antura.Book;
using Antura.Core.Services;
using Antura.Core.Services.OnlineAnalytics;
using Antura.Database;
using Antura.Helpers;
using Antura.Keeper;
using Antura.Language;
using Antura.Minigames;
using Antura.Profile;
using Antura.Rewards;
using Antura.Teacher;
using Antura.UI;
using Antura.Utilities;
using UnityEngine;
using UnityEngine.Networking;
namespace Antura.Core
{

    /// <summary>
    /// Core of the application.
    /// Works as a general manager and entry point for all other systems and managers.
    /// </summary>
    public class AppManager : SingletonMonoBehaviour<AppManager>
    {
        public SAppConfig AppConfig;
        public LanguageSwitcher LanguageSwitcher;

        public AppSettingsManager AppSettingsManager;
        public TeacherAI Teacher;
        public VocabularyHelper VocabularyHelper;
        public ScoreHelper ScoreHelper;
        public JourneyHelper JourneyHelper;
        public DatabaseManager DB;
        public MiniGameLauncher GameLauncher;
        public LogManager LogManager;
        public ServicesManager Services;
        public FirstContactManager FirstContactManager;
        public PlayerProfileManager PlayerProfileManager;
        public RewardSystemManager RewardSystemManager;
        public FacebookManager FacebookManager;

        [HideInInspector]
        public NavigationManager NavigationManager;

        public AppSettings AppSettings
        {
            get { return AppSettingsManager.Settings; }
        }

        public PlayerProfile Player
        {
            get { return PlayerProfileManager.CurrentPlayer; }
            set { PlayerProfileManager.CurrentPlayer = value; }
        }

        public bool IsPaused { get; private set; }
        public bool ModalWindowActivated = false;


        #region Initialisation

        /// <summary>
        /// Prevent multiple setups.
        /// Set to true after first setup.
        /// </summary>
        private bool alreadySetup;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
        }

        /// <summary>
        /// first Init, from Awake()
        /// </summary>
        protected override void Init()
        {
            if (alreadySetup) {
                return;
            }
            alreadySetup = true;

            AppSettingsManager = new AppSettingsManager();

            LanguageSwitcher = new LanguageSwitcher();
            var learningDB = LanguageSwitcher.GetManager(LanguageUse.Learning);
            DB = learningDB;

            // TODO refactor: standardize initialisation of managers
            LogManager = new LogManager();
            VocabularyHelper = new VocabularyHelper(learningDB);
            JourneyHelper = new JourneyHelper(learningDB);
            ScoreHelper = new ScoreHelper(learningDB);
            Teacher = new TeacherAI(learningDB, VocabularyHelper, ScoreHelper);
            GameLauncher = new MiniGameLauncher(Teacher);
            FirstContactManager = new FirstContactManager();
            Services = new ServicesManager();
            FacebookManager = gameObject.AddComponent<FacebookManager>();
            FacebookManager.verbose = true;

            // MonoBehaviors
            NavigationManager = gameObject.AddComponent<NavigationManager>();
            NavigationManager.Init();
            gameObject.AddComponent<KeeperManager>();
            gameObject.AddComponent<BookManager>();

            RewardSystemManager = new RewardSystemManager();
            RewardSystemManager.Init();

            PlayerProfileManager = new PlayerProfileManager();
            PlayerProfileManager.LoadPlayerSettings();

            Services = new ServicesManager();

            Debug.Log("AppManager Init(): UIDirector.Init()");
            UIDirector.Init(); // Must be called after NavigationManager has been initialized

            // Debugger setup
            Debug.unityLogger.logEnabled = Core.AppConfig.DebugLogEnabled;
            gameObject.AddComponent<Debugging.DebugManager>();

            Debug.Log("AppManager Init(): UpdateAppVersion");
            // Update settings
            AppSettingsManager.UpdateAppVersion();
        }
        #endregion

        void Update()
        {
            // Exit with Android back button
            if (Input.GetKeyDown(KeyCode.Escape)) {
                if (Application.platform == RuntimePlatform.Android && !AppSettings.KioskMode) {
                    QuitApplication();
                }
            }
        }

        public void OnSceneChanged()
        {
            ModalWindowActivated = false;
        }

        public void QuitApplication()
        {
            GlobalUI.ShowPrompt(LocalizationDataId.UI_AreYouSure, () => {
                Debug.Log("Application Quit");
                Application.Quit();
            }, () => {
            });
        }

        public void StartNewPlaySession()
        {
            LogManager.I.InitNewSession();
            LogManager.I.LogInfo(InfoEvent.AppPlay, JsonUtility.ToJson(new DeviceInfo()));
            Services.Notifications.DeleteAllLocalNotifications();
            Services.Analytics.TrackPlayerSession(Player.Age, Player.Gender);
        }

        #region Main App Suspend method
        void OnApplicationPause(bool pauseStatus)
        {
            IsPaused = pauseStatus;

            // app is pausing
            if (IsPaused) {
                LogManager.I.LogInfo(InfoEvent.AppSuspend);
                Services.Notifications.AppSuspended();
            }

            //app is resuming
            if (!IsPaused) {
                LogManager.I.LogInfo(InfoEvent.AppResume);
                Services.Notifications.AppResumed();
                LogManager.I.InitNewSession();
            }
            AudioManager.I.OnAppPause(IsPaused);
        }
        #endregion

        public void OpenSupportForm()
        {
            var parameters = "";
            parameters += "?entry.346861357=" + UnityWebRequest.EscapeURL(new DeviceInfo().ToJsonData());
            parameters += "&entry.1999287882=" + UnityWebRequest.EscapeURL(Player.ToJsonData());

            Application.OpenURL(Core.AppConfig.UrlSupportForm + parameters);
        }

        #region TMPro hack
        /// <summary>
        /// TextMesh Pro hack to manage Diacritic Symbols correct positioning
        /// </summary>
        void OnEnable()
        {
            // Subscribe to event fired when text object has been regenerated.
            TMPro.TMPro_EventManager.TEXT_CHANGED_EVENT.Add(On_TMPro_Text_Changed);
        }

        void OnDisable()
        {
            TMPro.TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(On_TMPro_Text_Changed);
        }

        void On_TMPro_Text_Changed(Object obj)
        {
            var tmpText = obj as TMPro.TMP_Text;
            if (tmpText != null && ArabicAlphabetHelper.FixTMProDiacriticPositions(tmpText.textInfo)) {
                tmpText.UpdateVertexData();
            }
        }
        #endregion
    }
}