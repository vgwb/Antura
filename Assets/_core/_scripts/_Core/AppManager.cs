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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public AppEditionConfig AppEdition => RootConfig.LoadedAppEdition;
        public ContentEditionConfig ContentEdition => RootConfig.ContentEdition;

        public RootConfig RootConfig;
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
        public AssetManager AssetManager;

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

        public bool IsAppSuspended { get; private set; }
        public bool ModalWindowActivated = false;


        #region Initialisation

        /// <summary>
        /// Prevent multiple setups.
        /// Set to true after first setup.
        /// </summary>
        private bool alreadySetup;

        /// <summary>
        /// Set to true only after the application has finished loading everything
        /// </summary>
        public bool Loaded;

        protected override void Awake()
        {
            GetComponent<AppBootstrap>().InitManagers();

            base.Awake();
            DontDestroyOnLoad(this);

            GlobalUI.Init();
        }

        /// <summary>
        /// first Init, from Awake()
        /// </summary>
        protected override void Init()
        {
            if (alreadySetup)
            {
                return;
            }
            alreadySetup = true;

            if (DebugConfig.I.AddressablesBlockingLoad)
            {
                BlockingCoroutine(InitCO());
            }
            else
            {
                StartCoroutine(InitCO());
            }

        }

        private void BlockingCoroutine(IEnumerator c)
        {
            Stack<IEnumerator> stack = new Stack<IEnumerator>();
            stack.Push(c);
            while (stack.Count > 0)
            {
                var peek = stack.Peek();
                bool result = peek.MoveNext();
                if (result)
                {
                    if (peek.Current != null)
                    {
                        stack.Push(peek.Current as IEnumerator);
                    }
                }
                else
                {
                    stack.Pop();
                }
            }
        }

        private IEnumerator InitCO()
        {
            AppSettingsManager = new AppSettingsManager();
            AssetManager = new AssetManager();

            yield return ReloadEdition();

            FirstContactManager = new FirstContactManager();
            Services = new ServicesManager(gameObject);

            if (AppEdition.EnableNotifications)
            {
                Services.Notifications.Init();
            }

            // MonoBehaviors
            NavigationManager = gameObject.AddComponent<NavigationManager>();
            NavigationManager.Init();
            gameObject.AddComponent<KeeperManager>();
            gameObject.AddComponent<BookManager>();

            RewardSystemManager = new RewardSystemManager();
            RewardSystemManager.Init();

            PlayerProfileManager = new PlayerProfileManager();
            bool hasUpgraded = PlayerProfileManager.LoadPlayerSettings();
            if (hasUpgraded)
            {
                yield return ReloadEdition();
            }

            //            Debug.Log("AppManager Init(): UIDirector.Init()");
            UIDirector.Init(); // Must be called after NavigationManager has been initialized

            // Debugger setup
            if (!DebugConfig.I.DebugLogEnabled)
            {
                Debug.LogWarning("LOGS ARE DISABLED - check the App Config");
            }
            Debug.unityLogger.logEnabled = DebugConfig.I.DebugLogEnabled;
            gameObject.AddComponent<Debugging.DebugManager>();

            // Debug.Log("AppManager Init(): UpdateAppVersion");
            // Update settings
            AppSettingsManager.UpdateAppVersion();

            Time.timeScale = 1;
            Loaded = true;
        }

        public IEnumerator ReloadEdition()
        {
            LanguageSwitcher = new LanguageSwitcher();
            yield return LanguageSwitcher.LoadAllLanguageData();
            yield return LanguageSwitcher.LoadEditionData();
            DB = new DatabaseManager(ContentEdition);
            yield return LanguageSwitcher.PreloadLocalizedDataCO();

            // TODO refactor: standardize initialisation of managers
            VocabularyHelper = new VocabularyHelper(DB);
            JourneyHelper = new JourneyHelper(DB);
            ScoreHelper = new ScoreHelper(DB);
            Teacher = new TeacherAI(DB, VocabularyHelper, ScoreHelper);
            LogManager = new LogManager();
            GameLauncher = new MiniGameLauncher(Teacher);
        }

        public IEnumerator ResetLanguageSetup(LanguageCode langCode)
        {
            AppSettingsManager.SetNativeLanguage(langCode);
            yield return LanguageSwitcher.ReloadNativeLanguage();
        }

        #endregion

        void Update()
        {
            // Exit with Android back button
            //if (Input.GetKeyDown(KeyCode.Escape)) {
            //    if (Application.platform == RuntimePlatform.Android && !AppSettings.KioskMode) {
            //        QuitApplication();
            //    }
            //}

            if (Input.GetKeyDown(KeyCode.T))
            {
                Services.Analytics.TestEvent();
            }
        }

        public void OnSceneChanged()
        {
            ModalWindowActivated = false;
        }

        public void QuitApplication()
        {
            GlobalUI.ShowPrompt(LocalizationDataId.UI_AreYouSure, () =>
            {
                Debug.Log("Application Quit");
                Application.Quit();
            }, () =>
            {
            });
        }

        public void StartNewPlaySession()
        {
            LogManager.I.InitNewSession();
            LogManager.I.LogInfo(InfoEvent.AppPlay, JsonUtility.ToJson(new DeviceInfo()));
            if (AppManager.I.AppEdition.EnableNotifications)
            {
                Services.Notifications.DeleteAllLocalNotifications();
            }
            Services.Analytics.TrackPlayerSession(Player.Age, Player.Gender);
        }

        #region App Pause Suspend/Resume
#if UNITY_ANDROID
        void OnApplicationFocus(bool focus)
        {
            SuspendApplication(!focus);
        }
#endif

#if UNITY_EDITOR || UNITY_IOS
        void OnApplicationPause(bool pause)
        {
            SuspendApplication(pause);
        }
#endif

        private void SuspendApplication(bool pause)
        {
            if (pause == IsAppSuspended)
                return; // Ignore if paused already
            IsAppSuspended = pause;
            if (IsAppSuspended)
            {
                // app is pausing
                if (LogManager.I != null)
                    LogManager.I.LogInfo(InfoEvent.AppSuspend);

                if (Services != null)
                    Services.Notifications.AppSuspended();
            }
            else
            {
                // app is resuming
                if (LogManager.I != null)
                {
                    LogManager.I.LogInfo(InfoEvent.AppResume);
                    LogManager.I.InitNewSession();
                }

                if (Services != null)
                    Services.Notifications.AppResumed();
            }
            AudioManager.I.OnAppPause(IsAppSuspended);
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
            if (tmpText != null && LanguageSwitcher.I.GetHelper(LanguageUse.Learning).FixTMProDiacriticPositions(tmpText.textInfo))
            {
                tmpText.UpdateVertexData();
            }
        }
        #endregion
    }
}
