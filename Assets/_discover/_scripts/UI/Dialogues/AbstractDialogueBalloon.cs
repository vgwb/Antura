using Antura.Core;
using Antura.Audio;
using Antura.Homer;
using Antura.UI;
using Demigiant.DemiTools;
using DG.DeInspektor.Attributes;
using DG.Tweening;
using Homer;
using UnityEngine;
using UnityEngine.UI;
using Antura.Minigames.DiscoverCountry.Interaction;

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
            if (bt.interactable && Input.GetKeyDown(KeyCode.E))
                OnBalloonClicked.Dispatch();
        }

        #endregion

        #region Public Methods

        public void Show(QuestNode node)
        {
            IsOpen = true;
            currNode = node;
            SetInteractable(false);
            textRender.SetText(node.Content);
            showTween.timeScale = 1;
            showTween.Restart();
            this.gameObject.SetActive(true);
            if (node.IsDialogueNode)
            {
                btContinue.gameObject.SetActive(true);
            }
            else
            {
                btContinue.gameObject.SetActive(false);
            }

            //            Debug.Log("QUI PLAYO SpeechCycle: " + SpeechCycle);
            if (node.Native)
            {
                SpeechCycle = true;
            }
            Language.LanguageCode spokenLang = SpeechCycle ? AppManager.I.AppSettings.NativeLanguage : AppManager.I.ContentEdition.LearningLanguage;
            AudioManager.I.PlayDiscoverDialogue(
                node.LocId,
                spokenLang
            );
            SpeechCycle = !SpeechCycle;
            DiscoverNotifier.Game.OnShowDialogueBalloon.Dispatch(currNode);
            if (currNode.Action != null)
            {
                ActionManager.I.ResolveAction(currNode.Action);
            }
        }

        public void Hide()
        {
            IsOpen = false;
            SpeechCycle = false;
            SetInteractable(false);
            showTween.timeScale = 2;
            showTween.PlayBackwards();
            DiscoverNotifier.Game.OnCloseDialogueBalloon.Dispatch(currNode);
            if (currNode.NextTarget != null)
            {
                ActionManager.I.CameraShowTarget(currNode.NextTarget);
            }
            //            Debug.Log("ACTION POST: " + currNode.ActionPost);
            if (currNode.ActionPost != null)
            {
                ActionManager.I.ResolveAction(currNode.ActionPost);
            }

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
