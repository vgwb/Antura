using Antura.Core;
using Antura.Database;
using Antura.Language;
using Antura.UI;
using System;
using System.Collections;
using Antura.Audio;
using Antura.Keeper;
using UnityEngine;

namespace Antura.Scenes
{
    public class EditionSelectionManager : MonoBehaviour
    {
        public static bool HasSelectedEdition;
        public static bool MustChooseContentEditions => !HasSelectedEdition && AppManager.I.AppEdition.HasMultipleContentEditions && AppManager.I.Player == null;

        public SelectNativeLanguagePanel selectNativeLanguagePanel;
        public SelectLearningContentPanel selectLearningContentPanel;
        public UIButton selectedNativeButton;

        private Coroutine currentCoroutine;

        public void CompleteSelection(bool firstTime)
        {
            selectNativeLanguagePanel.Close();
            selectLearningContentPanel.Close();

            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
                currentCoroutine = null;
            }
            currentCoroutine = StartCoroutine(CompleteSelectionCO(firstTime));
        }

        public void ContentEditionSelection()
        {
            selectNativeLanguagePanel.Close();
            selectLearningContentPanel.Close();

            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
                currentCoroutine = null;
            }
            currentCoroutine = StartCoroutine(ContentEditionSelectionCO(false));
        }

        private IEnumerator CompleteSelectionCO(bool firstTime)
        {
            yield return NativeLanguageSelectionCO(firstTime);
            yield return ContentEditionSelectionCO(true);
        }

        private IEnumerator NativeLanguageSelectionCO(bool firstTime)
        {
            GlobalUI.ShowPauseMenu(false);
            selectedNativeButton.gameObject.SetActive(false);

            yield return selectNativeLanguagePanel.Open(firstTime);
            while (!selectNativeLanguagePanel.HasPerformedSelection)
                yield return null;

            var textRender = selectedNativeButton.GetComponentInChildren<TextRender>(true);
            textRender.SetOverridenLanguageText(AppManager.I.AppSettings.NativeLanguage, LocalizationDataId.Language_Name);
            selectedNativeButton.gameObject.SetActive(true);

            bool waiting = true;
            AudioManager.I.PlayDialogue(LocalizationManager.GetLocalizationData(LocalizationDataId.Language_Name), AppManager.I.AppSettings.NativeLanguage, callback: () => waiting = false, clearPreviousCallback:true);
            while (waiting) yield return null;

            GlobalUI.ShowPauseMenu(true);
        }

        private IEnumerator ContentEditionSelectionCO(bool fromLanguage)
        {
            KeeperManager.I.StopSpeaking();
            HasSelectedEdition = false;
            GlobalUI.ShowPauseMenu(false);

            var textRender = selectedNativeButton.GetComponentInChildren<TextRender>(true);
            textRender.SetOverridenLanguageText(AppManager.I.AppSettings.NativeLanguage, LocalizationDataId.Language_Name);
            selectedNativeButton.gameObject.SetActive(true);

            selectLearningContentPanel.Open(scrollToLast:!fromLanguage);
            while (!selectLearningContentPanel.HasPerformedSelection)
                yield return null;

            var btn = selectLearningContentPanel.SelectedButton;

            bool waiting = true;
            AudioManager.I.PlayDialogue(LocalizationManager.GetLocalizationData(btn.LocKey), AppManager.I.AppSettings.NativeLanguage, callback: () => waiting = false, clearPreviousCallback:true);
            while (waiting) yield return null;

            bool hasAnswered = false;
            if (!AppManager.I.AppSettingsManager.NewSettings.Exists())
            {
                hasAnswered = false;
                GlobalUI.ShowPrompt(LocalizationDataId.UI_PromptOnlineAnalytics,
                () =>
                {
                    AppManager.I.AppSettingsManager.EnableShareAnalytics(true);
                    hasAnswered = true;
                },
                () =>
                {
                    AppManager.I.AppSettingsManager.EnableShareAnalytics(false);
                    hasAnswered = true;
                });

                while (!hasAnswered)
                    yield return null;

                hasAnswered = false;
                GlobalUI.ShowPrompt(LocalizationDataId.UI_PromptNotifications,
                () =>
                {
                    AppManager.I.AppSettingsManager.EnableNotifications(true);
                    hasAnswered = true;
                },
                () =>
                {
                    AppManager.I.AppSettingsManager.EnableNotifications(false);
                    hasAnswered = true;
                });

                while (!hasAnswered)
                    yield return null;
            }

            HasSelectedEdition = true;
            yield return AppManager.I.ReloadEdition();
            AppManager.I.NavigationManager.GoToHome(true);
        }

    }
}
