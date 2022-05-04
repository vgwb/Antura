using System;
using System.Collections;
using System.Collections.Generic;
using Antura.Audio;
using Antura.Core;
using Antura.Database;
using Antura.Language;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

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
                var key = LocalizationDataId.Language_MotherLanguage;
                AudioManager.I.PlayDialogue(LocalizationManager.GetLocalizationData(key), AvailableNativeCodes[nativeCodeIndex]);

                QuestionText.SetOverridenLanguageText(AvailableNativeCodes[nativeCodeIndex], key);

                nativeCodeIndex++;
                if (nativeCodeIndex >= AvailableNativeCodes.Count)
                    nativeCodeIndex = 0;
                yield return new WaitForSeconds(3f);
            }
        }

        public SelectNativeLanguageButton prefabButton;
        private List<SelectNativeLanguageButton> buttons = new List<SelectNativeLanguageButton>();

        private List<LanguageCode> AvailableNativeCodes = new List<LanguageCode>();

        public void OnEnable()
        {
            foreach (var button in buttons)
                Destroy(button.gameObject);
            buttons.Clear();

            // Find all supported native languages, and create a button for each
            for (var iContentEdition = 0; iContentEdition < AppManager.I.AppEdition.ContentEditions.Length; iContentEdition++)
            {
                var contentEditionConfig = AppManager.I.AppEdition.ContentEditions[iContentEdition];
                foreach (var supportedNativeLanguage in contentEditionConfig.SupportedNativeLanguages)
                {
                    if (AvailableNativeCodes.Contains(supportedNativeLanguage)) continue;
                    AvailableNativeCodes.Add(supportedNativeLanguage);
                }
            }

            AvailableNativeCodes.Sort((code1, code2) => AppManager.I.RootConfig.LanguageSorting.IndexOf(code1) - AppManager.I.RootConfig.LanguageSorting.IndexOf(code2));

            foreach (var langCode in AvailableNativeCodes)
            {
                var buttonGO = Instantiate(prefabButton.gameObject, prefabButton.transform.parent, true);
                buttonGO.transform.localScale = Vector3.one;
                buttonGO.SetActive(true);
                var button = buttonGO.GetComponent<SelectNativeLanguageButton>();
                button.Setup(langCode);
                buttons.Add(button);
            }

            prefabButton.gameObject.SetActive(false);

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

        public RectTransform scrollRectTr;
        private Color BGColor;
        public Image BG;

        private bool isOpen;
        public void Open()
        {
            scrollRectTr.anchoredPosition = new Vector2(1200, 0);
            scrollRectTr.DOAnchorPos(new Vector2(0, 0), 0.35f);
            if (BGColor == default) BGColor = BG.color;
            BG.color = new Color(BGColor.r, BGColor.g, BGColor.b, 0f);
            BG.DOColor(BGColor, 0.35f);
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
