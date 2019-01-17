using UnityEngine;
using System;

namespace Antura.Plugins.Notification
{
#if (UNITY_IPHONE || UNITY_EDITOR)
    public class NotificationBridge_iOS : NotificationBridge_Interface
    {
        public NotificationBridge_iOS()
        {
            UnityEngine.iOS.NotificationServices.RegisterForNotifications(
            UnityEngine.iOS.NotificationType.Alert |
            UnityEngine.iOS.NotificationType.Badge |
            UnityEngine.iOS.NotificationType.Sound
            );
            Debug.Log("NotificationBridge_iOS init - Registered for Notifications");
        }

        public void Test()
        {
            Debug.Log("NotificationBridge_iOS.Test()");
        }

        public int ScheduleNotification(NotificationParams notificationParams)
        {
            var p = notificationParams;
            var delaySeconds = (int)p.Delay.TotalSeconds;
            var delayMs = (long)p.Delay.TotalMilliseconds;

            UnityEngine.iOS.LocalNotification notification = new UnityEngine.iOS.LocalNotification();
            DateTime now = DateTime.Now;
            DateTime fireDate = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second).AddSeconds(delaySeconds);
            notification.fireDate = fireDate;
            notification.alertBody = p.Message;
            notification.alertAction = p.Title;
            notification.hasAction = false;
            UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(notification);

            return (int)fireDate.Ticks;
        }

        public void CancelNotification(int id)
        {
            foreach (UnityEngine.iOS.LocalNotification notif in UnityEngine.iOS.NotificationServices.scheduledLocalNotifications) {
                if ((int)notif.fireDate.Ticks == id) {
                    UnityEngine.iOS.NotificationServices.CancelLocalNotification(notif);
                }
            }
        }

        public void CancelAllNotifications()
        {
            // TODO check taht this method could be bugged.. search for an alternative
            UnityEngine.iOS.NotificationServices.ClearLocalNotifications();
        }

    }
#endif
}