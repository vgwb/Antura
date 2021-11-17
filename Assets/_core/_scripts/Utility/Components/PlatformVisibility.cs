using Antura.Core;
using UnityEngine;

namespace Antura.Utilities
{
    public class PlatformVisibility : MonoBehaviour
    {
        public enum ConditionSettingEnum
        {
            none = 0,
            ShowDonate = 1,
            ShowTeacherGuide = 2
        }

        public ConditionSettingEnum ConditionSetting;
        public bool MobileOnly;
        public bool AndroidOnly;
        public bool DesktopOnly;

        public AppEditions EditionOnly;

        void Start()
        {
            if (ConditionSetting != ConditionSettingEnum.none) {
                switch (ConditionSetting) {
                    case ConditionSettingEnum.ShowDonate:
                        gameObject.SetActive(AppManager.I.AppEdition.ShowDonate);
                        break;
                    case ConditionSettingEnum.ShowTeacherGuide:
                        gameObject.SetActive(AppManager.I.AppEdition.ShowTeacherGuide);
                        break;
                }

            } else if (!Application.isEditor) {
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

                if (EditionOnly == AppEditions.LearnEnglish_Ceibal && AppManager.I.AppEdition.Edition == AppEditions.LearnEnglish_Ceibal) {
                    visible = true;
                }
                if (EditionOnly == AppEditions.LearnEnglish && AppManager.I.AppEdition.Edition == AppEditions.LearnEnglish) {
                    visible = true;
                }

                gameObject.SetActive(visible);
            }
        }
    }
}