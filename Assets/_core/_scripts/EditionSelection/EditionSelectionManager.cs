using System;
using System.Collections;
using Antura.Core;
using Antura.Database;
using Antura.UI;
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
            currentCoroutine = StartCoroutine(ContentEditionSelectionCO());
        }

        private IEnumerator CompleteSelectionCO(bool firstTime)
        {
            yield return NativeLanguageSelectionCO(firstTime);
            yield return ContentEditionSelectionCO();
        }

        private IEnumerator NativeLanguageSelectionCO(bool firstTime)
        {
            GlobalUI.ShowPauseMenu(false);
            selectedNativeButton.gameObject.SetActive(false);

            yield return selectNativeLanguagePanel.Open(firstTime);
            while (!selectNativeLanguagePanel.HasPerformedSelection)
                yield return null;

            GlobalUI.ShowPauseMenu(true);
        }

        private IEnumerator ContentEditionSelectionCO()
        {
            GlobalUI.ShowPauseMenu(false);

            var textRender = selectedNativeButton.GetComponentInChildren<TextRender>(true);
            textRender.SetOverridenLanguageText(AppManager.I.AppSettings.NativeLanguage, LocalizationDataId.Language_Name);
            selectedNativeButton.gameObject.SetActive(true);

            selectLearningContentPanel.Open();
            while (!selectLearningContentPanel.HasPerformedSelection)
                yield return null;

            HasSelectedEdition = true;
            yield return AppManager.I.ReloadEdition();
            AppManager.I.NavigationManager.GoToHome(true);
        }

    }
}
