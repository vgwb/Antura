using Antura.Core;
using Antura.Database;
using Demigiant.DemiTools.DeUnityExtended;
using DG.DeInspektor.Attributes;
using UnityEngine;

namespace Antura.UI
{
    public class ClassroomOptionsPanel : MonoBehaviour
    {
        #region Serialized

        [DeEmptyAlert]
        [SerializeField] DeUIButton btAnalytics, btNotifications;

        #endregion

        #region Unity

        void Start()
        {
            btAnalytics.Toggle(AppManager.I.AppSettings.ShareAnalyticsEnabled, false);
            btNotifications.Toggle(AppManager.I.AppSettings.NotificationsEnabled, false);

            btAnalytics.onClick.AddListener(() =>
            {
                GlobalUI.ShowPrompt(LocalizationManager.GetNewLocalized("UI_PromptOnlineAnalytics"),
                    () =>
                    {
                        AppManager.I.AppSettingsManager.EnableShareAnalytics(true);
                        btAnalytics.Toggle(true);
                    },
                    () =>
                    {
                        AppManager.I.AppSettingsManager.EnableShareAnalytics(false);
                        btAnalytics.Toggle(false);
                    });
            });
            btNotifications.onClick.AddListener(() =>
            {
                GlobalUI.ShowPrompt(LocalizationManager.GetNewLocalized("UI_PromptNotifications"),
                    () =>
                    {
                        AppManager.I.AppSettingsManager.EnableNotifications(true);
                        btNotifications.Toggle(true);
                    }, () =>
                    {
                        AppManager.I.AppSettingsManager.EnableNotifications(false);
                        btNotifications.Toggle(false);
                    });
            });
        }

        #endregion
    }
}
