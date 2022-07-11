using Antura.Audio;
using System;
using Antura.Language;
using UnityEngine;

namespace Antura.Core
{
    public class AppSettingsManager
    {
        public NewAppSettings NewSettings;

        private const string SETTINGS_PREFS_KEY = "OPTIONS";
        private AppSettings _settings;

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
            NewSettings = new NewAppSettings();
            if (NewSettings.Exists())
            {
                NewSettings.Load();
            }
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
                Settings.ContentID = LearningContentID.None;
                Settings.NativeLanguage = LanguageCode.english;
            }

            if (AudioManager.I != null)
                AudioManager.I.MusicEnabled = Settings.MusicEnabled;

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

        public void EnableShareAnalytics(bool status)
        {
            Debug.Log("EnableShareAnalytics " + status);
            NewSettings.ShareAnalyticsEnabled = status;
            NewSettings.Save();
        }

        public void EnableNotifications(bool status)
        {
            Debug.Log("EnableNotifications " + status);
            NewSettings.NotificationsEnabled = status;
            NewSettings.Save();
        }

        public void ToggleShareAnalytics()
        {
            EnableShareAnalytics(!NewSettings.ShareAnalyticsEnabled);
        }

        public void ToggleNotifications()
        {
            EnableNotifications(!NewSettings.NotificationsEnabled);
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

        public void SetNativeLanguage(LanguageCode langCode)
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
