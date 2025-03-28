﻿using System.Collections.Generic;
using System.Linq;
using Antura.Audio;
using Antura.Core;
using Antura.Database;
using Antura.Language;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    public class SelectLearningContentPanel : MonoBehaviour
    {
        public static void FindAllContentEditions(List<ContentConfig> supportedConfigs, LanguageCode nativeCode)
        {
            // Find all content editions with the current native language
            for (var iContentEdition = 0; iContentEdition < AppManager.I.AppEdition.ContentConfigs.Length; iContentEdition++)
            {
                var contentEditionConfig = AppManager.I.AppEdition.ContentConfigs[iContentEdition];

                bool isSupported = contentEditionConfig.SupportsLanguage(nativeCode);

                // HACK: For Arabic, we also show the Arabic_Legacy contents
                if (!isSupported && nativeCode == LanguageCode.arabic)
                {
                    isSupported = contentEditionConfig.OverridenNativeLanguages.Contains(LanguageCode.arabic_legacy);
                }
                if (!isSupported)
                    continue;

                supportedConfigs.Add(contentEditionConfig);
            }

            // Place as first
            var learnToReadConfig = supportedConfigs.FirstOrDefault(x => x.LearnMethod.Method == LearnMethod.LearnToRead);
            if (learnToReadConfig != null)
            {
                supportedConfigs.Remove(learnToReadConfig);
                supportedConfigs.Insert(0, learnToReadConfig);
            }
        }

        public TextRender QuestionText;
        public SelectNativeLanguageButton SelectNativeLanguageButton;

        public SelectLearningContentButton prefabButton;
        private List<SelectLearningContentButton> buttons = new List<SelectLearningContentButton>();

        private LearningContentID PreferredContentID;
        public LanguageCode SelectedNativeCode;
        public void OnEnable()
        {
            GetComponentInChildren<ScrollRect>().normalizedPosition = Vector2.zero;

            // HACK: arabic legacy should instead be arabic when entering here
            if (SelectedNativeCode == LanguageCode.arabic_legacy)
                SelectedNativeCode = LanguageCode.arabic;

            HasPerformedSelection = false;

            foreach (var button in buttons)
                Destroy(button.gameObject);
            buttons.Clear();

            PreferredContentID = AppManager.I.AppSettings.ContentID;

            var supportedConfigs = new List<ContentConfig>();
            FindAllContentEditions(supportedConfigs, SelectedNativeCode);

            foreach (ContentConfig contentEditionConfig in supportedConfigs)
            {
                var buttonGO = Instantiate(prefabButton.gameObject, prefabButton.transform.parent, true);
                buttonGO.transform.localScale = Vector3.one;
                buttonGO.SetActive(true);
                var button = buttonGO.GetComponent<SelectLearningContentButton>();
                button.Setup(contentEditionConfig);

                //checking if the lang name contains more than 2 words or if any word has more than 11 chars, in that case we decrease the font size so it fits in two lines
                var words = button.Title.text.Split(" ");
                var auxBtn = button.GetComponent<Button>();
                if (words.Length > 2)
                    auxBtn.GetComponentInChildren<TMP_Text>().fontSize = 24;
                else
                    for (int i = 0; i < words.Length; i++)
                        if (words[i].Length > 11)
                            auxBtn.GetComponentInChildren<TMP_Text>().fontSize = 24;

                buttons.Add(button);
            }

            prefabButton.gameObject.SetActive(false);

            var key = LocalizationDataId.Learn_What;
            AudioManager.I.PlayDialogue(LocalizationManager.GetLocalizationData(key), SelectedNativeCode);
            QuestionText.SetOverridenLanguageText(SelectedNativeCode, key);
        }

        public static void ConfirmNativeLanguage(LanguageCode nativeCode, LearningContentID contentId)
        {
            // Update data only after the selection is performed
            AppManager.I.AppSettingsManager.SetNativeLanguage(nativeCode);

            // HACK: if we are looking for arabic, but we need to actually use arabic_legacy, do so now
            // TODO: check this! Is it needed anymore?
            var content = AppManager.I.AppEdition.ContentConfigs.FirstOrDefault(x => x.ContentID == contentId);
            // if (nativeCode == LanguageCode.arabic && content.OverridenNativeLanguages.Contains(LanguageCode.arabic_legacy))
            // {
            //     AppManager.I.AppSettingsManager.SetNativeLanguage(LanguageCode.arabic_legacy);
            // }
        }

        public bool HasPerformedSelection;
        public SelectLearningContentButton SelectedButton;
        public void ConfirmSelection(LearningContentID contentId)
        {
            ConfirmNativeLanguage(SelectedNativeCode, contentId);

            AppManager.I.AppSettingsManager.SetLearningContentID(contentId);
            RefreshSelection();
            HasPerformedSelection = true;

            Overlay.enabled = true;
            Overlay.DOColor(new Color(1, 1, 1, 1), 0.35f);
        }

        public void RefreshSelection()
        {
            foreach (var button in buttons)
            {
                if (button.ContentId == AppManager.I.AppSettings.ContentID)
                    button.SetSelected();
                else
                    button.SetUnselected();
            }
        }

        public RectTransform questionRectTr;
        public RectTransform scrollRectTr;
        private Color BGColor;
        public Image BG;
        public Image Overlay;

        private bool isOpen;
        public void Open(bool scrollToLast)
        {
            gameObject.SetActive(true);
            isOpen = true;

            Overlay.color = new Color(1, 1, 1, 0);
            Overlay.enabled = false;

            questionRectTr.gameObject.SetActive(true);
            questionRectTr.anchoredPosition = new Vector2(0, 500);
            questionRectTr.DOAnchorPos(new Vector2(0, 0), 0.35f);

            scrollRectTr.anchoredPosition = new Vector2(2500, 0);
            scrollRectTr.DOAnchorPos(new Vector2(0, 0), 0.35f).SetDelay(0.5f).OnComplete(() =>
            {
                if (scrollToLast)
                    ScrollTo(PreferredContentID);
            });
            if (BGColor == default)
                BGColor = BG.color;
            BG.color = new Color(BGColor.r, BGColor.g, BGColor.b, 0f);
            BG.DOColor(BGColor, 0.35f);
        }

        public void Close()
        {
            questionRectTr.DOAnchorPos(new Vector2(0, 500), 0.35f);

            gameObject.SetActive(false);
            isOpen = false;
        }

        public bool IsOpen()
        {
            return isOpen;
        }

        public void ScrollTo(LearningContentID id)
        {
            var scrollView = scrollRectTr.GetComponent<ScrollRect>();
            var dir = Vector3.zero;
            var xDelta = 0f;
            var btn = buttons.FirstOrDefault(x => x.ContentId == id);
            if (btn == null)
                return;

            if (btn.transform.position.x < 0)
            {
                xDelta = -btn.transform.position.x + btn.GetComponent<RectTransform>().rect.x;
                dir.x = 1;
            }
            else if (btn.transform.position.x > Screen.width)
            {
                xDelta = btn.transform.position.x - Screen.width - btn.GetComponent<RectTransform>().rect.x;
                dir.x = -1;
            }
            scrollView.content.transform.DOLocalMove(new Vector2(-xDelta, 0), 0.35f);
        }
    }
}
