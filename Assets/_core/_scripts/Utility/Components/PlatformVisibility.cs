using Antura.Core;
using UnityEngine;

namespace Antura.Utilities
{
    public class PlatformVisibility : MonoBehaviour
    {
        public bool MobileOnly;
        public bool AndroidOnly;
        public bool DesktopOnly;

        public AppEditions EditionOnly;

        void Start()
        {
            if (!Application.isEditor) {
                bool visible = false;

                if (MobileOnly && AppConfig.IsMobilePlatform()) {
                    visible = true;
                }
                if (AndroidOnly && Application.platform == RuntimePlatform.Android) {
                    visible = true;
                }
                if (DesktopOnly && AppConfig.IsDesktopPlatform()) {
                    visible = true;
                }

                if (EditionOnly == AppEditions.LearnEnglish_Ceibal && EditionConfig.I.Edition == AppEditions.LearnEnglish_Ceibal) {
                    visible = true;
                }
                if (EditionOnly == AppEditions.LearnEnglish && EditionConfig.I.Edition == AppEditions.LearnEnglish) {
                    visible = true;
                }

                gameObject.SetActive(visible);
            }
        }
    }
}