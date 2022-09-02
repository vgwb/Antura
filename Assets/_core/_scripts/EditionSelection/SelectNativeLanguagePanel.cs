using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Antura.Audio;
using Antura.Core;
using Antura.Database;
using Antura.Language;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace Antura.UI
{
    public class SelectNativeLanguagePanel : MonoBehaviour
    {
        public TextRender QuestionText;

        private Coroutine switchQuestionTextCO;
        public IEnumerator SwitchQuestionTextCO()
        {
            List<LanguageCode> sortedCodes = new List<LanguageCode>();

            if (AvailableNativeCodes.Contains(PreferredLanguage))
            {
                AvailableNativeCodes.Remove(PreferredLanguage);
                AvailableNativeCodes.Insert(0, PreferredLanguage);
            }

            sortedCodes.AddRange(AvailableNativeCodes);

            int nativeCodeIndex = 0;
            while (true)
            {
                var key = LocalizationDataId.Language_MotherLanguage;

                AudioManager.I.PlayDialogue(LocalizationManager.GetLocalizationData(key), sortedCodes[nativeCodeIndex]);

                QuestionText.SetOverridenLanguageText(sortedCodes[nativeCodeIndex], key);

                nativeCodeIndex++;
                if (nativeCodeIndex >= sortedCodes.Count)
                    nativeCodeIndex = 0;
                yield return new WaitForSeconds(3f);
            }
        }

        public SelectNativeLanguageButton prefabButton;
        private List<SelectNativeLanguageButton> buttons = new List<SelectNativeLanguageButton>();
        public SelectNativeLanguageButton ReselectNativeButton;

        private List<LanguageCode> AvailableNativeCodes = new List<LanguageCode>();
        private LanguageCode PreferredLanguage;

        public void OnEnable()
        {
            var scrollView = scrollRectTr.GetComponent<ScrollRect>();
            scrollView.content.transform.localPosition = Vector2.zero;

            foreach (var button in buttons)
                Destroy(button.gameObject);
            buttons.Clear();

            // Find all supported native languages, and create a button for each
            foreach (var lang in AppManager.I.AppEdition.SupportedNativeLanguages)
            {
                var l = lang;
                // HACK: Legacy is treated as arabic when selected as a language
                if (l == LanguageCode.arabic_legacy)
                    l = LanguageCode.arabic;
                if (AvailableNativeCodes.Contains(l))
                    continue;
                AvailableNativeCodes.Add(l);
            }

            for (var iContentEdition = 0; iContentEdition < AppManager.I.AppEdition.ContentEditions.Length; iContentEdition++)
            {
                var contentEditionConfig = AppManager.I.AppEdition.ContentEditions[iContentEdition];
                for (var index = 0; index < contentEditionConfig.OverridenNativeLanguages.Length; index++)
                {
                    var lang = contentEditionConfig.OverridenNativeLanguages[index];

                    // HACK: Legacy is treated as arabic when selected as a language
                    if (lang == LanguageCode.arabic_legacy)
                        lang = LanguageCode.arabic;

                    if (AvailableNativeCodes.Contains(lang))
                        continue;
                    AvailableNativeCodes.Add(lang);
                }
            }

            AvailableNativeCodes.Sort((code1, code2) => AppManager.I.RootConfig.LanguageSorting.IndexOf(code1) - AppManager.I.RootConfig.LanguageSorting.IndexOf(code2));

            PreferredLanguage = AppManager.I.AppSettingsManager.Settings.NativeLanguage;
            if (PreferredLanguage == LanguageCode.NONE && AppManager.I.AppEdition.DetectSystemLanguage)
            {
                LanguageCode systemNativeLanguage = LanguageCode.NONE;
                foreach (var lang in AvailableNativeCodes)
                {
                    if (string.Equals(lang.ToString(), Application.systemLanguage.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        systemNativeLanguage = lang;
                        break;
                    }
                }

                if (systemNativeLanguage != LanguageCode.NONE)
                {
                    PreferredLanguage = systemNativeLanguage;
                }
            }


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
            switchQuestionTextCO = StartCoroutine(SwitchQuestionTextCO());
        }

        public LanguageCode SelectedCode;
        public bool HasPerformedSelection;
        public SelectNativeLanguageButton SelectedButton;
        public void ConfirmSelection(LanguageCode languageCode)
        {
            SelectedCode = languageCode;
            if (SelectedCode == LanguageCode.arabic_legacy)
                SelectedCode = LanguageCode.arabic;

            StopCoroutine(switchQuestionTextCO);
            foreach (var btn in buttons)
            {
                var btnRT = btn.GetComponent<RectTransform>();
                if (btn.LanguageCode == languageCode)
                {
                    ReselectNativeButton.transform.position = btn.transform.position;
                    btn.transform.localScale = Vector3.zero;

                    btnRT = ReselectNativeButton.GetComponent<RectTransform>();
                    btnRT.sizeDelta = new Vector2(btnRT.sizeDelta.x, 200);
                    btnRT.DOAnchorPos(new Vector2(165, 70), 0.5f);
                    btnRT.DOSizeDelta(new Vector2(btnRT.sizeDelta.x, 100), 0.5f);
                }
                else
                {
                    btnRT.DOScale(new Vector2(0, 0), 0.25f);
                }

            }

            RefreshSelection();
            HasPerformedSelection = true;
        }

        public void RefreshSelection()
        {
            foreach (var button in buttons)
            {
                if (button.LanguageCode == SelectedCode)
                    button.SetSelected();
                else
                    button.SetUnselected();
            }
        }

        public RectTransform questionRectTr;
        public RectTransform scrollRectTr;
        private Color BGColor;
        public Image BG;

        private bool isOpen;
        public IEnumerator Open(bool firstTime)
        {
            gameObject.SetActive(true);
            isOpen = true;

            questionRectTr.anchoredPosition = new Vector2(0, 500);
            scrollRectTr.anchoredPosition = new Vector2(2200, scrollRectTr.anchoredPosition.y);

            if (BGColor == default)
                BGColor = BG.color;
            if (firstTime)
            {
                BG.color = BGColor;
                yield return new WaitForSeconds(1f); // Wait a bit for initialisation so the transition ends
            }
            else
            {
                BG.color = new Color(BGColor.r, BGColor.g, BGColor.b, 0f);
                BG.DOColor(BGColor, 0.35f);
            }

            questionRectTr.DOAnchorPos(new Vector2(0, 0), 0.35f);
            scrollRectTr.DOAnchorPos(new Vector2(0, scrollRectTr.anchoredPosition.y), 0.35f).OnComplete(() => ScrollTo(PreferredLanguage));
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

        public void ScrollTo(LanguageCode code)
        {
            var scrollView = scrollRectTr.GetComponent<ScrollRect>();
            var xDelta = 0f;
            var btn = buttons.FirstOrDefault(x => x.LanguageCode == code);
            if (btn == null)
                return;

            var offsetWidth = btn.GetComponent<RectTransform>().rect.width / 2f;
            if (btn.transform.position.x < 0)
            {
                xDelta = -btn.transform.position.x + offsetWidth;
            }
            else if (btn.transform.position.x > Screen.width - offsetWidth)
            {
                xDelta = (btn.transform.position.x - Screen.width) + offsetWidth;
            }
            //Debug.LogError("X pos " + btn.transform.position.x + " X DELTA: " + xDelta + " for preferred " + code);
            scrollView.content.transform.DOLocalMove(new Vector2(-xDelta, 0), 0.35f);
        }
    }
}
