using Antura.Audio;
using System;
using System.Linq;
using UnityEngine;

namespace Antura.Core
{
    public class AppSettingsManager
    {
        private const string SETTINGS_PREFS_KEY = "OPTIONS";
        private AppSettings _settings = new AppSettings();

        public AppSettings Settings
        {
            get { return _settings; }
            set
            {
                if (value != _settings)
                {
                    _settings = value;
                    // Auto save at any change
                    SaveSettings();
                }
                else
                {
                    _settings = value;
                }
            }
        }

        private bool isFirstIstall;
        public Version AppVersionPrevious;

        public AppSettingsManager()
        {
            LoadSettings();
        }

        /// <summary>
        /// Loads the settings. Creates new settings if none are found.
        /// </summary>
        public AppSettings LoadSettings()
        {
            if (PlayerPrefs.HasKey(SETTINGS_PREFS_KEY))
            {
                var serializedObjs = PlayerPrefs.GetString(SETTINGS_PREFS_KEY);
                Settings = JsonUtility.FromJson<AppSettings>(serializedObjs);
                //Debug.Log("LoadSettings() " + serializedObjs);
            }
            else
            {
                // FIRST INSTALLATION
                isFirstIstall = true;
                Debug.Log("LoadSettings() FIRST INSTALLATION");
                Settings = new AppSettings();
                Settings.SetAppVersion(AppManager.I.AppEdition.AppVersion);
                Settings.ContentID = AppManager.I.ContentEdition.ContentID;

                // set native Language
                // first set the default / fallback language
                Settings.NativeLanguage = AppManager.I.ContentEdition.NativeLanguage;
                if (AppManager.I.AppEdition.DetectSystemLanguage)
                {
                    foreach (var lang in AppManager.I.ContentEdition.SupportedNativeLanguages)
                    {
                        if (lang == Language.LanguageCode.italian && Application.systemLanguage == SystemLanguage.Italian)
                        {
                            Settings.NativeLanguage = lang;
                        }
                        if (lang == Language.LanguageCode.spanish && Application.systemLanguage == SystemLanguage.Spanish)
                        {
                            Settings.NativeLanguage = lang;
                        }
                        if (lang == Language.LanguageCode.arabic && Application.systemLanguage == SystemLanguage.Arabic)
                        {
                            Settings.NativeLanguage = lang;
                        }
                        if (lang == Language.LanguageCode.english && Application.systemLanguage == SystemLanguage.English)
                        {
                            Settings.NativeLanguage = lang;
                        }
                    }
                }
            }

            AudioManager.I.MusicEnabled = Settings.MusicEnabled;
            // force Subtitles ON
            Settings.KeeperSubtitlesEnabled = false;

            // TODO: redo this without affecting SAppConfig.I
            //SAppConfig.I.NativeLanguage = SAppConfig.I.SubtitlesLanguage = Settings.NativeLanguage;

            // Debug.Log("Setting music to " + Settings.MusicOn);
            return _settings;
        }

        /// <summary>
        /// Save all settings. This also saves player profiles.
        /// </summary>
        public void SaveSettings()
        {
            var serializedObjs = JsonUtility.ToJson(Settings);
            PlayerPrefs.SetString(SETTINGS_PREFS_KEY, serializedObjs);
            PlayerPrefs.Save();
            // Debug.Log("AppSettingsManager SaveSettings() " + serializedObjs);
        }

        /// <summary>
        /// Delete all settings. This also deletes all player profiles.
        /// </summary>
        public void DeleteAllSettings()
        {
            PlayerPrefs.DeleteAll();
        }

        #region external API to save single settings
        public void SaveMusicSetting(bool musicOn)
        {
            Settings.MusicEnabled = musicOn;
            SaveSettings();
        }
        #endregion

        public bool IsAppJustUpdatedFromOldVersion()
        {
            if (!isFirstIstall && AppVersionPrevious != null && AppVersionPrevious <= new Version(1, 0, 0, 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateAppVersion()
        {
            if (Settings.AppVersion != null && Settings.AppVersion == "")
            {
                AppVersionPrevious = new Version(0, 0, 0, 0);
            }
            else
            {
                AppVersionPrevious = new Version(Settings.AppVersion);
            }
            Debug.Log("AppVersion is: " + AppManager.I.AppEdition.AppVersion + " (previous:" + AppVersionPrevious + ")");
            Settings.SetAppVersion(AppManager.I.AppEdition.AppVersion);
            SaveSettings();
        }

        public Action<bool> onEnableShareAnalytics;

        public void EnableShareAnalytics(bool status)
        {
            Settings.ShareAnalyticsEnabled = status;
            SaveSettings();
            if (onEnableShareAnalytics != null)
                onEnableShareAnalytics(status);
        }

        public void ToggleShareAnalytics()
        {
            EnableShareAnalytics(!Settings.ShareAnalyticsEnabled);
        }

        public void SetKioskMode(bool status)
        {
            Settings.KioskMode = status;
            SaveSettings();
        }

        public void DeleteAllPlayers()
        {
            Settings.DeletePlayers();
            SaveSettings();
        }

        public void ToggleQualitygfx()
        {
            Settings.HighQualityGfx = !Settings.HighQualityGfx;
            SaveSettings();
            // CameraGameplayController.I.EnableFX(Settings.HighQualityGfx);
        }

        public void ToggleKeeperSubtitles()
        {
            Settings.KeeperSubtitlesEnabled = !Settings.KeeperSubtitlesEnabled;
            SaveSettings();
        }

        public void SetNativeLanguage(Language.LanguageCode langCode)
        {
            Settings.NativeLanguage = langCode;
            SaveSettings();
        }

        public void SetLearningContentID(LearningContentID contentID)
        {
            Settings.ContentID = contentID;
            SaveSettings();
        }

    }
}
