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

namespace Antura.Minigames.DiscoverCountry
{
    public abstract class AbstractDialogueBalloon : MonoBehaviour
    {
        #region Events

        public readonly ActionEvent OnBalloonClicked = new("DialogueBalloon.OnBalloonClicked");

        #endregion

        #region Serialized

        [Header("References")]
        [DeEmptyAlert]
        [SerializeField] protected Button bt;
        [DeEmptyAlert]
        [SerializeField] TextRender textRender;
        [DeEmptyAlert]
        [SerializeField] RectTransform icoContinue;

        #endregion

        public bool IsOpen { get; private set; }

        protected bool SpeechCycle = false;
        protected QuestNode currNode;
        protected Tween showTween, icoContinueTween;

        #region Unity

        void Start()
        {
            CreateShowTween();

            icoContinueTween = icoContinue.DOAnchorPosY(16, 0.75f).SetRelative().SetAutoKill(false).Pause()
                .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);

            bt.interactable = false;
            this.gameObject.SetActive(false);

            bt.onClick.AddListener(OnBalloonClicked.Dispatch);
            SpeechCycle = false;
        }

        void OnDestroy()
        {
            showTween.Kill();
            icoContinueTween.Kill();
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
            bt.interactable = false;
            textRender.SetText(node.Content);
            showTween.timeScale = 1;
            showTween.Restart();
            this.gameObject.SetActive(true);
            if (node.Type == HomerNode.NodeType.TEXT)
            {
                icoContinue.gameObject.SetActive(true);
                icoContinueTween.Restart();
            }
            else
            {
                icoContinue.gameObject.SetActive(false);
            }

            //Debug.Log("QUI PLAYO LocId: " + node.LocId);
            AudioManager.I.PlayDiscoverDialogue(
                 node.LocId,
                 SpeechCycle ? AppManager.I.AppSettings.NativeLanguage : AppManager.I.ContentEdition.LearningLanguage
            );
            SpeechCycle = !SpeechCycle;
        }

        public void Hide()
        {
            IsOpen = false;
            SpeechCycle = false;
            bt.interactable = false;
            showTween.timeScale = 2;
            showTween.PlayBackwards();
        }

        #endregion

        #region Methods

        protected abstract void CreateShowTween();

        #endregion
    }
}
