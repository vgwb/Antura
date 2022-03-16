using Antura.Core.Services.Notification;
using Antura.Core.Services.Gallery;
using Antura.Core.Services.OnlineAnalytics;
using Antura.Core.Services.WebView;

namespace Antura.Core.Services
{
    public class ServicesManager
    {
        public NotificationService Notifications;
        public GalleryService Gallery;
        public AnalyticsService Analytics;
        public WebViewService WebView;

        public ServicesManager()
        {
            Notifications = new NotificationService();
            Gallery = new GalleryService();
            //Analytics = new AnalyticsService();
            WebView = new WebViewService();
        }
    }
}
