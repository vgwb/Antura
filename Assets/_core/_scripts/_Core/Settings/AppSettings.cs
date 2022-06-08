using Antura.Language;
using Antura.Profile;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Core
{

    public class NewAppSettings
    {
        public bool NotificationsEnabled = true;
        public bool ShareAnalyticsEnabled = true;
        // public bool MusicEnabled = true;
        // public bool SubtitlesEnabled = true;

        public void Save()
        {
            PlayerPrefs.SetInt("NotificationsEnabled", NotificationsEnabled ? 1 : 0);
            PlayerPrefs.SetInt("AnalyticsEnabled", ShareAnalyticsEnabled ? 1 : 0);
            // PlayerPrefs.SetInt("MusicEnabled", MusicEnabled ? 1 : 0);
            // PlayerPrefs.SetInt("SubtitlesEnabled", SubtitlesEnabled ? 1 : 0);
        }

        public void Load()
        {
            NotificationsEnabled = PlayerPrefs.GetInt("NotificationsEnabled", 1) == 1;
            ShareAnalyticsEnabled = PlayerPrefs.GetInt("AnalyticsEnabled", 1) == 1;
        }

        public bool Exists()
        {
            return PlayerPrefs.HasKey("NotificationsEnabled");
        }
    }

    /// <summary>
    /// Defines app settings that must be saved locally.
    /// </summary>
    [Serializable]
    public class AppSettings
    {
        public LearningContentID ContentID;
        public LanguageCode NativeLanguage = LanguageCode.english;

        // not used anymore.. but could be useful in the future
        public bool HighQualityGfx = false;

        // the uuid of currently active player
        public string LastActivePlayerUUID;

        // to enable subtitles in the Keeper Widget that shows what he's saying
        public bool KeeperSubtitlesEnabled = true;

        public bool MusicEnabled = true;

        //        public bool ShareAnalyticsEnabled = true;

        // if set the app starts is special scene mode, used in museums and demo installations
        public bool KioskMode = false;

        // we save the current AppVersion maybe we should compare when installing updated versions
        public string AppVersion;

        // the list of saved players
        public List<PlayerIconData> SavedPlayers = new List<PlayerIconData>();

        public void SetAppVersion(string _version)
        {
            AppVersion = _version;
        }

        public void DeletePlayers()
        {
            SavedPlayers = new List<PlayerIconData>();
            LastActivePlayerUUID = "";
        }
    }
}
