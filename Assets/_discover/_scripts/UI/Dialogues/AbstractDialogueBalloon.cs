using Antura.Audio;
using Antura.Core;
using Antura.Discover.Audio;
using Antura.Language;
using Antura.UI;
using Demigiant.DemiTools;
using DG.DeInspektor.Attributes;
using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Discover
{
    public abstract class AbstractDialogueBalloon : MonoBehaviour
    {
        #region Events

        public readonly ActionEvent OnBalloonClicked = new("DialogueBalloon.OnBalloonClicked");
        public readonly ActionEvent OnBalloonContinueClicked = new("DialogueBalloon.OnBalloonContinueClicked");

        #endregion

        #region Serialized

        [Header("References")]
        [DeEmptyAlert]
        [SerializeField] protected Button bt;
        [DeEmptyAlert]
        [SerializeField] TextRender textRender;
        [DeEmptyAlert]
        [SerializeField] Button btContinue;

        [DeEmptyAlert]
        [SerializeField] GameObject iconTranslate;

        float continueHintDelay = 3f;
        float continueHintScaleMultiplier = 1.12f;
        float continueHintPulseDuration = 0.16f;
        float lettersPerSecond = 20;
        private Tween textTween;
        #endregion

        public bool IsOpen { get; private set; }

        protected bool SpeechCycle = false;
        protected QuestNode currNode;
        protected Tween showTween;
        Sequence continueHintTween;
        Vector3 continueDefaultScale;

        #region Unity

        void Start()
        {
            CreateShowTween();
            CreateContinueHintTween();

            SetInteractable(false);
            this.gameObject.SetActive(false);

            bt.onClick.AddListener(OnBalloonClicked.Dispatch);
            btContinue.onClick.AddListener(OnBalloonContinueClicked.Dispatch);
            SpeechCycle = false;
        }

        void OnDestroy()
        {
            showTween.Kill();
            continueHintTween?.Kill();
        }

        void Update()
        {
            // if (Input.GetKeyDown(KeyCode.E) && bt.IsInteractable() && !UIManager.I.dialogues.IsPostcardOpen && InteractionManager.I.LastActionFrame != Time.frameCount)
            // {
            //     OnBalloonClicked.Dispatch();
            // }
        }

        #endregion

        #region Public Methods

        public void Show(QuestNode node, bool UseLearningLanguage)
        {
            if (IsOpen)
                return;

            IsOpen = true;
            currNode = node;
            SetInteractable(false);
            showTween.timeScale = 1;
            showTween.Restart();
            this.gameObject.SetActive(true);
            if (currNode.IsDialogueNode() || currNode.IsPanel())
            {
                btContinue.gameObject.SetActive(true);
                RestartContinueHint();
            }
            else
            {
                btContinue.gameObject.SetActive(false);
                StopContinueHint();
            }

            if (QuestManager.I.HasTranslation)
            { iconTranslate.SetActive(true); }
            else
            { iconTranslate.SetActive(false); }

            DisplayText(UseLearningLanguage, automaticTranslation: QuestManager.I.HasTranslation);

            DiscoverNotifier.Game.OnShowDialogueBalloon.Dispatch(currNode);
            QuestManager.I.OnNodeStart(currNode);
        }

        public void DisplayText(bool UseLearningLanguage, bool automaticTranslation = false)
        {
            var textLength = 0;
            // Debug.Log("Displaying dialogue in " + UseLearningLanguage + " : " + currNode.Content + " / " + currNode.ContentNative);
            if (UseLearningLanguage)
            {
                textLength = currNode.Content.Length;
                textRender.SetText(currNode.Content, LanguageUse.Learning, Font2Use.UI);
                if (currNode.AudioLearning != null)
                {
                    if (automaticTranslation)
                    {
                        DiscoverAudioManager.I.PlayDialogue(currNode.AudioLearning, () => OnEndSpeaking(UseLearningLanguage));
                    }
                    else
                    {
                        DiscoverAudioManager.I.PlayDialogue(currNode.AudioLearning);
                    }
                }
            }
            else
            {
                textLength = currNode.ContentNative.Length;
                textRender.SetText(currNode.ContentNative, LanguageUse.Native, Font2Use.Default);
                if (currNode.AudioNative != null)
                {
                    if (automaticTranslation)
                    {
                        DiscoverAudioManager.I.PlayDialogue(currNode.AudioNative, () => OnEndSpeaking(UseLearningLanguage));
                    }
                    else
                    {
                        DiscoverAudioManager.I.PlayDialogue(currNode.AudioNative);
                    }

                }
            }
            //Debug.Log("DisplayText() " + learningText.Length + " fillPeriod " + fillPeriod);

            StartCoroutine(DisplayTextCoroutine(textLength / lettersPerSecond));
        }

        public void OnEndSpeaking(bool UseLearningLanguage)
        {
            DisplayText(!UseLearningLanguage, false);
        }

        IEnumerator DisplayTextCoroutine(float fillPeriod)
        {
            yield return null; // Wait 1 frame otherwise TMP doesn't update characterCount

            var tmpro = textRender.GetComponent<TextMeshProUGUI>();
            tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
            textTween = DOTween.To(() => tmpro.maxVisibleCharacters, x => tmpro.maxVisibleCharacters = x, 0, fillPeriod)
                               .From().SetUpdate(true).SetEase(Ease.Linear);
        }

        public void Hide()
        {
            if (!IsOpen)
                return;

            IsOpen = false;
            SpeechCycle = false;
            SetInteractable(false);
            showTween.timeScale = 2;
            showTween.PlayBackwards();
            DiscoverNotifier.Game.OnCloseDialogueBalloon.Dispatch(currNode);
            QuestManager.I.OnNodeEnd(currNode);
            StopContinueHint();
        }

        #endregion

        #region Methods

        protected abstract void CreateShowTween();

        protected void SetInteractable(bool interactable)
        {
            bt.interactable = btContinue.interactable = interactable;
        }

        void CreateContinueHintTween()
        {
            if (btContinue == null)
                return;

            var target = btContinue.transform;
            continueDefaultScale = target.localScale;

            continueHintTween = DOTween.Sequence()
                .SetAutoKill(false)
                .Pause()
                .SetUpdate(true);

            continueHintTween.AppendInterval(Mathf.Max(0f, continueHintDelay));
            AppendPulse(continueHintTween, target);
            continueHintTween.AppendInterval(Mathf.Max(0f, continueHintDelay));
            continueHintTween.SetLoops(-1, LoopType.Restart);
        }

        void AppendPulse(Sequence sequence, Transform target)
        {
            float duration = Mathf.Max(0.01f, continueHintPulseDuration);
            float mult = Mathf.Max(continueHintScaleMultiplier, 1f);

            sequence.Append(target.DOScale(continueDefaultScale * mult, duration).SetEase(Ease.OutQuad));
            sequence.Append(target.DOScale(continueDefaultScale, duration).SetEase(Ease.InQuad));
            sequence.Append(target.DOScale(continueDefaultScale * mult, duration).SetEase(Ease.OutQuad));
            sequence.Append(target.DOScale(continueDefaultScale, duration).SetEase(Ease.InQuad));
        }

        void RestartContinueHint()
        {
            if (continueHintTween == null)
                return;

            btContinue.transform.localScale = continueDefaultScale;
            continueHintTween.Restart();
        }

        void StopContinueHint()
        {
            if (continueHintTween == null)
                return;

            continueHintTween.Rewind();
            continueHintTween.Pause();
            btContinue.transform.localScale = continueDefaultScale;
        }

        #endregion
    }
}
