using Antura.Language;
using Antura.Profile;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Core
{
    /// <summary>
    /// Defines app settings that must be saved locally.
    /// </summary>
    [Serializable]
    public class AppSettings
    {
        public bool FirstRun = true;

        public LearningContentID ContentID;
        public LanguageCode NativeLanguage = LanguageCode.english;

        public bool NotificationsEnabled = true;
        public bool ShareAnalyticsEnabled = true;

        // 0 is off, 1,2,3,4,5 is on with the class selected
        public int ClassRoomMode = 0;
        public bool isClassroomMode() { return ClassRoomMode > 0; }

        // not used anymore.. but could be useful in the future
        public bool HighQualityGfx = false;

        // the uuid of currently active player
        public string LastActivePlayerUUID;

        // to enable subtitles in the Keeper Widget that shows what he's saying
        public bool KeeperSubtitlesEnabled = true;

        public bool MusicEnabled = true;

        // if set the app starts is special scene mode, used in museums and demo installations
        public bool KioskMode = false;

        // we save the current AppVersion maybe we should compare when installing updated versions
        public string AppVersion;

        // the list of saved players
        public List<PlayerProfilePreview> SavedPlayers = new List<PlayerProfilePreview>();

        public void SetAppVersion(string _version)
        {
            AppVersion = _version;
        }

        public void DeletePlayers()
        {
            SavedPlayers = new List<PlayerProfilePreview>();
            LastActivePlayerUUID = "";
        }
    }
}
