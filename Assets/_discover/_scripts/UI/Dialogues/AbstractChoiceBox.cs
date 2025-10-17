using Antura.Audio;
using Antura.Core;
using Antura.Discover.Audio;
using Antura.Language;
using Antura.UI;
using System.Globalization;
using Demigiant.DemiTools;
using DG.DeInspektor.Attributes;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Discover
{
    public abstract class AbstractChoiceBox : MonoBehaviour
    {
        protected enum BoxType
        {
            Text,
            TextAndImage
        }
        
        #region EVENTS

        public readonly ActionEvent<AbstractChoiceBox> OnSelect = new("ChoiceBox.OnSelect");
        public readonly ActionEvent<AbstractChoiceBox> OnConfirm = new("ChoiceBox.OnConfirm");

        #endregion

        #region Serialized

        [SerializeField] protected float selectedScale = 1.2f;
        [SerializeField] protected float deselectedShift = 0; // Used by children
        [SerializeField] Color selectedNumberColor = Color.green;
        [Header("Exit Choice ColorBlock")]
        [SerializeField] ColorBlock exitChoiceColors = ColorBlock.defaultColorBlock;
        [Header("References")]
        [DeEmptyAlert]
        [SerializeField] protected Button btMain, btConfirm;
        [DeEmptyAlert]
        [SerializeField] protected RectTransform numbox;
        [DeEmptyAlert]
        [SerializeField] protected Image icoTranslation;
        [DeEmptyAlert]
        [SerializeField] TMP_Text tfNumber;
        [DeEmptyAlert]
        [SerializeField] TextRender textRender;

        [DeEmptyAlert]
        [SerializeField] protected RectTransform bg, box, confirmBg, confirmArrow;

        #endregion

        public bool IsShowingOrHiding { get { return showTween != null && showTween.IsPlaying(); } }
        public int Index { get; private set; }
        public RectTransform RT { get; private set; }

        protected virtual BoxType boxType { get; }
        public Vector2 DefPosition { get; private set; }
        public Vector2 DefSize { get; private set; }
        
        protected NodeChoice currChoice;
        bool useLearningLanguage = true;
        bool selected;
        bool confirmedForThisRound;
        ColorBlock defMainColorBlock;
        Sequence showTween, hoverTween, selectTween, confirmHoverTween, confirmTween;

        #region Unity

        void Awake()
        {
            RT = (RectTransform)this.transform;
            DefPosition = RT.anchoredPosition;
            DefSize = RT.sizeDelta;
            defMainColorBlock = btMain.colors;

            showTween = CreateShowTween().SetAutoKill(false).Pause();
            hoverTween = CreateHoverTween().SetAutoKill(false).Pause();
            selectTween = CreateSelectTween().SetAutoKill(false).Pause()
                .OnComplete(() =>
                {
                    if (!showTween.IsPlaying())
                        SetInteractable(true);
                })
                .OnRewind(() =>
                {
                    if (!confirmedForThisRound && !showTween.IsPlaying())
                        SetInteractable(true);
                    btConfirm.gameObject.SetActive(false);
                });
            confirmHoverTween = DOTween.Sequence().SetAutoKill(false).Pause()
                .Join(confirmArrow.DOAnchorPosX(10, 0.25f).SetRelative())
                .Join(confirmArrow.DOScale(1.25f, 0.25f))
                .Join(confirmBg.DOLocalRotate(new Vector3(0, 0, boxType == BoxType.Text ? 11.67f : -8), 0.25f).SetEase(Ease.OutBack));
            confirmTween = DOTween.Sequence().SetAutoKill(false).Pause()
                .Join(this.transform.DOScale(Vector3.one * 1.25f, 0.3f).SetEase(Ease.OutBack));

            btMain.onClick.AddListener(Select);
            btConfirm.onClick.AddListener(Confirm);
            useLearningLanguage = true;
        }

        void OnDestroy()
        {
            showTween.Kill();
            hoverTween.Kill();
            selectTween.Kill();
            confirmHoverTween.Kill();
            confirmTween.Kill();
        }

        #endregion

        #region Public Methods

        public void SetIndex(int indexToSet)
        {
            Index = indexToSet;
            tfNumber.text = (Index + 1).ToString(CultureInfo.InvariantCulture);
        }

        public void SetInteractable(bool interactable)
        {
            btMain.interactable = interactable;
            btConfirm.interactable = interactable;
        }

        public virtual void Show(NodeChoice choiceNode, bool isQuiz, bool doUseLearningLanguage)
        {
            useLearningLanguage = doUseLearningLanguage;
            numbox.gameObject.SetActive(isQuiz);
            btMain.colors = choiceNode.Highlight ? exitChoiceColors : defMainColorBlock;
            currChoice = choiceNode;
            confirmedForThisRound = false;
            confirmTween.Rewind();

            icoTranslation.gameObject.SetActive(QuestManager.I.HasTranslation);

            DisplayText(useLearningLanguage);
            showTween.Restart();
        }

        public void Hide()
        {
            Deselect();
            hoverTween.PlayBackwards();
            confirmHoverTween.PlayBackwards();
            showTween.PlayBackwards();
        }

        public void Deselect(float timeScale = 2)
        {
            if (!selected)
                return;

            selected = false;
            SetInteractable(false);
            selectTween.timeScale = timeScale;
            selectTween.PlayBackwards();
            confirmHoverTween.PlayBackwards();
        }

        public void MouseEnter()
        {
            hoverTween.timeScale = 1;
            hoverTween.PlayForward();
        }

        public void MouseExit()
        {
            hoverTween.timeScale = 2;
            hoverTween.PlayBackwards();
        }

        public void MouseEnterConfirm()
        {
            confirmHoverTween.PlayForward();
        }

        public void MouseExitConfirm()
        {
            confirmHoverTween.PlayBackwards();
        }

        #endregion

        #region Methods

        protected abstract Sequence CreateShowTween();
        protected abstract Sequence CreateSelectTween();
        protected abstract Sequence CreateHoverTween();

        private void Select()
        {
            DisplayText(useLearningLanguage, true);

            if (QuestManager.I.TalkToPlayerMode == TalkToPlayerMode.LearningThenNative
                        || QuestManager.I.TalkToPlayerMode == TalkToPlayerMode.NativeThenLearning)
            {
                // Debug.Log("Switching language");
                useLearningLanguage = !useLearningLanguage;
            }

            if (selected)
                return;

            selected = true;
            this.transform.SetAsLastSibling();
            SetInteractable(false);
            selectTween.timeScale = 1;
            selectTween.Restart();
            btConfirm.gameObject.SetActive(true);
            OnSelect.Dispatch(this);
        }

        private void DisplayText(bool UseLearningLanguage, bool playAudio = false)
        {

            if (UseLearningLanguage)
            {
                textRender.SetText(currChoice.Content, LanguageUse.Learning, Font2Use.UI);
                if (playAudio && currChoice.AudioLearning != null)
                {
                    DiscoverAudioManager.I.PlayDialogue(currChoice.AudioLearning);
                }
            }
            else
            {
                textRender.SetText(currChoice.ContentNative, LanguageUse.Native, Font2Use.Default);
                if (playAudio && currChoice.AudioNative != null)
                {
                    DiscoverAudioManager.I.PlayDialogue(currChoice.AudioNative);
                }
            }
        }

        void Confirm()
        {
            confirmedForThisRound = true;
            SetInteractable(false);
            Deselect(0.5f);
            confirmTween.Restart();
            OnConfirm.Dispatch(this);
        }

        #endregion
    }
}
