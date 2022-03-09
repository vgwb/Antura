using System;
using UnityEngine;
using UnityEngine.Analytics;

namespace Antura
{
    public class DataPrivacyOptIn
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void InitStepOne()
        {
            Analytics.initializeOnStartup = false;
        }

        public void EnableAnonymizingData()
        {
            Analytics.limitUserTracking = true;
        }

        // Call this when the user has given permission to collect data
        public void UserHasOptedIntoDataCollection_LetsResumeAnalyticsInitialization()
        {
            Analytics.ResumeInitialization();
        }

        // Call this when the user has given limited data collection permission
        public void UserHasOptedIntoLimitedDataCollection_LetsResumeAnalyticsInitialization()
        {
            EnableAnonymizingData();
            Analytics.ResumeInitialization();
        }

        // Call this when the user doesn't want any data collection.
        public void UserHasOptedOutOfAllDataCollection()
        {
            // Don't call ResumeInitialization
            // But disable Analytics so the code knows to shutdown
            DisableAnalyticsCompletely();
        }

        // If you want to disable Analytics completely during runtime
        public void DisableAnalyticsCompletely()
        {
            Analytics.enabled = false;
            Analytics.deviceStatsEnabled = false;
            PerformanceReporting.enabled = false;

            //            Analytics.limitUserTracking = false;
            //            Analytics.deviceStatsEnabled = false;
        }
    }
}
