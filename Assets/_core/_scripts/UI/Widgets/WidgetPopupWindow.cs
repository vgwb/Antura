using Antura.Audio;
using Antura.Core;
using Antura.Database;
using Antura.LivingLetters;
using System;
using Antura.Keeper;
using Antura.Language;
using TMPro;
using DG.DeExtensions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    /// <summary>
    /// Shows a general-purpose popup window.
    /// Can be used by minigames to show additional info to the player.
    /// </summary>
    public class WidgetPopupWindow : MonoBehaviour
    {
        public static WidgetPopupWindow I;

        public static bool IsShown { get; private set; }

        [Header("Options")]
        public bool timeIndependent = true;

        [Header("References")]
        public GameObject Window;

        public GameObject TitleGO;
        public GameObject DrawingImageGO;
        public GameObject WordTextGO;
        public GameObject MessageTextGO;
        public UIButton ButtonGO;
        public GameObject TutorialImageGO;
        public GameObject MarkOK;
        public GameObject MarkKO;
        public Sprite gameTimeUpSprite;
        public GameObject[] ActivateTheseOnAwake;

        private bool clicked;
        private Action currentCallback;
        private Tween showTween;

        void Awake()
        {
            I = this;

            if (ActivateTheseOnAwake != null)
            {
                foreach (GameObject go in ActivateTheseOnAwake)
                    go.SetActive(true);
            }

            showTween = this.GetComponent<RectTransform>().DOAnchorPosY(-800, 0.5f).From().SetUpdate(timeIndependent)
                .SetEase(Ease.OutBack).SetAutoKill(false).Pause()
                .OnPlay(() => this.gameObject.SetActive(true))
                .OnRewind(() => this.gameObject.SetActive(false))
                .OnComplete(() => ButtonGO.Pulse());

            this.gameObject.SetActive(false);
        }

        public void ResetContents()
        {
            clicked = false;
            TutorialImageGO.SetActive(false);
            SetTitle("", LanguageUse.Native);
            SetWord("", null);
            MarkOK.SetActive(false);
            MarkKO.SetActive(false);
            MessageTextGO.GetComponent<TextRender>().text = "";
        }

        public void Close(bool _immediate = false)
        {
            if (IsShown || _immediate)
            {
                Show(false, _immediate);
            }
        }

        public void Show(bool _doShow, bool _immediate = false)
        {
            GlobalUI.Init();

            IsShown = _doShow;
            if (_doShow)
            {
                clicked = false;
                if (_immediate)
                {
                    I.showTween.Complete();
                }
                else
                {
                    I.showTween.PlayForward();
                }
            }
            else
            {
                if (_immediate)
                {
                    I.showTween.Rewind();
                }
                else
                {
                    I.showTween.PlayBackwards();
                }
            }
        }


        public void ShowTextDirect(Action callback, string myText)
        {
            ResetContents();

            currentCallback = callback;
            ButtonGO.gameObject.SetActive(callback != null);

            TitleGO.GetComponent<TextRender>().SetTextUnfiltered(myText);

            Show(true);
        }

        public void SetButtonCallback(Action callback)
        {
            currentCallback = callback;
        }

        public void ShowSentence(Action callback, LocalizationDataId SentenceId)
        {
            ResetContents();

            currentCallback = callback;
            ButtonGO.gameObject.SetActive(callback != null);

            LocalizationData row = LocalizationManager.GetLocalizationData(SentenceId);
            TitleGO.GetComponent<TextRender>().SetText(row.LearningText, LanguageUse.Learning);

            KeeperManager.I.PlayDialogue(SentenceId);

            Show(true);
        }

        public void ShowSentence(Action callback, LocalizationDataId sentenceId, Sprite image2show)
        {
            ResetContents();

            currentCallback = callback;
            ButtonGO.gameObject.SetActive(callback != null);

            if (image2show != null)
            {
                TutorialImageGO.GetComponent<Image>().sprite = image2show;
                TutorialImageGO.SetActive(true);
            }

            TitleGO.GetComponent<TextRender>().SetText(LocalizationManager.GetLearning(sentenceId), LanguageUse.Learning);
            KeeperManager.I.PlayDialogue(sentenceId);

            Show(true);
        }

        public void ShowSentenceWithMark(Action callback, LocalizationDataId sentenceId, bool result, Sprite image2show)
        {
            ResetContents();

            currentCallback = callback;
            ButtonGO.gameObject.SetActive(callback != null);

            MarkOK.SetActive(result);
            MarkKO.SetActive(!result);

            if (image2show != null)
            {
                TutorialImageGO.GetComponent<Image>().sprite = image2show;
                TutorialImageGO.SetActive(true);
            }

            TitleGO.GetComponent<TextRender>().SetSentence(sentenceId);

            KeeperManager.I.PlayDialogue(sentenceId);

            Show(true);
        }

        public void SetMark(bool visible, bool ok)
        {
            MarkOK.SetActive(ok);
            MarkKO.SetActive(!ok);
        }

        public void SetImage(Sprite image2show)
        {
            if (image2show != null)
            {
                TutorialImageGO.GetComponent<Image>().sprite = image2show;
                TutorialImageGO.SetActive(true);
            }
            else
                TutorialImageGO.SetActive(false);
        }


        public void ShowSentenceAndWord(Action callback, LocalizationDataId SentenceId, LL_WordData wordData)
        {
            ResetContents();

            currentCallback = callback;
            ButtonGO.gameObject.SetActive(callback != null);

            TitleGO.GetComponent<TextRender>().SetSentence(SentenceId);

            //AudioManager.I.PlayDialog(SentenceId);

            SetWord(wordData.DrawingCharForLivingLetter, wordData);

            Show(true);
        }

        public void ShowSentenceAndWordWithMark(Action callback, LocalizationDataId SentenceId, LL_WordData wordData, bool result)
        {
            ResetContents();

            currentCallback = callback;
            ButtonGO.gameObject.SetActive(callback != null);

            MarkOK.SetActive(result);
            MarkKO.SetActive(!result);


            TitleGO.GetComponent<TextRender>().SetSentence(SentenceId);

            //AudioManager.I.PlayDialog(SentenceId);

            SetWord(wordData.DrawingCharForLivingLetter, wordData);

            Show(true);
        }

        public void ShowTimeUp(Action callback)
        {
            ShowSentence(callback, LocalizationDataId.Keeper_TimeUp, gameTimeUpSprite);
        }

        public void ShowTutorial(Action callback, Sprite tutorialImage)
        {
            ResetContents();

            currentCallback = callback;
            ButtonGO.gameObject.SetActive(callback != null);

            TutorialImageGO.GetComponent<Image>().sprite = tutorialImage;
            TutorialImageGO.SetActive(true);

            AudioManager.I.PlaySound(Sfx.UIPopup);
            Show(true);
        }

        public void SetTitle(string text, LanguageUse languageUse)
        {
            TitleGO.GetComponent<TextRender>().SetText(text, languageUse);
        }

        public void SetMessage(LocalizationDataId SentenceId)
        {
            MessageTextGO.GetComponent<TextRender>().SetSentence(SentenceId);
        }

        public void SetMessage(string text)
        {
            MessageTextGO.GetComponent<TextRender>().SetTextUnfiltered(text);
        }

        public void SetTitleSentence(LocalizationDataId SentenceId)
        {
            TitleGO.GetComponent<TextRender>().SetSentence(SentenceId);
        }

        public void SetWord(string imageId, LL_WordData wordData)
        {
            if (wordData == null)
            {
                WordTextGO.SetActive(false);
            }
            else
            {
                WordTextGO.SetActive(true);
                WordTextGO.GetComponent<TextRender>().SetLetterData(wordData);
            }

            if (!imageId.IsNullOrEmpty())
            {
                var imageData = new LL_ImageData(wordData.Id);
                DrawingImageGO.SetActive(true);
                DrawingImageGO.GetComponent<TextRender>().SetLetterData(imageData);
            }
            else
            {
                DrawingImageGO.SetActive(false);
            }
        }

        public void OnPressButtonPanel()
        {
            //Debug.Log("OnPressButtonPanel() " + clicked);
            OnPressButton();
        }

        public void OnPressButton()
        {
            //Debug.Log("OnPressButton() " + clicked);
            if (clicked)
            {
                return;
            }

            clicked = true;
            AudioManager.I.PlaySound(Sfx.UIButtonClick);

            if (currentCallback != null)
            {
                currentCallback();
            }
        }
    }
}
