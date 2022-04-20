using Antura.Core;
using Antura.Core.Services.WebView;
using UnityEngine;
using TMPro;

namespace Antura.Kiosk
{
    public class WebPanel : MonoBehaviour
    {
        public TextMeshProUGUI WebViewTitle;
        private float idleTime;
        private const int idleTimeDuration = 5;

        public void Open(string url)
        {

            WebViewTitle.text = "Loading ...";
            gameObject.SetActive(true);
            WebViewComponent.I.OpenBrowser(url, 0, 130, 0, 0);
            idleTime = Time.time + idleTimeDuration;
        }

        public void OnClose()
        {
            WebViewComponent.I.CloseBrowser();
            gameObject.SetActive(false);
        }

        void Update()
        {
            if (idleTime > 0 && Time.time > idleTime)
            {
                idleTime = -1;
                WebViewTitle.text = "www.antura.org";
            }
        }
    }
}
