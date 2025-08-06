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
            if (Input.GetKeyDown(KeyCode.E) && bt.IsInteractable() && !UIManager.I.dialogues.IsPostcardOpen && InteractionManager.I.LastActionFrame != Time.frameCount)
            {
                OnBalloonClicked.Dispatch();
            }
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
            if (currNode.IsDialogueNode())
                btContinue.gameObject.SetActive(true);
            else
                btContinue.gameObject.SetActive(false);

            DisplayText(UseLearningLanguage);

            DiscoverNotifier.Game.OnShowDialogueBalloon.Dispatch(currNode);
            QuestManager.I.OnNodeStart(currNode);
        }

        public void DisplayText(bool UseLearningLanguage)
        {
            LanguageCode spokenLang;
            if (UseLearningLanguage)
            {
                textRender.SetText(currNode.Content, LanguageUse.Learning, Font2Use.UI);
                spokenLang = AppManager.I.ContentEdition.LearningLanguage;
            }
            else
            {
                textRender.SetText(currNode.ContentNative, LanguageUse.Native, Font2Use.Default);
                spokenLang = AppManager.I.AppSettings.NativeLanguage;
            }
            AudioManager.I.PlayDiscoverDialogue(
                currNode.AudioId,
                QuestManager.I.CurrentQuest.assetsFolder,
                spokenLang
            );
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
