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

        public void StartSelection()
        {
            StartCoroutine(SelectionCO());
        }

        private IEnumerator SelectionCO()
        {
            selectedNativeButton.gameObject.SetActive(false);

            selectNativeLanguagePanel.Open();
            while (!selectNativeLanguagePanel.HasPerformedSelection) yield return null;
            selectNativeLanguagePanel.Close();

            var textRender = selectedNativeButton.GetComponentInChildren<TextRender>(true);
            textRender.SetOverridenLanguageText(AppManager.I.AppSettings.NativeLanguage, LocalizationDataId.Game_Title_2);
            selectedNativeButton.gameObject.SetActive(true);

            selectLearningContentPanel.Open();
            while (!selectLearningContentPanel.HasPerformedSelection) yield return null;

            HasSelectedEdition = true;
            yield return AppManager.I.ReloadEdition();
            AppManager.I.NavigationManager.GoToHome(true);
        }
    }
}
