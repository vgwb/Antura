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
    public class DialogueBalloon : MonoBehaviour
    {
        #region Events

        public readonly ActionEvent OnBalloonClicked = new("DialogueBalloon.OnBalloonClicked");

        #endregion
        
        #region Serialized

        [Header("References")]
        [DeEmptyAlert]
        [SerializeField] Button bt;
        [DeEmptyAlert]
        [SerializeField] TextRender textRender;
        [DeEmptyAlert]
        [SerializeField] RectTransform icoContinue;

        #endregion

        QuestNode currNode;
        Tween showTween, icoContinueTween;
        
        #region Unity

        void Start()
        {
            showTween = this.transform.DOScale(0, 0.5f).From().SetAutoKill(false).Pause()
                .SetEase(Ease.OutBack)
                .OnComplete(() => {
                    bt.interactable = currNode.Type == HomerNode.NodeType.TEXT;
                })
                .OnRewind(() => {
                    icoContinueTween.Rewind();
                    this.gameObject.SetActive(false);
                });

            icoContinueTween = icoContinue.DOAnchorPosY(16, 0.75f).SetRelative().SetAutoKill(false).Pause()
                .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);

            bt.interactable = false;
            this.gameObject.SetActive(false);
            
            bt.onClick.AddListener(OnBalloonClicked.Dispatch);
        }

        void OnDestroy()
        {
            showTween.Kill();
            icoContinueTween.Kill();
        }

        void Update()
        {
            if (bt.interactable && Input.GetKeyDown(KeyCode.E)) OnBalloonClicked.Dispatch();
        }

        #endregion

        #region Public Methods

        public void Show(QuestNode node)
        {
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
        }

        public void Hide()
        {
            bt.interactable = false;
            showTween.timeScale = 2;
            showTween.PlayBackwards();
        }

        #endregion
    }
}