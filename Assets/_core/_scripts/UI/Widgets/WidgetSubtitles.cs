using Antura.Core;
using DG.Tweening;
using System;
using System.Collections;
using Antura.Audio;
using Antura.Database;
using Antura.Keeper;
using Antura.Language;
using Antura.LivingLetters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    /// <summary>
    /// Shows a popup with text subtitles.
    /// Used by the Keeper.
    /// </summary>
    public class WidgetSubtitles : MonoBehaviour
    {
        public Color BgNoKeeperColor = Color.white;

        [Header("References")]
        public GameObject Background;

        public TextRender TextUI;
        public TextRender TextUItranslation;
        public WalkieTalkie WalkieTalkie;

        public static WidgetSubtitles I;

        private System.Action currentCallback;
        private int index;
        private Tween showTween, bgColorTween, textTween;

        void Awake()
        {
            I = this;
            WalkieTalkie.gameObject.SetActive(true);
            WalkieTalkie.Setup();

            showTween = DOTween.Sequence().SetUpdate(true).SetAutoKill(false).Pause()
                .Append(this.GetComponent<RectTransform>().DOAnchorPosY(170, 0.3f).From().SetEase(Ease.OutBack))
                .OnPlay(() => this.gameObject.SetActive(true))
                .OnRewind(() =>
                {
                    TextUI.text = "";
                    TextUItranslation.text = "";
                    this.gameObject.SetActive(false);
                });
            bgColorTween = Background.GetComponent<Image>().DOColor(BgNoKeeperColor, 0.3f).SetEase(Ease.Linear)
                .SetAutoKill(false).Pause();

            clearAndHide();
        }

        void OnDestroy()
        {
            if (I == this)
            { I = null; }
            this.StopAllCoroutines();
            showTween.Kill();
            bgColorTween.Kill();
            textTween.Kill();
        }

        public void DisplayDebug(string _sentence)
        {
            this.StopAllCoroutines();
            currentCallback = null;
            showTween.PlayForward();
            TextUI.text = _sentence;
        }

        /// <summary>
        /// Activate view elements if SentenceId != "" and display sentence.
        /// </summary>
        public void DisplayDialogue(LocalizationDataId _sentenceId, float fillPeriod = 2, bool _isKeeper = false, Action _callback = null)
        {
            var data = LocalizationManager.GetLocalizationData(_sentenceId);
            DisplayDialogue(data, fillPeriod, _isKeeper, _callback);
        }
        public void DisplayDialogue(LocalizationData data, float fillPeriod = 2, bool _isKeeper = false, Action _callback = null)
        {
            ShownData = null;
            var learningText = LocalizationManager.GetLearning(data.Id);
            var helpText = data.HelpText;
            DisplayText(learningText, helpText, fillPeriod, _isKeeper, _callback);
        }

        public void DisplayVocabularyData(ILivingLetterData data, float fillPeriod = 2, bool _isKeeper = false, Action _callback = null)
        {
            ShownData = data;
            var learningText = data.TextForLivingLetter;
            DisplayText(learningText, "", fillPeriod, _isKeeper, _callback);
        }

        private void DisplayText(string learningText, string helpText, float fillPeriod = 2, bool _isKeeper = false,
                Action _callback = null)
        {
            if (!AppManager.I.AppSettings.KeeperSubtitlesEnabled)
            {
                return;
            }

            GlobalUI.Init();
            StopAllCoroutines();
            currentCallback = _callback;
            showTween.PlayForward();
            if (_isKeeper)
            {
                bgColorTween.PlayBackwards();
            }
            else
            {
                bgColorTween.PlayForward();
            }

            if (!string.IsNullOrEmpty(learningText))
            {
                WalkieTalkie.Show(_isKeeper);
            }

            Display(learningText, helpText, fillPeriod);
        }

        void Display(string learningText, string helpText, float fillPeriod = 3)
        {
            this.StopAllCoroutines();
            textTween.Kill();
            TextUI.text = "";

            if (string.IsNullOrEmpty(learningText))
            {
                this.gameObject.SetActive(false);
                return;
            }

            this.gameObject.SetActive(true);
            if (WalkieTalkie.IsShown)
            { WalkieTalkie.Pulse(); }

            TextUI.SetText(learningText, LanguageUse.Learning);

            //string.IsNullOrEmpty(localizedText) ? data.Id : ReverseText(ArabicFixer.Fix(localizedText));
            if (AppManager.I.ContentEdition.LearnMethod.ShowKeeperTranslation)
            {
                TextUItranslation.SetText(helpText, LanguageUse.Help);
            }

            float lettersPerSecond = 20;
            fillPeriod = learningText.Length / lettersPerSecond;
            //Debug.Log("DisplayText() " + learningText.Length + " fillPeriod " + fillPeriod);

            StartCoroutine(DisplayTextCoroutine(fillPeriod));
        }

        public void Close(bool _immediate = false)
        {
            this.StopAllCoroutines();
            if (_immediate)
            {
                showTween.Rewind();
            }
            else
            {
                showTween.PlayBackwards();
            }
            WalkieTalkie.Show(false, _immediate);
        }

        void clearAndHide()
        {
            this.StopAllCoroutines();
            textTween.Kill();
            TextUI.text = "";
            TextUItranslation.text = "";
            this.gameObject.SetActive(false);
        }

        IEnumerator DisplayTextCoroutine(float fillPeriod)
        {
            yield return null; // Wait 1 frame otherwise TMP doesn't update characterCount

            var tmpro = TextUI.GetComponent<TextMeshProUGUI>();
            tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
            textTween = DOTween.To(() => tmpro.maxVisibleCharacters, x => tmpro.maxVisibleCharacters = x, 0, fillPeriod)
                               .From().SetUpdate(true).SetEase(Ease.Linear)
                               .OnComplete(() =>
                               {
                                   WalkieTalkie.StopPulse();
                                   if (currentCallback != null)
                                   {
                                       currentCallback();
                                   }
                               });
        }

        string ReverseText(string _text)
        {
            char[] cArray = _text.ToCharArray();
            string reverse = String.Empty;
            for (int i = cArray.Length - 1; i > -1; i--)
            {
                reverse += cArray[i];
            }
            return reverse;
        }

        #region Hint

        public ILivingLetterData ShownData;

        public void OnHintClicked()
        {
            if (AppManager.I.ContentEdition.LearnMethod.AllowSubtitleSkip)
            {
                AudioManager.I.SkipCurrentDialogue();
            }

            if (ShownData != null)
            {
                AudioManager.I.PlayVocabularyDataAudio(ShownData);
                Wobble();
            }
        }

        private void Wobble()
        {
            gameObject.transform.DOKill(true);
            gameObject.transform.DOShakeScale(0.5f);
        }

        #endregion

    }

}
