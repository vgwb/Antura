using Antura.Core;
using ArabicSupport;
using DG.Tweening;
using System;
using System.Collections;
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

        public TextMeshProUGUI TextUI;
        public TextMeshProUGUI TextUItranslation;
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

            TextUI.isRightToLeftText = true;

            showTween = DOTween.Sequence().SetUpdate(true).SetAutoKill(false).Pause()
                .Append(this.GetComponent<RectTransform>().DOAnchorPosY(170, 0.3f).From().SetEase(Ease.OutBack))
                .OnPlay(() => this.gameObject.SetActive(true))
                .OnRewind(() => {
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
            if (I == this) { I = null; }
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
        public void DisplaySentence(Database.LocalizationDataId _sentenceId, float _duration = 2, bool _isKeeper = false,
            System.Action _callback = null)
        {
            var data = LocalizationManager.GetLocalizationData(_sentenceId);
            DisplaySentence(data, _duration, _isKeeper, _callback);
        }

        /// <summary>
        /// Activate view elements if SentenceId != "" and display sentence.
        /// </summary>
        public void DisplaySentence(Database.LocalizationData data, float _duration = 2, bool _isKeeper = false,
            System.Action _callback = null)
        {
            if (!AppManager.I.AppSettings.SubtitlesEnabled) {
                return;
            }

            GlobalUI.Init();
            this.StopAllCoroutines();
            currentCallback = _callback;
            showTween.PlayForward();
            if (_isKeeper) {
                bgColorTween.PlayBackwards();
            } else {
                bgColorTween.PlayForward();
            }
            WalkieTalkie.Show(_isKeeper);
            DisplayText(data, _duration);
        }

        // Overload - not used for now thus commented
        //        public void DisplaySentence(string[] _sentenceIdList, float _duration = 2, bool _isKeeper = false, System.Action _callback = null)
        //        {
        //            index = 0;
        //            DisplaySentence(_sentenceIdList[index], _duration, _isKeeper, _callback);
        //        }

        public void Close(bool _immediate = false)
        {
            this.StopAllCoroutines();
            if (_immediate) {
                showTween.Rewind();
            } else {
                showTween.PlayBackwards();
            }
            WalkieTalkie.Show(false, _immediate);
        }

        void DisplayText(Database.LocalizationData data, float _duration = 3)
        {
            this.StopAllCoroutines();
            textTween.Kill();
            TextUI.text = "";
            var localizedText = LocalizationManager.GetTranslation(data.Id);
            if (string.IsNullOrEmpty(localizedText)) {
                this.gameObject.SetActive(false);
                return;
            }

            this.gameObject.SetActive(true);
            if (WalkieTalkie.IsShown) { WalkieTalkie.Pulse(); }

            TextUI.text = string.IsNullOrEmpty(localizedText) ? data.Id : ReverseText(ArabicFixer.Fix(localizedText));
            if (AppManager.I.AppSettings.EnglishSubtitles && SAppConfig.I.ShowSubtitles) {
                TextUItranslation.text = string.IsNullOrEmpty(localizedText) ? data.Id : data.GetSubtitleTranslation();
            }
            this.StartCoroutine(DisplayTextCoroutine(_duration));

            //Debug.Log("DisplayText() " + data + " - " + data.English);
        }

        void clearAndHide()
        {
            this.StopAllCoroutines();
            textTween.Kill();
            TextUI.text = "";
            TextUItranslation.text = "";
            this.gameObject.SetActive(false);
        }

        IEnumerator DisplayTextCoroutine(float _duration)
        {
            yield return null; // Wait 1 frame otherwise TMP doesn't update characterCount

            TextUI.maxVisibleCharacters = TextUI.textInfo.characterCount;
            textTween = DOTween.To(() => TextUI.maxVisibleCharacters, x => TextUI.maxVisibleCharacters = x, 0, _duration)
                               .From().SetUpdate(true).SetEase(Ease.Linear)
                               .OnComplete(() => {
                                   WalkieTalkie.StopPulse();
                                   if (currentCallback != null) {
                                       currentCallback();
                                   }
                               });
        }

        string ReverseText(string _text)
        {
            char[] cArray = _text.ToCharArray();
            string reverse = String.Empty;
            for (int i = cArray.Length - 1; i > -1; i--) {
                reverse += cArray[i];
            }
            return reverse;
        }
    }
}