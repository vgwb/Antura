using System;
using System.Linq;
using UnityEngine;

namespace Antura.Core
{

    /// <summary>
    /// Container for application-wide static constants.
    /// </summary>
    // TODO refactor: enforce code convention
    public static class AppConfig
    {
        /// <summary>
        /// Version of the Static Database Scheme.
        /// v1.0.7 - added ArabicFemale to LocalizationData
        /// </summary>
        public const string StaticDbSchemeVersion = "1.0.0.0";

        /// <summary>
        /// Version of the MySQL Database Scheme.
        /// @note: Change with EXTREME CAUTION, as the MySQL databases are regenerated (and thus the data is removed) when a change is detected.
        /// 1.0.2.0 (20211117): added language edition index to player profile
        /// 1.0.1.0 (20190701): added colors to player profile
        /// </summary>
        public const string DynamicDbSchemeVersion = "1.0.2.0";

        #region Debug Options

        /// <summary>
        /// Logs all MySQL database inserts.
        /// Set to FALSE for production.
        /// </summary>
        public static bool DebugLogDbInserts = false;

        // for Test configuration
        public static bool DisableFirstContact = false;

        public static bool MinigameTutorialsEnabled = true;

        // disables the listing and use of the Shaddah from all the words/book
        public static bool DisableShaddah = true;

        #endregion

        #region Application Constants

        // public URLs
        public const string UrlWebsite = "http://www.antura.org";
        public const string UrlPrivacy = "http://www.antura.org/en/privacy-policy/";
        public const string UrlDonate = "http://www.antura.org/donate/";
        public const string UrlUploadData = "https://upload.antura.org";
        public const string UrlStoreiOSApple = "https://itunes.apple.com/us/app/antura-and-the-letters/id1210334699?ls=1&mt=8";
        public const string UrlStoreAndroidGoogle = "https://play.google.com/store/apps/details?id=org.eduapp4syria.antura";
        public const string UrlCommunityTelegram = "https://t.me/antura";
        public const string UrlCommunityFacebook = "https://www.facebook.com/antura.initiative";
        public const string UrlCommunityTwitter = "https://twitter.com/AnturaGame";
        public const string UrlCommunityInstagram = "https://www.instagram.com/anturagame/";
        public const string UrlSupportForm = "https://docs.google.com/forms/d/e/1FAIpQLScWxs5I0w-k8GlIgPFKoWBitMVJ9gxxJlKvGKOXzZsnAA0qNw/viewform";
        public const string UrlGithubRepository = "https://github.com/vgwb/Antura";
        public const string UrlDeveloperDocs = "https://docs.antura.org";

        // files
        public const string PdfAndroidInstall = "TeacherManual.pdf";

        // the directories of exported / imported databases
        public const string DbFileExtension = ".sqlite3";
        public const string DbPlayersFolder = "players";
        public const string DbExportFolder = "db_export";
        public const string DbImportFolder = "db_import";
        public const string DbJoinedFolder = "db_export_joined";

        // Range and Constrain values
        public const float MinPlayerAge = 4;
        public const float MaxPlayerAge = 10;
        public const int MinStage = 1;
        public const int MaxStage = 6;
        public const int MinMoodValue = 1;
        public const int MaxMoodValue = 5;
        public const int MaxNumberOfPlayerProfiles = 5;
        public const int MinMiniGameScore = 0;
        public const int MaxMiniGameScore = 3;

        // used for float to float comparisons
        public const float EPSILON = 0.00001f;

        // Resource Paths
        public const string RESOURCES_DIR_AVATARS = "Images/Avatars/";
        public const string RESOURCES_PATH_DEBUG_PANEL = "Prefabs/Debug/Debug UI Canvas";

        #endregion

        public static string GetPlayerUUIDFromDatabaseFilename(string fileName)
        {
            return fileName.Split('/').Last().Split('\\').Last().Replace("Antura_Player_", "").Replace(DbFileExtension, "");
        }

        public static string GetPlayerDatabaseFilename(string playerUuid)
        {
            return "Antura_Player_" + playerUuid + DbFileExtension;
        }

        public static string GetPlayerDatabaseFilenameForExport(string playerUuid)
        {
            return "export_Antura_Player_" + playerUuid + "_" + DateTime.Now.ToString("yyyy-MM-dd_HHmm") + DbFileExtension;
        }

        public static string GetJoinedDatabaseFilename()
        {
            return "Antura_Joined_" + DateTime.Now.ToString("yyyy-MM-dd_HHmm") + DbFileExtension;
        }

        public static bool IsDesktopPlatform()
        {
            return (Application.platform == RuntimePlatform.WindowsPlayer ||
                    Application.platform == RuntimePlatform.OSXPlayer ||
                    Application.platform == RuntimePlatform.LinuxPlayer);
        }

        public static bool IsMobilePlatform()
        {
            return (Application.platform == RuntimePlatform.Android ||
                    Application.platform == RuntimePlatform.IPhonePlayer);
        }

        public static bool IsMobileTablet()
        {
            if (IsMobilePlatform())
            {
                return (DeviceDiagonalSizeInInches() > 6.5f);
            }
            return false;
        }

        public static bool IsMobileSMartphone()
        {
            if (IsMobilePlatform())
            {
                return (DeviceDiagonalSizeInInches() <= 6.5f);
            }
            return false;
        }

        private static float DeviceDiagonalSizeInInches()
        {
            float screenWidth = Screen.width / Screen.dpi;
            float screenHeight = Screen.height / Screen.dpi;
            float diagonalInches = Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));
            return diagonalInches;
        }
    }
}
