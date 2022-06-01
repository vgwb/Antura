using Antura.Core.Services.Notification;
using Antura.Core.Services.Gallery;
using Antura.Core.Services.OnlineAnalytics;
using Antura.Core.Services.WebView;
using UnityEngine;

namespace Antura.Core.Services
{
    public class ServicesManager
    {
        public NotificationService Notifications;
        public GalleryService Gallery;
        public Analytics Analytics;
        public WebViewService WebView;

        public ServicesManager(GameObject _gameObject)
        {
            Notifications = new NotificationService(_gameObject);
            Gallery = new GalleryService();

            Analytics = _gameObject.AddComponent<Analytics>();
            Analytics.Init();

            WebView = new WebViewService();
        }
    }
}
