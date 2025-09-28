using Antura.Audio;
using Antura.Core;
using Antura.Language;
using Antura.UI;
using Antura.Discover.Interaction;
using Demigiant.DemiTools;
using DG.DeInspektor.Attributes;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Antura.Discover.Audio;

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
        #endregion

        public bool IsOpen { get; private set; }

        protected bool SpeechCycle = false;
        protected QuestNode currNode;
        protected Tween showTween;

        #region Unity

        void Start()
        {
            CreateShowTween();

            SetInteractable(false);
            this.gameObject.SetActive(false);

            bt.onClick.AddListener(OnBalloonClicked.Dispatch);
            btContinue.onClick.AddListener(OnBalloonContinueClicked.Dispatch);
            SpeechCycle = false;
        }

        void OnDestroy()
        {
            showTween.Kill();
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
                btContinue.gameObject.SetActive(true);
            else
                btContinue.gameObject.SetActive(false);

            if (QuestManager.I.HasTranslation)
            { iconTranslate.SetActive(true); }
            else
            { iconTranslate.SetActive(false); }

            DisplayText(UseLearningLanguage);

            DiscoverNotifier.Game.OnShowDialogueBalloon.Dispatch(currNode);
            QuestManager.I.OnNodeStart(currNode);
        }

        public void DisplayText(bool UseLearningLanguage)
        {
            // Debug.Log("Displaying dialogue in " + UseLearningLanguage + " : " + currNode.Content + " / " + currNode.ContentNative);
            if (UseLearningLanguage)
            {
                textRender.SetText(currNode.Content, LanguageUse.Learning, Font2Use.UI);
                if (currNode.AudioLearning != null)
                {
                    DiscoverAudioManager.I.PlayDialogue(currNode.AudioLearning);
                }
            }
            else
            {
                textRender.SetText(currNode.ContentNative, LanguageUse.Native, Font2Use.Default);
                if (currNode.AudioNative != null)
                {
                    DiscoverAudioManager.I.PlayDialogue(currNode.AudioNative);
                }
            }
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
        }

        #endregion

        #region Methods

        protected abstract void CreateShowTween();

        protected void SetInteractable(bool interactable)
        {
            bt.interactable = btContinue.interactable = interactable;
        }

        #endregion
    }
}
