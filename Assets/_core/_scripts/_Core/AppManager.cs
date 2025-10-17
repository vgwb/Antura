using Antura.Audio;
using Antura.Book;
using Antura.Core.Services;
using Antura.Database;
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
using UnityEngine.SceneManagement;

namespace Antura.Core
{
    /// <summary>
    /// Core of the application.
    /// Works as a general manager and entry point for all other systems and managers.
    /// </summary>
    [DefaultExecutionOrder(-900)]
    public class AppManager : SingletonMonoBehaviour<AppManager>
    {
        public static bool VERBOSE_INVERSION = false;

        public AppEditionConfig AppEdition => RootConfig.LoadedAppEdition;
        public ContentConfig ContentEdition => RootConfig.ContentEdition;

        public RootConfig RootConfig;
        public LanguageManager LanguageManager;

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
            base.Awake();
            if (I != this)
                return;

            Debug.Log("<color=#ff249c>>> WELCOME to LEARN WITH ANTURA - v" + AppEdition?.GetAppVersionString() + "</color>");

            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            InitScene();
        }

        void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
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

            Debug.Log("<color=#FF9900>##### AppManager Init</color>");

            if (DebugConfig.I.AddressablesBlockingLoad)
            {
                BlockingCoroutine(InitCO());
            }
            else
            {
                StartCoroutine(InitCO());
            }

        }

        // Init called on startup and every time a new scene is loaded
        void InitScene()
        {
            GlobalUI.Init();
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
            // Managers
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

            UIDirector.Init(); // Must be called after NavigationManager has been initialized

            // Debugger setup
            if (!DebugConfig.I.DebugLogEnabled)
            {
                Debug.LogWarning("LOGS ARE DISABLED - check the App Config");
            }
            Debug.unityLogger.logEnabled = DebugConfig.I.DebugLogEnabled;
            gameObject.AddComponent<Debugging.DebugManager>();

            // Update settings
            AppSettingsManager.UpdateAppVersion();

            Time.timeScale = 1;
            Loaded = true;
        }

        public IEnumerator ReloadEdition(bool skipLanguages = false)
        {
            if (!skipLanguages)
            {
                LanguageManager = new LanguageManager();
                yield return LanguageManager.LoadAllLanguageData();
                yield return LanguageManager.LoadEditionData();
            }
            DB = new DatabaseManager(ContentEdition);

            // We need to make sure we also set the player profile again, as it needs to re-load the Dynamic DB
            if (PlayerProfileManager != null && Player != null)
            {
                if (VERBOSE_INVERSION)
                    Debug.Log($"[Inversion] Reloading Player {Player.Uuid}");
                DB.LoadDatabaseForPlayer(Player.Uuid);
            }

            yield return LanguageManager.PreloadLocalizedDataCO();

            // TODO refactor: standardize initialisation of managers
            VocabularyHelper = new VocabularyHelper(DB);
            JourneyHelper = new JourneyHelper(DB);
            ScoreHelper = new ScoreHelper(DB);
            Teacher = new TeacherAI(DB, VocabularyHelper, ScoreHelper);
            LogManager = new LogManager();
            GameLauncher = new MiniGameLauncher(Teacher);

            if (PlayerProfileManager != null && Player != null)
            {
                Teacher.SetPlayerProfile(Player);
            }

        }

        public IEnumerator ResetLanguageSetup(LanguageCode langCode)
        {
            Debug.Log("ResetLanguageSetup " + langCode);
            AppSettingsManager.SetNativeLanguage(langCode);
            yield return LanguageManager.ReloadNativeLanguage();
        }

        #endregion
        
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            InitScene();
        }

        public void OnSceneChanged()
        {
            ModalWindowActivated = false;
        }

        public void QuitApplication()
        {
            GlobalUI.ShowPrompt(LocalizationDataId.UI_AreYouSure, () =>
            {
                Application.Quit();
            }, () => { });
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

#if UNITY_IOS
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
            if (tmpText != null && LanguageManager.I.GetHelper(LanguageUse.Learning).FixTMProDiacriticPositions(tmpText.textInfo))
            {
                tmpText.UpdateVertexData();
            }
        }
        #endregion

        #region Learning Method Shortcuts

        public static bool IsLearningMethod(LearnMethod learnMethodID)
        {
            return AppManager.I.ContentEdition.LearnMethod.Method == learnMethodID;
        }

        #endregion
    }
}
