using Antura.Database;
using Antura.Language;
using Antura.Scenes;
using Antura.UI;
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Antura.Core
{
    public class PanelAppUpdate : MonoBehaviour
    {
        [FormerlySerializedAs("EnglishText")]
        public TextRender HelpText;

        [FormerlySerializedAs("ArabicText")]
        public TextRender LearningText;

        public void Init()
        {
            var titleText = LocalizationManager.GetLocalizationData(LocalizationDataId.UI_Attention);
            var panelText = LocalizationManager.GetLocalizationData(LocalizationDataId.UI_AlertFinalRelease);

            LearningText.text = $"<b>{titleText.LearningText}</b>\n\n{panelText.LearningText}";
            HelpText.text = AppManager.I.ContentEdition.LearnMethod.ShowHelpText ? $"<b>{titleText.HelpText}</b>\n\n{panelText.HelpText}" : "";

            gameObject.SetActive(true);
        }

        public void OnBtnContinue()
        {
            gameObject.SetActive(false);
            if (!AppManager.I.AppSettingsManager.NewSettings.ShareAnalyticsEnabled)
            {
                OnlineAnalyticsRequest();
            }
            else
            {
                Close();
            }
        }

        public void OnlineAnalyticsRequest()
        {
            GlobalUI.ShowPrompt(LocalizationDataId.UI_PromptOnlineAnalytics,
            () =>
            {
                AppManager.I.AppSettingsManager.EnableShareAnalytics(true);
                Close();
            },
            () =>
            {
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
