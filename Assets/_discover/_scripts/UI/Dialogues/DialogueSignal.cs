using System;
using DG.DeInspektor.Attributes;
using DG.Tweening;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class DialogueSignal : MonoBehaviour
    {
        #region Serialized

        [SerializeField] Vector3 offsetFromLLAgentBase = new Vector3(0, 1.2f, 0);
        [SerializeField] Vector3 offsetFromInfoPointBase = new Vector3(0, 0.8f, 0);
        [SerializeField] bool animateIco;
        [Header("References")]
        [DeEmptyAlert]
        [SerializeField] GameObject balloon_talk, balloon_info_action;
        [DeEmptyAlert]
        [SerializeField] Transform iconContainer;
        [DeEmptyAlert]
        [SerializeField] GameObject ico_talk, ico_info, ico_action;

        #endregion

        Vector3 currOffset;
        Transform trans;
        Transform targetTrans;
        Tween showTween, loopTween;

        #region Unity

        void Awake()
        {
            trans = this.transform;

            showTween = this.transform.DOScale(0, 0.35f).From().SetAutoKill(false).Pause()
                .SetEase(Ease.OutCubic)
                .OnRewind(() =>
                {
                    loopTween.Rewind();
                    this.gameObject.SetActive(false);
                    targetTrans = null;
                });
            loopTween = iconContainer.DOScale(1.1f, 0.45f).From(Vector3.one * 0.9f).SetAutoKill(false).Pause()
                .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);

            this.gameObject.SetActive(false);
        }

        void OnDestroy()
        {
            showTween.Kill();
            loopTween.Kill();
        }

        void Update()
        {
            trans.position = targetTrans.position + currOffset;
        }

        #endregion

        #region Public Methods

        public void ShowFor(EdAgent agent)
        {
            SetAppearance(true);
            Show(agent.transform);

        }
        public void ShowFor(InfoPoint infoPoint)
        {
            SetAppearance(false);
            Show(infoPoint.transform);
        }

        public void Hide()
        {
            showTween.PlayBackwards();
        }

        #endregion

        #region Methods

        void Show(Transform target)
        {
            showTween.Restart();
            if (animateIco)
                loopTween.Restart();
            this.gameObject.SetActive(true);
            targetTrans = target;
        }

        void SetAppearance(bool isDialogue)
        {
            currOffset = isDialogue ? offsetFromLLAgentBase : offsetFromInfoPointBase;
            balloon_talk.gameObject.SetActive(isDialogue);
            ico_talk.gameObject.SetActive(isDialogue);
            balloon_info_action.gameObject.SetActive(!isDialogue);
            ico_info.gameObject.SetActive(!isDialogue);
            ico_action.gameObject.SetActive(false); // TODO implement when we have a distinction
        }

        #endregion
    }
}
