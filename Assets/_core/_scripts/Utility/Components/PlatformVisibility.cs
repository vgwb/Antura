using Antura.Core;
using UnityEngine;

namespace Antura.Utilities
{
    public class PlatformVisibility : MonoBehaviour
    {
        public bool EditorOnly;
        public enum Platform
        {
            All = 0,
            Mobile = 1,
            Android = 2,
            Desktop = 3
        }

        [Header("Visible ONLY on this platform:")]
        public Platform PlatformType;

        void OnEnable()
        {

            bool visible = false;

            if (EditorOnly && Application.isEditor)
            {
                visible = true;
            }

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

            if (PlatformType == Platform.All)
            {
                visible = true;
            }

            gameObject.SetActive(visible);
        }

    }
}
