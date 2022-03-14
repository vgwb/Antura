using System.Collections;
using UnityEngine;

namespace Antura.Core.Services.WebView
{
    public class WebViewComponent : MonoBehaviour
    {
        public static WebViewComponent I;
        private GameObject webViewGameObject;
        //private WebViewObject webViewObject;

        private int marginLeft;
        private int marginTop;
        private int marginRight;
        private int marginBottom;

        void Awake()
        {
            I = this;
        }

        public void OpenBrowser(string url, int margin_left, int margin_top, int margin_right, int margin_bottom)
        {
            Debug.Log("OpenBrowser " + url);
            marginLeft = margin_left;
            marginTop = margin_top;
            marginRight = margin_right;
            marginBottom = margin_bottom;
            //StartCoroutine(InitWeb(cleanUrl(url)));

            // OPEN WEB VIEW!
        }

        public void CloseBrowser()
        {
            //Destroy(webViewGameObject);
        }

        private IEnumerator InitWeb(string _url)
        {
            //            webViewGameObject = new GameObject("WebViewObject");
            //            webViewObject = webViewGameObject.AddComponent<WebViewObject>();
            //            webViewObject.Init(
            //                cb: (msg) => {
            //                    Debug.Log(string.Format("CallFromJS[{0}]", msg));
            //                },
            //                err: (msg) => {
            //                    Debug.Log(string.Format("CallOnError[{0}]", msg));
            //                },
            //                ld: (msg) => {
            //                    Debug.Log(string.Format("CallOnLoaded[{0}]", msg));
            //                    webViewObject.EvaluateJS(@"Unity.call('ua=' + navigator.userAgent)");
            //                },
            //                //ua: "custom user agent string",
            //                enableWKWebView: true);

            //#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
            //            webViewObject.bitmapRefreshCycle = 1;
            //#endif

            //webViewObject.SetMargins(marginLeft, marginTop, marginRight, marginBottom);
            //webViewObject.SetVisibility(true);

            //webViewObject.LoadURL(_url);

            yield break;
        }

        private string cleanUrl(string _url)
        {
            return _url.Replace(" ", "%20");
        }

    }
}
