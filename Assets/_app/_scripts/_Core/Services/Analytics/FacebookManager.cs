using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;

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
            if (choice)
            {
                CheckActivation();
            }
            else
            {
                StopEvents();
            }
        }

        private void StopEvents()
        {
            if (FB.IsInitialized)
            {
                FB.Mobile.SetAutoLogAppEventsEnabled(false);
                FB.LogOut();   
            }
        }

        void OnApplicationPause(bool pauseStatus)
        {
            // Check the pauseStatus to see if we are in the foreground or background
            if (!pauseStatus)
            {
                CheckActivation();
            }
        }

        private void CheckActivation()
        {
            if (!AppManager.I.AppSettings.ShareAnalyticsEnabled) return;

            if (FB.IsInitialized)
            {
                Activate();
            }
            else
            {
                FB.Init(OnInitComplete, OnHideUnity);
            }
        }

        private void OnInitComplete()
        {
            if (verbose)
            {
                string logMessage = string.Format(
                    "OnInitComplete IsLoggedIn='{0}' IsInitialized='{1}'",
                    FB.IsLoggedIn,
                    FB.IsInitialized);
                Debug.Log(logMessage);
            }
            Activate();
        }

        private void Activate()
        {
            FB.ActivateApp();
            FB.Mobile.SetAutoLogAppEventsEnabled(true);

            //if (!FB.IsLoggedIn) FB.LogInWithReadPermissions();

            //var parameters = new Dictionary<string, object>();
            //parameters[AppEventParameterName.Description] = "Game Activated";
            //FB.LogAppEvent(AppEventName.UnlockedAchievement, parameters : parameters);

            //FB.LogAppEvent("testEvent");
        }

        private void OnHideUnity(bool isGameShown)
        {
            if (verbose)
            {
                string logMessage = string.Format("Success Response: OnHideUnity {0}\n", isGameShown);
                Debug.Log(logMessage);
            }
        }

        public void LogAppEvent(string logEvent, float? valueToSum = default(float?), Dictionary<string, object> parameters = null)
        { 
            FB.LogAppEvent(logEvent, valueToSum, parameters);
        }

    }

}