﻿using Antura.Database;
using System;
using UnityEngine;
#if UNITY_ANDROID || UNITY_IOS
using Unity.Notifications;
using NotificationSamples;
#endif

namespace Antura.Core.Services.Notification
{
    public class NotificationService
    {
#if UNITY_ANDROID || UNITY_IOS
        public GameNotificationsManager NotificationsManager;
#endif
        public const string ChannelId = "game_channel0";
        private bool inizialized = false;
        private GameObject myGameObject;
        protected int playReminderHour = 18;

        public NotificationService(GameObject _gameObject)
        {
            myGameObject = _gameObject;
            //Init();
        }

        public void Init()
        {
#if UNITY_ANDROID || UNITY_IOS
            if (!AppManager.I.AppEdition.EnableNotifications)
                return;

            if (!inizialized)
            {

                if (DebugConfig.I.VerboseAntura)
                    Debug.Log("NotificationService Init");
                NotificationsManager = myGameObject.AddComponent<GameNotificationsManager>();
                NotificationsManager.Initialize();
                inizialized = true;
            }
#endif
        }

        #region main
        /// <summary>
        /// automatically call everything to setup Notifications at AppSuspended
        /// </summary>
        public void AppSuspended()
        {
            if (AppManager.I.AppEdition.EnableNotifications && AppManager.I.AppSettingsManager.Settings.NotificationsEnabled)
            {
                PrepareNextLocalNotification();
            }
        }

        /// <summary>
        /// automatically restore all Notifications at AppResumed
        /// </summary>
        public void AppResumed()
        {
#if UNITY_ANDROID || UNITY_IOS
            // if (!NotificationsManager.Initialized)
            // {
            //     Init();
            // }
            if (NotificationsManager != null && NotificationsManager.Initialized)
            {
                NotificationsManager.DismissAllNotifications();
            }
#endif
        }
        #endregion

        private void PrepareNextLocalNotification()
        {
            //DeleteAllLocalNotifications();
            //Debug.Log("Next Local Notifications prepared");
            ScheduleNotification(
                GetDateTimeTomorrow(),
                "Antura and the Letters",
                LocalizationManager.GetLocalizationData(LocalizationDataId.UI_Notification_24h).NativeText
            );

            //NotificationManager.ScheduleSimpleWithAppIcon(
            //    TimeSpan.FromSeconds(60),
            //    "Antura and the Letters",
            //    "Test notification after closing the app [60 seconds]",
            //    Color.blue
            //);
        }

        #region direct plugins methods

        public void ScheduleNotification(DateTime deliveryTime, string title, string message)
        {
#if UNITY_ANDROID || UNITY_IOS
            var notification = NotificationsManager.CreateNotification();

            notification.Title = title;
            notification.Body = message;
            // notification.Group = ChannelId;
            // notification.LargeIcon = "icon_antura";
            NotificationsManager.ScheduleNotification(notification, deliveryTime);

            Debug.Log("ScheduleNotification - " + deliveryTime);
#endif
        }

        public void DeleteAllLocalNotifications()
        {

        }
        #endregion

        #region time utilities
        private DateTime GetDateTimeTomorrow()
        {
            DateTime deliveryTime = DateTime.Now.ToLocalTime().AddDays(1);
            deliveryTime = new DateTime(deliveryTime.Year, deliveryTime.Month, deliveryTime.Day, playReminderHour, 0, 0,
                DateTimeKind.Local);
            return deliveryTime;
            //return DateTime.Now.AddHours(20);
        }

        private DateTime GetDateTimeInMinutes(int minutes)
        {
            return DateTime.Now.ToLocalTime().AddMinutes(minutes);
        }

        private int CalculateSecondsToTomorrowMidnight()
        {
            TimeSpan ts = DateTime.Today.AddDays(2).Subtract(DateTime.Now);
            return (int)ts.TotalSeconds;
        }
        #endregion

        #region tests
        public void TestLocalNotification()
        {
            //Debug.Log("Tomorrows midnight is in " + CalculateSecondsToTomorrowMidnight() + " seconds");
            var description = LocalizationManager.GetLocalizationData(LocalizationDataId.UI_Notification_24h).NativeText;
            ScheduleNotification(
                GetDateTimeInMinutes(1),
                "Antura and the Letters",
                description
            );
        }
        #endregion
    }
}
