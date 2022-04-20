using System;
using System.Collections;
using System.Collections.Generic;
using Antura.Audio;
using Antura.Core;
using Antura.Database;
using Antura.Language;
using UnityEngine;

namespace Antura.UI
{
    public class SelectNativeLanguagePanel : MonoBehaviour
    {
        public TextRender QuestionText;

        public IEnumerator SwitchQuestionTextCO()
        {
            int nativeCodeIndex = 0;
            while (true)
            {
                var key = LocalizationDataId.Game_Title_2;
                yield return AudioManager.I.PlayDialogue(LocalizationManager.GetLocalizationData(key), AvailableNativeCodes[nativeCodeIndex]);

                QuestionText.SetOverridenLanguageText(AvailableNativeCodes[nativeCodeIndex], key);

                nativeCodeIndex++;
                if (nativeCodeIndex >= AvailableNativeCodes.Count) nativeCodeIndex = 0;
                yield return new WaitForSeconds(3f);
            }
        }

        public SelectNativeLanguageButton prefabButton;
        private List<SelectNativeLanguageButton> buttons = new List<SelectNativeLanguageButton>();

        private List<LanguageCode> AvailableNativeCodes = new List<LanguageCode>();

        public void Awake()
        {
            // Find all supported native languages, and create a button for each
            for (var iContentEdition = 0; iContentEdition < AppManager.I.AppEdition.ContentEditions.Length; iContentEdition++)
            {
                var contentEditionConfig = AppManager.I.AppEdition.ContentEditions[iContentEdition];
                foreach (var supportedNativeLanguage in contentEditionConfig.SupportedNativeLanguages)
                {
                    var buttonGO = Instantiate(prefabButton.gameObject, prefabButton.transform.parent, true);
                    buttonGO.transform.localScale = Vector3.one;
                    buttonGO.SetActive(true);
                    var button = buttonGO.GetComponent<SelectNativeLanguageButton>();
                    button.Setup(supportedNativeLanguage);
                    buttons.Add(button);
                    AvailableNativeCodes.Add(supportedNativeLanguage);
                }
            }
            prefabButton.gameObject.SetActive(false);
        }

        public void OnEnable()
        {
            HasPerformedSelection = false;
            StartCoroutine(SwitchQuestionTextCO());
        }

        public bool HasPerformedSelection;
        public void ConfirmSelection(LanguageCode languageCode)
        {
            AppManager.I.AppSettingsManager.SetNativeLanguage(languageCode);
            RefreshSelection();
            HasPerformedSelection = true;
        }

        public void RefreshSelection()
        {
            foreach (var button in buttons)
            {
                if (button.LanguageCode == AppManager.I.AppSettings.NativeLanguage)
                    button.SetSelected();
                else
                    button.SetUnselected();
            }
        }

        private bool isOpen;
        public void Open()
        {
            gameObject.SetActive(true);
            isOpen = true;
        }

        public void Close()
        {
            gameObject.SetActive(false);
            isOpen = false;
        }

        public bool IsOpen()
        {
            return isOpen;
        }
    }
}
