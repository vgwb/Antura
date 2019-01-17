using System;
using UnityEngine;

namespace Antura.Plugins.Notification
{

    public class NotificationBridge_Editor : NotificationBridge_Interface
    {
        public NotificationBridge_Editor()
        {
            //Debug.Log("NotificationBridge_Editor init");
        }

        public void Test()
        {
            //Debug.Log("NotificationBridge_Editor.Test()");
        }

        public int ScheduleNotification(NotificationParams notificationParams)
        {
            //Debug.LogWarning("Local Notifications are not supported for current platform. Only iOS and Android are supported!");
            return 0;
        }

        public void CancelNotification(int id)
        {
        }

        public void CancelAllNotifications()
        {
        }

    }
}