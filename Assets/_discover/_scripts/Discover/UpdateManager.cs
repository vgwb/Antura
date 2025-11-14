using Antura.Core;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using System.Collections;

namespace Discover
{
    public class UpdateManager : MonoBehaviour
    {
        public static UpdateManager Instance { get; private set; }
        private string updateVersionUrl = "https://antura.org/latest-version.txt";

        private const string PREFS_KEY_LATEST_VERSION = "LatestAppVersion";
        private const string PREFS_KEY_LAST_FETCH_TIME = "LastAppVersionFetchTime";
        private const float FETCH_INTERVAL_HOURS = 24f;

        private string currentVersion;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                CheckForUpdate();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void CheckForUpdate()
        {
            currentVersion = AppManager.I.AppEdition.AppVersion;

            string remoteVersion = PlayerPrefs.GetString(PREFS_KEY_LATEST_VERSION, currentVersion);
            float lastFetchTime = PlayerPrefs.GetFloat(PREFS_KEY_LAST_FETCH_TIME, 0f);
            bool needsFetch = (Time.realtimeSinceStartup / 3600f - lastFetchTime >= FETCH_INTERVAL_HOURS) || (remoteVersion == currentVersion);
            if (!needsFetch)
            {
                if (IsRemoteNewer(remoteVersion, currentVersion))
                {
                    ShowUpdateNotification(remoteVersion);
                }
                return;
            }

            StartCoroutine(FetchAndCompareVersion());
        }

        private IEnumerator FetchAndCompareVersion()
        {
            using (var www = UnityWebRequest.Get(updateVersionUrl))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    string fetchedVersion = www.downloadHandler.text.Trim();
                    PlayerPrefs.SetString(PREFS_KEY_LATEST_VERSION, fetchedVersion);
                    PlayerPrefs.SetFloat(PREFS_KEY_LAST_FETCH_TIME, Time.realtimeSinceStartup / 3600f);
                    PlayerPrefs.Save();

                    if (IsRemoteNewer(fetchedVersion, currentVersion))
                    {
                        ShowUpdateNotification(fetchedVersion);
                    }
                }
                else
                {
                    Debug.LogWarning($"Version fetch failed: {www.error}. Using cached version.");
                    CheckForUpdate();
                }
            }
        }

        private bool IsRemoteNewer(string remoteVersion, string currentVersion)
        {
            // Semantic versioning comparison (e.g., "1.2.3" > "1.2.2")
            var remoteParts = remoteVersion.Split('.').Select(int.Parse).ToArray();
            var currentParts = currentVersion.Split('.').Select(int.Parse).ToArray();

            for (int i = 0; i < Mathf.Min(remoteParts.Length, currentParts.Length); i++)
            {
                if (remoteParts[i] > currentParts[i])
                    return true;
                if (remoteParts[i] < currentParts[i])
                    return false;
            }
            return remoteParts.Length > currentParts.Length; // 1.2 < "1.2.0"
        }

        private void ShowUpdateNotification(string remoteVersion)
        {
            Debug.Log($"Update available! Current: {currentVersion}, Latest: {remoteVersion}");
        }
    }
}
