using System.Collections.Generic;
using UnityEngine;

#if FB_SDK
using Facebook.Unity;
#endif

namespace Antura.Core.Services.OnlineAnalytics
{
    public class FacebookManager : MonoBehaviour
    {
        public bool verbose;

        void Awake()
        {
            CheckActivation();
            AppManager.I.AppSettingsManager.onEnableShareAnalytics += HandleEnableShareAnalytics;
        }

        private void HandleEnableShareAnalytics(bool choice)
        {
            if (choice) {
                CheckActivation();
            } else {
                StopEvents();
            }
        }

        private void StopEvents()
        {
#if FB_SDK
            if (FB.IsInitialized) {
                FB.Mobile.SetAutoLogAppEventsEnabled(false);
                FB.LogOut();
            }
#endif
        }

        void OnApplicationPause(bool pauseStatus)
        {
            // Check the pauseStatus to see if we are in the foreground or background
            if (!pauseStatus) {
                CheckActivation();
            }
        }

        private void CheckActivation()
        {
            if (!AppManager.I.AppSettings.ShareAnalyticsEnabled) return;

#if FB_SDK
            if (FB.IsInitialized) {
                Activate();
            } else {
                FB.Init(OnInitComplete, OnHideUnity);
            }
#endif
        }

        private void OnInitComplete()
        {
#if FB_SDK
            if (verbose) {
                string logMessage = string.Format(
                    "OnInitComplete IsLoggedIn='{0}' IsInitialized='{1}'",
                    FB.IsLoggedIn,
                    FB.IsInitialized);
                Debug.Log(logMessage);
            }
#endif
            Activate();
        }

        private void Activate()
        {
#if FB_SDK
            FB.ActivateApp();
            FB.Mobile.SetAutoLogAppEventsEnabled(true);
#endif
            //if (!FB.IsLoggedIn) FB.LogInWithReadPermissions();

            //var parameters = new Dictionary<string, object>();
            //parameters[AppEventParameterName.Description] = "Game Activated";
            //FB.LogAppEvent(AppEventName.UnlockedAchievement, parameters : parameters);

            //FB.LogAppEvent("testEvent");
        }

        private void OnHideUnity(bool isGameShown)
        {
            if (verbose) {
                string logMessage = string.Format("Success Response: OnHideUnity {0}\n", isGameShown);
                Debug.Log(logMessage);
            }
        }

        public void LogAppEvent(string logEvent, float? valueToSum = default(float?), Dictionary<string, object> parameters = null)
        {
#if FB_SDK
            if (!FB.IsInitialized) {
                CheckActivation();
            } else {
                FB.LogAppEvent(logEvent, valueToSum, parameters);
            }
#endif
        }

    }

}