using System;
using UnityEngine;

namespace Antura.Plugins.Notification
{
    public class NotificationParams
    {
        /// <summary>
        /// Use random id for each new notification.
        /// </summary>
        public int Id;
        public TimeSpan Delay;
        public string Title;
        public string Message;
        public string Ticker;
        public bool Sound = true;
        public bool Vibrate = true;
        public bool Light = true;
        public NotificationIcon SmallIcon;
        public Color SmallIconColor;
        /// <summary>
        /// Use "" for simple notification. Use "app_icon" to use the app icon. Use custom value but first place image to "simple-android-notifications.aar/res/". To modify "aar" file just rename it to "zip" and back.
        /// </summary>
        public string LargeIcon;
    }
}