using System;
using UnityEngine;

namespace Antura.Plugins.Notification
{
    public class NotificationBridge_Android : NotificationBridge_Interface
    {
        private const string FullClassName = "com.hippogames.simpleandroidnotifications.Controller";
        private const string MainActivityClassName = "com.unity3d.player.UnityPlayerActivity";

        AndroidJavaClass _class;
        AndroidJavaObject instance { get { return _class.GetStatic<AndroidJavaObject>("instance"); } }

        public NotificationBridge_Android()
        {
            Debug.Log("NotificationBridge_Android init");
        }

        public void Test()
        {
            Debug.Log("NotificationBridge_Android.Test()");
        }

        public int ScheduleNotification(NotificationParams notificationParams)
        {
            var p = notificationParams;
            var delaySeconds = (int)p.Delay.TotalSeconds;
            var delayMs = (long)p.Delay.TotalMilliseconds;

            AndroidJavaClass pluginClass = new AndroidJavaClass(FullClassName);
            if (pluginClass != null) {
                pluginClass.CallStatic("SetNotification",
                    p.Id,
                    delayMs,
                    p.Title,
                    p.Message,
                    p.Ticker,
                    p.Sound ? 1 : 0,
                    p.Vibrate ? 1 : 0,
                    p.Light ? 1 : 0,
                    p.LargeIcon,
                    GetSmallIconName(p.SmallIcon),
                    ColorToInt(p.SmallIconColor),
                    MainActivityClassName
                );
            }
            return p.Id;

        }

        public void CancelNotification(int id)
        {
            AndroidJavaClass pluginClass = new AndroidJavaClass(FullClassName);
            if (pluginClass != null) {
                pluginClass.CallStatic("CancelNotification", id);
            }

        }

        public void CancelAllNotifications()
        {
            AndroidJavaClass pluginClass = new AndroidJavaClass(FullClassName);
            if (pluginClass != null) {
                pluginClass.CallStatic("CancelAllNotifications");
            }
        }

        private int ColorToInt(Color color)
        {
            var smallIconColor = (Color32)color;
            return smallIconColor.r * 65536 + smallIconColor.g * 256 + smallIconColor.b;
        }

        private string GetSmallIconName(NotificationIcon icon)
        {
            return "anp_" + icon.ToString().ToLower();
        }
    }
}