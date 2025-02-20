using Antura.Core;
using Antura.Audio;
using Antura.UI;
using Demigiant.DemiTools;
using DG.DeInspektor.Attributes;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Antura.Minigames.DiscoverCountry.Interaction;
using Antura.Language;
using UnityEngine.EventSystems;

namespace Antura.Minigames.DiscoverCountry
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

        public void Show(QuestNode node)
        {
            if (IsOpen)
                return;

            IsOpen = true;
            currNode = node;
            SetInteractable(false);
            textRender.SetText(currNode.Content);
            showTween.timeScale = 1;
            showTween.Restart();
            this.gameObject.SetActive(true);
            if (currNode.IsDialogueNode())
                btContinue.gameObject.SetActive(true);
            else
                btContinue.gameObject.SetActive(false);

            if (currNode.Native)
                SpeechCycle = true;
            else
                SpeechCycle = false;

            var spokenLang = SpeechCycle ? AppManager.I.AppSettings.NativeLanguage : AppManager.I.ContentEdition.LearningLanguage;
            AudioManager.I.PlayDiscoverDialogue(
                node.AudioId,
                spokenLang
            );
            // Debug.Log("Show Dialogue: LocId: " + node.LocId);
            SpeechCycle = !SpeechCycle;
            DiscoverNotifier.Game.OnShowDialogueBalloon.Dispatch(currNode);
            QuestManager.I.OnNodeStart(currNode);
        }

        public void LocalizeText(LanguageUse languageUse)
        {
            if (languageUse == LanguageUse.Learning) textRender.SetText(currNode.Content);
            else textRender.SetText(currNode.ContentNative);
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
