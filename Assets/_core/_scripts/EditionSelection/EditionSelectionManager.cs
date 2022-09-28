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

        private LanguageCode prevLanguageCode;

        private bool firstTime => MustChooseContentEditions && !manuallyOpened;
        private bool manuallyOpened;

        public void CompleteSelection()
        {
            prevLanguageCode = AppManager.I.AppSettings.NativeLanguage;
            selectNativeLanguagePanel.Close();
            selectLearningContentPanel.Close();

            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
                currentCoroutine = null;
            }
            currentCoroutine = StartCoroutine(CompleteSelectionCO());
        }

        public void ContentEditionSelection(bool manuallyOpened)
        {
            this.manuallyOpened = manuallyOpened;
            prevLanguageCode = AppManager.I.AppSettings.NativeLanguage;
            selectNativeLanguagePanel.SelectedCode = AppManager.I.AppSettings.NativeLanguage;
            selectNativeLanguagePanel.Close();
            selectLearningContentPanel.Close();

            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
                currentCoroutine = null;
            }
            currentCoroutine = StartCoroutine(ContentEditionSelectionCO(false));
        }

        private IEnumerator CompleteSelectionCO()
        {
            yield return NativeLanguageSelectionCO();
            yield return ContentEditionSelectionCO(true);
        }

        private void Back()
        {
            StopCoroutine(currentCoroutine);
            AppManager.I.AppSettingsManager.SetNativeLanguage(prevLanguageCode);
            selectNativeLanguagePanel.Close();
            selectLearningContentPanel.Close();
            selectedNativeButton.gameObject.SetActive(false);
            GlobalUI.ShowBackButton(false);
            GlobalUI.ShowPauseMenu(true, PauseMenuType.StartScreen);
        }

        private IEnumerator NativeLanguageSelectionCO()
        {
            GlobalUI.ShowPauseMenu(false);
            GlobalUI.ShowBackButton(!firstTime, Back);
            selectedNativeButton.gameObject.SetActive(false);

            yield return selectNativeLanguagePanel.Open(firstTime);
            while (!selectNativeLanguagePanel.HasPerformedSelection)
                yield return null;

            var textRender = selectedNativeButton.GetComponentInChildren<TextRender>(true);
            textRender.SetOverridenLanguageText(selectNativeLanguagePanel.SelectedCode, LocalizationDataId.Language_Name);
            selectedNativeButton.gameObject.SetActive(true);

            bool waiting = true;
            AudioManager.I.PlayDialogue(LocalizationManager.GetLocalizationData(LocalizationDataId.Language_Name), selectNativeLanguagePanel.SelectedCode, callback: () => waiting = false, clearPreviousCallback:true);
            while (waiting) yield return null;

            GlobalUI.ShowPauseMenu(true);
        }

        private IEnumerator ContentEditionSelectionCO(bool fromLanguage)
        {
            selectLearningContentPanel.SelectedNativeCode = selectNativeLanguagePanel.SelectedCode;
            KeeperManager.I.ResetKeeper();
            HasSelectedEdition = false;
            GlobalUI.ShowPauseMenu(false);
            GlobalUI.ShowBackButton(!firstTime, Back);

            var textRender = selectedNativeButton.GetComponentInChildren<TextRender>(true);
            textRender.SetOverridenLanguageText(selectNativeLanguagePanel.SelectedCode, LocalizationDataId.Language_Name);
            selectedNativeButton.gameObject.SetActive(true);

            selectLearningContentPanel.Open(scrollToLast:!fromLanguage);
            while (!selectLearningContentPanel.HasPerformedSelection)
                yield return null;

            GlobalUI.ShowBackButton(false);
            
            var btn = selectLearningContentPanel.SelectedButton;
            bool waiting = true;
            AudioManager.I.PlayDialogue(LocalizationManager.GetLocalizationData(btn.LocKey), selectNativeLanguagePanel.SelectedCode, callback: () => waiting = false, clearPreviousCallback:true);
            while (waiting) yield return null;

            HasSelectedEdition = true;

            yield return AppManager.I.ReloadEdition();

            bool hasAnswered = false;
            if (!AppManager.I.AppSettingsManager.NewSettings.Exists())
            {
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

            AppManager.I.NavigationManager.GoToHome(true);
        }

    }
}
