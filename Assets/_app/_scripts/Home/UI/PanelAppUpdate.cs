using Antura.Database;
using Antura.Scenes;
using Antura.UI;
using System;
using UnityEngine;

namespace Antura.Core
{
    public class PanelAppUpdate : MonoBehaviour
    {
        public TextRender EnglishText;
        public TextRender ArabicText;

        public void Init()
        {
            var titleText = LocalizationManager.GetLocalizationData(LocalizationDataId.UI_Attention);
            var panelText = LocalizationManager.GetLocalizationData(LocalizationDataId.UI_AlertFinalRelease);

            EnglishText.text = "<b>" + titleText.English + "</b>\n\n" + panelText.English;
            ArabicText.text = "<b>" + titleText.Arabic + "<b/>\n\n" + panelText.Arabic;

            gameObject.SetActive(true);
        }

        public void OnBtnContinue()
        {
            gameObject.SetActive(false);
            if (!AppManager.I.AppSettings.ShareAnalyticsEnabled) {
                OnlineAnalyticsRequest();
            } else {
                Close();
            }
        }

        public void OnlineAnalyticsRequest()
        {
            GlobalUI.ShowPrompt(LocalizationDataId.UI_PromptOnlineAnalytics, () => {
                AppManager.I.AppSettingsManager.EnableShareAnalytics(true);
                Close();
            }, () => {
                AppManager.I.AppSettingsManager.EnableShareAnalytics(false);
                Close();
            });
        }

        private void Close()
        {
            (BootstrapScene.I as BootstrapScene).CloseAppUpdatePanel();
        }

    }
}