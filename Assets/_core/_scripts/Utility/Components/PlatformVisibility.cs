using Antura.Core;
using UnityEngine;

namespace Antura.Utilities
{
    public class PlatformVisibility : MonoBehaviour
    {
        public enum Platform
        {
            Mobile,
            Android,
            Desktop
        }

        [Header("Visible ONLY on this platform:")]
        public Platform PlatformType;

        void OnEnable()
        {

            bool visible = false;

            if (PlatformType == Platform.Mobile && AppConfig.IsMobilePlatform())
            {
                visible = true;
            }
            if (PlatformType == Platform.Android && Application.platform == RuntimePlatform.Android)
            {
                visible = true;
            }
            if (PlatformType == Platform.Desktop && AppConfig.IsDesktopPlatform())
            {
                visible = true;
            }

            gameObject.SetActive(visible);
        }

    }
}
